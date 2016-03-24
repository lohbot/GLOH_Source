using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Item_Box_Random_Result_Dlg : Form
{
	private struct BEST_ITEM
	{
		public int nItemUnique;

		public int nItemNum;
	}

	private const int ITEM_BOX_RARE = 1000;

	private const int EFFECT_COLUMDATA_POS = 2;

	private Button m_btSkip;

	private Label m_lbTitle;

	private DrawTexture m_dxTitleLine;

	private DrawTexture m_dxMainBG;

	private DrawTexture m_dxMiddleLine;

	private FlashLabel m_flCloseText;

	private NewListBox m_ShowList1;

	private NewListBox m_ShowList2;

	private NewListBox m_ShowList3;

	private List<Item_Box_Random_Result_Dlg.BEST_ITEM> m_BestItemUnique = new List<Item_Box_Random_Result_Dlg.BEST_ITEM>();

	private int[] m_nItemUnique = new int[12];

	private int[] m_nItemNum = new int[12];

	private List<ITEM> m_GetItems = new List<ITEM>();

	private int m_nIndex;

	private int m_nBoxUnique;

	private float showTime = 0.5f;

	private bool m_bEnableShow = true;

	private string strDownloadFilePath = string.Format("{0}", "Effect/Instant/fx_skill_active01");

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/DLG_Itembox_Random_result", G_ID.ITEM_BOX_RANDOM_RESULT, true);
		base.AlwaysUpdate = true;
	}

	public override void SetComponent()
	{
		this.m_dxMainBG = (base.GetControl("DrawTexture_MainBG") as DrawTexture);
		this.m_dxMainBG.SetSize(GUICamera.width, GUICamera.height);
		this.m_dxMainBG.SetTextureFromBundle("ui/item/bg_solcompose2");
		this.m_lbTitle = (base.GetControl("Label_Title") as Label);
		this.m_dxTitleLine = (base.GetControl("DrawTexture_TitleLine") as DrawTexture);
		this.m_btSkip = (base.GetControl("Button_ButtonSkip") as Button);
		this.m_btSkip.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Button));
		this.m_flCloseText = (base.GetControl("FlashLabel_CloseText") as FlashLabel);
		this.m_flCloseText.Visible = false;
		this.m_ShowList1 = (base.GetControl("NewListBox_ItemResult1") as NewListBox);
		this.m_ShowList1.touchScroll = false;
		this.m_ShowList1.SelectStyle = "Com_B_Transparent";
		this.m_ShowList1.clipContents = false;
		this.m_ShowList2 = (base.GetControl("NewListBox_ItemResult2") as NewListBox);
		this.m_ShowList2.touchScroll = false;
		this.m_ShowList2.SelectStyle = "Com_B_Transparent";
		this.m_ShowList2.clipContents = false;
		this.m_ShowList3 = (base.GetControl("NewListBox_ItemResult3") as NewListBox);
		this.m_ShowList3.touchScroll = false;
		this.m_ShowList3.SelectStyle = "Com_B_Transparent";
		this.m_ShowList3.clipContents = false;
		this.m_dxMiddleLine = (base.GetControl("DrawTexture_MiddleLine") as DrawTexture);
		this.SetPositon();
	}

	private void SetPositon()
	{
		float num = GUICamera.width / 2f;
		float num2 = GUICamera.height / 2f;
		this.m_lbTitle.SetLocation(0f, num2 - 360f);
		this.m_lbTitle.SetSize(GUICamera.width, this.m_lbTitle.GetSize().y);
		this.m_dxTitleLine.SetLocation(num - 575f, num2 - 310f);
		this.m_dxMiddleLine.SetLocation(num - 640f, num2 - 305f);
		this.m_dxMiddleLine.SetSize(GUICamera.width, this.m_dxMiddleLine.GetSize().y);
		this.m_ShowList1.SetLocation(num - 624f, num2 - 290f);
		this.m_ShowList2.SetLocation(num - 624f, num2 - 85f);
		this.m_ShowList3.SetLocation(num - 624f, num2 + 120f);
		this.m_flCloseText.SetLocation(num - 640f, GUICamera.height - this.m_flCloseText.height);
		this.m_btSkip.SetSize(GUICamera.width, GUICamera.height);
		this.m_btSkip.SetLocation(0f, 0f, this.m_btSkip.GetLocation().z - 0.5f);
		base.SetSize(GUICamera.width, GUICamera.height);
	}

	public void SetData(GS_BOX_USE_ACK ACK)
	{
		for (int i = 0; i < 12; i++)
		{
			this.m_nItemUnique[i] = ACK.m_nGetItemUnique[i];
			this.m_nItemNum[i] = ACK.m_nGetItemNum[i];
			if (ACK.m_caAddItem[i].m_nItemUnique > 0)
			{
				this.m_GetItems.Add(ACK.m_caAddItem[i]);
			}
		}
		this.m_nBoxUnique = ACK.m_lUnique;
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_nBoxUnique);
		if (itemInfo.IsItemATB(65536L))
		{
			ITEM_BOX_GROUP boxGroup = NrTSingleton<ItemManager>.Instance.GetBoxGroup(this.m_nBoxUnique);
			if (boxGroup != null)
			{
				for (int j = 0; j < 12; j++)
				{
					if (boxGroup.i32GroupItemUnique[j] > 0 && boxGroup.i32GroupItemRate[j] <= 1000)
					{
						Item_Box_Random_Result_Dlg.BEST_ITEM item = default(Item_Box_Random_Result_Dlg.BEST_ITEM);
						item.nItemUnique = boxGroup.i32GroupItemUnique[j];
						item.nItemNum = boxGroup.i32GroupItemNum[j];
						this.m_BestItemUnique.Add(item);
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < 12; k++)
			{
				if (itemInfo.m_nBoxItemProbability[k] > 0 && itemInfo.m_nBoxItemProbability[k] <= 1000)
				{
					Item_Box_Random_Result_Dlg.BEST_ITEM item2 = default(Item_Box_Random_Result_Dlg.BEST_ITEM);
					item2.nItemUnique = itemInfo.m_nBoxItemUnique[k];
					item2.nItemNum = itemInfo.m_nBoxItemNumber[k];
					this.m_BestItemUnique.Add(item2);
				}
			}
		}
		this.m_lbTitle.SetText(NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_nBoxUnique));
		this.SetContent();
	}

	private bool SetContent()
	{
		if (this.m_nItemUnique.Length <= this.m_nIndex || this.m_nItemUnique[this.m_nIndex] <= 0)
		{
			this.m_bEnableShow = false;
			this.m_flCloseText.Visible = true;
			return false;
		}
		this.showTime = Time.realtimeSinceStartup + 0.1f;
		if (this.m_nItemUnique[this.m_nIndex] > 0)
		{
			NewListItem newListItem = new NewListItem(this.m_ShowList1.ColumnNum, true, string.Empty);
			string empty = string.Empty;
			int num = this.m_nItemUnique[this.m_nIndex];
			int num2 = this.m_nItemNum[this.m_nIndex];
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1803"),
				"itemname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(num)
			});
			newListItem.SetListItemData(8, empty, null, null, null);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("572"),
				"Count",
				this.m_nItemNum[this.m_nIndex]
			});
			newListItem.SetListItemData(9, empty, null, null, null);
			ITEM iTEM = this.GetItem(num, num2);
			if (iTEM == null)
			{
				iTEM = new ITEM();
				iTEM.m_nItemUnique = num;
				iTEM.m_nItemNum = num2;
			}
			newListItem.SetListItemData(5, iTEM, null, null, null);
			bool flag = false;
			if (this.CanPile(num) && this.IsBestItem(num, num2))
			{
				flag = true;
			}
			if (flag)
			{
				newListItem.SetListItemData(2, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("WIN_T_Slot_Best_Item"), null, null, null);
				newListItem.SetListItemData2(2, this.strDownloadFilePath);
			}
			if (this.m_nIndex < 4)
			{
				this.m_ShowList1.Add(newListItem);
				this.m_ShowList1.RepositionItems();
			}
			else if (this.m_nIndex < 8)
			{
				this.m_ShowList2.Add(newListItem);
				this.m_ShowList2.RepositionItems();
			}
			else
			{
				this.m_ShowList3.Add(newListItem);
				this.m_ShowList3.RepositionItems();
			}
			this.m_GetItems.Remove(iTEM);
			this.m_nIndex++;
		}
		return true;
	}

	private void On_Button(IUIObject obj)
	{
		if (!this.SetContent())
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_RANDOM_RESULT);
		}
	}

	public override void Update()
	{
		if (this.m_bEnableShow && this.showTime < Time.realtimeSinceStartup)
		{
			this.SetContent();
		}
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	private bool IsBestItem(int ItemUnique, int Itemnum)
	{
		foreach (Item_Box_Random_Result_Dlg.BEST_ITEM current in this.m_BestItemUnique)
		{
			if (current.nItemUnique == ItemUnique && current.nItemNum == Itemnum)
			{
				return true;
			}
		}
		return false;
	}

	private ITEM GetItem(int ItemUniquem, int ItemNum)
	{
		foreach (ITEM current in this.m_GetItems)
		{
			if (current.m_nItemUnique == ItemUniquem && current.m_nItemNum == ItemNum)
			{
				return current;
			}
		}
		return null;
	}

	private bool CanPile(int ItemUniquem)
	{
		switch (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(ItemUniquem))
		{
		case eITEM_PART.ITEMPART_WEAPON:
		case eITEM_PART.ITEMPART_ARMOR:
		case eITEM_PART.ITEMPART_ACCESSORY:
			return false;
		default:
			return true;
		}
	}
}
