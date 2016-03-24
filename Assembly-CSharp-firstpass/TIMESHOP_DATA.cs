using System;
using TsLibs;

public class TIMESHOP_DATA : NrTableData
{
	public long m_lIdx;

	public byte m_byType;

	public byte m_byRecommend;

	public string m_strIconPath = string.Empty;

	public string m_strProductTextKey = string.Empty;

	public string m_strProductNote = string.Empty;

	public int m_nMoneyType;

	public long m_lDisplayPrice;

	public long m_lPrice;

	public int m_nItemUnique;

	public long m_lItemNum;

	public float m_fRate;

	public string m_strItemToolTip = string.Empty;

	public string m_strSolKind = string.Empty;

	public TIMESHOP_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_lIdx = 0L;
		this.m_byType = 0;
		this.m_byRecommend = 0;
		this.m_strIconPath = string.Empty;
		this.m_strProductTextKey = string.Empty;
		this.m_strProductNote = string.Empty;
		this.m_nMoneyType = 0;
		this.m_lDisplayPrice = 0L;
		this.m_lPrice = 0L;
		this.m_nItemUnique = 0;
		this.m_lItemNum = 0L;
		this.m_fRate = 0f;
		this.m_strItemToolTip = string.Empty;
		this.m_strSolKind = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_lIdx);
		row.GetColumn(num++, out this.m_byType);
		row.GetColumn(num++, out this.m_byRecommend);
		row.GetColumn(num++, out this.m_strIconPath);
		row.GetColumn(num++, out this.m_strProductTextKey);
		row.GetColumn(num++, out this.m_strProductNote);
		row.GetColumn(num++, out this.m_nMoneyType);
		row.GetColumn(num++, out this.m_lDisplayPrice);
		row.GetColumn(num++, out this.m_lPrice);
		row.GetColumn(num++, out this.m_nItemUnique);
		row.GetColumn(num++, out this.m_lItemNum);
		row.GetColumn(num++, out this.m_fRate);
		row.GetColumn(num++, out this.m_strItemToolTip);
		row.GetColumn(num++, out this.m_strSolKind);
	}
}
