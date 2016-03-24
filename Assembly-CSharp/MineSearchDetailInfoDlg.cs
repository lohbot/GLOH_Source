using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class MineSearchDetailInfoDlg : Form
{
	private const int MAX_SOL_SLOT_NUM = 9;

	private DrawTexture m_dtBG;

	private Label m_lTitle;

	private Label m_laGuildName;

	private Label m_laMineCurNum;

	private Label m_laOccGuildMineNum;

	private Label m_laSearchMoney;

	private Label m_lGuildName;

	private Label m_laOccSelectMemberName;

	private Button m_bWaitGuildinfo;

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

	private DrawTexture m_dtMineIcon;

	private DrawTexture m_dtMineIcon2;

	private Button[] m_btOccMilitary = new Button[9];

	private ItemTexture[] m_itOccMilitary = new ItemTexture[9];

	private Button[] m_btOccMilitarySelect = new Button[9];

	private ItemTexture[] m_itOccMilitarySelect = new ItemTexture[9];

	private DrawTexture[] m_dOccPersonImage = new DrawTexture[9];

	private DrawTexture[] m_dOccSelectImage = new DrawTexture[9];

	private DrawTexture[] m_dtOccSolPosEffect = new DrawTexture[9];

	private Button[] m_btAttackMilitary = new Button[9];

	private ItemTexture[] m_iAttackMilitary = new ItemTexture[9];

	private Button[] m_btAttackMilitarySelect = new Button[9];

	private ItemTexture[] m_itAttackMilitarySelect = new ItemTexture[9];

	private DrawTexture[] m_dAttackPersonImage = new DrawTexture[9];

	private DrawTexture[] m_dAttackSelectImage = new DrawTexture[9];

	private DrawTexture[] m_dtAttackSolPosEffect = new DrawTexture[9];

	private Button m_btClose;

	private Button m_btResearch;

	private Button m_btGoMilitary;

	private Button m_btStart;

	private Button m_btClose01;

	private Button m_btClose03;

	private Button m_btStart02;

	private Button m_btChange01;

	private Button m_btCancel04;

	private Button m_btChange02;

	private Button m_btClose02;

	public int m_nSelectBatchIndex = -1;

	public int m_nCurrentBatchIndex = -1;

	public bool m_bChangeCheck;

	public bool m_bHaveEffectBundle;

	public bool m_bOriKeeping;

	public bool m_bOriPlunder;

	public bool m_Temp;

	public MINE_INFO m_mine_info = new MINE_INFO();

	private int m_old_select_index = -1;

	private int m_select_index = -1;

	public long m_GuildID;

	public string m_GuildName = string.Empty;

	public int nTempCurrentIndex = -1;

	public bool m_bHaveMilitary;

	public bool m_bLeadCheck;

	public eMineSearchDetailInfo_Mode m_eMode;

	private Dictionary<int, MINE_MILITARY_USER_SOLINFO> m_dicOccupy_User_SolList = new Dictionary<int, MINE_MILITARY_USER_SOLINFO>();

	private Dictionary<int, ECO> m_dicEcoGroupInfo = new Dictionary<int, ECO>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Mine/DLG_MineSearchDetailInfo", G_ID.MINE_SEARCH_DETAILINFO_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_lTitle = (base.GetControl("LB_Title") as Label);
		this.m_dtBG = (base.GetControl("DT_Image") as DrawTexture);
		this.m_laGuildName = (base.GetControl("LB_GuildName") as Label);
		this.m_laGuildName.SetText(string.Empty);
		this.m_laMineCurNum = (base.GetControl("LB_MinNum") as Label);
		this.m_laSearchMoney = (base.GetControl("LB_Investigation_Gold") as Label);
		this.m_laOccGuildMineNum = (base.GetControl("LB_BoxNum") as Label);
		this.m_laOccSelectMemberName = (base.GetControl("Label_Label64") as Label);
		this.m_laOccSelectMemberName.SetText(string.Empty);
		this.m_lGuildName = (base.GetControl("LB_GuildName") as Label);
		this.m_bWaitGuildinfo = (base.GetControl("Button_Button172") as Button);
		Button expr_EC = this.m_bWaitGuildinfo;
		expr_EC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_EC.Click, new EZValueChangedDelegate(this.WaitGuildinfodlg));
		this.m_btOriKeepingHelpIcon = (base.GetControl("BT_Help01") as Button);
		Button expr_129 = this.m_btOriKeepingHelpIcon;
		expr_129.Click = (EZValueChangedDelegate)Delegate.Combine(expr_129.Click, new EZValueChangedDelegate(this.OnClickOriKeepingHelpText));
		this.m_dtOriKeepingHelpBg = (base.GetControl("DT_HelpBg01") as DrawTexture);
		this.m_dtOriKeepingHelpBg.Visible = false;
		this.m_lOriKeepingHelpText = (base.GetControl("LB_HelpText01") as Label);
		this.m_lOriKeepingHelpText.Visible = false;
		this.m_dtOriKeepingHelpTail = (base.GetControl("DT_HelpTail01") as DrawTexture);
		this.m_dtOriKeepingHelpTail.Visible = false;
		this.m_btOriPlunderHelpIcon = (base.GetControl("BT_Help02") as Button);
		Button expr_1CC = this.m_btOriPlunderHelpIcon;
		expr_1CC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1CC.Click, new EZValueChangedDelegate(this.OnClickOriPlunderHelpText));
		this.m_dtOriPlunderHelpBg = (base.GetControl("DT_HelpBg02") as DrawTexture);
		this.m_dtOriPlunderHelpBg.Visible = false;
		this.m_lOriPlunderHelpText = (base.GetControl("LB_HelpText02") as Label);
		this.m_lOriPlunderHelpText.Visible = false;
		this.m_dtOriPlunderHelpTail = (base.GetControl("DT_HelpTail02") as DrawTexture);
		this.m_dtOriPlunderHelpTail.Visible = false;
		this.m_dtMineIcon = (base.GetControl("DT_MineIcon") as DrawTexture);
		this.m_dtMineIcon2 = (base.GetControl("DT_MineIcon2") as DrawTexture);
		for (int i = 0; i < 9; i++)
		{
			this.m_itOccSol[i] = (base.GetControl(string.Format("IT_SolInfo0{0}", i + 1)) as ItemTexture);
			this.m_dtOccSolBG[i] = (base.GetControl(string.Format("DT_SolInfoBG0{0}", i + 1)) as DrawTexture);
		}
		for (int j = 0; j < 9; j++)
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
			this.m_btAttackMilitary[j] = (base.GetControl(string.Format("Btn_R_Sol0{0}", j + 1)) as Button);
			this.m_btAttackMilitary[j].EffectAni = false;
			this.m_iAttackMilitary[j] = (base.GetControl(string.Format("IT_R_SolImage0{0}", j + 1)) as ItemTexture);
			this.m_btAttackMilitarySelect[j] = (base.GetControl(string.Format("Btn_R_SelectSol0{0}", j + 1)) as Button);
			this.m_btAttackMilitarySelect[j].EffectAni = false;
			this.m_btAttackMilitarySelect[j].Visible = false;
			this.m_itAttackMilitarySelect[j] = (base.GetControl(string.Format("IT_R_SelectSolImage0{0}", j + 1)) as ItemTexture);
			this.m_itAttackMilitarySelect[j].Visible = false;
			this.m_dAttackPersonImage[j] = (base.GetControl(string.Format("DT_R_PersonImage0{0}", j + 1)) as DrawTexture);
			this.m_dAttackPersonImage[j].Visible = false;
			this.m_dAttackSelectImage[j] = (base.GetControl(string.Format("DT_R_SelectImage0{0}", j + 1)) as DrawTexture);
			this.m_dAttackSelectImage[j].Visible = false;
			this.m_dtAttackSolPosEffect[j] = (base.GetControl(string.Format("DT_R_SelectSol0{0}", j + 1)) as DrawTexture);
			this.m_dtAttackSolPosEffect[j].Visible = false;
		}
		this.m_btClose = (base.GetControl("Btn_Close") as Button);
		Button expr_5D5 = this.m_btClose;
		expr_5D5.Click = (EZValueChangedDelegate)Delegate.Combine(expr_5D5.Click, new EZValueChangedDelegate(this.OnBtnClickClose));
		this.m_btGoMilitary = (base.GetControl("Btn_Start") as Button);
		Button expr_612 = this.m_btGoMilitary;
		expr_612.Click = (EZValueChangedDelegate)Delegate.Combine(expr_612.Click, new EZValueChangedDelegate(this.OnBtnClickGoMilitary));
		this.m_btResearch = (base.GetControl("Btn_Return") as Button);
		Button expr_64F = this.m_btResearch;
		expr_64F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_64F.Click, new EZValueChangedDelegate(this.OnBtnClickResearch));
		this.m_btStart = (base.GetControl("Btn_Start01") as Button);
		Button expr_68C = this.m_btStart;
		expr_68C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_68C.Click, new EZValueChangedDelegate(this.OnBtnClickStart));
		this.m_btClose01 = (base.GetControl("Btn_Close01") as Button);
		Button expr_6C9 = this.m_btClose01;
		expr_6C9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_6C9.Click, new EZValueChangedDelegate(this.OnBtnClickClose));
		this.m_btChange01 = (base.GetControl("Btn_Change01") as Button);
		Button expr_706 = this.m_btChange01;
		expr_706.Click = (EZValueChangedDelegate)Delegate.Combine(expr_706.Click, new EZValueChangedDelegate(this.OnClickMilitaryPosChange));
		this.m_btClose03 = (base.GetControl("Btn_Close03") as Button);
		Button expr_743 = this.m_btClose03;
		expr_743.Click = (EZValueChangedDelegate)Delegate.Combine(expr_743.Click, new EZValueChangedDelegate(this.OnBtnClickClose));
		this.m_btStart02 = (base.GetControl("Btn_Start02") as Button);
		this.m_btStart02.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615");
		Button expr_79A = this.m_btStart02;
		expr_79A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_79A.Click, new EZValueChangedDelegate(this.OnBtnClickStart));
		this.m_btChange02 = (base.GetControl("Btn_Change02") as Button);
		Button expr_7D7 = this.m_btChange02;
		expr_7D7.Click = (EZValueChangedDelegate)Delegate.Combine(expr_7D7.Click, new EZValueChangedDelegate(this.OnClickChangeMessageBox));
		this.m_btCancel04 = (base.GetControl("Btn_Close04") as Button);
		Button expr_814 = this.m_btCancel04;
		expr_814.Click = (EZValueChangedDelegate)Delegate.Combine(expr_814.Click, new EZValueChangedDelegate(this.OnClickCancelMessageBox));
		this.m_btClose02 = (base.GetControl("Btn_Close02") as Button);
		Button expr_851 = this.m_btClose02;
		expr_851.Click = (EZValueChangedDelegate)Delegate.Combine(expr_851.Click, new EZValueChangedDelegate(this.OnBtnClickClose));
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
		this.m_old_select_index = -1;
		this.nTempCurrentIndex = -1;
		this.m_itOccMilitarySelect[nIndex].Visible = false;
		this.m_itOccMilitary[nIndex].Visible = false;
		this.m_dOccPersonImage[nIndex].Visible = false;
		this.m_dOccSelectImage[nIndex].Visible = false;
		this.m_dtOccSolPosEffect[nIndex].Visible = false;
		this.m_itAttackMilitarySelect[nIndex].Visible = false;
		this.m_iAttackMilitary[nIndex].Visible = false;
		this.m_dAttackPersonImage[nIndex].Visible = false;
		this.m_dAttackSelectImage[nIndex].Visible = false;
		this.m_dtAttackSolPosEffect[nIndex].Visible = false;
	}

	public void InitInterface()
	{
		this.m_old_select_index = -1;
		for (int i = 0; i < 9; i++)
		{
			this.m_btOccMilitarySelect[i].Visible = false;
			this.m_itOccMilitarySelect[i].Visible = false;
			this.m_itOccMilitary[i].Visible = false;
			this.m_dOccPersonImage[i].Visible = false;
			this.m_dOccSelectImage[i].Visible = false;
			this.m_dtOccSolPosEffect[i].Visible = false;
			this.m_btAttackMilitarySelect[i].Visible = false;
			this.m_itAttackMilitarySelect[i].Visible = false;
			this.m_iAttackMilitary[i].Visible = false;
			this.m_dAttackPersonImage[i].Visible = false;
			this.m_dAttackSelectImage[i].Visible = false;
			this.m_dtAttackSolPosEffect[i].Visible = false;
			Button expr_BE = this.m_btAttackMilitary[i];
			expr_BE.Click = (EZValueChangedDelegate)Delegate.Remove(expr_BE.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
			Button expr_E7 = this.m_btOccMilitary[i];
			expr_E7.Click = (EZValueChangedDelegate)Delegate.Remove(expr_E7.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
			Button expr_110 = this.m_btOccMilitary[i];
			expr_110.Click = (EZValueChangedDelegate)Delegate.Remove(expr_110.Click, new EZValueChangedDelegate(this.ClickEcoMonDetailInfo));
		}
	}

	public void InitEditMode(bool bEditMode)
	{
		if (bEditMode)
		{
			base.SetShowLayer(6, false);
			base.SetShowLayer(7, true);
		}
		else
		{
			base.SetShowLayer(7, false);
			base.SetShowLayer(6, true);
			this.m_btChange01.EffectAni = false;
		}
		string path = string.Format("{0}", "UI/Mine/fx_mineface_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		this.LeadEffect(bEditMode);
		bool flag = false;
		bool flag2 = false;
		if (this.m_eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK)
		{
			flag = true;
		}
		if (this.m_eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE)
		{
			flag2 = true;
		}
		for (int i = 0; i < 9; i++)
		{
			Button expr_8C = this.m_btAttackMilitary[i];
			expr_8C.Click = (EZValueChangedDelegate)Delegate.Remove(expr_8C.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
			this.m_btAttackMilitary[i].data = i;
			Button expr_C8 = this.m_btAttackMilitary[i];
			expr_C8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_C8.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
			if (flag)
			{
				this.m_btAttackMilitary[i].Visible = true;
				if (bEditMode)
				{
					NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_btAttackMilitary[i], this.m_btAttackMilitary[i].GetSize());
					this.m_btAttackMilitary[i].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddEffectDelegate));
				}
				else
				{
					Transform transform = this.m_btAttackMilitary[i].gameObject.transform.FindChild("child_effect");
					if (transform != null)
					{
						UnityEngine.Object.Destroy(transform.gameObject);
					}
				}
			}
			Button expr_180 = this.m_btOccMilitary[i];
			expr_180.Click = (EZValueChangedDelegate)Delegate.Remove(expr_180.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
			this.m_btOccMilitary[i].data = i;
			Button expr_1BC = this.m_btOccMilitary[i];
			expr_1BC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1BC.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
			if (flag2)
			{
				this.m_btOccMilitary[i].Visible = true;
				if (bEditMode)
				{
					NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_btOccMilitary[i], this.m_btOccMilitary[i].GetSize());
					this.m_btOccMilitary[i].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddEffectDelegate));
				}
				else
				{
					Transform transform2 = this.m_btOccMilitary[i].gameObject.transform.FindChild("child_effect");
					if (transform2 != null)
					{
						UnityEngine.Object.Destroy(transform2.gameObject);
					}
				}
			}
		}
	}

	public void SetMineIninfo(MINE_INFO info, long guildid, string szguildname, MINE_MILITARY_USER_SOLINFO[] occupy_info, eMineSearchDetailInfo_Mode eMode)
	{
		if (eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_DEFAULT)
		{
			return;
		}
		this.m_eMode = eMode;
		switch (eMode)
		{
		case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_SEARCH:
			base.ShowLayer(1, 4);
			break;
		case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE:
			this.m_bLeadCheck = this.IsLeaderCheck(occupy_info);
			if (this.m_bLeadCheck)
			{
				base.ShowLayer(1, 6);
				this.m_bHaveMilitary = true;
			}
			else
			{
				base.ShowLayer(1, 5);
				if (this.IsHaveMyMilitary(occupy_info))
				{
					this.m_btStart.Text = NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615");
					this.m_btStart.SetButtonTextureKey("Win_B_NewBtnRed");
					this.m_bHaveMilitary = true;
				}
				else
				{
					this.m_btStart.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1323");
					this.m_btStart.SetButtonTextureKey("Win_B_NewBtnBlue");
					this.m_bHaveMilitary = false;
				}
				this.m_btClose01.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1044");
			}
			break;
		case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_OTHERDEFENCE:
			base.ShowLayer(1, 9);
			this.m_btClose02.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1044");
			break;
		case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK:
			this.m_bLeadCheck = this.IsLeaderCheck(occupy_info);
			if (this.m_bLeadCheck)
			{
				base.ShowLayer(2, 6);
				this.m_bHaveMilitary = true;
			}
			else
			{
				base.ShowLayer(2, 5);
				if (this.IsHaveMyMilitary(occupy_info))
				{
					this.m_btStart.Text = NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615");
					this.m_btStart.SetButtonTextureKey("Win_B_NewBtnRed");
					this.m_bHaveMilitary = true;
				}
				else
				{
					this.m_btStart.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1323");
					this.m_btStart.SetButtonTextureKey("Win_B_NewBtnBlue");
					this.m_bHaveMilitary = false;
				}
				this.m_btClose01.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1044");
			}
			break;
		case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_OTHERATTACK:
			base.ShowLayer(2, 9);
			this.m_btClose02.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1044");
			break;
		}
		this.InitInterface();
		this.m_mine_info.Init();
		this.m_mine_info.Set(info);
		this.SetMineOccupySolList(guildid, szguildname, occupy_info);
		this.m_lGuildName.Text = szguildname;
		this.ShowMineInfo(eMode);
		MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(info.i16MineDataID);
		if (mineCreateDataFromID != null)
		{
			this.m_dtMineIcon.SetTexture(mineCreateDataFromID.Mine_MiniIcon);
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(mineCreateDataFromID.MINE_ITEM_UNIQUE);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1944"),
				"targetname",
				itemNameByItemUnique
			});
			this.m_lOriKeepingHelpText.SetText(empty);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1945"),
				"targetname",
				itemNameByItemUnique
			});
			this.m_lOriPlunderHelpText.SetText(empty);
		}
	}

	public bool IsHaveMyMilitary(MINE_MILITARY_USER_SOLINFO[] occupy_info)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		long personID = charPersonInfo.GetPersonID();
		for (int i = 0; i < occupy_info.Length; i++)
		{
			if (occupy_info[i].i64PersonID == personID)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsLeaderCheck(MINE_MILITARY_USER_SOLINFO[] occupy_info)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		long personID = charPersonInfo.GetPersonID();
		for (int i = 0; i < occupy_info.Length; i++)
		{
			if (occupy_info[i].nLeaderMilitary > 0 && personID == occupy_info[i].i64PersonID)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsAddMilitary()
	{
		int num = 0;
		foreach (KeyValuePair<int, MINE_MILITARY_USER_SOLINFO> current in this.m_dicOccupy_User_SolList)
		{
			if (current.Value != null)
			{
				num++;
			}
		}
		return num < 9;
	}

	public void SetMineOccupySolList(long guildid, string szguildname, MINE_MILITARY_USER_SOLINFO[] occupy_info)
	{
		this.m_GuildID = guildid;
		this.m_GuildName = szguildname;
		this.m_dicOccupy_User_SolList.Clear();
		for (int i = 0; i < occupy_info.Length; i++)
		{
			this.m_dicOccupy_User_SolList.Add((int)occupy_info[i].ui8BatchIndex, occupy_info[i]);
		}
	}

	public void ShowMineInfo(eMineSearchDetailInfo_Mode eMode)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(this.m_mine_info.i16MineDataID);
		if (mineCreateDataFromID == null)
		{
			return;
		}
		MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(mineCreateDataFromID.GetGrade());
		if (mineDataFromGrade == null)
		{
			return;
		}
		if (eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_SEARCH)
		{
			DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
			if (directionDLG != null)
			{
				directionDLG.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_MINESEARCH, (int)mineCreateDataFromID.GetGrade());
			}
		}
		this.SetMine_SolInfo(eMode);
		string text3 = string.Empty;
		string str = string.Empty;
		if (eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE || eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_OTHERDEFENCE)
		{
			str = mineDataFromGrade.MINE_BG_NAME;
			text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1322");
		}
		else if (eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK || eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_OTHERATTACK)
		{
			str = mineDataFromGrade.MINE_BG1_NAME;
			text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1642");
		}
		else if (eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_SEARCH)
		{
			str = mineDataFromGrade.MINE_BG_NAME;
			text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1322");
		}
		this.m_lTitle.SetText(text3);
		this.m_dtMineIcon2.SetTexture(mineDataFromGrade.Mine_UI_Icon);
		this.m_dtBG.SetTextureFromBundle("UI/Mine/" + str);
		this.m_laMineCurNum.SetText(this.m_mine_info.i32LeftItemNum.ToString());
		this.m_laOccGuildMineNum.SetText(this.m_mine_info.i32PlunderItemNum.ToString());
		if (this.m_GuildID <= 0L)
		{
			this.m_laOccSelectMemberName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1332"));
			this.m_laGuildName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1325"));
		}
		else
		{
			if (eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK || eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_OTHERATTACK)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1949");
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1326");
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"targetname",
				this.m_GuildName
			});
			this.m_laGuildName.SetText(text2);
			if (100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel)
			{
				text2 = text2 + " " + this.m_mine_info.i64MineID.ToString();
			}
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1775");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"gold",
			mineDataFromGrade.MINE_SEARCH_MONEY
		});
		this.m_laSearchMoney.SetText(text2);
		this.Show();
	}

	public void SetMine_SolInfo(eMineSearchDetailInfo_Mode eMode)
	{
		if (this.m_GuildID <= 0L)
		{
			this.SetEcoMoninfo();
		}
		else
		{
			this.SetOccupySolInfo(eMode);
		}
	}

	public void SetEcoMoninfo()
	{
		this.m_dicEcoGroupInfo.Clear();
		MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(this.m_mine_info.i16MineDataID);
		if (mineCreateDataFromID == null)
		{
			return;
		}
		for (int i = 0; i < 9; i++)
		{
			this.SetEcoMoninfo(i, mineCreateDataFromID.MINE_ECO[i]);
		}
		this.SetEcoMonDetailinfo(0);
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
		if (eco_info != null)
		{
			for (int i = 0; i < 6; i++)
			{
				if ((eco_info.nBattlePos[i] >= 0 || eco_info.nBattlePos[i] < 9) && NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco_info.szCharCode[i]) > 0)
				{
					this.m_itOccSol[eco_info.nBattlePos[i]].SetSolImageTexure(eCharImageType.SMALL, NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco_info.szCharCode[i]), -1);
				}
			}
		}
	}

	public void InitOccSolInfo()
	{
		for (int i = 0; i < 9; i++)
		{
			this.m_itOccSol[i].ClearData();
			this.m_dtOccSolBG[i].SetTexture("Win_T_ItemEmpty");
		}
	}

	public int GetHighFightPowerSolArrayIndex(MINE_MILITARY_USER_SOLINFO user_solinfo)
	{
		int result = 0;
		long num = 0L;
		for (int i = 0; i < 5; i++)
		{
			if (user_solinfo.mine_solinfo[i].i32Kind > 0)
			{
				if (num <= 0L)
				{
					num = user_solinfo.mine_solinfo[i].nFightPower;
					result = i;
				}
				else if (user_solinfo.mine_solinfo[i].nFightPower > num)
				{
					num = user_solinfo.mine_solinfo[i].nFightPower;
					result = i;
				}
			}
		}
		return result;
	}

	public void SetOccupySolInfo(eMineSearchDetailInfo_Mode eMode)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		long personID = charPersonInfo.GetPersonID();
		foreach (KeyValuePair<int, MINE_MILITARY_USER_SOLINFO> current in this.m_dicOccupy_User_SolList)
		{
			MINE_MILITARY_USER_SOLINFO value = current.Value;
			int ui8BatchIndex = (int)value.ui8BatchIndex;
			NkListSolInfo listSolInfo = this.GetListSolInfo(value);
			if (eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK || eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_OTHERATTACK)
			{
				this.LeadEffect(false);
				this.m_iAttackMilitary[ui8BatchIndex].Visible = true;
				this.m_btAttackMilitary[ui8BatchIndex].Visible = true;
				if (eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK)
				{
					listSolInfo.ShowGrade = false;
				}
				this.m_iAttackMilitary[ui8BatchIndex].SetSolImageTexure(eCharImageType.SMALL, listSolInfo);
				this.m_btAttackMilitary[ui8BatchIndex].Data = ui8BatchIndex;
				Button expr_C0 = this.m_btAttackMilitary[ui8BatchIndex];
				expr_C0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_C0.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
			}
			else
			{
				this.LeadEffect(false);
				this.m_itOccMilitary[ui8BatchIndex].Visible = true;
				this.m_btOccMilitary[ui8BatchIndex].Visible = true;
				if (eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE)
				{
					listSolInfo.ShowGrade = false;
				}
				this.m_itOccMilitary[ui8BatchIndex].SetSolImageTexure(eCharImageType.SMALL, listSolInfo);
				this.m_btOccMilitary[ui8BatchIndex].Data = ui8BatchIndex;
				Button expr_149 = this.m_btOccMilitary[ui8BatchIndex];
				expr_149.Click = (EZValueChangedDelegate)Delegate.Combine(expr_149.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
			}
		}
		if (this.m_bLeadCheck)
		{
			for (int i = 0; i < 9; i++)
			{
				if (this.m_dicOccupy_User_SolList.ContainsKey(i))
				{
					if (this.m_dicOccupy_User_SolList[i].nLeaderMilitary != 0)
					{
						this.m_old_select_index = -1;
						this.SetOccupyDetailinfo(i);
					}
				}
			}
		}
		else
		{
			bool flag = false;
			for (int j = 0; j < 9; j++)
			{
				if (this.m_dicOccupy_User_SolList.ContainsKey(j))
				{
					if (this.m_dicOccupy_User_SolList[j].i64PersonID == personID)
					{
						this.SetOccupyDetailinfo(j);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				for (int k = 0; k < 9; k++)
				{
					if (this.m_dicOccupy_User_SolList.ContainsKey(k))
					{
						if (this.m_dicOccupy_User_SolList[k].nLeaderMilitary != 0)
						{
							this.SetOccupyDetailinfo(k);
						}
					}
				}
			}
		}
	}

	public void SetOccupyDetailinfo(int Index)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		long personID = charPersonInfo.GetPersonID();
		if (this.m_dicOccupy_User_SolList.ContainsKey(Index))
		{
			this.m_select_index = Index;
			if (this.m_eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK || this.m_eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_OTHERATTACK)
			{
				if (!this.m_bChangeCheck)
				{
					this.m_btAttackMilitary[this.m_select_index].Visible = false;
					this.m_iAttackMilitary[this.m_select_index].Visible = false;
					this.m_iAttackMilitary[this.m_select_index].SetText(string.Empty);
					if (personID == this.m_dicOccupy_User_SolList[this.m_select_index].i64PersonID)
					{
						this.m_dAttackSelectImage[this.m_select_index].Visible = true;
					}
					this.m_btAttackMilitarySelect[this.m_select_index].Visible = true;
					this.m_itAttackMilitarySelect[this.m_select_index].Visible = true;
					NkListSolInfo listSolInfo = this.GetListSolInfo(this.m_dicOccupy_User_SolList[this.m_select_index]);
					if (this.m_eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK)
					{
						listSolInfo.ShowGrade = false;
					}
					this.m_itAttackMilitarySelect[this.m_select_index].SetSolImageTexure(eCharImageType.SMALL, listSolInfo);
				}
				else
				{
					this.m_btAttackMilitary[this.m_select_index].Visible = true;
					this.m_iAttackMilitary[this.m_select_index].Visible = true;
					if (personID == this.m_dicOccupy_User_SolList[this.m_select_index].i64PersonID)
					{
						this.m_dAttackSelectImage[this.m_select_index].Visible = true;
					}
					this.m_btAttackMilitarySelect[this.m_select_index].Visible = false;
					this.m_itAttackMilitarySelect[this.m_select_index].Visible = false;
					NkListSolInfo listSolInfo2 = this.GetListSolInfo(this.m_dicOccupy_User_SolList[this.m_select_index]);
					if (this.m_eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK)
					{
						listSolInfo2.ShowGrade = false;
					}
					this.m_iAttackMilitary[this.m_select_index].SetSolImageTexure(eCharImageType.SMALL, listSolInfo2);
				}
				if (this.m_old_select_index >= 0)
				{
					if (this.m_dicOccupy_User_SolList.ContainsKey(this.m_old_select_index))
					{
						this.m_btAttackMilitary[this.m_old_select_index].Visible = true;
						this.m_iAttackMilitary[this.m_old_select_index].Visible = true;
						NkListSolInfo listSolInfo3 = this.GetListSolInfo(this.m_dicOccupy_User_SolList[this.m_old_select_index]);
						if (this.m_eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK)
						{
							listSolInfo3.ShowGrade = false;
						}
						this.m_iAttackMilitary[this.m_old_select_index].SetSolImageTexure(eCharImageType.SMALL, listSolInfo3);
					}
					if (personID == this.m_dicOccupy_User_SolList[this.m_old_select_index].i64PersonID)
					{
						this.m_dAttackSelectImage[this.m_old_select_index].Visible = false;
					}
					this.m_itAttackMilitarySelect[this.m_old_select_index].Visible = false;
					this.m_btAttackMilitarySelect[this.m_old_select_index].Visible = false;
					this.m_itAttackMilitarySelect[this.m_old_select_index].SetText(string.Empty);
				}
				else if (this.m_bChangeCheck)
				{
					if (this.m_dicOccupy_User_SolList.ContainsKey(this.m_select_index))
					{
						this.m_btAttackMilitary[this.m_select_index].Visible = true;
						this.m_iAttackMilitary[this.m_select_index].Visible = true;
						NkListSolInfo listSolInfo4 = this.GetListSolInfo(this.m_dicOccupy_User_SolList[this.m_select_index]);
						if (this.m_eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK)
						{
							listSolInfo4.ShowGrade = false;
						}
						this.m_iAttackMilitary[this.m_select_index].SetSolImageTexure(eCharImageType.SMALL, listSolInfo4);
					}
					if (personID == this.m_dicOccupy_User_SolList[this.m_select_index].i64PersonID)
					{
						this.m_dAttackSelectImage[this.m_select_index].Visible = false;
					}
					this.m_itAttackMilitarySelect[this.m_select_index].Visible = false;
					this.m_btAttackMilitarySelect[this.m_select_index].Visible = false;
					this.m_itAttackMilitarySelect[this.m_select_index].SetText(string.Empty);
				}
			}
			else
			{
				if (!this.m_bChangeCheck)
				{
					this.m_btOccMilitary[this.m_select_index].Visible = false;
					this.m_itOccMilitary[this.m_select_index].Visible = false;
					this.m_itOccMilitary[this.m_select_index].SetText(string.Empty);
					if (personID == this.m_dicOccupy_User_SolList[this.m_select_index].i64PersonID)
					{
						this.m_dOccSelectImage[this.m_select_index].Visible = true;
					}
					this.m_btOccMilitarySelect[this.m_select_index].Visible = true;
					this.m_itOccMilitarySelect[this.m_select_index].Visible = true;
					NkListSolInfo listSolInfo5 = this.GetListSolInfo(this.m_dicOccupy_User_SolList[this.m_select_index]);
					if (this.m_eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE)
					{
						listSolInfo5.ShowGrade = false;
					}
					this.m_itOccMilitarySelect[this.m_select_index].SetSolImageTexure(eCharImageType.SMALL, listSolInfo5);
				}
				else
				{
					this.m_btOccMilitary[this.m_select_index].Visible = true;
					this.m_itOccMilitary[this.m_select_index].Visible = true;
					if (personID == this.m_dicOccupy_User_SolList[this.m_select_index].i64PersonID)
					{
						this.m_dOccSelectImage[this.m_select_index].Visible = true;
					}
					this.m_btOccMilitarySelect[this.m_select_index].Visible = false;
					this.m_itOccMilitarySelect[this.m_select_index].Visible = false;
					NkListSolInfo listSolInfo6 = this.GetListSolInfo(this.m_dicOccupy_User_SolList[this.m_select_index]);
					if (this.m_eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK)
					{
						listSolInfo6.ShowGrade = false;
					}
					this.m_itOccMilitary[this.m_select_index].SetSolImageTexure(eCharImageType.SMALL, listSolInfo6);
				}
				if (this.m_old_select_index >= 0)
				{
					if (this.m_dicOccupy_User_SolList.ContainsKey(this.m_old_select_index))
					{
						this.m_btOccMilitary[this.m_old_select_index].Visible = true;
						this.m_itOccMilitary[this.m_old_select_index].Visible = true;
						NkListSolInfo listSolInfo7 = this.GetListSolInfo(this.m_dicOccupy_User_SolList[this.m_old_select_index]);
						if (this.m_eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE)
						{
							listSolInfo7.ShowGrade = false;
						}
						this.m_itOccMilitary[this.m_old_select_index].SetSolImageTexure(eCharImageType.SMALL, listSolInfo7);
					}
					if (personID == this.m_dicOccupy_User_SolList[this.m_old_select_index].i64PersonID)
					{
						this.m_dOccSelectImage[this.m_old_select_index].Visible = false;
					}
					this.m_itOccMilitarySelect[this.m_old_select_index].Visible = false;
					this.m_btOccMilitarySelect[this.m_old_select_index].Visible = false;
					this.m_itOccMilitarySelect[this.m_old_select_index].SetText(string.Empty);
				}
				else if (this.m_bChangeCheck)
				{
					if (this.m_dicOccupy_User_SolList.ContainsKey(this.m_select_index))
					{
						this.m_btOccMilitary[this.m_select_index].Visible = true;
						this.m_itOccMilitary[this.m_select_index].Visible = true;
						NkListSolInfo listSolInfo8 = this.GetListSolInfo(this.m_dicOccupy_User_SolList[this.m_select_index]);
						if (this.m_eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE)
						{
							listSolInfo8.ShowGrade = true;
						}
						this.m_itOccMilitary[this.m_select_index].SetSolImageTexure(eCharImageType.SMALL, listSolInfo8);
					}
					if (personID == this.m_dicOccupy_User_SolList[this.m_select_index].i64PersonID)
					{
						this.m_dOccSelectImage[this.m_select_index].Visible = false;
					}
					this.m_itOccMilitarySelect[this.m_select_index].Visible = false;
					this.m_btOccMilitarySelect[this.m_select_index].Visible = false;
					this.m_itOccMilitarySelect[this.m_select_index].SetText(string.Empty);
				}
			}
			this.m_old_select_index = this.m_select_index;
			if (this.m_dicOccupy_User_SolList != null)
			{
				this.SetOccupyDetailinfo(this.m_dicOccupy_User_SolList[Index]);
			}
		}
		else
		{
			this.InitInterface(Index);
		}
	}

	public NkListSolInfo GetListSolInfo(MINE_MILITARY_USER_SOLINFO solInfo)
	{
		int highFightPowerSolArrayIndex = this.GetHighFightPowerSolArrayIndex(solInfo);
		NkListSolInfo nkListSolInfo = new NkListSolInfo();
		nkListSolInfo.SolCharKind = solInfo.mine_solinfo[highFightPowerSolArrayIndex].i32Kind;
		nkListSolInfo.SolGrade = (int)solInfo.mine_solinfo[highFightPowerSolArrayIndex].ui8Grade;
		nkListSolInfo.SolLevel = -1;
		nkListSolInfo.FightPower = -1L;
		nkListSolInfo.ShowGrade = true;
		string costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(solInfo.mine_solinfo[highFightPowerSolArrayIndex].nCostumeUnique);
		nkListSolInfo.SolCostumePortraitPath = costumePortraitPath;
		if (this.m_eMode != eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE)
		{
			nkListSolInfo.ShowGrade = true;
		}
		return nkListSolInfo;
	}

	public void SetOccupyDetailinfo(MINE_MILITARY_USER_SOLINFO info)
	{
		this.InitOccSolInfo();
		string empty = string.Empty;
		string str = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1639"),
			"count",
			info.i16CharLevel.ToString(),
			"targetname",
			TKString.NEWString(info.szCharname)
		});
		bool flag = false;
		for (int i = 0; i < 8; i++)
		{
			if (info.nMilitaryState == 1)
			{
				str = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1525");
				flag = true;
				break;
			}
			if (info.nMilitaryState == 2)
			{
				str = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1526");
				flag = true;
				break;
			}
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
			for (int j = 0; j < 5; j++)
			{
				if (info.mine_solinfo[j].ui8BatchPos >= 0)
				{
					if (info.mine_solinfo[j].i32Kind > 0)
					{
						NkListSolInfo nkListSolInfo = new NkListSolInfo();
						nkListSolInfo.ShowCombat = true;
						nkListSolInfo.FightPower = info.mine_solinfo[j].nFightPower;
						nkListSolInfo.SolLevel = info.mine_solinfo[j].i16Level;
						nkListSolInfo.SolCharKind = info.mine_solinfo[j].i32Kind;
						nkListSolInfo.SolGrade = (int)info.mine_solinfo[j].ui8Grade;
						nkListSolInfo.ShowLevel = false;
						nkListSolInfo.SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(info.mine_solinfo[j].nCostumeUnique);
						EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(nkListSolInfo.SolCharKind, (byte)nkListSolInfo.SolGrade);
						if (eventHeroCharCode != null)
						{
							this.m_dtOccSolBG[(int)info.mine_solinfo[j].ui8BatchPos].SetTexture("Win_I_EventSol");
						}
						else
						{
							UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(nkListSolInfo.SolCharKind, (int)((byte)nkListSolInfo.SolGrade));
							if (legendFrame != null)
							{
								this.m_dtOccSolBG[(int)info.mine_solinfo[j].ui8BatchPos].SetTexture(legendFrame);
							}
						}
						this.m_itOccSol[(int)info.mine_solinfo[j].ui8BatchPos].SetSolImageTexure(eCharImageType.SMALL, nkListSolInfo, false);
					}
				}
			}
		}
	}

	public void ClickEcoMonDetailInfo(IUIObject obj)
	{
		if (this.m_GuildID <= 0L)
		{
			int num = (int)obj.Data;
			if (num >= 0)
			{
				this.SetEcoMonDetailinfo(num);
			}
		}
	}

	public void ClickOccupyDetailInfo(IUIObject obj)
	{
		int num = (int)obj.Data;
		this.nTempCurrentIndex = (int)obj.Data;
		if (num >= 0)
		{
			if (this.m_bLeadCheck && this.m_bChangeCheck)
			{
				if (this.m_nCurrentBatchIndex < 0)
				{
					this.m_nCurrentBatchIndex = num;
					this.SetOccupyDetailinfo(num);
					if (this.m_eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK)
					{
						this.m_dtAttackSolPosEffect[num].Visible = true;
					}
					else if (this.m_eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE)
					{
						this.m_dtOccSolPosEffect[num].Visible = true;
					}
				}
				else
				{
					this.m_nSelectBatchIndex = num;
					this.IsValidChangeMilitaryPos();
					if (this.m_eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK)
					{
						this.m_dtAttackSolPosEffect[num].Visible = false;
						if (this.m_nCurrentBatchIndex > 0)
						{
							this.m_dtAttackSolPosEffect[this.m_nCurrentBatchIndex].Visible = false;
						}
					}
					else if (this.m_eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE)
					{
						this.m_dtOccSolPosEffect[num].Visible = false;
						if (this.m_nCurrentBatchIndex > 0)
						{
							this.m_dtOccSolPosEffect[this.m_nCurrentBatchIndex].Visible = false;
						}
					}
				}
			}
			else
			{
				if (this.m_dicOccupy_User_SolList.ContainsKey(num))
				{
					if (this.m_dicOccupy_User_SolList[num].nLeaderMilitary != 0)
					{
						this.LeadEffect(false);
					}
					else
					{
						this.LeadEffect(true);
						this.m_Temp = true;
					}
				}
				this.SetOccupyDetailinfo(num);
			}
		}
	}

	public void IsValidChangeMilitaryPos()
	{
		MINE_MILITARY_USER_SOLINFO mINE_MILITARY_USER_SOLINFO = null;
		MINE_MILITARY_USER_SOLINFO mINE_MILITARY_USER_SOLINFO2 = null;
		if (this.m_nCurrentBatchIndex == this.m_nSelectBatchIndex)
		{
			this.m_nCurrentBatchIndex = -1;
			this.m_nSelectBatchIndex = -1;
			return;
		}
		if (this.m_nSelectBatchIndex >= 0)
		{
			if (this.m_dicOccupy_User_SolList.ContainsKey(this.m_nCurrentBatchIndex))
			{
				mINE_MILITARY_USER_SOLINFO = this.m_dicOccupy_User_SolList[this.m_nCurrentBatchIndex];
				mINE_MILITARY_USER_SOLINFO.ui8BatchIndex = (byte)this.m_nSelectBatchIndex;
			}
			if (this.m_dicOccupy_User_SolList.ContainsKey(this.m_nSelectBatchIndex))
			{
				mINE_MILITARY_USER_SOLINFO2 = this.m_dicOccupy_User_SolList[this.m_nSelectBatchIndex];
				mINE_MILITARY_USER_SOLINFO2.ui8BatchIndex = (byte)this.m_nCurrentBatchIndex;
			}
			if (!this.m_dicOccupy_User_SolList.ContainsKey(this.m_nSelectBatchIndex))
			{
				this.m_dicOccupy_User_SolList.Remove(this.m_nCurrentBatchIndex);
				this.SetOccupyDetailinfo(this.m_nCurrentBatchIndex);
			}
			if (!this.m_dicOccupy_User_SolList.ContainsKey(this.m_nCurrentBatchIndex))
			{
				this.m_dicOccupy_User_SolList.Remove(this.m_nSelectBatchIndex);
				this.SetOccupyDetailinfo(this.m_nSelectBatchIndex);
			}
			if (mINE_MILITARY_USER_SOLINFO != null)
			{
				if (this.m_dicOccupy_User_SolList.ContainsKey(this.m_nSelectBatchIndex))
				{
					this.m_dicOccupy_User_SolList.Remove(this.m_nSelectBatchIndex);
				}
				this.m_dicOccupy_User_SolList.Add(this.m_nSelectBatchIndex, mINE_MILITARY_USER_SOLINFO);
				this.SetOccupyDetailinfo(this.m_nSelectBatchIndex);
			}
			if (mINE_MILITARY_USER_SOLINFO2 != null)
			{
				if (this.m_dicOccupy_User_SolList.ContainsKey(this.m_nCurrentBatchIndex))
				{
					this.m_dicOccupy_User_SolList.Remove(this.m_nCurrentBatchIndex);
				}
				this.m_dicOccupy_User_SolList.Add(this.m_nCurrentBatchIndex, mINE_MILITARY_USER_SOLINFO2);
				this.SetOccupyDetailinfo(this.m_nCurrentBatchIndex);
			}
		}
		this.LeadEffect(true);
		this.m_nCurrentBatchIndex = -1;
		this.m_nSelectBatchIndex = -1;
	}

	public void OnBtnClickClose(IUIObject obj)
	{
		this.Close();
	}

	public void OnBtnClickStart(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (this.m_bHaveMilitary)
		{
			long num = 0L;
			MINE_CONSTANT_Manager instance = MINE_CONSTANT_Manager.GetInstance();
			if (instance != null)
			{
				num = (long)instance.GetValue(eMINE_CONSTANT.eMINE_CONSTANT_MINE_RETURN_TIME);
			}
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
			string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("161");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromMessageBox2,
				"count",
				num.ToString()
			});
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.OnStartBackMove), null, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL, 2);
			return;
		}
		if (kMyCharInfo.GetMilitaryList().FindEmptyMineMilitaryIndex() == -1)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("691"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!NrTSingleton<MineManager>.Instance.IsEnoughMineJoinCount())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("405"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!this.IsStartBattle())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("528"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!this.IsAddMilitary())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("317"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(this.m_mine_info.i16MineDataID);
		if (mineCreateDataFromID == null)
		{
			return;
		}
		MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(BASE_MINE_DATA.ParseGradeFromString(mineCreateDataFromID.MINE_GRADE));
		GS_MINE_MILITARY_GET_REQ gS_MINE_MILITARY_GET_REQ = new GS_MINE_MILITARY_GET_REQ();
		gS_MINE_MILITARY_GET_REQ.i64MineID = this.m_mine_info.i64MineID;
		gS_MINE_MILITARY_GET_REQ.m_nMineGrade = mineDataFromGrade.GetGrade();
		gS_MINE_MILITARY_GET_REQ.m_nMode = 2;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_MILITARY_GET_REQ, gS_MINE_MILITARY_GET_REQ);
	}

	private void OnStartBackMove(object a_oObject)
	{
		GS_MINE_MILITARY_BACKMOVE_REQ gS_MINE_MILITARY_BACKMOVE_REQ = new GS_MINE_MILITARY_BACKMOVE_REQ();
		gS_MINE_MILITARY_BACKMOVE_REQ.m_nMineID = this.m_mine_info.i64MineID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_MILITARY_BACKMOVE_REQ, gS_MINE_MILITARY_BACKMOVE_REQ);
		this.Close();
	}

	public void OnBtnClickGoMilitary(IUIObject obj)
	{
		string text = string.Empty;
		string message = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo.GetMilitaryList().FindEmptyMineMilitaryIndex() == -1)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("691"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!NrTSingleton<MineManager>.Instance.IsEnoughMineJoinCount())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("405"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!this.IsStartBattle())
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("528");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(this.m_mine_info.i16MineDataID);
		if (mineCreateDataFromID == null)
		{
			return;
		}
		MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(BASE_MINE_DATA.ParseGradeFromString(mineCreateDataFromID.MINE_GRADE));
		if (kMyCharInfo.GetLevel() < (int)mineDataFromGrade.POSSIBLELEVEL)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("408");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref message, new object[]
			{
				text,
				"count",
				mineDataFromGrade.POSSIBLELEVEL
			});
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_MINE_MILITARY_GET_REQ gS_MINE_MILITARY_GET_REQ = new GS_MINE_MILITARY_GET_REQ();
		gS_MINE_MILITARY_GET_REQ.i64MineID = this.m_mine_info.i64MineID;
		gS_MINE_MILITARY_GET_REQ.m_nMineGrade = mineDataFromGrade.GetGrade();
		gS_MINE_MILITARY_GET_REQ.m_nMode = 2;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_MILITARY_GET_REQ, gS_MINE_MILITARY_GET_REQ);
	}

	public void OnBtnClickResearch(IUIObject obj)
	{
		MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(this.m_mine_info.i16MineDataID);
		if (mineCreateDataFromID == null)
		{
			return;
		}
		MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(mineCreateDataFromID.GetGrade());
		if (mineDataFromGrade == null)
		{
			return;
		}
		string message = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo.GetMilitaryList().FindEmptyMineMilitaryIndex() == -1)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("691");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (mineDataFromGrade.MINE_SEARCH_MONEY > kMyCharInfo.m_Money)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!NrTSingleton<MineManager>.Instance.IsEnoughMineJoinCount())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("405"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!this.IsStartBattle())
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("528");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MINE_WAITMILTARYINFO_DLG);
		GS_MINE_SEARCH_REQ gS_MINE_SEARCH_REQ = new GS_MINE_SEARCH_REQ();
		gS_MINE_SEARCH_REQ.bSearchMineGrade = mineCreateDataFromID.GetGrade();
		gS_MINE_SEARCH_REQ.m_nMineID = 0L;
		gS_MINE_SEARCH_REQ.m_nGuildID = 0L;
		gS_MINE_SEARCH_REQ.m_nMode = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_SEARCH_REQ, gS_MINE_SEARCH_REQ);
	}

	private void OnClickMilitaryPosChange(IUIObject obj)
	{
		this.m_bChangeCheck = true;
		this.InitEditMode(this.m_bChangeCheck);
		Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("93"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
		this.SetOccupyDetailinfo(this.m_select_index);
	}

	public void OnClickChangeMessageBox(IUIObject obj)
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2513");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("165");
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.ChangeMilitaryUpdate), null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL, 2);
	}

	public void ChangeMilitaryUpdate(object a_oObject)
	{
		this.m_bChangeCheck = false;
		this.InitEditMode(this.m_bChangeCheck);
		for (int i = 0; i < 9; i++)
		{
			if (this.m_dicOccupy_User_SolList.ContainsKey(i) && this.m_dicOccupy_User_SolList[i].nLeaderMilitary != 0)
			{
				this.InitInterface(i);
				this.SetOccupyDetailinfo(i);
				this.LeadEffect(false);
			}
		}
		if (this.m_GuildID <= 0L)
		{
			return;
		}
		if (this.m_mine_info == null)
		{
			return;
		}
		if (this.m_mine_info.i64MineID <= 0L)
		{
			return;
		}
		GS_MINE_MILITRAY_CHANGE_POS_REQ gS_MINE_MILITRAY_CHANGE_POS_REQ = new GS_MINE_MILITRAY_CHANGE_POS_REQ();
		gS_MINE_MILITRAY_CHANGE_POS_REQ.nGuildID = this.m_GuildID;
		gS_MINE_MILITRAY_CHANGE_POS_REQ.i64MineID = this.m_mine_info.i64MineID;
		int num = 0;
		foreach (KeyValuePair<int, MINE_MILITARY_USER_SOLINFO> current in this.m_dicOccupy_User_SolList)
		{
			if (current.Value != null)
			{
				if (current.Value.i64PersonID > 0L)
				{
					if (current.Value.ui8BatchIndex >= 0)
					{
						gS_MINE_MILITRAY_CHANGE_POS_REQ.MilitaryBatchInfo[num].nPersonID = current.Value.i64PersonID;
						gS_MINE_MILITRAY_CHANGE_POS_REQ.MilitaryBatchInfo[num].nBatchIndex = current.Value.ui8BatchIndex;
						num++;
						if (9 <= num)
						{
							break;
						}
					}
				}
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_MILITRAY_CHANGE_POS_REQ, gS_MINE_MILITRAY_CHANGE_POS_REQ);
	}

	public void OnClickCancelMessageBox(IUIObject obj)
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2513");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("166");
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnClickCancel), null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL, 2);
	}

	private void OnClickCancel(object a_oObject)
	{
		this.m_bChangeCheck = false;
		this.InitEditMode(this.m_bChangeCheck);
		this.RollBackBattlePos();
		this.LeadEffect(false);
		Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("308"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
	}

	public bool IsStartBattle()
	{
		int num = 0;
		MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(this.m_mine_info.i16MineDataID);
		if (mineCreateDataFromID == null)
		{
			return false;
		}
		MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(BASE_MINE_DATA.ParseGradeFromString(mineCreateDataFromID.MINE_GRADE));
		if (mineDataFromGrade == null)
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
				if (current.GetSolPosType() != 2)
				{
					if (current.GetSolPosType() != 6)
					{
						int mineMoneyFromSolPossibleLevel = BASE_MINE_DATA.GetMineMoneyFromSolPossibleLevel(mineDataFromGrade.GetGrade());
						if ((int)current.GetLevel() >= mineMoneyFromSolPossibleLevel)
						{
							num++;
						}
					}
				}
			}
		}
		return num > 0;
	}

	public void RollBackBattlePos()
	{
		MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(this.m_mine_info.i16MineDataID);
		if (mineCreateDataFromID == null)
		{
			return;
		}
		GS_MINE_SEARCH_REQ gS_MINE_SEARCH_REQ = new GS_MINE_SEARCH_REQ();
		gS_MINE_SEARCH_REQ.bSearchMineGrade = mineCreateDataFromID.GetGrade();
		gS_MINE_SEARCH_REQ.m_nMineID = this.m_mine_info.i64MineID;
		gS_MINE_SEARCH_REQ.m_nGuildID = this.m_GuildID;
		gS_MINE_SEARCH_REQ.m_nMode = (byte)this.m_eMode;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_SEARCH_REQ, gS_MINE_SEARCH_REQ);
	}

	public void DrawTextureAddEffectDelegate(IUIObject control, GameObject obj)
	{
		if (obj == null)
		{
			return;
		}
		for (int i = 0; i < 9; i++)
		{
			if (this.m_btAttackMilitary[i] == control)
			{
				obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
				obj.transform.localPosition = new Vector3(50f, -44f, -0.1f);
				break;
			}
			if (this.m_btOccMilitary[i] == control)
			{
				obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
				obj.transform.localPosition = new Vector3(50f, -44f, -0.1f);
				break;
			}
		}
	}

	public void DrawTextureAddLeadEffectDelegate(IUIObject control, GameObject obj)
	{
		if (obj == null)
		{
			return;
		}
		for (int i = 0; i < 9; i++)
		{
			if (this.m_dAttackPersonImage[i] == control)
			{
				obj.transform.localScale = new Vector3(1f, 1f, 1f);
				obj.transform.localPosition = new Vector3(18.3f, 4f, -0.2f);
				break;
			}
			if (this.m_dAttackSelectImage[i] == control)
			{
				obj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
				obj.transform.localPosition = new Vector3(16.3f, 2.1f, -0.2f);
				break;
			}
			if (this.m_dOccPersonImage[i] == control)
			{
				obj.transform.localScale = new Vector3(1f, 1f, 1f);
				obj.transform.localPosition = new Vector3(18.3f, 4f, -0.2f);
				break;
			}
			if (this.m_dOccSelectImage[i] == control)
			{
				obj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
				obj.transform.localPosition = new Vector3(16.3f, 2.1f, -0.2f);
				break;
			}
		}
	}

	public void LeadEffect(bool bEditMode)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		long personID = charPersonInfo.GetPersonID();
		for (int i = 0; i < 9; i++)
		{
			Transform transform = this.m_dAttackSelectImage[i].gameObject.transform.FindChild("child_effect");
			if (transform != null)
			{
				this.m_dAttackSelectImage[i].Visible = false;
				this.m_dtAttackSolPosEffect[i].Visible = false;
				UnityEngine.Object.DestroyImmediate(transform.gameObject);
			}
			Transform transform2 = this.m_dOccSelectImage[i].gameObject.transform.FindChild("child_effect");
			if (transform2 != null)
			{
				this.m_dOccSelectImage[i].Visible = false;
				this.m_dtOccSolPosEffect[i].Visible = false;
				UnityEngine.Object.DestroyImmediate(transform2.gameObject);
			}
			Transform transform3 = this.m_dAttackPersonImage[i].gameObject.transform.FindChild("child_effect");
			if (transform3 != null)
			{
				this.m_dAttackPersonImage[i].Visible = false;
				this.m_dtAttackSolPosEffect[i].Visible = false;
				UnityEngine.Object.DestroyImmediate(transform3.gameObject);
			}
			Transform transform4 = this.m_dOccPersonImage[i].gameObject.transform.FindChild("child_effect");
			if (transform4 != null)
			{
				this.m_dOccPersonImage[i].Visible = false;
				this.m_dtOccSolPosEffect[i].Visible = false;
				UnityEngine.Object.DestroyImmediate(transform4.gameObject);
			}
		}
		bool flag = false;
		bool flag2 = false;
		if (this.m_eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK)
		{
			flag = true;
		}
		if (this.m_eMode == eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE)
		{
			flag2 = true;
		}
		string path = string.Format("{0}", "UI/Mine/fx_ui_minecrown" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		if (bEditMode)
		{
			for (int j = 0; j < 9; j++)
			{
				if (flag)
				{
					if (this.m_dicOccupy_User_SolList.ContainsKey(j))
					{
						if (this.m_dicOccupy_User_SolList[j].nLeaderMilitary != 0)
						{
							NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dAttackPersonImage[j], this.m_dAttackPersonImage[j].GetSize());
							this.m_dAttackPersonImage[j].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
							this.m_dAttackPersonImage[j].Visible = true;
						}
						if (personID == this.m_dicOccupy_User_SolList[j].i64PersonID)
						{
							this.MyMilitarySearch(j, bEditMode);
						}
					}
				}
			}
			for (int k = 0; k < 9; k++)
			{
				if (flag2)
				{
					if (this.m_dicOccupy_User_SolList.ContainsKey(k))
					{
						if (this.m_dicOccupy_User_SolList[k].nLeaderMilitary != 0)
						{
							NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dOccPersonImage[k], this.m_dOccPersonImage[k].GetSize());
							this.m_dOccPersonImage[k].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
							this.m_dOccPersonImage[k].Visible = true;
						}
						if (personID == this.m_dicOccupy_User_SolList[k].i64PersonID)
						{
							this.MyMilitarySearch(k, bEditMode);
						}
					}
				}
			}
		}
		if (!bEditMode)
		{
			for (int l = 0; l < 9; l++)
			{
				if (flag)
				{
					if (this.m_dicOccupy_User_SolList.ContainsKey(l))
					{
						int ui8BatchIndex = (int)this.m_dicOccupy_User_SolList[l].ui8BatchIndex;
						if (this.m_dicOccupy_User_SolList[ui8BatchIndex].nLeaderMilitary != 0)
						{
							if (this.nTempCurrentIndex == -1)
							{
								if (this.m_bLeadCheck)
								{
									NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dAttackSelectImage[ui8BatchIndex], this.m_dAttackSelectImage[ui8BatchIndex].GetSize());
									this.m_dAttackSelectImage[ui8BatchIndex].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
									this.m_dAttackSelectImage[ui8BatchIndex].Visible = true;
								}
								else
								{
									if (!this.m_bHaveMilitary)
									{
										NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dAttackSelectImage[ui8BatchIndex], this.m_dAttackSelectImage[ui8BatchIndex].GetSize());
										this.m_dAttackSelectImage[ui8BatchIndex].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
										this.m_dAttackSelectImage[ui8BatchIndex].Visible = true;
										break;
									}
									NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dAttackPersonImage[ui8BatchIndex], this.m_dAttackPersonImage[ui8BatchIndex].GetSize());
									this.m_dAttackPersonImage[ui8BatchIndex].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
									this.m_dAttackPersonImage[ui8BatchIndex].Visible = true;
								}
							}
							else if (this.nTempCurrentIndex == (int)this.m_dicOccupy_User_SolList[ui8BatchIndex].ui8BatchIndex)
							{
								NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dAttackSelectImage[ui8BatchIndex], this.m_dAttackSelectImage[ui8BatchIndex].GetSize());
								this.m_dAttackSelectImage[ui8BatchIndex].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
								this.m_dAttackSelectImage[ui8BatchIndex].Visible = true;
							}
							else
							{
								NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dAttackPersonImage[ui8BatchIndex], this.m_dAttackPersonImage[ui8BatchIndex].GetSize());
								this.m_dAttackPersonImage[ui8BatchIndex].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
								this.m_dAttackPersonImage[ui8BatchIndex].Visible = true;
							}
						}
						if (personID == this.m_dicOccupy_User_SolList[ui8BatchIndex].i64PersonID)
						{
							this.MyMilitarySearch(ui8BatchIndex, bEditMode);
						}
					}
				}
			}
			for (int m = 0; m < 9; m++)
			{
				if (flag2)
				{
					if (this.m_dicOccupy_User_SolList.ContainsKey(m))
					{
						int ui8BatchIndex2 = (int)this.m_dicOccupy_User_SolList[m].ui8BatchIndex;
						if (this.m_dicOccupy_User_SolList[ui8BatchIndex2].nLeaderMilitary != 0)
						{
							if (this.nTempCurrentIndex == -1)
							{
								if (this.m_bLeadCheck)
								{
									NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dOccSelectImage[ui8BatchIndex2], this.m_dOccSelectImage[ui8BatchIndex2].GetSize());
									this.m_dOccSelectImage[ui8BatchIndex2].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
									this.m_dOccSelectImage[ui8BatchIndex2].Visible = true;
								}
								else
								{
									if (!this.m_bHaveMilitary)
									{
										NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dOccSelectImage[ui8BatchIndex2], this.m_dOccSelectImage[ui8BatchIndex2].GetSize());
										this.m_dOccSelectImage[ui8BatchIndex2].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
										this.m_dOccSelectImage[ui8BatchIndex2].Visible = true;
										break;
									}
									NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dOccPersonImage[ui8BatchIndex2], this.m_dOccPersonImage[ui8BatchIndex2].GetSize());
									this.m_dOccPersonImage[ui8BatchIndex2].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
									this.m_dOccPersonImage[ui8BatchIndex2].Visible = true;
								}
							}
							else if (this.nTempCurrentIndex == (int)this.m_dicOccupy_User_SolList[ui8BatchIndex2].ui8BatchIndex)
							{
								NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dOccSelectImage[ui8BatchIndex2], this.m_dOccSelectImage[ui8BatchIndex2].GetSize());
								this.m_dOccSelectImage[ui8BatchIndex2].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
								this.m_dOccSelectImage[ui8BatchIndex2].Visible = true;
							}
							else
							{
								NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_dOccPersonImage[ui8BatchIndex2], this.m_dOccPersonImage[ui8BatchIndex2].GetSize());
								this.m_dOccPersonImage[ui8BatchIndex2].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddLeadEffectDelegate));
								this.m_dOccPersonImage[ui8BatchIndex2].Visible = true;
							}
						}
						if (personID == this.m_dicOccupy_User_SolList[ui8BatchIndex2].i64PersonID)
						{
							this.MyMilitarySearch(ui8BatchIndex2, bEditMode);
						}
					}
				}
			}
		}
	}

	public void WaitGuildinfodlg(IUIObject obj)
	{
		GS_MINE_WAIT_MILITARY_INFO_REQ gS_MINE_WAIT_MILITARY_INFO_REQ = new GS_MINE_WAIT_MILITARY_INFO_REQ();
		gS_MINE_WAIT_MILITARY_INFO_REQ.m_nMineID = this.m_mine_info.i64MineID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_WAIT_MILITARY_INFO_REQ, gS_MINE_WAIT_MILITARY_INFO_REQ);
	}

	public void MyMilitarySearch(int nIndex, bool bEditMode)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		long personID = charPersonInfo.GetPersonID();
		int ui8BatchIndex = (int)this.m_dicOccupy_User_SolList[nIndex].ui8BatchIndex;
		for (int i = 0; i < 9; i++)
		{
			this.m_btOccMilitary[i].SetButtonTextureKey("Win_I_Portrait04");
			this.m_btOccMilitarySelect[i].SetButtonTextureKey("Win_I_Portrait04");
			this.m_btAttackMilitary[i].SetButtonTextureKey("Win_I_Portrait04");
			this.m_btAttackMilitarySelect[i].SetButtonTextureKey("Win_I_Portrait04");
		}
		if (this.m_dicOccupy_User_SolList[nIndex].i64PersonID == personID && this.m_dicOccupy_User_SolList[nIndex].nLeaderMilitary != 1)
		{
			bEditMode = false;
			this.m_Temp = false;
		}
		if (bEditMode)
		{
			switch (this.m_eMode)
			{
			case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE:
				if (ui8BatchIndex != this.nTempCurrentIndex)
				{
					this.m_btOccMilitary[ui8BatchIndex].Visible = true;
					this.m_btOccMilitary[ui8BatchIndex].SetButtonTextureKey("Win_I_Portrait03");
				}
				break;
			case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK:
				if (ui8BatchIndex != this.nTempCurrentIndex)
				{
					this.m_btAttackMilitary[ui8BatchIndex].Visible = true;
					this.m_btAttackMilitary[ui8BatchIndex].SetButtonTextureKey("Win_I_Portrait03");
				}
				break;
			}
		}
		if (!bEditMode)
		{
			switch (this.m_eMode)
			{
			case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYDEFENCE:
				if (ui8BatchIndex == this.nTempCurrentIndex || this.nTempCurrentIndex == -1)
				{
					this.m_btOccMilitarySelect[ui8BatchIndex].Visible = true;
					this.m_btOccMilitarySelect[ui8BatchIndex].SetButtonTextureKey("Win_I_Portrait03");
				}
				else
				{
					this.m_btOccMilitary[ui8BatchIndex].Visible = true;
					this.m_btOccMilitary[ui8BatchIndex].SetButtonTextureKey("Win_I_Portrait03");
				}
				break;
			case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_OTHERDEFENCE:
				if (ui8BatchIndex == this.nTempCurrentIndex || this.nTempCurrentIndex == -1)
				{
					this.m_btOccMilitarySelect[ui8BatchIndex].Visible = true;
					this.m_btOccMilitarySelect[ui8BatchIndex].SetButtonTextureKey("Win_I_Portrait03");
				}
				else
				{
					this.m_btOccMilitary[ui8BatchIndex].Visible = true;
					this.m_btOccMilitary[ui8BatchIndex].SetButtonTextureKey("Win_I_Portrait03");
				}
				break;
			case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_MYATTACK:
				if (ui8BatchIndex == this.nTempCurrentIndex || this.nTempCurrentIndex == -1)
				{
					this.m_btAttackMilitarySelect[ui8BatchIndex].Visible = true;
					this.m_btAttackMilitarySelect[ui8BatchIndex].SetButtonTextureKey("Win_I_Portrait03");
				}
				else
				{
					this.m_btAttackMilitary[ui8BatchIndex].Visible = true;
					this.m_btAttackMilitary[ui8BatchIndex].SetButtonTextureKey("Win_I_Portrait03");
				}
				break;
			case eMineSearchDetailInfo_Mode.eMINE_DETAILDLG_OTHERATTACK:
				if (ui8BatchIndex == this.nTempCurrentIndex || this.nTempCurrentIndex == -1)
				{
					this.m_btAttackMilitarySelect[ui8BatchIndex].Visible = true;
					this.m_btAttackMilitarySelect[ui8BatchIndex].SetButtonTextureKey("Win_I_Portrait03");
				}
				else
				{
					this.m_btAttackMilitary[ui8BatchIndex].Visible = true;
					this.m_btAttackMilitary[ui8BatchIndex].SetButtonTextureKey("Win_I_Portrait03");
				}
				break;
			}
		}
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
}
