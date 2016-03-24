using GAME;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityForms;

public class ItemSetTooltip_Dlg : Form
{
	private FlashLabel m_lbSetItemInfo;

	private Label m_lbSetItemName;

	private int m_ParentWinID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = false;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/item_set_tooltip", G_ID.SETITEMTOOLTIP_DLG, true);
		form.Draggable = false;
		if (null != base.InteractivePanel)
		{
			base.SetLocation(base.GetLocation().x, base.GetLocation().y, 90f);
		}
		base.AlwaysUpdate = true;
	}

	public override void SetComponent()
	{
		this.m_lbSetItemInfo = (base.GetControl("FLASHLABEL_SET") as FlashLabel);
		this.m_lbSetItemName = (base.GetControl("LB_SETNAME") as Label);
	}

	public void SetData(ITEM pkItem, NkSoldierInfo pSolInfo, int WinID)
	{
		this.m_ParentWinID = WinID;
		Debug.Log("ParentWinID :" + this.m_ParentWinID);
		Dictionary<int, ITEM_SET> dictionary = null;
		int num = 0;
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(pkItem.m_nItemUnique);
		if (itemInfo == null)
		{
			return;
		}
		if (itemInfo.m_nSetUnique == 0)
		{
			return;
		}
		if (pSolInfo != null)
		{
			for (int i = 0; i < 6; i++)
			{
				ITEM equipItem = pSolInfo.GetEquipItem(i);
				if (equipItem != null && equipItem.m_nItemID > 0L)
				{
					if (equipItem.m_nDurability > 0)
					{
						ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(equipItem.m_nItemUnique);
						if (itemInfo2 != null)
						{
							if (itemInfo2.m_nSetUnique != 0)
							{
								if (dictionary == null)
								{
									dictionary = new Dictionary<int, ITEM_SET>();
								}
								if (dictionary.ContainsKey(itemInfo2.m_nSetUnique))
								{
									ITEM_SET value = dictionary[itemInfo2.m_nSetUnique];
									value.m_nSetCount++;
									dictionary.Remove(itemInfo2.m_nSetUnique);
									dictionary.Add(value.m_SetUnique, value);
								}
								else
								{
									ITEM_SET value2 = default(ITEM_SET);
									value2.m_SetUnique = itemInfo2.m_nSetUnique;
									value2.m_nSetCount = 0;
									dictionary.Add(value2.m_SetUnique, value2);
								}
							}
						}
					}
				}
			}
			if (dictionary != null && dictionary.ContainsKey(itemInfo.m_nSetUnique))
			{
				num = dictionary[itemInfo.m_nSetUnique].m_nSetCount;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		string text = string.Empty;
		for (int j = 0; j < 6; j++)
		{
			text = NrTSingleton<NrSetItemDataManager>.Instance.GetString(itemInfo.m_nSetUnique, j);
			int value3 = NrTSingleton<NrSetItemDataManager>.Instance.GetValue(itemInfo.m_nSetUnique, j);
			if (!string.IsNullOrEmpty(text))
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					text,
					"SETNUM",
					j + 1,
					"VALUE",
					value3
				});
				if (j <= num)
				{
					stringBuilder.Append(NrTSingleton<CTextParser>.Instance.GetTextColor("2002"));
				}
				stringBuilder.Append(text);
				stringBuilder.Append(NrTSingleton<CTextParser>.Instance.GetTextColor("1002"));
				stringBuilder.Append("\n");
			}
		}
		this.m_lbSetItemInfo.SetFlashLabel(stringBuilder.ToString());
		this.m_lbSetItemName.SetText(NrTSingleton<NrSetItemDataManager>.Instance.GetSetItemName(itemInfo.m_nSetUnique));
		base.SetSize(base.GetSizeX(), this.m_lbSetItemInfo.GetLocationY() + this.m_lbSetItemInfo.Height);
	}

	public override void Update()
	{
		base.Update();
		if ((NkInputManager.GetMouseButtonDown(0) || NkInputManager.GetMouseButtonDown(1)) && NrTSingleton<FormsManager>.Instance.FocusFormID() == this.m_ParentWinID && TsPlatform.IsMobile)
		{
			Form form = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_ParentWinID);
			if (form != null)
			{
				if (form is ItemTooltipDlg)
				{
					ItemTooltipDlg itemTooltipDlg = form as ItemTooltipDlg;
					if (itemTooltipDlg != null)
					{
						itemTooltipDlg.CloseSetItemTooltip();
					}
				}
				if (form is ItemTooltipDlg_Second)
				{
					ItemTooltipDlg_Second itemTooltipDlg_Second = form as ItemTooltipDlg_Second;
					if (itemTooltipDlg_Second != null)
					{
						itemTooltipDlg_Second.CloseSetItemTooltip();
					}
				}
				if (form is Item_Box_Select_Dlg)
				{
					Item_Box_Select_Dlg item_Box_Select_Dlg = form as Item_Box_Select_Dlg;
					if (item_Box_Select_Dlg != null)
					{
						item_Box_Select_Dlg.CloseSetItemTooltip();
					}
				}
			}
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		Debug.Log("Test!!!");
		Debug.Log("ParentWinID :" + this.m_ParentWinID);
	}
}
