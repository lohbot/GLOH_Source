using GAME;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityForms;

public class ReforgeSelectDlg : Form
{
	private enum SHOWTYPE
	{
		ITEM,
		SOLDER,
		SOLITEM
	}

	private NewListBox m_NewListBox;

	private Toolbar m_Tab;

	private DropDownList m_Droplist;

	private Button m_btConfirm1;

	private Button m_btConfirm2;

	private Button m_HelpButton;

	private Label m_lbText;

	private byte m_nSearch_SolPosType = 1;

	private int m_nSearch_SolSortType;

	private List<NkSoldierInfo> m_kSolList = new List<NkSoldierInfo>();

	private List<NkSoldierInfo> m_kSolSortList = new List<NkSoldierInfo>();

	private List<ITEM> m_InvenItemList = new List<ITEM>();

	private NkSoldierInfo m_SelectSol;

	private ITEM m_SelectItem;

	private ReforgeSelectDlg.SHOWTYPE m_ShowType;

	private long m_SolID;

	private byte m_byMilityUnique;

	private UIPanelTab _GuideItem;

	private float _ButtonZ;

	private int m_nWinID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Reforge/DLG_ReforgeSelect", G_ID.REFORGESELECT_DLG, true);
		base.bCloseAni = false;
		base.ShowBlackBG(0.5f);
	}

	public override void AfterShow()
	{
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.transform.localPosition = new Vector3(base.BLACK_BG.transform.localPosition.x, base.BLACK_BG.transform.localPosition.y, 10f);
		}
	}

	public override void SetComponent()
	{
		this.m_lbText = (base.GetControl("Label_text") as Label);
		this.m_lbText.Visible = false;
		this.m_btConfirm1 = (base.GetControl("Button_confirm1") as Button);
		this.m_btConfirm1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnNewListClick));
		this.m_btConfirm1.Visible = false;
		this.m_btConfirm2 = (base.GetControl("Button_confirm2") as Button);
		this.m_btConfirm2.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnNewListClick));
		this.m_btConfirm2.Visible = false;
		this.m_Tab = (base.GetControl("ToolBar_tab") as Toolbar);
		this.m_Tab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("204");
		this.m_Tab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1490");
		UIPanelTab expr_F9 = this.m_Tab.Control_Tab[0];
		expr_F9.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_F9.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_127 = this.m_Tab.Control_Tab[1];
		expr_127.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_127.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_Droplist = (base.GetControl("DropDownList_DropDownList1") as DropDownList);
		this.m_Droplist.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), 1);
		this.m_Droplist.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("123"), 0);
		this.m_Droplist.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("120"), 100);
		this.m_Droplist.SetViewArea(this.m_Droplist.Count);
		this.m_Droplist.RepositionItems();
		this.m_Droplist.SetFirstItem();
		this.m_Droplist.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeSortSolList));
		this.m_Tab.FirstSetting();
		this.m_Tab.SetSelectTabIndex(0);
		this.m_NewListBox = (base.GetControl("NewListBox_ReforgeSelect") as NewListBox);
		this.m_NewListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnNewListClick));
		this.m_HelpButton = (base.GetControl("Help_Button") as Button);
		this.m_HelpButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		if (null != this.m_NewListBox)
		{
			this.SetColumFromShowType();
		}
		this.SetData();
	}

	private void SetColumFromShowType()
	{
		this.m_NewListBox.Clear();
		base.SetShowLayer(1, this.m_ShowType == ReforgeSelectDlg.SHOWTYPE.ITEM);
		base.SetShowLayer(2, this.m_ShowType == ReforgeSelectDlg.SHOWTYPE.SOLDER);
		if (this.m_ShowType == ReforgeSelectDlg.SHOWTYPE.ITEM)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGESELECTITEM_DLG);
		}
		this.m_btConfirm1.Visible = false;
		this.m_btConfirm2.Visible = false;
	}

	public void SetData()
	{
		ReforgeSelectDlg.SHOWTYPE showType = this.m_ShowType;
		if (showType != ReforgeSelectDlg.SHOWTYPE.ITEM)
		{
			if (showType == ReforgeSelectDlg.SHOWTYPE.SOLDER)
			{
				this.SetSolData();
			}
		}
		else
		{
			this.SetInvItemData();
		}
	}

	public void UpdateData(int nItemPos, int nItemType, long nItemID)
	{
		bool flag = false;
		for (int i = 0; i < this.m_NewListBox.Count; i++)
		{
			if (this.m_NewListBox.GetItem(i) != null)
			{
				ITEM iTEM = this.m_NewListBox.GetItem(i).Data as ITEM;
				if (iTEM.m_nItemPos == nItemPos && iTEM.m_nPosType == nItemType)
				{
					ITEM item = NkUserInventory.GetInstance().GetItem(nItemType, nItemPos);
					NewListItem item2 = new NewListItem(this.m_NewListBox.ColumnNum, true, string.Empty);
					this.SetItemColum(item, i, ref item2);
					this.m_NewListBox.UpdateAdd(i, item2);
					flag = true;
				}
			}
		}
		this.m_NewListBox.RepositionItems();
		if (!flag)
		{
			this.SetInvItemData();
		}
	}

	private void SetSolData()
	{
		this.MakeSolListAndSort();
		this.m_NewListBox.Clear();
		for (int i = 0; i < this.m_kSolSortList.Count; i++)
		{
			NewListItem item = new NewListItem(this.m_NewListBox.ColumnNum, true, string.Empty);
			this.SetSolColum(i, ref item);
			this.m_NewListBox.Add(item);
		}
		this.m_NewListBox.RepositionItems();
		if (this.m_kSolSortList.Count == 0)
		{
			this.m_lbText.Visible = true;
		}
		else
		{
			this.m_lbText.Visible = false;
		}
	}

	private void SetInvItemData()
	{
		bool flag = true;
		this.m_SolID = 0L;
		this.m_NewListBox.Clear();
		this.m_InvenItemList.Clear();
		for (int i = 1; i <= 4; i++)
		{
			for (int j = 0; j < ItemDefine.INVENTORY_ITEMSLOT_MAX; j++)
			{
				ITEM item = NkUserInventory.GetInstance().GetItem(i, j);
				if (item != null)
				{
					if (item.GetRank() != eITEM_RANK_TYPE.ITEM_RANK_SS)
					{
						if (NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique) != null)
						{
							item = NkUserInventory.GetInstance().GetItem(i, j);
							this.m_InvenItemList.Add(item);
							flag = false;
						}
					}
				}
			}
		}
		if (this.m_InvenItemList.Count > 0)
		{
			this.m_InvenItemList.Sort(new Comparison<ITEM>(this.CompareItemLevel));
			for (int k = 0; k < this.m_InvenItemList.Count; k++)
			{
				NewListItem item2 = new NewListItem(this.m_NewListBox.ColumnNum, true, string.Empty);
				this.SetItemColum(this.m_InvenItemList[k], k, ref item2);
				this.m_NewListBox.Add(item2);
			}
		}
		this.m_NewListBox.RepositionItems();
		if (flag)
		{
			this.m_lbText.Visible = true;
		}
		else
		{
			this.m_lbText.Visible = false;
		}
	}

	private void SetSolEquipItem()
	{
		if (this.m_SelectSol == null)
		{
			this.m_ShowType = ReforgeSelectDlg.SHOWTYPE.SOLDER;
			return;
		}
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			ITEM item = this.m_SelectSol.GetEquipItemInfo().m_kItem[i].GetItem();
			if (item != null)
			{
				if (item.m_nItemUnique > 0)
				{
					if (NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique) != null)
					{
						NewListItem item2 = new NewListItem(this.m_NewListBox.ColumnNum, true, string.Empty);
						this.SetItemColum(item, num++, ref item2);
						this.m_NewListBox.Add(item2);
					}
				}
			}
		}
		this.m_NewListBox.RepositionItems();
		if (num == 0)
		{
			this.m_lbText.Visible = true;
		}
		else
		{
			this.m_lbText.Visible = false;
		}
	}

	private void SetSolColum(int pos, ref NewListItem item)
	{
		if (this.m_kSolSortList.Count <= pos)
		{
			return;
		}
		if (this.m_kSolSortList[pos] == null)
		{
			TsLog.Log("m_kSolSortList[pos] == null", new object[0]);
			return;
		}
		EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(this.m_kSolSortList[pos].GetCharKind(), this.m_kSolSortList[pos].GetGrade());
		if (eventHeroCharCode != null)
		{
			item.EventMark = true;
			item.SetListItemData(1, "Win_I_EventSol", null, null, null);
		}
		else
		{
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(this.m_kSolSortList[pos].GetCharKind(), (int)this.m_kSolSortList[pos].GetGrade());
			if (legendFrame != null)
			{
				item.SetListItemData(1, legendFrame, null, null, null);
			}
		}
		item.SetListItemData(2, this.m_kSolSortList[pos].GetListSolInfo(false), null, null, null);
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"count1",
			this.m_kSolSortList[pos].GetLevel(),
			"count2",
			this.m_kSolSortList[pos].GetSolMaxLevel()
		});
		item.SetListItemData(3, this.m_kSolSortList[pos].GetName(), null, null, null);
		item.SetListItemData(4, textFromInterface, null, null, null);
		item.SetListItemData(0, false);
		item.Data = this.m_kSolSortList[pos];
	}

	private void SetItemColum(ITEM itemdata, int pos, ref NewListItem item)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(itemdata);
		item.SetListItemData(2, itemdata, null, null, null);
		item.SetListItemData(3, rankColorName, null, null, null);
		int rank = itemdata.m_nOption[2];
		item.SetListItemData(4, ItemManager.RankTextColor(rank) + ItemManager.RankText(rank), null, null, null);
		stringBuilder.Remove(0, stringBuilder.Length);
		if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(itemdata.m_nItemUnique) == eITEM_PART.ITEMPART_WEAPON)
		{
			int nValue = Protocol_Item.Get_Min_Damage(itemdata);
			int optionValue = Tooltip_Dlg.GetOptionValue(itemdata, nValue, 1);
			int nValue2 = Protocol_Item.Get_Max_Damage(itemdata);
			int optionValue2 = Tooltip_Dlg.GetOptionValue(itemdata, nValue2, 1);
			stringBuilder.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("242") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString(), " ~ ", optionValue2.ToString()));
		}
		else if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(itemdata.m_nItemUnique) == eITEM_PART.ITEMPART_ARMOR)
		{
			int nValue = Protocol_Item.Get_Defense(itemdata);
			int optionValue = Tooltip_Dlg.GetOptionValue(itemdata, nValue, 2);
			stringBuilder.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("243") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString()));
		}
		item.SetListItemData(0, stringBuilder.ToString(), null, null, null);
		item.Data = itemdata;
	}

	private void MakeBattleSolList()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			this.AddSolList(soldierInfo, eSOL_POSTYPE.SOLPOS_BATTLE);
		}
	}

	private void MakeReadySolList()
	{
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			this.AddSolList(current, eSOL_POSTYPE.SOLPOS_READY);
		}
	}

	private void MakeMilitarySolList(int militaryunique)
	{
		if (militaryunique <= 0)
		{
			return;
		}
		NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
		if (militaryList == null)
		{
			return;
		}
		NkMineMilitaryInfo mineMilitaryInfo = militaryList.GetMineMilitaryInfo((byte)militaryunique);
		if (mineMilitaryInfo == null)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo solInfo = mineMilitaryInfo.GetSolInfo(i);
			this.AddSolList(solInfo, eSOL_POSTYPE.SOLPOS_MINE_MILITARY);
		}
	}

	private void MakeMilitarySolList()
	{
		NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
		if (militaryList != null)
		{
			for (int i = 0; i < 10; i++)
			{
				byte militaryunique = (byte)(i + 1);
				NkMineMilitaryInfo mineMilitaryInfo = militaryList.GetMineMilitaryInfo(militaryunique);
				if (mineMilitaryInfo != null && mineMilitaryInfo.IsValid())
				{
					this.MakeMilitarySolList((int)militaryunique);
				}
			}
		}
	}

	private void AddSolList(NkSoldierInfo pkSolinfo, eSOL_POSTYPE eAddPosType)
	{
		if (pkSolinfo == null || !pkSolinfo.IsValid())
		{
			return;
		}
		if (pkSolinfo.GetSolPosType() != (byte)eAddPosType)
		{
			return;
		}
		this.m_kSolList.Add(pkSolinfo);
	}

	private void MakeSolListAndSort()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		this.m_kSolList.Clear();
		this.m_kSolSortList.Clear();
		byte nSearch_SolPosType = this.m_nSearch_SolPosType;
		switch (nSearch_SolPosType)
		{
		case 0:
			this.MakeMilitarySolList();
			this.MakeReadySolList();
			goto IL_BD;
		case 1:
			this.MakeBattleSolList();
			goto IL_BD;
		case 2:
		case 6:
			this.MakeMilitarySolList((int)this.m_byMilityUnique);
			goto IL_BD;
		case 3:
		case 4:
		case 5:
			IL_54:
			if (nSearch_SolPosType != 100)
			{
				goto IL_BD;
			}
			if (this.m_nSearch_SolSortType == 1)
			{
				this.MakeBattleSolList();
			}
			else
			{
				this.MakeBattleSolList();
				this.MakeMilitarySolList();
				this.MakeReadySolList();
			}
			goto IL_BD;
		}
		goto IL_54;
		IL_BD:
		switch (this.m_nSearch_SolSortType)
		{
		case 1:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.ComparePosIndex));
			break;
		case 2:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareName));
			break;
		case 3:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevel));
			break;
		case 4:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareCombatPower));
			break;
		case 5:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.ComparePosIndex));
			break;
		}
		if (this.m_nSearch_SolPosType == 100)
		{
			NkSoldierInfo leaderSoldierInfo = charPersonInfo.GetLeaderSoldierInfo();
			if (leaderSoldierInfo != null)
			{
				for (int i = 0; i < this.m_kSolList.Count; i++)
				{
					if (leaderSoldierInfo.GetSolID() == this.m_kSolList[i].GetSolID())
					{
						this.m_kSolSortList.Add(this.m_kSolList[i]);
						this.m_kSolList.Remove(this.m_kSolSortList[0]);
						break;
					}
				}
			}
		}
		for (int j = 0; j < this.m_kSolList.Count; j++)
		{
			this.m_kSolSortList.Add(this.m_kSolList[j]);
		}
	}

	private int ComparePosIndex(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetSolPosIndex().CompareTo(b.GetSolPosIndex());
	}

	private int CompareName(NkSoldierInfo a, NkSoldierInfo b)
	{
		if (a.GetName().Equals(b.GetName()))
		{
			return this.CompareLevel(a, b);
		}
		return a.GetName().CompareTo(b.GetName());
	}

	private int CompareLevel(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetLevel().CompareTo(b.GetLevel());
	}

	private int CompareCombatPower(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetCombatPower().CompareTo(a.GetCombatPower());
	}

	private int CompareItemLevel(ITEM a, ITEM b)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(a.m_nItemUnique);
		ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(b.m_nItemUnique);
		if (itemInfo.m_nQualityLevel != itemInfo2.m_nQualityLevel)
		{
			return -itemInfo.m_nQualityLevel.CompareTo(itemInfo2.m_nQualityLevel);
		}
		if (a.GetRank() != b.GetRank())
		{
			return -a.GetRank().CompareTo(b.GetRank());
		}
		int useMinLevel = NrTSingleton<ItemManager>.Instance.GetUseMinLevel(a);
		int useMinLevel2 = NrTSingleton<ItemManager>.Instance.GetUseMinLevel(b);
		if (useMinLevel == useMinLevel2)
		{
			return a.m_nItemUnique.CompareTo(b.m_nItemUnique);
		}
		return -useMinLevel.CompareTo(useMinLevel2);
	}

	public void OnNewListClick(IUIObject obj)
	{
		if (this.m_ShowType == ReforgeSelectDlg.SHOWTYPE.SOLDER)
		{
			NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)this.m_NewListBox.SelectedItem.Data;
			if (nkSoldierInfo != null)
			{
				this.m_SelectSol = nkSoldierInfo;
				this.m_SolID = nkSoldierInfo.GetSolID();
				ItemSelectDlg itemSelectDlg = base.SetChildForm(G_ID.REFORGESELECTITEM_DLG, Form.ChildLocation.LEFT) as ItemSelectDlg;
				if (itemSelectDlg != null)
				{
					itemSelectDlg.SetData(this.m_SolID);
				}
			}
		}
		else if (this.m_ShowType == ReforgeSelectDlg.SHOWTYPE.ITEM || this.m_ShowType == ReforgeSelectDlg.SHOWTYPE.SOLITEM)
		{
			if (null == this.m_NewListBox.SelectedItem)
			{
				return;
			}
			ITEM iTEM = (ITEM)this.m_NewListBox.SelectedItem.Data;
			if (iTEM != null)
			{
				this.m_SelectItem = iTEM;
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGERESULT_DLG);
				ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
				if (reforgeMainDlg != null)
				{
					reforgeMainDlg.Set_Value(this.m_SelectItem);
					reforgeMainDlg.SetSolID(this.m_SolID);
				}
			}
		}
	}

	public void OnClickCancle(IUIObject obj)
	{
		this.m_ShowType = ReforgeSelectDlg.SHOWTYPE.SOLDER;
		this.m_SelectSol = null;
		this.SetColumFromShowType();
		this.SetData();
	}

	public void OnItemConfirm(IUIObject obj)
	{
		if (this.m_SelectItem != null)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGERESULT_DLG);
			ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
			if (reforgeMainDlg != null)
			{
				reforgeMainDlg.Set_Value(this.m_SelectItem);
				reforgeMainDlg.SetSolID(this.m_SolID);
			}
		}
		else
		{
			TsLog.Log("m_SelectItem == null", new object[0]);
		}
	}

	public void OnClickTab(IUIObject obj)
	{
		this.HideUIGuide();
		this.closeButton.Visible = true;
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGECONFIRM_DLG);
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_ShowType = (ReforgeSelectDlg.SHOWTYPE)uIPanelTab.panel.index;
		this.SetColumFromShowType();
		this.SetData();
	}

	public void OnChangeSortSolList(IUIObject obj)
	{
		this.m_nSearch_SolPosType = 1;
		if (this.m_Droplist.Count > 0 && this.m_Droplist.SelectedItem != null)
		{
			ListItem listItem = this.m_Droplist.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.m_nSearch_SolPosType = (byte)listItem.Key;
				if (this.m_nSearch_SolPosType == 2 || this.m_nSearch_SolPosType == 6)
				{
					this.m_byMilityUnique = (byte)this.m_Droplist.SelectIndex;
				}
			}
		}
		this.SetData();
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Gear_Strengthen.ToString());
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGEMAIN_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGERESULT_DLG);
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.depthChangeable = false;
		}
		this._GuideItem = this.m_Tab.Control_Tab[1];
		this.m_nWinID = winID;
		if (null != this._GuideItem)
		{
			this._ButtonZ = this._GuideItem.GetLocation().z;
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				if (uI_UIGuide.GetLocation().z == base.GetLocation().z)
				{
					uI_UIGuide.SetLocation(uI_UIGuide.GetLocationX(), uI_UIGuide.GetLocationY(), uI_UIGuide.GetLocation().z - 10f);
				}
				this._GuideItem.EffectAni = false;
				Vector2 x = new Vector2(base.GetLocationX() + this._GuideItem.GetLocationX() + 72f, base.GetLocationY() + this._GuideItem.GetLocationY() + 44f);
				uI_UIGuide.Move(x, UI_UIGuide.eTIPPOS.BUTTOM);
				this._ButtonZ = this._GuideItem.gameObject.transform.localPosition.z;
				this._GuideItem.SetLocationZ(uI_UIGuide.GetLocation().z - base.GetLocation().z - 1f);
				this._GuideItem.AlphaAni(1f, 0.5f, -0.5f);
			}
		}
	}

	public void HideUIGuide()
	{
		if (null != this._GuideItem)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			this._GuideItem.SetLocationZ(this._ButtonZ);
			this._GuideItem.StopAni();
			this._GuideItem.AlphaAni(1f, 1f, 0f);
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				uI_UIGuide.Close();
			}
		}
		this._GuideItem = null;
	}
}
