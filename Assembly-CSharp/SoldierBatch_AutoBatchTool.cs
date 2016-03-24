using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityForms;

public class SoldierBatch_AutoBatchTool
{
	public static void AutoBatch()
	{
		clTempBattlePos[] autoBatchPos = SoldierBatch_SolList.GetAutoBatchPos(SoldierBatch_AutoBatchTool.GetMaxBatchCount(), SoldierBatch.SOLDIER_BATCH_MODE, null);
		if (autoBatchPos == null)
		{
			return;
		}
		Queue<long> queue = new Queue<long>();
		for (int i = 0; i < autoBatchPos.Length; i++)
		{
			queue.Enqueue(autoBatchPos[i].m_nSolID);
		}
		SoldierBatch_AutoBatchTool.BatchSoldiers(queue);
	}

	public static void BatchSoldiers(Queue<long> soldierIDQueue)
	{
		SoldierBatch_AutoBatchTool.ResetGridPosition();
		NmMainFrameWork.GetMainFrameWork().StartCoroutine(SoldierBatch_AutoBatchTool.SyncBatchSolCombinationInEmptyGrid(soldierIDQueue));
	}

	private static void ResetGridPosition()
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON)
		{
			SoldierBatch.SOLDIERBATCH.ResetAllGrid();
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			SolCombination_Dlg solCombination_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMBINATION_DLG) as SolCombination_Dlg;
			if (solCombination_Dlg != null)
			{
				SoldierBatch.SOLDIERBATCH.ResetSelectStartPosGrid(solCombination_Dlg._selectedInfiBattleIndex);
			}
		}
		else
		{
			SoldierBatch.SOLDIERBATCH.ResetSolPosition();
		}
	}

	public static int GetMaxBatchCount()
	{
		int result = 0;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			result = SoldierBatch.SOLDIERBATCH.GetBeAbleToBatchSoldier_InBabel();
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID)
		{
			result = SoldierBatch.SOLDIERBATCH.GetBeAbleToBatchSoldier_InMythRaid();
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			result = 9;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			result = 5;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION)
		{
			result = 5;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			result = 5;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON)
		{
			result = 6;
		}
		return result;
	}

	[DebuggerHidden]
	private static IEnumerator SyncBatchSolCombinationInEmptyGrid(object param)
	{
		SoldierBatch_AutoBatchTool.<SyncBatchSolCombinationInEmptyGrid>c__Iterator8 <SyncBatchSolCombinationInEmptyGrid>c__Iterator = new SoldierBatch_AutoBatchTool.<SyncBatchSolCombinationInEmptyGrid>c__Iterator8();
		<SyncBatchSolCombinationInEmptyGrid>c__Iterator.param = param;
		<SyncBatchSolCombinationInEmptyGrid>c__Iterator.<$>param = param;
		return <SyncBatchSolCombinationInEmptyGrid>c__Iterator;
	}

	private static void InsertEmptyGridAyMyChar(Queue<long> soldierIDQueue)
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			SoldierBatch.SOLDIERBATCH.InsertBabelEmptyGrid_At_MyChar(soldierIDQueue);
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID)
		{
			SoldierBatch.SOLDIERBATCH.InsertMythRaidEmptyGrid_At_MyChar(soldierIDQueue);
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON)
		{
			SoldierBatch.SOLDIERBATCH.InsertGuildBossEmptyGrid_At_MyChar(soldierIDQueue);
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			SolCombination_Dlg solCombination_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMBINATION_DLG) as SolCombination_Dlg;
			if (solCombination_Dlg != null)
			{
				SoldierBatch.SOLDIERBATCH.InsertInfiBattleEmptyGrid_At_MyChar(soldierIDQueue, solCombination_Dlg._selectedInfiBattleIndex);
			}
		}
	}
}
