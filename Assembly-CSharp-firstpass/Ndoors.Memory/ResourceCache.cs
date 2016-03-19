using System;
using System.Collections.Generic;
using System.Text;
using TsBundle;
using UnityEngine;

namespace Ndoors.Memory
{
	public static class ResourceCache
	{
		private struct LifeSaver
		{
			private object refObject;

			private int cntRequest;

			private float accessTime;

			public LifeSaver(object obj)
			{
				this.refObject = obj;
				this.cntRequest = 1;
				this.accessTime = Time.time;
			}

			public object Acquire()
			{
				this.cntRequest++;
				this.accessTime = Time.time;
				return this.refObject;
			}

			public void Release()
			{
				this.cntRequest--;
				if (this.cntRequest < 0)
				{
					TsLog.LogWarning("ResourceCache.LifeSaver reference count is invalid.", new object[0]);
				}
			}

			public bool isTimeOut(float curTime)
			{
				return curTime - this.accessTime > ResourceCache.timeOver;
			}
		}

		[ObjectPool(typeof(ResourceCache.WWWDownloader), 16, 32, 128)]
		private class WWWDownloader : IPoolObject
		{
			public static Dictionary<string, ResourceCache.LifeSaver> ms_refCacheDic;

			private PostProcPerItem callback;

			private object param;

			private static object CreateInstanceStaicPrivate()
			{
				return new ResourceCache.WWWDownloader();
			}

			public void OnCreate(object param)
			{
				this.callback = (param as PostProcPerItem);
				TsLog.Assert(param != null, "WWWDownloader.OnCreate(param) param is null!", new object[0]);
			}

			public void OnDelete()
			{
			}

			public void SetParam(object param)
			{
				this.param = param;
			}

			public void OnCompleteDonwload(WWWItem wItem, object wwwdownload)
			{
				object mainAsset = wItem.mainAsset;
				if (mainAsset == null)
				{
					TsLog.LogWarning("WWW.mainAsset casting error. {0}", new object[]
					{
						wItem.assetPath
					});
				}
				else
				{
					ResourceCache.LifeSaver value = new ResourceCache.LifeSaver(mainAsset);
					ResourceCache.WWWDownloader.ms_refCacheDic.Add(wItem.assetPath, value);
					this.callback(wItem, this.param);
				}
				ResourceCache.WWWDownloader tobj = wwwdownload as ResourceCache.WWWDownloader;
				ObjectPoolManager.Release<ResourceCache.WWWDownloader>(tobj);
			}
		}

		private static Dictionary<string, ResourceCache.LifeSaver> ms_CacheDic;

		public static float timeOver
		{
			get;
			set;
		}

		static ResourceCache()
		{
			ResourceCache.ms_CacheDic = new Dictionary<string, ResourceCache.LifeSaver>();
			ResourceCache.WWWDownloader.ms_refCacheDic = ResourceCache.ms_CacheDic;
			ResourceCache.timeOver = 120f;
		}

		public static string GetDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.AppendFormat("Total : {0} ---------", ResourceCache.ms_CacheDic.Count);
			foreach (KeyValuePair<string, ResourceCache.LifeSaver> current in ResourceCache.ms_CacheDic)
			{
				stringBuilder.AppendFormat("\n{0}", current.Key);
			}
			return stringBuilder.ToString();
		}

		public static object GetResource(string strKey)
		{
			string key = strKey.ToLower();
			ResourceCache.LifeSaver lifeSaver;
			if (ResourceCache.ms_CacheDic.TryGetValue(key, out lifeSaver))
			{
				return lifeSaver.Acquire();
			}
			return null;
		}

		public static object LoadFromResourcesImmediate(string strKey)
		{
			string arg = strKey.ToLower();
			string path = string.Empty;
			if (strKey.IndexOf("Shader/".ToLower()) < 0)
			{
				if (TsPlatform.IsMobile)
				{
					path = string.Format("Mobile/{0}", arg);
				}
				else
				{
					path = string.Format("WebPlayer/{0}", arg);
				}
			}
			return Resources.Load(path);
		}

		public static object LoadFromResources(string strKey)
		{
			if (string.IsNullOrEmpty(strKey))
			{
				Debug.LogWarning("key name can't be null.");
				return null;
			}
			string text = strKey.ToLower();
			object obj = ResourceCache.GetResource(text);
			if (obj != null)
			{
				return obj;
			}
			obj = ResourceCache.LoadFromResourcesImmediate(text);
			if (obj == null)
			{
				TsLog.LogError("Fail! CResources::Load - {0}\n\\rCallStack : {1}", new object[]
				{
					text,
					StackTraceUtility.ExtractStackTrace()
				});
			}
			else
			{
				ResourceCache.ms_CacheDic.Add(text, new ResourceCache.LifeSaver(obj));
			}
			return obj;
		}

		public static UnityEngine.Object LoadFromResourcesByClone(string keyLower)
		{
			UnityEngine.Object @object = ResourceCache.LoadFromResources(keyLower) as UnityEngine.Object;
			if (null == @object)
			{
				TsLog.LogError("Fail! CResources::LoadClone - {0}\n\\rCallStack : {1}", new object[]
				{
					keyLower,
					StackTraceUtility.ExtractStackTrace()
				});
				return null;
			}
			return UnityEngine.Object.Instantiate(@object);
		}

		public static GameObject LoadFromResourcesWithClone(string keyLower, GameObject goParent)
		{
			GameObject gameObject = ResourceCache.LoadFromResources(keyLower) as GameObject;
			if (null == gameObject)
			{
				TsLog.LogError("Fail! CResources::LoadClone - {0}\n\\rCallStack : {1}", new object[]
				{
					keyLower,
					StackTraceUtility.ExtractStackTrace()
				});
				return null;
			}
			return ResourceCache.CloneObject(gameObject, goParent);
		}

		public static GameObject CloneObject(GameObject goChild, GameObject goParent)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(goChild, Vector3.zero, Quaternion.identity) as GameObject;
			if (null != gameObject && null != gameObject.transform && null != goParent)
			{
				gameObject.transform.parent = goParent.transform;
			}
			ResourceCache.SetLocal_I(gameObject, goChild);
			return gameObject;
		}

		private static void SetLocal_I(GameObject goTo, GameObject goFrom)
		{
			goTo.transform.localPosition = goFrom.transform.position;
			goTo.transform.localRotation = goFrom.transform.rotation;
			goTo.transform.localScale = goFrom.transform.localScale;
		}

		public static void LoadAsyncFromWWW(string strKey, PostProcPerItem callback, object param, string bundleStackName)
		{
			string key = strKey.ToLower();
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(key, bundleStackName);
			wWWItem.SetItemType(ItemType.TEXTURE2D);
			ResourceCache.WWWDownloader wWWDownloader = ObjectPoolManager.Acquire<ResourceCache.WWWDownloader>(callback);
			wWWDownloader.SetParam(param);
			wWWItem.SetCallback(new PostProcPerItem(wWWDownloader.OnCompleteDonwload), wWWDownloader);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}

		public static void Remove(string keyLower)
		{
			ResourceCache.ms_CacheDic.Remove(keyLower);
		}

		public static void ClearCache()
		{
			ResourceCache.ms_CacheDic.Clear();
		}

		[Obsolete("현우 부장님~~ 지워주세요 ^^")]
		public static void UnloadUnusedAssetResourceCache()
		{
			float time = Time.time;
			foreach (KeyValuePair<string, ResourceCache.LifeSaver> current in ResourceCache.ms_CacheDic)
			{
				if (current.Value.isTimeOut(time))
				{
					ResourceCache.ms_CacheDic.Remove(current.Key);
					TsLog.Log("ResourceCache.UnloadUnusedAssetResourceCache - {0}", new object[]
					{
						current.Key
					});
					break;
				}
			}
		}
	}
}
