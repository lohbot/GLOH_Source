using System;

public class EventCondition_True : EventTriggerItem_EventCondition
{
	private void Update()
	{
		base.Verify = true;
	}

	public override void RegisterEvent()
	{
		base.Verify = true;
	}

	public override void CleanEvent()
	{
	}

	public override bool IsVaildValue()
	{
		return true;
	}

	public override void IsVerify(object sender, EventArgs e)
	{
	}

	public override string GetComment()
	{
		return "무조건 True";
	}
}
