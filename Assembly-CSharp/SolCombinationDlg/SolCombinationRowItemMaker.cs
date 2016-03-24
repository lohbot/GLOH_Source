using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

namespace SolCombinationDlg
{
	public class SolCombinationRowItemMaker
	{
		public NewListItem CreateSolCombinationItem(NewListBox combinationRowBoxList, Dictionary<int, string> gradeTextureKeyDic, List<int> charOwnSoldierKindList, SolCombinationInfo_Data combinationData)
		{
			NewListItem newListItem = new NewListItem(combinationRowBoxList.ColumnNum, true, string.Empty);
			this.MakeCombinationSolPortrait(charOwnSoldierKindList, newListItem, combinationData);
			if (gradeTextureKeyDic.ContainsKey(combinationData.m_nCombinationGrade))
			{
				newListItem.SetListItemData(16, gradeTextureKeyDic[combinationData.m_nCombinationGrade], null, null, null);
			}
			newListItem.SetListItemData(17, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(combinationData.m_textTitleKey), null, null, null);
			newListItem.SetListItemData(18, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(combinationData.m_textDetailKey), null, null, null);
			newListItem.SetListItemData(19, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2706"), combinationData, new EZValueChangedDelegate(this.ButtonClick_CompleteCombinationBatch), null);
			newListItem.SetListItemData(19, this.IsButtonVisible(ref charOwnSoldierKindList, ref combinationData));
			return newListItem;
		}

		private bool IsButtonVisible(ref List<int> charOwnSoldierKindList, ref SolCombinationInfo_Data combinationData)
		{
			return Scene.CurScene == Scene.Type.SOLDIER_BATCH && NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.IsCompleteCombination(charOwnSoldierKindList, combinationData) && (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON);
		}

		public void ButtonClick_CompleteCombinationBatch(IUIObject obj)
		{
			SolCombinationInfo_Data solCombinationInfo_Data = (SolCombinationInfo_Data)obj.Data;
			if (solCombinationInfo_Data == null)
			{
				Debug.LogError("ERROR, BatchButtonMaker.cs, ButtonClick_CompleteCombinationBatch(), combinationData is Null");
				return;
			}
			int combinationSolCount = solCombinationInfo_Data.GetCombinationSolCount();
			int maxBatchCount = SoldierBatch_AutoBatchTool.GetMaxBatchCount();
			if (maxBatchCount < combinationSolCount)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("416"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("287");
			string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("288");
			msgBoxUI.SetMsg(new YesDelegate(this.YesClick_CompleteCombination), solCombinationInfo_Data, textFromMessageBox, textFromMessageBox2, eMsgType.MB_OK_CANCEL, 2);
		}

		public void YesClick_CompleteCombination(object obj)
		{
			SolCombinationInfo_Data solCombinationInfo_Data = (SolCombinationInfo_Data)obj;
			if (solCombinationInfo_Data == null)
			{
				return;
			}
			Queue<long> queue = new Queue<long>();
			List<string> solCombinationCharCodeList = solCombinationInfo_Data.GetSolCombinationCharCodeList();
			foreach (string current in solCombinationCharCodeList)
			{
				int charKindByCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(current);
				long num = 0L;
				if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
				{
					num = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetBestPowerSoldierID_InMineBattlePossibleSol(charKindByCode);
				}
				else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION)
				{
					List<NkSoldierInfo> solList = SoldierBatch_SolList.GetSolList(eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION);
					if (solList != null)
					{
						foreach (NkSoldierInfo current2 in solList)
						{
							if (current2.GetCharKind() == charKindByCode)
							{
								num = current2.GetSolID();
								break;
							}
						}
					}
				}
				else
				{
					num = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetBestPowerSoldierID_InBattleReadyAndReadySol(charKindByCode);
				}
				if (num != 0L)
				{
					queue.Enqueue(num);
				}
			}
			SoldierBatch_AutoBatchTool.BatchSoldiers(queue);
		}

		private void MakeCombinationSolPortrait(List<int> charOwnSoldierKindList, NewListItem item, SolCombinationInfo_Data combinationInfoData)
		{
			int num = 3;
			for (int i = 0; i < 5; i++)
			{
				int num2 = i * num + 1;
				int index = num2 + 1;
				NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(combinationInfoData.m_szCombinationIsCharCode[i]);
				if (charKindInfoFromCode == null)
				{
					item.SetListItemData(index, false);
					item.SetListItemData(num2, false);
				}
				else
				{
					bool flag = charOwnSoldierKindList != null && charOwnSoldierKindList.Contains(charKindInfoFromCode.GetCharKind());
					item.SetListItemData(index, !flag);
					item.SetListItemData(num2, this.GetSolInfoByKind(charKindInfoFromCode), charKindInfoFromCode, new EZValueChangedDelegate(this.OnSelectSolPortrait), null);
				}
			}
		}

		private NkListSolInfo GetSolInfoByKind(NrCharKindInfo charKindInfo)
		{
			NkListSolInfo nkListSolInfo = new NkListSolInfo();
			nkListSolInfo.SolCharKind = charKindInfo.GetCharKind();
			nkListSolInfo.SolGrade = NrTSingleton<NrTableSolGuideManager>.Instance.GetCharKindGrade(nkListSolInfo.SolCharKind) - 1;
			nkListSolInfo.ShowLevel = false;
			nkListSolInfo.ShowCombat = false;
			nkListSolInfo.ShowGrade = true;
			return nkListSolInfo;
		}

		private void OnSelectSolPortrait(IUIObject obj)
		{
			if (obj == null || obj.Data == null)
			{
				return;
			}
			NrCharKindInfo nrCharKindInfo = (NrCharKindInfo)obj.Data;
			if (nrCharKindInfo == null)
			{
				return;
			}
			SOL_GUIDE solGuild = NrTSingleton<NrTableSolGuideManager>.Instance.GetSolGuild(nrCharKindInfo.GetCharKind());
			if (solGuild == null)
			{
				return;
			}
			SolSlotData solSlotData = new SolSlotData(nrCharKindInfo.GetName(), solGuild.m_i32CharKind, (byte)solGuild.m_iSolGrade, solGuild.m_bFlagSet, solGuild.m_bFlagSetCount - 1, solGuild.m_bSeason, solGuild.m_i32SkillUnique, solGuild.m_i32SkillText);
			if (solSlotData == null || solSlotData.i32KindInfo == 0)
			{
				return;
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLDETAIL_DLG))
			{
				return;
			}
			SolDetail_Info_Dlg solDetail_Info_Dlg = (SolDetail_Info_Dlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAIL_DLG);
			if (!solDetail_Info_Dlg.Visible)
			{
				solDetail_Info_Dlg.Show();
			}
			solDetail_Info_Dlg.SetSolKind(solSlotData);
		}
	}
}
