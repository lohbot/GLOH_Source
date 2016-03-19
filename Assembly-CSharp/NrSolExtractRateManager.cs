using System;
using System.Collections.Generic;

public class NrSolExtractRateManager : NrTSingleton<NrSolExtractRateManager>
{
	private List<EXTRACT_RATE> m_SolExtractRateDataList = new List<EXTRACT_RATE>();

	private NrSolExtractRateManager()
	{
		this.m_SolExtractRateDataList.Clear();
	}

	public EXTRACT_RATE[] GetSolExtractRateInfo()
	{
		return this.m_SolExtractRateDataList.ToArray();
	}

	public void AddData(EXTRACT_RATE data)
	{
		this.m_SolExtractRateDataList.Add(data);
	}

	public int GetSolExtractRateItemInfo(int SolSeason, int SolGrade, bool bHeartsUse)
	{
		int result = 0;
		int num = SolSeason + 1;
		int num2 = SolGrade + 1;
		for (int i = 0; i < this.m_SolExtractRateDataList.Count; i++)
		{
			EXTRACT_RATE eXTRACT_RATE = this.m_SolExtractRateDataList[i];
			if ((int)eXTRACT_RATE.Season == num && ((int)eXTRACT_RATE.Grade == num2 || (num2 > 7 && eXTRACT_RATE.Grade == 7)))
			{
				if (bHeartsUse)
				{
					result = eXTRACT_RATE.i32ExtrateHeartsRate;
				}
				else
				{
					result = eXTRACT_RATE.i32ExtrateRate;
				}
			}
		}
		return result;
	}
}
