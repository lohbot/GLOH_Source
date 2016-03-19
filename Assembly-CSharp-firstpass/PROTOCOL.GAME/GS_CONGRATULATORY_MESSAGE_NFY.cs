using System;

namespace PROTOCOL.GAME
{
	public class GS_CONGRATULATORY_MESSAGE_NFY
	{
		public short m_nMsgType;

		public long m_nPersonID;

		public char[] char_name = new char[21];

		public short level;

		public int m_nItemUnique;

		public int m_nItemNum;

		public int[] i32params = new int[6];
	}
}
