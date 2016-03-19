using System;
using UnityEngine;

public class FadeSprite : EZAnimation
{
	protected Color start;

	protected Color delta;

	protected Color end;

	protected SpriteRoot sprite;

	protected Color temp;

	public FadeSprite()
	{
		this.type = EZAnimation.ANIM_TYPE.FadeSprite;
	}

	public override object GetSubject()
	{
		return this.sprite;
	}

	public override void _end()
	{
		if (this.sprite != null)
		{
			this.sprite.SetColor(this.end);
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
			this.start = this.sprite.color;
		}
	}

	protected override void DoAnim()
	{
		if (this.sprite == null)
		{
			base._stop();
			return;
		}
		this.temp.r = this.interpolator(this.timeElapsed, this.start.r, this.delta.r, this.interval);
		this.temp.g = this.interpolator(this.timeElapsed, this.start.g, this.delta.g, this.interval);
		this.temp.b = this.interpolator(this.timeElapsed, this.start.b, this.delta.b, this.interval);
		this.temp.a = this.interpolator(this.timeElapsed, this.start.a, this.delta.a, this.interval);
		this.sprite.SetColor(this.temp);
	}

	public static FadeSprite Do(SpriteRoot sprt, EZAnimation.ANIM_MODE mode, Color begin, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		FadeSprite fadeSprite = (FadeSprite)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.FadeSprite);
		fadeSprite.Start(sprt, mode, begin, dest, interp, dur, delay, startDel, del);
		return fadeSprite;
	}

	public static FadeSprite Do(SpriteRoot sprt, EZAnimation.ANIM_MODE mode, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		FadeSprite fadeSprite = (FadeSprite)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.FadeSprite);
		fadeSprite.Start(sprt, mode, dest, interp, dur, delay, startDel, del);
		return fadeSprite;
	}

	public override bool Start(GameObject sub, AnimParams parms)
	{
		if (sub == null)
		{
			return false;
		}
		this.sprite = (SpriteRoot)sub.GetComponent(typeof(SpriteRoot));
		if (this.sprite == null)
		{
			return false;
		}
		this.pingPong = parms.pingPong;
		this.restartOnRepeat = parms.restartOnRepeat;
		this.repeatDelay = parms.repeatDelay;
		if (parms.mode == EZAnimation.ANIM_MODE.FromTo)
		{
			this.Start(this.sprite, parms.mode, parms.color, parms.color2, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		else
		{
			this.Start(this.sprite, parms.mode, this.sprite.color, parms.color, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		return true;
	}

	public void Start(SpriteRoot sprt, EZAnimation.ANIM_MODE mode, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.Start(sprt, mode, sprt.color, dest, interp, dur, delay, startDel, del);
	}

	public void Start(SpriteRoot sprt, EZAnimation.ANIM_MODE mode, Color begin, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.sprite = sprt;
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
			this.sprite.SetColor(this.start);
		}
		EZAnimator.instance.AddAnimation(this);
	}

	public void Start()
	{
		if (this.sprite == null)
		{
			return;
		}
		this.direction = 1f;
		this.timeElapsed = 0f;
		this.wait = this.m_wait;
		if (this.m_mode == EZAnimation.ANIM_MODE.By)
		{
			this.start = this.sprite.color;
			this.end = this.start + this.delta;
		}
	}
}
