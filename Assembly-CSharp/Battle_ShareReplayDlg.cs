using System;
using UnityForms;

public class Battle_ShareReplayDlg : Form
{
	private CheckBox m_kShareChat;

	private CheckBox m_kShareStoryChat;

	private Button m_kShare;

	private Button m_btClose;

	private int m_nType;

	private long m_nReplayUnique;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_ShareReplay", G_ID.BATTLE_SHAREREPLAY_DLG, false);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_kShareChat = (base.GetControl("CheckBox_Chat") as CheckBox);
		this.m_kShareChat.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickShareChat));
		this.m_kShareStoryChat = (base.GetControl("CheckBox_StoryChat") as CheckBox);
		this.m_kShareStoryChat.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickShareStoryChat));
		this.m_kShare = (base.GetControl("BT_Upload") as Button);
		this.m_kShare.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickShare));
		this.m_kShare.controlIsEnabled = false;
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
	}

	public void SetReplayInfo(int type, long unique)
	{
		this.m_nType = type;
		this.m_nReplayUnique = unique;
	}

	public void ClickShareChat(IUIObject obj)
	{
		if (this.m_kShareChat.StateNum == 1)
		{
			this.m_kShare.controlIsEnabled = true;
		}
		else if (this.m_kShareChat.StateNum == 0 && this.m_kShareStoryChat.StateNum == 0)
		{
			this.m_kShare.controlIsEnabled = false;
		}
	}

	public void ClickShareStoryChat(IUIObject obj)
	{
		if (this.m_kShareStoryChat.StateNum == 1)
		{
			this.m_kShare.controlIsEnabled = true;
		}
		else if (this.m_kShareChat.StateNum == 0 && this.m_kShareStoryChat.StateNum == 0)
		{
			this.m_kShare.controlIsEnabled = false;
		}
	}

	public void ClickShare(IUIObject obj)
	{
		if (0L >= this.m_nReplayUnique || 0 >= this.m_nType)
		{
			return;
		}
		if (this.m_kShareChat.StateNum == 0 && this.m_kShareStoryChat.StateNum == 0)
		{
			return;
		}
		string text = string.Empty;
		if (this.m_nType == 1)
		{
			text = NrLinkText.PlunDerReplayName(this.m_nReplayUnique);
		}
		else if (this.m_nType == 2)
		{
			text = NrLinkText.ColosseumReplayName(this.m_nReplayUnique);
		}
		else if (this.m_nType == 3)
		{
			text = NrLinkText.InfiBattleReplayName(this.m_nReplayUnique);
		}
		else if (this.m_nType == 4)
		{
			text = NrLinkText.MineReplayName(this.m_nReplayUnique);
		}
		if (string.Empty == text)
		{
			return;
		}
		if (this.m_kShareChat.StateNum == 1)
		{
			ChatMobile_Sub_Dlg chatMobile_Sub_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHAT_MOBILE_SUB_DLG) as ChatMobile_Sub_Dlg;
			if (chatMobile_Sub_Dlg != null)
			{
				chatMobile_Sub_Dlg.Show();
				chatMobile_Sub_Dlg.SetInputText(text);
			}
		}
		if (this.m_kShareStoryChat.StateNum == 1)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.STORYCHAT_DLG);
			StoryChatSetDlg storyChatSetDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.STORYCHAT_SET_DLG) as StoryChatSetDlg;
			if (storyChatSetDlg != null)
			{
				storyChatSetDlg.SetInputText(text);
			}
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERMAIN_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERRECORD_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COLOSSEUMMAIN_DLG);
		this.Close();
	}
}
