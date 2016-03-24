using System;
using UnityEngine;

namespace WellFired
{
	[ExecuteInEditMode]
	public class AnimationTimelineController : MonoBehaviour
	{
		private Animator animator;

		private USTimelineAnimation animationTimeline;

		private Animator Animator
		{
			get
			{
				if (this.animator == null)
				{
					this.animator = base.GetComponent<Animator>();
				}
				return this.animator;
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

		private void OnAnimatorMove()
		{
			base.transform.position = this.Animator.rootPosition;
			base.transform.rotation = this.Animator.rootRotation;
		}
	}
}
