using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class Attendance_RewardChange_Dlg : Form
{
	private Label m_LB_RewardDay01;

	private Label m_LB_RewardDay02;

	private CheckBox m_DT_CheckBox01;

	private CheckBox m_DT_CheckBox02;

	private ItemTexture m_IT_ItemTexture01;

	private ItemTexture m_IT_ItemTexture02;

	private Label m_LB_ItemName01;

	private Label m_LB_ItemName02;

	private Label m_LB_DdayCount01;

	private Label m_LB_DdayCount02;

	private Button m_Button_NO;

	private Button m_Button_OK;

	private int m_AttendItemToolTipUnique;

	private short m_AttendItemToolTipCount;

	private bool bCheck01 = true;

	private bool bCheck02 = true;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Event/DLG_RewardChoiceMsgBox", G_ID.EVENT_REWARD_CHANGE_DLG, false, true);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_LB_RewardDay01 = (base.GetControl("LB_RewardDay01") as Label);
		this.m_LB_RewardDay02 = (base.GetControl("LB_RewardDay02") as Label);
		this.m_DT_CheckBox01 = (base.GetControl("DT_CheckBox01") as CheckBox);
		this.m_DT_CheckBox02 = (base.GetControl("DT_CheckBox02") as CheckBox);
		CheckBox expr_5E = this.m_DT_CheckBox01;
		expr_5E.CheckedChanged = (EZValueChangedDelegate)Delegate.Combine(expr_5E.CheckedChanged, new EZValueChangedDelegate(this.CheckChange01));
		CheckBox expr_85 = this.m_DT_CheckBox02;
		expr_85.CheckedChanged = (EZValueChangedDelegate)Delegate.Combine(expr_85.CheckedChanged, new EZValueChangedDelegate(this.CheckChange02));
		this.m_IT_ItemTexture01 = (base.GetControl("IT_RewardItem01") as ItemTexture);
		this.m_IT_ItemTexture01.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemAttend));
		this.m_IT_ItemTexture02 = (base.GetControl("IT_RewardItem02") as ItemTexture);
		this.m_IT_ItemTexture02.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemAttend));
		this.m_LB_ItemName01 = (base.GetControl("LB_ItemName01") as Label);
		this.m_LB_ItemName02 = (base.GetControl("LB_ItemName02") as Label);
		this.m_LB_DdayCount01 = (base.GetControl("LB_DdayCount01") as Label);
		this.m_LB_DdayCount02 = (base.GetControl("LB_DdayCount02") as Label);
		this.m_Button_NO = (base.GetControl("Button_NO") as Button);
		this.m_Button_NO.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickNO));
		this.m_Button_OK = (base.GetControl("Button_OK") as Button);
		this.m_Button_OK.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickOK));
		this.m_Button_OK.SetEnabled(true);
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		string empty = string.Empty;
		ATTENDANCE aTTENDANCE = NrTSingleton<NrAttendance_Manager>.Instance.Get_ConsecutivelyattendanceIndex(1, myCharInfo.ConsecutivelyattendanceRewardType);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3373"),
			"Count",
			aTTENDANCE.m_i16Attend_Sequence
		});
		this.m_LB_RewardDay01.SetText(empty);
		this.m_LB_DdayCount01.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3377"));
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1078"),
			"count",
			aTTENDANCE.m_i16Item_Num
		});
		this.m_LB_ItemName01.SetText(empty);
		this.m_IT_ItemTexture01.SetTextureFromBundle(aTTENDANCE.m_strImageBundle);
		this.m_IT_ItemTexture01.data = 1;
		this.m_DT_CheckBox01.data = (byte)aTTENDANCE.m_i16Attend_Sequence;
		aTTENDANCE = NrTSingleton<NrAttendance_Manager>.Instance.Get_ConsecutivelyattendanceIndex(2, myCharInfo.ConsecutivelyattendanceRewardType);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3373"),
			"Count",
			aTTENDANCE.m_i16Attend_Sequence
		});
		this.m_LB_RewardDay02.SetText(empty);
		this.m_LB_DdayCount02.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3378"));
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1078"),
			"count",
			aTTENDANCE.m_i16Item_Num
		});
		this.m_LB_ItemName02.SetText(empty);
		this.m_IT_ItemTexture02.SetTextureFromBundle(aTTENDANCE.m_strImageBundle);
		this.m_IT_ItemTexture02.data = 2;
		this.m_DT_CheckBox02.data = (byte)aTTENDANCE.m_i16Attend_Sequence;
	}

	public void OnClickItemAttend(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		byte index = (byte)obj.Data;
		ATTENDANCE aTTENDANCE = NrTSingleton<NrAttendance_Manager>.Instance.Get_ConsecutivelyattendanceIndex(index, myCharInfo.ConsecutivelyattendanceRewardType);
		if (aTTENDANCE != null)
		{
			ITEM iTEM = new ITEM();
			iTEM.m_nItemUnique = aTTENDANCE.m_i32Item_Unique;
			iTEM.m_nItemNum = (int)aTTENDANCE.m_i16Item_Num;
			ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
		}
	}

	public void OnClickNO(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickOK(IUIObject obj)
	{
		if (!this.bCheck01 && !this.bCheck02)
		{
			return;
		}
		GS_CONSECUTIVELY_ATTENDACNE_REQ gS_CONSECUTIVELY_ATTENDACNE_REQ = new GS_CONSECUTIVELY_ATTENDACNE_REQ();
		if (this.bCheck01)
		{
			gS_CONSECUTIVELY_ATTENDACNE_REQ.i8TotalNum = (byte)this.m_DT_CheckBox01.data;
		}
		else if (this.bCheck02)
		{
			gS_CONSECUTIVELY_ATTENDACNE_REQ.i8TotalNum = (byte)this.m_DT_CheckBox02.data;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CONSECUTIVELY_ATTENDACNE_REQ, gS_CONSECUTIVELY_ATTENDACNE_REQ);
		this.Close();
	}

	private void CheckChange01(IUIObject a_cUIObject)
	{
		this.bCheck01 = !this.bCheck01;
		this.m_DT_CheckBox01.SetCheckState((!this.bCheck01) ? 0 : 1);
		if (this.m_DT_CheckBox01.IsChecked())
		{
			this.bCheck02 = false;
			this.m_DT_CheckBox02.SetCheckState(0);
			this.m_Button_OK.SetEnabled(true);
		}
		else if (!this.bCheck02)
		{
			this.m_Button_OK.SetEnabled(false);
		}
	}

	private void CheckChange02(IUIObject a_cUIObject)
	{
		this.bCheck02 = !this.bCheck02;
		this.m_DT_CheckBox02.SetCheckState((!this.bCheck02) ? 0 : 1);
		if (this.m_DT_CheckBox02.IsChecked())
		{
			this.bCheck01 = false;
			this.m_DT_CheckBox01.SetCheckState(0);
			this.m_Button_OK.SetEnabled(true);
		}
		else if (!this.bCheck01)
		{
			this.m_Button_OK.SetEnabled(false);
		}
	}
}
