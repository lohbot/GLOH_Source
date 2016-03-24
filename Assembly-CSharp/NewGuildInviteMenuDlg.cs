using System;
using UnityForms;

public class NewGuildInviteMenuDlg : Form
{
	private Button m_btChating;

	private Button m_btManual;

	private Button m_btBoard;

	private Button m_btCancel;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "NewGuild/dlg_newguild_invitemenu", G_ID.NEWGUILD_INVITE_MENU_DLG, true);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btChating = (base.GetControl("BT_Chating") as Button);
		Button expr_1C = this.m_btChating;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.ClickChating));
		this.m_btManual = (base.GetControl("BT_Manual") as Button);
		Button expr_59 = this.m_btManual;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.ClickManual));
		this.m_btBoard = (base.GetControl("BT_Board") as Button);
		Button expr_96 = this.m_btBoard;
		expr_96.Click = (EZValueChangedDelegate)Delegate.Combine(expr_96.Click, new EZValueChangedDelegate(this.ClickBoard));
		this.m_btBoard.Hide(true);
		this.m_btCancel = (base.GetControl("BT_cancel") as Button);
		Button expr_DF = this.m_btCancel;
		expr_DF.Click = (EZValueChangedDelegate)Delegate.Combine(expr_DF.Click, new EZValueChangedDelegate(this.ClickCancel));
	}

	public void ClickChating(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_INVITE_INPUT_DLG);
		base.CloseNow();
	}

	public void ClickManual(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_INVITE_DLG);
		base.CloseNow();
	}

	public void ClickBoard(IUIObject obj)
	{
	}

	public void ClickCancel(IUIObject obj)
	{
		this.Close();
	}
}
