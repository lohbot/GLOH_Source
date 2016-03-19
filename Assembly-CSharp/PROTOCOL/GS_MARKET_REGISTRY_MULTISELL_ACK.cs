using System;

namespace PROTOCOL
{
	public class GS_MARKET_REGISTRY_MULTISELL_ACK
	{
		public int m_nResult;

		public long m_lSellItemAllMoney;

		public int m_nSellItemAllCount;

		public int[] m_byPosType;

		public int[] m_shPosItem;

		public int[] m_shSellNextItemNum;

		public GS_MARKET_REGISTRY_MULTISELL_ACK()
		{
			this.m_byPosType = new int[30];
			this.m_shPosItem = new int[30];
			this.m_shSellNextItemNum = new int[30];
		}
	}
}
