using GAME;
using System;

namespace PROTOCOL.WORLD
{
	public class WS_CHARLIST_ACK
	{
		public class NEW_CHARLIST_INFO
		{
			public long PersonID;

			public char[] szCharName = new char[21];

			public short Level;

			public NrCharBasePart kBasePart;

			public long SolID;

			public int CharKind;

			public long LastLoginTime;

			public NEW_CHARLIST_INFO()
			{
				this.PersonID = 0L;
				this.Level = 0;
				this.kBasePart = new NrCharBasePart();
				this.SolID = 0L;
				this.CharKind = 0;
				this.LastLoginTime = 0L;
			}
		}

		public int Result;

		public byte NumChars;
	}
}
