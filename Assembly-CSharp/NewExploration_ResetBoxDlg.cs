using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class NewExploration_ResetBoxDlg : Form
{
	private Button m_btOk;

	private Button m_btCancel;

	private Label m_lbVipLv;

	private DrawTexture m_dtVIPMark;

	private Button m_btVip;

	private Label m_lbResetCount;

	private Label m_lbGemCount;

	private byte m_i8VipLevel;

	private sbyte m_bResetCount;

	private sbyte m_bMaxResetCount;

	private NEWEXPLORATION_RESET_INFO m_ResetInfoData;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewExploration/DLG_ResetBox", G_ID.NEWEXPLORATION_RESETBOX_DLG, false, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btOk = (base.GetControl("Button_ok") as Button);
		Button expr_1C = this.m_btOk;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickOk));
		this.m_btCancel = (base.GetControl("Button_Cancel") as Button);
		Button expr_59 = this.m_btCancel;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.OnClickCancel));
		this.m_lbVipLv = (base.GetControl("LB_Lv") as Label);
		this.m_dtVIPMark = (base.GetControl("DT_VIPMark") as DrawTexture);
		this.m_btVip = (base.GetControl("Btn_VIP") as Button);
		Button expr_C2 = this.m_btVip;
		expr_C2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_C2.Click, new EZValueChangedDelegate(this.OnClickVip));
		this.m_lbResetCount = (base.GetControl("LB_Count") as Label);
		this.m_lbGemCount = (base.GetControl("LB_SoulGem") as Label);
		this.m_bResetCount = NrTSingleton<NewExplorationManager>.Instance.GetResetCount();
		this.m_i8VipLevel = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.VipLevel;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2790"),
			"level",
			this.m_i8VipLevel
		});
		this.m_lbVipLv.SetText(empty);
		this.m_dtVIPMark.SetTextureFromBundle(string.Format("UI/etc/{0}", NrTSingleton<NrVipSubInfoManager>.Instance.GetIconPath(this.m_i8VipLevel)));
		this.m_bMaxResetCount = NrTSingleton<NewExplorationManager>.Instance.GetMaxResetCount();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3482"),
			"count1",
			(int)this.m_bResetCount,
			"count2",
			this.m_bMaxResetCount
		});
		this.m_lbResetCount.SetText(empty);
		int num = 0;
		this.m_ResetInfoData = NrTSingleton<NewExplorationManager>.Instance.GetResetInfoData((sbyte)((int)this.m_bResetCount + 1));
		if (this.m_ResetInfoData != null)
		{
			num = this.m_ResetInfoData.i32ItemNum;
			this.m_btOk.enabled = false;
		}
		this.m_lbGemCount.SetText(num.ToString());
	}

	public void OnClickOk(IUIObject obj)
	{
		if (this.m_ResetInfoData == null || (int)this.m_bMaxResetCount <= (int)this.m_bResetCount)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("891"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		ITEM item = NkUserInventory.instance.GetItem(this.m_ResetInfoData.i32ItemUnique);
		if (item == null || item.m_nItemNum < this.m_ResetInfoData.i32ItemNum)
		{
			string empty = string.Empty;
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_ResetInfoData.i32ItemUnique);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1256"),
				"itemname",
				itemNameByItemUnique,
				"count",
				this.m_ResetInfoData.i32ItemNum
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() != eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_END)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("893"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (NrTSingleton<NewExplorationManager>.Instance.CanGetEndReward())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("892"));
			return;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWEXPLORATION_RESET_REQ, new GS_NEWEXPLORATION_RESET_REQ());
		this.Close();
	}

	public void OnClickCancel(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickVip(IUIObject obj)
	{
		VipInfoDlg vipInfoDlg = (VipInfoDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.VIPINFO_DLG);
		if (vipInfoDlg != null)
		{
			vipInfoDlg.SetLevel(this.m_i8VipLevel, false);
		}
	}
}
