using System;
using UnityEngine;

public abstract class EventTriggerItem_EndBehavior : EventTriggerItem
{
	[HideInInspector]
	protected bool bExcute;

	public abstract void Init();

	public abstract bool Excute();

	public abstract bool IsPopNext();

	public abstract void Draw();

	public abstract Behavior._BEHAVIORTYPE GetBehaviorType();
}
