using System;

namespace omniata
{
	public interface NetworkResponseHandler
	{
		void OnSuccess(string content, string url, int durationInMillis);

		void OnError(string message, string url, int durationInMillis);
	}
}
