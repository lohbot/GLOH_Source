using GAME;
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
	}

	public void SetPushAll()
	{
	}

	public void CalclePush(eLOCAL_PUSH_TYPE Type)
	{
	}

	private void SendActity()
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			return;
		}
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
	}

	private void SendBattleMatchOpen()
	{
	}
}
