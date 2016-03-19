using System;

namespace PROTOCOL.GAME
{
	public class GS_QUEST_ACCEPT_ACK
	{
		public int Result;

		public long i64QuestID;

		public char[] strQuestUnique = new char[33];

		public int i32Grade;

		public ST_QUESTGOOD_CHECK[] kGood = new ST_QUESTGOOD_CHECK[3];

		public int i32ActionID;

		public int i32QuestGroupUnique;

		public long i64Time;

		public long i64LastTime;

		public GS_QUEST_ACCEPT_ACK()
		{
			this.kGood[0] = new ST_QUESTGOOD_CHECK();
			this.kGood[1] = new ST_QUESTGOOD_CHECK();
			this.kGood[2] = new ST_QUESTGOOD_CHECK();
			this.i32ActionID = 0;
			this.i32QuestGroupUnique = 0;
			this.i64Time = 0L;
			this.i64LastTime = 0L;
		}
	}
}
