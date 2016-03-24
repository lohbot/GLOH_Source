using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class PlunderRecordDlg : Form
{
	private Button m_btOk;

	private Label m_lbMatchPoint;

	private Label m_lbRank;

	private NewListBox m_LBRecordList;

	private List<PLUNDER_RECORDINFO> record_list = new List<PLUNDER_RECORDINFO>();

	private Label m_Label_infiniterank2;

	private Label m_Label_winningstreak2;

	private Label m_Label_latelyrank2;

	private NewListBox m_infiLBRecordList;

	private Label m_Label_winningrate;

	private DrawTexture m_dtRankIcon;

	private DrawTexture m_dtLateRankIcon;

	private List<INFIBATTLE_RECORDINFO> infirecord_list = new List<INFIBATTLE_RECORDINFO>();

	private int Rank;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/dlg_pvp_record", G_ID.PLUNDERRECORD_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_btOk = (base.GetControl("Button_ok") as Button);
		Button expr_1C = this.m_btOk;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.ClickOK));
		this.m_lbMatchPoint = (base.GetControl("Label_matchpoint2") as Label);
		this.m_lbRank = (base.GetControl("Label_rank2") as Label);
		this.m_LBRecordList = (base.GetControl("pvp_recordlist") as NewListBox);
		this.m_Label_infiniterank2 = (base.GetControl("Label_infiniterank2") as Label);
		this.m_Label_winningstreak2 = (base.GetControl("Label_winningstreak2") as Label);
		this.m_Label_latelyrank2 = (base.GetControl("Label_latelyrank2") as Label);
		this.m_infiLBRecordList = (base.GetControl("infipvp_recordlist") as NewListBox);
		this.m_Label_winningrate = (base.GetControl("Label_winningrate") as Label);
		this.m_dtRankIcon = (base.GetControl("DT_infiniteIcon") as DrawTexture);
		this.m_dtLateRankIcon = (base.GetControl("DT_latelyIcon") as DrawTexture);
		base.SetScreenCenter();
		this.InitData();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void AddPlunderRecordInfo(PLUNDER_RECORDINFO info)
	{
		this.record_list.Add(info);
	}

	public void AddInfiBattleRecordInfo(INFIBATTLE_RECORDINFO info)
	{
		this.infirecord_list.Add(info);
	}

	public void SetInfiBattleInfo()
	{
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		this.ShowinfiBattleInfo();
	}

	public void ShowPlunderInfo()
	{
		string text = string.Empty;
		string text2 = string.Empty;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long num = 1000L + myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_MATCHPOINT);
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2091");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"ratingpoint",
			num
		});
		this.m_lbMatchPoint.SetText(text2);
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2093");
		if (this.Rank > 0)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"rank",
				this.Rank
			});
		}
		else
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("292");
		}
		this.m_lbRank.SetText(text2);
		this.ShowPlunderList();
	}

	public void ShowPlunderList()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		this.m_LBRecordList.Clear();
		foreach (PLUNDER_RECORDINFO current in this.record_list)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			NewListItem newListItem = new NewListItem(this.m_LBRecordList.ColumnNum, true, string.Empty);
			DateTime dueDate = PublicMethod.GetDueDate(current.i64BattleTime);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("293");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"month",
				dueDate.Month,
				"day",
				dueDate.Day,
				"hour",
				dueDate.Hour,
				"min",
				dueDate.Minute,
				"sec",
				dueDate.Second
			});
			newListItem.SetListItemData(2, text2, null, null, null);
			if (current.i64AttackPersonID == charPersonInfo.GetPersonID())
			{
				newListItem.SetListItemData(0, false);
				if (current.byWinType == 0)
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("139");
				}
				else
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("140");
				}
			}
			else
			{
				newListItem.SetListItemData(0, true);
				if (current.byWinType == 0)
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("142");
				}
				else
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("141");
				}
			}
			newListItem.SetListItemData(3, text2, null, null, null);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("471");
			if (current.i64AttackPersonID == charPersonInfo.GetPersonID())
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"targetname",
					TKString.NEWString(current.szDefenceCharName),
					"count",
					current.i16DefenceCharLevel
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"targetname",
					TKString.NEWString(current.szAttackCharName),
					"count",
					current.i16AttackCharLevel
				});
			}
			newListItem.SetListItemData(4, text2, null, null, null);
			newListItem.SetListItemData(5, current.i64PlunderMoney.ToString(), null, null, null);
			newListItem.SetListItemData(6, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("496"), current, new EZValueChangedDelegate(this.ClickShareReply), null);
			newListItem.SetListItemData(7, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("289"), current, new EZValueChangedDelegate(this.ClickReplay), null);
			newListItem.SetListItemData(7, true);
			this.m_LBRecordList.Add(newListItem);
		}
		this.m_LBRecordList.RepositionItems();
	}

	public void ShowinfiBattleInfo()
	{
		string text = string.Empty;
		string text2 = string.Empty;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		int num = 0;
		if (instance != null)
		{
			num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_RANKLIMIT);
		}
		if (num < myCharInfo.InfinityBattle_Rank || 0 >= myCharInfo.InfinityBattle_Rank)
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2093");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"rank",
				myCharInfo.InfinityBattle_Rank
			});
		}
		this.m_dtRankIcon.SetTexture(this.GetRankImg(myCharInfo.InfinityBattle_Rank));
		this.m_Label_infiniterank2.SetText(text2);
		num = 0;
		if (instance != null)
		{
			num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_RANKLIMIT);
		}
		if (num < myCharInfo.InfinityBattle_OldRank || 0 >= myCharInfo.InfinityBattle_OldRank)
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2093");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"rank",
				myCharInfo.InfinityBattle_OldRank
			});
		}
		this.m_Label_latelyrank2.SetText(text2);
		this.m_dtLateRankIcon.SetTexture(this.GetRankImg(myCharInfo.InfinityBattle_OldRank));
		if (myCharInfo.InifBattle_WinCount > 0)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2759");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"count",
				myCharInfo.InifBattle_WinCount
			});
			this.m_Label_winningstreak2.SetText(text2);
		}
		else
		{
			this.m_Label_winningstreak2.SetText(string.Empty);
		}
		float num2 = (float)myCharInfo.InifBattle_WinCount / (float)myCharInfo.InifBattle_TotalCount;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2936");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"count",
			(int)(num2 * 100f)
		});
		this.m_Label_winningrate.SetText(text2);
		this.ShowinfiBattleList();
	}

	public void ShowinfiBattleList()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		this.m_infiLBRecordList.Clear();
		foreach (INFIBATTLE_RECORDINFO current in this.infirecord_list)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			NewListItem newListItem = new NewListItem(this.m_infiLBRecordList.ColumnNum, true, string.Empty);
			DateTime dueDate = PublicMethod.GetDueDate(current.i64BattleTime);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("293");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"month",
				dueDate.Month,
				"day",
				dueDate.Day,
				"hour",
				dueDate.Hour,
				"min",
				dueDate.Minute,
				"sec",
				dueDate.Second
			});
			newListItem.SetListItemData(1, text2, null, null, null);
			UIBaseInfoLoader loader;
			if (current.i64AttackPersonID == charPersonInfo.GetPersonID())
			{
				if (current.ui8WinType == 0)
				{
					loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_ArrowUp2");
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2313");
				}
				else
				{
					loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_ArrowMaintain");
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2314");
				}
			}
			else if (current.ui8WinType == 0)
			{
				loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_ArrowDown2");
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2316");
			}
			else
			{
				loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_ArrowMaintain");
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2315");
			}
			newListItem.SetListItemData(6, loader, null, null, null);
			newListItem.SetListItemData(2, text2, null, null, null);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("471");
			if (current.i64AttackPersonID == charPersonInfo.GetPersonID())
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"targetname",
					TKString.NEWString(current.szDefenceCharName),
					"count",
					current.i16DefenceCharLevel
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"targetname",
					TKString.NEWString(current.szAttackCharName),
					"count",
					current.i16AttackCharLevel
				});
			}
			newListItem.SetListItemData(3, text2, null, null, null);
			newListItem.SetListItemData(4, string.Empty, current, new EZValueChangedDelegate(this.ClickShareReply), null);
			newListItem.SetListItemData(5, string.Empty, current, new EZValueChangedDelegate(this.ClickReplay), null);
			if (current.i64AttackPersonID == charPersonInfo.GetPersonID())
			{
				if (current.ui8WinType == 0)
				{
					if (current.i32OldRank == current.i32NewRank)
					{
						text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2232");
					}
					else
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2230"),
							"rankcount",
							current.i32OldRank - current.i32NewRank
						});
					}
				}
				else
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2232");
				}
			}
			else if (current.ui8WinType == 0)
			{
				if (current.i32OldRank == current.i32NewRank)
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2232");
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2231"),
						"rankcount",
						current.i32NewRank - current.i32OldRank
					});
				}
			}
			else
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2232");
			}
			newListItem.SetListItemData(7, text2, null, null, null);
			newListItem.SetListItemData(12, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("289"), null, null, null);
			newListItem.SetListItemData(13, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("496"), null, null, null);
			this.m_infiLBRecordList.Add(newListItem);
		}
		this.m_infiLBRecordList.RepositionItems();
	}

	public UIBaseInfoLoader GetRankImg(int i32Rank)
	{
		string key = string.Empty;
		if (i32Rank == 1)
		{
			key = "Win_I_Rank03";
		}
		else if (i32Rank == 2)
		{
			key = "Win_I_Rank02";
		}
		else if (i32Rank == 3)
		{
			key = "Win_I_Rank01";
		}
		else
		{
			key = string.Empty;
		}
		return NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(key);
	}

	public void ClickOK(IUIObject obj)
	{
		this.Close();
	}

	public void ClickShareReply(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		INFIBATTLE_RECORDINFO iNFIBATTLE_RECORDINFO = (INFIBATTLE_RECORDINFO)obj.Data;
		if (iNFIBATTLE_RECORDINFO == null)
		{
			return;
		}
		Battle_ShareReplayDlg battle_ShareReplayDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_SHAREREPLAY_DLG) as Battle_ShareReplayDlg;
		if (battle_ShareReplayDlg != null)
		{
			battle_ShareReplayDlg.SetReplayInfo(3, iNFIBATTLE_RECORDINFO.i64InfinityBattleRecordID);
		}
	}

	public void ClickReplay(IUIObject obj)
	{
		INFIBATTLE_RECORDINFO iNFIBATTLE_RECORDINFO = obj.Data as INFIBATTLE_RECORDINFO;
		if (iNFIBATTLE_RECORDINFO != null)
		{
			NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayinfiBattleHttp(iNFIBATTLE_RECORDINFO.i64InfinityBattleRecordID);
		}
	}
}
