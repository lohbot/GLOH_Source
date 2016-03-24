using System;
using UnityForms;

public class CostumeBuyMsgBox_Dlg : Form
{
	private YesDelegate _yesDelegate;

	private object _data;

	private Label _lbSoulGem;

	private Label _lbMythExlixir;

	private Label _lbTitle;

	private Label _lbNote;

	private Button _btnOK;

	private Button _btnCancel;

	public override void InitializeComponent()
	{
		Form form = this;
		base.Scale = true;
		base.TopMost = true;
		NrTSingleton<UIBaseFileManager>.Instance.LoadFileAll(ref form, "SolGuide/DLG_CostumeRoom_MsgBox", G_ID.COSTUME_BUY_MSG_BOX, false, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
		base.Draggable = false;
	}

	public override void SetComponent()
	{
		this._lbSoulGem = (base.GetControl("LB_SoulGem") as Label);
		this._lbMythExlixir = (base.GetControl("LB_MythElixir") as Label);
		this._lbTitle = (base.GetControl("Label_title") as Label);
		this._lbNote = (base.GetControl("Label_Note") as Label);
		this._btnOK = (base.GetControl("Button_ok") as Button);
		this._btnOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickOk));
		this._btnCancel = (base.GetControl("Button_cancel") as Button);
		this._btnCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickCancel));
	}

	public override void OnClose()
	{
		base.OnClose();
		this.VisibleCostumeChar(true);
	}

	public void ShowMsgBox(YesDelegate yesDelegate, object data, string title, string message)
	{
		this._yesDelegate = (YesDelegate)Delegate.Combine(this._yesDelegate, yesDelegate);
		this._data = data;
		this._lbTitle.Text = title;
		this._lbNote.Text = message;
		CharCostumeInfo_Data charCostumeInfo_Data = (CharCostumeInfo_Data)data;
		if (charCostumeInfo_Data == null)
		{
			return;
		}
		this._lbSoulGem.Text = string.Format("{0:##,##0}", charCostumeInfo_Data.m_Price1Num);
		this._lbMythExlixir.Text = string.Format("{0:##,##0}", charCostumeInfo_Data.m_Price2Num);
		this.VisibleCostumeChar(false);
		this.Show();
	}

	private void OnClickOk(object EventObject)
	{
		this._yesDelegate(this._data);
		this.Close();
	}

	private void OnClickCancel(object EventObject)
	{
		this.Close();
	}

	private void VisibleCostumeChar(bool visible)
	{
		CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
		if (costumeRoom_Dlg == null || costumeRoom_Dlg._costumeViewerSetter == null || costumeRoom_Dlg._costumeViewerSetter._costumeCharSetter == null)
		{
			return;
		}
		costumeRoom_Dlg._costumeViewerSetter._costumeCharSetter.VisibleChar(visible);
	}
}
