using System;
using UnityEngine;

namespace UnityForms
{
	public class Label : UIButton
	{
		public override bool Visible
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
			set
			{
				base.gameObject.SetActive(value);
				if (value)
				{
					base.gameObject.renderer.enabled = false;
				}
				if (null != this.spriteText)
				{
					this.spriteText.renderer.enabled = value;
				}
				if (null != this.spriteTextShadow)
				{
					this.spriteTextShadow.renderer.enabled = value;
				}
			}
		}

		public new static Label Create(string name, Vector3 pos)
		{
			return (Label)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(Label));
		}

		public void SetFontSize(int size)
		{
			if (this.spriteText != null)
			{
				this.spriteText.SetCharacterSize((float)size);
			}
		}

		public void SetAnchorText(SpriteText.Anchor_Pos anchor)
		{
			this.spriteText.SetAnchor(anchor);
			this.spriteTextShadow.SetAnchor(anchor);
			switch (anchor)
			{
			case SpriteText.Anchor_Pos.Upper_Left:
			case SpriteText.Anchor_Pos.Middle_Left:
			case SpriteText.Anchor_Pos.Lower_Left:
				this.spriteText.SetAlignment(SpriteText.Alignment_Type.Left);
				this.spriteTextShadow.SetAlignment(SpriteText.Alignment_Type.Left);
				break;
			case SpriteText.Anchor_Pos.Upper_Center:
			case SpriteText.Anchor_Pos.Middle_Center:
			case SpriteText.Anchor_Pos.Lower_Center:
				this.spriteText.SetAlignment(SpriteText.Alignment_Type.Center);
				this.spriteTextShadow.SetAlignment(SpriteText.Alignment_Type.Center);
				break;
			case SpriteText.Anchor_Pos.Upper_Right:
			case SpriteText.Anchor_Pos.Middle_Right:
			case SpriteText.Anchor_Pos.Lower_Right:
				this.spriteText.SetAlignment(SpriteText.Alignment_Type.Right);
				this.spriteTextShadow.SetAlignment(SpriteText.Alignment_Type.Right);
				break;
			}
		}

		public float GetWidth()
		{
			return this.spriteText.GetTextWidth();
		}
	}
}
