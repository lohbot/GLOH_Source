using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityForms;

public class Take_DLG : Form
{
	private Label m_Label_title;

	private DrawTexture m_ItemTexture_ItemTexture13;

	private Label m_Label_Note;

	private Label m_Label_ItemQuality;

	private Label m_Label_Itemdrag;

	private Label m_Label_questprice;

	private TextField m_TextField_TextField8;

	private Button m_Button_ok;

	private Button m_Button_cancel;

	private CQuest m_Quest;

	private int m_i32CharKind;

	private ITEM m_Item;

	private QUEST_COMMON_SUB m_QuestCommonSub;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		if (TsPlatform.IsMobile)
		{
			instance.LoadFileAll(ref form, "NpcTalk/DLG_npcextraquest", G_ID.TAKE_DLG, true);
		}
		else
		{
			instance.LoadFileAll(ref form, "NpcTalk/DLG_npcextraquest", G_ID.TAKE_DLG, true);
			UIBaseFileManager instance2 = NrTSingleton<UIBaseFileManager>.Instance;
			int num;
			int num2;
			instance2.LoadFileSize("Inventory/DLG_Inventory", out num, out num2);
			base.SetLocation((GUICamera.width - ((float)num + base.GetSize().x)) / 2f, (GUICamera.height - (float)num2) / 2f);
		}
	}

	public override void SetComponent()
	{
		this.m_Label_title = (base.GetControl("Label_title") as Label);
		this.m_ItemTexture_ItemTexture13 = (base.GetControl("DrawTexture_item") as DrawTexture);
		this.m_Label_Note = (base.GetControl("Label_Note") as Label);
		this.m_Label_ItemQuality = (base.GetControl("Label_ItemQuality") as Label);
		this.m_Label_Itemdrag = (base.GetControl("Label_Itemdrag") as Label);
		this.m_Label_questprice = (base.GetControl("Label_questprice") as Label);
		this.m_Button_ok = (base.GetControl("Button_ok") as Button);
		this.m_Button_cancel = (base.GetControl("Button_cancel") as Button);
		this.m_TextField_TextField8 = (base.GetControl("TextField_TextField8") as TextField);
		Button expr_CC = this.m_Button_ok;
		expr_CC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_CC.Click, new EZValueChangedDelegate(this.OnOK));
		Button expr_F3 = this.m_Button_cancel;
		expr_F3.Click = (EZValueChangedDelegate)Delegate.Combine(expr_F3.Click, new EZValueChangedDelegate(this.OnCancle));
	}

	public void SetQuest(CQuest kQuest, int i32Charkind)
	{
		this.m_Quest = kQuest;
		this.m_Label_title.Text = this.m_Quest.GetQuestTitle();
		this.m_TextField_TextField8.Text = string.Empty;
		if (this.m_Quest.GetQuestCommon().kQuestCommonSub != null)
		{
			for (byte b = 0; b < 3; b += 1)
			{
				if (this.m_Quest.GetQuestCommon().cQuestCondition[(int)b].i32QuestCode == 122 && this.m_Quest.GetQuestCommon().cQuestCondition[(int)b].i64Param == (long)i32Charkind)
				{
					this.m_i32CharKind = i32Charkind;
					this.SetMenu(this.m_Quest.GetQuestCommon().kQuestCommonSub[(int)b]);
					break;
				}
			}
		}
	}

	private void SetMenu(QUEST_COMMON_SUB kSub)
	{
		this.m_QuestCommonSub = kSub;
		this.m_ItemTexture_ItemTexture13.Visible = true;
		this.m_Label_Note.Visible = true;
		this.m_Label_ItemQuality.Visible = true;
		this.m_Label_Itemdrag.Visible = true;
		this.m_Label_questprice.Visible = true;
		GS_QUEST_GET_CHAR_REQ gS_QUEST_GET_CHAR_REQ = new GS_QUEST_GET_CHAR_REQ();
		TKString.StringChar(this.m_Quest.GetQuestUnique(), ref gS_QUEST_GET_CHAR_REQ.strQuestUnique);
		gS_QUEST_GET_CHAR_REQ.i32CharKind = this.m_i32CharKind;
		gS_QUEST_GET_CHAR_REQ.bItemType = 0;
		gS_QUEST_GET_CHAR_REQ.nItemPos = 0;
		gS_QUEST_GET_CHAR_REQ.i32ItemNum = 0;
		SendPacket.GetInstance().SendObject(1023, gS_QUEST_GET_CHAR_REQ);
		this.Close();
	}

	private void OnOK(IUIObject obj)
	{
		if (this.m_QuestCommonSub == null)
		{
			return;
		}
		bool flag = false;
		if (this.m_QuestCommonSub.i32Code == 15)
		{
			long num = 0L;
			long.TryParse(this.m_TextField_TextField8.Text, out num);
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money < num && num < this.m_QuestCommonSub.i64ParamVAl)
			{
				return;
			}
			flag = true;
			GS_QUEST_GET_CHAR_REQ gS_QUEST_GET_CHAR_REQ = new GS_QUEST_GET_CHAR_REQ();
			TKString.StringChar(this.m_Quest.GetQuestUnique(), ref gS_QUEST_GET_CHAR_REQ.strQuestUnique);
			gS_QUEST_GET_CHAR_REQ.i32CharKind = this.m_i32CharKind;
			gS_QUEST_GET_CHAR_REQ.bItemType = 0;
			gS_QUEST_GET_CHAR_REQ.nItemPos = 0;
			gS_QUEST_GET_CHAR_REQ.i32ItemNum = 0;
			SendPacket.GetInstance().SendObject(1023, gS_QUEST_GET_CHAR_REQ);
		}
		else if (this.m_QuestCommonSub.i32Code == 48)
		{
			if (this.m_Item == null)
			{
				return;
			}
			if ((long)this.m_Item.m_nItemUnique != this.m_QuestCommonSub.i64Param)
			{
				return;
			}
			if ((long)this.m_Item.m_nItemNum < this.m_QuestCommonSub.i64ParamVAl)
			{
				return;
			}
			flag = true;
			GS_QUEST_GET_CHAR_REQ gS_QUEST_GET_CHAR_REQ2 = new GS_QUEST_GET_CHAR_REQ();
			TKString.StringChar(this.m_Quest.GetQuestUnique(), ref gS_QUEST_GET_CHAR_REQ2.strQuestUnique);
			gS_QUEST_GET_CHAR_REQ2.i32CharKind = this.m_i32CharKind;
			gS_QUEST_GET_CHAR_REQ2.bItemType = this.m_Item.m_nPosType;
			gS_QUEST_GET_CHAR_REQ2.nItemPos = this.m_Item.m_nItemPos;
			gS_QUEST_GET_CHAR_REQ2.i32ItemNum = this.m_Item.m_nItemNum;
			SendPacket.GetInstance().SendObject(1023, gS_QUEST_GET_CHAR_REQ2);
		}
		TakeTalk_DLG takeTalk_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TAKETALK_DLG) as TakeTalk_DLG;
		if (takeTalk_DLG != null && !flag)
		{
			takeTalk_DLG.SetFailMessage();
		}
		this.Close();
	}

	public override void Set_Value(object a_oObject)
	{
		base.Set_Value(a_oObject);
		this.OnItemDragDrop(a_oObject as ITEM);
	}

	public void OnItemDragDrop(ITEM kItem)
	{
		if ((long)kItem.m_nItemNum < this.m_QuestCommonSub.i64ParamVAl)
		{
			return;
		}
		this.m_Label_Note.Visible = true;
		this.m_Label_ItemQuality.Visible = true;
		this.m_Item = kItem;
		this.m_ItemTexture_ItemTexture13.BaseInfoLoderImage = NrTSingleton<ItemManager>.Instance.GetItemTexture(kItem.m_nItemUnique);
		if (this.m_QuestCommonSub.i32Code == 48)
		{
			this.m_Label_Itemdrag.Visible = false;
			this.m_Label_ItemQuality.Text = this.m_QuestCommonSub.i64ParamVAl.ToString();
			this.m_Label_Note.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(kItem.m_nItemUnique);
		}
	}

	private void OnCancle(IUIObject obj)
	{
		this.Close();
	}
}
