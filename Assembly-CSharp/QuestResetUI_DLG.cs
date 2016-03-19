using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class QuestResetUI_DLG : Form
{
	private const int MAX_COUNT_PER_PAGE = 10;

	private List<CQuestGroup> m_QuestGroupList = new List<CQuestGroup>();

	private List<CQuestGroup> m_QuestSearchList = new List<CQuestGroup>();

	private Button m_Questreset_nextbutton;

	private Button m_Questreset_prebutton;

	private Button m_Questreset_reset;

	private Button m_Questreset_searchcancel;

	private DropDownList m_DropDownList_DropDownList49;

	private TextArea m_Questreset_searchbase;

	private ListBox m_Questreset_listbox;

	private Box m_Questreset_page;

	private ListItem[] m_LItems = new ListItem[10];

	private QUEST_CHAPTER m_CurChapter;

	private int m_i32CurPage = 1;

	private int m_i32TotalPage;

	private QuestGradeInfoUI_DLG m_QuestGradeInfoUI_DLG;

	private CQuestGroup m_CurQuestGroup;

	private string m_1202 = NrTSingleton<CTextParser>.Instance.GetTextColor("1202");

	private string m_1102 = NrTSingleton<CTextParser>.Instance.GetTextColor("1102");

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestReset/DLG_Questreset", G_ID.QUEST_RESET_DLG, true);
		base.SetLocation((GUICamera.width - base.GetSize().x) / 2f - 163f, (GUICamera.height - base.GetSize().y) / 2f);
		for (int i = 0; i < 10; i++)
		{
			this.m_LItems[i] = new ListItem();
		}
		Dictionary<int, CQuestGroup> hashQuestGroup = NrTSingleton<NkQuestManager>.Instance.GetHashQuestGroup();
		foreach (CQuestGroup current in hashQuestGroup.Values)
		{
			this.m_QuestGroupList.Add(current);
		}
		this.m_QuestGroupList.Sort(new Comparison<CQuestGroup>(QuestResetUI_DLG.AscendingNum));
	}

	public override void SetComponent()
	{
		this.m_Questreset_nextbutton = (base.GetControl("Questreset_nextbutton") as Button);
		this.m_Questreset_prebutton = (base.GetControl("Questreset_prebutton") as Button);
		this.m_Questreset_reset = (base.GetControl("Questreset_reset") as Button);
		this.m_DropDownList_DropDownList49 = (base.GetControl("DropDownList_DropDownList49") as DropDownList);
		this.m_Questreset_searchcancel = (base.GetControl("Questreset_searchcancel") as Button);
		this.m_Questreset_searchbase = (base.GetControl("Questreset_searchbase") as TextArea);
		this.m_Questreset_listbox = (base.GetControl("Questreset_listbox") as ListBox);
		this.m_Questreset_page = (base.GetControl("Questreset_page") as Box);
		Button expr_B6 = this.m_Questreset_nextbutton;
		expr_B6.Click = (EZValueChangedDelegate)Delegate.Combine(expr_B6.Click, new EZValueChangedDelegate(this.BtnNext));
		Button expr_DD = this.m_Questreset_prebutton;
		expr_DD.Click = (EZValueChangedDelegate)Delegate.Combine(expr_DD.Click, new EZValueChangedDelegate(this.BtnPre));
		Button expr_104 = this.m_Questreset_reset;
		expr_104.Click = (EZValueChangedDelegate)Delegate.Combine(expr_104.Click, new EZValueChangedDelegate(this.BtnReset));
		Button expr_12B = this.m_Questreset_searchcancel;
		expr_12B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_12B.Click, new EZValueChangedDelegate(this.BtnSearchCancle));
		this.m_Questreset_listbox.ColumnNum = 4;
		this.m_Questreset_listbox.UseColumnRect = true;
		this.m_Questreset_listbox.SetColumnRect(0, 6, 4, 60, 20, SpriteText.Anchor_Pos.Middle_Center, 15f);
		this.m_Questreset_listbox.SetColumnRect(1, 89, 4, 200, 20, SpriteText.Anchor_Pos.Middle_Left, 15f);
		this.m_Questreset_listbox.SetColumnRect(2, 327, 0, 92, 20, SpriteText.Anchor_Pos.Middle_Center, 14f);
		this.m_Questreset_listbox.SetColumnRect(3, 422, 5, 20, 20);
		this.m_Questreset_listbox.SetValueChangedDelegate(new EZValueChangedDelegate(this.SelectColum));
		this.m_Questreset_searchbase.SetValueChangedDelegate(new EZValueChangedDelegate(this.OnTextChaged));
		Dictionary<short, QUEST_CHAPTER> hashQuestChapter = NrTSingleton<NkQuestManager>.Instance.GetHashQuestChapter();
		if (hashQuestChapter != null)
		{
			this.m_DropDownList_DropDownList49.SetViewArea(3);
			this.m_DropDownList_DropDownList49.Clear();
			foreach (QUEST_CHAPTER current in hashQuestChapter.Values)
			{
				ListItem listItem = new ListItem();
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("667");
				string str = current.i16QuestChapterUnique.ToString() + textFromInterface;
				listItem.SetColumnStr(0, str);
				listItem.Key = current;
				this.m_DropDownList_DropDownList49.Add(listItem);
			}
			this.m_DropDownList_DropDownList49.RepositionItems();
			this.m_DropDownList_DropDownList49.SetFirstItem();
		}
		ListItem listItem2 = this.m_DropDownList_DropDownList49.SelectedItem.Data as ListItem;
		this.m_CurChapter = (listItem2.Key as QUEST_CHAPTER);
		this.m_DropDownList_DropDownList49.AddValueChangedDelegate(new EZValueChangedDelegate(this.DropDownChapter));
		this.m_QuestSearchList.Clear();
		foreach (CQuestGroup current2 in this.m_QuestGroupList)
		{
			if (this.m_CurChapter.i16QuestChapterUnique == current2.GetChapterUnique())
			{
				this.m_QuestSearchList.Add(current2);
			}
		}
		this.m_QuestSearchList.Sort(new Comparison<CQuestGroup>(QuestResetUI_DLG.AscendingNum));
		this.SetCurrentPage();
		this.ShowList();
	}

	private void ShowList()
	{
		int num = 0;
		int num2 = 0;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("666");
		this.m_Questreset_listbox.Clear();
		foreach (CQuestGroup current in this.m_QuestSearchList)
		{
			if (10 < num2)
			{
				break;
			}
			if (num >= (this.m_i32CurPage - 1) * 10 && num < this.m_i32CurPage * 10)
			{
				string str = current.GetPage() + textFromInterface;
				if (current.GetQuestType() == 2)
				{
					str = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Title("sidestory");
				}
				if (NrTSingleton<NkQuestManager>.Instance.QuestGroupClearCheck(current.GetGroupUnique()) == QUEST_CONST.E_QUEST_GROUP_STATE.E_QUEST_GROUP_STATE_NONE)
				{
					this.m_LItems[num2].SetColumnGUIContent(0, this.m_1202 + str);
					this.m_LItems[num2].SetColumnGUIContent(1, this.m_1202 + current.GetGroupTitle());
				}
				else
				{
					this.m_LItems[num2].SetColumnGUIContent(0, this.m_1102 + str);
					this.m_LItems[num2].SetColumnGUIContent(1, this.m_1102 + current.GetGroupTitle());
				}
				this.m_LItems[num2].SetColumnGUIContent(3, string.Empty, "Com_I_MoneyGold");
				this.m_LItems[num2].Key = current;
				this.m_Questreset_listbox.Add(this.m_LItems[num2]);
				num2++;
			}
			num++;
		}
		this.m_Questreset_listbox.RepositionItems();
	}

	private void SetCurrentPage()
	{
		this.m_i32TotalPage = this.m_QuestSearchList.Count / 10;
		this.m_i32TotalPage++;
		if (this.m_i32TotalPage <= 0)
		{
			this.m_i32TotalPage = 1;
		}
		this.m_Questreset_page.Text = this.m_i32CurPage.ToString() + "/" + this.m_i32TotalPage.ToString();
	}

	private void DropDownChapter(IUIObject obj)
	{
		ListItem listItem = this.m_DropDownList_DropDownList49.SelectedItem.Data as ListItem;
		this.m_CurChapter = (QUEST_CHAPTER)listItem.Key;
		this.m_QuestSearchList.Clear();
		foreach (CQuestGroup current in this.m_QuestGroupList)
		{
			if (this.m_CurChapter.i16QuestChapterUnique == current.GetChapterUnique())
			{
				this.m_QuestSearchList.Add(current);
			}
		}
		this.m_QuestSearchList.Sort(new Comparison<CQuestGroup>(QuestResetUI_DLG.AscendingNum));
		this.m_i32CurPage = 1;
		this.SetCurrentPage();
		this.ShowList();
	}

	private void OnTextChaged(IUIObject obj)
	{
		this.m_QuestSearchList.Clear();
		this.m_i32CurPage = 1;
		if (this.m_Questreset_searchbase.Text == string.Empty)
		{
			foreach (CQuestGroup current in this.m_QuestGroupList)
			{
				if (this.m_CurChapter.i16QuestChapterUnique == current.GetChapterUnique())
				{
					this.m_QuestSearchList.Add(current);
				}
			}
		}
		else
		{
			for (int i = 0; i < this.m_QuestGroupList.Count; i++)
			{
				if (InitialSearch.IsCheckString(this.m_QuestGroupList[i].GetGroupTitle(), this.m_Questreset_searchbase.Text))
				{
					this.m_QuestSearchList.Add(this.m_QuestGroupList[i]);
				}
			}
		}
		this.m_QuestSearchList.Sort(new Comparison<CQuestGroup>(QuestResetUI_DLG.AscendingNum));
		this.SetCurrentPage();
		this.ShowList();
	}

	private void BtnNext(IUIObject obj)
	{
		this.m_i32CurPage++;
		if (this.m_i32TotalPage < this.m_i32CurPage)
		{
			this.m_i32CurPage = this.m_i32TotalPage;
		}
		this.SetCurrentPage();
		this.ShowList();
	}

	private void BtnPre(IUIObject obj)
	{
		this.m_i32CurPage--;
		if (0 >= this.m_i32CurPage)
		{
			this.m_i32CurPage = 1;
		}
		this.SetCurrentPage();
		this.ShowList();
	}

	private void BtnSearchCancle(IUIObject obj)
	{
		this.m_Questreset_searchbase.Text = string.Empty;
		this.OnTextChaged(null);
	}

	private void BtnReset(IUIObject obj)
	{
		if (this.m_CurQuestGroup != null)
		{
			QUEST_CONST.E_QUEST_GROUP_STATE e_QUEST_GROUP_STATE = NrTSingleton<NkQuestManager>.Instance.QuestGroupClearCheck(this.m_CurQuestGroup.GetGroupUnique());
			if (e_QUEST_GROUP_STATE != QUEST_CONST.E_QUEST_GROUP_STATE.E_QUEST_GROUP_STATE_NONE)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("103");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo != null)
			{
			}
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("658");
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("63");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromMessageBox,
				"Targetname1",
				this.m_CurQuestGroup.GetGroupTitle()
			});
			msgBoxUI.SetMsg(new YesDelegate(this.On_Ok), this.m_CurQuestGroup, textFromInterface, empty, eMsgType.MB_OK_CANCEL);
		}
	}

	private void SelectColum(IUIObject obj)
	{
		ListBox listBox = (ListBox)obj;
		UIListItemContainer selectedItem = listBox.SelectedItem;
		this.m_CurQuestGroup = (CQuestGroup)selectedItem.Data;
		this.m_QuestGradeInfoUI_DLG = (base.SetChildForm(G_ID.QUEST_GRADE_INFO_DLG) as QuestGradeInfoUI_DLG);
		if (this.m_QuestGradeInfoUI_DLG != null && this.m_CurQuestGroup != null)
		{
			this.m_QuestGradeInfoUI_DLG.SetGroupInfo(this.m_CurQuestGroup);
			this.m_QuestGradeInfoUI_DLG.Show();
		}
	}

	private void On_Ok(object a_oObject)
	{
		CQuestGroup cQuestGroup = a_oObject as CQuestGroup;
		GS_QUEST_GROUP_RESET_REQ gS_QUEST_GROUP_RESET_REQ = new GS_QUEST_GROUP_RESET_REQ();
		gS_QUEST_GROUP_RESET_REQ.i32GroupUnique = cQuestGroup.GetGroupUnique();
		SendPacket.GetInstance().SendObject(1015, gS_QUEST_GROUP_RESET_REQ);
	}

	private static int AscendingNum(CQuestGroup x, CQuestGroup y)
	{
		if (x.GetGroupSort() >= y.GetGroupSort())
		{
			return 1;
		}
		return -1;
	}

	private static int DescendingNum(CQuestGroup x, CQuestGroup y)
	{
		if (x.GetGroupSort() < y.GetGroupSort())
		{
			return 1;
		}
		return -1;
	}
}
