using System;
using System.Collections.Generic;
using UnityEngine;

public struct SpriteChar
{
	public int id;

	public Rect UVs;

	public float xOffset;

	public float yOffset;

	public float xAdvance;

	public Dictionary<char, float> kernings;

	public Dictionary<char, float> origKernings;

	public int page;

	public int chnl;

	public float layer;

	public float GetKerning(char prevChar)
	{
		if (this.kernings == null)
		{
			return 0f;
		}
		float result = 0f;
		this.kernings.TryGetValue(prevChar, out result);
		return result;
	}
}
