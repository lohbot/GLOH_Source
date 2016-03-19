using System;

namespace omniata
{
	public class EventAPINetworkResponseHandler : NetworkResponseHandler
	{
		private const string TAG = "EventAPINetworkResponseHandler";

		private Omniata Omniata
		{
			get;
			set;
		}

		private QueueElement QueueElement
		{
			get;
			set;
		}

		public EventAPINetworkResponseHandler(Omniata omniata, QueueElement queueElement)
		{
			this.Omniata = omniata;
			this.QueueElement = queueElement;
		}

		public void OnSuccess(string content, string url, int durationInMillis)
		{
			Log.Debug("EventAPINetworkResponseHandler", string.Concat(new object[]
			{
				"Success (in ",
				durationInMillis,
				"), url: ",
				url,
				", retries: ",
				this.QueueElement.Retries
			}));
			this.Omniata.EventSendSucceeded(this.QueueElement);
		}

		public void OnError(string message, string url, int durationInMillis)
		{
			Log.Error("EventAPINetworkResponseHandler", string.Concat(new object[]
			{
				"Event sending failed (in ",
				durationInMillis,
				"), message: ",
				message,
				", url: ",
				url,
				", retries: ",
				this.QueueElement.Retries
			}));
			this.Omniata.EventSendFailed(this.QueueElement);
		}
	}
}
