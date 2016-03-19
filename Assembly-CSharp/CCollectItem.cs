using System;

public class CCollectItem : CQuestCondition
{
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
		if (NrTSingleton<NkQuestManager>.Instance == null)
		{
			return string.Empty;
		}
		NkUserInventory instance = NkUserInventory.GetInstance();
		if (instance == null)
		{
			return string.Empty;
		}
		long num = (long)instance.GetItemCnt((int)base.GetParam());
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
		string text = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
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
