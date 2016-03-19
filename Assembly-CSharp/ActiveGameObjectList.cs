using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGameObjectList : MonoBehaviour
{
	public List<GameObject> m_ActiveGameObjectList = new List<GameObject>();

	public void ActiveGameObject(bool bActive)
	{
		foreach (GameObject current in this.m_ActiveGameObjectList)
		{
			if (!(current == null))
			{
				current.SetActive(bActive);
			}
		}
	}

	public T FindComponet<T>() where T : Component
	{
		foreach (GameObject current in this.m_ActiveGameObjectList)
		{
			if (!(current == null))
			{
				T component = current.GetComponent<T>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return (T)((object)null);
	}

	public Component[] FindComponets(Type TYPE)
	{
		List<Component> list = new List<Component>();
		foreach (GameObject current in this.m_ActiveGameObjectList)
		{
			if (!(current == null))
			{
				Component component = current.GetComponent(TYPE.ToString());
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
		return list.ToArray();
	}

	public void ActiveType(Type TYPE, bool bActive)
	{
		Component[] array = this.FindComponets(typeof(EventTrigger));
		if (array.Length > 0)
		{
			Component[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Component component = array2[i];
				component.gameObject.SetActive(bActive);
			}
		}
	}

	public GameObject FindeObjectName(string ObjectName)
	{
		foreach (GameObject current in this.m_ActiveGameObjectList)
		{
			if (!(current == null))
			{
				if (current.name == ObjectName)
				{
					return current;
				}
			}
		}
		return null;
	}
}
