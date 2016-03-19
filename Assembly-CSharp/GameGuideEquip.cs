using GAME;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;

public class GameGuideEquip : GameGuideInfo
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
		if (this.srcItem == null)
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
		if (soldierInfoFromSolID.GetSolPosType() == 2 || soldierInfoFromSolID.GetSolPosType() == 6)
		{
			return;
		}
		ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(this.srcItem.m_nItemUnique);
		if (itemTypeInfo == null)
		{
			return;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.srcItem.m_nItemUnique);
		if (itemInfo != null && itemInfo.m_nItemType == 19 && !soldierInfoFromSolID.IsAtbCommonFlag(2L))
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
		string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(this.srcItem.m_nItemUnique);
		if (!string.IsNullOrEmpty(itemMaterialCode))
		{
			TsAudioManager.Container.RequestAudioClip("UI_ITEM", itemMaterialCode, "DROP", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	private bool CheckGuide()
	{
		this.srcItem = null;
		this.destItem = null;
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
						ITEM iTEM = null;
						int num = soldierInfo.GetEquipItemInfo().HaveEquipItem(ref iTEM, soldierInfo);
						if (-1 < num && iTEM != null)
						{
							NkSoldierInfo soldierInfo2 = charPersonInfo.GetSoldierInfo(0);
							if (soldierInfo2 != null)
							{
								this.srcSolID = soldierInfo2.GetSolID();
							}
							ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
							if (itemInfo != null && itemInfo.m_nItemType == 19 && !soldierInfo.IsAtbCommonFlag(2L))
							{
								return false;
							}
							this.srcItem = iTEM;
							this.destSolID = soldierInfo.GetSolID();
							return true;
						}
						else
						{
							NrEquipItemInfo equipItemInfo = soldierInfo.GetEquipItemInfo();
							if (equipItemInfo != null)
							{
								for (int j = 0; j < 6; j++)
								{
									if (j != 5 || soldierInfo.IsAtbCommonFlag(2L))
									{
										if (0 < equipItemInfo.GetItemUnique(j))
										{
											if (equipItemInfo.GetItem(j).m_nOption[4] == 0)
											{
												ITEM batterItemByUnique = NkUserInventory.instance.GetBatterItemByUnique(equipItemInfo.GetItem(j), soldierInfo.GetLevel());
												if (batterItemByUnique != null)
												{
													this.destItem = equipItemInfo.GetItem(j);
													this.srcItem = batterItemByUnique;
													NkSoldierInfo soldierInfo3 = charPersonInfo.GetSoldierInfo(0);
													if (soldierInfo3 != null)
													{
														this.srcSolID = soldierInfo3.GetSolID();
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
							if (current.GetSolPosType() != 2 && current.GetSolPosType() != 6)
							{
								ITEM iTEM2 = null;
								int num2 = current.GetEquipItemInfo().HaveEquipItem(ref iTEM2, current);
								if (-1 < num2 && iTEM2 != null)
								{
									NkSoldierInfo soldierInfo4 = charPersonInfo.GetSoldierInfo(0);
									if (soldierInfo4 != null)
									{
										this.srcSolID = soldierInfo4.GetSolID();
									}
									ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM2.m_nItemUnique);
									bool result;
									if (itemInfo2 != null && itemInfo2.m_nItemType == 19 && !current.IsAtbCommonFlag(2L))
									{
										result = false;
										return result;
									}
									this.srcItem = iTEM2;
									this.destSolID = current.GetSolID();
									result = true;
									return result;
								}
								else
								{
									NrEquipItemInfo equipItemInfo2 = current.GetEquipItemInfo();
									if (equipItemInfo2 != null)
									{
										for (int k = 0; k < 6; k++)
										{
											if (k != 5 || current.IsAtbCommonFlag(2L))
											{
												if (0 < equipItemInfo2.GetItemUnique(k))
												{
													ITEM batterItemByUnique2 = NkUserInventory.instance.GetBatterItemByUnique(equipItemInfo2.GetItem(k), current.GetLevel());
													if (batterItemByUnique2 != null)
													{
														this.destItem = equipItemInfo2.GetItem(k);
														this.srcItem = batterItemByUnique2;
														NkSoldierInfo soldierInfo5 = charPersonInfo.GetSoldierInfo(0);
														if (soldierInfo5 != null)
														{
															this.srcSolID = soldierInfo5.GetSolID();
														}
														this.destSolID = current.GetSolID();
														bool result = true;
														return result;
													}
												}
											}
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
		if (this.srcItem != null && this.destItem == null)
		{
			string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromToolTip, new object[]
			{
				textFromToolTip,
				"solname",
				soldierInfoFromSolID.GetName(),
				"grade",
				ItemManager.ChangeRankToColorString(this.srcItem.m_nOption[2]),
				"itemname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.srcItem.m_nItemUnique)
			});
			return textFromToolTip;
		}
		if (this.srcItem != null && this.destItem != null)
		{
			string textFromToolTip2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip("2025");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromToolTip2, new object[]
			{
				textFromToolTip2,
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
			return textFromToolTip2;
		}
		return string.Empty;
	}
}
