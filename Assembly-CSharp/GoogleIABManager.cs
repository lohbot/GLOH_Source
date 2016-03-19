using Prime31;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GoogleIABManager : AbstractManager
{
	public static event Action billingSupportedEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			GoogleIABManager.billingSupportedEvent = (Action)Delegate.Combine(GoogleIABManager.billingSupportedEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			GoogleIABManager.billingSupportedEvent = (Action)Delegate.Remove(GoogleIABManager.billingSupportedEvent, value);
		}
	}

	public static event Action<string> billingNotSupportedEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			GoogleIABManager.billingNotSupportedEvent = (Action<string>)Delegate.Combine(GoogleIABManager.billingNotSupportedEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			GoogleIABManager.billingNotSupportedEvent = (Action<string>)Delegate.Remove(GoogleIABManager.billingNotSupportedEvent, value);
		}
	}

	public static event Action<List<GooglePurchase>, List<GoogleSkuInfo>> queryInventorySucceededEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			GoogleIABManager.queryInventorySucceededEvent = (Action<List<GooglePurchase>, List<GoogleSkuInfo>>)Delegate.Combine(GoogleIABManager.queryInventorySucceededEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			GoogleIABManager.queryInventorySucceededEvent = (Action<List<GooglePurchase>, List<GoogleSkuInfo>>)Delegate.Remove(GoogleIABManager.queryInventorySucceededEvent, value);
		}
	}

	public static event Action<string> queryInventoryFailedEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			GoogleIABManager.queryInventoryFailedEvent = (Action<string>)Delegate.Combine(GoogleIABManager.queryInventoryFailedEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			GoogleIABManager.queryInventoryFailedEvent = (Action<string>)Delegate.Remove(GoogleIABManager.queryInventoryFailedEvent, value);
		}
	}

	public static event Action<string, string> purchaseCompleteAwaitingVerificationEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent = (Action<string, string>)Delegate.Combine(GoogleIABManager.purchaseCompleteAwaitingVerificationEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent = (Action<string, string>)Delegate.Remove(GoogleIABManager.purchaseCompleteAwaitingVerificationEvent, value);
		}
	}

	public static event Action<GooglePurchase> purchaseSucceededEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			GoogleIABManager.purchaseSucceededEvent = (Action<GooglePurchase>)Delegate.Combine(GoogleIABManager.purchaseSucceededEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			GoogleIABManager.purchaseSucceededEvent = (Action<GooglePurchase>)Delegate.Remove(GoogleIABManager.purchaseSucceededEvent, value);
		}
	}

	public static event Action<string> purchaseFailedEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			GoogleIABManager.purchaseFailedEvent = (Action<string>)Delegate.Combine(GoogleIABManager.purchaseFailedEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			GoogleIABManager.purchaseFailedEvent = (Action<string>)Delegate.Remove(GoogleIABManager.purchaseFailedEvent, value);
		}
	}

	public static event Action<GooglePurchase> consumePurchaseSucceededEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			GoogleIABManager.consumePurchaseSucceededEvent = (Action<GooglePurchase>)Delegate.Combine(GoogleIABManager.consumePurchaseSucceededEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			GoogleIABManager.consumePurchaseSucceededEvent = (Action<GooglePurchase>)Delegate.Remove(GoogleIABManager.consumePurchaseSucceededEvent, value);
		}
	}

	public static event Action<string> consumePurchaseFailedEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			GoogleIABManager.consumePurchaseFailedEvent = (Action<string>)Delegate.Combine(GoogleIABManager.consumePurchaseFailedEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			GoogleIABManager.consumePurchaseFailedEvent = (Action<string>)Delegate.Remove(GoogleIABManager.consumePurchaseFailedEvent, value);
		}
	}

	static GoogleIABManager()
	{
		AbstractManager.initialize(typeof(GoogleIABManager));
	}

	public void billingSupported(string empty)
	{
		GoogleIABManager.billingSupportedEvent.fire();
	}

	public void billingNotSupported(string error)
	{
		GoogleIABManager.billingNotSupportedEvent.fire(error);
	}

	public void queryInventorySucceeded(string json)
	{
		if (GoogleIABManager.queryInventorySucceededEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			GoogleIABManager.queryInventorySucceededEvent(GooglePurchase.fromList(dictionary["purchases"] as List<object>), GoogleSkuInfo.fromList(dictionary["skus"] as List<object>));
		}
	}

	public void queryInventoryFailed(string error)
	{
		GoogleIABManager.queryInventoryFailedEvent.fire(error);
	}

	public void purchaseCompleteAwaitingVerification(string json)
	{
		if (GoogleIABManager.purchaseCompleteAwaitingVerificationEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			string arg = dictionary["purchaseData"].ToString();
			string arg2 = dictionary["signature"].ToString();
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent(arg, arg2);
		}
	}

	public void purchaseSucceeded(string json)
	{
		GoogleIABManager.purchaseSucceededEvent.fire(new GooglePurchase(json.dictionaryFromJson()));
	}

	public void purchaseFailed(string error)
	{
		GoogleIABManager.purchaseFailedEvent.fire(error);
	}

	public void consumePurchaseSucceeded(string json)
	{
		if (GoogleIABManager.consumePurchaseSucceededEvent != null)
		{
			GoogleIABManager.consumePurchaseSucceededEvent.fire(new GooglePurchase(json.dictionaryFromJson()));
		}
	}

	public void consumePurchaseFailed(string error)
	{
		GoogleIABManager.consumePurchaseFailedEvent.fire(error);
	}
}
