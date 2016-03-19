using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class FriendPush_DLG : Form
{
	private TextField m_UserName;

	private TextArea m_Comment;

	private Button m_btnSend;

	private Button m_btnFriendList;

	private NrMyCharInfo m_pkMyChar;

	private long m_nFriendPersonID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		form.AlwaysUpdate = true;
		instance.LoadFileAll(ref form, "Community/DLG_FriendPush", G_ID.FRIEND_PUSH_DLG, true);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_UserName = (base.GetControl("TextField_InputName") as TextField);
		this.m_UserName.Text = string.Empty;
		TextField expr_2C = this.m_UserName;
		expr_2C.CommitDelegate = (EZKeyboardCommitDelegate)Delegate.Combine(expr_2C.CommitDelegate, new EZKeyboardCommitDelegate(this.OnInputText));
		this.m_Comment = (base.GetControl("TextArea_InputText") as TextArea);
		this.m_btnSend = (base.GetControl("Button_Send") as Button);
		this.m_btnSend.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnSendPush));
		this.m_btnFriendList = (base.GetControl("Button_friendlist") as Button);
		this.m_btnFriendList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnFriendList));
		this.m_pkMyChar = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
	}

	public void ConfirmRequestByName(string strName)
	{
		TsLog.Log("ConfirmRequestByName : {0}", new object[]
		{
			strName
		});
		this.m_UserName.ClearDefaultText(this.m_UserName);
		this.m_UserName.Text = strName;
		this.CheckCharName();
	}

	private void OnInputText(IKeyFocusable obj)
	{
		this.CheckCharName();
	}

	private void CheckCharName()
	{
		if (this.m_UserName.Text.Length <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("98"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (this.m_UserName.Text == nrCharUser.GetCharName())
		{
			string title = string.Empty;
			string message = string.Empty;
			title = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1111");
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("122");
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(null, null, title, message, eMsgType.MB_OK);
			return;
		}
		bool flag = false;
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in this.m_pkMyChar.m_kFriendInfo.GetFriendInfoValues())
		{
			string text = TKString.NEWString(uSER_FRIEND_INFO.szName);
			if (text.Equals(this.m_UserName.Text))
			{
				flag = true;
				this.m_nFriendPersonID = uSER_FRIEND_INFO.nPersonID;
				break;
			}
		}
		if (!flag)
		{
			TsLog.Log("CheckCharName : {0}", new object[]
			{
				this.m_UserName.Text
			});
			this.m_UserName.Text = string.Empty;
			this.m_nFriendPersonID = 0L;
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("98"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
	}

	private void OnSendPush(IUIObject obj)
	{
		if (this.m_nFriendPersonID != 0L)
		{
			GS_FRIEND_PUSH_REQ gS_FRIEND_PUSH_REQ = new GS_FRIEND_PUSH_REQ();
			gS_FRIEND_PUSH_REQ.i64PersonID = this.m_pkMyChar.m_PersonID;
			gS_FRIEND_PUSH_REQ.i64FriendPersonID = this.m_nFriendPersonID;
			TKString.StringChar(this.m_Comment.Text, ref gS_FRIEND_PUSH_REQ.szChatStr);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_PUSH_REQ, gS_FRIEND_PUSH_REQ);
		}
	}

	private void OnCancle(IUIObject obj)
	{
	}

	private void OnFriendList(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POSTFRIEND_DLG) == null)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POSTFRIEND_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.POSTFRIEND_DLG);
		}
	}
}
