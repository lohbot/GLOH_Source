using System;
using UnityEngine;

public class SimpleSpriteMirror : SpriteRootMirror
{
	public Vector2 lowerLeftPixel;

	public Vector2 pixelDimensions;

	public override void Mirror(SpriteRoot s)
	{
		base.Mirror(s);
		this.lowerLeftPixel = ((SimpleSprite)s).lowerLeftPixel;
		this.pixelDimensions = ((SimpleSprite)s).pixelDimensions;
	}

	public override bool DidChange(SpriteRoot s)
	{
		return base.DidChange(s) || ((SimpleSprite)s).lowerLeftPixel != this.lowerLeftPixel || ((SimpleSprite)s).pixelDimensions != this.pixelDimensions;
	}
}
