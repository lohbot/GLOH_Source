using System;
using System.Collections.Generic;

public class NkATBParse
{
	private Dictionary<string, long> m_kDicValue;

	public NkATBParse()
	{
		this.m_kDicValue = new Dictionary<string, long>();
	}

	~NkATBParse()
	{
	}

	public int GetCount()
	{
		return this.m_kDicValue.Count;
	}

	public void InsertCodeValue(string strCode, long nCodeValue)
	{
		string key = strCode.ToUpper();
		if (this.m_kDicValue.ContainsKey(key))
		{
			return;
		}
		this.m_kDicValue.Add(key, nCodeValue);
	}

	public long GetValue(string strCode)
	{
		long result = 0L;
		if (!this.m_kDicValue.TryGetValue(strCode.ToUpper(), out result))
		{
			return 0L;
		}
		return result;
	}

	public long ParseCode(string strCodeContents)
	{
		long num = 0L;
		if (string.IsNullOrEmpty(strCodeContents))
		{
			return 0L;
		}
		string text = string.Empty;
		int num2 = 0;
		int i;
		for (i = 0; i < strCodeContents.Length; i++)
		{
			char c = strCodeContents[i];
			if (c != ' ')
			{
				if (c == '+')
				{
					text = strCodeContents.Substring(num2, i - num2);
					num |= this.GetValue(text.Trim());
					num2 = i + 1;
				}
			}
		}
		if (i > num2 + 1)
		{
			text = strCodeContents.Substring(num2, i - num2);
			num |= this.GetValue(text.Trim());
		}
		return num;
	}

	public string[] GetString()
	{
		Dictionary<string, long>.KeyCollection keys = this.m_kDicValue.Keys;
		string[] array = new string[keys.Count];
		int num = 0;
		foreach (string current in keys)
		{
			array[num] = current;
			num++;
			if (num >= keys.Count)
			{
				break;
			}
		}
		return array;
	}
}
