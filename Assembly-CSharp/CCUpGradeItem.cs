using System;

public class CCUpGradeItem : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return (i64Param == base.GetParam() && i64ParamVal >= base.GetParamVal()) || (base.GetParam() == 0L && i64ParamVal >= base.GetParamVal());
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string text = string.Empty;
		string empty = string.Empty;
		if (base.GetParam() == 0L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count2",
				base.GetParamVal(),
				"count1",
				i64ParamVal
			});
		}
		else if (base.GetParam() > 0L)
		{
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"targetname",
				itemNameByItemUnique,
				"count1",
				base.GetParamVal(),
				"count2",
				i64ParamVal
			});
		}
		return empty;
	}
}
