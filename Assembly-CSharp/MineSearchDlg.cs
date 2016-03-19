using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityForms;

public class MineSearchDlg : Form
{
	private DrawTexture m_dtBG;

	private Button m_btList;

	private Button m_btClose;

	private Button[] m_btSearchGrade = new Button[5];

	private Label m_lagoMineJoinCount;

	private Label[] m_laSearchGradeName = new Label[5];

	private DrawTexture[] m_dtSearchGradeTextBG = new DrawTexture[5];

	private Button m_btMineGuideWebcall;

	private Label m_lbMineGuide;

	private DrawTexture m_dtMineGuide;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Mine/DLG_MineExploration", G_ID.MINE_SEARCH_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_dtBG = (base.GetControl("DT_SubBG") as DrawTexture);
		this.m_dtBG.SetTextureFromBundle("UI/Mine/bg_mineexploration");
		this.m_btList = (base.GetControl("BT_MineList01") as Button);
		Button expr_42 = this.m_btList;
		expr_42.Click = (EZValueChangedDelegate)Delegate.Combine(expr_42.Click, new EZValueChangedDelegate(this.OnClickCurrentState));
		this.m_btClose = (base.GetControl("BT_MineExit01") as Button);
		Button expr_7F = this.m_btClose;
		expr_7F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_7F.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_lagoMineJoinCount = (base.GetControl("LB_Text02") as Label);
		for (byte b = 1; b < 5; b += 1)
		{
			this.m_laSearchGradeName[(int)b] = (base.GetControl(string.Format("LB_MineName0{0}", b)) as Label);
			this.m_btSearchGrade[(int)b] = (base.GetControl(string.Format("BT_Mine0{0}", b)) as Button);
			this.m_dtSearchGradeTextBG[(int)b] = (base.GetControl(string.Format("DT_Mine0{0}_TextBG02", b)) as DrawTexture);
			this.m_btSearchGrade[(int)b].Data = b;
			Button expr_141 = this.m_btSearchGrade[(int)b];
			expr_141.Click = (EZValueChangedDelegate)Delegate.Combine(expr_141.Click, new EZValueChangedDelegate(this.OnBtnClickSearch));
			if (!NrTSingleton<ContentsLimitManager>.Instance.IsValidMineGrade(b))
			{
				this.m_btSearchGrade[(int)b].Hide(true);
				this.m_laSearchGradeName[(int)b].Hide(true);
				this.m_dtSearchGradeTextBG[(int)b].Hide(true);
			}
		}
		this.m_btMineGuideWebcall = (base.GetControl("BT_MineExit01") as Button);
		Button expr_1C4 = this.m_btMineGuideWebcall;
		expr_1C4.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C4.Click, new EZValueChangedDelegate(this.OnClickMineGuideWebCall));
		if (this.m_btMineGuideWebcall != null)
		{
			this.m_btMineGuideWebcall.Visible = false;
		}
		this.m_lbMineGuide = (base.GetControl("Label_Label23") as Label);
		if (this.m_lbMineGuide != null)
		{
			this.m_lbMineGuide.Visible = false;
		}
		this.m_dtMineGuide = (base.GetControl("DrawTexture_DrawTexture22") as DrawTexture);
		if (this.m_dtMineGuide != null)
		{
			this.m_dtMineGuide.Visible = false;
		}
		base.SetScreenCenter();
		this.Hide();
	}

	public override void Show()
	{
		this.SetTextUI();
		base.Show();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void SetTextUI()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		string text = string.Empty;
		string text2 = string.Empty;
		for (byte b = 1; b < 5; b += 1)
		{
			string szColorNum = string.Empty;
			MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(b);
			if (mineDataFromGrade != null)
			{
				if (kMyCharInfo.GetLevel() < (int)mineDataFromGrade.POSSIBLELEVEL)
				{
					szColorNum = "1305";
				}
				else
				{
					szColorNum = "1101";
				}
			}
			text2 = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor(szColorNum), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(mineDataFromGrade.MINE_INTERFACEKEY));
			this.m_laSearchGradeName[(int)b].SetText(text2);
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1832");
		MINE_CONSTANT_Manager instance = MINE_CONSTANT_Manager.GetInstance();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"count1",
			kMyCharInfo.GetCharDetail(8),
			"count2",
			instance.GetValue(eMINE_CONSTANT.eMINE_DAY_COUNT)
		});
		this.m_lagoMineJoinCount.SetText(text2);
	}

	public void OnBtnClickSearch(IUIObject obj)
	{
		string title = string.Empty;
		string text = string.Empty;
		string message = string.Empty;
		byte b = (byte)obj.Data;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo.GetMilitaryList().FindEmptyMineMilitaryIndex() == -1)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("691");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(b);
		NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(kMyCharInfo.m_PersonID);
		if (memberInfoFromPersonID == null)
		{
			return;
		}
		if (memberInfoFromPersonID.GetRank() <= NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_INITIATE)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("532");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (mineDataFromGrade.MINE_SEARCH_MONEY > kMyCharInfo.m_Money)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (kMyCharInfo.GetLevel() < (int)mineDataFromGrade.POSSIBLELEVEL)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("272");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref message, new object[]
			{
				text,
				"count",
				mineDataFromGrade.POSSIBLELEVEL,
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(mineDataFromGrade.MINE_INTERFACEKEY)
			});
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		long num = 0L;
		MINE_CONSTANT_Manager instance = MINE_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			num = (long)instance.GetValue(eMINE_CONSTANT.eMINE_DAY_COUNT);
		}
		if (num > 0L && kMyCharInfo.GetCharDetail(8) >= num)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("405");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsValidMineGrade(b))
		{
			return;
		}
		title = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1316");
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("128");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref message, new object[]
		{
			text,
			"count",
			mineDataFromGrade.MINE_SEARCH_MONEY,
			"targetname1",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(mineDataFromGrade.MINE_INTERFACEKEY),
			"targetname2",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(mineDataFromGrade.MINE_GRADE_INTERFACEKEY)
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnSearch), b, title, message, eMsgType.MB_OK_CANCEL);
	}

	public void OnSearch(object obj)
	{
		byte bySearchMineGrade = (byte)obj;
		GS_MINE_SEARCH_REQ gS_MINE_SEARCH_REQ = new GS_MINE_SEARCH_REQ();
		gS_MINE_SEARCH_REQ.bySearchMineGrade = bySearchMineGrade;
		gS_MINE_SEARCH_REQ.m_nMineID = 0L;
		gS_MINE_SEARCH_REQ.m_nGuildID = 0L;
		gS_MINE_SEARCH_REQ.m_nMode = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_SEARCH_REQ, gS_MINE_SEARCH_REQ);
	}

	public void OnClickCurrentState(IUIObject obj)
	{
		GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ = new GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ();
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.i64GuildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.i32Page = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ, gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ);
	}

	public void OnClickClose(IUIObject obj)
	{
		MineGuildCurrentStatusInfoDlg mineGuildCurrentStatusInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG) as MineGuildCurrentStatusInfoDlg;
		if (mineGuildCurrentStatusInfoDlg != null)
		{
			mineGuildCurrentStatusInfoDlg.Close();
		}
		this.Close();
	}

	public void OnClickMineGuideWebCall(IUIObject obj)
	{
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			NrMobileNoticeWeb nrMobileNoticeWeb = new NrMobileNoticeWeb();
			string weCallURL = TableData_GameWebCallURLInfo.GetWeCallURL(eGameWebCallURL.WEBCALL_MINE);
			if (weCallURL != string.Empty)
			{
				nrMobileNoticeWeb.OnMineGuideWebCall(weCallURL);
			}
			else
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("727");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
	}
}
