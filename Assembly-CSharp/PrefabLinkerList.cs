using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLinkerList : MonoBehaviour
{
	public List<PrefabInfo> m_PrefabInfo = new List<PrefabInfo>();

	public UnityEngine.Object[] GetPrefabs()
	{
		UnityEngine.Object[] array = new UnityEngine.Object[this.m_PrefabInfo.Count];
		for (int i = 0; i < this.m_PrefabInfo.Count; i++)
		{
			array[i] = this.m_PrefabInfo[i].m_PrefabObject;
		}
		return array;
	}

	public PrefabInfo GetInfo(UnityEngine.Object oj)
	{
		foreach (PrefabInfo current in this.m_PrefabInfo)
		{
			if (current.m_PrefabObject == oj)
			{
				return current;
			}
		}
		return null;
	}

	public PrefabInfo[] GetInfos()
	{
		PrefabInfo[] array = new PrefabInfo[this.m_PrefabInfo.Count];
		for (int i = 0; i < this.m_PrefabInfo.Count; i++)
		{
			array[i] = this.m_PrefabInfo[i];
		}
		return array;
	}
}
