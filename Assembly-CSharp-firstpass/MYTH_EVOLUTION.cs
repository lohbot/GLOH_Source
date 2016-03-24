using System;
using TsLibs;

public class MYTH_EVOLUTION : NrTableData
{
	public byte m_bSeason;

	public int m_i32SpendItemUnique1;

	public int m_i32SpendItemNum1;

	public int m_i32SpendItemUnique2;

	public int m_i32SpendItemNum2;

	public MYTH_EVOLUTION() : base(NrTableData.eResourceType.eRT_MYTH_EVOLUTION_SPEND)
	{
		this.Init();
	}

	public void Init()
	{
		this.m_bSeason = 0;
		this.m_i32SpendItemUnique1 = 0;
		this.m_i32SpendItemNum1 = 0;
		this.m_i32SpendItemUnique2 = 0;
		this.m_i32SpendItemNum2 = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_bSeason);
		row.GetColumn(num++, out this.m_i32SpendItemUnique1);
		row.GetColumn(num++, out this.m_i32SpendItemNum1);
		row.GetColumn(num++, out this.m_i32SpendItemUnique2);
		row.GetColumn(num++, out this.m_i32SpendItemNum2);
	}
}
