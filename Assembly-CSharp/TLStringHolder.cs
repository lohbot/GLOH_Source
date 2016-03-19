using System;
using UnityEngine;

public class TLStringHolder : ScriptableObject
{
	public string[] content;

	public TLStringHolder(string[] content)
	{
		this.content = content;
	}

	public TLStringHolder()
	{
	}

	public void Attach(string[] content)
	{
		this.content = content;
	}
}
