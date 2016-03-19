using System;
using System.Runtime.CompilerServices;

public class EventHandler_CloseUI
{
	public event EventHandler CloseUI
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.CloseUI = (EventHandler)Delegate.Combine(this.CloseUI, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.CloseUI = (EventHandler)Delegate.Remove(this.CloseUI, value);
		}
	}

	public void OnTrigger()
	{
		if (this.CloseUI != null)
		{
			this.CloseUI(this, null);
		}
	}
}
