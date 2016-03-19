using System;

namespace PROTOCOL.GAME
{
	public class GS_QUEST_OBJECT_COUNTINFO_ACK
	{
		public int nLoad;

		public int[] nSaveTpye = new int[50];

		public int[] nSaveIndex = new int[50];

		public int[] nSaveCharKind = new int[50];

		public int[] nSaveCount = new int[50];
	}
}
