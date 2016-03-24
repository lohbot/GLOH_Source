using System;
using System.Collections.Generic;
using UnityEngine;

namespace WellFired
{
	[ExecuteInEditMode]
	[Serializable]
	public abstract class USEventBase : MonoBehaviour
	{
		[SerializeField]
		private bool fireOnSkip;

		[SerializeField]
		private float firetime = -1f;

		[SerializeField]
		private float duration = -1f;

		[HideInInspector, SerializeField]
		private string[] serializedAdditionalObjectsPaths = new string[0];

		public float FireTime
		{
			get
			{
				return this.firetime;
			}
			set
			{
				this.firetime = value;
				if (this.firetime < 0f)
				{
					this.firetime = 0f;
				}
				if (this.firetime > this.Sequence.Duration)
				{
					this.firetime = this.Sequence.Duration;
				}
			}
		}

		public float Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = value;
			}
		}

		public USSequencer Sequence
		{
			get
			{
				return (!this.Timeline) ? null : this.Timeline.Sequence;
			}
		}

		public USTimelineBase Timeline
		{
			get
			{
				if (!base.transform.parent)
				{
					return null;
				}
				USTimelineBase component = base.transform.parent.GetComponent<USTimelineBase>();
				if (!component)
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						base.name,
						" does not have a parent with an attached timeline, this is a problem. ",
						base.name,
						"'s parent is : ",
						base.transform.parent.name
					}));
				}
				return component;
			}
		}

		public USTimelineContainer TimelineContainer
		{
			get
			{
				return (!this.Timeline) ? null : this.Timeline.TimelineContainer;
			}
		}

		public GameObject AffectedObject
		{
			get
			{
				return (!this.TimelineContainer.AffectedObject) ? null : this.TimelineContainer.AffectedObject.gameObject;
			}
		}

		public bool IsFireAndForgetEvent
		{
			get
			{
				return this.Duration < 0f;
			}
		}

		public bool FireOnSkip
		{
			get
			{
				return this.fireOnSkip;
			}
			set
			{
				this.fireOnSkip = value;
			}
		}

		public void SetSerializedAdditionalObjectsPaths(string[] paths)
		{
			this.serializedAdditionalObjectsPaths = paths;
		}

		public void FixupAdditionalObjects()
		{
			if (this.serializedAdditionalObjectsPaths == null || this.serializedAdditionalObjectsPaths.Length == 0)
			{
				return;
			}
			if (this.HasValidAdditionalObjects())
			{
				return;
			}
			List<Transform> list = new List<Transform>();
			string[] array = this.serializedAdditionalObjectsPaths;
			for (int i = 0; i < array.Length; i++)
			{
				string name = array[i];
				GameObject gameObject = GameObject.Find(name);
				if (gameObject)
				{
					list.Add(gameObject.transform);
				}
			}
			this.SetAdditionalObjects(list.ToArray());
		}

		public abstract void FireEvent();

		public abstract void ProcessEvent(float runningTime);

		public virtual void PauseEvent()
		{
		}

		public virtual void ResumeEvent()
		{
		}

		public virtual void StopEvent()
		{
		}

		public virtual void EndEvent()
		{
		}

		public virtual void UndoEvent()
		{
		}

		public virtual void ManuallySetTime(float deltaTime)
		{
		}

		public virtual Transform[] GetAdditionalObjects()
		{
			return new Transform[0];
		}

		public virtual void SetAdditionalObjects(Transform[] additionalObjects)
		{
		}

		public virtual bool HasValidAdditionalObjects()
		{
			return false;
		}

		public virtual void MakeUnique()
		{
		}
	}
}
