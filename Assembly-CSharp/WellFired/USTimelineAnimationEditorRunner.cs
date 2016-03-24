using System;
using System.Collections.Generic;
using UnityEngine;

namespace WellFired
{
	[Serializable]
	public class USTimelineAnimationEditorRunner : ScriptableObject
	{
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private USTimelineAnimation animationTimeline;

		[SerializeField]
		private List<AnimationClipData> allClips = new List<AnimationClipData>();

		private List<AnimationClipData> cachedRunningClips = new List<AnimationClipData>();

		private float previousTime;

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
			this.previousTime = 0f;
		}

		public void Process(float sequenceTime, float playbackRate)
		{
			this.allClips.Clear();
			for (int i = 0; i < this.AnimationTimeline.AnimationTracks.Count; i++)
			{
				AnimationTrack animationTrack = this.AnimationTimeline.AnimationTracks[i];
				for (int j = 0; j < animationTrack.TrackClips.Count; j++)
				{
					AnimationClipData animationClipData = animationTrack.TrackClips[j];
					this.allClips.Add(animationClipData);
					animationClipData.RunningLayer = animationTrack.Layer;
				}
			}
			float num = sequenceTime - this.previousTime;
			float num2 = Mathf.Abs(num);
			bool flag = num < 0f;
			float num3 = USSequencer.SequenceUpdateRate;
			float num4 = this.previousTime + num3;
			if (flag)
			{
				this.AnimationTimeline.ResetAnimation();
				this.previousTime = 0f;
				this.AnimationTimeline.Process(sequenceTime, playbackRate);
			}
			else
			{
				while (num2 > 0f)
				{
					this.cachedRunningClips.Clear();
					for (int k = 0; k < this.allClips.Count; k++)
					{
						AnimationClipData animationClipData2 = this.allClips[k];
						if (AnimationClipData.IsClipRunning(num4, animationClipData2))
						{
							this.cachedRunningClips.Add(animationClipData2);
						}
					}
					this.cachedRunningClips.Sort((AnimationClipData x, AnimationClipData y) => x.StartTime.CompareTo(y.StartTime));
					for (int l = 0; l < this.cachedRunningClips.Count; l++)
					{
						AnimationClipData animationClipData3 = this.cachedRunningClips[l];
						this.PlayClip(animationClipData3, animationClipData3.RunningLayer, num4);
					}
					this.Animator.Update(num3);
					num2 -= USSequencer.SequenceUpdateRate;
					if (!Mathf.Approximately(num2, 1.401298E-45f) && num2 < USSequencer.SequenceUpdateRate)
					{
						num3 = num2;
					}
					num4 += num3;
				}
			}
			this.previousTime = sequenceTime;
		}

		public void PauseTimeline()
		{
			this.Animator.enabled = false;
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
	}
}
