using System;

namespace PROTOCOL.GAME
{
	public class GS_QUEST_GET_CHAR_ACK
	{
		public int Result;

		public short i16CharUnique;

		public int i32CharKind;

		public byte ui8rCharKindSlot;

		public int nItemUnique;

		public int i32rPosType;

		public int i32rItemPos;

		public int i32rItemNum_Cur;

		public long i64rCharMoney_Cur;
	}
}
