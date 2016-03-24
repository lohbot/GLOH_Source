using System;
using UnityEngine;
using UnityForms;

public class NewGuildInviteInputDlg : Form
{
	private TextArea m_tfText;

	private Button m_btOK;

	private Button m_btCancel;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "NewGuild/dlg_newguild_inviteinput", G_ID.NEWGUILD_INVITE_INPUT_DLG, true);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btOK = (base.GetControl("BT_OK") as Button);
		Button expr_1C = this.m_btOK;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.ClickOK));
		this.m_btCancel = (base.GetControl("BT_cancel") as Button);
		Button expr_59 = this.m_btCancel;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.ClickCancel));
		this.m_tfText = (base.GetControl("TextField_Text1") as TextArea);
		this.m_tfText.maxLength = 50;
		this.m_tfText.MultiLine = true;
		string @string = PlayerPrefs.GetString(NrPrefsKey.NEWGUILD_INVITE_ADVERTISING);
		this.m_tfText.Text = @string;
	}

	public void ClickOK(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		string text = this.m_tfText.Text;
		PlayerPrefs.SetString(NrPrefsKey.NEWGUILD_INVITE_ADVERTISING, text);
		string first = NrLinkText.GuildName(NrTSingleton<NewGuildManager>.Instance.GetGuildName());
		string @string = NrTSingleton<UIDataManager>.Instance.GetString(first, text);
		NrTSingleton<ChatManager>.Instance.SendMessage(CHAT_TYPE.NORMAL, @string, false, null, 0, 0L, 0);
		NrTSingleton<ChatManager>.Instance.MakeChatText(NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MOBILE_SUB_DLG), CHAT_TYPE.NORMAL, kMyCharInfo.ColosseumGrade, @string, null);
		Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("866"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		NrTSingleton<NewGuildManager>.Instance.ClearDlg();
	}

	public void ClickCancel(IUIObject obj)
	{
		this.Close();
	}
}
