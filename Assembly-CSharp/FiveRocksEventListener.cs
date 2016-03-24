using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TapjoyUnity;
using UnityEngine;
using UnityForms;

public class FiveRocksEventListener : MonoBehaviour
{
	private const ItemMallItemManager.eItemMall_SellType m_eSellType = ItemMallItemManager.eItemMall_SellType.FIVEROCKSEVENT;

	private ITEM_MALL_ITEM m_ItemMall;

	private Dictionary<string, TJPlacement> _placementDic;

	private void Awake()
	{
		this._placementDic = new Dictionary<string, TJPlacement>();
	}

	private void Start()
	{
		TJPlacement.OnRequestSuccess += new TJPlacement.OnRequestSuccessHandler(this.HandlePlacementRequestSuccess);
		TJPlacement.OnRequestFailure += new TJPlacement.OnRequestFailureHandler(this.HandlePlacementRequestFailure);
		TJPlacement.OnContentReady += new TJPlacement.OnContentReadyHandler(this.HandlePlacementContentReady);
		TJPlacement.OnContentShow += new TJPlacement.OnContentShowHandler(this.HandlePlacementContentShow);
		TJPlacement.OnContentDismiss += new TJPlacement.OnContentDismissHandler(this.HandlePlacementContentDismiss);
		TJPlacement.OnPurchaseRequest += new TJPlacement.OnPurchaseRequestHandler(this.HandleOnPurchaseRequest);
		TJPlacement.OnRewardRequest += new TJPlacement.OnRewardRequestHandler(this.HandleOnRewardRequest);
		Tapjoy.OnConnectSuccess += new Tapjoy.OnConnectSuccessHandler(this.HandleOnConnectSuccess);
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		Debug.Log("GameControl.OnDestroy()");
		TJPlacement.OnRequestSuccess -= new TJPlacement.OnRequestSuccessHandler(this.HandlePlacementRequestSuccess);
		TJPlacement.OnRequestFailure -= new TJPlacement.OnRequestFailureHandler(this.HandlePlacementRequestFailure);
		TJPlacement.OnContentReady -= new TJPlacement.OnContentReadyHandler(this.HandlePlacementContentReady);
		TJPlacement.OnContentShow -= new TJPlacement.OnContentShowHandler(this.HandlePlacementContentShow);
		TJPlacement.OnContentDismiss -= new TJPlacement.OnContentDismissHandler(this.HandlePlacementContentDismiss);
		TJPlacement.OnPurchaseRequest -= new TJPlacement.OnPurchaseRequestHandler(this.HandleOnPurchaseRequest);
		TJPlacement.OnRewardRequest -= new TJPlacement.OnRewardRequestHandler(this.HandleOnRewardRequest);
		Tapjoy.OnConnectSuccess -= new Tapjoy.OnConnectSuccessHandler(this.HandleOnConnectSuccess);
		if (this._placementDic != null)
		{
			this._placementDic.Clear();
		}
	}

	public void RequestPlacementContent(string placementName)
	{
		if (this._placementDic == null)
		{
			Debug.LogError("ERROR, FiveRocksEventListener.cs, RequestPlacementContent(), _placementDic is null");
			return;
		}
		if (!this._placementDic.ContainsKey(placementName))
		{
			TJPlacement value = TJPlacement.CreatePlacement(placementName);
			this._placementDic.Add(placementName, value);
		}
		this._placementDic[placementName].RequestContent();
	}

	public void HandleOnConnectSuccess()
	{
		TJPlacement tJPlacement = TJPlacement.CreatePlacement("start");
		tJPlacement.RequestContent();
	}

	public void HandlePlacementRequestSuccess(TJPlacement placement)
	{
		this.ShowMessage("Content for " + placement.GetName(), new object[0]);
	}

	public void HandlePlacementRequestFailure(TJPlacement placement, string error)
	{
		this.ShowMessage("C#: HandlePlacementRequestFailure", new object[0]);
		this.ShowMessage("C#: Request for " + placement.GetName() + " has failed because: " + error, new object[0]);
	}

	public void HandlePlacementContentReady(TJPlacement placement)
	{
		this.ShowMessage("Tapjoy.OnContentReadyHandler({0})", new object[]
		{
			placement
		});
		if (!placement.IsContentAvailable())
		{
			this.RemovePlacement(placement);
			return;
		}
		placement.ShowContent();
	}

	public void HandlePlacementContentShow(TJPlacement placement)
	{
		this.ShowMessage("C#: HandlePlacementContentShow", new object[0]);
	}

	public void HandlePlacementContentDismiss(TJPlacement placement)
	{
		this.ShowMessage("C#: HandlePlacementContentDismiss", new object[0]);
	}

	private void HandleOnPurchaseRequest(TJPlacement placement, TJActionRequest request, string productId)
	{
		this.ShowMessage("C#: HandleOnPurchaseRequest", new object[0]);
		request.Completed();
		this.OnPurchaseRequest(string.Empty, productId);
	}

	private void HandleOnRewardRequest(TJPlacement placement, TJActionRequest request, string itemId, int quantity)
	{
		this.ShowMessage("C#: HandleOnRewardRequest", new object[0]);
		request.Completed();
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

	private void RemovePlacement(TJPlacement placement)
	{
		if (!this._placementDic.ContainsKey(placement.GetName()))
		{
			return;
		}
		this._placementDic.Remove(placement.GetName());
	}
}
