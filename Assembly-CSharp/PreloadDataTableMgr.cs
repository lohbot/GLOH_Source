using NPatch;
using SERVICE;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PreloadDataTableMgr : NrTSingleton<PreloadDataTableMgr>
{
	private List<string> m_loadCompleteTableList;

	private Coroutine m_PreloadCoroutine;

	private PreloadDataTableMgr()
	{
		this.m_loadCompleteTableList = new List<string>();
	}

	public void StartPreLoadTable()
	{
		NmMainFrameWork mainFrameWork = this.GetMainFrameWork();
		if (mainFrameWork == null)
		{
			return;
		}
		if (this.m_PreloadCoroutine != null)
		{
			Debug.LogError(" ERROR, PreloadDataTableMgr.cs, StartPreLoadTable(), m_PreloadCoroutine is Not Null ");
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (this.IsPatchTableExist())
		{
			Debug.LogError("PatchTableCheckTime : " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString());
			return;
		}
		Debug.LogError("PatchTableCheckTime : " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString());
		this.m_PreloadCoroutine = mainFrameWork.StartCoroutine(TableDataLoad.Load());
	}

	public void StopPreLoadTable()
	{
		if (this.m_PreloadCoroutine == null)
		{
			return;
		}
		NmMainFrameWork mainFrameWork = this.GetMainFrameWork();
		if (mainFrameWork == null)
		{
			return;
		}
		Debug.LogError("StopPreLoadTable");
		mainFrameWork.StopCoroutine(this.m_PreloadCoroutine);
		this.m_PreloadCoroutine = null;
		GC.Collect();
	}

	public void InsertLoadCompleteTable(string strTablePath)
	{
		string fileName = Path.GetFileName(strTablePath);
		if (this.m_loadCompleteTableList == null)
		{
			Debug.LogError(" ERROR, PreloadDataTableMgr.cs, InsertLoadCompleteTable(), m_loadCompleteTableList is Null ");
			return;
		}
		if (this.m_loadCompleteTableList.Contains(fileName))
		{
			Debug.LogError(" ERROR, PreloadDataTableMgr.cs, InsertLoadCompleteTable(), m_loadCompleteTableList.Contains( strFilename ) == true ");
			return;
		}
		this.m_loadCompleteTableList.Add(fileName);
	}

	public bool IsLoadCompleteTable(string strTablePath)
	{
		string fileName = Path.GetFileName(strTablePath);
		if (this.m_loadCompleteTableList == null)
		{
			Debug.LogError(" ERROR, PreloadDataTableMgr.cs, IsLoadCompleteTable(), m_loadCompleteTableList is Null ");
			return false;
		}
		return this.m_loadCompleteTableList.Contains(fileName);
	}

	private NmMainFrameWork GetMainFrameWork()
	{
		NmMainFrameWork mainFrameWork = NmMainFrameWork.GetMainFrameWork();
		if (mainFrameWork == null)
		{
			Debug.LogError(" ERROR, PreloadDataTableMgr.cs, GetMainFrameWork(), NmMainFrameWork is Null ");
			return null;
		}
		return mainFrameWork;
	}

	private bool IsPatchTableExist()
	{
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USLOCAL)
		{
			return false;
		}
		string patchSerialUrl = NrTSingleton<NrGlobalReference>.Instance.GetPatchSerialUrl();
		string text = string.Format("{0}/at2/cacheroot/", TsPlatform.Operator.GetFileDir());
		Util.eCheckResourcePatch eCheckResourcePatch = Util.CheckRsourcePatchForLOH(patchSerialUrl, text);
		Debug.LogError("localRoot : " + text + ", eCheckResourcePatch Result : " + eCheckResourcePatch.ToString());
		return eCheckResourcePatch != Util.eCheckResourcePatch.NOT_NEED;
	}
}
