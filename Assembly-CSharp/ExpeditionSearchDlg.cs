using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class ExpeditionSearchDlg : Form
{
	private DrawTexture m_dtBG;

	private Button m_btList;

	private Button m_btClose;

	private Button[] m_btSearchGrade = new Button[5];

	private Label m_lagoExpeditionJoinCount;

	private Label[] m_laSearchGradeName = new Label[5];

	private DrawTexture[] m_dtTextBG = new DrawTexture[5];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Expedition/DLG_Expedition_Exploration", G_ID.EXPEDITION_SEARCH_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_dtBG = (base.GetControl("DT_SubBG") as DrawTexture);
		this.m_dtBG.SetTextureFromBundle("UI/expedition/bg_personal_min002");
		this.m_btList = (base.GetControl("BT_MineList01") as Button);
		Button expr_42 = this.m_btList;
		expr_42.Click = (EZValueChangedDelegate)Delegate.Combine(expr_42.Click, new EZValueChangedDelegate(this.OnClickCurrentState));
		this.m_btClose = (base.GetControl("BT_MineExit01") as Button);
		Button expr_7F = this.m_btClose;
		expr_7F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_7F.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_lagoExpeditionJoinCount = (base.GetControl("LB_Text02") as Label);
		for (byte b = 1; b < 5; b += 1)
		{
			this.m_dtTextBG[(int)b] = (base.GetControl(string.Format("DT_Mine0{0}_TextBG02", b)) as DrawTexture);
			this.m_dtTextBG[(int)b].Hide(true);
			this.m_laSearchGradeName[(int)b] = (base.GetControl(string.Format("LB_MineName0{0}", b)) as Label);
			this.m_laSearchGradeName[(int)b].Hide(true);
			this.m_btSearchGrade[(int)b] = (base.GetControl(string.Format("BT_Mine0{0}", b)) as Button);
			this.m_btSearchGrade[(int)b].Data = b;
			Button expr_15D = this.m_btSearchGrade[(int)b];
			expr_15D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_15D.Click, new EZValueChangedDelegate(this.OnBtnClickSearch));
			this.m_btSearchGrade[(int)b].Hide(true);
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
	}

	public override void OnClose()
	{
	}

	public void SetTextUI()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		string text = string.Empty;
		string text2 = string.Empty;
		for (byte b = 1; b < 5; b += 1)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.ExpeditionGradeLimit() >= (int)b)
			{
				string szColorNum = string.Empty;
				EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(b);
				if (expeditionDataFromGrade != null)
				{
					if (kMyCharInfo.GetLevel() < (int)expeditionDataFromGrade.Possiblelevel)
					{
						szColorNum = "1305";
					}
					else
					{
						szColorNum = "1101";
					}
				}
				this.m_laSearchGradeName[(int)b].Hide(false);
				this.m_btSearchGrade[(int)b].Hide(false);
				this.m_dtTextBG[(int)b].Hide(false);
				text2 = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor(szColorNum), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(expeditionDataFromGrade.Expedition_INTERFACEKEY));
				this.m_laSearchGradeName[(int)b].SetText(text2);
			}
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2413");
		EXPEDITION_CONSTANT_MANAGER instance = EXPEDITION_CONSTANT_MANAGER.GetInstance();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"count1",
			kMyCharInfo.GetCharDetail(10),
			"count2",
			instance.GetValue(eEXPEDITION_CONSTANT.eEXPEDITION_DAY_COUNT)
		});
		this.m_lagoExpeditionJoinCount.SetText(text2);
	}

	public void OnBtnClickSearch(IUIObject obj)
	{
		string title = string.Empty;
		string text = string.Empty;
		string message = string.Empty;
		byte b = (byte)obj.Data;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolCount() == 0)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("528");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (kMyCharInfo.GetMilitaryList().FindEmptyExpeditionMilitaryIndex() == -1)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("765");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(b);
		if (expeditionDataFromGrade.Expedition_SEARCH_MONEY > kMyCharInfo.m_Money)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (kMyCharInfo.GetLevel() < (int)expeditionDataFromGrade.Possiblelevel)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("272");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref message, new object[]
			{
				text,
				"count",
				expeditionDataFromGrade.Possiblelevel,
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(expeditionDataFromGrade.Expedition_INTERFACEKEY)
			});
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		long num = 0L;
		EXPEDITION_CONSTANT_MANAGER instance = EXPEDITION_CONSTANT_MANAGER.GetInstance();
		if (instance != null)
		{
			num = (long)instance.GetValue(eEXPEDITION_CONSTANT.eEXPEDITION_DAY_COUNT);
		}
		if (num > 0L && kMyCharInfo.GetCharDetail(10) >= num)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("405");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		title = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1316");
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("128");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref message, new object[]
		{
			text,
			"count",
			expeditionDataFromGrade.Expedition_SEARCH_MONEY,
			"targetname1",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(expeditionDataFromGrade.Expedition_INTERFACEKEY),
			"targetname2",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(expeditionDataFromGrade.Expedition_GRADE_INTERFACEKEY)
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnSearch), b, title, message, eMsgType.MB_OK_CANCEL, 2);
	}

	public void OnSearch(object obj)
	{
		byte i8Grade = (byte)obj;
		GS_EXPEDITION_SERACH_REQ gS_EXPEDITION_SERACH_REQ = new GS_EXPEDITION_SERACH_REQ();
		gS_EXPEDITION_SERACH_REQ.i8Grade = i8Grade;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_SERACH_REQ, gS_EXPEDITION_SERACH_REQ);
	}

	public void OnClickCurrentState(IUIObject obj)
	{
		GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ = new GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ();
		gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ.i32Page = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ, gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ);
	}

	public void OnClickClose(IUIObject obj)
	{
		this.Close();
	}
}
