using System;
using System.Collections.Generic;
using UnityEngine;

public class GxRP
{
	private const int MAX_LINK = 4;

	private short m_sRPIndex = -1;

	private short m_sMapIndex;

	private Vector3 m_v3Pos = Vector3.zero;

	private Vector2 m_v2Pos = Vector2.zero;

	private List<short> m_sLinkedRP = new List<short>();

	private int m_sLinkedCount;

	public void SetData(short _sRPIndex, short _sMapIndex, Vector3 _MapPos)
	{
		this.m_sRPIndex = _sRPIndex;
		this.m_sMapIndex = _sMapIndex;
		this.m_v3Pos = _MapPos;
		this.m_v2Pos = new Vector2(_MapPos.x, _MapPos.z);
	}

	public void SetData(short _sRPIndex, short _sMapIndex, short _X, short _Y)
	{
		this.m_sRPIndex = _sRPIndex;
		this.m_sMapIndex = _sMapIndex;
		this.m_v2Pos = new Vector2((float)_X, (float)_Y);
	}

	public void SetData(ROAD_POINT kRoadPoint)
	{
		this.m_sRPIndex = kRoadPoint.ROADPOINT_IDX;
		this.m_sMapIndex = kRoadPoint.MAP_IDX;
		this.m_v3Pos = new Vector3(kRoadPoint.POSX, kRoadPoint.POSY, kRoadPoint.POSZ);
		this.m_v2Pos = new Vector2(kRoadPoint.POSX, kRoadPoint.POSZ);
		for (int i = 0; i < 4; i++)
		{
			this.AddLinkedRP(kRoadPoint.LINK_RPIDX[i]);
		}
	}

	public void ClearData()
	{
		this.m_sRPIndex = -1;
		this.m_sMapIndex = -1;
		this.m_sLinkedCount = 0;
		this.m_v3Pos = Vector3.zero;
		this.m_v2Pos = Vector2.zero;
		this.m_sLinkedRP.Clear();
	}

	public bool AddLinkedRP(short _sRPIndex)
	{
		if (this.IsLinked(_sRPIndex))
		{
			return true;
		}
		if (this.m_sLinkedCount < 4 && _sRPIndex != 0)
		{
			this.m_sLinkedRP.Add(_sRPIndex);
			this.m_sLinkedCount++;
		}
		return false;
	}

	public void DelLinkedRP(short sRPIndex)
	{
		for (int i = 0; i < this.m_sLinkedRP.Count; i++)
		{
			if (this.m_sLinkedRP[i] == sRPIndex)
			{
				this.m_sLinkedRP.RemoveAt(i);
				this.m_sLinkedRP.Sort();
				this.m_sLinkedCount--;
				return;
			}
		}
	}

	public bool IsLinked(short sRPIndex)
	{
		for (int i = 0; i < this.m_sLinkedRP.Count; i++)
		{
			if (this.m_sLinkedRP[i] == sRPIndex)
			{
				return true;
			}
		}
		return false;
	}

	public short GetIndex()
	{
		return this.m_sRPIndex;
	}

	public short GetMapIndex()
	{
		return this.m_sMapIndex;
	}

	public short GetX()
	{
		return (short)this.m_v2Pos.x;
	}

	public short GetY()
	{
		return (short)this.m_v2Pos.y;
	}

	public Vector3 GetPos()
	{
		return this.m_v3Pos;
	}

	public Vector2 GetPos2()
	{
		return this.m_v2Pos;
	}

	public bool IsEmpty()
	{
		return this.m_sRPIndex == -1;
	}

	public int GetLinkedCount()
	{
		return this.m_sLinkedCount;
	}

	public short GetLinkedRP(int n)
	{
		if (this.m_sLinkedRP.Count <= n || n >= 4)
		{
			return 0;
		}
		return this.m_sLinkedRP[n];
	}

	public int CalcDistance(short x, short y)
	{
		return (int)Math.Sqrt(Math.Pow((double)((long)((float)x - this.m_v2Pos.x)), 2.0) + (double)((long)Math.Pow((double)((long)((float)y - this.m_v2Pos.y)), 2.0)));
	}

	public int CalcCost(short x, short y)
	{
		return Math.Abs((int)((short)this.m_v2Pos.x - x)) + Math.Abs((int)((short)this.m_v2Pos.y - y));
	}
}
