using GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class AuctionItemSelectDlg : Form
{
	private Toolbar m_tbTab;

	private NewListBox m_nlbItemList;

	private Button m_btOK;

	private string m_strText = string.Empty;

	private AuctionDefine.eAUCTIONREGISTERTYPE m_eAuctionRegisterType;

	private List<NkSoldierInfo> m_kSolList = new List<NkSoldierInfo>();

	private string m_strMessage = string.Empty;

	private string m_strMsg = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Auction/DLG_AuctionItemSelect", G_ID.AUCTION_ITEMSELECT_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_tbTab = (base.GetControl("ToolBar") as Toolbar);
		this.m_tbTab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("451");
		this.m_tbTab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1490");
		UIPanelTab expr_65 = this.m_tbTab.Control_Tab[0];
		expr_65.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_65.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_93 = this.m_tbTab.Control_Tab[1];
		expr_93.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_93.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_nlbItemList = (base.GetControl("NewListBox_AuctionItemSelect") as NewListBox);
		this.m_nlbItemList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickList));
		this.m_btOK = (base.GetControl("Button_OK") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		this.SelectTab(this.m_eAuctionRegisterType);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClickList(IUIObject obj)
	{
		this.SelectItem();
	}

	public void ClickOK(IUIObject obj)
	{
		this.SelectItem();
	}

	public void ShowItemList()
	{
		this.m_nlbItemList.Clear();
		int num = 5;
		for (int i = 1; i < num; i++)
		{
			for (int j = 0; j < ItemDefine.INVENTORY_ITEMSLOT_MAX; j++)
			{
				ITEM item = NkUserInventory.GetInstance().GetItem(i, j);
				if (item != null)
				{
					if (0 < item.m_nOption[7])
					{
						ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
						if (itemInfo != null)
						{
							if (!itemInfo.IsItemATB(131072L) && !itemInfo.IsItemATB(524288L))
							{
								this.SetAddItemInfo(item, itemInfo);
							}
						}
					}
				}
			}
		}
		this.m_nlbItemList.RepositionItems();
	}

	public void SelectItem()
	{
		UIListItemContainer uIListItemContainer = this.m_nlbItemList.GetSelectItem() as UIListItemContainer;
		if (null == uIListItemContainer)
		{
			return;
		}
		if (uIListItemContainer.Data == null)
		{
			return;
		}
		if (this.m_eAuctionRegisterType == AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_ITEM)
		{
			ITEM iTEM = uIListItemContainer.data as ITEM;
			if (iTEM.IsLock())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("726"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				return;
			}
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
				auctionMainDlg.SetSelectItem(iTEM);
			}
		}
		else if (this.m_eAuctionRegisterType == AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_SOL)
		{
			NkSoldierInfo selectSol = uIListItemContainer.data as NkSoldierInfo;
			AuctionMainDlg auctionMainDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg2 != null)
			{
				auctionMainDlg2.SetSelectSol(selectSol);
			}
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_ITEMSELECT_DLG);
	}

	public void SetAddItemInfo(ITEM InvenItem, ITEMINFO Iteminfo)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbItemList.ColumnNum, true);
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(InvenItem);
		newListItem.SetListItemData(0, true);
		newListItem.SetListItemData(1, InvenItem, null, null, null);
		newListItem.SetListItemData(2, rankColorName, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			InvenItem.m_nOption[7]
		});
		newListItem.SetListItemData(3, this.m_strText, null, null, null);
		newListItem.SetListItemData(4, string.Empty, null, new EZValueChangedDelegate(this.ClickDetailInfo), null);
		newListItem.SetListItemData(5, InvenItem.IsLock());
		newListItem.Data = InvenItem;
		this.m_nlbItemList.Add(newListItem);
	}

	public void ClickDetailInfo(IUIObject obj)
	{
		UIListItemContainer uIListItemContainer = this.m_nlbItemList.GetSelectItem() as UIListItemContainer;
		if (null == uIListItemContainer)
		{
			return;
		}
		if (uIListItemContainer.Data == null)
		{
			return;
		}
		ITEM iTEM = uIListItemContainer.data as ITEM;
		if (iTEM != null)
		{
			AuctionMainDlg.ShowItemDetailInfo(iTEM, (G_ID)base.WindowID);
		}
		else
		{
			NkSoldierInfo nkSoldierInfo = uIListItemContainer.data as NkSoldierInfo;
			if (nkSoldierInfo != null)
			{
				AuctionMainDlg.ShowSolDetailInfo(nkSoldierInfo, this);
			}
		}
	}

	public void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_eAuctionRegisterType = (AuctionDefine.eAUCTIONREGISTERTYPE)uIPanelTab.panel.index;
		this.SelectTab(this.m_eAuctionRegisterType);
	}

	public void SelectTab(AuctionDefine.eAUCTIONREGISTERTYPE eAuctionRegisterType)
	{
		this.ChangeTab();
		if (eAuctionRegisterType != AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_ITEM)
		{
			if (eAuctionRegisterType == AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_SOL)
			{
				this.ShowSolList();
			}
		}
		else
		{
			this.ShowItemList();
		}
	}

	public void ChangeTab()
	{
	}

	public void ShowSolList()
	{
		this.MakeReadySolList();
		this.m_nlbItemList.Clear();
		for (int i = 0; i < this.m_kSolList.Count; i++)
		{
			if (0L < this.m_kSolList[i].GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_TRADE_COUNT))
			{
				this.SetAddSolInfo(this.m_kSolList[i], this.m_strMessage, this.m_strMsg);
			}
		}
		this.m_nlbItemList.RepositionItems();
	}

	private void MakeReadySolList()
	{
		this.m_kSolList.Clear();
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
		if (pkSolinfo.GetSolPosType() != (byte)eAddPosType)
		{
			return;
		}
		this.m_kSolList.Add(pkSolinfo);
	}

	public void SetAddSolInfo(NkSoldierInfo kSolInfo, string strMessage, string strMsg)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbItemList.ColumnNum, true);
		newListItem.SetListItemData(0, true);
		newListItem.SetListItemData(1, kSolInfo.GetListSolInfo(true), null, null, null);
		newListItem.SetListItemData(2, kSolInfo.GetName(), null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref strMessage, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			kSolInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_TRADE_COUNT)
		});
		newListItem.SetListItemData(3, strMessage, null, null, null);
		newListItem.SetListItemData(4, string.Empty, null, new EZValueChangedDelegate(this.ClickDetailInfo), null);
		newListItem.SetListItemData(5, false);
		newListItem.Data = kSolInfo;
		this.m_nlbItemList.Add(newListItem);
	}
}
