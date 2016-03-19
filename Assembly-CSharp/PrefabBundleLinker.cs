using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBundleLinker : MonoBehaviour
{
	public List<PrefabGameObject> m_PrefabGameObject;

	public void Add(string PrefabName, GameObject go)
	{
		if (this.m_PrefabGameObject == null)
		{
			this.m_PrefabGameObject = new List<PrefabGameObject>();
		}
		PrefabGameObject prefabGameObject = null;
		foreach (PrefabGameObject current in this.m_PrefabGameObject)
		{
			if (current.m_PrefabName == PrefabName)
			{
				prefabGameObject = current;
			}
		}
		if (prefabGameObject == null)
		{
			prefabGameObject = new PrefabGameObject();
			prefabGameObject.m_PrefabName = PrefabName;
			prefabGameObject.m_PrefabGameObjectList = new List<GameObject>();
			this.m_PrefabGameObject.Add(prefabGameObject);
		}
		prefabGameObject.m_PrefabGameObjectList.Add(go);
	}

	public void GameObjectActive(bool bActive)
	{
		foreach (PrefabGameObject current in this.m_PrefabGameObject)
		{
			foreach (GameObject current2 in current.m_PrefabGameObjectList)
			{
				current2.SetActive(bActive);
			}
		}
	}
}
