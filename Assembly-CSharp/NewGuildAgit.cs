using PROTOCOL;
using System;
using System.Collections.Generic;

public class NewGuildAgit
{
	private short m_i16AgitLevel;

	private long m_i64AgitExp;

	private int m_GoldenEggGetCount;

	private List<AGIT_NPC_SUB_DATA> m_NPCSubData = new List<AGIT_NPC_SUB_DATA>();

	private List<AgitNPCInfo> m_NPCInfoList = new List<AgitNPCInfo>();

	private List<AGIT_GOLDENEGG_INFO_SUB_DATA> m_RewardPersonInfoList = new List<AGIT_GOLDENEGG_INFO_SUB_DATA>();

	public short AgitLevel
	{
		get
		{
			return this.m_i16AgitLevel;
		}
		set
		{
			this.m_i16AgitLevel = value;
		}
	}

	public long AgitExp
	{
		get
		{
			return this.m_i64AgitExp;
		}
		set
		{
			this.m_i64AgitExp = value;
		}
	}

	public int GetGoldenEggGetCount()
	{
		return this.m_GoldenEggGetCount;
	}

	public void SetGoldenEggGetCount(int GoldenEggGetCount)
	{
		this.m_GoldenEggGetCount = GoldenEggGetCount;
	}

	public List<AGIT_GOLDENEGG_INFO_SUB_DATA> GetRewardPersonInfoList()
	{
		return this.m_RewardPersonInfoList;
	}

	public long GetGoldenEggGetLastPerson()
	{
		long result = 0L;
		foreach (AGIT_GOLDENEGG_INFO_SUB_DATA current in this.m_RewardPersonInfoList)
		{
			result = current.i64PersonID;
		}
		return result;
	}

	public void Clear()
	{
		this.m_i16AgitLevel = 0;
		this.m_i64AgitExp = 0L;
		this.m_NPCSubData.Clear();
	}

	public void AddAgitNPCData(AGIT_NPC_SUB_DATA Data)
	{
		for (int i = 0; i < this.m_NPCSubData.Count; i++)
		{
			if (this.m_NPCSubData[i].ui8NPCType == Data.ui8NPCType)
			{
				return;
			}
		}
		this.m_NPCSubData.Add(Data);
	}

	public int GetAgitNPCSubDataCount()
	{
		return this.m_NPCSubData.Count;
	}

	public AGIT_NPC_SUB_DATA GetAgitNPCSubData(int iIndex)
	{
		if (iIndex < 0 || iIndex >= this.m_NPCSubData.Count)
		{
			return null;
		}
		return this.m_NPCSubData[iIndex];
	}

	public AGIT_NPC_SUB_DATA GetAgitNPCSubDataFromNPCType(byte i8NPCType)
	{
		for (int i = 0; i < this.m_NPCSubData.Count; i++)
		{
			if (this.m_NPCSubData[i].ui8NPCType == i8NPCType)
			{
				return this.m_NPCSubData[i];
			}
		}
		return null;
	}

	public void DelAgitNPC(byte ui8NPCType)
	{
		for (int i = 0; i < this.m_NPCSubData.Count; i++)
		{
			if (this.m_NPCSubData[i].ui8NPCType == ui8NPCType)
			{
				this.m_NPCSubData.RemoveAt(i);
				break;
			}
		}
		for (int j = 0; j < this.m_NPCInfoList.Count; j++)
		{
			if (this.m_NPCInfoList[j].GetNPCType() == ui8NPCType)
			{
				NrTSingleton<NkCharManager>.Instance.DeleteChar(this.m_NPCInfoList[j].GetCharID());
				this.m_NPCInfoList.RemoveAt(j);
				break;
			}
		}
	}

	public void ClearNPCInfo()
	{
		this.m_NPCInfoList.Clear();
	}

	public void AddNPCInfo(AgitNPCInfo NPCInfo)
	{
		this.m_NPCInfoList.Add(NPCInfo);
	}

	public bool IsAgitNPC(byte i8NPCType)
	{
		for (int i = 0; i < this.m_NPCSubData.Count; i++)
		{
			if (this.m_NPCSubData[i].ui8NPCType == i8NPCType)
			{
				return true;
			}
		}
		return false;
	}
}
