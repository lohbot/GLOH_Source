using System;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerObject : MonoBehaviour
{
	public List<EventTriggerObjectInfo> InfoList = new List<EventTriggerObjectInfo>();

	private void Start()
	{
		this.SetObject();
		this.MoveObject(base.gameObject.transform.parent, null);
	}

	public void SetObject()
	{
		foreach (EventTriggerObjectInfo current in this.InfoList)
		{
			current.SetObject();
		}
	}

	public void MoveObject(Transform current, Transform move)
	{
		foreach (EventTriggerObjectInfo current2 in this.InfoList)
		{
			GameObject gameObject;
			if (current2.GetObject().GetType() != typeof(GameObject))
			{
				gameObject = GameObject.Find(current2.m_Key);
			}
			else
			{
				gameObject = (current2.GetObject() as GameObject);
			}
			while (!(gameObject == null))
			{
				if (!(gameObject.transform.parent != current) || !(gameObject.transform.parent != move))
				{
					gameObject.transform.parent = move;
					break;
				}
				gameObject = gameObject.transform.parent.gameObject;
			}
		}
	}

	public void Add(EventTriggerObjectInfo Info)
	{
		if (!this.IsKey(Info.m_Key, Info.GetObject()))
		{
			this.InfoList.Add(Info);
		}
	}

	public UnityEngine.Object GetObject(string key, string TypeName)
	{
		foreach (EventTriggerObjectInfo current in this.InfoList)
		{
			if (current.m_Key == key && current.m_ValueTypeName == TypeName)
			{
				return current.GetObject();
			}
		}
		return GameObject.Find(key);
	}

	private bool IsKey(string key, UnityEngine.Object oj)
	{
		foreach (EventTriggerObjectInfo current in this.InfoList)
		{
			if (current.m_Key == key && current.m_ValueTypeName == oj.GetType().AssemblyQualifiedName)
			{
				return true;
			}
		}
		return false;
	}
}
