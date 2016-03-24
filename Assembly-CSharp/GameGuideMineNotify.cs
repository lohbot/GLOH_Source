using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;

public class GameGuideMineNotify : GameGuideInfo
{
	private long _i64MineID;

	private int _i32ItemUnique;

	private int _i32itemNum;

	private int _i32BonusItemNum;

	public override void Init()
	{
		this._i64MineID = 0L;
		this._i32ItemUnique = 0;
		this._i32itemNum = 0;
		base.Init();
	}

	public void SetInfo(byte mode, long mineid, int itemunique, int itemnum, int i32BonusItemNum)
	{
		this._i64MineID = mineid;
		this._i32ItemUnique = itemunique;
		this._i32itemNum = itemnum;
		this._i32BonusItemNum = i32BonusItemNum;
	}

	public override void InitData()
	{
		this.m_nCheckTime = Time.realtimeSinceStartup;
	}

	public override void ExcuteGameGuide()
	{
		GS_MINE_OCCUPIE_USER_ITEM_GET_REQ gS_MINE_OCCUPIE_USER_ITEM_GET_REQ = new GS_MINE_OCCUPIE_USER_ITEM_GET_REQ();
		gS_MINE_OCCUPIE_USER_ITEM_GET_REQ.i64MineID = this._i64MineID;
		gS_MINE_OCCUPIE_USER_ITEM_GET_REQ.i32ItemUnique = this._i32ItemUnique;
		gS_MINE_OCCUPIE_USER_ITEM_GET_REQ.i32ItemNum = this._i32itemNum;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_OCCUPIE_USER_ITEM_GET_REQ, gS_MINE_OCCUPIE_USER_ITEM_GET_REQ);
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
		string empty = string.Empty;
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this._i32ItemUnique);
		if (this._i32BonusItemNum == 0)
		{
			string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromToolTip,
				"count",
				this._i32itemNum,
				"targetname",
				itemNameByItemUnique
			});
		}
		else
		{
			string textFromToolTip2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip("8");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromToolTip2,
				"count",
				this._i32itemNum,
				"count2",
				this._i32BonusItemNum,
				"targetname",
				itemNameByItemUnique
			});
		}
		return empty;
	}
}
