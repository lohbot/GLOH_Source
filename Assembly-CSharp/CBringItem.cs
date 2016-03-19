using System;
using UnityForms;

public class CBringItem : CQuestCondition
{
	private int m_nItemUnique;

	private int m_i32ItemNum;

	public override bool IsServerCheck()
	{
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_nItemUnique);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo((int)base.GetParam());
		if (charKindInfo == null)
		{
			return string.Empty;
		}
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname1",
			itemNameByItemUnique,
			"targetname2",
			charKindInfo.GetName()
		});
		return empty;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		int itemCnt = NkUserInventory.GetInstance().GetItemCnt(this.m_nItemUnique);
		return this.m_i32ItemNum <= itemCnt;
	}

	public override void AfterAutoPath()
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NPCTALK_DLG))
		{
			return;
		}
		QuestAutoPathInfo autoPathInfo = NrTSingleton<NkQuestManager>.Instance.GetAutoPathInfo();
		if (autoPathInfo != null)
		{
			NrCharNPC nrCharNPC = (NrCharNPC)NrTSingleton<NkCharManager>.Instance.GetCharByCharKind(autoPathInfo.m_nCharKind);
			if (nrCharNPC != null && autoPathInfo.m_nCharKind > 0 && nrCharNPC != null)
			{
				NrCharKindInfo charKindInfo = nrCharNPC.GetCharKindInfo();
				if (charKindInfo != null)
				{
					NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NPCTALK_DLG);
					npcTalkUI_DLG.SetNpcID(charKindInfo.GetCharKind(), nrCharNPC.GetCharUnique());
					npcTalkUI_DLG.Show();
				}
			}
		}
	}

	public void SetItemInfo(int nItemUnique, int i32ItemNum)
	{
		this.m_nItemUnique = nItemUnique;
		this.m_i32ItemNum = i32ItemNum;
	}
}
