using System;
using TsLibs;

public class NEWEXPLORATION_TREASURE
{
	public short i16Index;

	public sbyte bFloor;

	public sbyte bSubFloor;

	public int[] i32ItemUnique = new int[3];

	public int[] i32ItemNum = new int[3];

	public NEWEXPLORATION_TREASURE()
	{
		this.Init();
	}

	public void Init()
	{
		this.i16Index = 0;
		this.bFloor = 0;
		this.bSubFloor = 0;
		Array.Clear(this.i32ItemUnique, 0, this.i32ItemUnique.Length);
		Array.Clear(this.i32ItemNum, 0, this.i32ItemNum.Length);
	}

	public void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.i16Index);
		row.GetColumn(num++, out this.bFloor);
		row.GetColumn(num++, out this.bSubFloor);
		for (int i = 0; i < 3; i++)
		{
			row.GetColumn(num++, out this.i32ItemUnique[i]);
			row.GetColumn(num++, out this.i32ItemNum[i]);
		}
	}
}
