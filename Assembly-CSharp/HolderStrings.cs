using System;
using UnityEngine;

public class HolderStrings : ScriptableObject
{
	public string[] m_kContent;

	public HolderStrings()
	{
	}

	public HolderStrings(string[] arg)
	{
		this.m_kContent = arg;
	}

	public void AttachContent(string[] arg)
	{
		this.m_kContent = arg;
	}

	public string[] GetContent()
	{
		return this.m_kContent;
	}
}
