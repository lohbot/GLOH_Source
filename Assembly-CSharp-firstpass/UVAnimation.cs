using System;
using UnityEngine;

[Serializable]
public class UVAnimation
{
	public enum ANIM_END_ACTION
	{
		Do_Nothing,
		Revert_To_Static,
		Play_Default_Anim,
		Hide,
		Deactivate,
		Destroy
	}

	protected SPRITE_FRAME[] frames;

	protected int curFrame = -1;

	protected int stepDir = 1;

	protected int numLoops;

	protected bool playInReverse;

	protected float length;

	public string name;

	public int loopCycles;

	public bool loopReverse;

	[HideInInspector]
	public float framerate = 15f;

	[HideInInspector]
	public int index = -1;

	[HideInInspector]
	public UVAnimation.ANIM_END_ACTION onAnimEnd;

	public int StepDirection
	{
		get
		{
			return this.stepDir;
		}
	}

	public UVAnimation(UVAnimation anim)
	{
		this.frames = new SPRITE_FRAME[anim.frames.Length];
		anim.frames.CopyTo(this.frames, 0);
		this.name = anim.name;
		this.loopCycles = anim.loopCycles;
		this.loopReverse = anim.loopReverse;
		this.framerate = anim.framerate;
		this.onAnimEnd = anim.onAnimEnd;
		this.curFrame = anim.curFrame;
		this.stepDir = anim.stepDir;
		this.numLoops = anim.numLoops;
		this.playInReverse = anim.playInReverse;
		this.length = anim.length;
		this.CalcLength();
	}

	public UVAnimation()
	{
		this.frames = new SPRITE_FRAME[0];
	}

	public UVAnimation Clone()
	{
		return new UVAnimation(this);
	}

	public void Reset()
	{
		this.curFrame = -1;
		this.stepDir = 1;
		this.numLoops = 0;
		this.playInReverse = false;
	}

	public void PlayInReverse()
	{
		this.stepDir = -1;
		this.curFrame = this.frames.Length;
		this.numLoops = 0;
		this.playInReverse = true;
	}

	public void SetStepDir(int dir)
	{
		if (dir < 0)
		{
			this.stepDir = -1;
			this.playInReverse = true;
		}
		else
		{
			this.stepDir = 1;
		}
	}

	public bool GetNextFrame(ref SPRITE_FRAME nextFrame)
	{
		if (this.frames.Length < 1)
		{
			return false;
		}
		if (this.curFrame + this.stepDir >= this.frames.Length || this.curFrame + this.stepDir < 0)
		{
			if (this.stepDir > 0 && this.loopReverse)
			{
				this.stepDir = -1;
				this.curFrame += this.stepDir;
				this.curFrame = Mathf.Clamp(this.curFrame, 0, this.frames.Length - 1);
				nextFrame = this.frames[this.curFrame];
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
					this.curFrame += this.stepDir;
					this.curFrame = Mathf.Clamp(this.curFrame, 0, this.frames.Length - 1);
				}
				else if (this.playInReverse)
				{
					this.curFrame = this.frames.Length - 1;
				}
				else
				{
					this.curFrame = 0;
				}
				nextFrame = this.frames[this.curFrame];
			}
		}
		else
		{
			this.curFrame += this.stepDir;
			nextFrame = this.frames[this.curFrame];
		}
		return true;
	}

	public SPRITE_FRAME GetCurrentFrame()
	{
		return this.frames[Mathf.Clamp(this.curFrame, 0, this.curFrame)];
	}

	public SPRITE_FRAME GetFrame(int frame)
	{
		return this.frames[frame];
	}

	public SPRITE_FRAME[] BuildUVAnim(Vector2 start, Vector2 cellSize, int cols, int rows, int totalCells)
	{
		int num = 0;
		this.frames = new SPRITE_FRAME[totalCells];
		this.frames[0] = new SPRITE_FRAME(0f);
		this.frames[0].uvs.x = start.x;
		this.frames[0].uvs.y = start.y;
		this.frames[0].uvs.xMax = start.x + cellSize.x;
		this.frames[0].uvs.yMax = start.y + cellSize.y;
		for (int i = 0; i < rows; i++)
		{
			int num2 = 0;
			while (num2 < cols && num < totalCells)
			{
				this.frames[num] = new SPRITE_FRAME(0f);
				this.frames[num].uvs.x = start.x + cellSize.x * (float)num2;
				this.frames[num].uvs.y = start.y - cellSize.y * (float)i;
				this.frames[num].uvs.xMax = this.frames[num].uvs.x + cellSize.x;
				this.frames[num].uvs.yMax = this.frames[num].uvs.y + cellSize.y;
				num++;
				num2++;
			}
		}
		this.CalcLength();
		return this.frames;
	}

	public SPRITE_FRAME[] BuildWrappedUVAnim(Vector2 start, Vector2 cellSize, int cols, int rows, int totalCells)
	{
		return this.BuildWrappedUVAnim(start, cellSize, totalCells);
	}

	public SPRITE_FRAME[] BuildWrappedUVAnim(Vector2 start, Vector2 cellSize, int totalCells)
	{
		this.frames = new SPRITE_FRAME[totalCells];
		this.frames[0] = new SPRITE_FRAME(0f);
		this.frames[0].uvs.x = start.x;
		this.frames[0].uvs.y = start.y;
		this.frames[0].uvs.xMax = start.x + cellSize.x;
		this.frames[0].uvs.yMax = start.y + cellSize.y;
		Vector2 vector = start;
		for (int i = 1; i < totalCells; i++)
		{
			vector.x += cellSize.x;
			if (vector.x + cellSize.x > 1.01f)
			{
				vector.x = 0f;
				vector.y -= cellSize.y;
			}
			this.frames[i] = new SPRITE_FRAME(0f);
			this.frames[i].uvs.x = vector.x;
			this.frames[i].uvs.y = vector.y;
			this.frames[i].uvs.xMax = vector.x + cellSize.x;
			this.frames[i].uvs.yMax = vector.y + cellSize.y;
		}
		return this.frames;
	}

	public void SetAnim(SPRITE_FRAME[] anim)
	{
		this.frames = anim;
		this.CalcLength();
	}

	public void SetAnim(TextureAnim anim, int idx)
	{
		if (anim == null)
		{
			return;
		}
		if (anim.spriteFrames == null)
		{
			return;
		}
		this.frames = new SPRITE_FRAME[anim.spriteFrames.Length];
		this.index = idx;
		this.name = anim.name;
		this.loopCycles = anim.loopCycles;
		this.loopReverse = anim.loopReverse;
		this.framerate = anim.framerate;
		this.onAnimEnd = anim.onAnimEnd;
		try
		{
			for (int i = 0; i < this.frames.Length; i++)
			{
				this.frames[i] = anim.spriteFrames[i].ToStruct();
			}
		}
		catch (Exception ex)
		{
			TsLog.LogError("Exception caught in UVAnimation.SetAnim(). Make sure you have re-built your atlases!\nException: " + ex.Message, new object[0]);
		}
		this.CalcLength();
	}

	public void AppendAnim(SPRITE_FRAME[] anim)
	{
		SPRITE_FRAME[] array = this.frames;
		this.frames = new SPRITE_FRAME[this.frames.Length + anim.Length];
		array.CopyTo(this.frames, 0);
		anim.CopyTo(this.frames, array.Length);
		this.CalcLength();
	}

	public void SetCurrentFrame(int f)
	{
		f = Mathf.Clamp(f, -1, this.frames.Length + 1);
		this.curFrame = f;
	}

	public void SetPosition(float pos)
	{
		pos = Mathf.Clamp01(pos);
		if (this.loopCycles < 1)
		{
			this.SetClipPosition(pos);
			return;
		}
		float num = 1f / ((float)this.loopCycles + 1f);
		this.numLoops = Mathf.FloorToInt(pos / num);
		float num2 = pos - (float)this.numLoops * num;
		float num3 = num2 / num;
		if (this.loopReverse)
		{
			if (num3 < 0.5f)
			{
				this.curFrame = (int)(((float)this.frames.Length - 1f) * (num3 / 0.5f));
				this.stepDir = 1;
			}
			else
			{
				this.curFrame = this.frames.Length - 1 - (int)(((float)this.frames.Length - 1f) * ((num3 - 0.5f) / 0.5f));
				this.stepDir = -1;
			}
		}
		else
		{
			this.curFrame = (int)(((float)this.frames.Length - 1f) * num3);
		}
	}

	public void SetClipPosition(float pos)
	{
		this.curFrame = (int)(((float)this.frames.Length - 1f) * pos);
	}

	protected void CalcLength()
	{
		this.length = 1f / this.framerate * (float)this.frames.Length;
	}

	public float GetLength()
	{
		return this.length;
	}

	public float GetDuration()
	{
		if (this.loopCycles < 0)
		{
			return -1f;
		}
		float num = this.GetLength();
		if (this.loopReverse)
		{
			num *= 2f;
		}
		return num + (float)this.loopCycles * num;
	}

	public int GetFrameCount()
	{
		return this.frames.Length;
	}

	public int GetFramesDisplayed()
	{
		if (this.loopCycles == -1)
		{
			return -1;
		}
		int num = this.frames.Length + this.frames.Length * this.loopCycles;
		if (this.loopReverse)
		{
			num *= 2;
		}
		return num;
	}

	public int GetCurPosition()
	{
		return this.curFrame;
	}
}
