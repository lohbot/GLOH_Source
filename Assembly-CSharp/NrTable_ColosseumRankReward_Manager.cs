using System;
using System.Collections.Generic;

public class NrTable_ColosseumRankReward_Manager : NrTSingleton<NrTable_ColosseumRankReward_Manager>
{
	private SortedDictionary<short, List<COLOSSEUM_RANK_REWARD>> m_sdGradeRankReward;

	private SortedDictionary<short, COLOSSEUM_GRADEINFO> m_sdGradeInfoList;

	private NrTable_ColosseumRankReward_Manager()
	{
		this.m_sdGradeRankReward = new SortedDictionary<short, List<COLOSSEUM_RANK_REWARD>>();
		this.m_sdGradeInfoList = new SortedDictionary<short, COLOSSEUM_GRADEINFO>();
	}

	public void AddRewardInfo(COLOSSEUM_RANK_REWARD _value)
	{
		if (!this.m_sdGradeRankReward.ContainsKey(_value.m_nGrade))
		{
			List<COLOSSEUM_RANK_REWARD> value = new List<COLOSSEUM_RANK_REWARD>();
			this.m_sdGradeRankReward.Add(_value.m_nGrade, value);
		}
		this.m_sdGradeRankReward[_value.m_nGrade].Add(_value);
		this.m_sdGradeRankReward[_value.m_nGrade].Sort(new Comparison<COLOSSEUM_RANK_REWARD>(NrTable_ColosseumRankReward_Manager.CompareLevel));
	}

	private static int CompareLevel(COLOSSEUM_RANK_REWARD x, COLOSSEUM_RANK_REWARD y)
	{
		if (x.m_nRank_Min < y.m_nRank_Min)
		{
			return -1;
		}
		return 1;
	}

	public List<COLOSSEUM_RANK_REWARD> Get_RewarList(short grade)
	{
		if (!this.m_sdGradeInfoList.ContainsKey(grade))
		{
			return null;
		}
		return this.m_sdGradeRankReward[grade];
	}

	public void AddGradeInfo(COLOSSEUM_GRADEINFO _value)
	{
		if (this.m_sdGradeInfoList.ContainsKey(_value.m_nGrade))
		{
			return;
		}
		this.m_sdGradeInfoList.Add(_value.m_nGrade, _value);
	}

	public COLOSSEUM_RANK_REWARD GetDataRank(short nColosseumMyGrade, int Rank)
	{
		if (!this.m_sdGradeRankReward.ContainsKey(nColosseumMyGrade))
		{
			return null;
		}
		List<COLOSSEUM_RANK_REWARD> list = this.m_sdGradeRankReward[nColosseumMyGrade];
		foreach (COLOSSEUM_RANK_REWARD current in list)
		{
			if (current.m_nRank_Min <= Rank && current.m_nRank_Max >= Rank)
			{
				return current;
			}
		}
		return null;
	}

	public string GetGradeTexture(short iGrade)
	{
		string empty = string.Empty;
		foreach (COLOSSEUM_GRADEINFO current in this.m_sdGradeInfoList.Values)
		{
			if (iGrade == current.m_nGrade)
			{
				return current.m_GradeIcon_ImageKey;
			}
		}
		return empty;
	}

	public string GetGradeTextKey(short iGrade)
	{
		string empty = string.Empty;
		foreach (COLOSSEUM_GRADEINFO current in this.m_sdGradeInfoList.Values)
		{
			if (iGrade == current.m_nGrade)
			{
				return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(current.m_Textkey_InterFace);
			}
		}
		return empty;
	}
}
