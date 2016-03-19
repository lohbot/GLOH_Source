using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class QuestGM_DLG : Form
{
	private Button m_Button_Button4;

	private Button m_Button_Button7;

	private DropDownList m_DropDownList2;

	private DropDownList m_DropDownList3;

	private Button m_Button_Button15;

	private Button m_Button_Button16;

	private Button m_Button_Button17;

	private Button m_Button_Button18;

	private Button m_Button_Button19;

	private Button m_Button_Button20;

	private Button m_Button_Button23;

	private Button m_Button_Button25;

	private Button m_Button_Button26;

	private ListBox m_ListBox;

	private QUEST_CHAPTER m_CurrentChapter;

	private CQuestGroup m_CurGroup;

	private CQuest m_CurQuest;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestList/DLG_QuestGM", G_ID.QUEST_GM_DLG, true);
		float x = (GUICamera.width - base.GetSize().x) / 2f;
		float y = (GUICamera.height - base.GetSize().y) / 2f;
		y = GUICamera.height * 0.13f;
		base.SetLocation(x, y);
	}

	public override void SetComponent()
	{
		this.m_DropDownList2 = (base.GetControl("DropDownList2") as DropDownList);
		this.m_DropDownList3 = (base.GetControl("DropDownList3") as DropDownList);
		this.m_Button_Button4 = (base.GetControl("Button_Button4") as Button);
		this.m_Button_Button7 = (base.GetControl("Button_Button7") as Button);
		this.m_Button_Button15 = (base.GetControl("Button_Button15") as Button);
		this.m_Button_Button16 = (base.GetControl("Button_Button16") as Button);
		this.m_Button_Button17 = (base.GetControl("Button_Button17") as Button);
		this.m_Button_Button18 = (base.GetControl("Button_Button18") as Button);
		this.m_Button_Button19 = (base.GetControl("Button_Button19") as Button);
		this.m_Button_Button20 = (base.GetControl("Button_Button20") as Button);
		this.m_Button_Button23 = (base.GetControl("Button_Button23") as Button);
		this.m_Button_Button25 = (base.GetControl("Button_Button23") as Button);
		this.m_Button_Button26 = (base.GetControl("Button_Button23") as Button);
		this.m_ListBox = (base.GetControl("ListBox") as ListBox);
		Button expr_13A = this.m_Button_Button4;
		expr_13A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_13A.Click, new EZValueChangedDelegate(this.OnCharUniqueSet));
		Button expr_161 = this.m_Button_Button7;
		expr_161.Click = (EZValueChangedDelegate)Delegate.Combine(expr_161.Click, new EZValueChangedDelegate(this.OnQuestUniqueSet));
		Button expr_188 = this.m_Button_Button15;
		expr_188.Click = (EZValueChangedDelegate)Delegate.Combine(expr_188.Click, new EZValueChangedDelegate(this.OnQuestPass_Small));
		Button expr_1AF = this.m_Button_Button16;
		expr_1AF.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1AF.Click, new EZValueChangedDelegate(this.OnQuestInit_Small));
		Button expr_1D6 = this.m_Button_Button17;
		expr_1D6.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1D6.Click, new EZValueChangedDelegate(this.OnQuestPass_Middle));
		Button expr_1FD = this.m_Button_Button18;
		expr_1FD.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1FD.Click, new EZValueChangedDelegate(this.OnQuestInit_Middle));
		Button expr_224 = this.m_Button_Button19;
		expr_224.Click = (EZValueChangedDelegate)Delegate.Combine(expr_224.Click, new EZValueChangedDelegate(this.OnQuestAllPass));
		Button expr_24B = this.m_Button_Button20;
		expr_24B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_24B.Click, new EZValueChangedDelegate(this.OnQuestAllInit));
		Button expr_272 = this.m_Button_Button23;
		expr_272.Click = (EZValueChangedDelegate)Delegate.Combine(expr_272.Click, new EZValueChangedDelegate(this.OnOption));
		Button expr_299 = this.m_Button_Button25;
		expr_299.Click = (EZValueChangedDelegate)Delegate.Combine(expr_299.Click, new EZValueChangedDelegate(this.OnQuestPass_Big));
		Button expr_2C0 = this.m_Button_Button26;
		expr_2C0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2C0.Click, new EZValueChangedDelegate(this.OnQuestInit_Big));
		this.m_DropDownList2.AddValueChangedDelegate(new EZValueChangedDelegate(this.ChangeChapter));
		this.m_DropDownList3.AddValueChangedDelegate(new EZValueChangedDelegate(this.ChangePage));
		this.m_ListBox.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickList));
		Dictionary<short, QUEST_CHAPTER> hashQuestChapter = NrTSingleton<NkQuestManager>.Instance.GetHashQuestChapter();
		if (hashQuestChapter != null)
		{
			this.m_DropDownList2.Clear();
			foreach (QUEST_CHAPTER current in hashQuestChapter.Values)
			{
				ListItem listItem = new ListItem();
				listItem.SetColumnStr(0, current.i16QuestChapterUnique.ToString());
				listItem.Key = current;
				this.m_DropDownList2.Add(listItem);
			}
			this.m_DropDownList2.SetFirstItem();
		}
		this.m_DropDownList2.RepositionItems();
		ListItem listItem2 = this.m_DropDownList2.SelectedItem.Data as ListItem;
		this.m_CurrentChapter = (listItem2.Key as QUEST_CHAPTER);
		Dictionary<int, CQuestGroup> hashQuestGroup = NrTSingleton<NkQuestManager>.Instance.GetHashQuestGroup();
		foreach (CQuestGroup current2 in hashQuestGroup.Values)
		{
			this.m_DropDownList3.SetViewArea(10);
			this.m_DropDownList3.Clear();
			if (this.m_CurrentChapter.i16QuestChapterUnique == current2.GetChapterUnique())
			{
				ListItem listItem3 = new ListItem();
				listItem3.SetColumnStr(0, current2.GetGroupTitle() + " " + current2.GetPageUnique().ToString());
				listItem3.Key = current2;
				this.m_DropDownList3.Add(listItem3);
			}
		}
		this.m_DropDownList3.RepositionItems();
		this.m_DropDownList3.SetFirstItem();
		listItem2 = (this.m_DropDownList3.SelectedItem.Data as ListItem);
		this.m_CurGroup = (listItem2.Key as CQuestGroup);
		this.m_ListBox.ColumnNum = 1;
		if (this.m_CurGroup != null)
		{
			foreach (QUEST_SORTID current3 in this.m_CurGroup.GetGroupInfo().m_QuestUnique.Values)
			{
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(current3.m_strQuestUnique);
				if (questByQuestUnique != null)
				{
					ListItem listItem4 = new ListItem();
					listItem4.SetColumnStr(0, questByQuestUnique.GetQuestTitle() + " " + questByQuestUnique.GetQuestUnique().ToString());
					listItem4.Key = questByQuestUnique;
					this.m_ListBox.Add(listItem4);
				}
			}
			this.m_ListBox.RepositionItems();
		}
	}

	private void ClickList(IUIObject obj)
	{
		ListBox listBox = (ListBox)obj;
		UIListItemContainer selectedItem = listBox.SelectedItem;
		this.m_CurQuest = (CQuest)selectedItem.Data;
	}

	private void OnCharUniqueSet(IUIObject obj)
	{
	}

	private void OnQuestUniqueSet(IUIObject obj)
	{
	}

	private void OnQuestPass_Small(IUIObject obj)
	{
		if (this.m_CurQuest != null)
		{
			GS_QUEST_PASS_GM_REQ gS_QUEST_PASS_GM_REQ = new GS_QUEST_PASS_GM_REQ();
			TKString.StringChar(this.m_CurQuest.GetQuestUnique(), ref gS_QUEST_PASS_GM_REQ.strQuestUnique);
			SendPacket.GetInstance().SendObject(1018, gS_QUEST_PASS_GM_REQ);
		}
	}

	private void OnQuestInit_Small(IUIObject obj)
	{
		if (this.m_CurQuest != null)
		{
			GS_QUEST_INIT_GM_REQ gS_QUEST_INIT_GM_REQ = new GS_QUEST_INIT_GM_REQ();
			TKString.StringChar(this.m_CurQuest.GetQuestUnique(), ref gS_QUEST_INIT_GM_REQ.strQuestUnique);
			SendPacket.GetInstance().SendObject(1019, gS_QUEST_INIT_GM_REQ);
		}
	}

	private void OnQuestPass_Middle(IUIObject obj)
	{
		if (this.m_CurGroup != null)
		{
			GS_QUEST_GROUP_PASS_GM_REQ gS_QUEST_GROUP_PASS_GM_REQ = new GS_QUEST_GROUP_PASS_GM_REQ();
			gS_QUEST_GROUP_PASS_GM_REQ.i32GroupUnique = this.m_CurGroup.GetGroupUnique();
			SendPacket.GetInstance().SendObject(1020, gS_QUEST_GROUP_PASS_GM_REQ);
		}
	}

	private void OnQuestInit_Middle(IUIObject obj)
	{
		if (this.m_CurGroup != null)
		{
			GS_QUEST_GROUP_INIT_GM_REQ gS_QUEST_GROUP_INIT_GM_REQ = new GS_QUEST_GROUP_INIT_GM_REQ();
			gS_QUEST_GROUP_INIT_GM_REQ.i32GroupUnique = this.m_CurGroup.GetGroupUnique();
			SendPacket.GetInstance().SendObject(1021, gS_QUEST_GROUP_INIT_GM_REQ);
		}
	}

	private void OnQuestPass_Big(IUIObject obj)
	{
	}

	private void OnQuestInit_Big(IUIObject obj)
	{
	}

	private void OnQuestAllPass(IUIObject obj)
	{
	}

	private void OnQuestAllInit(IUIObject obj)
	{
	}

	private void OnOption(IUIObject obj)
	{
	}

	private void ChangeChapter(IUIObject obj)
	{
		ListItem listItem = this.m_DropDownList2.SelectedItem.Data as ListItem;
		this.m_CurrentChapter = (listItem.Key as QUEST_CHAPTER);
		Dictionary<int, CQuestGroup> hashQuestGroup = NrTSingleton<NkQuestManager>.Instance.GetHashQuestGroup();
		this.m_DropDownList3.Clear();
		foreach (CQuestGroup current in hashQuestGroup.Values)
		{
			if (this.m_CurrentChapter.i16QuestChapterUnique == current.GetChapterUnique())
			{
				ListItem listItem2 = new ListItem();
				listItem2.SetColumnStr(0, current.GetPageUnique().ToString());
				listItem2.Key = current;
				this.m_DropDownList3.Add(listItem2);
			}
		}
		this.m_DropDownList3.SetFirstItem();
		this.m_DropDownList3.RepositionItems();
		ListItem listItem3 = this.m_DropDownList3.SelectedItem.Data as ListItem;
		this.m_CurGroup = (listItem3.Key as CQuestGroup);
		if (this.m_CurGroup != null)
		{
			this.m_ListBox.Clear();
			foreach (QUEST_SORTID current2 in this.m_CurGroup.GetGroupInfo().m_QuestUnique.Values)
			{
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(current2.m_strQuestUnique);
				if (questByQuestUnique != null)
				{
					ListItem listItem4 = new ListItem();
					listItem4.SetColumnStr(0, questByQuestUnique.GetQuestTitle() + " " + questByQuestUnique.GetQuestUnique().ToString());
					listItem4.Key = questByQuestUnique;
					this.m_ListBox.Add(listItem4);
				}
			}
			this.m_ListBox.RepositionItems();
		}
	}

	private void ChangePage(IUIObject obj)
	{
		ListItem listItem = this.m_DropDownList3.SelectedItem.Data as ListItem;
		this.m_CurGroup = (listItem.Key as CQuestGroup);
		if (this.m_CurGroup != null)
		{
			this.m_ListBox.Clear();
			foreach (QUEST_SORTID current in this.m_CurGroup.GetGroupInfo().m_QuestUnique.Values)
			{
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(current.m_strQuestUnique);
				if (questByQuestUnique != null)
				{
					ListItem listItem2 = new ListItem();
					listItem2.SetColumnStr(0, questByQuestUnique.GetQuestTitle() + " " + questByQuestUnique.GetQuestUnique().ToString());
					listItem2.Key = questByQuestUnique;
					this.m_ListBox.Add(listItem2);
				}
			}
			this.m_ListBox.RepositionItems();
		}
	}

	private void ChangeNPC(IUIObject obj)
	{
	}
}
