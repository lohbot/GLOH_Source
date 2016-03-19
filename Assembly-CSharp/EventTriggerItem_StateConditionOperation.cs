using System;

public abstract class EventTriggerItem_StateConditionOperation : EventTriggerItem_StateCondition
{
	private EventTriggerItem_StateCondition _lItem;

	private EventTriggerItem_StateCondition _rItem;

	public EventTriggerItem_StateCondition lItem
	{
		get
		{
			return this._lItem;
		}
		set
		{
			this._lItem = value;
		}
	}

	public EventTriggerItem_StateCondition rItem
	{
		get
		{
			return this._rItem;
		}
		set
		{
			this._rItem = value;
		}
	}

	public void Clear()
	{
		this._lItem = null;
		this._rItem = null;
	}
}
