using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Slider")]
public class UISlider : AutoSpriteControlBase
{
	protected float m_value;

	public MonoBehaviour scriptWithMethodToInvoke;

	public string methodToInvoke = string.Empty;

	public float defaultValue;

	public float stopKnobFromEdge;

	public Vector3 knobOffset = new Vector3(1f, 0f, -0.1f);

	public Vector2 knobSize;

	public SpriteTile m_sprKnobTile = new SpriteTile();

	public Vector2 knobColliderSizeFactor = new Vector2(2f, 2f);

	public SimpleSprite emptySprite;

	protected UIScrollKnob knob;

	public UIButton upButton;

	public UIButton downButton;

	public bool darkStyle;

	public float totalLineCount;

	public float lineHeight;

	public bool useknobButton = true;

	public static float buttonX = 8f;

	public static float buttonY = 8f;

	public UIScrollList list;

	[HideInInspector]
	public TextureAnim[] states = new TextureAnim[]
	{
		new TextureAnim("Filled bar"),
		new TextureAnim("Knob, Normal"),
		new TextureAnim("Knob, Over"),
		new TextureAnim("Knob, Active")
	};

	[HideInInspector]
	public EZTransitionList[] transitions = new EZTransitionList[]
	{
		null,
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From Over"),
			new EZTransition("From Active"),
			new EZTransition("From Disabled")
		}),
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From Normal"),
			new EZTransition("From Active")
		}),
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From Normal"),
			new EZTransition("From Over")
		})
	};

	public SpriteRoot[] filledLayers = new SpriteRoot[0];

	public SpriteRoot[] emptyLayers = new SpriteRoot[0];

	public SpriteRoot[] knobLayers = new SpriteRoot[0];

	protected float truncFloor;

	protected float truncRange;

	protected int[] filledIndices;

	protected int[] emptyIndices;

	private bool callChangeDelegate = true;

	public bool clickButton;

	public override bool Visible
	{
		get
		{
			return base.gameObject.activeInHierarchy;
		}
		set
		{
			base.gameObject.SetActive(value);
		}
	}

	public override bool controlIsEnabled
	{
		get
		{
			return this.m_controlIsEnabled;
		}
		set
		{
			this.m_controlIsEnabled = value;
			if (null != this.knob)
			{
				this.knob.controlIsEnabled = value;
			}
		}
	}

	public float Value
	{
		get
		{
			return this.m_value;
		}
		set
		{
			float value2 = this.m_value;
			this.m_value = Mathf.Clamp01(value);
			if (this.m_value != value2)
			{
				this.UpdateValue();
			}
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
			return this.transitions;
		}
		set
		{
			this.transitions = value;
		}
	}

	public bool CallChangeDelegate
	{
		set
		{
			this.callChangeDelegate = value;
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
					this.container.RemoveChild(this.upButton.gameObject);
					this.container.RemoveChild(this.downButton.gameObject);
					this.container.RemoveChild(this.emptySprite.gameObject);
					this.container.RemoveChild(this.knob.gameObject);
				}
				if (value != null)
				{
					if (this.upButton != null)
					{
						value.AddChild(this.upButton.gameObject);
					}
					if (this.downButton != null)
					{
						value.AddChild(this.downButton.gameObject);
					}
					if (this.emptySprite != null)
					{
						value.AddChild(this.emptySprite.gameObject);
					}
					if (this.knob != null)
					{
						value.AddChild(this.knob.gameObject);
					}
				}
			}
			base.Container = value;
		}
	}

	public override void OnInput(ref POINTER_INFO ptr)
	{
		base.OnInput(ref ptr);
		if (!this.m_controlIsEnabled)
		{
			return;
		}
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			if (null != this.list)
			{
				this.list.overList = false;
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOUSE_WHEEL:
			if (null != this.list)
			{
				this.list.overList = true;
			}
			break;
		}
	}

	public void UpdateKnobPosition(float value)
	{
		float value2 = this.m_value;
		this.m_value = Mathf.Clamp01(value);
		if (this.m_value != value2)
		{
			if (this.knob == null)
			{
				return;
			}
			float truncVal = this.truncFloor + this.m_value * this.truncRange;
			this.UpdateAppearance(truncVal);
			this.knob.SetPosition(this.m_value);
		}
	}

	public override EZTransitionList GetTransitions(int index)
	{
		if (index >= this.transitions.Length)
		{
			return null;
		}
		return this.transitions[index];
	}

	public void ClickUpButton(IUIObject obj)
	{
		if (this.knob)
		{
			this.knob.UpButtonScroll(this.lineHeight);
		}
	}

	public void ClickUpButtonDonotCallChangeDelegate(IUIObject obj)
	{
		this.callChangeDelegate = false;
		if (this.knob)
		{
			this.knob.UpButtonScroll(this.lineHeight);
		}
	}

	public void ClickDownButton(IUIObject obj)
	{
		if (this.knob)
		{
			this.knob.DownButtonScroll(this.lineHeight);
		}
	}

	public void ClickDownButtonDonotCallChangeDelegate(IUIObject obj)
	{
		this.callChangeDelegate = false;
		if (this.knob)
		{
			this.knob.DownButtonScroll(this.lineHeight);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.m_value = this.defaultValue;
		this.upButton = (UIButton)new GameObject
		{
			name = base.name + " - UpButton",
			transform = 
			{
				parent = base.transform
			}
		}.AddComponent(typeof(UIButton));
		this.upButton.renderCamera = this.renderCamera;
		this.upButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickUpButton));
		this.downButton = (UIButton)new GameObject
		{
			name = base.name + " - DownButton",
			transform = 
			{
				parent = base.transform
			}
		}.AddComponent(typeof(UIButton));
		this.downButton.renderCamera = this.renderCamera;
		this.downButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDownButton));
		this.emptySprite = (SimpleSprite)new GameObject
		{
			name = base.name + " - Bar",
			transform = 
			{
				parent = base.transform
			}
		}.AddComponent(typeof(SimpleSprite));
		this.emptySprite.autoResize = false;
		this.emptySprite.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		this.emptySprite.renderCamera = this.renderCamera;
		this.emptySprite.gameObject.layer = GUICamera.UILayer;
		this.emptySprite.SetWindingOrder(SpriteRoot.WINDING_ORDER.CW);
	}

	public override void Start()
	{
		if (this.m_started)
		{
			return;
		}
		base.Start();
		if (Application.isPlaying)
		{
			this.truncFloor = this.stopKnobFromEdge / this.width;
			this.truncRange = 1f - this.truncFloor * 2f;
			this.knob = (UIScrollKnob)new GameObject
			{
				name = base.name + " - Knob",
				transform = 
				{
					parent = base.transform,
					localPosition = this.CalcKnobStartPos(),
					localRotation = Quaternion.identity,
					localScale = Vector3.one
				},
				layer = base.gameObject.layer
			}.AddComponent(typeof(UIScrollKnob));
			this.knob.plane = this.plane;
			this.knob.SetOffset(this.knobOffset);
			this.knob.persistent = this.persistent;
			this.knob.bleedCompensation = this.bleedCompensation;
			if (!this.managed)
			{
				if (this.knob.spriteMesh != null)
				{
					((SpriteMesh)this.knob.spriteMesh).material = base.renderer.sharedMaterial;
				}
			}
			else if (this.manager != null)
			{
				this.knob.Managed = this.managed;
				this.manager.AddSprite(this.knob);
				this.knob.SetDrawLayer(this.drawLayer + 1);
			}
			else
			{
				TsLog.LogError("Sprite on object \"" + base.name + "\" not assigned to a SpriteManager!", new object[0]);
			}
			this.knob.SetSpriteTile(this.m_sprKnobTile.m_eTileMode, this.m_sprKnobTile.m_fTileWidth, this.m_sprKnobTile.m_fTileHeight);
			this.knob.autoResize = this.autoResize;
			if (this.pixelPerfect)
			{
				this.knob.pixelPerfect = true;
			}
			else
			{
				this.knob.SetSize(this.knobSize.x, this.knobSize.y);
			}
			this.knob.ignoreClipping = this.ignoreClipping;
			this.knob.SetColliderSizeFactor(this.knobColliderSizeFactor);
			this.knob.SetSlider(this);
			this.knob.SetMaxScroll(this.width - this.stopKnobFromEdge * 2f);
			this.knob.SetInputDelegate(this.inputDelegate);
			this.knob.transitions[0] = this.transitions[1];
			this.knob.transitions[1] = this.transitions[2];
			this.knob.transitions[2] = this.transitions[3];
			this.knob.layers = this.knobLayers;
			for (int i = 0; i < this.knobLayers.Length; i++)
			{
				this.knobLayers[i].transform.parent = this.knob.transform;
			}
			this.knob.animations[0].SetAnim(this.states[1], 0);
			this.knob.animations[1].SetAnim(this.states[2], 1);
			this.knob.animations[2].SetAnim(this.states[3], 2);
			this.knob.SetupAppearance();
			this.knob.SetCamera(this.renderCamera);
			this.knob.Hide(base.IsHidden());
			if (this.container != null)
			{
				this.container.AddChild(this.knob.gameObject);
				this.container.AddChild(this.emptySprite.gameObject);
				this.container.AddChild(this.upButton.gameObject);
				this.container.AddChild(this.downButton.gameObject);
				this.downButton.PlayAnim(0, 0);
				this.upButton.PlayAnim(0, 0);
			}
			BoxCollider boxCollider = (BoxCollider)base.gameObject.AddComponent(typeof(BoxCollider));
			boxCollider.size = new Vector3(this.width, this.height, 0f);
			boxCollider.center = new Vector3(this.width / 2f, this.height / 2f, 0f);
			boxCollider.isTrigger = true;
			this.customCollider = true;
			this.SetState(0);
			this.m_value = -1f;
			this.Value = this.defaultValue;
		}
		if (this.managed && this.m_hidden)
		{
			this.Hide(true);
		}
		this.SetColor(this.color);
	}

	public void SetList(UIScrollList c)
	{
		this.list = c;
	}

	public override void SetSize(float width, float height)
	{
		base.SetSize(width, height);
		if (this.knob == null)
		{
			return;
		}
		this.knob.SetStartPos(this.CalcKnobStartPos());
		this.knob.SetMaxScroll(width - this.stopKnobFromEdge * 2f);
		this.knob.SetPosition(this.m_value);
		if (this.upButton)
		{
			if (this.darkStyle)
			{
				this.upButton.transform.localPosition = new Vector3(-this.upButton.height / 2f - 1f, height / 2f, -0.5f);
			}
			else
			{
				this.upButton.transform.localPosition = new Vector3(UISlider.buttonX, UISlider.buttonY, -0.5f);
			}
		}
		if (this.downButton)
		{
			if (this.darkStyle)
			{
				this.downButton.transform.localPosition = new Vector3(width + this.downButton.height / 2f + 2f, height / 2f, -0.5f);
			}
			else
			{
				this.downButton.transform.localPosition = new Vector3(width - UISlider.buttonX, UISlider.buttonY, -0.5f);
			}
		}
	}

	public override void Copy(SpriteRoot s)
	{
		this.Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);
		if (!(s is UISlider))
		{
			return;
		}
		UISlider uISlider = (UISlider)s;
		if ((flags & ControlCopyFlags.Invocation) == ControlCopyFlags.Invocation)
		{
			this.scriptWithMethodToInvoke = uISlider.scriptWithMethodToInvoke;
			this.methodToInvoke = uISlider.methodToInvoke;
		}
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.defaultValue = uISlider.defaultValue;
			this.stopKnobFromEdge = uISlider.stopKnobFromEdge;
			this.knobOffset = uISlider.knobOffset;
			this.knobSize = uISlider.knobSize;
			this.knobColliderSizeFactor = uISlider.knobColliderSizeFactor;
		}
		if ((flags & ControlCopyFlags.Appearance) == ControlCopyFlags.Appearance && Application.isPlaying)
		{
			if (this.emptySprite != null)
			{
				this.emptySprite.Copy(uISlider.emptySprite);
			}
			if (this.knob != null)
			{
				this.knob.Copy(uISlider.knob);
			}
			this.truncFloor = uISlider.truncFloor;
			this.truncRange = uISlider.truncRange;
		}
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			this.CalcKnobStartPos();
			this.Value = uISlider.Value;
		}
	}

	protected Vector3 CalcKnobStartPos()
	{
		Vector3 zero = Vector3.zero;
		zero.z = -1f;
		switch (this.anchor)
		{
		case SpriteRoot.ANCHOR_METHOD.UPPER_LEFT:
			zero.x = this.stopKnobFromEdge;
			zero.y = this.height * -0.5f;
			break;
		case SpriteRoot.ANCHOR_METHOD.UPPER_CENTER:
			zero.x = this.width * -0.5f + this.stopKnobFromEdge;
			zero.y = this.height * -0.5f;
			break;
		case SpriteRoot.ANCHOR_METHOD.UPPER_RIGHT:
			zero.x = this.width * -1f + this.stopKnobFromEdge;
			zero.y = this.height * -0.5f;
			break;
		case SpriteRoot.ANCHOR_METHOD.MIDDLE_LEFT:
			zero.x = this.stopKnobFromEdge;
			break;
		case SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER:
			zero.x = this.width * -0.5f + this.stopKnobFromEdge;
			break;
		case SpriteRoot.ANCHOR_METHOD.MIDDLE_RIGHT:
			zero.x = this.width * -1f + this.stopKnobFromEdge;
			break;
		case SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT:
			zero.x = this.stopKnobFromEdge;
			zero.y = this.height * 0.5f;
			break;
		case SpriteRoot.ANCHOR_METHOD.BOTTOM_CENTER:
			zero.x = this.width * -0.5f + this.stopKnobFromEdge;
			zero.y = this.height * 0.5f;
			break;
		case SpriteRoot.ANCHOR_METHOD.BOTTOM_RIGHT:
			zero.x = this.width * -1f + this.stopKnobFromEdge;
			zero.y = this.height * 0.5f;
			break;
		case SpriteRoot.ANCHOR_METHOD.TEXTURE_OFFSET:
			zero.x = this.width * -0.5f + this.stopKnobFromEdge;
			break;
		}
		return zero;
	}

	public override void InitUVs()
	{
		if (this.states[0].spriteFrames.Length != 0)
		{
			this.frameInfo.Copy(this.states[0].spriteFrames[0]);
		}
		base.InitUVs();
	}

	protected void UpdateValue()
	{
		if (this.knob == null)
		{
			return;
		}
		float truncVal = this.truncFloor + this.m_value * this.truncRange;
		this.UpdateAppearance(truncVal);
		this.knob.SetPosition(this.m_value);
		if (this.scriptWithMethodToInvoke != null)
		{
			this.scriptWithMethodToInvoke.Invoke(this.methodToInvoke, 0f);
		}
		if (this.callChangeDelegate && this.changeDelegate != null)
		{
			this.changeDelegate(this);
		}
		this.callChangeDelegate = true;
	}

	public void ScrollKnobMoved(UIScrollKnob knob, float val)
	{
		this.m_value = val;
		float truncVal = this.truncFloor + this.m_value * this.truncRange;
		this.UpdateAppearance(truncVal);
		if (this.scriptWithMethodToInvoke != null)
		{
			this.scriptWithMethodToInvoke.Invoke(this.methodToInvoke, 0f);
		}
		if (this.callChangeDelegate && this.changeDelegate != null)
		{
			this.changeDelegate(this);
		}
		this.callChangeDelegate = true;
	}

	public override void SetInputDelegate(EZInputDelegate del)
	{
		if (this.knob != null)
		{
			this.knob.SetInputDelegate(del);
		}
		base.SetInputDelegate(del);
	}

	public override void AddInputDelegate(EZInputDelegate del)
	{
		if (this.knob != null)
		{
			this.knob.AddInputDelegate(del);
		}
		base.AddInputDelegate(del);
	}

	public override void RemoveInputDelegate(EZInputDelegate del)
	{
		if (this.knob != null)
		{
			this.knob.RemoveInputDelegate(del);
		}
		base.RemoveInputDelegate(del);
	}

	protected void UpdateAppearance(float truncVal)
	{
		this.emptySprite.SetSize(this.width * truncVal, this.emptySprite.height);
	}

	public UIScrollKnob GetKnob()
	{
		return this.knob;
	}

	public override void Unclip()
	{
		if (this.ignoreClipping)
		{
			return;
		}
		base.Unclip();
		this.upButton.Unclip();
		this.downButton.Unclip();
		this.emptySprite.Unclip();
		this.knob.Unclip();
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
		this.upButton.SetClipped(value);
		this.downButton.SetClipped(value);
		this.emptySprite.SetClipped(value);
		this.knob.SetClipped(value);
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
		this.upButton.SetClippingRect(value);
		this.downButton.SetClippingRect(value);
		this.emptySprite.SetClippingRect(value);
		this.knob.SetClippingRect(value);
	}

	public static UISlider Create(string name, Vector3 pos)
	{
		return (UISlider)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UISlider));
	}

	public static UISlider Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UISlider)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UISlider));
	}

	public override void Hide(bool tf)
	{
		base.Hide(tf);
		if (this.emptySprite != null)
		{
			this.emptySprite.Hide(tf);
		}
		if (this.knob != null)
		{
			this.knob.Hide(tf);
		}
		if (this.upButton != null)
		{
			this.upButton.Hide(tf);
		}
		if (this.downButton != null)
		{
			this.downButton.Hide(tf);
		}
	}

	public override void SetColor(Color c)
	{
		base.SetColor(c);
		if (this.emptySprite != null)
		{
			this.emptySprite.SetColor(c);
		}
		if (this.knob != null)
		{
			this.knob.SetColor(c);
		}
		if (this.upButton != null)
		{
			this.upButton.SetColor(c);
		}
		if (this.downButton != null)
		{
			this.downButton.SetColor(c);
		}
	}
}
