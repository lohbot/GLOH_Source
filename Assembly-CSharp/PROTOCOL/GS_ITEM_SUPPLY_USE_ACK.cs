using System;

namespace PROTOCOL
{
	public class GS_ITEM_SUPPLY_USE_ACK
	{
		public int m_nResult;

		public int m_nItemUnique;

		public long m_nDestSolID;

		public int m_shCurrentItemNum;

		public int m_byPosType;

		public int m_shPosItem;

		public int m_shPara1;

		public int m_shPara2;

		public int[] m_nParaArray1 = new int[6];

		public int[] m_nParaArray2 = new int[6];

		public long m_ni64CurrnetMoney;
	}
}
