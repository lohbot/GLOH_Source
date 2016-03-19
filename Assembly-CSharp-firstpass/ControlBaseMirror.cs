using System;

public class ControlBaseMirror
{
	private string text;

	private float textOffsetZ;

	public virtual void Mirror(ControlBase c)
	{
		this.text = c.text;
		this.textOffsetZ = c.textOffsetZ;
	}

	public virtual bool DidChange(ControlBase c)
	{
		if (this.text != c.text)
		{
			c.Text = c.text;
			return true;
		}
		if (this.textOffsetZ != c.textOffsetZ)
		{
			if (c.spriteText != null)
			{
				c.spriteText.offsetZ = this.textOffsetZ;
			}
			return true;
		}
		return false;
	}

	public virtual void Validate(ControlBase c)
	{
	}
}
