using System;

namespace PROTOCOL.WORLD
{
	public class WS_CHANGE_CHAR_NAME_REQ
	{
		public char[] szCharName = new char[21];

		public char[] szChangeName = new char[21];

		public long nPersonID;
	}
}
