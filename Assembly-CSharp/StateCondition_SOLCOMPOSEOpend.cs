using System;
using UnityForms;

public class StateCondition_SOLCOMPOSEOpend : EventTriggerItem_StateCondition
{
	public int _mainTabType;

	public int _challengeUnique = -1;

	public override bool IsVaildValue()
	{
		return true;
	}

	public override bool Verify()
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG))
		{
			return false;
		}
		SolComposeMainDlg_challengequest solComposeMainDlg_challengequest = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG) as SolComposeMainDlg_challengequest;
		return solComposeMainDlg_challengequest != null && (0 >= this._challengeUnique || solComposeMainDlg_challengequest._ChallengeQuestUnique == this._challengeUnique) && solComposeMainDlg_challengequest.GetSolComposeType() == (SOLCOMPOSE_TYPE)this._mainTabType;
	}

	public override string GetComment()
	{
		string str = string.Empty;
		if (this._mainTabType == 0)
		{
			str = "합성";
		}
		else if (this._mainTabType == 1)
		{
			str = "판매";
		}
		else if (this._mainTabType == 2)
		{
			str = "정수추출";
		}
		else if (this._mainTabType == 3)
		{
			str = "초월";
		}
		string text = string.Empty;
		if (this._challengeUnique != -1)
		{
			text = text + this._challengeUnique + " 도전과제에 의해서";
		}
		return text + " SOLCOMPOSE_MAIN_DLG 의 " + str + " 탭이 열려있다면";
	}
}
