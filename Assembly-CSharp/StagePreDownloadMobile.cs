using GameMessage.Private;
using ICSharpCode.SharpZipLib.Zip;
using Ndoors.Framework.Stage;
using StageHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TsBundle;
using TsLibs;
using TsPatch;
using UnityEngine;
using UnityForms;

public class StagePreDownloadMobile : AStage
{
	private class ZipFileInfo
	{
		public string fileName = string.Empty;

		public int fileSize;

		public string szMD5 = string.Empty;
	}

	private const string FirstFilename = "final/mobile_first.patchlist.txt";

	private const string FinalFilename = "final/mobile_final.patchlist.txt";

	private static int UNITY_CACHE_SIZE = 500;

	private static long LIMIT_CAPACITY = 600L;

	private Mobile_PreDownloadDlg PreDownloadDlg;

	private WWWProgress wProgressRate;

	private static string m_strLocalFilePath = string.Empty;

	private int m_nServerVersion = -1;

	private int m_nLocalVersion = -1;

	private int m_nDelCacheVersion = -1;

	private bool m_bExistCompressionFile;

	private bool m_bPreDownLoad;

	private bool m_bWifiCheck;

	private int m_nTotalFileSize;

	private bool _bPreDownloadStart;

	private List<string> _ReDownloadRequest = new List<string>();

	private Dictionary<string, string> _DownLoadList = new Dictionary<string, string>();

	private int m_nRequestCount;

	private bool m_bCompleteRePreDownload;

	private int _CompressCount;

	private int _DownloadZipFileTotalSize;

	private int _DownloadZipFileSize;

	private List<StagePreDownloadMobile.ZipFileInfo> m_CompressionFileInfo = new List<StagePreDownloadMobile.ZipFileInfo>();

	private List<StagePreDownloadMobile.ZipFileInfo> m_CompleteCompressionFileInfo = new List<StagePreDownloadMobile.ZipFileInfo>();

	private static byte[] m_UnzipBuffer = StagePreDownloadMobile._GetUnzipBuffer();

	public override Scene.Type SceneType()
	{
		return Scene.Type.PREDOWNLOAD;
	}

	public override void OnPrepareSceneChange()
	{
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		NrLoadPageScreen.StepUpMain(1);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.LOGINRATING_DLG);
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PREDOWNLOAD_DLG);
		NmMainFrameWork.DeleteImage();
	}

	public override void OnEnter()
	{
		string text = string.Format("{0} OnEnter OnEnter Memory = {1}MB", base.GetType().FullName, NrTSingleton<NrMainSystem>.Instance.AppMemory);
		TsPlatform.FileLog(text);
		TsLog.LogWarning(text, new object[0]);
		TsLog.Log("====== {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		Scene.ChangeSceneType(this.SceneType());
		NrMainSystem.CheckAndSetReLoginMainCamera();
		Camera.main.gameObject.AddComponent<DefaultCameraController>();
		TsCaching.useCustomCacheOnly = true;
		base.StartTaskSerial(CommonTasks.InitializeChangeScene());
		base.StartTaskSerial(this._RequestPredownloadTables());
		base.StartTaskSerial(this._CheckRePredownload(1));
		base.StartTaskSerial(this._CheckRePredownload(2));
		base.StartTaskSerial(this._CheckRePredownload(3));
		base.StartTaskSerial(this._DownloadAssetBundles());
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(true));
		base.StartTaskPararell(this._PredownloadProgressUpdate());
	}

	public override void OnExit()
	{
		TsLog.Log("====== {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
		NrLoadPageScreen.ShowHideLoadingImg(true);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PREDOWNLOAD_DLG);
		Option.usePatchDir = false;
	}

	protected override void OnUpdateAfterStagePrework()
	{
	}

	[DebuggerHidden]
	private IEnumerator _CheckRePredownload(int retryCnt)
	{
		StagePreDownloadMobile.<_CheckRePredownload>c__Iterator3D <_CheckRePredownload>c__Iterator3D = new StagePreDownloadMobile.<_CheckRePredownload>c__Iterator3D();
		<_CheckRePredownload>c__Iterator3D.retryCnt = retryCnt;
		<_CheckRePredownload>c__Iterator3D.<$>retryCnt = retryCnt;
		<_CheckRePredownload>c__Iterator3D.<>f__this = this;
		return <_CheckRePredownload>c__Iterator3D;
	}

	[DebuggerHidden]
	private IEnumerator _RequestPredownloadTables()
	{
		StagePreDownloadMobile.<_RequestPredownloadTables>c__Iterator3E <_RequestPredownloadTables>c__Iterator3E = new StagePreDownloadMobile.<_RequestPredownloadTables>c__Iterator3E();
		<_RequestPredownloadTables>c__Iterator3E.<>f__this = this;
		return <_RequestPredownloadTables>c__Iterator3E;
	}

	[DebuggerHidden]
	private IEnumerator _DownloadAssetBundles()
	{
		StagePreDownloadMobile.<_DownloadAssetBundles>c__Iterator3F <_DownloadAssetBundles>c__Iterator3F = new StagePreDownloadMobile.<_DownloadAssetBundles>c__Iterator3F();
		<_DownloadAssetBundles>c__Iterator3F.<>f__this = this;
		return <_DownloadAssetBundles>c__Iterator3F;
	}

	[DebuggerHidden]
	private IEnumerator _PredownloadProgressUpdate()
	{
		StagePreDownloadMobile.<_PredownloadProgressUpdate>c__Iterator40 <_PredownloadProgressUpdate>c__Iterator = new StagePreDownloadMobile.<_PredownloadProgressUpdate>c__Iterator40();
		<_PredownloadProgressUpdate>c__Iterator.<>f__this = this;
		return <_PredownloadProgressUpdate>c__Iterator;
	}

	public static void SetPlatformPath()
	{
		string text = string.Empty;
		string text2 = string.Empty;
		if (TsPlatform.IsEditor)
		{
			text2 = string.Format("{0}/Mobile/", NrTSingleton<NrGlobalReference>.Instance.basePath);
		}
		else if (TsPlatform.IsIPhone)
		{
			text2 = string.Format("{0}/at2/cacheroot/", TsPlatform.Operator.GetFileDir());
		}
		else if (TsPlatform.IsAndroid)
		{
			text2 = string.Format("{0}/cacheroot/", TsPlatform.Operator.GetFileDir());
		}
		else
		{
			TsLog.Assert(false, "Unknown Platform", new object[0]);
		}
		StagePreDownloadMobile.m_strLocalFilePath = text2;
		Option.SetProtocolRootPath(Protocol.FILE, text2);
		text = Option.GetProtocolRootPath(Protocol.HTTP);
		TsLog.LogWarning("HttpPath[{0}] Cachepath[{1}]", new object[]
		{
			text,
			text2
		});
		TsPlatform.FileLog("HttpPath = " + text + ", Cachepath = " + text2);
		NrTSingleton<NrGlobalReference>.Instance.localWWW = false;
		NrTSingleton<NrGlobalReference>.Instance.useCache = true;
		TsCaching.InitiailzeCustomCaching(text, text2);
		Caching.maximumAvailableDiskSpace = (long)(1048576 * StagePreDownloadMobile.UNITY_CACHE_SIZE);
		if (TsPlatform.IsAndroid)
		{
			NrTSingleton<NrGlobalReference>.Instance.STR_MOBILE_VER = TsPlatform.APP_VERSION_AND;
		}
		else if (TsPlatform.IsIPhone || TsPlatform.IsIPad())
		{
			NrTSingleton<NrGlobalReference>.Instance.STR_MOBILE_VER = TsPlatform.APP_VERSION_IOS;
		}
	}

	private void _FirstPatchListComplete(IDownloadedItem wItem, object obj)
	{
		TsPlatform.FileLog("Add _FirstPatchListComplete File : ");
		if (this.PreDownloadDlg == null)
		{
			NrLoadPageScreen.ShowHideLoadingImg(false);
			this.PreDownloadDlg = (Mobile_PreDownloadDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PREDOWNLOAD_DLG);
		}
		if (wItem.canAccessString)
		{
			using (TsDataReader tsDataReader = new TsDataReader())
			{
				if (tsDataReader.LoadFrom(wItem.safeString))
				{
					int num = -1;
					string empty = string.Empty;
					tsDataReader.ReadKeyData("PatchVersion", out this.m_nServerVersion);
					tsDataReader.ReadKeyData("AppVersion", out empty);
					char[] separator = new char[]
					{
						'.'
					};
					string[] array = empty.Split(separator);
					string[] array2 = null;
					if (TsPlatform.IsAndroid)
					{
						array2 = TsPlatform.APP_VERSION_AND.Split(separator);
					}
					else if (TsPlatform.IsIPhone)
					{
						array2 = TsPlatform.APP_VERSION_IOS.Split(separator);
					}
					int nLocal = -1;
					int nServer = -1;
					bool flag = false;
					for (int i = 0; i < array.Length; i++)
					{
						int.TryParse(array[i], out nServer);
						int.TryParse(array2[i], out nLocal);
						flag = this._CheckVersion(nLocal, nServer);
						if (flag)
						{
							break;
						}
					}
					UnityEngine.Debug.LogWarning(" !!!!!!! nLocalVersion = " + nLocal.ToString() + ", nServerVersion = " + nServer.ToString());
					UnityEngine.Debug.LogWarning(" !!!!!!! bAppUpdate = " + flag.ToString());
					if (flag)
					{
						MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
						string textFromPreloadText = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("2");
						string textFromPreloadText2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("1");
						msgBoxUI.SetMsg(new YesDelegate(StagePreDownloadMobile.On_OK_URL), null, textFromPreloadText, textFromPreloadText2, eMsgType.MB_OK);
						int num2 = (int)(GUICamera.width / 2f - msgBoxUI.GetSize().x / 2f);
						int num3 = (int)(GUICamera.height / 2f - msgBoxUI.GetSize().y / 2f);
						msgBoxUI.SetLocation((float)num2, (float)num3, 50f);
						return;
					}
					UnityEngine.Debug.LogWarning(" !!!!!!! m_bExistCompressionFile = " + this.m_bExistCompressionFile.ToString());
					if (this.m_bExistCompressionFile)
					{
						bool flag2 = false;
						if (num != this.m_nDelCacheVersion)
						{
							this.m_nDelCacheVersion = num;
							TsLog.Log("@@@@  local=[{0}]  server=[{1}]", new object[]
							{
								this.m_nDelCacheVersion.ToString(),
								num.ToString()
							});
							flag2 = true;
						}
						if (this.m_nServerVersion == this.m_nLocalVersion)
						{
							this.m_bPreDownLoad = true;
							if (flag2)
							{
								this.SaveCompleteCompressUnZipFile(true);
							}
							return;
						}
					}
				}
			}
			TsPlatform.FileLog("Add _FirstPatchListComplete File : ");
			if (this.m_nServerVersion > this.m_nLocalVersion && this.m_nLocalVersion > 0)
			{
				TsPlatform.FileLog("Zip File ServerVersion > LocalVersion Clear All CompleteList");
				this.m_CompleteCompressionFileInfo.Clear();
			}
			this._SaveCompressList(wItem.safeString);
			if (this.m_bPreDownLoad && this.m_CompressionFileInfo.Count > 0)
			{
				this.m_bPreDownLoad = false;
			}
			TsLog.LogError("1111111111", new object[0]);
			base.StartTaskPararell(CommonTasks.DownloadStringXML("final/mobile_final.patchlist.txt", new PostProcPerItem(this._FinalPatchListComplete)));
		}
	}

	private void _FinalPatchListComplete(IDownloadedItem wItem, object obj)
	{
		TsLog.LogError("_FinalPatchListComplete", new object[0]);
		if (wItem.canAccessString)
		{
			using (TsDataReader tsDataReader = new TsDataReader())
			{
				if (tsDataReader.LoadFrom(wItem.safeString))
				{
					if (tsDataReader.BeginSection("[FinalList2]"))
					{
						StringBuilder stringBuilder = new StringBuilder(512);
						StringBuilder stringBuilder2 = new StringBuilder(512);
						StringBuilder stringBuilder3 = new StringBuilder(512);
						while (!tsDataReader.IsEndOfSection())
						{
							TsDataReader.Row currentRow = tsDataReader.GetCurrentRow();
							if (currentRow.LineType == TsDataReader.Row.TYPE.LINE_DATA)
							{
								stringBuilder.Length = 0;
								stringBuilder.Append(currentRow.GetColumn(0));
								stringBuilder = PatchFinalList.ReplaceWord(stringBuilder, false);
								if (stringBuilder[0] == '?')
								{
									stringBuilder.Remove(0, 1);
									stringBuilder3.Length = 0;
									stringBuilder3.Append(stringBuilder.ToString());
								}
								else
								{
									stringBuilder2.Length = 0;
									stringBuilder2.AppendFormat("{0}/{1}", stringBuilder3, stringBuilder);
									string text = stringBuilder2.ToString().Remove(0, 1);
									if (!text.ToLower().Contains(".unity3d"))
									{
										int nVersion = 0;
										int.TryParse(currentRow.GetToken(1), out nVersion);
										CustomCaching.AddCacheList(text, nVersion);
									}
								}
							}
							tsDataReader.NextLine();
						}
					}
					if (!TsPlatform.IsEditor)
					{
						if (TsPlatform.Operator.GetSDCardCapacity() < StagePreDownloadMobile.LIMIT_CAPACITY)
						{
							IntroMsgBoxDlg introMsgBoxDlg = (IntroMsgBoxDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INTROMSGBOX_DLG);
							if (introMsgBoxDlg != null)
							{
								string textFromPreloadText = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("6");
								introMsgBoxDlg.SetBtnChangeName(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"));
								introMsgBoxDlg.SetMsg(new Action<IntroMsgBoxDlg, object>(this._OnMessageBoxCancel), null, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"), textFromPreloadText, eMsgType.MB_OK);
							}
						}
						else if (!TsPlatform.Operator.IsWifiConnect())
						{
							IntroMsgBoxDlg introMsgBoxDlg2 = (IntroMsgBoxDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INTROMSGBOX_DLG);
							if (introMsgBoxDlg2 != null)
							{
								string textFromPreloadText2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("5");
								introMsgBoxDlg2.SetBtnChangeName(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"), NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("9"));
								introMsgBoxDlg2.SetMsg(new Action<IntroMsgBoxDlg, object>(this._OnMessageBoxZipOk), null, new Action<IntroMsgBoxDlg, object>(this._OnMessageBoxCancel), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"), textFromPreloadText2, eMsgType.MB_OK_CANCEL);
							}
						}
						else
						{
							this._DownLoadZipFiles();
							this.PreDownloadDlg = (Mobile_PreDownloadDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PREDOWNLOAD_DLG);
						}
					}
				}
			}
		}
	}

	private void _OnZipFileCompleteDownload(IDownloadedItem wItem, object obj)
	{
		if (wItem.canAccessBytes)
		{
			TsLog.LogError("_OnZipFileCompleteDownload " + wItem.assetPath, new object[0]);
			string text = wItem.assetPath.ToLower().Replace("package/", string.Empty);
			text = text.Replace(".zip", string.Empty);
			string strLocalFilePath = StagePreDownloadMobile.m_strLocalFilePath;
			string mD = PatchFinalList.Instance.GetMD5(wItem.safeBytes, text);
			if (this.m_CompressionFileInfo[this._CompressCount].szMD5 != mD)
			{
				GC.Collect();
				StagePreDownloadMobile.ZipFileInfo zipFileInfo = this.m_CompressionFileInfo[this._CompressCount];
				WWWItem wWWItem = Holder.TryGetOrCreateBundle(zipFileInfo.fileName, Option.defaultStackName);
				wWWItem.SetItemType(ItemType.USER_BYTESA);
				wWWItem.SetCallback(new PostProcPerItem(this._OnZipFileCompleteDownload), null);
				this.wProgressRate = TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
				return;
			}
			if (!TsPlatform.IsEditor)
			{
				if (NrGlobalReference.strLangType.Equals("eng"))
				{
					TsPlatform.Operator.ShowProgreesDlg("Patch file is being extracted.");
				}
				else
				{
					TsPlatform.Operator.ShowProgreesDlg("패치 파일을 풀고 있습니다.");
				}
			}
			if (!this._UnZipFiles(wItem.safeBytes, strLocalFilePath))
			{
				TsLog.LogError("Filed~!! UnZipe!!", new object[0]);
			}
			GC.Collect();
			if (!TsPlatform.IsEditor)
			{
				TsPlatform.Operator.DestroyProgreesDlg();
			}
			if (this._CompressCount == this.m_CompressionFileInfo.Count - 1)
			{
				CustomCaching.SaveCacheList();
				bool flag = true;
				for (int i = 0; i < this.m_CompleteCompressionFileInfo.Count; i++)
				{
					if (this.m_CompleteCompressionFileInfo[i].fileName == this.m_CompressionFileInfo[this._CompressCount].fileName)
					{
						this.m_CompleteCompressionFileInfo[i].fileSize = this.m_CompressionFileInfo[this._CompressCount].fileSize;
						this.m_CompleteCompressionFileInfo[i].szMD5 = this.m_CompressionFileInfo[this._CompressCount].szMD5;
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.m_CompleteCompressionFileInfo.Add(this.m_CompressionFileInfo[this._CompressCount]);
				}
				this.SaveCompleteCompressUnZipFile(true);
				this.m_bPreDownLoad = true;
				this.wProgressRate = null;
			}
			else
			{
				bool flag2 = true;
				for (int j = 0; j < this.m_CompleteCompressionFileInfo.Count; j++)
				{
					if (this.m_CompleteCompressionFileInfo[j].fileName == this.m_CompressionFileInfo[this._CompressCount].fileName)
					{
						this.m_CompleteCompressionFileInfo[j].fileSize = this.m_CompressionFileInfo[this._CompressCount].fileSize;
						this.m_CompleteCompressionFileInfo[j].szMD5 = this.m_CompressionFileInfo[this._CompressCount].szMD5;
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					this.m_CompleteCompressionFileInfo.Add(this.m_CompressionFileInfo[this._CompressCount]);
				}
				this.SaveCompleteCompressUnZipFile(false);
				StagePreDownloadMobile.ZipFileInfo zipFileInfo2 = this.m_CompressionFileInfo[++this._CompressCount];
				this._DownloadZipFileSize += zipFileInfo2.fileSize;
				WWWItem wWWItem2 = Holder.TryGetOrCreateBundle(zipFileInfo2.fileName, Option.defaultStackName);
				wWWItem2.SetItemType(ItemType.USER_BYTESA);
				wWWItem2.SetCallback(new PostProcPerItem(this._OnZipFileCompleteDownload), null);
				this.wProgressRate = TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem2, DownGroup.RUNTIME, true);
			}
		}
	}

	private void _OnDownloadedAllUnZip(List<WWWItem> wiList, object obj)
	{
	}

	public void _OnCompleteDownload(List<WWWItem> wList, object obj)
	{
		TsCaching.SaveCacheList();
		string text = Option.GetProtocolRootPath(Protocol.FILE);
		if (TsPlatform.IsEditor)
		{
			text = text.Remove(0, 7);
		}
		else
		{
			text = text.Remove(0, 6);
		}
		this.m_nTotalFileSize = 0;
		this._ReDownloadRequest.Clear();
		foreach (KeyValuePair<string, PatchFileInfo> current in PatchFinalList.Instance.FilesList)
		{
			string text2 = current.Key.Remove(0, 1);
			string text3 = text + text2;
			if (!File.Exists(text3))
			{
				this.m_nTotalFileSize += current.Value.nFileSize;
				this._ReDownloadRequest.Add(text2);
				if (!this._DownLoadList.ContainsKey(text2))
				{
					this._DownLoadList.Add(text2, current.Value.szMD5);
				}
				TsPlatform.FileLog("Download fail = " + text2);
			}
			else if (this._DownLoadList.ContainsKey(text2))
			{
				string mD = PatchFinalList.Instance.GetMD5(text3);
				if (current.Value.szMD5.ToLower() != mD.ToLower())
				{
					TsPlatform.FileLog("Hash Not Same : " + text2);
					TsPlatform.FileLog("data=" + current.Value.szMD5.ToLower() + ", file=" + mD.ToLower());
					this.m_nTotalFileSize += current.Value.nFileSize;
					this._ReDownloadRequest.Add(text2);
				}
				else
				{
					this._DownLoadList.Remove(text2);
				}
			}
		}
		if (this._ReDownloadRequest.Count == 0)
		{
			this._DownLoadList.Clear();
			TsAudio.RefreshAllAudioVolumes();
			if (SystemInfo.processorCount == 1)
			{
				TsAudio.SetDisableDownloadAudio(EAudioType.AMBIENT, true);
				TsAudio.SetDisableDownloadAudio(EAudioType.ENVIRONMENT, true);
			}
			FacadeHandler.EndPreDownload();
			GC.Collect();
		}
		this.m_nRequestCount++;
		this.m_bCompleteRePreDownload = false;
	}

	private bool _CheckVersion(int nLocal, int nServer)
	{
		return nLocal < nServer;
	}

	private void _OnMessageBoxConnectOK(IntroMsgBoxDlg a_cthis, object a_oObject)
	{
		TsPlatform.BuildPackage buildPackage = TsPlatform.GetBuildPackage();
		if (buildPackage == TsPlatform.BuildPackage.GooglePlay)
		{
			if (buildPackage == TsPlatform.BuildPackage.GooglePlayWifi)
			{
				TsPlatform.Operator.ConnectGooglePlay("market://details?id=google.ndoors.sampoomwifi");
			}
			else
			{
				TsPlatform.Operator.ConnectGooglePlay("market://details?id=google.ndoors.sampoom");
			}
		}
		else if (buildPackage == TsPlatform.BuildPackage.TStore)
		{
			Application.OpenURL("http://tsto.re/0000300585");
		}
		else if (buildPackage == TsPlatform.BuildPackage.AppStore)
		{
			Application.OpenURL("https://itunes.apple.com/kr/app/samgugjileul-pumda/id553092782?mt=8");
		}
	}

	private void _OnMessageBoxOk(IntroMsgBoxDlg a_cthis, object a_oObject)
	{
		List<string> list = a_oObject as List<string>;
		if (list != null)
		{
			this.wProgressRate = Helper.PreDownloadRequest(list, new PostProcPerList(this._OnCompleteDownload), null);
			this._bPreDownloadStart = true;
		}
		else
		{
			TsLog.LogError("_OnMessageBoxOk() downloadList is null~!!", new object[0]);
		}
	}

	private void _OnMessageBoxCancel(IntroMsgBoxDlg a_cthis, object a_oObject)
	{
		NrTSingleton<NrMainSystem>.Instance.QuitGame();
	}

	private void _OnMessageBoxZipOk(IntroMsgBoxDlg a_cthis, object a_oObject)
	{
		this.m_bWifiCheck = true;
		this._DownLoadZipFiles();
	}

	private void _SaveCompressList(string strContext)
	{
		using (TsDataReader tsDataReader = new TsDataReader())
		{
			if (tsDataReader.LoadFrom(strContext) && tsDataReader.BeginSection("[FirstList]"))
			{
				while (!tsDataReader.IsEndOfSection())
				{
					TsDataReader.Row currentRow = tsDataReader.GetCurrentRow();
					string token = currentRow.GetToken(0);
					if (!token.Contains("null") && token.Length > 0)
					{
						string token2 = currentRow.GetToken(1);
						int num = Convert.ToInt32(token2);
						string token3 = currentRow.GetToken(2);
						bool flag = true;
						for (int i = 0; i < this.m_CompleteCompressionFileInfo.Count; i++)
						{
							if (this.m_CompleteCompressionFileInfo[i].fileName == token && this.m_CompleteCompressionFileInfo[i].fileSize == num && this.m_CompleteCompressionFileInfo[i].szMD5 == token3)
							{
								flag = false;
								TsPlatform.FileLog("Pass Zip File : " + token);
								break;
							}
						}
						if (flag)
						{
							StagePreDownloadMobile.ZipFileInfo zipFileInfo = new StagePreDownloadMobile.ZipFileInfo();
							zipFileInfo.fileName = token;
							zipFileInfo.fileSize = num;
							zipFileInfo.szMD5 = token3;
							this._DownloadZipFileTotalSize += num;
							this.m_CompressionFileInfo.Add(zipFileInfo);
							TsPlatform.FileLog("Add Zip File : " + token);
						}
					}
					tsDataReader.NextLine();
				}
			}
		}
	}

	private void SaveCompleteCompressUnZipFile(bool bComplete)
	{
		string path = string.Format("{0}{1}", StagePreDownloadMobile.m_strLocalFilePath, Path.GetFileName("final/mobile_first.patchlist.txt"));
		using (StreamWriter streamWriter = new StreamWriter(path, false))
		{
			int num = this.m_nServerVersion;
			if (!bComplete)
			{
				num = -1;
			}
			string value = string.Format("PatchVersion={0}", num);
			streamWriter.WriteLine(value);
			value = string.Format("RemoveCacheRoot={0}", this.m_nDelCacheVersion);
			streamWriter.WriteLine(value);
			streamWriter.WriteLine("[FirstList]");
			for (int i = 0; i < this.m_CompleteCompressionFileInfo.Count; i++)
			{
				value = string.Format("{0}\t{1}\t{2}", this.m_CompleteCompressionFileInfo[i].fileName, this.m_CompleteCompressionFileInfo[i].fileSize, this.m_CompleteCompressionFileInfo[i].szMD5);
				streamWriter.WriteLine(value);
			}
		}
	}

	private void _DownLoadZipFiles()
	{
		TsLog.LogError("_DownLoadZipFiles", new object[0]);
		if (this.PreDownloadDlg == null)
		{
			this.PreDownloadDlg = (Mobile_PreDownloadDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PREDOWNLOAD_DLG);
		}
		StagePreDownloadMobile.ZipFileInfo zipFileInfo = this.m_CompressionFileInfo[0];
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(zipFileInfo.fileName, Option.defaultStackName);
		wWWItem.SetItemType(ItemType.USER_BYTESA);
		wWWItem.SetCallback(new PostProcPerItem(this._OnZipFileCompleteDownload), null);
		this.wProgressRate = TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private static byte[] _GetUnzipBuffer()
	{
		return new byte[2048];
	}

	private bool _UnZipFiles(byte[] btData, string strUnZipTagetFolderPath)
	{
		MemoryStream memoryStream = new MemoryStream(btData);
		try
		{
			using (ZipInputStream zipInputStream = new ZipInputStream(memoryStream))
			{
				ZipEntry nextEntry;
				while ((nextEntry = zipInputStream.GetNextEntry()) != null)
				{
					string directoryName = Path.GetDirectoryName(nextEntry.Name);
					string fileName = Path.GetFileName(nextEntry.Name);
					string extension = Path.GetExtension(fileName);
					string text = string.Format("{0}/{1}", strUnZipTagetFolderPath, directoryName);
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					if (fileName != string.Empty && extension != string.Empty)
					{
						string path = string.Empty;
						path = string.Format("{0}/{1}", text.ToLower(), Path.GetFileName(nextEntry.Name).ToLower());
						FileStream fileStream = File.Create(path);
						if (fileStream != null)
						{
							while (true)
							{
								int num = zipInputStream.Read(StagePreDownloadMobile.m_UnzipBuffer, 0, StagePreDownloadMobile.m_UnzipBuffer.Length);
								if (num <= 0)
								{
									break;
								}
								fileStream.Write(StagePreDownloadMobile.m_UnzipBuffer, 0, num);
							}
							fileStream.Close();
						}
					}
				}
			}
			memoryStream.Close();
		}
		catch (Exception arg)
		{
			TsLog.LogError(string.Format("UnZipFiles failed: {0}", arg), new object[0]);
			return false;
		}
		return true;
	}

	private bool _UnZipFiles(string strZipFilePath, string strUnZipTagetFolderPath)
	{
		if (File.Exists(strZipFilePath))
		{
			try
			{
				using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(strZipFilePath)))
				{
					ZipEntry nextEntry;
					while ((nextEntry = zipInputStream.GetNextEntry()) != null)
					{
						string directoryName = Path.GetDirectoryName(nextEntry.Name);
						string fileName = Path.GetFileName(nextEntry.Name);
						string extension = Path.GetExtension(fileName);
						string text = string.Format("{0}/{1}", strUnZipTagetFolderPath, directoryName);
						if (!Directory.Exists(text))
						{
							Directory.CreateDirectory(text);
						}
						if (fileName != string.Empty && extension != string.Empty)
						{
							FileStream fileStream = File.Create(string.Format("{0}/{1}", text, Path.GetFileName(nextEntry.Name)));
							int num = 2048;
							byte[] array = new byte[num];
							while (true)
							{
								num = zipInputStream.Read(array, 0, array.Length);
								if (num <= 0)
								{
									break;
								}
								fileStream.Write(array, 0, num);
							}
							fileStream.Close();
						}
					}
				}
			}
			catch (Exception arg)
			{
				TsLog.LogError(string.Format("UnZipFiles failed: {0}", arg), new object[0]);
				return false;
			}
			return true;
		}
		return true;
	}

	public static void On_OK_URL(object a_oObject)
	{
		string url = string.Format("http://{0}/mobile/updateurl.aspx?code={1}", NrGlobalReference.strWebPageDomain, NrGlobalReference.MOBILEID);
		Application.OpenURL(url);
	}
}
