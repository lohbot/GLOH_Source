using SERVICE;
using System;
using UnityEngine;
using UnityForms;

public class GuestIDCombineDlg : Form
{
	private Button m_kEmail;

	private Button m_kFaceBook;

	private Button m_kKakao;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "GuestID/DLG_GuestID_Combine", G_ID.GUESTID_COMBINE_DLG, true);
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	public override void SetComponent()
	{
		this.m_kEmail = (base.GetControl("Button_ok") as Button);
		this.m_kEmail.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickEmail));
		this.m_kFaceBook = (base.GetControl("Button_ok_C") as Button);
		this.m_kFaceBook.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickFaceBook));
		this.m_kKakao = (base.GetControl("Button_ok_C_C") as Button);
		this.m_kKakao.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickKakao));
		this.m_kFaceBook.Visible = false;
		this.m_kKakao.Visible = false;
		this.m_kEmail.Visible = true;
		this.m_kFaceBook.Visible = true;
		base.SetScreenCenter();
	}

	private void ClickKakao(IUIObject obj)
	{
		NrMobileAuthSystem.Instance.RequestLogout = true;
		NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
		PlayerPrefs.SetInt(NrPrefsKey.CONVERT_GUESTID, 0);
	}

	private void ClickEmail(IUIObject obj)
	{
		NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_GUESTID);
	}

	private void ClickFaceBook(IUIObject obj)
	{
		NmFacebookManager.instance.SyncGestID();
	}
}
