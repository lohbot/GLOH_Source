using GAME;
using System;
using UnityEngine;
using UnityForms;

public class BubbleGameGuideDlg : Form
{
	private DrawTexture back;

	private DrawTexture leftUp;

	private DrawTexture rightUp;

	private DrawTexture leftDown;

	private DrawTexture rightDown;

	private Label guideText;

	private bool battleType;

	private float width;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "System/DLG_BubbleGameGuide", G_ID.BUBBLEGAMEGUIDE_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.DonotDepthChange(UIPanelManager.UI_DEPTH);
	}

	public override void SetComponent()
	{
		this.back = (base.GetControl("bubble_tutorial_bg") as DrawTexture);
		this.leftUp = (base.GetControl("bubble_tutorial_left_up") as DrawTexture);
		this.leftUp.Visible = false;
		this.rightUp = (base.GetControl("bubble_tutorial_right_up") as DrawTexture);
		this.rightUp.Visible = false;
		this.leftDown = (base.GetControl("bubble_tutorial_left_down") as DrawTexture);
		this.leftDown.Visible = false;
		this.rightDown = (base.GetControl("bubble_tutorial_right_down") as DrawTexture);
		this.guideText = (base.GetControl("bubble_tutorial_label") as Label);
		this.guideText.MultiLine = false;
		this.guideText.MaxWidth = 600f;
		this.guideText.UpdateText = true;
		if (NrTSingleton<GameGuideManager>.Instance.GetGameGuideInfo() != null)
		{
			string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(NrTSingleton<GameGuideManager>.Instance.GetGameGuideInfo().m_strBaloonTextKey);
			this.guideText.spriteText.multiline = false;
			this.guideText.Text = textFromToolTip;
			this.width = this.guideText.GetWidth();
			float totalHeight = this.guideText.spriteText.TotalHeight;
			this.guideText.SetSize(this.width, totalHeight);
			this.guideText.Text = textFromToolTip;
			float x = this.width + 20f;
			float num = totalHeight + 20f;
			base.SetSize(x, num);
			this.back.SetSize(x, num);
			this.rightDown.SetLocation(x, this.back.GetLocationY() + num + this.rightDown.GetSize().y - 3f);
			this.UpdatePosition();
		}
	}

	public void SetText(string str, Vector3 pos)
	{
		this.battleType = true;
		string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(str);
		this.guideText.Text = textFromToolTip;
		this.width = this.guideText.GetWidth();
		float totalHeight = this.guideText.spriteText.TotalHeight;
		this.guideText.SetSize(this.width, totalHeight);
		this.guideText.Text = textFromToolTip;
		float x = this.width + 20f;
		float num = totalHeight + 20f;
		base.SetSize(x, num);
		this.back.SetSize(x, num);
		this.rightDown.SetLocation(x, this.back.GetLocationY() + num + this.rightDown.GetSize().y - 3f);
		base.SetLocation(pos.x - this.width, pos.y, pos.z - 1f);
	}

	public void UpdatePosition()
	{
		if (!this.battleType)
		{
			Vector3 zero = Vector3.zero;
			NoticeIconDlg noticeIconDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_UI_ICON) as NoticeIconDlg;
			if (noticeIconDlg != null)
			{
				if (TsPlatform.IsWeb)
				{
					zero.x = noticeIconDlg.GetLocationX() + noticeIconDlg.GetButtonLocationX(ICON_TYPE.GAMEGUIDE) * 0.7f - this.width;
				}
				else
				{
					zero.x = noticeIconDlg.GetLocationX() + noticeIconDlg.GetButtonLocationX(ICON_TYPE.GAMEGUIDE) - this.width;
				}
				zero.y = noticeIconDlg.GetLocationY() - noticeIconDlg.GetSizeY() - 25f;
			}
			base.SetLocation(zero.x, zero.y, noticeIconDlg.GetLocation().z - 1f);
		}
	}

	public override void Update()
	{
		this.UpdatePosition();
	}
}
