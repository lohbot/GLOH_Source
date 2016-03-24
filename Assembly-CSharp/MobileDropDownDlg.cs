using System;
using UnityEngine;
using UnityForms;

public class MobileDropDownDlg : Form
{
	private ListBox m_kListBox;

	private DropDownList m_kParentDropDownList;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "System/DLG_MobileDropDown", G_ID.MOBILE_DROPDOWN_DLG, false);
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.AddValueChangedDelegate(new EZValueChangedDelegate(this.HideDropDown));
		}
	}

	public void HideDropDown(IUIObject obj)
	{
		if (null != this.m_kParentDropDownList)
		{
			this.m_kParentDropDownList.SetHideList();
		}
	}

	public override void SetComponent()
	{
		this.m_kListBox = (base.GetControl("big_list_listbox") as ListBox);
		this.m_kListBox.ColumnNum = 1;
		this.m_kListBox.UseColumnRect = true;
		this.m_kListBox.LineHeight = 80f;
		this.m_kListBox.SetColumnRect(0, new Rect(0f, 0f, 1226f, 80f), SpriteText.Anchor_Pos.Middle_Center, 45f);
		this.m_kListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickList));
		base.SetScreenCenter();
		base.SetLocation(base.GetLocation().x, base.GetLocationY() + 80f);
	}

	public void ClickList(IUIObject obj)
	{
		if (null == this.m_kParentDropDownList)
		{
			return;
		}
		UIListItemContainer selectedItem = this.m_kListBox.SelectedItem;
		if (null != selectedItem)
		{
			int selectIndex = this.m_kListBox.SelectIndex;
			this.m_kParentDropDownList.SetSelectedItem(selectIndex);
			if (null != this.m_kParentDropDownList.SelectedItem)
			{
				this.m_kParentDropDownList.DidClick(this.m_kParentDropDownList.SelectedItem);
			}
		}
		this.Close();
	}

	public void SetData(DropDownList dw)
	{
		if (null == dw)
		{
			return;
		}
		if (dw.GetParentCheck() && NrTSingleton<FormsManager>.Instance.IsForm(dw.ParentGID))
		{
			MsgBoxAutoSellUI msgBoxAutoSellUI = NrTSingleton<FormsManager>.Instance.GetForm(dw.ParentGID) as MsgBoxAutoSellUI;
			if (msgBoxAutoSellUI != null)
			{
				base.SetLocation(base.GetLocation().x, base.GetLocationY() + 80f, msgBoxAutoSellUI.GetLocation().z - 2f);
			}
		}
		this.m_kParentDropDownList = dw;
		this.m_kListBox.Clear();
		for (int i = 0; i < dw.Count; i++)
		{
			IUIListObject item = dw.GetItem(i);
			if (item != null)
			{
				ListItem listItem = (ListItem)item.Data;
				if (listItem != null)
				{
					ListItem listItem2 = new ListItem();
					listItem2.SetColumnStr(0, listItem.GetColumnStr(0));
					listItem2.Key = listItem2;
					this.m_kListBox.Add(listItem2);
				}
			}
		}
		this.m_kListBox.RepositionItems();
	}
}
