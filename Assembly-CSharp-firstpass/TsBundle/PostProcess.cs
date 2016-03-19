using System;
using System.Collections.Generic;
using UnityEngine;

namespace TsBundle
{
	public static class PostProcess
	{
		public static void JustDownloadItemVerify(IDownloadedItem wItem, object obj)
		{
			TsLog.Log("JustDownloadItemVerify : Downloaded {0}. Error = {1}", new object[]
			{
				wItem.assetPath,
				!wItem.canAccessAssetBundle
			});
		}

		public static void JustDownloadItemsVerify(List<WWWItem> wiList, object obj)
		{
			foreach (WWWItem current in wiList)
			{
				PostProcess.JustDownloadItemVerify(current, obj);
			}
		}

		public static void InstantiatePrefab(WWWItem wItem, object obj)
		{
			if (wItem.isCanceled)
			{
				return;
			}
			GameObject gameObject = obj as GameObject;
			if (null == gameObject)
			{
				TsLog.LogError("InstantiatePrefab 2nd parameter is not GameObject! - {0}", new object[]
				{
					wItem.assetName
				});
				return;
			}
			if (!wItem.canAccessAssetBundle)
			{
				TsLog.LogError("Can't access assetbundle! - {0}", new object[]
				{
					wItem.assetName
				});
				return;
			}
			UnityEngine.Object mainAsset = wItem.GetSafeBundle().mainAsset;
			if (null != mainAsset)
			{
				GameObject gameObject2 = mainAsset as GameObject;
				GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2) as GameObject;
				gameObject3.transform.parent = gameObject.transform;
				gameObject3.transform.localPosition = gameObject2.transform.localPosition;
				gameObject3.transform.localRotation = gameObject2.transform.localRotation;
			}
			else
			{
				TsLog.LogError("OnCompleteAsyncLoadGameObject(). assetBundle.mainAsset is not GameObjcet! - {0}", new object[]
				{
					wItem.assetName
				});
			}
		}

		public static void LoadLevel(WWWItem wItem, object obj)
		{
			try
			{
				Application.LoadLevel(wItem.assetName);
			}
			catch (Exception obj2)
			{
				TsLog.LogWarning(obj2);
			}
		}

		public static void LoadLevelAdditive(WWWItem wItem, object obj)
		{
			try
			{
				Application.LoadLevelAdditive(wItem.assetName);
			}
			catch (Exception obj2)
			{
				TsLog.LogWarning(obj2);
			}
		}

		public static void LoadComponents(List<WWWItem> wiList, object obj)
		{
			foreach (WWWItem current in wiList)
			{
				if (current.canAccessAssetBundle)
				{
					PostProcess.LoadComponent(current, obj);
				}
			}
		}

		public static void LoadComponent(WWWItem wItem, object obj)
		{
			switch (wItem.itemType)
			{
			case ItemType.AUDIO:
				PostProcess.LoadComponentAudioClip(wItem, obj);
				break;
			case ItemType.MATERIAL:
				PostProcess.LoadComponentMaterial(wItem, obj);
				break;
			}
		}

		public static void LoadComponentMaterial(WWWItem wItem, object obj)
		{
			if (!wItem.canAccessAssetBundle)
			{
				TsLog.LogError("TsBundle[{0}] LoadComponentMaterial download fail! {1}", new object[]
				{
					Time.frameCount,
					wItem.assetPath
				});
				return;
			}
			GameObject gameObject = obj as GameObject;
			if (null == gameObject)
			{
				TsLog.LogError("TsBundle[{0}] LoadComponentMaterial 2nd parameter is not GameObject!", new object[]
				{
					Time.frameCount
				});
				return;
			}
			try
			{
				MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
				Material material = wItem.GetSafeBundle().mainAsset as Material;
				if (null == component || null == material)
				{
					TsLog.LogError("TsBundle[{0}] Fail cast to Material. {1}", new object[]
					{
						Time.frameCount,
						wItem.assetPath
					});
				}
				else
				{
					int num = component.sharedMaterials.Length;
					Material[] sharedMaterials = component.sharedMaterials;
					Material[] array = new Material[num + 1];
					for (int i = 0; i < num; i++)
					{
						array[i] = sharedMaterials[i];
					}
					array[num] = material;
					component.sharedMaterials = array;
				}
			}
			catch (Exception obj2)
			{
				TsLog.LogWarning(obj2);
			}
		}

		public static void LoadComponentAudioClip(WWWItem wItem, object obj)
		{
			if (!wItem.canAccessAssetBundle)
			{
				return;
			}
			GameObject gameObject = obj as GameObject;
			if (null == gameObject)
			{
				TsLog.LogError("LoadComponentAudioClip 2nd parameter is not GameObject!", new object[0]);
				return;
			}
			try
			{
				AudioClip audioClip = wItem.GetSafeBundle().mainAsset as AudioClip;
				if (null == gameObject.audio)
				{
					TsLog.LogError("TsBundle[{0}] Fail find AudioSource. {1}", new object[]
					{
						Time.frameCount,
						wItem.assetPath
					});
				}
				else if (null == audioClip)
				{
					TsLog.LogError("TsBundle[{0}] Fail cast to AudioClip. {1}", new object[]
					{
						Time.frameCount,
						wItem.assetPath
					});
				}
				else
				{
					gameObject.audio.clip = audioClip;
					gameObject.audio.Play();
				}
			}
			catch (Exception obj2)
			{
				TsLog.LogWarning(obj2);
			}
		}

		public static void LoadSkyMeterial(IDownloadedItem wItem, object obj)
		{
			if (!wItem.canAccessAssetBundle)
			{
				return;
			}
			try
			{
				RenderSettings.skybox = (wItem.GetSafeBundle().mainAsset as Material);
			}
			catch (Exception obj2)
			{
				TsLog.LogWarning(obj2);
			}
		}

		public static void LoadAudioClips(List<WWWItem> wiList, object obj)
		{
		}

		public static void LoadAnimationClips(List<WWWItem> wiList, object obj)
		{
		}
	}
}
