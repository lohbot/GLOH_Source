using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class UsequenceObjectReloader : MonoBehaviour
{
	private UsequenceBundleLoader Loader;

	private static bool inProgress;

	private bool editorMode;

	private bool jobComplete;

	private int nowPath;

	private bool inLoad;

	private string[] paths;

	private UsequenceLoadableObject[] targets;

	public bool JobComplete
	{
		get
		{
			return this.jobComplete;
		}
	}

	public void Start()
	{
		if (UsequenceObjectReloader.inProgress)
		{
			return;
		}
		UsequenceObjectReloader.inProgress = true;
		this.editorMode = !Application.isPlaying;
		if (!this.SetPaths())
		{
			this.jobComplete = true;
			UsequenceObjectReloader.inProgress = false;
			return;
		}
		this.Init();
	}

	private void Init()
	{
		this.nowPath = 0;
		this.Loader = new UsequenceBundleLoader();
		this.Loader.Init();
		UsequenceBundleLoader.InitPool();
		this.jobComplete = false;
	}

	public void Dispose()
	{
		if (this.Loader != null)
		{
			this.Loader.Dispose(true);
		}
	}

	private void Update()
	{
		if (!this.editorMode && !this.jobComplete)
		{
			this.ReloadUpdate();
		}
	}

	private void ReloadUpdate()
	{
		try
		{
			GameObject gameObject = null;
			if (!this.inLoad)
			{
				if (this.paths != null)
				{
					gameObject = this.Loader.InstantiateFromLoadOrPool(this.paths[this.nowPath]);
				}
				if (gameObject == null)
				{
					this.inLoad = true;
					return;
				}
			}
			else
			{
				if (!this.Loader.DoneLoadBundle())
				{
					return;
				}
				string fileName = Path.GetFileName(this.paths[this.nowPath]);
				gameObject = this.Loader.InstantiateFromLoad(true, fileName);
			}
			this.HandleLoadedBundle(this.targets[this.nowPath++], gameObject);
			this.inLoad = false;
			if (this.IsAllLoaded())
			{
				this.OnAllLoaded();
			}
		}
		catch (Exception arg)
		{
			Debug.Log("로드 오브젝트 처리 실패: " + arg);
			this.Loader.Dispose(true);
		}
	}

	private bool IsAllLoaded()
	{
		return this.nowPath >= this.targets.Length;
	}

	private void OnAllLoaded()
	{
		UsequenceBundleLoader.ReleasePool();
		base.enabled = false;
		UsequenceObjectReloader.inProgress = false;
		this.jobComplete = true;
	}

	private bool SetPaths()
	{
		this.targets = base.gameObject.GetComponentsInChildren<UsequenceLoadableObject>();
		this.paths = new string[this.targets.Length];
		if (this.targets.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.targets.Length; i++)
		{
			this.paths[i] = this.targets[i].path.ToLower();
		}
		return true;
	}

	private void HandleLoadedBundle(UsequenceLoadableObject from, GameObject LoadedObject)
	{
		LoadedObject.name = LoadedObject.name.Replace("(Clone)", string.Empty);
		LoadedObject.name = string.Format("Loaded_{0}", LoadedObject.name);
		this.Loader.ReInitShader(LoadedObject);
		UsequenceLoadableObject usequenceLoadableObject = LoadedObject.AddComponent<UsequenceLoadableObject>();
		usequenceLoadableObject.loaded = false;
		usequenceLoadableObject.path = from.path;
		usequenceLoadableObject.addAnimations = from.addAnimations;
		usequenceLoadableObject.SetAnimations();
		this.MovingChildren(from.gameObject, LoadedObject);
		LoadedObject.transform.parent = from.gameObject.transform.parent;
		UnityEngine.Object.DestroyImmediate(from.gameObject);
	}

	private void MovingChildren(GameObject from, GameObject to)
	{
		Transform transform = from.transform;
		Transform transform2 = to.transform;
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.name.Contains("Loaded_"))
			{
				list.Add(child);
			}
		}
		foreach (Transform current in list)
		{
			current.parent = transform2;
		}
	}

	private void UpdatePrefab()
	{
	}
}
