using GAME;
using Ndoors.Memory;
using PROTOCOL;
using System;
using UnityEngine;

public class NewGuildMember
{
	private long m_lPersonID;

	private long m_lGuildID;

	private short m_iLevel;

	private NewGuildDefine.eNEWGUILD_MEMBER_RANK m_eRank;

	private long m_lJoinDate;

	private long m_lLoginDate;

	private short m_iChannelID;

	private int m_iMapUnique;

	private int m_iConnectedWorldID;

	private int m_iFaceCharKind;

	private string m_strCharName = string.Empty;

	private int m_iContribute;

	private long m_lLogOffTime;

	private Texture2D m_GuildMemberPortrait;

	private long m_lSN;

	private bool m_bConnected;

	public NewGuildMember(NEWGUILDMEMBER_INFO NewGuildMemberInfo)
	{
		this.m_lPersonID = NewGuildMemberInfo.i64PersonID;
		this.m_lGuildID = NewGuildMemberInfo.i64GuildID;
		this.m_iLevel = NewGuildMemberInfo.i16Level;
		this.m_eRank = (NewGuildDefine.eNEWGUILD_MEMBER_RANK)NewGuildMemberInfo.ui8Rank;
		this.m_lJoinDate = NewGuildMemberInfo.i64JoinDate;
		this.m_lLoginDate = NewGuildMemberInfo.i64LoginDate;
		this.m_iChannelID = NewGuildMemberInfo.i16ChannelID;
		this.m_iMapUnique = NewGuildMemberInfo.i32MapUnique;
		this.m_iConnectedWorldID = NewGuildMemberInfo.i32ConnectedWorldID;
		this.m_iFaceCharKind = NewGuildMemberInfo.i32FaceCharKind;
		this.m_strCharName = TKString.NEWString(NewGuildMemberInfo.strCharName);
		this.m_iContribute = NewGuildMemberInfo.i32Contribute;
		this.m_lLogOffTime = NewGuildMemberInfo.i64LogoffTime;
		this.SetPortrait();
		this.m_lSN = NewGuildMemberInfo.i64SN;
		this.m_bConnected = NewGuildMemberInfo.bConnected;
	}

	private void Clear()
	{
		this.m_lPersonID = 0L;
		this.m_lGuildID = 0L;
		this.m_iLevel = 0;
		this.m_eRank = NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_NONE;
		this.m_lJoinDate = 0L;
		this.m_lLoginDate = 0L;
		this.m_iChannelID = 0;
		this.m_iMapUnique = 0;
		this.m_iConnectedWorldID = 0;
		this.m_iFaceCharKind = 0;
		this.m_strCharName = string.Empty;
		this.m_iContribute = 0;
		this.m_lLogOffTime = 0L;
		this.m_GuildMemberPortrait = null;
	}

	public long GetPersonID()
	{
		return this.m_lPersonID;
	}

	public long GetGuildID()
	{
		return this.m_lGuildID;
	}

	public short GetLevel()
	{
		return this.m_iLevel;
	}

	public NewGuildDefine.eNEWGUILD_MEMBER_RANK GetRank()
	{
		return this.m_eRank;
	}

	public void SetRank(NewGuildDefine.eNEWGUILD_MEMBER_RANK eRank)
	{
		this.m_eRank = eRank;
	}

	public long GetJoinDate()
	{
		return this.m_lJoinDate;
	}

	public long GetLoginDate()
	{
		return this.m_lLoginDate;
	}

	public short GetChannelID()
	{
		return this.m_iChannelID;
	}

	public int GetMapUnique()
	{
		return this.m_iMapUnique;
	}

	public int GetConnectedWorldID()
	{
		return this.m_iConnectedWorldID;
	}

	public int GetFaceCharKind()
	{
		return this.m_iFaceCharKind;
	}

	public string GetCharName()
	{
		return this.m_strCharName;
	}

	public string GetRankText()
	{
		return NrTSingleton<NewGuildManager>.Instance.GetRankText(this.m_eRank);
	}

	public int GetContribute()
	{
		return this.m_iContribute;
	}

	public long GetLogOffTime()
	{
		return this.m_lLogOffTime;
	}

	public Texture2D GetPortrait()
	{
		return this.m_GuildMemberPortrait;
	}

	public void SetPortrait()
	{
		if (this.m_lPersonID > 0L && this.m_lPersonID > 11L)
		{
			string userPortraitURL = NrTSingleton<NkCharManager>.Instance.GetUserPortraitURL(this.m_lPersonID);
			WebFileCache.RequestImageWebFile(userPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebGuildImageCallback), this.m_lPersonID);
		}
	}

	public long GetSN()
	{
		return this.m_lSN;
	}

	public bool GetConnected()
	{
		return this.m_bConnected;
	}

	private void ReqWebGuildImageCallback(Texture2D txtr, object _param)
	{
		long num = (long)_param;
		if (num == this.m_lPersonID)
		{
			if (txtr != null)
			{
				this.m_GuildMemberPortrait = txtr;
			}
			else
			{
				this.m_GuildMemberPortrait = null;
			}
		}
	}

	public bool IsConnected()
	{
		string mapName = NrTSingleton<MapManager>.Instance.GetMapName(this.m_iMapUnique);
		return 0 < this.GetChannelID() && 0 < this.GetMapUnique() && !(mapName == string.Empty);
	}
}
