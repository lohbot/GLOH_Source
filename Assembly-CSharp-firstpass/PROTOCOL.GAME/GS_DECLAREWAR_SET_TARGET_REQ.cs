using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	[StructLayout(LayoutKind.Sequential)]
	public class GS_DECLAREWAR_SET_TARGET_REQ
	{
		public long i64TargetGuildId;
	}
}
