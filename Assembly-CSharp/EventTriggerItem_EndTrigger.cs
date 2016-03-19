using System;
using UnityEngine;

public abstract class EventTriggerItem_EndTrigger : EventTriggerItem
{
	[HideInInspector]
	protected bool bExcute;

	public abstract void Init();

	public abstract bool Excute();

	public abstract bool IsPopNext();
}
