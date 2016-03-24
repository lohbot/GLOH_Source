using MATSDK;
using SERVICE;
using System;
using UnityEngine;

public class MATEventManager : NrTSingleton<MATEventManager>
{
	public GameObject m_goMATDelegate;

	private MATEventManager()
	{
	}

	public void Set_MAT()
	{
		this.m_goMATDelegate = GameObject.Find("MobileAppTracker");
		if (this.m_goMATDelegate == null)
		{
			this.m_goMATDelegate = new GameObject("MobileAppTracker");
		}
		if (this.m_goMATDelegate != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(this.m_goMATDelegate);
			MATDelegate x = this.m_goMATDelegate.GetComponent<MATDelegate>();
			if (x == null)
			{
				x = this.m_goMATDelegate.AddComponent<MATDelegate>();
			}
			MATBinding x2 = this.m_goMATDelegate.GetComponent<MATBinding>();
			if (x2 == null)
			{
				x2 = this.m_goMATDelegate.AddComponent<MATBinding>();
			}
		}
	}

	public void Init(string _strAdID, string _strConversionKey, string _strPackageName)
	{
		if (this.m_goMATDelegate == null)
		{
			return;
		}
		MATBinding.Init(_strAdID, _strConversionKey);
		MATBinding.SetPackageName(_strPackageName);
		MATBinding.SetFacebookEventLogging(true, false);
		MATBinding.AutomateIapEventMeasurement(false);
		MATBinding.MeasureSession();
	}

	public void Set_UserID(string _strUserId)
	{
		if (this.m_goMATDelegate == null)
		{
			return;
		}
		MATBinding.SetUserId(_strUserId);
	}

	public void MeasureEvent(string _EventName)
	{
		if (this.m_goMATDelegate == null)
		{
			return;
		}
		MATBinding.MeasureEvent(_EventName);
	}

	public void Measure_Purchase(long _MallIndex)
	{
		if (this.m_goMATDelegate == null)
		{
			return;
		}
		ITEM_MALL_ITEM item = NrTSingleton<ItemMallItemManager>.Instance.GetItem(_MallIndex);
		if (item == null)
		{
			return;
		}
		string textFromItem = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(item.m_strTextKey);
		string currencyCode = string.Empty;
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORGOOGLE || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORKAKAO)
		{
			if (item.m_nMoneyType == 1)
			{
				currencyCode = "KRW";
				MATItem[] eventItems = new MATItem[]
				{
					new MATItem(textFromItem)
					{
						quantity = new int?(1),
						unitPrice = new double?((double)item.m_nPrice),
						revenue = new double?((double)item.m_nPrice)
					}
				};
				MATBinding.MeasureEvent(new MATEvent("Purchase")
				{
					currencyCode = currencyCode,
					eventItems = eventItems,
					receiptSignature = PlayerPrefs.GetString(NrPrefsKey.SHOP_RECEIPT, string.Empty),
					revenue = new double?((double)item.m_nPrice),
					advertiserRefId = "186576"
				});
			}
		}
		else if (item.m_nMoneyType == 1)
		{
			currencyCode = "USD";
			MATBinding.MeasureEvent(new MATEvent("Purchase")
			{
				revenue = new double?((double)item.m_fPrice),
				currencyCode = currencyCode,
				advertiserRefId = textFromItem
			});
		}
	}

	public void TutorialComplete()
	{
		if (this.m_goMATDelegate == null)
		{
			return;
		}
		MATBinding.MeasureEvent("tutorial complete");
	}
}
