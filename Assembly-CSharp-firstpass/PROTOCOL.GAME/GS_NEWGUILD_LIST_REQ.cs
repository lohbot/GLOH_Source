using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_LIST_REQ
	{
		public short i16CurPageNum;

		public char[] strMasterName = new char[11];

		public char[] strGuildName = new char[11];

		public byte i8SortType;
	}
}
