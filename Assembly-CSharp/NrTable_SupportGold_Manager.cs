using GAME;
using System;
using System.Collections.Generic;

public class NrTable_SupportGold_Manager : NrTSingleton<NrTable_SupportGold_Manager>
{
	private SortedDictionary<int, PLUNDER_SUPPORT_GOLD> m_sdCollection;

	private NrTable_SupportGold_Manager()
	{
		this.m_sdCollection = new SortedDictionary<int, PLUNDER_SUPPORT_GOLD>();
	}

	public SortedDictionary<int, PLUNDER_SUPPORT_GOLD> Get_Collection()
	{
		return this.m_sdCollection;
	}

	public int Get_Count()
	{
		return this.m_sdCollection.Count;
	}

	public PLUNDER_SUPPORT_GOLD Get_Value(int nCharLevel)
	{
		if (this.m_sdCollection.ContainsKey(nCharLevel))
		{
			return this.m_sdCollection[nCharLevel];
		}
		return null;
	}

	public void Set_Value(PLUNDER_SUPPORT_GOLD a_cValue)
	{
		if (this.m_sdCollection.ContainsKey(a_cValue.m_nCharLevel))
		{
			this.m_sdCollection[a_cValue.m_nCharLevel] = a_cValue;
		}
		else
		{
			this.m_sdCollection.Add(a_cValue.m_nCharLevel, a_cValue);
		}
	}

	public long GetReceiveGold(int nCharLevel, long nPastTime)
	{
		PLUNDER_SUPPORT_GOLD pLUNDER_SUPPORT_GOLD = this.Get_Value(nCharLevel);
		if (pLUNDER_SUPPORT_GOLD == null)
		{
			return 0L;
		}
		long nSupportGold = pLUNDER_SUPPORT_GOLD.m_nSupportGold;
		long nMaxGold = pLUNDER_SUPPORT_GOLD.m_nMaxGold;
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_SUPPORTTIME);
		int num = (int)(nPastTime / 60L / (long)value);
		long val = (long)num * nSupportGold;
		return Math.Min(val, nMaxGold);
	}
}
