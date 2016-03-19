using System;

public class CHARKIND_SOLSTATINFO
{
	public int STR;

	public int DEX;

	public int INT;

	public int VIT;

	public int HP;

	public int MIN_DAMAGE;

	public int MAX_DAMAGE;

	public int DEFENSE;

	public int MAGICDEFENSE;

	public int HITRATE;

	public int EVASION;

	public int CRITICAL;

	public CHARKIND_SOLSTATINFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.STR = 0;
		this.DEX = 0;
		this.INT = 0;
		this.VIT = 0;
		this.HP = 0;
		this.MIN_DAMAGE = 0;
		this.MAX_DAMAGE = 0;
		this.DEFENSE = 0;
		this.MAGICDEFENSE = 0;
		this.HITRATE = 0;
		this.EVASION = 0;
		this.CRITICAL = 0;
	}

	public void Set(ref CHARKIND_SOLSTATINFO pkSOLSTATINFO)
	{
		this.STR = pkSOLSTATINFO.STR;
		this.DEX = pkSOLSTATINFO.DEX;
		this.INT = pkSOLSTATINFO.INT;
		this.VIT = pkSOLSTATINFO.VIT;
		this.HP = pkSOLSTATINFO.HP;
		this.MIN_DAMAGE = pkSOLSTATINFO.MIN_DAMAGE;
		this.MAX_DAMAGE = pkSOLSTATINFO.MAX_DAMAGE;
		this.DEFENSE = pkSOLSTATINFO.DEFENSE;
		this.MAGICDEFENSE = pkSOLSTATINFO.MAGICDEFENSE;
		this.HITRATE = pkSOLSTATINFO.HITRATE;
		this.EVASION = pkSOLSTATINFO.EVASION;
		this.CRITICAL = pkSOLSTATINFO.CRITICAL;
	}
}
