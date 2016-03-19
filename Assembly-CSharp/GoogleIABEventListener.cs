using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GoogleIABEventListener : MonoBehaviour
{
	private StringBuilder Receipt = new StringBuilder();

	private void OnEnable()
	{
		GoogleIABManager.billingSupportedEvent += new Action(this.billingSupportedEvent);
		GoogleIABManager.billingNotSupportedEvent += new Action<string>(this.billingNotSupportedEvent);
		GoogleIABManager.queryInventorySucceededEvent += new Action<List<GooglePurchase>, List<GoogleSkuInfo>>(this.queryInventorySucceededEvent);
		GoogleIABManager.queryInventoryFailedEvent += new Action<string>(this.queryInventoryFailedEvent);
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += new Action<string, string>(this.purchaseCompleteAwaitingVerificationEvent);
		GoogleIABManager.purchaseSucceededEvent += new Action<GooglePurchase>(this.purchaseSucceededEvent);
		GoogleIABManager.purchaseFailedEvent += new Action<string>(this.purchaseFailedEvent);
		GoogleIABManager.consumePurchaseSucceededEvent += new Action<GooglePurchase>(this.consumePurchaseSucceededEvent);
		GoogleIABManager.consumePurchaseFailedEvent += new Action<string>(this.consumePurchaseFailedEvent);
	}

	private void OnDisable()
	{
		GoogleIABManager.billingSupportedEvent -= new Action(this.billingSupportedEvent);
		GoogleIABManager.billingNotSupportedEvent -= new Action<string>(this.billingNotSupportedEvent);
		GoogleIABManager.queryInventorySucceededEvent -= new Action<List<GooglePurchase>, List<GoogleSkuInfo>>(this.queryInventorySucceededEvent);
		GoogleIABManager.queryInventoryFailedEvent -= new Action<string>(this.queryInventoryFailedEvent);
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += new Action<string, string>(this.purchaseCompleteAwaitingVerificationEvent);
		GoogleIABManager.purchaseSucceededEvent -= new Action<GooglePurchase>(this.purchaseSucceededEvent);
		GoogleIABManager.purchaseFailedEvent -= new Action<string>(this.purchaseFailedEvent);
		GoogleIABManager.consumePurchaseSucceededEvent -= new Action<GooglePurchase>(this.consumePurchaseSucceededEvent);
		GoogleIABManager.consumePurchaseFailedEvent -= new Action<string>(this.consumePurchaseFailedEvent);
	}

	private void billingSupportedEvent()
	{
		string[] items = NrTSingleton<ItemMallItemManager>.Instance.GetItems();
		GoogleIAB.queryInventory(items);
		Debug.Log("billingSupportedEvent skus :" + items.Length);
	}

	private void billingNotSupportedEvent(string error)
	{
		Debug.Log("billingNotSupportedEvent: " + error);
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		if (purchases.Count > 0)
		{
			GameObject gameObject = GameObject.Find("BillingManager_Google");
			if (gameObject != null)
			{
				BillingManager_Google component = gameObject.GetComponent<BillingManager_Google>();
				if (component != null)
				{
					component.AddPurchase(purchases);
				}
			}
		}
	}

	private void queryInventoryFailedEvent(string error)
	{
		Debug.Log("queryInventoryFailedEvent: " + error);
	}

	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("purchaseSucceededEvent: " + purchase);
		string arg = string.Concat(new object[]
		{
			"{\"orderId\":\"",
			purchase.orderId,
			"\",\"packageName\":\"",
			purchase.packageName,
			"\",\"productId\":\"",
			purchase.productId,
			"\",\"purchaseTime\":",
			purchase.purchaseTime,
			",\"purchaseState\":",
			(int)purchase.purchaseState,
			",\"developerPayload\":\"",
			purchase.developerPayload,
			"\",\"purchaseToken\":\"",
			purchase.purchaseToken,
			"\"}"
		});
		this.Receipt.Append(string.Format("MarketType={0}&Signature={1}&SignedData={2}", "android", purchase.signature, arg));
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			GS_BILLINGCHECK_REQ gS_BILLINGCHECK_REQ = new GS_BILLINGCHECK_REQ();
			gS_BILLINGCHECK_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(purchase.productId);
			gS_BILLINGCHECK_REQ.SN = myCharInfo.m_SN;
			TKString.StringChar(this.Receipt.ToString(), ref gS_BILLINGCHECK_REQ.Receipt);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLINGCHECK_REQ, gS_BILLINGCHECK_REQ);
			TsPlatform.FileLog(string.Format("GooglePurchaseComplete\nReceipt ={0} ProductID = {1} ItemMallIdex = {2}", this.Receipt, purchase.productId, gS_BILLINGCHECK_REQ.i64ItemMallIndex));
			GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
			gS_BILLING_ITEM_RECODE_REQ.i8Type = 1;
			gS_BILLING_ITEM_RECODE_REQ.i8Result = 0;
			gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = gS_BILLINGCHECK_REQ.i64ItemMallIndex;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
			PlayerPrefs.SetString(NrPrefsKey.SHOP_PRODUCT_ID, gS_BILLINGCHECK_REQ.i64ItemMallIndex.ToString());
			PlayerPrefs.SetString(NrPrefsKey.SHOP_RECEIPT, this.Receipt.ToString());
			if (TsPlatform.IsBand)
			{
				PlayerPrefs.SetString(NrPrefsKey.BAND_RECEIPT, purchase.orderId);
			}
			this.Receipt.Remove(0, this.Receipt.Length);
		}
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = false;
	}

	private void purchaseFailedEvent(string error)
	{
		Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("211"));
		GameObject gameObject = GameObject.Find("BillingManager_Google");
		if (gameObject != null)
		{
			BillingManager_Google component = gameObject.GetComponent<BillingManager_Google>();
			if (component != null)
			{
				GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
				gS_BILLING_ITEM_RECODE_REQ.i8Type = 1;
				gS_BILLING_ITEM_RECODE_REQ.i8Result = 1;
				gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(component.BuyItem);
				NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ, error);
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
				component.CheckRestoreItem();
			}
		}
		Debug.Log("purchaseFailedEvent: " + error);
		this.Receipt.Remove(0, this.Receipt.Length);
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = false;
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
		this.Receipt.Remove(0, this.Receipt.Length);
		BillingManager_Google.Instance.RemoveRcoveryItemData(purchase);
	}

	private void consumePurchaseFailedEvent(string error)
	{
		Debug.Log("consumePurchaseFailedEvent: " + error);
		BillingManager_Google component = BillingManager_Google.Instance.GetComponent<BillingManager_Google>();
		if (component != null && component.m_RecoveryItem != null)
		{
			GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
			gS_BILLING_ITEM_RECODE_REQ.i8Type = 2;
			gS_BILLING_ITEM_RECODE_REQ.i8Result = 1;
			gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(component.m_RecoveryItem.productId);
			NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ, error);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
			BillingManager_Google.Instance.ConsumeFailedItemData(component.m_RecoveryItem);
		}
	}
}
