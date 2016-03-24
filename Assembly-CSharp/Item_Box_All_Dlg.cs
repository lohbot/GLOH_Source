using GAME;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Item_Box_All_Dlg : Form
{
	private const int N_LIST_COUNT = 5;

	private Label m_laTitle;

	private ListBox m_lbListBox;

	private Button m_buButton;

	private Button m_btClose;

	private bool m_bBoxCollider = true;

	private ITEM m_cItem;

	private eITEMMALL_BOXTRADE_TYPE m_eItemMall_BoxType = eITEMMALL_BOXTRADE_TYPE.ITEMMALL_TRADETYPE_GETBOX;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item_Box/DLG_Itembox_All", G_ID.ITEM_BOX_ALL_DLG, true);
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
		this.m_lbListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemIcon));
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		base.SetScreenCenter();
	}

	private void On_Button(IUIObject obj)
	{
		List<Protocol_Item.Add_Item> list = new List<Protocol_Item.Add_Item>();
		Protocol_Item.Add_Item item = default(Protocol_Item.Add_Item);
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
		if (itemInfo != null)
		{
			for (int i = 0; i < 12; i++)
			{
				int num;
				int num2;
				if (iTEM_BOX_GROUP != null)
				{
					num = iTEM_BOX_GROUP.i32GroupItemUnique[i];
					num2 = iTEM_BOX_GROUP.i32GroupItemNum[i];
				}
				else
				{
					num = itemInfo.m_nBoxItemUnique[i];
					num2 = itemInfo.m_nBoxItemNumber[i];
				}
				if (num > 0)
				{
					if (!Protocol_Item.CanAddItem(num, num2))
					{
						string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46");
						Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						return;
					}
					item.m_nItemUnique = num;
					item.m_nItemNum = num2;
					list.Add(item);
				}
			}
		}
		if (this.m_eItemMall_BoxType == eITEMMALL_BOXTRADE_TYPE.ITEMMALL_TRADETYPE_GETBOX)
		{
			GS_BOX_USE_REQ gS_BOX_USE_REQ = new GS_BOX_USE_REQ();
			gS_BOX_USE_REQ.m_nItemID = this.m_cItem.m_nItemID;
			gS_BOX_USE_REQ.m_nItemUnique = this.m_cItem.m_nItemUnique;
			gS_BOX_USE_REQ.m_nPosType = this.m_cItem.m_nPosType;
			gS_BOX_USE_REQ.m_nItemPos = this.m_cItem.m_nItemPos;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BOX_USE_REQ, gS_BOX_USE_REQ);
		}
		this.Close();
	}

	public void Set_Item(ITEM a_cItem)
	{
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
		int i = 0;
		for (int j = 0; j < 12; j++)
		{
			int num;
			int num2;
			int num3;
			if (iTEM_BOX_GROUP != null)
			{
				num = iTEM_BOX_GROUP.i32GroupItemUnique[j];
				num2 = iTEM_BOX_GROUP.i32GroupItemNum[j];
				num3 = iTEM_BOX_GROUP.i32GroupItemGrade[j];
			}
			else
			{
				num = itemInfo.m_nBoxItemUnique[j];
				num2 = itemInfo.m_nBoxItemNumber[j];
				num3 = itemInfo.m_nBoxRank;
			}
			if (num > 0)
			{
				i++;
				ListItem listItem = new ListItem();
				listItem.SetColumnGUIContent(0, string.Empty, "Win_T_ItemEmpty");
				if (num3 == 0)
				{
					UIBaseInfoLoader itemTexture = NrTSingleton<ItemManager>.Instance.GetItemTexture(num);
					listItem.SetColumnGUIContent(1, string.Empty, itemTexture, NrTSingleton<ItemManager>.Instance.GetBoxItemTemp(this.m_cItem.m_nItemUnique, j));
				}
				else
				{
					ITEM iTEM = new ITEM();
					if (iTEM_BOX_GROUP != null)
					{
						iTEM.m_nItemID = -9223372036854775808L;
						iTEM.m_nItemUnique = iTEM_BOX_GROUP.i32GroupItemUnique[j];
						iTEM.m_nItemNum = iTEM_BOX_GROUP.i32GroupItemNum[j];
						iTEM.m_nOption[0] = 100;
						iTEM.m_nOption[1] = 100;
						iTEM.m_nOption[2] = iTEM_BOX_GROUP.i32GroupItemGrade[j];
						iTEM.m_nOption[3] = 1;
						iTEM.m_nOption[4] = iTEM_BOX_GROUP.i32GroupItemSkillUnique[j];
						iTEM.m_nOption[5] = iTEM_BOX_GROUP.i32GroupItemSkillLevel[j];
						iTEM.m_nOption[7] = iTEM_BOX_GROUP.i32GroupItemTradePoint[j];
						iTEM.m_nOption[8] = iTEM_BOX_GROUP.i32GroupItemReducePoint[j];
						iTEM.m_nOption[6] = iTEM_BOX_GROUP.i32GroupItemSkill2Unique[j];
						iTEM.m_nOption[9] = iTEM_BOX_GROUP.i32GroupItemSkill2Level[j];
						iTEM.m_nDurability = 100;
						listItem.SetColumnGUIContent(1, iTEM, true);
					}
					else
					{
						iTEM.Set(this.m_cItem);
						iTEM.m_nItemUnique = num;
						iTEM.m_nOption[2] = num3;
						listItem.SetColumnGUIContent(1, iTEM, true);
					}
				}
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(num);
				string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");
				listItem.SetColumnStr(2, itemNameByItemUnique, textColor);
				int num4 = num2;
				string str = Protocol_Item.Money_Format((long)num4) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442");
				string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1105");
				listItem.SetColumnStr(3, str, textColor2);
				listItem.Key = j;
				this.m_lbListBox.Add(listItem);
			}
		}
		while (i < 5)
		{
			ListItem listItem2 = new ListItem();
			listItem2.SetColumnGUIContent(0, string.Empty, "Win_T_ItemEmpty");
			listItem2.SetColumnGUIContent(1, string.Empty);
			listItem2.SetColumnGUIContent(2, string.Empty);
			listItem2.SetColumnGUIContent(3, string.Empty);
			listItem2.Key = i;
			this.m_lbListBox.Add(listItem2);
			i++;
		}
		this.m_lbListBox.RepositionItems();
		this.Show();
	}

	private void OnClickItemIcon(IUIObject obj)
	{
		if (!TsPlatform.IsMobile)
		{
			return;
		}
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
		int num3 = itemInfo.m_nBoxRank;
		if (iTEM_BOX_GROUP != null)
		{
			num2 = iTEM_BOX_GROUP.i32GroupItemUnique[num];
			num3 = iTEM_BOX_GROUP.i32GroupItemGrade[num];
		}
		if (itemInfo == null || num2 == 0)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("226");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		ITEM iTEM = new ITEM();
		if (iTEM_BOX_GROUP != null)
		{
			iTEM.m_nItemID = -9223372036854775808L;
			iTEM.m_nItemUnique = iTEM_BOX_GROUP.i32GroupItemUnique[num];
			iTEM.m_nItemNum = iTEM_BOX_GROUP.i32GroupItemNum[num];
			iTEM.m_nOption[0] = (int)NrTSingleton<Item_Makerank_Manager>.Instance.GetItemAblility((byte)iTEM_BOX_GROUP.i32GroupItemGrade[num]);
			iTEM.m_nOption[1] = (int)NrTSingleton<Item_Makerank_Manager>.Instance.GetItemAblility((byte)iTEM_BOX_GROUP.i32GroupItemGrade[num]);
			iTEM.m_nOption[2] = iTEM_BOX_GROUP.i32GroupItemGrade[num];
			iTEM.m_nOption[3] = 1;
			iTEM.m_nOption[4] = iTEM_BOX_GROUP.i32GroupItemSkillUnique[num];
			iTEM.m_nOption[5] = 1;
			iTEM.m_nOption[7] = iTEM_BOX_GROUP.i32GroupItemTradePoint[num];
			iTEM.m_nOption[8] = iTEM_BOX_GROUP.i32GroupItemReducePoint[num];
			iTEM.m_nOption[6] = iTEM_BOX_GROUP.i32GroupItemSkill2Unique[num];
			iTEM.m_nOption[9] = 1;
			iTEM.m_nDurability = 100;
		}
		else
		{
			iTEM.Set(this.m_cItem);
			iTEM.m_nItemUnique = num2;
			iTEM.m_nOption[2] = num3;
			iTEM.m_nOption[0] = (int)NrTSingleton<Item_Makerank_Manager>.Instance.GetItemAblility((byte)num3);
			iTEM.m_nOption[1] = (int)NrTSingleton<Item_Makerank_Manager>.Instance.GetItemAblility((byte)num3);
		}
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		if (itemTooltipDlg != null)
		{
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
			this.BoxColliderActive(false);
		}
	}

	private void BoxColliderActive(bool bActive)
	{
		BoxCollider boxCollider = (BoxCollider)base.BLACK_BG.gameObject.GetComponent(typeof(BoxCollider));
		if (boxCollider != null)
		{
			boxCollider.enabled = bActive;
		}
		this.m_bBoxCollider = bActive;
	}

	public override void Update()
	{
		base.Update();
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMTOOLTIP_DLG) && !this.m_bBoxCollider)
		{
			this.BoxColliderActive(true);
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMTOOLTIP_DLG) && NkInputManager.GetMouseButtonDown(0))
		{
			Ray ray = NrTSingleton<UIManager>.Instance.rayCamera.ScreenPointToRay(NkInputManager.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit) && raycastHit.collider.name.Contains("BT_SET"))
			{
				return;
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
		}
	}
}
