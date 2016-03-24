using GAME;
using System;
using UnityForms;

namespace CostumeRoomDlg.COSTUME_LIST
{
	public class ListItemMover
	{
		private CostumeRoom_Dlg _costumeRoomDlg;

		public ListItemMover(CostumeRoom_Dlg costumeRoomDlg)
		{
			this._costumeRoomDlg = costumeRoomDlg;
		}

		public void CostumeListScrollToTarget(NkSoldierInfo solInfo)
		{
			if (solInfo == null)
			{
				return;
			}
			NewListBox costumeListBox = this._costumeRoomDlg._variables._costumeListBox;
			if (costumeListBox == null)
			{
				return;
			}
			float costumeMoveTargetPos = this.GetCostumeMoveTargetPos(costumeListBox, solInfo);
			if (costumeMoveTargetPos < 0f)
			{
				return;
			}
			costumeListBox.ScrollListTo_Internal(costumeMoveTargetPos);
		}

		private float GetCostumeMoveTargetPos(NewListBox costumeListBox, NkSoldierInfo solInfo)
		{
			if (solInfo == null)
			{
				return -1f;
			}
			if (!solInfo.IsCostumeEquip())
			{
				return 0f;
			}
			UIListItemContainer moveTargetItem_By_SolCostume = this.GetMoveTargetItem_By_SolCostume(costumeListBox, solInfo);
			if (moveTargetItem_By_SolCostume == null)
			{
				return -1f;
			}
			return this.GetNewListBoxTargetPos(costumeListBox.Count, moveTargetItem_By_SolCostume);
		}

		private UIListItemContainer GetMoveTargetItem_By_SolCostume(NewListBox costumeListBox, NkSoldierInfo solInfo)
		{
			if (costumeListBox == null)
			{
				return null;
			}
			if (solInfo == null)
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
						CharCostumeInfo_Data charCostumeInfo_Data = (CharCostumeInfo_Data)item.Data;
						if (charCostumeInfo_Data != null)
						{
							if (charCostumeInfo_Data.m_costumeUnique == (int)solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME))
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
