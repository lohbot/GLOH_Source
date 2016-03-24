using System;
using UnityEngine;
using UnityForms;

public class NewExploration_RepeatBgDlg : Form
{
	private Button m_btRepeatStop;

	private DrawTexture m_dtBack;

	private string m_LoadingImg = string.Empty;

	private bool bIsSendPacket;

	private float m_fOpenTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewExploration/DLG_NewExploration_Repeat_bg", G_ID.NEWEXPLORATION_REPEAT_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.ShowBlackBG(1f);
		this.m_LoadingImg = "UI/Loading/chaostower";
	}

	public override void SetComponent()
	{
		this.m_btRepeatStop = (base.GetControl("Button_RepeatStop") as Button);
		Button expr_1C = this.m_btRepeatStop;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnStopRepeat));
		this.m_dtBack = (base.GetControl("Main_BG") as DrawTexture);
		UIDataManager.MuteSound(true);
		this.m_dtBack.SetTextureFromBundle(this.m_LoadingImg);
		base.SetLocation(0f, 0f);
		if (this.m_dtBack != null)
		{
			this.m_dtBack.SetSize(GUICamera.width, GUICamera.height);
			this.m_btRepeatStop.SetSize(GUICamera.width, GUICamera.height);
		}
		this.m_fOpenTime = Time.realtimeSinceStartup;
	}

	public override void Update()
	{
		base.Update();
		if (Time.realtimeSinceStartup - this.m_fOpenTime > 2f && !this.bIsSendPacket)
		{
			if (NrTSingleton<NewExplorationManager>.Instance.Send_GS_NEWEXPLORATION_START_REQ())
			{
				this.bIsSendPacket = true;
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI != null)
				{
					msgBoxUI.Close();
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWEXPLORATION_MAIN_DLG);
				this.Close();
			}
		}
	}

	public void OnStopRepeat(IUIObject obj)
	{
		if (this.bIsSendPacket)
		{
			return;
		}
		if (NrTSingleton<NewExplorationManager>.Instance.AutoBattle)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.StopAutoBattle), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3491"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("368"), eMsgType.MB_OK_CANCEL, 2);
			return;
		}
	}

	public void StopAutoBattle(object a_oObject)
	{
		NrTSingleton<NewExplorationManager>.Instance.SetAutoBattle(false, false, false);
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWEXPLORATION_MAIN_DLG);
		this.Close();
	}

	public override void OnClose()
	{
		UIDataManager.MuteSound(false);
		NrTSingleton<UIImageBundleManager>.Instance.DeleteTexture(this.m_LoadingImg);
	}
}
