using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class UI_MiniDramaCaption : Form
{
	private float CamearaWidth;

	private Label NPCName;

	private FlashLabel NPCTalk;

	private Box NPCTalk_BG;

	private DrawTexture BGLine1;

	private DrawTexture BGLine2;

	private Button MiniDramaSkip;

	private bool _BGShow;

	private bool skip;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "MiniDrama/DLG_MiniDramaCaption", G_ID.MINIDRAMACAPTION_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.NPCTalk_BG = (base.GetControl("NPCTalk_BG") as Box);
		this.BGLine1 = (base.GetControl("NPCTalk_BG_line") as DrawTexture);
		this.BGLine2 = (base.GetControl("NPCTalk_BG_line2") as DrawTexture);
		this.NPCName = (base.GetControl("NPCTalk_npcname") as Label);
		this.NPCTalk = (base.GetControl("NPCTalk_talklabel") as FlashLabel);
		this.MiniDramaSkip = (base.GetControl("Button_CancelBTN") as Button);
		Button expr_8A = this.MiniDramaSkip;
		expr_8A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_8A.Click, new EZValueChangedDelegate(this.Event_SkipMiniDrama));
		if (this.NPCName != null)
		{
			this.NPCName.SetText(string.Empty);
		}
		if (this.NPCTalk != null)
		{
			this.NPCTalk.SetFlashLabel(string.Empty);
		}
		this.Show();
		this.ShowBG(this._BGShow);
		this.SetPosition();
	}

	private void SetPosition()
	{
		this.CamearaWidth = GUICamera.width;
		base.SetSize(GUICamera.width, base.GetSize().y);
		base.SetLocation(base.GetLocation().x, GUICamera.height - base.GetSize().y);
		this.NPCTalk_BG.SetSize(GUICamera.width, this.NPCTalk_BG.GetSize().y);
		this.BGLine1.SetSize(GUICamera.width, this.BGLine1.GetSize().y);
		this.BGLine2.SetSize(GUICamera.width, this.BGLine2.GetSize().y);
		float num = this.NPCTalk_BG.width / 2f - this.NPCTalk.width / 2f;
		float num2 = this.NPCTalk_BG.height / 2f - this.NPCTalk.height / 2f;
		this.NPCTalk.SetLocation(this.NPCTalk_BG.GetLocationX() + num, this.NPCTalk_BG.GetLocationY() + num2);
		this.MiniDramaSkip.SetLocation(base.GetSizeX() - this.MiniDramaSkip.GetSize().x - 7f, base.GetSizeY() - this.MiniDramaSkip.GetSize().y - 7f);
	}

	public override void Update()
	{
		if (this.CamearaWidth != GUICamera.width)
		{
			this.SetPosition();
		}
	}

	public void SetName(string name)
	{
		if (this.NPCName != null)
		{
			this.NPCName.SetText(name);
		}
	}

	public void SetTalk(string talk)
	{
		if (this.NPCTalk != null)
		{
			this.NPCTalk.SetFlashLabel(talk);
			this.NPCTalk.text = talk;
			float num = this.NPCTalk_BG.width / 2f - this.NPCTalk.width / 2f;
			float num2 = this.NPCTalk_BG.height / 2f - this.NPCTalk.height / 2f;
			this.NPCTalk.SetLocation(this.NPCTalk_BG.GetLocationX() + num, this.NPCTalk_BG.GetLocationY() + num2);
		}
	}

	public string GetCurrentTalk()
	{
		if (this.NPCTalk != null)
		{
			return this.NPCTalk.text;
		}
		return string.Empty;
	}

	public bool IsShowBG()
	{
		return !this.NPCTalk_BG.IsHidden();
	}

	public void ShowBG(bool show)
	{
		this.NPCTalk_BG.Visible = show;
		this.NPCName.Visible = show;
		this.NPCTalk.Visible = show;
		this.BGLine1.Visible = show;
		this.BGLine2.Visible = show;
	}

	private void Event_SkipMiniDrama(IUIObject sender)
	{
		if (this.skip)
		{
			return;
		}
		List<EventTrigger_Game> onTrigger = NrTSingleton<NrEventTriggerInfoManager>.Instance.GetOnTrigger();
		if (onTrigger != null)
		{
			foreach (EventTrigger_Game current in onTrigger)
			{
				foreach (GameObject current2 in current.BehaviorList)
				{
					if (current2.GetComponent<Behavior_MiniDramaEnd>() != null)
					{
						current.StartBehavior(current2);
						this.skip = true;
						break;
					}
				}
			}
			foreach (EventTrigger_Game current3 in onTrigger)
			{
				foreach (GameObject current4 in current3.BehaviorList)
				{
					if (current4.GetComponent<Behavior_uSequencer>() != null)
					{
						current4.GetComponent<Behavior_uSequencer>().SkipUSequencer();
					}
				}
			}
		}
	}

	public override void ChangedResolution()
	{
		this.SetPosition();
	}
}
