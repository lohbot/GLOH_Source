using System;

namespace PROTOCOL.GAME
{
	public class GS_QUEST_GET_CHAR_REQ
	{
		public char[] strQuestUnique = new char[33];

		public int i32CharKind;

		public int bItemType;

		public int nItemPos;

		public int i32ItemNum;

		public long i64Money;
	}
}
