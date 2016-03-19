using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_QUEST_COMPLETE_REQ
	{
		public char[] strQuestUnique = new char[33];

		public ITEM[] CompleteItem = new ITEM[3];

		public int i32Falg;

		public GS_QUEST_COMPLETE_REQ()
		{
			this.CompleteItem[0] = new ITEM();
			this.CompleteItem[1] = new ITEM();
			this.CompleteItem[2] = new ITEM();
		}
	}
}
