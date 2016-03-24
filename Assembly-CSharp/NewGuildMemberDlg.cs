using GAME;
using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NewGuildMemberDlg : Form
{
	public enum eSORT
	{
		eSORT_NAME,
		eSORT_LEVEL,
		eSORT_CONTRIBUTE,
		eSORT_CONNECTTIME,
		eSORT_MAX
	}

	private Label m_lbMemberCount;

	private Label m_lbGuildName;

	private DrawTexture m_dtGuildMarkBG;

	private DrawTexture m_dtGuildMark;

	private Label m_lbGuildNotify;

	private NewListBox m_nlbMember;

	private Button m_btGuildList;

	private Button m_btMyGuildInfo;

	private Button m_btGuildBossRoom;

	private Box m_bxNotice;

	private Box m_bxNotice1;

	private Button m_btSortName;

	private Button m_btSortLevel;

	private Button m_btContribute;

	private Button m_btConnectTime;

	private DrawTexture[] m_dtSort = new DrawTexture[4];

	private DrawTexture[] m_dtSortHL = new DrawTexture[4];

	private Button m_btGuildWar;

	private Box m_bxGuildWarNotice;

	private Button m_btAgitCreate;

	private Button m_btAgitInfo;

	private Box m_bxAgitNotice;

	private Label m_lbGWarName;

	private float m_fShowTime;

	private List<NewGuildMember> m_MemberList = new List<NewGuildMember>();

	private List<NewGuildMember> m_SortList = new List<NewGuildMember>();

	private List<NewGuildMember> m_FirstList = new List<NewGuildMember>();

	private NewGuildMemberDlg.eSORT m_eSort;

	private bool m_bSortName = true;

	private bool m_bSortLevel;

	private bool m_bSortContribute;

	private bool m_bSortConnectTime;

	private float m_fSortDelayTime;

	private string m_strText = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_NewGuild_Member", G_ID.NEWGUILD_MEMBER_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_lbMemberCount = (base.GetControl("Label_MemberCount") as Label);
		this.m_lbMemberCount.SetText(string.Empty);
		this.m_lbGuildName = (base.GetControl("Label_GuildName") as Label);
		this.m_dtGuildMarkBG = (base.GetControl("DrawTexture_GuildMarkBG") as DrawTexture);
		this.m_dtGuildMarkBG.SetTextureFromBundle("UI/Etc/GuildImg03");
		this.m_dtGuildMark = (base.GetControl("DrawTexture_GuildMark") as DrawTexture);
		this.m_lbGuildNotify = (base.GetControl("LB_Notice") as Label);
		this.m_lbGuildNotify.SetText(string.Empty);
		this.m_nlbMember = (base.GetControl("NLB_GuildMemberList") as NewListBox);
		this.m_btGuildList = (base.GetControl("Button_GuildList") as Button);
		this.m_btGuildList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGuildList));
		this.m_btMyGuildInfo = (base.GetControl("BT_MyGuildInfo") as Button);
		this.m_btMyGuildInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMyGuildInfo));
		this.m_bxNotice = (base.GetControl("Box_Notice") as Box);
		this.m_bxNotice.Hide(true);
		this.m_btGuildBossRoom = (base.GetControl("BT_GbossInfo") as Button);
		Button expr_14C = this.m_btGuildBossRoom;
		expr_14C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_14C.Click, new EZValueChangedDelegate(this.OnClickGuildBoss));
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsGuildBoss())
		{
			this.m_btGuildBossRoom.Visible = false;
		}
		this.m_bxNotice1 = (base.GetControl("Box_Notice2") as Box);
		this.m_bxNotice1.Hide(true);
		this.m_btSortName = (base.GetControl("Button_SortName") as Button);
		this.m_btSortName.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSortName));
		this.m_btSortName.EffectAni = false;
		this.m_btSortLevel = (base.GetControl("Button_SortLevel") as Button);
		this.m_btSortLevel.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSortLevel));
		this.m_btSortLevel.EffectAni = false;
		this.m_btContribute = (base.GetControl("Button_SortContribute") as Button);
		this.m_btContribute.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSortContribute));
		this.m_btContribute.EffectAni = false;
		this.m_btConnectTime = (base.GetControl("Button_SortConnectTime") as Button);
		this.m_btConnectTime.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSortConnectTime));
		this.m_btConnectTime.EffectAni = false;
		this.m_dtSort[0] = (base.GetControl("DT_Arrow_Name") as DrawTexture);
		this.m_dtSort[1] = (base.GetControl("DT_Arrow_Level") as DrawTexture);
		this.m_dtSort[2] = (base.GetControl("DT_Arrow_Cont") as DrawTexture);
		this.m_dtSort[3] = (base.GetControl("DT_Arrow_Time") as DrawTexture);
		this.m_dtSortHL[0] = (base.GetControl("DT_LB_NAME_HL") as DrawTexture);
		this.m_dtSortHL[1] = (base.GetControl("DT_LB_State1_HL") as DrawTexture);
		this.m_dtSortHL[2] = (base.GetControl("DT_LB_State3_HL") as DrawTexture);
		this.m_dtSortHL[3] = (base.GetControl("DT_LB_State2_HL") as DrawTexture);
		this.m_btGuildWar = (base.GetControl("Button_GuildWar") as Button);
		this.m_btGuildWar.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGuildWar));
		this.m_bxGuildWarNotice = (base.GetControl("Box_Notice3") as Box);
		this.m_btAgitCreate = (base.GetControl("Button_Agit") as Button);
		this.m_btAgitCreate.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCreateAgit));
		this.m_btAgitInfo = (base.GetControl("Button_AgitInfo") as Button);
		this.m_btAgitInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAgitInfo));
		this.m_bxAgitNotice = (base.GetControl("Box_Notice4") as Box);
		this.m_lbGWarName = (base.GetControl("LB_GWarName") as Label);
		this.m_lbGWarName.SetText(string.Empty);
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(0);
		this.RefreshInfo();
		this.GuildBossCheck();
		this.SetLayerState();
		this.m_fShowTime = Time.time;
		this.SetLoadGuildMark();
		base.SetLayerZ(4, -1f);
	}

	public override void Update()
	{
		if (Time.time - this.m_fShowTime > 60f)
		{
			NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(0);
			this.m_fShowTime = Time.time;
		}
		if (0f < this.m_fSortDelayTime && this.m_fSortDelayTime <= Time.time)
		{
			this.m_fSortDelayTime = 0f;
		}
		base.Update();
	}

	public override void OnLoad()
	{
		NrSound.ImmedatePlay("UI_SFX", "GUILD", "OPEN");
	}

	public override void OnClose()
	{
		NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		NkInputManager.IsInputMode = true;
		base.OnClose();
		NrSound.ImmedatePlay("UI_SFX", "GUILD", "CLOSE");
	}

	public void ClickGuildList(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_LIST_DLG);
	}

	public void ClickMyGuildInfo(IUIObject obj)
	{
		if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			GS_NEWGUILD_DETAILINFO_REQ gS_NEWGUILD_DETAILINFO_REQ = new GS_NEWGUILD_DETAILINFO_REQ();
			gS_NEWGUILD_DETAILINFO_REQ.i64GuildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_DETAILINFO_REQ, gS_NEWGUILD_DETAILINFO_REQ);
		}
	}

	public void RefreshInfo()
	{
		this.SetLayerState();
		if (NrTSingleton<GuildWarManager>.Instance.CanGetGuildWarReward())
		{
			this.m_bxGuildWarNotice.Hide(false);
		}
		else
		{
			this.m_bxGuildWarNotice.Hide(true);
		}
		this.m_nlbMember.Clear();
		this.m_MemberList.Clear();
		this.m_SortList.Clear();
		this.m_FirstList.Clear();
		string str = string.Empty;
		if (NrTSingleton<NewGuildManager>.Instance.IsGuildWar())
		{
			str = NrTSingleton<CTextParser>.Instance.GetTextColor("1401");
		}
		this.m_lbGuildName.SetText(str + NrTSingleton<NewGuildManager>.Instance.GetGuildName());
		this.m_lbGuildNotify.SetText(NrTSingleton<NewGuildManager>.Instance.GetGuildNotice());
		int num = 0;
		for (int i = 0; i < NrTSingleton<NewGuildManager>.Instance.GetMemberCount(); i++)
		{
			NewGuildMember newGuildMember = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromIndex(i);
			if (newGuildMember != null)
			{
				this.m_MemberList.Add(newGuildMember);
			}
		}
		if (this.m_eSort == NewGuildMemberDlg.eSORT.eSORT_CONNECTTIME)
		{
			for (int i = 0; i < this.m_MemberList.Count; i++)
			{
				if (this.m_MemberList[i].IsConnected())
				{
					this.m_FirstList.Add(this.m_MemberList[i]);
				}
				else
				{
					this.m_SortList.Add(this.m_MemberList[i]);
				}
			}
			this.SortNewGuildMember(this.m_SortList);
			this.m_MemberList.Clear();
			for (int i = 0; i < this.m_FirstList.Count; i++)
			{
				this.m_MemberList.Add(this.m_FirstList[i]);
			}
			for (int i = 0; i < this.m_SortList.Count; i++)
			{
				this.m_MemberList.Add(this.m_SortList[i]);
			}
		}
		else
		{
			this.SortNewGuildMember(this.m_MemberList);
		}
		for (int i = 0; i < this.m_MemberList.Count; i++)
		{
			NewGuildMember newGuildMember = this.m_MemberList[i];
			if (newGuildMember != null)
			{
				if (this.SetGuildMemberUpdate(newGuildMember, i))
				{
					num++;
				}
			}
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2454"),
			"count1",
			num,
			"count2",
			NrTSingleton<NewGuildManager>.Instance.GetMaxMemberNum()
		});
		this.m_lbMemberCount.SetText(empty);
		for (int i = 0; i < this.m_MemberList.Count; i++)
		{
			NewGuildMember newGuildMember = this.m_MemberList[i];
			if (newGuildMember != null)
			{
				this.SetUserTexture(newGuildMember.GetPersonID());
			}
		}
		if (0 < NrTSingleton<NewGuildManager>.Instance.GetApplicantCount() && NrTSingleton<NewGuildManager>.Instance.IsDischargeMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			this.m_bxNotice.Hide(false);
			this.m_bxNotice.SetText(NrTSingleton<NewGuildManager>.Instance.GetApplicantCount().ToString());
		}
		else
		{
			this.m_bxNotice.Hide(true);
		}
		if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID() && NrTSingleton<NewGuildManager>.Instance.IsChangeGuildName(NrTSingleton<NewGuildManager>.Instance.GetGuildName()))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_CHANGENAME_DLG);
		}
		this.CheckAgitEnter();
	}

	public void SetUserTexture(long lPersonID)
	{
		if (lPersonID > 0L && lPersonID > 11L)
		{
			string userPortraitURL = NrTSingleton<NkCharManager>.Instance.GetUserPortraitURL(lPersonID);
			WebFileCache.RequestImageWebFile(userPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebUserStoryChatImageCallback), lPersonID);
		}
	}

	private void ReqWebUserStoryChatImageCallback(Texture2D txtr, object _param)
	{
		long i64PersonID = (long)_param;
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_MEMBER_DLG) && txtr != null)
		{
			this.SetUserTextureUpdate(txtr, i64PersonID);
		}
	}

	public bool SetGuildMemberUpdate(NewGuildMember GuildMember, int iIndex)
	{
		if (GuildMember == null)
		{
			return false;
		}
		NewListItem newListItem = new NewListItem(this.m_nlbMember.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(0, true);
		if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(GuildMember.GetFaceCharKind()) == null)
		{
			return false;
		}
		EVENT_HERODATA eventHeroCharFriendCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharFriendCode(GuildMember.GetFaceCharKind());
		if (eventHeroCharFriendCode != null)
		{
			newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
			newListItem.EventMark = true;
		}
		newListItem.SetListItemData(1, this.GetGuildMemberPortraitInfo(GuildMember), null, null, null);
		newListItem.SetListItemData(2, GuildMember.GetCharName(), null, null, null);
		newListItem.SetListItemData(3, string.Empty, GuildMember, new EZValueChangedDelegate(this.ClickRightMenu), null);
		string empty = string.Empty;
		string empty2 = string.Empty;
		bool result = NewGuildMemberDlg.CurrentLocationName(GuildMember, ref empty, ref empty2);
		newListItem.SetListItemData(4, NrTSingleton<CTextParser>.Instance.GetTextColor(empty2) + empty, null, null, null);
		string rankText = GuildMember.GetRankText();
		newListItem.SetListItemData(5, rankText, null, null, null);
		newListItem.SetListItemData(6, GuildMember.GetLevel().ToString(), null, null, null);
		newListItem.SetListItemData(7, GuildMember.GetContribute().ToString(), null, null, null);
		newListItem.Data = GuildMember;
		this.m_nlbMember.Add(newListItem);
		this.m_nlbMember.RepositionItems();
		return result;
	}

	public void SetUserTextureUpdate(Texture2D _Texture, long i64PersonID)
	{
		for (int i = 0; i < this.m_MemberList.Count; i++)
		{
			NewGuildMember newGuildMember = this.m_MemberList[i];
			if (newGuildMember == null)
			{
				return;
			}
			if (i64PersonID == newGuildMember.GetPersonID())
			{
				NewListItem newListItem = new NewListItem(this.m_nlbMember.ColumnNum, true, string.Empty);
				newListItem.SetListItemData(0, true);
				newListItem.SetListItemData(1, _Texture, null, null, null, null);
				newListItem.SetListItemData(2, newGuildMember.GetCharName(), null, null, null);
				newListItem.SetListItemData(3, string.Empty, newGuildMember, new EZValueChangedDelegate(this.ClickRightMenu), null);
				string empty = string.Empty;
				string empty2 = string.Empty;
				NewGuildMemberDlg.CurrentLocationName(newGuildMember, ref empty, ref empty2);
				newListItem.SetListItemData(4, NrTSingleton<CTextParser>.Instance.GetTextColor(empty2) + empty, null, null, null);
				string rankText = newGuildMember.GetRankText();
				newListItem.SetListItemData(5, rankText, null, null, null);
				newListItem.SetListItemData(6, newGuildMember.GetLevel().ToString(), null, null, null);
				newListItem.SetListItemData(7, newGuildMember.GetContribute().ToString(), null, null, null);
				newListItem.Data = newGuildMember;
				this.m_nlbMember.RemoveAdd(i, newListItem);
				this.m_nlbMember.RepositionItems();
				break;
			}
		}
	}

	private void ClickRightMenu(IUIObject obj)
	{
		if (TsPlatform.IsWeb)
		{
			NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		}
		else if (NrTSingleton<CRightClickMenu>.Instance.IsOpen())
		{
			TsLog.Log("CloseUI(CRightClickMenu.CLOSEOPTION.CLICK", new object[0]);
			NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
			return;
		}
		NewGuildMember newGuildMember = obj.Data as NewGuildMember;
		if (newGuildMember == null)
		{
			return;
		}
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char.GetPersonID() == newGuildMember.GetPersonID())
		{
			return;
		}
		bool flag;
		if (NrTSingleton<NewGuildManager>.Instance.IsMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			if (NrTSingleton<NewGuildManager>.Instance.IsMaster(newGuildMember.GetPersonID()) || NrTSingleton<NewGuildManager>.Instance.IsSubMaster(newGuildMember.GetPersonID()))
			{
				flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(newGuildMember.GetPersonID(), 0, newGuildMember.GetCharName(), CRightClickMenu.KIND.GUILD_MASTER_SELECT_CLICK, CRightClickMenu.TYPE.SIMPLE_SECTION_1, false);
			}
			else
			{
				flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(newGuildMember.GetPersonID(), 0, newGuildMember.GetCharName(), CRightClickMenu.KIND.GUILD_MASTER_CLICK, CRightClickMenu.TYPE.SIMPLE_SECTION_1, false);
			}
		}
		else if (NrTSingleton<NewGuildManager>.Instance.IsSubMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			if (NrTSingleton<NewGuildManager>.Instance.IsMaster(newGuildMember.GetPersonID()) || NrTSingleton<NewGuildManager>.Instance.IsSubMaster(newGuildMember.GetPersonID()))
			{
				flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(newGuildMember.GetPersonID(), 0, newGuildMember.GetCharName(), CRightClickMenu.KIND.GUILD_SUBMASTER_SELECT_CLICK, CRightClickMenu.TYPE.SIMPLE_SECTION_1, false);
			}
			else
			{
				flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(newGuildMember.GetPersonID(), 0, newGuildMember.GetCharName(), CRightClickMenu.KIND.GUILD_SUBMASTER_CLICK, CRightClickMenu.TYPE.SIMPLE_SECTION_1, false);
			}
		}
		else
		{
			flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(newGuildMember.GetPersonID(), 0, newGuildMember.GetCharName(), CRightClickMenu.KIND.GUILD_MEMBER_CLICK, CRightClickMenu.TYPE.SIMPLE_SECTION_1, false);
		}
		Button button = obj as Button;
		if (button != null && flag)
		{
			float x = this.m_nlbMember.GetSize().x;
			float height = 28f;
			float left = base.GetLocation().x + this.m_nlbMember.GetLocation().x + button.gameObject.transform.localPosition.x;
			float top = base.GetLocationY() + this.m_nlbMember.GetLocationY() + -button.gameObject.transform.localPosition.y;
			Rect windowRect = new Rect(left, top, x, height);
			NrTSingleton<CRightClickMenu>.Instance.SetWindowRect(windowRect);
		}
	}

	public static bool CurrentLocationName(NewGuildMember GuildMember, ref string strName, ref string ColorNum)
	{
		bool result = false;
		short num = GuildMember.GetChannelID();
		int mapUnique = GuildMember.GetMapUnique();
		string mapNameFromUnique = NrTSingleton<MapManager>.Instance.GetMapNameFromUnique(mapUnique);
		if (0 >= num || 0 >= mapUnique || mapNameFromUnique == string.Empty)
		{
			string text = string.Empty;
			if (0L < GuildMember.GetLogOffTime())
			{
				long iSec = PublicMethod.GetCurTime() - GuildMember.GetLogOffTime();
				long totalDayFromSec = PublicMethod.GetTotalDayFromSec(iSec);
				long hourFromSec = PublicMethod.GetHourFromSec(iSec);
				long minuteFromSec = PublicMethod.GetMinuteFromSec(iSec);
				if (totalDayFromSec > 0L)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2173");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref strName, new object[]
					{
						text,
						"count",
						totalDayFromSec
					});
				}
				else if (hourFromSec > 0L)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2172");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref strName, new object[]
					{
						text,
						"count",
						hourFromSec
					});
				}
				else
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2171");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref strName, new object[]
					{
						text,
						"count",
						minuteFromSec
					});
				}
			}
			else
			{
				strName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2421");
			}
			if (GuildMember.GetConnected())
			{
				GS_NEWGUILD_MEMBER_LOG_REQ gS_NEWGUILD_MEMBER_LOG_REQ = new GS_NEWGUILD_MEMBER_LOG_REQ();
				gS_NEWGUILD_MEMBER_LOG_REQ.i16ChannelID = num;
				gS_NEWGUILD_MEMBER_LOG_REQ.i32MapUnique = mapUnique;
				TKString.StringChar(mapNameFromUnique, ref gS_NEWGUILD_MEMBER_LOG_REQ.strMapName);
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_MEMBER_LOG_REQ, gS_NEWGUILD_MEMBER_LOG_REQ);
			}
		}
		else
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1777");
			byte b = 200;
			num = (short)((byte)(num - (short)b));
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
			{
				textFromInterface,
				"count",
				num
			});
			strName = string.Format("{0}({1})", mapNameFromUnique, textFromInterface);
			ColorNum = "1104";
			result = true;
		}
		return result;
	}

	public void SetLayerState()
	{
		base.SetShowLayer(2, true);
		if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			base.SetShowLayer(1, true);
		}
		else
		{
			base.SetShowLayer(1, false);
		}
		bool visible = false;
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsNewGuildWarLimit())
		{
			visible = true;
		}
		this.m_btGuildWar.Visible = visible;
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsGuildBoss())
		{
			this.m_btGuildBossRoom.Visible = false;
		}
		if (NrTSingleton<ContentsLimitManager>.Instance.IsAgitLimit())
		{
			this.m_btAgitCreate.Visible = false;
			this.m_btAgitInfo.Visible = false;
		}
		else
		{
			this.m_btAgitCreate.Visible = true;
			if (NrTSingleton<NewGuildManager>.Instance.GetAgitLevel() > 0)
			{
				this.m_btAgitInfo.Visible = true;
			}
			else
			{
				this.m_btAgitInfo.Visible = false;
			}
		}
	}

	public void OnClickGuildBoss(IUIObject obj)
	{
		if (NrTSingleton<NewGuildManager>.Instance.GetGuildID() <= 0L)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("545"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		BabelGuildBossDlg babelGuildBossDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABEL_GUILDBOSS_MAIN_DLG) as BabelGuildBossDlg;
		if (babelGuildBossDlg != null)
		{
			babelGuildBossDlg.ShowList();
		}
		this.Close();
	}

	public void GuildBossCheck()
	{
		bool flag = false;
		bool guildBossRewardInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossRewardInfo();
		if (guildBossRewardInfo)
		{
			flag = true;
		}
		bool guildBossCheck = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossCheck();
		if (guildBossCheck)
		{
			flag = true;
		}
		if (flag)
		{
			this.m_bxNotice1.Hide(false);
		}
		else
		{
			this.m_bxNotice1.Hide(true);
		}
	}

	public void SortNewGuildMember(List<NewGuildMember> MemberList)
	{
		switch (this.m_eSort)
		{
		case NewGuildMemberDlg.eSORT.eSORT_NAME:
			this.ChangeSortTexture(this.m_bSortName);
			if (this.m_bSortName)
			{
				MemberList.Sort(new Comparison<NewGuildMember>(this.CompareNameDESC));
			}
			else
			{
				MemberList.Sort(new Comparison<NewGuildMember>(this.CompareNameASC));
			}
			break;
		case NewGuildMemberDlg.eSORT.eSORT_LEVEL:
			this.ChangeSortTexture(this.m_bSortLevel);
			if (this.m_bSortLevel)
			{
				MemberList.Sort(new Comparison<NewGuildMember>(this.CompareLevelDESC));
			}
			else
			{
				MemberList.Sort(new Comparison<NewGuildMember>(this.CompareLevelASC));
			}
			break;
		case NewGuildMemberDlg.eSORT.eSORT_CONTRIBUTE:
			this.ChangeSortTexture(this.m_bSortContribute);
			if (this.m_bSortContribute)
			{
				MemberList.Sort(new Comparison<NewGuildMember>(this.CompareContributeDESC));
			}
			else
			{
				MemberList.Sort(new Comparison<NewGuildMember>(this.CompareContributeASC));
			}
			break;
		case NewGuildMemberDlg.eSORT.eSORT_CONNECTTIME:
			this.ChangeSortTexture(this.m_bSortConnectTime);
			if (this.m_bSortConnectTime)
			{
				MemberList.Sort(new Comparison<NewGuildMember>(this.CompareConnectTimeDESC));
			}
			else
			{
				MemberList.Sort(new Comparison<NewGuildMember>(this.CompareConnectTimeASC));
			}
			break;
		}
	}

	private int CompareNameDESC(NewGuildMember a, NewGuildMember b)
	{
		int num = b.GetRank().CompareTo(a.GetRank());
		if (num == 0)
		{
			return b.GetCharName().CompareTo(a.GetCharName());
		}
		return num;
	}

	private int CompareNameASC(NewGuildMember a, NewGuildMember b)
	{
		int num = a.GetRank().CompareTo(b.GetRank());
		if (num == 0)
		{
			return a.GetCharName().CompareTo(b.GetCharName());
		}
		return num;
	}

	private int CompareLevelDESC(NewGuildMember a, NewGuildMember b)
	{
		return b.GetLevel().CompareTo(a.GetLevel());
	}

	private int CompareLevelASC(NewGuildMember a, NewGuildMember b)
	{
		return a.GetLevel().CompareTo(b.GetLevel());
	}

	private int CompareContributeDESC(NewGuildMember a, NewGuildMember b)
	{
		return b.GetContribute().CompareTo(a.GetContribute());
	}

	private int CompareContributeASC(NewGuildMember a, NewGuildMember b)
	{
		return a.GetContribute().CompareTo(b.GetContribute());
	}

	private int CompareConnectTimeDESC(NewGuildMember a, NewGuildMember b)
	{
		return a.GetLogOffTime().CompareTo(b.GetLogOffTime());
	}

	private int CompareConnectTimeASC(NewGuildMember a, NewGuildMember b)
	{
		return b.GetLogOffTime().CompareTo(a.GetLogOffTime());
	}

	public void OnClickSortName(IUIObject obj)
	{
		if (!this.SetSortDelayTime())
		{
			return;
		}
		this.m_eSort = NewGuildMemberDlg.eSORT.eSORT_NAME;
		this.m_bSortName = !this.m_bSortName;
		this.RefreshInfo();
	}

	public void OnClickSortLevel(IUIObject obj)
	{
		if (!this.SetSortDelayTime())
		{
			return;
		}
		this.m_eSort = NewGuildMemberDlg.eSORT.eSORT_LEVEL;
		this.m_bSortLevel = !this.m_bSortLevel;
		this.RefreshInfo();
	}

	public void OnClickSortContribute(IUIObject obj)
	{
		if (!this.SetSortDelayTime())
		{
			return;
		}
		this.m_eSort = NewGuildMemberDlg.eSORT.eSORT_CONTRIBUTE;
		this.m_bSortContribute = !this.m_bSortContribute;
		this.RefreshInfo();
	}

	public void OnClickSortConnectTime(IUIObject obj)
	{
		if (!this.SetSortDelayTime())
		{
			return;
		}
		this.m_eSort = NewGuildMemberDlg.eSORT.eSORT_CONNECTTIME;
		this.m_bSortConnectTime = !this.m_bSortConnectTime;
		this.RefreshInfo();
	}

	private bool SetSortDelayTime()
	{
		if (0f < this.m_fSortDelayTime)
		{
			return false;
		}
		this.m_fSortDelayTime = Time.time + 1f;
		return true;
	}

	public void ChangeSortTexture(bool bSort)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.m_eSort == (NewGuildMemberDlg.eSORT)i)
			{
				this.m_dtSort[i].Visible = true;
				this.m_dtSortHL[i].Visible = true;
			}
			else
			{
				this.m_dtSort[i].Visible = false;
				this.m_dtSortHL[i].Visible = false;
			}
		}
		if (bSort)
		{
			this.m_dtSort[(int)this.m_eSort].SetTextureKey("Win_I_ArrowDown");
		}
		else
		{
			this.m_dtSort[(int)this.m_eSort].SetTextureKey("Win_I_ArrowUp");
		}
	}

	public void SetLoadGuildMark()
	{
		if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(NrTSingleton<NewGuildManager>.Instance.GetGuildID());
			WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebGuildImageCallback), this.m_dtGuildMark);
		}
	}

	public void SetGuildWarEnemyString(string strGuildWarEnemyName)
	{
		if (strGuildWarEnemyName == string.Empty || strGuildWarEnemyName == string.Empty)
		{
			this.m_lbGWarName.SetText(string.Empty);
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2946"),
			"targetname",
			strGuildWarEnemyName
		});
		this.m_lbGWarName.SetText(empty);
	}

	public void SetAgitNoty()
	{
		this.m_bxAgitNotice.Hide(!NrTSingleton<NewGuildManager>.Instance.CanGetGoldenEggReward());
	}

	private void ReqWebGuildImageCallback(Texture2D txtr, object _param)
	{
		DrawTexture drawTexture = (DrawTexture)_param;
		if (txtr == null)
		{
			drawTexture.SetTexture(NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture());
		}
		else
		{
			drawTexture.SetTexture(txtr);
		}
	}

	public void ClickGuildWar(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_INFO_REQ();
	}

	public void ClickCreateAgit(IUIObject obj)
	{
		if (NrTSingleton<NewGuildManager>.Instance.GetAgitLevel() <= 0)
		{
			NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID);
			if (memberInfoFromPersonID == null)
			{
				return;
			}
			if (memberInfoFromPersonID.GetRank() < NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_OFFICER)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("769"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			short num = 1;
			AgitInfoData agitData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitData(num.ToString());
			if (agitData == null)
			{
				return;
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("235"),
				"count",
				agitData.i64NeedGuildFund
			});
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.MsgOKCreateAgit), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("234"), this.m_strText, eMsgType.MB_OK_CANCEL, 2);
		}
		else
		{
			NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_AGIT_ENTER_REQ();
		}
	}

	public void MsgOKCreateAgit(object a_oObject)
	{
		short num = 1;
		AgitInfoData agitData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitData(num.ToString());
		if (agitData == null)
		{
			return;
		}
		if (NrTSingleton<NewGuildManager>.Instance.GetFund() < agitData.i64NeedGuildFund)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("754"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_AGIT_CREATE_REQ();
	}

	public void ClickAgitInfo(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AGIT_MAIN_DLG);
	}

	public void CheckAgitEnter()
	{
		if (NrTSingleton<NewGuildManager>.Instance.GetAgitLevel() <= 0)
		{
			this.m_btAgitCreate.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2703"));
		}
		else
		{
			this.m_btAgitCreate.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2720"));
			this.m_btAgitCreate.Hide(NrTSingleton<MapManager>.Instance.CurrentMapIndex == 12);
			this.m_bxAgitNotice.Visible = (NrTSingleton<MapManager>.Instance.CurrentMapIndex != 12);
		}
		this.SetAgitNoty();
	}

	private NkListSolInfo GetGuildMemberPortraitInfo(NewGuildMember guildMember)
	{
		if (guildMember == null)
		{
			return null;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(guildMember.GetFaceCharKind());
		NkListSolInfo nkListSolInfo = new NkListSolInfo();
		if (charKindInfo != null)
		{
			nkListSolInfo.SolCharKind = charKindInfo.GetCharKind();
		}
		nkListSolInfo.SolGrade = -1;
		nkListSolInfo.SolInjuryStatus = false;
		nkListSolInfo.ShowCombat = false;
		nkListSolInfo.ShowGrade = false;
		nkListSolInfo.ShowLevel = false;
		nkListSolInfo.SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(guildMember.GetCostumeUnique());
		return nkListSolInfo;
	}
}
