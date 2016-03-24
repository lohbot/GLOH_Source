using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class DailyDungeon_MsgBox_Dlg : Form
{
	private Label m_lbMySoulGem;

	private Label m_lbLimitCount;

	private Label m_lbUseSoulGem;

	private Button m_btShopOpen1;

	private Button m_btShopOpen2;

	private Button m_btCancel;

	private Button m_btOk;

	private int m_SoulGemsCount;

	private sbyte m_DayOfWeek = -1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "DailyDungeon/DLG_DailyMsgBox", G_ID.DAILYDUNGEON_MSGBOX, false);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.draggable = false;
		}
	}

	public override void SetComponent()
	{
		this.m_lbMySoulGem = (base.GetControl("LB_MySoulGem") as Label);
		this.m_lbLimitCount = (base.GetControl("LB_Count") as Label);
		this.m_lbUseSoulGem = (base.GetControl("LB_UseSoulGem") as Label);
		this.m_btShopOpen1 = (base.GetControl("Btn_ShopSoulGem1") as Button);
		Button expr_5E = this.m_btShopOpen1;
		expr_5E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_5E.Click, new EZValueChangedDelegate(this.ClickShopOpen));
		this.m_btShopOpen2 = (base.GetControl("Btn_ShopSoulGem2") as Button);
		Button expr_9B = this.m_btShopOpen2;
		expr_9B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_9B.Click, new EZValueChangedDelegate(this.ClickShopOpen));
		this.m_btCancel = (base.GetControl("Button_cancel") as Button);
		Button expr_D8 = this.m_btCancel;
		expr_D8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_D8.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_btOk = (base.GetControl("Button_ok") as Button);
		Button expr_115 = this.m_btOk;
		expr_115.Click = (EZValueChangedDelegate)Delegate.Combine(expr_115.Click, new EZValueChangedDelegate(this.OnClickOk));
		base.SetScreenCenter();
		this.SetData(0);
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public void OnClickClose(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickOk(IUIObject obj)
	{
		sbyte b = (sbyte)NrTSingleton<DailyDungeonManager>.Instance.GetCurrWeekofDay();
		DAILYDUNGEON_INFO dAILYDUNGEON_INFO = null;
		if ((int)b == 0 || (int)b == 6)
		{
			Dictionary<int, DAILYDUNGEON_INFO> totalDailyDungeonInfo = NrTSingleton<DailyDungeonManager>.Instance.GetTotalDailyDungeonInfo();
			if (totalDailyDungeonInfo == null)
			{
				this.Close();
			}
			foreach (DAILYDUNGEON_INFO current in totalDailyDungeonInfo.Values)
			{
				if (current.m_i32DayOfWeek != 0)
				{
					if ((int)current.m_i8IsReward == 1)
					{
						dAILYDUNGEON_INFO = current;
					}
				}
			}
		}
		else
		{
			dAILYDUNGEON_INFO = NrTSingleton<DailyDungeonManager>.Instance.GetDailyDungeonInfo((int)this.m_DayOfWeek);
		}
		if (dAILYDUNGEON_INFO == null)
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
		byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
		byte dailyDungeonDcByVipLevel = NrTSingleton<NrTableVipManager>.Instance.GetDailyDungeonDcByVipLevel(levelExp);
		this.m_SoulGemsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70002);
		long num = this.MaxResetCount();
		long resetCount = NrTSingleton<DailyDungeonManager>.Instance.GetResetCount();
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_DAILYDUNGEON_COST_PLUS);
		long num2 = (long)(value - (int)dailyDungeonDcByVipLevel);
		if (num <= resetCount)
		{
			Main_UI_SystemMessage.ADDMessage("�ʱ�ȭ Ƚ���� �̹� �ִ�ġ", SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (num2 >= (long)this.m_SoulGemsCount)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("910"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		GS_CHARACTER_DAILYDUNGEON_SET_REQ gS_CHARACTER_DAILYDUNGEON_SET_REQ = new GS_CHARACTER_DAILYDUNGEON_SET_REQ();
		gS_CHARACTER_DAILYDUNGEON_SET_REQ.i8Diff = dAILYDUNGEON_INFO.m_i8Diff;
		gS_CHARACTER_DAILYDUNGEON_SET_REQ.i32DayOfWeek = dAILYDUNGEON_INFO.m_i32DayOfWeek;
		gS_CHARACTER_DAILYDUNGEON_SET_REQ.i8IsReset = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHARACTER_DAILYDUNGEON_SET_REQ, gS_CHARACTER_DAILYDUNGEON_SET_REQ);
	}

	public void SetData(sbyte nWeek)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			this.Close();
			return;
		}
		this.m_DayOfWeek = nWeek;
		string empty = string.Empty;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
		byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
		byte dailyDungeonDcByVipLevel = NrTSingleton<NrTableVipManager>.Instance.GetDailyDungeonDcByVipLevel(levelExp);
		this.m_SoulGemsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70002);
		this.m_lbMySoulGem.SetText(ANNUALIZED.Convert(this.m_SoulGemsCount));
		long num = this.MaxResetCount();
		long resetCount = NrTSingleton<DailyDungeonManager>.Instance.GetResetCount();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3441"),
			"count1",
			resetCount.ToString(),
			"count2",
			num.ToString()
		});
		this.m_lbLimitCount.Text = empty;
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_DAILYDUNGEON_COST_PLUS);
		long num2 = (long)(value - (int)dailyDungeonDcByVipLevel);
		this.m_lbUseSoulGem.SetText(num2.ToString());
	}

	public void ClickShopOpen(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsForm(G_ID.ITEMMALL_DLG))
		{
			ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
			if (itemMallDlg != null)
			{
				itemMallDlg.SetShowMode(ItemMallDlg.eMODE.eMODE_NORMAL);
				itemMallDlg.SetShowType(eITEMMALL_TYPE.BUY_HERO);
			}
		}
	}

	public long MaxResetCount()
	{
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_DAILYDUNGEON_LIMIT);
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
		byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
		byte dailyDungeonResetByVipLevel = NrTSingleton<NrTableVipManager>.Instance.GetDailyDungeonResetByVipLevel(levelExp);
		return num + (long)dailyDungeonResetByVipLevel;
	}
}
