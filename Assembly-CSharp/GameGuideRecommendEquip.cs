using GAME;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;

public class GameGuideRecommendEquip : GameGuideInfo
{
	private long srcSolID;

	private long destSolID;

	private ITEM srcItem;

	private ITEM destItem;

	public override void Init()
	{
		base.Init();
		this.srcSolID = 0L;
		this.destSolID = 0L;
		this.srcItem = null;
		this.destItem = null;
	}

	public ITEM GetSrcItem()
	{
		return this.srcItem;
	}

	public ITEM GetDestItem()
	{
		return this.destItem;
	}

	public override void ExcuteGameGuide()
	{
		if (this.srcItem == null || this.destItem == null)
		{
			return;
		}
		GS_ITEM_MOVE_REQ gS_ITEM_MOVE_REQ = new GS_ITEM_MOVE_REQ();
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(this.destSolID);
		if (soldierInfoFromSolID == null)
		{
			return;
		}
		ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(this.srcItem.m_nItemUnique);
		if (itemTypeInfo == null)
		{
			return;
		}
		if (!soldierInfoFromSolID.IsEquipClassType(itemTypeInfo.WEAPONTYPE, itemTypeInfo.EQUIPCLASSTYPE))
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("34");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (NrTSingleton<ItemManager>.Instance.GetItemMinLevelFromItem(this.srcItem) > (int)soldierInfoFromSolID.GetLevel())
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("358");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int equipItemPos = (int)NrTSingleton<ItemManager>.Instance.GetEquipItemPos(this.srcItem.m_nItemUnique);
		gS_ITEM_MOVE_REQ.m_nMoveType = NrTSingleton<ItemManager>.Instance.GetItemMoveType_InvenToSol(this.srcItem.m_nPosType);
		gS_ITEM_MOVE_REQ.m_nSrcItemID = this.srcItem.m_nItemID;
		gS_ITEM_MOVE_REQ.m_nSrcItemPos = this.srcItem.m_nItemPos;
		gS_ITEM_MOVE_REQ.m_nSrcSolID = this.srcSolID;
		if (this.destItem != null)
		{
			gS_ITEM_MOVE_REQ.m_nDestItemID = this.destItem.m_nItemID;
			gS_ITEM_MOVE_REQ.m_nDestItemPos = equipItemPos;
			gS_ITEM_MOVE_REQ.m_nDestSolID = this.destSolID;
		}
		else
		{
			gS_ITEM_MOVE_REQ.m_nDestItemID = 0L;
			gS_ITEM_MOVE_REQ.m_nDestItemPos = equipItemPos;
			gS_ITEM_MOVE_REQ.m_nDestSolID = this.destSolID;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_MOVE_REQ, gS_ITEM_MOVE_REQ);
		NrTSingleton<GameGuideManager>.Instance.ExecuteGuide = true;
	}

	public void OpenUI()
	{
	}

	private bool CheckGuide()
	{
		this.destItem = null;
		this.srcItem = null;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
				if (soldierInfo != null)
				{
					if (soldierInfo.GetSolID() != 0L)
					{
						NrEquipItemInfo equipItemInfo = soldierInfo.GetEquipItemInfo();
						if (equipItemInfo != null)
						{
							for (int j = 0; j < 6; j++)
							{
								if (0 < equipItemInfo.GetItemUnique(j))
								{
									ITEM batterItemByUnique = NkUserInventory.instance.GetBatterItemByUnique(equipItemInfo.GetItem(j), soldierInfo.GetLevel());
									if (batterItemByUnique != null)
									{
										this.destItem = equipItemInfo.GetItem(j);
										this.srcItem = batterItemByUnique;
										NkSoldierInfo soldierInfo2 = charPersonInfo.GetSoldierInfo(0);
										if (soldierInfo2 != null)
										{
											this.srcSolID = soldierInfo2.GetSolID();
										}
										this.destSolID = soldierInfo.GetSolID();
										return true;
									}
								}
							}
						}
					}
				}
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return false;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current != null)
			{
				if (0L < current.GetExp())
				{
					if (current.GetSolID() != 0L)
					{
						NrEquipItemInfo equipItemInfo2 = current.GetEquipItemInfo();
						if (equipItemInfo2 != null)
						{
							for (int k = 0; k < 6; k++)
							{
								if (0 < equipItemInfo2.GetItemUnique(k))
								{
									ITEM batterItemByUnique2 = NkUserInventory.instance.GetBatterItemByUnique(equipItemInfo2.GetItem(k), current.GetLevel());
									if (batterItemByUnique2 != null)
									{
										this.destItem = equipItemInfo2.GetItem(k);
										this.srcItem = batterItemByUnique2;
										NkSoldierInfo soldierInfo3 = charPersonInfo.GetSoldierInfo(0);
										if (soldierInfo3 != null)
										{
											this.srcSolID = soldierInfo3.GetSolID();
										}
										this.destSolID = current.GetSolID();
										return true;
									}
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	public override bool CheckGameGuideOnce()
	{
		return this.CheckGuide();
	}

	public override bool CheckGameGuide()
	{
		if (Time.realtimeSinceStartup - this.m_nCheckTime > this.m_nDelayTime)
		{
			this.m_nCheckTime = Time.realtimeSinceStartup;
			return this.CheckGuide();
		}
		return false;
	}

	public override string GetGameGuideText()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return string.Empty;
		}
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(this.destSolID);
		if (soldierInfoFromSolID == null)
		{
			return string.Empty;
		}
		string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromToolTip, new object[]
		{
			textFromToolTip,
			"solname",
			soldierInfoFromSolID.GetName(),
			"grade1",
			ItemManager.ChangeRankToColorString(this.destItem.m_nOption[2]),
			"itemname1",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.destItem.m_nItemUnique),
			"grade2",
			ItemManager.ChangeRankToColorString(this.srcItem.m_nOption[2]),
			"itemname2",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.srcItem.m_nItemUnique)
		});
		return textFromToolTip;
	}
}
