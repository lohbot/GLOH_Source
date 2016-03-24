using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Inventory_Dlg : Form
{
	private enum eSTATE
	{
		eSTATE_NONE,
		eSTATE_MULTISELL,
		eSTATE_MULTILOCK,
		eSTATE_MULTIBREAK,
		eSTATE_MULTIDELETE
	}

	private const float BLINK_TIME = 0.5f;

	private const float VISIBLE_BLINK_ON = 1f;

	private const float VISIBLE_BLINK_OFF = 0.3f;

	private const int MAX_BLINK_COUNT = 10;

	private Inventory_Dlg.eSTATE m_State;

	private Dictionary<long, UIListItemContainer> m_lockList = new Dictionary<long, UIListItemContainer>();

	private Label m_laMoney;

	private Toolbar m_tbToolbar;

	private Button m_btDelete;

	private Button m_btItemSell;

	private Button m_btLock;

	private Button m_btItemBreak;

	private Button m_HelpButton;

	private Button m_btnAddGold;

	private CheckBox m_ckSelectAll;

	private Label m_lbSelectAll;

	private string m_strDeleteName = string.Empty;

	private VerticalSlider m_Slider;

	private long solID;

	private float mTime;

	private float m_BlinkValue;

	private int m_BlinkCount;

	private int m_BlinkPosType;

	private UIButton _GuideItem;

	private UIListItemContainer _GuideSlot;

	private float _ButtonZ;

	private int m_nWinID;

	private List<ITEM> m_ItemBreakList = new List<ITEM>();

	private static byte s_byTab = 1;

	private GameObject m_goSelect;

	private Vector3 m_vSelectPosition;

	private GameObject rootGameObject;

	private Animation aniGameObject;

	private bool bRequest;

	private bool bLoadActionReforge;

	public bool bSendRequest;

	private G_ID m_eParentForm;

	public ImageView c_ivImageView
	{
		get;
		private set;
	}

	public UIListItemContainer c_cCopyItem
	{
		get;
		set;
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Inventory/DLG_Inventory", G_ID.INVENTORY_DLG, false, true);
		NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(63, 0L, 1L);
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		this.solID = charPersonInfo.GetSoldierInfo(0).GetSolID();
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_laMoney = (base.GetControl("Label_Label2") as Label);
		this.c_ivImageView = (base.GetControl("ImageView_ImageView1") as ImageView);
		this.c_ivImageView.line = true;
		this.c_ivImageView.spacingAtEnds = false;
		this.c_ivImageView.SetImageView(ItemDefine.N_SLOT_COUNT_X, ItemDefine.N_SLOT_COUNT_Y, 114, 114, 10, 10, 317);
		this.c_ivImageView.ScrollListTo(0f);
		this.c_ivImageView.AddLongTapDelegate(new EZValueChangedDelegate(this.On_ToolTip));
		this.c_ivImageView.AddDoubleClickDelegate(new EZValueChangedDelegate(this.On_Double_Click));
		this.c_ivImageView.touchScroll = false;
		this.c_ivImageView.itemSpacing = 10f;
		this.c_ivImageView.clipContents = false;
		this.c_ivImageView.MaxMultiSelectNum = 30;
		this.InitMultiItem();
		if (this.c_ivImageView.slider)
		{
			UnityEngine.Object.Destroy(this.c_ivImageView.slider.gameObject);
		}
		this.m_strDeleteName = "Button_InThrowOut";
		this.m_btDelete = (base.GetControl(this.m_strDeleteName) as Button);
		this.m_btDelete.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickInThrowOut));
		this.m_btDelete.Visible = false;
		this.m_btItemSell = (base.GetControl("Button_Sell") as Button);
		this.m_btItemSell.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemSell));
		this.m_btLock = (base.GetControl("Button_Lock01") as Button);
		this.m_btLock.AddValueChangedDelegate(new EZValueChangedDelegate(this.onClickItemLock));
		this.m_btItemBreak = (base.GetControl("BT_break") as Button);
		this.m_btItemBreak.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemBreak));
		this.m_tbToolbar = (base.GetControl("ToolBar_ToolBar1") as Toolbar);
		string[] array = new string[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("757"),
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1378"),
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1390"),
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1391"),
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("192"),
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("191")
		};
		for (int i = 0; i < array.Length; i++)
		{
			this.m_tbToolbar.Control_Tab[i].Text = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1002"), array[i]);
			UIPanelTab expr_2A0 = this.m_tbToolbar.Control_Tab[i];
			expr_2A0.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_2A0.ButtonClick, new EZValueChangedDelegate(this.On_Tab_Button));
		}
		this.m_tbToolbar.SetFirstTab(0);
		Protocol_Item.s_deMoneyDelegate = (Action)Delegate.Combine(Protocol_Item.s_deMoneyDelegate, new Action(this.Money_Refresh));
		this.Money_Refresh();
		this.m_goSelect = new GameObject("selectImage");
		DrawTexture drawTexture = this.m_goSelect.AddComponent<DrawTexture>();
		drawTexture.gameObject.layer = GUICamera.UILayer;
		drawTexture.width = 114f;
		drawTexture.height = 114f;
		drawTexture.SetTexture("Win_T_ItemSelected");
		base.InteractivePanel.MakeChild(this.m_goSelect);
		this.m_goSelect.SetActive(false);
		this.m_HelpButton = (base.GetControl("Help_Button") as Button);
		this.m_HelpButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		this.m_ckSelectAll = (base.GetControl("CheckBox_button_AllCheck") as CheckBox);
		if (this.m_ckSelectAll != null)
		{
			this.m_ckSelectAll.AddValueChangedDelegate(new EZValueChangedDelegate(this.MultiSelect));
			this.m_lbSelectAll = (base.GetControl("Label_AllCheck") as Label);
		}
		this.m_btnAddGold = (base.GetControl("Button_AddGold") as Button);
		this.m_btnAddGold.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBuyGold));
		this.Item_Draw((int)Inventory_Dlg.s_byTab);
		base.SetScreenCenter();
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		ITEMMALL_POPUPSHOP atbToIDX = NrTSingleton<ItemMallPoPupShopManager>.Instance.GetAtbToIDX(ItemMallPoPupShopManager.ePoPupShop_Type.InvenOpen);
		if (atbToIDX != null)
		{
			GS_ITEMSHOP_ITEMPOPUP_INFO_REQ gS_ITEMSHOP_ITEMPOPUP_INFO_REQ = new GS_ITEMSHOP_ITEMPOPUP_INFO_REQ();
			gS_ITEMSHOP_ITEMPOPUP_INFO_REQ.i64PersonID = myCharInfo.m_PersonID;
			gS_ITEMSHOP_ITEMPOPUP_INFO_REQ.i64Idx = (long)atbToIDX.m_Idx;
			gS_ITEMSHOP_ITEMPOPUP_INFO_REQ.i32ATB = 4;
			SendPacket.GetInstance().SendObject(2536, gS_ITEMSHOP_ITEMPOPUP_INFO_REQ);
		}
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("inven_open");
	}

	public override void OnClose()
	{
		if (this.bLoadActionReforge && null != this.rootGameObject && null != this.aniGameObject)
		{
			this.SendServerItemBreak();
			UnityEngine.Object.DestroyImmediate(this.rootGameObject);
			this.bLoadActionReforge = false;
			this.bRequest = false;
			this.aniGameObject = null;
			TsAudio.RestoreMuteAllAudio();
			TsAudio.RefreshAllMuteAudio();
			NkInputManager.IsInputMode = true;
		}
		if (null != this.c_cCopyItem)
		{
			UnityEngine.Object.Destroy(this.c_cCopyItem.gameObject);
			this.c_cCopyItem = null;
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "INVENTORY", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		Protocol_Item.s_deMoneyDelegate = (Action)Delegate.Remove(Protocol_Item.s_deMoneyDelegate, new Action(this.Money_Refresh));
		this.Child_Close();
		NrTSingleton<FormsManager>.Instance.Hide(G_ID.SOLSELECT_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DISASSEMBLEITEM_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMSELL_DLG);
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
		this.InitLockList();
		this.HideUIGuide();
	}

	public override void Hide()
	{
		if (null != this.c_cCopyItem)
		{
			UnityEngine.Object.Destroy(this.c_cCopyItem.gameObject);
			this.c_cCopyItem = null;
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "INVENTORY", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		base.Hide();
	}

	public override void Show()
	{
		base.Show();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "INVENTORY", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(63, 0L, 1L);
		base.SetScreenCenter();
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		this.SetTap(1);
		this.HideUIGuide();
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
				Vector2 x = new Vector2(base.GetLocationX() + this._GuideItem.GetLocationX() + 72f, base.GetLocationY() + this._GuideItem.GetLocationY() - 14f);
				uI_UIGuide.Move(x, UI_UIGuide.eTIPPOS.TOP);
				this._ButtonZ = this._GuideItem.gameObject.transform.localPosition.z;
				this._GuideItem.SetLocationZ(uI_UIGuide.GetLocation().z - base.GetLocation().z - 1f);
				this._GuideItem.AlphaAni(1f, 0.5f, -0.5f);
			}
		}
		else
		{
			int num = 0;
			if (!int.TryParse(param1, out num))
			{
				return;
			}
			for (int i = 0; i < this.c_ivImageView.Count; i++)
			{
				UIListItemContainer item = this.c_ivImageView.GetItem(i);
				if (!(item == null))
				{
					ImageSlot imageSlot = item.Data as ImageSlot;
					if (imageSlot != null)
					{
						ITEM iTEM = imageSlot.c_oItem as ITEM;
						if (iTEM != null && iTEM.m_nItemID > 0L)
						{
							if (iTEM.m_nItemUnique == num)
							{
								this._GuideSlot = item;
								break;
							}
						}
					}
				}
			}
			if (this._GuideSlot != null)
			{
				this._GuideSlot.isDraggable = false;
				this._ButtonZ = this._GuideSlot.gameObject.transform.localPosition.z;
				UI_UIGuide uI_UIGuide2 = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
				if (uI_UIGuide2 != null)
				{
					uI_UIGuide2.SetLocation(uI_UIGuide2.GetLocationX(), uI_UIGuide2.GetLocationY(), base.GetLocation().z - 10f);
					Vector2 x2 = new Vector2(base.GetLocationX() + this.c_ivImageView.GetLocationX() + this._GuideSlot.gameObject.transform.localPosition.x + 74f, base.GetLocationY() + this.c_ivImageView.GetLocationY() - this._GuideSlot.gameObject.transform.localPosition.y + 17f);
					uI_UIGuide2.Move(x2, UI_UIGuide.eTIPPOS.TOP);
					this._ButtonZ = this._GuideSlot.gameObject.transform.localPosition.z;
					this._GuideSlot.gameObject.transform.localPosition = new Vector3(this._GuideSlot.gameObject.transform.localPosition.x, this._GuideSlot.gameObject.transform.localPosition.y, uI_UIGuide2.GetLocation().z - base.GetLocation().z - 1f);
					this._GuideSlot.AlphaAni(1f, 0.5f, -0.5f);
				}
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
		}
		if (null != this._GuideSlot)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			this._GuideSlot.StopAni();
			this._GuideSlot.AlphaAni(1f, 1f, 0f);
			this._GuideSlot.isDraggable = true;
			this._GuideSlot.gameObject.transform.localPosition = new Vector3(this._GuideSlot.gameObject.transform.localPosition.x, this._GuideSlot.gameObject.transform.localPosition.y, this._ButtonZ);
		}
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
			uI_UIGuide.Close();
		}
		this._GuideItem = null;
		this._GuideSlot = null;
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.BetterGear.ToString());
		}
	}

	private void OnClickItemReforge(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG))
		{
			ReforgeMainDlg reforgeMainDlg = (ReforgeMainDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REFORGEMAIN_DLG);
			if (!reforgeMainDlg.Visible)
			{
				reforgeMainDlg.Show();
			}
		}
		this.HideUIGuide();
	}

	private void OnClickItemSell(IUIObject obj)
	{
		if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTILOCK || this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTIBREAK)
		{
			this.InitMultiItem();
		}
		if (!this.c_ivImageView.GetMultiSelectMode())
		{
			this.c_ivImageView.ClearSelectedItems();
			this.c_ivImageView.SetMultiSelectMode(true);
			this.m_btItemSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("701"));
			this.m_btItemSell.SetButtonTextureKey("Win_B_AcceptBtn02");
			this.m_State = Inventory_Dlg.eSTATE.eSTATE_MULTISELL;
			this.MultiItemSellDiable(true);
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("12"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
		}
		else
		{
			if (this.c_ivImageView.SelectedItems.Count == 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("207"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
			else
			{
				this.MultiIemSell();
			}
			this.InitMultiItem();
		}
		this.HideUIGuide();
	}

	private void OnClickInThrowOut(IUIObject obj)
	{
		if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTILOCK || this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTIBREAK)
		{
			this.InitMultiItem();
		}
		if (!this.c_ivImageView.GetMultiSelectMode())
		{
			this.c_ivImageView.ClearSelectedItems();
			this.c_ivImageView.SetMultiSelectMode(true);
			this.m_btDelete.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3214"));
			this.m_btDelete.SetButtonTextureKey("Win_B_AcceptBtn02");
			this.m_State = Inventory_Dlg.eSTATE.eSTATE_MULTIDELETE;
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("836"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
		}
		else
		{
			if (this.c_ivImageView.SelectedItems.Count == 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("837"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
			else
			{
				this.MultiIemDelete();
			}
			this.InitMultiItem();
		}
		this.HideUIGuide();
	}

	private void onClickItemLock(IUIObject obj)
	{
		if (this.m_State != Inventory_Dlg.eSTATE.eSTATE_MULTILOCK)
		{
			this.InitMultiItem();
			this.m_btLock.SetButtonTextureKey("Win_B_AcceptBtn02");
			this.m_State = Inventory_Dlg.eSTATE.eSTATE_MULTILOCK;
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("725"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
		}
		else
		{
			this.InitMultiItem();
		}
	}

	private void OnClickItemBreak(IUIObject obj)
	{
		if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTILOCK || this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTISELL)
		{
			this.InitMultiItem();
		}
		if (!this.c_ivImageView.GetMultiSelectMode())
		{
			this.c_ivImageView.ClearSelectedItems();
			this.c_ivImageView.SetMultiSelectMode(true);
			this.m_btItemBreak.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("777"));
			this.m_btItemBreak.SetButtonTextureKey("Win_B_AcceptBtn02");
			this.m_State = Inventory_Dlg.eSTATE.eSTATE_MULTIBREAK;
			if (this.MultiItemBreakDiable(true) > 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("825"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("11"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
		else
		{
			if (this.c_ivImageView.SelectedItems.Count == 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("207"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
			else
			{
				this.MultiIemBreak();
			}
			this.InitMultiItem();
		}
		this.HideUIGuide();
	}

	public void InitMultiItem()
	{
		this.c_ivImageView.SetMultiSelectMode(false);
		this.c_ivImageView.ClearSelectedItems();
		if (this.m_ckSelectAll != null)
		{
			this.m_ckSelectAll.SetCheckState(0);
		}
		if (this.m_btItemSell != null)
		{
			this.m_btItemSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("79"));
			this.m_btItemSell.SetButtonTextureKey("Win_B_AcceptBtn");
		}
		if (this.m_btLock != null)
		{
			this.m_btLock.SetButtonTextureKey("Win_I_Lock01");
			this.m_btLock.SetButtonTextureKey("Win_B_AcceptBtn");
		}
		if (this.m_btItemBreak != null)
		{
			this.m_btItemBreak.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("420"));
			this.m_btItemBreak.SetButtonTextureKey("Win_B_AcceptBtn");
		}
		if (this.m_btDelete != null)
		{
			this.m_btDelete.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3213"));
			this.m_btDelete.SetButtonTextureKey("Win_B_AcceptBtn");
		}
		if (this.m_lockList.Count != 0)
		{
			this.InitLockList();
		}
		if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTIBREAK)
		{
			this.MultiItemBreakDiable(false);
		}
		if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTISELL)
		{
			this.MultiItemSellDiable(false);
		}
		this.m_State = Inventory_Dlg.eSTATE.eSTATE_NONE;
	}

	public void InitLockList()
	{
		if (this.m_lockList.Count == 0)
		{
			return;
		}
		GS_ITEM_LOCK_MULTI_REQ gS_ITEM_LOCK_MULTI_REQ = new GS_ITEM_LOCK_MULTI_REQ();
		int num = 0;
		foreach (KeyValuePair<long, UIListItemContainer> current in this.m_lockList)
		{
			if (!(current.Value == null) && current.Value.Data != null)
			{
				ImageSlot imageSlot = current.Value.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM != null && current.Key == iTEM.m_nItemID)
					{
						if (current.Value.IsLocked() != iTEM.IsLock())
						{
							gS_ITEM_LOCK_MULTI_REQ.i64ItemID[num] = current.Key;
							gS_ITEM_LOCK_MULTI_REQ.bLocked[num] = current.Value.IsLocked();
							current.Value.SetLocked(iTEM.IsLock());
							num++;
						}
					}
				}
			}
		}
		this.m_lockList.Clear();
		if (num > 0)
		{
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_LOCK_MULTI_REQ, gS_ITEM_LOCK_MULTI_REQ);
		}
	}

	private bool MultiIemSell()
	{
		int num = 0;
		List<ITEM> list = new List<ITEM>();
		ITEM iTEM = null;
		int num2 = 0;
		bool bRankItem = false;
		foreach (KeyValuePair<int, UIListItemContainer> current in this.c_ivImageView.SelectedItems)
		{
			UIListItemContainer value = current.Value;
			if (value != null)
			{
				ImageSlot imageSlot = value.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM2 = imageSlot.c_oItem as ITEM;
					if (iTEM2 != null && iTEM2.m_nItemUnique > 0 && !iTEM2.IsLock())
					{
						switch (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(iTEM2.m_nItemUnique))
						{
						case eITEM_PART.ITEMPART_WEAPON:
						case eITEM_PART.ITEMPART_ARMOR:
						case eITEM_PART.ITEMPART_ACCESSORY:
						{
							ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM2.m_nItemUnique);
							if (itemInfo == null)
							{
								bool result = false;
								return result;
							}
							ITEM_SELL itemSellData = NrTSingleton<ITEM_SELL_Manager>.Instance.GetItemSellData(itemInfo.m_nQualityLevel, (int)iTEM2.GetRank());
							if (itemSellData != null)
							{
								num += itemSellData.nItemSellMoney;
								if (iTEM == null)
								{
									iTEM = iTEM2;
									num2 = itemSellData.nItemSellMoney;
								}
								if (num2 < itemSellData.nItemSellMoney)
								{
									iTEM = iTEM2;
									num2 = itemSellData.nItemSellMoney;
								}
								if (iTEM2.GetRank() >= eITEM_RANK_TYPE.ITEM_RANK_A)
								{
									bRankItem = true;
								}
								list.Add(iTEM2);
							}
							break;
						}
						default:
						{
							bool result = false;
							return result;
						}
						}
					}
				}
			}
		}
		if (list.Count > 0)
		{
			ItemSellDlg itemSellDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMSELL_DLG) as ItemSellDlg;
			if (itemSellDlg != null)
			{
				itemSellDlg.SetMultiItemSellInfo(list, num, iTEM, bRankItem);
				return true;
			}
		}
		return false;
	}

	private bool MultiIemBreak()
	{
		this.m_ItemBreakList.Clear();
		ITEM iTEM = null;
		int num = 0;
		bool bRankItem = false;
		int num2 = 0;
		int num3 = 0;
		int itemUnique = 0;
		foreach (KeyValuePair<int, UIListItemContainer> current in this.c_ivImageView.SelectedItems)
		{
			UIListItemContainer value = current.Value;
			if (value != null)
			{
				ImageSlot imageSlot = value.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM2 = imageSlot.c_oItem as ITEM;
					if (iTEM2 != null && iTEM2.m_nItemUnique > 0 && !iTEM2.IsLock())
					{
						switch (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(iTEM2.m_nItemUnique))
						{
						case eITEM_PART.ITEMPART_WEAPON:
						case eITEM_PART.ITEMPART_ARMOR:
						case eITEM_PART.ITEMPART_ACCESSORY:
						{
							ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM2.m_nItemUnique);
							if (itemInfo != null)
							{
								ITEM_BREAK iTEM_BREAK = NrTSingleton<Item_Break_Manager>.Instance.Get_RankData(itemInfo.m_nQualityLevel, 6);
								if (iTEM_BREAK != null)
								{
									int num4 = iTEM2.m_nOption[5];
									num4 += iTEM2.m_nOption[9];
									num2 += iTEM_BREAK.i32ItemNum_Min + num4 * iTEM_BREAK.i32Skill_increase_itemnum;
									num3 += iTEM_BREAK.i32ItemNum_Max + num4 * iTEM_BREAK.i32Skill_increase_itemnum;
									itemUnique = iTEM_BREAK.i32ItemUnique;
									if (iTEM == null)
									{
										iTEM = iTEM2;
										num = itemInfo.m_nQualityLevel;
									}
									if (num < itemInfo.m_nQualityLevel)
									{
										iTEM = iTEM2;
										num = itemInfo.m_nQualityLevel;
									}
									if (iTEM2.GetRank() >= eITEM_RANK_TYPE.ITEM_RANK_SS)
									{
										bRankItem = true;
										this.m_ItemBreakList.Add(iTEM2);
									}
								}
							}
							break;
						}
						default:
							return false;
						}
					}
				}
			}
		}
		if (this.m_ItemBreakList.Count > 0)
		{
			ItemSellDlg itemSellDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMSELL_DLG) as ItemSellDlg;
			if (itemSellDlg != null)
			{
				itemSellDlg.SetMultiItemBreakInfo(this.m_ItemBreakList, iTEM, bRankItem, itemUnique, num2, num3);
				return true;
			}
		}
		return false;
	}

	private bool MultiIemDelete()
	{
		List<ITEM> list = new List<ITEM>();
		foreach (KeyValuePair<int, UIListItemContainer> current in this.c_ivImageView.SelectedItems)
		{
			UIListItemContainer value = current.Value;
			if (value != null)
			{
				ImageSlot imageSlot = value.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM != null && iTEM.m_nItemUnique > 0 && !iTEM.IsLock())
					{
						if (NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique) == null)
						{
							return false;
						}
						list.Add(iTEM);
					}
				}
			}
		}
		if (list.Count > 0)
		{
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3213"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("420"), eMsgType.MB_OK_CANCEL, new YesDelegate(this.SendServerItemDelete), list);
		}
		return false;
	}

	private void Money_Refresh()
	{
		this.m_laMoney.Text = Protocol_Item.Money_Format(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money);
	}

	private void Child_Close()
	{
	}

	private void Select_Move(int a_nPosItem)
	{
		if (null == this.m_goSelect)
		{
			return;
		}
		if (!this.m_goSelect.activeInHierarchy)
		{
			this.m_goSelect.SetActive(true);
		}
		IUIListObject item = this.c_ivImageView.GetItem(a_nPosItem);
		if (item == null)
		{
			return;
		}
		this.m_vSelectPosition.x = item.gameObject.transform.position.x + 114f * base.InteractivePanel.transform.localScale.x / 2f;
		this.m_vSelectPosition.y = item.gameObject.transform.position.y - 114f * base.InteractivePanel.transform.localScale.y / 2f;
		this.m_vSelectPosition.z = item.gameObject.transform.position.z - 0.1f;
		this.m_goSelect.transform.position = this.m_vSelectPosition;
	}

	private void On_Sort_Button(IUIObject obj)
	{
		byte b = this.ChangeTabIndexToPostype(this.m_tbToolbar.SeletedToolIndex);
		int num = NkUserInventory.GetInstance().Get_Tab_List_Count((int)b);
		if (num > 0 && Protocol_Item.Is_Sort((int)b))
		{
			GS_ITEM_INVEN_SORT_REQ gS_ITEM_INVEN_SORT_REQ = new GS_ITEM_INVEN_SORT_REQ();
			gS_ITEM_INVEN_SORT_REQ.m_byPosType = (int)b;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_INVEN_SORT_REQ, gS_ITEM_INVEN_SORT_REQ);
		}
	}

	public byte ChangeTabIndexToPostype(int TabIndex)
	{
		byte result = 0;
		switch ((byte)TabIndex)
		{
		case 0:
			result = 1;
			break;
		case 1:
			result = 2;
			break;
		case 2:
			result = 3;
			break;
		case 3:
			result = 4;
			break;
		case 4:
			result = 5;
			break;
		case 5:
			result = 6;
			break;
		}
		return result;
	}

	public int ChangePostypeToTabIndex(byte posType)
	{
		int result = 0;
		switch (posType)
		{
		case 1:
			result = 0;
			break;
		case 2:
			result = 1;
			break;
		case 3:
			result = 2;
			break;
		case 4:
			result = 3;
			break;
		case 5:
			result = 4;
			break;
		case 6:
			result = 5;
			break;
		}
		return result;
	}

	private void On_Tab_Button(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		Inventory_Dlg.s_byTab = this.ChangeTabIndexToPostype(uIPanelTab.panel.index);
		this.InitMultiItem();
		if (Inventory_Dlg.s_byTab == 1 || Inventory_Dlg.s_byTab == 2 || Inventory_Dlg.s_byTab == 3 || Inventory_Dlg.s_byTab == 4)
		{
			if (this.m_btItemSell != null)
			{
				this.m_btItemSell.Visible = true;
			}
			if (this.m_btItemBreak != null)
			{
				this.m_btItemBreak.Visible = true;
			}
			if (this.m_ckSelectAll != null)
			{
				this.m_ckSelectAll.Visible = true;
			}
			if (this.m_lbSelectAll != null)
			{
				this.m_lbSelectAll.Visible = true;
			}
			if (this.m_btDelete != null)
			{
				this.m_btDelete.Visible = false;
			}
			this.m_btLock.Hide(false);
		}
		else
		{
			if (this.m_btItemSell != null)
			{
				this.m_btItemSell.Visible = false;
			}
			if (this.m_btItemBreak != null)
			{
				this.m_btItemBreak.Visible = false;
			}
			if (this.m_ckSelectAll != null)
			{
				this.m_ckSelectAll.Visible = false;
			}
			if (this.m_lbSelectAll != null)
			{
				this.m_lbSelectAll.Visible = false;
			}
			if (this.m_btDelete != null)
			{
				this.m_btDelete.Visible = true;
			}
			this.m_btLock.Hide(true);
		}
		this.Item_Draw((int)Inventory_Dlg.s_byTab);
		NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(63, 0L, 1L);
	}

	public override void Update()
	{
		if (this.m_BlinkPosType > 0 && this.m_BlinkCount > 0)
		{
			if (this.mTime < Time.time)
			{
				if (this.m_BlinkValue == 1f)
				{
					this.SetBlinkValue(this.m_BlinkPosType, 0.3f);
				}
				else
				{
					this.SetBlinkValue(this.m_BlinkPosType, 1f);
				}
				this.mTime = Time.time + 0.5f;
				this.m_BlinkCount--;
			}
		}
		else if (this.m_BlinkPosType > 0)
		{
			this.SetBlinkValue(this.m_BlinkPosType, 1f);
			this.m_BlinkPosType = 0;
			this.m_BlinkCount = 0;
			this.mTime = 0f;
		}
		if (this.bLoadActionReforge && null != this.rootGameObject && null != this.aniGameObject && !this.aniGameObject.isPlaying)
		{
			UnityEngine.Object.DestroyImmediate(this.rootGameObject);
			this.bLoadActionReforge = false;
			this.bRequest = false;
			this.aniGameObject = null;
			TsAudio.RestoreMuteAllAudio();
			TsAudio.RefreshAllMuteAudio();
			this.SendServerItemBreak();
			NkInputManager.IsInputMode = true;
		}
	}

	public void SetBlinkValueStart(int PosType)
	{
		this.m_BlinkPosType = PosType;
		this.m_BlinkCount = 10;
		this.mTime = Time.time;
	}

	private void SetBlinkValue(int PosType, float fValue)
	{
		if (PosType <= 0 || PosType > this.m_tbToolbar.Count)
		{
			return;
		}
		this.m_BlinkValue = fValue;
		this.m_tbToolbar.Control_Tab[this.ChangePostypeToTabIndex((byte)PosType)].SetAlphaBG(fValue);
		this.m_tbToolbar.Control_Tab[this.ChangePostypeToTabIndex((byte)PosType)].SetAlphaText(fValue);
	}

	public void SetTap(int PosType)
	{
		this.m_tbToolbar.SetSelectTabIndex(this.ChangePostypeToTabIndex((byte)PosType));
	}

	public void ResetItemLock()
	{
		for (int i = 0; i < this.c_ivImageView.Count; i++)
		{
			UIListItemContainer item = this.c_ivImageView.GetItem(i);
			if (!(item == null))
			{
				ImageSlot imageSlot = item.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM != null && iTEM.m_nItemID > 0L)
					{
						item.SetLocked(iTEM.IsLock());
					}
				}
			}
		}
	}

	public int GetCurrentTap()
	{
		return this.m_tbToolbar.SeletedToolIndex;
	}

	public void Refresh_PosType(int a_byPosType, int a_shPosItem)
	{
		if (a_byPosType != (int)Inventory_Dlg.s_byTab)
		{
			return;
		}
		this.Item_Draw(a_byPosType, a_shPosItem);
		ITEM item = NkUserInventory.GetInstance().GetItem(a_byPosType, a_shPosItem);
		if (item != null)
		{
			this.Select_Move(a_shPosItem);
		}
		else
		{
			this.m_goSelect.SetActive(false);
		}
	}

	public void Set_Enable_Slot(byte a_byPosType, short a_shPosItem)
	{
		if (!TsPlatform.IsMobile)
		{
			Inventory_Dlg.s_byTab = a_byPosType;
		}
	}

	private void _funcDownloaded(IDownloadedItem wItem, object obj)
	{
		if (base.IsDestroy())
		{
			return;
		}
		GameObject gameObject = obj as GameObject;
		if (gameObject == null)
		{
			return;
		}
		if (wItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
		}
		else
		{
			GameObject original = wItem.mainAsset as GameObject;
			float num = 24f;
			float num2 = 26f;
			float x = 0.93f;
			float y = 0.93f;
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity);
			NkUtil.SetAllChildLayer(gameObject2, GUICamera.UILayer);
			Vector3 position = new Vector3(gameObject.transform.position.x + num, gameObject.transform.position.y - num2, gameObject.transform.position.z - 1.9f);
			gameObject2.transform.position = position;
			gameObject2.transform.localScale = new Vector3(x, y, 1f);
			gameObject2.transform.parent = gameObject.transform.parent;
			gameObject2.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			this.Item_Draw((int)Inventory_Dlg.s_byTab);
			wItem.unloadImmediate = true;
		}
	}

	public void Item_Draw(int posType, int pos)
	{
		if (this.m_goSelect.activeInHierarchy)
		{
			this.m_goSelect.SetActive(false);
		}
		this.m_tbToolbar.SetSelectTabIndex(this.ChangePostypeToTabIndex((byte)posType));
		int num = Protocol_Item.Get_Enable_Slot_Count(posType);
		ImageSlot imageSlot = new ImageSlot();
		if (num <= pos)
		{
			imageSlot.c_bDisable = true;
		}
		else
		{
			imageSlot.c_bDisable = false;
		}
		int num2 = Protocol_Item.Is_Enable_Slot(posType, (short)((byte)pos));
		if (num2 > 0)
		{
			imageSlot.p_nAddEnableSlot = num2;
		}
		ITEM item = NkUserInventory.GetInstance().GetItem(posType, pos);
		if (item != null && item.IsValid())
		{
			imageSlot.c_oItem = item;
			imageSlot.c_bEnable = true;
			imageSlot.Index = item.m_nItemPos;
			imageSlot.imageStr = "Win_T_ItemEmpty";
			imageSlot.itemunique = item.m_nItemUnique;
			imageSlot.WindowID = base.WindowID;
			if (item.m_nItemNum > 1)
			{
				imageSlot.SlotInfo._visibleNum = true;
			}
			if (Protocol_Item.Is_EquipItem(item.m_nItemUnique))
			{
				imageSlot.SlotInfo._visibleRank = true;
			}
			imageSlot.SlotInfo.Set(item.m_nItemNum.ToString(), "+" + item.m_nRank.ToString());
		}
		else
		{
			imageSlot.c_oItem = null;
			imageSlot.Index = pos;
			imageSlot.imageStr = "Win_T_ItemEmpty";
			imageSlot.SlotInfo.Set("0", "0");
			imageSlot.WindowID = base.WindowID;
		}
		imageSlot._solID = this.solID;
		this.c_ivImageView.SetImageSlot(pos, imageSlot, new EZDragDropDelegate(this.DragDrop), new EZValueChangedDelegate(this.On_Right_Mouse_Up), null, null);
		if (null != this.c_cCopyItem)
		{
			UnityEngine.Object.Destroy(this.c_cCopyItem.gameObject);
		}
	}

	public void Item_Draw(int a_byPosType)
	{
		if (this.m_goSelect.activeInHierarchy)
		{
			this.m_goSelect.SetActive(false);
		}
		this.m_tbToolbar.SetSelectTabIndex(this.ChangePostypeToTabIndex((byte)a_byPosType));
		this.c_ivImageView.Clear();
		int num = Protocol_Item.Get_Enable_Slot_Count(a_byPosType);
		for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
		{
			ImageSlot imageSlot = new ImageSlot();
			if (num <= i)
			{
				imageSlot.c_bDisable = true;
			}
			else
			{
				imageSlot.c_bDisable = false;
			}
			int num2 = Protocol_Item.Is_Enable_Slot(a_byPosType, (short)((byte)i));
			if (num2 > 0)
			{
				imageSlot.p_nAddEnableSlot = num2;
			}
			ITEM item = NkUserInventory.GetInstance().GetItem(a_byPosType, i);
			if (item != null && item.IsValid())
			{
				imageSlot.c_oItem = item;
				imageSlot.c_bEnable = true;
				imageSlot.Index = item.m_nItemPos;
				imageSlot.imageStr = "Win_T_ItemEmpty";
				imageSlot.itemunique = item.m_nItemUnique;
				imageSlot.WindowID = base.WindowID;
				if (item.m_nItemNum > 1)
				{
					imageSlot.SlotInfo._visibleNum = true;
				}
				if (Protocol_Item.Is_EquipItem(item.m_nItemUnique))
				{
					imageSlot.SlotInfo._visibleRank = true;
				}
				imageSlot.SlotInfo.Set(item.m_nItemNum.ToString(), "+" + item.m_nRank.ToString());
			}
			else
			{
				imageSlot.c_oItem = null;
				imageSlot.Index = i;
				imageSlot.imageStr = "Win_T_ItemEmpty";
				imageSlot.SlotInfo.Set("0", "0");
				imageSlot.WindowID = base.WindowID;
			}
			imageSlot._solID = this.solID;
			this.c_ivImageView.SetImageSlot(i, imageSlot, new EZDragDropDelegate(this.DragDrop), new EZValueChangedDelegate(this.On_Right_Mouse_Up), null, null);
		}
		this.c_ivImageView.RepositionItems();
		if (null != this.c_cCopyItem)
		{
			UnityEngine.Object.Destroy(this.c_cCopyItem.gameObject);
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
				if (imageSlot != null)
				{
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM != null && iTEM.m_nItemUnique > 0)
					{
						ITEM iTEM2 = null;
						SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
						if (solMilitaryGroupDlg != null && solMilitaryGroupDlg.Visible)
						{
							NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_SECOND_DLG);
							NkSoldierInfo selectedSolinfo = solMilitaryGroupDlg.GetSelectedSolinfo();
							if (selectedSolinfo != null)
							{
								iTEM2 = selectedSolinfo.GetEquipItemByUnique(iTEM.m_nItemUnique);
								if (iTEM2 != null && iTEM2.IsValid())
								{
									ItemTooltipDlg_Second itemTooltipDlg_Second = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_SECOND_DLG) as ItemTooltipDlg_Second;
									itemTooltipDlg_Second.Set_Tooltip((G_ID)base.WindowID, iTEM2, true, Vector3.zero, 0L);
								}
							}
						}
						ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
						itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, iTEM2, false);
					}
					else if (imageSlot.c_bDisable)
					{
					}
				}
			}
		}
	}

	private void On_Mouse_Out(IUIObject a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_SECOND_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_SECOND_DLG);
	}

	private void On_ToolTip(IUIObject a_oObject)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		if (this.c_ivImageView.GetMultiSelectMode())
		{
			return;
		}
		if (a_oObject == null)
		{
			return;
		}
		UIScrollList uIScrollList = a_oObject as UIScrollList;
		if (uIScrollList == null)
		{
			return;
		}
		UIListItemContainer uIListItemContainer;
		if (TsPlatform.IsMobile)
		{
			uIListItemContainer = uIScrollList.SelectedItem;
		}
		else
		{
			uIListItemContainer = uIScrollList.MouseItem;
		}
		if (uIListItemContainer == null)
		{
			return;
		}
		ImageSlot imageSlot = uIListItemContainer.Data as ImageSlot;
		if (imageSlot != null)
		{
			ITEM iTEM = imageSlot.c_oItem as ITEM;
			if (iTEM != null && iTEM.m_nItemUnique > 0)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
				Protocol_Item.Item_ShowItemInfo((G_ID)base.WindowID, iTEM, Vector3.zero, null, 0L);
			}
		}
	}

	private void On_Right_Mouse_Up(IUIObject a_oObject)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		if (a_oObject == null)
		{
			return;
		}
		UIListItemContainer uIListItemContainer = (UIListItemContainer)a_oObject;
		if (null == uIListItemContainer)
		{
			return;
		}
		ITEM iTEM = null;
		ImageSlot imageSlot = uIListItemContainer.Data as ImageSlot;
		if (imageSlot != null)
		{
			iTEM = (imageSlot.c_oItem as ITEM);
		}
		if (iTEM == null)
		{
			return;
		}
		this.HideUIGuide();
		if (this.c_ivImageView.GetMultiSelectMode())
		{
			if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTISELL)
			{
				if (this.c_ivImageView.SelectedItems.Count == 0)
				{
					this.m_btItemSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("78"));
					this.InitMultiItem();
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("207"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				}
				else if (this.c_ivImageView.SelectedItems.Count < 30)
				{
					this.m_btItemSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("78"));
				}
				else if (this.c_ivImageView.SelectedItems.Count > 30)
				{
					this.m_btItemSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("78"));
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("205"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				}
				if (uIListItemContainer.IsDisable())
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("201"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					this.c_ivImageView.RemoveSelectedItems(uIListItemContainer);
				}
				if (iTEM.m_nLock > 0)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("726"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				}
			}
			else if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTIBREAK)
			{
				if (this.c_ivImageView.SelectedItems.Count == 0)
				{
					this.m_btItemBreak.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("420"));
					this.InitMultiItem();
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("207"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				}
				else if (this.c_ivImageView.SelectedItems.Count < 30)
				{
					this.m_btItemBreak.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("420"));
				}
				else if (this.c_ivImageView.SelectedItems.Count > 30)
				{
					this.m_btItemBreak.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("420"));
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("205"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				}
				if (uIListItemContainer.IsDisable() || iTEM.m_nLock > 0)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("826"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				}
			}
			else if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTIDELETE)
			{
				if (this.c_ivImageView.SelectedItems.Count == 0)
				{
					this.InitMultiItem();
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("207"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				}
				else if (this.c_ivImageView.SelectedItems.Count < 30)
				{
					this.m_btDelete.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3213"));
				}
			}
			if (iTEM.m_nLock > 0 || uIListItemContainer.IsDisable())
			{
				this.c_ivImageView.RemoveSelectedItems(uIListItemContainer);
			}
			return;
		}
		if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTILOCK)
		{
			uIListItemContainer.SetSelected(false);
			uIListItemContainer.SetLocked(!uIListItemContainer.IsLocked());
			this.m_lockList[iTEM.m_nItemID] = uIListItemContainer;
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.POST_DLG))
		{
			PostDlg postDlg = (PostDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG);
			if (postDlg != null)
			{
				if (NrTSingleton<ItemManager>.Instance.IsItemATB(iTEM.m_nItemUnique, 2L) || NrTSingleton<ItemManager>.Instance.IsItemATB(iTEM.m_nItemUnique, 4L))
				{
					postDlg.SetItem(iTEM);
				}
				this.Close();
				return;
			}
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
		if (itemInfo != null && itemInfo.IsItemATB(16L) && !itemInfo.IsItemATB(16384L))
		{
			ItemBoxContinue_Dlg itemBoxContinue_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_CONTINUE_DLG) as ItemBoxContinue_Dlg;
			if (itemBoxContinue_Dlg != null)
			{
				itemBoxContinue_Dlg.SetItemData(iTEM, ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_RANDOMBOX);
			}
		}
		else if (NrTSingleton<ItemManager>.Instance.isItemGoldBar(iTEM.m_nItemUnique))
		{
			ItemBoxContinue_Dlg itemBoxContinue_Dlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_CONTINUE_DLG) as ItemBoxContinue_Dlg;
			if (itemBoxContinue_Dlg2 != null)
			{
				itemBoxContinue_Dlg2.SetItemData(iTEM, ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_GOLDBAR);
			}
		}
		else if (NrTSingleton<ItemManager>.Instance.isExchangeItem(iTEM.m_nItemUnique))
		{
			ItemBoxContinue_Dlg itemBoxContinue_Dlg3 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_CONTINUE_DLG) as ItemBoxContinue_Dlg;
			if (itemBoxContinue_Dlg3 != null)
			{
				itemBoxContinue_Dlg3.SetItemData(iTEM, ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_EXCHANGE);
			}
		}
		else if (NrTSingleton<ItemManager>.Instance.isBattleSpeedItem(iTEM.m_nItemUnique))
		{
			ItemBoxContinue_Dlg itemBoxContinue_Dlg4 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_CONTINUE_DLG) as ItemBoxContinue_Dlg;
			if (itemBoxContinue_Dlg4 != null)
			{
				itemBoxContinue_Dlg4.SetItemData(iTEM, ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_BATTLESPEED);
			}
		}
		else
		{
			ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
		}
	}

	private void CreateRightClickUI(ITEM itemdata)
	{
		CRightClickMenu.TYPE formType = CRightClickMenu.TYPE.SIMPLE_SECTION_1;
		if (TsPlatform.IsMobile)
		{
			formType = CRightClickMenu.TYPE.NAME_SECTION_1;
		}
		if (Protocol_Item.Enable_Compose(itemdata.m_nItemUnique))
		{
			NrTSingleton<CRightClickMenu>.Instance.CreateUI(itemdata, CRightClickMenu.KIND.ITEM_COMPOSE_CLICK, formType);
		}
		else if (Protocol_Item.Enable_Use(itemdata.m_nItemUnique))
		{
			NrTSingleton<CRightClickMenu>.Instance.CreateUI(itemdata, CRightClickMenu.KIND.ITEM_USABLE_CLICK, formType);
		}
		else if (Protocol_Item.Is_EquipItem(itemdata.m_nItemUnique))
		{
			if (itemdata.m_nPosType != 10)
			{
				NrTSingleton<CRightClickMenu>.Instance.CreateUI(itemdata, CRightClickMenu.KIND.ITEM_NOTEQUIP_CLICK, formType);
			}
		}
		else
		{
			NrTSingleton<CRightClickMenu>.Instance.CreateUI(itemdata, CRightClickMenu.KIND.ITEM_UNUSABLE_CLICK, formType);
		}
	}

	private void On_Double_Click(IUIObject a_oObject)
	{
	}

	private void On_Mouse_Click(IUIObject a_oObject)
	{
		ImageView imageView = a_oObject as ImageView;
		if (imageView == null)
		{
			return;
		}
		IUIListObject mouseItem = imageView.MouseItem;
		if (mouseItem == null)
		{
			return;
		}
		ImageSlot imageSlot = mouseItem.Data as ImageSlot;
		if (imageSlot == null)
		{
			return;
		}
		if (imageSlot.itemunique > 0 && !imageSlot.c_bDisable && !this.c_ivImageView.GetMultiSelectMode())
		{
			this.Select_Move(imageSlot.Index);
			string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(imageSlot.itemunique);
			if (!string.IsNullOrEmpty(itemMaterialCode))
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_ITEM", itemMaterialCode, "SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
			ITEM iTEM = imageSlot.c_oItem as ITEM;
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.POST_DLG))
			{
				PostDlg postDlg = (PostDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG);
				if (postDlg != null && iTEM != null)
				{
					if (NrTSingleton<ItemManager>.Instance.IsItemATB(iTEM.m_nItemUnique, 2L) || NrTSingleton<ItemManager>.Instance.IsItemATB(iTEM.m_nItemUnique, 4L))
					{
						postDlg.SetItem(iTEM);
					}
					this.Close();
					return;
				}
			}
			if (NkInputManager.GetKey(KeyCode.LeftShift) && iTEM != null && NrTSingleton<FormsManager>.Instance.IsShow(G_ID.CHAT_MAIN_DLG))
			{
				ChatManager.AddItemLink(iTEM);
			}
		}
	}

	public void DragDrop(EZDragDropParams dragDropParams)
	{
		if (dragDropParams.evt == EZDragDropEvent.Begin)
		{
			this.StartDragItem(dragDropParams);
		}
		else
		{
			if (dragDropParams.evt == EZDragDropEvent.Dropped)
			{
				if (dragDropParams.dragObj.DropTarget != null)
				{
					ITEM iTEM = null;
					if (dragDropParams.dragObj.Data != null)
					{
						ImageSlot imageSlot = dragDropParams.dragObj.Data as ImageSlot;
						if (imageSlot != null)
						{
							iTEM = (imageSlot.c_oItem as ITEM);
							if (iTEM == null)
							{
								goto IL_27C;
							}
						}
						if (!(null == dragDropParams.dragObj.DropTarget.transform))
						{
							Transform parent = dragDropParams.dragObj.DropTarget.transform.parent;
							if (!(null == parent))
							{
								string name = dragDropParams.dragObj.DropTarget.name;
								if (!this.ItemDrag(iTEM, parent, name, G_ID.CHAT_MAIN_DLG))
								{
									if (!this.ItemDrag(iTEM, parent, name, G_ID.MARKET_SELL_DLG))
									{
										if (name == this.m_strDeleteName)
										{
											if (iTEM.IsLock())
											{
												return;
											}
											Protocol_Item.DeleteItem(iTEM);
										}
										else
										{
											UIListItemContainer component = dragDropParams.dragObj.DropTarget.GetComponent<UIListItemContainer>();
											if (component == null)
											{
												component = parent.GetComponent<UIListItemContainer>();
												if (component == null)
												{
													goto IL_27C;
												}
											}
											if (component.Data == null)
											{
												goto IL_27C;
											}
											ImageSlot imageSlot2 = component.Data as ImageSlot;
											if (imageSlot2 == null)
											{
												goto IL_27C;
											}
											G_ID windowID = (G_ID)imageSlot2.WindowID;
											if (windowID != G_ID.POST_DLG)
											{
												if (windowID == G_ID.REFORGEMAIN_DLG)
												{
													ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
													if (reforgeMainDlg != null)
													{
														reforgeMainDlg.OnItemDragDrop(imageSlot);
													}
												}
											}
											else
											{
												PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
												if (postDlg != null)
												{
													postDlg.OnItemDragDrop(imageSlot);
												}
											}
											if (!Protocol_Item.MoveItem(dragDropParams))
											{
												goto IL_27C;
											}
											dragDropParams.dragObj.DropHandled = true;
										}
										string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(iTEM.m_nItemUnique);
										TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_ITEM", itemMaterialCode, "DROP", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
										return;
									}
								}
							}
						}
					}
				}
				IL_27C:
				if (null != this.c_cCopyItem)
				{
					UnityEngine.Object.Destroy(this.c_cCopyItem.gameObject);
					this.c_cCopyItem = null;
				}
				return;
			}
			if (dragDropParams.evt == EZDragDropEvent.Cancelled)
			{
				this.CancelDragItem(dragDropParams);
			}
			else if (dragDropParams.evt == EZDragDropEvent.Update)
			{
			}
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
	}

	private bool ItemDrag(ITEM a_cItem, Transform a_trParent, string a_strTargetName, G_ID a_eFormID)
	{
		string b = a_eFormID.ToString();
		if (a_strTargetName == b || (a_trParent != null && a_trParent.name == b))
		{
			Form form = NrTSingleton<FormsManager>.Instance.GetForm(a_eFormID);
			if (form != null)
			{
				form.Set_Value(a_cItem);
			}
			return true;
		}
		return false;
	}

	private void StartDragItem(EZDragDropParams dragDropParams)
	{
		if (null != this.c_cCopyItem)
		{
			UnityEngine.Object.Destroy(this.c_cCopyItem.gameObject);
			this.c_cCopyItem = null;
		}
		if (dragDropParams.dragObj == null)
		{
			return;
		}
		ImageSlot imageSlot = dragDropParams.dragObj.Data as ImageSlot;
		if (imageSlot != null && imageSlot.itemunique > 0)
		{
			this.c_cCopyItem = this.c_ivImageView.MakeListItem(0, imageSlot, null, null, null, null);
			if (null == this.c_cCopyItem)
			{
				return;
			}
			this.c_cCopyItem.transform.position = dragDropParams.dragObj.transform.position;
			this.c_cCopyItem.transform.localScale = base.InteractivePanel.transform.localScale;
			this.c_cCopyItem.FadeListItemContainer();
			dragDropParams.dragObj.DropHandled = false;
			string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(imageSlot.itemunique);
			if (!string.IsNullOrEmpty(itemMaterialCode))
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_ITEM", itemMaterialCode, "SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
			ITEM iTEM = imageSlot.c_oItem as ITEM;
			if (iTEM != null)
			{
				this.Select_Move(iTEM.m_nItemPos);
			}
		}
	}

	private void CancelDragItem(EZDragDropParams dragDropParams)
	{
		ImageSlot imageSlot = dragDropParams.dragObj.Data as ImageSlot;
		if (imageSlot != null && imageSlot.itemunique > 0)
		{
			string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(imageSlot.itemunique);
			if (!string.IsNullOrEmpty(itemMaterialCode))
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_ITEM", itemMaterialCode, "DROP", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
		}
		if (null != this.c_cCopyItem)
		{
			UnityEngine.Object.Destroy(this.c_cCopyItem.gameObject);
			this.c_cCopyItem = null;
		}
	}

	public void Set_Parent(G_ID a_eParentForm)
	{
		this.m_eParentForm = a_eParentForm;
		this.Show();
	}

	public bool Set_Parent_Item(ITEM a_cItem)
	{
		bool result = false;
		if (this.m_eParentForm != G_ID.NONE)
		{
			Form form = NrTSingleton<FormsManager>.Instance.GetForm(this.m_eParentForm);
			if (form != null)
			{
				form.Set_Value(a_cItem);
				result = true;
			}
		}
		return result;
	}

	public void MultiSelect(IUIObject a_oObject)
	{
		if (this.m_ckSelectAll.IsChecked())
		{
			this.c_ivImageView.SetMultiSelectMode(true);
			for (int i = 0; i < this.c_ivImageView.Count; i++)
			{
				UIListItemContainer item = this.c_ivImageView.GetItem(i);
				if (!(item == null))
				{
					ImageSlot imageSlot = item.Data as ImageSlot;
					if (imageSlot != null)
					{
						ITEM iTEM = imageSlot.c_oItem as ITEM;
						if (iTEM != null && iTEM.m_nItemID > 0L)
						{
							this.c_ivImageView.AutoMultiClickItem(item, this.m_ckSelectAll.IsChecked());
						}
					}
				}
			}
		}
		else
		{
			this.c_ivImageView.ClearSelectedItems();
			if (this.m_State != Inventory_Dlg.eSTATE.eSTATE_MULTISELL && this.m_State != Inventory_Dlg.eSTATE.eSTATE_MULTIBREAK)
			{
				this.c_ivImageView.SetMultiSelectMode(false);
			}
		}
		if (this.m_ckSelectAll.IsChecked())
		{
			if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTISELL)
			{
				this.m_btItemSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("78"));
			}
			else if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTIBREAK)
			{
				this.m_btItemBreak.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("420"));
			}
			else if (this.m_State != Inventory_Dlg.eSTATE.eSTATE_MULTILOCK)
			{
				this.m_btItemSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("78"));
				this.m_btItemSell.SetButtonTextureKey("Win_B_AcceptBtn02");
				this.m_btItemBreak.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("420"));
				this.m_btItemBreak.SetButtonTextureKey("Win_B_AcceptBtn02");
			}
		}
		else if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTISELL)
		{
			this.m_btItemSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("701"));
		}
		else if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTIBREAK)
		{
			this.m_btItemBreak.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("777"));
		}
		else
		{
			if (this.m_btItemBreak != null)
			{
				this.m_btItemBreak.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("420"));
				this.m_btItemBreak.SetButtonTextureKey("Win_B_AcceptBtn");
			}
			if (this.m_btItemSell != null)
			{
				this.m_btItemSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("79"));
				this.m_btItemSell.SetButtonTextureKey("Win_B_AcceptBtn");
			}
		}
	}

	public int MultiItemBreakDiable(bool bDisable)
	{
		int num = 0;
		for (int i = 0; i < this.c_ivImageView.Count; i++)
		{
			UIListItemContainer item = this.c_ivImageView.GetItem(i);
			if (!(item == null))
			{
				ImageSlot imageSlot = item.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM != null && iTEM.m_nItemID > 0L)
					{
						ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
						if (itemInfo != null)
						{
							if (NrTSingleton<Item_Break_Manager>.Instance.Get_RankData(itemInfo.m_nQualityLevel, 6) == null)
							{
								item.SetDisable(bDisable);
							}
							else
							{
								num++;
							}
						}
					}
				}
			}
		}
		return num;
	}

	public void MultiItemSellDiable(bool bDisable)
	{
		for (int i = 0; i < this.c_ivImageView.Count; i++)
		{
			UIListItemContainer item = this.c_ivImageView.GetItem(i);
			if (!(item == null))
			{
				ImageSlot imageSlot = item.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM != null && iTEM.m_nItemID > 0L)
					{
						ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
						if (itemInfo != null)
						{
							if (NrTSingleton<ITEM_SELL_Manager>.Instance.GetItemSellData(itemInfo.m_nQualityLevel, (int)iTEM.GetRank()) == null)
							{
								item.SetDisable(bDisable);
							}
						}
					}
				}
			}
		}
	}

	public void ActionItemBreak()
	{
		if (!this.bRequest)
		{
			string str = string.Format("{0}", "UI/Item/fx_reinforce" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetActionItemBreak), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			this.bRequest = true;
		}
	}

	private void SetActionItemBreak(WWWItem _item, object _param)
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

	public void SendServerItemBreak()
	{
		GS_DISASSEMBLEITEM_REQ gS_DISASSEMBLEITEM_REQ = new GS_DISASSEMBLEITEM_REQ();
		for (int i = 0; i < this.m_ItemBreakList.Count; i++)
		{
			if (this.m_ItemBreakList[i] != null && this.m_ItemBreakList[i].m_nItemUnique > 0)
			{
				gS_DISASSEMBLEITEM_REQ.SrcItemID[i] = this.m_ItemBreakList[i].m_nItemID;
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_DISASSEMBLEITEM_REQ, gS_DISASSEMBLEITEM_REQ);
	}

	public void SendServerItemDelete(object a_oObject)
	{
		List<ITEM> list = a_oObject as List<ITEM>;
		if (list != null)
		{
			GS_ITEM_MULTI_DELETE_REQ gS_ITEM_MULTI_DELETE_REQ = new GS_ITEM_MULTI_DELETE_REQ();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null && list[i].m_nItemUnique > 0)
				{
					gS_ITEM_MULTI_DELETE_REQ.SrcItemID[i] = list[i].m_nItemID;
				}
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_MULTI_DELETE_REQ, gS_ITEM_MULTI_DELETE_REQ);
		}
	}

	public void OnClickBuyGold(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_GOLD, true);
	}

	public void AddBreakItem(ITEM pItem)
	{
		this.m_ItemBreakList.Clear();
		if (this.m_ItemBreakList != null)
		{
			this.m_ItemBreakList.Add(pItem);
		}
	}
}
