using System;

public class ITEM_BOX_GROUP
{
	public int i32BoxUnique;

	public int[] i32GroupItemUnique;

	public int[] i32GroupItemNum;

	public int[] i32GroupItemGrade;

	public int[] i32GroupItemSkillUnique;

	public int[] i32GroupItemSkillLevel;

	public int[] i32GroupItemReducePoint;

	public int[] i32GroupItemTradePoint;

	public int[] i32GroupItemRate;

	public int[] i32GroupItemCongratsType;

	public int[] i32GroupItemSkill2Unique;

	public int[] i32GroupItemSkill2Level;

	public ITEM_BOX_GROUP()
	{
		this.Init();
	}

	public void Init()
	{
		this.i32BoxUnique = 0;
		this.i32GroupItemUnique = new int[12];
		this.i32GroupItemNum = new int[12];
		this.i32GroupItemGrade = new int[12];
		this.i32GroupItemSkillUnique = new int[12];
		this.i32GroupItemSkillLevel = new int[12];
		this.i32GroupItemReducePoint = new int[12];
		this.i32GroupItemTradePoint = new int[12];
		this.i32GroupItemRate = new int[12];
		this.i32GroupItemCongratsType = new int[12];
		this.i32GroupItemSkill2Unique = new int[12];
		this.i32GroupItemSkill2Level = new int[12];
	}

	public void AddGroupItemData(ITEM_BOX_GROUP_DATA GroupItemData)
	{
		this.i32BoxUnique = GroupItemData.i32BoxUnique;
		for (int i = 0; i < 12; i++)
		{
			if (this.i32GroupItemUnique[i] == 0)
			{
				this.i32GroupItemUnique[i] = GroupItemData.i32GroupItemUnique;
				this.i32GroupItemNum[i] = GroupItemData.i32GroupItemNum;
				this.i32GroupItemGrade[i] = GroupItemData.i32GroupItemGrade;
				this.i32GroupItemSkillUnique[i] = GroupItemData.i32GroupItemSkillUnique;
				this.i32GroupItemSkillLevel[i] = GroupItemData.i32GroupItemSkillLevel;
				this.i32GroupItemReducePoint[i] = GroupItemData.i32GroupItemReducePoint;
				this.i32GroupItemTradePoint[i] = GroupItemData.i32GroupItemTradePoint;
				this.i32GroupItemRate[i] = GroupItemData.i32GroupItemRate;
				this.i32GroupItemCongratsType[i] = GroupItemData.i32GroupItemCongratsType;
				this.i32GroupItemSkill2Unique[i] = GroupItemData.i32GroupItemSkill2Unique;
				this.i32GroupItemSkill2Level[i] = GroupItemData.i32GroupItemSkill2Level;
				break;
			}
		}
	}
}
