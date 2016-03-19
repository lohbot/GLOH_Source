using System;
using UnityEngine;

public class NkNPCWideCollAreaInfo
{
	public int nClientID;

	public Rect kWideCollArea = new Rect(0f, 0f, 0f, 0f);

	public void SetArea(float left, float top, float width, float height)
	{
		this.kWideCollArea.Set(left, top, width, height);
	}

	public bool IsArea(Vector2 pos)
	{
		return this.kWideCollArea.Contains(pos);
	}
}
