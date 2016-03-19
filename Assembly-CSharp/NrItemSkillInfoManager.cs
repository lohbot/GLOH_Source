using GAME;
using System;
using System.Collections.Generic;

public class NrItemSkillInfoManager : NrTSingleton<NrItemSkillInfoManager>
{
	private SortedDictionary<int, ITEMSKILL_INFO> m_sdCollection;

	private List<ITEMSKILL_INFO> m_ItemSkillList = new List<ITEMSKILL_INFO>();

	private NrItemSkillInfoManager()
	{
		this.m_sdCollection = new SortedDictionary<int, ITEMSKILL_INFO>();
	}

	public void Set_Value(ITEMSKILL_INFO a_cValue)
	{
		if (!this.m_sdCollection.ContainsKey(a_cValue.SkillUnique))
		{
			this.m_sdCollection.Add(a_cValue.SkillUnique, a_cValue);
		}
		this.m_ItemSkillList.Add(a_cValue);
	}

	public ITEMSKILL_INFO Get_Value(int nSkillUnique)
	{
		if (this.m_sdCollection.ContainsKey(nSkillUnique))
		{
			return this.m_sdCollection[nSkillUnique];
		}
		return null;
	}

	public string GetPreText(int nSkillUnique)
	{
		ITEMSKILL_INFO iTEMSKILL_INFO = this.Get_Value(nSkillUnique);
		if (iTEMSKILL_INFO != null)
		{
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper(iTEMSKILL_INFO.PrefixText);
		}
		return string.Empty;
	}

	public List<ITEMSKILL_INFO> GetValue()
	{
		return this.m_ItemSkillList;
	}

	public List<ITEMSKILL_INFO> GetValueFromItemType(eITEM_TYPE eItemType)
	{
		List<ITEMSKILL_INFO> list = new List<ITEMSKILL_INFO>();
		for (int i = 0; i < this.m_ItemSkillList.Count; i++)
		{
			if (eItemType == this.m_ItemSkillList[i].m_eItemType)
			{
				list.Add(this.m_ItemSkillList[i]);
			}
		}
		return list;
	}

	public List<ITEMSKILL_INFO> GetValueFromAll()
	{
		List<ITEMSKILL_INFO> list = new List<ITEMSKILL_INFO>();
		for (int i = 0; i < this.m_ItemSkillList.Count; i++)
		{
			list.Add(this.m_ItemSkillList[i]);
		}
		return list;
	}

	public eWEAPON_TYPE GetWeaponTypeFromString(string strWeaponName)
	{
		eWEAPON_TYPE result = eWEAPON_TYPE.WEAPON_NONE;
		switch (strWeaponName)
		{
		case "SWORD":
			result = eWEAPON_TYPE.WEAPON_SWORD;
			break;
		case "SPEAR":
			result = eWEAPON_TYPE.WEAPON_SPEAR;
			break;
		case "AXE":
			result = eWEAPON_TYPE.WEAPON_AXE;
			break;
		case "BOW":
			result = eWEAPON_TYPE.WEAPON_BOW;
			break;
		case "GUN":
			result = eWEAPON_TYPE.WEAPON_GUN;
			break;
		case "CANNON":
			result = eWEAPON_TYPE.WEAPON_CANNON;
			break;
		case "STAFF":
			result = eWEAPON_TYPE.WEAPON_STAFF;
			break;
		case "BIBLE":
			result = eWEAPON_TYPE.WEAPON_BIBLE;
			break;
		case "SHIELD":
			result = eWEAPON_TYPE.WEAPON_SHIELD;
			break;
		case "ORB":
			result = eWEAPON_TYPE.WEAPON_ORB;
			break;
		}
		return result;
	}
}
