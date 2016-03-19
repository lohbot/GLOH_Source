using GooglePlayGames;
using PROTOCOL;
using PROTOCOL.WORLD;
using SERVICE;
using System;
using UnityEngine;
using UnityForms;

public class ConvertPlatformIDDlg : Form
{
	private Button m_kConvert1;

	private Button m_kConvert2;

	private Button m_kConvert3;

	private Button m_kConvert4;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "GuestID/dlg_accountlink", G_ID.CONVER_PLATFORMID_DLG, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_kConvert1 = (base.GetControl("Button_Link1") as Button);
		this.m_kConvert1.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickConvert1));
		this.m_kConvert2 = (base.GetControl("Button_Link2") as Button);
		this.m_kConvert2.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickConvert2));
		this.m_kConvert3 = (base.GetControl("Button_Link3") as Button);
		this.m_kConvert3.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickConvert3));
		this.m_kConvert4 = (base.GetControl("Button_Link4") as Button);
		this.m_kConvert4.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickConvert4));
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		base.SetShowLayer(3, false);
		if (TsPlatform.IsIPhone)
		{
			this.m_kConvert2.Visible = false;
		}
		base.SetScreenCenter();
	}

	private void ClickConvert1(IUIObject obj)
	{
	}

	private void ClickConvert2(IUIObject obj)
	{
		if (NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType() == 6)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("720"));
			return;
		}
		bool bAuth = false;
		if (TsPlatform.IsAndroid)
		{
			Social.localUser.Authenticate(delegate(bool success)
			{
				bAuth = true;
				if (success)
				{
					Debug.Log("success to sign in with Google Play Games.");
					Debug.Log("@@@@@@@@@ID : " + PlayGamesPlatform.Instance.localUser.id);
					NrTSingleton<NkClientLogic>.Instance.AuthPlatformType = eAuthPlatformType.AUTH_PLATFORMTYPE_GOOGLEPLAY;
					WS_PLATFORMID_SET_REQ wS_PLATFORMID_SET_REQ = new WS_PLATFORMID_SET_REQ();
					TKString.StringChar(PlayGamesPlatform.Instance.localUser.id, ref wS_PLATFORMID_SET_REQ.m_szPlatformID);
					wS_PLATFORMID_SET_REQ.m_nPlatformType = (int)NrTSingleton<NkClientLogic>.Instance.AuthPlatformType;
					SendPacket.GetInstance().SendObject(16777273, wS_PLATFORMID_SET_REQ);
				}
				else
				{
					Debug.Log("Failed to sign in with Google Play Games.");
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("145"));
				}
			});
		}
		if (!bAuth)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("145"));
		}
	}

	private void ClickConvert3(IUIObject obj)
	{
		if (NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType() == 2)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("720"));
			return;
		}
		NmFacebookManager.instance.SyncGestID();
	}

	private void ClickConvert4(IUIObject obj)
	{
		if (NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType() == 1)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("720"));
			return;
		}
		NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_GUESTID);
	}
}
