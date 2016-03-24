using System;
using TsLibs;

public class Achivement_GoogleData : NrTableData
{
	public int m_nIdx;

	public string m_strCode = string.Empty;

	public Achivement_GoogleData()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nIdx = 0;
		this.m_strCode = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nIdx);
		row.GetColumn(num++, out this.m_strCode);
	}
}
