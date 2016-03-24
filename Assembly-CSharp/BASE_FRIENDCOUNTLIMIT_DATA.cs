using System;
using System.Collections.Generic;
using TsLibs;

public class BASE_FRIENDCOUNTLIMIT_DATA : NrTableBase
{
	public static List<FRIENDCOUNTLIMIT_DATA> m_listFriendCountLimitData = new List<FRIENDCOUNTLIMIT_DATA>();

	public BASE_FRIENDCOUNTLIMIT_DATA() : base(CDefinePath.FriendCountLimitDataURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			FRIENDCOUNTLIMIT_DATA fRIENDCOUNTLIMIT_DATA = new FRIENDCOUNTLIMIT_DATA();
			fRIENDCOUNTLIMIT_DATA.SetData(data);
			BASE_FRIENDCOUNTLIMIT_DATA.m_listFriendCountLimitData.Add(fRIENDCOUNTLIMIT_DATA);
		}
		return true;
	}

	public static int GetLimitFriendCount(short level)
	{
		if (level <= 0)
		{
			return 0;
		}
		foreach (FRIENDCOUNTLIMIT_DATA current in BASE_FRIENDCOUNTLIMIT_DATA.m_listFriendCountLimitData)
		{
			if (current.Level_Min <= level && current.Level_Max >= level)
			{
				return current.FriendLimitCount;
			}
		}
		return 30;
	}

	public static FRIENDCOUNTLIMIT_DATA GetFirneCountLimitInfo(short level)
	{
		if (level <= 0)
		{
			return null;
		}
		foreach (FRIENDCOUNTLIMIT_DATA current in BASE_FRIENDCOUNTLIMIT_DATA.m_listFriendCountLimitData)
		{
			if (current.Level_Min <= level && current.Level_Max >= level)
			{
				return current;
			}
		}
		return null;
	}
}
