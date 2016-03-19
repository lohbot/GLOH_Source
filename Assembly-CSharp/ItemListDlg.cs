using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using System.Text;
using TsBundle;
using UnityForms;

public class ItemListDlg : Form
{
	private Label _lbTitle;

	private Button _btPrev;

	private Button _btNext;

	private Box m_Box_page;

	private Button[] _btItem = new Button[ItemListDlg.NUM_ITEMLIST];

	private ItemTexture[] _dtItemIcon = new ItemTexture[ItemListDlg.NUM_ITEMLIST];

	private DrawTexture[] _dtItemBG = new DrawTexture[ItemListDlg.NUM_ITEMLIST];

	private DrawTexture _dtSelectItem;

	private Label[] _lbItemName = new Label[ItemListDlg.NUM_ITEMLIST];

	private Label[] _lbItemDur = new Label[ItemListDlg.NUM_ITEMLIST];

	private Label[] _lbItemDurText = new Label[ItemListDlg.NUM_ITEMLIST];

	private DrawTexture[] m_DrawTexture_Select = new DrawTexture[ItemListDlg.NUM_ITEMLIST];

	private Button m_Button_arrow1_C;

	private Button m_Button_arrow2_C;

	private Button m_Button_AllRepair;

	private Box m_Box_page_C;

	private long m_SolID;

	private long m_SetSolID;

	private int m_CurrentPage;

	private int m_TotalPage;

	private List<ITEM> m_ItemList = new List<ITEM>();

	private int m_UpdateCount;

	private static int NUM_ITEMLIST
	{
		get
		{
			if (TsPlatform.IsMobile)
			{
				return 8;
			}
			return 7;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Item/DLG_ItemList", G_ID.ITEMLIST_DLG, true);
	}

	private void ShowHideControl()
	{
		SelectItemDlg selectItemDlg = (SelectItemDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SELECTITEM_DLG);
		if (selectItemDlg.DlgType == SelectItemDlg.eType.Repair)
		{
			base.ShowLayer(1);
			this._btPrev.Visible = false;
			this._btNext.Visible = false;
			this.m_Box_page.Visible = false;
		}
		else
		{
			base.SetShowLayer(1, false);
		}
	}

	public override void SetComponent()
	{
		this._lbTitle = (base.GetControl("Label_title") as Label);
		this._btPrev = (base.GetControl("Button_arrow1") as Button);
		Button expr_32 = this._btPrev;
		expr_32.Click = (EZValueChangedDelegate)Delegate.Combine(expr_32.Click, new EZValueChangedDelegate(this.OnClickPrev));
		this._btNext = (base.GetControl("Button_arrow2") as Button);
		Button expr_6F = this._btNext;
		expr_6F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_6F.Click, new EZValueChangedDelegate(this.OnClickNext));
		this.m_Box_page = (base.GetControl("Box_page") as Box);
		for (int i = 0; i < ItemListDlg.NUM_ITEMLIST; i++)
		{
			this._btItem[i] = (base.GetControl("Button_Button" + i.ToString()) as Button);
			this._btItem[i].data = i;
			Button expr_EC = this._btItem[i];
			expr_EC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_EC.Click, new EZValueChangedDelegate(this.ClickItemButton));
			this._dtItemIcon[i] = (base.GetControl("ItemTexture_Item" + (i + 1).ToString()) as ItemTexture);
			this._dtItemIcon[i].data = i;
			this._dtItemIcon[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemDrawTexture));
			this._dtItemBG[i] = (base.GetControl("DrawTexture_ItemBG" + (i + 1).ToString()) as DrawTexture);
			this._lbItemName[i] = (base.GetControl("Label_itemname" + (i + 1).ToString()) as Label);
			this._lbItemDur[i] = (base.GetControl("Label_dur" + (i + 1).ToString()) as Label);
			this._lbItemDurText[i] = (base.GetControl("Label_durability" + (i + 1).ToString()) as Label);
			this.m_DrawTexture_Select[i] = (base.GetControl("DrawTexture_Select" + i.ToString()) as DrawTexture);
		}
		this._dtSelectItem = (base.GetControl("DrawTexture_Active1") as DrawTexture);
		this.m_Box_page_C = (base.GetControl("Box_page_C") as Box);
		this.m_Button_arrow1_C = (base.GetControl("Button_arrow1_C") as Button);
		Button expr_27E = this.m_Button_arrow1_C;
		expr_27E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_27E.Click, new EZValueChangedDelegate(this.OnClickPrev));
		this.m_Button_arrow2_C = (base.GetControl("Button_arrow2_C") as Button);
		Button expr_2BB = this.m_Button_arrow2_C;
		expr_2BB.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2BB.Click, new EZValueChangedDelegate(this.OnClickNext));
		this.m_Button_AllRepair = (base.GetControl("Button_AllRepair") as Button);
		Button expr_2F8 = this.m_Button_AllRepair;
		expr_2F8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2F8.Click, new EZValueChangedDelegate(this.OnClickRepairAll));
		this.ShowHideControl();
		this.ClearItemListCtrls();
	}

	private void OnClickPrev(IUIObject obj)
	{
		if (this.m_CurrentPage > 1)
		{
			this.m_CurrentPage--;
			this.ShowItemList();
		}
	}

	private void OnClickRepairAll(IUIObject obj)
	{
		if (this.m_SolID == 0L)
		{
			return;
		}
	}

	public void On_OK_RepairAll(MsgBoxUI a_cthis, object a_oObject)
	{
	}

	private void OnClickNext(IUIObject obj)
	{
		if (this.m_CurrentPage < this.m_TotalPage)
		{
			this.m_CurrentPage++;
			this.ShowItemList();
		}
	}

	private void SetToolTip(IUIObject obj)
	{
		Button button = (Button)obj;
		int num = (this.m_CurrentPage - 1) * ItemListDlg.NUM_ITEMLIST;
		num += (int)button.data;
		if (num >= this.m_ItemList.Count)
		{
			return;
		}
	}

	private void CloseTooltip(IUIObject obj)
	{
	}

	private void ClickItemDrawTexture(IUIObject obj)
	{
		ItemTexture itemTexture = (ItemTexture)obj;
		int num = (this.m_CurrentPage - 1) * ItemListDlg.NUM_ITEMLIST;
		num += (int)itemTexture.data;
		if (num >= this.m_ItemList.Count)
		{
			return;
		}
		SelectItemDlg selectItemDlg = (SelectItemDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SELECTITEM_DLG);
		SelectItemDlg.eType dlgType = selectItemDlg.DlgType;
		if (dlgType != SelectItemDlg.eType.Enhance)
		{
		}
		if (this._btItem[num] != null)
		{
			this._dtSelectItem.SetLocation(this._btItem[num].GetLocation().x, this._btItem[num].GetLocationY());
			this._dtSelectItem.Visible = true;
		}
		string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(this.m_ItemList[num].m_nItemUnique);
		if (!string.IsNullOrEmpty(itemMaterialCode))
		{
			TsAudioManager.Container.RequestAudioClip("UI_ITEM", itemMaterialCode, "DROP", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		if (TsPlatform.IsMobile)
		{
			selectItemDlg.CloseNow();
		}
	}

	private void ClickItemButton(IUIObject obj)
	{
		Button button = (Button)obj;
		int num = (this.m_CurrentPage - 1) * ItemListDlg.NUM_ITEMLIST;
		num += (int)button.data;
		if (num >= this.m_ItemList.Count)
		{
			return;
		}
		if (num >= this.m_ItemList.Count)
		{
			return;
		}
		SelectItemDlg selectItemDlg = (SelectItemDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SELECTITEM_DLG);
		this._dtSelectItem.SetLocation(button.GetLocation().x, button.GetLocationY());
		this._dtSelectItem.Visible = true;
		string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(this.m_ItemList[num].m_nItemUnique);
		if (!string.IsNullOrEmpty(itemMaterialCode))
		{
			TsAudioManager.Container.RequestAudioClip("UI_ITEM", itemMaterialCode, "DROP", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		if (TsPlatform.IsMobile)
		{
			selectItemDlg.CloseNow();
		}
	}

	public void RequestItemList(long SolID)
	{
		if (this.m_SolID == SolID)
		{
			return;
		}
		this.m_SetSolID = SolID;
		this.m_UpdateCount = 0;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(SolID);
		if (soldierInfoFromSolID == null)
		{
			return;
		}
		string name = soldierInfoFromSolID.GetName();
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("260"),
			"charname",
			name
		});
		this._lbTitle.Text = empty;
		if (soldierInfoFromSolID.GetItemUpdateCount() == 0 && !soldierInfoFromSolID.IsItemReceiveData())
		{
			GS_SOLDIER_EQUIPITEM_REQ gS_SOLDIER_EQUIPITEM_REQ = new GS_SOLDIER_EQUIPITEM_REQ();
			gS_SOLDIER_EQUIPITEM_REQ.SolID = SolID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_EQUIPITEM_REQ, gS_SOLDIER_EQUIPITEM_REQ);
			return;
		}
	}

	public override void Update()
	{
		if (this.m_SetSolID == 0L)
		{
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(this.m_SetSolID);
		if (soldierInfoFromSolID == null)
		{
			return;
		}
		if (this.m_SolID == this.m_SetSolID && soldierInfoFromSolID.GetItemUpdateCount() == this.m_UpdateCount)
		{
			return;
		}
		this.m_SolID = this.m_SetSolID;
		this.SetItemList();
	}

	public void SetItemList()
	{
		if (this.m_SolID == 0L)
		{
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(this.m_SolID);
		if (soldierInfoFromSolID == null)
		{
			return;
		}
		this.m_ItemList.Clear();
		SelectItemDlg selectItemDlg = (SelectItemDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SELECTITEM_DLG);
		for (int i = 0; i < 6; i++)
		{
			ITEM item = soldierInfoFromSolID.GetEquipItemInfo().m_kItem[i].GetItem();
			if (item != null)
			{
				if (item.m_nItemUnique > 0)
				{
					ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
					if (itemInfo != null)
					{
						if (selectItemDlg.DlgType == SelectItemDlg.eType.Enhance && item.m_nRank >= 10)
						{
							if (itemInfo == null)
							{
								goto IL_12F;
							}
							if ((int)soldierInfoFromSolID.GetLevel() < itemInfo.GetUseMinLevel(item))
							{
								goto IL_12F;
							}
						}
						if (selectItemDlg.DlgType != SelectItemDlg.eType.Repair || item.m_nDurability < 100)
						{
							if (selectItemDlg.DlgType == SelectItemDlg.eType.Dissemblie)
							{
							}
							this.m_ItemList.Add(soldierInfoFromSolID.GetEquipItemInfo().m_kItem[i].GetItem());
						}
					}
				}
			}
			IL_12F:;
		}
		this.m_TotalPage = Math.Abs(this.m_ItemList.Count - 1) / ItemListDlg.NUM_ITEMLIST + 1;
		this.m_CurrentPage = 1;
		this.m_UpdateCount = soldierInfoFromSolID.GetItemUpdateCount();
		this.ShowItemList();
	}

	public void ShowItemList()
	{
		this.ClearItemListCtrls();
		int num = (this.m_CurrentPage - 1) * ItemListDlg.NUM_ITEMLIST;
		int num2 = num;
		for (int i = 0; i < ItemListDlg.NUM_ITEMLIST; i++)
		{
			if (num2 >= this.m_ItemList.Count)
			{
				break;
			}
			long num3 = (long)this.m_ItemList[num2].m_nItemUnique;
			if (num3 > 0L)
			{
				this._dtItemIcon[i].SetItemTexture(this.m_ItemList[num2]);
				this._dtItemIcon[i].c_cItemTooltip = this.m_ItemList[num2];
				this._dtItemIcon[i].Visible = true;
				this.DrawItemName(i, this.m_ItemList[num2]);
				string szColorNum = string.Empty;
				if (this.m_ItemList[num2].m_nDurability > 20)
				{
					szColorNum = "1203";
				}
				else if (this.m_ItemList[num2].m_nDurability > 0)
				{
					szColorNum = "1204";
				}
				else
				{
					szColorNum = "1501";
				}
				this._lbItemDur[i].Text = NrTSingleton<CTextParser>.Instance.GetTextColor(szColorNum) + this.m_ItemList[num2].m_nDurability.ToString() + NrTSingleton<CTextParser>.Instance.GetTextColor("1203") + " / 100";
				this._lbItemDurText[i].Visible = true;
				this._dtItemBG[i].Visible = true;
				this._btItem[i].controlIsEnabled = true;
			}
			num2++;
		}
		this.m_Box_page.Text = this.m_CurrentPage + " / " + this.m_TotalPage;
		this.m_Box_page_C.Text = this.m_CurrentPage + " / " + this.m_TotalPage;
	}

	public void DrawItemName(int slotidx, ITEM iteminfo)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (iteminfo.m_nRank > 0)
		{
			stringBuilder.Append("+" + iteminfo.m_nRank + " ");
		}
		stringBuilder.Append(NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(iteminfo));
		this._lbItemName[slotidx].Text = stringBuilder.ToString();
	}

	public void ClearItemListCtrls()
	{
		for (int i = 0; i < ItemListDlg.NUM_ITEMLIST; i++)
		{
			this._dtItemIcon[i].ClearData();
			this._dtItemIcon[i].c_cItemTooltip = null;
			this._dtItemIcon[i].Visible = false;
			this._lbItemName[i].Text = string.Empty;
			this._lbItemDur[i].Text = string.Empty;
			this._lbItemDurText[i].Visible = false;
			this._dtItemBG[i].Visible = false;
			this._btItem[i].controlIsEnabled = false;
			this.m_DrawTexture_Select[i].Visible = false;
		}
		this._dtSelectItem.Visible = false;
		this.m_Button_AllRepair.Visible = false;
	}

	public override void OnClose()
	{
		if (TsPlatform.IsMobile)
		{
			Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SELECTITEM_DLG);
			if (form != null)
			{
				form.Show();
				UIPanelManager.instance.DepthChange(form.InteractivePanel);
			}
		}
	}

	public void NotifyStartDisassemble(long itemID)
	{
		int num = (this.m_CurrentPage - 1) * ItemListDlg.NUM_ITEMLIST;
		int num2 = num;
		for (int i = 0; i < ItemListDlg.NUM_ITEMLIST; i++)
		{
			if (num2 >= this.m_ItemList.Count)
			{
				break;
			}
			if (this.m_ItemList[num2].m_nItemID == itemID)
			{
				this.DrawItemName(i, this.m_ItemList[num2]);
				break;
			}
			num2++;
		}
	}
}
