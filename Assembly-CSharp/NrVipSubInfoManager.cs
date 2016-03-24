using System;
using System.Collections.Generic;

public class NrVipSubInfoManager : NrTSingleton<NrVipSubInfoManager>
{
	private Dictionary<byte, VipSubInfo> m_dicVipSubInfo = new Dictionary<byte, VipSubInfo>();

	private NrVipSubInfoManager()
	{
	}

	public void Set_Value(VipSubInfo a_cValue)
	{
		if (this.m_dicVipSubInfo.ContainsKey(a_cValue.byVipLevel))
		{
			TsLog.LogError("VipSubInfo Duplicate Data Level={0}!!!!!!", new object[]
			{
				a_cValue.byVipLevel
			});
			this.m_dicVipSubInfo.Remove(a_cValue.byVipLevel);
		}
		this.m_dicVipSubInfo.Add(a_cValue.byVipLevel, a_cValue);
	}

	public VipSubInfo Get_VipSubInfo(byte VipLevel)
	{
		if (this.m_dicVipSubInfo.ContainsKey(VipLevel))
		{
			return this.m_dicVipSubInfo[VipLevel];
		}
		return null;
	}

	public string GetIconPath(byte VipLevel)
	{
		if (this.m_dicVipSubInfo.ContainsKey(VipLevel))
		{
			return this.m_dicVipSubInfo[VipLevel].strIconPath;
		}
		return string.Empty;
	}

	public int GetSize()
	{
		return this.m_dicVipSubInfo.Count;
	}
}
