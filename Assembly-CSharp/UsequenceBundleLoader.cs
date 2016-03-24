using System;
using System.Collections.Generic;
using System.IO;
using TsBundle;
using UnityEngine;

public class UsequenceBundleLoader
{
	public static string cdnPath = "d:/ndoors/at2dev/mobile/";

	private List<AssetBundleCreateRequest> assets = new List<AssetBundleCreateRequest>();

	private AssetBundleCreateRequest _currAsset;

	private static Dictionary<string, GameObject> LoadedPool = new Dictionary<string, GameObject>();

	private static int CallInitPool = 0;

	public void Init()
	{
		this._currAsset = null;
	}

	public void Dispose(bool unLoadAllObject)
	{
		if (this._currAsset == null)
		{
			return;
		}
		foreach (AssetBundleCreateRequest current in this.assets)
		{
			if (current.assetBundle != null)
			{
				current.assetBundle.Unload(unLoadAllObject);
			}
		}
		this.assets.Clear();
		this.assets = null;
		this._currAsset = null;
	}

	public void SetAsset(string path)
	{
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		byte[] array = new byte[fileStream.Length];
		fileStream.Read(array, 0, (int)fileStream.Length);
		this._currAsset = AssetBundle.CreateFromMemory(array);
		this.assets.Add(this._currAsset);
		fileStream.Dispose();
		fileStream.Close();
	}

	public bool DoneLoadBundle()
	{
		return this._currAsset != null && this._currAsset.isDone;
	}

	public GameObject InstantiateFromLoadOrPool(string path)
	{
		string text = Option.GetProtocolRootPath(Protocol.FILE);
		text = text.Substring("file:///".Length, text.Length - "file:///".Length);
		path = string.Format("{0}{1}", text, path);
		path = path.ToLower();
		if (!File.Exists(path))
		{
			Debug.LogError("존재하지 않는 파일: " + path);
			throw new Exception("존재하지 않는 파일: " + path);
		}
		string fileName = Path.GetFileName(path);
		if (!UsequenceBundleLoader.LoadedPool.ContainsKey(fileName))
		{
			this.SetAsset(path);
			return null;
		}
		return this.InstantiateFromPool(fileName);
	}

	private GameObject InstantiateFromPool(string fileNameFromPool)
	{
		return UnityEngine.Object.Instantiate(UsequenceBundleLoader.LoadedPool[fileNameFromPool]) as GameObject;
	}

	public GameObject InstantiateFromLoad(bool addToPool, string fileNameToPool)
	{
		if (this._currAsset.assetBundle == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(this._currAsset.assetBundle.mainAsset) as GameObject;
		if (!addToPool)
		{
			return gameObject;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
		fileNameToPool = fileNameToPool.ToLower();
		UsequenceBundleLoader.LoadedPool[fileNameToPool] = gameObject2;
		gameObject2.transform.parent = UsequenceBundleLoader.LoadedPool["@pool"].transform;
		gameObject2.name = gameObject2.name.Replace("(Clone)", string.Empty);
		return gameObject;
	}

	public static void InitPool()
	{
		UsequenceBundleLoader.LoadedPool = new Dictionary<string, GameObject>();
		GameObject value = new GameObject("@RELOADER_POOL");
		UsequenceBundleLoader.LoadedPool["@pool"] = value;
		UsequenceBundleLoader.CallInitPool++;
	}

	public static void ReleasePool()
	{
		if (--UsequenceBundleLoader.CallInitPool > 0)
		{
			return;
		}
		UnityEngine.Object.DestroyImmediate(UsequenceBundleLoader.LoadedPool["@pool"]);
		UsequenceBundleLoader.LoadedPool = null;
		UsequenceBundleLoader.CallInitPool = 0;
	}

	public void ReInitShader(GameObject loadedObject)
	{
		Renderer[] componentsInChildren = loadedObject.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			renderer.sharedMaterial.shader = Shader.Find(renderer.sharedMaterial.shader.name);
		}
	}
}
