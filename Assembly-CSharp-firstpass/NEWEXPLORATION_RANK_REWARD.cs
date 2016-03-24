using System;
using TsLibs;

public class NEWEXPLORATION_RANK_REWARD
{
	public short i16Index;

	public byte bRankType;

	public short i16Min_Rank_Rate;

	public short i16MAX_Rank_Rate;

	public int[] i32ItemUnique = new int[3];

	public int[] i32ItemNum = new int[3];

	public NEWEXPLORATION_RANK_REWARD()
	{
		this.Init();
	}

	public void Init()
	{
		this.i16Index = 0;
		this.bRankType = 0;
		this.i16Min_Rank_Rate = 0;
		this.i16MAX_Rank_Rate = 0;
		Array.Clear(this.i32ItemUnique, 0, this.i32ItemUnique.Length);
		Array.Clear(this.i32ItemNum, 0, this.i32ItemNum.Length);
	}

	public void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.i16Index);
		row.GetColumn(num++, out this.bRankType);
		row.GetColumn(num++, out this.i16Min_Rank_Rate);
		row.GetColumn(num++, out this.i16MAX_Rank_Rate);
		row.GetColumn(num++, out this.i32ItemUnique[0]);
		row.GetColumn(num++, out this.i32ItemNum[0]);
		row.GetColumn(num++, out this.i32ItemUnique[1]);
		row.GetColumn(num++, out this.i32ItemNum[1]);
		row.GetColumn(num++, out this.i32ItemUnique[2]);
		row.GetColumn(num++, out this.i32ItemNum[2]);
	}
}
