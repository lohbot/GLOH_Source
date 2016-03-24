using GameMessage;
using GooglePlayGames;
using SERVICE;
using System;
using UnityEngine;
using UnityForms;

public class Login_Select_PlatformDLG : Form
{
	private Button m_btnEmailLogin;

	private Button m_btnFacebookLogin;

	private Button m_btnBandLogin;

	private Button m_btnGooglePlay;

	private Button m_btnGuest;

	private Button m_btnKakao;

	private DrawTexture m_txKakao;

	private Button m_btnLine;

	private Label pkText;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		form.Scale = false;
		instance.LoadFileAll(ref form, "Login/dlg_login", G_ID.LOGIN_SELECT_PLATFORM_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		float num = 0f;
		this.m_btnEmailLogin = (base.GetControl("BT_Email") as Button);
		this.m_btnEmailLogin.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickEmail));
		this.m_btnEmailLogin.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("25"));
		this.m_btnEmailLogin.UseDefaultSound = false;
		if (TsPlatform.IsIPhone)
		{
			this.m_btnEmailLogin.SetLocationY(this.m_btnEmailLogin.GetLocationY() + num);
		}
		this.m_btnFacebookLogin = (base.GetControl("BT_FaceBook") as Button);
		this.m_btnFacebookLogin.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickFacebook));
		this.m_btnFacebookLogin.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("26"));
		this.m_btnGooglePlay = (base.GetControl("BT_Google") as Button);
		this.m_btnGooglePlay.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickGooglePlay));
		this.m_btnGooglePlay.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("1188"));
		this.m_btnFacebookLogin.UseDefaultSound = false;
		if (TsPlatform.IsIPhone)
		{
			this.m_btnFacebookLogin.SetLocationY(this.m_btnFacebookLogin.GetLocationY() + num);
		}
		this.m_btnBandLogin = (base.GetControl("BT_Band") as Button);
		this.m_btnBandLogin.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBand));
		this.m_btnBandLogin.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("31"));
		if (TsPlatform.IsIPhone)
		{
			this.m_btnBandLogin.SetLocationY(this.m_btnBandLogin.GetLocationY() + num);
		}
		this.m_btnGuest = (base.GetControl("BT_GUEST") as Button);
		this.m_btnGuest.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnclickGuest));
		this.m_btnGuest.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("33"));
		this.m_btnGuest.UseDefaultSound = false;
		if (TsPlatform.IsIPhone)
		{
			this.m_btnGuest.SetLocationY(this.m_btnGuest.GetLocationY() + num);
		}
		this.m_btnKakao = (base.GetControl("BT_Kakao") as Button);
		this.m_btnKakao.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickKaKao));
		this.m_btnKakao.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("34"));
		this.m_btnKakao.UseDefaultSound = false;
		this.m_txKakao = (base.GetControl("DrawTexture_Kakao") as DrawTexture);
		if (TsPlatform.IsIPhone)
		{
			this.m_btnKakao.SetLocationY(this.m_btnKakao.GetLocationY() + num);
			this.m_txKakao.SetLocationY(this.m_txKakao.GetLocationY() + num);
		}
		this.pkText = (base.GetControl("Label_Copyright") as Label);
		this.pkText.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("37");
		this.pkText.SetSize(this.pkText.spriteText.TotalWidth, this.pkText.spriteText.TotalHeight);
		this.pkText.SetLocation((GUICamera.width - this.pkText.width) / 2f, GUICamera.height - 10f - this.pkText.height);
		this.m_btnLine = (base.GetControl("BT_Line") as Button);
		this.m_btnLine.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickLoginLine));
		if (TsPlatform.IsAndroid)
		{
			this.m_btnGuest.Visible = false;
		}
		else if (PlayerPrefs.HasKey(NrPrefsKey.CONVERT_GUESTID))
		{
			if (PlayerPrefs.GetInt(NrPrefsKey.CONVERT_GUESTID) == 1)
			{
				this.m_btnGuest.Visible = false;
			}
			else
			{
				this.m_btnGuest.Visible = true;
			}
		}
		else
		{
			PlayerPrefs.SetInt(NrPrefsKey.CONVERT_GUESTID, 0);
			this.m_btnGuest.Visible = true;
		}
		this.m_btnBandLogin.Visible = false;
		this.m_btnEmailLogin.Visible = true;
		this.m_btnFacebookLogin.Visible = true;
		this.m_btnKakao.Visible = false;
		this.m_txKakao.Visible = false;
		this.m_btnGooglePlay.Visible = false;
		this.m_btnLine.Visible = false;
		if (TsPlatform.IsAndroid)
		{
			this.m_btnGooglePlay.Visible = true;
		}
		base.SetScreenCenter();
	}

	private void ClickLoginLine(IUIObject obk)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.LOGINRATING_DLG);
		this.Close();
	}

	private void OnclickGuest(IUIObject obk)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(new YesDelegate(this.ClickGuest), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("2"), NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("36"), eMsgType.MB_OK_CANCEL, 2);
		}
	}

	private void ClickGuest(object obj)
	{
		NrTSingleton<NkClientLogic>.Instance.AuthPlatformType = eAuthPlatformType.AUTH_PLATFORMTYPE_NONE;
		NrTSingleton<NkClientLogic>.Instance.SetGuestLogin(true);
		MsgHandler.Handle("RequestLogin", new object[0]);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.LOGINRATING_DLG);
		this.Close();
	}

	private void OnClickEmail(IUIObject obk)
	{
		NrTSingleton<NkClientLogic>.Instance.SetGuestLogin(false);
		NrMobileAuthSystem.Instance.Auth.RequestLogin();
		NmFacebookManager.instance.FacebookLogin = false;
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.LOGINRATING_DLG);
		this.Close();
	}

	private void OnClickGooglePlay(IUIObject obk)
	{
		NrTSingleton<NkClientLogic>.Instance.SetGuestLogin(false);
		string @string = PlayerPrefs.GetString(NrPrefsKey.PLAYER_PREFS_MOBILEAUTHKEY);
		if (!string.IsNullOrEmpty(Social.localUser.id))
		{
			MsgHandler.Handle("RequestLogin", new object[0]);
			Debug.Log("@@@@@@@@@RequestLogin11111111111111111");
		}
		else
		{
			Social.localUser.Authenticate(delegate(bool success)
			{
				if (success)
				{
					Debug.Log("success to sign in with Google Play Games.");
					Debug.Log("@@@@@@@@@ID : " + PlayGamesPlatform.Instance.localUser.id);
					NrTSingleton<NkClientLogic>.Instance.AuthPlatformType = eAuthPlatformType.AUTH_PLATFORMTYPE_GOOGLEPLAY;
					MsgHandler.Handle("RequestLogin", new object[0]);
					Debug.Log("@@@@@@@@@RequestLogin");
				}
				else
				{
					Debug.Log("Failed to sign in with Google Play Games.");
					Login_Select_PlatformDLG login_Select_PlatformDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGIN_SELECT_PLATFORM_DLG) as Login_Select_PlatformDLG;
					if (login_Select_PlatformDLG != null)
					{
						login_Select_PlatformDLG.Show();
					}
				}
			});
		}
		if (TsPlatform.IsAndroid)
		{
			this.Close();
		}
	}

	private void OnClickFacebook(IUIObject obk)
	{
		NrTSingleton<NkClientLogic>.Instance.SetGuestLogin(false);
		NrTSingleton<NkClientLogic>.Instance.AuthPlatformType = eAuthPlatformType.AUTH_PLATFORMTYPE_FACEBOOK;
		NmFacebookManager.instance.init();
		NmFacebookManager.instance.GetMeFacebookData();
		NmFacebookManager.instance.FacebookLogin = true;
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.LOGINRATING_DLG);
		this.Close();
	}

	private void OnClickBand(IUIObject obk)
	{
	}

	private void OnClickKaKao(IUIObject obk)
	{
		if (TsPlatform.IsAndroid)
		{
			this.Close();
		}
	}

	public void PlatformLogin()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.LOGINRATING_DLG);
		if (TsPlatform.IsAndroid)
		{
			this.Close();
		}
	}

	public override void OnClose()
	{
	}
}
