using System;

public class NrQuestParamNfyTemp
{
	public string strQuestUnique = string.Empty;

	public int i32Code;

	public QUEST_CONDITION[] stCondition = new QUEST_CONDITION[3];

	public long i64Unique;

	public bool bCheck;

	public NrQuestParamNfyTemp()
	{
		this.stCondition[0] = new QUEST_CONDITION();
		this.stCondition[1] = new QUEST_CONDITION();
		this.stCondition[2] = new QUEST_CONDITION();
	}
}
