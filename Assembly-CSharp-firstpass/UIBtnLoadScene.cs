using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Load Scene Button")]
public class UIBtnLoadScene : UIButton
{
	public string scene;

	public UIPanelBase loadingPanel;

	public void LoadSceneDelegate(UIPanelBase panel, EZTransition trans)
	{
		base.StartCoroutine(this.LoadScene());
	}

	public override void OnInput(ref POINTER_INFO ptr)
	{
		if (this.deleted)
		{
			return;
		}
		base.OnInput(ref ptr);
		if (!this.m_controlIsEnabled || base.IsHidden())
		{
			return;
		}
		if (ptr.evt == this.whenToInvoke)
		{
			if (this.loadingPanel != null)
			{
				UIPanelManager uIPanelManager = (UIPanelManager)this.loadingPanel.Container;
				this.loadingPanel.AddTempTransitionDelegate(new UIPanelBase.TransitionCompleteDelegate(this.LoadSceneDelegate));
				if (uIPanelManager is UIPanelManager && uIPanelManager != null)
				{
					uIPanelManager.BringIn(this.loadingPanel);
				}
				else
				{
					this.loadingPanel.StartTransition(UIPanelManager.SHOW_MODE.BringInForward);
				}
			}
			else
			{
				base.Invoke("DoLoadScene", this.delay);
			}
		}
	}

	protected void DoLoadScene()
	{
		base.StartCoroutine(this.LoadScene());
	}

	[DebuggerHidden]
	protected IEnumerator LoadScene()
	{
		UIBtnLoadScene.<LoadScene>c__Iterator4 <LoadScene>c__Iterator = new UIBtnLoadScene.<LoadScene>c__Iterator4();
		<LoadScene>c__Iterator.<>f__this = this;
		return <LoadScene>c__Iterator;
	}

	public override void Copy(SpriteRoot s)
	{
		this.Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);
		if (!(s is UIBtnLoadScene))
		{
			return;
		}
		UIBtnLoadScene uIBtnLoadScene = (UIBtnLoadScene)s;
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.scene = uIBtnLoadScene.scene;
			this.loadingPanel = uIBtnLoadScene.loadingPanel;
		}
	}

	public new static UIBtnLoadScene Create(string name, Vector3 pos)
	{
		return (UIBtnLoadScene)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIBtnLoadScene));
	}

	public new static UIBtnLoadScene Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIBtnLoadScene)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIBtnLoadScene));
	}
}
