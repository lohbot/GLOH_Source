using System;
using TsLibs;

public class DAILY_GIFT : NrTableData
{
	public short GroupUnique;

	public short DayCount;

	public short[] i16Lev = new short[11];

	public int[] i32ItemUnique = new int[11];

	public short[] i16ItemCount = new short[11];

	public string[] strBundleName = new string[11];

	public override void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.GroupUnique);
		row.GetColumn(num++, out this.DayCount);
		row.GetColumn(num++, out this.i16Lev[0]);
		row.GetColumn(num++, out this.i32ItemUnique[0]);
		row.GetColumn(num++, out this.i16ItemCount[0]);
		row.GetColumn(num++, out this.strBundleName[0]);
		row.GetColumn(num++, out this.i16Lev[1]);
		row.GetColumn(num++, out this.i32ItemUnique[1]);
		row.GetColumn(num++, out this.i16ItemCount[1]);
		row.GetColumn(num++, out this.strBundleName[1]);
		row.GetColumn(num++, out this.i16Lev[2]);
		row.GetColumn(num++, out this.i32ItemUnique[2]);
		row.GetColumn(num++, out this.i16ItemCount[2]);
		row.GetColumn(num++, out this.strBundleName[2]);
		row.GetColumn(num++, out this.i16Lev[3]);
		row.GetColumn(num++, out this.i32ItemUnique[3]);
		row.GetColumn(num++, out this.i16ItemCount[3]);
		row.GetColumn(num++, out this.strBundleName[3]);
		row.GetColumn(num++, out this.i16Lev[4]);
		row.GetColumn(num++, out this.i32ItemUnique[4]);
		row.GetColumn(num++, out this.i16ItemCount[4]);
		row.GetColumn(num++, out this.strBundleName[4]);
		row.GetColumn(num++, out this.i16Lev[5]);
		row.GetColumn(num++, out this.i32ItemUnique[5]);
		row.GetColumn(num++, out this.i16ItemCount[5]);
		row.GetColumn(num++, out this.strBundleName[5]);
		row.GetColumn(num++, out this.i16Lev[6]);
		row.GetColumn(num++, out this.i32ItemUnique[6]);
		row.GetColumn(num++, out this.i16ItemCount[6]);
		row.GetColumn(num++, out this.strBundleName[6]);
		row.GetColumn(num++, out this.i16Lev[7]);
		row.GetColumn(num++, out this.i32ItemUnique[7]);
		row.GetColumn(num++, out this.i16ItemCount[7]);
		row.GetColumn(num++, out this.strBundleName[7]);
		row.GetColumn(num++, out this.i16Lev[8]);
		row.GetColumn(num++, out this.i32ItemUnique[8]);
		row.GetColumn(num++, out this.i16ItemCount[8]);
		row.GetColumn(num++, out this.strBundleName[8]);
		row.GetColumn(num++, out this.i16Lev[9]);
		row.GetColumn(num++, out this.i32ItemUnique[9]);
		row.GetColumn(num++, out this.i16ItemCount[9]);
		row.GetColumn(num++, out this.strBundleName[9]);
		row.GetColumn(num++, out this.i16Lev[10]);
		row.GetColumn(num++, out this.i32ItemUnique[10]);
		row.GetColumn(num++, out this.i16ItemCount[10]);
		row.GetColumn(num++, out this.strBundleName[10]);
	}
}
