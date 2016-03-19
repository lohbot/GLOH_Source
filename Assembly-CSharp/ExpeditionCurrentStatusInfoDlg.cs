using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class ExpeditionCurrentStatusInfoDlg : Form
{
	private const int ONE_PAGE_COUNT = 4;

	private List<EXPEDITION_CURRENT_STATE_INFO> m_listExpedition_CurrentStatus = new List<EXPEDITION_CURRENT_STATE_INFO>();

	private NewListBox m_lbCurrentStatus;

	private Button m_btExpeditionSearch;

	private Box m_bxPage;

	private Button m_btPagePrev;

	private Button m_btPageNext;

	private Button m_btRefresh;

	private Label m_lCurrentCount;

	private int m_Page = 1;

	private int m_MaxPage = 1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Expedition/dlg_Expeditioncondition", G_ID.EXPEDITION_CURRENTSTATUSINFO_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_lbCurrentStatus = (base.GetControl("NLB_Expeditioncondition") as NewListBox);
		this.m_lbCurrentStatus.Reserve = false;
		this.m_btExpeditionSearch = (base.GetControl("BT_MineScarch") as Button);
		Button expr_3E = this.m_btExpeditionSearch;
		expr_3E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3E.Click, new EZValueChangedDelegate(this.OnClickSearchExpedition));
		this.m_bxPage = (base.GetControl("Box_Page") as Box);
		this.m_lCurrentCount = (base.GetControl("Label_Label18") as Label);
		this.m_btPagePrev = (base.GetControl("BT_Page01") as Button);
		Button expr_A7 = this.m_btPagePrev;
		expr_A7.Click = (EZValueChangedDelegate)Delegate.Combine(expr_A7.Click, new EZValueChangedDelegate(this.OnClickPagePrev));
		this.m_btPageNext = (base.GetControl("BT_Page02") as Button);
		Button expr_E4 = this.m_btPageNext;
		expr_E4.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E4.Click, new EZValueChangedDelegate(this.OnClickPageNext));
		this.m_btRefresh = (base.GetControl("BT_Refresh02") as Button);
		Button expr_121 = this.m_btRefresh;
		expr_121.Click = (EZValueChangedDelegate)Delegate.Combine(expr_121.Click, new EZValueChangedDelegate(this.OnClickRefresh));
		base.SetScreenCenter();
	}

	public override void Show()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		string text = string.Empty;
		string empty = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2416");
		EXPEDITION_CONSTANT_MANAGER instance = EXPEDITION_CONSTANT_MANAGER.GetInstance();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"count1",
			kMyCharInfo.GetCharDetail(10),
			"count2",
			instance.GetValue(eEXPEDITION_CONSTANT.eEXPEDITION_DAY_COUNT)
		});
		this.m_lCurrentCount.SetText(empty);
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

	public void AddInfo(EXPEDITION_CURRENT_STATE_INFO[] info)
	{
		this.m_lbCurrentStatus.Clear();
		this.m_listExpedition_CurrentStatus.Clear();
		for (int i = 0; i < info.Length; i++)
		{
			this.m_listExpedition_CurrentStatus.Add(info[i]);
		}
		this.Show();
	}

	public void SetList()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		this.m_lbCurrentStatus.Clear();
		string text = string.Empty;
		string empty = string.Empty;
		int num = 0;
		foreach (EXPEDITION_CURRENT_STATE_INFO current in this.m_listExpedition_CurrentStatus)
		{
			NewListItem newListItem = new NewListItem(this.m_lbCurrentStatus.ColumnNum, true);
			if ((current.ui8ExpeditionState == 1 || current.ui8ExpeditionState == 3) && current.i64Time == current.i64CheckBattleTime)
			{
				newListItem.SetListItemData(0, true);
				newListItem.SetListItemData(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615"), current, new EZValueChangedDelegate(this.OnClickBackMove), null);
			}
			else
			{
				newListItem.SetListItemData(0, false);
			}
			newListItem.SetListItemData(1, this.GetExpeditionState(current), null, null, null);
			if (current.ui8ExpeditionState == 1 || current.ui8ExpeditionState == 2 || current.ui8ExpeditionState == 4)
			{
				NkExpeditionMilitaryInfo validExpeditionMilitaryInfo = kMyCharInfo.GetMilitaryList().GetValidExpeditionMilitaryInfo(current.ui8ExpeditionMilitaryUniq);
				if (validExpeditionMilitaryInfo != null)
				{
					NkSoldierInfo leaderSolInfo = validExpeditionMilitaryInfo.GetLeaderSolInfo();
					if (leaderSolInfo != null)
					{
						EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(leaderSolInfo.GetCharKind(), leaderSolInfo.GetGrade());
						if (eventHeroCharCode != null)
						{
							newListItem.SetListItemData(5, "Win_I_EventSol", null, null, null);
						}
						else
						{
							UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(leaderSolInfo.GetCharKind(), (int)leaderSolInfo.GetGrade());
							if (legendFrame != null)
							{
								newListItem.SetListItemData(5, legendFrame, null, null, null);
							}
							else
							{
								newListItem.SetListItemData(5, "Win_I_Cancel", null, null, null);
							}
						}
						newListItem.SetListItemData(7, leaderSolInfo.GetListSolInfo(false), null, null, null);
						newListItem.SetListItemData(3, string.Empty, current, new EZValueChangedDelegate(this.OnClickDetailInfo), null);
					}
					newListItem.SetListItemData(9, leaderSolInfo.GetCharKindInfo().GetName(), null, null, null);
				}
				EXPEDITION_CREATE_DATA expedtionCreateData = BASE_EXPEDITION_CREATE_DATA.GetExpedtionCreateData(current.i16ExpeditionCreateDataID);
				if (expedtionCreateData != null)
				{
					EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(expedtionCreateData.EXPEDITION_GRADE);
					if (expeditionDataFromGrade != null)
					{
						newListItem.SetListItemData(8, false);
						UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(expeditionDataFromGrade.Expedition_ICON_NAME);
						if (uIBaseInfoLoader != null)
						{
							newListItem.SetListItemData(4, uIBaseInfoLoader, current, new EZValueChangedDelegate(this.OnClickMonDetailInfo), null);
						}
						newListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(expeditionDataFromGrade.Expedition_GRADE_INTERFACEKEY), null, null, null);
					}
				}
			}
			else if (current.ui8ExpeditionState == 3)
			{
				EXPEDITION_CREATE_DATA expedtionCreateData2 = BASE_EXPEDITION_CREATE_DATA.GetExpedtionCreateData(current.i16ExpeditionCreateDataID);
				if (expedtionCreateData2 != null)
				{
					int num2 = expedtionCreateData2.EXPEDITION_ECO[0];
					ECO eco = NrTSingleton<NrBaseTableManager>.Instance.GetEco(num2.ToString());
					if (eco != null)
					{
						newListItem.SetListItemData(7, NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco.szCharCode[0]), null, null, null);
						newListItem.SetListItemData(3, NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco.szCharCode[0]), current, new EZValueChangedDelegate(this.OnClickMonDetailInfo), null);
						newListItem.SetListItemData(9, NrTSingleton<NrCharKindInfoManager>.Instance.GetName(NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco.szCharCode[0])), null, null, null);
					}
				}
				NkExpeditionMilitaryInfo validExpeditionMilitaryInfo2 = kMyCharInfo.GetMilitaryList().GetValidExpeditionMilitaryInfo(current.ui8ExpeditionMilitaryUniq);
				if (validExpeditionMilitaryInfo2 != null)
				{
					NkSoldierInfo leaderSolInfo2 = validExpeditionMilitaryInfo2.GetLeaderSolInfo();
					if (leaderSolInfo2 != null)
					{
						UIBaseInfoLoader legendFrame2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(leaderSolInfo2.GetCharKind(), (int)leaderSolInfo2.GetGrade());
						if (legendFrame2 != null)
						{
							newListItem.SetListItemData(6, legendFrame2, null, null, null);
						}
						else
						{
							newListItem.SetListItemData(6, "Win_I_Cancel", null, null, null);
						}
						newListItem.SetListItemData(8, leaderSolInfo2.GetListSolInfo(false), null, null, null);
					}
					newListItem.SetListItemData(4, string.Empty, current, new EZValueChangedDelegate(this.OnClickDetailInfo), null);
					newListItem.SetListItemData(10, leaderSolInfo2.GetCharKindInfo().GetName(), null, null, null);
				}
			}
			newListItem.Data = current;
			this.m_lbCurrentStatus.Add(newListItem);
			num++;
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1633");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"count1",
			this.m_Page,
			"count2",
			this.m_MaxPage
		});
		this.m_bxPage.SetText(empty);
		this.m_lbCurrentStatus.RepositionItems();
	}

	public void UpdateTime()
	{
		for (int i = 0; i < this.m_lbCurrentStatus.Count; i++)
		{
			IUIListObject item = this.m_lbCurrentStatus.GetItem(i);
			EXPEDITION_CURRENT_STATE_INFO eXPEDITION_CURRENT_STATE_INFO = item.Data as EXPEDITION_CURRENT_STATE_INFO;
			if (eXPEDITION_CURRENT_STATE_INFO != null)
			{
				Label label = ((UIListItemContainer)item).GetElement(1) as Label;
				if (null != label)
				{
					label.Text = this.GetExpeditionState(eXPEDITION_CURRENT_STATE_INFO);
				}
				UIButton uIButton = ((UIListItemContainer)item).GetElement(0) as UIButton;
				if (null != uIButton)
				{
					if (eXPEDITION_CURRENT_STATE_INFO.ui8ExpeditionState == 1)
					{
						uIButton.Visible = true;
					}
					else if (eXPEDITION_CURRENT_STATE_INFO.ui8ExpeditionState == 2 || eXPEDITION_CURRENT_STATE_INFO.ui8ExpeditionState == 3)
					{
						if (eXPEDITION_CURRENT_STATE_INFO.i64Time == eXPEDITION_CURRENT_STATE_INFO.i64CheckBattleTime)
						{
							uIButton.Visible = true;
						}
						else
						{
							uIButton.Visible = false;
						}
					}
					else
					{
						uIButton.Visible = false;
					}
				}
			}
		}
	}

	public void RefreshList()
	{
		GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ = new GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ();
		gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ.i32Page = this.m_Page;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ, gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ);
	}

	public string GetExpeditionState(EXPEDITION_CURRENT_STATE_INFO info)
	{
		string result = string.Empty;
		if (info.i64Time - PublicMethod.GetCurTime() <= 0L)
		{
			if (info.ui8ExpeditionState == 1)
			{
				info.ui8ExpeditionState = 2;
			}
			info.i64CheckBattleTime = 0L;
		}
		if (info.i64Time == info.i64CheckBattleTime)
		{
			result = string.Format("{0} {1}", this.GetExpeditionTextFromState(info), this.GetLeftTime(info.i64Time));
		}
		else
		{
			result = this.GetExpeditionTextFromState(info);
		}
		return result;
	}

	public string GetExpeditionTextFromState(EXPEDITION_CURRENT_STATE_INFO info)
	{
		string result = string.Empty;
		switch (info.ui8ExpeditionState)
		{
		case 1:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1525");
			break;
		case 2:
			if (info.i64Time == info.i64CheckBattleTime)
			{
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1525");
			}
			else
			{
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1623");
			}
			break;
		case 3:
			if (info.i64Time == info.i64CheckBattleTime)
			{
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2736");
			}
			else
			{
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1623");
			}
			break;
		case 4:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1526");
			break;
		}
		return result;
	}

	public string GetLeftTime(long time)
	{
		string empty = string.Empty;
		long num = time - PublicMethod.GetCurTime();
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
		}
		return empty;
	}

	public void OnClickPagePrev(IUIObject obj)
	{
		if (this.m_Page <= 1)
		{
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ = new GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ();
		gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ.i32Page = --this.m_Page;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ, gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ);
	}

	public void OnClickPageNext(IUIObject obj)
	{
		if (this.m_Page >= this.m_MaxPage)
		{
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ = new GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ();
		gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ.i32Page = ++this.m_Page;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ, gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ);
	}

	public void OnClickSearchExpedition(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.EXPEDITION_SEARCH_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.EXPEDITION_SEARCH_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EXPEDITION_SEARCH_DLG);
		}
	}

	private void OnClickDetailInfo(IUIObject obj)
	{
		EXPEDITION_CURRENT_STATE_INFO eXPEDITION_CURRENT_STATE_INFO = obj.Data as EXPEDITION_CURRENT_STATE_INFO;
		if (eXPEDITION_CURRENT_STATE_INFO != null)
		{
			if (eXPEDITION_CURRENT_STATE_INFO.i16ExpeditionCreateDataID <= 0)
			{
				return;
			}
			NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
			if (militaryList == null)
			{
				return;
			}
			NkExpeditionMilitaryInfo validExpeditionMilitaryInfo = militaryList.GetValidExpeditionMilitaryInfo(eXPEDITION_CURRENT_STATE_INFO.ui8ExpeditionMilitaryUniq);
			if (validExpeditionMilitaryInfo == null)
			{
				return;
			}
			NkExpeditionMilitaryInfo expeditionMilitaryInfo = militaryList.GetExpeditionMilitaryInfo(validExpeditionMilitaryInfo.GetMilitaryUnique());
			if (expeditionMilitaryInfo == null)
			{
				return;
			}
			NkSoldierInfo[] expeditionSolInfo = expeditionMilitaryInfo.GetExpeditionSolInfo();
			if (expeditionSolInfo == null)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < 15; i++)
			{
				if (expeditionSolInfo[i] == null)
				{
					num++;
				}
			}
			if (15 <= num)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("758");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else
			{
				GS_EXPEDITION_DETAILINFO_REQ gS_EXPEDITION_DETAILINFO_REQ = new GS_EXPEDITION_DETAILINFO_REQ();
				gS_EXPEDITION_DETAILINFO_REQ.ui8ExpeditionMilitaryUniq = 0;
				gS_EXPEDITION_DETAILINFO_REQ.i64ExpeditionID = eXPEDITION_CURRENT_STATE_INFO.i64ExpeditionID;
				gS_EXPEDITION_DETAILINFO_REQ.bUserInfo = true;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_DETAILINFO_REQ, gS_EXPEDITION_DETAILINFO_REQ);
			}
		}
	}

	private void OnClickMonDetailInfo(IUIObject obj)
	{
		EXPEDITION_CURRENT_STATE_INFO eXPEDITION_CURRENT_STATE_INFO = obj.Data as EXPEDITION_CURRENT_STATE_INFO;
		if (eXPEDITION_CURRENT_STATE_INFO != null)
		{
			if (eXPEDITION_CURRENT_STATE_INFO.i16ExpeditionCreateDataID <= 0)
			{
				return;
			}
			GS_EXPEDITION_DETAILINFO_REQ gS_EXPEDITION_DETAILINFO_REQ = new GS_EXPEDITION_DETAILINFO_REQ();
			gS_EXPEDITION_DETAILINFO_REQ.ui8ExpeditionMilitaryUniq = 0;
			gS_EXPEDITION_DETAILINFO_REQ.i64ExpeditionID = eXPEDITION_CURRENT_STATE_INFO.i64ExpeditionID;
			gS_EXPEDITION_DETAILINFO_REQ.bUserInfo = false;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_DETAILINFO_REQ, gS_EXPEDITION_DETAILINFO_REQ);
		}
	}

	public void OnClickBackMove(IUIObject obj)
	{
		EXPEDITION_CURRENT_STATE_INFO eXPEDITION_CURRENT_STATE_INFO = obj.Data as EXPEDITION_CURRENT_STATE_INFO;
		if (eXPEDITION_CURRENT_STATE_INFO != null)
		{
			if ((eXPEDITION_CURRENT_STATE_INFO.ui8ExpeditionState == 1 || eXPEDITION_CURRENT_STATE_INFO.ui8ExpeditionState == 3) && eXPEDITION_CURRENT_STATE_INFO.i64Time == eXPEDITION_CURRENT_STATE_INFO.i64CheckBattleTime)
			{
				GS_EXPEDITION_MILITARY_BACKMOVE_REQ gS_EXPEDITION_MILITARY_BACKMOVE_REQ = new GS_EXPEDITION_MILITARY_BACKMOVE_REQ();
				gS_EXPEDITION_MILITARY_BACKMOVE_REQ.i64ExpeditionID = eXPEDITION_CURRENT_STATE_INFO.i64ExpeditionID;
				gS_EXPEDITION_MILITARY_BACKMOVE_REQ.i16ExpeditionCreateID = eXPEDITION_CURRENT_STATE_INFO.i16ExpeditionCreateDataID;
				gS_EXPEDITION_MILITARY_BACKMOVE_REQ.byExpeditionMilitaryUniq = eXPEDITION_CURRENT_STATE_INFO.ui8ExpeditionMilitaryUniq;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_MILITARY_BACKMOVE_REQ, gS_EXPEDITION_MILITARY_BACKMOVE_REQ);
			}
			else if (eXPEDITION_CURRENT_STATE_INFO.ui8ExpeditionState == 2)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("319");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (eXPEDITION_CURRENT_STATE_INFO.ui8ExpeditionState == 4)
			{
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("406");
				Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
	}

	public void OnClickRefresh(IUIObject obj)
	{
		this.RefreshList();
	}
}
