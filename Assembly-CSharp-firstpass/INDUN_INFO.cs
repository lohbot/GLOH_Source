using GAME;
using System;
using TsLibs;

public class INDUN_INFO : NrTableData
{
	public int m_nIndunIDX;

	public string szTextKey = string.Empty;

	public string szName = string.Empty;

	public int m_nMapIndex;

	public int m_nDunScenarioIdx;

	public eINDUN_TYPE m_eIndun_Type;

	public int m_nNeedLevel;

	public int m_nCloseingWaitTime;

	public int m_nResetTime;

	public int m_nMaxIndunNum;

	public int m_nReturnGateUnique;

	public float m_fPlayTime;

	public POS3D m_stStartPos = new POS3D();

	public int m_nQuestGroupUnique;

	public string m_szWebImagePath = string.Empty;

	public long m_nRewardGold;

	public int m_nMaxUser = 1;

	public int m_nNpcCode = -1;

	public bool m_bShowUI;

	public bool m_bUseActivity;

	public string m_szMobileImagePath = string.Empty;

	public string strIndunType = string.Empty;

	public string strNpcCode = string.Empty;

	public string IndunImagePath
	{
		get
		{
			if (TsPlatform.IsMobile)
			{
				return this.m_szMobileImagePath;
			}
			return this.m_szWebImagePath;
		}
	}

	public INDUN_INFO() : base(NrTableData.eResourceType.eRT_INDUN_INFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nIndunIDX = 0;
		this.szTextKey = string.Empty;
		this.szName = string.Empty;
		this.m_nMapIndex = 0;
		this.m_nDunScenarioIdx = 0;
		this.m_eIndun_Type = eINDUN_TYPE.eINDUN_TYPE_NONE;
		this.m_nNeedLevel = 0;
		this.m_nCloseingWaitTime = 0;
		this.m_nResetTime = 0;
		this.m_nMaxIndunNum = 0;
		this.m_nReturnGateUnique = 0;
		this.m_fPlayTime = 0f;
		this.m_nQuestGroupUnique = 0;
		this.m_szWebImagePath = string.Empty;
		this.m_nRewardGold = 0L;
		this.m_nMaxUser = 1;
		this.m_nNpcCode = -1;
		this.m_bShowUI = false;
		this.m_bUseActivity = false;
		this.m_szMobileImagePath = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		int bShowUI = 0;
		int bUseActivity = 0;
		row.GetColumn(num++, out this.m_nIndunIDX);
		row.GetColumn(num++, out this.szTextKey);
		row.GetColumn(num++, out this.szName);
		row.GetColumn(num++, out this.m_nMapIndex);
		row.GetColumn(num++, out this.m_nDunScenarioIdx);
		row.GetColumn(num++, out this.strIndunType);
		row.GetColumn(num++, out this.m_nNeedLevel);
		row.GetColumn(num++, out this.m_nCloseingWaitTime);
		row.GetColumn(num++, out this.m_nResetTime);
		row.GetColumn(num++, out this.m_nMaxIndunNum);
		row.GetColumn(num++, out this.m_nReturnGateUnique);
		row.GetColumn(num++, out this.m_fPlayTime);
		row.GetColumn(num++, out this.m_stStartPos.x);
		row.GetColumn(num++, out this.m_stStartPos.y);
		row.GetColumn(num++, out this.m_stStartPos.z);
		row.GetColumn(num++, out this.m_nQuestGroupUnique);
		row.GetColumn(num++, out this.m_szWebImagePath);
		row.GetColumn(num++, out this.m_nRewardGold);
		row.GetColumn(num++, out this.m_nMaxUser);
		row.GetColumn(num++, out this.strNpcCode);
		row.GetColumn(num++, out bShowUI);
		row.GetColumn(num++, out bUseActivity);
		row.GetColumn(num++, out this.m_szMobileImagePath);
		this.m_bShowUI = (bShowUI != 0);
		this.m_bUseActivity = (bUseActivity != 0);
	}
}
