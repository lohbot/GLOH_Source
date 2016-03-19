using System;
using System.Collections.Generic;
using UnityEngine;

namespace omniata
{
	public class Omniata
	{
		private const string TAG = "Omniata";

		private const string SDK_VERSION = "unity-1.0";

		private const int MAX_WAIT_TIME = 64;

		private const int MIN_WAIT_TIME = 1;

		public const string EVENT_PARAM_API_KEY = "api_key";

		public const string EVENT_PARAM_CURRENCY_CODE = "currency_code";

		public const string EVENT_PARAM_EVENT_TYPE = "om_event_type";

		public const string EVENT_PARAM_TOTAL = "total";

		public const string EVENT_PARAM_UID = "uid";

		public const string EVENT_PARAM_OM_DELTA = "om_delta";

		public const string EVENT_PARAM_OM_DEVICE = "om_device";

		public const string EVENT_PARAM_OM_PLATFORM = "om_platform";

		public const string EVENT_PARAM_OM_OS_VERSION = "om_os_version";

		public const string EVENT_PARAM_OM_SDK_VERSION = "om_sdk_version";

		public const string EVENT_TYPE_OM_LOAD = "om_load";

		public const string EVENT_TYPE_OM_REVENUE = "om_revenue";

		public const string PLATFORM_ANDROID = "android";

		public const string PLATFORM_IOS = "ios";

		public const string PLATFORM_WP8 = "wp8";

		internal string ApiKey
		{
			get;
			set;
		}

		internal string UserID
		{
			get;
			set;
		}

		internal bool Debug
		{
			get;
			set;
		}

		internal bool AutomaticParametersEnabled
		{
			get;
			set;
		}

		internal Reachability Reachability
		{
			get;
			set;
		}

		internal EventPolicy EventPolicy
		{
			get;
			set;
		}

		internal bool Initialized
		{
			get;
			set;
		}

		internal bool UseSSL
		{
			get;
			set;
		}

		private Queue EventQueue
		{
			get;
			set;
		}

		private Network Network
		{
			get;
			set;
		}

		private OmniataComponent OmniataComponent
		{
			get;
			set;
		}

		public Omniata(OmniataComponent omniataComponent, bool useSSL)
		{
			this.UseSSL = useSSL;
			this.OmniataComponent = omniataComponent;
			this.EventQueue = new Queue(omniataComponent.PersistentDataPath());
			this.Network = new Network(omniataComponent);
		}

		public void ProcessEvents()
		{
			if (this.EventQueue.Count() == 0)
			{
				return;
			}
			if (!this.Reachability.Reachable())
			{
				return;
			}
			QueueElement queueElement = this.EventQueue.Peek();
			EventAction eventAction = this.EventPolicy.AfterLoad(queueElement);
			if (eventAction == EventAction.SEND)
			{
				this.EventQueue.Take();
				this.SendToEventAPI(queueElement, new EventAPINetworkResponseHandler(this, queueElement));
			}
			else if (eventAction == EventAction.DISCARD)
			{
				this.EventQueue.Take();
			}
			else if (eventAction != EventAction.STORE)
			{
				Log.Error("Omniata", "Unkown eventAction: " + eventAction);
			}
		}

		public void Track(string eventType, Dictionary<string, string> parameters)
		{
			if (!this.Initialized)
			{
				throw new InvalidOperationException("Uninitialized");
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>(parameters);
			dictionary.Add("om_event_type", eventType);
			dictionary.Add("api_key", this.ApiKey);
			dictionary.Add("uid", this.UserID);
			QueueElement queueElement = new QueueElement(eventType, Utils.SecondsSinceEpoch(), 0, dictionary);
			EventAction eventAction = this.EventPolicy.AfterTrack(queueElement);
			if (eventAction == EventAction.SEND)
			{
				this.SendToEventAPI(queueElement, new EventAPINetworkResponseHandler(this, queueElement));
			}
			else if (eventAction != EventAction.DISCARD)
			{
				if (eventAction == EventAction.STORE)
				{
					this.EventQueue.Put(queueElement);
				}
				else
				{
					Log.Error("Omniata", "Unkown eventAction: " + eventAction);
				}
			}
		}

		public void Channel(int channelId, NetworkResponseHandler networkResponseHandler)
		{
			if (!this.Initialized)
			{
				throw new InvalidOperationException("Uninitialized");
			}
			this.SendToChannelAPI(channelId, networkResponseHandler);
		}

		public void AddAutomaticParameters(Dictionary<string, string> parameters)
		{
			if (!this.AutomaticParametersEnabled)
			{
				return;
			}
			RuntimePlatform platform = Application.platform;
			string value = string.Empty;
			if (platform == RuntimePlatform.Android)
			{
				value = "android";
			}
			else if (platform == RuntimePlatform.IPhonePlayer)
			{
				value = "ios";
			}
			else if (platform == RuntimePlatform.WP8Player)
			{
				value = "wp8";
			}
			else
			{
				value = "unknown";
			}
			parameters.Add("om_platform", value);
			parameters.Add("om_device", this.OmniataComponent.GetDevice());
			parameters.Add("om_os_version", this.OmniataComponent.GetOsVersion());
			parameters.Add("om_sdk_version", "unity-1.0");
		}

		private bool RunningInEditor()
		{
			return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor;
		}

		private void SendToEventAPI(QueueElement element, NetworkResponseHandler networkResponseHandler)
		{
			string url = Utils.GetEventAPI(this.UseSSL, this.Debug) + "?" + element.WireFormat();
			this.Network.Send(url, networkResponseHandler);
		}

		private void SendToChannelAPI(int channelId, NetworkResponseHandler networkResponseHandler)
		{
			string url = string.Concat(new object[]
			{
				Utils.GetChannelAPI(this.UseSSL, this.Debug),
				"?api_key=",
				this.ApiKey,
				"&uid=",
				WWW.EscapeURL(this.UserID),
				"&channel_id=",
				channelId
			});
			this.Network.Send(url, networkResponseHandler);
		}

		internal void EventSendSucceeded(QueueElement element)
		{
			this.OmniataComponent.WaitTime = 1;
		}

		internal void EventSendFailed(QueueElement element)
		{
			element.Retries++;
			int waitTime = Math.Min(64, this.OmniataComponent.WaitTime * 2);
			this.OmniataComponent.WaitTime = waitTime;
			EventAction eventAction = this.EventPolicy.AfterSendFail(element);
			if (eventAction == EventAction.SEND)
			{
				this.SendToEventAPI(element, new EventAPINetworkResponseHandler(this, element));
			}
			else if (eventAction != EventAction.DISCARD)
			{
				if (eventAction == EventAction.STORE)
				{
					this.EventQueue.Prepend(element);
				}
				else
				{
					Log.Error("Omniata", "Unkown eventAction: " + eventAction);
				}
			}
		}
	}
}
