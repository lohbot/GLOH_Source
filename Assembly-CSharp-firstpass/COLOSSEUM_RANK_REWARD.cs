using System;
using TsLibs;

public class COLOSSEUM_RANK_REWARD : NrTableData
{
	public short m_nGrade;

	public string m_strLeagueName;

	public int m_nRank_Min;

	public int m_nRank_Max;

	public string m_strRewardItemName;

	public int m_nItemUnique;

	public int m_nItemNum;

	public string m_strExplainRewardItemName1;

	public int m_nExplainItemUnique1;

	public int m_nExplainItemNum1;

	public string m_strExplainRewardItemName2;

	public int m_nExplainItemUnique2;

	public int m_nExplainItemNum2;

	public COLOSSEUM_RANK_REWARD()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nGrade = 0;
		this.m_strLeagueName = null;
		this.m_nRank_Min = 0;
		this.m_nRank_Max = 0;
		this.m_strRewardItemName = null;
		this.m_nItemUnique = 0;
		this.m_nItemNum = 0;
		this.m_strExplainRewardItemName1 = null;
		this.m_nExplainItemUnique1 = 0;
		this.m_nExplainItemNum1 = 0;
		this.m_strExplainRewardItemName2 = null;
		this.m_nExplainItemUnique2 = 0;
		this.m_nExplainItemNum2 = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nGrade);
		row.GetColumn(num++, out this.m_strLeagueName);
		row.GetColumn(num++, out this.m_nRank_Min);
		row.GetColumn(num++, out this.m_nRank_Max);
		row.GetColumn(num++, out this.m_strRewardItemName);
		row.GetColumn(num++, out this.m_nItemUnique);
		row.GetColumn(num++, out this.m_nItemNum);
		row.GetColumn(num++, out this.m_strExplainRewardItemName1);
		row.GetColumn(num++, out this.m_nExplainItemUnique1);
		row.GetColumn(num++, out this.m_nExplainItemNum1);
		row.GetColumn(num++, out this.m_strExplainRewardItemName2);
		row.GetColumn(num++, out this.m_nExplainItemUnique2);
		row.GetColumn(num++, out this.m_nExplainItemNum2);
	}
}
