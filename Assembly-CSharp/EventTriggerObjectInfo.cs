using System;
using UnityEngine;

[Serializable]
public class EventTriggerObjectInfo
{
	public string m_Key;

	public string m_ValueTypeName;

	public UnityEngine.Object m_Value;

	public EventTriggerObjectInfo(string key, UnityEngine.Object value)
	{
		this.m_Key = key;
		this.m_ValueTypeName = value.GetType().AssemblyQualifiedName;
		this.m_Value = value;
	}

	public UnityEngine.Object GetObject()
	{
		if (this.m_Value == null)
		{
			this.SetObject();
		}
		return this.m_Value;
	}

	public void SetObject()
	{
		if (this.m_Value != null)
		{
			return;
		}
		Type type = Type.GetType(this.m_ValueTypeName);
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(type);
		UnityEngine.Object[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			UnityEngine.Object @object = array2[i];
			if (@object.name == this.m_Key)
			{
				this.m_Value = @object;
				return;
			}
		}
	}
}
