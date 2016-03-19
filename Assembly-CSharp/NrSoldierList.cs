using GAME;
using System;

public class NrSoldierList
{
	public NkSoldierInfo[] m_kSolInfo;

	public NrSoldierList()
	{
		this.m_kSolInfo = new NkSoldierInfo[6];
		this.Init();
	}

	public void Init()
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kSolInfo[i] == null)
			{
				this.m_kSolInfo[i] = new NkSoldierInfo();
			}
			this.m_kSolInfo[i].Init();
		}
	}

	public void SetBaseCharID(int charid)
	{
		for (int i = 0; i < 6; i++)
		{
			this.m_kSolInfo[i].SetBaseCharID(charid);
		}
	}

	public void Set(NrSoldierList pkList)
	{
		for (int i = 0; i < 6; i++)
		{
			if (pkList.m_kSolInfo[i].GetCharKind() <= 0 || pkList.m_kSolInfo[i].GetCharKind() >= 5000)
			{
				this.m_kSolInfo[i].Init();
			}
			else
			{
				this.m_kSolInfo[i].Set(pkList.m_kSolInfo[i]);
			}
		}
	}

	public NkSoldierInfo SetSoldierInfo(int solindex, NkSoldierInfo pkSolinfo)
	{
		if (solindex < 0 || solindex >= 6)
		{
			return null;
		}
		this.m_kSolInfo[solindex].Set(pkSolinfo);
		return this.GetSoldierInfo(solindex);
	}

	public NkSoldierInfo SetSoldierInfo(int solindex, SOLDIER_INFO pkSolinfoInfo)
	{
		if (solindex < 0 || solindex >= 6)
		{
			return null;
		}
		this.m_kSolInfo[solindex].Set(pkSolinfoInfo);
		return this.GetSoldierInfo(solindex);
	}

	public NkSoldierInfo GetSoldierInfoBySolID(long solid)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kSolInfo[i].GetSolID() == solid)
			{
				return this.m_kSolInfo[i];
			}
		}
		return null;
	}

	public NkSoldierInfo GetSoldierInfoByKind(int charKind)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kSolInfo[i].GetCharKind() == charKind)
			{
				return this.m_kSolInfo[i];
			}
		}
		return null;
	}

	public NkSoldierInfo GetSoldierInfo(int solindex)
	{
		if (solindex < 0 || solindex >= 6)
		{
			return null;
		}
		return this.m_kSolInfo[solindex];
	}

	public void Remove(long solid)
	{
		NkSoldierInfo soldierInfoBySolID = this.GetSoldierInfoBySolID(solid);
		if (soldierInfoBySolID != null)
		{
			soldierInfoBySolID.Init();
		}
	}

	public CHARKIND_ATTACKINFO GetSoldierAttackInfo(int solindex)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo(solindex);
		if (soldierInfo == null)
		{
			return null;
		}
		return soldierInfo.GetAttackInfo();
	}

	public void Item_Equipment(long solID, ITEM a_cItem)
	{
		NkSoldierInfo soldierInfoBySolID = this.GetSoldierInfoBySolID(solID);
		if (soldierInfoBySolID != null)
		{
			soldierInfoBySolID.EquipmentItem(a_cItem);
		}
	}

	public long CalcTotal_CombatPower()
	{
		long num = 0L;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo nkSoldierInfo = this.m_kSolInfo[i];
			if (nkSoldierInfo.IsValid())
			{
				if (nkSoldierInfo.GetSolPosType() == 1)
				{
					num += nkSoldierInfo.GetCombatPower();
				}
			}
		}
		return num;
	}

	public void SetBattleSol_CombatPower()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		kMyCharInfo.DepolyCombatPower = this.CalcTotal_CombatPower();
	}

	public byte GetBattleSoldierCount()
	{
		byte b = 0;
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kSolInfo[i].IsValid())
			{
				NkSoldierInfo nkSoldierInfo = this.m_kSolInfo[i];
				if (nkSoldierInfo.GetSolPosType() == 1)
				{
					b += 1;
				}
			}
		}
		return b;
	}

	public void UpdateSoldierInfo()
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kSolInfo[i].IsValid())
			{
				this.m_kSolInfo[i].UpdateSoldierInfo();
			}
		}
	}

	public void AddHP(int addHP, int[] HP)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kSolInfo[i].IsValid())
			{
				this.m_kSolInfo[i].AddHP(addHP, 0);
				if (this.m_kSolInfo[i].GetHP() != HP[i])
				{
					this.m_kSolInfo[i].SetHP(HP[i], 0);
				}
			}
		}
	}

	public int GetSolItemRankCount(int rankItemUnique, int rankValue)
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kSolInfo[i].IsValid())
			{
				if (NrTSingleton<ItemManager>.Instance.GetItemInfo(rankItemUnique) != null)
				{
					for (int j = 0; j < 6; j++)
					{
						NkItem equipItem = this.m_kSolInfo[i].GetEquipItemInfo().GetEquipItem(j);
						if (equipItem != null)
						{
							if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(rankItemUnique) == NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(equipItem.GetItemUnique()))
							{
								if (rankValue <= equipItem.GetRank())
								{
									num++;
								}
							}
						}
					}
				}
			}
		}
		return num;
	}

	public int GetUpgradeBattleSkillNum()
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kSolInfo[i].IsValid())
			{
				num += this.m_kSolInfo[i].GetUpgradeBattleSkillNum();
			}
		}
		return num;
	}

	public NkSoldierInfo IsHelpSol(long FriendPersonID)
	{
		NkSoldierInfo[] kSolInfo = this.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.GetFriendPersonID() == FriendPersonID)
			{
				return nkSoldierInfo;
			}
		}
		return null;
	}

	public int AddExpHelpsolCount()
	{
		int num = 0;
		NkSoldierInfo[] kSolInfo = this.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.AddHelpExp > 0L)
			{
				num++;
			}
		}
		return num;
	}

	public void SetSolSubData(SOLDIER_SUBDATA SolSubData)
	{
		if (SolSubData == null)
		{
			return;
		}
		NkSoldierInfo soldierInfoBySolID = this.GetSoldierInfoBySolID(SolSubData.nSolID);
		if (soldierInfoBySolID == null)
		{
			return;
		}
		soldierInfoBySolID.SetSolSubData(SolSubData.nSubDataType, SolSubData.nSubDataValue);
	}
}
