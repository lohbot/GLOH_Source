using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TsBundle;
using UnityEngine;

public static class CustomCaching
{
	private class CacheItem
	{
		public bool isExistLocalFile;

		public string szMD5 = string.Empty;

		public TsWeakReference<WWW> wref;

		public string url
		{
			get;
			set;
		}

		public int version
		{
			get;
			set;
		}

		public long size
		{
			get;
			set;
		}
	}

	private static string _webRoot;

	private static string _locRoot;

	private static Dictionary<string, CustomCaching.CacheItem> _cacheList;

	private static long _spaceOccupied;

	public static long spaceFree
	{
		get
		{
			TsLog.LogWarning("CustomCaching.spaceFree is not yet implemented.", new object[0]);
			return 1L;
		}
	}

	public static long spaceOccupied
	{
		get
		{
			return CustomCaching._spaceOccupied;
		}
	}

	static CustomCaching()
	{
		CustomCaching._webRoot = string.Empty;
		CustomCaching._locRoot = string.Empty;
		CustomCaching._cacheList = null;
		CustomCaching._spaceOccupied = 0L;
	}

	public static void InitiailzeCustumCaching(string url, string local)
	{
		CustomCaching._webRoot = url.ToLower();
		if (local.Contains(":///"))
		{
			CustomCaching._locRoot = local.Substring(8);
		}
		else if (local.Contains("://"))
		{
			CustomCaching._locRoot = local.Substring(7);
		}
		else
		{
			CustomCaching._locRoot = local;
		}
		CustomCaching._cacheList = new Dictionary<string, CustomCaching.CacheItem>();
		CustomCaching._ReadCacheList();
	}

	public static void AddCacheList(string url, int nVersion)
	{
		CustomCaching.CacheItem cacheItem = null;
		CustomCaching._cacheList.TryGetValue(url, out cacheItem);
		if (cacheItem == null)
		{
			cacheItem = new CustomCaching.CacheItem();
			cacheItem.url = url;
			cacheItem.version = nVersion;
			CustomCaching._cacheList.Add(cacheItem.url, cacheItem);
		}
		else if (cacheItem.version < nVersion)
		{
			cacheItem.version = nVersion;
		}
	}

	private static void _ReadCacheList()
	{
		string path = string.Format("{0}cachelist.txt", CustomCaching._locRoot);
		CustomCaching._cacheList.Clear();
		if (File.Exists(path))
		{
			try
			{
				using (Stream stream = File.Open(path, FileMode.Open))
				{
					using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8))
					{
						while (binaryReader.PeekChar() != -1)
						{
							CustomCaching.CacheItem cacheItem = new CustomCaching.CacheItem();
							cacheItem.url = binaryReader.ReadString().ToLower();
							cacheItem.version = binaryReader.ReadInt32();
							cacheItem.size = binaryReader.ReadInt64();
							CustomCaching._spaceOccupied += cacheItem.size;
							CustomCaching._cacheList.Add(cacheItem.url, cacheItem);
						}
					}
				}
			}
			catch (Exception obj)
			{
				TsLog.LogWarning(obj);
			}
			TsLog.Log("CustomCaching. Read cache item list. ({0})", new object[]
			{
				CustomCaching._cacheList.Count
			});
		}
	}

	public static void SaveCacheList()
	{
		string path = string.Format("{0}cachelist.txt", CustomCaching._locRoot);
		try
		{
			using (Stream stream = File.Create(path))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
				{
					foreach (CustomCaching.CacheItem current in CustomCaching._cacheList.Values)
					{
						binaryWriter.Write(current.url);
						binaryWriter.Write(current.version);
						binaryWriter.Write(current.size);
					}
				}
			}
		}
		catch (Exception obj)
		{
			TsLog.LogWarning(obj);
		}
	}

	public static bool CleanCache()
	{
		return true;
	}

	public static bool IsVersionCached(string url, int version)
	{
		CustomCaching.CacheItem cacheItem;
		string text;
		return CustomCaching._IsVersionCached(url, version, out cacheItem, out text);
	}

	public static void DeleteCacheFolder(string strRootPath)
	{
		if (Directory.Exists(strRootPath))
		{
			TsPlatform.Operator.DeleteDirectory(strRootPath, true);
			CustomCaching._cacheList.Clear();
		}
	}

	private static bool _IsVersionCached(string url, int version, out CustomCaching.CacheItem cItem, out string localPath)
	{
		cItem = CustomCaching._GetCacheItem(url);
		if (cItem == null || cItem.version < version)
		{
			localPath = null;
			return false;
		}
		localPath = string.Format("{0}{1}", CustomCaching._locRoot, cItem.url);
		if (!cItem.isExistLocalFile)
		{
			cItem.isExistLocalFile = CustomCaching._IsExistLocalFile(localPath);
		}
		return cItem.isExistLocalFile;
	}

	private static string _UrlToKey(string url)
	{
		string text;
		if (url.Contains(":"))
		{
			text = url.Substring(CustomCaching._webRoot.Length);
			if (Option.usePatchDir)
			{
				int num = text.IndexOf('/');
				if (num == -1)
				{
					num = text.IndexOf('\\');
				}
				if (num != -1)
				{
					string s = text.Substring(0, num).TrimStart(new char[]
					{
						'/',
						'\\'
					});
					int num2;
					if (int.TryParse(s, out num2))
					{
						text = text.Substring(num + 1);
					}
				}
			}
		}
		else
		{
			text = url;
		}
		return text;
	}

	private static CustomCaching.CacheItem _GetCacheItem(string url)
	{
		CustomCaching.CacheItem cacheItem = null;
		CustomCaching._cacheList.TryGetValue(CustomCaching._UrlToKey(url), out cacheItem);
		if (cacheItem == null)
		{
			TsLog.Log("CustomCaching._GetCacheItem is null = " + CustomCaching._UrlToKey(url), new object[0]);
		}
		return cacheItem;
	}

	private static void _RemoveCacheItem(string url)
	{
		string key = CustomCaching._UrlToKey(url);
		if (CustomCaching._cacheList.ContainsKey(key))
		{
			CustomCaching._cacheList.Remove(key);
		}
	}

	private static bool _IsExistLocalFile(string path)
	{
		if (!File.Exists(path))
		{
			return false;
		}
		long fileLength = TsPlatform.Operator.GetFileLength(path);
		return fileLength > 0L;
	}

	public static WWW LoadFromCacheOrDownload(string url, int version, long fileSize, WWWItem wItem)
	{
		CustomCaching.CacheItem cacheItem;
		string arg;
		bool flag = CustomCaching._IsVersionCached(url, version, out cacheItem, out arg);
		WWW wWW;
		if (flag)
		{
			if (cacheItem.wref != null && cacheItem.wref.Target != null)
			{
				TsLog.Assert(cacheItem.wref.IsAlive, "CustomCaching: Duplicated request! but WWW reference is removed! (url=\"{0}\")", new object[]
				{
					cacheItem.url
				});
				wWW = cacheItem.wref.CastedTarget;
			}
			else
			{
				string url2 = string.Format("file://{0}", arg);
				wWW = new WWW(url2);
			}
		}
		else
		{
			wWW = new WWW(url);
			if (cacheItem == null)
			{
				if (CustomCaching._cacheList.TryGetValue(wItem.assetPath, out cacheItem))
				{
					TsLog.LogWarning("CustomCaching. Duplicated cache list item (path=\"{0}\")", new object[]
					{
						wItem.assetPath
					});
				}
				cacheItem = new CustomCaching.CacheItem();
				cacheItem.url = wItem.assetPath;
			}
			cacheItem.wref = wWW;
			cacheItem.version = version;
			cacheItem.size = fileSize;
			wItem.SetCallback(new PostProcPerItem(CustomCaching.CallbackSaveAssetBundle), cacheItem);
		}
		return wWW;
	}

	private static void CallbackSaveAssetBundle(IDownloadedItem wItem, object obj)
	{
		try
		{
			CustomCaching.CacheItem cacheItem = obj as CustomCaching.CacheItem;
			if (cacheItem == null)
			{
				TsLog.LogWarning("CacheItem not found. (assetPath=\"{0}\")", new object[]
				{
					wItem.assetPath
				});
			}
			else if (!wItem.isCanceled && string.IsNullOrEmpty(wItem.errorString))
			{
				string text = string.Format("{0}{1}", CustomCaching._locRoot, wItem.assetPath);
				string[] array = text.Split(new char[]
				{
					'/'
				});
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length - 1; i++)
				{
					stringBuilder.AppendFormat("{0}{1}", array[i], '/');
					string path = stringBuilder.ToString();
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
				}
				try
				{
					if (wItem.rawBytes == null)
					{
						throw new NullReferenceException(string.Format("[CustomCache] wItem.rawBytes is null. (path=\"{0}\")\n", wItem.assetPath));
					}
					cacheItem.isExistLocalFile = false;
					if (File.Exists(text))
					{
						File.Delete(text);
					}
					using (FileStream fileStream = File.Create(text))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8))
						{
							byte[] rawBytes = wItem.rawBytes;
							binaryWriter.Write(rawBytes, 0, rawBytes.Length);
							long num = (long)rawBytes.Length;
							CustomCaching._spaceOccupied += cacheItem.size;
							if (num != cacheItem.size)
							{
								TsLog.LogError("Not Match File={0} OrgSize={1} DownSize={2}", new object[]
								{
									wItem.assetPath,
									cacheItem.size,
									num
								});
								if (CustomCaching._cacheList.ContainsKey(cacheItem.url))
								{
									CustomCaching._cacheList.Remove(cacheItem.url);
								}
								if (File.Exists(text))
								{
									File.Delete(text);
								}
							}
							else if (!CustomCaching._cacheList.ContainsKey(cacheItem.url))
							{
								CustomCaching._cacheList.Add(wItem.assetPath, cacheItem);
							}
							else
							{
								CustomCaching._cacheList.Remove(cacheItem.url);
								CustomCaching._cacheList.Add(wItem.assetPath, cacheItem);
							}
						}
					}
				}
				catch (Exception ex)
				{
					CustomCaching._RemoveCacheItem(cacheItem.url);
					TsLog.Assert(false, "Cache file save error! (Path=\"{1}\") : {0}", new object[]
					{
						ex,
						text
					});
				}
				cacheItem.wref = null;
			}
			else
			{
				CustomCaching._RemoveCacheItem(cacheItem.url);
				TsLog.LogError("CustomCaching. Error file saving (url=\"{0}\", error={1}, canceled={2})", new object[]
				{
					cacheItem.url,
					wItem.errorString,
					wItem.isCanceled
				});
			}
			TsCaching.SaveCacheList();
		}
		catch (Exception obj2)
		{
			TsLog.LogWarning(obj2);
		}
	}
}
