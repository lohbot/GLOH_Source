using System;

public class CSignupItem : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return (i64Param == base.GetParam() && i64ParamVal >= base.GetParamVal()) || (base.GetParam() == 0L && i64ParamVal >= base.GetParamVal());
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string empty = string.Empty;
		string text = string.Empty;
		if (0L < base.GetParam())
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"targetname",
				itemNameByItemUnique,
				"count1",
				i64ParamVal.ToString(),
				"count2",
				base.GetParamVal().ToString()
			});
		}
		else if (base.GetParam() == 0L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count2",
				base.GetParamVal().ToString(),
				"count1",
				i64ParamVal.ToString()
			});
		}
		return empty;
	}
}
