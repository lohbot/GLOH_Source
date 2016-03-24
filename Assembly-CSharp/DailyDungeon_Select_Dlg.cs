using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class DailyDungeon_Select_Dlg : Form
{
	private const int MAX_SLOT_NUM = 5;

	private Label m_lbLimitCount;

	private Label[] m_lbRewardItemName = new Label[5];

	private Label[] m_lbStage = new Label[5];

	private Button[] m_btStage = new Button[5];

	private Button[] m_btStage_Reset = new Button[5];

	private Button m_btBack;

	private DrawTexture[] m_dtSatge = new DrawTexture[5];

	private DrawTexture[] m_dtSatge_Lock1 = new DrawTexture[5];

	private DrawTexture[] m_dtSatge_Lock2 = new DrawTexture[5];

	private ItemTexture[] m_itRewardItem = new ItemTexture[5];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "DailyDungeon/DLG_DAILYDUNGEON_SELECT", G_ID.DAILYDUNGEON_SELECT, false);
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
		this.m_btBack = (base.GetControl("BTN_Back") as Button);
		Button expr_1C = this.m_btBack;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickBack));
		this.m_lbLimitCount = (base.GetControl("LB_REPEATLIMIT") as Label);
		for (int i = 0; i < 5; i++)
		{
			this.m_lbStage[i] = (base.GetControl(string.Format("Label_stage{0}", i + 1)) as Label);
			this.m_btStage[i] = (base.GetControl(string.Format("BT_STAGE{0}", i + 1)) as Button);
			this.m_btStage_Reset[i] = (base.GetControl(string.Format("BT_STAGE{0}_RESET", i + 1)) as Button);
			this.m_btStage_Reset[i].Visible = false;
			this.m_dtSatge_Lock1[i] = (base.GetControl(string.Format("DT_LockIcon{0}_1", i + 1)) as DrawTexture);
			this.m_dtSatge_Lock1[i].Visible = false;
			this.m_dtSatge_Lock2[i] = (base.GetControl(string.Format("DT_LockIcon{0}_2", i + 1)) as DrawTexture);
			this.m_dtSatge_Lock2[i].Visible = false;
			this.m_dtSatge[i] = (base.GetControl(string.Format("DT_STAGE{0}", i + 1)) as DrawTexture);
			this.m_dtSatge[i].Visible = true;
			this.m_lbRewardItemName[i] = (base.GetControl(string.Format("LB_ItemName{0}", i + 1)) as Label);
			this.m_itRewardItem[i] = (base.GetControl(string.Format("IT_ItemIcon{0}", i + 1)) as ItemTexture);
		}
		base.SetScreenCenter();
		GS_CHARACTER_DAILYDUNGEON_GET_REQ obj = new GS_CHARACTER_DAILYDUNGEON_GET_REQ();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHARACTER_DAILYDUNGEON_GET_REQ, obj);
		this.SetData();
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

	public void OnClickStart(IUIObject obj)
	{
	}

	public void OnClickBack(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_SELECT))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DAILYDUNGEON_SELECT);
		}
	}

	public void SetData()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			this.Close();
			return;
		}
		string empty = string.Empty;
		bool flag = false;
		sbyte b = (sbyte)NrTSingleton<DailyDungeonManager>.Instance.GetCurrWeekofDay();
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
		for (int i = 0; i < 5; i++)
		{
			EVENT_DAILY_DUNGEON_INFO dailyDungeonInfo = EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(1, (sbyte)(i + 1));
			if ((int)b == 0 || (int)b == 6)
			{
				int num2 = this.DailyDungeonClearCheck((sbyte)(i + 1));
				flag = this.DailyDungeonRewardCheck((sbyte)(i + 1));
				if (flag)
				{
					this.m_btStage[i].controlIsEnabled = false;
					this.m_btStage[i].Click = null;
					this.m_dtSatge[i].enabled = true;
					this.m_dtSatge[i].SetTextureFromBundle("UI/" + dailyDungeonInfo.szClose);
					this.m_dtSatge_Lock1[i].Visible = true;
					this.m_dtSatge_Lock2[i].Visible = true;
					this.m_btStage_Reset[i].Visible = true;
					this.m_btStage_Reset[i].Data = (sbyte)(i + 1);
					Button expr_176 = this.m_btStage_Reset[i];
					expr_176.Click = (EZValueChangedDelegate)Delegate.Combine(expr_176.Click, new EZValueChangedDelegate(this.ClickDailyDungeonReset));
				}
				else
				{
					this.m_btStage[i].controlIsEnabled = true;
					this.m_btStage[i].Data = (sbyte)(i + 1);
					this.m_btStage[i].Click = new EZValueChangedDelegate(this.ClickDailyDungeonOpen);
					this.m_dtSatge[i].SetTextureFromBundle("UI/" + dailyDungeonInfo.szOpen);
					this.m_dtSatge_Lock1[i].Visible = false;
					this.m_dtSatge_Lock2[i].Visible = false;
					this.m_btStage_Reset[i].Visible = false;
					this.m_btStage_Reset[i].Data = (sbyte)(i + 1);
					this.m_btStage_Reset[i].Click = null;
				}
				if (num2 != 0 && num2 != i + 1)
				{
					this.m_btStage[i].controlIsEnabled = false;
					this.m_btStage[i].Click = null;
					this.m_dtSatge[i].enabled = true;
					this.m_dtSatge[i].SetTextureFromBundle("UI/" + dailyDungeonInfo.szClose);
					this.m_dtSatge_Lock1[i].Visible = false;
					this.m_dtSatge_Lock2[i].Visible = false;
					this.m_btStage_Reset[i].Visible = false;
				}
			}
			else if ((int)b == (int)((sbyte)i) + 1)
			{
				flag = this.DailyDungeonRewardCheck((sbyte)(i + 1));
				if (flag)
				{
					this.m_btStage[i].controlIsEnabled = false;
					this.m_dtSatge[i].enabled = true;
					this.m_dtSatge[i].SetTextureFromBundle("UI/" + dailyDungeonInfo.szClose);
					this.m_dtSatge_Lock1[i].Visible = true;
					this.m_dtSatge_Lock2[i].Visible = true;
					this.m_btStage_Reset[i].Visible = true;
					this.m_btStage_Reset[i].Data = (sbyte)(i + 1);
					this.m_btStage_Reset[i].Click = new EZValueChangedDelegate(this.ClickDailyDungeonReset);
				}
				else
				{
					this.m_btStage[i].controlIsEnabled = true;
					this.m_btStage[i].Data = (sbyte)(i + 1);
					this.m_btStage[i].Click = new EZValueChangedDelegate(this.ClickDailyDungeonOpen);
					this.m_dtSatge[i].enabled = false;
					this.m_dtSatge[i].SetTextureFromBundle("UI/" + dailyDungeonInfo.szOpen);
					this.m_dtSatge_Lock1[i].Visible = false;
					this.m_dtSatge_Lock2[i].Visible = false;
					this.m_btStage_Reset[i].Visible = false;
					this.m_btStage_Reset[i].Data = (sbyte)(i + 1);
				}
			}
			else
			{
				this.m_btStage[i].controlIsEnabled = false;
				this.m_dtSatge[i].enabled = true;
				this.m_dtSatge[i].SetTextureFromBundle("UI/" + dailyDungeonInfo.szClose);
				this.m_dtSatge_Lock1[i].Visible = false;
				this.m_dtSatge_Lock2[i].Visible = false;
				this.m_btStage_Reset[i].Visible = false;
				this.m_btStage_Reset[i].Data = (sbyte)(i + 1);
				this.m_btStage_Reset[i].Click = null;
			}
			this.m_itRewardItem[i].SetItemTexture(dailyDungeonInfo.i32RewardItemUnique);
			this.m_lbRewardItemName[i].SetText(NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(dailyDungeonInfo.i32RewardItemUnique));
			if (resetCount >= num && flag)
			{
				this.m_btStage[i].controlIsEnabled = false;
				this.m_dtSatge_Lock1[i].Visible = false;
				this.m_dtSatge_Lock2[i].Visible = false;
				this.m_btStage_Reset[i].Visible = false;
			}
		}
	}

	public void ClickDailyDungeonOpen(IUIObject obj)
	{
		sbyte b = (sbyte)obj.Data;
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_MAIN))
		{
			DailyDungeon_Main_Dlg dailyDungeon_Main_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DAILYDUNGEON_MAIN) as DailyDungeon_Main_Dlg;
			if (dailyDungeon_Main_Dlg != null)
			{
				NrTSingleton<DailyDungeonManager>.Instance.SetDayOfWeek(b);
				dailyDungeon_Main_Dlg.SetBasicData(b, false);
			}
		}
	}

	public void ClickDailyDungeonReset(IUIObject obj)
	{
		sbyte data = (sbyte)obj.Data;
		DailyDungeon_MsgBox_Dlg dailyDungeon_MsgBox_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DAILYDUNGEON_MSGBOX) as DailyDungeon_MsgBox_Dlg;
		if (dailyDungeon_MsgBox_Dlg != null)
		{
			dailyDungeon_MsgBox_Dlg.SetData(data);
		}
	}

	public bool DailyDungeonRewardCheck(sbyte nDayOfWeek)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			this.Close();
			return false;
		}
		Dictionary<int, DAILYDUNGEON_INFO> totalDailyDungeonInfo = NrTSingleton<DailyDungeonManager>.Instance.GetTotalDailyDungeonInfo();
		if (totalDailyDungeonInfo == null)
		{
			this.Close();
		}
		foreach (DAILYDUNGEON_INFO current in totalDailyDungeonInfo.Values)
		{
			if (current.m_i32DayOfWeek != 0)
			{
				if ((int)current.m_i8IsReward != 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int DailyDungeonClearCheck(sbyte nDayOfWeek)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			this.Close();
			return 0;
		}
		Dictionary<int, DAILYDUNGEON_INFO> totalDailyDungeonInfo = NrTSingleton<DailyDungeonManager>.Instance.GetTotalDailyDungeonInfo();
		if (totalDailyDungeonInfo == null)
		{
			this.Close();
			return 0;
		}
		foreach (DAILYDUNGEON_INFO current in totalDailyDungeonInfo.Values)
		{
			if (current.m_i32DayOfWeek != 0)
			{
				if (current.m_i32IsClear != 0)
				{
					return current.m_i32DayOfWeek;
				}
			}
		}
		return 0;
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

	public void SetInfo(int nIndex, int nDayOfWeek, bool bReward, bool bClear)
	{
		sbyte b = (sbyte)NrTSingleton<DailyDungeonManager>.Instance.GetCurrWeekofDay();
		string empty = string.Empty;
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
		EVENT_DAILY_DUNGEON_INFO dailyDungeonInfo = EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(1, (sbyte)nDayOfWeek);
		if (dailyDungeonInfo == null)
		{
			return;
		}
		if (bReward)
		{
			this.m_dtSatge[nIndex].enabled = true;
			this.m_dtSatge[nIndex].SetTextureFromBundle("UI/" + dailyDungeonInfo.szClose);
			this.m_btStage[nIndex].controlIsEnabled = true;
			this.m_btStage[nIndex].Click = null;
			this.m_dtSatge_Lock1[nIndex].Visible = true;
			this.m_dtSatge_Lock2[nIndex].Visible = true;
			this.m_btStage_Reset[nIndex].Visible = true;
			this.m_btStage_Reset[nIndex].Data = (sbyte)nDayOfWeek;
			this.m_btStage_Reset[nIndex].Click = new EZValueChangedDelegate(this.ClickDailyDungeonReset);
			if (resetCount >= num)
			{
				this.m_dtSatge_Lock1[nIndex].Visible = false;
				this.m_dtSatge_Lock2[nIndex].Visible = false;
				this.m_btStage_Reset[nIndex].Visible = false;
				this.m_btStage_Reset[nIndex].Click = null;
			}
		}
		else if ((int)b == 0 || (int)b == 6)
		{
			if (bClear && !bReward)
			{
				this.m_dtSatge[nIndex].enabled = true;
				this.m_dtSatge[nIndex].SetTextureFromBundle("UI/" + dailyDungeonInfo.szClose);
				this.m_dtSatge[nIndex].SetTextureFromBundle("UI/" + dailyDungeonInfo.szOpen);
				this.m_btStage[nIndex].controlIsEnabled = true;
				this.m_btStage[nIndex].Data = (sbyte)(nIndex + 1);
				this.m_btStage[nIndex].Click = new EZValueChangedDelegate(this.ClickDailyDungeonOpen);
				this.m_dtSatge_Lock1[nIndex].Visible = true;
				this.m_dtSatge_Lock2[nIndex].Visible = true;
				this.m_btStage_Reset[nIndex].Visible = true;
				this.m_btStage_Reset[nIndex].Data = (sbyte)(nIndex + 1);
				Button expr_263 = this.m_btStage_Reset[nIndex];
				expr_263.Click = (EZValueChangedDelegate)Delegate.Combine(expr_263.Click, new EZValueChangedDelegate(this.ClickDailyDungeonReset));
			}
		}
		this.m_dtSatge[nIndex].enabled = true;
		this.m_dtSatge[nIndex].SetTextureFromBundle("UI/" + dailyDungeonInfo.szClose);
		this.m_dtSatge[nIndex].SetTextureFromBundle("UI/" + dailyDungeonInfo.szOpen);
		this.m_btStage[nIndex].controlIsEnabled = true;
		this.m_btStage[nIndex].Data = (sbyte)(nIndex + 1);
		this.m_btStage[nIndex].Click = new EZValueChangedDelegate(this.ClickDailyDungeonOpen);
		this.m_dtSatge_Lock1[nIndex].Visible = true;
		this.m_dtSatge_Lock2[nIndex].Visible = true;
		this.m_btStage_Reset[nIndex].Visible = true;
		this.m_btStage_Reset[nIndex].Data = (sbyte)(nIndex + 1);
		Button expr_358 = this.m_btStage_Reset[nIndex];
		expr_358.Click = (EZValueChangedDelegate)Delegate.Combine(expr_358.Click, new EZValueChangedDelegate(this.ClickDailyDungeonReset));
	}
}
