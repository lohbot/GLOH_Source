using System;
using UnityForms;

public class StateCondition_SOLCOMPOSE_IsExtractSolSetted : EventTriggerItem_StateCondition
{
	public override bool IsVaildValue()
	{
		return true;
	}

	public override bool Verify()
	{
		SolComposeMainDlg solComposeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG) as SolComposeMainDlg;
		return solComposeMainDlg != null && solComposeMainDlg.GetExtractSolCount() > 0;
	}

	public override string GetComment()
	{
		return "영웅 합성/추출 UI 에서 추출 영웅이 셋팅되어 있는가?";
	}
}
