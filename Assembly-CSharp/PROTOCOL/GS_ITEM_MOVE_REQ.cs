using System;

namespace PROTOCOL
{
	public class GS_ITEM_MOVE_REQ
	{
		public long m_nSrcSolID;

		public long m_nDestSolID;

		public int m_nMoveType;

		public long m_nSrcItemID;

		public int m_nSrcItemPos;

		public long m_nDestItemID;

		public int m_nDestItemPos;
	}
}
