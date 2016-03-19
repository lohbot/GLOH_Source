using GAME;
using System;
using UnityForms;

public class ItemTooltip_Btn_Dlg : Form
{
	private Button m_bEquip;

	private Button m_bClose;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/tooltip_btn_dlg", G_ID.ITEMTOOLTIP_BUTTON, true);
		base.SetLocation(base.GetLocation().x, base.GetLocation().y, 90f);
	}

	public override void SetComponent()
	{
		this.m_bEquip = (base.GetControl("Button_Button1") as Button);
		this.m_bEquip.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickEquipItem));
		this.m_bClose = (base.GetControl("Button_Button2") as Button);
		this.m_bClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickClose));
		ItemTooltipDlg_Second itemTooltipDlg_Second = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMTOOLTIP_SECOND_DLG) as ItemTooltipDlg_Second;
		TsLog.LogWarning("ItemTooltipDlg_Second GetLocation() = {0}", new object[]
		{
			itemTooltipDlg_Second.GetLocation()
		});
		base.SetLocation(GUICamera.width - base.GetSizeX(), GUICamera.height - base.GetSizeY(), itemTooltipDlg_Second.GetLocation().z - 2f);
		TsLog.LogWarning("This GetLocation() = {0}", new object[]
		{
			base.GetLocation()
		});
		base.InteractivePanel.twinFormID = G_ID.ITEMTOOLTIP_SECOND_DLG;
	}

	public void OnClickEquipItem(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLEQUIPITEMSELECT_DLG))
		{
			SolEquipItemSelectDlg solEquipItemSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLEQUIPITEMSELECT_DLG) as SolEquipItemSelectDlg;
			ITEM iTEM = solEquipItemSelectDlg.MobileSelbutton.data as ITEM;
			if (iTEM == null)
			{
				return;
			}
			long solid = 0L;
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null && solMilitaryGroupDlg.Visible)
			{
				NkSoldierInfo selectedSolinfo = solMilitaryGroupDlg.GetSelectedSolinfo();
				solid = selectedSolinfo.GetSolID();
			}
			Protocol_Item.Item_Use(iTEM, solid);
			solEquipItemSelectDlg.CloseForm(null);
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLSELECT_DLG))
		{
			ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
			ItemTooltipDlg_Second itemTooltipDlg_Second = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMTOOLTIP_SECOND_DLG) as ItemTooltipDlg_Second;
			if (itemTooltipDlg != null && itemTooltipDlg_Second != null)
			{
				Protocol_Item.Item_Use(itemTooltipDlg.Item, itemTooltipDlg.SolID);
				SoldierSelectDlg soldierSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLSELECT_DLG) as SoldierSelectDlg;
				if (soldierSelectDlg != null)
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLSELECT_DLG);
				}
			}
		}
		this.Close();
	}

	public override void OnClose()
	{
	}

	public void OnClickClose(IUIObject obj)
	{
		this.Close();
	}
}
