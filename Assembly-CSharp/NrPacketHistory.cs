using System;
using System.Collections.Generic;

public class NrPacketHistory : NrTSingleton<NrPacketHistory>
{
	private ushort m_ui16UniqID;

	private List<NrPacketInfo> m_arPacketInfo;

	private NrPacketHistory()
	{
		this.m_arPacketInfo = new List<NrPacketInfo>();
	}

	public void AddPacketInfo(NrPacketInfo kPacketInfo)
	{
		if (this.m_arPacketInfo.Count >= 1000)
		{
			this.m_arPacketInfo.RemoveRange(0, 500);
		}
		kPacketInfo.m_ui16UniqID = this.m_ui16UniqID;
		this.m_arPacketInfo.Add(kPacketInfo);
		this.m_ui16UniqID += 1;
	}

	public List<NrPacketInfo> GetPacketInfoList()
	{
		return this.m_arPacketInfo;
	}

	public NrPacketInfo GetPacketInfo(ushort ui16ID)
	{
		foreach (NrPacketInfo current in this.m_arPacketInfo)
		{
			if (current.m_ui16UniqID == ui16ID)
			{
				return current;
			}
		}
		return null;
	}
}
