using System;

namespace PROTOCOL
{
	public class GS_MARKET_REGISTRY_MULTISELL_REQ
	{
		public int[] m_nItemUnique;

		public int[] m_byPosType;

		public int[] m_shPosItem;

		public int[] m_shSellItemNum;

		public GS_MARKET_REGISTRY_MULTISELL_REQ()
		{
			this.m_nItemUnique = new int[30];
			this.m_byPosType = new int[30];
			this.m_shPosItem = new int[30];
			this.m_shSellItemNum = new int[30];
		}
	}
}
