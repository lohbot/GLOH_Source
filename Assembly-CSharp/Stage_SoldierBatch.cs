using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using StageHelper;
using System;
using System.Collections;
using System.Diagnostics;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Stage_SoldierBatch : AStage
{
	private static readonly string s_battleAudioListener = "_BattleAudioListener";

	private SoldierBatch m_SoldierBatch;

	private bool _bTemporalBattleUpdate;

	private float m_fLoadComplete;

	public override Scene.Type SceneType()
	{
		return Scene.Type.SOLDIER_BATCH;
	}

	public override void OnPrepareSceneChange()
	{
		NrTSingleton<NkCharManager>.Instance.DeleteBattleChar();
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		NrLoadPageScreen.StepUpMain(1);
		NrLoadPageScreen.SetSkipMainStep(1);
		NrLoadPageScreen.ShowHideLoadingImg(true);
		this.m_SoldierBatch = new SoldierBatch();
		this.m_SoldierBatch.Init();
		this._bTemporalBattleUpdate = true;
		TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			@char.m_kCharMove.MoveStop(true, false);
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PLUNDER)
		{
			GS_PLUNDER_MATCH_PLAYER_REQ gS_PLUNDER_MATCH_PLAYER_REQ = new GS_PLUNDER_MATCH_PLAYER_REQ();
			gS_PLUNDER_MATCH_PLAYER_REQ.m_PersonID = charPersonInfo.GetPersonID();
			gS_PLUNDER_MATCH_PLAYER_REQ.m_nMode = 0;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_MATCH_PLAYER_REQ, gS_PLUNDER_MATCH_PLAYER_REQ);
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE)
		{
			int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_LEVEL);
			if ((int)charPersonInfo.GetLeaderSoldierInfo().GetLevel() < value)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129"),
					"level",
					value
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			GS_INFIBATTLE_MATCH_REQ obj = new GS_INFIBATTLE_MATCH_REQ();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INFIBATTLE_MATCH_REQ, obj);
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			GS_INFIBATTLE_PRACTICE_START_REQ gS_INFIBATTLE_PRACTICE_START_REQ = new GS_INFIBATTLE_PRACTICE_START_REQ();
			gS_INFIBATTLE_PRACTICE_START_REQ.i64PersonID = charPersonInfo.GetPersonID();
			gS_INFIBATTLE_PRACTICE_START_REQ.i64TargetPersonID = charPersonInfo.InfiBattlePersonID;
			gS_INFIBATTLE_PRACTICE_START_REQ.i32Rank = charPersonInfo.InfiBattleRank;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INFIBATTLE_PRACTICE_START_REQ, gS_INFIBATTLE_PRACTICE_START_REQ);
		}
		GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ = new GS_CHAR_STATE_SET_REQ();
		gS_CHAR_STATE_SET_REQ.nSet = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ);
		base.ResetCoTasks();
	}

	public override void OnEnter()
	{
		TsLog.Log("====== {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		if (TsPlatform.IsLowSystemMemory)
		{
			TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.WorldScene);
			Holder.ClearStackItem(Option.IndependentFromStageStackName, false);
			base.StartTaskSerial(CommonTasks.LoadEmptyMainScene());
			base.StartTaskSerial(CommonTasks.MemoryCleaning(true, 8));
		}
		TsPlatform.FileLog("SoldierBatch OnEnter Mem=" + NrTSingleton<NrMainSystem>.Instance.AppMemory);
		Scene.ChangeSceneType(this.SceneType());
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PLUNDER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			base.StartTaskSerial(CommonTasks.DownloadAsset(string.Format("Effect/Instant/fx_plunderloading{0}{1}", NrTSingleton<UIDataManager>.Instance.AddFilePath, Option.extAsset), new PostProcPerItem(this._fundPlunderPrepareLoading), null, true));
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			string path = string.Format("Effect/Instant/fx_direct_chaostower_gate{0}{1}", NrTSingleton<UIDataManager>.Instance.AddFilePath, Option.extAsset);
			base.StartTaskSerial(CommonTasks.DownloadAsset(path, new PostProcPerItem(this._fundPlunderPrepareLoading), null, true));
		}
		base.StartTaskSerial(CommonTasks.InitializeChangeScene());
		base.StartTaskSerial(CommonTasks.ExceptMuteAudioOnOff(EAudioType.UI, true));
		base.StartTaskSerial(this._StageProcess());
		base.StartTaskSerial(this._StageAfterPlunderMapLoadProcess());
		base.StartTaskSerial(CommonTasks.MemoryCleaning(true, 8));
		base.StartTaskSerial(CommonTasks.ExceptMuteAudioOnOff(EAudioType.UI, false));
		base.StartTaskSerial(this._StageStartPlunderPrepare());
		base.StartTaskSerial(CommonTasks.LoadEnvironment(true));
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(true));
		base.StartTaskSerial(this._StageLoadingComplete());
		base.StartTaskPararell(CommonTasks.WaitGoToBattleSoldierBatch());
	}

	public override void OnExit()
	{
		TsLog.Log("====== {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
		NrTSingleton<NkBattleCharManager>.Instance.Init();
		Battle.BabelPartyCount = (int)SoldierBatch.BABELTOWER_INFO.Count;
		SoldierBatch.BABELTOWER_INFO.Init();
		this.m_SoldierBatch.Close();
		this.m_SoldierBatch.Dispose();
		this.m_SoldierBatch = null;
		Time.timeScale = 1f;
		Resources.UnloadUnusedAssets();
		TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ = new GS_CHAR_STATE_SET_REQ();
		gS_CHAR_STATE_SET_REQ.nSet = 2;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ);
		NrTSingleton<FormsManager>.Instance.ClearShowHideForms();
	}

	protected override void OnUpdateAfterStagePrework()
	{
		if (this.m_SoldierBatch != null)
		{
			this.m_SoldierBatch.m_Input.GridInputMouse();
			this.m_SoldierBatch.Update();
		}
	}

	[DebuggerHidden]
	private IEnumerator _UpdateBattleTemporal()
	{
		Stage_SoldierBatch.<_UpdateBattleTemporal>c__Iterator49 <_UpdateBattleTemporal>c__Iterator = new Stage_SoldierBatch.<_UpdateBattleTemporal>c__Iterator49();
		<_UpdateBattleTemporal>c__Iterator.<>f__this = this;
		return <_UpdateBattleTemporal>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator _StageProcess()
	{
		Stage_SoldierBatch.<_StageProcess>c__Iterator4A <_StageProcess>c__Iterator4A = new Stage_SoldierBatch.<_StageProcess>c__Iterator4A();
		<_StageProcess>c__Iterator4A.<>f__this = this;
		return <_StageProcess>c__Iterator4A;
	}

	[DebuggerHidden]
	private IEnumerator _StageAfterPlunderMapLoadProcess()
	{
		Stage_SoldierBatch.<_StageAfterPlunderMapLoadProcess>c__Iterator4B <_StageAfterPlunderMapLoadProcess>c__Iterator4B = new Stage_SoldierBatch.<_StageAfterPlunderMapLoadProcess>c__Iterator4B();
		<_StageAfterPlunderMapLoadProcess>c__Iterator4B.<>f__this = this;
		return <_StageAfterPlunderMapLoadProcess>c__Iterator4B;
	}

	[DebuggerHidden]
	private IEnumerator _StageStartPlunderPrepare()
	{
		Stage_SoldierBatch.<_StageStartPlunderPrepare>c__Iterator4C <_StageStartPlunderPrepare>c__Iterator4C = new Stage_SoldierBatch.<_StageStartPlunderPrepare>c__Iterator4C();
		<_StageStartPlunderPrepare>c__Iterator4C.<>f__this = this;
		return <_StageStartPlunderPrepare>c__Iterator4C;
	}

	public bool GameResult()
	{
		this.DeleteBattleAudioListener();
		return true;
	}

	public GameObject CreateBattleAudioListener()
	{
		maxCamera component = Camera.main.gameObject.GetComponent<maxCamera>();
		if (component == null)
		{
			TsLog.LogError("Cannot Found CameraController~! at GO= " + Camera.main.gameObject.name, new object[0]);
			return null;
		}
		GameObject gameObject = GameObject.Find(Stage_SoldierBatch.s_battleAudioListener);
		if (gameObject != null)
		{
			if (gameObject.GetComponent<TsPositionFollower>() == null)
			{
				gameObject.AddComponent<TsPositionFollower>();
			}
		}
		else
		{
			gameObject = new GameObject(Stage_SoldierBatch.s_battleAudioListener, new Type[]
			{
				typeof(TsPositionFollower)
			});
		}
		gameObject.transform.parent = TsSceneSwitcher.Instance._GetBundle_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene).transform;
		return gameObject;
	}

	public void DeleteBattleAudioListener()
	{
		TsAudioManager.Instance.InitAudioListenerSwitcher();
		GameObject gameObject = GameObject.Find(Stage_SoldierBatch.s_battleAudioListener);
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	[DebuggerHidden]
	private IEnumerator _StageLoadingComplete()
	{
		Stage_SoldierBatch.<_StageLoadingComplete>c__Iterator4D <_StageLoadingComplete>c__Iterator4D = new Stage_SoldierBatch.<_StageLoadingComplete>c__Iterator4D();
		<_StageLoadingComplete>c__Iterator4D.<>f__this = this;
		return <_StageLoadingComplete>c__Iterator4D;
	}

	private void _fundPlunderPrepareLoading(IDownloadedItem wItem, object obj)
	{
		try
		{
			if (this.m_SoldierBatch != null)
			{
				if (this.m_SoldierBatch.PlunderLoading != null)
				{
					this.m_SoldierBatch.PlunderLoading = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.m_SoldierBatch.PlunderLoading = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.m_SoldierBatch.PlunderLoading.SetActive(false);
					if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PLUNDER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
					{
						NewLoaingDlg newLoaingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE) as NewLoaingDlg;
						if (newLoaingDlg != null)
						{
							newLoaingDlg.SetLoadingPageEffect(this.m_SoldierBatch.PlunderLoading);
						}
					}
					TsPlatform.FileLog("SoldierBatch _fundPlunderPrepareLoading Pass");
					if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
					{
						this.m_SoldierBatch.MakePlunderPrepareUI();
					}
				}
			}
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}
}
