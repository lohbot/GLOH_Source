using GAME;
using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityForms;

public class ItemTooltipDlg : Form
{
	private const float _LINE_UP_MARGIN = 8f;

	private const float _LINE_DOWN_MARGIN = 14f;

	private const float _LINE_END_MARGIN = 92f;

	private Label m_lbTitle;

	private Label m_lbClass;

	private Label m_lbType;

	private Label m_lbMainOption;

	private FlashLabel m_lbSubOption;

	private FlashLabel m_flMainValue;

	private FlashLabel m_flSubValue;

	private FlashLabel m_lbText;

	private FlashLabel m_lbItemSkillName;

	private FlashLabel m_lbItemSkillText;

	private FlashLabel m_lbItemSkillText2;

	private ItemTexture m_itItemTex;

	private DrawTexture m_txClass;

	private DrawTexture m_txBG;

	private DrawTexture m_txLine;

	private Button m_btnSell;

	private Button m_btnEquip;

	private Button m_btnUpgrade;

	private bool bText;

	private bool bItemSkillText;

	private bool bItemSkillText2;

	private G_ID m_eParentWindowID;

	private ITEM m_cItem = new ITEM();

	private long m_SolID;

	private bool m_bRectUpdate = true;

	public static Vector3 MOBILE_TOOLTIP_POS = new Vector3(0f, 0f, 0f);

	private NkSoldierInfo m_SolInfo;

	public ITEM Item
	{
		get
		{
			return this.m_cItem;
		}
		set
		{
			this.m_cItem = value;
		}
	}

	public long SolID
	{
		get
		{
			return this.m_SolID;
		}
		set
		{
			this.m_SolID = value;
		}
	}

	public bool RectUpdate
	{
		get
		{
			return this.m_bRectUpdate;
		}
		set
		{
			this.m_bRectUpdate = value;
		}
	}

	public NkSoldierInfo SolInfo
	{
		get
		{
			return this.m_SolInfo;
		}
		set
		{
			this.m_SolInfo = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = false;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/itemtooltip_dlg", G_ID.ITEMTOOLTIP_DLG, true);
		form.Draggable = false;
		if (null != base.InteractivePanel)
		{
			base.SetLocation(base.GetLocation().x, base.GetLocation().y, 92f);
		}
		base.AlwaysUpdate = true;
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("Label_title") as Label);
		this.m_lbClass = (base.GetControl("Label_name") as Label);
		this.m_lbType = (base.GetControl("Label_kind") as Label);
		this.m_lbMainOption = (base.GetControl("Label_01") as Label);
		this.m_lbSubOption = (base.GetControl("Label_02") as FlashLabel);
		this.m_lbText = (base.GetControl("Label_03") as FlashLabel);
		this.m_lbItemSkillName = (base.GetControl("Label_04") as FlashLabel);
		this.m_lbItemSkillText = (base.GetControl("Label_05") as FlashLabel);
		this.m_lbItemSkillText2 = (base.GetControl("Label_07") as FlashLabel);
		this.m_flMainValue = (base.GetControl("FlashLabel_01") as FlashLabel);
		this.m_flSubValue = (base.GetControl("FlashLabel_02") as FlashLabel);
		this.m_itItemTex = (base.GetControl("ItemTexture_Item") as ItemTexture);
		this.m_txClass = (base.GetControl("DrawTexture_class") as DrawTexture);
		this.m_txBG = (base.GetControl("DrawTexture_BG") as DrawTexture);
		this.m_btnEquip = (base.GetControl("Button_Button02") as Button);
		this.m_btnUpgrade = (base.GetControl("Button_Button03") as Button);
		this.m_btnSell = (base.GetControl("Button_Button01") as Button);
		this.m_txLine = (base.GetControl("DrawTexture_MainBorder") as DrawTexture);
	}

	public void SetRectUpdate(bool bUpdate)
	{
	}

	public void Set_TooltipForEquip(G_ID eWidowID, ITEM pkEquipedItem, ITEM pkItem, bool bEquiped)
	{
		this.Set_Tooltip(eWidowID, pkEquipedItem, pkItem, Vector3.zero, bEquiped);
	}

	public void Set_Tooltip(G_ID a_eWidowID, ITEM pkItem, ITEM pkEquipedItem, Vector3 showPosition, bool bEquiped = false)
	{
		this.m_eParentWindowID = a_eWidowID;
		this.m_cItem = pkItem;
		this.Item_Tooltip(this, this.m_cItem, pkEquipedItem, this.m_eParentWindowID, bEquiped);
		this.Tooltip_Rect(this, showPosition);
		this.Show();
	}

	public void Set_Tooltip(G_ID eWidowID, ITEM pkItem, ITEM pkEquipedItem = null, bool bEquiped = false)
	{
		this.Set_Tooltip(eWidowID, pkItem, pkEquipedItem, Vector3.zero, bEquiped);
	}

	public void Set_Tooltip(G_ID a_eWidowID, ITEM pkItem, bool bEquiped, Vector3 showPosition)
	{
		this.Set_Tooltip(a_eWidowID, pkItem, null, showPosition, bEquiped);
	}

	public void Set_Tooltip(ITEM pkItem)
	{
		this.m_cItem = pkItem;
		this.Item_Tooltip(this, this.m_cItem, null, G_ID.NONE, false);
		this.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	public void Set_Tooltip(G_ID eWidowID, int itemunique)
	{
		this.m_eParentWindowID = eWidowID;
		this.m_cItem.Init();
		this.m_cItem.m_nItemUnique = itemunique;
		this.Item_Tooltip(this, this.m_cItem, null, eWidowID, false);
		this.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	public void Item_Tooltip(Form cThis, ITEM pkItem, ITEM pkEquipItem, G_ID eWidowID, bool bEquiped = false)
	{
		if (pkItem == null || !pkItem.IsValid())
		{
			return;
		}
		ItemOption_Text[] array = ItemTooltipDlg.Get_Item_Info(pkItem, pkEquipItem, bEquiped, true);
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
		int rank = pkItem.m_nOption[2];
		int num = 0;
		string name = NrTSingleton<ItemManager>.Instance.GetName(pkItem);
		this.m_txClass.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString(rank));
		this.m_lbTitle.Text = string.Format("{0} {1}", name, (!bEquiped) ? string.Empty : string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("2002"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1479")));
		if (pkItem.m_nItemNum > 0)
		{
			this.m_itItemTex.SetItemTexture(pkItem);
		}
		else
		{
			this.m_itItemTex.SetItemTexture(pkItem, false);
		}
		if (pkItem.m_nPosType == 10 || pkItem.m_nPosType == 1 || pkItem.m_nPosType == 2 || pkItem.m_nPosType == 3 || pkItem.m_nPosType == 4)
		{
			this.m_lbClass.Text = string.Format("{0}{1} {2}", ItemManager.RankTextColor(rank), ItemManager.RankText(rank), NrTSingleton<ItemManager>.Instance.GetItemTypeName((eITEM_TYPE)itemInfo.m_nItemType));
		}
		else
		{
			this.m_lbClass.Text = string.Format("{0}", NrTSingleton<ItemManager>.Instance.GetItemTypeName((eITEM_TYPE)itemInfo.m_nItemType));
		}
		this.m_lbType.Text = Protocol_Item.GetItemPartText(NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(pkItem.m_nItemUnique));
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
		}
		else
		{
			this.m_lbMainOption.Text = string.Empty;
			this.m_flMainValue.SetFlashLabel(string.Empty);
			this.m_lbSubOption.SetFlashLabel(string.Empty);
			this.m_flSubValue.SetFlashLabel(string.Empty);
		}
		this.m_lbText.SetLocation(this.m_lbText.GetLocation().x, this.m_lbSubOption.GetLocationY() + this.m_lbSubOption.Height + 10f);
		if (itemInfo.m_strToolTipTextKey != "0")
		{
			string textFromItemHelper = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper(itemInfo.m_strToolTipTextKey);
			this.m_lbText.SetFlashLabel(textFromItemHelper);
			this.bText = true;
		}
		else
		{
			this.m_lbText.SetFlashLabel(string.Empty);
			this.bText = false;
		}
		int num2 = pkItem.m_nOption[4];
		int num3 = pkItem.m_nOption[5];
		int num4 = pkItem.m_nOption[6];
		int num5 = pkItem.m_nOption[9];
		this.bItemSkillText = false;
		this.bItemSkillText2 = false;
		if (num2 > 0 && num3 > 0)
		{
			BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num2, num3);
			if (battleSkillDetail != null)
			{
				string flashLabel = string.Empty;
				if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
				{
					flashLabel = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2381"));
				}
				else
				{
					flashLabel = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2183"));
				}
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail.m_nSkillTooltip), battleSkillDetail, null);
				if (this.bText)
				{
					this.m_lbItemSkillName.SetLocation(this.m_lbItemSkillName.GetLocation().x, this.m_lbText.GetLocationY() + this.m_lbText.Height + 20f);
				}
				else
				{
					this.m_lbItemSkillName.SetLocation(this.m_lbItemSkillName.GetLocation().x, this.m_lbSubOption.GetLocationY() + this.m_lbSubOption.Height + 20f);
				}
				this.m_lbItemSkillName.SetFlashLabel(flashLabel);
				this.m_lbItemSkillText.SetLocation(this.m_lbItemSkillText.GetLocation().x, this.m_lbItemSkillName.GetLocationY() + this.m_lbItemSkillName.Height);
				this.m_lbItemSkillText.SetFlashLabel(empty);
				this.bItemSkillText = true;
			}
		}
		else
		{
			this.m_lbItemSkillName.SetFlashLabel(string.Empty);
			this.m_lbItemSkillText.SetFlashLabel(string.Empty);
		}
		if (num4 > 0 && num5 > 0 && this.bItemSkillText)
		{
			BATTLESKILL_DETAIL battleSkillDetail2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num4, num5);
			if (battleSkillDetail2 != null)
			{
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref empty2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail2.m_nSkillTooltip), battleSkillDetail2, null);
				this.m_lbItemSkillText2.SetLocation(this.m_lbItemSkillText.GetLocation().x, this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height);
				this.m_lbItemSkillText2.SetFlashLabel(empty2);
				this.bItemSkillText2 = true;
			}
		}
		else
		{
			this.m_lbItemSkillText2.SetFlashLabel(string.Empty);
		}
		float height = 0f;
		this.ButtonSetting(ref height, pkItem);
		base.SetSize(base.GetSizeX(), height);
		this.m_txBG.SetSize(base.GetSize().x, height);
	}

	private static void Tooltip_Base(Form cForm, ItemOption_Text[] _Option)
	{
	}

	public static ItemOption_Text[] Get_Item_Info(ITEM pkItem, ITEM pkEquipedItem, bool bEquiped, bool bShowItemNum = true)
	{
		List<ItemOption_Text> list = new List<ItemOption_Text>();
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
		ITEMINFO iTEMINFO = null;
		if (pkEquipedItem != null)
		{
			iTEMINFO = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkEquipedItem.m_nItemUnique);
		}
		float weight = Protocol_Item.Get_Weight(pkItem.m_nItemUnique, 0);
		float weight2 = 0f;
		if (pkEquipedItem != null)
		{
			weight2 = Protocol_Item.Get_Weight(pkEquipedItem.m_nItemUnique, 0);
		}
		if (bShowItemNum && !Protocol_Item.Is_EquipItem(pkItem.m_nItemUnique) && pkItem.m_nPosType != 200 && pkItem.m_nItemNum != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2005"),
				m_OptionValue = pkItem.m_nItemNum.ToString(),
				m_MainOption = true
			});
		}
		if (itemInfo.m_nMinDamage != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1263"),
				m_OptionValue = ItemTooltipDlg.GetStat(itemInfo.m_nMinDamage, weight, pkItem.m_nOption[0], bEquiped, (iTEMINFO == null) ? 0 : iTEMINFO.m_nMinDamage, weight2, (iTEMINFO == null) ? 0 : pkEquipedItem.m_nOption[0], itemInfo.m_nMaxDamage, (iTEMINFO == null) ? 0 : iTEMINFO.m_nMaxDamage),
				m_MainOption = true
			});
		}
		if (itemInfo.m_nDefense != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1264"),
				m_OptionValue = ItemTooltipDlg.GetStat(itemInfo.m_nDefense, weight, pkItem.m_nOption[0], bEquiped, (iTEMINFO == null) ? 0 : iTEMINFO.m_nDefense, weight2, (pkEquipedItem == null) ? 0 : pkEquipedItem.m_nOption[0], 0, 0),
				m_MainOption = true
			});
		}
		if (itemInfo.m_nDEX != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1272"),
				m_OptionValue = ItemTooltipDlg.GetStat((int)itemInfo.m_nDEX, weight, pkItem.m_nOption[1], bEquiped, (int)((pkEquipedItem == null) ? 0 : iTEMINFO.m_nDEX), weight2, (pkEquipedItem == null) ? 0 : pkEquipedItem.m_nOption[1], 0, 0)
			});
		}
		if (itemInfo.m_nCriticalPlus != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1267"),
				m_OptionValue = ItemTooltipDlg.GetStat(itemInfo.m_nCriticalPlus, weight, pkItem.m_nOption[1], bEquiped, (iTEMINFO == null) ? 0 : iTEMINFO.m_nCriticalPlus, weight2, (pkEquipedItem == null) ? 0 : pkEquipedItem.m_nOption[1], 0, 0)
			});
		}
		if (itemInfo.m_nAttackSpeed != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("256"),
				m_OptionValue = ItemTooltipDlg.GetStat(itemInfo.m_nAttackSpeed, weight, pkItem.m_nOption[1], bEquiped, (iTEMINFO == null) ? 0 : iTEMINFO.m_nAttackSpeed, weight2, (pkEquipedItem == null) ? 0 : pkEquipedItem.m_nOption[1], 0, 0)
			});
		}
		if (itemInfo.m_nAddHP != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1265"),
				m_OptionValue = ItemTooltipDlg.GetStat(itemInfo.m_nAddHP, weight, pkItem.m_nOption[1], bEquiped, (iTEMINFO == null) ? 0 : iTEMINFO.m_nAddHP, weight2, (pkEquipedItem == null) ? 0 : pkEquipedItem.m_nOption[1], 0, 0)
			});
		}
		if (itemInfo.m_nEvasionPlus != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1269"),
				m_OptionValue = ItemTooltipDlg.GetStat(itemInfo.m_nEvasionPlus, weight, pkItem.m_nOption[1], bEquiped, (iTEMINFO == null) ? 0 : iTEMINFO.m_nEvasionPlus, weight2, (pkEquipedItem == null) ? 0 : pkEquipedItem.m_nOption[1], 0, 0)
			});
		}
		if (itemInfo.m_nHitratePlus != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1268"),
				m_OptionValue = ItemTooltipDlg.GetStat(itemInfo.m_nHitratePlus, weight, pkItem.m_nOption[1], bEquiped, (iTEMINFO == null) ? 0 : iTEMINFO.m_nHitratePlus, weight2, (pkEquipedItem == null) ? 0 : pkEquipedItem.m_nOption[1], 0, 0)
			});
		}
		if (itemInfo.m_nINT != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1274"),
				m_OptionValue = ItemTooltipDlg.GetStat((int)itemInfo.m_nINT, weight, pkItem.m_nOption[1], bEquiped, (int)((pkEquipedItem == null) ? 0 : iTEMINFO.m_nINT), weight2, (pkEquipedItem == null) ? 0 : pkEquipedItem.m_nOption[1], 0, 0)
			});
		}
		if (itemInfo.m_nMagicDefense != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = "\n" + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1237"),
				m_OptionValue = "\n" + itemInfo.m_nMagicDefense
			});
		}
		int num = pkItem.m_nOption[4];
		int num2 = pkItem.m_nOption[5];
		string optionName = string.Empty;
		if (num > 0 && num2 > 0)
		{
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(num);
			if (battleSkillBase != null)
			{
				if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
				{
					optionName = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2379"));
				}
				else
				{
					optionName = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("993"));
				}
				list.Add(new ItemOption_Text
				{
					m_OptionName = optionName,
					m_OptionValue = string.Format("\n{0}{1} {2}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey), "Lv." + num2)
				});
			}
		}
		int num3 = pkItem.m_nOption[6];
		int num4 = pkItem.m_nOption[9];
		optionName = string.Empty;
		if (num3 > 0 && num4 > 0)
		{
			BATTLESKILL_BASE battleSkillBase2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(num3);
			if (battleSkillBase2 != null)
			{
				if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
				{
					optionName = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2380"));
				}
				else
				{
					optionName = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("993"));
				}
				list.Add(new ItemOption_Text
				{
					m_OptionName = optionName,
					m_OptionValue = string.Format("\n{0}{1} {2}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase2.m_strTextKey), "Lv." + num4)
				});
			}
		}
		if (itemInfo.m_nUseMinLevel != 0)
		{
			ItemOption_Text itemOption_Text = new ItemOption_Text();
			itemOption_Text.m_OptionName = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1101"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1258"));
			if (0 >= pkItem.m_nOption[8])
			{
				itemOption_Text.m_OptionValue = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1101"), itemInfo.m_nUseMinLevel);
			}
			else
			{
				itemOption_Text.m_OptionValue = string.Format("\n{0}{1}({2})", NrTSingleton<CTextParser>.Instance.GetTextColor("1105"), itemInfo.GetUseMinLevel(pkItem), -pkItem.m_nOption[8]);
			}
			list.Add(itemOption_Text);
		}
		if (itemInfo.m_nNeedOpenItemUnique != 0)
		{
			ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemInfo.m_nNeedOpenItemUnique);
			if (itemInfo2 != null)
			{
				ItemOption_Text itemOption_Text2 = new ItemOption_Text();
				itemOption_Text2.m_OptionName = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1107"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1900"));
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1416"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemInfo.m_nNeedOpenItemUnique),
					"count",
					itemInfo.m_nNeedOpenItemNum.ToString()
				});
				itemOption_Text2.m_OptionValue = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1107"), empty);
				list.Add(itemOption_Text2);
			}
		}
		int itemMakeRank = pkItem.m_nOption[2];
		ITEM_SELL itemSellData = NrTSingleton<ITEM_SELL_Manager>.Instance.GetItemSellData(itemInfo.m_nQualityLevel, itemMakeRank);
		if (itemSellData != null && itemSellData.nItemSellMoney != 0)
		{
			list.Add(new ItemOption_Text
			{
				m_OptionName = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1101"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("47")),
				m_OptionValue = string.Format("\n{0}{1} {2}", NrTSingleton<CTextParser>.Instance.GetTextColor("1107"), Protocol_Item.Money_Format((long)itemSellData.nItemSellMoney), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676"))
			});
		}
		if (pkItem.GetTradeCount() != 0)
		{
			ItemOption_Text itemOption_Text3 = new ItemOption_Text();
			itemOption_Text3.m_OptionName = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1101"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1309"));
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1143"),
				"count",
				pkItem.GetTradeCount()
			});
			itemOption_Text3.m_OptionValue = string.Format("\n{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1101"), empty2);
			list.Add(itemOption_Text3);
		}
		return list.ToArray();
	}

	public static string GetStat(int MainValue, float Weight, int OptionValue, bool bEquip = false, int MainValue2 = 0, float Weight2 = 0f, int OptinValue2 = 0, int MainValue3 = 0, int MainValue4 = 0)
	{
		string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1401");
		string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1304");
		string textColor3 = NrTSingleton<CTextParser>.Instance.GetTextColor("1011");
		string result = string.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		bool flag = false;
		int nValue = (int)((float)MainValue2 * Weight2);
		int optionValue = Tooltip_Dlg.GetOptionValue(OptinValue2, nValue);
		int nValue2 = (int)((float)MainValue * Weight);
		int optionValue2 = Tooltip_Dlg.GetOptionValue(OptionValue, nValue2);
		int num4 = optionValue2 - optionValue;
		if (MainValue3 != 0)
		{
			if (MainValue4 != 0)
			{
				int nValue3 = (int)((float)MainValue4 * Weight);
				num = Tooltip_Dlg.GetOptionValue(OptinValue2, nValue3);
			}
			int nValue4 = (int)((float)MainValue3 * Weight);
			num2 = Tooltip_Dlg.GetOptionValue(OptionValue, nValue4);
			num3 = num2 - num;
		}
		if (MainValue3 != 0)
		{
			if (num4 != 0 || num3 != 0)
			{
				flag = true;
			}
			if (!bEquip && flag && MainValue4 != 0)
			{
				string arg = string.Empty;
				if (num3 < 0)
				{
					arg = string.Format("{0}({1} ~ {2}){3}", new object[]
					{
						textColor,
						num4.ToString(),
						num3.ToString(),
						textColor3
					});
				}
				else
				{
					arg = string.Format("{0}(+{1} ~ +{2}){3}", new object[]
					{
						textColor2,
						num4.ToString(),
						num3.ToString(),
						textColor3
					});
				}
				result = ((MainValue > 0 && MainValue3 > 0) ? string.Format("{0} ~ {1} {2}", optionValue2.ToString(), num2.ToString(), arg) : null);
			}
			else
			{
				result = ((MainValue > 0 && MainValue3 > 0) ? string.Format("{0} ~ {1}", optionValue2.ToString(), num2.ToString()) : null);
			}
		}
		else if (!bEquip && (MainValue2 != 0 || Weight2 != 0f || OptinValue2 != 0) && num4 != 0)
		{
			string arg2 = string.Empty;
			if (num4 < 0)
			{
				arg2 = textColor + NrTSingleton<UIDataManager>.Instance.GetString("(", num4.ToString(), ")") + textColor3;
			}
			else
			{
				arg2 = textColor2 + NrTSingleton<UIDataManager>.Instance.GetString("(", "+", num4.ToString(), ")") + textColor3;
			}
			result = ((MainValue > 0) ? (optionValue2 + " " + arg2) : null);
		}
		else
		{
			result = ((MainValue > 0) ? optionValue2.ToString() : null);
		}
		return result;
	}

	public void Tooltip_Rect(Form cForm, Vector3 showPosition)
	{
		if (TsPlatform.IsMobile)
		{
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMTOOLTIP_SECOND_DLG))
			{
				ItemTooltipDlg_Second itemTooltipDlg_Second = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMTOOLTIP_SECOND_DLG) as ItemTooltipDlg_Second;
				itemTooltipDlg_Second.SetChildForm(base.Orignal_ID, Form.ChildLocation.RIGHT);
			}
			else
			{
				base.SetScreenCenter();
				base.SetLocation(base.GetLocationX(), 10f);
			}
		}
		else if (!TsPlatform.IsMobile || (showPosition == Vector3.zero && ItemTooltipDlg.MOBILE_TOOLTIP_POS == Vector3.zero))
		{
			float num = 20f;
			Vector3 vector = GUICamera.ScreenToGUIPoint(NkInputManager.mousePosition);
			Vector2 size = cForm.GetSize();
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMTOOLTIP_SECOND_DLG))
			{
				ItemTooltipDlg_Second itemTooltipDlg_Second2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMTOOLTIP_SECOND_DLG) as ItemTooltipDlg_Second;
				itemTooltipDlg_Second2.SetChildForm(base.Orignal_ID, Form.ChildLocation.RIGHT);
			}
			else
			{
				float num2 = vector.x + num;
				if (num2 + size.x > GUICamera.width)
				{
					num2 = vector.x - num - size.x;
				}
				float num3 = GUICamera.height - vector.y;
				if (num3 + size.y > GUICamera.height)
				{
					float num4 = num3 + size.y - GUICamera.height;
					num3 -= num4;
				}
				cForm.SetLocation(num2, num3);
			}
		}
		else
		{
			if (showPosition != Vector3.zero && ItemTooltipDlg.MOBILE_TOOLTIP_POS == Vector3.zero)
			{
				showPosition.y = -showPosition.y;
				ItemTooltipDlg.MOBILE_TOOLTIP_POS = showPosition;
			}
			Vector3 vector = GUICamera.ScreenToGUIPoint(ItemTooltipDlg.MOBILE_TOOLTIP_POS);
			Vector2 size2 = cForm.GetSize();
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMTOOLTIP_SECOND_DLG))
			{
				Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMTOOLTIP_SECOND_DLG);
				float x = vector.x;
				float y = vector.y;
				form.SetLocation(x + size2.x + 2f, y);
				cForm.SetLocation(x, y);
			}
			else
			{
				float x = vector.x;
				float y = vector.y;
				cForm.SetLocation(x, y);
			}
		}
	}

	public override void Update()
	{
		base.Update();
		if ((NkInputManager.GetMouseButtonDown(0) || NkInputManager.GetMouseButtonDown(1)) && NrTSingleton<FormsManager>.Instance.FocusFormID() != base.WindowID)
		{
			if (TsPlatform.IsMobile)
			{
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMTOOLTIP_SECOND_DLG))
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_SECOND_DLG);
			}
		}
		if (this.m_eParentWindowID != G_ID.NONE && this.m_eParentWindowID != G_ID.TERRITORY_TOOTIP && !NrTSingleton<FormsManager>.Instance.IsShow(this.m_eParentWindowID))
		{
			this.Close();
		}
	}

	private void ButtonSetting(ref float SizeY, ITEM pkItem)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMTOOLTIP_SECOND_DLG))
		{
			if (this.bItemSkillText2)
			{
				SizeY = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText2.Height + 10f;
			}
			else if (this.bItemSkillText)
			{
				SizeY = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 10f;
			}
			else
			{
				SizeY = this.m_lbText.GetLocationY() + this.m_lbText.Height + 10f;
			}
			this.m_btnEquip.Visible = false;
			this.m_btnUpgrade.Visible = false;
			this.m_btnSell.Visible = false;
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.AUCTION_MAIN_DLG) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DLG_OTHER_CHAR_EQUIPMENT) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMSKILL_DLG) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REDUCEMAIN_DLG) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.EXCHANGE_JEWELRY_DLG) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.EVENT_DAILY_GIFT_DLG))
		{
			this.m_btnEquip.Visible = false;
			this.m_btnUpgrade.Visible = false;
			this.m_btnSell.Visible = false;
		}
		if (this.m_SolInfo != null && this.m_SolInfo.IsSolWarehouse())
		{
			this.m_btnEquip.SetEnabled(false);
			this.m_btnUpgrade.SetEnabled(false);
			this.m_btnSell.SetEnabled(false);
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
		if (this.m_SolInfo != null && itemInfo != null && itemInfo.m_nItemType == 19 && !this.m_SolInfo.IsAtbCommonFlag(2L) && !this.m_SolInfo.GetCharKindInfo().IsATB(1L))
		{
			this.m_btnEquip.SetEnabled(false);
		}
		this.m_btnSell.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemSell));
		this.m_btnSell.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemReleaseEquip));
		this.m_btnEquip.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemUseEquip));
		this.m_btnUpgrade.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemEnhance));
		if (pkItem.m_nPosType == 1 || pkItem.m_nPosType == 2 || pkItem.m_nPosType == 3 || pkItem.m_nPosType == 4)
		{
			this.m_btnSell.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemSell));
			this.m_btnSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("704"));
			this.m_btnEquip.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemUseEquip));
			this.m_btnEquip.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("626"));
			this.m_btnUpgrade.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemEnhance));
			this.m_btnUpgrade.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("261"));
			float num;
			if (this.bItemSkillText2)
			{
				num = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText2.Height + 8f;
			}
			else if (this.bItemSkillText)
			{
				num = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 8f;
			}
			else
			{
				num = this.m_lbText.GetLocationY() + this.m_lbText.Height + 8f;
			}
			this.m_txLine.SetLocationY(num);
			num = this.m_txLine.GetLocationY() + this.m_txLine.GetSize().y + 14f;
			this.m_btnEquip.SetLocation(this.m_btnEquip.GetLocationX(), num);
			this.m_btnUpgrade.SetLocation(this.m_btnUpgrade.GetLocationX(), num);
			this.m_btnSell.SetLocation(this.m_btnSell.GetLocationX(), num);
			if (this.bItemSkillText2)
			{
				SizeY = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText2.Height + 92f;
			}
			else if (this.bItemSkillText)
			{
				SizeY = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 92f;
			}
			else
			{
				SizeY = this.m_lbText.GetLocationY() + this.m_lbText.Height + 92f;
			}
		}
		else if (pkItem.m_nPosType == 10)
		{
			this.m_btnSell.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemReleaseEquip));
			this.m_btnSell.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("627"));
			this.m_btnEquip.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemUseEquip));
			this.m_btnEquip.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("305"));
			this.m_btnUpgrade.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemEnhance));
			this.m_btnUpgrade.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("261"));
			float num;
			if (this.bItemSkillText2)
			{
				num = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText2.Height + 8f;
			}
			else if (this.bItemSkillText)
			{
				num = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 8f;
			}
			else
			{
				num = this.m_lbText.GetLocationY() + this.m_lbText.Height + 8f;
			}
			this.m_txLine.SetLocationY(num);
			num = this.m_txLine.GetLocationY() + this.m_txLine.GetSize().y + 14f;
			this.m_btnEquip.SetLocation(this.m_btnEquip.GetLocationX(), num);
			this.m_btnUpgrade.SetLocation(this.m_btnUpgrade.GetLocationX(), num);
			this.m_btnSell.SetLocation(this.m_btnSell.GetLocationX(), num);
			if (this.bItemSkillText2)
			{
				SizeY = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText2.Height + 92f;
			}
			else if (this.bItemSkillText)
			{
				SizeY = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 92f;
			}
			else
			{
				SizeY = this.m_lbText.GetLocationY() + this.m_lbText.Height + 92f;
			}
		}
		else
		{
			if (Protocol_Item.Enable_Use(pkItem.m_nItemUnique) && !Protocol_Item.Enable_SystemUse(pkItem.m_nItemUnique) && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEM_BOX_RARERANDOM_DLG) && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLRECRUIT_DLG))
			{
				this.m_btnEquip.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemUseEquip));
				this.m_btnEquip.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("573"));
				float num;
				if (this.bItemSkillText2)
				{
					SizeY = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText2.Height + 92f;
					num = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText2.Height + 8f;
				}
				else if (this.bItemSkillText)
				{
					SizeY = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 92f;
					num = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 8f;
				}
				else
				{
					SizeY = this.m_lbText.GetLocationY() + this.m_lbText.Height + 92f;
					num = this.m_lbText.GetLocationY() + this.m_lbText.Height + 8f;
				}
				this.m_txLine.SetLocationY(num);
				num = this.m_txLine.GetLocationY() + this.m_txLine.GetSize().y + 14f;
				this.m_btnEquip.SetLocation(this.m_btnEquip.GetLocationX(), num);
			}
			else if (Protocol_Item.Enable_Compose(pkItem.m_nItemUnique))
			{
				this.m_btnEquip.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("306"));
				float num;
				if (this.bItemSkillText2)
				{
					SizeY = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText2.Height + 92f;
					num = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText2.Height + 8f;
				}
				else if (this.bItemSkillText)
				{
					SizeY = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 92f;
					num = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 8f;
				}
				else
				{
					SizeY = this.m_lbText.GetLocationY() + this.m_lbText.Height + 92f;
					num = this.m_lbText.GetLocationY() + this.m_lbText.Height + 8f;
				}
				this.m_txLine.SetLocationY(num);
				num = this.m_txLine.GetLocationY() + this.m_txLine.GetSize().y + 14f;
				this.m_btnEquip.SetLocation(this.m_btnEquip.GetLocationX(), num);
			}
			else
			{
				if (this.bItemSkillText2)
				{
					SizeY = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText2.Height + 14f;
				}
				else if (this.bItemSkillText)
				{
					SizeY = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 14f;
				}
				else
				{
					SizeY = this.m_lbText.GetLocationY() + this.m_lbText.Height + 14f;
				}
				this.m_btnEquip.Visible = false;
				this.m_txLine.Visible = false;
			}
			this.m_btnUpgrade.Visible = false;
			this.m_btnSell.Visible = false;
		}
	}

	public void ClickItemUseEquip(IUIObject obj)
	{
		if (this.m_eParentWindowID == G_ID.SOLMILITARYGROUP_DLG)
		{
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				if (this.m_SolInfo.GetSolPosType() == 2 || this.m_SolInfo.GetSolPosType() == 6)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("369"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					return;
				}
				SolEquipItemSelectDlg solEquipItemSelectDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLEQUIPITEMSELECT_DLG) as SolEquipItemSelectDlg;
				if (solEquipItemSelectDlg != null)
				{
					solEquipItemSelectDlg.SetData(ref this.m_SolInfo, NrTSingleton<ItemManager>.Instance.GetEquipItemPos(this.m_cItem.m_nItemUnique));
					solEquipItemSelectDlg.SetLocationByForm(solMilitaryGroupDlg);
					solEquipItemSelectDlg.SetFocus();
				}
			}
		}
		else if (this.m_cItem != null)
		{
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_cItem.m_nItemUnique);
			if (itemInfo != null && itemInfo.m_nNeedOpenItemUnique > 0 && itemInfo.m_nNeedOpenItemNum > 0 && !itemInfo.IsItemATB(16384L))
			{
				if (!NrTSingleton<ItemManager>.Instance.CheckBoxNeedItem(itemInfo.m_nItemUnique, true, true))
				{
					return;
				}
				ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemInfo.m_nNeedOpenItemUnique);
				if (itemInfo2 != null)
				{
					string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("171"),
						"targetname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemInfo.m_nNeedOpenItemUnique),
						"count",
						itemInfo.m_nNeedOpenItemNum.ToString(),
						"targetname1",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_cItem.m_nItemUnique)
					});
					MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
					if (msgBoxUI != null)
					{
						msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), null, null, null, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
					}
				}
				return;
			}
			else
			{
				Protocol_Item.Item_Use(this.m_cItem);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DISASSEMBLEITEM_DLG);
			}
		}
		this.Close();
	}

	private void MsgBoxOKEvent(object obj)
	{
		if (this.m_cItem == null)
		{
			return;
		}
		Protocol_Item.Item_Use(this.m_cItem);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DISASSEMBLEITEM_DLG);
		this.Close();
	}

	public void ClickItemEnhance(IUIObject obj)
	{
		if (this.m_SolInfo != null && (this.m_SolInfo.GetSolPosType() == 2 || this.m_SolInfo.GetSolPosType() == 6))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("370"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGERESULT_DLG);
			ReforgeMainDlg reforgeMainDlg = (ReforgeMainDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REFORGEMAIN_DLG);
			reforgeMainDlg.Show();
			reforgeMainDlg.Set_Value(this.m_cItem);
			if (this.m_SolInfo != null)
			{
				reforgeMainDlg.SetSolID(this.m_SolInfo.GetSolID());
			}
			else
			{
				reforgeMainDlg.SetSolID(0L);
			}
		}
		this.Close();
	}

	public void ClickItemSell(IUIObject obj)
	{
		if (this.m_eParentWindowID == G_ID.INVENTORY_DLG)
		{
			bool flag = false;
			ITEM cItem = this.m_cItem;
			if (cItem == null)
			{
				return;
			}
			if (this.m_cItem.IsLock())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("726"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				return;
			}
			switch (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(cItem.m_nItemUnique))
			{
			case eITEM_PART.ITEMPART_WEAPON:
			case eITEM_PART.ITEMPART_ARMOR:
			case eITEM_PART.ITEMPART_ACCESSORY:
				flag = true;
				break;
			}
			if (!flag)
			{
				return;
			}
			if (NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(cItem.m_nItemUnique) == 1000)
			{
				return;
			}
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(cItem.m_nItemUnique);
			if (itemInfo == null)
			{
				return;
			}
			ITEM_SELL itemSellData = NrTSingleton<ITEM_SELL_Manager>.Instance.GetItemSellData(itemInfo.m_nQualityLevel, (int)cItem.GetRank());
			if (itemSellData == null)
			{
				this.Close();
				return;
			}
			int nItemSellMoney = itemSellData.nItemSellMoney;
			ItemSellDlg itemSellDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMSELL_DLG) as ItemSellDlg;
			if (itemSellDlg != null)
			{
				itemSellDlg.SetItemSellInfo(cItem, nItemSellMoney);
			}
		}
		this.Close();
	}

	public void ClickItemReleaseEquip(IUIObject data)
	{
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg == null || !solMilitaryGroupDlg.Visible)
		{
			return;
		}
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		Protocol_Item.Send_EquipSol_InvenEquip(this.m_cItem);
		this.Close();
	}

	public override void OnClose()
	{
	}
}
