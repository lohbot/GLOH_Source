using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolAwakeningDlg : Form
{
	private float TEX_SIZE = 512f;

	private float COOL_TIME = 3f;

	private float TEXT_AIN_TIME = 0.5f;

	private Button m_btSol_1;

	private Button m_btSol_2;

	private Label m_lbSolName;

	private DrawTexture m_dtSolGrade;

	private DrawTexture m_dtSolImage;

	private Label m_lbGuideText_1;

	private Label m_lbGuideText_2;

	private Label[] m_lbSolStat_1 = new Label[4];

	private Label[] m_lbSolStat_2 = new Label[4];

	private Label[] m_lbBaseStat = new Label[4];

	private Label[] m_lbCurStat = new Label[4];

	private Label[] m_lbIncStat = new Label[4];

	private DrawTexture[] m_dtTextBG = new DrawTexture[4];

	private Button m_btStatSelect;

	private Label m_lbMyAwakeningItemNum;

	private Label m_lbNeedAwakeningItemNum;

	private Button m_btStatCalc;

	private Label m_lbCount;

	private DrawTexture m_dtBGImg;

	private DrawTexture m_dtInvenSlotBG;

	private DrawTexture m_dtRingSlot;

	private DrawTexture m_dtTextBGRingSlot;

	private Label m_lbCurRingSlot;

	private Label m_lbStatRingSlot;

	private Button m_btAwakeningReset;

	private Button m_HelpButton;

	private long m_SolID;

	private string m_strStat = string.Empty;

	private string m_strText = string.Empty;

	private float m_fCoolTime;

	private int m_iAwakeningItemNum;

	private int m_iAwakeningItemUnique;

	private List<AWAKENING_TRY_INFO> m_AwakeningTryList = new List<AWAKENING_TRY_INFO>();

	private int[] m_iAwakeningStat = new int[4];

	private string m_strBaseTextColor = string.Empty;

	private string m_strIncTextColor = string.Empty;

	private float[] m_fTextAniTime = new float[4];

	private string[] m_strBaseText = new string[4];

	private GameObject m_gbWakeup_Glow;

	private GameObject m_gbWakeup_Start;

	private GameObject m_gbWakeup_Apply;

	private GameObject m_gbWakeup_Text;

	private GameObject m_gbWakeup_Change_1;

	private GameObject m_gbWakeup_Change_2;

	private GameObject m_gbWakeup_Change_3;

	private bool m_bRingSlotOpen;

	private float m_fTextAniTimeRingSlot;

	private bool m_bEffectShow;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolAwakening", G_ID.SOLAWAKENING_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_btSol_1 = (base.GetControl("Button_BaseSol01") as Button);
		this.m_btSol_1.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolSelect));
		this.m_btSol_2 = (base.GetControl("Button_BaseSol02") as Button);
		this.m_btSol_2.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolSelect));
		this.m_lbSolName = (base.GetControl("Label_BaseSolName") as Label);
		this.m_dtSolGrade = (base.GetControl("DrawTexture_SolRank") as DrawTexture);
		this.m_dtSolImage = (base.GetControl("DrawTexture_BaseSolimg") as DrawTexture);
		this.m_lbGuideText_1 = (base.GetControl("Label_BaseGuideText") as Label);
		this.m_lbGuideText_2 = (base.GetControl("Label_BaseGuideText02") as Label);
		this.m_lbSolStat_1[0] = (base.GetControl("Label_STR_Text01") as Label);
		this.m_lbSolStat_1[1] = (base.GetControl("Label_DEX_Text01") as Label);
		this.m_lbSolStat_1[2] = (base.GetControl("Label_VIT_Text01") as Label);
		this.m_lbSolStat_1[3] = (base.GetControl("Label_INT_Text01") as Label);
		this.m_lbSolStat_2[0] = (base.GetControl("Label_STR_Text02") as Label);
		this.m_lbSolStat_2[1] = (base.GetControl("Label_DEX_Text02") as Label);
		this.m_lbSolStat_2[2] = (base.GetControl("Label_VIT_Text02") as Label);
		this.m_lbSolStat_2[3] = (base.GetControl("Label_INT_Text02") as Label);
		this.m_lbBaseStat[0] = (base.GetControl("Label_STR_Stat") as Label);
		this.m_lbBaseStat[1] = (base.GetControl("Label_DEX_Stat") as Label);
		this.m_lbBaseStat[2] = (base.GetControl("Label_VIT_Stat") as Label);
		this.m_lbBaseStat[3] = (base.GetControl("Label_INT_Stat") as Label);
		this.m_lbCurStat[0] = (base.GetControl("Label_STR_NowStat") as Label);
		this.m_lbCurStat[1] = (base.GetControl("Label_DEX_NowStat") as Label);
		this.m_lbCurStat[2] = (base.GetControl("Label_VIT_NowStat") as Label);
		this.m_lbCurStat[3] = (base.GetControl("Label_INT_NowStat") as Label);
		this.m_lbIncStat[0] = (base.GetControl("Label_STR_incStat") as Label);
		this.m_lbIncStat[1] = (base.GetControl("Label_DEX_incStat") as Label);
		this.m_lbIncStat[2] = (base.GetControl("Label_VIT_incStat") as Label);
		this.m_lbIncStat[3] = (base.GetControl("Label_INT_incStat") as Label);
		this.m_dtTextBG[0] = (base.GetControl("DrawTexture_Innerbg02") as DrawTexture);
		this.m_dtTextBG[1] = (base.GetControl("DrawTexture_Innerbg03") as DrawTexture);
		this.m_dtTextBG[2] = (base.GetControl("DrawTexture_Innerbg04") as DrawTexture);
		this.m_dtTextBG[3] = (base.GetControl("DrawTexture_Innerbg05") as DrawTexture);
		this.m_btStatSelect = (base.GetControl("StatSelect_Button") as Button);
		this.m_btStatSelect.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickStatSelect));
		this.m_btStatSelect.controlIsEnabled = false;
		this.m_lbMyAwakeningItemNum = (base.GetControl("Label_MyAknItemNum") as Label);
		this.m_lbNeedAwakeningItemNum = (base.GetControl("Label_NeedAknItemNum") as Label);
		this.m_btStatCalc = (base.GetControl("AknStart_Button") as Button);
		this.m_btStatCalc.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickStatCalc));
		this.m_lbCount = (base.GetControl("Label_Count") as Label);
		if (COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TRADECOUNT_USE) == 0)
		{
			this.m_lbCount.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2347"));
		}
		this.m_dtBGImg = (base.GetControl("DrawTexture_BGIMG") as DrawTexture);
		this.m_dtBGImg.SetTextureFromBundle("UI/Etc/awakeningBG");
		this.m_dtInvenSlotBG = (base.GetControl("DrawTexture_InvenSlot11") as DrawTexture);
		this.m_dtInvenSlotBG.Visible = false;
		this.m_dtRingSlot = (base.GetControl("DrawTexture_ringslotlock") as DrawTexture);
		this.m_dtRingSlot.Visible = false;
		this.m_dtTextBGRingSlot = (base.GetControl("DrawTexture_Innerbg06") as DrawTexture);
		this.m_lbCurRingSlot = (base.GetControl("Label_Ring_slot01") as Label);
		this.m_lbCurRingSlot.Visible = false;
		this.m_lbStatRingSlot = (base.GetControl("Label_Ring_slot02") as Label);
		this.m_lbStatRingSlot.Visible = false;
		this.m_btAwakeningReset = (base.GetControl("Button_reset") as Button);
		this.m_btAwakeningReset.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAwakeningReset));
		this.m_strBaseTextColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
		this.m_strIncTextColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1104");
		this.m_strBaseText[0] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1271");
		this.m_strBaseText[1] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1272");
		this.m_strBaseText[2] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1273");
		this.m_strBaseText[3] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1274");
		this.m_HelpButton = (base.GetControl("Help_Button") as Button);
		this.m_HelpButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		this.InitInfo();
		this.m_iAwakeningItemUnique = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_AWAKENING_ITEM_UNIQUE);
		this.m_lbMyAwakeningItemNum.SetText("0");
		this.m_lbNeedAwakeningItemNum.SetText("0");
		base.SetDeleagteCloseButton(new EZValueChangedDelegate(this.ClickClose));
		this.LoadEffect();
	}

	public void InitInfo()
	{
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strStat, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2289"),
			"count",
			1
		});
		this.m_btStatCalc.SetText(this.m_strStat);
		for (int i = 0; i < 4; i++)
		{
			this.m_lbSolStat_1[i].Hide(true);
			this.m_lbSolStat_2[i].Hide(true);
			this.m_lbBaseStat[i].SetText(string.Empty);
			this.m_lbCurStat[i].SetText(string.Empty);
			this.m_lbIncStat[i].SetText(string.Empty);
			this.m_iAwakeningStat[i] = 0;
		}
		this.m_lbStatRingSlot.Visible = false;
		this.m_bRingSlotOpen = false;
	}

	public void ClickSolSelect(IUIObject obj)
	{
		SolMilitarySelectDlg solMilitarySelectDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLMILITARYSELECT_DLG) as SolMilitarySelectDlg;
		if (solMilitarySelectDlg != null)
		{
			solMilitarySelectDlg.SetLoadType(SolMilitarySelectDlg.LoadType.SOLAWAKENING);
			solMilitarySelectDlg.SetLocationByForm(this);
			solMilitarySelectDlg.SetFocus();
			solMilitarySelectDlg.SolSortType = 2;
			solMilitarySelectDlg.SetSortList();
		}
	}

	public void ClickStatSelect(IUIObject obj)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo == null || this.m_SolID == 0L)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("698"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!this.IsAwakening())
		{
			return;
		}
		GS_SOLAWAKENING_REQ gS_SOLAWAKENING_REQ = new GS_SOLAWAKENING_REQ();
		gS_SOLAWAKENING_REQ.i64SolID = soldierInfo.GetSolID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLAWAKENING_REQ, gS_SOLAWAKENING_REQ);
		this.SetCoolTime();
		if (null != this.m_gbWakeup_Start)
		{
			this.m_gbWakeup_Start.SetActive(false);
		}
		if (null != this.m_gbWakeup_Apply)
		{
			this.m_gbWakeup_Apply.SetActive(true);
		}
		if (null != this.m_gbWakeup_Change_1)
		{
			this.m_gbWakeup_Change_1.SetActive(false);
		}
		if (null != this.m_gbWakeup_Change_2)
		{
			this.m_gbWakeup_Change_2.SetActive(false);
		}
		if (null != this.m_gbWakeup_Change_3)
		{
			this.m_gbWakeup_Change_3.SetActive(false);
		}
		this.m_bEffectShow = false;
		this.m_lbStatRingSlot.Visible = false;
	}

	public void ClickStatCalc(IUIObject obj)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo == null || this.m_SolID == 0L)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("698"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int num = NkUserInventory.GetInstance().Get_First_ItemCnt(this.m_iAwakeningItemUnique);
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_INFO);
		short n16SubData_ = sUBDATA_UNION.n16SubData_0;
		if (!this.IsAwakeningStat((int)(n16SubData_ + 1)))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("662"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int needAwakeningItemNum = this.GetNeedAwakeningItemNum((int)(n16SubData_ + 1), soldierInfo.GetSeason());
		if (num < needAwakeningItemNum)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("683"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_SOLAWAKENING_STAT_REQ gS_SOLAWAKENING_STAT_REQ = new GS_SOLAWAKENING_STAT_REQ();
		gS_SOLAWAKENING_STAT_REQ.i64SolID = soldierInfo.GetSolID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLAWAKENING_STAT_REQ, gS_SOLAWAKENING_STAT_REQ);
		this.SetCoolTime();
		if (null != this.m_gbWakeup_Start)
		{
			this.m_gbWakeup_Start.SetActive(true);
		}
		if (null != this.m_gbWakeup_Apply)
		{
			this.m_gbWakeup_Apply.SetActive(false);
		}
		if (null != this.m_gbWakeup_Change_1)
		{
			this.m_gbWakeup_Change_1.SetActive(false);
		}
		if (null != this.m_gbWakeup_Change_2)
		{
			this.m_gbWakeup_Change_2.SetActive(false);
		}
		if (null != this.m_gbWakeup_Change_3)
		{
			this.m_gbWakeup_Change_3.SetActive(false);
		}
		this.m_bEffectShow = true;
		this.m_lbStatRingSlot.Visible = false;
	}

	public void SelelctSoldier(ref NkSoldierInfo pkSolinfo)
	{
		this.InitInfo();
		for (int i = 0; i < 4; i++)
		{
			this.m_lbSolStat_1[i].Hide(false);
		}
		NkSoldierInfo nkSoldierInfo = pkSolinfo;
		this.m_SolID = pkSolinfo.GetSolID();
		this.m_lbGuideText_1.Hide(true);
		this.m_lbGuideText_2.Hide(true);
		this.m_dtInvenSlotBG.Visible = true;
		this.m_lbCurRingSlot.Visible = true;
		if (nkSoldierInfo.IsAtbCommonFlag(2L))
		{
			this.m_dtRingSlot.Visible = false;
			this.m_lbCurRingSlot.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2547"));
		}
		else
		{
			this.m_dtRingSlot.Visible = true;
			this.m_lbCurRingSlot.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2548"));
		}
		this.ShowSolInfo(false);
		if (null != this.m_gbWakeup_Start)
		{
			this.m_gbWakeup_Start.SetActive(false);
		}
		if (null != this.m_gbWakeup_Apply)
		{
			this.m_gbWakeup_Apply.SetActive(false);
		}
		if (null != this.m_gbWakeup_Change_1)
		{
			this.m_gbWakeup_Change_1.SetActive(false);
		}
		if (null != this.m_gbWakeup_Change_2)
		{
			this.m_gbWakeup_Change_2.SetActive(false);
		}
		if (null != this.m_gbWakeup_Change_3)
		{
			this.m_gbWakeup_Change_3.SetActive(false);
		}
	}

	public void ShowSolInfo(bool bStatCalc)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo == null)
		{
			return;
		}
		if (soldierInfo.GetCharKindInfo() == null)
		{
			return;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("290"),
			"targetname",
			soldierInfo.GetName(),
			"count1",
			soldierInfo.GetLevel(),
			"count2",
			soldierInfo.GetSolMaxLevel()
		});
		this.m_lbSolName.SetText(this.m_strText);
		this.m_dtSolImage.SetUVMask(new Rect(4f / this.TEX_SIZE, 0f, 504f / this.TEX_SIZE, 448f / this.TEX_SIZE));
		string costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(soldierInfo);
		this.m_dtSolImage.SetTexture(eCharImageType.LARGE, soldierInfo.GetCharKind(), (int)soldierInfo.GetGrade(), costumePortraitPath);
		UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(soldierInfo.GetCharKind(), (int)soldierInfo.GetGrade());
		if (solLargeGradeImg != null)
		{
			this.m_dtSolGrade.Hide(false);
			if (0 < NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(soldierInfo.GetCharKind(), (int)soldierInfo.GetGrade()))
			{
				this.m_dtSolGrade.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
			}
			this.m_dtSolGrade.SetTexture(solLargeGradeImg);
		}
		else
		{
			this.m_dtSolGrade.Hide(true);
		}
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_STRDEX);
		SUBDATA_UNION sUBDATA_UNION2 = default(SUBDATA_UNION);
		sUBDATA_UNION2.nSubData = soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_VITINT);
		SUBDATA_UNION sUBDATA_UNION3 = default(SUBDATA_UNION);
		sUBDATA_UNION3.nSubData = soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_INFO);
		short n16SubData_ = sUBDATA_UNION3.n16SubData_0;
		int statSTR = soldierInfo.GetStatSTR();
		int statDEX = soldierInfo.GetStatDEX();
		int statVIT = soldierInfo.GetStatVIT();
		int statINT = soldierInfo.GetStatINT();
		this.ShowBaseSolStatAwakening(this.m_lbBaseStat[0], statSTR, sUBDATA_UNION.n32SubData_0);
		this.ShowBaseSolStatAwakening(this.m_lbBaseStat[1], statDEX, sUBDATA_UNION.n32SubData_1);
		this.ShowBaseSolStatAwakening(this.m_lbBaseStat[2], statVIT, sUBDATA_UNION2.n32SubData_0);
		this.ShowBaseSolStatAwakening(this.m_lbBaseStat[3], statINT, sUBDATA_UNION2.n32SubData_1);
		if (this.IsAwakening())
		{
			for (int i = 0; i < 4; i++)
			{
				this.m_lbSolStat_2[i].Hide(false);
			}
			this.ShowBaseSolStat(this.m_lbCurStat[0], statSTR, sUBDATA_UNION.n32SubData_0, this.m_strBaseTextColor);
			this.ShowBaseSolStat(this.m_lbCurStat[1], statDEX, sUBDATA_UNION.n32SubData_1, this.m_strBaseTextColor);
			this.ShowBaseSolStat(this.m_lbCurStat[2], statVIT, sUBDATA_UNION2.n32SubData_0, this.m_strBaseTextColor);
			this.ShowBaseSolStat(this.m_lbCurStat[3], statINT, sUBDATA_UNION2.n32SubData_1, this.m_strBaseTextColor);
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				this.m_lbSolStat_2[i].Hide(true);
				this.m_lbCurStat[i].SetText(string.Empty);
			}
		}
		if (bStatCalc)
		{
			for (int i = 0; i < 4; i++)
			{
				this.m_lbSolStat_2[i].Hide(true);
				this.m_lbCurStat[i].Hide(true);
			}
		}
		int needAwakeningItemNum = this.GetNeedAwakeningItemNum((int)(n16SubData_ + 1), soldierInfo.GetSeason());
		this.m_lbNeedAwakeningItemNum.SetText(needAwakeningItemNum.ToString());
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strStat, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2289"),
			"count",
			(int)(n16SubData_ + 1)
		});
		this.m_btStatCalc.SetText(this.m_strStat);
	}

	public override void Update()
	{
		this.ShowTextAni();
		if (0f < this.m_fCoolTime && this.m_fCoolTime <= Time.time)
		{
			this.m_fCoolTime = 0f;
			this.SetControlEnable(true);
			this.ShowTextChangeEffect();
		}
		if (this.m_iAwakeningItemNum != NkUserInventory.GetInstance().Get_First_ItemCnt(this.m_iAwakeningItemUnique))
		{
			this.m_iAwakeningItemNum = NkUserInventory.GetInstance().Get_First_ItemCnt(this.m_iAwakeningItemUnique);
			this.m_lbMyAwakeningItemNum.SetText(ANNUALIZED.Convert(this.m_iAwakeningItemNum));
		}
	}

	public void SetCoolTime()
	{
		this.m_fCoolTime = Time.time + this.COOL_TIME;
		this.SetControlEnable(false);
		if (null != this.m_gbWakeup_Glow)
		{
			this.m_gbWakeup_Glow.SetActive(true);
			Animation componentInChildren = this.m_gbWakeup_Glow.GetComponentInChildren<Animation>();
			if (componentInChildren != null)
			{
				componentInChildren.Play();
			}
		}
	}

	public void SetControlEnable(bool bEnable)
	{
		this.m_btSol_1.controlIsEnabled = bEnable;
		this.m_btSol_2.controlIsEnabled = bEnable;
		this.m_btStatSelect.controlIsEnabled = bEnable;
		this.m_btStatCalc.controlIsEnabled = bEnable;
	}

	public void SetAwakeningInfo(GS_SOLAWAKENING_INFO_ACK ACK, NkDeserializePacket kDeserializePacket)
	{
		this.m_AwakeningTryList.Clear();
		for (int i = 0; i < (int)ACK.i16TryInfoCount; i++)
		{
			AWAKENING_TRY_INFO packet = kDeserializePacket.GetPacket<AWAKENING_TRY_INFO>();
			this.m_AwakeningTryList.Add(packet);
		}
	}

	public bool IsAwakeningStat(int iTryCount)
	{
		for (int i = 0; i < this.m_AwakeningTryList.Count; i++)
		{
			if ((int)this.m_AwakeningTryList[i].i16TryCount == iTryCount)
			{
				return true;
			}
		}
		return false;
	}

	public int GetNeedAwakeningItemNum(int iTryCount, int iSolSeason)
	{
		if (0 > iSolSeason || 10 <= iSolSeason)
		{
			return 0;
		}
		for (int i = 0; i < this.m_AwakeningTryList.Count; i++)
		{
			if ((int)this.m_AwakeningTryList[i].i16TryCount == iTryCount)
			{
				return this.m_AwakeningTryList[i].i32NeedNum[iSolSeason];
			}
		}
		return 0;
	}

	public AWAKENING_TRY_INFO GetAwakeningTryInfo(int iTryCount, int iSolSeason)
	{
		if (0 > iSolSeason || 10 <= iSolSeason)
		{
			return null;
		}
		for (int i = 0; i < this.m_AwakeningTryList.Count; i++)
		{
			if ((int)this.m_AwakeningTryList[i].i16TryCount == iTryCount)
			{
				return this.m_AwakeningTryList[i];
			}
		}
		return null;
	}

	public void SetAwakeningStat(GS_SOLAWAKENING_STAT_ACK ACK)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo != null)
		{
			int statSTR = soldierInfo.GetStatSTR();
			int statDEX = soldierInfo.GetStatDEX();
			int statVIT = soldierInfo.GetStatVIT();
			int statINT = soldierInfo.GetStatINT();
			this.ShowBaseSolStat(this.m_lbCurStat[0], statSTR, this.m_iAwakeningStat[0], this.m_strBaseTextColor);
			this.ShowBaseSolStat(this.m_lbCurStat[1], statDEX, this.m_iAwakeningStat[1], this.m_strBaseTextColor);
			this.ShowBaseSolStat(this.m_lbCurStat[2], statVIT, this.m_iAwakeningStat[2], this.m_strBaseTextColor);
			this.ShowBaseSolStat(this.m_lbCurStat[3], statINT, this.m_iAwakeningStat[3], this.m_strBaseTextColor);
		}
		int i;
		for (i = 0; i < 4; i++)
		{
			this.m_iAwakeningStat[i] = ACK.i32AwakeningStat[i];
			this.m_lbIncStat[i].SetText(string.Empty);
			this.m_fTextAniTime[i] = Time.time + (float)(i + 1) * this.TEXT_AIN_TIME;
		}
		this.m_fTextAniTimeRingSlot = Time.time + (float)(i + 1) * this.TEXT_AIN_TIME;
		this.m_bRingSlotOpen = ACK.bRingSlotOpen;
		this.ShowSolInfo(true);
	}

	public void SetAwakening()
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo != null)
		{
			SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
			sUBDATA_UNION.nSubData = soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_STRDEX);
			SUBDATA_UNION sUBDATA_UNION2 = default(SUBDATA_UNION);
			sUBDATA_UNION2.nSubData = soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_VITINT);
			int statSTR = soldierInfo.GetStatSTR();
			int statDEX = soldierInfo.GetStatDEX();
			int statVIT = soldierInfo.GetStatVIT();
			int statINT = soldierInfo.GetStatINT();
			this.ShowBaseSolStat(this.m_lbCurStat[0], statSTR, sUBDATA_UNION.n32SubData_0, this.m_strBaseTextColor);
			this.ShowBaseSolStat(this.m_lbCurStat[1], statDEX, sUBDATA_UNION.n32SubData_1, this.m_strBaseTextColor);
			this.ShowBaseSolStat(this.m_lbCurStat[2], statVIT, sUBDATA_UNION2.n32SubData_0, this.m_strBaseTextColor);
			this.ShowBaseSolStat(this.m_lbCurStat[3], statINT, sUBDATA_UNION2.n32SubData_1, this.m_strBaseTextColor);
			if (soldierInfo.IsAtbCommonFlag(2L))
			{
				this.m_dtRingSlot.Visible = false;
				this.m_lbCurRingSlot.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2547"));
			}
			else
			{
				this.m_dtRingSlot.Visible = true;
				this.m_lbCurRingSlot.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2548"));
			}
		}
		for (int i = 0; i < 4; i++)
		{
			this.m_iAwakeningStat[i] = 0;
			this.m_lbIncStat[i].SetText(string.Empty);
		}
		this.ShowSolInfo(false);
	}

	public bool IsAwakening()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.m_iAwakeningStat[i] != 0)
			{
				return true;
			}
		}
		return false;
	}

	public void ShowBaseSolStat(Label lbStat, int iStat, int iAwakeningStat, string strColorText)
	{
		if (iAwakeningStat != 0)
		{
			if (0 < iAwakeningStat)
			{
				iStat -= iAwakeningStat;
			}
			else
			{
				iStat += Math.Abs(iAwakeningStat);
			}
		}
		this.m_strText = string.Format(" {0}{1}", strColorText, iStat);
		lbStat.SetText(this.m_strText);
	}

	public void ShowBaseSolStatAwakening(Label lbStat, int iStat, int iAwakeningStat)
	{
		if (iAwakeningStat == 0)
		{
			this.m_strText = string.Format(" {0}{1}", this.m_strBaseTextColor, iStat);
		}
		else if (0 < iAwakeningStat)
		{
			iStat -= iAwakeningStat;
			this.m_strText = string.Format(" {0}{1}{2}(+{3})", new object[]
			{
				this.m_strBaseTextColor,
				iStat,
				this.m_strIncTextColor,
				iAwakeningStat
			});
		}
		else
		{
			iStat += Math.Abs(iAwakeningStat);
			this.m_strText = string.Format(" {0}{1}{2}({3})", new object[]
			{
				this.m_strBaseTextColor,
				iStat,
				this.m_strIncTextColor,
				iAwakeningStat
			});
		}
		lbStat.SetText(this.m_strText);
	}

	public void ClickClose(IUIObject obj)
	{
		if (this.IsAwakening())
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOK), null, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1795"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("208"), eMsgType.MB_OK_CANCEL);
			}
			return;
		}
		base.CloseNow();
	}

	private void MsgBoxOK(object obj)
	{
		base.CloseNow();
	}

	public void ShowTextAni()
	{
		for (int i = 0; i < 4; i++)
		{
			if (0f < this.m_fTextAniTime[i])
			{
				if (this.m_fTextAniTime[i] <= Time.time)
				{
					this.m_fTextAniTime[i] = 0f;
					this.m_lbSolStat_2[i].Hide(false);
					this.m_lbCurStat[i].Hide(false);
					if (this.m_iAwakeningStat[i] == 0)
					{
						this.m_strText = string.Format("{0}{1}", this.m_strBaseTextColor, this.m_strBaseText[i]);
						this.m_strStat = this.m_strBaseTextColor;
					}
					else
					{
						this.m_strText = string.Format("{0}{1}", this.m_strIncTextColor, this.m_strBaseText[i]);
						this.m_strStat = this.m_strIncTextColor;
					}
					this.m_lbSolStat_2[i].SetText(this.m_strText);
					this.ShowBaseSolStat(this.m_lbCurStat[i], this.GetBaseStat(i), this.GetAwakeningStat(i), this.m_strStat);
					if (this.m_iAwakeningStat[i] == 0)
					{
						this.m_strText = string.Empty;
					}
					else if (0 < this.m_iAwakeningStat[i])
					{
						this.m_strText = string.Format("+{0}", this.m_iAwakeningStat[i]);
					}
					else
					{
						this.m_strText = this.m_iAwakeningStat[i].ToString();
					}
					this.m_lbIncStat[i].SetText(this.m_strText);
					this.ShowTextEffect(this.m_gbWakeup_Text, this.m_dtTextBG[i]);
				}
			}
		}
		if (this.m_fTextAniTimeRingSlot > 0f && this.m_fTextAniTimeRingSlot <= Time.time)
		{
			this.m_fTextAniTimeRingSlot = 0f;
			this.ShowTextEffect(this.m_gbWakeup_Text, this.m_dtTextBGRingSlot);
			this.m_lbStatRingSlot.Visible = true;
			if (this.m_bRingSlotOpen)
			{
				this.m_lbStatRingSlot.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2584"));
			}
			else
			{
				this.m_lbStatRingSlot.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2585"));
			}
			if (0f < this.m_fCoolTime)
			{
				this.m_fCoolTime = Time.time + this.TEXT_AIN_TIME;
			}
		}
	}

	public int GetBaseStat(int iStat)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo == null)
		{
			return 0;
		}
		int result = 0;
		switch (iStat)
		{
		case 0:
			result = soldierInfo.GetStatSTR();
			break;
		case 1:
			result = soldierInfo.GetStatDEX();
			break;
		case 2:
			result = soldierInfo.GetStatVIT();
			break;
		case 3:
			result = soldierInfo.GetStatINT();
			break;
		}
		return result;
	}

	public int GetAwakeningStat(int iStat)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo == null)
		{
			return 0;
		}
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_STRDEX);
		SUBDATA_UNION sUBDATA_UNION2 = default(SUBDATA_UNION);
		sUBDATA_UNION2.nSubData = soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_VITINT);
		int result = 0;
		switch (iStat)
		{
		case 0:
			result = sUBDATA_UNION.n32SubData_0;
			break;
		case 1:
			result = sUBDATA_UNION.n32SubData_1;
			break;
		case 2:
			result = sUBDATA_UNION2.n32SubData_0;
			break;
		case 3:
			result = sUBDATA_UNION2.n32SubData_1;
			break;
		}
		return result;
	}

	public void LoadEffect()
	{
		this.m_strText = string.Format("{0}{1}", "UI/Soldier/fx_wakeup_glow", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(this.m_strText + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Wakeup_Glow), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		this.m_strText = string.Format("{0}{1}", "UI/Soldier/fx_wakeup_start", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		wWWItem = Holder.TryGetOrCreateBundle(this.m_strText + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Wakeup_Start), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		this.m_strText = string.Format("{0}{1}", "UI/Soldier/fx_wakeup_apply", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		wWWItem = Holder.TryGetOrCreateBundle(this.m_strText + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Wakeup_Apply), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		this.m_strText = string.Format("{0}{1}", "UI/Soldier/fx_wakeup_text", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		wWWItem = Holder.TryGetOrCreateBundle(this.m_strText + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Wakeup_Text), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		this.m_strText = string.Format("{0}{1}", "UI/Soldier/fx_wakeup_change", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		wWWItem = Holder.TryGetOrCreateBundle(this.m_strText + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Wakeup_Change), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void Effect_Wakeup_Glow(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbWakeup_Glow = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Glow);
					return;
				}
				Vector2 size = this.m_dtSolImage.GetSize();
				this.m_gbWakeup_Glow.transform.parent = this.m_dtSolImage.gameObject.transform;
				this.m_gbWakeup_Glow.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, this.m_dtSolImage.gameObject.transform.localPosition.z - 0.1f);
				NkUtil.SetAllChildLayer(this.m_gbWakeup_Glow, GUICamera.UILayer);
				this.m_gbWakeup_Glow.SetActive(false);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbWakeup_Glow);
				}
			}
		}
	}

	private void Effect_Wakeup_Start(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbWakeup_Start = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Start);
					return;
				}
				Vector2 size = this.m_dtSolImage.GetSize();
				this.m_gbWakeup_Start.transform.parent = this.m_dtSolImage.gameObject.transform;
				this.m_gbWakeup_Start.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, this.m_dtSolImage.gameObject.transform.localPosition.z - 0.1f);
				NkUtil.SetAllChildLayer(this.m_gbWakeup_Start, GUICamera.UILayer);
				this.m_gbWakeup_Start.SetActive(false);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbWakeup_Start);
				}
			}
		}
	}

	private void Effect_Wakeup_Apply(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbWakeup_Apply = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Apply);
					return;
				}
				Vector2 size = this.m_dtSolImage.GetSize();
				this.m_gbWakeup_Apply.transform.parent = this.m_dtSolImage.gameObject.transform;
				this.m_gbWakeup_Apply.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, this.m_dtSolImage.gameObject.transform.localPosition.z - 0.1f);
				NkUtil.SetAllChildLayer(this.m_gbWakeup_Apply, GUICamera.UILayer);
				this.m_gbWakeup_Apply.SetActive(false);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbWakeup_Apply);
				}
			}
		}
	}

	private void Effect_Wakeup_Text(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbWakeup_Text = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Text);
					return;
				}
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				this.m_gbWakeup_Text.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_gbWakeup_Text, GUICamera.UILayer);
				this.m_gbWakeup_Text.SetActive(false);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbWakeup_Text);
				}
			}
		}
	}

	private void Effect_Wakeup_Change(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbWakeup_Change_1 = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				this.m_gbWakeup_Change_2 = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				this.m_gbWakeup_Change_3 = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Change_1);
					UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Change_2);
					UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Change_3);
					return;
				}
				NkUtil.SetAllChildLayer(this.m_gbWakeup_Change_1, GUICamera.UILayer);
				NkUtil.SetAllChildLayer(this.m_gbWakeup_Change_2, GUICamera.UILayer);
				NkUtil.SetAllChildLayer(this.m_gbWakeup_Change_3, GUICamera.UILayer);
				this.m_gbWakeup_Change_1.SetActive(false);
				this.m_gbWakeup_Change_2.SetActive(false);
				this.m_gbWakeup_Change_3.SetActive(false);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbWakeup_Change_1);
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbWakeup_Change_2);
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbWakeup_Change_3);
				}
			}
		}
	}

	public override void Close()
	{
		base.Close();
		if (null != this.m_gbWakeup_Glow)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Glow);
		}
		if (null != this.m_gbWakeup_Start)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Start);
		}
		if (null != this.m_gbWakeup_Apply)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Apply);
		}
		if (null != this.m_gbWakeup_Text)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Text);
		}
		if (null != this.m_gbWakeup_Change_1)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Change_1);
		}
		if (null != this.m_gbWakeup_Change_2)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Change_2);
		}
		if (null != this.m_gbWakeup_Change_3)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbWakeup_Change_3);
		}
	}

	public void ShowTextEffect(GameObject gbEffect, AutoSpriteControlBase Control)
	{
		if (null != gbEffect)
		{
			Vector2 size = Control.GetSize();
			gbEffect.transform.parent = Control.gameObject.transform;
			gbEffect.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, Control.gameObject.transform.localPosition.z - 0.1f);
			gbEffect.SetActive(true);
			Animation componentInChildren = gbEffect.GetComponentInChildren<Animation>();
			if (componentInChildren != null)
			{
				componentInChildren.Stop();
				componentInChildren.Play();
			}
		}
	}

	public void ShowTextChangeEffect()
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (this.m_iAwakeningStat[i] != 0)
			{
				if (num == 0)
				{
					this.ShowTextEffect(this.m_gbWakeup_Change_1, this.m_dtTextBG[i]);
				}
				else
				{
					this.ShowTextEffect(this.m_gbWakeup_Change_2, this.m_dtTextBG[i]);
				}
				num++;
			}
		}
		if (this.m_bEffectShow)
		{
			if (this.m_bRingSlotOpen)
			{
				this.ShowTextEffect(this.m_gbWakeup_Change_3, this.m_dtTextBGRingSlot);
			}
			this.m_bEffectShow = false;
		}
	}

	public void ClickAwakeningReset(IUIObject obj)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("698"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_INFO);
		short n16SubData_ = sUBDATA_UNION.n16SubData_0;
		if (n16SubData_ <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("729"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			AWAKENING_TRY_INFO awakeningTryInfo = this.GetAwakeningTryInfo((int)n16SubData_, soldierInfo.GetSeason());
			if (awakeningTryInfo == null)
			{
				return;
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("222"),
				"itemname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(awakeningTryInfo.i32ResetItemUnique),
				"itemnum",
				awakeningTryInfo.i32ResetItemNum
			});
			msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxReset), null, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("221"), this.m_strText, eMsgType.MB_OK_CANCEL);
		}
	}

	private void MsgBoxReset(object obj)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("698"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!this.IsAwakeningReset())
		{
			return;
		}
		GS_SOLAWAKENING_RESET_REQ gS_SOLAWAKENING_RESET_REQ = new GS_SOLAWAKENING_RESET_REQ();
		gS_SOLAWAKENING_RESET_REQ.i64SolID = soldierInfo.GetSolID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLAWAKENING_RESET_REQ, gS_SOLAWAKENING_RESET_REQ);
		this.SetCoolTime();
		if (null != this.m_gbWakeup_Start)
		{
			this.m_gbWakeup_Start.SetActive(false);
		}
		if (null != this.m_gbWakeup_Apply)
		{
			this.m_gbWakeup_Apply.SetActive(true);
		}
		if (null != this.m_gbWakeup_Change_1)
		{
			this.m_gbWakeup_Change_1.SetActive(false);
		}
		if (null != this.m_gbWakeup_Change_2)
		{
			this.m_gbWakeup_Change_2.SetActive(false);
		}
		if (null != this.m_gbWakeup_Change_3)
		{
			this.m_gbWakeup_Change_3.SetActive(false);
		}
		this.m_bEffectShow = false;
		this.m_lbStatRingSlot.Visible = false;
	}

	public bool IsAwakeningReset()
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo == null)
		{
			Debug.LogError("m_SelectSol is null");
			return false;
		}
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_INFO);
		short n16SubData_ = sUBDATA_UNION.n16SubData_0;
		AWAKENING_TRY_INFO awakeningTryInfo = this.GetAwakeningTryInfo((int)n16SubData_, soldierInfo.GetSeason());
		if (awakeningTryInfo == null)
		{
			return false;
		}
		int num = NkUserInventory.GetInstance().Get_First_ItemCnt(awakeningTryInfo.i32ResetItemUnique);
		if (awakeningTryInfo.i32ResetItemNum > num)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("273"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		return true;
	}

	public void SetAwakeningReset(GS_SOLAWAKENING_RESET_ACK ACK)
	{
		for (int i = 0; i < 4; i++)
		{
			this.m_iAwakeningStat[i] = 0;
			this.m_lbIncStat[i].SetText(string.Empty);
		}
		this.ShowSolInfo(true);
	}

	private NkSoldierInfo GetSoldierInfo()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkSoldierInfo nkSoldierInfo = soldierList.GetSoldierInfoBySolID(this.m_SolID);
		if (nkSoldierInfo == null)
		{
			nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySoldierInfoBySolID(this.m_SolID);
		}
		return nkSoldierInfo;
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Soldier_Awakening.ToString());
		}
	}
}
