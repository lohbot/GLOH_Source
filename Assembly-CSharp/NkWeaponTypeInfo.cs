using System;

public class NkWeaponTypeInfo
{
	private WEAPONTYPE_INFO m_pkWEAPONTYPE_INFO;

	private int m_nWeaponType;

	private long m_nATB;

	private string m_szName = string.Empty;

	public NkWeaponTypeInfo(NkWeaponTypeTempData pkTempData)
	{
		this.m_pkWEAPONTYPE_INFO = pkTempData.m_pkWEAPONTYPE_INFO;
		this.m_nWeaponType = 0;
		this.m_nATB = 0L;
		this.m_szName = string.Empty;
	}

	public void SetWeaponTypeInfo(int weapontype, ref WEAPONTYPE_INFO weapontypeinfo, long atb)
	{
		this.m_pkWEAPONTYPE_INFO = weapontypeinfo;
		this.m_nWeaponType = weapontype;
		this.m_nATB = atb;
		if (string.IsNullOrEmpty(this.m_szName))
		{
			this.m_szName = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(this.m_pkWEAPONTYPE_INFO.TEXTKEY);
		}
	}

	public WEAPONTYPE_INFO GetWEAPONTYPE_INFO()
	{
		return this.m_pkWEAPONTYPE_INFO;
	}

	public bool IsValid()
	{
		return this.m_nWeaponType != 0;
	}

	public int GetWeaponType()
	{
		return this.m_nWeaponType;
	}

	public string GetCode()
	{
		return this.m_pkWEAPONTYPE_INFO.WEAPONCODE;
	}

	public string GetName()
	{
		return this.m_szName;
	}

	public bool IsATB(long flag)
	{
		return (this.m_nATB & flag) != 0L;
	}

	public byte GetEquipCount()
	{
		return this.m_pkWEAPONTYPE_INFO.EQUIPCOUNT;
	}

	public string GetBattleDummy()
	{
		return this.m_pkWEAPONTYPE_INFO.BATTLEDUMMY;
	}

	public string GetBackDummy()
	{
		return this.m_pkWEAPONTYPE_INFO.BACKDUMMY;
	}
}
