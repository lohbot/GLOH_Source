using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Action Button")]
public class UIActionBtn : UIButton
{
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
		if (this.repeat)
		{
			NrTSingleton<UIManager>.Instance.RayActive = ((base.controlState != UIButton.CONTROL_STATE.ACTIVE) ? UIManager.RAY_ACTIVE_STATE.Inactive : UIManager.RAY_ACTIVE_STATE.Constant);
		}
		else if (ptr.evt == this.whenToInvoke)
		{
			NrTSingleton<UIManager>.Instance.RayActive = UIManager.RAY_ACTIVE_STATE.Momentary;
		}
		base.OnInput(ref ptr);
	}

	public new static UIActionBtn Create(string name, Vector3 pos)
	{
		return (UIActionBtn)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIActionBtn));
	}

	public new static UIActionBtn Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIActionBtn)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIActionBtn));
	}
}
