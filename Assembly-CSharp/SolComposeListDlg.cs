using GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class SolComposeListDlg : Form
{
	public enum SOL_LIST_INSERT_TYPE
	{
		WAIT,
		ALL,
		BATTLE
	}

	public enum SOL_LIST_SORT_TYPE
	{
		NAME,
		LEVEL_HIGH,
		LEVEL_LOW,
		RANK_HIGH,
		RANK_LOW
	}

	private string[] ddlListKey1 = new string[]
	{
		"123",
		"120",
		"121"
	};

	private string[] ddlListKey2 = new string[]
	{
		"1890",
		"1888",
		"1889",
		"1891",
		"1892"
	};

	private NewListBox ComposeNewListBox;

	private int ListCnt;

	private DropDownList ddList1;

	private DropDownList ddList2;

	private Label lbSubNum;

	private Label lbCostMoney;

	private Label lbCostName;

	private Button btnOk;

	private bool m_bMainSelect;

	private List<NkSoldierInfo> mSortList = new List<NkSoldierInfo>();

	private List<long> mCheckList = new List<long>();

	private SOLCOMPOSE_TYPE m_eShowType;

	public SOLCOMPOSE_TYPE ShowType
	{
		get
		{
			return this.m_eShowType;
		}
		set
		{
			this.m_eShowType = value;
			this.SetCostName();
		}
	}

	public SolComposeListDlg.SOL_LIST_INSERT_TYPE INSERT_TYPE
	{
		get
		{
			return (SolComposeListDlg.SOL_LIST_INSERT_TYPE)PlayerPrefs.GetInt("compose_sol_ddl_insert_index");
		}
		set
		{
			PlayerPrefs.SetInt("compose_sol_ddl_insert_index", (int)value);
		}
	}

	public SolComposeListDlg.SOL_LIST_SORT_TYPE SORT_TYPE
	{
		get
		{
			return (SolComposeListDlg.SOL_LIST_SORT_TYPE)PlayerPrefs.GetInt("compose_sol_ddl_index");
		}
		set
		{
			PlayerPrefs.SetInt("compose_sol_ddl_index", (int)value);
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/dlg_solcomposelist", G_ID.SOLCOMPOSE_LIST_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.lbSubNum = (base.GetControl("Label_SelectionSolNum") as Label);
		this.lbCostMoney = (base.GetControl("Label_Money") as Label);
		this.btnOk = (base.GetControl("BT_OK") as Button);
		Button expr_48 = this.btnOk;
		expr_48.Click = (EZValueChangedDelegate)Delegate.Combine(expr_48.Click, new EZValueChangedDelegate(this.ClickOk));
		this.ComposeNewListBox = (base.GetControl("ListBox_ComposeList") as NewListBox);
		this.ComposeNewListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickListBox));
		this.ComposeNewListBox.AddDoubleClickDelegate(new EZValueChangedDelegate(this.BtnDblClickListBox));
		this.ddList1 = (base.GetControl("DDL_Sort01") as DropDownList);
		this.ddList1.AddValueChangedDelegate(new EZValueChangedDelegate(this.ChangeInsertListDDL));
		if (null != this.ddList1)
		{
			this.ddList1.SetViewArea(this.ddlListKey1.Length);
			this.ddList1.Clear();
			int num = 0;
			string[] array = this.ddlListKey1;
			for (int i = 0; i < array.Length; i++)
			{
				string strTextKey = array[i];
				ListItem listItem = new ListItem();
				listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey));
				listItem.Key = num++;
				this.ddList1.Add(listItem);
			}
			this.ddList1.SetIndex((int)this.INSERT_TYPE);
			this.ddList1.RepositionItems();
		}
		this.ddList2 = (base.GetControl("DDL_Sort02") as DropDownList);
		this.ddList2.AddValueChangedDelegate(new EZValueChangedDelegate(this.ChangeSortDDL));
		if (null != this.ddList2)
		{
			this.ddList2.SetViewArea(this.ddlListKey2.Length);
			this.ddList2.Clear();
			int num2 = 0;
			string[] array2 = this.ddlListKey2;
			for (int j = 0; j < array2.Length; j++)
			{
				string strTextKey2 = array2[j];
				ListItem listItem2 = new ListItem();
				listItem2.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey2));
				listItem2.Key = num2++;
				this.ddList2.Add(listItem2);
			}
			this.ddList2.SetIndex((int)this.SORT_TYPE);
			this.ddList2.RepositionItems();
		}
		this.lbCostName = (base.GetControl("Label_ComploseCost") as Label);
		this.InitData();
		base.SetScreenCenter();
	}

	public override void InitData()
	{
	}

	public static void LoadSelectList(bool bMainSelect, SOLCOMPOSE_TYPE ShowType = SOLCOMPOSE_TYPE.COMPOSE)
	{
		SolComposeListDlg solComposeListDlg = (SolComposeListDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_LIST_DLG);
		if (solComposeListDlg != null)
		{
			solComposeListDlg.InitData();
			solComposeListDlg.ShowType = ShowType;
			solComposeListDlg.InitList(bMainSelect);
		}
	}

	private void InsertList(SolComposeListDlg.SOL_LIST_INSERT_TYPE eIndex)
	{
		if (SolComposeMainDlg.Instance == null)
		{
			this.OnClose();
			return;
		}
		this.mSortList.Clear();
		this.mCheckList.Clear();
		bool flag = eIndex == SolComposeListDlg.SOL_LIST_INSERT_TYPE.ALL || eIndex == SolComposeListDlg.SOL_LIST_INSERT_TYPE.BATTLE;
		bool flag2 = eIndex == SolComposeListDlg.SOL_LIST_INSERT_TYPE.ALL || eIndex == SolComposeListDlg.SOL_LIST_INSERT_TYPE.WAIT;
		if (!this.m_bMainSelect)
		{
			flag = false;
			flag2 = true;
		}
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo != null)
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (flag && charPersonInfo != null)
			{
				for (int i = 0; i < 6; i++)
				{
					NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
					if (soldierInfo != null)
					{
						if (soldierInfo.GetSolID() > 0L)
						{
							if (this.m_bMainSelect || !soldierInfo.IsLeader())
							{
								if (this.m_eShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE && this.m_bMainSelect)
								{
									if (soldierInfo.GetGrade() < 6)
									{
										goto IL_12C;
									}
									if (soldierInfo.GetGrade() >= 9)
									{
										goto IL_12C;
									}
								}
								if (this.m_eShowType != SOLCOMPOSE_TYPE.COMPOSE || this.IsAddComposeList(soldierInfo))
								{
									this.mSortList.Add(soldierInfo);
								}
							}
						}
					}
					IL_12C:;
				}
			}
			if (flag2)
			{
				NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
				foreach (NkSoldierInfo current in readySolList.GetList().Values)
				{
					if (current.GetSolPosType() == 0)
					{
						switch (this.m_eShowType)
						{
						case SOLCOMPOSE_TYPE.COMPOSE:
							if (!this.IsAddComposeList(current))
							{
								continue;
							}
							if (!this.m_bMainSelect && current.GetGrade() >= 6)
							{
								continue;
							}
							if (current.IsAtbCommonFlag(1L))
							{
								continue;
							}
							break;
						case SOLCOMPOSE_TYPE.EXTRACT:
							if (current.GetGrade() < 5)
							{
								continue;
							}
							if (current.GetGrade() >= 6)
							{
								continue;
							}
							if (current.IsAtbCommonFlag(1L))
							{
								continue;
							}
							break;
						case SOLCOMPOSE_TYPE.TRANSCENDENCE:
							if (current.GetGrade() < 6)
							{
								continue;
							}
							if (!this.m_bMainSelect)
							{
								if (current.IsAtbCommonFlag(1L))
								{
									continue;
								}
							}
							else if (current.GetGrade() >= 9)
							{
								continue;
							}
							break;
						}
						this.mSortList.Add(current);
					}
				}
			}
			if (this.m_bMainSelect)
			{
				List<long> list = new List<long>();
				list.AddRange(SolComposeMainDlg.Instance.SUB_ARRAY);
				if (list.Count != 0)
				{
					List<NkSoldierInfo> list2 = new List<NkSoldierInfo>();
					foreach (NkSoldierInfo current2 in this.mSortList)
					{
						for (int j = 0; j < list.Count; j++)
						{
							if (current2.GetSolID() == list[j])
							{
								list2.Add(current2);
							}
						}
					}
					for (int k = 0; k < list2.Count; k++)
					{
						this.mSortList.Remove(list2[k]);
					}
					list2.Clear();
				}
			}
			if (!this.m_bMainSelect && SolComposeMainDlg.Instance != null)
			{
				this.mCheckList.AddRange(SolComposeMainDlg.Instance.SUB_ARRAY);
			}
		}
		this.ddList1.SetIndex((int)eIndex);
		this.ddList1.RepositionItems();
		this.ChangeSortDDL(null);
	}

	private ListItem GetItem(NkSoldierInfo kSolInfo)
	{
		for (int i = 0; i < this.ListCnt; i++)
		{
			ListItem listItem = this.ComposeNewListBox.GetItem(i) as ListItem;
			if (listItem != null && (long)listItem.Key == kSolInfo.GetSolID())
			{
				return listItem;
			}
		}
		return null;
	}

	private void UpdateList()
	{
		this.ComposeNewListBox.Clear();
		this.ListCnt = 0;
		foreach (NkSoldierInfo current in this.mSortList)
		{
			if (this.UpdateSolList(current))
			{
				this.ListCnt++;
			}
		}
		this.ComposeNewListBox.RepositionItems();
		if (SolComposeMainDlg.Instance != null)
		{
			this.lbSubNum.SetText(string.Format("{0}/{1}", SolComposeMainDlg.Instance.SELECT_COUNT, 50));
			this.lbCostMoney.SetText(string.Format("{0:###,###,###,##0}", SolComposeMainDlg.Instance.COST));
		}
	}

	private bool UpdateSolList(NkSoldierInfo kSolInfo)
	{
		if (!kSolInfo.IsValid() || SolComposeMainDlg.Instance == null)
		{
			return false;
		}
		if (SolComposeMainDlg.Instance.ContainBaseSoldier(kSolInfo.GetSolID()))
		{
			return false;
		}
		string text = string.Empty;
		NewListItem newListItem = new NewListItem(this.ComposeNewListBox.ColumnNum, true);
		if (newListItem == null)
		{
			return false;
		}
		EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(kSolInfo.GetCharKind(), kSolInfo.GetGrade());
		if (eventHeroCharCode != null)
		{
			newListItem.EventMark = true;
			newListItem.SetListItemData(3, "Win_I_EventSol", null, null, null);
		}
		else
		{
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(kSolInfo.GetCharKind(), (int)kSolInfo.GetGrade());
			if (legendFrame != null)
			{
				newListItem.SetListItemData(3, legendFrame, null, null, null);
			}
		}
		newListItem.SetListItemData(4, kSolInfo.GetListSolInfo(false), null, null, null);
		string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(kSolInfo.GetCharKind(), (int)kSolInfo.GetGrade(), kSolInfo.GetName());
		newListItem.SetListItemData(5, legendName, null, null, null);
		long num = kSolInfo.GetExp() - kSolInfo.GetCurBaseExp();
		long num2 = kSolInfo.GetNextExp() - kSolInfo.GetCurBaseExp();
		float num3 = 1f;
		if (!kSolInfo.IsMaxLevel())
		{
			num3 = ((float)num2 - (float)kSolInfo.GetRemainExp()) / (float)num2;
			if (num3 > 1f)
			{
				num3 = 1f;
			}
			if (0f > num3)
			{
				num3 = 0f;
			}
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
			"count1",
			kSolInfo.GetLevel().ToString(),
			"count2",
			kSolInfo.GetSolMaxLevel().ToString()
		});
		newListItem.SetListItemData(6, text, null, null, null);
		newListItem.SetListItemData(0, "Win_T_ReputelPrgBG", null, null, null);
		newListItem.SetListItemData(1, "Com_T_GauWaPr4", 270f * num3, null, null);
		if (kSolInfo.IsMaxLevel())
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
				"exp",
				num.ToString(),
				"maxexp",
				num2.ToString()
			});
		}
		newListItem.SetListItemData(2, text, null, null, null);
		newListItem.SetListItemData(7, false);
		if (!this.m_bMainSelect)
		{
			foreach (long current in this.mCheckList)
			{
				if (kSolInfo.GetSolID() == current)
				{
					newListItem.SetListItemData(7, true);
					newListItem.SetListItemData(7, "Com_I_Check", null, null, null);
					break;
				}
			}
		}
		if (!kSolInfo.IsAwakening())
		{
			newListItem.SetListItemData(8, false);
		}
		newListItem.SetListItemData(9, string.Empty, null, new EZValueChangedDelegate(this.ClickSolDetailInfo), null);
		if (kSolInfo.IsAtbCommonFlag(1L))
		{
			newListItem.SetListItemData(10, true);
		}
		else
		{
			newListItem.SetListItemData(10, false);
		}
		newListItem.Data = kSolInfo;
		this.ComposeNewListBox.Add(newListItem);
		return true;
	}

	private void UpdateSellData()
	{
		long num = 0L;
		foreach (long current in this.mCheckList)
		{
			NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(current);
			num += SolComposeMainDlg.GetSelCost(soldierInfo);
		}
		this.lbSubNum.SetText(string.Format("{0}/{1}", this.mCheckList.Count, 50));
		this.lbCostMoney.SetText(Protocol_Item.Money_Format(num));
	}

	private void UpdateComposeData()
	{
		long num = 0L;
		foreach (long current in this.mCheckList)
		{
			NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(current);
			num += SolComposeMainDlg.GetComposeCost(soldierInfo);
		}
		this.lbSubNum.SetText(string.Format("{0}/{1}", this.mCheckList.Count, 50));
		this.lbCostMoney.SetText(string.Format("{0:###,###,###,##0}", num));
	}

	private int CompareName(NkSoldierInfo a, NkSoldierInfo b)
	{
		int num = SolComposeListDlg.COMPARE_NAME(a, b);
		if (num == 0)
		{
			return this.COMPARE_LEVEL(a, b);
		}
		return num;
	}

	private int CompareLevel_High(NkSoldierInfo a, NkSoldierInfo b)
	{
		return this.COMPARE_LEVEL(a, b);
	}

	private int CompareLevel_Low(NkSoldierInfo a, NkSoldierInfo b)
	{
		return this.COMPARE_LEVEL(b, a);
	}

	private int CompareCombat_High(NkSoldierInfo a, NkSoldierInfo b)
	{
		return this.COMPARE_COMBAT(a, b);
	}

	private int CompareCombat_Low(NkSoldierInfo a, NkSoldierInfo b)
	{
		return this.COMPARE_COMBAT(b, a);
	}

	public static int CompareGrade_High(NkSoldierInfo a, NkSoldierInfo b)
	{
		int num = SolComposeListDlg.COMPARE_GRADE(a, b);
		if (num == 0)
		{
			return SolComposeListDlg.COMPARE_NAME(a, b);
		}
		return num;
	}

	public static int CompareGrade_Low(NkSoldierInfo a, NkSoldierInfo b)
	{
		int num = SolComposeListDlg.COMPARE_GRADE(b, a);
		if (num == 0)
		{
			return SolComposeListDlg.COMPARE_NAME(b, a);
		}
		return num;
	}

	private int COMPARE_SELECT(NkSoldierInfo a, NkSoldierInfo b)
	{
		if (this.IsContainSelect(a.GetSolID()))
		{
			return -1;
		}
		if (this.IsContainSelect(b.GetSolID()))
		{
			return 1;
		}
		return 0;
	}

	public static int COMPARE_NAME(NkSoldierInfo a, NkSoldierInfo b)
	{
		string name = a.GetCharKindInfo().GetName();
		string name2 = b.GetCharKindInfo().GetName();
		return string.Compare(name, name2);
	}

	private int COMPARE_COMBAT(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetCombatPower().CompareTo(a.GetCombatPower());
	}

	private int COMPARE_LEVEL(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetLevel().CompareTo(a.GetLevel());
	}

	public static int COMPARE_GRADE(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetGrade().CompareTo(a.GetGrade());
	}

	private void ChangeInsertListDDL(IUIObject obj)
	{
		DropDownList dropDownList = this.ddList1;
		if (obj != null)
		{
			ListItem listItem = dropDownList.SelectedItem.Data as ListItem;
			this.INSERT_TYPE = (SolComposeListDlg.SOL_LIST_INSERT_TYPE)((int)listItem.Key);
		}
		this.InsertList(this.INSERT_TYPE);
	}

	private void ChangeSortDDL(IUIObject obj)
	{
		DropDownList dropDownList = this.ddList2;
		if (obj != null)
		{
			ListItem listItem = dropDownList.SelectedItem.Data as ListItem;
			this.SORT_TYPE = (SolComposeListDlg.SOL_LIST_SORT_TYPE)((int)listItem.Key);
			dropDownList.SetIndex((int)this.SORT_TYPE);
			dropDownList.RepositionItems();
		}
		this.mSortList.Sort(new Comparison<NkSoldierInfo>(this.COMPARE_SELECT));
		switch (this.SORT_TYPE)
		{
		case SolComposeListDlg.SOL_LIST_SORT_TYPE.NAME:
			this.mSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareName));
			break;
		case SolComposeListDlg.SOL_LIST_SORT_TYPE.LEVEL_HIGH:
			this.mSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevel_High));
			break;
		case SolComposeListDlg.SOL_LIST_SORT_TYPE.LEVEL_LOW:
			this.mSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevel_Low));
			break;
		case SolComposeListDlg.SOL_LIST_SORT_TYPE.RANK_HIGH:
			this.mSortList.Sort(new Comparison<NkSoldierInfo>(SolComposeListDlg.CompareGrade_High));
			break;
		case SolComposeListDlg.SOL_LIST_SORT_TYPE.RANK_LOW:
			this.mSortList.Sort(new Comparison<NkSoldierInfo>(SolComposeListDlg.CompareGrade_Low));
			break;
		}
		this.UpdateList();
	}

	private bool IsContainSelect(long SolID)
	{
		return this.mCheckList.Contains(SolID);
	}

	private void BtnClickListBox(IUIObject obj)
	{
		UIListItemContainer selectedItem = this.ComposeNewListBox.SelectedItem;
		if (null == selectedItem)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)selectedItem.data;
		if (nkSoldierInfo == null)
		{
			return;
		}
		if (this.IsContainSelect(nkSoldierInfo.GetSolID()))
		{
			this.mCheckList.Remove(nkSoldierInfo.GetSolID());
		}
		else
		{
			if (!this.m_bMainSelect && nkSoldierInfo.IsAtbCommonFlag(1L))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("724"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				return;
			}
			if (this.m_eShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
			{
				foreach (long current in this.mCheckList)
				{
					NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(current);
					this.UpdateSolListCheck(soldierInfo, false);
				}
				this.mCheckList.Clear();
			}
			this.mCheckList.Add(nkSoldierInfo.GetSolID());
		}
		if (this.m_bMainSelect)
		{
			this.SelectFinish();
			return;
		}
		if (this.IsContainSelect(nkSoldierInfo.GetSolID()))
		{
			this.UpdateSolListCheck(nkSoldierInfo, true);
		}
		else
		{
			this.UpdateSolListCheck(nkSoldierInfo, false);
		}
		SOLCOMPOSE_TYPE eShowType = this.m_eShowType;
		if (eShowType != SOLCOMPOSE_TYPE.COMPOSE)
		{
			if (eShowType == SOLCOMPOSE_TYPE.SELL)
			{
				this.UpdateSellData();
			}
		}
		else
		{
			this.UpdateComposeData();
		}
	}

	private void BtnDblClickListBox(IUIObject obj)
	{
		this.BtnClickListBox(null);
	}

	public void InitList(bool bMainSelect)
	{
		SolComposeListDlg.SOL_LIST_INSERT_TYPE eIndex = this.INSERT_TYPE;
		if (null != this.ddList1)
		{
			if (bMainSelect && this.ShowType == SOLCOMPOSE_TYPE.COMPOSE)
			{
				this.ddList1.SetViewArea(this.ddlListKey1.Length);
				this.ddList1.Clear();
				int num = 0;
				string[] array = this.ddlListKey1;
				for (int i = 0; i < array.Length; i++)
				{
					string strTextKey = array[i];
					ListItem listItem = new ListItem();
					listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey));
					listItem.Key = num++;
					this.ddList1.Add(listItem);
				}
				this.ddList1.SetIndex((int)this.INSERT_TYPE);
				this.ddList1.RepositionItems();
			}
			else if (bMainSelect && this.ShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
			{
				this.ddList1.SetViewArea(this.ddlListKey1.Length);
				this.ddList1.Clear();
				int num2 = 0;
				string[] array2 = this.ddlListKey1;
				for (int j = 0; j < array2.Length; j++)
				{
					string strTextKey2 = array2[j];
					ListItem listItem2 = new ListItem();
					listItem2.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey2));
					listItem2.Key = num2++;
					this.ddList1.Add(listItem2);
				}
				this.ddList1.SetIndex((int)this.INSERT_TYPE);
				this.ddList1.RepositionItems();
			}
			else
			{
				this.ddList1.SetViewArea(1);
				this.ddList1.Clear();
				int num3 = 0;
				ListItem listItem3 = new ListItem();
				listItem3.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.ddlListKey1[0]));
				listItem3.Key = num3++;
				this.ddList1.Add(listItem3);
				this.ddList1.SetIndex((int)this.INSERT_TYPE);
				this.ddList1.RepositionItems();
			}
		}
		if (!bMainSelect || this.ShowType == SOLCOMPOSE_TYPE.EXTRACT || this.ShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
		{
			eIndex = SolComposeListDlg.SOL_LIST_INSERT_TYPE.WAIT;
		}
		UIScrollList arg_229_0 = this.ddList1;
		this.m_bMainSelect = bMainSelect;
		arg_229_0.controlIsEnabled = bMainSelect;
		this.InsertList(eIndex);
	}

	private void ClickOk(IUIObject obj)
	{
		this.SelectFinish();
	}

	private void SelectFinish()
	{
		if (SolComposeMainDlg.Instance != null)
		{
			if (this.m_bMainSelect)
			{
				SolComposeMainDlg.Instance.SelectBase(this.mCheckList);
			}
			else
			{
				SolComposeMainDlg.Instance.SelectSub(this.mCheckList);
			}
		}
		base.CloseNow();
	}

	private void SetCostName()
	{
		SOLCOMPOSE_TYPE eShowType = this.m_eShowType;
		if (eShowType != SOLCOMPOSE_TYPE.COMPOSE)
		{
			if (eShowType == SOLCOMPOSE_TYPE.SELL)
			{
				this.lbCostName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("34"));
			}
		}
		else
		{
			this.lbCostName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1728"));
		}
	}

	private void UpdateSolListCheck(NkSoldierInfo kSolInfo, bool bClickSoldier)
	{
		if (!kSolInfo.IsValid())
		{
			return;
		}
		if (SolComposeMainDlg.Instance == null || SolComposeMainDlg.Instance.ContainBaseSoldier(kSolInfo.GetSolID()))
		{
			return;
		}
		string text = string.Empty;
		int i = 0;
		while (i < this.ComposeNewListBox.Count)
		{
			IUIListObject item = this.ComposeNewListBox.GetItem(i);
			NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)item.Data;
			if (nkSoldierInfo.GetSolID() != kSolInfo.GetSolID())
			{
				i++;
			}
			else
			{
				NewListItem newListItem = new NewListItem(this.ComposeNewListBox.ColumnNum, true);
				if (newListItem == null)
				{
					return;
				}
				EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(kSolInfo.GetCharKind(), kSolInfo.GetGrade());
				if (eventHeroCharCode != null)
				{
					newListItem.EventMark = true;
					newListItem.SetListItemData(3, "Win_I_EventSol", null, null, null);
				}
				else
				{
					UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(kSolInfo.GetCharKind(), (int)kSolInfo.GetGrade());
					if (legendFrame != null)
					{
						newListItem.SetListItemData(3, legendFrame, null, null, null);
					}
				}
				newListItem.SetListItemData(4, kSolInfo.GetListSolInfo(false), null, null, null);
				string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(kSolInfo.GetCharKind(), (int)kSolInfo.GetGrade(), kSolInfo.GetName());
				newListItem.SetListItemData(5, legendName, null, null, null);
				long num = kSolInfo.GetExp() - kSolInfo.GetCurBaseExp();
				long num2 = kSolInfo.GetNextExp() - kSolInfo.GetCurBaseExp();
				float num3 = 1f;
				if (!kSolInfo.IsMaxLevel())
				{
					num3 = ((float)num2 - (float)kSolInfo.GetRemainExp()) / (float)num2;
					if (num3 > 1f)
					{
						num3 = 1f;
					}
					if (0f > num3)
					{
						num3 = 0f;
					}
				}
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
					"count1",
					kSolInfo.GetLevel().ToString(),
					"count2",
					kSolInfo.GetSolMaxLevel().ToString()
				});
				newListItem.SetListItemData(6, text, null, null, null);
				newListItem.SetListItemData(0, "Win_T_ReputelPrgBG", null, null, null);
				newListItem.SetListItemData(1, "Com_T_GauWaPr4", 270f * num3, null, null);
				if (kSolInfo.IsMaxLevel())
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
						"exp",
						num.ToString(),
						"maxexp",
						num2.ToString()
					});
				}
				newListItem.SetListItemData(2, text, null, null, null);
				if (bClickSoldier)
				{
					newListItem.SetListItemData(7, true);
					newListItem.SetListItemData(7, "Com_I_Check", null, null, null);
				}
				else
				{
					newListItem.SetListItemData(7, false);
					newListItem.SetListItemData(7, string.Empty, null, null, null);
				}
				if (!kSolInfo.IsAwakening())
				{
					newListItem.SetListItemData(8, false);
				}
				newListItem.SetListItemData(9, string.Empty, null, new EZValueChangedDelegate(this.ClickSolDetailInfo), null);
				if (kSolInfo.IsAtbCommonFlag(1L))
				{
					newListItem.SetListItemData(10, true);
				}
				else
				{
					newListItem.SetListItemData(10, false);
				}
				newListItem.Data = kSolInfo;
				this.ComposeNewListBox.RemoveAdd(i, newListItem);
				this.ComposeNewListBox.RepositionItems();
				break;
			}
		}
	}

	public bool IsAddComposeList(NkSoldierInfo kSolInfo)
	{
		return !this.m_bMainSelect || !kSolInfo.IsMaxLevel() || !kSolInfo.IsMaxGrade();
	}

	public void ClickSolDetailInfo(IUIObject obj)
	{
		UIListItemContainer selectedItem = this.ComposeNewListBox.SelectedItem;
		if (null == selectedItem)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)selectedItem.data;
		if (nkSoldierInfo == null)
		{
			return;
		}
		SolDetailinfoDlg solDetailinfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAILINFO_DLG) as SolDetailinfoDlg;
		if (solDetailinfoDlg != null)
		{
			solDetailinfoDlg.SetData(ref nkSoldierInfo);
			solDetailinfoDlg.SetLocationByForm(this);
			solDetailinfoDlg.SetFocus();
		}
	}

	public void RefreshSoldierInfo(NkSoldierInfo pkSoldierInfo, bool bCheckListRemove)
	{
		if (pkSoldierInfo == null)
		{
			return;
		}
		if (bCheckListRemove)
		{
			this.mCheckList.Remove(pkSoldierInfo.GetSolID());
		}
		if (this.IsContainSelect(pkSoldierInfo.GetSolID()))
		{
			this.UpdateSolListCheck(pkSoldierInfo, true);
		}
		else
		{
			this.UpdateSolListCheck(pkSoldierInfo, false);
		}
	}
}
