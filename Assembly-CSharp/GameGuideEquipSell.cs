using PROTOCOL;
using PROTOCOL.GAME;
using System;
using TsBundle;
using UnityEngine;

public class GameGuideEquipSell : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		long equipSellMoney = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetEquipSellMoney();
		if (equipSellMoney <= 0L)
		{
			return;
		}
		GS_CHAR_EQUIPSELLMONEY_REQ gS_CHAR_EQUIPSELLMONEY_REQ = new GS_CHAR_EQUIPSELLMONEY_REQ();
		gS_CHAR_EQUIPSELLMONEY_REQ.m_nMoney = equipSellMoney;
		SendPacket.GetInstance().SendObject(28, gS_CHAR_EQUIPSELLMONEY_REQ);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MARKET", "SELL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		return true;
	}

	public override bool CheckGameGuide()
	{
		if (this.m_eCheck == GameGuideCheck.LOGIN)
		{
			long equipSellMoney = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetEquipSellMoney();
			return equipSellMoney > 0L;
		}
		if (this.m_eCheck == GameGuideCheck.CYCLECAL && Time.realtimeSinceStartup - this.m_nCheckTime > this.m_nDelayTime)
		{
			this.m_nCheckTime = Time.realtimeSinceStartup;
			long equipSellMoney2 = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetEquipSellMoney();
			return equipSellMoney2 > 0L;
		}
		return false;
	}

	public override string GetGameGuideText()
	{
		long equipSellMoney = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetEquipSellMoney();
		string result = string.Empty;
		if (equipSellMoney == 0L && NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().EquipMoneyAttackPlunder)
		{
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip("2024");
		}
		else
		{
			string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref result, new object[]
			{
				textFromToolTip,
				"gold",
				ANNUALIZED.Convert(equipSellMoney)
			});
		}
		return result;
	}
}
