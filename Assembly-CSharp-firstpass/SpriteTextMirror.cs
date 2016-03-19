using System;
using UnityEngine;

public class SpriteTextMirror
{
	public string text;

	public TextAsset font;

	public float offsetZ;

	public float characterSize;

	public float characterSpacing;

	public float lineSpacing;

	public SpriteText.Anchor_Pos anchor;

	public SpriteText.Alignment_Type alignment;

	public int tabSize;

	public Color color;

	public float maxWidth;

	public bool pixelPerfect;

	public Camera renderCamera;

	public bool hideAtStart;

	public virtual void Mirror(SpriteText s)
	{
		this.text = s.text;
		this.font = s.font;
		this.offsetZ = s.offsetZ;
		this.characterSize = s.characterSize;
		this.characterSpacing = s.characterSpacing;
		this.lineSpacing = s.lineSpacing;
		this.anchor = s.anchor;
		this.alignment = s.alignment;
		this.tabSize = s.tabSize;
		this.color = s.color;
		this.maxWidth = s.maxWidth;
		this.pixelPerfect = s.pixelPerfect;
		this.renderCamera = s.renderCamera;
		this.hideAtStart = s.hideAtStart;
	}

	public virtual bool Validate(SpriteText s)
	{
		return true;
	}

	public virtual bool DidChange(SpriteText s)
	{
		if (s.text != this.text)
		{
			return true;
		}
		if (s.font != this.font)
		{
			return true;
		}
		if (s.offsetZ != this.offsetZ)
		{
			return true;
		}
		if (s.characterSize != this.characterSize)
		{
			return true;
		}
		if (s.characterSpacing != this.characterSpacing)
		{
			return true;
		}
		if (s.lineSpacing != this.lineSpacing)
		{
			return true;
		}
		if (s.anchor != this.anchor)
		{
			return true;
		}
		if (s.alignment != this.alignment)
		{
			return true;
		}
		if (s.tabSize != this.tabSize)
		{
			return true;
		}
		if (s.color.r != this.color.r || s.color.g != this.color.g || s.color.b != this.color.b || s.color.a != this.color.a)
		{
			return true;
		}
		if (this.maxWidth != s.maxWidth)
		{
			return true;
		}
		if (s.pixelPerfect != this.pixelPerfect)
		{
			s.SetCamera(s.renderCamera);
			return true;
		}
		if (s.renderCamera != this.renderCamera)
		{
			s.SetCamera(s.renderCamera);
			return true;
		}
		if (s.hideAtStart != this.hideAtStart)
		{
			s.Hide(s.hideAtStart);
			return true;
		}
		return false;
	}
}
