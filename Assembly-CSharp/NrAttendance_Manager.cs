using GAME;
using System;
using System.Collections.Generic;

public class NrAttendance_Manager : NrTSingleton<NrAttendance_Manager>
{
	private Dictionary<int, ATTENDANCE> m_DiNomalAttend = new Dictionary<int, ATTENDANCE>();

	private Dictionary<int, ATTENDANCE> m_DiNewAttend = new Dictionary<int, ATTENDANCE>();

	private Dictionary<int, ATTENDANCE> m_DiReturnAttend = new Dictionary<int, ATTENDANCE>();

	private Dictionary<int, ATTENDANCE> m_DiConsecutivelyAttend = new Dictionary<int, ATTENDANCE>();

	private NrAttendance_Manager()
	{
	}

	public void AddData(ATTENDANCE data)
	{
		switch (data.m_i16UserType)
		{
		case 1:
			if (this.m_DiNomalAttend.ContainsKey(data.m_i32Index))
			{
				this.m_DiNomalAttend[data.m_i32Index] = data;
			}
			else
			{
				this.m_DiNomalAttend.Add(data.m_i32Index, data);
			}
			break;
		case 2:
			if (this.m_DiNewAttend.ContainsKey(data.m_i32Index))
			{
				this.m_DiNewAttend[data.m_i32Index] = data;
			}
			else
			{
				this.m_DiNewAttend.Add(data.m_i32Index, data);
			}
			break;
		case 3:
			if (this.m_DiReturnAttend.ContainsKey(data.m_i32Index))
			{
				this.m_DiReturnAttend[data.m_i32Index] = data;
			}
			else
			{
				this.m_DiReturnAttend.Add(data.m_i32Index, data);
			}
			break;
		case 4:
			if (this.m_DiConsecutivelyAttend.ContainsKey(data.m_i32Index))
			{
				this.m_DiConsecutivelyAttend[data.m_i32Index] = data;
			}
			else
			{
				this.m_DiConsecutivelyAttend.Add(data.m_i32Index, data);
			}
			break;
		}
	}

	public void Get_Attend_Item(eATTENDANCE_USERTYPE a_Type, int a_i32Group, short a_i16Sequence, byte i8RewardType, ref int i32ItemUnique, ref short i16ItemNum)
	{
		switch (a_Type)
		{
		case eATTENDANCE_USERTYPE.eATTENDANCE_NORMAL:
			foreach (ATTENDANCE current in this.m_DiNomalAttend.Values)
			{
				if (current.m_i32Group == a_i32Group && current.m_i16Attend_Sequence == a_i16Sequence)
				{
					i32ItemUnique = current.m_i32Item_Unique;
					i16ItemNum = current.m_i16Item_Num;
					break;
				}
			}
			break;
		case eATTENDANCE_USERTYPE.eATTENDANCE_NEW:
			foreach (ATTENDANCE current2 in this.m_DiNewAttend.Values)
			{
				if (current2.m_i32Group == a_i32Group && current2.m_i16Attend_Sequence == a_i16Sequence)
				{
					i32ItemUnique = current2.m_i32Item_Unique;
					i16ItemNum = current2.m_i16Item_Num;
					break;
				}
			}
			break;
		case eATTENDANCE_USERTYPE.eATTENDANCE_RETURN:
			foreach (ATTENDANCE current3 in this.m_DiReturnAttend.Values)
			{
				if (current3.m_i32Group == a_i32Group && current3.m_i16Attend_Sequence == a_i16Sequence)
				{
					i32ItemUnique = current3.m_i32Item_Unique;
					i16ItemNum = current3.m_i16Item_Num;
					break;
				}
			}
			break;
		case eATTENDANCE_USERTYPE.eATTENDANCE_CONSECUTIVELY:
			foreach (ATTENDANCE current4 in this.m_DiConsecutivelyAttend.Values)
			{
				if (current4.m_i32Group == a_i32Group && current4.m_i16Attend_Sequence == a_i16Sequence && current4.m_i8RewardType == i8RewardType)
				{
					i32ItemUnique = current4.m_i32Item_Unique;
					i16ItemNum = current4.m_i16Item_Num;
					break;
				}
			}
			break;
		}
	}

	public string Get_NewAttend_ItemImage(int a_i32Group, short a_i16Sequence)
	{
		foreach (ATTENDANCE current in this.m_DiNewAttend.Values)
		{
			if (current.m_i32Group == a_i32Group && current.m_i16Attend_Sequence == a_i16Sequence)
			{
				return current.m_strImageBundle;
			}
		}
		return string.Empty;
	}

	public ATTENDANCE Get_Consecutivelyattendance(byte bTotalNum, byte i8RewardType)
	{
		ATTENDANCE result = null;
		foreach (ATTENDANCE current in this.m_DiConsecutivelyAttend.Values)
		{
			if (current.m_i16Attend_Sequence == (short)bTotalNum && current.m_i8RewardType == i8RewardType)
			{
				result = current;
				break;
			}
		}
		return result;
	}

	public ATTENDANCE Get_ConsecutivelyattendanceIndex(byte Index, byte i8RewardType)
	{
		ATTENDANCE result = null;
		int num = 1;
		foreach (ATTENDANCE current in this.m_DiConsecutivelyAttend.Values)
		{
			if (current.m_i8RewardType == i8RewardType)
			{
				if (num == (int)Index)
				{
					result = current;
					break;
				}
				num++;
			}
		}
		return result;
	}
}
