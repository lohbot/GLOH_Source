using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class PlunderStartAndReMatchDlg : Form
{
	private Button m_bPlunderStart;

	private Button m_bReMatch;

	private Button m_bExit;

	private Button m_bOK;

	private Button m_bStartBabel;

	private Button m_bReadyBabel;

	private Button m_bCancelBabel;

	private Button m_bStartMine;

	private Button m_bExitMine;

	private Button m_bInitiativeOpen;

	private Button m_btStartInfiBattle;

	private Button m_btReMatchInfiBattle;

	private Button m_btExit;

	private Button m_bStartExpedition;

	private Button m_bExitExpedition;

	private Button m_btApplyGuildWar;

	private Button m_btExitGuildWar;

	private bool m_nStartRematch;

	private float fDelayTime;

	private float m_fDelayTimeInfiBattle;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/dlg_pvp_button", G_ID.PLUNDER_STARTANDREMATCH_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_bPlunderStart = (base.GetControl("Button_PVP") as Button);
		Button expr_1C = this.m_bPlunderStart;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnPlunderStart));
		this.m_bReMatch = (base.GetControl("Button_Rematch") as Button);
		Button expr_59 = this.m_bReMatch;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.OnReMatch));
		this.m_bExit = (base.GetControl("Button_cancel") as Button);
		Button expr_96 = this.m_bExit;
		expr_96.Click = (EZValueChangedDelegate)Delegate.Combine(expr_96.Click, new EZValueChangedDelegate(this.OnExit));
		this.m_bOK = (base.GetControl("Button_Formation") as Button);
		Button expr_D3 = this.m_bOK;
		expr_D3.Click = (EZValueChangedDelegate)Delegate.Combine(expr_D3.Click, new EZValueChangedDelegate(this.OnClickOK));
		this.m_bStartBabel = (base.GetControl("Button_Start") as Button);
		Button expr_110 = this.m_bStartBabel;
		expr_110.Click = (EZValueChangedDelegate)Delegate.Combine(expr_110.Click, new EZValueChangedDelegate(this.OnClickStartBabel));
		this.m_bReadyBabel = (base.GetControl("Button_Ready") as Button);
		Button expr_14D = this.m_bReadyBabel;
		expr_14D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_14D.Click, new EZValueChangedDelegate(this.OnClickReady));
		this.m_bCancelBabel = (base.GetControl("Button_cancel2") as Button);
		Button expr_18A = this.m_bCancelBabel;
		expr_18A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_18A.Click, new EZValueChangedDelegate(this.OnClickCancelBabel));
		this.m_bStartMine = (base.GetControl("Button_MINE_Ok") as Button);
		Button expr_1C7 = this.m_bStartMine;
		expr_1C7.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C7.Click, new EZValueChangedDelegate(this.OnClickStartMine));
		this.m_bExitMine = (base.GetControl("Button_MINE_Cancel") as Button);
		Button expr_204 = this.m_bExitMine;
		expr_204.Click = (EZValueChangedDelegate)Delegate.Combine(expr_204.Click, new EZValueChangedDelegate(this.OnClickExitMine));
		this.m_bInitiativeOpen = (base.GetControl("Button_MINE_Initiative") as Button);
		Button expr_241 = this.m_bInitiativeOpen;
		expr_241.Click = (EZValueChangedDelegate)Delegate.Combine(expr_241.Click, new EZValueChangedDelegate(this.OnClickInitiativeOpen));
		this.m_btStartInfiBattle = (base.GetControl("Button_InfiBattle") as Button);
		Button expr_27E = this.m_btStartInfiBattle;
		expr_27E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_27E.Click, new EZValueChangedDelegate(this.OnClickStartInfiBattle));
		this.m_btReMatchInfiBattle = (base.GetControl("Button_InfiRematch") as Button);
		Button expr_2BB = this.m_btReMatchInfiBattle;
		expr_2BB.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2BB.Click, new EZValueChangedDelegate(this.OnClickReMatchInfiBattle));
		this.m_btExit = (base.GetControl("Button_cancel3") as Button);
		Button expr_2F8 = this.m_btExit;
		expr_2F8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2F8.Click, new EZValueChangedDelegate(this.OnClickExitInfiBattle));
		this.m_bStartExpedition = (base.GetControl("Button_EXPEDITION_Ok") as Button);
		Button expr_335 = this.m_bStartExpedition;
		expr_335.Click = (EZValueChangedDelegate)Delegate.Combine(expr_335.Click, new EZValueChangedDelegate(this.OnClickStartExpedition));
		this.m_bExitExpedition = (base.GetControl("Button_EXPEDITION_Cancel") as Button);
		Button expr_372 = this.m_bExitExpedition;
		expr_372.Click = (EZValueChangedDelegate)Delegate.Combine(expr_372.Click, new EZValueChangedDelegate(this.OnClickExitExpedition));
		this.m_btApplyGuildWar = (base.GetControl("Button_GuildWar_Ok") as Button);
		this.m_btApplyGuildWar.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickApplyGuildWar));
		this.m_btExitGuildWar = (base.GetControl("Button_GuildWar_Cancel") as Button);
		this.m_btExitGuildWar.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickExitGuildWar));
		float x = GUICamera.width / 2f - base.GetSize().x / 2f;
		float y = GUICamera.height - base.GetSize().y;
		base.SetLocation(x, y, base.GetLocation().z);
		base.ShowLayer(1);
	}

	public override void InitData()
	{
	}

	public override void Update()
	{
		if (this.m_nStartRematch)
		{
			if (Time.time - this.fDelayTime > 1f)
			{
				this.m_nStartRematch = false;
				this.m_bReMatch.SetEnabled(true);
				this.fDelayTime = Time.time;
			}
			else
			{
				this.m_bReMatch.SetEnabled(false);
			}
		}
		if (0f < this.m_fDelayTimeInfiBattle && this.m_fDelayTimeInfiBattle < Time.time)
		{
			this.m_btReMatchInfiBattle.SetEnabled(true);
			this.m_fDelayTimeInfiBattle = 0f;
		}
	}

	public void SetButtonMode()
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PLUNDER)
		{
			base.ShowLayer(1);
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("71");
			string empty = string.Empty;
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			int level = kMyCharInfo.GetLevel();
			long num;
			if (level > 50)
			{
				num = (long)(level * (level - COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_MATCHGOLD2)));
			}
			else
			{
				num = (long)(level * COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_MATCHGOLD));
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"gold",
				num.ToString()
			});
			this.m_bReMatch.Text = empty;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo != null)
			{
				if (SoldierBatch.BABELTOWER_INFO.IsBabelLeader(charPersonInfo.GetPersonID()))
				{
					base.ShowLayer(3, 5);
				}
				else
				{
					base.ShowLayer(4, 5);
				}
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			base.ShowLayer(6);
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE)
		{
			base.ShowLayer(7);
			this.m_btReMatchInfiBattle.SetEnabled(true);
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			base.ShowLayer(7);
			this.m_btReMatchInfiBattle.SetEnabled(false);
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			base.ShowLayer(8);
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			base.ShowLayer(9);
		}
		else
		{
			base.ShowLayer(2);
		}
	}

	public bool SetBabelReadyState()
	{
		bool flag = false;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return flag;
		}
		if (SoldierBatch.BABELTOWER_INFO.IsBabelLeader(charPersonInfo.GetPersonID()))
		{
			return flag;
		}
		if (!SoldierBatch.BABELTOWER_INFO.IsReadyBattle(charPersonInfo.GetPersonID()))
		{
			flag = true;
		}
		string text = string.Empty;
		if (flag)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("685");
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("662");
		}
		this.m_bReadyBabel.SetText(text);
		return flag;
	}

	public void UpdateBabelReadyState()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		if (SoldierBatch.BABELTOWER_INFO.IsBabelLeader(charPersonInfo.GetPersonID()))
		{
			return;
		}
		bool flag = SoldierBatch.BABELTOWER_INFO.IsReadyBattle(charPersonInfo.GetPersonID());
		string text = string.Empty;
		if (flag)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("685");
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("662");
		}
		this.m_bReadyBabel.SetText(text);
	}

	public void OnPlunderStart(IUIObject obj)
	{
		if (!SoldierBatch.SOLDIERBATCH.IsHeroBatch())
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
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_PLUNDER_COOLTIME);
		long dueDateTick = PublicMethod.GetDueDateTick(charSubData);
		if (dueDateTick > 0L)
		{
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
			string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("24");
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.OnStartMatch), null, textFromMessageBox, textFromMessageBox2, eMsgType.MB_OK_CANCEL);
			return;
		}
		int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
		int maxSolArray = SoldierBatch.SOLDIERBATCH.GetMaxSolArray();
		if (solBatchNum < maxSolArray)
		{
			string textFromMessageBox3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
			string textFromMessageBox4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("147");
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				textFromMessageBox4,
				"currentnum",
				solBatchNum,
				"maxnum",
				maxSolArray
			});
			MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI2.SetMsg(new YesDelegate(this.OnStartMatch), null, textFromMessageBox3, empty2, eMsgType.MB_OK_CANCEL);
			return;
		}
		this.OnStartMatch(null);
	}

	private void OnStartMatch(object a_oObject)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_PLUNDER_DELAYTIME);
		long curTime = PublicMethod.GetCurTime();
		long num = charSubData - curTime;
		if (num > 0L)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("136"),
				"timestring",
				PublicMethod.ConvertTime(num)
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_PLUNDER_BATTLE_START_REQ gS_PLUNDER_BATTLE_START_REQ = new GS_PLUNDER_BATTLE_START_REQ();
		int num2 = 0;
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		if (soldierList == null)
		{
			return;
		}
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS) > 0L)
			{
				gS_PLUNDER_BATTLE_START_REQ.m_nSolID[num2] = nkSoldierInfo.GetSolID();
				gS_PLUNDER_BATTLE_START_REQ.m_nSolSubData[num2] = nkSoldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS);
				num2++;
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS) > 0L)
			{
				gS_PLUNDER_BATTLE_START_REQ.m_nSolID[num2] = current.GetSolID();
				gS_PLUNDER_BATTLE_START_REQ.m_nSolSubData[num2] = current.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS);
				num2++;
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_BATTLE_START_REQ, gS_PLUNDER_BATTLE_START_REQ);
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERSOLLIST_DLG);
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "START", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void OnExit(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		GS_PLUNDER_MATCH_PLAYER_REQ gS_PLUNDER_MATCH_PLAYER_REQ = new GS_PLUNDER_MATCH_PLAYER_REQ();
		gS_PLUNDER_MATCH_PLAYER_REQ.m_PersonID = charPersonInfo.GetPersonID();
		gS_PLUNDER_MATCH_PLAYER_REQ.m_nMode = 2;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_MATCH_PLAYER_REQ, gS_PLUNDER_MATCH_PLAYER_REQ);
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		myCharInfo.PlunderMoney = 0L;
		myCharInfo.PlunderCharName = string.Empty;
		myCharInfo.PlunderCharLevel = 0;
	}

	public void OnReMatch(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_PLUNDER_DELAYTIME);
		long curTime = PublicMethod.GetCurTime();
		long num = charSubData - curTime;
		if (num > 0L)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("136"),
				"timestring",
				PublicMethod.ConvertTime(num)
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int level = myCharInfo.GetLevel();
		long num2;
		if (level > 50)
		{
			num2 = (long)(level * (level - COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_MATCHGOLD2)));
		}
		else
		{
			num2 = (long)(level * COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_MATCHGOLD));
		}
		if (num2 > myCharInfo.m_Money)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		NewLoaingDlg newLoaingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE) as NewLoaingDlg;
		if (newLoaingDlg != null)
		{
			newLoaingDlg.SetLoadingPageEffect(SoldierBatch.SOLDIERBATCH.PlunderLoading);
			this.Hide();
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		GS_PLUNDER_MATCH_PLAYER_REQ gS_PLUNDER_MATCH_PLAYER_REQ = new GS_PLUNDER_MATCH_PLAYER_REQ();
		gS_PLUNDER_MATCH_PLAYER_REQ.m_PersonID = charPersonInfo.GetPersonID();
		gS_PLUNDER_MATCH_PLAYER_REQ.m_nMode = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_MATCH_PLAYER_REQ, gS_PLUNDER_MATCH_PLAYER_REQ);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "REMATCHING", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		SoldierBatch.SOLDIERBATCH.InitEnemyChar();
		NrTSingleton<NrMainSystem>.Instance.CleanUpImmediate();
		this.m_nStartRematch = true;
		this.fDelayTime = Time.time;
	}

	public void OnClickOK(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "FORMATION-COMPLETE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		SoldierBatch.SOLDIERBATCH.CheckSameBattlePos(SoldierBatch.SOLDIER_BATCH_MODE);
		if (!SoldierBatch.SOLDIERBATCH.IsHeroBatch())
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
		int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
		int num;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
		{
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
			{
				return;
			}
			num = 6;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			num = 3;
		}
		else
		{
			num = SoldierBatch.SOLDIERBATCH.GetMaxSolArray();
		}
		if (solBatchNum < num)
		{
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
			string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("147");
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				textFromMessageBox2,
				"currentnum",
				solBatchNum,
				"maxnum",
				num
			});
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.OnCompleteBatch), null, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL);
			return;
		}
		this.OnCompleteBatch(null);
	}

	private void OnCompleteBatch(object a_oObject)
	{
		byte nMode = 0;
		long nObjBatch = 0L;
		eSOL_SUBDATA eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_STATUSVALUE;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP)
		{
			StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP;
			nMode = 0;
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP;
			nMode = 1;
			nObjBatch = SoldierBatch.SOLDIERBATCH.GetObjectDataToSubData().nSubData;
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
		{
			StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP;
			nMode = 2;
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP)
		{
			StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP;
			nMode = 3;
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP)
		{
			StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP;
			nMode = 4;
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENSE_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP;
			nMode = 5;
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			SoldierBatch.SOLDIERBATCH.SavePvpMakeup2SolInfo();
			NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
			StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2;
			nMode = 6;
		}
		GS_PLUNDER_SET_SOLMAKEUP_REQ gS_PLUNDER_SET_SOLMAKEUP_REQ = new GS_PLUNDER_SET_SOLMAKEUP_REQ();
		gS_PLUNDER_SET_SOLMAKEUP_REQ.m_nMode = nMode;
		gS_PLUNDER_SET_SOLMAKEUP_REQ.m_nObjBatch = nObjBatch;
		if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			Dictionary<int, long> dictionary = new Dictionary<int, long>();
			int num = 0;
			NrSoldierList soldierList = charPersonInfo.GetSoldierList();
			if (soldierList == null)
			{
				return;
			}
			NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
			for (int i = 0; i < kSolInfo.Length; i++)
			{
				NkSoldierInfo nkSoldierInfo = kSolInfo[i];
				if (nkSoldierInfo.GetSolSubData(eSOL_SUBDATA) > 0L)
				{
					gS_PLUNDER_SET_SOLMAKEUP_REQ.m_nSolID[num] = nkSoldierInfo.GetSolID();
					gS_PLUNDER_SET_SOLMAKEUP_REQ.m_nSolSubData[num] = nkSoldierInfo.GetSolSubData(eSOL_SUBDATA);
					if (eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
					{
						if (dictionary.ContainsKey(nkSoldierInfo.GetCharKind()))
						{
							return;
						}
						dictionary.Add(nkSoldierInfo.GetCharKind(), nkSoldierInfo.GetSolID());
					}
					num++;
				}
			}
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null)
			{
				return;
			}
			foreach (NkSoldierInfo current in readySolList.GetList().Values)
			{
				if (current.GetSolSubData(eSOL_SUBDATA) > 0L)
				{
					gS_PLUNDER_SET_SOLMAKEUP_REQ.m_nSolID[num] = current.GetSolID();
					gS_PLUNDER_SET_SOLMAKEUP_REQ.m_nSolSubData[num] = current.GetSolSubData(eSOL_SUBDATA);
					if (eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
					{
						if (dictionary.ContainsKey(current.GetCharKind()))
						{
							return;
						}
						dictionary.Add(current.GetCharKind(), current.GetSolID());
					}
					num++;
				}
			}
		}
		else
		{
			Dictionary<int, long> dictionary2 = new Dictionary<int, long>();
			int num2 = 0;
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo == null)
			{
				return;
			}
			for (int j = 0; j < 3; j++)
			{
				long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COLOSSEUMBATCH1 + j);
				if (charSubData != 0L)
				{
					SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
					sUBDATA_UNION.nSubData = charSubData;
					int n32SubData_ = sUBDATA_UNION.n32SubData_0;
					int n32SubData_2 = sUBDATA_UNION.n32SubData_1;
					byte b = 0;
					byte b2 = 0;
					SoldierBatch.GetCalcBattlePos((long)n32SubData_, ref b, ref b2);
					if (b2 >= 0 && b2 < 9)
					{
						if (n32SubData_2 > 0)
						{
							if (dictionary2.ContainsKey(n32SubData_2))
							{
								return;
							}
							dictionary2.Add(n32SubData_2, (long)n32SubData_2);
							gS_PLUNDER_SET_SOLMAKEUP_REQ.m_nSolID[num2] = (long)n32SubData_2;
							gS_PLUNDER_SET_SOLMAKEUP_REQ.m_nSolSubData[num2] = charSubData;
							num2++;
						}
					}
				}
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_SET_SOLMAKEUP_REQ, gS_PLUNDER_SET_SOLMAKEUP_REQ);
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}

	public void OnClickStartBabel(IUIObject obj)
	{
		byte partyCount = SoldierBatch.BABELTOWER_INFO.GetPartyCount();
		byte readyPersonCount = SoldierBatch.BABELTOWER_INFO.GetReadyPersonCount();
		if (!SoldierBatch.SOLDIERBATCH.IsHeroBabelBatch())
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
		if (partyCount != readyPersonCount)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("182"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
		if (solBatchNum <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("181"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int solBatchNum2 = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
		if (SoldierBatch.BABELTOWER_INFO.Count <= 0)
		{
			SoldierBatch.BABELTOWER_INFO.Count = 1;
		}
		byte count = SoldierBatch.BABELTOWER_INFO.Count;
		int num;
		if (count == 1)
		{
			num = (int)(12 / count);
		}
		else
		{
			num = (int)(12 / count + 1);
		}
		if (solBatchNum2 < num)
		{
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("21");
			string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("74");
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				textFromMessageBox2,
				"currentnum",
				solBatchNum2,
				"maxnum",
				num
			});
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.OnStartBabel), null, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL);
			return;
		}
		this.OnStartBabel(null);
	}

	private void OnStartBabel(object a_oObject)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		if (SoldierBatch.BABELTOWER_INFO.GetPartyCount() == 1)
		{
			SoldierBatch.SOLDIERBATCH.SaveBatchSolInfo();
		}
		GS_BABELTOWER_START_REQ gS_BABELTOWER_START_REQ = new GS_BABELTOWER_START_REQ();
		gS_BABELTOWER_START_REQ.nBabelRoomIndex = SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
		gS_BABELTOWER_START_REQ.nPersonID = charPersonInfo.GetPersonID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_START_REQ, gS_BABELTOWER_START_REQ);
	}

	public void OnClickReady(IUIObject obj)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		if (!SoldierBatch.BABELTOWER_INFO.IsReadyBattle(charPersonInfo.GetPersonID()))
		{
			if (!SoldierBatch.SOLDIERBATCH.IsHeroBabelBatch())
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
			int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
			if (SoldierBatch.BABELTOWER_INFO.Count <= 0)
			{
				SoldierBatch.BABELTOWER_INFO.Count = 1;
			}
			byte count = SoldierBatch.BABELTOWER_INFO.Count;
			int num;
			if (count == 1)
			{
				num = (int)(12 / count);
			}
			else
			{
				num = (int)(12 / count + 1);
			}
			if (solBatchNum < num)
			{
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("21");
				string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("74");
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					textFromMessageBox2,
					"currentnum",
					solBatchNum,
					"maxnum",
					num
				});
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI.SetMsg(new YesDelegate(this.OnReadyBabel), null, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL);
				return;
			}
		}
		this.OnReadyBabel(null);
	}

	private void OnReadyBabel(object a_oObject)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		bool bReady = this.SetBabelReadyState();
		GS_BABELTOWER_READY_REQ gS_BABELTOWER_READY_REQ = new GS_BABELTOWER_READY_REQ();
		gS_BABELTOWER_READY_REQ.nBabelRoomIndex = SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
		gS_BABELTOWER_READY_REQ.nPersonID = charPersonInfo.GetPersonID();
		gS_BABELTOWER_READY_REQ.bReady = bReady;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_READY_REQ, gS_BABELTOWER_READY_REQ);
	}

	public void OnClickCancelBabel(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		byte mode;
		if (SoldierBatch.BABELTOWER_INFO.IsBabelLeader(charPersonInfo.GetPersonID()))
		{
			mode = 0;
		}
		else
		{
			mode = 1;
		}
		GS_BABELTOWER_LEAVE_REQ gS_BABELTOWER_LEAVE_REQ = new GS_BABELTOWER_LEAVE_REQ();
		gS_BABELTOWER_LEAVE_REQ.mode = mode;
		gS_BABELTOWER_LEAVE_REQ.nLeavePersonID = charPersonInfo.GetPersonID();
		gS_BABELTOWER_LEAVE_REQ.nBabelRoomIndex = SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_LEAVE_REQ, gS_BABELTOWER_LEAVE_REQ);
		SoldierBatch.BABELTOWER_INFO.Init();
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}

	public void OnClickExitMine(IUIObject obj)
	{
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
		this.Close();
	}

	public void OnClickInitiativeOpen(IUIObject obj)
	{
		int tempCount = SoldierBatch.SOLDIERBATCH.GetTempCount();
		if (tempCount <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("740"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		InitiativeSetDlg initiativeSetDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INITIATIVE_SET_DLG) as InitiativeSetDlg;
		if (initiativeSetDlg != null)
		{
			initiativeSetDlg.SetBatchSolList(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE);
		}
	}

	public void OnClickStartMine(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "FORMATION-COMPLETE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		int tempCount = SoldierBatch.SOLDIERBATCH.GetTempCount();
		int num = 5;
		if (tempCount <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("181"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
		string text = string.Empty;
		string empty = string.Empty;
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (tempCount < num)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("147");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"currentnum",
				tempCount,
				"maxnum",
				num
			});
			msgBoxUI.SetMsg(new YesDelegate(this.OnCompleteMineBatch), null, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
			return;
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("157");
		msgBoxUI.SetMsg(new YesDelegate(this.OnCompleteMineBatch), null, textFromMessageBox, text, eMsgType.MB_OK_CANCEL);
	}

	private void OnCompleteMineBatch(object a_oObject)
	{
		if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
		{
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		long guildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		clTempBattlePos[] tempBattlePosInfo = SoldierBatch.SOLDIERBATCH.GetTempBattlePosInfo();
		GS_SET_SOLDIER_MILLITARY_REQ gS_SET_SOLDIER_MILLITARY_REQ = new GS_SET_SOLDIER_MILLITARY_REQ();
		gS_SET_SOLDIER_MILLITARY_REQ.m_nMode = 0;
		gS_SET_SOLDIER_MILLITARY_REQ.m_nGuildID = guildID;
		gS_SET_SOLDIER_MILLITARY_REQ.m_nMineID = SoldierBatch.MINE_INFO.m_i64MineID;
		gS_SET_SOLDIER_MILLITARY_REQ.m_nMineGrade = SoldierBatch.MINE_INFO.m_nMineGrade;
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			if (tempBattlePosInfo[i].m_nSolID > 0L)
			{
				gS_SET_SOLDIER_MILLITARY_REQ.MilitaryInfo[num].SolID = tempBattlePosInfo[i].m_nSolID;
				byte b = 0;
				byte solPosType = 0;
				SoldierBatch.GetCalcBattlePos((long)tempBattlePosInfo[i].m_nBattlePos, ref b, ref solPosType);
				gS_SET_SOLDIER_MILLITARY_REQ.MilitaryInfo[num].SolPosType = solPosType;
				gS_SET_SOLDIER_MILLITARY_REQ.MilitaryInfo[num].MilitaryUnique = 0;
				num++;
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SET_SOLDIER_MILLITARY_REQ, gS_SET_SOLDIER_MILLITARY_REQ);
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}

	public void OnClickStartInfiBattle(IUIObject obj)
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_INFIBATTLE && SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_INFIBATTLE_COOLTIME);
		long curTime = PublicMethod.GetCurTime();
		long time = charSubData - curTime;
		if (curTime < charSubData)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("136"),
				"timestring",
				PublicMethod.ConvertTime(time)
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			int infiBattleCount = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleCount;
			int num = infiBattleCount - (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharDetail(21);
			if (num < 0)
			{
				num = 0;
			}
			TsLog.LogWarning(" Match Count , {0} : {1}", new object[]
			{
				num,
				infiBattleCount
			});
			if (num <= 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("684"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("684"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
		int maxSolArray = SoldierBatch.SOLDIERBATCH.GetMaxSolArray();
		if (solBatchNum < maxSolArray)
		{
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
			string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("147");
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				textFromMessageBox2,
				"currentnum",
				solBatchNum,
				"maxnum",
				maxSolArray
			});
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.OnInfiStartMatch), null, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL);
			return;
		}
		this.OnInfiStartMatch(null);
	}

	private void OnInfiStartMatch(object a_oObject)
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			GS_INFIBATTLE_START_REQ gS_INFIBATTLE_START_REQ = new GS_INFIBATTLE_START_REQ();
			int num = 0;
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			NrSoldierList soldierList = charPersonInfo.GetSoldierList();
			if (soldierList == null)
			{
				return;
			}
			NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
			for (int i = 0; i < kSolInfo.Length; i++)
			{
				NkSoldierInfo nkSoldierInfo = kSolInfo[i];
				if (nkSoldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE) > 0L)
				{
					gS_INFIBATTLE_START_REQ.i64SolID[num] = nkSoldierInfo.GetSolID();
					gS_INFIBATTLE_START_REQ.i64BattlePos[num] = nkSoldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE);
					num++;
				}
			}
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null)
			{
				return;
			}
			foreach (NkSoldierInfo current in readySolList.GetList().Values)
			{
				if (current.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE) > 0L)
				{
					if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
					{
						gS_INFIBATTLE_START_REQ.i8PracticeBattleMode = 1;
					}
					else
					{
						gS_INFIBATTLE_START_REQ.i8PracticeBattleMode = 0;
					}
					gS_INFIBATTLE_START_REQ.i64SolID[num] = current.GetSolID();
					gS_INFIBATTLE_START_REQ.i64BattlePos[num] = current.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE);
					num++;
				}
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INFIBATTLE_START_REQ, gS_INFIBATTLE_START_REQ);
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERSOLLIST_DLG);
			}
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "START", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public void OnClickReMatchInfiBattle(IUIObject obj)
	{
		NewLoaingDlg newLoaingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE) as NewLoaingDlg;
		if (newLoaingDlg != null)
		{
			newLoaingDlg.SetLoadingPageEffect(SoldierBatch.SOLDIERBATCH.PlunderLoading);
			this.Hide();
		}
		this.Send_InfiBattleMatch(0);
		SoldierBatch.SOLDIERBATCH.InitEnemyChar();
		NrTSingleton<NrMainSystem>.Instance.CleanUpImmediate();
		this.m_fDelayTimeInfiBattle = Time.time + 5f;
		this.m_btReMatchInfiBattle.SetEnabled(false);
	}

	public void OnClickExitInfiBattle(IUIObject obj)
	{
		this.ExitInfiBattle();
	}

	public void ExitInfiBattle()
	{
		NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleCharLevel = 0;
		this.Send_InfiBattleMatch(1);
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
		base.CloseNow();
	}

	public void Send_InfiBattleMatch(byte byMode)
	{
		GS_INFIBATTLE_MATCH_REQ gS_INFIBATTLE_MATCH_REQ = new GS_INFIBATTLE_MATCH_REQ();
		gS_INFIBATTLE_MATCH_REQ.i8Mode = byMode;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INFIBATTLE_MATCH_REQ, gS_INFIBATTLE_MATCH_REQ);
	}

	public void OnClickExitExpedition(IUIObject obj)
	{
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
		this.Close();
	}

	public void OnClickStartExpedition(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "FORMATION-COMPLETE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		int tempCount = SoldierBatch.SOLDIERBATCH.GetTempCount();
		int num = 15;
		EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade((byte)SoldierBatch.EXPEDITION_INFO.m_eExpeditionGrade);
		if (expeditionDataFromGrade != null)
		{
			num = expeditionDataFromGrade.Expedition_SolBatch_Array * 3;
		}
		if (tempCount <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("181"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
		string text = string.Empty;
		string empty = string.Empty;
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (tempCount < num)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("147");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"currentnum",
				tempCount,
				"maxnum",
				num
			});
			msgBoxUI.SetMsg(new YesDelegate(this.OnCompleteExpedition), null, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
			return;
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("157");
		msgBoxUI.SetMsg(new YesDelegate(this.OnCompleteExpedition), null, textFromMessageBox, text, eMsgType.MB_OK_CANCEL);
	}

	private void OnCompleteExpedition(object a_oObject)
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
		GS_EXPEDITION_START_REQ gS_EXPEDITION_START_REQ = new GS_EXPEDITION_START_REQ();
		gS_EXPEDITION_START_REQ.ui8ExpeditionGrade = (byte)SoldierBatch.EXPEDITION_INFO.m_eExpeditionGrade;
		int num = 0;
		for (int i = 0; i < 15; i++)
		{
			if (tempBattlePosInfo[i].m_nSolID > 0L)
			{
				gS_EXPEDITION_START_REQ.i64SolID[num] = tempBattlePosInfo[i].m_nSolID;
				byte b = 0;
				byte b2 = 0;
				SoldierBatch.GetCalcBattlePos((long)tempBattlePosInfo[i].m_nBattlePos, ref b, ref b2);
				gS_EXPEDITION_START_REQ.i8PosIndex[num] = b;
				gS_EXPEDITION_START_REQ.i16BattlePos[num] = (short)b2;
				num++;
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_START_REQ, gS_EXPEDITION_START_REQ);
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}

	public void OnClickApplyGuildWar(object a_oObject)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "FORMATION-COMPLETE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		int tempCount = SoldierBatch.SOLDIERBATCH.GetTempCount();
		int num = 5;
		if (tempCount <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("181"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
		string text = string.Empty;
		string empty = string.Empty;
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (tempCount < num)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("147");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"currentnum",
				tempCount,
				"maxnum",
				num
			});
			msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxApplyGuildWar), null, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
			return;
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("157");
		msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxApplyGuildWar), null, textFromMessageBox, text, eMsgType.MB_OK_CANCEL);
	}

	public void MsgBoxApplyGuildWar(object a_oObject)
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
		GS_GUILDWAR_APPLY_REQ gS_GUILDWAR_APPLY_REQ = new GS_GUILDWAR_APPLY_REQ();
		gS_GUILDWAR_APPLY_REQ.ui8RaidUnique = SoldierBatch.GuildWarRaidUnique;
		gS_GUILDWAR_APPLY_REQ.ui8RaidBattlePos = SoldierBatch.GuildWarRaidBattlePos;
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			gS_GUILDWAR_APPLY_REQ.i64SolID[i] = 0L;
			gS_GUILDWAR_APPLY_REQ.ui8SolPosIndex[i] = 0;
			if (tempBattlePosInfo[i].m_nSolID > 0L)
			{
				gS_GUILDWAR_APPLY_REQ.i64SolID[num] = tempBattlePosInfo[i].m_nSolID;
				byte b = 0;
				byte b2 = 0;
				SoldierBatch.GetCalcBattlePos((long)tempBattlePosInfo[i].m_nBattlePos, ref b, ref b2);
				gS_GUILDWAR_APPLY_REQ.ui8SolPosIndex[num] = (byte)num;
				gS_GUILDWAR_APPLY_REQ.i16BattlePos[num] = (short)b2;
				num++;
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_GUILDWAR_APPLY_REQ, gS_GUILDWAR_APPLY_REQ);
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}

	public void OnClickExitGuildWar(IUIObject obj)
	{
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}
}
