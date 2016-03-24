using System;
using TsLibs;

public class GROUP_SOL_TICKET : NrTableData
{
	public long i64GroupUnique;

	public string strCHARCODE = string.Empty;

	public byte i8Grade;

	public GROUP_SOL_TICKET() : base(NrTableData.eResourceType.eRT_ITEM_GROUP_SOL_TICKET)
	{
		this.Init();
	}

	public void Init()
	{
		this.i64GroupUnique = 0L;
		this.strCHARCODE = string.Empty;
		this.i8Grade = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(1, out this.i64GroupUnique);
		row.GetColumn(2, out this.strCHARCODE);
		row.GetColumn(3, out this.i8Grade);
	}
}
