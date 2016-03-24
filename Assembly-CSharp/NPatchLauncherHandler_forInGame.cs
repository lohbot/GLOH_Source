using GameMessage.Private;
using NPatch;
using omniata;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

internal class NPatchLauncherHandler_forInGame : LauncherHandler
{
	private Mobile_PreDownloadDlg patchDlg = (Mobile_PreDownloadDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PREDOWNLOAD_DLG);

	public static bool _isClosedMsgBox;

	public static bool _isShowMsgBox;

	public static bool _isWifiOK;

	public void OnProcess(string prcess, float ftotal, float fSub, int nTotal)
	{
		this.patchDlg.SetTotalProgress(ftotal, fSub, string.Empty);
		this.patchDlg.UpdateTotalProgress(ftotal, fSub, (long)nTotal, string.Empty);
	}

	public override void SetEdgeURL(ref string url_root)
	{
		string text = string.Format("http://{0}", NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceAreaInfo().szOriginalDataCDNPath);
		if (Option.GetProtocolRootPath(Protocol.HTTP) == text)
		{
			url_root = Option.GetProtocolRootPath(Protocol.HTTP);
		}
		else
		{
			url_root = string.Format("http://{0}", NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceAreaInfo().szEdgeDataCDNPath);
		}
		Debug.LogError("======================== url HTTP  : " + Option.GetProtocolRootPath(Protocol.HTTP) + " ===== " + text);
		Debug.LogError("======================== url : " + url_root);
	}

	public override Launcher.Task.TaskResult OnCheckStart()
	{
		if (!NPatchLauncherHandler_forInGame._isShowMsgBox)
		{
			if (!TsPlatform.IsEditor)
			{
				if (TsPlatform.Operator.GetSDCardCapacity() < StageNPatchLauncher.LIMIT_CAPACITY)
				{
					new MB_CapacityError
					{
						OK = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8")
					}.Show(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("6"), NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"), MessageBox.Type.OK);
				}
				if (TsPlatform.Operator.IsWifiConnect())
				{
					NPatchLauncherHandler_forInGame._isWifiOK = true;
					return Launcher.Task.TaskResult.SUCCESS;
				}
				MessageBox messageBox = new MB_WifiWarning();
				string empty = string.Empty;
				int num = (int)(Launcher.Instance._status.totalDownloadSize / 1024L / 1024L);
				if (0 >= num)
				{
					num = 1;
				}
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("5"),
					"count",
					num
				});
				messageBox.Show(empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"), MessageBox.Type.OK);
			}
			NPatchLauncherHandler_forInGame._isShowMsgBox = true;
		}
		if (!NPatchLauncherHandler_forInGame._isClosedMsgBox)
		{
			return Launcher.Task.TaskResult.RUNNING;
		}
		if (NPatchLauncherHandler_forInGame._isWifiOK)
		{
			return Launcher.Task.TaskResult.SUCCESS;
		}
		return Launcher.Task.TaskResult.FAILED;
	}

	public override void OnStartDownloadPack(string fileName, int order)
	{
		if (PlayerPrefs.GetInt(NrPrefsKey.DOWNLOAD_MOVIE, 0) == 0)
		{
			PlayerPrefs.SetInt(NrPrefsKey.DOWNLOAD_MOVIE, 1);
			Mobile_PreDownloadDlg mobile_PreDownloadDlg = (Mobile_PreDownloadDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PREDOWNLOAD_DLG);
			if (mobile_PreDownloadDlg != null)
			{
				mobile_PreDownloadDlg.PlayMovie();
			}
		}
		Logger.WriteLog(string.Format("Download start : {0}", fileName));
		NPatchLauncherHandler_forInGame.FileDownloadOmniataLog(fileName);
	}

	public override void OnStartInstallPack(string fileName, int order)
	{
		Logger.WriteLog(string.Format("Install start : {0}", fileName));
		NPatchLauncherHandler_forInGame.FileDownloadOmniataLog(fileName);
	}

	public override void OnEndPrepack()
	{
	}

	public void OnFinish()
	{
		TsAudio.RefreshAllAudioVolumes();
		if (SystemInfo.processorCount == 1)
		{
			TsAudio.SetDisableDownloadAudio(EAudioType.AMBIENT, true);
			TsAudio.SetDisableDownloadAudio(EAudioType.ENVIRONMENT, true);
		}
		FacadeHandler.EndNPATCHDownLoad(false);
	}

	public static void FileDownloadOmniataLog(string strPath)
	{
		DateTime dateTime = DateTime.Now.ToLocalTime();
		DateTime arg_29_0 = dateTime;
		DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		int num = (int)(arg_29_0 - dateTime2.ToLocalTime()).TotalSeconds;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ts", num.ToString());
		dictionary.Add("file_name", strPath);
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		dictionary.Add("account_id", myCharInfo.m_SN.ToString());
		GameObject gameObject = GameObject.Find("OmniataManager");
		if (gameObject)
		{
			OmniataComponent component = gameObject.GetComponent<OmniataComponent>();
			if (component)
			{
				component.Track("om_funnel_load", dictionary);
			}
		}
	}
}
