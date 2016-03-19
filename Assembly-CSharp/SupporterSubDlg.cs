using PROTOCOL;
using PROTOCOL.WORLD;
using System;
using UnityForms;

public class SupporterSubDlg : Form
{
	private TextField m_TextField_NameInput;

	private Button m_Button_OK;

	private Button m_Button_back;

	private string m_strSupporterName = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Community/dlg_supporter_add", G_ID.SUPPORTERSUB_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_TextField_NameInput = (base.GetControl("TextField_NameInput") as TextField);
		this.m_TextField_NameInput.Text = string.Empty;
		this.m_Button_OK = (base.GetControl("Button_OK") as Button);
		this.m_Button_back = (base.GetControl("Button_back") as Button);
		this.m_TextField_NameInput.AddValueChangedDelegate(new EZValueChangedDelegate(this.TextFieldChange));
		this.m_Button_OK.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickOK));
		this.m_Button_back.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickBack));
		base.Draggable = false;
		base.SetScreenCenter();
	}

	private void TextFieldChange(IUIObject obj)
	{
	}

	public void On_ClickOK(IUIObject a_cObject)
	{
		if (this.m_TextField_NameInput.Text.Length <= 0)
		{
			return;
		}
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (this.m_TextField_NameInput.Text == nrCharUser.GetCharName())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("51"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (COMMON_CONSTANT_Manager.GetInstance() == null)
		{
			TsLog.LogWarning("!!!!!!!!!!!!!!!!!!!!! NOT Error Common_Constant", new object[0]);
			return;
		}
		SupporterDlg supporterDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SUPPORTER_DLG) as SupporterDlg;
		if (supporterDlg != null)
		{
			string charName = supporterDlg.GetCharName();
			if (charName != string.Empty)
			{
				WS_SUPPORTER_ADD_REQ wS_SUPPORTER_ADD_REQ = new WS_SUPPORTER_ADD_REQ();
				TKString.StringChar(this.m_TextField_NameInput.Text, ref wS_SUPPORTER_ADD_REQ.szCharName_Target);
				SendPacket.GetInstance().SendObject(16777287, wS_SUPPORTER_ADD_REQ);
			}
		}
	}

	public void On_ClickBack(IUIObject a_cObject)
	{
		if (a_cObject == null)
		{
			return;
		}
		SupporterDlg supporterDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SUPPORTER_DLG) as SupporterDlg;
		if (supporterDlg != null)
		{
			supporterDlg.ShowDrawHide(false);
		}
		base.CloseForm(a_cObject);
	}

	public void SetSUPPORTER_ADD_ACK(WS_SUPPORTER_ADD_ACK Ack)
	{
		if (Ack.i32Result == 0)
		{
			this.m_strSupporterName = TKString.NEWString(Ack.szCharName_Target);
			SupporterDlg supporterDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SUPPORTER_DLG) as SupporterDlg;
			if (supporterDlg != null)
			{
				supporterDlg.SetSupport(this.m_strSupporterName);
			}
			this.OnClose();
		}
		else if (Ack.i32Result == -10)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("42"),
				"targetrname",
				TKString.NEWString(Ack.szCharName_Target)
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
		else if (Ack.i32Result == -20)
		{
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("449"),
				"targetname",
				TKString.NEWString(Ack.szCharName_Target)
			});
			Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
		else
		{
			TsLog.LogWarning("!!!!!!!!!!!! WS_SUPPORTER_ADD_ACK Error {0}", new object[]
			{
				Ack.i32Result
			});
		}
	}

	public override void CloseForm(IUIObject obj)
	{
		SupporterDlg supporterDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SUPPORTER_DLG) as SupporterDlg;
		if (supporterDlg != null)
		{
			supporterDlg.ShowDrawHide(false);
		}
		base.CloseForm(obj);
	}
}
