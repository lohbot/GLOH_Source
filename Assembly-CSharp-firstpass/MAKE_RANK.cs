using System;
using TsLibs;

public class MAKE_RANK : NrTableData
{
	public byte m_byRank;

	public short m_shAblility;

	public string stRank = string.Empty;

	public MAKE_RANK()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_byRank = 0;
		this.m_shAblility = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(0, out this.stRank);
		row.GetColumn(3, out this.m_shAblility);
	}
}
