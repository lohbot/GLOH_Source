using GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class QuestItemSel_Dlg : Form
{
	private const int N_LINE_MAX = 7;

	private Button m_Button_Button5;

	private Button m_Button_Button6;

	private Label m_Label_Page;

	private ListBox m_ListBox_ListBox37;

	private List<ITEM> m_ItemList;

	private int m_nCurrentPage = 1;

	private int m_nMaxPage = 1;

	private G_ID m_gid;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NpcTalk/DLG_Select_Item", G_ID.QUESTITEM_SEL_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_Button_Button5 = (base.GetControl("Button_Button5") as Button);
		this.m_Button_Button6 = (base.GetControl("Button_Button6") as Button);
		this.m_Label_Page = (base.GetControl("Label_Page") as Label);
		this.m_ListBox_ListBox37 = (base.GetControl("ListBox_ListBox37") as ListBox);
		this.m_ListBox_ListBox37.itemSpacing = 4f;
		this.m_ListBox_ListBox37.ColumnNum = 3;
		this.m_ListBox_ListBox37.LineHeight = 52f;
		this.m_ListBox_ListBox37.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickList));
		this.m_ListBox_ListBox37.UseColumnRect = true;
		this.m_ListBox_ListBox37.SetColumnRect(0, 7, 6, 41, 41);
		this.m_ListBox_ListBox37.SetColumnRect(1, 7, 6, 40, 40);
		this.m_ListBox_ListBox37.SetColumnRect(2, 59, 16, 226, 20, SpriteText.Anchor_Pos.Middle_Left);
		Button expr_E9 = this.m_Button_Button5;
		expr_E9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E9.Click, new EZValueChangedDelegate(this.On_Prev_Page_Button));
		Button expr_110 = this.m_Button_Button6;
		expr_110.Click = (EZValueChangedDelegate)Delegate.Combine(expr_110.Click, new EZValueChangedDelegate(this.On_Next_Page_Button));
	}

	public void SetQuestItem(ITEM item, G_ID gid)
	{
		this.m_ItemList = NkUserInventory.GetInstance().GetQuestConItem(item);
		this.m_gid = gid;
		this.Draw_Item_List();
	}

	private void Draw_Item_List()
	{
		this.m_ListBox_ListBox37.SelectIndex = -1;
		this.m_ListBox_ListBox37.Clear();
		int count = this.m_ItemList.Count;
		if (count > 0)
		{
			int num = 0;
			int num2 = 0;
			Protocol_COMMON.Page_Setting(count, 7, ref this.m_nCurrentPage, ref this.m_nMaxPage, out num, out num2);
			for (int i = num; i < num2; i++)
			{
				ListItem listItem = new ListItem();
				listItem.Key = this.m_ItemList[i];
				int nItemUnique = this.m_ItemList[i].m_nItemUnique;
				listItem.SetColumnGUIContent(0, string.Empty, "Win_T_ItemEmpty");
				UIBaseInfoLoader itemTexture = NrTSingleton<ItemManager>.Instance.GetItemTexture(nItemUnique);
				listItem.SetColumnGUIContent(1, string.Empty, itemTexture, this.m_ItemList[i]);
				string name = NrTSingleton<ItemManager>.Instance.GetName(this.m_ItemList[i]);
				listItem.SetColumnStr(2, name, string.Empty);
				this.m_ListBox_ListBox37.Add(listItem);
			}
			this.m_ListBox_ListBox37.RepositionItems();
		}
		this.m_Label_Page.Text = this.m_nCurrentPage + " / " + this.m_nMaxPage;
	}

	private void On_Prev_Page_Button(IUIObject a_cUIObject)
	{
		if (Protocol_COMMON.Prev_Page(ref this.m_nCurrentPage))
		{
			this.Draw_Item_List();
		}
	}

	private void On_Next_Page_Button(IUIObject a_cUIObject)
	{
		if (Protocol_COMMON.Next_Page(ref this.m_nCurrentPage, this.m_nMaxPage))
		{
			this.Draw_Item_List();
		}
	}

	private void OnClickList(IUIObject a_cUIObject)
	{
		IUIListObject selectItem = this.m_ListBox_ListBox37.GetSelectItem();
		if (selectItem != null && selectItem.Data is ITEM && this.m_gid == G_ID.QUESTGIVEITEM_DLG)
		{
			QuestGiveItemDlg questGiveItemDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.QUESTGIVEITEM_DLG) as QuestGiveItemDlg;
			if (questGiveItemDlg != null)
			{
				questGiveItemDlg.SetItemSlot((ITEM)selectItem.Data);
				this.Close();
			}
		}
	}
}
