using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WBundle : MonoBehaviour
{
	public List<string> URL_List = new List<string>();

	private BaseBundle[] BundleList;

	public static WBundle GetWBundle(GameObject _Obj)
	{
		WBundle wBundle = null;
		if (_Obj)
		{
			wBundle = _Obj.GetComponent<WBundle>();
			if (null == wBundle)
			{
				wBundle = _Obj.AddComponent<WBundle>();
			}
		}
		return wBundle;
	}

	public List<string> GetList()
	{
		return this.URL_List;
	}

	private void Awake()
	{
		this.RegisterManager();
	}

	public void RegisterManager()
	{
		if (this.URL_List.Count == 0)
		{
			UnityEngine.Debug.LogError("WBundle URL_List is empty :" + base.gameObject.name);
		}
		this.BundleList = WBundleManager.GetInstance().Down(this.URL_List.ToArray());
	}

	public void AddList(string _RelativePath)
	{
		_RelativePath = _RelativePath.Replace("Assets/", string.Empty);
		this.URL_List.Add(_RelativePath);
	}

	[DebuggerHidden]
	public IEnumerator Start()
	{
		WBundle.<Start>c__Iterator1 <Start>c__Iterator = new WBundle.<Start>c__Iterator1();
		<Start>c__Iterator.<>f__this = this;
		return <Start>c__Iterator;
	}

	private void MakeComplete()
	{
		BaseBundle[] bundleList = this.BundleList;
		for (int i = 0; i < bundleList.Length; i++)
		{
			BaseBundle baseBundle = bundleList[i];
			baseBundle.AttachType(base.gameObject);
		}
		UnityEngine.Object.Destroy(this);
	}
}
