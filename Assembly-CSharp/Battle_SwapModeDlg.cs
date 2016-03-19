using System;
using UnityForms;

public class Battle_SwapModeDlg : Form
{
	private Button m_btCharinfo;

	private Button m_btChat;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_SwapMode", G_ID.BATTLE_SWAPMODE_DLG, false);
		if (base.InteractivePanel != null)
		{
			base.Draggable = false;
		}
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_btCharinfo = (base.GetControl("btn_battle_charinfo") as Button);
		Button expr_1C = this.m_btCharinfo;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickCharinfoMode));
		this.m_btCharinfo.Visible = false;
		this.m_btChat = (base.GetControl("btn_battle_chat") as Button);
		Button expr_65 = this.m_btChat;
		expr_65.Click = (EZValueChangedDelegate)Delegate.Combine(expr_65.Click, new EZValueChangedDelegate(this.OnClickChatMode));
		base.SetLocation(0f, 0f);
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		base.SetLocation(0f, 0f);
	}

	public override void OnClose()
	{
	}

	public void OnClickCharinfoMode(IUIObject obj)
	{
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CHARINFO_DLG);
		if (form != null)
		{
			form.Show();
		}
		if (!TsPlatform.IsMobile)
		{
			form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
			if (form != null)
			{
				form.Hide();
			}
		}
		else
		{
			form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
			if (form != null)
			{
				form.Hide();
			}
		}
		this.m_btCharinfo.Visible = false;
		this.m_btChat.Visible = true;
	}

	public void OnClickChatMode(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.CHAT_MOBILE_SUB_DLG);
	}

	public void ShowChange()
	{
		if (this.m_btChat.Visible)
		{
			this.OnClickChatMode(this.m_btChat);
		}
		else if (this.m_btCharinfo.Visible)
		{
			if (!TsPlatform.IsMobile)
			{
				MainChatDlg mainChatDlg = (MainChatDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
				if (mainChatDlg != null && mainChatDlg.IsChatFocused())
				{
					return;
				}
			}
			else
			{
				ChatMobile_Sub_Dlg chatMobile_Sub_Dlg = (ChatMobile_Sub_Dlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MOBILE_SUB_DLG);
				if (chatMobile_Sub_Dlg != null && chatMobile_Sub_Dlg.IsChatFocused())
				{
					return;
				}
			}
			this.OnClickCharinfoMode(this.m_btCharinfo);
		}
	}
}
