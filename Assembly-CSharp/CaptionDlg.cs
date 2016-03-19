using System;
using UnityForms;

public class CaptionDlg : Form
{
	private enum _Layer
	{
		NOEXPLAIN = 1,
		EXPLAIN
	}

	private Label lblTalkName;

	private FlashLabel lblTalk;

	private Box boxBG;

	private Button btDramaSkip;

	public override void InitializeComponent()
	{
		NrTSingleton<NkCharManager>.Instance.ShowHideAll(false, false, false);
		NrTSingleton<FormsManager>.Instance.HideAll();
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		if (!instance.LoadFileAll(ref form, "Caption/DLG_Caption", G_ID.CAPTION_DLG, false))
		{
			this.Close();
			return;
		}
		base.TopMost = true;
	}

	public override void SetComponent()
	{
		this.boxBG = (base.GetControl("DramaTalk_BG") as Box);
		this.lblTalkName = (base.GetControl("Drama_Npcname") as Label);
		this.lblTalk = (base.GetControl("Drama_Talk") as FlashLabel);
		this.btDramaSkip = (base.GetControl("Button_DramaPass") as Button);
		Button expr_5E = this.btDramaSkip;
		expr_5E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_5E.Click, new EZValueChangedDelegate(this.Event_SkipDrama));
		if (this.lblTalkName != null)
		{
			this.lblTalkName.SetText(string.Empty);
		}
		if (this.lblTalk != null)
		{
			this.lblTalk.SetFlashLabel(string.Empty);
		}
		this.SetPosition();
	}

	private void SetPosition()
	{
		base.SetSize(GUICamera.width, base.GetSize().y);
		float y = GUICamera.height - base.GetSize().y;
		base.SetLocation(0f, y);
		this.boxBG.SetSize(GUICamera.width, this.boxBG.GetSize().y);
		this.btDramaSkip.SetLocation(base.GetSize().x - this.btDramaSkip.GetSize().x - 20f, base.GetSize().y - this.btDramaSkip.GetSize().y - 20f);
	}

	public void SetName(string name, string explain)
	{
		if (this.lblTalkName != null)
		{
			this.lblTalkName.SetText(name);
		}
		if (string.IsNullOrEmpty(explain))
		{
			base.ShowLayer(1);
		}
		else
		{
			base.ShowLayer(2);
		}
	}

	public void SetTalk(string talk)
	{
		if (this.lblTalk != null)
		{
			this.lblTalk.SetFlashLabel(talk);
			this.lblTalk.text = talk;
		}
	}

	public void SetTalkCation(string talk)
	{
		if (this.lblTalk != null)
		{
			this.lblTalk.SetFlashLabel(NrTSingleton<NrTextMgr>.Instance.GetTextFromCaption(talk));
			this.lblTalk.text = talk;
		}
	}

	public string GetCurrentTalk()
	{
		if (this.lblTalk != null)
		{
			return this.lblTalk.text;
		}
		return string.Empty;
	}

	public void ShowBG(bool show)
	{
		if (this.boxBG != null)
		{
			this.boxBG.Hide(!show);
		}
	}

	public override void OnClose()
	{
		NrTSingleton<NkQuestManager>.Instance.DeleteBundle();
		NrTSingleton<NkCharManager>.Instance.ShowHideAll(true, true, true);
		NrTSingleton<FormsManager>.Instance.Show(G_ID.NPCTALK_DLG);
	}

	private void Event_SkipDrama(IUIObject sender)
	{
		this.Close();
	}

	public override void ChangedResolution()
	{
	}
}
