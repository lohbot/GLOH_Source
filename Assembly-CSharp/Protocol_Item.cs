using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public static class Protocol_Item
{
	public struct Add_Item
	{
		public int m_nItemUnique;

		public int m_nItemNum;
	}

	public enum E_TEMP_ITEM_ID : long
	{
		BOX = -9223372036854775808L
	}

	public static Dictionary<int, ITEM> s_diSortItem = new Dictionary<int, ITEM>();

	public static Action s_deMoneyDelegate
	{
		get;
		set;
	}

	public static int GetItemPosType(int itemunique)
	{
		return (int)Protocol_Item.GetPosTypeByItemType(itemunique);
	}

	public static bool Enable_Use(int itemunique)
	{
		return NrTSingleton<ItemManager>.Instance.IsItemATB(itemunique, 8L);
	}

	public static bool Enable_SystemUse(int itemunique)
	{
		return NrTSingleton<ItemManager>.Instance.IsItemATB(itemunique, 128L);
	}

	public static bool Enable_Compose(int itemunique)
	{
		return NrTSingleton<ItemManager>.Instance.IsItemATB(itemunique, 4096L);
	}

	public static bool Is_EquipItem(int itemunique)
	{
		switch (Protocol_Item.GetStaticItemPart(itemunique))
		{
		case eITEM_PART.ITEMPART_WEAPON:
		case eITEM_PART.ITEMPART_ARMOR:
		case eITEM_PART.ITEMPART_ACCESSORY:
			return true;
		default:
			return false;
		}
	}

	public static bool Is_Rank(int itemunique)
	{
		switch (Protocol_Item.GetStaticItemPart(itemunique))
		{
		case eITEM_PART.ITEMPART_WEAPON:
		case eITEM_PART.ITEMPART_ARMOR:
		case eITEM_PART.ITEMPART_ACCESSORY:
			return true;
		default:
			return false;
		}
	}

	public static bool Is_Durability(int itemunique)
	{
		switch (Protocol_Item.GetStaticItemPart(itemunique))
		{
		case eITEM_PART.ITEMPART_WEAPON:
		case eITEM_PART.ITEMPART_ARMOR:
		case eITEM_PART.ITEMPART_ACCESSORY:
			return true;
		default:
			return false;
		}
	}

	public static int Is_Making_Number(int itemunique, int a_lItemNumber)
	{
		switch (Protocol_Item.GetStaticItemPart(itemunique))
		{
		case eITEM_PART.ITEMPART_MATERIAL:
		case eITEM_PART.ITEMPART_BOX:
		case eITEM_PART.ITEMPART_SUPPLY:
		case eITEM_PART.ITEMPART_ETC:
			if (a_lItemNumber > 1000)
			{
				return 1000;
			}
			return 0;
		}
		if (a_lItemNumber > 3)
		{
			return 3;
		}
		return 0;
	}

	public static bool CanPile(int itemunique)
	{
		switch (Protocol_Item.GetStaticItemPart(itemunique))
		{
		case eITEM_PART.ITEMPART_WEAPON:
		case eITEM_PART.ITEMPART_ARMOR:
		case eITEM_PART.ITEMPART_ACCESSORY:
			return false;
		default:
			return true;
		}
	}

	public static string GetItemPartText(eITEM_PART eItemPart)
	{
		switch (eItemPart)
		{
		case eITEM_PART.ITEMPART_WEAPON:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("756");
		case eITEM_PART.ITEMPART_ARMOR:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("757");
		case eITEM_PART.ITEMPART_MATERIAL:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("191");
		case eITEM_PART.ITEMPART_SUPPLY:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("192");
		case eITEM_PART.ITEMPART_QUEST:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("190");
		}
		return string.Empty;
	}

	public static eITEM_POSTYPE GetPosTypeByItemType(int itemunique)
	{
		return NrTSingleton<ItemManager>.Instance.GetPosTypeByItemType(itemunique);
	}

	public static eITEM_PART GetStaticItemPart(int itemunique)
	{
		return NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(itemunique);
	}

	public static eITEM_TYPE GetStaticItemType(int itemunique)
	{
		return NrTSingleton<ItemManager>.Instance.GetItemTypeByItemUnique(itemunique);
	}

	public static int GetStaticItemOption(int itemunique, int itemOption)
	{
		return NrTSingleton<ItemManager>.Instance.GetItemOptionByItemUnique(itemunique, itemOption);
	}

	public static string Money_Format(long a_lValue)
	{
		if (a_lValue == 0L)
		{
			return "0";
		}
		return string.Format("{0:#,###}", a_lValue);
	}

	public static string Money_Format(float a_lValue)
	{
		if (a_lValue == 0f)
		{
			return "0";
		}
		return string.Format("{0:#,###.##}", a_lValue);
	}

	public static long Get_Price(int itemunique)
	{
		SortedDictionary<int, ITEMINFO> total_Collection = NrTSingleton<ItemManager>.Instance.Get_Total_Collection();
		ITEMINFO iTEMINFO = null;
		if (total_Collection.TryGetValue(itemunique, out iTEMINFO))
		{
			return iTEMINFO.m_nPrice;
		}
		return -1L;
	}

	public static eITEM_SUPPLY_FUNCTION Get_Item_Supplies_Function_Index(int itemunique)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemunique);
		if (itemInfo != null)
		{
			return (eITEM_SUPPLY_FUNCTION)itemInfo.m_nFunctions;
		}
		return eITEM_SUPPLY_FUNCTION.SUPPLY_NORMAL;
	}

	public static bool MoveItem(EZDragDropParams dragDropParams)
	{
		if (dragDropParams.dragObj == null)
		{
			return false;
		}
		if (dragDropParams.dragObj.Data == null)
		{
			return false;
		}
		ImageSlot imageSlot = dragDropParams.dragObj.Data as ImageSlot;
		if (imageSlot == null)
		{
			return false;
		}
		if (imageSlot.c_oItem == null)
		{
			return false;
		}
		ITEM iTEM = imageSlot.c_oItem as ITEM;
		if (iTEM == null)
		{
			return false;
		}
		if (null == dragDropParams.dragObj.DropTarget)
		{
			return false;
		}
		int num = 0;
		UIListItemContainer component = dragDropParams.dragObj.DropTarget.GetComponent<UIListItemContainer>();
		if (null == component)
		{
			return false;
		}
		if (component.Data == null)
		{
			return false;
		}
		ImageSlot imageSlot2 = component.Data as ImageSlot;
		if (imageSlot2 == null)
		{
			return false;
		}
		if (imageSlot2.c_bDisable)
		{
			return false;
		}
		ITEM iTEM2 = imageSlot2.c_oItem as ITEM;
		if (iTEM2 != null)
		{
			num = iTEM2.m_nItemNum;
		}
		int windowID = imageSlot.WindowID;
		int windowID2 = imageSlot2.WindowID;
		int num2 = imageSlot2.Index;
		long solID = imageSlot._solID;
		long solID2 = imageSlot2._solID;
		GS_ITEM_MOVE_REQ gS_ITEM_MOVE_REQ = new GS_ITEM_MOVE_REQ();
		gS_ITEM_MOVE_REQ.m_nMoveType = NrTSingleton<ItemManager>.Instance.GetItemMoveType_InvenToInven(iTEM.m_nPosType);
		gS_ITEM_MOVE_REQ.m_nSrcItemID = iTEM.m_nItemID;
		gS_ITEM_MOVE_REQ.m_nSrcItemPos = iTEM.m_nItemPos;
		gS_ITEM_MOVE_REQ.m_nSrcSolID = solID;
		if (windowID == 231 && windowID2 == 79)
		{
			gS_ITEM_MOVE_REQ.m_nMoveType = NrTSingleton<ItemManager>.Instance.GetItemMoveType_InvenToSol(iTEM.m_nPosType);
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return false;
			}
			NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(solID2);
			if (soldierInfoFromSolID == null)
			{
				return false;
			}
			ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(iTEM.m_nItemUnique);
			if (itemTypeInfo == null)
			{
				return false;
			}
			if (!soldierInfoFromSolID.IsEquipClassType(itemTypeInfo.WEAPONTYPE, itemTypeInfo.EQUIPCLASSTYPE))
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("34");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
			if (NrTSingleton<ItemManager>.Instance.GetItemMinLevelFromItem(iTEM) > (int)soldierInfoFromSolID.GetLevel())
			{
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("358");
				Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
			num2 = (int)NrTSingleton<ItemManager>.Instance.GetEquipItemPos(iTEM.m_nItemUnique);
			ITEM equipItem = soldierInfoFromSolID.GetEquipItem(num2);
			iTEM2 = equipItem;
		}
		else if (windowID != 231 || windowID2 != 231)
		{
			if (windowID != 79 || windowID2 != 79)
			{
				if (windowID == 79 && windowID2 == 231)
				{
					int itemPosType = Protocol_Item.GetItemPosType(iTEM.m_nItemUnique);
					gS_ITEM_MOVE_REQ.m_nMoveType = NrTSingleton<ItemManager>.Instance.GetItemMoveType_SolToInven(itemPosType);
					if (iTEM2 != null && !Protocol_Item.Is_Item_Equipment(iTEM2, solID2))
					{
						return false;
					}
					if (iTEM2 != null && 0 < iTEM2.m_nItemUnique)
					{
						NrPersonInfoUser charPersonInfo2 = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
						if (charPersonInfo2 == null)
						{
							return false;
						}
						NkSoldierInfo soldierInfoFromSolID2 = charPersonInfo2.GetSoldierInfoFromSolID(solID2);
						if (soldierInfoFromSolID2 == null)
						{
							return false;
						}
						int equipItemPos = (int)NrTSingleton<ItemManager>.Instance.GetEquipItemPos(iTEM2.m_nItemUnique);
						ITEM equipItem2 = soldierInfoFromSolID2.GetEquipItem(equipItemPos);
						gS_ITEM_MOVE_REQ.m_nSrcItemID = equipItem2.m_nItemID;
						gS_ITEM_MOVE_REQ.m_nSrcItemPos = equipItemPos;
					}
				}
			}
		}
		if (iTEM2 != null)
		{
			gS_ITEM_MOVE_REQ.m_nDestItemID = iTEM2.m_nItemID;
			gS_ITEM_MOVE_REQ.m_nDestItemPos = num2;
			gS_ITEM_MOVE_REQ.m_nDestSolID = solID2;
		}
		else
		{
			gS_ITEM_MOVE_REQ.m_nDestItemID = 0L;
			gS_ITEM_MOVE_REQ.m_nDestItemPos = num2;
			gS_ITEM_MOVE_REQ.m_nDestSolID = solID2;
		}
		if ((gS_ITEM_MOVE_REQ.m_nMoveType == 3 || gS_ITEM_MOVE_REQ.m_nMoveType == 4 || gS_ITEM_MOVE_REQ.m_nMoveType == 5 || gS_ITEM_MOVE_REQ.m_nMoveType == 6) && gS_ITEM_MOVE_REQ.m_nSrcItemPos == gS_ITEM_MOVE_REQ.m_nDestItemPos)
		{
			return false;
		}
		if (iTEM.m_nItemNum + num > 9999999)
		{
			return false;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_MOVE_REQ, gS_ITEM_MOVE_REQ);
		return true;
	}

	public static void DeleteItem(ITEM a_cItem)
	{
		if (a_cItem != null)
		{
			if (a_cItem.IsLock())
			{
				return;
			}
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("52");
			string empty = string.Empty;
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(a_cItem);
			string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("14");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromMessageBox2,
				"Target_Item",
				itemNameByItemUnique
			});
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(Protocol_Item.On_Delete), a_cItem, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
		}
	}

	public static void On_Delete(object a_oObject)
	{
		ITEM iTEM = a_oObject as ITEM;
		if (iTEM != null)
		{
			TsAudioManager.Container.RequestAudioClip("UI_SFX", "ETC", "WASTEBASKET", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			GS_ITEMS_DELETE_REQ gS_ITEMS_DELETE_REQ = new GS_ITEMS_DELETE_REQ();
			gS_ITEMS_DELETE_REQ.m_byPosType = iTEM.m_nPosType;
			gS_ITEMS_DELETE_REQ.m_shPosItem = iTEM.m_nItemPos;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMS_DELETE_REQ, gS_ITEMS_DELETE_REQ);
		}
	}

	public static void Sort_Item_Clear()
	{
		Protocol_Item.s_diSortItem.Clear();
	}

	public static void Set_Sort_Item(ITEM a_cSortItem)
	{
		Protocol_Item.s_diSortItem.Add(a_cSortItem.m_nItemPos, a_cSortItem);
	}

	public static bool Is_Sort(int a_byPosType)
	{
		bool result = false;
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			if (NkUserInventory.GetInstance() != null)
			{
				result = true;
				break;
			}
			ITEM item = NkUserInventory.GetInstance().GetItem(a_byPosType, i);
			if (item != null)
			{
				if (item.m_nPosType != Protocol_Item.s_diSortItem[i].m_nPosType || item.m_nItemPos != Protocol_Item.s_diSortItem[i].m_nItemPos || item.m_nItemID != Protocol_Item.s_diSortItem[i].m_nItemID || item.m_nItemUnique != Protocol_Item.s_diSortItem[i].m_nItemUnique)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public static void Item_ShowItemInfo(G_ID eWidowID, ITEM pkItem, Vector3 showPosition, ITEM pkSecondItem, long SolID = 0L)
	{
		if (pkItem != null && pkItem.m_nItemUnique > 0)
		{
			ITEM iTEM = null;
			SoldierSelectDlg soldierSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLSELECT_DLG) as SoldierSelectDlg;
			SolEquipItemSelectDlg solEquipItemSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLEQUIPITEMSELECT_DLG) as SolEquipItemSelectDlg;
			if (pkSecondItem != null && (soldierSelectDlg != null || solEquipItemSelectDlg != null))
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_SECOND_DLG);
				iTEM = pkSecondItem;
				if (iTEM != null && iTEM.IsValid())
				{
					ItemTooltipDlg_Second itemTooltipDlg_Second = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_SECOND_DLG) as ItemTooltipDlg_Second;
					itemTooltipDlg_Second.Set_Tooltip(eWidowID, iTEM, true, showPosition, SolID);
				}
			}
			ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
			itemTooltipDlg.SolID = SolID;
			itemTooltipDlg.Set_Tooltip(eWidowID, pkItem, iTEM, showPosition, false);
		}
	}

	public static void Item_Use(ITEM pkItem)
	{
		if (Scene.IsCurScene(Scene.Type.BATTLE))
		{
			return;
		}
		if (pkItem != null)
		{
			if (!NrTSingleton<ItemManager>.Instance.IsUsableItem(pkItem.m_nItemUnique))
			{
				return;
			}
			switch (Protocol_Item.GetStaticItemPart(pkItem.m_nItemUnique))
			{
			case eITEM_PART.ITEMPART_WEAPON:
			case eITEM_PART.ITEMPART_ARMOR:
			case eITEM_PART.ITEMPART_ACCESSORY:
			{
				bool flag = false;
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null && solMilitaryGroupDlg.Visible)
				{
					NkSoldierInfo selectedSolinfo = solMilitaryGroupDlg.GetSelectedSolinfo();
					if (selectedSolinfo != null)
					{
						Protocol_Item.Item_Use(pkItem, selectedSolinfo.GetSolID());
						flag = true;
					}
				}
				if (!flag)
				{
					SoldierSelectDlg soldierSelectDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLSELECT_DLG) as SoldierSelectDlg;
					if (soldierSelectDlg != null)
					{
						soldierSelectDlg.InitData();
						soldierSelectDlg.SetDataItem(pkItem);
					}
				}
				break;
			}
			case eITEM_PART.ITEMPART_BOX:
			{
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEM_BOX_RANDOM_DLG) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEM_BOX_SELECT_DLG) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEM_BOX_ALL_DLG) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEM_BOX_RARERANDOM_DLG))
				{
					return;
				}
				ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
				ITEM_BOX_GROUP iTEM_BOX_GROUP = null;
				if (itemInfo.IsItemATB(65536L))
				{
					iTEM_BOX_GROUP = NrTSingleton<ItemManager>.Instance.GetBoxGroup(pkItem.m_nItemUnique);
					if (iTEM_BOX_GROUP == null)
					{
						return;
					}
				}
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				NkSoldierInfo userSoldierInfo = nrCharUser.GetUserSoldierInfo();
				int level = (int)userSoldierInfo.GetLevel();
				if (itemInfo != null && level >= itemInfo.GetUseMinLevel(pkItem) && (itemInfo.m_nUseMaxLevel == 0 || level <= itemInfo.m_nUseMaxLevel))
				{
					if (!NrTSingleton<ItemManager>.Instance.CheckBoxNeedItem(itemInfo.m_nItemUnique, true, true))
					{
						return;
					}
					if (itemInfo.IsItemATB(16L))
					{
						for (int i = 0; i < 12; i++)
						{
							int num;
							int itemnum;
							if (iTEM_BOX_GROUP != null)
							{
								num = iTEM_BOX_GROUP.i32GroupItemUnique[i];
								itemnum = iTEM_BOX_GROUP.i32GroupItemNum[i];
							}
							else
							{
								num = itemInfo.m_nBoxItemUnique[i];
								itemnum = itemInfo.m_nBoxItemNumber[i];
							}
							if (num > 0)
							{
								int itemPosType = Protocol_Item.GetItemPosType(num);
								if (itemPosType > 0)
								{
									int num2 = NkUserInventory.GetInstance().Get_Tab_List_Count(itemPosType);
									int num3 = 30;
									if (num2 >= num3)
									{
										if (!Protocol_Item.CanPile(num))
										{
											string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46");
											Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
											return;
										}
										if (!Protocol_Item.CanAddItem(num, itemnum))
										{
											string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46");
											Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
											return;
										}
									}
								}
							}
						}
						if (itemInfo.IsItemATB(16384L))
						{
							Item_Box_RareRandom_Dlg item_Box_RareRandom_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_RARERANDOM_DLG) as Item_Box_RareRandom_Dlg;
							item_Box_RareRandom_Dlg.Set_Item(pkItem);
						}
						else
						{
							Protocol_Item_Box.Item_Box_Random_Show(pkItem);
						}
					}
					else if (itemInfo.IsItemATB(32L))
					{
						NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_SELECT_DLG);
						Item_Box_Select_Dlg item_Box_Select_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_SELECT_DLG) as Item_Box_Select_Dlg;
						item_Box_Select_Dlg.Set_Item(pkItem);
					}
					else if (itemInfo.IsItemATB(64L))
					{
						NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_ALL_DLG);
						Item_Box_All_Dlg item_Box_All_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_ALL_DLG) as Item_Box_All_Dlg;
						item_Box_All_Dlg.Set_Item(pkItem);
					}
				}
				else if (level < itemInfo.GetUseMinLevel(pkItem))
				{
					string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("356");
					Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else if (level > itemInfo.m_nUseMaxLevel)
				{
					string textFromNotify4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("483");
					Main_UI_SystemMessage.ADDMessage(textFromNotify4, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				break;
			}
			case eITEM_PART.ITEMPART_SUPPLY:
				if (NrTSingleton<ItemManager>.Instance.IsItemATB(pkItem.m_nItemUnique, 512L))
				{
					GS_TICKET_SELL_INFO_REQ obj = new GS_TICKET_SELL_INFO_REQ();
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TICKET_SELL_INFO_REQ, obj);
					return;
				}
				if (NrTSingleton<ItemManager>.Instance.IsItemATB(pkItem.m_nItemUnique, 1024L))
				{
					ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
					if (itemInfo2 == null)
					{
						return;
					}
					SolRecruitDlg.ExcuteTicket(pkItem.m_nItemUnique, itemInfo2.m_nParam[0], itemInfo2.m_nParam[1], false);
					return;
				}
				else if (NrTSingleton<ItemManager>.Instance.IsItemATB(pkItem.m_nItemUnique, 262144L))
				{
					ITEMINFO itemInfo3 = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
					if (itemInfo3 == null)
					{
						return;
					}
					SolRecruitDlg.ExcuteTicket(pkItem.m_nItemUnique, itemInfo3.m_nParam[0], itemInfo3.m_nParam[1], false);
					return;
				}
				else
				{
					if (NrTSingleton<ItemManager>.Instance.IsItemATB(pkItem.m_nItemUnique, 2048L) || NrTSingleton<ItemManager>.Instance.IsItemATB(pkItem.m_nItemUnique, 32768L))
					{
						NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
						if (readySolList == null || readySolList.GetCount() >= 50)
						{
							string textFromNotify5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
							Main_UI_SystemMessage.ADDMessage(textFromNotify5, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
							return;
						}
					}
					SolMilitaryGroupDlg solMilitaryGroupDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
					long solID;
					if (solMilitaryGroupDlg2 != null && solMilitaryGroupDlg2.Visible)
					{
						NkSoldierInfo selectedSolinfo2 = solMilitaryGroupDlg2.GetSelectedSolinfo();
						solID = selectedSolinfo2.GetSolID();
					}
					else
					{
						PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
						if (plunderSolListDlg != null && plunderSolListDlg.Visible)
						{
							NkSoldierInfo selectedSolinfo3 = plunderSolListDlg.GetSelectedSolinfo();
							solID = selectedSolinfo3.GetSolID();
						}
						else
						{
							NrCharUser nrCharUser2 = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
							NkSoldierInfo userSoldierInfo2 = nrCharUser2.GetUserSoldierInfo();
							solID = userSoldierInfo2.GetSolID();
						}
					}
					GS_ITEM_SUPPLY_USE_REQ gS_ITEM_SUPPLY_USE_REQ = new GS_ITEM_SUPPLY_USE_REQ();
					gS_ITEM_SUPPLY_USE_REQ.m_nItemUnique = pkItem.m_nItemUnique;
					gS_ITEM_SUPPLY_USE_REQ.m_nDestSolID = solID;
					gS_ITEM_SUPPLY_USE_REQ.m_shItemNum = 1;
					gS_ITEM_SUPPLY_USE_REQ.m_byPosType = pkItem.m_nPosType;
					gS_ITEM_SUPPLY_USE_REQ.m_shPosItem = pkItem.m_nItemPos;
					ITEMINFO itemInfo4 = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
					if (itemInfo4 != null)
					{
						MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
						if (itemInfo4.m_nFunctions == 6)
						{
							if (msgBoxUI != null)
							{
								string empty = string.Empty;
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
								{
									NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1553"),
									"targetname",
									NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(itemInfo4.m_nParam[1].ToString())
								});
								msgBoxUI.SetMsg(new YesDelegate(NrTSingleton<ItemManager>.Instance.ItemSupplyUseReq), gS_ITEM_SUPPLY_USE_REQ, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1552"), empty, eMsgType.MB_OK_CANCEL);
								msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("320"));
								msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("321"));
							}
						}
						else if (itemInfo4.m_nFunctions == 10)
						{
							NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
							if (myCharInfo != null && msgBoxUI != null)
							{
								string empty2 = string.Empty;
								int num4 = itemInfo4.m_nParam[0] / 60;
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
								{
									NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("209"),
									"timestring",
									num4.ToString()
								});
								string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("150");
								msgBoxUI.SetMsg(new YesDelegate(NrTSingleton<ItemManager>.Instance.ItemSupplyUseReq), gS_ITEM_SUPPLY_USE_REQ, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL);
								msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("320"));
								msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("321"));
							}
						}
						else
						{
							SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SUPPLY_USE_REQ, gS_ITEM_SUPPLY_USE_REQ);
						}
						NrTSingleton<ItemManager>.Instance.PlayItemUseSound(pkItem.m_nItemUnique, false);
					}
				}
				break;
			case eITEM_PART.ITEMPART_QUEST:
			{
				SolMilitaryGroupDlg solMilitaryGroupDlg3 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				long solID2;
				if (solMilitaryGroupDlg3 != null && solMilitaryGroupDlg3.Visible)
				{
					NkSoldierInfo selectedSolinfo4 = solMilitaryGroupDlg3.GetSelectedSolinfo();
					solID2 = selectedSolinfo4.GetSolID();
				}
				else
				{
					NrCharUser nrCharUser3 = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
					NkSoldierInfo userSoldierInfo3 = nrCharUser3.GetUserSoldierInfo();
					solID2 = userSoldierInfo3.GetSolID();
				}
				GS_ITEM_SUPPLY_USE_REQ gS_ITEM_SUPPLY_USE_REQ2 = new GS_ITEM_SUPPLY_USE_REQ();
				gS_ITEM_SUPPLY_USE_REQ2.m_nItemUnique = pkItem.m_nItemUnique;
				gS_ITEM_SUPPLY_USE_REQ2.m_nDestSolID = solID2;
				gS_ITEM_SUPPLY_USE_REQ2.m_shItemNum = 1;
				gS_ITEM_SUPPLY_USE_REQ2.m_byPosType = pkItem.m_nPosType;
				gS_ITEM_SUPPLY_USE_REQ2.m_shPosItem = pkItem.m_nItemPos;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SUPPLY_USE_REQ, gS_ITEM_SUPPLY_USE_REQ2);
				NrTSingleton<ItemManager>.Instance.PlayItemUseSound(pkItem.m_nItemUnique, false);
				break;
			}
			}
		}
	}

	public static void Item_Use(ITEM pkItem, long solid)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(solid);
		if (pkItem != null && soldierInfoFromSolID != null)
		{
			if (!NrTSingleton<ItemManager>.Instance.IsUsableItem(pkItem.m_nItemUnique))
			{
				return;
			}
			if (soldierInfoFromSolID.GetSolPosType() == 2 || soldierInfoFromSolID.GetSolPosType() == 6)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("368"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			switch (Protocol_Item.GetStaticItemPart(pkItem.m_nItemUnique))
			{
			case eITEM_PART.ITEMPART_WEAPON:
			case eITEM_PART.ITEMPART_ARMOR:
			case eITEM_PART.ITEMPART_ACCESSORY:
				if (soldierInfoFromSolID != null && Protocol_Item.Is_Item_Equipment(pkItem, soldierInfoFromSolID))
				{
					Protocol_Item.Send_InvenEquip_EquipSol(pkItem, soldierInfoFromSolID);
				}
				break;
			case eITEM_PART.ITEMPART_SUPPLY:
			case eITEM_PART.ITEMPART_QUEST:
			{
				long nDestSolID = 0L;
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null && solMilitaryGroupDlg.Visible)
				{
					NkSoldierInfo selectedSolinfo = solMilitaryGroupDlg.GetSelectedSolinfo();
					nDestSolID = selectedSolinfo.GetSolID();
				}
				GS_ITEM_SUPPLY_USE_REQ gS_ITEM_SUPPLY_USE_REQ = new GS_ITEM_SUPPLY_USE_REQ();
				gS_ITEM_SUPPLY_USE_REQ.m_nItemUnique = pkItem.m_nItemUnique;
				gS_ITEM_SUPPLY_USE_REQ.m_nDestSolID = nDestSolID;
				gS_ITEM_SUPPLY_USE_REQ.m_shItemNum = 1;
				gS_ITEM_SUPPLY_USE_REQ.m_byPosType = pkItem.m_nPosType;
				gS_ITEM_SUPPLY_USE_REQ.m_shPosItem = pkItem.m_nItemPos;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SUPPLY_USE_REQ, gS_ITEM_SUPPLY_USE_REQ);
				break;
			}
			}
		}
	}

	public static bool Is_Use_Item(ITEM pkItem, NkSoldierInfo pkSolinfo, bool a_bIsMessageVisible)
	{
		bool result = false;
		if (pkItem != null && pkSolinfo != null)
		{
			int nItemUnique = pkItem.m_nItemUnique;
			switch (Protocol_Item.GetStaticItemPart(nItemUnique))
			{
			case eITEM_PART.ITEMPART_WEAPON:
			case eITEM_PART.ITEMPART_ARMOR:
			case eITEM_PART.ITEMPART_ACCESSORY:
				result = Protocol_Item.Is_Item_Equipment(pkItem, pkSolinfo, a_bIsMessageVisible);
				break;
			case eITEM_PART.ITEMPART_SUPPLY:
			{
				ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(nItemUnique);
				if ((int)pkSolinfo.GetLevel() >= itemInfo.GetUseMinLevel(pkItem) && (int)pkSolinfo.GetLevel() <= itemInfo.m_nUseMaxLevel)
				{
					result = true;
				}
				break;
			}
			}
		}
		return result;
	}

	public static void Send_InvenEquip_EquipSol(ITEM pkItem, NkSoldierInfo pkSolinfo)
	{
		if (pkItem != null && pkSolinfo != null)
		{
			pkSolinfo.EquipmentItem(pkItem);
		}
	}

	public static bool Send_EquipSol_InvenEquip(ITEM pkItem)
	{
		long solID = 0L;
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg != null && solMilitaryGroupDlg.Visible)
		{
			NkSoldierInfo selectedSolinfo = solMilitaryGroupDlg.GetSelectedSolinfo();
			if (selectedSolinfo == null)
			{
				return false;
			}
			solID = selectedSolinfo.GetSolID();
		}
		return Protocol_Item.Send_EquipSol_InvenEquip(pkItem, solID);
	}

	public static bool Send_EquipSol_InvenEquip(ITEM pkItem, long solID)
	{
		if (pkItem != null)
		{
			GS_ITEM_MOVE_REQ gS_ITEM_MOVE_REQ = new GS_ITEM_MOVE_REQ();
			if (solID > 0L)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(solID);
				if (soldierInfoFromSolID == null)
				{
					return false;
				}
				if (soldierInfoFromSolID.GetSolPosType() == 2 || soldierInfoFromSolID.GetSolPosType() == 6)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("367"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					return false;
				}
			}
			int itemPosType = Protocol_Item.GetItemPosType(pkItem.m_nItemUnique);
			gS_ITEM_MOVE_REQ.m_nMoveType = NrTSingleton<ItemManager>.Instance.GetItemMoveType_SolToInven(itemPosType);
			gS_ITEM_MOVE_REQ.m_nSrcItemID = pkItem.m_nItemID;
			gS_ITEM_MOVE_REQ.m_nSrcItemPos = pkItem.m_nItemPos;
			gS_ITEM_MOVE_REQ.m_nSrcSolID = solID;
			int emptySlot = NkUserInventory.GetInstance().GetEmptySlot(itemPosType);
			if (emptySlot == -1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
				return false;
			}
			long nDestSolID = 0L;
			NrPersonInfoUser charPersonInfo2 = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo2 != null)
			{
				NkSoldierInfo soldierInfo = charPersonInfo2.GetSoldierInfo(0);
				if (soldierInfo != null)
				{
					nDestSolID = soldierInfo.GetSolID();
				}
			}
			gS_ITEM_MOVE_REQ.m_nDestItemID = 0L;
			gS_ITEM_MOVE_REQ.m_nDestItemPos = emptySlot;
			gS_ITEM_MOVE_REQ.m_nDestSolID = nDestSolID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_MOVE_REQ, gS_ITEM_MOVE_REQ);
			TsLog.LogWarning("Send_EquipSol_InvenEquip ItemId = {0}", new object[]
			{
				pkItem.m_nItemID
			});
		}
		return true;
	}

	public static void Send_EquipSol_InvenEquip_All(NkSoldierInfo kSolInfo)
	{
		int num = 0;
		int num2 = 0;
		List<int> list = new List<int>();
		long solID = kSolInfo.GetSolID();
		for (int i = 0; i < 6; i++)
		{
			ITEM equipItem = kSolInfo.GetEquipItem(i);
			if (equipItem != null && 0 < equipItem.m_nItemUnique)
			{
				num++;
			}
		}
		for (int j = 0; j < 6; j++)
		{
			ITEM equipItem2 = kSolInfo.GetEquipItem(j);
			if (equipItem2 != null && 0 < equipItem2.m_nItemUnique && equipItem2 != null)
			{
				GS_ITEM_MOVE_REQ gS_ITEM_MOVE_REQ = new GS_ITEM_MOVE_REQ();
				int itemPosType = Protocol_Item.GetItemPosType(equipItem2.m_nItemUnique);
				gS_ITEM_MOVE_REQ.m_nMoveType = NrTSingleton<ItemManager>.Instance.GetItemMoveType_SolToInven(itemPosType);
				gS_ITEM_MOVE_REQ.m_nSrcItemID = equipItem2.m_nItemID;
				gS_ITEM_MOVE_REQ.m_nSrcItemPos = equipItem2.m_nItemPos;
				gS_ITEM_MOVE_REQ.m_nSrcSolID = solID;
				list.Clear();
				NkUserInventory.GetInstance().GetEmptySlot(itemPosType, ref list);
				if (num2 >= list.Count)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
					return;
				}
				int nDestItemPos = list[num2++];
				long nDestSolID = 0L;
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo != null)
				{
					NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(0);
					if (soldierInfo != null)
					{
						nDestSolID = soldierInfo.GetSolID();
					}
				}
				gS_ITEM_MOVE_REQ.m_nDestItemID = 0L;
				gS_ITEM_MOVE_REQ.m_nDestItemPos = nDestItemPos;
				gS_ITEM_MOVE_REQ.m_nDestSolID = nDestSolID;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_MOVE_REQ, gS_ITEM_MOVE_REQ);
				TsLog.LogWarning("Send_EquipSol_InvenEquip ItemId = {0}", new object[]
				{
					equipItem2.m_nItemID
				});
			}
		}
	}

	public static bool Is_Item_Equipment(ITEM pkItem, long solid)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(solid);
		return Protocol_Item.Is_Item_Equipment(pkItem, soldierInfoFromSolID);
	}

	public static bool Is_Item_Equipment(ITEM pkItem, int solid)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID((long)solid);
		return Protocol_Item.Is_Item_Equipment(pkItem, soldierInfoFromSolID);
	}

	public static bool Is_Item_Equipment(ITEM pkItem, int solid, bool bMessageView)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID((long)solid);
		return Protocol_Item.Is_Item_Equipment(pkItem, soldierInfoFromSolID, bMessageView);
	}

	public static bool UserEnableWeapon(ref int itemunique, ref NkSoldierInfo pkSolinfo, eITEM_PART type)
	{
		return Protocol_Item.UserEnableWeapon(ref itemunique, ref pkSolinfo, type, true);
	}

	public static bool UserEnableWeapon(ref int itemunique, ref NkSoldierInfo pkSolinfo, eITEM_PART eItemPart, bool _IsMessageVisible)
	{
		return true;
	}

	public static bool Is_Item_Equipment(ITEM pkItem, NkSoldierInfo pkSolinfo)
	{
		return Protocol_Item.Is_Item_Equipment(pkItem, pkSolinfo, true);
	}

	public static bool Is_Item_Equipment(ITEM pkItem, NkSoldierInfo pkSolinfo, bool _IsMessageVisible)
	{
		if (pkItem == null || pkSolinfo == null)
		{
			return false;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
		if (itemInfo == null)
		{
			return false;
		}
		short level = pkSolinfo.GetLevel();
		if (itemInfo.GetUseMinLevel(pkItem) > 0 && (int)level < itemInfo.GetUseMinLevel(pkItem))
		{
			return false;
		}
		if (itemInfo.m_nUseMaxLevel > 0 && (int)level > itemInfo.m_nUseMaxLevel)
		{
			return false;
		}
		ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(pkItem.m_nItemUnique);
		if (itemTypeInfo == null)
		{
			return false;
		}
		if (!pkSolinfo.IsEquipClassType(itemTypeInfo.WEAPONTYPE, itemTypeInfo.EQUIPCLASSTYPE))
		{
			if (_IsMessageVisible)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("34");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			return false;
		}
		if (itemInfo.m_nItemType == 19 && !pkSolinfo.GetCharKindInfo().IsATB(1L) && !pkSolinfo.IsAtbCommonFlag(2L))
		{
			return false;
		}
		if (pkItem.m_nDurability <= 0)
		{
			if (_IsMessageVisible)
			{
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("197");
				Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			return false;
		}
		int nItemUnique = pkItem.m_nItemUnique;
		eITEM_PART staticItemPart = Protocol_Item.GetStaticItemPart(nItemUnique);
		return staticItemPart == eITEM_PART.ITEMPART_WEAPON || staticItemPart == eITEM_PART.ITEMPART_ARMOR || true;
	}

	public static bool CanAddItem(ITEM pkItem)
	{
		return Protocol_Item.CanAddItem(pkItem.m_nItemUnique, pkItem.m_nItemNum);
	}

	public static bool CanAddItem(int itemunique, int itemnum)
	{
		int itemPosType = Protocol_Item.GetItemPosType(itemunique);
		switch (itemPosType)
		{
		case 1:
		case 2:
		case 3:
		case 4:
		{
			if (NkUserInventory.GetInstance() != null)
			{
				return true;
			}
			int num = Protocol_Item.Get_Enable_Slot_Count(itemPosType);
			short num2 = 0;
			while ((int)num2 < num)
			{
				if (NkUserInventory.GetInstance().GetItem(itemPosType, (int)num2) == null)
				{
					return true;
				}
				num2 += 1;
			}
			break;
		}
		case 5:
		case 6:
		case 7:
		{
			if (NkUserInventory.GetInstance() != null)
			{
				return true;
			}
			int num3 = Protocol_Item.Get_Enable_Slot_Count(itemPosType);
			short num4 = 0;
			while ((int)num4 < num3)
			{
				if (NkUserInventory.GetInstance().GetItem(itemPosType, (int)num4) == null)
				{
					return true;
				}
				num4 += 1;
			}
			for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
			{
				ITEM item = NkUserInventory.GetInstance().GetItem(itemPosType, i);
				if (item != null && item.m_nItemUnique == itemunique && item.m_nItemNum + itemnum < 9999999)
				{
					return num3 > item.m_nItemPos;
				}
			}
			break;
		}
		}
		return false;
	}

	public static int Is_Enable_Slot(int a_nPosType, short a_shPosItem)
	{
		return 1;
	}

	public static int Get_Enable_Slot_Count(int a_byPosType)
	{
		if (a_byPosType != 7)
		{
		}
		return 30;
	}

	public static float Get_Weight(int a_nItemUnique, int a_nRank)
	{
		int itemQuailtyLevel = NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(a_nItemUnique);
		Item_Rank item_Rank = Item_Rank_Manager.Get_Instance().Get_RankData(itemQuailtyLevel, a_nRank);
		if (item_Rank != null)
		{
			return (float)item_Rank.ItemPerformanceRate / 100f;
		}
		return 1f;
	}

	public static int Get_Min_Damage(ITEM pkItem)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(pkItem.m_nItemUnique, pkItem.m_nRank);
			return (int)((float)itemInfo.m_nMinDamage * num);
		}
		return 0;
	}

	public static int Get_Min_Damage(int itemUnique, int Rank)
	{
		Rank = 0;
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(itemUnique, Rank);
			return (int)((float)itemInfo.m_nMinDamage * num);
		}
		return 0;
	}

	public static int Get_Max_Damage(ITEM pkItem)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(pkItem.m_nItemUnique, pkItem.m_nRank);
			return (int)((float)itemInfo.m_nMaxDamage * num);
		}
		return 0;
	}

	public static int Get_Max_Damage(int itemUnique, int Rank)
	{
		Rank = 0;
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(itemUnique, Rank);
			return (int)((float)itemInfo.m_nMaxDamage * num);
		}
		return 0;
	}

	public static int Get_Defense(ITEM pkItem)
	{
		return Protocol_Item.Get_Defense(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_Defense(int itemUnique, int Rank)
	{
		Rank = 0;
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(itemUnique, Rank);
			return (int)((float)itemInfo.m_nDefense * num);
		}
		return 0;
	}

	public static int Get_Magic_Defense(ITEM pkItem)
	{
		return Protocol_Item.Get_Magic_Defense(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_Magic_Defense(int ItemUnique, int Rank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(ItemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(ItemUnique, Rank);
			return (int)((float)itemInfo.m_nMagicDefense * num);
		}
		return 0;
	}

	public static int Get_ADDHP(ITEM pkItem)
	{
		return Protocol_Item.Get_ADDHP(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_ADDHP(int ItemUnique, int Rank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(ItemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(ItemUnique, Rank);
			return (int)((float)itemInfo.m_nAddHP * num);
		}
		return 0;
	}

	public static int Get_STR(ITEM pkItem)
	{
		return Protocol_Item.Get_STR(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_STR(int ItemUnique, int Rank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(ItemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(ItemUnique, Rank);
			return (int)((float)itemInfo.m_nSTR * num);
		}
		return 0;
	}

	public static int Get_DEX(ITEM pkItem)
	{
		return Protocol_Item.Get_DEX(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_DEX(int ItemUnique, int Rank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(ItemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(ItemUnique, Rank);
			return (int)((float)itemInfo.m_nDEX * num);
		}
		return 0;
	}

	public static int Get_INT(ITEM pkItem)
	{
		return Protocol_Item.Get_INT(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_INT(int ItemUnique, int Rank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(ItemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(ItemUnique, Rank);
			return (int)((float)itemInfo.m_nINT * num);
		}
		return 0;
	}

	public static int Get_VIT(ITEM pkItem)
	{
		return Protocol_Item.Get_VIT(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_VIT(int ItemUnique, int Rank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(ItemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(ItemUnique, Rank);
			return (int)((float)itemInfo.m_nVIT * num);
		}
		return 0;
	}

	public static int Get_Critical_Plus(ITEM pkItem)
	{
		return Protocol_Item.Get_Critical_Plus(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_Critical_Plus(int ItemUnique, int Rank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(ItemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(ItemUnique, Rank);
			return (int)((float)itemInfo.m_nCriticalPlus * num);
		}
		return 0;
	}

	public static int Get_AttackSpeed(ITEM pkItem)
	{
		return Protocol_Item.Get_AttackSpeed(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_AttackSpeed(int itemUnique, int nRank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(itemUnique, nRank);
			return (int)((float)itemInfo.m_nAttackSpeed * num);
		}
		return 0;
	}

	public static int Get_Hitrate_Plus(ITEM pkItem)
	{
		return Protocol_Item.Get_Hitrate_Plus(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_Hitrate_Plus(int itemUnique, int nRank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(itemUnique, nRank);
			return (int)((float)itemInfo.m_nHitratePlus * num);
		}
		return 0;
	}

	public static int Get_Evasion_Plus(ITEM pkItem)
	{
		return Protocol_Item.Get_Evasion_Plus(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_Evasion_Plus(int itemUnique, int nRank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(itemUnique, nRank);
			return (int)((float)itemInfo.m_nEvasionPlus * num);
		}
		return 0;
	}

	public static int Get_Move_Speed(ITEM pkItem)
	{
		return Protocol_Item.Get_Move_Speed(pkItem.m_nItemUnique, pkItem.m_nRank);
	}

	public static int Get_Move_Speed(int itemUnique, int nRank)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemUnique);
		if (itemInfo != null)
		{
			float num = Protocol_Item.Get_Weight(itemUnique, nRank);
			return (int)((float)itemInfo.m_nMoveSpeed * num);
		}
		return 0;
	}

	public static void Send_AutoItemSell(long lItemID)
	{
		if (0L >= lItemID)
		{
			return;
		}
		GS_ITEM_SELL_REQ gS_ITEM_SELL_REQ = new GS_ITEM_SELL_REQ();
		gS_ITEM_SELL_REQ.i64ItemID = lItemID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SELL_REQ, gS_ITEM_SELL_REQ);
	}
}
