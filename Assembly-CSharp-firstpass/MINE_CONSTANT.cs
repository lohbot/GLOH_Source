using GAME;
using System;
using TsLibs;

public class MINE_CONSTANT : NrTableData
{
	public eMINE_CONSTANT m_eConstant = eMINE_CONSTANT.eMINE_CONSTANT_MAX;

	public int m_nConstant;

	public string strConstant = string.Empty;

	public void Init()
	{
		this.m_eConstant = eMINE_CONSTANT.eMINE_CONSTANT_MAX;
		this.m_nConstant = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.strConstant);
		row.GetColumn(num++, out this.m_nConstant);
	}
}
