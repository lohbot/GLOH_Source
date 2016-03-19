using PROTOCOL;
using System;
using System.Collections.Generic;

public class NrGuildBoss_MyRoomInfoList
{
	private Dictionary<short, NEWGUILD_MY_BOSS_ROOMINFO> m_GuildBoss_MyRoomInfo = new Dictionary<short, NEWGUILD_MY_BOSS_ROOMINFO>();

	public NrGuildBoss_MyRoomInfoList()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_GuildBoss_MyRoomInfo.Clear();
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

	public bool GuildBossCheck()
	{
		foreach (NEWGUILD_MY_BOSS_ROOMINFO current in this.m_GuildBoss_MyRoomInfo.Values)
		{
			if (current.ui8PlayState > 0)
			{
				return true;
			}
		}
		return false;
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
