using GameMessage;
using System;
using UnityEngine;

public class NrMobileNoticeWeb : IWebViewListener
{
	private string OpenURL = string.Empty;

	public void OnFrontNotice()
	{
		this.OpenURL = string.Format("http://{0}/mobilenotice/top_view.aspx", NrGlobalReference.strWebPageDomain);
		TsPlatform.Operator.OpenURL(this.OpenURL, this, true, false, true, true);
	}

	public void OpenWebURL(string url)
	{
		if (TsPlatform.IsMobile)
		{
			TsPlatform.Operator.OpenURL(url, this, true, true, false, true);
		}
		else
		{
			Application.OpenURL(url);
		}
	}

	public void OnGameUnregister(string strOTP)
	{
		this.OpenURL = string.Format("{0}://{1}/member/member_out_auth.aspx?OTP={2}", (!NrTSingleton<NrGlobalReference>.Instance.IsLocalServiceArea()) ? "https" : "http", NrGlobalReference.strWebPageDomain, strOTP);
		TsPlatform.Operator.OpenURL(this.OpenURL, this, true, true, false, true);
	}

	public void OnGameNotice(string strOTP, string strCharName)
	{
		string text = string.Empty;
		if (TsPlatform.IsAndroid)
		{
			text = TsPlatform.APP_VERSION_AND;
		}
		else if (TsPlatform.IsIPhone)
		{
			text = TsPlatform.APP_VERSION_IOS;
		}
		this.OpenURL = string.Format("https://forum.nexonm.com/forum/legion-of-heroes/announcements-aa#main-navbar-wrapper", new object[0]);
		TsPlatform.Operator.OpenURL(this.OpenURL, this, true, true, false, true);
	}

	public void OnGameQuestion(string strOTP, string strCharName)
	{
		string text = string.Empty;
		if (TsPlatform.IsAndroid)
		{
			text = TsPlatform.APP_VERSION_AND;
		}
		else if (TsPlatform.IsIPhone)
		{
			text = TsPlatform.APP_VERSION_IOS;
		}
		this.OpenURL = string.Format("{0}://{1}/legion-of-heroes-support", (!NrTSingleton<NrGlobalReference>.Instance.IsLocalServiceArea()) ? "https" : "http", NrGlobalReference.strUSAWepPageDomain, strOTP);
		TsPlatform.Operator.OpenURL(this.OpenURL, this, true, true, false, true);
	}

	public void OnFaceBook()
	{
		if (NrGlobalReference.strLangType.Equals("eng"))
		{
			TsPlatform.Operator.OpenURL("https://www.facebook.com/playlegionofheroes", this, true, true, false, true);
		}
		else
		{
			TsPlatform.Operator.OpenURL("http://www.facebook.com/NEXONyg", this, true, true, false, true);
		}
	}

	public void OnMainCafe()
	{
		if (NrGlobalReference.strLangType.Equals("eng"))
		{
			TsPlatform.Operator.OpenURL("https://forum.nexonm.com/forum/legion-of-heroes#main-navbar-wrapper", this, true, true, false, true);
		}
		else
		{
			TsPlatform.Operator.OpenURL("http://cafe.naver.com/legionofheroes", this, true, true, false, true);
		}
	}

	public void OnBandTerm()
	{
		TsPlatform.Operator.OpenURL(string.Format("http://{0}/member/bandpolicy.aspx", NrGlobalReference.strWebPageDomain), this, true, false, false, false);
	}

	public void OnNoticeEvent(string num)
	{
		this.OpenURL = string.Format("https://{0}/mobilenotice/event_view.aspx?seq={1}", NrGlobalReference.strWebPageDomain, num);
		TsPlatform.Operator.OpenURL(this.OpenURL, this, true, false, true, true);
	}

	public void OnPolicyView()
	{
		TsPlatform.Operator.OpenURL("http://m-page.nexon.com/term/16", this, true, true, false, true);
	}

	public bool OnWebCall(string WebCall)
	{
		if (WebCall.Contains("InternetConnnetError"))
		{
			MsgHandler.Handle("InternetConnnetError", new object[0]);
			TsPlatform.Operator.CloseWebView();
			return true;
		}
		if (WebCall.Contains("EmailOpen"))
		{
			char[] separator = new char[]
			{
				'='
			};
			string[] array = WebCall.Split(separator);
			if (2 <= array.Length)
			{
				string url = "http://" + array[1];
				Application.OpenURL(url);
				MsgHandler.Handle("QuitAPP", new object[0]);
			}
		}
		if (WebCall.Contains("close"))
		{
			NrWebViewObject gameObject = NrWebViewObject.GetGameObject();
			if (gameObject.GameNoticeOpen)
			{
				gameObject.GameNoticeOpen = false;
			}
			TsPlatform.Operator.CloseWebView();
		}
		if (WebCall.Contains("member_out_proc"))
		{
			NrWebViewObject gameObject2 = NrWebViewObject.GetGameObject();
			if (gameObject2.GameNoticeOpen)
			{
				gameObject2.GameNoticeOpen = false;
			}
			TsPlatform.Operator.CloseWebView();
			MsgHandler.Handle("RequestMemberSecession", new object[0]);
		}
		if (WebCall.Contains("bandpolicy_proc"))
		{
			NrWebViewObject gameObject3 = NrWebViewObject.GetGameObject();
			if (gameObject3.GameNoticeOpen)
			{
				gameObject3.GameNoticeOpen = false;
			}
			TsPlatform.Operator.CloseWebView();
			MsgHandler.Handle("ShowTerm", new object[0]);
			PlayerPrefs.SetInt(NrPrefsKey.SHOW_TERM, 1);
		}
		if (WebCall.Contains("qna_Write.aspx"))
		{
			MsgHandler.Handle("SetPlacement", new object[]
			{
				"cs_direct"
			});
		}
		if (WebCall.Contains("customer.aspx"))
		{
			MsgHandler.Handle("SetPlacement", new object[]
			{
				"cs_menu"
			});
		}
		if (WebCall.Contains("AuthPlatformSync_proc.aspx") && WebCall.Contains("msg=success"))
		{
			TsPlatform.Operator.CloseWebView();
			MsgHandler.Handle("ConvertGuestID", new object[]
			{
				false
			});
		}
		return true;
	}

	public void OnAlertViewConfirmed(string message)
	{
		TsLog.LogError("OnAlertViewConfirmed Message = {0}", new object[]
		{
			message
		});
		if (message.Contains("â�� �ݾ��ֽñ�"))
		{
			TsPlatform.Operator.CloseWebView();
		}
	}

	public void OnFinishCall(string message)
	{
		TsLog.LogError("OnFinishCall ", new object[0]);
		if (TsPlatform.IsAndroid)
		{
			TsPlatform.Operator.CloseWebView();
		}
	}

	public void OnGoCustomer()
	{
	}

	public void OnMineGuideWebCall(string mineguidewebcall)
	{
		if (mineguidewebcall != string.Empty)
		{
			TsPlatform.Operator.OpenURL(mineguidewebcall, this, true, false, false, true);
		}
	}

	public void OnRateOpenUrl(string strUrl)
	{
		TsPlatform.Operator.OpenURL(strUrl, this, true, true, false, true);
	}
}
