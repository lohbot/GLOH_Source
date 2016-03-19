using System;

public class AutoSpriteControlBaseMirror : SpriteRootMirror
{
	private string text;

	private float textOffsetZ;

	public override void Mirror(SpriteRoot s)
	{
		AutoSpriteControlBase autoSpriteControlBase = (AutoSpriteControlBase)s;
		base.Mirror(s);
		this.text = autoSpriteControlBase.Text;
		this.textOffsetZ = autoSpriteControlBase.textOffsetZ;
	}

	public override bool DidChange(SpriteRoot s)
	{
		AutoSpriteControlBase autoSpriteControlBase = (AutoSpriteControlBase)s;
		if (this.text != autoSpriteControlBase.Text)
		{
			autoSpriteControlBase.Text = autoSpriteControlBase.Text;
			return true;
		}
		if (this.textOffsetZ != autoSpriteControlBase.textOffsetZ)
		{
			if (autoSpriteControlBase.spriteText != null)
			{
				autoSpriteControlBase.spriteText.offsetZ = this.textOffsetZ;
			}
			return true;
		}
		return base.DidChange(s);
	}
}
