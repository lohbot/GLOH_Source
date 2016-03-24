using System;
using UnityForms;

public class StateCondition_SOLCOMPOSE_IsBaseSolSetted : EventTriggerItem_StateCondition
{
	public bool check = true;

	public override bool IsVaildValue()
	{
		return true;
	}

	public override bool Verify()
	{
		return this.IsSolSettedCheck();
	}

	public override string GetComment()
	{
		string str = string.Empty;
		if (this.check)
		{
			str = " 셋팅되어 있으면 통과";
		}
		else
		{
			str = " 셋팅되어 있지 않으면 통과";
		}
		return "UI 에서 베이스 영웅이" + str;
	}

	private bool IsSolSettedCheck()
	{
		SolComposeMainDlg_challengequest solComposeMainDlg_challengequest = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG) as SolComposeMainDlg_challengequest;
		if (solComposeMainDlg_challengequest == null)
		{
			return false;
		}
		if (this.check)
		{
			if (solComposeMainDlg_challengequest._dummySolBase == null)
			{
				return false;
			}
			if (0L < solComposeMainDlg_challengequest._dummySolBase.GetSolID())
			{
				return true;
			}
		}
		else if (!this.check && (solComposeMainDlg_challengequest._dummySolBase == null || solComposeMainDlg_challengequest._dummySolBase.GetSolID() <= 0L))
		{
			return true;
		}
		return false;
	}
}
