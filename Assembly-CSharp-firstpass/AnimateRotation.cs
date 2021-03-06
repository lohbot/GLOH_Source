using System;
using UnityEngine;

public class AnimateRotation : EZAnimation
{
	protected Vector3 start;

	protected Vector3 delta;

	protected Vector3 end;

	protected GameObject subject;

	protected Transform subTrans;

	protected Vector3 temp;

	public AnimateRotation()
	{
		this.type = EZAnimation.ANIM_TYPE.Rotate;
	}

	public override object GetSubject()
	{
		return this.subject;
	}

	public override void _end()
	{
		if (this.subTrans != null)
		{
			this.subTrans.localEulerAngles = this.end;
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
			this.start = this.subTrans.localEulerAngles;
		}
	}

	protected override void DoAnim()
	{
		if (this.subTrans == null)
		{
			base._stop();
			return;
		}
		this.temp.x = this.interpolator(this.timeElapsed, this.start.x, this.delta.x, this.interval);
		this.temp.y = this.interpolator(this.timeElapsed, this.start.y, this.delta.y, this.interval);
		this.temp.z = this.interpolator(this.timeElapsed, this.start.z, this.delta.z, this.interval);
		this.subTrans.localEulerAngles = this.temp;
	}

	public static AnimateRotation Do(GameObject sub, EZAnimation.ANIM_MODE mode, Vector3 begin, Vector3 dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		AnimateRotation animateRotation = (AnimateRotation)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.Rotate);
		animateRotation.Start(sub, mode, begin, dest, interp, dur, delay, startDel, del);
		return animateRotation;
	}

	public static AnimateRotation Do(GameObject sub, EZAnimation.ANIM_MODE mode, Vector3 dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		AnimateRotation animateRotation = (AnimateRotation)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.Rotate);
		animateRotation.Start(sub, mode, dest, interp, dur, delay, startDel, del);
		return animateRotation;
	}

	public override bool Start(GameObject sub, AnimParams parms)
	{
		if (sub == null)
		{
			return false;
		}
		this.pingPong = parms.pingPong;
		this.restartOnRepeat = parms.restartOnRepeat;
		this.repeatDelay = parms.repeatDelay;
		if (parms.mode == EZAnimation.ANIM_MODE.FromTo)
		{
			this.Start(sub, parms.mode, parms.vec, parms.vec2, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		else
		{
			this.Start(sub, parms.mode, sub.transform.localEulerAngles, parms.vec, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		return true;
	}

	public void Start(GameObject sub, EZAnimation.ANIM_MODE mode, Vector3 dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		if (sub == null)
		{
			return;
		}
		this.Start(sub, mode, sub.transform.localEulerAngles, dest, interp, dur, delay, startDel, del);
	}

	public void Start(GameObject sub, EZAnimation.ANIM_MODE mode, Vector3 begin, Vector3 dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.subject = sub;
		this.subTrans = this.subject.transform;
		this.start = begin;
		this.m_mode = mode;
		if (mode == EZAnimation.ANIM_MODE.By)
		{
			this.delta = dest;
		}
		else
		{
			this.delta = new Vector3(dest.x - this.start.x, dest.y - this.start.y, dest.z - this.start.z);
		}
		this.end = this.start + this.delta;
		this.interpolator = interp;
		this.duration = dur;
		this.m_wait = delay;
		this.completedDelegate = del;
		this.startDelegate = startDel;
		base.StartCommon();
		if (mode == EZAnimation.ANIM_MODE.FromTo && delay == 0f)
		{
			this.subTrans.localEulerAngles = this.start;
		}
		EZAnimator.instance.AddAnimation(this);
	}

	public void Start()
	{
		if (this.subject == null)
		{
			return;
		}
		this.direction = 1f;
		this.timeElapsed = 0f;
		this.wait = this.m_wait;
		if (this.m_mode == EZAnimation.ANIM_MODE.By)
		{
			this.start = this.subject.transform.localEulerAngles;
			this.end = this.start + this.delta;
		}
		EZAnimator.instance.AddAnimation(this);
	}
}
