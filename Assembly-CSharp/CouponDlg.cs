using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityForms;

public class CouponDlg : Form
{
	private enum TYPE
	{
		SEND_SERVER,
		SEND_CHAT
	}

	private CouponDlg.TYPE m_eType;

	private TextField m_tfInput;

	private Button m_btOK;

	private Button m_btCancel;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "System/DLG_Couponinput", G_ID.COUPON_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_tfInput = (base.GetControl("TextField_coupon") as TextField);
		this.m_tfInput.maxLength = 32;
		this.m_tfInput.MaxWidth = 0f;
		this.m_btOK = (base.GetControl("Button_OK") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		this.m_btCancel = (base.GetControl("Button_cancel") as Button);
		this.m_btCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCancel));
		base.SetScreenCenter();
	}

	public void SetCouponText(string str)
	{
		this.m_tfInput.Text = str;
	}

	public void SetChatType()
	{
		this.m_eType = CouponDlg.TYPE.SEND_CHAT;
	}

	public void ClickOK(IUIObject obj)
	{
		if (string.Empty.Equals(this.m_tfInput.Text))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("85"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (this.m_eType == CouponDlg.TYPE.SEND_SERVER)
		{
			GS_COUPON_USE_REQ gS_COUPON_USE_REQ = new GS_COUPON_USE_REQ();
			TKString.StringChar(this.m_tfInput.Text, ref gS_COUPON_USE_REQ.strCouponCode);
			SendPacket.GetInstance().SendObject(1648, gS_COUPON_USE_REQ);
		}
		else
		{
			StoryChatSetDlg storyChatSetDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_SET_DLG) as StoryChatSetDlg;
			if (storyChatSetDlg != null)
			{
				storyChatSetDlg.SetInputText(NrLinkText.CouponName(this.m_tfInput.Text));
			}
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COUPON_DLG);
	}

	public void ClickCancel(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COUPON_DLG);
	}
}
