using System;
using UnityEngine;

public class NrHolderStrings : ScriptableObject
{
	public string[] m_kContent;

	public NrHolderStrings()
	{
	}

	public NrHolderStrings(string[] arg)
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
