using GAME;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class Item_Box_Select_Dlg : Form
{
	private const int N_LIST_COUNT = 5;

	private Label m_laTitle;

	private ListBox m_lbListBox;

	private Button m_buButton;

	private ITEM m_cItem;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item_Box/DLG_Itembox_All", G_ID.ITEM_BOX_SELECT_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_laTitle = (base.GetControl("Label_Title") as Label);
		this.m_buButton = (base.GetControl("Button_Sel") as Button);
		Button expr_32 = this.m_buButton;
		expr_32.Click = (EZValueChangedDelegate)Delegate.Combine(expr_32.Click, new EZValueChangedDelegate(this.On_Button));
		this.m_buButton.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("445");
		this.m_lbListBox = (base.GetControl("ListBox_ListBox") as ListBox);
		this.m_lbListBox.AutoListBox = false;
		this.m_lbListBox.itemSpacing = 2f;
		this.m_lbListBox.LineHeight = 74f;
		this.m_lbListBox.UseColumnRect = true;
		this.m_lbListBox.ColumnNum = 4;
		this.m_lbListBox.SetColumnRect(0, 8, 7, 61, 61);
		this.m_lbListBox.SetColumnRect(1, 8, 7, 60, 60);
		this.m_lbListBox.SetColumnRect(2, 80, 12, 400, 34, SpriteText.Anchor_Pos.Upper_Left, 28f);
		this.m_lbListBox.SetColumnRect(3, 80, 44, 400, 34, SpriteText.Anchor_Pos.Upper_Left, 26f);
		this.m_lbListBox.SetDoubleClickDelegate(new EZValueChangedDelegate(this.On_Button));
		base.SetScreenCenter();
	}

	private void On_Button(IUIObject obj)
	{
		if (null == this.m_lbListBox.SelectedItem)
		{
			return;
		}
		int num = (int)this.m_lbListBox.SelectedItem.Data;
		if (num == -1 || num < 0 || num >= 12)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("226");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_cItem.m_nItemUnique);
		ITEM_BOX_GROUP iTEM_BOX_GROUP = null;
		if (itemInfo.IsItemATB(65536L))
		{
			iTEM_BOX_GROUP = NrTSingleton<ItemManager>.Instance.GetBoxGroup(this.m_cItem.m_nItemUnique);
			if (iTEM_BOX_GROUP == null)
			{
				return;
			}
		}
		int num2 = itemInfo.m_nBoxItemUnique[num];
		int itemnum = itemInfo.m_nBoxItemNumber[num];
		if (iTEM_BOX_GROUP != null)
		{
			num2 = iTEM_BOX_GROUP.i32GroupItemUnique[num];
			itemnum = iTEM_BOX_GROUP.i32GroupItemNum[num];
		}
		if (itemInfo == null || num2 == 0)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("226");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (!Protocol_Item.CanAddItem(num2, itemnum))
		{
			string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46");
			Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_BOX_USE_REQ gS_BOX_USE_REQ = new GS_BOX_USE_REQ();
		gS_BOX_USE_REQ.m_nItemID = this.m_cItem.m_nItemID;
		gS_BOX_USE_REQ.m_nItemUnique = this.m_cItem.m_nItemUnique;
		gS_BOX_USE_REQ.m_nPosType = this.m_cItem.m_nPosType;
		gS_BOX_USE_REQ.m_nItemPos = this.m_cItem.m_nItemPos;
		gS_BOX_USE_REQ.m_nArrayIndex = num;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BOX_USE_REQ, gS_BOX_USE_REQ);
		base.CloseNow();
	}

	public void Set_Item(ITEM a_cItem)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		NkSoldierInfo nkSoldierInfo = null;
		if (nrCharUser != null)
		{
			nkSoldierInfo = nrCharUser.GetPersonInfo().GetLeaderSoldierInfo();
		}
		this.m_lbListBox.Clear();
		this.m_cItem = a_cItem;
		this.m_laTitle.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_cItem);
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_cItem.m_nItemUnique);
		ITEM_BOX_GROUP iTEM_BOX_GROUP = null;
		if (itemInfo.IsItemATB(65536L))
		{
			iTEM_BOX_GROUP = NrTSingleton<ItemManager>.Instance.GetBoxGroup(this.m_cItem.m_nItemUnique);
			if (iTEM_BOX_GROUP == null)
			{
				return;
			}
		}
		int num = 0;
		for (int i = 0; i < 12; i++)
		{
			int num2;
			int num3;
			int num4;
			if (iTEM_BOX_GROUP != null)
			{
				num2 = iTEM_BOX_GROUP.i32GroupItemUnique[i];
				num3 = iTEM_BOX_GROUP.i32GroupItemNum[i];
				num4 = iTEM_BOX_GROUP.i32GroupItemGrade[i];
			}
			else
			{
				num2 = itemInfo.m_nBoxItemUnique[i];
				num3 = itemInfo.m_nBoxItemNumber[i];
				num4 = itemInfo.m_nBoxRank;
			}
			if (num2 > 0)
			{
				if (NrTSingleton<ItemManager>.Instance.IsItemATB(a_cItem.m_nItemUnique, 256L))
				{
					NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
					if (kMyCharInfo != null)
					{
						ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(num2);
						if (itemTypeInfo != null)
						{
							if (nkSoldierInfo != null && nkSoldierInfo.IsEquipClassType(itemTypeInfo.WEAPONTYPE, itemTypeInfo.EQUIPCLASSTYPE))
							{
								num++;
								ListItem listItem = new ListItem();
								listItem.SetGameObjectDelegate(new EZGameObjectDelegate(this.ItemAddEffectDelegate));
								listItem.SetColumnGUIContent(0, string.Empty, "Win_T_ItemEmpty");
								if (num4 == 0)
								{
									UIBaseInfoLoader itemTexture = NrTSingleton<ItemManager>.Instance.GetItemTexture(num2);
									listItem.SetColumnGUIContent(1, string.Empty, itemTexture, NrTSingleton<ItemManager>.Instance.GetBoxItemTemp(this.m_cItem.m_nItemUnique, i));
								}
								else
								{
									ITEM iTEM = new ITEM();
									if (iTEM_BOX_GROUP != null)
									{
										iTEM.m_nItemID = -9223372036854775808L;
										iTEM.m_nItemUnique = iTEM_BOX_GROUP.i32GroupItemUnique[i];
										iTEM.m_nItemNum = iTEM_BOX_GROUP.i32GroupItemNum[i];
										iTEM.m_nOption[0] = 100;
										iTEM.m_nOption[1] = 100;
										iTEM.m_nOption[2] = iTEM_BOX_GROUP.i32GroupItemGrade[i];
										iTEM.m_nOption[3] = 1;
										iTEM.m_nOption[4] = iTEM_BOX_GROUP.i32GroupItemSkillUnique[i];
										iTEM.m_nOption[5] = iTEM_BOX_GROUP.i32GroupItemSkillLevel[i];
										iTEM.m_nOption[7] = iTEM_BOX_GROUP.i32GroupItemTradePoint[i];
										iTEM.m_nOption[8] = iTEM_BOX_GROUP.i32GroupItemReducePoint[i];
										iTEM.m_nOption[6] = iTEM_BOX_GROUP.i32GroupItemSkill2Unique[i];
										iTEM.m_nOption[9] = iTEM_BOX_GROUP.i32GroupItemSkill2Level[i];
										iTEM.m_nDurability = 100;
										listItem.SetColumnGUIContent(1, iTEM, true);
									}
									else
									{
										iTEM.Set(this.m_cItem);
										iTEM.m_nItemUnique = num2;
										iTEM.m_nOption[2] = num4;
										listItem.SetColumnGUIContent(1, iTEM, true);
									}
								}
								string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(num2);
								string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");
								listItem.SetColumnStr(2, itemNameByItemUnique, textColor);
								string str = Protocol_Item.Money_Format((long)num3) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442");
								string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1105");
								listItem.SetColumnStr(3, str, textColor2);
								listItem.Key = i;
								this.m_lbListBox.Add(listItem);
							}
						}
					}
				}
				else
				{
					num++;
					ListItem listItem2 = new ListItem();
					listItem2.SetGameObjectDelegate(new EZGameObjectDelegate(this.ItemAddEffectDelegate));
					listItem2.SetColumnGUIContent(0, string.Empty, "Win_T_ItemEmpty");
					if (num4 == 0)
					{
						UIBaseInfoLoader itemTexture2 = NrTSingleton<ItemManager>.Instance.GetItemTexture(num2);
						listItem2.SetColumnGUIContent(1, string.Empty, itemTexture2, NrTSingleton<ItemManager>.Instance.GetBoxItemTemp(this.m_cItem.m_nItemUnique, i));
					}
					else
					{
						ITEM iTEM2 = new ITEM();
						if (iTEM_BOX_GROUP != null)
						{
							iTEM2.m_nItemID = -9223372036854775808L;
							iTEM2.m_nItemUnique = iTEM_BOX_GROUP.i32GroupItemUnique[i];
							iTEM2.m_nItemNum = iTEM_BOX_GROUP.i32GroupItemNum[i];
							iTEM2.m_nOption[0] = 100;
							iTEM2.m_nOption[1] = 100;
							iTEM2.m_nOption[2] = iTEM_BOX_GROUP.i32GroupItemGrade[i];
							iTEM2.m_nOption[3] = 1;
							iTEM2.m_nOption[4] = iTEM_BOX_GROUP.i32GroupItemSkillUnique[i];
							iTEM2.m_nOption[5] = iTEM_BOX_GROUP.i32GroupItemSkillLevel[i];
							iTEM2.m_nOption[7] = iTEM_BOX_GROUP.i32GroupItemTradePoint[i];
							iTEM2.m_nOption[8] = iTEM_BOX_GROUP.i32GroupItemReducePoint[i];
							iTEM2.m_nOption[6] = iTEM_BOX_GROUP.i32GroupItemSkill2Unique[i];
							iTEM2.m_nOption[9] = iTEM_BOX_GROUP.i32GroupItemSkill2Level[i];
							iTEM2.m_nDurability = 100;
							listItem2.SetColumnGUIContent(1, iTEM2, true);
						}
						else
						{
							iTEM2.Set(this.m_cItem);
							iTEM2.m_nItemUnique = num2;
							iTEM2.m_nOption[2] = num4;
							listItem2.SetColumnGUIContent(1, iTEM2, true);
						}
					}
					string itemNameByItemUnique2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(num2);
					string textColor3 = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");
					listItem2.SetColumnStr(2, itemNameByItemUnique2, textColor3);
					string str2 = Protocol_Item.Money_Format((long)num3) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442");
					string textColor4 = NrTSingleton<CTextParser>.Instance.GetTextColor("1105");
					listItem2.SetColumnStr(3, str2, textColor4);
					listItem2.Key = i;
					this.m_lbListBox.Add(listItem2);
				}
			}
		}
		this.m_lbListBox.RepositionItems();
		this.Show();
	}

	public void ItemAddEffectDelegate(IUIObject control, GameObject obj)
	{
		if (null == obj)
		{
			return;
		}
		obj.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
	}
}
