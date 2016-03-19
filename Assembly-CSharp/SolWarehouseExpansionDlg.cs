using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class SolWarehouseExpansionDlg : Form
{
	private Label m_lbText;

	private Label m_lbCost;

	private Label m_lbHearts;

	private Button m_btExpansion;

	private Button m_btCancel;

	private string m_strText = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolWarehouseExpansion", G_ID.SOL_WAREHOUSE_EXPANSION_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_lbText = (base.GetControl("Label_select_text") as Label);
		this.m_lbCost = (base.GetControl("Label_Cost") as Label);
		this.m_lbHearts = (base.GetControl("Label_money") as Label);
		this.m_btExpansion = (base.GetControl("Button_OK") as Button);
		this.m_btExpansion.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickExpansion));
		this.m_btCancel = (base.GetControl("Button_Cancel") as Button);
		this.m_btCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCancel));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClickExpansion(IUIObject obj)
	{
		long num = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_SOL_WAREHOUSE_MAX);
		num += 1L;
		SolWarehouseInfo solWarehouseInfo = NrTSingleton<NrBaseTableManager>.Instance.GetSolWarehouseInfo(num.ToString());
		if (solWarehouseInfo == null)
		{
			return;
		}
		if (solWarehouseInfo.iNeedHeartsNum > NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("273"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		GS_SOLDIER_WAREHOUSE_ADD_REQ gS_SOLDIER_WAREHOUSE_ADD_REQ = new GS_SOLDIER_WAREHOUSE_ADD_REQ();
		gS_SOLDIER_WAREHOUSE_ADD_REQ.i8UseMoney = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_WAREHOUSE_ADD_REQ, gS_SOLDIER_WAREHOUSE_ADD_REQ);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOL_WAREHOUSE_EXPANSION_DLG);
	}

	public void ClickCancel(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOL_WAREHOUSE_EXPANSION_DLG);
	}

	public void SetSolWarehouseInfo()
	{
		long num = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_SOL_WAREHOUSE_MAX);
		num += 1L;
		SolWarehouseInfo solWarehouseInfo = NrTSingleton<NrBaseTableManager>.Instance.GetSolWarehouseInfo(num.ToString());
		if (solWarehouseInfo == null)
		{
			return;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2182"),
			"count",
			num
		});
		this.m_lbText.SetText(this.m_strText);
		this.m_lbCost.SetText(ANNUALIZED.Convert(solWarehouseInfo.iNeedHeartsNum));
		this.m_lbHearts.SetText(ANNUALIZED.Convert(NkUserInventory.GetInstance().Get_First_ItemCnt(70000)));
	}
}
