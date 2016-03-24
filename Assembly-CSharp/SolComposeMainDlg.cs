using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class SolComposeMainDlg : Form
{
	private class SOLKINDTONUM
	{
		public int Kind;

		public int Count;

		public bool ZeroExpSol;

		public bool bBattle;

		public bool bReady;

		public bool bWareHouse;
	}

	private const int GRADE_MAX = 5;

	protected const string BUTTON_ADDIMG_KEY = "Win_B_Addlist";

	protected const string BUTTON_CHANGEIMG_KEY = "Win_B_Change";

	private const int SOL_COMPOSE_LIMIT_GRADE = 3;

	private int USE_HEARTS_NUM;

	private List<int> guideWinIDList = new List<int>();

	private UIButton _Touch;

	public SOLCOMPOSE_TYPE m_ShowType;

	protected Toolbar m_Toolbar;

	protected Button btnBaseSelect;

	private Button btnBaseImgSelect;

	protected DrawTexture dtBase;

	protected Label lbBaseName;

	protected Button btnSubSelect;

	protected Button btnSubSelectMain;

	private Button m_HelpButton;

	private Button m_btnAddGold;

	private Button m_btnAddGoldTrans;

	private Label lbSubNum;

	private NewListBox lxList;

	protected DrawTexture dtGage;

	protected DrawTexture dtExpBG;

	protected Button btnOk;

	protected Label lbMoney;

	protected Label lblComposeCost;

	protected Label lblExp;

	protected Label lblGradeText;

	protected DrawTexture SolRank;

	protected Label lblComposeText;

	protected Label lblBaseGuideText;

	protected Label lblMaterialGuideText;

	protected Label lblBaseSeasonText;

	protected DrawTexture GradeExpBG;

	protected DrawTexture GradeExpGage;

	protected Label GradeExpText;

	private NewListBox lxList2;

	private DrawTexture dtSelBaseSol;

	private Label lbMoney2;

	private Label lbSelMoney;

	private Label lbSubNum2;

	private Label lbExplain;

	private Label lbSellGuide;

	private Button btnSell;

	private Button btnSubSelectMain2;

	private Button btnSubSelect2;

	protected Button btnRecommend;

	private NewListBox lxList3;

	private NewListBox lxSelectList;

	private Label lbExtract_Heart;

	private Label lbSolNameSpace;

	private Button btnExtractsolBase1;

	private Button btnExtractsolBase2;

	private Button btnHearts1;

	private Button btnHearts2;

	private Label lbNowHearts;

	protected CheckBox chHeartsUse;

	private Button btnCheckHeartsUse;

	private Label lbExtractHelpText;

	private Label lbExtractSolText1;

	private Label lbExtarct_Title;

	private DrawTexture dwExtractSolImpossible;

	private DrawTexture dwExtractBG;

	private DrawTexture dwExtractTextBG;

	private DrawTexture dwExtractTitleBG;

	private Button btnExtractStart;

	private Label lbExtractSolText2;

	private Label lbExtractSolText3;

	protected Label lbExtractPercentage;

	public bool m_bUseHearts;

	public NkSoldierInfo mBaseSol;

	public byte mBaseSolSeason;

	protected List<long> mSubSolList = new List<long>();

	protected List<long> m_SolExtract = new List<long>();

	private List<int> m_RecommandSol = new List<int>();

	private long mComposeCost;

	private long mMaxLvEvelution;

	private long mSellCost;

	protected float GAGE_SRC_WIDTH;

	private bool m_bGradeUp;

	private List<long> m_SaveSubSolIDList = new List<long>();

	protected Button btn_Transcendence_StartButton;

	protected Button btn_TranscendenceBase_Button1;

	protected Button btn_TranscendenceUse_Add1;

	protected Button btn_TranscendenceBase_Button2;

	protected Button btn_TranscendenceBase_Button3;

	protected Button btn_TranscendenceUse_Add2;

	protected Button btn_TranscendenceUse_Add3;

	protected Label lb_Label_AddPercentage;

	protected DrawTexture dt_DrawTexture_PercentagePlus;

	protected Label lb_Transcendence_AddPercentage_Text;

	protected Label lb_Transcendence_AddPercentage;

	protected Label lb_Tarnscendence_ExpGet;

	protected Label lb_TarnscendenceBaseSeasont;

	protected DrawTexture dt_Transcendence_GaugeBar;

	protected Label lb_TranscendenceBase_SolName;

	protected Label lb_Transcendence_Money;

	protected Label lb_TranscendenceUseMoney;

	protected Label lb_Transcendence_SuccessRate_Text;

	protected Label lb_Transcendence_SuccessRate;

	protected Label lb_TranscendenceBase_Help;

	protected Label lb_Transcendence_Help;

	protected Label lb_Transcendence_Help2;

	protected DrawTexture dt_TranscendenceBase_SolImg;

	protected DrawTexture dt_TranscendenceBase_SolRank;

	public NewListBox lxList4;

	protected float GAGE_TRANSCENDENCE_WIDTH;

	public static SolComposeMainDlg Instance
	{
		get
		{
			SolComposeMainDlg solComposeMainDlg = (SolComposeMainDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_DLG);
			if (solComposeMainDlg == null)
			{
				solComposeMainDlg = (SolComposeMainDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG);
			}
			return solComposeMainDlg;
		}
	}

	public int SELECT_COUNT
	{
		get
		{
			return this.mSubSolList.Count;
		}
	}

	public long COST
	{
		get
		{
			SOLCOMPOSE_TYPE showType = this.m_ShowType;
			if (showType == SOLCOMPOSE_TYPE.COMPOSE)
			{
				return this.mComposeCost;
			}
			if (showType != SOLCOMPOSE_TYPE.SELL)
			{
				return 0L;
			}
			return this.mSellCost;
		}
	}

	public long[] SUB_ARRAY
	{
		get
		{
			return this.mSubSolList.ToArray();
		}
	}

	public long[] SUB_EXTRACTARRAY
	{
		get
		{
			return this.m_SolExtract.ToArray();
		}
	}

	public SOLCOMPOSE_TYPE GetSolComposeType()
	{
		return this.m_ShowType;
	}

	public static NkReadySolList GetSoldierReadyList()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo != null)
		{
			return NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		}
		return null;
	}

	public static NkSoldierInfo GetSoldierInfo(long SoldID)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			return charPersonInfo.GetSoldierInfoFromSolID(SoldID);
		}
		return null;
	}

	public static long GetSelCost(NkSoldierInfo kSoldInfo)
	{
		long result = 0L;
		if (kSoldInfo != null)
		{
			NrCharKindInfo charKindInfo = kSoldInfo.GetCharKindInfo();
			long sellPrice = charKindInfo.GetSellPrice((int)kSoldInfo.GetGrade());
			int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_SOL_TRADE1);
			int value2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_SOL_TRADE2);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < 6; i++)
			{
				num2 = kSoldInfo.GetBattleSkillUnique(i);
				if (0 < num2)
				{
					num = kSoldInfo.GetBattleSkillLevel(num2);
					if (NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(num2, num) != null)
					{
						if (0 < num)
						{
							break;
						}
					}
				}
			}
			for (int j = 1; j <= num; j++)
			{
				BATTLESKILL_TRAINING battleSkillTraining = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(num2, j);
				num3 += battleSkillTraining.m_nSkillNeedGold;
			}
			result = sellPrice + (long)((int)kSoldInfo.GetLevel() * value / 100) + (long)(num3 * value2 / 100);
		}
		return result;
	}

	public static long GetComposeCost(NkSoldierInfo kSoldInfo)
	{
		long num = 0L;
		if (kSoldInfo != null && SolComposeMainDlg.Instance != null)
		{
			NrCharKindInfo charKindInfo = kSoldInfo.GetCharKindInfo();
			long composeCost = charKindInfo.GetComposeCost((int)kSoldInfo.GetGrade());
			num = (long)((float)composeCost + (float)composeCost * ((float)kSoldInfo.GetLevel() * 20f));
			if (kSoldInfo.GetCharKind() == SolComposeMainDlg.Instance.mBaseSol.GetCharKind())
			{
				NrCharKindInfo charKindInfo2 = kSoldInfo.GetCharKindInfo();
				if (charKindInfo2 != null)
				{
					num += charKindInfo2.GetEvolutionCost((int)kSoldInfo.GetGrade());
				}
			}
		}
		return num;
	}

	public void GetComposeExp(ref short iBaseLevel, NkSoldierInfo kSoldInfo, ref long ComposeExp, ref long ComposeEvolution, NkSoldierInfo BaseSol, ref long MaxLvEvolution)
	{
		long num = 0L;
		if (kSoldInfo != null)
		{
			byte @base = (byte)BaseSol.GetSeason();
			NrCharKindInfo charKindInfo = kSoldInfo.GetCharKindInfo();
			long nextExp = NrTSingleton<NkLevelManager>.Instance.GetNextExp(BaseSol.GetClassInfo().EXP_TYPE, iBaseLevel);
			if (nextExp <= BaseSol.GetExp() + ComposeExp)
			{
				this.GetNextlevel(ref iBaseLevel, BaseSol.GetExp() + ComposeExp, BaseSol);
			}
			float num2 = 1f;
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			if (instance != null)
			{
				num2 = Mathf.Max((float)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_SOLCOMPOSE), num2) / 100f;
			}
			long exp = NrTSingleton<ComposeExpManager>.Instance.GetExp(iBaseLevel, (int)kSoldInfo.GetGrade());
			num = charKindInfo.GetComposeExp((int)kSoldInfo.GetGrade()) * exp / 100L + (long)((float)kSoldInfo.GetExp() * num2);
			if (BaseSol.GetCharKind() == charKindInfo.GetCharKind())
			{
				long evolutionExp = charKindInfo.GetEvolutionExp((int)kSoldInfo.GetGrade());
				long num3 = kSoldInfo.GetEvolutionExp() - NrTSingleton<NrCharKindInfoManager>.Instance.GetSolEvolutionNeedEXP(kSoldInfo.GetCharKind(), (int)kSoldInfo.GetGrade());
				long num4 = 0L;
				if (0L > num3)
				{
					num3 = 0L;
				}
				ComposeEvolution += evolutionExp + num3 + num4;
				this.mMaxLvEvelution += num4;
			}
			else if (instance != null)
			{
				int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_EVOLUTION_EXP_RATE);
				int num5 = 10000;
				long num6 = 0L;
				BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = charKindInfo.GetCHARKIND_SOLGRADEINFO((int)kSoldInfo.GetGrade());
				if (cHARKIND_SOLGRADEINFO != null)
				{
					num5 = NrTSingleton<Evolution_EXP_Penalty_Manager>.Instance.GetSeasonExpPenalty(@base, (byte)cHARKIND_SOLGRADEINFO.SolSeason);
					if (kSoldInfo.IsMaxLevel())
					{
						num6 = cHARKIND_SOLGRADEINFO.MaxLv_Evolution_Exp;
					}
				}
				ComposeEvolution += charKindInfo.GetEvolutionExp((int)kSoldInfo.GetGrade()) * (long)value / 100L * (long)num5 / 10000L + num6;
				this.mMaxLvEvelution += num6;
			}
		}
		ComposeExp += num;
	}

	public void GetSubSolSkillMoneySum(NkSoldierInfo kSoldInfo, ref long nSkillMoneySum)
	{
		long num = 0L;
		if (kSoldInfo != null)
		{
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < 6; i++)
			{
				num3 = kSoldInfo.GetBattleSkillUnique(i);
				if (0 < num3)
				{
					num2 = kSoldInfo.GetBattleSkillLevel(num3);
					if (NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(num3, num2) != null)
					{
						if (0 < num2)
						{
							break;
						}
					}
				}
			}
			if (num2 > 1)
			{
				for (int j = 2; j <= num2; j++)
				{
					BATTLESKILL_TRAINING battleSkillTraining = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(num3, j);
					num += (long)battleSkillTraining.m_nSkillNeedGold;
				}
			}
		}
		nSkillMoneySum += num;
	}

	public bool IsGradeUp()
	{
		return this.m_bGradeUp;
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/Compose/DLG_SolComposeMain_New", G_ID.SOLCOMPOSE_MAIN_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.btnBaseSelect = (base.GetControl("Button_BaseSol01") as Button);
		this.btnBaseSelect.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBase));
		this.btnBaseImgSelect = (base.GetControl("Button_BaseSol02") as Button);
		this.btnBaseImgSelect.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBase));
		this.btnSubSelect = (base.GetControl("Button_MaterialSol") as Button);
		this.btnSubSelect.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickSub));
		this.btnSubSelect2 = (base.GetControl("Button_MaterialSol2") as Button);
		this.btnSubSelect2.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickSub));
		this.lbSubNum = (base.GetControl("Label_sub02") as Label);
		this.btnSubSelectMain = (base.GetControl("Button_MaterialTransBTN1") as Button);
		this.btnSubSelectMain.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickSub));
		this.btnSubSelectMain2 = (base.GetControl("Button_MaterialTransBTN2") as Button);
		this.btnSubSelectMain2.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickSub));
		this.btnRecommend = (base.GetControl("Button_Recommend") as Button);
		this.btnRecommend.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickRecommand));
		this.lxList = (base.GetControl("Sol_ListBox") as NewListBox);
		this.lxList.AutoListBox = false;
		this.lxList2 = (base.GetControl("Sol_ListBox2") as NewListBox);
		this.lxList2.AutoListBox = false;
		this.lxList3 = (base.GetControl("Sol_ListBox3") as NewListBox);
		this.lxList3.AutoListBox = false;
		this.lxSelectList = (base.GetControl("NLB_ExtractSol_Base") as NewListBox);
		this.m_Toolbar = (base.GetControl("ToolBar_01") as Toolbar);
		this.m_Toolbar.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("55");
		this.m_Toolbar.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("56");
		this.m_Toolbar.Control_Tab[2].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2835");
		this.m_Toolbar.Control_Tab[3].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2874");
		UIPanelTab expr_27A = this.m_Toolbar.Control_Tab[0];
		expr_27A.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_27A.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_2A9 = this.m_Toolbar.Control_Tab[1];
		expr_2A9.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_2A9.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_2D8 = this.m_Toolbar.Control_Tab[2];
		expr_2D8.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_2D8.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_307 = this.m_Toolbar.Control_Tab[3];
		expr_307.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_307.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		if (NrTSingleton<ContentsLimitManager>.Instance.IsExtract())
		{
			this.m_Toolbar.Control_Tab[2].Visible = false;
		}
		if (NrTSingleton<ContentsLimitManager>.Instance.IsTranscendence())
		{
			this.m_Toolbar.Control_Tab[3].Visible = false;
		}
		this.dtBase = (base.GetControl("DrawTexture_BaseSolimg") as DrawTexture);
		this.lbBaseName = (base.GetControl("Label_BaseSolName") as Label);
		this.lblBaseGuideText = (base.GetControl("Label_BaseGuideText") as Label);
		this.lblMaterialGuideText = (base.GetControl("Label_MaterialGuideText") as Label);
		this.lblBaseSeasonText = (base.GetControl("Label_BaseSeason") as Label);
		this.SolRank = (base.GetControl("DrawTexture_SolRank") as DrawTexture);
		this.lblComposeCost = (base.GetControl("Label_Cost") as Label);
		this.lblExp = (base.GetControl("Label_BaseExp") as Label);
		this.lblGradeText = (base.GetControl("Label_GradeTextDetail") as Label);
		this.lblComposeText = (base.GetControl("Label_ComposeText") as Label);
		this.lblComposeText.Visible = false;
		this.dtExpBG = (base.GetControl("DrawTexture_EXPBG") as DrawTexture);
		this.dtGage = (base.GetControl("DrawTexture_GaugeBar") as DrawTexture);
		this.GAGE_SRC_WIDTH = this.dtGage.GetSize().x;
		this.lbMoney = (base.GetControl("Label_Gold") as Label);
		this.btnOk = (base.GetControl("Sol_Compose_Button") as Button);
		this.btnOk.Click = new EZValueChangedDelegate(this.OnClickStart);
		this.GradeExpBG = (base.GetControl("DrawTexture_GradePRGBG") as DrawTexture);
		this.GradeExpGage = (base.GetControl("DrawTexture_GradePRG") as DrawTexture);
		this.GradeExpText = (base.GetControl("Label_GradeText") as Label);
		this.dtSelBaseSol = (base.GetControl("layer2_sol1") as DrawTexture);
		this.dtSelBaseSol.SetTexture(eCharImageType.LARGE, 242, -1, string.Empty);
		this.lbMoney2 = (base.GetControl("layer2_Gold") as Label);
		this.lbSelMoney = (base.GetControl("layer2_Cost") as Label);
		this.btnSell = (base.GetControl("Sol_sale_Button") as Button);
		this.btnSell.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSell));
		this.lbSubNum2 = (base.GetControl("layer2_Label02") as Label);
		this.lbExplain = (base.GetControl("layer2_Label01") as Label);
		this.lbSellGuide = (base.GetControl("Label_SellGuideText") as Label);
		this.lbExtract_Heart = (base.GetControl("Label_Extract_HeartsUseInfo") as Label);
		this.lbSolNameSpace = (base.GetControl("SolExtract_SolNameSpace") as Label);
		this.btnExtractsolBase1 = (base.GetControl("Button_ExtractSol_Base") as Button);
		this.btnExtractsolBase1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickAddExtractSol));
		this.btnExtractsolBase2 = (base.GetControl("Add_ExtractSol_Button") as Button);
		this.btnExtractsolBase2.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickAddExtractSol));
		this.btnCheckHeartsUse = (base.GetControl("Button_ExtractHeartsUse") as Button);
		this.btnCheckHeartsUse.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickExtractHeartsUse));
		this.btnHearts1 = (base.GetControl("Button_Hearts_Purchase1") as Button);
		this.btnHearts1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickHeartsPurchase));
		this.btnHearts2 = (base.GetControl("Button_Hearts_Purchase2") as Button);
		this.btnHearts2.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickHeartsPurchase));
		this.lbNowHearts = (base.GetControl("Label_Hearts_Num") as Label);
		int num = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
		if (num >= 0)
		{
			this.lbNowHearts.SetText(num.ToString());
		}
		this.USE_HEARTS_NUM = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_EXTRACT_HEARTSCOUNT);
		this.chHeartsUse = (base.GetControl("CheckBox_Extract_UseHearts") as CheckBox);
		this.chHeartsUse.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickCheckBoxUseHearts));
		if (num < this.USE_HEARTS_NUM)
		{
			this.chHeartsUse.SetEnabled(false);
			this.m_bUseHearts = false;
		}
		this.lbExtractHelpText = (base.GetControl("Extract_Help") as Label);
		this.lbExtractSolText1 = (base.GetControl("Extract_Text") as Label);
		this.dwExtractSolImpossible = (base.GetControl("DrawTexture_ImpossibleExtract") as DrawTexture);
		this.dwExtractSolImpossible.Visible = false;
		this.btnExtractStart = (base.GetControl("Sol_Extract_Button") as Button);
		this.btnExtractStart.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickExtractStart));
		this.btnExtractStart.EffectAni = false;
		this.lbExtractSolText2 = (base.GetControl("Extract_Text1") as Label);
		this.lbExtractSolText3 = (base.GetControl("Extract_Text2") as Label);
		this.dwExtractTextBG = (base.GetControl("DrawTexture_Extract_Text1Acc") as DrawTexture);
		this.dwExtractBG = (base.GetControl("DrawTexture_ExtractBGimg") as DrawTexture);
		this.dwExtractBG.SetTextureFromBundle("UI/Soldier/legendextract_backimg");
		this.lbExtarct_Title = (base.GetControl("Label_SolExtract_Title") as Label);
		this.dwExtractTitleBG = (base.GetControl("SolExtract_Title_BG") as DrawTexture);
		this.lbExtractPercentage = (base.GetControl("Extract_Percentage") as Label);
		this.m_HelpButton = (base.GetControl("Help_Button") as Button);
		this.m_HelpButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		this.m_btnAddGold = (base.GetControl("Button_AddGold") as Button);
		this.m_btnAddGold.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBuyGold));
		this.m_btnAddGoldTrans = (base.GetControl("Button_AddGoldTrans") as Button);
		this.m_btnAddGoldTrans.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBuyGold));
		this.SetTranscendenceComponent();
		base.SetScreenCenter();
		this.MakeSubSolList();
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, false);
		base.SetShowLayer(4, false);
		this.SetComposeBaseSol(false);
		this.CalcData();
		this.m_RecommandSol.Clear();
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("solcomposedlg_open");
	}

	public void CalcumData()
	{
		string empty = string.Empty;
		string text = string.Empty;
		string empty2 = string.Empty;
		this.mComposeCost = 0L;
		this.mMaxLvEvelution = 0L;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			text = string.Format("{0:###,###,###,##0}", kMyCharInfo.m_Money);
		}
		this.lbMoney.SetText(text);
		if (this.mBaseSol == null)
		{
			this.lbBaseName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2031"));
			this.lblComposeCost.SetText(Protocol_Item.Money_Format(this.mComposeCost));
			this.lblBaseSeasonText.SetText(string.Empty);
			this.btnBaseSelect.SetButtonTextureKey("Win_B_Addlist");
			this.btnSubSelectMain.Visible = false;
			this.lblBaseGuideText.Visible = true;
			this.lblMaterialGuideText.Visible = false;
			this.dtExpBG.Visible = false;
			this.lblExp.Visible = false;
			this.lblGradeText.Visible = false;
			this.GradeExpBG.Visible = false;
			this.GradeExpGage.Visible = false;
			this.GradeExpText.Visible = false;
			return;
		}
		this.btnBaseSelect.SetButtonTextureKey("Win_B_Change");
		this.lblBaseGuideText.Visible = false;
		int num = 0;
		short num2 = 0;
		byte b = 0;
		int num3 = 0;
		long num4 = 0L;
		long num5 = 0L;
		long num6 = 0L;
		long num7 = 0L;
		int num8 = 0;
		int num9 = 0;
		if (this.mBaseSol != null)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("290"),
				"targetname",
				this.mBaseSol.GetName(),
				"count1",
				this.mBaseSol.GetLevel().ToString(),
				"count2",
				this.mBaseSol.GetSolMaxLevel().ToString()
			});
			num = this.mBaseSol.GetCharKind();
			num2 = this.mBaseSol.GetLevel();
			b = this.mBaseSol.GetGrade();
			num9 = this.mBaseSol.GetSeason() + 1;
		}
		if (this.mSubSolList.Count == 0)
		{
			this.btnSubSelectMain.Visible = true;
			this.btnSubSelect.Visible = true;
			this.btnSubSelect.SetButtonTextureKey("Win_B_Addlist");
			this.dtExpBG.Visible = false;
			this.lblExp.Visible = false;
			this.lblGradeText.Visible = false;
			this.lblMaterialGuideText.Visible = true;
		}
		else
		{
			this.btnSubSelect.SetButtonTextureKey("Win_B_Change");
			this.btnSubSelectMain.Visible = false;
			this.dtExpBG.Visible = true;
			this.lblExp.Visible = true;
			this.lblGradeText.Visible = true;
			this.lblMaterialGuideText.Visible = false;
		}
		this.mSubSolList.Sort(new Comparison<long>(this.CompareExp));
		bool visible = false;
		foreach (long current in this.mSubSolList)
		{
			NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(current);
			if (soldierInfo != null)
			{
				this.mComposeCost += SolComposeMainDlg.GetComposeCost(soldierInfo);
				if (this.mBaseSol != null)
				{
					this.GetComposeExp(ref num2, soldierInfo, ref num4, ref num5, this.mBaseSol, ref num6);
					this.GetSubSolSkillMoneySum(soldierInfo, ref num7);
				}
				if (soldierInfo.GetCharKind() == num)
				{
					visible = true;
				}
			}
		}
		for (int i = 0; i < 1; i++)
		{
			num8 = this.mBaseSol.SelectBattleSkillByWeapon(i + 1);
			if (num8 > 0)
			{
				break;
			}
		}
		long nextExp = NrTSingleton<NkLevelManager>.Instance.GetNextExp(this.mBaseSol.GetClassInfo().EXP_TYPE, num2);
		if (nextExp <= this.mBaseSol.GetExp() + num4)
		{
			this.GetNextlevel(ref num2, this.mBaseSol.GetExp() + num4, this.mBaseSol);
		}
		long nextEvolutionExp = this.mBaseSol.GetNextEvolutionExp();
		if (nextEvolutionExp <= this.mBaseSol.GetEvolutionExp() + num5)
		{
			this.GetNextGrade(ref b, this.mBaseSol.GetEvolutionExp() + num5);
		}
		if (num8 > 0)
		{
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_KEEP_SKILL_LV_RATE);
			num7 = (long)((int)(num7 * (long)value / 100L));
			num3 = this.mBaseSol.GetBattleSkillLevel(num8);
			this.GetNextSkillLevel(ref num3, num2, num8, num7);
		}
		string text2 = string.Empty;
		if (this.mBaseSol.IsMaxLevel() || this.mBaseSol.GetSolMaxLevel() <= num2)
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
		}
		else if (this.mBaseSol != null && this.mBaseSol.GetLevel() == num2)
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2035");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text2,
				"count",
				num4
			});
		}
		else if (this.mBaseSol != null && this.mBaseSol.GetLevel() != num2)
		{
			int num10 = Math.Min((int)this.mBaseSol.GetSolMaxLevel(), (int)(num2 - this.mBaseSol.GetLevel()));
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1732");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text2,
				"count",
				num10.ToString()
			});
		}
		this.lblComposeText.Visible = visible;
		string costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(this.mBaseSol);
		this.dtBase.SetTexture(eCharImageType.LARGE, num, (int)b, costumePortraitPath);
		this.SolRank.Visible = true;
		bool flag = false;
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsReincarnation() && this.mBaseSol.IsLeader())
		{
			this.SolRank.Visible = false;
			flag = true;
		}
		if (!flag)
		{
			UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(this.mBaseSol.GetCharKind(), (int)this.mBaseSol.GetGrade());
			if (0 < NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(this.mBaseSol.GetCharKind(), (int)this.mBaseSol.GetGrade()))
			{
				this.SolRank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
			}
			this.SolRank.SetTexture(solLargeGradeImg);
		}
		if (num9 != 0)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3274"),
				"season",
				num9
			});
		}
		this.lblBaseSeasonText.SetText(empty2);
		this.lbBaseName.SetText(empty);
		this.lbMoney.SetText(text);
		this.lblExp.Visible = (0L < num4);
		this.lblExp.SetText(text2);
		this.lblComposeCost.SetText(string.Format("{0:###,###,###,##0}", this.mComposeCost));
		this.m_bGradeUp = false;
		if (this.mBaseSol != null)
		{
			float num12;
			if (this.mBaseSol.GetLevel() != num2)
			{
				long nextExp2 = NrTSingleton<NkLevelManager>.Instance.GetNextExp(this.mBaseSol.GetClassInfo().EXP_TYPE, num2);
				long nextExp3 = NrTSingleton<NkLevelManager>.Instance.GetNextExp(this.mBaseSol.GetClassInfo().EXP_TYPE, num2 - 1);
				long remainExp = NrTSingleton<NkLevelManager>.Instance.GetRemainExp(this.mBaseSol.GetClassInfo().EXP_TYPE, num2, this.mBaseSol.GetExp() + num4);
				long num11 = nextExp2 - nextExp3;
				num12 = ((float)num11 - (float)remainExp) / (float)num11;
			}
			else
			{
				long num13 = this.mBaseSol.GetNextExp() - this.mBaseSol.GetCurBaseExp();
				num12 = ((float)num13 - (float)(this.mBaseSol.GetRemainExp() - num4)) / (float)num13;
			}
			if (this.mBaseSol.IsMaxLevel() || this.mBaseSol.GetSolMaxLevel() <= num2)
			{
				num12 = 1f;
			}
			if (num12 > 1f)
			{
				num12 = 1f;
			}
			if (0f > num12)
			{
				num12 = 0f;
			}
			this.dtGage.SetSize(this.GAGE_SRC_WIDTH * num12, this.dtGage.GetSize().y);
			this.GradeExpBG.Visible = true;
			this.GradeExpGage.Visible = true;
			this.GradeExpText.Visible = true;
			this.lblGradeText.Visible = true;
			long num14 = 0L;
			long num15 = 0L;
			float num16;
			if (this.mBaseSol.IsMaxGrade())
			{
				num16 = 1f;
			}
			else if (this.mBaseSol.GetGrade() != b)
			{
				if (b + 1 >= 15)
				{
					num16 = 1f;
				}
				else
				{
					long solEvolutionNeedEXP = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolEvolutionNeedEXP(num, (int)(b + 1));
					long solEvolutionNeedEXP2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolEvolutionNeedEXP(num, (int)b);
					long num17 = solEvolutionNeedEXP - (this.mBaseSol.GetEvolutionExp() + num5);
					long num18 = solEvolutionNeedEXP - solEvolutionNeedEXP2;
					num16 = ((float)num18 - (float)num17) / (float)num18;
					if (num16 <= 0f)
					{
						num16 = 1f;
					}
				}
			}
			else
			{
				num14 = this.mBaseSol.GetEvolutionExp() + num5 - this.mBaseSol.GetCurBaseEvolutionExp();
				num15 = this.mBaseSol.GetNextEvolutionExp() - this.mBaseSol.GetCurBaseEvolutionExp();
				num16 = ((float)num15 - (float)(this.mBaseSol.GetRemainEvolutionExp() - num5)) / (float)num15;
			}
			string text3 = string.Empty;
			if (num16 > 1f)
			{
				num16 = 1f;
			}
			bool flag2 = true;
			if (NrTSingleton<ContentsLimitManager>.Instance.IsReincarnation())
			{
				flag2 = false;
			}
			this.GradeExpGage.SetSize(414f * num16, this.GradeExpGage.height);
			if (!this.mBaseSol.IsMaxGrade())
			{
				if (this.mBaseSol.GetGrade() != b)
				{
					text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("44");
					this.m_bGradeUp = true;
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text3, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
						"exp",
						num14.ToString(),
						"maxexp",
						num15.ToString()
					});
				}
			}
			else
			{
				text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("484");
			}
			if (flag2 && this.mBaseSol.IsLeader())
			{
				text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("484");
			}
			this.GradeExpText.SetText(text3);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text3, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3273"),
				"evolexp",
				num5
			});
			this.lblGradeText.SetText(text3);
		}
		if ((num4 != 0L || num5 != 0L) && TsPlatform.IsEditor)
		{
			TsLog.LogError("예상 경험치: {0}  예상 진화 경험치:{1}", new object[]
			{
				num4,
				num5
			});
		}
	}

	private void GetNextlevel(ref short BaseLevel, long Exp, NkSoldierInfo BaseSol)
	{
		long nextExp = NrTSingleton<NkLevelManager>.Instance.GetNextExp(BaseSol.GetClassInfo().EXP_TYPE, BaseLevel);
		if (nextExp > Exp)
		{
			return;
		}
		if (200 < BaseLevel)
		{
			return;
		}
		BaseLevel += 1;
		this.GetNextlevel(ref BaseLevel, Exp, BaseSol);
	}

	private void GetNextGrade(ref byte BaseGrade, long Exp)
	{
		long solEvolutionNeedEXP = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolEvolutionNeedEXP(this.mBaseSol.GetCharKind(), (int)(BaseGrade + 1));
		if (solEvolutionNeedEXP == 0L || solEvolutionNeedEXP > Exp)
		{
			return;
		}
		BaseGrade += 1;
		this.GetNextGrade(ref BaseGrade, Exp);
	}

	private void GetNextSkillLevel(ref int BaseSkillLevel, short TagertLevel, int skillunique, long Money)
	{
		if ((int)TagertLevel <= BaseSkillLevel)
		{
			return;
		}
		BATTLESKILL_TRAINING battleSkillTraining = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(skillunique, BaseSkillLevel + 1);
		if (battleSkillTraining == null)
		{
			return;
		}
		if (Money >= (long)battleSkillTraining.m_nSkillNeedGold)
		{
			BaseSkillLevel++;
			Money -= (long)battleSkillTraining.m_nSkillNeedGold;
			this.GetNextSkillLevel(ref BaseSkillLevel, TagertLevel, skillunique, Money);
			return;
		}
	}

	public void CalcSellData()
	{
		this.mSellCost = 0L;
		string text = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			text = Protocol_Item.Money_Format(kMyCharInfo.m_Money);
		}
		this.lbMoney2.SetText(text);
		this.mSubSolList.Sort(new Comparison<long>(this.CompareExp));
		foreach (long current in this.mSubSolList)
		{
			NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(current);
			if (soldierInfo != null)
			{
				this.mSellCost += SolComposeMainDlg.GetSelCost(soldierInfo);
			}
		}
		this.lbSelMoney.SetText(Protocol_Item.Money_Format(this.mSellCost));
		if (this.mSubSolList.Count == 0)
		{
			this.btnSell.Visible = false;
			this.btnSubSelectMain2.Visible = true;
			this.lbExplain.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("54"));
			this.btnSubSelect2.Visible = true;
			this.lbSellGuide.Visible = true;
			this.btnSubSelect2.SetButtonTextureKey("Win_B_Addlist");
		}
		else
		{
			this.btnSell.Visible = true;
			this.btnSubSelectMain2.Visible = false;
			this.lbSellGuide.Visible = false;
			this.lbExplain.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3067"));
			this.btnSubSelect2.SetButtonTextureKey("Win_B_Change");
		}
	}

	public void CalcExtractData()
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2826"),
			"count",
			this.m_SolExtract.Count
		});
		this.lbSolNameSpace.SetText(empty);
		int num = 0;
		float num2 = 0f;
		if (this.m_SolExtract.Count > 0)
		{
			for (int i = 0; i < this.m_SolExtract.Count; i++)
			{
				num += 100;
				NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(this.m_SolExtract[i]);
				if (soldierInfo != null)
				{
					int solExtractRateItemInfo = NrTSingleton<NrSolExtractRateManager>.Instance.GetSolExtractRateItemInfo(soldierInfo.GetSeason(), (int)soldierInfo.GetGrade(), this.m_bUseHearts);
					num2 += (float)solExtractRateItemInfo / 100f;
				}
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2827"),
				"count",
				num
			});
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2857"),
				"count",
				string.Format("{0:F2}", num2 / (float)this.m_SolExtract.Count)
			});
			this.lbExtractPercentage.SetText(empty);
		}
		else
		{
			this.chHeartsUse.SetToggleState(0);
			this.m_bUseHearts = false;
			num = this.USE_HEARTS_NUM;
			this.ShowExtractSol(false);
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2827"),
			"count",
			num
		});
		this.lbExtract_Heart.SetText(empty2);
		int num3 = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
		if (num3 >= 0)
		{
			this.lbNowHearts.SetText(num3.ToString());
		}
	}

	public override void OnLoad()
	{
		NrSound.ImmedatePlay("UI_SFX", "MERCENARY-COMPOSE", "OPEN");
	}

	public override void OnClose()
	{
		this.HideTouch(true);
		if (NrTSingleton<FormsManager>.Instance.IsPopUPDlgNotExist(base.WindowID))
		{
			NrTSingleton<UIImageBundleManager>.Instance.DeleteTexture();
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLCOMPOSE_LIST_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLCOMPOSE_CHECK_DLG);
		NrSound.ImmedatePlay("UI_SFX", "MERCENARY-COMPOSE", "CLOSE");
		NrTSingleton<NrMainSystem>.Instance.CleanUpReserved();
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
	}

	public bool ContainBaseSoldier(long SolID)
	{
		return this.mBaseSol != null && this.mBaseSol.GetSolID() == SolID;
	}

	public bool ContainSubSoldier(long SolID)
	{
		return this.mSubSolList.Contains(SolID);
	}

	public bool ContainExtractSoldier(long SolID)
	{
		return this.m_SolExtract.Contains(SolID);
	}

	public bool CheckBaseSoldier(NkSoldierInfo pkSolInfo)
	{
		if (this.mBaseSol != null && this.mBaseSol.GetSolID() == pkSolInfo.GetSolID())
		{
			this.mBaseSol = pkSolInfo;
			return true;
		}
		return false;
	}

	protected NewListBox GetListBox()
	{
		NewListBox result = null;
		switch (this.m_ShowType)
		{
		case SOLCOMPOSE_TYPE.COMPOSE:
			result = this.lxList;
			break;
		case SOLCOMPOSE_TYPE.SELL:
			result = this.lxList2;
			break;
		case SOLCOMPOSE_TYPE.EXTRACT:
			result = this.lxSelectList;
			break;
		case SOLCOMPOSE_TYPE.TRANSCENDENCE:
			result = this.lxList4;
			break;
		}
		return result;
	}

	private void AddExtractSolList(long SolIDX)
	{
		NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(SolIDX);
		if (soldierInfo == null)
		{
			return;
		}
		NewListBox listBox = this.GetListBox();
		NewListItem newListItem = new NewListItem(listBox.ColumnNum, true, string.Empty);
		long num = soldierInfo.GetNextExp() - soldierInfo.GetCurBaseExp();
		float num2 = ((float)num - (float)soldierInfo.GetRemainExp()) / (float)num;
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		if (0f > num2)
		{
		}
		string empty = string.Empty;
		if (soldierInfo.IsMaxLevel())
		{
		}
		UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(soldierInfo.GetCharKind(), (int)soldierInfo.GetGrade());
		if (legendFrame != null)
		{
			newListItem.SetListItemData(0, legendFrame, null, null, null);
		}
		newListItem.SetListItemData(2, soldierInfo.GetListSolInfo(false), null, null, null);
		string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(soldierInfo.GetCharKind(), (int)soldierInfo.GetGrade(), soldierInfo.GetName());
		newListItem.SetListItemData(3, legendName, null, null, null);
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"count1",
			soldierInfo.GetLevel(),
			"count2",
			soldierInfo.GetSolMaxLevel()
		});
		newListItem.SetListItemData(4, textFromInterface, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3274"),
			"season",
			soldierInfo.GetSeason() + 1
		});
		newListItem.SetListItemData(5, empty, null, null, null);
		if (this.CheckEquipItem(soldierInfo))
		{
			newListItem.SetListItemData(3, false);
			newListItem.SetListItemData(4, false);
			newListItem.SetListItemData(5, false);
			string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("627");
			newListItem.SetListItemData(6, true);
			newListItem.SetListItemData(7, textFromInterface2, soldierInfo, new EZValueChangedDelegate(this.OnClickReleaseEquip), null);
			newListItem.SetListItemData(8, false);
		}
		else if (soldierInfo.GetFriendPersonID() > 0L)
		{
			newListItem.SetListItemData(3, false);
			newListItem.SetListItemData(4, false);
			newListItem.SetListItemData(5, false);
			string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("74");
			newListItem.SetListItemData(6, true);
			newListItem.SetListItemData(7, false);
			newListItem.SetListItemData(8, textFromInterface3, soldierInfo, new EZValueChangedDelegate(this.OnClickUnsetSolHelp), null);
		}
		else if (soldierInfo.IsCostumeEquip())
		{
			newListItem.SetListItemData(3, false);
			newListItem.SetListItemData(4, false);
			newListItem.SetListItemData(5, false);
			string textFromInterface4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3305");
			newListItem.SetListItemData(6, true);
			newListItem.SetListItemData(7, false);
			newListItem.SetListItemData(8, textFromInterface4, soldierInfo, new EZValueChangedDelegate(this.OnClickUnsetCostume), null);
		}
		else
		{
			newListItem.SetListItemData(6, false);
			newListItem.SetListItemData(7, false);
			newListItem.SetListItemData(8, false);
		}
		newListItem.SetListItemData(9, string.Empty, soldierInfo.GetSolID(), new EZValueChangedDelegate(this.OnClickExtract), null);
		newListItem.Data = soldierInfo.GetSolID();
		listBox.Add(newListItem);
	}

	private void AddSubSolList(long SolIDX)
	{
		NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(SolIDX);
		if (soldierInfo == null)
		{
			return;
		}
		NewListBox listBox = this.GetListBox();
		NewListItem newListItem = new NewListItem(listBox.ColumnNum, true, string.Empty);
		long num = soldierInfo.GetExp() - soldierInfo.GetCurBaseExp();
		long num2 = soldierInfo.GetNextExp() - soldierInfo.GetCurBaseExp();
		float num3 = ((float)num2 - (float)soldierInfo.GetRemainExp()) / (float)num2;
		if (num3 > 1f)
		{
			num3 = 1f;
		}
		if (0f > num3)
		{
			num3 = 0f;
		}
		string text = string.Format("{0}/{1}", num, num2);
		string empty = string.Empty;
		if (soldierInfo.IsMaxLevel())
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
			num3 = 1f;
		}
		UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(soldierInfo.GetCharKind(), (int)soldierInfo.GetGrade());
		if (legendFrame != null)
		{
			newListItem.SetListItemData(0, legendFrame, null, null, null);
		}
		newListItem.SetListItemData(1, soldierInfo.GetListSolInfo(false), null, null, null);
		string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(soldierInfo.GetCharKind(), (int)soldierInfo.GetGrade(), soldierInfo.GetName());
		newListItem.SetListItemData(2, legendName, null, null, null);
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"count1",
			soldierInfo.GetLevel(),
			"count2",
			soldierInfo.GetSolMaxLevel()
		});
		newListItem.SetListItemData(3, textFromInterface, null, null, null);
		newListItem.SetListItemData(4, false);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3274"),
			"season",
			soldierInfo.GetSeason() + 1
		});
		newListItem.SetListItemData(5, empty, null, null, null);
		newListItem.SetListItemData(6, false);
		newListItem.SetListItemData(7, false);
		listBox.SetColumnSize(9, (int)(250f * num3), 26);
		newListItem.SetListItemData(10, text, null, null, null);
		if (this.CheckEquipItem(soldierInfo))
		{
			string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("627");
			newListItem.SetListItemData(10, false);
			newListItem.SetListItemData(11, true);
			newListItem.SetListItemData(13, textFromInterface2, soldierInfo, new EZValueChangedDelegate(this.OnClickReleaseEquip), null);
		}
		else if (soldierInfo.GetFriendPersonID() > 0L)
		{
			string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("74");
			newListItem.SetListItemData(10, false);
			newListItem.SetListItemData(11, true);
			newListItem.SetListItemData(13, textFromInterface3, soldierInfo, new EZValueChangedDelegate(this.OnClickUnsetSolHelp), null);
		}
		else if (soldierInfo.IsCostumeEquip())
		{
			string textFromInterface4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3305");
			newListItem.SetListItemData(10, false);
			newListItem.SetListItemData(11, true);
			newListItem.SetListItemData(13, textFromInterface4, soldierInfo, new EZValueChangedDelegate(this.OnClickUnsetCostume), null);
		}
		else
		{
			newListItem.SetListItemData(11, false);
			newListItem.SetListItemData(13, false);
		}
		newListItem.SetListItemData(12, string.Empty, soldierInfo.GetSolID(), new EZValueChangedDelegate(this.OnClickSubCancel), null);
		newListItem.Data = soldierInfo.GetSolID();
		listBox.Add(newListItem);
	}

	private int CompareExp(long a, long b)
	{
		NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(a);
		NkSoldierInfo soldierInfo2 = SolComposeMainDlg.GetSoldierInfo(b);
		return soldierInfo2.GetExp().CompareTo(soldierInfo.GetExp());
	}

	private int CompareGrade(long a, long b)
	{
		NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(a);
		NkSoldierInfo soldierInfo2 = SolComposeMainDlg.GetSoldierInfo(b);
		return soldierInfo.GetGrade().CompareTo(soldierInfo2.GetGrade());
	}

	public int GetListIndexBySolID(long SolIDX)
	{
		NewListBox listBox = this.GetListBox();
		if (this.mSubSolList.Contains(SolIDX))
		{
			for (int i = 0; i < listBox.Count; i++)
			{
				IUIListObject item = listBox.GetItem(i);
				if (SolIDX == (long)item.Data)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public void RemoveSubSolList(long SolIDX)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_CHECK_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLCOMPOSE_CHECK_DLG);
		}
		int listIndexBySolID = this.GetListIndexBySolID(SolIDX);
		if (0 <= listIndexBySolID)
		{
			NewListBox listBox = this.GetListBox();
			this.mSubSolList.Remove(SolIDX);
			listBox.RemoveItem(listIndexBySolID, false);
			listBox.RepositionItems();
			this.SetSoldierNum();
		}
	}

	public void MakeSubSolList()
	{
		if (this.m_ShowType == SOLCOMPOSE_TYPE.EXTRACT)
		{
			if (this.mBaseSol == null)
			{
				this.ShowExtractSol(false);
			}
			else
			{
				this.ShowExtractSol(true);
			}
		}
		else
		{
			NewListBox listBox = this.GetListBox();
			if (listBox != null)
			{
				listBox.Clear();
				foreach (long current in this.mSubSolList)
				{
					this.AddSubSolList(current);
				}
				listBox.RepositionItems();
				this.SetSoldierNum();
			}
		}
	}

	public bool CheckEquipItem(NkSoldierInfo kSolInfo)
	{
		if (kSolInfo != null)
		{
			for (int i = 0; i < 6; i++)
			{
				ITEM equipItem = kSolInfo.GetEquipItem(i);
				if (equipItem != null && 0 < equipItem.m_nItemUnique)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool CheckUpAble(NkSoldierInfo kBaseSol, NkSoldierInfo kSubSol)
	{
		if (kSubSol != null)
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo != null)
			{
				NrSoldierList soldierList = charPersonInfo.GetSoldierList();
				if (soldierList != null && soldierList.GetSoldierInfoBySolID(kSubSol.GetSolID()) != null)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("515");
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					return false;
				}
			}
		}
		return true;
	}

	private void ReleaseEquipItem(NkSoldierInfo kSolInfo)
	{
		Protocol_Item.Send_EquipSol_InvenEquip_All(kSolInfo);
	}

	protected virtual void OnClickStart(IUIObject obj)
	{
		if (this.mBaseSol == null)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("504");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (this.mSubSolList.Count == 0)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("503");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		foreach (long current in this.mSubSolList)
		{
			NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(current);
			if (this.CheckEquipItem(soldierInfo))
			{
				string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("509");
				Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
		}
		foreach (long current2 in this.mSubSolList)
		{
			NkSoldierInfo soldierInfo2 = SolComposeMainDlg.GetSoldierInfo(current2);
			if (soldierInfo2 != null)
			{
				if (soldierInfo2.GetFriendPersonID() > 0L)
				{
					string textFromNotify4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("372");
					Main_UI_SystemMessage.ADDMessage(textFromNotify4, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					return;
				}
				NrCharKindInfo charKindInfo = soldierInfo2.GetCharKindInfo();
				if (charKindInfo != null)
				{
					BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = charKindInfo.GetCHARKIND_SOLGRADEINFO((int)soldierInfo2.GetGrade());
					if (cHARKIND_SOLGRADEINFO != null)
					{
						if (cHARKIND_SOLGRADEINFO.ComposeLimit != 0)
						{
							if (this.mBaseSol.IsLeader() && cHARKIND_SOLGRADEINFO.ComposeLimit == 1)
							{
								string textFromNotify5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("617");
								Main_UI_SystemMessage.ADDMessage(textFromNotify5, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
								return;
							}
							if (!this.mBaseSol.IsLeader() && cHARKIND_SOLGRADEINFO.ComposeLimit == 2)
							{
								string textFromNotify6 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("618");
								Main_UI_SystemMessage.ADDMessage(textFromNotify6, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
								return;
							}
						}
					}
				}
			}
		}
		foreach (long current3 in this.mSubSolList)
		{
			NkSoldierInfo soldierInfo3 = SolComposeMainDlg.GetSoldierInfo(current3);
			if (soldierInfo3 != null)
			{
				if (soldierInfo3.IsCostumeEquip())
				{
					string textFromNotify7 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("856");
					Main_UI_SystemMessage.ADDMessage(textFromNotify7, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					return;
				}
			}
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null && this.mComposeCost > kMyCharInfo.m_Money)
		{
			string textFromNotify8 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(textFromNotify8, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			LackGold_dlg lackGold_dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GOLDLACK_DLG) as LackGold_dlg;
			if (lackGold_dlg != null)
			{
				lackGold_dlg.SetData(this.mComposeCost - kMyCharInfo.m_Money);
			}
			return;
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLCOMPOSE_LIST_DLG);
		SolComposeCheckDlg solComposeCheckDlg = (SolComposeCheckDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_CHECK_DLG);
		if (solComposeCheckDlg != null)
		{
			solComposeCheckDlg.SetData(this.mBaseSol, this.mSubSolList, this.m_ShowType);
		}
		this.m_RecommandSol.Clear();
	}

	protected virtual void OnClickBase(IUIObject obj)
	{
		SolComposeListDlg.LoadSelectList(true, SOLCOMPOSE_TYPE.COMPOSE);
	}

	protected virtual void OnClickSub(IUIObject obj)
	{
		SolComposeListDlg.LoadSelectList(false, this.m_ShowType);
	}

	protected virtual void OnClickExtract(IUIObject obj)
	{
		UIButton uIButton = obj as UIButton;
		if (uIButton == null)
		{
			return;
		}
		long num = (long)uIButton.Data;
		if (0L < num && this.m_SolExtract.Contains(num))
		{
			this.RemoveExtractSolList(num);
			this.CalcData();
		}
	}

	public void RemoveExtractSolList(long SolIDX)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_CHECK_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLCOMPOSE_CHECK_DLG);
		}
		int listExtractIndexBySolID = this.GetListExtractIndexBySolID(SolIDX);
		if (0 <= listExtractIndexBySolID)
		{
			NewListBox listBox = this.GetListBox();
			this.m_SolExtract.Remove(SolIDX);
			listBox.RemoveItem(listExtractIndexBySolID, false);
			listBox.RepositionItems();
			this.SetSoldierNum();
		}
	}

	public int GetListExtractIndexBySolID(long SolIDX)
	{
		NewListBox listBox = this.GetListBox();
		if (this.m_SolExtract.Contains(SolIDX))
		{
			for (int i = 0; i < listBox.Count; i++)
			{
				IUIListObject item = listBox.GetItem(i);
				if (SolIDX == (long)item.Data)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private void OnClickSubCancel(IUIObject obj)
	{
		UIButton uIButton = obj as UIButton;
		if (uIButton == null)
		{
			return;
		}
		long num = (long)uIButton.Data;
		if (0L < num && this.mSubSolList.Contains(num))
		{
			this.RemoveSubSolList(num);
			this.CalcData();
		}
	}

	private void OnClickReleaseEquip(IUIObject obj)
	{
		UIButton uIButton = obj as UIButton;
		if (uIButton == null)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = uIButton.Data as NkSoldierInfo;
		if (nkSoldierInfo != null)
		{
			this.ReleaseEquipItem(nkSoldierInfo);
		}
	}

	private void OnClickSell(IUIObject obj)
	{
		if (this.mSubSolList == null || this.mSubSolList.Count == 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("90"));
			return;
		}
		foreach (long current in this.mSubSolList)
		{
			NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(current);
			if (this.CheckEquipItem(soldierInfo))
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("509");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
		}
		foreach (long current2 in this.mSubSolList)
		{
			NkSoldierInfo soldierInfo2 = SolComposeMainDlg.GetSoldierInfo(current2);
			if (soldierInfo2 != null)
			{
				if (soldierInfo2.GetFriendPersonID() > 0L)
				{
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("372");
					Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					return;
				}
			}
		}
		foreach (long current3 in this.mSubSolList)
		{
			NkSoldierInfo soldierInfo3 = SolComposeMainDlg.GetSoldierInfo(current3);
			if (soldierInfo3 != null)
			{
				if (soldierInfo3.IsCostumeEquip())
				{
					string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("856");
					Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					return;
				}
			}
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLCOMPOSE_LIST_DLG);
		SolComposeCheckDlg solComposeCheckDlg = (SolComposeCheckDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_CHECK_DLG);
		if (solComposeCheckDlg != null)
		{
			solComposeCheckDlg.SetData(this.mBaseSol, this.mSubSolList, this.m_ShowType);
		}
	}

	private void OnClickUnsetSolHelp(IUIObject obj)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)obj.Data;
		if (nkSoldierInfo == null)
		{
			return;
		}
		GS_FRIEND_HELPSOL_UNSET_REQ gS_FRIEND_HELPSOL_UNSET_REQ = new GS_FRIEND_HELPSOL_UNSET_REQ();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64FriendPersonID = nkSoldierInfo.GetFriendPersonID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64SolID = nkSoldierInfo.GetSolID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64AddExp = nkSoldierInfo.AddHelpExp;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_HELPSOL_UNSET_REQ, gS_FRIEND_HELPSOL_UNSET_REQ);
	}

	private void OnClickCheckBoxUseHearts(IUIObject obj)
	{
		int num = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
		if (this.m_SolExtract.Count <= 0)
		{
			return;
		}
		if (num < this.USE_HEARTS_NUM * this.m_SolExtract.Count)
		{
			return;
		}
		this.chHeartsUse.SetEnabled(true);
		if (this.chHeartsUse.StateNum == 1)
		{
			this.m_bUseHearts = true;
		}
		else
		{
			this.m_bUseHearts = false;
		}
		this.CalcExtractData();
	}

	private void OnClickExtractHeartsUse(IUIObject obj)
	{
		int num = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
		if (this.m_SolExtract.Count > 0)
		{
			if (num < this.USE_HEARTS_NUM * this.m_SolExtract.Count)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("273"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			if (this.chHeartsUse.StateNum == 1)
			{
				this.chHeartsUse.SetToggleState(0);
			}
			else
			{
				this.chHeartsUse.SetToggleState(1);
			}
			this.OnClickCheckBoxUseHearts(null);
		}
	}

	protected virtual void OnClickAddExtractSol(IUIObject obj)
	{
		this.HideTouch(false);
		SolComposeListDlg.LoadSelectExtractList(this.m_ShowType);
		this.chHeartsUse.SetToggleState(0);
		this.chHeartsUse.SetEnabled(false);
		this.m_bUseHearts = false;
	}

	protected virtual void OnClickHeartsPurchase(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_HEARTS, true);
		this.Close();
	}

	protected virtual void OnClickExtractStart(IUIObject obj)
	{
		if (this.m_SolExtract.Count <= 0)
		{
			return;
		}
		bool flag = true;
		for (int i = 0; i < this.m_SolExtract.Count; i++)
		{
			NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(this.m_SolExtract[i]);
			if (soldierInfo == null)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				flag = false;
				break;
			}
			if (this.CheckEquipItem(soldierInfo))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("789"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				flag = false;
				break;
			}
			if (soldierInfo.GetFriendPersonID() > 0L)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("790"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				flag = false;
				break;
			}
			if (soldierInfo.IsCostumeEquip())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("856"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				flag = false;
				break;
			}
		}
		if (flag)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI == null)
			{
				return;
			}
			string empty = string.Empty;
			NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(this.m_SolExtract[0]);
			if (soldierInfo == null)
			{
				return;
			}
			if (this.m_SolExtract.Count - 1 > 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("440"),
					"charname",
					soldierInfo.GetName(),
					"count",
					this.m_SolExtract.Count - 1
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("295"),
					"charname",
					soldierInfo.GetName()
				});
			}
			msgBoxUI.SetMsg(new YesDelegate(this.MessageBoxClassChangeOK), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2866"), empty, eMsgType.MB_OK_CANCEL, 2);
			msgBoxUI.Show();
			this.HideTouch(false);
		}
	}

	private void MessageBoxClassChangeOK(object a_oObject)
	{
		if (this.m_SolExtract.Count <= 0)
		{
			return;
		}
		int i32HeartsCount = 0;
		if (this.m_bUseHearts)
		{
			i32HeartsCount = this.USE_HEARTS_NUM * this.m_SolExtract.Count;
		}
		GS_SOLDIERS_EXTRACT_REQ gS_SOLDIERS_EXTRACT_REQ = new GS_SOLDIERS_EXTRACT_REQ();
		for (int i = 0; i < this.m_SolExtract.Count; i++)
		{
			gS_SOLDIERS_EXTRACT_REQ.i64ExtractSolID[i] = this.m_SolExtract[i];
		}
		gS_SOLDIERS_EXTRACT_REQ.i32HeartsCount = i32HeartsCount;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIERS_EXTRACT_REQ, gS_SOLDIERS_EXTRACT_REQ);
	}

	private void OnClickRemoveWeapon(IUIObject obj)
	{
		if (this.mBaseSol != null)
		{
			this.ReleaseEquipItem(this.mBaseSol);
		}
	}

	private void OnClickClearSupport(IUIObject obj)
	{
		if (this.mBaseSol != null)
		{
			GS_FRIEND_HELPSOL_UNSET_REQ gS_FRIEND_HELPSOL_UNSET_REQ = new GS_FRIEND_HELPSOL_UNSET_REQ();
			gS_FRIEND_HELPSOL_UNSET_REQ.i64FriendPersonID = this.mBaseSol.GetFriendPersonID();
			gS_FRIEND_HELPSOL_UNSET_REQ.i64SolID = this.mBaseSol.GetSolID();
			gS_FRIEND_HELPSOL_UNSET_REQ.i64AddExp = this.mBaseSol.AddHelpExp;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_HELPSOL_UNSET_REQ, gS_FRIEND_HELPSOL_UNSET_REQ);
		}
	}

	protected void ShowExtractSol(bool bShowExtractSol)
	{
		this.btnExtractsolBase1.Visible = true;
		this.btnExtractsolBase2.Visible = true;
		this.btnHearts1.Visible = true;
		this.btnHearts2.Visible = true;
		this.lbNowHearts.Visible = true;
		this.chHeartsUse.Visible = true;
		int num = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
		if (num < this.USE_HEARTS_NUM)
		{
			this.chHeartsUse.SetEnabled(false);
			this.m_bUseHearts = false;
		}
		this.lbExtractHelpText.Visible = !bShowExtractSol;
		this.lbExtractSolText1.Visible = !bShowExtractSol;
		this.lbExtract_Heart.Visible = bShowExtractSol;
		this.btnExtractStart.Visible = bShowExtractSol;
		this.lbExtractSolText2.Visible = bShowExtractSol;
		this.lbExtractSolText3.Visible = bShowExtractSol;
		this.lbExtractPercentage.Visible = bShowExtractSol;
		this.dwExtractTextBG.Visible = bShowExtractSol;
		this.dwExtractBG.Visible = bShowExtractSol;
		this.lbExtarct_Title.Visible = !bShowExtractSol;
		this.dwExtractTitleBG.Visible = !bShowExtractSol;
		if (this.mBaseSol != null)
		{
			string empty = string.Empty;
			int num2 = this.mBaseSol.GetSeason() + 1;
			if (num2 != 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3274"),
					"season",
					num2
				});
			}
		}
		else
		{
			this.dwExtractSolImpossible.Visible = false;
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2826"),
				"count",
				this.m_SolExtract.Count
			});
			this.lbSolNameSpace.SetText(empty2);
		}
	}

	public virtual void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.SetComposeType((SOLCOMPOSE_TYPE)uIPanelTab.panel.index);
	}

	public virtual void SetComposeType(SOLCOMPOSE_TYPE eType)
	{
		this.m_ShowType = eType;
		this.m_Toolbar.SetSelectTabIndex((int)eType);
		this.ComposeInit();
	}

	public void ComposeTranscendence()
	{
		if (this.m_ShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
		{
			this.ClearList();
			base.SetShowLayer(1, this.m_ShowType == SOLCOMPOSE_TYPE.COMPOSE);
			base.SetShowLayer(2, this.m_ShowType == SOLCOMPOSE_TYPE.SELL);
			base.SetShowLayer(3, this.m_ShowType == SOLCOMPOSE_TYPE.EXTRACT);
			base.SetShowLayer(4, this.m_ShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE);
			this.ShowTranscendence(true);
			this.CalcData();
			this.SetSoldierNum();
			this.m_HelpButton.Visible = (this.m_ShowType != SOLCOMPOSE_TYPE.SELL);
		}
	}

	public void ComposeInit()
	{
		this.ClearList();
		base.SetShowLayer(1, this.m_ShowType == SOLCOMPOSE_TYPE.COMPOSE);
		base.SetShowLayer(2, this.m_ShowType == SOLCOMPOSE_TYPE.SELL);
		base.SetShowLayer(3, this.m_ShowType == SOLCOMPOSE_TYPE.EXTRACT);
		base.SetShowLayer(4, this.m_ShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE);
		this.SolComposeInit();
		if (this.m_ShowType == SOLCOMPOSE_TYPE.EXTRACT)
		{
			this.ShowExtractSol(false);
		}
		else if (this.m_ShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
		{
			this.ShowTranscendence(false);
		}
		this.CalcData();
		this.SetSoldierNum();
		this.m_HelpButton.Visible = (this.m_ShowType != SOLCOMPOSE_TYPE.SELL);
	}

	public void InitExtract()
	{
		this.mBaseSol = null;
		this.m_SolExtract.Clear();
		NewListBox listBox = this.GetListBox();
		if (listBox != null)
		{
			listBox.Clear();
		}
		this.ShowExtractSol(false);
		this.CalcData();
	}

	private void OnClickRecommand(IUIObject obj)
	{
		Dictionary<int, SolComposeMainDlg.SOLKINDTONUM> dictionary = new Dictionary<int, SolComposeMainDlg.SOLKINDTONUM>();
		List<long> list = new List<long>();
		NkSoldierInfo nkSoldierInfo = null;
		if (!this.FindRecommandSameSol(ref nkSoldierInfo, ref list, ref dictionary))
		{
			if (this.FindRecommandNonSameSol(ref nkSoldierInfo, ref list, false))
			{
				this.m_RecommandSol.Add(nkSoldierInfo.GetCharKind());
			}
			else if (this.FindRecommandNonSameSol(ref nkSoldierInfo, ref list, true))
			{
				this.m_RecommandSol.Add(nkSoldierInfo.GetCharKind());
			}
			else if (this.FindRecommandLeaderSol(ref nkSoldierInfo, ref list))
			{
				this.m_RecommandSol.Add(nkSoldierInfo.GetCharKind());
			}
		}
		if (nkSoldierInfo == null && this.m_RecommandSol.Count > 0)
		{
			this.RefreshFindSol(ref nkSoldierInfo, ref list, ref dictionary);
		}
		if (list.Count > 0 && nkSoldierInfo != null)
		{
			this.SelectBase(nkSoldierInfo.GetSolID());
			this.SelectSub(list);
		}
	}

	private void SetSaveSubSolIDList(List<long> SaveSubSolIDList)
	{
		this.m_SaveSubSolIDList = SaveSubSolIDList;
	}

	private void LatestSolRecommand()
	{
		NkSoldierInfo nkSoldierInfo = null;
		List<long> list = null;
		long num = 0L;
		string @string = PlayerPrefs.GetString(NrPrefsKey.LATEST_SOLID, string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return;
		}
		if (!long.TryParse(@string, out num))
		{
			return;
		}
		if (num == 0L)
		{
			return;
		}
		nkSoldierInfo = SolComposeMainDlg.GetSoldierInfo(num);
		if (nkSoldierInfo == null)
		{
			return;
		}
		list = new List<long>();
		if (this.LatestSolRecommand(ref nkSoldierInfo, ref list) && list.Count > 0 && nkSoldierInfo != null)
		{
			this.SelectBase(nkSoldierInfo.GetSolID());
			this.SelectSub(list);
		}
	}

	private void GetNonZeroExpSol(NkSoldierInfo _BaseSol, ref List<long> _SubSolIDList)
	{
		if (_BaseSol == null || _SubSolIDList.Count == 0)
		{
			return;
		}
		long num = 0L;
		long num2 = 0L;
		List<long> list = new List<long>();
		short level = _BaseSol.GetLevel();
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		for (int i = 0; i < _SubSolIDList.Count; i++)
		{
			NkSoldierInfo solInfo = readySolList.GetSolInfo(_SubSolIDList[i]);
			long num3 = num;
			long num4 = num2;
			this.GetComposeExp(ref level, solInfo, ref num, ref num2, _BaseSol, ref this.mMaxLvEvelution);
			if (num3 == num && num4 == num2)
			{
				list.Add(solInfo.GetSolID());
			}
			else if (solInfo.IsAwakening() || solInfo.IsAtbCommonFlag(1L))
			{
				list.Add(solInfo.GetSolID());
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			_SubSolIDList.Remove(list[j]);
		}
		list.Clear();
	}

	private bool FindSameSol(int SolKind, ref NkSoldierInfo _BaseSol, ref List<long> _SubSolIDList)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			if (this.CheckRecommandBaseSol(soldierInfo, 1))
			{
				if (SolKind == soldierInfo.GetCharKind())
				{
					_BaseSol = soldierInfo;
					break;
				}
			}
		}
		List<NkSoldierInfo> solWarehouseList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouseList();
		foreach (NkSoldierInfo current in solWarehouseList)
		{
			if (this.CheckRecommandBaseSol(current, 5))
			{
				if (_BaseSol == null)
				{
					if (SolKind == current.GetCharKind())
					{
						_BaseSol = current;
						break;
					}
				}
				else if (SolKind == current.GetCharKind())
				{
					if (current.GetGrade() > _BaseSol.GetGrade())
					{
						_BaseSol = current;
					}
					else if (current.GetGrade() == _BaseSol.GetGrade() && current.GetLevel() > _BaseSol.GetLevel())
					{
						_BaseSol = current;
					}
				}
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		foreach (NkSoldierInfo current2 in readySolList.GetList().Values)
		{
			if (this.CheckRecommandBaseSol(current2, 0))
			{
				if (_BaseSol == null)
				{
					if (SolKind == current2.GetCharKind())
					{
						_BaseSol = current2;
					}
				}
				else if (SolKind == current2.GetCharKind())
				{
					if (current2.GetGrade() > _BaseSol.GetGrade())
					{
						_BaseSol = current2;
					}
					else if (current2.GetGrade() == _BaseSol.GetGrade() && current2.GetLevel() > _BaseSol.GetLevel())
					{
						_BaseSol = current2;
					}
				}
			}
		}
		List<long> list = new List<long>();
		foreach (NkSoldierInfo current3 in readySolList.GetList().Values)
		{
			if (this.CheckRecommandBaseSol(current3, 0))
			{
				if (_BaseSol != null && SolKind == current3.GetCharKind() && _BaseSol.GetSolID() != current3.GetSolID() && current3.GetExp() == 0L && !current3.IsAwakening() && !current3.IsAtbCommonFlag(1L))
				{
					list.Add(current3.GetSolID());
				}
			}
		}
		list.Sort(new Comparison<long>(this.CompareGrade));
		long num = 0L;
		long num2 = 0L;
		long maxNextEvolutionExp = _BaseSol.GetMaxNextEvolutionExp();
		foreach (long current4 in list)
		{
			NkSoldierInfo soldierInfo2 = SolComposeMainDlg.GetSoldierInfo(current4);
			NrCharKindInfo charKindInfo = soldierInfo2.GetCharKindInfo();
			if (soldierInfo2 != null && charKindInfo != null && _BaseSol != null)
			{
				long evolutionExp = charKindInfo.GetEvolutionExp((int)soldierInfo2.GetGrade());
				long num3 = soldierInfo2.GetEvolutionExp() - NrTSingleton<NrCharKindInfoManager>.Instance.GetSolEvolutionNeedEXP(soldierInfo2.GetCharKind(), (int)soldierInfo2.GetGrade());
				if (0L > num3)
				{
					num3 = 0L;
				}
				num += evolutionExp + num3;
				if (maxNextEvolutionExp < num)
				{
					num2 = num;
					_SubSolIDList.Add(current4);
					break;
				}
				_SubSolIDList.Add(current4);
			}
		}
		List<long> list2 = new List<long>();
		foreach (long current5 in _SubSolIDList)
		{
			NkSoldierInfo soldierInfo3 = SolComposeMainDlg.GetSoldierInfo(current5);
			NrCharKindInfo charKindInfo2 = soldierInfo3.GetCharKindInfo();
			if (soldierInfo3 != null && charKindInfo2 != null && _BaseSol != null)
			{
				long evolutionExp2 = charKindInfo2.GetEvolutionExp((int)soldierInfo3.GetGrade());
				long num4 = soldierInfo3.GetEvolutionExp() - NrTSingleton<NrCharKindInfoManager>.Instance.GetSolEvolutionNeedEXP(soldierInfo3.GetCharKind(), (int)soldierInfo3.GetGrade());
				if (0L > num4)
				{
					num4 = 0L;
				}
				long num5 = evolutionExp2 + num4;
				if (maxNextEvolutionExp <= num2 - num5)
				{
					list2.Add(current5);
					num2 -= num5;
				}
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			_SubSolIDList.Remove(list2[j]);
		}
		list2.Clear();
		list.Clear();
		if (_BaseSol != null && _BaseSol.IsMaxGrade())
		{
			_BaseSol = null;
		}
		if (_BaseSol == null || _SubSolIDList.Count == 0)
		{
			_BaseSol = null;
			_SubSolIDList.Clear();
			return false;
		}
		return true;
	}

	private void RefreshFindSol(ref NkSoldierInfo _BaseSol, ref List<long> _SubSolIDList, ref Dictionary<int, SolComposeMainDlg.SOLKINDTONUM> _SolKindToNum)
	{
		this.m_RecommandSol.Clear();
		foreach (SolComposeMainDlg.SOLKINDTONUM current in _SolKindToNum.Values)
		{
			if (current.Count > 1 && current.ZeroExpSol && this.FindSameSol(current.Kind, ref _BaseSol, ref _SubSolIDList))
			{
				this.m_RecommandSol.Add(current.Kind);
				break;
			}
		}
		if (_BaseSol == null && this.FindRecommandNonSameSol(ref _BaseSol, ref _SubSolIDList, false))
		{
			this.m_RecommandSol.Add(_BaseSol.GetCharKind());
		}
	}

	private bool FindRecommandNonSameSol(ref NkSoldierInfo _BaseSol, ref List<long> _SubSolIDList, bool bBattle = false)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (charPersonInfo != null)
		{
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
				if (this.CheckRecommandBaseSol(soldierInfo, 1))
				{
					if (soldierInfo.GetLevel() >= 2 && !soldierInfo.IsMaxLevel())
					{
						if (_BaseSol == null)
						{
							_BaseSol = soldierInfo;
						}
						else if (_BaseSol.GetExp() > soldierInfo.GetExp())
						{
							_BaseSol = soldierInfo;
						}
					}
				}
			}
		}
		List<NkSoldierInfo> solWarehouseList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouseList();
		foreach (NkSoldierInfo current in solWarehouseList)
		{
			if (this.CheckRecommandBaseSol(current, 5))
			{
				if (_BaseSol == null)
				{
					if (current.GetLevel() >= 2 && !current.IsMaxLevel())
					{
						_BaseSol = current;
					}
				}
				else if (current.GetLevel() >= 2 && !current.IsMaxLevel() && _BaseSol.GetLevel() > current.GetLevel())
				{
					_BaseSol = current;
				}
			}
		}
		foreach (NkSoldierInfo current2 in readySolList.GetList().Values)
		{
			if (this.CheckRecommandBaseSol(current2, 0))
			{
				if (!bBattle && current2.GetLevel() >= 2 && !current2.IsMaxLevel())
				{
					if (_BaseSol == null)
					{
						_BaseSol = current2;
					}
					else if (_BaseSol.GetExp() > current2.GetExp())
					{
						_BaseSol = current2;
					}
				}
			}
		}
		foreach (NkSoldierInfo current3 in readySolList.GetList().Values)
		{
			if (this.CheckRecommandBaseSol(current3, 0))
			{
				if (_BaseSol != null && _BaseSol.GetSolID() != current3.GetSolID())
				{
					if (current3.GetExp() == 0L && current3.GetGrade() < 3 && !current3.IsAwakening() && !current3.IsAtbCommonFlag(1L))
					{
						_SubSolIDList.Add(current3.GetSolID());
					}
					else if (_BaseSol.IsLeader())
					{
						if (current3.GetCharKind() == 1131 && !_BaseSol.IsMaxLevel())
						{
							_SubSolIDList.Add(current3.GetSolID());
						}
					}
					else if (current3.GetCharKind() == 1129)
					{
						_SubSolIDList.Add(current3.GetSolID());
					}
				}
			}
		}
		if (_BaseSol != null && _BaseSol.IsMaxLevel())
		{
			_BaseSol = null;
		}
		if (_BaseSol == null)
		{
			_SubSolIDList.Clear();
			return false;
		}
		if (this.m_RecommandSol.Contains(_BaseSol.GetCharKind()))
		{
			_BaseSol = null;
			_SubSolIDList.Clear();
			return false;
		}
		if (_SubSolIDList.Count > 0)
		{
			List<long> list = new List<long>();
			for (int j = 0; j < _SubSolIDList.Count; j++)
			{
				NkSoldierInfo solInfo = readySolList.GetSolInfo(_SubSolIDList[j]);
				NrCharKindInfo charKindInfo = solInfo.GetCharKindInfo();
				if (charKindInfo != null)
				{
					BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = charKindInfo.GetCHARKIND_SOLGRADEINFO((int)_BaseSol.GetGrade());
					if (cHARKIND_SOLGRADEINFO != null)
					{
						if (_BaseSol.IsLeader() && cHARKIND_SOLGRADEINFO.ComposeLimit == 1)
						{
							list.Add(_SubSolIDList[j]);
						}
						else if (!_BaseSol.IsLeader() && cHARKIND_SOLGRADEINFO.ComposeLimit == 2)
						{
							list.Add(_SubSolIDList[j]);
						}
						else if (_BaseSol.GetSeason() < solInfo.GetSeason())
						{
							list.Add(_SubSolIDList[j]);
						}
						else if (solInfo.IsAwakening())
						{
							list.Add(_SubSolIDList[j]);
						}
						else if (solInfo.IsAtbCommonFlag(1L))
						{
							list.Add(_SubSolIDList[j]);
						}
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				_SubSolIDList.Remove(list[j]);
			}
			list.Clear();
		}
		return true;
	}

	private bool CheckRecommandBaseSol(NkSoldierInfo kSolInfo, byte SolPosType)
	{
		return kSolInfo != null && kSolInfo.GetSolID() > 0L && kSolInfo.GetSolPosType() == SolPosType && (kSolInfo.IsLeader() || kSolInfo.GetGrade() <= 4) && kSolInfo.GetLegendType() <= 0;
	}

	private bool FindRecommandSameSol(ref NkSoldierInfo _BaseSol, ref List<long> _SubSolIDList, ref Dictionary<int, SolComposeMainDlg.SOLKINDTONUM> _SolKindToNum)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
				if (this.CheckRecommandBaseSol(soldierInfo, 1))
				{
					SolComposeMainDlg.SOLKINDTONUM sOLKINDTONUM = new SolComposeMainDlg.SOLKINDTONUM();
					sOLKINDTONUM.Kind = soldierInfo.GetCharKind();
					sOLKINDTONUM.Count = 1;
					sOLKINDTONUM.bBattle = true;
					_SolKindToNum.Add(soldierInfo.GetCharKind(), sOLKINDTONUM);
				}
			}
		}
		List<NkSoldierInfo> solWarehouseList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouseList();
		foreach (NkSoldierInfo current in solWarehouseList)
		{
			if (this.CheckRecommandBaseSol(current, 5))
			{
				int charKind = current.GetCharKind();
				if (_SolKindToNum.ContainsKey(charKind))
				{
					_SolKindToNum[charKind].bWareHouse = true;
				}
				else
				{
					SolComposeMainDlg.SOLKINDTONUM sOLKINDTONUM2 = new SolComposeMainDlg.SOLKINDTONUM();
					sOLKINDTONUM2.Kind = current.GetCharKind();
					sOLKINDTONUM2.Count = 1;
					sOLKINDTONUM2.bWareHouse = true;
					_SolKindToNum.Add(current.GetCharKind(), sOLKINDTONUM2);
				}
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		foreach (NkSoldierInfo current2 in readySolList.GetList().Values)
		{
			if (this.CheckRecommandBaseSol(current2, 0))
			{
				int charKind2 = current2.GetCharKind();
				if (_SolKindToNum.ContainsKey(charKind2))
				{
					_SolKindToNum[charKind2].Count = _SolKindToNum[charKind2].Count + 1;
					_SolKindToNum[charKind2].bReady = true;
					if (current2.GetExp() == 0L)
					{
						_SolKindToNum[charKind2].ZeroExpSol = true;
					}
				}
				else
				{
					SolComposeMainDlg.SOLKINDTONUM sOLKINDTONUM3 = new SolComposeMainDlg.SOLKINDTONUM();
					sOLKINDTONUM3.Kind = current2.GetCharKind();
					sOLKINDTONUM3.Count = 1;
					sOLKINDTONUM3.bReady = true;
					if (current2.GetExp() == 0L)
					{
						sOLKINDTONUM3.ZeroExpSol = true;
					}
					_SolKindToNum.Add(charKind2, sOLKINDTONUM3);
				}
			}
		}
		foreach (SolComposeMainDlg.SOLKINDTONUM current3 in _SolKindToNum.Values)
		{
			if (current3.Count > 1 && current3.ZeroExpSol && !this.m_RecommandSol.Contains(current3.Kind) && this.FindSameSol(current3.Kind, ref _BaseSol, ref _SubSolIDList))
			{
				this.m_RecommandSol.Add(current3.Kind);
				break;
			}
		}
		if (_BaseSol == null)
		{
			_SubSolIDList.Clear();
			return false;
		}
		return true;
	}

	private bool FindRecommandLeaderSol(ref NkSoldierInfo _BaseSol, ref List<long> _SubSolIDList)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (charPersonInfo != null)
		{
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
				if (this.CheckRecommandBaseSol(soldierInfo, 1))
				{
					if (soldierInfo.IsLeader())
					{
						if (soldierInfo.IsMaxLevel() && soldierInfo.IsMaxGrade())
						{
							return false;
						}
						_BaseSol = soldierInfo;
					}
				}
			}
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolPosType() == 0)
			{
				if (current.GetExp() == 0L && current.GetGrade() < 3)
				{
					if (current.IsAtbCommonFlag(1L))
					{
						continue;
					}
					NrCharKindInfo charKindInfo = current.GetCharKindInfo();
					if (charKindInfo != null)
					{
						BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = charKindInfo.GetCHARKIND_SOLGRADEINFO((int)_BaseSol.GetGrade());
						if (cHARKIND_SOLGRADEINFO != null)
						{
							if (cHARKIND_SOLGRADEINFO.ComposeLimit == 1)
							{
								continue;
							}
							if (_BaseSol.GetSeason() < current.GetSeason())
							{
								continue;
							}
						}
						_SubSolIDList.Add(current.GetSolID());
					}
				}
				if (_BaseSol != null && _BaseSol.IsLeader() && current.GetCharKind() == 1131 && !_BaseSol.IsMaxLevel())
				{
					_SubSolIDList.Add(current.GetSolID());
				}
			}
		}
		if (this.m_RecommandSol.Contains(_BaseSol.GetCharKind()))
		{
			_BaseSol = null;
			_SubSolIDList.Clear();
			return false;
		}
		return true;
	}

	private bool LatestSolRecommand(ref NkSoldierInfo _BaseSol, ref List<long> _SubSolIDList)
	{
		if (_BaseSol.IsMaxLevel() && _BaseSol.IsMaxGrade())
		{
			return false;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (!_BaseSol.IsMaxGrade())
		{
			foreach (NkSoldierInfo current in readySolList.GetList().Values)
			{
				if (current.GetSolPosType() == 0)
				{
					if (_BaseSol.GetSolID() != current.GetSolID())
					{
						if (current.GetCharKind() == _BaseSol.GetCharKind())
						{
							if (_BaseSol.GetGrade() >= current.GetGrade())
							{
								if (current.GetLevel() == 1)
								{
									if (_BaseSol == null || _BaseSol.GetGrade() >= current.GetGrade())
									{
										_SubSolIDList.Add(current.GetSolID());
									}
								}
								else if (_BaseSol.GetGrade() == current.GetGrade() && _BaseSol.GetLevel() > current.GetLevel())
								{
									if (_BaseSol.GetSolPosType() == 0)
									{
										_SubSolIDList.Add(_BaseSol.GetSolID());
									}
									_BaseSol = current;
								}
								else if (current.GetExp() == 0L)
								{
									_SubSolIDList.Add(current.GetSolID());
								}
							}
						}
					}
				}
			}
			if (_SubSolIDList != null && _SubSolIDList.Count > 0)
			{
				return true;
			}
		}
		foreach (NkSoldierInfo current2 in readySolList.GetList().Values)
		{
			if (current2.GetSolPosType() == 0)
			{
				if (current2.GetExp() == 0L && current2.GetGrade() < 3 && current2.GetSolPosType() == 0 && _BaseSol.GetSeason() >= current2.GetSeason())
				{
					_SubSolIDList.Add(current2.GetSolID());
				}
			}
		}
		if (_SubSolIDList.Count == 0)
		{
			_BaseSol = null;
			return false;
		}
		return true;
	}

	public bool SelectBase(long SolID)
	{
		this.mBaseSol = SolComposeMainDlg.GetSoldierInfo(SolID);
		NrCharKindInfo charKindInfo = this.mBaseSol.GetCharKindInfo();
		if (charKindInfo != null)
		{
			BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = charKindInfo.GetCHARKIND_SOLGRADEINFO((int)this.mBaseSol.GetGrade());
			if (cHARKIND_SOLGRADEINFO != null)
			{
				this.mBaseSolSeason = (byte)cHARKIND_SOLGRADEINFO.SolSeason;
			}
		}
		this.CalcData();
		switch (this.m_ShowType)
		{
		case SOLCOMPOSE_TYPE.COMPOSE:
			this.SetComposeBaseSol(true);
			break;
		case SOLCOMPOSE_TYPE.EXTRACT:
			this.ShowExtractSol(true);
			break;
		case SOLCOMPOSE_TYPE.TRANSCENDENCE:
			this.ShowTranscendence(true);
			break;
		}
		if (this.m_SaveSubSolIDList.Count > 0)
		{
			this.SelectSub(this.m_SaveSubSolIDList);
			this.m_SaveSubSolIDList.Clear();
		}
		return true;
	}

	public bool SelectBase(List<long> List)
	{
		if (this.m_ShowType != SOLCOMPOSE_TYPE.EXTRACT)
		{
			if (0 < List.Count)
			{
				return this.SelectBase(List[0]);
			}
		}
		return true;
	}

	public void RefreshSelectExtract()
	{
		if (this.m_ShowType == SOLCOMPOSE_TYPE.EXTRACT)
		{
			NewListBox listBox = this.GetListBox();
			listBox.Clear();
			for (int i = 0; i < this.m_SolExtract.Count; i++)
			{
				this.AddExtractSolList(this.m_SolExtract[i]);
			}
			this.ShowExtractSol(true);
			this.CalcExtractData();
			listBox.RepositionItems();
		}
	}

	public bool SelectSub(List<long> List)
	{
		if (0 < List.Count)
		{
			if (this.m_ShowType == SOLCOMPOSE_TYPE.EXTRACT)
			{
				this.m_SolExtract.Clear();
				NewListBox listBox = this.GetListBox();
				listBox.Clear();
				foreach (long current in List)
				{
					if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetFaceSolID() != current)
					{
						NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(List[0]);
						if (this.CheckUpAble(null, soldierInfo))
						{
							this.m_SolExtract.Add(current);
							this.AddExtractSolList(current);
						}
					}
				}
				this.ShowExtractSol(true);
				this.CalcExtractData();
				listBox.RepositionItems();
			}
			else
			{
				this.mSubSolList.Clear();
				NewListBox listBox2 = this.GetListBox();
				listBox2.Clear();
				foreach (long current2 in List)
				{
					if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetFaceSolID() != current2)
					{
						NkSoldierInfo soldierInfo2 = SolComposeMainDlg.GetSoldierInfo(List[0]);
						if (this.CheckUpAble(this.mBaseSol, soldierInfo2))
						{
							this.mSubSolList.Add(current2);
							this.AddSubSolList(current2);
						}
					}
				}
				this.CalcData();
				listBox2.RepositionItems();
				this.SetSoldierNum();
			}
		}
		return true;
	}

	public void ClearList()
	{
		this.mSubSolList.Clear();
		this.m_SolExtract.Clear();
		NewListBox listBox = this.GetListBox();
		if (listBox != null)
		{
			listBox.Clear();
		}
		this.SetSoldierNum();
		this.lbSelMoney.SetText("0");
	}

	private void SetComposeBaseSol(bool bSet)
	{
		this.dtBase.Visible = bSet;
		this.SolRank.Visible = bSet;
		this.lblExp.Visible = bSet;
		this.lblComposeText.Visible = false;
		this.dtGage.Visible = bSet;
		this.btnOk.Visible = bSet;
		this.btnSubSelect.Visible = bSet;
	}

	private void SolComposeInit()
	{
		this.mBaseSol = null;
		this.dtBase.SetTexture(string.Empty);
		this.lbBaseName.SetText(string.Empty);
		this.lblBaseSeasonText.SetText(string.Empty);
		this.SolRank.SetTexture(string.Empty);
		this.lblComposeCost.SetText(string.Empty);
		this.lblExp.SetText(string.Empty);
		this.lbMoney.SetText(string.Empty);
		this.SetComposeBaseSol(false);
	}

	public void CalcData()
	{
		switch (this.m_ShowType)
		{
		case SOLCOMPOSE_TYPE.COMPOSE:
			this.CalcumData();
			break;
		case SOLCOMPOSE_TYPE.SELL:
			this.CalcSellData();
			break;
		case SOLCOMPOSE_TYPE.EXTRACT:
			this.CalcExtractData();
			break;
		case SOLCOMPOSE_TYPE.TRANSCENDENCE:
			this.CalcTranscendence();
			break;
		}
	}

	private void SetSoldierNum()
	{
		string empty = string.Empty;
		SOLCOMPOSE_TYPE showType = this.m_ShowType;
		if (showType != SOLCOMPOSE_TYPE.COMPOSE)
		{
			if (showType == SOLCOMPOSE_TYPE.SELL)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("53"),
					"count1",
					this.mSubSolList.Count,
					"count2",
					50
				});
				this.lbSubNum2.Text = empty;
			}
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("63"),
				"count1",
				this.mSubSolList.Count,
				"count2",
				50
			});
			this.lbSubNum.Text = empty;
		}
	}

	public void RefreshMoney()
	{
		string text = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			text = string.Format("{0:###,###,###,##0}", kMyCharInfo.m_Money);
		}
		this.lbMoney.SetText(text);
	}

	protected virtual void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			if (this.m_ShowType == SOLCOMPOSE_TYPE.COMPOSE)
			{
				gameHelpList_Dlg.SetViewType(eHELP_LIST.Soldier_Synthesis.ToString());
			}
			else if (this.m_ShowType == SOLCOMPOSE_TYPE.EXTRACT)
			{
				gameHelpList_Dlg.SetViewType(eHELP_LIST.Soldier_Extract.ToString());
			}
			else if (this.m_ShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
			{
				gameHelpList_Dlg.SetViewType(eHELP_LIST.Soldier_Transcend.ToString());
			}
		}
	}

	public virtual void OnClickBuyGold(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_GOLD, true);
	}

	private void OnClickUnsetCostume(IUIObject obj)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)obj.Data;
		if (nkSoldierInfo == null)
		{
			return;
		}
		CharCostumeInfo_Data normalCostumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetNormalCostumeData(nkSoldierInfo.GetCharCode());
		if (normalCostumeData == null || !normalCostumeData.IsNormalCostume())
		{
			Debug.LogError("ERROR, SolComposeMainDlg.cs, OnClickUnsetCostume(), costumeNormalData is error ");
			return;
		}
		NrTSingleton<CostumeWearManager>.Instance.RequestCostumeWear(nkSoldierInfo, normalCostumeData.m_costumeUnique);
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (string.IsNullOrEmpty(param1))
		{
			return;
		}
		if (this.guideWinIDList != null && !this.guideWinIDList.Contains(winID))
		{
			this.guideWinIDList.Add(winID);
		}
		string[] array = param1.Split(new char[]
		{
			','
		});
		if (array == null || array.Length != 4)
		{
			return;
		}
		IUIObject control = base.GetControl(array[0]);
		if (control == null)
		{
			return;
		}
		if (this._Touch == null)
		{
			this._Touch = UICreateControl.Button("touch", "Main_I_Touch01", 196f, 154f);
		}
		if (this._Touch == null)
		{
			return;
		}
		int anchor = int.Parse(array[1]);
		this._Touch.SetAnchor((SpriteRoot.ANCHOR_METHOD)anchor);
		this._Touch.PlayAni(true);
		this._Touch.gameObject.SetActive(true);
		this._Touch.gameObject.transform.parent = control.gameObject.transform;
		this._Touch.transform.position = new Vector3(control.transform.position.x, control.transform.position.y, control.transform.position.z - 3f);
		float x = float.Parse(array[2]);
		float y = float.Parse(array[3]);
		this._Touch.transform.eulerAngles = new Vector3(x, y, this._Touch.transform.eulerAngles.z);
		BoxCollider component = this._Touch.gameObject.GetComponent<BoxCollider>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	protected void HideTouch(bool closeUI)
	{
		if (this._Touch != null && this._Touch.gameObject != null)
		{
			this._Touch.gameObject.SetActive(false);
		}
		if (!closeUI)
		{
			return;
		}
		if (this.guideWinIDList == null)
		{
			return;
		}
		foreach (int current in this.guideWinIDList)
		{
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)current) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				uI_UIGuide.CloseUI = true;
			}
		}
		this._Touch = null;
	}

	public int GetExtractSolCount()
	{
		if (this.lxSelectList == null)
		{
			return -1;
		}
		return this.lxSelectList.Count;
	}

	public void SetTranscendenceComponent()
	{
		this.btn_Transcendence_StartButton = (base.GetControl("Button_Transcendence_StartButton") as Button);
		this.btn_TranscendenceBase_Button1 = (base.GetControl("Button_TranscendenceBase_Button1") as Button);
		this.btn_TranscendenceBase_Button2 = (base.GetControl("Button_TranscendenceBase_Button2") as Button);
		this.btn_TranscendenceBase_Button3 = (base.GetControl("Button_TranscendenceBase_Button3") as Button);
		this.btn_Transcendence_StartButton.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickTranscendenceStart));
		this.btn_TranscendenceBase_Button1.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickTranscendenceBase));
		this.btn_TranscendenceBase_Button2.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickTranscendenceBase));
		this.btn_TranscendenceBase_Button3.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickTranscendenceBase));
		this.lb_TranscendenceBase_SolName = (base.GetControl("Label_TranscendenceBase_SolName") as Label);
		this.lb_Transcendence_Money = (base.GetControl("Label_Transcendence_Money") as Label);
		this.lb_TranscendenceUseMoney = (base.GetControl("Label_TranscendenceUseMoney") as Label);
		this.lb_Tarnscendence_ExpGet = (base.GetControl("Label_Tarnscendence_ExpGet") as Label);
		this.dt_Transcendence_GaugeBar = (base.GetControl("DrawTexture_Transcendence_GaugeBar") as DrawTexture);
		this.GAGE_TRANSCENDENCE_WIDTH = this.dt_Transcendence_GaugeBar.GetSize().x;
		this.dt_TranscendenceBase_SolImg = (base.GetControl("DrawTexture_TranscendenceBase_SolImg") as DrawTexture);
		this.dt_TranscendenceBase_SolRank = (base.GetControl("DrawTexture_TranscendenceBase_SolRank") as DrawTexture);
		this.lb_Transcendence_SuccessRate_Text = (base.GetControl("Label_Transcendence_SuccessRate_Text") as Label);
		this.lb_Transcendence_SuccessRate = (base.GetControl("Label_Transcendence_SuccessRate") as Label);
		this.lb_TranscendenceBase_Help = (base.GetControl("Label_TranscendenceBase_Help") as Label);
		this.lb_Transcendence_Help = (base.GetControl("Label_Transcendence_Help") as Label);
		this.btn_TranscendenceUse_Add1 = (base.GetControl("Button_TranscendenceUse_Add1") as Button);
		this.btn_TranscendenceUse_Add1.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickTranscendenceUseAdd));
		this.btn_TranscendenceUse_Add2 = (base.GetControl("Button_TranscendenceUse_Add2") as Button);
		this.btn_TranscendenceUse_Add2.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickTranscendenceUseAdd));
		this.btn_TranscendenceUse_Add3 = (base.GetControl("Button_TranscendenceUse_Add3") as Button);
		this.btn_TranscendenceUse_Add3.AddMouseDownDelegate(new EZValueChangedDelegate(this.OnClickTranscendenceUseAdd));
		this.lb_Transcendence_Help2 = (base.GetControl("Label_Transcendence_Help2") as Label);
		this.lb_TarnscendenceBaseSeasont = (base.GetControl("Label_TranscendenceBaseSeason") as Label);
		this.lxList4 = (base.GetControl("Sol_Listbox4") as NewListBox);
		this.dt_DrawTexture_PercentagePlus = (base.GetControl("DrawTexture_PercentagePlus") as DrawTexture);
		this.lb_Label_AddPercentage = (base.GetControl("Label_AddPercentage") as Label);
		this.lb_Transcendence_AddPercentage_Text = (base.GetControl("Label_Transcendence_AddPercentage_Text") as Label);
		this.lb_Transcendence_AddPercentage = (base.GetControl("Label_Transcendence_AddPercentage") as Label);
	}

	public void ShowTranscendence(bool bShowTranscendence)
	{
		this.dt_Transcendence_GaugeBar.Visible = bShowTranscendence;
		this.dt_TranscendenceBase_SolImg.Visible = bShowTranscendence;
		this.dt_TranscendenceBase_SolRank.Visible = bShowTranscendence;
		this.lb_Transcendence_Money.SetText(this.GetMoneyFormat(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money));
		this.btn_Transcendence_StartButton.Visible = false;
		if (this.mBaseSol != null)
		{
			if (this.mBaseSol.GetGrade() >= 6)
			{
				this.lb_TranscendenceBase_Help.Visible = false;
			}
			else
			{
				this.mBaseSol = null;
				this.lb_TranscendenceBase_Help.Visible = true;
			}
			if (this.mBaseSol.GetGrade() == 9 || this.mBaseSol.GetGrade() >= 13)
			{
				this.btn_TranscendenceUse_Add1.Visible = false;
				this.btn_TranscendenceUse_Add2.Visible = false;
				this.btn_TranscendenceUse_Add3.Visible = false;
				this.lb_Label_AddPercentage.Visible = false;
			}
			else
			{
				this.btn_TranscendenceUse_Add1.Visible = true;
				this.btn_TranscendenceUse_Add2.Visible = true;
				this.btn_TranscendenceUse_Add3.Visible = true;
				this.lb_Label_AddPercentage.Visible = true;
			}
		}
		else
		{
			this.btn_TranscendenceUse_Add1.Visible = false;
			this.btn_TranscendenceUse_Add2.Visible = false;
			this.btn_TranscendenceUse_Add3.Visible = false;
			this.lb_TranscendenceBase_Help.Visible = true;
			this.lb_Label_AddPercentage.Visible = false;
		}
		this.lb_TranscendenceUseMoney.Visible = false;
		this.lb_Tarnscendence_ExpGet.Visible = false;
		this.lb_Transcendence_SuccessRate.Visible = false;
		this.lb_Transcendence_SuccessRate_Text.Visible = false;
		this.lb_Transcendence_AddPercentage_Text.Visible = false;
		this.lb_Transcendence_AddPercentage.Visible = false;
		this.dt_DrawTexture_PercentagePlus.Visible = false;
		this.lb_Transcendence_Help2.Visible = false;
		if (this.mSubSolList.Count == 1)
		{
			this.CalcTranscendence();
		}
	}

	protected virtual void OnClickTranscendenceStart(IUIObject obj)
	{
		if (this.mBaseSol == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (this.mSubSolList.Count > 1)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		NkSoldierInfo nkSoldierInfo = null;
		foreach (long current in this.mSubSolList)
		{
			nkSoldierInfo = SolComposeMainDlg.GetReadySoldierInfo(current);
		}
		if (nkSoldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		long transcendenceMoney = NrTSingleton<NrTableTranscendenceManager>.Instance.GetTranscendenceMoney(this.mBaseSol.GetGrade(), this.mBaseSol.GetSeason(), nkSoldierInfo.GetGrade(), nkSoldierInfo.GetSeason());
		if (transcendenceMoney > kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("787"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (nkSoldierInfo.IsAtbCommonFlag(1L))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("788"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (this.CheckEquipItem(nkSoldierInfo))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("789"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (nkSoldierInfo.GetFriendPersonID() > 0L)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("790"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (nkSoldierInfo.IsCostumeEquip())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("856"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("294"),
			"charname",
			nkSoldierInfo.GetName()
		});
		msgBoxUI.SetMsg(new YesDelegate(this.OnOKStart), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2874"), empty, eMsgType.MB_OK_CANCEL, 2);
	}

	public void OnOKStart(object a_oObject)
	{
		if (this.mBaseSol == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (this.mSubSolList.Count > 1)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		NkSoldierInfo nkSoldierInfo = null;
		foreach (long current in this.mSubSolList)
		{
			nkSoldierInfo = SolComposeMainDlg.GetReadySoldierInfo(current);
		}
		if (nkSoldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		GS_TRANSCENDENCS_REQ gS_TRANSCENDENCS_REQ = new GS_TRANSCENDENCS_REQ();
		gS_TRANSCENDENCS_REQ.i64PersonID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID;
		gS_TRANSCENDENCS_REQ.i64BaseSolID = this.mBaseSol.GetSolID();
		gS_TRANSCENDENCS_REQ.i64MaterialSolID = nkSoldierInfo.GetSolID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TRANSCENDENCS_REQ, gS_TRANSCENDENCS_REQ);
	}

	protected virtual void OnClickTranscendenceBase(IUIObject obj)
	{
		this.HideTouch(false);
		SolComposeListDlg.LoadSelectList(true, this.m_ShowType);
	}

	protected virtual void OnClickTranscendenceUseAdd(IUIObject obj)
	{
		this.HideTouch(false);
		if (this.btn_TranscendenceUse_Add1.Visible && this.btn_TranscendenceUse_Add2.Visible && this.btn_TranscendenceUse_Add3.Visible)
		{
			SolComposeListDlg.LoadSelectList(false, this.m_ShowType);
		}
	}

	public void CalcTranscendence()
	{
		string empty = string.Empty;
		short num = 0;
		long num2 = 0L;
		long num3 = 0L;
		if (this.mBaseSol != null)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("290"),
				"targetname",
				this.mBaseSol.GetName(),
				"count1",
				this.mBaseSol.GetLevel().ToString(),
				"count2",
				this.mBaseSol.GetSolMaxLevel().ToString()
			});
			int charKind = this.mBaseSol.GetCharKind();
			byte grade = this.mBaseSol.GetGrade();
			num = this.mBaseSol.GetLevel();
			this.lb_TranscendenceBase_SolName.SetText(empty);
			string costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(this.mBaseSol);
			this.dt_TranscendenceBase_SolImg.SetTexture(eCharImageType.LARGE, charKind, (int)grade, costumePortraitPath);
			string empty2 = string.Empty;
			int num4 = this.mBaseSol.GetSeason() + 1;
			if (num4 != 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3274"),
					"season",
					num4
				});
			}
			this.lb_TarnscendenceBaseSeasont.SetText(empty2);
			short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(this.mBaseSol.GetCharKind(), (int)this.mBaseSol.GetGrade());
			UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(this.mBaseSol.GetCharKind(), (int)this.mBaseSol.GetGrade());
			if (0 < legendType)
			{
				this.dt_TranscendenceBase_SolRank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
			}
			this.dt_TranscendenceBase_SolRank.SetTexture(solLargeGradeImg);
			long num5 = this.mBaseSol.GetNextExp() - this.mBaseSol.GetCurBaseExp();
			float num6 = ((float)num5 - (float)(this.mBaseSol.GetRemainExp() - num2)) / (float)num5;
			if (this.mBaseSol.IsMaxLevel() || this.mBaseSol.GetSolMaxLevel() <= num)
			{
				num6 = 1f;
			}
			if (num6 > 1f)
			{
				num6 = 1f;
			}
			if (0f > num6)
			{
				num6 = 0f;
			}
			this.dt_Transcendence_GaugeBar.SetSize(this.GAGE_TRANSCENDENCE_WIDTH * num6, this.dtGage.GetSize().y);
			long solSubData = this.mBaseSol.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_TRANSCENDENCE_PLUS);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3283"),
				"count",
				(float)solSubData * 0.01f
			});
			this.lb_Label_AddPercentage.SetText(empty2);
		}
		else
		{
			this.dt_TranscendenceBase_SolRank.SetTexture(string.Empty);
			this.dt_TranscendenceBase_SolImg.SetTexture(string.Empty);
			this.lb_TranscendenceBase_SolName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2031"));
			this.lbSolNameSpace.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2826"));
			this.lb_TarnscendenceBaseSeasont.SetText(string.Empty);
		}
		this.lb_Transcendence_Help.Visible = false;
		if (this.mSubSolList.Count == 0)
		{
			this.lb_TranscendenceUseMoney.Visible = false;
			this.lb_Tarnscendence_ExpGet.Visible = false;
			this.lb_Transcendence_SuccessRate.Visible = false;
			this.lb_Transcendence_SuccessRate_Text.Visible = false;
			this.lb_Transcendence_Help2.Visible = false;
			this.btn_Transcendence_StartButton.Visible = false;
			this.lb_Transcendence_Help.Visible = true;
			this.lb_Transcendence_AddPercentage_Text.Visible = false;
			this.lb_Transcendence_AddPercentage.Visible = false;
			this.dt_DrawTexture_PercentagePlus.Visible = false;
		}
		else
		{
			if (this.mBaseSol == null)
			{
				return;
			}
			if (this.mSubSolList.Count > 1)
			{
				this.mSubSolList.Clear();
			}
			else
			{
				this.btn_Transcendence_StartButton.Visible = true;
				NkSoldierInfo nkSoldierInfo = null;
				foreach (long current in this.mSubSolList)
				{
					nkSoldierInfo = SolComposeMainDlg.GetReadySoldierInfo(current);
				}
				if (nkSoldierInfo == null)
				{
					return;
				}
				long transcendenceMoney = NrTSingleton<NrTableTranscendenceManager>.Instance.GetTranscendenceMoney(this.mBaseSol.GetGrade(), this.mBaseSol.GetSeason(), nkSoldierInfo.GetGrade(), nkSoldierInfo.GetSeason());
				if (transcendenceMoney <= 0L)
				{
					this.btn_Transcendence_StartButton.Visible = false;
				}
				this.lb_TranscendenceUseMoney.Visible = true;
				this.lb_TranscendenceUseMoney.SetText(this.GetMoneyFormat(transcendenceMoney));
				int transcendenceRate = NrTSingleton<NrTableTranscendenceManager>.Instance.GetTranscendenceRate(this.mBaseSol.GetGrade(), this.mBaseSol.GetSeason(), this.mBaseSol.GetCharKind(), nkSoldierInfo.GetGrade(), nkSoldierInfo.GetSeason(), nkSoldierInfo.GetCharKind());
				short transcendenceFailItemNum = NrTSingleton<NrTableTranscendenceManager>.Instance.GetTranscendenceFailItemNum(this.mBaseSol.GetGrade(), this.mBaseSol.GetSeason(), nkSoldierInfo.GetGrade(), nkSoldierInfo.GetSeason());
				int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TRANSCENDENCE_PLUSRATE);
				string text = string.Empty;
				if (transcendenceRate <= 0 || transcendenceRate > 10000 || transcendenceFailItemNum <= 0)
				{
					this.btn_Transcendence_StartButton.Visible = false;
				}
				else
				{
					this.lb_Transcendence_SuccessRate.Visible = true;
					text = string.Format(" {0} %", (float)transcendenceRate * 0.01f);
					this.lb_Transcendence_SuccessRate.SetText(text);
					long solSubData2 = this.mBaseSol.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_TRANSCENDENCE_PLUS);
					this.lb_Transcendence_AddPercentage.Visible = true;
					text = string.Format(" {0} %", (float)solSubData2 * 0.01f);
					this.lb_Transcendence_AddPercentage.SetText(text);
				}
				this.GetComposeExp(ref num, nkSoldierInfo, ref num2, ref num3, this.mBaseSol, ref this.mMaxLvEvelution);
				string text2 = string.Empty;
				if (this.mBaseSol.IsMaxLevel() || this.mBaseSol.GetSolMaxLevel() <= num)
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
				}
				else if (this.mBaseSol != null && this.mBaseSol.GetLevel() == num)
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2035");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text2,
						"count",
						num2
					});
				}
				else if (this.mBaseSol != null && this.mBaseSol.GetLevel() != num)
				{
					int num7 = Math.Min((int)this.mBaseSol.GetSolMaxLevel(), (int)(num - this.mBaseSol.GetLevel()));
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1732");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text2,
						"count",
						num7.ToString()
					});
				}
				long num8 = this.mBaseSol.GetNextExp() - this.mBaseSol.GetCurBaseExp();
				float num9 = ((float)num8 - (float)(this.mBaseSol.GetRemainExp() - num2)) / (float)num8;
				if (this.mBaseSol.IsMaxLevel() || this.mBaseSol.GetSolMaxLevel() <= num)
				{
					num9 = 1f;
				}
				if (num9 > 1f)
				{
					num9 = 1f;
				}
				if (0f > num9)
				{
					num9 = 0f;
				}
				this.lb_Tarnscendence_ExpGet.Visible = (0L < num2);
				this.lb_Tarnscendence_ExpGet.SetText(text2);
				this.dt_Transcendence_GaugeBar.SetSize(this.GAGE_TRANSCENDENCE_WIDTH * num9, this.dtGage.GetSize().y);
				this.lb_Transcendence_SuccessRate_Text.Visible = true;
				this.lb_Transcendence_Help2.Visible = true;
				float num10 = (float)(transcendenceRate * value) * 0.0001f;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2869"),
					"count",
					transcendenceFailItemNum,
					"count1",
					num10
				});
				this.lb_Transcendence_Help2.SetText(text);
				this.lb_Transcendence_AddPercentage_Text.Visible = true;
				this.dt_DrawTexture_PercentagePlus.Visible = true;
			}
		}
	}

	public string GetMoneyFormat(long lmoney)
	{
		string empty = string.Empty;
		return string.Format("{0:###,###,###,##0}", lmoney);
	}

	public static NkSoldierInfo GetReadySoldierInfo(long SoldID)
	{
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		return readySolList.GetSolInfo(SoldID);
	}
}
