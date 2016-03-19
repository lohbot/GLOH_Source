using System;
using UnityEngine;

public class RunFrameAnimation : EZAnimation
{
	protected GameObject subject;

	private int index;

	private float[] frameWait = new float[3];

	public RunFrameAnimation()
	{
		this.type = EZAnimation.ANIM_TYPE.FrameAnimation;
		this.pingPong = false;
	}

	public override object GetSubject()
	{
		return this.subject;
	}

	public override bool Step(float timeDelta)
	{
		if (!this.running)
		{
			return true;
		}
		if (this.wait >= 0f)
		{
			this.wait -= timeDelta;
			if (this.wait < 0f)
			{
				if (!(null != this.subject))
				{
					return false;
				}
				Emoticon component = this.subject.GetComponent<Emoticon>();
				if (null != component)
				{
					component.DoAnim(this.index++);
					if (this.index >= component.states.Length)
					{
						this.index = 0;
					}
					this.wait = this.frameWait[this.index];
				}
			}
		}
		return true;
	}

	protected override void DoAnim()
	{
	}

	public static RunFrameAnimation Do(GameObject sub, float dur, float[] delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		RunFrameAnimation runFrameAnimation = (RunFrameAnimation)EZAnimator.instance.GetAnimation(EZAnimation.ANIM_TYPE.FrameAnimation);
		runFrameAnimation.Start(sub, dur, delay, startDel, del);
		return runFrameAnimation;
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
		float[] delay = new float[]
		{
			parms.delay,
			parms.delay,
			parms.delay
		};
		this.Start(sub, parms.duration, delay, null, new EZAnimation.CompletionDelegate(parms.transition.OnAnimEnd));
		return true;
	}

	public void Start(GameObject sub, float dur, float[] delay, EZAnimation.CompletionDelegate startDel, EZAnimation.CompletionDelegate del)
	{
		if (sub == null)
		{
			return;
		}
		this.subject = sub;
		this.duration = dur;
		this.frameWait = delay;
		this.completedDelegate = del;
		this.startDelegate = startDel;
		this.m_wait = this.frameWait[0];
		base.StartCommon();
		EZAnimator.instance.AddAnimation(this);
	}
}
