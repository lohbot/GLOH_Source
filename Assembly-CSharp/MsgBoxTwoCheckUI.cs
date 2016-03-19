using GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class MsgBoxTwoCheckUI : Form
{
	private YesDelegate m_YesDelegate;

	private NoDelegate m_NoDelegate;

	private CheckBox1Delegate m_Check1Delegate;

	private CheckBox2Delegate m_Check2Delegate;

	private object m_oYesObject;

	private object m_oNoObject;

	private Box m_boxBG;

	private DrawTexture m_DrawTextureBG;

	private DrawTexture m_DrawTexture_bg1;

	private DrawTexture m_DrawTexture_bg2;

	private DrawTexture m_DT_ITEMREMAIN;

	private DrawTexture m_DT_ITEMREMAIN2;

	private DrawTexture m_DT_SPEEDINNER;

	private DrawTexture m_DT_HEARTINNER;

	private DrawTexture m_DT_HEART;

	private DrawTexture m_DT_SPEEDUP;

	private Label m_LbTitle;

	private Label m_LbNote;

	private Button m_btnOK;

	private Button m_btnCancel;

	private CheckBox m_CheckBox1;

	private Label m_LbCheckBox1;

	private CheckBox m_CheckBox2;

	private Label m_LbCheckBox2;

	private Button m_btnHeart1;

	private Button m_btnHeart2;

	private Button m_btnSpeedUp1;

	private Button m_btnSpeedUp2;

	private Label m_LbHeartNum;

	private Label m_LbSpeedUpNum;

	private int m_iHeartsNum;

	private int m_iSpeedUpNum;

	public int m_iParam;

	private string strOKText = string.Empty;

	private string strCancelText = string.Empty;

	private float m_fAutoCloseTime;

	private bool m_bOkEventImmediatelyClose = true;

	public float AutoCloseTime
	{
		get
		{
			return this.m_fAutoCloseTime;
		}
		set
		{
			this.m_fAutoCloseTime = value;
		}
	}

	public bool OkEventImmediatelyClose
	{
		set
		{
			this.m_bOkEventImmediatelyClose = value;
		}
	}

	public override void InitializeComponent()
	{
		this.m_YesDelegate = null;
		this.m_oYesObject = null;
		this.m_YesDelegate = null;
		this.m_oNoObject = null;
		this.m_Check1Delegate = null;
		this.m_Check2Delegate = null;
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFile(ref form, "Message/DLG_MsgBoxTwoCheck", G_ID.MSGBOX_TWOCHECK_DLG, true);
		instance.CreateControl(ref this.m_boxBG, "MessageBg");
		instance.CreateControl(ref this.m_DrawTextureBG, "DrawTexture_DrawTexture6");
		instance.CreateControl(ref this.m_LbTitle, "Label_title");
		instance.CreateControl(ref this.m_LbNote, "Label_Note");
		instance.CreateControl(ref this.m_btnOK, "Button_ok");
		instance.CreateControl(ref this.m_btnCancel, "Button_cancel");
		instance.CreateControl(ref this.m_LbCheckBox1, "Label_1");
		instance.CreateControl(ref this.m_LbCheckBox2, "Label_2");
		instance.CreateControl(ref this.m_btnHeart2, "BT_HEART2");
		instance.CreateControl(ref this.m_btnSpeedUp2, "BT_SPEEDUP2");
		instance.CreateControl(ref this.m_LbHeartNum, "LB_HEARTNUM");
		instance.CreateControl(ref this.m_LbSpeedUpNum, "LB_SPEEDUPNUM");
		instance.CreateControl(ref this.m_DrawTexture_bg1, "DrawTexture_bg1");
		instance.CreateControl(ref this.m_DrawTexture_bg2, "DrawTexture_bg2");
		instance.CreateControl(ref this.m_DT_ITEMREMAIN, "DT_ITEMREMAIN");
		instance.CreateControl(ref this.m_DT_ITEMREMAIN2, "DT_ITEMREMAIN2");
		instance.CreateControl(ref this.m_DT_SPEEDINNER, "DT_SPEEDINNER");
		instance.CreateControl(ref this.m_DT_HEARTINNER, "DT_HEARTINNER");
		instance.CreateControl(ref this.m_DT_HEART, "DT_HEART");
		instance.CreateControl(ref this.m_DT_SPEEDUP, "DT_SPEEDUP");
		instance.CreateControl(ref this.m_CheckBox1, "CheckBox_1");
		instance.CreateControl(ref this.m_CheckBox2, "CheckBox_2");
		instance.CreateControl(ref this.m_btnHeart1, "BT_HEART");
		instance.CreateControl(ref this.m_btnSpeedUp1, "BT_SPEEDUP");
		Button expr_203 = this.m_btnOK;
		expr_203.Click = (EZValueChangedDelegate)Delegate.Combine(expr_203.Click, new EZValueChangedDelegate(this.BtnMsg_OK));
		this.m_btnOK.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"));
		Button expr_244 = this.m_btnCancel;
		expr_244.Click = (EZValueChangedDelegate)Delegate.Combine(expr_244.Click, new EZValueChangedDelegate(this.BtnMsg_Cancel));
		this.m_btnCancel.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("9"));
		Button expr_285 = this.m_btnHeart1;
		expr_285.Click = (EZValueChangedDelegate)Delegate.Combine(expr_285.Click, new EZValueChangedDelegate(this.OnClickBuyHearts));
		this.m_btnHeart1.DeleteSpriteText();
		Button expr_2B7 = this.m_btnHeart2;
		expr_2B7.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2B7.Click, new EZValueChangedDelegate(this.OnClickBuyHearts));
		this.m_btnHeart2.DeleteSpriteText();
		Button expr_2E9 = this.m_btnSpeedUp1;
		expr_2E9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2E9.Click, new EZValueChangedDelegate(this.OnClickBuySpeedUp));
		this.m_btnSpeedUp1.DeleteSpriteText();
		Button expr_31B = this.m_btnSpeedUp2;
		expr_31B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_31B.Click, new EZValueChangedDelegate(this.OnClickBuySpeedUp));
		this.m_btnSpeedUp2.DeleteSpriteText();
		this.m_LbHeartNum.SetText(string.Empty);
		if (0 >= NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			this.m_LbHeartNum.SetText(this.m_iHeartsNum.ToString());
		}
		this.m_LbSpeedUpNum.SetText(string.Empty);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_BATTLESPEED_COUNT);
			if (0L >= charSubData)
			{
				this.m_LbSpeedUpNum.SetText(charSubData.ToString());
			}
		}
		this.m_CheckBox1.SetToggleState(1);
		this.m_CheckBox2.SetToggleState(1);
		this.m_CheckBox1.SetValueChangedDelegate(new EZValueChangedDelegate(this.CheckBox1_Click));
		this.m_CheckBox2.SetValueChangedDelegate(new EZValueChangedDelegate(this.CheckBox2_Click));
		this.Initialize();
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
		base.DonotDepthChange(90f);
		base.SetScreenCenter();
		this.Hide();
	}

	public void Initialize()
	{
		this.m_btnOK.Visible = false;
		this.m_btnCancel.Visible = false;
		this.strOKText = this.m_btnOK.GetText();
		this.strCancelText = this.m_btnCancel.GetText();
		this.InitData();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_COMMON", "WINDOW", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void InitData()
	{
	}

	public void OnClickBuyHearts(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_HEARTS, true);
		this.Close();
	}

	public void OnClickBuySpeedUp(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_ORI, true);
		this.Close();
	}

	public void SetMsg(YesDelegate a_deYes, object a_oObject, NoDelegate b_deNo, object b_oObject, string title, string message, string checkbox1Text, CheckBox1Delegate a_deCheck1, string checkbox2Text, CheckBox2Delegate a_deCheck2, eMsgType type)
	{
		if (a_deYes != null)
		{
			this.m_YesDelegate = (YesDelegate)Delegate.Combine(this.m_YesDelegate, new YesDelegate(a_deYes.Invoke));
		}
		if (a_oObject != null)
		{
			this.m_oYesObject = a_oObject;
		}
		if (b_deNo != null)
		{
			this.m_NoDelegate = (NoDelegate)Delegate.Combine(this.m_NoDelegate, new NoDelegate(b_deNo.Invoke));
		}
		if (b_oObject != null)
		{
			this.m_oNoObject = b_oObject;
		}
		this.SetMsgType(type);
		this.m_LbTitle.Text = title;
		this.m_LbNote.SetText(message);
		this.m_LbCheckBox1.SetText(checkbox1Text);
		if (a_deCheck1 != null)
		{
			this.m_Check1Delegate = (CheckBox1Delegate)Delegate.Combine(this.m_Check1Delegate, new CheckBox1Delegate(a_deCheck1.Invoke));
		}
		this.m_LbCheckBox2.SetText(checkbox2Text);
		if (a_deCheck2 != null)
		{
			this.m_Check2Delegate = (CheckBox2Delegate)Delegate.Combine(this.m_Check2Delegate, new CheckBox2Delegate(a_deCheck2.Invoke));
		}
		this.Show();
	}

	public void SetMsg(YesDelegate a_deYes, object a_oObject, NoDelegate b_deNo, object b_oObject, string title, string message, eMsgType type)
	{
		if (a_deYes != null)
		{
			this.m_YesDelegate = (YesDelegate)Delegate.Combine(this.m_YesDelegate, new YesDelegate(a_deYes.Invoke));
		}
		if (a_oObject != null)
		{
			this.m_oYesObject = a_oObject;
		}
		if (b_deNo != null)
		{
			this.m_NoDelegate = (NoDelegate)Delegate.Combine(this.m_NoDelegate, new NoDelegate(b_deNo.Invoke));
		}
		if (b_oObject != null)
		{
			this.m_oNoObject = b_oObject;
		}
		this.SetMsgType(type);
		this.m_LbTitle.Text = title;
		this.m_LbNote.SetText(message);
		this.Show();
	}

	public void SetMsg(YesDelegate a_deYes, object a_oObject, string title, string message, eMsgType type)
	{
		if (a_deYes != null)
		{
			this.m_YesDelegate = (YesDelegate)Delegate.Combine(this.m_YesDelegate, new YesDelegate(a_deYes.Invoke));
		}
		if (a_oObject != null)
		{
			this.m_oYesObject = a_oObject;
		}
		this.SetMsgType(type);
		this.m_LbTitle.Text = title;
		this.m_LbNote.SetText(message);
		this.Show();
	}

	public object GetYesObject()
	{
		return this.m_oYesObject;
	}

	public void SetMsgType(eMsgType type)
	{
		bool visible = false;
		bool visible2 = false;
		bool visible3 = false;
		switch (type)
		{
		case eMsgType.MB_OK_CANCEL:
			visible = true;
			break;
		case eMsgType.MB_CHECK_OK:
			visible2 = true;
			break;
		case eMsgType.MB_CHECK_OK_CANCEL:
			visible = true;
			visible2 = true;
			break;
		case eMsgType.MB_CHECK12_OK:
			visible = true;
			visible2 = true;
			visible3 = true;
			break;
		case eMsgType.MB_CHECK12_OK_CANCEL:
			visible = true;
			visible2 = true;
			visible3 = true;
			break;
		}
		this.m_btnOK.Visible = visible;
		this.m_btnCancel.Visible = visible;
		this.m_CheckBox1.Visible = visible2;
		this.m_CheckBox2.Visible = visible3;
		this.m_LbCheckBox1.Visible = visible2;
		this.m_LbCheckBox2.Visible = visible3;
	}

	private void CheckBox1_Click(IUIObject obj)
	{
		if (this.m_Check1Delegate != null)
		{
			this.m_Check1Delegate(this.m_oYesObject);
		}
	}

	private void CheckBox2_Click(IUIObject obj)
	{
		if (this.m_Check2Delegate != null)
		{
			this.m_Check2Delegate(this.m_oYesObject);
		}
	}

	private void BtnMsg_OK(IUIObject obj)
	{
		if (this.m_YesDelegate != null)
		{
			this.m_YesDelegate(this.m_oYesObject);
		}
		if (this.m_bOkEventImmediatelyClose)
		{
			this.Close();
		}
	}

	private void BtnMsg_Cancel(IUIObject obj)
	{
		if (this.m_NoDelegate != null)
		{
			this.m_NoDelegate(this.m_oNoObject);
		}
		this.Close();
	}

	private void BtnMsg_Cancel2(IUIObject obj)
	{
		if (this.m_YesDelegate != null)
		{
			this.m_YesDelegate(this.m_oYesObject);
		}
		this.Close();
	}

	public void SetButtonOKText(string strText)
	{
		this.m_btnOK.SetText(strText);
	}

	public void SetButtonCancelText(string strText)
	{
		this.m_btnCancel.SetText(strText);
	}

	public override void OnClose()
	{
		this.m_btnOK.SetText(this.strOKText);
		this.m_btnCancel.SetText(this.strCancelText);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_COMMON", "WINDOW", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public bool IsChecked(int index)
	{
		bool result;
		if (index == 1)
		{
			result = this.m_CheckBox1.IsChecked();
		}
		else
		{
			result = this.m_CheckBox2.IsChecked();
		}
		return result;
	}

	public void SetMessageAnchor(SpriteText.Anchor_Pos _Anchor)
	{
		this.m_LbNote.SetAnchorText(_Anchor);
	}

	public void AddBlackBgClickCloseForm()
	{
		base.BLACK_BG.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
	}

	public override void Update()
	{
		if (0f < this.m_fAutoCloseTime && this.m_fAutoCloseTime <= Time.time)
		{
			this.m_fAutoCloseTime = 0f;
			this.BtnMsg_Cancel(null);
		}
		if (this.m_iHeartsNum != NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			this.m_iHeartsNum = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
			this.m_LbHeartNum.SetText(ANNUALIZED.Convert(this.m_iHeartsNum));
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_BATTLESPEED_COUNT);
			if ((long)this.m_iSpeedUpNum != charSubData)
			{
				this.m_iSpeedUpNum = (int)charSubData;
				this.m_LbSpeedUpNum.SetText(ANNUALIZED.Convert(this.m_iSpeedUpNum));
			}
		}
	}

	public void SetCheckBoxState(int index, bool bCheck)
	{
		if (bCheck)
		{
			if (index == 1)
			{
				this.m_CheckBox1.SetToggleState(1);
			}
			else
			{
				this.m_CheckBox2.SetToggleState(1);
			}
		}
		else if (index == 1)
		{
			this.m_CheckBox1.SetToggleState(0);
		}
		else
		{
			this.m_CheckBox2.SetToggleState(0);
		}
	}
}
