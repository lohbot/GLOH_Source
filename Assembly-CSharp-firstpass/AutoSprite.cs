using System;

public class AutoSprite : AutoSpriteBase
{
	public TextureAnim[] textureAnimations;

	public override TextureAnim[] States
	{
		get
		{
			return this.textureAnimations;
		}
		set
		{
			this.textureAnimations = value;
		}
	}

	protected override void Awake()
	{
		if (this.textureAnimations == null)
		{
			this.textureAnimations = new TextureAnim[0];
		}
		base.Awake();
		this.Init();
	}

	protected override void Init()
	{
		base.Init();
	}
}
