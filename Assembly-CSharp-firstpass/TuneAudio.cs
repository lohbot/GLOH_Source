using System;
using UnityEngine;

public class TuneAudio : EZAnimation
{
	protected float start;

	protected AudioSource subject;

	protected float delta;

	protected float end;

	public TuneAudio()
	{
		this.type = EZAnimation.ANIM_TYPE.TuneAudio;
		this.pingPong = false;
	}

	public override object GetSubject()
	{
		return this.subject;
	}

	public override void _end()
	{
		if (this.subject != null)
		{
			this.subject.volume = this.end;
		}
		base._end();
	}

	protected override void LoopReset()
	{
		if (base.Mode == EZAnimation.ANIM_MODE.By && !this.restartOnRepeat)
		{
			this.start = this.end;
			this.end = this.start + this.delta;
		}
	}

	protected override void WaitDone()
	{
		base.WaitDone();
		if (base.Mode == EZAnimation.ANIM_MODE.By)
		{
			this.start = this.subject.pitch;
		}
	}

	protected override void DoAnim()
	{
		if (this.subject == null)
		{
			base._stop();
			return;
		}
		this.subject.pitch = this.interpolator(this.timeElapsed, this.start, this.delta, this.interval);
	}

	public static TuneAudio Do(AudioSource audio, EZAnimation.ANIM_MODE mode, float begin, float dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		TuneAudio tuneAudio = (TuneAudio)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.TuneAudio);
		tuneAudio.Start(audio, mode, begin, dest, interp, dur, delay, startDel, del);
		return tuneAudio;
	}

	public static TuneAudio Do(AudioSource audio, EZAnimation.ANIM_MODE mode, float dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		TuneAudio tuneAudio = (TuneAudio)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.TuneAudio);
		tuneAudio.Start(audio, mode, dest, interp, dur, delay, startDel, del);
		return tuneAudio;
	}

	public override bool Start(GameObject sub, AnimParams parms)
	{
		if (sub == null)
		{
			return false;
		}
		this.subject = (AudioSource)sub.GetComponent(typeof(AudioSource));
		if (this.subject == null)
		{
			return false;
		}
		this.pingPong = parms.pingPong;
		this.restartOnRepeat = parms.restartOnRepeat;
		this.repeatDelay = parms.repeatDelay;
		if (parms.mode == EZAnimation.ANIM_MODE.FromTo)
		{
			this.Start(this.subject, parms.mode, parms.floatVal, parms.floatVal2, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		else
		{
			this.Start(this.subject, parms.mode, this.subject.pitch, parms.floatVal, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		return true;
	}

	public void Start(AudioSource audio, EZAnimation.ANIM_MODE mode, float dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.Start(audio, mode, audio.pitch, dest, interp, dur, delay, startDel, del);
	}

	public void Start(AudioSource sub, EZAnimation.ANIM_MODE mode, float begin, float dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.subject = sub;
		this.start = begin;
		this.m_mode = mode;
		if (mode == EZAnimation.ANIM_MODE.By)
		{
			this.delta = dest;
		}
		else
		{
			this.delta = dest - this.start;
		}
		this.end = this.start + this.delta;
		this.interpolator = interp;
		this.duration = dur;
		this.m_wait = delay;
		this.completedDelegate = del;
		this.startDelegate = startDel;
		base.StartCommon();
		EZAnimator.instance.Stop(this.subject, this.type, mode == EZAnimation.ANIM_MODE.By);
		if (mode == EZAnimation.ANIM_MODE.FromTo && delay == 0f)
		{
			this.subject.pitch = this.start;
		}
		EZAnimator.instance.AddAnimation(this);
	}
}
