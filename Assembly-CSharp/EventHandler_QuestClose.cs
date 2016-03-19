using System;
using System.Runtime.CompilerServices;

public class EventHandler_QuestClose
{
	public EventArgs_QuestClose Value = new EventArgs_QuestClose();

	public event EventHandler QuestClose
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.QuestClose = (EventHandler)Delegate.Combine(this.QuestClose, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.QuestClose = (EventHandler)Delegate.Remove(this.QuestClose, value);
		}
	}

	public void OnTrigger()
	{
		if (this.QuestClose != null)
		{
			this.QuestClose(this, this.Value);
		}
	}
}
