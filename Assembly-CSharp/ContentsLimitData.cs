using PROTOCOL;
using System;
using System.Collections.Generic;

public class ContentsLimitData
{
	private List<CONTENTSLIMIT_DATA> m_LimitDataList = new List<CONTENTSLIMIT_DATA>();

	public void Clear()
	{
		this.m_LimitDataList.Clear();
	}

	public void AddLimitData(CONTENTSLIMIT_DATA Data)
	{
		this.m_LimitDataList.Add(Data);
	}

	public bool IsQuestAccept(int iQuestGroupUnique)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if ((long)iQuestGroupUnique == this.m_LimitDataList[i].i64Param[j])
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsWarpMap(int iMapIndex)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if ((long)iMapIndex == this.m_LimitDataList[i].i64Param[j])
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public int GetBabelTowerLastFloor(short nFloorType = 1)
	{
		int num = 0;
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (nFloorType == 2)
				{
					if ((long)num <= this.m_LimitDataList[i].i64Param[1])
					{
						num = (int)this.m_LimitDataList[i].i64Param[1];
					}
				}
				else if ((long)num <= this.m_LimitDataList[i].i64Param[0])
				{
					num = (int)this.m_LimitDataList[i].i64Param[0];
				}
			}
		}
		if (0 < num)
		{
			num--;
		}
		return num;
	}

	public bool IsShopTab(int iIndex)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if ((long)iIndex == this.m_LimitDataList[i].i64Param[j])
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsShopProduct(long lItemIDX)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (lItemIDX == this.m_LimitDataList[i].i64Param[j])
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsWorldMapMove(int iMoveIndex)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if ((long)iMoveIndex == this.m_LimitDataList[i].i64Param[j])
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsMineApply(short iLevel)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if ((long)iLevel < this.m_LimitDataList[i].i64Param[j])
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsValidMineGrade(byte bMineGrade)
	{
		return this.m_LimitDataList[0].i64Param[1] >= (long)bMineGrade;
	}

	public bool IsBlueStacksUser()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool IsExploration()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsSupporter()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsReincarnation()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsSolCharKindinfo(int i32CharKindInfo)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if ((long)i32CharKindInfo == this.m_LimitDataList[i].i64Param[j])
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsSoldierRecruit(int i32CharKind)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if ((long)i32CharKind == this.m_LimitDataList[i].i64Param[j])
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool IsGuildBoss()
	{
		return this.m_LimitDataList.Count <= 0 || this.m_LimitDataList[0].i64Param[0] <= 0L;
	}

	public short GetGuildBossLastFloor()
	{
		if (this.m_LimitDataList.Count > 0 && this.m_LimitDataList[0].i64Param[1] > 0L)
		{
			return (short)this.m_LimitDataList[0].i64Param[1];
		}
		return 0;
	}

	public bool IsAuctionUse()
	{
		return this.m_LimitDataList.Count <= 0 || this.m_LimitDataList[0].i64Param[0] <= 0L;
	}

	public short GetAuctionUseLevel()
	{
		if (this.m_LimitDataList.Count > 0 && this.m_LimitDataList[0].i64Param[1] > 0L)
		{
			return (short)this.m_LimitDataList[0].i64Param[1];
		}
		return 0;
	}

	public int GetDLGSolRecruit()
	{
		if (0 < this.m_LimitDataList.Count)
		{
			return (int)this.m_LimitDataList[0].i64Param[0];
		}
		return 0;
	}

	public bool IsTreasure()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (this.m_LimitDataList[i].i64Param[j] == 1L)
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool IsInfiBattle()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (this.m_LimitDataList[i].i64Param[j] == 1L)
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool IsCouponUse()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsHP_Auth()
	{
		return this.m_LimitDataList.Count <= 0 || this.m_LimitDataList[0].i64Param[2] <= 0L;
	}

	public bool IsPointExchage()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsTicketSell(int iCharKind)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if ((long)iCharKind == this.m_LimitDataList[i].i64Param[j])
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsXpsPromotion()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsAwakeningUse()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsElementKind(int i32CharKind)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if ((long)i32CharKind == this.m_LimitDataList[i].i64Param[j])
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsNewColosseumSupport()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool IsBountyHunt()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool IsAlchemy()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsSolGuide_Season(int i32Season)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if ((long)i32Season == this.m_LimitDataList[i].i64Param[j])
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool IsTradeCaralyst()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (this.m_LimitDataList[i].i64Param[j] == 1L)
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool IsUseCaralyst()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (this.m_LimitDataList[i].i64Param[j] == 1L)
				{
					return false;
				}
			}
		}
		return true;
	}

	public short GetLimitLevel(long i64atbtype)
	{
		int num;
		if (i64atbtype == 1L)
		{
			num = 0;
		}
		else
		{
			if (i64atbtype != 2L)
			{
				return 0;
			}
			num = 1;
		}
		if (this.m_LimitDataList[0].i64Param[num] > 0L)
		{
			return (short)this.m_LimitDataList[0].i64Param[num];
		}
		return 0;
	}

	public bool IsNPCLimit(int i32CharKind)
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (i32CharKind == (int)this.m_LimitDataList[i].i64Param[j])
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsFaceBookLimit()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if ((int)this.m_LimitDataList[i].i64Param[j] != 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsExpeditionLimit()
	{
		return this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public bool IsExpeditionLevel(int iLevel)
	{
		return (long)iLevel >= this.m_LimitDataList[0].i64Param[1];
	}

	public int ExpeditionGradeLimit()
	{
		return (int)this.m_LimitDataList[0].i64Param[2];
	}

	public bool IsNewGuildWarLimit()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public bool IsAgitLimit()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[1] == 1L;
	}

	public bool IsGuildWarExchangeLimit()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[2] != 1L;
	}

	public bool IsExchangeJewelry()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					if (this.m_LimitDataList[i].i64Param[j] == 1L)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public int GetLimitAdventure()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (0L < this.m_LimitDataList[i].i64Param[j])
				{
					return (int)this.m_LimitDataList[i].i64Param[j];
				}
			}
		}
		return 0;
	}

	public bool IsHeroBattle()
	{
		for (int i = 0; i < this.m_LimitDataList.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (this.m_LimitDataList[i].i64Param[j] == 1L)
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool IsVoucherLimit()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public bool IsVipExp()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public bool IsLegend()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public short GetLimitSolGrade()
	{
		if (0 >= this.m_LimitDataList.Count)
		{
			return 0;
		}
		if (0L <= this.m_LimitDataList[0].i64Param[0])
		{
			return (short)(this.m_LimitDataList[0].i64Param[0] - 1L);
		}
		return 0;
	}

	public bool IsShowFriendInviteButton()
	{
		return 0 >= this.m_LimitDataList.Count || this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public bool IsTutorialBattleStart()
	{
		return 0 >= this.m_LimitDataList.Count || this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public bool IsExchangeMythicSol()
	{
		return this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public bool IsTranscendence()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public bool IsLegendHire()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public bool IsExtract()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public bool IsQuestTalkSkip()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public bool IsLineFriendInviteButton()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public bool IsMythRaidOn()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public bool IsRateUrl()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public bool IsChallenge()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public bool IsTimeShop()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public bool IsAttend()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] != 1L;
	}

	public short Attend_LastGroup(int a_nParam)
	{
		if (this.m_LimitDataList.Count > 0 && this.m_LimitDataList[0].i64Param[a_nParam] > 0L)
		{
			return (short)this.m_LimitDataList[0].i64Param[a_nParam];
		}
		return 0;
	}

	public bool IsItemNormalSkillBlock()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public bool IsItemLevelCheckBlock()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[1] == 1L;
	}

	public bool IsItemEvolution(bool ExMode = false)
	{
		if (0 >= this.m_LimitDataList.Count)
		{
			return false;
		}
		if (ExMode)
		{
			if (this.m_LimitDataList[0].i64Param[1] == 1L)
			{
				return false;
			}
		}
		else if (this.m_LimitDataList[0].i64Param[0] == 1L)
		{
			return false;
		}
		return true;
	}

	public bool IsNewExplorationLimit()
	{
		return this.m_LimitDataList[0].i64Param[0] >= 1L;
	}

	public short NewExplorationLimitLevel()
	{
		return (short)this.m_LimitDataList[0].i64Param[1];
	}

	public bool IsUseWillSpend()
	{
		return this.m_LimitDataList[0].i64Param[0] == 0L;
	}

	public bool IsDailyDungeonLimit()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public bool IsCostumeLimit()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public bool IsBattleStopLimit()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}

	public bool IsMineLimit()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[2] == 1L;
	}

	public bool IsMythEvolutionLimit()
	{
		return 0 < this.m_LimitDataList.Count && this.m_LimitDataList[0].i64Param[0] == 1L;
	}
}
