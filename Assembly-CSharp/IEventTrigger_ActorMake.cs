using System;

public interface IEventTrigger_ActorMake
{
	void SetMakeActorCode(string ActorCode);

	string[] GetMakeActorName();

	void MakeActor(string ActorName, float x, float y, float Angle, bool Hide);
}
