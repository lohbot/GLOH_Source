using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_FOLLOWCHAR_ACK
	{
		public short i16Result;

		public char[] Name = new char[21];

		public long i64CID;

		public short i16DunID;

		public int i32ChannelID;

		public int i32WorldID;

		public POS3D pos = new POS3D();
	}
}
