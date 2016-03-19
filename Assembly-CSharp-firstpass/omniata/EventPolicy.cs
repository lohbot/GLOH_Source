using System;

namespace omniata
{
	public interface EventPolicy
	{
		EventAction AfterTrack(QueueElement queueElement);

		EventAction AfterSendFail(QueueElement queueElement);

		EventAction AfterLoad(QueueElement queueElement);
	}
}
