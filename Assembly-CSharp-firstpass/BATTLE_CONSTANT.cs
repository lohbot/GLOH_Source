using System;
using TsLibs;

public class BATTLE_CONSTANT : NrTableData
{
	public eBATTLE_CONSTANT m_eConstant = eBATTLE_CONSTANT.eBATTLE_CONSTANT_MAX;

	public float m_nConstant;

	public string strConstant = string.Empty;

	public void Init()
	{
		this.m_eConstant = eBATTLE_CONSTANT.eBATTLE_CONSTANT_MAX;
		this.m_nConstant = 0f;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.strConstant);
		row.GetColumn(num++, out this.m_nConstant);
	}
}
