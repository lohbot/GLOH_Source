using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityEngine;
using UnityForms;

public class GameGuideAddFriend : GameGuideInfo
{
	private INVITE_PERSONINFO pInvite_personinfo = new INVITE_PERSONINFO();

	private bool find;

	private bool addGuide;

	public override void Init()
	{
		this.pInvite_personinfo.Init();
		base.Init();
	}

	public void SetInvite(INVITE_PERSONINFO info)
	{
		this.pInvite_personinfo = info;
		this.find = true;
	}

	public void REQ_INVITE_FRIEND()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		int limitFriendCount = BASE_FRIENDCOUNTLIMIT_DATA.GetLimitFriendCount((short)kMyCharInfo.GetLevel());
		if (kMyCharInfo.m_kFriendInfo.GetFriendCount() >= limitFriendCount)
		{
			return;
		}
		if (Friend_InvitePersonManager.Get_Instance().IsAble_InvitePerson(kMyCharInfo.GetLevel(), kMyCharInfo.m_kFriendInfo.GetFriendCount()))
		{
			GS_GUIDE_INVITE_FRIEND_REQ gS_GUIDE_INVITE_FRIEND_REQ = new GS_GUIDE_INVITE_FRIEND_REQ();
			gS_GUIDE_INVITE_FRIEND_REQ.ui8ReqType = 0;
			SendPacket.GetInstance().SendObject(912, gS_GUIDE_INVITE_FRIEND_REQ);
		}
	}

	public override void InitData()
	{
		this.m_nCheckTime = Time.realtimeSinceStartup;
		this.pInvite_personinfo.Init();
		this.find = false;
		this.addGuide = false;
	}

	public override void ExcuteGameGuide()
	{
		Friend_Invite_Info_DLG friend_Invite_Info_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.FRIEND_INVITE_INFO_DLG) as Friend_Invite_Info_DLG;
		if (friend_Invite_Info_DLG != null)
		{
			friend_Invite_Info_DLG.SetInfo(this.pInvite_personinfo);
		}
		this.InitData();
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		return true;
	}

	public override bool CheckGameGuide()
	{
		if (Time.realtimeSinceStartup - this.m_nCheckTime > this.m_nDelayTime)
		{
			if (!this.find)
			{
				this.REQ_INVITE_FRIEND();
			}
			this.m_nCheckTime = Time.realtimeSinceStartup;
		}
		if (this.find && !this.addGuide && this.pInvite_personinfo.Invite_PersonID != 0L)
		{
			this.addGuide = true;
			return true;
		}
		return false;
	}

	public override string GetGameGuideText()
	{
		string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
		if (this.pInvite_personinfo.Invite_PersonID != 0L)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromToolTip, new object[]
			{
				textFromToolTip,
				"targetname",
				this.pInvite_personinfo.Invite_UserName
			});
		}
		return textFromToolTip;
	}
}
