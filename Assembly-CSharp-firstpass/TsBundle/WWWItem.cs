using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TsPatch;
using UnityEngine;

namespace TsBundle
{
	public class WWWItem : IDisposable, IDownloadedItem, ISelfRemover
	{
		private enum StateWI
		{
			CREATED,
			REQUESTED,
			SUCCESS,
			ERROR,
			SERVER_ERR,
			CANCEL,
			UNLOADED,
			DESTROIED
		}

		private class CbParamObj
		{
			public PostProcPerItem callbackDelegate
			{
				get;
				set;
			}

			public object callbackParam
			{
				get;
				set;
			}

			public CbParamObj(PostProcPerItem callbackDelegate, object callbackParam)
			{
				this.callbackDelegate = callbackDelegate;
				this.callbackParam = callbackParam;
			}
		}

		private static class PageNotFoundParsing
		{
			private static bool m_ignore;

			public static bool isIgnore
			{
				get
				{
					return WWWItem.PageNotFoundParsing.m_ignore;
				}
			}

			static PageNotFoundParsing()
			{
				if (TsPlatform.IsIPhone)
				{
					WWWItem.PageNotFoundParsing.m_ignore = false;
					return;
				}
				string[] array = Application.unityVersion.Split(new char[]
				{
					'.'
				});
				if (array.Length < 2)
				{
					WWWItem.PageNotFoundParsing.m_ignore = false;
				}
				else
				{
					int num = int.Parse(array[0]);
					int num2 = int.Parse(array[1]);
					WWWItem.PageNotFoundParsing.m_ignore = (num >= 3 && num2 >= 5);
				}
			}
		}

		public delegate void OnIncreaseSizeOfDownloadHandler(int increaseSize);

		public static ErrorCallback _errorCallback;

		private WWW m_www;

		private UnityEngine.Object _mainAsset;

		public string RequestCallStack = string.Empty;

		private bool m_markerUnloadReserved;

		private bool m_unloadImmediate;

		private bool m_cacheChecked;

		private Protocol m_protocol = Protocol.HTTP;

		private string m_assetPath;

		private PatchFileInfo m_kItem;

		private WWWProgress m_wwwProgress;

		private string anotherURL = string.Empty;

		public static event WWWItem.OnIncreaseSizeOfDownloadHandler OnIncreaseSizeOfDownload
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				WWWItem.OnIncreaseSizeOfDownload = (WWWItem.OnIncreaseSizeOfDownloadHandler)Delegate.Combine(WWWItem.OnIncreaseSizeOfDownload, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				WWWItem.OnIncreaseSizeOfDownload = (WWWItem.OnIncreaseSizeOfDownloadHandler)Delegate.Remove(WWWItem.OnIncreaseSizeOfDownload, value);
			}
		}

		string IDownloadedItem.url
		{
			get
			{
				return this.m_www.url;
			}
		}

		public AssetBundle safeBundle
		{
			get;
			private set;
		}

		public string safeString
		{
			get;
			private set;
		}

		public byte[] safeBytes
		{
			get;
			private set;
		}

		public AudioClip safeAudioClip
		{
			get;
			private set;
		}

		public int safeSize
		{
			get;
			private set;
		}

		public UnityEngine.Object[] subBundles
		{
			get;
			private set;
		}

		public byte[] rawBytes
		{
			get;
			private set;
		}

		public UnityEngine.Object mainAsset
		{
			get
			{
				return this._mainAsset;
			}
			private set
			{
				this._mainAsset = value;
			}
		}

		public bool canAccessAssetBundle
		{
			get
			{
				return this.wiState == WWWItem.StateWI.SUCCESS && null != this.safeBundle;
			}
		}

		public bool canAccessString
		{
			get
			{
				return this.wiState == WWWItem.StateWI.SUCCESS && null != this.safeString;
			}
		}

		public bool canAccessBytes
		{
			get
			{
				return this.wiState == WWWItem.StateWI.SUCCESS && null != this.safeBytes;
			}
		}

		public bool canAccessAudioClip
		{
			get
			{
				return this.wiState == WWWItem.StateWI.SUCCESS && null != this.safeAudioClip;
			}
		}

		public bool isCompleteAsyncOp
		{
			get
			{
				return this.wiState >= WWWItem.StateWI.SUCCESS;
			}
		}

		internal bool inUndefinedStack
		{
			get
			{
				return this.itemType == ItemType.UNDEFINED && this.stackName == Option.undefinedStackName;
			}
		}

		internal bool isFirstCreation
		{
			get
			{
				return !this.isCached && this.justCreated && this.refCnt == 1;
			}
		}

		public string stackName
		{
			get;
			private set;
		}

		public int version
		{
			get
			{
				if (this.m_kItem == null)
				{
					return -1;
				}
				return this.m_kItem.nVersion;
			}
		}

		public bool UseCustomCache
		{
			get
			{
				return this.m_kItem != null && this.m_kItem.bUseCustomCache;
			}
		}

		public int filesize
		{
			get
			{
				if (this.m_kItem == null)
				{
					return 0;
				}
				return this.m_kItem.nFileSize;
			}
		}

		private WWWItem.StateWI wiState
		{
			get;
			set;
		}

		public string stateString
		{
			get
			{
				return this.wiState.ToString();
			}
		}

		private int retryCnt
		{
			get;
			set;
		}

		private bool loadAll
		{
			get;
			set;
		}

		private bool skipLog
		{
			get;
			set;
		}

		public bool isDone
		{
			get
			{
				return this.m_www != null && this.m_www.isDone;
			}
		}

		public float progress
		{
			get
			{
				return (this.m_www == null) ? 1f : this.m_www.progress;
			}
		}

		public bool isCreated
		{
			get
			{
				return this.m_www != null;
			}
		}

		public string errorString
		{
			get
			{
				return (this.m_www == null || this.m_www.error == null) ? string.Empty : this.m_www.error;
			}
		}

		internal bool DebugUnloadReserved
		{
			get
			{
				return this.m_markerUnloadReserved;
			}
			set
			{
				this.m_markerUnloadReserved = value;
			}
		}

		public bool isCacheHit
		{
			get;
			private set;
		}

		public bool useLoadFromCacheOrDownload
		{
			get;
			private set;
		}

		public bool unloadImmediate
		{
			get
			{
				return this.m_unloadImmediate;
			}
			set
			{
				this.m_unloadImmediate = value;
			}
		}

		private bool isUnityAsset
		{
			get
			{
				return this.assetPath.Contains(Option.extAsset) || this.assetPath.Contains(Option.extScene);
			}
		}

		public bool isCached
		{
			get
			{
				if (!this.m_cacheChecked)
				{
					this._CheckCaching();
				}
				return this.isCacheHit;
			}
		}

		public int refCnt
		{
			get;
			set;
		}

		public bool retryRequested
		{
			get;
			set;
		}

		public bool justCreated
		{
			get
			{
				return this.wiState == WWWItem.StateWI.CREATED;
			}
		}

		public bool justRequest
		{
			get
			{
				return this.wiState == WWWItem.StateWI.REQUESTED;
			}
		}

		public bool isCanceled
		{
			get
			{
				return this.wiState == WWWItem.StateWI.CANCEL;
			}
		}

		public bool isUnloaded
		{
			get
			{
				return this.wiState == WWWItem.StateWI.UNLOADED || this.wiState == WWWItem.StateWI.DESTROIED;
			}
		}

		public bool isSuccess
		{
			get
			{
				return this.wiState == WWWItem.StateWI.SUCCESS && this.itemType != ItemType.UNDEFINED && (null != this.safeBundle || this.safeString != null || this.safeBytes != null || null != this.safeAudioClip);
			}
		}

		public bool isFileNotFound
		{
			get;
			private set;
		}

		public string assetPath
		{
			get
			{
				return (!string.IsNullOrEmpty(this.anotherURL)) ? this.anotherURL : this.m_assetPath;
			}
			private set
			{
				this.m_assetPath = value;
			}
		}

		public ItemType itemType
		{
			get;
			private set;
		}

		public string assetName
		{
			get;
			private set;
		}

		public string strParam
		{
			get;
			private set;
		}

		public int indexParam
		{
			get;
			private set;
		}

		private List<WWWItem.CbParamObj> callbackList
		{
			get;
			set;
		}

		public WWWItem() : this(string.Empty)
		{
		}

		public WWWItem(string stackName)
		{
			this.wiState = WWWItem.StateWI.CREATED;
			this.refCnt = 0;
			this.retryCnt = 0;
			this.retryRequested = false;
			this.loadAll = false;
			this.itemType = ItemType.USER_ASSETB;
			this.safeSize = 0;
			this.stackName = stackName;
		}

		void ISelfRemover.UnloadBundle(bool clearMemory)
		{
			if (this.refCnt > 0 && this.refCnt-- == 1)
			{
				this.UnloadSafeBundle(false);
				this.Dispose();
			}
		}

		public AssetBundle GetSafeBundle()
		{
			return this.safeBundle;
		}

		public void SafeBundleUnload(bool unloadAllLoadedObject)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.safeBundleUnload && this.m_unloadImmediate && this.safeBundle != null)
			{
				this.safeBundle.Unload(unloadAllLoadedObject);
				this.m_unloadImmediate = false;
				this.m_www.Dispose();
				this.safeBundle = null;
			}
		}

		internal void UnloadSafeBundle(bool clearMemory)
		{
			if (this.safeBundle != null)
			{
				if (Option.EnableTrace)
				{
					TsLog.Log("[TsBundle] unload www (UnloadAllLoaedObject={0}, AssetPath=\"{1}\", Stack=\"{2}\", Type={3}, UnloadReserved={4}, RefCount={5})", new object[]
					{
						clearMemory,
						this.assetPath,
						this.stackName,
						this.itemType,
						this.DebugUnloadReserved,
						this.refCnt
					});
				}
				this.safeBundle.Unload(clearMemory);
				this.safeBundle = null;
			}
			this.wiState = WWWItem.StateWI.UNLOADED;
		}

		public void SetProtocol(Protocol protocol)
		{
			this.m_protocol = protocol;
		}

		private void _CheckCaching()
		{
			this.m_cacheChecked = true;
			this.isCacheHit = false;
			this.useLoadFromCacheOrDownload = false;
			if (this.m_protocol == Protocol.FILE)
			{
				return;
			}
			if (Option.localWWW)
			{
				this.m_protocol = Protocol.FILE;
				return;
			}
			if (!string.IsNullOrEmpty(this.anotherURL))
			{
				return;
			}
			if (!Option.useCache)
			{
				return;
			}
			if (this.m_kItem == null)
			{
				return;
			}
			if (!this.m_kItem.bUseCustomCache && !this.isUnityAsset)
			{
				return;
			}
			this.useLoadFromCacheOrDownload = true;
			string url = Option.GetProtocolRootPath(this.m_protocol) + this.assetPath;
			if (TsCaching.IsVersionCached(url, this.m_kItem.nVersion, this.m_kItem.bUseCustomCache))
			{
				this.isCacheHit = true;
			}
		}

		public void SetAnotherUrl(string url)
		{
			this.anotherURL = url;
		}

		public void SetAssetPath(string aPath)
		{
			this.assetPath = aPath.ToLower();
			string resFileName = string.Empty;
			if (this.assetPath[0] == '/')
			{
				resFileName = this.assetPath;
			}
			else
			{
				resFileName = "/" + this.assetPath;
			}
			this.m_kItem = PatchFinalList.Instance.GetPatchFileInfo(resFileName);
			if (this.m_kItem != null)
			{
				this.safeSize = this.m_kItem.nFileSize;
			}
		}

		public void SetCallback(PostProcPerItem callbackDelegate, object callbackParam)
		{
			if (callbackDelegate != null)
			{
				if (this.callbackList == null)
				{
					this.callbackList = new List<WWWItem.CbParamObj>();
				}
				this.callbackList.Add(new WWWItem.CbParamObj(callbackDelegate, callbackParam));
			}
		}

		public void SetItemType(ItemType iType)
		{
			this.itemType = iType;
		}

		public void SetSceneName(string resName)
		{
			this.assetName = resName;
		}

		public void SetIndexParam(int target)
		{
			this.indexParam = target;
		}

		public void SetStringParam(string target)
		{
			this.strParam = target;
		}

		public void SetProgressGroup(WWWProgress wwwProgress)
		{
			this.m_wwwProgress = wwwProgress;
		}

		public void SetLoadAll(bool onOff)
		{
			this.loadAll = onOff;
		}

		public void SetSkipLog(bool onOff)
		{
			this.skipLog = onOff;
		}

		public void _InternalOnly_ChangeStateRequest()
		{
			if (this.wiState != WWWItem.StateWI.UNLOADED)
			{
				if (this.wiState != WWWItem.StateWI.DESTROIED)
				{
					this.retryRequested = false;
					this.wiState = WWWItem.StateWI.REQUESTED;
					string text;
					if (string.IsNullOrEmpty(this.anotherURL))
					{
						if (Option.usePatchDir && this.m_protocol == Protocol.HTTP && this.m_kItem != null)
						{
							string protocolRootPath = Option.GetProtocolRootPath(this.m_protocol);
							string format = string.Empty;
							if (protocolRootPath[protocolRootPath.Length - 1] == '/')
							{
								if (this.assetPath[0] == '/')
								{
									format = "{0}{1}{2}";
								}
								else
								{
									format = "{0}{1}/{2}";
								}
							}
							else if (this.assetPath[0] == '/')
							{
								format = "{0}/{1}{2}";
							}
							else
							{
								format = "{0}/{1}/{2}";
							}
							text = string.Format(format, protocolRootPath, this.m_kItem.VersionDir, this.assetPath);
						}
						else
						{
							text = Option.GetProtocolRootPath(this.m_protocol) + this.assetPath;
						}
					}
					else
					{
						text = this.anotherURL;
					}
					if (this.m_protocol == Protocol.HTTP && 0 < this.retryCnt)
					{
						if (text.Contains("?r="))
						{
							text = string.Format("{0}{1}", text, this.retryCnt);
						}
						else
						{
							text = string.Format("{0}?r={1}{2}", text, (int)(UnityEngine.Random.value * 1000000f), this.retryCnt);
						}
						TsLog.Log("[TsBundle] TsBundle:Retry = \"{0}\"", new object[]
						{
							text
						});
					}
					if (Option.EnableTrace)
					{
						TsLog.Log("[TsBundle] www loading (AssetPath=\"{0}\", Stack=\"{1}\", Type={2}, Size={3:#,###,###,###}, UnloadReserved={4}, RefCount={5} ) {6}", new object[]
						{
							this.assetPath,
							this.stackName,
							this.itemType,
							(this.m_kItem != null) ? this.m_kItem.nFileSize : 0,
							this.DebugUnloadReserved,
							this.refCnt,
							(this.m_kItem != null) ? ((!TsCaching.IsVersionCached(text, this.m_kItem.nVersion, this.m_kItem.bUseCustomCache)) ? "<<will download>>" : "<<Cached>>") : "<<not listed>>"
						});
					}
					if (string.IsNullOrEmpty(this.anotherURL))
					{
						UsingAssetRecorder.RecordFile(this.assetPath);
					}
					this._ReleaseLoadedAsset();
					if (this.useLoadFromCacheOrDownload)
					{
						if (!this.inUndefinedStack)
						{
							this.m_www = Holder.CancelPreDownload(this.assetPath);
						}
						if (this.m_www == null)
						{
							this.m_www = TsCaching.LoadFromCacheOrDownload(text, this.m_kItem.nVersion, (long)this.m_kItem.nFileSize, this.m_kItem.bUseCustomCache, this);
						}
						if (!this.isCacheHit)
						{
							Holder.DbgAddWWWItemStat(this.assetPath, this.m_kItem.nFileSize);
							if (0 < this.m_kItem.nFileSize && WWWItem.OnIncreaseSizeOfDownload != null)
							{
								WWWItem.OnIncreaseSizeOfDownload(this.m_kItem.nFileSize);
							}
						}
					}
					else
					{
						Holder.DbgAddWWWItemStat(this.assetPath, 0);
						this.m_www = new WWW(text);
					}
					this.retryCnt++;
					return;
				}
			}
			try
			{
				if (Option.EnableTrace)
				{
					TsLog.Log("[TsBundle] cannot request download, already unloaded bundle (AssetPath=\"{0}\", Stack=\"{1}\", Type={2}, SIze={3:#,###,###,###})\n\rCallStack={4}", new object[]
					{
						this.assetPath,
						this.stackName,
						this.itemType,
						(this.m_kItem != null) ? this.m_kItem.nFileSize : 0,
						this.RequestCallStack
					});
				}
			}
			catch (Exception arg)
			{
				TsLog.Log("[TsBundle] " + arg, new object[0]);
			}
		}

		public void _InternalOnly_ChangeStateCancel()
		{
			this.wiState = WWWItem.StateWI.CANCEL;
			this.retryRequested = true;
		}

		public void _InternalOnly_ChangeStateSuccess()
		{
			this.wiState = WWWItem.StateWI.SUCCESS;
			this.safeBundle = null;
			this.safeBytes = null;
			this.safeString = null;
			this.safeAudioClip = null;
			if (this.useLoadFromCacheOrDownload)
			{
				this.safeSize = ((this.m_kItem == null) ? 0 : this.m_kItem.nFileSize);
			}
			else
			{
				this.safeSize = ((this.m_www == null) ? 0 : this.m_www.size);
			}
			bool flag = !this.isCacheHit && this.itemType == ItemType.UNDEFINED && this.UseCustomCache;
			if (flag)
			{
				if (Option.EnableTrace)
				{
					TsLog.LogWarning("[TsBundle] set RawBytes => Cache Hit[{0}], Use[{1}] (AssetPath=\"{2}\", Stack=\"{3}\", Type={4})", new object[]
					{
						this.isCacheHit,
						this.useLoadFromCacheOrDownload,
						this.assetPath,
						this.stackName,
						this.itemType
					});
				}
				this.rawBytes = this.m_www.bytes;
			}
			try
			{
				if (this.itemType == ItemType.USER_STRING)
				{
					this.safeString = this.m_www.text;
					if (this.safeString == null)
					{
						this._InternalOnly_ChangeStateErrorOrRetry("fail to access www.text.");
					}
				}
				else if (this.itemType == ItemType.USER_BYTESA)
				{
					this.safeBytes = ((this.rawBytes == null) ? this.m_www.bytes : this.rawBytes);
					if (this.safeBytes == null)
					{
						this._InternalOnly_ChangeStateErrorOrRetry("fail to access www.bytes.");
					}
				}
				else if (this.itemType == ItemType.USER_AUDIO)
				{
					this.safeAudioClip = this.m_www.GetAudioClip(false, true);
					if (null == this.safeAudioClip)
					{
						this._InternalOnly_ChangeStateErrorOrRetry("fail to access www.AudioClip.");
					}
				}
				else if (this.itemType == ItemType.UNDEFINED)
				{
					if (this.refCnt == 1)
					{
						this.Dispose();
					}
					if (this.m_kItem != null)
					{
						TsCaching.MarkAsUsed(this.assetPath, this.m_kItem.nVersion, this.m_kItem.bUseCustomCache);
					}
				}
				else
				{
					this.safeBundle = this.m_www.assetBundle;
					if (null == this.safeBundle)
					{
						this._InternalOnly_ChangeStateErrorOrRetry("fail to access www.assetBundle.");
						throw new NullReferenceException();
					}
					this.mainAsset = this.safeBundle.mainAsset;
					TsFix.AudioSourceWarning(this.mainAsset as GameObject);
					if (this.loadAll)
					{
						this.subBundles = this.safeBundle.LoadAll();
						UnityEngine.Object[] subBundles = this.subBundles;
						for (int i = 0; i < subBundles.Length; i++)
						{
							UnityEngine.Object @object = subBundles[i];
							TsFix.AudioSourceWarning(@object as GameObject);
						}
					}
				}
				if (Option.EnableTrace)
				{
					bool flag2 = false;
					int num = -1;
					if (this.m_kItem != null)
					{
						string url = Helper.FullURL(this.assetPath);
						num = this.m_kItem.nVersion;
						flag2 = TsCaching.IsVersionCached(url, num, this.m_kItem.bUseCustomCache);
					}
					TsLog.Log("[TsBundle] download => success (AssetPath=\"{0}\", Stack=\"{1}\", Type={2}, Version={3}, RefCnt={4}, LoadFromCacheOrDownload={5}, Disposed={6}), Cached => {7}\r\nCallStack={8}", new object[]
					{
						this.assetPath,
						this.stackName,
						this.itemType,
						num,
						this.refCnt,
						this.useLoadFromCacheOrDownload,
						this.m_www == null,
						(string.IsNullOrEmpty(this.anotherURL) || this.useLoadFromCacheOrDownload) ? flag2.ToString() : "(ignore)"
					});
				}
			}
			catch (Exception ex)
			{
				TsLog.LogError("[TsBundle] Unity Assetbundle access exception! (Protocol=\"{0}\", AssetPath=\"{1}\", Stack=\"{2}\", Type={3}) => {4}\r\nCallStack={5}", new object[]
				{
					Option.GetProtocolRootPath(this.m_protocol),
					this.assetPath,
					this.stackName,
					this.itemType,
					ex.ToString(),
					this.RequestCallStack
				});
				this._InternalOnly_ChangeStateErrorOrRetry("Exception! Unity Assetbundle access violation!");
			}
			if (!this.useLoadFromCacheOrDownload && !this.isCacheHit && 0 < this.safeSize)
			{
				Holder.DbgAddWWWItemStat(this.assetPath, this.safeSize);
				if (WWWItem.OnIncreaseSizeOfDownload != null)
				{
					WWWItem.OnIncreaseSizeOfDownload(this.safeSize);
				}
			}
		}

		public void _InternalOnly_ChangeStateServerError(string error)
		{
			this.wiState = WWWItem.StateWI.SERVER_ERR;
			TsLog.LogWarning("{0}", new object[]
			{
				error
			});
		}

		public void _InternalOnly_ChangeStateErrorOrRetry(string error)
		{
			string text = string.Format("{5}\t[TsBundle] {0} (AssetPath=\"{1}\", Stack=\"{2}\", Type={3})\r\nRequestCallStack=\"{4}\"", new object[]
			{
				error,
				this.assetPath,
				this.stackName,
				this.itemType,
				this.RequestCallStack,
				Scene.CurScene.ToString()
			});
			if (TsPlatform.IsMobile)
			{
				TsPlatform.FileLog(text);
			}
			if (error.Contains("404 Not Found"))
			{
				TsLog.LogWarning(text, new object[0]);
				this.wiState = WWWItem.StateWI.ERROR;
				this.isFileNotFound = true;
			}
			else if (error.Contains("Exception!"))
			{
				this.wiState = WWWItem.StateWI.ERROR;
			}
			else
			{
				int num = 2;
				if (string.IsNullOrEmpty(this.anotherURL))
				{
					if (TsPlatform.IsMobile)
					{
						num = 5;
					}
				}
				else
				{
					TsLog.LogWarning(text, new object[0]);
				}
				if (!this.retryRequested)
				{
					if (!Option.isPause && this.retryCnt >= num)
					{
						if (string.IsNullOrEmpty(this.anotherURL))
						{
							string text2 = string.Format("{6}\t[TsBundle] Retry: {5} - {0} (AssetPath=\"{1}\", Stack=\"{2}\", Type={3})\r\nRequestCallStack=\"{4}\"", new object[]
							{
								error,
								this.assetPath,
								this.stackName,
								this.itemType,
								this.RequestCallStack,
								this.retryCnt,
								Scene.CurScene.ToString()
							});
							TsLog.Assert(false, text2, new object[0]);
							if (WWWItem._errorCallback != null)
							{
								WWWItem._errorCallback(text2);
							}
							if (TsPlatform.IsMobile)
							{
								TsPlatform.FileLog(text2);
							}
						}
						this.wiState = WWWItem.StateWI.ERROR;
					}
					else
					{
						this.wiState = WWWItem.StateWI.CREATED;
					}
				}
			}
		}

		public void _InternalOnly_NotifyDownloadComplete()
		{
			if (WWWItem.StateWI.REQUESTED >= this.wiState)
			{
				return;
			}
			this.NotifyToProgress();
			if (this.callbackList != null && 0 < this.callbackList.Count)
			{
				try
				{
					foreach (WWWItem.CbParamObj current in this.callbackList)
					{
						try
						{
							if (current.callbackDelegate != null)
							{
								current.callbackDelegate(this, current.callbackParam);
							}
						}
						catch (Exception ex)
						{
							TsLog.LogWarning("{0} - {1}\nStack\n{2}", new object[]
							{
								ex.Message,
								this.assetPath,
								ex.StackTrace
							});
						}
					}
					this.callbackList.Clear();
				}
				catch (Exception ex2)
				{
					TsLog.LogError("CustumCaching.{0} TSBundle callback({1}) for {2}\n{3}", new object[]
					{
						ex2.Message,
						this.assetPath,
						this.callbackList.Count,
						ex2.StackTrace
					});
					this.callbackList = null;
				}
			}
		}

		public void NotifyToProgress()
		{
			if (this.m_wwwProgress != null)
			{
				this.m_wwwProgress.AddCompletionCnt();
			}
		}

		internal WWW DeliveryWWW()
		{
			if (this.safeBundle != null)
			{
				TsLog.LogWarning("[TsBundle] loaded AssetBundle (AssetPath=\"{0}\", Stack=\"{1}\", Type={2})\r\nCallStack={3}", new object[]
				{
					this.assetPath,
					this.stackName,
					this.itemType,
					this.RequestCallStack
				});
			}
			this.wiState = WWWItem.StateWI.CANCEL;
			WWW www = this.m_www;
			this.m_www = null;
			this.safeString = null;
			this.safeBytes = null;
			this.subBundles = null;
			this.safeAudioClip = null;
			return www;
		}

		private bool _ReleaseLoadedAsset()
		{
			if (this.m_www != null)
			{
				if (Option.EnableTrace)
				{
					TsLog.Log("[TsBundle] your www instance already is loaded, so dispose it (AssetPaht=\"{0}\", Stack=\"{1}\", Type={2})\r\nCallStack={3}", new object[]
					{
						this.assetPath,
						this.stackName,
						this.itemType,
						this.RequestCallStack
					});
				}
				if (this.safeBundle != null)
				{
					this.safeBundle.Unload(true);
				}
				this.m_www.Dispose();
				this.m_www = null;
				this.safeBundle = null;
				this.safeString = null;
				this.safeBytes = null;
				this.subBundles = null;
				this.safeAudioClip = null;
				return true;
			}
			return false;
		}

		public void Dispose()
		{
			if (Option.EnableTrace)
			{
				TsLog.Log("[TsBundle] Dispose (AssetPath=\"{0}\", Stack=\"{1}\", Type={2}, Size={3:#,###,###,###})", new object[]
				{
					this.assetPath,
					this.stackName,
					this.itemType,
					(this.m_kItem != null) ? this.m_kItem.nFileSize : 0
				});
			}
			if (this.canAccessAssetBundle)
			{
				this.UnloadSafeBundle(true);
			}
			if (this.m_www != null)
			{
				this.m_www.Dispose();
			}
			this.m_www = null;
			this.safeBundle = null;
			this.safeString = null;
			this.safeBytes = null;
			this.subBundles = null;
			this.safeAudioClip = null;
			this.wiState = WWWItem.StateWI.DESTROIED;
		}

		public bool IsWebRequestError(out string err)
		{
			err = string.Empty;
			if (this.m_protocol == Protocol.FILE || this.useLoadFromCacheOrDownload)
			{
				return false;
			}
			bool flag = this.m_protocol == Protocol.HTTP && !WWWItem.PageNotFoundParsing.isIgnore && this.m_www.size != 0 && this.m_www.size < 10000;
			if (!flag)
			{
				return false;
			}
			if (TsPlatform.IsIPhone && !this.assetPath.Contains(".jpg"))
			{
				flag = false;
			}
			if (flag)
			{
				char[] array = new char[20];
				byte[] bytes = this.m_www.bytes;
				Decoder decoder = Encoding.UTF8.GetDecoder();
				int num;
				int num2;
				bool flag2;
				decoder.Convert(bytes, 0, 20, array, 0, array.Length, true, out num, out num2, out flag2);
				string text = new string(array);
				if (text.Contains("!DOC"))
				{
					err = string.Format("404 Page not found! (url=\"{0}\")", this.m_www.url);
				}
			}
			return !string.IsNullOrEmpty(err);
		}

		public static bool IsVersionCached(Protocol protocol, string path, bool markAsUsed)
		{
			path = path.ToLower();
			string text = Option.GetProtocolRootPath(protocol) + path;
			string text2 = "/" + path;
			PatchFileInfo patchFileInfo = PatchFinalList.Instance.GetPatchFileInfo(text2);
			if (patchFileInfo == null)
			{
				TsLog.LogWarning("TsBundle.WWWItem.IsVersionCached Warning! - Key:{0}", new object[]
				{
					text2.ToString()
				});
				return false;
			}
			bool flag = TsCaching.IsVersionCached(text, patchFileInfo.nVersion, patchFileInfo.bUseCustomCache);
			if (flag && markAsUsed)
			{
				TsCaching.MarkAsUsed(text, patchFileInfo.nVersion, patchFileInfo.bUseCustomCache);
			}
			return flag;
		}

		public UnityEngine.Object Load(string name)
		{
			return this.m_www.assetBundle.Load(name);
		}

		public UnityEngine.Object Load(string name, Type type)
		{
			return this.m_www.assetBundle.Load(name, type);
		}

		public AssetBundleRequest LoadAsync(string name, Type type)
		{
			return this.m_www.assetBundle.LoadAsync(name, type);
		}
	}
}
