using System;
using TsLibs;

public class charSpend : NrTableData
{
	public short iLevel;

	public long lCharChangeGold;

	public long iCharWillChargeGold;

	public short iResurrection_spend;

	public long lFriendSopprtGold;

	public int i32FriendSupport_1;

	public int i32FriendSupport_2;

	public long iCharWillChargeLimit1Gold;

	public long iCharWillChargeLimit2Gold;

	public long iCharWillChargeLimit3Gold;

	public long iCharWillChargeLimit4Gold;

	public long iCharWillChargeLimit5Gold;

	public long iCharWillChargeLimit6Gold;

	public charSpend() : base(NrTableData.eResourceType.eRT_CHARSPEND)
	{
		this.Init();
	}

	public void Init()
	{
		this.iLevel = 0;
		this.lCharChangeGold = 0L;
		this.iCharWillChargeGold = 0L;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(0, out this.iLevel);
		row.GetColumn(1, out this.lCharChangeGold);
		row.GetColumn(2, out this.iCharWillChargeGold);
		row.GetColumn(3, out this.iResurrection_spend);
		row.GetColumn(4, out this.lFriendSopprtGold);
		row.GetColumn(5, out this.i32FriendSupport_1);
		row.GetColumn(6, out this.i32FriendSupport_2);
		row.GetColumn(7, out this.iCharWillChargeLimit1Gold);
		row.GetColumn(8, out this.iCharWillChargeLimit2Gold);
		row.GetColumn(9, out this.iCharWillChargeLimit3Gold);
		row.GetColumn(10, out this.iCharWillChargeLimit4Gold);
		row.GetColumn(11, out this.iCharWillChargeLimit5Gold);
		row.GetColumn(12, out this.iCharWillChargeLimit6Gold);
	}
}
