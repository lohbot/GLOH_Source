using System;
using UnityForms;

public class QuestGradeInfoUI_DLG : Form
{
	private Label m_Questreset_gradeinfo_level;

	private Label m_Questreset_gradeinfo_exp;

	private Label m_Questreset_gradeinfo_money;

	private Box m_Box_Box27;

	private ListBox m_Questreset_gradeinfo_recruitlist;

	private ListBox m_Questreset_gradeinfo_upgradelist;

	private Button m_Questreset_gradeinfo_pregrade;

	private Button m_Questreset_gradeinfo_nextgrade;

	private DrawTexture m_Questreset_gradeinfo_star1;

	private DrawTexture m_Questreset_gradeinfo_star2;

	private DrawTexture m_Questreset_gradeinfo_star3;

	private DrawTexture m_Questreset_gradeinfo_star4;

	private DrawTexture m_Questreset_gradeinfo_star5;

	private int m_i32CurrentGrade = 1;

	private CQuestGroup m_CurrentGroup;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestReset/DLG_Questreset_gradeinfo", G_ID.QUEST_GRADE_INFO_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_Questreset_gradeinfo_level = (base.GetControl("Questreset_gradeinfo_level") as Label);
		this.m_Questreset_gradeinfo_exp = (base.GetControl("Questreset_gradeinfo_exp") as Label);
		this.m_Questreset_gradeinfo_money = (base.GetControl("Questreset_gradeinfo_money") as Label);
		this.m_Box_Box27 = (base.GetControl("Box_Box27") as Box);
		this.m_Questreset_gradeinfo_recruitlist = (base.GetControl("Questreset_gradeinfo_recruitlist") as ListBox);
		this.m_Questreset_gradeinfo_upgradelist = (base.GetControl("Questreset_gradeinfo_upgradelist") as ListBox);
		this.m_Questreset_gradeinfo_pregrade = (base.GetControl("Questreset_gradeinfo_pregrade") as Button);
		this.m_Questreset_gradeinfo_nextgrade = (base.GetControl("Questreset_gradeinfo_nextgrade") as Button);
		this.m_Questreset_gradeinfo_star1 = (base.GetControl("Questreset_gradeinfo_star1") as DrawTexture);
		this.m_Questreset_gradeinfo_star2 = (base.GetControl("Questreset_gradeinfo_star2") as DrawTexture);
		this.m_Questreset_gradeinfo_star3 = (base.GetControl("Questreset_gradeinfo_star3") as DrawTexture);
		this.m_Questreset_gradeinfo_star4 = (base.GetControl("Questreset_gradeinfo_star4") as DrawTexture);
		this.m_Questreset_gradeinfo_star5 = (base.GetControl("Questreset_gradeinfo_star5") as DrawTexture);
		Button expr_124 = this.m_Questreset_gradeinfo_pregrade;
		expr_124.Click = (EZValueChangedDelegate)Delegate.Combine(expr_124.Click, new EZValueChangedDelegate(this.Button_Left));
		Button expr_14B = this.m_Questreset_gradeinfo_nextgrade;
		expr_14B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_14B.Click, new EZValueChangedDelegate(this.Button_Right));
		this.m_Questreset_gradeinfo_recruitlist.ColumnNum = 2;
		this.m_Questreset_gradeinfo_recruitlist.UseColumnRect = true;
		this.m_Questreset_gradeinfo_recruitlist.SetColumnRect(0, 10, 7, 51, 51, SpriteText.Anchor_Pos.Middle_Center, 15f);
		this.m_Questreset_gradeinfo_recruitlist.SetColumnRect(1, 68, 23, 120, 20, SpriteText.Anchor_Pos.Middle_Center, 15f);
		this.m_Questreset_gradeinfo_upgradelist.ColumnNum = 2;
		this.m_Questreset_gradeinfo_upgradelist.UseColumnRect = true;
		this.m_Questreset_gradeinfo_upgradelist.SetColumnRect(0, 10, 7, 51, 51, SpriteText.Anchor_Pos.Middle_Center, 15f);
		this.m_Questreset_gradeinfo_upgradelist.SetColumnRect(1, 68, 23, 120, 20, SpriteText.Anchor_Pos.Middle_Center, 15f);
	}

	private void SetGrade()
	{
		switch (this.m_i32CurrentGrade)
		{
		case 1:
			this.m_Questreset_gradeinfo_star1.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star2.SetTexture("Com_I_Star22");
			this.m_Questreset_gradeinfo_star3.SetTexture("Com_I_Star22");
			this.m_Questreset_gradeinfo_star4.SetTexture("Com_I_Star22");
			this.m_Questreset_gradeinfo_star5.SetTexture("Com_I_Star22");
			break;
		case 2:
			this.m_Questreset_gradeinfo_star1.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star2.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star3.SetTexture("Com_I_Star22");
			this.m_Questreset_gradeinfo_star4.SetTexture("Com_I_Star22");
			this.m_Questreset_gradeinfo_star5.SetTexture("Com_I_Star22");
			break;
		case 3:
			this.m_Questreset_gradeinfo_star1.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star2.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star3.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star4.SetTexture("Com_I_Star22");
			this.m_Questreset_gradeinfo_star5.SetTexture("Com_I_Star22");
			break;
		case 4:
			this.m_Questreset_gradeinfo_star1.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star2.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star3.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star4.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star5.SetTexture("Com_I_Star22");
			break;
		case 5:
			this.m_Questreset_gradeinfo_star1.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star2.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star3.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star4.SetTexture("Com_I_Star21");
			this.m_Questreset_gradeinfo_star5.SetTexture("Com_I_Star21");
			break;
		}
	}

	public void SetGroupInfo(CQuestGroup kQuestGroup)
	{
		this.m_CurrentGroup = kQuestGroup;
		this.SetDlgInfo();
	}

	private void SetDlgInfo()
	{
		byte b = 0;
		byte b2 = 0;
		long num = 0L;
		long num2 = 0L;
		this.m_Questreset_gradeinfo_recruitlist.Clear();
		this.m_Questreset_gradeinfo_upgradelist.Clear();
		this.SetGrade();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("667");
		string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("666");
		this.m_Box_Box27.Text = string.Concat(new string[]
		{
			this.m_CurrentGroup.GetChapterUnique().ToString(),
			textFromInterface,
			" ",
			this.m_CurrentGroup.GetPage(),
			textFromInterface2,
			" ",
			this.m_CurrentGroup.GetGroupTitle()
		});
		if (this.m_CurrentGroup != null)
		{
			for (int i = 0; i < 200; i++)
			{
				QUEST_SORTID qUEST_SORTID = this.m_CurrentGroup.GetGroupInfo().m_QuestUniqueBit[i];
				if (qUEST_SORTID != null)
				{
					if (qUEST_SORTID.m_strQuestUnique != string.Empty && !qUEST_SORTID.m_strQuestUnique.Equals("0"))
					{
						CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(qUEST_SORTID.m_strQuestUnique);
						if (questByQuestUnique != null)
						{
							QEUST_REWARD_ITEM qEUST_REWARD_ITEM = questByQuestUnique.GetQuestCommon().cQuestRewardItem[this.m_i32CurrentGrade - 1];
							if (qEUST_REWARD_ITEM != null)
							{
								num += qEUST_REWARD_ITEM.i64RewardMoney;
								num2 += qEUST_REWARD_ITEM.i64RewardExp;
								if (qEUST_REWARD_ITEM.i32RecruitGenCharKind > 0)
								{
									NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(qEUST_REWARD_ITEM.i32RecruitGenCharKind);
									if (charKindInfo != null)
									{
										ListItem listItem = new ListItem();
										listItem.SetColumnGUIContent(0, charKindInfo.GetCharKind(), false);
										listItem.SetColumnGUIContent(1, NrTSingleton<CTextParser>.Instance.GetTextColor("202") + charKindInfo.GetName());
										this.m_Questreset_gradeinfo_recruitlist.Add(listItem);
										b += 1;
									}
								}
								if (qEUST_REWARD_ITEM.i32UpgradeGenCharKind > 0)
								{
									NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(qEUST_REWARD_ITEM.i32UpgradeGenCharKind);
									if (charKindInfo2 != null)
									{
										ListItem listItem2 = new ListItem();
										listItem2.SetColumnGUIContent(0, charKindInfo2.GetCharKind(), false);
										listItem2.SetColumnGUIContent(1, NrTSingleton<CTextParser>.Instance.GetTextColor("202") + charKindInfo2.GetName());
										this.m_Questreset_gradeinfo_upgradelist.Add(listItem2);
										b2 += 1;
									}
								}
							}
						}
					}
				}
			}
		}
		this.m_Questreset_gradeinfo_recruitlist.RepositionItems();
		this.m_Questreset_gradeinfo_upgradelist.RepositionItems();
		CQuest firstQuest = this.m_CurrentGroup.GetFirstQuest();
		this.m_Questreset_gradeinfo_level.Text = "Lv.001";
		if (firstQuest != null)
		{
			short num3 = firstQuest.GetQuestCommon().i16RequireLevel[this.m_i32CurrentGrade - 1];
			this.m_Questreset_gradeinfo_level.Text = "Lv." + num3.ToString("00#");
		}
		this.m_Questreset_gradeinfo_exp.Text = ANNUALIZED.Convert(num2);
		this.m_Questreset_gradeinfo_money.Text = ANNUALIZED.Convert(num);
	}

	private void Button_Left(IUIObject obj)
	{
		this.m_i32CurrentGrade--;
		if (1 > this.m_i32CurrentGrade)
		{
			this.m_i32CurrentGrade = 1;
		}
		this.SetDlgInfo();
	}

	private void Button_Right(IUIObject obj)
	{
		this.m_i32CurrentGrade++;
		if (5 < this.m_i32CurrentGrade)
		{
			this.m_i32CurrentGrade = 5;
		}
		this.SetDlgInfo();
	}
}
