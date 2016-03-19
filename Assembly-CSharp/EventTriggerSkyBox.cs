using System;
using UnityEngine;

public class EventTriggerSkyBox : MonoBehaviour
{
	public string m_SkyBoxName = string.Empty;

	public string m_SkyBoxAssetPath = string.Empty;

	public RenderSettingValue m_MapRenderSetting = new RenderSettingValue();

	public void SetRenderSettingValue()
	{
		this.m_MapRenderSetting.ambientLight = RenderSettings.ambientLight;
		this.m_MapRenderSetting.flareStrength = RenderSettings.flareStrength;
		this.m_MapRenderSetting.fog = RenderSettings.fog;
		this.m_MapRenderSetting.fogColor = RenderSettings.fogColor;
		this.m_MapRenderSetting.fogDensity = RenderSettings.fogDensity;
		this.m_MapRenderSetting.fogEndDistance = RenderSettings.fogEndDistance;
		this.m_MapRenderSetting.fogMode = RenderSettings.fogMode;
		this.m_MapRenderSetting.fogStartDistance = RenderSettings.fogStartDistance;
		this.m_MapRenderSetting.haloStrength = RenderSettings.haloStrength;
		this.m_MapRenderSetting.skybox = RenderSettings.skybox;
	}

	public void SetRenderSetting()
	{
		EnivironmentSet enivironmentSet = UnityEngine.Object.FindObjectOfType(typeof(EnivironmentSet)) as EnivironmentSet;
		if (enivironmentSet != null)
		{
			enivironmentSet.enabled = false;
		}
		RenderSettings.ambientLight = this.m_MapRenderSetting.ambientLight;
		RenderSettings.flareStrength = this.m_MapRenderSetting.flareStrength;
		RenderSettings.fog = this.m_MapRenderSetting.fog;
		RenderSettings.fogColor = this.m_MapRenderSetting.fogColor;
		RenderSettings.fogDensity = this.m_MapRenderSetting.fogDensity;
		RenderSettings.fogEndDistance = this.m_MapRenderSetting.fogEndDistance;
		RenderSettings.fogMode = this.m_MapRenderSetting.fogMode;
		RenderSettings.fogStartDistance = this.m_MapRenderSetting.fogStartDistance;
		RenderSettings.haloStrength = this.m_MapRenderSetting.haloStrength;
		RenderSettings.skybox = this.m_MapRenderSetting.skybox;
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.Log("Set RenderSettings", new object[0]);
		}
	}

	private void Start()
	{
		SkinnedMeshRenderer[] array = UnityEngine.Object.FindObjectsOfType(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer[];
		if (array == null)
		{
			return;
		}
		SkinnedMeshRenderer[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = array2[i];
			if (skinnedMeshRenderer.material.shader.name == "TK/TK_Char_Diffuse_rim_dye")
			{
				if (this.m_MapRenderSetting.time == RenderSettingValue._Time.NIGHTTIME)
				{
					skinnedMeshRenderer.material.SetFloat("_Illumination", 0.1f);
					skinnedMeshRenderer.material.SetColor("_RimColor2", new Color(0.01953125f, 0.01953125f, 0.01953125f, 0f));
					skinnedMeshRenderer.material.SetFloat("_RimPower2", 5f);
				}
				else
				{
					skinnedMeshRenderer.material.SetFloat("_Illumination", 0.5f);
					skinnedMeshRenderer.material.SetColor("_RimColor2", new Color(0.140625f, 0.140625f, 0.12109375f, 0f));
					skinnedMeshRenderer.material.SetFloat("_RimPower2", 3f);
				}
				skinnedMeshRenderer.material.SetColor("_RimColor2", new Color(0f, 0f, 0f, 0f));
				skinnedMeshRenderer.material.SetFloat("_RimPower2", 0f);
			}
		}
	}
}
