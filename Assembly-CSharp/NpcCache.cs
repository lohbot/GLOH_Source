using Ndoors.Framework.Stage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TsBundle;
using UnityEngine;

public static class NpcCache
{
	private class CachingItem
	{
		public int refCount;

		public GameObject model;

		public CachingItem(GameObject go)
		{
			this.refCount = 1;
			this.model = go;
		}
	}

	private static Dictionary<string, NpcCache.CachingItem> mCachedItems = new Dictionary<string, NpcCache.CachingItem>();

	private static GameObject mRoot = null;

	private static bool mEnabled = true;

	public static bool Enabled
	{
		get
		{
			return NpcCache.mEnabled;
		}
		set
		{
			NpcCache.mEnabled = value;
		}
	}

	public static bool TryCloneObject(string key, out GameObject clonedObject)
	{
		NpcCache.CachingItem cachingItem;
		if (NpcCache.Enabled && NpcCache.mCachedItems.TryGetValue(key, out cachingItem) && cachingItem.model != null)
		{
			cachingItem.refCount++;
			clonedObject = (UnityEngine.Object.Instantiate(cachingItem.model) as GameObject);
			NpcCache.RegisterAutoRemover(key, clonedObject);
			return true;
		}
		clonedObject = null;
		return false;
	}

	private static void RegisterAutoRemover(string key, GameObject model)
	{
		MonoRemoveCachedModel monoRemoveCachedModel = model.AddComponent<MonoRemoveCachedModel>();
		monoRemoveCachedModel.RegisterAction(key, new Action<string>(NpcCache.Remove));
	}

	public static GameObject AddAndClone(string key, IDownloadedItem item)
	{
		if (!NpcCache.Enabled)
		{
			GameObject original = item.mainAsset as GameObject;
			return UnityEngine.Object.Instantiate(original) as GameObject;
		}
		if (string.IsNullOrEmpty(key))
		{
			int startIndex = item.assetPath.IndexOf('.');
			key = item.assetPath.Remove(startIndex);
		}
		GameObject gameObject;
		if (NpcCache.TryCloneObject(key, out gameObject))
		{
			return gameObject;
		}
		if (NpcCache.mRoot == null)
		{
			NpcCache.mRoot = GameObject.Find("@Internal NPC");
			if (NpcCache.mRoot == null)
			{
				NpcCache.mRoot = new GameObject("@Internal NPC");
				NpcCache.mRoot.SetActive(false);
				UnityEngine.Object.DontDestroyOnLoad(NpcCache.mRoot);
			}
		}
		gameObject = (UnityEngine.Object.Instantiate(item.mainAsset) as GameObject);
		gameObject.name = key;
		NpcCache.mCachedItems[key] = new NpcCache.CachingItem(gameObject);
		gameObject.transform.parent = NpcCache.mRoot.transform;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
		NpcCache.RegisterAutoRemover(key, gameObject2);
		return gameObject2;
	}

	[DebuggerHidden]
	private static IEnumerator RemoveTimer(string key, NpcCache.CachingItem item)
	{
		NpcCache.<RemoveTimer>c__Iterator7 <RemoveTimer>c__Iterator = new NpcCache.<RemoveTimer>c__Iterator7();
		<RemoveTimer>c__Iterator.key = key;
		<RemoveTimer>c__Iterator.item = item;
		<RemoveTimer>c__Iterator.<$>key = key;
		<RemoveTimer>c__Iterator.<$>item = item;
		return <RemoveTimer>c__Iterator;
	}

	private static void RemoveCached(string key, NpcCache.CachingItem item)
	{
		if (item.refCount <= 0)
		{
			if (item.model != null)
			{
				UnityEngine.Object.Destroy(item.model);
			}
			NpcCache.mCachedItems.Remove(key);
		}
	}

	private static void Remove(string key)
	{
		NpcCache.CachingItem cachingItem;
		if (NpcCache.mCachedItems.TryGetValue(key, out cachingItem) && --cachingItem.refCount == 0)
		{
			StageSystem.AddCommonPararellTask(NpcCache.RemoveTimer(key, cachingItem));
		}
	}

	public static void Clear()
	{
		UnityEngine.Object.Destroy(NpcCache.mRoot);
		NpcCache.mRoot = null;
		NpcCache.mCachedItems.Clear();
	}
}
