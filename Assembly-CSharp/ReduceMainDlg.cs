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

public class ReduceMainDlg : Form
{
	public enum eTAB
	{
		eTAB_ITEM,
		eTAB_SOLDER
	}

	public const int QUALITYLEVEL_MIN = 1001;

	private Label m_lbtitle;

	private Label m_lbSubtitle1;

	private Label m_lbSubtitle2;

	private Label m_lbGold;

	private DrawTexture m_dtSubBG;

	private DrawTexture m_dtSelectItemBG;

	private ImageView m_ivSelectItem;

	private Label m_lbSelectItem;

	private Label m_lbItemText;

	private Label m_lbNeedItemText;

	private Label m_lbMyItemText;

	private Label m_lbNeedItemNum;

	private Label m_lbCurItemNum;

	private ImageView m_ivNeedItem;

	private DrawTexture m_dtMaterial;

	private Label m_lbNeedItemName;

	private Toolbar m_tbTab;

	private DropDownList m_dlList;

	private NewListBox m_nlbItem;

	private NewListBox m_nlbSolList;

	private Label m_lbNoItem;

	private ImageView[] m_ivSlot = new ImageView[6];

	private DrawTexture[] m_dtSlot = new DrawTexture[6];

	private DrawTexture m_dtSlotBG;

	private Button m_btConfirm;

	private Label m_lbReduce;

	private Label m_lbAgitNPCInfo;

	private Button m_HelpButton;

	private Label m_lbGold2;

	private ReduceMainDlg.eTAB m_eTab;

	private StringBuilder m_Text = new StringBuilder();

	private byte m_nSearch_SolPosType = 1;

	private int m_nSearch_SolSortType;

	private List<NkSoldierInfo> m_kSolList = new List<NkSoldierInfo>();

	private List<NkSoldierInfo> m_kSolSortList = new List<NkSoldierInfo>();

	private NkSoldierInfo m_SelectSol;

	private ITEM m_SelectItem;

	private byte m_byMilityUnique;

	private GS_ITEM_USELEVEL_DEC_REQ m_Packet = new GS_ITEM_USELEVEL_DEC_REQ();

	private string m_strMessage = string.Empty;

	private ITEM m_NeedItem = new ITEM();

	private GameObject m_RootGameObject;

	private bool m_bRequest;

	private bool m_bLoadActionReforge;

	private float m_fStartTime;

	private string m_strQualityLevel = string.Empty;

	private List<ITEM> m_ItemList = new List<ITEM>();

	private string m_strNeedItemName = string.Empty;

	private string m_strItemName = string.Empty;

	private Button m_btEffectShowHelp;

	private DrawTexture m_txRingslotlock;

	private bool m_bAgitNPC;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "item/dlg_reducemain", G_ID.REDUCEMAIN_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_btEffectShowHelp = (base.GetControl("Help_Button") as Button);
		this.m_btEffectShowHelp.Click = new EZValueChangedDelegate(this.BtnShowEffectHelpSol);
		this.m_lbtitle = (base.GetControl("Label_title") as Label);
		this.m_lbSubtitle1 = (base.GetControl("Label_subtitle1") as Label);
		this.m_lbSubtitle2 = (base.GetControl("Label_subtitle2") as Label);
		this.m_lbGold = (base.GetControl("Label_gold") as Label);
		this.m_dtSubBG = (base.GetControl("DrawTexture_subbg") as DrawTexture);
		this.m_dtSubBG.SetTextureFromBundle("UI/Etc/reforge");
		this.m_dtSelectItemBG = (base.GetControl("DrawTexture_DrawTexture22") as DrawTexture);
		this.m_ivSelectItem = (base.GetControl("ImageView_equip") as ImageView);
		this.m_ivSelectItem.SetImageView(1, 1, 80, 80, 1, 1, (int)this.m_ivSelectItem.GetSize().y);
		this.m_ivSelectItem.spacingAtEnds = false;
		this.m_ivSelectItem.touchScroll = false;
		this.m_ivSelectItem.clipContents = false;
		this.m_ivSelectItem.ListDrag = false;
		this.m_ivSelectItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Mouse_Over));
		this.m_ivSelectItem.AddMouseOutDelegate(new EZValueChangedDelegate(this.On_Mouse_Out));
		this.m_lbSelectItem = (base.GetControl("Label_equip") as Label);
		this.m_lbNeedItemText = (base.GetControl("Label_text2") as Label);
		this.m_lbMyItemText = (base.GetControl("Label_text4") as Label);
		this.m_lbItemText = (base.GetControl("Label_text") as Label);
		this.m_lbNeedItemNum = (base.GetControl("Label_text8") as Label);
		this.m_lbNeedItemNum.SetText(string.Empty);
		this.m_lbCurItemNum = (base.GetControl("Label_text10") as Label);
		this.m_lbCurItemNum.SetText(string.Empty);
		this.m_txRingslotlock = (base.GetControl("DrawTexture_ringslotlock") as DrawTexture);
		this.m_txRingslotlock.Visible = true;
		this.m_ivNeedItem = (base.GetControl("ImageView_material") as ImageView);
		this.m_ivNeedItem.SetImageView(1, 1, 80, 80, 1, 1, (int)this.m_ivNeedItem.GetSize().y);
		this.m_ivNeedItem.spacingAtEnds = false;
		this.m_ivNeedItem.touchScroll = false;
		this.m_ivNeedItem.clipContents = false;
		this.m_ivNeedItem.ListDrag = false;
		this.m_ivNeedItem.AddMouseOutDelegate(new EZValueChangedDelegate(this.On_Mouse_Out));
		this.m_ivNeedItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Mouse_Over));
		this.m_dtMaterial = (base.GetControl("DrawTexture_material") as DrawTexture);
		this.m_lbNeedItemName = (base.GetControl("Label_text7") as Label);
		this.m_lbNeedItemName.SetText(string.Empty);
		this.m_tbTab = (base.GetControl("ToolBar_tab") as Toolbar);
		this.m_tbTab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("204");
		this.m_tbTab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1490");
		UIPanelTab expr_362 = this.m_tbTab.Control_Tab[0];
		expr_362.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_362.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_390 = this.m_tbTab.Control_Tab[1];
		expr_390.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_390.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_dlList = (base.GetControl("DropDownList_DropDownList1") as DropDownList);
		this.m_dlList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), 1);
		this.m_dlList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("123"), 0);
		this.m_dlList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("120"), 100);
		this.m_dlList.SetViewArea(this.m_dlList.Count);
		this.m_dlList.RepositionItems();
		this.m_dlList.SetFirstItem();
		this.m_dlList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeSortSolList));
		this.m_nlbItem = (base.GetControl("NewListBox_reduce1") as NewListBox);
		this.m_nlbItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemList));
		this.m_nlbSolList = (base.GetControl("NewListBox_reduce2") as NewListBox);
		this.m_nlbSolList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolList));
		this.m_lbNoItem = (base.GetControl("Label_text6") as Label);
		this.m_lbNoItem.Hide(true);
		string name = string.Empty;
		for (int i = 0; i < 6; i++)
		{
			name = string.Format("ImageView_slot{0}", i + 1);
			this.m_ivSlot[i] = (base.GetControl(name) as ImageView);
			this.m_ivSlot[i].Visible = false;
			this.m_ivSlot[i].SetImageView(1, 1, 80, 80, 1, 1, (int)this.m_ivSelectItem.GetSize().y);
			this.m_ivSlot[i].spacingAtEnds = false;
			this.m_ivSlot[i].touchScroll = false;
			this.m_ivSlot[i].clipContents = false;
			this.m_ivSlot[i].ListDrag = false;
			this.m_ivSlot[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolItem));
			this.m_ivSlot[i].AddMouseOutDelegate(new EZValueChangedDelegate(this.On_Mouse_Out));
			name = string.Format("DrawTexture_slot{0}", i + 1);
			this.m_dtSlot[i] = (base.GetControl(name) as DrawTexture);
			this.m_dtSlot[i].Hide(true);
		}
		this.m_dtSlotBG = (base.GetControl("DrawTexture_subbg3") as DrawTexture);
		this.m_dtSlotBG.Hide(true);
		this.m_btConfirm = (base.GetControl("Button_confirm") as Button);
		this.m_btConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickConfirm));
		this.m_lbReduce = (base.GetControl("Label_reduce") as Label);
		this.m_lbReduce.SetText(string.Empty);
		this.m_HelpButton = (base.GetControl("Help_List") as Button);
		this.m_HelpButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		this.m_lbAgitNPCInfo = (base.GetControl("Label_AgitNPC") as Label);
		this.m_lbGold2 = (base.GetControl("Label_gold2") as Label);
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsGuildAgit() && NrTSingleton<NewGuildManager>.Instance.IsAgitNPC(3))
		{
			AGIT_NPC_SUB_DATA agitNPCSubDataFromNPCType = NrTSingleton<NewGuildManager>.Instance.GetAgitNPCSubDataFromNPCType(3);
			AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(agitNPCSubDataFromNPCType.ui8NPCType.ToString());
			if (agitNPCSubDataFromNPCType != null && agitNPCData != null)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2751"),
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
		this.m_strQualityLevel = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("253");
		this.SelectTab(this.m_eTab);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
		this.HideControl(true);
	}

	public override void Show()
	{
		base.Show();
		if (base.p_nSelectIndex == 100)
		{
			base.SetShowLayer(2, false);
			base.SetShowLayer(4, false);
			base.SetShowLayer(5, true);
			this.SetItemRepairShow();
		}
		else
		{
			base.SetShowLayer(5, false);
		}
	}

	public void InitRepairData()
	{
		this.m_ivSelectItem.Clear();
		this.m_lbItemText.Clear();
		this.m_lbSelectItem.Clear();
		string text = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1721"),
			"gold",
			0
		});
		this.m_lbGold.SetText(text);
		text = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
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
		this.m_lbGold2.SetText(text);
		this.m_lbSelectItem.SetText(string.Empty);
		this.m_lbReduce.SetText(string.Empty);
		this.m_SelectItem = null;
		this.m_nlbItem.Clear();
		this.ShowInvenItemList();
	}

	private void SetItemRepairShow()
	{
		this.m_lbtitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("421"));
		this.m_lbSubtitle1.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("274"));
		this.m_lbSubtitle2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("278"));
		this.m_tbTab.Control_Tab[1].Visible = false;
		this.m_dlList.SetVisible(false);
		this.m_HelpButton.Visible = false;
		this.m_btEffectShowHelp.Visible = false;
		this.m_btConfirm.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("275"));
		this.m_btConfirm.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.ClickConfirm));
		this.m_btConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemRepairConfirm));
		this.InitRepairData();
	}

	private void BtnShowEffectHelpSol(IUIObject obj)
	{
	}

	public void ClickItemRepairConfirm(IUIObject obj)
	{
		if (this.m_SelectItem == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("552"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int itemQuailtyLevel = NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(this.m_SelectItem.m_nItemUnique);
		int itemSkillLevel = this.m_SelectItem.m_nOption[9];
		int num = this.m_SelectItem.m_nOption[7];
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ITEMREPAIR_LIMIT);
		if (num > value)
		{
			return;
		}
		ITEMSKILLREINFORCE itemskillReinforceData = NrTSingleton<ITEMSKILLREINFORCE_Manager>.Instance.GetItemskillReinforceData(itemQuailtyLevel, itemSkillLevel);
		if (itemskillReinforceData == null)
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		int num2 = 1;
		if (num != 0)
		{
			for (int i = 1; i <= num; i++)
			{
				num2 *= 2;
			}
		}
		num++;
		long num3 = (long)itemskillReinforceData.nRestoreGold * (long)num2;
		if (num3 > myCharInfo.m_Money)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676")
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			return;
		}
		GS_ITEMSKILL_REINFORCE_REQ gS_ITEMSKILL_REINFORCE_REQ = new GS_ITEMSKILL_REINFORCE_REQ();
		gS_ITEMSKILL_REINFORCE_REQ.RepairType = 1;
		gS_ITEMSKILL_REINFORCE_REQ.SrcPosType = this.m_SelectItem.m_nPosType;
		gS_ITEMSKILL_REINFORCE_REQ.SrcItemPos = this.m_SelectItem.m_nItemPos;
		gS_ITEMSKILL_REINFORCE_REQ.SrcItemUnique = this.m_SelectItem.m_nItemUnique;
		gS_ITEMSKILL_REINFORCE_REQ.i8UseProtectitem = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMSKILL_REINFORCE_REQ, gS_ITEMSKILL_REINFORCE_REQ);
	}

	public void ClickConfirm(IUIObject obj)
	{
		if (this.m_SelectItem == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("552"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_SelectItem.m_nItemUnique);
		ItemReduceInfo itemReduceInfo = NrTSingleton<NrBaseTableManager>.Instance.GetItemReduceInfo(itemInfo.m_nQualityLevel.ToString());
		if (itemReduceInfo == null)
		{
			return;
		}
		int num = NkUserInventory.GetInstance().Get_First_ItemCnt(itemReduceInfo.iNeedItemUnique);
		int num2 = itemReduceInfo.iNeedItemNum;
		int num3 = this.m_SelectItem.m_nOption[8];
		if (num3 == 0 && 0 < itemReduceInfo.iFirstNum)
		{
			num2 = itemReduceInfo.iFirstNum;
		}
		if (num < num2)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strMessage, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemReduceInfo.iNeedItemUnique)
			});
			Main_UI_SystemMessage.ADDMessage(this.m_strMessage, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			int iRedueceMax = itemReduceInfo.iRedueceMax;
			if (iRedueceMax <= num3)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("217"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			if (itemInfo.m_nUseMinLevel <= num3 + 1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("217"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
		}
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(this.m_SelectItem);
		this.m_strNeedItemName = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemReduceInfo.iNeedItemUnique);
		this.m_strItemName = rankColorName;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strMessage, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("93"),
			"targetname",
			this.m_strNeedItemName,
			"count",
			num2,
			"targetname1",
			this.m_strItemName
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKConfirm), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("984"), this.m_strMessage, eMsgType.MB_OK_CANCEL, 2);
		msgBoxUI.Show();
	}

	public void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_eTab = (ReduceMainDlg.eTAB)uIPanelTab.panel.index;
		this.SelectTab(this.m_eTab);
	}

	public void SelectTab(ReduceMainDlg.eTAB eTab)
	{
		this.ChangeTab();
		if (eTab != ReduceMainDlg.eTAB.eTAB_ITEM)
		{
			if (eTab == ReduceMainDlg.eTAB.eTAB_SOLDER)
			{
				this.m_txRingslotlock.Visible = false;
				this.ShowSolderList();
			}
		}
		else
		{
			this.ShowInvenItemList();
		}
	}

	public void ShowInvenItemList()
	{
		this.m_nlbItem.Visible = true;
		this.m_ItemList.Clear();
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ITEMREPAIR_LIMIT);
		if (base.p_nSelectIndex == 100)
		{
			for (int i = 1; i <= 4; i++)
			{
				for (int j = 0; j < ItemDefine.INVENTORY_ITEMSLOT_MAX; j++)
				{
					ITEM item = NkUserInventory.GetInstance().GetItem(i, j);
					if (item != null)
					{
						ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
						if (itemInfo != null)
						{
							if (item.m_nDurability <= 0)
							{
								int num = item.m_nOption[7];
								if (num <= value)
								{
									if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
									{
										this.m_ItemList.Add(item);
									}
								}
							}
						}
					}
				}
			}
		}
		else
		{
			for (int k = 1; k <= 4; k++)
			{
				for (int j = 0; j < ItemDefine.INVENTORY_ITEMSLOT_MAX; j++)
				{
					ITEM item = NkUserInventory.GetInstance().GetItem(k, j);
					if (item != null)
					{
						ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
						if (itemInfo != null)
						{
							if (1001 < itemInfo.m_nQualityLevel)
							{
								this.m_ItemList.Add(item);
							}
						}
					}
				}
			}
		}
		if (0 < this.m_ItemList.Count)
		{
			this.m_ItemList.Sort(new Comparison<ITEM>(this.CompareItemLevel));
			for (int j = 0; j < this.m_ItemList.Count; j++)
			{
				NewListItem item2 = new NewListItem(this.m_nlbItem.ColumnNum, true, string.Empty);
				this.SetItemColum(this.m_ItemList[j], j, ref item2);
				this.m_nlbItem.Add(item2);
			}
			this.m_lbNoItem.Hide(true);
		}
		else
		{
			this.m_lbNoItem.Hide(false);
		}
		this.m_nlbItem.RepositionItems();
	}

	public void SetAddItemInfo(ITEM InvenItem, ITEMINFO Iteminfo)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbItem.ColumnNum, true, string.Empty);
		this.m_Text.Remove(0, this.m_Text.Length);
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(InvenItem);
		newListItem.SetListItemData(0, true);
		newListItem.SetListItemData(1, InvenItem, null, null, null);
		newListItem.SetListItemData(2, rankColorName, null, null, null);
		if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(InvenItem.m_nItemUnique) == eITEM_PART.ITEMPART_WEAPON)
		{
			int nValue = Protocol_Item.Get_Min_Damage(InvenItem);
			int optionValue = Tooltip_Dlg.GetOptionValue(InvenItem, nValue, 1);
			int nValue2 = Protocol_Item.Get_Max_Damage(InvenItem);
			int optionValue2 = Tooltip_Dlg.GetOptionValue(InvenItem, nValue2, 1);
			this.m_Text.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("242") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString(), " ~ ", optionValue2.ToString()));
		}
		else if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(InvenItem.m_nItemUnique) == eITEM_PART.ITEMPART_ARMOR)
		{
			int nValue = Protocol_Item.Get_Defense(InvenItem);
			int optionValue = Tooltip_Dlg.GetOptionValue(InvenItem, nValue, 2);
			this.m_Text.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("243") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString()));
		}
		newListItem.SetListItemData(3, this.m_Text.ToString(), null, null, null);
		newListItem.SetListItemData(4, false);
		newListItem.Data = InvenItem;
		this.m_nlbItem.Add(newListItem);
	}

	public void ChangeTab()
	{
		this.HideControl(true);
		this.m_nlbItem.Clear();
		this.m_nlbItem.Visible = false;
		this.m_nlbSolList.Clear();
		this.m_nlbSolList.Visible = false;
		this.m_SelectItem = null;
		this.m_Packet.i32ItemUnique = 0;
		this.m_Packet.i32ItemPos = 0;
		this.m_Packet.i64SolID = 0L;
		this.InitIamgeView(this.m_ivSelectItem);
		this.InitIamgeView(this.m_ivNeedItem);
		this.m_dlList.SetVisible(false);
		this.m_lbNoItem.Hide(true);
		for (int i = 0; i < 6; i++)
		{
			this.m_ivSlot[i].Visible = false;
			this.m_dtSlot[i].Hide(true);
			this.InitIamgeView(this.m_ivSlot[i]);
		}
		this.m_dtSlotBG.Hide(true);
		this.m_lbSelectItem.SetText(string.Empty);
		this.m_lbItemText.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1990"));
		this.m_txRingslotlock.Visible = false;
	}

	public void ShowSolderList()
	{
		this.m_nlbSolList.Visible = true;
		this.m_dlList.SetVisible(true);
		for (int i = 0; i < 6; i++)
		{
			this.m_ivSlot[i].Visible = true;
			this.m_dtSlot[i].Hide(false);
		}
		this.m_dtSlotBG.Hide(false);
		this.MakeSolListAndSort();
		this.m_nlbSolList.Clear();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167");
		for (int i = 0; i < this.m_kSolSortList.Count; i++)
		{
			this.SetAddSolInfo(this.m_kSolSortList[i], this.m_strMessage, textFromInterface);
		}
		this.m_nlbSolList.RepositionItems();
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
				int num = i + 1;
				NkMineMilitaryInfo mineMilitaryInfo = militaryList.GetMineMilitaryInfo((byte)num);
				if (mineMilitaryInfo != null && mineMilitaryInfo.IsValid())
				{
					this.MakeMilitarySolList(num);
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
		if (pkSolinfo.GetSolPosType() != (byte)eAddPosType)
		{
			return;
		}
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
			goto IL_B7;
		case 1:
			this.MakeBattleSolList();
			goto IL_B7;
		case 2:
		case 6:
			this.MakeMilitarySolList((int)this.m_byMilityUnique);
			goto IL_B7;
		case 3:
		case 4:
		case 5:
			IL_54:
			if (nSearch_SolPosType != 100)
			{
				goto IL_B7;
			}
			if (this.m_nSearch_SolSortType == 1)
			{
				this.MakeBattleSolList();
			}
			else
			{
				this.MakeBattleSolList();
				this.MakeMilitarySolList();
				this.MakeReadySolList();
			}
			goto IL_B7;
		}
		goto IL_54;
		IL_B7:
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

	public void OnChangeSortSolList(IUIObject obj)
	{
		this.m_nSearch_SolPosType = 1;
		if (this.m_dlList.Count > 0 && this.m_dlList.SelectedItem != null)
		{
			ListItem listItem = this.m_dlList.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.m_nSearch_SolPosType = (byte)listItem.Key;
				if (this.m_nSearch_SolPosType == 2 || this.m_nSearch_SolPosType == 6)
				{
					this.m_byMilityUnique = (byte)this.m_dlList.SelectIndex;
				}
			}
		}
		this.ShowSolderList();
	}

	public void SetAddSolInfo(NkSoldierInfo kSolInfo, string strMessage, string strMsg)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbSolList.ColumnNum, true, string.Empty);
		this.m_Text.Remove(0, this.m_Text.Length);
		EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(kSolInfo.GetCharKind(), kSolInfo.GetGrade());
		if (eventHeroCharCode != null)
		{
			newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
			newListItem.EventMark = true;
		}
		else
		{
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(kSolInfo.GetCharKind(), (int)kSolInfo.GetGrade());
			if (legendFrame != null)
			{
				newListItem.SetListItemData(0, legendFrame, null, null, null);
			}
			else
			{
				newListItem.SetListItemData(0, true);
			}
		}
		newListItem.SetListItemData(1, kSolInfo.GetListSolInfo(false), null, null, null);
		newListItem.SetListItemData(2, kSolInfo.GetName(), null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref strMessage, new object[]
		{
			strMsg,
			"count1",
			kSolInfo.GetLevel(),
			"count2",
			kSolInfo.GetSolMaxLevel()
		});
		newListItem.SetListItemData(3, strMessage, null, null, null);
		newListItem.Data = kSolInfo;
		this.m_nlbSolList.Add(newListItem);
	}

	public void ClickItemList(IUIObject obj)
	{
		UIListItemContainer selectItem = this.m_nlbItem.GetSelectItem();
		if (null == selectItem)
		{
			return;
		}
		if (selectItem.Data == null)
		{
			return;
		}
		ReduceMainDlg.eTAB eTab = this.m_eTab;
		if (eTab != ReduceMainDlg.eTAB.eTAB_ITEM)
		{
			if (eTab == ReduceMainDlg.eTAB.eTAB_SOLDER)
			{
				NkSoldierInfo kSolInfo = selectItem.Data as NkSoldierInfo;
				this.SelectSolderList(kSolInfo);
			}
		}
		else
		{
			ITEM item = selectItem.Data as ITEM;
			this.SelectItemList(item, false, 0L);
		}
	}

	public void SelectItemList(ITEM Item, bool bForceUpdate, long i64SolID)
	{
		if (this.m_bRequest)
		{
			return;
		}
		if (Item == null)
		{
			return;
		}
		if (0 >= Item.m_nItemUnique)
		{
			return;
		}
		if (!bForceUpdate && this.m_SelectItem == Item)
		{
			return;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(Item.m_nItemUnique);
		if (itemInfo == null)
		{
			return;
		}
		if (base.p_nSelectIndex == 100)
		{
			ImageSlot slot = new ImageSlot();
			ReforgeMainDlg.SetImageSlotFromItem(ref slot, Item, 0);
			this.m_ivSelectItem.Clear();
			this.m_ivSelectItem.SetImageSlot(0, slot, null, null, null, null);
			this.m_ivSelectItem.RepositionItems();
			this.m_lbItemText.SetText(string.Empty);
			string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(Item);
			this.m_lbSelectItem.SetText(rankColorName);
			int itemQuailtyLevel = NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(Item.m_nItemUnique);
			int itemSkillLevel = Item.m_nOption[9];
			int num = Item.m_nOption[7];
			ITEMSKILLREINFORCE itemskillReinforceData = NrTSingleton<ITEMSKILLREINFORCE_Manager>.Instance.GetItemskillReinforceData(itemQuailtyLevel, itemSkillLevel);
			if (itemskillReinforceData != null)
			{
				this.m_strMessage = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strMessage, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2968"),
					"count",
					num
				});
				this.m_lbReduce.SetText(this.m_strMessage);
				int num2 = 1;
				if (num != 0)
				{
					for (int i = 1; i <= num; i++)
					{
						num2 *= 2;
					}
				}
				num++;
				long num3 = (long)itemskillReinforceData.nRestoreGold * (long)num2;
				this.m_strMessage = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strMessage, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1721"),
					"gold",
					ANNUALIZED.Convert(num3)
				});
				this.m_lbGold.SetText(this.m_strMessage);
			}
		}
		else
		{
			if (1001 >= itemInfo.m_nQualityLevel)
			{
				Main_UI_SystemMessage.ADDMessage(this.m_strQualityLevel, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			this.HideControl(false);
			ImageSlot slot2 = new ImageSlot();
			ReforgeMainDlg.SetImageSlotFromItem(ref slot2, Item, 0);
			this.m_ivSelectItem.Clear();
			this.m_ivSelectItem.SetImageSlot(0, slot2, null, null, null, null);
			this.m_ivSelectItem.RepositionItems();
			int rank = Item.m_nOption[2];
			string rankColorName2 = NrTSingleton<ItemManager>.Instance.GetRankColorName(Item);
			this.m_lbSelectItem.SetText(rankColorName2);
			this.m_lbItemText.SetText(string.Empty);
			this.m_dtSelectItemBG.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString(rank));
			bool flag = false;
			ItemReduceInfo itemReduceInfo = NrTSingleton<NrBaseTableManager>.Instance.GetItemReduceInfo(itemInfo.m_nQualityLevel.ToString());
			if (itemReduceInfo != null)
			{
				int num4 = itemReduceInfo.iNeedItemNum;
				if (Item.m_nOption[8] == 0 && 0 < itemReduceInfo.iFirstNum)
				{
					num4 = itemReduceInfo.iFirstNum;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strMessage, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2132"),
						"count",
						itemReduceInfo.iFirstPoint
					});
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strMessage, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2133"),
						"count1",
						itemReduceInfo.iNormalPoint,
						"count2",
						itemReduceInfo.iSpecialPoint
					});
				}
				this.m_lbReduce.SetText(this.m_strMessage);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strMessage, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
					"Count",
					num4
				});
				this.m_lbNeedItemNum.SetText(this.m_strMessage);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strMessage, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
					"Count",
					NkUserInventory.GetInstance().Get_First_ItemCnt(itemReduceInfo.iNeedItemUnique)
				});
				this.m_lbCurItemNum.SetText(this.m_strMessage);
				flag = true;
				this.m_NeedItem.m_nItemUnique = itemReduceInfo.iNeedItemUnique;
				this.m_NeedItem.m_nItemNum = num4;
				ImageSlot slot3 = new ImageSlot();
				ReforgeMainDlg.SetImageSlotFromItem(ref slot3, this.m_NeedItem, 0);
				this.m_ivNeedItem.Clear();
				this.m_ivNeedItem.SetImageSlot(0, slot3, new EZDragDropDelegate(this.DragDrop), new EZValueChangedDelegate(this.On_Mouse_Over), new EZValueChangedDelegate(this.On_Mouse_Out), null);
				this.m_ivNeedItem.RepositionItems();
				this.m_lbNeedItemName.SetText(NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemReduceInfo.iNeedItemUnique));
			}
			else
			{
				this.m_lbReduce.SetText(string.Empty);
			}
			if (!flag)
			{
				this.m_lbNeedItemNum.SetText(string.Empty);
				this.m_lbCurItemNum.SetText(string.Empty);
			}
			this.m_Packet.i64SolID = i64SolID;
			this.m_Packet.i32ItemUnique = Item.m_nItemUnique;
			this.m_Packet.i32PosType = Item.m_nPosType;
			this.m_Packet.i32ItemPos = Item.m_nItemPos;
		}
		this.m_SelectItem = Item;
	}

	public void SelectSolderList(NkSoldierInfo kSolInfo)
	{
		if (kSolInfo == null)
		{
			return;
		}
		this.m_SelectSol = kSolInfo;
		for (int i = 0; i < 6; i++)
		{
			this.InitIamgeView(this.m_ivSlot[i]);
		}
		for (int j = 0; j < 6; j++)
		{
			if (j == 5)
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
			ITEM item = kSolInfo.GetEquipItemInfo().m_kItem[j].GetItem();
			if (item != null)
			{
				if (item.m_nItemUnique > 0)
				{
					if (NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique) != null)
					{
						ImageSlot slot = new ImageSlot();
						ReforgeMainDlg.SetImageSlotFromItem(ref slot, item, 0);
						this.m_ivSlot[j].Clear();
						this.m_ivSlot[j].SetImageSlot(0, slot, null, null, null, null);
						this.m_ivSlot[j].RepositionItems();
					}
				}
			}
		}
	}

	public void DragDrop(EZDragDropParams a_sDragDropParams)
	{
		if (a_sDragDropParams.evt == EZDragDropEvent.Dropped && a_sDragDropParams.dragObj.DropTarget != null)
		{
			ImageSlot imageSlot = a_sDragDropParams.dragObj.Data as ImageSlot;
			if (imageSlot != null && !(imageSlot.c_oItem is ITEM))
			{
				return;
			}
			UIListItemContainer component = a_sDragDropParams.dragObj.DropTarget.GetComponent<UIListItemContainer>();
			if (component == null)
			{
				return;
			}
			ImageSlot imageSlot2 = component.Data as ImageSlot;
			if (imageSlot2 == null)
			{
				return;
			}
			if (imageSlot2.c_bDisable)
			{
				return;
			}
			G_ID windowID = (G_ID)imageSlot2.WindowID;
			if (windowID == G_ID.INVENTORY_DLG)
			{
				Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
				if (inventory_Dlg != null)
				{
				}
			}
		}
	}

	private void On_Mouse_Over(IUIObject a_oObject)
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
						itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
					}
				}
			}
		}
	}

	private void On_Mouse_Out(IUIObject a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
	}

	public void ClickSolItem(IUIObject a_oObject)
	{
		ImageView imageView = a_oObject as ImageView;
		if (null == imageView)
		{
			return;
		}
		UIListItemContainer item = imageView.GetItem(0);
		if (null == item)
		{
			return;
		}
		ImageSlot imageSlot = item.Data as ImageSlot;
		if (imageSlot == null)
		{
			return;
		}
		ITEM item2 = imageSlot.c_oItem as ITEM;
		if (this.m_SelectSol == null)
		{
			return;
		}
		this.SelectItemList(item2, false, this.m_SelectSol.GetSolID());
	}

	public void MsgBoxOKConfirm(object a_oObject)
	{
		if (0 >= this.m_Packet.i32ItemUnique)
		{
			return;
		}
		this.ActionReforge();
		this.SetEnable(false);
	}

	public void SetEnable(bool bEnable)
	{
		this.m_tbTab.SetEnabled(bEnable);
		this.m_btConfirm.SetEnabled(bEnable);
		this.m_nlbItem.enabled = bEnable;
		this.m_nlbItem.controlIsEnabled = bEnable;
		for (int i = 0; i < 6; i++)
		{
			this.m_ivSlot[i].enabled = bEnable;
			this.m_ivSlot[i].controlIsEnabled = bEnable;
		}
	}

	public void InitIamgeView(ImageView iv)
	{
		ImageSlot imageSlot = new ImageSlot();
		imageSlot = new ImageSlot();
		imageSlot.c_oItem = null;
		imageSlot.Index = 0;
		imageSlot.imageStr = "Com_I_Transparent";
		imageSlot.WindowID = base.WindowID;
		imageSlot.itemunique = 0;
		imageSlot.SlotInfo.Set(string.Empty, string.Empty);
		iv.Clear();
		iv.SetImageSlot(0, imageSlot, null, null, null, null);
		iv.RepositionItems();
		this.m_lbReduce.SetText(string.Empty);
	}

	public void RefreshSelectItemInfo(GS_ENHANCEITEM_ACK ACK)
	{
		ITEM itemFromItemID = NkUserInventory.GetInstance().GetItemFromItemID(ACK.i64ItemID);
		if (itemFromItemID == null)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(ACK.i64SolID);
			if (soldierInfoFromSolID != null)
			{
				itemFromItemID = soldierInfoFromSolID.GetEquipItemInfo().GetItemFromItemID(ACK.i64ItemID);
			}
		}
		if (itemFromItemID == null)
		{
			return;
		}
		if (this.m_eTab == ReduceMainDlg.eTAB.eTAB_SOLDER)
		{
			this.SelectTab(this.m_eTab);
		}
		this.SelectItemList(itemFromItemID, true, ACK.i64SolID);
		if (this.m_eTab == ReduceMainDlg.eTAB.eTAB_SOLDER && this.m_SelectSol != null)
		{
			this.SelectSolderList(this.m_SelectSol);
		}
	}

	public void ActionReforge()
	{
		if (!this.m_bRequest)
		{
			string str = string.Format("{0}", "UI/Item/fx_reinforce" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetActionReforge), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			this.m_bRequest = true;
		}
	}

	private void SetActionReforge(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_RootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 300f;
				this.m_RootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_RootGameObject, GUICamera.UILayer);
				base.InteractivePanel.MakeChild(this.m_RootGameObject);
				this.m_fStartTime = Time.time;
				this.m_bLoadActionReforge = true;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_RootGameObject);
				}
			}
		}
	}

	public override void Update()
	{
		if (this.m_bLoadActionReforge && Time.time - this.m_fStartTime > 3f)
		{
			UnityEngine.Object.DestroyImmediate(this.m_RootGameObject);
			this.m_bLoadActionReforge = false;
			this.m_bRequest = false;
			if (this.m_bAgitNPC)
			{
				this.m_Packet.i8AgitNPC = 1;
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_USELEVEL_DEC_REQ, this.m_Packet);
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		if (null != this.m_RootGameObject)
		{
			UnityEngine.Object.DestroyImmediate(this.m_RootGameObject);
		}
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
		NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.EQUIP_ITEM);
	}

	public void ClickSolList(IUIObject obj)
	{
		UIListItemContainer selectItem = this.m_nlbSolList.GetSelectItem();
		if (null == selectItem)
		{
			return;
		}
		if (selectItem.Data == null)
		{
			return;
		}
		ReduceMainDlg.eTAB eTab = this.m_eTab;
		if (eTab != ReduceMainDlg.eTAB.eTAB_ITEM)
		{
			if (eTab == ReduceMainDlg.eTAB.eTAB_SOLDER)
			{
				NkSoldierInfo kSolInfo = selectItem.Data as NkSoldierInfo;
				this.SelectSolderList(kSolInfo);
			}
		}
		else
		{
			ITEM item = selectItem.Data as ITEM;
			this.SelectItemList(item, false, 0L);
		}
	}

	public void HideControl(bool bHide)
	{
		this.m_lbNeedItemText.Hide(bHide);
		this.m_lbMyItemText.Hide(bHide);
		this.m_lbNeedItemName.Hide(bHide);
		this.m_lbNeedItemNum.Hide(bHide);
		this.m_lbCurItemNum.Hide(bHide);
		this.m_dtSelectItemBG.Hide(bHide);
		this.m_ivNeedItem.Visible = !bHide;
		this.m_dtMaterial.Hide(bHide);
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

	public void UpdateData(int nItemPos, int nItemType, long nItemID = 0L)
	{
		bool flag = false;
		for (int i = 0; i < this.m_nlbItem.Count; i++)
		{
			if (this.m_nlbItem.GetItem(i) != null)
			{
				ITEM iTEM = this.m_nlbItem.GetItem(i).Data as ITEM;
				if (iTEM.m_nItemPos == nItemPos && iTEM.m_nPosType == nItemType)
				{
					ITEM item = NkUserInventory.GetInstance().GetItem(nItemType, nItemPos);
					NewListItem item2 = new NewListItem(this.m_nlbItem.ColumnNum, true, string.Empty);
					this.SetItemColum(item, i, ref item2);
					this.m_nlbItem.UpdateAdd(i, item2);
					flag = true;
				}
			}
		}
		this.m_nlbItem.RepositionItems();
		if (!flag)
		{
			this.ShowInvenItemList();
		}
	}

	private void SetItemColum(ITEM itemdata, int pos, ref NewListItem item)
	{
		this.m_Text.Remove(0, this.m_Text.Length);
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(itemdata);
		item.SetListItemData(1, itemdata, true, null, null);
		item.SetListItemData(2, rankColorName, null, null, null);
		if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(itemdata.m_nItemUnique) == eITEM_PART.ITEMPART_WEAPON)
		{
			int nValue = Protocol_Item.Get_Min_Damage(itemdata);
			int optionValue = Tooltip_Dlg.GetOptionValue(itemdata, nValue, 1);
			int nValue2 = Protocol_Item.Get_Max_Damage(itemdata);
			int optionValue2 = Tooltip_Dlg.GetOptionValue(itemdata, nValue2, 1);
			this.m_Text.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("242") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString(), " ~ ", optionValue2.ToString()));
		}
		else if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(itemdata.m_nItemUnique) == eITEM_PART.ITEMPART_ARMOR)
		{
			int nValue = Protocol_Item.Get_Defense(itemdata);
			int optionValue = Tooltip_Dlg.GetOptionValue(itemdata, nValue, 2);
			this.m_Text.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("243") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString()));
		}
		item.SetListItemData(3, this.m_Text.ToString(), null, null, null);
		item.SetListItemData(4, string.Empty, itemdata, new EZValueChangedDelegate(this.OnClickItemView), null);
		item.Data = itemdata;
	}

	private void OnClickItemView(IUIObject obj)
	{
		if (TsPlatform.IsMobile)
		{
			UIButton uIButton = obj as UIButton;
			if (null == uIButton)
			{
				return;
			}
			ITEM pkItem = (ITEM)uIButton.data;
			Protocol_Item.Item_ShowItemInfo((G_ID)base.WindowID, pkItem, Vector3.zero, null, 0L);
		}
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null && NrTSingleton<ContentsLimitManager>.Instance.IsItemLevelCheckBlock())
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Gear_Grinding.ToString());
		}
	}
}
