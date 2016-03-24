using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NIAPNativeExtension : MonoBehaviour
{
	private NIAPPlugin plugin;

	private static NIAPNativeExtension _instance;

	public event Action<NIAPResultError> failureEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.failureEvent = (Action<NIAPResultError>)Delegate.Combine(this.failureEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.failureEvent = (Action<NIAPResultError>)Delegate.Remove(this.failureEvent, value);
		}
	}

	public event Action<NIAPResult> getProductDetailsSuccessEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.getProductDetailsSuccessEvent = (Action<NIAPResult>)Delegate.Combine(this.getProductDetailsSuccessEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.getProductDetailsSuccessEvent = (Action<NIAPResult>)Delegate.Remove(this.getProductDetailsSuccessEvent, value);
		}
	}

	public event Action<NIAPPurchase> requestPaymentSuccessEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.requestPaymentSuccessEvent = (Action<NIAPPurchase>)Delegate.Combine(this.requestPaymentSuccessEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.requestPaymentSuccessEvent = (Action<NIAPPurchase>)Delegate.Remove(this.requestPaymentSuccessEvent, value);
		}
	}

	public event Action<NIAPResult> requestPaymentCanceledEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.requestPaymentCanceledEvent = (Action<NIAPResult>)Delegate.Combine(this.requestPaymentCanceledEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.requestPaymentCanceledEvent = (Action<NIAPResult>)Delegate.Remove(this.requestPaymentCanceledEvent, value);
		}
	}

	public event Action<NIAPResult> consumeSuccessEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.consumeSuccessEvent = (Action<NIAPResult>)Delegate.Combine(this.consumeSuccessEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.consumeSuccessEvent = (Action<NIAPResult>)Delegate.Remove(this.consumeSuccessEvent, value);
		}
	}

	public event Action<NIAPResult> getPurchasesSuccessEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.getPurchasesSuccessEvent = (Action<NIAPResult>)Delegate.Combine(this.getPurchasesSuccessEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.getPurchasesSuccessEvent = (Action<NIAPResult>)Delegate.Remove(this.getPurchasesSuccessEvent, value);
		}
	}

	public event Action<NIAPPurchase> getSinglePurchaseSuccessEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.getSinglePurchaseSuccessEvent = (Action<NIAPPurchase>)Delegate.Combine(this.getSinglePurchaseSuccessEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.getSinglePurchaseSuccessEvent = (Action<NIAPPurchase>)Delegate.Remove(this.getSinglePurchaseSuccessEvent, value);
		}
	}

	public static NIAPNativeExtension Instance
	{
		get
		{
			if (!NIAPNativeExtension._instance)
			{
				NIAPNativeExtension._instance = (UnityEngine.Object.FindObjectOfType(typeof(NIAPNativeExtension)) as NIAPNativeExtension);
				if (!NIAPNativeExtension._instance)
				{
					GameObject gameObject = new GameObject("NIAPNativeExtension");
					NIAPNativeExtension._instance = (gameObject.AddComponent(typeof(NIAPNativeExtension)) as NIAPNativeExtension);
					UnityEngine.Object.DontDestroyOnLoad(NIAPNativeExtension._instance);
				}
			}
			return NIAPNativeExtension._instance;
		}
	}

	public void initialize(string publicKey)
	{
		if (this.plugin == null)
		{
			this.plugin = ScriptableObject.CreateInstance<NIAPPluginAndroid>();
		}
		NIAPParamInit param = new NIAPParamInit(publicKey);
		this.plugin.invoke(param);
	}

	public void getProductDetails(List<string> productCodes, Action<NIAPResult> success, Action<NIAPResultError> failure)
	{
		this.getProductDetailsSuccessEvent = success;
		this.failureEvent = failure;
		NIAPParamProductInfos param = new NIAPParamProductInfos(productCodes);
		this.plugin.invoke(param);
	}

	public void requestPayment(string productCode, int niapRequestCode, string payLoad, Action<NIAPPurchase> success, Action<NIAPResult> canceled, Action<NIAPResultError> failure)
	{
		this.requestPaymentSuccessEvent = success;
		this.requestPaymentCanceledEvent = canceled;
		this.failureEvent = failure;
		NIAPParamPayment param = new NIAPParamPayment(productCode, niapRequestCode, payLoad);
		this.plugin.invoke(param);
	}

	public void requestConsume(string originalPurchaseAsJsonText, string signature, Action<NIAPResult> success, Action<NIAPResultError> failure)
	{
		this.consumeSuccessEvent = success;
		this.failureEvent = failure;
		NIAPParamConsume param = new NIAPParamConsume(originalPurchaseAsJsonText, signature);
		this.plugin.invoke(param);
	}

	public void getPurchases(Action<NIAPResult> success, Action<NIAPResultError> failure)
	{
		this.getPurchasesSuccessEvent = success;
		this.failureEvent = failure;
		NIAPParam param = new NIAPParam(NIAPConstant.InvokeMethod.getPurchases);
		this.plugin.invoke(param);
	}

	public void getSinglePurchases(string paymentSeq, Action<NIAPPurchase> success, Action<NIAPResultError> failure)
	{
		this.getSinglePurchaseSuccessEvent = success;
		this.failureEvent = failure;
		NIAPParamSinglePurchase param = new NIAPParamSinglePurchase(paymentSeq);
		this.plugin.invoke(param);
	}

	public void returnSuccess(string result)
	{
		JSONNode jSONNode = JSON.Parse(result);
		string text = jSONNode[NIAPConstant.invokeMethod];
		Debug.Log("success! invokeMethod : " + text + " result : " + result);
		if (string.Equals(text, NIAPConstant.InvokeMethod.getProductDetails))
		{
			if (this.getProductDetailsSuccessEvent != null)
			{
				this.getProductDetailsSuccessEvent(NIAPResult.Build(result));
			}
			this.getProductDetailsSuccessEvent = null;
		}
		else if (string.Equals(text, NIAPConstant.InvokeMethod.requestPayment))
		{
			if (this.requestPaymentSuccessEvent != null)
			{
				this.requestPaymentSuccessEvent(NIAPPurchase.Build(result));
			}
			this.requestPaymentSuccessEvent = null;
			this.requestPaymentCanceledEvent = null;
		}
		else if (string.Equals(text, NIAPConstant.InvokeMethod.requestConsume))
		{
			if (this.consumeSuccessEvent != null)
			{
				this.consumeSuccessEvent(NIAPResult.Build(result));
			}
			this.consumeSuccessEvent = null;
		}
		else if (string.Equals(text, NIAPConstant.InvokeMethod.getPurchases))
		{
			if (this.getPurchasesSuccessEvent != null)
			{
				this.getPurchasesSuccessEvent(NIAPResult.Build(result));
			}
			this.getPurchasesSuccessEvent = null;
		}
		else if (string.Equals(text, NIAPConstant.InvokeMethod.getSinglePurchase))
		{
			if (this.getSinglePurchaseSuccessEvent != null)
			{
				this.getSinglePurchaseSuccessEvent(NIAPPurchase.Build(result));
			}
			this.getSinglePurchaseSuccessEvent = null;
		}
	}

	public void returnFailure(string error)
	{
		Debug.Log("fail! error result error : " + error);
		if (this.failureEvent != null)
		{
			this.failureEvent(NIAPResultError.Build(error));
		}
		this.failureEvent = null;
	}

	public void returnCancel(string result)
	{
		Debug.Log("fail! returnCancel result : " + result);
		JSONNode jSONNode = JSON.Parse(result);
		string text = jSONNode[NIAPConstant.invokeMethod];
		Debug.Log("canceled! invokeMethod : " + text + " result : " + result);
		if (string.Equals(text, NIAPConstant.InvokeMethod.requestPayment))
		{
			if (this.requestPaymentCanceledEvent != null)
			{
				this.requestPaymentCanceledEvent(NIAPResult.Build(result));
			}
			this.requestPaymentSuccessEvent = null;
			this.requestPaymentCanceledEvent = null;
		}
	}

	public void showMessage(string message)
	{
		this.plugin.showMessage(message);
	}
}
