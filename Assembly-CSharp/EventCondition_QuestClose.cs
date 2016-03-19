using System;

public class EventCondition_QuestClose : EventTriggerItem_EventCondition
{
	public string m_QuestUnique = string.Empty;

	public override void RegisterEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.QuestClose.QuestClose += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.QuestClose.QuestClose -= new EventHandler(this.IsVerify);
	}

	public override bool IsVaildValue()
	{
		return !string.IsNullOrEmpty(this.m_QuestUnique);
	}

	public override void IsVerify(object sender, EventArgs e)
	{
		EventArgs_QuestClose eventArgs_QuestClose = e as EventArgs_QuestClose;
		if (eventArgs_QuestClose == null)
		{
			return;
		}
		if (this.m_QuestUnique == eventArgs_QuestClose.m_QuestUnique)
		{
			base.Verify = true;
		}
	}

	public override string GetComment()
	{
		return this.m_QuestUnique + " 퀘스트 대화를 보고 창을 닫았을 때";
	}
}
