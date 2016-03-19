using System;
using UnityEngine;

public class FadeMaterial : EZAnimation
{
	protected Color start;

	protected Color delta;

	protected Color end;

	protected Material mat;

	protected Color temp;

	public FadeMaterial()
	{
		this.type = EZAnimation.ANIM_TYPE.FadeMaterial;
	}

	public override object GetSubject()
	{
		return this.mat;
	}

	public override void _end()
	{
		if (this.mat != null)
		{
			this.mat.color = this.end;
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
			this.start = this.mat.color;
		}
	}

	protected override void DoAnim()
	{
		if (this.mat == null)
		{
			base._stop();
			return;
		}
		this.temp.r = this.interpolator(this.timeElapsed, this.start.r, this.delta.r, this.interval);
		this.temp.g = this.interpolator(this.timeElapsed, this.start.g, this.delta.g, this.interval);
		this.temp.b = this.interpolator(this.timeElapsed, this.start.b, this.delta.b, this.interval);
		this.temp.a = this.interpolator(this.timeElapsed, this.start.a, this.delta.a, this.interval);
		this.mat.color = this.temp;
	}

	public static FadeMaterial Do(Material material, EZAnimation.ANIM_MODE mode, Color begin, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		FadeMaterial fadeMaterial = (FadeMaterial)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.FadeMaterial);
		fadeMaterial.Start(material, mode, begin, dest, interp, dur, delay, startDel, del);
		return fadeMaterial;
	}

	public static FadeMaterial Do(Material material, EZAnimation.ANIM_MODE mode, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		FadeMaterial fadeMaterial = (FadeMaterial)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.FadeMaterial);
		fadeMaterial.Start(material, mode, dest, interp, dur, delay, startDel, del);
		return fadeMaterial;
	}

	public override bool Start(GameObject sub, AnimParams parms)
	{
		if (sub == null)
		{
			return false;
		}
		if (sub.renderer == null)
		{
			return false;
		}
		if (sub.renderer.material == null)
		{
			return false;
		}
		this.pingPong = parms.pingPong;
		this.restartOnRepeat = parms.restartOnRepeat;
		this.repeatDelay = parms.repeatDelay;
		if (parms.mode == EZAnimation.ANIM_MODE.FromTo)
		{
			this.Start(sub.renderer.material, parms.mode, parms.color, parms.color2, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		else
		{
			this.Start(sub.renderer.material, parms.mode, sub.renderer.material.color, parms.color, EZAnimation.GetInterpolator(parms.easing), parms.duration, parms.delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		}
		return true;
	}

	public void Start(Material material, EZAnimation.ANIM_MODE mode, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.Start(material, mode, material.color, dest, interp, dur, delay, startDel, del);
	}

	public void Start(Material material, EZAnimation.ANIM_MODE mode, Color begin, Color dest, EZAnimation.Interpolator interp, float dur, float delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		this.mat = material;
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
		EZAnimator.instance.Stop(this.mat, this.type, mode == EZAnimation.ANIM_MODE.By);
		if (mode == EZAnimation.ANIM_MODE.FromTo && delay == 0f)
		{
			this.mat.color = this.start;
		}
		EZAnimator.instance.AddAnimation(this);
	}

	public void Start()
	{
		if (this.mat == null)
		{
			return;
		}
		this.direction = 1f;
		this.timeElapsed = 0f;
		this.wait = this.m_wait;
		if (this.m_mode == EZAnimation.ANIM_MODE.By)
		{
			this.start = this.mat.color;
			this.end = this.start + this.delta;
		}
		EZAnimator.instance.AddAnimation(this);
	}
}
