using Ndoors.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TsPatch;
using UnityEngine;

namespace TsBundle
{
	internal class BackgroundInstall : IDisposable
	{
		internal class Progress
		{
			public int total;

			public int count;

			internal bool downloading;

			public Progress(int total)
			{
				this.total = total;
				this.count = 0;
			}

			public Progress(int total, int count)
			{
				this.total = total;
				this.count = count;
			}
		}

		private class DownloadAssets : IDonwloadProgress
		{
			public string listFileName;

			public List<string> requestFiles = new List<string>();

			public PreDownloader.DownlaodToComplete userCompleteCallback;

			public DownloadAssets(string listFileName)
			{
				this.listFileName = listFileName;
			}

			public void IncProgress()
			{
				BackgroundInstall.Progress progress;
				if (BackgroundInstall.ms_ProgressCollection.TryGetValue(this.listFileName, out progress))
				{
					progress.count++;
				}
			}
		}

		private class CachingReportPack
		{
			public string listFileName;

			public bool fullLog;

			public StringBuilder targetLog;

			public PreDownloader.CachingReport callback;

			public CachingReportPack(string listFileName, bool fullLog, StringBuilder targetLog, PreDownloader.CachingReport callback)
			{
				this.listFileName = listFileName;
				this.fullLog = fullLog;
				this.targetLog = targetLog;
				this.callback = callback;
			}
		}

		private const string SUB_FOLDER = "PreDownload/";

		internal static Dictionary<string, BackgroundInstall.Progress> ms_ProgressCollection = new Dictionary<string, BackgroundInstall.Progress>();

		private StreamWriter m_StreamWriter;

		internal bool _IsCached(string listFileName, bool fullLog, StringBuilder targetLog, PreDownloader.CachingReport callback)
		{
			if (Path.HasExtension(listFileName))
			{
				WWWItem wWWItem = Holder.TryGetOrCreateBundle(listFileName, Option.undefinedStackName);
				if (wWWItem != null)
				{
					wWWItem.SetCallback(new PostProcPerItem(this._DownloadComplete_ListFile_ForReporting), new BackgroundInstall.CachingReportPack(listFileName, fullLog, targetLog, callback));
					wWWItem.SetItemType(ItemType.USER_STRING);
					TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
					return true;
				}
				TsLog.LogWarning("[PreDownload] Request( \"{0}\" ) => cannot read file from AssetBundle.", new object[]
				{
					listFileName
				});
				return false;
			}
			else
			{
				TextAsset textAsset = ResourceCache.LoadFromResourcesImmediate(string.Format("{0}{1}", "PreDownload/", listFileName)) as TextAsset;
				if (textAsset)
				{
					float cachedRate = this._CachingReport(listFileName, textAsset.text, fullLog, targetLog);
					if (callback != null)
					{
						callback(listFileName, cachedRate, targetLog);
					}
					return true;
				}
				if (targetLog != null)
				{
					targetLog.AppendFormat("{0} => error (refresh asset database, please.)", listFileName);
					targetLog.AppendLine();
				}
				TsLog.LogWarning("[PreDownload] IsCached( \"{0}\" ) => cannot read file from resources.", new object[]
				{
					listFileName
				});
				return false;
			}
		}

		private void _DownloadComplete_ListFile_ForReporting(IDownloadedItem item, object param)
		{
			if (!item.isSuccess || !item.canAccessString)
			{
				TsLog.LogWarning("[PreDownload] IsCached( \"{0}\" ) => download failed.", new object[]
				{
					item.assetPath
				});
				return;
			}
			string safeString = item.safeString;
			BackgroundInstall.CachingReportPack cachingReportPack = param as BackgroundInstall.CachingReportPack;
			if (cachingReportPack != null)
			{
				float cachedRate = this._CachingReport(item.assetPath, safeString, cachingReportPack.fullLog, cachingReportPack.targetLog);
				if (cachingReportPack.callback != null)
				{
					cachingReportPack.callback(item.assetPath, cachedRate, cachingReportPack.targetLog);
				}
			}
		}

		private float _CachingReport(string listFileName, string assetListText, bool fullLog, StringBuilder targetLog)
		{
			int num = 0;
			Queue<KeyValuePair<string, int>> queue = new Queue<KeyValuePair<string, int>>();
			Queue<KeyValuePair<string, int>> queue2 = new Queue<KeyValuePair<string, int>>();
			Queue<string> queue3 = new Queue<string>();
			if (targetLog != null)
			{
				targetLog.Length = 0;
				targetLog.AppendLine("==================================================================================================");
			}
			foreach (string current in this._ParseBundleList(assetListText))
			{
				PatchFileInfo patchFileInfo = PatchFinalList.Instance.GetPatchFileInfo(current);
				if (patchFileInfo.nVersion != -1)
				{
					if (this._IsVersionCached(current, patchFileInfo.nVersion, patchFileInfo.bUseCustomCache))
					{
						queue.Enqueue(new KeyValuePair<string, int>(current, patchFileInfo.nVersion));
					}
					else
					{
						queue2.Enqueue(new KeyValuePair<string, int>(current, patchFileInfo.nVersion));
					}
				}
				else
				{
					queue3.Enqueue(current);
					if (Option.useCache)
					{
						TsLog.LogWarning("[PreDownload] IsCached( \"{0}\" ) : {1} => not listed in FinalPathList", new object[]
						{
							listFileName,
							current
						});
					}
				}
				num++;
			}
			int count = queue.Count;
			int count2 = queue2.Count;
			if (targetLog != null)
			{
				if (fullLog)
				{
					while (queue.Count > 0)
					{
						KeyValuePair<string, int> keyValuePair = queue.Dequeue();
						string key = keyValuePair.Key;
						int value = keyValuePair.Value;
						targetLog.AppendFormat("   \"{0}\" (version={1}) => OK (cached)", key, value);
						targetLog.AppendLine();
					}
				}
				while (queue2.Count > 0)
				{
					KeyValuePair<string, int> keyValuePair2 = queue2.Dequeue();
					string key2 = keyValuePair2.Key;
					int value2 = keyValuePair2.Value;
					targetLog.AppendFormat("   \"{0}\" (version={1}) => not cached", key2, value2);
					targetLog.AppendLine();
				}
				while (queue3.Count > 0)
				{
					string arg = queue3.Dequeue();
					targetLog.AppendFormat("   \"{0}\" => error, not listed in FinalPatchList", arg);
					targetLog.AppendLine();
				}
				targetLog.AppendLine("==================================================================================================");
				targetLog.AppendFormat("Total = {0}, Cached = {1}, Non-Cached = {2}\r\n", num, count, count2);
				targetLog.AppendFormat("Cached rate = {0}% ({1}/{2})", 100f * (float)count / (float)num, count, num);
				targetLog.AppendLine();
			}
			if (BackgroundInstall.ms_ProgressCollection.ContainsKey(listFileName))
			{
				BackgroundInstall.ms_ProgressCollection[listFileName].count = count;
			}
			else
			{
				BackgroundInstall.ms_ProgressCollection.Add(listFileName, new BackgroundInstall.Progress(num, count));
			}
			return (count != num) ? ((float)count / (float)num) : 1f;
		}

		internal bool _RequestPreDownload(string listFileName, PreDownloader.DownlaodToComplete callback)
		{
			if (!Option.EnablePreDownload)
			{
				TsLog.LogWarning("[PreDownload] Request( \"{0}\" ) => Disalbe Pre-Download.", new object[]
				{
					listFileName
				});
				return false;
			}
			BackgroundInstall.Progress progress;
			if (BackgroundInstall.ms_ProgressCollection.TryGetValue(listFileName, out progress) && progress.downloading)
			{
				TsLog.LogWarning("[PreDownload] Request( \"{0}\" ) => alreay request pre-download.", new object[]
				{
					listFileName
				});
				return false;
			}
			if (Path.HasExtension(listFileName))
			{
				WWWItem wWWItem = Holder.TryGetOrCreateBundle(listFileName, Option.undefinedStackName);
				if (wWWItem == null)
				{
					TsLog.LogWarning("[PreDownload] Request( \"{0}\" ) => cannot read file from AssetBundle.", new object[]
					{
						listFileName
					});
					return false;
				}
				wWWItem.SetCallback(new PostProcPerItem(this._DownloadComplete_ListFile), callback);
				wWWItem.SetItemType(ItemType.USER_STRING);
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			}
			else
			{
				TextAsset textAsset = ResourceCache.LoadFromResourcesImmediate(string.Format("PreDownload/{0}", listFileName)) as TextAsset;
				if (!textAsset)
				{
					TsLog.LogWarning("[PreDownload] Request( \"{0}\" ) => cannot read file from resources.", new object[]
					{
						listFileName
					});
					return false;
				}
				this._DownloadAssets(listFileName, textAsset.text, callback);
			}
			return true;
		}

		private void _DownloadComplete_ListFile(IDownloadedItem item, object param)
		{
			if (!item.isSuccess || !item.canAccessString)
			{
				TsLog.LogWarning("[PreDownload] Request( \"{0}\" ) => AssetBundle download failed.", new object[]
				{
					item.assetPath
				});
				return;
			}
			this._DownloadAssets(item.assetPath, item.safeString, param as PreDownloader.DownlaodToComplete);
		}

		private void _DownloadAssets(string listFileName, string text, PreDownloader.DownlaodToComplete userCompleteCallback)
		{
			int num = 0;
			BackgroundInstall.DownloadAssets downloadAssets = new BackgroundInstall.DownloadAssets(listFileName);
			foreach (string current in this._ParseBundleList(text))
			{
				PatchFileInfo patchFileInfo = PatchFinalList.Instance.GetPatchFileInfo(current);
				if (this._IsVersionCached(current, patchFileInfo.nVersion, patchFileInfo.bUseCustomCache))
				{
					num++;
				}
				else
				{
					downloadAssets.requestFiles.Add(current);
				}
			}
			int num2 = downloadAssets.requestFiles.Count + num;
			BackgroundInstall.Progress progress = new BackgroundInstall.Progress(num2, num);
			BackgroundInstall.ms_ProgressCollection[listFileName] = progress;
			if (downloadAssets.requestFiles.Count > 0)
			{
				progress.downloading = true;
				if (Option.EnableTrace)
				{
					TsLog.Log("[PreDownload] Request( \"{0}\" ) => wait for downloads (Requests={1}, Cacheds={2}, Total={3})", new object[]
					{
						listFileName,
						downloadAssets.requestFiles.Count,
						num,
						num2
					});
				}
				downloadAssets.userCompleteCallback = userCompleteCallback;
				Helper.PreDownloadRequest(downloadAssets.requestFiles, new PostProcPerList(this._DownloadComplete_Assets), downloadAssets);
			}
			else
			{
				if (Option.EnableTrace)
				{
					TsLog.Log("[PreDownload] Request( \"{0}\" ) => Complete to download: All file is chached on disk. (Progress={1}%, CachedFiles={2}, Total={3})", new object[]
					{
						listFileName,
						(float)num / (float)num2 * 100f,
						num,
						num2
					});
				}
				if (userCompleteCallback != null)
				{
					userCompleteCallback(listFileName, (num != num2) ? ((float)num / (float)num2) : 1f);
				}
			}
		}

		private void _DownloadComplete_Assets(List<WWWItem> downlaodList, object param)
		{
			BackgroundInstall.DownloadAssets downloadAssets = param as BackgroundInstall.DownloadAssets;
			if (downloadAssets == null)
			{
				TsLog.LogWarning("[PreDownload] download to complete ( <<unknown>>, downloads = {0} )", new object[]
				{
					downlaodList.Count
				});
				return;
			}
			int count = downloadAssets.requestFiles.Count;
			int num = 0;
			StringBuilder stringBuilder = (!Option.EnableTrace) ? null : new StringBuilder(1024);
			if (stringBuilder != null)
			{
				stringBuilder.AppendFormat("[PreDownload] Download to complete ( PreDownloadListFile=\"{0}\", Requests={1}, Downloads={2} )", downloadAssets.listFileName, downloadAssets.requestFiles.Count, downlaodList.Count);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("==================================================================================================");
				Dictionary<string, WWWItem> dictionary = new Dictionary<string, WWWItem>(downlaodList.Count);
				foreach (WWWItem current in downlaodList)
				{
					dictionary[current.assetPath] = current;
				}
				foreach (string current2 in downloadAssets.requestFiles)
				{
					string text = (!dictionary.ContainsKey(current2)) ? "<<skipped>>" : "<<download>>";
					PatchFileInfo patchFileInfo = PatchFinalList.Instance.GetPatchFileInfo(current2);
					if (patchFileInfo.nVersion != -1)
					{
						if (this._IsVersionCached(current2, patchFileInfo.nVersion, patchFileInfo.bUseCustomCache))
						{
							num++;
							stringBuilder.AppendFormat("   \"{0}\" (version={1}, {2}) => chached", current2, patchFileInfo.nVersion, text);
							stringBuilder.AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("   \"{0}\" (version={1}, {2}) => download to complete, but bundle-file is not cached.", current2, patchFileInfo.nVersion, text);
							stringBuilder.AppendLine();
						}
					}
					else
					{
						stringBuilder.AppendFormat("   \"{0}\" ({1}) => not listed FinalPatchLIst", current2, text);
						stringBuilder.AppendLine();
					}
				}
				stringBuilder.AppendLine("==================================================================================================");
				stringBuilder.AppendFormat("Total = {0}, Downloads = {1}, Cached = {2}\r\n", count, downlaodList.Count, num);
				stringBuilder.AppendFormat("Downloads Rate = {0} %\r\n", 100f * (float)downlaodList.Count / (float)count);
				stringBuilder.AppendFormat("Cached Rate = {0} %\r\n", 100f * (float)num / (float)count);
				TsLog.Log(stringBuilder.ToString(), new object[0]);
			}
			float cachedRate = (count != 0) ? ((num != count) ? ((float)num / (float)count) : 1f) : 0f;
			BackgroundInstall.Progress progress;
			if (BackgroundInstall.ms_ProgressCollection.TryGetValue(downloadAssets.listFileName, out progress))
			{
				progress.count = progress.total;
				progress.downloading = false;
			}
			if (downloadAssets.userCompleteCallback != null)
			{
				downloadAssets.userCompleteCallback(downloadAssets.listFileName, cachedRate);
			}
		}

		[DebuggerHidden]
		private IEnumerable<string> _ParseBundleList(string bundleListText)
		{
			BackgroundInstall.<_ParseBundleList>c__Iterator20 <_ParseBundleList>c__Iterator = new BackgroundInstall.<_ParseBundleList>c__Iterator20();
			<_ParseBundleList>c__Iterator.bundleListText = bundleListText;
			<_ParseBundleList>c__Iterator.<$>bundleListText = bundleListText;
			BackgroundInstall.<_ParseBundleList>c__Iterator20 expr_15 = <_ParseBundleList>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		~BackgroundInstall()
		{
			this.StopRecord();
		}

		public void StartRecord(string listFileName)
		{
			string path = string.Format("{0}/Resources/{1}{2}.txt", Application.dataPath, "PreDownload/", listFileName);
			try
			{
				this.m_StreamWriter = new StreamWriter(path, false, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				this.m_StreamWriter = null;
				TsLog.LogWarning("[PreDownlaod] StartRecord( \"{0}\" ) => failed : {1}", new object[]
				{
					listFileName,
					ex.ToString()
				});
			}
		}

		public void StopRecord()
		{
			if (this.m_StreamWriter != null)
			{
				this.m_StreamWriter.Dispose();
			}
			this.m_StreamWriter = null;
		}

		public void RecordFile(string bundleFilePath)
		{
			if (this.m_StreamWriter == null)
			{
				TsLog.LogWarning("[PreDownload] RecordFile( \"{0}\" ) => failed, does not call StartRecord()", new object[]
				{
					bundleFilePath
				});
				return;
			}
			if (Option.useCache)
			{
				PatchFileInfo patchFileInfo = PatchFinalList.Instance.GetPatchFileInfo(bundleFilePath);
				if (patchFileInfo != null && patchFileInfo.nVersion != -1)
				{
					int nFileSize = patchFileInfo.nFileSize;
					this.m_StreamWriter.Write(string.Format("{0} \t\t; ({1:#,###,###,###} bytes)", bundleFilePath, nFileSize));
					this.m_StreamWriter.WriteLine();
				}
				else
				{
					this.m_StreamWriter.Write(";");
					this.m_StreamWriter.Write(bundleFilePath);
					this.m_StreamWriter.WriteLine(" \t\t; <<not listed in FinalPatchList>>");
				}
			}
			else
			{
				this.m_StreamWriter.Write(string.Format("{0} \t\t; (unknown size) => Retry recording after useCache is enabled.", bundleFilePath));
				this.m_StreamWriter.WriteLine();
			}
		}

		public void Dispose()
		{
			this.StopRecord();
		}

		private bool _IsVersionCached(string assetPath, int version, bool forceUseCustomCache)
		{
			return (Option.VerifyLoadedBundle && Holder.IsLoadedBundle(assetPath)) || TsCaching.IsVersionCached(Helper.FullURL(assetPath), version, forceUseCustomCache);
		}
	}
}
