using CostumeRoomDlg.MY_CHAR_LIST;
using System;
using System.Collections.Generic;
using UnityForms;

namespace CostumeRoomDlg
{
	public class MyCharListSetter
	{
		private CostumeRoom_Dlg _owner;

		private NkSoldierInfo _selectedSolInfo;

		public CallbackProcessor _callbackProcessor;

		private SortProcessor _sortProcessor;

		private ListItemSetter _listItemSetter;

		private ListItemMover _listItemMover;

		public NkSoldierInfo _SelectedSolInfo
		{
			get
			{
				return this._selectedSolInfo;
			}
			set
			{
				this._selectedSolInfo = value;
			}
		}

		public MyCharListSetter(CostumeRoom_Dlg owner)
		{
			this._owner = owner;
			this._selectedSolInfo = null;
			this._callbackProcessor = new CallbackProcessor(owner, this);
			this._sortProcessor = new SortProcessor();
			this._listItemSetter = new ListItemSetter(this);
			this._listItemMover = new ListItemMover(owner);
		}

		public void InitMyCharList(ref NewListBox mySolKindListBox, List<int> costumeKindList, NkSoldierInfo initSelectedSolInfo)
		{
			List<NkSoldierInfo> allSoldierInfoListByKind = this.GetAllSoldierInfoListByKind(costumeKindList);
			this._sortProcessor.SortMySolListByPower(ref allSoldierInfoListByKind);
			this.SetFirstCharSelected(allSoldierInfoListByKind);
			this.SetWantedSolSelected(allSoldierInfoListByKind, ref initSelectedSolInfo);
			this._listItemSetter.SetMySolList(ref mySolKindListBox, ref allSoldierInfoListByKind);
			this._listItemMover.MoveListToTarget(this._selectedSolInfo);
		}

		public void RefreshMyCharList(ref NewListBox mySolKindListBox)
		{
			for (int i = 0; i < mySolKindListBox.Count; i++)
			{
				UIListItemContainer item = mySolKindListBox.GetItem(i);
				if (!(item == null))
				{
					NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)item.data;
					if (nkSoldierInfo != null)
					{
						NewListItem item2 = new NewListItem(mySolKindListBox.ColumnNum, true, string.Empty);
						this._listItemSetter.SetMyCharListBoxItem(ref item2, nkSoldierInfo);
						mySolKindListBox.UpdateContents(i, item2);
					}
				}
			}
			mySolKindListBox.RepositionItems();
		}

		private List<NkSoldierInfo> GetAllSoldierInfoListByKind(List<int> costumeKindList)
		{
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null || costumeKindList == null)
			{
				return null;
			}
			List<NkSoldierInfo> list = new List<NkSoldierInfo>();
			List<NkSoldierInfo> allSolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetAllSolList();
			if (allSolList == null)
			{
				return null;
			}
			foreach (NkSoldierInfo current in allSolList)
			{
				if (current != null)
				{
					if (costumeKindList.Contains(current.GetCharKind()))
					{
						list.Add(current);
					}
				}
			}
			return list;
		}

		private void SetFirstCharSelected(List<NkSoldierInfo> allSoldierList)
		{
			if (allSoldierList == null || allSoldierList.Count == 0)
			{
				return;
			}
			foreach (NkSoldierInfo current in allSoldierList)
			{
				if (current != null)
				{
					if (current.GetCharKind() == this._owner._settingCostumeKind)
					{
						this._selectedSolInfo = current;
						break;
					}
				}
			}
		}

		private void SetWantedSolSelected(List<NkSoldierInfo> allSoldierList, ref NkSoldierInfo initSelectedSolInfo)
		{
			if (initSelectedSolInfo == null)
			{
				return;
			}
			if (allSoldierList == null || allSoldierList.Count == 0)
			{
				return;
			}
			foreach (NkSoldierInfo current in allSoldierList)
			{
				if (current != null)
				{
					if (current.GetCharKind() == this._owner._settingCostumeKind)
					{
						if (current.GetSolID() == initSelectedSolInfo.GetSolID())
						{
							this._selectedSolInfo = current;
							break;
						}
					}
				}
			}
		}
	}
}
