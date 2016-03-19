using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TsBundle;
using TsPatch;
using UnityEngine;

public class TsBundleService : MonoBehaviour
{
	private static Queue<WWWItem>[] ms_wiQueues = new Queue<WWWItem>[3];

	private static List<WWWItem> ms_wiRetryReqList = new List<WWWItem>();

	private static int ms_totToken = 3;

	private static int ms_lastFrame = 0;

	private static int ms_lastFrameChecked = -1;

	private static int _dbgCntSvcList = 0;

	private static int _dbgMaxSvcList = 0;

	private static float m_downloadDeltaTime = 0f;

	private static bool ms_retryPoll
	{
		get;
		set;
	}

	public static int dbgCntToken
	{
		get
		{
			return TsBundleService.ms_totToken;
		}
	}

	public static int dbgCntQueue
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				num += TsBundleService.ms_wiQueues[i].Count;
			}
			return num;
		}
	}

	public static int dbgCntSvcList
	{
		get
		{
			return TsBundleService._dbgCntSvcList;
		}
	}

	public static int dbgMaxSvcList
	{
		get
		{
			return TsBundleService._dbgMaxSvcList;
		}
	}

	private static void _IncToken()
	{
		TsBundleService.ms_totToken++;
		TsBundleService.ms_lastFrame = Time.frameCount;
	}

	private static void _DecToken()
	{
		TsBundleService.ms_totToken--;
		TsBundleService.ms_lastFrame = Time.frameCount;
		TsLog.Assert(TsBundleService.ms_totToken >= 0, "TsBundle token count is out of bound!", new object[0]);
	}

	private void _CheckTokenCnt()
	{
		if (TsBundleService.ms_totToken <= 0 && TsBundleService.ms_lastFrame == TsBundleService.ms_lastFrameChecked)
		{
			TsLog.LogWarning("TsBundle token count is 0. TsBundle service is blocked! Last frame of bundle operation requested is [{0}]", new object[]
			{
				TsBundleService.ms_lastFrame
			});
		}
		TsBundleService.ms_lastFrameChecked = TsBundleService.ms_lastFrame;
	}

	public static string DbgGetDownloadingFileInfo()
	{
		for (int i = 0; i < 3; i++)
		{
			Queue<WWWItem>[] array = TsBundleService.ms_wiQueues;
			for (int j = 0; j < array.Length; j++)
			{
				Queue<WWWItem> queue = array[j];
				if (0 < queue.Count)
				{
					WWWItem wWWItem = queue.Peek();
					if (wWWItem != null)
					{
						return wWWItem.assetPath;
					}
				}
			}
		}
		return string.Empty;
	}

	private void DbgIncSvcCnt(int cntItems)
	{
		TsBundleService._dbgCntSvcList++;
		if (TsBundleService._dbgCntSvcList > TsBundleService._dbgMaxSvcList)
		{
			TsBundleService._dbgMaxSvcList = TsBundleService._dbgCntSvcList;
		}
	}

	private void DbgDecSvcCnt(int cntItems)
	{
		TsBundleService._dbgCntSvcList--;
	}

	public WWWProgress RequestDownloadCoroutine(List<WWWItem> wiList, DownGroup dnGroup, bool unloadAfterPostProcess, PostProcPerList cbDownloadPostProcPerList, object cbTargetGroupGO)
	{
		StringBuilder stringBuilder = null;
		if (Option.EnableTrace)
		{
			stringBuilder = new StringBuilder(1024);
			foreach (WWWItem current in wiList)
			{
				current.DebugUnloadReserved = unloadAfterPostProcess;
				stringBuilder.AppendFormat("   \"{0}\"\r\n", current.assetPath);
				if (Option.EnableReportCallStatck)
				{
					current.RequestCallStack = StackTraceUtility.ExtractStackTrace();
				}
			}
			TsLog.Log("[TsBundle] RequestDownloadCoroutine( Unload={0} ) Requests={1}\r\n{2}", new object[]
			{
				unloadAfterPostProcess,
				wiList.Count,
				stringBuilder.ToString()
			});
		}
		WWWProgress wWWProgress = new WWWProgress(wiList);
		if (wiList == null || 0 >= wiList.Count)
		{
			TsLog.LogWarning("Download List is empty!", new object[0]);
			cbDownloadPostProcPerList(wiList, cbTargetGroupGO);
		}
		else
		{
			this._AddRequestList(wiList, dnGroup, wWWProgress);
			base.StartCoroutine(this._DownloadPerList(wiList, unloadAfterPostProcess, cbDownloadPostProcPerList, cbTargetGroupGO));
		}
		return wWWProgress;
	}

	public WWWProgress RequestDownloadCoroutine(WWWItem wItem, DownGroup dnGroup, bool unloadAfterPostProcess)
	{
		if (wItem != null)
		{
			List<WWWItem> list = new List<WWWItem>();
			if (Option.EnableReportCallStatck)
			{
				wItem.RequestCallStack = StackTraceUtility.ExtractStackTrace();
			}
			list.Add(wItem);
			if (Option.EnableTrace)
			{
				wItem.DebugUnloadReserved = unloadAfterPostProcess;
				TsLog.Log("[TsBundle] RequestDownloadCoroutine( Unload={0} ) => \"{1}\"", new object[]
				{
					unloadAfterPostProcess,
					wItem.assetPath
				});
			}
			WWWProgress wWWProgress = new WWWProgress(list);
			this._AddRequestList(list, dnGroup, wWWProgress);
			base.StartCoroutine(this._DownloadPerList(list, unloadAfterPostProcess, null, null));
			return wWWProgress;
		}
		return null;
	}

	public void RequestLoadAsync(LoadAsyncCallback asyncLoader, IDownloadedItem wItem, object targetObj, string name, Type type)
	{
		base.StartCoroutine(asyncLoader(wItem, targetObj, name, type));
	}

	[DebuggerHidden]
	private IEnumerator _DownloadPerList(List<WWWItem> wiList, bool unloadAfterPostProcess, PostProcPerList cbDownloadPostProcPerList, object cbTargetGroupGO)
	{
		TsBundleService.<_DownloadPerList>c__Iterator20 <_DownloadPerList>c__Iterator = new TsBundleService.<_DownloadPerList>c__Iterator20();
		<_DownloadPerList>c__Iterator.wiList = wiList;
		<_DownloadPerList>c__Iterator.cbDownloadPostProcPerList = cbDownloadPostProcPerList;
		<_DownloadPerList>c__Iterator.cbTargetGroupGO = cbTargetGroupGO;
		<_DownloadPerList>c__Iterator.unloadAfterPostProcess = unloadAfterPostProcess;
		<_DownloadPerList>c__Iterator.<$>wiList = wiList;
		<_DownloadPerList>c__Iterator.<$>cbDownloadPostProcPerList = cbDownloadPostProcPerList;
		<_DownloadPerList>c__Iterator.<$>cbTargetGroupGO = cbTargetGroupGO;
		<_DownloadPerList>c__Iterator.<$>unloadAfterPostProcess = unloadAfterPostProcess;
		<_DownloadPerList>c__Iterator.<>f__this = this;
		return <_DownloadPerList>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator _DownloadPerItem(WWWItem wItem, int prioIdx)
	{
		TsBundleService.<_DownloadPerItem>c__Iterator21 <_DownloadPerItem>c__Iterator = new TsBundleService.<_DownloadPerItem>c__Iterator21();
		<_DownloadPerItem>c__Iterator.wItem = wItem;
		<_DownloadPerItem>c__Iterator.prioIdx = prioIdx;
		<_DownloadPerItem>c__Iterator.<$>wItem = wItem;
		<_DownloadPerItem>c__Iterator.<$>prioIdx = prioIdx;
		<_DownloadPerItem>c__Iterator.<>f__this = this;
		return <_DownloadPerItem>c__Iterator;
	}

	private void _DownloadPerItemPreOp(WWWItem wItem, int prioIdx)
	{
		wItem._InternalOnly_ChangeStateRequest();
		TsBundleService._DecToken();
	}

	private void _DownloadPerItemPostOp(WWWItem wItem, int prioIdx)
	{
		TsBundleService._IncToken();
		if (!wItem.isCanceled)
		{
			string error;
			if (!string.IsNullOrEmpty(wItem.errorString))
			{
				wItem._InternalOnly_ChangeStateErrorOrRetry(wItem.errorString);
			}
			else if (wItem.IsWebRequestError(out error))
			{
				wItem._InternalOnly_ChangeStateServerError(error);
			}
			else
			{
				wItem._InternalOnly_ChangeStateSuccess();
			}
			if (!wItem.isCompleteAsyncOp && !wItem.retryRequested)
			{
				wItem.retryRequested = true;
				TsBundleService.ms_wiRetryReqList.Add(wItem);
			}
		}
	}

	private void _AddRequestList(List<WWWItem> wiList, DownGroup reqPriority, WWWProgress wwwProgress)
	{
		Queue<WWWItem> queue = TsBundleService.ms_wiQueues[(int)reqPriority];
		if (queue == null)
		{
			TsLog.LogError("[Stack:{0}] TsBundleService is not initialized! Check Awake() & Start() dependency first!", new object[]
			{
				reqPriority
			});
		}
		else
		{
			foreach (WWWItem current in wiList)
			{
				current.SetProgressGroup(wwwProgress);
				if (current.isCached)
				{
					queue.Enqueue(current);
				}
				else
				{
					queue.Enqueue(current);
				}
			}
		}
	}

	private void _WWWProcessWaitOrComplete(int idx, WWWItem wItem)
	{
		if (wItem.justCreated)
		{
			TsImmortal.bundleService.StartCoroutine(this._DownloadPerItem(wItem, idx));
		}
		else
		{
			wItem._InternalOnly_NotifyDownloadComplete();
		}
	}

	private void OnApplicationQuit()
	{
		base.StopAllCoroutines();
		Holder.ClearHolder();
	}

	private void Awake()
	{
		TsBundleService.ms_totToken = PlayerPrefs.GetInt("maxbundledownload", 3);
		for (int i = 0; i < 3; i++)
		{
			if (TsBundleService.ms_wiQueues[i] == null)
			{
				TsBundleService.ms_wiQueues[i] = new Queue<WWWItem>();
			}
		}
		UnityEngine.Object.DontDestroyOnLoad(this);
		base.InvokeRepeating("_ResetRetryPoll", 2f, 1f);
		base.InvokeRepeating("_CheckTokenCnt", 30f, 30f);
	}

	private void _ResetRetryPoll()
	{
		TsBundleService.ms_retryPoll = true;
	}

	private void Start()
	{
		this.Init();
	}

	public void Init()
	{
		base.StartCoroutine(this.PatchListLoadAsync());
	}

	[DebuggerHidden]
	private IEnumerator PatchListLoadAsync()
	{
		return new TsBundleService.<PatchListLoadAsync>c__Iterator22();
	}

	private void Update()
	{
		if (!PatchFinalList.Instance.isLoadedOrSkipped || Option.isPause)
		{
			return;
		}
		if (!TsCaching.ready)
		{
			return;
		}
		TsBundleService._UpdateCommon();
	}

	private static void _UpdateCommon()
	{
		if (TsBundleService.ms_retryPoll)
		{
			Queue<WWWItem> queue = TsBundleService.ms_wiQueues[1];
			foreach (WWWItem current in TsBundleService.ms_wiRetryReqList)
			{
				queue.Enqueue(current);
			}
			TsBundleService.ms_wiRetryReqList.Clear();
			TsBundleService.ms_retryPoll = false;
		}
		bool pauseBGLOAD = Option.PauseBGLOAD;
		float downloadDelay = Option.DownloadDelay;
		TsBundleService.m_downloadDeltaTime += Time.deltaTime;
		int i = 0;
		while (i < 3)
		{
			Queue<WWWItem> queue = TsBundleService.ms_wiQueues[i];
			bool flag = false;
			if (i != 2)
			{
				goto IL_BE;
			}
			if (!pauseBGLOAD && TsBundleService.m_downloadDeltaTime >= downloadDelay)
			{
				if (downloadDelay != 0f)
				{
					flag = true;
					goto IL_BE;
				}
				goto IL_BE;
			}
			IL_FB:
			i++;
			continue;
			IL_BE:
			while (0 < TsBundleService.ms_totToken && 0 < queue.Count)
			{
				WWWItem wItem = queue.Dequeue();
				TsImmortal.bundleService._WWWProcessWaitOrComplete(i, wItem);
				if (flag)
				{
					break;
				}
			}
			goto IL_FB;
		}
		if (TsBundleService.m_downloadDeltaTime >= downloadDelay)
		{
			TsBundleService.m_downloadDeltaTime = 0f;
		}
	}
}
