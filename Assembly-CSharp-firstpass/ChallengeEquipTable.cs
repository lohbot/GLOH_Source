using System;
using TsLibs;

public class ChallengeEquipTable
{
	public short m_ChallengeIdx;

	public int m_GroupIdx;

	public string m_LabelText1 = string.Empty;

	public string m_LabelText2 = string.Empty;

	public ChallengeEquipTable()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_GroupIdx = 0;
		this.m_ChallengeIdx = 0;
		this.m_LabelText1 = string.Empty;
		this.m_LabelText2 = string.Empty;
	}

	public void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string text;
		row.GetColumn(num++, out text);
		row.GetColumn(num++, out this.m_ChallengeIdx);
		row.GetColumn(num++, out this.m_GroupIdx);
		row.GetColumn(num++, out this.m_LabelText1);
		row.GetColumn(num++, out this.m_LabelText2);
	}
}
