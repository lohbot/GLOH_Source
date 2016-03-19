using GAME;
using System;
using UnityEngine;
using UnityForms;

public class MyCharInfoDlg : Form
{
	private Label gold;

	private Label m_lbActivityTime;

	private Label m_lb_WillNum;

	private Button btWillCharge1;

	private Button btWillCharge2;

	private Button m_btBuyGold1;

	private Button m_btBuyGold2;

	private Button m_btBuyHearts1;

	private Button m_btBuyHearts2;

	private Label m_lbHearts;

	private long m_nMaxActivity;

	private long m_nCurrentActivity;

	private string m_szStrActivityTime = string.Empty;

	private long oldMoney;

	private float m_fActivityUpdateTime;

	private int m_iHeartsNum;

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
		instance.LoadFileAll(ref form, "Main/DLG_MyCharInfo", G_ID.MYCHARINFO_DLG, true);
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
		this.btWillCharge1 = (base.GetControl("Button_WillCharge1") as Button);
		Button expr_2C = this.btWillCharge1;
		expr_2C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2C.Click, new EZValueChangedDelegate(this.OnClickWillCharge));
		this.btWillCharge1.DeleteSpriteText();
		this.btWillCharge2 = (base.GetControl("Button_WillCharge2") as Button);
		Button expr_74 = this.btWillCharge2;
		expr_74.Click = (EZValueChangedDelegate)Delegate.Combine(expr_74.Click, new EZValueChangedDelegate(this.OnClickWillCharge));
		this.btWillCharge2.DeleteSpriteText();
		this.m_lbActivityTime = (base.GetControl("Label_Time") as Label);
		this.m_lb_WillNum = (base.GetControl("Label_WillNum") as Label);
		this.m_btBuyGold1 = (base.GetControl("Button_Gold01") as Button);
		Button expr_E8 = this.m_btBuyGold1;
		expr_E8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E8.Click, new EZValueChangedDelegate(this.OnClickBuyGold));
		this.m_btBuyGold1.DeleteSpriteText();
		this.m_btBuyGold2 = (base.GetControl("Button_Gold02") as Button);
		Button expr_130 = this.m_btBuyGold2;
		expr_130.Click = (EZValueChangedDelegate)Delegate.Combine(expr_130.Click, new EZValueChangedDelegate(this.OnClickBuyGold));
		this.m_btBuyGold2.DeleteSpriteText();
		this.m_btBuyHearts1 = (base.GetControl("Button_Hearts01") as Button);
		Button expr_178 = this.m_btBuyHearts1;
		expr_178.Click = (EZValueChangedDelegate)Delegate.Combine(expr_178.Click, new EZValueChangedDelegate(this.OnClickBuyHearts));
		this.m_btBuyHearts1.DeleteSpriteText();
		this.m_btBuyHearts2 = (base.GetControl("Button_Hearts02") as Button);
		Button expr_1C0 = this.m_btBuyHearts2;
		expr_1C0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C0.Click, new EZValueChangedDelegate(this.OnClickBuyHearts));
		this.m_btBuyHearts2.DeleteSpriteText();
		this.m_lbHearts = (base.GetControl("Label_Hearts") as Label);
		this.m_lbHearts.SetText(string.Empty);
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(0);
			if (soldierInfo != null)
			{
			}
		}
		this.gold = (base.GetControl("Label_GoldLabel") as Label);
		this.oldMoney = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money;
		this.gold.Text = ANNUALIZED.Convert(this.oldMoney);
		if (0 >= NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			this.m_lbHearts.SetText(this.m_iHeartsNum.ToString());
		}
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
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(0);
			if (soldierInfo == null || soldierInfo.GetSolID() != 0L)
			{
			}
		}
		if (this.oldMoney != kMyCharInfo.m_Money)
		{
			this.oldMoney = kMyCharInfo.m_Money;
			this.gold.Text = ANNUALIZED.Convert(kMyCharInfo.m_Money);
		}
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
		if (this.m_iHeartsNum != NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			this.m_iHeartsNum = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
			this.m_lbHearts.SetText(ANNUALIZED.Convert(this.m_iHeartsNum));
		}
	}

	public void SetActivityPointUI()
	{
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
			solMilitarySelectDlg.CloseByParent(79);
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
}
