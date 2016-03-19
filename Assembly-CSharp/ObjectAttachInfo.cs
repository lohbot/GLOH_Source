using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectAttachInfo
{
	public UnityEngine.Object m_AttachObject;

	public string m_BoneName = string.Empty;

	public List<GameObject> m_ParentObjectList;

	public string m_PrefabName = string.Empty;

	public string m_PrefabAssetPath = string.Empty;

	private GameObject CreateObject()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(this.m_AttachObject) as GameObject;
		gameObject.name = this.m_AttachObject.name;
		return gameObject;
	}

	private void AttachObject(GameObject goParent, GameObject goObject)
	{
		if (goParent == null || goObject == null)
		{
			return;
		}
		Transform transform = goParent.transform.FindChild(this.m_BoneName);
		if (transform == null)
		{
			transform = goParent.transform;
		}
		goObject.transform.parent = transform;
		goObject.transform.localPosition = transform.localPosition;
		goObject.transform.localRotation = transform.localRotation;
	}

	public void Processing()
	{
		foreach (GameObject current in this.m_ParentObjectList)
		{
			if (!(current == null))
			{
				this.AttachObject(current, this.CreateObject());
			}
		}
	}
}
