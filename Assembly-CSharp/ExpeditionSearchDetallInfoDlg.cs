using GAME;
using GameMessage.Private;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class ExpeditionSearchDetallInfoDlg : Form
{
	private const int MAX_SOL_SLOT_NUM = 9;

	private const int MON_MAX_SOL_SLOT_NUM = 3;

	private DrawTexture m_dtBG;

	private Label m_lTitle;

	private Label m_laExpeditionCurNum;

	private Label m_laOccExpeditionNum;

	private Label m_laSearchMoney;

	private Label m_laOccSelectMemberName;

	private ItemTexture[] m_itOccSol = new ItemTexture[9];

	private DrawTexture[] m_dtOccSolBG = new DrawTexture[9];

	private Button m_btOriKeepingHelpIcon;

	private DrawTexture m_dtOriKeepingHelpBg;

	private Label m_lOriKeepingHelpText;

	private DrawTexture m_dtOriKeepingHelpTail;

	private Button m_btOriPlunderHelpIcon;

	private DrawTexture m_dtOriPlunderHelpBg;

	private Label m_lOriPlunderHelpText;

	private DrawTexture m_dtOriPlunderHelpTail;

	private DrawTexture m_dtMineIcon2;

	private Button[] m_btOccMilitary = new Button[15];

	private ItemTexture[] m_itOccMilitary = new ItemTexture[15];

	private Button[] m_btOccMilitarySelect = new Button[15];

	private ItemTexture[] m_itOccMilitarySelect = new ItemTexture[15];

	private DrawTexture[] m_dOccPersonImage = new DrawTexture[15];

	private DrawTexture[] m_dOccSelectImage = new DrawTexture[15];

	private DrawTexture[] m_dtOccSolPosEffect = new DrawTexture[15];

	private Button m_btClose;

	private Button m_btResearch;

	private Button m_btGoMilitary;

	private Button m_btClose03;

	private Button m_btStart02;

	private Button m_btClose02;

	public bool m_bOriKeeping;

	public bool m_bOriPlunder;

	public EXPEDITION_SEARCH_INFO m_expeditionSearch_info = new EXPEDITION_SEARCH_INFO();

	public GS_EXPEDITION_DETAILINFO_ACK m_expeditiondetailinfo = new GS_EXPEDITION_DETAILINFO_ACK();

	public NkExpeditionMilitaryInfo m_solinfo = new NkExpeditionMilitaryInfo();

	public NkSoldierInfo[] m_pksolinfo;

	private int m_old_select_index = -1;

	private int m_select_index = -1;

	private int m_SolInfoNumber = -1;

	private int m_old_SolInfoNumber = -1;

	public bool m_bHaveMilitary;

	public eExpeditionSearchDetailInfo_Mode m_eMode;

	private Dictionary<int, ECO> m_dicEcoGroupInfo = new Dictionary<int, ECO>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Expedition/DLG_ExpeditionSearchDetailInfo", G_ID.EXPEDITION_SEARCHDETAILINFO_DLG, false, true);
		base.ShowBlackBG(1f);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_lTitle = (base.GetControl("LB_Title") as Label);
		this.m_dtBG = (base.GetControl("DT_Image") as DrawTexture);
		this.m_laExpeditionCurNum = (base.GetControl("LB_MinNum") as Label);
		this.m_laSearchMoney = (base.GetControl("LB_Investigation_Gold") as Label);
		this.m_laOccExpeditionNum = (base.GetControl("LB_BoxNum") as Label);
		this.m_laOccSelectMemberName = (base.GetControl("Label_Label64") as Label);
		this.m_laOccSelectMemberName.SetText(string.Empty);
		this.m_btOriKeepingHelpIcon = (base.GetControl("BT_Help01") as Button);
		Button expr_B0 = this.m_btOriKeepingHelpIcon;
		expr_B0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_B0.Click, new EZValueChangedDelegate(this.OnClickOriKeepingHelpText));
		this.m_dtOriKeepingHelpBg = (base.GetControl("DT_HelpBg01") as DrawTexture);
		this.m_dtOriKeepingHelpBg.Visible = false;
		this.m_lOriKeepingHelpText = (base.GetControl("LB_HelpText01") as Label);
		this.m_lOriKeepingHelpText.Visible = false;
		this.m_dtOriKeepingHelpTail = (base.GetControl("DT_HelpTail01") as DrawTexture);
		this.m_dtOriKeepingHelpTail.Visible = false;
		this.m_btOriPlunderHelpIcon = (base.GetControl("BT_Help02") as Button);
		Button expr_153 = this.m_btOriPlunderHelpIcon;
		expr_153.Click = (EZValueChangedDelegate)Delegate.Combine(expr_153.Click, new EZValueChangedDelegate(this.OnClickOriPlunderHelpText));
		this.m_dtOriPlunderHelpBg = (base.GetControl("DT_HelpBg02") as DrawTexture);
		this.m_dtOriPlunderHelpBg.Visible = false;
		this.m_lOriPlunderHelpText = (base.GetControl("LB_HelpText02") as Label);
		this.m_lOriPlunderHelpText.Visible = false;
		this.m_dtOriPlunderHelpTail = (base.GetControl("DT_HelpTail02") as DrawTexture);
		this.m_dtOriPlunderHelpTail.Visible = false;
		this.m_dtMineIcon2 = (base.GetControl("DT_MineIcon2") as DrawTexture);
		for (int i = 0; i < 9; i++)
		{
			this.m_itOccSol[i] = (base.GetControl(string.Format("IT_SolInfo0{0}", i + 1)) as ItemTexture);
			this.m_dtOccSolBG[i] = (base.GetControl(string.Format("DT_SolInfoBG0{0}", i + 1)) as DrawTexture);
		}
		for (int j = 0; j < 3; j++)
		{
			this.m_btOccMilitary[j] = (base.GetControl(string.Format("Btn_Sol0{0}", j + 1)) as Button);
			this.m_btOccMilitary[j].EffectAni = false;
			this.m_itOccMilitary[j] = (base.GetControl(string.Format("IT_SolImage0{0}", j + 1)) as ItemTexture);
			this.m_btOccMilitarySelect[j] = (base.GetControl(string.Format("Btn_SelectSol0{0}", j + 1)) as Button);
			this.m_btOccMilitarySelect[j].EffectAni = false;
			this.m_btOccMilitarySelect[j].Visible = false;
			this.m_itOccMilitarySelect[j] = (base.GetControl(string.Format("IT_SelectSolImage0{0}", j + 1)) as ItemTexture);
			this.m_itOccMilitarySelect[j].Visible = false;
			this.m_dOccPersonImage[j] = (base.GetControl(string.Format("DT_PersonImage0{0}", j + 1)) as DrawTexture);
			this.m_dOccPersonImage[j].Visible = false;
			this.m_dOccSelectImage[j] = (base.GetControl(string.Format("DT_SelectImage0{0}", j + 1)) as DrawTexture);
			this.m_dOccSelectImage[j].Visible = false;
			this.m_dtOccSolPosEffect[j] = (base.GetControl(string.Format("DT_SelectSol0{0}", j + 1)) as DrawTexture);
			this.m_dtOccSolPosEffect[j].Visible = false;
		}
		this.m_btClose = (base.GetControl("Btn_Close") as Button);
		Button expr_3E0 = this.m_btClose;
		expr_3E0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3E0.Click, new EZValueChangedDelegate(this.OnBtnClickClose));
		this.m_btGoMilitary = (base.GetControl("Btn_Start") as Button);
		Button expr_41D = this.m_btGoMilitary;
		expr_41D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_41D.Click, new EZValueChangedDelegate(this.OnBtnClickGoMilitary));
		this.m_btResearch = (base.GetControl("Btn_Return") as Button);
		Button expr_45A = this.m_btResearch;
		expr_45A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_45A.Click, new EZValueChangedDelegate(this.OnBtnClickResearch));
		this.m_btClose03 = (base.GetControl("Btn_Close03") as Button);
		Button expr_497 = this.m_btClose03;
		expr_497.Click = (EZValueChangedDelegate)Delegate.Combine(expr_497.Click, new EZValueChangedDelegate(this.OnBtnClickClose));
		this.m_btStart02 = (base.GetControl("Btn_Start02") as Button);
		this.m_btStart02.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615");
		Button expr_4EE = this.m_btStart02;
		expr_4EE.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4EE.Click, new EZValueChangedDelegate(this.OnClickBackMove));
		this.m_btClose02 = (base.GetControl("Btn_Close02") as Button);
		Button expr_52B = this.m_btClose02;
		expr_52B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_52B.Click, new EZValueChangedDelegate(this.OnBtnClickClose));
		base.SetScreenCenter();
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
	}

	public void InitInterface(int nIndex)
	{
		if (nIndex < 0)
		{
			return;
		}
		this.m_itOccMilitarySelect[nIndex].Visible = false;
		this.m_itOccMilitary[nIndex].Visible = false;
		this.m_dOccPersonImage[nIndex].Visible = false;
		this.m_dOccSelectImage[nIndex].Visible = false;
		this.m_dtOccSolPosEffect[nIndex].Visible = false;
	}

	public void InitInterface()
	{
		for (int i = 0; i < 3; i++)
		{
			this.m_btOccMilitarySelect[i].Visible = false;
			this.m_itOccMilitarySelect[i].Visible = false;
			this.m_itOccMilitary[i].Visible = false;
			this.m_dOccPersonImage[i].Visible = false;
			this.m_dOccSelectImage[i].Visible = false;
			this.m_dtOccSolPosEffect[i].Visible = false;
			Button expr_63 = this.m_btOccMilitary[i];
			expr_63.Click = (EZValueChangedDelegate)Delegate.Remove(expr_63.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
			Button expr_8C = this.m_btOccMilitary[i];
			expr_8C.Click = (EZValueChangedDelegate)Delegate.Remove(expr_8C.Click, new EZValueChangedDelegate(this.ClickEcoMonDetailInfo));
		}
	}

	public void InitInfo()
	{
		this.m_expeditiondetailinfo.init();
		this.m_solinfo.Init();
		if (this.m_pksolinfo != null)
		{
			for (int i = 0; i < this.m_pksolinfo.Length; i++)
			{
				if (this.m_pksolinfo[i] != null)
				{
					this.m_pksolinfo[i].Init();
				}
			}
		}
	}

	public void SetExpeditionInfo(GS_EXPEDITION_DETAILINFO_ACK occupy_info, eExpeditionSearchDetailInfo_Mode eMode)
	{
		this.InitInfo();
		if (occupy_info != null)
		{
			this.m_expeditiondetailinfo = occupy_info;
			if (occupy_info.bUserInfo)
			{
				this.m_bHaveMilitary = true;
				eMode = eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_ATTACK;
			}
			else
			{
				this.m_bHaveMilitary = false;
				eMode = eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_DEFENCE;
			}
		}
		this.m_eMode = eMode;
		NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
		if (militaryList == null)
		{
			return;
		}
		NkExpeditionMilitaryInfo validExpeditionMilitaryInfo = militaryList.GetValidExpeditionMilitaryInfo(this.m_expeditiondetailinfo.ui8ExpeditionMilitaryUniq);
		if (validExpeditionMilitaryInfo != null)
		{
			this.m_solinfo = validExpeditionMilitaryInfo;
		}
		NkExpeditionMilitaryInfo expeditionMilitaryInfo = militaryList.GetExpeditionMilitaryInfo(this.m_solinfo.GetMilitaryUnique());
		if (expeditionMilitaryInfo == null)
		{
			return;
		}
		NkSoldierInfo[] expeditionSolInfo = expeditionMilitaryInfo.GetExpeditionSolInfo();
		if (expeditionSolInfo != null)
		{
			this.m_pksolinfo = expeditionSolInfo;
		}
		if (eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_SEARCH)
		{
			base.ShowLayer(1, 4);
		}
		else if (eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_ATTACK)
		{
			base.ShowLayer(1, 6);
			if (occupy_info.ui8ExpeditionMilitaryUniq > 0)
			{
				this.m_btStart02.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615");
			}
			else
			{
				this.m_btStart02.Hide(true);
			}
		}
		else if (eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_DEFENCE)
		{
			base.ShowLayer(1, 9);
		}
		this.InitInterface();
		this.ShowExpeditionInfo(eMode);
	}

	public void ShowExpeditionInfo(eExpeditionSearchDetailInfo_Mode eMode)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string str = string.Empty;
		if (eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_SEARCH)
		{
			EXPEDITION_CREATE_DATA expeditionCreateDataFromID = BASE_EXPEDITION_CREATE_DATA.GetExpeditionCreateDataFromID(this.m_expeditionSearch_info.ui8ExpeditionGrade, (int)this.m_expeditionSearch_info.i16xpeditionCreateDataID);
			if (expeditionCreateDataFromID == null)
			{
				return;
			}
			EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(expeditionCreateDataFromID.GetGrade());
			if (expeditionDataFromGrade == null)
			{
				return;
			}
			if (eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_SEARCH)
			{
				DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
				if (directionDLG != null)
				{
					directionDLG.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_MINESEARCH, (int)expeditionCreateDataFromID.GetGrade());
				}
			}
			this.Expedition_ModeCheck(eMode);
			string text3 = string.Empty;
			str = expeditionDataFromGrade.Expedition_BG_NAME;
			text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1322");
			this.m_lTitle.SetText(text3);
			this.m_dtBG.SetTextureFromBundle("UI/Mine/" + str);
			this.m_dtMineIcon2.SetTexture(expeditionDataFromGrade.Expedition_UI_ICON);
			this.m_laExpeditionCurNum.SetText(this.m_expeditionSearch_info.i32ExpeditionNum.ToString());
			this.m_laOccExpeditionNum.SetText(this.m_expeditionSearch_info.i32MonPlunderItemNum.ToString());
			if (100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel)
			{
				text2 = text2 + " " + this.m_expeditionSearch_info.i16xpeditionCreateDataID.ToString();
			}
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1775");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"gold",
				expeditionDataFromGrade.Expedition_SEARCH_MONEY
			});
			this.m_laSearchMoney.SetText(text2);
		}
		else
		{
			EXPEDITION_CREATE_DATA expedtionCreateData = BASE_EXPEDITION_CREATE_DATA.GetExpedtionCreateData(this.m_expeditiondetailinfo.i16ExpeditionCreateDataID);
			if (expedtionCreateData == null)
			{
				return;
			}
			EXPEDITION_DATA expeditionDataFromGrade2 = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(expedtionCreateData.GetGrade());
			if (expeditionDataFromGrade2 == null)
			{
				return;
			}
			this.m_dtMineIcon2.SetTexture(expeditionDataFromGrade2.Expedition_UI_ICON);
			this.m_laExpeditionCurNum.SetText(this.m_expeditiondetailinfo.i32ExpeditionTotalItemNum.ToString());
			this.Expedition_ModeCheck(eMode);
		}
		this.Show();
	}

	public void Expedition_ModeCheck(eExpeditionSearchDetailInfo_Mode eMode)
	{
		string str = string.Empty;
		string text = string.Empty;
		if (eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_SEARCH)
		{
			this.SetEcoMoninfo(eMode);
		}
		else
		{
			EXPEDITION_CREATE_DATA expedtionCreateData = BASE_EXPEDITION_CREATE_DATA.GetExpedtionCreateData(this.m_expeditiondetailinfo.i16ExpeditionCreateDataID);
			if (expedtionCreateData == null)
			{
				return;
			}
			EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(expedtionCreateData.GetGrade());
			if (expeditionDataFromGrade == null)
			{
				return;
			}
			if (this.m_expeditiondetailinfo.ui8ExpeditionState == 2 || this.m_expeditiondetailinfo.ui8ExpeditionState == 1 || this.m_expeditiondetailinfo.ui8ExpeditionState == 4)
			{
				if (this.m_bHaveMilitary)
				{
					this.SetOccupySolInfo(eMode);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2734");
					this.m_lTitle.SetText(text);
					str = expeditionDataFromGrade.Expedition_BG1_NAME;
				}
				else
				{
					this.SetEcoMoninfo(eMode);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2735");
					this.m_lTitle.SetText(text);
					str = expeditionDataFromGrade.Expedition_BG_NAME;
				}
				this.m_laOccExpeditionNum.SetText(this.m_expeditiondetailinfo.i32ExpeditionMonPlunderItemNum.ToString());
			}
			else if (this.m_expeditiondetailinfo.ui8ExpeditionState == 3)
			{
				if (this.m_bHaveMilitary)
				{
					this.SetOccupySolInfo(eMode);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2734");
					this.m_lTitle.SetText(text);
					str = expeditionDataFromGrade.Expedition_BG_NAME;
				}
				else
				{
					this.SetEcoMoninfo(eMode);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2735");
					this.m_lTitle.SetText(text);
					str = expeditionDataFromGrade.Expedition_BG1_NAME;
				}
				this.m_laOccExpeditionNum.SetText(this.m_expeditiondetailinfo.i32ExpeditionRewardItemNum.ToString());
			}
		}
		this.m_dtBG.SetTextureFromBundle("UI/Mine/" + str);
	}

	public void SetEcoMoninfo(eExpeditionSearchDetailInfo_Mode eMode)
	{
		if (eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_SEARCH)
		{
			this.m_dicEcoGroupInfo.Clear();
			EXPEDITION_CREATE_DATA expeditionCreateDataFromID = BASE_EXPEDITION_CREATE_DATA.GetExpeditionCreateDataFromID(this.m_expeditionSearch_info.ui8ExpeditionGrade, (int)this.m_expeditionSearch_info.i16xpeditionCreateDataID);
			if (expeditionCreateDataFromID == null)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				this.SetEcoMoninfo(i, expeditionCreateDataFromID.EXPEDITION_ECO[i]);
			}
			this.SetEcoMonDetailinfo(0);
		}
		else
		{
			this.m_dicEcoGroupInfo.Clear();
			EXPEDITION_CREATE_DATA expedtionCreateData = BASE_EXPEDITION_CREATE_DATA.GetExpedtionCreateData(this.m_expeditiondetailinfo.i16ExpeditionCreateDataID);
			if (expedtionCreateData == null)
			{
				return;
			}
			for (int j = 0; j < 3; j++)
			{
				this.SetEcoMoninfo(j, expedtionCreateData.EXPEDITION_ECO[j]);
			}
			this.SetEcoMonDetailinfo(0);
		}
	}

	public void SetEcoMoninfo(int index, int groupunique)
	{
		ECO eco = NrTSingleton<NrBaseTableManager>.Instance.GetEco(groupunique.ToString());
		if (eco != null)
		{
			this.m_itOccMilitary[index].Visible = true;
			this.m_itOccMilitary[index].SetSolImageTexure(eCharImageType.SMALL, NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco.szCharCode[0]), -1);
			this.m_btOccMilitary[index].Data = index;
			this.m_dicEcoGroupInfo.Add(index, eco);
			Button expr_6F = this.m_btOccMilitary[index];
			expr_6F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_6F.Click, new EZValueChangedDelegate(this.ClickEcoMonDetailInfo));
		}
	}

	public void SetEcoMonDetailinfo(int index)
	{
		if (this.m_dicEcoGroupInfo.ContainsKey(index))
		{
			this.m_select_index = index;
			this.m_btOccMilitary[this.m_select_index].Visible = false;
			this.m_itOccMilitary[this.m_select_index].Visible = false;
			this.m_itOccMilitary[this.m_select_index].SetText(string.Empty);
			this.m_btOccMilitarySelect[this.m_select_index].Visible = true;
			this.m_itOccMilitarySelect[this.m_select_index].Visible = true;
			this.m_itOccMilitarySelect[this.m_select_index].SetSolImageTexure(eCharImageType.SMALL, NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(this.m_dicEcoGroupInfo[index].szCharCode[0]), -1);
			if (this.m_old_select_index >= 0)
			{
				if (this.m_dicEcoGroupInfo.ContainsKey(this.m_old_select_index))
				{
					this.m_btOccMilitary[this.m_old_select_index].Visible = true;
					this.m_itOccMilitary[this.m_old_select_index].Visible = true;
					this.m_itOccMilitary[this.m_old_select_index].SetSolImageTexure(eCharImageType.SMALL, NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(this.m_dicEcoGroupInfo[this.m_old_select_index].szCharCode[0]), -1);
				}
				this.m_itOccMilitarySelect[this.m_old_select_index].Visible = false;
				this.m_btOccMilitarySelect[this.m_old_select_index].Visible = false;
				this.m_itOccMilitarySelect[this.m_old_select_index].SetText(string.Empty);
			}
			this.m_old_select_index = this.m_select_index;
			this.SetEcoMonDetailinfo(this.m_dicEcoGroupInfo[index]);
		}
	}

	public void SetEcoMonDetailinfo(ECO eco_info)
	{
		this.InitOccSolInfo();
		string empty = string.Empty;
		if (this.m_eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_SEARCH)
		{
			if (eco_info != null)
			{
				for (int i = 0; i < 6; i++)
				{
					if ((eco_info.nBattlePos[i] >= 0 || eco_info.nBattlePos[i] < 9) && NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco_info.szCharCode[i]) > 0)
					{
						NkListSolInfo nkListSolInfo = new NkListSolInfo();
						nkListSolInfo.SolLevel = this.m_expeditionSearch_info.i16MonLevel;
						nkListSolInfo.SolCharKind = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco_info.szCharCode[i]);
						nkListSolInfo.ShowLevel = true;
						nkListSolInfo.ShowCombat = false;
						this.m_itOccSol[eco_info.nBattlePos[i]].SetSolImageTexure(eCharImageType.SMALL, nkListSolInfo, false);
					}
				}
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1639"),
					"count",
					this.m_expeditionSearch_info.i16MonLevel.ToString(),
					"targetname",
					NrTSingleton<NrCharKindInfoManager>.Instance.GetName(NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco_info.szCharCode[0]))
				});
			}
		}
		else if (eco_info != null)
		{
			for (int j = 0; j < 6; j++)
			{
				if ((eco_info.nBattlePos[j] >= 0 || eco_info.nBattlePos[j] < 9) && NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco_info.szCharCode[j]) > 0)
				{
					NkListSolInfo nkListSolInfo2 = new NkListSolInfo();
					nkListSolInfo2.SolLevel = this.m_expeditiondetailinfo.i16ExpeditionMonLevel;
					nkListSolInfo2.SolCharKind = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco_info.szCharCode[j]);
					nkListSolInfo2.ShowLevel = true;
					nkListSolInfo2.ShowCombat = false;
					this.m_itOccSol[eco_info.nBattlePos[j]].SetSolImageTexure(eCharImageType.SMALL, nkListSolInfo2, false);
				}
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1639"),
				"count",
				this.m_expeditiondetailinfo.i16ExpeditionMonLevel.ToString(),
				"targetname",
				NrTSingleton<NrCharKindInfoManager>.Instance.GetName(NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco_info.szCharCode[0]))
			});
		}
		this.m_laOccSelectMemberName.SetText(empty);
	}

	public bool IsStartBattle()
	{
		int num = 0;
		EXPEDITION_CREATE_DATA expeditionCreateDataFromID = BASE_EXPEDITION_CREATE_DATA.GetExpeditionCreateDataFromID(this.m_expeditionSearch_info.ui8ExpeditionGrade, (int)this.m_expeditionSearch_info.i16xpeditionCreateDataID);
		if (expeditionCreateDataFromID == null)
		{
			return false;
		}
		EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(expeditionCreateDataFromID.GetGrade());
		if (expeditionDataFromGrade == null)
		{
			return false;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return false;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolID() > 0L)
			{
				if (current.GetSolPosType() != 6)
				{
					if (current.GetSolPosType() != 2)
					{
						int expeditionMoneyFromSolPossibleLevel = BASE_EXPEDITION_DATA.GetExpeditionMoneyFromSolPossibleLevel(expeditionDataFromGrade.GetGrade());
						if ((int)current.GetLevel() >= expeditionMoneyFromSolPossibleLevel)
						{
							num++;
						}
					}
				}
			}
		}
		return num > 0;
	}

	public void ClickEcoMonDetailInfo(IUIObject obj)
	{
		int num = (int)obj.Data;
		if (num >= 0)
		{
			this.SetEcoMonDetailinfo(num);
		}
	}

	public int GetSolPosIndex(int Index)
	{
		int num = -1;
		for (int i = 0; i < 15; i++)
		{
			if (this.m_pksolinfo[i] != null)
			{
				if ((int)this.m_pksolinfo[i].m_kBase.SolPosIndex == Index)
				{
					if ((int)this.m_pksolinfo[i].m_kBase.SolPosIndex == Index)
					{
						if (num > (int)this.m_pksolinfo[i].m_kBase.BattlePos)
						{
							num = (int)this.m_pksolinfo[i].m_kBase.BattlePos;
							this.m_SolInfoNumber = i;
						}
						else if (num == -1)
						{
							num = (int)this.m_pksolinfo[i].m_kBase.BattlePos;
							this.m_SolInfoNumber = i;
						}
					}
				}
			}
		}
		return this.m_SolInfoNumber;
	}

	public void SetOccupySolInfo(eExpeditionSearchDetailInfo_Mode eMode)
	{
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			int solPosIndex = this.GetSolPosIndex(i);
			if (solPosIndex < 0)
			{
				return;
			}
			byte solPosIndex2 = this.m_pksolinfo[solPosIndex].m_kBase.SolPosIndex;
			this.m_itOccMilitary[(int)solPosIndex2].Visible = true;
			this.m_btOccMilitary[(int)solPosIndex2].Visible = true;
			this.m_itOccMilitary[(int)solPosIndex2].SetSolImageTexure(eCharImageType.SMALL, this.m_pksolinfo[solPosIndex].m_kBase.CharKind, (int)this.m_pksolinfo[solPosIndex].m_kBase.Grade);
			this.m_btOccMilitary[(int)solPosIndex2].Data = solPosIndex2;
			Button expr_A6 = this.m_btOccMilitary[(int)solPosIndex2];
			expr_A6.Click = (EZValueChangedDelegate)Delegate.Combine(expr_A6.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
		}
		this.SetOccupyDetailinfo(0);
	}

	public void ClickOccupyDetailInfo(IUIObject obj)
	{
		byte occupyDetailinfo = (byte)obj.Data;
		this.SetOccupyDetailinfo((int)occupyDetailinfo);
	}

	public void SetOccupyDetailinfo(int Index)
	{
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		this.m_SolInfoNumber = this.GetSolPosIndex(Index);
		if (this.m_pksolinfo[this.m_SolInfoNumber] != null)
		{
			this.m_select_index = Index;
			if (this.m_eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_ATTACK)
			{
				this.m_btOccMilitary[this.m_select_index].Visible = false;
				this.m_itOccMilitary[this.m_select_index].Visible = false;
				this.m_itOccMilitary[this.m_select_index].SetText(string.Empty);
				this.m_dOccSelectImage[this.m_select_index].Visible = true;
				this.m_btOccMilitarySelect[this.m_select_index].Visible = true;
				this.m_itOccMilitarySelect[this.m_select_index].Visible = true;
				this.m_itOccMilitarySelect[this.m_select_index].SetSolImageTexure(eCharImageType.SMALL, this.m_pksolinfo[this.m_SolInfoNumber].m_kBase.CharKind, (int)this.m_pksolinfo[this.m_SolInfoNumber].m_kBase.Grade);
				if (this.m_old_select_index >= 0)
				{
					if (this.m_pksolinfo[this.m_old_select_index] != null)
					{
						this.m_btOccMilitary[this.m_old_select_index].Visible = true;
						this.m_itOccMilitary[this.m_old_select_index].Visible = true;
						if (this.m_eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_ATTACK)
						{
							this.m_itOccMilitary[this.m_old_select_index].SetSolImageTexure(eCharImageType.SMALL, this.m_pksolinfo[this.m_old_SolInfoNumber].m_kBase.CharKind, (int)this.m_pksolinfo[this.m_old_SolInfoNumber].m_kBase.Grade);
						}
						else
						{
							this.m_itOccMilitary[this.m_old_select_index].SetSolImageTexure(eCharImageType.SMALL, this.m_pksolinfo[this.m_old_SolInfoNumber].m_kBase.CharKind, -1);
						}
					}
					this.m_itOccMilitarySelect[this.m_old_select_index].Visible = false;
					this.m_btOccMilitarySelect[this.m_old_select_index].Visible = false;
					this.m_itOccMilitarySelect[this.m_old_select_index].SetText(string.Empty);
				}
			}
			else if (this.m_eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_DEFENCE)
			{
				this.m_btOccMilitary[this.m_select_index].Visible = false;
				this.m_itOccMilitary[this.m_select_index].Visible = false;
				this.m_itOccMilitary[this.m_select_index].SetText(string.Empty);
				this.m_btOccMilitarySelect[this.m_select_index].Visible = true;
				this.m_itOccMilitarySelect[this.m_select_index].Visible = true;
				if (this.m_eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_DEFENCE)
				{
					this.m_itOccMilitarySelect[this.m_select_index].SetSolImageTexure(eCharImageType.SMALL, this.m_pksolinfo[this.m_SolInfoNumber].m_kBase.CharKind, (int)this.m_pksolinfo[this.m_SolInfoNumber].m_kBase.Grade);
				}
				else
				{
					this.m_itOccMilitarySelect[this.m_select_index].SetSolImageTexure(eCharImageType.SMALL, this.m_pksolinfo[this.m_SolInfoNumber].m_kBase.CharKind, -1);
				}
				if (this.m_old_select_index >= 0)
				{
					if (this.m_pksolinfo[this.m_old_select_index] != null)
					{
						this.m_btOccMilitary[this.m_old_select_index].Visible = true;
						this.m_itOccMilitary[this.m_old_select_index].Visible = true;
						if (this.m_eMode == eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_DEFENCE)
						{
							this.m_itOccMilitary[this.m_old_select_index].SetSolImageTexure(eCharImageType.SMALL, this.m_pksolinfo[this.m_old_SolInfoNumber].m_kBase.CharKind, (int)this.m_pksolinfo[this.m_old_SolInfoNumber].m_kBase.Grade);
						}
						else
						{
							this.m_itOccMilitary[this.m_old_select_index].SetSolImageTexure(eCharImageType.SMALL, this.m_pksolinfo[this.m_old_SolInfoNumber].m_kBase.CharKind, -1);
						}
					}
					this.m_itOccMilitarySelect[this.m_old_select_index].Visible = false;
					this.m_btOccMilitarySelect[this.m_old_select_index].Visible = false;
					this.m_itOccMilitarySelect[this.m_old_select_index].SetText(string.Empty);
				}
			}
			this.m_old_select_index = this.m_select_index;
			this.m_old_SolInfoNumber = this.m_SolInfoNumber;
			if (this.m_pksolinfo[0].m_kBase != null)
			{
				this.SetOccupyDetailinfo(this.m_pksolinfo[this.m_SolInfoNumber]);
			}
		}
		else
		{
			this.InitInterface(Index);
		}
	}

	public void SetOccupyDetailinfo(NkSoldierInfo info)
	{
		this.InitOccSolInfo();
		string empty = string.Empty;
		string str = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1639"),
			"count",
			info.m_kBase.Level.ToString(),
			"targetname",
			info.GetName()
		});
		bool flag = false;
		if (this.m_expeditiondetailinfo.ui8ExpeditionState == 1)
		{
			str = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1525");
			flag = true;
		}
		else if (this.m_expeditiondetailinfo.ui8ExpeditionState == 4)
		{
			str = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1526");
			flag = true;
		}
		if (flag)
		{
			this.m_laOccSelectMemberName.SetText(empty + " (" + str + ")");
		}
		else
		{
			this.m_laOccSelectMemberName.SetText(empty);
		}
		if (info != null)
		{
			for (int i = 0; i < 15; i++)
			{
				if (info.m_kBase.BattlePos >= 0)
				{
					if (info.m_kBase.CharKind > 0)
					{
						if (this.m_pksolinfo[i] != null)
						{
							if (this.m_pksolinfo[i].m_kBase.SolPosIndex == info.m_kBase.SolPosIndex)
							{
								NkListSolInfo nkListSolInfo = new NkListSolInfo();
								nkListSolInfo.ShowCombat = true;
								nkListSolInfo.FightPower = (long)this.m_pksolinfo[i].GetFightPower();
								nkListSolInfo.SolLevel = this.m_pksolinfo[i].m_kBase.Level;
								nkListSolInfo.SolCharKind = this.m_pksolinfo[i].m_kBase.CharKind;
								nkListSolInfo.SolGrade = (int)this.m_pksolinfo[i].m_kBase.Grade;
								nkListSolInfo.ShowLevel = false;
								EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(nkListSolInfo.SolCharKind, (byte)nkListSolInfo.SolGrade);
								if (eventHeroCharCode != null)
								{
									this.m_dtOccSolBG[(int)this.m_pksolinfo[i].m_kBase.BattlePos].SetTexture("Win_I_EventSol");
								}
								else
								{
									UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(nkListSolInfo.SolCharKind, (int)((byte)nkListSolInfo.SolGrade));
									if (legendFrame != null)
									{
										this.m_dtOccSolBG[(int)this.m_pksolinfo[i].m_kBase.BattlePos].SetTexture(legendFrame);
									}
								}
								this.m_itOccSol[(int)this.m_pksolinfo[i].m_kBase.BattlePos].SetSolImageTexure(eCharImageType.SMALL, nkListSolInfo, false);
							}
						}
					}
				}
			}
		}
	}

	public void OnBtnClickResearch(IUIObject obj)
	{
		string message = string.Empty;
		EXPEDITION_CREATE_DATA expeditionCreateDataFromID = BASE_EXPEDITION_CREATE_DATA.GetExpeditionCreateDataFromID(this.m_expeditionSearch_info.ui8ExpeditionGrade, (int)this.m_expeditionSearch_info.i16xpeditionCreateDataID);
		if (expeditionCreateDataFromID == null)
		{
			return;
		}
		EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(expeditionCreateDataFromID.GetGrade());
		if (expeditionDataFromGrade == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (expeditionDataFromGrade.Expedition_SEARCH_MONEY > kMyCharInfo.m_Money)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		long num = 0L;
		EXPEDITION_CONSTANT_MANAGER instance = EXPEDITION_CONSTANT_MANAGER.GetInstance();
		if (instance != null)
		{
			num = (long)instance.GetValue(eEXPEDITION_CONSTANT.eEXPEDITION_DAY_COUNT);
		}
		if (num > 0L && kMyCharInfo.GetCharDetail(10) >= num)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("405");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!this.IsStartBattle())
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("528");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_EXPEDITION_SERACH_REQ gS_EXPEDITION_SERACH_REQ = new GS_EXPEDITION_SERACH_REQ();
		gS_EXPEDITION_SERACH_REQ.i8Grade = expeditionCreateDataFromID.GetGrade();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_SERACH_REQ, gS_EXPEDITION_SERACH_REQ);
	}

	public void InitOccSolInfo()
	{
		for (int i = 0; i < 9; i++)
		{
			this.m_itOccSol[i].ClearData();
			this.m_dtOccSolBG[i].SetTexture("Win_T_ItemEmpty");
		}
	}

	public void OnBtnClickClose(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickBackMove(IUIObject obj)
	{
		if ((this.m_expeditiondetailinfo.ui8ExpeditionState == 1 || this.m_expeditiondetailinfo.ui8ExpeditionState == 3) && this.m_expeditiondetailinfo.i64BattleTime == this.m_expeditiondetailinfo.i64CheckBattleTime)
		{
			if (this.m_expeditiondetailinfo != null)
			{
				GS_EXPEDITION_MILITARY_BACKMOVE_REQ gS_EXPEDITION_MILITARY_BACKMOVE_REQ = new GS_EXPEDITION_MILITARY_BACKMOVE_REQ();
				gS_EXPEDITION_MILITARY_BACKMOVE_REQ.i64ExpeditionID = this.m_expeditiondetailinfo.i64ExpeditionID;
				gS_EXPEDITION_MILITARY_BACKMOVE_REQ.i16ExpeditionCreateID = this.m_expeditiondetailinfo.i16ExpeditionCreateDataID;
				gS_EXPEDITION_MILITARY_BACKMOVE_REQ.byExpeditionMilitaryUniq = this.m_expeditiondetailinfo.ui8ExpeditionMilitaryUniq;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_MILITARY_BACKMOVE_REQ, gS_EXPEDITION_MILITARY_BACKMOVE_REQ);
				this.Close();
			}
		}
		else if (this.m_expeditiondetailinfo.ui8ExpeditionState == 2)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("319");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		else if (this.m_expeditiondetailinfo.ui8ExpeditionState == 4)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("406");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	public void OnBtnClickGoMilitary(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "ATTACK-FORMATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP;
		SoldierBatch.EXPEDITION_INFO.ExpeditionCreateDataID = (int)this.m_expeditionSearch_info.i16xpeditionCreateDataID;
		SoldierBatch.EXPEDITION_INFO.m_eExpeditionGrade = (eEXPEDITION_GRADE)this.m_expeditionSearch_info.ui8ExpeditionGrade;
		FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
	}

	public void SetOriKeeping(bool bShow)
	{
		this.m_dtOriKeepingHelpBg.Visible = bShow;
		this.m_lOriKeepingHelpText.Visible = bShow;
		this.m_dtOriKeepingHelpTail.Visible = bShow;
		if (bShow)
		{
			this.m_dtOriPlunderHelpBg.Visible = false;
			this.m_lOriPlunderHelpText.Visible = false;
			this.m_dtOriPlunderHelpTail.Visible = false;
			this.m_bOriPlunder = false;
		}
	}

	public void SetOriPlunder(bool bShow)
	{
		this.m_dtOriPlunderHelpBg.Visible = bShow;
		this.m_lOriPlunderHelpText.Visible = bShow;
		this.m_dtOriPlunderHelpTail.Visible = bShow;
		if (bShow)
		{
			this.m_dtOriKeepingHelpBg.Visible = false;
			this.m_lOriKeepingHelpText.Visible = false;
			this.m_dtOriKeepingHelpTail.Visible = false;
			this.m_bOriKeeping = false;
		}
	}

	public void OnClickOriKeepingHelpText(IUIObject obj)
	{
		if (this.m_bOriKeeping)
		{
			this.m_bOriKeeping = false;
		}
		else
		{
			this.m_bOriKeeping = true;
			this.SetOriPlunder(false);
		}
		this.SetOriKeeping(this.m_bOriKeeping);
	}

	public void OnClickOriPlunderHelpText(IUIObject obj)
	{
		if (this.m_bOriPlunder)
		{
			this.m_bOriPlunder = false;
		}
		else
		{
			this.m_bOriPlunder = true;
			this.SetOriKeeping(false);
		}
		this.SetOriPlunder(this.m_bOriPlunder);
	}

	public void SetExpeditionSearchData(byte _ui8ExpeditionGrade, short _i16ExpeditionID, int _i32MineNum, short _i16MonLevel, int _i32MonPlunderItemNum)
	{
		this.m_expeditionSearch_info.Init();
		this.m_expeditionSearch_info.Set(_ui8ExpeditionGrade, _i16ExpeditionID, _i32MineNum, _i16MonLevel, _i32MonPlunderItemNum);
	}
}
