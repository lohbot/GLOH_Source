using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class ItemSellDlg : Form
{
	private ItemTexture m_iItemIcon;

	private DrawTexture m_dIconBack;

	private Label m_lItemName;

	private Label m_lSellGold;

	private Label m_lItemCount;

	private DrawTexture m_dNpcImg;

	private Button m_bSellOK;

	private ITEM m_cItem;

	private List<ITEM> m_cAllItem;

	private int m_nAllItemMoney;

	private bool m_bRankItem;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Item/DLG_ItemSell", G_ID.ITEMSELL_DLG, false);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_iItemIcon = (base.GetControl("ItemTexture_ItemIcon") as ItemTexture);
		this.m_dIconBack = (base.GetControl("DrawTexture_ItemBG") as DrawTexture);
		this.m_lItemName = (base.GetControl("Label_ItemName") as Label);
		this.m_lSellGold = (base.GetControl("Label_Gold") as Label);
		this.m_lItemCount = (base.GetControl("Label_ItemNum") as Label);
		this.m_dNpcImg = (base.GetControl("DrawTexture_NPCIMG") as DrawTexture);
		this.m_bSellOK = (base.GetControl("Button_OK") as Button);
		Button expr_A0 = this.m_bSellOK;
		expr_A0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_A0.Click, new EZValueChangedDelegate(this.OnSellOKCheck));
		base.SetScreenCenter();
	}

	public void SetMultiItemSellInfo(List<ITEM> l_cItem, int nAllMoney, ITEM TopItem, bool bRankItem)
	{
		if (TopItem == null || TopItem.m_nItemUnique <= 0)
		{
			return;
		}
		this.m_cAllItem = l_cItem;
		this.m_nAllItemMoney = nAllMoney;
		this.m_bRankItem = bRankItem;
		this.m_cItem = null;
		this.m_iItemIcon.SetItemTexture(TopItem);
		this.m_iItemIcon.c_cItemTooltip = TopItem;
		this.m_dIconBack.SetTexture(TopItem.GetRankImage());
		this.m_lItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(TopItem);
		this.m_lSellGold.Text = Protocol_Item.Money_Format((long)nAllMoney);
		string empty = string.Empty;
		int num = l_cItem.Count - 1;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("295"),
			"count",
			num.ToString()
		});
		this.m_lItemCount.SetText(empty);
		this.m_dNpcImg.SetTexture(eCharImageType.LARGE, 242, -1);
	}

	public void SetItemSellInfo(ITEM cItem, int nMoney)
	{
		this.m_cItem = cItem;
		this.m_iItemIcon.SetItemTexture(this.m_cItem);
		this.m_iItemIcon.c_cItemTooltip = this.m_cItem;
		this.m_dIconBack.SetTexture(this.m_cItem.GetRankImage());
		this.m_lItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_cItem);
		this.m_lSellGold.Text = Protocol_Item.Money_Format((long)nMoney);
		this.m_lItemCount.Visible = false;
		this.m_dNpcImg.SetTexture(eCharImageType.LARGE, 242, -1);
	}

	public override void InitData()
	{
	}

	public void OnSellOKCheck(IUIObject obj)
	{
		if (this.m_cItem != null && this.m_cItem.IsLock())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("726"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			return;
		}
		if (this.m_bRankItem)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI == null)
			{
				return;
			}
			msgBoxUI.SetMsg(new YesDelegate(this.OnSellOKSend), null, new NoDelegate(this.OnSellCancelSend), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("57"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("67"), eMsgType.MB_OK_CANCEL);
		}
		else
		{
			this.OnSellOK();
		}
	}

	public void OnSellCancelSend(object a_oObject)
	{
		this.Close();
	}

	public void OnSellOKSend(object a_oObject)
	{
		this.OnSellOK();
	}

	public void OnSellOK()
	{
		if (this.m_cItem != null)
		{
			if (Protocol_Market.s_lsSellItem.Count >= 30)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("237");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			Protocol_Item.Send_AutoItemSell(this.m_cItem.m_nItemID);
		}
		else
		{
			GS_ITEM_SELL_MULTI_REQ gS_ITEM_SELL_MULTI_REQ = new GS_ITEM_SELL_MULTI_REQ();
			for (int i = 0; i < this.m_cAllItem.Count; i++)
			{
				if (this.m_cAllItem[i] != null && this.m_cAllItem[i].m_nItemUnique > 0)
				{
					gS_ITEM_SELL_MULTI_REQ.i64ItemID[i] = this.m_cAllItem[i].m_nItemID;
				}
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SELL_MULTI_REQ, gS_ITEM_SELL_MULTI_REQ);
		}
		this.Close();
	}

	public override void OnClose()
	{
		Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
		if (inventory_Dlg != null && this.m_nAllItemMoney != 0)
		{
			inventory_Dlg.InitMultiItem();
		}
	}
}
