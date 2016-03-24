using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class NewGuildWarRewardInfoDlg : Form
{
	private List<GUILDWAR_RANK_DATA> m_GuildDataList = new List<GUILDWAR_RANK_DATA>();

	private NewListBox m_nlbBaseReward;

	private NewListBox m_nlbGuildRank;

	private Button m_btBack;

	private Button m_btRewardGet;

	private Box m_box_Noti1;

	private Box m_bxPage;

	private Button m_PagePrev;

	private Button m_PageNext;

	private Button btHelp01;

	private Label lbHelpText01;

	private DrawTexture dtHelpBg01;

	private DrawTexture dtHelpTail01;

	private string m_strText = string.Empty;

	private short m_i16CurPage = -1;

	private short m_i16MaxPage = -1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_GuildWarRewardInfo", G_ID.GUILDWAR_REWARDINFO_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btBack = (base.GetControl("Button_Back") as Button);
		this.m_btBack.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBack));
		this.m_strText = string.Format("UI/ETC/Plunder", new object[0]);
		this.m_nlbBaseReward = (base.GetControl("nlb_BaseReward") as NewListBox);
		this.m_nlbGuildRank = (base.GetControl("nlb_rank") as NewListBox);
		this.m_btRewardGet = (base.GetControl("BT_RewardGet") as Button);
		this.m_btRewardGet.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickedRewardGet));
		this.m_btRewardGet.SetEnabled(false);
		this.m_box_Noti1 = (base.GetControl("Box_Notice1") as Box);
		this.m_box_Noti1.Hide(true);
		this.m_bxPage = (base.GetControl("Box_Page") as Box);
		this.SetPage(1, 1);
		this.m_PagePrev = (base.GetControl("BT_Page01") as Button);
		this.m_PagePrev.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickPrevPage));
		this.m_PageNext = (base.GetControl("BT_Page02") as Button);
		this.m_PageNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickNextPage));
		this.btHelp01 = (base.GetControl("BT_Help01") as Button);
		this.btHelp01.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickedHelp1));
		this.lbHelpText01 = (base.GetControl("LB_HelpText01") as Label);
		this.lbHelpText01.Hide(true);
		this.lbHelpText01.SetLocationZ(this.lbHelpText01.GetLocation().z - 1f);
		this.dtHelpBg01 = (base.GetControl("DT_HelpBG01") as DrawTexture);
		this.dtHelpBg01.Hide(true);
		this.dtHelpBg01.SetLocationZ(this.dtHelpBg01.GetLocation().z - 1f);
		this.dtHelpTail01 = (base.GetControl("DT_HelpTail01") as DrawTexture);
		this.dtHelpTail01.Hide(true);
		this.dtHelpTail01.SetLocationZ(this.dtHelpTail01.GetLocation().z - 1f);
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_RANKINFO_REQ(-1);
		this.SetRewardList();
	}

	public void ClearGuildData()
	{
		this.m_GuildDataList.Clear();
	}

	public void AddGuildData(GUILDWAR_RANK_DATA GuildData)
	{
		this.m_GuildDataList.Add(GuildData);
	}

	public void SetRewardList()
	{
		this.m_nlbGuildRank.Clear();
		foreach (GUILDWAR_REWARD_DATA current in NrTSingleton<GuildWarManager>.Instance.m_GuildWarRewardList)
		{
			this.MakeBaseRewarItem(current);
		}
		this.m_nlbBaseReward.RepositionItems();
	}

	public void MakeBaseRewarItem(GUILDWAR_REWARD_DATA Data)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbBaseReward.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(0, ANNUALIZED.Convert(Data.Win_Guild_Point), null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(Data.Win_Reward_ItemUnique),
			"count",
			ANNUALIZED.Convert(Data.Win_Reward_ItemCount)
		});
		newListItem.SetListItemData(1, this.m_strText, null, null, null);
		newListItem.SetListItemData(2, ANNUALIZED.Convert(Data.Win_Reward_Gold), null, null, null);
		newListItem.SetListItemData(5, Data.Max_Guild_Rank.ToString(), null, null, null);
		newListItem.SetListItemData(6, "~", null, null, null);
		newListItem.SetListItemData(7, Data.Min_Guild_Rank.ToString(), null, null, null);
		newListItem.SetListItemData(8, NrTSingleton<ItemManager>.Instance.GetItemTexture(Data.Win_Reward_ItemUnique), null, null, null);
		this.m_nlbBaseReward.Add(newListItem);
	}

	public void RefreshRankInfo()
	{
		this.m_nlbGuildRank.Clear();
		foreach (GUILDWAR_RANK_DATA current in this.m_GuildDataList)
		{
			this.MakeGuildRankItem(current);
		}
		this.m_nlbGuildRank.RepositionItems();
	}

	public void MakeGuildRankItem(GUILDWAR_RANK_DATA GuildData)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbGuildRank.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(0, TKString.NEWString(GuildData.strGuildName), null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2509"),
			"rank",
			GuildData.i16GuildRank
		});
		newListItem.SetListItemData(1, this.m_strText, null, null, null);
		newListItem.SetListItemData(4, ANNUALIZED.Convert(GuildData.i32GuildWarPoint), null, null, null);
		this.m_nlbGuildRank.Add(newListItem);
	}

	public void OnClickBack(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_INFO_REQ();
	}

	public void OnClickedRewardGet(IUIObject obj)
	{
		SendPacket.GetInstance().SendObject(2210, new GS_GUILDWAR_REWARD_REQ());
	}

	public void OnClickedHelp1(IUIObject obj)
	{
		if (this.dtHelpBg01.IsHidden())
		{
			this.lbHelpText01.Hide(false);
			this.dtHelpTail01.Hide(false);
			this.dtHelpBg01.Hide(false);
		}
		else
		{
			this.lbHelpText01.Hide(true);
			this.dtHelpTail01.Hide(true);
			this.dtHelpBg01.Hide(true);
		}
	}

	public void OnClickPrevPage(IUIObject obj)
	{
		if (this.m_i16CurPage > 1)
		{
			NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_RANKINFO_REQ(this.m_i16CurPage - 1);
		}
	}

	public void OnClickNextPage(IUIObject obj)
	{
		if (this.m_i16MaxPage <= this.m_i16CurPage)
		{
			return;
		}
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_RANKINFO_REQ(this.m_i16CurPage + 1);
	}

	public void SetPage(short i16CurPage, short i16MaxPage)
	{
		this.m_i16CurPage = i16CurPage;
		this.m_i16MaxPage = i16MaxPage;
		string text = string.Format("{0}/{1}", i16CurPage.ToString(), i16MaxPage.ToString());
		this.m_bxPage.SetText(text);
	}

	public void SetRewardButtonEnable(bool isEnabel)
	{
		this.m_box_Noti1.Hide(!isEnabel);
		this.m_btRewardGet.SetEnabled(isEnabel);
	}
}
