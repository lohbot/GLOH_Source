using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	[StructLayout(LayoutKind.Sequential)]
	public class GS_DECLAREWAR_CANCEL_ACK
	{
		public int i32Result;

		public long i64GuildID;

		public char[] strEnemyGuildName = new char[11];

		public byte bDaclareDay;

		public byte bDaclareHour;

		public byte bDaclareMin;

		public byte bDeclareWarStartDay;

		public byte bDeclareWarStartHour;

		public byte bDeclareWarStartMin;
	}
}
