using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class ColosseumBattleRecordDlg : Form
{
	private Button m_btClose;

	private Label m_laMyColosseum_WinCount;

	private NewListBox m_lbColossenumRecordList;

	private List<COLOSSEUM_RECORDINFO> record_list = new List<COLOSSEUM_RECORDINFO>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/dlg_colosseum_battlerecord", G_ID.COLOSSEUM_BATTLE_RECORD_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_btClose = (base.GetControl("Close_Button") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this.m_laMyColosseum_WinCount = (base.GetControl("Label_WinCount") as Label);
		this.m_lbColossenumRecordList = (base.GetControl("NewListBox_fight_record") as NewListBox);
		GS_COLOSSEUM_RECORD_LIST_GET_REQ gS_COLOSSEUM_RECORD_LIST_GET_REQ = new GS_COLOSSEUM_RECORD_LIST_GET_REQ();
		gS_COLOSSEUM_RECORD_LIST_GET_REQ.i64PersonID = 0L;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_RECORD_LIST_GET_REQ, gS_COLOSSEUM_RECORD_LIST_GET_REQ);
		base.SetScreenCenter();
		this.Hide();
	}

	public override void Show()
	{
		this.ShowList();
		base.Show();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void AddRecordInfo(COLOSSEUM_RECORDINFO info)
	{
		this.record_list.Add(info);
	}

	public void ShowList()
	{
		string text = string.Empty;
		string text2 = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2759");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"count",
			kMyCharInfo.ColosseumWinCount
		});
		this.m_laMyColosseum_WinCount.SetText(text2);
		this.m_lbColossenumRecordList.Clear();
		foreach (COLOSSEUM_RECORDINFO current in this.record_list)
		{
			NewListItem newListItem = new NewListItem(this.m_lbColossenumRecordList.ColumnNum, true, string.Empty);
			newListItem.SetListItemData(0, true);
			DateTime dueDate = PublicMethod.GetDueDate(current.i64BattleTime);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("602");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"year",
				dueDate.Year,
				"month",
				dueDate.Month,
				"day",
				dueDate.Day
			});
			newListItem.SetListItemData(1, text2, null, null, null);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1527");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"hour",
				dueDate.Hour,
				"min",
				dueDate.Minute,
				"sec",
				dueDate.Second
			});
			newListItem.SetListItemData(2, text2, null, null, null);
			text2 = TKString.NEWString(current.szTargetName);
			newListItem.SetListItemData(3, text2, null, null, null);
			text2 = "Lv." + current.iTargetLevel.ToString();
			newListItem.SetListItemData(4, text2, null, null, null);
			if (current.i64WinPersonID == charPersonInfo.GetPersonID())
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("234");
			}
			else
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("235");
			}
			newListItem.SetListItemData(5, text2, null, null, null);
			newListItem.SetListItemData(6, string.Empty, current, new EZValueChangedDelegate(this.ClickReplay), null);
			newListItem.SetListItemData(7, string.Empty, current, new EZValueChangedDelegate(this.ClickShare), null);
			newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("289"), null, null, null);
			newListItem.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("496"), null, null, null);
			this.m_lbColossenumRecordList.Add(newListItem);
		}
		this.m_lbColossenumRecordList.RepositionItems();
	}

	public void ClickReplay(IUIObject obj)
	{
		COLOSSEUM_RECORDINFO cOLOSSEUM_RECORDINFO = obj.Data as COLOSSEUM_RECORDINFO;
		if (cOLOSSEUM_RECORDINFO != null)
		{
			NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayColosseumHttp(cOLOSSEUM_RECORDINFO.i64Colosseum_battleunique);
		}
		this.Close();
	}

	public void ClickShare(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		COLOSSEUM_RECORDINFO cOLOSSEUM_RECORDINFO = (COLOSSEUM_RECORDINFO)obj.Data;
		if (cOLOSSEUM_RECORDINFO == null)
		{
			return;
		}
		Battle_ShareReplayDlg battle_ShareReplayDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_SHAREREPLAY_DLG) as Battle_ShareReplayDlg;
		if (battle_ShareReplayDlg != null)
		{
			battle_ShareReplayDlg.SetReplayInfo(2, cOLOSSEUM_RECORDINFO.i64Colosseum_battleunique);
		}
	}
}
