using GAME;
using System;

namespace PROTOCOL
{
	public class GS_MARKET_BUY_ACK
	{
		public int m_nResult;

		public long m_lMoney;

		public int m_nItemUnique;

		public int m_byPosType;

		public int m_shPosItem;

		public ITEM m_cItem = new ITEM();

		public int m_nMode;
	}
}
