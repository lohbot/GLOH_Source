using GAME;
using System;
using UnityForms;

public class QuestGiveItemDlg : Form
{
	private static int MAX_LIST_ITEMNUM = 3;

	private ListBox listBox;

	private Button giveItemButton;

	private NPC_TALK_QUEST_STATE m_State;

	private QUEST_GIVE_ITEM_UI_SET[] m_ItemUISet = new QUEST_GIVE_ITEM_UI_SET[3];

	private byte m_bCurrentSlot;

	private int m_i32ChildWidth_1;

	private QuestItemSel_Dlg m_QuestItemSeldlg;

	private float m_fOri_X;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "dlg_npcgiveitem", G_ID.QUESTGIVEITEM_DLG, false);
	}

	public override void SetComponent()
	{
		this.listBox = (base.GetControl("ListBox_ListBox") as ListBox);
		this.listBox.itemSpacing = 0f;
		this.listBox.LineHeight = 114f;
		this.listBox.UseColumnRect = true;
		this.listBox.ColumnNum = 5;
		this.listBox.SetColumnRect(0, 10, 10, 94, 94);
		this.listBox.SetColumnRect(1, 17, 17, 80, 80);
		this.listBox.SetColumnRect(2, 114, 21, 240, 30, SpriteText.Anchor_Pos.Middle_Left, 22f);
		this.listBox.SetColumnRect(3, 114, 62, 80, 25, SpriteText.Anchor_Pos.Middle_Left, 22f);
		this.listBox.SetColumnRect(4, 150, 80, 80, 26, SpriteText.Anchor_Pos.Middle_Center, 22f);
		this.giveItemButton = (base.GetControl("Button_ok") as Button);
		this.giveItemButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGiveItem));
		for (int i = 0; i < 3; i++)
		{
			this.m_ItemUISet[i] = new QUEST_GIVE_ITEM_UI_SET();
		}
		base.SetScreenCenter();
		base.SetLocation(base.GetLocation().x, GUICamera.height - (base.GetSize().y + 270f));
	}

	public void ClickGiveItem(IUIObject obj)
	{
		if (this.m_State.eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
		{
			NrTSingleton<NkQuestManager>.Instance.SetCompleteItem(this.m_ItemUISet[0].m_Item, this.m_ItemUISet[1].m_Item, this.m_ItemUISet[2].m_Item);
		}
		NpcTalkUI_DLG npcTalkUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NPCTALK_DLG) as NpcTalkUI_DLG;
		if (npcTalkUI_DLG != null)
		{
			npcTalkUI_DLG.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_TALK);
		}
		this.Close();
	}

	public void SetGiveItemList(ITEM item)
	{
		this.listBox.Clear();
		for (int i = 0; i < QuestGiveItemDlg.MAX_LIST_ITEMNUM; i++)
		{
			ListItem listItem = new ListItem();
			listItem.SetColumnGUIContent(0, string.Empty, "Win_T_ItemEmpty");
			listItem.SetColumnGUIContent(1, string.Empty, NrTSingleton<ItemManager>.Instance.GetItemTexture(item.m_nItemUnique));
			listItem.SetColumnGUIContent(2, NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(item));
			listItem.SetColumnGUIContent(3, item.m_nItemNum.ToString());
		}
	}

	public void SetTalkState(NPC_TALK_QUEST_STATE State)
	{
		this.m_State = State;
		this.listBox.Clear();
		for (int i = 0; i < 3; i++)
		{
			if (this.m_State.kQuest.GetQuestCommon().cQuestCondition[i].i32QuestCode == 7 || this.m_State.kQuest.GetQuestCommon().cQuestCondition[i].i32QuestCode == 8 || this.m_State.kQuest.GetQuestCommon().cQuestCondition[i].i32QuestCode == 48 || this.m_State.kQuest.GetQuestCommon().cQuestCondition[i].i32QuestCode == 107)
			{
				int num;
				if (this.m_State.kQuest.GetQuestCommon().cQuestCondition[i].i32QuestCode == 8)
				{
					num = (int)this.m_State.kQuest.GetQuestCommon().cQuestCondition[1].i64Param;
				}
				else
				{
					num = (int)this.m_State.kQuest.GetQuestCommon().cQuestCondition[i].i64Param;
				}
				if (0 < num)
				{
					this.m_ItemUISet[i].m_Item = NkUserInventory.GetInstance().GetFirstItemByUniqueLowRank(num);
					if (this.m_ItemUISet[i].m_Item != null)
					{
						ListItem listItem = new ListItem();
						string name = NrTSingleton<ItemManager>.Instance.GetName(this.m_ItemUISet[i].m_Item);
						listItem.SetColumnGUIContent(0, string.Empty, "Win_T_ItemEmpty");
						listItem.SetColumnGUIContent(1, string.Empty, NrTSingleton<ItemManager>.Instance.GetItemTexture(num));
						listItem.SetColumnGUIContent(2, name);
						listItem.SetColumnGUIContent(3, this.m_State.kQuest.GetQuestCommon().cQuestCondition[i].i64ParamVal.ToString() + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442"));
						if (Protocol_Item.Is_EquipItem(this.m_ItemUISet[i].m_Item.m_nItemUnique))
						{
							listItem.SetColumnGUIContent(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1522"), "Win_B_BasicBtnSS", i, new EZValueChangedDelegate(this.OnChange));
						}
						else
						{
							listItem.SetColumnGUIContent(4, string.Empty, "Com_I_Transparent");
						}
						listItem.Key = this.m_ItemUISet[i].m_Item;
						this.listBox.Add(listItem);
					}
				}
			}
		}
		this.listBox.RepositionItems();
	}

	public void SetItemSlot(ITEM item)
	{
		this.m_ItemUISet[(int)this.m_bCurrentSlot].m_Item = item;
		this.listBox.Clear();
		for (int i = 0; i < 3; i++)
		{
			if (this.m_ItemUISet[i].m_Item != null)
			{
				int nItemUnique = this.m_ItemUISet[i].m_Item.m_nItemUnique;
				if (0 < nItemUnique)
				{
					ListItem listItem = new ListItem();
					string name = NrTSingleton<ItemManager>.Instance.GetName(this.m_ItemUISet[i].m_Item);
					listItem.SetColumnGUIContent(0, string.Empty, "Win_T_ItemEmpty");
					listItem.SetColumnGUIContent(1, string.Empty, NrTSingleton<ItemManager>.Instance.GetItemTexture(nItemUnique));
					listItem.SetColumnGUIContent(2, name);
					listItem.SetColumnGUIContent(3, this.m_State.kQuest.GetQuestCommon().cQuestCondition[i].i64ParamVal.ToString());
					if (Protocol_Item.Is_EquipItem(this.m_ItemUISet[i].m_Item.m_nItemUnique))
					{
						listItem.SetColumnGUIContent(4, "�ٲٱ�", "Win_B_BasicBtnSS", i, new EZValueChangedDelegate(this.OnChange));
					}
					else
					{
						listItem.SetColumnGUIContent(4, string.Empty, "Com_I_Transparent");
					}
					listItem.Key = this.m_ItemUISet[i].m_Item;
					this.listBox.Add(listItem);
				}
			}
		}
		this.listBox.RepositionItems();
		if (this.m_QuestItemSeldlg != null)
		{
			ITEM item2 = this.m_ItemUISet[(int)this.m_bCurrentSlot].m_Item;
			this.m_QuestItemSeldlg.SetQuestItem(item2, G_ID.QUESTGIVEITEM_DLG);
		}
	}

	private void OnChange(IUIObject obj)
	{
		UIButton uIButton = obj as UIButton;
		if (null == uIButton)
		{
			return;
		}
		if (TsPlatform.IsMobile)
		{
			this.m_QuestItemSeldlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.QUESTITEM_SEL_DLG) as QuestItemSel_Dlg);
			if (this.m_QuestItemSeldlg == null)
			{
				return;
			}
		}
		else
		{
			float y = (GUICamera.height - 130f - base.GetSize().y) / 2f;
			base.SetLocation(this.m_fOri_X - (float)this.m_i32ChildWidth_1, y);
			this.m_QuestItemSeldlg = (base.SetChildForm(G_ID.QUESTITEM_SEL_DLG) as QuestItemSel_Dlg);
			if (this.m_QuestItemSeldlg == null)
			{
				return;
			}
		}
		this.m_bCurrentSlot = (byte)uIButton.Data;
		ITEM item = this.m_ItemUISet[(int)this.m_bCurrentSlot].m_Item;
		this.m_QuestItemSeldlg.SetQuestItem(item, G_ID.QUESTGIVEITEM_DLG);
		this.m_QuestItemSeldlg.Show();
	}
}
