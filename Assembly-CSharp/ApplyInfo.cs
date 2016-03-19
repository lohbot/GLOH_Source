using PROTOCOL;
using System;
using System.Collections.Generic;

public class ApplyInfo
{
	private ApplyUserInfo m_ApplyUserInfo = new ApplyUserInfo();

	private List<GUILDWAR_APPLY_MILITARY_DETAIL_INFO> m_DetailInfo = new List<GUILDWAR_APPLY_MILITARY_DETAIL_INFO>();

	public ApplyInfo(GUILDWAR_APPLY_MILITARY_USER_INFO UserInfo)
	{
		this.m_ApplyUserInfo.SetApllyUserInfo(UserInfo);
	}

	public void AddDetailInfo(GUILDWAR_APPLY_MILITARY_DETAIL_INFO DetailInfo)
	{
		this.m_DetailInfo.Add(DetailInfo);
	}

	public byte GetRaidBattlePos()
	{
		return this.m_ApplyUserInfo.GetRaidBattlePos();
	}

	public long GetMilitaryID()
	{
		return this.m_ApplyUserInfo.GetMilitaryID();
	}

	public long GetPersonID()
	{
		return this.m_ApplyUserInfo.GetPersonID();
	}

	public byte GetMilitaryUnique()
	{
		return this.m_ApplyUserInfo.GetMilitaryUnique();
	}

	public string GetCharName()
	{
		return this.m_ApplyUserInfo.GetCharName();
	}

	public short GetCharLevel()
	{
		return this.m_ApplyUserInfo.GetCharLevel();
	}

	public byte GetLeader()
	{
		return this.m_ApplyUserInfo.GetLeader();
	}

	public ApplyUserInfo GetUserInfo()
	{
		return this.m_ApplyUserInfo;
	}

	public GUILDWAR_APPLY_MILITARY_DETAIL_INFO GetDetailInfo(int iIndex)
	{
		if (iIndex < 0 || iIndex >= this.m_DetailInfo.Count)
		{
			return null;
		}
		return this.m_DetailInfo[iIndex];
	}

	public GUILDWAR_APPLY_MILITARY_DETAIL_INFO GetDetailInfoBattlePos(int iBattlePos)
	{
		foreach (GUILDWAR_APPLY_MILITARY_DETAIL_INFO current in this.m_DetailInfo)
		{
			if ((int)current.i16BattlePos == iBattlePos)
			{
				return current;
			}
		}
		return null;
	}
}
