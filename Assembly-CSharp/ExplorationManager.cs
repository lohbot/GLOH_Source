using GAME;
using System;
using System.Collections.Generic;

public class ExplorationManager : NrTSingleton<ExplorationManager>
{
	public const int MAX_REWARD_NUM = 3;

	private List<NkSoldierInfo> m_kSolInfo = new List<NkSoldierInfo>();

	private Dictionary<int, List<ExplorationTable>> m_kExploration = new Dictionary<int, List<ExplorationTable>>();

	private int m_nExpCount;

	private long m_nRewardMoney;

	private Dictionary<int, ITEM> m_kItemList = new Dictionary<int, ITEM>();

	private ExplorationManager()
	{
	}

	public bool Initialize()
	{
		return true;
	}

	public void AddExplorationTable(ExplorationTable table)
	{
		if (!this.m_kExploration.ContainsKey(table.m_nLimitLevel))
		{
			List<ExplorationTable> list = new List<ExplorationTable>();
			list.Add(table);
			this.m_kExploration.Add(table.m_nLimitLevel, list);
		}
		else
		{
			this.m_kExploration[table.m_nLimitLevel].Add(table);
		}
	}

	public List<ExplorationTable> GetExplorationTable(int limitLevel)
	{
		foreach (int current in this.m_kExploration.Keys)
		{
			if (current > limitLevel)
			{
				return this.m_kExploration[current];
			}
		}
		return null;
	}

	public ExplorationTable GetExplorationTable(int limitLevel, int index)
	{
		foreach (int current in this.m_kExploration.Keys)
		{
			if (limitLevel <= current)
			{
				List<ExplorationTable> list = this.m_kExploration[current];
				if (list != null)
				{
					return list[index];
				}
			}
		}
		return null;
	}

	public int GetExpCount()
	{
		return this.m_nExpCount;
	}

	public long GetRewardMoney()
	{
		return this.m_nRewardMoney;
	}

	public Dictionary<int, ITEM> GetRewardItem()
	{
		return this.m_kItemList;
	}

	public void AddReward(long money, ITEM item)
	{
		this.m_nExpCount++;
		this.m_nRewardMoney += money;
		if (0 < item.m_nItemUnique)
		{
			if (!this.m_kItemList.ContainsKey(item.m_nItemUnique))
			{
				this.m_kItemList.Add(item.m_nItemUnique, item);
			}
			else
			{
				this.m_kItemList[item.m_nItemUnique].m_nItemNum += item.m_nItemNum;
			}
		}
	}

	public void ClearReward()
	{
		this.m_nExpCount = 0;
		this.m_nRewardMoney = 0L;
		this.m_kItemList.Clear();
	}

	private static int CompareExp(NkSoldierInfo a, NkSoldierInfo b)
	{
		if (0 > a.GetLevel() - b.GetLevel())
		{
			return -1;
		}
		return 1;
	}

	public void SortSolInfo()
	{
		this.m_kSolInfo.Sort(new Comparison<NkSoldierInfo>(ExplorationManager.CompareExp));
	}

	public bool IsSolInfo(long solID)
	{
		foreach (NkSoldierInfo current in this.m_kSolInfo)
		{
			if (current.GetSolID() == solID)
			{
				return true;
			}
		}
		return false;
	}

	public void AddCheckSolInfo(NkSoldierInfo solInfo)
	{
		if (this.m_kSolInfo.Count >= 6)
		{
			return;
		}
		this.AddSolInfo(solInfo);
	}

	public void AddSolInfo(NkSoldierInfo solInfo)
	{
		foreach (NkSoldierInfo current in this.m_kSolInfo)
		{
			if (current.GetSolID() == solInfo.GetSolID())
			{
				return;
			}
		}
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(solInfo);
		this.m_kSolInfo.Add(nkSoldierInfo);
	}

	public void RemoveSolInfo(long solID)
	{
		foreach (NkSoldierInfo current in this.m_kSolInfo)
		{
			if (current.GetSolID() == solID)
			{
				this.m_kSolInfo.Remove(current);
				break;
			}
		}
		this.m_kSolInfo.Sort(new Comparison<NkSoldierInfo>(ExplorationManager.CompareExp));
	}

	public List<NkSoldierInfo> GetSolInfo()
	{
		return this.m_kSolInfo;
	}

	public long GetSolID(int index)
	{
		if (this.m_kSolInfo[index] == null)
		{
			return 0L;
		}
		return this.m_kSolInfo[index].GetSolID();
	}

	public void UpdateSolInfo(NkSoldierInfo info)
	{
		foreach (NkSoldierInfo current in this.m_kSolInfo)
		{
			if (current.GetSolID() == info.GetSolID())
			{
				current.m_kBase.Level = info.GetLevel();
				current.m_kBase.Exp = info.GetExp();
				break;
			}
		}
	}
}
