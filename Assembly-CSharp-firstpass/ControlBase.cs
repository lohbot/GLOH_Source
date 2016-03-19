using System;
using UnityEngine;

public abstract class ControlBase : MonoBehaviour, IEZDragDrop, IControl, IUIObject
{
	public const string DittoString = "[\"]";

	protected ControlBaseMirror mirror;

	public string text;

	public SpriteText spriteText;

	public float textOffsetZ = -0.1f;

	public bool includeTextInAutoCollider;

	protected SpriteText.Anchor_Pos defaultTextAnchor = SpriteText.Anchor_Pos.Middle_Center;

	protected SpriteText.Alignment_Type defaultTextAlignment = SpriteText.Alignment_Type.Center;

	protected bool deleted;

	public bool detargetOnDisable;

	protected bool customCollider;

	[HideInInspector]
	public object data;

	private int layer;

	protected bool m_controlIsEnabled = true;

	protected IUIContainer container;

	protected EZInputDelegate inputDelegate;

	protected EZValueChangedDelegate changeDelegate;

	protected EZValueChangedDelegate mouseOverDelegate;

	protected EZValueChangedDelegate mouseOutDelegate;

	protected EZValueChangedDelegate rightMouseDelegate;

	protected EZDragDropHelper dragDropHelper = new EZDragDropHelper();

	public bool isDraggable;

	public float dragOffset = 1f;

	public float mouseOffset = 1f;

	public EZAnimation.EASING_TYPE cancelDragEasing;

	public float cancelDragDuration;

	public virtual string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			this.text = value;
			if (this.spriteText == null)
			{
				if (this.text == string.Empty)
				{
					return;
				}
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
				gameObject.name = "control_text";
				MeshRenderer meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
				meshRenderer.sharedMaterial = NrTSingleton<UIManager>.Instance.defaultFontMaterial;
				this.spriteText = (SpriteText)gameObject.AddComponent(typeof(SpriteText));
				this.spriteText.font = NrTSingleton<UIManager>.Instance.defaultFont;
				this.spriteText.offsetZ = this.textOffsetZ;
				this.spriteText.Parent = this;
				this.spriteText.anchor = this.defaultTextAnchor;
				this.spriteText.alignment = this.defaultTextAlignment;
				this.spriteText.SetCharacterSize(16f);
				this.spriteText.pixelPerfect = false;
				this.spriteText.Start();
			}
			this.spriteText.Text = this.text;
			if (this.includeTextInAutoCollider)
			{
				this.UpdateCollider();
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
			if (this.container != null && this.spriteText != null)
			{
				this.container.RemoveChild(this.spriteText.gameObject);
			}
			if (value != null && this.spriteText != null)
			{
				value.AddChild(this.spriteText.gameObject);
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

	public abstract string[] States
	{
		get;
	}

	public abstract EZTransitionList[] Transitions
	{
		get;
		set;
	}

	protected virtual void Awake()
	{
		if (base.collider != null)
		{
			this.customCollider = true;
		}
		if (this.dragDropHelper == null)
		{
			this.dragDropHelper = new EZDragDropHelper(this);
		}
		else
		{
			this.dragDropHelper.host = this;
		}
	}

	public virtual void Start()
	{
		if (this.spriteText != null)
		{
			this.spriteText.Parent = this;
		}
	}

	protected virtual void AddCollider()
	{
		if (this.customCollider)
		{
			return;
		}
		base.gameObject.AddComponent(typeof(BoxCollider));
		this.UpdateCollider();
	}

	public virtual void UpdateCollider()
	{
		if (this.customCollider || !(base.collider is BoxCollider))
		{
			return;
		}
		BoxCollider boxCollider = (BoxCollider)base.collider;
		if (this.includeTextInAutoCollider && this.spriteText != null)
		{
			Bounds bounds = new Bounds(boxCollider.center, boxCollider.size);
			Matrix4x4 localToWorldMatrix = this.spriteText.transform.localToWorldMatrix;
			Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
			Vector3 point = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.spriteText.TopLeft)) * 2f;
			Vector3 point2 = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(this.spriteText.BottomRight)) * 2f;
			bounds.Encapsulate(point);
			bounds.Encapsulate(point2);
			boxCollider.size = bounds.extents;
			boxCollider.center = bounds.center * 0.5f;
		}
		boxCollider.isTrigger = true;
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

	public void SetMouseOverDelegate(EZValueChangedDelegate del)
	{
		this.mouseOverDelegate = del;
	}

	public void AddMouseOverDelegate(EZValueChangedDelegate del)
	{
		this.mouseOverDelegate = (EZValueChangedDelegate)Delegate.Combine(this.mouseOverDelegate, del);
	}

	public void RemoveMouseOverDelegate(EZValueChangedDelegate del)
	{
		this.mouseOverDelegate = (EZValueChangedDelegate)Delegate.Remove(this.mouseOverDelegate, del);
	}

	public void SetMouseOutDelegate(EZValueChangedDelegate del)
	{
		this.mouseOutDelegate = del;
	}

	public void AddMouseOutDelegate(EZValueChangedDelegate del)
	{
		this.mouseOutDelegate = (EZValueChangedDelegate)Delegate.Combine(this.mouseOutDelegate, del);
	}

	public void RemoveMouseOutDelegate(EZValueChangedDelegate del)
	{
		this.mouseOutDelegate = (EZValueChangedDelegate)Delegate.Remove(this.mouseOutDelegate, del);
	}

	public void SetRightMouseDelegate(EZValueChangedDelegate del)
	{
		this.rightMouseDelegate = del;
	}

	public void AddRightMouseDelegate(EZValueChangedDelegate del)
	{
		this.rightMouseDelegate = (EZValueChangedDelegate)Delegate.Combine(this.rightMouseDelegate, del);
	}

	public void RemoveRightMouseDelegate(EZValueChangedDelegate del)
	{
		this.rightMouseDelegate = (EZValueChangedDelegate)Delegate.Remove(this.rightMouseDelegate, del);
	}

	public virtual void OnInput(POINTER_INFO ptr)
	{
		if (this.Container != null)
		{
			ptr.callerIsControl = true;
			this.Container.OnInput(ptr);
		}
	}

	public virtual void OnEnable()
	{
	}

	public virtual void OnDisable()
	{
		if (Application.isPlaying)
		{
			if (EZAnimator.Exists())
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

	public virtual void OnDestroy()
	{
		this.deleted = true;
	}

	public virtual void Copy(IControl ctl)
	{
		this.Copy(ctl, ControlCopyFlags.All);
	}

	public virtual void Copy(IControl ctl, ControlCopyFlags flags)
	{
		if (!(ctl is ControlBase))
		{
			return;
		}
		ControlBase controlBase = (ControlBase)ctl;
		if ((flags & ControlCopyFlags.Transitions) == ControlCopyFlags.Transitions)
		{
			if (controlBase is UIStateToggleBtn3D)
			{
				if (controlBase.Transitions != null)
				{
					((UIStateToggleBtn3D)this).transitions = new EZTransitionList[controlBase.Transitions.Length];
					for (int i = 0; i < this.Transitions.Length; i++)
					{
						controlBase.Transitions[i].CopyToNew(this.Transitions[i], true);
					}
				}
			}
			else if (this.Transitions != null && controlBase.Transitions != null)
			{
				int num = 0;
				while (num < this.Transitions.Length && num < controlBase.Transitions.Length)
				{
					controlBase.Transitions[num].CopyTo(this.Transitions[num], true);
					num++;
				}
			}
		}
		if ((flags & ControlCopyFlags.Text) == ControlCopyFlags.Text)
		{
			if (this.spriteText == null && controlBase.spriteText != null)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(controlBase.spriteText.gameObject);
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = controlBase.spriteText.transform.localPosition;
				gameObject.transform.localScale = controlBase.spriteText.transform.localScale;
				gameObject.transform.localRotation = controlBase.spriteText.transform.localRotation;
			}
			this.Text = controlBase.Text;
		}
		if ((flags & ControlCopyFlags.Appearance) == ControlCopyFlags.Appearance && base.collider.GetType() == controlBase.collider.GetType())
		{
			if (base.collider is BoxCollider)
			{
				BoxCollider boxCollider = (BoxCollider)base.collider;
				BoxCollider boxCollider2 = (BoxCollider)controlBase.collider;
				boxCollider.center = boxCollider2.center;
				boxCollider.size = boxCollider2.size;
			}
			else if (base.collider is SphereCollider)
			{
				SphereCollider sphereCollider = (SphereCollider)base.collider;
				SphereCollider sphereCollider2 = (SphereCollider)controlBase.collider;
				sphereCollider.center = sphereCollider2.center;
				sphereCollider.radius = sphereCollider2.radius;
			}
			else if (base.collider is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = (CapsuleCollider)base.collider;
				CapsuleCollider capsuleCollider2 = (CapsuleCollider)controlBase.collider;
				capsuleCollider.center = capsuleCollider2.center;
				capsuleCollider.radius = capsuleCollider2.radius;
				capsuleCollider.height = capsuleCollider2.height;
				capsuleCollider.direction = capsuleCollider2.direction;
			}
			else if (base.collider is MeshCollider)
			{
				MeshCollider meshCollider = (MeshCollider)base.collider;
				MeshCollider meshCollider2 = (MeshCollider)controlBase.collider;
				meshCollider.smoothSphereCollisions = meshCollider2.smoothSphereCollisions;
				meshCollider.convex = meshCollider2.convex;
				meshCollider.sharedMesh = meshCollider2.sharedMesh;
			}
			base.collider.isTrigger = controlBase.collider.isTrigger;
		}
		if ((flags & ControlCopyFlags.Invocation) == ControlCopyFlags.Invocation)
		{
			this.changeDelegate = controlBase.changeDelegate;
			this.inputDelegate = controlBase.inputDelegate;
		}
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			this.Container = controlBase.Container;
			if (Application.isPlaying)
			{
				this.controlIsEnabled = controlBase.controlIsEnabled;
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
		return this.States;
	}

	public abstract EZTransitionList GetTransitions(int index);

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

	public virtual void DoMirror()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.mirror == null)
		{
			this.mirror = new ControlBaseMirror();
			this.mirror.Mirror(this);
		}
		this.mirror.Validate(this);
		if (this.mirror.DidChange(this))
		{
			this.mirror.Mirror(this);
		}
	}

	public virtual void OnDrawGizmos()
	{
		this.DoMirror();
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
