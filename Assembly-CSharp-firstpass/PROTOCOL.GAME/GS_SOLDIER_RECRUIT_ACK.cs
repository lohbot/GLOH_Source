using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIER_RECRUIT_ACK
	{
		public int Result;

		public byte i8bPosted;

		public int SolCount;

		public int SolSubDataCount;

		public int RecruitType;

		public ITEM kItem = new ITEM();

		public byte ui8VoucherType;

		public long i64ItemMallID;

		public long i64RefreshTime;
	}
}
