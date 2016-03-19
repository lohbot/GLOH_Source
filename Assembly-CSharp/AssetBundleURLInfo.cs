using System;
using System.Collections.Generic;
using TsPatch;
using UnityEngine;

public class AssetBundleURLInfo : NrTSingleton<AssetBundleURLInfo>
{
	private Dictionary<string, string> _BundleList;

	private bool _Load;

	public bool IsLoad
	{
		get
		{
			return this._Load;
		}
		set
		{
			this._Load = value;
		}
	}

	private AssetBundleURLInfo()
	{
		if (!this.CollectPatchBundleInfo() && TsPlatform.IsWeb)
		{
			GameObject gameObject = new GameObject(typeof(AssetBundleListDownLoad).Name);
			gameObject.AddComponent<AssetBundleListDownLoad>();
		}
	}

	public string GetURL(string BundleName)
	{
		if (this._BundleList != null)
		{
			string result = null;
			if (this._BundleList.TryGetValue(BundleName.ToLower(), out result))
			{
				return result;
			}
		}
		return null;
	}

	private bool CollectPatchBundleInfo()
	{
		return PatchFinalList.Instance.FilesList.Keys.Count > 0 && this.CollectBundleInfo(PatchFinalList.Instance.FilesList.Keys);
	}

	public bool CollectBundleInfo(IEnumerable<string> IBundleList)
	{
		foreach (string current in IBundleList)
		{
			string[] array = current.Split(new char[]
			{
				'/'
			});
			string key = array[array.Length - 1];
			if (this._BundleList == null)
			{
				this._BundleList = new Dictionary<string, string>();
			}
			if (!this._BundleList.ContainsKey(key))
			{
				this._BundleList.Add(key, current.Substring(1, current.Length - 1));
			}
		}
		this._Load = true;
		TsLog.Log(string.Format("[AssetBundleURL] Collect : {0}", this._BundleList.Count), new object[0]);
		return true;
	}
}
