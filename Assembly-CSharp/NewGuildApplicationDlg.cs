using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityForms;

public class NewGuildApplicationDlg : Form
{
	private Label m_lbText;

	private Button m_btOK;

	private Button m_btCancel;

	private string m_strGuildName = string.Empty;

	private long m_lGuildID;

	private long m_lBeforeApplicantGuildID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_NewGuild_Application", G_ID.NEWGUILD_APPLICATION_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_lbText = (base.GetControl("Label_GuildNametext") as Label);
		this.m_btOK = (base.GetControl("Button_OK") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		this.m_btCancel = (base.GetControl("Button_Cencel") as Button);
		this.m_btCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCancel));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		this.Hide();
	}

	public void ClickOK(IUIObject obj)
	{
		if (0L < this.m_lBeforeApplicantGuildID)
		{
			GS_NEWGUILD_MEMBER_FORCE_APPLY_REQ gS_NEWGUILD_MEMBER_FORCE_APPLY_REQ = new GS_NEWGUILD_MEMBER_FORCE_APPLY_REQ();
			gS_NEWGUILD_MEMBER_FORCE_APPLY_REQ.i64BeforeGuildID = this.m_lBeforeApplicantGuildID;
			gS_NEWGUILD_MEMBER_FORCE_APPLY_REQ.i64GuildID = this.m_lGuildID;
			SendPacket.GetInstance().SendObject(1808, gS_NEWGUILD_MEMBER_FORCE_APPLY_REQ);
		}
		else
		{
			GS_NEWGUILD_MEMBER_APPLY_REQ gS_NEWGUILD_MEMBER_APPLY_REQ = new GS_NEWGUILD_MEMBER_APPLY_REQ();
			gS_NEWGUILD_MEMBER_APPLY_REQ.i64GuildID = this.m_lGuildID;
			SendPacket.GetInstance().SendObject(1806, gS_NEWGUILD_MEMBER_APPLY_REQ);
		}
		base.CloseNow();
	}

	public void ClickCancel(IUIObject obj)
	{
		base.CloseNow();
	}

	public void SetGuildInfo(string strGuildName, long lGuildID)
	{
		this.m_strGuildName = strGuildName;
		this.m_lGuildID = lGuildID;
	}

	public void SetApplicatInfo(string strBeforeApplicantGuildName, long lBeforeApplicantGuildID)
	{
		this.m_lBeforeApplicantGuildID = lBeforeApplicantGuildID;
		string text = string.Empty;
		if (string.Empty != strBeforeApplicantGuildName)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("135");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text,
				"targetname1",
				strBeforeApplicantGuildName,
				"targetname2",
				this.m_strGuildName
			});
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1805");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text,
				"targetname",
				this.m_strGuildName
			});
		}
		this.m_lbText.SetText(text);
		this.Show();
	}

	public string GetGuildName()
	{
		return this.m_strGuildName;
	}
}
