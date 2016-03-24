using NLibCs.ErrorCollector;
using System;
using UnityEngine;

internal class ErrorCollectorManager : MonoBehaviour
{
	protected static GameObject _gameobject;

	protected static ErrorCollectorManager _instance;

	protected static bool _isFirst = true;

	protected static bool _sendPersonalData;

	protected static bool _sendLogError;

	protected long _userSN;

	protected int _playerPlatformType;

	protected int _authPlatformType;

	protected string _bundleVersion = string.Empty;

	private float _lastSendTime;

	private readonly float _sendDelay = 3f;

	private readonly float _countClearDelay = 60f;

	private int _sendCount;

	private readonly int _maxSendCount = 5;

	public static void Start(string local_path, string server_IP, int server_port, ErrorCollectorHandler handler, long userSN, int playerPlatformType, int authPlatformType, string bundleVersion)
	{
		if (ErrorCollectorManager.SetObject(userSN, playerPlatformType, authPlatformType, bundleVersion))
		{
			EC_Manager.Instance.Init(server_IP, server_port, handler, local_path);
		}
		ErrorCollectorManager.OnEnable(false, false);
	}

	public static void Start(string local_path, string url_path, ErrorCollectorHandler handler, long userSN, int playerPlatformType, int authPlatformType, string bundleVersion)
	{
		if (ErrorCollectorManager.SetObject(userSN, playerPlatformType, authPlatformType, bundleVersion))
		{
			EC_Manager.Instance.Init(url_path, handler, local_path);
		}
		ErrorCollectorManager.OnEnable(false, false);
	}

	public static bool SetObject(long userSN, int playerPlatformType, int authPlatformType, string bundleVersion)
	{
		if (ErrorCollectorManager._isFirst)
		{
			ErrorCollectorManager._gameobject = new GameObject("@ErrorCollector");
			ErrorCollectorManager._gameobject.AddComponent<ErrorCollectorManager>();
			UnityEngine.Object.DontDestroyOnLoad(ErrorCollectorManager._gameobject);
			ErrorCollectorManager._instance = new ErrorCollectorManager();
			ErrorCollectorManager._instance._userSN = userSN;
			ErrorCollectorManager._instance._playerPlatformType = playerPlatformType;
			ErrorCollectorManager._instance._authPlatformType = authPlatformType;
			ErrorCollectorManager._instance._bundleVersion = bundleVersion;
			ErrorCollectorManager._isFirst = false;
			return true;
		}
		return false;
	}

	public static void OnEnable(bool sendPersonalData, bool sendLogError)
	{
		ErrorCollectorManager._sendPersonalData = sendPersonalData;
		ErrorCollectorManager._sendLogError = sendLogError;
		ErrorCollectorManager._gameobject.SetActive(true);
		Application.RegisterLogCallback(new Application.LogCallback(ErrorCollectorManager._instance.HandleException));
	}

	public static void OnDisable()
	{
		Application.RegisterLogCallback(null);
		ErrorCollectorManager._gameobject.SetActive(false);
	}

	private void HandleException(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Log || type == LogType.Warning)
		{
			return;
		}
		if (type == LogType.Error && !ErrorCollectorManager._sendLogError)
		{
			return;
		}
		if (Time.realtimeSinceStartup < this._lastSendTime + this._sendDelay)
		{
			return;
		}
		if (Time.realtimeSinceStartup > this._lastSendTime + this._countClearDelay)
		{
			this._sendCount = 0;
		}
		if (this._maxSendCount < this._sendCount)
		{
			return;
		}
		this._lastSendTime = Time.realtimeSinceStartup;
		this._sendCount++;
		string deviceModel = string.Empty;
		string deviceOS = string.Empty;
		if (ErrorCollectorManager._sendPersonalData)
		{
			deviceModel = SystemInfo.deviceModel;
			deviceOS = SystemInfo.operatingSystem;
		}
		if (stackTrace.IndexOf(" ") != -1)
		{
			condition = condition + "\r\n" + stackTrace.Substring(0, stackTrace.IndexOf(" "));
		}
		else
		{
			condition = condition + "\r\n" + stackTrace;
		}
		if (condition.Length >= 129)
		{
			condition = condition.Substring(0, 128);
		}
		if (stackTrace.Length >= 2049)
		{
			stackTrace = stackTrace.Substring(0, 2048);
		}
		if (Connector.Instance != null)
		{
			Connector.Instance.Send_Error(this._userSN, this._playerPlatformType, this._authPlatformType, this._bundleVersion, condition, stackTrace, deviceModel, deviceOS);
		}
	}
}
