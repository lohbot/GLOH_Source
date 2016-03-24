using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using StageHelper;
using System;
using System.Collections;
using System.Diagnostics;

public class StageCutScene : AStage
{
	public override Scene.Type SceneType()
	{
		return Scene.Type.CUTSCENE;
	}

	public override void OnPrepareSceneChange()
	{
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		NrLoadPageScreen.CustomLoadingProgress = true;
		NrLoadPageScreen.StepUpMain(1);
		NrLoadPageScreen.ShowHideLoadingImg(true);
	}

	public override void OnEnter()
	{
		TsLog.Log("--- {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		SceneSwitch.SetLastLoadedField(string.Empty);
		Scene.ChangeSceneType(this.SceneType());
		SceneSwitch.DeleteSceneExceptTerritory();
		CommonTasks.ClearAssetBundleResources(false);
		base.StartTaskSerial(CommonTasks.InitializeChangeScene());
		base.StartTaskSerial(CommonTasks.MuteAudio(true));
		base.StartTaskSerial(CommonTasks.LoadEmptyMainScene());
		base.StartTaskSerial(this._LoadCutScene());
		base.StartTaskSerial(CommonTasks.SetGUIBehaviourScene());
		base.StartTaskSerial(this._WaitReadyGameDrama());
		base.StartTaskSerial(CommonTasks.MemoryCleaning(true, 8));
		base.StartTaskSerial(CommonTasks.MuteAudio(false));
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(true));
	}

	public override void OnExit()
	{
		TsLog.Log("--- {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo != null && NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bGameConnected)
		{
			GS_SET_DRAMA_REQ gS_SET_DRAMA_REQ = new GS_SET_DRAMA_REQ();
			gS_SET_DRAMA_REQ.SetType = 2;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SET_DRAMA_REQ, gS_SET_DRAMA_REQ);
		}
		NrLoadPageScreen.CustomLoadingProgress = false;
	}

	protected override void OnUpdateAfterStagePrework()
	{
	}

	[DebuggerHidden]
	private IEnumerator _LoadCutScene()
	{
		return new StageCutScene.<_LoadCutScene>c__Iterator35();
	}

	[DebuggerHidden]
	private IEnumerator _WaitReadyGameDrama()
	{
		return new StageCutScene.<_WaitReadyGameDrama>c__Iterator36();
	}
}
