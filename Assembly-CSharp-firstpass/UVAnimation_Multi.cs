using System;
using UnityEngine;

[Serializable]
public class UVAnimation_Multi
{
	public string name;

	public int loopCycles;

	public bool loopReverse;

	public float framerate = 15f;

	public UVAnimation.ANIM_END_ACTION onAnimEnd;

	public UVAnimation_Auto[] clips;

	[HideInInspector]
	public int index;

	protected int curClip;

	protected int stepDir = 1;

	protected int numLoops;

	protected float duration;

	protected bool ret;

	protected int i;

	protected int framePos = -1;

	public UVAnimation_Multi()
	{
		if (this.clips == null)
		{
			this.clips = new UVAnimation_Auto[0];
		}
	}

	public UVAnimation_Multi(UVAnimation_Multi anim)
	{
		this.name = anim.name;
		this.loopCycles = anim.loopCycles;
		this.loopReverse = anim.loopReverse;
		this.framerate = anim.framerate;
		this.onAnimEnd = anim.onAnimEnd;
		this.curClip = anim.curClip;
		this.stepDir = anim.stepDir;
		this.numLoops = anim.numLoops;
		this.duration = anim.duration;
		this.clips = new UVAnimation_Auto[anim.clips.Length];
		for (int i = 0; i < this.clips.Length; i++)
		{
			this.clips[i] = anim.clips[i].Clone();
		}
		this.CalcDuration();
	}

	public UVAnimation_Multi Clone()
	{
		return new UVAnimation_Multi(this);
	}

	public UVAnimation_Auto GetCurrentClip()
	{
		return this.clips[this.curClip];
	}

	public UVAnimation_Auto[] BuildUVAnim(SpriteRoot s)
	{
		this.i = 0;
		while (this.i < this.clips.Length)
		{
			this.clips[this.i].BuildUVAnim(s);
			this.i++;
		}
		this.CalcDuration();
		return this.clips;
	}

	public bool GetNextFrame(ref SPRITE_FRAME nextFrame)
	{
		if (this.clips.Length < 1)
		{
			return false;
		}
		this.ret = this.clips[this.curClip].GetNextFrame(ref nextFrame);
		if (!this.ret)
		{
			if (this.curClip + this.stepDir >= this.clips.Length || this.curClip + this.stepDir < 0)
			{
				if (this.stepDir > 0 && this.loopReverse)
				{
					this.stepDir = -1;
					this.curClip += this.stepDir;
					this.curClip = Mathf.Clamp(this.curClip, 0, this.clips.Length - 1);
					this.clips[this.curClip].Reset();
					this.clips[this.curClip].PlayInReverse();
					this.clips[this.curClip].GetNextFrame(ref nextFrame);
				}
				else
				{
					if (this.numLoops + 1 > this.loopCycles && this.loopCycles != -1)
					{
						return false;
					}
					this.numLoops++;
					if (this.loopReverse)
					{
						this.stepDir *= -1;
						this.curClip += this.stepDir;
						this.curClip = Mathf.Clamp(this.curClip, 0, this.clips.Length - 1);
						this.clips[this.curClip].Reset();
						if (this.stepDir < 0)
						{
							this.clips[this.curClip].PlayInReverse();
						}
						this.clips[this.curClip].GetNextFrame(ref nextFrame);
					}
					else
					{
						this.curClip = 0;
						this.framePos = -1;
						this.clips[this.curClip].Reset();
					}
				}
			}
			else
			{
				this.curClip += this.stepDir;
				this.clips[this.curClip].Reset();
				if (this.stepDir < 0)
				{
					this.clips[this.curClip].PlayInReverse();
					this.clips[this.curClip].GetNextFrame(ref nextFrame);
				}
			}
			this.framePos += this.stepDir;
			this.clips[this.curClip].GetNextFrame(ref nextFrame);
			return true;
		}
		this.framePos += this.stepDir;
		return true;
	}

	public SPRITE_FRAME GetCurrentFrame()
	{
		return this.clips[Mathf.Clamp(this.curClip, 0, this.curClip)].GetCurrentFrame();
	}

	public void AppendAnim(int index, SPRITE_FRAME[] anim)
	{
		if (index >= this.clips.Length)
		{
			return;
		}
		this.clips[index].AppendAnim(anim);
		this.CalcDuration();
	}

	public void AppendClip(UVAnimation clip)
	{
		UVAnimation[] array = this.clips;
		this.clips = new UVAnimation_Auto[this.clips.Length + 1];
		array.CopyTo(this.clips, 0);
		this.clips[this.clips.Length - 1] = (UVAnimation_Auto)clip;
		this.CalcDuration();
	}

	public void PlayInReverse()
	{
		this.i = 0;
		while (this.i < this.clips.Length)
		{
			this.clips[this.i].PlayInReverse();
			this.i++;
		}
		this.stepDir = -1;
		this.framePos = this.GetFrameCount() - 1;
		this.curClip = this.clips.Length - 1;
	}

	public void SetAnim(int index, SPRITE_FRAME[] frames)
	{
		if (index >= this.clips.Length)
		{
			return;
		}
		this.clips[index].SetAnim(frames);
		this.CalcDuration();
	}

	public void Reset()
	{
		this.curClip = 0;
		this.stepDir = 1;
		this.numLoops = 0;
		this.framePos = -1;
		this.i = 0;
		while (this.i < this.clips.Length)
		{
			this.clips[this.i].Reset();
			this.i++;
		}
	}

	public void SetPosition(float pos)
	{
		pos = Mathf.Clamp01(pos);
		if (this.loopCycles < 1)
		{
			this.SetAnimPosition(pos);
			return;
		}
		float num = 1f / ((float)this.loopCycles + 1f);
		this.numLoops = Mathf.FloorToInt(pos / num);
		float num2 = pos - (float)this.numLoops * num;
		this.SetAnimPosition(num2 / num);
	}

	public void SetAnimPosition(float pos)
	{
		int num = 0;
		float num2 = pos;
		for (int i = 0; i < this.clips.Length; i++)
		{
			num += this.clips[i].GetFramesDisplayed();
		}
		if (this.loopReverse)
		{
			if (pos < 0.5f)
			{
				this.stepDir = 1;
				num2 *= 2f;
				for (int j = 0; j < this.clips.Length; j++)
				{
					float num3 = (float)(this.clips[j].GetFramesDisplayed() / num);
					if (num2 <= num3)
					{
						this.curClip = j;
						this.clips[this.curClip].SetPosition(num2 / num3);
						this.framePos = (int)num3 * (num - 1);
						return;
					}
					num2 -= num3;
				}
			}
			else
			{
				this.stepDir = -1;
				num2 = (num2 - 0.5f) / 0.5f;
				for (int k = this.clips.Length - 1; k >= 0; k--)
				{
					float num3 = (float)(this.clips[k].GetFramesDisplayed() / num);
					if (num2 <= num3)
					{
						this.curClip = k;
						this.clips[this.curClip].SetPosition(1f - num2 / num3);
						this.clips[this.curClip].SetStepDir(-1);
						this.framePos = (int)num3 * (num - 1);
						return;
					}
					num2 -= num3;
				}
			}
		}
		else
		{
			for (int l = 0; l < this.clips.Length; l++)
			{
				float num3 = (float)(this.clips[l].GetFramesDisplayed() / num);
				if (num2 <= num3)
				{
					this.curClip = l;
					this.clips[this.curClip].SetPosition(num2 / num3);
					this.framePos = (int)num3 * (num - 1);
					return;
				}
				num2 -= num3;
			}
		}
	}

	protected void CalcDuration()
	{
		if (this.loopCycles < 0)
		{
			this.duration = -1f;
			return;
		}
		this.duration = 0f;
		for (int i = 0; i < this.clips.Length; i++)
		{
			this.duration += this.clips[i].GetDuration();
		}
		if (this.loopReverse)
		{
			this.duration *= 2f;
		}
		this.duration += (float)this.loopCycles * this.duration;
	}

	public float GetDuration()
	{
		return this.duration;
	}

	public int GetFrameCount()
	{
		int num = 0;
		for (int i = 0; i < this.clips.Length; i++)
		{
			num += this.clips[i].GetFramesDisplayed();
		}
		return num;
	}

	public int GetCurPosition()
	{
		return this.framePos;
	}
}
