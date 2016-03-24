using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace omniata
{
	public class Network
	{
		private const string TAG = "Network";

		private OmniataComponent OmniataComponent
		{
			get;
			set;
		}

		public Network(OmniataComponent omniataComponent)
		{
			this.OmniataComponent = omniataComponent;
		}

		public void Send(string url, NetworkResponseHandler networkResponseHandler)
		{
			WWW www = new WWW(url);
			Log.Debug("Network", "sending, URL: " + url);
			this.OmniataComponent.StartCoroutine(this.Request(www, url, networkResponseHandler));
		}

		[DebuggerHidden]
		private IEnumerator Request(WWW www, string url, NetworkResponseHandler networkResponseHandler)
		{
			Network.<Request>c__Iterator15 <Request>c__Iterator = new Network.<Request>c__Iterator15();
			<Request>c__Iterator.www = www;
			<Request>c__Iterator.networkResponseHandler = networkResponseHandler;
			<Request>c__Iterator.url = url;
			<Request>c__Iterator.<$>www = www;
			<Request>c__Iterator.<$>networkResponseHandler = networkResponseHandler;
			<Request>c__Iterator.<$>url = url;
			return <Request>c__Iterator;
		}
	}
}
