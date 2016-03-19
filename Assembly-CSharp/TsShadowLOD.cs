using System;
using UnityEngine;

public class TsShadowLOD : MonoBehaviour
{
	private void Start()
	{
		if (base.light == null)
		{
			base.enabled = false;
			return;
		}
	}

	private void OnEnable()
	{
		if (base.light == null)
		{
			return;
		}
		base.light.shadows = TsShadowLodManager.Instance.GetLightShadows();
	}

	private void FixedUpdate()
	{
		TsShadowLodManager instance = TsShadowLodManager.Instance;
		if (!instance.IsUpdateTime)
		{
			return;
		}
		Vector3 targetPosition = instance.TargetPosition;
		targetPosition.y = 0f;
		Vector3 position = base.transform.position;
		position.y = 0f;
		float sqrMagnitude = instance.sqrMagnitude;
		float sqrMagnitude2 = (position - targetPosition).sqrMagnitude;
		if (sqrMagnitude2 > sqrMagnitude)
		{
			base.light.shadows = LightShadows.None;
		}
		else
		{
			base.light.shadows = instance.GetLightShadows();
		}
	}

	private void ChnageShadowType()
	{
		if (base.light != null)
		{
			base.light.shadows = TsShadowLodManager.Instance.GetLightShadows();
		}
	}
}
