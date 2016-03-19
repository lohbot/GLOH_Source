using GAME;
using System;
using UnityEngine;
using UnityForms;

public class Tooltip_Second_Dlg : Form
{
	private ITEM m_cItem;

	public G_ID m_eParentWindowID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Tooltip/DLG_tootip", G_ID.TOOLTIP_SECOND_DLG, true);
	}

	public override void Update()
	{
		base.Update();
		if (this.m_eParentWindowID != G_ID.NONE && !NrTSingleton<FormsManager>.Instance.IsShow(this.m_eParentWindowID))
		{
			this.Close();
		}
	}

	public override void Hide_End()
	{
		this.m_eParentWindowID = G_ID.NONE;
		this.m_cItem = null;
	}

	public void Set_Tooltip(G_ID a_eWidowID, ITEM a_cItem)
	{
		this.m_eParentWindowID = a_eWidowID;
		this.m_cItem = a_cItem;
		Tooltip_Dlg.Item_Tooltip(this, this.m_cItem, this.m_eParentWindowID);
		Form cForm = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TOOLTIP_DLG);
		Tooltip_Dlg.Tooltip_Rect(cForm, Vector3.zero);
		this.Show();
	}

	public void Set_Tooltip(G_ID a_eWidowID, ITEM a_cItem, bool bEquiped, Vector3 showPosition)
	{
		this.m_eParentWindowID = a_eWidowID;
		this.m_cItem = a_cItem;
		Tooltip_Dlg.Item_Tooltip(this, this.m_cItem, this.m_eParentWindowID, bEquiped);
		Form cForm = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TOOLTIP_DLG);
		Tooltip_Dlg.Tooltip_Rect(cForm, showPosition);
		this.Show();
	}

	public void Set_TooltipForEquip(G_ID eWidowID, ITEM pkEquipedItem, ITEM pkItem, bool bEquiped)
	{
		this.m_eParentWindowID = eWidowID;
		this.m_cItem = pkItem;
		Tooltip_Dlg.Item_Tooltip(this, pkEquipedItem, this.m_cItem, this.m_eParentWindowID, bEquiped);
		Form cForm = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TOOLTIP_DLG);
		Tooltip_Dlg.Tooltip_Rect(cForm, Vector3.zero);
		this.Show();
	}
}
