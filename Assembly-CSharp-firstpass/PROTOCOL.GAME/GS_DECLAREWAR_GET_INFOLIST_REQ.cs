using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	[StructLayout(LayoutKind.Sequential)]
	public class GS_DECLAREWAR_GET_INFOLIST_REQ
	{
		public short i16PageIndex;
	}
}
