using System;

public interface INrCharInfo
{
	string GetCharName();

	string GetTerrainMaterial();

	int Get_Char_ID();

	NrCharBase GetChar();

	NkBattleChar GetBattleChar();

	string GetWeapon();
}
