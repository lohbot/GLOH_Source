using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class WBundleLoader : MonoBehaviour
{
	private Action<Transform, GameObject> CompleteFunc;

	public string BaseURL = string.Empty;

	private BaseBundle BBundle;

	protected GameObject BaseMain;

	protected bool bLoaded;

	public GameObject GetBaseMain()
	{
		return this.BaseMain;
	}

	public static bool IsComplete(GameObject _Target)
	{
		if (_Target)
		{
			WBundleLoader[] componentsInChildren = _Target.GetComponentsInChildren<WBundleLoader>();
			if (componentsInChildren.Length != 0)
			{
				return false;
			}
			WBundle[] componentsInChildren2 = _Target.GetComponentsInChildren<WBundle>();
			if (componentsInChildren2 == null || componentsInChildren2.Length == 0)
			{
				return true;
			}
		}
		return false;
	}

	[DebuggerHidden]
	public static IEnumerator IsDone(GameObject _Target)
	{
		WBundleLoader.<IsDone>c__Iterator2 <IsDone>c__Iterator = new WBundleLoader.<IsDone>c__Iterator2();
		<IsDone>c__Iterator._Target = _Target;
		<IsDone>c__Iterator.<$>_Target = _Target;
		return <IsDone>c__Iterator;
	}

	public static void Load(GameObject _Parnet, string _BaseUrl, Action<Transform, GameObject> _CompFunc)
	{
		if (_Parnet)
		{
			WBundleLoader wBundleLoader = _Parnet.GetComponent<WBundleLoader>();
			if (null == wBundleLoader)
			{
				wBundleLoader = _Parnet.AddComponent<WBundleLoader>();
			}
			wBundleLoader.Load(_BaseUrl, _CompFunc);
		}
	}

	public static void Load(GameObject _Parnet, string _BaseUrl)
	{
		WBundleLoader.Load(_Parnet, _BaseUrl, null);
	}

	public void Load(string _BaseUrl)
	{
		if (_BaseUrl == null || 0 >= _BaseUrl.Length)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"\n==",
				_BaseUrl == null,
				"   err load ",
				base.name
			}));
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.CreateBase(_BaseUrl);
	}

	public void Load(string _BaseUrl, Action<Transform, GameObject> _CompFunc)
	{
		if (_BaseUrl == null || 0 >= _BaseUrl.Length)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"\n==",
				_BaseUrl == null,
				"   err load ",
				base.name
			}));
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.CreateBase(_BaseUrl);
		this.CompleteFunc = _CompFunc;
	}

	public void CreateBase(string _PathUrl)
	{
		if (!_PathUrl.Equals("NULL") && !_PathUrl.Equals("null"))
		{
			WBundleManager instance = WBundleManager.GetInstance();
			this.BBundle = instance.Down(_PathUrl);
		}
		if (this.BBundle == null)
		{
			UnityEngine.Debug.Log("ERR CreateBase " + _PathUrl);
			UnityEngine.Object.Destroy(this);
		}
	}

	protected void SelfDelete()
	{
		if (Application.isEditor)
		{
			UnityEngine.Object.DestroyImmediate(this);
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	protected virtual void UpdateBase()
	{
	}

	protected virtual void LoadFisnish()
	{
		this.SelfDelete();
	}

	public bool LoadBase(Transform _Parent, UnityEngine.Object _Obj)
	{
		GameObject gameObject = (GameObject)_Obj;
		if (gameObject)
		{
			this.BaseMain = (GameObject)UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
			if (_Parent)
			{
				this.BaseMain.transform.parent = _Parent;
			}
			this.BaseMain.transform.localPosition = gameObject.transform.localPosition;
			this.BaseMain.transform.localRotation = gameObject.transform.localRotation;
			if (this.CompleteFunc != null)
			{
				this.CompleteFunc(_Parent, this.BaseMain);
			}
			this.LoadFisnish();
		}
		return true;
	}

	public string GetName(string _URL)
	{
		int num = _URL.LastIndexOf("/") + 1;
		int num2 = _URL.LastIndexOf(".");
		int length = num2 - num;
		return _URL.Substring(num, length);
	}

	public bool LoadScene(BaseBundle _Bundle)
	{
		if (_Bundle != null && _Bundle.IsLoad())
		{
			string name = this.GetName(_Bundle.wItem.assetPath);
			UnityEngine.Debug.LogError("Deprecated! LoadLevelAdditive - FileName -- " + name);
			if (Application.CanStreamedLevelBeLoaded(name))
			{
				Application.LoadLevelAdditive(name);
				this.LoadFisnish();
			}
		}
		return true;
	}

	protected bool IsLoadAble()
	{
		return this.BBundle != null && this.BBundle.IsLoad() && !this.bLoaded;
	}

	private void Update()
	{
		if (this.IsLoadAble())
		{
			if (this.BBundle.LoadAsset())
			{
				UnityEngine.Object @base = this.BBundle.GetBase();
				if (@base)
				{
					this.bLoaded = this.LoadBase(base.transform, @base);
				}
				else
				{
					this.bLoaded = this.LoadScene(this.BBundle);
				}
			}
			else
			{
				this.LoadFisnish();
			}
		}
		this.UpdateBase();
	}

	public void UpdateEditMode()
	{
		this.Update();
	}
}
