using System;

public class CStorageFood : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		return myCharInfo != null && (!NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend() || myCharInfo.m_nActivityPoint >= base.GetParamVal());
	}

	public override string GetConditionText(long i64ParamVal)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return string.Empty;
		}
		string empty = string.Empty;
		if (NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromQuest_Code,
				"count2",
				base.GetParamVal().ToString(),
				"count1",
				myCharInfo.m_nActivityPoint.ToString(),
				"count2",
				base.GetParamVal().ToString()
			});
		}
		return empty;
	}
}
