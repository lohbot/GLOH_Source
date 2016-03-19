using System;
using UnityEngine;

public class Crash : EZAnimation
{
	protected Vector3 start;

	protected Vector3 magnitude;

	protected GameObject subject;

	protected Transform subTrans;

	protected Vector3 tempMag;

	protected Vector3 temp;

	protected float factor;

	public Crash()
	{
		this.type = EZAnimation.ANIM_TYPE.Crash;
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
			this.subTrans.localPosition = this.start;
		}
		base._end();
	}

	protected override void WaitDone()
	{
		base.WaitDone();
		this.start = this.subTrans.localPosition;
	}

	protected override void DoAnim()
	{
		if (this.subTrans == null)
		{
			base._stop();
			return;
		}
		this.factor = this.timeElapsed / this.interval;
		this.tempMag.x = this.magnitude.x - this.factor * this.magnitude.x;
		this.tempMag.y = this.magnitude.y - this.factor * this.magnitude.y;
		this.tempMag.z = this.magnitude.z - this.factor * this.magnitude.z;
		this.temp.x = this.start.x + UnityEngine.Random.Range(-this.tempMag.x, this.tempMag.x);
		this.temp.y = this.start.y + UnityEngine.Random.Range(-this.tempMag.y, this.tempMag.y);
		this.temp.z = this.start.z + UnityEngine.Random.Range(-this.tempMag.z, this.tempMag.z);
		this.subTrans.localPosition = this.temp;
	}

	public static Crash Do(GameObject sub, Vector3 mag, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		Crash crash = (Crash)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.Crash);
		crash.Start(sub, mag, dur, delay, startDel, del);
		return crash;
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
		this.Start(sub, sub.transform.localPosition, parms.vec, parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		return true;
	}

	public void Start(GameObject sub, Vector3 mag, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		if (sub == null)
		{
			return;
		}
		this.Start(sub, sub.transform.localPosition, mag, dur, delay, startDel, del);
	}

	public void Start(GameObject sub, Vector3 begin, Vector3 mag, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.subject = sub;
		this.subTrans = this.subject.transform;
		this.start = begin;
		this.subTrans.localPosition = this.start;
		if (mag.x < 0f)
		{
			mag.x = UnityEngine.Random.Range(1f, -mag.x);
		}
		if (mag.y < 0f)
		{
			mag.y = UnityEngine.Random.Range(1f, -mag.y);
		}
		if (mag.z < 0f)
		{
			mag.z = UnityEngine.Random.Range(1f, -mag.z);
		}
		this.magnitude = mag;
		this.m_mode = EZAnimation.ANIM_MODE.By;
		this.duration = dur;
		this.m_wait = delay;
		this.completedDelegate = del;
		this.startDelegate = startDel;
		base.StartCommon();
		EZAnimator.instance.AddAnimation(this);
	}
}
