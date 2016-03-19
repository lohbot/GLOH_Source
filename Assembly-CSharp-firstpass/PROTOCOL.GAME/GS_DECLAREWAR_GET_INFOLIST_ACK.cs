using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	[StructLayout(LayoutKind.Sequential)]
	public class GS_DECLAREWAR_GET_INFOLIST_ACK
	{
		public short i16PageIndex;

		public short i16PageMax;

		public long i64TargetGuild;

		public short i16Count;

		public bool bIsEndTime;
	}
}
