using System;
using UnityEngine;

namespace WellFired
{
	[Serializable]
	public class AnimationClipData : ScriptableObject
	{
		public delegate bool StateCheck(float sequencerTime, AnimationClipData clipData);

		[SerializeField]
		private bool crossFade;

		[SerializeField]
		private float transitionDuration;

		[SerializeField]
		private float startTime;

		[SerializeField]
		private float playbackDuration;

		[SerializeField]
		private float stateDuration;

		[HideInInspector, SerializeField]
		private string stateName = "NotSet";

		[SerializeField]
		private AnimationTrack track;

		[HideInInspector, SerializeField]
		private GameObject targetObject;

		[HideInInspector]
		private bool dirty;

		public bool CrossFade
		{
			get
			{
				return this.crossFade;
			}
			set
			{
				this.crossFade = value;
			}
		}

		public float TransitionDuration
		{
			get
			{
				return this.transitionDuration;
			}
			set
			{
				this.transitionDuration = value;
			}
		}

		public float StartTime
		{
			get
			{
				return this.startTime;
			}
			set
			{
				this.startTime = value;
				this.dirty = true;
			}
		}

		public float PlaybackDuration
		{
			get
			{
				return this.playbackDuration;
			}
			set
			{
				this.playbackDuration = value;
				this.dirty = true;
			}
		}

		public float StateDuration
		{
			get
			{
				return this.stateDuration;
			}
			set
			{
				this.stateDuration = value;
				this.dirty = true;
			}
		}

		public string StateName
		{
			get
			{
				return this.stateName;
			}
			set
			{
				this.stateName = value;
				this.FriendlyName = AnimationClipData.MakeFriendlyStateName(this.StateName);
				this.dirty = true;
			}
		}

		public AnimationTrack Track
		{
			get
			{
				return this.track;
			}
			set
			{
				this.track = value;
				this.dirty = true;
			}
		}

		public string FriendlyName
		{
			get
			{
				return AnimationClipData.MakeFriendlyStateName(this.StateName);
			}
			private set
			{
			}
		}

		public GameObject TargetObject
		{
			get
			{
				return this.targetObject;
			}
			set
			{
				this.targetObject = value;
				this.dirty = true;
			}
		}

		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
			set
			{
				this.dirty = value;
			}
		}

		public float EndTime
		{
			get
			{
				return this.startTime + this.playbackDuration;
			}
			private set
			{
			}
		}

		public int RunningLayer
		{
			get;
			set;
		}

		public static bool IsClipNotRunning(float sequencerTime, AnimationClipData clipData)
		{
			return sequencerTime < clipData.StartTime;
		}

		public static bool IsClipRunning(float sequencerTime, AnimationClipData clipData)
		{
			return sequencerTime > clipData.StartTime && sequencerTime < clipData.EndTime;
		}

		public static bool IsClipFinished(float sequencerTime, AnimationClipData clipData)
		{
			return sequencerTime >= clipData.EndTime;
		}

		public static string MakeFriendlyStateName(string stateName)
		{
			int num = stateName.IndexOf("Layer.");
			if (num == -1)
			{
				return stateName;
			}
			int count = num + "Layer.".Length;
			return stateName.Remove(0, count);
		}
	}
}
