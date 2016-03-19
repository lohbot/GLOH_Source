using System;
using UnityEngine;

public interface ITsQualityManager
{
	event Action UserSetting;

	TsQualityManager.Level CurrLevel
	{
		get;
		set;
	}

	ITsGameQuality CurrQuality
	{
		get;
	}

	ITsGameQuality this[int index]
	{
		get;
	}

	ITsGameQuality this[TsQualityManager.Level level]
	{
		get;
	}

	bool IsCustomMode
	{
		get;
	}

	int ShaderMaxLOD
	{
		get;
	}

	void CollectShader(GameObject modelRoot);

	void CollectShader(Shader shader);

	void Refresh();

	bool SaveCustomSettings();

	void RecoveryCustomSettings();

	bool RegisterScript(MonoBehaviour target, TsQualityManager.Level startLevel);

	void ReleaseScript(MonoBehaviour target);

	void AutoEnableScript(MonoBehaviour target);
}
