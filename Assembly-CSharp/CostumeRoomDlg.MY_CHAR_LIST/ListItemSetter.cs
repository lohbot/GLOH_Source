using GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

namespace CostumeRoomDlg.MY_CHAR_LIST
{
	public class ListItemSetter
	{
		private MyCharListSetter _myCharListSetter;

		public ListItemSetter(MyCharListSetter charListSetter)
		{
			this._myCharListSetter = charListSetter;
		}

		public void SetMySolList(ref NewListBox mySolKindListBox, ref List<NkSoldierInfo> solList)
		{
			if (solList == null)
			{
				return;
			}
			mySolKindListBox.Clear();
			if (solList.Count == 0)
			{
				return;
			}
			foreach (NkSoldierInfo current in solList)
			{
				if (current != null)
				{
					NewListItem item = new NewListItem(mySolKindListBox.ColumnNum, true, string.Empty);
					this.SetMyCharListBoxItem(ref item, current);
					mySolKindListBox.Add(item);
				}
			}
			mySolKindListBox.RepositionItems();
		}

		public void SetMyCharListBoxItem(ref NewListItem item, NkSoldierInfo solInfo)
		{
			item.Data = solInfo;
			this.SetFrame(ref item, solInfo);
			this.SetPortrait(ref item, solInfo);
			this.SetSelectedUI(ref item, solInfo);
			item.SetListItemData(1, true);
			item.SetListItemData(2, false);
			item.SetListItemData(3, false);
			item.SetListItemData(4, false);
			item.SetListItemData(5, false);
			item.SetListItemData(6, false);
			item.SetListItemData(7, false);
		}

		private void SetFrame(ref NewListItem item, NkSoldierInfo solInfo)
		{
			if (item == null || solInfo == null)
			{
				return;
			}
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(solInfo.GetCharKind(), (int)solInfo.GetGrade());
			if (legendFrame != null)
			{
				item.SetListItemData(0, legendFrame, null, null, null);
			}
			else
			{
				item.SetListItemData(0, "Win_T_ItemEmpty", null, null, null);
			}
		}

		private void SetPortrait(ref NewListItem item, NkSoldierInfo solInfo)
		{
			if (item == null || solInfo == null)
			{
				return;
			}
			Texture2D portraitLeaderSol = this.GetPortraitLeaderSol(solInfo.GetCharKind());
			if (portraitLeaderSol != null)
			{
				item.SetListItemData(1, portraitLeaderSol, solInfo, solInfo.GetLevel(), new EZValueChangedDelegate(this._myCharListSetter._callbackProcessor.OnClickListUpBox), null);
				return;
			}
			NkListSolInfo nkListSolInfo = new NkListSolInfo();
			nkListSolInfo.SolCharKind = solInfo.GetCharKind();
			nkListSolInfo.SolGrade = (int)solInfo.GetGrade();
			nkListSolInfo.SolLevel = solInfo.GetLevel();
			nkListSolInfo.FightPower = solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER);
			nkListSolInfo.SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath((int)solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME));
			if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(solInfo.GetCharKind()) == null)
			{
				return;
			}
			if (50 <= solInfo.GetLevel())
			{
				nkListSolInfo.ShowCombat = true;
				nkListSolInfo.ShowLevel = false;
			}
			else
			{
				nkListSolInfo.ShowCombat = false;
				nkListSolInfo.ShowLevel = true;
			}
			item.SetListItemData(1, nkListSolInfo, solInfo, new EZValueChangedDelegate(this._myCharListSetter._callbackProcessor.OnClickListUpBox), null);
		}

		private void SetSelectedUI(ref NewListItem item, NkSoldierInfo solInfo)
		{
			item.SetListItemData(8, false);
			item.SetListItemData(9, false);
			if (item == null || solInfo == null || this._myCharListSetter._SelectedSolInfo == null)
			{
				return;
			}
			if (solInfo.GetSolID() != this._myCharListSetter._SelectedSolInfo.GetSolID())
			{
				return;
			}
			item.SetListItemData(8, true);
			item.SetListItemData(9, true);
		}

		private Texture2D GetPortraitLeaderSol(int iCharKind)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo != null && kMyCharInfo.UserPortrait)
			{
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser.GetCharKind() == iCharKind)
				{
					return kMyCharInfo.UserPortraitTexture;
				}
			}
			return null;
		}
	}
}
