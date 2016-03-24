using GAME;
using GameMessage;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using PROTOCOL.WORLD;
using SERVICE;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class MainMenuDlg : Form
{
	private enum TYPE
	{
		NONE = -1,
		CHALLENGE,
		POST,
		ITEMUPGRADE,
		ITEMSHOP,
		AUCTION,
		MINIALBUM,
		SOLCOMBINATION,
		NPCAUTOMOVE,
		SYSTEM,
		CHANNELMOVE,
		NOTICE,
		EXIT,
		FACEBOOK,
		MAINCAFE,
		END
	}

	private float TEX_SIZE = 512f;

	private Button[] m_BtnGameInfo = new Button[14];

	private DrawTexture[] m_dwButtonImg = new DrawTexture[14];

	private Label[] m_lbMenuName = new Label[14];

	private DrawTexture[] m_dwBGImg = new DrawTexture[14];

	private Label m_lbFacebook;

	private DrawTexture m_dwFacebook;

	private DrawTexture m_dtCharImg;

	private Label m_lbCharName;

	private Label m_lbRealTime;

	private Label m_lbPlayTime;

	private TextArea m_taIntroMsg;

	private Button m_BtnChangeIntroMsg;

	private Box challengeNotice;

	private DrawTexture m_dtCoupon;

	private Button m_btCoupon;

	private Box m_bxAuction;

	private Button m_btGoCustomer;

	private Box m_bxCustomerNotice;

	public long m_TotalPlayTimeCount;

	public long m_RealPlayTimeCount;

	public int m_nChallenge_Event;

	private float sendtime;

	private bool bChangeIntroMsg;

	private Button m_btnKaKao;

	private Button m_btnConvert;

	private Button[] m_btnSDK = new Button[3];

	private Dictionary<MainMenuDlg.TYPE, FunDelegate> mapFun = new Dictionary<MainMenuDlg.TYPE, FunDelegate>();

	private int count;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "System/Dlg_Mainmenu", G_ID.MAINMENU_DLG, false, true);
		this.sendtime = Time.time;
		this.sendtime += 1f;
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_btnKaKao = (base.GetControl("Button_Kakao") as Button);
		this.m_btnKaKao.EffectAni = false;
		this.m_btnKaKao.SetValueChangedDelegate(new EZValueChangedDelegate(this.Exit));
		this.m_btnConvert = (base.GetControl("BT_AccountLink") as Button);
		this.m_btnConvert.SetValueChangedDelegate(new EZValueChangedDelegate(this.ConverPlatformID));
		this.m_btnKaKao.Visible = false;
		this.m_btnConvert.Visible = false;
		for (int i = 0; i < 14; i++)
		{
			this.m_BtnGameInfo[i] = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("Button_MenuBTN", (i + 1).ToString("00"))) as Button);
			this.m_dwButtonImg[i] = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("DrawTexture_BTN_IMG_", (i + 1).ToString("00"))) as DrawTexture);
			this.m_lbMenuName[i] = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("Label_BTN_Label_", (i + 1).ToString("00"))) as Label);
			this.m_dwBGImg[i] = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("DrawTexture_", (i + 1).ToString("00"))) as DrawTexture);
		}
		this.ShowHideButton();
		this.m_BtnGameInfo[12] = (base.GetControl("BT_GoFaceBook") as Button);
		this.m_BtnGameInfo[12].data = 12;
		this.m_BtnGameInfo[12].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickButton));
		if (TsPlatform.IsBand || NrTSingleton<ContentsLimitManager>.Instance.IsFacebookLimit())
		{
			this.m_lbFacebook = (base.GetControl("LB_FaceBook") as Label);
			this.m_dwFacebook = (base.GetControl("DT_FaceBook") as DrawTexture);
			this.m_BtnGameInfo[12].Visible = false;
			this.m_lbFacebook.Visible = false;
			this.m_dwFacebook.Visible = false;
		}
		if (this.m_lbFacebook == null)
		{
			this.m_lbFacebook = (base.GetControl("LB_FaceBook") as Label);
		}
		if (this.m_dwFacebook == null)
		{
			this.m_dwFacebook = (base.GetControl("DT_FaceBook") as DrawTexture);
		}
		this.m_BtnGameInfo[12].Visible = true;
		this.m_lbFacebook.Visible = true;
		this.m_dwFacebook.Visible = true;
		this.m_BtnGameInfo[13] = (base.GetControl("BT_GoGafe") as Button);
		this.m_BtnGameInfo[13].data = 13;
		this.m_BtnGameInfo[13].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickButton));
		this.m_dtCharImg = (base.GetControl("DT_CharImg") as DrawTexture);
		this.m_lbCharName = (base.GetControl("LB_CharName") as Label);
		this.m_lbRealTime = (base.GetControl("Label_realtime01") as Label);
		this.m_lbRealTime.SetText(string.Empty);
		this.m_lbPlayTime = (base.GetControl("Label_playtime") as Label);
		this.m_lbPlayTime.SetText(string.Empty);
		this.m_taIntroMsg = (base.GetControl("TextArea_Greeting") as TextArea);
		this.m_taIntroMsg.maxLength = 40;
		this.m_taIntroMsg.controlIsEnabled = false;
		this.m_BtnChangeIntroMsg = (base.GetControl("BT_ChangeGreeting") as Button);
		this.m_BtnChangeIntroMsg.AddValueChangedDelegate(new EZValueChangedDelegate(this.ChangeIntroMsg));
		this.challengeNotice = (base.GetControl("Box_Notice") as Box);
		this.challengeNotice.Visible = false;
		this.m_dtCoupon = (base.GetControl("DT_Coupon") as DrawTexture);
		this.m_btCoupon = (base.GetControl("BT_GoCoupon") as Button);
		this.m_btCoupon.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCoupon));
		this.m_bxAuction = (base.GetControl("Box_Auction") as Box);
		this.m_bxAuction.Hide(true);
		this.m_bxCustomerNotice = (base.GetControl("Box_Notice_Customer") as Box);
		this.m_bxCustomerNotice.Hide(true);
		this.m_btGoCustomer = (base.GetControl("BT_GoCustomer") as Button);
		this.m_btGoCustomer.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGoCustomer));
		this.m_btnSDK[1] = (base.GetControl("BT_MoreGame_Chn") as Button);
		this.m_btnSDK[1].Hide(true);
		this.m_btnSDK[0] = (base.GetControl("BT_About_Chn") as Button);
		this.m_btnSDK[0].Hide(true);
		this.m_btnSDK[2] = (base.GetControl("BT_Exit_Chn") as Button);
		this.m_btnSDK[2].Hide(true);
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		this.mapFun.Add(MainMenuDlg.TYPE.NOTICE, new FunDelegate(this.Notice));
		this.mapFun.Add(MainMenuDlg.TYPE.SOLCOMBINATION, new FunDelegate(this.SolCombination));
		this.mapFun.Add(MainMenuDlg.TYPE.ITEMUPGRADE, new FunDelegate(this.ItemUpgrade));
		this.mapFun.Add(MainMenuDlg.TYPE.POST, new FunDelegate(this.Post));
		this.mapFun.Add(MainMenuDlg.TYPE.CHANNELMOVE, new FunDelegate(this.ChannelMove));
		this.mapFun.Add(MainMenuDlg.TYPE.SYSTEM, new FunDelegate(this.System));
		this.mapFun.Add(MainMenuDlg.TYPE.EXIT, new FunDelegate(this.Exit));
		this.mapFun.Add(MainMenuDlg.TYPE.FACEBOOK, new FunDelegate(this.GoFaceBook));
		this.mapFun.Add(MainMenuDlg.TYPE.MAINCAFE, new FunDelegate(this.GoMainCafe));
		this.mapFun.Add(MainMenuDlg.TYPE.CHALLENGE, new FunDelegate(this.Challenge));
		this.mapFun.Add(MainMenuDlg.TYPE.ITEMSHOP, new FunDelegate(this.ItemShop));
		this.mapFun.Add(MainMenuDlg.TYPE.NPCAUTOMOVE, new FunDelegate(this.NPCAutoMove));
		this.mapFun.Add(MainMenuDlg.TYPE.AUCTION, new FunDelegate(this.Auction));
		this.mapFun.Add(MainMenuDlg.TYPE.MINIALBUM, new FunDelegate(this.MiniAlbum));
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		GS_SERVER_CHARINFO_REQ gS_SERVER_CHARINFO_REQ = new GS_SERVER_CHARINFO_REQ();
		gS_SERVER_CHARINFO_REQ.siCharUnique = @char.GetCharUnique();
		gS_SERVER_CHARINFO_REQ.bTimeOnly = true;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SERVER_CHARINFO_REQ, gS_SERVER_CHARINFO_REQ);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MAINMENU", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		base.SetScreenCenter();
		this.CheckCouponUse();
	}

	public override void OnClose()
	{
		base.OnClose();
		this.RequestCustomerAnswerCount();
	}

	private void ConverPlatformID(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONVER_PLATFORMID_DLG);
	}

	private void ClickMasterChat(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.CHAT_MOBILE_SUB_DLG);
	}

	private void SetCharInfo()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(0);
			if (soldierInfo != null)
			{
				this.m_dtCharImg.SetUVMask(new Rect(4f / this.TEX_SIZE, 0f, 504f / this.TEX_SIZE, 448f / this.TEX_SIZE));
				this.m_dtCharImg.SetTexture(eCharImageType.LARGE, soldierInfo.GetCharKind(), (int)soldierInfo.GetGrade(), string.Empty);
				this.m_lbCharName.Text = string.Format("Lv.{0} {1}", soldierInfo.GetLevel(), soldierInfo.GetName());
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IntroMsg == string.Empty)
				{
					string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("124");
					string text = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						textFromInterface,
						"username",
						charPersonInfo.GetCharName()
					});
					if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
					{
						text = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
						{
							text
						});
					}
					if (text.Contains("*"))
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("797"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					}
					this.m_taIntroMsg.SetText(text);
				}
				else
				{
					this.m_taIntroMsg.SetText(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IntroMsg);
				}
			}
		}
	}

	public override void Show()
	{
		if (!TsPlatform.IsIPhone && !NrGlobalReference.strLangType.Equals("eng"))
		{
			this.SendAuctionRegisterInfo();
		}
		this.SetCharInfo();
		this.CheckCouponUse();
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo != null)
		{
			this.ShowCustomerNotice(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.CustomerAnswerCount);
		}
		base.Show();
	}

	public void SetMenuButton(int nIndex, int nTypeIndex)
	{
		if (nIndex < 0)
		{
			return;
		}
		string texture = string.Empty;
		string text = string.Empty;
		switch (nTypeIndex)
		{
		case 0:
			texture = "Main_B_Achievement";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("424");
			break;
		case 1:
			texture = "Win_I_LetterC01";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("4");
			break;
		case 2:
			texture = "Win_I_Grind";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("98");
			break;
		case 3:
			texture = "Win_I_Market";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("836");
			break;
		case 4:
			texture = "Win_I_AuctionHouse";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1021");
			break;
		case 5:
			texture = "Win_I_Album";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("888");
			break;
		case 6:
			texture = "Win_I_SolTeamIcon";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1863");
			break;
		case 7:
			texture = "Win_I_Npc";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1327");
			break;
		case 8:
			texture = "Win_I_Setup";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("215");
			break;
		case 9:
			texture = "Win_I_Channel";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("214");
			break;
		case 10:
			texture = "Win_I_Notice";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("368");
			break;
		case 11:
			texture = "Win_I_Exit";
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1168");
			break;
		}
		this.m_dwButtonImg[nIndex].SetTexture(texture);
		this.m_lbMenuName[nIndex].Text = text;
	}

	public override void Update()
	{
		if (Time.time >= this.sendtime)
		{
			this.sendtime += 10f;
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null)
			{
				GS_SERVER_CHARINFO_REQ gS_SERVER_CHARINFO_REQ = new GS_SERVER_CHARINFO_REQ();
				gS_SERVER_CHARINFO_REQ.siCharUnique = @char.GetCharUnique();
				gS_SERVER_CHARINFO_REQ.bTimeOnly = true;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SERVER_CHARINFO_REQ, gS_SERVER_CHARINFO_REQ);
			}
		}
		int rewardNoticeCount = NrTSingleton<ChallengeManager>.Instance.GetRewardNoticeCount();
		if (0 < rewardNoticeCount)
		{
			if (this.count != rewardNoticeCount)
			{
				int num = rewardNoticeCount;
				this.challengeNotice.Visible = true;
				this.challengeNotice.Text = num.ToString();
				this.count = num;
			}
		}
		else if (this.challengeNotice.Visible)
		{
			this.challengeNotice.Visible = false;
		}
	}

	public void SetTimeData(long TotalPlayTime, long RealPlayTime)
	{
		this.m_TotalPlayTimeCount = TotalPlayTime;
		this.m_RealPlayTimeCount = RealPlayTime;
		this.DrawTime();
	}

	public void DrawTime()
	{
		string text = PublicMethod.GetMonthFromSec(this.m_TotalPlayTimeCount).ToString();
		string text2 = PublicMethod.GetDayFromSec(this.m_TotalPlayTimeCount).ToString();
		string text3 = PublicMethod.GetHourFromSec(this.m_TotalPlayTimeCount).ToString();
		string text4 = PublicMethod.GetMinuteFromSec(this.m_TotalPlayTimeCount).ToString();
		string empty = string.Empty;
		if ("0" == text)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1626"),
				"day",
				text2,
				"hour",
				text3,
				"min",
				text4
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2322"),
				"month",
				text,
				"day",
				text2,
				"hour",
				text3,
				"min",
				text4
			});
		}
		this.m_lbPlayTime.SetText(empty);
		string text5 = PublicMethod.GetMonthFromSec(this.m_RealPlayTimeCount).ToString();
		string text6 = PublicMethod.GetDayFromSec(this.m_RealPlayTimeCount).ToString();
		string text7 = PublicMethod.GetHourFromSec(this.m_RealPlayTimeCount).ToString();
		string text8 = PublicMethod.GetMinuteFromSec(this.m_RealPlayTimeCount).ToString();
		string empty2 = string.Empty;
		if ("0" == text5)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1626"),
				"day",
				text6,
				"hour",
				text7,
				"min",
				text8
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2322"),
				"month",
				text5,
				"day",
				text6,
				"hour",
				text7,
				"min",
				text8
			});
		}
		this.m_lbRealTime.SetText(empty2);
	}

	public void SetItroMsg(string intro_msg)
	{
		if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
		{
			intro_msg = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
			{
				intro_msg
			});
		}
		if (intro_msg.Contains("*"))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("797"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		this.m_taIntroMsg.Text = intro_msg;
		string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("125");
		Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
	}

	public override void CloseForm(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MAINMENU", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		MenuIconDlg menuIconDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MENUICON_DLG) as MenuIconDlg;
		if (menuIconDlg != null)
		{
			menuIconDlg.ClickCloseMenu(null);
		}
		base.CloseForm(null);
	}

	public void ClickButton(IUIObject obj)
	{
		Button button = (Button)obj;
		if (null == button)
		{
			return;
		}
		MainMenuDlg.TYPE key = (MainMenuDlg.TYPE)((int)button.data);
		if (this.mapFun.ContainsKey(key))
		{
			this.mapFun[key](null);
		}
		if (TsPlatform.IsIPhone)
		{
			MenuIconDlg menuIconDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MENUICON_DLG) as MenuIconDlg;
			if (menuIconDlg != null)
			{
				menuIconDlg.ClickCloseMenu(null);
			}
		}
	}

	public void Notice(IUIObject obj)
	{
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			NrWebViewObject gameObject = NrWebViewObject.GetGameObject();
			gameObject.MainmenuNoticeOpen = true;
			NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_USERAUTH);
			NrTSingleton<FiveRocksEventManager>.Instance.Placement("notice_open");
		}
	}

	public void GoFaceBook(IUIObject obj)
	{
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			NrMobileNoticeWeb nrMobileNoticeWeb = new NrMobileNoticeWeb();
			nrMobileNoticeWeb.OnFaceBook();
		}
	}

	public void GoMainCafe(IUIObject obj)
	{
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			NrMobileNoticeWeb nrMobileNoticeWeb = new NrMobileNoticeWeb();
			nrMobileNoticeWeb.OnMainCafe();
		}
	}

	public void QuestList(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.QUESTLIST_DLG))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.QUESTLIST_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.QUESTLIST_DLG);
		}
	}

	public void Market(IUIObject obj)
	{
	}

	public void Post(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.POST_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.POST_DLG);
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				postDlg.ChangeTab_RecvList();
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.POST_DLG);
		}
	}

	public void ItemUpgrade(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.REFORGEMAIN_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGEMAIN_DLG);
		}
	}

	public void SolCombination(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMBINATION_DLG))
		{
			SolCombination_Dlg solCombination_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMBINATION_DLG) as SolCombination_Dlg;
			if (solCombination_Dlg == null)
			{
				Debug.LogError("ERROR, MainMenuDlg.cs, SolCombination(), SolCombination_Dlg is Null");
				return;
			}
			solCombination_Dlg.MakeCombinationSolUI(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetOwnBattleReadyAndReadySolKindList(), -1);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLCOMBINATION_DLG);
		}
	}

	public void MiniAlbum(IUIObject obj)
	{
		this.SendSolGuideInfo(false);
	}

	public void Community(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COMMUNITY_DLG))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COMMUNITY_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COMMUNITY_DLG);
		}
	}

	public void MyGuild(IUIObject obj)
	{
	}

	public void System(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SYSTEM_OPTION))
		{
			System_Option_Dlg system_Option_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SYSTEM_OPTION) as System_Option_Dlg;
			if (system_Option_Dlg != null)
			{
				system_Option_Dlg.CheckColosseumInvite();
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SYSTEM_OPTION);
		}
	}

	public void Exit(IUIObject obj)
	{
		NrMobileAuthSystem.Instance.RequestLogout = true;
		NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
		NrTSingleton<NrMainSystem>.Instance.QuitGame(false);
	}

	public void ChannelMove(IUIObject obj)
	{
		if (Scene.CurScene == Scene.Type.WORLD)
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.CHANNEL_MOVE_DLG))
			{
				WS_CHANNEL_LIST_REQ wS_CHANNEL_LIST_REQ = new WS_CHANNEL_LIST_REQ();
				wS_CHANNEL_LIST_REQ.i16Req = 6;
				SendPacket.GetInstance().SendObject(16777246, wS_CHANNEL_LIST_REQ);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHANNEL_MOVE_DLG);
			}
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("561"));
		}
	}

	public void ChangeIntroMsg(IUIObject obj)
	{
		if (!this.bChangeIntroMsg)
		{
			this.m_taIntroMsg.controlIsEnabled = true;
			this.m_taIntroMsg.Text = string.Empty;
			this.m_taIntroMsg.SetFocus();
			this.bChangeIntroMsg = true;
			this.m_BtnChangeIntroMsg.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("547");
		}
		else
		{
			this.m_taIntroMsg.controlIsEnabled = false;
			this.m_BtnChangeIntroMsg.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("557");
			this.bChangeIntroMsg = false;
			if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
			{
				this.m_taIntroMsg.Text = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
				{
					this.m_taIntroMsg.Text
				});
			}
			if (this.m_taIntroMsg.Text.Contains("*"))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("797"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			GS_CHARACTER_INTRO_MSG_SET_REQ gS_CHARACTER_INTRO_MSG_SET_REQ = new GS_CHARACTER_INTRO_MSG_SET_REQ();
			TKString.StringChar(this.m_taIntroMsg.Text, ref gS_CHARACTER_INTRO_MSG_SET_REQ.szIntromsg);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHARACTER_INTRO_MSG_SET_REQ, gS_CHARACTER_INTRO_MSG_SET_REQ);
		}
	}

	public void Challenge(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.CHALLENGE_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.CHALLENGE_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
		}
	}

	private void Test(IUIObject obj)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.ClickInit), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("1147"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("100"), eMsgType.MB_OK_CANCEL, 2);
	}

	private void ClickInit(object a_oObject)
	{
		GS_CHAR_REPUTEREWARD_REQ obj = new GS_CHAR_REPUTEREWARD_REQ();
		SendPacket.GetInstance().SendObject(1028, obj);
	}

	public void ItemShop(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_HEARTS, true);
	}

	public void ClickCoupon(IUIObject obj)
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsCouponUse())
		{
			return;
		}
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COUPON_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.COUPON_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COUPON_DLG);
		}
	}

	public void NPCAutoMove(IUIObject obj)
	{
		TsLog.LogWarning("NPCAutoMove", new object[0]);
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NPC_AUTOMOVE_DLG))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NPC_AUTOMOVE_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NPC_AUTOMOVE_DLG);
		}
	}

	public void Auction(IUIObject obj)
	{
		short auctionUseLevel = NrTSingleton<ContentsLimitManager>.Instance.GetAuctionUseLevel();
		if (auctionUseLevel > (short)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel())
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("628"),
				"count",
				auctionUseLevel
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (NrTSingleton<ContentsLimitManager>.Instance.IsHP_Auth())
		{
			if (!TsPlatform.IsEditor)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					" Auction Platform : [ ",
					NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea(),
					" ] .[ ",
					NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType(),
					"].[",
					PlayerPrefs.GetInt(NrPrefsKey.LAST_AUTH_PLATFORM),
					" ]"
				}));
				int @int = PlayerPrefs.GetInt(NrPrefsKey.LAST_AUTH_PLATFORM);
				if (@int == 1 && 0 >= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nConfirmCheck)
				{
					NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_EMAIL);
					return;
				}
			}
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null)
			{
				if (!TsPlatform.IsEditor)
				{
					WS_AUCTION_AUTHINFO_REQ wS_AUCTION_AUTHINFO_REQ = new WS_AUCTION_AUTHINFO_REQ();
					wS_AUCTION_AUTHINFO_REQ.i64SN = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nSerialNumber;
					SendPacket.GetInstance().SendObject(16777297, wS_AUCTION_AUTHINFO_REQ);
				}
				else
				{
					NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG);
				}
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG);
		}
	}

	public void SetAuctionRegisterInfo(int iRegisterNum)
	{
		if (0 < iRegisterNum)
		{
			this.m_bxAuction.Hide(false);
			this.m_bxAuction.SetText(iRegisterNum.ToString());
		}
		else
		{
			this.m_bxAuction.Hide(true);
		}
	}

	public void SendAuctionRegisterInfo()
	{
		short auctionUseLevel = NrTSingleton<ContentsLimitManager>.Instance.GetAuctionUseLevel();
		if (0 >= auctionUseLevel)
		{
			this.m_bxAuction.Hide(true);
			if (null != this.m_BtnGameInfo[4])
			{
				this.m_BtnGameInfo[4].SetEnabled(false);
			}
		}
		else
		{
			if (null != this.m_BtnGameInfo[4])
			{
				this.m_BtnGameInfo[4].SetEnabled(true);
			}
			this.m_bxAuction.Hide(true);
			GS_AUCTION_REGISTERINFO_REQ obj = new GS_AUCTION_REGISTERINFO_REQ();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUCTION_REGISTERINFO_REQ, obj);
		}
	}

	public void CheckCouponUse()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsCouponUse())
		{
			this.m_dtCoupon.Hide(false);
			this.m_btCoupon.Hide(false);
		}
		else
		{
			this.m_dtCoupon.Hide(true);
			this.m_btCoupon.Hide(true);
		}
	}

	public void SetHP_AuthMessageBox()
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2264");
			string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2265");
			msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), null, null, null, textFromInterface, textFromInterface2, eMsgType.MB_OK_CANCEL);
			msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2269"));
			msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("415"));
		}
	}

	public void MsgBoxOKEvent(object EventObject)
	{
		NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_HP_AUTH);
		NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.HP_AuthRequest = 1;
	}

	public void ClickGoCustomer(IUIObject obj)
	{
		NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_HELPQUESTION);
		if (this.m_bxCustomerNotice != null)
		{
			this.m_bxCustomerNotice.Hide(true);
		}
	}

	public void ShowHideButton()
	{
		for (int i = 0; i < 14; i++)
		{
			if (!(this.m_BtnGameInfo[i] == null))
			{
				if (!(this.m_dwButtonImg[i] == null))
				{
					if (!(this.m_lbMenuName[i] == null))
					{
						if (!(this.m_dwBGImg[i] == null))
						{
							this.m_BtnGameInfo[i].Visible = false;
							this.m_BtnGameInfo[i].controlIsEnabled = false;
							this.m_BtnGameInfo[i].data = -1;
							this.m_BtnGameInfo[i].RemoveValueChangedDelegate(new EZValueChangedDelegate(this.ClickButton));
							this.m_dwButtonImg[i].Visible = false;
							this.m_lbMenuName[i].Visible = false;
							this.m_dwBGImg[i].Visible = false;
						}
					}
				}
			}
		}
		int num = 0;
		for (int j = 0; j < 14; j++)
		{
			if (!(this.m_BtnGameInfo[j] == null))
			{
				if (!(this.m_dwButtonImg[j] == null))
				{
					if (!(this.m_lbMenuName[j] == null))
					{
						if (!(this.m_dwBGImg[j] == null))
						{
							if (TsPlatform.IsIPhone || TsPlatform.IsBand || TsPlatform.IsKakao || NrGlobalReference.strLangType.Equals("eng"))
							{
								if ((TsPlatform.IsIPhone || NrGlobalReference.strLangType.Equals("eng")) && j == 4)
								{
									goto IL_2DD;
								}
								if (TsPlatform.IsBand && j == 12)
								{
									goto IL_2DD;
								}
								if (TsPlatform.IsKakao && j == 12)
								{
									goto IL_2DD;
								}
							}
							if (NrTSingleton<ContentsLimitManager>.Instance.IsAuctionUse() || j != 4)
							{
								this.m_BtnGameInfo[num].controlIsEnabled = true;
								this.m_BtnGameInfo[num].data = j;
								this.m_BtnGameInfo[num].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickButton));
								this.m_BtnGameInfo[num].Visible = true;
								this.SetMenuButton(num, j);
								if (TsPlatform.IsBand && j == 11)
								{
									this.m_dwButtonImg[num].Visible = false;
								}
								else if (TsPlatform.IsKakao && j == 11)
								{
									if (!NrTSingleton<NkClientLogic>.Instance.IsGuestLogin())
									{
										this.m_dwButtonImg[num].Visible = false;
									}
									else
									{
										this.m_dwButtonImg[num].Visible = true;
									}
								}
								else
								{
									this.m_dwButtonImg[num].Visible = true;
								}
								this.m_lbMenuName[num].Visible = true;
								this.m_dwBGImg[num].Visible = true;
								num++;
							}
						}
					}
				}
			}
			IL_2DD:;
		}
	}

	public void SendSolGuideInfo(bool bElementMark)
	{
		GS_SOLGUIDE_INFO_REQ gS_SOLGUIDE_INFO_REQ = new GS_SOLGUIDE_INFO_REQ();
		gS_SOLGUIDE_INFO_REQ.bElementMark = bElementMark;
		gS_SOLGUIDE_INFO_REQ.i32CharKind = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLGUIDE_INFO_REQ, gS_SOLGUIDE_INFO_REQ);
	}

	public void ShowCustomerNotice(int answerCount)
	{
		if (this.m_bxCustomerNotice == null)
		{
			return;
		}
		if (answerCount <= 0)
		{
			return;
		}
		this.m_bxCustomerNotice.Hide(false);
		this.m_bxCustomerNotice.Text = answerCount.ToString();
		this.m_bxCustomerNotice.AlphaAni(1f, 0.5f, -0.5f);
	}

	private void RequestCustomerAnswerCount()
	{
		if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
		{
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().CustomerAnswerCount <= 0)
		{
			return;
		}
		GS_INQUIRE_ANSWER_COUNT_REQ obj = new GS_INQUIRE_ANSWER_COUNT_REQ();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INQUIRE_ANSWER_COUNT_REQ, obj);
	}
}
