using System;

public class EventCondition_MYTH_EVOLUTION_INFO_OpenMsgBox : EventTriggerItem_EventCondition
{
	public override void RegisterEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.MythEvolutionInfoMsgBox.callback += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.MythEvolutionInfoMsgBox.callback -= new EventHandler(this.IsVerify);
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
		return "추천도전과제 전설강림 소재영웅 셋팅 dlg 에서 MsgBox 를 열었는가?";
	}
}
