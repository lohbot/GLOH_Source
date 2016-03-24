using System;

public class GROUPTICKET_INFO
{
	public string m_strSolKind = string.Empty;

	public byte m_i8Grade;

	public GROUPTICKET_INFO()
	{
		this.Init();
	}

	private void Init()
	{
		this.m_strSolKind = string.Empty;
		this.m_i8Grade = 0;
	}
}
