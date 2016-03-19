using System;

public class Behavior_ActorWalk : Behavior_ActorMove
{
	public override void Init()
	{
		this.MovePosition(this.m_DestX, this.m_DestY, 0f, this.m_MoveSecond);
	}

	public override string GetComment()
	{
		return string.Format("{0} 캐릭터를 X:{1}, Y:{2}로 {3}초 동안 걷도록 시킨다.", new object[]
		{
			this.m_ActorName,
			this.m_DestX.ToString(),
			this.m_DestY.ToString(),
			this.m_MoveSecond.ToString()
		});
	}

	public override void MovePosition(float x, float y, float Angle, float time)
	{
		NrTSingleton<EventTriggerMiniDrama>.Instance.WalkActor(this.m_ActorName, x, y, time);
	}
}
