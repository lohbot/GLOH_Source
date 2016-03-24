using System;
using UnityEngine;
using UnityForms;

public class MythRaid_RewardInfo_DLG : Form
{
	private NewListBox nlb_RewardInfo;

	private Label lb_RewardRankInfo;

	private Label lb_RewardInfo;

	private Button bt_RewardGet;

	private DrawTexture dt_popup_BG;

	private Button bt_Exit;

	private float listChangeTime;

	private int myRankIndex;

	private bool isListChange;

	private bool isEffectOn;

	private string[] itemName = new string[7];

	private int[] itemNum = new int[7];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "mythraid/dlg_myth_rewardinfo", G_ID.MYTHRAID_REWARDINFO_DLG, false);
		base.AlwaysUpdate = true;
	}

	public override void OnClose()
	{
	}

	public override void SetComponent()
	{
		base.SetScreenCenter();
		this.nlb_RewardInfo = (base.GetControl("NLB_RewardInfo") as NewListBox);
		this.lb_RewardRankInfo = (base.GetControl("LB_RewardRankInfo") as Label);
		this.lb_RewardInfo = (base.GetControl("LB_RewardInfo") as Label);
		this.bt_RewardGet = (base.GetControl("BT_RewardGet") as Button);
		Button expr_64 = this.bt_RewardGet;
		expr_64.Click = (EZValueChangedDelegate)Delegate.Combine(expr_64.Click, new EZValueChangedDelegate(this.OnClickRewardGet));
		this.bt_RewardGet.SetEnabled(false);
		this.dt_popup_BG = (base.GetControl("DrawTexture_popup_BG") as DrawTexture);
		this.bt_Exit = (base.GetControl("Button_Exit") as Button);
		Button expr_C3 = this.bt_Exit;
		expr_C3.Click = (EZValueChangedDelegate)Delegate.Combine(expr_C3.Click, new EZValueChangedDelegate(this.CloseForm));
		base.ShowBlackBG(0.5f);
	}

	private void OnClickRewardGet(IUIObject obj)
	{
		NrTSingleton<MythRaidManager>.Instance.AskRewardInfo(false);
	}

	public override void InitData()
	{
		base.InitData();
	}

	public override void Update()
	{
		if (Time.realtimeSinceStartup - this.listChangeTime >= 0.3f && !this.isListChange)
		{
			this.nlb_RewardInfo.ScrollToItem(this.myRankIndex, 0.5f);
			this.isListChange = true;
		}
		if (this.isEffectOn)
		{
			Animation componentInChildren = this.dt_popup_BG.transform.GetComponentInChildren<Animation>();
			if (componentInChildren != null && !componentInChildren.isPlaying)
			{
				this.isEffectOn = false;
				NrTSingleton<MythRaidManager>.Instance.ActiveRewardMsgBox(this.itemName, this.itemNum);
				this.itemNum.Initialize();
				this.itemName.Initialize();
			}
		}
		base.Update();
	}

	public void Show(int rewardRank)
	{
		this.listChangeTime = Time.realtimeSinceStartup;
		this.isListChange = false;
		this.myRankIndex = 0;
		this.SetRewardUI(NrTSingleton<MythRaidManager>.Instance.CanGetReward);
		this.SetMyRankReward(rewardRank);
		this.ShowList(NrTSingleton<MythRaidManager>.Instance.MyNowBestRank());
		base.Show();
	}

	public void SetMyRankReward(int myRank)
	{
		if (myRank == 0)
		{
			this.lb_RewardInfo.SetText(string.Empty);
			this.lb_RewardRankInfo.SetText("-");
			return;
		}
		int rankIndex = this.GetRankIndex(myRank);
		MYTHRAID_RANK_REWARD_INFO mythRaidRankRewardData = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidRankRewardData(rankIndex.ToString());
		this.lb_RewardInfo.SetText(this.GetReward(mythRaidRankRewardData));
		this.lb_RewardRankInfo.SetText(myRank.ToString());
	}

	public int GetRankIndex(int _rank)
	{
		int result = 0;
		int resourceCount = NrTSingleton<NrBaseTableManager>.Instance.GetResourceCount(NrTableData.eResourceType.eRT_MYTHRAIDRANKREWARD);
		for (int i = 0; i < resourceCount; i++)
		{
			MYTHRAID_RANK_REWARD_INFO mythRaidRankRewardData = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidRankRewardData(i.ToString());
			if (mythRaidRankRewardData.RANK >= _rank)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private void ShowList(int _rank)
	{
		this.myRankIndex = this.GetRankIndex(_rank);
		this.nlb_RewardInfo.SetColumnCenter();
		int resourceCount = NrTSingleton<NrBaseTableManager>.Instance.GetResourceCount(NrTableData.eResourceType.eRT_MYTHRAIDRANKREWARD);
		for (int i = 0; i < resourceCount; i++)
		{
			MYTHRAID_RANK_REWARD_INFO mythRaidRankRewardData = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidRankRewardData(i.ToString());
			NewListItem newListItem = new NewListItem(this.nlb_RewardInfo.ColumnNum, true, string.Empty);
			newListItem.SetListItemData(3, this.ChangeRank(i), null, null, null);
			string reward = this.GetReward(mythRaidRankRewardData);
			newListItem.SetListItemData(1, reward, null, null, null);
			if (this.myRankIndex - 1 == i)
			{
				newListItem.SetListItemData(0, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_T_ListBoxBlueHL"), null, null, null);
			}
			else if (_rank != 0 && this.myRankIndex == i)
			{
				newListItem.SetListItemData(0, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_T_ListBoxOrangeHL"), null, null, null);
			}
			this.nlb_RewardInfo.Add(newListItem);
		}
		this.nlb_RewardInfo.RepositionItems();
	}

	private string ChangeRank(int index)
	{
		MYTHRAID_RANK_REWARD_INFO mythRaidRankRewardData = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidRankRewardData(index.ToString());
		MYTHRAID_RANK_REWARD_INFO mythRaidRankRewardData2 = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidRankRewardData((index - 1).ToString());
		if (mythRaidRankRewardData2 == null)
		{
			return mythRaidRankRewardData.RANK.ToString();
		}
		if (NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidRankRewardData((index + 1).ToString()) == null)
		{
			return string.Format("{0}{1}", mythRaidRankRewardData2.RANK + 1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3251"));
		}
		return string.Format("{0} ~ {1}", mythRaidRankRewardData2.RANK + 1, mythRaidRankRewardData.RANK.ToString());
	}

	private string GetReward(MYTHRAID_RANK_REWARD_INFO rewardInfo)
	{
		string text = string.Empty;
		for (int i = 0; i < rewardInfo.REWARD_UNIQUE.Count; i++)
		{
			if (rewardInfo.REWARD_UNIQUE[i] <= 0)
			{
				break;
			}
			if (i >= 1)
			{
				text += " + ";
			}
			text = string.Concat(new object[]
			{
				text,
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(rewardInfo.REWARD_UNIQUE[i]),
				" x ",
				rewardInfo.REWARD_NUMBER[i]
			});
		}
		return text;
	}

	public void SetRewardUI(bool value)
	{
		this.bt_RewardGet.SetEnabled(value);
	}

	public void SetRewardInfo(string[] _itemName, int[] _itemNum)
	{
		this.isEffectOn = true;
		this.itemName = _itemName;
		this.itemNum = _itemNum;
		NrTSingleton<MythRaidManager>.Instance.ActiveRewardEffect(this.dt_popup_BG);
	}
}
