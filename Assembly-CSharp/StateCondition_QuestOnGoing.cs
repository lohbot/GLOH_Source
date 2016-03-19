using System;

public class StateCondition_QuestOnGoing : EventTriggerItem_StateCondition, IEventTriggerItem_StateConditionQuest
{
	public string m_QuestUnique = string.Empty;

	public override bool IsVaildValue()
	{
		return !string.IsNullOrEmpty(this.m_QuestUnique);
	}

	public override bool Verify()
	{
		return EventTriggerGameHelper.IsQuestState(this.m_QuestUnique, QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING);
	}

	public override string GetComment()
	{
		return this.m_QuestUnique + " 퀘스트를 진행 중이라면";
	}

	public bool IsConditionQuest(string QuestUnique)
	{
		return this.m_QuestUnique.Equals(QuestUnique);
	}
}
