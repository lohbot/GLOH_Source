using System;
using UnityForms;

public class StateCondition_SOLCOMPOSE_IsMaterialSolSetted : EventTriggerItem_StateCondition
{
	public bool check = true;

	public override bool IsVaildValue()
	{
		return true;
	}

	public override bool Verify()
	{
		SolComposeMainDlg_challengequest solComposeMainDlg_challengequest = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG) as SolComposeMainDlg_challengequest;
		return solComposeMainDlg_challengequest != null && !(solComposeMainDlg_challengequest.GetListBox() == null) && ((this.check && 0 < solComposeMainDlg_challengequest.GetListBox().Count) || (!this.check && solComposeMainDlg_challengequest.GetListBox().Count <= 0));
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
		return "영웅 UI 에서 소재 영웅이" + str;
	}
}
