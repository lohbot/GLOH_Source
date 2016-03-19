using System;
using System.Runtime.CompilerServices;

public class EventHandler_ItemEquip
{
	public EventArgs_ItemEquip Value = new EventArgs_ItemEquip();

	public event EventHandler ItemEquip
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.ItemEquip = (EventHandler)Delegate.Combine(this.ItemEquip, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.ItemEquip = (EventHandler)Delegate.Remove(this.ItemEquip, value);
		}
	}

	public void OnTrigger()
	{
		if (this.ItemEquip != null)
		{
			this.ItemEquip(this, this.Value);
		}
	}
}
