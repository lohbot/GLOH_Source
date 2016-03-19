using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Panels/Interactive Panel")]
[Serializable]
public class UIInteractivePanel : UIPanelBase
{
	private enum eFormMode
	{
		PARENT,
		TWIN,
		CHILD
	}

	public enum STATE
	{
		NORMAL,
		OVER,
		DRAGGING
	}

	protected UIInteractivePanel.STATE m_panelState;

	[HideInInspector]
	public EZTransitionList transitions = new EZTransitionList(new EZTransition[]
	{
		new EZTransition("Bring In Forward"),
		new EZTransition("Bring In Back"),
		new EZTransition("Dismiss Forward"),
		new EZTransition("Dismiss Back"),
		new EZTransition("Normal from Over"),
		new EZTransition("Normal from Dragging"),
		new EZTransition("Over from Normal"),
		new EZTransition("Over from Dragging"),
		new EZTransition("Dragging")
	});

	public bool draggable;

	public bool constrainDragArea;

	public Vector3 dragBoundaryMin;

	public Vector3 dragBoundaryMax;

	public UIPanelManager panelManager;

	public string resourceFileName = "TEST!!!!";

	public UIInteractivePanel.STATE State
	{
		get
		{
			return this.m_panelState;
		}
	}

	public override EZTransitionList Transitions
	{
		get
		{
			return this.transitions;
		}
	}

	public bool IsFocus()
	{
		return this.panelManager && this.panelManager.FocusPanel == this;
	}

	public override void OnInput(POINTER_INFO ptr)
	{
		if (null == this)
		{
			return;
		}
		if (!this.m_controlIsEnabled)
		{
			return;
		}
		if (this.inputDelegate != null)
		{
			this.inputDelegate(ref ptr);
		}
		POINTER_INFO.INPUT_EVENT evt = ptr.evt;
		switch (evt)
		{
		case POINTER_INFO.INPUT_EVENT.MOVE:
			if (this.panelManager == null)
			{
				this.panelManager = UIPanelManager.instance;
			}
			if (base.collider != null)
			{
				this.panelManager.MouseOverPanel = this;
			}
			if (this.m_panelState != UIInteractivePanel.STATE.OVER)
			{
				this.SetPanelState(UIInteractivePanel.STATE.OVER);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			if (base.collider != null)
			{
				RaycastHit raycastHit;
				if (!base.collider.Raycast(ptr.ray, out raycastHit, ptr.rayDepth))
				{
					this.SetPanelState(UIInteractivePanel.STATE.NORMAL);
					if (this.panelManager == null)
					{
						this.panelManager = UIPanelManager.instance;
					}
					this.panelManager.MouseOverPanel = null;
				}
				else if (ptr.evt == POINTER_INFO.INPUT_EVENT.MOVE_OFF)
				{
					ptr.evt = POINTER_INFO.INPUT_EVENT.MOVE;
				}
				else
				{
					ptr.evt = POINTER_INFO.INPUT_EVENT.RELEASE;
				}
			}
			break;
		case POINTER_INFO.INPUT_EVENT.DRAG:
			if (this.draggable && !ptr.callerIsControl)
			{
				if (ptr.inputDelta.sqrMagnitude != 0f)
				{
					if (this.panelManager == null)
					{
						this.panelManager = UIPanelManager.instance;
					}
					if (base.collider != null)
					{
						this.panelManager.MouseOverPanel = this;
					}
					Plane plane = default(Plane);
					plane.SetNormalAndPosition(base.transform.forward * -1f, base.transform.position);
					float d;
					plane.Raycast(ptr.ray, out d);
					Vector3 vector = ptr.ray.origin + ptr.ray.direction * d;
					plane.Raycast(ptr.prevRay, out d);
					Vector3 b = ptr.prevRay.origin + ptr.prevRay.direction * d;
					vector = base.transform.position + base.transform.InverseTransformDirection(vector - b);
					if (this.constrainDragArea)
					{
						vector.x = Mathf.Clamp(vector.x, this.dragBoundaryMin.x, this.dragBoundaryMax.x);
						vector.y = Mathf.Clamp(vector.y, this.dragBoundaryMin.y, this.dragBoundaryMax.y);
						vector.z = Mathf.Clamp(vector.z, this.dragBoundaryMin.z, this.dragBoundaryMax.z);
					}
					base.transform.position = vector;
					if (this.twinFormID != G_ID.NONE)
					{
						UIInteractivePanel uIInteractivePanel = this.panelManager.FindPanel((int)this.twinFormID) as UIInteractivePanel;
						if (null != uIInteractivePanel)
						{
							plane = default(Plane);
							plane.SetNormalAndPosition(uIInteractivePanel.transform.forward * -1f, uIInteractivePanel.transform.position);
							plane.Raycast(ptr.ray, out d);
							vector = ptr.ray.origin + ptr.ray.direction * d;
							plane.Raycast(ptr.prevRay, out d);
							b = ptr.prevRay.origin + ptr.prevRay.direction * d;
							vector = uIInteractivePanel.transform.position + uIInteractivePanel.transform.InverseTransformDirection(vector - b);
							if (uIInteractivePanel.constrainDragArea)
							{
								vector.x = Mathf.Clamp(vector.x, uIInteractivePanel.dragBoundaryMin.x, uIInteractivePanel.dragBoundaryMax.x);
								vector.y = Mathf.Clamp(vector.y, uIInteractivePanel.dragBoundaryMin.y, uIInteractivePanel.dragBoundaryMax.y);
								vector.z = Mathf.Clamp(vector.z, uIInteractivePanel.dragBoundaryMin.z, uIInteractivePanel.dragBoundaryMax.z);
							}
							uIInteractivePanel.transform.position = vector;
							uIInteractivePanel.SetPanelState(UIInteractivePanel.STATE.DRAGGING);
						}
					}
					this.MoveChild(this.childFormID_0, this.childFormID_1);
					this.SetPanelState(UIInteractivePanel.STATE.DRAGGING);
				}
			}
			break;
		default:
			if (evt == POINTER_INFO.INPUT_EVENT.PRESS)
			{
				if (this.panelManager == null)
				{
					this.panelManager = UIPanelManager.instance;
				}
				if (base.collider != null)
				{
					this.panelManager.FocusPanel = this;
					this.panelManager.MouseOverPanel = this;
				}
			}
			break;
		}
	}

	protected void SetPanelState(UIInteractivePanel.STATE s)
	{
		if (this.m_panelState == s)
		{
			return;
		}
		UIInteractivePanel.STATE panelState = this.m_panelState;
		this.m_panelState = s;
		if (this.prevTransition != null)
		{
			this.prevTransition.StopSafe();
		}
		this.StartTransition(s, panelState);
	}

	protected void StartTransition(UIInteractivePanel.STATE s, UIInteractivePanel.STATE prevState)
	{
		int num;
		switch (s)
		{
		case UIInteractivePanel.STATE.NORMAL:
			if (prevState == UIInteractivePanel.STATE.OVER)
			{
				num = 4;
			}
			else
			{
				num = 5;
			}
			break;
		case UIInteractivePanel.STATE.OVER:
			if (prevState == UIInteractivePanel.STATE.NORMAL)
			{
				num = 6;
			}
			else
			{
				num = 7;
			}
			break;
		case UIInteractivePanel.STATE.DRAGGING:
			num = 8;
			break;
		default:
			num = 4;
			break;
		}
		this.prevTransition = this.transitions.list[num];
		this.prevTransition.Start();
	}

	public void Hide()
	{
		this.StartTransition(UIPanelManager.SHOW_MODE.DismissForward);
	}

	public void Reveal()
	{
		this.StartTransition(UIPanelManager.SHOW_MODE.BringInForward);
	}

	public static UIInteractivePanel Create(string name, Vector3 pos)
	{
		return (UIInteractivePanel)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIInteractivePanel));
	}

	public static UIInteractivePanel Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIInteractivePanel)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIInteractivePanel));
	}

	public void MoveChild()
	{
		this.MoveChild(this.childFormID_0, this.childFormID_1);
	}

	private void MoveChild(G_ID child0, G_ID child1)
	{
	}

	public void SetAlpha(float value)
	{
		foreach (IUIObject current in this.uiObjs.Values)
		{
			if (current is AutoSpriteControlBase)
			{
				((AutoSpriteControlBase)current).SetAlpha(value);
			}
			else if (current is UIScrollList)
			{
				UIScrollList uIScrollList = (UIScrollList)current;
				uIScrollList.SetAlpha(value);
			}
		}
	}
}
