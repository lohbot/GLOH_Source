using System;
using UnityEngine;

[Serializable]
public class UVAnimation_Auto : UVAnimation
{
	public Vector2 start;

	public Vector2 pixelsToNextColumnAndRow;

	public int cols;

	public int rows;

	public int totalCells;

	public UVAnimation_Auto()
	{
	}

	public UVAnimation_Auto(UVAnimation_Auto anim) : base(anim)
	{
		this.start = anim.start;
		this.pixelsToNextColumnAndRow = anim.pixelsToNextColumnAndRow;
		this.cols = anim.cols;
		this.rows = anim.rows;
		this.totalCells = anim.totalCells;
	}

	public new UVAnimation_Auto Clone()
	{
		return new UVAnimation_Auto(this);
	}

	public SPRITE_FRAME[] BuildUVAnim(SpriteRoot s)
	{
		if (this.totalCells < 1)
		{
			return null;
		}
		return base.BuildUVAnim(s.PixelCoordToUVCoord(this.start), s.PixelSpaceToUVSpace(this.pixelsToNextColumnAndRow), this.cols, this.rows, this.totalCells);
	}
}
