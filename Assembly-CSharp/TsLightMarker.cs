using System;
using UnityEngine;

public class TsLightMarker : MonoBehaviour
{
	private LightShadows m_OriginalShadowType;

	private Light m_TargetLight;

	public Light TargetLight
	{
		get
		{
			return this.m_TargetLight;
		}
		set
		{
			if (base.light != null)
			{
				this.m_OriginalShadowType = base.light.shadows;
				this.m_TargetLight = value;
				if (TsPlatform.IsEditor)
				{
					base.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSave | HideFlags.NotEditable);
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (base.light != null && base.light == this.TargetLight)
		{
			base.light.shadows = this.m_OriginalShadowType;
		}
	}
}
