using System;
using UnityForms;

public class MineGuildPushConfirmDlg : Form
{
	private Button m_btOk;

	private Button m_btCancel;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Mine/DLG_MineGuildPushConfirm", G_ID.MINE_GUILDPUSH_CONFIRM_DLG, false, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btOk = (base.GetControl("Button_ok") as Button);
		Button expr_1C = this.m_btOk;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.BtClickGuildPush));
		this.m_btCancel = (base.GetControl("Button_cancel") as Button);
		Button expr_59 = this.m_btCancel;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.BtClickClose));
	}

	public void BtClickGuildPush(IUIObject obj)
	{
		PostDlg postDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POST_DLG) as PostDlg;
		if (postDlg != null)
		{
			postDlg.SetSendState(PostDlg.eSEND_STATE.eSEND_STATE_NEWGUILD);
		}
		this.Close();
	}

	public void BtClickClose(IUIObject obj)
	{
		this.Close();
	}
}
