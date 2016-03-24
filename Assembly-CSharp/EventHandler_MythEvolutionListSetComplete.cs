using System;
using System.Runtime.CompilerServices;

public class EventHandler_MythEvolutionListSetComplete
{
	public event EventHandler callback
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.callback = (EventHandler)Delegate.Combine(this.callback, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.callback = (EventHandler)Delegate.Remove(this.callback, value);
		}
	}

	public void OnTrigger()
	{
		if (this.callback == null)
		{
			return;
		}
		this.callback(null, null);
	}
}
