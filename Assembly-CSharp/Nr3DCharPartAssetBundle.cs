using System;
using System.Collections.Generic;
using UnityEngine;

public class Nr3DCharPartAssetBundle
{
	private AssetBundle m_kAssetBundle;

	private bool m_bMainAsset;

	private string m_szNewName = string.Empty;

	private bool m_bSetBundle;

	private string m_szLoadPartName = string.Empty;

	public Nr3DCharPartAssetBundle()
	{
		this.Init();
	}

	public void Init()
	{
		this.InitBundleInfo();
		this.m_szLoadPartName = string.Empty;
	}

	public void InitBundleInfo()
	{
		this.m_kAssetBundle = null;
		this.m_bMainAsset = false;
		this.m_szNewName = string.Empty;
		this.m_bSetBundle = false;
	}

	public bool IsValid()
	{
		return this.m_bSetBundle;
	}

	public void SetAssetBundle(AssetBundle kAssetBundle)
	{
		this.SetAssetBundle(kAssetBundle, false);
	}

	public void SetAssetBundle(AssetBundle kAssetBundle, bool bMainAsset)
	{
		this.m_kAssetBundle = kAssetBundle;
		if (this.m_kAssetBundle.mainAsset != null)
		{
			this.m_bMainAsset = true;
		}
		else
		{
			this.m_bMainAsset = bMainAsset;
		}
		this.m_bSetBundle = true;
	}

	public AssetBundle GetAssetBundle()
	{
		return this.m_kAssetBundle;
	}

	public void SetNewName(string newname)
	{
		this.m_szNewName = newname;
	}

	public string GetNewName()
	{
		return this.m_szNewName;
	}

	public void SetLoadPartName(string partname)
	{
		this.m_szLoadPartName = partname;
		this.m_bSetBundle = false;
	}

	public string GetLoadPartName()
	{
		return this.m_szLoadPartName;
	}

	public bool IsSameLoadPartName(string partname)
	{
		return this.m_szLoadPartName.Equals(partname) && this.IsValid();
	}

	public bool IsLoadedBundle()
	{
		return this.m_szLoadPartName.Length > 0;
	}

	public GameObject GetRenderObject()
	{
		if (null == this.m_kAssetBundle)
		{
			return null;
		}
		GameObject gameObject;
		if (!this.m_bMainAsset)
		{
			AssetBundleRequest assetBundleRequest = this.m_kAssetBundle.LoadAsync("rendererobject", typeof(GameObject));
			if (assetBundleRequest == null)
			{
				return null;
			}
			gameObject = (GameObject)assetBundleRequest.asset;
		}
		else
		{
			gameObject = (GameObject)this.m_kAssetBundle.mainAsset;
		}
		if (gameObject == null)
		{
			return null;
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
		gameObject2.name = this.GetNewName();
		return gameObject2;
	}

	public SkinnedMeshRenderer[] GetSkinnedMeshRenderer()
	{
		GameObject gameObject;
		if (this.m_bMainAsset)
		{
			GameObject original = (GameObject)this.m_kAssetBundle.mainAsset;
			gameObject = (GameObject)UnityEngine.Object.Instantiate(original);
			SkinnedMeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
			SkinnedMeshRenderer[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = array[i];
				skinnedMeshRenderer.gameObject.transform.parent = null;
				UnityEngine.Object.Destroy(gameObject);
			}
			return componentsInChildren;
		}
		if (null == this.m_kAssetBundle)
		{
			return null;
		}
		AssetBundleRequest assetBundleRequest = this.m_kAssetBundle.LoadAsync("rendererobject", typeof(GameObject));
		if (assetBundleRequest == null)
		{
			return null;
		}
		GameObject original2 = (GameObject)assetBundleRequest.asset;
		gameObject = (GameObject)UnityEngine.Object.Instantiate(original2);
		return new SkinnedMeshRenderer[]
		{
			(SkinnedMeshRenderer)gameObject.renderer
		};
	}

	public string[] GetBoneNameList()
	{
		if (this.m_bMainAsset)
		{
			List<string> list = new List<string>();
			SkinnedMeshRenderer[] skinnedMeshRenderer = this.GetSkinnedMeshRenderer();
			SkinnedMeshRenderer[] array = skinnedMeshRenderer;
			for (int i = 0; i < array.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer2 = array[i];
				Transform[] bones = skinnedMeshRenderer2.bones;
				for (int j = 0; j < bones.Length; j++)
				{
					Transform transform = bones[j];
					list.Add(transform.name);
				}
				UnityEngine.Object.Destroy(skinnedMeshRenderer2.gameObject);
			}
			return list.ToArray();
		}
		if (null == this.m_kAssetBundle)
		{
			return null;
		}
		UnityEngine.Object @object = this.m_kAssetBundle.Load("bonenames");
		if (@object == null)
		{
			Debug.LogError("couldn't found bonenames.");
			Debug.Break();
			return null;
		}
		if (@object.GetType() == typeof(HolderStrings))
		{
			return ((HolderStrings)@object).GetContent();
		}
		return ((TLStringHolder)@object).content;
	}
}
