using GAME;
using System;
using TsLibs;

public class CHARKIND_ATTACKINFO : NrTableData
{
	public int ATTACKTYPE;

	public string ATTACKCODE = string.Empty;

	public string WEAPONCODE = string.Empty;

	public string JOBTYPE = string.Empty;

	public string BulletCode = string.Empty;

	public string HitEffectCode = string.Empty;

	public byte ATTACKGRID;

	public byte ATTACKINTERVAL;

	public byte MOVERANGE;

	public short ATTACKRANGE;

	public byte SIGHTRANGE;

	public byte CANATTACKRANGE;

	public byte MINACTIVEPOWER;

	public byte MAXACTIVEPOWER;

	public float fAttackInterval;

	public float fMoveRange;

	public float fAttackRange;

	public float fSightRange;

	public int nWeaponType;

	public byte nJobType;

	public CHARKIND_ATTACKINFO() : base(NrTableData.eResourceType.eRT_CHARKIND_ATTACKINFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.ATTACKTYPE = 0;
		this.ATTACKCODE = string.Empty;
		this.WEAPONCODE = string.Empty;
		this.JOBTYPE = string.Empty;
		this.BulletCode = string.Empty;
		this.HitEffectCode = string.Empty;
		this.ATTACKGRID = 0;
		this.ATTACKINTERVAL = 0;
		this.MOVERANGE = 0;
		this.ATTACKRANGE = 0;
		this.SIGHTRANGE = 0;
		this.fAttackInterval = 0f;
		this.fMoveRange = 0f;
		this.fAttackRange = 0f;
		this.fSightRange = 0f;
		this.nWeaponType = 0;
		this.nJobType = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.ATTACKTYPE);
		row.GetColumn(num++, out this.ATTACKCODE);
		row.GetColumn(num++, out this.WEAPONCODE);
		row.GetColumn(num++, out this.JOBTYPE);
		row.GetColumn(num++, out this.BulletCode);
		row.GetColumn(num++, out this.HitEffectCode);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.ATTACKINTERVAL);
		row.GetColumn(num++, out this.MOVERANGE);
		row.GetColumn(num++, out this.ATTACKRANGE);
		row.GetColumn(num++, out this.SIGHTRANGE);
		row.GetColumn(num++, out this.CANATTACKRANGE);
		row.GetColumn(num++, out this.MINACTIVEPOWER);
		row.GetColumn(num++, out this.MAXACTIVEPOWER);
		if (Enum.IsDefined(typeof(E_ATTACK_GRID_TYPE), empty))
		{
			this.ATTACKGRID = (byte)((int)Enum.Parse(typeof(E_ATTACK_GRID_TYPE), empty));
		}
		this.fAttackInterval = (float)this.ATTACKINTERVAL / 10f;
		this.fMoveRange = (float)this.MOVERANGE / 10f;
		this.fAttackRange = (float)this.ATTACKRANGE / 10f;
		this.fSightRange = (float)this.SIGHTRANGE / 10f;
	}
}
