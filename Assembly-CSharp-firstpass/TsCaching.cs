using System;
using TsBundle;
using UnityEngine;

public static class TsCaching
{
	private const long CachingSize = 10737418240L;

	private static bool bUseCustomCacheOnly = PlayerPrefs.GetInt("customcacheonly", 0) == 1;

	private static readonly string s_LastSpaceOccupied_KEY = "LastSOKey";

	private static bool m_EnableCacheReady = PlayerPrefs.GetInt("EnableCacheReady", 1) != 0;

	private static bool m_hitCacheReady = false;

	public static bool useCustomCacheOnly
	{
		get
		{
			return TsCaching.bUseCustomCacheOnly;
		}
		set
		{
			TsCaching.bUseCustomCacheOnly = value;
		}
	}

	public static bool EnableCacheReady
	{
		get
		{
			return TsCaching.m_EnableCacheReady;
		}
		set
		{
			TsCaching.m_EnableCacheReady = value;
			PlayerPrefs.SetInt("EnableCacheReady", (!value) ? 0 : 1);
		}
	}

	public static bool ready
	{
		get
		{
			if (TsCaching.EnableCacheReady && TsPlatform.IsWeb && NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				bool ready = Caching.ready;
				if (ready && !TsCaching.m_hitCacheReady)
				{
					TsCaching.m_hitCacheReady = true;
					TsLog.Log("[Caching] CacheReady => OK", new object[0]);
				}
				return ready;
			}
			return true;
		}
	}

	public static long spaceFree
	{
		get
		{
			if (TsCaching.useCustomCacheOnly)
			{
				return CustomCaching.spaceFree;
			}
			return Caching.spaceFree;
		}
	}

	public static long spaceOccupied
	{
		get
		{
			if (TsCaching.useCustomCacheOnly)
			{
				return CustomCaching.spaceOccupied;
			}
			return Caching.spaceOccupied;
		}
	}

	public static void InitiailzeCustomCaching(string url, string local)
	{
		if (string.IsNullOrEmpty(url))
		{
			TsLog.LogError("CustomCaching. Fail initialization. web url is null", new object[0]);
			return;
		}
		if (string.IsNullOrEmpty(local))
		{
			TsLog.LogError("CustomCaching. Fail initialization. local path is null", new object[0]);
			return;
		}
		TsLog.LogWarning("$$$SysConfig:InitializeCustomCaching.........", new object[0]);
		CustomCaching.InitiailzeCustumCaching(url, local);
	}

	public static long GetLastSpaceOccupied()
	{
		long spaceOccupied = Caching.spaceOccupied;
		if (!PlayerPrefs.HasKey(TsCaching.s_LastSpaceOccupied_KEY))
		{
			return spaceOccupied;
		}
		string @string = PlayerPrefs.GetString(TsCaching.s_LastSpaceOccupied_KEY);
		if (!long.TryParse(@string, out spaceOccupied))
		{
			TsLog.LogError("Filed~! Parse Last SapceOccupied stringValue[{0}]", new object[]
			{
				@string
			});
			return spaceOccupied;
		}
		return spaceOccupied;
	}

	public static void SaveLastSpaceOccupied()
	{
		TsCaching.SaveLastSpaceOccupied(Caching.spaceOccupied);
	}

	public static void SaveLastSpaceOccupied(long spaceOccupied)
	{
		PlayerPrefs.SetString(TsCaching.s_LastSpaceOccupied_KEY, spaceOccupied.ToString());
		PlayerPrefs.Save();
	}

	public static void AuthorizeInner()
	{
	}

	public static void AuthorizeOuter()
	{
	}

	public static bool Authorize(string name, string domain, long size, int expiration, string signature)
	{
		return Caching.Authorize(name, domain, size, expiration, signature);
	}

	public static bool CleanCache()
	{
		TsCaching.SaveLastSpaceOccupied(0L);
		if (TsCaching.useCustomCacheOnly)
		{
			return CustomCaching.CleanCache();
		}
		return Caching.CleanCache();
	}

	public static bool IsVersionCached(string url, int version, bool forceUseCustomCache)
	{
		bool flag = (!TsCaching.useCustomCacheOnly && !forceUseCustomCache) ? Caching.IsVersionCached(url, version) : CustomCaching.IsVersionCached(url, version);
		if (Option.EnableTrace)
		{
			TsLog.Log("[TsBundle] IsVersionCached( url=\"{0}\", version={1} ) => {2}", new object[]
			{
				url,
				version,
				flag
			});
		}
		return flag;
	}

	public static WWW LoadFromCacheOrDownload(string url, int version, long fileSize, bool forceUseCustomCache, WWWItem wItem)
	{
		if (TsCaching.useCustomCacheOnly || forceUseCustomCache)
		{
			return CustomCaching.LoadFromCacheOrDownload(url, version, fileSize, wItem);
		}
		return WWW.LoadFromCacheOrDownload(url, version);
	}

	public static void SaveCacheList()
	{
		CustomCaching.SaveCacheList();
	}

	public static void MarkAsUsed(string assetPath, int nVersion, bool bUseCustomCache)
	{
		if (!bUseCustomCache)
		{
			Caching.MarkAsUsed(assetPath, nVersion);
		}
	}
}
