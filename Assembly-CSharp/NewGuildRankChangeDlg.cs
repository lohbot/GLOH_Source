using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityForms;

public class NewGuildRankChangeDlg : Form
{
	private const int RANK_COUNT = 5;

	private Toggle[] m_tgRank = new Toggle[5];

	private Label[] m_lbRank = new Label[5];

	private Button m_btOK;

	private Button m_btClose;

	private NewGuildMember m_GuildMember;

	private NewGuildDefine.eNEWGUILD_MEMBER_RANK m_eSelectRank;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_NewGuild_RankChange", G_ID.NEWGUILD_RANKCHANGE_DLG, true);
	}

	public override void SetComponent()
	{
		for (int i = 0; i < 5; i++)
		{
			NewGuildDefine.eNEWGUILD_MEMBER_RANK eRank = i + NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_INITIATE;
			this.m_tgRank[i] = (base.GetControl("Toggle_RadioBtn" + (i + 1).ToString()) as Toggle);
			this.m_tgRank[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRank));
			this.m_lbRank[i] = (base.GetControl("Label_position" + (i + 1).ToString()) as Label);
			this.m_lbRank[i].SetText(NrTSingleton<NewGuildManager>.Instance.GetRankText(eRank));
		}
		this.m_btOK = (base.GetControl("Button_OK") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_MEMBER_DLG);
		base.SetScreenCenter();
	}

	private void ActiveToggle(int index, bool bValue)
	{
		if (0 > index || 5 <= index)
		{
			return;
		}
		this.m_tgRank[index].controlIsEnabled = bValue;
	}

	public void SetChangeMember(NewGuildMember GuildMember)
	{
		this.m_GuildMember = GuildMember;
		if (GuildMember != null)
		{
			for (int i = 0; i < 5; i++)
			{
				this.ActiveToggle(i, true);
				if (i == this.ToIndex(GuildMember.GetRank()))
				{
					this.ActiveToggle(i, false);
				}
			}
			NewGuildDefine.eNEWGUILD_MEMBER_RANK rank = GuildMember.GetRank();
			if (NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_SUB_MASTER > rank)
			{
				this.ActiveToggle(this.ToIndex(NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_MASTER), false);
			}
		}
	}

	public void ClickRank(IUIObject obj)
	{
		Toggle x = obj as Toggle;
		NewGuildDefine.eNEWGUILD_MEMBER_RANK eSelectRank = NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_NONE;
		for (int i = 0; i < 5; i++)
		{
			if (x == this.m_tgRank[i])
			{
				eSelectRank = this.ToRank(i);
				break;
			}
		}
		this.m_eSelectRank = eSelectRank;
	}

	public void ClickOK(IUIObject obj)
	{
		if (NrTSingleton<NewGuildManager>.Instance.IsRankChange(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			NewGuildDefine.eNEWGUILD_MEMBER_RANK rank = this.m_GuildMember.GetRank();
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1833");
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("140");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromMessageBox, new object[]
				{
					textFromMessageBox,
					"targetname",
					this.m_GuildMember.GetCharName(),
					"position1",
					NrTSingleton<NewGuildManager>.Instance.GetRankText(rank),
					"position2",
					NrTSingleton<NewGuildManager>.Instance.GetRankText(this.m_eSelectRank)
				});
				msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), this.m_eSelectRank, new NoDelegate(this.MsgBoxCancelEvent), null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
				msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10"));
				msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
			}
		}
		else
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("110");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromNotify, new object[]
			{
				textFromNotify,
				"position",
				NrTSingleton<NewGuildManager>.Instance.GetRankText(NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_SUB_MASTER)
			});
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
			base.CloseNow();
		}
	}

	private NewGuildDefine.eNEWGUILD_MEMBER_RANK ToRank(int Index)
	{
		return Index + NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_INITIATE;
	}

	private int ToIndex(NewGuildDefine.eNEWGUILD_MEMBER_RANK Rank)
	{
		return this.ToIndex((byte)Rank);
	}

	private int ToIndex(byte Rank)
	{
		return (int)(Rank - 1);
	}

	public void MsgBoxOKEvent(object EventObject)
	{
		if (this.m_GuildMember != null)
		{
			GS_NEWGUILD_MEMBER_CHANGE_RANK_REQ gS_NEWGUILD_MEMBER_CHANGE_RANK_REQ = new GS_NEWGUILD_MEMBER_CHANGE_RANK_REQ();
			gS_NEWGUILD_MEMBER_CHANGE_RANK_REQ.i64PersonID_ChangeRank = this.m_GuildMember.GetPersonID();
			gS_NEWGUILD_MEMBER_CHANGE_RANK_REQ.i8NewRank = (byte)this.m_eSelectRank;
			SendPacket.GetInstance().SendObject(1817, gS_NEWGUILD_MEMBER_CHANGE_RANK_REQ);
			base.CloseNow();
		}
	}

	public void MsgBoxCancelEvent(object EventObject)
	{
		base.CloseNow();
	}
}
