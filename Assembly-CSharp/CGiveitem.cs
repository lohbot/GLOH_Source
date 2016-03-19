using System;

public class CGiveitem : CQuestCondition
{
	private int m_i32ResultNpcID;

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
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_i32ResultNpcID);
		string empty = string.Empty;
		if (base.GetParam() > 0L)
		{
			NkUserInventory instance = NkUserInventory.GetInstance();
			if (instance == null)
			{
				return string.Empty;
			}
			long num = (long)instance.GetItemCnt((int)base.GetParam());
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
			string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromQuest_Code,
				"targetname1",
				itemNameByItemUnique,
				"targetname2",
				charKindInfo.GetName(),
				"count1",
				num,
				"count2",
				base.GetParamVal()
			});
		}
		else if (base.GetParam() == 0L)
		{
			string textFromQuest_Code2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromQuest_Code2,
				"targetname",
				charKindInfo.GetName(),
				"count1",
				i64ParamVal,
				"count2",
				base.GetParamVal()
			});
		}
		return empty;
	}

	public void SetResultNpcID(int i32NpcID)
	{
		this.m_i32ResultNpcID = i32NpcID;
	}
}
