using GAME;
using System;
using System.Collections.Generic;
using System.Text;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolEquipItemSelectDlg : Form
{
	public const int QUALITYLEVEL_MIN = 1001;

	private NewListBox m_nlbItem;

	private List<ITEM> m_ItemList = new List<ITEM>();

	private StringBuilder m_Text = new StringBuilder();

	private NkSoldierInfo pkSolinfo;

	private eEQUIP_ITEM eEquipItemPos;

	private UIButton m_MobileSelbutton;

	public UIButton MobileSelbutton
	{
		get
		{
			return this.m_MobileSelbutton;
		}
		set
		{
			this.m_MobileSelbutton = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Item/DLG_SolEquipItemSelect", G_ID.SOLEQUIPITEMSELECT_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_nlbItem = (base.GetControl("NewListBox_reduce1") as NewListBox);
		this.m_nlbItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemSelectConfirm));
		this.InitData();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ITEM-LIST", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void ShowInvenItemList()
	{
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg == null || !solMilitaryGroupDlg.Visible)
		{
			return;
		}
		this.m_ItemList.Clear();
		this.m_nlbItem.Clear();
		if (this.eEquipItemPos == eEQUIP_ITEM.EQUIP_WEAPON1 && this.pkSolinfo.GetCharKindInfo().IsATB(1L) && this.pkSolinfo.GetCharKindInfo().GetCharTribe() == 2)
		{
			for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
			{
				for (int j = 2; j < 4; j++)
				{
					ITEM item = NkUserInventory.GetInstance().GetItem(j, i);
					if (item != null && item.IsValid())
					{
						if (NrTSingleton<ItemManager>.Instance.GetEquipItemPos(item.m_nItemUnique) == this.eEquipItemPos)
						{
							if (Protocol_Item.Is_Item_Equipment(item, this.pkSolinfo, false))
							{
								this.m_ItemList.Add(item);
							}
						}
					}
				}
			}
		}
		else
		{
			int itemPosType;
			if (this.eEquipItemPos == eEQUIP_ITEM.EQUIP_WEAPON1)
			{
				itemPosType = this.pkSolinfo.GetItemPosTypeByWeaponType();
			}
			else
			{
				itemPosType = 1;
			}
			for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
			{
				ITEM item = NkUserInventory.GetInstance().GetItem(itemPosType, i);
				if (item != null && item.IsValid())
				{
					if (NrTSingleton<ItemManager>.Instance.GetEquipItemPos(item.m_nItemUnique) == this.eEquipItemPos)
					{
						if (Protocol_Item.Is_Item_Equipment(item, this.pkSolinfo, false))
						{
							this.m_ItemList.Add(item);
						}
					}
				}
			}
		}
		this.m_ItemList.Sort(new Comparison<ITEM>(this.CompareItemLevel));
		if (0 < this.m_ItemList.Count)
		{
			for (int i = 0; i < this.m_ItemList.Count; i++)
			{
				NewListItem item2 = new NewListItem(this.m_nlbItem.ColumnNum, true);
				this.SetItemColum(this.m_ItemList[i], i, ref item2);
				this.m_nlbItem.Add(item2);
			}
		}
		this.m_nlbItem.RepositionItems();
	}

	public void SetLocationByForm(Form pkTargetDlg)
	{
		float x = 100f;
		float y = 100f;
		if (pkTargetDlg != null)
		{
			x = pkTargetDlg.GetLocationX() + pkTargetDlg.GetSize().x / 2f - base.GetSize().x;
			y = pkTargetDlg.GetLocationY();
		}
		base.SetLocation(x, y);
	}

	public void Refresh()
	{
		this.ShowInvenItemList();
	}

	public void SetData(ref NkSoldierInfo solinfo, eEQUIP_ITEM equipitempos)
	{
		this.pkSolinfo = solinfo;
		this.eEquipItemPos = equipitempos;
		this.ShowInvenItemList();
	}

	private void SetItemColum(ITEM itemdata, int pos, ref NewListItem item)
	{
		this.m_Text.Remove(0, this.m_Text.Length);
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(itemdata);
		item.SetListItemData(1, itemdata, true, null, null);
		item.SetListItemData(2, rankColorName, null, null, null);
		if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(itemdata.m_nItemUnique) == eITEM_PART.ITEMPART_WEAPON)
		{
			int nValue = Protocol_Item.Get_Min_Damage(itemdata);
			int optionValue = Tooltip_Dlg.GetOptionValue(itemdata, nValue, 1);
			int nValue2 = Protocol_Item.Get_Max_Damage(itemdata);
			int optionValue2 = Tooltip_Dlg.GetOptionValue(itemdata, nValue2, 1);
			this.m_Text.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("242") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString(), " ~ ", optionValue2.ToString()));
		}
		else if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(itemdata.m_nItemUnique) == eITEM_PART.ITEMPART_ARMOR)
		{
			int nValue = Protocol_Item.Get_Defense(itemdata);
			int optionValue = Tooltip_Dlg.GetOptionValue(itemdata, nValue, 2);
			this.m_Text.Append(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("243") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString()));
		}
		item.SetListItemData(3, this.m_Text.ToString(), null, null, null);
		item.SetListItemData(4, string.Empty, itemdata, new EZValueChangedDelegate(this.OnClickItemView), null);
		item.Data = itemdata;
	}

	public void ItemAddEffectDelegate(IUIObject control, GameObject obj)
	{
		if (null == obj)
		{
			return;
		}
		obj.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
	}

	private void OnClickItemView(IUIObject obj)
	{
		if (TsPlatform.IsMobile)
		{
			UIButton uIButton = obj as UIButton;
			if (null == uIButton)
			{
				return;
			}
			this.m_MobileSelbutton = uIButton;
			ITEM iTEM = (ITEM)uIButton.data;
			ITEM equipItem = this.pkSolinfo.GetEquipItem((int)this.eEquipItemPos);
			if (iTEM != null)
			{
				if (equipItem != null && equipItem.IsValid())
				{
					Protocol_Item.Item_ShowItemInfo((G_ID)base.WindowID, iTEM, Vector3.zero, equipItem, 0L);
				}
				else
				{
					Protocol_Item.Item_ShowItemInfo((G_ID)base.WindowID, iTEM, Vector3.zero, null, 0L);
				}
			}
		}
	}

	public void OnClickItemSelectConfirm(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		if (this.m_nlbItem.SelectedItem == null)
		{
			this.CloseForm(null);
			return;
		}
		ITEM iTEM = this.m_nlbItem.SelectedItem.Data as ITEM;
		if (iTEM == null)
		{
			return;
		}
		if (this.pkSolinfo == null)
		{
			this.CloseForm(null);
			return;
		}
		Protocol_Item.Item_Use(iTEM, this.pkSolinfo.GetSolID());
		this.CloseForm(null);
	}

	private void OnClickItemRemove(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		ITEM equipItem = this.pkSolinfo.GetEquipItem((int)this.eEquipItemPos);
		if (equipItem == null)
		{
			return;
		}
		if (this.pkSolinfo == null)
		{
			this.CloseForm(null);
			return;
		}
		Protocol_Item.Send_EquipSol_InvenEquip(equipItem, this.pkSolinfo.GetSolID());
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ITEM-LIST", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
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
		return -itemInfo.GetUseMinLevel(a).CompareTo(itemInfo2.GetUseMinLevel(b));
	}
}
