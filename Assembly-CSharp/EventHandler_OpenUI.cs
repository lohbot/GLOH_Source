using System;
using System.Runtime.CompilerServices;

public class EventHandler_OpenUI
{
	public event EventHandler OpenUI
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.OpenUI = (EventHandler)Delegate.Combine(this.OpenUI, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.OpenUI = (EventHandler)Delegate.Remove(this.OpenUI, value);
		}
	}

	public void OnTrigger()
	{
		if (this.OpenUI != null)
		{
			this.OpenUI(this, null);
		}
	}
}
