using GAME;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NkReadySolList
{
	private Dictionary<long, NkSoldierInfo> ReadySolList = new Dictionary<long, NkSoldierInfo>();

	public void Init()
	{
		this.ReadySolList.Clear();
	}

	public NkSoldierInfo GetSolInfo(long solID)
	{
		if (!this.ReadySolList.ContainsKey(solID))
		{
			return null;
		}
		return this.ReadySolList[solID];
	}

	public List<NkSoldierInfo> GetSolInfoListByKind(int charKind)
	{
		List<NkSoldierInfo> list = new List<NkSoldierInfo>();
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current.GetCharKind() == charKind)
			{
				list.Add(current);
			}
		}
		return list;
	}

	public NkSoldierInfo AddSolInfo(NkSoldierInfo solInfo)
	{
		if (this.ReadySolList.ContainsKey(solInfo.GetSolID()))
		{
			return null;
		}
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(solInfo);
		this.ReadySolList.Add(nkSoldierInfo.GetSolID(), nkSoldierInfo);
		return nkSoldierInfo;
	}

	public NkSoldierInfo AddSolInfo(SOLDIER_INFO solInfo, SOLDIER_BATTLESKILL_INFO pkBattleSkill, bool bReadyEquipItem)
	{
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(solInfo);
		if (pkBattleSkill != null)
		{
			nkSoldierInfo.SetBattleSkillInfo(pkBattleSkill);
		}
		if (this.ReadySolList.ContainsKey(solInfo.SolID))
		{
			NkSoldierInfo nkSoldierInfo2;
			if (this.ReadySolList.TryGetValue(solInfo.SolID, out nkSoldierInfo2))
			{
				NrEquipItemInfo equipItemInfo = nkSoldierInfo.GetEquipItemInfo();
				if (equipItemInfo != null)
				{
					equipItemInfo.Set(nkSoldierInfo2.GetEquipItemInfo());
				}
			}
			this.ReadySolList.Remove(nkSoldierInfo.GetSolID());
			bReadyEquipItem = true;
		}
		nkSoldierInfo.SetReceivedEquipItem(bReadyEquipItem);
		nkSoldierInfo.UpdateSoldierStatInfo();
		this.ReadySolList.Add(nkSoldierInfo.GetSolID(), nkSoldierInfo);
		return nkSoldierInfo;
	}

	public int GetCount()
	{
		return this.ReadySolList.Count;
	}

	public void DelSol(long solid)
	{
		this.ReadySolList.Remove(solid);
	}

	public Dictionary<long, NkSoldierInfo> GetList()
	{
		return this.ReadySolList;
	}

	public NkSoldierInfo IsHelpSol(long FriendPersonID)
	{
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current.GetFriendPersonID() == FriendPersonID)
			{
				return current;
			}
		}
		return null;
	}

	public int AddExpHelpsolCount()
	{
		int num = 0;
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current.AddHelpExp > 0L)
			{
				num++;
			}
		}
		return num;
	}

	public void UpdateSoldierInfo()
	{
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current.IsValid())
			{
				current.UpdateSoldierInfo();
			}
		}
	}

	public int GetSameSolNumFromSolPosType(eSOL_POSTYPE eSolPosType)
	{
		int num = 0;
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current.GetSolPosType() == (byte)eSolPosType)
			{
				num++;
			}
		}
		return num;
	}

	public int ReadySoliderCount()
	{
		int num = 0;
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current.GetSolID() > 0L)
			{
				if (current.GetSolPosType() != 6)
				{
					if (current.GetSolPosType() != 2)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public List<int> GetReadySolKindList()
	{
		List<int> list = new List<int>();
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current == null)
			{
				Debug.LogError("ERROR, NrMyCharInfo.cs, GetReadySolKindList(), pkSolinfo is Null");
			}
			else if (!list.Contains(current.GetCharKind()))
			{
				list.Add(current.GetCharKind());
			}
		}
		return list;
	}

	public List<int> GetMineBattlePossibleKindList()
	{
		List<NkSoldierInfo> mineBattlePossibleSolInfoList = this.GetMineBattlePossibleSolInfoList();
		List<int> list = new List<int>();
		foreach (NkSoldierInfo current in mineBattlePossibleSolInfoList)
		{
			if (current == null)
			{
				Debug.LogError("ERROR, NrMyCharInfo.cs, GetReadySolKindList(), pkSolinfo is Null");
			}
			else if (!list.Contains(current.GetCharKind()))
			{
				list.Add(current.GetCharKind());
			}
		}
		return list;
	}

	public List<NkSoldierInfo> GetMineBattlePossibleSolInfoList()
	{
		List<NkSoldierInfo> list = new List<NkSoldierInfo>();
		int mineMoneyFromSolPossibleLevel = BASE_MINE_DATA.GetMineMoneyFromSolPossibleLevel(SoldierBatch.MINE_INFO.m_nMineGrade);
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current == null)
			{
				Debug.LogError("ERROR, NrMyCharInfo.cs, GetMineBattlePossibleSolInfoList(), pkSolinfo is Null");
			}
			else if (current.GetSolPosType() != 2 && current.GetSolPosType() != 6)
			{
				if ((int)current.GetLevel() >= mineMoneyFromSolPossibleLevel)
				{
					if (!list.Contains(current))
					{
						list.Add(current);
					}
				}
			}
		}
		return list;
	}

	public Dictionary<long, NkSoldierInfo> GetReadyAllSolList()
	{
		if (this.ReadySolList == null)
		{
			return null;
		}
		return this.ReadySolList;
	}
}
