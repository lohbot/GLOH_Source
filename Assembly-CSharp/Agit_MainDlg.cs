using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Agit_MainDlg : Form
{
	public enum AGIT_SHOWTYPE
	{
		AGIT_INFO,
		AGIT_TRADER,
		AGIT_TAMING_MONSTER
	}

	public const float DELAY_TIME = 1f;

	private Toolbar m_tbTab;

	private DrawTexture m_dtExp;

	private Button m_btLevelUp;

	private Button m_btNPCInvite;

	private Button m_btMonster;

	private Button m_btGoNewGuildMemberDlg;

	private Button m_btDonation;

	private NewListBox m_nlbNPCList;

	private NewListBox m_nlbGoldRecord;

	private NewListBox m_nlbInviteNPC;

	private Label m_lbMonsterInfo;

	private Label m_lbGuildFund;

	private Label m_lbExp1;

	private Label m_lbLevel;

	private Label m_lbNPCInfo;

	private Label m_lbMyGold;

	private DropDownList m_dlNPCLevel;

	private float m_fDelayTime;

	private string m_strText = string.Empty;

	private string m_strTime = string.Empty;

	private float m_fExpWidth;

	private string m_strInfo = string.Empty;

	private short m_i16NPCLevel = 1;

	private GameObject m_goLevelUp;

	private Agit_MainDlg.AGIT_SHOWTYPE m_ShowType;

	private List<NEWGUILD_FUND_USE_INFO> m_FundUseInfo = new List<NEWGUILD_FUND_USE_INFO>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/Agit/DLG_Agit_Main", G_ID.AGIT_MAIN_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_tbTab = (base.GetControl("ToolBar_Agitmenu") as Toolbar);
		this.m_tbTab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2679");
		this.m_tbTab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2681");
		this.m_tbTab.Control_Tab[2].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2682");
		UIPanelTab expr_86 = this.m_tbTab.Control_Tab[0];
		expr_86.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_86.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_B4 = this.m_tbTab.Control_Tab[1];
		expr_B4.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_B4.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_E2 = this.m_tbTab.Control_Tab[2];
		expr_E2.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_E2.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_tbTab.Control_Tab[2].controlIsEnabled = false;
		this.m_lbLevel = (base.GetControl("LB_Level2") as Label);
		this.m_dtExp = (base.GetControl("DT_exp") as DrawTexture);
		this.m_fExpWidth = this.m_dtExp.width;
		this.m_lbExp1 = (base.GetControl("LB_EXPText1") as Label);
		this.m_btLevelUp = (base.GetControl("BT_levelup") as Button);
		this.m_btLevelUp.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickLevelUp));
		this.m_lbGuildFund = (base.GetControl("LB_guildmoney") as Label);
		this.m_btDonation = (base.GetControl("BT_donate") as Button);
		this.m_btDonation.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDonation));
		this.m_nlbNPCList = (base.GetControl("NLB_NPClist") as NewListBox);
		this.m_lbNPCInfo = (base.GetControl("LB_NPCinfo") as Label);
		this.m_btNPCInvite = (base.GetControl("BT_NPC") as Button);
		this.m_btNPCInvite.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNPCInvite));
		this.m_lbMonsterInfo = (base.GetControl("LB_MONinfo") as Label);
		this.m_btMonster = (base.GetControl("BT_MON") as Button);
		this.m_btMonster.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMonster));
		this.m_btGoNewGuildMemberDlg = (base.GetControl("BT_back") as Button);
		this.m_btGoNewGuildMemberDlg.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGoNewGuildMemberDlg));
		this.m_lbMyGold = (base.GetControl("LB_mymoney") as Label);
		this.m_nlbGoldRecord = (base.GetControl("NLB_goldrecord") as NewListBox);
		this.m_nlbInviteNPC = (base.GetControl("NLB_NPCinvite") as NewListBox);
		this.m_dlNPCLevel = (base.GetControl("DDL_DDL1") as DropDownList);
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(0);
		this.SelectTab();
		this.RefreshInfo();
		this.LoadEffect();
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_ShowType = (Agit_MainDlg.AGIT_SHOWTYPE)uIPanelTab.panel.index;
		this.SelectTab();
	}

	public void SelectTab()
	{
		base.SetShowLayer(1, this.m_ShowType == Agit_MainDlg.AGIT_SHOWTYPE.AGIT_INFO);
		base.SetShowLayer(2, this.m_ShowType == Agit_MainDlg.AGIT_SHOWTYPE.AGIT_TRADER);
		base.SetShowLayer(3, this.m_ShowType == Agit_MainDlg.AGIT_SHOWTYPE.AGIT_TAMING_MONSTER);
		this.m_dlNPCLevel.SetVisible(false);
		Agit_MainDlg.AGIT_SHOWTYPE showType = this.m_ShowType;
		if (showType != Agit_MainDlg.AGIT_SHOWTYPE.AGIT_INFO)
		{
			if (showType == Agit_MainDlg.AGIT_SHOWTYPE.AGIT_TRADER)
			{
				this.RefreshInfoInviteNPC();
			}
		}
		else
		{
			this.ClickFundUseHistory(null);
		}
	}

	public void ClickLevelUp(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AGIT_LEVELUP_DLG);
	}

	public void ClickDonation(IUIObject obj)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		long num = 0L;
		if (NrTSingleton<NewGuildManager>.Instance.GetFundExchangeRate() > 0)
		{
			num = (long)(NrTSingleton<NewGuildManager>.Instance.GetFundDonation() / NrTSingleton<NewGuildManager>.Instance.GetFundExchangeRate());
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("232"),
			"count1",
			ANNUALIZED.Convert(NrTSingleton<NewGuildManager>.Instance.GetFundDonation()),
			"count2",
			num
		});
		msgBoxUI.SetMsg(new YesDelegate(this.MsgOKDonation), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("231"), this.m_strText, eMsgType.MB_OK_CANCEL, 2);
	}

	public void ClickFundUseHistory(IUIObject obj)
	{
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_FUND_USE_HISTORY_GET_REQ();
		this.SetControlEnable(false);
	}

	public void SetControlEnable(bool bEnable)
	{
		if (!bEnable)
		{
			this.m_fDelayTime = Time.time + 1f;
		}
		this.m_btDonation.controlIsEnabled = bEnable;
	}

	public override void Update()
	{
		if (this.m_fDelayTime > 0f && this.m_fDelayTime < Time.time)
		{
			this.m_fDelayTime = 0f;
			this.SetControlEnable(true);
		}
		this.UpdateTime();
	}

	public void RefreshInfo()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		this.m_lbMyGold.SetText(ANNUALIZED.Convert(kMyCharInfo.m_Money));
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1302"),
			"count",
			NrTSingleton<NewGuildManager>.Instance.GetAgitLevel()
		});
		this.m_lbLevel.SetText(this.m_strText);
		short agitLevel = NrTSingleton<NewGuildManager>.Instance.GetAgitLevel();
		AgitInfoData agitData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitData(agitLevel.ToString());
		if (agitData != null)
		{
			long num = NrTSingleton<NewGuildManager>.Instance.GetAgitExp();
			float num2 = (float)num / (float)agitData.i64Exp;
			if (num2 >= 1f)
			{
				num2 = 1f;
				num = agitData.i64Exp;
				this.m_btLevelUp.controlIsEnabled = true;
				if (this.m_goLevelUp != null)
				{
					this.m_goLevelUp.SetActive(true);
				}
			}
			else
			{
				this.m_btLevelUp.controlIsEnabled = false;
				if (this.m_goLevelUp != null)
				{
					this.m_goLevelUp.SetActive(false);
				}
			}
			this.m_dtExp.SetSize(this.m_fExpWidth * num2, this.m_dtExp.GetSize().y);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("603"),
				"count1",
				num,
				"count2",
				agitData.i64Exp
			});
			NEWGUILD_CONSTANT data = NewGuildConstant_Manager.GetInstance().GetData(eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_AGIT_MAX_LEVEL);
			if ((long)agitLevel >= data.i64Value)
			{
				this.m_lbExp1.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286"));
			}
			else
			{
				this.m_lbExp1.SetText(this.m_strText);
			}
		}
		else
		{
			this.m_dtExp.width = 0f;
			this.m_lbExp1.SetText(string.Empty);
			this.m_btLevelUp.controlIsEnabled = false;
			if (this.m_goLevelUp != null)
			{
				this.m_goLevelUp.SetActive(false);
			}
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3258"),
			"guildgold",
			ANNUALIZED.Convert(NrTSingleton<NewGuildManager>.Instance.GetFund())
		});
		this.m_lbGuildFund.SetText(this.m_strText);
		int agitNPCSubDataCount = NrTSingleton<NewGuildManager>.Instance.GetAgitNPCSubDataCount();
		this.m_nlbNPCList.Clear();
		for (int i = 0; i < agitNPCSubDataCount; i++)
		{
			AGIT_NPC_SUB_DATA agitNPCSubData = NrTSingleton<NewGuildManager>.Instance.GetAgitNPCSubData(i);
			if (agitNPCSubData != null)
			{
				this.MakeNPCItem(agitNPCSubData);
			}
		}
		this.m_nlbNPCList.RepositionItems();
		if (agitData != null)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2688"),
				"count1",
				agitNPCSubDataCount,
				"count2",
				agitData.i8NPCNum
			});
			this.m_lbNPCInfo.SetText(this.m_strText);
		}
		else
		{
			this.m_lbNPCInfo.SetText(string.Empty);
		}
		this.m_lbMonsterInfo.SetText(string.Empty);
	}

	public void MsgOKDonation(object a_oObject)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money < (long)NrTSingleton<NewGuildManager>.Instance.GetFundDonation())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_DONATION_FUND_REQ();
		this.SetControlEnable(false);
	}

	public void ClickMonster(IUIObject obj)
	{
	}

	public void MakeNPCItem(AGIT_NPC_SUB_DATA NPCSubData)
	{
		if (NPCSubData == null)
		{
			return;
		}
		AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(NPCSubData.ui8NPCType.ToString());
		if (agitNPCData == null)
		{
			return;
		}
		NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(agitNPCData.strCharCode);
		if (charKindInfoFromCode == null)
		{
			return;
		}
		NkListSolInfo nkListSolInfo = new NkListSolInfo();
		nkListSolInfo.SolCharKind = charKindInfoFromCode.GetCharKind();
		nkListSolInfo.SolGrade = -1;
		nkListSolInfo.SolLevel = NPCSubData.i16NPCLevel;
		NewListItem newListItem = new NewListItem(this.m_nlbNPCList.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(1, nkListSolInfo, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1435"),
			"charname",
			charKindInfoFromCode.GetName()
		});
		newListItem.SetListItemData(2, this.m_strText, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1031"),
			"count",
			NPCSubData.i16NPCLevel
		});
		newListItem.SetListItemData(3, this.m_strText, null, null, null);
		NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID);
		if (memberInfoFromPersonID == null)
		{
			return;
		}
		if (memberInfoFromPersonID.GetRank() >= NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_OFFICER)
		{
			newListItem.SetListItemData(4, string.Empty, NPCSubData, new EZValueChangedDelegate(this.ClickAgieDelNPC), null);
		}
		else
		{
			newListItem.SetListItemData(4, false);
		}
		long i64Time = NPCSubData.i64NPCEndTime - PublicMethod.GetCurTime();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strTime, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1763"),
			"timestring",
			this.GetTimeToString(i64Time)
		});
		newListItem.SetListItemData(5, this.m_strTime, null, null, null);
		newListItem.Data = NPCSubData;
		this.m_nlbNPCList.Add(newListItem);
	}

	public void ClickAgieDelNPC(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		if (obj.Data == null)
		{
			return;
		}
		AGIT_NPC_SUB_DATA aGIT_NPC_SUB_DATA = obj.Data as AGIT_NPC_SUB_DATA;
		if (aGIT_NPC_SUB_DATA == null)
		{
			return;
		}
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
		AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(aGIT_NPC_SUB_DATA.ui8NPCType.ToString());
		if (agitNPCData == null)
		{
			return;
		}
		NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(agitNPCData.strCharCode);
		if (charKindInfoFromCode == null)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("255"),
			"charname",
			charKindInfoFromCode.GetName()
		});
		msgBoxUI.SetMsg(new YesDelegate(this.MsgOKAgitDelNPC), aGIT_NPC_SUB_DATA.ui8NPCType, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("254"), this.m_strText, eMsgType.MB_OK_CANCEL, 2);
	}

	public string GetTimeToString(long i64Time)
	{
		this.m_strText = string.Empty;
		if (i64Time > 0L)
		{
			long totalHourFromSec = PublicMethod.GetTotalHourFromSec(i64Time);
			long minuteFromSec = PublicMethod.GetMinuteFromSec(i64Time);
			long num = i64Time % 60L;
			this.m_strText = string.Format("{0}:{1}:{2}", totalHourFromSec.ToString("00"), minuteFromSec.ToString("00"), num.ToString("00"));
		}
		return this.m_strText;
	}

	public void UpdateTime()
	{
		for (int i = 0; i < this.m_nlbNPCList.Count; i++)
		{
			UIListItemContainer item = this.m_nlbNPCList.GetItem(i);
			if (!(item == null))
			{
				if (item.Data != null)
				{
					AGIT_NPC_SUB_DATA aGIT_NPC_SUB_DATA = item.Data as AGIT_NPC_SUB_DATA;
					if (aGIT_NPC_SUB_DATA != null)
					{
						Label label = item.GetElement(5) as Label;
						if (label != null)
						{
							long i64Time = aGIT_NPC_SUB_DATA.i64NPCEndTime - PublicMethod.GetCurTime();
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strTime, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1763"),
								"timestring",
								this.GetTimeToString(i64Time)
							});
							label.SetText(this.m_strTime);
						}
					}
				}
			}
		}
	}

	public void MsgOKAgitDelNPC(object a_oObject)
	{
		byte ui8NPCType = (byte)a_oObject;
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_AGIT_DEL_NPC_REQ(ui8NPCType);
	}

	public void ClickGoNewGuildMemberDlg(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_MEMBER_DLG);
		this.Close();
	}

	public void LoadEffect()
	{
		NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_ANCIENTTREASURE_UI", this.m_btLevelUp, this.m_btLevelUp.GetSize());
		this.m_btLevelUp.AddGameObjectDelegate(new EZGameObjectDelegate(this.LoadEffectDelegate));
	}

	public void LoadEffectDelegate(IUIObject control, GameObject obj)
	{
		this.m_goLevelUp = obj;
		this.m_goLevelUp.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
		this.RefreshInfo();
	}

	public override void Close()
	{
		base.Close();
		if (this.m_goLevelUp != null)
		{
			UnityEngine.Object.Destroy(this.m_goLevelUp);
		}
	}

	public void ClearFundUseInfo()
	{
		this.m_FundUseInfo.Clear();
	}

	public void AddFundUseInfo(NEWGUILD_FUND_USE_INFO Data)
	{
		this.m_FundUseInfo.Add(Data);
	}

	public void RefreshInfoGold()
	{
		this.m_nlbGoldRecord.Clear();
		for (int i = 0; i < this.m_FundUseInfo.Count; i++)
		{
			this.MakeFundUseInfoItem(this.m_FundUseInfo[i]);
		}
		this.m_nlbGoldRecord.RepositionItems();
	}

	public void MakeFundUseInfoItem(NEWGUILD_FUND_USE_INFO Data)
	{
		if (Data == null)
		{
			return;
		}
		NewListItem newListItem = new NewListItem(this.m_nlbGoldRecord.ColumnNum, true, string.Empty);
		DateTime dueDate = PublicMethod.GetDueDate(Data.i64UseTime);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("602"),
			"year",
			dueDate.Year,
			"month",
			dueDate.Month,
			"day",
			dueDate.Day
		});
		newListItem.SetListItemData(0, this.m_strText, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1527"),
			"hour",
			dueDate.Hour,
			"min",
			dueDate.Minute,
			"sec",
			dueDate.Second
		});
		newListItem.SetListItemData(1, this.m_strText, null, null, null);
		newListItem.SetListItemData(2, NrTSingleton<NewGuildManager>.Instance.GetStringFromFundUseType((NewGuildDefine.eNEWGUILD_FUND_USE_TYPE)Data.ui8UseType), null, null, null);
		newListItem.SetListItemData(3, ANNUALIZED.Convert(Data.i64UseFund), null, null, null);
		newListItem.SetListItemData(4, TKString.NEWString(Data.szUseCharName), null, null, null);
		this.m_nlbGoldRecord.Add(newListItem);
	}

	public void ClickNPCInvite(IUIObject obj)
	{
		UIListItemContainer selectItem = this.m_nlbInviteNPC.GetSelectItem();
		if (selectItem == null)
		{
			return;
		}
		if (selectItem.Data == null)
		{
			return;
		}
		AgitNPCData agitNPCData = (AgitNPCData)selectItem.Data;
		if (agitNPCData == null)
		{
			return;
		}
		AgitInfoData agitData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitData(this.m_i16NPCLevel.ToString());
		if (agitData == null)
		{
			return;
		}
		AgitInfoData agitData2 = NrTSingleton<NrBaseTableManager>.Instance.GetAgitData(NrTSingleton<NewGuildManager>.Instance.GetAgitLevel().ToString());
		if (agitData2 == null)
		{
			return;
		}
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
		if (NrTSingleton<NewGuildManager>.Instance.GetFund() < (long)agitData.i32NPCCost)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("754"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (NrTSingleton<NewGuildManager>.Instance.GetAgitNPCSubDataCount() >= (int)agitData2.i8NPCNum)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("770"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		AGIT_NPC_SUB_DATA agitNPCSubDataFromNPCType = NrTSingleton<NewGuildManager>.Instance.GetAgitNPCSubDataFromNPCType(agitNPCData.ui8NPCType);
		if (agitNPCSubDataFromNPCType != null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("771"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(agitNPCData.strCharCode);
		if (charKindInfoFromCode == null)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("257"),
			"count",
			agitData.i32NPCCost,
			"charname",
			charKindInfoFromCode.GetName(),
			"level",
			this.m_i16NPCLevel
		});
		msgBoxUI.SetMsg(new YesDelegate(this.MsgOKNPCInvite), agitNPCData, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("256"), this.m_strText, eMsgType.MB_OK_CANCEL, 2);
	}

	public void MsgOKNPCInvite(object a_oObject)
	{
		AgitNPCData agitNPCData = (AgitNPCData)a_oObject;
		if (agitNPCData == null)
		{
			return;
		}
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_AGIT_ADD_NPC_REQ(agitNPCData.ui8NPCType, this.m_i16NPCLevel);
	}

	public void RefreshInfoInviteNPC()
	{
		this.m_dlNPCLevel.SetVisible(true);
		this.m_i16NPCLevel = NrTSingleton<NewGuildManager>.Instance.GetAgitLevel();
		this.m_dlNPCLevel.SetViewArea((int)this.m_i16NPCLevel);
		if (this.m_dlNPCLevel.Count > 0)
		{
			this.m_dlNPCLevel.Clear();
		}
		for (int i = 0; i < (int)this.m_i16NPCLevel; i++)
		{
			short num = (short)(i + 1);
			this.m_strText = string.Format("{0}", num);
			this.m_dlNPCLevel.AddItem(this.m_strText, num);
		}
		this.m_dlNPCLevel.RepositionItems();
		if (this.m_dlNPCLevel.Count <= 1)
		{
			this.m_dlNPCLevel.SetFirstItem();
		}
		else
		{
			this.m_dlNPCLevel.SetIndex((int)(this.m_i16NPCLevel - 1));
		}
		this.m_dlNPCLevel.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeNPCLevel));
		this.m_nlbInviteNPC.Clear();
		for (int i = 2; i < 6; i++)
		{
			AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(i.ToString());
			if (agitNPCData != null)
			{
				if (agitNPCData.ui8NPCType != 3)
				{
					if (agitNPCData.ui8NPCType != 4)
					{
						this.MakeNPCInfo(agitNPCData);
					}
				}
			}
		}
		this.m_nlbInviteNPC.RepositionItems();
	}

	public void MakeNPCInfo(AgitNPCData Data)
	{
		short agitLevel = NrTSingleton<NewGuildManager>.Instance.GetAgitLevel();
		AgitInfoData agitData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitData(agitLevel.ToString());
		if (agitData == null)
		{
			return;
		}
		NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(Data.strCharCode);
		if (charKindInfoFromCode == null)
		{
			return;
		}
		NkListSolInfo nkListSolInfo = new NkListSolInfo();
		nkListSolInfo.SolCharKind = charKindInfoFromCode.GetCharKind();
		nkListSolInfo.SolGrade = -1;
		nkListSolInfo.SolLevel = NrTSingleton<NewGuildManager>.Instance.GetAgitLevel();
		NewListItem newListItem = new NewListItem(this.m_nlbInviteNPC.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(0, ANNUALIZED.Convert(agitData.i32NPCCost), null, null, null);
		newListItem.SetListItemData(1, true);
		newListItem.SetListItemData(2, nkListSolInfo, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1435"),
			"charname",
			charKindInfoFromCode.GetName()
		});
		newListItem.SetListItemData(3, this.m_strText, null, null, null);
		this.m_strText = string.Empty;
		int num = Data.i32LevelRate[(int)(agitLevel - 1)];
		switch (Data.ui8NPCType)
		{
		case 2:
			this.m_strInfo = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2747");
			num /= 100;
			break;
		case 3:
			this.m_strInfo = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2751");
			num /= 100;
			break;
		case 4:
			this.m_strInfo = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2750");
			num /= 100;
			break;
		case 5:
			this.m_strInfo = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2794");
			break;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			this.m_strInfo,
			"count",
			num
		});
		newListItem.SetListItemData(4, this.m_strText, null, null, null);
		newListItem.Data = Data;
		this.m_nlbInviteNPC.Add(newListItem);
	}

	public void OnChangeNPCLevel(IUIObject obj)
	{
		if (this.m_dlNPCLevel.Count > 0 && this.m_dlNPCLevel.SelectedItem != null)
		{
			ListItem listItem = this.m_dlNPCLevel.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				if (listItem.Key == null)
				{
					return;
				}
				this.m_i16NPCLevel = (short)listItem.Key;
			}
		}
		this.SelectNPCLevel();
	}

	public void SelectNPCLevel()
	{
		AgitInfoData agitData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitData(this.m_i16NPCLevel.ToString());
		if (agitData == null)
		{
			return;
		}
		for (int i = 0; i < this.m_nlbInviteNPC.Count; i++)
		{
			UIListItemContainer item = this.m_nlbInviteNPC.GetItem(i);
			if (!(item == null))
			{
				if (item.Data != null)
				{
					AgitNPCData agitNPCData = item.Data as AgitNPCData;
					if (agitNPCData != null)
					{
						Label label = item.GetElement(0) as Label;
						if (label != null)
						{
							label.SetText(ANNUALIZED.Convert(agitData.i32NPCCost));
						}
						Label label2 = item.GetElement(4) as Label;
						if (label2 != null)
						{
							this.m_strText = string.Empty;
							int num = agitNPCData.i32LevelRate[(int)(this.m_i16NPCLevel - 1)];
							switch (agitNPCData.ui8NPCType)
							{
							case 2:
								this.m_strInfo = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2747");
								num /= 100;
								break;
							case 3:
								this.m_strInfo = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2751");
								num /= 100;
								break;
							case 4:
								this.m_strInfo = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2750");
								num /= 100;
								break;
							case 5:
								this.m_strInfo = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2794");
								break;
							}
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
							{
								this.m_strInfo,
								"count",
								num
							});
							label2.SetText(this.m_strText);
						}
					}
				}
			}
		}
	}
}
