using GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class LackGold_dlg : Form
{
	private Label m_lbText;

	private Button m_btnOK;

	private DrawTexture m_txNPCImg;

	private int m_ItemUnique;

	private eITEMMALL_TYPE m_RequestMallType;

	private bool m_bLackHearts;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Item/DLG_Enchant_lackgold", G_ID.GOLDLACK_DLG, true);
		base.DonotDepthChange(90f);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_lbText = (base.GetControl("Label_BubbleText") as Label);
		this.m_btnOK = (base.GetControl("Button_OK") as Button);
		this.m_btnOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Mouse_Click));
		this.m_txNPCImg = (base.GetControl("DrawTexture_NPCIMG") as DrawTexture);
		this.m_txNPCImg.SetTexture(eCharImageType.LARGE, 242, -1);
	}

	public void SetData(long _LackGold)
	{
		long num = 0L;
		int num2 = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
		List<ITEM_MALL_ITEM> group = NrTSingleton<ItemMallItemManager>.Instance.GetGroup(4);
		this.m_RequestMallType = eITEMMALL_TYPE.BUY_GOLD;
		string empty = string.Empty;
		if (group != null)
		{
			for (int i = 0; i < group.Count; i++)
			{
				if (group[i].m_nGetMoney > _LackGold)
				{
					num = group[i].m_nPrice;
					break;
				}
			}
			if (num > (long)num2)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("170"),
					"gold",
					_LackGold
				});
				this.m_bLackHearts = true;
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("169"),
					"gold",
					_LackGold
				});
				this.m_bLackHearts = false;
			}
			this.m_lbText.SetText(empty);
		}
	}

	public bool SetDataShopItem(int ItemUnique, eITEMMALL_TYPE _Type = eITEMMALL_TYPE.NONE)
	{
		if (ItemUnique > 0 && _Type == eITEMMALL_TYPE.NONE)
		{
			ITEM_MALL_ITEM itemData = NrTSingleton<ItemMallItemManager>.Instance.GetItemData(ItemUnique);
			if (itemData == null || !NrTSingleton<ContentsLimitManager>.Instance.IsShopProduct(itemData.m_Idx))
			{
				this.Close();
				return false;
			}
		}
		this.m_RequestMallType = _Type;
		this.m_ItemUnique = ItemUnique;
		string empty = string.Empty;
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(ItemUnique);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("226"),
			"itemname",
			itemNameByItemUnique,
			"itemname",
			itemNameByItemUnique
		});
		this.m_lbText.SetText(empty);
		return true;
	}

	private void On_Mouse_Click(IUIObject a_oObject)
	{
		eITEMMALL_TYPE eItemMallType = eITEMMALL_TYPE.BUY_HEARTS;
		if (!this.m_bLackHearts)
		{
			eItemMallType = eITEMMALL_TYPE.BUY_GOLD;
		}
		if (this.m_ItemUnique > 0 && this.m_RequestMallType == eITEMMALL_TYPE.NONE)
		{
			ITEM_MALL_ITEM itemData = NrTSingleton<ItemMallItemManager>.Instance.GetItemData(this.m_ItemUnique);
			if (itemData != null)
			{
				eItemMallType = (eITEMMALL_TYPE)itemData.m_nGroup;
			}
		}
		else
		{
			eItemMallType = this.m_RequestMallType;
		}
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eItemMallType, true);
		this.Close();
	}
}
