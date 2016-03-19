using GAME;
using Ndoors.Memory;
using System;
using UnityEngine;
using UnityForms;

public class COMMUNITY_USER_INFO
{
	public bool bConnect;

	public long i64PersonID;

	public string strName = string.Empty;

	public short i16Level;

	public byte byLocation;

	public int i32MapUnique;

	public int i32WorldID;

	public int i32WorldID_Connect;

	public int i32FaceCharKind;

	public byte ui8HelpSolUse;

	public short i16BattleMatch;

	public byte byUserPlayState;

	public long i64IntimacyExp;

	public byte byFliendPlatformType;

	public byte by3PumTalkState;

	public byte byGuildRank;

	public FRIEND_HELPSOLINFO Friend_HelpSolInfo = new FRIEND_HELPSOLINFO();

	public string strFaceBookID = string.Empty;

	public string strPlatformName = string.Empty;

	public long i64LogoutTime;

	public string strGuildName = string.Empty;

	public short i16ColosseumGrade;

	public byte ui8UserPortrait;

	public Texture2D UserPortrait;

	public void Set(USER_FRIEND_INFO userFriendInfo)
	{
		this.strName = TKString.NEWString(userFriendInfo.szName);
		this.i16Level = userFriendInfo.i16Level;
		this.i32WorldID = userFriendInfo.i32FriendWorldID;
		this.i32WorldID_Connect = userFriendInfo.i32WorldID_Connect;
		this.i64PersonID = userFriendInfo.nPersonID;
		this.i16BattleMatch = userFriendInfo.i16BattleMatch;
		this.bConnect = (userFriendInfo.i8Location > 0);
		this.byLocation = userFriendInfo.i8Location;
		this.i32MapUnique = userFriendInfo.i32MapUnique;
		this.ui8HelpSolUse = userFriendInfo.ui8HelpUse;
		this.i32FaceCharKind = userFriendInfo.i32FaceCharKind;
		this.byUserPlayState = userFriendInfo.i8UserPlayState;
		this.Friend_HelpSolInfo = userFriendInfo.FriendHelpSolInfo;
		this.strFaceBookID = TKString.NEWString(userFriendInfo.szFaceBookID);
		this.strPlatformName = TKString.NEWString(userFriendInfo.szPlatformName);
		this.strPlatformName.Trim();
		this.i64LogoutTime = userFriendInfo.i64LogoutTIme;
		this.strGuildName = TKString.NEWString(userFriendInfo.szGuildName);
		this.i16ColosseumGrade = userFriendInfo.i16ColosseumGrade;
		this.ui8UserPortrait = userFriendInfo.ui8UserPortrait;
		if (userFriendInfo.ui8UserPortrait == 1 && this.i64PersonID > 0L && this.i64PersonID > 11L)
		{
			string userPortraitURL = NrTSingleton<NkCharManager>.Instance.GetUserPortraitURL(this.i64PersonID);
			WebFileCache.RequestImageWebFile(userPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebUserImageCallback), this.UserPortrait);
		}
	}

	public void Update(USER_FRIEND_INFO userFriendInfo)
	{
		this.strName = TKString.NEWString(userFriendInfo.szName);
		this.i16Level = userFriendInfo.i16Level;
		this.i32WorldID = userFriendInfo.i32FriendWorldID;
		this.i32WorldID_Connect = userFriendInfo.i32WorldID_Connect;
		this.i64PersonID = userFriendInfo.nPersonID;
		this.i16BattleMatch = userFriendInfo.i16BattleMatch;
		this.bConnect = (userFriendInfo.i8Location > 0);
		this.byLocation = userFriendInfo.i8Location;
		this.i32MapUnique = userFriendInfo.i32MapUnique;
		this.i32FaceCharKind = userFriendInfo.i32FaceCharKind;
		this.byUserPlayState = userFriendInfo.i8UserPlayState;
		this.strPlatformName = TKString.NEWString(userFriendInfo.szPlatformName);
		this.i64LogoutTime = userFriendInfo.i64LogoutTIme;
		this.strGuildName = TKString.NEWString(userFriendInfo.szGuildName);
		this.i16ColosseumGrade = userFriendInfo.i16ColosseumGrade;
		this.ui8UserPortrait = userFriendInfo.ui8UserPortrait;
		if (userFriendInfo.ui8UserPortrait == 1 && this.i64PersonID > 0L && this.i64PersonID > 11L)
		{
			string userPortraitURL = NrTSingleton<NkCharManager>.Instance.GetUserPortraitURL(this.i64PersonID);
			WebFileCache.RequestImageWebFile(userPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebUserImageCallback), this.UserPortrait);
		}
	}

	public void Set(NewGuildMember _guildmember)
	{
		this.strName = _guildmember.GetCharName();
		this.i16Level = _guildmember.GetLevel();
		this.i32WorldID_Connect = _guildmember.GetConnectedWorldID();
		this.i64PersonID = _guildmember.GetPersonID();
		this.bConnect = (_guildmember.GetMapUnique() > 0);
		this.i32MapUnique = _guildmember.GetMapUnique();
		this.i32FaceCharKind = _guildmember.GetFaceCharKind();
	}

	private void ReqWebUserImageCallback(Texture2D txtr, object _param)
	{
		if (txtr != null)
		{
			this.UserPortrait = txtr;
		}
		else
		{
			this.UserPortrait = null;
		}
		if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.COMMUNITY_DLG))
		{
			CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
			if (communityUI_DLG != null)
			{
				communityUI_DLG.UpdateCommunity_Friend(this);
			}
			if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.DLG_OTHER_CHAR_DETAIL))
			{
				DLG_OtherCharDetailInfo dLG_OtherCharDetailInfo = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_OTHER_CHAR_DETAIL) as DLG_OtherCharDetailInfo;
				if (dLG_OtherCharDetailInfo != null)
				{
					dLG_OtherCharDetailInfo.UpsateSoldierList();
				}
			}
		}
		if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.STORYCHAT_DLG))
		{
			StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
			if (storyChatDlg != null)
			{
				storyChatDlg.UpdateUserCommunity(this.i64PersonID);
			}
		}
	}

	public NkSoldierInfo MyHelpSol()
	{
		return NrTSingleton<NkCharManager>.Instance.GetMyHelpSol(this.i64PersonID);
	}

	public long GetHelpSolExp()
	{
		NkSoldierInfo nkSoldierInfo = this.MyHelpSol();
		if (nkSoldierInfo == null)
		{
			return 0L;
		}
		return nkSoldierInfo.AddHelpExp;
	}

	public bool GetHelpSolUse()
	{
		bool result = false;
		if (this.ui8HelpSolUse >= 1)
		{
			result = true;
		}
		return result;
	}
}
