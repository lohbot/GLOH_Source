using System;
using UnityForms;

namespace CostumeRoomDlg.MY_CHAR_LIST
{
	public class ListItemMover
	{
		private CostumeRoom_Dlg _costumeRoomDlg;

		public ListItemMover(CostumeRoom_Dlg costumeRoomDlg)
		{
			this._costumeRoomDlg = costumeRoomDlg;
		}

		public void MoveListToTarget(NkSoldierInfo selectedMySolInfo)
		{
			if (selectedMySolInfo == null)
			{
				return;
			}
			NewListBox mySolKindListBox = this._costumeRoomDlg._variables._mySolKindListBox;
			if (mySolKindListBox == null)
			{
				return;
			}
			float myCharMoveTargetPos = this.GetMyCharMoveTargetPos(mySolKindListBox, selectedMySolInfo);
			if (myCharMoveTargetPos < 0f)
			{
				return;
			}
			mySolKindListBox.ScrollListTo_Internal(myCharMoveTargetPos);
		}

		private float GetMyCharMoveTargetPos(NewListBox costumeListBox, NkSoldierInfo selectedMySolInfo)
		{
			UIListItemContainer moveTargetItem_By_MySolID = this.GetMoveTargetItem_By_MySolID(costumeListBox, selectedMySolInfo);
			if (moveTargetItem_By_MySolID == null)
			{
				return -1f;
			}
			return this.GetNewListBoxTargetPos(costumeListBox.Count, moveTargetItem_By_MySolID);
		}

		private UIListItemContainer GetMoveTargetItem_By_MySolID(NewListBox costumeListBox, NkSoldierInfo selectedMySolInfo)
		{
			if (costumeListBox == null)
			{
				return null;
			}
			if (selectedMySolInfo == null)
			{
				return null;
			}
			for (int i = 0; i < costumeListBox.Count; i++)
			{
				UIListItemContainer item = costumeListBox.GetItem(i);
				if (!(item == null))
				{
					if (item.Data != null)
					{
						NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)item.Data;
						if (nkSoldierInfo != null)
						{
							if (nkSoldierInfo.GetSolID() == selectedMySolInfo.GetSolID())
							{
								return item;
							}
						}
					}
				}
			}
			return null;
		}

		private float GetNewListBoxTargetPos(int maxCount, UIListItemContainer moveTargetItem)
		{
			if (moveTargetItem == null)
			{
				return 0f;
			}
			int index = moveTargetItem.index;
			return (float)index / (float)maxCount;
		}
	}
}
