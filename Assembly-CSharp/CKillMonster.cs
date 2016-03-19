using System;

public class CKillMonster : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		if (NrTSingleton<NkQuestManager>.Instance == null)
		{
			return string.Empty;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo((int)base.GetParam());
		if (charKindInfo == null)
		{
			return string.Empty;
		}
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string name = charKindInfo.GetName();
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			name,
			"count1",
			i64ParamVal.ToString(),
			"count2",
			base.GetParamVal().ToString()
		});
		return empty;
	}
}
