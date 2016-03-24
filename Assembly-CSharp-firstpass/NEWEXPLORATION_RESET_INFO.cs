using System;
using TsLibs;

public class NEWEXPLORATION_RESET_INFO
{
	public short i16CountIndex;

	public int i32ItemUnique;

	public int i32ItemNum;

	public NEWEXPLORATION_RESET_INFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.i16CountIndex = 0;
		this.i32ItemUnique = 0;
		this.i32ItemNum = 0;
	}

	public void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.i16CountIndex);
		row.GetColumn(num++, out this.i32ItemUnique);
		row.GetColumn(num++, out this.i32ItemNum);
	}
}
