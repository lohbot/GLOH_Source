using System;
using UnityEngine;

public class AnimateScale : EZAnimation
{
	protected Vector3 start;

	protected Vector3 startPos = Vector3.zero;

	protected Vector3 delta;

	protected Vector3 deltaPos = Vector3.zero;

	protected Vector3 end;

	protected GameObject subject;

	protected Transform subTrans;

	protected Vector3 temp;

	protected Vector3 temp2;

	protected Vector3 tempPos = Vector3.zero;

	protected bool movePos;

	private float startWidth;

	private float startHeight;

	public AnimateScale()
	{
		this.type = EZAnimation.ANIM_TYPE.Scale;
	}

	public override object GetSubject()
	{
		return this.subject;
	}

	public override void _end()
	{
		if (this.subTrans != null)
		{
			this.subTrans.localScale = this.end;
			this.subTrans.localPosition = this.startPos;
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
		if (this.subTrans == null)
		{
			return;
		}
		base.WaitDone();
		if (base.Mode == EZAnimation.ANIM_MODE.By || base.Mode == EZAnimation.ANIM_MODE.To)
		{
			this.start = this.subTrans.localScale;
			this.end = this.start + this.delta;
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
		this.subTrans.localScale = this.temp;
		if (0f < this.temp.x && 0f < this.temp.y)
		{
			this.tempPos.x = this.startPos.x + (this.startWidth - this.startWidth * this.temp.x) / 2f;
			this.tempPos.y = this.startPos.y - (this.startHeight - this.startHeight * this.temp.y) / 2f;
		}
		else if (0f < this.temp.x && 0f > this.temp.y)
		{
			this.tempPos.x = (this.startPos.x - (this.startHeight + this.startHeight * this.temp.x) / 2f) * -1f;
			this.tempPos.y = this.startPos.y - (this.startWidth + this.startWidth * this.temp.y) / 2f;
		}
		else if (0f > this.temp.x && 0f < this.temp.y)
		{
			this.tempPos.x = this.startPos.x - (this.startWidth + this.startWidth * this.temp.x) / 2f;
			this.tempPos.y = this.startPos.y - (this.startHeight - this.startHeight * this.temp.y) / 2f;
		}
		this.tempPos.z = this.startPos.z;
		this.subTrans.localPosition = this.tempPos;
	}

	public static AnimateScale Do(GameObject sub, EZAnimation.ANIM_MODE mode, float width, float height, Vector3 begin, Vector3 dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		AnimateScale animateScale = (AnimateScale)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.Scale);
		animateScale.Start(sub, mode, width, height, begin, dest, interp, dur, delay, startDel, del);
		return animateScale;
	}

	public static AnimateScale Do(GameObject sub, EZAnimation.ANIM_MODE mode, bool increase, Vector3 begin, Vector3 dest, Vector3 beginPos, Vector3 destPos, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		AnimateScale animateScale = (AnimateScale)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.Scale);
		animateScale.Start(sub, mode, increase, begin, dest, beginPos, destPos, interp, dur, delay, startDel, del);
		return animateScale;
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
			this.Start(sub, parms.mode, 0f, 0f, parms.vec, parms.vec2, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		else
		{
			this.Start(sub, parms.mode, 0f, 0f, sub.transform.localScale, parms.vec, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		return true;
	}

	public void Start(GameObject sub, EZAnimation.ANIM_MODE mode, float width, float height, Vector3 begin, Vector3 dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.startWidth = width;
		this.startHeight = height;
		this.movePos = false;
		this.subject = sub;
		this.subTrans = this.subject.transform;
		this.start = begin;
		this.m_mode = mode;
		if (mode == EZAnimation.ANIM_MODE.By)
		{
			this.delta = Vector3.Scale(this.start, dest) - this.start;
		}
		else
		{
			this.delta = dest - this.start;
		}
		this.startPos = this.subject.transform.localPosition;
		this.deltaPos = this.subject.transform.localPosition;
		this.end = this.start + this.delta;
		this.interpolator = interp;
		this.duration = dur;
		this.m_wait = delay;
		this.completedDelegate = del;
		this.startDelegate = startDel;
		base.StartCommon();
		if (mode == EZAnimation.ANIM_MODE.FromTo && delay == 0f)
		{
			this.subTrans.localScale = this.start;
		}
		EZAnimator.instance.AddAnimation(this);
	}

	public void Start(GameObject sub, EZAnimation.ANIM_MODE mode, bool increase, Vector3 begin, Vector3 dest, Vector3 beginPos, Vector3 destPos, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.movePos = true;
		this.subject = sub;
		this.subTrans = this.subject.transform;
		this.start = begin;
		this.m_mode = mode;
		if (mode == EZAnimation.ANIM_MODE.By)
		{
			this.delta = Vector3.Scale(this.start, dest) - this.start;
			this.deltaPos = Vector3.Scale(this.start, dest) - this.start;
			this.startPos = beginPos;
			this.deltaPos = destPos - this.startPos;
		}
		else
		{
			this.delta = dest - this.start;
			if (increase)
			{
				this.startPos = Vector3.zero;
			}
			else
			{
				this.startPos = this.subject.transform.localPosition;
			}
			this.deltaPos = destPos - beginPos;
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
			this.subTrans.localScale = this.start;
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
			this.start = this.subject.transform.localScale;
			this.end = this.start + this.delta;
		}
		EZAnimator.instance.AddAnimation(this);
	}
}
