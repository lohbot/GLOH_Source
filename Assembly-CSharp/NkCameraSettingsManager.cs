using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;

public class NkCameraSettingsManager : NrTSingleton<NkCameraSettingsManager>
{
	private enum eExtraBattleType
	{
		MYTHRAID = 15,
		NEWEXPLORATION,
		GUILDBOSS,
		MAX
	}

	private Dictionary<int, CAMERASETTING_DATA>[] m_kCameraSettings;

	private Dictionary<string, int> m_dicSceneType = new Dictionary<string, int>();

	private int m_nCameraLevelMax = 8;

	private NkCameraSettingsManager()
	{
		this.m_kCameraSettings = new Dictionary<int, CAMERASETTING_DATA>[18];
		for (int i = 0; i < this.m_kCameraSettings.Length; i++)
		{
			this.m_kCameraSettings[i] = new Dictionary<int, CAMERASETTING_DATA>();
		}
		this.m_dicSceneType.Add("NONE", 0);
		this.m_dicSceneType.Add("INIT", 6);
		this.m_dicSceneType.Add("PREPAREGAME", 8);
		this.m_dicSceneType.Add("SELECTAVATAR", 7);
		this.m_dicSceneType.Add("WORLD", 10);
		this.m_dicSceneType.Add("BATTLE", 12);
		this.m_dicSceneType.Add("CURDIRECTION", 13);
		this.m_dicSceneType.Add("MYTHRAID", 15);
		this.m_dicSceneType.Add("NEWEXPLORATION", 16);
		this.m_dicSceneType.Add("GUILDBOSS", 17);
	}

	public int GetSceneEnum(string szSceneName)
	{
		if (this.m_dicSceneType.ContainsKey(szSceneName))
		{
			return this.m_dicSceneType[szSceneName];
		}
		return 0;
	}

	public int GetMaxLevel()
	{
		return this.m_nCameraLevelMax;
	}

	public void AddCameraSettingData(int eType, CAMERASETTING_DATA kData)
	{
		if (eType <= 0 || eType > 18)
		{
			return;
		}
		if (kData.m_Level < 0 || kData.m_Level >= this.m_nCameraLevelMax)
		{
			return;
		}
		if (this.m_kCameraSettings[eType].ContainsKey(kData.m_Level))
		{
			return;
		}
		this.m_kCameraSettings[eType].Add(kData.m_Level, kData);
	}

	public CAMERASETTING_DATA GetCameraData(int nLevel)
	{
		if (nLevel < 0 || nLevel >= this.m_nCameraLevelMax)
		{
			return null;
		}
		if (Scene.CurScene <= Scene.Type.INITIALIZE || Scene.CurScene > Scene.Type.CUTSCENE)
		{
			return null;
		}
		if (Battle.BATTLE != null && Scene.CurScene == Scene.Type.BATTLE)
		{
			eBATTLE_ROOMTYPE battleRoomtype = Battle.BATTLE.BattleRoomtype;
			switch (battleRoomtype)
			{
			case eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID:
				return this.m_kCameraSettings[15][nLevel];
			case eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PREVIEW:
				IL_64:
				if (battleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS)
				{
					goto IL_A1;
				}
				return this.m_kCameraSettings[17][nLevel];
			case eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION:
				return this.m_kCameraSettings[16][nLevel];
			}
			goto IL_64;
		}
		IL_A1:
		if (this.m_kCameraSettings[(int)Scene.CurScene].ContainsKey(nLevel))
		{
			return this.m_kCameraSettings[(int)Scene.CurScene][nLevel];
		}
		return null;
	}
}
