using System;

namespace PROTOCOL.GAME
{
	public class GS_QUEST_GROUP_RESET_ACK
	{
		public int Result;

		public int i32GroupUnique;

		public byte bCleard;

		public int i32CurGrade;

		public byte[] byReset = new byte[25];

		public long i64CurrentMoney;

		public long i64QuestID;

		public char[] strQuestUnique = new char[33];

		public int i32Grade;

		public ST_QUESTGOOD_CHECK[] kGood = new ST_QUESTGOOD_CHECK[3];

		public int i32ActionID;

		public long i64Time;

		public long i64LastTime;

		public GS_QUEST_GROUP_RESET_ACK()
		{
			this.kGood[0] = new ST_QUESTGOOD_CHECK();
			this.kGood[1] = new ST_QUESTGOOD_CHECK();
			this.kGood[2] = new ST_QUESTGOOD_CHECK();
			this.i64QuestID = 0L;
			this.i32Grade = 0;
			this.i32ActionID = 0;
			this.i64Time = 0L;
			this.i64LastTime = 0L;
		}
	}
}
