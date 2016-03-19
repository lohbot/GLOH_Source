using System;

public class QUEST_CONDITION
{
	public int i32QuestCode;

	public string szCodeTextKey = string.Empty;

	public long i64Param;

	public long i64ParamVal;

	public int nMapUnique;

	public int i32SectonUnique;

	public float fTargetPosX;

	public float fTargetPosZ;

	public int i32CharKind = -1;

	public QUEST_CONDITION()
	{
		this.Init();
	}

	public void Init()
	{
		this.i32QuestCode = 0;
		this.szCodeTextKey = string.Empty;
		this.i64Param = 0L;
		this.i64ParamVal = 0L;
		this.fTargetPosX = 0f;
		this.fTargetPosZ = 0f;
		this.i32CharKind = -1;
	}

	public void SetQuestCondition(QUEST_CONDITION cQuestCondition)
	{
		this.i32QuestCode = cQuestCondition.i32QuestCode;
		this.szCodeTextKey = cQuestCondition.szCodeTextKey;
		this.i64Param = cQuestCondition.i64Param;
		this.i64ParamVal = cQuestCondition.i64ParamVal;
		this.fTargetPosX = cQuestCondition.fTargetPosX;
		this.fTargetPosZ = cQuestCondition.fTargetPosZ;
		this.i32CharKind = cQuestCondition.i32CharKind;
	}
}
