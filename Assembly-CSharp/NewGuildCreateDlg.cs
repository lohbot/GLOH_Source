using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class NewGuildCreateDlg : Form
{
	private Label m_lbText;

	private TextField m_tfGuildName;

	private Button m_btOK;

	private Button m_btCancel;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_NewGuild_Create", G_ID.NEWGUILD_CREATE_DLG, true);
	}

	public override void SetComponent()
	{
		string empty = string.Empty;
		this.m_lbText = (base.GetControl("Label_Msgtext1") as Label);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1789"),
			"count1",
			NrTSingleton<NewGuildManager>.Instance.GetLevelForCreate(),
			"count2",
			NrTSingleton<NewGuildManager>.Instance.GetCreateMoney()
		});
		this.m_lbText.SetText(empty);
		this.m_tfGuildName = (base.GetControl("TextField_GuildNameInput") as TextField);
		this.m_tfGuildName.ClearText();
		this.m_btOK = (base.GetControl("Button_OK") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		this.m_btCancel = (base.GetControl("Button_Cencel") as Button);
		this.m_btCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCancel));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClickOK(IUIObject obj)
	{
		if (!this.IsCheckGuildCreate())
		{
			return;
		}
		string text = this.m_tfGuildName.GetText();
		if (UIDataManager.IsFilterSpecialCharacters(text, NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea()))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("79"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		GS_NEWGUILD_CREATE_REQ gS_NEWGUILD_CREATE_REQ = new GS_NEWGUILD_CREATE_REQ();
		TKString.StringChar(text, ref gS_NEWGUILD_CREATE_REQ.strGuildName);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_CREATE_REQ, gS_NEWGUILD_CREATE_REQ);
	}

	public void ClickCancel(IUIObject obj)
	{
		base.CloseNow();
	}

	public bool IsCheckGuildCreate()
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
		if (NrTSingleton<NewGuildManager>.Instance.GetCreateMoney() > NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		if ((int)NrTSingleton<NewGuildManager>.Instance.GetLevelForCreate() > NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel())
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("88"),
				"count",
				NrTSingleton<NewGuildManager>.Instance.GetLevelForCreate()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		return true;
	}
}
