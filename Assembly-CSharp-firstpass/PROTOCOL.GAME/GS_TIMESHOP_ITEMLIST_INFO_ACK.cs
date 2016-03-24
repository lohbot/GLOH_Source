using System;

namespace PROTOCOL.GAME
{
	public class GS_TIMESHOP_ITEMLIST_INFO_ACK
	{
		public int i32Result;

		public int i32ItemListCount;

		public short i16RefreshCount;

		public long i64NextRefreshTime;
	}
}
