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
					string text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
					string path = Logger.logPath + "/NPatchLog.txt";
					StreamWriter streamWriter = new StreamWriter(path, true, Encoding.UTF8);
					streamWriter.WriteLine(string.Concat(new string[]
					{
						text,
						"\t",
						errorInfo,
						"\t",
						log,
						"\t"
					}));
					streamWriter.Close();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}
	}
}
