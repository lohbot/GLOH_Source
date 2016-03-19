using System;
using UnityEngine;

public class RunAnimClip : EZAnimation
{
	protected GameObject subject;

	protected string m_clip;

	protected bool waitForClip = true;

	protected bool playedYet;

	protected float blending;

	public RunAnimClip()
	{
		this.type = EZAnimation.ANIM_TYPE.AnimClip;
		this.pingPong = false;
	}

	public override object GetSubject()
	{
		return this.subject;
	}

	public override bool Step(float timeDelta)
	{
		if (this.wait > 0f)
		{
			this.wait -= timeDelta;
			if (this.wait >= 0f)
			{
				return true;
			}
			timeDelta -= timeDelta + this.wait;
		}
		if (!this.playedYet)
		{
			if (this.duration == 0f && this.blending == 0f)
			{
				this.subject.animation.Play(this.m_clip);
			}
			else
			{
				this.subject.animation.Blend(this.m_clip, this.blending, this.duration);
			}
			this.playedYet = true;
			return true;
		}
		if (this.subject.animation.IsPlaying(this.m_clip))
		{
			return true;
		}
		this._end();
		return false;
	}

	protected override void DoAnim()
	{
	}

	public static RunAnimClip Do(GameObject sub, string clip, float blend, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		RunAnimClip runAnimClip = (RunAnimClip)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.AnimClip);
		runAnimClip.Start(sub, clip, blend, dur, delay, startDel, del);
		return runAnimClip;
	}

	public override bool Start(GameObject sub, AnimParams parms)
	{
		if (sub == null)
		{
			return false;
		}
		if (sub.animation == null)
		{
			return false;
		}
		this.pingPong = parms.pingPong;
		this.restartOnRepeat = parms.restartOnRepeat;
		this.repeatDelay = parms.repeatDelay;
		this.Start(sub, parms.strVal, parms.floatVal, parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		return true;
	}

	public void Start(GameObject sub, string clip, float blend, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		if (sub == null)
		{
			return;
		}
		if (sub.animation == null)
		{
			return;
		}
		this.playedYet = false;
		this.subject = sub;
		this.m_clip = clip;
		this.blending = blend;
		this.duration = dur;
		this.m_wait = delay;
		this.completedDelegate = del;
		this.startDelegate = startDel;
		base.StartCommon();
		EZAnimator.instance.AddAnimation(this);
	}
}
