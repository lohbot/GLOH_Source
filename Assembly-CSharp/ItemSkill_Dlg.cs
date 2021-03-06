using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using System.Text;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ItemSkill_Dlg : Form
{
	private enum SHOWTYPE
	{
		ITEM,
		SOLDER,
		SOLITEM
	}

	private const int MATERIAL_ITEM_UNIQUE_1 = 50304;

	private const int MATERIAL_ITEM_UNIQUE_2 = 50309;

	private const int MATERIAL_ITEM_UNIQUE_3 = 50308;

	private Toolbar m_Tab;

	private DropDownList m_Droplist;

	private NewListBox m_InvenItemListBox;

	private NewListBox m_SolListBox;

	private Button m_btEffectShowHelp;

	private Label m_EmptyMessage;

	private ImageView[] m_EquipItemSlot;

	private ImageView m_ItemSlot;

	private ImageView m_ItemMaterialSlot;

	private DrawTexture m_txBG;

	private DrawTexture m_txItemSlotBG;

	private DrawTexture m_txLabelBG;

	private DrawTexture m_txRingslotlock;

	private byte m_nSearch_SolPosType = 1;

	private int m_nSearch_SolSortType = 4;

	private List<NkSoldierInfo> m_kSolList = new List<NkSoldierInfo>();

	private List<NkSoldierInfo> m_kSolSortList = new List<NkSoldierInfo>();

	private List<ITEM> m_InvenItemList = new List<ITEM>();

	private Label m_lbHaveMaterial;

	private Label m_lbHaveMaterial_2;

	private Label m_lbHaveMaterial_3;

	private Label m_lbRequestMaterial;

	private Label m_lbGuidText;

	private Label m_lbItemName;

	private Label m_lbMaterialName;

	private Button m_btnConfirm;

	private Label m_lbTradeConfirm;

	private Label m_lbEnhance;

	private CheckBox m_cbEnhance;

	private Label m_lbAgitNPCInfo;

	private Button m_HelpButton;

	private NkSoldierInfo m_SelectSol;

	private ITEM m_SelectItem;

	private ITEM m_pMaterialItem;

	private DrawTexture m_txItemIcon1;

	private DrawTexture m_txItemIcon2;

	private DrawTexture m_txItemIcon4;

	private DrawTexture m_txItemIcon5;

	private DrawTexture m_txItemIcon6;

	private DrawTexture m_txItemIcon7;

	private Label m_lbMyMoney;

	private Label m_lbtitle;

	private Label m_lbSubtitle;

	private Label m_lbSkillReinforceGold;

	private Label m_lbSkillReinforceNeedItem1;

	private Label m_lbSkillReinforceNeedItem2;

	private Label m_lbSkillReinforceProtect;

	private Label m_lbSkillReinforceRate;

	private Label m_lbSkillReinforceNowSkilllevel;

	private Label m_lbSkillReinforceNextSkillLevel;

	private CheckBox m_cbSkillReinforceProtect;

	private ItemSkill_Dlg.SHOWTYPE m_ShowType;

	private long m_SolID;

	private long m_SelectItemSol;

	private int m_RequestGold;

	private int m_RequestMaterialNum;

	private int m_RequestMaterialUnique;

	private int m_RequestEnhanceUnique;

	private int m_RequestEnhanceNum;

	private byte m_byMilityUnique;

	private GameObject rootGameObject;

	private bool bRequest;

	private bool bLoadActionItemSkill;

	private float fStartTime;

	public bool m_bAgitNPC;

	private int m_nMaterial1_Num = -1;

	private int m_nMaterial2_Num = -1;

	private int m_nMaterial3_Num = -1;

	private float m_fMaterialNumCheck;

	private int REINFORCE_NEED_ITEM_UNIQUE_1 = 50304;

	private int REINFORCE_NEED_ITEM_UNIQUE_2 = 50314;

	private int REINFORCE_PROTECT_ITEM_UNIQUE_3 = 50309;

	private int m_nReinforceProtectItem_Num;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/itemskill/dlg_itemskillmain", G_ID.ITEMSKILL_DLG, true);
	}

	public override void SetComponent()
	{
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
		this.m_txBG = (base.GetControl("DrawTexture_subbg") as DrawTexture);
		this.m_txBG.SetTextureFromBundle("UI/Etc/reforge_magic");
		this.m_txItemSlotBG = (base.GetControl("DrawTexture_DrawTexture22") as DrawTexture);
		this.m_btEffectShowHelp = (base.GetControl("Help_Button") as Button);
		this.m_btEffectShowHelp.Click = new EZValueChangedDelegate(this.BtnShowEffectHelpSol);
		this.m_lbRequestMaterial = (base.GetControl("Label_text8") as Label);
		this.m_lbHaveMaterial = (base.GetControl("Label_text10") as Label);
		this.m_lbHaveMaterial_2 = (base.GetControl("Label_text11") as Label);
		this.m_lbHaveMaterial_3 = (base.GetControl("Label_text12") as Label);
		this.m_lbItemName = (base.GetControl("Label_equip") as Label);
		this.m_lbGuidText = (base.GetControl("Label_text") as Label);
		this.m_lbMaterialName = (base.GetControl("Label_text7") as Label);
		this.m_ItemSlot = (base.GetControl("ImageView_equip") as ImageView);
		this.m_ItemSlot.SetImageView(1, 1, 80, 80, 1, 1, (int)this.m_ItemSlot.GetSize().y);
		this.m_ItemSlot.spacingAtEnds = false;
		this.m_ItemSlot.touchScroll = false;
		this.m_ItemSlot.clipContents = false;
		this.m_ItemSlot.ListDrag = false;
		if (TsPlatform.IsMobile)
		{
			this.m_ItemSlot.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Over));
		}
		else
		{
			this.m_ItemSlot.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Click));
			this.m_ItemSlot.AddMouseOutDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Out));
		}
		this.m_ItemMaterialSlot = (base.GetControl("ImageView_material") as ImageView);
		this.m_ItemMaterialSlot.SetImageView(1, 1, 80, 80, 1, 1, (int)this.m_ItemSlot.GetSize().y);
		this.m_ItemMaterialSlot.spacingAtEnds = false;
		this.m_ItemMaterialSlot.touchScroll = false;
		this.m_ItemMaterialSlot.clipContents = false;
		this.m_ItemMaterialSlot.ListDrag = false;
		this.m_EmptyMessage = (base.GetControl("Label_text6") as Label);
		this.m_EquipItemSlot = new ImageView[6];
		for (int i = 0; i < 6; i++)
		{
			this.m_EquipItemSlot[i] = (base.GetControl("ImageView_slot" + (i + 1).ToString()) as ImageView);
			this.m_EquipItemSlot[i].SetImageView(1, 1, 80, 80, 1, 1, (int)this.m_EquipItemSlot[i].GetSize().y);
			this.m_EquipItemSlot[i].spacingAtEnds = false;
			this.m_EquipItemSlot[i].touchScroll = false;
			this.m_EquipItemSlot[i].clipContents = false;
			this.m_EquipItemSlot[i].ListDrag = false;
			this.m_EquipItemSlot[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Click));
			this.m_EquipItemSlot[i].AddMouseOutDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Out));
		}
		this.m_txRingslotlock = (base.GetControl("DrawTexture_ringslotlock") as DrawTexture);
		this.m_txRingslotlock.Visible = true;
		this.m_Tab = (base.GetControl("ToolBar_tab") as Toolbar);
		this.m_Tab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("204");
		this.m_Tab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1490");
		UIPanelTab expr_3BE = this.m_Tab.Control_Tab[0];
		expr_3BE.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_3BE.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_3EC = this.m_Tab.Control_Tab[1];
		expr_3EC.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_3EC.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_Droplist = (base.GetControl("DropDownList_DropDownList1") as DropDownList);
		this.m_Droplist.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), 1);
		this.m_Droplist.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("123"), 0);
		this.m_Droplist.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("120"), 100);
		this.m_Droplist.SetViewArea(this.m_Droplist.Count);
		this.m_Droplist.RepositionItems();
		this.m_Droplist.SetFirstItem();
		this.m_Droplist.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeSortSolList));
		this.m_Tab.FirstSetting();
		this.m_Tab.SetSelectTabIndex(0);
		this.m_InvenItemListBox = (base.GetControl("NewListBox_ListBox1") as NewListBox);
		this.m_InvenItemListBox.SetColumnData("Mobile/DLG/Item/ItemSkill/newlistbox_listbox1_columndata" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		this.m_InvenItemListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemClick));
		this.m_InvenItemListBox.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnSolClick));
		this.m_SolListBox = (base.GetControl("NewListBox_ListBox2") as NewListBox);
		this.m_SolListBox.SetColumnData("Mobile/DLG/Item/ItemSkill/newlistbox_listbox2_columndata" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		this.m_SolListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnSolClick));
		this.m_SolListBox.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnItemClick));
		this.m_btnConfirm = (base.GetControl("Button_confirm") as Button);
		this.m_btnConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemConfirm));
		this.m_lbTradeConfirm = (base.GetControl("Label_limit") as Label);
		this.m_lbTradeConfirm.Visible = false;
		this.m_txLabelBG = (base.GetControl("DrawTexture_labelbg2") as DrawTexture);
		this.m_txLabelBG.Visible = false;
		this.m_lbEnhance = (base.GetControl("Label_enhance") as Label);
		this.m_lbEnhance.Visible = false;
		this.m_cbEnhance = (base.GetControl("CheckBox_enhance") as CheckBox);
		this.m_cbEnhance.Visible = false;
		this.m_HelpButton = (base.GetControl("Help_List") as Button);
		this.m_HelpButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		this.m_lbAgitNPCInfo = (base.GetControl("Label_AgitNPC") as Label);
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsGuildAgit() && NrTSingleton<NewGuildManager>.Instance.IsAgitNPC(4))
		{
			AGIT_NPC_SUB_DATA agitNPCSubDataFromNPCType = NrTSingleton<NewGuildManager>.Instance.GetAgitNPCSubDataFromNPCType(4);
			AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(agitNPCSubDataFromNPCType.ui8NPCType.ToString());
			if (agitNPCSubDataFromNPCType != null && agitNPCData != null)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2750"),
					"count",
					agitNPCData.i32LevelRate[(int)(agitNPCSubDataFromNPCType.i16NPCLevel - 1)] / 100
				});
				this.m_lbAgitNPCInfo.SetText(empty);
				this.m_bAgitNPC = true;
			}
		}
		if (!this.m_bAgitNPC)
		{
			this.m_lbAgitNPCInfo.Visible = false;
		}
		this.m_txItemIcon1 = (base.GetControl("DT_1") as DrawTexture);
		this.m_txItemIcon1.Visible = false;
		this.m_txItemIcon2 = (base.GetControl("DT_2") as DrawTexture);
		this.m_txItemIcon2.Visible = false;
		this.m_txItemIcon4 = (base.GetControl("DT_4") as DrawTexture);
		this.m_txItemIcon4.Visible = false;
		this.m_txItemIcon5 = (base.GetControl("DT_5") as DrawTexture);
		this.m_txItemIcon5.Visible = false;
		this.m_txItemIcon6 = (base.GetControl("DT_6") as DrawTexture);
		this.m_txItemIcon6.Visible = false;
		this.m_txItemIcon7 = (base.GetControl("DT_7") as DrawTexture);
		this.m_txItemIcon7.Visible = false;
		this.m_lbMyMoney = (base.GetControl("Label_text1") as Label);
		this.m_lbMyMoney.Visible = false;
		this.m_lbtitle = (base.GetControl("Label_title") as Label);
		this.m_lbSubtitle = (base.GetControl("Label_subtitle1") as Label);
		this.m_lbSkillReinforceGold = (base.GetControl("Label_gold") as Label);
		this.m_lbSkillReinforceNeedItem1 = (base.GetControl("Label_item1") as Label);
		this.m_lbSkillReinforceNeedItem2 = (base.GetControl("Label_item2") as Label);
		this.m_lbSkillReinforceProtect = (base.GetControl("Label_protect") as Label);
		this.m_lbSkillReinforceRate = (base.GetControl("Label_rate") as Label);
		this.m_lbSkillReinforceNowSkilllevel = (base.GetControl("Label_skill1") as Label);
		this.m_lbSkillReinforceNextSkillLevel = (base.GetControl("Label_skill2") as Label);
		this.m_cbSkillReinforceProtect = (base.GetControl("CheckBox_protect") as CheckBox);
		base.SetShowLayer(4, false);
		if (null != this.m_InvenItemListBox && null != this.m_SolListBox)
		{
			this.SetColumFromShowType();
		}
		this.SetData();
		this.InitSlots();
		int itemCnt = NkUserInventory.GetInstance().GetItemCnt(50304);
		int itemCnt2 = NkUserInventory.GetInstance().GetItemCnt(50309);
		int itemCnt3 = NkUserInventory.GetInstance().GetItemCnt(50308);
		this.SetMaterialNum(itemCnt, itemCnt2, itemCnt3);
	}

	public void ClickProtectCheckbox(IUIObject obj)
	{
		if (this.m_cbSkillReinforceProtect.StateNum != 1)
		{
			return;
		}
		this.m_cbSkillReinforceProtect.SetCheckState(0);
		if (this.m_SelectItem == null)
		{
			return;
		}
		int itemQuailtyLevel = NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(this.m_SelectItem.m_nItemUnique);
		int itemSkillLevel = this.m_SelectItem.m_nOption[9];
		ITEMSKILLREINFORCE itemskillReinforceData = NrTSingleton<ITEMSKILLREINFORCE_Manager>.Instance.GetItemskillReinforceData(itemQuailtyLevel, itemSkillLevel);
		if (itemskillReinforceData == null)
		{
			return;
		}
		string empty = string.Empty;
		ITEM firstItemByUnique = NkUserInventory.GetInstance().GetFirstItemByUnique(itemskillReinforceData.nProtectitem_Unique);
		if (firstItemByUnique == null || (firstItemByUnique != null && itemskillReinforceData.nProtectitem_num > firstItemByUnique.m_nItemNum))
		{
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemskillReinforceData.nProtectitem_Unique);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("226"),
				"itemname",
				itemNameByItemUnique
			});
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("280"), empty, eMsgType.MB_OK_CANCEL, new YesDelegate(this.On_ItemMall_Yes), null);
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("284"),
				"count",
				this.m_nReinforceProtectItem_Num
			});
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("283"), empty, eMsgType.MB_OK_CANCEL, new YesDelegate(this.On_ProtectCheck_Yes), null);
		}
	}

	public void On_ItemMall_Yes(object a_oObject)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_ORI, true);
	}

	public void On_ProtectCheck_Yes(object a_oObject)
	{
		this.m_cbSkillReinforceProtect.SetCheckState(1);
	}

	private void SetColumFromShowType()
	{
		ItemSkill_Dlg.SHOWTYPE showType = this.m_ShowType;
		if (showType != ItemSkill_Dlg.SHOWTYPE.ITEM)
		{
			if (showType != ItemSkill_Dlg.SHOWTYPE.SOLDER)
			{
			}
		}
	}

	public void SetData()
	{
		ItemSkill_Dlg.SHOWTYPE showType = this.m_ShowType;
		if (showType != ItemSkill_Dlg.SHOWTYPE.ITEM)
		{
			if (showType == ItemSkill_Dlg.SHOWTYPE.SOLDER)
			{
				this.SetSolData();
			}
		}
		else
		{
			this.SetInvItemData();
		}
	}

	private void SetSolData()
	{
		this.MakeSolListAndSort();
		this.m_SolListBox.Clear();
		for (int i = 0; i < this.m_kSolSortList.Count; i++)
		{
			NewListItem item = new NewListItem(this.m_SolListBox.ColumnNum, true, string.Empty);
			this.SetSolColum(i, ref item);
			this.m_SolListBox.Add(item);
		}
		this.m_SolListBox.RepositionItems();
		if (this.m_kSolSortList.Count == 0)
		{
			this.m_EmptyMessage.Visible = true;
		}
		else
		{
			this.m_EmptyMessage.Visible = false;
		}
	}

	private void SetInvItemData()
	{
		bool flag = true;
		this.m_SolID = 0L;
		this.m_InvenItemListBox.Clear();
		this.m_InvenItemList.Clear();
		if (base.p_nSelectIndex == 1)
		{
			for (int i = 1; i <= 4; i++)
			{
				for (int j = 0; j < ItemDefine.INVENTORY_ITEMSLOT_MAX; j++)
				{
					ITEM item = NkUserInventory.GetInstance().GetItem(i, j);
					if (item != null)
					{
						if (item.m_nDurability != 0)
						{
							ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
							if (itemInfo != null)
							{
								if (!itemInfo.IsItemATB(2097152L))
								{
									bool flag2 = false;
									if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
									{
										flag2 = true;
									}
									int num = item.m_nOption[9];
									int num2 = 30;
									if (!flag2)
									{
										num = item.m_nOption[5];
										num2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_NORMAL_ITEMSKILLREINFORCE_MAXLEVEL);
									}
									if (num != 0)
									{
										if (num < num2)
										{
											this.m_InvenItemList.Add(item);
											flag = false;
										}
									}
								}
							}
						}
					}
				}
			}
			if (this.m_InvenItemList.Count > 0)
			{
				this.m_InvenItemList.Sort(new Comparison<ITEM>(this.CompareItemLevel));
				for (int k = 0; k < this.m_InvenItemList.Count; k++)
				{
					NewListItem item2 = new NewListItem(this.m_InvenItemListBox.ColumnNum, true, string.Empty);
					this.SetItemColum(this.m_InvenItemList[k], k, ref item2);
					this.m_InvenItemListBox.Add(item2);
				}
			}
		}
		else
		{
			for (int l = 1; l <= 4; l++)
			{
				for (int m = 0; m < ItemDefine.INVENTORY_ITEMSLOT_MAX; m++)
				{
					ITEM item = NkUserInventory.GetInstance().GetItem(l, m);
					if (item != null)
					{
						if (item.GetRank() == eITEM_RANK_TYPE.ITEM_RANK_SS)
						{
							ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
							if (itemInfo != null)
							{
								if (!itemInfo.IsItemATB(2097152L))
								{
									if (!itemInfo.IsItemATB(131072L) && !itemInfo.IsItemATB(524288L))
									{
										this.m_InvenItemList.Add(item);
										flag = false;
									}
								}
							}
						}
					}
				}
			}
			if (this.m_InvenItemList.Count > 0)
			{
				this.m_InvenItemList.Sort(new Comparison<ITEM>(this.CompareItemLevel));
				for (int n = 0; n < this.m_InvenItemList.Count; n++)
				{
					NewListItem item3 = new NewListItem(this.m_InvenItemListBox.ColumnNum, true, string.Empty);
					this.SetItemColum(this.m_InvenItemList[n], n, ref item3);
					this.m_InvenItemListBox.Add(item3);
				}
			}
		}
		this.m_InvenItemListBox.RepositionItems();
		if (flag)
		{
			this.m_EmptyMessage.Visible = true;
		}
		else
		{
			this.m_EmptyMessage.Visible = false;
		}
	}

	private void SetSolEquipItem()
	{
		if (this.m_SelectSol == null)
		{
			this.m_ShowType = ItemSkill_Dlg.SHOWTYPE.SOLDER;
			return;
		}
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			ITEM item = this.m_SelectSol.GetEquipItemInfo().m_kItem[i].GetItem();
			if (item != null)
			{
				if (item.m_nItemUnique > 0)
				{
					if (NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique) != null)
					{
						if (i == 5)
						{
							if (this.m_SelectSol.IsAtbCommonFlag(2L))
							{
								this.m_txRingslotlock.Visible = false;
							}
							else
							{
								this.m_txRingslotlock.Visible = true;
							}
						}
						NewListItem item2 = new NewListItem(this.m_InvenItemListBox.ColumnNum, true, string.Empty);
						this.SetItemColum(item, num++, ref item2);
						this.m_InvenItemListBox.Add(item2);
					}
				}
			}
		}
		this.m_InvenItemListBox.RepositionItems();
		if (num == 0)
		{
			this.m_EmptyMessage.Visible = true;
		}
		else
		{
			this.m_EmptyMessage.Visible = false;
		}
	}

	private void SetSolColum(int pos, ref NewListItem item)
	{
		if (this.m_kSolSortList.Count <= pos)
		{
			return;
		}
		if (this.m_kSolSortList[pos] == null)
		{
			TsLog.Log("m_kSolSortList[pos] == null", new object[0]);
			return;
		}
		EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(this.m_kSolSortList[pos].GetCharKind(), this.m_kSolSortList[pos].GetGrade());
		if (eventHeroCharCode != null)
		{
			item.SetListItemData(0, "Win_I_EventSol", null, null, null);
			item.SetListItemData(1, this.m_kSolSortList[pos].GetListSolInfo(false), true, null, null);
		}
		else
		{
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(this.m_kSolSortList[pos].GetCharKind(), (int)this.m_kSolSortList[pos].GetGrade());
			if (legendFrame != null)
			{
				item.SetListItemData(0, legendFrame, null, null, null);
			}
			else
			{
				item.SetListItemData(0, true);
			}
			item.SetListItemData(1, this.m_kSolSortList[pos].GetListSolInfo(false), false, null, null);
		}
		item.SetListItemData(2, this.m_kSolSortList[pos].GetName(), null, null, null);
		item.Data = this.m_kSolSortList[pos];
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"count1",
			this.m_kSolSortList[pos].GetLevel(),
			"count2",
			this.m_kSolSortList[pos].GetSolMaxLevel()
		});
		item.SetListItemData(3, textFromInterface, null, null, null);
	}

	private void SetItemColum(ITEM itemdata, int pos, ref NewListItem item)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(itemdata);
		item.SetListItemData(1, itemdata, true, null, null);
		item.SetListItemData(2, rankColorName, null, null, null);
		stringBuilder.Remove(0, stringBuilder.Length);
		if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(itemdata.m_nItemUnique) == eITEM_PART.ITEMPART_WEAPON)
		{
			int nValue = Protocol_Item.Get_Min_Damage(itemdata);
			int optionValue = Tooltip_Dlg.GetOptionValue(itemdata, nValue, 1);
			int nValue2 = Protocol_Item.Get_Max_Damage(itemdata);
			int optionValue2 = Tooltip_Dlg.GetOptionValue(itemdata, nValue2, 1);
			stringBuilder.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("242") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString(), " ~ ", optionValue2.ToString()));
		}
		else if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(itemdata.m_nItemUnique) == eITEM_PART.ITEMPART_ARMOR)
		{
			int nValue = Protocol_Item.Get_Defense(itemdata);
			int optionValue = Tooltip_Dlg.GetOptionValue(itemdata, nValue, 2);
			stringBuilder.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("243") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString()));
		}
		item.SetListItemData(3, stringBuilder.ToString(), null, null, null);
		item.Data = itemdata;
	}

	private void BtnShowEffectHelpSol(IUIObject obj)
	{
	}

	public void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_ShowType = (ItemSkill_Dlg.SHOWTYPE)uIPanelTab.panel.index;
		if (this.m_ShowType == ItemSkill_Dlg.SHOWTYPE.SOLDER)
		{
			base.SetShowLayer(4, true);
			base.SetShowLayer(3, false);
		}
		else
		{
			base.SetShowLayer(4, false);
			base.SetShowLayer(3, true);
		}
		this.ItemSlotClear();
		this.InitSlots();
		this.EquipSlotClear();
		this.initItemSkillReinforceData();
		this.SetColumFromShowType();
		this.SetData();
	}

	public void OnChangeSortSolList(IUIObject obj)
	{
		this.m_nSearch_SolPosType = 1;
		if (this.m_Droplist.Count > 0 && this.m_Droplist.SelectedItem != null)
		{
			ListItem listItem = this.m_Droplist.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.m_nSearch_SolPosType = (byte)listItem.Key;
				if (this.m_nSearch_SolPosType == 2 || this.m_nSearch_SolPosType == 6)
				{
					this.m_byMilityUnique = (byte)this.m_Droplist.SelectIndex;
				}
			}
		}
		this.ItemSlotClear();
		this.InitSlots();
		this.initItemSkillReinforceData();
		this.EquipSlotClear();
		this.SetData();
	}

	public void OnSolClick(IUIObject obj)
	{
		if (this.m_ShowType == ItemSkill_Dlg.SHOWTYPE.SOLDER)
		{
			this.InitSlots();
			this.ItemSlotClear();
			this.initItemSkillReinforceData();
			NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)this.m_SolListBox.SelectedItem.Data;
			if (nkSoldierInfo != null)
			{
				this.m_SolID = nkSoldierInfo.GetSolID();
				this.m_SelectSol = nkSoldierInfo;
				this.SolEquipItem();
			}
		}
	}

	public void OnItemClick(IUIObject obj)
	{
		if (this.m_ShowType == ItemSkill_Dlg.SHOWTYPE.ITEM || this.m_ShowType == ItemSkill_Dlg.SHOWTYPE.SOLITEM)
		{
			if (null == this.m_InvenItemListBox.SelectedItem)
			{
				return;
			}
			ITEM iTEM = (ITEM)this.m_InvenItemListBox.SelectedItem.Data;
			if (iTEM != null)
			{
				this.m_SelectItem = iTEM;
				this.m_SelectItemSol = this.GetSolIID();
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMSKILL_RESULT_DLG);
				if (base.p_nSelectIndex == 1)
				{
					this.SendItemSkillReinforceSet(this.m_SelectItem);
				}
				else
				{
					this.SendItemSet(this.m_SelectItem);
				}
			}
		}
	}

	public void OnClickCancle(IUIObject obj)
	{
		this.m_ShowType = ItemSkill_Dlg.SHOWTYPE.SOLDER;
		this.m_SelectSol = null;
		this.SetColumFromShowType();
		this.SetData();
	}

	public void OnItemSkillReinforceConfirm(IUIObject obj)
	{
		if (this.m_SelectItem == null)
		{
			return;
		}
		int itemQuailtyLevel = NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(this.m_SelectItem.m_nItemUnique);
		int num = this.m_SelectItem.m_nOption[9];
		bool flag = false;
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_SelectItem.m_nItemUnique);
		if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
		{
			flag = true;
		}
		if (!flag)
		{
			num = this.m_SelectItem.m_nOption[5];
		}
		ITEMSKILLREINFORCE itemskillReinforceData = NrTSingleton<ITEMSKILLREINFORCE_Manager>.Instance.GetItemskillReinforceData(itemQuailtyLevel, num);
		if (itemskillReinforceData == null)
		{
			return;
		}
		string empty = string.Empty;
		bool flag2 = true;
		ITEM firstItemByUnique = NkUserInventory.GetInstance().GetFirstItemByUnique(itemskillReinforceData.nNeedItem_Unique[0]);
		if (firstItemByUnique == null || (firstItemByUnique != null && itemskillReinforceData.nNeedItem_num[0] > firstItemByUnique.m_nItemNum))
		{
			ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.REINFORCE_NEED_ITEM_UNIQUE_1);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(itemInfo2.m_strTextKey)
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			flag2 = false;
		}
		if (flag)
		{
			firstItemByUnique = NkUserInventory.GetInstance().GetFirstItemByUnique(itemskillReinforceData.nNeedItem_Unique[1]);
			if (firstItemByUnique == null || (firstItemByUnique != null && itemskillReinforceData.nNeedItem_num[1] > firstItemByUnique.m_nItemNum))
			{
				ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.REINFORCE_NEED_ITEM_UNIQUE_2);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(itemInfo2.m_strTextKey)
				});
				Main_UI_SystemMessage.ADDMessage(empty);
				flag2 = false;
			}
		}
		if (this.m_cbSkillReinforceProtect.Visible && this.m_cbSkillReinforceProtect.IsChecked())
		{
			firstItemByUnique = NkUserInventory.GetInstance().GetFirstItemByUnique(itemskillReinforceData.nProtectitem_Unique);
			if (firstItemByUnique == null || (firstItemByUnique != null && itemskillReinforceData.nProtectitem_num > firstItemByUnique.m_nItemNum))
			{
				ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.REINFORCE_PROTECT_ITEM_UNIQUE_3);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(itemInfo2.m_strTextKey)
				});
				Main_UI_SystemMessage.ADDMessage(empty);
				flag2 = false;
			}
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if ((long)itemskillReinforceData.nNeedGold > myCharInfo.m_Money)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676")
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			flag2 = false;
		}
		if (!flag2)
		{
			return;
		}
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ITEMSKILLREINFORCE_DOWN);
		int value2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ITEMSKILLREINFORCE_DESTORY);
		int value3 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ITEMSKILLREINFORCE_MAXLEVEL);
		if (!flag)
		{
			value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_NORMAL_ITEMSKILLREINFORCE_DOWN);
			value2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_NORMAL_ITEMSKILLREINFORCE_DESTORY);
			value3 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_NORMAL_ITEMSKILLREINFORCE_MAXLEVEL);
		}
		string empty2 = string.Empty;
		if (num < value)
		{
			this.ActionItemSkill();
		}
		else if (this.m_cbSkillReinforceProtect.IsChecked())
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("284"),
				"count",
				this.m_nReinforceProtectItem_Num
			});
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("280"), empty2, eMsgType.MB_OK_CANCEL, new YesDelegate(this.On_MessageBok_OK), null);
		}
		else if (num >= value && num < value2)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("281")
			});
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("280"), empty2, eMsgType.MB_OK_CANCEL, new YesDelegate(this.On_MessageBok_OK), null);
		}
		else if (num >= value2 && num < value3)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("282")
			});
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("280"), empty2, eMsgType.MB_OK_CANCEL, new YesDelegate(this.On_MessageBok_OK), null);
		}
	}

	public void OnItemConfirm(IUIObject obj)
	{
		if (this.m_SelectItem == null)
		{
			return;
		}
		string empty = string.Empty;
		ITEM firstItemByUnique = NkUserInventory.GetInstance().GetFirstItemByUnique(this.m_RequestMaterialUnique);
		this.m_pMaterialItem = firstItemByUnique;
		if (this.m_pMaterialItem == null || this.m_pMaterialItem.m_nItemNum < this.m_RequestMaterialNum)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromItem("50304")
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if ((long)this.m_RequestGold > myCharInfo.m_Money)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676")
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			return;
		}
		string empty2 = string.Empty;
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(this.m_SelectItem);
		string rankColorName2 = NrTSingleton<ItemManager>.Instance.GetRankColorName(this.m_pMaterialItem);
		if (this.m_cbEnhance.Visible && this.m_cbEnhance.IsChecked() && this.m_pMaterialItem != null)
		{
			ITEM firstItemByUnique2 = NkUserInventory.GetInstance().GetFirstItemByUnique(this.m_RequestEnhanceUnique);
			if (firstItemByUnique2 == null || firstItemByUnique2.m_nItemNum < this.m_RequestEnhanceNum)
			{
				LackGold_dlg lackGold_dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GOLDLACK_DLG) as LackGold_dlg;
				if (lackGold_dlg != null && !lackGold_dlg.SetDataShopItem(this.m_RequestEnhanceUnique, eITEMMALL_TYPE.BUY_ORI))
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
						"targetname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_RequestEnhanceUnique)
					});
					Main_UI_SystemMessage.ADDMessage(empty);
				}
				return;
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("228"),
				"itemname1",
				rankColorName2,
				"itemnum1",
				this.m_RequestMaterialNum,
				"itemname2",
				NrTSingleton<ItemManager>.Instance.GetRankColorName(firstItemByUnique2),
				"itemnum2",
				this.m_RequestEnhanceNum
			});
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("227"), empty2, eMsgType.MB_OK_CANCEL, new YesDelegate(this.On_MessageBok_OK), null);
		}
		else if (this.m_pMaterialItem != null)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("98"),
				"targetname1",
				rankColorName,
				"targetname",
				rankColorName2,
				"count1",
				this.m_RequestMaterialNum,
				"count2",
				this.m_RequestGold
			});
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("996"), empty2, eMsgType.MB_OK_CANCEL, new YesDelegate(this.On_MessageBok_OK), null);
		}
	}

	private void On_EquipMouse_Over(IUIObject a_oObject)
	{
		ImageView imageView = a_oObject as ImageView;
		if (imageView != null)
		{
			IUIListObject iUIListObject;
			if (TsPlatform.IsMobile)
			{
				iUIListObject = imageView.SelectedItem;
			}
			else
			{
				iUIListObject = imageView.MouseItem;
			}
			if (iUIListObject != null)
			{
				ImageSlot imageSlot = iUIListObject.Data as ImageSlot;
				if (imageSlot != null && imageSlot.c_oItem != null)
				{
					ITEM iTEM = new ITEM();
					iTEM.Set(imageSlot.c_oItem as ITEM);
					if (iTEM != null && iTEM.m_nItemUnique > 0)
					{
						ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
						if (itemTooltipDlg != null)
						{
							itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
						}
					}
				}
			}
		}
	}

	private void On_EquipMouse_Out(IUIObject a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
	}

	private void On_EquipMouse_Click(IUIObject a_oObject)
	{
		if (this.m_SelectSol == null)
		{
			return;
		}
		UIScrollList uIScrollList = a_oObject as UIScrollList;
		if (uIScrollList != null)
		{
			UIListItemContainer selectedItem = uIScrollList.SelectedItem;
			if (selectedItem != null)
			{
				ImageSlot imageSlot = selectedItem.Data as ImageSlot;
				if (imageSlot != null)
				{
					if (!(imageSlot.c_oItem is ITEM))
					{
						return;
					}
					bool flag = false;
					ITEM equipItem = this.m_SelectSol.GetEquipItem(imageSlot.Index);
					if (equipItem != null && equipItem.IsValid())
					{
						flag = true;
					}
					if (flag)
					{
						this.m_SelectItem = equipItem;
						this.m_SelectItemSol = this.GetSolIID();
						if (base.p_nSelectIndex == 1)
						{
							this.SendItemSkillReinforceSet(this.m_SelectItem);
						}
						else
						{
							this.SendItemSet(this.m_SelectItem);
						}
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("557"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					}
				}
			}
		}
	}

	private void On_Mouse_Over(IUIObject a_oObject)
	{
		ImageView imageView = a_oObject as ImageView;
		if (imageView != null)
		{
			IUIListObject mouseItem = imageView.MouseItem;
			if (mouseItem != null)
			{
				ImageSlot imageSlot = mouseItem.Data as ImageSlot;
				if (imageSlot != null && imageSlot.c_oItem != null)
				{
					ITEM iTEM = new ITEM();
					iTEM.Set(imageSlot.c_oItem as ITEM);
					if (iTEM != null && iTEM.m_nItemUnique > 0)
					{
						ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
						if (itemTooltipDlg != null)
						{
							itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
						}
					}
				}
			}
		}
	}

	private void On_Mouse_Out(IUIObject a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
	}

	private void On_Mouse_Click(IUIObject a_oObject)
	{
		this.ItemSlotClear();
		this.InitSlots();
	}

	private void On_MessageBok_OK(object a_oObject)
	{
		if (this.m_SelectItem != null)
		{
			this.ActionItemSkill();
		}
	}

	private void MakeBattleSolList()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			this.AddSolList(soldierInfo, eSOL_POSTYPE.SOLPOS_BATTLE);
		}
	}

	private void MakeReadySolList()
	{
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			this.AddSolList(current, eSOL_POSTYPE.SOLPOS_READY);
		}
	}

	private void MakeMilitarySolList(int militaryunique)
	{
		if (militaryunique <= 0)
		{
			return;
		}
		NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
		if (militaryList == null)
		{
			return;
		}
		NkMineMilitaryInfo mineMilitaryInfo = militaryList.GetMineMilitaryInfo((byte)militaryunique);
		if (mineMilitaryInfo == null)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo solInfo = mineMilitaryInfo.GetSolInfo(i);
			this.AddSolList(solInfo, eSOL_POSTYPE.SOLPOS_MINE_MILITARY);
		}
	}

	private void MakeMilitarySolList()
	{
		NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
		if (militaryList != null)
		{
			for (int i = 0; i < 10; i++)
			{
				byte militaryunique = (byte)(i + 1);
				NkMineMilitaryInfo mineMilitaryInfo = militaryList.GetMineMilitaryInfo(militaryunique);
				if (mineMilitaryInfo != null && mineMilitaryInfo.IsValid())
				{
					this.MakeMilitarySolList((int)militaryunique);
				}
			}
		}
	}

	private void AddSolList(NkSoldierInfo pkSolinfo, eSOL_POSTYPE eAddPosType)
	{
		if (pkSolinfo == null || !pkSolinfo.IsValid())
		{
			return;
		}
		if (eAddPosType == eSOL_POSTYPE.SOLPOS_BATTLE)
		{
			if (pkSolinfo.GetSolPosType() != (byte)eAddPosType)
			{
				return;
			}
		}
		else if (pkSolinfo.GetSolPosType() != 0 && pkSolinfo.GetSolPosType() != 2 && pkSolinfo.GetSolPosType() != 6)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(pkSolinfo);
		this.m_kSolList.Add(pkSolinfo);
	}

	private void MakeSolListAndSort()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		this.m_kSolList.Clear();
		this.m_kSolSortList.Clear();
		byte nSearch_SolPosType = this.m_nSearch_SolPosType;
		switch (nSearch_SolPosType)
		{
		case 0:
			this.MakeReadySolList();
			goto IL_B1;
		case 1:
			this.MakeBattleSolList();
			goto IL_B1;
		case 2:
		case 6:
			this.MakeMilitarySolList((int)this.m_byMilityUnique);
			goto IL_B1;
		case 3:
		case 4:
		case 5:
			IL_54:
			if (nSearch_SolPosType != 100)
			{
				goto IL_B1;
			}
			if (this.m_nSearch_SolSortType == 1)
			{
				this.MakeBattleSolList();
			}
			else
			{
				this.MakeBattleSolList();
				this.MakeReadySolList();
			}
			goto IL_B1;
		}
		goto IL_54;
		IL_B1:
		switch (this.m_nSearch_SolSortType)
		{
		case 1:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.ComparePosIndex));
			break;
		case 2:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareName));
			break;
		case 3:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevel));
			break;
		case 4:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareCombatPower));
			break;
		case 5:
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.ComparePosIndex));
			break;
		}
		if (this.m_nSearch_SolPosType == 100)
		{
			NkSoldierInfo leaderSoldierInfo = charPersonInfo.GetLeaderSoldierInfo();
			if (leaderSoldierInfo != null)
			{
				for (int i = 0; i < this.m_kSolList.Count; i++)
				{
					if (leaderSoldierInfo.GetSolID() == this.m_kSolList[i].GetSolID())
					{
						this.m_kSolSortList.Add(this.m_kSolList[i]);
						this.m_kSolList.Remove(this.m_kSolSortList[0]);
						break;
					}
				}
			}
		}
		for (int j = 0; j < this.m_kSolList.Count; j++)
		{
			this.m_kSolSortList.Add(this.m_kSolList[j]);
		}
	}

	private int ComparePosIndex(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetSolPosIndex().CompareTo(b.GetSolPosIndex());
	}

	private int CompareName(NkSoldierInfo a, NkSoldierInfo b)
	{
		if (a.GetName().Equals(b.GetName()))
		{
			return this.CompareLevel(a, b);
		}
		return a.GetName().CompareTo(b.GetName());
	}

	private int CompareLevel(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetLevel().CompareTo(b.GetLevel());
	}

	private int CompareCombatPower(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetCombatPower().CompareTo(a.GetCombatPower());
	}

	private int CompareItemLevel(ITEM a, ITEM b)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(a.m_nItemUnique);
		ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(b.m_nItemUnique);
		if (itemInfo.m_nQualityLevel != itemInfo2.m_nQualityLevel)
		{
			return -itemInfo.m_nQualityLevel.CompareTo(itemInfo2.m_nQualityLevel);
		}
		if (a.GetRank() != b.GetRank())
		{
			return -a.GetRank().CompareTo(b.GetRank());
		}
		int useMinLevel = NrTSingleton<ItemManager>.Instance.GetUseMinLevel(a);
		int useMinLevel2 = NrTSingleton<ItemManager>.Instance.GetUseMinLevel(b);
		if (useMinLevel == useMinLevel2)
		{
			return a.m_nItemUnique.CompareTo(b.m_nItemUnique);
		}
		return -useMinLevel.CompareTo(useMinLevel2);
	}

	public void SendItemSkillReinforceSet(ITEM srcItem)
	{
		this.m_ItemSlot.Clear();
		ImageSlot imageSlot = new ImageSlot();
		if (srcItem != null)
		{
			imageSlot.c_oItem = srcItem;
			imageSlot.c_bEnable = true;
			imageSlot.Index = 0;
			imageSlot.itemunique = srcItem.m_nItemUnique;
			imageSlot._solID = this.m_SolID;
			imageSlot.WindowID = base.WindowID;
			if (srcItem.m_nItemNum > 1)
			{
				imageSlot.SlotInfo._visibleNum = true;
			}
			imageSlot.SlotInfo._visibleRank = true;
			imageSlot.SlotInfo.Set(string.Empty, "+ " + srcItem.m_nRank.ToString());
			this.m_ItemSlot.SetImageSlot(0, imageSlot, null, null, null, null);
			this.m_ItemSlot.RepositionItems();
			string name = NrTSingleton<ItemManager>.Instance.GetName(srcItem);
			this.m_lbItemName.SetText(name);
			this.m_lbItemName.Visible = true;
			this.m_lbGuidText.Visible = false;
			this.m_SelectItem = srcItem;
			this.m_txItemSlotBG.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString((int)srcItem.GetRank()));
			this.CheckSkillReinforceNeedItem();
			return;
		}
		this.ItemSlotClear();
		this.initItemSkillReinforceData();
	}

	private void CheckSkillReinforceNeedItem()
	{
		if (this.m_SelectItem == null)
		{
			return;
		}
		int itemQuailtyLevel = NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(this.m_SelectItem.m_nItemUnique);
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_SelectItem.m_nItemUnique);
		bool flag = false;
		if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
		{
			flag = true;
		}
		int num = this.m_SelectItem.m_nOption[9];
		int skillUnique = this.m_SelectItem.m_nOption[6];
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ITEMSKILLREINFORCE_DOWN);
		if (!flag)
		{
			num = this.m_SelectItem.m_nOption[5];
			skillUnique = this.m_SelectItem.m_nOption[4];
			value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_NORMAL_ITEMSKILLREINFORCE_DOWN);
		}
		ITEMSKILLREINFORCE itemskillReinforceData = NrTSingleton<ITEMSKILLREINFORCE_Manager>.Instance.GetItemskillReinforceData(itemQuailtyLevel, num);
		if (itemskillReinforceData != null)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1721"),
				"gold",
				ANNUALIZED.Convert(itemskillReinforceData.nNeedGold)
			});
			this.m_lbSkillReinforceGold.SetText(empty);
			empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
				"Count",
				itemskillReinforceData.nNeedItem_num[0]
			});
			this.m_lbSkillReinforceNeedItem1.SetText(empty);
			empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
				"Count",
				itemskillReinforceData.nNeedItem_num[1]
			});
			this.m_lbSkillReinforceNeedItem2.SetText(empty);
			empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672"),
				"Count",
				100 - itemskillReinforceData.nFailrate
			});
			this.m_lbSkillReinforceRate.SetText(empty);
			if (num >= value)
			{
				empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2956"),
					"count",
					itemskillReinforceData.nProtectitem_num
				});
				this.m_lbSkillReinforceProtect.SetText(empty);
				this.m_lbSkillReinforceProtect.Visible = true;
				this.m_nReinforceProtectItem_Num = itemskillReinforceData.nProtectitem_num;
				this.m_cbSkillReinforceProtect.Visible = true;
			}
			else
			{
				this.m_lbSkillReinforceProtect.Visible = false;
				this.m_cbSkillReinforceProtect.SetCheckState(0);
				this.m_cbSkillReinforceProtect.Visible = false;
			}
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
			string strTextKey = battleSkillBase.m_strTextKey;
			if (battleSkillBase != null)
			{
				empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2670"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey),
					"skilllevel",
					num
				});
				this.m_lbSkillReinforceNowSkilllevel.SetText(empty);
				this.m_lbSkillReinforceNowSkilllevel.Visible = true;
				empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2670"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey),
					"skilllevel",
					num + 1
				});
				this.m_lbSkillReinforceNextSkillLevel.SetText(empty);
				this.m_lbSkillReinforceNextSkillLevel.Visible = true;
			}
		}
	}

	public void SendItemSet(ITEM srcItem)
	{
		if (srcItem != null)
		{
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(srcItem.m_nItemUnique);
			if (base.p_nSelectIndex == 1)
			{
				if (itemInfo != null && !this.IsCanReinforce(srcItem, itemInfo))
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("261"));
					this.ItemSlotClear();
					this.InitSlots();
					return;
				}
			}
			else if (itemInfo != null && (srcItem.GetRank() != eITEM_RANK_TYPE.ITEM_RANK_SS || itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L)))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("261"));
				this.ItemSlotClear();
				this.InitSlots();
				return;
			}
		}
		base.SetShowLayer(2, true);
		this.m_ItemSlot.Clear();
		ImageSlot imageSlot = new ImageSlot();
		if (srcItem != null)
		{
			imageSlot.c_oItem = srcItem;
			imageSlot.c_bEnable = true;
			imageSlot.Index = 0;
			imageSlot.itemunique = srcItem.m_nItemUnique;
			imageSlot._solID = this.m_SolID;
			imageSlot.WindowID = base.WindowID;
			if (srcItem.m_nItemNum > 1)
			{
				imageSlot.SlotInfo._visibleNum = true;
			}
			imageSlot.SlotInfo._visibleRank = true;
			imageSlot.SlotInfo.Set(string.Empty, "+ " + srcItem.m_nRank.ToString());
			this.m_ItemSlot.SetImageSlot(0, imageSlot, null, null, null, null);
			this.m_ItemSlot.RepositionItems();
			string name = NrTSingleton<ItemManager>.Instance.GetName(srcItem);
			this.m_lbItemName.SetText(name);
			this.m_lbItemName.Visible = true;
			this.m_lbGuidText.Visible = false;
			this.m_SelectItem = srcItem;
			this.m_txItemSlotBG.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString((int)srcItem.GetRank()));
			this.PushMaterialSlot();
			ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_SelectItem.m_nItemUnique);
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			if (itemInfo2 != null && instance != null)
			{
				int nUseMinLevel = itemInfo2.m_nUseMinLevel;
				int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ITEM_TRADE_LIMIT);
				int value2 = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TRADECOUNT_USE);
				if (value >= 0 && nUseMinLevel <= value && value2 == 1)
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1857"),
						"level",
						nUseMinLevel
					});
					this.m_lbTradeConfirm.Text = empty;
					this.m_lbTradeConfirm.Visible = true;
					this.m_txLabelBG.Visible = true;
				}
				else
				{
					this.m_lbTradeConfirm.Visible = false;
					this.m_txLabelBG.Visible = false;
				}
			}
			if (base.p_nSelectIndex == 0)
			{
				this.m_txItemIcon1.Visible = true;
				this.m_txItemIcon2.Visible = true;
			}
			return;
		}
		this.ItemSlotClear();
	}

	public void ItemSlotClear()
	{
		this.m_ItemSlot.Clear();
		this.m_SelectItem = null;
		ImageSlot imageSlot = new ImageSlot();
		imageSlot.c_oItem = null;
		imageSlot.Index = 0;
		imageSlot.imageStr = "Com_I_Transparent";
		imageSlot.WindowID = base.WindowID;
		imageSlot.itemunique = 0;
		imageSlot.SlotInfo.Set(string.Empty, string.Empty);
		this.m_ItemSlot.SetImageSlot(0, imageSlot, null, null, null, null);
		this.m_ItemSlot.RepositionItems();
		this.m_lbGuidText.Visible = true;
		this.m_lbItemName.Visible = false;
		this.m_txRingslotlock.Visible = false;
		base.SetShowLayer(2, false);
		if (base.p_nSelectIndex == 0)
		{
			this.m_txItemIcon1.Visible = true;
			this.m_txItemIcon2.Visible = true;
		}
	}

	public void EquipSlotClear()
	{
		this.m_SolID = 0L;
		for (int i = 0; i < 6; i++)
		{
			if (!(this.m_EquipItemSlot[i] == null))
			{
				this.m_EquipItemSlot[i].Clear();
				ImageSlot imageSlot = new ImageSlot();
				imageSlot.c_oItem = null;
				imageSlot.Index = i;
				imageSlot._solID = 0L;
				imageSlot.WindowID = base.WindowID;
				imageSlot.SlotInfo.Set(string.Empty, string.Empty);
				this.m_EquipItemSlot[i].SetImageSlot(0, imageSlot, null, null, null, null);
				this.m_EquipItemSlot[i].RepositionItems();
			}
		}
	}

	private void MaterialSlotClear()
	{
		this.m_lbMaterialName.SetText(string.Empty);
		this.m_lbRequestMaterial.SetText(string.Empty);
		ImageSlot imageSlot = new ImageSlot();
		imageSlot.c_oItem = null;
		imageSlot.Index = 0;
		imageSlot._solID = 0L;
		imageSlot.WindowID = base.WindowID;
		imageSlot.SlotInfo.Set(string.Empty, string.Empty);
		this.m_ItemMaterialSlot.SetImageSlot(0, imageSlot, null, null, null, null);
		this.m_ItemMaterialSlot.RepositionItems();
	}

	private void SolEquipItem()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(this.m_SolID);
			if (soldierInfoFromSolID != null)
			{
				for (int i = 0; i < 6; i++)
				{
					if (!(this.m_EquipItemSlot[i] == null))
					{
						this.m_EquipItemSlot[i].Clear();
						ImageSlot imageSlot = new ImageSlot();
						ITEM equipItem = soldierInfoFromSolID.GetEquipItem(i);
						ITEMINFO iTEMINFO = null;
						bool flag = false;
						bool flag2 = false;
						if (equipItem != null)
						{
							iTEMINFO = NrTSingleton<ItemManager>.Instance.GetItemInfo(equipItem.m_nItemUnique);
							if (iTEMINFO != null && (iTEMINFO.IsItemATB(131072L) || iTEMINFO.IsItemATB(524288L) || iTEMINFO.IsItemATB(2097152L)))
							{
								flag = true;
							}
						}
						if (equipItem != null && equipItem.m_nItemID != 0L)
						{
							if (base.p_nSelectIndex == 1)
							{
								flag2 = this.IsCanReinforce(equipItem, iTEMINFO);
							}
							else if (base.p_nSelectIndex != 1 && !flag)
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							imageSlot.c_oItem = equipItem;
							imageSlot.c_bEnable = true;
							imageSlot.Index = i;
							imageSlot.itemunique = equipItem.m_nItemUnique;
							imageSlot._solID = soldierInfoFromSolID.GetSolID();
							imageSlot.WindowID = base.WindowID;
							if (equipItem.m_nItemNum > 1)
							{
								imageSlot.SlotInfo._visibleNum = true;
							}
							imageSlot.SlotInfo._visibleRank = true;
							imageSlot.SlotInfo.Set(string.Empty, "+ " + equipItem.m_nRank.ToString());
						}
						else
						{
							imageSlot.c_oItem = null;
							imageSlot.Index = i;
							imageSlot._solID = soldierInfoFromSolID.GetSolID();
							imageSlot.WindowID = base.WindowID;
							imageSlot.SlotInfo.Set(string.Empty, string.Empty);
						}
						this.m_EquipItemSlot[i].SetImageSlot(0, imageSlot, null, null, null, null);
						this.m_EquipItemSlot[i].RepositionItems();
					}
				}
			}
		}
	}

	private void PushMaterialSlot()
	{
		this.InitSlots();
		if (this.m_SelectItem == null)
		{
			return;
		}
		int itemQuailtyLevel = NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(this.m_SelectItem.m_nItemUnique);
		ITEM_REFORGE itemReforgeData = NrTSingleton<ITEM_REFORGE_Manager>.Instance.GetItemReforgeData(itemQuailtyLevel, (int)this.m_SelectItem.GetRank());
		if (itemReforgeData == null)
		{
			Main_UI_SystemMessage.ADDMessage("INVALID RANK DATA");
			return;
		}
		this.m_RequestMaterialUnique = itemReforgeData.nReforgeItemUnique;
		this.m_RequestMaterialNum = itemReforgeData.nReforgeItemNum;
		this.m_lbMaterialName.SetText(NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_RequestMaterialUnique));
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
			"Count",
			ANNUALIZED.Convert(itemReforgeData.nReforgeItemNum)
		});
		this.m_lbRequestMaterial.SetText(empty);
		this.m_RequestGold = itemReforgeData.nReforgeGold;
		this.m_ItemMaterialSlot.Clear();
		ImageSlot imageSlot = new ImageSlot();
		imageSlot.c_bEnable = true;
		imageSlot.Index = 0;
		imageSlot.itemunique = this.m_RequestMaterialUnique;
		imageSlot._solID = this.m_SolID;
		imageSlot.WindowID = base.WindowID;
		imageSlot.SlotInfo._visibleRank = true;
		this.m_ItemMaterialSlot.SetImageSlot(0, imageSlot, null, null, null, null);
		this.m_ItemMaterialSlot.RepositionItems();
		if (itemReforgeData.nEnhanceUnique > 0)
		{
			this.m_RequestEnhanceUnique = itemReforgeData.nEnhanceUnique;
			this.m_RequestEnhanceNum = itemReforgeData.nEnhancenum;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2671"),
				"itemname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemReforgeData.nEnhanceUnique),
				"itemnum",
				ANNUALIZED.Convert(itemReforgeData.nEnhancenum)
			});
			this.m_lbEnhance.SetText(empty);
			this.m_lbEnhance.Visible = true;
			this.m_cbEnhance.Visible = true;
		}
		else
		{
			this.m_RequestEnhanceUnique = 0;
			this.m_RequestEnhanceNum = 0;
			this.m_lbEnhance.Visible = false;
			this.m_cbEnhance.Visible = false;
		}
	}

	public void RefrshData()
	{
		if (this.m_ShowType == ItemSkill_Dlg.SHOWTYPE.SOLDER)
		{
			this.SolEquipItem();
		}
		if (this.m_SelectItem == null)
		{
			if (base.p_nSelectIndex == 1)
			{
				this.SendItemSkillReinforceSet(null);
			}
			else
			{
				this.SendItemSet(null);
			}
		}
		else
		{
			ITEM item = NkUserInventory.GetInstance().GetItem(this.m_SelectItem.m_nPosType, this.m_SelectItem.m_nItemPos);
			if (item == null)
			{
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser == null)
				{
					return;
				}
				NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(this.m_SelectItemSol);
				if (soldierInfoFromSolID != null)
				{
					item = soldierInfoFromSolID.GetEquipItemInfo().GetItem(this.m_SelectItem.m_nItemPos);
				}
			}
			if (base.p_nSelectIndex == 1)
			{
				if (item.m_nDurability > 0)
				{
					this.m_SelectItem = item;
				}
				else
				{
					this.m_SelectItem = null;
				}
				this.SendItemSkillReinforceSet(this.m_SelectItem);
			}
			else
			{
				this.m_SelectItem = item;
				this.SendItemSet(this.m_SelectItem);
			}
		}
	}

	private void InitSlots()
	{
		this.m_lbMaterialName.SetText(string.Empty);
		this.m_lbRequestMaterial.SetText(string.Empty);
		this.m_pMaterialItem = null;
		this.m_RequestMaterialNum = 0;
		this.MaterialSlotClear();
	}

	public long GetSolIID()
	{
		return this.m_SolID;
	}

	public long GetItemSelectSolID()
	{
		return this.m_SelectItemSol;
	}

	public void ActionItemSkill()
	{
		if (!this.bRequest)
		{
			string str = string.Format("{0}", "UI/Item/fx_reinforce_magic" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetActionItemSkill), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			this.bRequest = true;
		}
	}

	private void SetActionItemSkill(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = base.GetLocation().z - 300f;
				this.rootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.rootGameObject, GUICamera.UILayer);
				base.InteractivePanel.MakeChild(this.rootGameObject);
				this.fStartTime = Time.time;
				this.bLoadActionItemSkill = true;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootGameObject);
				}
			}
			TsAudio.StoreMuteAllAudio();
			TsAudio.SetExceptMuteAllAudio(EAudioType.UI, true);
			TsAudio.RefreshAllMuteAudio();
			NkInputManager.IsInputMode = false;
		}
	}

	public void initItemSkillReinforceData()
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1721"),
			"gold",
			0
		});
		this.m_lbSkillReinforceGold.SetText(empty);
		empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
			"Count",
			0
		});
		this.m_lbSkillReinforceNeedItem1.SetText(empty);
		this.m_lbSkillReinforceNeedItem2.SetText(empty);
		empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2956"),
			"count",
			0
		});
		this.m_lbSkillReinforceProtect.SetText(empty);
		this.m_lbSkillReinforceProtect.Visible = false;
		empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672"),
			"Count",
			0
		});
		this.m_lbSkillReinforceRate.SetText(empty);
		this.m_lbSkillReinforceNowSkilllevel.Visible = false;
		this.m_lbSkillReinforceNextSkillLevel.Visible = false;
		this.m_cbSkillReinforceProtect.Clear();
		this.m_cbSkillReinforceProtect.Visible = false;
		if (this.m_ShowType == ItemSkill_Dlg.SHOWTYPE.ITEM)
		{
			this.SetInvItemData();
			this.m_InvenItemListBox.Visible = true;
			this.m_SolListBox.Visible = false;
		}
		else
		{
			this.m_InvenItemListBox.Visible = false;
			this.m_SolListBox.Visible = true;
		}
	}

	public void SetItemSkillReinforceShow()
	{
		int itemCnt = NkUserInventory.GetInstance().GetItemCnt(this.REINFORCE_NEED_ITEM_UNIQUE_1);
		int itemCnt2 = NkUserInventory.GetInstance().GetItemCnt(this.REINFORCE_NEED_ITEM_UNIQUE_2);
		int itemCnt3 = NkUserInventory.GetInstance().GetItemCnt(this.REINFORCE_PROTECT_ITEM_UNIQUE_3);
		this.SetMaterialNum(itemCnt, itemCnt2, itemCnt3);
		this.m_lbtitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2972"));
		this.m_lbSubtitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2981"));
		this.m_btnConfirm.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2982"));
		this.m_btnConfirm.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnItemConfirm));
		this.m_btnConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemSkillReinforceConfirm));
		this.initItemSkillReinforceData();
	}

	public override void Show()
	{
		base.Show();
		if (base.p_nSelectIndex == 1)
		{
			base.SetShowLayer(2, false);
			base.SetShowLayer(3, false);
			base.SetShowLayer(4, false);
			base.SetShowLayer(5, true);
			this.m_txItemIcon4.Visible = true;
			this.m_txItemIcon5.Visible = true;
			this.m_txItemIcon6.Visible = true;
			this.m_txItemIcon7.Visible = true;
			this.SetItemSkillReinforceShow();
		}
		else
		{
			base.SetShowLayer(5, false);
			this.m_txItemIcon1.Visible = true;
			this.m_txItemIcon2.Visible = true;
		}
	}

	public override void Update()
	{
		if (this.bLoadActionItemSkill && Time.time - this.fStartTime > 3f)
		{
			UnityEngine.Object.DestroyImmediate(this.rootGameObject);
			this.bLoadActionItemSkill = false;
			this.bRequest = false;
			if (base.p_nSelectIndex == 1)
			{
				this.SendItemSkillReinforceServer();
			}
			else
			{
				this.SendServer();
			}
			TsAudio.RestoreMuteAllAudio();
			TsAudio.RefreshAllMuteAudio();
			NkInputManager.IsInputMode = true;
		}
		if (base.Visible && Time.time - this.m_fMaterialNumCheck > 1f)
		{
			int itemCnt;
			int itemCnt2;
			int itemCnt3;
			if (base.p_nSelectIndex == 1)
			{
				itemCnt = NkUserInventory.GetInstance().GetItemCnt(this.REINFORCE_NEED_ITEM_UNIQUE_1);
				itemCnt2 = NkUserInventory.GetInstance().GetItemCnt(this.REINFORCE_NEED_ITEM_UNIQUE_2);
				itemCnt3 = NkUserInventory.GetInstance().GetItemCnt(this.REINFORCE_PROTECT_ITEM_UNIQUE_3);
			}
			else
			{
				itemCnt = NkUserInventory.GetInstance().GetItemCnt(50304);
				itemCnt2 = NkUserInventory.GetInstance().GetItemCnt(50309);
				itemCnt3 = NkUserInventory.GetInstance().GetItemCnt(50308);
			}
			this.SetMaterialNum(itemCnt, itemCnt2, itemCnt3);
		}
	}

	private void SendItemSkillReinforceServer()
	{
		GS_ITEMSKILL_REINFORCE_REQ gS_ITEMSKILL_REINFORCE_REQ = new GS_ITEMSKILL_REINFORCE_REQ();
		gS_ITEMSKILL_REINFORCE_REQ.RepairType = 0;
		gS_ITEMSKILL_REINFORCE_REQ.SrcPosType = this.m_SelectItem.m_nPosType;
		gS_ITEMSKILL_REINFORCE_REQ.SrcItemPos = this.m_SelectItem.m_nItemPos;
		gS_ITEMSKILL_REINFORCE_REQ.SrcItemUnique = this.m_SelectItem.m_nItemUnique;
		gS_ITEMSKILL_REINFORCE_REQ.SolID = this.m_SelectItemSol;
		if (this.m_cbSkillReinforceProtect.IsChecked())
		{
			gS_ITEMSKILL_REINFORCE_REQ.i8UseProtectitem = 1;
		}
		else
		{
			gS_ITEMSKILL_REINFORCE_REQ.i8UseProtectitem = 0;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMSKILL_REINFORCE_REQ, gS_ITEMSKILL_REINFORCE_REQ);
	}

	private void SendServer()
	{
		GS_ENHANCEITEM_REQ gS_ENHANCEITEM_REQ = new GS_ENHANCEITEM_REQ();
		gS_ENHANCEITEM_REQ.nSrcItemUnique = this.m_SelectItem.m_nItemUnique;
		gS_ENHANCEITEM_REQ.nSrcItemPos = this.m_SelectItem.m_nItemPos;
		gS_ENHANCEITEM_REQ.nSrcPosType = this.m_SelectItem.m_nPosType;
		gS_ENHANCEITEM_REQ.SolID = this.m_SelectItemSol;
		gS_ENHANCEITEM_REQ.nReforgeGold = this.m_RequestGold;
		gS_ENHANCEITEM_REQ.UpgradeType = 1;
		if (this.m_cbEnhance.Visible && this.m_cbEnhance.IsChecked())
		{
			gS_ENHANCEITEM_REQ.UpgradeExtraType = 1;
		}
		else
		{
			gS_ENHANCEITEM_REQ.UpgradeExtraType = 0;
		}
		if (this.m_bAgitNPC)
		{
			gS_ENHANCEITEM_REQ.i8AgitNPC = 1;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ENHANCEITEM_REQ, gS_ENHANCEITEM_REQ);
	}

	public override void OnClose()
	{
		NkInputManager.IsInputMode = true;
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
	}

	public void CheckSelectItem()
	{
		if (this.m_SelectItem != null)
		{
			int num = this.m_SelectItem.m_nOption[9];
			int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ITEMSKILLREINFORCE_MAXLEVEL);
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_SelectItem.m_nItemUnique);
			bool flag = false;
			if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
			{
				flag = true;
			}
			if (!flag)
			{
				num = this.m_SelectItem.m_nOption[5];
				value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_NORMAL_ITEMSKILLREINFORCE_MAXLEVEL);
			}
			if (num >= value)
			{
				this.SendItemSkillReinforceSet(null);
			}
		}
	}

	public void UpdateData(int nItemPos, int nItemType, long nItemID = 0L)
	{
		bool flag = false;
		for (int i = 0; i < this.m_InvenItemListBox.Count; i++)
		{
			if (this.m_InvenItemListBox.GetItem(i) != null)
			{
				ITEM iTEM = this.m_InvenItemListBox.GetItem(i).Data as ITEM;
				if (iTEM.m_nItemPos == nItemPos && iTEM.m_nPosType == nItemType)
				{
					ITEM item = NkUserInventory.GetInstance().GetItem(nItemType, nItemPos);
					NewListItem item2 = new NewListItem(this.m_InvenItemListBox.ColumnNum, true, string.Empty);
					this.SetItemColum(item, i, ref item2);
					this.m_InvenItemListBox.UpdateAdd(i, item2);
					flag = true;
				}
			}
		}
		this.m_InvenItemListBox.RepositionItems();
		if (base.p_nSelectIndex == 1)
		{
			this.m_cbSkillReinforceProtect.SetCheckState(0);
		}
		else if (!flag)
		{
			this.SetInvItemData();
		}
	}

	private void SetMaterialNum(int updatenum1, int updatenum2, int updatenum3)
	{
		string empty = string.Empty;
		if (this.m_nMaterial1_Num != updatenum1)
		{
			this.m_nMaterial1_Num = updatenum1;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
				"Count",
				ANNUALIZED.Convert(this.m_nMaterial1_Num)
			});
			this.m_lbHaveMaterial.SetText(empty);
		}
		if (this.m_nMaterial2_Num != updatenum2)
		{
			this.m_nMaterial2_Num = updatenum2;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
				"Count",
				ANNUALIZED.Convert(this.m_nMaterial2_Num)
			});
			this.m_lbHaveMaterial_2.SetText(empty);
		}
		if (this.m_nMaterial3_Num != updatenum3)
		{
			this.m_nMaterial3_Num = updatenum3;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
				"Count",
				ANNUALIZED.Convert(this.m_nMaterial3_Num)
			});
			this.m_lbHaveMaterial_3.SetText(empty);
		}
		if (base.p_nSelectIndex == 1)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			string text = string.Empty;
			if (kMyCharInfo != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1721"),
					"gold",
					ANNUALIZED.Convert(kMyCharInfo.m_Money)
				});
			}
			else
			{
				text = "0";
			}
			this.m_lbMyMoney.SetText(text);
			this.m_txItemIcon4.Visible = true;
			this.m_lbHaveMaterial_3.Visible = true;
		}
		else if (base.p_nSelectIndex == 0)
		{
			this.m_lbHaveMaterial_3.Visible = false;
		}
		this.m_fMaterialNumCheck = Time.time;
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			if (base.p_nSelectIndex == 1)
			{
				gameHelpList_Dlg.SetViewType(eHELP_LIST.Gear_Enchant.ToString());
			}
			else if (NrTSingleton<ContentsLimitManager>.Instance.IsItemNormalSkillBlock())
			{
				gameHelpList_Dlg.SetViewType(eHELP_LIST.Gear_Carving.ToString());
			}
		}
	}

	private bool IsCanReinforce(ITEM _pItem, ITEMINFO _pItemInfo)
	{
		if (_pItem == null)
		{
			return false;
		}
		if (_pItemInfo == null)
		{
			return false;
		}
		if (_pItem.m_nDurability == 0)
		{
			return false;
		}
		if (_pItemInfo.IsItemATB(2097152L))
		{
			return false;
		}
		bool flag = false;
		if (_pItemInfo.IsItemATB(131072L) || _pItemInfo.IsItemATB(524288L))
		{
			flag = true;
		}
		int num = _pItem.m_nOption[9];
		int num2 = 30;
		if (!flag)
		{
			num = _pItem.m_nOption[5];
			num2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_NORMAL_ITEMSKILLREINFORCE_MAXLEVEL);
		}
		return num != 0 && num < num2;
	}
}
