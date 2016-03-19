using System;
using TsLibs;

public class ITEM_BOX_GROUP_DATA : NrTableData
{
	public int i32BoxUnique;

	public int i32GroupItemUnique;

	public int i32GroupItemNum;

	public int i32GroupItemGrade;

	public int i32GroupItemSkillUnique;

	public int i32GroupItemSkillLevel;

	public int i32GroupItemReducePoint;

	public int i32GroupItemTradePoint;

	public int i32GroupItemRate;

	public int i32GroupItemCongratsType;

	public int i32GroupItemSkill2Unique;

	public int i32GroupItemSkill2Level;

	public ITEM_BOX_GROUP_DATA() : base(NrTableData.eResourceType.eRT_ITEM_BOX_GROUP)
	{
		this.Init();
	}

	public void Init()
	{
		this.i32BoxUnique = 0;
		this.i32GroupItemUnique = 0;
		this.i32GroupItemNum = 0;
		this.i32GroupItemGrade = 0;
		this.i32GroupItemSkillUnique = 0;
		this.i32GroupItemSkillLevel = 0;
		this.i32GroupItemReducePoint = 0;
		this.i32GroupItemTradePoint = 0;
		this.i32GroupItemRate = 0;
		this.i32GroupItemCongratsType = 0;
		this.i32GroupItemSkill2Unique = 0;
		this.i32GroupItemSkill2Level = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.i32BoxUnique);
		row.GetColumn(num++, out this.i32GroupItemUnique);
		row.GetColumn(num++, out this.i32GroupItemNum);
		row.GetColumn(num++, out this.i32GroupItemGrade);
		row.GetColumn(num++, out this.i32GroupItemSkillUnique);
		row.GetColumn(num++, out this.i32GroupItemSkillLevel);
		row.GetColumn(num++, out this.i32GroupItemReducePoint);
		row.GetColumn(num++, out this.i32GroupItemTradePoint);
		row.GetColumn(num++, out this.i32GroupItemRate);
		row.GetColumn(num++, out this.i32GroupItemCongratsType);
		row.GetColumn(num++, out this.i32GroupItemSkill2Unique);
		row.GetColumn(num++, out this.i32GroupItemSkill2Level);
	}
}
