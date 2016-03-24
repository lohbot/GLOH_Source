using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class Normal_Attend_Dlg : Form
{
	private const int ATTENDANCE_COUNT_MAX = 28;

	private Label m_lbTitle;

	private ItemTexture[] m_ItItemIcon = new ItemTexture[28];

	private Label[] m_lbDayNum = new Label[28];

	private Label[] m_lbReward = new Label[28];

	private DrawTexture[] m_dtRewardClear = new DrawTexture[28];

	private DrawTexture[] m_dtRewardItemBG = new DrawTexture[28];

	private Button m_btOK;

	private Button m_btClose;

	private int[] m_ItemToolTipUnique = new int[30];

	private int[] m_ItemToolTipCount = new int[30];

	private int m_AttendItemToolTipUnique;

	private short m_AttendItemToolTipCount;

	private Label m_laDday_Count;

	private Button m_btItemGet;

	private Label m_LB_ItemName;

	private ItemTexture m_IT_RewardItem;

	private Label m_LB_RewardDay;

	private int m_nUserType;

	private int m_nGroup;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Event/DLG_NormalUser_Attendance", G_ID.EVENT_NORMAL_ATTEND, false, true);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("LB_Title") as Label);
		for (int i = 0; i < 28; i++)
		{
			this.m_ItItemIcon[i] = (base.GetControl(string.Format("DT_ItemIcon{0}", i + 1)) as ItemTexture);
			this.m_ItItemIcon[i].UsedCollider(true);
			this.m_ItItemIcon[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemDT));
			this.m_lbDayNum[i] = (base.GetControl(string.Format("LB_DayNum{0}", i + 1)) as Label);
			this.m_lbDayNum[i].Text = string.Format("{0}", i + 1);
			this.m_lbReward[i] = (base.GetControl(string.Format("Label_ItemName{0}", i + 1)) as Label);
			this.m_dtRewardClear[i] = (base.GetControl(string.Format("DT_Check{0}", i + 1)) as DrawTexture);
			this.m_dtRewardItemBG[i] = (base.GetControl(string.Format("DT_CheckBG{0}", i + 1)) as DrawTexture);
		}
		this.m_btOK = (base.GetControl("Btn_OK") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickOK));
		this.m_btClose = (base.GetControl("Button_Close") as Button);
		this.m_btClose.Click = new EZValueChangedDelegate(this.OnClickOK);
		this.m_laDday_Count = (base.GetControl("LB_DdayCount") as Label);
		this.m_LB_ItemName = (base.GetControl("LB_ItemName") as Label);
		this.m_IT_RewardItem = (base.GetControl("IT_RewardItem") as ItemTexture);
		this.m_IT_RewardItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemAttend));
		this.m_LB_RewardDay = (base.GetControl("LB_RewardDay") as Label);
		this.m_btItemGet = (base.GetControl("Btn_Reward01") as Button);
		this.m_btItemGet.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Item_Get));
	}

	public override void Update()
	{
	}

	public void OnClickOK(IUIObject obj)
	{
		this.Close();
	}

	public void InitData(int a_nUserType)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			this.m_nUserType = a_nUserType;
			this.m_nGroup = ((kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_GROUP) != 0L) ? ((int)kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_GROUP)) : 1);
		}
		if (this.m_nUserType == 1)
		{
			this.m_lbTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2741");
		}
		else if (this.m_nUserType == 3)
		{
			this.m_lbTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3384");
		}
		this.Init_Item();
		this.m_AttendItemToolTipUnique = 0;
		this.m_AttendItemToolTipCount = 0;
		this.Init_Consecutively_Attend();
		this.DailyEventDay_View();
		this.Show();
	}

	public void InitSet()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		int a_nUserType = (int)myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_TYPE);
		this.InitData(a_nUserType);
	}

	public void Init_Consecutively_Attend()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			ATTENDANCE aTTENDANCE = NrTSingleton<NrAttendance_Manager>.Instance.Get_Consecutivelyattendance(myCharInfo.ConsecutivelyattendanceTotalNum, myCharInfo.ConsecutivelyattendanceRewardType);
			if (aTTENDANCE != null)
			{
				this.m_AttendItemToolTipUnique = aTTENDANCE.m_i32Item_Unique;
				this.m_AttendItemToolTipCount = aTTENDANCE.m_i16Item_Num;
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3373"),
					"Count",
					aTTENDANCE.m_i16Attend_Sequence
				});
				this.m_LB_RewardDay.SetText(empty);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1078"),
					"count",
					aTTENDANCE.m_i16Item_Num
				});
				this.m_LB_ItemName.SetText(empty);
				this.m_IT_RewardItem.SetTextureFromBundle(aTTENDANCE.m_strImageBundle);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3374"),
					"Count",
					(int)(myCharInfo.ConsecutivelyattendanceTotalNum - myCharInfo.ConsecutivelyattendanceCurrentNum)
				});
				if (!myCharInfo.ConsecutivelyattendanceReward)
				{
					if (myCharInfo.ConsecutivelyattendanceTotalNum <= myCharInfo.ConsecutivelyattendanceCurrentNum)
					{
						this.m_laDday_Count.SetText(string.Empty);
					}
					else
					{
						this.m_laDday_Count.SetText(empty);
					}
				}
				else
				{
					this.m_laDday_Count.SetText(empty);
				}
				if (myCharInfo.ConsecutivelyattendanceTotalNum > myCharInfo.ConsecutivelyattendanceCurrentNum)
				{
					this.m_btItemGet.SetButtonTextureKey("Win_B_NewBtnOrange");
					this.m_btItemGet.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3375"));
				}
				else if (myCharInfo.ConsecutivelyattendanceReward)
				{
					this.m_btItemGet.SetButtonTextureKey("Win_B_NewBtnBlue");
					this.m_btItemGet.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("493"));
				}
				else
				{
					this.m_btItemGet.SetButtonTextureKey("Win_B_NewBtnOrange");
					this.m_btItemGet.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3375"));
				}
			}
			else
			{
				this.m_laDday_Count.SetText(string.Empty);
				this.m_LB_ItemName.SetText(string.Empty);
				this.m_IT_RewardItem.SetTexture("Win_T_ItemEmpty");
				this.m_LB_RewardDay.SetText(string.Empty);
			}
		}
	}

	public void Init_Item()
	{
		string empty = string.Empty;
		int num = 0;
		short num2 = 0;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		for (short num3 = 0; num3 < 28; num3 += 1)
		{
			NrTSingleton<NrAttendance_Manager>.Instance.Get_Attend_Item((eATTENDANCE_USERTYPE)this.m_nUserType, this.m_nGroup, num3 + 1, myCharInfo.ConsecutivelyattendanceRewardType, ref num, ref num2);
			ITEM iTEM = new ITEM();
			iTEM.m_nItemUnique = num;
			iTEM.m_nItemNum = (int)num2;
			this.m_ItItemIcon[(int)num3].SetItemTexture(iTEM);
			this.m_ItItemIcon[(int)num3].DownText_Visible = false;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1078"),
				"count",
				num2
			});
			this.m_ItemToolTipUnique[(int)num3] = num;
			this.m_ItemToolTipCount[(int)num3] = (int)num2;
			this.m_ItItemIcon[(int)num3].Data = (int)num3;
			this.m_lbReward[(int)num3].SetText(empty);
			this.m_dtRewardClear[(int)num3].Visible = false;
			this.m_dtRewardItemBG[(int)num3].Visible = false;
		}
	}

	public void OnClickItemDT(IUIObject obj)
	{
		int num = (int)obj.Data;
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = this.m_ItemToolTipUnique[num];
		iTEM.m_nItemNum = this.m_ItemToolTipCount[num];
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
	}

	public void OnClickItemAttend(IUIObject obj)
	{
		if (this.m_AttendItemToolTipUnique <= 0 || this.m_AttendItemToolTipCount <= 0)
		{
			return;
		}
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = this.m_AttendItemToolTipUnique;
		iTEM.m_nItemNum = (int)this.m_AttendItemToolTipCount;
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
	}

	public void SetitemToolTip(int i32ItemUnique, int i32ItmeNum)
	{
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = i32ItemUnique;
		iTEM.m_nItemNum = i32ItmeNum;
		GetItemDlg getItemDlg = (GetItemDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.GET_ITEM_DLG);
		if (getItemDlg != null)
		{
			getItemDlg.SetAttendItem(i32ItemUnique, i32ItmeNum, 5f);
		}
	}

	public void On_Item_Get(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			if (myCharInfo.ConsecutivelyattendanceTotalNum > myCharInfo.ConsecutivelyattendanceCurrentNum)
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EVENT_REWARD_CHANGE_DLG);
			}
			else if (myCharInfo.ConsecutivelyattendanceReward)
			{
				GS_CONSECUTIVELY_ATTENDACNE_REWARD_REQ gS_CONSECUTIVELY_ATTENDACNE_REWARD_REQ = new GS_CONSECUTIVELY_ATTENDACNE_REWARD_REQ();
				gS_CONSECUTIVELY_ATTENDACNE_REWARD_REQ.i8TotalNum = myCharInfo.ConsecutivelyattendanceTotalNum;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CONSECUTIVELY_ATTENDACNE_REWARD_REQ, gS_CONSECUTIVELY_ATTENDACNE_REWARD_REQ);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EVENT_REWARD_CHANGE_DLG);
			}
		}
	}

	public void DailyEventDay_View()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		int num = (int)myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_SEQUENCE) - 1;
		if (num >= 0 && num < 28)
		{
			for (int i = 0; i < 28; i++)
			{
				if (num > i)
				{
					this.m_dtRewardClear[i].Visible = true;
					this.m_dtRewardItemBG[i].Visible = true;
				}
				else
				{
					this.m_dtRewardClear[i].Visible = false;
					this.m_dtRewardItemBG[i].Visible = false;
				}
			}
			this.m_dtRewardClear[num].Visible = true;
		}
	}
}
