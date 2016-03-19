using GAME;
using System;
using System.Collections.Generic;

public class NkReadySolList
{
	private Dictionary<long, NkSoldierInfo> ReadySolList = new Dictionary<long, NkSoldierInfo>();

	private List<byte> MilitaryList;

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

	public bool IsGuildWarApply(byte iMilitaryUnique)
	{
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current.GetSolPosType() == 7)
			{
				if (current.GetMilitaryUnique() == iMilitaryUnique)
				{
					return true;
				}
			}
		}
		return false;
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
						if (current.GetSolPosType() != 7)
						{
							num++;
						}
					}
				}
			}
		}
		return num;
	}

	public bool IsGuildWarApplyUser()
	{
		if (this.MilitaryList == null)
		{
			this.MilitaryList = new List<byte>();
		}
		this.MilitaryList.Clear();
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current.GetSolPosType() == 7)
			{
				bool flag = true;
				foreach (byte current2 in this.MilitaryList)
				{
					if (current2 == current.GetMilitaryUnique())
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.MilitaryList.Add(current.GetMilitaryUnique());
				}
			}
		}
		return this.MilitaryList.Count < (int)NrTSingleton<GuildWarManager>.Instance.GuildWarJoinCount;
	}

	public int GetGuildWarApplyMilitaryCount()
	{
		if (this.MilitaryList == null)
		{
			this.MilitaryList = new List<byte>();
		}
		this.MilitaryList.Clear();
		foreach (NkSoldierInfo current in this.ReadySolList.Values)
		{
			if (current.GetSolPosType() == 7)
			{
				bool flag = true;
				foreach (byte current2 in this.MilitaryList)
				{
					if (current2 == current.GetMilitaryUnique())
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.MilitaryList.Add(current.GetMilitaryUnique());
				}
			}
		}
		return this.MilitaryList.Count;
	}
}
