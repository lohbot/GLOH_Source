using NPatch;
using System;
using UnityEngine;

public class NPatchLauncherBehaviour : MonoBehaviour
{
	private static GameObject _owner;

	private static NPatchLauncherBehaviour _launcherBehaviour;

	public static int useFrame;

	private bool isRunning;

	private bool isFinish;

	public Launcher LanucherCore = Launcher.Instance;

	public static GameObject Owner
	{
		get
		{
			return NPatchLauncherBehaviour._owner;
		}
	}

	public static NPatchLauncherBehaviour Instance
	{
		get
		{
			return NPatchLauncherBehaviour._launcherBehaviour;
		}
	}

	public bool IsFinish
	{
		get
		{
			return this.isFinish;
		}
	}

	public bool IsRunning
	{
		get
		{
			return this.isRunning;
		}
	}

	public ERRORLEVEL ErrorLevel
	{
		get
		{
			return Launcher.Instance.PatchErrorLevel;
		}
	}

	public string ErrorString
	{
		get
		{
			return Launcher.Instance.PatchErrorString;
		}
	}

	public NPATCHSTAGE stage
	{
		get
		{
			return Launcher.Instance.Stage;
		}
	}

	private static void CreateInstance()
	{
		NPatchLauncherBehaviour._owner = new GameObject("@NPatchLauncherBehaviour");
		NPatchLauncherBehaviour._launcherBehaviour = NPatchLauncherBehaviour._owner.AddComponent<NPatchLauncherBehaviour>();
	}

	public static bool PatchStart(string root_local, string root_url, LauncherHandler handler, bool _isCallPrepackEndFuncOnlyFirstPatch, int maxver, bool isMaster = false, string apkVersion = "")
	{
		Debug.Log("maxver : " + maxver);
		if (NPatchLauncherBehaviour._owner == null)
		{
			NPatchLauncherBehaviour.CreateInstance();
		}
		NPatchLauncherBehaviour._launcherBehaviour.isRunning = false;
		NPatchLauncherBehaviour._launcherBehaviour.isFinish = false;
		NPatchLauncherBehaviour.useFrame = Application.targetFrameRate;
		Application.targetFrameRate = -1;
		NPatchLauncherBehaviour._launcherBehaviour.isRunning = NPatchLauncherBehaviour._launcherBehaviour.LanucherCore.PatchStart(root_local, root_url, handler, _isCallPrepackEndFuncOnlyFirstPatch, maxver, isMaster, apkVersion);
		return NPatchLauncherBehaviour._launcherBehaviour.isRunning;
	}

	public void Start()
	{
	}

	public void Update()
	{
		if (this.stage <= NPATCHSTAGE.RUNNINGTASK)
		{
			this.LanucherCore.PatchUpdate();
		}
		else if (this.stage == NPATCHSTAGE.ENDALLTASK)
		{
			this.isFinish = true;
			this.LanucherCore.PatchFinish();
			Application.targetFrameRate = NPatchLauncherBehaviour.useFrame;
		}
	}
}
