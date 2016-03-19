using PROTOCOL;
using PROTOCOL.GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Community_FriendMenu_DLG : Form
{
	private const int BAND_UI_LAYER = 2;

	private const int OTHER_UI_LAYER = 1;

	public Button m_btFriendRecommend;

	public Button m_btFriendAdd;

	public Button m_btFaceBookFriendAdd;

	public DrawTexture m_dtKakaoTalkImg;

	public Button m_btClose;

	private CommunityMsgUI_DLG m_clMsg;

	private bool m_bInviteFriend;

	private float fStartTime_InviteFriend;

	private float m_ShowTime_InviteFriendBtn = 5f;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Community/dlg_friendadd", G_ID.COMMUNITY_FRIENDMENU_DLG, true);
	}

	public override void SetComponent()
	{
		if (TsPlatform.IsBand || TsPlatform.IsKakao)
		{
			this.m_btFriendRecommend = (base.GetControl("FriendRecommend_Btn1") as Button);
			this.m_btFriendRecommend.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickRecommend));
			this.m_btFriendAdd = (base.GetControl("FriendAdd_Btn1") as Button);
			this.m_btFriendAdd.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickAdd));
			base.SetShowLayer(1, false);
			base.SetShowLayer(2, true);
		}
		else
		{
			this.m_btFriendRecommend = (base.GetControl("FriendRecommend_Btn") as Button);
			this.m_btFriendRecommend.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickRecommend));
			this.m_btFriendAdd = (base.GetControl("FriendAdd_Btn") as Button);
			this.m_btFriendAdd.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickAdd));
			this.m_btFaceBookFriendAdd = (base.GetControl("Facebook_Btn1") as Button);
			this.m_btFaceBookFriendAdd.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickFaceBookAdd));
			base.SetShowLayer(2, false);
			base.SetShowLayer(1, true);
		}
		this.m_btClose = (base.GetControl("Button_cancel") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickClose));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
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

	public override void Update()
	{
		if (this.m_bInviteFriend && Time.realtimeSinceStartup - this.fStartTime_InviteFriend > this.m_ShowTime_InviteFriendBtn)
		{
			this.m_bInviteFriend = false;
			this.fStartTime_InviteFriend = 0f;
			this.m_btFriendRecommend.SetEnabled(true);
		}
	}

	public void BtClickRecommend(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		int limitFriendCount = BASE_FRIENDCOUNTLIMIT_DATA.GetLimitFriendCount((short)kMyCharInfo.GetLevel());
		if (kMyCharInfo.m_kFriendInfo.GetFriendCount() >= limitFriendCount)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("630");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		this.fStartTime_InviteFriend = Time.realtimeSinceStartup;
		this.m_bInviteFriend = true;
		this.m_btFriendRecommend.SetEnabled(false);
		GS_GUIDE_INVITE_FRIEND_REQ gS_GUIDE_INVITE_FRIEND_REQ = new GS_GUIDE_INVITE_FRIEND_REQ();
		gS_GUIDE_INVITE_FRIEND_REQ.ui8ReqType = 1;
		SendPacket.GetInstance().SendObject(912, gS_GUIDE_INVITE_FRIEND_REQ);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COMMUNITY_DLG);
		this.Close();
	}

	public void BtClickAdd(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		int limitFriendCount = BASE_FRIENDCOUNTLIMIT_DATA.GetLimitFriendCount((short)kMyCharInfo.GetLevel());
		if (kMyCharInfo.m_kFriendInfo.GetFriendCount() >= limitFriendCount)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("630");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		this.m_clMsg = (CommunityMsgUI_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COMMUNITYMSG_DLG);
		this.m_clMsg.SetMode(eMsgMox_Type.eCommunity_Friend_Add);
	}

	public void BtClickFaceBookAdd(IUIObject obj)
	{
		if (!TsPlatform.IsMobile || TsPlatform.IsEditor)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		int limitFriendCount = BASE_FRIENDCOUNTLIMIT_DATA.GetLimitFriendCount((short)kMyCharInfo.GetLevel());
		if (kMyCharInfo.m_kFriendInfo.GetFriendCount() >= limitFriendCount)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("630");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int @int = PlayerPrefs.GetInt("FacebookGetFriend:" + kMyCharInfo.m_PersonID.ToString());
		TsLog.LogWarning("FacebookGetFriend:{0}={1}", new object[]
		{
			kMyCharInfo.m_PersonID,
			@int
		});
		if (@int == 0)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1019");
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this._OnMessageBoxOK), null, new NoDelegate(this._OnMessageBoxCancel), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1018"), textFromInterface, eMsgType.MB_OK_CANCEL);
			msgBoxUI.Show();
		}
		else
		{
			NmFacebookManager.instance.InviteUser();
		}
	}

	public void _OnMessageBoxOK(object a_oObject)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		PlayerPrefs.SetInt("FacebookGetFriend:" + kMyCharInfo.m_PersonID.ToString(), 1);
		NmFacebookManager.instance.AutoInviteUser();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MSGBOX_DLG);
	}

	public void _OnMessageBoxCancel(object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MSGBOX_DLG);
	}

	public void BtClicFaceBookMsg(IUIObject obj)
	{
	}

	public void BtClickClose(IUIObject obj)
	{
		this.Close();
	}
}
