using System;

public class EventCondition_MYTH_ELEMENT_SLECT_DLG_IsSetComplete : EventTriggerItem_EventCondition
{
	public override void RegisterEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.MythElementSelectSet.callback += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.MythElementSelectSet.callback -= new EventHandler(this.IsVerify);
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
		return "추천도전과제 전설강림 소재 선택 DLG 가 셋팅되었는가 ? ";
	}
}
