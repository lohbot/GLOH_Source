using System;
using TsLibs;

public class ITEM_BREAK : NrTableData
{
	public int i32GroupUnique;

	public int i32ItemMakeRank;

	public int i32ItemUnique;

	public int i32ItemNum_Min;

	public int i32ItemNum_Max;

	public int i32ProbSpecial;

	public int i32SpecialItemUnique;

	public int i32SpecialItemNum_Min;

	public int i32SpecialItemNum_Max;

	public int i32Skill_increase_itemnum;

	public int i32Skill_increase_ProbSpecial;

	public ITEM_BREAK()
	{
		this.Init();
	}

	public void Init()
	{
		this.i32GroupUnique = 0;
		this.i32ItemMakeRank = 0;
		this.i32ItemUnique = 0;
		this.i32ItemNum_Min = 0;
		this.i32ItemNum_Max = 0;
		this.i32ProbSpecial = 0;
		this.i32SpecialItemUnique = 0;
		this.i32SpecialItemNum_Min = 0;
		this.i32SpecialItemNum_Max = 0;
		this.i32Skill_increase_itemnum = 0;
		this.i32Skill_increase_ProbSpecial = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.i32GroupUnique);
		row.GetColumn(num++, out this.i32ItemMakeRank);
		row.GetColumn(num++, out this.i32ItemUnique);
		row.GetColumn(num++, out this.i32ItemNum_Min);
		row.GetColumn(num++, out this.i32ItemNum_Max);
		row.GetColumn(num++, out this.i32ProbSpecial);
		row.GetColumn(num++, out this.i32SpecialItemUnique);
		row.GetColumn(num++, out this.i32SpecialItemNum_Min);
		row.GetColumn(num++, out this.i32SpecialItemNum_Max);
		row.GetColumn(num++, out this.i32Skill_increase_itemnum);
		row.GetColumn(num++, out this.i32Skill_increase_ProbSpecial);
	}
}
