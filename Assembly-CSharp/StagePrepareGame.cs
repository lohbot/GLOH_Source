using GameMessage.Private;
using Ndoors.Framework.Stage;
using StageHelper;
using System;
using System.Collections;
using System.Diagnostics;
using TsBundle;
using UnityEngine;
using UnityForms;

public class StagePrepareGame : AStage
{
	private static bool IsTableLoaded;

	public override Scene.Type SceneType()
	{
		return Scene.Type.PREPAREGAME;
	}

	public override void OnPrepareSceneChange()
	{
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		if (NrLoadPageScreen.LoginLatestChar)
		{
			NrLoadPageScreen.StepUpMain(1);
		}
		else
		{
			NrLoadPageScreen.StepUpMain(2);
		}
		NrLoadPageScreen.ShowHideLoadingImg(true);
	}

	public override void OnEnter()
	{
		TsLog.Log("--- {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		Scene.ChangeSceneType(this.SceneType());
		CommonTasks.ClearAssetBundleResources(false);
		if (Scene.PreScene == Scene.Type.SELECTCHAR)
		{
			SceneSwitch.DeleteFieldScene();
		}
		NrTSingleton<FormsManager>.Instance.ClearShowHideForms();
		base.StartTaskSerial(CommonTasks.InitializeChangeScene());
		base.StartTaskSerial(CommonTasks.LoadEmptyMainScene());
		base.StartTaskSerial(CommonTasks.MemoryCleaning(true, 8));
		base.StartTaskSerial(this._DownloadTables());
		base.StartTaskSerial(this._SetBilling());
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(false));
		base.StartTaskSerial(this._WaitToGoNextStage());
		if (TsPlatform.IsWeb)
		{
		}
		NrTSingleton<NkClientLogic>.Instance.SetLoginGameServer(false);
		UnityEngine.Debug.LogWarning("========== GS_LOAD_CHAR_NFY : SetLoginGameServer false ----- ");
	}

	public override void OnExit()
	{
		TsLog.Log("--- {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
		NrLoadPageScreen.LoginLatestChar = false;
		NmMainFrameWork.RemoveBGM(true);
		Application.targetFrameRate = PlayerPrefs.GetInt("SaveFps", NmMainFrameWork.MAX_FPS);
	}

	protected override void OnUpdateAfterStagePrework()
	{
	}

	[DebuggerHidden]
	private IEnumerator _WaitToGoNextStage()
	{
		return new StagePrepareGame.<_WaitToGoNextStage>c__Iterator44();
	}

	private void OnDownload_PredownloadMarking(IDownloadedItem wItem, object obj)
	{
		if (!wItem.isSuccess || wItem.isCanceled)
		{
			TsLog.LogWarning("Failed~! OnDownload_PredownloadMarking() wItem= {0}", new object[]
			{
				wItem.assetPath
			});
			return;
		}
		bool flag = TsCaching.IsVersionCached(wItem.url, wItem.version, wItem.UseCustomCache);
		if (flag)
		{
			TsCaching.MarkAsUsed(wItem.url, wItem.version, wItem.UseCustomCache);
		}
	}

	[DebuggerHidden]
	private IEnumerator _DownloadTables()
	{
		return new StagePrepareGame.<_DownloadTables>c__Iterator45();
	}

	public bool OnGameServerConnected()
	{
		TsLog.Log("{0}.Rcv_WS_CONNECT_GAMESERVER_ACK", new object[]
		{
			StageSystem.GetCurrentStageName()
		});
		FacadeHandler.Req_GS_AUTH_SESSION_REQ(NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID, NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_siAuthSessionKey, NrTSingleton<NrMainSystem>.Instance.GetLatestPersonID(), 0, 403);
		return true;
	}

	[DebuggerHidden]
	private IEnumerator _SetBilling()
	{
		return new StagePrepareGame.<_SetBilling>c__Iterator46();
	}
}
