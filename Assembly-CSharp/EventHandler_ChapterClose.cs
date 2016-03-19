using System;
using System.Runtime.CompilerServices;

public class EventHandler_ChapterClose
{
	public EventArgs_ChapterClose Value = new EventArgs_ChapterClose();

	public event EventHandler ChapterClose
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.ChapterClose = (EventHandler)Delegate.Combine(this.ChapterClose, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.ChapterClose = (EventHandler)Delegate.Remove(this.ChapterClose, value);
		}
	}

	public void OnTrigger()
	{
		if (this.ChapterClose != null)
		{
			this.ChapterClose(this, this.Value);
		}
	}
}
