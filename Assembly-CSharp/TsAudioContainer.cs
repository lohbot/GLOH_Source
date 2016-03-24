using SERVICE;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using TsBundle;
using TsLibs;
using UnityEngine;

public class TsAudioContainer
{
	public class Domain
	{
		internal class _SorterCategory : IComparer<TsAudioContainer.Category>
		{
			public int Compare(TsAudioContainer.Category x, TsAudioContainer.Category y)
			{
				return x.Key.CompareTo(y.Key);
			}
		}

		private List<TsAudioContainer.Category> _categories;

		private string _key;

		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this._key = value;
				}
			}
		}

		public int CategoryCount
		{
			get
			{
				return this._categories.Count;
			}
		}

		public string[] CategoryKeys
		{
			get
			{
				List<string> list = new List<string>();
				foreach (TsAudioContainer.Category current in this._categories)
				{
					list.Add(current.Key);
				}
				return list.ToArray();
			}
		}

		public TsAudioContainer.Category[] CategoryArray
		{
			get
			{
				return this._categories.ToArray();
			}
		}

		private Domain()
		{
		}

		public static TsAudioContainer.Domain Create(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			return new TsAudioContainer.Domain
			{
				_key = key,
				_categories = new List<TsAudioContainer.Category>()
			};
		}

		public TsAudioContainer.Category AddCategory(string categoryKey)
		{
			if (string.IsNullOrEmpty(categoryKey))
			{
				return null;
			}
			TsAudioContainer.Category category = this.FindCategory(categoryKey);
			if (category == null)
			{
				category = TsAudioContainer.Category.Create(categoryKey);
				if (category != null)
				{
					this._categories.Add(category);
				}
			}
			return category;
		}

		public bool EditCategory(string currentKey, string newKey)
		{
			TsAudioContainer.Category category = this.FindCategory(currentKey);
			if (category == null)
			{
				return false;
			}
			category.Key = newKey;
			return true;
		}

		public void RemoveCategory(string categoryKey)
		{
			TsAudioContainer.Category category = this.FindCategory(categoryKey);
			if (category != null)
			{
				this._categories.Remove(category);
			}
		}

		public TsAudioContainer.Category FindCategory(string categoryKey)
		{
			if (string.IsNullOrEmpty(categoryKey))
			{
				return null;
			}
			foreach (TsAudioContainer.Category current in this._categories)
			{
				if (current.Key.Equals(categoryKey, StringComparison.CurrentCultureIgnoreCase))
				{
					return current;
				}
			}
			return null;
		}

		public TsAudioContainer.Category FindCategory(int index)
		{
			if (index < 0 || this._categories.Count <= index)
			{
				return null;
			}
			return this._categories[index];
		}

		public void SortCategories()
		{
			this._categories.Sort(new TsAudioContainer.Domain._SorterCategory());
		}
	}

	public class Category
	{
		internal class _SorterItem : IComparer<TsAudioContainer.Item>
		{
			public int Compare(TsAudioContainer.Item x, TsAudioContainer.Item y)
			{
				return x.Key.CompareTo(y.Key);
			}
		}

		private List<TsAudioContainer.Item> _items;

		private string _key;

		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this._key = value;
				}
			}
		}

		public int ItemCount
		{
			get
			{
				return this._items.Count;
			}
		}

		public string[] ItemKeys
		{
			get
			{
				List<string> list = new List<string>();
				foreach (TsAudioContainer.Item current in this._items)
				{
					list.Add(current.Key);
				}
				return list.ToArray();
			}
		}

		public TsAudioContainer.Item[] ItemArray
		{
			get
			{
				return this._items.ToArray();
			}
		}

		private Category()
		{
		}

		public static TsAudioContainer.Category Create(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			return new TsAudioContainer.Category
			{
				_key = key,
				_items = new List<TsAudioContainer.Item>()
			};
		}

		public TsAudioContainer.Item AddItem(string itemKey)
		{
			TsAudioContainer.Item item = this.FindItem(itemKey);
			if (item == null)
			{
				item = TsAudioContainer.Item.Create(itemKey);
				if (item != null)
				{
					this._items.Add(item);
				}
			}
			return item;
		}

		public bool EditItem(string currentKey, string newKey)
		{
			TsAudioContainer.Item item = this.FindItem(currentKey);
			if (item == null)
			{
				return false;
			}
			item.Key = newKey;
			return true;
		}

		public void RemoveItem(string itemKey)
		{
			TsAudioContainer.Item item = this.FindItem(itemKey);
			if (item != null)
			{
				this._items.Remove(item);
			}
		}

		public TsAudioContainer.Item FindItem(string itemKey)
		{
			if (string.IsNullOrEmpty(itemKey))
			{
				return null;
			}
			foreach (TsAudioContainer.Item current in this._items)
			{
				if (current.Key.Equals(itemKey, StringComparison.CurrentCultureIgnoreCase))
				{
					return current;
				}
			}
			return null;
		}

		public TsAudioContainer.Item FindItem(int index)
		{
			if (index < 0 || this._items.Count <= index)
			{
				return null;
			}
			return this._items[index];
		}

		public void SortItems()
		{
			this._items.Sort(new TsAudioContainer.Category._SorterItem());
		}
	}

	public class Item
	{
		internal class _SorterBundle : IComparer<TsAudioContainer.Bundle>
		{
			public int Compare(TsAudioContainer.Bundle x, TsAudioContainer.Bundle y)
			{
				return x.Key.CompareTo(y.Key);
			}
		}

		public enum CollectReturnType
		{
			Key,
			AssetPath,
			BundleName
		}

		private List<TsAudioContainer.Bundle> _bundles;

		private string _key = string.Empty;

		private EAudioType _audioType;

		private TsAudio.RandomizeVolume _volumeRand;

		private TsAudio.RandomizePitch _pitchRand;

		private bool _isPredownload;

		private bool _skipIfPlayingSame;

		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this._key = value;
				}
			}
		}

		public int BundleCount
		{
			get
			{
				return this._bundles.Count;
			}
		}

		public string[] BundleKeys
		{
			get
			{
				List<string> list = new List<string>();
				foreach (TsAudioContainer.Bundle current in this._bundles)
				{
					list.Add(current.Key);
				}
				return list.ToArray();
			}
		}

		public TsAudioContainer.Bundle[] BundleArray
		{
			get
			{
				return this._bundles.ToArray();
			}
		}

		public EAudioType AudioType
		{
			get
			{
				return this._audioType;
			}
			set
			{
				this._audioType = value;
			}
		}

		public TsAudio.RandomizeVolume VolumeRand
		{
			get
			{
				return this._volumeRand;
			}
		}

		public TsAudio.RandomizePitch PitchRand
		{
			get
			{
				return this._pitchRand;
			}
		}

		public bool IsPredownload
		{
			get
			{
				return this._isPredownload;
			}
			set
			{
				this._isPredownload = value;
			}
		}

		public bool SkipIfPlayingSame
		{
			get
			{
				return this._skipIfPlayingSame;
			}
			set
			{
				this._skipIfPlayingSame = value;
			}
		}

		private Item()
		{
		}

		public static TsAudioContainer.Item Create(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			return new TsAudioContainer.Item
			{
				_key = key,
				_bundles = new List<TsAudioContainer.Bundle>(),
				_volumeRand = new TsAudio.RandomizeVolume(),
				_pitchRand = new TsAudio.RandomizePitch()
			};
		}

		public TsAudioContainer.Bundle AddBundle(string key, string bundleName, string assetPath)
		{
			TsAudioContainer.Bundle bundle = this.FindBundle(key);
			if (bundle == null)
			{
				bundle = TsAudioContainer.Bundle.Create(key, bundleName, assetPath);
				if (bundle != null)
				{
					this._bundles.Add(bundle);
				}
			}
			return bundle;
		}

		public void RemoveBundle(string key)
		{
			TsAudioContainer.Bundle bundle = this.FindBundle(key);
			if (bundle != null)
			{
				this._bundles.Remove(bundle);
			}
		}

		public TsAudioContainer.Bundle FindBundle(int index)
		{
			if (index < 0 || this._bundles.Count <= index)
			{
				return null;
			}
			return this._bundles[index];
		}

		public TsAudioContainer.Bundle FindBundle(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			foreach (TsAudioContainer.Bundle current in this._bundles)
			{
				if (current.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
				{
					return current;
				}
			}
			return null;
		}

		public void SortBundles()
		{
			this._bundles.Sort(new TsAudioContainer.Item._SorterBundle());
		}

		public string[] CollectBundleDatas(TsAudioContainer.Item.CollectReturnType collectType)
		{
			List<string> list = new List<string>();
			foreach (TsAudioContainer.Bundle current in this._bundles)
			{
				switch (collectType)
				{
				case TsAudioContainer.Item.CollectReturnType.Key:
					list.Add(current.Key);
					break;
				case TsAudioContainer.Item.CollectReturnType.AssetPath:
					list.Add(current.AssetPath);
					break;
				case TsAudioContainer.Item.CollectReturnType.BundleName:
					list.Add(current.BundleName);
					break;
				}
			}
			return list.ToArray();
		}
	}

	public class Bundle
	{
		private TsAudioBundleInfo _info;

		public TsAudioBundleInfo Info
		{
			get
			{
				return this._info;
			}
		}

		public string Key
		{
			get
			{
				return this._info.AudioClipName;
			}
		}

		public string BundleName
		{
			get
			{
				return this._info.AssetBundleNameWithoutExtension;
			}
		}

		public string AssetPath
		{
			get
			{
				return this._info.AssetPathOfAudioClip;
			}
		}

		private Bundle()
		{
		}

		public static TsAudioContainer.Bundle Create(string key, string bundleName, string assetPath)
		{
			if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(bundleName) || string.IsNullOrEmpty(assetPath))
			{
				return null;
			}
			TsAudioContainer.Bundle bundle = new TsAudioContainer.Bundle();
			bundle._info = new TsAudioBundleInfo();
			if (!bundle._info.FillBundleInfo(key, assetPath, bundleName))
			{
				return null;
			}
			return bundle;
		}
	}

	internal class _SorterDomain : IComparer<TsAudioContainer.Domain>
	{
		public int Compare(TsAudioContainer.Domain x, TsAudioContainer.Domain y)
		{
			return x.Key.CompareTo(y.Key);
		}
	}

	public static bool PatchCombine = true;

	private readonly string ROOT_E = "AUDIO_CONTAINER";

	private readonly string ROOT_A_VERSION = "version";

	private readonly string VOLUMESCALE_E = "VOLUME_SCALE";

	private readonly string VOLUMESCALE_E_TYPE = "TYPE";

	private readonly string VOLUMESCALE_A_KEY = "key";

	private readonly string VOLUMESCALE_A_VALUE = "value";

	private readonly string DOMAIN_E = "DOMAIN";

	private readonly string DOMAIN_A_KEY = "key";

	private readonly string CATEGORY_E = "CATEGORY";

	private readonly string CATEGORY_A_KEY = "key";

	private readonly string ITEM_E = "ITEM";

	private readonly string ITEM_A_KEY = "key";

	private readonly string ITEM_A_AUDIOTYPE = "audio_type";

	private readonly string ITEM_A_PREDOWNLOAD = "predownload";

	private readonly string ITEM_A_SKIPIFPLAYINGSAME = "skip_if_playingsame";

	private readonly string VOLUME_E = "VOLUME";

	private readonly string VOLUME_A_DEFAULT = "default";

	private readonly string VOLUME_A_RAND_ENABLE = "rand_enable";

	private readonly string VOLUME_A_RAND_MIN = "rand_min";

	private readonly string VOLUME_A_RAND_MAX = "rand_max";

	private readonly string PITCH_E = "PITCH";

	private readonly string PITCH_A_DEFAULT = "default";

	private readonly string PITCH_A_RAND_ENABLE = "rand_enable";

	private readonly string PITCH_A_RAND_MIN = "rand_min";

	private readonly string PITCH_A_RAND_MAX = "rand_max";

	private readonly string BUNDLE_E = "BUNDLE";

	private readonly string BUNDLE_A_KEY = "key";

	private readonly string BUNDLE_A_BUNDLENAME = "bundle_name";

	private readonly string BUNDLE_A_ASSETPATH = "asset_path";

	private readonly string VERSION = "1.0";

	private List<TsAudioContainer.Domain> _domains = new List<TsAudioContainer.Domain>();

	private HashSet<string> _predownloadingList = new HashSet<string>();

	private bool _isComplatedDownloadXML;

	private string _lastLoadResult = string.Empty;

	public string XML_FILE_NAME
	{
		get
		{
			if (!TsPlatform.IsMobile)
			{
				return "AudioContainer";
			}
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USAMAZON || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_GLOBALENGLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_GLOBALENGQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_USQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_USIOS)
			{
				return "AudioContainer_ENG";
			}
			if (NrGlobalReference.IsLiteVersion())
			{
				return "AudioContainer_LITE";
			}
			return "AudioContainer_AT2";
		}
	}

	public string XML_FILE_PATH
	{
		get
		{
			string text = Option.GetProtocolRootPath(Protocol.FILE);
			text = text.Substring("file:///".Length, text.Length - "file:///".Length);
			return string.Format("{0}{1}/{2}.xml", text, "GameData/XML", this.XML_FILE_NAME);
		}
	}

	public string XML_DOWNLOAD_PATH
	{
		get
		{
			string text = string.Empty;
			if (NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				if (TsPlatform.IsMobile && NrTSingleton<NrGlobalReference>.Instance.isLoadWWW)
				{
					text = text + CDefinePath.XMLBundlePath() + this.XML_FILE_NAME + "_mobile.assetbundle";
				}
				else
				{
					text = text + CDefinePath.XMLPath() + this.XML_FILE_NAME + ".xml";
				}
			}
			else
			{
				text = text + CDefinePath.XMLPath() + this.XML_FILE_NAME + ".xml";
			}
			return text;
		}
	}

	public int DomainCount
	{
		get
		{
			return this._domains.Count;
		}
	}

	public string[] DomainKeys
	{
		get
		{
			List<string> list = new List<string>();
			foreach (TsAudioContainer.Domain current in this._domains)
			{
				list.Add(current.Key);
			}
			return list.ToArray();
		}
	}

	public TsAudioContainer.Domain[] DomainArray
	{
		get
		{
			return this._domains.ToArray();
		}
	}

	public string[] CollectBundleDatas(TsAudioContainer.Item.CollectReturnType collectType)
	{
		List<string> list = new List<string>();
		TsAudioContainer.Domain[] domainArray = this.DomainArray;
		for (int i = 0; i < domainArray.Length; i++)
		{
			TsAudioContainer.Domain domain = domainArray[i];
			TsAudioContainer.Category[] categoryArray = domain.CategoryArray;
			for (int j = 0; j < categoryArray.Length; j++)
			{
				TsAudioContainer.Category category = categoryArray[j];
				TsAudioContainer.Item[] itemArray = category.ItemArray;
				for (int k = 0; k < itemArray.Length; k++)
				{
					TsAudioContainer.Item item = itemArray[k];
					string[] array = item.CollectBundleDatas(collectType);
					string[] array2 = array;
					for (int l = 0; l < array2.Length; l++)
					{
						string item2 = array2[l];
						list.Add(item2);
					}
				}
			}
		}
		return list.ToArray();
	}

	public void Log()
	{
		TsLog.Log("Domains.Count= " + this._domains.Count, new object[0]);
		foreach (TsAudioContainer.Domain current in this._domains)
		{
			TsAudioContainer.Category[] categoryArray = current.CategoryArray;
			TsLog.Log("Categories.Count= " + categoryArray.Length, new object[0]);
			TsAudioContainer.Category[] array = categoryArray;
			for (int i = 0; i < array.Length; i++)
			{
				TsAudioContainer.Category category = array[i];
				TsAudioContainer.Item[] itemArray = category.ItemArray;
				TsLog.Log("-NkCategory.Key= " + category.Key, new object[0]);
				TsLog.Log("-ITEM.Count= " + itemArray.Length, new object[0]);
				TsAudioContainer.Item[] array2 = itemArray;
				for (int j = 0; j < array2.Length; j++)
				{
					TsAudioContainer.Item item = array2[j];
					TsAudioContainer.Bundle[] bundleArray = item.BundleArray;
					TsLog.Log("--NkItem.Key= " + item.Key, new object[0]);
					TsLog.Log("--BUNDLE.Count= " + bundleArray.Length, new object[0]);
					TsAudioContainer.Bundle[] array3 = bundleArray;
					for (int k = 0; k < array3.Length; k++)
					{
						TsAudioContainer.Bundle bundle = array3[k];
						TsLog.Log("---NkBundle.Info= " + bundle.Info.ToString(), new object[0]);
					}
				}
			}
		}
	}

	public TsAudioContainer.Domain AddDomain(string domainKey)
	{
		TsAudioContainer.Domain domain = this.FindDomain(domainKey);
		if (domain == null)
		{
			domain = TsAudioContainer.Domain.Create(domainKey);
			if (domain != null)
			{
				this._domains.Add(domain);
			}
		}
		return domain;
	}

	public bool EditDomain(string currentKey, string newKey)
	{
		TsAudioContainer.Domain domain = this.FindDomain(currentKey);
		if (domain == null)
		{
			return false;
		}
		domain.Key = newKey;
		return true;
	}

	public void RemoveDomain(string domainKey)
	{
		TsAudioContainer.Domain domain = this.FindDomain(domainKey);
		if (domain != null)
		{
			this._domains.Remove(domain);
		}
	}

	public TsAudioContainer.Domain FindDomain(string domainKey)
	{
		if (string.IsNullOrEmpty(domainKey))
		{
			return null;
		}
		foreach (TsAudioContainer.Domain current in this._domains)
		{
			if (current.Key.Equals(domainKey, StringComparison.CurrentCultureIgnoreCase))
			{
				return current;
			}
		}
		return null;
	}

	public TsAudioContainer.Domain FindDomain(int index)
	{
		if (index < 0 || this._domains.Count <= index)
		{
			return null;
		}
		return this._domains[index];
	}

	public void ClearDomain()
	{
		this._domains.Clear();
	}

	public TsAudio.BaseData TryToMakeAudioBaseData(string domainKey, string categoryKey, string audioKey)
	{
		if (!this._isComplatedDownloadXML)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log("TsAudioContainer~! XML downloading is not yet finished~!", new object[0]);
			}
			return null;
		}
		TsAudioContainer.Domain domain = this.FindDomain(domainKey);
		if (domain == null)
		{
			TsLog.LogWarning("Failed~! Cannot Found DomainKey= " + domainKey, new object[0]);
			return null;
		}
		TsAudioContainer.Category category = domain.FindCategory(categoryKey);
		if (category == null)
		{
			TsLog.LogWarning("Failed~! Cannot Found CategoryKey= " + categoryKey + "   DomainKey= " + domainKey, new object[0]);
			return null;
		}
		TsAudioContainer.Item item = category.FindItem(audioKey);
		if (item == null)
		{
			TsLog.LogWarning(string.Concat(new string[]
			{
				"Failed~! Cannot Found AudioKey= ",
				audioKey,
				"   DomainKey= ",
				domainKey,
				"   CategoryKey= ",
				categoryKey
			}), new object[0]);
			return null;
		}
		if (item.BundleCount <= 0)
		{
			return null;
		}
		int bundleCount = item.BundleCount;
		int index = UnityEngine.Random.Range(0, bundleCount);
		TsAudioContainer.Bundle bundle = item.FindBundle(index);
		if (bundle == null)
		{
			TsLog.LogError("Failed~! Cannot Found Bundle... BundleCount= " + item.BundleCount, new object[0]);
			return null;
		}
		return TsAudio.BaseData.Create(item.AudioType, item.PitchRand, item.VolumeRand, item.SkipIfPlayingSame, bundle.Info);
	}

	private TsAudio.BaseData TryToMakeAudioBaseData(string domainKey, string categoryKey, string audioKey, string bundleKey)
	{
		if (!this._isComplatedDownloadXML)
		{
			TsLog.Log("not yet XML download~!", new object[0]);
			return null;
		}
		TsAudioContainer.Domain domain = this.FindDomain(domainKey);
		if (domain == null)
		{
			TsLog.LogWarning("Failed~! Cannot Found DomainKey= " + domainKey, new object[0]);
			return null;
		}
		TsAudioContainer.Category category = domain.FindCategory(categoryKey);
		if (category == null)
		{
			TsLog.LogWarning("Failed~! Cannot Found CategoryKey= " + categoryKey, new object[0]);
			return null;
		}
		TsAudioContainer.Item item = category.FindItem(audioKey);
		if (item == null)
		{
			TsLog.LogWarning("Failed~! Cannot Found AudioKey= " + audioKey, new object[0]);
			return null;
		}
		TsAudioContainer.Bundle bundle = item.FindBundle(bundleKey);
		if (bundle == null)
		{
			TsLog.LogWarning("Failed~! Cannot Found BundleKey= " + bundleKey, new object[0]);
			return null;
		}
		return TsAudio.BaseData.Create(item.AudioType, item.PitchRand, item.VolumeRand, item.SkipIfPlayingSame, bundle.Info);
	}

	public void Sort()
	{
		TsLog.Log("TsAudioContainer.Sort()", new object[0]);
		this._domains.Sort(new TsAudioContainer._SorterDomain());
		foreach (TsAudioContainer.Domain current in this._domains)
		{
			current.SortCategories();
			TsAudioContainer.Category[] categoryArray = current.CategoryArray;
			for (int i = 0; i < categoryArray.Length; i++)
			{
				TsAudioContainer.Category category = categoryArray[i];
				category.SortItems();
				TsAudioContainer.Item[] itemArray = category.ItemArray;
				for (int j = 0; j < itemArray.Length; j++)
				{
					TsAudioContainer.Item item = itemArray[j];
					item.SortBundles();
				}
			}
		}
	}

	public bool LoadXML()
	{
		this._isComplatedDownloadXML = false;
		bool result;
		if (Application.isPlaying && NrTSingleton<NrGlobalReference>.Instance.isLoadWWW)
		{
			result = this._LoadXML_FromServer();
		}
		else
		{
			result = this._LoadXML_FromLocal();
		}
		return result;
	}

	private bool _LoadXML_FromServer()
	{
		TsLog.Log("Start _LoadXML_FromServer Download AudioContainer~! = " + this.XML_DOWNLOAD_PATH, new object[0]);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(this.XML_DOWNLOAD_PATH, Option.defaultStackName);
		if (NrTSingleton<NrGlobalReference>.Instance.useCache)
		{
			wWWItem.SetItemType(ItemType.USER_ASSETB);
		}
		else
		{
			wWWItem.SetItemType(ItemType.USER_STRING);
		}
		wWWItem.SetCallback(new PostProcPerItem(this.OnCompleteDownloadXML), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		return true;
	}

	public void OnCompleteDownloadXML(IDownloadedItem wItem, object obj)
	{
		if (wItem.isCanceled)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			string xml = this._ParseXMLString(wItem);
			xmlDocument.LoadXml(xml);
		}
		catch (Exception ex)
		{
			TsLog.LogError("Failed~! TsAudioContainer.OnCompleteDownloadXML() WWW= " + wItem.assetPath + "   Exception~!= " + ex.ToString(), new object[0]);
			return;
		}
		if (!this._LoadXml(xmlDocument, wItem.assetName))
		{
			TsLog.LogError(this._lastLoadResult, new object[0]);
			return;
		}
	}

	private string _ParseXMLString(IDownloadedItem item)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		if (item.canAccessAssetBundle)
		{
			GameObject gameObject = item.mainAsset as GameObject;
			if (gameObject == null)
			{
				text2 = "downloadedWWW.assetBundle.mainAsset is not GameObject~!";
				goto IL_B8;
			}
			TsGameDataAdapter component = gameObject.GetComponent<TsGameDataAdapter>();
			if (component == null)
			{
				text2 = "cannot GetComponent() TsGameDataAdapter";
				goto IL_B8;
			}
			if (component.GameData.serializeGameDatas.Count <= 0)
			{
				text2 = "GameData.serializeGameDatas.Count <= 0~!!!!";
				goto IL_B8;
			}
			text = component.GameData.serializeGameDatas[0];
		}
		else if (item.canAccessString)
		{
			text = item.safeString;
		}
		if (string.IsNullOrEmpty(text))
		{
			text2 = "Empty GameData String~!!!!";
		}
		IL_B8:
		if (!string.IsNullOrEmpty(text2))
		{
			TsLog.LogError("Error~! reason= " + text2 + "   WWW_URL= " + item.url, new object[0]);
		}
		return text;
	}

	private bool _LoadXML_FromLocal()
	{
		if (NrTSingleton<NrGlobalReference>.Instance.isLoadWWW)
		{
			TsLog.Log("Start Download AudioContainer~! = " + this.XML_FILE_PATH, new object[0]);
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(this.XML_FILE_PATH);
			}
			catch (Exception ex)
			{
				TsLog.LogError("Failed~! TsAudioContainer._LoadXML_FromLocal()   Exception~!= " + ex.ToString(), new object[0]);
				bool result = false;
				return result;
			}
			if (!this._LoadXml(xmlDocument, this.XML_FILE_PATH))
			{
				TsLog.LogError(this._lastLoadResult, new object[0]);
				return false;
			}
			return true;
		}
		TsDataReader tsDataReader = new TsDataReader();
		tsDataReader.UseMD5 = true;
		string text = Path.Combine(NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceAreaInfo().szOriginalDataCDNPath, this.XML_FILE_PATH);
		string fileString = tsDataReader.GetFileString(this.XML_FILE_PATH);
		XmlDocument xmlDocument2 = new XmlDocument();
		xmlDocument2.LoadXml(fileString);
		if (!this._LoadXml(xmlDocument2, this.XML_FILE_PATH))
		{
			TsLog.LogError(this._lastLoadResult, new object[0]);
			return false;
		}
		return true;
	}

	private bool _LoadXml(XmlDocument doc, string path)
	{
		bool result = false;
		this._lastLoadResult = "Failed~! TsAudioContainer._LoadXml() reason= ";
		this.ClearDomain();
		if (doc == null)
		{
			this._lastLoadResult += "doc is Null~!!";
		}
		else if (doc.DocumentElement == null)
		{
			this._lastLoadResult += "Cannot Load XMLFile~!";
		}
		else if (!doc.DocumentElement.Name.Equals(this.ROOT_E))
		{
			this._lastLoadResult += "Not Equals Root ElementName->";
			this._lastLoadResult += this.ROOT_E;
		}
		else
		{
			string attribute = doc.DocumentElement.GetAttribute(this.ROOT_A_VERSION);
			if (string.IsNullOrEmpty(attribute))
			{
				this._lastLoadResult += "version String is Empty~!";
			}
			else if (!this._LoadXML_VolumeScale(doc.DocumentElement.ChildNodes))
			{
				this._lastLoadResult += "Falied~! _LoadXML_VolumeScale()";
			}
			else if (!this._LoadXML_Domains(doc.DocumentElement.ChildNodes))
			{
				this._lastLoadResult += "Failed~! _LoadXML_Domains()";
			}
			else
			{
				this._lastLoadResult = string.Format("Success~! TsAudioContainer.LoadXML( {0} )~!", path);
				result = true;
			}
		}
		if (Application.isPlaying)
		{
			this.Predownloading();
		}
		this._isComplatedDownloadXML = true;
		return result;
	}

	private bool _LoadXML_VolumeScale(XmlNodeList elementList)
	{
		bool[] array = new bool[10];
		float[] array2 = new float[]
		{
			0.7f,
			0.7f,
			0.6f,
			0.7f,
			0.45f,
			0.7f,
			0.6f,
			0.6f,
			0.7f,
			1f
		};
		foreach (XmlNode xmlNode in elementList)
		{
			XmlElement xmlElement = xmlNode as XmlElement;
			if (xmlElement != null)
			{
				if (xmlElement.Name.Equals(this.VOLUMESCALE_E))
				{
					foreach (XmlNode xmlNode2 in xmlElement.ChildNodes)
					{
						XmlElement xmlElement2 = xmlNode2 as XmlElement;
						if (xmlElement2 == null)
						{
							TsLog.Log("typeE == null", new object[0]);
						}
						else if (!xmlElement2.Name.Equals(this.VOLUMESCALE_E_TYPE))
						{
							TsLog.Log("false == typeE.Name.Equals( VOLUMESCALE_E_TYPE )", new object[0]);
						}
						else
						{
							string attribute = xmlElement2.GetAttribute(this.VOLUMESCALE_A_KEY);
							if (string.IsNullOrEmpty(attribute))
							{
								TsLog.Log("string.IsNullOrEmpty( key ) == true", new object[0]);
							}
							else
							{
								string attribute2 = xmlElement2.GetAttribute(this.VOLUMESCALE_A_VALUE);
								if (string.IsNullOrEmpty(attribute2))
								{
									TsLog.LogWarning("string.IsNullOrEmpty( value ) == true", new object[0]);
								}
								else
								{
									EAudioType eAudioType = (EAudioType)((int)Enum.Parse(typeof(EAudioType), attribute, true));
									float num;
									if (!float.TryParse(attribute2, out num))
									{
										TsLog.LogWarning("false == System.Single.TryParse( value, out volumeScale )", new object[0]);
									}
									else
									{
										array[(int)eAudioType] = true;
										array2[(int)eAudioType] = num;
									}
								}
							}
						}
					}
				}
			}
		}
		EAudioType[] array3 = (EAudioType[])Enum.GetValues(typeof(EAudioType));
		EAudioType[] array4 = array3;
		for (int i = 0; i < array4.Length; i++)
		{
			EAudioType eAudioType2 = array4[i];
			if (eAudioType2 < EAudioType.TOTAL)
			{
				if (!array[(int)eAudioType2])
				{
					TsLog.LogWarning("_LoadXML_VolumeScale() type[{0}] == false", new object[]
					{
						eAudioType2
					});
				}
			}
		}
		TsAudio.ApplyVolumeScalings(array2);
		return true;
	}

	private bool _LoadXML_Domains(XmlNodeList elementList)
	{
		foreach (XmlNode xmlNode in elementList)
		{
			XmlElement xmlElement = xmlNode as XmlElement;
			if (xmlElement != null)
			{
				if (xmlElement.Name.Equals(this.DOMAIN_E))
				{
					string attribute = xmlElement.GetAttribute(this.DOMAIN_A_KEY);
					if (!string.IsNullOrEmpty(attribute))
					{
						TsAudioContainer.Domain domain = this.AddDomain(attribute);
						if (domain != null)
						{
							if (!this._LoadXML_Categories(xmlElement.ChildNodes, domain))
							{
								return false;
							}
						}
					}
				}
			}
		}
		return true;
	}

	private bool _LoadXML_Categories(XmlNodeList categoryEList, TsAudioContainer.Domain domain)
	{
		if (domain == null)
		{
			return false;
		}
		foreach (XmlNode xmlNode in categoryEList)
		{
			XmlElement xmlElement = xmlNode as XmlElement;
			if (xmlElement != null)
			{
				if (xmlElement.Name.Equals(this.CATEGORY_E))
				{
					string attribute = xmlElement.GetAttribute(this.CATEGORY_A_KEY);
					if (!string.IsNullOrEmpty(attribute))
					{
						TsAudioContainer.Category category = domain.AddCategory(attribute);
						if (category != null)
						{
							if (!this._LoadXML_Items(xmlElement.ChildNodes, category))
							{
								return false;
							}
						}
					}
				}
			}
		}
		return true;
	}

	private bool _LoadXML_Items(XmlNodeList itemEList, TsAudioContainer.Category category)
	{
		if (category == null)
		{
			return false;
		}
		foreach (XmlNode xmlNode in itemEList)
		{
			XmlElement xmlElement = xmlNode as XmlElement;
			if (xmlElement != null)
			{
				if (xmlElement.Name.Equals(this.ITEM_E))
				{
					string attribute = xmlElement.GetAttribute(this.ITEM_A_KEY);
					if (!string.IsNullOrEmpty(attribute))
					{
						TsAudioContainer.Item item = category.AddItem(attribute);
						if (item != null)
						{
							string attribute2 = xmlElement.GetAttribute(this.ITEM_A_AUDIOTYPE);
							if (!string.IsNullOrEmpty(attribute2))
							{
								try
								{
									item.AudioType = (EAudioType)((int)Enum.Parse(typeof(EAudioType), attribute2));
								}
								catch (Exception ex)
								{
									TsLog.LogError(ex.ToString(), new object[0]);
								}
							}
							bool isPredownload = false;
							if (!TsPlatform.IsMobile)
							{
								string attribute3 = xmlElement.GetAttribute(this.ITEM_A_PREDOWNLOAD);
								if (!string.IsNullOrEmpty(attribute3) && !bool.TryParse(attribute3, out isPredownload))
								{
									isPredownload = false;
								}
							}
							item.IsPredownload = isPredownload;
							bool skipIfPlayingSame = false;
							string attribute4 = xmlElement.GetAttribute(this.ITEM_A_SKIPIFPLAYINGSAME);
							if (!string.IsNullOrEmpty(attribute4) && !bool.TryParse(attribute4, out skipIfPlayingSame))
							{
								skipIfPlayingSame = false;
							}
							item.SkipIfPlayingSame = skipIfPlayingSame;
							if (!this._LoadXML_BundlesAndEtc(xmlElement.ChildNodes, item))
							{
								return false;
							}
						}
					}
				}
			}
		}
		return true;
	}

	private bool _LoadXML_BundlesAndEtc(XmlNodeList childEList, TsAudioContainer.Item item)
	{
		if (item == null)
		{
			return false;
		}
		foreach (XmlNode xmlNode in childEList)
		{
			XmlElement xmlElement = xmlNode as XmlElement;
			if (xmlElement != null)
			{
				if (xmlElement.Name.Equals(this.VOLUME_E))
				{
					try
					{
						item.VolumeRand.volume = float.Parse(xmlElement.GetAttribute(this.VOLUME_A_DEFAULT), NumberStyles.Float);
						item.VolumeRand.enable = bool.Parse(xmlElement.GetAttribute(this.VOLUME_A_RAND_ENABLE));
						item.VolumeRand.min = float.Parse(xmlElement.GetAttribute(this.VOLUME_A_RAND_MIN), NumberStyles.Float);
						item.VolumeRand.max = float.Parse(xmlElement.GetAttribute(this.VOLUME_A_RAND_MAX), NumberStyles.Float);
					}
					catch (Exception ex)
					{
						TsLog.Log("Exception~! = " + ex.ToString(), new object[0]);
					}
				}
				else if (xmlElement.Name.Equals(this.PITCH_E))
				{
					try
					{
						item.PitchRand.pitch = float.Parse(xmlElement.GetAttribute(this.PITCH_A_DEFAULT), NumberStyles.Float);
						item.PitchRand.enable = bool.Parse(xmlElement.GetAttribute(this.PITCH_A_RAND_ENABLE));
						item.PitchRand.min = float.Parse(xmlElement.GetAttribute(this.PITCH_A_RAND_MIN), NumberStyles.Float);
						item.PitchRand.max = float.Parse(xmlElement.GetAttribute(this.PITCH_A_RAND_MAX), NumberStyles.Float);
					}
					catch (Exception ex2)
					{
						TsLog.Log("Exception~! = " + ex2.ToString(), new object[0]);
					}
				}
				else if (xmlElement.Name.Equals(this.BUNDLE_E))
				{
					string attribute = xmlElement.GetAttribute(this.BUNDLE_A_KEY);
					string attribute2 = xmlElement.GetAttribute(this.BUNDLE_A_BUNDLENAME);
					string attribute3 = xmlElement.GetAttribute(this.BUNDLE_A_ASSETPATH);
					if (!string.IsNullOrEmpty(attribute) && !string.IsNullOrEmpty(attribute2) && !string.IsNullOrEmpty(attribute3))
					{
						TsAudioContainer.Bundle bundle = item.AddBundle(attribute, attribute2, attribute3);
						if (bundle != null)
						{
							if (item.IsPredownload)
							{
								this._predownloadingList.Add(bundle.BundleName);
							}
						}
					}
				}
			}
		}
		return true;
	}

	public void SaveXML()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.CreateXmlDeclaration("1.0", Encoding.UTF8.EncodingName, "yes");
		XmlElement xmlElement = xmlDocument.CreateElement(this.ROOT_E);
		xmlElement.SetAttribute(this.ROOT_A_VERSION, this.VERSION);
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement(this.VOLUMESCALE_E);
		xmlElement.AppendChild(xmlElement2);
		EAudioType[] array = (EAudioType[])Enum.GetValues(typeof(EAudioType));
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				XmlElement xmlElement3 = xmlDocument.CreateElement(this.VOLUMESCALE_E_TYPE);
				float volumeScaling = TsAudio.GetVolumeScaling(eAudioType);
				xmlElement3.SetAttribute(this.VOLUMESCALE_A_KEY, eAudioType.ToString());
				xmlElement3.SetAttribute(this.VOLUMESCALE_A_VALUE, volumeScaling.ToString("#0.00"));
				xmlElement2.AppendChild(xmlElement3);
			}
		}
		TsAudioContainer.Domain[] domainArray = this.DomainArray;
		for (int j = 0; j < domainArray.Length; j++)
		{
			TsAudioContainer.Domain domain = domainArray[j];
			XmlElement xmlElement4 = xmlDocument.CreateElement(this.DOMAIN_E);
			xmlElement4.SetAttribute(this.DOMAIN_A_KEY, domain.Key);
			xmlElement.AppendChild(xmlElement4);
			TsAudioContainer.Category[] categoryArray = domain.CategoryArray;
			for (int k = 0; k < categoryArray.Length; k++)
			{
				TsAudioContainer.Category category = categoryArray[k];
				XmlElement xmlElement5 = xmlDocument.CreateElement(this.CATEGORY_E);
				xmlElement5.SetAttribute(this.CATEGORY_A_KEY, category.Key);
				xmlElement4.AppendChild(xmlElement5);
				TsAudioContainer.Item[] itemArray = category.ItemArray;
				for (int l = 0; l < itemArray.Length; l++)
				{
					TsAudioContainer.Item item = itemArray[l];
					XmlElement xmlElement6 = xmlDocument.CreateElement(this.ITEM_E);
					xmlElement6.SetAttribute(this.ITEM_A_KEY, item.Key);
					xmlElement6.SetAttribute(this.ITEM_A_AUDIOTYPE, item.AudioType.ToString());
					xmlElement6.SetAttribute(this.ITEM_A_PREDOWNLOAD, item.IsPredownload.ToString());
					xmlElement6.SetAttribute(this.ITEM_A_SKIPIFPLAYINGSAME, item.SkipIfPlayingSame.ToString());
					xmlElement5.AppendChild(xmlElement6);
					XmlElement xmlElement7 = xmlDocument.CreateElement(this.VOLUME_E);
					xmlElement7.SetAttribute(this.VOLUME_A_DEFAULT, item.VolumeRand.volume.ToString("#0.00"));
					xmlElement7.SetAttribute(this.VOLUME_A_RAND_ENABLE, item.VolumeRand.enable.ToString());
					xmlElement7.SetAttribute(this.VOLUME_A_RAND_MIN, item.VolumeRand.min.ToString("#0.00"));
					xmlElement7.SetAttribute(this.VOLUME_A_RAND_MAX, item.VolumeRand.max.ToString("#0.00"));
					xmlElement6.AppendChild(xmlElement7);
					XmlElement xmlElement8 = xmlDocument.CreateElement(this.PITCH_E);
					xmlElement8.SetAttribute(this.PITCH_A_DEFAULT, item.PitchRand.pitch.ToString("#0.00"));
					xmlElement8.SetAttribute(this.PITCH_A_RAND_ENABLE, item.PitchRand.enable.ToString());
					xmlElement8.SetAttribute(this.PITCH_A_RAND_MIN, item.PitchRand.min.ToString("#0.00"));
					xmlElement8.SetAttribute(this.PITCH_A_RAND_MAX, item.PitchRand.max.ToString("#0.00"));
					xmlElement6.AppendChild(xmlElement8);
					TsAudioContainer.Bundle[] bundleArray = item.BundleArray;
					for (int m = 0; m < bundleArray.Length; m++)
					{
						TsAudioContainer.Bundle bundle = bundleArray[m];
						XmlElement xmlElement9 = xmlDocument.CreateElement(this.BUNDLE_E);
						xmlElement9.SetAttribute(this.BUNDLE_A_KEY, bundle.Key);
						xmlElement9.SetAttribute(this.BUNDLE_A_BUNDLENAME, bundle.BundleName);
						xmlElement9.SetAttribute(this.BUNDLE_A_ASSETPATH, bundle.AssetPath);
						xmlElement6.AppendChild(xmlElement9);
					}
				}
			}
		}
		string message = string.Format("Success~! Save AudioContainer XML= {0}", this.XML_FILE_PATH);
		try
		{
			xmlDocument.Save(this.XML_FILE_PATH);
		}
		catch (Exception ex)
		{
			message = "Failed~! Save AudioContainer XML= " + this.XML_FILE_PATH + "   Exception= " + ex.ToString();
		}
		TsLog.Log(message, new object[0]);
	}

	public void RequestAudioClip(string domainKey, string categoryKey, string audioKey, PostProcPerItem onEvent)
	{
		TsAudio.BaseData baseData = this.TryToMakeAudioBaseData(domainKey, categoryKey, audioKey);
		if (baseData == null)
		{
			return;
		}
		if (domainKey == "UI_SFX")
		{
			baseData.Tag = TsAudioManager.UI_SOUND;
		}
		this._RequestDownload(baseData, onEvent);
	}

	public void RequestAudioClip(string domainKey, string categoryKey, string audioKey, PostProcPerItem onEvent, string tag)
	{
		TsAudio.BaseData baseData = this.TryToMakeAudioBaseData(domainKey, categoryKey, audioKey);
		if (baseData == null)
		{
			return;
		}
		baseData.Tag = tag;
		this._RequestDownload(baseData, onEvent);
	}

	public void RequestAudioClip(string domainKey, string categoryKey, string audioKey, PostProcPerItem onEvent, string tag, bool loop)
	{
		TsAudio.BaseData baseData = this.TryToMakeAudioBaseData(domainKey, categoryKey, audioKey);
		if (baseData == null)
		{
			return;
		}
		baseData.Tag = tag;
		baseData.Loop = loop;
		this._RequestDownload(baseData, onEvent);
	}

	public void RequestAudioClip(string domainKey, string categoryKey, string audioKey, bool isDontDestroyOnLoad, PostProcPerItem onEvent)
	{
		TsAudio.BaseData baseData = this.TryToMakeAudioBaseData(domainKey, categoryKey, audioKey);
		if (baseData == null)
		{
			return;
		}
		if (domainKey == "UI_SFX")
		{
			baseData.Tag = TsAudioManager.UI_SOUND;
		}
		baseData.IsDontDestroyOnLoad = isDontDestroyOnLoad;
		this._RequestDownload(baseData, onEvent);
	}

	public void RequestAudioClip(string domainKey, string categoryKey, string audioKey, string bundleKey, PostProcPerItem onEvent)
	{
		TsAudio.BaseData baseData = this.TryToMakeAudioBaseData(domainKey, categoryKey, audioKey, bundleKey);
		if (baseData == null)
		{
			return;
		}
		if (domainKey == "UI_SFX")
		{
			baseData.Tag = TsAudioManager.UI_SOUND;
		}
		this._RequestDownload(baseData, onEvent);
	}

	public void _RequestDownload(TsAudio.BaseData baseData, PostProcPerItem onEvent)
	{
		if (baseData == null)
		{
			return;
		}
		if (baseData.BundleInfoCount <= 0)
		{
			return;
		}
		if (onEvent == null)
		{
			return;
		}
		TsAudioBundleInfo tsAudioBundleInfo = baseData.DecideBundleInfo();
		if (tsAudioBundleInfo == null)
		{
			return;
		}
		bool flag = tsAudioBundleInfo._RequestDownload(new TsAudio.RequestData(baseData.CurrentBundleInfoIndex, baseData), onEvent);
		if (flag)
		{
			baseData.RequestedIndexList.Add(baseData.CurrentBundleInfoIndex);
		}
	}

	public bool RequestDownload_AllAudioBundles(TsAudio.BaseData baseData, PostProcPerItem onEvent_Downloaded)
	{
		if (baseData == null)
		{
			return false;
		}
		if (baseData.BundleInfoCount <= 0)
		{
			return false;
		}
		TsAudioBundleInfo[] bundleInfoArray = baseData.BundleInfoArray;
		for (int i = 0; i < bundleInfoArray.Length; i++)
		{
			TsAudioBundleInfo tsAudioBundleInfo = bundleInfoArray[i];
			bool flag = tsAudioBundleInfo._RequestDownload(new TsAudio.RequestData(i, baseData), onEvent_Downloaded);
			if (flag)
			{
				baseData.RequestedIndexList.Add(i);
			}
		}
		return true;
	}

	public bool RequestDownload_SelectiveAudioBundles(TsAudio.BaseData baseData, int[] indexes, PostProcPerItem onEvent_Downloaded)
	{
		if (baseData == null)
		{
			TsLog.LogError("Failed~! RequestDownload() (baseData is NULL)", new object[0]);
			return false;
		}
		if (baseData.BundleInfoCount <= 0)
		{
			TsLog.LogError("Failed~! RequestDownload() (baseData.BundleInfoCount Less then 0)", new object[0]);
			return false;
		}
		if (indexes == null || indexes.Length <= 0)
		{
			TsLog.LogError("Failed~! RequestDownload() (Info is NULL)", new object[0]);
			return false;
		}
		bool result = true;
		TsAudioBundleInfo[] bundleInfoArray = baseData.BundleInfoArray;
		for (int i = 0; i < indexes.Length; i++)
		{
			int num = indexes[i];
			try
			{
				bool flag = bundleInfoArray[num]._RequestDownload(new TsAudio.RequestData(num, baseData), onEvent_Downloaded);
				if (flag)
				{
					baseData.RequestedIndexList.Add(num);
				}
			}
			catch (Exception ex)
			{
				string text = (bundleInfoArray.Length < 1) ? "No Bundles" : bundleInfoArray[0].AssetBundleName;
				if (TsPlatform.IsWeb)
				{
					TsLog.Assert(false, "doesn't match Index~! check it~! IDX[{0} / {1}] 0ABName[{2}]  exception={3}", new object[]
					{
						num,
						bundleInfoArray.Length,
						text,
						ex.ToString()
					});
				}
				result = false;
			}
		}
		return result;
	}

	public void Predownloading()
	{
		if (this._predownloadingList.Count <= 0)
		{
			return;
		}
		TsLog.Log("TsAudioContainer Predownloading cnt = {0}", new object[]
		{
			this._predownloadingList.Count
		});
		List<string> list = new List<string>();
		foreach (string current in this._predownloadingList)
		{
			list.Add(TsAudioBundleInfo.GetDownloadPath(current));
		}
		Helper.PreDownloadRequest(list, new PostProcPerList(TsAudio.OnDownloaded_Predownload), null);
		this._predownloadingList = new HashSet<string>();
	}

	public void RemoveBGM(string key)
	{
		TsAudioAdapterBGM[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapterBGM)) as TsAudioAdapterBGM[];
		TsAudioAdapterBGM[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TsAudioAdapterBGM tsAudioAdapterBGM = array2[i];
			if (tsAudioAdapterBGM != null)
			{
				string text = tsAudioAdapterBGM.gameObject.name.ToLower();
				if (string.IsNullOrEmpty(key) || text.Contains(key.ToLower()))
				{
					TsLog.LogWarning("AudioBGM_Remove == {0}", new object[]
					{
						tsAudioAdapterBGM.gameObject.name
					});
					UnityEngine.Object.Destroy(tsAudioAdapterBGM.gameObject);
				}
			}
		}
	}

	public void RemoveAmbient(string key)
	{
		TsAudioAdapterAmbient[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapterAmbient)) as TsAudioAdapterAmbient[];
		TsAudioAdapterAmbient[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TsAudioAdapterAmbient tsAudioAdapterAmbient = array2[i];
			if (tsAudioAdapterAmbient != null)
			{
				string text = tsAudioAdapterAmbient.gameObject.name.ToLower();
				if (string.IsNullOrEmpty(key) || text.Contains(key.ToLower()))
				{
					TsLog.LogWarning("Audioambient_Remove == {0}", new object[]
					{
						tsAudioAdapterAmbient.gameObject.name
					});
					UnityEngine.Object.Destroy(tsAudioAdapterAmbient.gameObject);
				}
			}
		}
	}

	public void RemoveUI(string key)
	{
		TsAudioAdapterUI[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapterUI)) as TsAudioAdapterUI[];
		TsAudioAdapterUI[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TsAudioAdapterUI tsAudioAdapterUI = array2[i];
			if (tsAudioAdapterUI != null)
			{
				string text = tsAudioAdapterUI.gameObject.name.ToLower();
				if (string.IsNullOrEmpty(key) || text.Contains(key.ToLower()))
				{
					TsLog.LogWarning("Audioaui_Remove == {0}", new object[]
					{
						tsAudioAdapterUI.gameObject.name
					});
					UnityEngine.Object.Destroy(tsAudioAdapterUI.gameObject);
				}
			}
		}
	}

	public void OffBGM(bool off)
	{
		TsAudioAdapterBGM[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapterBGM)) as TsAudioAdapterBGM[];
		TsAudioAdapterBGM[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TsAudioAdapterBGM tsAudioAdapterBGM = array2[i];
			if (tsAudioAdapterBGM != null)
			{
				tsAudioAdapterBGM.gameObject.audio.mute = off;
			}
		}
	}

	public void OffAmbient(bool off)
	{
		TsAudioAdapterAmbient[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapterAmbient)) as TsAudioAdapterAmbient[];
		TsAudioAdapterAmbient[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TsAudioAdapterAmbient tsAudioAdapterAmbient = array2[i];
			if (tsAudioAdapterAmbient != null)
			{
				tsAudioAdapterAmbient.gameObject.audio.mute = off;
			}
		}
	}
}
