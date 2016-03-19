using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	public class GS_UPDATE_COMPLETED_QUEST_NFY
	{
		public int Result;

		public int i32QuesGroupUnique;

		public byte bCleared;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
		public byte[] byCompleteQuest = new byte[25];
	}
}
