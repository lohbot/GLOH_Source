using System;
using UnityEngine;

namespace WellFired
{
	[USequencerEvent("Signal/Send Message (Int)"), USequencerEventHideDuration, USequencerFriendlyName("Send Message (Int)")]
	public class USSendMessageIntEvent : USEventBase
	{
		public GameObject receiver;

		public string action = "OnSignal";

		[SerializeField]
		private int valueToSend;

		public override void FireEvent()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.receiver)
			{
				this.receiver.SendMessage(this.action, this.valueToSend);
			}
			else
			{
				Debug.LogWarning(string.Format("No receiver of signal \"{0}\" on object {1} ({2})", this.action, this.receiver.name, this.receiver.GetType().Name), this.receiver);
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
		}
	}
}
