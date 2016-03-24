using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class MyCharInfoDlg : Form
{
	private Label gold;

	private DrawTexture m_dwRightTop;

	private DrawTexture m_dwRightBottom;

	private Box m_bxNotice;

	private Button m_btBuyGold1;

	private Button m_btBuyGold2;

	private Button m_btBuyHearts1;

	private Button m_btBuyHearts2;

	private Button m_btEvent;

	private Label m_lbHearts;

	private Label m_lbSoulGems;

	private Button m_btBuySoulGem;

	private Button m_btBuySoulGem2;

	private Button m_btnTimeShop;

	private Label m_lbTimeShop;

	private Label m_lbTimeShopRemain;

	private DrawTexture m_dtTimeShopBG1;

	private DrawTexture m_dtTimeShopBG2;

	private long m_nMaxActivity;

	private long m_nCurrentActivity;

	private string m_szStrActivityTime = string.Empty;

	private long oldMoney;

	private int m_iHeartsNum;

	private int m_iSoulGemsNum;

	private long m_i64RemainTime;

	private string m_strRemainTime = string.Empty;

	private Button costumeMenu;

	private DrawTexture newCostume;

	private Label m_lbCostume;

	private Label m_lbActivityTime;

	private Label m_lb_WillNum;

	private Button btWillCharge1;

	private Button btWillCharge2;

	private float m_fActivityUpdateTime;

	private Button m_btAttend;

	private Box m_bxAttend;

	private Label m_lbAttend;

	public string StrActivityTime
	{
		get
		{
			return this.m_szStrActivityTime;
		}
		set
		{
			this.m_szStrActivityTime = value;
		}
	}

	public long CurrentActivity
	{
		get
		{
			return this.m_nCurrentActivity;
		}
		set
		{
			this.m_nCurrentActivity = value;
		}
	}

	public long MaxActivity
	{
		get
		{
			return this.m_nMaxActivity;
		}
		set
		{
			this.m_nMaxActivity = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Main/DLG_MyCharInfo", G_ID.MYCHARINFO_DLG, false);
		base.ChangeSceneDestory = false;
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.draggable = false;
		}
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.DonotDepthChange(UIPanelManager.UI_DEPTH);
	}

	public override void SetComponent()
	{
		base.SetLocation(0f, 0f);
		this.m_btBuyGold1 = (base.GetControl("Button_Gold01") as Button);
		Button expr_2C = this.m_btBuyGold1;
		expr_2C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2C.Click, new EZValueChangedDelegate(this.OnClickBuyGold));
		this.m_btBuyGold1.DeleteSpriteText();
		this.m_btBuyGold2 = (base.GetControl("Button_Gold02") as Button);
		Button expr_74 = this.m_btBuyGold2;
		expr_74.Click = (EZValueChangedDelegate)Delegate.Combine(expr_74.Click, new EZValueChangedDelegate(this.OnClickBuyGold));
		this.m_btBuyGold2.DeleteSpriteText();
		this.m_btBuyHearts1 = (base.GetControl("Button_Hearts01") as Button);
		Button expr_BC = this.m_btBuyHearts1;
		expr_BC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_BC.Click, new EZValueChangedDelegate(this.OnClickBuyHearts));
		this.m_btBuyHearts1.DeleteSpriteText();
		this.m_btBuyHearts2 = (base.GetControl("Button_Hearts02") as Button);
		Button expr_104 = this.m_btBuyHearts2;
		expr_104.Click = (EZValueChangedDelegate)Delegate.Combine(expr_104.Click, new EZValueChangedDelegate(this.OnClickBuyHearts));
		this.m_btBuyHearts2.DeleteSpriteText();
		this.m_btBuySoulGem = (base.GetControl("Button_SoulGem01") as Button);
		this.m_btBuySoulGem.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBuyHearts));
		this.m_btBuySoulGem.DeleteSpriteText();
		this.m_btBuySoulGem2 = (base.GetControl("Button_SoulGem02") as Button);
		this.m_btBuySoulGem2.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBuyHearts));
		this.m_btBuySoulGem2.DeleteSpriteText();
		this.costumeMenu = (base.GetControl("BT_Costume") as Button);
		this.costumeMenu.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCostumeMenu));
		this.newCostume = (base.GetControl("Icon_New") as DrawTexture);
		this.newCostume.Visible = NrTSingleton<NrCharCostumeTableManager>.Instance.IsNewCostumeExist();
		this.m_lbCostume = (base.GetControl("LB_Costume") as Label);
		if (NrTSingleton<ContentsLimitManager>.Instance.IsCostumeLimit())
		{
			this.costumeMenu.Visible = false;
			this.newCostume.Visible = false;
			this.m_lbCostume.Visible = false;
			this.costumeMenu.SetValueChangedDelegate(null);
		}
		this.m_btAttend = (base.GetControl("BT_DailyCheck") as Button);
		this.m_btAttend.Click = new EZValueChangedDelegate(this.OnAttend);
		this.m_bxAttend = (base.GetControl("Box_Notice2") as Box);
		this.m_lbAttend = (base.GetControl("LB_DailyCheck") as Label);
		this.Attend_Notice_Show();
		this.m_lbHearts = (base.GetControl("Label_Hearts") as Label);
		this.m_lbHearts.SetText(string.Empty);
		this.m_lbSoulGems = (base.GetControl("Label_SoulGemLabel") as Label);
		this.m_lbSoulGems.SetText(string.Empty);
		this.gold = (base.GetControl("Label_GoldLabel") as Label);
		this.oldMoney = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money;
		this.gold.Text = ANNUALIZED.Convert(this.oldMoney);
		this.btWillCharge1 = (base.GetControl("Button_WillCharge1") as Button);
		this.m_lbActivityTime = (base.GetControl("Label_Time") as Label);
		this.m_lb_WillNum = (base.GetControl("Label_WillNum") as Label);
		if (NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			Button expr_390 = this.btWillCharge1;
			expr_390.Click = (EZValueChangedDelegate)Delegate.Combine(expr_390.Click, new EZValueChangedDelegate(this.OnClickWillCharge));
			this.btWillCharge1.DeleteSpriteText();
		}
		else
		{
			this.btWillCharge1.Visible = false;
			this.m_lbActivityTime.Visible = false;
			this.m_lb_WillNum.Visible = false;
		}
		this.m_dwRightTop = (base.GetControl("DrawTexture_MainBG_RightTop") as DrawTexture);
		this.m_dwRightTop.SetLocation(GUICamera.width - this.m_dwRightTop.GetSize().x, 0f);
		this.m_dwRightBottom = (base.GetControl("DrawTexture_MainBG_RightBottom") as DrawTexture);
		this.m_dwRightBottom.SetLocation(GUICamera.width - this.m_dwRightBottom.GetSize().x, GUICamera.height - this.m_dwRightBottom.GetSize().y);
		this.m_bxNotice = (base.GetControl("Box_Notice") as Box);
		int num = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.CurrentEventCount();
		this.m_bxNotice.SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + num.ToString());
		this.m_btEvent = (base.GetControl("BT_Event") as Button);
		this.m_btEvent.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickEvent));
		this.m_btnTimeShop = (base.GetControl("BT_TimeShop") as Button);
		this.m_btnTimeShop.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_TimeShop));
		this.m_lbTimeShop = (base.GetControl("LB_Timeshop") as Label);
		this.m_lbTimeShopRemain = (base.GetControl("LB_Timeshop_Remain") as Label);
		this.m_dtTimeShopBG1 = (base.GetControl("DT_TimeShop_timeBG01") as DrawTexture);
		this.m_dtTimeShopBG2 = (base.GetControl("DT_TimeShop_timeBG01_C_C") as DrawTexture);
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsTimeShop())
		{
			this.m_btnTimeShop.Visible = false;
			this.m_lbTimeShop.Visible = false;
			this.m_lbTimeShopRemain.Visible = false;
			this.m_dtTimeShopBG1.Visible = false;
			this.m_dtTimeShopBG2.Visible = false;
		}
		if (0 >= NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			this.m_lbHearts.SetText(this.m_iHeartsNum.ToString());
		}
		if (0 >= NkUserInventory.GetInstance().Get_First_ItemCnt(70002))
		{
			this.m_lbSoulGems.SetText(this.m_iSoulGemsNum.ToString());
		}
	}

	public override void InitData()
	{
		base.InitData();
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsAttend())
		{
			this.m_btAttend.Visible = false;
			this.m_bxAttend.Visible = false;
			this.m_lbAttend.Visible = false;
		}
	}

	public void SetCurrentNoticeUpdate()
	{
		int num = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.CurrentEventCount();
		this.m_bxNotice.SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + num.ToString());
	}

	public void SetPartyShow(bool bNowPartyShow)
	{
		if (bNowPartyShow)
		{
		}
	}

	public void ClickbuSolButton(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		int num = (int)obj.Data;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(num);
		if (soldierInfo == null || !soldierInfo.IsValid())
		{
			return;
		}
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg != null)
		{
			solMilitaryGroupDlg.SetSoldierSelectByBattle(num);
		}
	}

	public void RightbuSolButton(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		int solindex = (int)obj.Data;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(solindex);
		if (soldierInfo == null || !soldierInfo.IsValid())
		{
			return;
		}
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg != null)
		{
			solMilitaryGroupDlg.RefreshSolList();
		}
	}

	public void RightbuPartylButton(IUIObject obj)
	{
	}

	public override void Update()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (this.oldMoney != kMyCharInfo.m_Money)
		{
			this.oldMoney = kMyCharInfo.m_Money;
			this.gold.Text = ANNUALIZED.Convert(kMyCharInfo.m_Money);
		}
		if (NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			if (Time.realtimeSinceStartup >= kMyCharInfo.m_fCurrentActivityTime && kMyCharInfo.m_fCurrentActivityTime != 0f)
			{
				float num = 600f;
				if (instance != null)
				{
					if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
					{
						num = (float)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_UPDATE_ACTIVITY_MINUTE) * 60f;
					}
					else
					{
						short vipLevelActivityTime = NrTSingleton<NrTableVipManager>.Instance.GetVipLevelActivityTime();
						num = (float)(instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_UPDATE_ACTIVITY_MINUTE) - (int)vipLevelActivityTime) * 60f;
					}
				}
				kMyCharInfo.m_fCurrentActivityTime = num + kMyCharInfo.m_fCurrentActivityTime;
				if (kMyCharInfo.m_nActivityPoint < kMyCharInfo.m_nMaxActivityPoint)
				{
					kMyCharInfo.AddActivityPoint(1L);
				}
			}
			if (this.m_nCurrentActivity != kMyCharInfo.m_nActivityPoint || this.m_nMaxActivity != kMyCharInfo.m_nMaxActivityPoint)
			{
				this.m_nCurrentActivity = kMyCharInfo.m_nActivityPoint;
				this.m_nMaxActivity = kMyCharInfo.m_nMaxActivityPoint;
				this.SetActivityPointUI();
			}
			if (this.m_fActivityUpdateTime < Time.realtimeSinceStartup)
			{
				float num2 = kMyCharInfo.m_fCurrentActivityTime - Time.realtimeSinceStartup;
				int num3 = (int)(num2 / 3600f);
				int num4 = (int)((num2 - (float)num3 * 3600f) / 60f);
				int num5 = (int)((num2 - (float)num3 * 3600f - (float)num4 * 60f) % 60f);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_szStrActivityTime, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("89"),
					"min",
					num4.ToString("00"),
					"sec",
					num5.ToString("00")
				});
				this.m_lbActivityTime.SetText(this.m_szStrActivityTime);
				this.m_fActivityUpdateTime = Time.realtimeSinceStartup + 1f;
			}
		}
		if (this.m_iHeartsNum != NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			this.m_iHeartsNum = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
			this.m_lbHearts.SetText(ANNUALIZED.Convert(this.m_iHeartsNum));
		}
		if (this.m_iSoulGemsNum != NkUserInventory.GetInstance().Get_First_ItemCnt(70002))
		{
			this.m_iSoulGemsNum = NkUserInventory.GetInstance().Get_First_ItemCnt(70002);
			this.m_lbSoulGems.SetText(ANNUALIZED.Convert(this.m_iSoulGemsNum));
		}
		if (NrTSingleton<ContentsLimitManager>.Instance.IsTimeShop())
		{
			this.Update_TimeShopRemainTime();
		}
	}

	public void SetActivityPointUI()
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		string empty = string.Empty;
		if (this.m_nCurrentActivity > this.m_nMaxActivity)
		{
			string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1304");
			string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2791"),
				"CurrentNum",
				textColor + this.m_nCurrentActivity.ToString() + textColor2,
				"MaxNum",
				this.m_nMaxActivity
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2791"),
				"CurrentNum",
				this.m_nCurrentActivity,
				"MaxNum",
				this.m_nMaxActivity
			});
		}
		this.m_lb_WillNum.SetText(empty);
	}

	private void OnClickHeroInfo(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		NrTSingleton<FormsManager>.Instance.ShowHide(G_ID.SOLMILITARYGROUP_DLG);
		SolMilitarySelectDlg solMilitarySelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYSELECT_DLG) as SolMilitarySelectDlg;
		if (solMilitarySelectDlg != null)
		{
			solMilitarySelectDlg.CloseByParent(82);
		}
	}

	public void OnClickWillCharge(IUIObject obj)
	{
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHARGE_ACTIVITY_MAX);
		if (this.m_nCurrentActivity >= num)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("135"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WILLCHARGE_DLG);
	}

	public void OnClickBuyGold(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_GOLD, true);
	}

	public void OnClickBuyHearts(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_HEARTS, true);
	}

	public void UpdateNoticeInfo()
	{
		int num = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.CurrentEventCount();
		this.m_bxNotice.SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + num.ToString());
	}

	public void ClickEvent(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.EVENT_MAIN))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.EVENT_MAIN);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EVENT_MAIN);
		}
	}

	private void ClickCostumeMenu(IUIObject obj)
	{
		CostumeGuide_Dlg costumeGuide_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COSTUMEGUIDE_DLG) as CostumeGuide_Dlg;
		if (costumeGuide_Dlg == null)
		{
			Debug.LogError("ERROR, MyCharInfoDlg.cs, ClickCostumeMenu(), costumeGuideDlg is Null");
			return;
		}
		costumeGuide_Dlg.SetCostumeGuide();
		costumeGuide_Dlg.Show();
	}

	private void Click_TimeShop(IUIObject _obj)
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsTimeShop())
		{
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TIMESHOP_DLG);
	}

	private void Update_TimeShopRemainTime()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		this.m_i64RemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.NextRefreshTime - PublicMethod.GetCurTime();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strRemainTime, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3302"),
			"timestring",
			NrTSingleton<NrTableTimeShopManager>.Instance.GetTimeToString(this.m_i64RemainTime)
		});
		this.m_lbTimeShopRemain.SetText(this.m_strRemainTime);
	}

	public void OnAttend(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		long charDetail = kMyCharInfo.GetCharDetail(23);
		int num = (int)kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_TYPE);
		if (num == 1 || num == 3)
		{
			Normal_Attend_Dlg normal_Attend_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EVENT_NORMAL_ATTEND) as Normal_Attend_Dlg;
			if (normal_Attend_Dlg != null)
			{
				normal_Attend_Dlg.InitData(num);
			}
			if (charDetail == 0L)
			{
				GS_ACCUMULATE_ATTEND_NFY gS_ACCUMULATE_ATTEND_NFY = new GS_ACCUMULATE_ATTEND_NFY();
				gS_ACCUMULATE_ATTEND_NFY.m_i16AttendType = 0;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ACCUMULATE_ATTEND_NFY, gS_ACCUMULATE_ATTEND_NFY);
			}
		}
		else if (num == 2)
		{
			New_Attend_Dlg new_Attend_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EVENT_NEW_ATTEND) as New_Attend_Dlg;
			if (new_Attend_Dlg != null)
			{
				new_Attend_Dlg.Show();
			}
			if (charDetail == 0L)
			{
				GS_ACCUMULATE_ATTEND_NFY gS_ACCUMULATE_ATTEND_NFY2 = new GS_ACCUMULATE_ATTEND_NFY();
				gS_ACCUMULATE_ATTEND_NFY2.m_i16AttendType = 0;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ACCUMULATE_ATTEND_NFY, gS_ACCUMULATE_ATTEND_NFY2);
			}
			else if (new_Attend_Dlg != null)
			{
				new_Attend_Dlg.DailyEventDay_View();
			}
		}
	}

	public void Attend_Notice_Show()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo.GetCharDetail(23) == 0L)
		{
			this.m_bxAttend.Visible = true;
		}
		else if (kMyCharInfo.ConsecutivelyattendanceReward)
		{
			this.m_bxAttend.Visible = true;
		}
		else
		{
			this.m_bxAttend.Visible = false;
		}
	}
}
