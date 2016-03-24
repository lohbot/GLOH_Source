using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using SERVICE;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class BillingManager_Google : BillingManager
{
	private string _key = string.Empty;

	public string BuyItem = string.Empty;

	public Dictionary<string, GooglePurchase> m_PurchaseList = new Dictionary<string, GooglePurchase>();

	public List<GooglePurchase> m_ConsumeFailedList = new List<GooglePurchase>();

	public Stack<GooglePurchase> m_PushPurchaseList = new Stack<GooglePurchase>();

	public GooglePurchase m_RecoveryItem;

	private bool m_bRecovery;

	private bool m_bSendQueryInventory;

	private static BillingManager_Google s_instance;

	public bool SendQueryInventory
	{
		get
		{
			return this.m_bSendQueryInventory;
		}
		set
		{
			this.m_bSendQueryInventory = value;
		}
	}

	public static BillingManager_Google Instance
	{
		get
		{
			if (BillingManager_Google.s_instance == null)
			{
				GameObject gameObject = GameObject.Find("BillingManager_Google");
				if (gameObject == null)
				{
					gameObject = new GameObject("BillingManager_Google");
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				BillingManager_Google.s_instance = gameObject.GetComponent<BillingManager_Google>();
				if (BillingManager_Google.s_instance == null)
				{
					BillingManager_Google.s_instance = gameObject.AddComponent<BillingManager_Google>();
				}
			}
			return BillingManager_Google.s_instance;
		}
	}

	public BillingManager_Google()
	{
		this.SetIABListener();
	}

	public void SetIABListener()
	{
		GameObject gameObject = GameObject.Find("GoogleIABEventListener");
		if (gameObject == null)
		{
			gameObject = new GameObject("GoogleIABEventListener");
			gameObject.AddComponent<GoogleIABEventListener>();
		}
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		if (TsPlatform.IsBand)
		{
			this._key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlcgaV+vJy5E2k/rPt5bqyL/c+0w+zomsHSYehTqDJKjLAmJb4Xa0TDTjGWs5s13LiH8P92pM8CgQ4H66evBarLAOnsGay218MsgxNFcoMQk3zr0zH0rYuR74/gVK0c2XEuipaqVRCtgFZxbjjmua80p49x0QEErHVOAbhOgNJFhTdS/JzWZY6hphkkfrJP5okwp9gV9dQOMxQFKJuGENx0Of7PxZ9aamZBipAWCX6bv2mAKUTZNLFR9uuiSnKb155hEKvfdzU7gJhFcUCEVIIVEpzM8NEDpGgiuxZVPS2vtBc2wrcDxbsCdGe/1jSyH/Gv6u1gIGZFSVMvMUyEfTDwIDAQAB";
		}
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORGOOGLE)
		{
			this._key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiXp63SWTrJxRCsSR40TTSpo59MmdM4PKoEWCzfX9u4ZQmTGkZdZTYiIJ6Ti6T8JRW0x5J4aSDFLhnRCXZ/FpVABBiAJrMdaw5wtL7OLlY+aWplYBcfSvN/oodlaEl/ZMO1qfUWdmB3yn16ufKDUtQDKW56wDlsv5rT9vLJb1M6ySAXwY8WRu7LsatpWdciqU1NynwyIAtwwjqWTw9Fkh0tRHRjw+4cJhlzZRaoPKXeV255Qk83+hZ5MKiZveQZJy/SEYIa/CN3wCjmqJcMtgkbahv8lRc2XdCpiuevW++Rsrv6g9SNB4qUbLnh6+yO6DZsmbj4k60NaLjbusAj78mQIDAQAB";
		}
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORQA)
		{
			this._key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjBAxIJFyvjdhpQm3BPkwR9qbmPSZU6vXnffk/CC42kaIdsJCAiyy+NoZOnqycKOsdVcCsoXI0gRlxmRo1OnZy3l7X6Z11dl9bNZeeZSzs6ra4pf/SGKxA/RqL2PyfO12O44hriPBuW2SzGGPWp9MoQgOREw8Tl0blRFjj6Qo6TGL+fkLsON67wZbOQInrP61ucOmumGdQ+14/agSrmPmRtLfNPelUzxEBGiM15vjjJtBd4vmD+1lqrLZBLlamT3adnpkqyu/qCeF5+i8qPW5yLLoOkpkvz6Mgbew9/NzlEWL/iPau38ylHb1YlToKEonzQYB8wURBviepFUBapYBiwIDAQAB";
		}
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORKAKAO)
		{
			this._key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnGI8L7RnBfN/dKGdq4d2LfazJTMezVFksm+jJaEQBdSkz+WznSv8tROvRYO48LlZr891SjiKr4ucLu4CXmYAoMJ3ZrDSPz4p6uG3idjG9KBPLnQeC5CiKcxiZMAs3VkrhLMbds4My8+n8Fuw+caXGW9AwqhIX2jzs4OXHApOW7JK28erhp2AXKshD7z99vB82gz095Pa8X+eIGBhsLnYPP+eidCs+CFPtt9i1upuS3txG6VzvJgTSh3dFKsoFylaW/UIzLZJW7RY907QYNhgonmL/WwOesGUfjQdcjwurvmo0o7wPFvjkD3zoyZ+/UbMiYUE9io9r4rNKVbEkqK8GwIDAQAB";
		}
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE)
		{
			this._key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArcHJBFJXvvTCJ3Qaxqmj8r7btZHucb0UYyHxTv4sf7PAmNmKjwh03pSUz7vKZwxCIQdHcEcIzMuWbbhTFOyR8G3CqDsVZVmHzG1/F0m9iAvZ3mNcyuSzBf04gG6WpTjmJDEv8OEotBaAr9KH3akx20lmq4SxlknvWp7y36Xzv7IYfoX8lgdYn2hP2/qKCf0YwpMqngC7OCjA7Qol/3r9wP5JLSVQXM0ui2FiQJCAKYWmk2meL0FNBYLyk0v+FhnSxquG6JBeAmSSUwRkpR3EYqjh+Yq8rbKYfl8gQjX3uIyG5r1brtlC83d5Ycpvmj2v5CWqgslcrJcDSi/Z15O06QIDAQAB";
		}
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USQA)
		{
			this._key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnJ6vmwk1CUm5TsgY9UiWTHshNqz8hsu1swCrmHMNOb5WpKX/DLCy0g5imZSfKxe/3uMzwsuibA4EUNHmiHQBgaYhTPNxFd5o14IVSi4/KnMJyrP6qn7OfKdGV2hy/i8lfXUSrfgWUC+APkLvRxrQy2zzvPjfWoWiA+DsZ6i8KfIyyEIXJhEdi1RkH7rLwF3ZTJGWWQ4Ijdco5aQEbbRKP6SrX0weCkagQIRFmgKK4iMsVjwLvZbn7e8rlQTmr+nyfvjiMggS9OgL0/B5+NKpibL2LxN5/95PNqlhw6plDdGpIpuFdtXA7y4XavTzD6X/HQdtyvANcnJJReq/B1xAcQIDAQAB";
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		GoogleIAB.init(this._key);
	}

	public override void PurchaseItem(string strItem, int price)
	{
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = true;
		if (this.m_PurchaseList.ContainsKey(strItem))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("664"));
			return;
		}
		GoogleIAB.purchaseProduct(strItem, string.Format("1{0:yyMMddHHmmss}_{1}", DateTime.Now, NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID));
		this.BuyItem = strItem;
	}

	public void AddPurchase(List<GooglePurchase> purchases)
	{
		this.m_PurchaseList.Clear();
		this.m_PushPurchaseList.Clear();
		for (int i = 0; i < purchases.Count; i++)
		{
			this.m_PurchaseList.Add(purchases[i].productId, purchases[i]);
		}
		foreach (GooglePurchase current in this.m_PurchaseList.Values)
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

	public bool IsRecoveryItem()
	{
		return this.m_bRecovery;
	}

	public void StartRecoveryItem()
	{
		this.m_bRecovery = true;
	}

	public void CheckRestoreItem()
	{
		if (!this.m_bSendQueryInventory)
		{
			string[] items = NrTSingleton<ItemMallItemManager>.Instance.GetItems();
			GoogleIAB.queryInventory(items);
			this.m_bSendQueryInventory = true;
		}
	}

	public void RecoveryItem()
	{
		this.m_RecoveryItem = this.m_PushPurchaseList.Pop();
		if (this.m_RecoveryItem != null)
		{
			this.Receipt.Remove(0, this.Receipt.Length);
			string arg = string.Concat(new object[]
			{
				"{\"orderId\":\"",
				this.m_RecoveryItem.orderId,
				"\",\"packageName\":\"",
				this.m_RecoveryItem.packageName,
				"\",\"productId\":\"",
				this.m_RecoveryItem.productId,
				"\",\"purchaseTime\":",
				this.m_RecoveryItem.purchaseTime,
				",\"purchaseState\":",
				(int)this.m_RecoveryItem.purchaseState,
				",\"developerPayload\":\"",
				this.m_RecoveryItem.developerPayload,
				"\",\"purchaseToken\":\"",
				this.m_RecoveryItem.purchaseToken,
				"\"}"
			});
			this.Receipt.Append(string.Format("MarketType={0}&Signature={1}&SignedData={2}", "android", this.m_RecoveryItem.signature, arg));
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				GS_BILLINGCHECK_REQ gS_BILLINGCHECK_REQ = new GS_BILLINGCHECK_REQ();
				gS_BILLINGCHECK_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(this.m_RecoveryItem.productId);
				gS_BILLINGCHECK_REQ.SN = myCharInfo.m_SN;
				TKString.StringChar(this.Receipt.ToString(), ref gS_BILLINGCHECK_REQ.Receipt);
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLINGCHECK_REQ, gS_BILLINGCHECK_REQ);
				TsPlatform.FileLog(string.Format("GooglePurchaseComplete\nReceipt ={0} ProductID = {1} ItemMallIdex = {2}", this.Receipt, this.m_RecoveryItem.productId, gS_BILLINGCHECK_REQ.i64ItemMallIndex));
				GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
				gS_BILLING_ITEM_RECODE_REQ.i8Type = 2;
				gS_BILLING_ITEM_RECODE_REQ.i8Result = 0;
				gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = gS_BILLINGCHECK_REQ.i64ItemMallIndex;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
				PlayerPrefs.SetString(NrPrefsKey.SHOP_PRODUCT_ID, gS_BILLINGCHECK_REQ.i64ItemMallIndex.ToString());
				PlayerPrefs.SetString(NrPrefsKey.SHOP_RECEIPT, this.Receipt.ToString());
				if (TsPlatform.IsBand)
				{
					PlayerPrefs.SetString(NrPrefsKey.BAND_RECEIPT, this.m_RecoveryItem.orderId);
				}
			}
		}
		else
		{
			this.m_bRecovery = false;
		}
	}

	public void ConsumeProduct(string ProductId)
	{
		GoogleIAB.consumeProduct(ProductId);
	}

	public void ConsumeProduct()
	{
		if (this.m_RecoveryItem != null)
		{
			GoogleIAB.consumeProduct(this.m_RecoveryItem.productId);
		}
	}

	public void RemoveRcoveryItemData(GooglePurchase ConsumeItem)
	{
		this.m_bRecovery = false;
		if (this.m_RecoveryItem != null)
		{
			this.m_RecoveryItem = null;
		}
		this.m_PurchaseList.Remove(ConsumeItem.productId);
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

	public void ConsumeFailedItemData(GooglePurchase ConsumeItem)
	{
		this.RemoveRcoveryItemData(ConsumeItem);
		if (!this.m_ConsumeFailedList.Contains(ConsumeItem))
		{
			this.m_ConsumeFailedList.Add(ConsumeItem);
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
