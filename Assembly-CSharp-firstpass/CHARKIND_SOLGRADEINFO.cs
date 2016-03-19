using System;

public class CHARKIND_SOLGRADEINFO
{
	public BASE_SOLGRADEINFO[] kBaseGradeInfo = new BASE_SOLGRADEINFO[15];

	public CHARKIND_SOLGRADEINFO()
	{
		for (int i = 0; i < 15; i++)
		{
			this.kBaseGradeInfo[i] = new BASE_SOLGRADEINFO();
		}
		this.Init();
	}

	public void Init()
	{
		for (int i = 0; i < 15; i++)
		{
			this.kBaseGradeInfo[i].Init();
		}
	}

	public void SetSolGradeInfo(ref BASE_SOLGRADEINFO pkInfo)
	{
		if (pkInfo.SolGrade < 0 || pkInfo.SolGrade >= 15)
		{
			return;
		}
		this.kBaseGradeInfo[pkInfo.SolGrade].SetGradeInfo(ref pkInfo);
	}

	public void SetMaxLevel(int solgrade, short level)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return;
		}
		this.kBaseGradeInfo[solgrade].MaxLevel = level;
	}

	public short GetMaxLevel(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 200;
		}
		return this.kBaseGradeInfo[solgrade].MaxLevel;
	}

	public short GetLegendType(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.kBaseGradeInfo[solgrade].LegendType;
	}
}
