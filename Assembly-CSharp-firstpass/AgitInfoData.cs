using System;
using TsLibs;

public class AgitInfoData : NrTableData
{
	public short i16Level;

	public long i64Exp;

	public long i64NeedGuildFund;

	public byte i8NPCNum;

	public int i32NPCCost;

	public byte i8MonsterNum;

	public short i16MonsterMaxLevel;

	public short i16MerchantRate;

	public byte i8MerchantSellKind;

	public byte i8MerchantVisitNum;

	public AgitInfoData() : base(NrTableData.eResourceType.eRT_AGIT_INFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.i16Level = 0;
		this.i64Exp = 0L;
		this.i64NeedGuildFund = 0L;
		this.i8NPCNum = 0;
		this.i32NPCCost = 0;
		this.i8MonsterNum = 0;
		this.i16MerchantRate = 0;
		this.i8MerchantSellKind = 0;
		this.i8MerchantVisitNum = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(0, out this.i16Level);
		row.GetColumn(1, out this.i64Exp);
		row.GetColumn(2, out this.i64NeedGuildFund);
		row.GetColumn(3, out this.i8NPCNum);
		row.GetColumn(4, out this.i32NPCCost);
		row.GetColumn(5, out this.i8MonsterNum);
		row.GetColumn(6, out this.i16MonsterMaxLevel);
		row.GetColumn(7, out this.i16MerchantRate);
		row.GetColumn(8, out this.i8MerchantSellKind);
		row.GetColumn(9, out this.i8MerchantVisitNum);
	}
}
