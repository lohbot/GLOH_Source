using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIERS_UPGRADE_REQ
	{
		public long i64BaseSolID;

		public byte i8Cnt;

		public long[] i64SubSolID = new long[50];
	}
}
