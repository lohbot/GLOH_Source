using System;
using System.Collections.Generic;
using UnityEngine;

public class NrShadowSystem : NrTSingleton<NrShadowSystem>
{
	private List<GameObject> m_arGameObeject;

	private NrShadowSystem()
	{
		this.m_arGameObeject = new List<GameObject>();
	}

	public void Register(GameObject kGameObj)
	{
		this.m_arGameObeject.Add(kGameObj);
	}

	public void ShadowTest(bool b)
	{
		foreach (GameObject current in this.m_arGameObeject)
		{
			Renderer component = current.GetComponent<Renderer>();
			component.castShadows = b;
			component.receiveShadows = b;
		}
	}

	public void CollectObjectsToRender()
	{
		this.m_arGameObeject.Clear();
		object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i] as GameObject;
			Renderer component = gameObject.GetComponent<Renderer>();
			if (null != component)
			{
				this.Register(gameObject);
			}
		}
	}
}
