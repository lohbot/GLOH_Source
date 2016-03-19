using PROTOCOL;
using System;

public class ApplyUserInfo
{
	private byte m_ui8RaidBattlePos;

	private long m_i64MilitaryID;

	private long m_i64PersonID;

	private byte m_ui8MilitaryUnique;

	private string m_strCharName = string.Empty;

	private short m_i16CharLevel;

	private byte m_ui8Leader;

	public void SetApllyUserInfo(GUILDWAR_APPLY_MILITARY_USER_INFO UserInfo)
	{
		this.m_ui8RaidBattlePos = UserInfo.ui8RaidBattlePos;
		this.m_i64MilitaryID = UserInfo.i64MilitaryID;
		this.m_i64PersonID = UserInfo.i64PersonID;
		this.m_ui8MilitaryUnique = UserInfo.ui8MilitaryUnique;
		this.m_strCharName = TKString.NEWString(UserInfo.szCharName);
		this.m_i16CharLevel = UserInfo.i16CharLevel;
		this.m_ui8Leader = UserInfo.ui8Leader;
	}

	public byte GetRaidBattlePos()
	{
		return this.m_ui8RaidBattlePos;
	}

	public long GetMilitaryID()
	{
		return this.m_i64MilitaryID;
	}

	public long GetPersonID()
	{
		return this.m_i64PersonID;
	}

	public byte GetMilitaryUnique()
	{
		return this.m_ui8MilitaryUnique;
	}

	public string GetCharName()
	{
		return this.m_strCharName;
	}

	public short GetCharLevel()
	{
		return this.m_i16CharLevel;
	}

	public byte GetLeader()
	{
		return this.m_ui8Leader;
	}
}
