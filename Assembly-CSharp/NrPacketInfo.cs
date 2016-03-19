using System;
using System.Collections.Generic;

public class NrPacketInfo
{
	public string m_strPacketName;

	public ushort m_ui16UniqID;

	public string m_strTime;

	public List<string> m_arContents;

	public NrPacketInfo()
	{
		string text = string.Empty;
		text = DateTime.Now.Hour.ToString() + ":";
		text = text + DateTime.Now.Minute.ToString() + ":";
		text += DateTime.Now.Second.ToString();
		this.m_strTime = text;
		this.m_arContents = new List<string>();
	}

	public void SetPacketName(string str)
	{
		this.m_strPacketName = str;
	}

	public void AddContent(string str)
	{
		this.m_arContents.Add(str);
	}
}
