using System;

public class AgitNPCInfo
{
	private int m_i32CharID;

	private byte m_iNPCType;

	public void SetCharID(int i32CharID)
	{
		this.m_i32CharID = i32CharID;
	}

	public int GetCharID()
	{
		return this.m_i32CharID;
	}

	public void SetNPCType(byte iNPCType)
	{
		this.m_iNPCType = iNPCType;
	}

	public byte GetNPCType()
	{
		return this.m_iNPCType;
	}
}
