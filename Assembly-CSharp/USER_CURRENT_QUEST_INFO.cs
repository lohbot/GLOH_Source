using System;
using System.Runtime.InteropServices;

public class USER_CURRENT_QUEST_INFO
{
	public long i64QuestID;

	public string strQuestUnique;

	public int i32Grade;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public long[] i64ParamVal = new long[3];

	public long i64QuestTime;

	public int i32QuestGroupUnique;

	public byte bFailed;

	public long i64LastTime;

	public USER_CURRENT_QUEST_INFO()
	{
		this.strQuestUnique = string.Empty;
		this.i64ParamVal[0] = 0L;
		this.i64ParamVal[1] = 0L;
		this.i64ParamVal[2] = 0L;
		this.i64QuestTime = 0L;
		this.i32QuestGroupUnique = 0;
		this.bFailed = 0;
		this.i64LastTime = 0L;
	}

	public void Init()
	{
		this.i64QuestID = 0L;
		this.strQuestUnique = string.Empty;
		this.i32Grade = 0;
		this.i64ParamVal[0] = 0L;
		this.i64ParamVal[1] = 0L;
		this.i64ParamVal[2] = 0L;
		this.i64QuestTime = 0L;
		this.i32QuestGroupUnique = 0;
		this.bFailed = 0;
		this.i64LastTime = 0L;
	}

	public void SetUserCurrentQuestInfo(USER_CURRENT_QUEST_INFO_PACKET cUserCurrentQuestInfo)
	{
		this.strQuestUnique = TKString.NEWString(cUserCurrentQuestInfo.strQuestUnique);
		this.i64QuestID = cUserCurrentQuestInfo.i64QuestID;
		this.i64ParamVal[0] = cUserCurrentQuestInfo.i64ParamVal[0];
		this.i64ParamVal[1] = cUserCurrentQuestInfo.i64ParamVal[1];
		this.i64ParamVal[2] = cUserCurrentQuestInfo.i64ParamVal[2];
		this.i64QuestTime = cUserCurrentQuestInfo.i64QuestTime;
		this.i32QuestGroupUnique = cUserCurrentQuestInfo.i32QuestGroupUnique;
		this.bFailed = cUserCurrentQuestInfo.bFailed;
		this.i64LastTime = cUserCurrentQuestInfo.i64LastTime;
	}
}
