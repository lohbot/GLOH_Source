using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NIAPUnityPlugin : MonoBehaviour
{
	private static NIAPUnityPlugin _instance;

	private static string DELIMITER = "(*^o^*)";

	public AndroidJavaObject activity;

	public event Action<NIAPResult> onPaymentCompletedEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.onPaymentCompletedEvent = (Action<NIAPResult>)Delegate.Combine(this.onPaymentCompletedEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.onPaymentCompletedEvent = (Action<NIAPResult>)Delegate.Remove(this.onPaymentCompletedEvent, value);
		}
	}

	public event Action<NIAPResult> onReceivedProductInfosEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.onReceivedProductInfosEvent = (Action<NIAPResult>)Delegate.Combine(this.onReceivedProductInfosEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.onReceivedProductInfosEvent = (Action<NIAPResult>)Delegate.Remove(this.onReceivedProductInfosEvent, value);
		}
	}

	public event Action<NIAPResult> onReceivedPaymentSeqEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.onReceivedPaymentSeqEvent = (Action<NIAPResult>)Delegate.Combine(this.onReceivedPaymentSeqEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.onReceivedPaymentSeqEvent = (Action<NIAPResult>)Delegate.Remove(this.onReceivedPaymentSeqEvent, value);
		}
	}

	public event Action<NIAPResult> onReceivedReceiptEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.onReceivedReceiptEvent = (Action<NIAPResult>)Delegate.Combine(this.onReceivedReceiptEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.onReceivedReceiptEvent = (Action<NIAPResult>)Delegate.Remove(this.onReceivedReceiptEvent, value);
		}
	}

	public event Action<NIAPResult> onPaymentCanceledEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.onPaymentCanceledEvent = (Action<NIAPResult>)Delegate.Combine(this.onPaymentCanceledEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.onPaymentCanceledEvent = (Action<NIAPResult>)Delegate.Remove(this.onPaymentCanceledEvent, value);
		}
	}

	public event Action<NIAPResult> onErrorEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.onErrorEvent = (Action<NIAPResult>)Delegate.Combine(this.onErrorEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.onErrorEvent = (Action<NIAPResult>)Delegate.Remove(this.onErrorEvent, value);
		}
	}

	public static NIAPUnityPlugin instance
	{
		get
		{
			if (NIAPUnityPlugin._instance == null)
			{
				NIAPUnityPlugin._instance = (UnityEngine.Object.FindObjectOfType(typeof(NIAPUnityPlugin)) as NIAPUnityPlugin);
				if (NIAPUnityPlugin._instance == null)
				{
					NIAPUnityPlugin._instance = new GameObject("NIAPUnityPlugin").AddComponent<NIAPUnityPlugin>();
					UnityEngine.Object.DontDestroyOnLoad(NIAPUnityPlugin._instance);
				}
			}
			return NIAPUnityPlugin._instance;
		}
	}

	private void Awake()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		this.activity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public void initialize(string appCode, string iapKey)
	{
		NIAPUnityPlugin.instance.activity.Call("initialize", new object[]
		{
			appCode,
			iapKey
		});
	}

	public void requestProductInfos(string[] productCodes)
	{
		IntPtr methodID = AndroidJNI.GetMethodID(NIAPUnityPlugin.instance.activity.GetRawClass(), "requestProductInfos", "([Ljava/lang/String;)V");
		AndroidJNI.CallVoidMethod(NIAPUnityPlugin.instance.activity.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(new object[]
		{
			productCodes
		}));
	}

	public void requestPayment(string productCode, int paymentPrice, string extra)
	{
		NIAPUnityPlugin.instance.activity.Call("requestPayment", new object[]
		{
			productCode,
			paymentPrice,
			extra
		});
	}

	public void requestReceipt(string paymentSeq)
	{
		NIAPUnityPlugin.instance.activity.Call("requestReceipt", new object[]
		{
			paymentSeq
		});
	}

	public void onPaymentCompleted(string message)
	{
		if (this.onPaymentCompletedEvent != null)
		{
			NIAPResult obj = this.convertToObject(message);
			this.onPaymentCompletedEvent(obj);
		}
	}

	public void onReceivedProductInfos(string message)
	{
		if (this.onReceivedProductInfosEvent != null)
		{
			NIAPResult obj = this.convertToObject(message);
			this.onReceivedProductInfosEvent(obj);
		}
	}

	public void onReceivedPaymentSeq(string message)
	{
		if (this.onReceivedPaymentSeqEvent != null)
		{
			NIAPResult obj = this.convertToObject(message);
			this.onReceivedPaymentSeqEvent(obj);
		}
	}

	public void onReceivedReceipt(string message)
	{
		if (this.onReceivedReceiptEvent != null)
		{
			NIAPResult obj = this.convertToObject(message);
			this.onReceivedReceiptEvent(obj);
		}
	}

	public void onPaymentCanceled(string message)
	{
		if (this.onPaymentCanceledEvent != null)
		{
			NIAPResult obj = this.convertToObject(message);
			this.onPaymentCanceledEvent(obj);
		}
	}

	public void onError(string message)
	{
		if (this.onErrorEvent != null)
		{
			NIAPResult obj = this.convertToObject(message);
			this.onErrorEvent(obj);
		}
	}

	public NIAPResult convertToObject(string message)
	{
		string[] array = message.Split(new string[]
		{
			NIAPUnityPlugin.DELIMITER
		}, StringSplitOptions.None);
		string requestType = array[0];
		string resultType = array[1];
		string result = array[2];
		string extraValue = array[3];
		return new NIAPResult(requestType, resultType, result, extraValue);
	}

	public void showErrorMessage(NIAPResult result)
	{
		NIAPUnityPlugin.instance.activity.Call("showErrorMessage", new object[]
		{
			result.getRequestType(),
			result.getResult()
		});
	}

	public void showResultMessage(NIAPResult result)
	{
		NIAPUnityPlugin.instance.activity.Call("showResultMessage", new object[]
		{
			result.getRequestType(),
			result.getResultType(),
			result.getResult(),
			result.getExtraValue()
		});
	}
}
