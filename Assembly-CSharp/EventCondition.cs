using System;
using UnityEngine;

public class EventCondition : MonoBehaviour
{
	public bool Verify
	{
		get
		{
			EventTriggerItem_EventCondition eventTriggerItem_EventCondition = this._GetItem();
			return !(eventTriggerItem_EventCondition == null) && eventTriggerItem_EventCondition.Verify;
		}
		set
		{
			EventTriggerItem_EventCondition eventTriggerItem_EventCondition = this._GetItem();
			if (eventTriggerItem_EventCondition != null)
			{
				eventTriggerItem_EventCondition.Verify = value;
			}
		}
	}

	private EventTriggerItem_EventCondition _GetItem()
	{
		return base.gameObject.GetComponent<EventTriggerItem_EventCondition>();
	}

	public void RegisterEvent()
	{
		EventTriggerItem_EventCondition eventTriggerItem_EventCondition = this._GetItem();
		if (eventTriggerItem_EventCondition != null)
		{
			eventTriggerItem_EventCondition.RegisterEvent();
		}
	}

	public void CleanEvent()
	{
		EventTriggerItem_EventCondition eventTriggerItem_EventCondition = this._GetItem();
		if (eventTriggerItem_EventCondition != null)
		{
			eventTriggerItem_EventCondition.CleanEvent();
		}
	}
}
