using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Progress Bar")]
public class UIProgressBar : AutoSpriteControlBase
{
	protected float m_value;

	protected AutoSprite emptySprite;

	[HideInInspector]
	public TextureAnim[] states = new TextureAnim[]
	{
		new TextureAnim("Filled"),
		new TextureAnim("Empty")
	};

	public SpriteRoot[] filledLayers = new SpriteRoot[0];

	public SpriteRoot[] emptyLayers = new SpriteRoot[0];

	protected int[] filledIndices;

	protected int[] emptyIndices;

	public float Value
	{
		get
		{
			return this.m_value;
		}
		set
		{
			this.m_value = Mathf.Clamp01(value);
			this.UpdateProgress();
		}
	}

	public override TextureAnim[] States
	{
		get
		{
			return this.states;
		}
		set
		{
			this.states = value;
		}
	}

	public override EZTransitionList[] Transitions
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public override IUIContainer Container
	{
		get
		{
			return base.Container;
		}
		set
		{
			if (value != this.container)
			{
				if (this.container != null)
				{
					this.container.RemoveChild(this.emptySprite.gameObject);
				}
				if (value != null && this.emptySprite != null)
				{
					value.AddChild(this.emptySprite.gameObject);
				}
			}
			base.Container = value;
		}
	}

	public override EZTransitionList GetTransitions(int index)
	{
		return null;
	}

	public override void OnInput(ref POINTER_INFO ptr)
	{
	}

	public override void Start()
	{
		if (this.m_started)
		{
			return;
		}
		base.Start();
		this.aggregateLayers = new SpriteRoot[2][];
		this.aggregateLayers[0] = this.filledLayers;
		this.aggregateLayers[1] = this.emptyLayers;
		if (Application.isPlaying)
		{
			this.filledIndices = new int[this.filledLayers.Length];
			this.emptyIndices = new int[this.emptyLayers.Length];
			for (int i = 0; i < this.filledLayers.Length; i++)
			{
				if (this.filledLayers[i] == null)
				{
					TsLog.LogError("A null layer sprite was encountered on control \"" + base.name + "\". Please fill in the layer reference, or remove the empty element.", new object[0]);
				}
				else
				{
					this.filledIndices[i] = this.filledLayers[i].GetStateIndex("filled");
					if (this.filledIndices[i] != -1)
					{
						this.filledLayers[i].SetState(this.filledIndices[i]);
					}
				}
			}
			for (int j = 0; j < this.emptyLayers.Length; j++)
			{
				if (this.emptyLayers[j] == null)
				{
					TsLog.LogError("A null layer sprite was encountered on control \"" + base.name + "\". Please fill in the layer reference, or remove the empty element.", new object[0]);
				}
				else
				{
					this.emptyIndices[j] = this.emptyLayers[j].GetStateIndex("empty");
					if (this.emptyIndices[j] != -1)
					{
						this.emptyLayers[j].SetState(this.emptyIndices[j]);
					}
				}
			}
			GameObject gameObject = new GameObject();
			gameObject.name = base.name + " - Empty Bar";
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.layer = base.gameObject.layer;
			this.emptySprite = (AutoSprite)gameObject.AddComponent(typeof(AutoSprite));
			this.emptySprite.plane = this.plane;
			this.emptySprite.autoResize = this.autoResize;
			this.emptySprite.pixelPerfect = this.pixelPerfect;
			this.emptySprite.persistent = this.persistent;
			this.emptySprite.ignoreClipping = this.ignoreClipping;
			this.emptySprite.bleedCompensation = this.bleedCompensation;
			if (!this.managed)
			{
				this.emptySprite.renderer.sharedMaterial = base.renderer.sharedMaterial;
			}
			else if (this.manager != null)
			{
				this.emptySprite.Managed = this.managed;
				this.manager.AddSprite(this.emptySprite);
				this.emptySprite.SetDrawLayer(this.drawLayer);
			}
			else
			{
				TsLog.LogError("Sprite on object \"" + base.name + "\" not assigned to a SpriteManager!", new object[0]);
			}
			this.emptySprite.SetAnchor(this.anchor);
			this.emptySprite.Setup(this.width, this.height, this.m_spriteMesh.material);
			if (this.states[1].spriteFrames.Length != 0)
			{
				this.emptySprite.animations = new UVAnimation[1];
				this.emptySprite.animations[0] = new UVAnimation();
				this.emptySprite.animations[0].SetAnim(this.states[1], 0);
				this.emptySprite.PlayAnim(0, 0);
			}
			this.emptySprite.renderCamera = this.renderCamera;
			this.emptySprite.Hide(base.IsHidden());
			this.Value = this.m_value;
			if (this.container != null)
			{
				this.container.AddChild(gameObject);
			}
			this.SetState(0);
		}
		if (this.managed && this.m_hidden)
		{
			this.Hide(true);
		}
	}

	public override void SetSize(float width, float height)
	{
		base.SetSize(width, height);
		if (this.emptySprite == null)
		{
			return;
		}
		this.emptySprite.SetSize(width, height);
	}

	public override void Copy(SpriteRoot s)
	{
		this.Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);
		if (!(s is UIProgressBar))
		{
			return;
		}
		if (Application.isPlaying)
		{
			UIProgressBar uIProgressBar = (UIProgressBar)s;
			if ((flags & ControlCopyFlags.Appearance) == ControlCopyFlags.Appearance && this.emptySprite != null)
			{
				this.emptySprite.Copy(uIProgressBar.emptySprite);
			}
		}
	}

	public override void InitUVs()
	{
		if (this.states[0].spriteFrames.Length != 0)
		{
			this.frameInfo.Copy(this.states[0].spriteFrames[0]);
		}
		base.InitUVs();
	}

	protected void UpdateProgress()
	{
		this.TruncateRight(this.m_value);
		if (this.emptySprite != null)
		{
			this.emptySprite.TruncateLeft(1f - this.m_value);
		}
		for (int i = 0; i < this.filledLayers.Length; i++)
		{
			this.filledLayers[i].TruncateRight(this.m_value);
		}
		for (int j = 0; j < this.emptyLayers.Length; j++)
		{
			this.emptyLayers[j].TruncateLeft(1f - this.m_value);
		}
	}

	public override void Unclip()
	{
		if (this.ignoreClipping)
		{
			return;
		}
		base.Unclip();
		this.emptySprite.Unclip();
	}

	public override bool IsClipped()
	{
		return base.IsClipped();
	}

	public override void SetClipped(bool value)
	{
		if (this.ignoreClipping)
		{
			return;
		}
		base.SetClipped(value);
		this.emptySprite.SetClipped(value);
	}

	public override Rect3D GetClippingRect()
	{
		return base.GetClippingRect();
	}

	public override void SetClippingRect(Rect3D value)
	{
		if (this.ignoreClipping)
		{
			return;
		}
		base.SetClippingRect(value);
		this.emptySprite.SetClippingRect(value);
	}

	public static UIProgressBar Create(string name, Vector3 pos)
	{
		return (UIProgressBar)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIProgressBar));
	}

	public static UIProgressBar Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIProgressBar)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIProgressBar));
	}

	public override void Hide(bool tf)
	{
		base.Hide(tf);
		if (this.emptySprite != null)
		{
			this.emptySprite.Hide(tf);
		}
	}

	public override void SetColor(Color c)
	{
		base.SetColor(c);
		if (this.emptySprite != null)
		{
			this.emptySprite.SetColor(c);
		}
	}
}
