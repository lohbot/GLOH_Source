using GAME;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using System.Text;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Item_Box_Select_Dlg : Form
{
	private const int DEFAULT_CURITEMNUM = 1;

	private Label m_lbTitle;

	private Label m_lbCount;

	private Label m_lbNotice;

	private Button m_btnClose;

	private Button m_btnBuy;

	private Button m_btnCountMinus;

	private Button m_btnCountPlus;

	private Button m_btnCountNum;

	private HorizontalSlider m_hsCount;

	private NewListBox m_nlbEquipList;

	private NewListBox m_nlbEquipInfo;

	private ITEM m_cItem;

	private bool m_bBoxCollider = true;

	private int m_nCurItemNum;

	private int m_nMaxItemNum;

	private float m_fSliderValue;

	private Item_Box_Select_Dlg_PreProcess _preProcess;

	private ItemSetTooltip_Dlg m_pSetItemTooltipDlg;

	private GameObject m_gbEffect_Set;

	public override void InitializeComponent()
	{
		this._preProcess = new Item_Box_Select_Dlg_PreProcess();
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item_Box/DLG_Itembox_Select_renual", G_ID.ITEM_BOX_SELECT_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
		base.AlwaysUpdate = true;
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("LB_Title") as Label);
		this.m_lbCount = (base.GetControl("LB_NumCount") as Label);
		this.m_lbNotice = (base.GetControl("LB_Notice") as Label);
		this.m_btnClose = (base.GetControl("Button_Exit") as Button);
		this.m_btnClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this.m_btnBuy = (base.GetControl("Button_Sel") as Button);
		this.m_btnBuy.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Buy));
		this.m_btnCountMinus = (base.GetControl("Btn_Minus") as Button);
		this.m_btnCountMinus.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Minus));
		this.m_btnCountPlus = (base.GetControl("Btn_Plus") as Button);
		this.m_btnCountPlus.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Plus));
		this.m_btnCountNum = (base.GetControl("Btn_NumCount") as Button);
		this.m_btnCountNum.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Number));
		this.m_hsCount = (base.GetControl("HSlider_gage") as HorizontalSlider);
		this.m_nlbEquipInfo = (base.GetControl("NLB_EquipmentInfo") as NewListBox);
		this.m_nlbEquipList = (base.GetControl("NLB_EquipmentList") as NewListBox);
		this.m_nlbEquipList.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Item));
		base.SetScreenCenter();
		this.Set_Init();
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
		if (this.m_fSliderValue != this.m_hsCount.Value)
		{
			int num = (int)(this.m_hsCount.Value * 100f / (100f / (float)this.m_nMaxItemNum));
			this.m_nCurItemNum = num + 1;
			if (this.m_nCurItemNum >= this.m_nMaxItemNum)
			{
				this.m_nCurItemNum = this.m_nMaxItemNum;
			}
			if (this.m_nMaxItemNum <= 1)
			{
				this.m_nCurItemNum = 1;
			}
			this.m_lbCount.SetText(this.m_nCurItemNum.ToString());
		}
	}

	public override void OnClose()
	{
		if (this.m_pSetItemTooltipDlg != null)
		{
			this.m_pSetItemTooltipDlg.Close();
		}
		if (this.m_gbEffect_Set != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbEffect_Set);
		}
	}

	private void Set_Init()
	{
		this.m_nCurItemNum = 0;
		this.m_nMaxItemNum = 0;
		this.m_gbEffect_Set = null;
	}

	public void Set_Item(ITEM a_cItem)
	{
		if (a_cItem == null)
		{
			return;
		}
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		NkSoldierInfo nkSoldierInfo = null;
		if (nrCharUser != null)
		{
			nkSoldierInfo = nrCharUser.GetPersonInfo().GetLeaderSoldierInfo();
		}
		this.m_cItem = a_cItem;
		this.m_nCurItemNum = 1;
		this.m_nMaxItemNum = 12;
		if (a_cItem.m_nItemNum < 12)
		{
			this.m_nMaxItemNum = a_cItem.m_nItemNum;
		}
		this.Set_GetItemNum();
		this.m_lbNotice.Visible = true;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_cItem),
			"count",
			this.m_nMaxItemNum
		});
		this.m_lbTitle.Text = empty;
		this.m_nlbEquipList.Clear();
		this.m_nlbEquipList.SelectStyle = "Win_B_ListBoxOrange";
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
								NewListItem newListItem = new NewListItem(this.m_nlbEquipList.ColumnNum, true, string.Empty);
								if (num4 == 0)
								{
									UIBaseInfoLoader itemTexture = NrTSingleton<ItemManager>.Instance.GetItemTexture(num2);
									newListItem.SetListItemData(1, itemTexture, NrTSingleton<ItemManager>.Instance.GetBoxItemTemp(this.m_cItem.m_nItemUnique, i), null, null);
									TsLog.LogError("0 == itemRank", new object[0]);
								}
								else
								{
									ITEM iTEM = new ITEM();
									if (iTEM_BOX_GROUP != null)
									{
										iTEM.m_nItemID = -9223372036854775808L;
										iTEM.m_nItemUnique = iTEM_BOX_GROUP.i32GroupItemUnique[i];
										iTEM.m_nItemNum = iTEM_BOX_GROUP.i32GroupItemNum[i];
										iTEM.m_nOption[0] = (int)NrTSingleton<Item_Makerank_Manager>.Instance.GetItemAblility((byte)iTEM_BOX_GROUP.i32GroupItemGrade[i]);
										iTEM.m_nOption[1] = (int)NrTSingleton<Item_Makerank_Manager>.Instance.GetItemAblility((byte)iTEM_BOX_GROUP.i32GroupItemGrade[i]);
										iTEM.m_nOption[2] = iTEM_BOX_GROUP.i32GroupItemGrade[i];
										iTEM.m_nOption[3] = 1;
										iTEM.m_nOption[4] = iTEM_BOX_GROUP.i32GroupItemSkillUnique[i];
										iTEM.m_nOption[5] = iTEM_BOX_GROUP.i32GroupItemSkillLevel[i];
										iTEM.m_nOption[7] = iTEM_BOX_GROUP.i32GroupItemTradePoint[i];
										iTEM.m_nOption[8] = iTEM_BOX_GROUP.i32GroupItemReducePoint[i];
										iTEM.m_nOption[6] = iTEM_BOX_GROUP.i32GroupItemSkill2Unique[i];
										iTEM.m_nOption[9] = iTEM_BOX_GROUP.i32GroupItemSkill2Level[i];
										iTEM.m_nDurability = 100;
										newListItem.SetListItemData(1, iTEM, null, null, null);
									}
									else
									{
										iTEM.Set(this.m_cItem);
										iTEM.m_nItemUnique = num2;
										iTEM.m_nOption[2] = num4;
										newListItem.SetListItemData(1, iTEM, null, null, null);
									}
								}
								string text = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1101"), NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(num2));
								newListItem.SetListItemData(2, text, null, null, null);
								string arg = Protocol_Item.Money_Format((long)num3) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442");
								string text2 = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1105"), arg);
								newListItem.SetListItemData(3, text2, null, null, null);
								newListItem.Data = i;
								this.m_nlbEquipList.Add(newListItem);
							}
						}
					}
				}
				else
				{
					num++;
					NewListItem newListItem2 = new NewListItem(this.m_nlbEquipList.ColumnNum, true, string.Empty);
					newListItem2.SetListItemData(0, string.Empty, "Win_T_ItemEmpty", null, null);
					if (num4 == 0)
					{
						UIBaseInfoLoader itemTexture2 = NrTSingleton<ItemManager>.Instance.GetItemTexture(num2);
						newListItem2.SetListItemData(1, itemTexture2, NrTSingleton<ItemManager>.Instance.GetBoxItemTemp(this.m_cItem.m_nItemUnique, i), null, null);
					}
					else
					{
						ITEM iTEM2 = new ITEM();
						if (iTEM_BOX_GROUP != null)
						{
							iTEM2.m_nItemID = -9223372036854775808L;
							iTEM2.m_nItemUnique = iTEM_BOX_GROUP.i32GroupItemUnique[i];
							iTEM2.m_nItemNum = iTEM_BOX_GROUP.i32GroupItemNum[i];
							iTEM2.m_nOption[0] = (int)NrTSingleton<Item_Makerank_Manager>.Instance.GetItemAblility((byte)iTEM_BOX_GROUP.i32GroupItemGrade[i]);
							iTEM2.m_nOption[1] = (int)NrTSingleton<Item_Makerank_Manager>.Instance.GetItemAblility((byte)iTEM_BOX_GROUP.i32GroupItemGrade[i]);
							iTEM2.m_nOption[2] = iTEM_BOX_GROUP.i32GroupItemGrade[i];
							iTEM2.m_nOption[3] = 1;
							iTEM2.m_nOption[4] = iTEM_BOX_GROUP.i32GroupItemSkillUnique[i];
							iTEM2.m_nOption[5] = iTEM_BOX_GROUP.i32GroupItemSkillLevel[i];
							iTEM2.m_nOption[7] = iTEM_BOX_GROUP.i32GroupItemTradePoint[i];
							iTEM2.m_nOption[8] = iTEM_BOX_GROUP.i32GroupItemReducePoint[i];
							iTEM2.m_nOption[6] = iTEM_BOX_GROUP.i32GroupItemSkill2Unique[i];
							iTEM2.m_nOption[9] = iTEM_BOX_GROUP.i32GroupItemSkill2Level[i];
							iTEM2.m_nDurability = 100;
							newListItem2.SetListItemData(1, iTEM2, null, null, null);
						}
						else
						{
							iTEM2.Set(this.m_cItem);
							iTEM2.m_nItemUnique = num2;
							iTEM2.m_nOption[2] = num4;
							newListItem2.SetListItemData(1, iTEM2, null, null, null);
						}
					}
					string text3 = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1101"), NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(num2));
					newListItem2.SetListItemData(2, text3, null, null, null);
					string arg2 = Protocol_Item.Money_Format((long)num3) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442");
					string text4 = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1105"), arg2);
					newListItem2.SetListItemData(3, text4, null, null, null);
					newListItem2.Data = i;
					this.m_nlbEquipList.Add(newListItem2);
				}
			}
		}
		this.m_nlbEquipList.RepositionItems();
		this.Show();
	}

	public void Set_ItemInfo(ITEM _cItem)
	{
		if (_cItem == null || !_cItem.IsValid())
		{
			return;
		}
		this.m_nlbEquipInfo.Clear();
		ItemOption_Text[] array = ItemTooltipDlg.Get_Item_Info(_cItem, null, false, true, G_ID.NONE);
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(_cItem.m_nItemUnique);
		if (itemInfo == null)
		{
			return;
		}
		NewListItem newListItem = new NewListItem(this.m_nlbEquipInfo.ColumnNum, true, string.Empty);
		int rank = _cItem.m_nOption[2];
		int num = 0;
		bool flag = false;
		string text = string.Empty;
		if (_cItem.m_nPosType == 10 || _cItem.m_nPosType == 1 || _cItem.m_nPosType == 2 || _cItem.m_nPosType == 3 || _cItem.m_nPosType == 4)
		{
			text = string.Format("{0}{1} {2}", ItemManager.RankTextColor(rank), ItemManager.RankText(rank), NrTSingleton<ItemManager>.Instance.GetItemTypeName((eITEM_TYPE)itemInfo.m_nItemType));
		}
		else
		{
			text = string.Format("{0}", NrTSingleton<ItemManager>.Instance.GetItemTypeName((eITEM_TYPE)itemInfo.m_nItemType));
		}
		newListItem.SetListItemData(2, text, null, null, null);
		if (array.Length > 0)
		{
			if (array[0].m_MainOption)
			{
				newListItem.SetListItemData(4, array[0].m_OptionName, null, null, null);
				newListItem.SetListItemData(6, array[0].m_OptionValue, null, null, null);
				num++;
			}
			else
			{
				newListItem.SetListItemData(4, string.Empty, null, null, null);
				newListItem.SetListItemData(6, string.Empty, null, null, null);
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
			newListItem.SetListItemData(5, stringBuilder.ToString(), null, null, null);
			newListItem.SetListItemData(7, stringBuilder2.ToString(), null, null, null);
		}
		else
		{
			newListItem.SetListItemData(4, string.Empty, null, null, null);
			newListItem.SetListItemData(5, string.Empty, null, null, null);
			newListItem.SetListItemData(6, string.Empty, null, null, null);
			newListItem.SetListItemData(7, string.Empty, null, null, null);
		}
		string text2 = NrTSingleton<ItemManager>.Instance.GetName(_cItem);
		if (_cItem.m_nDurability == 0 && (_cItem.m_nPosType == 1 || _cItem.m_nPosType == 2 || _cItem.m_nPosType == 3 || _cItem.m_nPosType == 4))
		{
			text2 = string.Format("{0} {1}", text2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2984"));
		}
		newListItem.SetListItemData(11, text2, null, null, null);
		StringBuilder stringBuilder3 = new StringBuilder();
		string value = "\r\n";
		if (itemInfo.m_strToolTipTextKey != "0")
		{
			stringBuilder3.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper(itemInfo.m_strToolTipTextKey));
			stringBuilder3.Append(value);
		}
		int num2 = _cItem.m_nOption[4];
		int num3 = _cItem.m_nOption[5];
		int num4 = _cItem.m_nOption[6];
		int num5 = _cItem.m_nOption[9];
		if (num2 > 0 && num3 > 0)
		{
			string value2 = string.Empty;
			string empty = string.Empty;
			BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num2, num3);
			if (battleSkillDetail != null)
			{
				if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
				{
					value2 = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2381"));
				}
				else
				{
					value2 = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2183"));
				}
				stringBuilder3.Append(value2);
				stringBuilder3.Append(value);
				NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail.m_nSkillTooltip), battleSkillDetail, null, -1);
				stringBuilder3.Append(empty);
				stringBuilder3.Append(value);
				flag = true;
			}
			string empty2 = string.Empty;
			if (num4 > 0 && num5 > 0 && flag)
			{
				BATTLESKILL_DETAIL battleSkillDetail2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num4, num5);
				if (battleSkillDetail2 != null)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref empty2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail2.m_nSkillTooltip), battleSkillDetail2, null, -1);
				}
				stringBuilder3.Append(empty2);
				stringBuilder3.Append(value);
			}
		}
		newListItem.SetListItemData(14, stringBuilder3.ToString(), null, null, null);
		newListItem.SetListItemData(8, NrTSingleton<ItemManager>.Instance.GetItemTexture(_cItem.m_nItemUnique), null, null, null);
		if (itemInfo.m_nSetUnique != 0)
		{
			newListItem.SetListItemData(12, string.Empty, _cItem, new EZValueChangedDelegate(this.Click_SetItem), null);
		}
		else
		{
			newListItem.SetListItemData(12, false);
		}
		newListItem.Data = _cItem;
		this.m_nlbEquipInfo.Add(newListItem);
		this.m_nlbEquipInfo.RepositionItems();
		if (itemInfo.m_nSetUnique != 0)
		{
			this.Load_SetEffect();
		}
		this.m_lbNotice.Visible = false;
	}

	public void Set_GetItemNum()
	{
		float value = 0f;
		if (this.m_nMaxItemNum > 0)
		{
			value = (float)this.m_nCurItemNum / (float)this.m_nMaxItemNum;
		}
		this.m_lbCount.SetText(this.m_nCurItemNum.ToString());
		this.m_hsCount.Value = value;
		this.m_fSliderValue = this.m_hsCount.Value;
	}

	private void Click_Buy(IUIObject _obj)
	{
		if (this.m_cItem == null)
		{
			return;
		}
		if (this.m_nCurItemNum == 0)
		{
			return;
		}
		if (null == this.m_nlbEquipList.SelectedItem)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("226");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		int num = (int)this.m_nlbEquipList.SelectedItem.Data;
		if (num == -1 || num < 0 || num >= 12)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("226");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (!Protocol_Item.CanAddItem(this.m_cItem.m_nItemUnique, this.m_nCurItemNum))
		{
			string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46");
			Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (this._preProcess.PreProcess(this.m_cItem, num, new YesDelegate(this.Request_OpenBox)))
		{
			return;
		}
		this.Request_OpenBox(num);
	}

	public void Request_OpenBox(object param)
	{
		if (this.m_cItem == null)
		{
			Debug.LogError("ERROR, Item_Box_Select_Dlg.cs, BoxOpen(), m_cItem Is Null");
			return;
		}
		int nArrayIndex = (int)param;
		GS_BOX_USE_REQ gS_BOX_USE_REQ = new GS_BOX_USE_REQ();
		gS_BOX_USE_REQ.m_nItemID = this.m_cItem.m_nItemID;
		gS_BOX_USE_REQ.m_nItemUnique = this.m_cItem.m_nItemUnique;
		gS_BOX_USE_REQ.m_nPosType = this.m_cItem.m_nPosType;
		gS_BOX_USE_REQ.m_nItemPos = this.m_cItem.m_nItemPos;
		gS_BOX_USE_REQ.m_nArrayIndex = nArrayIndex;
		gS_BOX_USE_REQ.m_nItemCount = this.m_nCurItemNum;
		gS_BOX_USE_REQ.m_byAllOpen = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BOX_USE_REQ, gS_BOX_USE_REQ);
		base.CloseNow();
	}

	private void Click_Minus(IUIObject _obj)
	{
		if (this.m_nCurItemNum <= 1)
		{
			return;
		}
		this.m_nCurItemNum--;
		this.Set_GetItemNum();
	}

	private void Click_Plus(IUIObject _obj)
	{
		if (this.m_nCurItemNum >= this.m_nMaxItemNum)
		{
			return;
		}
		this.m_nCurItemNum++;
		this.Set_GetItemNum();
	}

	private void Click_Number(IUIObject _obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.On_InputData), null, new Action<InputNumberDlg, object>(this.OnClose_InputNumber), null);
		inputNumberDlg.SetMinMax(1L, (long)this.m_nMaxItemNum);
		inputNumberDlg.SetNum((long)this.m_nCurItemNum);
		inputNumberDlg.SetLocation(inputNumberDlg.GetLocationX(), inputNumberDlg.GetLocationY(), base.GetLocation().z - 2f);
	}

	private void On_InputData(InputNumberDlg a_cForm, object a_oObject)
	{
		int num = (int)a_cForm.GetNum();
		if (num > this.m_nMaxItemNum)
		{
			num = this.m_nMaxItemNum;
		}
		this.m_nCurItemNum = num;
		this.Set_GetItemNum();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	private void OnClose_InputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	private void Click_Item(IUIObject _obj)
	{
		if (null == this.m_nlbEquipList.SelectedItem)
		{
			return;
		}
		int num = (int)this.m_nlbEquipList.SelectedItem.Data;
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
		this.Set_ItemInfo(iTEM);
	}

	private void Click_SetItem(IUIObject _obj)
	{
		if (_obj == null)
		{
			return;
		}
		ITEM iTEM = _obj.Data as ITEM;
		if (iTEM == null)
		{
			return;
		}
		if (this.m_pSetItemTooltipDlg == null)
		{
			this.m_pSetItemTooltipDlg = (NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.SETITEMTOOLTIP_DLG) as ItemSetTooltip_Dlg);
		}
		if (this.m_pSetItemTooltipDlg != null)
		{
			this.m_pSetItemTooltipDlg.SetData(iTEM, null, base.WindowID);
			this.m_pSetItemTooltipDlg.SetLocation(base.GetLocationX() + this.m_nlbEquipList.GetSize().x + this.m_pSetItemTooltipDlg.GetSizeX() * 0.3f, base.GetLocationY() + this.m_lbTitle.GetSize().y, base.GetLocation().z - 2f);
		}
	}

	public void CloseSetItemTooltip()
	{
		if (this.m_pSetItemTooltipDlg != null)
		{
			this.m_pSetItemTooltipDlg.Close();
			this.m_pSetItemTooltipDlg = null;
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

	public void Load_SetEffect()
	{
		UIListItemContainer item = this.m_nlbEquipInfo.GetItem(0);
		if (item != null)
		{
			AutoSpriteControlBase element = item.GetElement(12);
			if (element != null)
			{
				string str = string.Format("{0}{1}", "Effect/Instant/fx_setbuuton_ui", NrTSingleton<UIDataManager>.Instance.AddFilePath);
				WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
				wWWItem.SetItemType(ItemType.USER_ASSETB);
				wWWItem.SetCallback(new PostProcPerItem(this.Effect_Set), element);
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			}
		}
	}

	private void Effect_Set(WWWItem _item, object _param)
	{
		AutoSpriteControlBase autoSpriteControlBase = _param as AutoSpriteControlBase;
		if (null != _item.GetSafeBundle() && autoSpriteControlBase != null && autoSpriteControlBase.gameObject != null && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbEffect_Set = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_gbEffect_Set);
					return;
				}
				Vector2 size = autoSpriteControlBase.GetSize();
				this.m_gbEffect_Set.transform.parent = autoSpriteControlBase.gameObject.transform;
				this.m_gbEffect_Set.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, autoSpriteControlBase.gameObject.transform.localPosition.z + 1.05f);
				NkUtil.SetAllChildLayer(this.m_gbEffect_Set, GUICamera.UILayer);
				this.m_gbEffect_Set.SetActive(true);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbEffect_Set);
				}
			}
		}
	}
}
