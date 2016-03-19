using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModelPrefabInfo
{
	public string m_PrefabName;

	public string m_PrefabAssetPath;

	public UnityEngine.Object m_Prefab;

	public Vector3 m_PrefabScale = Vector3.one;

	public List<ModelInfo> m_ModelInfoList;
}
