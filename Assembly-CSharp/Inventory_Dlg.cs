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
		eSTATE_MULTILOCK
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

	private Button m_btItemReforge;

	private Button m_btItemSell;

	private Button m_btLock;

	private string m_strDeleteName = string.Empty;

	private string m_strItemReforge = string.Empty;

	private VerticalSlider m_Slider;

	private long solID;

	private float mTime;

	private float m_BlinkValue;

	private int m_BlinkCount;

	private int m_BlinkPosType;

	private static byte s_byTab = 1;

	private GameObject m_goSelect;

	private Vector3 m_vSelectPosition;

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
		this.m_btDelete.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("400");
		this.m_btDelete.Hide(false);
		this.m_strItemReforge = "Button_ReforgeBtn";
		this.m_btItemReforge = (base.GetControl(this.m_strItemReforge) as Button);
		this.m_btItemReforge.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemReforge));
		this.m_btItemSell = (base.GetControl("Button_Sell") as Button);
		this.m_btItemSell.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemSell));
		this.m_btLock = (base.GetControl("Button_Lock01") as Button);
		this.m_btLock.AddValueChangedDelegate(new EZValueChangedDelegate(this.onClickItemLock));
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
			UIPanelTab expr_2AF = this.m_tbToolbar.Control_Tab[i];
			expr_2AF.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_2AF.ButtonClick, new EZValueChangedDelegate(this.On_Tab_Button));
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
		this.Item_Draw((int)Inventory_Dlg.s_byTab);
		base.SetScreenCenter();
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("inven_open");
	}

	public override void OnClose()
	{
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
	}

	private void OnClickItemSell(IUIObject obj)
	{
		if (this.m_State == Inventory_Dlg.eSTATE.eSTATE_MULTILOCK)
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

	public void InitMultiItem()
	{
		this.c_ivImageView.SetMultiSelectMode(false);
		this.c_ivImageView.ClearSelectedItems();
		this.m_State = Inventory_Dlg.eSTATE.eSTATE_NONE;
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
		if (this.m_lockList.Count != 0)
		{
			this.InitLockList();
		}
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
		foreach (KeyValuePair<int, IUIListObject> current in this.c_ivImageView.SelectedItems)
		{
			IUIListObject value = current.Value;
			if (value != null)
			{
				ImageSlot imageSlot = value.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM2 = imageSlot.c_oItem as ITEM;
					if (iTEM2 != null && iTEM2.m_nItemUnique > 0)
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
							if (itemSellData == null)
							{
								bool result = false;
								return result;
							}
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
			this.m_btLock.Hide(false);
		}
		else
		{
			if (this.m_btItemSell != null)
			{
				this.m_btItemSell.Visible = false;
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
			UIListItemContainer uIListItemContainer = this.c_ivImageView.GetItem(i) as UIListItemContainer;
			if (!(uIListItemContainer == null))
			{
				ImageSlot imageSlot = uIListItemContainer.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM != null && iTEM.m_nItemID > 0L)
					{
						uIListItemContainer.SetLocked(iTEM.IsLock());
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
			uIListItemContainer = (uIScrollList.MouseItem as UIListItemContainer);
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
		if (this.c_ivImageView.GetMultiSelectMode())
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
			if (iTEM.m_nLock > 0)
			{
				this.c_ivImageView.RemoveSelectedItems(uIListItemContainer);
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("726"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
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
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
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
								goto IL_2F7;
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
										else if (name == this.m_strItemReforge)
										{
											if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG) && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG))
											{
												NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGERESULT_DLG);
												ReforgeMainDlg reforgeMainDlg = (ReforgeMainDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REFORGEMAIN_DLG);
												reforgeMainDlg.Show();
												reforgeMainDlg.Set_Value(iTEM);
												reforgeMainDlg.SetSolID(0L);
											}
										}
										else
										{
											UIListItemContainer component = dragDropParams.dragObj.DropTarget.GetComponent<UIListItemContainer>();
											if (component == null)
											{
												component = parent.GetComponent<UIListItemContainer>();
												if (component == null)
												{
													goto IL_2F7;
												}
											}
											if (component.Data == null)
											{
												goto IL_2F7;
											}
											ImageSlot imageSlot2 = component.Data as ImageSlot;
											if (imageSlot2 == null)
											{
												goto IL_2F7;
											}
											G_ID windowID = (G_ID)imageSlot2.WindowID;
											if (windowID != G_ID.POST_DLG)
											{
												if (windowID == G_ID.REFORGEMAIN_DLG)
												{
													ReforgeMainDlg reforgeMainDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
													if (reforgeMainDlg2 != null)
													{
														reforgeMainDlg2.OnItemDragDrop(imageSlot);
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
												goto IL_2F7;
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
				IL_2F7:
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
}
