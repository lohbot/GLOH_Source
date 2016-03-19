using System;
using System.Collections.Generic;
using UnityEngine;

public static class NrWWWStorage
{
	private static Dictionary<string, WWW> ms_mapWWW = new Dictionary<string, WWW>();

	public static int Count
	{
		get
		{
			return NrWWWStorage.ms_mapWWW.Count;
		}
	}

	public static void Add(string strURL, WWW kWWW)
	{
		strURL = strURL.ToLower();
		NrWWWStorage.ms_mapWWW.Add(strURL, kWWW);
	}

	public static WWW Get(string strURL)
	{
		strURL = strURL.ToLower();
		return NrWWWStorage.ms_mapWWW[strURL];
	}

	public static bool ContainsKey(string strKey)
	{
		strKey = strKey.ToLower();
		return NrWWWStorage.ms_mapWWW.ContainsKey(strKey);
	}

	public static void Remove(string strKey)
	{
		strKey = strKey.ToLower();
		if (!NrWWWStorage.ms_mapWWW.ContainsKey(strKey))
		{
			Debug.LogWarning("not exist key or remove already:" + strKey);
			return;
		}
		NrWWWStorage.ms_mapWWW.Remove(strKey);
	}

	public static void UnloadRemove(string strKey)
	{
		if (!NrWWWStorage.ms_mapWWW.ContainsKey(strKey))
		{
			Debug.LogError("not exist key:" + strKey);
			Debug.Break();
		}
		NrWWWStorage.ms_mapWWW[strKey].assetBundle.Unload(false);
		NrWWWStorage.ms_mapWWW.Remove(strKey);
	}
}
