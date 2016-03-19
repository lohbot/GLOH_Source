using System;
using UnityEngine;

[Serializable]
public class CSpriteFrame
{
	public Rect uvs;

	public Vector2 scaleFactor = new Vector2(0.5f, 0.5f);

	public Vector2 topLeftOffset = new Vector2(-1f, 1f);

	public Vector2 bottomRightOffset = new Vector2(1f, -1f);

	public CSpriteFrame()
	{
	}

	public CSpriteFrame(CSpriteFrame f)
	{
		this.Copy(f);
	}

	public CSpriteFrame(SPRITE_FRAME f)
	{
		this.Copy(f);
	}

	public void Copy(SPRITE_FRAME f)
	{
		this.uvs = f.uvs;
		this.scaleFactor = f.scaleFactor;
		this.topLeftOffset = f.topLeftOffset;
		this.bottomRightOffset = f.bottomRightOffset;
	}

	public void Copy(CSpriteFrame f)
	{
		this.uvs = f.uvs;
		this.scaleFactor = f.scaleFactor;
		this.topLeftOffset = f.topLeftOffset;
		this.bottomRightOffset = f.bottomRightOffset;
	}

	public SPRITE_FRAME ToStruct()
	{
		SPRITE_FRAME result;
		result.uvs = this.uvs;
		result.scaleFactor = this.scaleFactor;
		result.topLeftOffset = this.topLeftOffset;
		result.bottomRightOffset = this.bottomRightOffset;
		return result;
	}
}
