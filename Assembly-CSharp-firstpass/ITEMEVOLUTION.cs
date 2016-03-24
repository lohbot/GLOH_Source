using System;
using TsLibs;

public class ITEMEVOLUTION : NrTableData
{
	public struct ItemIndexData
	{
		public string name;

		public string GRADE_TYPE;

		public int STAR_GREDE;
	}

	public int nItemIndex;

	public ITEMEVOLUTION.ItemIndexData nItemIndex_Data = default(ITEMEVOLUTION.ItemIndexData);

	public int nResult_Index;

	public int nItemSkill_Condition;

	public int nItemSkillLevel;

	public int nItemSkillPenalty;

	public int nNeedGold;

	public int[] nNeedItem_Unique = new int[2];

	public int[] nNeedItem_num = new int[2];

	public ITEMEVOLUTION()
	{
		this.Init();
	}

	public void Init()
	{
		this.nItemIndex = 0;
		this.nResult_Index = 0;
		for (int i = 0; i < 3; i++)
		{
			this.nItemIndex_Data = default(ITEMEVOLUTION.ItemIndexData);
		}
		this.nItemSkillLevel = 0;
		this.nItemSkill_Condition = 0;
		this.nItemSkillPenalty = 0;
		this.nNeedGold = 0;
		for (int j = 0; j < 2; j++)
		{
			this.nNeedItem_Unique[j] = 0;
			this.nNeedItem_num[j] = 0;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.nItemIndex);
		row.GetColumn(num++, out this.nItemIndex_Data.name);
		row.GetColumn(num++, out this.nItemIndex_Data.GRADE_TYPE);
		row.GetColumn(num++, out this.nItemIndex_Data.STAR_GREDE);
		row.GetColumn(num++, out this.nResult_Index);
		row.GetColumn(num++, out this.nNeedItem_Unique[0]);
		row.GetColumn(num++, out this.nNeedItem_num[0]);
		row.GetColumn(num++, out this.nNeedItem_Unique[1]);
		row.GetColumn(num++, out this.nNeedItem_num[1]);
		row.GetColumn(num++, out this.nNeedGold);
		row.GetColumn(num++, out this.nItemSkill_Condition);
		row.GetColumn(num++, out this.nItemSkillPenalty);
	}
}
