using System;
using System.Runtime.CompilerServices;

public class EventHandler_MapIn
{
	public event EventHandler MapIn
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.MapIn = (EventHandler)Delegate.Combine(this.MapIn, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.MapIn = (EventHandler)Delegate.Remove(this.MapIn, value);
		}
	}

	public void OnTrigger()
	{
		if (this.MapIn != null)
		{
			this.MapIn(this, null);
		}
	}
}
