using System;
using UnityEngine;

namespace WellFired
{
	public class USTimelineBase : MonoBehaviour
	{
		private USTimelineContainer timelineContainer;

		public Transform AffectedObject
		{
			get
			{
				return this.TimelineContainer.AffectedObject;
			}
		}

		public USTimelineContainer TimelineContainer
		{
			get
			{
				if (this.timelineContainer)
				{
					return this.timelineContainer;
				}
				this.timelineContainer = base.transform.parent.GetComponent<USTimelineContainer>();
				return this.timelineContainer;
			}
		}

		public USSequencer Sequence
		{
			get
			{
				return this.TimelineContainer.Sequence;
			}
		}

		public bool ShouldRenderGizmos
		{
			get;
			set;
		}

		public virtual void StopTimeline()
		{
		}

		public virtual void StartTimeline()
		{
		}

		public virtual void EndTimeline()
		{
		}

		public virtual void PauseTimeline()
		{
		}

		public virtual void ResumeTimeline()
		{
		}

		public virtual void SkipTimelineTo(float time)
		{
		}

		public virtual void Process(float sequencerTime, float playbackRate)
		{
		}

		public virtual void ManuallySetTime(float sequencerTime)
		{
		}

		public virtual void LateBindAffectedObjectInScene(Transform newAffectedObject)
		{
		}

		public virtual string GetJson()
		{
			throw new NotImplementedException();
		}

		public virtual void ResetCachedData()
		{
			this.timelineContainer = null;
		}
	}
}
