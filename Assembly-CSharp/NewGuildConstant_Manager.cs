using GAME;
using System;
using TsLibs;

public class NewGuildConstant_Manager : NrTableBase
{
	private NkValueParse<eNEWGUILD_CONSTANT> m_kConstantCode;

	private NEWGUILD_CONSTANT[] m_GuildConstant = new NEWGUILD_CONSTANT[24];

	private static NewGuildConstant_Manager Instance;

	private NewGuildConstant_Manager() : base(CDefinePath.NEWGUILD_CONSTANT)
	{
		this.Init();
		this.SetConstantCode();
	}

	public static NewGuildConstant_Manager GetInstance()
	{
		if (NewGuildConstant_Manager.Instance == null)
		{
			NewGuildConstant_Manager.Instance = new NewGuildConstant_Manager();
		}
		return NewGuildConstant_Manager.Instance;
	}

	public void Init()
	{
		this.m_kConstantCode = new NkValueParse<eNEWGUILD_CONSTANT>();
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		string empty = string.Empty;
		long value = 0L;
		foreach (TsDataReader.Row row in dr)
		{
			NEWGUILD_CONSTANT nEWGUILD_CONSTANT = new NEWGUILD_CONSTANT();
			row.GetColumn(0, out empty);
			row.GetColumn(1, out value);
			nEWGUILD_CONSTANT.SetData(this.GetConstantCode(empty), value);
			this.SetData(nEWGUILD_CONSTANT);
		}
		return true;
	}

	public void SetConstantCode()
	{
		this.m_kConstantCode.InsertCodeValue("GUILD_CREATE_MONEY", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_CREATE_MONEY);
		this.m_kConstantCode.InsertCodeValue("GUILD_REQ_LEVEL_FOR_CREATE", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_LEVEL_FOR_CREATE);
		this.m_kConstantCode.InsertCodeValue("GUILD_MAX_NUM", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_MAX_NUM);
		this.m_kConstantCode.InsertCodeValue("GUILD_MAX_MEMBER_NUM", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_MAX_MEMBER_NUM);
		this.m_kConstantCode.InsertCodeValue("GUILD_MAX_APPLICANTS_NUM", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_MAX_APPLICANTS_NUM);
		this.m_kConstantCode.InsertCodeValue("GUILD_NEWMASTER_CHECK_DAY", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_NEWMASTER_CHECK_DAY);
		this.m_kConstantCode.InsertCodeValue("GUILD_EXP_GET_RATE", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_EXP_GET_RATE);
		this.m_kConstantCode.InsertCodeValue("NEWBIE_LIMIT_TIME", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_NEWBIE_LIMIT_TIME);
		this.m_kConstantCode.InsertCodeValue("GUILD_POST_TEX", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_POST_TEX);
		this.m_kConstantCode.InsertCodeValue("GUILD_ADMIN_NUM", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_ADMIN_NUM);
		this.m_kConstantCode.InsertCodeValue("GUILDBOSS_CONTRIBUTION", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_BOSSBATTLECONTRIBUTE);
		this.m_kConstantCode.InsertCodeValue("GUILDBOSS_CLEARPOINT", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_BOSS_CLEARPOINT);
		this.m_kConstantCode.InsertCodeValue("GUILDBOSS_OPENLIMIT_START", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_BOSSBATTLE_OPENLIMIT_START);
		this.m_kConstantCode.InsertCodeValue("GUILDBOSS_OPENLIMIT_END", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_BOSSBATTLE_OPENLIMIT_END);
		this.m_kConstantCode.InsertCodeValue("GUILDBOSS_PLAYLIMIT_START", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_BOSSBATTLE_PLAYLIMIT_START);
		this.m_kConstantCode.InsertCodeValue("GUILDBOSS_PLAYLIMIT_END", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_BOSSBATTLE_PLAYLIMIT_END);
		this.m_kConstantCode.InsertCodeValue("GUILDMONEY_GET", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_FUNDGET);
		this.m_kConstantCode.InsertCodeValue("GUILDMONEY_EXCHANGE_RATE", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_GUILDMONEY_EXCHANGE_RATE);
		this.m_kConstantCode.InsertCodeValue("GUILDMONEY_DONATE", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_GUILDMONEY_DONATE);
		this.m_kConstantCode.InsertCodeValue("MERCHANT_HOLDING_TIME", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_MERCHANT_HOLDING_TIME);
		this.m_kConstantCode.InsertCodeValue("GUILDAGIT_OUTPOS", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_GUILDAGIT_OUTPOS);
		this.m_kConstantCode.InsertCodeValue("NPC_HOLDING_TIME", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_NPC_HOLDING_TIME);
		this.m_kConstantCode.InsertCodeValue("MON_TAMING_TIME", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_MON_TAMING_TIME);
		this.m_kConstantCode.InsertCodeValue("AGIT_MAX_LEVEL", eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_AGIT_MAX_LEVEL);
	}

	public eNEWGUILD_CONSTANT GetConstantCode(string strConstantConde)
	{
		return this.m_kConstantCode.GetValue(strConstantConde);
	}

	private void SetData(NEWGUILD_CONSTANT kData)
	{
		this.m_GuildConstant[(int)kData.eConstant] = kData;
	}

	public NEWGUILD_CONSTANT GetData(eNEWGUILD_CONSTANT eConstant)
	{
		return this.m_GuildConstant[(int)eConstant];
	}
}
