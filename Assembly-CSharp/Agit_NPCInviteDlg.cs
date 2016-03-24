using GAME;
using PROTOCOL;
using System;
using UnityForms;

public class Agit_NPCInviteDlg : Form
{
	public const float DELAY_TIME = 1f;

	private DropDownList m_dlNPCLevel;

	private NewListBox m_nlbNPCList;

	private Button m_btInvite;

	private string m_strText = string.Empty;

	private string m_strInfo = string.Empty;

	private short m_i16NPCLevel = 1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/Agit/dlg_NPC_Invite", G_ID.AGIT_NPC_INVITE_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dlNPCLevel = (base.GetControl("DDL_DDL1") as DropDownList);
		this.m_nlbNPCList = (base.GetControl("NLB_NPCinvite") as NewListBox);
		this.m_btInvite = (base.GetControl("Button_confirm") as Button);
		this.m_btInvite.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNPCInvite));
		this.RefreshInfo();
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClickNPCInvite(IUIObject obj)
	{
		UIListItemContainer selectItem = this.m_nlbNPCList.GetSelectItem();
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

	public void RefreshInfo()
	{
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
		this.m_nlbNPCList.Clear();
		for (int i = 2; i < 6; i++)
		{
			AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(i.ToString());
			if (agitNPCData != null)
			{
				this.MakeNPCInfo(agitNPCData);
			}
		}
		this.m_nlbNPCList.RepositionItems();
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
		NewListItem newListItem = new NewListItem(this.m_nlbNPCList.ColumnNum, true, string.Empty);
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
		this.m_nlbNPCList.Add(newListItem);
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
		for (int i = 0; i < this.m_nlbNPCList.Count; i++)
		{
			UIListItemContainer item = this.m_nlbNPCList.GetItem(i);
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

	public void MsgOKNPCInvite(object a_oObject)
	{
		AgitNPCData agitNPCData = (AgitNPCData)a_oObject;
		if (agitNPCData == null)
		{
			return;
		}
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_AGIT_ADD_NPC_REQ(agitNPCData.ui8NPCType, this.m_i16NPCLevel);
	}
}
