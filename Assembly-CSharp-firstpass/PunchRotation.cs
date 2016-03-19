using System;
using UnityEngine;

public class PunchRotation : EZAnimation
{
	protected Vector3 start;

	protected Vector3 magnitude;

	protected GameObject subject;

	protected Transform subTrans;

	protected Vector3 temp;

	protected float factor;

	public PunchRotation()
	{
		this.type = EZAnimation.ANIM_TYPE.PunchRotation;
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
		this.factor = this.timeElapsed / this.interval;
		this.temp.x = this.start.x + EZAnimation.punch(this.magnitude.x, this.factor);
		this.temp.y = this.start.y + EZAnimation.punch(this.magnitude.y, this.factor);
		this.temp.z = this.start.z + EZAnimation.punch(this.magnitude.z, this.factor);
		this.subTrans.localRotation = Quaternion.Euler(this.temp);
	}

	public static PunchRotation Do(GameObject sub, Vector3 mag, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		PunchRotation punchRotation = (PunchRotation)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.PunchRotation);
		punchRotation.Start(sub, mag, dur, delay, startDel, del);
		return punchRotation;
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
		this.Start(sub, sub.transform.localEulerAngles, parms.vec, parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		return true;
	}

	public void Start(GameObject sub, Vector3 mag, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		if (sub == null)
		{
			return;
		}
		this.Start(sub, sub.transform.localEulerAngles, mag, dur, delay, startDel, del);
	}

	public void Start(GameObject sub, Vector3 begin, Vector3 mag, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.subject = sub;
		this.subTrans = this.subject.transform;
		this.start = begin;
		this.subTrans.localEulerAngles = this.start;
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
