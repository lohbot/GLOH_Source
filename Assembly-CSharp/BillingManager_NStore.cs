using Prime31;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using SERVICE;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BillingManager_NStore : BillingManager
{
	private string strProductID = string.Empty;

	private void OnEnable()
	{
		string appCode = "RNIL325121397121506325";
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORNAVER)
		{
			appCode = "ONDA528291417429290852";
		}
		string iapKey = "hjityjdO3m";
		NIAPUnityPlugin.instance.initialize(appCode, iapKey);
		NIAPUnityPlugin.instance.onPaymentCompletedEvent += new Action<NIAPResult>(this.onPaymentCompleted);
		NIAPUnityPlugin.instance.onReceivedProductInfosEvent += new Action<NIAPResult>(this.onReceivedProductInfos);
		NIAPUnityPlugin.instance.onReceivedPaymentSeqEvent += new Action<NIAPResult>(this.onReceivedPaymentSeq);
		NIAPUnityPlugin.instance.onReceivedReceiptEvent += new Action<NIAPResult>(this.onReceivedReceipt);
		NIAPUnityPlugin.instance.onPaymentCanceledEvent += new Action<NIAPResult>(this.onPaymentCanceled);
		NIAPUnityPlugin.instance.onErrorEvent += new Action<NIAPResult>(this.onError);
		this.billingSupportedEvent();
	}

	private void OnDisable()
	{
		NIAPUnityPlugin.instance.onPaymentCompletedEvent -= new Action<NIAPResult>(this.onPaymentCompleted);
		NIAPUnityPlugin.instance.onReceivedProductInfosEvent -= new Action<NIAPResult>(this.onReceivedProductInfos);
		NIAPUnityPlugin.instance.onReceivedPaymentSeqEvent -= new Action<NIAPResult>(this.onReceivedPaymentSeq);
		NIAPUnityPlugin.instance.onReceivedReceiptEvent -= new Action<NIAPResult>(this.onReceivedReceipt);
		NIAPUnityPlugin.instance.onPaymentCanceledEvent -= new Action<NIAPResult>(this.onPaymentCanceled);
		NIAPUnityPlugin.instance.onErrorEvent -= new Action<NIAPResult>(this.onError);
	}

	private void billingSupportedEvent()
	{
		Debug.Log("billingSupportedEvent");
	}

	public override void PurchaseItem(string strItem, int price)
	{
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = true;
		this.strProductID = strItem;
		string extra = "extra field";
		NIAPUnityPlugin.instance.requestPayment(this.strProductID, price, extra);
	}

	public void onPaymentCompleted(NIAPResult result)
	{
		Debug.Log("onPaymentCompleted " + result.getResult());
		string empty = string.Empty;
		this.NStorePurchase(result.getResult().dictionaryFromJson(), ref this.strProductID);
		this.NStorePurchase(result.getExtraValue().dictionaryFromJson(), ref empty);
		if (TsPlatform.IsBand)
		{
			this.Receipt.Append(string.Format("MarketType={0}&Signature={1}&SignedData={2}", "bandnaver", empty, result.getResult()));
		}
		else
		{
			this.Receipt.Append(string.Format("MarketType={0}&Signature={1}&SignedData={2}", "naver", empty, result.getResult()));
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			GS_BILLINGCHECK_REQ gS_BILLINGCHECK_REQ = new GS_BILLINGCHECK_REQ();
			gS_BILLINGCHECK_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(this.strProductID);
			TKString.StringChar(this.Receipt.ToString(), ref gS_BILLINGCHECK_REQ.Receipt);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLINGCHECK_REQ, gS_BILLINGCHECK_REQ);
			PlayerPrefs.SetString(NrPrefsKey.SHOP_PRODUCT_ID, gS_BILLINGCHECK_REQ.i64ItemMallIndex.ToString());
			PlayerPrefs.SetString(NrPrefsKey.SHOP_RECEIPT, this.Receipt.ToString());
			if (TsPlatform.IsBand)
			{
				PlayerPrefs.SetString(NrPrefsKey.BAND_RECEIPT, result.getResult());
			}
			TsPlatform.FileLog(string.Format("TStorePurchaseComplete\nReceipt ={0} ProductID = {1} ItemMallIdex = {2}", this.Receipt, this.strProductID, gS_BILLINGCHECK_REQ.i64ItemMallIndex));
			TsLog.LogWarning("Send BillingCheck ProductID = {0} ItemMallIdex = {1}", new object[]
			{
				this.strProductID,
				gS_BILLINGCHECK_REQ.i64ItemMallIndex
			});
			this.Receipt.Remove(0, this.Receipt.Length);
		}
		GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
		gS_BILLING_ITEM_RECODE_REQ.i8Type = 1;
		gS_BILLING_ITEM_RECODE_REQ.i8Result = 0;
		gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(this.strProductID);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = false;
	}

	public void onReceivedProductInfos(NIAPResult result)
	{
	}

	public void onReceivedPaymentSeq(NIAPResult result)
	{
	}

	public void onReceivedReceipt(NIAPResult result)
	{
	}

	public void onPaymentCanceled(NIAPResult result)
	{
		NIAPUnityPlugin.instance.showResultMessage(result);
		GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
		gS_BILLING_ITEM_RECODE_REQ.i8Type = 1;
		gS_BILLING_ITEM_RECODE_REQ.i8Result = 1;
		gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(this.strProductID);
		NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ, string.Format("RequestType:{0}, ResultType:{1}, Result:{2}, ExtraValue:{3}", new object[]
		{
			result.getRequestType(),
			result.getResultType(),
			result.getResult(),
			result.getExtraValue()
		}));
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = false;
	}

	public void onError(NIAPResult result)
	{
		NIAPUnityPlugin.instance.showErrorMessage(result);
		GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
		gS_BILLING_ITEM_RECODE_REQ.i8Type = 1;
		gS_BILLING_ITEM_RECODE_REQ.i8Result = 1;
		gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(this.strProductID);
		NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ, string.Format("RequestType:{0}, ResultType:{1}, Result:{2}, ExtraValue:{3}", new object[]
		{
			result.getRequestType(),
			result.getResultType(),
			result.getResult(),
			result.getExtraValue()
		}));
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = false;
	}

	public void NStorePurchase(Dictionary<string, object> dict, ref string recvData)
	{
		if (dict.ContainsKey("productCode"))
		{
			recvData = dict["productCode"].ToString();
		}
		if (dict.ContainsKey("receipt"))
		{
			Dictionary<string, object> dict2 = dict["receipt"] as Dictionary<string, object>;
			this.NStorePurchase(dict2, ref recvData);
		}
		if (dict.ContainsKey("signature"))
		{
			recvData = dict["signature"].ToString();
		}
	}
}
