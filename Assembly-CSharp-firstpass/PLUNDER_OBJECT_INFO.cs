using System;
using TsLibs;

public class PLUNDER_OBJECT_INFO : NrTableData
{
	public byte nObjectID;

	public string szObjectKindCode;

	public int nObject_Kind;

	public int nNeedLevel;

	public long nSpendGold;

	public PLUNDER_OBJECT_INFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.nObjectID = 0;
		this.szObjectKindCode = string.Empty;
		this.nObject_Kind = 0;
		this.nNeedLevel = 0;
		this.nSpendGold = 0L;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.nObjectID);
		row.GetColumn(num++, out this.szObjectKindCode);
		row.GetColumn(num++, out this.nNeedLevel);
		row.GetColumn(num++, out this.nSpendGold);
	}
}
