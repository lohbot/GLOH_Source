using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ReforgeMainDlg : Form
{
	private enum eREFOGE_TYPE
	{
		MONEY,
		ITEM
	}

	private Button m_btConfirm;

	private Label m_lbItemName;

	private Label m_lbHaveMoney;

	private Label m_lbRequestMoney;

	private Label m_lbItemGrade;

	private DrawTexture m_txItemBG;

	private DrawTexture m_txBG;

	private ImageView m_ivReforgeItem;

	private ImageView m_ivReforgeItemTiket;

	private Label m_lbRequestTicketCount;

	private Label m_lbHaveTicketCount;

	private Label m_lbTicketName;

	private Button m_btnReforgeTicket;

	private Button m_btnReforgeHelp;

	private Label m_lbAgitNPCInfo;

	private GS_ENHANCEITEM_REQ m_Packet = new GS_ENHANCEITEM_REQ();

	private ITEM m_SetItem;

	private long m_SolID;

	private GameObject rootGameObject;

	private Animation aniGameObject;

	private bool bRequest;

	private bool bLoadActionReforge;

	private bool bShowTicketHelp;

	public bool bSendRequest;

	private bool m_bAgitNPC;

	private ReforgeMainDlg.eREFOGE_TYPE m_eReforgeType;

	private ITEM_REFORGE m_ReforgeInfo;

	private DrawTexture m_dHelpBack;

	private Label m_lbHelp;

	private UIButton _GuideItem;

	private float _ButtonZ;

	private int m_nWinID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Reforge/DLG_ReforgeMain", G_ID.REFORGEMAIN_DLG, true);
	}

	public override void InitData()
	{
	}

	public override void SetComponent()
	{
		this.m_txItemBG = (base.GetControl("DrawTexture_DrawTexture22") as DrawTexture);
		this.m_txBG = (base.GetControl("DrawTexture_subbg") as DrawTexture);
		this.m_txBG.SetTextureFromBundle("UI/Etc/reforge");
		this.m_btConfirm = (base.GetControl("Button_confirm") as Button);
		this.m_btConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickConfirm));
		this.m_lbItemName = (base.GetControl("Label_equip") as Label);
		this.m_lbHaveMoney = (base.GetControl("Label_text3") as Label);
		this.m_lbRequestMoney = (base.GetControl("Label_text5") as Label);
		this.m_lbItemGrade = (base.GetControl("Label_grade") as Label);
		this.m_dHelpBack = (base.GetControl("DrawTexture_TicketHelp1") as DrawTexture);
		this.m_dHelpBack.SetLocationZ(-0.3f);
		this.m_lbHelp = (base.GetControl("Label_TicketHelp") as Label);
		this.m_lbHelp.SetLocationZ(-0.4f);
		this.m_ivReforgeItem = (base.GetControl("ImageView_equip") as ImageView);
		this.m_ivReforgeItem.SetImageView(1, 1, 80, 80, 1, 1, (int)this.m_ivReforgeItem.GetSize().y);
		this.m_ivReforgeItem.spacingAtEnds = false;
		this.m_ivReforgeItem.touchScroll = false;
		this.m_ivReforgeItem.clipContents = false;
		this.m_ivReforgeItem.ListDrag = false;
		if (TsPlatform.IsMobile)
		{
			this.m_ivReforgeItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Mouse_Over));
		}
		else
		{
			this.m_ivReforgeItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Mouse_Click));
			this.m_ivReforgeItem.AddMouseOutDelegate(new EZValueChangedDelegate(this.On_Mouse_Out));
		}
		this.m_ivReforgeItemTiket = (base.GetControl("ImageView_Ticket") as ImageView);
		this.m_ivReforgeItemTiket.SetImageView(1, 1, 80, 80, 1, 1, (int)this.m_ivReforgeItem.GetSize().y);
		this.m_ivReforgeItemTiket.spacingAtEnds = false;
		this.m_ivReforgeItemTiket.touchScroll = false;
		this.m_ivReforgeItemTiket.clipContents = false;
		this.m_ivReforgeItemTiket.ListDrag = false;
		this.m_btnReforgeHelp = (base.GetControl("Button_TicketHelp") as Button);
		this.m_btnReforgeHelp.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickTicketHelp));
		this.m_btnReforgeHelp.Visible = false;
		this.m_btnReforgeTicket = (base.GetControl("Button_Ticket") as Button);
		this.m_btnReforgeTicket.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickShowTicket));
		this.m_btnReforgeTicket.Visible = false;
		this.m_lbRequestTicketCount = (base.GetControl("Label_Ticket4") as Label);
		this.m_lbHaveTicketCount = (base.GetControl("Label_Ticket5") as Label);
		this.m_lbTicketName = (base.GetControl("Label_Ticket1") as Label);
		this.m_lbAgitNPCInfo = (base.GetControl("Label_AgitNPC") as Label);
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsGuildAgit() && NrTSingleton<NewGuildManager>.Instance.IsAgitNPC(2))
		{
			AGIT_NPC_SUB_DATA agitNPCSubDataFromNPCType = NrTSingleton<NewGuildManager>.Instance.GetAgitNPCSubDataFromNPCType(2);
			AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(agitNPCSubDataFromNPCType.ui8NPCType.ToString());
			if (agitNPCSubDataFromNPCType != null && agitNPCData != null)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2747"),
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
		this.ItemSlotClear();
		float x = (GUICamera.width - base.GetSizeX() * 2f) / 2f;
		float y = (GUICamera.height - base.GetSizeY()) / 2f;
		base.SetLocation(x, y);
		this.m_lbHaveMoney.Text = Protocol_Item.Money_Format(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money);
		ReforgeSelectDlg reforgeSelectDlg = base.SetChildForm(G_ID.REFORGESELECT_DLG, Form.ChildLocation.RIGHT) as ReforgeSelectDlg;
		if (reforgeSelectDlg != null)
		{
			reforgeSelectDlg.Show();
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PRODUCTION", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("itemreforgedlg_open");
		base.SetShowLayer(3, false);
		base.SetShowLayer(4, false);
	}

	public void ItemSlotClear()
	{
		this.m_lbItemName.Text = string.Empty;
		this.m_ivReforgeItem.Clear();
		this.m_Packet.SolID = 0L;
		this.m_Packet.nSrcItemPos = 0;
		this.m_Packet.nSrcItemUnique = 0;
		this.m_Packet.nSrcPosType = 0;
		this.m_Packet.nReforgeGold = 0;
		this.m_Packet.ItemUsed = 0;
		ImageSlot imageSlot = new ImageSlot();
		imageSlot.c_oItem = null;
		imageSlot.Index = 0;
		imageSlot.imageStr = "Com_I_Transparent";
		imageSlot.WindowID = base.WindowID;
		imageSlot.itemunique = 0;
		imageSlot.SlotInfo.Set(string.Empty, string.Empty);
		this.m_ivReforgeItem.SetImageSlot(0, imageSlot, null, null, null, null);
		this.m_ivReforgeItem.RepositionItems();
		this.m_txItemBG.SetTexture("Win_I_FrameD");
		if (this.m_eReforgeType == ReforgeMainDlg.eREFOGE_TYPE.ITEM)
		{
			this.TicketSlotClear();
		}
		this.m_btnReforgeTicket.Visible = false;
		this.m_btnReforgeHelp.Visible = false;
		this.m_eReforgeType = ReforgeMainDlg.eREFOGE_TYPE.MONEY;
		this.bShowTicketHelp = false;
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, false);
		base.SetShowLayer(4, false);
		this.m_btnReforgeTicket.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1946");
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGECONFIRM_DLG);
		ReforgeSelectDlg reforgeSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGESELECT_DLG) as ReforgeSelectDlg;
		if (reforgeSelectDlg != null)
		{
			reforgeSelectDlg.closeButton.Visible = true;
		}
	}

	private void TicketSlotClear()
	{
		this.m_lbHaveTicketCount.SetText(string.Empty);
		this.m_lbRequestTicketCount.SetText(string.Empty);
		ImageSlot imageSlot = new ImageSlot();
		imageSlot.c_oItem = null;
		imageSlot.Index = 0;
		imageSlot._solID = 0L;
		imageSlot.WindowID = base.WindowID;
		imageSlot.SlotInfo.Set(string.Empty, string.Empty);
		this.m_ivReforgeItemTiket.SetImageSlot(0, imageSlot, null, null, null, null);
		this.m_ivReforgeItemTiket.RepositionItems();
	}

	public bool OnItemDragDrop(ImageSlot srcSlot)
	{
		ITEM item = srcSlot.c_oItem as ITEM;
		if (!this.IsEnableItem(item))
		{
			return false;
		}
		this.SetSendItem(ref srcSlot, 1);
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		return true;
	}

	public bool IsEnableItem(ITEM item)
	{
		bool result = true;
		if (!Protocol_Item.Is_EquipItem(item.m_nItemUnique) && item.m_nPosType != 10)
		{
			string message = string.Empty;
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("553");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			result = false;
		}
		return result;
	}

	public void SetSendItem(ref ImageSlot itemslot, int i32ItemNum)
	{
		this.ItemSlotClear();
		ITEM iTEM = itemslot.c_oItem as ITEM;
		this.m_SetItem = iTEM;
		ImageSlot imageSlot = new ImageSlot();
		this.m_Packet.nSrcItemUnique = iTEM.m_nItemUnique;
		this.m_Packet.nSrcPosType = iTEM.m_nPosType;
		this.m_Packet.nSrcItemPos = iTEM.m_nItemPos;
		this.m_ivReforgeItem.Clear();
		itemslot.WindowID = base.WindowID;
		itemslot.Index = 0;
		imageSlot.SlotInfo.Set(string.Empty, itemslot.Rank.ToString());
		this.m_ivReforgeItem.SetImageSlot(0, itemslot, new EZDragDropDelegate(this.DragDrop), new EZValueChangedDelegate(this.On_Mouse_Over), new EZValueChangedDelegate(this.On_Mouse_Out), null);
		this.m_ivReforgeItem.RepositionItems();
		int num = iTEM.m_nOption[2];
		string str = ItemManager.RankTextColor(num);
		this.m_lbItemGrade.Text = str + ItemManager.RankText(num);
		this.m_txItemBG.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString(num));
		this.m_lbItemName.Text = str + NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemslot.itemunique, iTEM.m_nRank);
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
		if (itemInfo == null)
		{
			return;
		}
		this.m_ReforgeInfo = NrTSingleton<ITEM_REFORGE_Manager>.Instance.GetItemReforgeData(itemInfo.m_nQualityLevel, num);
		this.SetRequestMoney();
		if (this.m_ReforgeInfo != null && this.m_ReforgeInfo.nReforgeItemUnique != 0 && num < 6 && NrTSingleton<ContentsLimitManager>.Instance.IsUseCaralyst())
		{
			this.m_btnReforgeTicket.Visible = true;
			this.m_btnReforgeHelp.Visible = true;
		}
		else
		{
			this.m_eReforgeType = ReforgeMainDlg.eREFOGE_TYPE.MONEY;
			this.m_btnReforgeTicket.Visible = false;
			this.m_btnReforgeHelp.Visible = false;
		}
	}

	public void SetRequestMoney()
	{
		this.m_lbHaveMoney.Visible = true;
		this.m_lbHaveMoney.Text = Protocol_Item.Money_Format(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money);
		if (this.m_ReforgeInfo == null)
		{
			TsLog.Log("reforge == null", new object[0]);
			return;
		}
		this.m_lbRequestMoney.Visible = true;
		this.m_lbRequestMoney.Text = Protocol_Item.Money_Format((long)this.m_ReforgeInfo.nReforgeGold);
		this.m_Packet.nReforgeGold = this.m_ReforgeInfo.nReforgeGold;
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
		ReforgeSelectDlg reforgeSelectDlg = base.SetChildForm(G_ID.REFORGESELECT_DLG, Form.ChildLocation.RIGHT) as ReforgeSelectDlg;
		if (reforgeSelectDlg != null)
		{
			this.ItemSlotClear();
			reforgeSelectDlg.Show();
		}
	}

	private void OnClickConfirm(IUIObject a_oObject)
	{
		if (!this.IsCheck())
		{
			return;
		}
		ReforgeConfirmDlg reforgeConfirmDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REFORGECONFIRM_DLG) as ReforgeConfirmDlg;
		if (reforgeConfirmDlg != null)
		{
			if (this.m_eReforgeType == ReforgeMainDlg.eREFOGE_TYPE.MONEY)
			{
				reforgeConfirmDlg.SetData(this.m_SetItem, (long)this.m_Packet.nReforgeGold, 0, 0);
			}
			else
			{
				reforgeConfirmDlg.SetData(this.m_SetItem, 0L, this.m_ReforgeInfo.nReforgeItemUnique, this.m_ReforgeInfo.nReforgeItemNum);
			}
		}
		ReforgeSelectDlg reforgeSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGESELECT_DLG) as ReforgeSelectDlg;
		if (reforgeSelectDlg != null)
		{
			reforgeSelectDlg.closeButton.Visible = false;
		}
		this.HideUIGuide();
	}

	public bool IsCheck()
	{
		if (this.bSendRequest)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		if (this.m_SolID > 0L)
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(this.m_SolID);
			if (soldierInfoFromSolID == null)
			{
				return false;
			}
			if (soldierInfoFromSolID.GetSolPosType() == 6)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("370"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
			this.m_Packet.SolID = this.m_SolID;
		}
		if (this.m_Packet.nSrcItemUnique == 0)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("552");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		if (this.m_SetItem.GetRank() == eITEM_RANK_TYPE.ITEM_RANK_SS)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("505");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		return this.isMaterialCheck();
	}

	private void OnClickTicketHelp(IUIObject a_oObject)
	{
		this.bShowTicketHelp = !this.bShowTicketHelp;
		base.SetShowLayer(4, this.bShowTicketHelp);
	}

	private void OnClickShowTicket(IUIObject a_oObject)
	{
		if (this.m_eReforgeType == ReforgeMainDlg.eREFOGE_TYPE.MONEY)
		{
			this.m_eReforgeType = ReforgeMainDlg.eREFOGE_TYPE.ITEM;
			base.SetShowLayer(3, this.m_eReforgeType == ReforgeMainDlg.eREFOGE_TYPE.ITEM);
			base.SetShowLayer(2, this.m_eReforgeType == ReforgeMainDlg.eREFOGE_TYPE.MONEY);
			this.m_ivReforgeItemTiket.Clear();
			ImageSlot imageSlot = new ImageSlot();
			imageSlot.c_bEnable = true;
			imageSlot.Index = 0;
			imageSlot.itemunique = this.m_ReforgeInfo.nReforgeItemUnique;
			imageSlot._solID = 0L;
			imageSlot.WindowID = base.WindowID;
			imageSlot.SlotInfo._visibleRank = true;
			this.m_ivReforgeItemTiket.SetImageSlot(0, imageSlot, null, null, null, null);
			this.m_ivReforgeItemTiket.RepositionItems();
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
				"Count",
				ANNUALIZED.Convert(this.m_ReforgeInfo.nReforgeItemNum)
			});
			this.m_lbRequestTicketCount.SetText(empty);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
				"Count",
				ANNUALIZED.Convert(NkUserInventory.GetInstance().GetItemCnt(this.m_ReforgeInfo.nReforgeItemUnique))
			});
			this.m_lbHaveTicketCount.SetText(empty);
			this.m_lbTicketName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_ReforgeInfo.nReforgeItemUnique);
			this.m_btnReforgeTicket.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1975");
		}
		else
		{
			this.m_eReforgeType = ReforgeMainDlg.eREFOGE_TYPE.MONEY;
			this.TicketSlotClear();
			base.SetShowLayer(3, this.m_eReforgeType == ReforgeMainDlg.eREFOGE_TYPE.ITEM);
			base.SetShowLayer(2, this.m_eReforgeType == ReforgeMainDlg.eREFOGE_TYPE.MONEY);
			this.m_btnReforgeTicket.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1946");
		}
	}

	public override void Set_Value(object a_oObject)
	{
		base.Set_Value(a_oObject);
		this.SendItemSet(a_oObject as ITEM);
	}

	public void SetSolID(long SolID)
	{
		this.m_Packet.SolID = SolID;
		this.m_SolID = SolID;
	}

	public void SendItemSet(ITEM item)
	{
		ImageSlot srcSlot = new ImageSlot();
		ReforgeMainDlg.SetImageSlotFromItem(ref srcSlot, item, 0);
		this.OnItemDragDrop(srcSlot);
	}

	public static void SetImageSlotFromItem(ref ImageSlot targetSlot, ITEM srcItem, short a_shPosItem)
	{
		if (srcItem != null)
		{
			targetSlot.c_oItem = srcItem;
			targetSlot.c_bEnable = true;
			targetSlot.Index = srcItem.m_nItemPos;
			targetSlot.imageStr = "Win_T_ItemEmpty";
			targetSlot.itemunique = srcItem.m_nItemUnique;
			if (srcItem.m_nItemNum > 1)
			{
				targetSlot.SlotInfo._visibleNum = true;
			}
			if (Protocol_Item.Is_EquipItem(srcItem.m_nItemUnique))
			{
				targetSlot.SlotInfo._visibleRank = true;
			}
			targetSlot.SlotInfo.Set(srcItem.m_nItemNum.ToString(), "+" + srcItem.m_nRank.ToString());
		}
		else
		{
			targetSlot.c_oItem = null;
			targetSlot.Index = (int)a_shPosItem;
			targetSlot.imageStr = "Win_T_ItemEmpty";
			targetSlot.SlotInfo.Set("0", "0");
		}
	}

	public void SendServer()
	{
		if (!this.IsExecutEnhance())
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("551");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (this.m_eReforgeType == ReforgeMainDlg.eREFOGE_TYPE.ITEM)
		{
			this.m_Packet.ItemUsed = 1;
			this.m_Packet.nReforgeGold = 0;
		}
		else
		{
			this.m_Packet.ItemUsed = 0;
		}
		this.m_Packet.UpgradeType = 0;
		if (this.m_bAgitNPC)
		{
			this.m_Packet.i8AgitNPC = 1;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ENHANCEITEM_REQ, this.m_Packet);
		this.ItemSlotClear();
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		this.bSendRequest = true;
	}

	public bool IsExecutEnhance()
	{
		bool result = true;
		if (this.m_Packet.nSrcPosType == 10)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return false;
			}
			NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(this.m_Packet.SolID);
			if (soldierInfoFromSolID != null)
			{
				ITEM item = soldierInfoFromSolID.GetEquipItemInfo().GetItem(this.m_Packet.nSrcItemPos);
				if (item == null)
				{
					result = false;
				}
				else if (item.m_nItemID == this.m_SetItem.m_nItemID && item.m_nItemUnique == this.m_SetItem.m_nItemUnique && item.m_nRank == this.m_SetItem.m_nRank)
				{
					result = true;
				}
			}
		}
		else
		{
			ITEM item = NkUserInventory.GetInstance().GetItem((int)((byte)this.m_Packet.nSrcPosType), this.m_Packet.nSrcItemPos);
			if (item == null)
			{
				result = false;
			}
			else if (item.m_nItemID == this.m_SetItem.m_nItemID && item.m_nItemUnique == this.m_SetItem.m_nItemUnique && item.m_nRank == this.m_SetItem.m_nRank)
			{
				result = true;
			}
		}
		return result;
	}

	public bool isMaterialCheck()
	{
		bool result = true;
		if (this.m_eReforgeType == ReforgeMainDlg.eREFOGE_TYPE.MONEY)
		{
			if ((long)this.m_Packet.nReforgeGold > NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("550");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				LackGold_dlg lackGold_dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GOLDLACK_DLG) as LackGold_dlg;
				if (lackGold_dlg != null)
				{
					lackGold_dlg.SetData((long)this.m_Packet.nReforgeGold - NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money);
				}
				return false;
			}
		}
		else if (NkUserInventory.GetInstance().GetItemCnt(this.m_ReforgeInfo.nReforgeItemUnique) < this.m_ReforgeInfo.nReforgeItemNum)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_ReforgeInfo.nReforgeItemUnique)
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			return false;
		}
		return result;
	}

	public bool IsItemCheck(ITEM SourceItem)
	{
		bool result = false;
		if (SourceItem == this.m_SetItem)
		{
			result = true;
		}
		return result;
	}

	public long GetSolID()
	{
		return this.m_SolID;
	}

	public void ReFreshItem()
	{
		if (this.m_SetItem == null)
		{
			return;
		}
		ITEM iTEM = new ITEM();
		if (this.m_SetItem.m_nPosType == 10)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(this.m_SolID);
			if (soldierInfoFromSolID != null)
			{
				iTEM = soldierInfoFromSolID.GetEquipItemInfo().GetItem(this.m_SetItem.m_nItemPos);
			}
		}
		else
		{
			iTEM = NkUserInventory.GetInstance().GetItem(this.m_SetItem.m_nPosType, this.m_SetItem.m_nItemPos);
			this.m_SolID = 0L;
		}
		this.m_SetItem = iTEM;
		this.SetSolID(this.m_SolID);
		this.Set_Value(iTEM);
	}

	public override void OnClose()
	{
		NkInputManager.IsInputMode = true;
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PRODUCTION", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
		NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.EQUIP_ITEM);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
	}

	public void ActionReforge()
	{
		if (!this.bRequest)
		{
			string str = string.Format("{0}", "UI/Item/fx_reinforce" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetActionReforge), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			this.bRequest = true;
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
				this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 300f;
				this.rootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.rootGameObject, GUICamera.UILayer);
				base.InteractivePanel.MakeChild(this.rootGameObject);
				this.aniGameObject = this.rootGameObject.GetComponentInChildren<Animation>();
				this.bLoadActionReforge = true;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootGameObject);
				}
				TsAudio.StoreMuteAllAudio();
				TsAudio.SetExceptMuteAllAudio(EAudioType.UI, true);
				TsAudio.RefreshAllMuteAudio();
				NkInputManager.IsInputMode = false;
			}
		}
	}

	public override void Update()
	{
		if (this.bLoadActionReforge && null != this.rootGameObject && null != this.aniGameObject && !this.aniGameObject.isPlaying)
		{
			UnityEngine.Object.DestroyImmediate(this.rootGameObject);
			this.bLoadActionReforge = false;
			this.bRequest = false;
			this.aniGameObject = null;
			TsAudio.RestoreMuteAllAudio();
			TsAudio.RefreshAllMuteAudio();
			this.SendServer();
			NkInputManager.IsInputMode = true;
		}
	}

	public ITEM GetSelectItem()
	{
		return this.m_SetItem;
	}

	public void UpdateMoney()
	{
		this.m_lbHaveMoney.Text = Protocol_Item.Money_Format(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money);
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.depthChangeable = false;
		}
		this._GuideItem = (base.GetControl(param1) as UIButton);
		this.m_nWinID = winID;
		if (null != this._GuideItem)
		{
			this._ButtonZ = this._GuideItem.GetLocation().z;
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				if (uI_UIGuide.GetLocation().z == base.GetLocation().z)
				{
					uI_UIGuide.SetLocation(uI_UIGuide.GetLocationX(), uI_UIGuide.GetLocationY(), uI_UIGuide.GetLocation().z - 10f);
				}
				this._GuideItem.EffectAni = false;
				Vector2 x = new Vector2(base.GetLocationX() + this._GuideItem.GetLocationX() + 72f, base.GetLocationY() + this._GuideItem.GetLocationY() - 17f);
				uI_UIGuide.Move(x, UI_UIGuide.eTIPPOS.BUTTOM);
				this._ButtonZ = this._GuideItem.gameObject.transform.localPosition.z;
				this._GuideItem.SetLocationZ(uI_UIGuide.GetLocation().z - base.GetLocation().z - 1f);
				this._GuideItem.AlphaAni(1f, 0.5f, -0.5f);
			}
		}
	}

	public void HideUIGuide()
	{
		if (null != this._GuideItem)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			this._GuideItem.SetLocationZ(this._ButtonZ);
			this._GuideItem.StopAni();
			this._GuideItem.AlphaAni(1f, 1f, 0f);
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				uI_UIGuide.Close();
			}
		}
		this._GuideItem = null;
	}
}
