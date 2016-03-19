using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefabInfo
{
	public UnityEngine.Object m_PrefabObject;

	public string m_PrefabPath = string.Empty;

	public List<GameObject> m_GameObjectList = new List<GameObject>();

	public void Set(UnityEngine.Object prefab, string prefabpath)
	{
		this.m_PrefabObject = prefab;
		this.m_PrefabPath = prefabpath;
	}

	public void Add(GameObject go)
	{
		if (!this.m_GameObjectList.Contains(go))
		{
			this.m_GameObjectList.Add(go);
		}
	}
}
