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
		BATTLE,
		WAREHOUSE
	}

	public enum SOL_LIST_SORT_TYPE
	{
		NAME,
		LEVEL_HIGH,
		LEVEL_LOW,
		RANK_HIGH,
		RANK_LOW
	}

	protected string[] ddlListKey1 = new string[]
	{
		"123",
		"120",
		"121",
		"2179"
	};

	private string[] ddlListKey2 = new string[]
	{
		"1890",
		"1888",
		"1889",
		"1891",
		"1892"
	};

	protected NewListBox ComposeNewListBox;

	private int ListCnt;

	protected DropDownList ddList1;

	protected DropDownList ddList2;

	private Label lbSubNum;

	private Label lbCostMoney;

	private Label lbCostName;

	private Button btnOk;

	protected bool m_bMainSelect;

	private Button m_btClose;

	protected List<NkSoldierInfo> mSortList = new List<NkSoldierInfo>();

	private List<long> mCheckList = new List<long>();

	private int guideWinID = -1;

	private UIButton _Touch;

	private SOLCOMPOSE_TYPE m_eShowType;

	private int m_kOldSolNum;

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

	public UIHelper.eSolSortOrder SORT_TYPE
	{
		get
		{
			return (UIHelper.eSolSortOrder)PlayerPrefs.GetInt("compose_new_sol_ddl_index");
		}
		set
		{
			PlayerPrefs.SetInt("compose_new_sol_ddl_index", (int)value);
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
		this.ComposeNewListBox.AddScrollDelegate(new EZScrollDelegate(this.ChangeSolInfo));
		this.ComposeNewListBox.ReUse = true;
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
			this.ddList2.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.ddlListKey2[0]), 4);
			this.ddList2.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.ddlListKey2[1]), 2);
			this.ddList2.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.ddlListKey2[2]), 3);
			this.ddList2.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.ddlListKey2[3]), 5);
			this.ddList2.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.ddlListKey2[4]), 6);
			int index = 0;
			if (this.SORT_TYPE == UIHelper.eSolSortOrder.SORTORDER_NAME)
			{
				index = 0;
			}
			else if (this.SORT_TYPE == UIHelper.eSolSortOrder.SORTORDER_LEVELDESC)
			{
				index = 1;
			}
			else if (this.SORT_TYPE == UIHelper.eSolSortOrder.SORTORDER_LEVELASC)
			{
				index = 2;
			}
			else if (this.SORT_TYPE == UIHelper.eSolSortOrder.SORTORDER_GRADEDESC)
			{
				index = 3;
			}
			else if (this.SORT_TYPE == UIHelper.eSolSortOrder.SORTORDER_GRADEASC)
			{
				index = 4;
			}
			this.ddList2.SetIndex(index);
			this.ddList2.RepositionItems();
		}
		this.lbCostName = (base.GetControl("Label_ComploseCost") as Label);
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this.InitData();
		base.SetScreenCenter();
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
		this.HideTouch();
		base.OnClose();
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

	public static void LoadSelectExtractList(SOLCOMPOSE_TYPE ShowType = SOLCOMPOSE_TYPE.EXTRACT)
	{
		SolComposeListDlg solComposeListDlg = (SolComposeListDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_LIST_DLG);
		if (solComposeListDlg != null)
		{
			solComposeListDlg.InitData();
			solComposeListDlg.ShowType = ShowType;
			solComposeListDlg.InitExtractList();
		}
	}

	public static void LoadMythEvolution(SOLCOMPOSE_TYPE ShowType = SOLCOMPOSE_TYPE.MYTHEVOLUTION)
	{
		SolComposeListDlg solComposeListDlg = (SolComposeListDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_LIST_DLG);
		if (solComposeListDlg != null)
		{
			solComposeListDlg.InitData();
			solComposeListDlg.ShowType = ShowType;
			solComposeListDlg.InitMythEvolutionList();
		}
	}

	public void InitExtractList()
	{
		SolComposeListDlg.SOL_LIST_INSERT_TYPE iNSERT_TYPE = this.INSERT_TYPE;
		if (null != this.ddList1)
		{
			this.ddList1.SetViewArea(1);
			this.ddList1.Clear();
			int num = 0;
			ListItem listItem = new ListItem();
			listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.ddlListKey1[0]));
			listItem.Key = num++;
			this.ddList1.Add(listItem);
			this.ddList1.SetIndex((int)this.INSERT_TYPE);
			this.ddList1.RepositionItems();
		}
		this.ddList1.controlIsEnabled = (this.m_bMainSelect = false);
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolID() != NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetFaceSolID())
			{
				if (current.GetSolPosType() == 0 && current.GetGrade() == 5)
				{
					if (!current.IsAtbCommonFlag(1L))
					{
						this.mSortList.Add(current);
					}
				}
			}
		}
		if (!this.m_bMainSelect && SolComposeMainDlg.Instance != null)
		{
			this.mCheckList.AddRange(SolComposeMainDlg.Instance.SUB_EXTRACTARRAY);
		}
		this.ddList1.SetIndex((int)iNSERT_TYPE);
		this.ddList1.RepositionItems();
		this.ChangeSortDDL(null);
	}

	private void InsertMythList(SolComposeListDlg.SOL_LIST_INSERT_TYPE eIndex)
	{
		this.mSortList.Clear();
		this.mCheckList.Clear();
		bool flag = eIndex == SolComposeListDlg.SOL_LIST_INSERT_TYPE.ALL || eIndex == SolComposeListDlg.SOL_LIST_INSERT_TYPE.BATTLE;
		bool flag2 = eIndex == SolComposeListDlg.SOL_LIST_INSERT_TYPE.ALL || eIndex == SolComposeListDlg.SOL_LIST_INSERT_TYPE.WAIT;
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
							if (soldierInfo.GetGrade() >= 6 && soldierInfo.GetGrade() < 10)
							{
								this.mSortList.Add(soldierInfo);
							}
						}
					}
				}
			}
			if (flag2)
			{
				NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
				foreach (NkSoldierInfo current in readySolList.GetList().Values)
				{
					if (current.GetSolID() != NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetFaceSolID())
					{
						if (current.GetSolPosType() == 0 && current.GetGrade() >= 6 && current.GetGrade() < 10)
						{
							this.mSortList.Add(current);
						}
					}
				}
			}
		}
		this.ddList1.SetIndex((int)eIndex);
		this.ddList1.RepositionItems();
		this.ChangeSortDDL(null);
	}

	public void InitMythEvolutionList()
	{
		this.INSERT_TYPE = SolComposeListDlg.SOL_LIST_INSERT_TYPE.BATTLE;
		if (null != this.ddList1)
		{
			this.ddList1.SetViewArea(1);
			this.ddList1.Clear();
			int num = 0;
			ListItem listItem = new ListItem();
			listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.ddlListKey1[0]));
			listItem.Key = num++;
			this.ddList1.Add(listItem);
			listItem = new ListItem();
			listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.ddlListKey1[1]));
			listItem.Key = num++;
			this.ddList1.Add(listItem);
			listItem = new ListItem();
			listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.ddlListKey1[2]));
			listItem.Key = num++;
			this.ddList1.Add(listItem);
			this.ddList1.SetIndex((int)this.INSERT_TYPE);
			this.ddList1.RepositionItems();
		}
		this.ddList1.controlIsEnabled = (this.m_bMainSelect = true);
		this.InsertMythList(this.INSERT_TYPE);
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
		bool flag3 = eIndex == SolComposeListDlg.SOL_LIST_INSERT_TYPE.ALL || eIndex == SolComposeListDlg.SOL_LIST_INSERT_TYPE.WAREHOUSE;
		if (!this.m_bMainSelect)
		{
			flag = false;
			flag2 = true;
			flag3 = false;
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
								if (this.m_eShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
								{
									if (this.m_bMainSelect)
									{
										if (soldierInfo.GetGrade() < 6)
										{
											goto IL_152;
										}
										if (soldierInfo.GetGrade() == 9 || soldierInfo.GetGrade() >= 13)
										{
											goto IL_152;
										}
									}
								}
								else if (this.m_eShowType == SOLCOMPOSE_TYPE.COMPOSE && !this.IsAddComposeList(soldierInfo))
								{
									goto IL_152;
								}
								this.mSortList.Add(soldierInfo);
							}
						}
					}
					IL_152:;
				}
			}
			if (flag2)
			{
				NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
				foreach (NkSoldierInfo current in readySolList.GetList().Values)
				{
					if (this.m_bMainSelect && this.m_eShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
					{
						if (current.GetSolPosType() != 0 && current.GetSolPosType() != 2)
						{
							continue;
						}
					}
					else if (current.GetSolPosType() != 0)
					{
						continue;
					}
					if (this.m_bMainSelect || NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetFaceSolID() != current.GetSolID())
					{
						switch (this.m_eShowType)
						{
						case SOLCOMPOSE_TYPE.COMPOSE:
							if (!this.IsAddComposeList(current))
							{
								continue;
							}
							if (!this.m_bMainSelect)
							{
								if (current.GetGrade() >= 6)
								{
									continue;
								}
								if (current.IsAtbCommonFlag(1L))
								{
									continue;
								}
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
							else if (current.GetGrade() == 9 || current.GetGrade() >= 13)
							{
								continue;
							}
							break;
						}
						this.mSortList.Add(current);
					}
				}
			}
			if (flag3 && this.m_bMainSelect)
			{
				List<NkSoldierInfo> solWarehouseList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouseList();
				foreach (NkSoldierInfo current2 in solWarehouseList)
				{
					if (current2.GetSolPosType() == 5)
					{
						switch (this.m_eShowType)
						{
						case SOLCOMPOSE_TYPE.COMPOSE:
							if (!this.IsAddComposeList(current2))
							{
								continue;
							}
							if (!this.m_bMainSelect && current2.IsAtbCommonFlag(1L))
							{
								continue;
							}
							break;
						case SOLCOMPOSE_TYPE.TRANSCENDENCE:
							if (current2.GetGrade() < 6)
							{
								continue;
							}
							if (current2.GetGrade() == 9 || current2.GetGrade() >= 13)
							{
								continue;
							}
							break;
						}
						this.mSortList.Add(current2);
					}
				}
			}
			if (!this.m_bMainSelect && SolComposeMainDlg.Instance.mBaseSol != null)
			{
				this.mSortList.Remove(SolComposeMainDlg.Instance.mBaseSol);
			}
			if (this.m_bMainSelect)
			{
				List<long> list = new List<long>();
				list.AddRange(SolComposeMainDlg.Instance.SUB_ARRAY);
				if (list.Count != 0)
				{
					List<NkSoldierInfo> list2 = new List<NkSoldierInfo>();
					foreach (NkSoldierInfo current3 in this.mSortList)
					{
						for (int j = 0; j < list.Count; j++)
						{
							if (current3.GetSolID() == list[j])
							{
								list2.Add(current3);
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
			ListItem listItem = this.ComposeNewListBox.GetItem(i).Data as ListItem;
			if (listItem != null && (long)listItem.Key == kSolInfo.GetSolID())
			{
				return listItem;
			}
		}
		return null;
	}

	private void UpdateList(int sortType, bool clear)
	{
		if (!clear && this.ComposeNewListBox.Count > 0 && this.m_kOldSolNum == this.mSortList.Count && this.ComposeNewListBox.Reserve)
		{
			UIHelper.SetSolSort(sortType, this.ComposeNewListBox.GetItems());
		}
		else
		{
			this.ComposeNewListBox.Clear();
			this.ListCnt = 0;
			foreach (NkSoldierInfo current in this.mSortList)
			{
				NewListItem newListItem = this.UpdateSolList(current);
				if (newListItem != null)
				{
					this.ComposeNewListBox.Add(newListItem);
					this.ListCnt++;
				}
			}
		}
		this.m_kOldSolNum = this.mSortList.Count;
		this.ComposeNewListBox.RepositionItems();
		if (SolComposeMainDlg.Instance != null)
		{
			this.lbSubNum.SetText(string.Format("{0}/{1}", SolComposeMainDlg.Instance.SELECT_COUNT, 50));
			this.lbCostMoney.SetText(string.Format("{0:###,###,###,##0}", SolComposeMainDlg.Instance.COST));
		}
	}

	public NewListItem UpdateSolList(NkSoldierInfo kSolInfo)
	{
		if (!kSolInfo.IsValid())
		{
			return null;
		}
		if (this.m_eShowType != SOLCOMPOSE_TYPE.MYTHEVOLUTION)
		{
			if (SolComposeMainDlg.Instance == null)
			{
				return null;
			}
			if (SolComposeMainDlg.Instance.ContainBaseSoldier(kSolInfo.GetSolID()))
			{
				return null;
			}
		}
		string text = string.Empty;
		NewListItem newListItem = new NewListItem(this.ComposeNewListBox.ColumnNum, true, string.Empty);
		if (newListItem == null)
		{
			return null;
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
			else
			{
				newListItem.SetListItemData(3, "Win_T_ItemEmpty", null, null, null);
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
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG) == null)
		{
			newListItem.SetListItemData(9, string.Empty, null, new EZValueChangedDelegate(this.ClickSolDetailInfo), null);
		}
		else
		{
			newListItem.SetListItemData(9, false);
		}
		if (kSolInfo.IsAtbCommonFlag(1L))
		{
			newListItem.SetListItemData(10, true);
		}
		else
		{
			newListItem.SetListItemData(10, false);
		}
		newListItem.Data = kSolInfo;
		return newListItem;
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

	private void UpdateExtractData()
	{
		this.lbSubNum.SetText(string.Format("{0}/{1}", this.mCheckList.Count, 10));
	}

	private void UpdateMythEvolution()
	{
		this.lbSubNum.SetText(string.Empty);
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

	protected virtual void ChangeInsertListDDL(IUIObject obj)
	{
		DropDownList dropDownList = this.ddList1;
		if (obj != null)
		{
			ListItem listItem = dropDownList.SelectedItem.Data as ListItem;
			this.INSERT_TYPE = (SolComposeListDlg.SOL_LIST_INSERT_TYPE)((int)listItem.Key);
		}
		if (this.m_eShowType == SOLCOMPOSE_TYPE.MYTHEVOLUTION)
		{
			this.InsertMythList(this.INSERT_TYPE);
		}
		else
		{
			this.InsertList(this.INSERT_TYPE);
		}
	}

	protected void ChangeSortDDL(IUIObject obj)
	{
		DropDownList dropDownList = this.ddList2;
		bool clear = true;
		if (obj != null)
		{
			ListItem listItem = dropDownList.SelectedItem.Data as ListItem;
			this.SORT_TYPE = (UIHelper.eSolSortOrder)((int)listItem.Key);
			clear = false;
		}
		this.mSortList.Sort(new Comparison<NkSoldierInfo>(this.COMPARE_SELECT));
		switch (this.SORT_TYPE)
		{
		case UIHelper.eSolSortOrder.SORTORDER_LEVELDESC:
			this.mSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevel_High));
			break;
		case UIHelper.eSolSortOrder.SORTORDER_LEVELASC:
			this.mSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevel_Low));
			break;
		case UIHelper.eSolSortOrder.SORTORDER_NAME:
			this.mSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareName));
			break;
		case UIHelper.eSolSortOrder.SORTORDER_GRADEDESC:
			this.mSortList.Sort(new Comparison<NkSoldierInfo>(SolComposeListDlg.CompareGrade_High));
			break;
		case UIHelper.eSolSortOrder.SORTORDER_GRADEASC:
			this.mSortList.Sort(new Comparison<NkSoldierInfo>(SolComposeListDlg.CompareGrade_Low));
			break;
		}
		this.UpdateList((int)this.SORT_TYPE, clear);
	}

	protected bool IsContainSelect(long SolID)
	{
		return this.mCheckList.Contains(SolID);
	}

	private void ChangeSolInfo(IUIObject obj, int index)
	{
		if (index >= this.mSortList.Count)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = this.mSortList[index];
		if (nkSoldierInfo == null)
		{
			return;
		}
		NewListItem newListItem = this.UpdateSolList(nkSoldierInfo);
		if (newListItem == null)
		{
			return;
		}
		this.ComposeNewListBox.UpdateContents(index, newListItem);
		int index2 = index % this.ComposeNewListBox.Count;
		UIListItemContainer item = this.ComposeNewListBox.GetItem(index2);
		if (null != item)
		{
			if (this.IsContainSelect(nkSoldierInfo.GetSolID()))
			{
				item.SetSelected(true);
			}
			else
			{
				item.SetSelected(false);
			}
		}
	}

	protected virtual void BtnClickListBox(IUIObject obj)
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
			if (this.m_eShowType == SOLCOMPOSE_TYPE.EXTRACT)
			{
				if (nkSoldierInfo.GetGrade() < 5 || nkSoldierInfo.GetGrade() >= 6)
				{
					return;
				}
				if (this.mCheckList.Count >= 10)
				{
					return;
				}
			}
			else if (this.m_eShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
			{
				foreach (long current in this.mCheckList)
				{
					NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(current);
					if (soldierInfo != null)
					{
						this.UpdateSolListCheck(soldierInfo, false);
					}
				}
				this.mCheckList.Clear();
			}
			else if (this.m_eShowType == SOLCOMPOSE_TYPE.MYTHEVOLUTION)
			{
				if (nkSoldierInfo.GetGrade() < 5 || nkSoldierInfo.GetGrade() >= 10)
				{
					return;
				}
			}
			else if (this.mCheckList.Count >= 50)
			{
				return;
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
		switch (this.m_eShowType)
		{
		case SOLCOMPOSE_TYPE.COMPOSE:
			this.UpdateComposeData();
			break;
		case SOLCOMPOSE_TYPE.SELL:
			this.UpdateSellData();
			break;
		case SOLCOMPOSE_TYPE.EXTRACT:
			this.UpdateExtractData();
			break;
		case SOLCOMPOSE_TYPE.MYTHEVOLUTION:
			this.UpdateMythEvolution();
			break;
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
		if (!bMainSelect || this.ShowType == SOLCOMPOSE_TYPE.EXTRACT)
		{
			eIndex = SolComposeListDlg.SOL_LIST_INSERT_TYPE.WAIT;
		}
		if (bMainSelect && this.ShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
		{
			eIndex = SolComposeListDlg.SOL_LIST_INSERT_TYPE.ALL;
		}
		UIScrollList arg_231_0 = this.ddList1;
		this.m_bMainSelect = bMainSelect;
		arg_231_0.controlIsEnabled = bMainSelect;
		this.InsertList(eIndex);
	}

	protected virtual void ClickOk(IUIObject obj)
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
		if (this.m_eShowType == SOLCOMPOSE_TYPE.MYTHEVOLUTION)
		{
			Myth_Evolution_Main_DLG myth_Evolution_Main_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTH_EVOLUTION_MAIN_DLG) as Myth_Evolution_Main_DLG;
			if (myth_Evolution_Main_DLG != null && this.mCheckList.Count > 0)
			{
				myth_Evolution_Main_DLG.SetBaseSol(this.mCheckList[0]);
			}
		}
		this.Close();
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
				NewListItem newListItem = new NewListItem(this.ComposeNewListBox.ColumnNum, true, string.Empty);
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
				if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG) == null)
				{
					newListItem.SetListItemData(9, string.Empty, null, new EZValueChangedDelegate(this.ClickSolDetailInfo), null);
				}
				else
				{
					newListItem.SetListItemData(9, false);
				}
				if (kSolInfo.IsAtbCommonFlag(1L))
				{
					newListItem.SetListItemData(10, true);
				}
				else
				{
					newListItem.SetListItemData(10, false);
				}
				newListItem.Data = kSolInfo;
				this.ComposeNewListBox.UpdateContents(i, newListItem);
				break;
			}
		}
	}

	public bool IsAddComposeList(NkSoldierInfo kSolInfo)
	{
		if (this.m_bMainSelect && kSolInfo.IsMaxLevel() && kSolInfo.IsMaxGrade())
		{
			return false;
		}
		SolComposeMainDlg solComposeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_DLG) as SolComposeMainDlg;
		return solComposeMainDlg == null || solComposeMainDlg.mBaseSol == null || solComposeMainDlg.mBaseSol.GetSolID() != kSolInfo.GetSolID();
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

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (this.ComposeNewListBox == null || this.ComposeNewListBox.Count == 0)
		{
			return;
		}
		this.guideWinID = winID;
		UIListItemContainer item = this.ComposeNewListBox.GetItem(0);
		if (item == null)
		{
			return;
		}
		if (this._Touch == null)
		{
			this._Touch = UICreateControl.Button("touch", "Main_I_Touch01", 196f, 154f);
			this._Touch.PlayAni(true);
		}
		if (this._Touch == null)
		{
			return;
		}
		this._Touch.gameObject.transform.parent = item.gameObject.transform;
		this._Touch.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z - 3f);
		BoxCollider component = this._Touch.gameObject.GetComponent<BoxCollider>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	private void HideTouch()
	{
		if (this._Touch != null && this._Touch.gameObject != null)
		{
			this._Touch.gameObject.SetActive(false);
		}
		this._Touch = null;
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.guideWinID) as UI_UIGuide;
		if (uI_UIGuide == null)
		{
			return;
		}
		uI_UIGuide.CloseUI = true;
	}
}
