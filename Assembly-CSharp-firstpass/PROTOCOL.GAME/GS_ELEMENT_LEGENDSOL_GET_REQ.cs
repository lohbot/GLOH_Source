using System;

namespace PROTOCOL.GAME
{
	public class GS_ELEMENT_LEGENDSOL_GET_REQ
	{
		public long i64PersonID;

		public int i32CharKind;

		public long[] i64SolID = new long[5];
	}
}
