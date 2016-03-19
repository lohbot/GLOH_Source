using System;
using TsLibs;

public class RECOMMEND_REWARD : NrTableData
{
	public byte i8RecommendCount;

	public int i32ItemUnique;

	public byte i8ItemCount;

	public long i64Money;

	public RECOMMEND_REWARD() : base(NrTableData.eResourceType.eRT_RECOMMEND_REWARD)
	{
		this.Init();
	}

	public void Init()
	{
		this.i8RecommendCount = 0;
		this.i32ItemUnique = 0;
		this.i8ItemCount = 0;
		this.i64Money = 0L;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.i8RecommendCount);
		row.GetColumn(num++, out this.i32ItemUnique);
		row.GetColumn(num++, out this.i8ItemCount);
		row.GetColumn(num++, out this.i64Money);
	}
}
