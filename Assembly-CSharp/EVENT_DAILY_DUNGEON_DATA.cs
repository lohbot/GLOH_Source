using System;
using System.Collections.Generic;
using TsLibs;

public class EVENT_DAILY_DUNGEON_DATA : NrTableBase
{
	public Dictionary<sbyte, Dictionary<sbyte, EVENT_DAILY_DUNGEON_INFO>> m_dicDailyDungeonData;

	private static EVENT_DAILY_DUNGEON_DATA Instance;

	public EVENT_DAILY_DUNGEON_DATA() : base(CDefinePath.DAILY_DUNGEON_URL, true)
	{
		this.m_dicDailyDungeonData = new Dictionary<sbyte, Dictionary<sbyte, EVENT_DAILY_DUNGEON_INFO>>();
		this.m_dicDailyDungeonData.Clear();
		EVENT_DAILY_DUNGEON_DATA.Instance = this;
	}

	public static EVENT_DAILY_DUNGEON_DATA GetInstance()
	{
		if (EVENT_DAILY_DUNGEON_DATA.Instance == null)
		{
			EVENT_DAILY_DUNGEON_DATA.Instance = new EVENT_DAILY_DUNGEON_DATA();
		}
		return EVENT_DAILY_DUNGEON_DATA.Instance;
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			EVENT_DAILY_DUNGEON_INFO eVENT_DAILY_DUNGEON_INFO = new EVENT_DAILY_DUNGEON_INFO();
			eVENT_DAILY_DUNGEON_INFO.SetData(data);
			if (this.m_dicDailyDungeonData.ContainsKey(eVENT_DAILY_DUNGEON_INFO.i8DayOfWeek))
			{
				Dictionary<sbyte, EVENT_DAILY_DUNGEON_INFO> dictionary = this.m_dicDailyDungeonData[eVENT_DAILY_DUNGEON_INFO.i8DayOfWeek];
				if (dictionary != null && !dictionary.ContainsKey(eVENT_DAILY_DUNGEON_INFO.i8Difficulty))
				{
					dictionary.Add(eVENT_DAILY_DUNGEON_INFO.i8Difficulty, eVENT_DAILY_DUNGEON_INFO);
				}
			}
			else
			{
				Dictionary<sbyte, EVENT_DAILY_DUNGEON_INFO> dictionary2 = new Dictionary<sbyte, EVENT_DAILY_DUNGEON_INFO>();
				dictionary2.Add(eVENT_DAILY_DUNGEON_INFO.i8Difficulty, eVENT_DAILY_DUNGEON_INFO);
				this.m_dicDailyDungeonData.Add(eVENT_DAILY_DUNGEON_INFO.i8DayOfWeek, dictionary2);
			}
		}
		return true;
	}

	public EVENT_DAILY_DUNGEON_INFO GetDailyDungeonInfo(sbyte nDifficulty, sbyte nDayOfWeek)
	{
		if (this.m_dicDailyDungeonData.ContainsKey(nDayOfWeek))
		{
			Dictionary<sbyte, EVENT_DAILY_DUNGEON_INFO> dictionary = this.m_dicDailyDungeonData[nDayOfWeek];
			if (dictionary != null && dictionary.ContainsKey(nDifficulty))
			{
				return dictionary[nDifficulty];
			}
		}
		return null;
	}

	public Dictionary<sbyte, EVENT_DAILY_DUNGEON_INFO> GetDailyDungeonInfo(sbyte nDayOfWeek)
	{
		if (this.m_dicDailyDungeonData.ContainsKey(nDayOfWeek))
		{
			Dictionary<sbyte, EVENT_DAILY_DUNGEON_INFO> dictionary = this.m_dicDailyDungeonData[nDayOfWeek];
			if (dictionary != null)
			{
				return dictionary;
			}
		}
		return null;
	}
}
