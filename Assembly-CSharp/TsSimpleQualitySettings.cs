using System;
using System.Collections.Generic;
using UnityEngine;

public class TsSimpleQualitySettings : MonoBehaviour
{
	private List<Light> m_Lights;

	public bool m_SupportRealTimeShadow;

	private void AddLight(Light light)
	{
		TsLightMarker tsLightMarker = light.GetComponent<TsLightMarker>();
		if (tsLightMarker != null && tsLightMarker.TargetLight != light)
		{
			UnityEngine.Object.Destroy(tsLightMarker);
			tsLightMarker = null;
		}
		if (tsLightMarker == null && light.shadows != LightShadows.None)
		{
			tsLightMarker = light.gameObject.AddComponent<TsLightMarker>();
			tsLightMarker.TargetLight = light;
		}
		if (tsLightMarker != null)
		{
			this.m_Lights.Add(light);
		}
	}

	private void CollecLights()
	{
		this.m_Lights.Clear();
		Light[] array = UnityEngine.Object.FindObjectsOfType(typeof(Light)) as Light[];
		Light[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Light light = array2[i];
			this.AddLight(light);
		}
	}

	private void Awake()
	{
		this.m_Lights = new List<Light>();
	}

	private void Start()
	{
		if (this.m_SupportRealTimeShadow)
		{
			this.CollecLights();
		}
	}

	private void OnEnable()
	{
		ITsLightCollector tsLightCollector = TsQualityManager.Instance as ITsLightCollector;
		if (tsLightCollector != null)
		{
			tsLightCollector.ChnageCollector(new Func<IEnumerable<Light>>(this.GetLights));
		}
	}

	private void OnDisalbe()
	{
		ITsLightCollector tsLightCollector = TsQualityManager.Instance as ITsLightCollector;
		if (tsLightCollector != null)
		{
			tsLightCollector.Clear();
		}
	}

	public IEnumerable<Light> GetLights()
	{
		return this.m_Lights;
	}
}
