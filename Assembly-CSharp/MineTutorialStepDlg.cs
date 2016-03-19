using PROTOCOL;
using System;
using UnityForms;

public class MineTutorialStepDlg : Form
{
	private DrawTexture backImage;

	private FlashLabel talkText;

	private Button close;

	private Button excute;

	private DrawTexture m_CenterNpcImage;

	private Label m_CenterNpcName;

	private DrawTexture[] m_CenterNpcNameBack = new DrawTexture[4];

	private long m_CurStep;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "System/Dlg_GameGuide", G_ID.MINE_TUTORIAL_STEP_DLG, false);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.backImage = (base.GetControl("NPCTalk_BG") as DrawTexture);
		this.m_CenterNpcImage = (base.GetControl("DrawTexture_NPCFace01") as DrawTexture);
		this.m_CenterNpcImage.SetTexture(eCharImageType.LARGE, 242, -1);
		this.m_CenterNpcName = (base.GetControl("NPCTalk_npcname") as Label);
		this.m_CenterNpcName.Text = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(242);
		this.m_CenterNpcNameBack[0] = (base.GetControl("DrawTexture_NPCTalk_npcnameBG_left_line") as DrawTexture);
		this.m_CenterNpcNameBack[1] = (base.GetControl("DrawTexture_NPCTalk_npcnameBG_right_line") as DrawTexture);
		this.m_CenterNpcNameBack[2] = (base.GetControl("DrawTexture_NPCTalk_npcnameBG_left") as DrawTexture);
		this.m_CenterNpcNameBack[3] = (base.GetControl("DrawTexture_NPCTalk_npcnameBG_right") as DrawTexture);
		this.close = (base.GetControl("NPCTalk_close") as Button);
		this.close.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickClose));
		this.excute = (base.GetControl("Button_Button9") as Button);
		this.excute.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickType));
		this.talkText = (base.GetControl("NPCTalk_talklabel") as FlashLabel);
		this.talkText.Visible = true;
		base.SetShowLayer(2, false);
		this.RepositionControl();
	}

	public void SetStep(long step)
	{
		string flashLabel = string.Empty;
		string text = string.Empty;
		this.m_CurStep = step;
		if (step == 1L)
		{
			flashLabel = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1877");
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("213");
		}
		else if (step == 2L)
		{
			flashLabel = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1878");
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1894");
		}
		this.talkText.SetFlashLabel(flashLabel);
		this.excute.SetText(text);
	}

	private void RepositionControl()
	{
		base.SetSize(GUICamera.width, base.GetSizeY());
		base.SetLocation(0f, GUICamera.height - base.GetSizeY());
		this.backImage.SetSize(GUICamera.width, this.backImage.height);
		this.close.SetLocation(GUICamera.width - this.close.GetSize().x - 10f, this.close.GetLocationY());
		this.close.Visible = false;
		this.excute.Visible = true;
		float x = (GUICamera.width - this.m_CenterNpcImage.GetSize().x) / 2f;
		this.m_CenterNpcImage.SetLocation(x, this.m_CenterNpcImage.GetLocationY());
		x = (GUICamera.width - this.m_CenterNpcName.GetSize().x) / 2f;
		this.m_CenterNpcName.SetLocation(x, this.m_CenterNpcName.GetLocationY());
		x = (GUICamera.width - this.m_CenterNpcNameBack[0].GetSize().x) / 2f + this.m_CenterNpcNameBack[0].GetSize().x / 2f;
		this.m_CenterNpcNameBack[0].SetLocation(x, this.m_CenterNpcNameBack[0].GetLocationY());
		x = this.m_CenterNpcNameBack[0].GetLocationX();
		this.m_CenterNpcNameBack[1].SetLocation(x, this.m_CenterNpcNameBack[1].GetLocationY());
		x = (GUICamera.width - this.m_CenterNpcNameBack[2].GetSize().x) / 2f + this.m_CenterNpcNameBack[2].GetSize().x / 2f;
		this.m_CenterNpcNameBack[2].SetLocation(x, this.m_CenterNpcNameBack[2].GetLocationY());
		x = this.m_CenterNpcNameBack[2].GetLocationX();
		this.m_CenterNpcNameBack[3].SetLocation(x, this.m_CenterNpcNameBack[3].GetLocationY());
	}

	public override void OnClose()
	{
	}

	public void ClickClose(IUIObject obj)
	{
		this.ExecuteGuide();
	}

	public void ClickType(IUIObject obj)
	{
		if (this.m_CurStep == 1L)
		{
			SendPacket.GetInstance().SendIDType(1718);
		}
		else if (this.m_CurStep == 2L)
		{
			NrTSingleton<NkQuestManager>.Instance.NPCAutoMove(125);
			SendPacket.GetInstance().SendIDType(1718);
			this.Close();
		}
	}

	private void ExecuteGuide()
	{
		NrTSingleton<FormsManager>.Instance.Main_UI_Show(FormsManager.eMAIN_UI_VISIBLE_MODE.COMMON);
		NrTSingleton<NkCharManager>.Instance.ShowHideAll(true, true, true);
		NrTSingleton<FormsManager>.Instance.AddReserveDeleteForm(base.WindowID);
	}

	public override void ChangedResolution()
	{
		this.RepositionControl();
	}
}
