using System;

public class BATTLE_SKILL_PARSER : NrTSingleton<BATTLE_SKILL_PARSER>
{
	private NkValueParse<int> m_kBattleSkillDetailParatypeInfo;

	private NkValueParse<int> m_kParseCharBuffTargetType;

	private NkValueParse<byte> m_kCharJobTypeCode;

	private NkValueParse<byte> m_kCharSkillTypeCode;

	private NkValueParse<byte> m_kCharAutoTypeCode;

	private NkValueParse<byte> m_kCharTargetTypeCode;

	private NkValueParse<byte> m_kCharGridTypeCode;

	private BATTLE_SKILL_PARSER()
	{
		this._RegisterParser();
	}

	public void _RegisterParser()
	{
		this.m_kBattleSkillDetailParatypeInfo = new NkValueParse<int>();
		this.m_kParseCharBuffTargetType = new NkValueParse<int>();
		this.m_kCharJobTypeCode = new NkValueParse<byte>();
		this.m_kCharSkillTypeCode = new NkValueParse<byte>();
		this.m_kCharAutoTypeCode = new NkValueParse<byte>();
		this.m_kCharTargetTypeCode = new NkValueParse<byte>();
		this.m_kCharGridTypeCode = new NkValueParse<byte>();
		this.m_kCharJobTypeCode.InsertCodeValue("NOMOVENOBULLETPHYSICS", 1);
		this.m_kCharJobTypeCode.InsertCodeValue("NOMOVENOBULLETMAGIC", 2);
		this.m_kCharJobTypeCode.InsertCodeValue("NOMOVEBULLETPHYSICS", 3);
		this.m_kCharJobTypeCode.InsertCodeValue("NOMOVEBULLETMAGIC", 4);
		this.m_kCharJobTypeCode.InsertCodeValue("MOVENOBULLETPHYSICS", 5);
		this.m_kCharJobTypeCode.InsertCodeValue("MOVENOBULLETMAGIC", 6);
		this.m_kCharJobTypeCode.InsertCodeValue("MOVEBULLETPHYSICS", 7);
		this.m_kCharJobTypeCode.InsertCodeValue("MOVEBULLETMAGIC", 8);
		this.m_kCharJobTypeCode.InsertCodeValue("HALFMOVEBULLETPHYSICS", 9);
		this.m_kCharJobTypeCode.InsertCodeValue("HALFMOVEBULLETMAGIC", 10);
		this.m_kCharJobTypeCode.InsertCodeValue("HALFMOVENOBULLETPHYSICS", 11);
		this.m_kCharJobTypeCode.InsertCodeValue("HALFMOVENOBULLETMAGIC", 12);
		this.m_kCharSkillTypeCode.InsertCodeValue("Active", 1);
		this.m_kCharSkillTypeCode.InsertCodeValue("Passive", 2);
		this.m_kCharSkillTypeCode.InsertCodeValue("Aura", 3);
		this.m_kCharAutoTypeCode.InsertCodeValue("FALSE", 0);
		this.m_kCharAutoTypeCode.InsertCodeValue("TRUE", 1);
		this.m_kCharTargetTypeCode.InsertCodeValue("Self", 1);
		this.m_kCharTargetTypeCode.InsertCodeValue("Self+Friend", 2);
		this.m_kCharTargetTypeCode.InsertCodeValue("Enemy", 3);
		this.m_kCharTargetTypeCode.InsertCodeValue("ALL", 4);
		this.m_kCharGridTypeCode.InsertCodeValue("GRID_ONE", 0);
		this.m_kCharGridTypeCode.InsertCodeValue("GRID_SIDE", 1);
		this.m_kCharGridTypeCode.InsertCodeValue("GRID_LINE", 2);
		this.m_kCharGridTypeCode.InsertCodeValue("GRID_TWO", 3);
		this.m_kCharGridTypeCode.InsertCodeValue("GRID_COLUMN", 4);
		this.m_kCharGridTypeCode.InsertCodeValue("GRID_CROSS", 5);
		this.m_kCharGridTypeCode.InsertCodeValue("GRID_ALL", 6);
		this.m_kCharGridTypeCode.InsertCodeValue("GRID_SQUARE", 7);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PARA_NONE", 0);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("DAMAGE", 1);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("HEAL", 2);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_STR", 5);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_DEX", 6);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_INT", 7);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_VIT", 8);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_PHY_DEFENSE", 9);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_MAG_DEFENSE", 10);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_HIT_RATE", 11);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_EVASION", 12);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_CRITICAL", 13);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_MIN_DAMAGE", 14);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_MAX_DAMAGE", 15);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("NOMOVE_ON", 38);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("NOTURN_ON", 39);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("NOSKILL_ON", 40);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("NOATTACK_ON", 41);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("IMMORTAL_ON", 42);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("NOATTACK_ON", 44);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("NONORMAL_ATTACK_ON", 45);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_HEAL", 46);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_HEAL_P", 47);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PLUS_HEAL", 48);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PLUS_HEAL_P", 49);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_DAMAGE", 50);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_DAMAGE_P", 51);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PLUS_DAMAGE", 52);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PLUS_DAMAGE_P", 53);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_DAMAGE_POISON", 55);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_DAMAGE_FIRE", 56);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_DAMAGE_ICE", 57);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_DAMAGE_LIGHTNING", 58);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_DAMAGE_BLEEDING", 59);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_DAMAGE_POISON_P", 60);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_DAMAGE_FIRE_P", 61);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_DAMAGE_ICE_P", 62);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_DAMAGE_LIGHTNING_P", 63);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_DAMAGE_BLEEDING_P", 64);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_HEAL", 65);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("DAMAGE_P", 3);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("DEL_BUFF_TYPE", 76);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("DEL_BUFF_ALL", 75);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ACTION_HP_P", 77);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SUICIDE", 78);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("REVIVAL", 79);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SLEEP_ON", 54);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SUMMON_KIND", 80);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SUMMON_LEVEL", 81);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SUMMON_LEVEL_P", 82);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SUMMON_ADDSTATE_DAMAGE_P", 83);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SUMMON_ADDSTATE_PHYDEFENSE_P", 84);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SUMMON_ADDSTATE_MAGDEFENSE_P", 85);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SUMMON_ADDSTATE_MAXHP_P", 86);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PUSH_PULL_MODE", 87);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PUSH_PULL_COUNT", 88);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("BLOOD_SUCKING_PER", 89);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("BLOOD_SUCKING_VALUE", 90);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_BLOOD_SUCKING_PER", 91);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_BLOOD_SUCKING_VALUE", 92);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("BLOOD_SUCKING_ALL_PER", 93);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("BLOOD_SUCKING_ALL_VALUE", 94);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("AFTER_USED_DAMAGE", 68);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("AFTER_USED_DAMAGE_P", 69);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("AFTER_USED_DAMAGE_TYPE", 70);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("AFTER_USED_DAMAGE_TARGET", 71);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("AFTER_USED_HEAL", 72);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("AFTER_USED_SKILL", 73);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("AFTER_USED_SKILL_LEVEL", 74);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PROTECT_SHIELD_TYPE", 95);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PROTECT_SHIELD_VALUE", 96);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PROTECT_SHIELD_VALUE_P", 97);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("HEAL_P", 4);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ENDURE_HEAL_P", 66);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SKILL_SHIELD_TYPE", 98);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SKILL_SHIELD_BUFFTYPE", 99);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_ANGERLYPOINT", 100);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PLUNDER_ANGERLYPOINT_P", 101);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_USE_ANGERLYPOINT_P", 102);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_USE_ANGERLYPOINT_P", 103);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MARKING_ON", 110);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("LIFE_LINK_ON", 111);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("WEAKPOINT_ON", 112);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("WEAKPOINT_ON_ONE", 113);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("DEL_BUFF_TYPE_ONE", 118);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SELFTARGET_LINK_SKILL", 114);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SELFTARGET_LINK_LEVEL", 115);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MAINTARGET_LINK_SKILL", 116);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MAINTARGET_LINK_LEVEL", 117);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("EMPTY_VALUE", 119);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("EMPTY_P", 120);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("EMPTY_DAMAGE_P", 121);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_PHY_DEFENSE_P", 17);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_MAG_DEFENSE_P", 18);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_HIT_RATE_P", 19);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_EVASION_P", 20);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_CRITICAL_P", 21);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_MIN_DAMAGE_P", 22);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_MAX_DAMAGE_P", 23);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_MAX_HP_P", 24);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_MAX_HP", 16);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_MIN_DAMAGE", 25);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_MAX_DAMAGE", 26);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_MIN_DAMAGE_P", 27);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_MAX_DAMAGE_P", 28);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_PHY_DEFENSE", 29);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_MAG_DEFENSE", 30);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_PHY_DEFENSE_P", 31);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_MAG_DEFENSE_P", 32);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_MAX_HP", 33);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_MAX_HP_P", 34);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("MINUS_CRITICAL", 35);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PLUS_MIN_DAMAGE_P", 36);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("PLUS_MAX_DAMAGE_P", 37);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("IMMUNE_NOTURN_ON", 104);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("IMMUNE_NOSKILL_ON", 105);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("IMMUNE_NOCONTROL_ON", 106);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("ADD_BERSERK_DAMAGE_P", 107);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SUMMON_BARRIER_KIND", 108);
		this.m_kBattleSkillDetailParatypeInfo.InsertCodeValue("SUMMON_BARRIER_LEVEL", 109);
		this.m_kParseCharBuffTargetType.InsertCodeValue("ALL", 0);
		this.m_kParseCharBuffTargetType.InsertCodeValue("MAIN", 1);
	}

	public byte GetCharSkillType(string datacode)
	{
		return this.m_kCharSkillTypeCode.GetValue(datacode);
	}

	public byte GetCharAutoType(string datacode)
	{
		return this.m_kCharAutoTypeCode.GetValue(datacode);
	}

	public byte GetCharTargetType(string datacode)
	{
		return this.m_kCharTargetTypeCode.GetValue(datacode);
	}

	public byte GetCharGridType(string datacode)
	{
		return this.m_kCharGridTypeCode.GetValue(datacode);
	}

	public int GetParamType(string detailParaType)
	{
		return this.m_kBattleSkillDetailParatypeInfo.GetValue(detailParaType);
	}

	public int GetBuffTargetTypeType(string BuffTargetType)
	{
		return this.m_kParseCharBuffTargetType.GetValue(BuffTargetType);
	}

	public byte GetBattleSkillJobType(string datacode)
	{
		return this.m_kCharJobTypeCode.GetValue(datacode);
	}
}
