using System;
using TsLibs;

public class BATTLESKILL_BASE : NrTableData
{
	public int m_nSkillUnique;

	public string m_strTextKey = string.Empty;

	public string m_waSkillName = string.Empty;

	public int m_nSkillMaxLevel;

	public int m_nMythSkillType;

	public int m_nSkillType;

	public int m_nSkillRange;

	public int m_nSkillTargetType;

	public int m_nSkillJobType;

	public int m_nSkillGridType;

	public long m_nSkillTargetWeaponType;

	public int m_nSkilNeedWeapon;

	public float m_nSkillMoveRange;

	public string m_strSkillCameraShake = string.Empty;

	public string m_nSkillIconCode = string.Empty;

	public string[] m_nBuffIconCode = new string[2];

	public string m_strSkillBulletCode = string.Empty;

	public int m_nSkillAniSequenceCode;

	public string m_strSkillCasterEffectCode = string.Empty;

	public string m_strSkillEnemyCastEffectCode = string.Empty;

	public string m_strSkillTargetEffectCode = string.Empty;

	public string m_strSkillTargetEndureEffectCode = string.Empty;

	public string m_strSkillGridEffectCode = string.Empty;

	public string m_strSkillHitCenterGridEffectCode = string.Empty;

	public string m_strSkillBuffEffectCode = string.Empty;

	public string m_strBuffEndEffectCode = string.Empty;

	public int m_nSkillUseDetailInclude;

	public int m_nSkillBuffType;

	public int m_nSkillAiType;

	public int m_nSkillItemType;

	public string m_nSkillDESC = string.Empty;

	public int m_nSkillNoUseTurn;

	public int m_nColosseumSkillDesc;

	public string[] m_nSkillDescSub = new string[6];

	public string m_strParserWeaphoneType = string.Empty;

	public string m_strParserCharAniType = string.Empty;

	public string m_strParserBuffType = string.Empty;

	public string m_strParserSkillAniType = string.Empty;

	public string m_strParserTargetWeaponType = string.Empty;

	public BATTLESKILL_BASE()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nSkillUnique = 0;
		this.m_strTextKey = string.Empty;
		this.m_waSkillName = string.Empty;
		this.m_nSkillMaxLevel = 0;
		this.m_nMythSkillType = 0;
		this.m_nSkillType = 0;
		this.m_nSkillRange = 0;
		this.m_nSkillTargetType = 0;
		this.m_nSkillJobType = 0;
		this.m_nSkillGridType = 0;
		this.m_nSkillTargetWeaponType = 0L;
		this.m_nSkilNeedWeapon = 0;
		this.m_nSkillMoveRange = 0f;
		this.m_strSkillCameraShake = string.Empty;
		this.m_nSkillIconCode = string.Empty;
		for (int i = 0; i < 2; i++)
		{
			this.m_nBuffIconCode[i] = string.Empty;
		}
		this.m_strSkillBulletCode = string.Empty;
		this.m_nSkillAniSequenceCode = 0;
		this.m_strSkillCasterEffectCode = string.Empty;
		this.m_strSkillEnemyCastEffectCode = string.Empty;
		this.m_strSkillTargetEffectCode = string.Empty;
		this.m_strSkillTargetEndureEffectCode = string.Empty;
		this.m_strSkillGridEffectCode = string.Empty;
		this.m_strSkillHitCenterGridEffectCode = string.Empty;
		this.m_strSkillBuffEffectCode = string.Empty;
		this.m_strBuffEndEffectCode = string.Empty;
		this.m_nSkillUseDetailInclude = 0;
		this.m_nSkillBuffType = 0;
		this.m_nSkillAiType = 0;
		this.m_nSkillItemType = 0;
		this.m_nSkillDESC = string.Empty;
		this.m_nSkillNoUseTurn = 0;
		this.m_nColosseumSkillDesc = 0;
		for (int j = 0; j < 6; j++)
		{
			this.m_nSkillDescSub[j] = string.Empty;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		string empty = string.Empty;
		int num = 0;
		int num2 = 0;
		row.GetColumn(num2++, out this.m_nSkillUnique);
		row.GetColumn(num2++, out this.m_strTextKey);
		row.GetColumn(num2++, out this.m_waSkillName);
		row.GetColumn(num2++, out this.m_nSkillMaxLevel);
		row.GetColumn(num2++, out this.m_nMythSkillType);
		row.GetColumn(num2++, out empty);
		this.m_nSkillType = (int)NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetCharSkillType(empty);
		row.GetColumn(num2++, out this.m_nSkillRange);
		row.GetColumn(num2++, out empty);
		this.m_nSkillTargetType = (int)NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetCharTargetType(empty);
		row.GetColumn(num2++, out empty);
		this.m_nSkillJobType = (int)NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetBattleSkillJobType(empty);
		row.GetColumn(num2++, out empty);
		this.m_nSkillGridType = (int)NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetCharGridType(empty);
		row.GetColumn(num2++, out this.m_strParserTargetWeaponType);
		row.GetColumn(num2++, out this.m_strParserWeaphoneType);
		row.GetColumn(num2++, out num);
		this.m_nSkillMoveRange = (float)num / 10f;
		row.GetColumn(num2++, out this.m_strSkillCameraShake);
		row.GetColumn(num2++, out this.m_nSkillIconCode);
		row.GetColumn(num2++, out this.m_nBuffIconCode[0]);
		row.GetColumn(num2++, out this.m_nBuffIconCode[1]);
		row.GetColumn(num2++, out this.m_strSkillBulletCode);
		row.GetColumn(num2++, out this.m_strParserCharAniType);
		row.GetColumn(num2++, out this.m_strSkillCasterEffectCode);
		row.GetColumn(num2++, out this.m_strSkillEnemyCastEffectCode);
		row.GetColumn(num2++, out this.m_strSkillTargetEffectCode);
		row.GetColumn(num2++, out this.m_strSkillTargetEndureEffectCode);
		row.GetColumn(num2++, out this.m_strSkillGridEffectCode);
		row.GetColumn(num2++, out this.m_strSkillHitCenterGridEffectCode);
		row.GetColumn(num2++, out this.m_strSkillBuffEffectCode);
		row.GetColumn(num2++, out this.m_strBuffEndEffectCode);
		row.GetColumn(num2++, out this.m_nSkillUseDetailInclude);
		row.GetColumn(num2++, out this.m_strParserBuffType);
		row.GetColumn(num2++, out this.m_strParserSkillAniType);
		row.GetColumn(num2++, out this.m_nSkillItemType);
		row.GetColumn(num2++, out this.m_nSkillDESC);
		row.GetColumn(num2++, out this.m_nSkillNoUseTurn);
		row.GetColumn(num2++, out this.m_nColosseumSkillDesc);
		row.GetColumn(num2++, out this.m_nSkillDescSub[0]);
		row.GetColumn(num2++, out this.m_nSkillDescSub[1]);
		row.GetColumn(num2++, out this.m_nSkillDescSub[2]);
		row.GetColumn(num2++, out this.m_nSkillDescSub[3]);
		row.GetColumn(num2++, out this.m_nSkillDescSub[4]);
		row.GetColumn(num2++, out this.m_nSkillDescSub[5]);
	}

	public string GetBSkillHitCenterGridEffectCode()
	{
		if (string.IsNullOrEmpty(this.m_strSkillHitCenterGridEffectCode) || this.m_strSkillHitCenterGridEffectCode == "0")
		{
			this.m_strSkillHitCenterGridEffectCode = string.Empty;
		}
		return this.m_strSkillHitCenterGridEffectCode;
	}

	public string GetBSkillEnemyCastEffect()
	{
		if (string.IsNullOrEmpty(this.m_strSkillEnemyCastEffectCode) || this.m_strSkillEnemyCastEffectCode == "0")
		{
			this.m_strSkillEnemyCastEffectCode = string.Empty;
		}
		return this.m_strSkillEnemyCastEffectCode;
	}

	public string GetBSkillCameraShake()
	{
		if (string.IsNullOrEmpty(this.m_strSkillCameraShake) || this.m_strSkillCameraShake == "0")
		{
			this.m_strSkillCameraShake = string.Empty;
		}
		return this.m_strSkillCameraShake;
	}

	public string GetBSkillBuffEffectCode()
	{
		if (string.IsNullOrEmpty(this.m_strSkillBuffEffectCode) || this.m_strSkillBuffEffectCode == "0")
		{
			this.m_strSkillBuffEffectCode = string.Empty;
		}
		return this.m_strSkillBuffEffectCode;
	}

	public string GetBSkillTargetEffectCode()
	{
		if (string.IsNullOrEmpty(this.m_strSkillTargetEffectCode) || this.m_strSkillTargetEffectCode == "0")
		{
			this.m_strSkillTargetEffectCode = string.Empty;
		}
		return this.m_strSkillTargetEffectCode;
	}

	public string GetBSkillTargetEndureEffectCode()
	{
		if (string.IsNullOrEmpty(this.m_strSkillTargetEndureEffectCode) || this.m_strSkillTargetEndureEffectCode == "0")
		{
			this.m_strSkillTargetEndureEffectCode = string.Empty;
		}
		return this.m_strSkillTargetEndureEffectCode;
	}

	public string GetBSkillCasterEffectCode()
	{
		if (string.IsNullOrEmpty(this.m_strSkillCasterEffectCode) || this.m_strSkillCasterEffectCode == "0")
		{
			this.m_strSkillCasterEffectCode = string.Empty;
		}
		return this.m_strSkillCasterEffectCode;
	}

	public string GetBSkillGridEffectCode()
	{
		if (string.IsNullOrEmpty(this.m_strSkillGridEffectCode) || this.m_strSkillGridEffectCode == "0")
		{
			this.m_strSkillGridEffectCode = string.Empty;
		}
		return this.m_strSkillGridEffectCode;
	}

	public string GetBSkillBuffEndEffectCode()
	{
		if (string.IsNullOrEmpty(this.m_strBuffEndEffectCode) || this.m_strBuffEndEffectCode == "0")
		{
			this.m_strBuffEndEffectCode = string.Empty;
		}
		return this.m_strBuffEndEffectCode;
	}

	public bool ChecJobTypeMove()
	{
		return this.m_nSkillJobType == 5 || this.m_nSkillJobType == 6 || this.m_nSkillJobType == 7 || this.m_nSkillJobType == 8 || this.ChecJobTypeHalfMove();
	}

	public bool ChecJobTypeHalfMove()
	{
		return this.m_nSkillJobType == 9 || this.m_nSkillJobType == 10 || this.m_nSkillJobType == 11 || this.m_nSkillJobType == 12;
	}

	public bool ChecJobTypeBullet()
	{
		return this.m_nSkillJobType == 3 || this.m_nSkillJobType == 4 || this.m_nSkillJobType == 7 || this.m_nSkillJobType == 8 || this.m_nSkillJobType == 10 || this.m_nSkillJobType == 9;
	}

	public bool ChecJobTypeMagicDamage()
	{
		return this.m_nSkillJobType == 4 || this.m_nSkillJobType == 2 || this.m_nSkillJobType == 8 || this.m_nSkillJobType == 6 || this.m_nSkillJobType == 10 || this.m_nSkillJobType == 12;
	}

	public bool CheckTargetWeaponType(long TargetWeaponType)
	{
		return this.m_nSkillTargetWeaponType != 0L && TargetWeaponType > 0L && (TargetWeaponType == 1L || (this.m_nSkillTargetWeaponType & TargetWeaponType) > 0L);
	}
}
