using GameMessage;
using System;
using System.Text;
using UnityEngine;

public class NrWebViewObject : MonoBehaviour
{
	private bool m_bRefreshWebCall = true;

	private string m_szLoadURL = string.Empty;

	private static bool m_bFirstNoticeOpen;

	private static bool m_bGameNoticeOpen;

	public bool m_bMainmenuNoticeOpen;

	private IWebViewListener _WebListener;

	private AndroidJavaClass webView;

	private Vector2 offset;

	private string _strIdentifierName = "com.nexon.loh.korlocal.AndroidAPI";

	public bool FirstNoticeOpen
	{
		get
		{
			return NrWebViewObject.m_bFirstNoticeOpen;
		}
		set
		{
			NrWebViewObject.m_bFirstNoticeOpen = value;
		}
	}

	public bool GameNoticeOpen
	{
		get
		{
			return NrWebViewObject.m_bGameNoticeOpen;
		}
		set
		{
			NrWebViewObject.m_bGameNoticeOpen = value;
		}
	}

	public bool MainmenuNoticeOpen
	{
		get
		{
			return this.m_bMainmenuNoticeOpen;
		}
		set
		{
			this.m_bMainmenuNoticeOpen = value;
		}
	}

	public static NrWebViewObject GetGameObject()
	{
		GameObject gameObject = GameObject.Find(typeof(NrWebViewObject).ToString());
		if (gameObject == null)
		{
			gameObject = new GameObject(typeof(NrWebViewObject).ToString(), new Type[]
			{
				typeof(NrWebViewObject)
			});
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		return gameObject.GetComponent<NrWebViewObject>();
	}

	public void Init(IWebViewListener webcall, bool bShowCloseButton = true)
	{
		this._WebListener = webcall;
		this._strIdentifierName = NrGlobalReference.MOBILEID + ".AndroidAPI";
		this.m_bRefreshWebCall = true;
		this.m_szLoadURL = string.Empty;
	}

	public void Destory()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				if (this.webView != null)
				{
					this.webView.CallStatic("DestoryWebPage", new object[0]);
				}
			}
		}
	}

	public void CloseBanner()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				if (this.webView != null)
				{
					this.webView.CallStatic("CloseBanner", new object[]
					{
						@static
					});
				}
			}
		}
	}

	private void OnDestroy()
	{
	}

	public void SetMargins(int left, int top, int right, int bottom)
	{
	}

	public void SetVisibility(bool v)
	{
	}

	public void LoadURL(string url, bool bVisility, bool bAccessBack, bool bAppQuit, bool bShowCloseButton)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				if (this.webView == null)
				{
					this.webView = new AndroidJavaClass(this._strIdentifierName);
				}
				this.webView.CallStatic("ConnectWebPage", new object[]
				{
					@static,
					base.name,
					url,
					bVisility,
					bAccessBack,
					bAppQuit,
					bShowCloseButton
				});
			}
		}
	}

	public void OpenURL(string url, int nX, int nY, bool bVisibility)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				if (this.webView == null)
				{
					this.webView = new AndroidJavaClass(this._strIdentifierName);
				}
				this.webView.CallStatic("OpenBanner", new object[]
				{
					@static,
					base.name,
					url,
					nX,
					nY,
					bVisibility
				});
			}
		}
	}

	public void PostURL(string url, string post, bool bVisibility)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				if (this.webView == null)
				{
					this.webView = new AndroidJavaClass(this._strIdentifierName);
				}
				byte[] bytes = Encoding.Default.GetBytes(post);
				this.webView.CallStatic("ConnectWebPage_PostUrl", new object[]
				{
					@static,
					base.name,
					url,
					bVisibility,
					bytes,
					true,
					Screen.width,
					Screen.height
				});
			}
		}
	}

	public void Toast(string message)
	{
	}

	public void Update()
	{
		if (!this.m_bRefreshWebCall)
		{
			TsPlatform.Operator.OpenURL(this.m_szLoadURL, this._WebListener, true, false, true, true);
			this.m_bRefreshWebCall = true;
		}
	}

	public void OnWebCall(string loadurl)
	{
		if (this._WebListener != null && loadurl.Length > 0)
		{
			this.m_bRefreshWebCall = this._WebListener.OnWebCall(loadurl);
			if (!this.m_bRefreshWebCall)
			{
				this.m_szLoadURL = loadurl;
			}
			else
			{
				this.m_szLoadURL = string.Empty;
			}
		}
		else
		{
			this.m_bRefreshWebCall = true;
			this.m_szLoadURL = string.Empty;
			MsgHandler.Handle("FacebookLoginFailed", new object[]
			{
				"WebListenerError"
			});
		}
	}

	public void OnAlertViewConfirmed(string message)
	{
		if (this._WebListener != null)
		{
			this._WebListener.OnAlertViewConfirmed(message);
		}
	}

	public void OnFinishCall(string message)
	{
		if (this._WebListener != null)
		{
			this._WebListener.OnFinishCall(message);
		}
	}
}
