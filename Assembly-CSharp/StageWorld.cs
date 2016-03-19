using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using StageHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TsBundle;
using UnityEngine;
using UnityForms;

public class StageWorld : AStage
{
	public static bool s_bIsRefresh;

	public static bool s_bIsShowRegionName = true;

	private static eSOLDIER_BATCH_MODE m_eBatchMode = eSOLDIER_BATCH_MODE.MODE_MAX;

	private static eMINE_MESSAGE m_eMineMsgType;

	private static ePLUNDER_MESSAGE m_ePlunderMsgType;

	private static readonly string s_WorldAudioListener = "_WorldAudioListener";

	private NrCharMapInfo m_kCharMapInfo;

	public static bool m_bReloadMap;

	public bool m_bLoadMap;

	public static bool m_bWorldStable;

	private MapLoader _mapLoader;

	public bool m_bAfterWorldLoadComplete;

	public bool m_bEmulatorCheck;

	private static bool bSendFirstProfile;

	public static eSOLDIER_BATCH_MODE BATCH_MODE
	{
		get
		{
			return StageWorld.m_eBatchMode;
		}
		set
		{
			StageWorld.m_eBatchMode = value;
		}
	}

	public static eMINE_MESSAGE MINEMSG_TYPE
	{
		get
		{
			return StageWorld.m_eMineMsgType;
		}
		set
		{
			StageWorld.m_eMineMsgType = value;
		}
	}

	public static ePLUNDER_MESSAGE PLUNDERMSG_TYPE
	{
		get
		{
			return StageWorld.m_ePlunderMsgType;
		}
		set
		{
			StageWorld.m_ePlunderMsgType = value;
		}
	}

	public StageWorld()
	{
		this.m_kCharMapInfo = new NrCharMapInfo();
	}

	public override Scene.Type SceneType()
	{
		return Scene.Type.WORLD;
	}

	public override void OnPrepareSceneChange()
	{
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		NrLoadPageScreen.StepUpMain(1);
		this._mapLoader.Reset();
		if (Scene.CurScene != Scene.Type.BATTLE)
		{
			NrLoadPageScreen.ShowHideLoadingImg(true);
		}
		else if (Battle.BATTLE.Observer)
		{
			NrLoadPageScreen.ShowHideLoadingImg(true);
		}
		if ((Scene.PreScene == Scene.Type.BATTLE || Scene.CurScene == Scene.Type.BATTLE || Scene.PreScene == Scene.Type.SOLDIER_BATCH || Scene.CurScene == Scene.Type.SOLDIER_BATCH) && (TsPlatform.IsLowSystemMemory || TsPlatform.IsEditor))
		{
			NrTSingleton<NkClientLogic>.Instance.CharWarpRequest(0);
			GS_WARP_REQ gS_WARP_REQ = new GS_WARP_REQ();
			gS_WARP_REQ.nMode = 1;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WARP_REQ, gS_WARP_REQ);
		}
		base.ResetCoTasks();
	}

	public override void OnReloadReserved()
	{
		NrLoadPageScreen.StepUpMain(1);
		NrLoadPageScreen.SetSkipMainStep(1);
	}

	public override void OnEnter()
	{
		TsLog.Log("====== {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		Scene.ChangeSceneType(this.SceneType());
		this._BeforeMapLoad();
		base.StartTaskSerial(CommonTasks.InitializeChangeScene());
		if (Scene.PreScene != Scene.Type.BATTLE)
		{
			base.StartTaskSerial(CommonTasks.BGMExceptMuteAudio(true));
		}
		base.StartTaskSerial(CommonTasks.ClearAudioStack());
		if (StageWorld.m_bReloadMap)
		{
			base.StartTaskSerial(CommonTasks.LoadEmptyMainScene());
			base.StartTaskSerial(CommonTasks.MemoryCleaning(true, 8));
		}
		if (this.m_bLoadMap)
		{
			base.StartTaskSerial(this._mapLoader.StartLoadMap());
		}
		base.StartTaskSerial(this._StageProcess());
		base.StartTaskSerial(CommonTasks.EnableCharacterLoad());
		base.StartTaskSerial(EventTriggerMapManager.Instance.RunStandByWork());
		base.StartTaskSerial(CommonTasks.LoadEnvironment(true));
		if (Scene.PreScene != Scene.Type.BATTLE)
		{
			base.StartTaskSerial(CommonTasks.MuteAudio(false));
		}
		base.StartTaskSerial(this.BlueStacksCheck());
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(true));
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			base.StartTaskSerial(this.OpenMobileNotice());
		}
		base.StartTaskSerial(EventTriggerMapManager.Instance.RunPostLoadWork());
		NrLoadPageScreen.IncreaseProgress(2f);
		base.StartTaskSerial(this.EndWorldLoad());
		base.StartTaskPararell(this._ProcessAfterWorldLoadComplete());
		base.StartTaskPararell(CommonTasks.WaitGoToBattleWorld());
	}

	[DebuggerHidden]
	private IEnumerator TestGC()
	{
		return new StageWorld.<TestGC>c__Iterator58();
	}

	public override void OnExit()
	{
		NrTSingleton<NkQuestManager>.Instance.ClearClientNpc();
		NrTSingleton<NrAutoPath>.Instance.ClearRPPoint();
		NrTSingleton<MapManager>.Instance.ClearExtraMapInfo();
		RightMenuQuestUI rightMenuQuestUI = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_QUEST) as RightMenuQuestUI;
		if (rightMenuQuestUI != null)
		{
			rightMenuQuestUI.OnTouchSoundStop();
		}
		StageWorld.m_bWorldStable = false;
		NrTSingleton<UIManager>.Instance.CloseKeyboard();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.GAMEGUIDE_DLG);
		NrTSingleton<UIDataManager>.Instance.DeleteBundle();
		TsLog.Log("====== {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
	}

	protected override void OnUpdateAfterStagePrework()
	{
		AlarmManager.GetInstance().LevelUpAlarmUpdate();
	}

	private void _BeforeMapLoad()
	{
		NpcCache.Enabled = true;
		this.m_kCharMapInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo;
		StageWorld.m_bWorldStable = false;
		this.m_bAfterWorldLoadComplete = false;
		NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
		NrTSingleton<NkQuestManager>.Instance.RewardShow = false;
		if (Scene.CurScene != Scene.Type.EMPTY)
		{
			if (Scene.PreScene == Scene.Type.WORLD && Scene.CurScene == Scene.Type.BATTLE)
			{
				if (TsPlatform.IsLowSystemMemory || TsPlatform.IsEditor)
				{
					StageWorld.m_bReloadMap = true;
				}
				else
				{
					StageWorld.m_bReloadMap = false;
				}
			}
			else if (Scene.PreScene == Scene.Type.BATTLE && Scene.CurScene == Scene.Type.WORLD)
			{
				if (TsPlatform.IsLowSystemMemory || TsPlatform.IsEditor)
				{
					StageWorld.m_bReloadMap = true;
				}
				else
				{
					StageWorld.m_bReloadMap = false;
				}
			}
			else if (Scene.PreScene == Scene.Type.SOLDIER_BATCH && Scene.CurScene == Scene.Type.WORLD)
			{
				if (TsPlatform.IsLowSystemMemory || TsPlatform.IsEditor)
				{
					StageWorld.m_bReloadMap = true;
				}
				else
				{
					StageWorld.m_bReloadMap = false;
				}
			}
			else
			{
				StageWorld.m_bReloadMap = true;
			}
		}
		else
		{
			StageWorld.m_bReloadMap = true;
		}
		this.DeleteWorldMap();
		this.m_bLoadMap = false;
		NrTSingleton<NrTerrain>.Instance.InitTerrainInfo();
		this.m_kCharMapInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo;
		int nMapIndex = this.m_kCharMapInfo.m_nMapIndex;
		if (NrTSingleton<MapManager>.Instance.LoadCurrentMapResource(nMapIndex, this) || StageWorld.m_bReloadMap)
		{
			this.Init_Map(this);
		}
	}

	[DebuggerHidden]
	private IEnumerator ChangeTerritoryStage()
	{
		return new StageWorld.<ChangeTerritoryStage>c__Iterator59();
	}

	[DebuggerHidden]
	private IEnumerator _StageProcess()
	{
		StageWorld.<_StageProcess>c__Iterator5A <_StageProcess>c__Iterator5A = new StageWorld.<_StageProcess>c__Iterator5A();
		<_StageProcess>c__Iterator5A.<>f__this = this;
		return <_StageProcess>c__Iterator5A;
	}

	[DebuggerHidden]
	private IEnumerator _ProcessAfterWorldLoadComplete()
	{
		StageWorld.<_ProcessAfterWorldLoadComplete>c__Iterator5B <_ProcessAfterWorldLoadComplete>c__Iterator5B = new StageWorld.<_ProcessAfterWorldLoadComplete>c__Iterator5B();
		<_ProcessAfterWorldLoadComplete>c__Iterator5B.<>f__this = this;
		return <_ProcessAfterWorldLoadComplete>c__Iterator5B;
	}

	[DebuggerHidden]
	private IEnumerator _ProcessBattleMapLoad()
	{
		StageWorld.<_ProcessBattleMapLoad>c__Iterator5C <_ProcessBattleMapLoad>c__Iterator5C = new StageWorld.<_ProcessBattleMapLoad>c__Iterator5C();
		<_ProcessBattleMapLoad>c__Iterator5C.<>f__this = this;
		return <_ProcessBattleMapLoad>c__Iterator5C;
	}

	private void Load_Completed()
	{
		GS_LOAD_COMPLETED_REQ gS_LOAD_COMPLETED_REQ = new GS_LOAD_COMPLETED_REQ();
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_nIndunUnique != -1)
		{
			gS_LOAD_COMPLETED_REQ.Status = 65536L;
		}
		else
		{
			gS_LOAD_COMPLETED_REQ.Status = 2L;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_LOAD_COMPLETED_REQ, gS_LOAD_COMPLETED_REQ);
	}

	private void DeleteWorldMap()
	{
		if (StageWorld.m_bReloadMap)
		{
			TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.WorldScene);
			Holder.ClearStackItem(Option.IndependentFromStageStackName, false);
		}
	}

	public void Init_Map(AStage a_cStageID)
	{
		Battle.DeleteBattleMap();
		MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(this.m_kCharMapInfo.m_nMapIndex.ToString());
		if (mapInfo == null)
		{
			return;
		}
		this._mapLoader = new MapLoader(this, mapInfo);
		this.m_bLoadMap = true;
	}

	public static void Map_Load()
	{
		if (Camera.main == null)
		{
			return;
		}
		maxCamera maxCamera = Camera.main.gameObject.GetComponent<maxCamera>();
		if (maxCamera == null)
		{
			maxCamera = Camera.main.gameObject.AddComponent<maxCamera>();
		}
		CameraController component = maxCamera.GetComponent<CameraController>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		if (!maxCamera.enabled)
		{
			maxCamera.enabled = true;
		}
		FPS component2 = Camera.main.gameObject.GetComponent<FPS>();
		if (component2 == null)
		{
			Camera.main.gameObject.AddComponent<FPS>();
		}
		TsQualityManager.Instance.Refresh();
	}

	private void _Gate_DownLoad_Completed(List<WWWItem> wiList, object obj)
	{
		if (wiList == null || 0 >= wiList.Count)
		{
			return;
		}
		MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(this.m_kCharMapInfo.m_nMapIndex.ToString());
		if (mapInfo == null)
		{
			return;
		}
		Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
		foreach (WWWItem current in wiList)
		{
			if (!(null == current.GetSafeBundle()))
			{
				GameObject value = current.mainAsset as GameObject;
				dictionary.Add(current.strParam, value);
			}
		}
		GameObject gameObject = NkUtil.FindOrCreate("WarpGate");
		GATE_INFO[] gateInfo = mapInfo.GetGateInfo();
		for (int i = 0; i < gateInfo.Length; i++)
		{
			GATE_INFO gATE_INFO = gateInfo[i];
			if (dictionary.ContainsKey(gATE_INFO.BUNDLE_PATH))
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(dictionary[gATE_INFO.BUNDLE_PATH], Vector3.zero, Quaternion.identity) as GameObject;
				if (!(null == gameObject2))
				{
					gameObject2.transform.parent = gameObject.transform;
					gameObject2.transform.localScale *= gATE_INFO.GATE_SCALE;
					gameObject2.transform.name = string.Format("WarpGate_{0}", gATE_INFO.GATE_IDX);
					gameObject2.layer = TsLayer.TERRAIN;
					Vector3 localPosition = gameObject2.transform.localPosition;
					localPosition.x = gATE_INFO.SRC_POSX;
					localPosition.y = gATE_INFO.SRC_POSY;
					localPosition.z = gATE_INFO.SRC_POSZ;
					gameObject2.transform.localPosition = localPosition;
					gameObject2.transform.localEulerAngles = new Vector3(0f, gATE_INFO.SRC_ANGLE, 0f);
					NmWarpGateController nmWarpGateController = gameObject2.AddComponent<NmWarpGateController>();
					if (null != nmWarpGateController)
					{
						nmWarpGateController.nGateIndex = gATE_INFO.GATE_IDX;
					}
					if (gATE_INFO.GATE_SCALE > 0f)
					{
						BoxCollider component = gameObject2.GetComponent<BoxCollider>();
						if (component != null)
						{
							component.size = new Vector3(component.size.x, component.size.y, component.size.z + 1f / gATE_INFO.GATE_SCALE);
						}
					}
				}
			}
		}
		TsSceneSwitcher.Instance.Collect(TsSceneSwitcher.ESceneType.WorldScene, gameObject);
		dictionary.Clear();
	}

	public static QUEST_CONST.eQUESTSTATE GetQuestState(int nCharKind)
	{
		QUEST_CONST.eQUESTSTATE eQUESTSTATE = QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_NOT_VIEW;
		if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nCharKind) == null)
		{
			return eQUESTSTATE;
		}
		NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
		NkQuestManager instance2 = NrTSingleton<NkQuestManager>.Instance;
		USER_CURRENT_QUEST_INFO[] userCurrentQuestInfo = instance.GetUserCurrentQuestInfo();
		bool flag = false;
		bool flag2 = false;
		string text = string.Empty;
		for (byte b = 0; b < 10; b += 1)
		{
			if (userCurrentQuestInfo[(int)b].strQuestUnique != null)
			{
				string strQuestUnique = userCurrentQuestInfo[(int)b].strQuestUnique;
				CQuest questByQuestUnique = instance2.GetQuestByQuestUnique(strQuestUnique);
				if (questByQuestUnique != null && questByQuestUnique.GetQuestCommon().i32EndType == 0 && questByQuestUnique.GetQuestCommon().i64EndTypeVal == (long)nCharKind)
				{
					if (instance.CheckQuestComplete(strQuestUnique, userCurrentQuestInfo[(int)b]))
					{
						eQUESTSTATE = QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE;
						text = strQuestUnique;
					}
					else
					{
						for (int i = 0; i < 3; i++)
						{
							if (questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode == 30)
							{
								eQUESTSTATE = QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE;
								flag2 = true;
								text = strQuestUnique;
							}
						}
					}
				}
			}
		}
		NPC_QUEST_LIST npcQuestMatchInfo = NrTSingleton<NkQuestManager>.Instance.GetNpcQuestMatchInfo(nCharKind);
		if (npcQuestMatchInfo != null && eQUESTSTATE != QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
		{
			for (int j = 0; j < npcQuestMatchInfo.NpcQuestList.Count; j++)
			{
				QUEST_CONST.eQUESTSTATE questState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(npcQuestMatchInfo.NpcQuestList[j].strQuestUnique);
				if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
				{
					CQuest questByQuestUnique2 = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(npcQuestMatchInfo.NpcQuestList[j].strQuestUnique);
					if (questByQuestUnique2 != null && questByQuestUnique2.GetQuestCommon().i64EndTypeVal == (long)nCharKind)
					{
						eQUESTSTATE = questState;
						text = npcQuestMatchInfo.NpcQuestList[j].strQuestUnique;
						break;
					}
				}
				else if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
				{
					eQUESTSTATE = questState;
					flag = true;
					text = npcQuestMatchInfo.NpcQuestList[j].strQuestUnique;
				}
				else if (!flag && questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING)
				{
					eQUESTSTATE = questState;
					flag2 = true;
				}
				else if (!flag && !flag2)
				{
					eQUESTSTATE = questState;
				}
			}
		}
		if (text != string.Empty)
		{
			CQuest questByQuestUnique3 = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(text);
			if (questByQuestUnique3 == null)
			{
				return eQUESTSTATE;
			}
			if (questByQuestUnique3.IsDayQuest())
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo == null)
				{
					return eQUESTSTATE;
				}
				int num = (int)myCharInfo.GetCharDetail(5);
				if (0 < num && NrTSingleton<NkQuestManager>.Instance.IsCompletedQuestGroup(num))
				{
					eQUESTSTATE = QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_NOT_VIEW;
				}
				else if (eQUESTSTATE == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
				{
					eQUESTSTATE = QUEST_CONST.eQUESTSTATE.QUESTSTATE_DAYQUEST_COMPLETE;
				}
				else if (eQUESTSTATE == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
				{
					eQUESTSTATE = QUEST_CONST.eQUESTSTATE.QUESTSTATE_DAYQUEST_ACCEPTABLE;
				}
			}
		}
		return eQUESTSTATE;
	}

	public GameObject CreateWorldAudioListener()
	{
		if (Camera.main)
		{
			maxCamera component = Camera.main.gameObject.GetComponent<maxCamera>();
			if (component == null)
			{
				TsLog.LogError("Cannot Found CameraController~! at GO= " + Camera.main.gameObject.name, new object[0]);
				return null;
			}
		}
		GameObject gameObject = GameObject.Find(StageWorld.s_WorldAudioListener);
		if (gameObject != null)
		{
			if (gameObject.GetComponent<TsPositionFollower>() == null)
			{
				gameObject.AddComponent<TsPositionFollower>();
			}
		}
		else
		{
			gameObject = new GameObject(StageWorld.s_WorldAudioListener, new Type[]
			{
				typeof(TsPositionFollower)
			});
		}
		TsPositionFollower component2 = gameObject.GetComponent<TsPositionFollower>();
		gameObject.transform.parent = TsSceneSwitcher.Instance._GetBundle_RootSceneGO(TsSceneSwitcher.ESceneType.WorldScene).transform;
		component2.SetPositionFollower(NrTSingleton<NkCharManager>.Instance.GetMyCharObject().transform, Camera.main);
		return gameObject;
	}

	public static void ChageWorldAudioListener()
	{
		GameObject gameObject = GameObject.Find(StageWorld.s_WorldAudioListener);
		if (gameObject == null)
		{
			return;
		}
		TsPositionFollower component = gameObject.GetComponent<TsPositionFollower>();
		if (component == null)
		{
			return;
		}
		gameObject.transform.parent = TsSceneSwitcher.Instance._GetBundle_RootSceneGO(TsSceneSwitcher.ESceneType.WorldScene).transform;
		component.SetPositionFollower(NrTSingleton<NkCharManager>.Instance.GetMyCharObject().transform, Camera.main);
		TsAudioListenerSwitcher tsAudioListenerSwitcher = new TsAudioListenerSwitcher(gameObject);
		tsAudioListenerSwitcher.Switch();
	}

	public void DeleteWorldAudioListener()
	{
		TsAudioManager.Instance.InitAudioListenerSwitcher();
		GameObject gameObject = GameObject.Find(StageWorld.s_WorldAudioListener);
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public bool GetSupporter()
	{
		NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
		NkQuestManager instance2 = NrTSingleton<NkQuestManager>.Instance;
		USER_CURRENT_QUEST_INFO[] userCurrentQuestInfo = instance.GetUserCurrentQuestInfo();
		bool flag = false;
		for (byte b = 0; b < 10; b += 1)
		{
			if (userCurrentQuestInfo[(int)b].strQuestUnique != null)
			{
				string strQuestUnique = userCurrentQuestInfo[(int)b].strQuestUnique;
				CQuest questByQuestUnique = instance2.GetQuestByQuestUnique(strQuestUnique);
				if (questByQuestUnique != null)
				{
					flag = true;
					break;
				}
			}
		}
		return !flag;
	}

	[DebuggerHidden]
	private IEnumerator EndWorldLoad()
	{
		StageWorld.<EndWorldLoad>c__Iterator5D <EndWorldLoad>c__Iterator5D = new StageWorld.<EndWorldLoad>c__Iterator5D();
		<EndWorldLoad>c__Iterator5D.<>f__this = this;
		return <EndWorldLoad>c__Iterator5D;
	}

	[DebuggerHidden]
	private IEnumerator OpenMobileNotice()
	{
		return new StageWorld.<OpenMobileNotice>c__Iterator5E();
	}

	[DebuggerHidden]
	private IEnumerator BlueStacksCheck()
	{
		StageWorld.<BlueStacksCheck>c__Iterator5F <BlueStacksCheck>c__Iterator5F = new StageWorld.<BlueStacksCheck>c__Iterator5F();
		<BlueStacksCheck>c__Iterator5F.<>f__this = this;
		return <BlueStacksCheck>c__Iterator5F;
	}

	private void OnKickOutUser(object a_oObject)
	{
		if ((bool)a_oObject)
		{
			NrTSingleton<NrMainSystem>.Instance.QuitGame();
		}
		this.m_bEmulatorCheck = true;
	}
}
