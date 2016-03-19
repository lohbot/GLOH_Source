using System;
using System.Collections.Generic;
using UnityEngine;

public class NrGateManager
{
	private Dictionary<int, List<GateData>> m_dGateList = new Dictionary<int, List<GateData>>();

	public void Add(int MapIdx, List<GateData> _List)
	{
		if (!this.m_dGateList.ContainsKey(MapIdx))
		{
			this.m_dGateList.Add(MapIdx, new List<GateData>());
		}
		this.m_dGateList[MapIdx].Clear();
		for (int i = 0; i < _List.Count; i++)
		{
			this.m_dGateList[MapIdx].Add(_List[i]);
		}
	}

	public void Add(int MapIdx, GateData _Gate)
	{
		if (!this.m_dGateList.ContainsKey(MapIdx))
		{
			this.m_dGateList.Add(MapIdx, new List<GateData>());
		}
		this.m_dGateList[MapIdx].Add(_Gate);
	}

	public Vector2 GetPos(int MapIdx, int NextMapIdx)
	{
		if (!this.m_dGateList.ContainsKey(MapIdx))
		{
			return Vector2.zero;
		}
		for (int i = 0; i < this.m_dGateList[MapIdx].Count; i++)
		{
			if (this.m_dGateList[MapIdx][i].m_NextMapIdx == NextMapIdx)
			{
				return this.m_dGateList[MapIdx][i].m_vPos;
			}
		}
		return Vector2.zero;
	}
}
