using System;
using UnityEngine;

namespace TsBundle
{
	public interface IDownloadedItem
	{
		bool isSuccess
		{
			get;
		}

		bool isCanceled
		{
			get;
		}

		bool isFileNotFound
		{
			get;
		}

		string stateString
		{
			get;
		}

		string errorString
		{
			get;
		}

		ItemType itemType
		{
			get;
		}

		int safeSize
		{
			get;
		}

		bool canAccessAssetBundle
		{
			get;
		}

		bool canAccessString
		{
			get;
		}

		bool canAccessBytes
		{
			get;
		}

		bool canAccessAudioClip
		{
			get;
		}

		string safeString
		{
			get;
		}

		byte[] safeBytes
		{
			get;
		}

		byte[] rawBytes
		{
			get;
		}

		AudioClip safeAudioClip
		{
			get;
		}

		UnityEngine.Object mainAsset
		{
			get;
		}

		bool unloadImmediate
		{
			set;
		}

		string assetName
		{
			get;
		}

		string assetPath
		{
			get;
		}

		string url
		{
			get;
		}

		string strParam
		{
			get;
		}

		int indexParam
		{
			get;
		}

		string stackName
		{
			get;
		}

		int version
		{
			get;
		}

		bool UseCustomCache
		{
			get;
		}

		AssetBundle GetSafeBundle();

		void SafeBundleUnload(bool unloadAllLoadedObject);

		UnityEngine.Object Load(string name);

		UnityEngine.Object Load(string name, Type type);

		AssetBundleRequest LoadAsync(string name, Type type);
	}
}
