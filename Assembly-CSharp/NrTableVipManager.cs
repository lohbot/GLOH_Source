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
		vIP_INFODATA.i32NeedExp = _VipInfo.i32NeedExp;
		vIP_INFODATA.i8MaxWill = _VipInfo.i8MaxWill;
		vIP_INFODATA.i8WillChangeTime = _VipInfo.i8WillChangeTime;
		vIP_INFODATA.i32FriendSupportItem = _VipInfo.i32FriendSupportItem;
		vIP_INFODATA.i8FriendSupportNum = _VipInfo.i8FriendSupportNum;
		vIP_INFODATA.i16FastBattle = _VipInfo.i16FastBattle;
		vIP_INFODATA.i8Battle_Repeat_Add = _VipInfo.i8Battle_Repeat_Add;
		if (vIP_INFODATA.i8VipLevel > 0)
		{
			this.m_VipInfoList.Add(vIP_INFODATA);
		}
	}

	public int GetLevelMaxExp(byte i8Level)
	{
		int result = 0;
		if (i8Level < 0)
		{
			return 0;
		}
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == i8Level)
			{
				result = this.m_VipInfoList[i].i32NeedExp;
				break;
			}
		}
		return result;
	}

	public int GetNextLevelMaxExp(byte i8Level)
	{
		int result = 0;
		byte b;
		i8Level = (b = i8Level + 1);
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == b)
			{
				result = this.m_VipInfoList[i].i32NeedExp;
				break;
			}
		}
		return result;
	}

	public byte GetLevelExp(int i32VipExp)
	{
		byte result = 0;
		if (i32VipExp <= 0)
		{
			return 0;
		}
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i32NeedExp <= i32VipExp)
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
		byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((int)charSubData);
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
		byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((int)charSubData);
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

	public int GetBeforLevelMaxExp(byte i8Level)
	{
		int result = 0;
		byte b;
		i8Level = (b = i8Level - 1);
		if (b <= 0)
		{
			return 0;
		}
		for (int i = 0; i < this.m_VipInfoList.Count; i++)
		{
			if (this.m_VipInfoList[i].i8VipLevel == b)
			{
				result = this.m_VipInfoList[i].i32NeedExp;
				break;
			}
		}
		return result;
	}

	public short GetVipLevelAddBattleRepeat()
	{
		long charSubData = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
		byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((int)charSubData);
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
}
