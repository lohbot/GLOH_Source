using System;

public interface IEventTrigger_ActorAction
{
	void SetPosition(float x, float y, float Angle);

	void GetPosition(ref float x, ref float y, ref float Angle);

	void MovePosition(float x, float y, float Angle, float time);
}
