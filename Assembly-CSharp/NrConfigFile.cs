using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NrConfigFile : NrTSingleton<NrConfigFile>
{
	public enum eKey
	{
		LoginID,
		LoginPW,
		screenHeight,
		screenWidth,
		cache,
		SFX,
		BGM,
		SFX_CHECK,
		BGM_CHECK,
		END
	}

	private Dictionary<string, string> m_arData;

	private bool m_bLoadedAll;

	private NrConfigFile()
	{
		this.m_arData = new Dictionary<string, string>();
	}

	public void SetData(string strKey, string strValue)
	{
		if (this.m_arData.ContainsKey(strKey))
		{
			this.m_arData[strKey] = strValue;
			return;
		}
		this.m_arData.Add(strKey, strValue);
	}

	public string GetData(string strKey)
	{
		if (!this.m_arData.ContainsKey(strKey))
		{
			return string.Empty;
		}
		return this.m_arData[strKey];
	}

	public void SetLoadedAll()
	{
		this.m_bLoadedAll = true;
	}

	public bool IsLoadedAll()
	{
		return this.m_bLoadedAll;
	}

	public void LoadData()
	{
	}

	public void SaveData()
	{
	}

	private void SaveFromStream()
	{
		StreamWriter streamWriter = new StreamWriter("c:\\SPConfig.conf");
		for (NrConfigFile.eKey eKey = NrConfigFile.eKey.LoginID; eKey < NrConfigFile.eKey.END; eKey++)
		{
			streamWriter.WriteLine(eKey.ToString() + "=" + this.GetData(eKey.ToString()));
		}
		streamWriter.Close();
	}

	private void LoadFromStream()
	{
		try
		{
			StreamReader streamReader = new StreamReader("c:\\SPConfig.conf");
			if (streamReader == null)
			{
				return;
			}
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				string[] array = text.Split(new char[]
				{
					'='
				});
				if (array.Length == 2)
				{
					this.SetData(array[0], array[1]);
				}
			}
			streamReader.Close();
		}
		catch (FileNotFoundException)
		{
		}
		this.SetLoadedAll();
	}

	private void SaveFromCookie()
	{
		for (NrConfigFile.eKey eKey = NrConfigFile.eKey.LoginID; eKey < NrConfigFile.eKey.END; eKey++)
		{
			Application.ExternalCall("setCookie", new object[]
			{
				eKey.ToString(),
				this.GetData(eKey.ToString())
			});
		}
	}

	private void LoadFromCookie()
	{
		for (NrConfigFile.eKey eKey = NrConfigFile.eKey.LoginID; eKey < NrConfigFile.eKey.END; eKey++)
		{
			Application.ExternalCall("getCookie", new object[]
			{
				eKey.ToString()
			});
		}
		Application.ExternalCall("LoadedAllConfig", new object[0]);
	}
}
