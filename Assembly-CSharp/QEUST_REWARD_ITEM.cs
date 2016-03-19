using System;
using TsLibs;

public class QEUST_REWARD_ITEM
{
	public string strQuestUnique = string.Empty;

	public int i32Grade;

	public long i64RewardMoney;

	public long i64RewardExp;

	public int nRewardItemUnique0;

	public int nRewardItemNum0;

	public int nRewardItemUnique1;

	public int nRewardItemNum1;

	public int nRewardItemUnique2;

	public int nRewardItemNum2;

	public int nRewardItemUnique3;

	public int nRewardItemNum3;

	public int nRewardItemUnique4;

	public int nRewardItemNum4;

	public int i32RecruitGenneralUnique;

	public int i32RecruitGenCharKind;

	public int nRecruitReplaceItemUnique;

	public int nRecruitReplaceItemNum;

	public int i32UpgradeSoldierUnique;

	public int i32UpgradeGenCharKind;

	public int i32UpgradePreGrade;

	public int i32UpgradePostGrade;

	public int nUpgradeReplaceItemUnique;

	public int nUpgradeReplaceItemNum;

	public short nReputeUnique;

	public int nReputeValue;

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.strQuestUnique);
		row.GetColumn(num++, out this.i32Grade);
		row.GetColumn(num++, out this.i64RewardMoney);
		row.GetColumn(num++, out this.i64RewardExp);
		row.GetColumn(num++, out this.nRewardItemUnique0);
		row.GetColumn(num++, out this.nRewardItemNum0);
		row.GetColumn(num++, out this.nRewardItemUnique1);
		row.GetColumn(num++, out this.nRewardItemNum1);
		row.GetColumn(num++, out this.nRewardItemUnique2);
		row.GetColumn(num++, out this.nRewardItemNum2);
		row.GetColumn(num++, out this.nRewardItemUnique3);
		row.GetColumn(num++, out this.nRewardItemNum3);
		row.GetColumn(num++, out this.nRewardItemUnique4);
		row.GetColumn(num++, out this.nRewardItemNum4);
		row.GetColumn(num++, out this.i32RecruitGenCharKind);
		row.GetColumn(num++, out this.nRecruitReplaceItemUnique);
		row.GetColumn(num++, out this.nRecruitReplaceItemNum);
		row.GetColumn(num++, out this.i32UpgradeGenCharKind);
		row.GetColumn(num++, out this.i32UpgradePreGrade);
		row.GetColumn(num++, out this.i32UpgradePostGrade);
		row.GetColumn(num++, out this.nUpgradeReplaceItemUnique);
		row.GetColumn(num++, out this.nUpgradeReplaceItemNum);
		row.GetColumn(num++, out this.nReputeUnique);
		row.GetColumn(num++, out this.nReputeValue);
	}
}
