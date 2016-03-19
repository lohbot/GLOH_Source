using System;
using UnityEngine;

public class UITextFieldMirror : AutoSpriteControlBaseMirror
{
	public Vector2 margins;

	public bool multiline;

	public override void Mirror(SpriteRoot s)
	{
		base.Mirror(s);
		UITextField uITextField = (UITextField)s;
		this.margins = uITextField.margins;
		this.multiline = uITextField.multiline;
	}

	public override bool Validate(SpriteRoot s)
	{
		return base.Validate(s);
	}

	public override bool DidChange(SpriteRoot s)
	{
		UITextField uITextField = (UITextField)s;
		if (this.margins.x != uITextField.margins.x || this.margins.y != uITextField.margins.y || this.width != uITextField.width || this.height != uITextField.height)
		{
			uITextField.SetMargins(uITextField.margins);
			uITextField.CalcClippingRect();
			this.margins = uITextField.margins;
		}
		if (this.multiline != uITextField.multiline)
		{
			if (uITextField.spriteText != null)
			{
				uITextField.spriteText.multiline = uITextField.multiline;
			}
			return true;
		}
		return base.DidChange(s);
	}
}
