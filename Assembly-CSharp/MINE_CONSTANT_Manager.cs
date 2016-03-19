using GAME;
using System;
using TsLibs;

public class MINE_CONSTANT_Manager : NrTableBase
{
	private int[] m_MineConstant = new int[45];

	private static MINE_CONSTANT_Manager Instance;

	private NkValueParse<eMINE_CONSTANT> m_kConstantCode;

	public MINE_CONSTANT_Manager(string strFilePath) : base(strFilePath)
	{
		this.m_kConstantCode = new NkValueParse<eMINE_CONSTANT>();
		this.SetConstantCode();
	}

	public static MINE_CONSTANT_Manager GetInstance()
	{
		if (MINE_CONSTANT_Manager.Instance == null)
		{
			MINE_CONSTANT_Manager.Instance = new MINE_CONSTANT_Manager(CDefinePath.MineConstantURL);
		}
		return MINE_CONSTANT_Manager.Instance;
	}

	public void SetConstantCode()
	{
		this.m_kConstantCode.InsertCodeValue("MINE_GIVE_TIME", eMINE_CONSTANT.eMINE_CONSTANT_GIVE_TIME);
		this.m_kConstantCode.InsertCodeValue("GIVEITEM_BASE_RATE", eMINE_CONSTANT.eMINE_CONSTANT_BASE_GIVEITEM_RATE);
		this.m_kConstantCode.InsertCodeValue("GIVEITEM_RANK1_RATE", eMINE_CONSTANT.eMINE_CONSTANT_GIVEITEM_RANK1_RATE);
		this.m_kConstantCode.InsertCodeValue("GIVEITEM_RANK2_RATE", eMINE_CONSTANT.eMINE_CONSTANT_GIVEITEM_RANK2_RATE);
		this.m_kConstantCode.InsertCodeValue("GIVEITEM_RANK3_RATE", eMINE_CONSTANT.eMINE_CONSTANT_GIVEITEM_RANK3_RATE);
		this.m_kConstantCode.InsertCodeValue("GIVEITEM_RANK4_RATE", eMINE_CONSTANT.eMINE_CONSTANT_GIVEITEM_RANK4_RATE);
		this.m_kConstantCode.InsertCodeValue("GIVEITEM_RANK5_RATE", eMINE_CONSTANT.eMINE_CONSTANT_GIVEITEM_RANK5_RATE);
		this.m_kConstantCode.InsertCodeValue("MINE_MOVE_TIME", eMINE_CONSTANT.eMINE_CONSTANT_MINE_MOVE_TIME);
		this.m_kConstantCode.InsertCodeValue("MINE_WAIT_TIME", eMINE_CONSTANT.eMINE_CONSTANT_BATTLE_END_WAITTIME);
		this.m_kConstantCode.InsertCodeValue("MINE_RETURN_TIME", eMINE_CONSTANT.eMINE_CONSTANT_MINE_RETURN_TIME);
		this.m_kConstantCode.InsertCodeValue("MINE_REMAKE_TIME", eMINE_CONSTANT.eMINE_CONSTANT_REMAKE_TIME);
		this.m_kConstantCode.InsertCodeValue("MINE_EXP", eMINE_CONSTANT.eMINE_CONSTANT_MINE_EXP);
		this.m_kConstantCode.InsertCodeValue("MINE_SOL_LV", eMINE_CONSTANT.eMINE_CONSTANT_MINE_SOL_LV);
		this.m_kConstantCode.InsertCodeValue("MINE_SUPPORT_TIME", eMINE_CONSTANT.eMINE_CONSTANT_MINE_SUPPORT_TIME);
		this.m_kConstantCode.InsertCodeValue("MINE_DAY_COUNT", eMINE_CONSTANT.eMINE_DAY_COUNT);
		this.m_kConstantCode.InsertCodeValue("MINE_TUTORIAL_ITEMUNIQUE", eMINE_CONSTANT.eMINE_CONSTANT_TUTORIAL_ITEMUNIQUE);
		this.m_kConstantCode.InsertCodeValue("MINE_TUTORIAL_ITEMNUM", eMINE_CONSTANT.eMINE_CONSTANT_TUTORIAL_ITEMNUM);
		this.m_kConstantCode.InsertCodeValue("GIVEITEM_SMALL_RATE", eMINE_CONSTANT.eMINE_CONSTANT_GIVEITEM_SMALL_RATE);
		this.m_kConstantCode.InsertCodeValue("GIVEITEM_MEDIUM_RATE", eMINE_CONSTANT.eMINE_CONSTANT_GIVEITEM_MEDIUM_RATE);
		this.m_kConstantCode.InsertCodeValue("GIVEITEM_LARGE_RATE", eMINE_CONSTANT.eMINE_CONSTANT_GIVEITEM_LARGE_RATE);
		this.m_kConstantCode.InsertCodeValue("GUILD_POINT_MINE_SMALL", eMINE_CONSTANT.eMINE_CONSTANT_GUILD_POINT_MINE_SMALL);
		this.m_kConstantCode.InsertCodeValue("GUILD_POINT_MINE_MEDIUM", eMINE_CONSTANT.eMINE_CONSTANT_GUILD_POINT_MINE_MEDIUM);
		this.m_kConstantCode.InsertCodeValue("GUILD_POINT_MINE_LARGE", eMINE_CONSTANT.eMINE_CONSTANT_GUILD_POINT_MINE_LARGE);
		this.m_kConstantCode.InsertCodeValue("MINE_AUTO_CREATECOUNT", eMINE_CONSTANT.eMINE_CONSTANT_SET_MINE_AUTOCOUNT);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_START_DAY", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_START_DAY);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_START_HOUR", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_START_HOUR);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_START_MIMUTE", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_START_MIMUTE);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_PLAY_DAY", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_PLAY_DAY);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_PLAY_HOUR", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_PLAY_HOUR);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_PLAY_MIMUTE", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_PLAY_MIMUTE);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_END_DAY", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_END_DAY);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_END_HOUR", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_END_HOUR);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_END_MIMUTE", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_END_MIMUTE);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_RESULT_DAY", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_RESULT_DAY);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_RESULT_HOUR", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_RESULT_HOUR);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_RESULT_MIMUTE", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_RESULT_MIMUTE);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_HONOR_POINT_SMALL", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_HONOR_POINT_SMALL);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_HONOR_POINT_MEDIUM", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_HONOR_POINT_MEDIUM);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_HONOR_POINT_LARGE", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_HONOR_POINT_LARGE);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_HONOR_POINT_LARGEST", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_HONOR_POINT_LARGEST);
		this.m_kConstantCode.InsertCodeValue("DECLAREWAR_MINE_LIMIT", eMINE_CONSTANT.eMINE_CONSTANT_DECLAREWAR_MINE_LIMIT);
	}

	public eMINE_CONSTANT GetConstantCode(string strConstantConde)
	{
		return this.m_kConstantCode.GetValue(strConstantConde);
	}

	public void SetData(eMINE_CONSTANT eConst, int value)
	{
		if (eConst < (eMINE_CONSTANT)0 || eConst >= eMINE_CONSTANT.eMINE_CONSTANT_MAX)
		{
			return;
		}
		this.m_MineConstant[(int)eConst] = value;
	}

	public int GetValue(eMINE_CONSTANT Const)
	{
		if (Const < (eMINE_CONSTANT)0 || Const >= eMINE_CONSTANT.eMINE_CONSTANT_MAX)
		{
			return 0;
		}
		return this.m_MineConstant[(int)Const];
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			MINE_CONSTANT mINE_CONSTANT = new MINE_CONSTANT();
			mINE_CONSTANT.SetData(data);
			mINE_CONSTANT.m_eConstant = this.GetConstantCode(mINE_CONSTANT.strConstant);
			this.SetData(mINE_CONSTANT.m_eConstant, mINE_CONSTANT.m_nConstant);
		}
		return true;
	}
}
