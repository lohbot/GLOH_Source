using System;
using System.Collections.Generic;

public class NrDebugLoger
{
	private List<string> m_arLog = new List<string>();

	public void Log(string strLog)
	{
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			this.m_arLog.Add(DateTime.Now.ToString("hh:mm:ss") + " " + strLog);
		}
	}

	public void PrintLogs()
	{
		NrTSingleton<NrDebugConsole>.Instance.Print(string.Empty);
		foreach (string current in this.m_arLog)
		{
			NrTSingleton<NrDebugConsole>.Instance.Print(current);
		}
	}
}
