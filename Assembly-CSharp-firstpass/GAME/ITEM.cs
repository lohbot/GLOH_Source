using System;

namespace GAME
{
	public class ITEM
	{
		public long m_nItemID;

		public int m_nItemUnique;

		public int m_nItemNum;

		public int m_nEndTime;

		public int m_nRank;

		public int m_nDurability;

		public int m_nLock;

		public int m_nPosType;

		public int m_nItemPos;

		public int[] m_nOption = new int[10];

		public void Init()
		{
			this.m_nItemID = 0L;
			this.m_nItemUnique = 0;
			this.m_nItemNum = 0;
			this.m_nEndTime = 0;
			this.m_nRank = 0;
			this.m_nDurability = 0;
			this.m_nLock = 0;
			this.m_nPosType = 0;
			this.m_nItemPos = 0;
			for (int i = 0; i < 10; i++)
			{
				this.m_nOption[i] = 0;
			}
		}

		public bool IsValid()
		{
			return this.m_nItemUnique > 0 && this.m_nItemNum > 0;
		}

		public bool IsLock()
		{
			return this.m_nLock != 0;
		}

		public bool IsBreak()
		{
			return this.m_nPosType >= 1 && this.m_nPosType <= 4 && this.m_nDurability <= 0;
		}

		public void Set(ITEM info)
		{
			this.m_nItemID = info.m_nItemID;
			this.m_nItemUnique = info.m_nItemUnique;
			this.m_nItemNum = info.m_nItemNum;
			this.m_nEndTime = info.m_nEndTime;
			this.m_nRank = info.m_nRank;
			this.m_nDurability = info.m_nDurability;
			this.m_nLock = info.m_nLock;
			this.m_nPosType = info.m_nPosType;
			this.m_nItemPos = info.m_nItemPos;
			for (byte b = 0; b < 10; b += 1)
			{
				this.m_nOption[(int)b] = info.m_nOption[(int)b];
			}
		}

		public int GetTradeCount()
		{
			return this.m_nOption[7];
		}

		public eITEM_RANK_TYPE GetRank()
		{
			return (eITEM_RANK_TYPE)this.m_nOption[2];
		}

		public string GetRankImage()
		{
			string result = string.Empty;
			eITEM_RANK_TYPE rank = this.GetRank();
			if (rank == eITEM_RANK_TYPE.ITEM_RANK_SS)
			{
				result = "Win_I_FrameSS";
			}
			else if (rank == eITEM_RANK_TYPE.ITEM_RANK_S)
			{
				result = "Win_I_FrameS";
			}
			else if (rank == eITEM_RANK_TYPE.ITEM_RANK_A)
			{
				result = "Win_I_FrameA";
			}
			else if (rank == eITEM_RANK_TYPE.ITEM_RANK_B)
			{
				result = "Win_I_FrameB";
			}
			else if (rank == eITEM_RANK_TYPE.ITEM_RANK_C)
			{
				result = "Win_I_FrameC";
			}
			else if (rank == eITEM_RANK_TYPE.ITEM_RANK_D)
			{
				result = "Win_I_FrameD";
			}
			return result;
		}
	}
}
