using GAME;
using System;
using TsLibs;

public class EXPEDITION_CONSTANT : NrTableData
{
	public eEXPEDITION_CONSTANT m_eConstant = eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_MAX;

	public int m_nConstant;

	public string strConstant = string.Empty;

	public void Init()
	{
		this.m_eConstant = eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_MAX;
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
