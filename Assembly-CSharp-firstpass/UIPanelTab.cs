using GameMessage;
using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Panel Tab")]
public class UIPanelTab : UIRadioBtn
{
	public bool toggle;

	public UIPanelManager panelManager;

	public UIPanelBase panel;

	public UIInteractivePanel parentPanel;

	public bool panelShowingAtStart;

	protected bool panelIsShowing = true;

	protected bool internalCall;

	private string oldColorText = string.Empty;

	private string orgText = string.Empty;

	public override bool Value
	{
		get
		{
			return base.Value;
		}
		set
		{
			base.Value = value;
			if (!this.toggle)
			{
				if (this.panelIsShowing == value)
				{
					return;
				}
			}
			else if (this.internalCall)
			{
				return;
			}
			if (this.panelManager != null)
			{
				if (value)
				{
					this.panelManager.BringIn(this.panel, UIPanelManager.MENU_DIRECTION.Forwards);
				}
				else if (this.panelManager.CurrentPanel == this.panel)
				{
					this.panelManager.Dismiss();
				}
				this.panelManager.CurrentPanel = this.panel;
			}
			else if (value)
			{
				this.panel.BringIn();
			}
			else
			{
				this.panel.Dismiss();
			}
			this.panelIsShowing = value;
		}
	}

	public EZValueChangedDelegate ButtonClick
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

	public override bool controlIsEnabled
	{
		get
		{
			return this.m_controlIsEnabled;
		}
		set
		{
			base.UpdateText = true;
			this.m_controlIsEnabled = value;
			if (!value)
			{
				base.DisableMe();
				if (this.orgText == string.Empty)
				{
					this.oldColorText = base.ColorText;
					this.orgText = this.Text;
				}
				base.ColorText = "[#7F7F7FFF]";
				this.Text = this.orgText;
			}
			else
			{
				this.SetButtonState();
				if (string.Empty != this.oldColorText)
				{
					base.ColorText = this.oldColorText;
					this.Text = this.orgText;
				}
			}
		}
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
			if (this.panelManager == null && this.panel != null && this.panel.Container != null)
			{
				this.panelManager = (UIPanelManager)this.panel.Container;
			}
			this.panelIsShowing = this.panelShowingAtStart;
			this.Value = this.panelShowingAtStart;
		}
		if (this.managed && this.m_hidden)
		{
			this.Hide(true);
		}
	}

	public override void OnInput(ref POINTER_INFO ptr)
	{
		if (this.deleted)
		{
			return;
		}
		this.internalCall = true;
		base.OnInput(ref ptr);
		if (!this.m_controlIsEnabled || base.IsHidden())
		{
			return;
		}
		if (this.panel == null)
		{
			return;
		}
		if (ptr.evt == this.whenToInvoke)
		{
			this.DoPanelStuff();
			MsgHandler.Handle("ToolbarSound", new object[0]);
		}
		this.internalCall = false;
	}

	protected void DoPanelStuff()
	{
		if (this.toggle)
		{
			if (this.panelManager != null)
			{
				if (this.panelManager.CurrentPanel == this.panel)
				{
					this.panelManager.Dismiss(UIPanelManager.MENU_DIRECTION.Forwards);
					this.panelIsShowing = false;
				}
				else
				{
					this.panelManager.BringIn(this.panel);
					this.panelIsShowing = true;
				}
				this.panelManager.CurrentPanel = this.panel;
			}
			else
			{
				if (this.panelIsShowing)
				{
					this.panel.Dismiss();
				}
				else
				{
					this.panel.BringIn();
				}
				this.panelIsShowing = !this.panelIsShowing;
			}
			base.Value = this.panelIsShowing;
		}
		else if (this.panelManager != null)
		{
			this.panelManager.BringIn(this.panel, UIPanelManager.MENU_DIRECTION.Forwards);
			this.panelManager.CurrentPanel = this.panel;
		}
		else
		{
			this.panel.BringIn();
		}
	}

	public override void Copy(SpriteRoot s)
	{
		this.Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);
		if (!(s is UIPanelTab))
		{
			return;
		}
		UIPanelTab uIPanelTab = (UIPanelTab)s;
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.toggle = uIPanelTab.toggle;
			this.panelManager = uIPanelTab.panelManager;
			this.panel = uIPanelTab.panel;
			this.panelShowingAtStart = uIPanelTab.panelShowingAtStart;
		}
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			this.Value = uIPanelTab.Value;
		}
	}

	public new static UIPanelTab Create(string name, Vector3 pos)
	{
		return (UIPanelTab)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIPanelTab));
	}

	public new static UIPanelTab Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIPanelTab)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIPanelTab));
	}
}
