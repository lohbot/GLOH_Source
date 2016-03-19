using GAME;
using System;
using TsLibs;

public class EXPEDITION_CONSTANT_MANAGER : NrTableBase
{
	private int[] m_Expedition_Constant = new int[15];

	private static EXPEDITION_CONSTANT_MANAGER Instance;

	private NkValueParse<eEXPEDITION_CONSTANT> m_kConstantCode;

	public EXPEDITION_CONSTANT_MANAGER(string strFilePath) : base(strFilePath)
	{
		this.m_kConstantCode = new NkValueParse<eEXPEDITION_CONSTANT>();
		this.SetConstantCode();
	}

	public static EXPEDITION_CONSTANT_MANAGER GetInstance()
	{
		if (EXPEDITION_CONSTANT_MANAGER.Instance == null)
		{
			EXPEDITION_CONSTANT_MANAGER.Instance = new EXPEDITION_CONSTANT_MANAGER(CDefinePath.ExpeditionConstantURL);
		}
		return EXPEDITION_CONSTANT_MANAGER.Instance;
	}

	public void SetConstantCode()
	{
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_ATTACK_TIME", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_ATTACK_TIME);
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_MOVE_TIME", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_MOVE_TIME);
		this.m_kConstantCode.InsertCodeValue("EXPEDITON_RETURN_TIME", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_RETURN_TIME);
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_REMAKE_TIME", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_REMAKE_TIME);
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_EXP", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_EXP);
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_DAY_COUNT", eEXPEDITION_CONSTANT.eEXPEDITION_DAY_COUNT);
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_TUTORIAL_ITEMUNIQUE", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_TUTORIAL_ITEMUNIQUE);
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_TUTORIAL_ITEMNUM", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_TUTORIAL_ITEMNUM);
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_SMALL_EXP", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_SMALL_EXP);
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_MEDIUM_EXP", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_MEDIUM_EXP);
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_LARGE_EXP", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_LARGE_EXP);
		this.m_kConstantCode.InsertCodeValue("EXPEDITION_EXTRALARGE_EXP", eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_EXTRALARGE_EXP);
	}

	public eEXPEDITION_CONSTANT GetConstantCode(string strConstantConde)
	{
		return this.m_kConstantCode.GetValue(strConstantConde);
	}

	public void SetData(eEXPEDITION_CONSTANT eConst, int value)
	{
		if (eConst < (eEXPEDITION_CONSTANT)0 || eConst >= eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_MAX)
		{
			return;
		}
		this.m_Expedition_Constant[(int)eConst] = value;
	}

	public int GetValue(eEXPEDITION_CONSTANT Const)
	{
		if (Const < (eEXPEDITION_CONSTANT)0 || Const >= eEXPEDITION_CONSTANT.eEXPEDITION_CONSTANT_MAX)
		{
			return 0;
		}
		return this.m_Expedition_Constant[(int)Const];
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			EXPEDITION_CONSTANT eXPEDITION_CONSTANT = new EXPEDITION_CONSTANT();
			eXPEDITION_CONSTANT.SetData(data);
			eXPEDITION_CONSTANT.m_eConstant = this.GetConstantCode(eXPEDITION_CONSTANT.strConstant);
			this.SetData(eXPEDITION_CONSTANT.m_eConstant, eXPEDITION_CONSTANT.m_nConstant);
		}
		return true;
	}
}
