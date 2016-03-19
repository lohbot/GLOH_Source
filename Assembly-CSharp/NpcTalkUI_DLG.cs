using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class NpcTalkUI_DLG : Form
{
	private Button m_NPCTalk_transbutton;

	private Box m_NPCTalk_BG;

	private Button m_NPCTalk_close;

	private FlashLabel m_NPCTalk_talklabel;

	private DrawTexture m_DrawTexture_line1;

	private DrawTexture m_DrawTexture_line2;

	private DrawTexture m_DrawTexture_line3;

	private DrawTexture m_DrawTexture_line4;

	private DrawTexture m_DrawTexture_line5;

	private DrawTexture m_DrawTexture_line6;

	private TalkButton[] m_TalkButton = new TalkButton[6];

	private int m_i32Grade;

	private E_NPC_TALK_STEP m_Step;

	private int m_i32Start = Environment.TickCount;

	private int m_i32End = Environment.TickCount;

	private int m_i32DlgCount = 1;

	private string m_strDlgIndex = string.Empty;

	private List<NPC_TALK_QUEST_STATE> m_NpcUIMenu = new List<NPC_TALK_QUEST_STATE>();

	private List<NPC_TALK_QUEST_STATE> m_QuestMenu = new List<NPC_TALK_QUEST_STATE>();

	private short m_i16CurrentPage = 1;

	private maxCamera m_WorldCamera;

	private NPC_TALK_QUEST_STATE m_State;

	private NrCharKindInfo m_CurNpc;

	private short m_16CurCharUnique;

	private CNpcUI m_NpcUI;

	private string m_strNpcCHARCODE;

	private short m_i16PreUnique;

	public bool m_IsEvent;

	private int m_i32StartCamera = Environment.TickCount;

	private int m_i32EndCamera = Environment.TickCount;

	private bool m_bClickedMoveNPC;

	private bool m_bShakeCamera;

	private Vector3 m_vStartPos = default(Vector3);

	private Button m_UserAnswer;

	private DrawTexture m_SelectTalkArrow;

	private bool m_bBackCamera;

	private float firstPosY1;

	private float firstPosY2;

	private QuestGiveItemDlg m_QuestGiveItemDlg;

	private DrawTexture m_QuestImage;

	private DrawTexture m_FadeImage;

	private bool m_bAutoQuest;

	private bool m_bSet;

	private bool m_bShowTerritory = true;

	private Button m_Start;

	public string strNpcTalkCount = string.Empty;

	private Label m_CenterNpcName;

	private DrawTexture[] m_CenterNpcNameBack = new DrawTexture[4];

	private Box m_LeftNpcName;

	private DrawTexture m_LeftNpcNameBack;

	private DrawTexture m_LeftNpcNameBackLine;

	private Box m_RightNpcName;

	private DrawTexture m_RightNpcNameBack;

	private DrawTexture m_RightNpcNameBackLine;

	private DrawTexture m_CenterNpcImage;

	private DrawTexture m_LeftNpcImage;

	private DrawTexture m_RightNpcImage;

	private bool loadQuestImage;

	private Texture2D questImage;

	private int i32MaxCount;

	public QUEST_DLG_INFO currentDlgInfo;

	private bool m_bShowAll = true;

	private int m_i32MenuCount
	{
		get
		{
			return this.m_QuestMenu.Count + this.m_NpcUIMenu.Count;
		}
	}

	public short NPCCharUnique
	{
		get
		{
			return this.m_16CurCharUnique;
		}
		set
		{
			this.m_16CurCharUnique = value;
		}
	}

	public override void InitializeComponent()
	{
		NrTSingleton<UIDataManager>.Instance.DeleteBundle();
		NrTSingleton<FormsManager>.Instance.HideExcept(G_ID.NPCTALK_DLG);
		NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(true);
		NrTSingleton<NkCharManager>.Instance.ShowHideAll(true, true);
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = false;
		instance.LoadFileAll(ref form, "NpcTalk/DLG_NPCTalk_New", G_ID.NPCTALK_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_NPCTalk_transbutton = (base.GetControl("NPCTalk_Transbutton") as Button);
		this.m_NPCTalk_BG = (base.GetControl("NPCTalk_BG") as Box);
		this.m_NPCTalk_close = (base.GetControl("NPCTalk_close") as Button);
		this.m_NPCTalk_close.Visible = false;
		this.firstPosY1 = this.m_NPCTalk_close.GetLocationY() - 210f;
		this.firstPosY2 = this.m_NPCTalk_close.GetLocationY() - 20f;
		this.m_UserAnswer = (base.GetControl("Button_SelectTalk") as Button);
		Button expr_98 = this.m_UserAnswer;
		expr_98.Click = (EZValueChangedDelegate)Delegate.Combine(expr_98.Click, new EZValueChangedDelegate(this.BtnClickNext));
		this.m_UserAnswer.MultiLine = true;
		this.m_UserAnswer.MaxWidth = 1000f;
		this.m_UserAnswer.Visible = false;
		this.m_UserAnswer.UpdateText = true;
		this.m_NPCTalk_talklabel = (base.GetControl("NPCTalk_talklabel") as FlashLabel);
		this.m_TalkButton[0] = new TalkButton();
		this.m_TalkButton[1] = new TalkButton();
		this.m_TalkButton[2] = new TalkButton();
		this.m_TalkButton[3] = new TalkButton();
		this.m_TalkButton[4] = new TalkButton();
		this.m_TalkButton[5] = new TalkButton();
		this.m_TalkButton[0].m_NPCTalk_menutitle = (base.GetControl("NPCTalk_menutitle4") as Button);
		this.m_TalkButton[1].m_NPCTalk_menutitle = (base.GetControl("NPCTalk_menutitle5") as Button);
		this.m_TalkButton[2].m_NPCTalk_menutitle = (base.GetControl("NPCTalk_menutitle6") as Button);
		this.m_TalkButton[3].m_NPCTalk_menutitle = (base.GetControl("NPCTalk_menutitle7") as Button);
		this.m_TalkButton[4].m_NPCTalk_menutitle = (base.GetControl("NPCTalk_menutitle8") as Button);
		this.m_TalkButton[5].m_NPCTalk_menutitle = (base.GetControl("NPCTalk_menutitle9") as Button);
		this.m_TalkButton[0].m_Label_menutitle_font = (base.GetControl("Label_menutitle4_font") as Label);
		this.m_TalkButton[1].m_Label_menutitle_font = (base.GetControl("Label_menutitle5_font") as Label);
		this.m_TalkButton[2].m_Label_menutitle_font = (base.GetControl("Label_menutitle6_font") as Label);
		this.m_TalkButton[3].m_Label_menutitle_font = (base.GetControl("Label_menutitle7_font") as Label);
		this.m_TalkButton[4].m_Label_menutitle_font = (base.GetControl("Label_menutitle8_font") as Label);
		this.m_TalkButton[5].m_Label_menutitle_font = (base.GetControl("Label_menutitle9_font") as Label);
		this.m_TalkButton[0].m_NPCTalk_menuicon = (base.GetControl("NPCTalk_menuicon4") as DrawTexture);
		this.m_TalkButton[1].m_NPCTalk_menuicon = (base.GetControl("NPCTalk_menuicon5") as DrawTexture);
		this.m_TalkButton[2].m_NPCTalk_menuicon = (base.GetControl("NPCTalk_menuicon6") as DrawTexture);
		this.m_TalkButton[3].m_NPCTalk_menuicon = (base.GetControl("NPCTalk_menuicon7") as DrawTexture);
		this.m_TalkButton[4].m_NPCTalk_menuicon = (base.GetControl("NPCTalk_menuicon8") as DrawTexture);
		this.m_TalkButton[5].m_NPCTalk_menuicon = (base.GetControl("NPCTalk_menuicon9") as DrawTexture);
		this.m_DrawTexture_line1 = (base.GetControl("DrawTexture_line1") as DrawTexture);
		this.m_DrawTexture_line2 = (base.GetControl("DrawTexture_line2") as DrawTexture);
		this.m_DrawTexture_line3 = (base.GetControl("DrawTexture_line3") as DrawTexture);
		this.m_DrawTexture_line4 = (base.GetControl("DrawTexture_line4") as DrawTexture);
		this.m_DrawTexture_line5 = (base.GetControl("DrawTexture_line5") as DrawTexture);
		this.m_DrawTexture_line6 = (base.GetControl("DrawTexture_line6") as DrawTexture);
		Button expr_3EC = this.m_TalkButton[0].m_NPCTalk_menutitle;
		expr_3EC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3EC.Click, new EZValueChangedDelegate(this.Menu1));
		Button expr_41A = this.m_TalkButton[1].m_NPCTalk_menutitle;
		expr_41A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_41A.Click, new EZValueChangedDelegate(this.Menu1));
		Button expr_448 = this.m_TalkButton[2].m_NPCTalk_menutitle;
		expr_448.Click = (EZValueChangedDelegate)Delegate.Combine(expr_448.Click, new EZValueChangedDelegate(this.Menu1));
		Button expr_476 = this.m_TalkButton[3].m_NPCTalk_menutitle;
		expr_476.Click = (EZValueChangedDelegate)Delegate.Combine(expr_476.Click, new EZValueChangedDelegate(this.Menu1));
		Button expr_4A4 = this.m_TalkButton[4].m_NPCTalk_menutitle;
		expr_4A4.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4A4.Click, new EZValueChangedDelegate(this.Menu1));
		Button expr_4D2 = this.m_TalkButton[5].m_NPCTalk_menutitle;
		expr_4D2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4D2.Click, new EZValueChangedDelegate(this.Menu1));
		Button expr_4F9 = this.m_NPCTalk_close;
		expr_4F9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4F9.Click, new EZValueChangedDelegate(this.BtnClickNext));
		this.m_NPCTalk_close.Data = true;
		Button expr_531 = this.m_NPCTalk_transbutton;
		expr_531.Click = (EZValueChangedDelegate)Delegate.Combine(expr_531.Click, new EZValueChangedDelegate(this.BtnClickNext));
		this.m_NPCTalk_transbutton.UseDefaultSound = false;
		this.m_SelectTalkArrow = (base.GetControl("DrawTexture_SelectTalkArrow") as DrawTexture);
		this.m_SelectTalkArrow.Visible = false;
		this.m_QuestImage = (base.GetControl("DrawTexture_Image") as DrawTexture);
		this.m_QuestImage.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickNext));
		this.m_QuestImage.Visible = false;
		this.m_FadeImage = (base.GetControl("DrawTexture_Fade") as DrawTexture);
		this.m_FadeImage.SetSize(GUICamera.width, GUICamera.height);
		this.m_FadeImage.SetLocation(0f, -(GUICamera.height - base.GetSizeY()), -10f);
		this.m_FadeImage.Visible = false;
		this.m_Start = (base.GetControl("Button_StartBTN") as Button);
		this.m_Start.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickStart));
		this.m_Start.Visible = false;
		this.m_CenterNpcName = (base.GetControl("NPCTalk_npcname") as Label);
		this.m_CenterNpcNameBack[0] = (base.GetControl("NPCTalk_nameBGcenter_left_line") as DrawTexture);
		this.m_CenterNpcNameBack[1] = (base.GetControl("NPCTalk_nameBGcenter_right_line") as DrawTexture);
		this.m_CenterNpcNameBack[2] = (base.GetControl("NPCTalk_nameBGcenter_left") as DrawTexture);
		this.m_CenterNpcNameBack[3] = (base.GetControl("NPCTalk_nameBGcenter_right") as DrawTexture);
		this.m_LeftNpcName = (base.GetControl("NPCTalk_npcname_left") as Box);
		this.m_LeftNpcNameBack = (base.GetControl("NPCTalk_nameBGleft") as DrawTexture);
		this.m_LeftNpcNameBackLine = (base.GetControl("NPCTalk_nameBGleft_line") as DrawTexture);
		this.m_LeftNpcName.Visible = false;
		this.m_LeftNpcNameBack.Visible = false;
		this.m_LeftNpcNameBackLine.Visible = false;
		this.m_RightNpcName = (base.GetControl("NPCTalk_npcname_right") as Box);
		this.m_RightNpcNameBack = (base.GetControl("NPCTalk_nameBGright") as DrawTexture);
		this.m_RightNpcNameBackLine = (base.GetControl("NPCTalk_nameBGright_line") as DrawTexture);
		this.m_RightNpcName.Visible = false;
		this.m_RightNpcNameBack.Visible = false;
		this.m_RightNpcNameBackLine.Visible = false;
		this.m_CenterNpcImage = (base.GetControl("DrawTexture_NPCFace01") as DrawTexture);
		this.m_CenterNpcImage.Visible = false;
		this.m_LeftNpcImage = (base.GetControl("DrawTexture_NPCFace01_left") as DrawTexture);
		this.m_LeftNpcImage.Visible = false;
		this.m_RightNpcImage = (base.GetControl("DrawTexture_NPCFace01_right") as DrawTexture);
		this.m_RightNpcImage.Visible = false;
		this.ReLocation();
	}

	public void VisibleNpcName(QUEST_DLG_INFO questInfo)
	{
		if (string.Empty != questInfo.QuestDlgCharCode)
		{
			NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(questInfo.QuestDlgCharCode);
			if (charKindInfoFromCode == null)
			{
				return;
			}
			string text = string.Empty;
			int kind = 0;
			NrCharKindInfo charKindInfoFromCode2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(questInfo.QuestDlgCharCode2);
			if (charKindInfoFromCode2 != null)
			{
				text = charKindInfoFromCode2.GetName();
				kind = charKindInfoFromCode2.GetCharKind();
			}
			int charKind = charKindInfoFromCode.GetCharKind();
			string text2 = charKindInfoFromCode.GetName();
			if (questInfo.bTalkUser)
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null)
				{
					text2 = @char.GetCharName();
					charKind = @char.GetCharKindInfo().GetCharKind();
				}
				else if (questInfo.nNpcName != 0)
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo(questInfo.nNpcName.ToString());
				}
			}
			else if (questInfo.nNpcName != 0)
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo(questInfo.nNpcName.ToString());
			}
			if (questInfo.ePosition == QUEST_DLG_INFO.Alignment.LEFT)
			{
				this.m_RightNpcImage.Visible = false;
				this.m_RightNpcName.Visible = false;
				this.m_RightNpcNameBack.Visible = false;
				this.m_CenterNpcImage.Visible = false;
				this.m_CenterNpcName.Visible = false;
				this.m_CenterNpcNameBack[0].Visible = false;
				this.m_CenterNpcNameBack[1].Visible = false;
				this.m_CenterNpcNameBack[2].Visible = false;
				this.m_CenterNpcNameBack[3].Visible = false;
				this.m_LeftNpcImage.SetTexture(eCharImageType.LARGE, charKind, -1);
				this.m_LeftNpcImage.Visible = true;
				if (questInfo.bShowName)
				{
					this.m_LeftNpcName.Visible = true;
					this.m_LeftNpcName.Text = text2;
				}
				else
				{
					this.m_LeftNpcName.Visible = false;
				}
				this.m_LeftNpcNameBack.Visible = true;
				this.m_LeftNpcNameBackLine.Visible = true;
			}
			else if (questInfo.ePosition == QUEST_DLG_INFO.Alignment.RIGHT)
			{
				this.m_LeftNpcImage.Visible = false;
				this.m_LeftNpcName.Visible = false;
				this.m_LeftNpcNameBack.Visible = false;
				this.m_CenterNpcImage.Visible = false;
				this.m_CenterNpcName.Visible = false;
				this.m_CenterNpcNameBack[0].Visible = false;
				this.m_CenterNpcNameBack[1].Visible = false;
				this.m_CenterNpcNameBack[2].Visible = false;
				this.m_CenterNpcNameBack[3].Visible = false;
				this.m_RightNpcImage.SetTexture(eCharImageType.LARGE, charKind, -1);
				this.m_RightNpcImage.Visible = true;
				if (questInfo.bShowName)
				{
					this.m_RightNpcName.Visible = true;
					this.m_RightNpcName.Text = text2;
				}
				else
				{
					this.m_RightNpcName.Visible = false;
				}
				this.m_RightNpcNameBack.Visible = true;
				this.m_RightNpcNameBackLine.Visible = true;
			}
			else if (questInfo.ePosition == QUEST_DLG_INFO.Alignment.CENTER)
			{
				this.m_LeftNpcImage.Visible = false;
				this.m_LeftNpcName.Visible = false;
				this.m_LeftNpcNameBack.Visible = false;
				this.m_LeftNpcNameBackLine.Visible = false;
				this.m_RightNpcImage.Visible = false;
				this.m_RightNpcName.Visible = false;
				this.m_RightNpcNameBack.Visible = false;
				this.m_RightNpcNameBackLine.Visible = false;
				this.m_CenterNpcImage.SetTexture(eCharImageType.LARGE, charKind, -1);
				this.m_CenterNpcImage.Visible = true;
				this.m_CenterNpcNameBack[0].Visible = true;
				this.m_CenterNpcNameBack[1].Visible = true;
				this.m_CenterNpcNameBack[2].Visible = true;
				this.m_CenterNpcNameBack[3].Visible = true;
				if (questInfo.bShowName)
				{
					this.m_CenterNpcName.Visible = true;
					this.m_CenterNpcName.Text = text2;
				}
				else
				{
					this.m_CenterNpcName.Visible = false;
				}
			}
			else if (questInfo.ePosition == QUEST_DLG_INFO.Alignment.NONE)
			{
				this.m_LeftNpcImage.Visible = false;
				this.m_LeftNpcName.Visible = false;
				this.m_LeftNpcNameBack.Visible = false;
				this.m_RightNpcImage.Visible = false;
				this.m_RightNpcName.Visible = false;
				this.m_RightNpcNameBack.Visible = false;
				this.m_CenterNpcImage.Visible = false;
				this.m_CenterNpcName.Visible = false;
				this.m_CenterNpcNameBack[0].Visible = false;
				this.m_CenterNpcNameBack[1].Visible = false;
				this.m_CenterNpcNameBack[2].Visible = false;
				this.m_CenterNpcNameBack[3].Visible = false;
			}
			if (questInfo.ePosition2 == QUEST_DLG_INFO.Alignment.LEFT)
			{
				this.m_LeftNpcImage.SetTexture(eCharImageType.LARGE, kind, -1);
				this.m_LeftNpcImage.Visible = true;
				if (questInfo.bShowName2)
				{
					this.m_LeftNpcName.Visible = true;
					this.m_LeftNpcName.Text = text;
				}
				else
				{
					this.m_LeftNpcName.Visible = false;
				}
				this.m_LeftNpcNameBack.Visible = true;
				this.m_LeftNpcNameBackLine.Visible = true;
			}
			else if (questInfo.ePosition2 == QUEST_DLG_INFO.Alignment.RIGHT)
			{
				this.m_RightNpcImage.SetTexture(eCharImageType.LARGE, kind, -1);
				this.m_RightNpcImage.Visible = true;
				if (questInfo.bShowName2)
				{
					this.m_RightNpcName.Visible = true;
					this.m_RightNpcName.Text = text;
				}
				else
				{
					this.m_RightNpcName.Visible = false;
				}
				this.m_RightNpcNameBack.Visible = true;
				this.m_RightNpcNameBackLine.Visible = true;
			}
		}
	}

	public void QuestTalkOption(QUEST_DLG_INFO questInfo)
	{
		if (questInfo.IsDLGOption(1L))
		{
			this.m_CenterNpcImage.Visible = false;
			this.m_LeftNpcImage.Visible = false;
			this.m_RightNpcImage.Visible = false;
		}
		if (questInfo.IsDLGOption(2L))
		{
			if (null != this.m_WorldCamera)
			{
				this.m_vStartPos = this.m_WorldCamera.transform.position;
			}
			this.SetShake();
		}
		if (questInfo.IsDLGOption(8L))
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", questInfo.strSound, new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public void ClickStart(IUIObject obj)
	{
		this.BtnClickAccept(null);
	}

	public void SetTalkText(string strText)
	{
		this.m_NPCTalk_talklabel.Visible = true;
		this.m_NPCTalk_transbutton.Visible = true;
		if (this.m_NPCTalk_talklabel.SetFlashLabel(strText))
		{
			this.m_NPCTalk_close.Visible = true;
		}
		else
		{
			this.m_NPCTalk_close.Visible = true;
		}
	}

	public override void OnClose()
	{
		this.Clear();
	}

	public void Clear()
	{
		this.m_i32DlgCount = 1;
		NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
		NrTSingleton<NkCharManager>.Instance.ShowHideAll(true, true, true);
		NrTSingleton<NkQuestManager>.Instance.ReleaseQuestCamera();
		if (null != this.m_WorldCamera)
		{
			this.m_WorldCamera.RestoreCameraInfo();
		}
		NrTSingleton<NkQuestManager>.Instance.CloseAllReward();
	}

	public void SetNpcID(int i32NpcKind, short i16CharUnique)
	{
		if (this.m_bSet)
		{
			return;
		}
		this.m_bSet = true;
		this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_START);
		this.m_bBackCamera = false;
		this.m_NPCTalk_talklabel.Visible = true;
		this.m_NPCTalk_transbutton.Visible = true;
		this.m_NPCTalk_close.Visible = true;
		this.m_TalkButton[0].Show(false);
		this.m_TalkButton[1].Show(false);
		this.m_TalkButton[2].Show(false);
		this.m_TalkButton[3].Show(false);
		this.m_TalkButton[4].Show(false);
		this.m_TalkButton[5].Show(false);
		this.m_DrawTexture_line1.Visible = false;
		this.m_DrawTexture_line2.Visible = false;
		this.m_DrawTexture_line3.Visible = false;
		this.m_DrawTexture_line4.Visible = false;
		this.m_DrawTexture_line5.Visible = false;
		this.m_DrawTexture_line6.Visible = false;
		this.m_CurNpc = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32NpcKind);
		NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(30, (long)i32NpcKind, 1L);
		NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(8, (long)i32NpcKind, 1L);
		NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(99, (long)i32NpcKind, 1L);
		NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(48, (long)i32NpcKind, 1L);
		this.m_16CurCharUnique = i16CharUnique;
		NrTSingleton<CNpcUIManager>.Instance.AddNpc(this.m_CurNpc);
		this.m_NpcUI = NrTSingleton<CNpcUIManager>.Instance.GetNpcUIByNpcKind(i32NpcKind);
		NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(i16CharUnique);
		if (charByCharUnique != null)
		{
			charByCharUnique.Get3DChar().FadeOutIllumination();
		}
		this.m_bClickedMoveNPC = false;
		if (this.m_CurNpc != null)
		{
			base.SetSize(GUICamera.width, base.GetSizeY());
			base.SetLocation(0f, GUICamera.height - base.GetSizeY());
			this.m_NPCTalk_BG.SetSize(GUICamera.width, this.m_NPCTalk_BG.GetSize().y);
			this.m_NPCTalk_close.SetLocation(GUICamera.width - this.m_NPCTalk_close.GetSize().x - 20f, this.firstPosY1);
			this.m_Start.SetLocation(GUICamera.width - this.m_Start.GetSize().x - 15f, this.m_Start.GetLocationY());
			this.m_CenterNpcImage.Visible = true;
			float num = 0f;
			if (!this.m_bBackCamera)
			{
				NrTSingleton<NkClientLogic>.Instance.BackMainCameraInfo();
				this.m_bBackCamera = true;
			}
			if (null != Camera.main)
			{
				this.m_WorldCamera = Camera.main.GetComponent<maxCamera>();
				if (this.m_WorldCamera != null)
				{
					this.m_WorldCamera.StopCameraControl();
				}
			}
			for (int i = 0; i < 6; i++)
			{
				this.m_TalkButton[i].m_Label_menutitle_font.SetLocation(this.m_TalkButton[i].m_Label_menutitle_font.GetLocation().x + num, this.m_TalkButton[i].m_Label_menutitle_font.GetLocationY());
				this.m_TalkButton[i].m_NPCTalk_menuicon.SetLocation(this.m_TalkButton[i].m_NPCTalk_menuicon.GetLocation().x + num, this.m_TalkButton[i].m_NPCTalk_menuicon.GetLocationY());
				this.m_TalkButton[i].m_NPCTalk_menutitle.SetLocation(this.m_TalkButton[i].m_NPCTalk_menutitle.GetLocation().x + num, this.m_TalkButton[i].m_NPCTalk_menutitle.GetLocationY());
			}
			this.m_CenterNpcName.SetLocation(this.m_CenterNpcName.GetLocation().x + num, this.m_CenterNpcName.GetLocationY());
			this.m_NPCTalk_talklabel.SetLocation(this.m_NPCTalk_talklabel.GetLocation().x + num, this.m_NPCTalk_talklabel.GetLocationY());
			this.m_NPCTalk_transbutton.SetLocation(this.m_NPCTalk_transbutton.GetLocation().x + num, this.m_NPCTalk_transbutton.GetLocationY());
			this.m_UserAnswer.SetLocation(this.m_UserAnswer.GetLocation().x + num, this.m_UserAnswer.GetLocationY() - 10f);
			this.m_SelectTalkArrow.SetLocation(this.m_SelectTalkArrow.GetLocation().x + num, this.m_SelectTalkArrow.GetLocationY());
			this.SetQuestMenuItem(i32NpcKind);
			this.SetNPCMenuItem(i32NpcKind);
		}
		this.OnlySound("NPC", "GREETING");
		if (this.m_i32MenuCount == 0)
		{
			this.m_CenterNpcName.Text = this.m_CurNpc.GetName();
			this.m_CenterNpcImage.SetTexture(eCharImageType.LARGE, i32NpcKind, -1);
			this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_END);
			if (charByCharUnique != null)
			{
				charByCharUnique.SetShowHide3DModel(true, false, false);
			}
			if (this.m_CurNpc.GetCHARKIND_NPCINFO() != null)
			{
				this.SetTalkText(NrTSingleton<CNpcUIManager>.Instance.GetTextGreeting(this.m_CurNpc));
			}
		}
		else if (0 < this.m_QuestMenu.Count)
		{
			if (1 < this.m_QuestMenu.Count)
			{
				this.m_CenterNpcName.Text = this.m_CurNpc.GetName();
				this.m_CenterNpcImage.SetTexture(eCharImageType.LARGE, i32NpcKind, -1);
			}
			else if (0 < this.m_QuestMenu.Count && 0 < this.m_NpcUIMenu.Count)
			{
				this.m_CenterNpcName.Text = this.m_CurNpc.GetName();
				this.m_CenterNpcImage.SetTexture(eCharImageType.LARGE, i32NpcKind, -1);
			}
			this.BtnClickNext(null);
		}
		else
		{
			this.m_CenterNpcName.Text = this.m_CurNpc.GetName();
			this.m_CenterNpcImage.SetTexture(eCharImageType.LARGE, i32NpcKind, -1);
			if (this.m_CurNpc.GetCHARKIND_NPCINFO() != null)
			{
				this.SetTalkText(NrTSingleton<CNpcUIManager>.Instance.GetTextGreeting(this.m_CurNpc));
			}
		}
	}

	public int GetNPCKind()
	{
		return this.m_CurNpc.GetCharKind();
	}

	private void SetMenu()
	{
		this.m_TalkButton[0].Show(false);
		this.m_TalkButton[1].Show(false);
		this.m_TalkButton[2].Show(false);
		this.m_TalkButton[3].Show(false);
		this.m_TalkButton[4].Show(false);
		this.m_TalkButton[5].Show(false);
		this.m_DrawTexture_line1.Visible = true;
		this.m_DrawTexture_line2.Visible = true;
		this.m_DrawTexture_line3.Visible = true;
		this.m_DrawTexture_line4.Visible = true;
		this.m_DrawTexture_line5.Visible = true;
		this.m_DrawTexture_line6.Visible = true;
		if (this.m_i32MenuCount <= 6)
		{
			for (int i = 0; i < this.m_QuestMenu.Count; i++)
			{
				NPC_TALK_QUEST_STATE nPC_TALK_QUEST_STATE = this.m_QuestMenu[i];
				this.m_TalkButton[i].m_NPCTalk_menutitle.Data = nPC_TALK_QUEST_STATE;
				this.m_TalkButton[i].m_NPCTalk_menutitle.Text = string.Empty;
				this.m_TalkButton[i].m_NPCTalk_menuicon.SetTexture(nPC_TALK_QUEST_STATE.strIconPath);
				this.m_TalkButton[i].m_Label_menutitle_font.SetText(nPC_TALK_QUEST_STATE.strTitle);
				this.m_TalkButton[i].Show(true);
			}
			int count = this.m_QuestMenu.Count;
			for (int i = 0; i < this.m_NpcUIMenu.Count; i++)
			{
				NPC_TALK_QUEST_STATE nPC_TALK_QUEST_STATE2 = this.m_NpcUIMenu[i];
				this.m_TalkButton[i + count].m_NPCTalk_menutitle.Data = nPC_TALK_QUEST_STATE2;
				this.m_TalkButton[i + count].m_NPCTalk_menutitle.Text = string.Empty;
				this.m_TalkButton[i + count].m_NPCTalk_menuicon.SetTexture(nPC_TALK_QUEST_STATE2.strIconPath);
				this.m_TalkButton[i + count].m_Label_menutitle_font.SetText(nPC_TALK_QUEST_STATE2.strTitle);
				this.m_TalkButton[i + count].Show(true);
			}
		}
		else
		{
			int j;
			for (j = 0; j < this.m_NpcUIMenu.Count; j++)
			{
				NPC_TALK_QUEST_STATE nPC_TALK_QUEST_STATE3 = this.m_NpcUIMenu[j];
				if (nPC_TALK_QUEST_STATE3.eNpcUIType != NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST)
				{
					this.m_TalkButton[j].m_NPCTalk_menutitle.Data = nPC_TALK_QUEST_STATE3;
					this.m_TalkButton[j].m_NPCTalk_menutitle.Text = string.Empty;
					this.m_TalkButton[j].m_NPCTalk_menuicon.SetTexture(nPC_TALK_QUEST_STATE3.strIconPath);
					this.m_TalkButton[j].m_Label_menutitle_font.SetText(nPC_TALK_QUEST_STATE3.strTitle);
					this.m_TalkButton[j].Show(true);
				}
			}
			NPC_TALK_QUEST_STATE nPC_TALK_QUEST_STATE4 = new NPC_TALK_QUEST_STATE();
			nPC_TALK_QUEST_STATE4.eNpcUIType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST_SUB;
			this.m_TalkButton[j].m_NPCTalk_menutitle.Data = nPC_TALK_QUEST_STATE4;
			this.m_TalkButton[j].m_NPCTalk_menutitle.Text = string.Empty;
			this.m_TalkButton[j].m_Label_menutitle_font.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("417"));
			this.m_TalkButton[j].Show(true);
		}
		this.m_NPCTalk_talklabel.Visible = false;
		this.m_NPCTalk_transbutton.Visible = false;
	}

	private void SetQuestMenu()
	{
		this.m_TalkButton[0].Show(false);
		this.m_TalkButton[1].Show(false);
		this.m_TalkButton[2].Show(false);
		this.m_TalkButton[3].Show(false);
		this.m_TalkButton[4].Show(false);
		this.m_TalkButton[5].Show(false);
		int num = 0;
		for (int i = 0; i < this.m_QuestMenu.Count; i++)
		{
			if (i >= (int)((this.m_i16CurrentPage - 1) * 6) && i < (int)(this.m_i16CurrentPage * 6) && num >= 0 && num < 6)
			{
				NPC_TALK_QUEST_STATE nPC_TALK_QUEST_STATE = this.m_QuestMenu[i];
				this.m_TalkButton[num].m_NPCTalk_menutitle.Data = nPC_TALK_QUEST_STATE;
				this.m_TalkButton[num].m_NPCTalk_menutitle.Text = string.Empty;
				this.m_TalkButton[num].m_NPCTalk_menuicon.SetTexture(nPC_TALK_QUEST_STATE.strIconPath);
				this.m_TalkButton[num].m_Label_menutitle_font.SetText(nPC_TALK_QUEST_STATE.strTitle);
				this.m_TalkButton[num].Show(true);
				num++;
			}
		}
		this.m_NPCTalk_talklabel.Visible = false;
		this.m_NPCTalk_transbutton.Visible = false;
	}

	private void SetNPCMenuItem(int i32NPCKind)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32NPCKind);
		if (charKindInfo != null)
		{
			NrTSingleton<CNpcUIManager>.Instance.AddNpc(charKindInfo);
		}
		this.m_NpcUI = NrTSingleton<CNpcUIManager>.Instance.GetNpcUIByNpcKind(i32NPCKind);
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (this.m_NpcUI != null && nrCharUser != null)
		{
			for (int i = 0; i < 6; i++)
			{
				if (string.Empty != this.m_NpcUI.m_kMenu[i].strMenu && nrCharUser.GetPersonInfoUser().GetLevel(0L) >= (int)this.m_NpcUI.m_kMenu[i].nLimintLevel)
				{
					if (charKindInfo.IsATB(512L))
					{
						if (NrTSingleton<ContentsLimitManager>.Instance.IsPointExchage())
						{
							this.SetItemInfo(i, this.m_NpcUI.m_kMenu[i].byMenuType, null, QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE, this.m_NpcUI.m_kMenu[i].strMenu, this.m_NpcUI.m_kMenu[i].strIconPath);
						}
					}
					else
					{
						this.SetItemInfo(i, this.m_NpcUI.m_kMenu[i].byMenuType, null, QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE, this.m_NpcUI.m_kMenu[i].strMenu, this.m_NpcUI.m_kMenu[i].strIconPath);
					}
				}
				else if (charKindInfo.IsATB(512L) && nrCharUser.GetPersonInfoUser().GetLevel(0L) < (int)this.m_NpcUI.m_kMenu[i].nLimintLevel)
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("659"),
						"count",
						this.m_NpcUI.m_kMenu[i].nLimintLevel
					});
					Main_UI_SystemMessage.ADDMessage(empty);
					break;
				}
			}
		}
	}

	private void SetQuestMenuItem(int i32NPCKind)
	{
		NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
		if (instance == null)
		{
			return;
		}
		NPC_QUEST_LIST npcQuestMatchInfo = instance.GetNpcQuestMatchInfo(i32NPCKind);
		string text = string.Empty;
		if (npcQuestMatchInfo != null)
		{
			for (int i = 0; i < npcQuestMatchInfo.NpcQuestList.Count; i++)
			{
				text = npcQuestMatchInfo.NpcQuestList[i].strQuestUnique;
				CQuest questByQuestUnique = instance.GetQuestByQuestUnique(text);
				if (questByQuestUnique != null)
				{
					QUEST_CONST.eQUESTSTATE questState = instance.GetQuestState(text);
					if (questState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE && questState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_END && questState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE && questState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_NOT_VIEW)
					{
						string textFromQuest_Title = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Title(questByQuestUnique.GetQuestCommon().strTextKey);
						if (questByQuestUnique.IsDayQuest())
						{
							if (instance.CanDayQuest(text))
							{
								this.SetItemInfo(99, NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST, questByQuestUnique, questState, textFromQuest_Title, string.Empty);
							}
						}
						else
						{
							this.SetItemInfo(99, NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST, questByQuestUnique, questState, textFromQuest_Title, string.Empty);
						}
					}
				}
			}
		}
		USER_CURRENT_QUEST_INFO[] userCurrentQuestInfo = instance.GetUserCurrentQuestInfo();
		if (userCurrentQuestInfo != null)
		{
			for (int j = 0; j < 10; j++)
			{
				if (userCurrentQuestInfo[j].strQuestUnique != null)
				{
					string strQuestUnique = userCurrentQuestInfo[j].strQuestUnique;
					if (strQuestUnique != string.Empty)
					{
						CQuest questByQuestUnique2 = instance.GetQuestByQuestUnique(strQuestUnique);
						if (questByQuestUnique2 != null && questByQuestUnique2.GetQuestCommon().i32EndType == 0 && questByQuestUnique2.GetQuestCommon().i64EndTypeVal == (long)i32NPCKind && instance.CheckQuestComplete(strQuestUnique, userCurrentQuestInfo[j]))
						{
							string textFromQuest_Title2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Title(questByQuestUnique2.GetQuestCommon().strTextKey);
							this.SetItemInfo(99, NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST, questByQuestUnique2, QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE, textFromQuest_Title2, string.Empty);
						}
					}
				}
			}
		}
	}

	private void SetItemInfo(int bMenuNum, NPC_UI.E_NPC_UI_TYPE eNpcUIType, CQuest cQuest, QUEST_CONST.eQUESTSTATE state, string strText, string strIcon)
	{
		for (int i = 0; i < this.m_QuestMenu.Count; i++)
		{
			NPC_TALK_QUEST_STATE nPC_TALK_QUEST_STATE = this.m_QuestMenu[i];
			if (nPC_TALK_QUEST_STATE != null)
			{
				if (cQuest == null)
				{
					break;
				}
				if (nPC_TALK_QUEST_STATE.kQuest.GetQuestCommon().strQuestUnique.Equals(cQuest.GetQuestCommon().strQuestUnique))
				{
					return;
				}
			}
		}
		bool flag = false;
		if (cQuest != null)
		{
			flag = NrTSingleton<NkQuestManager>.Instance.IsDayQuest(cQuest.GetQuestUnique());
		}
		NPC_TALK_QUEST_STATE nPC_TALK_QUEST_STATE2 = new NPC_TALK_QUEST_STATE();
		nPC_TALK_QUEST_STATE2.strTitle = strText;
		nPC_TALK_QUEST_STATE2.eNpcUIType = eNpcUIType;
		nPC_TALK_QUEST_STATE2.eState = state;
		nPC_TALK_QUEST_STATE2.kQuest = cQuest;
		nPC_TALK_QUEST_STATE2.bMenuNum = bMenuNum;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("119");
		string str = string.Empty;
		switch (state)
		{
		case QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE:
			str = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");
			if (flag)
			{
				nPC_TALK_QUEST_STATE2.strIconPath = "NPC_I_QuestI23";
			}
			else
			{
				nPC_TALK_QUEST_STATE2.strIconPath = "NPC_I_QuestI21";
			}
			nPC_TALK_QUEST_STATE2.strTitle = str + strText;
			break;
		case QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING:
			str = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");
			nPC_TALK_QUEST_STATE2.strIconPath = "NPC_I_QuestI12";
			nPC_TALK_QUEST_STATE2.strTitle = str + strText;
			break;
		case QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE:
			str = NrTSingleton<CTextParser>.Instance.GetTextColor("1106");
			if (flag)
			{
				nPC_TALK_QUEST_STATE2.strIconPath = "NPC_I_QuestI13";
			}
			else
			{
				nPC_TALK_QUEST_STATE2.strIconPath = "NPC_I_QuestI11";
			}
			nPC_TALK_QUEST_STATE2.strTitle = str + strText + textFromInterface;
			break;
		default:
			nPC_TALK_QUEST_STATE2.strIconPath = strIcon;
			break;
		}
		if (eNpcUIType == NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST)
		{
			this.m_QuestMenu.Add(nPC_TALK_QUEST_STATE2);
		}
		else
		{
			this.m_NpcUIMenu.Add(nPC_TALK_QUEST_STATE2);
		}
	}

	public override void Update()
	{
		this.UpdateShakeCamera();
		if (this.m_bClickedMoveNPC)
		{
			this.CheckMoveNpc();
		}
		if (this.loadQuestImage && null != this.questImage)
		{
			this.questImage.wrapMode = TextureWrapMode.Clamp;
			this.m_QuestImage.Visible = true;
			this.m_QuestImage.SetSize(GUICamera.width, GUICamera.height);
			this.m_QuestImage.SetLocation(0f, -base.GetLocationY(), this.m_QuestImage.GetLocation().z - 1f);
			this.m_QuestImage.SetTexture(this.questImage);
			this.questImage = null;
			this.loadQuestImage = false;
			this.currentDlgInfo = null;
		}
		if (Input.GetKeyUp(KeyCode.Escape) && !this.m_bAutoQuest)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.QUESTGIVEITEM_DLG);
			this.Clear();
			this.Close();
		}
		if (!BaseNet_Game.GetInstance().IsSocketConnected() || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WAIT_DLG))
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("591");
			Main_UI_SystemMessage.ADDMessage(textFromNotify);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.QUESTGIVEITEM_DLG);
			this.Clear();
			this.Close();
		}
	}

	public void ReLocation()
	{
		base.SetSize(GUICamera.width, base.GetSizeY());
		base.SetLocation(0f, GUICamera.height - base.GetSizeY());
		this.m_NPCTalk_BG.SetSize(GUICamera.width, this.m_NPCTalk_BG.GetSize().y);
		float x = (GUICamera.width - this.m_NPCTalk_talklabel.width) / 2f;
		this.m_NPCTalk_talklabel.SetLocation(x, this.m_NPCTalk_talklabel.GetLocationY());
		this.m_NPCTalk_transbutton.SetLocation(x, this.m_NPCTalk_transbutton.GetLocationY());
		this.m_UserAnswer.SetLocation(x, this.m_UserAnswer.GetLocationY());
		x = (GUICamera.width - this.m_CenterNpcImage.GetSize().x) / 2f;
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
		x = GUICamera.width - this.m_RightNpcName.GetSize().x;
		this.m_RightNpcName.SetLocation(x, this.m_RightNpcName.GetLocationY());
		x = GUICamera.width - this.m_RightNpcImage.GetSize().x;
		this.m_RightNpcImage.SetLocation(x, this.m_RightNpcImage.GetLocationY());
		x = GUICamera.width;
		this.m_RightNpcNameBack.SetLocation(x, this.m_RightNpcNameBack.GetLocationY());
		x = GUICamera.width;
		this.m_RightNpcNameBackLine.SetLocation(x, this.m_RightNpcNameBackLine.GetLocationY());
	}

	public override void ChangedResolution()
	{
		this.ReLocation();
	}

	public void BtnClickNext(IUIObject obj)
	{
		bool skip = false;
		if (obj != null && obj.Data != null && obj.Data is bool)
		{
			skip = (bool)obj.Data;
		}
		if (NrTSingleton<NkQuestManager>.Instance.RewardShow)
		{
			NrTSingleton<NkQuestManager>.Instance.AddDebugMsg("NrQuestSystem.Instance.RewardShow == true");
			return;
		}
		if (this.m_FadeImage.Visible)
		{
			return;
		}
		if (this.m_UserAnswer.Visible && (Button)obj == this.m_NPCTalk_transbutton)
		{
			return;
		}
		if (this.m_Step == E_NPC_TALK_STEP.E_NPC_TALK_STEP_START)
		{
			this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_MENU);
			this.SetMenu();
			if (this.m_QuestMenu.Count == 1 && this.m_NpcUIMenu.Count == 0)
			{
				for (int i = 0; i < 6; i++)
				{
					this.m_TalkButton[i].Show(false);
				}
				if (!this.m_bAutoQuest)
				{
					this.AutoClickButton();
				}
			}
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "NEXT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else if (this.m_Step == E_NPC_TALK_STEP.E_NPC_TALK_STEP_MENU)
		{
			this.BtnClickExit(null);
		}
		else if (this.m_Step == E_NPC_TALK_STEP.E_NPC_TALK_STEP_TALK)
		{
			if (this.m_State.eNpcUIType == NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST)
			{
				this.ChangeDlgText(skip);
			}
			else if (this.m_State.eNpcUIType == NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE_POPUP)
			{
				if (this.m_NpcUI.NpcDo())
				{
					this.BtnClickExit(null);
				}
				else
				{
					this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_END);
				}
			}
		}
		else if (this.m_Step == E_NPC_TALK_STEP.E_NPC_TALK_STEP_REWARD)
		{
			this.BtnClickAccept(null);
		}
		else if (this.m_Step == E_NPC_TALK_STEP.E_NPC_TALK_STEP_END || this.m_Step == E_NPC_TALK_STEP.E_NPC_TALK_STEP_ACCEPT)
		{
			this.BtnClickExit(null);
		}
		else
		{
			if (this.m_Step != E_NPC_TALK_STEP.E_NPC_TALK_STEP_COMPETE)
			{
				if (NrTSingleton<NkQuestManager>.Instance.Request && 0f < NrTSingleton<NkQuestManager>.Instance.RequestTime && Time.realtimeSinceStartup - NrTSingleton<NkQuestManager>.Instance.RequestTime >= 15f)
				{
					GS_QUEST_INFO gS_QUEST_INFO = new GS_QUEST_INFO();
					gS_QUEST_INFO.nType = 1;
					SendPacket.GetInstance().SendObject(1003, gS_QUEST_INFO);
					NrTSingleton<NkQuestManager>.Instance.Request = false;
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("591");
					Main_UI_SystemMessage.ADDMessage(textFromNotify);
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.QUESTGIVEITEM_DLG);
					this.Clear();
					this.Close();
				}
				return;
			}
			if (this.m_State.kQuest == null)
			{
				return;
			}
			CQuest nextQuest = NrTSingleton<NkQuestManager>.Instance.GetNextQuest(this.m_State.kQuest.GetQuestUnique());
			if (((nextQuest != null && this.m_State.kQuest.GetQuestCommon().i32EndType == 0) || (nextQuest != null && this.m_State.kQuest.GetQuestCommon().i32EndType == 2)) && NrTSingleton<NkQuestManager>.Instance.GetQuestState(nextQuest.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
			{
				CQuestGroup questGroupByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(this.m_State.kQuest.GetQuestUnique());
				CQuestGroup questGroupByQuestUnique2 = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(nextQuest.GetQuestUnique());
				if (questGroupByQuestUnique != null && questGroupByQuestUnique2 != null)
				{
					this.m_State.kQuest = nextQuest;
					this.m_State.eState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(nextQuest.GetQuestUnique());
					this.m_i32DlgCount = 1;
					this.m_strDlgIndex = NpcTalkUI_DLG.QuestDlgIndex(this.m_State.kQuest, this.m_State.eState);
					this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_TALK);
					this.ChangeDlgText(false);
				}
				else
				{
					this.Clear();
					this.Close();
				}
			}
			else if (nextQuest != null && NrTSingleton<NkQuestManager>.Instance.GetQuestState(nextQuest.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE)
			{
				string message = nextQuest.GetQuestTitle() + " 이미 완료된 퀘스트입니다.";
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				this.Clear();
				this.Close();
			}
			else
			{
				this.Clear();
				this.Close();
			}
		}
	}

	public void OnAccept(object a_oObject)
	{
		int num = (int)a_oObject;
		USER_CURRENT_QUEST_INFO uSER_CURRENT_QUEST_INFO = null;
		if (num == 1)
		{
			uSER_CURRENT_QUEST_INFO = NrTSingleton<NkQuestManager>.Instance.GetCurrentMainQuest();
		}
		else if (num == 2)
		{
			uSER_CURRENT_QUEST_INFO = NrTSingleton<NkQuestManager>.Instance.GetCurrentSubQuest();
			if (uSER_CURRENT_QUEST_INFO == null)
			{
				uSER_CURRENT_QUEST_INFO = NrTSingleton<NkQuestManager>.Instance.GetCurrentDayQuest();
			}
		}
		else if (num == 100)
		{
			uSER_CURRENT_QUEST_INFO = NrTSingleton<NkQuestManager>.Instance.GetCurrentDayQuest();
			if (uSER_CURRENT_QUEST_INFO == null)
			{
				uSER_CURRENT_QUEST_INFO = NrTSingleton<NkQuestManager>.Instance.GetCurrentSubQuest();
			}
		}
		if (uSER_CURRENT_QUEST_INFO != null)
		{
			GS_QUEST_CANCLE_REQ gS_QUEST_CANCLE_REQ = new GS_QUEST_CANCLE_REQ();
			TKString.StringChar(uSER_CURRENT_QUEST_INFO.strQuestUnique, ref gS_QUEST_CANCLE_REQ.strQuestUnique);
			SendPacket.GetInstance().SendObject(1013, gS_QUEST_CANCLE_REQ);
		}
		this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_PREACCEPT);
		GS_QUEST_ACCEPT_REQ gS_QUEST_ACCEPT_REQ = new GS_QUEST_ACCEPT_REQ();
		TKString.StringChar(this.m_State.kQuest.GetQuestUnique(), ref gS_QUEST_ACCEPT_REQ.strQuestUnique);
		gS_QUEST_ACCEPT_REQ.i32Grade = this.m_i32Grade;
		SendPacket.GetInstance().SendObject(1004, gS_QUEST_ACCEPT_REQ);
		NrTSingleton<NkQuestManager>.Instance.Request = true;
	}

	public void OnCancel(object a_oObject)
	{
		this.Clear();
		this.Close();
	}

	private void BtnClickAccept(IUIObject obj)
	{
		if (this.m_State.eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
		{
			CQuestGroup questGroupByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(this.m_State.kQuest.GetQuestUnique());
			if (questGroupByQuestUnique == null)
			{
				return;
			}
			if (1 <= NrTSingleton<NkQuestManager>.Instance.GetCurrentMainQuestCount() && questGroupByQuestUnique.GetQuestType() == 1)
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI == null)
				{
					return;
				}
				msgBoxUI.SetMsg(new YesDelegate(this.OnAccept), questGroupByQuestUnique.GetQuestType(), new NoDelegate(this.OnCancel), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("197"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("176"), eMsgType.MB_OK_CANCEL);
			}
			else if (1 <= NrTSingleton<NkQuestManager>.Instance.GetCurrentSubQuestCount() || 1 <= NrTSingleton<NkQuestManager>.Instance.GetCurrentDayQuestCount())
			{
				if (questGroupByQuestUnique.GetQuestType() == 2)
				{
					MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
					if (msgBoxUI2 == null)
					{
						return;
					}
					msgBoxUI2.SetMsg(new YesDelegate(this.OnAccept), questGroupByQuestUnique.GetQuestType(), new NoDelegate(this.OnCancel), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("197"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("177"), eMsgType.MB_OK_CANCEL);
				}
				else if (questGroupByQuestUnique.GetQuestType() == 100)
				{
					NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
					if (myCharInfo == null)
					{
						return;
					}
					if (0L < myCharInfo.GetCharDetail(5) && (long)questGroupByQuestUnique.GetGroupUnique() != myCharInfo.GetCharDetail(5))
					{
						return;
					}
					MsgBoxUI msgBoxUI3 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
					if (msgBoxUI3 == null)
					{
						return;
					}
					msgBoxUI3.SetMsg(new YesDelegate(this.OnAccept), questGroupByQuestUnique.GetQuestType(), new NoDelegate(this.OnCancel), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("197"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("177"), eMsgType.MB_OK_CANCEL);
				}
				else
				{
					this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_PREACCEPT);
					GS_QUEST_ACCEPT_REQ gS_QUEST_ACCEPT_REQ = new GS_QUEST_ACCEPT_REQ();
					TKString.StringChar(this.m_State.kQuest.GetQuestUnique(), ref gS_QUEST_ACCEPT_REQ.strQuestUnique);
					gS_QUEST_ACCEPT_REQ.i32Grade = this.m_i32Grade;
					SendPacket.GetInstance().SendObject(1004, gS_QUEST_ACCEPT_REQ);
					NrTSingleton<NkQuestManager>.Instance.Request = true;
				}
			}
			else
			{
				this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_PREACCEPT);
				GS_QUEST_ACCEPT_REQ gS_QUEST_ACCEPT_REQ2 = new GS_QUEST_ACCEPT_REQ();
				TKString.StringChar(this.m_State.kQuest.GetQuestUnique(), ref gS_QUEST_ACCEPT_REQ2.strQuestUnique);
				gS_QUEST_ACCEPT_REQ2.i32Grade = this.m_i32Grade;
				SendPacket.GetInstance().SendObject(1004, gS_QUEST_ACCEPT_REQ2);
				NrTSingleton<NkQuestManager>.Instance.Request = true;
			}
		}
		else if (this.m_State.eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING)
		{
			CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_State.kQuest.GetQuestUnique());
			if (questByQuestUnique != null)
			{
				questByQuestUnique.AfterOnGoing();
			}
			this.Clear();
			this.Close();
		}
		else if (this.m_State.eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
		{
			this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_PRECOMPETE);
			if (!(NrTSingleton<FormsManager>.Instance.GetForm(G_ID.QUESTGIVEITEM_DLG) is QuestGiveItemDlg))
			{
				GS_QUEST_COMPLETE_REQ gS_QUEST_COMPLETE_REQ = new GS_QUEST_COMPLETE_REQ();
				TKString.StringChar(this.m_State.kQuest.GetQuestUnique(), ref gS_QUEST_COMPLETE_REQ.strQuestUnique);
				for (int i = 0; i < 3; i++)
				{
					gS_QUEST_COMPLETE_REQ.CompleteItem[i] = NrTSingleton<NkQuestManager>.Instance.CompleteItem[i];
				}
				SendPacket.GetInstance().SendObject(1006, gS_QUEST_COMPLETE_REQ);
				NrTSingleton<NkQuestManager>.Instance.InitCompleteItem();
				NrTSingleton<NkQuestManager>.Instance.Request = true;
			}
		}
		else
		{
			this.Clear();
			this.Close();
		}
	}

	private void WebLoadQuestImage(WWWItem _item, object _param)
	{
		if (_item.isCanceled)
		{
			return;
		}
		if (_item.GetSafeBundle() != null)
		{
			this.questImage = (_item.GetSafeBundle().mainAsset as Texture2D);
			this.loadQuestImage = true;
		}
	}

	public void ChangeDlgText(bool skip = false)
	{
		this.i32MaxCount = NrTSingleton<NkQuestManager>.Instance.GetQuestDlgMaxCount(this.m_strDlgIndex);
		if (this.currentDlgInfo != null && (this.currentDlgInfo.IsDLGOption(64L) || this.currentDlgInfo.IsDLGOption(128L)))
		{
			this.m_FadeImage.Visible = true;
			if (this.currentDlgInfo.IsDLGOption(64L))
			{
				this.m_FadeImage.SetTextureKey("Win_T_BK");
			}
			else
			{
				this.m_FadeImage.SetTextureKey("Win_T_WH");
			}
			FadeSprite.Do(this.m_FadeImage, EZAnimation.ANIM_MODE.FromTo, new Color(0f, 0f, 0f, 0f), new Color(1f, 1f, 1f, 1f), new EZAnimation.Interpolator(EZAnimation.linear), 1f, 0f, null, new EZAnimation.CompletionDelegate(this.FadeImage));
		}
		if (skip)
		{
			this.m_i32DlgCount = this.i32MaxCount;
		}
		this.currentDlgInfo = NrTSingleton<NkQuestManager>.Instance.GetQuestDlgInfo(this.m_strDlgIndex, this.m_i32DlgCount);
		this.SetDlgText(this.currentDlgInfo);
	}

	public void SetDlgText(QUEST_DLG_INFO cQuestDlgInfo)
	{
		if (cQuestDlgInfo == null)
		{
			this.BtnClickAccept(null);
			this.m_i32DlgCount = 1;
			return;
		}
		string strLoadImage = cQuestDlgInfo.strLoadImage;
		if (string.Empty != strLoadImage)
		{
			string arg = "UI/Quest";
			string str = string.Format("{0}/{1}", arg, strLoadImage + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.EffectBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.WebLoadQuestImage), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		else if (cQuestDlgInfo.bImageClose)
		{
			this.m_QuestImage.Visible = false;
		}
		if (this.m_State.eState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
		{
			if (this.m_State.eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.m_State.kQuest.GetQuestCommon().cQuestCondition[i].i32QuestCode == 155)
					{
						MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
						if (msgBoxUI != null)
						{
							string empty = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1533"),
								"targetname",
								NrTSingleton<NrCharKindInfoManager>.Instance.GetName((int)this.m_State.kQuest.GetQuestCommon().cQuestCondition[1].i64Param)
							});
							msgBoxUI.SetMsg(new YesDelegate(NrTSingleton<NkQuestManager>.Instance.OpenQuestBattle), this.m_State.kQuest, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1532"), empty, eMsgType.MB_OK_CANCEL);
							msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("320"));
							msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("321"));
						}
					}
				}
			}
			else if (this.m_State.eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
			{
				for (int j = 0; j < 3; j++)
				{
					if ((this.m_State.kQuest.GetQuestCommon().cQuestCondition[j].i32QuestCode == 7 || this.m_State.kQuest.GetQuestCommon().cQuestCondition[j].i32QuestCode == 8 || this.m_State.kQuest.GetQuestCommon().cQuestCondition[j].i32QuestCode == 48 || this.m_State.kQuest.GetQuestCommon().cQuestCondition[j].i32QuestCode == 107) && this.m_QuestGiveItemDlg == null && this.m_i32DlgCount == 1)
					{
						this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_PRECOMPETE);
						this.m_QuestGiveItemDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.QUESTGIVEITEM_DLG) as QuestGiveItemDlg);
						this.m_QuestGiveItemDlg.SetTalkState(this.m_State);
						this.m_QuestGiveItemDlg.Show();
					}
				}
			}
		}
		if (cQuestDlgInfo != null && "event" == cQuestDlgInfo.QuestDlgCharCode)
		{
			this.m_i32DlgCount++;
			NrQuestDlgCurrentInfo nrQuestDlgCurrentInfo = new NrQuestDlgCurrentInfo();
			nrQuestDlgCurrentInfo.bCheck = E_EVENT_TYPE.E_EVENT_TYPE_QUEST;
			nrQuestDlgCurrentInfo.strDlgIndex = this.m_strDlgIndex;
			nrQuestDlgCurrentInfo.iDlg32Count = this.m_i32DlgCount;
			nrQuestDlgCurrentInfo.state = this.m_State;
			nrQuestDlgCurrentInfo.step = this.m_Step;
			nrQuestDlgCurrentInfo.kCurNpc = this.m_CurNpc;
			nrQuestDlgCurrentInfo.kNpcUI = this.m_NpcUI;
			nrQuestDlgCurrentInfo.i16CharUnique = this.m_16CurCharUnique;
			nrQuestDlgCurrentInfo.i32MenuCount = this.m_i32MenuCount;
			nrQuestDlgCurrentInfo.strCharCode = this.m_CurNpc.GetCode();
			NrTSingleton<NkQuestManager>.Instance.SetQuestDlgInfo(nrQuestDlgCurrentInfo);
			NrTSingleton<NrEventTriggerInfoManager>.Instance.OnTrigger(cQuestDlgInfo.strLang_Idx);
			cQuestDlgInfo = NrTSingleton<NkQuestManager>.Instance.GetQuestDlgInfo(this.m_strDlgIndex, this.m_i32DlgCount);
		}
		else if (cQuestDlgInfo != null && "drama" == cQuestDlgInfo.QuestDlgCharCode)
		{
			this.m_i32DlgCount++;
			NrQuestDlgCurrentInfo nrQuestDlgCurrentInfo2 = new NrQuestDlgCurrentInfo();
			nrQuestDlgCurrentInfo2.bCheck = E_EVENT_TYPE.E_EVENT_TYPE_QUEST;
			nrQuestDlgCurrentInfo2.strDlgIndex = this.m_strDlgIndex;
			nrQuestDlgCurrentInfo2.iDlg32Count = this.m_i32DlgCount;
			nrQuestDlgCurrentInfo2.state = this.m_State;
			nrQuestDlgCurrentInfo2.step = this.m_Step;
			nrQuestDlgCurrentInfo2.kCurNpc = this.m_CurNpc;
			nrQuestDlgCurrentInfo2.kNpcUI = this.m_NpcUI;
			nrQuestDlgCurrentInfo2.i16CharUnique = this.m_16CurCharUnique;
			nrQuestDlgCurrentInfo2.i32MenuCount = this.m_i32MenuCount;
			nrQuestDlgCurrentInfo2.strCharCode = this.m_CurNpc.GetCode();
			NrTSingleton<NkQuestManager>.Instance.SetQuestDlgInfo(nrQuestDlgCurrentInfo2);
			NrTSingleton<NkQuestManager>.Instance.PlayBundle(cQuestDlgInfo.strLang_Idx);
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BACK_DLG);
			this.Hide();
			cQuestDlgInfo = NrTSingleton<NkQuestManager>.Instance.GetQuestDlgInfo(this.m_strDlgIndex, this.m_i32DlgCount);
		}
		if (cQuestDlgInfo != null)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "NEXT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			string strLang_Idx = cQuestDlgInfo.strLang_Idx;
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				strLang_Idx
			});
			if (string.Empty != cQuestDlgInfo.strUserAnswer)
			{
				this.m_UserAnswer.Visible = true;
				this.m_SelectTalkArrow.Visible = false;
				this.m_NPCTalk_close.Visible = false;
				this.m_UserAnswer.Text = cQuestDlgInfo.strUserAnswer;
				float totalWidth = this.m_UserAnswer.spriteText.TotalWidth;
				this.m_UserAnswer.Setup(totalWidth + 80f, this.m_UserAnswer.GetSize().y);
				if (NrGlobalReference.strLangType.Equals("eng"))
				{
					this.m_UserAnswer.SetLocation(GUICamera.width - 155f - this.m_UserAnswer.width, this.m_UserAnswer.GetLocationY());
				}
				this.m_UserAnswer.Text = cQuestDlgInfo.strUserAnswer;
			}
			else
			{
				this.m_UserAnswer.Visible = false;
				this.m_SelectTalkArrow.Visible = false;
				this.m_NPCTalk_close.Visible = true;
			}
			this.SetTalkText(empty2);
			this.VisibleNpcName(cQuestDlgInfo);
			this.QuestTalkOption(cQuestDlgInfo);
		}
		if (this.i32MaxCount == this.m_i32DlgCount)
		{
			this.m_i32DlgCount = 1;
			if (!this.m_bShowTerritory && this.m_State.eState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
			{
				this.m_Start.Visible = true;
			}
			this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_REWARD);
		}
		this.m_i32DlgCount++;
	}

	public void SetNextQuestDlg(int CharKind, CQuest cQuest)
	{
		this.SetQuestDlg(CharKind, cQuest);
		for (int i = 0; i < 6; i++)
		{
			this.m_TalkButton[i].Show(false);
		}
		if (!this.m_bAutoQuest)
		{
			this.AutoClickButton(cQuest.GetQuestUnique());
		}
	}

	public void AcceptExit()
	{
		this.m_Step = E_NPC_TALK_STEP.E_NPC_TALK_STEP_ACCEPT;
		this.BtnClickExit(null);
	}

	private void BtnClickExit(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		if (!string.IsNullOrEmpty(this.m_strNpcCHARCODE))
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("NPC", this.m_strNpcCHARCODE, "BYE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		if (this.m_NpcUI != null)
		{
			this.m_NpcUI.CloseUIALL();
		}
		this.Clear();
		if (this.m_Step == E_NPC_TALK_STEP.E_NPC_TALK_STEP_ACCEPT)
		{
			this.CloseAccept();
		}
		else
		{
			this.Close();
		}
	}

	private void UIProccess()
	{
		this.m_NPCTalk_transbutton.Visible = true;
		this.m_NPCTalk_close.Visible = true;
		if (this.m_i32MenuCount == 0)
		{
			this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_END);
		}
		if (this.m_State.eNpcUIType == NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST)
		{
			this.m_TalkButton[0].Show(false);
			this.m_TalkButton[1].Show(false);
			this.m_TalkButton[2].Show(false);
			this.m_TalkButton[3].Show(false);
			this.m_TalkButton[4].Show(false);
			this.m_TalkButton[5].Show(false);
			this.m_DrawTexture_line1.Visible = false;
			this.m_DrawTexture_line2.Visible = false;
			this.m_DrawTexture_line3.Visible = false;
			this.m_DrawTexture_line4.Visible = false;
			this.m_DrawTexture_line5.Visible = false;
			this.m_DrawTexture_line6.Visible = false;
			this.m_strDlgIndex = NpcTalkUI_DLG.QuestDlgIndex(this.m_State.kQuest, this.m_State.eState);
			CQuestGroup questGroupByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(this.m_State.kQuest.GetQuestUnique());
			if (questGroupByQuestUnique == null)
			{
				string text = "퀘스트 그룹유니크 : " + this.m_State.kQuest.GetQuestCommon().nQuestGroupUnique.ToString();
				text = text + " 퀘스트 유니크 : " + this.m_State.kQuest.GetQuestUnique();
				NrTSingleton<NrMainSystem>.Instance.Alert(text);
				return;
			}
			USER_QUEST_COMPLETE_INFO completeQuestInfo = NrTSingleton<NkQuestManager>.Instance.GetCompleteQuestInfo(questGroupByQuestUnique.GetGroupUnique());
			if (questGroupByQuestUnique != null && completeQuestInfo != null)
			{
				this.m_i32Grade = (int)completeQuestInfo.bCurrentGrade;
			}
			else
			{
				this.m_i32Grade = 1;
			}
		}
		else if (this.m_State.eNpcUIType != NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST_VICTORYSCENARIOBATTLE)
		{
			this.m_TalkButton[0].Show(false);
			this.m_TalkButton[1].Show(false);
			this.m_TalkButton[2].Show(false);
			this.m_TalkButton[3].Show(false);
			this.m_TalkButton[4].Show(false);
			this.m_TalkButton[5].Show(false);
			this.m_DrawTexture_line1.Visible = false;
			this.m_DrawTexture_line2.Visible = false;
			this.m_DrawTexture_line3.Visible = false;
			this.m_DrawTexture_line4.Visible = false;
			this.m_DrawTexture_line5.Visible = false;
			this.m_DrawTexture_line6.Visible = false;
			this.m_NPCTalk_close.Visible = true;
			this.m_NPCTalk_transbutton.Visible = false;
			if (this.m_State.eNpcUIType != NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE_POPUP)
			{
				this.m_Step = E_NPC_TALK_STEP.E_NPC_TALK_STEP_MENU;
			}
			this.SetTalkText(string.Empty);
			this.m_NpcUI.OpenUI(this.m_State.bMenuNum);
		}
	}

	public void SetButton()
	{
		this.m_TalkButton[0].Show(false);
		this.m_TalkButton[1].Show(false);
		this.m_TalkButton[2].Show(false);
		this.m_TalkButton[3].Show(false);
		this.m_TalkButton[4].Show(false);
		this.m_TalkButton[5].Show(false);
		this.m_DrawTexture_line1.Visible = false;
		this.m_DrawTexture_line2.Visible = false;
		this.m_DrawTexture_line3.Visible = false;
		this.m_DrawTexture_line4.Visible = false;
		this.m_DrawTexture_line5.Visible = false;
		this.m_DrawTexture_line6.Visible = false;
		this.m_NPCTalk_talklabel.Visible = true;
		this.m_NPCTalk_transbutton.Visible = true;
		this.m_NPCTalk_close.Visible = true;
		if (this.m_i32MenuCount == 0)
		{
			this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_END);
		}
	}

	public void SetAutoQuest(bool flag)
	{
		this.m_bAutoQuest = flag;
	}

	public void AutoClickButton()
	{
		this.Menu1(this.m_TalkButton[0].m_NPCTalk_menutitle);
		this.m_NPCTalk_talklabel.Visible = true;
		this.m_NPCTalk_transbutton.Visible = true;
	}

	public void AutoClickButton(string questUnique)
	{
		Button button = null;
		for (int i = 0; i < 6; i++)
		{
			NPC_TALK_QUEST_STATE nPC_TALK_QUEST_STATE = this.m_TalkButton[i].m_NPCTalk_menutitle.Data as NPC_TALK_QUEST_STATE;
			if (nPC_TALK_QUEST_STATE != null)
			{
				if (nPC_TALK_QUEST_STATE.kQuest.GetQuestUnique() == questUnique)
				{
					button = this.m_TalkButton[i].m_NPCTalk_menutitle;
					break;
				}
			}
		}
		if (null != button)
		{
			this.Menu1(button);
			this.m_NPCTalk_talklabel.Visible = true;
			this.m_NPCTalk_transbutton.Visible = true;
		}
	}

	private void Menu1(IUIObject obj)
	{
		UIButton uIButton = (UIButton)obj;
		NPC_TALK_QUEST_STATE nPC_TALK_QUEST_STATE = uIButton.Data as NPC_TALK_QUEST_STATE;
		if (nPC_TALK_QUEST_STATE == null)
		{
			return;
		}
		this.m_State = nPC_TALK_QUEST_STATE;
		if (NrTSingleton<NkQuestManager>.Instance.RewardShow)
		{
			return;
		}
		if (this.m_State.eNpcUIType == NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST_SUB)
		{
			this.SetQuestMenu();
			return;
		}
		this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_TALK);
		this.UIProccess();
		if (this.m_State.eNpcUIType == NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST)
		{
			this.BtnClickNext(null);
		}
	}

	private void BtnPagePre(IUIObject obj)
	{
		this.m_i16CurrentPage -= 1;
		if (this.m_i16CurrentPage <= 0)
		{
			this.m_i16CurrentPage = 1;
		}
		this.SetQuestMenu();
	}

	private void BtnPageNext(IUIObject obj)
	{
		this.m_i16CurrentPage += 1;
		short num = (short)(this.m_QuestMenu.Count / 6);
		num += 1;
		if (this.m_i16CurrentPage > num)
		{
			this.m_i16CurrentPage = num;
		}
		this.SetQuestMenu();
	}

	public override void Show()
	{
		base.Show();
	}

	public void CloseSub()
	{
		bool flag = false;
		for (int i = 0; i < 6; i++)
		{
			if (this.m_NpcUI != null && NrTSingleton<FormsManager>.Instance.IsShow(this.m_NpcUI.GetUID(i)))
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.Clear();
			this.Close();
		}
	}

	public void SetGrade(int i32Grade)
	{
		this.m_i32Grade = i32Grade;
	}

	public void SetStep(E_NPC_TALK_STEP step)
	{
		this.m_Step = step;
		switch (step)
		{
		case E_NPC_TALK_STEP.E_NPC_TALK_STEP_START:
			this.m_NPCTalk_transbutton.Visible = true;
			this.m_NPCTalk_close.Visible = true;
			this.m_NPCTalk_close.PlayAni(false);
			this.m_NPCTalk_close.SetButtonTextureKey("Win_B_CloseN");
			this.m_NPCTalk_close.SetSize(50f, 50f);
			this.m_NPCTalk_close.SetLocation(GUICamera.width - this.m_NPCTalk_close.GetSize().x - 20f, this.firstPosY1);
			break;
		case E_NPC_TALK_STEP.E_NPC_TALK_STEP_MENU:
			this.m_NPCTalk_transbutton.Visible = false;
			this.m_NPCTalk_close.Visible = true;
			this.m_NPCTalk_close.PlayAni(false);
			this.m_NPCTalk_close.SetButtonTextureKey("Win_B_CloseN");
			this.m_NPCTalk_close.SetSize(50f, 50f);
			this.m_NPCTalk_close.SetLocation(GUICamera.width - this.m_NPCTalk_close.GetSize().x - 20f, this.firstPosY1);
			this.m_NPCTalk_close.Text = " ";
			break;
		case E_NPC_TALK_STEP.E_NPC_TALK_STEP_TALK:
			this.m_NPCTalk_transbutton.Visible = true;
			this.m_NPCTalk_close.Visible = true;
			this.m_NPCTalk_close.PlayAni(true);
			this.m_NPCTalk_close.SetButtonTextureKey("Win_B_TalkNext");
			this.m_NPCTalk_close.SetSize(52f, 20f);
			this.m_NPCTalk_close.Text = " ";
			this.m_NPCTalk_close.SetLocation(GUICamera.width - this.m_NPCTalk_close.GetSize().x - 20f, this.firstPosY2);
			break;
		case E_NPC_TALK_STEP.E_NPC_TALK_STEP_ACCEPT:
			this.m_NPCTalk_transbutton.Visible = true;
			break;
		case E_NPC_TALK_STEP.E_NPC_TALK_STEP_REWARD:
			this.m_NPCTalk_transbutton.Visible = true;
			break;
		case E_NPC_TALK_STEP.E_NPC_TALK_STEP_END:
			this.m_NPCTalk_transbutton.Visible = true;
			break;
		}
	}

	private string SetCurrentPage()
	{
		short num = (short)(this.m_QuestMenu.Count / 6);
		num += 1;
		return this.m_i16CurrentPage.ToString() + " / " + num.ToString();
	}

	public void CloseAccept()
	{
		if (this.m_State.kQuest != null)
		{
			this.Hide();
			this.m_State.kQuest.AfterAccept();
			RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_QUEST);
			if (rightMenuQuestUI != null)
			{
				rightMenuQuestUI.QuestAccept(this.m_State.kQuest);
			}
			else
			{
				Debug.LogWarning("NpcTalk RightMenuQuestUI == null");
			}
			CQuestGroup questGroupByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(this.m_State.kQuest.GetQuestUnique());
			if (questGroupByQuestUnique != null)
			{
				if (questGroupByQuestUnique.IsFristQuest(this.m_State.kQuest.GetQuestUnique()))
				{
					if (questGroupByQuestUnique.GetQuestType() == 1)
					{
						ChapterStart_DLG chapterStart_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.QUEST_CHAPTERSTART) as ChapterStart_DLG;
						if (chapterStart_DLG != null)
						{
							chapterStart_DLG.SetInfo(this.m_State.kQuest.GetQuestUnique());
							chapterStart_DLG.Show();
							this.m_bShowAll = false;
							Main_UI_SystemMessage.ClearText();
						}
					}
				}
				else
				{
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "ACCEPT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
			this.Clear();
		}
		else
		{
			Debug.LogWarning("NpcTalk m_State.kQuest == null");
		}
		this.Close();
	}

	public override void Close()
	{
		if (this.m_State != null && this.m_State.kQuest != null && "10101_005" != this.m_State.kQuest.GetQuestUnique())
		{
			NrTSingleton<EventConditionHandler>.Instance.QuestClose.Value.Set(this.m_State.kQuest.GetQuestUnique());
			NrTSingleton<EventConditionHandler>.Instance.QuestClose.OnTrigger();
		}
		NrTSingleton<NkQuestManager>.Instance.CloseAllReward();
		NrTSingleton<NkQuestManager>.Instance.UpdateClientNpc(0);
		this.OnlySound("NPC", "BYE");
		NrTSingleton<NkQuestManager>.Instance.DequeueNpcTell();
		NrTSingleton<NrMainSystem>.Instance.MemoryCleanUP();
		if (this.m_bShowAll)
		{
			NrTSingleton<FormsManager>.Instance.Main_UI_Show(FormsManager.eMAIN_UI_VISIBLE_MODE.COMMON);
		}
		base.Close();
		NrTSingleton<NewGuildManager>.Instance.ShowAgitInfoDLG();
	}

	public void FadeImage(EZAnimation anim)
	{
		FadeSprite.Do(this.m_FadeImage, EZAnimation.ANIM_MODE.To, new Color(0f, 0f, 0f, 0f), new EZAnimation.Interpolator(EZAnimation.linear), 0.5f, 0.25f, null, new EZAnimation.CompletionDelegate(this.HideFadeImage));
	}

	public void HideFadeImage(EZAnimation anim)
	{
		this.m_FadeImage.Visible = false;
		this.currentDlgInfo = null;
		this.ChangeDlgText(false);
	}

	public void OnlySound(string domainKey, string audioKey)
	{
		if (this.m_CurNpc != null)
		{
			string code = this.m_CurNpc.GetCode();
			TsAudioManager.Instance.AudioContainer.RequestAudioClip(domainKey, code, audioKey, new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	private void SetCameraSet(short i16CharUnique, QUEST_DLG_INFO dlgInfo)
	{
		NrCharNPC nrCharNPC = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(i16CharUnique) as NrCharNPC;
		if (nrCharNPC == null)
		{
			return;
		}
		if (this.m_i16PreUnique != i16CharUnique)
		{
			NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(this.m_i16PreUnique);
			if (charByCharUnique != null)
			{
				charByCharUnique.SetShowHide3DModel(false, false, false);
			}
			this.m_i16PreUnique = i16CharUnique;
			NrTSingleton<NkClientLogic>.Instance.SetNpcTalkCharUnique((int)i16CharUnique);
			if (!dlgInfo.IsDLGOption(1L))
			{
				nrCharNPC.SetShowHide3DModel(true, false, false);
			}
			else if (dlgInfo.IsDLGOption(1L))
			{
				nrCharNPC.SetShowHide3DModel(false, false, false);
			}
			if (nrCharNPC.Get3DChar() == null)
			{
				return;
			}
			nrCharNPC.ProcessMouseEvent = false;
			if (!nrCharNPC.m_kCharMove.IsMoving())
			{
				if (nrCharNPC.IsHaveAnimation(eCharAnimationType.TalkStart1))
				{
					nrCharNPC.SetAnimation(eCharAnimationType.TalkStart1, true, false);
				}
				else if (nrCharNPC.IsHaveAnimation(eCharAnimationType.TalkStay1))
				{
					nrCharNPC.SetAnimation(eCharAnimationType.TalkStay1, true, false);
				}
				else
				{
					nrCharNPC.SetAnimation(eCharAnimationType.Stay1, true, false);
				}
			}
			GameObject charObject = nrCharNPC.GetCharObject();
			if (charObject != null)
			{
				if (!this.m_bBackCamera)
				{
					NrTSingleton<NkClientLogic>.Instance.BackMainCameraInfo();
					this.m_bBackCamera = true;
				}
				Transform child = NkUtil.GetChild(charObject.transform, "Camera");
				this.m_WorldCamera = Camera.main.GetComponent<maxCamera>();
				if (this.m_WorldCamera != null)
				{
					this.m_WorldCamera.StopCameraControl();
				}
				Camera.main.fieldOfView = 40f;
				NrTSingleton<NkQuestManager>.Instance.SetQuestCamera(nrCharNPC, child);
				if (null != child)
				{
					this.m_vStartPos = child.position;
				}
				if (dlgInfo.IsDLGOption(2L))
				{
					this.SetShake();
				}
				NrTSingleton<NkCharManager>.Instance.SyncBillboardRotate();
			}
			else
			{
				Debug.LogWarning("npc.Get3DChar().GetRootGameObject() == null");
			}
		}
		else if (nrCharNPC != null && this.m_i16PreUnique == i16CharUnique)
		{
			if (!dlgInfo.IsDLGOption(1L))
			{
				nrCharNPC.SetShowHide3DModel(true, false, false);
			}
			else if (dlgInfo.IsDLGOption(1L))
			{
				nrCharNPC.SetShowHide3DModel(false, false, false);
			}
			if (dlgInfo.IsDLGOption(2L))
			{
				this.SetShake();
			}
		}
		else if (nrCharNPC == null)
		{
			Debug.LogWarning("null == npc");
		}
	}

	public void InitCameara()
	{
		this.m_i32StartCamera = Environment.TickCount;
		this.m_i32EndCamera = this.m_i32StartCamera;
	}

	private void SetShake()
	{
		this.m_bShakeCamera = true;
		this.m_i32Start = Environment.TickCount;
		this.m_i32StartCamera = Environment.TickCount;
		this.m_i32End = Environment.TickCount;
		this.m_i32EndCamera = Environment.TickCount;
	}

	private void CheckMoveNpc()
	{
		NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(this.m_16CurCharUnique);
		if (charByCharUnique == null)
		{
			this.BtnClickExit(null);
			return;
		}
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char == null)
		{
			return;
		}
		GameObject rootGameObject = @char.m_k3DChar.GetRootGameObject();
		GameObject rootGameObject2 = charByCharUnique.m_k3DChar.GetRootGameObject();
		if (null == rootGameObject || null == rootGameObject2)
		{
			return;
		}
		float num = Vector3.Distance(rootGameObject.transform.position, rootGameObject2.transform.position);
		if (20f <= num)
		{
			@char.MoveTo(charByCharUnique.GetCharObject().transform.position.x, charByCharUnique.GetCharObject().transform.position.y, charByCharUnique.GetCharObject().transform.position.z, false);
		}
		Transform child = NkUtil.GetChild(charByCharUnique.GetCharObject().transform, "camera");
		if (null == child)
		{
			return;
		}
		Camera.main.fieldOfView = 40f;
		EventTriggerHelper.CameraMove(Camera.main, child);
	}

	private void UpdateShakeCamera()
	{
		if (this.m_bShakeCamera)
		{
			this.m_i32Start = Environment.TickCount;
			if (this.m_i32Start - this.m_i32End < 300)
			{
				this.m_i32StartCamera = Environment.TickCount;
				if (this.m_i32StartCamera - this.m_i32EndCamera < 50)
				{
					Vector3 vStartPos = this.m_vStartPos;
					vStartPos.x += (float)UnityEngine.Random.Range(0, 25) * 0.03f;
					vStartPos.y += (float)UnityEngine.Random.Range(0, 25) * 0.03f;
					vStartPos.z += (float)UnityEngine.Random.Range(0, 25) * 0.03f;
					NrTSingleton<NkQuestManager>.Instance.GetQuestCamera().SetACtive(false);
					Camera.main.transform.position = vStartPos;
					this.m_i32EndCamera = this.m_i32StartCamera;
				}
			}
			else
			{
				this.m_bShakeCamera = false;
				NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(this.m_i16PreUnique);
				if (charByCharUnique != null)
				{
					Transform child = NkUtil.GetChild(charByCharUnique.Get3DChar().GetRootGameObject().transform, "camera");
					if (null != child)
					{
						Camera.main.fieldOfView = 40f;
						NrTSingleton<NkQuestManager>.Instance.GetQuestCamera().SetACtive(true);
					}
				}
				Camera.main.transform.position = this.m_vStartPos;
			}
		}
	}

	public void SetQuestDlg(int CharKind, CQuest cQuest)
	{
		if (cQuest == null)
		{
			return;
		}
		this.SetDlgInit();
		this.m_NPCTalk_talklabel.Visible = true;
		this.m_NPCTalk_transbutton.Visible = true;
		this.m_NPCTalk_close.Visible = true;
		this.m_CurNpc = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(CharKind);
		if (this.m_CurNpc == null)
		{
			this.m_CurNpc = cQuest.GetQuestNpc();
		}
		NrTSingleton<CNpcUIManager>.Instance.AddNpc(this.m_CurNpc);
		this.m_NpcUI = NrTSingleton<CNpcUIManager>.Instance.GetNpcUIByNpcKind(this.m_CurNpc.GetCharKind());
		if (this.m_State == null)
		{
			this.m_State = new NPC_TALK_QUEST_STATE();
		}
		this.m_State.eNpcUIType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_QUEST;
		this.m_State.kQuest = cQuest;
		this.m_State.eState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(cQuest.GetQuestUnique());
		this.m_i32DlgCount = 1;
		this.m_strDlgIndex = NpcTalkUI_DLG.QuestDlgIndex(this.m_State.kQuest, this.m_State.eState);
		this.SetQuestMenuItem(CharKind);
		this.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_TALK);
		if (this.m_CurNpc != null)
		{
			this.m_CenterNpcName.Text = this.m_CurNpc.GetName();
		}
	}

	private void SetDlgInit()
	{
		this.m_NPCTalk_talklabel.Visible = true;
		this.m_NPCTalk_transbutton.Visible = true;
		this.m_NPCTalk_close.Visible = true;
		this.m_TalkButton[0].Show(false);
		this.m_TalkButton[1].Show(false);
		this.m_TalkButton[2].Show(false);
		this.m_TalkButton[3].Show(false);
		this.m_TalkButton[4].Show(false);
		this.m_TalkButton[5].Show(false);
		this.m_DrawTexture_line1.Visible = false;
		this.m_DrawTexture_line2.Visible = false;
		this.m_DrawTexture_line3.Visible = false;
		this.m_DrawTexture_line4.Visible = false;
		this.m_DrawTexture_line5.Visible = false;
		this.m_DrawTexture_line6.Visible = false;
	}

	public static string QuestDlgIndex(CQuest kQuest, QUEST_CONST.eQUESTSTATE eState)
	{
		string result = string.Empty;
		switch (eState)
		{
		case QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE:
			result = kQuest.GetQuestUnique().ToString() + "a";
			break;
		case QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING:
			result = kQuest.GetQuestUnique().ToString() + "g";
			break;
		case QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE:
			result = kQuest.GetQuestUnique().ToString() + "p";
			break;
		default:
			result = kQuest.GetQuestUnique().ToString() + "a";
			break;
		}
		return result;
	}
}
