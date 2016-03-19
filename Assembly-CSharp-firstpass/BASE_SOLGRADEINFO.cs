using System;
using TsLibs;

public class BASE_SOLGRADEINFO : NrTableData
{
	public string CharCode = string.Empty;

	public int SolGrade;

	public int SolSeason;

	public short LegendType;

	public short MaxLevel;

	public CHARKIND_SOLSTATINFO kSolStatInfo = new CHARKIND_SOLSTATINFO();

	public CHARKIND_SOLINCSTATINFO kSolIncStatInfo = new CHARKIND_SOLINCSTATINFO();

	public int[] nGainRate = new int[20];

	public long ComposeExp;

	public long EvolutionExp;

	public long ComposeCost;

	public long EvolutionCost;

	public long SellPrice;

	public byte TradeRank;

	public byte ComposeLimit;

	public long EvolutionNeedExp;

	public long MaxLv_Evolution_Exp;

	public BASE_SOLGRADEINFO() : base(NrTableData.eResourceType.eRT_CHARKIND_SOLGRADEINFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.CharCode = string.Empty;
		this.SolGrade = 0;
		this.SolSeason = 0;
		this.LegendType = 0;
		this.MaxLevel = 0;
		this.kSolStatInfo.Init();
		this.kSolIncStatInfo.Init();
		for (int i = 0; i < 20; i++)
		{
			this.nGainRate[i] = 0;
		}
		this.ComposeExp = 0L;
		this.EvolutionExp = 0L;
		this.ComposeCost = 0L;
		this.EvolutionCost = 0L;
		this.SellPrice = 0L;
		this.TradeRank = 0;
		this.ComposeLimit = 0;
		this.EvolutionNeedExp = 0L;
		this.MaxLv_Evolution_Exp = 0L;
	}

	public void SetGradeInfo(ref BASE_SOLGRADEINFO pkInfo)
	{
		this.CharCode = pkInfo.CharCode;
		this.SolGrade = pkInfo.SolGrade;
		this.SolSeason = pkInfo.SolSeason;
		this.LegendType = pkInfo.LegendType;
		this.MaxLevel = pkInfo.MaxLevel;
		this.kSolStatInfo.Set(ref pkInfo.kSolStatInfo);
		this.kSolIncStatInfo.Set(ref pkInfo.kSolIncStatInfo);
		for (int i = 0; i < 20; i++)
		{
			this.nGainRate[i] = pkInfo.nGainRate[i];
		}
		this.ComposeExp = pkInfo.ComposeExp;
		this.EvolutionExp = pkInfo.EvolutionExp;
		this.ComposeCost = pkInfo.ComposeCost;
		this.EvolutionCost = pkInfo.EvolutionCost;
		this.SellPrice = pkInfo.SellPrice;
		this.TradeRank = pkInfo.TradeRank;
		this.ComposeLimit = pkInfo.ComposeLimit;
		this.EvolutionNeedExp = pkInfo.EvolutionNeedExp;
		this.MaxLv_Evolution_Exp = pkInfo.MaxLv_Evolution_Exp;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		int num2 = 0;
		int num3 = 0;
		row.GetColumn(num++, out this.CharCode);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out this.LegendType);
		row.GetColumn(num++, out num3);
		row.GetColumn(num++, out this.MaxLevel);
		row.GetColumn(num++, out this.kSolStatInfo.STR);
		row.GetColumn(num++, out this.kSolStatInfo.DEX);
		row.GetColumn(num++, out this.kSolStatInfo.INT);
		row.GetColumn(num++, out this.kSolStatInfo.VIT);
		row.GetColumn(num++, out this.kSolStatInfo.HP);
		row.GetColumn(num++, out this.kSolStatInfo.MIN_DAMAGE);
		row.GetColumn(num++, out this.kSolStatInfo.MAX_DAMAGE);
		row.GetColumn(num++, out this.kSolStatInfo.DEFENSE);
		row.GetColumn(num++, out this.kSolStatInfo.MAGICDEFENSE);
		row.GetColumn(num++, out this.kSolStatInfo.HITRATE);
		row.GetColumn(num++, out this.kSolStatInfo.EVASION);
		row.GetColumn(num++, out this.kSolStatInfo.CRITICAL);
		row.GetColumn(num++, out this.kSolIncStatInfo.INC_STR);
		row.GetColumn(num++, out this.kSolIncStatInfo.INC_DEX);
		row.GetColumn(num++, out this.kSolIncStatInfo.INC_INT);
		row.GetColumn(num++, out this.kSolIncStatInfo.INC_VIT);
		row.GetColumn(num++, out this.nGainRate[0]);
		row.GetColumn(num++, out this.nGainRate[1]);
		row.GetColumn(num++, out this.nGainRate[2]);
		row.GetColumn(num++, out this.nGainRate[3]);
		row.GetColumn(num++, out this.nGainRate[4]);
		row.GetColumn(num++, out this.nGainRate[5]);
		row.GetColumn(num++, out this.ComposeExp);
		row.GetColumn(num++, out this.EvolutionExp);
		row.GetColumn(num++, out this.ComposeCost);
		row.GetColumn(num++, out this.EvolutionCost);
		row.GetColumn(num++, out this.SellPrice);
		row.GetColumn(num++, out this.TradeRank);
		row.GetColumn(num++, out this.ComposeLimit);
		row.GetColumn(num++, out this.EvolutionNeedExp);
		row.GetColumn(num++, out this.MaxLv_Evolution_Exp);
		this.SolSeason = num2 - 1;
		this.SolGrade = num3 - 1;
	}
}
