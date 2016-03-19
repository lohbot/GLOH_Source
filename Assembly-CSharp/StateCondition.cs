using System;
using UnityEngine;

public class StateCondition : MonoBehaviour
{
	public bool Verify()
	{
		EventTriggerItem_StateCondition eventTriggerItem_StateCondition = this._GetItem();
		return !(eventTriggerItem_StateCondition == null) && eventTriggerItem_StateCondition.Verify();
	}

	public bool IsOperation()
	{
		return base.gameObject.GetComponent<EventTriggerItem_StateConditionOperation>() != null;
	}

	private EventTriggerItem_StateCondition _GetItem()
	{
		return base.gameObject.GetComponent<EventTriggerItem_StateCondition>();
	}
}
