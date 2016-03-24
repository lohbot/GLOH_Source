using GameMessage;
using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using SERVICE;
using System;
using UnityEngine;
using UnityForms;

public class NewGuildAdminMenuDlg : Form
{
	private Button m_btBack;

	private DrawTexture m_dtGuildMark;

	private Button m_btRefreshGuildMark;

	private TextArea m_tfGuildMessage;

	private Label m_lbGuildMessageDefault;

	private Button m_btGuildMessage;

	private NewListBox m_nlbApllicant;

	private Button m_btChangeGuildMark;

	private Button m_btGuildInvite;

	private Button m_btGuildDelete;

	private Button m_btPost;

	private TextArea m_tfGuildNotice;

	private Label m_lbGuildNoticeDefault;

	private Button m_btGuildNotice;

	private Label m_lbWatingCount;

	private Label m_lbGuildName;

	private DrawTexture m_dxGuildBG;

	private float m_fRefreshTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_NewGuild_AdminMenu", G_ID.NEWGUILD_ADMINMENU_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_btBack = (base.GetControl("Button_Back") as Button);
		this.m_btBack.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickBack));
		this.m_dtGuildMark = (base.GetControl("DrawTexture_GMark") as DrawTexture);
		this.m_btRefreshGuildMark = (base.GetControl("Button_Refresh") as Button);
		this.m_btRefreshGuildMark.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRefreshGuildMark));
		this.m_tfGuildMessage = (base.GetControl("TextField_GuildNotice") as TextArea);
		this.m_tfGuildMessage.maxLength = 50;
		this.m_tfGuildMessage.AddCommitDelegate(new EZKeyboardCommitDelegate(this.OnInputText));
		this.m_tfGuildMessage.AddFocusDelegate(new UITextField.FocusDelegate(this.OnFocusText));
		this.m_tfGuildMessage.MultiLine = true;
		this.m_lbGuildMessageDefault = (base.GetControl("Label_GuildNoticeDefaultText") as Label);
		this.m_btGuildMessage = (base.GetControl("Button_GuildNotice") as Button);
		this.m_btGuildMessage.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGuildMessage));
		this.m_nlbApllicant = (base.GetControl("NLB_Awaiter") as NewListBox);
		this.m_nlbApllicant.Reserve = false;
		this.m_btChangeGuildMark = (base.GetControl("Button_MarkChange") as Button);
		this.m_btChangeGuildMark.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickChangeMark));
		this.m_btGuildInvite = (base.GetControl("Button_Invite") as Button);
		this.m_btGuildInvite.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickInvite));
		this.m_btGuildDelete = (base.GetControl("Button_Dissolution") as Button);
		this.m_btGuildDelete.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGuildDelete));
		this.m_btPost = (base.GetControl("Button_GuildPost") as Button);
		this.m_btPost.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGuildPost));
		if (NrTSingleton<NewGuildManager>.Instance.IsGuildPost(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			this.m_btPost.SetEnabled(true);
		}
		else
		{
			this.m_btPost.SetEnabled(false);
		}
		this.m_tfGuildNotice = (base.GetControl("TextField_GuildNotice2") as TextArea);
		this.m_tfGuildNotice.maxLength = 50;
		this.m_tfGuildNotice.AddFocusDelegate(new UITextField.FocusDelegate(this.OnFocusText2));
		this.m_tfGuildNotice.MultiLine = true;
		this.m_btGuildNotice = (base.GetControl("Button_GuildNotice2") as Button);
		this.m_btGuildNotice.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGuildNotice));
		this.m_lbGuildNoticeDefault = (base.GetControl("Label_GuildNotice2DefaultText") as Label);
		this.m_lbWatingCount = (base.GetControl("Label_WaitCount") as Label);
		this.m_lbGuildName = (base.GetControl("Label_GuildName") as Label);
		this.m_lbGuildName.SetText(NrTSingleton<NewGuildManager>.Instance.GetGuildName());
		this.m_dxGuildBG = (base.GetControl("DrawTexture_GuildMarkBG") as DrawTexture);
		this.m_dxGuildBG.SetTextureFromBundle("ui/etc/guildimg03");
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		this.WaitCount();
		this.Send_NewGuildInfo();
		if (!NrTSingleton<NewGuildManager>.Instance.IsMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			this.m_btGuildDelete.Hide(true);
		}
		if (!NrTSingleton<NewGuildManager>.Instance.IsDischargeMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			this.m_btChangeGuildMark.Hide(true);
			this.m_btRefreshGuildMark.Hide(true);
		}
		if (!NrTSingleton<NewGuildManager>.Instance.IsInviteMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			this.m_btGuildInvite.SetEnabled(false);
		}
		if (!string.IsNullOrEmpty(NrTSingleton<NewGuildManager>.Instance.GetGuildMessage()))
		{
			this.m_lbGuildMessageDefault.Visible = false;
		}
		if (!string.IsNullOrEmpty(NrTSingleton<NewGuildManager>.Instance.GetGuildNotice()))
		{
			this.m_lbGuildNoticeDefault.Visible = false;
		}
		this.m_tfGuildMessage.SetText(NrTSingleton<NewGuildManager>.Instance.GetGuildMessage());
		this.m_tfGuildNotice.SetText(NrTSingleton<NewGuildManager>.Instance.GetGuildNotice());
	}

	public void ClickRefreshGuildMark(IUIObject obj)
	{
		float num = Time.time - this.m_fRefreshTime;
		if (num > 3f)
		{
			if (NrTSingleton<NewGuildManager>.Instance.GetGuildID() > 0L)
			{
				GS_GETPORTRAIT_INFO_REQ gS_GETPORTRAIT_INFO_REQ = new GS_GETPORTRAIT_INFO_REQ();
				gS_GETPORTRAIT_INFO_REQ.bDataType = 1;
				gS_GETPORTRAIT_INFO_REQ.i64DataID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
				SendPacket.GetInstance().SendObject(1726, gS_GETPORTRAIT_INFO_REQ);
				this.m_fRefreshTime = Time.time;
			}
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"));
		}
	}

	public void Send_NewGuildInfo()
	{
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(0);
		this.SetLoadGuildMark();
	}

	public void RefreshInfo()
	{
		this.m_nlbApllicant.Clear();
		for (int i = 0; i < NrTSingleton<NewGuildManager>.Instance.GetApplicantCount(); i++)
		{
			NewGuildApplicant applicantInfoFromIndex = NrTSingleton<NewGuildManager>.Instance.GetApplicantInfoFromIndex(i);
			if (applicantInfoFromIndex != null)
			{
				NewListItem newListItem = new NewListItem(this.m_nlbApllicant.ColumnNum, true, string.Empty);
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1639");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
				{
					textFromInterface,
					"count",
					applicantInfoFromIndex.GetLevel(),
					"targetname",
					applicantInfoFromIndex.GetCharName()
				});
				newListItem.SetListItemData(0, textFromInterface, null, null, null);
				newListItem.SetListItemData(1, string.Empty, applicantInfoFromIndex, new EZValueChangedDelegate(this.ApplicantDetailInfo), null);
				newListItem.SetListItemData(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("318"), applicantInfoFromIndex, new EZValueChangedDelegate(this.ClickReject), null);
				newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("317"), applicantInfoFromIndex, new EZValueChangedDelegate(this.ClickApproval), null);
				newListItem.Data = applicantInfoFromIndex;
				this.m_nlbApllicant.Add(newListItem);
			}
		}
		this.m_nlbApllicant.RepositionItems();
		this.WaitCount();
	}

	public void ClickGuildMessage(IUIObject obj)
	{
		if (!NrTSingleton<NewGuildManager>.Instance.IsMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			return;
		}
		string text = this.m_tfGuildMessage.GetText();
		if (string.Empty == text)
		{
			return;
		}
		if (text == NrTSingleton<NewGuildManager>.Instance.GetGuildMessage())
		{
			return;
		}
		if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
		{
			text = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
			{
				text
			});
		}
		if (text.Contains("*"))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("797"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		long num = (long)text.Length;
		if (num > 50L)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("721"), SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
			return;
		}
		GS_NEWGUILD_UPDATE_REQ gS_NEWGUILD_UPDATE_REQ = new GS_NEWGUILD_UPDATE_REQ();
		TKString.StringChar(text, ref gS_NEWGUILD_UPDATE_REQ.strGuildMessage);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_UPDATE_REQ, gS_NEWGUILD_UPDATE_REQ);
	}

	public void SetEnable(bool bEnable)
	{
	}

	public void ClickApproval(object EventObject)
	{
		if (null == this.m_nlbApllicant.SelectedItem)
		{
			return;
		}
		NewGuildApplicant applicantInfoFromIndex = NrTSingleton<NewGuildManager>.Instance.GetApplicantInfoFromIndex(this.m_nlbApllicant.SelectedItem.GetIndex());
		if (applicantInfoFromIndex == null)
		{
			return;
		}
		if (!NrTSingleton<NewGuildManager>.Instance.IsAddMember())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("316"), SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
			return;
		}
		GS_NEWGUILD_MEMBER_JOIN_REQ gS_NEWGUILD_MEMBER_JOIN_REQ = new GS_NEWGUILD_MEMBER_JOIN_REQ();
		gS_NEWGUILD_MEMBER_JOIN_REQ.bApprove = true;
		gS_NEWGUILD_MEMBER_JOIN_REQ.i64PersonID_NewMember = applicantInfoFromIndex.GetPersonID();
		gS_NEWGUILD_MEMBER_JOIN_REQ.i16Level = applicantInfoFromIndex.GetLevel();
		TKString.StringChar(applicantInfoFromIndex.GetCharName(), ref gS_NEWGUILD_MEMBER_JOIN_REQ.strName_NewMember);
		SendPacket.GetInstance().SendObject(1811, gS_NEWGUILD_MEMBER_JOIN_REQ);
	}

	public void ClickReject(object EventObject)
	{
		if (null == this.m_nlbApllicant.SelectedItem)
		{
			return;
		}
		NewGuildApplicant applicantInfoFromIndex = NrTSingleton<NewGuildManager>.Instance.GetApplicantInfoFromIndex(this.m_nlbApllicant.SelectedItem.GetIndex());
		if (applicantInfoFromIndex == null)
		{
			return;
		}
		GS_NEWGUILD_MEMBER_JOIN_REQ gS_NEWGUILD_MEMBER_JOIN_REQ = new GS_NEWGUILD_MEMBER_JOIN_REQ();
		gS_NEWGUILD_MEMBER_JOIN_REQ.bApprove = false;
		gS_NEWGUILD_MEMBER_JOIN_REQ.i64PersonID_NewMember = applicantInfoFromIndex.GetPersonID();
		gS_NEWGUILD_MEMBER_JOIN_REQ.i16Level = applicantInfoFromIndex.GetLevel();
		TKString.StringChar(applicantInfoFromIndex.GetCharName(), ref gS_NEWGUILD_MEMBER_JOIN_REQ.strName_NewMember);
		SendPacket.GetInstance().SendObject(1811, gS_NEWGUILD_MEMBER_JOIN_REQ);
	}

	public void ApplicantDetailInfo(IUIObject obj)
	{
		NewGuildApplicant newGuildApplicant = obj.Data as NewGuildApplicant;
		if (newGuildApplicant == null)
		{
			return;
		}
		GS_OTHERCHAR_INFO_PERMIT_REQ gS_OTHERCHAR_INFO_PERMIT_REQ = new GS_OTHERCHAR_INFO_PERMIT_REQ();
		gS_OTHERCHAR_INFO_PERMIT_REQ.nPersonID = newGuildApplicant.GetPersonID();
		TKString.StringChar(newGuildApplicant.GetCharName().Trim(), ref gS_OTHERCHAR_INFO_PERMIT_REQ.szCharName);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_OTHERCHAR_INFO_PERMIT_REQ, gS_OTHERCHAR_INFO_PERMIT_REQ);
	}

	public void ClickChangeMark(IUIObject obj)
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			NrTSingleton<NkClientLogic>.Instance.SetOTPRequestInfo(eOTPRequestType.OTPREQ_GUILDMARK);
		}
	}

	public void SetLoadGuildMark()
	{
		if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(NrTSingleton<NewGuildManager>.Instance.GetGuildID());
			WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebGuildImageCallback), this.m_dtGuildMark);
		}
	}

	private void ReqWebGuildImageCallback(Texture2D txtr, object _param)
	{
		DrawTexture drawTexture = (DrawTexture)_param;
		if (txtr == null)
		{
			drawTexture.SetTexture(NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture());
		}
		else
		{
			drawTexture.SetTexture(txtr);
		}
	}

	public void ClickInvite(IUIObject obj)
	{
		if (NrTSingleton<NewGuildManager>.Instance.IsInviteMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_INVITE_MENU_DLG);
		}
	}

	public void ClickGuildDelete(IUIObject obj)
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("19");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("138");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromMessageBox, new object[]
		{
			textFromMessageBox,
			"targetname",
			NrTSingleton<NewGuildManager>.Instance.GetGuildName()
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), null, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
		}
	}

	public void ClickGuildPost(IUIObject obj)
	{
		PostDlg postDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POST_DLG) as PostDlg;
		if (postDlg != null)
		{
			postDlg.SetSendState(PostDlg.eSEND_STATE.eSEND_STATE_NEWGUILD);
		}
	}

	public void MsgBoxOKEvent(object EventObject)
	{
		if (NrTSingleton<NewGuildManager>.Instance.IsMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			SendPacket.GetInstance().SendObject(1802);
			base.CloseNow();
		}
	}

	public void RefreshGuildMessage(string strGuildMessage)
	{
		this.m_tfGuildMessage.SetText(strGuildMessage);
	}

	public void ClickBack(IUIObject obj)
	{
		GS_NEWGUILD_DETAILINFO_REQ gS_NEWGUILD_DETAILINFO_REQ = new GS_NEWGUILD_DETAILINFO_REQ();
		gS_NEWGUILD_DETAILINFO_REQ.i64GuildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_DETAILINFO_REQ, gS_NEWGUILD_DETAILINFO_REQ);
	}

	public void ClickGuildNotice(IUIObject obj)
	{
		if (!NrTSingleton<NewGuildManager>.Instance.IsMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			return;
		}
		string text = this.m_tfGuildNotice.GetText();
		if (string.Empty == text)
		{
			return;
		}
		if (text == NrTSingleton<NewGuildManager>.Instance.GetGuildMessage())
		{
			return;
		}
		if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
		{
			text = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
			{
				text
			});
		}
		if (text.Contains("*"))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("797"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		long num = (long)text.Length;
		if (num > 50L)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("721"), SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
			return;
		}
		GS_NEWGUILD_NOTICE_SET_REQ gS_NEWGUILD_NOTICE_SET_REQ = new GS_NEWGUILD_NOTICE_SET_REQ();
		TKString.StringChar(text, ref gS_NEWGUILD_NOTICE_SET_REQ.strGuildNotice);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_NOTICE_SET_REQ, gS_NEWGUILD_NOTICE_SET_REQ);
	}

	public void RefreshGuildNotice(string strGuildNotice)
	{
		this.m_tfGuildNotice.SetText(strGuildNotice);
	}

	private void WaitCount()
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3226"),
			"count1",
			NrTSingleton<NewGuildManager>.Instance.GetApplicantCount()
		});
		this.m_lbWatingCount.SetText(empty);
	}

	private void OnInputText(IKeyFocusable obj)
	{
	}

	private void OnFocusText(IKeyFocusable obj)
	{
		this.m_lbGuildMessageDefault.Visible = false;
	}

	private void OnFocusText2(IKeyFocusable obj)
	{
		this.m_lbGuildNoticeDefault.Visible = false;
	}
}
