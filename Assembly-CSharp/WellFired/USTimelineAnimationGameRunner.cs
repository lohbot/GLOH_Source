using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WellFired
{
	[Serializable]
	public class USTimelineAnimationGameRunner : ScriptableObject
	{
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private USTimelineAnimation animationTimeline;

		[SerializeField]
		private List<AnimationClipData> allClips = new List<AnimationClipData>();

		[SerializeField]
		private List<AnimationClipData> notRunningClips = new List<AnimationClipData>();

		[SerializeField]
		private List<AnimationClipData> runningClips = new List<AnimationClipData>();

		[SerializeField]
		private List<AnimationClipData> finishedClips = new List<AnimationClipData>();

		[SerializeField]
		private List<AnimationClipData> newProcessingClips = new List<AnimationClipData>();

		public Animator Animator
		{
			private get
			{
				return this.animator;
			}
			set
			{
				this.animator = value;
			}
		}

		public USTimelineAnimation AnimationTimeline
		{
			private get
			{
				return this.animationTimeline;
			}
			set
			{
				this.animationTimeline = value;
			}
		}

		public void Stop()
		{
			this.allClips.Clear();
			this.notRunningClips.Clear();
			this.runningClips.Clear();
			this.finishedClips.Clear();
			this.newProcessingClips.Clear();
		}

		public void Process(float sequenceTime, float playbackRate)
		{
			this.allClips.Clear();
			foreach (AnimationTrack current in this.AnimationTimeline.AnimationTracks)
			{
				this.allClips.AddRange(current.TrackClips);
				foreach (AnimationClipData current2 in current.TrackClips)
				{
					current2.RunningLayer = current.Layer;
				}
			}
			this.SortClipsAtTime(sequenceTime, this.notRunningClips, new AnimationClipData.StateCheck(AnimationClipData.IsClipNotRunning));
			this.SortClipsAtTime(sequenceTime, this.runningClips, new AnimationClipData.StateCheck(AnimationClipData.IsClipRunning));
			this.SortClipsAtTime(sequenceTime, this.finishedClips, new AnimationClipData.StateCheck(AnimationClipData.IsClipFinished));
			this.SortNewProcessingClips(sequenceTime);
			this.Animator.speed = playbackRate;
			if (Application.isEditor)
			{
				this.SanityCheckClipData();
			}
		}

		public void PauseTimeline()
		{
		}

		private void SortNewProcessingClips(float sequenceTime)
		{
			IOrderedEnumerable<AnimationClipData> orderedEnumerable = from processingClip in this.newProcessingClips
			orderby processingClip.StartTime
			select processingClip;
			foreach (AnimationClipData current in orderedEnumerable)
			{
				if (AnimationClipData.IsClipNotRunning(sequenceTime, current))
				{
					this.notRunningClips.Add(current);
				}
				else if (!AnimationClipData.IsClipNotRunning(sequenceTime, current) && this.notRunningClips.Contains(current))
				{
					this.notRunningClips.Remove(current);
				}
				if (AnimationClipData.IsClipRunning(sequenceTime, current))
				{
					this.runningClips.Add(current);
					this.PlayClip(current, current.RunningLayer, sequenceTime);
				}
				else if (!AnimationClipData.IsClipRunning(sequenceTime, current) && this.runningClips.Contains(current))
				{
					this.runningClips.Remove(current);
				}
				if (AnimationClipData.IsClipFinished(sequenceTime, current))
				{
					this.finishedClips.Add(current);
				}
				else if (!AnimationClipData.IsClipFinished(sequenceTime, current) && this.finishedClips.Contains(current))
				{
					this.finishedClips.Remove(current);
				}
			}
			this.newProcessingClips.Clear();
		}

		private void SortClipsAtTime(float sequenceTime, IEnumerable<AnimationClipData> sortInto, AnimationClipData.StateCheck stateCheck)
		{
			for (int i = 0; i < this.allClips.Count; i++)
			{
				AnimationClipData animationClipData = this.allClips[i];
				bool flag = sortInto.Contains(animationClipData);
				if (stateCheck(sequenceTime, animationClipData) && !flag)
				{
					if (!this.newProcessingClips.Contains(animationClipData))
					{
						this.newProcessingClips.Add(animationClipData);
					}
				}
				else if (!stateCheck(sequenceTime, animationClipData) && flag && !this.newProcessingClips.Contains(animationClipData))
				{
					this.newProcessingClips.Add(animationClipData);
				}
			}
		}

		private void PlayClip(AnimationClipData clipToPlay, int layer, float sequenceTime)
		{
			float normalizedTime = (sequenceTime - clipToPlay.StartTime) / clipToPlay.StateDuration;
			if (clipToPlay.CrossFade)
			{
				float num = clipToPlay.TransitionDuration - (sequenceTime - clipToPlay.StartTime);
				num = Mathf.Clamp(num, 0f, float.PositiveInfinity);
				this.Animator.CrossFade(clipToPlay.StateName, num, layer, normalizedTime);
			}
			else
			{
				this.Animator.Play(clipToPlay.StateName, layer, normalizedTime);
			}
		}

		private void SanityCheckClipData()
		{
			IEnumerable<AnimationClipData> enumerable = this.AnimationTimeline.AnimationTracks.SelectMany((AnimationTrack animationTrack) => animationTrack.TrackClips);
			foreach (AnimationClipData clip in enumerable)
			{
				bool flag = this.notRunningClips.Contains(clip);
				bool flag2 = this.runningClips.Contains(clip);
				bool flag3 = this.finishedClips.Contains(clip);
				if ((from element in this.notRunningClips
				where element == clip
				select element).Count<AnimationClipData>() > 1)
				{
					throw new Exception("Clip is in the same list multiple times, this is an error.");
				}
				if ((from element in this.runningClips
				where element == clip
				select element).Count<AnimationClipData>() > 1)
				{
					throw new Exception("Clip is in the same list multiple times, this is an error.");
				}
				if ((from element in this.finishedClips
				where element == clip
				select element).Count<AnimationClipData>() > 1)
				{
					throw new Exception("Clip is in the same list multiple times, this is an error.");
				}
				if (!flag || flag2 || flag3)
				{
					if (flag || !flag2 || flag3)
					{
						if (flag || flag2 || !flag3)
						{
							if (flag || flag2 || flag3)
							{
								throw new Exception("Clip is in multiple lists, this is an error.");
							}
						}
					}
				}
			}
		}
	}
}
