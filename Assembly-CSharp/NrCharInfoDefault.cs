using System;

public class NrCharInfoDefault : INrCharInfo
{
	private static readonly string s_nullDesc = "Null character information Object";

	public string GetCharName()
	{
		return NrCharInfoDefault.s_nullDesc;
	}

	public string GetTerrainMaterial()
	{
		return NrCharInfoDefault.s_nullDesc;
	}

	public int Get_Char_ID()
	{
		return -1;
	}

	public NrCharBase GetChar()
	{
		return null;
	}

	public NkBattleChar GetBattleChar()
	{
		return null;
	}

	public string GetWeapon()
	{
		return NrCharInfoDefault.s_nullDesc;
	}
}
