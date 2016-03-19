using System;
using UnityEngine;

[Serializable]
public class RenderSettingValue
{
	public enum _Time
	{
		DAYTIME,
		NIGHTTIME
	}

	public Color ambientLight = Color.black;

	public float flareStrength;

	public bool fog;

	public Color fogColor = Color.black;

	public float fogDensity;

	public float fogEndDistance;

	public FogMode fogMode = FogMode.Exponential;

	public float fogStartDistance;

	public float haloStrength;

	public Material skybox;

	public RenderSettingValue._Time time;

	public void Set(RenderSettingValue SettingValue)
	{
		this.ambientLight = SettingValue.ambientLight;
		this.flareStrength = SettingValue.flareStrength;
		this.fog = SettingValue.fog;
		this.fogColor = SettingValue.fogColor;
		this.fogDensity = SettingValue.fogDensity;
		this.fogEndDistance = SettingValue.fogEndDistance;
		this.fogMode = SettingValue.fogMode;
		this.fogStartDistance = SettingValue.fogStartDistance;
		this.haloStrength = SettingValue.haloStrength;
		this.skybox = SettingValue.skybox;
		this.time = SettingValue.time;
	}
}
