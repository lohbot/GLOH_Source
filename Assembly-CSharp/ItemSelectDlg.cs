using GAME;
using System;
using System.Text;
using UnityForms;

public class ItemSelectDlg : Form
{
	private Label m_lbSolname;

	private Button m_btnConfirm;

	private ListBox m_ListBox;

	private Label m_lbText;

	private Button m_btClose;

	private ITEM m_SelectItem;

	private long m_SolID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Reforge/DLG_ReforgeSelectItem", G_ID.REFORGESELECTITEM_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_lbSolname = (base.GetControl("Label_solname") as Label);
		this.m_lbText = (base.GetControl("Label_text") as Label);
		this.m_btnConfirm = (base.GetControl("Button_confirm") as Button);
		this.m_btnConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemConfirm));
		this.m_btnConfirm.Visible = false;
		this.m_ListBox = (base.GetControl("ListBox_ListBox1") as ListBox);
		this.m_ListBox.LineHeight = 84f;
		this.m_ListBox.UseColumnRect = true;
		this.m_ListBox.SelectStyle = "Win_B_ListBtn02";
		this.m_ListBox.ColumnNum = 4;
		this.m_ListBox.SetColumnRect(0, 12, 12, 60, 60);
		this.m_ListBox.SetColumnRect(1, 94, 10, 260, 25, SpriteText.Anchor_Pos.Middle_Left, 26f, false);
		this.m_ListBox.SetColumnRect(2, 94, 48, 140, 25, SpriteText.Anchor_Pos.Middle_Left, 26f, false);
		this.m_ListBox.SetColumnRect(3, 234, 48, 180, 25, SpriteText.Anchor_Pos.Middle_Right, 26f, false);
		this.m_ListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemClick));
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
	}

	public void SetData(long SolID)
	{
		int num = 0;
		this.m_lbSolname.Text = string.Empty;
		this.m_ListBox.Clear();
		this.m_SolID = SolID;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(this.m_SolID);
		if (soldierInfoFromSolID != null)
		{
			this.m_lbSolname.Text = soldierInfoFromSolID.GetName();
			for (int i = 0; i < 6; i++)
			{
				ITEM item = soldierInfoFromSolID.GetEquipItemInfo().m_kItem[i].GetItem();
				if (item != null)
				{
					if (item.m_nItemUnique > 0)
					{
						if (NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique) != null)
						{
							ListItem item2 = new ListItem();
							this.SetItemColum(item, num++, ref item2);
							this.m_ListBox.Add(item2);
						}
					}
				}
			}
			this.m_ListBox.RepositionItems();
		}
		if (num == 0)
		{
			this.m_lbText.Visible = true;
		}
		else
		{
			this.m_lbText.Visible = false;
		}
	}

	public void OnItemClick(IUIObject obj)
	{
		ITEM iTEM = (ITEM)this.m_ListBox.SelectedItem.Data;
		if (iTEM != null)
		{
			this.m_SelectItem = iTEM;
			ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
			reforgeMainDlg.Set_Value(this.m_SelectItem);
			reforgeMainDlg.SetSolID(this.m_SolID);
			this.Close();
		}
	}

	private void SetItemColum(ITEM itemdata, int pos, ref ListItem item)
	{
		StringBuilder stringBuilder = new StringBuilder();
		item.SetColumnGUIContent(0, itemdata, true);
		item.SetColumnStr(1, NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemdata));
		int rank = itemdata.m_nOption[2];
		item.SetColumnStr(2, ItemManager.RankTextColor(rank) + ItemManager.RankText(rank));
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
		item.SetColumnStr(3, stringBuilder.ToString());
		item.Key = itemdata;
	}

	public void OnItemConfirm(IUIObject obj)
	{
		if (this.m_SelectItem != null)
		{
			ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
			reforgeMainDlg.Set_Value(this.m_SelectItem);
			reforgeMainDlg.SetSolID(this.m_SolID);
			this.Close();
		}
		else
		{
			TsLog.Log("m_SelectItem == null", new object[0]);
		}
	}
}
