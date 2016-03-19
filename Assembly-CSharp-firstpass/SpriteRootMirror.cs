using System;
using UnityEngine;

public class SpriteRootMirror
{
	public bool managed;

	public SpriteManager manager;

	public int drawLayer;

	public SpriteRoot.SPRITE_PLANE plane;

	public SpriteRoot.WINDING_ORDER winding;

	public float width;

	public float height;

	public Vector2 bleedCompensation;

	public SpriteRoot.ANCHOR_METHOD anchor;

	public Vector3 offset;

	public Color color;

	public bool pixelPerfect;

	public bool autoResize;

	public Camera renderCamera;

	public bool hideAtStart;

	public virtual void Mirror(SpriteRoot s)
	{
		this.managed = s.managed;
		this.manager = s.manager;
		this.drawLayer = s.drawLayer;
		this.plane = s.plane;
		this.winding = s.winding;
		this.width = s.width;
		this.height = s.height;
		this.bleedCompensation = s.bleedCompensation;
		this.anchor = s.anchor;
		this.offset = s.offset;
		this.color = s.color;
		this.pixelPerfect = s.pixelPerfect;
		this.autoResize = s.autoResize;
		this.renderCamera = s.renderCamera;
		this.hideAtStart = s.hideAtStart;
	}

	public virtual bool Validate(SpriteRoot s)
	{
		if (s.pixelPerfect)
		{
			s.autoResize = true;
		}
		return true;
	}

	public virtual bool DidChange(SpriteRoot s)
	{
		if (s.managed != this.managed)
		{
			this.HandleManageState(s);
			return true;
		}
		if (s.manager != this.manager)
		{
			this.UpdateManager(s);
			return true;
		}
		if (s.drawLayer != this.drawLayer)
		{
			this.HandleDrawLayerChange(s);
			return true;
		}
		if (s.plane != this.plane)
		{
			return true;
		}
		if (s.winding != this.winding)
		{
			return true;
		}
		if (s.width != this.width)
		{
			return true;
		}
		if (s.height != this.height)
		{
			return true;
		}
		if (s.bleedCompensation != this.bleedCompensation)
		{
			return true;
		}
		if (s.anchor != this.anchor)
		{
			return true;
		}
		if (s.offset != this.offset)
		{
			return true;
		}
		if (s.color.r != this.color.r || s.color.g != this.color.g || s.color.b != this.color.b || s.color.a != this.color.a)
		{
			return true;
		}
		if (s.pixelPerfect != this.pixelPerfect)
		{
			return true;
		}
		if (s.autoResize != this.autoResize)
		{
			return true;
		}
		if (s.renderCamera != this.renderCamera)
		{
			return true;
		}
		if (s.hideAtStart != this.hideAtStart)
		{
			s.Hide(s.hideAtStart);
			return true;
		}
		return false;
	}

	protected virtual void HandleManageState(SpriteRoot s)
	{
		s.managed = this.managed;
		s.Managed = !this.managed;
	}

	public virtual void UpdateManager(SpriteRoot s)
	{
		if (!s.managed)
		{
			s.manager = null;
		}
		else
		{
			if (this.manager != null)
			{
				this.manager.RemoveSprite(s);
			}
			if (s.manager != null)
			{
				s.manager.AddSprite(s);
			}
		}
	}

	protected virtual void HandleDrawLayerChange(SpriteRoot s)
	{
		if (!s.managed)
		{
			s.drawLayer = 0;
			return;
		}
		s.SetDrawLayer(s.drawLayer);
	}
}
