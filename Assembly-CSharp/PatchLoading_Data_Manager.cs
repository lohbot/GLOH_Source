using System;
using System.Collections.Generic;
using UnityEngine;

public class PatchLoading_Data_Manager : NrTSingleton<PatchLoading_Data_Manager>
{
	private Dictionary<int, PatchLoading_Data> m_PatchLoading_Datas;

	private PatchLoading_Data_Manager()
	{
		this.m_PatchLoading_Datas = new Dictionary<int, PatchLoading_Data>();
	}

	public void Add(PatchLoading_Data Data)
	{
		if (!this.m_PatchLoading_Datas.ContainsKey(Data.id))
		{
			this.m_PatchLoading_Datas.Add(Data.id, Data);
		}
		else
		{
			Debug.LogError("Duplicate Data id=" + Data.id);
		}
	}

	public PatchLoading_Data GetData(int id)
	{
		if (this.m_PatchLoading_Datas.ContainsKey(id))
		{
			return this.m_PatchLoading_Datas[id];
		}
		return null;
	}
}
