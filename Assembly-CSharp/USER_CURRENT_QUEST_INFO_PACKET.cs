using System;
using System.Runtime.InteropServices;

public class USER_CURRENT_QUEST_INFO_PACKET
{
	public long i64QuestID;

	public char[] strQuestUnique;

	public int i32Grade;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public long[] i64ParamVal = new long[3];

	public long i64QuestTime;

	public int i32QuestGroupUnique;

	public byte bFailed;

	public long i64LastTime;

	public USER_CURRENT_QUEST_INFO_PACKET()
	{
		this.strQuestUnique = new char[33];
		this.i64ParamVal[0] = 0L;
		this.i64ParamVal[1] = 0L;
		this.i64ParamVal[2] = 0L;
		this.i64QuestTime = 0L;
		this.i32QuestGroupUnique = 0;
		this.i64LastTime = 0L;
	}

	public void Init()
	{
		this.i64QuestID = 0L;
		this.strQuestUnique = null;
		this.i32Grade = 0;
		this.i64ParamVal[0] = 0L;
		this.i64ParamVal[1] = 0L;
		this.i64ParamVal[2] = 0L;
		this.i64QuestTime = 0L;
		this.i32QuestGroupUnique = 0;
		this.i64LastTime = 0L;
	}
}
