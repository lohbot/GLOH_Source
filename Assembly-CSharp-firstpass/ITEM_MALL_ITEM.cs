using SERVICE;
using System;
using TsLibs;

public class ITEM_MALL_ITEM : NrTableData
{
	public long m_Idx;

	public int m_nGroup;

	public bool m_isRecommend;

	public string m_strIconPath = string.Empty;

	public string m_strTextKey = string.Empty;

	public string m_strTextNote = string.Empty;

	public int m_nMoneyType;

	public long m_nPrice;

	public float m_fPrice;

	public long m_nItemUnique;

	public long m_nItemNum;

	public long m_nBounusItemNum;

	public long m_nGetMoney;

	public int m_nSaleNum;

	public byte m_nGift;

	public string m_strItemTextKey = string.Empty;

	public string m_strItemTooltip = string.Empty;

	public string m_strSolKind = string.Empty;

	public bool m_isEvent;

	public int m_nVipExp;

	public string m_strGoogleQA = string.Empty;

	public string m_strGoogle = string.Empty;

	public string m_strApple = string.Empty;

	public ITEM_MALL_ITEM()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_Idx = 0L;
		this.m_nGroup = 0;
		this.m_isRecommend = false;
		this.m_nItemUnique = 0L;
		this.m_strIconPath = string.Empty;
		this.m_strTextKey = string.Empty;
		this.m_strTextNote = string.Empty;
		this.m_nMoneyType = 0;
		this.m_nPrice = 0L;
		this.m_fPrice = 0f;
		this.m_nItemUnique = 0L;
		this.m_nItemNum = 0L;
		this.m_nBounusItemNum = 0L;
		this.m_nGetMoney = 0L;
		this.m_nSaleNum = 0;
		this.m_nGift = 0;
		this.m_strItemTextKey = string.Empty;
		this.m_strItemTooltip = string.Empty;
		this.m_strSolKind = string.Empty;
		this.m_isEvent = false;
		this.m_nVipExp = 0;
		this.m_strGoogleQA = string.Empty;
		this.m_strGoogle = string.Empty;
		this.m_strApple = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_Idx);
		row.GetColumn(num++, out this.m_nGroup);
		row.GetColumn(num++, out this.m_isRecommend);
		row.GetColumn(num++, out this.m_strIconPath);
		row.GetColumn(num++, out this.m_strTextKey);
		row.GetColumn(num++, out this.m_strTextNote);
		row.GetColumn(num++, out this.m_nMoneyType);
		row.GetColumn(num++, out this.m_nPrice);
		row.GetColumn(num++, out this.m_fPrice);
		row.GetColumn(num++, out this.m_nItemUnique);
		row.GetColumn(num++, out this.m_nItemNum);
		row.GetColumn(num++, out this.m_nBounusItemNum);
		row.GetColumn(num++, out this.m_nGetMoney);
		row.GetColumn(num++, out this.m_nSaleNum);
		row.GetColumn(num++, out this.m_nGift);
		row.GetColumn(num++, out this.m_strItemTextKey);
		row.GetColumn(num++, out this.m_strItemTooltip);
		row.GetColumn(num++, out this.m_strSolKind);
		row.GetColumn(num++, out this.m_isEvent);
		row.GetColumn(num++, out this.m_nVipExp);
		row.GetColumn(num++, out this.m_strGoogleQA);
		row.GetColumn(num++, out this.m_strGoogle);
		row.GetColumn(num++, out this.m_strApple);
	}

	public void Set(ITEM_MALL_ITEM Data)
	{
		this.m_Idx = Data.m_Idx;
		this.m_nGroup = Data.m_nGroup;
		this.m_isRecommend = Data.m_isRecommend;
		this.m_strIconPath = Data.m_strIconPath;
		this.m_strTextKey = Data.m_strTextKey;
		this.m_strTextNote = Data.m_strTextNote;
		this.m_nMoneyType = Data.m_nMoneyType;
		this.m_nPrice = Data.m_nPrice;
		this.m_fPrice = Data.m_fPrice;
		this.m_nItemUnique = Data.m_nItemUnique;
		this.m_nItemNum = Data.m_nItemNum;
		this.m_nBounusItemNum = Data.m_nBounusItemNum;
		this.m_nGetMoney = Data.m_nGetMoney;
		this.m_nSaleNum = Data.m_nSaleNum;
		this.m_nGift = Data.m_nGift;
		this.m_strItemTextKey = Data.m_strItemTextKey;
		this.m_strItemTooltip = Data.m_strItemTooltip;
		this.m_strSolKind = Data.m_strSolKind;
		this.m_isEvent = Data.m_isEvent;
		this.m_strGoogleQA = Data.m_strGoogleQA;
		this.m_strGoogle = Data.m_strGoogle;
		this.m_strApple = Data.m_strApple;
	}

	public string GetStoreItem()
	{
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		eSERVICE_AREA eSERVICE_AREA = currentServiceArea;
		if (eSERVICE_AREA == eSERVICE_AREA.SERVICE_ANDROID_USQA)
		{
			return this.m_strGoogleQA;
		}
		if (eSERVICE_AREA == eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE)
		{
			return this.m_strGoogle;
		}
		if (eSERVICE_AREA != eSERVICE_AREA.SERVICE_IOS_USIOS)
		{
			return string.Empty;
		}
		return this.m_strApple;
	}
}
