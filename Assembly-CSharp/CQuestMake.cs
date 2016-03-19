using System;

public class CQuestMake : CQuestCondition
{
	public override bool IsServerCheck()
	{
		return false;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NkUserInventory instance = NkUserInventory.GetInstance();
		if (instance == null)
		{
			return false;
		}
		long num = (long)instance.GetItemCnt((int)base.GetParam());
		i64ParamVal = num;
		return num >= base.GetParamVal();
	}

	public override string GetConditionText(long i64ParamVal)
	{
		NkUserInventory instance = NkUserInventory.GetInstance();
		if (instance == null)
		{
			return string.Empty;
		}
		long num = (long)instance.GetItemCnt((int)base.GetParam());
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			itemNameByItemUnique,
			"count1",
			num.ToString(),
			"count2",
			base.GetParamVal().ToString()
		});
		return empty;
	}
}
