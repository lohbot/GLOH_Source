using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace WellFired
{
	[ExecuteInEditMode]
	[Serializable]
	public class USSequencer : MonoBehaviour
	{
		public delegate void PlaybackDelegate(USSequencer sequencer);

		public delegate void UpdateDelegate(USSequencer sequencer, float newRunningTime);

		[SerializeField]
		private List<Transform> observedObjects = new List<Transform>();

		[SerializeField]
		private float runningTime;

		[SerializeField]
		private float playbackRate = 1f;

		[SerializeField]
		private int version = 2;

		[SerializeField]
		private float duration = 10f;

		[SerializeField]
		private bool isLoopingSequence;

		[SerializeField]
		private bool isPingPongingSequence;

		[SerializeField]
		private bool updateOnFixedUpdate;

		private bool playing;

		private bool isFreshPlayback = true;

		private float previousTime;

		private float minPlaybackRate = -100f;

		private float maxPlaybackRate = 100f;

		private float setSkipTime = -1f;

		private USTimelineContainer[] timelineContainers;

		public USSequencer.PlaybackDelegate PlaybackStarted = delegate
		{
		};

		public USSequencer.PlaybackDelegate PlaybackStopped = delegate
		{
		};

		public USSequencer.PlaybackDelegate PlaybackPaused = delegate
		{
		};

		public USSequencer.PlaybackDelegate PlaybackFinished = delegate
		{
		};

		public USSequencer.UpdateDelegate BeforeUpdate = delegate
		{
		};

		public USSequencer.UpdateDelegate AfterUpdate = delegate
		{
		};

		public USSequencer.PlaybackDelegate OnRunningTimeSet = delegate
		{
		};

		public int Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		public List<Transform> ObservedObjects
		{
			get
			{
				return this.observedObjects;
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
				if (this.duration <= 0f)
				{
					this.duration = 0.1f;
				}
			}
		}

		public bool IsPlaying
		{
			get
			{
				return this.playing;
			}
		}

		public bool IsLopping
		{
			get
			{
				return this.isLoopingSequence;
			}
			set
			{
				this.isLoopingSequence = value;
			}
		}

		public bool IsPingPonging
		{
			get
			{
				return this.isPingPongingSequence;
			}
			set
			{
				this.isPingPongingSequence = value;
			}
		}

		public bool IsComplete
		{
			get
			{
				return !this.IsPlaying && this.RunningTime >= this.Duration;
			}
			set
			{
			}
		}

		public float RunningTime
		{
			get
			{
				return this.runningTime;
			}
			set
			{
				this.runningTime = value;
				if (this.runningTime <= 0f)
				{
					this.runningTime = 0f;
				}
				if (this.runningTime > this.duration)
				{
					this.runningTime = this.duration;
				}
				if (this.isFreshPlayback)
				{
					USTimelineContainer[] array = this.TimelineContainers;
					for (int i = 0; i < array.Length; i++)
					{
						USTimelineContainer uSTimelineContainer = array[i];
						USTimelineBase[] timelines = uSTimelineContainer.Timelines;
						for (int j = 0; j < timelines.Length; j++)
						{
							USTimelineBase uSTimelineBase = timelines[j];
							uSTimelineBase.StartTimeline();
						}
					}
					this.isFreshPlayback = false;
				}
				USTimelineContainer[] array2 = this.TimelineContainers;
				for (int k = 0; k < array2.Length; k++)
				{
					USTimelineContainer uSTimelineContainer2 = array2[k];
					uSTimelineContainer2.ManuallySetTime(this.RunningTime);
					uSTimelineContainer2.ProcessTimelines(this.RunningTime, this.PlaybackRate);
				}
				this.OnRunningTimeSet(this);
			}
		}

		public float PlaybackRate
		{
			get
			{
				return this.playbackRate;
			}
			set
			{
				this.playbackRate = Mathf.Clamp(value, this.MinPlaybackRate, this.MaxPlaybackRate);
			}
		}

		public float MinPlaybackRate
		{
			get
			{
				return this.minPlaybackRate;
			}
		}

		public float MaxPlaybackRate
		{
			get
			{
				return this.maxPlaybackRate;
			}
		}

		public bool HasSequenceBeenStarted
		{
			get
			{
				return !this.isFreshPlayback;
			}
		}

		public USTimelineContainer[] TimelineContainers
		{
			get
			{
				if (this.timelineContainers == null)
				{
					this.timelineContainers = base.GetComponentsInChildren<USTimelineContainer>();
				}
				return this.timelineContainers;
			}
		}

		public USTimelineContainer[] SortedTimelineContainers
		{
			get
			{
				USTimelineContainer[] array = this.TimelineContainers;
				Array.Sort<USTimelineContainer>(array, new Comparison<USTimelineContainer>(USTimelineContainer.Comparer));
				return array;
			}
		}

		public int TimelineContainerCount
		{
			get
			{
				return this.TimelineContainers.Length;
			}
		}

		public int ObservedObjectCount
		{
			get
			{
				return this.ObservedObjects.Count;
			}
		}

		public bool UpdateOnFixedUpdate
		{
			get
			{
				return this.updateOnFixedUpdate;
			}
			set
			{
				this.updateOnFixedUpdate = value;
			}
		}

		public static float SequenceUpdateRate
		{
			get
			{
				return 0.01f * Time.timeScale;
			}
		}

		private void OnDestroy()
		{
			base.StopCoroutine("UpdateSequencerCoroutine");
		}

		private void Start()
		{
			USTimelineContainer[] array = this.TimelineContainers;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = array[i];
				if (uSTimelineContainer)
				{
					USTimelineBase[] timelines = uSTimelineContainer.Timelines;
					for (int j = 0; j < timelines.Length; j++)
					{
						USTimelineBase uSTimelineBase = timelines[j];
						USTimelineEvent uSTimelineEvent = uSTimelineBase as USTimelineEvent;
						if (uSTimelineEvent)
						{
							USEventBase[] events = uSTimelineEvent.Events;
							for (int k = 0; k < events.Length; k++)
							{
								USEventBase uSEventBase = events[k];
								uSEventBase.FixupAdditionalObjects();
							}
						}
						USTimelineObserver uSTimelineObserver = uSTimelineBase as USTimelineObserver;
						if (uSTimelineObserver)
						{
							foreach (USObserverKeyframe current in uSTimelineObserver.observerKeyframes)
							{
								if (!(current.cameraPath == string.Empty))
								{
									GameObject gameObject = GameObject.Find(current.cameraPath);
									if (gameObject)
									{
										Camera component = gameObject.GetComponent<Camera>();
										if (component)
										{
											current.KeyframeCamera = component;
										}
									}
								}
							}
						}
						USTimelineObjectPath uSTimelineObjectPath = uSTimelineBase as USTimelineObjectPath;
						if (uSTimelineObjectPath)
						{
							uSTimelineObjectPath.FixupAdditionalObjects();
						}
					}
				}
			}
		}

		public void TogglePlayback()
		{
			if (this.playing)
			{
				this.Pause();
			}
			else
			{
				this.Play();
			}
		}

		public void Play()
		{
			if (this.PlaybackStarted != null)
			{
				this.PlaybackStarted(this);
			}
			base.StartCoroutine("UpdateSequencerCoroutine");
			if (this.isFreshPlayback)
			{
				USTimelineContainer[] array = this.TimelineContainers;
				for (int i = 0; i < array.Length; i++)
				{
					USTimelineContainer uSTimelineContainer = array[i];
					USTimelineBase[] timelines = uSTimelineContainer.Timelines;
					for (int j = 0; j < timelines.Length; j++)
					{
						USTimelineBase uSTimelineBase = timelines[j];
						uSTimelineBase.StartTimeline();
					}
				}
				this.isFreshPlayback = false;
			}
			else
			{
				USTimelineContainer[] array2 = this.TimelineContainers;
				for (int k = 0; k < array2.Length; k++)
				{
					USTimelineContainer uSTimelineContainer2 = array2[k];
					USTimelineBase[] timelines2 = uSTimelineContainer2.Timelines;
					for (int l = 0; l < timelines2.Length; l++)
					{
						USTimelineBase uSTimelineBase2 = timelines2[l];
						uSTimelineBase2.ResumeTimeline();
					}
				}
			}
			this.playing = true;
			this.previousTime = Time.time;
		}

		public void Pause()
		{
			if (this.PlaybackPaused != null)
			{
				this.PlaybackPaused(this);
			}
			this.playing = false;
			USTimelineContainer[] array = this.TimelineContainers;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = array[i];
				USTimelineBase[] timelines = uSTimelineContainer.Timelines;
				for (int j = 0; j < timelines.Length; j++)
				{
					USTimelineBase uSTimelineBase = timelines[j];
					uSTimelineBase.PauseTimeline();
				}
			}
		}

		public void Stop()
		{
			if (this.PlaybackStopped != null)
			{
				this.PlaybackStopped(this);
			}
			base.StopCoroutine("UpdateSequencerCoroutine");
			USTimelineContainer[] array = this.TimelineContainers;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = array[i];
				USTimelineBase[] timelines = uSTimelineContainer.Timelines;
				for (int j = 0; j < timelines.Length; j++)
				{
					USTimelineBase uSTimelineBase = timelines[j];
					if (uSTimelineBase.GetType() == typeof(USTimelineObserver) || uSTimelineBase.AffectedObject != null)
					{
						uSTimelineBase.StopTimeline();
					}
				}
			}
			this.isFreshPlayback = true;
			this.playing = false;
			this.runningTime = 0f;
		}

		private void End()
		{
			if (this.PlaybackFinished != null)
			{
				this.PlaybackFinished(this);
			}
			if (this.isLoopingSequence || this.isPingPongingSequence)
			{
				return;
			}
			USTimelineContainer[] array = this.TimelineContainers;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = array[i];
				USTimelineBase[] timelines = uSTimelineContainer.Timelines;
				for (int j = 0; j < timelines.Length; j++)
				{
					USTimelineBase uSTimelineBase = timelines[j];
					if (uSTimelineBase.AffectedObject != null)
					{
						uSTimelineBase.EndTimeline();
					}
				}
			}
		}

		public USTimelineContainer CreateNewTimelineContainer(Transform affectedObject)
		{
			USTimelineContainer uSTimelineContainer = new GameObject("Timelines for " + affectedObject.name)
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<USTimelineContainer>();
			uSTimelineContainer.AffectedObject = affectedObject;
			int num = 0;
			USTimelineContainer[] array = this.TimelineContainers;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineContainer uSTimelineContainer2 = array[i];
				if (uSTimelineContainer2.Index > num)
				{
					num = uSTimelineContainer2.Index;
				}
			}
			uSTimelineContainer.Index = num + 1;
			return uSTimelineContainer;
		}

		public bool HasTimelineContainerFor(Transform affectedObject)
		{
			USTimelineContainer[] array = this.TimelineContainers;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = array[i];
				if (uSTimelineContainer.AffectedObject == affectedObject)
				{
					return true;
				}
			}
			return false;
		}

		public USTimelineContainer GetTimelineContainerFor(Transform affectedObject)
		{
			USTimelineContainer[] array = this.TimelineContainers;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = array[i];
				if (uSTimelineContainer.AffectedObject == affectedObject)
				{
					return uSTimelineContainer;
				}
			}
			return null;
		}

		public void DeleteTimelineContainer(USTimelineContainer timelineContainer)
		{
			UnityEngine.Object.DestroyImmediate(timelineContainer.gameObject);
		}

		public void RemoveObservedObject(Transform observedObject)
		{
			if (!this.observedObjects.Contains(observedObject))
			{
				return;
			}
			this.observedObjects.Remove(observedObject);
		}

		public void SkipTimelineTo(float time)
		{
			if (this.RunningTime <= 0f && !this.IsPlaying)
			{
				this.Play();
			}
			this.setSkipTime = time;
		}

		public void SetPlaybackRate(float rate)
		{
			this.PlaybackRate = rate;
		}

		public void SetPlaybackTime(float time)
		{
			this.RunningTime = time;
		}

		public void UpdateSequencer(float deltaTime)
		{
			deltaTime *= this.playbackRate;
			if (this.playing)
			{
				this.runningTime += deltaTime;
				float num = this.runningTime;
				if (num <= 0f)
				{
					num = 0f;
				}
				if (num > this.Duration)
				{
					num = this.Duration;
				}
				this.BeforeUpdate(this, this.runningTime);
				USTimelineContainer[] array = this.TimelineContainers;
				for (int i = 0; i < array.Length; i++)
				{
					USTimelineContainer uSTimelineContainer = array[i];
					uSTimelineContainer.ProcessTimelines(num, this.PlaybackRate);
				}
				this.AfterUpdate(this, this.runningTime);
				bool flag = false;
				if (this.playbackRate > 0f && this.RunningTime >= this.duration)
				{
					flag = true;
				}
				if (this.playbackRate < 0f && this.RunningTime <= 0f)
				{
					flag = true;
				}
				if (flag)
				{
					if (this.isLoopingSequence)
					{
						float num2 = 0f;
						if (this.playbackRate > 0f && this.RunningTime >= this.Duration)
						{
							num2 = this.RunningTime - this.Duration;
						}
						if (this.playbackRate < 0f && this.RunningTime <= 0f)
						{
							num2 = this.Duration + this.RunningTime;
						}
						this.Stop();
						this.runningTime = num2;
						this.previousTime = -1f;
						this.Play();
						this.UpdateSequencer(0f);
						return;
					}
					if (this.isPingPongingSequence)
					{
						if (this.playbackRate > 0f && this.RunningTime >= this.Duration)
						{
							this.runningTime = this.Duration + (this.Duration - this.RunningTime);
						}
						if (this.playbackRate < 0f && this.RunningTime <= 0f)
						{
							this.runningTime = -1f * this.RunningTime;
						}
						this.playbackRate *= -1f;
						return;
					}
					this.playing = false;
					base.StopCoroutine("UpdateSequencerCoroutine");
					this.End();
				}
			}
			if (this.setSkipTime > 0f)
			{
				USTimelineContainer[] array2 = this.TimelineContainers;
				for (int j = 0; j < array2.Length; j++)
				{
					USTimelineContainer uSTimelineContainer2 = array2[j];
					uSTimelineContainer2.SkipTimelineTo(this.setSkipTime);
				}
				this.runningTime = this.setSkipTime;
				this.previousTime = Time.time;
				this.setSkipTime = -1f;
			}
		}

		[DebuggerHidden]
		private IEnumerator UpdateSequencerCoroutine()
		{
			USSequencer.<UpdateSequencerCoroutine>c__Iterator73 <UpdateSequencerCoroutine>c__Iterator = new USSequencer.<UpdateSequencerCoroutine>c__Iterator73();
			<UpdateSequencerCoroutine>c__Iterator.<>f__this = this;
			return <UpdateSequencerCoroutine>c__Iterator;
		}

		private void FixedUpdate()
		{
			if (!this.UpdateOnFixedUpdate)
			{
				return;
			}
			float time = Time.time;
			this.UpdateSequencer(time - this.previousTime);
			this.previousTime = time;
		}

		public void ResetCachedData()
		{
			this.timelineContainers = null;
			USTimelineContainer[] array = this.TimelineContainers;
			for (int i = 0; i < array.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = array[i];
				uSTimelineContainer.ResetCachedData();
			}
		}
	}
}
