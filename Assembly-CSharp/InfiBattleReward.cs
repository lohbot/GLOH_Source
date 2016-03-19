using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityForms;

public class InfiBattleReward : Form
{
	private DrawTexture m_DrawTexture_Main_BG;

	private Label[] m_LabelLBReward = new Label[9];

	private Label m_LabelLastWeekRank;

	private Button m_ButtonRewardGet;

	private Button m_ButtonClose;

	private NewListBox m_NewlistBox;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Plunder/InfiBattle/DLG_InfiRewardInfo", G_ID.INFIBATTLE_REWARD_DLG, true);
	}

	public override void SetComponent()
	{
		for (int i = 0; i < 9; i++)
		{
			int num = i + 1;
			this.m_LabelLBReward[i] = (base.GetControl("LB_Reward" + num) as Label);
		}
		this.m_LabelLastWeekRank = (base.GetControl("LB_LastweekRank2") as Label);
		this.m_ButtonRewardGet = (base.GetControl("BT_RewardGet") as Button);
		this.m_ButtonRewardGet.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickRewardGet));
		this.m_ButtonClose = (base.GetControl("BT_Close") as Button);
		this.m_ButtonClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickClose));
		this.m_NewlistBox = (base.GetControl("NewListBox_recentrank") as NewListBox);
		this.m_NewlistBox.Reserve = false;
		this.m_ButtonRewardGet.Visible = false;
		this.m_DrawTexture_Main_BG = (base.GetControl("DrawTexture_BGIMG") as DrawTexture);
		this.m_DrawTexture_Main_BG.SetTextureFromBundle("UI/PvP/infibattle_reward");
		this.InitGUI();
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void InitGUI()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		for (int i = 0; i < 9; i++)
		{
			this.m_LabelLBReward[i].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225"));
		}
		string text = string.Empty;
		string text2 = string.Empty;
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_RANKLIMIT);
			if (value < myCharInfo.InfinityBattle_OldRank || 0 >= myCharInfo.InfinityBattle_OldRank)
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2509");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"rank",
					myCharInfo.InfinityBattle_Rank
				});
			}
			this.m_LabelLastWeekRank.SetText(text2);
		}
	}

	public void On_ClickRewardGet(IUIObject a_cObject)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			InfiBattleReward infiBattleReward = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INFIBATTLE_REWARD_DLG) as InfiBattleReward;
			if (infiBattleReward != null)
			{
				GS_INFIBATTLE_GETREWARD_REQ gS_INFIBATTLE_GETREWARD_REQ = new GS_INFIBATTLE_GETREWARD_REQ();
				gS_INFIBATTLE_GETREWARD_REQ.i64PersonID = myCharInfo.m_PersonID;
				SendPacket.GetInstance().SendObject(2013, gS_INFIBATTLE_GETREWARD_REQ);
			}
		}
	}

	public void On_ClickClose(IUIObject a_cObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.INFIBATTLE_REWARD_DLG);
	}

	public void SetRewardInfo(GS_INFIBATTLE_REWARDINFO_ACK ACK)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		for (int i = 0; i < 9; i++)
		{
			if (0 >= ACK.i32Rank[i] || 0 >= ACK.i32RewardUnique[i])
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
			}
			else
			{
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(ACK.i32RewardUnique[i]);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"itemname",
					itemNameByItemUnique,
					"count",
					ACK.i16RewardNum[i]
				});
			}
			this.m_LabelLBReward[i].SetText(text2);
		}
		if (ACK.i32Result == 0)
		{
			this.m_LabelLastWeekRank.SetText(ACK.i32OldRank.ToString());
			this.m_ButtonRewardGet.Visible = true;
		}
		else
		{
			TsLog.LogWarning("!!!! GS_INFIBATTLE_REWARDINFO_ACK :{0}", new object[]
			{
				ACK.i32Result
			});
			this.m_ButtonRewardGet.Visible = false;
		}
	}

	public void SetTopRankStart()
	{
		this.m_NewlistBox.Clear();
	}

	public void SetTopRankEnd()
	{
		this.m_NewlistBox.RepositionItems();
	}

	public void SetTopRank(int iCount, int i32Rank, string szCharName)
	{
		string empty = string.Empty;
		NewListItem newListItem = new NewListItem(this.m_NewlistBox.ColumnNum, true);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1435"),
			"charname",
			szCharName
		});
		newListItem.SetListItemData(0, empty, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2509"),
			"rank",
			i32Rank
		});
		newListItem.SetListItemData(1, empty, null, null, null);
		this.m_NewlistBox.InsertAdd(iCount, newListItem);
	}
}
