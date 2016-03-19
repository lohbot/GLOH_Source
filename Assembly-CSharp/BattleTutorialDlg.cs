using System;
using UnityEngine;
using UnityForms;

public class BattleTutorialDlg : Form
{
	private DrawTexture m_dtTotalBG;

	private FlashLabel m_flText;

	private float m_ScreenWidth;

	private float m_ScreenHeight;

	private float m_fDlgDepth;

	private G_ID m_eShowDlg;

	private float m_fEndTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "System/DLG_Battle_Tutorial", G_ID.BATTLE_TUTORIAL_DLG, false);
		base.ChangeSceneDestory = false;
		base.Draggable = false;
		this.Show();
		base.DonotDepthChange(NrTSingleton<FormsManager>.Instance.GetTopMostZ());
	}

	public override void SetComponent()
	{
		this._SetComponetBasic();
		this.ResizeDlg();
	}

	public override void InitData()
	{
		base.InitData();
	}

	public override void Update()
	{
		base.Update();
		if (this.m_ScreenWidth != GUICamera.width || this.m_ScreenHeight != GUICamera.height)
		{
			this.ResizeDlg();
		}
		if (Time.time > this.m_fEndTime)
		{
			this.Close();
		}
	}

	public void _SetComponetBasic()
	{
		this.m_dtTotalBG = (base.GetControl("Talk_BG") as DrawTexture);
		this.m_flText = (base.GetControl("Talk_label") as FlashLabel);
	}

	public void ResizeDlg()
	{
		base.SetLocation(0f, 0f);
		this.m_ScreenWidth = GUICamera.width;
		this.m_ScreenHeight = GUICamera.height;
		this.m_dtTotalBG.SetLocation(0f, 0f);
		this.m_dtTotalBG.SetSize(this.m_ScreenWidth, this.m_ScreenHeight);
	}

	public override void Close()
	{
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(this.m_eShowDlg);
		if (form != null)
		{
			form.InteractivePanel.depthChangeable = true;
			form.SetLocation(form.GetLocationX(), form.GetLocationY(), this.m_fDlgDepth);
			form.AutoAni = false;
		}
		base.Close();
	}

	public void SetDlgData(int nTextKey, G_ID eShowDlg, float fTime)
	{
		this.m_flText.SetFlashLabel(NrTSingleton<NrTextMgr>.Instance.GetTextFromTBS(nTextKey.ToString()));
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(eShowDlg);
		if (form != null)
		{
			this.m_fDlgDepth = form.GetLocation().z;
			form.SetLocation(form.GetLocationX(), form.GetLocationY(), NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 1f);
			form.InteractivePanel.depthChangeable = false;
			form.AutoAni = true;
			this.m_eShowDlg = eShowDlg;
			this.m_fEndTime = Time.time + fTime;
		}
	}
}
