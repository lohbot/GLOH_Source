using GAME;
using System;

public abstract class EventTriggerItem_EventCondition : EventTriggerItem
{
	private bool _Verify;

	public bool Verify
	{
		get
		{
			return this._Verify;
		}
		set
		{
			this._Verify = value;
		}
	}

	public abstract void RegisterEvent();

	public abstract void CleanEvent();

	public abstract void IsVerify(object sender, EventArgs e);

	public virtual POS3D GetAreaEvent()
	{
		return null;
	}
}
