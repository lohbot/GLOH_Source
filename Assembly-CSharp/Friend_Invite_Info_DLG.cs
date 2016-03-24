using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using System.Text;
using TsBundle;
using UnityForms;

public class Friend_Invite_Info_DLG : Form
{
	private INVITE_PERSONINFO pInvite_personinfo = new INVITE_PERSONINFO();

	private Label m_lbTitle;

	private Label m_lbIntroMsg;

	private Label m_lbFaceharKindName;

	private Label m_lbInviteFriendExp;

	private ItemTexture m_ItFaceChar_Image;

	private DrawTexture m_dtMineChar_Image;

	private Button m_btFriendInvite;

	private Button m_btFriendCancel;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Community/DLG_FRIEND_RECOMMEND", G_ID.FRIEND_INVITE_INFO_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("LB_Title") as Label);
		this.m_lbIntroMsg = (base.GetControl("LB_Greeting") as Label);
		this.m_lbInviteFriendExp = (base.GetControl("LB_MineText") as Label);
		this.m_lbFaceharKindName = (base.GetControl("LB_SubTitle01") as Label);
		this.m_ItFaceChar_Image = (base.GetControl("IT_SolImage") as ItemTexture);
		this.m_dtMineChar_Image = (base.GetControl("DT_MineImage") as DrawTexture);
		this.m_btFriendInvite = (base.GetControl("BT_Accept") as Button);
		this.m_btFriendInvite.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickInviteFriend));
		this.m_btFriendCancel = (base.GetControl("BT_Refusal") as Button);
		this.m_btFriendCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtCancelInvite));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		this.Hide();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COMMUNITY", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		base.SetScreenCenter();
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COMMUNITY", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void SetInfo(INVITE_PERSONINFO Info)
	{
		this.pInvite_personinfo.Set(Info);
		this.ShowInfo();
	}

	public void ShowInfo()
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("92");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"targetname",
			this.pInvite_personinfo.Invite_UserName,
			"count",
			this.pInvite_personinfo.Invite_PersonLevel.ToString()
		});
		this.m_lbTitle.SetText(empty);
		if (this.pInvite_personinfo.Invite_User_InfoMsg == string.Empty)
		{
			textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("124");
			empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"username",
				this.pInvite_personinfo.Invite_UserName
			});
			this.m_lbIntroMsg.SetText(empty);
		}
		else
		{
			this.m_lbIntroMsg.SetText(this.pInvite_personinfo.Invite_User_InfoMsg);
		}
		this.m_lbInviteFriendExp.Text = this.GetInvitePerson_Explain(this.pInvite_personinfo.eInvte_type);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.pInvite_personinfo.Invite_PersonFaceCharKind);
		if (charKindInfo == null)
		{
			return;
		}
		if (NrTSingleton<NrCharKindInfoManager>.Instance.IsUserCharKind(charKindInfo.GetCharKind()))
		{
			this.m_lbFaceharKindName.Text = this.pInvite_personinfo.Invite_UserName;
		}
		else
		{
			this.m_lbFaceharKindName.Text = charKindInfo.GetName();
		}
		this.m_ItFaceChar_Image.SetSolImageTexure(eCharImageType.LARGE, charKindInfo.GetCharKind(), -1);
		this.m_dtMineChar_Image.SetTexture(eCharImageType.SMALL, 242, -1, string.Empty);
		this.Show();
	}

	private string GetInvitePerson_Explain(eFRIEND_INVITETYPE _invite_type)
	{
		string result = string.Empty;
		string text = string.Empty;
		switch (_invite_type)
		{
		case eFRIEND_INVITETYPE.eINVITETYPE_FRIENDOFFRIEND:
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("145");
			stringBuilder.Append(text);
			stringBuilder.Append("\n");
			List<INIVITEPERSON_FRIENDINFO> list_InvitePerson_FriendList = this.pInvite_personinfo.list_InvitePerson_FriendList;
			for (int i = 0; i < list_InvitePerson_FriendList.Count; i++)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("146");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					text,
					"username",
					TKString.NEWString(list_InvitePerson_FriendList[i].InviteName)
				});
				stringBuilder.Append(text);
				stringBuilder.Append("\n");
			}
			result = stringBuilder.ToString();
			break;
		}
		case eFRIEND_INVITETYPE.eINVITETYPE_SAMELEVEL:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("148");
			break;
		case eFRIEND_INVITETYPE.eINVITETYPE_SIMILARLEVEL:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("150");
			break;
		case eFRIEND_INVITETYPE.eINVITETYPE_RND:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("151");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref result, new object[]
			{
				text,
				"username",
				this.pInvite_personinfo.Invite_UserName
			});
			break;
		}
		return result;
	}

	private void BtClickInviteFriend(IUIObject obj)
	{
		GS_FRIEND_APPLY_REQ gS_FRIEND_APPLY_REQ = new GS_FRIEND_APPLY_REQ();
		gS_FRIEND_APPLY_REQ.i32WorldID = 0;
		TKString.StringChar(this.pInvite_personinfo.Invite_UserName, ref gS_FRIEND_APPLY_REQ.name);
		SendPacket.GetInstance().SendObject(904, gS_FRIEND_APPLY_REQ);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("23"),
			"Charname",
			this.pInvite_personinfo.Invite_UserName
		});
		Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COMMUNITY", "ACCEPT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.Close();
	}

	private void BtCancelInvite(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COMMUNITY", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.Close();
	}
}
