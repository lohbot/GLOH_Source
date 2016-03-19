using GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle_Control_Dlg : Form
{
	private enum eBATTLECONTROLLAYER
	{
		eBATTLECONTROLLAYER_NORMAL = 1,
		eBATTLECONTROLLAYER_ONLYRETREAT
	}

	private enum eSKILL_COUNT
	{
		eSKILL_COUNT_01,
		eSKILL_COUNT_MAX
	}

	private DrawTexture[] m_dwSkillIcon;

	private Button[] m_btSkill;

	private DrawTexture m_dwAngerPrg;

	private DrawTexture m_dwNeedAngerPrg;

	private DrawTexture m_dwAngerFrame;

	private Label m_lbMove;

	private Label m_lbTurnOver;

	private Label m_lbRetreat;

	private Label m_lbRetreatOnly;

	private Label m_lbFriendHelp;

	private Label m_lbAutoBattle;

	private Button m_btMove;

	private Button m_btTurnOver;

	private Button m_btRetreat;

	private Button m_btRetreatOnly;

	private Button m_btFriendHelp;

	private Button m_btAutoBattle;

	private Button m_btChat;

	private Button m_btSkillInfo;

	private Label m_lbNeedSkillAngerNum;

	private Label m_lbNowAngerNum;

	private Button m_btTarget;

	private Label m_lbTarget;

	private DrawTexture m_dwTargetBG07_0;

	private DrawTexture m_dwTargetBG07_1;

	private DrawTexture m_dwTargetBG07_2;

	private Box menuNotice;

	private Label m_lbTargetNotice;

	private BATTLESKILL_DETAIL[] m_BSkillDetail;

	private GameObject[] m_goSkillActiveEffect;

	private GameObject[] m_goSkillClickEffect;

	private GameObject m_goAngerActiveEffect;

	private int m_nNowGetAngerlyPoint;

	private float m_fAngerProgressHeight;

	private float m_fBeforeAngerPercent = -1f;

	private float m_fBlockTime = 0.5f;

	private float m_fStartBlockTime;

	private bool bMakeOne;

	private int m_nMaxAngryPoint = 1000;

	private int m_nTargetButtonCount;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/Dlg_battle_Control", G_ID.BATTLE_CONTROL_DLG, false);
		form.AlwaysUpdate = true;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		this._SetDialogPos();
	}

	public void ShowUIGuide()
	{
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 1f);
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.UIGUIDE_DLG) as UI_UIGuide;
		if (uI_UIGuide == null)
		{
			return;
		}
		Vector3 v = new Vector2(base.GetLocationX() + this.m_btSkill[0].GetLocationX() + 60f, base.GetLocationY() + this.m_btAutoBattle.GetLocationY() - 25f);
		Vector2 x = new Vector2(base.GetLocationX() + this.m_btSkill[0].GetLocationX(), base.GetLocationY() + this.m_btAutoBattle.GetLocationY() - 55f);
		uI_UIGuide.Move(v, x);
	}

	public Vector2 GetSkillButtonPos()
	{
		Vector2 zero = Vector2.zero;
		zero.x = base.GetLocationX() + this.m_btSkill[0].GetLocationX();
		zero.y = base.GetLocationY() + this.m_btSkill[0].GetLocationY();
		return zero;
	}

	public override void SetComponent()
	{
		this.SetOrderControl();
		this.m_btSkillInfo = (base.GetControl("BT_SkillDetail") as Button);
		if (this.m_btSkillInfo)
		{
			Button expr_32 = this.m_btSkillInfo;
			expr_32.Click = (EZValueChangedDelegate)Delegate.Combine(expr_32.Click, new EZValueChangedDelegate(this.OnClickSkillInfo));
			this.m_btSkillInfo.controlIsEnabled = true;
			this.m_btSkillInfo.EffectAni = false;
			this.m_btSkillInfo.Visible = true;
		}
		this.m_dwAngerFrame = (base.GetControl("DT_AngerFrame") as DrawTexture);
		Texture2D texture = CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Texture/AngerBG") as Texture2D;
		this.m_dwAngerFrame.SetTexture(texture);
		this.m_lbNeedSkillAngerNum = (base.GetControl("LB_SkillAnger") as Label);
		this.m_lbNowAngerNum = (base.GetControl("LB_AngerNum") as Label);
		this.m_lbNowAngerNum.SetText(this.m_nNowGetAngerlyPoint.ToString());
		this.m_dwSkillIcon = new DrawTexture[1];
		this.m_btSkill = new Button[1];
		this.m_BSkillDetail = new BATTLESKILL_DETAIL[1];
		this.m_goSkillActiveEffect = new GameObject[1];
		this.m_goSkillClickEffect = new GameObject[1];
		this.m_dwAngerPrg = (base.GetControl("DT_AngerBG") as DrawTexture);
		this.m_fAngerProgressHeight = this.m_dwAngerPrg.GetSize().y;
		this.m_dwAngerPrg.Visible = false;
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			if (Battle.BabelPartyCount == 1)
			{
				this.m_nMaxAngryPoint = 1500;
			}
			else
			{
				this.m_nMaxAngryPoint = 2000;
			}
		}
		else if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS)
		{
			this.m_nMaxAngryPoint = 1500;
		}
		else
		{
			this.m_nMaxAngryPoint = 1000;
		}
		this.m_dwNeedAngerPrg = (base.GetControl("DT_AngerGage") as DrawTexture);
		this.SetAngryText();
		this.m_dwAngerPrg.SetLocation(this.m_dwAngerPrg.GetLocationX(), this.m_dwAngerPrg.GetLocationY(), 39f);
		this.m_dwNeedAngerPrg.SetLocation(this.m_dwNeedAngerPrg.GetLocationX(), this.m_dwNeedAngerPrg.GetLocationY(), 37f);
		for (int i = 0; i < 1; i++)
		{
			string name = string.Format("skill_icon0{0}", (i + 1).ToString());
			this.m_dwSkillIcon[i] = (base.GetControl(name) as DrawTexture);
			name = string.Format("btn0{0}", (i + 1).ToString());
			this.m_btSkill[i] = (base.GetControl(name) as Button);
			this.m_btSkill[i].AddMouseOverDelegate(new EZValueChangedDelegate(this.MouseOverSkill));
			this.m_btSkill[i].AddMouseOutDelegate(new EZValueChangedDelegate(this.MouseOutSkill));
			Button expr_2E8 = this.m_btSkill[i];
			expr_2E8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2E8.Click, new EZValueChangedDelegate(this.OnClickBattleSkill));
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_SKILL_ACTIVE", this.m_btSkill[i], this.m_btSkill[i].GetSize());
			this.m_btSkill[i].AddGameObjectDelegate(new EZGameObjectDelegate(this.ButtonAddEffectDelegate));
		}
		this.UpdateBattleSkillData();
		base.AllHideLayer();
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
		{
			base.ShowLayer(2);
		}
		else
		{
			base.ShowLayer(1);
		}
		this._SetDialogPos();
		this.SetAngergaugeFX_Click(false);
	}

	public void SetOrderControl()
	{
		this.m_btMove = (base.GetControl("btn_Move") as Button);
		this.m_btTurnOver = (base.GetControl("btn_TurnOver") as Button);
		this.m_btRetreat = (base.GetControl("btn_Retreat") as Button);
		this.m_btRetreatOnly = (base.GetControl("btn_Retreat2") as Button);
		this.m_btFriendHelp = (base.GetControl("btn_Friendhelp") as Button);
		this.m_btAutoBattle = (base.GetControl("btn_AutoBattle") as Button);
		this.m_btChat = (base.GetControl("Button_Chat") as Button);
		this.m_lbMove = (base.GetControl("Label_control_Move") as Label);
		this.m_lbTurnOver = (base.GetControl("Label_control_TurnOver") as Label);
		this.m_lbRetreat = (base.GetControl("Label_control_Retreat") as Label);
		this.m_lbRetreatOnly = (base.GetControl("Label_Retreat2") as Label);
		this.m_lbFriendHelp = (base.GetControl("Label_control_Friendhelp") as Label);
		this.m_lbAutoBattle = (base.GetControl("Label_control_Auto") as Label);
		this.m_btTarget = (base.GetControl("btn_target") as Button);
		this.m_lbTarget = (base.GetControl("Label_target") as Label);
		this.m_dwTargetBG07_0 = (base.GetControl("DT_ButtonBG07") as DrawTexture);
		this.m_dwTargetBG07_1 = (base.GetControl("DT_ControlBG07") as DrawTexture);
		this.m_dwTargetBG07_2 = (base.GetControl("DT_ControlBG07_1") as DrawTexture);
		this.menuNotice = (base.GetControl("Box_Notice") as Box);
		this.menuNotice.Visible = false;
		this.m_lbTargetNotice = (base.GetControl("Label_Label6") as Label);
		this.m_lbTargetNotice.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("925"));
		this.m_lbTargetNotice.Visible = false;
		if (this.m_btMove)
		{
			Button expr_200 = this.m_btMove;
			expr_200.Click = (EZValueChangedDelegate)Delegate.Combine(expr_200.Click, new EZValueChangedDelegate(this.OnClickMove));
			this.m_btMove.controlIsEnabled = true;
			this.m_btMove.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1384");
			this.m_btMove.EffectAni = false;
		}
		if (this.m_btTurnOver)
		{
			Button expr_269 = this.m_btTurnOver;
			expr_269.Click = (EZValueChangedDelegate)Delegate.Combine(expr_269.Click, new EZValueChangedDelegate(this.OnClickTurnOver));
			this.m_btTurnOver.controlIsEnabled = true;
			this.m_btTurnOver.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1388");
			this.m_btTurnOver.EffectAni = false;
		}
		if (this.m_btRetreat)
		{
			Button expr_2D2 = this.m_btRetreat;
			expr_2D2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2D2.Click, new EZValueChangedDelegate(this.OnClickRetreat));
			this.m_btRetreat.controlIsEnabled = true;
			this.m_btRetreat.EffectAni = false;
			if (!Battle.BATTLE.Observer)
			{
				this.m_btRetreat.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1389");
			}
			else
			{
				this.m_btRetreat.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1045");
			}
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
			{
				Button expr_36A = this.m_btRetreat;
				expr_36A.Click = (EZValueChangedDelegate)Delegate.Remove(expr_36A.Click, new EZValueChangedDelegate(this.OnClickRetreat));
				Button expr_391 = this.m_btRetreat;
				expr_391.Click = (EZValueChangedDelegate)Delegate.Combine(expr_391.Click, new EZValueChangedDelegate(this.OnClickUsingDisable));
			}
		}
		if (this.m_btRetreatOnly)
		{
			Button expr_3C8 = this.m_btRetreatOnly;
			expr_3C8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3C8.Click, new EZValueChangedDelegate(this.OnClickRetreat));
			this.m_btRetreatOnly.controlIsEnabled = true;
			this.m_btRetreatOnly.EffectAni = false;
			if (!Battle.BATTLE.Observer)
			{
				this.m_btRetreatOnly.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1389");
			}
			else
			{
				this.m_btRetreatOnly.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1045");
			}
		}
		if (this.m_btFriendHelp)
		{
			Button expr_45F = this.m_btFriendHelp;
			expr_45F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_45F.Click, new EZValueChangedDelegate(this.OnClickContinueFriendHelp));
			this.m_btFriendHelp.controlIsEnabled = true;
			this.m_btFriendHelp.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1386");
			this.m_btFriendHelp.EffectAni = false;
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
			{
				Button expr_4C9 = this.m_btFriendHelp;
				expr_4C9.Click = (EZValueChangedDelegate)Delegate.Remove(expr_4C9.Click, new EZValueChangedDelegate(this.OnClickContinueFriendHelp));
				Button expr_4F0 = this.m_btFriendHelp;
				expr_4F0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4F0.Click, new EZValueChangedDelegate(this.OnClickUsingDisable));
			}
		}
		if (this.m_btAutoBattle)
		{
			Button expr_527 = this.m_btAutoBattle;
			expr_527.Click = (EZValueChangedDelegate)Delegate.Combine(expr_527.Click, new EZValueChangedDelegate(this.OnClickAutoBattle));
			this.m_btAutoBattle.controlIsEnabled = true;
			this.m_btAutoBattle.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1385");
			this.m_btAutoBattle.EffectAni = false;
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
			{
				Button expr_591 = this.m_btAutoBattle;
				expr_591.Click = (EZValueChangedDelegate)Delegate.Remove(expr_591.Click, new EZValueChangedDelegate(this.OnClickAutoBattle));
				Button expr_5B8 = this.m_btAutoBattle;
				expr_5B8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_5B8.Click, new EZValueChangedDelegate(this.OnClickUsingDisable));
			}
		}
		if (this.m_btTarget && !Battle.BATTLE.Observer && (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY) && !TsPlatform.IsIPhone)
		{
			Button expr_629 = this.m_btTarget;
			expr_629.Click = (EZValueChangedDelegate)Delegate.Combine(expr_629.Click, new EZValueChangedDelegate(this.OnClickTarget));
			this.m_btTarget.controlIsEnabled = true;
			this.m_btTarget.EffectAni = false;
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_SKILL_ACTIVE", this.m_btTarget, this.m_btTarget.GetSize());
			BATTLE_CONSTANT_Manager instance = BATTLE_CONSTANT_Manager.GetInstance();
			int num = (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_AGGROTARGET_COUNT);
			if (num == 0)
			{
				num = 3;
			}
			Battle.BATTLE.SetTargetBtCount(num);
		}
		if (this.m_lbMove)
		{
			if (!TsPlatform.IsMobile)
			{
				this.m_lbMove.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1392"));
			}
			else
			{
				this.m_lbMove.Visible = false;
			}
		}
		if (this.m_lbTurnOver)
		{
			if (!TsPlatform.IsMobile)
			{
				this.m_lbTurnOver.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1396"));
			}
			else
			{
				this.m_lbTurnOver.Visible = false;
			}
		}
		if (this.m_lbRetreat)
		{
			if (!TsPlatform.IsMobile)
			{
				this.m_lbRetreat.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1397"));
			}
			else
			{
				this.m_lbRetreat.Visible = false;
			}
		}
		if (this.m_lbRetreatOnly)
		{
			if (!TsPlatform.IsMobile)
			{
				this.m_lbRetreatOnly.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1397"));
			}
			else
			{
				this.m_lbRetreatOnly.Visible = false;
			}
		}
		if (this.m_lbFriendHelp)
		{
			if (!TsPlatform.IsMobile)
			{
				this.m_lbFriendHelp.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1398"));
			}
			else
			{
				this.m_lbFriendHelp.Visible = false;
			}
		}
		if (this.m_lbAutoBattle)
		{
			if (!TsPlatform.IsMobile)
			{
				this.m_lbAutoBattle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1395"));
			}
			else
			{
				this.m_lbAutoBattle.Visible = false;
			}
		}
		if (this.m_btChat)
		{
			Button expr_859 = this.m_btChat;
			expr_859.Click = (EZValueChangedDelegate)Delegate.Combine(expr_859.Click, new EZValueChangedDelegate(this.OnClickChat));
		}
		this.SetOberver();
	}

	public void SetOberver()
	{
		if (Battle.BATTLE.Observer)
		{
			if (this.m_btMove)
			{
				this.m_btMove.controlIsEnabled = false;
			}
			if (this.m_btTurnOver)
			{
				this.m_btTurnOver.controlIsEnabled = false;
			}
			if (this.m_btRetreat)
			{
				this.m_btRetreat.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1045");
			}
			if (this.m_btFriendHelp)
			{
				this.m_btFriendHelp.controlIsEnabled = false;
			}
			if (this.m_btAutoBattle)
			{
				this.m_btAutoBattle.controlIsEnabled = false;
			}
		}
	}

	public void OnClickSkillInfo(IUIObject obj)
	{
		NkBattleChar nkBattleChar = Battle.BATTLE.SelectBattleSkillChar();
		if (nkBattleChar == null)
		{
			return;
		}
		Battle_SkillInfoDlg battle_SkillInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_SKILLINFO_DLG) as Battle_SkillInfoDlg;
		if (battle_SkillInfoDlg != null)
		{
			int num = 0;
			for (int i = 0; i < 1; i++)
			{
				num = nkBattleChar.GetSoldierInfo().SelectBattleSkillByWeapon(i + 1);
				if (num > 0)
				{
					break;
				}
			}
			int battleSkillLevel = nkBattleChar.GetSoldierInfo().GetBattleSkillLevel(num);
			battle_SkillInfoDlg.SetDataForBattle(nkBattleChar.GetSoldierInfo(), num, battleSkillLevel);
		}
	}

	public void OnClickMove(IUIObject obj)
	{
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		if (!Battle.BATTLE.IsEnableOrderTime)
		{
			return;
		}
		if (Battle.BATTLE.CurrentTurnAlly == Battle.BATTLE.MyAlly)
		{
			this.SetAngergaugeFX_Click(false);
			Battle.BATTLE.Init_BattleSkill_Input(true);
			Battle.BATTLE.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_CHANGEPOS;
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("364"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "BATTLE", "FORMATION-CHANGE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public void OnClickContinueFriendHelp(IUIObject obj)
	{
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("156"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
		{
			return;
		}
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COMMUNITY_DLG))
		{
			CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
			if (communityUI_DLG != null)
			{
				this.SetAngergaugeFX_Click(false);
				Battle.BATTLE.Init_BattleSkill_Input(true);
				communityUI_DLG.RequestCommunityData(eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE);
				communityUI_DLG.HideSort();
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COMMUNITY_DLG);
		}
		Battle.BATTLE.GRID_MANAGER.ActiveAttackGridCanTarget();
	}

	public void OnClickTurnNext(IUIObject obj)
	{
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		if (Battle.BATTLE != null)
		{
			this.SetAngergaugeFX_Click(false);
			Battle.BATTLE.Init_BattleSkill_Input(true);
			Battle.BATTLE.SelectNextChar();
		}
	}

	public void OnClickTurnOver(IUIObject obj)
	{
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		if (Battle.BATTLE != null)
		{
			this.RequestTurnOver();
		}
	}

	public void OnClickRetreat(IUIObject obj)
	{
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		this.SetAngergaugeFX_Click(false);
		Battle.BATTLE.Init_BattleSkill_Input(true);
		Battle.BATTLE.GRID_MANAGER.ActiveAttackGridCanTarget();
		this.RequestRetreat();
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("battle_retreat");
	}

	public void OnClickTarget(IUIObject obj)
	{
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		Battle.BATTLE.ClickCheckTargetBt();
		if (Battle.BATTLE.GetCheckTargetBt())
		{
		}
	}

	public void OnClickAutoBattle(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.UIGUIDE_DLG);
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.RequestBabelMacroStopAndAutoBattle), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("187"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("188"), eMsgType.MB_OK_CANCEL);
			return;
		}
		if (Battle.BATTLE != null)
		{
			this.SetAngergaugeFX_Click(false);
			Battle.BATTLE.Init_BattleSkill_Input(true);
			Battle.BATTLE.GRID_MANAGER.ActiveAttackGridCanTarget();
			Battle.BATTLE.Send_GS_BATTLE_AUTO_REQ();
		}
	}

	public void OnClickChat(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.CHAT_MOBILE_SUB_DLG);
	}

	public void OnClickUsingDisable(IUIObject obj)
	{
		Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("156"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
	}

	public void RequestBabelMacroStopAndAutoBattle(object a_oObject)
	{
		if (Battle.BATTLE != null)
		{
			this.SetAngergaugeFX_Click(false);
			Battle.BATTLE.Init_BattleSkill_Input(true);
			Battle.BATTLE.GRID_MANAGER.ActiveAttackGridCanTarget();
			Battle.BATTLE.Send_GS_BATTLE_AUTO_REQ();
		}
	}

	public void RequestRetreat()
	{
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		if (!Battle.BATTLE.Observer && Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		string message = string.Empty;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1068");
		if (!Battle.BATTLE.Observer)
		{
			if (Battle.BATTLE.ExitReservation)
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("124");
			}
			else if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER)
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("167");
			}
			else
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("125");
			}
		}
		else
		{
			textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1767");
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("102");
		}
		msgBoxUI.SetMsg(new YesDelegate(this.OnRetreatkOK), null, textFromInterface, message, eMsgType.MB_OK_CANCEL);
	}

	public void OnRetreatkOK(object a_oObject)
	{
		Battle.BATTLE.Send_GS_BATTLE_CLOSE_REQ();
	}

	public void RequestTurnOver()
	{
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		if (!Battle.BATTLE.Observer && Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		string message = string.Empty;
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("182");
		message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("183");
		msgBoxUI.SetMsg(new YesDelegate(this.OnTurnOverOK), null, textFromMessageBox, message, eMsgType.MB_OK_CANCEL);
	}

	public void OnTurnOverOK(object a_oObject)
	{
		this.SetAngergaugeFX_Click(false);
		Battle.BATTLE.Init_BattleSkill_Input(true);
		Battle.BATTLE.Send_GS_BF_HOPE_TO_ENDTURN_REQ();
	}

	public override void Show()
	{
		base.AllHideLayer();
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
		{
			base.ShowLayer(2);
		}
		else
		{
			base.ShowLayer(1);
		}
		base.Show();
	}

	public override void OnClose()
	{
		base.OnClose();
		if (this.m_goAngerActiveEffect != null)
		{
			this.m_goAngerActiveEffect.SetActive(false);
			UnityEngine.Object.Destroy(this.m_goAngerActiveEffect);
			this.m_goAngerActiveEffect = null;
		}
	}

	public void UpdateBattleSkillData()
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
		{
			return;
		}
		if (Battle.BATTLE.ControlAngergauge != null && this.m_goAngerActiveEffect == null && this.visible && !this.bMakeOne)
		{
			this.m_goAngerActiveEffect = (GameObject)UnityEngine.Object.Instantiate(Battle.BATTLE.ControlAngergauge, Vector3.zero, Quaternion.identity);
			NkUtil.SetAllChildLayer(this.m_goAngerActiveEffect, GUICamera.UILayer);
			base.InteractivePanel.MakeChild(this.m_goAngerActiveEffect.gameObject);
			float x = this.m_dwAngerPrg.GetLocationX() + (GUICamera.width - base.GetSizeX()) + 103f;
			float y = -(this.m_dwAngerPrg.GetLocationY() + GUICamera.height - base.GetSizeY() + 93f);
			float z = base.GetLocation().z + 21f;
			this.m_goAngerActiveEffect.transform.position = new Vector3(x, y, z);
			this.m_goAngerActiveEffect.SetActive(true);
			this.bMakeOne = true;
		}
		this.SetAngryText();
		if (Battle.BATTLE.GetCurrentSelectChar() == null)
		{
			if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BUBBLEGAMEGUIDE_DLG) != null)
			{
				NrTSingleton<FormsManager>.Instance.Hide(G_ID.BUBBLEGAMEGUIDE_DLG);
			}
			for (int i = 0; i < 1; i++)
			{
				this.m_BSkillDetail[i] = null;
				this.m_dwSkillIcon[i].controlIsEnabled = false;
				this.m_dwSkillIcon[i].Visible = false;
				this.m_lbNeedSkillAngerNum.Visible = false;
				if (this.m_goSkillActiveEffect[i] != null)
				{
					this.m_goSkillActiveEffect[i].SetActive(false);
					this.m_goSkillClickEffect[i].SetActive(false);
					this.SetFxGlow(i, false);
				}
			}
		}
		else
		{
			if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BUBBLEGAMEGUIDE_DLG) != null)
			{
				NrTSingleton<FormsManager>.Instance.Show(G_ID.BUBBLEGAMEGUIDE_DLG);
			}
			NkBattleChar nkBattleChar = Battle.BATTLE.SelectBattleSkillChar();
			if (nkBattleChar == null)
			{
				return;
			}
			for (int i = 0; i < 1; i++)
			{
				int num = nkBattleChar.GetSoldierInfo().SelectBattleSkillByWeapon(i + 1);
				if (num > 0)
				{
					UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(num);
					this.m_dwSkillIcon[i].SetTexture(battleSkillIconTexture);
					this.m_dwSkillIcon[i].controlIsEnabled = true;
					this.m_dwSkillIcon[i].Visible = true;
					this.m_lbNeedSkillAngerNum.Visible = true;
					int battleSkillLevel = nkBattleChar.GetSoldierInfo().GetBattleSkillLevel(num);
					BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num, battleSkillLevel);
					if (battleSkillDetail != null)
					{
						this.m_BSkillDetail[i] = battleSkillDetail;
						if (!this.CheckBattleSkillUseAble(num, battleSkillDetail.m_nSkillNeedAngerlyPoint) || nkBattleChar.IsBattleCharATB(32))
						{
							this.m_dwSkillIcon[i].color.a = 1f;
							this.m_dwSkillIcon[i].SetAlpha(0.3f);
						}
						else
						{
							this.m_dwSkillIcon[i].color.a = 1f;
							this.m_dwSkillIcon[i].SetAlpha(1f);
						}
					}
				}
				else
				{
					this.m_BSkillDetail[i] = null;
					this.m_dwSkillIcon[i].controlIsEnabled = false;
					this.m_dwSkillIcon[i].Visible = false;
					this.m_lbNeedSkillAngerNum.Visible = false;
					if (this.m_goSkillActiveEffect[i] != null)
					{
						this.m_goSkillActiveEffect[i].SetActive(false);
						this.m_goSkillClickEffect[i].SetActive(false);
						this.SetFxGlow(i, false);
					}
				}
			}
			this.UpdateSelectSkillObject();
		}
	}

	public void SetAngerlyPoint(int AngerlyPoint)
	{
		if (AngerlyPoint >= this.m_nMaxAngryPoint)
		{
			AngerlyPoint = this.m_nMaxAngryPoint;
		}
		this.m_nNowGetAngerlyPoint = AngerlyPoint;
		this.SetAngryText();
	}

	public void AddAngerlyPoint(int AngerlyPoint)
	{
		this.m_nNowGetAngerlyPoint += AngerlyPoint;
		if (this.m_nNowGetAngerlyPoint >= this.m_nMaxAngryPoint)
		{
			this.m_nNowGetAngerlyPoint = this.m_nMaxAngryPoint;
		}
		if (this.m_nNowGetAngerlyPoint < 0)
		{
			this.m_nNowGetAngerlyPoint = 0;
		}
		this.SetAngryText();
	}

	public bool CheckBattleSkillUseAble(int BattleSkillUnique, int needAngerlyPoint)
	{
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(BattleSkillUnique);
		if (battleSkillBase == null)
		{
			return false;
		}
		if (battleSkillBase.m_nSkillType == 1)
		{
			NkBattleChar nkBattleChar = Battle.BATTLE.SelectBattleSkillChar();
			int num = needAngerlyPoint;
			if (nkBattleChar != null)
			{
				num = this.GetRealUseAngerlyPoint(nkBattleChar, needAngerlyPoint);
			}
			if (this.m_nNowGetAngerlyPoint >= num)
			{
				return true;
			}
		}
		return false;
	}

	public override void Update()
	{
		base.Update();
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (kMyCharInfo.GetAutoBattle() == E_BF_AUTO_TYPE.AUTO)
		{
			this.m_btAutoBattle.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
		}
		else if (kMyCharInfo.GetAutoBattle() == E_BF_AUTO_TYPE.MANUAL)
		{
			this.m_btAutoBattle.SetControlState(UIButton.CONTROL_STATE.NORMAL);
		}
		int targetBtCount = Battle.BATTLE.GetTargetBtCount();
		if (0 < targetBtCount)
		{
			if (Battle.BATTLE.GetCheckTargetBt())
			{
				this.m_btTarget.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
				this.m_lbTargetNotice.Visible = true;
			}
			else
			{
				this.m_btTarget.SetControlState(UIButton.CONTROL_STATE.NORMAL);
				this.m_lbTargetNotice.Visible = false;
			}
			if (this.m_nTargetButtonCount != targetBtCount)
			{
				this.menuNotice.Visible = true;
				this.menuNotice.Text = targetBtCount.ToString();
				this.m_nTargetButtonCount = targetBtCount;
			}
		}
		else
		{
			if (this.menuNotice.Visible)
			{
				this.menuNotice.Visible = false;
			}
			if (this.m_btTarget.Visible)
			{
				this.m_btTarget.Visible = false;
				this.m_lbTarget.Visible = false;
				this.m_dwTargetBG07_0.Visible = false;
				this.m_dwTargetBG07_1.Visible = false;
				this.m_dwTargetBG07_2.Visible = false;
				this.m_lbTargetNotice.Visible = false;
			}
		}
		if (this.m_fStartBlockTime + this.m_fBlockTime < Time.time)
		{
			this.m_fStartBlockTime = 0f;
		}
		if (5 >= kMyCharInfo.GetLevel() && this.m_nMaxAngryPoint <= this.m_nNowGetAngerlyPoint)
		{
			if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL && NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BUBBLEGAMEGUIDE_DLG) == null && Battle.BATTLE.GetCurrentSelectChar() != null)
			{
				BubbleGameGuideDlg bubbleGameGuideDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BUBBLEGAMEGUIDE_DLG) as BubbleGameGuideDlg;
				if (bubbleGameGuideDlg != null)
				{
					Vector3 location = base.GetLocation();
					location.x += this.m_btSkill[0].GetLocation().x;
					location.y = -location.y + this.m_btSkill[0].GetSize().y / 2f;
					bubbleGameGuideDlg.SetText("1019", location);
				}
			}
		}
		else if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BUBBLEGAMEGUIDE_DLG) != null)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BUBBLEGAMEGUIDE_DLG);
		}
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetLocation(GUICamera.width - base.GetSizeX(), GUICamera.height - base.GetSizeY());
	}

	public void MouseOutSkill(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
	}

	public void MouseOverSkill(IUIObject obj)
	{
		NkBattleChar nkBattleChar = Battle.BATTLE.SelectBattleSkillChar();
		for (int i = 0; i < 1; i++)
		{
			if (this.m_btSkill[i] == obj && this.m_BSkillDetail[i] != null)
			{
				Tooltip_Dlg tooltip_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TOOLTIP_DLG) as Tooltip_Dlg;
				if (tooltip_Dlg != null)
				{
					int num = this.m_BSkillDetail[i].m_nSkillLevel;
					if (num == 0)
					{
						num = 1;
					}
					tooltip_Dlg.SetBattleControlSkillTooltip((G_ID)base.WindowID, this.m_BSkillDetail[i].m_nSkillUnique, num, 0, nkBattleChar.GetSoldierInfo());
				}
				break;
			}
		}
	}

	public void OnClickBattleSkill(IUIObject obj)
	{
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		NkBattleChar nkBattleChar = Battle.BATTLE.SelectBattleSkillChar();
		if (this.m_fStartBlockTime > 0f)
		{
			return;
		}
		this.m_fStartBlockTime = Time.time;
		for (int i = 0; i < 1; i++)
		{
			if (this.m_btSkill[i] == obj)
			{
				if (Battle.BATTLE.m_iBattleSkillIndex == -1)
				{
					if (this.m_BSkillDetail[i] != null)
					{
						Battle.BATTLE.m_iBattleSkillIndex = i + 1;
						int battleSkillUnique = nkBattleChar.GetSoldierInfo().SelectBattleSkillByWeapon(i + 1);
						if (Battle.BATTLE.CanSelecActionBattleSkill(battleSkillUnique))
						{
							Battle.BATTLE.m_iBattleSkillIndex = i + 1;
							Battle.BATTLE.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_SKILL;
							Battle.BATTLE.ShowBattleSkillRange(true, battleSkillUnique);
							if (this.m_goSkillClickEffect[i].activeInHierarchy)
							{
								this.m_goSkillClickEffect[i].SetActive(false);
							}
							this.m_goSkillClickEffect[i].SetActive(true);
						}
					}
				}
				else
				{
					Battle.BATTLE.m_iBattleSkillIndex = -1;
					Battle.BATTLE.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
					Battle.BATTLE.ShowBattleSkillRange(false, 0);
					Battle.BATTLE.GRID_MANAGER.ActiveAttackGridCanTarget();
				}
				break;
			}
		}
		this.SetAngergaugeFX_Click(true);
		this.UpdateSelectSkillObject();
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void ButtonAddEffectDelegate(IUIObject control, GameObject obj)
	{
		if (obj == null)
		{
			return;
		}
		for (int i = 0; i < 1; i++)
		{
			if (this.m_btSkill[i] == control)
			{
				this.m_goSkillActiveEffect[i] = obj;
				this.m_goSkillActiveEffect[i].SetActive(false);
				this.m_goSkillActiveEffect[i].transform.localScale = new Vector3(1f, 1f, 1f);
				Transform child = NkUtil.GetChild(this.m_goSkillActiveEffect[i].transform, "fx_skill_click");
				if (child != null)
				{
					this.m_goSkillClickEffect[i] = child.gameObject;
				}
				break;
			}
		}
	}

	public void UpdateSelectSkillObject()
	{
		if (Battle.BATTLE == null)
		{
			return;
		}
		for (int i = 0; i < 1; i++)
		{
			if (!(this.m_goSkillActiveEffect[i] == null))
			{
				if (!this.m_dwSkillIcon[i].Visible)
				{
					if (this.m_goSkillActiveEffect[i].activeInHierarchy)
					{
						this.m_goSkillActiveEffect[i].SetActive(false);
						this.m_goSkillClickEffect[i].SetActive(false);
					}
				}
				else if (Battle.BATTLE.REQUEST_ORDER != eBATTLE_ORDER.eBATTLE_ORDER_SKILL)
				{
					if (this.m_dwSkillIcon[i].color.a == 1f)
					{
						if (!this.m_goSkillActiveEffect[i].activeInHierarchy)
						{
							this.m_goSkillActiveEffect[i].SetActive(true);
						}
						this.SetFxGlow(i, false);
						this.m_goSkillClickEffect[i].SetActive(false);
					}
					else if (this.m_goSkillActiveEffect[i].activeInHierarchy)
					{
						this.m_goSkillActiveEffect[i].SetActive(false);
						this.m_goSkillClickEffect[i].SetActive(false);
					}
					this.SetAngergaugeFX_Click(false);
				}
				else if (Battle.BATTLE.m_iBattleSkillIndex == i + 1)
				{
					if (!this.m_goSkillActiveEffect[i].activeInHierarchy)
					{
						this.m_goSkillActiveEffect[i].SetActive(true);
						this.m_goSkillClickEffect[i].SetActive(false);
					}
					this.SetFxGlow(i, true);
				}
				else if (this.m_goSkillActiveEffect[i].activeInHierarchy)
				{
					this.m_goSkillActiveEffect[i].SetActive(false);
					this.m_goSkillClickEffect[i].SetActive(false);
					this.SetFxGlow(i, false);
				}
			}
		}
	}

	private void SetFxGlow(int nIndex, bool bActive)
	{
		Transform child = NkUtil.GetChild(this.m_goSkillActiveEffect[nIndex].transform, "fx_glow");
		if (child != null && child.gameObject.activeInHierarchy != bActive)
		{
			child.gameObject.SetActive(bActive);
		}
	}

	public void SetAngergaugeFX_Click(bool bActive)
	{
		if (this.m_goAngerActiveEffect != null)
		{
			Transform child = NkUtil.GetChild(this.m_goAngerActiveEffect.transform, "fx_click");
			if (child != null)
			{
				child.gameObject.SetActive(bActive);
			}
		}
	}

	public void SetAngryText()
	{
		this.m_lbNowAngerNum.SetText(this.m_nNowGetAngerlyPoint.ToString());
		NkBattleChar nkBattleChar = Battle.BATTLE.SelectBattleSkillChar();
		if (nkBattleChar != null)
		{
			for (int i = 0; i < 1; i++)
			{
				int num = nkBattleChar.GetSoldierInfo().SelectBattleSkillByWeapon(i + 1);
				if (num > 0)
				{
					int battleSkillLevel = nkBattleChar.GetSoldierInfo().GetBattleSkillLevel(num);
					BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num, battleSkillLevel);
					if (battleSkillDetail != null)
					{
						string arg = string.Empty;
						int num2 = battleSkillDetail.m_nSkillNeedAngerlyPoint;
						num2 = this.GetRealUseAngerlyPoint(nkBattleChar, battleSkillDetail.m_nSkillNeedAngerlyPoint);
						string arg2 = string.Empty;
						if (num2 > battleSkillDetail.m_nSkillNeedAngerlyPoint)
						{
							arg2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1305");
						}
						else if (num2 < battleSkillDetail.m_nSkillNeedAngerlyPoint)
						{
							arg2 = NrTSingleton<CTextParser>.Instance.GetTextColor("2002");
						}
						arg = num2.ToString();
						this.m_lbNeedSkillAngerNum.SetText(string.Format("{0}{1}", arg2, arg));
					}
				}
			}
		}
		float num3;
		if (this.m_nNowGetAngerlyPoint == this.m_nMaxAngryPoint)
		{
			num3 = this.m_fAngerProgressHeight;
		}
		else
		{
			num3 = this.m_fAngerProgressHeight * ((float)this.m_nNowGetAngerlyPoint / (float)this.m_nMaxAngryPoint);
		}
		if (this.m_fBeforeAngerPercent != num3)
		{
			this.m_dwNeedAngerPrg.SetSize(this.m_dwNeedAngerPrg.GetSize().x, num3);
			this.m_dwNeedAngerPrg.SetLocation(this.m_dwNeedAngerPrg.GetLocationX(), this.m_dwAngerPrg.GetLocationY() + (this.m_fAngerProgressHeight - this.m_dwNeedAngerPrg.GetSize().y));
			this.m_fBeforeAngerPercent = num3;
		}
	}

	public int GetRealUseAngerlyPoint(NkBattleChar pkAngerlyChar, int disCountPoint)
	{
		if (pkAngerlyChar == null || disCountPoint == 0)
		{
			return 0;
		}
		int num = disCountPoint;
		BATTLE_CONSTANT_Manager instance = BATTLE_CONSTANT_Manager.GetInstance();
		int num2 = (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLESKILL_MAX_ADDANGERLYPOINT);
		int num3 = (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLESKILL_MIN_ADDANGERLYPOINT);
		if (num2 == 0)
		{
			num2 = 10000;
		}
		if (num3 == 0)
		{
			num3 = -10000;
		}
		else
		{
			num3 = -num3;
		}
		int num4 = pkAngerlyChar.GetBattleSkillBuffData(102);
		int num5 = pkAngerlyChar.GetBattleSkillBuffData(103);
		if (num5 > 0)
		{
			num5 = -num5;
		}
		if (num4 > 0 || num5 < 0)
		{
			if (num4 > num2)
			{
				num4 = num2;
			}
			if (num5 < num3)
			{
				num5 = num3;
			}
			int num6 = num4 + num5;
			if (num6 != 0)
			{
				num6 = (int)((float)disCountPoint * ((float)num6 / 10000f));
				num += num6;
				int num7 = disCountPoint * 2;
				int num8 = disCountPoint / 2;
				if (num > num7)
				{
					num = num7;
				}
				else if (num < num8)
				{
					num = num8;
				}
			}
		}
		return num;
	}
}
