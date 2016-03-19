using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class StoryChatSetDlg : Form
{
	private TextArea m_Message;

	private Button m_Send;

	private Button m_Cancel;

	private Button m_Coupon;

	private DrawTexture m_dtCoupon;

	private Label m_lbCoupon;

	private Toggle m_MyStory;

	private Toggle m_GuildStory;

	private int m_nType = 1;

	private bool m_Excute;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "StoryChat/DLG_StoryChatSet", G_ID.STORYCHAT_SET_DLG, false, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_Message = (base.GetControl("TextArea_TextArea5") as TextArea);
		this.m_Message.maxLength = 200;
		this.m_Message.MaxWidth = 0f;
		this.m_Message.AddCommitDelegate(new EZKeyboardCommitDelegate(this.OnInputText));
		this.m_Message.SetFocus();
		if (null != this.m_Message.spriteText)
		{
			this.m_Message.spriteText.parseColorTags = false;
			if (null != this.m_Message.spriteTextShadow)
			{
				this.m_Message.spriteTextShadow.parseColorTags = false;
			}
		}
		this.m_Send = (base.GetControl("BT_Enter") as Button);
		this.m_Send.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSend));
		this.m_Cancel = (base.GetControl("BT_Cancel") as Button);
		this.m_Cancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCancel));
		this.m_Coupon = (base.GetControl("BT_GoCoupon") as Button);
		this.m_Coupon.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickInputCoupon));
		this.m_dtCoupon = (base.GetControl("DT_Coupon") as DrawTexture);
		this.m_lbCoupon = (base.GetControl("LB_Coupon") as Label);
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsCouponUse())
		{
			this.m_Coupon.Visible = false;
			this.m_dtCoupon.Visible = false;
			this.m_lbCoupon.Visible = false;
		}
		this.m_MyStory = (base.GetControl("Toggle_storychat1") as Toggle);
		this.m_MyStory.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickType1));
		this.m_MyStory.Value = true;
		this.m_GuildStory = (base.GetControl("Toggle_storychat2") as Toggle);
		if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			this.m_GuildStory.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickType2));
		}
		else
		{
			this.m_GuildStory.controlIsEnabled = false;
		}
		base.SetScreenCenter();
	}

	private void ClickInputCoupon(IUIObject obj)
	{
		CouponDlg couponDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COUPON_DLG) as CouponDlg;
		if (couponDlg != null)
		{
			couponDlg.SetChatType();
		}
	}

	public void SelectStory(int index)
	{
		if (index == 0)
		{
			if (!this.m_MyStory.Value)
			{
				this.m_MyStory.Value = true;
			}
		}
		else if (index == 1 && !this.m_GuildStory.Value && this.m_GuildStory.controlIsEnabled)
		{
			this.m_GuildStory.Value = true;
		}
		this.m_Excute = true;
	}

	public void ClickType1(IUIObject obj)
	{
		Toggle toggle = (Toggle)obj;
		if (null == toggle)
		{
			return;
		}
		if (!toggle.Value)
		{
			return;
		}
		if (toggle.Value)
		{
			this.m_nType = 1;
		}
		if (this.m_Excute)
		{
			StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
			if (storyChatDlg != null)
			{
				storyChatDlg.SelectToolbar(0);
			}
		}
	}

	public void ClickType2(IUIObject obj)
	{
		Toggle toggle = (Toggle)obj;
		if (null == toggle)
		{
			return;
		}
		if (!toggle.Value)
		{
			return;
		}
		if (toggle.Value)
		{
			this.m_nType = 2;
		}
		if (this.m_Excute)
		{
			StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
			if (storyChatDlg != null)
			{
				storyChatDlg.SelectToolbar(1);
			}
		}
	}

	private void OnInputText(IKeyFocusable obj)
	{
	}

	public void ClickSend(IUIObject obj)
	{
		if (0 >= this.m_Message.Text.Length || string.Empty == this.m_Message.Text)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("122"));
			return;
		}
		string text = this.ConvertLinkText(this.m_Message.Text);
		if (text.Contains("[#"))
		{
			text = text.Replace("[#", string.Empty);
		}
		if (text.Length >= 200)
		{
			return;
		}
		GS_STORYCHAT_SET_REQ gS_STORYCHAT_SET_REQ = new GS_STORYCHAT_SET_REQ();
		gS_STORYCHAT_SET_REQ.m_nType = this.m_nType;
		gS_STORYCHAT_SET_REQ.m_nStoryChatID = 0L;
		TKString.StringChar(text, ref gS_STORYCHAT_SET_REQ.szMessage);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHAT_SET_REQ, gS_STORYCHAT_SET_REQ);
		NrTSingleton<FormsManager>.Instance.AddReserveDeleteForm(base.WindowID);
	}

	public void ClickCancel(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.AddReserveDeleteForm(base.WindowID);
	}

	public void SetInputText(string text)
	{
		TextArea expr_06 = this.m_Message;
		expr_06.Text += text;
		this.m_Message.SetFocus();
	}

	public string ConvertLinkText(string str)
	{
		string text = string.Empty;
		if (str.Contains("[PB") && str.Contains("]"))
		{
			text = str.Replace("[PB", "{@W[PB");
			text = text.Replace("]", "]}");
		}
		else if (str.Contains("[CB") && str.Contains("]"))
		{
			text = str.Replace("[CB", "{@C[CB");
			text = text.Replace("]", "]}");
		}
		else if (str.Contains("[CP:") && str.Contains("]"))
		{
			text = str.Replace("[CP:", "{@U[");
			text = text.Replace("]", "]}");
		}
		else if (str.Contains("[IB") && str.Contains("]"))
		{
			text = str.Replace("[IB", "{@F[IB");
			text = text.Replace("]", "]}");
		}
		else
		{
			text = str;
		}
		return text;
	}
}
