using System;
using UnityEngine;

public class SmoothCrash : EZAnimation
{
	protected const float PIx2 = 6.28318548f;

	protected Vector3 start;

	protected Vector3 magnitude;

	protected Vector3 oscillations;

	protected GameObject subject;

	protected Transform subTrans;

	protected Vector3 temp;

	protected float factor;

	protected float invFactor;

	public SmoothCrash()
	{
		this.type = EZAnimation.ANIM_TYPE.SmoothCrash;
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
		this.invFactor = 1f - this.factor;
		this.factor *= 6.28318548f;
		this.temp.x = this.start.x + Mathf.Sin(this.factor * this.oscillations.x) * this.magnitude.x * this.invFactor;
		this.temp.y = this.start.y + Mathf.Sin(this.factor * this.oscillations.y) * this.magnitude.y * this.invFactor;
		this.temp.z = this.start.z + Mathf.Sin(this.factor * this.oscillations.z) * this.magnitude.z * this.invFactor;
		this.subTrans.localPosition = this.temp;
	}

	public static SmoothCrash Do(GameObject sub, Vector3 mag, Vector3 oscill, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		SmoothCrash smoothCrash = (SmoothCrash)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.SmoothCrash);
		smoothCrash.Start(sub, mag, oscill, dur, delay, startDel, del);
		return smoothCrash;
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
		this.Start(sub, sub.transform.localPosition, parms.vec, parms.vec2, parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		return true;
	}

	public void Start(GameObject sub, Vector3 mag, Vector3 oscill, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		if (sub == null)
		{
			return;
		}
		this.Start(sub, sub.transform.localPosition, mag, oscill, dur, delay, startDel, del);
	}

	public void Start(GameObject sub, Vector3 begin, Vector3 mag, Vector3 oscill, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
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
		if (oscill.x < 0f)
		{
			oscill.x = UnityEngine.Random.Range(1f, -oscill.x);
		}
		if (oscill.y < 0f)
		{
			oscill.y = UnityEngine.Random.Range(1f, -oscill.y);
		}
		if (oscill.z < 0f)
		{
			oscill.z = UnityEngine.Random.Range(1f, -oscill.z);
		}
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
