using System;
using UnityEngine;

[Serializable]
public class NmRenderSettings : MonoBehaviour
{
	public bool Fog;

	public Color FogColor = Color.white;

	public float FogDensity;

	public FogMode eFogMode = FogMode.Exponential;

	public Color AmbientLight = Color.white;

	public float HaloStrength;

	public float FlareStrength;

	public string SkyBoxName = string.Empty;

	public float fFogStart = 180f;

	public float fFogEnd = 3000f;

	public void BackupRenderSettings()
	{
		this.Fog = RenderSettings.fog;
		this.FogColor = RenderSettings.fogColor;
		this.FogDensity = RenderSettings.fogDensity;
		this.eFogMode = RenderSettings.fogMode;
		this.AmbientLight = RenderSettings.ambientLight;
		this.HaloStrength = RenderSettings.haloStrength;
		this.FlareStrength = RenderSettings.flareStrength;
		this.fFogStart = RenderSettings.fogStartDistance;
		this.fFogEnd = RenderSettings.fogEndDistance;
		if (RenderSettings.skybox)
		{
			this.SkyBoxName = RenderSettings.skybox.name;
		}
	}

	public void RestoreRenderSettings()
	{
		RenderSettings.fog = this.Fog;
		RenderSettings.fogColor = this.FogColor;
		RenderSettings.fogDensity = this.FogDensity;
		RenderSettings.fogMode = this.eFogMode;
		RenderSettings.ambientLight = this.AmbientLight;
		RenderSettings.haloStrength = this.HaloStrength;
		RenderSettings.flareStrength = this.FlareStrength;
		RenderSettings.fogStartDistance = this.fFogStart;
		RenderSettings.fogEndDistance = this.fFogEnd;
	}
}
