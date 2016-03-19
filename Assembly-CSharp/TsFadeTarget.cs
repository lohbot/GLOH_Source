using System;
using UnityEngine;

[AddComponentMenu("TsFadeBuilding/TsFadeTarget")]
public class TsFadeTarget : MonoBehaviour
{
	private void Start()
	{
		TsFadeRayCaster tsFadeRayCaster = UnityEngine.Object.FindObjectOfType(typeof(TsFadeRayCaster)) as TsFadeRayCaster;
		if (tsFadeRayCaster != null && tsFadeRayCaster.fadeObjectContainer != null)
		{
			tsFadeRayCaster.fadeObjectContainer.AddFadeTarget(base.gameObject);
		}
	}
}
