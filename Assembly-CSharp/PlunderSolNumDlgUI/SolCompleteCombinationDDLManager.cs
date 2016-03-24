using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

namespace PlunderSolNumDlgUI
{
	public class SolCompleteCombinationDDLManager
	{
		private int ROOM_IDX_ERROR = -1;

		private int PREV_BATTLE_COMBINATION_NOT_EXIST = -1;

		private int _selectedListItemIdx = -1;

		private int _selectedSolCombinationCompleteUniqueKey = -1;

		private Dictionary<int, int> _listItemInfo;

		public SolCompleteCombinationDDLManager()
		{
			this._listItemInfo = new Dictionary<int, int>();
		}

		public void RenewDDL(DropDownList solCompleteCombinationDDL, int STARTPOS_INDEX)
		{
			if (!this.IsLeader())
			{
				return;
			}
			List<int> batchSolKindList = SoldierBatch.SOLDIERBATCH.GetBatchSolKindList(STARTPOS_INDEX);
			Dictionary<int, string> completeCombinationDic = NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetCompleteCombinationDic(batchSolKindList);
			this._listItemInfo.Clear();
			this.MakeDDL(solCompleteCombinationDDL, completeCombinationDic, STARTPOS_INDEX);
		}

		public void ChangeIdx(DropDownList solCompleteCombinationDDL, int STARTPOS_INDEX)
		{
			if (!this.IsLeader())
			{
				return;
			}
			this.SetSelectedListItemInfoProcess(solCompleteCombinationDDL, STARTPOS_INDEX);
			Debug.Log(string.Concat(new object[]
			{
				"ChangeIdx(), ListItemIdx : ",
				this._selectedListItemIdx,
				", CombinationUniqueKey : ",
				this._selectedSolCombinationCompleteUniqueKey
			}));
		}

		public bool IsLeader()
		{
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
			{
				return true;
			}
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
			{
				return Battle.isLeader;
			}
			return SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_MYTHRAID || Battle.isLeader;
		}

		private void SetSelectedListItemInfoProcess(DropDownList solCompleteCombinationDDL, int STARTPOS_INDEX)
		{
			if (solCompleteCombinationDDL.SelectedItem == null)
			{
				this.SetSelectedItemInfo(-1, -1, STARTPOS_INDEX);
				return;
			}
			if (solCompleteCombinationDDL.SelectedItem.Data == null)
			{
				this.SetSelectedItemInfo(-1, -1, STARTPOS_INDEX);
				return;
			}
			ListItem listItem = solCompleteCombinationDDL.SelectedItem.Data as ListItem;
			if (listItem == null)
			{
				this.SetSelectedItemInfo(-1, -1, STARTPOS_INDEX);
				return;
			}
			this.SetSelectedItemInfo(solCompleteCombinationDDL.SelectedItem.GetIndex(), (int)listItem.Key, STARTPOS_INDEX);
		}

		private void MakeDDL(DropDownList solCompleteCombinationDDL, Dictionary<int, string> complteCombinationDic, int STARTPOS_INDEX)
		{
			if (complteCombinationDic == null)
			{
				this.SetSelectedItemInfo(-1, -1, STARTPOS_INDEX);
				return;
			}
			solCompleteCombinationDDL.SetViewArea(complteCombinationDic.Count);
			solCompleteCombinationDDL.Clear();
			this.AddCompleteCombinationItem(solCompleteCombinationDDL, complteCombinationDic);
			this.SetListItemStartIndex(solCompleteCombinationDDL, STARTPOS_INDEX);
		}

		private void AddCompleteCombinationItem(DropDownList solCompleteCombinationDDL, Dictionary<int, string> complteCombinationDic)
		{
			int num = 0;
			foreach (KeyValuePair<int, string> current in complteCombinationDic)
			{
				ListItem listItem = new ListItem();
				listItem.Key = current.Key;
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(current.Value);
				listItem.SetColumnStr(0, textFromInterface);
				this._listItemInfo.Add(num++, current.Key);
				solCompleteCombinationDDL.Add(listItem);
			}
			solCompleteCombinationDDL.RepositionItems();
		}

		private void SetListItemStartIndex(DropDownList solCompleteCombinationDDL, int STARTPOS_INDEX)
		{
			if (this.PrevUserSelectSolCombinationSetting(solCompleteCombinationDDL, STARTPOS_INDEX))
			{
				return;
			}
			solCompleteCombinationDDL.SetFirstItem();
			this.SetSelectedListItemInfoProcess(solCompleteCombinationDDL, STARTPOS_INDEX);
		}

		private bool PrevUserSelectSolCombinationSetting(DropDownList solCompleteCombinationDDL, int STARTPOS_INDEX)
		{
			int prevBattleSolCombinationUnique = this.GetPrevBattleSolCombinationUnique(STARTPOS_INDEX);
			if (prevBattleSolCombinationUnique == this.PREV_BATTLE_COMBINATION_NOT_EXIST)
			{
				return false;
			}
			foreach (KeyValuePair<int, int> current in this._listItemInfo)
			{
				if (current.Value == prevBattleSolCombinationUnique)
				{
					solCompleteCombinationDDL.SetIndex(current.Key);
					this.SetSelectedListItemInfoProcess(solCompleteCombinationDDL, STARTPOS_INDEX);
					return true;
				}
			}
			return false;
		}

		private void SetSelectedItemInfo(int itemIdx, int solCombinationUniqueKey, int STARTPOS_INDEX)
		{
			this.SyncSolCombinationToParty(solCombinationUniqueKey);
			NrTSingleton<SolCombination_BatchSelectInfoManager>.Instance.SetUserSelectedUniqeKey(solCombinationUniqueKey, STARTPOS_INDEX);
			this._selectedListItemIdx = itemIdx;
			this._selectedSolCombinationCompleteUniqueKey = solCombinationUniqueKey;
		}

		private void SyncSolCombinationToParty(int newSolCombinationKey)
		{
			if (this._selectedSolCombinationCompleteUniqueKey == newSolCombinationKey)
			{
				return;
			}
			this.RequestSyncSelectedSolCombinationInfoToParty(newSolCombinationKey);
		}

		private void RequestSyncSelectedSolCombinationInfoToParty(int selectedSolCombinationUniqueKey)
		{
			Debug.Log("SyncSelectedSolCombinationInfoToParty");
			if (!this.IsPartyBatchMode())
			{
				return;
			}
			int batchModeRoomID = this.GetBatchModeRoomID();
			if (batchModeRoomID == this.ROOM_IDX_ERROR)
			{
				Debug.LogError("ERROR, SolCompleteCombinationDDLManager.cs, RequestSyncSelectedSolCombinationInfoToParty(). battleRoomID Error");
				return;
			}
			GS_SOLCOMBINATION_SYNC_REQ gS_SOLCOMBINATION_SYNC_REQ = default(GS_SOLCOMBINATION_SYNC_REQ);
			gS_SOLCOMBINATION_SYNC_REQ.i32RoomID = batchModeRoomID;
			gS_SOLCOMBINATION_SYNC_REQ.i32SolCombinationUnique = selectedSolCombinationUniqueKey;
			gS_SOLCOMBINATION_SYNC_REQ.i32BatchModeType = (int)SoldierBatch.SOLDIER_BATCH_MODE;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLCOMBINATION_SYNC_REQ, gS_SOLCOMBINATION_SYNC_REQ);
		}

		private int GetBatchModeRoomID()
		{
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
			{
				return SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
			}
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID)
			{
				return SoldierBatch.MYTHRAID_INFO.m_nMythRaidRoomIndex;
			}
			return this.ROOM_IDX_ERROR;
		}

		private bool IsPartyBatchMode()
		{
			return SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID;
		}

		private int GetPrevBattleSolCombinationUnique(int STARTPOS_INDEX)
		{
			if (SoldierBatch.SOLDIERBATCH == null || SoldierBatch.BABELTOWER_INFO == null)
			{
				return this.PREV_BATTLE_COMBINATION_NOT_EXIST;
			}
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER && SoldierBatch.BABELTOWER_INFO.m_nBountyHuntUnique <= 0 && 0 < SoldierBatch.BABELTOWER_INFO.m_nBabelFloor)
			{
				return NrTSingleton<SolCombination_RecentBattleStartInfoManager>.Instance.GetRecentBattleSolCombinationInfo(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER, 0);
			}
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
			{
				return NrTSingleton<SolCombination_RecentBattleStartInfoManager>.Instance.GetRecentBattleSolCombinationInfo(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS, 0);
			}
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID)
			{
				return NrTSingleton<SolCombination_RecentBattleStartInfoManager>.Instance.GetRecentBattleSolCombinationInfo(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID, 0);
			}
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER && 0 < SoldierBatch.BABELTOWER_INFO.m_nBountyHuntUnique)
			{
				return NrTSingleton<SolCombination_RecentBattleStartInfoManager>.Instance.GetRecentBattleSolCombinationInfo(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT, 0);
			}
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
			{
				return NrTSingleton<SolCombination_RecentBattleStartInfoManager>.Instance.GetRecentBattleSolCombinationInfo(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY, STARTPOS_INDEX);
			}
			return this.PREV_BATTLE_COMBINATION_NOT_EXIST;
		}
	}
}
