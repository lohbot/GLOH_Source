using System;
using UnityEngine;

namespace WellFired
{
	[USequencerEvent("Animation (Legacy)/Play Animation"), USequencerFriendlyName("Play Animation (Legacy)")]
	public class USPlayAnimEvent : USEventBase
	{
		public AnimationClip animationClip;

		public WrapMode wrapMode;

		public float playbackSpeed = 1f;

		public void Update()
		{
			if (this.wrapMode != WrapMode.Loop && this.animationClip)
			{
				base.Duration = this.animationClip.length / this.playbackSpeed;
			}
		}

		public override void FireEvent()
		{
			if (!this.animationClip)
			{
				Debug.Log("Attempting to play an animation on a GameObject but you haven't given the event an animation clip from USPlayAnimEvent::FireEvent");
				return;
			}
			Animation component = base.AffectedObject.GetComponent<Animation>();
			if (!component)
			{
				Debug.Log("Attempting to play an animation on a GameObject without an Animation Component from USPlayAnimEvent.FireEvent");
				return;
			}
			component.wrapMode = this.wrapMode;
			component.Play(this.animationClip.name);
			AnimationState animationState = component[this.animationClip.name];
			if (!animationState)
			{
				return;
			}
			animationState.speed = this.playbackSpeed;
		}

		public override void ProcessEvent(float deltaTime)
		{
			Animation animation = base.AffectedObject.GetComponent<Animation>();
			if (!this.animationClip)
			{
				return;
			}
			if (!animation)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Trying to play an animation : ",
					this.animationClip.name,
					" but : ",
					base.AffectedObject,
					" doesn't have an animation component, we will add one, this time, though you should add it manually"
				}));
				animation = base.AffectedObject.AddComponent<Animation>();
			}
			if (animation[this.animationClip.name] == null)
			{
				Debug.LogError("Trying to play an animation : " + this.animationClip.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
				animation.AddClip(this.animationClip, this.animationClip.name);
			}
			AnimationState animationState = animation[this.animationClip.name];
			if (!animation.IsPlaying(this.animationClip.name))
			{
				animation.wrapMode = this.wrapMode;
				animation.Play(this.animationClip.name);
			}
			animationState.speed = this.playbackSpeed;
			animationState.time = deltaTime * this.playbackSpeed;
			animationState.enabled = true;
			animation.Sample();
			animationState.enabled = false;
		}

		public override void StopEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			Animation component = base.AffectedObject.GetComponent<Animation>();
			if (component)
			{
				component.Stop();
			}
		}
	}
}
