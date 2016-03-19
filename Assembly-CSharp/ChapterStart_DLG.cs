using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ChapterStart_DLG : Form
{
	private float m_fStartTime;

	private float m_fCloseUITime;

	private DrawTexture m_UpImage;

	private DrawTexture m_DownImage;

	private DrawTexture m_LabelBack;

	private Label m_Label_Label2;

	private Label m_Label_Label3;

	private static float m_fBoxAniTime = 1.2f;

	private static float m_fTextAniTime = 1f;

	private string m_strCurrentQuestUnique = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "ChapterStart/DLG_ChapterStart", G_ID.QUEST_CHAPTERSTART, false);
		base.SetSize(GUICamera.width, GUICamera.height);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	public override void SetComponent()
	{
		this.m_Label_Label2 = (base.GetControl("Label_Label2") as Label);
		this.m_Label_Label2.Visible = false;
		this.m_Label_Label3 = (base.GetControl("Label_Label3") as Label);
		this.m_Label_Label3.Visible = false;
		this.m_UpImage = (base.GetControl("DrawTexture_Screen1") as DrawTexture);
		this.m_UpImage.SetSize(GUICamera.width, this.m_UpImage.height);
		this.m_UpImage.SetLocation(0f, -this.m_UpImage.height);
		this.m_UpImage.SetAlpha(0.8f);
		this.m_DownImage = (base.GetControl("DrawTexture_Screen2") as DrawTexture);
		this.m_DownImage.SetSize(GUICamera.width, this.m_DownImage.height);
		this.m_DownImage.SetLocation(0f, GUICamera.height);
		this.m_DownImage.SetAlpha(0.8f);
		this.m_LabelBack = (base.GetControl("DrawTexture_DrawTexture23") as DrawTexture);
		this.m_LabelBack.SetLocation((GUICamera.width - this.m_LabelBack.width) / 2f, (GUICamera.height - this.m_LabelBack.height) / 2f);
		this.m_LabelBack.Visible = false;
		base.SetScreenCenter();
	}

	public void SetInfo(string strQuestUnique)
	{
		this.m_strCurrentQuestUnique = strQuestUnique;
		Vector3 localPosition = this.m_UpImage.gameObject.transform.localPosition;
		Vector3 localPosition2 = this.m_UpImage.gameObject.transform.localPosition;
		localPosition2.y -= this.m_UpImage.height;
		AnimatePosition.Do(this.m_UpImage.gameObject, EZAnimation.ANIM_MODE.FromTo, localPosition, localPosition2, new EZAnimation.Interpolator(EZAnimation.linear), ChapterStart_DLG.m_fBoxAniTime, 0f, null, null);
		localPosition = this.m_DownImage.gameObject.transform.localPosition;
		localPosition2 = this.m_DownImage.gameObject.transform.localPosition;
		localPosition2.y += this.m_DownImage.height;
		AnimatePosition.Do(this.m_DownImage.gameObject, EZAnimation.ANIM_MODE.FromTo, localPosition, localPosition2, new EZAnimation.Interpolator(EZAnimation.linear), ChapterStart_DLG.m_fBoxAniTime, 0f, null, new EZAnimation.CompletionDelegate(this.EndAni));
	}

	private void EndAni(EZAnimation obj)
	{
		this.m_Label_Label2.Visible = true;
		this.m_Label_Label3.Visible = true;
		this.m_LabelBack.Visible = true;
		FadeSprite.Do(this.m_LabelBack, EZAnimation.ANIM_MODE.FromTo, new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f), new EZAnimation.Interpolator(EZAnimation.linear), ChapterStart_DLG.m_fTextAniTime, 0f, null, null);
		CQuestGroup questGroupByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(this.m_strCurrentQuestUnique);
		if (questGroupByQuestUnique == null)
		{
			return;
		}
		string empty = string.Empty;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("75");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"count",
			questGroupByQuestUnique.GetPage()
		});
		this.m_Label_Label2.Text = empty;
		this.m_Label_Label2.SetLocation((GUICamera.width - this.m_Label_Label2.width) / 2f, this.m_LabelBack.GetLocationY() + 20f);
		FadeText.Do(this.m_Label_Label2.spriteText, EZAnimation.ANIM_MODE.FromTo, new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f), new EZAnimation.Interpolator(EZAnimation.linear), ChapterStart_DLG.m_fTextAniTime, 0f, null, null);
		string groupTitle = questGroupByQuestUnique.GetGroupTitle();
		this.m_Label_Label3.Text = groupTitle;
		this.m_Label_Label3.SetLocation((GUICamera.width - this.m_Label_Label3.width) / 2f, this.m_LabelBack.GetLocationY() + 63f);
		FadeText.Do(this.m_Label_Label3.spriteText, EZAnimation.ANIM_MODE.FromTo, new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f), new EZAnimation.Interpolator(EZAnimation.linear), ChapterStart_DLG.m_fTextAniTime, 0f, null, null);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "ACT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.m_fStartTime = Time.realtimeSinceStartup;
	}

	public override void Update()
	{
		if (0f < this.m_fStartTime && Time.realtimeSinceStartup - this.m_fStartTime > ChapterStart_DLG.m_fBoxAniTime + 1f)
		{
			base.AlphaAni(0.5f, 0f, 1.8f);
			this.m_fStartTime = 0f;
			this.m_fCloseUITime = Time.realtimeSinceStartup;
		}
		if (0f < this.m_fCloseUITime && Time.realtimeSinceStartup - this.m_fCloseUITime > 1.8f)
		{
			this.Close();
		}
	}

	public override void OnClose()
	{
		NrTSingleton<EventConditionHandler>.Instance.ChapterClose.Value.Set(this.m_strCurrentQuestUnique);
		NrTSingleton<EventConditionHandler>.Instance.ChapterClose.OnTrigger();
		NrTSingleton<FormsManager>.Instance.Main_UI_Show(FormsManager.eMAIN_UI_VISIBLE_MODE.COMMON);
	}
}
