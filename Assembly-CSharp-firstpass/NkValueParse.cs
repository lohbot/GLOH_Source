using System;
using System.Collections.Generic;

public class NkValueParse<Type_>
{
	private Dictionary<string, Type_> m_kDicValue;

	public NkValueParse()
	{
		this.m_kDicValue = new Dictionary<string, Type_>();
	}

	~NkValueParse()
	{
	}

	public void InsertCodeValue(string strCode, Type_ tCodeValue)
	{
		string key = strCode.ToUpper();
		if (this.m_kDicValue.ContainsKey(key))
		{
			return;
		}
		this.m_kDicValue.Add(key, tCodeValue);
	}

	public Type_ GetValue(string strCode)
	{
		Type_ result = default(Type_);
		if (!this.m_kDicValue.TryGetValue(strCode.ToUpper(), out result))
		{
			return result;
		}
		return result;
	}

	public string[] GetKeys()
	{
		Dictionary<string, Type_>.KeyCollection keys = this.m_kDicValue.Keys;
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
