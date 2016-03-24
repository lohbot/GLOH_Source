using System;
using UnityForms;

public class CTestDlg : Form
{
	private NewListBox test1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "test/test", G_ID.TEST_DLG, true);
	}

	public override void SetComponent()
	{
		this.test1 = (base.GetControl("NLB_InteriorInfo") as NewListBox);
		this.test1.Clear();
		NewListItem newListItem = new NewListItem(this.test1.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(0, "test11234", null, null, null);
		newListItem.SetListItemData(1, "Main_B_Territory", null, null, null);
		newListItem.SetListItemData(2, NkUserInventory.instance.GetItem(1, 0), null, null, null);
		newListItem.SetListItemData(4, NrTSingleton<ItemManager>.Instance.GetItemTexture(70000), null, null, null);
		newListItem.SetListItemData(5, false);
		this.test1.Add(newListItem);
		this.test1.RepositionItems();
	}
}
