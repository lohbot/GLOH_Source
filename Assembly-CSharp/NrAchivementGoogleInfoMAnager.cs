using System;
using System.Collections.Generic;

public class NrAchivementGoogleInfoMAnager : NrTSingleton<NrAchivementGoogleInfoMAnager>
{
	private Dictionary<int, Achivement_GoogleData> m_AchivementInfoList;

	private NrAchivementGoogleInfoMAnager()
	{
		this.m_AchivementInfoList = new Dictionary<int, Achivement_GoogleData>();
	}

	public void SetData(Achivement_GoogleData info)
	{
		if (info != null)
		{
			this.m_AchivementInfoList.Add(info.m_nIdx, info);
		}
	}

	public Achivement_GoogleData GetData(int index)
	{
		if (this.m_AchivementInfoList.ContainsKey(index))
		{
			return this.m_AchivementInfoList[index];
		}
		return null;
	}

	public string GetCode(int index)
	{
		if (this.m_AchivementInfoList.ContainsKey(index))
		{
			return this.m_AchivementInfoList[index].m_strCode;
		}
		return string.Empty;
	}
}
