using System;

namespace omniata
{
	public class DefaultEventPolicy : EventPolicy
	{
		public EventAction AfterTrack(QueueElement queueElement)
		{
			return EventAction.STORE;
		}

		public EventAction AfterSendFail(QueueElement queueElement)
		{
			return EventAction.DISCARD;
		}

		public EventAction AfterLoad(QueueElement queueElement)
		{
			return EventAction.SEND;
		}
	}
}
