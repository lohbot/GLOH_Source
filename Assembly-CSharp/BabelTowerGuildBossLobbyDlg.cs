using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityForms;

public class BabelTowerGuildBossLobbyDlg : Form
{
	private const float BOSSHP_BAR_WIDTH = 400f;

	private const float DAMAGE_BAR_WIDTH = 400f;

	public DrawTexture m_dtBoss;

	public Label m_lBossHpText;

	public Label m_lBossHp;

	public Label m_lBossName;

	public Label m_lBossTip;

	public Button m_bBattleStart;

	public Button m_bChangeInitiative;

	public Button m_bCancel;

	public DrawTexture m_dtHpBar;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "GuildBoss/DLG_GuildBossTip", G_ID.BABEL_GUILDBOSS_LOBBY_DLG, false, true);
		float x = GUICamera.width - base.GetSizeX();
		float y = 0f;
		base.SetLocation(x, y);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.DonotDepthChange(1005f);
	}

	public override void SetComponent()
	{
		this.m_lBossName = (base.GetControl("Label_BossName") as Label);
		this.m_dtBoss = (base.GetControl("DrawTexture_character") as DrawTexture);
		this.m_lBossHpText = (base.GetControl("Label_HP") as Label);
		this.m_lBossHpText.Text = "HP";
		this.m_lBossHp = (base.GetControl("Label_BossHp") as Label);
		this.m_dtHpBar = (base.GetControl("DrawTexture_BG2") as DrawTexture);
		this.m_lBossTip = (base.GetControl("Label_Tip") as Label);
		this.m_bBattleStart = (base.GetControl("button_Ready") as Button);
		Button expr_B0 = this.m_bBattleStart;
		expr_B0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_B0.Click, new EZValueChangedDelegate(this.OnClickStartGuildBoss));
		this.m_bChangeInitiative = (base.GetControl("button_Initiative") as Button);
		Button expr_ED = this.m_bChangeInitiative;
		expr_ED.Click = (EZValueChangedDelegate)Delegate.Combine(expr_ED.Click, new EZValueChangedDelegate(this.OnClickChangeInitiativeGuildBoss));
		this.m_bCancel = (base.GetControl("button_cancel") as Button);
		Button expr_12A = this.m_bCancel;
		expr_12A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_12A.Click, new EZValueChangedDelegate(this.OnClickCancelGuildBoss));
		this.DateSetting();
		PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
		if (plunderSolNumDlg != null)
		{
			plunderSolNumDlg.SetTitle(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1965"));
		}
	}

	public void OnClickCancelGuildBoss(IUIObject obj)
	{
		StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP;
		SoldierBatch.BABELTOWER_INFO.Init();
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}

	public void OnClickChangeInitiativeGuildBoss(IUIObject obj)
	{
		if (!SoldierBatch.SOLDIERBATCH.IsHeroGuildBossBatch())
		{
			string empty = string.Empty;
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("124"),
				"charname",
				@char.GetCharName()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		InitiativeSetDlg initiativeSetDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INITIATIVE_SET_DLG) as InitiativeSetDlg;
		if (initiativeSetDlg != null)
		{
			initiativeSetDlg.SetBatchSolList(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS);
		}
	}

	public void OnClickStartGuildBoss(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "FORMATION-COMPLETE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		short guildBossLastFloor = NrTSingleton<ContentsLimitManager>.Instance.GetGuildBossLastFloor();
		if (0 < guildBossLastFloor && guildBossLastFloor < SoldierBatch.GUILDBOSS_INFO.m_i16Floor)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("608"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!SoldierBatch.SOLDIERBATCH.IsHeroGuildBossBatch())
		{
			string empty = string.Empty;
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("124"),
				"charname",
				@char.GetCharName()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int tempCount = SoldierBatch.SOLDIERBATCH.GetTempCount();
		int num = 9;
		if (tempCount <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("181"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
		string text = string.Empty;
		string empty2 = string.Empty;
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (tempCount < num)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("147");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				text,
				"currentnum",
				tempCount,
				"maxnum",
				num
			});
			msgBoxUI.SetMsg(new YesDelegate(this.OnCompleteGuildBossBatch), null, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL, 2);
			return;
		}
		this.OnCompleteGuildBossBatch(null);
	}

	private void OnCompleteGuildBossBatch(object a_oObject)
	{
		if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
		{
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		clTempBattlePos[] tempBattlePosInfo = SoldierBatch.SOLDIERBATCH.GetTempBattlePosInfo();
		SoldierBatch.SOLDIERBATCH.SaveGuildBossBatchSolInfo();
		GS_NEWGUILD_BOSS_STARTBATTLE_REQ gS_NEWGUILD_BOSS_STARTBATTLE_REQ = new GS_NEWGUILD_BOSS_STARTBATTLE_REQ();
		gS_NEWGUILD_BOSS_STARTBATTLE_REQ.i16Floor = SoldierBatch.GUILDBOSS_INFO.m_i16Floor;
		gS_NEWGUILD_BOSS_STARTBATTLE_REQ.nCombinationUnique = NrTSingleton<SolCombination_BatchSelectInfoManager>.Instance.GetUserSelectedUniqeKey(0);
		int num = 0;
		for (int i = 0; i < 9; i++)
		{
			if (tempBattlePosInfo[i].m_nSolID > 0L)
			{
				gS_NEWGUILD_BOSS_STARTBATTLE_REQ.clSolBatchPosInfo[num].SolID = tempBattlePosInfo[i].m_nSolID;
				byte b = 0;
				byte nBattlePos = 0;
				SoldierBatch.GetCalcBattlePos((long)tempBattlePosInfo[i].m_nBattlePos, ref b, ref nBattlePos);
				gS_NEWGUILD_BOSS_STARTBATTLE_REQ.clSolBatchPosInfo[num].nBattlePos = nBattlePos;
				num++;
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_BOSS_STARTBATTLE_REQ, gS_NEWGUILD_BOSS_STARTBATTLE_REQ);
		NEWGUILD_MY_BOSS_ROOMINFO guildBossMyRoomInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossMyRoomInfo(SoldierBatch.GUILDBOSS_INFO.m_i16Floor);
		if (guildBossMyRoomInfo != null)
		{
			guildBossMyRoomInfo.ui8PlayState = 0;
		}
	}

	public void DateSetting()
	{
		BABEL_GUILDBOSS babelGuildBossinfo = NrTSingleton<BabelTowerManager>.Instance.GetBabelGuildBossinfo(SoldierBatch.GUILDBOSS_INFO.m_i16Floor);
		string text = string.Empty;
		string empty = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1808");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"count1",
			ANNUALIZED.Convert(SoldierBatch.GUILDBOSS_INFO.m_i32BossHP),
			"count2",
			ANNUALIZED.Convert(babelGuildBossinfo.m_nBossMaxHP)
		});
		this.m_lBossHp.Text = empty;
		float num = (float)SoldierBatch.GUILDBOSS_INFO.m_i32BossHP / (float)babelGuildBossinfo.m_nBossMaxHP;
		this.m_dtHpBar.SetSize(400f * num, this.m_dtHpBar.GetSize().y);
		string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(babelGuildBossinfo.m_nBossKind);
		this.m_lBossName.Text = name;
		string empty2 = string.Empty;
		string text2 = string.Empty;
		text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(babelGuildBossinfo.m_strTextExplain);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
		{
			text2,
			"targetname",
			name
		});
		this.m_lBossTip.Text = empty2;
		this.m_dtBoss.SetTexture(eCharImageType.LARGE, babelGuildBossinfo.m_nBossKind, 0, string.Empty);
		this.BattleUserCheck();
	}

	public void BattleUserCheck()
	{
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) != null)
		{
			PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
			plunderSolNumDlg.GuildBossBattleUserName();
		}
	}
}
