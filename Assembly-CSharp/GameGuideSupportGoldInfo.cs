using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using TsBundle;
using UnityEngine;

public class GameGuideSupportGoldInfo : GameGuideInfo
{
	private long m_nReceiveGold;

	public override void Init()
	{
		base.Init();
		this.m_nReceiveGold = 0L;
	}

	public override void ExcuteGameGuide()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsHeroBattle())
		{
			GS_PLUNDER_SUPPORT_GOLD_GET_REQ gS_PLUNDER_SUPPORT_GOLD_GET_REQ = new GS_PLUNDER_SUPPORT_GOLD_GET_REQ();
			gS_PLUNDER_SUPPORT_GOLD_GET_REQ.m_nMoney = this.m_nReceiveGold;
			SendPacket.GetInstance().SendObject(1411, gS_PLUNDER_SUPPORT_GOLD_GET_REQ);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MARKET", "BUY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		this.Init();
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuide()
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsHeroBattle())
		{
			return false;
		}
		if (this.m_eCheck == GameGuideCheck.LOGIN)
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo == null)
			{
				return false;
			}
			int level = myCharInfo.GetLevel();
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_RECEIVE_SUPPORT_GOLD_TIME);
			if (charSubData <= 0L)
			{
				return false;
			}
			long curTime = PublicMethod.GetCurTime();
			long nPastTime = curTime - charSubData;
			this.m_nReceiveGold = NrTSingleton<NrTable_SupportGold_Manager>.Instance.GetReceiveGold(level, nPastTime);
			return this.m_nReceiveGold > 0L;
		}
		else
		{
			if (this.m_eCheck != GameGuideCheck.CYCLECAL || Time.realtimeSinceStartup - this.m_nCheckTime <= this.m_nDelayTime)
			{
				return false;
			}
			NrMyCharInfo myCharInfo2 = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo2 == null)
			{
				return false;
			}
			int level2 = myCharInfo2.GetLevel();
			long charSubData2 = myCharInfo2.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_RECEIVE_SUPPORT_GOLD_TIME);
			long curTime2 = PublicMethod.GetCurTime();
			if (charSubData2 <= 0L)
			{
				return false;
			}
			long nPastTime2 = curTime2 - charSubData2;
			this.m_nReceiveGold = NrTSingleton<NrTable_SupportGold_Manager>.Instance.GetReceiveGold(level2, nPastTime2);
			return this.m_nReceiveGold > 0L;
		}
	}

	public override string GetGameGuideText()
	{
		string empty = string.Empty;
		string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
		if (this.m_nReceiveGold < 0L)
		{
			this.m_nReceiveGold = 0L;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromToolTip,
			"gold",
			ANNUALIZED.Convert(this.m_nReceiveGold)
		});
		return empty;
	}
}
