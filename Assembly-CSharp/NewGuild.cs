using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;

public class NewGuild
{
	private long m_lGuildID;

	private short m_iLevel;

	private long m_lExp;

	private string m_strGuildName = string.Empty;

	private long m_lCreateDate;

	private byte m_bySetImage;

	private string m_strGuildMessage = string.Empty;

	private short m_iRank;

	private long m_lFund;

	private string m_strGuildNotice = string.Empty;

	private bool m_isGuildWar;

	private bool m_bIsExitAgit;

	private List<NewGuildMember> m_NewGuildMemberList = new List<NewGuildMember>();

	private List<NewGuildApplicant> m_NewGuildApplicantList = new List<NewGuildApplicant>();

	public long GetGuildID()
	{
		return this.m_lGuildID;
	}

	public short GetLevel()
	{
		return this.m_iLevel;
	}

	public long GetExp()
	{
		return this.m_lExp;
	}

	public string GetGuildName()
	{
		return this.m_strGuildName;
	}

	public long GetCreateDate()
	{
		return this.m_lCreateDate;
	}

	public byte GetSetImage()
	{
		return this.m_bySetImage;
	}

	public void SetImage(byte bySetImage)
	{
		this.m_bySetImage = bySetImage;
	}

	public string GetGuildMessage()
	{
		return this.m_strGuildMessage;
	}

	public short GetRank()
	{
		return this.m_iRank;
	}

	public long GetFund()
	{
		return this.m_lFund;
	}

	public string GetGuildNotice()
	{
		return this.m_strGuildNotice;
	}

	public bool IsGuildWar()
	{
		return this.m_isGuildWar;
	}

	public bool IsExitAgit()
	{
		return this.m_bIsExitAgit;
	}

	public void Clear()
	{
		this.m_lGuildID = 0L;
		this.m_iLevel = 0;
		this.m_lExp = 0L;
		this.m_strGuildName = string.Empty;
		this.m_lCreateDate = 0L;
		this.m_bySetImage = 0;
		this.m_strGuildMessage = string.Empty;
		this.m_iRank = 0;
		this.m_lFund = 0L;
		this.m_strGuildNotice = string.Empty;
		this.m_NewGuildMemberList.Clear();
		this.m_NewGuildApplicantList.Clear();
	}

	public void SetExitAgit(bool bIsExitAgit)
	{
		this.m_bIsExitAgit = bIsExitAgit;
	}

	public void SetGuildInfo(NEWGUILD_INFO NewGuildInfo)
	{
		this.m_lGuildID = NewGuildInfo.i64GuildID;
		this.m_iLevel = NewGuildInfo.i16Level;
		this.m_lExp = NewGuildInfo.i64Exp;
		this.m_strGuildName = TKString.NEWString(NewGuildInfo.strGuildName);
		this.m_lCreateDate = NewGuildInfo.i64CreateDate;
		this.m_bySetImage = NewGuildInfo.ui8SetImage;
		this.m_strGuildMessage = TKString.NEWString(NewGuildInfo.strGuildMessage);
		this.m_iRank = NewGuildInfo.i16Rank;
		this.m_lFund = NewGuildInfo.i64Fund;
		this.m_strGuildNotice = TKString.NEWString(NewGuildInfo.strGuildNotice);
		this.m_isGuildWar = NewGuildInfo.bIsGuildWar;
		this.m_bIsExitAgit = NewGuildInfo.bIsExitAgit;
		NrTSingleton<GuildWarManager>.Instance.bIsGuildWar = NewGuildInfo.bIsGuildWar;
		NrTSingleton<GuildWarManager>.Instance.bIsGuildWarCancelReservation = NewGuildInfo.bIsGuildWarCancelReservation;
	}

	public void AddMemberInfo(NEWGUILDMEMBER_INFO NewGuildMemberInfo)
	{
		NewGuildMember item = new NewGuildMember(NewGuildMemberInfo);
		this.m_NewGuildMemberList.Add(item);
	}

	public void AddApplicantInfo(NEWGUILDMEMBER_APPLICANT_INFO NewGuildApplicantInfo)
	{
		NewGuildApplicant item = new NewGuildApplicant(NewGuildApplicantInfo);
		this.m_NewGuildApplicantList.Add(item);
	}

	public bool IsMaster(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				if (this.m_NewGuildMemberList[i].GetRank() == NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_MASTER)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsSubMaster(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				if (this.m_NewGuildMemberList[i].GetRank() == NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_SUB_MASTER)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsOfficer(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				if (this.m_NewGuildMemberList[i].GetRank() == NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_OFFICER)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsRankChange(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				if (NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_SUB_MASTER <= this.m_NewGuildMemberList[i].GetRank())
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsDischargeMember(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				if (NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_OFFICER <= this.m_NewGuildMemberList[i].GetRank())
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsInviteMember(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				if (NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_OFFICER <= this.m_NewGuildMemberList[i].GetRank())
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsApplicantMemberNum(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				if (NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_OFFICER <= this.m_NewGuildMemberList[i].GetRank())
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetMemberCount()
	{
		return this.m_NewGuildMemberList.Count;
	}

	public NewGuildMember GetMemberInfoFromIndex(int iIndex)
	{
		if (0 > iIndex || this.m_NewGuildMemberList.Count <= iIndex)
		{
			return null;
		}
		return this.m_NewGuildMemberList[iIndex];
	}

	public NewGuildMember GetMemberInfoFromPersonID(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				return this.m_NewGuildMemberList[i];
			}
		}
		return null;
	}

	public int GetApplicantCount()
	{
		return this.m_NewGuildApplicantList.Count;
	}

	public NewGuildApplicant GetApplicantInfoFromIndex(int iIndex)
	{
		if (0 > iIndex || this.m_NewGuildApplicantList.Count <= iIndex)
		{
			return null;
		}
		return this.m_NewGuildApplicantList[iIndex];
	}

	public NewGuildApplicant GetApplicantInfoFromPersonID(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildApplicantList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildApplicantList[i].GetPersonID())
			{
				return this.m_NewGuildApplicantList[i];
			}
		}
		return null;
	}

	public void ChangeGuildMessage(string strGuildMessage)
	{
		this.m_strGuildMessage = strGuildMessage;
	}

	public void RemoveApplicant(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildApplicantList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildApplicantList[i].GetPersonID())
			{
				this.m_NewGuildApplicantList.RemoveAt(i);
				break;
			}
		}
	}

	public void RemoveGuildMember(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				this.m_NewGuildMemberList.RemoveAt(i);
			}
		}
	}

	public NewGuildDefine.eNEWGUILD_MEMBER_RANK GetMemberRank(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				return this.m_NewGuildMemberList[i].GetRank();
			}
		}
		return NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_NONE;
	}

	public void ChangeMemberRank(long lPersonID, NewGuildDefine.eNEWGUILD_MEMBER_RANK eRank)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				this.m_NewGuildMemberList[i].SetRank(eRank);
				return;
			}
		}
	}

	public bool IsGuildPost(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				if (NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_OFFICER <= this.m_NewGuildMemberList[i].GetRank())
				{
					return true;
				}
			}
		}
		return false;
	}

	public void ChangeGuildName(string strGuildName)
	{
		this.m_strGuildName = strGuildName;
	}

	public int GetGuildMemberIndex(long lPersonID)
	{
		for (int i = 0; i < this.m_NewGuildMemberList.Count; i++)
		{
			if (lPersonID == this.m_NewGuildMemberList[i].GetPersonID())
			{
				return i;
			}
		}
		return -1;
	}

	public void ChangeGuildNotice(string strGuildNotice)
	{
		this.m_strGuildNotice = strGuildNotice;
	}

	public void SetFund(long i64Fund)
	{
		this.m_lFund = i64Fund;
	}
}
