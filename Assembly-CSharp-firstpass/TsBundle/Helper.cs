using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace TsBundle
{
	public class Helper
	{
		public static string WebFileBundleStackName = "WebFileBundleGroup";

		internal static string FullURL(string assetPath)
		{
			return Option.GetProtocolRootPath((!Option.localWWW) ? Protocol.HTTP : Protocol.FILE) + assetPath;
		}

		private static bool _CheckUrl(string url)
		{
			string text = string.Copy(Path.GetExtension(url));
			text.ToLower();
			return !(text == "unity3d") && !(text == "assetbundle") && !(text == "xml");
		}

		public static bool RequestDownloadWebFile(string url, bool unloadAfterPostProcess, PostProcPerItem downlaodCompleteCallbak, object callbackParam)
		{
			if (!Helper._CheckUrl(url))
			{
				return false;
			}
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(url, Helper.WebFileBundleStackName, true);
			if (wWWItem == null)
			{
				return false;
			}
			wWWItem.SetItemType(ItemType.USER_BYTESA);
			wWWItem.SetCallback(downlaodCompleteCallbak, callbackParam);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, unloadAfterPostProcess);
			return true;
		}

		public static bool RequestDownloadWebFile(string url, bool unloadAfterPostProcess, PostProcPerItem downlaodCompleteCallbak)
		{
			return Helper.RequestDownloadWebFile(url, unloadAfterPostProcess, downlaodCompleteCallbak, null);
		}

		private static void _PreDownloadRequest(string assetPath, ref List<WWWItem> reqPreloadList, bool skipDuplicationCheck, object param)
		{
			if (reqPreloadList == null)
			{
				TsLog.LogWarning("[PreDownalod] reqPreloadList param is null!", new object[0]);
			}
			else
			{
				WWWItem wWWItem;
				if (skipDuplicationCheck)
				{
					wWWItem = Holder.TryGetOrCreateBundle(assetPath, Option.undefinedStackName);
				}
				else
				{
					wWWItem = Holder.GetPreDownloadBundle(assetPath, ItemType.UNDEFINED);
				}
				if (wWWItem == null)
				{
					if (Option.EnableTrace)
					{
						TsLog.Log("[PreDownload] _PreDownloadRequest( AssetPath=\"{0}\", Type={1} ) => already created WWWItem.", new object[]
						{
							assetPath,
							ItemType.UNDEFINED
						});
					}
				}
				else
				{
					if (Option.EnablePreDownloadHistory)
					{
						Helper._WriteToHistory(wWWItem);
					}
					wWWItem.SetCallback(new PostProcPerItem(Helper.DisposeDownloadedWWW), param);
					reqPreloadList.Add(wWWItem);
				}
			}
		}

		public static WWWProgress PreDownloadRequestSkipCheckDuplication(IEnumerable<string> urls, PostProcPerList callback, object param)
		{
			if (!NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				WWWProgress wWWProgress = new WWWProgress(1f);
				wWWProgress.AddCompletionCnt();
				return wWWProgress;
			}
			List<WWWItem> wiList = new List<WWWItem>();
			foreach (string current in urls)
			{
				Helper._PreDownloadRequest(current, ref wiList, true, param);
			}
			return TsImmortal.bundleService.RequestDownloadCoroutine(wiList, DownGroup.BGLOAD, true, callback, param);
		}

		private static void DisposeDownloadedWWW(IDownloadedItem obj, object param)
		{
			IDonwloadProgress donwloadProgress = param as IDonwloadProgress;
			if (donwloadProgress != null)
			{
				donwloadProgress.IncProgress();
			}
			WWWItem wWWItem = obj as WWWItem;
			if (wWWItem != null)
			{
				wWWItem.Dispose();
			}
		}

		public static WWWProgress PreDownloadRequest(IEnumerable<string> urls, PostProcPerList callback, object param)
		{
			if (!NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				WWWProgress wWWProgress = new WWWProgress(1f);
				wWWProgress.AddCompletionCnt();
				return wWWProgress;
			}
			List<WWWItem> wiList = new List<WWWItem>();
			foreach (string current in urls)
			{
				Helper._PreDownloadRequest(current, ref wiList, false, param);
			}
			return TsImmortal.bundleService.RequestDownloadCoroutine(wiList, DownGroup.BGLOAD, true, callback, param);
		}

		private static void _WriteToHistory(WWWItem item)
		{
			if (!Application.isEditor)
			{
				TsLog.LogWarning("[PreDownload] Pre-Download history => WebPlayer is not supported", new object[0]);
				return;
			}
			string path = string.Format("{0}/../PreDownload_History.txt", Application.dataPath);
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(path, true, Encoding.UTF8))
				{
					DateTime utcNow = DateTime.UtcNow;
					streamWriter.WriteLine("({6}-{7}-{8}) [{3}:{4}:{5}] Type={0}, Stack=\"{1}\", Path=\"{2}\"", new object[]
					{
						item.itemType,
						item.stackName,
						item.assetPath,
						utcNow.Hour,
						utcNow.Minute,
						utcNow.Second,
						utcNow.Year,
						utcNow.Month,
						utcNow.Day
					});
				}
			}
			catch (Exception ex)
			{
				TsLog.LogWarning("[PreDownlaod] ", new object[]
				{
					ex
				});
			}
		}

		public static void REQ_ASSETBUNDLE(string resPath, PostProcPerItem callback, object obj)
		{
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(resPath, null);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(callback, obj);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, false);
		}
	}
}
