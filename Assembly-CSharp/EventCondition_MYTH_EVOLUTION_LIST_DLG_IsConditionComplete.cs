using System;

public class EventCondition_MYTH_EVOLUTION_LIST_DLG_IsConditionComplete : EventTriggerItem_EventCondition
{
	public override void RegisterEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.MythEvolutionListReady.callback += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.MythEvolutionListReady.callback -= new EventHandler(this.IsVerify);
	}

	public override bool IsVaildValue()
	{
		return true;
	}

	public override void IsVerify(object sender, EventArgs e)
	{
		base.Verify = true;
	}

	public override string GetComment()
	{
		return "추천도전과제 전설강림 리스트 소재들이 모두 셋팅되었는가?";
	}
}
