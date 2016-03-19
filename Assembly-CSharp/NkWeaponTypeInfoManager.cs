using System;
using System.Collections.Generic;

public class NkWeaponTypeInfoManager : NrTSingleton<NkWeaponTypeInfoManager>
{
	public NkWeaponTypeInfo[] m_kWeaponTypeInfo;

	private NkWeaponTypeTempData m_kTempData;

	private NkWeaponDataCodeInfo m_kWeaponDataCodeInfo;

	private Dictionary<string, int> m_dicWeaponNameInfo;

	private NkWeaponTypeInfoManager()
	{
		this.m_kWeaponTypeInfo = new NkWeaponTypeInfo[11];
		this.m_kTempData = new NkWeaponTypeTempData();
		this.m_kWeaponDataCodeInfo = new NkWeaponDataCodeInfo();
		this.m_kWeaponDataCodeInfo.LoadDataCode();
		this.m_dicWeaponNameInfo = new Dictionary<string, int>();
	}

	public int GetWeaponType(string weaponcode)
	{
		return this.m_kWeaponDataCodeInfo.GetWeaponType(weaponcode);
	}

	public bool SetWeaponTypeInfo(int weapontype, ref WEAPONTYPE_INFO pkWEAPONTYPE_INFO)
	{
		if (pkWEAPONTYPE_INFO == null)
		{
			return false;
		}
		if (weapontype <= 0 || weapontype >= 11)
		{
			return false;
		}
		this.m_kWeaponTypeInfo[weapontype] = new NkWeaponTypeInfo(this.m_kTempData);
		this.m_kWeaponTypeInfo[weapontype].SetWeaponTypeInfo(weapontype, ref pkWEAPONTYPE_INFO, NrTSingleton<NkATB_Manager>.Instance.ParseWeaponTypeATB(pkWEAPONTYPE_INFO.ATB));
		string name = this.m_kWeaponTypeInfo[weapontype].GetName();
		if (!this.m_dicWeaponNameInfo.ContainsKey(name))
		{
			this.m_dicWeaponNameInfo.Add(name, weapontype);
		}
		return true;
	}

	public string GetWeaponCode(int weapontype)
	{
		NkWeaponTypeInfo weaponTypeInfo = this.GetWeaponTypeInfo(weapontype);
		if (weaponTypeInfo != null)
		{
			return weaponTypeInfo.GetCode();
		}
		return null;
	}

	public int GetWeaponTypeByName(string charname)
	{
		if (!this.m_dicWeaponNameInfo.ContainsKey(charname))
		{
			return 0;
		}
		return this.m_dicWeaponNameInfo[charname];
	}

	public NkWeaponTypeInfo GetWeaponTypeInfo(int weapontype)
	{
		if (weapontype <= 0 || weapontype >= 11)
		{
			return null;
		}
		return this.m_kWeaponTypeInfo[weapontype];
	}

	public NkWeaponTypeInfo GetWeaponTypeInfoFromCode(string weaponcode)
	{
		int weaponType = this.GetWeaponType(weaponcode);
		return this.GetWeaponTypeInfo(weaponType);
	}

	public WEAPONTYPE_INFO GetBaseWeaponTypeInfo(int weapontype)
	{
		NkWeaponTypeInfo weaponTypeInfo = this.GetWeaponTypeInfo(weapontype);
		if (weaponTypeInfo == null)
		{
			return null;
		}
		return weaponTypeInfo.GetWEAPONTYPE_INFO();
	}

	public WEAPONTYPE_INFO GetBaseWeaponTypeInfo(string weaponcode)
	{
		int weaponType = this.GetWeaponType(weaponcode);
		return this.GetBaseWeaponTypeInfo(weaponType);
	}
}
