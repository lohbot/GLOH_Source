using System;
using UnityEngine;

public class GateData
{
	public int m_NextMapIdx;

	public Vector2 m_vPos = Vector2.zero;

	public GateData()
	{
		this.m_NextMapIdx = 0;
		this.m_vPos = Vector2.zero;
	}

	public GateData(int nNextMapIdx, Vector2 vPos)
	{
		this.m_NextMapIdx = nNextMapIdx;
		this.m_vPos = vPos;
	}
}
