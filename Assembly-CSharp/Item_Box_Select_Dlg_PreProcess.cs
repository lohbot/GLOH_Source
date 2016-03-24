using GAME;
using System;
using UnityEngine;
using UnityForms;

public class Item_Box_Select_Dlg_PreProcess
{
	public bool PreProcess(ITEM item, int index, YesDelegate a_deYes)
	{
		if (item == null)
		{
			Debug.LogError("ERROR, Item_Box_Select_Dlg.cs, PreProcess_BoxOpen(), item Is Null");
			return false;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
		if (itemInfo == null)
		{
			return false;
		}
		if (itemInfo.m_nBoxItemUnique.Length <= index)
		{
			Debug.LogError("ERROR, Item_Box_Select_Dlg.cs, PreProcess_BoxOpen(), index out of range");
			return false;
		}
		return this.CostumeBoxPreProcess(item, index, a_deYes);
	}

	private bool CostumeBoxPreProcess(ITEM item, int index, YesDelegate a_deYes)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
		if (!itemInfo.IsItemATB(33554432L))
		{
			return false;
		}
		int itemunique = itemInfo.m_nBoxItemUnique[index];
		ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemunique);
		if (itemInfo2 == null)
		{
			return false;
		}
		if (itemInfo2.m_nFunctions != 14)
		{
			return false;
		}
		int num = itemInfo2.m_nParam[0];
		int costumeSolKind = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeSolKind(num);
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845");
		if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsMySolKindExist(costumeSolKind))
		{
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(textFromInterface, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("355"), eMsgType.MB_OK_CANCEL, a_deYes, index);
			return true;
		}
		if (NrTSingleton<NrCharCostumeTableManager>.Instance.IsBuyCostume(num))
		{
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(textFromInterface, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("356"), eMsgType.MB_OK_CANCEL, a_deYes, index);
			return true;
		}
		return false;
	}
}
