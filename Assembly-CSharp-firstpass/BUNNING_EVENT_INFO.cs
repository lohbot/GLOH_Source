using GAME;
using System;
using TsLibs;

public class BUNNING_EVENT_INFO : NrTableData
{
	public sbyte m_nWeek = -1;

	public short m_nYear;

	public byte m_nMonth;

	public byte m_nDay;

	public int m_nStartTime;

	public int m_nDurationTime;

	public short m_nEndYear;

	public byte m_nEndMonth;

	public byte m_nEndDay;

	public int m_nEndTime;

	public eBUNNING_EVENT m_eEventType;

	public int m_nExplain;

	public int m_nTitleText;

	public string m_strRewardType = string.Empty;

	public int m_nItemUnique;

	public int m_nRewardCount;

	public int m_nRewardIcon;

	public int m_nLimitCount;

	public string m_strImage = string.Empty;

	public string m_strSolTalkImg = string.Empty;

	public int m_nLimitLevel;

	public byte m_nShowList;

	public string strEventType = string.Empty;

	public BUNNING_EVENT_INFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nWeek = -1;
		this.m_nYear = 0;
		this.m_nMonth = 0;
		this.m_nDay = 0;
		this.m_nStartTime = 0;
		this.m_nDurationTime = 0;
		this.m_nEndYear = 0;
		this.m_nEndMonth = 0;
		this.m_nEndDay = 0;
		this.m_nEndTime = 0;
		this.m_eEventType = eBUNNING_EVENT.eBUNNING_EVENT_NONE;
		this.m_nExplain = 0;
		this.m_nTitleText = 0;
		this.m_strRewardType = string.Empty;
		this.m_nItemUnique = 0;
		this.m_nRewardCount = 0;
		this.m_nRewardIcon = 0;
		this.m_nLimitCount = 0;
		this.m_strImage = string.Empty;
		this.m_strSolTalkImg = string.Empty;
		this.m_nLimitLevel = 0;
		this.m_nShowList = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nWeek);
		row.GetColumn(num++, out this.m_nYear);
		row.GetColumn(num++, out this.m_nMonth);
		row.GetColumn(num++, out this.m_nDay);
		row.GetColumn(num++, out this.m_nStartTime);
		row.GetColumn(num++, out this.m_nDurationTime);
		row.GetColumn(num++, out this.m_nEndYear);
		row.GetColumn(num++, out this.m_nEndMonth);
		row.GetColumn(num++, out this.m_nEndDay);
		row.GetColumn(num++, out this.m_nEndTime);
		row.GetColumn(num++, out this.strEventType);
		row.GetColumn(num++, out this.m_nExplain);
		row.GetColumn(num++, out this.m_nTitleText);
		row.GetColumn(num++, out this.m_strRewardType);
		row.GetColumn(num++, out this.m_nItemUnique);
		row.GetColumn(num++, out this.m_nRewardCount);
		row.GetColumn(num++, out this.m_nRewardIcon);
		row.GetColumn(num++, out this.m_nLimitCount);
		row.GetColumn(num++, out this.m_strImage);
		row.GetColumn(num++, out this.m_strSolTalkImg);
		row.GetColumn(num++, out this.m_nLimitLevel);
		row.GetColumn(num++, out this.m_nShowList);
	}
}
