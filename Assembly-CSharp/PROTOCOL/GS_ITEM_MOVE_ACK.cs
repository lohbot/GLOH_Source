using GAME;
using System;

namespace PROTOCOL
{
	public class GS_ITEM_MOVE_ACK
	{
		public int m_nResult;

		public short CharUnique;

		public long m_nSrcSolID;

		public long m_nDestSolID;

		public byte m_nMoveType;

		public int m_bySrcPosType;

		public int m_byDestPosType;

		public int m_shPosItem;

		public int m_shDestPosItem;

		public ITEM m_cSrcItemInfo = new ITEM();

		public ITEM m_cDestItemInfo = new ITEM();
	}
}
