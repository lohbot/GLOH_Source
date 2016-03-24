using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityForms;

public class InfiBattleReward : Form
{
	private NewListBox m_ListBoxReward;

	private Button m_ButtonHallofFame;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Plunder/InfiBattle/DLG_InfiRewardInfo", G_ID.INFIBATTLE_REWARD_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_ButtonHallofFame = (base.GetControl("BT_HallofFame") as Button);
		this.m_ButtonHallofFame.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickHallofFame));
		this.m_ListBoxReward = (base.GetControl("NLB_InfiRewardInfo") as NewListBox);
		this.m_ListBoxReward.touchScroll = false;
		base.SetLayerZ(3, -0.14f);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void On_ClickHallofFame(IUIObject a_cObject)
	{
		GS_INFIBATTLE_REWARDINFO_REQ gS_INFIBATTLE_REWARDINFO_REQ = new GS_INFIBATTLE_REWARDINFO_REQ();
		gS_INFIBATTLE_REWARDINFO_REQ.i64PersonID = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().m_PersonID;
		SendPacket.GetInstance().SendObject(2011, gS_INFIBATTLE_REWARDINFO_REQ);
	}

	public void SetRewardInfo(GS_INFIBATTLE_GET_REWARDINFO_ACK ACK)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		this.m_ListBoxReward.Clear();
		for (int i = 0; i < 9; i++)
		{
			NewListItem newListItem = new NewListItem(this.m_ListBoxReward.ColumnNum, true, string.Empty);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.GetRankText(i).ToString());
			if (i == 0)
			{
				text = NrTSingleton<CTextParser>.Instance.GetTextColor("1403") + text;
			}
			else if (i == 1)
			{
				text = NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + text;
			}
			newListItem.SetListItemData(1, text, null, null, null);
			text3 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(ACK.i32RewardUnique[i]);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"itemname",
				text3,
				"count",
				ACK.i16RewardNum[i]
			});
			if (i == 0)
			{
				text2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1403") + text2;
			}
			else if (i == 1)
			{
				text2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + text2;
			}
			newListItem.SetListItemData(2, text2, null, null, null);
			text3 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(ACK.i32WinRewardUnique[i]);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"itemname",
				text3,
				"count",
				ACK.i16WinRewardNum[i]
			});
			if (i == 0)
			{
				text2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1403") + text2;
			}
			else if (i == 1)
			{
				text2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + text2;
			}
			newListItem.SetListItemData(3, text2, null, null, null);
			this.m_ListBoxReward.Add(newListItem);
		}
		this.m_ListBoxReward.RepositionItems();
	}

	public int GetRankText(int iCount)
	{
		int result = 0;
		switch (iCount)
		{
		case 0:
			result = 2501;
			break;
		case 1:
			result = 2502;
			break;
		case 2:
			result = 2503;
			break;
		case 3:
			result = 2504;
			break;
		case 4:
			result = 2475;
			break;
		case 5:
			result = 2505;
			break;
		case 6:
			result = 2476;
			break;
		case 7:
			result = 2477;
			break;
		case 8:
			result = 2506;
			break;
		}
		return result;
	}
}
