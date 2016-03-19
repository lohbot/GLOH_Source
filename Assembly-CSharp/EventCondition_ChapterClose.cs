using System;

public class EventCondition_ChapterClose : EventTriggerItem_EventCondition
{
	public string m_QuestUnique = string.Empty;

	public override void RegisterEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.ChapterClose.ChapterClose += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.ChapterClose.ChapterClose -= new EventHandler(this.IsVerify);
	}

	public override bool IsVaildValue()
	{
		return !string.IsNullOrEmpty(this.m_QuestUnique);
	}

	public override void IsVerify(object sender, EventArgs e)
	{
		EventArgs_ChapterClose eventArgs_ChapterClose = e as EventArgs_ChapterClose;
		if (eventArgs_ChapterClose == null)
		{
			return;
		}
		if (this.m_QuestUnique == eventArgs_ChapterClose.m_QuestUnique)
		{
			base.Verify = true;
		}
	}

	public override string GetComment()
	{
		return this.m_QuestUnique + " 챕터 시작 알림이 닫히고 난 후에";
	}
}
