using System;
using UnityEngine;

[Serializable]
public class TextureAnim
{
	public string name;

	public int loopCycles;

	public bool loopReverse;

	public float framerate = 15f;

	public UVAnimation.ANIM_END_ACTION onAnimEnd;

	[HideInInspector]
	public string[] framePaths;

	[HideInInspector]
	public string[] frameGUIDs;

	[HideInInspector]
	public CSpriteFrame[] spriteFrames;

	public TextureAnim()
	{
		this.Allocate();
	}

	public TextureAnim(string n)
	{
		this.name = n;
		this.Allocate();
	}

	public void Allocate()
	{
		bool flag = false;
		if (this.framePaths == null)
		{
			this.framePaths = new string[0];
		}
		if (this.frameGUIDs == null)
		{
			this.frameGUIDs = new string[0];
		}
		if (this.spriteFrames == null)
		{
			flag = true;
		}
		else if (this.spriteFrames.Length != this.frameGUIDs.Length)
		{
			flag = true;
		}
		if (flag)
		{
			this.spriteFrames = new CSpriteFrame[Mathf.Max(this.frameGUIDs.Length, this.framePaths.Length)];
			for (int i = 0; i < this.spriteFrames.Length; i++)
			{
				this.spriteFrames[i] = new CSpriteFrame();
			}
		}
	}

	public void Copy(TextureAnim a)
	{
		this.name = a.name;
		this.loopCycles = a.loopCycles;
		this.loopReverse = a.loopReverse;
		this.framerate = a.framerate;
		this.onAnimEnd = a.onAnimEnd;
		this.framePaths = new string[a.framePaths.Length];
		a.framePaths.CopyTo(this.framePaths, 0);
		this.frameGUIDs = new string[a.frameGUIDs.Length];
		a.frameGUIDs.CopyTo(this.frameGUIDs, 0);
		this.spriteFrames = new CSpriteFrame[a.spriteFrames.Length];
		for (int i = 0; i < this.spriteFrames.Length; i++)
		{
			this.spriteFrames[i] = new CSpriteFrame(a.spriteFrames[i]);
		}
	}
}
