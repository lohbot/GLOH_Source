using System;
using System.Collections.Generic;
using TsLibs;

public class BATTLE_EMOTICON_Manager : NrTableBase
{
	private static BATTLE_EMOTICON_Manager Instance;

	private NkValueParse<eBATTLE_EMOTICON> m_kConstantCode;

	private Dictionary<eBATTLE_EMOTICON, BATTLE_EMOTICON> m_dicEmoticonData;

	private BATTLE_EMOTICON_Manager(string strFilePath) : base(strFilePath)
	{
		this.m_kConstantCode = new NkValueParse<eBATTLE_EMOTICON>();
		this.m_dicEmoticonData = new Dictionary<eBATTLE_EMOTICON, BATTLE_EMOTICON>();
		this.SetConstantCode();
	}

	public static BATTLE_EMOTICON_Manager GetInstance()
	{
		if (BATTLE_EMOTICON_Manager.Instance == null)
		{
			BATTLE_EMOTICON_Manager.Instance = new BATTLE_EMOTICON_Manager(CDefinePath.BATTLE_EMOTICON_URL);
		}
		return BATTLE_EMOTICON_Manager.Instance;
	}

	public void SetConstantCode()
	{
		this.m_kConstantCode.InsertCodeValue("ATTACK", eBATTLE_EMOTICON.eBATTLE_EMOTICON_ATTACK);
		this.m_kConstantCode.InsertCodeValue("HEAL", eBATTLE_EMOTICON.eBATTLE_EMOTICON_HEAL);
		this.m_kConstantCode.InsertCodeValue("IGNORE", eBATTLE_EMOTICON.eBATTLE_EMOTICON_IGNORE);
		this.m_kConstantCode.InsertCodeValue("ANGER", eBATTLE_EMOTICON.eBATTLE_EMOTICON_ANGER);
		this.m_kConstantCode.InsertCodeValue("MAZZ", eBATTLE_EMOTICON.eBATTLE_EMOTICON_MAZZ);
		this.m_kConstantCode.InsertCodeValue("ANGRY", eBATTLE_EMOTICON.eBATTLE_EMOTICON_ANGRY);
		this.m_kConstantCode.InsertCodeValue("NERVOUS", eBATTLE_EMOTICON.eBATTLE_EMOTICON_NERVOUS);
		this.m_kConstantCode.InsertCodeValue("SMILE", eBATTLE_EMOTICON.eBATTLE_EMOTICON_SMILE);
		this.m_kConstantCode.InsertCodeValue("THANK", eBATTLE_EMOTICON.eBATTLE_EMOTICON_THANK);
	}

	public eBATTLE_EMOTICON GetConstantCode(string strConstantConde)
	{
		return this.m_kConstantCode.GetValue(strConstantConde);
	}

	public void SetData(BATTLE_EMOTICON Data)
	{
		if (Data.m_eConstant < eBATTLE_EMOTICON.eBATTLE_EMOTICON_ATTACK || Data.m_eConstant >= eBATTLE_EMOTICON.eBATTLE_EMOTICON_MAX)
		{
			return;
		}
		this.m_dicEmoticonData.Add(Data.m_eConstant, Data);
	}

	public BATTLE_EMOTICON GetData(eBATTLE_EMOTICON Const)
	{
		if (Const < eBATTLE_EMOTICON.eBATTLE_EMOTICON_ATTACK || Const >= eBATTLE_EMOTICON.eBATTLE_EMOTICON_MAX)
		{
			return null;
		}
		return this.m_dicEmoticonData[Const];
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLE_EMOTICON bATTLE_EMOTICON = new BATTLE_EMOTICON();
			bATTLE_EMOTICON.SetData(data);
			bATTLE_EMOTICON.m_eConstant = this.GetConstantCode(bATTLE_EMOTICON.strConstant);
			this.SetData(bATTLE_EMOTICON);
		}
		return true;
	}
}
