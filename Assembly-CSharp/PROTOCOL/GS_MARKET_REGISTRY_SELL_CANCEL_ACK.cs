using GAME;
using System;

namespace PROTOCOL
{
	public class GS_MARKET_REGISTRY_SELL_CANCEL_ACK
	{
		public int m_nResult;

		public long m_lMarketID;

		public ITEM m_cItem = new ITEM();
	}
}
