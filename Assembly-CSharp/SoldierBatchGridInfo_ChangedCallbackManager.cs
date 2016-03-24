using System;
using UnityEngine;
using UnityForms;

public class SoldierBatchGridInfo_ChangedCallbackManager : NrTSingleton<SoldierBatchGridInfo_ChangedCallbackManager>
{
	private SoldierBatchGridInfo_ChangedCallbackManager()
	{
	}

	public void PropertyChanged(object messageSenders, string propertyName, short STARTPOS_INDEX)
	{
		if (messageSenders == null)
		{
			Debug.LogError("ERROR, SoldierBatchGridInfo_ChangedCallbackManager.cs, PropertyChanged(), messageSender is null ");
			return;
		}
		if (messageSenders.GetType() != typeof(SoldierBatchGrid))
		{
			return;
		}
		this.PropertyChangedHandle(propertyName, STARTPOS_INDEX);
	}

	private void PropertyChangedHandle(string propertyName, short STARTPOS_INDEX)
	{
		if (propertyName == "CharacterKind")
		{
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
			{
				InfiCombinationDlg infiCombinationDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INFICOMBINATION_DLG) as InfiCombinationDlg;
				if (infiCombinationDlg == null)
				{
					Debug.Log("NORMAL, SoldierBatchGridInfo_ChangedCallbackManager.cs, PropertyChangedHandle(), InfiCombinationDlg is Null ");
					return;
				}
				infiCombinationDlg.RenewCompleteCombinationDDL(STARTPOS_INDEX);
			}
			else
			{
				PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
				if (plunderSolNumDlg == null)
				{
					Debug.Log("NORMAL, SoldierBatchGridInfo_ChangedCallbackManager.cs, PropertyChangedHandle(), PlunderSolNumDlg is Null ");
					return;
				}
				plunderSolNumDlg.RenewCompleteCombinationDDL(STARTPOS_INDEX);
			}
		}
	}
}
