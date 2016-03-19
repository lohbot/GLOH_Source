using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;

public class GameGuideExpeditionNotify : GameGuideInfo
{
	private byte _ui8ExpeditionMilitaryUniq;

	private short _i16ExpeditionCreateDataID;

	private int _i32itemNum;

	public override void Init()
	{
		this._ui8ExpeditionMilitaryUniq = 0;
		this._i16ExpeditionCreateDataID = 0;
		this._i32itemNum = 0;
		base.Init();
	}

	public void SetInfo(byte expedition_militaryuniq, short dataid, int itemnum)
	{
		this._ui8ExpeditionMilitaryUniq = expedition_militaryuniq;
		this._i16ExpeditionCreateDataID = dataid;
		this._i32itemNum = itemnum;
	}

	public override void InitData()
	{
		this.m_nCheckTime = Time.realtimeSinceStartup;
	}

	public override void ExcuteGameGuide()
	{
		GS_EXPEDITION_OCCUPY_ITEM_GET_REQ gS_EXPEDITION_OCCUPY_ITEM_GET_REQ = new GS_EXPEDITION_OCCUPY_ITEM_GET_REQ();
		gS_EXPEDITION_OCCUPY_ITEM_GET_REQ.ui8ExpeditionMilitaryUniq = this._ui8ExpeditionMilitaryUniq;
		gS_EXPEDITION_OCCUPY_ITEM_GET_REQ.i16ExpeditionCreateDataID = this._i16ExpeditionCreateDataID;
		gS_EXPEDITION_OCCUPY_ITEM_GET_REQ.i32ItemNum = this._i32itemNum;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_OCCUPY_ITEM_GET_REQ, gS_EXPEDITION_OCCUPY_ITEM_GET_REQ);
		this.InitData();
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
		return this.CheckGameGuideOnce();
	}

	public override string GetGameGuideText()
	{
		string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
		string empty = string.Empty;
		string text = string.Empty;
		EXPEDITION_CREATE_DATA expedtionCreateData = BASE_EXPEDITION_CREATE_DATA.GetExpedtionCreateData(this._i16ExpeditionCreateDataID);
		if (expedtionCreateData != null)
		{
			text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(expedtionCreateData.EXPEDITION_ITEM_UNIQUE);
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromToolTip,
			"count",
			this._i32itemNum,
			"targetname",
			text
		});
		return empty;
	}
}
