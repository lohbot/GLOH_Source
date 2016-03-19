using System;
using System.Collections.Generic;
using UnityEngine;

public class NrGridData
{
	private List<Vector2> m_Data = new List<Vector2>();

	private int[] m_ReturnData;

	public void InsertData(Vector2 index)
	{
		this.m_Data.Add(index);
	}

	public int[] GetIndex(Vector2 index, int xMax, int yMax)
	{
		if (this.m_ReturnData == null)
		{
			this.m_ReturnData = new int[this.m_Data.Count];
		}
		for (int i = 0; i < this.m_Data.Count; i++)
		{
			this.m_ReturnData[i] = NrGridData.Convert(index + this.m_Data[i], xMax, yMax);
		}
		return this.m_ReturnData;
	}

	public static Vector2 Convert(int index, int xMax)
	{
		Vector2 zero = Vector2.zero;
		zero.y = (float)(index / xMax);
		zero.x = (float)(index % xMax);
		return zero;
	}

	public static int Convert(Vector2 index, int xMax, int yMax)
	{
		if (index.x < 0f || index.x >= (float)xMax)
		{
			return xMax * yMax;
		}
		return (int)(index.x + index.y * (float)xMax);
	}

	public static bool IndexAccessAble(int index, int xMax, int yMax)
	{
		return 0 <= index && index < xMax * yMax;
	}
}
