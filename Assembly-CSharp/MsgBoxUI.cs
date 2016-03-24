using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class MsgBoxUI : Form
{
	private YesDelegate m_YesDelegate;

	private NoDelegate m_NoDelegate;

	private object m_oYesObject;

	private object m_oNoObject;

	private Box m_boxBG;

	private DrawTexture m_DrawTextureBG;

	private Label m_LbTitle;

	private Label m_LbNote;

	private Button m_btnOK;

	private Button m_btnCancel;

	private Button m_btnCancel2;

	private Button m_btnPolicy;

	private Label m_lbPolicy;

	private CheckBox m_CheckBox;

	private Label m_LbCheckBox;

	public int m_iParam;

	private string strOKText = string.Empty;

	private string strCancelText = string.Empty;

	private float m_fAutoCloseTime;

	private List<int> guideWinIDList = new List<int>();

	private UIButton _Touch;

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
		this.m_NoDelegate = null;
		this.m_oNoObject = null;
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Message/DLG_MsgBox", G_ID.MSGBOX_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_boxBG = (base.GetControl("MessageBg") as Box);
		this.m_DrawTextureBG = (base.GetControl("DrawTexture_DrawTexture6") as DrawTexture);
		this.m_LbTitle = (base.GetControl("Label_title") as Label);
		this.m_LbNote = (base.GetControl("Label_Note") as Label);
		this.m_btnOK = (base.GetControl("Button_ok") as Button);
		this.m_btnCancel = (base.GetControl("Button_cancel") as Button);
		this.m_btnCancel2 = (base.GetControl("Button_cancel2") as Button);
		this.m_CheckBox = (base.GetControl("CheckBox_C") as CheckBox);
		this.m_LbCheckBox = (base.GetControl("Label_C") as Label);
		this.m_btnPolicy = (base.GetControl("Btn_BuyRule") as Button);
		this.m_lbPolicy = (base.GetControl("LB_BuyRule") as Label);
		Button expr_F8 = this.m_btnPolicy;
		expr_F8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_F8.Click, new EZValueChangedDelegate(this.OnClickPolicy));
		Button expr_11F = this.m_btnOK;
		expr_11F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_11F.Click, new EZValueChangedDelegate(this.BtnMsg_OK));
		this.m_btnOK.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"));
		Button expr_160 = this.m_btnCancel;
		expr_160.Click = (EZValueChangedDelegate)Delegate.Combine(expr_160.Click, new EZValueChangedDelegate(this.BtnMsg_Cancel));
		this.m_btnCancel.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("9"));
		Button expr_1A1 = this.m_btnCancel2;
		expr_1A1.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1A1.Click, new EZValueChangedDelegate(this.BtnMsg_Cancel2));
		this.m_btnCancel2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"));
		this.m_CheckBox.SetToggleState(1);
		this.m_btnPolicy.Visible = false;
		this.m_lbPolicy.Visible = false;
		this.Initialize();
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
		base.DonotDepthChange(8f);
		base.SetScreenCenter();
		this.Hide();
	}

	public void Initialize()
	{
		this.m_btnOK.Visible = false;
		this.m_btnCancel.Visible = false;
		this.m_btnCancel2.Visible = false;
		this.strOKText = this.m_btnOK.GetText();
		this.strCancelText = this.m_btnCancel.GetText();
		this.InitData();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_COMMON", "WINDOW", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void InitData()
	{
	}

	public void SetMsg(YesDelegate a_deYes, object a_oObject, NoDelegate b_deNo, object b_oObject, string title, string message, string checkboxText, eMsgType type)
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
		this.m_LbCheckBox.SetText(checkboxText);
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

	public void SetMsg(YesDelegate a_deYes, object a_oObject, string title, string message, string checkboxText, eMsgType type)
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
		this.m_LbCheckBox.SetText(checkboxText);
		this.Show();
	}

	public void SetMsg(YesDelegate a_deYes, object a_oObject, string title, string message, eMsgType type, int _ShowSceneType = 2)
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
		base.ShowSceneType = _ShowSceneType;
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
		if (type == eMsgType.MB_OK)
		{
			visible = true;
		}
		else if (type == eMsgType.MB_OK_CANCEL)
		{
			visible2 = true;
		}
		else if (type == eMsgType.MB_CHECK_OK)
		{
			visible = true;
			visible3 = true;
		}
		else if (type == eMsgType.MB_CHECK_OK_CANCEL)
		{
			visible2 = true;
			visible3 = true;
		}
		this.m_btnOK.Visible = visible2;
		this.m_btnCancel.Visible = visible2;
		this.m_btnCancel2.Visible = visible;
		this.m_CheckBox.Visible = visible3;
		this.m_LbCheckBox.Visible = visible3;
	}

	public void OnOKByScript()
	{
		this.BtnMsg_OK(null);
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
		if (this.m_bOkEventImmediatelyClose)
		{
			this.Close();
		}
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

	public bool IsChecked()
	{
		return this.m_CheckBox.IsChecked();
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
	}

	public void SetCheckBoxState(bool bCheck)
	{
		if (bCheck)
		{
			this.m_CheckBox.SetToggleState(1);
		}
		else
		{
			this.m_CheckBox.SetToggleState(0);
		}
	}

	public void OnClickPolicy(IUIObject obj)
	{
		NrMobileNoticeWeb nrMobileNoticeWeb = new NrMobileNoticeWeb();
		nrMobileNoticeWeb.OnPolicyView();
	}

	public void ShowItemMallPolicy(bool bShow = true)
	{
		this.m_btnPolicy.Visible = bShow;
		this.m_lbPolicy.Visible = bShow;
	}

	public void SetPolicy(string Msg)
	{
		this.m_lbPolicy.SetText(Msg);
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (string.IsNullOrEmpty(param1))
		{
			return;
		}
		if (this.guideWinIDList != null && !this.guideWinIDList.Contains(winID))
		{
			this.guideWinIDList.Add(winID);
		}
		string[] array = param1.Split(new char[]
		{
			','
		});
		if (array == null || array.Length != 4)
		{
			return;
		}
		string text = array[0];
		Button btnOK = this.m_btnOK;
		if (btnOK == null)
		{
			return;
		}
		if (this._Touch == null)
		{
			this._Touch = UICreateControl.Button("touch", "Main_I_Touch01", 196f, 154f);
		}
		if (this._Touch == null)
		{
			return;
		}
		int anchor = int.Parse(array[1]);
		this._Touch.SetAnchor((SpriteRoot.ANCHOR_METHOD)anchor);
		this._Touch.PlayAni(true);
		this._Touch.gameObject.SetActive(true);
		this._Touch.gameObject.transform.parent = btnOK.gameObject.transform;
		this._Touch.transform.position = new Vector3(btnOK.transform.position.x, btnOK.transform.position.y, btnOK.transform.position.z - 3f);
		float x = float.Parse(array[2]);
		float y = float.Parse(array[3]);
		this._Touch.transform.eulerAngles = new Vector3(x, y, this._Touch.transform.eulerAngles.z);
		BoxCollider component = this._Touch.gameObject.GetComponent<BoxCollider>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	private void HideTouch(bool closeUI)
	{
		if (this._Touch != null && this._Touch.gameObject != null)
		{
			this._Touch.gameObject.SetActive(false);
		}
		if (!closeUI)
		{
			return;
		}
		if (this.guideWinIDList == null)
		{
			return;
		}
		foreach (int current in this.guideWinIDList)
		{
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)current) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				uI_UIGuide.CloseUI = true;
			}
		}
		this._Touch = null;
	}

	public void SetMessageFontSize(int ifontSize)
	{
		if (this.m_LbNote == null)
		{
			return;
		}
		this.m_LbNote.SetFontSize(ifontSize);
	}
}
