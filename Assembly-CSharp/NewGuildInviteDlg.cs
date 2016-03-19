using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityForms;

public class NewGuildInviteDlg : Form
{
	private TextField m_tfUserName;

	private Button m_btJoinOK;

	private Button m_btCancel;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "NewGuild/DLG_NewGuild_Invite", G_ID.NEWGUILD_INVITE_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_tfUserName = (base.GetControl("TextField_charname") as TextField);
		this.m_tfUserName.ClearText();
		this.m_btJoinOK = (base.GetControl("Button_add") as Button);
		Button expr_3D = this.m_btJoinOK;
		expr_3D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3D.Click, new EZValueChangedDelegate(this.ClickOk));
		this.m_btCancel = (base.GetControl("Button_cancel") as Button);
		Button expr_7A = this.m_btCancel;
		expr_7A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_7A.Click, new EZValueChangedDelegate(this.ClickCancel));
		base.SetScreenCenter();
	}

	public void ClickOk(IUIObject obj)
	{
		string text = this.m_tfUserName.GetText();
		if (string.Empty != text)
		{
			GS_NEWGUILD_INVITE_REQ gS_NEWGUILD_INVITE_REQ = new GS_NEWGUILD_INVITE_REQ();
			TKString.StringChar(text, ref gS_NEWGUILD_INVITE_REQ.strCharName);
			SendPacket.GetInstance().SendObject(1827, gS_NEWGUILD_INVITE_REQ);
		}
		base.CloseNow();
	}

	public void ClickCancel(IUIObject obj)
	{
		base.CloseNow();
	}
}
