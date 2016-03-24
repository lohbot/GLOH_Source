using SERVICE;
using System;
using UnityEngine;
using UnityForms;

public class HelpDeskDlg : Form
{
	private string m_strOTP = string.Empty;

	private byte m_ViewType;

	private Button m_btNativeView;

	private Button m_btOutView;

	private Button m_btClose;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "System/dlg_helpdesk", G_ID.HELPDESK_DLG, false);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_btNativeView = (base.GetControl("Button_original") as Button);
		this.m_btNativeView.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNativeView));
		this.m_btOutView = (base.GetControl("Button_new") as Button);
		this.m_btOutView.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOutView));
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		base.SetScreenCenter();
	}

	public void SetOTP(string OTP, string strCharName)
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
		this.m_strOTP = OTP;
		byte viewType = this.m_ViewType;
		if (viewType != 0)
		{
			if (viewType == 1)
			{
				if (TsPlatform.IsAndroid)
				{
					text = TsPlatform.APP_VERSION_AND;
				}
				else if (TsPlatform.IsIPhone)
				{
					text = TsPlatform.APP_VERSION_IOS;
				}
				NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_HELPQUESTION);
				string stringToEscape = string.Format("{0}://{1}/customer/auth.aspx?otp={2}&GameVer={3}&HpType={4}&HpOS={5}&CharacterName={6}&image=Y", new object[]
				{
					(!NrTSingleton<NrGlobalReference>.Instance.IsLocalServiceArea()) ? "https" : "http",
					NrGlobalReference.strWebPageDomain,
					this.m_strOTP,
					text,
					SystemInfo.deviceModel,
					(!TsPlatform.IsIPhone) ? SystemInfo.operatingSystem : string.Empty,
					strCharName
				});
				Application.OpenURL(Uri.EscapeUriString(stringToEscape));
			}
		}
		else
		{
			NrMobileNoticeWeb nrMobileNoticeWeb = new NrMobileNoticeWeb();
			nrMobileNoticeWeb.OnGameQuestion(this.m_strOTP, strCharName);
		}
	}

	public void ClickNativeView(IUIObject obj)
	{
		NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_HELPQUESTION);
		this.m_ViewType = 0;
	}

	public void ClickOutView(IUIObject obj)
	{
		NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_HELPQUESTION);
		this.m_ViewType = 1;
	}
}
