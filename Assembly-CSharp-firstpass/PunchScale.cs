using System;
using UnityEngine;

public class PunchScale : EZAnimation
{
	protected Vector3 start;

	protected Vector3 startPosition;

	protected Vector3 magnitude;

	protected GameObject subject;

	protected Transform subTrans;

	protected Vector3 temp;

	protected Vector3 temp2;

	protected float factor;

	private float startWidth;

	private float startHeight;

	public PunchScale()
	{
		this.type = EZAnimation.ANIM_TYPE.PunchScale;
	}

	public override object GetSubject()
	{
		return this.subject;
	}

	public override void _end()
	{
		if (this.subTrans != null)
		{
			this.subTrans.localScale = this.start;
			this.subTrans.localPosition = this.startPosition;
		}
		base._end();
	}

	protected override void WaitDone()
	{
		base.WaitDone();
		if (this.subTrans != null)
		{
			this.start = this.subTrans.localScale;
		}
	}

	protected override void DoAnim()
	{
		if (this.subTrans == null)
		{
			base._stop();
			return;
		}
		float num = EZAnimation.punch(this.magnitude.x, this.factor);
		float num2 = EZAnimation.punch(this.magnitude.x, this.factor);
		float num3 = EZAnimation.punch(this.magnitude.x, this.factor);
		this.factor = this.timeElapsed / this.interval;
		this.temp.x = this.start.x + num;
		this.temp.y = this.start.y + num2;
		this.temp.z = this.start.z + num3;
		this.subTrans.localScale = this.temp;
		this.temp2.x = this.startPosition.x - this.startWidth * 0.5f * num;
		this.temp2.y = this.startPosition.y + this.startHeight * 0.5f * num2;
		this.temp2.z = this.startPosition.z;
		this.subTrans.localPosition = this.temp2;
	}

	public static PunchScale Do(GameObject sub, Vector3 mag, float width, float height, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		PunchScale punchScale = (PunchScale)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.PunchScale);
		punchScale.Start(sub, mag, width, height, dur, delay, startDel, del);
		return punchScale;
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
		this.Start(sub, sub.transform.localScale, parms.vec, 0f, 0f, parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		return true;
	}

	public void Start(GameObject sub, Vector3 mag, float width, float height, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		if (sub == null)
		{
			return;
		}
		this.Start(sub, sub.transform.localScale, mag, width, height, dur, delay, startDel, del);
	}

	public void Start(GameObject sub, Vector3 begin, Vector3 mag, float width, float height, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.subject = sub;
		this.subTrans = this.subject.transform;
		this.start = begin;
		this.startPosition = this.subTrans.localPosition;
		this.subTrans.localScale = this.start;
		this.startWidth = width;
		this.startHeight = height;
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
