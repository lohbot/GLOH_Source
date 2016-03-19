using GAME;
using System;

public class EventCondition_RangeMove : EventTriggerItem_EventCondition
{
	public float X;

	public float Y;

	public double Range;

	public override void RegisterEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.RangeMove.RangeMove += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.RangeMove.RangeMove -= new EventHandler(this.IsVerify);
	}

	public override bool IsVaildValue()
	{
		return this.X != 0f || this.Y != 0f || this.Range != 0.0;
	}

	public override void IsVerify(object sender, EventArgs e)
	{
		EventArgs_RangeMove eventArgs_RangeMove = e as EventArgs_RangeMove;
		if (eventArgs_RangeMove == null)
		{
			return;
		}
		double num = Math.Sqrt(Math.Pow((double)(eventArgs_RangeMove.x - this.X), 2.0) + Math.Pow((double)(eventArgs_RangeMove.y - this.Y), 2.0));
		if (num <= this.Range)
		{
			base.Verify = true;
		}
	}

	public override POS3D GetAreaEvent()
	{
		return new POS3D
		{
			x = this.X,
			z = this.Y
		};
	}

	public override string GetComment()
	{
		return string.Concat(new string[]
		{
			"X: ",
			this.X.ToString(),
			" Y: ",
			this.Y.ToString(),
			" 근처에 군주가 갔을 때"
		});
	}
}
