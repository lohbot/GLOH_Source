using System;
using UnityEngine;

public class Behavior : MonoBehaviour
{
	public enum _BEHAVIORTYPE
	{
		CAMERA,
		DRAMA,
		ETC,
		OBJECT,
		SOUND,
		MAX_TYPE
	}

	[HideInInspector]
	public EventTrigger ParentTrigger;

	public bool bPopNext
	{
		get;
		set;
	}

	public Behavior()
	{
		this.bPopNext = false;
	}

	public void InitExcute()
	{
		EventTriggerItem_Behavior eventTriggerItem_Behavior = this._GetItem();
		if (eventTriggerItem_Behavior == null)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log("Null Behavior:" + base.gameObject.name, new object[0]);
			}
			return;
		}
		this.bPopNext = false;
		eventTriggerItem_Behavior.InitExcute();
	}

	public void Init(EventTrigger parent)
	{
		EventTriggerItem_Behavior eventTriggerItem_Behavior = this._GetItem();
		if (eventTriggerItem_Behavior == null)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log("Null Behavior:" + base.gameObject.name, new object[0]);
			}
			return;
		}
		this.ParentTrigger = parent;
		eventTriggerItem_Behavior.Init();
	}

	public bool IsNextPop()
	{
		EventTriggerItem_Behavior eventTriggerItem_Behavior = this._GetItem();
		if (eventTriggerItem_Behavior == null)
		{
			return false;
		}
		this.bPopNext = eventTriggerItem_Behavior.IsPopNext();
		return this.bPopNext;
	}

	public bool Excute()
	{
		EventTriggerItem_Behavior eventTriggerItem_Behavior = this._GetItem();
		return !(eventTriggerItem_Behavior == null) && eventTriggerItem_Behavior.Excute();
	}

	private EventTriggerItem_Behavior _GetItem()
	{
		return base.gameObject.GetComponent<EventTriggerItem_Behavior>();
	}
}
