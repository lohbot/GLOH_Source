using System;

public class NkSolStatInfo
{
	public int m_nSumSTR;

	public int m_nSumDEX;

	public int m_nSumINT;

	public int m_nSumVIT;

	public int m_nHitRate;

	public int m_nEvasion;

	public int m_nCritical;

	public int m_nCriticalInfoUI;

	public int m_nAttackSpeed;

	public int m_nPhysicalDefense;

	public float m_fPhysicalDefenseRate;

	public int m_nMagicDefense;

	public float m_fMagicDefenseRate;

	public int m_nMinDamage;

	public int m_nMaxDamage;

	public int m_nRecoveryHP;

	public int m_nMaxHP;

	public int m_nBS_SumSTR;

	public int m_nBS_SumDEX;

	public int m_nBS_SumINT;

	public int m_nBS_SumVIT;

	public int m_nBS_HitRate;

	public int m_nBS_Evasion;

	public int m_nBS_Critical;

	public int m_nBS_AttackSpeed;

	public int m_nBS_PhysicalDefense;

	public int m_nBS_MagicDefense;

	public int m_nBS_MinDamage;

	public int m_nBS_MaxDamage;

	public int m_nBS_RecoveryHP;

	public int m_nBS_MaxHP;

	public NkSolStatInfo()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nSumSTR = 0;
		this.m_nSumDEX = 0;
		this.m_nSumINT = 0;
		this.m_nSumVIT = 0;
		this.m_nHitRate = 0;
		this.m_nEvasion = 0;
		this.m_nCritical = 0;
		this.m_nCriticalInfoUI = 0;
		this.m_nAttackSpeed = 0;
		this.m_nPhysicalDefense = 0;
		this.m_fPhysicalDefenseRate = 0f;
		this.m_nMagicDefense = 0;
		this.m_fMagicDefenseRate = 0f;
		this.m_nMinDamage = 0;
		this.m_nMaxDamage = 0;
		this.m_nRecoveryHP = 0;
		this.m_nMaxHP = 0;
		this.m_nBS_SumSTR = 0;
		this.m_nBS_SumDEX = 0;
		this.m_nBS_SumINT = 0;
		this.m_nBS_SumVIT = 0;
		this.m_nBS_HitRate = 0;
		this.m_nBS_Evasion = 0;
		this.m_nBS_Critical = 0;
		this.m_nBS_AttackSpeed = 0;
		this.m_nBS_PhysicalDefense = 0;
		this.m_nBS_MagicDefense = 0;
		this.m_nBS_MinDamage = 0;
		this.m_nBS_MaxDamage = 0;
		this.m_nBS_RecoveryHP = 0;
		this.m_nBS_MaxHP = 0;
	}

	public void Init_BS_Stat()
	{
		this.m_nBS_SumSTR = 0;
		this.m_nBS_SumDEX = 0;
		this.m_nBS_SumINT = 0;
		this.m_nBS_SumVIT = 0;
		this.m_nBS_HitRate = 0;
		this.m_nBS_Evasion = 0;
		this.m_nBS_Critical = 0;
		this.m_nBS_AttackSpeed = 0;
		this.m_nBS_PhysicalDefense = 0;
		this.m_nBS_MagicDefense = 0;
		this.m_nBS_MinDamage = 0;
		this.m_nBS_MaxDamage = 0;
		this.m_nBS_RecoveryHP = 0;
		this.m_nBS_MaxHP = 0;
	}
}
