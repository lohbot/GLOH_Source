using System;

public class SolSlotData
{
	public int i32KindInfo;

	public byte bSolGrade;

	public bool bShow;

	public string strSolName = string.Empty;

	public byte bBitFlag;

	public byte bBitFlagCount;

	public byte bSeason;

	public int i32SkillUnique;

	public int i32SkillText;

	public SolSlotData(string strName, int KindInfo, byte Grade, byte BitFlag, byte BitFlagCount, byte Season, int SkillUnique, int SkillText)
	{
		this.i32KindInfo = KindInfo;
		this.bSolGrade = Grade;
		this.strSolName = strName;
		this.bBitFlag = BitFlag;
		if (BitFlagCount < 0)
		{
			this.bBitFlagCount = 0;
		}
		else
		{
			this.bBitFlagCount = BitFlagCount;
		}
		this.bSeason = Season;
		this.bShow = false;
		this.i32SkillUnique = SkillUnique;
		this.i32SkillText = SkillText;
	}

	public static SolSlotData GetSolSlotData(int i32CharKind, byte Grade)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
		if (charKindInfo == null)
		{
			return null;
		}
		SOL_GUIDE solGuild = NrTSingleton<NrTableSolGuideManager>.Instance.GetSolGuild(i32CharKind);
		if (solGuild == null || NrTSingleton<ContentsLimitManager>.Instance.IsSolGuide_Season((int)solGuild.m_bSeason))
		{
			return null;
		}
		return new SolSlotData(charKindInfo.GetName(), i32CharKind, Grade, solGuild.m_bFlagSet, solGuild.m_bFlagSetCount - 1, solGuild.m_bSeason, solGuild.m_i32SkillUnique, solGuild.m_i32SkillText);
	}
}
