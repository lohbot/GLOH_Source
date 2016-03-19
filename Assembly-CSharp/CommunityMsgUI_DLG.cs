using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityForms;

public class CommunityMsgUI_DLG : Form
{
	private Label m_LTitle;

	private Button m_BtnAdd;

	private Button m_BtnCancel;

	private Label m_LNote;

	private TextField m_TF_CharName;

	private USER_FRIEND_INFO m_FriendInfo;

	private eMsgMox_Type bMode;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Community/DLG_CommunityMsg", G_ID.COMMUNITYMSG_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_LTitle = (base.GetControl("Label_title") as Label);
		this.m_BtnAdd = (base.GetControl("Button_add") as Button);
		this.m_BtnCancel = (base.GetControl("Button_cancel") as Button);
		this.m_LNote = (base.GetControl("Label_note") as Label);
		this.m_TF_CharName = (base.GetControl("TextField_charname") as TextField);
		Button expr_74 = this.m_BtnAdd;
		expr_74.Click = (EZValueChangedDelegate)Delegate.Combine(expr_74.Click, new EZValueChangedDelegate(this.BtnClick00));
		Button expr_9B = this.m_BtnCancel;
		expr_9B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_9B.Click, new EZValueChangedDelegate(this.BtnClick01));
		this.SetMode(this.bMode);
		float x = (GUICamera.width - base.GetSize().x) / 2f;
		float y = (GUICamera.height - base.GetSize().y) / 2f;
		base.SetLocation(x, y);
	}

	public void SetMode(eMsgMox_Type _Mode)
	{
		this.bMode = _Mode;
		if (_Mode == eMsgMox_Type.eNull)
		{
			this.m_LTitle.Text = string.Empty;
			this.m_LNote.Text = string.Empty;
			this.m_BtnAdd.Text = string.Empty;
			this.m_BtnCancel.Text = string.Empty;
		}
		else if (_Mode == eMsgMox_Type.eCommunity_Friend_Add)
		{
			this.m_LTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("464");
			this.m_LNote.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("7");
			this.m_BtnAdd.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("331");
			this.m_BtnCancel.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11");
		}
		else if (_Mode == eMsgMox_Type.eCommunity_Alliance_Add)
		{
			this.m_LTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("467");
			this.m_LNote.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("332");
			this.m_BtnAdd.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("331");
			this.m_BtnCancel.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11");
		}
	}

	public void SetName(string strName)
	{
		this.m_TF_CharName.Text = strName;
	}

	private void BtnClick00(IUIObject obj)
	{
		if (this.bMode == eMsgMox_Type.eCommunity_Friend_Add)
		{
			if (this.m_TF_CharName.Text == string.Empty)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("51");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
			long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_FRIEND_MAIL_LIMIT);
			long charDetail = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharDetail(22);
			if (num <= charDetail)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("744"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				return;
			}
			GS_FRIEND_APPLY_REQ gS_FRIEND_APPLY_REQ = new GS_FRIEND_APPLY_REQ();
			gS_FRIEND_APPLY_REQ.i32WorldID = 0;
			string text = this.m_TF_CharName.Text;
			TKString.StringChar(text, ref gS_FRIEND_APPLY_REQ.name);
			SendPacket.GetInstance().SendObject(904, gS_FRIEND_APPLY_REQ);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("23"),
				"Charname",
				text
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
		else if (this.bMode == eMsgMox_Type.eCommunity_Alliance_Add)
		{
			GS_DEL_FRIEND_REQ gS_DEL_FRIEND_REQ = new GS_DEL_FRIEND_REQ();
			if (this.m_FriendInfo != null)
			{
				gS_DEL_FRIEND_REQ.i64FriendPersonID = this.m_FriendInfo.nPersonID;
				SendPacket.GetInstance().SendObject(908, gS_DEL_FRIEND_REQ);
			}
		}
		this.Close();
	}

	private void BtnClick01(IUIObject obj)
	{
		this.Close();
	}
}
