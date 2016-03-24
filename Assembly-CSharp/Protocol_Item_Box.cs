using GAME;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public static class Protocol_Item_Box
{
	public struct Roulette_Item
	{
		public int m_nItemUnique;

		public string m_strText;
	}

	public static void On_Sead_Box_Use_Random(object a_oObject)
	{
		ITEM iTEM = a_oObject as ITEM;
		if (iTEM != null)
		{
			GS_BOX_USE_REQ gS_BOX_USE_REQ = new GS_BOX_USE_REQ();
			gS_BOX_USE_REQ.m_nItemID = iTEM.m_nItemID;
			gS_BOX_USE_REQ.m_nItemUnique = iTEM.m_nItemUnique;
			gS_BOX_USE_REQ.m_nPosType = iTEM.m_nPosType;
			gS_BOX_USE_REQ.m_nItemPos = iTEM.m_nItemPos;
			gS_BOX_USE_REQ.m_nArrayIndex = 0;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BOX_USE_REQ, gS_BOX_USE_REQ);
		}
	}

	public static void Item_Box_Random_Show(ITEM a_cItem)
	{
		if (a_cItem != null)
		{
			List<Protocol_Item_Box.Roulette_Item> list = new List<Protocol_Item_Box.Roulette_Item>();
			Protocol_Item_Box.Roulette_Item item = default(Protocol_Item_Box.Roulette_Item);
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(a_cItem.m_nItemUnique);
			ITEM_BOX_GROUP iTEM_BOX_GROUP = null;
			if (itemInfo.IsItemATB(65536L))
			{
				iTEM_BOX_GROUP = NrTSingleton<ItemManager>.Instance.GetBoxGroup(itemInfo.m_nItemUnique);
				if (iTEM_BOX_GROUP == null)
				{
					return;
				}
			}
			for (int i = 0; i < 12; i++)
			{
				int num;
				int num2;
				if (iTEM_BOX_GROUP != null)
				{
					num = iTEM_BOX_GROUP.i32GroupItemUnique[i];
					num2 = iTEM_BOX_GROUP.i32GroupItemNum[i];
				}
				else
				{
					num = itemInfo.m_nBoxItemUnique[i];
					num2 = itemInfo.m_nBoxItemNumber[i];
				}
				if (num > 0)
				{
					item.m_nItemUnique = num;
					item.m_strText = NrTSingleton<UIDataManager>.Instance.GetString(num2.ToString(), " ", NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442"));
					list.Add(item);
				}
			}
			Protocol_Item_Box.Roulette_Show(a_cItem.m_nItemUnique, new Action<object>(Protocol_Item_Box.On_Sead_Box_Use_Random), a_cItem, list.ToArray());
		}
	}

	public static void Roulette_Show(int a_nItemUniuqe, Action<object> a_deDelegate, object a_oobject, Protocol_Item_Box.Roulette_Item[] a_saRouletteItem)
	{
		Item_Box_Random_Dlg item_Box_Random_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_RANDOM_DLG) as Item_Box_Random_Dlg;
		item_Box_Random_Dlg.Set_Item(a_nItemUniuqe, a_deDelegate, a_oobject, a_saRouletteItem);
	}

	public static void Roulette_Complete(int a_nItemUniuqe, int a_nItemNum)
	{
	}

	public static void On_Sead_Box_Trade_Random(object a_oObject)
	{
	}

	public static void Item_Box_Hearttype_Random_Show(ITEM a_cItem)
	{
	}
}
