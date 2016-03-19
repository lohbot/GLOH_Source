using GAME;
using System;

namespace PROTOCOL
{
	public class GS_BUY_ITEM_REQ
	{
		public int m_nCharKind;

		public long m_lTotalPrice;

		public int m_nItemNum;

		public ITEM m_cItem = new ITEM();
	}
}
