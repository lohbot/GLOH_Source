using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;

public class NkCameraSettingsManager : NrTSingleton<NkCameraSettingsManager>
{
	private Dictionary<int, CAMERASETTING_DATA>[] m_kCameraSettings;

	private Dictionary<string, Scene.Type> m_dicSceneType = new Dictionary<string, Scene.Type>();

	private int m_nCameraLevelMax = 8;

	private NkCameraSettingsManager()
	{
		this.m_kCameraSettings = new Dictionary<int, CAMERASETTING_DATA>[15];
		for (int i = 0; i < this.m_kCameraSettings.Length; i++)
		{
			this.m_kCameraSettings[i] = new Dictionary<int, CAMERASETTING_DATA>();
		}
		this.m_dicSceneType.Add("NONE", Scene.Type.EMPTY);
		this.m_dicSceneType.Add("INIT", Scene.Type.INITIALIZE);
		this.m_dicSceneType.Add("PREPAREGAME", Scene.Type.PREPAREGAME);
		this.m_dicSceneType.Add("SELECTAVATAR", Scene.Type.SELECTCHAR);
		this.m_dicSceneType.Add("WORLD", Scene.Type.WORLD);
		this.m_dicSceneType.Add("BATTLE", Scene.Type.BATTLE);
		this.m_dicSceneType.Add("CURDIRECTION", Scene.Type.CUTSCENE);
	}

	public Scene.Type GetSceneEnum(string szSceneName)
	{
		if (this.m_dicSceneType.ContainsKey(szSceneName))
		{
			return this.m_dicSceneType[szSceneName];
		}
		return Scene.Type.EMPTY;
	}

	public int GetMaxLevel()
	{
		return this.m_nCameraLevelMax;
	}

	public void AddCameraSettingData(Scene.Type eScene, CAMERASETTING_DATA kData)
	{
		if (eScene <= Scene.Type.EMPTY || eScene > Scene.Type.CUTSCENE)
		{
			return;
		}
		if (kData.m_Level < 0 || kData.m_Level >= this.m_nCameraLevelMax)
		{
			return;
		}
		if (this.m_kCameraSettings[(int)eScene].ContainsKey(kData.m_Level))
		{
			return;
		}
		this.m_kCameraSettings[(int)eScene].Add(kData.m_Level, kData);
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
		if (this.m_kCameraSettings[(int)Scene.CurScene].ContainsKey(nLevel))
		{
			return this.m_kCameraSettings[(int)Scene.CurScene][nLevel];
		}
		return null;
	}
}
