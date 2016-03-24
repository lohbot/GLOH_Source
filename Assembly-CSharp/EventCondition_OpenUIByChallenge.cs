using System;

public class EventCondition_OpenUIByChallenge : EventTriggerItem_EventCondition
{
	public int _challengeUnique = -1;

	public override void RegisterEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.OpenUIByChallenge.OpenUIByChallenge += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.OpenUIByChallenge.OpenUIByChallenge -= new EventHandler(this.IsVerify);
	}

	public override bool IsVaildValue()
	{
		return this._challengeUnique >= 0;
	}

	public override void IsVerify(object unique, EventArgs e)
	{
		short num = (short)unique;
		if (this._challengeUnique != (int)num)
		{
			return;
		}
		base.Verify = true;
	}

	public override string GetComment()
	{
		return "추천 도전과제 Unique : " + this._challengeUnique + " 클릭해서 UI를 열었을 때";
	}
}
