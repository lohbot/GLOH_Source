using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityForms;

public class Myth_Evolution_Main_DLG : Form
{
	public enum eMYTHTOOLBAR
	{
		eMYTH_LEGEND,
		eMYTH_EVOLUTION,
		eMYTH_MAXTOOLBAR
	}

	protected const int MAX_LIST_COUNT = 9;

	protected Toolbar m_ToolBar;

	protected DropDownList m_DropDownList_Season;

	private Label m_Label_Gold;

	private Label m_Label_Essence;

	protected NewListBox m_NewListBox;

	private Label m_LB_MythEvolution_Help2;

	private Label m_LB_MythEvolution_Help3;

	private Label m_Label_MythEvolution_Essence_Num;

	private Label m_Label_MythEvolution_DragonHeart_Num;

	private Label m_Label_MythEvolution_Descent_Num;

	private Label m_Label_MythEvolution_Essence_Num2;

	private Label m_Label_MythEvolution_DragonHeart_Num2;

	private Button m_BTN_MythEvolutionStart;

	private Label m_Label_BaseSeason;

	private Label m_Label_ResultSeason;

	private DrawTexture m_DT_BaseSolRank;

	private DrawTexture m_DT_ResultSolRank;

	private DrawTexture m_DT_BaseSolimg;

	private DrawTexture m_DT_ResultSolimg;

	private Label m_LB_BaseSolName;

	private Label m_LB_ResultSolName;

	private DrawTexture m_DT_MythSkillLock;

	private DrawTexture m_DT_MythSkill_BG1;

	private DrawTexture m_DT_MythSkillIcon1_1;

	private DrawTexture m_DT_MythSkillIcon1_2;

	private Label m_LB_MythSkillname1;

	private Label m_LB_MythSkillLevel1;

	private DrawTexture m_DT_MythSkill_BG2;

	private DrawTexture m_DT_MythSkillIcon2_1;

	private DrawTexture m_DT_MythSkillIcon2_2;

	private Label m_LB_MythSkillname2;

	private Label m_LB_MythSkillLevel2;

	private Button m_Button_BaseSol01;

	private Button m_Button_BaseSol02;

	private Button m_BTN_MythSkill_Info1;

	private Button m_BTN_MythSkill_Info2;

	private Label m_LB_MythTime;

	private Button m_BTN_MythTime;

	public MYTH_TYPE e_MythType;

	private Dictionary<byte, List<MythSolSlotData>> dicSlotData = new Dictionary<byte, List<MythSolSlotData>>();

	private List<SOLGUIDE_DATA> m_MythSolList = new List<SOLGUIDE_DATA>();

	private byte bCurrentSeason;

	private long m_i64BaseSolID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Soldier/Evolution/DLG_SolEvolution_Main", G_ID.MYTH_EVOLUTION_MAIN_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_ToolBar = (base.GetControl("ToolBar_01") as Toolbar);
		this.m_ToolBar.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3437");
		UIPanelTab expr_44 = this.m_ToolBar.Control_Tab[0];
		expr_44.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_44.ButtonClick, new EZValueChangedDelegate(this.ClickToolbar));
		this.m_ToolBar.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3438");
		UIPanelTab expr_93 = this.m_ToolBar.Control_Tab[1];
		expr_93.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_93.ButtonClick, new EZValueChangedDelegate(this.ClickToolbar));
		if (NrTSingleton<ContentsLimitManager>.Instance.IsMythEvolutionLimit())
		{
			this.m_ToolBar.Control_Tab[1].Visible = false;
		}
		this.m_DropDownList_Season = (base.GetControl("DDL_Season") as DropDownList);
		this.m_DropDownList_Season.AddValueChangedDelegate(new EZValueChangedDelegate(this.Change_Season));
		this.m_Label_Gold = (base.GetControl("Label_Gold") as Label);
		this.m_Label_Essence = (base.GetControl("Label_Essence") as Label);
		this.m_NewListBox = (base.GetControl("NLB_SolList") as NewListBox);
		this.m_Label_MythEvolution_Essence_Num = (base.GetControl("Label_MythEvolution_Essence_Num") as Label);
		this.m_Label_MythEvolution_DragonHeart_Num = (base.GetControl("Label_MythEvolution_DragonHeart_Num") as Label);
		this.m_Label_MythEvolution_Descent_Num = (base.GetControl("Label_MythEvolution_Descent_Num") as Label);
		this.m_Label_MythEvolution_Essence_Num2 = (base.GetControl("Label_MythEvolution_Essence_Num2") as Label);
		this.m_Label_MythEvolution_DragonHeart_Num2 = (base.GetControl("Label_MythEvolution_DragonHeart_Num2") as Label);
		this.m_LB_MythEvolution_Help2 = (base.GetControl("LB_MythEvolution_Help2") as Label);
		this.m_LB_MythEvolution_Help3 = (base.GetControl("LB_MythEvolution_Help3") as Label);
		this.m_BTN_MythEvolutionStart = (base.GetControl("BTN_MythEvolutionStart") as Button);
		this.m_BTN_MythEvolutionStart.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythEvolution));
		this.m_DT_BaseSolRank = (base.GetControl("DT_BaseSolRank") as DrawTexture);
		this.m_DT_ResultSolRank = (base.GetControl("DT_ResultSolRank") as DrawTexture);
		this.m_DT_BaseSolimg = (base.GetControl("DT_BaseSolimg") as DrawTexture);
		this.m_DT_ResultSolimg = (base.GetControl("DT_ResultSolimg") as DrawTexture);
		this.m_Label_BaseSeason = (base.GetControl("Label_BaseSeason") as Label);
		this.m_Label_ResultSeason = (base.GetControl("Label_ResultSeason") as Label);
		this.m_LB_BaseSolName = (base.GetControl("LB_BaseSolName") as Label);
		this.m_LB_ResultSolName = (base.GetControl("LB_ResultSolName") as Label);
		this.m_LB_MythSkillname1 = (base.GetControl("LB_MythSkillname1") as Label);
		this.m_LB_MythSkillLevel1 = (base.GetControl("LB_MythSkillLevel1") as Label);
		this.m_LB_MythSkillname2 = (base.GetControl("LB_MythSkillname2") as Label);
		this.m_LB_MythSkillLevel2 = (base.GetControl("LB_MythSkillLevel2") as Label);
		this.m_Button_BaseSol01 = (base.GetControl("Button_BaseSol01") as Button);
		this.m_Button_BaseSol01.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythBaseSol));
		this.m_Button_BaseSol02 = (base.GetControl("Button_BaseSol02") as Button);
		this.m_Button_BaseSol02.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythBaseSol));
		this.m_BTN_MythSkill_Info1 = (base.GetControl("BTN_MythSkill_Info1") as Button);
		this.m_BTN_MythSkill_Info1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythSkillDetail));
		this.m_BTN_MythSkill_Info2 = (base.GetControl("BTN_MythSkill_Info2") as Button);
		this.m_BTN_MythSkill_Info2.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythSkillDetail));
		this.m_DT_MythSkillIcon1_1 = (base.GetControl("DT_MythSkillIcon1_1") as DrawTexture);
		this.m_DT_MythSkillIcon1_2 = (base.GetControl("DT_MythSkillIcon1_2") as DrawTexture);
		this.m_DT_MythSkill_BG1 = (base.GetControl("DT_MythSkill_BG1") as DrawTexture);
		this.m_DT_MythSkillIcon2_1 = (base.GetControl("DT_MythSkillIcon2_1") as DrawTexture);
		this.m_DT_MythSkillIcon2_2 = (base.GetControl("DT_MythSkillIcon2_2") as DrawTexture);
		this.m_DT_MythSkill_BG2 = (base.GetControl("DT_MythSkill_BG2") as DrawTexture);
		this.m_DT_MythSkillLock = (base.GetControl("DT_MythSkillLock") as DrawTexture);
		this.m_LB_MythTime = (base.GetControl("LB_MythTime") as Label);
		this.m_BTN_MythTime = (base.GetControl("BTN_MythTime") as Button);
		this.m_BTN_MythTime.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythTime));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	private void ClickToolbar(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		if (uIPanelTab.panel.index == 0)
		{
			this.m_ToolBar.SetSelectTabIndex(uIPanelTab.panel.index);
			this.SetLegend();
		}
		else if (uIPanelTab.panel.index == 1)
		{
			this.m_ToolBar.SetSelectTabIndex(uIPanelTab.panel.index);
			this.SetEvolution();
		}
	}

	protected virtual void Change_Season(IUIObject obj)
	{
		ListItem listItem = this.m_DropDownList_Season.SelectedItem.Data as ListItem;
		if (this.bCurrentSeason != (byte)listItem.Key)
		{
			this.bCurrentSeason = (byte)listItem.Key;
			this.SetLegendDataSort(this.bCurrentSeason);
			this.SetLegendSolShow(this.bCurrentSeason);
		}
	}

	private void InitMythDataSet()
	{
		this.dicSlotData.Clear();
		List<SOL_GUIDE> value = NrTSingleton<NrTableSolGuideManager>.Instance.GetValue();
		if (value == null)
		{
			return;
		}
		for (int i = 0; i < value.Count; i++)
		{
			SOL_GUIDE sOL_GUIDE = value[i];
			if (sOL_GUIDE != null)
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(sOL_GUIDE.m_i32CharKind);
				if (charKindInfo != null)
				{
					if (sOL_GUIDE.m_bSeason != 0)
					{
						if (!NrTSingleton<ContentsLimitManager>.Instance.IsSolGuide_Season((int)sOL_GUIDE.m_bSeason))
						{
							if (!this.IsMythSolData(sOL_GUIDE.m_i32CharKind))
							{
								if (!NrTSingleton<ContentsLimitManager>.Instance.IsSolGuideCharKindInfo(sOL_GUIDE.m_i32CharKind))
								{
									if (sOL_GUIDE.m_i8Legend == 2)
									{
										if (!NrTSingleton<NrTableSolGuideManager>.Instance.FindSolInfo(sOL_GUIDE.m_i32CharKind))
										{
											goto IL_165;
										}
									}
									else if (sOL_GUIDE.m_i8Legend == 1)
									{
										goto IL_165;
									}
									MythSolSlotData slotData;
									if (charKindInfo != null)
									{
										slotData = new MythSolSlotData(charKindInfo.GetName(), sOL_GUIDE.m_i32CharKind, (byte)sOL_GUIDE.m_iSolGrade, sOL_GUIDE.m_bFlagSet, sOL_GUIDE.m_bFlagSetCount - 1, sOL_GUIDE.m_bSeason, sOL_GUIDE.m_i16LegendSort, sOL_GUIDE.m_i32SkillUnique, sOL_GUIDE.m_i32SkillText);
									}
									else
									{
										slotData = new MythSolSlotData(" ", 0, 0, 0, 0, 0, 0, 0, 0);
									}
									this.SetMythTableData(0, slotData);
									this.SetMythTableData(sOL_GUIDE.m_bSeason, slotData);
								}
							}
						}
					}
				}
			}
			IL_165:;
		}
	}

	private void SetMythTableData(byte bSeason, MythSolSlotData SlotData)
	{
		List<MythSolSlotData> list = null;
		if (this.dicSlotData.ContainsKey(bSeason))
		{
			this.dicSlotData.TryGetValue(bSeason, out list);
			if (list == null)
			{
				list = new List<MythSolSlotData>();
				list.Add(SlotData);
				this.dicSlotData.Add(bSeason, list);
			}
			else
			{
				list.Add(SlotData);
			}
		}
		else
		{
			list = new List<MythSolSlotData>();
			list.Add(SlotData);
			this.dicSlotData.Add(bSeason, list);
		}
	}

	private void SetDropDownList_Season()
	{
		this.m_DropDownList_Season.SetViewArea(this.dicSlotData.Count);
		this.m_DropDownList_Season.Clear();
		this.bCurrentSeason = 0;
		string str = string.Empty;
		foreach (byte current in this.dicSlotData.Keys)
		{
			ListItem listItem = new ListItem();
			listItem.Key = current;
			if (current == 0)
			{
				str = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1943");
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2302"),
					"count",
					current.ToString()
				});
			}
			listItem.SetColumnStr(0, str);
			this.m_DropDownList_Season.Add(listItem);
		}
		this.m_DropDownList_Season.RepositionItems();
		this.m_DropDownList_Season.SetFirstItem();
	}

	public void ClearMythSolInfo()
	{
		this.m_MythSolList.Clear();
	}

	public void AddMythSolInfo(SOLGUIDE_DATA Data)
	{
		for (int i = 0; i < this.m_MythSolList.Count; i++)
		{
			if (this.m_MythSolList[i].i32CharKind == Data.i32CharKind)
			{
				this.m_MythSolList[i] = Data;
				return;
			}
		}
		this.m_MythSolList.Add(Data);
	}

	public bool IsMythSolData(int iCharKind)
	{
		for (int i = 0; i < this.m_MythSolList.Count; i++)
		{
			if (this.m_MythSolList[i].i32CharKind == iCharKind)
			{
				return true;
			}
		}
		return false;
	}

	private void SetLegendSolShow(byte bSeason)
	{
		this.m_NewListBox.Clear();
		List<MythSolSlotData> list = null;
		if (this.dicSlotData.ContainsKey(bSeason))
		{
			this.dicSlotData.TryGetValue(bSeason, out list);
			int num = list.Count / 9 + 1;
			for (int i = 0; i < num; i++)
			{
				NewListItem newListItem = new NewListItem(this.m_NewListBox.ColumnNum, true, string.Empty);
				for (int j = 0; j < 9; j++)
				{
					int num2 = i * 9 + j;
					if (num2 < list.Count)
					{
						NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(list[num2].i32KindInfo);
						newListItem.SetListItemData(9 + j, charKindInfo.GetCharKind(), charKindInfo, new EZValueChangedDelegate(this.OnSoldierInfo), null);
						newListItem.SetListItemData(18 + j, charKindInfo.GetCharKind(), charKindInfo, null, null);
						newListItem.SetListItemData(27 + j, false);
					}
				}
				this.m_NewListBox.Add(newListItem);
			}
			this.m_NewListBox.RepositionItems();
		}
	}

	public void ClickListBox(IUIObject obj)
	{
		NewListBox newListBox = obj as NewListBox;
		if (obj == null || null == newListBox)
		{
			return;
		}
		this.OnSoldierInfo(newListBox.SelectedItem);
	}

	public virtual void OnSoldierInfo(IUIObject obj)
	{
		NrCharKindInfo nrCharKindInfo = obj.Data as NrCharKindInfo;
		if (nrCharKindInfo == null)
		{
			return;
		}
		Myth_Legend_Info_DLG myth_Legend_Info_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_LEGEND_INFO_DLG) as Myth_Legend_Info_DLG;
		if (myth_Legend_Info_DLG != null)
		{
			myth_Legend_Info_DLG.InitSetCharKind(nrCharKindInfo.GetCharKind());
		}
	}

	private int CompareSolNum(MythSolSlotData a, MythSolSlotData b)
	{
		if (a.i16SortNum < b.i16SortNum)
		{
			return 1;
		}
		if (a.i16SortNum == b.i16SortNum)
		{
			return 0;
		}
		return -1;
	}

	public void SetLegendDataSort(byte bSeason)
	{
		List<MythSolSlotData> list = null;
		if (this.dicSlotData.ContainsKey(bSeason))
		{
			this.dicSlotData.TryGetValue(bSeason, out list);
			list.Sort(new Comparison<MythSolSlotData>(this.CompareSolNum));
		}
	}

	public void SetEvolutionSkillImage(bool bShow)
	{
		this.m_DT_MythSkillLock.Visible = bShow;
		this.m_DT_MythSkill_BG1.Visible = bShow;
		this.m_DT_MythSkillIcon1_1.Visible = bShow;
		this.m_DT_MythSkillIcon1_2.Visible = bShow;
		this.m_LB_MythSkillname1.Visible = bShow;
		this.m_LB_MythSkillLevel1.Visible = bShow;
		this.m_DT_MythSkill_BG2.Visible = bShow;
		this.m_DT_MythSkillIcon2_1.Visible = bShow;
		this.m_DT_MythSkillIcon2_2.Visible = bShow;
		this.m_LB_MythSkillname2.Visible = bShow;
		this.m_LB_MythSkillLevel2.Visible = bShow;
		this.m_BTN_MythSkill_Info1.Visible = bShow;
		this.m_BTN_MythSkill_Info2.Visible = bShow;
		this.m_LB_MythEvolution_Help2.Visible = !bShow;
		this.m_LB_MythEvolution_Help3.Visible = !bShow;
		this.m_LB_ResultSolName.Visible = bShow;
		this.m_LB_ResultSolName.SetText(string.Empty);
		if (!bShow)
		{
			this.m_LB_MythEvolution_Help2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3434"));
			this.m_LB_MythEvolution_Help3.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3552"));
		}
	}

	public void SetEvolution()
	{
		this.e_MythType = MYTH_TYPE.MYTHTYPE_EVOLUTION;
		base.SetShowLayer(0, true);
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		base.SetShowLayer(3, false);
		this.m_ToolBar.SetSelectTabIndex(1);
		this.m_DropDownList_Season.SetVisible(false);
		this.m_DT_BaseSolimg.SetTexture(string.Empty);
		this.m_DT_ResultSolimg.SetTexture(string.Empty);
		this.m_DT_BaseSolRank.Visible = false;
		this.m_DT_ResultSolRank.Visible = false;
		this.m_DT_MythSkillIcon1_2.SetTexture(string.Empty);
		this.m_DT_MythSkillIcon2_2.SetTexture(string.Empty);
		this.m_Label_MythEvolution_DragonHeart_Num2.SetText(string.Empty);
		this.m_Label_MythEvolution_Essence_Num2.SetText(string.Empty);
		this.m_Label_MythEvolution_Essence_Num.SetText(ANNUALIZED.Convert(NkUserInventory.GetInstance().Get_First_ItemCnt(50313)));
		this.m_Label_MythEvolution_DragonHeart_Num.SetText(ANNUALIZED.Convert(NkUserInventory.GetInstance().Get_First_ItemCnt(50316)));
		this.m_Label_MythEvolution_Descent_Num.SetText(ANNUALIZED.Convert(NkUserInventory.GetInstance().Get_First_ItemCnt(50317)));
		this.m_Label_BaseSeason.SetText(string.Empty);
		this.m_Label_ResultSeason.SetText(string.Empty);
		this.m_LB_BaseSolName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2031"));
		this.m_LB_MythSkillname1.SetText(string.Empty);
		this.m_LB_MythSkillLevel1.SetText(string.Empty);
		this.m_LB_MythSkillname2.SetText(string.Empty);
		this.m_LB_MythSkillLevel2.SetText(string.Empty);
		this.m_i64BaseSolID = 0L;
		this.SetEvolutionSkillImage(false);
	}

	public void SetLegend()
	{
		this.e_MythType = MYTH_TYPE.MYTHTYPE_LEGEND;
		base.SetShowLayer(0, true);
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, false);
		this.m_ToolBar.SetSelectTabIndex(0);
		this.m_Label_Gold.SetText(ANNUALIZED.Convert(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money));
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ESSENCE_ITEMUNIQUE);
			ITEM item = NkUserInventory.GetInstance().GetItem(value);
			if (item != null)
			{
				this.m_Label_Essence.SetText(ANNUALIZED.Convert(item.m_nItemNum));
			}
			else
			{
				this.m_Label_Essence.SetText("0");
			}
		}
		this.InitMythDataSet();
		this.SetDropDownList_Season();
		this.SetLegendDataSort(0);
		this.SetLegendSolShow(0);
	}

	private void OnClickMythEvolution(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		NkSoldierInfo soldierInfo = this.GetSoldierInfo(this.m_i64BaseSolID);
		if (soldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		int num = soldierInfo.GetSeason() + 1;
		if (num < 0)
		{
			return;
		}
		MYTH_EVOLUTION myth_EvolutionSeason = NrTSingleton<NrTableMyth_EvolutionManager>.Instance.GetMyth_EvolutionSeason((byte)num);
		if (myth_EvolutionSeason != null)
		{
			string empty = string.Empty;
			if (NkUserInventory.GetInstance().Get_First_ItemCnt(myth_EvolutionSeason.m_i32SpendItemUnique1) < myth_EvolutionSeason.m_i32SpendItemNum1)
			{
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(myth_EvolutionSeason.m_i32SpendItemUnique1);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("780"),
					"target",
					itemNameByItemUnique
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				return;
			}
			if (NkUserInventory.GetInstance().Get_First_ItemCnt(myth_EvolutionSeason.m_i32SpendItemUnique2) < myth_EvolutionSeason.m_i32SpendItemNum2)
			{
				string itemNameByItemUnique2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(myth_EvolutionSeason.m_i32SpendItemUnique2);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("780"),
					"target",
					itemNameByItemUnique2
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				return;
			}
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_MYTH_EVOLUTION_TIME);
				long curTime = PublicMethod.GetCurTime();
				if (curTime > charSubData)
				{
					Myth_Evolution_Check_DLG myth_Evolution_Check_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_CHECK_DLG) as Myth_Evolution_Check_DLG;
					if (myth_Evolution_Check_DLG != null)
					{
						myth_Evolution_Check_DLG.SetMythEvolutionOK(this.m_i64BaseSolID);
					}
				}
				else
				{
					Myth_Evolution_Time_DLG myth_Evolution_Time_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_TIME_DLG) as Myth_Evolution_Time_DLG;
					if (myth_Evolution_Time_DLG != null)
					{
						myth_Evolution_Time_DLG.InitSet(this.e_MythType, soldierInfo.GetCharKind(), this.m_i64BaseSolID);
					}
				}
			}
		}
	}

	private void OnClickMythBaseSol(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		SolComposeListDlg.LoadMythEvolution(SOLCOMPOSE_TYPE.MYTHEVOLUTION);
	}

	private void OnClickMythSkillDetail(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		NkSoldierInfo soldierInfo = this.GetSoldierInfo(this.m_i64BaseSolID);
		if (soldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		int charKindbyMythSkillUnique = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindbyMythSkillUnique(soldierInfo.GetCharKind(), 0);
		if (charKindbyMythSkillUnique < 0)
		{
			return;
		}
		SolDetail_Skill_Dlg solDetail_Skill_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAIL_SKILLICON_DLG) as SolDetail_Skill_Dlg;
		if (solDetail_Skill_Dlg != null)
		{
			solDetail_Skill_Dlg.SetSkillData(charKindbyMythSkillUnique, charKindbyMythSkillUnique, true);
		}
	}

	public void SetBaseSol(long i64SolID)
	{
		this.m_i64BaseSolID = i64SolID;
		NkSoldierInfo soldierInfo = this.GetSoldierInfo(i64SolID);
		if (soldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		this.SetEvolutionSkillImage(true);
		int grade = (int)soldierInfo.GetGrade();
		int solgrade = (int)(soldierInfo.GetGrade() + 4);
		string costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(soldierInfo);
		this.m_DT_BaseSolimg.SetTexture(eCharImageType.LARGE, soldierInfo.GetCharKind(), grade, costumePortraitPath);
		this.m_DT_ResultSolimg.SetTexture(eCharImageType.LARGE, soldierInfo.GetCharKind(), solgrade, costumePortraitPath);
		short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(soldierInfo.GetCharKind(), grade);
		UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(soldierInfo.GetCharKind(), grade);
		if (solLargeGradeImg != null)
		{
			this.m_DT_BaseSolRank.Visible = true;
			if (0 < legendType)
			{
				this.m_DT_BaseSolRank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
			}
			this.m_DT_BaseSolRank.SetTexture(solLargeGradeImg);
			this.m_DT_ResultSolRank.Visible = false;
		}
		legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(soldierInfo.GetCharKind(), solgrade);
		solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(soldierInfo.GetCharKind(), solgrade);
		if (solLargeGradeImg != null)
		{
			this.m_DT_ResultSolRank.Visible = true;
			if (0 < legendType)
			{
				this.m_DT_ResultSolRank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
			}
			this.m_DT_ResultSolRank.SetTexture(solLargeGradeImg);
		}
		string text = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("290"),
			"targetname",
			soldierInfo.GetName(),
			"count1",
			soldierInfo.GetLevel().ToString(),
			"count2",
			soldierInfo.GetSolMaxLevel().ToString()
		});
		this.m_LB_BaseSolName.SetText(text);
		this.m_LB_ResultSolName.SetText(text);
		int num = soldierInfo.GetSeason() + 1;
		if (num != 0)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3274"),
				"season",
				num
			});
		}
		this.m_Label_BaseSeason.SetText(text);
		this.m_Label_ResultSeason.SetText(text);
		MYTH_EVOLUTION myth_EvolutionSeason = NrTSingleton<NrTableMyth_EvolutionManager>.Instance.GetMyth_EvolutionSeason((byte)num);
		if (myth_EvolutionSeason != null)
		{
			this.m_Label_MythEvolution_Essence_Num2.SetText(ANNUALIZED.Convert(myth_EvolutionSeason.m_i32SpendItemNum1));
			this.m_Label_MythEvolution_DragonHeart_Num2.SetText(ANNUALIZED.Convert(myth_EvolutionSeason.m_i32SpendItemNum2));
			int charKindbyMythSkillUnique = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindbyMythSkillUnique(soldierInfo.GetCharKind(), 0);
			if (charKindbyMythSkillUnique > 0)
			{
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(charKindbyMythSkillUnique);
				if (battleSkillBase != null)
				{
					text = battleSkillBase.m_waSkillName;
					this.m_LB_MythSkillname1.SetText(text);
					this.m_LB_MythSkillname2.SetText(text);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3551"),
						"skilllevel",
						0
					});
					this.m_LB_MythSkillLevel1.SetText(text);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3551"),
						"skilllevel",
						1
					});
					this.m_LB_MythSkillLevel2.SetText(text);
				}
				UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(charKindbyMythSkillUnique);
				if (battleSkillIconTexture != null)
				{
					this.m_DT_MythSkillIcon1_2.SetTexture(battleSkillIconTexture);
					this.m_DT_MythSkillIcon2_2.SetTexture(battleSkillIconTexture);
				}
			}
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_MYTH_EVOLUTION_TIME);
			long curTime = PublicMethod.GetCurTime();
			if (curTime < charSubData)
			{
				base.SetShowLayer(3, true);
				long iSec = charSubData - curTime;
				long totalDayFromSec = PublicMethod.GetTotalDayFromSec(iSec);
				long hourFromSec = PublicMethod.GetHourFromSec(iSec);
				long minuteFromSec = PublicMethod.GetMinuteFromSec(iSec);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3443"),
					"day",
					totalDayFromSec,
					"hour",
					hourFromSec,
					"min",
					minuteFromSec
				});
				this.m_LB_MythTime.SetText(text);
			}
			else
			{
				base.SetShowLayer(3, false);
			}
		}
	}

	private void OnClickMythTime(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		NkSoldierInfo soldierInfo = this.GetSoldierInfo(this.m_i64BaseSolID);
		if (soldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		Myth_Evolution_Time_DLG myth_Evolution_Time_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_TIME_DLG) as Myth_Evolution_Time_DLG;
		if (myth_Evolution_Time_DLG != null)
		{
			myth_Evolution_Time_DLG.InitSet(MYTH_TYPE.MYTHTYPE_EVOLUTION, soldierInfo.GetCharKind(), soldierInfo.GetSolID());
		}
	}

	public NkSoldierInfo GetSoldierInfo(long SoldID)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkSoldierInfo nkSoldierInfo = soldierList.GetSoldierInfoBySolID(SoldID);
		if (nkSoldierInfo == null)
		{
			nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySoldierInfoBySolID(SoldID);
		}
		return nkSoldierInfo;
	}
}
