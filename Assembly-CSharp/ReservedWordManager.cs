using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ReservedWordManager : NrTSingleton<ReservedWordManager>
{
	private Dictionary<string, string> m_mapReservedWord = new Dictionary<string, string>();

	private bool m_bUse = true;

	private ReservedWordManager()
	{
		if (!PlayerPrefs.HasKey(NrPrefsKey.RESERVED_WORD))
		{
			this.m_bUse = true;
		}
		else if (PlayerPrefs.GetInt(NrPrefsKey.RESERVED_WORD) == 0)
		{
			this.m_bUse = false;
		}
		else if (PlayerPrefs.GetInt(NrPrefsKey.RESERVED_WORD) == 1)
		{
			this.m_bUse = true;
		}
	}

	public void SetUse(bool flag)
	{
		this.m_bUse = flag;
	}

	public bool IsUse()
	{
		return this.m_bUse;
	}

	public void AddReservedWord(ReservedWord word)
	{
		string text = word.text.ToLower();
		if (!this.m_mapReservedWord.ContainsKey(text))
		{
			this.m_mapReservedWord.Add(text, text);
		}
	}

	public string ReplaceWord(string text)
	{
		if (text.Contains("class") || text.Contains("password"))
		{
			return text;
		}
		string text2 = text.ToLower();
		foreach (string current in this.m_mapReservedWord.Keys)
		{
			if (text2.Contains(current))
			{
				int length = current.Length;
				string text3 = string.Empty;
				for (int i = 0; i < length; i++)
				{
					text3 += "*";
				}
				text = Regex.Replace(text, current, text3, RegexOptions.IgnoreCase);
			}
		}
		return text;
	}
}
