using System;
using System.IO;
using System.Text;

namespace NPatch
{
	public static class Logger
	{
		private const string fileName = "NPatchLog.txt";

		public static string logPath;

		public static void WriteLog(string log, ERRORLEVEL errorLevel)
		{
			Logger.__wrtie_log(log, errorLevel.ToString());
		}

		public static void WriteLog(string log)
		{
			Logger.__wrtie_log(log, "INFO");
		}

		private static void __wrtie_log(string log, string errorInfo)
		{
			Type typeFromHandle = typeof(Logger);
			lock (typeFromHandle)
			{
				try
				{
					string text = DateTime.Now.ToString("yyyy/MM/dd H:mm:ss");
					string path = Path.Combine(Logger.logPath, "NPatchLog.txt");
					string s = string.Concat(new string[]
					{
						text,
						"\t",
						errorInfo,
						"\t",
						log,
						"\r\n"
					});
					byte[] bytes = Encoding.UTF8.GetBytes(s);
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(Logger.logPath);
					}
					using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
					{
						fileStream.Write(bytes, 0, bytes.Length);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}
	}
}
