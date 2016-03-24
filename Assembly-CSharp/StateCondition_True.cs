using System;

public class StateCondition_True : EventTriggerItem_StateCondition, IEventTriggerItem_StateConditionQuest
{
	public string m_QuestUnique = string.Empty;

	public override bool IsVaildValue()
	{
		return true;
	}

	public override bool Verify()
	{
		return true;
	}

	public override string GetComment()
	{
		return " STATE 무조건 TRUE ";
	}

	public bool IsConditionQuest(string QuestUnique)
	{
		return true;
	}
}
