using System;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
	[HideInInspector]
	public EventTrigger ParentTrigger;

	public bool bPopNext
	{
		get;
		set;
	}

	public EndTrigger()
	{
		this.bPopNext = false;
	}

	public void Init(EventTrigger parent)
	{
		EventTriggerItem_EndTrigger eventTriggerItem_EndTrigger = this._GetItem();
		if (eventTriggerItem_EndTrigger == null)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log("Null EndTrigger:" + base.gameObject.name, new object[0]);
			}
			return;
		}
		this.ParentTrigger = parent;
		eventTriggerItem_EndTrigger.Init();
	}

	public bool IsNextPop()
	{
		EventTriggerItem_EndTrigger eventTriggerItem_EndTrigger = this._GetItem();
		if (eventTriggerItem_EndTrigger == null)
		{
			return false;
		}
		this.bPopNext = eventTriggerItem_EndTrigger.IsPopNext();
		return this.bPopNext;
	}

	public bool Excute()
	{
		EventTriggerItem_EndTrigger eventTriggerItem_EndTrigger = this._GetItem();
		return !(eventTriggerItem_EndTrigger == null) && eventTriggerItem_EndTrigger.Excute();
	}

	private EventTriggerItem_EndTrigger _GetItem()
	{
		return base.gameObject.GetComponent<EventTriggerItem_EndTrigger>();
	}
}
