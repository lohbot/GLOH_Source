using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_LOAD_CHAR_NFY
	{
		public short WorldID;

		public long PersonID;

		public long Money;

		public int MapUnique;

		public long ActivityPoint;

		public long nServerTime;

		public int VipActivityAddTime;

		public long m_lLoginTime;

		public long m_CreateDate;

		public long m_TotalPlayTime;

		public NrCharPartInfo kPartInfo = new NrCharPartInfo();

		public byte NumTerritories;

		public short m_iBattleMapID;

		public long m_nEquipSellMoney;

		public int m_nPlunderRank;

		public short m_nColosseumGrade;

		public int m_nColosseumGradePoint;

		public short m_nColosseumOldGrade;

		public int m_nColosseumOldRank;

		public int m_nColosseumWinCount;

		public long m_nHeroPoint;

		public long m_nEquipPoint;

		public int i32HP_Auth;

		public byte m_i8RankingReward = 1;
	}
}
