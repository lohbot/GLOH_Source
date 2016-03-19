using System;
using UnityEngine;

public class Shake : EZAnimation
{
	protected const float PIx2 = 6.28318548f;

	protected Vector3 start;

	protected Vector3 magnitude;

	protected float oscillations;

	protected GameObject subject;

	protected Transform subTrans;

	protected Vector3 temp;

	protected float factor;

	public Shake()
	{
		this.type = EZAnimation.ANIM_TYPE.Shake;
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
		this.factor = Mathf.Sin(this.timeElapsed / this.interval * 6.28318548f * this.oscillations);
		this.temp.x = this.start.x + this.factor * this.magnitude.x;
		this.temp.y = this.start.y + this.factor * this.magnitude.y;
		this.temp.z = this.start.z + this.factor * this.magnitude.z;
		this.subTrans.localPosition = this.temp;
	}

	public static Shake Do(GameObject sub, Vector3 mag, float oscill, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		Shake shake = (Shake)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.Shake);
		shake.Start(sub, mag, oscill, dur, delay, startDel, del);
		return shake;
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
		this.Start(sub, sub.transform.localPosition, parms.vec, parms.floatVal, parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		return true;
	}

	public void Start(GameObject sub, Vector3 mag, float oscill, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		if (sub == null)
		{
			return;
		}
		this.Start(sub, sub.transform.localPosition, mag, oscill, dur, delay, startDel, del);
	}

	public void Start(GameObject sub, Vector3 begin, Vector3 mag, float oscill, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
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
		if (oscill < 0f)
		{
			oscill = UnityEngine.Random.Range(1f, -oscill);
		}
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
