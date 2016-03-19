using System;

namespace GAME
{
	public class NOTE_INFO
	{
		public long i64NoteID;

		public long i64PersonID;

		public int i32NoteType;

		public long i64Param_PersonID;

		public char[] szParam = new char[21];

		public long i64Param1;

		public long i64Param2;

		public long i64Param3;

		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"NOTE ID:",
				this.i64NoteID,
				"Name:",
				TKString.NEWString(this.szParam),
				":",
				(eNOTE_TYPE)this.i32NoteType,
				":",
				this.i64Param1,
				":",
				this.i64Param2,
				":",
				this.i64Param3
			});
		}
	}
}
