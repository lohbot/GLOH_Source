using PROTOCOL;
using System;
using UnityForms;

public class PlunderAgreeDlg : Form
{
	private Button m_bAgree;

	private Button m_bCancel;

	private DrawTexture m_dBackImage;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/dlg_pvp_agree", G_ID.PLUNDER_AGREE_DLG, false);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_bAgree = (base.GetControl("Button_hero") as Button);
		Button expr_1C = this.m_bAgree;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnAgree));
		this.m_bCancel = (base.GetControl("Button_normal") as Button);
		Button expr_59 = this.m_bCancel;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.OnCancel));
		this.m_dBackImage = (base.GetControl("DT_Innerbg") as DrawTexture);
		this.m_dBackImage.SetTextureFromBundle("UI/PvP/pvp_agree");
		base.SetScreenCenter();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
	}

	public void SetResultMsgBox()
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1668");
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(new YesDelegate(this.OnAgreeOK), null, textFromInterface, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("38"), eMsgType.MB_OK, 2);
		}
	}

	public void OnAgree(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		kMyCharInfo.SetCharSubData(5, 1L);
		SendPacket.GetInstance().SendObject(1407);
	}

	public void OnCancel(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDER_AGREE_DLG);
	}

	public void OnAgreeOK(object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MSGBOX_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDER_AGREE_DLG);
	}
}
