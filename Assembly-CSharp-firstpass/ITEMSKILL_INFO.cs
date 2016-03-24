using GAME;
using System;
using TsLibs;

public class ITEMSKILL_INFO : NrTableData
{
	public string m_strItemType = string.Empty;

	public int SkillUnique;

	public string PrefixText;

	public eITEM_TYPE m_eItemType;

	public short iAuctionSearch;

	public ITEMSKILL_INFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.SkillUnique = 0;
		this.PrefixText = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(1, out this.m_strItemType);
		row.GetColumn(2, out this.SkillUnique);
		row.GetColumn(3, out this.PrefixText);
		row.GetColumn(36, out this.iAuctionSearch);
	}
}
