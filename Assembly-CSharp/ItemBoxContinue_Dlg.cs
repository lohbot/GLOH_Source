using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Text;
using UnityForms;

public class ItemBoxContinue_Dlg : Form
{
	public enum SHOW_TYPE
	{
		ITEM_RANDOMBOX,
		ITEM_GOLDBAR,
		ITEM_EXCHANGE,
		ITEM_TICKET,
		ITEM_BATTLESPEED
	}

	private const float _LINE_DOWN_MARGIN = 14f;

	private const float _LINE_END_MARGIN = 92f;

	private Label m_lbItemTypeName;

	private Label m_lbItemTitle;

	private Label m_lbMainOption;

	private FlashLabel m_flMainValue;

	private FlashLabel m_lbSubOption;

	private FlashLabel m_flSubValue;

	private FlashLabel m_lbText;

	private DrawTexture m_txLine;

	private DrawTexture m_txBG;

	private ItemTexture m_itItemTex;

	private HorizontalSlider m_ItemOpenValueSlider;

	private DrawTexture m_ItemOpenValueSliderBG;

	private DrawTexture m_ItemOpenValue_textBG;

	private Button m_ItemOpenValue_Button;

	private Button m_ItemOpenValue_Minus;

	private Button m_ItemOpenValue_Add;

	private Label m_lbItemOpenValue_Text;

	private Button m_btClose;

	private Button m_btAllUse;

	private ITEM m_MainBoxItem;

	private int m_nItemOpenCount = 12;

	private float m_MaxCount;

	private float m_oldItemOpenCount;

	private bool FirstOpen;

	private int m_Multiply = 1;

	private ItemBoxContinue_Dlg.SHOW_TYPE m_eShowType;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/itembox_continuous_dlg", G_ID.ITEM_BOX_CONTINUE_DLG, true);
		base.AlwaysUpdate = true;
		base.SetLocation(base.GetLocation().x, base.GetLocation().y, 92f);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_lbItemTypeName = (base.GetControl("Label_name") as Label);
		this.m_itItemTex = (base.GetControl("ItemTexture_Item") as ItemTexture);
		this.m_lbItemTitle = (base.GetControl("Label_title") as Label);
		this.m_lbMainOption = (base.GetControl("Label_01") as Label);
		this.m_flMainValue = (base.GetControl("FlashLabel_01") as FlashLabel);
		this.m_lbSubOption = (base.GetControl("Label_02") as FlashLabel);
		this.m_flSubValue = (base.GetControl("FlashLabel_02") as FlashLabel);
		this.m_lbText = (base.GetControl("Label_03") as FlashLabel);
		this.m_txLine = (base.GetControl("DrawTexture_MainBorder") as DrawTexture);
		this.m_txBG = (base.GetControl("DrawTexture_BG") as DrawTexture);
		this.m_btClose = (base.GetControl("Button_Button01") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickClose));
		this.m_btAllUse = (base.GetControl("Button_Button02") as Button);
		this.m_btAllUse.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickUse));
		this.m_ItemOpenValueSlider = (base.GetControl("HSlider_HSlider1") as HorizontalSlider);
		this.m_ItemOpenValueSliderBG = (base.GetControl("DrawTexture_SliderBG") as DrawTexture);
		this.m_ItemOpenValue_textBG = (base.GetControl("DrawTexture_Count") as DrawTexture);
		this.m_ItemOpenValue_Button = (base.GetControl("Button_NumPad") as Button);
		this.m_ItemOpenValue_Button.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickInputOpenItem));
		this.m_lbItemOpenValue_Text = (base.GetControl("Label_Count") as Label);
		this.m_ItemOpenValue_Minus = (base.GetControl("Button_MINUS") as Button);
		this.m_ItemOpenValue_Minus.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnValueMinus));
		this.m_ItemOpenValue_Add = (base.GetControl("Button_PLUS") as Button);
		this.m_ItemOpenValue_Add.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnValueAdd));
		base.SetScreenCenter();
		base.SetLocation(base.GetLocationX(), 10f);
	}

	public void RefreshData(GS_BOX_USE_ACK _Ack)
	{
		this.SetItemData(new ITEM
		{
			m_nItemUnique = _Ack.m_lUnique,
			m_nItemNum = _Ack.m_nItemNum,
			m_nPosType = _Ack.m_byPosType,
			m_nItemPos = _Ack.m_shPosItem
		}, ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_RANDOMBOX);
	}

	public void SetItemData(ITEM pItem, ItemBoxContinue_Dlg.SHOW_TYPE type = ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_RANDOMBOX)
	{
		this.m_MainBoxItem = NkUserInventory.instance.GetFirstItemByUnique(pItem.m_nItemUnique);
		if (this.m_MainBoxItem == null)
		{
			this.Close();
			return;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_MainBoxItem.m_nItemUnique);
		if (itemInfo == null)
		{
			return;
		}
		this.m_eShowType = type;
		if (this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_RANDOMBOX)
		{
			this.SetItemRandomBox();
		}
		else if (this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_GOLDBAR)
		{
			this.SetItemGoldBar();
		}
		else if (this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_EXCHANGE)
		{
			this.SetItemExchage();
		}
		else if (this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_TICKET)
		{
			this.SetItemTicket();
		}
		else if (this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_BATTLESPEED)
		{
			this.SetItemBattleSpeed();
		}
		this.m_itItemTex.SetItemTexture(this.m_MainBoxItem);
		this.m_lbItemTitle.SetText(NrTSingleton<ItemManager>.Instance.GetName(this.m_MainBoxItem));
		this.m_lbItemTypeName.SetText(NrTSingleton<ItemManager>.Instance.GetItemTypeName((eITEM_TYPE)itemInfo.m_nItemType));
		ItemOption_Text[] array = ItemTooltipDlg.Get_Item_Info(this.m_MainBoxItem, null, false, true, G_ID.NONE);
		int num = 0;
		if (array.Length > 0)
		{
			if (array[0].m_MainOption)
			{
				this.m_lbMainOption.Text = array[0].m_OptionName;
				this.m_flMainValue.SetFlashLabel(array[0].m_OptionValue);
				num++;
			}
			else
			{
				this.m_lbMainOption.Text = string.Empty;
				this.m_flMainValue.SetFlashLabel(string.Empty);
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		if (array.Length > num)
		{
			for (int i = num; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].m_OptionName);
				stringBuilder2.Append(array[i].m_OptionValue);
			}
		}
		this.m_lbSubOption.SetFlashLabel(stringBuilder.ToString());
		this.m_flSubValue.SetFlashLabel(stringBuilder2.ToString());
		this.m_lbText.SetLocation(this.m_lbText.GetLocation().x, this.m_lbSubOption.GetLocationY() + this.m_lbSubOption.Height + 10f);
		if (itemInfo.m_strToolTipTextKey != "0")
		{
			string textFromItemHelper = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper(itemInfo.m_strToolTipTextKey);
			this.m_lbText.SetFlashLabel(textFromItemHelper);
		}
		float num2 = this.m_lbText.GetLocationY() + this.m_lbText.Height + 14f;
		this.m_txLine.SetLocationY(num2);
		num2 += 14f;
		this.m_ItemOpenValue_Add.SetLocationY(num2);
		this.m_ItemOpenValue_Minus.SetLocationY(num2);
		this.m_ItemOpenValueSlider.SetLocationY(num2 + 24f);
		this.m_ItemOpenValue_textBG.SetLocationY(num2 + 7f);
		this.m_ItemOpenValue_Button.SetLocationY(num2 + 7f);
		this.m_lbItemOpenValue_Text.SetLocationY(num2 + 7f);
		this.m_ItemOpenValueSliderBG.SetLocationY(num2 + 19f);
		num2 = num2 + this.m_ItemOpenValue_Minus.GetSize().y + 14f;
		this.m_btClose.SetLocationY(num2 + 14f);
		this.m_btAllUse.SetLocationY(num2 + 14f);
		base.SetSize(base.GetSizeX(), num2 + 92f);
		this.m_txBG.SetSize(base.GetSize().x, num2 + 92f);
	}

	public void OnClickUse(IUIObject obj)
	{
		if (this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_RANDOMBOX)
		{
			GS_BOX_USE_REQ gS_BOX_USE_REQ = new GS_BOX_USE_REQ();
			gS_BOX_USE_REQ.m_nItemID = this.m_MainBoxItem.m_nItemID;
			gS_BOX_USE_REQ.m_nItemUnique = this.m_MainBoxItem.m_nItemUnique;
			gS_BOX_USE_REQ.m_nPosType = this.m_MainBoxItem.m_nPosType;
			gS_BOX_USE_REQ.m_nItemPos = this.m_MainBoxItem.m_nItemPos;
			gS_BOX_USE_REQ.m_nArrayIndex = 0;
			gS_BOX_USE_REQ.m_byAllOpen = 1;
			gS_BOX_USE_REQ.m_nItemCount = this.m_nItemOpenCount;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BOX_USE_REQ, gS_BOX_USE_REQ);
		}
		else if (this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_GOLDBAR || this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_EXCHANGE || this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_BATTLESPEED)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			NkSoldierInfo userSoldierInfo = nrCharUser.GetUserSoldierInfo();
			long solID = userSoldierInfo.GetSolID();
			if (this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_BATTLESPEED && this.m_MaxCount <= 0f)
			{
				COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
				if (instance == null)
				{
					return;
				}
				int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_FASTBATTLE_MAXNUM);
				string empty = string.Empty;
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("801");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"count",
					value
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
			else
			{
				GS_ITEM_SUPPLY_USE_REQ gS_ITEM_SUPPLY_USE_REQ = new GS_ITEM_SUPPLY_USE_REQ();
				gS_ITEM_SUPPLY_USE_REQ.m_nItemUnique = this.m_MainBoxItem.m_nItemUnique;
				gS_ITEM_SUPPLY_USE_REQ.m_nDestSolID = solID;
				gS_ITEM_SUPPLY_USE_REQ.m_shItemNum = this.m_nItemOpenCount;
				gS_ITEM_SUPPLY_USE_REQ.m_byPosType = this.m_MainBoxItem.m_nPosType;
				gS_ITEM_SUPPLY_USE_REQ.m_shPosItem = this.m_MainBoxItem.m_nItemPos;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SUPPLY_USE_REQ, gS_ITEM_SUPPLY_USE_REQ);
				this.Close();
			}
		}
		else if (this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_TICKET)
		{
			if (!(obj.Data is ITEM))
			{
				return;
			}
			ITEM iTEM = obj.Data as ITEM;
			if (iTEM == null)
			{
				return;
			}
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null || readySolList.GetCount() >= 100)
			{
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
				Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
			if (NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique) == null)
			{
				return;
			}
			SolRecruitDlg solRecruitDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUIT_DLG) as SolRecruitDlg;
			if (solRecruitDlg != null)
			{
				solRecruitDlg.SetRecruitButtonEnable(false);
			}
			NrTSingleton<NkClientLogic>.Instance.SetCanOpenTicket(false);
			if (this.m_nItemOpenCount == 1)
			{
				Protocol_Item.Item_Use(iTEM);
				return;
			}
			GS_SOLDIER_RECRUIT_REQ gS_SOLDIER_RECRUIT_REQ = default(GS_SOLDIER_RECRUIT_REQ);
			gS_SOLDIER_RECRUIT_REQ.ItemUnique = iTEM.m_nItemUnique;
			gS_SOLDIER_RECRUIT_REQ.RecruitType = 20;
			gS_SOLDIER_RECRUIT_REQ.SubData = this.m_nItemOpenCount;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_RECRUIT_REQ, gS_SOLDIER_RECRUIT_REQ);
		}
	}

	public void OnClickClose(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_CONTINUE_DLG);
	}

	private int GetMaxItem()
	{
		switch (this.m_eShowType)
		{
		case ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_RANDOMBOX:
			if (this.m_MainBoxItem.m_nItemNum < 12)
			{
				return this.m_MainBoxItem.m_nItemNum;
			}
			return 12;
		case ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_GOLDBAR:
		case ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_EXCHANGE:
		case ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_BATTLESPEED:
			return this.m_MainBoxItem.m_nItemNum;
		case ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_TICKET:
		{
			int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CARD_MULTIPLE_OPEN);
			int result = value;
			if (this.m_MainBoxItem.m_nItemNum < value)
			{
				result = this.m_MainBoxItem.m_nItemNum;
			}
			return result;
		}
		default:
			return 1;
		}
	}

	public void BtClickInputOpenItem(IUIObject obj)
	{
		if (this.m_eShowType == ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_EXCHANGE)
		{
			return;
		}
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.On_InputData), null, new Action<InputNumberDlg, object>(this.On_Close_InputNumber), null);
		inputNumberDlg.SetMinMax(1L, (long)this.GetMaxItem());
		inputNumberDlg.SetNum((long)this.m_nItemOpenCount);
		inputNumberDlg.SetLocation(inputNumberDlg.GetLocationX(), inputNumberDlg.GetLocationY(), base.GetLocation().z - 2f);
	}

	private void On_InputData(InputNumberDlg a_cForm, object a_oObject)
	{
		int num = (int)a_cForm.GetNum();
		if (num > this.GetMaxItem())
		{
			num = this.GetMaxItem();
		}
		this.SetOpenItemNum(num);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	private void On_Close_InputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	private void OnValueMinus(IUIObject obj)
	{
		if (this.m_nItemOpenCount > 1)
		{
			this.m_nItemOpenCount--;
			this.SetOpenItemNum(this.m_nItemOpenCount);
		}
	}

	private void OnValueAdd(IUIObject obj)
	{
		if (this.m_nItemOpenCount < (int)this.m_MaxCount)
		{
			this.m_nItemOpenCount++;
			this.SetOpenItemNum(this.m_nItemOpenCount);
		}
	}

	private void SetOpenItemNum(int num)
	{
		this.m_nItemOpenCount = num;
		this.m_lbItemOpenValue_Text.SetText((this.m_nItemOpenCount * this.m_Multiply).ToString());
		float num2 = 0f;
		if (this.m_MaxCount != 0f)
		{
			num2 = (float)this.m_nItemOpenCount / 100f * (100f / this.m_MaxCount);
		}
		this.m_ItemOpenValueSlider.defaultValue = num2;
		this.m_ItemOpenValueSlider.Value = num2;
		this.m_oldItemOpenCount = num2;
	}

	public override void Update()
	{
		base.Update();
		if (this.m_oldItemOpenCount != this.m_ItemOpenValueSlider.Value)
		{
			int num = 0;
			if (this.m_MaxCount != 0f)
			{
				num = (int)(this.m_ItemOpenValueSlider.Value * 100f / (100f / this.m_MaxCount));
			}
			this.m_nItemOpenCount = num + 1;
			if (this.m_nItemOpenCount >= (int)this.m_MaxCount)
			{
				this.m_nItemOpenCount = (int)this.m_MaxCount;
			}
			if (this.m_MaxCount <= 0f)
			{
				this.m_nItemOpenCount = 0;
			}
			this.m_lbItemOpenValue_Text.SetText((this.m_nItemOpenCount * this.m_Multiply).ToString());
			this.m_oldItemOpenCount = this.m_ItemOpenValueSlider.Value;
		}
	}

	private void SetItemRandomBox()
	{
		this.m_MaxCount = 12f;
		if (this.m_MainBoxItem.m_nItemNum < 12)
		{
			this.m_MaxCount = (float)this.m_MainBoxItem.m_nItemNum;
		}
		if (!this.FirstOpen)
		{
			this.m_nItemOpenCount = (int)this.m_MaxCount;
			this.SetOpenItemNum(this.m_nItemOpenCount);
			this.FirstOpen = true;
		}
		else if (this.m_MainBoxItem.m_nItemNum < 12 && this.m_nItemOpenCount > this.m_MainBoxItem.m_nItemNum)
		{
			this.m_nItemOpenCount = (int)this.m_MaxCount;
			this.SetOpenItemNum(this.m_nItemOpenCount);
		}
	}

	private void SetItemGoldBar()
	{
		this.m_MaxCount = (float)this.m_MainBoxItem.m_nItemNum;
		this.m_nItemOpenCount = (int)this.m_MaxCount;
		this.SetOpenItemNum(this.m_nItemOpenCount);
	}

	private void SetItemExchage()
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_MainBoxItem.m_nItemUnique);
		if (itemInfo != null)
		{
			if (itemInfo.m_nParam[1] == 0)
			{
				this.m_nItemOpenCount = 0;
				return;
			}
			this.m_MaxCount = (float)(this.m_MainBoxItem.m_nItemNum / itemInfo.m_nParam[1]);
			this.m_nItemOpenCount = (int)this.m_MaxCount;
			this.m_Multiply = itemInfo.m_nParam[1];
			this.SetOpenItemNum(this.m_nItemOpenCount);
		}
	}

	private void SetItemTicket()
	{
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CARD_MULTIPLE_OPEN);
		this.m_MaxCount = (float)value;
		if (this.m_MainBoxItem.m_nItemNum < value)
		{
			this.m_MaxCount = (float)this.m_MainBoxItem.m_nItemNum;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		int num = 100 - readySolList.GetCount();
		if (num <= 0)
		{
			this.Close();
		}
		if ((float)num < this.m_MaxCount)
		{
			this.m_MaxCount = (float)num;
		}
		this.m_nItemOpenCount = (int)this.m_MaxCount;
		this.SetOpenItemNum(this.m_nItemOpenCount);
		this.m_btAllUse.data = this.m_MainBoxItem;
	}

	private void SetItemBattleSpeed()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_BATTLESPEED_COUNT);
			int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_FASTBATTLE_MAXNUM);
			int num = NkUserInventory.GetInstance().Get_First_ItemCnt(this.m_MainBoxItem.m_nItemUnique);
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_MainBoxItem.m_nItemUnique);
			if (itemInfo == null)
			{
				return;
			}
			long num2 = (long)value - charSubData;
			this.m_MaxCount = (float)(num2 / (long)itemInfo.m_nParam[0]);
			if (this.m_MaxCount > (float)num)
			{
				this.m_MaxCount = (float)num;
			}
			this.m_nItemOpenCount = (int)this.m_MaxCount;
			this.SetOpenItemNum(this.m_nItemOpenCount);
		}
	}
}
