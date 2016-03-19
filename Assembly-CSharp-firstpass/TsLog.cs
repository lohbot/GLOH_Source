using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TsLog : MonoBehaviour
{
	public enum ELogType
	{
		Default = -1,
		Error,
		Assert,
		Warning,
		Log,
		Exception,
		ALL,
		ALL_Except_Log
	}

	public enum EShowLogCount
	{
		Default = -1,
		_50 = 50,
		_100 = 100,
		_300 = 300,
		_500 = 500,
		_1000 = 1000,
		_2000 = 2000
	}

	public enum EDisplayType
	{
		Default = -1,
		Normal,
		Normal_Time,
		Normal_Frame,
		Normal_UsedHeapSize,
		Debug
	}

	public class LogContainer : List<TsLog.LogData>
	{
	}

	public class AssertLog
	{
		internal class _Info
		{
			public class _InfoComparer : IEqualityComparer<TsLog.AssertLog._Info>
			{
				public bool Equals(TsLog.AssertLog._Info x, TsLog.AssertLog._Info y)
				{
					return x.guiContent.text == y.guiContent.text;
				}

				public int GetHashCode(TsLog.AssertLog._Info obj)
				{
					return obj.guiContent.text.GetHashCode();
				}
			}

			public Rect rectWindow;

			public Rect rectOK;

			public GUIContent guiContent;

			public bool IsInitPos
			{
				get;
				set;
			}

			public _Info(string logMessage)
			{
				this.rectWindow = default(Rect);
				this.rectOK = default(Rect);
				this.guiContent = new GUIContent(logMessage);
			}
		}

		private HashSet<TsLog.AssertLog._Info> _logInfos = new HashSet<TsLog.AssertLog._Info>(new TsLog.AssertLog._Info._InfoComparer());

		public void Add(string logMessage)
		{
			if (Application.isEditor)
			{
				return;
			}
			TsLog.AssertLog._Info item = new TsLog.AssertLog._Info(logMessage);
			this._logInfos.Add(item);
		}

		public void Clear()
		{
			this._logInfos.Clear();
		}
	}

	[Serializable]
	public class Condition
	{
		[SerializeField]
		private TsLog.ELogType _logType = TsLog.ELogType.Default;

		[SerializeField]
		private TsLog.EDisplayType _displayType;

		private TsLog.Filter _filter = TsLog.Filter.LoadedFilter;

		[SerializeField]
		private TsLog.EShowLogCount _showLogCount = TsLog.EShowLogCount.Default;

		public TsLog.ELogType LogType
		{
			get
			{
				return this._logType;
			}
			set
			{
				if (value == TsLog.ELogType.Default)
				{
					value = TsLog.ELogType.ALL;
				}
				if (this._logType != value)
				{
					this.IsChangedCondition = true;
				}
				this._logType = value;
			}
		}

		public TsLog.EDisplayType DisplayType
		{
			get
			{
				return this._displayType;
			}
			set
			{
				if (value == TsLog.EDisplayType.Default)
				{
					value = TsLog.EDisplayType.Normal;
				}
				this._displayType = value;
			}
		}

		public TsLog.Filter FilterRef
		{
			get
			{
				return this._filter;
			}
			set
			{
				if (!this._filter.IsSame(value))
				{
					this.IsChangedCondition = true;
				}
				this._filter = value;
			}
		}

		public TsLog.EShowLogCount ShowLogCount
		{
			get
			{
				return this._showLogCount;
			}
			set
			{
				if (value == TsLog.EShowLogCount.Default)
				{
					value = TsLog.EShowLogCount._300;
				}
				if (this._showLogCount != value)
				{
					this.IsChangedCondition = true;
				}
				this._showLogCount = value;
			}
		}

		public bool IsChangedCondition
		{
			get;
			set;
		}

		public void Clear()
		{
			this.IsChangedCondition = true;
		}

		public override string ToString()
		{
			return string.Format("Type= {0}, Display= {1}, ShowCount= {2}, Filter.Inc= {3}, Filter.Exc= {4}", new object[]
			{
				this.LogType,
				this.DisplayType,
				this.ShowLogCount,
				this.FilterRef.IncludeText,
				this.FilterRef.ExcludeText
			});
		}
	}

	public class Filter
	{
		public static TsLog.Filter NoneFilter = new TsLog.Filter(string.Empty, string.Empty);

		public static TsLog.Filter LoadedFilter = TsLog.Filter.NoneFilter;

		public static readonly char[] SEPARATOR = new char[]
		{
			'+'
		};

		public static readonly string INCLUDE_KEY = "TsLog.Filter.Include";

		public static readonly string EXCLUDE_KEY = "TsLog.Filter.Exclude";

		public string IncludeText
		{
			get;
			private set;
		}

		public string ExcludeText
		{
			get;
			private set;
		}

		public string[] IncludeTexts
		{
			get;
			private set;
		}

		public string[] ExcludeTexts
		{
			get;
			private set;
		}

		public Filter(string includeText, string excludeText)
		{
			this.IncludeText = includeText.ToUpper();
			this.ExcludeText = excludeText.ToUpper();
			this.IncludeTexts = this.IncludeText.Split(TsLog.Filter.SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
			this.ExcludeTexts = this.ExcludeText.Split(TsLog.Filter.SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
		}

		public bool IsPassByFilter(string message)
		{
			string text = message.ToUpper();
			if (this.ExcludeTexts.Length >= 1)
			{
				string[] excludeTexts = this.ExcludeTexts;
				for (int i = 0; i < excludeTexts.Length; i++)
				{
					string value = excludeTexts[i];
					if (text.Contains(value))
					{
						return false;
					}
				}
			}
			if (this.IncludeTexts.Length <= 0)
			{
				return true;
			}
			string[] includeTexts = this.IncludeTexts;
			for (int j = 0; j < includeTexts.Length; j++)
			{
				string value2 = includeTexts[j];
				if (text.Contains(value2))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsSame(TsLog.Filter filter)
		{
			return this.ExcludeText.Equals(filter.ExcludeText, StringComparison.CurrentCultureIgnoreCase) && this.IncludeText.Equals(filter.IncludeText, StringComparison.CurrentCultureIgnoreCase);
		}

		public override string ToString()
		{
			return string.Format("Filter Include[ {0} ] Exclude[ {1} ]", this.IncludeText, this.ExcludeText);
		}

		public string GetDisplayString()
		{
			return string.Format("Include[ {0} ]    Exclude[ {1} ]", this.IncludeText, this.ExcludeText);
		}

		public static string MakeFilterText(string[] textList)
		{
			string text = string.Empty;
			for (int i = 0; i < textList.Length; i++)
			{
				text += textList[i];
				if (i < textList.Length - 1)
				{
					text += TsLog.Filter.SEPARATOR[0];
				}
			}
			return text;
		}

		public static void Save(TsLog.Filter filter)
		{
			PlayerPrefs.SetString(TsLog.Filter.INCLUDE_KEY, filter.IncludeText);
			PlayerPrefs.SetString(TsLog.Filter.EXCLUDE_KEY, filter.ExcludeText);
			TsLog.Filter.Load();
		}

		public static void Load()
		{
			string text = PlayerPrefs.GetString(TsLog.Filter.INCLUDE_KEY);
			if (string.IsNullOrEmpty(text))
			{
				text = string.Empty;
				PlayerPrefs.SetString(TsLog.Filter.INCLUDE_KEY, text);
			}
			string text2 = PlayerPrefs.GetString(TsLog.Filter.EXCLUDE_KEY);
			if (string.IsNullOrEmpty(text2))
			{
				text2 = string.Empty;
				PlayerPrefs.SetString(TsLog.Filter.EXCLUDE_KEY, text2);
			}
			TsLog.Filter.LoadedFilter = new TsLog.Filter(text, text2);
		}
	}

	public class LogData
	{
		public class StackInfo
		{
			public string fileName
			{
				get;
				private set;
			}

			public string methodName
			{
				get;
				private set;
			}

			public int lineNum
			{
				get;
				private set;
			}

			public StackInfo(string fileName, string methodName, int lineNum)
			{
				this.fileName = fileName;
				this.methodName = methodName;
				this.lineNum = lineNum;
			}
		}

		public class FrameInfo
		{
			public string frameText
			{
				get;
				private set;
			}

			public string fileName
			{
				get;
				private set;
			}

			public int line
			{
				get;
				private set;
			}

			public FrameInfo(string frameText, string fileName, int line)
			{
				this.frameText = frameText;
				this.fileName = fileName;
				this.line = line;
			}
		}

		public static readonly TsLog.LogData.FrameInfo DummyFrameInfo = new TsLog.LogData.FrameInfo("dummy", "dummy", 0);

		internal static string[] SEP = new string[]
		{
			"\n"
		};

		internal static string FileName_Sep1 = "(at ";

		internal static string FileName_Sep2 = "s:";

		private StringBuilder _toString = new StringBuilder(512);

		private TsLog.EDisplayType _displayType = TsLog.EDisplayType.Default;

		private string m_LineFeed;

		public TsLog.ELogType logType
		{
			get;
			private set;
		}

		public string message
		{
			get;
			private set;
		}

		public List<TsLog.LogData.StackInfo> stackInfos
		{
			get;
			private set;
		}

		public List<TsLog.LogData.FrameInfo> frameInfos
		{
			get;
			private set;
		}

		public TsLog.LogData.FrameInfo activeFrameInfo
		{
			get;
			private set;
		}

		public float time
		{
			get;
			private set;
		}

		public int frame
		{
			get;
			private set;
		}

		public uint usedHeapSize
		{
			get;
			private set;
		}

		private LogData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			this.m_LineFeed = stringBuilder.ToString();
		}

		public static TsLog.LogData Create(TsLog.ELogType logType, string message, string stackTrace)
		{
			TsLog.LogData logData = new TsLog.LogData();
			try
			{
				logData.logType = logType;
				logData.message = message;
			}
			catch (Exception ex)
			{
				logData.logType = TsLog.ELogType.Error;
				logData.message = "param���ڸ� Ȯ�����ּ���~!! exception= " + ex.ToString();
			}
			logData.time = Time.time;
			logData.frame = Time.frameCount;
			logData.usedHeapSize = Profiler.usedHeapSize;
			logData.stackInfos = TsLog.LogData.CreateStackInfo(stackTrace);
			logData.frameInfos = new List<TsLog.LogData.FrameInfo>();
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder(128);
			foreach (TsLog.LogData.StackInfo current in logData.stackInfos)
			{
				string text = current.fileName;
				if (!string.IsNullOrEmpty(text))
				{
					int num = text.IndexOf("Assets");
					if (num != -1)
					{
						if (logData.activeFrameInfo == null)
						{
							flag = true;
						}
					}
					else
					{
						num = text.IndexOf("Editor");
					}
					if (num != -1)
					{
						text = text.Substring(num);
					}
				}
				stringBuilder.Remove(0, stringBuilder.Length);
				stringBuilder.AppendFormat("{0}.{1}()", Path.GetFileNameWithoutExtension(text), current.methodName);
				TsLog.LogData.FrameInfo frameInfo = new TsLog.LogData.FrameInfo(string.Format("{0}   [{1}] : {2}", stringBuilder, text, current.lineNum), text, current.lineNum);
				logData.frameInfos.Add(frameInfo);
				if (flag)
				{
					logData.activeFrameInfo = frameInfo;
					flag = false;
				}
			}
			if (logData.activeFrameInfo == null)
			{
				if (logData.frameInfos.Count <= 0)
				{
					logData.activeFrameInfo = TsLog.LogData.DummyFrameInfo;
				}
				else
				{
					logData.activeFrameInfo = logData.frameInfos[0];
				}
			}
			return logData;
		}

		public static TsLog.LogData Create_CompileLog(TsLog.ELogType logType, string compileLog)
		{
			TsLog.LogData logData = TsLog.LogData.Create(logType, compileLog, string.Empty);
			if (logData != null)
			{
				try
				{
					int num = compileLog.IndexOf('(');
					int num2 = compileLog.IndexOf(',');
					string text = compileLog.Substring(0, num);
					string s = compileLog.Substring(num + 1, num2 - num - 1);
					int num3;
					if (!int.TryParse(s, out num3))
					{
						num3 = 0;
					}
					TsLog.LogData.FrameInfo frameInfo = new TsLog.LogData.FrameInfo(string.Format("{0} : {1}", text, num3), text, num3);
					logData.frameInfos.Insert(0, frameInfo);
					logData.activeFrameInfo = frameInfo;
				}
				catch
				{
				}
			}
			return logData;
		}

		public static List<TsLog.LogData.StackInfo> CreateStackInfo(string fullStack)
		{
			List<TsLog.LogData.StackInfo> list = new List<TsLog.LogData.StackInfo>();
			if (!string.IsNullOrEmpty(fullStack))
			{
				string[] array = fullStack.Split(TsLog.LogData.SEP, StringSplitOptions.None);
				string text = string.Empty;
				string methodName = string.Empty;
				string text2 = string.Empty;
				string text3 = string.Empty;
				for (int i = 0; i < array.Length; i++)
				{
					text = array[i];
					if (!text.Contains("Debug:Log") && !text.Contains("TsLog:Log") && !text.Contains("TsLog:Ass") && !text.Contains("TsLog:Exc"))
					{
						int num = text.LastIndexOf(TsLog.LogData.FileName_Sep1);
						if (num != -1)
						{
							methodName = text.Substring(0, num);
							text2 = text.Substring(num + TsLog.LogData.FileName_Sep1.Length);
							num = text2.LastIndexOf(TsLog.LogData.FileName_Sep2);
							if (num != -1)
							{
								text3 = text2.Substring(num + TsLog.LogData.FileName_Sep2.Length);
								text3 = text3.Remove(text3.Length - 1, 1);
								int lineNum;
								if (int.TryParse(text3, out lineNum))
								{
									text2 = text2.Remove(num + 1);
									TsLog.LogData.StackInfo item = new TsLog.LogData.StackInfo(text2, methodName, lineNum);
									list.Add(item);
								}
							}
						}
					}
				}
			}
			return list;
		}

		public string ToLogString()
		{
			string result;
			switch (this._displayType)
			{
			case TsLog.EDisplayType.Normal:
				result = this.message;
				break;
			case TsLog.EDisplayType.Normal_Time:
				result = string.Format("{0}    Time[{1:#.###}]", this.message, this.time);
				break;
			case TsLog.EDisplayType.Normal_Frame:
				result = string.Format("{0}    Frame[{1}]", this.message, this.frame);
				break;
			case TsLog.EDisplayType.Normal_UsedHeapSize:
				result = string.Format("{0}    HeapSize[{1:###,###,###}]", this.message, this.usedHeapSize);
				break;
			case TsLog.EDisplayType.Debug:
				result = string.Format("LogType[{0}] Message[{1}] Time[{2}] Frame[{3}]", new object[]
				{
					this.logType,
					this.message,
					this.time,
					this.frame
				});
				break;
			default:
				result = string.Format("Error~! Invalid TsLog.EToStringType", new object[0]);
				break;
			}
			return result;
		}

		public override string ToString()
		{
			if (this._displayType != TsLog.Instance.ConditionRef.DisplayType)
			{
				this._displayType = TsLog.Instance.ConditionRef.DisplayType;
				this._toString.Remove(0, this._toString.Length);
				switch (this._displayType)
				{
				case TsLog.EDisplayType.Normal:
					this._toString = this._toString.AppendFormat("{0}{1}{2}", this.message, this.m_LineFeed, this.activeFrameInfo.frameText);
					break;
				case TsLog.EDisplayType.Normal_Time:
					this._toString = this._toString.AppendFormat("{0}    Time[{1:#.###}] {2}{3}", new object[]
					{
						this.message,
						this.time,
						this.m_LineFeed,
						this.activeFrameInfo.frameText
					});
					break;
				case TsLog.EDisplayType.Normal_Frame:
					this._toString = this._toString.AppendFormat("{0}    Frame[{1}] {2}{3}", new object[]
					{
						this.message,
						this.frame,
						this.m_LineFeed,
						this.activeFrameInfo.frameText
					});
					break;
				case TsLog.EDisplayType.Normal_UsedHeapSize:
					this._toString = this._toString.AppendFormat("{0}    HeapSize[{1:###,###,###}] {2}{3}", new object[]
					{
						this.message,
						this.usedHeapSize,
						this.m_LineFeed,
						this.activeFrameInfo.frameText
					});
					break;
				case TsLog.EDisplayType.Debug:
					this._toString = this._toString.AppendFormat("LogType[{0}] Message[{1}] Time[{2}] Frame[{3}]{4}{5}", new object[]
					{
						this.logType,
						this.message,
						this.time,
						this.frame,
						this.m_LineFeed,
						this.activeFrameInfo.frameText
					});
					break;
				default:
					this._toString = this._toString.Append("Error~! Invalid TsLog.EToStringType");
					break;
				}
			}
			return this._toString.ToString();
		}
	}

	public delegate void OnEvent_Log(TsLog.LogData log);

	[SerializeField]
	private bool[] s_disable = new bool[5];

	public static readonly int MAX_LogCount = 2000;

	[SerializeField]
	private TsLog.LogContainer _logDatas = new TsLog.LogContainer();

	private TsLog.LogContainer _collectedLogDatas = new TsLog.LogContainer();

	private TsLog.Condition _condition = new TsLog.Condition();

	private TsLog.AssertLog _assertLog = new TsLog.AssertLog();

	private static readonly string GO_NAME = "@TsLogSytstem";

	private TsLog.OnEvent_Log _eventLog;

	private static TsLog s_instance;

	[SerializeField]
	public bool isErrorPause
	{
		get;
		set;
	}

	public bool needToFirstConnectLogViewer
	{
		get;
		set;
	}

	public TsLog.LogContainer _InternalOnly_LogDatas
	{
		get
		{
			return this._logDatas;
		}
	}

	public TsLog.LogContainer _InternalOnly_CollectedLogDatas
	{
		get
		{
			return this._collectedLogDatas;
		}
	}

	public TsLog.Condition ConditionRef
	{
		get
		{
			return this._condition;
		}
	}

	public TsLog.LogData SelectedLog
	{
		get;
		set;
	}

	public static TsLog Instance
	{
		get
		{
			if (TsLog.s_instance == null)
			{
				GameObject gameObject = GameObject.Find(TsLog.GO_NAME);
				if (gameObject == null)
				{
					gameObject = new GameObject(TsLog.GO_NAME);
				}
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				TsLog.s_instance = gameObject.GetComponent<TsLog>();
				if (TsLog.s_instance == null)
				{
					TsLog.s_instance = gameObject.AddComponent<TsLog>();
				}
				TsLog.s_instance.enabled = false;
				TsLog.s_instance.enabled = true;
				if (!TsLog.s_instance._Init())
				{
					TsLog.LogError("Failed~! Cannot Initialize TsLog~!", new object[0]);
				}
			}
			return TsLog.s_instance;
		}
	}

	public static bool EnableLogType(TsLog.ELogType eLogType)
	{
		return !TsLog.Instance.s_disable[(int)eLogType];
	}

	public static void EnableLogType(TsLog.ELogType eLogType, bool enable)
	{
		TsLog.Instance.s_disable[(int)eLogType] = !enable;
	}

	public static void EnableAllLogType(bool log, bool warning, bool error, bool assert, bool exception)
	{
		TsLog.EnableLogType(TsLog.ELogType.Log, log);
		TsLog.EnableLogType(TsLog.ELogType.Warning, warning);
		TsLog.EnableLogType(TsLog.ELogType.Error, error);
		TsLog.EnableLogType(TsLog.ELogType.Assert, assert);
		TsLog.EnableLogType(TsLog.ELogType.Exception, exception);
	}

	public void FirstInit()
	{
	}

	public void RegisterUnityCallback()
	{
		Application.RegisterLogCallback(new Application.LogCallback(TsLog.LogFromUnity));
	}

	private bool _Init()
	{
		this.RegisterUnityCallback();
		TsLog.Filter.Load();
		this._condition.FilterRef = TsLog.Filter.LoadedFilter;
		this.needToFirstConnectLogViewer = true;
		return true;
	}

	public static void RegisterEvent_Log(TsLog.OnEvent_Log eventFunc)
	{
		if (TsLog.Instance._eventLog == null)
		{
			TsLog.Instance._eventLog = eventFunc;
		}
		else
		{
			TsLog expr_24 = TsLog.Instance;
			expr_24._eventLog = (TsLog.OnEvent_Log)Delegate.Combine(expr_24._eventLog, eventFunc);
		}
	}

	public static void UnregisterEvent_Log(TsLog.OnEvent_Log eventFunc)
	{
		if (TsLog.Instance._eventLog == null)
		{
			return;
		}
		TsLog expr_15 = TsLog.Instance;
		expr_15._eventLog = (TsLog.OnEvent_Log)Delegate.Remove(expr_15._eventLog, eventFunc);
	}

	public bool IsSelected(TsLog.LogData log)
	{
		return this.SelectedLog == log;
	}

	public List<TsLog.LogData> CollectByCondition()
	{
		if (this._condition.IsChangedCondition)
		{
			this._CollectByCondition();
			this._condition.IsChangedCondition = false;
		}
		return this._collectedLogDatas;
	}

	private void _CollectByCondition()
	{
		this._collectedLogDatas.Clear();
		int showLogCount = (int)this._condition.ShowLogCount;
		for (int i = this._logDatas.Count - 1; i >= 0; i--)
		{
			TsLog.LogData logData = this._logDatas[i];
			if (showLogCount >= this._collectedLogDatas.Count)
			{
				bool flag = this._condition.FilterRef.IsPassByFilter(logData.message);
				bool flag2;
				if (this._condition.LogType == TsLog.ELogType.ALL)
				{
					flag2 = true;
				}
				else if (this._condition.LogType == TsLog.ELogType.ALL_Except_Log)
				{
					flag2 = (logData.logType != TsLog.ELogType.Log);
				}
				else
				{
					flag2 = (this._condition.LogType == logData.logType);
				}
				if (flag && flag2)
				{
					this._collectedLogDatas.Insert(0, logData);
				}
			}
		}
	}

	public void ClearLogData()
	{
		this.SelectedLog = null;
		this._condition.Clear();
		this._collectedLogDatas.Clear();
		this._logDatas.Clear();
		this._assertLog.Clear();
	}

	public void GetSafetyLog(ref int collectedLogIndex, ref TsLog.LogData returnLog)
	{
		collectedLogIndex = Mathf.Max(collectedLogIndex, this._collectedLogDatas.Count);
		collectedLogIndex = Mathf.Min(collectedLogIndex, 0);
		if (this._collectedLogDatas.Count >= 1)
		{
			returnLog = this._collectedLogDatas[collectedLogIndex];
		}
	}

	public static void LogFromUnity(string condition, string stackTrace, LogType type)
	{
		TsLog.Instance._LogFromUnity(TsLog.LogData.Create((TsLog.ELogType)type, condition, stackTrace));
	}

	private void _LogFromUnity(TsLog.LogData logData)
	{
		if (logData.logType == TsLog.ELogType.Error || logData.logType == TsLog.ELogType.Assert || logData.logType == TsLog.ELogType.Exception)
		{
			this.isErrorPause = true;
		}
		if (Application.isEditor)
		{
			this._condition.Clear();
			if (this._logDatas.Count >= TsLog.MAX_LogCount)
			{
				this._logDatas.RemoveAt(0);
			}
			this._logDatas.Add(logData);
		}
		if (this._eventLog != null)
		{
			this._eventLog(logData);
		}
	}

	public static void _InternalOnly_CompileLog(TsLog.ELogType logType, string compileLog)
	{
		TsLog.Instance._LogFromUnity(TsLog.LogData.Create_CompileLog(logType, compileLog));
	}

	public static void LogByType(TsLog.ELogType logType, string message, params object[] objs)
	{
		if (!TsLog.EnableLogType(logType))
		{
			return;
		}
		string text = string.Format(message, objs);
		switch (logType)
		{
		case TsLog.ELogType.Error:
			Debug.LogError(text);
			break;
		case TsLog.ELogType.Assert:
		case TsLog.ELogType.Exception:
			TsLog.Instance._assertLog.Add(text);
			Debug.LogError(text);
			break;
		case TsLog.ELogType.Warning:
			Debug.LogWarning(text);
			break;
		case TsLog.ELogType.Log:
			Debug.Log(text);
			break;
		}
	}

	public static void Log(object obj)
	{
		if (!TsLog.EnableLogType(TsLog.ELogType.Log))
		{
			return;
		}
		Debug.Log(obj);
	}

	public static void Log(string message, params object[] objs)
	{
		if (!TsLog.EnableLogType(TsLog.ELogType.Log))
		{
			return;
		}
		string message2 = string.Format(message, objs);
		Debug.Log(message2);
	}

	public static void LogOnlyEditor(object obj)
	{
		if (!TsLog.EnableLogType(TsLog.ELogType.Log))
		{
			return;
		}
		if (!TsPlatform.IsEditor)
		{
			return;
		}
		Debug.Log(obj);
	}

	public static void LogError(string message, params object[] objs)
	{
		if (!TsLog.EnableLogType(TsLog.ELogType.Error))
		{
			return;
		}
		string message2 = string.Format(message, objs);
		Debug.LogError(message2);
	}

	public static void LogError(object obj)
	{
		if (!TsLog.EnableLogType(TsLog.ELogType.Error))
		{
			return;
		}
		Debug.LogError(obj);
	}

	public static void LogWarning(string message, params object[] objs)
	{
		if (!TsLog.EnableLogType(TsLog.ELogType.Warning))
		{
			return;
		}
		string message2 = string.Format(message, objs);
		Debug.LogWarning(message2);
	}

	public static void LogWarning(object obj)
	{
		if (!TsLog.EnableLogType(TsLog.ELogType.Warning))
		{
			return;
		}
		Debug.LogWarning(obj);
	}

	public static void Assert(bool condition)
	{
		if (condition)
		{
			return;
		}
		if (!TsLog.EnableLogType(TsLog.ELogType.Assert))
		{
			return;
		}
		string fullStack = StackTraceUtility.ExtractStackTrace();
		List<TsLog.LogData.StackInfo> list = TsLog.LogData.CreateStackInfo(fullStack);
		if (list.Count <= 0)
		{
			return;
		}
		TsLog.LogData.StackInfo stackInfo = list[0];
		string text = string.Format("Assert~! {0}  [ {1} : {2} ]", stackInfo.methodName, stackInfo.fileName, stackInfo.lineNum);
		Debug.LogError(text);
		TsLog.Instance._assertLog.Add(text);
	}

	public static void Assert(bool condition, string message, params object[] objs)
	{
		if (condition)
		{
			return;
		}
		if (!TsLog.EnableLogType(TsLog.ELogType.Assert))
		{
			return;
		}
		string text = string.Format(message, objs);
		Debug.LogError(text);
		TsLog.Instance._assertLog.Add(text);
	}

	public static void Exception(string message, params object[] objs)
	{
		if (!TsLog.EnableLogType(TsLog.ELogType.Exception))
		{
			return;
		}
		string text = string.Format(message, objs);
		Debug.LogError(text);
		TsLog.Instance._assertLog.Add(text);
	}

	public static void Exception(object obj)
	{
		if (!TsLog.EnableLogType(TsLog.ELogType.Exception))
		{
			return;
		}
		Debug.LogError(obj);
		TsLog.Instance._assertLog.Add(obj.ToString());
	}
}
