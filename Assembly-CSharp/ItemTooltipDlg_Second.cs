using GAME;
using System;
using System.Text;
using UnityEngine;
using UnityForms;

public class ItemTooltipDlg_Second : Form
{
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

	private Button m_btnSell;

	private Button m_btnEquip;

	private Button m_btnUpgrade;

	private bool bText;

	private bool bItemSkillText;

	private bool bItemSkillText2;

	private G_ID m_eParentWindowID;

	private ITEM m_cItem = new ITEM();

	public static Vector3 MOBILE_TOOLTIP_POS = new Vector3(0f, 0f, 0f);

	private long m_SolID;

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

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = false;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/itemtooltip_dlg", G_ID.ITEMTOOLTIP_SECOND_DLG, false);
		if (null != base.InteractivePanel)
		{
			base.SetLocation(base.GetLocation().x, base.GetLocation().y, 92f);
		}
		base.ShowBlackBG(0.5f);
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
		this.m_btnEquip = (base.GetControl("Button_Button01") as Button);
		this.m_btnUpgrade = (base.GetControl("Button_Button02") as Button);
		this.m_btnSell = (base.GetControl("Button_Button03") as Button);
		this.m_btnEquip.Visible = false;
		this.m_btnUpgrade.Visible = false;
		this.m_btnSell.Visible = false;
	}

	public void Set_Tooltip(G_ID a_eWidowID, ITEM a_cItem, bool bEquiped, Vector3 showPosition, long SolID = 0L)
	{
		this.m_eParentWindowID = a_eWidowID;
		this.m_cItem = a_cItem;
		this.m_SolID = SolID;
		this.Item_Tooltip(this, this.m_cItem, null, this.m_eParentWindowID, bEquiped);
		this.Show();
		ItemTooltipDlg_Second.Tooltip_Rect(this, showPosition);
	}

	public void Set_TooltipForEquip(G_ID eWidowID, ITEM pkEquipedItem, ITEM pkItem, bool bEquiped)
	{
		this.m_eParentWindowID = eWidowID;
		this.m_cItem = pkItem;
		this.Item_Tooltip(this, pkEquipedItem, this.m_cItem, eWidowID, bEquiped);
		this.Show();
		ItemTooltipDlg_Second.Tooltip_Rect(this, Vector3.zero);
	}

	public void Set_Tooltip(G_ID eWidowID, ITEM pkItem, ITEM pkEquipedItem, Vector3 showPosition)
	{
		this.m_eParentWindowID = eWidowID;
		this.m_cItem = pkItem;
		this.Item_Tooltip(this, this.m_cItem, pkEquipedItem, eWidowID, false);
		this.Show();
		ItemTooltipDlg_Second.Tooltip_Rect(this, showPosition);
	}

	public void Set_Tooltip(G_ID eWidowID, ITEM pkItem, ITEM pkEquipedItem)
	{
		this.m_eParentWindowID = eWidowID;
		this.m_cItem = pkItem;
		this.Item_Tooltip(this, this.m_cItem, pkEquipedItem, eWidowID, false);
		this.Show();
		ItemTooltipDlg_Second.Tooltip_Rect(this, Vector3.zero);
	}

	public void Set_Tooltip(ITEM pkItem)
	{
		this.m_cItem = pkItem;
		this.Item_Tooltip(this, this.m_cItem, null, G_ID.NONE, false);
		this.Show();
		ItemTooltipDlg_Second.Tooltip_Rect(this, Vector3.zero);
	}

	public void Set_Tooltip(G_ID eWidowID, int itemunique)
	{
		this.m_eParentWindowID = eWidowID;
		this.m_cItem.Init();
		this.m_cItem.m_nItemUnique = itemunique;
		this.Item_Tooltip(this, this.m_cItem, null, eWidowID, false);
		this.Show();
		ItemTooltipDlg_Second.Tooltip_Rect(this, Vector3.zero);
	}

	public void Set_Tooltip(G_ID a_eWidowID, ITEM a_cItem, bool bEquiped = false)
	{
		this.m_eParentWindowID = a_eWidowID;
		this.m_cItem = a_cItem;
		this.Item_Tooltip(this, this.m_cItem, null, this.m_eParentWindowID, bEquiped);
		this.Show();
		ItemTooltipDlg_Second.Tooltip_Rect(this, Vector3.zero);
	}

	public void Item_Tooltip(Form cThis, ITEM pkItem, ITEM pkEquipItem, G_ID eWidowID, bool bEquiped = false)
	{
		if (pkItem == null || !pkItem.IsValid())
		{
			return;
		}
		ItemOption_Text[] array = ItemTooltipDlg.Get_Item_Info(pkItem, pkEquipItem, bEquiped, true);
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
		if (itemInfo == null)
		{
			return;
		}
		int rank = pkItem.m_nOption[2];
		int num = 0;
		string name = NrTSingleton<ItemManager>.Instance.GetName(pkItem);
		this.m_txClass.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString(rank));
		this.m_lbTitle.Text = string.Format("{0} {1}", name, (!bEquiped) ? string.Empty : string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("2002"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1479")));
		this.m_itItemTex.SetItemTexture(pkItem);
		if (pkItem.m_nPosType == 10 || Protocol_Item.Is_EquipItem(pkItem.m_nItemUnique))
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
				this.m_lbItemSkillText.SetLocation(this.m_lbItemSkillText.GetLocation().x, this.m_lbItemSkillName.GetLocationY() + this.m_lbItemSkillName.Height + 10f);
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
		float height;
		if (this.bItemSkillText)
		{
			if (this.bItemSkillText2)
			{
				height = this.m_lbItemSkillText2.GetLocationY() + this.m_lbItemSkillText.Height + 10f;
			}
			else
			{
				height = this.m_lbItemSkillText.GetLocationY() + this.m_lbItemSkillText.Height + 10f;
			}
		}
		else
		{
			height = this.m_lbText.GetLocationY() + this.m_lbText.Height + 14f;
		}
		base.SetSize(base.GetSizeX(), height);
		this.m_txBG.SetSize(base.GetSize().x, height);
	}

	public static void Tooltip_Rect(Form cForm, Vector3 showPosition)
	{
		if (TsPlatform.IsMobile)
		{
			float x = (GUICamera.width - cForm.GetSizeX() * 2f) / 2f;
			cForm.SetLocation(x, 10f);
		}
		else if (!TsPlatform.IsMobile || (showPosition == Vector3.zero && ItemTooltipDlg_Second.MOBILE_TOOLTIP_POS == Vector3.zero))
		{
			float num = 20f;
			Vector3 vector = GUICamera.ScreenToGUIPoint(NkInputManager.mousePosition);
			Vector2 size = cForm.GetSize();
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMTOOLTIP_SECOND_DLG))
			{
				float x2 = (GUICamera.width - cForm.GetSizeX() * 2f) / 2f;
				cForm.SetLocation(x2, 10f);
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
			if (showPosition != Vector3.zero && ItemTooltipDlg_Second.MOBILE_TOOLTIP_POS == Vector3.zero)
			{
				showPosition.y = -showPosition.y;
				ItemTooltipDlg_Second.MOBILE_TOOLTIP_POS = showPosition;
			}
			Vector3 vector = GUICamera.ScreenToGUIPoint(ItemTooltipDlg_Second.MOBILE_TOOLTIP_POS);
			Vector2 size2 = cForm.GetSize();
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMTOOLTIP_SECOND_DLG))
			{
				Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMTOOLTIP_SECOND_DLG);
				float x3 = vector.x;
				float y = vector.y;
				form.SetLocation(x3 + size2.x + 2f, y);
				cForm.SetLocation(x3, y);
			}
			else
			{
				float x3 = vector.x;
				float y = vector.y;
				cForm.SetLocation(x3, y);
			}
		}
	}

	public override void Update()
	{
		base.Update();
		if (this.m_eParentWindowID != G_ID.NONE && this.m_eParentWindowID != G_ID.TERRITORY_TOOTIP && !NrTSingleton<FormsManager>.Instance.IsShow(this.m_eParentWindowID))
		{
			this.Close();
		}
	}

	public override void OnClose()
	{
	}

	public override void AfterShow()
	{
		if (TsPlatform.IsMobile)
		{
			ItemTooltip_Btn_Dlg itemTooltip_Btn_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_BUTTON) as ItemTooltip_Btn_Dlg;
			itemTooltip_Btn_Dlg.SetLocation(GUICamera.width - itemTooltip_Btn_Dlg.GetSizeX(), GUICamera.height - itemTooltip_Btn_Dlg.GetSizeY());
			base.InteractivePanel.twinFormID = G_ID.ITEMTOOLTIP_BUTTON;
		}
	}
}
