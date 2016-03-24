using System;
using TsLibs;

public class ITEMMALL_POPUPSHOP : NrTableData
{
	public int m_Idx;

	public long m_nShopIDX;

	public int m_nATB;

	public long m_nBeforeShopIDX;

	public long m_nAfterShopIDX;

	public int m_nMinLevel;

	public int m_nMaxLevel;

	public int m_nCoolTime;

	public string m_strIconPath = string.Empty;

	public string m_strCloseText = string.Empty;

	public string m_strCloseDayText = string.Empty;

	public ITEMMALL_POPUPSHOP()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_Idx = 0;
		this.m_nShopIDX = 0L;
		this.m_nATB = 0;
		this.m_nBeforeShopIDX = 0L;
		this.m_nAfterShopIDX = 0L;
		this.m_nMinLevel = 0;
		this.m_nMaxLevel = 0;
		this.m_nCoolTime = 0;
		this.m_strIconPath = string.Empty;
		this.m_strCloseText = string.Empty;
		this.m_strCloseDayText = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.m_Idx);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.m_nShopIDX);
		row.GetColumn(num++, out this.m_nATB);
		row.GetColumn(num++, out this.m_nBeforeShopIDX);
		row.GetColumn(num++, out this.m_nAfterShopIDX);
		row.GetColumn(num++, out this.m_nMinLevel);
		row.GetColumn(num++, out this.m_nMaxLevel);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.m_nCoolTime);
		row.GetColumn(num++, out this.m_strIconPath);
		row.GetColumn(num++, out this.m_strCloseText);
		row.GetColumn(num++, out this.m_strCloseDayText);
	}

	public void Set(ITEMMALL_POPUPSHOP Data)
	{
		this.m_Idx = Data.m_Idx;
		this.m_nShopIDX = Data.m_nShopIDX;
		this.m_nATB = Data.m_nATB;
		this.m_nAfterShopIDX = Data.m_nBeforeShopIDX;
		this.m_nAfterShopIDX = Data.m_nAfterShopIDX;
		this.m_nMinLevel = Data.m_nMinLevel;
		this.m_nMaxLevel = Data.m_nMaxLevel;
		this.m_nCoolTime = Data.m_nCoolTime;
		this.m_strIconPath = Data.m_strIconPath;
		this.m_strCloseText = Data.m_strCloseText;
		this.m_strCloseDayText = Data.m_strCloseDayText;
	}

	public string GetStoreItem()
	{
		return string.Empty;
	}
}
