using System;
using UnityForms;

public class StateCondition_CheckUIOpen : EventTriggerItem_StateCondition
{
	public string _dlgName = string.Empty;

	public override bool IsVaildValue()
	{
		if (string.IsNullOrEmpty(this._dlgName))
		{
			return false;
		}
		try
		{
			Enum.Parse(typeof(G_ID), this._dlgName);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public override bool Verify()
	{
		if (string.IsNullOrEmpty(this._dlgName))
		{
			return false;
		}
		G_ID windowID = (G_ID)((int)Enum.Parse(typeof(G_ID), this._dlgName));
		return NrTSingleton<FormsManager>.Instance.IsShow(windowID);
	}

	public override string GetComment()
	{
		return this._dlgName + " DLG 가 열려있다면";
	}
}
