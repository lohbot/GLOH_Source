using GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class SoldierSelectDlg : Form
{
	public enum E_SOL_SORTLOST01
	{
		SOLSORT_BATTLE,
		SOLSORT_ALL,
		SOLSORT_READY
	}

	public enum E_SOL_SORTLOST02
	{
		SOLSORT_ITEM_UP,
		SOLSORT_ITEM_DOWN,
		SOLSORT_LEVEL_UP,
		SOLSORT_LEVEL_DOWN,
		SOLSORT_NAME,
		SOLSORT_RANK_UP,
		SOLSORT_RANK_DOWN
	}

	protected static int MAX_SOLDERSELECT_LIST = 4;

	private DropDownList SolSortList01;

	private DropDownList SolSortList02;

	private NewListBox SoldierSelectList;

	private int m_nSearch_ItemUnique;

	private ITEM m_pkEquipItem;

	private List<NkSoldierInfo> m_kSolList = new List<NkSoldierInfo>();

	private List<NkSoldierInfo> m_kSolSortList = new List<NkSoldierInfo>();

	private Form m_pkParentDlg;

	private long m_SelectSolID = -1L;

	public SoldierSelectDlg.E_SOL_SORTLOST01 SORT01_TYPE
	{
		get
		{
			return (SoldierSelectDlg.E_SOL_SORTLOST01)PlayerPrefs.GetInt("soldierSelect_dlg_sort01");
		}
		set
		{
			PlayerPrefs.SetInt("soldierSelect_dlg_sort01", (int)value);
		}
	}

	public SoldierSelectDlg.E_SOL_SORTLOST02 SORT02_TYPE
	{
		get
		{
			return (SoldierSelectDlg.E_SOL_SORTLOST02)PlayerPrefs.GetInt("soldierSelect_dlg_sort02");
		}
		set
		{
			PlayerPrefs.SetInt("soldierSelect_dlg_sort02", (int)value);
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SoldierSelect", G_ID.SOLSELECT_DLG, false);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.SolSortList01 = (base.GetControl("DropDownList_ListSorting01") as DropDownList);
		this.SolSortList01.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeSolt_SolPosTypeList01));
		this.SolSortList02 = (base.GetControl("DropDownList_ListSorting02") as DropDownList);
		this.SolSortList02.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeSolt_SolSortTypeList02));
		this.SoldierSelectList = (base.GetControl("NewListBox_sol") as NewListBox);
		this.SoldierSelectList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSoldierSelect));
		this.SetSortList();
		if (TsPlatform.IsMobile)
		{
			base.SetScreenCenter();
		}
	}

	public override void InitData()
	{
		Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
		if (inventory_Dlg != null)
		{
			if (TsPlatform.IsMobile)
			{
				base.SetScreenCenter();
			}
			else
			{
				float num = base.GetSizeX() + inventory_Dlg.GetSizeX();
				float num2 = GUICamera.width / 2f - num / 2f;
				float y = GUICamera.height / 2f - base.GetSizeY() / 2f;
				float num3 = GUICamera.height / 2f - inventory_Dlg.GetSizeY() / 2f;
				base.SetLocation((float)((int)(num2 + inventory_Dlg.GetSizeX())), y);
				inventory_Dlg.SetLocation(num2, (float)((int)num3));
			}
		}
	}

	public void SetLocationByForm(ref Form pkTargetDlg)
	{
		float x = 100f;
		float y = 100f;
		if (pkTargetDlg != null)
		{
			x = pkTargetDlg.GetLocationX() + pkTargetDlg.GetSizeX();
			y = pkTargetDlg.GetLocationY();
		}
		base.SetLocation(x, y);
		this.m_pkParentDlg = pkTargetDlg;
	}

	public void SetDataItem(ITEM pkItem)
	{
		if (pkItem != null && pkItem.IsValid())
		{
			this.m_nSearch_ItemUnique = pkItem.m_nItemUnique;
			this.m_pkEquipItem = pkItem;
		}
		else
		{
			this.m_nSearch_ItemUnique = 0;
			this.m_pkEquipItem = null;
		}
		this.SetData();
	}

	public void SetData()
	{
		if (this.m_nSearch_ItemUnique == 0 || this.m_pkEquipItem == null)
		{
			return;
		}
		this.MakeSolListAndSort();
		if (this.m_kSolSortList.Count > 0)
		{
			this.SetSolListInfo();
		}
		else
		{
			this.SoldierSelectList.Clear();
			this.InitData();
		}
	}

	private void SetSolListInfo()
	{
		string empty = string.Empty;
		this.SoldierSelectList.Clear();
		for (int i = 0; i < this.m_kSolSortList.Count; i++)
		{
			NkSoldierInfo nkSoldierInfo = this.m_kSolSortList[i];
			NewListItem newListItem = new NewListItem(this.SoldierSelectList.ColumnNum, true);
			EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(nkSoldierInfo.GetCharKind(), nkSoldierInfo.GetGrade());
			if (eventHeroCharCode != null)
			{
				newListItem.SetListItemData(1, "Win_I_EventSol", null, null, null);
				newListItem.EventMark = true;
			}
			else
			{
				UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(nkSoldierInfo.GetCharKind(), (int)nkSoldierInfo.GetGrade());
				if (legendFrame != null)
				{
					newListItem.SetListItemData(1, legendFrame, null, null, null);
				}
			}
			newListItem.SetListItemData(0, false);
			newListItem.SetListItemData(2, nkSoldierInfo.GetListSolInfo(false), null, null, null);
			newListItem.SetListItemData(3, nkSoldierInfo.GetName(), null, null, null);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
				"count1",
				nkSoldierInfo.GetLevel().ToString(),
				"count2",
				nkSoldierInfo.GetSolMaxLevel().ToString()
			});
			newListItem.SetListItemData(4, empty, null, null, null);
			ITEM equipItemByUnique = nkSoldierInfo.GetEquipItemByUnique(this.m_nSearch_ItemUnique);
			if (equipItemByUnique != null && equipItemByUnique.IsValid())
			{
				newListItem.SetListItemData(5, equipItemByUnique, this.m_pkEquipItem, nkSoldierInfo.GetSolID(), new EZValueChangedDelegate(this.OnClickItemIcon), null);
			}
			else
			{
				newListItem.SetListItemData(5, false);
			}
			newListItem.Data = nkSoldierInfo.GetSolID();
			this.SoldierSelectList.Add(newListItem);
		}
		this.SoldierSelectList.RepositionItems();
	}

	private void OnClickItemIcon(IUIObject obj)
	{
		if (!TsPlatform.IsMobile)
		{
			return;
		}
		ItemTexture itemTexture = obj as ItemTexture;
		if (null == itemTexture)
		{
			return;
		}
		ITEM pkItem = (ITEM)itemTexture.c_cItemTooltip;
		ITEM pkSecondItem = (ITEM)itemTexture.c_cItemSecondTooltip;
		Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
		if (inventory_Dlg != null)
		{
			long solID = (long)itemTexture.data;
			Protocol_Item.Item_ShowItemInfo((G_ID)inventory_Dlg.WindowID, pkItem, Vector3.zero, pkSecondItem, solID);
		}
	}

	private void SetSortList()
	{
		this.SolSortList01.Clear();
		this.SolSortList01.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), SoldierSelectDlg.E_SOL_SORTLOST01.SOLSORT_BATTLE);
		this.SolSortList01.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("120"), SoldierSelectDlg.E_SOL_SORTLOST01.SOLSORT_ALL);
		this.SolSortList01.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("123"), SoldierSelectDlg.E_SOL_SORTLOST01.SOLSORT_READY);
		this.SolSortList01.SetViewArea(this.SolSortList01.Count);
		this.SolSortList01.SetIndex((int)this.SORT01_TYPE);
		this.SolSortList01.RepositionItems();
		this.SolSortList02.Clear();
		this.SolSortList02.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("926"), SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_ITEM_UP);
		this.SolSortList02.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("927"), SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_ITEM_DOWN);
		this.SolSortList02.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1888"), SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_LEVEL_UP);
		this.SolSortList02.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1889"), SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_LEVEL_DOWN);
		this.SolSortList02.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1890"), SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_NAME);
		this.SolSortList02.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1891"), SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_RANK_UP);
		this.SolSortList02.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1892"), SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_RANK_DOWN);
		this.SolSortList02.SetViewArea(this.SolSortList02.Count);
		this.SolSortList02.SetIndex((int)this.SORT02_TYPE);
		this.SolSortList02.RepositionItems();
	}

	private void MakeAllSolList()
	{
		this.MakeBattleSolList();
		this.MakeReadySolList();
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

	private void AddSolList(NkSoldierInfo pkSolinfo, eSOL_POSTYPE eAddPosType)
	{
		if (pkSolinfo == null || !pkSolinfo.IsValid())
		{
			return;
		}
		if (eAddPosType != eSOL_POSTYPE.SOLPOS_MAX && pkSolinfo.GetSolPosType() != (byte)eAddPosType)
		{
			return;
		}
		if (this.m_nSearch_ItemUnique > 0 && this.m_pkEquipItem != null && !Protocol_Item.Is_Item_Equipment(this.m_pkEquipItem, pkSolinfo, false))
		{
			return;
		}
		this.m_kSolList.Add(pkSolinfo);
	}

	private void MakeSolListAndSort()
	{
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		this.m_kSolList.Clear();
		this.m_kSolSortList.Clear();
		switch (this.SORT01_TYPE)
		{
		case SoldierSelectDlg.E_SOL_SORTLOST01.SOLSORT_BATTLE:
			this.MakeBattleSolList();
			break;
		case SoldierSelectDlg.E_SOL_SORTLOST01.SOLSORT_ALL:
			this.MakeAllSolList();
			break;
		case SoldierSelectDlg.E_SOL_SORTLOST01.SOLSORT_READY:
			this.MakeReadySolList();
			break;
		}
		switch (this.SORT02_TYPE)
		{
		case SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_ITEM_UP:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareItemUp));
			break;
		case SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_ITEM_DOWN:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareItemDown));
			break;
		case SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_LEVEL_UP:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevelDESC));
			break;
		case SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_LEVEL_DOWN:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevelASC));
			break;
		case SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_NAME:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareName));
			break;
		case SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_RANK_UP:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareGradeDESC));
			break;
		case SoldierSelectDlg.E_SOL_SORTLOST02.SOLSORT_RANK_DOWN:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareGradeASC));
			break;
		}
		for (int i = 0; i < this.m_kSolList.Count; i++)
		{
			this.m_kSolSortList.Add(this.m_kSolList[i]);
		}
	}

	private int CompareCombatPowerDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetCombatPower().CompareTo(a.GetCombatPower());
	}

	private int CompareCombatPowerASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetCombatPower().CompareTo(b.GetCombatPower());
	}

	private int CompareLevelDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetLevel().CompareTo(a.GetLevel());
	}

	private int CompareLevelASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetLevel().CompareTo(b.GetLevel());
	}

	private int CompareName(NkSoldierInfo a, NkSoldierInfo b)
	{
		if (a.GetName().Equals(b.GetName()))
		{
			return this.CompareLevelDESC(a, b);
		}
		return a.GetName().CompareTo(b.GetName());
	}

	private int CompareGradeDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return SolComposeListDlg.CompareGrade_High(a, b);
	}

	private int CompareGradeASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return SolComposeListDlg.CompareGrade_Low(a, b);
	}

	private int CompareItemUp(NkSoldierInfo a, NkSoldierInfo b)
	{
		return this.CompareItem(a, b);
	}

	private int CompareItemDown(NkSoldierInfo a, NkSoldierInfo b)
	{
		return this.CompareItem(b, a);
	}

	private int CompareItem(NkSoldierInfo a, NkSoldierInfo b)
	{
		long num = 0L;
		long num2 = 0L;
		ITEM equipItemByUnique = a.GetEquipItemByUnique(this.m_nSearch_ItemUnique);
		ITEM equipItemByUnique2 = b.GetEquipItemByUnique(this.m_nSearch_ItemUnique);
		if (equipItemByUnique != null)
		{
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(equipItemByUnique.m_nItemUnique);
			if (itemInfo != null)
			{
				num = (long)(itemInfo.GetUseMinLevel(equipItemByUnique) * 100 + equipItemByUnique.GetRank());
			}
		}
		if (equipItemByUnique2 != null)
		{
			ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(equipItemByUnique2.m_nItemUnique);
			if (itemInfo2 != null)
			{
				num2 = (long)(itemInfo2.GetUseMinLevel(equipItemByUnique) * 100 + equipItemByUnique2.GetRank());
			}
		}
		if (num < num2)
		{
			return 1;
		}
		if (num > num2)
		{
			return -1;
		}
		return 0;
	}

	private void OnChangeSolt_SolPosTypeList01(IUIObject obj)
	{
		if (this.SolSortList01.Count > 0 && this.SolSortList01.SelectedItem != null)
		{
			ListItem listItem = this.SolSortList01.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.SORT01_TYPE = (SoldierSelectDlg.E_SOL_SORTLOST01)((int)listItem.Key);
			}
		}
		this.SetData();
	}

	private void OnChangeSolt_SolSortTypeList02(IUIObject obj)
	{
		if (this.SolSortList02.Count > 0 && this.SolSortList02.SelectedItem != null)
		{
			ListItem listItem = this.SolSortList02.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.SORT02_TYPE = (SoldierSelectDlg.E_SOL_SORTLOST02)((int)listItem.Key);
			}
		}
		this.SetData();
	}

	private void OnClickSoldierSelect(IUIObject obj)
	{
		NewListBox newListBox = obj as NewListBox;
		if (obj == null || null == newListBox)
		{
			return;
		}
		IUIListObject selectedItem = newListBox.SelectedItem;
		UIListItemContainer uIListItemContainer = (UIListItemContainer)selectedItem;
		if (null == uIListItemContainer)
		{
			return;
		}
		if (uIListItemContainer.data != null)
		{
			this.m_SelectSolID = (long)uIListItemContainer.Data;
		}
		if (this.m_pkParentDlg == null || !this.m_pkParentDlg.Visible)
		{
			this.CloseForm(null);
		}
		Protocol_Item.Item_Use(this.m_pkEquipItem, this.m_SelectSolID);
		this.Close();
	}

	private void OnClickSolButton(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		if (this.m_SelectSolID < 0L)
		{
			return;
		}
		if (this.m_pkParentDlg == null || !this.m_pkParentDlg.Visible)
		{
			this.Close();
		}
		Protocol_Item.Item_Use(this.m_pkEquipItem, this.m_SelectSolID);
		this.Close();
	}

	public void CloseByParent(int pkParentDlgID)
	{
		if (this.m_pkParentDlg != null && this.m_pkParentDlg.WindowID == pkParentDlgID)
		{
			this.Close();
		}
	}
}
