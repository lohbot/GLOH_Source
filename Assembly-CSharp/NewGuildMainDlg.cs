using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class NewGuildMainDlg : Form
{
	private DrawTexture m_dtGuildFlag;

	private Button m_btBack;

	private Label m_lbGuildName;

	private Label m_lbRank;

	private Label m_lbMasterName;

	private Label m_lbSubMasterName;

	private Label m_lbMemberNum;

	private Label m_lbLevel;

	private Label m_lbExpText;

	private Label m_lbGuildCreateDate;

	private Button m_btLeaveAndApplicantAndDeclareWar;

	private Button m_btAdmin;

	private Box m_bxApplicantNum;

	private Label m_lbGuildMessage;

	private DrawTexture m_dtGuildMarkTexture;

	private DrawTexture m_dtMedal;

	private DrawTexture m_dtExp;

	private Button m_btHelp1;

	private Button m_btHelp2;

	private Label m_lbHelp2;

	private Button m_btHelp3;

	private Label m_lbFund;

	private NewListBox m_DeclareWarList;

	private string m_strText = string.Empty;

	private long m_lGuildID;

	private string m_strGuildName = string.Empty;

	private float m_fExpWidth;

	private bool m_bHelp1;

	private bool m_bHelp2;

	private bool m_bHelp3;

	private bool m_bIsDeclareWarTarget;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_NewGuild_Main", G_ID.NEWGUILD_MAIN_DLG, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_dtGuildFlag = (base.GetControl("DrawTexture_Guild_Flag") as DrawTexture);
		this.m_btBack = (base.GetControl("Button_Back") as Button);
		this.m_btBack.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickBack));
		this.m_lbGuildName = (base.GetControl("Label_GuildName") as Label);
		this.m_lbRank = (base.GetControl("Label_GuildRanking") as Label);
		this.m_lbMasterName = (base.GetControl("Label_GuildMasterName") as Label);
		this.m_lbSubMasterName = (base.GetControl("Label_SubGuildMasterName") as Label);
		this.m_lbMemberNum = (base.GetControl("Label_GuildMemberCount") as Label);
		this.m_lbLevel = (base.GetControl("LB_LevelText") as Label);
		this.m_lbExpText = (base.GetControl("LB_ExpText") as Label);
		this.m_lbGuildCreateDate = (base.GetControl("Label_GuildBirthCount") as Label);
		this.m_lbGuildCreateDate.SetText(string.Empty);
		this.m_btLeaveAndApplicantAndDeclareWar = (base.GetControl("Button_GuildInvite") as Button);
		this.m_btLeaveAndApplicantAndDeclareWar.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickleaveApplicantAndDeclareWar));
		this.m_btAdmin = (base.GetControl("Button_AdminMenu") as Button);
		this.m_btAdmin.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAdmin));
		this.m_bxApplicantNum = (base.GetControl("Box_AdminNotice") as Box);
		this.m_lbGuildMessage = (base.GetControl("Label_GuildGreeting") as Label);
		this.m_dtGuildMarkTexture = (base.GetControl("DrawTexture_GMark") as DrawTexture);
		this.m_dtMedal = (base.GetControl("DrawTexture_Medal") as DrawTexture);
		this.m_dtMedal.Hide(true);
		this.m_dtExp = (base.GetControl("DrawTexture_ExpBar") as DrawTexture);
		this.m_fExpWidth = this.m_dtExp.GetSize().x;
		this.m_btHelp1 = (base.GetControl("Button_Help01") as Button);
		this.m_btHelp1.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp1));
		this.m_btHelp2 = (base.GetControl("Button_Help02") as Button);
		this.m_btHelp2.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp2));
		this.m_lbHelp2 = (base.GetControl("Label_HelpText02") as Label);
		this.m_btHelp3 = (base.GetControl("Button_Help03") as Button);
		this.m_btHelp3.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp3));
		this.m_lbFund = (base.GetControl("Label_GuildMoneyCount") as Label);
		this.m_DeclareWarList = (base.GetControl("nlb_DeclareWar") as NewListBox);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1859"),
			"count",
			NrTSingleton<NewGuildManager>.Instance.GetNewMasterCheckDay()
		});
		this.m_lbHelp2.SetText(this.m_strText);
		this.m_lbGuildName.transform.localPosition = new Vector3(this.m_lbGuildName.transform.localPosition.x, this.m_lbGuildName.transform.localPosition.y, 0.08f);
		base.AllHideLayer();
		this.SetBundleDownload();
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("guilddlg_open");
	}

	public override void OnLoad()
	{
		NrSound.ImmedatePlay("UI_SFX", "GUILD", "OPEN");
	}

	public override void OnClose()
	{
		base.OnClose();
		NrSound.ImmedatePlay("UI_SFX", "GUILD", "CLOSE");
	}

	public void SetDetailInfo(GS_NEWGUILD_DETAILINFO_ACK ACK)
	{
		this.m_lGuildID = ACK.i64GuildID;
		this.m_strGuildName = TKString.NEWString(ACK.strGuildName);
		if (ACK.bGuildWar)
		{
			this.m_strGuildName = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), this.m_strGuildName);
		}
		this.m_lbGuildName.Text = this.m_strGuildName;
		this.Send_GS_DECLAREWAR_LIST_REQ();
		int i16Rank = (int)ACK.i16Rank;
		if (0 < i16Rank)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1689"),
				"count",
				i16Rank,
				"count2",
				ACK.i32Point
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1851"),
				"count",
				ACK.i32Point
			});
		}
		this.m_lbRank.SetText(this.m_strText);
		long num = 0L;
		long num2 = 1L;
		GUILD_EXP guildLevelUpInfo = NrTSingleton<NkGuildExpManager>.Instance.GetGuildLevelUpInfo(ACK.i16Level);
		if (guildLevelUpInfo != null)
		{
			num = ACK.i64Exp - guildLevelUpInfo.m_nExp;
		}
		GUILD_EXP guildLevelUpInfo2 = NrTSingleton<NkGuildExpManager>.Instance.GetGuildLevelUpInfo(ACK.i16Level + 1);
		if (guildLevelUpInfo2 != null && guildLevelUpInfo != null)
		{
			num2 = guildLevelUpInfo2.m_nExp - guildLevelUpInfo.m_nExp;
			if (0L > num2)
			{
				num2 = 0L;
			}
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1786"),
			"count",
			ACK.i16Level
		});
		this.m_lbLevel.SetText(this.m_strText);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("603"),
			"count1",
			num,
			"count2",
			num2
		});
		this.m_lbExpText.SetText(this.m_strText);
		float num3 = (float)num / (float)num2;
		if (1f < num3)
		{
			num3 = 1f;
		}
		this.m_dtExp.SetSize(this.m_fExpWidth * num3, this.m_dtExp.GetSize().y);
		this.m_lbMasterName.SetText(TKString.NEWString(ACK.strMasterName));
		this.m_strText = TKString.NEWString(ACK.strSubMasterName);
		if (string.Empty == this.m_strText)
		{
			this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("48");
		}
		this.m_lbSubMasterName.SetText(this.m_strText);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1303"),
			"count1",
			ACK.i16CurMemberNum,
			"count2",
			ACK.i16MaxMemberNum
		});
		this.m_lbMemberNum.SetText(this.m_strText);
		this.m_lbGuildMessage.SetText(TKString.NEWString(ACK.strGuildMessage));
		if (0 < ACK.i16ApplicantNum && 0L < this.m_lGuildID && this.m_lGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID() && NrTSingleton<NewGuildManager>.Instance.IsDischargeMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			this.m_bxApplicantNum.Hide(false);
			this.m_bxApplicantNum.SetText(ACK.i16ApplicantNum.ToString());
		}
		else
		{
			this.m_bxApplicantNum.Hide(true);
		}
		if (0L < ACK.i64CreateDate)
		{
			DateTime dueDate = PublicMethod.GetDueDate(ACK.i64CreateDate);
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
			this.m_lbGuildCreateDate.SetText(this.m_strText);
		}
		else
		{
			this.m_lbGuildCreateDate.SetText(string.Empty);
		}
		this.m_bIsDeclareWarTarget = ACK.bIsDeclareWarTarget;
		this.SetButton();
		this.m_lbFund.SetText(ANNUALIZED.Convert(ACK.i64Fund));
		switch (i16Rank)
		{
		case 1:
			this.m_dtMedal.Hide(false);
			this.m_dtMedal.SetTexture("Win_I_Rank03");
			break;
		case 2:
			this.m_dtMedal.Hide(false);
			this.m_dtMedal.SetTexture("Win_I_Rank02");
			break;
		case 3:
			this.m_dtMedal.Hide(false);
			this.m_dtMedal.SetTexture("Win_I_Rank01");
			break;
		default:
			this.m_dtMedal.Hide(true);
			break;
		}
		if (0L < this.m_lGuildID)
		{
			string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(this.m_lGuildID);
			WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebImageCallback), null);
		}
	}

	public void SetButton()
	{
		if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID() && this.m_lGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			this.m_btLeaveAndApplicantAndDeclareWar.Hide(false);
			this.m_btLeaveAndApplicantAndDeclareWar.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("592"));
			if (!NrTSingleton<NewGuildManager>.Instance.IsMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
			{
				this.m_btLeaveAndApplicantAndDeclareWar.SetEnabled(true);
			}
			else
			{
				this.m_btLeaveAndApplicantAndDeclareWar.SetEnabled(false);
			}
			if (!NrTSingleton<NewGuildManager>.Instance.IsDischargeMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
			{
				this.m_btAdmin.Hide(true);
			}
			else
			{
				this.m_btAdmin.Hide(false);
			}
		}
		else
		{
			if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				this.m_btLeaveAndApplicantAndDeclareWar.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("583"));
				this.m_btLeaveAndApplicantAndDeclareWar.Hide(false);
			}
			else if (NrTSingleton<NewGuildManager>.Instance.CanDeclareWarSet())
			{
				this.m_btLeaveAndApplicantAndDeclareWar.Hide(false);
				if (!this.m_bIsDeclareWarTarget)
				{
					this.m_btLeaveAndApplicantAndDeclareWar.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2836"));
				}
				else
				{
					this.m_btLeaveAndApplicantAndDeclareWar.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2883"));
				}
			}
			else
			{
				this.m_btLeaveAndApplicantAndDeclareWar.Hide(true);
			}
			this.m_btAdmin.Hide(true);
		}
	}

	public void Set_ApplicantNotice()
	{
		if (0 < NrTSingleton<NewGuildManager>.Instance.GetApplicantCount() && NrTSingleton<NewGuildManager>.Instance.IsDischargeMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			this.m_bxApplicantNum.Hide(false);
			this.m_bxApplicantNum.SetText(NrTSingleton<NewGuildManager>.Instance.GetApplicantCount().ToString());
		}
		else
		{
			this.m_bxApplicantNum.Hide(true);
		}
	}

	public void SetIsDeclareWarTarget(bool IsDeclareWarTarget)
	{
		this.m_bIsDeclareWarTarget = IsDeclareWarTarget;
	}

	private void ReqWebImageCallback(Texture2D txtr, object _param)
	{
		if (this.m_dtGuildMarkTexture == null)
		{
			return;
		}
		if (txtr == null)
		{
			this.m_dtGuildMarkTexture.SetTexture(NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture());
		}
		else
		{
			this.m_dtGuildMarkTexture.SetTexture(txtr);
		}
	}

	public void ClickleaveApplicantAndDeclareWar(IUIObject obj)
	{
		if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			if (this.m_lGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				if (!NrTSingleton<NewGuildManager>.Instance.IsMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
				{
					this.SetLeaveGuild();
				}
			}
			else if (NrTSingleton<NewGuildManager>.Instance.CanDeclareWarSet())
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI == null)
				{
					return;
				}
				string title = string.Empty;
				string empty = string.Empty;
				if (!this.m_bIsDeclareWarTarget)
				{
					title = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2836");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("264"),
						"targetname",
						this.m_strGuildName
					});
				}
				else
				{
					title = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2883");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("277"),
						"targetname",
						this.m_strGuildName
					});
				}
				msgBoxUI.SetMsg(new YesDelegate(this.ClickSendDeclareWar), null, null, null, title, empty, eMsgType.MB_OK_CANCEL);
			}
		}
		else if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			SendPacket.GetInstance().SendObject(1833);
			NewGuildApplicationDlg newGuildApplicationDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_APPLICATION_DLG) as NewGuildApplicationDlg;
			if (newGuildApplicationDlg != null)
			{
				newGuildApplicationDlg.SetGuildInfo(this.m_strGuildName, this.m_lGuildID);
			}
		}
	}

	public void ClickAdmin(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_ADMINMENU_DLG);
	}

	public void SetLeaveGuild()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		if (charPersonInfo.GetSoldierInfo(0) == null)
		{
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("592");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("56");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromMessageBox, new object[]
		{
			textFromMessageBox,
			"targetname",
			NrTSingleton<NewGuildManager>.Instance.GetGuildName()
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), null, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
			msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10"));
			msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
		}
	}

	public void MsgBoxOKEvent(object EventObject)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsMineMilitaryAction())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("387"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		SendPacket.GetInstance().SendObject(1813);
		base.CloseNow();
	}

	public void ClickBack(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDWAR_LIST_DLG) == null)
		{
			if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID() && NrTSingleton<NewGuildManager>.Instance.GetGuildID() == this.m_lGuildID)
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_MEMBER_DLG);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_LIST_DLG);
			}
		}
		base.CloseNow();
	}

	public void SetBundleDownload()
	{
		string textureFromBundle = string.Format("UI/Etc/GuildImg03", new object[0]);
		this.m_dtGuildFlag.SetTextureFromBundle(textureFromBundle);
	}

	public void ClickHelp1(IUIObject obj)
	{
		this.m_bHelp1 = !this.m_bHelp1;
		if (this.m_bHelp1)
		{
			base.ShowLayer(1);
		}
		else
		{
			base.AllHideLayer();
		}
	}

	public void ClickHelp2(IUIObject obj)
	{
		this.m_bHelp2 = !this.m_bHelp2;
		if (this.m_bHelp2)
		{
			base.ShowLayer(2);
		}
		else
		{
			base.AllHideLayer();
		}
	}

	public void ClickHelp3(IUIObject obj)
	{
		this.m_bHelp3 = !this.m_bHelp3;
		if (this.m_bHelp3)
		{
			base.ShowLayer(3);
		}
		else
		{
			base.AllHideLayer();
		}
	}

	public void Send_GS_DECLAREWAR_LIST_REQ()
	{
		GS_DECLAREWAR_LIST_REQ gS_DECLAREWAR_LIST_REQ = new GS_DECLAREWAR_LIST_REQ();
		gS_DECLAREWAR_LIST_REQ.GuildID = this.m_lGuildID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_DECLAREWAR_LIST_REQ, gS_DECLAREWAR_LIST_REQ);
	}

	public void ClickSendDeclareWar(object EventObject)
	{
		if (!this.m_bIsDeclareWarTarget)
		{
			GS_DECLAREWAR_SET_TARGET_REQ gS_DECLAREWAR_SET_TARGET_REQ = new GS_DECLAREWAR_SET_TARGET_REQ();
			gS_DECLAREWAR_SET_TARGET_REQ.i64TargetGuildId = this.m_lGuildID;
			SendPacket.GetInstance().SendObject(2328, gS_DECLAREWAR_SET_TARGET_REQ);
		}
		else
		{
			GS_DECLAREWAR_CANCEL_REQ obj = new GS_DECLAREWAR_CANCEL_REQ();
			SendPacket.GetInstance().SendObject(2330, obj);
		}
	}

	public void ClearDeclareWarList()
	{
		this.m_DeclareWarList.Clear();
	}

	public void SetDeclareWarInfo(string strGuildName, string targetName)
	{
		if (strGuildName == string.Empty)
		{
			return;
		}
		NewListItem newListItem = new NewListItem(this.m_DeclareWarList.ColumnNum, true, string.Empty);
		string empty = string.Empty;
		if (targetName == strGuildName)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2943"),
				"targetname",
				strGuildName
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2942"),
				"targetname",
				strGuildName
			});
		}
		newListItem.SetListItemData(0, empty, null, null, null);
		this.m_DeclareWarList.Add(newListItem);
	}

	public void SetDeclareWarInfoEnd()
	{
		this.m_DeclareWarList.RepositionItems();
	}
}
