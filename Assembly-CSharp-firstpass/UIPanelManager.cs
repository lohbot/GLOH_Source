using Ndoors.Framework.Stage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("EZ GUI/Management/Panel Manager")]
[Serializable]
public class UIPanelManager : MonoBehaviour, IEZDragDrop, IUIContainer, IUIObject
{
	public enum SHOW_MODE
	{
		BringInForward,
		BringInBack,
		DismissForward,
		DismissBack
	}

	public enum MENU_DIRECTION
	{
		Forwards,
		Backwards,
		Auto
	}

	protected static UIPanelManager m_instance;

	protected List<UIPanelBase> panels = new List<UIPanelBase>();

	public UIPanelBase initialPanel;

	public bool deactivateAllButInitialAtStart;

	public bool linearNavigation;

	public bool circular;

	public bool advancePastEnd;

	protected UIPanelBase curPanel;

	protected UIPanelBase prevPanel;

	private UIPanelBase mouseOverPanel;

	private UIPanelBase focusPanel;

	protected bool m_started;

	protected List<UIPanelBase> breadcrumbs = new List<UIPanelBase>();

	public static readonly float EFFECT_UI_DEPTH = 10f;

	public static readonly float TOPMOST_UI_DEPTH = 100f;

	public static readonly float SCENE_DEPTH = 500f;

	public static readonly float UI_DEPTH = 1000f;

	public static readonly float UI_DEPTH_DecreValue = 8f;

	private float highestZValue = UIPanelManager.UI_DEPTH;

	private int layer;

	protected bool m_controlIsEnabled = true;

	protected IUIContainer container;

	protected EZInputDelegate inputDelegate;

	protected EZValueChangedDelegate changeDelegate;

	protected EZDragDropDelegate dragDropDelegate;

	public static UIPanelManager instance
	{
		get
		{
			return UIPanelManager.m_instance;
		}
	}

	public UIPanelBase CurrentPanel
	{
		get
		{
			return this.curPanel;
		}
		set
		{
			this.curPanel = value;
		}
	}

	public UIPanelBase MouseOverPanel
	{
		get
		{
			return this.mouseOverPanel;
		}
		set
		{
			this.mouseOverPanel = value;
		}
	}

	public UIPanelBase FocusPanel
	{
		get
		{
			return this.focusPanel;
		}
		set
		{
			this.focusPanel = value;
		}
	}

	public float HighestZValue
	{
		get
		{
			return this.highestZValue;
		}
	}

	public float TOPMOSTZ
	{
		get
		{
			return UIPanelManager.TOPMOST_UI_DEPTH;
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
			return false;
		}
		set
		{
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

	public UIPanelBase FindPanel(int index)
	{
		for (int i = 0; i < this.panels.Count; i++)
		{
			if (this.panels[i].index == index)
			{
				return this.panels[i];
			}
		}
		return null;
	}

	public void AddChild(GameObject go)
	{
		UIPanelBase uIPanelBase = (UIPanelBase)go.GetComponent(typeof(UIPanelBase));
		if (uIPanelBase == null)
		{
			return;
		}
		if (this.panels.IndexOf(uIPanelBase) >= 0)
		{
			return;
		}
		this.panels.Add(uIPanelBase);
		uIPanelBase.Container = this;
		if ("TOOTIP_DLG" == uIPanelBase.name)
		{
			return;
		}
		if (!uIPanelBase.topMost && Math.Abs(this.highestZValue - base.transform.position.z) > 1E-06f)
		{
			this.highestZValue = uIPanelBase.transform.position.z;
		}
	}

	public void RemoveChild(GameObject go)
	{
		UIPanelBase uIPanelBase = (UIPanelBase)go.GetComponent(typeof(UIPanelBase));
		if (uIPanelBase == null)
		{
			return;
		}
		this.panels.Remove(uIPanelBase);
		uIPanelBase.Container = null;
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
	}

	private void Awake()
	{
		if (UIPanelManager.m_instance == null)
		{
			UIPanelManager.m_instance = this;
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		UIPanelManager.<Start>c__Iterator5 <Start>c__Iterator = new UIPanelManager.<Start>c__Iterator5();
		<Start>c__Iterator.<>f__this = this;
		return <Start>c__Iterator;
	}

	protected virtual void OnEnable()
	{
		if (this.m_started && this.deactivateAllButInitialAtStart)
		{
			for (int i = 0; i < this.panels.Count; i++)
			{
				if (this.panels[i] != this.curPanel)
				{
					this.panels[i].gameObject.SetActive(false);
				}
			}
		}
	}

	public void ScanChildren()
	{
		this.panels.Clear();
		Component[] componentsInChildren = base.transform.GetComponentsInChildren(typeof(UIPanelBase), true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UIPanelManager.SetLayerRecursively(componentsInChildren[i].gameObject, base.gameObject.layer);
			UIPanelBase uIPanelBase = (UIPanelBase)componentsInChildren[i];
			if (uIPanelBase.RequestContainership(this))
			{
				this.panels.Add(uIPanelBase);
			}
		}
		this.panels.Sort(new Comparison<UIPanelBase>(UIPanelBase.CompareIndices));
	}

	public bool MoveForward()
	{
		int num = this.panels.IndexOf(this.curPanel);
		if (num >= this.panels.Count - 1)
		{
			if (!this.circular)
			{
				if (this.advancePastEnd)
				{
					if (this.curPanel != null)
					{
						this.curPanel.StartTransition(UIPanelManager.SHOW_MODE.DismissForward);
					}
					this.curPanel = null;
					if (this.breadcrumbs.Count > 0)
					{
						if (this.breadcrumbs[this.breadcrumbs.Count - 1] != null)
						{
							this.breadcrumbs.Add(null);
						}
					}
					else
					{
						this.breadcrumbs.Add(null);
					}
				}
				return false;
			}
			num = -1;
		}
		if (this.curPanel != null)
		{
			this.curPanel.StartTransition(UIPanelManager.SHOW_MODE.DismissForward);
		}
		num++;
		this.curPanel = this.panels[num];
		this.breadcrumbs.Add(this.curPanel);
		if (this.deactivateAllButInitialAtStart && !this.curPanel.gameObject.activeInHierarchy)
		{
			this.curPanel.Start();
			this.curPanel.gameObject.SetActive(true);
		}
		this.curPanel.StartTransition(UIPanelManager.SHOW_MODE.BringInForward);
		return num < this.panels.Count - 1 || this.circular;
	}

	public bool MoveBack()
	{
		if (this.linearNavigation)
		{
			int num = this.panels.IndexOf(this.curPanel);
			if (num <= 0)
			{
				if (!this.circular)
				{
					if (this.advancePastEnd)
					{
						if (this.curPanel != null)
						{
							this.curPanel.StartTransition(UIPanelManager.SHOW_MODE.DismissBack);
						}
						this.curPanel = null;
					}
					return false;
				}
				num = this.panels.Count;
			}
			if (this.curPanel != null)
			{
				this.curPanel.StartTransition(UIPanelManager.SHOW_MODE.DismissBack);
			}
			num--;
			this.curPanel = this.panels[num];
			if (this.deactivateAllButInitialAtStart && !this.curPanel.gameObject.activeInHierarchy)
			{
				this.curPanel.Start();
				this.curPanel.gameObject.SetActive(true);
			}
			this.curPanel.StartTransition(UIPanelManager.SHOW_MODE.BringInBack);
			return num > 0 || this.circular;
		}
		if (this.breadcrumbs.Count <= 1)
		{
			if (this.advancePastEnd)
			{
				if (this.curPanel != null)
				{
					this.curPanel.StartTransition(UIPanelManager.SHOW_MODE.DismissBack);
				}
				this.curPanel = null;
				if (this.breadcrumbs.Count > 0)
				{
					if (this.breadcrumbs[this.breadcrumbs.Count - 1] != null)
					{
						this.breadcrumbs.Add(null);
					}
				}
				else
				{
					this.breadcrumbs.Add(null);
				}
			}
			return false;
		}
		if (this.breadcrumbs.Count != 0)
		{
			this.breadcrumbs.RemoveAt(this.breadcrumbs.Count - 1);
		}
		if (this.curPanel != null)
		{
			this.curPanel.StartTransition(UIPanelManager.SHOW_MODE.DismissBack);
		}
		if (this.breadcrumbs.Count > 0)
		{
			this.curPanel = this.breadcrumbs[this.breadcrumbs.Count - 1];
		}
		if (this.curPanel != null)
		{
			if (this.deactivateAllButInitialAtStart && !this.curPanel.gameObject.activeInHierarchy)
			{
				this.curPanel.Start();
				this.curPanel.gameObject.SetActive(true);
			}
			this.curPanel.StartTransition(UIPanelManager.SHOW_MODE.BringInBack);
		}
		return this.breadcrumbs.Count > 1;
	}

	public void BringIn(UIPanelBase panel, UIPanelManager.MENU_DIRECTION dir)
	{
		if (this.curPanel == panel)
		{
			return;
		}
		if (dir == UIPanelManager.MENU_DIRECTION.Auto)
		{
			if (this.curPanel != null)
			{
				if (this.curPanel.index <= panel.index)
				{
					dir = UIPanelManager.MENU_DIRECTION.Forwards;
				}
				else
				{
					dir = UIPanelManager.MENU_DIRECTION.Backwards;
				}
			}
			else
			{
				dir = UIPanelManager.MENU_DIRECTION.Forwards;
			}
		}
		UIPanelManager.SHOW_MODE mode = (dir != UIPanelManager.MENU_DIRECTION.Forwards) ? UIPanelManager.SHOW_MODE.DismissBack : UIPanelManager.SHOW_MODE.DismissForward;
		UIPanelManager.SHOW_MODE mode2 = (dir != UIPanelManager.MENU_DIRECTION.Forwards) ? UIPanelManager.SHOW_MODE.BringInBack : UIPanelManager.SHOW_MODE.BringInForward;
		if (this.curPanel != null)
		{
			this.curPanel.StartTransition(mode);
		}
		this.curPanel = panel;
		this.breadcrumbs.Add(this.curPanel);
		if (this.deactivateAllButInitialAtStart && !this.curPanel.gameObject.activeInHierarchy)
		{
			this.curPanel.Start();
			this.curPanel.gameObject.SetActive(true);
		}
		this.curPanel.StartTransition(mode2);
	}

	public void BringInImmediate(UIPanelBase panel, UIPanelManager.MENU_DIRECTION dir)
	{
		UIPanelBase uIPanelBase = this.curPanel;
		if (dir == UIPanelManager.MENU_DIRECTION.Auto)
		{
			if (this.curPanel != null)
			{
				if (this.curPanel.index <= panel.index)
				{
					dir = UIPanelManager.MENU_DIRECTION.Forwards;
				}
				else
				{
					dir = UIPanelManager.MENU_DIRECTION.Backwards;
				}
			}
			else
			{
				dir = UIPanelManager.MENU_DIRECTION.Forwards;
			}
		}
		UIPanelManager.SHOW_MODE transition = (dir != UIPanelManager.MENU_DIRECTION.Forwards) ? UIPanelManager.SHOW_MODE.DismissBack : UIPanelManager.SHOW_MODE.DismissForward;
		UIPanelManager.SHOW_MODE transition2 = (dir != UIPanelManager.MENU_DIRECTION.Forwards) ? UIPanelManager.SHOW_MODE.BringInBack : UIPanelManager.SHOW_MODE.BringInForward;
		this.BringIn(panel, dir);
		if (uIPanelBase != null)
		{
			EZTransition transition3 = uIPanelBase.GetTransition(transition);
			transition3.End();
		}
		if (this.curPanel != null)
		{
			EZTransition transition3 = this.curPanel.GetTransition(transition2);
			transition3.End();
		}
	}

	public void BringIn(string panelName, UIPanelManager.MENU_DIRECTION dir)
	{
		UIPanelBase uIPanelBase = null;
		for (int i = 0; i < this.panels.Count; i++)
		{
			if (string.Equals(this.panels[i].name, panelName, StringComparison.CurrentCultureIgnoreCase))
			{
				uIPanelBase = this.panels[i];
				break;
			}
		}
		if (uIPanelBase != null)
		{
			this.BringIn(uIPanelBase, dir);
		}
	}

	public void BringIn(UIPanelBase panel)
	{
		this.BringIn(panel, UIPanelManager.MENU_DIRECTION.Auto);
	}

	public void BringIn(string panelName)
	{
		this.BringIn(panelName, UIPanelManager.MENU_DIRECTION.Auto);
	}

	public void BringIn(int panelIndex)
	{
		for (int i = 0; i < this.panels.Count; i++)
		{
			if (this.panels[i].index == panelIndex)
			{
				this.BringIn(this.panels[i]);
				return;
			}
		}
		TsLog.LogWarning("No panel found with index value of " + panelIndex, new object[0]);
	}

	public void BringIn(int panelIndex, UIPanelManager.MENU_DIRECTION dir)
	{
		for (int i = 0; i < this.panels.Count; i++)
		{
			if (this.panels[i].index == panelIndex)
			{
				this.BringIn(this.panels[i], dir);
				return;
			}
		}
		TsLog.LogWarning("No panel found with index value of " + panelIndex, new object[0]);
	}

	public void BringInImmediate(string panelName, UIPanelManager.MENU_DIRECTION dir)
	{
		UIPanelBase uIPanelBase = null;
		for (int i = 0; i < this.panels.Count; i++)
		{
			if (string.Equals(this.panels[i].name, panelName, StringComparison.CurrentCultureIgnoreCase))
			{
				uIPanelBase = this.panels[i];
				break;
			}
		}
		if (uIPanelBase != null)
		{
			this.BringInImmediate(uIPanelBase, dir);
		}
	}

	public void BringInImmediate(UIPanelBase panel)
	{
		this.BringInImmediate(panel, UIPanelManager.MENU_DIRECTION.Auto);
	}

	public void BringInImmediate(string panelName)
	{
		this.BringInImmediate(panelName, UIPanelManager.MENU_DIRECTION.Auto);
	}

	public void BringInImmediate(int panelIndex)
	{
		for (int i = 0; i < this.panels.Count; i++)
		{
			if (this.panels[i].index == panelIndex)
			{
				this.BringInImmediate(this.panels[i]);
				return;
			}
		}
		TsLog.LogWarning("No panel found with index value of " + panelIndex, new object[0]);
	}

	public void BringInImmediate(int panelIndex, UIPanelManager.MENU_DIRECTION dir)
	{
		for (int i = 0; i < this.panels.Count; i++)
		{
			if (this.panels[i].index == panelIndex)
			{
				this.BringInImmediate(this.panels[i], dir);
				return;
			}
		}
		TsLog.LogWarning("No panel found with index value of " + panelIndex, new object[0]);
	}

	public void Dismiss(UIPanelManager.MENU_DIRECTION dir)
	{
		if (dir == UIPanelManager.MENU_DIRECTION.Auto)
		{
			dir = UIPanelManager.MENU_DIRECTION.Backwards;
		}
		UIPanelManager.SHOW_MODE mode = (dir != UIPanelManager.MENU_DIRECTION.Forwards) ? UIPanelManager.SHOW_MODE.DismissBack : UIPanelManager.SHOW_MODE.DismissForward;
		if (this.curPanel != null)
		{
			this.curPanel.StartTransition(mode);
		}
		this.curPanel = null;
		if (this.breadcrumbs.Count > 0 && this.breadcrumbs[this.breadcrumbs.Count - 1] != null)
		{
			this.breadcrumbs.Add(null);
		}
	}

	public void Dismiss()
	{
		this.Dismiss(UIPanelManager.MENU_DIRECTION.Auto);
	}

	public void DismissImmediate(UIPanelManager.MENU_DIRECTION dir)
	{
		if (dir == UIPanelManager.MENU_DIRECTION.Auto)
		{
			dir = UIPanelManager.MENU_DIRECTION.Backwards;
		}
		UIPanelManager.SHOW_MODE transition = (dir != UIPanelManager.MENU_DIRECTION.Forwards) ? UIPanelManager.SHOW_MODE.DismissBack : UIPanelManager.SHOW_MODE.DismissForward;
		UIPanelBase uIPanelBase = this.curPanel;
		this.Dismiss(dir);
		if (uIPanelBase != null)
		{
			uIPanelBase.GetTransition(transition).End();
		}
	}

	public void DismissImmediate()
	{
		this.DismissImmediate(UIPanelManager.MENU_DIRECTION.Auto);
	}

	public static void SetLayerRecursively(GameObject go, int layer)
	{
		go.layer = layer;
		foreach (Transform transform in go.transform)
		{
			UIPanelManager.SetLayerRecursively(transform.gameObject, layer);
		}
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

	public static UIPanelManager Create(string name, Vector3 pos)
	{
		return (UIPanelManager)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIPanelManager));
	}

	public int GetGameObjectCount()
	{
		return this.panels.Count;
	}

	public void DepthChange(UIPanelBase panel)
	{
		if (panel.topMost)
		{
			return;
		}
		this.focusPanel = panel;
		if (!panel.depthChangeable)
		{
			return;
		}
		if (panel == this.CurrentPanel)
		{
			return;
		}
		this.panels.Remove(panel);
		this.panels.Add(panel);
		UIPanelBase uIPanelBase = null;
		foreach (UIPanelBase current in this.panels)
		{
			if (!(current == null))
			{
				if (!(current == panel))
				{
					if (current.name == "TOOTIP_DLG")
					{
						uIPanelBase = current;
						break;
					}
				}
			}
		}
		if (null != uIPanelBase)
		{
			this.panels.Remove(uIPanelBase);
			this.panels.Add(uIPanelBase);
		}
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			this.highestZValue = UIPanelManager.SCENE_DEPTH;
		}
		else
		{
			this.highestZValue = UIPanelManager.UI_DEPTH;
		}
		foreach (UIPanelBase current2 in this.panels)
		{
			if (!(current2 == null) && !(current2.gameObject == null))
			{
				if (!current2.topMost)
				{
					if (!current2.depthChangeable)
					{
						this.highestZValue -= UIPanelManager.UI_DEPTH_DecreValue;
					}
					else if (current2.transform.position.y != -2000f)
					{
						current2.transform.position = new Vector3(current2.transform.position.x, current2.transform.position.y, this.highestZValue);
						this.highestZValue -= UIPanelManager.UI_DEPTH_DecreValue;
					}
				}
			}
		}
	}

	public bool IsTopMost(UIPanelBase panel)
	{
		foreach (UIPanelBase current in this.panels)
		{
			if (current == panel && current.topMost)
			{
				bool result = true;
				return result;
			}
			if (current != panel && current.topMost)
			{
				bool result = false;
				return result;
			}
		}
		return false;
	}

	public List<UIPanelBase> GetListPanel()
	{
		return this.panels;
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
