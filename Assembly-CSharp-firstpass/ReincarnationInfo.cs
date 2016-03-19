using System;
using TsLibs;

public class ReincarnationInfo : NrTableData
{
	public int iType;

	public long lNeedMoney;

	public int[] iCharKind = new int[6];

	public string[] strText = new string[6];

	public int iNeedLevel;

	public ReincarnationInfo() : base(NrTableData.eResourceType.eRT_REINCARNATIONINFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.iType = 0;
		this.lNeedMoney = 0L;
		for (int i = 0; i < 6; i++)
		{
			this.iCharKind[i] = 0;
			this.strText[i] = string.Empty;
		}
		this.iNeedLevel = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(0, out this.iType);
		row.GetColumn(1, out this.lNeedMoney);
		row.GetColumn(2, out this.strText[0]);
		row.GetColumn(3, out this.strText[1]);
		row.GetColumn(4, out this.strText[2]);
		row.GetColumn(5, out this.strText[3]);
		row.GetColumn(6, out this.strText[4]);
		row.GetColumn(7, out this.strText[5]);
		row.GetColumn(8, out this.iNeedLevel);
	}

	public int GetReincarnationCharKind(ReincarnationInfo BeforeReincarnationInfo, int CharKind)
	{
		if (BeforeReincarnationInfo == null)
		{
			return 0;
		}
		for (int i = 0; i < 6; i++)
		{
			if (BeforeReincarnationInfo.iCharKind[i] == CharKind)
			{
				return this.iCharKind[i];
			}
		}
		return 0;
	}
}
