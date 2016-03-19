using System;
using System.Collections.Generic;
using TsLibs;

public class BASE_BATTLE_MAP_Manager : NrTableBase
{
	private Dictionary<short, BATTLE_MAP> m_hashBattleMap = new Dictionary<short, BATTLE_MAP>();

	private static BASE_BATTLE_MAP_Manager Instance;

	private BASE_BATTLE_MAP_Manager(string strFilePath) : base(strFilePath, true)
	{
	}

	public static BASE_BATTLE_MAP_Manager GetInstance()
	{
		if (BASE_BATTLE_MAP_Manager.Instance == null)
		{
			BASE_BATTLE_MAP_Manager.Instance = new BASE_BATTLE_MAP_Manager(CDefinePath.BattleMapInfoURL);
		}
		return BASE_BATTLE_MAP_Manager.Instance;
	}

	public BATTLE_MAP GetInfo(short BATTLE_MAP_ID)
	{
		if (this.m_hashBattleMap.ContainsKey(BATTLE_MAP_ID))
		{
			return this.m_hashBattleMap[BATTLE_MAP_ID];
		}
		return null;
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLE_MAP bATTLE_MAP = new BATTLE_MAP();
			bATTLE_MAP.SetData(data);
			if (!this.m_hashBattleMap.ContainsKey(bATTLE_MAP.BATTLE_MAP_ID))
			{
				this.m_hashBattleMap.Add(bATTLE_MAP.BATTLE_MAP_ID, bATTLE_MAP);
			}
		}
		return true;
	}
}
