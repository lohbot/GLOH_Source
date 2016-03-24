using System;
using TsLibs;

public class ITEMSKILLREINFORCE : NrTableData
{
	public int nGroupUnique;

	public int nItemSkillLevel;

	public int nRestoreGold;

	public int nFailrate;

	public int nNeedGold;

	public int[] nNeedItem_Unique = new int[2];

	public int[] nNeedItem_num = new int[2];

	public int nProtectitem_Unique;

	public int nProtectitem_num;

	public ITEMSKILLREINFORCE()
	{
		this.Init();
	}

	public void Init()
	{
		this.nGroupUnique = 0;
		this.nItemSkillLevel = 0;
		this.nRestoreGold = 0;
		this.nFailrate = 0;
		this.nNeedGold = 0;
		for (int i = 0; i < 2; i++)
		{
			this.nNeedItem_Unique[i] = 0;
			this.nNeedItem_num[i] = 0;
		}
		this.nProtectitem_Unique = 0;
		this.nProtectitem_num = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.nGroupUnique);
		row.GetColumn(num++, out this.nItemSkillLevel);
		row.GetColumn(num++, out this.nRestoreGold);
		row.GetColumn(num++, out this.nFailrate);
		row.GetColumn(num++, out this.nNeedGold);
		row.GetColumn(num++, out this.nNeedItem_Unique[0]);
		row.GetColumn(num++, out this.nNeedItem_num[0]);
		row.GetColumn(num++, out this.nNeedItem_Unique[1]);
		row.GetColumn(num++, out this.nNeedItem_num[1]);
		row.GetColumn(num++, out this.nProtectitem_Unique);
		row.GetColumn(num++, out this.nProtectitem_num);
	}

	public void Set(ITEMSKILLREINFORCE pkNowItemSkillReinforceData)
	{
		if (pkNowItemSkillReinforceData != null)
		{
			this.nGroupUnique = pkNowItemSkillReinforceData.nGroupUnique;
			this.nItemSkillLevel = pkNowItemSkillReinforceData.nItemSkillLevel;
			this.nRestoreGold = pkNowItemSkillReinforceData.nRestoreGold;
			this.nFailrate = pkNowItemSkillReinforceData.nFailrate;
			this.nNeedGold = pkNowItemSkillReinforceData.nNeedGold;
			for (int i = 0; i < 2; i++)
			{
				this.nNeedItem_Unique[i] = pkNowItemSkillReinforceData.nNeedItem_Unique[i];
				this.nNeedItem_num[i] = pkNowItemSkillReinforceData.nNeedItem_num[i];
			}
			this.nProtectitem_Unique = pkNowItemSkillReinforceData.nProtectitem_Unique;
			this.nProtectitem_num = pkNowItemSkillReinforceData.nProtectitem_num;
		}
	}

	public void Add(int skillLevel, ITEMSKILLREINFORCE pkNowItemSkillReinforceData)
	{
		if (pkNowItemSkillReinforceData != null && pkNowItemSkillReinforceData.nGroupUnique == this.nGroupUnique)
		{
			this.nItemSkillLevel = skillLevel;
			this.nRestoreGold += pkNowItemSkillReinforceData.nRestoreGold;
			this.nFailrate += pkNowItemSkillReinforceData.nFailrate;
			this.nNeedGold += pkNowItemSkillReinforceData.nNeedGold;
			for (int i = 0; i < 2; i++)
			{
				this.nNeedItem_num[i] += pkNowItemSkillReinforceData.nNeedItem_num[i];
			}
			this.nProtectitem_num += pkNowItemSkillReinforceData.nProtectitem_num;
		}
	}
}
