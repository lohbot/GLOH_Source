using GAME;
using System;
using TsLibs;

public class BATTLESKILL_DETAIL : NrTableData
{
	public int m_nSkillUnique;

	public int m_nSkillLevel;

	public int m_nSkillNeedAngerlyPoint;

	public int m_nSkillSuccessRate;

	public int m_nSkillDurationTurn;

	public int m_nSkillDurationRate;

	public int m_nSkillBuffTarget;

	public int m_nSkillAggroAdd;

	public int m_nSkillAggroDamagePer;

	public string m_nSkillTooltip = string.Empty;

	public int[] m_nSkillDetalParamType = new int[10];

	public int[] m_nSkillDetalParamValue = new int[10];

	public int m_nSkillDetailATB;

	public string m_strParserBuffType = string.Empty;

	public BATTLESKILL_DETAIL()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nSkillUnique = 0;
		this.m_nSkillLevel = 0;
		this.m_nSkillNeedAngerlyPoint = 0;
		this.m_nSkillSuccessRate = 0;
		this.m_nSkillDurationTurn = 0;
		this.m_nSkillDurationRate = 0;
		this.m_nSkillBuffTarget = 0;
		this.m_nSkillAggroAdd = 0;
		this.m_nSkillAggroDamagePer = 0;
		this.m_nSkillTooltip = string.Empty;
		for (int i = 0; i < 10; i++)
		{
			this.m_nSkillDetalParamType[i] = 0;
			this.m_nSkillDetalParamValue[i] = 0;
		}
		this.m_nSkillDetailATB = 0;
	}

	public void Set(int SkillLevelIndex, BATTLESKILL_DETAIL LastdetailData, BATTLESKILL_DETAILINCLUDE detailData_I)
	{
		this.m_nSkillUnique = LastdetailData.m_nSkillUnique;
		this.m_nSkillLevel = SkillLevelIndex;
		this.m_nSkillNeedAngerlyPoint = LastdetailData.m_nSkillNeedAngerlyPoint + detailData_I.m_nSkillNeedAngerlyPoint_I;
		this.m_nSkillSuccessRate = LastdetailData.m_nSkillSuccessRate + detailData_I.m_nSkillSuccessRate_I;
		this.m_nSkillDurationTurn = LastdetailData.m_nSkillDurationTurn + detailData_I.m_nSkillDurationTurn_I;
		this.m_nSkillDurationRate = LastdetailData.m_nSkillDurationRate + detailData_I.m_nSkillDurationRate_I;
		this.m_nSkillAggroAdd = LastdetailData.m_nSkillAggroAdd + detailData_I.m_nSkillAggroAdd_I;
		this.m_nSkillAggroDamagePer = LastdetailData.m_nSkillAggroDamagePer + detailData_I.m_nSkillAggroDamagePer_I;
		this.m_nSkillTooltip = LastdetailData.m_nSkillTooltip;
		for (int i = 0; i < 10; i++)
		{
			int num = LastdetailData.m_nSkillDetalParamValue[i];
			if (num > 0)
			{
				this.m_nSkillDetalParamType[i] = LastdetailData.m_nSkillDetalParamType[i];
				this.m_nSkillDetalParamValue[i] = num + detailData_I.GetSkillDetalIncludeParamValue(this.m_nSkillDetalParamType[i]);
			}
		}
	}

	public int GetSkillDetalParamValue(int detailParamType)
	{
		for (int i = 0; i < 10; i++)
		{
			if (this.m_nSkillDetalParamType[i] == detailParamType)
			{
				return this.m_nSkillDetalParamValue[i];
			}
		}
		return 0;
	}

	public bool CheckSkillDetailATB(int skillATB)
	{
		return (this.m_nSkillDetailATB & skillATB) > 0;
	}

	public void SetSkillDetailATBData(int skillATB)
	{
		this.m_nSkillDetailATB |= skillATB;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.m_nSkillUnique);
		row.GetColumn(num++, out this.m_nSkillLevel);
		row.GetColumn(num++, out this.m_nSkillNeedAngerlyPoint);
		row.GetColumn(num++, out this.m_nSkillSuccessRate);
		row.GetColumn(num++, out this.m_nSkillDurationTurn);
		row.GetColumn(num++, out this.m_nSkillDurationRate);
		row.GetColumn(num++, out this.m_strParserBuffType);
		row.GetColumn(num++, out this.m_nSkillAggroAdd);
		row.GetColumn(num++, out this.m_nSkillAggroDamagePer);
		row.GetColumn(num++, out this.m_nSkillTooltip);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType[0] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue[0]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType[1] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue[1]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType[2] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue[2]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType[3] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue[3]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType[4] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue[4]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType[5] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue[5]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType[6] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue[6]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType[7] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue[7]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType[8] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue[8]);
		row.GetColumn(num++, out empty);
		this.m_nSkillDetalParamType[9] = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(empty);
		row.GetColumn(num++, out this.m_nSkillDetalParamValue[9]);
	}

	public void SetSkillDetailATB()
	{
		this.m_nSkillDetailATB = 0;
		for (int i = 0; i < 10; i++)
		{
			if (this.m_nSkillDetalParamValue[i] > 0 && this.m_nSkillDetalParamType[i] > 0)
			{
				eBATTLESKILL_DETAIL_CODE eBATTLESKILL_DETAIL_CODE = (eBATTLESKILL_DETAIL_CODE)this.m_nSkillDetalParamType[i];
				switch (eBATTLESKILL_DETAIL_CODE)
				{
				case eBATTLESKILL_DETAIL_CODE.ENDURE_HEAL:
				case eBATTLESKILL_DETAIL_CODE.ENDURE_HEAL_P:
					goto IL_14F;
				case eBATTLESKILL_DETAIL_CODE.ENDURE_ANGERLYPOINT:
				case eBATTLESKILL_DETAIL_CODE.AFTER_USED_DAMAGE_TYPE:
				case eBATTLESKILL_DETAIL_CODE.AFTER_USED_DAMAGE_TARGET:
				case eBATTLESKILL_DETAIL_CODE.DEL_BUFF_ALL:
				case eBATTLESKILL_DETAIL_CODE.DEL_BUFF_TYPE:
				case eBATTLESKILL_DETAIL_CODE.ACTION_HP_P:
				case eBATTLESKILL_DETAIL_CODE.SUICIDE:
				case eBATTLESKILL_DETAIL_CODE.REVIVAL:
					IL_7C:
					switch (eBATTLESKILL_DETAIL_CODE)
					{
					case eBATTLESKILL_DETAIL_CODE.BLOOD_SUCKING_PER:
					case eBATTLESKILL_DETAIL_CODE.BLOOD_SUCKING_VALUE:
					case eBATTLESKILL_DETAIL_CODE.ADD_BLOOD_SUCKING_PER:
					case eBATTLESKILL_DETAIL_CODE.ADD_BLOOD_SUCKING_VALUE:
					case eBATTLESKILL_DETAIL_CODE.BLOOD_SUCKING_ALL_PER:
					case eBATTLESKILL_DETAIL_CODE.BLOOD_SUCKING_ALL_VALUE:
						if (!this.CheckSkillDetailATB(4))
						{
							this.SetSkillDetailATBData(4);
						}
						goto IL_19B;
					case eBATTLESKILL_DETAIL_CODE.PROTECT_SHIELD_TYPE:
					case eBATTLESKILL_DETAIL_CODE.PROTECT_SHIELD_VALUE:
					case eBATTLESKILL_DETAIL_CODE.PROTECT_SHIELD_VALUE_P:
					case eBATTLESKILL_DETAIL_CODE.SKILL_SHIELD_TYPE:
					case eBATTLESKILL_DETAIL_CODE.SKILL_SHIELD_BUFFTYPE:
						IL_B9:
						switch (eBATTLESKILL_DETAIL_CODE)
						{
						case eBATTLESKILL_DETAIL_CODE.NOTURN_ON:
						case eBATTLESKILL_DETAIL_CODE.NOSKILL_ON:
						case eBATTLESKILL_DETAIL_CODE.NOCONTROL_ON:
						case eBATTLESKILL_DETAIL_CODE.NOCONTROL_AND_NOTARGETING:
						case eBATTLESKILL_DETAIL_CODE.NONORMAL_ATTACK_ON:
							goto IL_11F;
						case eBATTLESKILL_DETAIL_CODE.NOATTACK_ON:
						case eBATTLESKILL_DETAIL_CODE.IMMORTAL_ON:
							IL_DE:
							switch (eBATTLESKILL_DETAIL_CODE)
							{
							case eBATTLESKILL_DETAIL_CODE.HEAL:
							case eBATTLESKILL_DETAIL_CODE.HEAL_P:
								goto IL_14F;
							case eBATTLESKILL_DETAIL_CODE.DAMAGE_P:
								IL_F2:
								if (eBATTLESKILL_DETAIL_CODE == eBATTLESKILL_DETAIL_CODE.SLEEP_ON)
								{
									goto IL_11F;
								}
								if (eBATTLESKILL_DETAIL_CODE != eBATTLESKILL_DETAIL_CODE.SUMMON_BARRIER_KIND)
								{
									goto IL_19B;
								}
								goto IL_181;
							}
							goto IL_F2;
						}
						goto IL_DE;
						IL_11F:
						if (!this.CheckSkillDetailATB(2))
						{
							this.SetSkillDetailATBData(2);
						}
						goto IL_19B;
					case eBATTLESKILL_DETAIL_CODE.ADD_ANGERLYPOINT:
					case eBATTLESKILL_DETAIL_CODE.PLUNDER_ANGERLYPOINT_P:
						if (!this.CheckSkillDetailATB(16))
						{
							this.SetSkillDetailATBData(16);
						}
						goto IL_19B;
					}
					goto IL_B9;
				case eBATTLESKILL_DETAIL_CODE.AFTER_USED_DAMAGE:
				case eBATTLESKILL_DETAIL_CODE.AFTER_USED_DAMAGE_P:
				case eBATTLESKILL_DETAIL_CODE.AFTER_USED_HEAL:
				case eBATTLESKILL_DETAIL_CODE.AFTER_USED_SKILL:
				case eBATTLESKILL_DETAIL_CODE.AFTER_USED_SKILL_LEVEL:
					if (!this.CheckSkillDetailATB(1))
					{
						this.SetSkillDetailATBData(1);
					}
					goto IL_19B;
				case eBATTLESKILL_DETAIL_CODE.SUMMON_KIND:
					goto IL_181;
				}
				goto IL_7C;
				IL_14F:
				if (!this.CheckSkillDetailATB(8))
				{
					this.SetSkillDetailATBData(8);
				}
				goto IL_19B;
				IL_181:
				if (!this.CheckSkillDetailATB(32))
				{
					this.SetSkillDetailATBData(32);
				}
			}
			IL_19B:;
		}
	}
}
