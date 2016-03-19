using System;
using UnityEngine;

namespace omniata
{
	public class Log : MonoBehaviour
	{
		private static LogLevel logLevel;

		public static LogLevel LogLevel
		{
			get
			{
				return Log.logLevel;
			}
			set
			{
				Log.logLevel = value;
			}
		}

		public static void Debug(string tag, string message)
		{
			Log.DoLog(LogLevel.DEBUG, tag, message);
		}

		public static void Info(string tag, string message)
		{
			Log.DoLog(LogLevel.INFO, tag, message);
		}

		public static void Warn(string tag, string message)
		{
			Log.DoLog(LogLevel.WARN, tag, message);
		}

		public static void Error(string tag, string message)
		{
			Log.DoLog(LogLevel.ERROR, tag, message);
		}

		public static void Fatal(string tag, string message)
		{
			Log.DoLog(LogLevel.FATAL, tag, message);
		}

		private static void DoLog(LogLevel logLevel, string tag, string msg)
		{
			if (logLevel >= Log.LogLevel)
			{
				msg = string.Concat(new object[]
				{
					DateTime.Now,
					" Omniata-",
					tag,
					"-",
					logLevel,
					": ",
					msg
				});
				if (logLevel == LogLevel.DEBUG || logLevel == LogLevel.INFO)
				{
					UnityEngine.Debug.Log(msg);
				}
				else if (logLevel == LogLevel.WARN)
				{
					UnityEngine.Debug.LogWarning(msg);
				}
				else
				{
					UnityEngine.Debug.LogError(msg);
				}
			}
		}
	}
}
