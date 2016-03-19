using System;

namespace PROTOCOL.GAME
{
	public class GS_NOTE_SET_ACK
	{
		public int i32Result;

		public long i64NoteID;

		public long i64PersonID;

		public int i32NoteType;

		public long i64Param_PersonID;

		public long i64Param1;

		public long i64Param2;

		public long i64Param3;

		public char[] szParam = new char[21];
	}
}
