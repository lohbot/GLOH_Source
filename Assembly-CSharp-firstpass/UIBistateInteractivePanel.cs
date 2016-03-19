using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Panels/Bi-State Interactive Panel")]
[Serializable]
public class UIBistateInteractivePanel : UIPanelBase
{
	public enum STATE
	{
		SHOWING,
		HIDDEN
	}

	protected UIBistateInteractivePanel.STATE m_panelState;

	[HideInInspector]
	public EZTransitionList transitions = new EZTransitionList(new EZTransition[]
	{
		new EZTransition("Bring In Forward"),
		new EZTransition("Bring In Back"),
		new EZTransition("Dismiss Forward"),
		new EZTransition("Dismiss Back")
	});

	public bool requireTap = true;

	public bool alwaysShowOnClick = true;

	public bool dismissOnOutsideClick = true;

	public bool dismissOnPeerClick;

	public bool dismissOnChildClick;

	public bool dismissOnMoveOff;

	public bool showOnChildClick = true;

	public UIBistateInteractivePanel.STATE initialState = UIBistateInteractivePanel.STATE.HIDDEN;

	protected int lastActionID = -1;

	protected POINTER_INFO.POINTER_TYPE lastPtrType = POINTER_INFO.POINTER_TYPE.MOUSE_TOUCHPAD;

	protected POINTER_INFO.POINTER_TYPE lastListenerType = POINTER_INFO.POINTER_TYPE.MOUSE_TOUCHPAD;

	public UIBistateInteractivePanel.STATE State
	{
		get
		{
			return this.m_panelState;
		}
		set
		{
			this.SetPanelState(value);
		}
	}

	public override EZTransitionList Transitions
	{
		get
		{
			return this.transitions;
		}
	}

	public override void OnInput(POINTER_INFO ptr)
	{
		if (!this.m_controlIsEnabled)
		{
			return;
		}
		if (this.inputDelegate != null)
		{
			this.inputDelegate(ref ptr);
		}
		this.lastPtrType = ptr.type;
		POINTER_INFO.INPUT_EVENT evt = ptr.evt;
		switch (evt)
		{
		case POINTER_INFO.INPUT_EVENT.TAP:
			this.PanelClicked(ptr);
			goto IL_104;
		case POINTER_INFO.INPUT_EVENT.RIGHT_TAP:
		case POINTER_INFO.INPUT_EVENT.LONG_TAP:
		case POINTER_INFO.INPUT_EVENT.MOVE:
			IL_59:
			if (evt != POINTER_INFO.INPUT_EVENT.PRESS)
			{
				goto IL_104;
			}
			if (!this.requireTap)
			{
				this.PanelClicked(ptr);
			}
			goto IL_104;
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			if (base.collider != null)
			{
				RaycastHit raycastHit;
				if (base.collider.Raycast(ptr.ray, out raycastHit, ptr.rayDepth))
				{
					if (ptr.evt == POINTER_INFO.INPUT_EVENT.MOVE_OFF)
					{
						ptr.evt = POINTER_INFO.INPUT_EVENT.MOVE;
					}
					else
					{
						ptr.evt = POINTER_INFO.INPUT_EVENT.RELEASE;
					}
				}
				else if (this.dismissOnMoveOff && this.m_panelState == UIBistateInteractivePanel.STATE.SHOWING)
				{
					this.SetPanelState(UIBistateInteractivePanel.STATE.HIDDEN);
				}
			}
			goto IL_104;
		}
		goto IL_59;
		IL_104:
		base.OnInput(ptr);
	}

	protected void PanelClicked(POINTER_INFO ptr)
	{
		if (ptr.actionID == this.lastActionID)
		{
			return;
		}
		this.lastActionID = ptr.actionID;
		if (ptr.callerIsControl)
		{
			if (this.m_panelState == UIBistateInteractivePanel.STATE.HIDDEN && this.showOnChildClick)
			{
				this.SetPanelState(UIBistateInteractivePanel.STATE.SHOWING);
			}
			else if (this.m_panelState == UIBistateInteractivePanel.STATE.SHOWING && this.dismissOnChildClick)
			{
				this.SetPanelState(UIBistateInteractivePanel.STATE.HIDDEN);
			}
			return;
		}
		if (this.alwaysShowOnClick)
		{
			this.SetPanelState(UIBistateInteractivePanel.STATE.SHOWING);
		}
		else
		{
			this.ToggleState();
		}
	}

	public void Awake()
	{
		this.m_panelState = this.initialState;
	}

	public void ToggleState()
	{
		if (this.m_panelState == UIBistateInteractivePanel.STATE.HIDDEN)
		{
			this.SetPanelState(UIBistateInteractivePanel.STATE.SHOWING);
		}
		else
		{
			this.SetPanelState(UIBistateInteractivePanel.STATE.HIDDEN);
		}
	}

	protected void SetPanelState(UIBistateInteractivePanel.STATE s)
	{
		if (this.m_panelState == s)
		{
			return;
		}
		this.m_panelState = s;
		if (this.dismissOnPeerClick || this.dismissOnOutsideClick)
		{
			if (this.m_panelState == UIBistateInteractivePanel.STATE.SHOWING)
			{
				if ((this.lastPtrType & POINTER_INFO.POINTER_TYPE.MOUSE_TOUCHPAD) == this.lastPtrType)
				{
					NrTSingleton<UIManager>.Instance.AddMouseTouchPtrListener(new UIManager.PointerInfoDelegate(this.ClickListener));
					this.lastListenerType = POINTER_INFO.POINTER_TYPE.MOUSE_TOUCHPAD;
				}
				else
				{
					NrTSingleton<UIManager>.Instance.AddRayPtrListener(new UIManager.PointerInfoDelegate(this.ClickListener));
					this.lastListenerType = POINTER_INFO.POINTER_TYPE.MOUSE_TOUCHPAD;
				}
			}
			else if ((this.lastListenerType & POINTER_INFO.POINTER_TYPE.MOUSE_TOUCHPAD) == this.lastListenerType)
			{
				NrTSingleton<UIManager>.Instance.RemoveMouseTouchPtrListener(new UIManager.PointerInfoDelegate(this.ClickListener));
			}
			else
			{
				NrTSingleton<UIManager>.Instance.RemoveRayPtrListener(new UIManager.PointerInfoDelegate(this.ClickListener));
			}
		}
		if (this.m_panelState == UIBistateInteractivePanel.STATE.SHOWING)
		{
			base.StartTransition(UIPanelManager.SHOW_MODE.BringInForward);
		}
		else
		{
			base.StartTransition(UIPanelManager.SHOW_MODE.DismissForward);
		}
	}

	public override void StartTransition(UIPanelManager.SHOW_MODE mode)
	{
		if (mode == UIPanelManager.SHOW_MODE.BringInBack || mode == UIPanelManager.SHOW_MODE.BringInForward)
		{
			this.SetPanelState(UIBistateInteractivePanel.STATE.SHOWING);
		}
		else
		{
			this.SetPanelState(UIBistateInteractivePanel.STATE.HIDDEN);
		}
	}

	protected void ClickListener(POINTER_INFO ptr)
	{
		if (ptr.evt != POINTER_INFO.INPUT_EVENT.PRESS)
		{
			return;
		}
		if (ptr.targetObj == null && this.dismissOnOutsideClick)
		{
			this.SetPanelState(UIBistateInteractivePanel.STATE.HIDDEN);
			return;
		}
		if (!this.dismissOnPeerClick)
		{
			return;
		}
		if (ptr.targetObj is Component && ((Component)ptr.targetObj).transform.IsChildOf(base.transform))
		{
			return;
		}
		if (this.dismissOnPeerClick)
		{
			this.SetPanelState(UIBistateInteractivePanel.STATE.HIDDEN);
		}
	}

	public void Hide()
	{
		this.StartTransition(UIPanelManager.SHOW_MODE.DismissForward);
	}

	public void Reveal()
	{
		this.StartTransition(UIPanelManager.SHOW_MODE.BringInForward);
	}

	public static UIBistateInteractivePanel Create(string name, Vector3 pos)
	{
		return (UIBistateInteractivePanel)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIBistateInteractivePanel));
	}

	public static UIBistateInteractivePanel Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIBistateInteractivePanel)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIBistateInteractivePanel));
	}
}
