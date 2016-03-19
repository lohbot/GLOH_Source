using System;
using UnityEngine;

public class FadeText : EZAnimation
{
	protected Color start;

	protected Color delta;

	protected Color end;

	protected SpriteText text;

	protected Color temp;

	public FadeText()
	{
		this.type = EZAnimation.ANIM_TYPE.FadeText;
	}

	public override object GetSubject()
	{
		return this.text;
	}

	public override void _end()
	{
		if (this.text != null)
		{
			this.text.SetColor(this.end);
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
		if ((base.Mode == EZAnimation.ANIM_MODE.By || base.Mode == EZAnimation.ANIM_MODE.To) && this.text != null)
		{
			this.start = this.text.color;
			this.end = this.start + this.delta;
		}
	}

	protected override void DoAnim()
	{
		if (this.text == null)
		{
			base._stop();
			return;
		}
		this.temp.r = this.interpolator(this.timeElapsed, this.start.r, this.delta.r, this.interval);
		this.temp.g = this.interpolator(this.timeElapsed, this.start.g, this.delta.g, this.interval);
		this.temp.b = this.interpolator(this.timeElapsed, this.start.b, this.delta.b, this.interval);
		this.temp.a = this.interpolator(this.timeElapsed, this.start.a, this.delta.a, this.interval);
		this.text.SetColor(this.temp);
	}

	public static FadeText Do(SpriteText txt, EZAnimation.ANIM_MODE mode, Color begin, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		FadeText fadeText = (FadeText)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.FadeText);
		fadeText.Start(txt, mode, begin, dest, interp, dur, delay, startDel, del);
		return fadeText;
	}

	public static FadeText Do(SpriteText txt, EZAnimation.ANIM_MODE mode, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		FadeText fadeText = (FadeText)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.FadeText);
		fadeText.Start(txt, mode, dest, interp, dur, delay, startDel, del);
		return fadeText;
	}

	public override bool Start(GameObject sub, AnimParams parms)
	{
		if (sub == null)
		{
			return false;
		}
		this.text = (SpriteText)sub.GetComponent(typeof(SpriteText));
		if (this.text == null)
		{
			return false;
		}
		this.pingPong = parms.pingPong;
		this.restartOnRepeat = parms.restartOnRepeat;
		this.repeatDelay = parms.repeatDelay;
		if (parms.mode == EZAnimation.ANIM_MODE.FromTo)
		{
			this.Start(this.text, parms.mode, parms.color, parms.color2, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		else
		{
			this.Start(this.text, parms.mode, this.text.color, parms.color, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		return true;
	}

	public void Start(SpriteText txt, EZAnimation.ANIM_MODE mode, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.Start(txt, mode, txt.color, dest, interp, dur, delay, startDel, del);
	}

	public void Start(SpriteText txt, EZAnimation.ANIM_MODE mode, Color begin, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.text = txt;
		this.start = begin;
		this.m_mode = mode;
		if (mode == EZAnimation.ANIM_MODE.By)
		{
			this.delta = dest;
		}
		else
		{
			this.delta = new Color(dest.r - this.start.r, dest.g - this.start.g, dest.b - this.start.b, dest.a - this.start.a);
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
			this.text.SetColor(this.start);
		}
		EZAnimator.instance.AddAnimation(this);
	}
}
