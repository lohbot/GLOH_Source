using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WellFired
{
	[Serializable]
	public class USTimelineAnimation : USTimelineBase
	{
		private Dictionary<int, List<AnimationInfo>> initialAnimationInfo = new Dictionary<int, List<AnimationInfo>>();

		private Dictionary<int, AnimatorStateInfo> initialAnimatorStateInfo = new Dictionary<int, AnimatorStateInfo>();

		[SerializeField]
		private List<AnimationTrack> animationsTracks = new List<AnimationTrack>();

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private USTimelineAnimationEditorRunner editorRunner;

		[SerializeField]
		private USTimelineAnimationGameRunner gameRunner;

		[SerializeField]
		private AnimationTimelineController animationTimelineController;

		[SerializeField]
		private Vector3 sourcePosition;

		[SerializeField]
		private Quaternion sourceOrientation;

		[SerializeField]
		private float sourceSpeed;

		public List<AnimationTrack> AnimationTracks
		{
			get
			{
				return this.animationsTracks;
			}
			private set
			{
				this.animationsTracks = value;
			}
		}

		private Animator Animator
		{
			get
			{
				if (this.animator == null)
				{
					this.animator = base.AffectedObject.GetComponent<Animator>();
				}
				return this.animator;
			}
		}

		private USTimelineAnimationEditorRunner EditorRunner
		{
			get
			{
				if (this.editorRunner == null)
				{
					this.editorRunner = ScriptableObject.CreateInstance<USTimelineAnimationEditorRunner>();
					this.editorRunner.Animator = this.Animator;
					this.editorRunner.AnimationTimeline = this;
				}
				return this.editorRunner;
			}
		}

		private USTimelineAnimationGameRunner GameRunner
		{
			get
			{
				if (this.gameRunner == null)
				{
					this.gameRunner = ScriptableObject.CreateInstance<USTimelineAnimationGameRunner>();
					this.gameRunner.Animator = this.Animator;
					this.gameRunner.AnimationTimeline = this;
				}
				return this.gameRunner;
			}
		}

		public override void StartTimeline()
		{
			this.sourcePosition = base.AffectedObject.transform.localPosition;
			this.sourceOrientation = base.AffectedObject.transform.localRotation;
			this.sourceSpeed = this.Animator.speed;
			for (int i = 0; i < this.Animator.layerCount; i++)
			{
				this.initialAnimationInfo[i] = new List<AnimationInfo>(this.Animator.GetCurrentAnimationClipState(i).ToList<AnimationInfo>());
			}
			for (int j = 0; j < this.Animator.layerCount; j++)
			{
				this.initialAnimatorStateInfo[j] = this.Animator.GetCurrentAnimatorStateInfo(j);
			}
			if (this.Animator.applyRootMotion)
			{
				this.animationTimelineController = this.Animator.gameObject.AddComponent<AnimationTimelineController>();
				this.animationTimelineController.AnimationTimeline = this;
			}
		}

		public override void StopTimeline()
		{
			if (this.animationTimelineController)
			{
				UnityEngine.Object.DestroyImmediate(this.animationTimelineController);
			}
			this.animationTimelineController = null;
			this.Animator.Update(-base.Sequence.RunningTime);
			this.Animator.StopPlayback();
			this.ResetAnimation();
			this.initialAnimationInfo.Clear();
			this.initialAnimatorStateInfo.Clear();
			this.GameRunner.Stop();
			this.EditorRunner.Stop();
			this.Animator.speed = this.sourceSpeed;
		}

		public void ResetAnimation()
		{
			for (int i = 0; i < this.Animator.layerCount; i++)
			{
				if (this.initialAnimatorStateInfo.ContainsKey(i))
				{
					this.Animator.Play(this.initialAnimatorStateInfo[i].nameHash, i, this.initialAnimatorStateInfo[i].normalizedTime);
					this.Animator.Update(0f);
				}
			}
			if (base.Sequence.RunningTime > 0f)
			{
				base.AffectedObject.transform.localPosition = this.sourcePosition;
				base.AffectedObject.transform.localRotation = this.sourceOrientation;
			}
		}

		public override void Process(float sequenceTime, float playbackRate)
		{
			bool flag = true;
			if (flag)
			{
				this.EditorRunner.Process(sequenceTime, playbackRate);
			}
			else
			{
				this.GameRunner.Process(sequenceTime, playbackRate);
			}
		}

		public override void PauseTimeline()
		{
			bool flag = true;
			if (flag)
			{
				this.EditorRunner.PauseTimeline();
			}
			else
			{
				this.GameRunner.PauseTimeline();
			}
		}

		public void AddTrack(AnimationTrack animationTrack)
		{
			this.animationsTracks.Add(animationTrack);
		}

		public void RemoveTrack(AnimationTrack animationTrack)
		{
			this.animationsTracks.Remove(animationTrack);
		}

		public void SetTracks(List<AnimationTrack> animationTracks)
		{
			this.AnimationTracks = animationTracks;
		}
	}
}
