using System;
using UnityEngine;

public class DumpManager : MonoBehaviour
{
	private static DumpManager _dumpManager;

	private static readonly string _dumpManagerName = "@DumpManager";

	private Dump_Base _dumpUtil;

	public static DumpManager GetInstance()
	{
		if (DumpManager._dumpManager != null)
		{
			return DumpManager._dumpManager;
		}
		GameObject gameObject = new GameObject(DumpManager._dumpManagerName);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		DumpManager._dumpManager = gameObject.AddComponent<DumpManager>();
		DumpManager._dumpManager.RegistPlatformUtil();
		gameObject.AddComponent<UnityExceptionHandler>();
		return DumpManager._dumpManager;
	}

	public void RegistDumpHandler()
	{
		if (this._dumpUtil == null)
		{
			return;
		}
		this._dumpUtil.RegistDumpHandler();
	}

	public void SendMail(string subject, string body)
	{
		if (this._dumpUtil == null)
		{
			return;
		}
		this._dumpUtil.SendMail(subject, body);
	}

	public void CrashTest()
	{
		if (this._dumpUtil == null)
		{
			return;
		}
		this._dumpUtil.ForceCrash();
	}

	private void RegistPlatformUtil()
	{
		this._dumpUtil = new Dump_Android();
	}
}
