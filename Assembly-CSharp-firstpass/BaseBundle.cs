using System;
using System.Collections;
using System.Diagnostics;
using TsBundle;
using UnityEngine;

public class BaseBundle
{
	public WWWItem wItem;

	public long StartTime;

	public long EndTime;

	private static string AssetRoot = "BUNDLE/";

	public static string AssetExt = ".assetbundle";

	public AssetBundle BaseAsset
	{
		get
		{
			return this.wItem.GetSafeBundle();
		}
	}

	public UnityEngine.Object BaseObject
	{
		get
		{
			if (this.BaseAsset == null)
			{
				return null;
			}
			return this.BaseAsset.mainAsset;
		}
	}

	private static string BaseURL
	{
		get
		{
			return CDefinePath.WebData() + "BUNDLE/";
		}
	}

	public BaseBundle(WWW kWWW)
	{
		UnityEngine.Debug.LogWarning(" BaseBundle.cs BaseBundle(WWW kWWW) - Not Use~!");
	}

	public BaseBundle(string _BundlePath)
	{
		string key = BaseBundle.AssetRoot + _BundlePath + BaseBundle.AssetExt;
		this.wItem = Holder.TryGetOrCreateBundle(key, null);
		this.wItem.SetItemType(ItemType.USER_ASSETB);
		TsImmortal.bundleService.RequestDownloadCoroutine(this.wItem, DownGroup.RUNTIME, false);
	}

	public string GetURL()
	{
		return (this.wItem != null) ? this.wItem.assetPath : null;
	}

	[DebuggerHidden]
	public IEnumerator WaitForDownload()
	{
		BaseBundle.<WaitForDownload>c__Iterator0 <WaitForDownload>c__Iterator = new BaseBundle.<WaitForDownload>c__Iterator0();
		<WaitForDownload>c__Iterator.<>f__this = this;
		return <WaitForDownload>c__Iterator;
	}

	public bool IsLoad()
	{
		if (0L >= this.StartTime)
		{
			this.StartTime = (long)Environment.TickCount;
		}
		if (!this.wItem.isCompleteAsyncOp)
		{
			return false;
		}
		if (!this.wItem.canAccessAssetBundle)
		{
			UnityEngine.Debug.LogError("---------------- BaseBundle.cs : IsLoad() wItem.canAccessAssetBundle :" + this.wItem.assetPath);
			return false;
		}
		if (0L >= this.EndTime)
		{
			this.EndTime = (long)Environment.TickCount;
		}
		return true;
	}

	public bool LoadAsset()
	{
		if (!this.wItem.canAccessAssetBundle)
		{
			UnityEngine.Debug.LogError("LoadAsset:ERR: " + this.wItem.assetPath);
			NrTSingleton<NrLogSystem>.Instance.AddString("LoadAsset Err: " + this.wItem.assetPath);
			return false;
		}
		return true;
	}

	public UnityEngine.Object GetBase()
	{
		if (!this.BaseAsset || !this.BaseAsset.mainAsset)
		{
			return null;
		}
		return this.BaseAsset.mainAsset;
	}

	public GameObject AttachType(GameObject _Base)
	{
		if (this.BaseObject != null)
		{
			bool flag = false;
			if (typeof(Material) == this.BaseObject.GetType())
			{
				flag = this.Attach(_Base, (Material)this.BaseObject);
			}
			else if (typeof(Mesh) == this.BaseObject.GetType())
			{
				flag = this.Attach(_Base, (Mesh)this.BaseObject);
			}
			else if (typeof(AnimationClip) == this.BaseObject.GetType())
			{
				flag = this.Attach(_Base, (AnimationClip)this.BaseObject);
			}
			else if (typeof(GameObject) == this.BaseObject.GetType())
			{
				UnityEngine.Debug.Log("ERR GAME OBEJCT" + this.BaseObject.name);
			}
			if (!flag)
			{
				UnityEngine.Debug.Log(" ERRRR " + this.BaseObject.name + this.BaseObject.GetType());
			}
		}
		return _Base;
	}

	public bool Attach(GameObject _Base, Material _Mat)
	{
		Renderer component = _Base.GetComponent<SkinnedMeshRenderer>();
		if (null == component)
		{
			component = _Base.GetComponent<MeshRenderer>();
		}
		if (component)
		{
			Material[] sharedMaterials = component.sharedMaterials;
			int num = sharedMaterials.Length;
			Material[] array = new Material[num + 1];
			for (int i = 0; i < num; i++)
			{
				array[i] = sharedMaterials[i];
			}
			array[num] = _Mat;
			component.sharedMaterials = array;
			return true;
		}
		return false;
	}

	public bool Attach(GameObject _Base, AnimationClip _Clip)
	{
		Animation component = _Base.GetComponent<Animation>();
		if (component && _Clip)
		{
			component.AddClip(_Clip, _Clip.name);
			return true;
		}
		return false;
	}

	public bool Attach(GameObject _Base, Mesh _Mesh)
	{
		SkinnedMeshRenderer component = _Base.GetComponent<SkinnedMeshRenderer>();
		if (component)
		{
			component.sharedMesh = _Mesh;
			return true;
		}
		return false;
	}
}
