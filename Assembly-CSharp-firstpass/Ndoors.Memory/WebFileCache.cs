using Ndoors.Framework.Stage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TsBundle;
using UnityEngine;

namespace Ndoors.Memory
{
	public static class WebFileCache
	{
		private class CacheItem
		{
			internal class ItemPair
			{
				internal WebFileCache.ReqTextureCallback callback;

				internal object cbparam;

				internal ItemPair(WebFileCache.ReqTextureCallback callback, object cbparam)
				{
					this.callback = callback;
					this.cbparam = cbparam;
				}
			}

			private enum Status
			{
				ERROR = -1,
				CREATED,
				SUCCESS
			}

			internal float lastAccessTime;

			private List<WebFileCache.CacheItem.ItemPair> callback_list = new List<WebFileCache.CacheItem.ItemPair>();

			private WebFileCache.CacheItem.Status status;

			public Texture2D txtr2d;

			public bool isSuccess
			{
				get
				{
					return this.status == WebFileCache.CacheItem.Status.SUCCESS;
				}
			}

			public bool isError
			{
				get
				{
					return this.status == WebFileCache.CacheItem.Status.ERROR;
				}
			}

			public CacheItem(string url, WebFileCache.ReqTextureCallback callback, object cbparam)
			{
				this.AddCallback(callback, cbparam);
			}

			public void AddCallback(WebFileCache.ReqTextureCallback callback, object cbparam)
			{
				this.callback_list.Add(new WebFileCache.CacheItem.ItemPair(callback, cbparam));
			}

			public void ReqWebImage(string urlwithrandom)
			{
				this.lastAccessTime = Time.time;
				Helper.RequestDownloadWebFile(urlwithrandom, true, new PostProcPerItem(this.ReqWebImageCallback), null);
			}

			private void ReqWebImageCallback(IDownloadedItem wItem, object obj)
			{
				if (!wItem.canAccessBytes)
				{
					this.status = WebFileCache.CacheItem.Status.ERROR;
					this.txtr2d = null;
				}
				else
				{
					this.status = WebFileCache.CacheItem.Status.SUCCESS;
					this.txtr2d = new Texture2D(0, 0, TextureFormat.DXT1, false);
					this.txtr2d.LoadImage(wItem.safeBytes);
					this.txtr2d.filterMode = FilterMode.Bilinear;
					this.txtr2d.mipMapBias = -1f;
				}
				foreach (WebFileCache.CacheItem.ItemPair current in this.callback_list)
				{
					current.callback(this.txtr2d, current.cbparam);
				}
				this.callback_list.Clear();
				this.callback_list = null;
			}
		}

		public delegate void ReqTextureCallback(Texture2D txtr, object obj);

		private static Dictionary<string, WebFileCache.CacheItem> ms_webfiles;

		private static Dictionary<string, float> ms_DelWebfile;

		static WebFileCache()
		{
			WebFileCache.ms_webfiles = new Dictionary<string, WebFileCache.CacheItem>();
			WebFileCache.ms_DelWebfile = new Dictionary<string, float>();
			StageSystem.AddCommonPararellTask(WebFileCache.RemoveOldFile());
			StageSystem.AddCommonPararellTask(WebFileCache.RemoveDeleteOldFile());
		}

		public static void RequestImageWebFile(string url, WebFileCache.ReqTextureCallback callback, object callbackParam)
		{
			WebFileCache.CacheItem cacheItem = null;
			string text = url;
			if (url.Contains("?"))
			{
				text = url.Substring(0, url.LastIndexOf("?"));
			}
			if (WebFileCache.ms_webfiles.TryGetValue(text, out cacheItem))
			{
				if (cacheItem.isSuccess)
				{
					callback(cacheItem.txtr2d, callbackParam);
				}
				else if (!cacheItem.isError)
				{
					cacheItem.AddCallback(callback, callbackParam);
				}
				else
				{
					callback(null, callbackParam);
				}
				cacheItem.lastAccessTime = Time.time;
			}
			else
			{
				cacheItem = new WebFileCache.CacheItem(text, callback, callbackParam);
				WebFileCache.ms_webfiles.Add(text, cacheItem);
				cacheItem.ReqWebImage(url);
			}
		}

		public static Texture2D RequestImagFileCache(string url, WebFileCache.ReqTextureCallback callback, object callbackParam)
		{
			WebFileCache.CacheItem cacheItem = null;
			string text = url;
			if (url.Contains("?"))
			{
				text = url.Substring(0, url.LastIndexOf("?"));
			}
			if (WebFileCache.ms_webfiles.TryGetValue(text, out cacheItem))
			{
				if (cacheItem.isSuccess)
				{
					callback(cacheItem.txtr2d, callbackParam);
				}
				else if (!cacheItem.isError)
				{
					cacheItem.AddCallback(callback, callbackParam);
				}
				else
				{
					callback(null, callbackParam);
				}
				cacheItem.lastAccessTime = Time.time;
			}
			else
			{
				TsLog.LogError("request imgae = {0}", new object[]
				{
					text
				});
				cacheItem = new WebFileCache.CacheItem(text, callback, callbackParam);
				WebFileCache.ms_webfiles.Add(text, cacheItem);
				cacheItem.ReqWebImage(url);
			}
			return null;
		}

		public static void ClearWebFileCache()
		{
			WebFileCache.ms_webfiles.Clear();
			WebFileCache.ms_DelWebfile.Clear();
		}

		public static void RemoveItem(string url)
		{
			string url2 = url;
			if (url.Contains("?"))
			{
				url2 = url.Substring(0, url.LastIndexOf("?"));
			}
			WebFileCache.RemoveEventItem(url2);
		}

		public static void RemoveEventItem(string url)
		{
			if (WebFileCache.ms_DelWebfile.ContainsKey(url))
			{
				return;
			}
			if (WebFileCache.ms_webfiles.ContainsKey(url))
			{
				WebFileCache.ms_webfiles.Remove(url);
				WebFileCache.ms_DelWebfile.Add(url, Time.time);
			}
		}

		[DebuggerHidden]
		public static IEnumerator RemoveDeleteOldFile()
		{
			return new WebFileCache.<RemoveDeleteOldFile>c__Iterator12();
		}

		[DebuggerHidden]
		public static IEnumerator RemoveOldFile()
		{
			return new WebFileCache.<RemoveOldFile>c__Iterator13();
		}

		public static string GetDebugString()
		{
			int num = 0;
			foreach (WebFileCache.CacheItem current in WebFileCache.ms_webfiles.Values)
			{
				if (current.isError)
				{
					num++;
				}
			}
			return string.Format("Total Count = {0}\nError Count = {1}", WebFileCache.ms_webfiles.Count, num);
		}
	}
}
