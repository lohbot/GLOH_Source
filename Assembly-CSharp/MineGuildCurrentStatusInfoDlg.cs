using GAME;
using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class MineGuildCurrentStatusInfoDlg : Form
{
	private const int ONE_PAGE_COUNT = 4;

	private List<MINE_GUILD_CURRENTSTATUS_INFO> m_listMineGuild_CurrentStatus = new List<MINE_GUILD_CURRENTSTATUS_INFO>();

	private NewListBox m_lbCurrentStatus;

	private Button m_btMineSearch;

	private Box m_bxPage;

	private Button m_btPagePrev;

	private Button m_btPageNext;

	private Button m_btRefresh;

	private Label m_lagoMineJoinCount;

	private int m_Page;

	private int m_MaxPage;

	private long m_GuildID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Mine/dlg_minecondition", G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_lbCurrentStatus = (base.GetControl("NLB_MineCondition") as NewListBox);
		this.m_lbCurrentStatus.Reserve = false;
		this.m_btMineSearch = (base.GetControl("BT_MineScarch") as Button);
		Button expr_3E = this.m_btMineSearch;
		expr_3E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3E.Click, new EZValueChangedDelegate(this.OnClickSearchMine));
		this.m_bxPage = (base.GetControl("Box_Page") as Box);
		this.m_btPagePrev = (base.GetControl("BT_Page01") as Button);
		Button expr_91 = this.m_btPagePrev;
		expr_91.Click = (EZValueChangedDelegate)Delegate.Combine(expr_91.Click, new EZValueChangedDelegate(this.OnClickPagePrev));
		this.m_btPageNext = (base.GetControl("BT_Page02") as Button);
		Button expr_CE = this.m_btPageNext;
		expr_CE.Click = (EZValueChangedDelegate)Delegate.Combine(expr_CE.Click, new EZValueChangedDelegate(this.OnClickPageNext));
		this.m_btRefresh = (base.GetControl("BT_Refresh02") as Button);
		Button expr_10B = this.m_btRefresh;
		expr_10B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_10B.Click, new EZValueChangedDelegate(this.OnClickRefresh));
		this.m_lagoMineJoinCount = (base.GetControl("Label_Label18") as Label);
		base.SetScreenCenter();
		this.m_GuildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		this.Hide();
	}

	public void SetGuildID(long Guild)
	{
		this.m_GuildID = Guild;
	}

	public void Show(int page, int totalcount)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		this.m_Page = page;
		this.m_MaxPage = totalcount / 4;
		if (totalcount % 4 != 0)
		{
			this.m_MaxPage++;
		}
		string text = string.Empty;
		string empty = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1842");
		MINE_CONSTANT_Manager instance = MINE_CONSTANT_Manager.GetInstance();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"count1",
			kMyCharInfo.GetCharDetail(8),
			"count2",
			instance.GetValue(eMINE_CONSTANT.eMINE_DAY_COUNT)
		});
		this.m_lagoMineJoinCount.SetText(empty);
		this.SetList();
		if (!base.ShowHide)
		{
			base.Show();
		}
	}

	public override void Update()
	{
		this.UpdateTime();
		base.Update();
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
	}

	public void AddInfo(MINE_GUILD_CURRENTSTATUS_INFO[] info)
	{
		this.m_lbCurrentStatus.Clear();
		this.m_listMineGuild_CurrentStatus.Clear();
		for (int i = 0; i < info.Length; i++)
		{
			this.m_listMineGuild_CurrentStatus.Add(info[i]);
		}
	}

	public void SetList()
	{
		this.m_lbCurrentStatus.Clear();
		string text = string.Empty;
		string text2 = string.Empty;
		int num = 0;
		foreach (MINE_GUILD_CURRENTSTATUS_INFO current in this.m_listMineGuild_CurrentStatus)
		{
			NewListItem newListItem = new NewListItem(this.m_lbCurrentStatus.ColumnNum, true);
			if (current.byCurrentStatusType == 1 || current.byCurrentStatusType == 0)
			{
				if (current.i64AttackGuildID > 0L)
				{
					newListItem.SetListItemData(1, TKString.NEWString(current.szAttackGuildName), null, null, null);
					newListItem.SetListItemData(5, string.Empty, current, new EZValueChangedDelegate(this.OnClickAttackMilitaryInfo), null);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1629");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text,
						"count",
						current.byAttackMilitaryCount,
						"count2",
						9
					});
					newListItem.SetListItemData(9, text2, null, null, null);
					newListItem.SetListItemData(11, string.Empty, null, null, null);
				}
				else
				{
					newListItem.SetListItemData(1, string.Empty, null, null, null);
					newListItem.SetListItemData(9, string.Empty, null, null, null);
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1632");
					newListItem.SetListItemData(11, text2, null, null, null);
				}
				MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(current.i16MineDataID);
				if (mineCreateDataFromID == null)
				{
					continue;
				}
				MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(BASE_MINE_DATA.ParseGradeFromString(mineCreateDataFromID.MINE_GRADE));
				if (mineDataFromGrade != null)
				{
					UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(mineDataFromGrade.MINE_ICON_NAME);
					if (uIBaseInfoLoader != null)
					{
						newListItem.SetListItemData(6, uIBaseInfoLoader, current, new EZValueChangedDelegate(this.OnClickDefenceMilitaryInfo), null);
					}
				}
				if (current.i64DefenceGuildID > 0L)
				{
					text2 = TKString.NEWString(current.szDefenceGuildName);
					if (100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel)
					{
						text2 = TKString.NEWString(current.szDefenceGuildName) + " " + current.i64MineID.ToString();
					}
					newListItem.SetListItemData(2, text2, null, null, null);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1629");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text,
						"count",
						current.byDefenceMilitaryCount,
						"count2",
						9
					});
					newListItem.SetListItemData(10, text2, null, null, null);
					newListItem.SetListItemData(12, string.Empty, null, null, null);
					newListItem.SetListItemData(13, BASE_MINE_DATA.GetMineName(mineCreateDataFromID.GetGrade(), current.i16MineDataID), null, null, null);
				}
				else
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1325");
					if (100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel)
					{
						text2 = text2 + " " + current.i64MineID.ToString();
					}
					newListItem.SetListItemData(2, text2, null, null, null);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text,
						"count",
						9,
						"count2",
						9
					});
					newListItem.SetListItemData(10, text2, null, null, null);
					newListItem.SetListItemData(12, string.Empty, null, null, null);
					newListItem.SetListItemData(13, BASE_MINE_DATA.GetMineName(mineCreateDataFromID.GetGrade(), current.i16MineDataID), null, null, null);
				}
				text2 = this.GetTextFromState(current);
				if (current.byMilitaryState == 2 || current.byMilitaryState == 3 || current.byMilitaryState == 6 || current.byMilitaryState == 7)
				{
					newListItem.SetListItemData(0, false);
					newListItem.SetListItemData(3, string.Empty, null, null, null);
					newListItem.SetListItemData(4, text2, null, null, null);
				}
				else
				{
					string data = string.Empty;
					newListItem.SetListItemData(0, true);
					if (current.byJoinMilitary == 0)
					{
						data = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1323");
						UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_B_BasicBtnB01");
						newListItem.SetListItemData(0, loader, current, new EZValueChangedDelegate(this.OnClickMilitaryBatch), null);
						newListItem.SetListItemData2(0, data);
					}
					else if (current.byJoinMilitary == 1)
					{
						data = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615");
						UIBaseInfoLoader loader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_B_BasicBtnB02");
						newListItem.SetListItemData(0, loader2, current, new EZValueChangedDelegate(this.OnClickMilitaryReturn), null);
						newListItem.SetListItemData2(0, data);
					}
					newListItem.SetListItemData(3, text2, null, null, null);
					newListItem.SetListItemData(4, string.Empty, null, null, null);
				}
			}
			else if (current.byCurrentStatusType == 2)
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				newListItem.SetListItemData(1, @char.GetCharName(), null, null, null);
				MINE_CREATE_DATA mineCreateDataFromID2 = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(current.i16MineDataID);
				if (mineCreateDataFromID2 == null)
				{
					continue;
				}
				MINE_DATA mineDataFromGrade2 = BASE_MINE_DATA.GetMineDataFromGrade(BASE_MINE_DATA.ParseGradeFromString(mineCreateDataFromID2.MINE_GRADE));
				if (mineDataFromGrade2 != null)
				{
					UIBaseInfoLoader uIBaseInfoLoader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(mineDataFromGrade2.MINE_ICON_NAME);
					if (uIBaseInfoLoader2 != null)
					{
						newListItem.SetListItemData(6, uIBaseInfoLoader2, current, new EZValueChangedDelegate(this.OnClickDefenceMilitaryInfo), null);
					}
				}
				if (current.i64DefenceGuildID > 0L)
				{
					newListItem.SetListItemData(2, TKString.NEWString(current.szDefenceGuildName), null, null, null);
					newListItem.SetListItemData(4, string.Empty, null, null, null);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1629");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text,
						"count",
						current.byDefenceMilitaryCount,
						"count2",
						9
					});
					newListItem.SetListItemData(10, text2, null, null, null);
					newListItem.SetListItemData(12, string.Empty, null, null, null);
					newListItem.SetListItemData(13, BASE_MINE_DATA.GetMineName(mineCreateDataFromID2.GetGrade(), current.i16MineDataID), null, null, null);
				}
				else
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1325");
					if (100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel)
					{
						text2 = text2 + " " + current.i64MineID.ToString();
					}
					newListItem.SetListItemData(2, text2, null, null, null);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text,
						"count",
						9,
						"count2",
						9
					});
					newListItem.SetListItemData(10, text2, null, null, null);
					newListItem.SetListItemData(12, string.Empty, null, null, null);
					newListItem.SetListItemData(13, BASE_MINE_DATA.GetMineName(mineCreateDataFromID2.GetGrade(), current.i16MineDataID), null, null, null);
				}
				text2 = this.GetTextFromState(current);
				newListItem.SetListItemData(3, string.Empty, null, null, null);
				newListItem.SetListItemData(4, text2, null, null, null);
			}
			newListItem.Data = current;
			this.m_lbCurrentStatus.Add(newListItem);
			num++;
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1633");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"count1",
			this.m_Page,
			"count2",
			this.m_MaxPage
		});
		this.m_bxPage.SetText(text2);
		this.SetListAttackGuildMark();
		this.m_lbCurrentStatus.RepositionItems();
	}

	public string GetTextFromState(MINE_GUILD_CURRENTSTATUS_INFO info)
	{
		string text = string.Empty;
		string empty = string.Empty;
		string result = string.Empty;
		switch (info.byMilitaryState)
		{
		case 1:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1525");
			break;
		case 2:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1526");
			break;
		case 3:
		case 6:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1623");
			break;
		case 4:
		case 5:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1622");
			break;
		case 7:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1661");
			break;
		}
		result = text;
		if (info.byMilitaryState == 1 || info.byMilitaryState == 2)
		{
			long num = info.i64LeftTime - PublicMethod.GetCurTime();
			if (num >= 0L)
			{
				long num2 = num / 60L / 60L % 24L;
				long num3 = num / 60L % 60L;
				long num4 = num % 60L;
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1527");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromInterface,
					"hour",
					num2,
					"min",
					num3,
					"sec",
					num4
				});
				result = string.Format("{0} {1}", text, empty);
			}
			else if (info.byCurrentStatusType == 1 || info.byCurrentStatusType == 0)
			{
				if (info.byMilitaryState == 1)
				{
					info.byMilitaryState = 6;
				}
				else if (info.byMilitaryState == 2)
				{
					info.byMilitaryState = 7;
				}
			}
			else if (info.byCurrentStatusType == 2)
			{
				if (info.i64DefenceGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
				{
					if (info.byMilitaryState == 1 || info.byMilitaryState == 2)
					{
						info.byMilitaryState = 7;
					}
				}
				else if (info.byMilitaryState == 1)
				{
					info.byMilitaryState = 6;
				}
				else if (info.byMilitaryState == 2)
				{
					info.byMilitaryState = 7;
				}
			}
		}
		return result;
	}

	public void UpdateTime()
	{
		for (int i = 0; i < this.m_lbCurrentStatus.Count; i++)
		{
			IUIListObject item = this.m_lbCurrentStatus.GetItem(i);
			MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = item.Data as MINE_GUILD_CURRENTSTATUS_INFO;
			if (mINE_GUILD_CURRENTSTATUS_INFO != null)
			{
				if (mINE_GUILD_CURRENTSTATUS_INFO.i64LeftTime > 0L)
				{
					string text = string.Empty;
					string text2 = string.Empty;
					if (mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 1 || mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 0)
					{
						if (mINE_GUILD_CURRENTSTATUS_INFO.byMilitaryState == 2 || mINE_GUILD_CURRENTSTATUS_INFO.byMilitaryState == 3 || mINE_GUILD_CURRENTSTATUS_INFO.byMilitaryState == 6 || mINE_GUILD_CURRENTSTATUS_INFO.byMilitaryState == 7)
						{
							text = string.Empty;
							text2 = this.GetTextFromState(mINE_GUILD_CURRENTSTATUS_INFO);
							UIButton uIButton = ((UIListItemContainer)item).GetElement(0) as UIButton;
							if (null != uIButton && uIButton.Visible)
							{
								uIButton.Visible = false;
							}
						}
						else
						{
							text = this.GetTextFromState(mINE_GUILD_CURRENTSTATUS_INFO);
							text2 = string.Empty;
						}
					}
					else if (mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 2)
					{
						UIButton uIButton2 = ((UIListItemContainer)item).GetElement(0) as UIButton;
						if (null != uIButton2)
						{
							uIButton2.Visible = false;
							text = string.Empty;
							text2 = this.GetTextFromState(mINE_GUILD_CURRENTSTATUS_INFO);
						}
					}
					Label label = ((UIListItemContainer)item).GetElement(3) as Label;
					if (null != label)
					{
						label.Text = text;
					}
					Label label2 = ((UIListItemContainer)item).GetElement(4) as Label;
					if (null != label2)
					{
						label2.Text = text2;
					}
				}
			}
		}
	}

	public void SetListAttackGuildMark()
	{
		for (int i = 0; i < this.m_lbCurrentStatus.Count; i++)
		{
			IUIListObject item = this.m_lbCurrentStatus.GetItem(i);
			if (item != null)
			{
				MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = item.Data as MINE_GUILD_CURRENTSTATUS_INFO;
				if (mINE_GUILD_CURRENTSTATUS_INFO != null)
				{
					if (mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 1 || mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 0)
					{
						if (mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID > 0L)
						{
							DrawTexture drawTexture = ((UIListItemContainer)item).GetElement(8) as DrawTexture;
							if (drawTexture != null)
							{
								string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID);
								WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebGuildImageCallback), drawTexture);
							}
						}
					}
					else if (mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 2)
					{
						DrawTexture drawTexture2 = ((UIListItemContainer)item).GetElement(8) as DrawTexture;
						if (drawTexture2 != null)
						{
							NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
							drawTexture2.SetTexture(eCharImageType.SMALL, kMyCharInfo.GetImgFaceCharKind(), -1);
						}
					}
				}
			}
		}
	}

	public void RefreshList()
	{
		if (0L >= this.m_GuildID)
		{
			return;
		}
		GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ = new GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ();
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.i64GuildID = this.m_GuildID;
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.i32Page = this.m_Page;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ, gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ);
	}

	public void OnClickAttackMilitaryInfo(IUIObject obj)
	{
		MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = obj.Data as MINE_GUILD_CURRENTSTATUS_INFO;
		if (mINE_GUILD_CURRENTSTATUS_INFO != null)
		{
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64MineID <= 0L)
			{
				return;
			}
			if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				return;
			}
			string message = string.Empty;
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
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
			byte nMode;
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				nMode = 4;
			}
			else
			{
				nMode = 5;
			}
			GS_MINE_SEARCH_REQ gS_MINE_SEARCH_REQ = new GS_MINE_SEARCH_REQ();
			gS_MINE_SEARCH_REQ.bySearchMineGrade = 0;
			gS_MINE_SEARCH_REQ.m_nMineID = mINE_GUILD_CURRENTSTATUS_INFO.i64MineID;
			gS_MINE_SEARCH_REQ.m_nGuildID = mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID;
			gS_MINE_SEARCH_REQ.m_nMode = nMode;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_SEARCH_REQ, gS_MINE_SEARCH_REQ);
		}
	}

	public void OnClickDefenceMilitaryInfo(IUIObject obj)
	{
		MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = obj.Data as MINE_GUILD_CURRENTSTATUS_INFO;
		if (mINE_GUILD_CURRENTSTATUS_INFO != null)
		{
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64MineID <= 0L)
			{
				return;
			}
			string message = string.Empty;
			if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
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
			byte nMode;
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64DefenceGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				nMode = 2;
			}
			else
			{
				nMode = 3;
			}
			GS_MINE_SEARCH_REQ gS_MINE_SEARCH_REQ = new GS_MINE_SEARCH_REQ();
			gS_MINE_SEARCH_REQ.bySearchMineGrade = 0;
			gS_MINE_SEARCH_REQ.m_nMineID = mINE_GUILD_CURRENTSTATUS_INFO.i64MineID;
			gS_MINE_SEARCH_REQ.m_nGuildID = 0L;
			gS_MINE_SEARCH_REQ.m_nMode = nMode;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_SEARCH_REQ, gS_MINE_SEARCH_REQ);
		}
	}

	public void OnClickMilitaryBatch(IUIObject obj)
	{
		string text = string.Empty;
		string message = string.Empty;
		MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = obj.Data as MINE_GUILD_CURRENTSTATUS_INFO;
		if (mINE_GUILD_CURRENTSTATUS_INFO != null)
		{
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64MineID <= 0L)
			{
				return;
			}
			if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
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
			MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(mINE_GUILD_CURRENTSTATUS_INFO.i16MineDataID);
			if (mineCreateDataFromID == null)
			{
				return;
			}
			MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(BASE_MINE_DATA.ParseGradeFromString(mineCreateDataFromID.MINE_GRADE));
			if (kMyCharInfo.GetLevel() < (int)mineDataFromGrade.POSSIBLELEVEL)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("408");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref message, new object[]
				{
					text,
					"count",
					mineDataFromGrade.POSSIBLELEVEL
				});
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			byte nMode = 0;
			long nGuildID = 0L;
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64DefenceGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				nMode = 2;
				nGuildID = mINE_GUILD_CURRENTSTATUS_INFO.i64DefenceGuildID;
			}
			else if (mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				nMode = 4;
				nGuildID = mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID;
			}
			GS_MINE_SEARCH_REQ gS_MINE_SEARCH_REQ = new GS_MINE_SEARCH_REQ();
			gS_MINE_SEARCH_REQ.bySearchMineGrade = 0;
			gS_MINE_SEARCH_REQ.m_nMineID = mINE_GUILD_CURRENTSTATUS_INFO.i64MineID;
			gS_MINE_SEARCH_REQ.m_nGuildID = nGuildID;
			gS_MINE_SEARCH_REQ.m_nMode = nMode;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_SEARCH_REQ, gS_MINE_SEARCH_REQ);
		}
	}

	public void OnClickMilitaryReturn(IUIObject obj)
	{
		MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = obj.Data as MINE_GUILD_CURRENTSTATUS_INFO;
		if (mINE_GUILD_CURRENTSTATUS_INFO == null)
		{
			return;
		}
		if (mINE_GUILD_CURRENTSTATUS_INFO.i64MineID <= 0L)
		{
			return;
		}
		long num = 0L;
		MINE_CONSTANT_Manager instance = MINE_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			num = (long)instance.GetValue(eMINE_CONSTANT.eMINE_CONSTANT_MINE_RETURN_TIME);
		}
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
		string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("161");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromMessageBox2,
			"count",
			num.ToString()
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnStartBackMove), mINE_GUILD_CURRENTSTATUS_INFO, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
	}

	private void OnStartBackMove(object a_oObject)
	{
		MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = a_oObject as MINE_GUILD_CURRENTSTATUS_INFO;
		if (mINE_GUILD_CURRENTSTATUS_INFO != null)
		{
			GS_MINE_MILITARY_BACKMOVE_REQ gS_MINE_MILITARY_BACKMOVE_REQ = new GS_MINE_MILITARY_BACKMOVE_REQ();
			gS_MINE_MILITARY_BACKMOVE_REQ.m_nMineID = mINE_GUILD_CURRENTSTATUS_INFO.i64MineID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_MILITARY_BACKMOVE_REQ, gS_MINE_MILITARY_BACKMOVE_REQ);
		}
		this.RefreshList();
	}

	public void OnClickPagePrev(IUIObject obj)
	{
		if (this.m_Page <= 1)
		{
			return;
		}
		if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			return;
		}
		GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ = new GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ();
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.i64GuildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.i32Page = --this.m_Page;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ, gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ);
	}

	public void OnClickPageNext(IUIObject obj)
	{
		if (this.m_Page >= this.m_MaxPage)
		{
			return;
		}
		if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			return;
		}
		GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ = new GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ();
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.i64GuildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.i32Page = ++this.m_Page;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ, gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ);
	}

	public void OnClickSearchMine(IUIObject obj)
	{
		MineSearchDlg mineSearchDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_SEARCH_DLG) as MineSearchDlg;
		if (mineSearchDlg != null)
		{
			mineSearchDlg.Show();
		}
	}

	public void OnClickRefresh(IUIObject obj)
	{
		this.RefreshList();
	}

	private void ReqWebGuildImageCallback(Texture2D txtr, object _param)
	{
		DrawTexture drawTexture = _param as DrawTexture;
		if (drawTexture == null)
		{
			return;
		}
		if (txtr == null)
		{
			drawTexture.SetTexture(NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture());
		}
		else
		{
			drawTexture.SetTexture(txtr);
		}
	}
}
