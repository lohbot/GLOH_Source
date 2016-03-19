using GameMessage;
using GameMessage.Private;
using Ndoors.Framework.CoTask;
using Ndoors.Framework.Stage;
using SERVICE;
using StageHelper;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityForms;

public class StageInitialize : AStage
{
	private bool m_bCalledNextStage;

	private static bool IsTableLoaded;

	public override Scene.Type SceneType()
	{
		return Scene.Type.INITIALIZE;
	}

	public override void OnPrepareSceneChange()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.LOGIN_SELECT_PLATFORM_DLG);
		NrLoadPageScreen.LoginLatestChar = (NrTSingleton<NrMainSystem>.Instance.GetLatestPersonID() > 0L);
		if (NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea() == eSERVICE_AREA.SERVICE_ANDROID_BANDNAVER || NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea() == eSERVICE_AREA.SERVICE_ANDROID_BANDGOOGLE)
		{
			NrLoadPageScreen.LoginLatestChar = false;
		}
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		if (NrLoadPageScreen.LoginLatestChar)
		{
			NrLoadPageScreen.StepUpMain(3);
		}
		else
		{
			NrLoadPageScreen.StepUpMain(2);
		}
		NrLoadPageScreen.ShowHideLoadingImg(true);
		if (TsPlatform.IsMobile)
		{
			NmMainFrameWork.DeleteImage();
		}
	}

	public override void OnEnter()
	{
		TsLog.Log("====== {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		Scene.ChangeSceneType(this.SceneType());
		NrTSingleton<FiveRocksEventManager>.Instance.StageFunnelsLog(Scene.CurScene);
		this.m_bCalledNextStage = false;
		CommonTasks.ClearAssetBundleResources(true);
		base.StartTaskSerial(CommonTasks.InitializeChangeScene());
		UnityEngine.Debug.LogError(string.Concat(new object[]
		{
			"!!!!!!!!!!!!!!!!!!! IsTableLoaded ",
			StageInitialize.IsTableLoaded,
			" NrMainSystem.Instance.m_ReLogin ",
			NrTSingleton<NrMainSystem>.Instance.m_ReLogin
		}));
		if (!StageInitialize.IsTableLoaded && !NrTSingleton<NrMainSystem>.Instance.m_ReLogin)
		{
			TsAudioManager.Container.LoadXML();
			base.StartTaskSerial(this._DownloadTextMgrTables());
			base.StartTaskSerial(this._Download1stTables());
			base.StartTaskSerial(this._Download2ndTables());
		}
		base.StartTaskSerial(this._StageProcess());
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(false));
		base.StartTaskSerial(CommonTasks.MemoryCleaning(false, 8));
	}

	public override void OnExit()
	{
		TsLog.Log("====== {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
	}

	protected override void OnUpdateAfterStagePrework()
	{
		if (NrTSingleton<NkCharManager>.Instance.CharacterListSetComplete && !this.m_bCalledNextStage)
		{
			if (NrLoadPageScreen.LoginLatestChar)
			{
				if ((NrCharUser)NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(NrTSingleton<NrMainSystem>.Instance.GetLatestPersonID()) == null)
				{
					MsgBoxUI msgBoxUI = (MsgBoxUI)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG);
					if (msgBoxUI != null)
					{
						msgBoxUI.SetMsg(new YesDelegate(this._OnMessageBoxOK_QuitGame), null, "경고", "캐릭터 정보를 읽어오는데 실패하였습니다...\r\n어플을 재실행해주세요.", eMsgType.MB_OK);
						NrLoadPageScreen.ShowHideLoadingImg(false);
					}
					TsLog.LogWarning("CID {0} User not found!", new object[]
					{
						NrTSingleton<NrMainSystem>.Instance.GetLatestPersonID()
					});
				}
				else
				{
					TsLog.LogWarning("StagePacketMsgHandler.CONNECT_GAMESERVER_REQ=============", new object[0]);
					MsgHandler.Handle("Req_CONNECT_GAMESERVER_REQ", new object[]
					{
						NrTSingleton<NrMainSystem>.Instance.GetLatestPersonID()
					});
					NrTSingleton<NkQuestManager>.Instance.SortingQuestInGroup();
					FacadeHandler.MoveStage(Scene.Type.PREPAREGAME);
				}
			}
			else
			{
				TsLog.LogWarning("==========NEXT STAGE=============", new object[0]);
				FacadeHandler.MoveStage(Scene.Type.SELECTCHAR);
			}
			this.m_bCalledNextStage = true;
		}
	}

	private void _OnMessageBoxOK_QuitGame(object a_oObject)
	{
		NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
		NrTSingleton<NrMainSystem>.Instance.QuitGame();
	}

	public bool OnGameServerConnected()
	{
		if (NrTSingleton<NrMainSystem>.Instance.GetLatestPersonID() == 0L)
		{
			MsgBoxUI msgBoxUI = (MsgBoxUI)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG);
			if (msgBoxUI != null)
			{
				msgBoxUI.SetMsg(new YesDelegate(this._OnMessageBoxOK_QuitGame), null, "경고", "lastpersonid가 0이다,,...\r\n어플을 재실행해주세요.", eMsgType.MB_OK);
			}
		}
		TsLog.LogWarning("{0}.Rcv_WS_CONNECT_GAMESERVER_ACK", new object[]
		{
			StageSystem.GetCurrentStageName()
		});
		FacadeHandler.Req_GS_AUTH_SESSION_REQ(NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID, NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_siAuthSessionKey, NrTSingleton<NrMainSystem>.Instance.GetLatestPersonID(), 0, 402);
		return true;
	}

	public bool GS_AUTH_SESSION_ACK()
	{
		TsLog.Log("{0}.GS_AUTH_SESSION_ACK", new object[]
		{
			StageSystem.GetCurrentStageName()
		});
		FacadeHandler.MoveStage(Scene.Type.SELECTCHAR);
		return true;
	}

	[DebuggerHidden]
	private IEnumerator _StageProcess()
	{
		StageInitialize.<_StageProcess>c__Iterator35 <_StageProcess>c__Iterator = new StageInitialize.<_StageProcess>c__Iterator35();
		<_StageProcess>c__Iterator.<>f__this = this;
		return <_StageProcess>c__Iterator;
	}

	private void _RequestConnectWorldServer()
	{
		NrLoadPageScreen.IncreaseProgress(1f);
		NrTSingleton<NrNetProcess>.Instance.RequestToGameServer(NrTSingleton<NrMainSystem>.Instance.m_strWorldServerIP, NrTSingleton<NrMainSystem>.Instance.m_nWorldServerPort);
	}

	private BaseCoTask _DownloadTextMgrTables()
	{
		TableInspector tableInspector = new TableInspector(this);
		NrFontColorTable tbl = new NrFontColorTable(CDefinePath.FONT_COLOR_PATH);
		tableInspector.Regist(tbl);
		NrTSingleton<NrTextMgr>.Instance.Register(ref tableInspector, tbl);
		return tableInspector;
	}

	private BaseCoTask _Download1stTables()
	{
		TableInspector tableInspector = new TableInspector(this, true);
		tableInspector.Regist(COMMON_CONSTANT_Manager.GetInstance());
		tableInspector.Regist(new NkTableWeaponTypeInfo());
		NkTableCharKindAttackInfo nkTableCharKindAttackInfo = new NkTableCharKindAttackInfo();
		NkTableCharKindClassInfo nkTableCharKindClassInfo = new NkTableCharKindClassInfo();
		NrTableCharKindInfo nrTableCharKindInfo = new NrTableCharKindInfo();
		NkTableCharKindStatInfo nkTableCharKindStatInfo = new NkTableCharKindStatInfo();
		NkTableCharKindMonStatInfo nkTableCharKindMonStatInfo = new NkTableCharKindMonStatInfo();
		NkTableCharKindMonsterInfo nkTableCharKindMonsterInfo = new NkTableCharKindMonsterInfo();
		NrTableCharKindNPCInfo nrTableCharKindNPCInfo = new NrTableCharKindNPCInfo();
		NkTableCharKindSolGradeInfo nkTableCharKindSolGradeInfo = new NkTableCharKindSolGradeInfo();
		NkTableCharKindAniInfo tbl = new NkTableCharKindAniInfo();
		tableInspector.Regist(nkTableCharKindAttackInfo);
		tableInspector.RegistWait(nkTableCharKindAttackInfo, nkTableCharKindClassInfo);
		tableInspector.RegistWait(nkTableCharKindClassInfo, nrTableCharKindInfo);
		tableInspector.RegistWait(nrTableCharKindInfo, nkTableCharKindStatInfo);
		tableInspector.RegistWait(nkTableCharKindStatInfo, nkTableCharKindMonStatInfo);
		tableInspector.RegistWait(nkTableCharKindMonStatInfo, nkTableCharKindMonsterInfo);
		tableInspector.RegistWait(nkTableCharKindMonsterInfo, nrTableCharKindNPCInfo);
		tableInspector.RegistWait(nrTableCharKindNPCInfo, nkTableCharKindSolGradeInfo);
		tableInspector.RegistWait(nkTableCharKindSolGradeInfo, tbl);
		NkTableItemTypeInfo nkTableItemTypeInfo = new NkTableItemTypeInfo();
		tableInspector.RegistWait(nrTableCharKindInfo, nkTableItemTypeInfo);
		tableInspector.RegistWait(nkTableItemTypeInfo, new NrTable_Item_Accessory());
		tableInspector.RegistWait(nkTableItemTypeInfo, new NrTable_Item_Armor());
		tableInspector.RegistWait(nkTableItemTypeInfo, new NrTable_Item_Box());
		tableInspector.RegistWait(nkTableItemTypeInfo, new NrTable_Item_Material());
		tableInspector.RegistWait(nkTableItemTypeInfo, new NrTable_Item_Quest());
		tableInspector.RegistWait(nkTableItemTypeInfo, new NrTable_Item_Second_Equip());
		tableInspector.RegistWait(nkTableItemTypeInfo, new NrTable_Item_Supplies());
		tableInspector.RegistWait(nkTableItemTypeInfo, new NrTable_Item_Ticket());
		tableInspector.RegistWait(nkTableItemTypeInfo, new NrTable_Item_Weapon());
		tableInspector.RegistWait(nkTableItemTypeInfo, new NrTable_ItemReduce());
		tableInspector.Regist(new NrTable_ITEM_COMPOSE());
		tableInspector.Regist(new NrTable_Item_Rank(CDefinePath.s_strItemRankURL));
		tableInspector.Regist(new NrTable_ITEM_BOX_GROUP_DATA());
		NkTableGateInfo nkTableGateInfo = new NkTableGateInfo();
		tableInspector.Regist(nkTableGateInfo);
		tableInspector.RegistWait(nkTableGateInfo, new NkTableWorldMapInfo());
		tableInspector.RegistWait(nkTableGateInfo, new NkTableLocalMapInfo());
		tableInspector.RegistWait(nkTableGateInfo, new NkTableMapInfo());
		tableInspector.RegistWait(nkTableGateInfo, new NkTableMapUnit());
		tableInspector.RegistWait(nrTableCharKindInfo, new NrTableEco(CDefinePath.ECO_PATH));
		tableInspector.RegistWait(nrTableCharKindInfo, new NrTableEcoTalk(CDefinePath.ECO_TALK_PATH));
		tableInspector.Regist(BASE_BATTLE_MAP_Manager.GetInstance());
		tableInspector.Regist(BASE_BATTLE_POS_Manager.GetInstance());
		tableInspector.Regist(BASE_BATTLE_GridData_Manager.GetInstance());
		tableInspector.Regist(new NrTable_ChallengeTimeTable(CDefinePath.CHALLENGE_TIMETABLE_PATH));
		tableInspector.Regist(new NrTable_ChallengeTable(CDefinePath.CHALLENGE_TABLE_PATH));
		tableInspector.Regist(new NrTable_ChallengeEquipTable(CDefinePath.CHALLENGE_EQUIPTABLE_PATH));
		tableInspector.Regist(new NrTable_PointTable(CDefinePath.POINT_TABLE_PATH));
		tableInspector.Regist(new NrTable_JewelryTable(CDefinePath.JEWELRY_TABLE_PATH));
		tableInspector.Regist(new NrTable_PointLimitTable(CDefinePath.POINTLIMIT_TABLE_PATH));
		tableInspector.Regist(new NrTable_MythicSolTable(CDefinePath.MYTHICSOL_TABLE_PATH));
		tableInspector.Regist(new NrTable_ExplorationTable(CDefinePath.EXPLORATION_TABLE_PATH));
		tableInspector.Regist(new NrTable_ReservedWord(CDefinePath.RESERVEDWORD_TABLE_PATH));
		NrTableClientNpcInfo tbl2 = new NrTableClientNpcInfo(CDefinePath.QUEST_CLIENTNPC_INFO);
		NrTableQuestCommon nrTableQuestCommon = new NrTableQuestCommon(CDefinePath.QUEST_COMMON_PATH);
		NrTableQuestReward tbl3 = new NrTableQuestReward(CDefinePath.QUEST_REWARD_PATH);
		NrTableQuestGroup tbl4 = new NrTableQuestGroup(CDefinePath.QUEST_GROUP_PATH);
		NrTableQuestDropItem tbl5 = new NrTableQuestDropItem(CDefinePath.QUEST_DROP_ITEM_PATH);
		NrTableAutoQuest tbl6 = new NrTableAutoQuest(CDefinePath.QUEST_ATUOQUEST_PATH);
		tableInspector.RegistWait(nrTableCharKindInfo, nrTableQuestCommon);
		tableInspector.RegistWait(nrTableQuestCommon, tbl3);
		tableInspector.RegistWait(nrTableQuestCommon, tbl4);
		tableInspector.RegistWait(nrTableQuestCommon, tbl2);
		tableInspector.RegistWait(nrTableQuestCommon, tbl5);
		tableInspector.RegistWait(nrTableQuestCommon, tbl6);
		tableInspector.Regist(new NrTable_GameGuideInfo());
		tableInspector.Regist(new NrTable_AdventureInfo());
		tableInspector.Regist(new NrTableSolGuide());
		tableInspector.Regist(new NrTableRecommend_Reward());
		tableInspector.Regist(new NrTableSupporter_Reward());
		tableInspector.Regist(new NrTableDailyGift());
		tableInspector.Regist(new NrTableSolExtractRate());
		tableInspector.Regist(new NrTableTranscendence_Cost());
		tableInspector.Regist(new NrTableTranscendence_Rate());
		tableInspector.Regist(new NrTableTranscendence_Sol());
		tableInspector.Regist(new NrTableTranscendence_FailReward());
		tableInspector.Regist(new NrTableGMHelpInfo());
		tableInspector.Regist(new NrTable_SolWarehouseInfo());
		tableInspector.Regist(new NrTable_CharSpend());
		tableInspector.Regist(new NrTable_ReincarnationInfo());
		tableInspector.Regist(new NrTableEvolution_EXP_Penalty());
		tableInspector.Regist(new NrTable_BountyInfo());
		tableInspector.Regist(new NrTable_BountyEco());
		tableInspector.Regist(new NrTable_AgitInfo());
		tableInspector.Regist(new NrTable_AgitNPC());
		return tableInspector;
	}

	private BaseCoTask _Download2ndTables()
	{
		TableInspector tableInspector = new TableInspector(this, true);
		tableInspector.Regist(new NkTableEffectInfo());
		tableInspector.Regist(new NkTableBulletInfo());
		tableInspector.Regist(new NkTableCameraSettings());
		tableInspector.Regist(new NrLEVEL_EXPTable(CDefinePath.TEXT_LEVEL_EXP));
		tableInspector.Regist(new NrGuild_EXPTable(CDefinePath.TEXT_GUILD_EXP));
		NrTable_BattleSkill_Base nrTable_BattleSkill_Base = new NrTable_BattleSkill_Base();
		NrTable_BattleSkill_DetailInclude nrTable_BattleSkill_DetailInclude = new NrTable_BattleSkill_DetailInclude();
		NrTable_BattleSkill_Detail nrTable_BattleSkill_Detail = new NrTable_BattleSkill_Detail();
		NrTable_BattleSkill_TrainingInclude nrTable_BattleSkill_TrainingInclude = new NrTable_BattleSkill_TrainingInclude();
		NrTable_BattleSkill_Training nrTable_BattleSkill_Training = new NrTable_BattleSkill_Training();
		NrTable_BattleSkill_Icon tbl = new NrTable_BattleSkill_Icon();
		tableInspector.Regist(nrTable_BattleSkill_Base);
		tableInspector.RegistWait(nrTable_BattleSkill_Base, nrTable_BattleSkill_DetailInclude);
		tableInspector.RegistWait(nrTable_BattleSkill_DetailInclude, nrTable_BattleSkill_Detail);
		tableInspector.RegistWait(nrTable_BattleSkill_Detail, nrTable_BattleSkill_TrainingInclude);
		tableInspector.RegistWait(nrTable_BattleSkill_TrainingInclude, nrTable_BattleSkill_Training);
		tableInspector.RegistWait(nrTable_BattleSkill_Training, tbl);
		tableInspector.Regist(new NrTable_ITEM_REFORGE());
		tableInspector.Regist(new NrTable_ITEM_SELL());
		tableInspector.Regist(new NrTable_BATTLE_SREWARD());
		tableInspector.Regist(new NrTable_BATTLE_BABEL_SREWARD());
		tableInspector.Regist(BATTLE_CONSTANT_Manager.GetInstance());
		tableInspector.Regist(BATTLE_EMOTICON_Manager.GetInstance());
		tableInspector.Regist(COLOSSEUM_CONSTANT_Manager.GetInstance());
		tableInspector.Regist(new COLOSSEUM_CHALLENGE_DATA());
		tableInspector.Regist(new NkTableIndunInfo());
		tableInspector.Regist(BASE_EVENTTRIGGER_MANAGER.GetInstance());
		tableInspector.Regist(new NrTable_Friend_InvitePersonCnt(CDefinePath.INVITE_PERSONCOUNT_URL));
		tableInspector.Regist(new NrTable_FacebookFeed());
		tableInspector.Regist(new NrTable_ComposeExpData());
		tableInspector.Regist(new NrTable_PlunderSupportGold());
		tableInspector.Regist(new NrTable_PlunderObjectInfo());
		tableInspector.Regist(new NrTable_BurnningEvent());
		tableInspector.Regist(new NrTable_ColosseumRankReward());
		tableInspector.Regist(new NrTable_ColosseumGradeInfo());
		tableInspector.Regist(new NkTableSoldierEvolutionExp());
		tableInspector.Regist(new NkTableSoldierTicketInfo());
		tableInspector.Regist(new NrTable_BABELTOWER_DATA());
		tableInspector.Regist(new NrTable_BABELGUILDBOSS_DATA());
		tableInspector.Regist(new NrTable_Item_Mall_Item());
		tableInspector.Regist(new NrTableItemSkillInfo());
		tableInspector.Regist(MINE_CONSTANT_Manager.GetInstance());
		tableInspector.Regist(new BASE_MINE_DATA());
		tableInspector.Regist(new BASE_MINE_CREATE_DATA());
		tableInspector.Regist(EVENT_DAILY_DUNGEON_DATA.GetInstance());
		tableInspector.Regist(new BASE_FRIENDCOUNTLIMIT_DATA());
		tableInspector.Regist(new BASE_EXPEDITION_DATA());
		tableInspector.Regist(new BASE_EXPEDITION_CREATE_DATA());
		tableInspector.Regist(EXPEDITION_CONSTANT_MANAGER.GetInstance());
		tableInspector.Regist(new TableData_GameWebCallURLInfo());
		tableInspector.Regist(NewGuildConstant_Manager.GetInstance());
		TsLog.LogWarning("_Download2ndTables!!!!!!!!!!!!!!!!!!!--------2222222222222222222222222222222222222", new object[0]);
		return tableInspector;
	}
}
