using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class NewGuildChangeNameDlg : Form
{
	private TextField m_tfGuildName;

	private Button m_btOK;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_NewGuild_ChangeName", G_ID.NEWGUILD_CHANGENAME_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_tfGuildName = (base.GetControl("TextField_TextField1") as TextField);
		this.m_tfGuildName.ClearText();
		this.m_btOK = (base.GetControl("Button_Button01") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		if (!NrTSingleton<NewGuildManager>.Instance.IsMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			this.m_tfGuildName.Hide(true);
			this.m_btOK.SetEnabled(false);
		}
	}

	public void ClickOK(IUIObject obj)
	{
		if (!this.IsCheckChangeGuildName())
		{
			base.CloseNow();
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("168");
			string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("202");
			msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), null, null, null, textFromMessageBox, textFromMessageBox2, eMsgType.MB_OK_CANCEL);
		}
	}

	public void ClickCancel(IUIObject obj)
	{
		base.CloseNow();
	}

	public bool IsCheckChangeGuildName()
	{
		string text = this.m_tfGuildName.GetText();
		if (text.Length == 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("264"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		if (text.Length > 10)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("262"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		if (UIDataManager.IsFilterSpecialCharacters(text, NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea()))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("79"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		return true;
	}

	public void MsgBoxOKEvent(object EventObject)
	{
		string text = this.m_tfGuildName.GetText();
		GS_NEWGUILD_CHANGE_NAME_REQ gS_NEWGUILD_CHANGE_NAME_REQ = new GS_NEWGUILD_CHANGE_NAME_REQ();
		TKString.StringChar(text, ref gS_NEWGUILD_CHANGE_NAME_REQ.strGuildName);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_CHANGE_NAME_REQ, gS_NEWGUILD_CHANGE_NAME_REQ);
		base.CloseNow();
	}
}
