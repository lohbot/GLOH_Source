using System;
using UnityEngine;
using UnityForms;

[AddComponentMenu("EZ GUI/Controls/Text Field")]
public class UITextField : AutoSpriteControlBase, IKeyFocusable
{
	public delegate void FocusDelegate(UITextField field);

	public delegate void FocusLostDelegate(UITextField field);

	[HideInInspector]
	public TextureAnim[] states = new TextureAnim[]
	{
		new TextureAnim("Field graphic"),
		new TextureAnim("Caret")
	};

	[HideInInspector]
	public EZTransitionList[] transitions = new EZTransitionList[]
	{
		null,
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("Caret Flash")
		})
	};

	public Vector2 margins;

	protected Rect3D clientClippingRect;

	protected Vector2 marginTopLeft;

	protected Vector2 marginBottomRight;

	public int maxLength;

	public bool multiline;

	public bool password;

	private bool numberMode;

	private long currentValue;

	private long maxValue = 100000000L;

	private long minValue;

	private int maxLineCount;

	public int InsetMaxNum;

	public string maskingCharacter = "*";

	public Vector2 caretSize;

	public SpriteRoot.ANCHOR_METHOD caretAnchor = SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT;

	public Vector3 caretOffset = new Vector3(0f, 0f, -0.1f);

	public bool showCaretOnMobile;

	private string defaultText = string.Empty;

	public TsPlatform.TouchScreenKeyboardType type;

	public bool autoCorrect;

	public bool secure;

	public bool alert;

	public MonoBehaviour scriptWithMethodToInvoke;

	public string methodToInvoke = string.Empty;

	protected EZKeyboardCommitDelegate commitDelegate;

	public AudioSource typingSoundEffect;

	public AudioSource fieldFullSound;

	public bool customKeyboard;

	public POINTER_INFO.INPUT_EVENT customFocusEvent = POINTER_INFO.INPUT_EVENT.PRESS;

	protected AutoSprite caret;

	protected float caretblinkTime;

	protected UITextField.FocusDelegate focusDelegate;

	protected UITextField.FocusLostDelegate focusLostDelegate;

	protected int insert;

	protected Vector3 cachedPos;

	protected Quaternion cachedRot;

	protected Vector3 cachedScale;

	protected bool hasFocus;

	protected Vector3 origTextPos;

	protected string originalContent = string.Empty;

	protected bool enterMode;

	protected int[,] stateIndices;

	public override bool controlIsEnabled
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

	public bool NumberMode
	{
		get
		{
			return this.numberMode;
		}
		set
		{
			this.numberMode = value;
		}
	}

	public long CurrentValue
	{
		get
		{
			return this.currentValue;
		}
	}

	public long MaxValue
	{
		get
		{
			return this.maxValue;
		}
		set
		{
			this.maxValue = value;
		}
	}

	public long MinValue
	{
		get
		{
			return this.minValue;
		}
		set
		{
			this.minValue = value;
		}
	}

	public int MaxLineCount
	{
		get
		{
			return this.maxLineCount;
		}
		set
		{
			this.maxLineCount = value;
		}
	}

	public string Content
	{
		get
		{
			return this.Text;
		}
	}

	public string OriginalContent
	{
		get
		{
			return this.originalContent;
		}
		set
		{
			this.originalContent = value;
		}
	}

	public bool EnterMode
	{
		get
		{
			return this.enterMode;
		}
		set
		{
			this.enterMode = value;
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
				if (this.container != null && this.caret != null)
				{
					this.container.RemoveChild(this.caret.gameObject);
				}
				if (value != null && this.caret != null)
				{
					value.AddChild(this.caret.gameObject);
				}
			}
			base.Container = value;
		}
	}

	public override string Text
	{
		get
		{
			if (this.spriteText != null)
			{
				return this.spriteText.Text;
			}
			return base.Text;
		}
		set
		{
			bool flag = this.spriteText == null;
			if (Application.isPlaying && !this.m_started)
			{
				this.Start();
			}
			if (this.numberMode)
			{
				if (value.Length > 0)
				{
					long num = 0L;
					if (!long.TryParse(value, out num))
					{
						num = 0L;
					}
					if (this.MaxValue < num)
					{
						num = this.MaxValue;
					}
					else if (this.MinValue > num)
					{
						num = this.MinValue;
					}
					this.currentValue = num;
					base.Text = ANNUALIZED.Convert(num);
				}
				else
				{
					base.Text = "0";
				}
			}
			else
			{
				base.Text = value;
			}
			if (flag && this.spriteText != null)
			{
				this.spriteText.transform.localPosition = new Vector4(this.margins.x, this.margins.y);
				this.spriteText.removeUnsupportedCharacters = true;
				this.spriteText.parseColorTags = false;
				this.spriteText.multiline = this.multiline;
			}
			if (this.cachedPos != base.transform.position || this.cachedRot != base.transform.rotation || this.cachedScale != base.transform.lossyScale)
			{
				this.cachedPos = base.transform.position;
				this.cachedRot = base.transform.rotation;
				this.cachedScale = base.transform.lossyScale;
				this.CalcClippingRect();
			}
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

	public override void OnDestroy()
	{
		NrTSingleton<UIManager>.Instance.CloseKeyboard();
		base.OnDestroy();
	}

	public override void OnInput(ref POINTER_INFO ptr)
	{
		if (this.deleted)
		{
			return;
		}
		if (!this.m_controlIsEnabled || base.IsHidden())
		{
			base.OnInput(ref ptr);
			return;
		}
		if (this.inputDelegate != null)
		{
			this.inputDelegate(ref ptr);
		}
		if (!this.m_controlIsEnabled || base.IsHidden())
		{
			base.OnInput(ref ptr);
			return;
		}
		if (ptr.evt == this.customFocusEvent)
		{
			Color color = this.color;
			color.a = 1f;
			this.SetColor(color);
			if (this.focusDelegate != null)
			{
				this.focusDelegate(this);
			}
		}
		if (ptr.evt == POINTER_INFO.INPUT_EVENT.PRESS)
		{
			this.PositionInsertionPoint(ptr.hitInfo.point);
		}
		base.OnInput(ref ptr);
	}

	public override void Copy(SpriteRoot s)
	{
		this.Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);
		if (!(s is UITextField))
		{
			return;
		}
		UITextField uITextField = (UITextField)s;
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.maxLength = uITextField.maxLength;
			if (TsPlatform.IsMobile)
			{
				this.type = uITextField.type;
				this.autoCorrect = uITextField.autoCorrect;
				this.secure = uITextField.secure;
				this.alert = uITextField.alert;
			}
			this.typingSoundEffect = uITextField.typingSoundEffect;
			this.fieldFullSound = uITextField.fieldFullSound;
		}
		if ((flags & ControlCopyFlags.Appearance) == ControlCopyFlags.Appearance)
		{
			this.caret.Copy(uITextField.caret);
		}
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			this.insert = uITextField.insert;
			this.Text = uITextField.Text;
		}
	}

	public override bool GotFocus()
	{
		if (this.customKeyboard)
		{
			return false;
		}
		this.hasFocus = this.m_controlIsEnabled;
		return this.m_controlIsEnabled;
	}

	public override void SetFocus()
	{
		this.insert = this.Text.Length;
		base.SetFocus();
	}

	public int GetInsertPos()
	{
		return this.insert;
	}

	public string GetInputText(ref KEYBOARD_INFO info)
	{
		info.insert = this.insert;
		info.maxLength = this.maxLength;
		if (TsPlatform.IsMobile)
		{
			info.type = this.type;
			info.autoCorrect = this.autoCorrect;
			info.multiline = this.multiline;
			info.secure = this.password;
			info.alert = this.alert;
		}
		this.ShowCaret();
		return this.text;
	}

	private bool CheckNumberString()
	{
		if (!this.numberMode)
		{
			return true;
		}
		if (NkInputManager.GetKeyDown(KeyCode.Delete) || NkInputManager.GetKeyDown(KeyCode.Backspace))
		{
			return true;
		}
		for (int i = 0; i < 10; i++)
		{
			if (NkInputManager.GetKeyDown(KeyCode.Alpha0 + i) || NkInputManager.GetKeyDown(KeyCode.Keypad0 + i))
			{
				return true;
			}
		}
		return false;
	}

	public string SetInputText(string inputText, ref int insertPt)
	{
		this.SetInputTextNoComposition(inputText, ref insertPt);
		if (this.caret != null && this.caret.IsHidden() && this.hasFocus)
		{
			this.caret.Hide(false);
		}
		return this.text;
	}

	public string SetInputTextNoComposition(string inputText, ref int insertPt)
	{
		if (!this.multiline)
		{
			int startIndex;
			if ((startIndex = inputText.IndexOf('\n')) != -1)
			{
				inputText = inputText.Remove(startIndex, 1);
				NrTSingleton<UIManager>.Instance.FocusObject = null;
			}
			while ((startIndex = inputText.IndexOf('\r')) != -1)
			{
				inputText = inputText.Remove(startIndex, 1);
				NrTSingleton<UIManager>.Instance.FocusObject = null;
			}
		}
		if (inputText.Length >= this.maxLength && this.maxLength > 0 && NrTSingleton<UIManager>.Instance.FocusObject != null)
		{
			NrTSingleton<UIManager>.Instance.InsertionPoint -= inputText.Length - this.maxLength;
			return this.text;
		}
		this.Text = inputText;
		this.insert = insertPt;
		if (this.insert > inputText.Length)
		{
			this.insert = inputText.Length;
		}
		if (this.typingSoundEffect != null)
		{
			this.typingSoundEffect.PlayOneShot(this.typingSoundEffect.clip);
		}
		if (this.changeDelegate != null)
		{
			this.changeDelegate(this);
		}
		if (this.caret != null)
		{
			this.PositionCaret();
		}
		if (NrTSingleton<UIManager>.Instance.FocusObject == null)
		{
			this.Commit();
		}
		return this.text;
	}

	public void LostFocus()
	{
		this.hasFocus = false;
		this.HideCaret();
		if (this.focusLostDelegate != null)
		{
			this.focusLostDelegate(this);
		}
	}

	public void Commit()
	{
		if (this.scriptWithMethodToInvoke != null && !string.IsNullOrEmpty(this.methodToInvoke))
		{
			this.scriptWithMethodToInvoke.Invoke(this.methodToInvoke, 0f);
		}
		if (this.commitDelegate != null)
		{
			this.commitDelegate(this);
		}
	}

	protected void ShowCaret()
	{
		if (this.caret == null)
		{
			return;
		}
		this.CalcClippingRect();
		this.caret.Hide(false);
		this.PositionCaret();
		if (!this.caret.IsHidden())
		{
			this.transitions[1].list[0].Start();
		}
	}

	public override void Hide(bool tf)
	{
		base.Hide(tf);
		if (this.caret != null)
		{
			if (!tf && this.hasFocus)
			{
				this.caret.Hide(tf);
			}
			else
			{
				this.caret.Hide(true);
			}
		}
		if (!tf)
		{
			this.CalcClippingRect();
		}
	}

	protected void HideCaret()
	{
		if (this.caret == null)
		{
			return;
		}
		this.transitions[1].list[0].StopSafe();
		this.caret.Hide(true);
	}

	protected void PositionCaret()
	{
		this.PositionCaret(true);
	}

	protected void PositionCaret(bool recur)
	{
		if (this.caret == null || this.spriteText == null)
		{
			return;
		}
		Vector3 vector = Vector3.zero;
		vector = base.transform.InverseTransformPoint(this.spriteText.GetInsertionPointPos(this.spriteText.PlainIndexToDisplayIndex(this.insert)));
		Vector3 vector2 = vector + Vector3.up * this.spriteText.BaseHeight * this.spriteText.transform.localScale.y;
		if (recur)
		{
			if (this.multiline)
			{
				if (vector2.y > this.marginTopLeft.y)
				{
					this.spriteText.transform.localPosition -= Vector3.up * this.spriteText.LineSpan;
					this.PositionCaret(false);
					this.spriteText.SetClippingRect(this.clientClippingRect);
					this.CallSliderChangeDelegate(true);
					return;
				}
				if (vector.y < this.marginBottomRight.y)
				{
					this.spriteText.transform.localPosition += Vector3.up * this.spriteText.LineSpan;
					this.PositionCaret(false);
					this.spriteText.SetClippingRect(this.clientClippingRect);
					this.CallSliderChangeDelegate(false);
					return;
				}
			}
			else
			{
				if (vector.x < this.marginTopLeft.x)
				{
					Vector3 centerPoint = base.GetCenterPoint();
					Vector3 localPosition = this.spriteText.transform.localPosition + Vector3.right * Mathf.Abs(centerPoint.x - vector.x);
					localPosition.x = Mathf.Min(localPosition.x, this.origTextPos.x);
					this.spriteText.transform.localPosition = localPosition;
					this.PositionCaret(false);
					this.spriteText.SetClippingRect(this.clientClippingRect);
					return;
				}
				if (vector.x > this.marginBottomRight.x)
				{
					Vector3 centerPoint2 = base.GetCenterPoint();
					Vector3 localPosition2 = this.spriteText.transform.localPosition - Vector3.right * Mathf.Abs(centerPoint2.x - vector.x);
					this.spriteText.transform.localPosition = localPosition2;
					this.PositionCaret(false);
					this.spriteText.SetClippingRect(this.clientClippingRect);
					return;
				}
			}
		}
		vector.y += this.margins.y + 1f;
		this.transitions[1].list[0].StopSafe();
		this.caret.transform.localPosition = vector;
		this.transitions[1].list[0].Start();
		this.caret.SetClippingRect(this.clientClippingRect);
	}

	public void ToggleCaretShow()
	{
		if (!this.hasFocus)
		{
			if (base.IsFocus())
			{
				this.GotFocus();
			}
			return;
		}
		float time = Time.time;
		if ((double)(time - this.caretblinkTime) > 0.5)
		{
			if (!this.caret.IsHidden())
			{
				this.HideCaret();
			}
			else
			{
				this.ShowCaret();
			}
			this.caretblinkTime = time;
		}
	}

	protected void PositionInsertionPoint(Vector3 pt)
	{
		if (this.caret == null || this.spriteText == null)
		{
			return;
		}
		this.insert = this.spriteText.DisplayIndexToPlainIndex(this.spriteText.GetNearestInsertionPoint(pt));
		NrTSingleton<UIManager>.Instance.InsertionPoint = this.insert;
		this.PositionCaret(true);
	}

	public void SetCommitDelegate(EZKeyboardCommitDelegate del)
	{
		this.commitDelegate = del;
	}

	public void AddCommitDelegate(EZKeyboardCommitDelegate del)
	{
		this.commitDelegate = (EZKeyboardCommitDelegate)Delegate.Combine(this.commitDelegate, del);
	}

	public void RemoveCommitDelegate(EZKeyboardCommitDelegate del)
	{
		this.commitDelegate = (EZKeyboardCommitDelegate)Delegate.Remove(this.commitDelegate, del);
	}

	public void SetFocusDelegate(UITextField.FocusDelegate del)
	{
		this.focusDelegate = del;
	}

	public void AddFocusDelegate(UITextField.FocusDelegate del)
	{
		this.focusDelegate = (UITextField.FocusDelegate)Delegate.Combine(this.focusDelegate, del);
	}

	public void RemoveFocusDelegate(UITextField.FocusDelegate del)
	{
		this.focusDelegate = (UITextField.FocusDelegate)Delegate.Remove(this.focusDelegate, del);
	}

	public void SetFocusLostDelegate(UITextField.FocusLostDelegate del)
	{
		this.focusLostDelegate = del;
	}

	public void AddFocusLostDelegate(UITextField.FocusLostDelegate del)
	{
		this.focusLostDelegate = (UITextField.FocusLostDelegate)Delegate.Combine(this.focusLostDelegate, del);
	}

	public void RemoveFocusLostDelegate(UITextField.FocusLostDelegate del)
	{
		this.focusLostDelegate = (UITextField.FocusLostDelegate)Delegate.Remove(this.focusLostDelegate, del);
	}

	protected override void Awake()
	{
		base.Awake();
		this.defaultTextAlignment = SpriteText.Alignment_Type.Left;
		this.defaultTextAnchor = SpriteText.Anchor_Pos.Upper_Left;
	}

	public override void Start()
	{
		if (this.m_started)
		{
			return;
		}
		base.Start();
		if (this.spriteText == null)
		{
			this.Text = string.Empty;
		}
		if (this.spriteText != null)
		{
			this.spriteText.password = this.password;
			this.spriteText.maskingCharacter = this.maskingCharacter;
			this.spriteText.multiline = this.multiline;
			this.origTextPos = this.spriteText.transform.localPosition;
			this.SetMargins(this.margins);
		}
		this.insert = this.Text.Length;
		if (Application.isPlaying)
		{
			if (base.collider == null)
			{
				this.AddCollider();
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				if (this.showCaretOnMobile)
				{
					this.CreateCaret();
				}
			}
			else
			{
				this.CreateCaret();
			}
		}
		this.cachedPos = base.transform.position;
		this.cachedRot = base.transform.rotation;
		this.cachedScale = base.transform.lossyScale;
		this.CalcClippingRect();
		if (this.managed && this.m_hidden)
		{
			this.Hide(true);
		}
		Color color = this.color;
		color.a = 0.8f;
		this.SetColor(color);
	}

	protected void CreateCaret()
	{
		this.caret = (AutoSprite)new GameObject
		{
			name = base.name + "_caret",
			transform = 
			{
				parent = base.transform,
				localPosition = Vector3.zero,
				localRotation = Quaternion.identity,
				localScale = Vector3.one
			},
			layer = GUICamera.UILayer
		}.AddComponent(typeof(AutoSprite));
		this.caret.plane = this.plane;
		this.caret.offset = this.caretOffset;
		this.caret.SetAnchor(this.caretAnchor);
		this.caret.persistent = this.persistent;
		if (!this.managed)
		{
			if (this.caret.spriteMesh != null)
			{
				Material material = CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Material/Default White Material" + NrTSingleton<UIDataManager>.Instance.AddFilePath) as Material;
				((SpriteMesh)this.caret.spriteMesh).material = material;
				((SpriteMesh)this.caret.spriteMesh).UpdateColors(this.spriteText.color);
				this.caret.SetUVs(new Rect(0f, 0f, 1f, 1f));
				this.caret.SetDrawLayer(this.drawLayer + 1);
			}
		}
		else if (this.manager != null)
		{
			this.caret.Managed = this.managed;
			this.manager.AddSprite(this.caret);
			this.caret.SetDrawLayer(this.drawLayer + 1);
			((SpriteMesh)this.caret.spriteMesh).UpdateColors(this.spriteText.color);
		}
		else
		{
			TsLog.LogError("Sprite on object \"" + base.name + "\" not assigned to a SpriteManager!", new object[0]);
		}
		this.caret.autoResize = this.autoResize;
		if (this.pixelPerfect)
		{
			this.caret.pixelPerfect = this.pixelPerfect;
		}
		else
		{
			this.caretSize.x = 3f;
			if (TsPlatform.IsWeb)
			{
				this.caretSize.y = this.height * 0.7f;
			}
			else
			{
				this.caretSize.y = this.spriteText.BaseHeight;
			}
			this.caret.SetSize(this.caretSize.x, this.caretSize.y);
		}
		if (this.states[1].spriteFrames.Length != 0)
		{
			this.caret.animations = new UVAnimation[1];
			this.caret.animations[0] = new UVAnimation();
			this.caret.animations[0].SetAnim(this.states[1], 0);
			this.caret.PlayAnim(0, 0);
		}
		this.caret.renderCamera = this.renderCamera;
		this.caret.SetCamera(this.renderCamera);
		this.caret.Hide(true);
		this.transitions[1].list[0].MainSubject = this.caret.gameObject;
		this.PositionCaret();
		if (this.container != null)
		{
			this.container.AddSubject(this.caret.gameObject);
		}
		this.caretblinkTime = Time.time;
	}

	public void CalcClippingRect()
	{
		if (this.spriteText == null)
		{
			return;
		}
		Vector3 vector = this.marginTopLeft;
		Vector3 vector2 = this.marginBottomRight;
		if (this.clipped)
		{
			Vector3 vector3 = vector;
			Vector3 vector4 = vector2;
			vector.x = Mathf.Clamp(this.localClipRect.x, vector3.x, vector4.x);
			vector2.x = Mathf.Clamp(this.localClipRect.xMax, vector3.x, vector4.x);
			vector.y = Mathf.Clamp(this.localClipRect.yMax, vector4.y, vector3.y);
			vector2.y = Mathf.Clamp(this.localClipRect.y, vector4.y, vector3.y);
		}
		this.clientClippingRect.FromRect(Rect.MinMaxRect(vector.x, vector2.y, vector2.x, vector.y));
		this.clientClippingRect.MultFast(base.transform.localToWorldMatrix);
		this.spriteText.SetClippingRect(this.clientClippingRect);
		if (this.caret != null)
		{
			this.caret.SetClippingRect(this.clientClippingRect);
		}
	}

	public void SetMargins(Vector2 marg)
	{
		this.margins = marg;
		Vector3 centerPoint = base.GetCenterPoint();
		this.marginTopLeft = new Vector3(centerPoint.x + this.margins.x - this.width * 0.5f, centerPoint.y + this.margins.y / 2f + this.height * 0.5f);
		this.marginBottomRight = new Vector3(centerPoint.x - this.margins.x + this.width * 0.5f, centerPoint.y - this.margins.y - this.height * 0.5f);
		if (this.multiline)
		{
			float num = 0f;
			switch (this.spriteText.anchor)
			{
			case SpriteText.Anchor_Pos.Upper_Left:
			case SpriteText.Anchor_Pos.Middle_Left:
			case SpriteText.Anchor_Pos.Lower_Left:
				num = this.marginBottomRight.x - this.origTextPos.x;
				break;
			case SpriteText.Anchor_Pos.Upper_Center:
			case SpriteText.Anchor_Pos.Middle_Center:
			case SpriteText.Anchor_Pos.Lower_Center:
				num = (this.marginBottomRight.x - this.marginTopLeft.x) * 2f - 2f * Mathf.Abs(this.origTextPos.x);
				break;
			case SpriteText.Anchor_Pos.Upper_Right:
			case SpriteText.Anchor_Pos.Middle_Right:
			case SpriteText.Anchor_Pos.Lower_Right:
				num = this.origTextPos.x - this.marginTopLeft.x;
				break;
			}
			this.spriteText.maxWidth = 1f / this.spriteText.transform.localScale.x * num;
		}
		else
		{
			this.spriteText.maxWidth = 0f;
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

	public static UITextField Create(string name, Vector3 pos)
	{
		return (UITextField)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UITextField));
	}

	public static UITextField Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UITextField)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UITextField));
	}

	public override void Unclip()
	{
		if (this.ignoreClipping)
		{
			return;
		}
		base.Unclip();
		this.CalcClippingRect();
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
		this.CalcClippingRect();
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
		this.CalcClippingRect();
	}

	public override void DrawPreTransitionUI(int selState, IGUIScriptSelector gui)
	{
		this.scriptWithMethodToInvoke = gui.DrawScriptSelection(this.scriptWithMethodToInvoke, ref this.methodToInvoke);
	}

	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = new Color(1f, 0f, 0.5f, 1f);
		Gizmos.DrawLine(this.clientClippingRect.topLeft, this.clientClippingRect.bottomLeft);
		Gizmos.DrawLine(this.clientClippingRect.bottomLeft, this.clientClippingRect.bottomRight);
		Gizmos.DrawLine(this.clientClippingRect.bottomRight, this.clientClippingRect.topRight);
		Gizmos.DrawLine(this.clientClippingRect.topRight, this.clientClippingRect.topLeft);
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
			this.mirror = new UITextFieldMirror();
			this.mirror.Mirror(this);
		}
		this.mirror.Validate(this);
		if (this.mirror.DidChange(this))
		{
			this.Init();
			this.mirror.Mirror(this);
		}
	}

	public void ClearDefaultText(UITextField textfield)
	{
		if (textfield && textfield.spriteText)
		{
			textfield.spriteText.color = Color.white;
		}
		if (this.Text == this.defaultText)
		{
			this.ClearText();
		}
		this.defaultText = string.Empty;
		this.RemoveFocusDelegate(new UITextField.FocusDelegate(this.ClearDefaultText));
	}

	public void SetDefaultText(string text)
	{
		if (text.Length > 0)
		{
			this.Text = text;
			this.defaultText = this.Text;
			this.AddFocusDelegate(new UITextField.FocusDelegate(this.ClearDefaultText));
		}
		else
		{
			this.RemoveFocusDelegate(new UITextField.FocusDelegate(this.ClearDefaultText));
		}
	}

	public string GetDefaultText()
	{
		return this.defaultText;
	}

	public void ClearText()
	{
		this.Text = string.Empty;
		this.OriginalContent = string.Empty;
		this.Clear();
	}

	public override void SetSize(float width, float height)
	{
		base.SetSize(width, height);
		if (this.spriteText)
		{
			this.SetMargins(Vector2.zero);
		}
	}

	public virtual void GoUp()
	{
		this.PositionCaret();
	}

	public virtual void GoDown()
	{
		this.PositionCaret();
	}

	public virtual void CallSliderChangeDelegate(bool upButton)
	{
	}
}
