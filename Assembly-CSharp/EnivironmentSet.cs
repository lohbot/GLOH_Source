using System;
using UnityEngine;

public class EnivironmentSet : MonoBehaviour
{
	public float m_FOGDensity = 0.0015f;

	public Color m_FOGCOLOR = Color.white;

	public Color m_AMBIENTCOLOR = Color.white;

	public string m_SkyBoxName;

	private void Start()
	{
		RenderSettings.fogColor = this.m_FOGCOLOR;
		RenderSettings.fogDensity = this.m_FOGDensity;
		RenderSettings.ambientLight = this.m_AMBIENTCOLOR;
	}

	private void Update()
	{
		RenderSettings.fogColor = this.m_FOGCOLOR;
		RenderSettings.fogDensity = this.m_FOGDensity;
		RenderSettings.ambientLight = this.m_AMBIENTCOLOR;
	}
}
