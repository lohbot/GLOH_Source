using GAME;
using System;

namespace PROTOCOL
{
	public class GS_BUY_ITEM_ACK
	{
		public int m_nResult;

		public int m_nCharKind;

		public long m_lMoney;

		public int m_nItemNum;

		public ITEM m_cItem = new ITEM();
	}
}
