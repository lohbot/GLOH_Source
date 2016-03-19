using System;
using System.Collections.Generic;
using UnityEngine;

public class CResources
{
	private static Dictionary<string, GameObject> goPrefabDict = new Dictionary<string, GameObject>();

	private static Dictionary<string, UnityEngine.Object> uiPrefabDict = new Dictionary<string, UnityEngine.Object>();

	public static void ClearGoPrefabCache()
	{
		foreach (GameObject current in CResources.goPrefabDict.Values)
		{
			UnityEngine.Object.Destroy(current);
		}
		CResources.goPrefabDict.Clear();
	}

	private static GameObject loadGoPrefabCache(string PfName)
	{
		GameObject gameObject = null;
		CResources.goPrefabDict.TryGetValue(PfName, out gameObject);
		if (null == gameObject)
		{
			gameObject = (GameObject)Resources.Load(PfName);
			if (null != gameObject)
			{
				CResources.goPrefabDict.Add(PfName, gameObject);
			}
		}
		return gameObject;
	}

	public static void ClearUiPrefabCache()
	{
		foreach (UnityEngine.Object current in CResources.uiPrefabDict.Values)
		{
			UnityEngine.Object.Destroy(current);
		}
		CResources.goPrefabDict.Clear();
	}

	private static UnityEngine.Object loadUiPrefabCache(string PfName)
	{
		UnityEngine.Object @object = null;
		CResources.uiPrefabDict.TryGetValue(PfName, out @object);
		if (null == @object)
		{
			@object = Resources.Load(PfName);
			if (null != @object)
			{
				CResources.uiPrefabDict.Add(PfName, @object);
			}
		}
		return @object;
	}

	public static Transform SetLocal(Transform Des, Transform Src)
	{
		Des.localPosition = Src.position;
		Des.localRotation = Src.rotation;
		Des.localScale = Src.localScale;
		return Des;
	}

	public static GameObject ADDPrefabLoad(GameObject _Parent, string _PfName)
	{
		GameObject gameObject = CResources.loadGoPrefabCache(_PfName);
		if (gameObject)
		{
			return CResources.ADDPrefabClone(_Parent, gameObject);
		}
		Debug.LogError("Fail! CResources::ADDPrefabLoad - " + _PfName);
		return null;
	}

	public static GameObject ADDPrefabClone(GameObject _Parent, GameObject _Prefab)
	{
		if (_Prefab)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(_Prefab, Vector3.zero, Quaternion.identity);
			if (_Parent && _Parent.transform)
			{
				gameObject.transform.parent = _Parent.transform;
			}
			CResources.SetLocal(gameObject.transform, _Prefab.transform);
			return gameObject;
		}
		return null;
	}

	public static T ADDPrefab<T>(GameObject _Parent, string _PfName) where T : Component
	{
		GameObject gameObject = CResources.ADDPrefabLoad(_Parent, _PfName);
		return gameObject.GetComponent<T>();
	}

	public static T ADDPrefabInChild<T>(GameObject _Parent, string _PfName) where T : Component
	{
		GameObject gameObject = CResources.ADDPrefabLoad(_Parent, _PfName);
		return gameObject.GetComponentInChildren<T>();
	}

	public static UnityEngine.Object Load(string _Path)
	{
		UnityEngine.Object result;
		try
		{
			UnityEngine.Object @object = CResources.loadUiPrefabCache(_Path);
			if (@object == null)
			{
				Debug.LogError("Fail! CResources::Load - " + _Path);
			}
			result = @object;
		}
		catch (Exception ex)
		{
			Debug.LogWarning("CResources Object Load Exception = " + ex.ToString());
			result = null;
		}
		return result;
	}

	public static void Delete(string _Path)
	{
		if (CResources.uiPrefabDict.ContainsKey(_Path))
		{
			CResources.uiPrefabDict.Remove(_Path);
		}
	}

	public static UnityEngine.Object LoadClone(string _Path)
	{
		UnityEngine.Object result;
		try
		{
			UnityEngine.Object @object = CResources.loadUiPrefabCache(_Path);
			if (@object != null)
			{
				@object = UnityEngine.Object.Instantiate(@object);
			}
			else
			{
				Debug.LogError("Fail! CResources::LoadClone - " + _Path);
			}
			result = @object;
		}
		catch (Exception ex)
		{
			Debug.LogWarning("CResources Object LoadClone Exception = " + ex.ToString());
			result = null;
		}
		return result;
	}
}
