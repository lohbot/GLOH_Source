using GAME;
using System;
using System.Text;
using UnityForms;

public class New_Attend_Dlg : Form
{
	private const int DAY_COUNT_MAX = 7;

	private DrawTexture[] m_dtItemIcon = new DrawTexture[7];

	private Label[] m_lbReward = new Label[7];

	private DrawTexture[] m_dtRewardClear = new DrawTexture[7];

	private DrawTexture[] m_dtRewardItemBG = new DrawTexture[7];

	private Button m_btOK;

	private int[] m_ItemToolTipUnique = new int[7];

	private int[] m_ItemToolTipCount = new int[7];

	private int m_nUserType;

	private int m_nGroup;

	private int m_nSequence;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Event/DLG_New_Attendance", G_ID.EVENT_NEW_ATTEND, false, true);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 7; i++)
		{
			stringBuilder.Length = 0;
			stringBuilder.Append("DrawTexture_GiftItem");
			stringBuilder.Append(string.Format("{0:00}", i + 1));
			this.m_dtItemIcon[i] = (base.GetControl(stringBuilder.ToString()) as DrawTexture);
			this.m_dtItemIcon[i].UsedCollider(true);
			this.m_dtItemIcon[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemDT));
			stringBuilder.Length = 0;
			stringBuilder.Append("Label_ItemName");
			stringBuilder.Append(string.Format("{0:00}", i + 1));
			this.m_lbReward[i] = (base.GetControl(stringBuilder.ToString()) as Label);
			stringBuilder.Length = 0;
			stringBuilder.Append("DrawTexture_Clear");
			stringBuilder.Append(string.Format("{0:00}", i + 1));
			this.m_dtRewardClear[i] = (base.GetControl(stringBuilder.ToString()) as DrawTexture);
			stringBuilder.Length = 0;
			stringBuilder.Append("DT_ItemBKBG");
			stringBuilder.Append(string.Format("{0:00}", i + 1));
			this.m_dtRewardItemBG[i] = (base.GetControl(stringBuilder.ToString()) as DrawTexture);
		}
		this.m_btOK = (base.GetControl("Button_ok") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickOK));
		this.InitSet();
	}

	public void InitSet()
	{
		this.Init_Data();
		this.Init_Item();
	}

	public override void Update()
	{
	}

	public void Init_Data()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			this.m_nUserType = ((kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_TYPE) != 0L) ? ((int)kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_TYPE)) : 1);
			this.m_nGroup = ((kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_GROUP) != 0L) ? ((int)kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_GROUP)) : 1);
			this.m_nSequence = ((kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_SEQUENCE) != 0L) ? ((int)kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_SEQUENCE)) : 1);
		}
		if (this.m_nSequence >= 7)
		{
			this.m_nGroup = (int)NrTSingleton<ContentsLimitManager>.Instance.Attend_Nomal_LastGroup();
		}
	}

	public void Init_Item()
	{
		string empty = string.Empty;
		int num = 0;
		short num2 = 0;
		string textureFromBundle = string.Empty;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		for (short num3 = 0; num3 < 7; num3 += 1)
		{
			NrTSingleton<NrAttendance_Manager>.Instance.Get_Attend_Item((eATTENDANCE_USERTYPE)this.m_nUserType, this.m_nGroup, num3 + 1, myCharInfo.ConsecutivelyattendanceRewardType, ref num, ref num2);
			ITEM iTEM = new ITEM();
			iTEM.m_nItemUnique = num;
			iTEM.m_nItemNum = 1;
			textureFromBundle = NrTSingleton<NrAttendance_Manager>.Instance.Get_NewAttend_ItemImage(this.m_nGroup, num3 + 1);
			this.m_dtItemIcon[(int)num3].SetTextureFromBundle(textureFromBundle);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1078"),
				"count",
				num2
			});
			this.m_ItemToolTipUnique[(int)num3] = num;
			this.m_ItemToolTipCount[(int)num3] = (int)num2;
			this.m_dtItemIcon[(int)num3].Data = (int)num3;
			this.m_lbReward[(int)num3].SetText(empty);
			this.m_dtRewardClear[(int)num3].Visible = false;
			this.m_dtRewardItemBG[(int)num3].Visible = false;
		}
	}

	public void OnClickOK(IUIObject obj)
	{
		this.Close();
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

	public void CheckDailyEventDay(int WeekDayCount)
	{
		int num = WeekDayCount - 1;
		if (num >= 0 && num < 7)
		{
			for (int i = 0; i < 7; i++)
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
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("ATTENDANCECHECK", this.m_dtItemIcon[num], this.m_dtItemIcon[num].GetSize());
			this.m_dtRewardClear[num].Visible = true;
		}
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

	public void DailyEventDay_View()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		int num = (int)myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_SEQUENCE) - 1;
		if (num >= 0 && num < 7)
		{
			for (int i = 0; i < 7; i++)
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
