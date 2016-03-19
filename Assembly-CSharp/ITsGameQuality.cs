using System;
using UnityEngine;

public interface ITsGameQuality
{
	bool DepthOfField
	{
		get;
		set;
	}

	bool Bloom
	{
		get;
		set;
	}

	float TerrainPixelErrorScale
	{
		get;
		set;
	}

	TsQualityManager.TextureQuality TextureQuality
	{
		get;
		set;
	}

	bool EnableShadow
	{
		get;
		set;
	}

	int ShaderMaxLOD
	{
		get;
	}

	bool IsSupportShadow
	{
		get;
	}

	LightShadows ShadowType
	{
		get;
	}

	float CamFar
	{
		get;
	}
}
