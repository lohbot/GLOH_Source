using System;
using System.Collections.Generic;

namespace Prime31
{
	public class FacebookGameRequestContent
	{
		public string title = string.Empty;

		public string message = string.Empty;

		public string data = string.Empty;

		public string objectId = string.Empty;

		public List<string> recipients = new List<string>();

		public List<string> recipientSuggestions = new List<string>();

		public bool frictionlessRequestsEnabled;
	}
}
