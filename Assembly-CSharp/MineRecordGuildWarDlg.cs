using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class MineRecordGuildWarDlg : Form
{
	private NewListBox m_nlbRecordList;

	private Box m_Box_Page;

	private Button m_btBack;

	private Button m_btNextPage;

	private Button m_btPrevPage;

	private short m_page = 1;

	public override void InitializeComponent()
	{
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_REQ(1);
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "NewGuild/dlg_MineRecord_GuildWar", G_ID.MINE_RECORD_GUILDWAR_DLG, false, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btBack = (base.GetControl("Button_Back") as Button);
		this.m_btBack.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBack));
		this.m_nlbRecordList = (base.GetControl("NLB_GuildWarRecord") as NewListBox);
		this.m_btNextPage = (base.GetControl("Button_Pre") as Button);
		this.m_btNextPage.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickPrev));
		this.m_btPrevPage = (base.GetControl("Button_Next") as Button);
		this.m_btPrevPage.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickNext));
		this.m_Box_Page = (base.GetControl("Box_Page") as Box);
	}

	public void SetList(GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_ACK _ACK, NkDeserializePacket kDeserializePacket)
	{
		if (_ACK.i16Page != 1 && _ACK.ui8Count == 0)
		{
			return;
		}
		string text = string.Empty;
		this.m_Box_Page.Text = _ACK.i16Page.ToString();
		this.m_nlbRecordList.Clear();
		this.m_page = _ACK.i16Page;
		for (int i = 0; i < (int)_ACK.ui8Count; i++)
		{
			NewListItem newListItem = new NewListItem(this.m_nlbRecordList.ColumnNum, true, string.Empty);
			MINE_BATTLE_RESULT_GUILDWAR_INFO packet = kDeserializePacket.GetPacket<MINE_BATTLE_RESULT_GUILDWAR_INFO>();
			if (i == 0)
			{
				NrTSingleton<MineManager>.Instance.m_i64FirstLegionActionID_By_List = packet.i64LegionActionID;
			}
			NrTSingleton<MineManager>.Instance.m_i64LastLegionActionID_By_List = packet.i64LegionActionID;
			newListItem.Data = packet;
			if (packet.i64BattleTime >= 0L)
			{
				DateTime dueDate = PublicMethod.GetDueDate(packet.i64BattleTime);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("602"),
					"year",
					dueDate.Year,
					"month",
					dueDate.Month,
					"day",
					dueDate.Day
				});
				newListItem.SetListItemData(1, text, null, null, null);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1527"),
					"hour",
					dueDate.Hour,
					"min",
					dueDate.Minute,
					"sec",
					dueDate.Second
				});
				newListItem.SetListItemData(2, text, null, null, null);
			}
			byte grade = 0;
			MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(packet.i16MineDataID);
			if (mineCreateDataFromID != null)
			{
				grade = mineCreateDataFromID.GetGrade();
			}
			newListItem.SetListItemData(3, BASE_MINE_DATA.GetMineName(grade, packet.i16MineDataID), null, null, null);
			newListItem.SetListItemData(4, string.Empty, packet, new EZValueChangedDelegate(this.OnClickReplay), null);
			newListItem.SetListItemData(5, string.Empty, packet, new EZValueChangedDelegate(this.ClickShare), null);
			newListItem.SetListItemData(6, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("289"), null, null, null);
			newListItem.SetListItemData(7, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("496"), null, null, null);
			if (packet.bAttack)
			{
				if (packet.bWin)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2997"),
						"count",
						packet.i32GildWarPoint
					});
				}
				else
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2999");
				}
			}
			else if (packet.bWin)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2998");
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3000");
			}
			newListItem.SetListItemData(9, text, null, null, null);
			this.m_nlbRecordList.Add(newListItem);
		}
		this.m_nlbRecordList.RepositionItems();
	}

	public override void Update()
	{
	}

	public override void OnClose()
	{
	}

	public void OnClickNext(IUIObject obj)
	{
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_REQ(this.m_page + 1);
	}

	public void OnClickBack(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_INFO_REQ();
	}

	public void OnClickPrev(IUIObject obj)
	{
		if (this.m_page <= 1)
		{
			return;
		}
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_REQ(this.m_page - 1);
	}

	public void OnClickReplay(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		MINE_BATTLE_RESULT_INFO mINE_BATTLE_RESULT_INFO = obj.Data as MINE_BATTLE_RESULT_INFO;
		if (mINE_BATTLE_RESULT_INFO == null)
		{
			return;
		}
		if (!NrTSingleton<NkBattleReplayManager>.Instance.IsReplay && mINE_BATTLE_RESULT_INFO.i64LegionActionID > 0L)
		{
			NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayMineHttp(mINE_BATTLE_RESULT_INFO.i64LegionActionID);
		}
	}

	public void ClickShare(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		MINE_BATTLE_RESULT_GUILDWAR_INFO mINE_BATTLE_RESULT_GUILDWAR_INFO = obj.Data as MINE_BATTLE_RESULT_GUILDWAR_INFO;
		if (mINE_BATTLE_RESULT_GUILDWAR_INFO == null)
		{
			return;
		}
		Battle_ShareReplayDlg battle_ShareReplayDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_SHAREREPLAY_DLG) as Battle_ShareReplayDlg;
		if (battle_ShareReplayDlg != null)
		{
			battle_ShareReplayDlg.SetReplayInfo(2, mINE_BATTLE_RESULT_GUILDWAR_INFO.i64LegionActionID);
		}
	}

	private void SEND_GS_MAILBOX_REPORT_REQ(MINE_BATTLE_RESULT_INFO info)
	{
		GS_MAILBOX_REPORT_REQ gS_MAILBOX_REPORT_REQ = new GS_MAILBOX_REPORT_REQ();
		gS_MAILBOX_REPORT_REQ.i64LegionActionID = info.i64LegionActionID;
		gS_MAILBOX_REPORT_REQ.i64MailID = 0L;
		gS_MAILBOX_REPORT_REQ.i32MailType = 122;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_REPORT_REQ, gS_MAILBOX_REPORT_REQ);
	}
}
