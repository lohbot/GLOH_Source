using PROTOCOL;
using System;
using System.Collections.Generic;

public class NrGuildBoss_MyRoomInfoList
{
	private Dictionary<short, NEWGUILD_MY_BOSS_ROOMINFO> m_GuildBoss_MyRoomInfo = new Dictionary<short, NEWGUILD_MY_BOSS_ROOMINFO>();

	private byte m_GuildBoss_RewardInfo;

	private List<short> m_GuildBoss_RoomStateInfo = new List<short>();

	public NrGuildBoss_MyRoomInfoList()
	{
		this.Init();
		this.InitData();
	}

	public void Init()
	{
		this.m_GuildBoss_MyRoomInfo.Clear();
		this.m_GuildBoss_RoomStateInfo.Clear();
		this.m_GuildBoss_RewardInfo = 0;
	}

	public void InitData()
	{
		this.m_GuildBoss_RoomStateInfo.Clear();
		this.m_GuildBoss_RewardInfo = 0;
	}

	public NEWGUILD_MY_BOSS_ROOMINFO GetInfo(short floor)
	{
		if (this.m_GuildBoss_MyRoomInfo.ContainsKey(floor))
		{
			return this.m_GuildBoss_MyRoomInfo[floor];
		}
		return null;
	}

	public int GetCount()
	{
		return this.m_GuildBoss_MyRoomInfo.Count;
	}

	public void AddGuildBossRoomStateInfo(short GuildBossFloor)
	{
		if (GuildBossFloor != 0)
		{
			this.m_GuildBoss_RoomStateInfo.Add(GuildBossFloor);
		}
	}

	public void RemoveGuildBossRoomStateInfo(short GuildBossFloor)
	{
		if (GuildBossFloor != 0)
		{
			this.m_GuildBoss_RoomStateInfo.Remove(GuildBossFloor);
		}
	}

	public bool GetGuildBossRoomStateInfo(short floor)
	{
		return !this.m_GuildBoss_RoomStateInfo.Contains(floor);
	}

	public bool GuildBossCheck()
	{
		foreach (NEWGUILD_MY_BOSS_ROOMINFO current in this.m_GuildBoss_MyRoomInfo.Values)
		{
			if (current.ui8PlayState == 1)
			{
				return true;
			}
		}
		return false;
	}

	public bool GetGuildBossRewardInfo()
	{
		return this.m_GuildBoss_RewardInfo == 1;
	}

	public void AddGuildBossRewardInfo(bool bGuildBossReward)
	{
		if (bGuildBossReward)
		{
			this.m_GuildBoss_RewardInfo = 1;
		}
		else
		{
			this.m_GuildBoss_RewardInfo = 0;
		}
	}

	public void AddInfo(NEWGUILD_MY_BOSS_ROOMINFO info)
	{
		if (this.m_GuildBoss_MyRoomInfo.ContainsKey(info.i16Floor))
		{
			this.m_GuildBoss_MyRoomInfo[info.i16Floor] = info;
		}
		else
		{
			this.m_GuildBoss_MyRoomInfo.Add(info.i16Floor, info);
		}
	}

	public void DelInfo(short floor)
	{
		if (this.m_GuildBoss_MyRoomInfo.ContainsKey(floor))
		{
			this.m_GuildBoss_MyRoomInfo.Remove(floor);
		}
	}

	public Dictionary<short, NEWGUILD_MY_BOSS_ROOMINFO> GetInfo()
	{
		return this.m_GuildBoss_MyRoomInfo;
	}
}
