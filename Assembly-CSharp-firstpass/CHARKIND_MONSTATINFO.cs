using System;
using TsLibs;

public class CHARKIND_MONSTATINFO : NrTableData
{
	public short MonType;

	public short LEVEL;

	public CHARKIND_SOLSTATINFO kSolStatInfo = new CHARKIND_SOLSTATINFO();

	public long EXP;

	public string DROPITEM = string.Empty;

	public CHARKIND_MONSTATINFO() : base(NrTableData.eResourceType.eRT_CHARKIND_MONSTATINFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.MonType = 0;
		this.LEVEL = 0;
		this.kSolStatInfo.Init();
		this.EXP = 0L;
		this.DROPITEM = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.MonType);
		row.GetColumn(num++, out this.LEVEL);
		row.GetColumn(num++, out this.kSolStatInfo.HP);
		row.GetColumn(num++, out this.kSolStatInfo.MIN_DAMAGE);
		row.GetColumn(num++, out this.kSolStatInfo.MAX_DAMAGE);
		row.GetColumn(num++, out this.kSolStatInfo.DEFENSE);
		row.GetColumn(num++, out this.kSolStatInfo.MAGICDEFENSE);
		row.GetColumn(num++, out this.kSolStatInfo.HITRATE);
		row.GetColumn(num++, out this.kSolStatInfo.EVASION);
		row.GetColumn(num++, out this.kSolStatInfo.CRITICAL);
		row.GetColumn(num++, out this.EXP);
		row.GetColumn(num++, out this.DROPITEM);
	}
}
