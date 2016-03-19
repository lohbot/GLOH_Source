using GameMessage;
using System;
using System.Globalization;
using UnityEngine;
using UnityForms;

public abstract class AutoSpriteControlBase : AutoSpriteBase, IEZDragDrop, IControl, IUIObject
{
	public const string DittoString = "[\"]";

	protected Vector3 bStartScale = Vector3.zero;

	protected bool bStartAni;

	protected bool bEffectAni = true;

	protected bool bUseDefaultSound = true;

	protected bool nullCamera;

	protected string text;

	public SpriteText spriteText;

	public SpriteText spriteTextShadow;

	public float maxWidth;

	public bool multiLine = true;

	public float textOffsetZ = -0.1f;

	public bool includeTextInAutoCollider;

	public float fontSize = 16f;

	protected SpriteText.Font_Effect defaultFontEffect = SpriteText.Font_Effect.Black_Shadow_Small;

	protected SpriteText.Anchor_Pos defaultTextAnchor;

	protected SpriteText.Alignment_Type defaultTextAlignment = SpriteText.Alignment_Type.Center;

	private bool ChangeUVs;

	private object m_cItem;

	private object m_cItemSecond;

	private int m_nItemUnique;

	private byte m_nEquipItemPosition;

	protected string m_strToolTip = string.Empty;

	private bool m_bShowToolTip;

	private INVERSE_MODE m_eInverseMode = INVERSE_MODE.NULL;

	private Vector3 m_v3ForRotate = Vector3.zero;

	private bool updateText;

	public bool detargetOnDisable;

	public bool customCollider;

	protected Vector3 savedColliderSize;

	protected Vector2 topLeftEdge;

	protected Vector2 bottomRightEdge;

	[HideInInspector]
	public object data;

	protected SpriteRoot[][] aggregateLayers;

	private bool autoAnimatorStop = true;

	private int layer;

	protected bool m_controlIsEnabled = true;

	protected IUIContainer container;

	protected EZInputDelegate inputDelegate;

	protected EZGameObjectDelegate gameObjectDelegate;

	protected EZValueChangedDelegate loadCompleteDelegate;

	protected EZValueChangedDelegate changeDelegate;

	protected EZValueChangedDelegate mouseOverDelegate;

	protected EZValueChangedDelegate mouseOutDelegate;

	protected EZValueChangedDelegate rightMouseDelegate;

	protected EZValueChangedDelegate doubleClickDelegate;

	protected EZValueChangedDelegate mouseDownDelegate;

	private bool m_bSetToolTip;

	protected EZDragDropHelper dragDropHelper = new EZDragDropHelper();

	public bool isDraggable;

	public float dragOffset = float.NaN;

	public float mouseOffset = 1f;

	public EZAnimation.EASING_TYPE cancelDragEasing;

	public float cancelDragDuration = -1f;

	protected Material texMat;

	private UIBaseInfoLoader kBaseInfo = new UIBaseInfoLoader();

	public bool EffectAni
	{
		get
		{
			return this.bEffectAni;
		}
		set
		{
			this.bEffectAni = value;
		}
	}

	public bool UseDefaultSound
	{
		set
		{
			this.bUseDefaultSound = value;
		}
	}

	public bool SPOT
	{
		set
		{
			if (null != this.spriteText)
			{
				this.spriteText.SPOT = value;
			}
			if (null != this.spriteTextShadow)
			{
				this.spriteTextShadow.SPOT = value;
			}
		}
	}

	public bool PassWord
	{
		set
		{
			if (null != this.spriteText)
			{
				this.spriteText.password = value;
			}
			if (null != this.spriteTextShadow)
			{
				this.spriteTextShadow.password = value;
			}
		}
	}

	public float MaxWidth
	{
		get
		{
			return this.maxWidth;
		}
		set
		{
			this.maxWidth = value;
			if (null != this.spriteText)
			{
				this.spriteText.maxWidth = value;
			}
			if (null != this.spriteTextShadow)
			{
				this.spriteTextShadow.maxWidth = value;
			}
		}
	}

	public bool MultiLine
	{
		get
		{
			return null != this.spriteText && this.spriteText.multiline;
		}
		set
		{
			if (null != this.spriteText)
			{
				this.spriteText.multiline = value;
			}
			if (null != this.spriteTextShadow)
			{
				this.spriteTextShadow.multiline = value;
			}
		}
	}

	public SpriteText.Font_Effect DefaultFontEffect
	{
		set
		{
			this.defaultFontEffect = value;
		}
	}

	public SpriteText.Anchor_Pos DefaultTextAnchor
	{
		set
		{
			this.defaultTextAnchor = value;
		}
	}

	public SpriteText.Alignment_Type DefaultTextAlignment
	{
		set
		{
			this.defaultTextAlignment = value;
		}
	}

	public G_ID c_eWindowID
	{
		get;
		set;
	}

	public string ColorText
	{
		get
		{
			if (this.spriteText)
			{
				return this.spriteText.ColorText;
			}
			return string.Empty;
		}
		set
		{
			if (this.spriteText)
			{
				this.ParseColorText(ref this.spriteText, value);
			}
			else
			{
				this.Text = " ";
				if (this.spriteText)
				{
					this.ParseColorText(ref this.spriteText, value);
				}
			}
		}
	}

	public string ColorTextShadow
	{
		get
		{
			if (this.spriteTextShadow)
			{
				return this.spriteTextShadow.ColorText;
			}
			return string.Empty;
		}
		set
		{
			if (this.spriteTextShadow)
			{
				this.ParseColorText(ref this.spriteTextShadow, value);
			}
			else
			{
				this.Text = " ";
				if (this.spriteText)
				{
					this.ParseColorText(ref this.spriteTextShadow, value);
				}
			}
		}
	}

	public bool UpdateText
	{
		set
		{
			this.updateText = value;
		}
	}

	public virtual string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			if (this.text != value || this.updateText)
			{
				this.text = value;
				this.CreateSpriteText();
				if (null != this.spriteText)
				{
					this.spriteText.m_fParentWidth = this.width;
					this.spriteText.m_fParentHeight = -this.height;
					this.spriteText.Text = this.text;
				}
				if (null != this.spriteTextShadow)
				{
					this.spriteTextShadow.m_fParentWidth = this.width;
					this.spriteTextShadow.m_fParentHeight = -this.height;
					this.spriteTextShadow.Text = this.text;
				}
				if (this.includeTextInAutoCollider)
				{
					this.UpdateCollider();
				}
				if (null != this.spriteTextShadow && null != this.spriteText)
				{
					this.spriteTextShadow.transform.localPosition = new Vector3(this.spriteText.transform.localPosition.x + 0.9f, this.spriteText.transform.localPosition.y - 0.6f, this.spriteText.transform.localPosition.z + 0.002f);
				}
			}
		}
	}

	public object Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
		}
	}

	public virtual bool IncludeTextInAutoCollider
	{
		get
		{
			return this.includeTextInAutoCollider;
		}
		set
		{
			this.includeTextInAutoCollider = value;
			this.UpdateCollider();
		}
	}

	public virtual bool Visible
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

	public virtual bool RenderEnabled
	{
		get
		{
			return base.gameObject.renderer.enabled;
		}
		set
		{
			base.gameObject.renderer.enabled = value;
		}
	}

	public EZValueChangedDelegate SelectionChange
	{
		get
		{
			return this.changeDelegate;
		}
		set
		{
			this.changeDelegate = value;
		}
	}

	public EZValueChangedDelegate TextChanged
	{
		get
		{
			return this.changeDelegate;
		}
		set
		{
			this.changeDelegate = value;
		}
	}

	public EZValueChangedDelegate MouseUp
	{
		get
		{
			return this.changeDelegate;
		}
		set
		{
			this.changeDelegate = value;
		}
	}

	public EZValueChangedDelegate MouseOut
	{
		get
		{
			return this.changeDelegate;
		}
		set
		{
			this.changeDelegate = value;
		}
	}

	public EZValueChangedDelegate MouseOver
	{
		get
		{
			return this.changeDelegate;
		}
		set
		{
			this.changeDelegate = value;
		}
	}

	public object c_cItemTooltip
	{
		get
		{
			return this.m_cItem;
		}
		set
		{
			if (value == null)
			{
				MsgHandler.Handle("CloseToolTip", new object[0]);
				this.m_bShowToolTip = false;
			}
			else
			{
				this.m_bShowToolTip = true;
			}
			this.m_cItem = value;
		}
	}

	public object c_cItemSecondTooltip
	{
		get
		{
			return this.m_cItemSecond;
		}
		set
		{
			if (value == null)
			{
				MsgHandler.Handle("CloseToolTip", new object[0]);
				this.m_bShowToolTip = false;
			}
			else
			{
				this.m_bShowToolTip = true;
			}
			this.m_cItemSecond = value;
		}
	}

	public byte nEquipItemPosition
	{
		get
		{
			return this.m_nEquipItemPosition;
		}
		set
		{
			this.m_nEquipItemPosition = value;
		}
	}

	public int nItemUniqueTooltip
	{
		get
		{
			return this.m_nItemUnique;
		}
		set
		{
			if (value > 0)
			{
				MsgHandler.Handle("CloseToolTip", new object[0]);
				this.m_bShowToolTip = true;
			}
			else
			{
				this.m_bShowToolTip = false;
			}
			this.m_nItemUnique = value;
		}
	}

	public string ToolTip
	{
		get
		{
			return this.m_strToolTip;
		}
		set
		{
			if (value != string.Empty)
			{
				this.m_bShowToolTip = true;
				this.m_strToolTip = value;
			}
		}
	}

	public bool ShowToolTip
	{
		get
		{
			return this.m_bShowToolTip;
		}
		set
		{
			this.m_bShowToolTip = value;
		}
	}

	public bool AutoAnimatorStop
	{
		set
		{
			this.autoAnimatorStop = value;
		}
	}

	public virtual int Layer
	{
		get
		{
			return this.layer;
		}
		set
		{
			this.layer = value;
		}
	}

	public virtual bool controlIsEnabled
	{
		get
		{
			return this.m_controlIsEnabled;
		}
		set
		{
			this.m_controlIsEnabled = value;
		}
	}

	public virtual bool DetargetOnDisable
	{
		get
		{
			return this.DetargetOnDisable;
		}
		set
		{
			this.DetargetOnDisable = value;
		}
	}

	public virtual IUIContainer Container
	{
		get
		{
			return this.container;
		}
		set
		{
			if (this.container != null)
			{
				if (this.aggregateLayers != null)
				{
					for (int i = 0; i < this.aggregateLayers.Length; i++)
					{
						if (this.aggregateLayers[i] != null)
						{
							for (int j = 0; j < this.aggregateLayers[i].Length; j++)
							{
								this.container.RemoveChild(this.aggregateLayers[i][j].gameObject);
							}
						}
					}
				}
				if (this.spriteText != null)
				{
					this.container.RemoveChild(this.spriteText.gameObject);
				}
				if (this.spriteTextShadow != null)
				{
					this.container.RemoveChild(this.spriteTextShadow.gameObject);
				}
			}
			if (value != null)
			{
				if (this.aggregateLayers != null)
				{
					for (int k = 0; k < this.aggregateLayers.Length; k++)
					{
						if (this.aggregateLayers[k] != null)
						{
							for (int l = 0; l < this.aggregateLayers[k].Length; l++)
							{
								value.AddChild(this.aggregateLayers[k][l].gameObject);
							}
						}
					}
				}
				if (this.spriteText != null)
				{
					value.AddChild(this.spriteText.gameObject);
				}
				if (this.spriteTextShadow != null)
				{
					value.AddChild(this.spriteTextShadow.gameObject);
				}
			}
			this.container = value;
		}
	}

	public bool IsDraggable
	{
		get
		{
			return this.isDraggable;
		}
		set
		{
			this.isDraggable = value;
		}
	}

	public float DragOffset
	{
		get
		{
			return this.dragOffset;
		}
		set
		{
			this.dragOffset = value;
			POINTER_INFO ptr;
			if (this.IsDragging() && UIManager.Exists() && NrTSingleton<UIManager>.Instance.GetPointer(this, out ptr))
			{
				this.dragDropHelper.DragUpdatePosition(ptr);
			}
		}
	}

	public float MouseOffset
	{
		get
		{
			return this.mouseOffset;
		}
		set
		{
			this.mouseOffset = value;
		}
	}

	public EZAnimation.EASING_TYPE CancelDragEasing
	{
		get
		{
			return this.cancelDragEasing;
		}
		set
		{
			this.cancelDragEasing = value;
		}
	}

	public float CancelDragDuration
	{
		get
		{
			return this.cancelDragDuration;
		}
		set
		{
			this.cancelDragDuration = value;
		}
	}

	public GameObject DropTarget
	{
		get
		{
			return this.dragDropHelper.DropTarget;
		}
		set
		{
			this.dragDropHelper.DropTarget = value;
		}
	}

	public bool DropHandled
	{
		get
		{
			return this.dragDropHelper.DropHandled;
		}
		set
		{
			this.dragDropHelper.DropHandled = value;
		}
	}

	public abstract EZTransitionList[] Transitions
	{
		get;
		set;
	}

	public Texture Image
	{
		set
		{
			this.SetTexture(value);
		}
	}

	public UIBaseInfoLoader BaseInfoLoderImage
	{
		set
		{
			this.SetTexture(value);
		}
	}

	public void SetCharacterSize(float fontSize)
	{
		this.fontSize = fontSize;
		if (null != this.spriteText)
		{
			this.spriteText.SetCharacterSize(fontSize);
		}
		if (null != this.spriteTextShadow)
		{
			this.spriteTextShadow.SetCharacterSize(fontSize);
		}
	}

	public void SetFontEffect(SpriteText.Font_Effect eFontEffect)
	{
		if (null != this.spriteText)
		{
			this.spriteText.SetFontEffect(eFontEffect);
		}
		if (null != this.spriteTextShadow)
		{
			this.spriteTextShadow.SetFontEffect(eFontEffect);
		}
	}

	public void SetAnchor(SpriteText.Anchor_Pos eAnchor_Pos)
	{
		if (null != this.spriteText)
		{
			this.spriteText.SetAnchor(eAnchor_Pos);
		}
		if (null != this.spriteTextShadow)
		{
			this.spriteTextShadow.SetAnchor(eAnchor_Pos);
		}
	}

	public void SetAlignment(SpriteText.Alignment_Type eAlignment_Type)
	{
		if (null != this.spriteText)
		{
			this.spriteText.SetAlignment(eAlignment_Type);
		}
		if (null != this.spriteTextShadow)
		{
			this.spriteTextShadow.SetAlignment(eAlignment_Type);
		}
	}

	private void ParseColorText(ref SpriteText text, string value)
	{
		if (text)
		{
			if (10 < value.Length && string.Compare(value, 0, "[#", 0, 2) == 0 && value[10] == ']')
			{
				int num = int.Parse(value.Substring(2, 2), NumberStyles.AllowHexSpecifier);
				int num2 = int.Parse(value.Substring(4, 2), NumberStyles.AllowHexSpecifier);
				int num3 = int.Parse(value.Substring(6, 2), NumberStyles.AllowHexSpecifier);
				int num4 = int.Parse(value.Substring(8, 2), NumberStyles.AllowHexSpecifier);
				text.color = new Color((float)num / 255f, (float)num2 / 255f, (float)num3 / 255f, (float)num4 / 255f);
			}
			text.ColorText = value;
		}
	}

	public void DeleteSpriteText()
	{
		if (null != this.spriteText)
		{
			UnityEngine.Object.Destroy(this.spriteText.gameObject);
		}
		if (null != this.spriteTextShadow)
		{
			UnityEngine.Object.Destroy(this.spriteTextShadow.gameObject);
		}
	}

	public void CreateSpriteText()
	{
		if (null == this.spriteText)
		{
			if (NrTSingleton<UIManager>.Instance == null)
			{
				TsLog.LogWarning("Warning: No UIManager exists in the scene. A UIManager with a default font is required to automatically add text to a control.", new object[0]);
				return;
			}
			GameObject gameObject = new GameObject();
			gameObject.layer = base.gameObject.layer;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "control_text";
			MeshRenderer meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
			meshRenderer.sharedMaterial = NrTSingleton<UIManager>.Instance.defaultFontMaterial;
			this.spriteText = (SpriteText)gameObject.AddComponent(typeof(SpriteText));
			this.spriteText.gameObject.layer = GUICamera.UILayer;
			this.spriteText.font = NrTSingleton<UIManager>.Instance.defaultFont;
			this.spriteText.offsetZ = this.textOffsetZ;
			this.spriteText.Persistent = this.persistent;
			this.spriteText.Parent = this;
			this.spriteText.anchor = this.defaultTextAnchor;
			this.spriteText.alignment = this.defaultTextAlignment;
			this.spriteText.pixelPerfect = false;
			this.spriteText.SetCharacterSize(this.fontSize);
			this.spriteText.SetCamera(this.renderCamera);
			this.spriteText.multiline = this.multiLine;
			this.spriteText.maxWidth = this.maxWidth;
			this.spriteText.color = this.color;
			if (Application.isPlaying)
			{
				this.spriteText.Persistent = this.persistent;
			}
			this.spriteText.Start();
			GameObject gameObject2 = new GameObject();
			gameObject2.layer = base.gameObject.layer;
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.name = "control_textShadow";
			MeshRenderer meshRenderer2 = (MeshRenderer)gameObject2.AddComponent(typeof(MeshRenderer));
			meshRenderer2.sharedMaterial = NrTSingleton<UIManager>.Instance.defaultFontMaterial;
			this.spriteTextShadow = (SpriteText)gameObject2.AddComponent(typeof(SpriteText));
			this.spriteTextShadow.gameObject.layer = GUICamera.UILayer;
			this.spriteTextShadow.font = NrTSingleton<UIManager>.Instance.defaultFont;
			this.spriteTextShadow.shadowText = true;
			this.spriteTextShadow.offsetZ = this.textOffsetZ;
			this.spriteTextShadow.Persistent = this.persistent;
			this.spriteTextShadow.Parent = this;
			this.spriteTextShadow.anchor = this.defaultTextAnchor;
			this.spriteTextShadow.alignment = this.defaultTextAlignment;
			this.spriteTextShadow.pixelPerfect = false;
			this.spriteTextShadow.SetCharacterSize(this.fontSize);
			this.spriteTextShadow.SetCamera(this.renderCamera);
			this.spriteTextShadow.multiline = this.multiLine;
			this.spriteTextShadow.maxWidth = this.maxWidth;
			this.spriteTextShadow.color = this.color;
			if (Application.isPlaying)
			{
				this.spriteTextShadow.Persistent = this.persistent;
			}
			this.spriteTextShadow.Start();
		}
	}

	protected override void Init()
	{
		this.nullCamera = (this.renderCamera == null);
		base.Init();
	}

	public void SetLocation(float x, float y, float z)
	{
		this.tempLocation.x = x;
		this.tempLocation.y = -y;
		this.tempLocation.z = z;
		this.tempRect.x = x;
		this.tempRect.y = y;
		base.transform.localPosition = this.tempLocation;
	}

	public void SetLocation(int x, int y, int z)
	{
		this.tempLocation.x = (float)x;
		this.tempLocation.y = (float)(-(float)y);
		this.tempLocation.z = (float)z;
		this.tempRect.x = (float)x;
		this.tempRect.y = (float)(-(float)y);
		base.transform.localPosition = this.tempLocation;
	}

	public void SetLocation(float x, float y)
	{
		this.tempLocation.x = x;
		this.tempLocation.y = -y;
		this.tempLocation.z = base.transform.localPosition.z;
		this.tempRect.x = x;
		this.tempRect.y = y;
		base.transform.localPosition = this.tempLocation;
	}

	public void SetLocation(int x, int y)
	{
		this.tempLocation.x = (float)x;
		this.tempLocation.y = (float)(-(float)y);
		this.tempLocation.z = base.transform.localPosition.z;
		this.tempRect.x = (float)x;
		this.tempRect.y = (float)y;
		base.transform.localPosition = this.tempLocation;
	}

	public void SetLocationY(float y)
	{
		this.tempLocation.x = base.transform.localPosition.x;
		this.tempLocation.y = -y;
		this.tempLocation.z = base.transform.localPosition.z;
		this.tempRect.x = base.transform.localPosition.x;
		this.tempRect.y = y;
		base.transform.localPosition = this.tempLocation;
	}

	public void SetLocationZ(float z)
	{
		this.tempLocation.x = base.transform.localPosition.x;
		this.tempLocation.y = base.transform.localPosition.y;
		this.tempLocation.z = z;
		this.tempRect.x = base.transform.localPosition.x;
		this.tempRect.y = -base.transform.localPosition.y;
		base.transform.localPosition = this.tempLocation;
	}

	public void MoveLocation(int x, int y)
	{
		this.SetLocation(this.GetLocation().x + (float)x, this.GetLocationY() + (float)y);
	}

	public void MoveLocation(float x, float y)
	{
		this.SetLocation(this.GetLocation().x + x, this.GetLocationY() + y);
	}

	public void SetLocationCheckRotate(float x, float y)
	{
		float z = base.transform.rotation.eulerAngles.z;
		if (z != 0f)
		{
			base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			base.transform.localPosition = this.tempLocation;
		}
		this.SetLocation(x, y);
		if (z != 0f)
		{
			this.Rotate(z);
		}
	}

	public Vector3 GetLocation()
	{
		return base.transform.localPosition;
	}

	public float GetLocationX()
	{
		return base.transform.localPosition.x;
	}

	public float GetLocationY()
	{
		return -base.transform.localPosition.y;
	}

	public override void SetSize(float width, float height)
	{
		base.SetSize(width, height);
	}

	public Vector2 GetSize()
	{
		return this.tempSize;
	}

	public void SetControlRect(float x, float y, float width, float height)
	{
		this.SetLocation(x, y, base.transform.localPosition.z);
		this.SetSize(width, height);
	}

	public Rect GetControlRect()
	{
		return this.tempRect;
	}

	public override void Start()
	{
		base.Start();
		if (UIManager.Exists())
		{
			if (this.nullCamera && NrTSingleton<UIManager>.Instance.uiCameras.Length > 0)
			{
				this.SetCamera(NrTSingleton<UIManager>.Instance.uiCameras[0].camera);
			}
			if (Application.isPlaying)
			{
				if (this.cancelDragEasing == EZAnimation.EASING_TYPE.Default)
				{
					this.cancelDragEasing = NrTSingleton<UIManager>.Instance.cancelDragEasing;
				}
				if (this.cancelDragDuration == -1f)
				{
					this.cancelDragDuration = NrTSingleton<UIManager>.Instance.cancelDragDuration;
				}
				if (float.IsNaN(this.dragOffset))
				{
					this.dragOffset = NrTSingleton<UIManager>.Instance.defaultDragOffset;
				}
			}
		}
		if (this.spriteText != null)
		{
			this.spriteText.Persistent = this.persistent;
			this.spriteText.Parent = this;
		}
		if (this.spriteTextShadow != null)
		{
			this.spriteTextShadow.Persistent = this.persistent;
			this.spriteTextShadow.Parent = this;
		}
	}

	public override void TruncateTop(float pct)
	{
		base.TruncateTop(pct);
		if (this.aggregateLayers != null)
		{
			for (int i = 0; i < this.aggregateLayers.Length; i++)
			{
				if (this.aggregateLayers[i] != null)
				{
					for (int j = 0; j < this.aggregateLayers[i].Length; j++)
					{
						this.aggregateLayers[i][j].TruncateTop(pct);
					}
				}
			}
		}
	}

	public override void TruncateBottom(float pct)
	{
		base.TruncateBottom(pct);
		if (this.aggregateLayers != null)
		{
			for (int i = 0; i < this.aggregateLayers.Length; i++)
			{
				if (this.aggregateLayers[i] != null)
				{
					for (int j = 0; j < this.aggregateLayers[i].Length; j++)
					{
						this.aggregateLayers[i][j].TruncateBottom(pct);
					}
				}
			}
		}
	}

	public override void TruncateLeft(float pct)
	{
		base.TruncateLeft(pct);
		if (this.aggregateLayers != null)
		{
			for (int i = 0; i < this.aggregateLayers.Length; i++)
			{
				if (this.aggregateLayers[i] != null)
				{
					for (int j = 0; j < this.aggregateLayers[i].Length; j++)
					{
						this.aggregateLayers[i][j].TruncateLeft(pct);
					}
				}
			}
		}
	}

	public override void TruncateRight(float pct)
	{
		base.TruncateRight(pct);
		if (this.aggregateLayers != null)
		{
			for (int i = 0; i < this.aggregateLayers.Length; i++)
			{
				if (this.aggregateLayers[i] != null)
				{
					for (int j = 0; j < this.aggregateLayers[i].Length; j++)
					{
						this.aggregateLayers[i][j].TruncateRight(pct);
					}
				}
			}
		}
	}

	public override void Untruncate()
	{
		base.Untruncate();
		if (this.aggregateLayers != null)
		{
			for (int i = 0; i < this.aggregateLayers.Length; i++)
			{
				if (this.aggregateLayers[i] != null)
				{
					for (int j = 0; j < this.aggregateLayers[i].Length; j++)
					{
						this.aggregateLayers[i][j].Untruncate();
					}
				}
			}
		}
	}

	public override void Unclip()
	{
		if (this.ignoreClipping)
		{
			return;
		}
		base.Unclip();
		if (this.spriteText != null)
		{
			this.spriteText.Unclip();
		}
		if (this.spriteTextShadow != null)
		{
			this.spriteTextShadow.Unclip();
		}
		if (this.aggregateLayers != null)
		{
			for (int i = 0; i < this.aggregateLayers.Length; i++)
			{
				if (this.aggregateLayers[i] != null)
				{
					for (int j = 0; j < this.aggregateLayers[i].Length; j++)
					{
						this.aggregateLayers[i][j].Unclip();
					}
				}
			}
		}
		this.UpdateCollider();
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
		if (this.spriteText != null)
		{
			this.spriteText.SetClipped(value);
		}
		if (this.spriteTextShadow != null)
		{
			this.spriteTextShadow.SetClipped(value);
		}
		if (this.aggregateLayers != null)
		{
			for (int i = 0; i < this.aggregateLayers.Length; i++)
			{
				if (this.aggregateLayers[i] != null)
				{
					for (int j = 0; j < this.aggregateLayers[i].Length; j++)
					{
						this.aggregateLayers[i][j].SetClipped(value);
					}
				}
			}
		}
		this.UpdateCollider();
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
		if (this.spriteText != null)
		{
			this.spriteText.SetClippingRect(value);
		}
		if (this.spriteTextShadow != null)
		{
			this.spriteTextShadow.SetClippingRect(value);
		}
		if (this.aggregateLayers != null)
		{
			for (int i = 0; i < this.aggregateLayers.Length; i++)
			{
				if (this.aggregateLayers[i] != null)
				{
					for (int j = 0; j < this.aggregateLayers[i].Length; j++)
					{
						this.aggregateLayers[i][j].SetClippingRect(value);
					}
				}
			}
		}
		this.UpdateCollider();
	}

	public override void SetCamera(Camera c)
	{
		base.SetCamera(c);
		if (this.pixelPerfect)
		{
			this.UpdateCollider();
		}
	}

	public override void Hide(bool tf)
	{
		if (!this.m_started)
		{
			this.Start();
		}
		if (!base.IsHidden() && tf)
		{
			if (base.collider is BoxCollider)
			{
				this.savedColliderSize = ((BoxCollider)base.collider).size;
				((BoxCollider)base.collider).size = Vector3.zero;
			}
		}
		else if (base.IsHidden() && !tf && base.collider is BoxCollider)
		{
			((BoxCollider)base.collider).size = this.savedColliderSize;
		}
		base.Hide(tf);
		if (this.aggregateLayers != null)
		{
			for (int i = 0; i < this.aggregateLayers.Length; i++)
			{
				if (this.aggregateLayers[i] != null)
				{
					for (int j = 0; j < this.aggregateLayers[i].Length; j++)
					{
						this.aggregateLayers[i][j].Hide(tf);
					}
				}
			}
		}
		if (this.spriteText != null)
		{
			this.spriteText.Hide(tf);
		}
		if (this.spriteTextShadow != null)
		{
			this.spriteTextShadow.Hide(tf);
		}
		if (!tf)
		{
			this.UpdateCollider();
		}
	}

	public void Copy(IControl c)
	{
		if (!(c is AutoSpriteControlBase))
		{
			return;
		}
		this.Copy((SpriteRoot)c);
	}

	public void Copy(IControl c, ControlCopyFlags flags)
	{
		if (!(c is AutoSpriteControlBase))
		{
			return;
		}
		this.Copy((SpriteRoot)c, flags);
	}

	public override void Copy(SpriteRoot s)
	{
		this.Copy(s, ControlCopyFlags.All);
	}

	public virtual void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		if ((flags & ControlCopyFlags.Appearance) == ControlCopyFlags.Appearance)
		{
			if (Application.isPlaying && s.Started)
			{
				base.Copy(s);
			}
			else
			{
				base.CopyAll(s);
			}
			if (!(s is AutoSpriteControlBase))
			{
				if (this.autoResize || this.pixelPerfect)
				{
					base.CalcSize();
				}
				else
				{
					this.SetSize(s.width, s.height);
				}
				base.SetBleedCompensation();
				return;
			}
		}
		AutoSpriteControlBase autoSpriteControlBase = (AutoSpriteControlBase)s;
		if ((flags & ControlCopyFlags.Transitions) == ControlCopyFlags.Transitions)
		{
			if (autoSpriteControlBase is UIStateToggleBtn || !Application.isPlaying)
			{
				if (autoSpriteControlBase.Transitions != null)
				{
					this.Transitions = new EZTransitionList[autoSpriteControlBase.Transitions.Length];
					for (int i = 0; i < this.Transitions.Length; i++)
					{
						this.Transitions[i] = new EZTransitionList();
						autoSpriteControlBase.Transitions[i].CopyToNew(this.Transitions[i], true);
					}
				}
			}
			else if (this.Transitions != null && autoSpriteControlBase.Transitions != null)
			{
				int num = 0;
				while (num < this.Transitions.Length && num < autoSpriteControlBase.Transitions.Length)
				{
					autoSpriteControlBase.Transitions[num].CopyTo(this.Transitions[num], true);
					num++;
				}
			}
		}
		if ((flags & ControlCopyFlags.Text) == ControlCopyFlags.Text)
		{
			if (this.spriteText == null && autoSpriteControlBase.spriteText != null)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(autoSpriteControlBase.spriteText.gameObject);
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = autoSpriteControlBase.spriteText.transform.localPosition;
				gameObject.transform.localScale = autoSpriteControlBase.spriteText.transform.localScale;
				gameObject.transform.localRotation = autoSpriteControlBase.spriteText.transform.localRotation;
			}
			if (this.spriteText != null)
			{
				this.spriteText.Copy(autoSpriteControlBase.spriteText);
			}
		}
		if ((flags & ControlCopyFlags.Data) == ControlCopyFlags.Data)
		{
			this.data = autoSpriteControlBase.data;
		}
		if ((flags & ControlCopyFlags.Appearance) == ControlCopyFlags.Appearance)
		{
			if (autoSpriteControlBase.collider != null)
			{
				if (base.collider.GetType() == autoSpriteControlBase.collider.GetType())
				{
					if (autoSpriteControlBase.collider is BoxCollider)
					{
						if (base.collider == null)
						{
							base.gameObject.AddComponent(typeof(BoxCollider));
						}
						BoxCollider boxCollider = (BoxCollider)base.collider;
						BoxCollider boxCollider2 = (BoxCollider)autoSpriteControlBase.collider;
						boxCollider.center = boxCollider2.center;
						boxCollider.size = boxCollider2.size;
					}
					else if (autoSpriteControlBase.collider is SphereCollider)
					{
						if (base.collider == null)
						{
							base.gameObject.AddComponent(typeof(SphereCollider));
						}
						SphereCollider sphereCollider = (SphereCollider)base.collider;
						SphereCollider sphereCollider2 = (SphereCollider)autoSpriteControlBase.collider;
						sphereCollider.center = sphereCollider2.center;
						sphereCollider.radius = sphereCollider2.radius;
					}
					else if (autoSpriteControlBase.collider is MeshCollider)
					{
						if (base.collider == null)
						{
							base.gameObject.AddComponent(typeof(MeshCollider));
						}
						MeshCollider meshCollider = (MeshCollider)base.collider;
						MeshCollider meshCollider2 = (MeshCollider)autoSpriteControlBase.collider;
						meshCollider.smoothSphereCollisions = meshCollider2.smoothSphereCollisions;
						meshCollider.convex = meshCollider2.convex;
						meshCollider.sharedMesh = meshCollider2.sharedMesh;
					}
					else if (autoSpriteControlBase.collider is CapsuleCollider)
					{
						if (base.collider == null)
						{
							base.gameObject.AddComponent(typeof(CapsuleCollider));
						}
						CapsuleCollider capsuleCollider = (CapsuleCollider)base.collider;
						CapsuleCollider capsuleCollider2 = (CapsuleCollider)autoSpriteControlBase.collider;
						capsuleCollider.center = capsuleCollider2.center;
						capsuleCollider.radius = capsuleCollider2.radius;
						capsuleCollider.height = capsuleCollider2.height;
						capsuleCollider.direction = capsuleCollider2.direction;
					}
					if (base.collider != null)
					{
						base.collider.isTrigger = autoSpriteControlBase.collider.isTrigger;
					}
				}
			}
			else if (Application.isPlaying)
			{
				if (base.collider == null && this.width != 0f && this.height != 0f && !float.IsNaN(this.width) && !float.IsNaN(this.height))
				{
					BoxCollider boxCollider3 = (BoxCollider)base.gameObject.AddComponent(typeof(BoxCollider));
					boxCollider3.size = new Vector3(autoSpriteControlBase.width, autoSpriteControlBase.height, 0.001f);
					boxCollider3.center = autoSpriteControlBase.GetCenterPoint();
					boxCollider3.isTrigger = true;
				}
				else if (base.collider is BoxCollider)
				{
					BoxCollider boxCollider4 = (BoxCollider)base.collider;
					boxCollider4.size = new Vector3(autoSpriteControlBase.width, autoSpriteControlBase.height, 0.001f);
					boxCollider4.center = autoSpriteControlBase.GetCenterPoint();
				}
				else if (base.collider is SphereCollider)
				{
					SphereCollider sphereCollider3 = (SphereCollider)base.collider;
					sphereCollider3.radius = Mathf.Max(autoSpriteControlBase.width, autoSpriteControlBase.height);
					sphereCollider3.center = autoSpriteControlBase.GetCenterPoint();
				}
			}
		}
		if ((flags & ControlCopyFlags.Invocation) == ControlCopyFlags.Invocation)
		{
			this.changeDelegate = autoSpriteControlBase.changeDelegate;
			this.inputDelegate = autoSpriteControlBase.inputDelegate;
		}
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State || (flags & ControlCopyFlags.Appearance) == ControlCopyFlags.Appearance)
		{
			this.Container = autoSpriteControlBase.Container;
			if (Application.isPlaying)
			{
				this.controlIsEnabled = autoSpriteControlBase.controlIsEnabled;
				this.Hide(autoSpriteControlBase.IsHidden());
			}
			if (this.curAnim != null)
			{
				if (this.curAnim.index == -1)
				{
					base.PlayAnim(this.curAnim);
				}
				else
				{
					this.SetState(this.curAnim.index);
				}
			}
			else
			{
				this.SetState(0);
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (this.dragDropHelper == null)
		{
			this.dragDropHelper = new EZDragDropHelper(this);
		}
		else
		{
			this.dragDropHelper.host = this;
		}
		if (base.collider != null)
		{
			this.customCollider = true;
		}
		this.Init();
		base.AddSpriteResizedDelegate(new SpriteRoot.SpriteResizedDelegate(this.OnResize));
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.managed && this.m_spriteMesh != null && this.m_hidden)
		{
			this.m_spriteMesh.Hide(true);
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.texMat)
		{
			if (null != this.texMat.mainTexture)
			{
				this.texMat.mainTexture = null;
			}
			UnityEngine.Object.Destroy(this.texMat);
			this.texMat = null;
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (Application.isPlaying)
		{
			if (EZAnimator.Exists() && this.autoAnimatorStop)
			{
				EZAnimator.instance.Stop(base.gameObject);
				EZAnimator.instance.Stop(this);
			}
			if (this.detargetOnDisable && UIManager.Exists())
			{
				NrTSingleton<UIManager>.Instance.Detarget(this);
			}
		}
	}

	protected void OnResize(float newWidth, float newHeight, SpriteRoot sprite)
	{
		this.UpdateCollider();
	}

	protected virtual void AddCollider()
	{
		if (this.customCollider || !Application.isPlaying || !this.m_started)
		{
			return;
		}
		BoxCollider boxCollider = (BoxCollider)base.gameObject.AddComponent(typeof(BoxCollider));
		if (base.IsHidden())
		{
			boxCollider.size = Vector3.zero;
		}
		else
		{
			this.UpdateCollider();
		}
	}

	public void ShowCollider(bool bShow)
	{
		if (base.collider is BoxCollider)
		{
			if (!bShow)
			{
				this.savedColliderSize = ((BoxCollider)base.collider).size;
				((BoxCollider)base.collider).size = Vector3.zero;
			}
			else
			{
				((BoxCollider)base.collider).size = this.savedColliderSize;
			}
		}
	}

	public virtual void UpdateCollider()
	{
		if (this.deleted || this.m_spriteMesh == null)
		{
			return;
		}
		if (!(base.collider is BoxCollider) || base.IsHidden() || this.m_spriteMesh == null || this.customCollider)
		{
			return;
		}
		Vector3[] v3TotalVertices = this.m_v3TotalVertices;
		Vector3 vector = v3TotalVertices[1];
		Vector3 a = v3TotalVertices[3];
		if (this.includeTextInAutoCollider && this.spriteText != null)
		{
			Matrix4x4 localToWorldMatrix = this.spriteText.transform.localToWorldMatrix;
			Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
			Vector3 vector2 = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.spriteText.TopLeft));
			Vector3 vector3 = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.spriteText.BottomRight));
			if (vector3.x - vector2.x > 0f && vector2.y - vector3.y > 0f)
			{
				vector.x = Mathf.Min(vector.x, vector2.x);
				vector.y = Mathf.Min(vector.y, vector3.y);
				a.x = Mathf.Max(a.x, vector3.x);
				a.y = Mathf.Max(a.y, vector2.y);
			}
		}
		BoxCollider boxCollider = (BoxCollider)base.collider;
		boxCollider.size = a - vector;
		boxCollider.center = vector + boxCollider.size * 0.5f;
		boxCollider.isTrigger = true;
	}

	public virtual void FindOuterEdges()
	{
		if (this.deleted)
		{
			return;
		}
		if (!this.m_started)
		{
			this.Start();
		}
		this.topLeftEdge = this.unclippedTopLeft;
		this.bottomRightEdge = this.unclippedBottomRight;
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		if (this.spriteText != null)
		{
			Matrix4x4 localToWorldMatrix = this.spriteText.transform.localToWorldMatrix;
			Vector3 vector = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.spriteText.UnclippedTopLeft));
			Vector3 vector2 = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.spriteText.UnclippedBottomRight));
			this.topLeftEdge.x = Mathf.Min(this.topLeftEdge.x, vector.x);
			this.topLeftEdge.y = Mathf.Max(this.topLeftEdge.y, vector.y);
			this.bottomRightEdge.x = Mathf.Max(this.bottomRightEdge.x, vector2.x);
			this.bottomRightEdge.y = Mathf.Min(this.bottomRightEdge.y, vector2.y);
		}
		if (this.aggregateLayers != null)
		{
			for (int i = 0; i < this.aggregateLayers.Length; i++)
			{
				for (int j = 0; j < this.aggregateLayers[i].Length; j++)
				{
					if (!this.aggregateLayers[i][j].IsHidden() && this.aggregateLayers[i][j].gameObject.activeInHierarchy)
					{
						Matrix4x4 localToWorldMatrix = this.aggregateLayers[i][j].transform.localToWorldMatrix;
						Vector3 vector = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.aggregateLayers[i][j].UnclippedTopLeft));
						Vector3 vector2 = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.aggregateLayers[i][j].UnclippedBottomRight));
						this.topLeftEdge.x = Mathf.Min(this.topLeftEdge.x, vector.x);
						this.topLeftEdge.y = Mathf.Max(this.topLeftEdge.y, vector.y);
						this.bottomRightEdge.x = Mathf.Max(this.bottomRightEdge.x, vector2.x);
						this.bottomRightEdge.y = Mathf.Min(this.bottomRightEdge.y, vector2.y);
					}
				}
			}
		}
	}

	public Vector2 TopLeftEdge()
	{
		return this.topLeftEdge;
	}

	public Vector2 BottomRightEdge()
	{
		return this.bottomRightEdge;
	}

	public bool RequestContainership(IUIContainer cont)
	{
		Transform parent = base.transform.parent;
		Transform transform = ((Component)cont).transform;
		while (parent != null)
		{
			if (parent == transform)
			{
				this.Container = cont;
				return true;
			}
			if (parent.gameObject.GetComponent("IUIContainer") != null)
			{
				return false;
			}
			parent = parent.parent;
		}
		return false;
	}

	public virtual bool GotFocus()
	{
		return false;
	}

	public bool IsFocus()
	{
		return (AutoSpriteControlBase)NrTSingleton<UIManager>.Instance.FocusObject == this;
	}

	public virtual void SetFocus()
	{
		NrTSingleton<UIManager>.Instance.FocusObject = this;
	}

	public void ClearFocus()
	{
		if ((AutoSpriteControlBase)NrTSingleton<UIManager>.Instance.FocusObject == this)
		{
			NrTSingleton<UIManager>.Instance.FocusObject = null;
		}
	}

	public virtual void SetInputDelegate(EZInputDelegate del)
	{
		this.inputDelegate = del;
	}

	public virtual void AddInputDelegate(EZInputDelegate del)
	{
		this.inputDelegate = (EZInputDelegate)Delegate.Combine(this.inputDelegate, del);
	}

	public virtual void RemoveInputDelegate(EZInputDelegate del)
	{
		this.inputDelegate = (EZInputDelegate)Delegate.Remove(this.inputDelegate, del);
	}

	public virtual void AddGameObjectDelegate(EZGameObjectDelegate del)
	{
		this.gameObjectDelegate = (EZGameObjectDelegate)Delegate.Combine(this.gameObjectDelegate, del);
	}

	public virtual void RemoveGameObjectDelegate(EZGameObjectDelegate del)
	{
		this.gameObjectDelegate = (EZGameObjectDelegate)Delegate.Remove(this.gameObjectDelegate, del);
	}

	public void ExcuteGameObjectDelegate(IUIObject control, GameObject obj)
	{
		if (this.gameObjectDelegate != null)
		{
			this.gameObjectDelegate(control, obj);
		}
	}

	public virtual void AddLoadCompleteDelegate(EZValueChangedDelegate del)
	{
		this.loadCompleteDelegate = (EZValueChangedDelegate)Delegate.Combine(this.loadCompleteDelegate, del);
	}

	public virtual void RemoveLoadCompleteDelegate(EZValueChangedDelegate del)
	{
		this.loadCompleteDelegate = (EZValueChangedDelegate)Delegate.Remove(this.loadCompleteDelegate, del);
	}

	public virtual bool IsHaveDelegate()
	{
		return this.changeDelegate != null || this.mouseOverDelegate != null || this.mouseOutDelegate != null || this.rightMouseDelegate != null || this.doubleClickDelegate != null || this.mouseDownDelegate != null;
	}

	public virtual void SetValueChangedDelegate(EZValueChangedDelegate del)
	{
		this.changeDelegate = del;
	}

	public virtual void AddValueChangedDelegate(EZValueChangedDelegate del)
	{
		this.changeDelegate = (EZValueChangedDelegate)Delegate.Combine(this.changeDelegate, del);
	}

	public virtual void RemoveValueChangedDelegate(EZValueChangedDelegate del)
	{
		this.changeDelegate = (EZValueChangedDelegate)Delegate.Remove(this.changeDelegate, del);
	}

	public virtual void SetMouseDownDelegate(EZValueChangedDelegate del)
	{
		this.mouseDownDelegate = del;
	}

	public virtual void AddMouseDownDelegate(EZValueChangedDelegate del)
	{
		this.mouseDownDelegate = (EZValueChangedDelegate)Delegate.Combine(this.mouseDownDelegate, del);
	}

	public virtual void RemoveMouseDownDelegate(EZValueChangedDelegate del)
	{
		this.mouseDownDelegate = (EZValueChangedDelegate)Delegate.Remove(this.mouseDownDelegate, del);
	}

	public virtual void SetMouseOverDelegate(EZValueChangedDelegate del)
	{
		this.mouseOverDelegate = del;
	}

	public virtual void AddMouseOverDelegate(EZValueChangedDelegate del)
	{
		this.mouseOverDelegate = (EZValueChangedDelegate)Delegate.Combine(this.mouseOverDelegate, del);
	}

	public virtual void RemoveMouseOverDelegate(EZValueChangedDelegate del)
	{
		this.mouseOverDelegate = (EZValueChangedDelegate)Delegate.Remove(this.mouseOverDelegate, del);
	}

	public virtual void SetMouseOutDelegate(EZValueChangedDelegate del)
	{
		this.mouseOutDelegate = del;
	}

	public virtual void AddMouseOutDelegate(EZValueChangedDelegate del)
	{
		this.mouseOutDelegate = (EZValueChangedDelegate)Delegate.Combine(this.mouseOutDelegate, del);
	}

	public virtual void RemoveMouseOutDelegate(EZValueChangedDelegate del)
	{
		this.mouseOutDelegate = (EZValueChangedDelegate)Delegate.Remove(this.mouseOutDelegate, del);
	}

	public virtual void SetRightMouseDelegate(EZValueChangedDelegate del)
	{
		this.rightMouseDelegate = del;
	}

	public virtual void AddRightMouseDelegate(EZValueChangedDelegate del)
	{
		this.rightMouseDelegate = (EZValueChangedDelegate)Delegate.Combine(this.rightMouseDelegate, del);
	}

	public virtual void RemoveRightMouseDelegate(EZValueChangedDelegate del)
	{
		this.rightMouseDelegate = (EZValueChangedDelegate)Delegate.Remove(this.rightMouseDelegate, del);
	}

	public virtual void SetDoubleClickDelegate(EZValueChangedDelegate del)
	{
		this.doubleClickDelegate = del;
	}

	public virtual void AddDoubleClickDelegate(EZValueChangedDelegate del)
	{
		this.doubleClickDelegate = (EZValueChangedDelegate)Delegate.Combine(this.doubleClickDelegate, del);
	}

	public virtual void RemoveDoubleClickDelegate(EZValueChangedDelegate del)
	{
		this.doubleClickDelegate = (EZValueChangedDelegate)Delegate.Remove(this.doubleClickDelegate, del);
	}

	public virtual void OnInput(POINTER_INFO ptr)
	{
		this.OnInput(ref ptr);
	}

	public virtual void OnInput(ref POINTER_INFO ptr)
	{
		if (this.Container != null)
		{
			ptr.callerIsControl = true;
			this.Container.OnInput(ptr);
			if (this.m_bShowToolTip)
			{
				switch (ptr.evt)
				{
				case POINTER_INFO.INPUT_EVENT.MOVE:
					if (TsPlatform.IsMobile && TsPlatform.IsEditor)
					{
						return;
					}
					if (!this.m_bSetToolTip)
					{
						MsgHandler.Handle("CloseToolTip", new object[0]);
						if (this.m_cItemSecond != null)
						{
							MsgHandler.Handle("DLG_ItemTooltipDlg_Second", new object[]
							{
								this.c_eWindowID,
								this.m_cItemSecond
							});
						}
						this.m_bSetToolTip = true;
					}
					break;
				case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
				case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
					MsgHandler.Handle("CloseToolTip", new object[0]);
					this.m_bSetToolTip = false;
					break;
				}
			}
		}
	}

	public bool IsDragging()
	{
		return this.dragDropHelper.IsDragging();
	}

	public void SetDragging(bool value)
	{
		this.dragDropHelper.SetDragging(value);
	}

	public bool DragUpdatePosition(POINTER_INFO ptr)
	{
		if (null == this)
		{
			return false;
		}
		this.dragDropHelper.DragUpdatePosition(ptr);
		return true;
	}

	public void CancelDrag()
	{
		if (null == this)
		{
			return;
		}
		this.dragDropHelper.CancelDrag();
	}

	public void OnEZDragDrop(EZDragDropParams parms)
	{
		this.dragDropHelper.OnEZDragDrop(parms);
	}

	public void AddDragDropDelegate(EZDragDropDelegate del)
	{
		this.dragDropHelper.AddDragDropDelegate(del);
	}

	public void RemoveDragDropDelegate(EZDragDropDelegate del)
	{
		this.dragDropHelper.RemoveDragDropDelegate(del);
	}

	public void SetDragDropDelegate(EZDragDropDelegate del)
	{
		this.dragDropHelper.SetDragDropDelegate(del);
	}

	public virtual int DrawPreStateSelectGUI(int selState, bool inspector)
	{
		return 0;
	}

	public virtual int DrawPostStateSelectGUI(int selState)
	{
		return 0;
	}

	public virtual void DrawPreTransitionUI(int selState, IGUIScriptSelector gui)
	{
	}

	public virtual string[] EnumStateElements()
	{
		string[] array = new string[this.States.Length];
		for (int i = 0; i < this.States.Length; i++)
		{
			array[i] = this.States[i].name;
		}
		return array;
	}

	public virtual EZTransitionList GetTransitions(int index)
	{
		return null;
	}

	public virtual string GetStateLabel(int index)
	{
		return null;
	}

	public virtual void SetStateLabel(int index, string label)
	{
	}

	public virtual ASCSEInfo GetStateElementInfo(int stateNum)
	{
		return new ASCSEInfo
		{
			stateObj = this.States[stateNum],
			transitions = this.GetTransitions(stateNum),
			stateLabel = this.GetStateLabel(stateNum)
		};
	}

	protected void UseStateLabel(int index)
	{
		string stateLabel = this.GetStateLabel(index);
		if (stateLabel == "[\"]")
		{
			return;
		}
		if (stateLabel == string.Empty && this.spriteText == null)
		{
			return;
		}
		this.Text = stateLabel;
	}

	public override void DoMirror()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.screenSize.x == 0f || this.screenSize.y == 0f)
		{
			this.Start();
		}
		if (this.mirror == null)
		{
			this.mirror = new AutoSpriteControlBaseMirror();
			this.mirror.Mirror(this);
		}
		this.mirror.Validate(this);
		if (this.mirror.DidChange(this))
		{
			this.Init();
			this.mirror.Mirror(this);
		}
	}

	public string SpriteColorStr()
	{
		string result = string.Empty;
		if (null == this.spriteText)
		{
			result = "RGBA{1,1,1,1}";
		}
		else
		{
			Color color = this.spriteText.color;
			result = string.Concat(new object[]
			{
				"RGBA{",
				color.r,
				",",
				color.g,
				",",
				color.b,
				",",
				color.a,
				"}"
			});
		}
		return result;
	}

	public void SetText(string _text)
	{
		this.Text = _text;
	}

	public void SetTextPos(Vector2 pos)
	{
		if (null != this.spriteText)
		{
			this.spriteText.gameObject.transform.localPosition = new Vector3(this.spriteText.gameObject.transform.localPosition.x + pos.y, this.spriteText.gameObject.transform.localPosition.y - pos.y, this.spriteText.gameObject.transform.localPosition.z);
		}
		if (null != this.spriteTextShadow)
		{
			this.spriteTextShadow.gameObject.transform.localPosition = new Vector3(this.spriteTextShadow.gameObject.transform.localPosition.x + pos.y, this.spriteTextShadow.gameObject.transform.localPosition.y - pos.y, this.spriteTextShadow.gameObject.transform.localPosition.z);
		}
	}

	public string GetText()
	{
		return this.Text;
	}

	public bool IsInverseMode(INVERSE_MODE eMode)
	{
		return eMode == this.m_eInverseMode;
	}

	public void Inverse(INVERSE_MODE eMode)
	{
		switch (eMode)
		{
		case INVERSE_MODE.LEFT_TO_RIGHT:
			this.InverseLeftToRight();
			break;
		case INVERSE_MODE.TOP_TO_BOTTOM:
			this.InverseTopToBottom();
			break;
		case INVERSE_MODE.TOPLEFT_TO_BOTTOMRIGHT:
			this.InverseTopLeftToBottomRight();
			break;
		}
		this.m_eInverseMode = eMode;
	}

	private void InverseLeftToRight()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
		Vector3 localPosition = base.transform.localPosition;
		if (0f < localScale.x)
		{
			localPosition.x -= this.width;
		}
		else
		{
			localPosition.x += this.width;
		}
		base.transform.localPosition = localPosition;
	}

	private void InverseTopToBottom()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.y *= -1f;
		base.transform.localScale = localScale;
		Vector3 localPosition = base.transform.localPosition;
		if (0f < localScale.y)
		{
			localPosition.y += this.height;
		}
		else
		{
			localPosition.y -= this.height;
		}
		base.transform.localPosition = localPosition;
	}

	private void InverseTopLeftToBottomRight()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		localScale.y *= -1f;
		base.transform.localScale = localScale;
		Vector3 localPosition = base.transform.localPosition;
		if (0f < localScale.x)
		{
			localPosition.x -= this.width;
		}
		else
		{
			localPosition.x += this.width;
		}
		if (0f < localScale.y)
		{
			localPosition.y += this.height;
		}
		else
		{
			localPosition.y -= this.height;
		}
		base.transform.localPosition = localPosition;
	}

	public void Rotate(float fAngle)
	{
		if (base.transform.parent)
		{
			this.m_v3ForRotate.x = base.transform.parent.position.x + this.tempLocation.x + this.width / 2f;
			this.m_v3ForRotate.y = base.transform.parent.position.y + this.tempLocation.y - this.height / 2f;
		}
		else
		{
			this.m_v3ForRotate.x = base.transform.position.x + this.width / 2f;
			this.m_v3ForRotate.y = base.transform.position.y - this.height / 2f;
		}
		base.transform.RotateAround(this.m_v3ForRotate, Vector3.forward, fAngle);
	}

	public void SetTexture(Texture pkImage)
	{
		if (TsPlatform.IsMobile)
		{
			this.SetTexture(pkImage, "Transparent/Vertex Colored_mobile");
		}
		else
		{
			this.SetTexture(pkImage, "Transparent/Vertex Colored");
		}
	}

	public void SetTexture(Texture pkImage, string strShaderName)
	{
		if (pkImage == null)
		{
			base.BackGroundHide(true);
		}
		else
		{
			if (null == this.texMat)
			{
				this.texMat = new Material(Shader.Find(strShaderName));
			}
			if (null == this.texMat)
			{
				base.BackGroundHide(true);
				return;
			}
			base.BackGroundHide(false);
			this.texMat.mainTexture = pkImage;
			base.SetSpriteTile(SpriteTile.SPRITE_TILE_MODE.STM_MIN, this.width, this.height);
			base.Setup(this.width, this.height, this.texMat);
			base.SetPixelToUV(pkImage);
			Rect uvRect = this.uvRect;
			if (!this.ChangeUVs)
			{
				uvRect = new Rect(0f, 0f, 1f, 1f);
			}
			else
			{
				uvRect = this.uvRect;
			}
			base.SetUVs(uvRect);
			this.kBaseInfo.StyleName = "AssetBundle_Image";
			if (this.loadCompleteDelegate != null)
			{
				this.loadCompleteDelegate(this);
			}
		}
	}

	public void DeleteMat()
	{
		if (null != this.texMat)
		{
			if (null != this.texMat.mainTexture)
			{
				this.texMat.mainTexture = null;
			}
			UnityEngine.Object.Destroy(this.texMat);
			this.texMat = null;
			base.BackGroundHide(true);
		}
	}

	public void SetTexture(string strUIKey)
	{
		if (this.kBaseInfo.StyleName == strUIKey)
		{
			return;
		}
		this.kBaseInfo.Initialize();
		if (!NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(strUIKey, ref this.kBaseInfo))
		{
			base.BackGroundHide(true);
			return;
		}
		base.BackGroundHide(false);
		base.SetSpriteTile(this.kBaseInfo.Tile, this.kBaseInfo.UVs.width, this.kBaseInfo.UVs.height);
		Material material = (Material)CResources.Load(this.kBaseInfo.Material);
		base.Setup(this.width, this.height, material);
		this.SetTextureUVs(new Vector2(this.kBaseInfo.UVs.x, this.kBaseInfo.UVs.y + this.kBaseInfo.UVs.height), new Vector2(this.kBaseInfo.UVs.width, this.kBaseInfo.UVs.height));
	}

	public void SetTexture(UIBaseInfoLoader baseInfo)
	{
		if (baseInfo == null)
		{
			base.BackGroundHide(true);
			return;
		}
		base.BackGroundHide(false);
		base.SetSpriteTile(baseInfo.Tile, baseInfo.UVs.width, baseInfo.UVs.height);
		Material material = (Material)CResources.Load(baseInfo.Material);
		base.Setup(this.width, this.height, material);
		this.SetTextureUVs(new Vector2(baseInfo.UVs.x, baseInfo.UVs.y + baseInfo.UVs.height), new Vector2(baseInfo.UVs.width, baseInfo.UVs.height));
		this.kBaseInfo.StyleName = baseInfo.StyleName;
	}

	public void SetTextureKey(string key)
	{
		this.SetTexture(key);
	}

	public void SetUVMask(Rect uv)
	{
		this.ChangeUVs = true;
		uv.y = Mathf.Clamp(1f - uv.height, 0f, 1f);
		base.SetUVs(uv);
	}

	public void SetTextureUVs(Vector2 lowerLeftPixel, Vector2 pixelDimensions)
	{
		if (NrTSingleton<UIDataManager>.Instance.LowImage)
		{
			lowerLeftPixel /= 2f;
			pixelDimensions /= 2f;
		}
		Rect uVs = new Rect(0f, 0f, 0f, 0f);
		Vector2 vector = base.PixelCoordToUVCoord(lowerLeftPixel);
		uVs.x = vector.x;
		uVs.y = vector.y;
		vector = base.PixelSpaceToUVSpace(pixelDimensions);
		uVs.xMax = uVs.x + vector.x;
		uVs.yMax = uVs.y + vector.y;
		base.SetUVs(uVs);
	}

	public void SetUseBoxCollider(bool bUse)
	{
		this.customCollider = !bUse;
	}

	public void SetAlphaBG(float _alpha)
	{
		if (_alpha < 0f)
		{
			_alpha = 0f;
		}
		else if (_alpha > 1f)
		{
			_alpha = 1f;
		}
		Color color = this.color;
		color.a = _alpha;
		this.SetColor(color);
		if (TsPlatform.IsWeb && base.renderer.material.HasProperty("_ColorWEB"))
		{
			base.renderer.material.SetColor("_ColorWEB", color);
		}
	}

	public void SetMatColor(Color c)
	{
		if (null != this.texMat && this.texMat.HasProperty("_Color"))
		{
			this.texMat.SetColor("_Color", c);
		}
	}

	public void SetAlphaText(float _alpha)
	{
		if (_alpha < 0f)
		{
			_alpha = 0f;
		}
		else if (_alpha > 1f)
		{
			_alpha = 1f;
		}
		if (this.spriteText != null)
		{
			Color color = this.spriteText.color;
			color.a = _alpha;
			if (_alpha > 0.1f)
			{
				this.spriteText.SetColor(color);
				if (!this.spriteText.IsHidden())
				{
					this.spriteText.Hide(false);
				}
			}
			else
			{
				this.spriteText.Hide(true);
			}
		}
	}

	public void SetAlpha(float _alpha)
	{
		if (_alpha < 0f)
		{
			_alpha = 0f;
		}
		else if (_alpha > 1f)
		{
			_alpha = 1f;
		}
		Color color = this.color;
		color.a *= _alpha;
		this.SetColor(color);
		if (TsPlatform.IsWeb && base.renderer.material.HasProperty("_ColorWEB"))
		{
			base.renderer.material.SetColor("_ColorWEB", color);
		}
		if (this.spriteText != null)
		{
			color = this.spriteText.color;
			color.a = _alpha;
			if (_alpha > 0.1f)
			{
				this.spriteText.SetColor(color);
				Color color2 = this.spriteTextShadow.color;
				color2.a = _alpha;
				this.spriteTextShadow.SetColor(color2);
				if (TsPlatform.IsWeb && this.spriteText.renderer.material.HasProperty("_Alpha"))
				{
					float value = _alpha;
					this.spriteText.renderer.material.SetFloat("_Alpha", value);
				}
				if (!this.spriteText.IsHidden())
				{
					this.spriteText.Hide(false);
				}
				if (!this.spriteTextShadow.IsHidden())
				{
					this.spriteTextShadow.Hide(false);
				}
			}
			else
			{
				this.spriteText.Hide(true);
				this.spriteTextShadow.Hide(true);
			}
		}
	}

	public void SetMultiLine(bool flag)
	{
		if (this.spriteText)
		{
			this.spriteText.multiline = flag;
			if (this.spriteText.multiline)
			{
				this.spriteText.maxWidth = this.width;
			}
			else
			{
				this.spriteText.maxWidth = 0f;
			}
			this.spriteTextShadow.multiline = flag;
			if (this.spriteTextShadow.multiline)
			{
				this.spriteTextShadow.maxWidth = this.width;
			}
			else
			{
				this.spriteTextShadow.maxWidth = 0f;
			}
		}
		else
		{
			this.Text = " ";
			this.spriteText.multiline = flag;
			if (this.spriteText.multiline)
			{
				this.spriteText.maxWidth = this.width;
			}
			else
			{
				this.spriteText.maxWidth = 0f;
			}
			this.spriteTextShadow.multiline = flag;
			if (this.spriteTextShadow.multiline)
			{
				this.spriteTextShadow.maxWidth = this.width;
			}
			else
			{
				this.spriteTextShadow.maxWidth = 0f;
			}
		}
	}

	protected void EffectAniStartDelegate(EZAnimation anim)
	{
		if (this == null || base.transform == null)
		{
			return;
		}
		this.bStartAni = true;
		this.bStartScale = base.transform.localScale;
	}

	protected void EffectAniCompletionDelegate(EZAnimation anim)
	{
		if (this == null || base.transform == null)
		{
			return;
		}
		this.bStartAni = false;
		base.transform.localScale = this.bStartScale;
	}

	public void DeleteChildEffect()
	{
		if (this == null || base.transform == null)
		{
			return;
		}
		Transform transform = base.transform.FindChild(NrTSingleton<UIDataManager>.Instance.AttachEffectKeyName);
		if (null != transform)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	virtual GameObject get_gameObject()
	{
		return base.gameObject;
	}

	virtual Transform get_transform()
	{
		return base.transform;
	}

	virtual string get_name()
	{
		return base.name;
	}
}
