using GAME;
using System;

namespace PROTOCOL
{
	public class GS_BOX_USE_ACK
	{
		public int m_nResult;

		public int m_lUnique;

		public int m_byPosType;

		public int m_shPosItem;

		public int m_nItemNum;

		public int[] m_naAddItemNum = new int[12];

		public ITEM[] m_caAddItem = new ITEM[12];

		public GS_BOX_USE_ACK()
		{
			for (int i = 0; i < 12; i++)
			{
				this.m_caAddItem[i] = new ITEM();
			}
		}
	}
}
