using System;
using TsLibs;

public class MYTHSKILL_TRAINING : NrTableData
{
	public int m_i32SkillUnique;

	public int m_i32SkillLevel;

	public string[] m_strCharCode = new string[20];

	public int m_i32SkillNeedItem;

	public string m_strAllCharCode = string.Empty;

	public MYTHSKILL_TRAINING()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_i32SkillUnique = 0;
		this.m_i32SkillLevel = 0;
		for (int i = 0; i < 20; i++)
		{
			this.m_strCharCode[i] = string.Empty;
		}
		this.m_i32SkillNeedItem = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_i32SkillUnique);
		row.GetColumn(num++, out this.m_i32SkillLevel);
		row.GetColumn(num++, out this.m_strAllCharCode);
		row.GetColumn(num++, out this.m_i32SkillNeedItem);
		this.BSTrainingParseCharcode(this.m_strAllCharCode);
	}

	public void BSTrainingParseCharcode(string strCharCode)
	{
		if (strCharCode == null)
		{
			return;
		}
		string text = string.Empty;
		int num = 0;
		int num2 = 0;
		int i;
		for (i = 0; i < strCharCode.Length; i++)
		{
			char c = strCharCode[i];
			if (c != ' ')
			{
				if (c == '+')
				{
					text = strCharCode.Substring(num, i - num);
					this.m_strCharCode[num2] = text;
					num2++;
					num = i + 1;
				}
			}
		}
		if (i > num + 1)
		{
			text = strCharCode.Substring(num, i - num);
			this.m_strCharCode[num2] = text;
		}
	}
}
