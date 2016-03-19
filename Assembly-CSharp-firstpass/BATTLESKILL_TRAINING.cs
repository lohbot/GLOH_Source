using System;
using TsLibs;

public class BATTLESKILL_TRAINING : NrTableData
{
	public int m_nSkillUnique;

	public int m_nSkillLevel;

	public string[] m_szCharCode = new string[10];

	public int m_nSkillNeedGold;

	public string szAllCharCode = string.Empty;

	public BATTLESKILL_TRAINING()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nSkillUnique = 0;
		this.m_nSkillLevel = 0;
		for (int i = 0; i < 10; i++)
		{
			this.m_szCharCode[i] = string.Empty;
		}
		this.m_nSkillNeedGold = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nSkillUnique);
		row.GetColumn(num++, out this.m_nSkillLevel);
		row.GetColumn(num++, out this.szAllCharCode);
		row.GetColumn(num++, out this.m_nSkillNeedGold);
		this.BSTrainingParseCharcode(this.szAllCharCode);
	}

	public void Set(int SkillLevelIndex, BATTLESKILL_TRAINING LastdetailData, BATTLE_SKILL_TRAININGINCLUDE trainingData_I)
	{
		this.m_nSkillUnique = LastdetailData.m_nSkillUnique;
		this.m_nSkillLevel = SkillLevelIndex;
		this.m_nSkillNeedGold = LastdetailData.m_nSkillNeedGold + trainingData_I.m_nSkillNeedGold_I;
	}

	public void BSTrainingParseCharcode(string Allcharcode)
	{
		if (Allcharcode == null)
		{
			return;
		}
		string text = string.Empty;
		int num = 0;
		int num2 = 0;
		int i;
		for (i = 0; i < Allcharcode.Length; i++)
		{
			char c = Allcharcode[i];
			if (c != ' ')
			{
				if (c == '+')
				{
					text = Allcharcode.Substring(num, i - num);
					this.m_szCharCode[num2] = text;
					num2++;
					num = i + 1;
				}
			}
		}
		if (i > num + 1)
		{
			text = Allcharcode.Substring(num, i - num);
			this.m_szCharCode[num2] = text;
		}
	}
}
