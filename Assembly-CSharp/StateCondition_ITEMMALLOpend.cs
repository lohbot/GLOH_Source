using System;
using UnityForms;

public class StateCondition_ITEMMALLOpend : EventTriggerItem_StateCondition
{
	public int _mainTabType;

	public override bool IsVaildValue()
	{
		return true;
	}

	public override bool Verify()
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMMALL_CHALLENGEQUEST_DLG))
		{
			return false;
		}
		ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_CHALLENGEQUEST_DLG) as ItemMallDlg;
		return itemMallDlg != null && itemMallDlg.MODE == (ItemMallDlg.eMODE)this._mainTabType;
	}

	public override string GetComment()
	{
		string str = string.Empty;
		if (this._mainTabType == 0)
		{
			str = "일반";
		}
		else if (this._mainTabType == 1)
		{
			str = "프리미엄 이용권";
		}
		else if (this._mainTabType == 2)
		{
			str = "프리미엄 고용";
		}
		return "ITEMMALL DLG 의" + str + " 탭이 열려있다면";
	}
}
