using GameMessage;
using GameMessage.Private;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.WORLD;
using SERVICE;
using System;
using UnityForms;

public class ChangeNameDlg : Form
{
	private TextField m_UserName;

	private Label m_lbMessage;

	private string m_originName = string.Empty;

	private string m_newName = string.Empty;

	private Button m_btOk;

	private eSERVICE_AREA _eCurrentService = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Login/dlg_changename", G_ID.CHANGENAME_DLG, false, true);
	}

	public override void SetComponent()
	{
		this.m_UserName = (base.GetControl("TextField_TextField1") as TextField);
		this.m_UserName.ClearDefaultText(this.m_UserName);
		this.m_UserName.Text = string.Empty;
		this.m_lbMessage = (base.GetControl("Label_Label01") as Label);
		this.m_lbMessage.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("199"));
		this.m_btOk = (base.GetControl("Button_Button01") as Button);
		this.m_btOk.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOk));
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		this.m_originName = nrCharUser.GetCharName();
		base.SetScreenCenter();
	}

	public void ClickOk(IUIObject obj)
	{
		this.m_newName = this.m_UserName.Text;
		this.m_newName = this.m_newName.Trim();
		if (this.m_newName.Length == 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("51"));
			return;
		}
		if (this.m_newName.Length > 20)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("126"));
			return;
		}
		if (UIDataManager.IsFilterSpecialCharacters(this.m_newName, this._eCurrentService))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("79"));
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(new YesDelegate(this._onOK), this, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("168"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("200"), eMsgType.MB_OK_CANCEL);
		}
	}

	private void _onOK(object arg)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		NrPersonInfoBase personInfo = nrCharUser.GetPersonInfo();
		WS_CHANGE_CHAR_NAME_REQ wS_CHANGE_CHAR_NAME_REQ = new WS_CHANGE_CHAR_NAME_REQ();
		TKString.StringChar(this.m_originName, ref wS_CHANGE_CHAR_NAME_REQ.szCharName);
		TKString.StringChar(this.m_newName, ref wS_CHANGE_CHAR_NAME_REQ.szChangeName);
		wS_CHANGE_CHAR_NAME_REQ.nPersonID = personInfo.GetPersonID();
		SendPacket.GetInstance().SendObject(16777258, wS_CHANGE_CHAR_NAME_REQ);
		this.m_btOk.SetEnabled(false);
	}

	public void ChangeNameAck(long nResult)
	{
		if (nResult == 0L)
		{
			TsLog.Log("이름 변경 성공", new object[0]);
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			NrPersonInfoBase personInfo = nrCharUser.GetPersonInfo();
			personInfo.SetCharName(this.m_newName);
			NrTSingleton<NrMainSystem>.Instance.SetLatestPersonID(personInfo.GetPersonID().ToString());
			MsgHandler.Handle("Req_CONNECT_GAMESERVER_REQ", new object[]
			{
				personInfo.GetPersonID()
			});
			FacadeHandler.MoveStage(Scene.Type.PREPAREGAME);
			NrTSingleton<NkQuestManager>.Instance.SortingQuestInGroup();
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHANGENAME_DLG);
		}
		else if (nResult == -20L)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("90"));
			this.m_btOk.SetEnabled(true);
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("91"));
			this.m_btOk.SetEnabled(true);
		}
	}
}
