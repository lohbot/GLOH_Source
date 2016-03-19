using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WBundleManager
{
	private static WBundleManager Instance;

	public Dictionary<string, BaseBundle> HashList = new Dictionary<string, BaseBundle>();

	public List<BaseBundle> DownIngList = new List<BaseBundle>();

	public static WBundleManager GetInstance()
	{
		if (WBundleManager.Instance == null)
		{
			WBundleManager.Instance = new WBundleManager();
		}
		return WBundleManager.Instance;
	}

	public bool IsDown()
	{
		return 0 != this.DownIngList.Count;
	}

	[DebuggerHidden]
	public IEnumerator IsDone()
	{
		WBundleManager.<IsDone>c__Iterator3 <IsDone>c__Iterator = new WBundleManager.<IsDone>c__Iterator3();
		<IsDone>c__Iterator.<>f__this = this;
		return <IsDone>c__Iterator;
	}

	public BaseBundle GetBase(string _URL)
	{
		if (this.HashList.ContainsKey(_URL))
		{
			return this.HashList[_URL];
		}
		if (NrWWWStorage.ContainsKey(_URL))
		{
			WWW kWWW = NrWWWStorage.Get(_URL);
			return new BaseBundle(kWWW);
		}
		return null;
	}

	public BaseBundle Down(string _URL)
	{
		BaseBundle baseBundle = this.GetBase(_URL);
		if (baseBundle == null)
		{
			baseBundle = new BaseBundle(_URL);
			this.HashList.Add(_URL, baseBundle);
			this.DownIngList.Add(baseBundle);
		}
		return baseBundle;
	}

	public BaseBundle[] Down(string[] _URLList)
	{
		if (_URLList != null && _URLList.Length != 0)
		{
			List<BaseBundle> list = new List<BaseBundle>();
			for (int i = 0; i < _URLList.Length; i++)
			{
				string uRL = _URLList[i];
				BaseBundle item = this.Down(uRL);
				list.Add(item);
			}
			return list.ToArray();
		}
		return null;
	}

	public void Remove(string _URL)
	{
		if (this.HashList.ContainsKey(_URL))
		{
			this.HashList.Remove(_URL);
		}
	}

	public void UnloadAll()
	{
		this.HashList.Clear();
	}
}
