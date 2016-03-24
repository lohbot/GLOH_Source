using PROTOCOL;
using System;
using System.Collections.Generic;

public class DailyDungeonManager : NrTSingleton<DailyDungeonManager>
{
	private Dictionary<int, DAILYDUNGEON_INFO> m_kDailyDungeonInfo = new Dictionary<int, DAILYDUNGEON_INFO>();

	private sbyte m_nDayOfWeek = -1;

	private DailyDungeonManager()
	{
		this.m_kDailyDungeonInfo = new Dictionary<int, DAILYDUNGEON_INFO>();
	}

	public void AddDailyDungeonInfo(DAILYDUNGEON_INFO info)
	{
		if (info == null)
		{
			return;
		}
		if (!this.m_kDailyDungeonInfo.ContainsKey(info.m_i32DayOfWeek))
		{
			this.m_kDailyDungeonInfo.Add(info.m_i32DayOfWeek, info);
		}
		else
		{
			foreach (int current in this.m_kDailyDungeonInfo.Keys)
			{
				if (this.m_kDailyDungeonInfo[current].m_i32DayOfWeek == info.m_i32DayOfWeek)
				{
					this.m_kDailyDungeonInfo[current].m_i8Diff = info.m_i8Diff;
					this.m_kDailyDungeonInfo[current].m_i32IsClear = info.m_i32IsClear;
					this.m_kDailyDungeonInfo[current].m_i32ResetCount = info.m_i32ResetCount;
					this.m_kDailyDungeonInfo[current].m_i8IsReward = info.m_i8IsReward;
					break;
				}
			}
		}
	}

	public void ClearDailyDungeon()
	{
		this.m_kDailyDungeonInfo.Clear();
	}

	public DAILYDUNGEON_INFO GetDailyDungeonInfo(int i32Week)
	{
		if (this.m_kDailyDungeonInfo.ContainsKey(i32Week))
		{
			return this.m_kDailyDungeonInfo[i32Week];
		}
		return null;
	}

	public Dictionary<int, DAILYDUNGEON_INFO> GetTotalDailyDungeonInfo()
	{
		return this.m_kDailyDungeonInfo;
	}

	public long GetResetCount()
	{
		long num = 0L;
		if (this.m_kDailyDungeonInfo == null)
		{
			return 0L;
		}
		foreach (DAILYDUNGEON_INFO current in this.m_kDailyDungeonInfo.Values)
		{
			if (0 < current.m_i32ResetCount)
			{
				num += (long)current.m_i32ResetCount;
			}
		}
		return num;
	}

	public void SetDayOfWeek(sbyte Week)
	{
		this.m_nDayOfWeek = Week;
	}

	public sbyte GetDayOfWeek()
	{
		return this.m_nDayOfWeek;
	}

	public int GetCurrWeekofDay()
	{
		DateTime dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
		if (dueDate.Hour <= 5 && dueDate.Minute < 59)
		{
			switch (dueDate.DayOfWeek)
			{
			case DayOfWeek.Sunday:
				return 6;
			case DayOfWeek.Monday:
				return 0;
			case DayOfWeek.Tuesday:
				return 1;
			case DayOfWeek.Wednesday:
				return 2;
			case DayOfWeek.Thursday:
				return 3;
			case DayOfWeek.Friday:
				return 4;
			case DayOfWeek.Saturday:
				return 5;
			}
		}
		return (int)dueDate.DayOfWeek;
	}

	public int GetNextWeekOfDayCheck(int i32Week)
	{
		if (!this.GetWeekend())
		{
			return this.GetCurrWeekofDay();
		}
		return i32Week;
	}

	public bool GetWeekend()
	{
		int currWeekofDay = this.GetCurrWeekofDay();
		return currWeekofDay == 0 || currWeekofDay == 6;
	}

	public bool IsDailyDungeonPlay(int i32Week)
	{
		return this.GetWeekend() || this.GetCurrWeekofDay() == i32Week;
	}
}
