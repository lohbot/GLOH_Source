using GAME;
using System;

public sealed class NrCharInfoLogic : INrCharInfo
{
	private NrCharBase m_charBase;

	private NkBattleChar m_battleChar;

	public NrCharInfoLogic(NrCharBase logicChar)
	{
		this.m_charBase = logicChar;
	}

	public NrCharInfoLogic(NkBattleChar logicChar)
	{
		this.m_battleChar = logicChar;
	}

	public string GetCharName()
	{
		return this.m_charBase.GetPersonInfo().GetCharName();
	}

	public string GetTerrainMaterial()
	{
		return "LAND";
	}

	public int Get_Char_ID()
	{
		return this.m_charBase.GetID();
	}

	public NrCharBase GetChar()
	{
		return this.m_charBase;
	}

	public NkBattleChar GetBattleChar()
	{
		return this.m_battleChar;
	}

	public string GetWeapon()
	{
		NrCharBase charBase = this.m_charBase;
		if (charBase != null)
		{
			eWEAPON_TYPE weaponType = (eWEAPON_TYPE)charBase.GetWeaponType();
			string text = weaponType.ToString();
			return text.ToUpper();
		}
		if (this.m_battleChar != null)
		{
			eWEAPON_TYPE attackCharWeaponType = (eWEAPON_TYPE)this.m_battleChar.GetAttackCharWeaponType();
			string text2 = attackCharWeaponType.ToString();
			return text2.ToUpper();
		}
		return "Null";
	}
}
