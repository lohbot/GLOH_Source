using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Change Panel Button")]
public class UIBtnChangePanel : UIButton
{
	public enum ChangeType
	{
		BringIn,
		BringInForward,
		BringInBack,
		Dismiss,
		DismissCurrent,
		Toggle,
		Forward,
		Back,
		BringInImmediate,
		DismissImmediate
	}

	public UIPanelManager panelManager;

	public UIBtnChangePanel.ChangeType changeType;

	public string panel;

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
		if (ptr.evt == this.whenToInvoke)
		{
			if (this.panelManager == null)
			{
				if (UIPanelManager.instance == null)
				{
					base.OnInput(ref ptr);
					return;
				}
				this.panelManager = UIPanelManager.instance;
				if (this.panelManager == null)
				{
					base.OnInput(ref ptr);
					return;
				}
			}
			switch (this.changeType)
			{
			case UIBtnChangePanel.ChangeType.BringIn:
				this.panelManager.BringIn(this.panel);
				break;
			case UIBtnChangePanel.ChangeType.BringInForward:
				this.panelManager.BringIn(this.panel, UIPanelManager.MENU_DIRECTION.Forwards);
				break;
			case UIBtnChangePanel.ChangeType.BringInBack:
				this.panelManager.BringIn(this.panel, UIPanelManager.MENU_DIRECTION.Backwards);
				break;
			case UIBtnChangePanel.ChangeType.Dismiss:
				if (this.panelManager.CurrentPanel != null && string.Equals(this.panelManager.CurrentPanel.name, this.panel, StringComparison.CurrentCultureIgnoreCase))
				{
					this.panelManager.Dismiss(UIPanelManager.MENU_DIRECTION.Forwards);
				}
				break;
			case UIBtnChangePanel.ChangeType.DismissCurrent:
				this.panelManager.Dismiss(UIPanelManager.MENU_DIRECTION.Forwards);
				break;
			case UIBtnChangePanel.ChangeType.Toggle:
				if (this.panelManager != null && this.panelManager.CurrentPanel != null && string.Equals(this.panelManager.CurrentPanel.name, this.panel, StringComparison.CurrentCultureIgnoreCase))
				{
					this.panelManager.Dismiss(UIPanelManager.MENU_DIRECTION.Forwards);
				}
				else
				{
					this.panelManager.BringIn(this.panel);
				}
				break;
			case UIBtnChangePanel.ChangeType.Forward:
				this.panelManager.MoveForward();
				break;
			case UIBtnChangePanel.ChangeType.Back:
				this.panelManager.MoveBack();
				break;
			case UIBtnChangePanel.ChangeType.BringInImmediate:
				this.panelManager.BringInImmediate(this.panel);
				break;
			case UIBtnChangePanel.ChangeType.DismissImmediate:
				this.panelManager.DismissImmediate(UIPanelManager.MENU_DIRECTION.Forwards);
				break;
			}
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
		if (!(s is UIBtnChangePanel))
		{
			return;
		}
		UIBtnChangePanel uIBtnChangePanel = (UIBtnChangePanel)s;
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.panelManager = uIBtnChangePanel.panelManager;
			this.changeType = uIBtnChangePanel.changeType;
			this.panel = uIBtnChangePanel.panel;
		}
	}

	public new static UIBtnChangePanel Create(string name, Vector3 pos)
	{
		return (UIBtnChangePanel)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIBtnChangePanel));
	}

	public new static UIBtnChangePanel Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIBtnChangePanel)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIBtnChangePanel));
	}
}
