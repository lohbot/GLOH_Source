using System;
using UnityEngine;

namespace WellFired
{
	[USequencerEvent("Animation (Legacy)/Play AnimationLayer"), USequencerFriendlyName("Play Animation (Legacy)")]
	public class USPlayAnimLayerEvent : USEventBase
	{
		public AnimationClip animationClip;

		public int AniLayer = 1;

		public float FadeTime = 0.3f;

		public void Update()
		{
			if (this.animationClip && base.Duration <= 0f)
			{
				base.Duration = this.animationClip.length;
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
			this._AniPlay();
		}

		public override void ProcessEvent(float deltaTime)
		{
			Animation animation = base.AffectedObject.GetComponent<Animation>();
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
				this._AniPlay();
			}
			animationState.time = deltaTime;
			if (deltaTime >= this.FadeTime)
			{
				animationState.enabled = true;
				animation.Sample();
				animationState.enabled = false;
				animation.Play(this.animationClip.name);
			}
		}

		public override void EndEvent()
		{
			foreach (AnimationState animationState in base.AffectedObject.animation)
			{
				if (animationState.layer == this.AniLayer)
				{
					animationState.wrapMode = WrapMode.Default;
					animationState.clip.wrapMode = WrapMode.Default;
				}
			}
		}

		public override void StopEvent()
		{
			Animation component = base.AffectedObject.GetComponent<Animation>();
			if (component)
			{
				AnimationState x = component[this.animationClip.name];
				if (x != null)
				{
					component.Stop();
				}
			}
		}

		private void _AniPlay()
		{
			Animation component = base.AffectedObject.GetComponent<Animation>();
			if (component[this.animationClip.name] == null)
			{
				component.AddClip(this.animationClip, this.animationClip.name);
			}
			component[this.animationClip.name].wrapMode = WrapMode.Loop;
			component[this.animationClip.name].layer = this.AniLayer;
			component.CrossFade(this.animationClip.name, this.FadeTime);
			component.Play(this.animationClip.name);
		}
	}
}
