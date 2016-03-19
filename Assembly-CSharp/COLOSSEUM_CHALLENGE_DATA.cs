using System;
using System.Collections.Generic;
using TsLibs;

public class COLOSSEUM_CHALLENGE_DATA : NrTableBase
{
	public static Dictionary<int, BASE_COLOSSEUM_CHALLENGE_DATA> m_dicColosseumChallengeData = new Dictionary<int, BASE_COLOSSEUM_CHALLENGE_DATA>();

	public COLOSSEUM_CHALLENGE_DATA() : base(CDefinePath.COLOSSEUM_CHALLENGE_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BASE_COLOSSEUM_CHALLENGE_DATA bASE_COLOSSEUM_CHALLENGE_DATA = new BASE_COLOSSEUM_CHALLENGE_DATA();
			bASE_COLOSSEUM_CHALLENGE_DATA.SetData(data);
			if (COLOSSEUM_CHALLENGE_DATA.m_dicColosseumChallengeData.ContainsKey(bASE_COLOSSEUM_CHALLENGE_DATA.m_i32Index))
			{
				return false;
			}
			COLOSSEUM_CHALLENGE_DATA.m_dicColosseumChallengeData.Add(bASE_COLOSSEUM_CHALLENGE_DATA.m_i32Index, bASE_COLOSSEUM_CHALLENGE_DATA);
		}
		return true;
	}

	public static BASE_COLOSSEUM_CHALLENGE_DATA GetColosseumChallengeData(int index)
	{
		if (COLOSSEUM_CHALLENGE_DATA.m_dicColosseumChallengeData.ContainsKey(index))
		{
			return COLOSSEUM_CHALLENGE_DATA.m_dicColosseumChallengeData[index];
		}
		return null;
	}

	public static int GetTotalCount()
	{
		return COLOSSEUM_CHALLENGE_DATA.m_dicColosseumChallengeData.Count;
	}
}
