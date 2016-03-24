using System;
using System.Runtime.CompilerServices;

public class EventHandler_OpenUIByChallenge
{
	public event EventHandler OpenUIByChallenge
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.OpenUIByChallenge = (EventHandler)Delegate.Combine(this.OpenUIByChallenge, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.OpenUIByChallenge = (EventHandler)Delegate.Remove(this.OpenUIByChallenge, value);
		}
	}

	public void OnTrigger(object unique)
	{
		if (this.OpenUIByChallenge == null)
		{
			return;
		}
		this.OpenUIByChallenge(unique, null);
	}
}
