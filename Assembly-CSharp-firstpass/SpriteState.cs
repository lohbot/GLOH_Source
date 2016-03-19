using System;
using UnityEngine;

[Serializable]
public class SpriteState
{
	public string name;

	[HideInInspector]
	public string imgPath;

	[HideInInspector]
	public CSpriteFrame frameInfo;

	public SpriteState(string n, string p)
	{
		this.name = n;
		this.imgPath = p;
	}
}
