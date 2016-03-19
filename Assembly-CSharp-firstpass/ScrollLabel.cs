using System;
using UnityEngine;

public class ScrollLabel : UIScrollList
{
	private SpriteText.Font_Effect fontEffect = SpriteText.Font_Effect.Black_Shadow_Small;

	private SpriteText.Anchor_Pos anchorPos;

	private SpriteText.Alignment_Type alignmentType;

	private string colorText = string.Empty;

	private int fontSize;

	private float fLineSpacing = 1.5f;

	public float LineSpasing
	{
		get
		{
			return this.fLineSpacing;
		}
		set
		{
			this.fLineSpacing = value;
		}
	}

	public string ColorText
	{
		set
		{
			this.colorText = value;
		}
	}

	public int FontSize
	{
		get
		{
			return this.fontSize;
		}
		set
		{
			this.fontSize = value;
		}
	}

	public SpriteText.Font_Effect FontEffect
	{
		set
		{
			this.fontEffect = value;
		}
	}

	public SpriteText.Anchor_Pos AnchorPos
	{
		get
		{
			return this.anchorPos;
		}
		set
		{
			this.anchorPos = value;
		}
	}

	public SpriteText.Alignment_Type AlignmentType
	{
		get
		{
			return this.alignmentType;
		}
		set
		{
			this.alignmentType = value;
		}
	}

	public new static ScrollLabel Create(string name, Vector3 pos)
	{
		return (ScrollLabel)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(ScrollLabel));
	}

	public void CreateBoxCollider()
	{
		this.bLabelScroll = true;
		BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
		if (boxCollider != null)
		{
			boxCollider.size = new Vector3(this.viewableArea.x, this.viewableArea.y, 0f);
			boxCollider.center = new Vector3(0f, 0f, -0.1f);
		}
	}

	public void SetScrollLabel(string text)
	{
		this.clipWhenMoving = true;
		base.ClearList(true);
		char[] separator = new char[]
		{
			'\n'
		};
		string[] array = text.Split(separator);
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = new GameObject("ScrollLabel" + i);
			FlashLabel flashLabel = gameObject.AddComponent<FlashLabel>();
			flashLabel.FontSize = (float)this.fontSize;
			flashLabel.FontEffect = this.fontEffect;
			flashLabel.FontColor = this.colorText;
			flashLabel.anchor = this.anchorPos;
			flashLabel.width = this.viewableArea.x - 20f;
			flashLabel.SetFlashLabel(array[i]);
			base.InsertItemDonotPosionUpdate(flashLabel, i, null, true);
		}
		base.RepositionItems();
		this.clipWhenMoving = true;
	}
}
