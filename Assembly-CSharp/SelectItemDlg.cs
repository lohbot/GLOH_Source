using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class SelectItemDlg : Form
{
	public enum eTabMode
	{
		Inventory,
		Soldier
	}

	public enum eType
	{
		Enhance,
		Repair,
		Dissemblie
	}

	private const string STYLE_REPAIR_NEED = "Win_I_CPortRepairI2";

	private const string STYLE_MUST_REPAIR_NEED = "Win_I_CPortRepairI1";

	private Button _btPrev;

	private Button _btNext;

	private Label _lbPage;

	private Toolbar _Toolbar;

	private static int NUM_ITEMLIST = 5;

	private Button[] _btItem = new Button[SelectItemDlg.NUM_ITEMLIST];

	private DrawTexture[] _dtItemIcon = new DrawTexture[SelectItemDlg.NUM_ITEMLIST];

	private DrawTexture[] _dtItemIconBG = new DrawTexture[SelectItemDlg.NUM_ITEMLIST];

	private Label[] _lbItemName = new Label[SelectItemDlg.NUM_ITEMLIST];

	private Label[] _lbItemDur = new Label[SelectItemDlg.NUM_ITEMLIST];

	private Label[] _lbItemDurTitle = new Label[SelectItemDlg.NUM_ITEMLIST];

	private DrawTexture _dtItemListOver;

	private static int NUM_SOLDIERLIST = 4;

	private Button[] _btSoldier = new Button[SelectItemDlg.NUM_SOLDIERLIST];

	private Button _btSearch;

	private DrawTexture[] _dtSoldierIcon = new DrawTexture[SelectItemDlg.NUM_SOLDIERLIST];

	private DrawTexture[] _dtWeaponIcon = new DrawTexture[SelectItemDlg.NUM_SOLDIERLIST];

	private Label[] _lbSoldierName = new Label[SelectItemDlg.NUM_SOLDIERLIST];

	private Label[] _lbSoldierLevel = new Label[SelectItemDlg.NUM_SOLDIERLIST];

	private DrawTexture[] _dtSoldierLine = new DrawTexture[SelectItemDlg.NUM_SOLDIERLIST];

	private DrawTexture _dtSoldierListOver;

	private DrawTexture[] _dtRepairIcon = new DrawTexture[SelectItemDlg.NUM_SOLDIERLIST];

	private DrawTexture[] _dtRepairIconBG = new DrawTexture[SelectItemDlg.NUM_SOLDIERLIST];

	private TextField _tfSearchSoldier;

	private List<SOLDIER_INFO_EXTEND> SoldierList = new List<SOLDIER_INFO_EXTEND>();

	private List<SOLDIER_INFO_EXTEND> m_ShowList = new List<SOLDIER_INFO_EXTEND>();

	private SelectItemDlg.eTabMode m_TabMode;

	private SelectItemDlg.eType m_DlgType;

	private int m_CurPage = 1;

	private int m_TotalPage = 1;

	private List<int> m_SetPosItem = new List<int>();

	private int m_ItemCurPage = 1;

	private int m_ItemTotalPage = 1;

	private int m_UpdateCount;

	public SelectItemDlg.eType DlgType
	{
		get
		{
			return this.m_DlgType;
		}
		set
		{
			this.m_DlgType = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Item/DLG_SelectItem", G_ID.SELECTITEM_DLG, true);
	}

	public override void SetComponent()
	{
		this._btPrev = (base.GetControl("Button_pre") as Button);
		Button expr_1C = this._btPrev;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickPrev));
		this._btNext = (base.GetControl("Button_next") as Button);
		Button expr_59 = this._btNext;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.OnClickNext));
		this._lbPage = (base.GetControl("Label_page") as Label);
		this._Toolbar = (base.GetControl("ToolBar_ToolBar36") as Toolbar);
		for (int i = 0; i < 2; i++)
		{
			UIPanelTab expr_BA = this._Toolbar.Control_Tab[i];
			expr_BA.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_BA.ButtonClick, new EZValueChangedDelegate(this.OnClickToolbar));
		}
		this._Toolbar.Control_Tab[0].Text = "소지품";
		this._Toolbar.Control_Tab[1].Text = "용병";
		this._Toolbar.FirstSetting();
		base.ShowLayer(1);
		for (int j = 0; j < SelectItemDlg.NUM_ITEMLIST; j++)
		{
			this._btItem[j] = (base.GetControl("Button_ItemBTN" + (j + 1).ToString()) as Button);
			this._btItem[j].data = j;
			Button expr_170 = this._btItem[j];
			expr_170.Click = (EZValueChangedDelegate)Delegate.Combine(expr_170.Click, new EZValueChangedDelegate(this.OnClickItemButton));
			this._dtItemIcon[j] = (base.GetControl("DrawTexture_item" + (j + 1).ToString()) as DrawTexture);
			this._dtItemIcon[j].data = j;
			this._dtItemIcon[j].AddDoubleClickDelegate(new EZValueChangedDelegate(this.OnClickItemDrawTexture));
			this._dtItemIconBG[j] = (base.GetControl("DrawTexture_ItemBG" + (j + 1).ToString()) as DrawTexture);
			this._lbItemName[j] = (base.GetControl("Label_ItemName" + (j + 1).ToString()) as Label);
			this._lbItemDur[j] = (base.GetControl("Label_dur" + (j + 1).ToString()) as Label);
			this._lbItemDurTitle[j] = (base.GetControl("Label_durability" + (j + 1).ToString()) as Label);
		}
		this._dtItemListOver = (base.GetControl("DrawTexture_Active2") as DrawTexture);
		this._btSearch = (base.GetControl("Button_search") as Button);
		Button expr_2CB = this._btSearch;
		expr_2CB.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2CB.Click, new EZValueChangedDelegate(this.OnTextChaged));
		for (int k = 0; k < SelectItemDlg.NUM_SOLDIERLIST; k++)
		{
			this._btSoldier[k] = (base.GetControl("Button_Button" + (k + 1).ToString()) as Button);
			Button expr_324 = this._btSoldier[k];
			expr_324.Click = (EZValueChangedDelegate)Delegate.Combine(expr_324.Click, new EZValueChangedDelegate(this.OnClickSoldierButton));
			this._dtSoldierIcon[k] = (base.GetControl("DrawTexture_general" + (k + 1).ToString()) as DrawTexture);
			this._dtRepairIcon[k] = (base.GetControl("DrawTexture_RepairIcon" + (k + 1).ToString()) as DrawTexture);
			this._dtRepairIconBG[k] = (base.GetControl("DrawTexture_RepairBG" + (k + 1).ToString()) as DrawTexture);
			this._dtWeaponIcon[k] = (base.GetControl("DrawTexture_weapon" + (k + 1).ToString()) as DrawTexture);
			this._lbSoldierName[k] = (base.GetControl("Label_generalname" + (k + 1).ToString()) as Label);
			this._lbSoldierLevel[k] = (base.GetControl("Label_generallv" + (k + 1).ToString()) as Label);
			this._dtSoldierLine[k] = (base.GetControl("DrawTexture_line" + (k + 1).ToString()) as DrawTexture);
		}
		this._dtSoldierListOver = (base.GetControl("DrawTexture_Active1") as DrawTexture);
		this._tfSearchSoldier = (base.GetControl("TextField_namesearch") as TextField);
		this._tfSearchSoldier.SetValueChangedDelegate(new EZValueChangedDelegate(this.OnTextChaged));
		this.ClearSoldierList();
	}

	public override void Update()
	{
		if (this.m_TabMode == SelectItemDlg.eTabMode.Inventory && this.m_UpdateCount != NkUserInventory.GetInstance().m_UpdateCount)
		{
			this.RefreshControl();
		}
	}

	public void RefreshControl()
	{
		this.SetItemList();
		this.ShowInventory();
	}

	private void SetToolTip(IUIObject obj)
	{
		Button button = (Button)obj;
		int num = (this.m_ItemCurPage - 1) * SelectItemDlg.NUM_ITEMLIST;
		num += (int)button.data;
		if (num >= this.m_SetPosItem.Count)
		{
			return;
		}
		if (null == null)
		{
			return;
		}
	}

	private void CloseTooltip(IUIObject obj)
	{
	}

	private void OnClickToolbar(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		Debug.Log("select tab = " + uIPanelTab.panel.index);
		base.ShowLayer(uIPanelTab.panel.index + 1);
		this.m_TabMode = (SelectItemDlg.eTabMode)uIPanelTab.panel.index;
		if (this.m_TabMode == SelectItemDlg.eTabMode.Inventory)
		{
			this.SetItemList();
			this.ShowInventory();
			this.ClearSoldierList();
		}
		else if (this.m_TabMode == SelectItemDlg.eTabMode.Soldier)
		{
			this.ClearInventoryControl();
			this.ShowSoldierList();
		}
	}

	private void OnClickPrev(IUIObject obj)
	{
		if (this.m_TabMode == SelectItemDlg.eTabMode.Inventory)
		{
			if (this.m_ItemCurPage <= 1)
			{
				return;
			}
			this.m_ItemCurPage--;
			this.ShowInventory();
		}
		else if (this.m_TabMode == SelectItemDlg.eTabMode.Soldier)
		{
			if (this.m_CurPage <= 1)
			{
				return;
			}
			this.m_CurPage--;
			this.ShowSoldierList();
		}
	}

	private void OnClickNext(IUIObject obj)
	{
		if (this.m_TabMode == SelectItemDlg.eTabMode.Inventory)
		{
			if (this.m_ItemCurPage >= this.m_ItemTotalPage)
			{
				return;
			}
			this.m_ItemCurPage++;
			this.ShowInventory();
		}
		else
		{
			if (this.m_CurPage >= this.m_TotalPage)
			{
				return;
			}
			this.m_CurPage++;
			this.ShowSoldierList();
		}
	}

	private void OnClickItemDrawTexture(IUIObject obj)
	{
		DrawTexture drawTexture = (DrawTexture)obj;
		int num = (this.m_ItemCurPage - 1) * SelectItemDlg.NUM_ITEMLIST;
		num += (int)drawTexture.data;
		if (num >= this.m_SetPosItem.Count)
		{
			return;
		}
		if (null == null)
		{
			return;
		}
		SelectItemDlg.eType dlgType = this.DlgType;
		if (dlgType != SelectItemDlg.eType.Enhance)
		{
		}
		this._dtItemListOver.SetLocation(drawTexture.GetLocation().x, drawTexture.GetLocationY());
		this._dtItemListOver.Visible = true;
	}

	private void OnClickItemButton(IUIObject obj)
	{
		Button button = (Button)obj;
		int num = (this.m_ItemCurPage - 1) * SelectItemDlg.NUM_ITEMLIST;
		num += (int)button.data;
		if (num >= this.m_SetPosItem.Count)
		{
			return;
		}
		if (null == null)
		{
			return;
		}
		SelectItemDlg.eType dlgType = this.DlgType;
		if (dlgType != SelectItemDlg.eType.Enhance)
		{
		}
		this._dtItemListOver.SetLocation(button.GetLocation().x, button.GetLocationY());
		this._dtItemListOver.Visible = true;
	}

	private void OnClickSoldierButton(IUIObject obj)
	{
		Button button = obj as Button;
		this.ShowSolEquipList((long)button.data, this.m_DlgType);
		this._dtSoldierListOver.SetLocation(button.GetLocation().x, button.GetLocationY());
	}

	private void ClearSoldierList()
	{
		for (int i = 0; i < SelectItemDlg.NUM_SOLDIERLIST; i++)
		{
			this._btSoldier[i].data = 0;
			this._btSoldier[i].controlIsEnabled = false;
			this._lbSoldierName[i].Text = string.Empty;
			this._lbSoldierLevel[i].Text = string.Empty;
			this._dtSoldierIcon[i].Image = null;
			this._dtRepairIcon[i].Visible = false;
			this._dtRepairIconBG[i].Visible = false;
			this._dtWeaponIcon[i].Image = null;
			this._dtSoldierLine[i].Visible = false;
		}
		this._dtSoldierListOver.Visible = false;
		this._btSearch.Visible = false;
	}

	private void ShowSoldierList()
	{
		this.SoldierList.Clear();
		this.m_ShowList.Clear();
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			if (soldierInfo != null)
			{
				if (soldierInfo.GetSolID() > 0L)
				{
					SOLDIER_INFO_EXTEND sOLDIER_INFO_EXTEND = this.SetUserData(soldierInfo);
					if (sOLDIER_INFO_EXTEND != null)
					{
						this.SoldierList.Add(sOLDIER_INFO_EXTEND);
						this.m_ShowList.Add(sOLDIER_INFO_EXTEND);
					}
					num++;
				}
			}
		}
		this.m_TotalPage = (num - 1) / SelectItemDlg.NUM_SOLDIERLIST + 1;
		if (this.m_TotalPage <= 0)
		{
			this.m_TotalPage = 1;
		}
		this.ShowList();
	}

	private void ShowList()
	{
		this.ClearSoldierList();
		if (this.m_ShowList.Count == 0)
		{
			return;
		}
		for (int i = 0; i < SelectItemDlg.NUM_SOLDIERLIST; i++)
		{
			int num = i + (this.m_CurPage - 1) * SelectItemDlg.NUM_SOLDIERLIST;
			if (num >= this.m_ShowList.Count)
			{
				break;
			}
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_ShowList[num].CharKind);
			if (charKindInfo != null)
			{
				this._btSoldier[i].controlIsEnabled = true;
				this._dtSoldierLine[i].Visible = true;
				this._dtSoldierIcon[i].SetTexture(eCharImageType.SMALL, this.m_ShowList[num].CharKind, (int)this.m_ShowList[num].Grade, string.Empty);
				this._dtRepairIcon[i].Visible = false;
				this._dtRepairIconBG[i].Visible = false;
				string text = charKindInfo.GetName();
				if (charKindInfo.GetCharKind() < 10)
				{
					NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
					text = nrCharUser.GetPersonInfo().GetCharName();
				}
				this._lbSoldierName[i].Text = text;
				this._lbSoldierLevel[i].Text = "Lv." + this.m_ShowList[num].Level.ToString();
				this._btSoldier[i].data = this.m_ShowList[num].SolID;
			}
		}
		this._lbPage.Text = this.m_CurPage + " / " + this.m_TotalPage;
		this._btSearch.Visible = true;
	}

	private void SetItemList()
	{
		this.m_SetPosItem.Clear();
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			ITEM iTEM = null;
			if (iTEM != null)
			{
				if (NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique) != null)
				{
					if (this.m_DlgType != SelectItemDlg.eType.Repair || iTEM.m_nDurability < 100)
					{
						this.m_SetPosItem.Add(iTEM.m_nItemPos);
					}
				}
			}
		}
		Debug.Log("SelectItemDlg : SetItemCount - " + this.m_SetPosItem.Count.ToString());
		this.m_ItemCurPage = 1;
		this.m_ItemTotalPage = Math.Abs(this.m_SetPosItem.Count - 1) / SelectItemDlg.NUM_ITEMLIST + 1;
		this.m_UpdateCount = NkUserInventory.GetInstance().m_UpdateCount;
	}

	private void ShowInventory()
	{
		this.ClearInventoryControl();
		this._lbPage.Text = this.m_ItemCurPage + " / " + this.m_ItemTotalPage;
	}

	private void ClearInventoryControl()
	{
		for (int i = 0; i < SelectItemDlg.NUM_ITEMLIST; i++)
		{
			this._dtItemIcon[i].Image = null;
			this._dtItemIconBG[i].Visible = false;
			this._dtItemIcon[i].c_cItemTooltip = null;
			this._lbItemName[i].Text = string.Empty;
			this._lbItemDur[i].Text = string.Empty;
			this._lbItemDurTitle[i].Visible = false;
			this._btItem[i].controlIsEnabled = false;
		}
		this._dtItemListOver.Visible = false;
	}

	private void ShowSolEquipList(long SolID, SelectItemDlg.eType Type)
	{
		if (SolID <= 0L)
		{
			return;
		}
		ItemListDlg itemListDlg = base.SetChildForm(G_ID.ITEMLIST_DLG) as ItemListDlg;
		itemListDlg.RequestItemList(SolID);
		this.m_DlgType = Type;
	}

	public void SetDlgType(SelectItemDlg.eType type)
	{
		this.m_DlgType = type;
		if (this.m_DlgType == SelectItemDlg.eType.Enhance)
		{
			base.InteractivePanel.twinFormID = G_ID.ENHANCEITEM_DLG;
		}
		else if (this.m_DlgType == SelectItemDlg.eType.Repair)
		{
			base.InteractivePanel.twinFormID = G_ID.REPAIRITEM_DLG;
		}
		else if (this.m_DlgType == SelectItemDlg.eType.Dissemblie)
		{
			base.InteractivePanel.twinFormID = G_ID.DISASSEMBLEITEM_DLG;
		}
	}

	private void OnTextChaged(IUIObject obj)
	{
		this.m_ShowList.Clear();
		string text = this._tfSearchSoldier.Text;
		text = text.TrimEnd(new char[0]);
		if (this._tfSearchSoldier.Text != string.Empty)
		{
			for (int i = 0; i < this.SoldierList.Count; i++)
			{
				if (InitialSearch.IsCheckString(this.SoldierList[i].Name, text))
				{
					this.m_ShowList.Add(this.SoldierList[i]);
				}
			}
			this.ShowList();
		}
		else
		{
			this.ShowSoldierList();
		}
	}

	private SOLDIER_INFO_EXTEND SetUserData(NkSoldierInfo pkSolinfo)
	{
		SOLDIER_INFO_EXTEND sOLDIER_INFO_EXTEND = new SOLDIER_INFO_EXTEND(pkSolinfo);
		if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(sOLDIER_INFO_EXTEND.CharKind) == null)
		{
			return null;
		}
		sOLDIER_INFO_EXTEND.Name = pkSolinfo.GetName();
		return sOLDIER_INFO_EXTEND;
	}

	public override void OnClose()
	{
	}
}
