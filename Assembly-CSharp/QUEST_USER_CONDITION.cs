using System;

public class QUEST_USER_CONDITION
{
	public int i32QuestCode;

	public int bIsServerCheck;

	public long i64Param;

	public long i64ParamVal;

	public char[] cTemp = new char[129];

	public QUEST_USER_CONDITION()
	{
		this.Init();
	}

	public void Init()
	{
		this.i32QuestCode = 0;
		this.bIsServerCheck = 0;
		this.i64Param = 0L;
		this.i64ParamVal = 0L;
	}

	public void SetQuestUserCondition(QUEST_USER_CONDITION cQuestCondition)
	{
		this.i32QuestCode = cQuestCondition.i32QuestCode;
		this.bIsServerCheck = cQuestCondition.bIsServerCheck;
		this.i64Param = cQuestCondition.i64Param;
		this.i64ParamVal = cQuestCondition.i64ParamVal;
	}
}
