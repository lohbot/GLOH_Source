using System;
using System.Collections.Generic;
using UnityEngine;

public class DeActiveManager : MonoBehaviour
{
	private static DeActiveManager s_instance;

	public List<GameObject> m_DeActiveGameObjectList;

	public static bool IsCreate
	{
		get
		{
			return DeActiveManager.GetInstance() != null;
		}
		set
		{
			if (value)
			{
				DeActiveManager.CreateInstance();
			}
		}
	}

	public static DeActiveManager Instance
	{
		get
		{
			if (DeActiveManager.s_instance == null)
			{
				DeActiveManager.s_instance = DeActiveManager.GetInstance();
			}
			return DeActiveManager.s_instance;
		}
	}

	private static DeActiveManager CreateInstance()
	{
		GameObject gameObject = GameObject.Find(typeof(DeActiveManager).Name);
		if (gameObject == null)
		{
			gameObject = new GameObject(typeof(DeActiveManager).Name, new Type[]
			{
				typeof(DeActiveManager)
			});
		}
		return gameObject.GetComponent<DeActiveManager>();
	}

	private static DeActiveManager GetInstance()
	{
		GameObject gameObject = GameObject.Find(typeof(DeActiveManager).Name);
		if (gameObject != null)
		{
			return gameObject.GetComponent<DeActiveManager>();
		}
		return null;
	}

	public void Reset()
	{
		DeActiveManager.s_instance = DeActiveManager.GetInstance();
	}

	private void Start()
	{
		if (this.m_DeActiveGameObjectList != null)
		{
			foreach (GameObject current in this.m_DeActiveGameObjectList)
			{
				if (!(current == null))
				{
					current.SetActive(false);
					EventTriggerHelper.ActiveAllTreeChildren(current, false);
				}
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
