using GAME;
using System;
using TsLibs;

public class COLOSSEUM_CONSTANT_Manager : NrTableBase
{
	private int[] m_CommonConstant = new int[12];

	private static COLOSSEUM_CONSTANT_Manager Instance;

	private NkValueParse<eCOLOSSEUM_CONSTANT> m_kConstantCode;

	private COLOSSEUM_CONSTANT_Manager(string strFilePath) : base(strFilePath)
	{
		this.m_kConstantCode = new NkValueParse<eCOLOSSEUM_CONSTANT>();
		this.SetConstantCode();
	}

	public static COLOSSEUM_CONSTANT_Manager GetInstance()
	{
		if (COLOSSEUM_CONSTANT_Manager.Instance == null)
		{
			COLOSSEUM_CONSTANT_Manager.Instance = new COLOSSEUM_CONSTANT_Manager(CDefinePath.COLOSSEUM_CONSTANT_URL);
		}
		return COLOSSEUM_CONSTANT_Manager.Instance;
	}

	public void SetConstantCode()
	{
		this.m_kConstantCode.InsertCodeValue("COLOSSEUM_LV", eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_CHECK_LEVEL);
		this.m_kConstantCode.InsertCodeValue("BRONZE_UPGRADECOUNT", eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_BRONZE_UPGRADE_WINCOUNT);
		this.m_kConstantCode.InsertCodeValue("UPGRADE_RATE", eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_UPGRADE_RATE);
		this.m_kConstantCode.InsertCodeValue("DOWNGRADE_RATE", eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_DOWNGRADE_RATE);
		this.m_kConstantCode.InsertCodeValue("RANKMATCH_REWARD_DAYLIMIT", eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_ONEDAY_GIVEITEM_LIMITCOUNT);
		this.m_kConstantCode.InsertCodeValue("LOBBY_TIMELIMIT", eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_LOBBY_TIMELIMIT);
	}

	public eCOLOSSEUM_CONSTANT GetConstantCode(string strConstantConde)
	{
		return this.m_kConstantCode.GetValue(strConstantConde);
	}

	public void SetData(eCOLOSSEUM_CONSTANT eConst, int value)
	{
		if (eConst < eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_MATCH_RANGE || eConst >= eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_MAX)
		{
			return;
		}
		this.m_CommonConstant[(int)eConst] = value;
	}

	public int GetValue(eCOLOSSEUM_CONSTANT Const)
	{
		if (Const < eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_MATCH_RANGE || Const >= eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_MAX)
		{
			return 0;
		}
		return this.m_CommonConstant[(int)Const];
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			COLOSSEUM_CONSTANT cOLOSSEUM_CONSTANT = new COLOSSEUM_CONSTANT();
			cOLOSSEUM_CONSTANT.SetData(data);
			cOLOSSEUM_CONSTANT.m_eConstant = this.GetConstantCode(cOLOSSEUM_CONSTANT.strConstant);
			this.SetData(cOLOSSEUM_CONSTANT.m_eConstant, cOLOSSEUM_CONSTANT.m_nConstant);
		}
		return true;
	}
}
