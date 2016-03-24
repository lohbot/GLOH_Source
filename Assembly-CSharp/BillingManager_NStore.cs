using Prime31;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class BillingManager_NStore : BillingManager
{
	private string strProductID = string.Empty;

	private string publicKey = string.Empty;

	private NIAPPurchase m_pResult;

	private NIAPPurchase m_RecoveryItem;

	public Dictionary<string, NIAPPurchase> m_PurchaseList = new Dictionary<string, NIAPPurchase>();

	public List<NIAPPurchase> m_ConsumeFailedList = new List<NIAPPurchase>();

	public Stack<NIAPPurchase> m_PushPurchaseList = new Stack<NIAPPurchase>();

	private bool m_bRecovery;

	private static BillingManager_NStore s_instance;

	public static BillingManager_NStore Instance
	{
		get
		{
			if (BillingManager_NStore.s_instance == null)
			{
				GameObject gameObject = GameObject.Find("BillingManager_NStore");
				if (gameObject == null)
				{
					gameObject = new GameObject("BillingManager_NStore");
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				BillingManager_NStore.s_instance = gameObject.GetComponent<BillingManager_NStore>();
				if (BillingManager_NStore.s_instance == null)
				{
					BillingManager_NStore.s_instance = gameObject.AddComponent<BillingManager_NStore>();
				}
			}
			return BillingManager_NStore.s_instance;
		}
	}

	private void OnEnable()
	{
		this.publicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCcAknakVih7osOCcB5cQN6YZXa9Sc6yn4b5IV3SVvKxlh9sEu4YraAI2pkGeTIAgp59gtEh8a1L2b2ZSW8+VfJ13M4RG4LfQ+Qg86agS2kSsCv9uJTcnKHRdvYr3vErgK5ok65vBLTxNLfQRlH8qkCCzlDcDYmfA/K9ZNnZ8ASUwIDAQAB";
		NIAPNativeExtension.Instance.initialize(this.publicKey);
		this.billingSupportedEvent();
	}

	private void OnDisable()
	{
	}

	private void billingSupportedEvent()
	{
		Debug.Log("billingSupportedEvent");
		this.CheckRestoreItem();
	}

	public override void PurchaseItem(string strItem, int price)
	{
		if (this.m_PurchaseList.ContainsKey(strItem))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("664"));
			return;
		}
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = true;
		this.strProductID = strItem;
		string text = string.Format("1{0:yyMMddHHmmss}_{1}", DateTime.Now, NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID);
		int niapRequestCode = 100;
		PlayerPrefs.SetString(NrPrefsKey.BUY_PRODUCT_ID, strItem);
		PlayerPrefs.SetString(NrPrefsKey.BUY_UNIQUE_CODE, text);
		PlayerPrefs.SetString(NrPrefsKey.BUY_DATE, string.Format("{0:yyyyMMdd}", DateTime.Now));
		NIAPNativeExtension.Instance.requestPayment(this.strProductID, niapRequestCode, text, new Action<NIAPPurchase>(this.onPaymentCompleted), new Action<NIAPResult>(this.onPaymentCanceled), new Action<NIAPResultError>(this.onPaymentError));
	}

	public void Consume()
	{
		NIAPPurchase pItem = null;
		if (this.m_bRecovery)
		{
			pItem = this.m_RecoveryItem;
		}
		else
		{
			pItem = this.m_pResult;
		}
		if (pItem != null)
		{
			if (pItem.getOriginalPurchaseAsJsonText() != null && pItem.getSignature() != null)
			{
				NIAPNativeExtension.Instance.requestConsume(pItem.getOriginalPurchaseAsJsonText(), pItem.getSignature(), delegate(NIAPResult niapResult)
				{
					this.RemoveRcoveryItemData(pItem);
				}, delegate(NIAPResultError error)
				{
					Debug.Log("consumePurchaseFailedEvent: " + error);
					BillingManager_NStore component = BillingManager_NStore.Instance.GetComponent<BillingManager_NStore>();
					if (component != null && component.m_RecoveryItem != null)
					{
						GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
						gS_BILLING_ITEM_RECODE_REQ.i8Type = 2;
						gS_BILLING_ITEM_RECODE_REQ.i8Result = 1;
						gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(pItem.getProductCode());
						NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ, string.Format("requestConsume error code : {0} , error message : {1}", error.getCode(), error.getMessage()));
						SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
					}
					this.RemoveRcoveryItemData(pItem);
					if (!this.m_ConsumeFailedList.Contains(pItem))
					{
						this.m_ConsumeFailedList.Add(pItem);
					}
				});
			}
		}
		else if (pItem == null)
		{
			TsLog.LogError("pItem == NULL", new object[0]);
		}
	}

	public void onPaymentCompleted(NIAPPurchase result)
	{
		string arg = string.Empty;
		this.m_pResult = result;
		this.NStorePurchase(result.getOriginalPurchaseAsJsonText().dictionaryFromJson(), ref this.strProductID);
		arg = result.getSignature();
		if (TsPlatform.IsBand)
		{
			this.Receipt.Append(string.Format("MarketType={0}&Signature={1}&SignedData={2}", "bandnaver", arg, result.getOriginalPurchaseAsJsonText()));
		}
		else
		{
			this.Receipt.Append(string.Format("MarketType={0}&Signature={1}&SignedData={2}", "naver", arg, result.getOriginalPurchaseAsJsonText()));
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
				PlayerPrefs.SetString(NrPrefsKey.BAND_RECEIPT, result.getOriginalPurchaseAsJsonText());
			}
			TsPlatform.FileLog(string.Format("TStorePurchaseComplete\nReceipt ={0} ProductID = {1} ItemMallIdex = {2}", this.Receipt, this.strProductID, gS_BILLINGCHECK_REQ.i64ItemMallIndex));
			this.Receipt.Remove(0, this.Receipt.Length);
		}
		GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
		gS_BILLING_ITEM_RECODE_REQ.i8Type = 1;
		gS_BILLING_ITEM_RECODE_REQ.i8Result = 0;
		gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(this.strProductID);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = false;
	}

	public void CheckRestoreItem()
	{
		this.m_PurchaseList.Clear();
		this.m_PushPurchaseList.Clear();
		NIAPNativeExtension.Instance.getPurchases(delegate(NIAPResult result)
		{
			string json = string.Empty;
			json = result.getOrignalString();
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			if (dictionary != null && dictionary.ContainsKey("result"))
			{
				List<object> list = dictionary["result"] as List<object>;
				List<object> list2 = dictionary["signature"] as List<object>;
				for (int i = 0; i < list.Count; i++)
				{
					string json2 = (string)list[i];
					Dictionary<string, object> dictionary2 = json2.dictionaryFromJson();
					NIAPPurchase nIAPPurchase = new NIAPPurchase(dictionary2["paymentSeq"].ToString(), dictionary2["purchaseToken"].ToString(), dictionary2["purchaseType"].ToString(), dictionary2["environment"].ToString(), dictionary2["packageName"].ToString(), dictionary2["appName"].ToString(), dictionary2["productCode"].ToString(), dictionary2["paymentTime"].ToString(), dictionary2["developerPayload"].ToString(), dictionary2["nonce"].ToString(), (string)list2[i], (string)list[i]);
					if (nIAPPurchase != null && !string.IsNullOrEmpty(nIAPPurchase.getDeveloperPayload()) && !nIAPPurchase.getDeveloperPayload().Contains("extra"))
					{
						this.m_PurchaseList.Add(dictionary2["productCode"].ToString(), nIAPPurchase);
					}
				}
				foreach (NIAPPurchase current in this.m_PurchaseList.Values)
				{
					if (!this.m_ConsumeFailedList.Contains(current))
					{
						this.m_PushPurchaseList.Push(current);
					}
				}
				if (this.m_PurchaseList.Count > 0)
				{
					NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.PURCHASE_RESTORE);
				}
			}
		}, delegate(NIAPResultError error)
		{
			TsLog.LogError("getPurchases error code : {0} , error message :{1} ", new object[]
			{
				error.getCode(),
				error.getMessage()
			});
		});
	}

	public bool IsRecoveryItem()
	{
		return this.m_bRecovery;
	}

	public void StartRecoveryItem()
	{
		this.m_bRecovery = true;
	}

	public void RecoveryItem()
	{
		this.m_RecoveryItem = this.m_PushPurchaseList.Pop();
		if (this.m_RecoveryItem != null)
		{
			this.Receipt.Remove(0, this.Receipt.Length);
			string arg = string.Empty;
			this.NStorePurchase(this.m_RecoveryItem.getOriginalPurchaseAsJsonText().dictionaryFromJson(), ref this.strProductID);
			arg = this.m_RecoveryItem.getSignature();
			if (TsPlatform.IsBand)
			{
				this.Receipt.Append(string.Format("MarketType={0}&Signature={1}&SignedData={2}", "bandnaver", arg, this.m_RecoveryItem.getOriginalPurchaseAsJsonText()));
			}
			else
			{
				this.Receipt.Append(string.Format("MarketType={0}&Signature={1}&SignedData={2}", "naver", arg, this.m_RecoveryItem.getOriginalPurchaseAsJsonText()));
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
					PlayerPrefs.SetString(NrPrefsKey.BAND_RECEIPT, this.m_RecoveryItem.getOriginalPurchaseAsJsonText());
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
		else
		{
			this.m_bRecovery = false;
		}
	}

	public void RemoveRcoveryItemData(NIAPPurchase ConsumeItem)
	{
		if (this.m_RecoveryItem == null)
		{
			return;
		}
		this.m_bRecovery = false;
		if (this.m_RecoveryItem != null)
		{
			this.m_RecoveryItem = null;
		}
		this.m_PurchaseList.Remove(ConsumeItem.getProductCode());
		if (NrTSingleton<GameGuideManager>.Instance.ExecuteGuide)
		{
			GameGuideDlg gameGuideDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GAMEGUIDE_DLG) as GameGuideDlg;
			if (gameGuideDlg != null)
			{
				if (NrTSingleton<GameGuideManager>.Instance.ContinueCheck(GameGuideType.PURCHASE_RESTORE))
				{
					gameGuideDlg.SetTalkText();
				}
				else
				{
					gameGuideDlg.ClickClose(null);
				}
			}
		}
		else
		{
			NrTSingleton<GameGuideManager>.Instance.RemoveGuide(GameGuideType.PURCHASE_RESTORE);
		}
	}

	public void onPaymentCanceled(NIAPResult result)
	{
		GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
		gS_BILLING_ITEM_RECODE_REQ.i8Type = 1;
		gS_BILLING_ITEM_RECODE_REQ.i8Result = 1;
		gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(this.strProductID);
		NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ, string.Format("requestPayment canceled", new object[0]));
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = false;
	}

	public void onPaymentError(NIAPResultError result)
	{
		GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
		gS_BILLING_ITEM_RECODE_REQ.i8Type = 1;
		gS_BILLING_ITEM_RECODE_REQ.i8Result = 2;
		gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(this.strProductID);
		NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ, string.Format("getCode:{0}, getMessage:{1}", result.getCode(), result.getMessage()));
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

	private void Update()
	{
		if (this.m_bRecovery && this.m_RecoveryItem == null && this.m_PushPurchaseList.Count > 0)
		{
			this.RecoveryItem();
		}
	}
}
