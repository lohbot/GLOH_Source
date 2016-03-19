using System;

public class NkWeaponDataCodeInfo
{
	private NkValueParse<int> m_kWeaponTypeCode;

	public NkWeaponDataCodeInfo()
	{
		this.m_kWeaponTypeCode = new NkValueParse<int>();
	}

	public void LoadDataCode()
	{
		this.m_kWeaponTypeCode.InsertCodeValue("NONE", 0);
		this.m_kWeaponTypeCode.InsertCodeValue("SWORD", 1);
		this.m_kWeaponTypeCode.InsertCodeValue("SPEAR", 2);
		this.m_kWeaponTypeCode.InsertCodeValue("AXE", 3);
		this.m_kWeaponTypeCode.InsertCodeValue("BOW", 4);
		this.m_kWeaponTypeCode.InsertCodeValue("GUN", 5);
		this.m_kWeaponTypeCode.InsertCodeValue("CANNON", 6);
		this.m_kWeaponTypeCode.InsertCodeValue("STAFF", 7);
		this.m_kWeaponTypeCode.InsertCodeValue("BIBLE", 8);
		this.m_kWeaponTypeCode.InsertCodeValue("SHIELD", 9);
		this.m_kWeaponTypeCode.InsertCodeValue("ORB", 10);
	}

	public int GetWeaponType(string datacode)
	{
		return this.m_kWeaponTypeCode.GetValue(datacode);
	}
}
