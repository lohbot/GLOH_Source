using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.IO;
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

	public enum eGUARDIANEFFECT
	{
		eGUARDIANEFFECT_NONE,
		eGUARDIANEFFECT_UI,
		eGUARDIANEFFECT_UIEND,
		eGUARDIANEFFECT_MOVIE,
		eGUARDIANEFFECT_MOVIEEND
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

	private Box menuNotice;

	private Label m_lbTargetNotice;

	private DrawTexture m_dwAngelSkillGauge_BG;

	private DrawTexture m_dwAngelSkillIcon;

	private DrawTexture m_dwAngelSkillGauge;

	private Button m_btGuardAngelSkill;

	private Label lb_Guardian_Skillname;

	private BATTLESKILL_DETAIL[] m_BSkillDetail;

	private BATTLESKILL_BASE[] m_BSkillBase;

	private GameObject[] m_goSkillActiveEffect;

	private GameObject[] m_goSkillClickEffect;

	private GameObject m_goAngerActiveEffect;

	private int m_nNowGetAngerlyPoint;

	private int m_nNowGetAngelAngerlyPoint;

	private float m_fAngelAngerProgress;

	private float m_fBeforeAngelAngerPercent = -1f;

	private float m_fAngerProgressHeight;

	private float m_fBeforeAngerPercent = -1f;

	private float m_fBlockTime = 0.5f;

	private float m_fStartBlockTime;

	private bool bMakeOne;

	private int m_nMaxAngryPoint = 1000;

	private int m_nMaxAngelAngryPoint = 1000;

	private int m_nTargetButtonCount;

	private int m_nSelectGuardAngelUnique;

	private MYTHRAID_GUARDIANANGEL_INFO m_nSelectGuardAngelInfo;

	private string effect_file_name = string.Empty;

	private GameObject m_EffectGameObject;

	private MYTHRAID_GUARDIANANGEL_INFO currentInvokeAngelInfo;

	public long Angelskill_Invoke_PersonID;

	private float m_fMovieBlockTime;

	public bool bESC;

	public Battle_Control_Dlg.eGUARDIANEFFECT e_guardianEffect;

	public Label Damage
	{
		get
		{
			return this.m_lbNowAngerNum;
		}
		private set
		{
		}
	}

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
		Texture2D texture = Resources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Texture/AngerBG") as Texture2D;
		this.m_dwAngerFrame.SetTexture(texture);
		this.m_lbNeedSkillAngerNum = (base.GetControl("LB_SkillAnger") as Label);
		this.m_lbNowAngerNum = (base.GetControl("LB_AngerNum") as Label);
		this.m_lbNowAngerNum.SetText(this.m_nNowGetAngerlyPoint.ToString());
		this.AngryTextAniSetting();
		this.m_dwSkillIcon = new DrawTexture[1];
		this.m_btSkill = new Button[1];
		this.m_BSkillDetail = new BATTLESKILL_DETAIL[1];
		this.m_BSkillBase = new BATTLESKILL_BASE[1];
		this.m_goSkillActiveEffect = new GameObject[1];
		this.m_goSkillClickEffect = new GameObject[1];
		this.m_nMaxAngelAngryPoint = (int)BATTLE_CONSTANT_Manager.GetInstance().GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_ANGELPOINT_MAX);
		this.m_nNowGetAngelAngerlyPoint = (int)BATTLE_CONSTANT_Manager.GetInstance().GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_ANGELPOINT_START);
		this.m_dwAngerPrg = (base.GetControl("DT_AngerBG") as DrawTexture);
		this.m_fAngerProgressHeight = this.m_dwAngerPrg.GetSize().y;
		this.m_dwAngerPrg.Visible = false;
		this.m_dwAngelSkillGauge_BG = (base.GetControl("DT_Angelpoint_Gauge_BG") as DrawTexture);
		this.m_dwAngelSkillIcon = (base.GetControl("DT_AngelSkillIcon") as DrawTexture);
		NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("UI/mythicraid/fx_mythbutton_ui_mobile", this.m_dwAngelSkillIcon, this.m_dwAngelSkillIcon.GetSize());
		this.m_dwAngelSkillGauge = (base.GetControl("DT_Angelpoint_Gauge") as DrawTexture);
		this.m_dwAngelSkillGauge.SetLocation(this.m_dwAngelSkillGauge_BG.GetLocationX() + 10f, this.m_dwAngelSkillGauge_BG.GetLocationY() + 5f);
		this.m_fAngelAngerProgress = this.m_dwAngelSkillGauge.GetSize().x;
		this.m_btGuardAngelSkill = (base.GetControl("BTN_AngerSkillUse") as Button);
		Button expr_275 = this.m_btGuardAngelSkill;
		expr_275.Click = (EZValueChangedDelegate)Delegate.Combine(expr_275.Click, new EZValueChangedDelegate(this.OnClickGuardAngelSkill));
		this.m_nSelectGuardAngelUnique = NrTSingleton<MythRaidManager>.Instance.m_iGuardAngelUnique;
		this.lb_Guardian_Skillname = (base.GetControl("LB_Guardian_Skillname") as Label);
		MYTHRAID_GUARDIANANGEL_INFO mythRaidGuardAngelInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidGuardAngelInfo(this.m_nSelectGuardAngelUnique);
		if (mythRaidGuardAngelInfo != null)
		{
			this.m_nSelectGuardAngelInfo = mythRaidGuardAngelInfo;
			this.m_dwAngelSkillIcon.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(this.m_nSelectGuardAngelInfo.SKILLUNIQUE));
		}
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
		else if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			this.m_nMaxAngryPoint = 1500;
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
			Button expr_460 = this.m_btSkill[i];
			expr_460.Click = (EZValueChangedDelegate)Delegate.Combine(expr_460.Click, new EZValueChangedDelegate(this.OnClickBattleSkill));
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_SKILL_ACTIVE", this.m_btSkill[i], this.m_btSkill[i].GetSize());
			this.m_btSkill[i].AddGameObjectDelegate(new EZGameObjectDelegate(this.ButtonAddEffectDelegate));
		}
		this.UpdateBattleSkillData();
		base.AllHideLayer();
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
		{
			base.ShowLayer(2);
		}
		else if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			base.ShowLayer(1);
			base.SetShowLayer(3, true);
			this.UpdateAngelSkillData(true);
		}
		else if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PREVIEW)
		{
			base.ShowLayer(1);
			this.SetControl_PreviewHero(true);
		}
		else if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
		{
			base.ShowLayer(1);
			this.SetControl_PreviewHero(false);
		}
		else
		{
			base.ShowLayer(1);
		}
		this._SetDialogPos();
		this.SetAngergaugeFX_Click(false);
		if (NrTSingleton<NkQuestManager>.Instance.IsWorldFirst())
		{
			this.m_btChat.Visible = false;
		}
		this.bESC = false;
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
		this.menuNotice = (base.GetControl("Box_Notice") as Box);
		this.menuNotice.Visible = false;
		this.m_lbTargetNotice = (base.GetControl("Label_Label6") as Label);
		this.m_lbTargetNotice.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("925"));
		this.m_lbTargetNotice.Visible = false;
		if (this.m_btMove)
		{
			Button expr_1A8 = this.m_btMove;
			expr_1A8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1A8.Click, new EZValueChangedDelegate(this.OnClickMove));
			this.m_btMove.controlIsEnabled = true;
			this.m_btMove.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1384");
			this.m_btMove.EffectAni = false;
		}
		if (this.m_btTurnOver)
		{
			Button expr_211 = this.m_btTurnOver;
			expr_211.Click = (EZValueChangedDelegate)Delegate.Combine(expr_211.Click, new EZValueChangedDelegate(this.OnClickTurnOver));
			this.m_btTurnOver.controlIsEnabled = true;
			this.m_btTurnOver.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1388");
			this.m_btTurnOver.EffectAni = false;
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
			{
				Button expr_27B = this.m_btTurnOver;
				expr_27B.Click = (EZValueChangedDelegate)Delegate.Remove(expr_27B.Click, new EZValueChangedDelegate(this.OnClickTurnOver));
				Button expr_2A2 = this.m_btTurnOver;
				expr_2A2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2A2.Click, new EZValueChangedDelegate(this.OnClickUsingDisable));
			}
		}
		if (this.m_btRetreat)
		{
			Button expr_2D9 = this.m_btRetreat;
			expr_2D9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2D9.Click, new EZValueChangedDelegate(this.OnClickRetreat));
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
				Button expr_371 = this.m_btRetreat;
				expr_371.Click = (EZValueChangedDelegate)Delegate.Remove(expr_371.Click, new EZValueChangedDelegate(this.OnClickRetreat));
				Button expr_398 = this.m_btRetreat;
				expr_398.Click = (EZValueChangedDelegate)Delegate.Combine(expr_398.Click, new EZValueChangedDelegate(this.OnClickUsingDisable));
			}
		}
		if (this.m_btRetreatOnly)
		{
			Button expr_3CF = this.m_btRetreatOnly;
			expr_3CF.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3CF.Click, new EZValueChangedDelegate(this.OnClickRetreat));
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
			Button expr_466 = this.m_btFriendHelp;
			expr_466.Click = (EZValueChangedDelegate)Delegate.Combine(expr_466.Click, new EZValueChangedDelegate(this.OnClickContinueFriendHelp));
			this.m_btFriendHelp.controlIsEnabled = true;
			this.m_btFriendHelp.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1386");
			this.m_btFriendHelp.EffectAni = false;
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
			{
				Button expr_4D0 = this.m_btFriendHelp;
				expr_4D0.Click = (EZValueChangedDelegate)Delegate.Remove(expr_4D0.Click, new EZValueChangedDelegate(this.OnClickContinueFriendHelp));
				Button expr_4F7 = this.m_btFriendHelp;
				expr_4F7.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4F7.Click, new EZValueChangedDelegate(this.OnClickUsingDisable));
			}
		}
		if (this.m_btAutoBattle)
		{
			Button expr_52E = this.m_btAutoBattle;
			expr_52E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_52E.Click, new EZValueChangedDelegate(this.OnClickAutoBattle));
			this.m_btAutoBattle.controlIsEnabled = true;
			this.m_btAutoBattle.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1385");
			this.m_btAutoBattle.EffectAni = false;
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
			{
				Button expr_598 = this.m_btAutoBattle;
				expr_598.Click = (EZValueChangedDelegate)Delegate.Remove(expr_598.Click, new EZValueChangedDelegate(this.OnClickAutoBattle));
				Button expr_5BF = this.m_btAutoBattle;
				expr_5BF.Click = (EZValueChangedDelegate)Delegate.Combine(expr_5BF.Click, new EZValueChangedDelegate(this.OnClickUsingDisable));
			}
		}
		if (this.m_btTarget && !Battle.BATTLE.Observer && (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY) && !TsPlatform.IsIPhone)
		{
			Button expr_630 = this.m_btTarget;
			expr_630.Click = (EZValueChangedDelegate)Delegate.Combine(expr_630.Click, new EZValueChangedDelegate(this.OnClickTarget));
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
			Button expr_860 = this.m_btChat;
			expr_860.Click = (EZValueChangedDelegate)Delegate.Combine(expr_860.Click, new EZValueChangedDelegate(this.OnClickChat));
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
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
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
			msgBoxUI.SetMsg(new YesDelegate(this.RequestBabelMacroStopAndAutoBattle), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("187"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("188"), eMsgType.MB_OK_CANCEL, 2);
			return;
		}
		if (Battle.BATTLE != null)
		{
			this.SetAngergaugeFX_Click(false);
			Battle.BATTLE.Init_BattleSkill_Input(true);
			Battle.BATTLE.GRID_MANAGER.ActiveAttackGridCanTarget();
			Battle.BATTLE.ChangeBattleAuto();
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
			Battle.BATTLE.ChangeBattleAuto();
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
			else if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PREVIEW)
			{
				textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3293");
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("439");
			}
			else if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
			{
				if (NrTSingleton<NewExplorationManager>.Instance.AutoBattle)
				{
					NrTSingleton<NewExplorationManager>.Instance.SetAutoBattle(false, false, false);
				}
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("125");
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
		string a_oObject = string.Empty;
		if (this.bESC)
		{
			a_oObject = "FromESC";
		}
		msgBoxUI.SetMsg(new YesDelegate(this.OnRetreatkOK), a_oObject, new NoDelegate(this.OnRetreatkCancle), null, textFromInterface, message, eMsgType.MB_OK_CANCEL);
	}

	public void OnRetreatkOK(object a_oObject)
	{
		if (this.bESC)
		{
			NrTSingleton<NrMainSystem>.Instance.GetMainCore().EscGame_Cancle();
		}
		this.bESC = false;
		Battle.BATTLE.Send_GS_BATTLE_CLOSE_REQ();
	}

	public void OnRetreatkCancle(object a_oObject)
	{
		if (this.bESC)
		{
			NrTSingleton<NrMainSystem>.Instance.GetMainCore().EscGame_Cancle();
		}
		this.bESC = false;
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
		msgBoxUI.SetMsg(new YesDelegate(this.OnTurnOverOK), null, textFromMessageBox, message, eMsgType.MB_OK_CANCEL, 2);
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
		this.bESC = false;
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

	public void UpdateAngelSkillData(bool canShow)
	{
		float num;
		if (this.m_nNowGetAngelAngerlyPoint == this.m_nMaxAngelAngryPoint)
		{
			num = this.m_fAngelAngerProgress;
		}
		else
		{
			num = this.m_fAngelAngerProgress * ((float)this.m_nNowGetAngelAngerlyPoint / (float)this.m_nMaxAngelAngryPoint);
		}
		if (this.m_fBeforeAngelAngerPercent != num)
		{
			this.m_dwAngelSkillGauge.SetSize(num, this.m_dwAngelSkillGauge.GetSize().y);
			if (num == 0f)
			{
				this.m_dwAngelSkillGauge.Visible = false;
			}
			else
			{
				this.m_dwAngelSkillGauge.Visible = true;
			}
			this.m_fBeforeAngelAngerPercent = num;
		}
		if (canShow)
		{
			BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(this.m_nSelectGuardAngelInfo.SKILLUNIQUE, 1);
			if (this.m_nNowGetAngelAngerlyPoint >= battleSkillDetail.m_nSkillNeedAngerlyPoint)
			{
				this.GuardianAngelSkillUI(true);
			}
			else
			{
				this.GuardianAngelSkillUI(false);
			}
		}
		else
		{
			this.GuardianAngelSkillUI(false);
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
				this.m_BSkillBase[i] = null;
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
					BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(num);
					if (battleSkillDetail != null)
					{
						this.m_BSkillDetail[i] = battleSkillDetail;
						this.m_BSkillBase[i] = battleSkillBase;
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
					this.m_BSkillBase[i] = null;
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

	public void SetAngelAngerlyPoint(int nGuardAngelUnique, int AngerlyPoint)
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID && this.m_nSelectGuardAngelUnique == nGuardAngelUnique)
		{
			if (AngerlyPoint >= this.m_nMaxAngelAngryPoint)
			{
				AngerlyPoint = this.m_nMaxAngelAngryPoint;
			}
			else if (AngerlyPoint < 0)
			{
				AngerlyPoint = 0;
			}
			this.m_nNowGetAngelAngerlyPoint = AngerlyPoint;
			this.UpdateAngelSkillData(true);
		}
	}

	public void SetAngelAngerlyPointForce(int AngerlyPoint)
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			if (AngerlyPoint >= this.m_nMaxAngelAngryPoint)
			{
				AngerlyPoint = this.m_nMaxAngelAngryPoint;
			}
			else if (AngerlyPoint < 0)
			{
				AngerlyPoint = 0;
			}
			this.m_nNowGetAngelAngerlyPoint = AngerlyPoint;
			this.UpdateAngelSkillData(true);
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

	public void AddAngelAngerlyPoint(int AngelAngerlyPoint)
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			this.m_nNowGetAngelAngerlyPoint += AngelAngerlyPoint;
			if (this.m_nNowGetAngelAngerlyPoint >= this.m_nMaxAngelAngryPoint)
			{
				this.m_nNowGetAngelAngerlyPoint = this.m_nMaxAngelAngryPoint;
			}
			this.UpdateAngelSkillData(true);
		}
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
		if (Battle.BATTLE == null)
		{
			return;
		}
		if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PREVIEW && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
		{
			if (kMyCharInfo.GetAutoBattle() == E_BF_AUTO_TYPE.AUTO)
			{
				this.m_btAutoBattle.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
			}
			else if (kMyCharInfo.GetAutoBattle() == E_BF_AUTO_TYPE.MANUAL)
			{
				this.m_btAutoBattle.SetControlState(UIButton.CONTROL_STATE.NORMAL);
			}
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
			if (this.m_btTarget.GetEnableD())
			{
				this.m_btTarget.SetEnabled(false);
				Transform child = NkUtil.GetChild(this.m_btTarget.transform, "child_effect");
				if (child != null)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
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
		switch (this.e_guardianEffect)
		{
		case Battle_Control_Dlg.eGUARDIANEFFECT.eGUARDIANEFFECT_UI:
			if (this.m_EffectGameObject != null & !this.m_EffectGameObject.GetComponentInChildren<Animation>().isPlaying)
			{
				this.e_guardianEffect = Battle_Control_Dlg.eGUARDIANEFFECT.eGUARDIANEFFECT_UIEND;
			}
			break;
		case Battle_Control_Dlg.eGUARDIANEFFECT.eGUARDIANEFFECT_UIEND:
			this.e_guardianEffect = Battle_Control_Dlg.eGUARDIANEFFECT.eGUARDIANEFFECT_MOVIE;
			this.lb_Guardian_Skillname.SetText(string.Empty);
			this.lb_Guardian_Skillname.Visible = false;
			this.PlayMovie();
			this.m_fMovieBlockTime = Time.time;
			break;
		case Battle_Control_Dlg.eGUARDIANEFFECT.eGUARDIANEFFECT_MOVIE:
			if (this.m_fMovieBlockTime > 0f && Time.time - this.m_fMovieBlockTime >= 8f)
			{
				this.MovieEnd();
			}
			break;
		case Battle_Control_Dlg.eGUARDIANEFFECT.eGUARDIANEFFECT_MOVIEEND:
			this.e_guardianEffect = Battle_Control_Dlg.eGUARDIANEFFECT.eGUARDIANEFFECT_NONE;
			this.Send_GS_BATTLE_ANGEL_ORDER_REQ();
			break;
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

	public void Send_GS_BATTLE_ANGEL_ORDER_REQ()
	{
		if (this.Angelskill_Invoke_PersonID != NrTSingleton<NkCharManager>.Instance.GetChar(1).GetPersonID())
		{
			return;
		}
		GS_BATTLE_ANGEL_ORDER_REQ gS_BATTLE_ANGEL_ORDER_REQ = new GS_BATTLE_ANGEL_ORDER_REQ();
		gS_BATTLE_ANGEL_ORDER_REQ.nMythRaidRoomIndex = SoldierBatch.MYTHRAID_INFO.m_nMythRaidRoomIndex;
		gS_BATTLE_ANGEL_ORDER_REQ.nGuardAngelUnique = this.m_nSelectGuardAngelUnique;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_ANGEL_ORDER_REQ, gS_BATTLE_ANGEL_ORDER_REQ);
	}

	public void OnClickGuardAngelSkill(IUIObject obj)
	{
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return;
		}
		if (this.m_fStartBlockTime > 0f)
		{
			return;
		}
		this.m_fStartBlockTime = Time.time;
		if (this.m_nSelectGuardAngelUnique < 0)
		{
			return;
		}
		if (Battle.BATTLE.CurrentTurnAlly != Battle.BATTLE.MyAlly)
		{
			return;
		}
		if (!Battle.BATTLE.IsEnableOrderTime)
		{
			return;
		}
		this.Send_EffectStart_Packet();
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
		this.AngryLabelUpdate(this.m_nNowGetAngerlyPoint.ToString());
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

	private void AngryTextAniSetting()
	{
		if (this.m_lbNowAngerNum == null)
		{
			Debug.LogError("ERROR, Battle_Control_Dlg.cs, AngryTextAniSetting(), m_lbNowAngerNum is Null");
			return;
		}
		UILabelStepByStepAni uILabelStepByStepAni = this.m_lbNowAngerNum.transform.gameObject.AddComponent<UILabelStepByStepAni>();
		uILabelStepByStepAni._loopTime = -1f;
		uILabelStepByStepAni._loopInterval = 0.01f;
		uILabelStepByStepAni._nextValueStopInterval = 0.1f;
		uILabelStepByStepAni._reverse = true;
		uILabelStepByStepAni._changePartUpdate = true;
		uILabelStepByStepAni._useComma = false;
	}

	private void AngryLabelUpdate(string angry)
	{
		UILabelStepByStepAni component = this.m_lbNowAngerNum.GetComponent<UILabelStepByStepAni>();
		if (component == null)
		{
			Debug.LogError("ERROR, Battle_Control_Dlg.cs, AngryLabelUpdate(), textAni is Null");
			return;
		}
		component.Clear();
		this.m_lbNowAngerNum.SetText(angry);
	}

	private void GuardianAngelSkillUI(bool _visible)
	{
		float alpha;
		if (_visible)
		{
			alpha = 1f;
		}
		else
		{
			alpha = 0.3f;
		}
		if (this.m_dwAngelSkillIcon.transform.childCount > 0)
		{
			this.m_dwAngelSkillIcon.transform.GetChild(0).gameObject.SetActive(_visible);
		}
		this.m_dwAngelSkillIcon.color.a = 1f;
		this.m_dwAngelSkillIcon.SetAlpha(alpha);
		this.m_btGuardAngelSkill.SetEnabled(_visible);
	}

	public void GuardianAngelEffectStart(int angelUnique)
	{
		if (this.m_nSelectGuardAngelInfo == null)
		{
			Debug.LogError("There no GuardianAngelInfo.");
			return;
		}
		this.currentInvokeAngelInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidGuardAngelInfo(angelUnique);
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(this.currentInvokeAngelInfo.EFFECTKEY);
		this.effect_file_name = string.Format("{0}{1}", "effect/instant/", effectInfo.MOBILE_BUNDLE_PATH);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(this.effect_file_name + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.ActionEffect), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		this.UpdateAngelSkillData(false);
	}

	private void ActionEffect(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_EffectGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 3f;
				this.m_EffectGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_EffectGameObject, GUICamera.UILayer);
				base.InteractivePanel.MakeChild(this.m_EffectGameObject);
				this.e_guardianEffect = Battle_Control_Dlg.eGUARDIANEFFECT.eGUARDIANEFFECT_UI;
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "BATTLE", "MYTH_SKILL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_EffectGameObject);
				}
				Transform child = NkUtil.GetChild(this.m_EffectGameObject.transform, "fx_text");
				this.lb_Guardian_Skillname.Visible = true;
				this.lb_Guardian_Skillname.transform.position = child.position;
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.currentInvokeAngelInfo.SKILLUNIQUE);
				this.lb_Guardian_Skillname.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey));
			}
		}
	}

	private void PlayMovie()
	{
		if (this.currentInvokeAngelInfo == null)
		{
			Debug.LogError("There no currentInvokeAngelInfo.");
			return;
		}
		string text = string.Format("{0}/{1}", NrTSingleton<NrGlobalReference>.Instance.basePath, this.currentInvokeAngelInfo.MOOVIEKEY);
		if (File.Exists(text))
		{
			Debug.LogError("Cannot find this File : " + text);
			this.MovieEnd();
			return;
		}
		NmMainFrameWork.PlayMovieURL(text, false, false, false);
	}

	public void MovieEnd()
	{
		this.e_guardianEffect = Battle_Control_Dlg.eGUARDIANEFFECT.eGUARDIANEFFECT_MOVIEEND;
	}

	private void Send_EffectStart_Packet()
	{
		SendPacket.GetInstance().SendIDType(258);
	}

	private void SetControl_PreviewHero(bool isChatOff)
	{
		if (this.m_btTurnOver != null)
		{
			this.m_btTurnOver.SetEnabled(false);
		}
		if (this.m_btMove != null)
		{
			this.m_btMove.SetEnabled(false);
		}
		if (this.m_btFriendHelp != null)
		{
			this.m_btFriendHelp.SetEnabled(false);
		}
		if (this.m_btAutoBattle != null)
		{
			this.m_btAutoBattle.SetEnabled(false);
		}
		if (this.m_btChat != null && isChatOff)
		{
			this.m_btChat.Visible = false;
		}
	}

	public void ShowRetreatWithCancel()
	{
		this.bESC = true;
		this.OnClickRetreat(null);
	}
}
