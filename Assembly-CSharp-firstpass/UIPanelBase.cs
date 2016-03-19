using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class UIPanelBase : MonoBehaviour, IEZDragDrop, IUIContainer, IUIObject
{
	public delegate void TransitionCompleteDelegate(UIPanelBase panel, EZTransition transition);

	protected Dictionary<int, IUIObject> uiObjs = new Dictionary<int, IUIObject>();

	protected EZLinkedList<EZLinkedListNode<UIPanelBase>> childPanels = new EZLinkedList<EZLinkedListNode<UIPanelBase>>();

	[HideInInspector]
	public bool[] blockInput = new bool[]
	{
		true,
		true,
		true,
		true
	};

	protected EZTransition prevTransition;

	protected int prevTransIndex;

	protected bool m_started;

	public int index;

	public bool deactivateAllOnDismiss;

	public bool detargetOnDisable;

	[NonSerialized]
	protected Dictionary<int, GameObject> subjects = new Dictionary<int, GameObject>();

	protected UIPanelBase.TransitionCompleteDelegate tempTransCompleteDel;

	public float width;

	public float height;

	public bool topMost;

	public G_ID twinFormID;

	public G_ID parentFormID;

	public G_ID childFormID_0;

	public G_ID childFormID_1;

	public bool depthChangeable = true;

	private int layer;

	protected bool m_controlIsEnabled = true;

	protected IUIContainer container;

	protected EZInputDelegate inputDelegate;

	protected EZValueChangedDelegate changeDelegate;

	protected EZDragDropDelegate dragDropDelegate;

	public abstract EZTransitionList Transitions
	{
		get;
	}

	public int ChildCount
	{
		get
		{
			return this.uiObjs.Count;
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
			this.container = value;
		}
	}

	public object Data
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool IsDraggable
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public float DragOffset
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float MouseOffset
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public EZAnimation.EASING_TYPE CancelDragEasing
	{
		get
		{
			return EZAnimation.EASING_TYPE.Default;
		}
		set
		{
		}
	}

	public float CancelDragDuration
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public GameObject DropTarget
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool DropHandled
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	protected virtual void OnDisable()
	{
		if (Application.isPlaying)
		{
			if (EZAnimator.Exists())
			{
				EZAnimator.instance.Stop(base.gameObject);
			}
			if (this.detargetOnDisable && UIManager.Exists())
			{
				NrTSingleton<UIManager>.Instance.Detarget(this);
			}
		}
	}

	public virtual void Start()
	{
		if (this.m_started)
		{
			return;
		}
		this.m_started = true;
		this.ScanChildren();
		for (int i = 0; i < this.Transitions.list.Length; i++)
		{
			this.Transitions.list[i].MainSubject = base.gameObject;
		}
		this.SetupTransitionSubjects();
	}

	public void ScanChildren()
	{
		this.uiObjs.Clear();
		Component[] componentsInChildren = base.transform.GetComponentsInChildren(typeof(IUIObject), true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i] == this))
			{
				if (base.gameObject.layer == NrTSingleton<UIManager>.Instance.rayMask)
				{
					UIPanelManager.SetLayerRecursively(componentsInChildren[i].gameObject, base.gameObject.layer);
				}
				IUIObject iUIObject = (IUIObject)componentsInChildren[i];
				if (!this.uiObjs.ContainsKey(componentsInChildren[i].GetHashCode()))
				{
					this.uiObjs.Add(componentsInChildren[i].GetHashCode(), (IUIObject)componentsInChildren[i]);
				}
				iUIObject.RequestContainership(this);
			}
		}
		componentsInChildren = base.transform.GetComponentsInChildren(typeof(UIPanelBase), true);
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (!(componentsInChildren[j] == this))
			{
				if (base.gameObject.layer == NrTSingleton<UIManager>.Instance.rayMask)
				{
					UIPanelManager.SetLayerRecursively(componentsInChildren[j].gameObject, base.gameObject.layer);
				}
				UIPanelBase uIPanelBase = (UIPanelBase)componentsInChildren[j];
				this.childPanels.Add(new EZLinkedListNode<UIPanelBase>(uIPanelBase));
				uIPanelBase.RequestContainership(this);
			}
		}
	}

	protected virtual void SetupTransitionSubjects()
	{
		for (int i = 0; i < 1; i++)
		{
			this.Transitions.list[i].AddTransitionEndDelegate(new EZTransition.OnTransitionEndDelegate(this.TransitionCompleted));
		}
	}

	public void AddChild(GameObject go)
	{
		IUIObject iUIObject = (IUIObject)go.GetComponent("IUIObject");
		if (iUIObject != null)
		{
			if (iUIObject.Container != this)
			{
				iUIObject.Container = this;
			}
			if (!this.uiObjs.ContainsKey(iUIObject.GetHashCode()))
			{
				this.uiObjs.Add(iUIObject.GetHashCode(), iUIObject);
			}
		}
		else
		{
			UIPanelBase uIPanelBase = (UIPanelBase)go.GetComponent(typeof(UIPanelBase));
			if (uIPanelBase != null)
			{
				if (uIPanelBase.Container != this)
				{
					uIPanelBase.Container = this;
				}
				this.childPanels.Add(new EZLinkedListNode<UIPanelBase>(uIPanelBase));
			}
		}
		if (!base.gameObject.activeInHierarchy)
		{
			go.SetActive(false);
		}
	}

	public void RemoveChild(GameObject go)
	{
		IUIObject iUIObject = (IUIObject)go.GetComponent("IUIObject");
		if (iUIObject != null)
		{
			if (this.uiObjs.ContainsKey(iUIObject.GetHashCode()))
			{
				this.uiObjs.Remove(iUIObject.GetHashCode());
				if (iUIObject.Container == this)
				{
					iUIObject.Container = null;
				}
			}
		}
		else
		{
			UIPanelBase uIPanelBase = (UIPanelBase)go.GetComponent(typeof(UIPanelBase));
			if (uIPanelBase != null)
			{
				if (this.childPanels.Rewind())
				{
					while (!(this.childPanels.Current.val == uIPanelBase))
					{
						if (!this.childPanels.MoveNext())
						{
							goto IL_CF;
						}
					}
					this.childPanels.Remove(this.childPanels.Current);
				}
				IL_CF:
				if (uIPanelBase.Container == this)
				{
					uIPanelBase.Container = null;
				}
			}
		}
	}

	public void RemoveFocusObject()
	{
		foreach (IUIObject current in this.uiObjs.Values)
		{
			if (NrTSingleton<UIManager>.Instance.FocusObject == current)
			{
				NrTSingleton<UIManager>.Instance.FocusObject = null;
				break;
			}
		}
	}

	public void MakeChild(GameObject go)
	{
		this.AddChild(go);
		go.transform.parent = base.transform;
		go.transform.localScale = Vector3.one;
	}

	public void AddSubject(GameObject go)
	{
		int hashCode = go.GetHashCode();
		if (this.subjects.ContainsKey(hashCode))
		{
			return;
		}
		this.subjects.Add(hashCode, go);
		for (int i = 0; i < this.Transitions.list.Length; i++)
		{
			this.Transitions.list[i].AddSubSubject(go);
		}
	}

	public void RemoveSubject(GameObject go)
	{
		int hashCode = go.GetHashCode();
		if (!this.subjects.ContainsKey(hashCode))
		{
			return;
		}
		this.subjects.Remove(hashCode);
		for (int i = 0; i < this.Transitions.list.Length; i++)
		{
			this.Transitions.list[i].RemoveSubSubject(go);
		}
	}

	public string[] GetTransitionNames()
	{
		if (this.Transitions == null)
		{
			return null;
		}
		string[] array = new string[this.Transitions.list.Length];
		for (int i = 0; i < this.Transitions.list.Length; i++)
		{
			array[i] = this.Transitions.list[i].name;
		}
		return array;
	}

	public EZTransition GetTransition(int index)
	{
		if (this.Transitions == null)
		{
			return null;
		}
		if (this.Transitions.list == null)
		{
			return null;
		}
		if (this.Transitions.list.Length <= index || index < 0)
		{
			return null;
		}
		return this.Transitions.list[index];
	}

	public EZTransition GetTransition(UIPanelManager.SHOW_MODE transition)
	{
		return this.GetTransition((int)transition);
	}

	public EZTransition GetTransition(string transName)
	{
		if (this.Transitions == null)
		{
			return null;
		}
		if (this.Transitions.list == null)
		{
			return null;
		}
		EZTransition[] list = this.Transitions.list;
		for (int i = 0; i < list.Length; i++)
		{
			if (string.Equals(list[i].name, transName, StringComparison.CurrentCultureIgnoreCase))
			{
				return list[i];
			}
		}
		return null;
	}

	public virtual void StartTransition(UIPanelManager.SHOW_MODE mode)
	{
		if (!this.m_started)
		{
			this.Start();
		}
		if (this.prevTransition != null)
		{
			this.prevTransition.StopSafe();
		}
		this.prevTransIndex = (int)mode;
		if (this.blockInput[this.prevTransIndex])
		{
			NrTSingleton<UIManager>.Instance.LockInput();
		}
		this.prevTransition = this.Transitions.list[this.prevTransIndex];
		if (this.deactivateAllOnDismiss && (mode == UIPanelManager.SHOW_MODE.BringInBack || mode == UIPanelManager.SHOW_MODE.BringInForward))
		{
			base.gameObject.SetActive(true);
			this.Start();
		}
		this.prevTransition.Start();
	}

	public virtual void StartTransition(string transName)
	{
		if (!this.m_started)
		{
			this.Start();
		}
		EZTransition[] list = this.Transitions.list;
		for (int i = 0; i < list.Length; i++)
		{
			if (string.Equals(list[i].name, transName, StringComparison.CurrentCultureIgnoreCase))
			{
				if (this.prevTransition != null)
				{
					this.prevTransition.StopSafe();
				}
				this.prevTransIndex = i;
				if (this.blockInput[this.prevTransIndex])
				{
					NrTSingleton<UIManager>.Instance.LockInput();
				}
				this.prevTransition = list[this.prevTransIndex];
				if (this.deactivateAllOnDismiss && (this.prevTransition == list[1] || this.prevTransition == list[0]))
				{
					base.gameObject.SetActive(true);
					this.Start();
				}
				this.prevTransition.Start();
			}
		}
	}

	public void TransitionCompleted(EZTransition transition)
	{
		if (this.deactivateAllOnDismiss && (transition == this.Transitions.list[2] || transition == this.Transitions.list[3]))
		{
			base.gameObject.SetActive(false);
		}
		if (this.tempTransCompleteDel != null)
		{
			this.tempTransCompleteDel(this, transition);
		}
		this.tempTransCompleteDel = null;
		if (this.blockInput[this.prevTransIndex] && UIManager.Exists())
		{
			NrTSingleton<UIManager>.Instance.UnlockInput();
		}
	}

	public virtual void BringIn()
	{
		this.StartTransition(UIPanelManager.SHOW_MODE.BringInForward);
	}

	public virtual void Dismiss()
	{
		this.StartTransition(UIPanelManager.SHOW_MODE.DismissForward);
	}

	public static int CompareIndices(UIPanelBase a, UIPanelBase b)
	{
		return a.index - b.index;
	}

	public static int CompareZOrder(UIPanelBase a, UIPanelBase b)
	{
		if (a == null || b == null)
		{
			return 0;
		}
		return (int)(b.transform.position.z - a.transform.position.z);
	}

	public void AddTempTransitionDelegate(UIPanelBase.TransitionCompleteDelegate del)
	{
		this.tempTransCompleteDel = (UIPanelBase.TransitionCompleteDelegate)Delegate.Combine(this.tempTransCompleteDel, del);
	}

	public bool RequestContainership(IUIContainer cont)
	{
		Transform parent = base.transform.parent;
		Transform transform = ((Component)cont).transform;
		while (parent != null)
		{
			if (parent == transform)
			{
				this.container = cont;
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

	public bool GotFocus()
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

	public virtual void OnInput(POINTER_INFO ptr)
	{
		if (this.Container != null)
		{
			ptr.callerIsControl = true;
			this.Container.OnInput(ptr);
		}
	}

	public bool IsDragging()
	{
		return false;
	}

	public void SetDragging(bool value)
	{
	}

	public bool DragUpdatePosition(POINTER_INFO ptr)
	{
		return true;
	}

	public void CancelDrag()
	{
	}

	public void OnEZDragDrop(EZDragDropParams parms)
	{
		if (this.dragDropDelegate != null)
		{
			this.dragDropDelegate(parms);
		}
	}

	public void AddDragDropDelegate(EZDragDropDelegate del)
	{
		this.dragDropDelegate = (EZDragDropDelegate)Delegate.Combine(this.dragDropDelegate, del);
	}

	public void RemoveDragDropDelegate(EZDragDropDelegate del)
	{
		this.dragDropDelegate = (EZDragDropDelegate)Delegate.Remove(this.dragDropDelegate, del);
	}

	public void SetDragDropDelegate(EZDragDropDelegate del)
	{
		this.dragDropDelegate = del;
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
