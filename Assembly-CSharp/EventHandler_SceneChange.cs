using System;
using System.Runtime.CompilerServices;

public class EventHandler_SceneChange
{
	public EventArgs_SceneChange Value = new EventArgs_SceneChange();

	public event EventHandler SceneChange
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.SceneChange = (EventHandler)Delegate.Combine(this.SceneChange, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.SceneChange = (EventHandler)Delegate.Remove(this.SceneChange, value);
		}
	}

	public void OnTrigger()
	{
		if (this.SceneChange != null)
		{
			this.SceneChange(this, this.Value);
		}
	}
}
