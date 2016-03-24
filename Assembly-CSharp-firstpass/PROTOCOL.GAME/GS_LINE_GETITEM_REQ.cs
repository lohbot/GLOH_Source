using System;

namespace PROTOCOL.GAME
{
	public class GS_LINE_GETITEM_REQ
	{
		public char[] szUserID = new char[256];

		public char[] szProductID = new char[21];

		public char[] szOrderID = new char[33];
	}
}
