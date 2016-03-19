using System;
using UnityEngine;

public abstract class EZAnimation : IEZLinkedListItem<EZAnimation>
{
	public enum ANIM_TYPE
	{
		AnimClip,
		FadeSprite,
		FadeMaterial,
		FadeText,
		Translate,
		PunchPosition,
		Crash,
		SmoothCrash,
		Shake,
		Scale,
		PunchScale,
		Rotate,
		PunchRotation,
		ShakeRotation,
		CrashRotation,
		FadeAudio,
		TuneAudio,
		FrameAnimation
	}

	public enum ANIM_MODE
	{
		By,
		To,
		FromTo
	}

	public enum EASING_TYPE
	{
		Default,
		Linear,
		BackIn,
		BackOut,
		BackInOut,
		BackOutIn,
		BounceIn,
		BounceOut,
		BounceInOut,
		BounceOutIn,
		CircularIn,
		CircularOut,
		CircularInOut,
		CircularOutIn,
		CubicIn,
		CubicOut,
		CubicInOut,
		CubicOutIn,
		ElasticIn,
		ElasticOut,
		ElasticInOut,
		ElasticOutIn,
		ExponentialIn,
		ExponentialOut,
		ExponentialInOut,
		ExponentialOutIn,
		QuadraticIn,
		QuadraticOut,
		QuadraticInOut,
		QuadraticOutIn,
		QuarticIn,
		QuarticOut,
		QuarticInOut,
		QuarticOutIn,
		QuinticIn,
		QuinticOut,
		QuinticInOut,
		QuinticOutIn,
		SinusoidalIn,
		SinusoidalOut,
		SinusoidalInOut,
		SinusoidalOutIn,
		Spring
	}

	public delegate void CompletionDelegate(EZAnimation anim);

	public delegate float Interpolator(float time, float start, float delta, float duration);

	public EZAnimation.ANIM_TYPE type;

	public bool pingPong = true;

	public bool repeatDelay;

	public bool restartOnRepeat;

	public bool running;

	protected bool m_paused;

	protected object data;

	protected EZAnimation.ANIM_MODE m_mode;

	protected float direction = 1f;

	protected float timeElapsed;

	protected float wait;

	protected float m_wait;

	protected float duration;

	protected float interval;

	protected EZAnimation.CompletionDelegate completedDelegate;

	protected EZAnimation.CompletionDelegate startDelegate;

	protected EZAnimation.Interpolator interpolator;

	protected EZAnimation m_prev;

	protected EZAnimation m_next;

	public object Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
		}
	}

	public float Duration
	{
		get
		{
			return this.duration;
		}
	}

	public float Wait
	{
		get
		{
			return this.wait;
		}
	}

	public bool Paused
	{
		get
		{
			return this.m_paused;
		}
		set
		{
			this.m_paused = (this.running && value);
		}
	}

	public EZAnimation.ANIM_MODE Mode
	{
		get
		{
			return this.m_mode;
		}
	}

	public EZAnimation.CompletionDelegate CompletedDelegate
	{
		get
		{
			return this.completedDelegate;
		}
		set
		{
			this.completedDelegate = value;
		}
	}

	public EZAnimation.CompletionDelegate StartDelegate
	{
		get
		{
			return this.startDelegate;
		}
		set
		{
			this.startDelegate = value;
		}
	}

	public EZAnimation prev
	{
		get
		{
			return this.m_prev;
		}
		set
		{
			this.m_prev = value;
		}
	}

	public EZAnimation next
	{
		get
		{
			return this.m_next;
		}
		set
		{
			this.m_next = value;
		}
	}

	public void Clear()
	{
		this.completedDelegate = null;
		this.startDelegate = null;
		this.data = null;
	}

	public abstract bool Start(GameObject sub, AnimParams parms);

	protected abstract void DoAnim();

	public virtual bool Step(float timeDelta)
	{
		if (this.m_paused)
		{
			return true;
		}
		if (this.wait > 0f)
		{
			this.wait -= timeDelta;
			if (this.wait >= 0f)
			{
				return true;
			}
			if (this.startDelegate != null)
			{
				this.startDelegate(this);
			}
			timeDelta -= timeDelta + this.wait;
			this.WaitDone();
		}
		this.timeElapsed += timeDelta * this.direction;
		if (this.timeElapsed >= this.interval || this.timeElapsed < 0f)
		{
			if (this.duration >= 0f)
			{
				this._end();
				return false;
			}
			if (this.pingPong)
			{
				if (this.timeElapsed >= this.interval)
				{
					this.direction = -1f;
					this.timeElapsed = this.interval - (this.timeElapsed - this.interval);
				}
				else
				{
					if (this.repeatDelay)
					{
						this.wait = this.m_wait - (this.timeElapsed - this.interval);
					}
					else
					{
						if (this.startDelegate != null)
						{
							this.startDelegate(this);
						}
						this.timeElapsed *= -1f;
					}
					this.direction = 1f;
				}
			}
			else
			{
				if (this.repeatDelay)
				{
					this.wait = this.m_wait;
				}
				else if (this.startDelegate != null)
				{
					this.startDelegate(this);
				}
				this.LoopReset();
				this.timeElapsed -= this.interval;
			}
		}
		this.DoAnim();
		return true;
	}

	public virtual void Stop()
	{
		EZAnimator.instance.StopAnimation(this);
	}

	public void _stop()
	{
		try
		{
			this.running = false;
			this.Paused = false;
			if (this.completedDelegate != null)
			{
				this.completedDelegate(this);
			}
		}
		catch (Exception ex)
		{
			Debug.Log("EZAnimator _stop() completedDelegate error : " + ex.ToString());
		}
	}

	public void End()
	{
		EZAnimator.instance.StopAnimation(this, true);
	}

	public void _cancel()
	{
		this.running = false;
		this.Clear();
	}

	public virtual void _end()
	{
		this._stop();
	}

	protected virtual void LoopReset()
	{
	}

	public abstract object GetSubject();

	protected virtual void WaitDone()
	{
	}

	protected void StartCommon()
	{
		this.wait = this.m_wait;
		if (this.wait == 0f && this.startDelegate != null)
		{
			this.startDelegate(this);
		}
		this.interval = Mathf.Abs(this.duration);
		this.direction = 1f;
		this.timeElapsed = 0f;
		this.Paused = false;
	}

	public void ResetDefaults()
	{
		this.pingPong = true;
		this.restartOnRepeat = false;
		this.data = null;
		this.completedDelegate = null;
		this.startDelegate = null;
	}

	public static float linear(float time, float start, float delta, float duration)
	{
		return delta * time / duration + start;
	}

	public static float quadraticIn(float time, float start, float delta, float duration)
	{
		time /= duration;
		return delta * time * time + start;
	}

	public static float quadraticOut(float time, float start, float delta, float duration)
	{
		time /= duration;
		return -delta * time * (time - 2f) + start;
	}

	public static float quadraticInOut(float time, float start, float delta, float duration)
	{
		time /= duration / 2f;
		if (time < duration / 2f)
		{
			return delta / 2f * time * time + start;
		}
		time -= 1f;
		return -delta / 2f * (time * (time - 2f) - 1f) + start;
	}

	public static float quadraticOutIn(float time, float start, float delta, float duration)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.quadraticOut(time * 2f, start, delta / 2f, duration);
		}
		return EZAnimation.quadraticIn(time * 2f - duration, start + delta / 2f, delta / 2f, duration);
	}

	public static float cubicIn(float time, float start, float delta, float duration)
	{
		time /= duration;
		return delta * time * time * time + start;
	}

	public static float cubicOut(float time, float start, float delta, float duration)
	{
		time /= duration;
		time -= 1f;
		return delta * (time * time * time + 1f) + start;
	}

	public static float cubicInOut(float time, float start, float delta, float duration)
	{
		time /= duration / 2f;
		if (time < 1f)
		{
			return delta / 2f * time * time * time + start;
		}
		time -= 2f;
		return delta / 2f * (time * time * time + 2f) + start;
	}

	public static float cubicOutIn(float time, float start, float delta, float duration)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.cubicOut(time * 2f, start, delta / 2f, duration);
		}
		return EZAnimation.cubicIn(time * 2f - duration, start + delta / 2f, delta / 2f, duration);
	}

	public static float quarticIn(float time, float start, float delta, float duration)
	{
		time /= duration;
		return delta * time * time * time * time + start;
	}

	public static float quarticOut(float time, float start, float delta, float duration)
	{
		time /= duration;
		time -= 1f;
		return -delta * (time * time * time * time - 1f) + start;
	}

	public static float quarticInOut(float time, float start, float delta, float duration)
	{
		time /= duration / 2f;
		if (time < 1f)
		{
			return delta / 2f * time * time * time * time + start;
		}
		time -= 2f;
		return -delta / 2f * (time * time * time * time - 2f) + start;
	}

	public static float quarticOutIn(float time, float start, float delta, float duration)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.quarticOut(time * 2f, start, delta / 2f, duration);
		}
		return EZAnimation.quarticIn(time * 2f - duration, start + delta / 2f, delta / 2f, duration);
	}

	public static float quinticIn(float time, float start, float delta, float duration)
	{
		time /= duration;
		return delta * time * time * time * time * time + start;
	}

	public static float quinticOut(float time, float start, float delta, float duration)
	{
		time /= duration;
		time -= 1f;
		return delta * (time * time * time * time * time + 1f) + start;
	}

	public static float quinticInOut(float time, float start, float delta, float duration)
	{
		time /= duration / 2f;
		if (time < 1f)
		{
			return delta / 2f * time * time * time * time * time + start;
		}
		time -= 2f;
		return delta / 2f * (time * time * time * time * time + 2f) + start;
	}

	public static float quinticOutIn(float time, float start, float delta, float duration)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.quinticOut(time * 2f, start, delta / 2f, duration);
		}
		return EZAnimation.quinticIn(time * 2f - duration, start + delta / 2f, delta / 2f, duration);
	}

	public static float sinusIn(float time, float start, float delta, float duration)
	{
		return -delta * Mathf.Cos(time / duration * 1.57079637f) + delta + start;
	}

	public static float sinusOut(float time, float start, float delta, float duration)
	{
		return delta * Mathf.Sin(time / duration * 1.57079637f) + start;
	}

	public static float sinusInOut(float time, float start, float delta, float duration)
	{
		return -delta / 2f * (Mathf.Cos(3.14159274f * time / duration) - 1f) + start;
	}

	public static float sinusOutIn(float time, float start, float delta, float duration)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.sinusOut(time * 2f, start, delta / 2f, duration);
		}
		return EZAnimation.sinusIn(time * 2f - duration, start + delta / 2f, delta / 2f, duration);
	}

	public static float expIn(float time, float start, float delta, float duration)
	{
		return delta * Mathf.Pow(2f, 10f * (time / duration - 1f)) + start;
	}

	public static float expOut(float time, float start, float delta, float duration)
	{
		return delta * (-Mathf.Pow(2f, -10f * time / duration) + 1f) + start;
	}

	public static float expInOut(float time, float start, float delta, float duration)
	{
		time /= duration / 2f;
		if (time < 1f)
		{
			return delta / 2f * Mathf.Pow(2f, 10f * (time - 1f)) + start;
		}
		time -= 1f;
		return delta / 2f * (-Mathf.Pow(2f, -10f * time) + 2f) + start;
	}

	public static float expOutIn(float time, float start, float delta, float duration)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.expOut(time * 2f, start, delta / 2f, duration);
		}
		return EZAnimation.expIn(time * 2f - duration, start + delta / 2f, delta / 2f, duration);
	}

	public static float circIn(float time, float start, float delta, float duration)
	{
		time /= duration;
		return -delta * (Mathf.Sqrt(1f - time * time) - 1f) + start;
	}

	public static float circOut(float time, float start, float delta, float duration)
	{
		time /= duration;
		time -= 1f;
		return delta * Mathf.Sqrt(1f - time * time) + start;
	}

	public static float circInOut(float time, float start, float delta, float duration)
	{
		time /= duration / 2f;
		if (time < 1f)
		{
			return -delta / 2f * (Mathf.Sqrt(1f - time * time) - 1f) + start;
		}
		time -= 2f;
		return delta / 2f * (Mathf.Sqrt(1f - time * time) + 1f) + start;
	}

	public static float circOutIn(float time, float start, float delta, float duration)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.circOut(time * 2f, start, delta / 2f, duration);
		}
		return EZAnimation.circIn(time * 2f - duration, start + delta / 2f, delta / 2f, duration);
	}

	public static float punch(float amplitude, float value)
	{
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.28318548f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.28318548f / num);
	}

	public static float spring(float time, float start, float delta, float duration)
	{
		float num = time / duration;
		num = Mathf.Clamp01(num);
		num = (Mathf.Sin(num * 3.14159274f * (0.2f + 2.5f * num * num * num)) * Mathf.Pow(1f - num, 2.2f) + num) * (1f + 1.2f * (1f - num));
		return start + delta * num;
	}

	public static float elasticIn(float time, float start, float delta, float duration)
	{
		return EZAnimation.elasticIn(time, start, delta, duration, 0f, duration * 0.3f);
	}

	public static float elasticIn(float time, float start, float delta, float duration, float amplitude, float period)
	{
		if (time == 0f)
		{
			return start;
		}
		if (delta == 0f)
		{
			return start;
		}
		if ((time /= duration) == 1f)
		{
			return start + delta;
		}
		float num;
		if (amplitude < Mathf.Abs(delta))
		{
			amplitude = delta;
			num = period / 4f;
		}
		else
		{
			num = period / 6.28318548f * Mathf.Asin(delta / amplitude);
		}
		return -(amplitude * Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time * duration - num) * 6.28318548f / period)) + start;
	}

	public static float elasticOut(float time, float start, float delta, float duration)
	{
		return EZAnimation.elasticOut(time, start, delta, duration, 0f, duration * 0.3f);
	}

	public static float elasticOut(float time, float start, float delta, float duration, float amplitude, float period)
	{
		if (time == 0f)
		{
			return start;
		}
		if (delta == 0f)
		{
			return start;
		}
		if ((time /= duration) == 1f)
		{
			return start + delta;
		}
		float num;
		if (amplitude < Mathf.Abs(delta))
		{
			amplitude = delta;
			num = period / 4f;
		}
		else
		{
			num = period / 6.28318548f * Mathf.Asin(delta / amplitude);
		}
		return amplitude * Mathf.Pow(2f, -10f * time) * Mathf.Sin((time * duration - num) * 6.28318548f / period) + delta + start;
	}

	public static float elasticInOut(float time, float start, float delta, float duration)
	{
		return EZAnimation.elasticInOut(time, start, delta, duration, 0f, duration * 0.3f * 1.5f);
	}

	public static float elasticInOut(float time, float start, float delta, float duration, float amplitude, float period)
	{
		if (time == 0f)
		{
			return start;
		}
		if (delta == 0f)
		{
			return start;
		}
		if ((time /= duration / 2f) == 2f)
		{
			return start + delta;
		}
		float num;
		if (amplitude < Mathf.Abs(delta))
		{
			amplitude = delta;
			num = period / 4f;
		}
		else
		{
			num = period / 6.28318548f * Mathf.Asin(delta / amplitude);
		}
		if (time < 1f)
		{
			return -0.5f * (amplitude * Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time * duration - num) * 6.28318548f / period)) + start;
		}
		return amplitude * Mathf.Pow(2f, -10f * (time -= 1f)) * Mathf.Sin((time * duration - num) * 6.28318548f / period) * 0.5f + delta + start;
	}

	public static float elasticOutIn(float time, float start, float delta, float duration)
	{
		return EZAnimation.elasticOutIn(time, start, delta, duration, 0f, duration * 0.3f);
	}

	public static float elasticOutIn(float time, float start, float delta, float duration, float amplitude, float period)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.elasticOut(time * 2f, start, delta / 2f, duration, amplitude, period);
		}
		return EZAnimation.elasticIn(time * 2f - duration, start + delta / 2f, delta / 2f, duration, amplitude, period);
	}

	public static float backIn(float time, float start, float delta, float duration)
	{
		return EZAnimation.backIn(time, start, delta, duration, 1.70158f);
	}

	public static float backIn(float time, float start, float delta, float duration, float overshootAmt)
	{
		return delta * (time /= duration) * time * ((overshootAmt + 1f) * time - overshootAmt) + start;
	}

	public static float backOut(float time, float start, float delta, float duration)
	{
		return EZAnimation.backOut(time, start, delta, duration, 1.70158f);
	}

	public static float backOut(float time, float start, float delta, float duration, float overshootAmt)
	{
		return delta * ((time = time / duration - 1f) * time * ((overshootAmt + 1f) * time + overshootAmt) + 1f) + start;
	}

	public static float backInOut(float time, float start, float delta, float duration)
	{
		return EZAnimation.backInOut(time, start, delta, duration, 1.70158f);
	}

	public static float backInOut(float time, float start, float delta, float duration, float overshootAmt)
	{
		if ((time /= duration / 2f) < 1f)
		{
			return delta / 2f * (time * time * (((overshootAmt *= 1.525f) + 1f) * time - overshootAmt)) + start;
		}
		return delta / 2f * ((time -= 2f) * time * (((overshootAmt *= 1.525f) + 1f) * time + overshootAmt) + 2f) + start;
	}

	public static float backOutIn(float time, float start, float delta, float duration)
	{
		return EZAnimation.backOutIn(time, start, delta, duration, 1.70158f);
	}

	public static float backOutIn(float time, float start, float delta, float duration, float overshootAmt)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.backOut(time * 2f, start, delta / 2f, duration, overshootAmt);
		}
		return EZAnimation.backIn(time * 2f - duration, start + delta / 2f, delta / 2f, duration, overshootAmt);
	}

	public static float bounceIn(float time, float start, float delta, float duration)
	{
		return delta - EZAnimation.bounceOut(duration - time, 0f, delta, duration) + start;
	}

	public static float bounceOut(float time, float start, float delta, float duration)
	{
		if ((time /= duration) < 0.363636374f)
		{
			return delta * (7.5625f * time * time) + start;
		}
		if (time < 0.727272749f)
		{
			return delta * (7.5625f * (time -= 0.545454562f) * time + 0.75f) + start;
		}
		if (time < 0.909090936f)
		{
			return delta * (7.5625f * (time -= 0.8181818f) * time + 0.9375f) + start;
		}
		return delta * (7.5625f * (time -= 0.954545438f) * time + 0.984375f) + start;
	}

	public static float bounceInOut(float time, float start, float delta, float duration)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.bounceIn(time * 2f, 0f, delta, duration) * 0.5f + start;
		}
		return EZAnimation.bounceOut(time * 2f - duration, 0f, delta, duration) * 0.5f + delta * 0.5f + start;
	}

	public static float bounceOutIn(float time, float start, float delta, float duration)
	{
		if (time < duration / 2f)
		{
			return EZAnimation.bounceOut(time * 2f, start, delta / 2f, duration);
		}
		return EZAnimation.bounceIn(time * 2f - duration, start + delta / 2f, delta / 2f, duration);
	}

	public static EZAnimation.Interpolator GetInterpolator(EZAnimation.EASING_TYPE type)
	{
		switch (type)
		{
		case EZAnimation.EASING_TYPE.Linear:
			return new EZAnimation.Interpolator(EZAnimation.linear);
		case EZAnimation.EASING_TYPE.BackIn:
			return new EZAnimation.Interpolator(EZAnimation.backIn);
		case EZAnimation.EASING_TYPE.BackOut:
			return new EZAnimation.Interpolator(EZAnimation.backOut);
		case EZAnimation.EASING_TYPE.BackInOut:
			return new EZAnimation.Interpolator(EZAnimation.backInOut);
		case EZAnimation.EASING_TYPE.BackOutIn:
			return new EZAnimation.Interpolator(EZAnimation.backOutIn);
		case EZAnimation.EASING_TYPE.BounceIn:
			return new EZAnimation.Interpolator(EZAnimation.bounceIn);
		case EZAnimation.EASING_TYPE.BounceOut:
			return new EZAnimation.Interpolator(EZAnimation.bounceOut);
		case EZAnimation.EASING_TYPE.BounceInOut:
			return new EZAnimation.Interpolator(EZAnimation.bounceInOut);
		case EZAnimation.EASING_TYPE.BounceOutIn:
			return new EZAnimation.Interpolator(EZAnimation.bounceOutIn);
		case EZAnimation.EASING_TYPE.CircularIn:
			return new EZAnimation.Interpolator(EZAnimation.circIn);
		case EZAnimation.EASING_TYPE.CircularOut:
			return new EZAnimation.Interpolator(EZAnimation.circOut);
		case EZAnimation.EASING_TYPE.CircularInOut:
			return new EZAnimation.Interpolator(EZAnimation.circInOut);
		case EZAnimation.EASING_TYPE.CircularOutIn:
			return new EZAnimation.Interpolator(EZAnimation.circOutIn);
		case EZAnimation.EASING_TYPE.CubicIn:
			return new EZAnimation.Interpolator(EZAnimation.cubicIn);
		case EZAnimation.EASING_TYPE.CubicOut:
			return new EZAnimation.Interpolator(EZAnimation.cubicOut);
		case EZAnimation.EASING_TYPE.CubicInOut:
			return new EZAnimation.Interpolator(EZAnimation.cubicInOut);
		case EZAnimation.EASING_TYPE.CubicOutIn:
			return new EZAnimation.Interpolator(EZAnimation.cubicOutIn);
		case EZAnimation.EASING_TYPE.ElasticIn:
			return new EZAnimation.Interpolator(EZAnimation.elasticIn);
		case EZAnimation.EASING_TYPE.ElasticOut:
			return new EZAnimation.Interpolator(EZAnimation.elasticOut);
		case EZAnimation.EASING_TYPE.ElasticInOut:
			return new EZAnimation.Interpolator(EZAnimation.elasticInOut);
		case EZAnimation.EASING_TYPE.ElasticOutIn:
			return new EZAnimation.Interpolator(EZAnimation.elasticOutIn);
		case EZAnimation.EASING_TYPE.ExponentialIn:
			return new EZAnimation.Interpolator(EZAnimation.expIn);
		case EZAnimation.EASING_TYPE.ExponentialOut:
			return new EZAnimation.Interpolator(EZAnimation.expOut);
		case EZAnimation.EASING_TYPE.ExponentialInOut:
			return new EZAnimation.Interpolator(EZAnimation.expInOut);
		case EZAnimation.EASING_TYPE.ExponentialOutIn:
			return new EZAnimation.Interpolator(EZAnimation.expOutIn);
		case EZAnimation.EASING_TYPE.QuadraticIn:
			return new EZAnimation.Interpolator(EZAnimation.quadraticIn);
		case EZAnimation.EASING_TYPE.QuadraticOut:
			return new EZAnimation.Interpolator(EZAnimation.quadraticOut);
		case EZAnimation.EASING_TYPE.QuadraticInOut:
			return new EZAnimation.Interpolator(EZAnimation.quadraticInOut);
		case EZAnimation.EASING_TYPE.QuadraticOutIn:
			return new EZAnimation.Interpolator(EZAnimation.quadraticOutIn);
		case EZAnimation.EASING_TYPE.QuarticIn:
			return new EZAnimation.Interpolator(EZAnimation.quarticIn);
		case EZAnimation.EASING_TYPE.QuarticOut:
			return new EZAnimation.Interpolator(EZAnimation.quarticOut);
		case EZAnimation.EASING_TYPE.QuarticInOut:
			return new EZAnimation.Interpolator(EZAnimation.quarticInOut);
		case EZAnimation.EASING_TYPE.QuarticOutIn:
			return new EZAnimation.Interpolator(EZAnimation.quarticOutIn);
		case EZAnimation.EASING_TYPE.QuinticIn:
			return new EZAnimation.Interpolator(EZAnimation.quinticIn);
		case EZAnimation.EASING_TYPE.QuinticOut:
			return new EZAnimation.Interpolator(EZAnimation.quinticOut);
		case EZAnimation.EASING_TYPE.QuinticInOut:
			return new EZAnimation.Interpolator(EZAnimation.quinticInOut);
		case EZAnimation.EASING_TYPE.QuinticOutIn:
			return new EZAnimation.Interpolator(EZAnimation.quinticOutIn);
		case EZAnimation.EASING_TYPE.SinusoidalIn:
			return new EZAnimation.Interpolator(EZAnimation.sinusIn);
		case EZAnimation.EASING_TYPE.SinusoidalOut:
			return new EZAnimation.Interpolator(EZAnimation.sinusOut);
		case EZAnimation.EASING_TYPE.SinusoidalInOut:
			return new EZAnimation.Interpolator(EZAnimation.sinusInOut);
		case EZAnimation.EASING_TYPE.SinusoidalOutIn:
			return new EZAnimation.Interpolator(EZAnimation.sinusOutIn);
		case EZAnimation.EASING_TYPE.Spring:
			return new EZAnimation.Interpolator(EZAnimation.spring);
		default:
			return new EZAnimation.Interpolator(EZAnimation.linear);
		}
	}
}
