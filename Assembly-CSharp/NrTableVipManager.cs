using GAME;
using System;
using System.Collections.Generic;

public class NrTableVipManager : NrTSingleton<NrTableVipManager>
{
	private List<VIP_INFODATA> m_VipInfoList = new List<VIP_INFODATA>();

	private NrTableVipManager()
	{
	}

	public void AddVipInfo(VIP_INFODATA _VipInfo)
	{
		VIP_INFODATA vIP_INFODATA = new VIP_INFODATA();
		vIP_INFODATA.i8VipLevel = _VipInfo.i8VipLevel;
		vIP_INFODATA.i64NeedExp = _VipInfo.i64NeedExp;
		vIP_INFODATA.i8MaxWill = _VipInfo.i8MaxWill;
		vIP_INFODATA.i8WillChangeTime = _VipInfo.i8WillChangeTime;
		vIP_INFODATA.i32FriendSupportItem = _VipInfo.i32FriendSupportItem;
		vIP_INFODATA.i8FriendSupportNum = _VipInfo.i8FriendSupportNum;
		vIP_INFODATA.i16FastBattle = _VipInfo.i16FastBattle;
		vIP_INFODATA.i8Battle_Repeat_Add = _VipInfo.i8Battle_Repeat_Add;
		vIP_INFODATA.i8TimeShopCount = _VipInfo.i8TimeShopCount;
		vIP_INFODATA.i8TimeShopFreeRefresh = _VipInfo.i8TimeShopFreeRefresh;
		vIP_INFODATA.i8DailyDungeonReset = _VipInfo.i8DailyDungeonReset;
		vIP_INFODATA.i8DailyDungeonDc = _VipInfo.i8DailyDungeonDc;
		vIP_INFODATA.i8NewExplorationReset = _VipInfo.i8NewExplorationReset;
		this.m_VipInfoList.Add(vIP_INFODATA);
	}

	public long GetLevelMaxExp(byte i8Level)
	{
		long result = 0L;
		if (i8Level < 0)
		{
			return 0L;
		}
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == i8Level)
			{
				result = this.m_VipInfoList[i].i64NeedExp;
				break;
			}
		}
		return result;
	}

	public long GetNextLevelMaxExp(byte i8Level)
	{
		long result = 0L;
		byte b;
		i8Level = (b = i8Level + 1);
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == b)
			{
				result = this.m_VipInfoList[i].i64NeedExp;
				break;
			}
		}
		return result;
	}

	public byte GetLevelExp(long i64VipExp)
	{
		byte result = 0;
		if (i64VipExp <= 0L)
		{
			return 0;
		}
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i64NeedExp <= i64VipExp)
			{
				result = this.m_VipInfoList[i].i8VipLevel;
			}
		}
		return result;
	}

	public List<VIP_INFODATA> GetValue()
	{
		return this.m_VipInfoList;
	}

	public void VipDataClear()
	{
		this.m_VipInfoList.Clear();
	}

	public byte GetVipLevelActivityPointMax()
	{
		long charSubData = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
		byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
		byte result = 0;
		if (levelExp <= 0)
		{
			return 0;
		}
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == levelExp)
			{
				result = this.m_VipInfoList[i].i8MaxWill;
				break;
			}
		}
		return result;
	}

	public short GetVipLevelActivityTime()
	{
		long charSubData = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
		byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
		short result = 0;
		if (levelExp <= 0)
		{
			return 0;
		}
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == levelExp)
			{
				result = (short)this.m_VipInfoList[i].i8WillChangeTime;
				break;
			}
		}
		return result;
	}

	public long GetBeforLevelMaxExp(byte i8Level)
	{
		long result = 0L;
		byte b;
		i8Level = (b = i8Level - 1);
		if (b <= 0)
		{
			return 0L;
		}
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == b)
			{
				result = this.m_VipInfoList[i].i64NeedExp;
				break;
			}
		}
		return result;
	}

	public short GetVipLevelAddBattleRepeat()
	{
		long charSubData = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
		byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
		short result = 0;
		if (levelExp <= 0)
		{
			return 0;
		}
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == levelExp)
			{
				result = (short)this.m_VipInfoList[i].i8Battle_Repeat_Add;
				break;
			}
		}
		return result;
	}

	public byte GetTimeShopCountByVipLevel(byte _i8Level)
	{
		byte result = 0;
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == _i8Level)
			{
				result = this.m_VipInfoList[i].i8TimeShopCount;
				break;
			}
		}
		return result;
	}

	public byte GetVipLevelByTimeShopCount(byte _i8TimeShopCount)
	{
		byte result = 0;
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8TimeShopCount == _i8TimeShopCount)
			{
				result = this.m_VipInfoList[i].i8VipLevel;
				break;
			}
		}
		return result;
	}

	public byte GetTimeShopFreeCountByVipLevel(byte _i8Level)
	{
		byte result = 0;
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == _i8Level)
			{
				result = this.m_VipInfoList[i].i8TimeShopFreeRefresh;
				break;
			}
		}
		return result;
	}

	public byte GetDailyDungeonResetByVipLevel(byte i8VipLevel)
	{
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == i8VipLevel)
			{
				return this.m_VipInfoList[i].i8DailyDungeonReset;
			}
		}
		return 0;
	}

	public byte GetDailyDungeonDcByVipLevel(byte i8VipLevel)
	{
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == i8VipLevel)
			{
				return this.m_VipInfoList[i].i8DailyDungeonDc;
			}
		}
		return 0;
	}

	public byte GetNewExplorationResetCountByVipLevel(byte i8VipLevel)
	{
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == i8VipLevel)
			{
				return this.m_VipInfoList[i].i8NewExplorationReset;
			}
		}
		return 0;
	}
}
