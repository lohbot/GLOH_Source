using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NrDebugConsole : NrTSingleton<NrDebugConsole>
{
	private bool m_bActive;

	private bool m_bDownCtrl;

	private string m_buff = string.Empty;

	private List<string> m_arCommand;

	private List<string> m_arMsg;

	private NrCommandInterpreter m_kCommandInterpreter;

	private short m_si16CommandLine;

	private NrDebugConsole()
	{
		this.m_arMsg = new List<string>();
		this.m_arCommand = new List<string>();
		this.m_kCommandInterpreter = new NrCommandInterpreter();
		this.Print("-- set unity debug mode.\n");
	}

	public void Update()
	{
		if (NkInputManager.GetKeyDown(KeyCode.LeftControl))
		{
			this.m_bDownCtrl = true;
		}
		if (NkInputManager.GetKeyUp(KeyCode.LeftControl))
		{
			this.m_bDownCtrl = false;
		}
		if (this.m_bDownCtrl && NkInputManager.GetKeyUp(KeyCode.LeftAlt))
		{
			this.m_bActive = !this.m_bActive;
			if (this.m_bActive)
			{
				NrTSingleton<FormsManager>.Instance.HideAll();
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.ShowAll();
			}
		}
		if (!this.m_bActive)
		{
			return;
		}
		if (NkInputManager.GetKeyDown(KeyCode.UpArrow))
		{
			this.m_si16CommandLine -= 1;
			if (this.m_si16CommandLine <= 0)
			{
				this.m_si16CommandLine = 0;
			}
			this.m_buff = this.m_arCommand[(int)this.m_si16CommandLine];
		}
		if (NkInputManager.GetKeyDown(KeyCode.DownArrow))
		{
			this.m_si16CommandLine += 1;
			if ((int)this.m_si16CommandLine >= this.m_arCommand.Count)
			{
				this.m_si16CommandLine = (short)this.m_arCommand.Count;
				this.m_buff = string.Empty;
			}
			else
			{
				this.m_buff = this.m_arCommand[(int)this.m_si16CommandLine];
			}
		}
		if (NkInputManager.GetKeyDown(KeyCode.Return) || NkInputManager.GetKeyDown(KeyCode.A))
		{
			this.m_kCommandInterpreter.ParseCommand(this.m_buff);
			this.m_arCommand.Add(this.m_buff.Trim());
			this.m_si16CommandLine = (short)this.m_arCommand.Count;
			this.m_buff = string.Empty;
		}
	}

	public void Print(string format, params object[] args)
	{
		this.Print(string.Format(format, args));
	}

	public void Print(string str)
	{
		TsPlatform.FileLog(str);
	}

	public void ClearScreen()
	{
		this.m_arMsg.Clear();
	}

	public bool IsActive()
	{
		return this.m_bActive;
	}
}
