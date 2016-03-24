using System;
using System.Linq;
using UnityEngine;

namespace WellFired
{
	public class USTimelineEvent : USTimelineBase
	{
		private float elapsedTime;

		private USEventBase[] cachedEvents;

		public int EventCount
		{
			get
			{
				return base.transform.childCount;
			}
		}

		public USEventBase[] Events
		{
			get
			{
				if (this.cachedEvents != null)
				{
					return this.cachedEvents;
				}
				this.cachedEvents = base.GetComponentsInChildren<USEventBase>();
				return this.cachedEvents;
			}
		}

		public USEventBase Event(int index)
		{
			if (index >= this.EventCount)
			{
				Debug.LogError("Trying to access an element in mEventList that does not exist from USTimelineEvent::Event");
				return null;
			}
			USEventBase component = base.transform.GetChild(index).GetComponent<USEventBase>();
			if (!component)
			{
				Debug.LogWarning("We've requested an event that doesn't have a USEventBase component : " + component.name + " from timeline : " + base.name);
			}
			return component;
		}

		public override void StopTimeline()
		{
			float prevElapsedTime = this.elapsedTime;
			this.elapsedTime = 0f;
			USEventBase[] source = (from e in this.Events
			where e.FireTime <= prevElapsedTime
			select e).ToArray<USEventBase>();
			foreach (USEventBase current in source.Reverse<USEventBase>())
			{
				current.StopEvent();
			}
		}

		public override void PauseTimeline()
		{
			USEventBase[] events = this.Events;
			for (int i = 0; i < events.Length; i++)
			{
				USEventBase uSEventBase = events[i];
				uSEventBase.PauseEvent();
			}
		}

		public override void ResumeTimeline()
		{
			USEventBase[] events = this.Events;
			for (int i = 0; i < events.Length; i++)
			{
				USEventBase uSEventBase = events[i];
				if (!uSEventBase.IsFireAndForgetEvent && base.Sequence.RunningTime > uSEventBase.FireTime && base.Sequence.RunningTime < uSEventBase.FireTime + uSEventBase.Duration)
				{
					uSEventBase.ResumeEvent();
				}
			}
		}

		public override void SkipTimelineTo(float time)
		{
			float num = this.elapsedTime;
			this.elapsedTime = time;
			foreach (Transform transform in base.transform)
			{
				USEventBase component = transform.GetComponent<USEventBase>();
				if (component)
				{
					bool flag = !component.IsFireAndForgetEvent || !component.FireOnSkip;
					if (!flag)
					{
						if ((num < component.FireTime || num <= 0f) && time > component.FireTime && base.Sequence.IsPlaying && component.AffectedObject)
						{
							component.FireEvent();
						}
					}
				}
			}
		}

		public override void Process(float sequencerTime, float playbackRate)
		{
			float num = this.elapsedTime;
			this.elapsedTime = sequencerTime;
			USEventBase[] events = this.Events;
			if (num < this.elapsedTime)
			{
				Array.Sort<USEventBase>(events, (USEventBase a, USEventBase b) => a.FireTime.CompareTo(b.FireTime));
			}
			else
			{
				Array.Sort<USEventBase>(events, (USEventBase a, USEventBase b) => b.FireTime.CompareTo(a.FireTime));
			}
			USEventBase[] array = events;
			for (int i = 0; i < array.Length; i++)
			{
				USEventBase baseEvent = array[i];
				if (playbackRate >= 0f)
				{
					this.FireEvent(baseEvent, num, this.elapsedTime);
				}
				else
				{
					this.FireEventReverse(baseEvent, num, this.elapsedTime);
				}
				this.FireEventCommon(baseEvent, sequencerTime, num, this.elapsedTime);
			}
		}

		private void FireEvent(USEventBase baseEvent, float prevElapsedTime, float elapsedTime)
		{
			if ((prevElapsedTime < baseEvent.FireTime || prevElapsedTime <= 0f) && elapsedTime >= baseEvent.FireTime && baseEvent.AffectedObject)
			{
				baseEvent.FireEvent();
			}
		}

		private void FireEventReverse(USEventBase baseEvent, float prevElapsedTime, float elapsedTime)
		{
		}

		private void FireEventCommon(USEventBase baseEvent, float sequencerTime, float prevElapsedTime, float elapsedTime)
		{
			if (elapsedTime > baseEvent.FireTime && elapsedTime <= baseEvent.FireTime + baseEvent.Duration)
			{
				float runningTime = sequencerTime - baseEvent.FireTime;
				if (baseEvent.AffectedObject)
				{
					baseEvent.ProcessEvent(runningTime);
				}
			}
			if (prevElapsedTime < baseEvent.FireTime + baseEvent.Duration && elapsedTime >= baseEvent.FireTime + baseEvent.Duration && baseEvent.AffectedObject)
			{
				float runningTime2 = sequencerTime - baseEvent.FireTime;
				baseEvent.ProcessEvent(runningTime2);
				baseEvent.EndEvent();
			}
			if (prevElapsedTime >= baseEvent.FireTime && elapsedTime < baseEvent.FireTime && baseEvent.AffectedObject)
			{
				baseEvent.UndoEvent();
			}
		}

		public override void ManuallySetTime(float sequencerTime)
		{
			foreach (Transform transform in base.transform)
			{
				USEventBase component = transform.GetComponent<USEventBase>();
				if (component)
				{
					float deltaTime = sequencerTime - component.FireTime;
					if (component.AffectedObject)
					{
						component.ManuallySetTime(deltaTime);
					}
				}
			}
		}

		public void AddNewEvent(USEventBase sequencerEvent)
		{
			sequencerEvent.transform.parent = base.transform;
			this.SortEvents();
		}

		public void RemoveAndDestroyEvent(Transform sequencerEvent)
		{
			if (!sequencerEvent.IsChildOf(base.transform))
			{
				Debug.LogError("We are trying to delete an Event that doesn't belong to this Timeline, from USTimelineEvent::RemoveAndDestroyEvent");
				return;
			}
			UnityEngine.Object.DestroyImmediate(sequencerEvent.gameObject);
		}

		public void SortEvents()
		{
			Debug.LogWarning("Implement a sorting algorithm here!");
		}

		public override void ResetCachedData()
		{
			base.ResetCachedData();
			this.cachedEvents = null;
		}
	}
}
