using System;
using System.Collections.Generic;
using UnityForms;

public class QuestList_DLG : Form
{
	private class GRADE_TEMP
	{
		public CQuest kQuest;

		public int i32Grade;
	}

	private DropDownList m_Quest_Info_List_Option_Ongoing;

	private TreeView m_Quest_Info_List_TreeView;

	private Toolbar m_Quest_Info_ToolBar;

	private TreeView m_Quest_Info_completeTreeview;

	private Label m_Quest_Info_progressPercent;

	private ProgressBar m_Quest_Info_progressBar;

	private Button m_Quest_StoryView;

	private Button m_Quest_DramaView;

	private TreeView.TreeNode m_OngoingNode;

	private TreeView.TreeNode m_AcceptableNode;

	private TreeView.TreeNode m_DayQuestNode;

	private QUESTLIST_TAB_MODE m_eMode;

	private List<CQuest> m_QuestList = new List<CQuest>();

	private List<CQuestGroup> m_QuesGroupList = new List<CQuestGroup>();

	private Dictionary<short, TreeView.TreeNode> m_ChapterTree = new Dictionary<short, TreeView.TreeNode>();

	private Dictionary<int, TreeView.TreeNode> m_PageTree = new Dictionary<int, TreeView.TreeNode>();

	private string m_str1202 = string.Empty;

	private string m_str1205 = string.Empty;

	private string m_str1206 = string.Empty;

	private string m_str1102 = string.Empty;

	private QuestListInfo_DLG m_QuestListInfo;

	private QuestList_ChapterInfo_GameDramaView_DLG m_DramaInfo;

	private QUEST_CONST.E_QUEST_TYPE m_CurQuestType = QUEST_CONST.E_QUEST_TYPE.E_QUEST_TYPE_MAIN;

	private CQuest m_CurQuest;

	private CQuestGroup m_CurQuestGroup;

	private bool m_bCloseDlg;

	public bool CloseDlg
	{
		get
		{
			return this.m_bCloseDlg;
		}
		set
		{
			this.m_bCloseDlg = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestList/DLG_QuestList", G_ID.QUESTLIST_DLG, true);
		base.SetLocation((GUICamera.width - base.GetSize().x) / 2f - base.GetSize().x / 2f, (GUICamera.height - base.GetSize().y) / 2f);
		NrTSingleton<NkQuestManager>.Instance.SetQuestList(ref this.m_QuestList);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_Quest_Info_List_Option_Ongoing = (base.GetControl("Quest_Info_List_Option_Ongoing") as DropDownList);
		this.m_Quest_Info_List_TreeView = (base.GetControl("Quest_Info_List_TreeView") as TreeView);
		this.m_Quest_Info_ToolBar = (base.GetControl("Quest_Info_ToolBar") as Toolbar);
		this.m_Quest_Info_completeTreeview = (base.GetControl("Quest_Info_completeTreeview") as TreeView);
		this.m_Quest_Info_progressBar = (base.GetControl("Quest_Info_progressBar") as ProgressBar);
		this.m_Quest_StoryView = (base.GetControl("Quest_StoryView") as Button);
		this.m_Quest_StoryView.Visible = false;
		this.m_Quest_DramaView = (base.GetControl("Quest_DramaView") as Button);
		this.m_Quest_DramaView.Visible = false;
		this.m_Quest_Info_progressPercent = (base.GetControl("Quest_Info_progressPercent") as Label);
		Button expr_CE = this.m_Quest_StoryView;
		expr_CE.Click = (EZValueChangedDelegate)Delegate.Combine(expr_CE.Click, new EZValueChangedDelegate(this.OnStoryView));
		this.m_Quest_Info_ToolBar.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1227");
		this.m_Quest_Info_ToolBar.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1228");
		this.m_Quest_Info_ToolBar.FirstSetting();
		UIPanelTab expr_149 = this.m_Quest_Info_ToolBar.Control_Tab[0];
		expr_149.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_149.ButtonClick, new EZValueChangedDelegate(this.OnTabClick));
		UIPanelTab expr_177 = this.m_Quest_Info_ToolBar.Control_Tab[1];
		expr_177.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_177.ButtonClick, new EZValueChangedDelegate(this.OnTabClick));
		this.m_Quest_Info_List_TreeView.LineHeight = 28f;
		this.m_Quest_Info_List_TreeView.ChildStartX = 16f;
		this.m_Quest_Info_List_TreeView.InitTreeData();
		this.m_Quest_Info_List_TreeView.SetValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickQuestList));
		this.m_Quest_Info_List_TreeView.ParentAlignment = SpriteText.Alignment_Type.Left;
		this.m_Quest_Info_List_TreeView.ChildAlignment = SpriteText.Alignment_Type.Left;
		this.m_Quest_Info_List_TreeView.UseDepthCount = true;
		this.m_Quest_Info_List_TreeView.SetChildDepthOption(0, 15, SpriteText.Font_Effect.Black_Shadow_Small, 0, 0, 0, 0, 0, 0);
		this.m_Quest_Info_List_TreeView.SetChildDepthOption(1, 15, SpriteText.Font_Effect.Black_Shadow_Small, 43, 257, 315, 202, 47, 120);
		this.m_Quest_Info_List_TreeView.SetChildrenAlignment(SpriteText.Alignment_Type.Left, SpriteText.Alignment_Type.Center, SpriteText.Alignment_Type.Center);
		this.m_str1202 = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");
		this.m_str1205 = NrTSingleton<CTextParser>.Instance.GetTextColor("1205");
		this.m_str1206 = NrTSingleton<CTextParser>.Instance.GetTextColor("1206");
		this.m_str1102 = NrTSingleton<CTextParser>.Instance.GetTextColor("1102");
		this.m_Quest_Info_completeTreeview.LineHeight = 28f;
		this.m_Quest_Info_completeTreeview.InitTreeData();
		this.m_Quest_Info_completeTreeview.SetValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickQuestList));
		this.m_Quest_Info_completeTreeview.UseDepthCount = true;
		this.m_Quest_Info_completeTreeview.SetChildDepthOption(0, 18, SpriteText.Font_Effect.Black_Shadow_Small, 0, 0, 0, 0, 0, 0);
		this.m_Quest_Info_completeTreeview.SetChildDepthOption(1, 18, SpriteText.Font_Effect.Black_Shadow_Small, 0, 0, 0, 0, 0, 0);
		this.m_Quest_Info_completeTreeview.SetChildDepthOption(2, 18, SpriteText.Font_Effect.Black_Shadow_Small, 0, 0, 0, 0, 0, 0);
		Dictionary<int, CQuestGroup> hashQuestGroup = NrTSingleton<NkQuestManager>.Instance.GetHashQuestGroup();
		foreach (CQuestGroup current in hashQuestGroup.Values)
		{
			this.m_QuesGroupList.Add(current);
		}
		this.m_QuesGroupList.Sort(new Comparison<CQuestGroup>(QuestList_DLG.AscendingNum));
		ListItem listItem = new ListItem();
		listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("787"));
		listItem.Key = QUEST_CONST.E_QUEST_TYPE.E_QUEST_TYPE_MAIN;
		ListItem listItem2 = new ListItem();
		listItem2.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("783"));
		listItem2.Key = QUEST_CONST.E_QUEST_TYPE.E_QUEST_TYPE_SUB;
		ListItem listItem3 = new ListItem();
		listItem3.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1635"));
		listItem3.Key = QUEST_CONST.E_QUEST_TYPE.E_QUEST_TYPE_DAY;
		this.m_Quest_Info_List_Option_Ongoing.SetViewArea(3);
		this.m_Quest_Info_List_Option_Ongoing.Clear();
		this.m_Quest_Info_List_Option_Ongoing.Add(listItem);
		this.m_Quest_Info_List_Option_Ongoing.Add(listItem2);
		this.m_Quest_Info_List_Option_Ongoing.Add(listItem3);
		this.m_Quest_Info_List_Option_Ongoing.RepositionItems();
		this.m_Quest_Info_List_Option_Ongoing.SetFirstItem();
		this.m_Quest_Info_List_Option_Ongoing.AddValueChangedDelegate(new EZValueChangedDelegate(this.DropDownList_OptionChange));
		this.SetOnGoing();
		this.m_Quest_Info_List_TreeView.SetSelectedItem(0);
		this.m_Quest_Info_List_TreeView.ClickButton(null);
		this.m_Quest_Info_List_TreeView.ExpandNode(this.m_OngoingNode, 0);
		this.SetTotal();
		this.ShowOnGoing();
		NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(140, 0L, 1L);
	}

	private void OnStoryView(IUIObject obj)
	{
		if (this.m_CurQuestGroup == null)
		{
			return;
		}
		this.m_DramaInfo = (base.SetChildForm(G_ID.QUESTLIST_CHAPTERINFO_GAMEDRAMAVIEW_DLG) as QuestList_ChapterInfo_GameDramaView_DLG);
		if (this.m_DramaInfo != null)
		{
			this.m_DramaInfo.SetQuest(this.m_CurQuestGroup, QUESTLIST_TAB_MODE2.QuestSummary);
			this.m_DramaInfo.Show();
			if (this.m_QuestListInfo != null)
			{
				this.m_QuestListInfo.Close();
			}
		}
	}

	private void OnDramaView(IUIObject obj)
	{
		if (this.m_CurQuestGroup == null)
		{
			return;
		}
		this.m_DramaInfo = (base.SetChildForm(G_ID.QUESTLIST_CHAPTERINFO_GAMEDRAMAVIEW_DLG) as QuestList_ChapterInfo_GameDramaView_DLG);
		if (this.m_DramaInfo != null)
		{
			this.m_DramaInfo.SetQuest(this.m_CurQuestGroup, QUESTLIST_TAB_MODE2.Drama);
			this.m_DramaInfo.Show();
			if (this.m_QuestListInfo != null)
			{
				this.m_QuestListInfo.Close();
			}
		}
	}

	private static int AscendingNum(CQuestGroup x, CQuestGroup y)
	{
		if (x.GetGroupSort() >= y.GetGroupSort())
		{
			return 1;
		}
		return -1;
	}

	private static int AscendingQuest(QuestList_DLG.GRADE_TEMP x, QuestList_DLG.GRADE_TEMP y)
	{
		if (x.i32Grade <= 0)
		{
			x.i32Grade = 1;
		}
		if (y.i32Grade <= 0)
		{
			y.i32Grade = 1;
		}
		if (x.kQuest.GetQuestCommon().i16RequireLevel[x.i32Grade - 1] >= y.kQuest.GetQuestCommon().i16RequireLevel[y.i32Grade - 1])
		{
			return 1;
		}
		return -1;
	}

	private void OnTabClick(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == (int)this.m_eMode)
		{
			return;
		}
		this.m_eMode = (QUESTLIST_TAB_MODE)uIPanelTab.panel.index;
		int index = uIPanelTab.panel.index;
		if (index != 0)
		{
			if (index == 1)
			{
				this.Showtotal();
				this.m_Quest_StoryView.Visible = false;
				this.m_Quest_DramaView.Visible = true;
			}
		}
		else
		{
			this.ShowOnGoing();
			this.m_Quest_StoryView.Visible = false;
			this.m_Quest_DramaView.Visible = false;
		}
	}

	private void SetOngoingRootNode()
	{
		this.m_Quest_Info_List_TreeView.InitTreeData();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1200");
		string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1201");
		string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1636");
		TREE_TYPE tREE_TYPE = new TREE_TYPE();
		TREE_TYPE tREE_TYPE2 = new TREE_TYPE();
		TREE_TYPE tREE_TYPE3 = new TREE_TYPE();
		tREE_TYPE.bType = 0;
		tREE_TYPE2.bType = 0;
		tREE_TYPE3.bType = 0;
		this.m_OngoingNode = this.m_Quest_Info_List_TreeView.InsertChildRoot(NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + textFromInterface, tREE_TYPE, true);
		this.m_AcceptableNode = this.m_Quest_Info_List_TreeView.InsertChildRoot(NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + textFromInterface2, tREE_TYPE2, true);
		this.m_DayQuestNode = this.m_Quest_Info_List_TreeView.InsertChildRoot(NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + textFromInterface3, tREE_TYPE3, true);
		this.m_Quest_Info_List_TreeView.RepositionItems();
	}

	public void ShowOnGoing()
	{
		base.ShowLayer(1);
		if (this.m_Quest_Info_List_TreeView.slider)
		{
			this.m_Quest_Info_List_TreeView.slider.Visible = false;
			this.m_Quest_Info_List_TreeView.RepositionItems();
		}
	}

	public void Showtotal()
	{
		base.ShowLayer(2);
		if (this.m_Quest_Info_completeTreeview.slider)
		{
			this.m_Quest_Info_completeTreeview.slider.Visible = false;
			this.m_Quest_Info_completeTreeview.RepositionItems();
		}
	}

	public void SetOnGoing()
	{
		this.SetOngoingRootNode();
		List<QuestList_DLG.GRADE_TEMP> list = new List<QuestList_DLG.GRADE_TEMP>();
		List<QuestList_DLG.GRADE_TEMP> list2 = new List<QuestList_DLG.GRADE_TEMP>();
		USER_CURRENT_QUEST_INFO[] userCurrentQuestInfo = NrTSingleton<NkQuestManager>.Instance.GetUserCurrentQuestInfo();
		for (byte b = 0; b < 10; b += 1)
		{
			if (userCurrentQuestInfo[(int)b].strQuestUnique != null)
			{
				string strQuestUnique = userCurrentQuestInfo[(int)b].strQuestUnique;
				if (strQuestUnique != string.Empty)
				{
					CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
					if (questByQuestUnique != null)
					{
						CQuestGroup questGroupByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(strQuestUnique);
						if (questGroupByQuestUnique != null)
						{
							QuestList_DLG.GRADE_TEMP gRADE_TEMP = new QuestList_DLG.GRADE_TEMP();
							gRADE_TEMP.kQuest = questByQuestUnique;
							gRADE_TEMP.i32Grade = userCurrentQuestInfo[(int)b].i32Grade;
							if (this.m_CurQuestType == (QUEST_CONST.E_QUEST_TYPE)questGroupByQuestUnique.GetQuestType())
							{
								list.Add(gRADE_TEMP);
							}
							else
							{
								list2.Add(gRADE_TEMP);
							}
						}
					}
				}
			}
		}
		list.Sort(new Comparison<QuestList_DLG.GRADE_TEMP>(QuestList_DLG.AscendingQuest));
		list2.Sort(new Comparison<QuestList_DLG.GRADE_TEMP>(QuestList_DLG.AscendingQuest));
		foreach (QuestList_DLG.GRADE_TEMP current in list)
		{
			TREE_TYPE tREE_TYPE = new TREE_TYPE();
			tREE_TYPE.bType = 1;
			tREE_TYPE.bData = current.kQuest;
			this.m_OngoingNode.AddChild(1, this.m_str1202 + current.kQuest.GetQuestTitle(), this.m_str1202 + current.kQuest.GetQuestCommon().i16RequireLevel[current.i32Grade].ToString(), this.m_str1202 + current.kQuest.GetQuestNpcName(), tREE_TYPE, "Bat_I_Minimap1");
		}
		foreach (QuestList_DLG.GRADE_TEMP current2 in list2)
		{
			TREE_TYPE tREE_TYPE2 = new TREE_TYPE();
			tREE_TYPE2.bType = 1;
			tREE_TYPE2.bData = current2.kQuest;
			this.m_OngoingNode.AddChild(1, this.m_str1202 + current2.kQuest.GetQuestTitle(), this.m_str1202 + current2.kQuest.GetQuestCommon().i16RequireLevel[current2.i32Grade].ToString(), this.m_str1202 + current2.kQuest.GetQuestNpcName(), tREE_TYPE2);
		}
		list.Clear();
		list2.Clear();
		foreach (CQuest current3 in this.m_QuestList)
		{
			if (NrTSingleton<NkQuestManager>.Instance.GetQuestState(current3.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
			{
				CQuestGroup questGroupByQuestUnique2 = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(current3.GetQuestUnique());
				if (questGroupByQuestUnique2 != null)
				{
					if (questGroupByQuestUnique2.GetQuestType() != 100)
					{
						QuestList_DLG.GRADE_TEMP gRADE_TEMP2 = new QuestList_DLG.GRADE_TEMP();
						gRADE_TEMP2.kQuest = current3;
						USER_QUEST_COMPLETE_INFO completeQuestInfo = NrTSingleton<NkQuestManager>.Instance.GetCompleteQuestInfo(questGroupByQuestUnique2.GetGroupUnique());
						if (completeQuestInfo != null)
						{
							gRADE_TEMP2.i32Grade = (int)completeQuestInfo.bCurrentGrade;
						}
						else
						{
							gRADE_TEMP2.i32Grade = 1;
						}
						if (this.m_CurQuestType == (QUEST_CONST.E_QUEST_TYPE)questGroupByQuestUnique2.GetQuestType())
						{
							list.Add(gRADE_TEMP2);
						}
						else
						{
							list2.Add(gRADE_TEMP2);
						}
					}
				}
			}
		}
		list.Sort(new Comparison<QuestList_DLG.GRADE_TEMP>(QuestList_DLG.AscendingQuest));
		list2.Sort(new Comparison<QuestList_DLG.GRADE_TEMP>(QuestList_DLG.AscendingQuest));
		foreach (QuestList_DLG.GRADE_TEMP current4 in list)
		{
			TREE_TYPE tREE_TYPE3 = new TREE_TYPE();
			tREE_TYPE3.bType = 1;
			tREE_TYPE3.bData = current4.kQuest;
			this.m_AcceptableNode.AddChild(1, this.m_str1202 + current4.kQuest.GetQuestTitle(), this.m_str1202 + current4.kQuest.GetQuestCommon().i16RequireLevel[current4.i32Grade - 1].ToString(), this.m_str1202 + current4.kQuest.GetQuestNpcName(), tREE_TYPE3, "Bat_I_Minimap1");
		}
		foreach (QuestList_DLG.GRADE_TEMP current5 in list2)
		{
			TREE_TYPE tREE_TYPE4 = new TREE_TYPE();
			tREE_TYPE4.bType = 1;
			tREE_TYPE4.bData = current5.kQuest;
			this.m_AcceptableNode.AddChild(1, this.m_str1202 + current5.kQuest.GetQuestTitle(), this.m_str1202 + current5.kQuest.GetQuestCommon().i16RequireLevel[current5.i32Grade - 1].ToString(), this.m_str1202 + current5.kQuest.GetQuestNpcName(), tREE_TYPE4);
		}
		foreach (CQuest current6 in this.m_QuestList)
		{
			if (NrTSingleton<NkQuestManager>.Instance.GetQuestState(current6.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
			{
				CQuestGroup questGroupByQuestUnique3 = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(current6.GetQuestUnique());
				if (questGroupByQuestUnique3 != null)
				{
					if (questGroupByQuestUnique3.GetQuestType() == 100)
					{
						QuestList_DLG.GRADE_TEMP gRADE_TEMP3 = new QuestList_DLG.GRADE_TEMP();
						gRADE_TEMP3.kQuest = current6;
						USER_QUEST_COMPLETE_INFO completeQuestInfo2 = NrTSingleton<NkQuestManager>.Instance.GetCompleteQuestInfo(questGroupByQuestUnique3.GetGroupUnique());
						if (completeQuestInfo2 != null)
						{
							gRADE_TEMP3.i32Grade = (int)completeQuestInfo2.bCurrentGrade;
						}
						else
						{
							gRADE_TEMP3.i32Grade = 1;
						}
						TREE_TYPE tREE_TYPE5 = new TREE_TYPE();
						tREE_TYPE5.bType = 1;
						tREE_TYPE5.bData = current6;
						this.m_DayQuestNode.AddChild(1, this.m_str1202 + current6.GetQuestTitle(), this.m_str1202 + current6.GetQuestCommon().i16RequireLevel[gRADE_TEMP3.i32Grade].ToString(), this.m_str1202 + current6.GetQuestNpcName(), tREE_TYPE5);
					}
				}
			}
		}
		this.m_Quest_Info_List_TreeView.RepositionItems();
	}

	public void SetTotal()
	{
		string str = string.Empty;
		this.m_Quest_Info_completeTreeview.ClearList(true);
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("666");
		Dictionary<short, QUEST_CHAPTER> hashQuestChapter = NrTSingleton<NkQuestManager>.Instance.GetHashQuestChapter();
		this.m_ChapterTree.Clear();
		if (hashQuestChapter != null)
		{
			foreach (QUEST_CHAPTER current in hashQuestChapter.Values)
			{
				if (this.m_CurQuestType == QUEST_CONST.E_QUEST_TYPE.E_QUEST_TYPE_SUB || this.m_CurQuestType == QUEST_CONST.E_QUEST_TYPE.E_QUEST_TYPE_DAY)
				{
					bool flag = false;
					foreach (CQuestGroup current2 in this.m_QuesGroupList)
					{
						if (current.i16QuestChapterUnique == current2.GetChapterUnique() && this.m_CurQuestType == (QUEST_CONST.E_QUEST_TYPE)current2.GetQuestType())
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						continue;
					}
				}
				TREE_TYPE tREE_TYPE = new TREE_TYPE();
				tREE_TYPE.bType = 0;
				tREE_TYPE.bData = current;
				string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("667");
				string textFromQuest_Title = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Title(current.strChapterTextKey);
				string text = current.i16QuestChapterUnique.ToString() + textFromInterface2 + ". " + textFromQuest_Title;
				text = this.m_str1206 + text;
				TreeView.TreeNode value = this.m_Quest_Info_completeTreeview.InsertChildRoot(text, tREE_TYPE, true);
				this.m_ChapterTree.Add(current.i16QuestChapterUnique, value);
			}
		}
		this.m_PageTree.Clear();
		foreach (CQuestGroup current3 in this.m_QuesGroupList)
		{
			if (this.m_CurQuestType == (QUEST_CONST.E_QUEST_TYPE)current3.GetQuestType() || this.m_CurQuestType == QUEST_CONST.E_QUEST_TYPE.E_QUEST_TYPE_TOTAL)
			{
				if (NrTSingleton<NkQuestManager>.Instance.QuestGroupClearCheck(current3.GetGroupUnique()) == QUEST_CONST.E_QUEST_GROUP_STATE.E_QUEST_GROUP_STATE_NONE)
				{
					this.m_str1205 = NrTSingleton<CTextParser>.Instance.GetTextColor("1205");
				}
				else
				{
					this.m_str1205 = NrTSingleton<CTextParser>.Instance.GetTextColor("1102");
				}
				TREE_TYPE tREE_TYPE2 = new TREE_TYPE();
				tREE_TYPE2.bType = 1;
				tREE_TYPE2.bData = current3;
				str = string.Concat(new string[]
				{
					this.m_str1205,
					current3.GetPage(),
					textFromInterface,
					". ",
					current3.GetGroupTitle()
				});
				if (this.m_CurQuestType == QUEST_CONST.E_QUEST_TYPE.E_QUEST_TYPE_SUB)
				{
					str = this.m_str1205 + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1454") + ". " + current3.GetGroupTitle();
				}
				else if (this.m_CurQuestType == QUEST_CONST.E_QUEST_TYPE.E_QUEST_TYPE_DAY)
				{
					str = this.m_str1205 + current3.GetGroupTitle();
				}
				if (this.m_ChapterTree.ContainsKey(current3.GetChapterUnique()))
				{
					TreeView.TreeNode treeNode = this.m_ChapterTree[current3.GetChapterUnique()];
					if (treeNode != null)
					{
						TreeView.TreeNode treeNode2 = treeNode.AddChild(1, str, tREE_TYPE2);
						this.m_PageTree.Add(current3.GetGroupUnique(), treeNode2);
						foreach (QUEST_SORTID current4 in current3.GetGroupInfo().m_QuestList)
						{
							CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(current4.m_strQuestUnique);
							if (questByQuestUnique != null)
							{
								TREE_TYPE tREE_TYPE3 = new TREE_TYPE();
								tREE_TYPE3.bType = 2;
								tREE_TYPE3.bData = questByQuestUnique;
								string str2 = string.Empty;
								if (NrTSingleton<NkQuestManager>.Instance.GetQuestState(current4.m_strQuestUnique) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE)
								{
									str2 = NrTSingleton<UIDataManager>.Instance.GetString(this.m_str1202, "- ", questByQuestUnique.GetQuestTitle());
								}
								else
								{
									str2 = NrTSingleton<UIDataManager>.Instance.GetString(this.m_str1102, "- ", questByQuestUnique.GetQuestTitle());
								}
								treeNode2.AddChild(2, str2, tREE_TYPE3);
							}
						}
					}
				}
			}
		}
		this.m_Quest_Info_completeTreeview.RepositionItems();
		this.SetProgress();
	}

	private void SetProgress()
	{
		float num = 0f;
		foreach (CQuest current in this.m_QuestList)
		{
			if (NrTSingleton<NkQuestManager>.Instance.GetQuestState(current.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE)
			{
				num += 1f;
			}
		}
		this.m_Quest_Info_progressBar.Value = num / (float)this.m_QuestList.Count;
		float num2 = num / (float)this.m_QuestList.Count * 100f;
		this.m_Quest_Info_progressPercent.Text = num2.ToString("##0.#") + "%";
	}

	private void BtnClickQuestList(IUIObject obj)
	{
		TreeView treeView = (TreeView)obj;
		int index = treeView.SelectedItem.GetIndex();
		UIListItemContainer selectedItem = treeView.SelectedItem;
		TreeView.TreeNode treeNode = (TreeView.TreeNode)selectedItem.Data;
		TREE_TYPE tREE_TYPE = treeNode.ObjectData as TREE_TYPE;
		if (tREE_TYPE.bType == 0)
		{
			treeView.ExpandNode(treeNode, index);
		}
		else if (tREE_TYPE.bType == 1)
		{
			if (this.m_eMode == QUESTLIST_TAB_MODE.Total)
			{
				this.m_CurQuestGroup = (CQuestGroup)tREE_TYPE.bData;
			}
			else if (this.m_eMode == QUESTLIST_TAB_MODE.OnGoing)
			{
				this.m_CurQuest = (CQuest)tREE_TYPE.bData;
				this.m_CurQuestGroup = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(this.m_CurQuest.GetQuestUnique());
				this.m_QuestListInfo = (base.SetChildForm(G_ID.QUESTLISTINFO_DLG) as QuestListInfo_DLG);
				this.m_QuestListInfo.SetQuestInfo((CQuest)tREE_TYPE.bData);
				this.m_QuestListInfo.Show();
				if (this.m_DramaInfo != null)
				{
					this.m_DramaInfo.Close();
				}
			}
			treeView.ExpandNode(treeNode, index);
		}
		else if (tREE_TYPE.bType == 2 && this.m_eMode == QUESTLIST_TAB_MODE.Total)
		{
			this.m_CurQuest = (CQuest)tREE_TYPE.bData;
			this.m_CurQuestGroup = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(this.m_CurQuest.GetQuestUnique());
			this.m_QuestListInfo = (base.SetChildForm(G_ID.QUESTLISTINFO_DLG) as QuestListInfo_DLG);
			this.m_QuestListInfo.SetQuestInfo((CQuest)tREE_TYPE.bData);
			this.m_QuestListInfo.Show();
			if (this.m_DramaInfo != null)
			{
				this.m_DramaInfo.Close();
			}
		}
	}

	private void DropDownList_OptionChange(IUIObject obj)
	{
		if (null == this.m_Quest_Info_List_Option_Ongoing.SelectedItem)
		{
			return;
		}
		ListItem listItem = this.m_Quest_Info_List_Option_Ongoing.SelectedItem.Data as ListItem;
		this.m_CurQuestType = (QUEST_CONST.E_QUEST_TYPE)((int)listItem.Key);
		this.SetTotal();
		this.Showtotal();
		if (this.m_CurQuestType == QUEST_CONST.E_QUEST_TYPE.E_QUEST_TYPE_MAIN)
		{
			this.m_Quest_DramaView.Visible = true;
			this.m_Quest_StoryView.Visible = false;
		}
		else
		{
			this.m_Quest_StoryView.Visible = false;
			this.m_Quest_DramaView.Visible = false;
		}
	}

	public void ExpandTree()
	{
		this.m_Quest_Info_List_TreeView.ExpandNode(this.m_OngoingNode, 0);
	}

	public override void Update()
	{
		if (this.CloseDlg)
		{
			this.Close();
		}
	}
}
