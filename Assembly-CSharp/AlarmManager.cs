using FiveRocksUnity;
using GAME;
using omniata;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using StageHelper;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

internal class AlarmManager
{
	private static AlarmManager ms_instance;

	private List<long> m_SolIDList = new List<long>();

	private List<int> m_lstEventType = new List<int>();

	private List<int> m_lstEventTitleText = new List<int>();

	private List<int> m_lstEventExplainText = new List<int>();

	private NkSoldierInfo m_kSolInfo;

	private int m_nEventType;

	private int m_nEventTitleText;

	private int m_nEventExplainText;

	public DateTime m_olddt = PublicMethod.GetDueDate(PublicMethod.GetCurTime());

	public static AlarmManager GetInstance()
	{
		if (AlarmManager.ms_instance == null)
		{
			AlarmManager.ms_instance = new AlarmManager();
		}
		return AlarmManager.ms_instance;
	}

	public void AddSolAlarm(long i64SolID)
	{
		if (this.m_SolIDList.Contains(i64SolID))
		{
			return;
		}
		this.m_SolIDList.Add(i64SolID);
	}

	public void AddEventType(int nEventType, int nEventTitle, int nEventExplain)
	{
		if (this.m_lstEventType.Contains(nEventType))
		{
			return;
		}
		this.m_lstEventType.Add(nEventType);
		if (this.m_lstEventTitleText.Contains(nEventTitle))
		{
			return;
		}
		this.m_lstEventTitleText.Add(nEventTitle);
		if (this.m_lstEventExplainText.Contains(nEventExplain))
		{
			return;
		}
		this.m_lstEventExplainText.Add(nEventExplain);
	}

	public void ShowLevelUpAlarm()
	{
		NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.RECOMMEND_SOL);
		NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.REVIEW);
		NrTSingleton<NkQuestManager>.Instance.UpdateClientNpc(0);
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser != null)
		{
			NrTSingleton<NkEffectManager>.Instance.AddEffect("LEVELUP", nrCharUser);
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo != null)
			{
				BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
				if (bookmarkDlg != null)
				{
					bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.SOLINFO);
				}
				FiveRocks.SetUserLevel(kMyCharInfo.GetLevel());
				string comment = string.Format("levelup{0}", kMyCharInfo.GetLevel());
				NrTSingleton<FiveRocksEventManager>.Instance.Placement(comment);
				DateTime dateTime = DateTime.Now.ToLocalTime();
				DateTime arg_D3_0 = dateTime;
				DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
				int num = (int)(arg_D3_0 - dateTime2.ToLocalTime()).TotalSeconds;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ts", num.ToString());
				dictionary.Add("level", kMyCharInfo.GetLevel().ToString());
				dictionary.Add("account_id", kMyCharInfo.m_SN.ToString());
				GameObject pkGoOminiata = StageSystemCheck.pkGoOminiata;
				if (pkGoOminiata)
				{
					OmniataComponent component = pkGoOminiata.GetComponent<OmniataComponent>();
					if (component)
					{
						component.Track("om_level", dictionary);
					}
				}
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel() >= 10 && PlayerPrefs.GetInt(NrPrefsKey.PLAYER_PLAYLOCKDEVICEID_SEND, 0) == 0)
				{
					TsPlatform.Operator.GetPlayLockID();
				}
			}
			NrTSingleton<NkClientLogic>.Instance.ShowChangeGuestIDUI();
		}
	}

	private void ShowLevelUpSoldier()
	{
		Main_UI_LevelUpAlarmSoldier main_UI_LevelUpAlarmSoldier = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_UI_LEVELUP_ALARM_SOLDIER) as Main_UI_LevelUpAlarmSoldier;
		if (main_UI_LevelUpAlarmSoldier == null)
		{
			main_UI_LevelUpAlarmSoldier = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_UI_LEVELUP_ALARM_SOLDIER) as Main_UI_LevelUpAlarmSoldier);
		}
		if (main_UI_LevelUpAlarmSoldier != null && this.m_kSolInfo != null)
		{
			main_UI_LevelUpAlarmSoldier.SetInfo(this.m_kSolInfo);
			main_UI_LevelUpAlarmSoldier.Show();
		}
		BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
		if (bookmarkDlg != null)
		{
			bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.SOLINFO);
		}
		TsAudioManager.Container.RequestAudioClip("UI_SFX", "MERCENARY", "LEVEL_UP", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void ShowEventAlarm()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int level = kMyCharInfo.GetLevel();
		BUNNING_EVENT_INFO value = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)this.m_nEventType);
		if (value == null)
		{
			return;
		}
		if (level > value.m_nLimitLevel)
		{
			if (value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_DAILYDUNGEON && NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.SetBasicData())
			{
				return;
			}
			if (value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_DAILYQUEST || value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_DAILYQUEST1 || value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_DAILYQUEST2)
			{
				int num = (int)kMyCharInfo.GetCharDetail(5);
				if (0 < num && NrTSingleton<NkQuestManager>.Instance.IsCompletedQuestGroup(num))
				{
					return;
				}
			}
			Main_UI_LevelUpAlarmSoldier main_UI_LevelUpAlarmSoldier = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_UI_LEVELUP_ALARM_SOLDIER) as Main_UI_LevelUpAlarmSoldier;
			if (main_UI_LevelUpAlarmSoldier == null)
			{
				main_UI_LevelUpAlarmSoldier = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_UI_LEVELUP_ALARM_SOLDIER) as Main_UI_LevelUpAlarmSoldier);
			}
			if (main_UI_LevelUpAlarmSoldier != null)
			{
				DateTime dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
				TimeSpan t = new TimeSpan(0, 0, 30);
				if (dueDate < this.m_olddt)
				{
					return;
				}
				main_UI_LevelUpAlarmSoldier.SetEventInfo(this.m_nEventType, this.m_nEventTitleText, this.m_nEventExplainText);
				main_UI_LevelUpAlarmSoldier.Show();
				this.m_olddt = dueDate + t;
			}
			TsAudioManager.Container.RequestAudioClip("UI_SFX", "MERCENARY", "LEVEL_UP", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public void CloseLevelUpAlarm()
	{
		this.m_kSolInfo = null;
	}

	public void LevelUpAlarmUpdate()
	{
		if (!CommonTasks.IsEndOfPrework)
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXPLORATION_PLAY_DLG) != null)
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXPLORATION_REWARD_DLG) != null)
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXPLORATION_DLG) != null)
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_GRADE_UP_SUCCESS_DLG) != null)
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_SUCCESS_DLG) != null)
		{
			return;
		}
		if (this.m_SolIDList.Count == 0 && this.m_lstEventType.Count == 0)
		{
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsEffectEnable())
		{
			return;
		}
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return;
		}
		if (this.m_SolIDList.Count > 0)
		{
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MAIN_UI_LEVELUP_ALARM_MONARCH) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MAIN_UI_LEVELUP_ALARM_SOLDIER))
			{
				return;
			}
			this.m_SolIDList.Sort(new Comparison<long>(AlarmManager.CompareLevel));
			long num = this.m_SolIDList[0];
			NrPersonInfoBase personInfo = nrCharUser.GetPersonInfo();
			if (personInfo == null)
			{
				return;
			}
			NkSoldierInfo soldierInfoFromSolID = personInfo.GetSoldierInfoFromSolID(num);
			if (soldierInfoFromSolID == null)
			{
				return;
			}
			if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(soldierInfoFromSolID.GetCharKind()) == null)
			{
				return;
			}
			this.m_kSolInfo = soldierInfoFromSolID;
			if (num == nrCharUser.GetUserSoldierInfo().GetSolID())
			{
				this.ShowLevelUpAlarm();
			}
			else
			{
				this.ShowLevelUpSoldier();
			}
			this.m_SolIDList.RemoveAt(0);
		}
		if (this.m_SolIDList.Count == 0 && this.m_lstEventType.Count > 0)
		{
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MAIN_UI_LEVELUP_ALARM_MONARCH) || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MAIN_UI_LEVELUP_ALARM_SOLDIER))
			{
				return;
			}
			this.m_nEventType = this.m_lstEventType[0];
			this.m_nEventTitleText = this.m_lstEventTitleText[0];
			this.m_nEventExplainText = this.m_lstEventExplainText[0];
			this.ShowEventAlarm();
			this.m_lstEventType.RemoveAt(0);
			this.m_lstEventTitleText.RemoveAt(0);
			this.m_lstEventExplainText.RemoveAt(0);
		}
	}

	private static int CompareLevel(long x, long y)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return -1;
		}
		NrPersonInfoBase personInfo = nrCharUser.GetPersonInfo();
		if (personInfo == null)
		{
			return -1;
		}
		NkSoldierInfo soldierInfoFromSolID = personInfo.GetSoldierInfoFromSolID(x);
		NkSoldierInfo soldierInfoFromSolID2 = personInfo.GetSoldierInfoFromSolID(y);
		if (soldierInfoFromSolID.GetLevel() < soldierInfoFromSolID2.GetLevel())
		{
			return 1;
		}
		if (soldierInfoFromSolID.GetLevel() == soldierInfoFromSolID2.GetLevel())
		{
			if (soldierInfoFromSolID.GetCharKind() > soldierInfoFromSolID2.GetCharKind())
			{
				return 1;
			}
			if (soldierInfoFromSolID.GetCharKind() == soldierInfoFromSolID2.GetCharKind())
			{
				return 0;
			}
		}
		return -1;
	}

	public static void On_OK_URL(object a_oObject)
	{
		string url = string.Format("http://{0}/mobile/updateurl.aspx?code={1}", NrGlobalReference.strWebPageDomain, NrGlobalReference.MOBILEID);
		Application.OpenURL(url);
	}

	public static void On_OK_SERVEY_URL(object a_oObject)
	{
		GS_CHAR_CHALLENGE_REQ gS_CHAR_CHALLENGE_REQ = new GS_CHAR_CHALLENGE_REQ();
		gS_CHAR_CHALLENGE_REQ.i16ChallengeUnique = 4010;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_CHALLENGE_REQ, gS_CHAR_CHALLENGE_REQ);
	}
}
