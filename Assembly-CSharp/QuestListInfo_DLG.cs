using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityForms;

public class QuestListInfo_DLG : Form
{
	private CQuest m_Quest;

	private QUEST_CONST.eQUESTSTATE m_eQuestState;

	private Label m_Quest_Info_Chapter_Page_Quest_Title;

	private Button m_Quest_Info_Cancel;

	private ListBox m_Quest_Info_Summary;

	private Box m_Quest_Info_Description_BG2;

	private Box m_Quest_Info_Description_BG1;

	private Label m_Quest_Info_Condition_Hint01;

	private Label m_Quest_Info_Condition_Hint02;

	private Label m_Quest_Info_Condition_Hint03;

	private DrawTexture m_Quest_Info_Difficulf1;

	private DrawTexture m_Quest_Info_Difficulf2;

	private DrawTexture m_Quest_Info_Difficulf3;

	private DrawTexture m_Quest_Info_Difficulf4;

	private DrawTexture m_Quest_Info_Difficulf5;

	private Button m_Button_Button30;

	private Button m_Button_Button31;

	private Label m_Quest_Info_Reward_Cash;

	private Label m_Quest_Info_Reward_Cash_Num;

	private DrawTexture m_DrawTexture_DrawTexture39;

	private Label m_Quest_Info_Reward_Exp;

	private Label m_Quest_Info_Reward_Exp_Num;

	private DrawTexture m_DrawTexture_DrawTexture41_C;

	private Label m_Label_Label35;

	private Label m_Label_Label36;

	private DrawTexture m_DrawTexture_DrawTexture42;

	private int m_i32CurGrade;

	private string str1101 = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");

	private string str1107 = NrTSingleton<CTextParser>.Instance.GetTextColor("1107");

	private string str1201 = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestList/DLG_QuestList_Info", G_ID.QUESTLISTINFO_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_Quest_Info_Chapter_Page_Quest_Title = (base.GetControl("Quest_Info_Chapter_Page_Quest_Title") as Label);
		this.m_Quest_Info_Cancel = (base.GetControl("Quest_Info_Cancel") as Button);
		this.m_Quest_Info_Summary = (base.GetControl("Quest_Info_Summary") as ListBox);
		this.m_Quest_Info_Summary.SetLabelScroll(15f, SpriteText.Font_Effect.Black_Shadow_Small);
		this.m_Quest_Info_Summary.itemSpacing = 10f;
		this.m_Quest_Info_Description_BG2 = (base.GetControl("Quest_Info_Description_BG2") as Box);
		this.m_Quest_Info_Description_BG1 = (base.GetControl("Quest_Info_Description_BG1") as Box);
		this.m_Quest_Info_Condition_Hint01 = (base.GetControl("Quest_Info_Condition_Hint01") as Label);
		this.m_Quest_Info_Condition_Hint02 = (base.GetControl("Quest_Info_Condition_Hint02") as Label);
		this.m_Quest_Info_Condition_Hint03 = (base.GetControl("Quest_Info_Condition_Hint03") as Label);
		this.m_Quest_Info_Difficulf1 = (base.GetControl("Quest_Info_Difficulf1") as DrawTexture);
		this.m_Quest_Info_Difficulf2 = (base.GetControl("Quest_Info_Difficulf2") as DrawTexture);
		this.m_Quest_Info_Difficulf3 = (base.GetControl("Quest_Info_Difficulf3") as DrawTexture);
		this.m_Quest_Info_Difficulf4 = (base.GetControl("Quest_Info_Difficulf4") as DrawTexture);
		this.m_Quest_Info_Difficulf5 = (base.GetControl("Quest_Info_Difficulf5") as DrawTexture);
		this.m_Button_Button30 = (base.GetControl("Button_Button30") as Button);
		this.m_Button_Button30.Visible = false;
		this.m_Button_Button31 = (base.GetControl("Button_Button31") as Button);
		this.m_Button_Button31.Visible = false;
		this.m_Quest_Info_Reward_Cash = (base.GetControl("Quest_Info_Reward_Cash") as Label);
		this.m_Quest_Info_Reward_Cash_Num = (base.GetControl("Quest_Info_Reward_Cash_Num") as Label);
		this.m_DrawTexture_DrawTexture39 = (base.GetControl("DrawTexture_DrawTexture39") as DrawTexture);
		this.m_Quest_Info_Reward_Exp = (base.GetControl("Quest_Info_Reward_Exp") as Label);
		this.m_Quest_Info_Reward_Exp_Num = (base.GetControl("Quest_Info_Reward_Exp_Num") as Label);
		this.m_DrawTexture_DrawTexture41_C = (base.GetControl("DrawTexture_DrawTexture41_C") as DrawTexture);
		this.m_Label_Label35 = (base.GetControl("Label_Label35") as Label);
		this.m_Label_Label36 = (base.GetControl("Label_Label36") as Label);
		this.m_DrawTexture_DrawTexture42 = (base.GetControl("DrawTexture_DrawTexture42") as DrawTexture);
		Button expr_24F = this.m_Button_Button30;
		expr_24F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_24F.Click, new EZValueChangedDelegate(this.Button_Left));
		Button expr_276 = this.m_Button_Button31;
		expr_276.Click = (EZValueChangedDelegate)Delegate.Combine(expr_276.Click, new EZValueChangedDelegate(this.Button_Right));
		Button expr_29D = this.m_Quest_Info_Cancel;
		expr_29D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_29D.Click, new EZValueChangedDelegate(this.OnCalcle));
		this.m_Quest_Info_Difficulf1.Visible = true;
		this.m_Quest_Info_Difficulf2.Visible = true;
		this.m_Quest_Info_Difficulf3.Visible = true;
		this.m_Quest_Info_Difficulf4.Visible = true;
		this.m_Quest_Info_Difficulf5.Visible = true;
	}

	public void SetQuestInfo(CQuest kQuest)
	{
		this.m_Quest = kQuest;
		this.m_eQuestState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(this.m_Quest.GetQuestUnique());
		if (this.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
		{
			this.m_Quest_Info_Cancel.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1162");
		}
		else if (this.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING || this.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
		{
			this.m_Quest_Info_Cancel.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("799");
		}
		else
		{
			this.m_Quest_Info_Cancel.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10");
		}
		CQuestGroup questGroupByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(this.m_Quest.GetQuestUnique());
		short chapterUnique = NrTSingleton<NkQuestManager>.Instance.GetChapterUnique(this.m_Quest.GetQuestUnique());
		string text = "None";
		if (questGroupByQuestUnique != null)
		{
			text = questGroupByQuestUnique.GetPage();
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("666");
		string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("667");
		string text2 = string.Concat(new object[]
		{
			this.str1101,
			chapterUnique,
			textFromInterface2,
			" ",
			text,
			textFromInterface,
			". ",
			this.str1107,
			kQuest.GetQuestTitle()
		});
		if (questGroupByQuestUnique.GetQuestType() == 2)
		{
			text2 = string.Concat(new string[]
			{
				this.str1101,
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1454"),
				". ",
				this.str1107,
				kQuest.GetQuestTitle()
			});
		}
		else if (questGroupByQuestUnique.GetQuestType() == 100)
		{
			text2 = string.Concat(new string[]
			{
				this.str1101,
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("784"),
				". ",
				this.str1107,
				kQuest.GetQuestTitle()
			});
		}
		this.m_Quest_Info_Chapter_Page_Quest_Title.SetText(text2);
		text2 = string.Empty;
		this.m_Quest_Info_Summary.Clear();
		ListItem listItem = new ListItem();
		if (this.m_eQuestState <= QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1202");
			this.m_Quest_Info_Summary.SetColumnAlignment(0, SpriteText.Anchor_Pos.Middle_Center);
			this.m_Quest_Info_Description_BG2.SetAlpha(0.7f);
			this.m_Quest_Info_Description_BG1.SetAlpha(0.7f);
		}
		else
		{
			text2 = this.m_Quest.GetQuestSummary();
			this.m_Quest_Info_Summary.SetColumnAlignment(0, SpriteText.Anchor_Pos.Middle_Left);
			this.m_Quest_Info_Description_BG2.SetAlpha(1f);
			this.m_Quest_Info_Description_BG1.SetAlpha(1f);
		}
		listItem.SetColumnStr(0, text2);
		this.m_Quest_Info_Summary.Add(listItem);
		this.m_Quest_Info_Summary.RepositionItems();
		text2 = string.Empty;
		if (this.m_Quest.GetQuestCommon().cQuestCondition[0] != null)
		{
			text2 = this.m_Quest.GetConditionText(0L, 0);
			this.m_Quest_Info_Condition_Hint01.SetText(this.str1201 + text2);
		}
		if (this.m_Quest.GetQuestCommon().cQuestCondition[1] != null)
		{
			text2 = this.m_Quest.GetConditionText(0L, 1);
			this.m_Quest_Info_Condition_Hint02.SetText(this.str1201 + text2);
		}
		if (this.m_Quest.GetQuestCommon().cQuestCondition[2] != null)
		{
			text2 = this.m_Quest.GetConditionText(0L, 2);
			this.m_Quest_Info_Condition_Hint03.SetText(this.str1201 + text2);
		}
		if (this.m_eQuestState <= QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
		{
			this.m_Quest_Info_Condition_Hint01.SetText(string.Empty);
			this.m_Quest_Info_Condition_Hint02.SetText(string.Empty);
			this.m_Quest_Info_Condition_Hint03.SetText(string.Empty);
		}
		this.SetGrade();
		this.SetReward();
	}

	private void SetReward()
	{
		this.m_Quest_Info_Reward_Cash.Visible = false;
		this.m_Quest_Info_Reward_Cash_Num.Visible = false;
		this.m_DrawTexture_DrawTexture39.Visible = false;
		this.m_Quest_Info_Reward_Exp.Visible = false;
		this.m_Quest_Info_Reward_Exp_Num.Visible = false;
		this.m_DrawTexture_DrawTexture41_C.Visible = false;
		this.m_Label_Label35.Visible = false;
		this.m_Label_Label36.Visible = false;
		this.m_DrawTexture_DrawTexture42.Visible = false;
		QEUST_REWARD_ITEM qEUST_REWARD_ITEM = this.m_Quest.GetQuestCommon().cQuestRewardItem[this.m_i32CurGrade];
		if (qEUST_REWARD_ITEM == null)
		{
			return;
		}
		byte b = 0;
		if (0L < qEUST_REWARD_ITEM.i64RewardMoney)
		{
			this.SetRewardInfo(b, 0, qEUST_REWARD_ITEM);
			b += 1;
		}
		if (0L < qEUST_REWARD_ITEM.i64RewardExp)
		{
			this.SetRewardInfo(b, 1, qEUST_REWARD_ITEM);
			b += 1;
		}
		if (0 < qEUST_REWARD_ITEM.nRewardItemUnique0)
		{
			this.SetRewardInfo(b, 2, qEUST_REWARD_ITEM);
			b += 1;
		}
		if (qEUST_REWARD_ITEM.i32RecruitGenCharKind == 0)
		{
			if (0 < qEUST_REWARD_ITEM.nRecruitReplaceItemUnique)
			{
				this.SetRewardInfo(b, 5, qEUST_REWARD_ITEM);
				b += 1;
			}
		}
		else
		{
			this.SetRewardInfo(b, 3, qEUST_REWARD_ITEM);
			b += 1;
		}
		if (qEUST_REWARD_ITEM.i32UpgradeGenCharKind == 0)
		{
			if (0 < qEUST_REWARD_ITEM.nUpgradeReplaceItemUnique)
			{
				this.SetRewardInfo(b, 6, qEUST_REWARD_ITEM);
				b += 1;
			}
		}
		else
		{
			this.SetRewardInfo(b, 4, qEUST_REWARD_ITEM);
			b += 1;
		}
	}

	private void SetRewardInfo(byte num, byte type, QEUST_REWARD_ITEM kReward)
	{
		if (num > 3)
		{
			return;
		}
		switch (num)
		{
		case 0:
			this.SetItem(type, kReward, ref this.m_Quest_Info_Reward_Cash, ref this.m_Quest_Info_Reward_Cash_Num, ref this.m_DrawTexture_DrawTexture39);
			break;
		case 1:
			this.SetItem(type, kReward, ref this.m_Quest_Info_Reward_Exp, ref this.m_Quest_Info_Reward_Exp_Num, ref this.m_DrawTexture_DrawTexture41_C);
			break;
		case 2:
			this.SetItem(type, kReward, ref this.m_Label_Label35, ref this.m_Label_Label36, ref this.m_DrawTexture_DrawTexture42);
			break;
		}
	}

	private void SetItem(byte type, QEUST_REWARD_ITEM kReward, ref Label kLabel1, ref Label kLabel2, ref DrawTexture texture)
	{
		switch (type)
		{
		case 0:
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676");
			kLabel1.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("790");
			kLabel2.Text = ANNUALIZED.Convert(kReward.i64RewardMoney) + textFromInterface;
			texture.SetTexture("Main_I_ExtraI01");
			texture.nItemUniqueTooltip = 0;
			break;
		}
		case 1:
			kLabel1.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("791");
			kLabel2.Text = ANNUALIZED.Convert(kReward.i64RewardExp);
			texture.SetTexture("Main_I_ExtraI02");
			texture.nItemUniqueTooltip = 0;
			break;
		case 2:
			kLabel1.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(kReward.nRewardItemUnique0);
			kLabel2.Text = "x " + kReward.nRewardItemNum0.ToString();
			texture.BaseInfoLoderImage = NrTSingleton<ItemManager>.Instance.GetItemTexture(kReward.nRewardItemUnique0);
			texture.nItemUniqueTooltip = kReward.nRewardItemUnique0;
			break;
		case 3:
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(kReward.i32RecruitGenCharKind);
			if (charKindInfo != null)
			{
				kLabel1.Text = charKindInfo.GetName();
				texture.SetTexture(eCharImageType.SMALL, charKindInfo.GetCharKind(), -1, string.Empty);
			}
			kLabel2.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("488");
			texture.nItemUniqueTooltip = 0;
			break;
		}
		case 4:
		{
			NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(kReward.i32UpgradeGenCharKind);
			if (charKindInfo2 != null)
			{
				kLabel1.Text = charKindInfo2.GetName();
				texture.SetTexture(eCharImageType.SMALL, charKindInfo2.GetCharKind(), -1, string.Empty);
			}
			kLabel2.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("487");
			texture.nItemUniqueTooltip = 0;
			break;
		}
		case 5:
			kLabel1.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(kReward.nRecruitReplaceItemUnique);
			kLabel2.Text = "x " + kReward.nRecruitReplaceItemNum.ToString();
			texture.BaseInfoLoderImage = NrTSingleton<ItemManager>.Instance.GetItemTexture(kReward.nRecruitReplaceItemNum);
			texture.nItemUniqueTooltip = kReward.nRecruitReplaceItemUnique;
			break;
		case 6:
			kLabel1.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(kReward.nUpgradeReplaceItemUnique);
			kLabel2.Text = "x " + kReward.nUpgradeReplaceItemNum.ToString();
			texture.BaseInfoLoderImage = NrTSingleton<ItemManager>.Instance.GetItemTexture(kReward.nUpgradeReplaceItemNum);
			texture.nItemUniqueTooltip = kReward.nUpgradeReplaceItemUnique;
			break;
		}
		kLabel1.Visible = true;
		kLabel2.Visible = true;
		texture.Visible = true;
	}

	public void CancelQuest(object a_oObject)
	{
		CQuest cQuest = (CQuest)a_oObject;
		if (cQuest == null)
		{
			return;
		}
		GS_QUEST_CANCLE_REQ gS_QUEST_CANCLE_REQ = new GS_QUEST_CANCLE_REQ();
		TKString.StringChar(cQuest.GetQuestUnique(), ref gS_QUEST_CANCLE_REQ.strQuestUnique);
		SendPacket.GetInstance().SendObject(1013, gS_QUEST_CANCLE_REQ);
		this.Close();
	}

	private void OnCalcle(IUIObject obj)
	{
		if (this.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING || this.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				msgBoxUI.SetMsg(new YesDelegate(this.CancelQuest), this.m_Quest, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("799"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("126"), eMsgType.MB_OK_CANCEL, 2);
				msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("320"));
				msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("321"));
			}
		}
		else if (this.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
		{
			NrTSingleton<NkQuestManager>.Instance.QuestAutoMove(this.m_Quest.GetQuestUnique());
			G_ID parentFormID = base.InteractivePanel.parentFormID;
			QuestList_DLG questList_DLG = NrTSingleton<FormsManager>.Instance.GetForm(parentFormID) as QuestList_DLG;
			if (questList_DLG != null)
			{
				questList_DLG.CloseDlg = true;
			}
		}
		else
		{
			this.Close();
		}
	}

	private void SetGrade()
	{
		switch (this.m_i32CurGrade)
		{
		case 0:
			this.m_Quest_Info_Difficulf1.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf2.SetTexture("Com_I_Star12");
			this.m_Quest_Info_Difficulf3.SetTexture("Com_I_Star12");
			this.m_Quest_Info_Difficulf4.SetTexture("Com_I_Star12");
			this.m_Quest_Info_Difficulf5.SetTexture("Com_I_Star12");
			break;
		case 1:
			this.m_Quest_Info_Difficulf1.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf2.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf3.SetTexture("Com_I_Star12");
			this.m_Quest_Info_Difficulf4.SetTexture("Com_I_Star12");
			this.m_Quest_Info_Difficulf5.SetTexture("Com_I_Star12");
			break;
		case 2:
			this.m_Quest_Info_Difficulf1.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf2.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf3.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf4.SetTexture("Com_I_Star12");
			this.m_Quest_Info_Difficulf5.SetTexture("Com_I_Star12");
			break;
		case 3:
			this.m_Quest_Info_Difficulf1.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf2.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf3.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf4.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf5.SetTexture("Com_I_Star12");
			break;
		case 4:
			this.m_Quest_Info_Difficulf1.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf2.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf3.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf4.SetTexture("Com_I_Star11");
			this.m_Quest_Info_Difficulf5.SetTexture("Com_I_Star11");
			break;
		}
	}

	private void Button_Left(IUIObject obj)
	{
		this.m_i32CurGrade--;
		if (this.m_i32CurGrade < 0)
		{
			this.m_i32CurGrade = 0;
		}
		this.SetGrade();
		this.SetReward();
	}

	private void Button_Right(IUIObject obj)
	{
		this.m_i32CurGrade++;
		if (this.m_i32CurGrade >= 5)
		{
			this.m_i32CurGrade = 4;
		}
		this.SetGrade();
		this.SetReward();
	}
}
