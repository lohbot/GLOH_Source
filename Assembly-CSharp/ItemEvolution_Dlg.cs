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

public class ItemEvolution_Dlg : Form
{
	private enum SHOWTYPE
	{
		ITEM,
		SOLDER,
		SOLITEM
	}

	private readonly int ORISTONENUM = 5;

	private readonly int EVOLUTIONSTONENUM = 4;

	private readonly int SLOTNUM = 6;

	private readonly int MATERIAL_EVOLUTIONSTRON_UNIQUE_1 = 50408;

	private readonly int MATERIAL_EVOLUTIONSTRON_UNIQUE_2 = 50409;

	private readonly int MATERIAL_EVOLUTIONSTRON_UNIQUE_3 = 50410;

	private readonly int MATERIAL_EVOLUTIONSTRON_UNIQUE_4 = 50411;

	private readonly int MATERIAL_LEGENDSOURCE_UNIQUE_1 = 50412;

	private DrawTexture m_DT_Back;

	private DrawTexture m_DT_StoneIcon;

	private ItemTexture m_IT_OriIcon;

	private DrawTexture m_DT_GoldIcon;

	private ItemTexture m_IT_5;

	private DrawTexture[] m_DT_OriStar;

	private DrawTexture m_DT_Nope1BG;

	private DrawTexture m_DT_Nope2BG;

	private DrawTexture m_DT_Nope3BG;

	private DrawTexture[] m_DT_CheckBG;

	private DrawTexture[] m_DT_Check;

	private DrawTexture m_DrawTexture_ringslotlock;

	private DrawTexture m_DrawTexture_subbg3;

	private ItemTexture m_IT_RewardItemIcon;

	private DrawTexture[] m_DrawTexture_slot;

	private DrawTexture m_DT_DropDownBox;

	private DrawTexture[] m_DT_Impossible;

	private Label m_LB_StoneName;

	private Label m_LB_OriName;

	private Label[] m_LB_EvolutionStone;

	private Label m_LB_Ori;

	private Label m_LB_Gold;

	private Label m_LB_ItemName;

	private Label m_Label_ItemSkillName;

	private Label m_Label_text6;

	private Label m_LB_Num1;

	private Label m_LB_Num2;

	private Label m_LB_GoldNum;

	private Label m_LB_Nope1;

	private Label m_LB_Nope2;

	private Label m_LB_Nope3;

	private NewListBox m_NLB_EquipmentList;

	private NewListBox m_NLB_SolList;

	private Button m_Button_confirm;

	private Button m_Btn_Info;

	private Button m_btnHelp;

	private ImageView[] m_ImageView_slot;

	private Toolbar m_ToolBar_tab;

	private ListBox m_ListBox_DropDownList1;

	private DropDownList m_DropDownList_DropDownList1;

	private byte m_nSearch_SolPosType = 1;

	private int m_nSearch_SolSortType = 4;

	private List<NkSoldierInfo> m_SolList = new List<NkSoldierInfo>();

	private List<NkSoldierInfo> m_SolSortList = new List<NkSoldierInfo>();

	private List<ITEM> m_InvenItemLIst = new List<ITEM>();

	private ITEM m_SelectItem;

	private NkSoldierInfo m_SelectSol;

	private ItemEvolution_Dlg.SHOWTYPE m_showtype;

	private long m_SolID;

	private long m_SelectItemSol;

	private byte m_byMilityUnique;

	private int[] BottomLayerMeterial;

	private float m_TimeCheck;

	private float fStartTime;

	private bool bLoadEvolutionEffect;

	private bool bRequest;

	private bool isSaveZpos;

	private float BackUpZPos;

	private GameObject rootGameObject;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/itemskill/dlg_equipmentevolution", G_ID.ITEMEVOLUTION_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_DT_Back = (base.GetControl("DT_Back") as DrawTexture);
		this.m_DT_StoneIcon = (base.GetControl("DT_StoneIcon") as DrawTexture);
		this.m_IT_OriIcon = (base.GetControl("IT_OriIcon") as ItemTexture);
		this.m_DT_GoldIcon = (base.GetControl("DT_GoldIcon") as DrawTexture);
		this.m_IT_5 = (base.GetControl("IT_5") as ItemTexture);
		this.m_DT_OriStar = new DrawTexture[this.ORISTONENUM];
		this.m_DT_OriStar[0] = (base.GetControl("DT_OriStar2") as DrawTexture);
		this.m_DT_OriStar[1] = (base.GetControl("DT_OriStar3") as DrawTexture);
		this.m_DT_OriStar[2] = (base.GetControl("DT_OriStar4") as DrawTexture);
		this.m_DT_OriStar[3] = (base.GetControl("DT_OriStar5") as DrawTexture);
		this.m_DT_OriStar[4] = (base.GetControl("DT_OriStar6") as DrawTexture);
		this.m_DT_Nope1BG = (base.GetControl("DT_Nope1BG") as DrawTexture);
		this.m_DT_Nope2BG = (base.GetControl("DT_Nope2BG") as DrawTexture);
		this.m_DT_Nope3BG = (base.GetControl("DT_Nope3BG") as DrawTexture);
		this.m_DT_CheckBG = new DrawTexture[this.SLOTNUM];
		this.m_DT_CheckBG[0] = (base.GetControl("DT_CheckBG1") as DrawTexture);
		this.m_DT_CheckBG[1] = (base.GetControl("DT_CheckBG2") as DrawTexture);
		this.m_DT_CheckBG[2] = (base.GetControl("DT_CheckBG3") as DrawTexture);
		this.m_DT_CheckBG[3] = (base.GetControl("DT_CheckBG4") as DrawTexture);
		this.m_DT_CheckBG[4] = (base.GetControl("DT_CheckBG5") as DrawTexture);
		this.m_DT_CheckBG[5] = (base.GetControl("DT_CheckBG6") as DrawTexture);
		this.m_DT_Check = new DrawTexture[this.SLOTNUM];
		this.m_DT_Check[0] = (base.GetControl("DT_Check1") as DrawTexture);
		this.m_DT_Check[1] = (base.GetControl("DT_Check2") as DrawTexture);
		this.m_DT_Check[2] = (base.GetControl("DT_Check3") as DrawTexture);
		this.m_DT_Check[3] = (base.GetControl("DT_Check4") as DrawTexture);
		this.m_DT_Check[4] = (base.GetControl("DT_Check5") as DrawTexture);
		this.m_DT_Check[5] = (base.GetControl("DT_Check6") as DrawTexture);
		this.m_DrawTexture_ringslotlock = (base.GetControl("DrawTexture_ringslotlock") as DrawTexture);
		this.m_DrawTexture_subbg3 = (base.GetControl("DrawTexture_subbg3") as DrawTexture);
		this.m_DrawTexture_slot = new DrawTexture[this.SLOTNUM];
		this.m_DrawTexture_slot[0] = (base.GetControl("DrawTexture_slot1") as DrawTexture);
		this.m_DrawTexture_slot[1] = (base.GetControl("DrawTexture_slot2") as DrawTexture);
		this.m_DrawTexture_slot[2] = (base.GetControl("DrawTexture_slot3") as DrawTexture);
		this.m_DrawTexture_slot[3] = (base.GetControl("DrawTexture_slot4") as DrawTexture);
		this.m_DrawTexture_slot[4] = (base.GetControl("DrawTexture_slot5") as DrawTexture);
		this.m_DrawTexture_slot[5] = (base.GetControl("DrawTexture_slot6") as DrawTexture);
		this.m_DT_DropDownBox = (base.GetControl("DT_DropDownBox") as DrawTexture);
		this.m_DT_Impossible = new DrawTexture[this.SLOTNUM];
		this.m_DT_Impossible[0] = (base.GetControl("DT_Impossible01") as DrawTexture);
		this.m_DT_Impossible[1] = (base.GetControl("DT_Impossible02") as DrawTexture);
		this.m_DT_Impossible[2] = (base.GetControl("DT_Impossible03") as DrawTexture);
		this.m_DT_Impossible[3] = (base.GetControl("DT_Impossible04") as DrawTexture);
		this.m_DT_Impossible[4] = (base.GetControl("DT_Impossible05") as DrawTexture);
		this.m_DT_Impossible[5] = (base.GetControl("DT_Impossible06") as DrawTexture);
		this.m_IT_RewardItemIcon = (base.GetControl("IT_RewardItemIcon") as ItemTexture);
		this.m_LB_StoneName = (base.GetControl("LB_StoneName") as Label);
		this.m_LB_OriName = (base.GetControl("LB_OriName") as Label);
		this.m_LB_EvolutionStone = new Label[this.EVOLUTIONSTONENUM];
		this.m_LB_EvolutionStone[0] = (base.GetControl("LB_EvolutionStone1") as Label);
		this.m_LB_EvolutionStone[1] = (base.GetControl("LB_EvolutionStone2") as Label);
		this.m_LB_EvolutionStone[2] = (base.GetControl("LB_EvolutionStone3") as Label);
		this.m_LB_EvolutionStone[3] = (base.GetControl("LB_EvolutionStone4") as Label);
		this.m_LB_Ori = (base.GetControl("LB_Ori") as Label);
		this.m_LB_Gold = (base.GetControl("LB_Gold") as Label);
		this.m_LB_ItemName = (base.GetControl("LB_ItemName") as Label);
		this.m_Label_ItemSkillName = (base.GetControl("Label_ItemSkillName") as Label);
		this.m_Label_text6 = (base.GetControl("Label_text6") as Label);
		this.m_LB_Num1 = (base.GetControl("LB_Num1") as Label);
		this.m_LB_Num2 = (base.GetControl("LB_Num2") as Label);
		this.m_LB_GoldNum = (base.GetControl("LB_GoldNum") as Label);
		this.m_LB_Nope1 = (base.GetControl("LB_Nope1") as Label);
		this.m_LB_Nope2 = (base.GetControl("LB_Nope2") as Label);
		this.m_LB_Nope3 = (base.GetControl("LB_Nope3") as Label);
		this.m_NLB_EquipmentList = (base.GetControl("NLB_EquipmentList") as NewListBox);
		this.m_NLB_EquipmentList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemClick));
		this.m_NLB_EquipmentList.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnSolClick));
		this.m_NLB_SolList = (base.GetControl("NLB_SolList") as NewListBox);
		this.m_NLB_SolList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnSolClick));
		this.m_NLB_SolList.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnItemClick));
		this.m_Button_confirm = (base.GetControl("Button_confirm") as Button);
		this.m_Button_confirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemEvolutionConfirm));
		this.m_Btn_Info = (base.GetControl("Btn_Info") as Button);
		this.m_Btn_Info.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemInfo));
		this.m_btnHelp = (base.GetControl("Help_Button") as Button);
		this.m_btnHelp.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		this.m_ImageView_slot = new ImageView[this.SLOTNUM];
		for (int i = 0; i < this.SLOTNUM; i++)
		{
			this.m_ImageView_slot[i] = (base.GetControl("ImageView_slot" + (i + 1).ToString()) as ImageView);
			this.m_ImageView_slot[i].SetImageView(1, 1, 70, 70, 1, 1, (int)this.m_ImageView_slot[i].GetSize().y);
			this.m_ImageView_slot[i].spacingAtEnds = false;
			this.m_ImageView_slot[i].touchScroll = false;
			this.m_ImageView_slot[i].clipContents = false;
			this.m_ImageView_slot[i].ListDrag = false;
			this.m_ImageView_slot[i].isDragging = false;
		}
		this.m_ToolBar_tab = (base.GetControl("ToolBar_tab") as Toolbar);
		this.m_ToolBar_tab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("204");
		this.m_ToolBar_tab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1490");
		UIPanelTab expr_7D9 = this.m_ToolBar_tab.Control_Tab[0];
		expr_7D9.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_7D9.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_807 = this.m_ToolBar_tab.Control_Tab[1];
		expr_807.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_807.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_ListBox_DropDownList1 = (base.GetControl("DropDownList_DropDownList1") as ListBox);
		this.m_DropDownList_DropDownList1 = (base.GetControl("DropDownList_DropDownList1") as DropDownList);
		this.m_DropDownList_DropDownList1.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), 1);
		this.m_DropDownList_DropDownList1.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("123"), 0);
		this.m_DropDownList_DropDownList1.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("120"), 100);
		this.m_DropDownList_DropDownList1.SetViewArea(this.m_DropDownList_DropDownList1.Count);
		this.m_DropDownList_DropDownList1.RepositionItems();
		this.m_DropDownList_DropDownList1.SetFirstItem();
		this.m_DropDownList_DropDownList1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeSortSolList));
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
		this.ComponentInit();
		this.DataInit();
		this.ShowTypeBG();
	}

	public override void Update()
	{
		if (this.bLoadEvolutionEffect && Time.time - this.fStartTime > 9f)
		{
			UnityEngine.Object.DestroyImmediate(this.rootGameObject);
			this.bLoadEvolutionEffect = false;
			this.bRequest = false;
			this.SendEvolutionDataToServer();
			TsAudio.RestoreMuteAllAudio();
			TsAudio.RefreshAllMuteAudio();
			NkInputManager.IsInputMode = true;
		}
		if (base.Visible && Time.time - this.m_TimeCheck > 1f)
		{
			this.m_TimeCheck = Time.deltaTime;
			int itemCnt = NkUserInventory.GetInstance().GetItemCnt(this.MATERIAL_EVOLUTIONSTRON_UNIQUE_1);
			int itemCnt2 = NkUserInventory.GetInstance().GetItemCnt(this.MATERIAL_EVOLUTIONSTRON_UNIQUE_2);
			int itemCnt3 = NkUserInventory.GetInstance().GetItemCnt(this.MATERIAL_EVOLUTIONSTRON_UNIQUE_3);
			int itemCnt4 = NkUserInventory.GetInstance().GetItemCnt(this.MATERIAL_EVOLUTIONSTRON_UNIQUE_4);
			this.SetBottomMaterial(itemCnt, itemCnt2, itemCnt3, itemCnt4);
			if (this.m_SelectItem != null && this.m_SelectItem.m_nItemUnique != 0 && NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(this.m_SelectItem.m_nItemUnique) != null)
			{
				this.SetBottonLegendSource(NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(this.m_SelectItem.m_nItemUnique).nNeedItem_Unique[1]);
			}
			else
			{
				this.SetBottonLegendSource(this.MATERIAL_LEGENDSOURCE_UNIQUE_1);
			}
		}
	}

	public override void OnClose()
	{
		NkInputManager.IsInputMode = true;
	}

	public void ComponentInit()
	{
		this.m_DT_Back.SetTextureFromBundle("UI/EquipmentEvolution/EquipmentEvolution_Back");
		this.m_NLB_EquipmentList.Visible = true;
	}

	public void DataInit()
	{
		this.m_showtype = ItemEvolution_Dlg.SHOWTYPE.ITEM;
		this.BottomLayerMeterial = new int[this.EVOLUTIONSTONENUM];
		int itemCnt = NkUserInventory.GetInstance().GetItemCnt(this.MATERIAL_EVOLUTIONSTRON_UNIQUE_1);
		int itemCnt2 = NkUserInventory.GetInstance().GetItemCnt(this.MATERIAL_EVOLUTIONSTRON_UNIQUE_2);
		int itemCnt3 = NkUserInventory.GetInstance().GetItemCnt(this.MATERIAL_EVOLUTIONSTRON_UNIQUE_3);
		int itemCnt4 = NkUserInventory.GetInstance().GetItemCnt(this.MATERIAL_EVOLUTIONSTRON_UNIQUE_4);
		this.SetBottomMaterial(itemCnt, itemCnt2, itemCnt3, itemCnt4);
		this.SetBottonLegendSource(this.MATERIAL_LEGENDSOURCE_UNIQUE_1);
	}

	public void SetBottomMaterial(int num1, int num2, int num3, int num4)
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
			"Count",
			ANNUALIZED.Convert(num1)
		});
		this.m_LB_EvolutionStone[0].SetText(empty);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
			"Count",
			ANNUALIZED.Convert(num2)
		});
		this.m_LB_EvolutionStone[1].SetText(empty);
		this.BottomLayerMeterial[2] = num3;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
			"Count",
			ANNUALIZED.Convert(num3)
		});
		this.m_LB_EvolutionStone[2].SetText(empty);
		this.BottomLayerMeterial[3] = num4;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
			"Count",
			ANNUALIZED.Convert(num4)
		});
		this.m_LB_EvolutionStone[3].SetText(empty);
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
		this.m_LB_Gold.SetText(text);
	}

	private void InitNeedItem()
	{
		this.m_IT_RewardItemIcon.Visible = false;
		this.m_LB_ItemName.Visible = false;
		this.m_Label_ItemSkillName.Visible = false;
		this.m_DT_StoneIcon.Visible = false;
		this.m_IT_OriIcon.Visible = false;
		this.m_DT_GoldIcon.Visible = false;
		this.m_LB_StoneName.Visible = false;
		this.m_LB_OriName.Visible = false;
		this.m_LB_Num1.Visible = false;
		this.m_LB_Num2.Visible = false;
		this.m_LB_GoldNum.Visible = false;
		this.m_DT_Nope1BG.Visible = false;
		this.m_DT_Nope2BG.Visible = false;
		this.m_DT_Nope3BG.Visible = false;
		this.m_LB_Nope1.Visible = false;
		this.m_LB_Nope2.Visible = false;
		this.m_LB_Nope3.Visible = false;
		this.m_Label_text6.Visible = false;
		this.m_Button_confirm.SetEnabled(false);
		this.m_Btn_Info.SetEnabled(false);
	}

	private void initInvenShow()
	{
		for (int i = 0; i < this.SLOTNUM; i++)
		{
			this.m_DT_CheckBG[i].Visible = false;
			this.m_DT_Check[i].Visible = false;
			this.m_DT_Impossible[i].Visible = false;
		}
		this.m_DrawTexture_ringslotlock.Visible = false;
		this.m_NLB_EquipmentList.Visible = true;
		this.m_NLB_SolList.Visible = false;
		this.m_ListBox_DropDownList1.Visible = false;
		this.m_DT_DropDownBox.Visible = true;
		this.m_DropDownList_DropDownList1.Visible = false;
	}

	private void initSolShow()
	{
		for (int i = 0; i < this.SLOTNUM; i++)
		{
			this.m_DT_CheckBG[i].Visible = true;
			this.m_DT_Check[i].Visible = false;
			this.m_DT_Impossible[i].Visible = false;
		}
		this.m_DrawTexture_ringslotlock.Visible = false;
		this.m_NLB_EquipmentList.Visible = false;
		this.m_NLB_SolList.Visible = true;
		this.m_ListBox_DropDownList1.Visible = true;
		this.m_DT_DropDownBox.Visible = true;
		this.m_DropDownList_DropDownList1.Visible = false;
		for (int j = 0; j < 6; j++)
		{
			if (!(this.m_ImageView_slot[j] == null))
			{
				this.m_ImageView_slot[j].Clear();
			}
		}
	}

	public void SetBottonLegendSource(int Unique_NUM)
	{
		string empty = string.Empty;
		this.m_IT_5.SetItemTexture(Unique_NUM, NrTSingleton<ItemManager>.Instance.GetItemInfo(Unique_NUM).m_nUseMinLevel, false, 0.7f);
		int itemCnt = NkUserInventory.GetInstance().GetItemCnt(Unique_NUM);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
			"Count",
			ANNUALIZED.Convert(itemCnt)
		});
		this.m_LB_Ori.SetText(empty);
	}

	private void ShowTypeBG()
	{
		this.InitNeedItem();
		if (this.m_showtype == ItemEvolution_Dlg.SHOWTYPE.ITEM)
		{
			this.m_DrawTexture_subbg3.Visible = false;
			for (int i = 0; i < this.SLOTNUM; i++)
			{
				this.m_ImageView_slot[i].Visible = false;
				this.m_DrawTexture_slot[i].Visible = false;
			}
			this.initInvenShow();
			this.SetInvItemData();
		}
		else if (this.m_showtype == ItemEvolution_Dlg.SHOWTYPE.SOLDER)
		{
			this.m_DrawTexture_subbg3.Visible = true;
			for (int j = 0; j < this.SLOTNUM; j++)
			{
				this.m_ImageView_slot[j].Visible = true;
				this.m_DrawTexture_slot[j].Visible = true;
			}
			this.initSolShow();
			this.SetSolData();
		}
	}

	public void SetData()
	{
		ItemEvolution_Dlg.SHOWTYPE showtype = this.m_showtype;
		if (showtype != ItemEvolution_Dlg.SHOWTYPE.ITEM)
		{
			if (showtype == ItemEvolution_Dlg.SHOWTYPE.SOLDER)
			{
				this.SetSolData();
			}
		}
		else
		{
			this.SetInvItemData();
		}
	}

	private void SetInvItemData()
	{
		this.m_SolID = 0L;
		this.m_NLB_EquipmentList.Clear();
		this.m_InvenItemLIst.Clear();
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
								if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
								{
									if (NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(item.m_nItemUnique) != null)
									{
										int num = item.m_nOption[9];
										if (num >= NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(item.m_nItemUnique).nItemSkill_Condition)
										{
											this.m_InvenItemLIst.Add(item);
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (this.m_InvenItemLIst.Count > 0)
		{
			this.m_InvenItemLIst.Sort(new Comparison<ITEM>(this.CompareItemLevel));
			for (int k = 0; k < this.m_InvenItemLIst.Count; k++)
			{
				NewListItem item2 = new NewListItem(this.m_NLB_EquipmentList.ColumnNum, true, string.Empty);
				this.SetItemColum(this.m_InvenItemLIst[k], k, ref item2);
				this.m_NLB_EquipmentList.Add(item2);
			}
		}
		else
		{
			this.m_Label_text6.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3514"));
			this.m_Label_text6.Visible = true;
		}
	}

	private void SetSolData()
	{
		this.MakeSolListAndSort();
		this.m_NLB_SolList.Clear();
		for (int i = 0; i < this.m_SolSortList.Count; i++)
		{
			NewListItem item = new NewListItem(this.m_NLB_SolList.ColumnNum, true, string.Empty);
			this.SetSolColum(i, ref item);
			this.m_NLB_SolList.Add(item);
		}
		this.m_NLB_SolList.RepositionItems();
		this.m_Label_text6.Visible = false;
	}

	public void OnChangeSortSolList(IUIObject obj)
	{
		if (this.m_showtype != ItemEvolution_Dlg.SHOWTYPE.SOLDER)
		{
			this.m_showtype = ItemEvolution_Dlg.SHOWTYPE.SOLDER;
			this.m_ToolBar_tab.SetSelectTabIndex(1);
			this.ShowTypeBG();
		}
		this.m_nSearch_SolPosType = 1;
		if (this.m_DropDownList_DropDownList1.Count > 0 && this.m_DropDownList_DropDownList1.SelectedItem != null)
		{
			ListItem listItem = this.m_DropDownList_DropDownList1.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.m_nSearch_SolPosType = (byte)listItem.Key;
				if (this.m_nSearch_SolPosType == 2 || this.m_nSearch_SolPosType == 6)
				{
					this.m_byMilityUnique = (byte)this.m_DropDownList_DropDownList1.SelectIndex;
				}
			}
		}
		this.initSolShow();
		this.SetData();
	}

	public void SendItemEvolutionSet(ITEM srcItem)
	{
		this.m_SelectItem = srcItem;
		int num = this.m_SelectItem.m_nOption[5];
		int skillUnique = this.m_SelectItem.m_nOption[4];
		ITEMEVOLUTION itemEvolutionData = NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(this.m_SelectItem.m_nItemUnique);
		if (itemEvolutionData != null)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1721"),
				"gold",
				ANNUALIZED.Convert(itemEvolutionData.nNeedGold)
			});
			this.m_LB_GoldNum.SetText(empty);
			this.m_LB_GoldNum.Visible = true;
			this.m_DT_GoldIcon.Visible = true;
			empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
				"Count",
				itemEvolutionData.nNeedItem_num[0]
			});
			this.m_LB_Num1.SetText(empty);
			this.m_LB_Num1.Visible = true;
			this.m_LB_StoneName.SetText(NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemEvolutionData.nNeedItem_Unique[0]));
			this.m_LB_StoneName.Visible = true;
			empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("571"),
				"Count",
				itemEvolutionData.nNeedItem_num[1]
			});
			this.m_LB_Num2.SetText(empty);
			this.m_LB_Num2.Visible = true;
			this.m_LB_OriName.SetText(NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemEvolutionData.nNeedItem_Unique[1]));
			this.m_LB_OriName.Visible = true;
			this.m_DT_StoneIcon.SetTexture(NrTSingleton<ItemManager>.Instance.GetItemTexture(itemEvolutionData.nNeedItem_Unique[0]));
			this.m_DT_StoneIcon.Visible = true;
			this.m_IT_OriIcon.SetItemTexture(itemEvolutionData.nNeedItem_Unique[1], 0, false, 1f);
			this.m_IT_OriIcon.Visible = true;
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
			if (battleSkillBase != null)
			{
				this.m_IT_RewardItemIcon.SetItemTexture(itemEvolutionData.nResult_Index, NrTSingleton<ItemManager>.Instance.GetItemInfo(itemEvolutionData.nResult_Index).m_nUseMinLevel, true, 1f);
				this.m_IT_RewardItemIcon.Visible = true;
				empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2931"),
					"grade",
					NrTSingleton<ItemManager>.Instance.GetItemInfo(itemEvolutionData.nResult_Index).m_nStarGrade,
					"target",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemEvolutionData.nResult_Index)
				});
				this.m_LB_ItemName.SetText(empty);
				this.m_LB_ItemName.Visible = true;
				int num2 = num - itemEvolutionData.nItemSkillPenalty;
				if (1 > num2)
				{
					num2 = 1;
				}
				empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1292"),
					"skillname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
					"skilllevel",
					num2
				});
				this.m_Label_ItemSkillName.SetText(empty);
				this.m_Label_ItemSkillName.Visible = true;
			}
			this.m_Btn_Info.SetEnabled(true);
			this.SetBottonLegendSource(itemEvolutionData.nNeedItem_Unique[1]);
			this.m_Button_confirm.SetEnabled(false);
			if (this.CheckEvolutonNeedItem())
			{
				this.m_Button_confirm.SetEnabled(true);
			}
		}
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
					if (!(this.m_ImageView_slot[i] == null))
					{
						this.m_ImageView_slot[i].Clear();
						ImageSlot imageSlot = new ImageSlot();
						ITEM equipItem = soldierInfoFromSolID.GetEquipItem(i);
						bool flag = true;
						bool flag2 = false;
						bool flag3 = false;
						if (equipItem != null)
						{
							ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(equipItem.m_nItemUnique);
							if (itemInfo != null)
							{
								if (itemInfo.m_nDurability == 0)
								{
									flag = false;
								}
								itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(equipItem.m_nItemUnique);
								if (itemInfo == null)
								{
									flag = false;
								}
								if (itemInfo.IsItemATB(2097152L))
								{
									flag = false;
								}
								if (!itemInfo.IsItemATB(131072L) && !itemInfo.IsItemATB(524288L))
								{
									flag = false;
								}
								if (NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(equipItem.m_nItemUnique) == null)
								{
									flag = false;
									flag2 = true;
								}
								else
								{
									int num = equipItem.m_nOption[9];
									if (num < NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(equipItem.m_nItemUnique).nItemSkill_Condition)
									{
										flag = false;
										flag3 = true;
									}
								}
							}
						}
						this.m_ImageView_slot[i].RemoveValueChangedDelegate(new EZValueChangedDelegate(this.On_NoItemList_Notification));
						this.m_ImageView_slot[i].RemoveValueChangedDelegate(new EZValueChangedDelegate(this.On_NotNeedSkillLevel_Notification));
						this.m_ImageView_slot[i].RemoveValueChangedDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Click));
						this.m_ImageView_slot[i].RemoveMouseOutDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Out));
						if (flag)
						{
							this.m_ImageView_slot[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Click));
							this.m_ImageView_slot[i].AddMouseOutDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Out));
							this.m_DT_Impossible[i].Visible = false;
						}
						else
						{
							this.m_ImageView_slot[i].RemoveValueChangedDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Click));
							this.m_ImageView_slot[i].RemoveMouseOutDelegate(new EZValueChangedDelegate(this.On_EquipMouse_Out));
							this.m_DT_Impossible[i].Visible = true;
							if (flag2)
							{
								this.m_ImageView_slot[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.On_NoItemList_Notification));
							}
							else if (flag3)
							{
								this.m_ImageView_slot[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.On_NotNeedSkillLevel_Notification));
							}
						}
						if (equipItem == null)
						{
							this.m_DT_Impossible[i].Visible = false;
						}
						if (equipItem != null && equipItem.m_nItemID != 0L)
						{
							imageSlot.c_oItem = equipItem;
							imageSlot.c_bEnable = false;
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
						this.m_ImageView_slot[i].SetImageSlot(0, imageSlot, null, null, null, null);
						this.m_ImageView_slot[i].RepositionItems();
						Vector3 localPosition = default(Vector3);
						localPosition = this.m_ImageView_slot[i].gameObject.transform.localPosition;
						if (!this.isSaveZpos)
						{
							this.isSaveZpos = true;
							this.BackUpZPos = localPosition.z;
						}
						if (flag)
						{
							localPosition.z = this.BackUpZPos;
							this.m_ImageView_slot[i].gameObject.transform.localPosition = localPosition;
						}
						else
						{
							localPosition.z = 0.05f;
							this.m_ImageView_slot[i].gameObject.transform.localPosition = localPosition;
						}
						this.m_DT_Check[i].Visible = false;
					}
				}
			}
		}
	}

	public bool CheckEvolutonNeedItem()
	{
		bool result = true;
		ITEMEVOLUTION itemEvolutionData = NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(this.m_SelectItem.m_nItemUnique);
		this.m_LB_Nope1.Visible = false;
		this.m_DT_Nope1BG.Visible = false;
		this.m_LB_Nope2.Visible = false;
		this.m_DT_Nope2BG.Visible = false;
		this.m_LB_Nope3.Visible = false;
		this.m_DT_Nope3BG.Visible = false;
		int itemCnt = NkUserInventory.GetInstance().GetItemCnt(itemEvolutionData.nNeedItem_Unique[0]);
		int num = itemEvolutionData.nNeedItem_num[0];
		if (num > itemCnt)
		{
			this.m_LB_Nope1.Visible = true;
			this.m_DT_Nope1BG.Visible = true;
			result = false;
		}
		long money = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money;
		num = itemEvolutionData.nNeedGold;
		if ((long)num > money)
		{
			this.m_LB_Nope2.Visible = true;
			this.m_DT_Nope2BG.Visible = true;
			result = false;
		}
		itemCnt = NkUserInventory.GetInstance().GetItemCnt(itemEvolutionData.nNeedItem_Unique[1]);
		num = itemEvolutionData.nNeedItem_num[1];
		if (num > itemCnt)
		{
			this.m_LB_Nope3.Visible = true;
			this.m_DT_Nope3BG.Visible = true;
			result = false;
		}
		return result;
	}

	public void EvolutionEffect(object a_oObject)
	{
		this.m_Button_confirm.SetEnabled(false);
		string str = string.Format("{0}", "effect/instant/fx_grade_evolution_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.SetActionItemSkill), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void SendEvolutionDataToServer()
	{
		GS_ITEMEVOLUTION_REQ gS_ITEMEVOLUTION_REQ = new GS_ITEMEVOLUTION_REQ();
		gS_ITEMEVOLUTION_REQ.SrcPosType = this.m_SelectItem.m_nPosType;
		gS_ITEMEVOLUTION_REQ.SrcItemPos = this.m_SelectItem.m_nItemPos;
		gS_ITEMEVOLUTION_REQ.SrcItemUnique = this.m_SelectItem.m_nItemUnique;
		gS_ITEMEVOLUTION_REQ.SolID = this.m_SelectItemSol;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMEVOLUTION_REQ, gS_ITEMEVOLUTION_REQ);
	}

	public void RefreshData()
	{
		this.InitNeedItem();
		if (this.m_showtype == ItemEvolution_Dlg.SHOWTYPE.SOLDER)
		{
			this.SolEquipItem();
		}
		else
		{
			this.SetInvItemData();
		}
	}

	public long GetItemSelectSolID()
	{
		return this.m_SelectItemSol;
	}

	public void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_showtype = (ItemEvolution_Dlg.SHOWTYPE)uIPanelTab.panel.index;
		if (this.m_showtype == ItemEvolution_Dlg.SHOWTYPE.SOLDER)
		{
			this.initSolShow();
		}
		else
		{
			this.initInvenShow();
		}
		this.ShowTypeBG();
	}

	public void OnItemClick(IUIObject obj)
	{
		this.InitNeedItem();
		if (this.m_showtype == ItemEvolution_Dlg.SHOWTYPE.ITEM || this.m_showtype == ItemEvolution_Dlg.SHOWTYPE.SOLITEM)
		{
			if (null == this.m_NLB_EquipmentList.SelectedItem)
			{
				return;
			}
			ITEM iTEM = (ITEM)this.m_NLB_EquipmentList.SelectedItem.Data;
			if (iTEM != null)
			{
				this.m_SelectItem = iTEM;
				this.m_SelectItemSol = this.m_SolID;
				this.SendItemEvolutionSet(this.m_SelectItem);
			}
		}
	}

	public void OnSolClick(IUIObject obj)
	{
		this.InitNeedItem();
		if (this.m_showtype == ItemEvolution_Dlg.SHOWTYPE.SOLDER)
		{
			NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)this.m_NLB_SolList.SelectedItem.Data;
			if (nkSoldierInfo != null)
			{
				this.m_SolID = nkSoldierInfo.GetSolID();
				this.m_SelectSol = nkSoldierInfo;
				this.SolEquipItem();
			}
		}
	}

	public void OnItemEvolutionConfirm(IUIObject obj)
	{
		if (this.m_SelectItem == null)
		{
			return;
		}
		ITEMEVOLUTION itemEvolutionData = NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(this.m_SelectItem.m_nItemUnique);
		if (itemEvolutionData == null)
		{
			return;
		}
		string empty = string.Empty;
		bool flag = true;
		ITEM firstItemByUnique = NkUserInventory.GetInstance().GetFirstItemByUnique(itemEvolutionData.nNeedItem_Unique[0]);
		if (firstItemByUnique == null || (firstItemByUnique != null && itemEvolutionData.nNeedItem_num[0] > firstItemByUnique.m_nItemNum))
		{
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemEvolutionData.nNeedItem_Unique[0]);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(itemInfo.m_strTextKey)
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			flag = false;
		}
		firstItemByUnique = NkUserInventory.GetInstance().GetFirstItemByUnique(itemEvolutionData.nNeedItem_Unique[1]);
		if (firstItemByUnique == null || (firstItemByUnique != null && itemEvolutionData.nNeedItem_num[1] > firstItemByUnique.m_nItemNum))
		{
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemEvolutionData.nNeedItem_Unique[1]);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(itemInfo.m_strTextKey)
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			flag = false;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if ((long)itemEvolutionData.nNeedGold > myCharInfo.m_Money)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676")
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			flag = false;
		}
		if (!flag)
		{
			this.m_Button_confirm.enabled = true;
			return;
		}
		string empty2 = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("370"),
			"count",
			itemEvolutionData.nItemSkillPenalty
		});
		NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3428"), empty2, eMsgType.MB_OK_CANCEL, new YesDelegate(this.EvolutionEffect), null);
	}

	public void OnItemInfo(IUIObject obj)
	{
		ITEM iTEM = new ITEM();
		iTEM.Set(this.m_SelectItem);
		iTEM.m_nItemNum = 1;
		iTEM.m_nItemUnique = NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(this.m_SelectItem.m_nItemUnique).nResult_Index;
		iTEM.m_nOption[4] = this.m_SelectItem.m_nOption[4];
		int num = this.m_SelectItem.m_nOption[5] - NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(this.m_SelectItem.m_nItemUnique).nItemSkillPenalty;
		if (1 > num)
		{
			num = 1;
		}
		iTEM.m_nOption[5] = num;
		if (iTEM != null && iTEM.IsValid())
		{
			Protocol_Item.Item_ShowItemInfo((G_ID)base.WindowID, iTEM, Vector3.zero, this.m_SelectItem, 0L);
		}
		else
		{
			Protocol_Item.Item_ShowItemInfo((G_ID)base.WindowID, this.m_SelectItem, Vector3.zero, null, 0L);
		}
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Gear_Evolve.ToString());
		}
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

	private void SetItemColum(ITEM itemdata, int pos, ref NewListItem item)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string empty = string.Empty;
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(itemdata);
		item.SetListItemData(1, itemdata, true, null, null);
		item.SetListItemData(2, rankColorName, null, null, null);
		int num = itemdata.m_nOption[5];
		int skillUnique = itemdata.m_nOption[4];
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1292"),
			"skillname",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
			"skilllevel",
			num
		});
		item.SetListItemData(3, empty, null, null, null);
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
		item.SetListItemData(4, stringBuilder.ToString(), null, null, null);
		item.Data = itemdata;
	}

	private void MakeSolListAndSort()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		this.m_SolList.Clear();
		this.m_SolSortList.Clear();
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
			this.m_SolList.Sort(new Comparison<NkSoldierInfo>(this.ComparePosIndex));
			break;
		case 2:
			this.m_SolList.Sort(new Comparison<NkSoldierInfo>(this.CompareName));
			break;
		case 3:
			this.m_SolList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevel));
			break;
		case 4:
			this.m_SolList.Sort(new Comparison<NkSoldierInfo>(this.CompareCombatPower));
			break;
		case 5:
			this.m_SolList.Sort(new Comparison<NkSoldierInfo>(this.ComparePosIndex));
			break;
		}
		if (this.m_nSearch_SolPosType == 100)
		{
			NkSoldierInfo leaderSoldierInfo = charPersonInfo.GetLeaderSoldierInfo();
			if (leaderSoldierInfo != null)
			{
				for (int i = 0; i < this.m_SolList.Count; i++)
				{
					if (leaderSoldierInfo.GetSolID() == this.m_SolList[i].GetSolID())
					{
						this.m_SolSortList.Add(this.m_SolList[i]);
						this.m_SolList.Remove(this.m_SolSortList[0]);
						break;
					}
				}
			}
		}
		for (int j = 0; j < this.m_SolList.Count; j++)
		{
			this.m_SolSortList.Add(this.m_SolList[j]);
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
		if (pkSolinfo.GetSolPosType() != (byte)eAddPosType)
		{
			return;
		}
		this.m_SolList.Add(pkSolinfo);
	}

	private void SetSolColum(int pos, ref NewListItem item)
	{
		if (this.m_SolSortList.Count <= pos)
		{
			return;
		}
		if (this.m_SolSortList[pos] == null)
		{
			TsLog.Log("m_SolSortList[pos] == null", new object[0]);
			return;
		}
		EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(this.m_SolSortList[pos].GetCharKind(), this.m_SolSortList[pos].GetGrade());
		if (eventHeroCharCode != null)
		{
			item.SetListItemData(0, "Win_I_EventSol", null, null, null);
			item.SetListItemData(1, this.m_SolSortList[pos].GetListSolInfo(false), true, null, null);
		}
		else
		{
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(this.m_SolSortList[pos].GetCharKind(), (int)this.m_SolSortList[pos].GetGrade());
			if (legendFrame != null)
			{
				item.SetListItemData(0, legendFrame, null, null, null);
			}
			else
			{
				item.SetListItemData(0, true);
			}
			item.SetListItemData(1, this.m_SolSortList[pos].GetListSolInfo(false), false, null, null);
		}
		item.SetListItemData(2, this.m_SolSortList[pos].GetName(), null, null, null);
		item.Data = this.m_SolSortList[pos];
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"count1",
			this.m_SolSortList[pos].GetLevel(),
			"count2",
			this.m_SolSortList[pos].GetSolMaxLevel()
		});
		item.SetListItemData(3, textFromInterface, null, null, null);
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

	private void On_NoItemList_Notification(IUIObject a_oObject)
	{
		string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("898");
		Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
	}

	private void On_NotNeedSkillLevel_Notification(IUIObject a_oObject)
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
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM == null)
					{
						return;
					}
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("897");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromNotify, new object[]
					{
						textFromNotify,
						"count",
						NrTSingleton<ITEMEVOLUTION_Manager>.Instance.GetItemEvolutionData(iTEM.m_nItemUnique).nItemSkill_Condition
					});
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
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
						this.m_SelectItemSol = this.m_SolID;
						this.SendItemEvolutionSet(this.m_SelectItem);
						for (int i = 0; i < this.SLOTNUM; i++)
						{
							if (i == imageSlot.Index)
							{
								this.m_DT_Check[i].Visible = true;
							}
							else
							{
								this.m_DT_Check[i].Visible = false;
							}
						}
						Vector3 localPosition = this.m_DT_Check[imageSlot.Index].gameObject.transform.localPosition;
						localPosition.z = -0.15f;
						this.m_DT_Check[imageSlot.Index].gameObject.transform.localPosition = localPosition;
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("557"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					}
				}
			}
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
				this.bLoadEvolutionEffect = true;
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
}
