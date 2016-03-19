using System;
using UnityEngine;

public class EventTriggerSequenceLoader : iSequenceLoader
{
	private EventTrigger[] _ActiveEventTriggers;

	public void Load()
	{
	}

	public bool IsSequenceOff()
	{
		if (this._ActiveEventTriggers == null)
		{
			this._ActiveEventTriggers = (UnityEngine.Object.FindObjectsOfType(typeof(EventTrigger)) as EventTrigger[]);
		}
		if (this._ActiveEventTriggers == null)
		{
			return false;
		}
		bool flag = false;
		EventTrigger[] activeEventTriggers = this._ActiveEventTriggers;
		for (int i = 0; i < activeEventTriggers.Length; i++)
		{
			EventTrigger eventTrigger = activeEventTriggers[i];
			if (eventTrigger.m_Mode != EventTrigger._MODE.GAME)
			{
				if (eventTrigger.enabled)
				{
					flag = true;
					break;
				}
			}
		}
		return !flag;
	}
}
