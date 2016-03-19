using PROTOCOL;
using PROTOCOL.GAME;
using System;

public class GuildWarManager : NrTSingleton<GuildWarManager>
{
	private short m_i16GuildWarJoinCount;

	private short m_i16GuildWarRound;

	private long m_i64GuildWarGuildID;

	private long m_i64GuildWarStartTime;

	private string m_strGuildWarGuildName = string.Empty;

	private short m_i16GuildWarApplyLevel;

	private short m_i16GuildWarApplySolLevel;

	public short GuildWarJoinCount
	{
		get
		{
			return this.m_i16GuildWarJoinCount;
		}
		set
		{
			this.m_i16GuildWarJoinCount = value;
		}
	}

	public short GuildWarRound
	{
		get
		{
			return this.m_i16GuildWarRound;
		}
		set
		{
			this.m_i16GuildWarRound = value;
		}
	}

	public long GuildWarGuildID
	{
		get
		{
			return this.m_i64GuildWarGuildID;
		}
		set
		{
			this.m_i64GuildWarGuildID = value;
		}
	}

	public long GuildWarStartTime
	{
		get
		{
			return this.m_i64GuildWarStartTime;
		}
		set
		{
			this.m_i64GuildWarStartTime = value;
		}
	}

	public string GuildWarGuildName
	{
		get
		{
			return this.m_strGuildWarGuildName;
		}
		set
		{
			this.m_strGuildWarGuildName = value;
		}
	}

	public short GuildWarApplyLevel
	{
		get
		{
			return this.m_i16GuildWarApplyLevel;
		}
		set
		{
			this.m_i16GuildWarApplyLevel = value;
		}
	}

	public short GuildWarApplySolLevel
	{
		get
		{
			return this.m_i16GuildWarApplySolLevel;
		}
		set
		{
			this.m_i16GuildWarApplySolLevel = value;
		}
	}

	private GuildWarManager()
	{
	}

	public void Clear()
	{
		this.m_i16GuildWarJoinCount = 0;
		this.m_i16GuildWarRound = 0;
		this.m_i64GuildWarGuildID = 0L;
		this.m_i64GuildWarStartTime = 0L;
		this.m_strGuildWarGuildName = string.Empty;
		this.m_i16GuildWarApplyLevel = 0;
		this.m_i16GuildWarApplySolLevel = 0;
	}

	public string GetGuildWarWaitTimeToString()
	{
		return string.Empty;
	}

	public void Send_GS_GUILDWAR_APPLY_INFO_REQ(int iPageIndex)
	{
		GS_GUILDWAR_APPLY_INFO_REQ gS_GUILDWAR_APPLY_INFO_REQ = new GS_GUILDWAR_APPLY_INFO_REQ();
		gS_GUILDWAR_APPLY_INFO_REQ.i32CurPage = iPageIndex;
		SendPacket.GetInstance().SendObject(2204, gS_GUILDWAR_APPLY_INFO_REQ);
	}

	public void Send_GS_GUILDWAR_APPLY_MILITARY_INFO_REQ(long i64GuildID, byte iRadeUnique)
	{
		GS_GUILDWAR_APPLY_MILITARY_INFO_REQ gS_GUILDWAR_APPLY_MILITARY_INFO_REQ = new GS_GUILDWAR_APPLY_MILITARY_INFO_REQ();
		gS_GUILDWAR_APPLY_MILITARY_INFO_REQ.i64GuildID = i64GuildID;
		gS_GUILDWAR_APPLY_MILITARY_INFO_REQ.ui8RaidUnique = iRadeUnique;
		SendPacket.GetInstance().SendObject(2206, gS_GUILDWAR_APPLY_MILITARY_INFO_REQ);
	}

	public void Send_GS_GUILDWAR_APPLY_CANCEL_REQ(byte iRadeUnique)
	{
		GS_GUILDWAR_APPLY_CANCEL_REQ gS_GUILDWAR_APPLY_CANCEL_REQ = new GS_GUILDWAR_APPLY_CANCEL_REQ();
		gS_GUILDWAR_APPLY_CANCEL_REQ.ui8RaidUnique = iRadeUnique;
		SendPacket.GetInstance().SendObject(2202, gS_GUILDWAR_APPLY_CANCEL_REQ);
	}

	public void Send_GS_GUILDWAR_REWARDINFO_REQ()
	{
		GS_GUILDWAR_REWARDINFO_REQ obj = new GS_GUILDWAR_REWARDINFO_REQ();
		SendPacket.GetInstance().SendObject(2210, obj);
	}

	public void ShowNotifyFromResult(int iResult)
	{
		switch (iResult)
		{
		}
	}
}
