using System;
using TsBundle;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TsMaterialBundleLoader : MonoBehaviour
{
	private static class CommonTextureGroup
	{
		private static string _name;

		public static string Name
		{
			get
			{
				return TsMaterialBundleLoader.CommonTextureGroup._name;
			}
		}

		static CommonTextureGroup()
		{
			TsMaterialBundleLoader.CommonTextureGroup._name = "CommonTexture";
			Holder.PushBundleGroup(TsMaterialBundleLoader.CommonTextureGroup._name);
		}
	}

	public string[] m_MaterialNames;

	private Material[] m_Materials;

	private int m_ProgressCount;

	private void Start()
	{
		this.LoadMaterial();
	}

	public void RemoveMaterial()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.m_MaterialNames != null && this.m_MaterialNames.Length > 0)
		{
			TsLog.LogWarning("이미 머터리얼이 제거되어 있습니다. (\"{0}\")", new object[]
			{
				base.gameObject.name
			});
			return;
		}
		Material[] sharedMaterials = base.renderer.sharedMaterials;
		this.m_MaterialNames = new string[sharedMaterials.Length];
		for (int i = 0; i < sharedMaterials.Length; i++)
		{
			string text = null;
			Material material = sharedMaterials[i];
			if (material != null)
			{
				text = material.name;
			}
			this.m_MaterialNames[i] = text;
			sharedMaterials[i] = null;
		}
		base.renderer.sharedMaterials = sharedMaterials;
	}

	private void LoadMaterial()
	{
		if (this.m_MaterialNames == null)
		{
			return;
		}
		this.m_ProgressCount = this.m_MaterialNames.Length;
		this.m_Materials = new Material[this.m_ProgressCount];
		for (int i = 0; i < this.m_MaterialNames.Length; i++)
		{
			if (!string.IsNullOrEmpty(this.m_MaterialNames[i]))
			{
				string text = TsMaterialBundleLoader.MakeBundlePath(this.m_MaterialNames[i]);
				WWWItem wWWItem = Holder.TryGetOrCreateBundle(text, TsMaterialBundleLoader.CommonTextureGroup.Name);
				if (wWWItem != null)
				{
					wWWItem.SetItemType(ItemType.USER_ASSETB);
					wWWItem.SetCallback(new PostProcPerItem(this.OnCompleteDownload), i);
					TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, false);
				}
				else
				{
					TsLog.LogError("[RuntimeTexture] cannot create WWWItem (\"{0}\")", new object[]
					{
						text
					});
				}
			}
		}
	}

	private void OnCompleteDownload(IDownloadedItem item, object param)
	{
		int num = (int)param;
		AssetBundle safeBundle = item.GetSafeBundle();
		if (safeBundle != null)
		{
			Material material = safeBundle.mainAsset as Material;
			this.m_Materials[num] = material;
		}
		else
		{
			TsLog.LogError("[TsMaterialBundleLoader] Bundle is null (Path=\"{0}\", Error=\"{1}\")", new object[]
			{
				item.assetPath,
				item.errorString
			});
		}
		if (--this.m_ProgressCount == 0)
		{
			base.renderer.materials = this.m_Materials;
			this.m_Materials = null;
		}
	}

	public static void ClearBundleStack()
	{
		Holder.ClearStackItem(TsMaterialBundleLoader.CommonTextureGroup.Name, true);
	}

	public static string MakeBundlePath(string textureName)
	{
		return string.Format("Material/{0}{1}.assetbundle", textureName, (!TsPlatform.IsMobile) ? string.Empty : "_mobile");
	}
}
