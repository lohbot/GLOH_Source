using System;
using UnityEngine;

public class EZScreenPlacementMirror
{
	public Vector3 worldPos;

	public Vector3 screenPos;

	public EZScreenPlacement.RelativeTo relativeTo;

	public Transform relativeObject;

	public Camera renderCamera;

	public EZScreenPlacementMirror()
	{
		this.relativeTo = new EZScreenPlacement.RelativeTo(null);
	}

	public virtual void Mirror(EZScreenPlacement sp)
	{
		this.worldPos = sp.transform.position;
		this.screenPos = sp.screenPos;
		this.relativeTo.Copy(sp.relativeTo);
		this.relativeObject = sp.relativeObject;
		this.renderCamera = sp.renderCamera;
	}

	public virtual bool Validate(EZScreenPlacement sp)
	{
		if (sp.relativeTo.horizontal != EZScreenPlacement.HORIZONTAL_ALIGN.OBJECT && sp.relativeTo.vertical != EZScreenPlacement.VERTICAL_ALIGN.OBJECT)
		{
			sp.relativeObject = null;
		}
		if (sp.relativeObject != null && !EZScreenPlacement.TestDepenency(sp))
		{
			TsLog.LogError(string.Concat(new string[]
			{
				"ERROR: The Relative Object you recently assigned on \"",
				sp.name,
				"\" which points to \"",
				sp.relativeObject.name,
				"\" would create a circular dependency.  Please check your placement dependencies to resolve this."
			}), new object[0]);
			sp.relativeObject = null;
		}
		return true;
	}

	public virtual bool DidChange(EZScreenPlacement sp)
	{
		if (this.worldPos != sp.transform.position)
		{
			sp.WorldToScreenPos(sp.transform.position);
			return true;
		}
		return this.screenPos != sp.screenPos || !this.relativeTo.Equals(sp.relativeTo) || this.renderCamera != sp.renderCamera || this.relativeObject != sp.relativeObject;
	}
}
