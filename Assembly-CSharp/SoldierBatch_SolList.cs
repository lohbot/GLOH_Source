using GAME;
using System;
using System.Collections.Generic;

public class SoldierBatch_SolList
{
	private const int SHOW_COMBATPOWER_LEVEL = 50;

	private static int ComparePower(NkSoldierInfo a, NkSoldierInfo b)
	{
		long solSubData = a.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER);
		return b.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER).CompareTo(solSubData);
	}

	private static int CompareLevel(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetLevel().CompareTo(a.GetLevel());
	}

	private static int CompareCombatPowerDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetCombatPower().CompareTo(a.GetCombatPower());
	}

	private static int CompareFriendSolLevel(USER_FRIEND_INFO a, USER_FRIEND_INFO b)
	{
		return b.FriendHelpSolInfo.iSolLevel.CompareTo(a.FriendHelpSolInfo.iSolLevel);
	}

	public static bool IsNotExcludeSol(NkSoldierInfo pkSolinfo, eSOLDIER_BATCH_MODE eSoldierBatchMode)
	{
		if (pkSolinfo == null || pkSolinfo.GetSolID() <= 0L)
		{
			return true;
		}
		if (eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			if (pkSolinfo.GetSolPosType() == 2)
			{
				return true;
			}
			if (pkSolinfo.GetSolPosType() == 6)
			{
				return true;
			}
		}
		if (eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION)
		{
			if (pkSolinfo.IsAtbCommonFlag(16L))
			{
				return true;
			}
			if (pkSolinfo.GetLevel() < NrTSingleton<ContentsLimitManager>.Instance.NewExplorationLimitLevel())
			{
				return true;
			}
		}
		else if (eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			int mineMoneyFromSolPossibleLevel = BASE_MINE_DATA.GetMineMoneyFromSolPossibleLevel(SoldierBatch.MINE_INFO.m_nMineGrade);
			if ((int)pkSolinfo.GetLevel() < mineMoneyFromSolPossibleLevel)
			{
				return true;
			}
		}
		else if (eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade((byte)SoldierBatch.EXPEDITION_INFO.m_eExpeditionGrade);
			if (expeditionDataFromGrade != null && pkSolinfo.GetLevel() < expeditionDataFromGrade.SolPossiblelevel)
			{
				return true;
			}
		}
		return false;
	}

	public static List<int> GetSolKindList(eSOLDIER_BATCH_MODE eSoldierBatchMode)
	{
		List<int> list = new List<int>();
		List<NkSoldierInfo> solList = SoldierBatch_SolList.GetSolList(eSoldierBatchMode);
		if (solList == null)
		{
			return list;
		}
		foreach (NkSoldierInfo current in solList)
		{
			list.Add(current.GetCharKind());
		}
		return list;
	}

	public static List<NkSoldierInfo> GetSolList(eSOLDIER_BATCH_MODE eSoldierBatchMode)
	{
		List<NkSoldierInfo> list = new List<NkSoldierInfo>();
		List<NkSoldierInfo> list2 = new List<NkSoldierInfo>();
		List<NkSoldierInfo> list3 = new List<NkSoldierInfo>();
		int num = 0;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return new List<NkSoldierInfo>();
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		if (soldierList == null)
		{
			return new List<NkSoldierInfo>();
		}
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
			{
				break;
			}
			if (!SoldierBatch_SolList.IsNotExcludeSol(nkSoldierInfo, eSoldierBatchMode))
			{
				if (nkSoldierInfo.IsLeader())
				{
					list.Add(nkSoldierInfo);
				}
				else
				{
					NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkSoldierInfo.GetCharKind());
					if (charKindInfo != null && nkSoldierInfo.GetLevel() >= 50)
					{
						list2.Add(nkSoldierInfo);
					}
					else
					{
						list3.Add(nkSoldierInfo);
					}
				}
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return new List<NkSoldierInfo>();
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (!SoldierBatch_SolList.IsNotExcludeSol(current, eSoldierBatchMode))
			{
				if (current.IsLeader())
				{
					list.Add(current);
				}
				else
				{
					NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.GetCharKind());
					if (charKindInfo2 != null && current.GetLevel() >= 50)
					{
						list2.Add(current);
					}
					else
					{
						list3.Add(current);
					}
				}
				num++;
			}
		}
		list2.Sort(new Comparison<NkSoldierInfo>(SoldierBatch_SolList.ComparePower));
		list3.Sort(new Comparison<NkSoldierInfo>(SoldierBatch_SolList.CompareLevel));
		for (int j = 0; j < list2.Count; j++)
		{
			list.Add(list2[j]);
		}
		for (int k = 0; k < list3.Count; k++)
		{
			list.Add(list3[k]);
		}
		return list;
	}

	public static List<USER_FRIEND_INFO> GetFriendSolList()
	{
		List<USER_FRIEND_INFO> list = new List<USER_FRIEND_INFO>();
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendInfoValues())
		{
			if (uSER_FRIEND_INFO.FriendHelpSolInfo.i64HelpSolID > 0L)
			{
				if (uSER_FRIEND_INFO.ui8HelpUse < 1)
				{
					list.Add(uSER_FRIEND_INFO);
				}
			}
		}
		list.Sort(new Comparison<USER_FRIEND_INFO>(SoldierBatch_SolList.CompareFriendSolLevel));
		return list;
	}

	public static int GetEmptyArrayIndex(clTempBattlePos[] battlePos, int maxCount)
	{
		for (int i = 0; i < maxCount; i++)
		{
			if (battlePos[i] == null || battlePos[i].m_nBattlePos < 0 || battlePos[i].m_nSolID <= 0L)
			{
				return i;
			}
		}
		return -1;
	}

	public static int GetSolCount(clTempBattlePos[] battlePos, int maxCount)
	{
		int num = 0;
		for (int i = 0; i < maxCount; i++)
		{
			if (battlePos[i] != null && battlePos[i].m_nBattlePos >= 0 && battlePos[i].m_nSolID > 0L)
			{
				num++;
			}
		}
		return num;
	}

	public static clTempBattlePos[] GetAutoBatchPos(int maxCount, eSOLDIER_BATCH_MODE eSoldierBatchMode, clTempBattlePos[] battlePos = null)
	{
		if (battlePos == null)
		{
			battlePos = new clTempBattlePos[maxCount];
		}
		int num = SoldierBatch_SolList.GetSolCount(battlePos, maxCount);
		if (num >= maxCount)
		{
			return battlePos;
		}
		List<NkSoldierInfo> solList = SoldierBatch_SolList.GetSolList(eSoldierBatchMode);
		if (solList.Count == 0)
		{
			return null;
		}
		foreach (NkSoldierInfo current in solList)
		{
			if (num >= maxCount)
			{
				clTempBattlePos[] result = battlePos;
				return result;
			}
			int emptyArrayIndex = SoldierBatch_SolList.GetEmptyArrayIndex(battlePos, maxCount);
			if (emptyArrayIndex < 0)
			{
				clTempBattlePos[] result = battlePos;
				return result;
			}
			bool flag = true;
			for (int i = 0; i < maxCount; i++)
			{
				if (battlePos[i] != null)
				{
					if (battlePos[i].m_nSolID == current.GetSolID() || battlePos[i].m_nCharKind == current.GetCharKind())
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				byte b = 0;
				while ((int)b < maxCount)
				{
					bool flag2 = true;
					for (int j = 0; j < maxCount; j++)
					{
						if (battlePos[j] != null && battlePos[j].m_nBattlePos == b)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						break;
					}
					b += 1;
				}
				battlePos[emptyArrayIndex] = new clTempBattlePos();
				battlePos[emptyArrayIndex].m_nSolID = current.GetSolID();
				battlePos[emptyArrayIndex].m_nCharKind = current.GetCharKind();
				battlePos[emptyArrayIndex].m_nBattlePos = b;
				num++;
			}
		}
		return battlePos;
	}
}
