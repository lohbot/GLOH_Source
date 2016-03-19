using System;
using System.Collections.Generic;

public class NkCharAniMapInfo
{
	public List<int> m_kCharKindList;

	public NkCharAniMapInfo()
	{
		this.m_kCharKindList = new List<int>();
		this.m_kCharKindList.Clear();
	}

	public void AddCharKind(int charkind)
	{
		if (!this.m_kCharKindList.Contains(charkind))
		{
			this.m_kCharKindList.Add(charkind);
		}
	}

	public int GetListCount()
	{
		return this.m_kCharKindList.Count;
	}

	public List<int> GetCharKindList()
	{
		return this.m_kCharKindList;
	}
}
