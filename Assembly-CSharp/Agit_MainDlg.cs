using GAME;
using PROTOCOL;
using System;
using UnityEngine;
using UnityForms;

public class Agit_MainDlg : Form
{
	public const float DELAY_TIME = 1f;

	private Label m_lbLevel;

	private DrawTexture m_dtExp;

	private Label m_lbExp1;

	private Button m_btLevelUp;

	private Label m_lbGuildFund;

	private Button m_btDonation;

	private Button m_btFundUseHistory;

	private NewListBox m_nlbNPCList;

	private Label m_lbNPCInfo;

	private Button m_btNPCInvite;

	private NewListBox m_nlbMonsterList;

	private Label m_lbMonsterInfo;

	private Button m_btMonster;

	private Button m_btGoNewGuildMemberDlg;

	private float m_fDelayTime;

	private string m_strText = string.Empty;

	private string m_strTime = string.Empty;

	private float m_fExpWidth;

	private GameObject m_goLevelUp;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/Agit/DLG_Agit_Main", G_ID.AGIT_MAIN_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_lbLevel = (base.GetControl("LB_Level2") as Label);
		this.m_dtExp = (base.GetControl("DT_exp") as DrawTexture);
		this.m_fExpWidth = this.m_dtExp.width;
		this.m_lbExp1 = (base.GetControl("LB_EXPText1") as Label);
		this.m_btLevelUp = (base.GetControl("BT_levelup") as Button);
		this.m_btLevelUp.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickLevelUp));
		this.m_lbGuildFund = (base.GetControl("LB_guildmoney") as Label);
		this.m_btDonation = (base.GetControl("BT_donate") as Button);
		this.m_btDonation.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDonation));
		this.m_btFundUseHistory = (base.GetControl("BT_record") as Button);
		this.m_btFundUseHistory.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickFundUseHistory));
		this.m_nlbNPCList = (base.GetControl("NLB_NPClist") as NewListBox);
		this.m_lbNPCInfo = (base.GetControl("LB_NPCinfo") as Label);
		this.m_btNPCInvite = (base.GetControl("BT_NPC") as Button);
		this.m_btNPCInvite.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNPCInvite));
		this.m_nlbMonsterList = (base.GetControl("NLB_MONlist") as NewListBox);
		this.m_lbMonsterInfo = (base.GetControl("LB_MONinfo") as Label);
		this.m_btMonster = (base.GetControl("BT_MON") as Button);
		this.m_btMonster.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMonster));
		this.m_btGoNewGuildMemberDlg = (base.GetControl("BT_back") as Button);
		this.m_btGoNewGuildMemberDlg.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGoNewGuildMemberDlg));
		this.RefreshInfo();
		this.LoadEffect();
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
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
		msgBoxUI.SetMsg(new YesDelegate(this.MsgOKDonation), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("231"), this.m_strText, eMsgType.MB_OK_CANCEL);
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
		this.m_btFundUseHistory.controlIsEnabled = bEnable;
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
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1721"),
			"gold",
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

	public void ClickNPCInvite(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AGIT_NPC_INVITE_DLG);
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
		NewListItem newListItem = new NewListItem(this.m_nlbNPCList.ColumnNum, true);
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
		msgBoxUI.SetMsg(new YesDelegate(this.MsgOKAgitDelNPC), aGIT_NPC_SUB_DATA.ui8NPCType, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("254"), this.m_strText, eMsgType.MB_OK_CANCEL);
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
			UIListItemContainer uIListItemContainer = this.m_nlbNPCList.GetItem(i) as UIListItemContainer;
			if (!(uIListItemContainer == null))
			{
				if (uIListItemContainer.Data != null)
				{
					AGIT_NPC_SUB_DATA aGIT_NPC_SUB_DATA = uIListItemContainer.Data as AGIT_NPC_SUB_DATA;
					if (aGIT_NPC_SUB_DATA != null)
					{
						Label label = uIListItemContainer.GetElement(5) as Label;
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
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(0);
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
}
