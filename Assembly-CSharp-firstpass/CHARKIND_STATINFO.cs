using PROTOCOL.GAME;
using System;
using TsLibs;

public class CHARKIND_STATINFO : NrTableData
{
	public string CharCode = string.Empty;

	public BATTLESKILL_DATA[] kBattleSkillData = new BATTLESKILL_DATA[3];

	public short BATTLESKILLUSERATE;

	public short ANGERLYPOINT_MIN;

	public short ANGERLYPOINT_MAX;

	public short nInitiativeValue;

	public CHARKIND_STATINFO() : base(NrTableData.eResourceType.eRT_CHARKIND_STATINFO)
	{
		for (int i = 0; i < 3; i++)
		{
			this.kBattleSkillData[i] = new BATTLESKILL_DATA();
		}
		this.Init();
	}

	public void Init()
	{
		this.CharCode = string.Empty;
		for (int i = 0; i < 3; i++)
		{
			this.kBattleSkillData[i].Init();
		}
		this.BATTLESKILLUSERATE = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.CharCode);
		for (int i = 0; i < 3; i++)
		{
			row.GetColumn(num++, out this.kBattleSkillData[i].BattleSkillUnique);
			row.GetColumn(num++, out this.kBattleSkillData[i].BattleSkillLevel);
		}
		row.GetColumn(num++, out this.BATTLESKILLUSERATE);
		row.GetColumn(num++, out this.ANGERLYPOINT_MIN);
		row.GetColumn(num++, out this.ANGERLYPOINT_MAX);
		row.GetColumn(num++, out this.nInitiativeValue);
	}
}
