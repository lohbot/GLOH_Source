using System;
using UnityEngine;

public class CustomQuality
{
	private static CustomQuality instance;

	public static CustomQuality GetInstance()
	{
		if (CustomQuality.instance == null)
		{
			CustomQuality.instance = new CustomQuality();
		}
		return CustomQuality.instance;
	}

	public void SetQualityScene()
	{
		this.SetQualitySettings(TsQualityManager.Instance.CurrLevel);
		TsQualityManager.Instance.Refresh();
	}

	public void SetQualitySettings(TsQualityManager.Level level)
	{
		this.ChangeCameraClipPlane(level);
		if (level == TsQualityManager.Instance.CurrLevel)
		{
			return;
		}
		StageSelectChar.OnChangeQualitySettings(level);
		NrTSingleton<NkCharManager>.Instance.SetCharLODByQualityLevel(level);
		TsQualityManager.Instance.CurrLevel = level;
	}

	public void ChangeCameraClipPlane(TsQualityManager.Level level)
	{
		if (null == Camera.main)
		{
			return;
		}
		maxCamera component = Camera.main.GetComponent<maxCamera>();
		if (null != component)
		{
			Camera component2 = component.GetComponent<Camera>();
			if (component2)
			{
				component2.farClipPlane = TsQualityManager.Instance[level].CamFar;
				component2.nearClipPlane = 0.1f;
			}
		}
	}
}
