using GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class ColosseumRewardExplainDlg : Form
{
	public class UI_RewardControl
	{
		public Label[] m_laRank = new Label[3];

		public Label[,] m_laRewardItem = new Label[3, 2];
	}

	private const int MAX_EXPLAIN_RANKIMAGE = 5;

	public const int MAX_GRADE_RANKREWARCOUNT = 3;

	public const int MAX_SHOW_ITEM_COUNT = 2;

	public Label m_laRewarCount;

	private ColosseumRewardExplainDlg.UI_RewardControl[] m_UIRewardControl = new ColosseumRewardExplainDlg.UI_RewardControl[6];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		form.AlwaysUpdate = false;
		instance.LoadFileAll(ref form, "Colosseum/DLG_RewardInfo", G_ID.COLOSSEUMREWARD_EXPLAIN_DLG, false, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_laRewarCount = (base.GetControl("LB_RewardCount") as Label);
		for (int i = 0; i < 6; i++)
		{
			this.m_UIRewardControl[i] = new ColosseumRewardExplainDlg.UI_RewardControl();
			for (int j = 0; j < 3; j++)
			{
				this.m_UIRewardControl[i].m_laRank[j] = (base.GetControl(string.Format("LB_Rank{0}_{1}", i, j)) as Label);
				for (int k = 0; k < 2; k++)
				{
					this.m_UIRewardControl[i].m_laRewardItem[j, k] = (base.GetControl(string.Format("LB_Reward{0}_{1}_{2}", i, j, k)) as Label);
				}
			}
		}
		base.SetScreenCenter();
	}

	public void ShowColosseumRewardExplain()
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697");
		string empty = string.Empty;
		string text = string.Empty;
		for (short num = 0; num < 6; num += 1)
		{
			if (num != 0)
			{
				List<COLOSSEUM_RANK_REWARD> list = NrTSingleton<NrTable_ColosseumRankReward_Manager>.Instance.Get_RewarList(num);
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].m_nExplainItemUnique1 > 0)
						{
							text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(list[i].m_nExplainItemUnique1);
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								textFromInterface,
								"itemname",
								text,
								"count",
								list[i].m_nExplainItemNum1.ToString()
							});
							if (this.m_UIRewardControl[(int)num].m_laRewardItem[i, 0] != null)
							{
								this.m_UIRewardControl[(int)num].m_laRewardItem[i, 0].Text = empty;
							}
						}
						if (list[i].m_nExplainItemUnique2 > 0)
						{
							text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(list[i].m_nExplainItemUnique2);
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								textFromInterface,
								"itemname",
								text,
								"count",
								list[i].m_nExplainItemNum2.ToString()
							});
							if (this.m_UIRewardControl[(int)num].m_laRewardItem[i, 1] != null)
							{
								this.m_UIRewardControl[(int)num].m_laRewardItem[i, 1].Text = empty;
							}
						}
					}
				}
			}
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		int value = COLOSSEUM_CONSTANT_Manager.GetInstance().GetValue(eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_ONEDAY_GIVEITEM_LIMITCOUNT);
		short charDetailFromUnion = kMyCharInfo.GetCharDetailFromUnion(eCHAR_DETAIL_INFO.eCHAR_DETAIL_INFO_LIMIT_COUNT, 3);
		textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2472");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"count",
			value - (int)charDetailFromUnion
		});
		this.m_laRewarCount.Text = empty;
	}
}
