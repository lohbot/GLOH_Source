using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityForms;

public class NewGuildWarRewardInfoDlg : Form
{
	private List<GUILDWAR_REWARD_GUILD_DATA> m_GuildDataList = new List<GUILDWAR_REWARD_GUILD_DATA>();

	private List<GUILDWAR_REWARD_DATA> m_RewardList = new List<GUILDWAR_REWARD_DATA>();

	private DrawTexture m_dtBaseBG;

	private NewListBox m_nlbBaseReward;

	private NewListBox m_nlbAddReward;

	private Label m_lbGuildPointReward;

	private Label m_lbGuildExpReward;

	private Label m_lbGuildFundReward;

	private Label m_lbMyGuildRank;

	private Button m_btRewardGet;

	private NewListBox m_nlbGuildRank;

	private string m_strText = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_GuildWarRewardInfo", G_ID.GUILDWAR_REWARDINFO_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtBaseBG = (base.GetControl("DrawTexture_BGIMG") as DrawTexture);
		this.m_strText = string.Format("UI/ETC/Plunder", new object[0]);
		this.m_dtBaseBG.SetTextureFromBundle(this.m_strText);
		this.m_nlbBaseReward = (base.GetControl("nlb_BaseReward") as NewListBox);
		this.m_nlbAddReward = (base.GetControl("nlb_AddReward") as NewListBox);
		this.m_lbGuildPointReward = (base.GetControl("LB_GuildReward1") as Label);
		this.m_lbGuildExpReward = (base.GetControl("LB_GuildReward2") as Label);
		this.m_lbGuildFundReward = (base.GetControl("LB_GuildReward3") as Label);
		this.m_lbMyGuildRank = (base.GetControl("LB_LastweekRank2") as Label);
		this.m_btRewardGet = (base.GetControl("BT_RewardGet") as Button);
		this.m_btRewardGet.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRewardGet));
		this.m_nlbGuildRank = (base.GetControl("nlb_rank") as NewListBox);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClickRewardGet(IUIObject obj)
	{
	}

	public void ClearGuildData()
	{
		this.m_GuildDataList.Clear();
	}

	public void AddGuildData(GUILDWAR_REWARD_GUILD_DATA GuildData)
	{
		this.m_GuildDataList.Add(GuildData);
	}

	public void ClearRewardList()
	{
		this.m_GuildDataList.Clear();
	}

	public void AddRewardData(GUILDWAR_REWARD_DATA Data)
	{
		this.m_RewardList.Add(Data);
	}

	public void RefreshRewardInfo()
	{
		this.m_nlbBaseReward.Clear();
		this.m_nlbAddReward.Clear();
		this.m_nlbGuildRank.Clear();
		foreach (GUILDWAR_REWARD_DATA current in this.m_RewardList)
		{
			this.MakeBaseRewarItem(current);
			this.MakeAddRewarItem(current);
		}
		this.m_nlbBaseReward.RepositionItems();
		this.m_nlbAddReward.RepositionItems();
		foreach (GUILDWAR_REWARD_GUILD_DATA current2 in this.m_GuildDataList)
		{
			this.MakeGuildRankItem(current2);
		}
		this.m_nlbGuildRank.RepositionItems();
		GUILDWAR_REWARD_DATA rewardDataFromRank = this.GetRewardDataFromRank(NrTSingleton<NewGuildManager>.Instance.GetRank());
		if (rewardDataFromRank != null)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2004"),
				"count1",
				rewardDataFromRank.i32WinGuildPoint,
				"count2",
				rewardDataFromRank.i32LoseGuildPoint
			});
			this.m_lbGuildPointReward.SetText(this.m_strText);
			this.m_lbGuildExpReward.SetText(this.m_strText);
			this.m_lbGuildFundReward.SetText(this.m_strText);
		}
		this.m_lbMyGuildRank.SetText(NrTSingleton<NewGuildManager>.Instance.GetRank().ToString());
	}

	public void MakeBaseRewarItem(GUILDWAR_REWARD_DATA Data)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbBaseReward.ColumnNum, true);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2651"),
			"count1",
			Data.i16RankMin,
			"count2",
			Data.i16RankMax
		});
		newListItem.SetListItemData(0, this.m_strText, null, null, null);
		newListItem.SetListItemData(1, true);
		newListItem.SetListItemData(2, ANNUALIZED.Convert(Data.i32RewardGold), null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(Data.i32RewardItemUnique),
			"count",
			Data.i32RewardItemNum
		});
		newListItem.SetListItemData(3, this.m_strText, null, null, null);
		this.m_nlbBaseReward.Add(newListItem);
	}

	public void MakeAddRewarItem(GUILDWAR_REWARD_DATA Data)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbAddReward.ColumnNum, true);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2651"),
			"count1",
			Data.i16RankMin,
			"count2",
			Data.i16RankMax
		});
		newListItem.SetListItemData(0, this.m_strText, null, null, null);
		newListItem.SetListItemData(1, true);
		newListItem.SetListItemData(2, ANNUALIZED.Convert(Data.i32AddRewardGold), null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(Data.i32AddRewardItemUnique),
			"count",
			Data.i32AddRewardItemNum
		});
		newListItem.SetListItemData(3, this.m_strText, null, null, null);
		this.m_nlbAddReward.Add(newListItem);
	}

	public void MakeGuildRankItem(GUILDWAR_REWARD_GUILD_DATA GuildData)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbGuildRank.ColumnNum, true);
		newListItem.SetListItemData(0, TKString.NEWString(GuildData.strGuildName), null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2509"),
			"rank",
			GuildData.i16GuildRank
		});
		newListItem.SetListItemData(1, this.m_strText, null, null, null);
		this.m_nlbGuildRank.Add(newListItem);
	}

	public GUILDWAR_REWARD_DATA GetRewardDataFromRank(short iRank)
	{
		foreach (GUILDWAR_REWARD_DATA current in this.m_RewardList)
		{
			if (current.i16RankMin <= iRank && current.i16RankMax >= iRank)
			{
				return current;
			}
		}
		return null;
	}
}
