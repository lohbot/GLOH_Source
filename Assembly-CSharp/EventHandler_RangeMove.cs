using System;
using System.Runtime.CompilerServices;

public class EventHandler_RangeMove
{
	public EventArgs_RangeMove Value = new EventArgs_RangeMove();

	public event EventHandler RangeMove
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.RangeMove = (EventHandler)Delegate.Combine(this.RangeMove, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.RangeMove = (EventHandler)Delegate.Remove(this.RangeMove, value);
		}
	}

	public void OnTrigger()
	{
		if (this.RangeMove != null)
		{
			this.RangeMove(this, this.Value);
		}
	}
}
