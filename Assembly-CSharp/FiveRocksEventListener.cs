using FiveRocksUnity;
using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class FiveRocksEventListener : MonoBehaviour, FiveRocksActionRequestHandler
{
	private const ItemMallItemManager.eItemMall_SellType m_eSellType = ItemMallItemManager.eItemMall_SellType.FIVEROCKSEVENT;

	private ITEM_MALL_ITEM m_ItemMall;

	private void Start()
	{
		FiveRocks.OnPlacementContentNone += new FiveRocks.PlacementContentNoneHandler(this.OnPlacementContentNone);
		FiveRocks.OnPlacementContentReady += new FiveRocks.PlacementContentReadyHandler(this.OnPlacementContentReady);
		FiveRocks.OnPlacementContentShow += new FiveRocks.PlacementContentShowHandler(this.OnPlacementContentShow);
		FiveRocks.OnPlacementContentClose += new FiveRocks.PlacementContentCloseHandler(this.OnPlacementContentClose);
		FiveRocks.OnPlacementContentClick += new FiveRocks.PlacementContentClickHandler(this.OnPlacementContentClick);
		FiveRocks.OnPlacementContentDismiss += new FiveRocks.PlacementContentDismissHandler(this.OnPlacementContentDismiss);
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		Debug.Log("GameControl.OnDestroy()");
		FiveRocks.OnPlacementContentNone -= new FiveRocks.PlacementContentNoneHandler(this.OnPlacementContentNone);
		FiveRocks.OnPlacementContentReady -= new FiveRocks.PlacementContentReadyHandler(this.OnPlacementContentReady);
		FiveRocks.OnPlacementContentShow -= new FiveRocks.PlacementContentShowHandler(this.OnPlacementContentShow);
		FiveRocks.OnPlacementContentClose -= new FiveRocks.PlacementContentCloseHandler(this.OnPlacementContentClose);
		FiveRocks.OnPlacementContentClick -= new FiveRocks.PlacementContentClickHandler(this.OnPlacementContentClick);
		FiveRocks.OnPlacementContentDismiss -= new FiveRocks.PlacementContentDismissHandler(this.OnPlacementContentDismiss);
	}

	private void OnPlacementContentNone(string placement)
	{
		this.ShowMessage("FiveRocks.OnPlacementContentNone({0})", new object[]
		{
			placement
		});
	}

	private void OnPlacementContentReady(string placement)
	{
		this.ShowMessage("FiveRocks.OnPlacementContentReady({0})", new object[]
		{
			placement
		});
		FiveRocks.ShowPlacementContent(placement);
	}

	private void OnPlacementContentShow(string placement)
	{
		this.ShowMessage("FiveRocks.OnPlacementContentShow({0})", new object[]
		{
			placement
		});
	}

	private void OnPlacementContentClose(string placement)
	{
	}

	private void OnPlacementContentClick(string placement, FiveRocksActionRequest actionRequest)
	{
		if (actionRequest != null)
		{
			actionRequest.DispatchTo(this);
		}
	}

	private void OnPlacementContentDismiss(string placement, FiveRocksActionRequest actionRequest)
	{
		if (actionRequest != null)
		{
			actionRequest.DispatchTo(this);
		}
	}

	public void OnPurchaseRequest(string campaignId, string productId)
	{
		this.ShowMessage("FiveRocks.OnPurchaseRequest(campaignId={0}, productId={1})", new object[]
		{
			campaignId,
			productId
		});
		long index = 0L;
		string empty = string.Empty;
		if (long.TryParse(productId, out index))
		{
			ITEM_MALL_ITEM item = NrTSingleton<ItemMallItemManager>.Instance.GetItem(index);
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
			}
		}
		else
		{
			NrTSingleton<FiveRocksEventData>.Instance.CheckEventData(productId);
		}
	}

	public void MsgBoxOKEvent(object EventObject)
	{
		int posType = 6;
		switch (NrTSingleton<ItemManager>.Instance.GetItemTypeByItemUnique((int)this.m_ItemMall.m_nItemUnique))
		{
		case eITEM_TYPE.ITEMTYPE_BOX:
		case eITEM_TYPE.ITEMTYPE_HEAL:
		case eITEM_TYPE.ITEMTYPE_SUPPLY:
			posType = 5;
			break;
		case eITEM_TYPE.ITEMTYPE_QUEST:
		case eITEM_TYPE.ITEMTYPE_MATERIAL:
			posType = 6;
			break;
		case eITEM_TYPE.ITEMTYPE_TICKET:
			posType = 7;
			break;
		}
		if (NkUserInventory.GetInstance().GetEmptySlot(posType) == -1)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
			return;
		}
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsShopProduct(this.m_ItemMall.m_Idx) || !BaseNet_Game.GetInstance().IsSocketConnected() || !NrTSingleton<ItemMallItemManager>.Instance.BuyItem(this.m_ItemMall.m_Idx))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("658"));
			return;
		}
		NrTSingleton<ItemMallItemManager>.Instance.SetTradeItem(this.m_ItemMall, ItemMallItemManager.eItemMall_SellType.FIVEROCKSEVENT);
		GS_ITEMMALL_CHECK_CAN_TRADE_REQ gS_ITEMMALL_CHECK_CAN_TRADE_REQ = new GS_ITEMMALL_CHECK_CAN_TRADE_REQ();
		gS_ITEMMALL_CHECK_CAN_TRADE_REQ.MallIndex = this.m_ItemMall.m_Idx;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMMALL_CHECK_CAN_TRADE_REQ, gS_ITEMMALL_CHECK_CAN_TRADE_REQ);
		this.m_ItemMall = null;
	}

	public void OnRewardRequest(string id, string name, int quantity, string token)
	{
		this.ShowMessage("FiveRocks.OnRewardRequest(id={0}, name={1}, quantity={2}, token={3})", new object[]
		{
			id,
			name,
			quantity,
			token
		});
	}

	private void ShowMessage(string format, params object[] args)
	{
		string message = string.Format(format, args);
		Debug.Log(message);
	}
}
