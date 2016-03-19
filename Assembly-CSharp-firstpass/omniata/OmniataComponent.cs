using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace omniata
{
	public class OmniataComponent : MonoBehaviour
	{
		private const string TAG = "OmniataComponent";

		private const bool USE_SSL = false;

		internal int WaitTime
		{
			get;
			set;
		}

		private bool isRunning
		{
			get;
			set;
		}

		private Omniata OmniataInstance
		{
			get;
			set;
		}

		private void Start()
		{
			this.WaitTime = 1;
			this.OmniataInstance = new Omniata(this, false);
		}

		[DebuggerHidden]
		internal IEnumerator Run()
		{
			OmniataComponent.<Run>c__Iterator15 <Run>c__Iterator = new OmniataComponent.<Run>c__Iterator15();
			<Run>c__Iterator.<>f__this = this;
			return <Run>c__Iterator;
		}

		public Omniata GetOmniataInstance()
		{
			return this.OmniataInstance;
		}

		public void Initialize(string apiKey, string userID)
		{
			bool debug = false;
			this.Initialize(apiKey, userID, debug, true, new DefaultReachability(), new DefaultEventPolicy());
		}

		public void Initialize(string apiKey, string userID, bool debug, bool automaticParametersEnabled, Reachability reachability, EventPolicy eventPolicy)
		{
			this.OmniataInstance.ApiKey = apiKey;
			this.OmniataInstance.UserID = userID;
			this.OmniataInstance.Debug = debug;
			this.OmniataInstance.AutomaticParametersEnabled = automaticParametersEnabled;
			this.OmniataInstance.Reachability = reachability;
			this.OmniataInstance.EventPolicy = eventPolicy;
			this.OmniataInstance.Initialized = true;
			if (!this.isRunning)
			{
				base.StartCoroutine(this.Run());
				this.isRunning = true;
			}
			Log.Debug("OmniataComponent", string.Concat(new object[]
			{
				"initialized. ApiKey: ",
				this.OmniataInstance.ApiKey,
				", UserID: ",
				this.OmniataInstance.UserID,
				", Debug: ",
				this.OmniataInstance.Debug,
				", AutomaticParametersEnabled: ",
				this.OmniataInstance.AutomaticParametersEnabled
			}));
		}

		public void LogLevel(LogLevel logLevel)
		{
			Log.LogLevel = logLevel;
		}

		public void Track(string eventType)
		{
			this.Track(eventType, new Dictionary<string, string>());
		}

		public void Track(string eventType, Dictionary<string, string> parameters)
		{
			this.OmniataInstance.Track(eventType, parameters);
		}

		public void TrackLoad()
		{
			if (this.OmniataInstance.Initialized)
			{
				this.TrackLoad(new Dictionary<string, string>());
			}
		}

		public void TrackLoad(Dictionary<string, string> parameters)
		{
			Dictionary<string, string> parameters2 = new Dictionary<string, string>(parameters);
			this.OmniataInstance.AddAutomaticParameters(parameters2);
			this.Track("om_load", parameters2);
		}

		public void TrackRevenue(decimal total, string currencyCode)
		{
			this.TrackRevenue(total, currencyCode, new Dictionary<string, string>());
		}

		public void TrackRevenue(decimal total, string currencyCode, Dictionary<string, string> parameters)
		{
			this.Track("om_revenue", new Dictionary<string, string>(parameters)
			{
				{
					"total",
					Utils.DecimalToString(total)
				},
				{
					"currency_code",
					currencyCode
				}
			});
		}

		public void Channel(int channelId, NetworkResponseHandler networkResponseHandler)
		{
			this.OmniataInstance.Channel(channelId, networkResponseHandler);
		}

		internal void StartCoroutine(Coroutine coroutine)
		{
			this.StartCoroutine(coroutine);
		}

		internal string PersistentDataPath()
		{
			return Application.persistentDataPath;
		}

		internal string GetOsVersion()
		{
			return SystemInfo.operatingSystem;
		}

		internal string GetDevice()
		{
			return SystemInfo.deviceModel;
		}
	}
}
