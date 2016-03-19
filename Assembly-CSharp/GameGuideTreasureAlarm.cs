using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;

public class GameGuideTreasureAlarm : GameGuideInfo
{
	private short m_i16TreasureUniqe;

	private int m_i32TreasureDay;

	private string m_strCharName = string.Empty;

	public override void Init()
	{
		base.Init();
	}

	public void SetInfo(string strCharName, short i16Treasure, int i32Day)
	{
		this.m_nCheckTime = Time.realtimeSinceStartup;
		this.m_strCharName = strCharName;
		this.m_i16TreasureUniqe = i16Treasure;
		this.m_i32TreasureDay = i32Day;
	}

	public override void ExcuteGameGuide()
	{
		GS_TREASUREBOX_MOVE_REQ gS_TREASUREBOX_MOVE_REQ = new GS_TREASUREBOX_MOVE_REQ();
		gS_TREASUREBOX_MOVE_REQ.i16TreasureUnique = this.m_i16TreasureUniqe;
		gS_TREASUREBOX_MOVE_REQ.i32Day = this.m_i32TreasureDay;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TREASUREBOX_MOVE_REQ, gS_TREASUREBOX_MOVE_REQ);
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
		DateTime dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
		return this.CheckGuide() && dueDate.Day == this.m_i32TreasureDay && this.m_eCheck == GameGuideCheck.CYCLECAL && Time.realtimeSinceStartup - this.m_nCheckTime > this.m_nDelayTime;
	}

	private bool CheckGuide()
	{
		return this.m_i16TreasureUniqe != 0 && this.m_i32TreasureDay != 0;
	}

	public override string GetGameGuideText()
	{
		string text = string.Empty;
		if (!string.IsNullOrEmpty(this.m_strCharName))
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text,
				"username",
				this.m_strCharName
			});
		}
		return text;
	}
}
