using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/List Item Container")]
[Serializable]
public class UIListItemContainer : ControlBase, IUIListObject, IEZDragDrop, IUIContainer, IUIObject
{
	public enum CONTROL_STATE
	{
		NORMAL,
		OVER,
		ACTIVE,
		DISABLED
	}

	private bool autoAnimatorStop = true;

	protected List<AutoSpriteControlBase> uiObjs = new List<AutoSpriteControlBase>();

	protected List<UIListItemContainer> containerObjs = new List<UIListItemContainer>();

	protected bool m_started;

	private float fontSize = 16f;

	protected SpriteText.Font_Effect fontEffect = SpriteText.Font_Effect.Black_Shadow_Small;

	private Vector2 topLeftEdge = Vector2.zero;

	private Vector2 bottomRightEdge = Vector2.zero;

	private Rect3D clippingRect;

	private bool clipped;

	private UIScrollList list;

	protected int index;

	private bool m_selected;

	private bool m_locked;

	private bool autoFindOuterEdges = true;

	protected UIListItemContainer.CONTROL_STATE m_ctrlState;

	public bool AutoAnimatorStop
	{
		set
		{
			this.autoAnimatorStop = value;
		}
	}

	public float FontSize
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
		get
		{
			return this.fontEffect;
		}
		set
		{
			this.fontEffect = value;
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
			base.Container = value;
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

	public override string[] States
	{
		get
		{
			return null;
		}
	}

	public UIListItemContainer.CONTROL_STATE controlState
	{
		get
		{
			return this.m_ctrlState;
		}
	}

	public override bool controlIsEnabled
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public bool AutoFindOuterEdges
	{
		set
		{
			this.autoFindOuterEdges = value;
		}
	}

	public bool Managed
	{
		get
		{
			return false;
		}
	}

	public override string Text
	{
		set
		{
			base.Text = value;
			this.FindOuterEdges();
			if (this.spriteText != null && this.spriteText.maxWidth > 0f && this.list != null)
			{
				this.list.PositionItems();
			}
		}
	}

	public SpriteText TextObj
	{
		get
		{
			return this.spriteText;
		}
	}

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

	public override void Start()
	{
		if (this.m_started)
		{
			return;
		}
		this.m_started = true;
	}

	public void AddChild(GameObject go)
	{
		AutoSpriteControlBase autoSpriteControlBase = (AutoSpriteControlBase)go.GetComponent(typeof(AutoSpriteControlBase));
		if (autoSpriteControlBase != null)
		{
			if (autoSpriteControlBase is AutoSpriteControlBase && autoSpriteControlBase.Container != this)
			{
				autoSpriteControlBase.Container = this;
			}
			this.uiObjs.Add(autoSpriteControlBase);
		}
		else
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)go.GetComponent(typeof(UIListItemContainer));
			if (null != uIListItemContainer)
			{
				this.containerObjs.Add(uIListItemContainer);
			}
		}
	}

	public void RemoveChild(GameObject go)
	{
		if (null == go)
		{
			return;
		}
		AutoSpriteControlBase autoSpriteControlBase = (AutoSpriteControlBase)go.GetComponent(typeof(AutoSpriteControlBase));
		if (autoSpriteControlBase != null)
		{
			for (int i = 0; i < this.uiObjs.Count; i++)
			{
				if (this.uiObjs[i] == autoSpriteControlBase)
				{
					this.uiObjs.RemoveAt(i);
					break;
				}
			}
			if (autoSpriteControlBase is AutoSpriteControlBase && autoSpriteControlBase.Container == this)
			{
				autoSpriteControlBase.Container = null;
			}
		}
		else
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)go.GetComponent(typeof(UIListItemContainer));
			if (uIListItemContainer != null)
			{
				for (int j = 0; j < this.containerObjs.Count; j++)
				{
					if (this.containerObjs[j] == uIListItemContainer)
					{
						this.containerObjs.RemoveAt(j);
						break;
					}
				}
			}
		}
	}

	public void AddSubject(GameObject go)
	{
	}

	public void RemoveSubject(GameObject go)
	{
	}

	public void MakeChild(GameObject go)
	{
		this.AddChild(go);
		go.transform.parent = base.transform;
		go.transform.localScale = Vector3.one;
	}

	public object GetElementObject(int index)
	{
		index++;
		if (0 > index || this.uiObjs.Count <= index)
		{
			return null;
		}
		if (!(null != this.uiObjs[index]))
		{
			return null;
		}
		if (this.uiObjs[index] is AutoSpriteControlBase)
		{
			return this.uiObjs[index].Data;
		}
		return null;
	}

	public AutoSpriteControlBase GetElement(string elementName)
	{
		for (int i = 0; i < this.uiObjs.Count; i++)
		{
			if (this.uiObjs[i].name == elementName)
			{
				return this.uiObjs[i];
			}
		}
		return null;
	}

	public AutoSpriteControlBase GetElement(int value)
	{
		int num = value + 1;
		if (0 <= num && num < base.transform.childCount)
		{
			Transform child = base.transform.GetChild(num);
			if (null != child)
			{
				AutoSpriteControlBase component = child.GetComponent<AutoSpriteControlBase>();
				if (null != component)
				{
					return component;
				}
			}
		}
		return null;
	}

	public override void OnInput(POINTER_INFO ptr)
	{
		if (this.deleted)
		{
			return;
		}
		if (null == this.list)
		{
			return;
		}
		if (!this.m_controlIsEnabled)
		{
			switch (ptr.evt)
			{
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.TAP:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				this.list.PointerReleased();
				break;
			case POINTER_INFO.INPUT_EVENT.DRAG:
			case POINTER_INFO.INPUT_EVENT.MOUSE_WHEEL:
				this.list.ListDragged(ptr);
				break;
			}
			if (this.Container != null)
			{
				ptr.callerIsControl = true;
				this.Container.OnInput(ptr);
			}
			return;
		}
		if (this.list != null && Vector3.SqrMagnitude(ptr.origPos - ptr.devicePos) > this.list.dragThreshold * this.list.dragThreshold)
		{
			ptr.isTap = false;
			if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP || ptr.evt == POINTER_INFO.INPUT_EVENT.LONG_TAP)
			{
				ptr.evt = POINTER_INFO.INPUT_EVENT.RELEASE;
			}
		}
		else
		{
			ptr.isTap = true;
		}
		if (this.inputDelegate != null)
		{
			this.inputDelegate(ref ptr);
		}
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
			if (this.list != null && ptr.active)
			{
				this.list.ListDragged(ptr);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.PRESS:
			if (!this.IsSelected())
			{
				if (this.list.GetMultiSelectMode())
				{
					if (this.list.MaxMultiSelectNum == this.list.SelectedItems.Count)
					{
						this.SetControlState(UIListItemContainer.CONTROL_STATE.NORMAL);
					}
					else
					{
						this.SetControlState(UIListItemContainer.CONTROL_STATE.ACTIVE);
					}
				}
				else
				{
					this.SetControlState(UIListItemContainer.CONTROL_STATE.ACTIVE);
				}
			}
			break;
		case POINTER_INFO.INPUT_EVENT.DOUBLE_PRESS:
			this.list.DidDoubleClick(this);
			this.list.PointerReleased();
			break;
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			if (!this.IsSelected())
			{
				this.SetControlState(UIListItemContainer.CONTROL_STATE.NORMAL);
			}
			this.list.PointerReleased();
			break;
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (ptr.isTap)
			{
				this.list.DidSelect(this);
				if (ptr.targetObj is AutoSpriteControlBase)
				{
					if (!((AutoSpriteControlBase)ptr.targetObj).IsHaveDelegate())
					{
						this.list.DidClick(this);
					}
				}
				else
				{
					this.list.DidClick(this);
				}
				if (this.changeDelegate != null)
				{
					this.changeDelegate(this);
				}
			}
			else if (!this.IsSelected())
			{
				this.SetControlState(UIListItemContainer.CONTROL_STATE.NORMAL);
			}
			else
			{
				this.SetControlState(UIListItemContainer.CONTROL_STATE.ACTIVE);
			}
			this.list.PointerReleased();
			break;
		case POINTER_INFO.INPUT_EVENT.RIGHT_TAP:
			this.list.DidRightMouse(this);
			this.list.PointerReleased();
			break;
		case POINTER_INFO.INPUT_EVENT.LONG_TAP:
			if (ptr.isTap)
			{
				this.list.DidLongClick(this);
			}
			else if (!this.IsSelected())
			{
				this.SetControlState(UIListItemContainer.CONTROL_STATE.NORMAL);
			}
			else
			{
				this.SetControlState(UIListItemContainer.CONTROL_STATE.ACTIVE);
			}
			this.list.PointerReleased();
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE:
			if (ptr.type != POINTER_INFO.POINTER_TYPE.TOUCHPAD && this.m_ctrlState != UIListItemContainer.CONTROL_STATE.OVER)
			{
				this.SetControlState(UIListItemContainer.CONTROL_STATE.OVER);
				this.list.DidMouseOver(this);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			if (!this.list.isScrolling)
			{
				this.list.overList = false;
			}
			this.list.DidMouseOut(this);
			if (!this.IsSelected())
			{
				this.SetControlState(UIListItemContainer.CONTROL_STATE.NORMAL);
			}
			else
			{
				this.SetControlState(UIListItemContainer.CONTROL_STATE.ACTIVE);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.DRAG:
			if (!ptr.isTap)
			{
				this.list.ListDragged(ptr);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOUSE_WHEEL:
			this.list.overList = true;
			this.list.ListDragged(ptr);
			break;
		}
		base.OnInput(ptr);
	}

	public override EZTransitionList GetTransitions(int index)
	{
		return null;
	}

	public void SetControlIsEnabled(bool value)
	{
		bool controlIsEnabled = this.m_controlIsEnabled;
		this.m_controlIsEnabled = value;
		if (!value)
		{
			this.SetControlState(UIListItemContainer.CONTROL_STATE.DISABLED);
		}
		else if (!controlIsEnabled)
		{
			this.SetControlState(UIListItemContainer.CONTROL_STATE.NORMAL);
		}
	}

	public void SetToolTip(string toolTip)
	{
		if (null != base.transform)
		{
			Transform transform = base.transform.FindChild(UIScrollList.backButtonName);
			if (null != transform)
			{
				UIButton component = transform.GetComponent<UIButton>();
				if (null != transform)
				{
					component.ToolTip = toolTip;
				}
			}
		}
	}

	public void SetControlState(UIListItemContainer.CONTROL_STATE s)
	{
		this.m_ctrlState = s;
		if (null != base.transform)
		{
			Transform transform = base.transform.FindChild(UIScrollList.backButtonName);
			if (transform == null)
			{
				return;
			}
			Component component = transform.GetComponent<UIButton>();
			if (null == component)
			{
				return;
			}
			UIButton uIButton = (UIButton)component;
			if (null == uIButton)
			{
				return;
			}
			uIButton.SetControlState((UIButton.CONTROL_STATE)s);
		}
	}

	public bool IsContainer()
	{
		return true;
	}

	public void FindOuterEdges()
	{
		if (!this.m_started)
		{
			this.Start();
		}
		this.topLeftEdge = Vector2.zero;
		this.bottomRightEdge = Vector2.zero;
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		if (!this.autoFindOuterEdges)
		{
			if (0 < this.uiObjs.Count)
			{
				int num = 0;
				Matrix4x4 localToWorldMatrix = this.uiObjs[num].transform.localToWorldMatrix;
				Vector3 vector2;
				if (this.uiObjs[num] is AutoSpriteControlBase)
				{
					this.uiObjs[num].FindOuterEdges();
					Vector3 vector = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.uiObjs[num].TopLeftEdge()));
					vector2 = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.uiObjs[num].BottomRightEdge()));
				}
				else
				{
					Vector3 vector = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.uiObjs[num].UnclippedTopLeft));
					vector2 = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.uiObjs[num].UnclippedBottomRight));
				}
				this.topLeftEdge.x = 0f;
				this.topLeftEdge.y = 0f;
				this.bottomRightEdge.x = Mathf.Max(this.bottomRightEdge.x, vector2.x);
				this.bottomRightEdge.y = Mathf.Min(this.bottomRightEdge.y, vector2.y);
			}
			return;
		}
		for (int i = 0; i < this.uiObjs.Count; i++)
		{
			if (null == this.uiObjs[i])
			{
				TsLog.LogWarning("uiObjs[" + i + "] null continue", new object[0]);
			}
			else
			{
				Matrix4x4 localToWorldMatrix = this.uiObjs[i].transform.localToWorldMatrix;
				Vector3 vector;
				Vector3 vector2;
				if (this.uiObjs[i] is AutoSpriteControlBase)
				{
					this.uiObjs[i].FindOuterEdges();
					vector = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.uiObjs[i].TopLeftEdge()));
					vector2 = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.uiObjs[i].BottomRightEdge()));
				}
				else
				{
					vector = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.uiObjs[i].UnclippedTopLeft));
					vector2 = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.uiObjs[i].UnclippedBottomRight));
				}
				this.topLeftEdge.x = Mathf.Min(this.topLeftEdge.x, vector.x);
				this.topLeftEdge.y = Mathf.Max(this.topLeftEdge.y, vector.y);
				this.bottomRightEdge.x = Mathf.Max(this.bottomRightEdge.x, vector2.x);
				this.bottomRightEdge.y = Mathf.Min(this.bottomRightEdge.y, vector2.y);
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

	public void Hide(bool tf)
	{
		for (int i = 0; i < this.uiObjs.Count; i++)
		{
			if (this.uiObjs[i] != null)
			{
				this.uiObjs[i].Hide(tf);
			}
		}
		for (int j = 0; j < this.containerObjs.Count; j++)
		{
			if (this.containerObjs[j] != null)
			{
				this.containerObjs[j].Hide(tf);
			}
		}
	}

	public Rect3D GetClippingRect()
	{
		return this.clippingRect;
	}

	public void SetClippingRect(Rect3D value)
	{
		this.clipped = true;
		this.clippingRect = value;
		for (int i = 0; i < this.uiObjs.Count; i++)
		{
			if (null != this.uiObjs[i])
			{
				this.uiObjs[i].SetClippingRect(value);
			}
		}
		for (int j = 0; j < this.containerObjs.Count; j++)
		{
			if (null != this.containerObjs[j])
			{
				this.containerObjs[j].SetClippingRect(value);
			}
		}
	}

	public bool IsClipped()
	{
		return this.clipped;
	}

	public void SetClipped(bool value)
	{
		if (value && !this.clipped)
		{
			this.clipped = true;
			this.SetClippingRect(this.clippingRect);
		}
		else if (this.clipped)
		{
			this.Unclip();
		}
		this.clipped = value;
	}

	public void Unclip()
	{
		this.clipped = false;
		for (int i = 0; i < this.uiObjs.Count; i++)
		{
			if (null != this.uiObjs[i])
			{
				this.uiObjs[i].Unclip();
			}
		}
		if (this.spriteText != null)
		{
			this.spriteText.Unclip();
		}
		for (int j = 0; j < this.containerObjs.Count; j++)
		{
			if (null != this.containerObjs[j])
			{
				this.containerObjs[j].Unclip();
			}
		}
	}

	public override void UpdateCollider()
	{
		for (int i = 0; i < this.uiObjs.Count; i++)
		{
			if (!(null == this.uiObjs[i]))
			{
				if (this.uiObjs[i] is AutoSpriteControlBase)
				{
					this.uiObjs[i].UpdateCollider();
				}
			}
		}
	}

	public void SetList(UIScrollList c)
	{
		this.list = c;
	}

	public int GetIndex()
	{
		return this.index;
	}

	public void SetIndex(int value)
	{
		this.index = value;
	}

	public bool IsSelected()
	{
		return this.m_selected;
	}

	public void SetSelected(bool value)
	{
		if (!this.m_started)
		{
			return;
		}
		this.m_selected = value;
		if (this.m_selected)
		{
			this.SetControlState(UIListItemContainer.CONTROL_STATE.ACTIVE);
		}
		else
		{
			this.SetControlState(UIListItemContainer.CONTROL_STATE.NORMAL);
		}
		Transform transform = base.transform.FindChild(UIScrollList.selectImageName);
		if (transform != null && null != transform.gameObject)
		{
			transform.gameObject.SetActive(value);
		}
	}

	public bool IsLocked()
	{
		return this.m_locked;
	}

	public void SetLocked(bool value)
	{
		this.m_locked = value;
		Transform transform = base.transform.FindChild(UIScrollList.lockImageName);
		if (transform != null && transform.gameObject != null)
		{
			transform.gameObject.SetActive(value);
		}
	}

	public void Delete()
	{
		for (int i = 0; i < this.uiObjs.Count; i++)
		{
			this.uiObjs[i].Delete();
		}
		this.uiObjs.Clear();
		for (int j = 0; j < this.containerObjs.Count; j++)
		{
			this.containerObjs[j].Delete();
		}
		this.containerObjs.Clear();
	}

	public void DeleteListItemContainer()
	{
		for (int i = 0; i < this.uiObjs.Count; i++)
		{
			this.uiObjs[i].Delete();
			UnityEngine.Object.Destroy(this.uiObjs[i].gameObject);
		}
		this.uiObjs.Clear();
		for (int j = 0; j < this.containerObjs.Count; j++)
		{
			this.containerObjs[j].Delete();
			UnityEngine.Object.Destroy(this.containerObjs[j].gameObject);
		}
		this.containerObjs.Clear();
	}

	public void DeleteAnim()
	{
		if (EZAnimator.Exists())
		{
			for (int i = 0; i < this.uiObjs.Count; i++)
			{
				EZAnimator.instance.Stop(this.uiObjs[i].gameObject);
				EZAnimator.instance.Stop(this.uiObjs[i]);
			}
		}
	}

	public void SetAlphaList(float a)
	{
		for (int i = 0; i < this.uiObjs.Count; i++)
		{
			if (null != this.uiObjs[i])
			{
				this.uiObjs[i].SetColor(new Color(1f, 1f, 1f, a));
			}
		}
	}

	public void FadeListItemContainer()
	{
		SpriteRoot[] componentsInChildren = base.transform.GetComponentsInChildren<SpriteRoot>(true);
		if (componentsInChildren != null)
		{
			SpriteRoot[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				SpriteRoot spriteRoot = array[i];
				if (null != spriteRoot)
				{
					FadeSprite.Do(spriteRoot, EZAnimation.ANIM_MODE.To, new Color(1f, 1f, 1f, 0.3f), new EZAnimation.Interpolator(EZAnimation.elasticIn), 0f, 0f, null, null);
				}
			}
		}
		SpriteText[] componentsInChildren2 = base.transform.GetComponentsInChildren<SpriteText>(true);
		if (componentsInChildren2 != null)
		{
			SpriteText[] array2 = componentsInChildren2;
			for (int j = 0; j < array2.Length; j++)
			{
				SpriteText spriteText = array2[j];
				if (null != spriteText)
				{
					FadeText.Do(spriteText, EZAnimation.ANIM_MODE.To, new Color(1f, 1f, 1f, 0.3f), new EZAnimation.Interpolator(EZAnimation.elasticIn), 0f, 0f, null, null);
				}
			}
		}
	}

	public void FadeListItemContainer(float dur)
	{
		SpriteRoot[] componentsInChildren = base.transform.GetComponentsInChildren<SpriteRoot>(true);
		if (componentsInChildren != null)
		{
			SpriteRoot[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				SpriteRoot spriteRoot = array[i];
				if (null != spriteRoot)
				{
					FadeSprite.Do(spriteRoot, EZAnimation.ANIM_MODE.FromTo, new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 0f), new EZAnimation.Interpolator(EZAnimation.linear), dur, 0f, null, null);
				}
			}
		}
		SpriteText[] componentsInChildren2 = base.transform.GetComponentsInChildren<SpriteText>(true);
		if (componentsInChildren2 != null)
		{
			SpriteText[] array2 = componentsInChildren2;
			for (int j = 0; j < array2.Length; j++)
			{
				SpriteText spriteText = array2[j];
				if (null != spriteText)
				{
					FadeText.Do(spriteText, EZAnimation.ANIM_MODE.FromTo, new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 0f), new EZAnimation.Interpolator(EZAnimation.linear), dur, 0f, null, null);
				}
			}
		}
	}

	public override void OnDisable()
	{
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

	public void SetAlpha(float _alpha)
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (null != child)
			{
				AutoSpriteControlBase component = child.GetComponent<AutoSpriteControlBase>();
				if (null != component)
				{
					component.SetAlpha(_alpha);
				}
			}
		}
	}

	public void AlphaAni(float startA, float destA, float time)
	{
		SpriteRoot[] componentsInChildren = base.transform.GetComponentsInChildren<SpriteRoot>(true);
		if (componentsInChildren != null)
		{
			SpriteRoot[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				SpriteRoot spriteRoot = array[i];
				if (null != spriteRoot)
				{
					FadeSprite.Do(spriteRoot, EZAnimation.ANIM_MODE.FromTo, new Color(spriteRoot.color.r, spriteRoot.color.g, spriteRoot.color.b, startA), new Color(spriteRoot.color.r, spriteRoot.color.g, spriteRoot.color.b, destA), new EZAnimation.Interpolator(EZAnimation.linear), time, 0f, null, null);
				}
			}
		}
		SpriteText[] componentsInChildren2 = base.transform.GetComponentsInChildren<SpriteText>(true);
		if (componentsInChildren2 != null)
		{
			SpriteText[] array2 = componentsInChildren2;
			for (int j = 0; j < array2.Length; j++)
			{
				SpriteText spriteText = array2[j];
				if (null != spriteText)
				{
					FadeText.Do(spriteText, EZAnimation.ANIM_MODE.FromTo, new Color(spriteText.color.r, spriteText.color.g, spriteText.color.b, startA), new Color(spriteText.color.r, spriteText.color.g, spriteText.color.b, destA), new EZAnimation.Interpolator(EZAnimation.linear), time, 0f, null, null);
				}
			}
		}
	}

	public void StopAni()
	{
		SpriteRoot[] componentsInChildren = base.transform.GetComponentsInChildren<SpriteRoot>(true);
		if (componentsInChildren != null)
		{
			SpriteRoot[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				SpriteRoot spriteRoot = array[i];
				if (null != spriteRoot)
				{
					EZAnimator.instance.Stop(spriteRoot.gameObject);
					EZAnimator.instance.Stop(spriteRoot);
				}
			}
		}
		SpriteText[] componentsInChildren2 = base.transform.GetComponentsInChildren<SpriteText>(true);
		if (componentsInChildren2 != null)
		{
			SpriteText[] array2 = componentsInChildren2;
			for (int j = 0; j < array2.Length; j++)
			{
				SpriteText spriteText = array2[j];
				if (null != spriteText)
				{
					EZAnimator.instance.Stop(spriteText.gameObject);
					EZAnimator.instance.Stop(spriteText);
				}
			}
		}
	}
}
