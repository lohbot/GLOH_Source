using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class PoPupShopDlg : Form
{
	private const ItemMallPoPupShopManager.ePoPupShop_Type m_ePopUpSellType = ItemMallPoPupShopManager.ePoPupShop_Type.NONE;

	private const ItemMallItemManager.eItemMall_SellType m_eSellType = ItemMallItemManager.eItemMall_SellType.FIVEROCKSEVENT;

	private ITEM_MALL_ITEM m_ItemMall;

	private ITEMMALL_POPUPSHOP m_itemPopUpData;

	private DrawTexture m_dtItemImage;

	private Button m_bBuy;

	private Button m_bCancel;

	private CheckBox m_cbCheckBox;

	private bool bcheck;

	private long m_nCurrPopUpShopIdx;

	private long m_nCurrPopUpIdx;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Item/dlg_popupshop", G_ID.ITEMMALL_POPUPSHOP_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtItemImage = (base.GetControl("DrawTexture_AD") as DrawTexture);
		this.m_bBuy = (base.GetControl("Button_Buy") as Button);
		this.m_bCancel = (base.GetControl("Button_Cancel") as Button);
		this.m_bBuy.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBuy));
		this.m_bCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickCancel));
		this.m_cbCheckBox = (base.GetControl("CheckBox_01") as CheckBox);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.8f);
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
		base.DonotDepthChange(-864f);
		base.SetScreenCenter();
	}

	public void SetData(long Idx)
	{
		ITEMMALL_POPUPSHOP originalItem = NrTSingleton<ItemMallPoPupShopManager>.Instance.GetOriginalItem(Idx);
		if (originalItem != null)
		{
			this.m_itemPopUpData = originalItem;
			string textureFromBundle = "ui/itemshop/" + originalItem.m_strIconPath;
			if (NrTSingleton<ContentsLimitManager>.Instance.IsShopProduct((long)originalItem.m_Idx))
			{
				this.m_dtItemImage.SetTextureFromBundle(textureFromBundle);
				this.m_nCurrPopUpIdx = Idx;
				this.m_nCurrPopUpShopIdx = originalItem.m_nShopIDX;
			}
			else
			{
				this.bcheck = true;
				this.Close();
			}
		}
		else
		{
			this.bcheck = true;
			this.Close();
		}
	}

	public void OnClickBuy(IUIObject obj)
	{
		string empty = string.Empty;
		if (this.m_nCurrPopUpShopIdx != 0L)
		{
			ITEM_MALL_ITEM item = NrTSingleton<ItemMallItemManager>.Instance.GetItem(this.m_nCurrPopUpShopIdx);
			if (item == null)
			{
				return;
			}
			this.m_ItemMall = item;
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("78"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(item.m_strTextKey)
				});
				if (item.m_nGroup == 8)
				{
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(item.m_strTextKey),
						"count",
						item.m_nItemNum
					});
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("78"),
						"targetname",
						empty2
					});
				}
				msgBoxUI.OkEventImmediatelyClose = false;
				NrTSingleton<ItemMallItemManager>.Instance.CheckMsgBox = msgBoxUI;
				msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), null, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty, eMsgType.MB_OK_CANCEL);
				msgBoxUI.SetLocation(msgBoxUI.GetLocationX(), msgBoxUI.GetLocationY(), -900f);
			}
		}
		else
		{
			TsLog.LogError("CurrPopUpShopIdx", new object[]
			{
				this.m_nCurrPopUpShopIdx
			});
		}
	}

	public void OnClickCancel(IUIObject obj)
	{
		ITEMMALL_POPUPSHOP originalItem = NrTSingleton<ItemMallPoPupShopManager>.Instance.GetOriginalItem(this.m_nCurrPopUpIdx);
		if (originalItem == null)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			if (this.m_cbCheckBox.IsChecked())
			{
				msgBoxUI.SetMsg(new YesDelegate(this.OnClickClose), null, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("352"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox(originalItem.m_strCloseDayText), eMsgType.MB_OK_CANCEL);
			}
			else
			{
				msgBoxUI.SetMsg(new YesDelegate(this.OnClickClose), null, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("352"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox(originalItem.m_strCloseText), eMsgType.MB_OK_CANCEL);
			}
			msgBoxUI.SetLocation(msgBoxUI.GetLocationX(), msgBoxUI.GetLocationY(), -900f);
		}
	}

	public void MsgBoxOKEvent(object EventObject)
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsShopProduct(this.m_ItemMall.m_Idx) || !BaseNet_Game.GetInstance().IsSocketConnected() || !NrTSingleton<ItemMallItemManager>.Instance.BuyItem(this.m_ItemMall.m_Idx))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("658"));
			return;
		}
		NrTSingleton<ItemMallItemManager>.Instance.SetTradeItem(this.m_ItemMall, ItemMallItemManager.eItemMall_SellType.FIVEROCKSEVENT);
		GS_ITEMMALL_CHECK_CAN_TRADE_REQ gS_ITEMMALL_CHECK_CAN_TRADE_REQ = new GS_ITEMMALL_CHECK_CAN_TRADE_REQ();
		gS_ITEMMALL_CHECK_CAN_TRADE_REQ.MallIndex = this.m_ItemMall.m_Idx;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMMALL_CHECK_CAN_TRADE_REQ, gS_ITEMMALL_CHECK_CAN_TRADE_REQ);
		if (this.m_cbCheckBox.IsChecked())
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			ITEMMALL_POPUPSHOP poPupShop_AfterItemBuyLimit = NrTSingleton<ItemMallPoPupShopManager>.Instance.GetPoPupShop_AfterItemBuyLimit(this.m_ItemMall.m_Idx);
			if (poPupShop_AfterItemBuyLimit != null)
			{
				GS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ gS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ = new GS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ();
				gS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ.i64PersonID = myCharInfo.m_PersonID;
				gS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ.i32Idx = poPupShop_AfterItemBuyLimit.m_Idx;
				gS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ.bCheckBox = true;
				SendPacket.GetInstance().SendObject(2538, gS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ);
			}
		}
		this.m_ItemMall = null;
		this.bcheck = true;
		this.Close();
	}

	public void OnClickClose(object obj)
	{
		if (this.m_cbCheckBox.IsChecked())
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			ITEMMALL_POPUPSHOP poPupShop_AfterItemBuyLimit = NrTSingleton<ItemMallPoPupShopManager>.Instance.GetPoPupShop_AfterItemBuyLimit((long)this.m_itemPopUpData.m_Idx);
			if (poPupShop_AfterItemBuyLimit != null)
			{
				GS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ gS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ = new GS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ();
				gS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ.i64PersonID = myCharInfo.m_PersonID;
				gS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ.i32Idx = poPupShop_AfterItemBuyLimit.m_Idx;
				gS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ.bCheckBox = true;
				SendPacket.GetInstance().SendObject(2538, gS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_REQ);
			}
		}
		this.bcheck = true;
		this.Close();
	}

	public override void Close()
	{
		if (this.bcheck)
		{
			base.Close();
		}
		else
		{
			this.m_bCancel.CallChangeDelegate();
		}
	}

	public override void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
		}
	}

	public override void CloseForm(IUIObject obj)
	{
		base.CloseForm(obj);
	}
}
