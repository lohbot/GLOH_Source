using GAME;
using System;

namespace PROTOCOL.WORLD
{
	public class WS_CREATE_CHAR_ACK
	{
		public byte Result;

		public char[] szCharName = new char[21];

		public long PersonID;

		public long SolID;

		public int CharKind;

		public NrCharBasePart kBasePart;

		public WS_CREATE_CHAR_ACK()
		{
			this.Result = 1;
			this.SolID = 0L;
			this.CharKind = 0;
			this.kBasePart = new NrCharBasePart();
		}
	}
}
