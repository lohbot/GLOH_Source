using GAME;
using System;

namespace PROTOCOL.WORLD
{
	public class WS_CREATE_CHAR_REQ
	{
		public char[] szCharName = new char[21];

		public char[] szSupporterName = new char[21];

		public byte ui8Country;

		public int i32CharKind;

		public NrCharBasePart kBasePart;

		public WS_CREATE_CHAR_REQ()
		{
			this.ui8Country = 2;
			this.i32CharKind = 0;
			this.kBasePart = new NrCharBasePart();
		}
	}
}
