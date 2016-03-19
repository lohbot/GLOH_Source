using System;
using UnityEngine;

public class ACTSource_Ani : ACTSource
{
	private const float DEFAULT_TIME = -1f;

	private Animation mSrcAni;

	private float mAniStateTime = -1f;

	public ACTSource_Ani(Animation Src)
	{
		this.mSrcAni = Src;
	}

	protected override bool IsValid()
	{
		return null != this.mSrcAni && null != this.mSrcAni.clip;
	}

	protected override void Disable()
	{
		string name = this.mSrcAni.clip.name;
		AnimationState animationState = this.mSrcAni[name];
		if (null != animationState && this.mSrcAni.isPlaying)
		{
			this.mSrcAni.playAutomatically = false;
			this.mAniStateTime = animationState.time;
			this.mSrcAni.Stop();
		}
	}

	protected override void Active()
	{
		string name = this.mSrcAni.clip.name;
		AnimationState animationState = this.mSrcAni[name];
		if (null != animationState && this.mAniStateTime != -1f)
		{
			animationState.time = this.mAniStateTime;
			this.mSrcAni.Play(name);
		}
	}
}
