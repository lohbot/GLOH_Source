using System;
using UnityEngine;

namespace WellFired
{
	[ExecuteInEditMode]
	public class USTimelineContainer : MonoBehaviour
	{
		[SerializeField]
		private Transform affectedObject;

		[SerializeField]
		private string affectedObjectPath;

		[SerializeField]
		private int index = -1;

		private USSequencer sequence;

		private USTimelineBase[] timelines;

		public Transform AffectedObject
		{
			get
			{
				if (this.affectedObject == null && this.affectedObjectPath != string.Empty)
				{
					GameObject gameObject = GameObject.Find(this.affectedObjectPath);
					if (gameObject)
					{
						this.affectedObject = gameObject.transform;
						USTimelineBase[] array = this.Timelines;
						for (int i = 0; i < array.Length; i++)
						{
							USTimelineBase uSTimelineBase = array[i];
							uSTimelineBase.LateBindAffectedObjectInScene(this.affectedObject);
						}
					}
				}
				return this.affectedObject;
			}
			set
			{
				this.affectedObject = value;
				if (this.affectedObject != null)
				{
					this.affectedObjectPath = this.affectedObject.transform.GetFullHierarchyPath();
				}
				this.RenameTimelineContainer();
			}
		}

		public USSequencer Sequence
		{
			get
			{
				if (this.sequence)
				{
					return this.sequence;
				}
				this.sequence = base.transform.parent.GetComponent<USSequencer>();
				return this.sequence;
			}
		}

		public USTimelineBase[] Timelines
		{
			get
			{
				if (this.timelines != null)
				{
					return this.timelines;
				}
				this.timelines = base.GetComponentsInChildren<USTimelineBase>();
				return this.timelines;
			}
		}

		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		public string AffectedObjectPath
		{
			get
			{
				return this.affectedObjectPath;
			}
			private set
			{
				this.affectedObjectPath = value;
			}
		}

		public static int Comparer(USTimelineContainer a, USTimelineContainer b)
		{
			return a.Index.CompareTo(b.Index);
		}

		private void Start()
		{
			if (this.affectedObjectPath == null)
			{
				this.affectedObjectPath = string.Empty;
			}
			else if (this.AffectedObject == null && this.affectedObjectPath.Length != 0)
			{
				GameObject gameObject = GameObject.Find(this.affectedObjectPath);
				this.AffectedObject = gameObject.transform;
			}
			USTimelineBase[] array = this.Timelines;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineBase uSTimelineBase = array[i];
				USTimelineProperty uSTimelineProperty = uSTimelineBase as USTimelineProperty;
				if (uSTimelineProperty)
				{
					uSTimelineProperty.TryToFixComponentReferences();
				}
			}
		}

		public void AddNewTimeline(USTimelineBase timeline)
		{
			base.transform.parent = base.transform;
		}

		public void ProcessTimelines(float sequencerTime, float playbackRate)
		{
			USTimelineBase[] array = this.Timelines;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineBase uSTimelineBase = array[i];
				uSTimelineBase.Process(sequencerTime, playbackRate);
			}
		}

		public void SkipTimelineTo(float sequencerTime)
		{
			USTimelineBase[] array = this.Timelines;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineBase uSTimelineBase = array[i];
				uSTimelineBase.SkipTimelineTo(sequencerTime);
			}
		}

		public void ManuallySetTime(float sequencerTime)
		{
			USTimelineBase[] array = this.Timelines;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineBase uSTimelineBase = array[i];
				uSTimelineBase.ManuallySetTime(sequencerTime);
			}
		}

		public void RenameTimelineContainer()
		{
			if (this.affectedObject)
			{
				base.name = "Timelines for " + this.affectedObject.name;
			}
		}

		public void ResetCachedData()
		{
			this.sequence = null;
			this.timelines = null;
			USTimelineBase[] array = this.Timelines;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineBase uSTimelineBase = array[i];
				uSTimelineBase.ResetCachedData();
			}
		}
	}
}
