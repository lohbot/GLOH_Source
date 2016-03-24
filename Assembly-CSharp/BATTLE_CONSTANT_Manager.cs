using System;
using System.Collections.Generic;
using TsLibs;

public class BATTLE_CONSTANT_Manager : NrTableBase
{
	private float[] m_BattleConstant = new float[166];

	private static BATTLE_CONSTANT_Manager Instance;

	private NkValueParse<eBATTLE_CONSTANT> m_kConstantCode;

	private SortedDictionary<int, DEFENSE_DATA> m_dicDefence_Data;

	private BATTLE_CONSTANT_Manager(string strFilePath) : base(strFilePath)
	{
		this.m_kConstantCode = new NkValueParse<eBATTLE_CONSTANT>();
		this.m_dicDefence_Data = new SortedDictionary<int, DEFENSE_DATA>();
		this.SetConstantCode();
	}

	public static BATTLE_CONSTANT_Manager GetInstance()
	{
		if (BATTLE_CONSTANT_Manager.Instance == null)
		{
			BATTLE_CONSTANT_Manager.Instance = new BATTLE_CONSTANT_Manager(CDefinePath.BATTLE_CONSTANT_URL);
		}
		return BATTLE_CONSTANT_Manager.Instance;
	}

	public void SetConstantCode()
	{
		this.m_kConstantCode.InsertCodeValue("STATDAMAGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_STATDAMAGE);
		this.m_kConstantCode.InsertCodeValue("BASEDAMAGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASEDAMAGE);
		this.m_kConstantCode.InsertCodeValue("STATDEFENCE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_STATDEFENCE);
		this.m_kConstantCode.InsertCodeValue("LEVELDEFENCE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_LEVELDEFENCE);
		this.m_kConstantCode.InsertCodeValue("DEFENCECONSTANT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_DEFENCECONSTANT);
		this.m_kConstantCode.InsertCodeValue("BASEHITRATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASEHITRATE);
		this.m_kConstantCode.InsertCodeValue("RELATIVEHITRATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_RELATIVEHITRATE);
		this.m_kConstantCode.InsertCodeValue("MINDAMAGERATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MINDAMAGERATE);
		this.m_kConstantCode.InsertCodeValue("LEVELHITRATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_LEVELHITRATE);
		this.m_kConstantCode.InsertCodeValue("ITEMCRITICAL", eBATTLE_CONSTANT.eBATTLE_CONSTANT_ITEMCRITICAL);
		this.m_kConstantCode.InsertCodeValue("CRITICALRATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_CRITICALRATE);
		this.m_kConstantCode.InsertCodeValue("BASECRITICAL", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASECRITICAL);
		this.m_kConstantCode.InsertCodeValue("BASEHP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASEHP);
		this.m_kConstantCode.InsertCodeValue("STATHP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_STATHP);
		this.m_kConstantCode.InsertCodeValue("HPCONSTANT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_HPCONSTANT);
		this.m_kConstantCode.InsertCodeValue("LEVELHP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_LEVELHP);
		this.m_kConstantCode.InsertCodeValue("BASEMP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASEMP);
		this.m_kConstantCode.InsertCodeValue("STATMP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_STATMP);
		this.m_kConstantCode.InsertCodeValue("MPCONSTANT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MPCONSTANT);
		this.m_kConstantCode.InsertCodeValue("LEVELMP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_LEVELMP);
		this.m_kConstantCode.InsertCodeValue("SKILLUSERATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_SKILLUSERATE);
		this.m_kConstantCode.InsertCodeValue("ITEMDAMAGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_ITEMDAMAGE);
		this.m_kConstantCode.InsertCodeValue("ITEMDEFENCE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_ITEMDEFENCE);
		this.m_kConstantCode.InsertCodeValue("AGGRO", eBATTLE_CONSTANT.eBATTLE_CONSTANT_AGGRO);
		this.m_kConstantCode.InsertCodeValue("AGGRORANGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_AGGRORANGE);
		this.m_kConstantCode.InsertCodeValue("AGGROHP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_AGGROHP);
		this.m_kConstantCode.InsertCodeValue("AGGRORATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_AGGRORATE);
		this.m_kConstantCode.InsertCodeValue("LEVELDAMAGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_LEVELDAMAGE);
		this.m_kConstantCode.InsertCodeValue("BP_ATK", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_ATK);
		this.m_kConstantCode.InsertCodeValue("BP_PDEF", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_PDEF);
		this.m_kConstantCode.InsertCodeValue("BP_HIT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_HIT);
		this.m_kConstantCode.InsertCodeValue("BP_DODGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_DODGE);
		this.m_kConstantCode.InsertCodeValue("BP_CRITICAL", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_CRITICAL);
		this.m_kConstantCode.InsertCodeValue("BP_STR", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_STR);
		this.m_kConstantCode.InsertCodeValue("BP_DEX", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_DEX);
		this.m_kConstantCode.InsertCodeValue("BP_INT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_INT);
		this.m_kConstantCode.InsertCodeValue("BP_VIT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_VIT);
		this.m_kConstantCode.InsertCodeValue("BP_WIS", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_WIS);
		this.m_kConstantCode.InsertCodeValue("BP_HP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_HP);
		this.m_kConstantCode.InsertCodeValue("BP_MP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_MP);
		this.m_kConstantCode.InsertCodeValue("BP_CORRECT1", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_CORRECT1);
		this.m_kConstantCode.InsertCodeValue("BP_CORRECT2", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_CORRECT2);
		this.m_kConstantCode.InsertCodeValue("TREASUREMOKET", eBATTLE_CONSTANT.eBATTLE_CONSTANT_TREASUREMOKET);
		this.m_kConstantCode.InsertCodeValue("BASECONSUME", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASECONSUME);
		this.m_kConstantCode.InsertCodeValue("LEVELCONSUME", eBATTLE_CONSTANT.eBATTLE_CONSTANT_LEVELCONSUME);
		this.m_kConstantCode.InsertCodeValue("CHAR_TURN_INTERVAL", eBATTLE_CONSTANT.eBATTLE_CONSTANT_CHAR_TURN_INTERVAL);
		this.m_kConstantCode.InsertCodeValue("RECOVERYRATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_RECOVERYRATE);
		this.m_kConstantCode.InsertCodeValue("MONSPECIALRATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MONSPECIALRATE);
		this.m_kConstantCode.InsertCodeValue("BOSSSPECIALRATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BOSSSPECIALRATE);
		this.m_kConstantCode.InsertCodeValue("SPECIALBC", eBATTLE_CONSTANT.eBATTLE_CONSTANT_SPECIALBC);
		this.m_kConstantCode.InsertCodeValue("SPECIALMK", eBATTLE_CONSTANT.eBATTLE_CONSTANT_SPECIALMK);
		this.m_kConstantCode.InsertCodeValue("SPECIALLV", eBATTLE_CONSTANT.eBATTLE_CONSTANT_SPECIALLV);
		this.m_kConstantCode.InsertCodeValue("COMBOHUMAN", eBATTLE_CONSTANT.eBATTLE_CONSTANT_COMBOHUMAN);
		this.m_kConstantCode.InsertCodeValue("COMBOFURRY", eBATTLE_CONSTANT.eBATTLE_CONSTANT_COMBOFURRY);
		this.m_kConstantCode.InsertCodeValue("COMBOELF", eBATTLE_CONSTANT.eBATTLE_CONSTANT_COMBOELF);
		this.m_kConstantCode.InsertCodeValue("COMBOMONSTER", eBATTLE_CONSTANT.eBATTLE_CONSTANT_COMBOMONSTER);
		this.m_kConstantCode.InsertCodeValue("RUNAWAY_ACTIVE_HP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_RUNAWAY_ACTIVE_HP);
		this.m_kConstantCode.InsertCodeValue("RUNAWAY_LAST_HP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_RUNAWAY_LAST_HP);
		this.m_kConstantCode.InsertCodeValue("RUNAWAY_PER", eBATTLE_CONSTANT.eBATTLE_CONSTANT_RUNAWAY_PER);
		this.m_kConstantCode.InsertCodeValue("BOSSDEF_PER", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BOSSDEF_PER);
		this.m_kConstantCode.InsertCodeValue("MOKETMAKELEVEL", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MOKETMAKELEVEL);
		this.m_kConstantCode.InsertCodeValue("MOKETMAKETURN", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MOKETMAKETURN);
		this.m_kConstantCode.InsertCodeValue("MOKETMAKEENDTURN", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MOKETMAKEENDTURN);
		this.m_kConstantCode.InsertCodeValue("MOKETRUNTURN", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MOKETRUNTURN);
		this.m_kConstantCode.InsertCodeValue("NORMAL_ATTACK_ANGERLY_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_ATTACK_ANGERLY_RATE);
		this.m_kConstantCode.InsertCodeValue("ENDURE_DAMAGE_ALGERLY_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_ENDURE_ANGERLY_RATE);
		this.m_kConstantCode.InsertCodeValue("GRID_ONE_ANGERLY_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_GRID_ONE_ANGERLY_RATE);
		this.m_kConstantCode.InsertCodeValue("GRID_TWO_ANGERLY_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_GRID_TWO_ANGERLY_RATE);
		this.m_kConstantCode.InsertCodeValue("GRID_SIDE_ANGERLY_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_GRID_SIDE_ANGERLY_RATE);
		this.m_kConstantCode.InsertCodeValue("GRID_LINE_ANGERLY_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_GRID_LINE_ANGERLY_RATE);
		this.m_kConstantCode.InsertCodeValue("GRID_COLUMN_ANGERLY_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_GRID_COLUMN_ANGERLY_RATE);
		this.m_kConstantCode.InsertCodeValue("GRID_ALL_ANGERLY_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_GRID_ALL_ANGERLY_RATE);
		this.m_kConstantCode.InsertCodeValue("GRID_CROSS_ANGERLY_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_GRID_CROSS_ANGERLY_RATE);
		this.m_kConstantCode.InsertCodeValue("MAX_CONTINUE_BATTLE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MAX_CONTINUE_BATTLE);
		this.m_kConstantCode.InsertCodeValue("DEAD_ANGERLY_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_DEAD_ANGERLY_RATE);
		this.m_kConstantCode.InsertCodeValue("IMMUNE_SKILL_COUNT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_IMMUNE_SKILL_COUNT);
		this.m_kConstantCode.InsertCodeValue("BATTLESKILL_AI_HEALTYPE_HPRATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLESKILL_AI_HEALTYPE_HPRATE);
		this.m_kConstantCode.InsertCodeValue("ADD_BATTLESKILLRATE_FOR_ANGERLY", eBATTLE_CONSTANT.eBATTLE_CONSTANT_ADD_BATTLESKILLRATE_FOR_ANGERLY);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_EXP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_EXP);
		this.m_kConstantCode.InsertCodeValue("FRIENDHELP_EXP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_FRIENDHELP_EXP);
		this.m_kConstantCode.InsertCodeValue("BATTLE_SUPPORT_GOLD", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLE_SUPPORT_GOLD);
		this.m_kConstantCode.InsertCodeValue("TURN_CONTROL_DELAY", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLE_TURN_CONTROL_DELAY);
		this.m_kConstantCode.InsertCodeValue("NEWAI", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLE_NEWAI);
		this.m_kConstantCode.InsertCodeValue("NEWAI_USERLEVEL", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLE_NEWAI_USERLEVEL);
		this.m_kConstantCode.InsertCodeValue("NEWAI_MONLEVEL", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLE_NEWAI_MONLEVEL);
		this.m_kConstantCode.InsertCodeValue("BABEL_DROP_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BABEL_DROP_RATE);
		this.m_kConstantCode.InsertCodeValue("BABEL_RANKTURN", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BABEL_RANKTURN);
		this.m_kConstantCode.InsertCodeValue("BABEL_RANKLEVEL", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BABEL_RANKLEVEL);
		this.m_kConstantCode.InsertCodeValue("BABEL_RANKADDLEVEL", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BABEL_RANKADDLEVEL);
		this.m_kConstantCode.InsertCodeValue("BABEL_RANKANGER", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BABEL_RANKANGER);
		this.m_kConstantCode.InsertCodeValue("SKILL_DESC_TIME", eBATTLE_CONSTANT.eBATTLE_CONSTANT_SKILL_DESC_TIME);
		this.m_kConstantCode.InsertCodeValue("LIMIT_EXP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_LIMIT_EXP);
		this.m_kConstantCode.InsertCodeValue("LIMIT_MINDAMAGE_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_LIMIT_MINDAMAGE_RATE);
		this.m_kConstantCode.InsertCodeValue("LIMIT_MAXDAMAGE_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_LIMIT_MAXDAMAGE_RATE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_MELLE_MINDAMAGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_MELLE_MINDAMAGE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_MELLE_MAXDAMAGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_MELLE_MAXDAMAGE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_MELLE_ADDHP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_MELLE_ADDHP);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_MELLE_PHYSICALDEFENSE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_MELLE_PHYSICALDEFENSE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_MELLE_MAGICDEFENSE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_MELLE_MAGICDEFENSE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_RANGE_MINDAMAGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_RANGE_MINDAMAGE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_RANGE_MAXDAMAGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_RANGE_MAXDAMAGE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_RANGE_ADDHP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_RANGE_ADDHP);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_RANGE_PHYSICALDEFENSE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_RANGE_PHYSICALDEFENSE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_RANGE_MAGICDEFENSE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_RANGE_MAGICDEFENSE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_MAGIC_MINDAMAGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_MAGIC_MINDAMAGE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_MAGIC_MAXDAMAGE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_MAGIC_MAXDAMAGE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_MAGIC_ADDHP", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_MAGIC_ADDHP);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_MAGIC_PHYSICALDEFENSE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_MAGIC_PHYSICALDEFENSE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_MAGIC_MAGICDEFENSE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_MAGIC_MAGICDEFENSE);
		this.m_kConstantCode.InsertCodeValue("PLUNDER_REVISION_CONSTANT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_PLUNDER_REVISION_CONSTANT);
		this.m_kConstantCode.InsertCodeValue("MULTI_GRID_ONE_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MULTI_GRID_ONE_RATE);
		this.m_kConstantCode.InsertCodeValue("MULTI_GRID_TWO_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MULTI_GRID_TWO_RATE);
		this.m_kConstantCode.InsertCodeValue("MULTI_GRID_SIDE_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MULTI_GRID_SIDE_RATE);
		this.m_kConstantCode.InsertCodeValue("MULTI_GRID_LINE_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MULTI_GRID_LINE_RATE);
		this.m_kConstantCode.InsertCodeValue("MULTI_GRID_COLUMN_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MULTI_GRID_COLUMN_RATE);
		this.m_kConstantCode.InsertCodeValue("MULTI_GRID_CROSS_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MULTI_GRID_CROSS_RATE);
		this.m_kConstantCode.InsertCodeValue("MULTI_GRID_SQUARE_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MULTI_GRID_SQUARE_RATE);
		this.m_kConstantCode.InsertCodeValue("MULTI_GRID_ALL_RATE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MULTI_GRID_ALL_RATE);
		this.m_kConstantCode.InsertCodeValue("MAX_PLUNDER_ANGERLYPOINT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MAX_PLUNDER_ANGERLYPOINT);
		this.m_kConstantCode.InsertCodeValue("BASE_ANGERLY_POINT_FIRST", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASE_ANGERLY_POINT_FIRST);
		this.m_kConstantCode.InsertCodeValue("BASE_ANGERLY_POINT_SECOUND", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASE_ANGERLY_POINT_SECOUND);
		this.m_kConstantCode.InsertCodeValue("RESURRECTION_COUNT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_RESURRECTION_COUNT);
		this.m_kConstantCode.InsertCodeValue("AGGROTARGET_COUNT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_AGGROTARGET_COUNT);
		this.m_kConstantCode.InsertCodeValue("AGGROTARGET_VALUE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_AGGROTARGET_VALUE);
		this.m_kConstantCode.InsertCodeValue("MAX_ADDANGERLYPOINT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLESKILL_MAX_ADDANGERLYPOINT);
		this.m_kConstantCode.InsertCodeValue("MIN_ADDANGERLYPOINT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLESKILL_MIN_ADDANGERLYPOINT);
		this.m_kConstantCode.InsertCodeValue("CC_RESIST_COUNT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_CC_RESIST_COUNT);
		this.m_kConstantCode.InsertCodeValue("MYTHRAID_EMERGENCY_COUNT", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_EMERGENCY_COUNT);
		this.m_kConstantCode.InsertCodeValue("MYTHRAID_ANGELPOINT_USEANGER", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_ANGELPOINT_USEANGER);
		this.m_kConstantCode.InsertCodeValue("MYTHRAID_ANGELPOINT_USEANGER_2", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_ANGELPOINT_USEANGER_2);
		this.m_kConstantCode.InsertCodeValue("MYTHRAID_ANGELPOINT_USEANGER_3", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_ANGELPOINT_USEANGER_3);
		this.m_kConstantCode.InsertCodeValue("MYTHRAID_ANGELPOINT_USEANGER_4", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_ANGELPOINT_USEANGER_4);
		this.m_kConstantCode.InsertCodeValue("MYTHRAID_ANGELPOINT_CHARDIE", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_ANGELPOINT_CHARDIE);
		this.m_kConstantCode.InsertCodeValue("MYTHRAID_ANGELPOINT_KILLMONSTER", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_ANGELPOINT_KILLMONSTER);
		this.m_kConstantCode.InsertCodeValue("MYTHRAID_ANGELPOINT_MAX", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_ANGELPOINT_MAX);
		this.m_kConstantCode.InsertCodeValue("MYTHRAID_ANGELPOINT_START", eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_ANGELPOINT_START);
	}

	public eBATTLE_CONSTANT GetConstantCode(string strConstantConde)
	{
		return this.m_kConstantCode.GetValue(strConstantConde);
	}

	public void SetData(eBATTLE_CONSTANT eConst, float value)
	{
		if (eConst < (eBATTLE_CONSTANT)0 || eConst >= eBATTLE_CONSTANT.eBATTLE_CONSTANT_MAX)
		{
			return;
		}
		this.m_BattleConstant[(int)eConst] = value;
	}

	public void SetData(DEFENSE_DATA pkData)
	{
		this.m_dicDefence_Data.Add(pkData.m_nIDX, pkData);
	}

	public float GetValue(eBATTLE_CONSTANT Const)
	{
		if (Const < (eBATTLE_CONSTANT)0 || Const >= eBATTLE_CONSTANT.eBATTLE_CONSTANT_MAX)
		{
			return 0f;
		}
		return this.m_BattleConstant[(int)Const];
	}

	public int GetDefense(int nDefenseValue)
	{
		int result = 0;
		int key = 0;
		foreach (DEFENSE_DATA current in this.m_dicDefence_Data.Values)
		{
			if (nDefenseValue < current.m_nDEFENSE_VALUE)
			{
				DEFENSE_DATA dEFENSE_DATA = null;
				if (this.m_dicDefence_Data.TryGetValue(key, out dEFENSE_DATA))
				{
					result = dEFENSE_DATA.m_nDAMAGE_DECREASE + (int)((float)(nDefenseValue - dEFENSE_DATA.m_nDEFENSE_VALUE) * dEFENSE_DATA.m_fDAMAGE_RATE);
					break;
				}
			}
			key = current.m_nIDX;
		}
		return result;
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLE_CONSTANT bATTLE_CONSTANT = new BATTLE_CONSTANT();
			bATTLE_CONSTANT.SetData(data);
			bATTLE_CONSTANT.m_eConstant = BATTLE_CONSTANT_Manager.GetInstance().GetConstantCode(bATTLE_CONSTANT.strConstant);
			this.SetData(bATTLE_CONSTANT.m_eConstant, bATTLE_CONSTANT.m_nConstant);
		}
		dr.BeginSection("[DEFENSE]");
		foreach (TsDataReader.Row data2 in dr)
		{
			DEFENSE_DATA dEFENSE_DATA = new DEFENSE_DATA();
			dEFENSE_DATA.SetData(data2);
			this.SetData(dEFENSE_DATA);
		}
		return true;
	}
}
