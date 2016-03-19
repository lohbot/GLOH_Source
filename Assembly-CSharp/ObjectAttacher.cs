using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttacher : MonoBehaviour
{
	private static ObjectAttacher s_instance;

	public List<ObjectAttachInfo> m_ObjectAttachInfoList;

	public static bool IsCreate
	{
		get
		{
			return ObjectAttacher.GetInstance() != null;
		}
		set
		{
			if (value)
			{
				ObjectAttacher.CreateInstance();
			}
		}
	}

	public static ObjectAttacher Instance
	{
		get
		{
			if (ObjectAttacher.s_instance == null)
			{
				ObjectAttacher.s_instance = ObjectAttacher.GetInstance();
			}
			return ObjectAttacher.s_instance;
		}
	}

	private static ObjectAttacher CreateInstance()
	{
		GameObject gameObject = GameObject.Find(typeof(ObjectAttacher).Name);
		if (gameObject == null)
		{
			gameObject = new GameObject(typeof(ObjectAttacher).Name, new Type[]
			{
				typeof(ObjectAttacher)
			});
		}
		return gameObject.GetComponent<ObjectAttacher>();
	}

	private static ObjectAttacher GetInstance()
	{
		GameObject gameObject = GameObject.Find(typeof(ObjectAttacher).Name);
		if (gameObject != null)
		{
			return gameObject.GetComponent<ObjectAttacher>();
		}
		return null;
	}

	public void Reset()
	{
		ObjectAttacher.s_instance = ObjectAttacher.GetInstance();
	}

	private void Start()
	{
		if (this.m_ObjectAttachInfoList != null)
		{
			foreach (ObjectAttachInfo current in this.m_ObjectAttachInfoList)
			{
				current.Processing();
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
