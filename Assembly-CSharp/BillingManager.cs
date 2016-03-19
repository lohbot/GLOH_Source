using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using SERVICE;
using System;
using System.Text;
using UnityEngine;

public abstract class BillingManager : MonoBehaviour
{
	public enum eBillingManager_Type
	{
		None,
		BillingManager_Google,
		BillingManager_TStore,
		BillingManager_IOS,
		BillingManager_NStore,
		BillingManager_ChnStore
	}

	protected StringBuilder Receipt = new StringBuilder();

	private static BillingManager.eBillingManager_Type g_eBillingType;

	public static BillingManager.eBillingManager_Type eBillingType
	{
		get
		{
			if (BillingManager.g_eBillingType == BillingManager.eBillingManager_Type.None)
			{
				BillingManager.InitBillingType();
			}
			return BillingManager.g_eBillingType;
		}
	}

	private static void InitBillingType()
	{
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		if (currentServiceArea.ToString().Contains("GOOGLE") || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORKAKAO || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE)
		{
			BillingManager.g_eBillingType = BillingManager.eBillingManager_Type.BillingManager_Google;
		}
		else if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORTSTORE || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KAKAOTSTORE)
		{
			BillingManager.g_eBillingType = BillingManager.eBillingManager_Type.BillingManager_TStore;
		}
		else if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORNAVER || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_BANDNAVER)
		{
			BillingManager.g_eBillingType = BillingManager.eBillingManager_Type.BillingManager_NStore;
		}
	}

	public abstract void PurchaseItem(string strItem, int price);

	public static void PurchaseItem(ITEM_MALL_ITEM item, bool bIfItemKeyNull_NotifyCantBuy)
	{
		if (item == null)
		{
			return;
		}
		BillingManager billingManager = BillingManager.GetBillingManager();
		if (billingManager == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(item.GetStoreItem()))
		{
			if (bIfItemKeyNull_NotifyCantBuy)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("211"));
			}
			else
			{
				TsLog.LogError("Store Item key is null", new object[0]);
				Main_UI_SystemMessage.ADDMessage("Store Item key is null", SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
			}
			return;
		}
		if (billingManager != null)
		{
			GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
			gS_BILLING_ITEM_RECODE_REQ.i8Type = 0;
			gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = item.m_Idx;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
			billingManager.PurchaseItem(item.GetStoreItem(), (int)item.m_nPrice);
			NrTSingleton<FiveRocksEventManager>.Instance.Placement("paying");
		}
	}

	public static BillingManager GetBillingManager()
	{
		GameObject gameObject = null;
		if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_Google)
		{
			gameObject = GameObject.Find("BillingManager_Google");
			if (gameObject != null)
			{
				return gameObject.GetComponent<BillingManager_Google>();
			}
		}
		else if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_TStore)
		{
			gameObject = GameObject.Find("BillingManager_TStore");
			if (gameObject != null)
			{
				return gameObject.GetComponent<BillingManager_TStore>();
			}
		}
		else if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_NStore)
		{
			gameObject = GameObject.Find("BillingManager_NStore");
			if (gameObject != null)
			{
				return gameObject.GetComponent<BillingManager_NStore>();
			}
		}
		if (gameObject == null)
		{
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			if (currentServiceArea != eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL)
			{
				TsLog.LogError("BillingManager file: objBillingManager == null", new object[0]);
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage("If KORLOCAL, Can't use billing system", SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
		return null;
	}
}
