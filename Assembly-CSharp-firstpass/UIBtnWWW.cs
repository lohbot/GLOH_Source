using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Weblink Button")]
public class UIBtnWWW : UIButton
{
	public string URL;

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
			base.Invoke("DoURL", this.delay);
		}
	}

	protected void DoURL()
	{
		NrNewOpenURL.NewOpenHelp_URL(this.URL);
	}

	public override void Copy(SpriteRoot s)
	{
		this.Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);
		if (!(s is UIBtnWWW))
		{
			return;
		}
		UIBtnWWW uIBtnWWW = (UIBtnWWW)s;
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.URL = uIBtnWWW.URL;
		}
	}

	public new static UIBtnWWW Create(string name, Vector3 pos)
	{
		return (UIBtnWWW)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIBtnWWW));
	}

	public new static UIBtnWWW Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIBtnWWW)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIBtnWWW));
	}
}
