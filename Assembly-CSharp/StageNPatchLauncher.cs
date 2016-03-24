using GameMessage.Private;
using Ndoors.Framework.Stage;
using NPatch;
using omniata;
using StageHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TsBundle;
using UnityEngine;
using UnityForms;

public class StageNPatchLauncher : AStage
{
	public static StageNPatchLauncher Instance;

	private static int UNITY_CACHE_SIZE = 500;

	public static long LIMIT_CAPACITY = 600L;

	private string m_strLocalFilePath = string.Empty;

	private string m_strWebPath = string.Empty;

	private bool bPatchEnd;

	private Mobile_PreDownloadDlg PreDownloadDlg;

	private float fTime;

	public StageNPatchLauncher()
	{
		UnityEngine.Debug.Log("StageNPatchLauncher.StageNPatchLauncher()");
		TsLog.LogError("!!!!!!StageNPatchLauncher()", new object[0]);
	}

	public override Scene.Type SceneType()
	{
		return Scene.Type.NPATCH_DOWNLOAD;
	}

	public override void OnPrepareSceneChange()
	{
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		NrLoadPageScreen.StepUpMain(1);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.LOGIN_SELECT_PLATFORM_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.LOGINRATING_DLG);
		this.PreDownloadDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PREDOWNLOAD_DLG) as Mobile_PreDownloadDlg);
		NmMainFrameWork.DeleteImage();
		this.SetPlatformPath();
	}

	public override void OnEnter()
	{
		if (StageNPatchLauncher.Instance != null)
		{
			StageNPatchLauncher.Instance = this;
		}
		if (this.PreDownloadDlg == null)
		{
			NrLoadPageScreen.ShowHideLoadingImg(false);
			this.PreDownloadDlg = (Mobile_PreDownloadDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PREDOWNLOAD_DLG);
		}
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
		int num = 0;
		if (!int.TryParse(NrTSingleton<NrGlobalReference>.Instance.ResourcesVer, out num))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"======================== end",
				NrTSingleton<NrGlobalReference>.Instance.ResourcesVer,
				"== ",
				num
			}));
			if (TsPlatform.IsAndroid)
			{
				NrTSingleton<NrMainSystem>.Instance.QuitGame(false);
			}
			else if (TsPlatform.IsIPhone)
			{
				NrTSingleton<NrMainSystem>.Instance.m_ReLogin = false;
				NrTSingleton<NrMainSystem>.Instance.m_Login_BG = true;
				NrTSingleton<NrGlobalReference>.Instance.localWWW = true;
				NrTSingleton<NrMainSystem>.Instance.ReLogin(false);
			}
			return;
		}
		string text2 = string.Format("http://{0}", NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceAreaInfo().szOriginalDataCDNPath);
		UnityEngine.Debug.LogError("======================== WebPath url : " + text2);
		NPatchLauncherBehaviour.PatchStart(this.m_strLocalFilePath, text2, new NPatchLauncherHandler_forInGame(), false, num, false, string.Empty);
		if (this.PreDownloadDlg != null)
		{
			this.PreDownloadDlg.SetText("Check File...");
		}
		StageNPatchLauncher.DownloadOmniataLog(true);
		base.StartTaskSerial(CommonTasks.InitializeChangeScene());
		base.StartTaskPararell(this._PatchWorkflow());
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(true));
		this.bPatchEnd = false;
		if (!Launcher.Instance.IsFullPatchLevel)
		{
			NrTSingleton<MATEventManager>.Instance.MeasureEvent("Content DL Start");
		}
		else
		{
			NrTSingleton<MATEventManager>.Instance.MeasureEvent("Add Content DL start");
		}
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
		StageNPatchLauncher.DownloadOmniataLog(false);
	}

	private void _OnMessageBoxOk(IntroMsgBoxDlg a_cthis, object a_oObject)
	{
		NrTSingleton<NrMainSystem>.Instance.QuitGame(false);
		TsLog.LogError("StageNPatch - Reset", new object[0]);
	}

	private void Exit(IntroMsgBoxDlg a_cthis, object a_oObject)
	{
		NrTSingleton<NrMainSystem>.Instance.QuitGame(false);
	}

	protected override void OnUpdateAfterStagePrework()
	{
		if (NPatchLauncherBehaviour.Instance.IsFinish)
		{
			if (NPatchLauncherBehaviour.Instance.ErrorString == string.Empty)
			{
				if ((int)Launcher.Instance._status.totalSize > 0)
				{
					FacadeHandler.EndNPATCHDownLoad(true);
					if (!this.bPatchEnd)
					{
						if (!Launcher.Instance.IsFullPatchLevel)
						{
							NrTSingleton<MATEventManager>.Instance.MeasureEvent("Content DL end");
						}
						else
						{
							NrTSingleton<MATEventManager>.Instance.MeasureEvent("Add Content DL end");
						}
					}
					this.bPatchEnd = true;
				}
				else
				{
					FacadeHandler.EndNPATCHDownLoad(false);
				}
			}
			else
			{
				new MB_PatchError
				{
					OK = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("40")
				}.Show(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("41"), NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("2"), MessageBox.Type.OK);
			}
			return;
		}
		if (this.PreDownloadDlg != null)
		{
			float num = 0f;
			this.PreDownloadDlg.SetTotalProgress(0f, 0f, string.Empty);
			long num2 = Launcher.Instance._status.totalSize;
			long num3 = Launcher.Instance._status.totalProcessedSize;
			string taskStatus = Launcher.Instance._status.taskStatus;
			if (num2 == 0L || num3 == 0L)
			{
			}
			num3 = Launcher.Instance._status.taskProcessedSize;
			num2 = Launcher.Instance._status.taskSize;
			float num4 = (float)num3 / (float)num2;
			if (Launcher.Instance._status.fullPackCount * 2 > Launcher.Instance._status.totalTaskProcessedCount)
			{
				int fullPackCount = Launcher.Instance._status.fullPackCount;
				float num5 = 1f / (float)fullPackCount;
				if (Launcher.Instance._status.taskType == TASKTYPE.DOWNLOAD)
				{
					num3 = (long)Launcher.Instance._status.totalTaskProcessedCount;
					num2 = (long)Launcher.Instance._status.fullPackCount;
					if (Launcher.Instance._status.totalTaskProcessedCount <= 0)
					{
						num = num4 * num5 * 0.8f;
					}
					else
					{
						if (num4 == 1f)
						{
							num = ((float)Launcher.Instance._status.totalTaskProcessedCount - 1f) * num5 * 0.8f + num4 * num5 * 0.8f;
						}
						else
						{
							num = (float)Launcher.Instance._status.totalTaskProcessedCount * num5 * 0.8f + num4 * num5 * 0.8f;
						}
						if (num > 0.8f)
						{
							num = 0.8f;
						}
					}
				}
				else if (Launcher.Instance._status.taskType == TASKTYPE.INSTALL)
				{
					int num6 = Launcher.Instance._status.totalTaskProcessedCount - Launcher.Instance._status.fullPackCount;
					num = (float)num6 * num5 * 0.1f + num4 * num5 * 0.1f + 0.8f;
				}
			}
			else
			{
				num2 = (long)(Launcher.Instance._status.totalTaskCount - Launcher.Instance._status.fullPackCount * 2);
				num3 = (long)(Launcher.Instance._status.totalTaskProcessedCount - Launcher.Instance._status.fullPackCount * 2);
				num = (float)num3 / (float)num2 * 0.1f + 0.9f;
				if (num > 1f)
				{
					num = 1f;
				}
				if (Time.realtimeSinceStartup - this.fTime > 0.2f)
				{
					this.fTime = Time.realtimeSinceStartup;
				}
			}
			if (Launcher.Instance.IsRunning)
			{
				this.PreDownloadDlg.SetTotalProgress(num, num4, taskStatus);
			}
			else
			{
				this.PreDownloadDlg.UpdateTotalProgress(num, num4, num2, taskStatus);
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator _PatchWorkflow()
	{
		return new StageNPatchLauncher.<_PatchWorkflow>c__Iterator3F();
	}

	public void SetPlatformPath()
	{
		string text = string.Empty;
		if (TsPlatform.IsEditor)
		{
			text = string.Format("{0}/Mobile/", NrTSingleton<NrGlobalReference>.Instance.basePath);
		}
		else if (TsPlatform.IsIPhone)
		{
			text = string.Format("{0}/at2/cacheroot/", TsPlatform.Operator.GetFileDir());
		}
		else if (TsPlatform.IsAndroid)
		{
			text = string.Format("{0}/at2/cacheroot/", TsPlatform.Operator.GetFileDir());
		}
		else
		{
			TsLog.Assert(false, "Unknown Platform", new object[0]);
		}
		this.m_strLocalFilePath = text;
		Option.SetProtocolRootPath(Protocol.FILE, text);
		UnityEngine.Debug.LogWarning("StageNPatchLauncher : " + text);
		this.m_strWebPath = Option.GetProtocolRootPath(Protocol.HTTP);
		TsLog.LogWarning("HttpPath[{0}] Cachepath[{1}]", new object[]
		{
			this.m_strWebPath,
			text
		});
		TsPlatform.FileLog("HttpPath = " + this.m_strWebPath + ", CacheDpath = " + text);
		NrTSingleton<NrGlobalReference>.Instance.localWWW = true;
		NrTSingleton<NrGlobalReference>.Instance.useCache = true;
		TsCaching.InitiailzeCustomCaching(this.m_strWebPath, text);
		Caching.maximumAvailableDiskSpace = (long)(1048576 * StageNPatchLauncher.UNITY_CACHE_SIZE);
		if (TsPlatform.IsAndroid)
		{
			NrTSingleton<NrGlobalReference>.Instance.STR_MOBILE_VER = TsPlatform.APP_VERSION_AND;
		}
		else if (TsPlatform.IsIPhone || TsPlatform.IsIPad())
		{
			NrTSingleton<NrGlobalReference>.Instance.STR_MOBILE_VER = TsPlatform.APP_VERSION_IOS;
		}
	}

	private void _FinalPatchListComplete(IDownloadedItem wItem, object obj)
	{
		if (!TsPlatform.IsEditor)
		{
			if (TsPlatform.Operator.GetSDCardCapacity() < StageNPatchLauncher.LIMIT_CAPACITY)
			{
				MessageBox.Show<MB_CapacityError>();
			}
			else if (!TsPlatform.Operator.IsWifiConnect())
			{
				MessageBox.Show<MB_WifiWarning>();
			}
		}
	}

	public static void DownloadOmniataLog(bool bStart)
	{
		DateTime dateTime = DateTime.Now.ToLocalTime();
		DateTime arg_29_0 = dateTime;
		DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		int num = (int)(arg_29_0 - dateTime2.ToLocalTime()).TotalSeconds;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ts", num.ToString());
		if (bStart)
		{
			dictionary.Add("step", "loading_start");
		}
		else
		{
			dictionary.Add("step", "loading_complete");
		}
		dictionary.Add("device", SystemInfo.deviceUniqueIdentifier);
		if (TsPlatform.IsAndroid)
		{
			dictionary.Add("version", TsPlatform.APP_VERSION_AND);
		}
		else if (TsPlatform.IsIPhone)
		{
			dictionary.Add("version", TsPlatform.APP_VERSION_IOS);
		}
		GameObject pkGoOminiata = StageSystemCheck.pkGoOminiata;
		if (pkGoOminiata)
		{
			OmniataComponent component = pkGoOminiata.GetComponent<OmniataComponent>();
			if (component)
			{
				component.TrackLoad(dictionary);
			}
		}
	}
}
