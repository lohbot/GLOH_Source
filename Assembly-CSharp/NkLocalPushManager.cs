using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using System;
using UnityEngine;

public class NkLocalPushManager : NrTSingleton<NkLocalPushManager>
{
	private NkLocalPushManager()
	{
	}

	public void SetPushSetting(eLOCAL_PUSH_TYPE Type, bool bPushSetting)
	{
		string text = string.Empty;
		switch (Type)
		{
		case eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_INJURYTIME:
			text = NrPrefsKey.LOCALPUSH_INJURYTIME;
			break;
		case eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_ACTIVITYTIME:
			text = NrPrefsKey.LOCALPUSH_ACTIVITY;
			break;
		case eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_BATTLEMATCHTIME:
			text = NrPrefsKey.LOCALPUSH_MATCHOPEN;
			break;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		PlayerPrefs.SetInt(text, (!bPushSetting) ? 0 : 1);
		if (bPushSetting)
		{
			this.SetPush(Type, 0L);
		}
		else
		{
			this.CalclePush(Type);
		}
	}

	public void SetPush(eLOCAL_PUSH_TYPE Type, long Addtime = 0L)
	{
		if (Scene.CurScene < Scene.Type.PREPAREGAME)
		{
			return;
		}
		switch (Type)
		{
		case eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_INJURYTIME:
			this.SendInjuryTime();
			break;
		case eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_ACTIVITYTIME:
			this.SendActity();
			break;
		case eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_BATTLEMATCHTIME:
			this.SendBattleMatchOpen();
			break;
		}
	}

	public void SetPushAll()
	{
		this.SendActity();
		this.SendInjuryTime();
		this.SendBattleMatchOpen();
	}

	public void CalclePush(eLOCAL_PUSH_TYPE Type)
	{
		switch (Type)
		{
		case eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_INJURYTIME:
			TsPlatform.Operator.CancelLocalPush(10);
			break;
		case eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_ACTIVITYTIME:
			TsPlatform.Operator.CancelLocalPush(11);
			break;
		case eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_BATTLEMATCHTIME:
			TsPlatform.Operator.CancelLocalPush(12);
			break;
		}
	}

	private void SendActity()
	{
		int @int = PlayerPrefs.GetInt(NrPrefsKey.LOCALPUSH_ACTIVITY);
		if (@int != 0)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			long num = kMyCharInfo.m_nMaxActivityPoint;
			if (num == 0L && instance != null)
			{
				num = (long)(instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BASE_ACTIVITY) + (int)NrTSingleton<NrTableVipManager>.Instance.GetVipLevelActivityPointMax());
			}
			TsLog.LogWarning("SetActivityPointMax ActivityPoint = {0}, MaxActivityPoint = {1} MaxActivityPoint = {2}", new object[]
			{
				kMyCharInfo.m_nActivityPoint,
				kMyCharInfo.m_nMaxActivityPoint,
				num
			});
			if (kMyCharInfo.m_nActivityPoint < num)
			{
				float num2 = 600f;
				if (instance != null)
				{
					if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
					{
						num2 = (float)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_UPDATE_ACTIVITY_MINUTE) * 60f;
					}
					else
					{
						short vipLevelActivityTime = NrTSingleton<NrTableVipManager>.Instance.GetVipLevelActivityTime();
						num2 = (float)(instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_UPDATE_ACTIVITY_MINUTE) - (int)vipLevelActivityTime) * 60f;
					}
				}
				float num3 = num2 * (float)(num - kMyCharInfo.m_nActivityPoint - 1L) + kMyCharInfo.m_fCurrentActivityTime;
				TsLog.LogWarning("ActivtyTime UpdateTime = {0}, m_fCurrentActivityTime = {1}", new object[]
				{
					num2,
					kMyCharInfo.m_fCurrentActivityTime
				});
				TsPlatform.Operator.SendLocalPush(11, (long)num3, NrTSingleton<NrTextMgr>.Instance.GetTextFromPush("4"));
			}
			else
			{
				TsPlatform.Operator.CancelLocalPush(11);
			}
		}
	}

	private void SendInjuryTime()
	{
		if (PlayerPrefs.GetInt(NrPrefsKey.LOCALPUSH_INJURYTIME) == 0)
		{
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			long num = 0L;
			NrSoldierList soldierList = charPersonInfo.GetSoldierList();
			NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
			for (int i = 0; i < kSolInfo.Length; i++)
			{
				NkSoldierInfo nkSoldierInfo = kSolInfo[i];
				long remainInjuryTime = nkSoldierInfo.GetRemainInjuryTime();
				if (num < remainInjuryTime)
				{
					num = remainInjuryTime;
				}
			}
			if (num == 0L)
			{
				TsPlatform.Operator.CancelLocalPush(10);
			}
			else
			{
				TsLog.LogWarning("GS_SOLDIER_SUBDATA_ACK INJURY Push", new object[0]);
				TsPlatform.Operator.SendLocalPush(10, num, NrTSingleton<NrTextMgr>.Instance.GetTextFromPush("5"));
			}
		}
	}

	private void SendBattleMatchOpen()
	{
		if (PlayerPrefs.GetInt(NrPrefsKey.LOCALPUSH_MATCHOPEN) == 0)
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_PLUNDER_DELAYTIME);
		long curTime = PublicMethod.GetCurTime();
		long num = charSubData - curTime;
		if (num > 1L)
		{
			TsLog.LogWarning("CHAR_SUBDATA_PLUNDER_DELAYTIME  Push", new object[0]);
			TsPlatform.Operator.SendLocalPush(12, num, NrTSingleton<NrTextMgr>.Instance.GetTextFromPush("6"));
		}
		else
		{
			TsPlatform.Operator.CancelLocalPush(12);
		}
	}
}
