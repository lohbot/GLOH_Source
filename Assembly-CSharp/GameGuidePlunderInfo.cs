using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;

public class GameGuidePlunderInfo : GameGuideInfo
{
	private int m_nPlunderCount;

	private long m_nPlunderMoney;

	private long m_nPlunderSellMoney;

	public override void Init()
	{
		base.Init();
		this.m_nPlunderCount = 0;
		this.m_nPlunderMoney = 0L;
		this.m_nPlunderSellMoney = 0L;
	}

	public void SetPlunderInfo(int nPlunderCount, long nPlunderCurMoney, long nPlunderSellMoney, long nPlunderSupportMoney)
	{
		this.m_nPlunderCount = nPlunderCount;
		this.m_nPlunderMoney = nPlunderCurMoney;
		this.m_nPlunderSellMoney = nPlunderSellMoney;
	}

	public override void ExcuteGameGuide()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		GS_PLUNDER_RECORD_LIST_GET_REQ gS_PLUNDER_RECORD_LIST_GET_REQ = new GS_PLUNDER_RECORD_LIST_GET_REQ();
		gS_PLUNDER_RECORD_LIST_GET_REQ.i64PersonID = charPersonInfo.GetPersonID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_RECORD_LIST_GET_REQ, gS_PLUNDER_RECORD_LIST_GET_REQ);
		this.Init();
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuide()
	{
		return this.m_eCheck == GameGuideCheck.LOGIN && this.m_nPlunderCount > 0;
	}

	public override string GetGameGuideText()
	{
		string empty = string.Empty;
		string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromToolTip,
			"count",
			this.m_nPlunderCount.ToString(),
			"gold1",
			ANNUALIZED.Convert(this.m_nPlunderMoney),
			"gold2",
			ANNUALIZED.Convert(this.m_nPlunderSellMoney)
		});
		return empty;
	}
}
