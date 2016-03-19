using System;
using UnityEngine;

public class CLevelMake : CQuestCondition
{
	public int m_i32bLearn;

	public override bool CheckCondition(long i64Param, ref long i64PramVal)
	{
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string empty = string.Empty;
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			Debug.LogWarning("QUESTCODE_LEVELMAKE null == kMyChar");
			return string.Empty;
		}
		return empty;
	}
}
