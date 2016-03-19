using System;
using UnityForms;

public class InputNumberDlg : Form
{
	private Action<InputNumberDlg, object> m_deApply;

	private Action<InputNumberDlg, object> m_deCancel;

	private object m_object_Apply;

	private object m_object_Cancel;

	private Label m_lbTitle;

	private Box m_boxNumeric;

	private Button m_btBackspace;

	private Button[] m_btNum = new Button[10];

	private Button m_btInit;

	private Button m_btHundred;

	private Button m_btApply;

	private Button m_btCancel;

	private long m_iInputNum;

	private long m_iMaxValue = 999999999999L;

	private long m_iMinValue;

	private string m_strTitle = string.Empty;

	private string m_strText = string.Empty;

	private string m_strMessage = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "dlg_inputnumber", G_ID.DLG_INPUTNUMBER, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("Label_Label0") as Label);
		this.m_boxNumeric = (base.GetControl("Box_Inputnumber") as Box);
		this.m_btBackspace = (base.GetControl("Button_Button2") as Button);
		Button expr_48 = this.m_btBackspace;
		expr_48.Click = (EZValueChangedDelegate)Delegate.Combine(expr_48.Click, new EZValueChangedDelegate(this.OnClickBackspace));
		this.m_btNum[0] = (base.GetControl("Button_Button13") as Button);
		this.m_btNum[1] = (base.GetControl("Button_Button9") as Button);
		this.m_btNum[2] = (base.GetControl("Button_Button10") as Button);
		this.m_btNum[3] = (base.GetControl("Button_Button11") as Button);
		this.m_btNum[4] = (base.GetControl("Button_Button6") as Button);
		this.m_btNum[5] = (base.GetControl("Button_Button7") as Button);
		this.m_btNum[6] = (base.GetControl("Button_Button8") as Button);
		this.m_btNum[7] = (base.GetControl("Button_Button3") as Button);
		this.m_btNum[8] = (base.GetControl("Button_Button4") as Button);
		this.m_btNum[9] = (base.GetControl("Button_Button5") as Button);
		for (long num = 0L; num < 10L; num += 1L)
		{
			checked
			{
				this.m_btNum[(int)((IntPtr)num)].Data = num;
				Button expr_17F = this.m_btNum[(int)((IntPtr)num)];
				expr_17F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_17F.Click, new EZValueChangedDelegate(this.OnClickNum));
			}
		}
		this.m_btInit = (base.GetControl("Button_Button12") as Button);
		Button expr_1CA = this.m_btInit;
		expr_1CA.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1CA.Click, new EZValueChangedDelegate(this.OnClickInit));
		this.m_btHundred = (base.GetControl("Button_Button14") as Button);
		Button expr_207 = this.m_btHundred;
		expr_207.Click = (EZValueChangedDelegate)Delegate.Combine(expr_207.Click, new EZValueChangedDelegate(this.OnClickHundred));
		this.m_btApply = (base.GetControl("Button_Button15") as Button);
		Button expr_244 = this.m_btApply;
		expr_244.Click = (EZValueChangedDelegate)Delegate.Combine(expr_244.Click, new EZValueChangedDelegate(this.OnClickApply));
		this.m_btCancel = (base.GetControl("Button_Button16") as Button);
		Button expr_281 = this.m_btCancel;
		expr_281.Click = (EZValueChangedDelegate)Delegate.Combine(expr_281.Click, new EZValueChangedDelegate(this.OnClickCancel));
		this.m_iInputNum = 0L;
		this.m_boxNumeric.Text = "0";
		this.m_iMaxValue = 999999999999L;
		this.m_iMinValue = 0L;
		if (TsPlatform.IsMobile)
		{
			base.SetScreenCenter();
		}
	}

	public override void Update()
	{
	}

	public void SetMinMax(long i64Min, long i64Max)
	{
		this.m_iMinValue = i64Min;
		this.m_iMaxValue = i64Max;
		this.AdjustInputNum();
	}

	public void SetNum(long i64Num)
	{
		this.m_iInputNum = i64Num;
		this.AdjustInputNum();
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public void SetCallback(Action<InputNumberDlg, object> a_deApply, object object_apply, Action<InputNumberDlg, object> a_deCancel, object object_cancel)
	{
		if (a_deApply != null)
		{
			this.m_deApply = (Action<InputNumberDlg, object>)Delegate.Combine(this.m_deApply, new Action<InputNumberDlg, object>(a_deApply.Invoke));
		}
		if (a_deCancel != null)
		{
			this.m_deCancel = (Action<InputNumberDlg, object>)Delegate.Combine(this.m_deCancel, new Action<InputNumberDlg, object>(a_deCancel.Invoke));
		}
		this.m_object_Apply = object_apply;
		this.m_object_Cancel = object_cancel;
	}

	public long GetNum()
	{
		return this.m_iInputNum;
	}

	private void AdjustInputNum()
	{
		if (this.m_iInputNum > this.m_iMaxValue)
		{
			this.m_iInputNum = this.m_iMaxValue;
		}
		this.m_boxNumeric.Text = ANNUALIZED.Convert(this.m_iInputNum);
	}

	private void OnClickBackspace(IUIObject obj)
	{
		this.m_iInputNum /= 10L;
		this.AdjustInputNum();
	}

	private void OnClickInit(IUIObject obj)
	{
		this.m_iInputNum = 0L;
		this.AdjustInputNum();
	}

	private void OnClickHundred(IUIObject obj)
	{
		this.m_iInputNum *= 100L;
		this.AdjustInputNum();
	}

	private void OnClickNum(IUIObject obj)
	{
		long num = (long)obj.Data;
		this.ClickNum(num);
	}

	private void ClickNum(long Num)
	{
		this.m_iInputNum = this.m_iInputNum * 10L + Num;
		this.AdjustInputNum();
	}

	private void OnClickApply(IUIObject obj)
	{
		if (this.m_iInputNum < this.m_iMinValue || this.m_iInputNum > this.m_iMaxValue)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (string.Empty == this.m_strTitle)
			{
				this.m_strTitle = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1643");
			}
			if (string.Empty == this.m_strText)
			{
				this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1644");
			}
			this.m_strMessage = string.Format("{0}({1} ~ {2})", this.m_strText, this.m_iMinValue.ToString(), this.m_iMaxValue.ToString());
			msgBoxUI.SetMsg(null, null, this.m_strTitle, this.m_strMessage, eMsgType.MB_OK);
			return;
		}
		if (this.m_deApply != null)
		{
			this.m_deApply(this, this.m_object_Apply);
		}
		this.Close();
	}

	public void OnClickCancel(IUIObject obj)
	{
		if (this.m_deCancel != null)
		{
			this.m_deCancel(this, this.m_object_Cancel);
		}
		this.Close();
	}

	public void SetPos(float posx, float posy)
	{
		base.SetLocation(posx, posy);
	}

	public void SetInputNum(long lInputNum)
	{
		this.m_iInputNum = lInputNum;
	}

	public void SetTitleText(string strText)
	{
		this.m_lbTitle.SetText(strText);
	}
}
