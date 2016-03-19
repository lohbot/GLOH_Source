using System;
using UnityEngine;

public class ShakeRotation : EZAnimation
{
	protected const float PIx2 = 6.28318548f;

	protected Vector3 start;

	protected Vector3 magnitude;

	protected float oscillations;

	protected GameObject subject;

	protected Transform subTrans;

	protected Vector3 temp;

	protected float factor;

	public ShakeRotation()
	{
		this.type = EZAnimation.ANIM_TYPE.ShakeRotation;
		this.pingPong = false;
	}

	public override object GetSubject()
	{
		return this.subject;
	}

	public override void _end()
	{
		if (this.subTrans != null)
		{
			this.subTrans.localEulerAngles = this.start;
		}
		base._end();
	}

	protected override void WaitDone()
	{
		base.WaitDone();
		this.start = this.subTrans.localEulerAngles;
	}

	protected override void DoAnim()
	{
		if (this.subTrans == null)
		{
			base._stop();
			return;
		}
		this.factor = Mathf.Sin(this.timeElapsed / this.interval * 6.28318548f * this.oscillations);
		this.temp.x = this.start.x + this.factor * this.magnitude.x;
		this.temp.y = this.start.y + this.factor * this.magnitude.y;
		this.temp.z = this.start.z + this.factor * this.magnitude.z;
		this.subTrans.localRotation = Quaternion.Euler(this.temp);
	}

	public static ShakeRotation Do(GameObject sub, Vector3 mag, float oscill, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		ShakeRotation shakeRotation = (ShakeRotation)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.ShakeRotation);
		shakeRotation.Start(sub, mag, oscill, dur, delay, startDel, del);
		return shakeRotation;
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
		this.Start(sub, sub.transform.localEulerAngles, parms.vec, parms.floatVal, parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		return true;
	}

	public void Start(GameObject sub, Vector3 mag, float oscill, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		if (sub == null)
		{
			return;
		}
		this.Start(sub, sub.transform.localEulerAngles, mag, oscill, dur, delay, startDel, del);
	}

	public void Start(GameObject sub, Vector3 begin, Vector3 mag, float oscill, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.subject = sub;
		this.subTrans = this.subject.transform;
		this.start = begin;
		this.subTrans.localEulerAngles = this.start;
		this.magnitude = mag;
		this.oscillations = oscill;
		this.m_mode = EZAnimation.ANIM_MODE.By;
		this.duration = dur;
		this.m_wait = delay;
		this.completedDelegate = del;
		this.startDelegate = startDel;
		base.StartCommon();
		EZAnimator.instance.AddAnimation(this);
	}
}
