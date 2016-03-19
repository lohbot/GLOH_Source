using System;

namespace PROTOCOL.GAME
{
	public class GS_GMCOMMAND_PWTIME_NFY
	{
		public class GMCOMMAND_PWTIME_WORKS_INFO
		{
			public long i64TID;

			public byte i8CommandType;

			public int i32CharKind;

			public long i64CompleteTime;
		}

		public char[] strName_GM = new char[21];

		public int i32WorksNum;
	}
}
