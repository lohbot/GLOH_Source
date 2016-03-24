using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using System.Text;
using TsBundle;
using UnityForms;

public class MythRaidManager : NrTSingleton<MythRaidManager>
{
	private GS_MYTHRAID_CHARINFO_ACK myInfo;

	private bool isPartySearch;

	private bool canGetReward;

	public Dictionary<long, List<GS_BATTLE_RESULT_MYTHRAID_SOLDIER>> dic_SolInfo = new Dictionary<long, List<GS_BATTLE_RESULT_MYTHRAID_SOLDIER>>();

	public long[] partyPersonID = new long[4];

	public string[] partyPersonName = new string[4];

	public List<GS_BATTLE_RESULT_MYTHRAID_SOLDIER> list_SolInfo = new List<GS_BATTLE_RESULT_MYTHRAID_SOLDIER>();

	public int m_iGuardAngelUnique;

	private bool isParty;

	private int myRewardRank;

	private bool isRewardMail;

	public bool IsParty
	{
		get
		{
			return this.isParty;
		}
		set
		{
			this.isParty = value;
		}
	}

	public int MyRewardRank
	{
		get
		{
			return this.myRewardRank;
		}
		set
		{
			this.myRewardRank = value;
		}
	}

	public bool CanGetReward
	{
		get
		{
			return this.canGetReward;
		}
		set
		{
			this.canGetReward = value;
			MythRaid_Lobby_DLG mythRaid_Lobby_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_LOBBY_DLG) as MythRaid_Lobby_DLG;
			if (mythRaid_Lobby_DLG != null)
			{
				mythRaid_Lobby_DLG.SetRewardUI(this.canGetReward);
			}
			MythRaid_RewardInfo_DLG mythRaid_RewardInfo_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_REWARDINFO_DLG) as MythRaid_RewardInfo_DLG;
			if (mythRaid_RewardInfo_DLG != null)
			{
				mythRaid_RewardInfo_DLG.SetRewardUI(this.canGetReward);
			}
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.BATTLE);
			}
			BattleCollect_DLG battleCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLECOLLECT_DLG) as BattleCollect_DLG;
			if (battleCollect_DLG != null)
			{
				battleCollect_DLG.Update_Notice();
			}
		}
	}

	public bool IsRewardMail
	{
		get
		{
			return this.isRewardMail;
		}
		set
		{
			this.isRewardMail = value;
		}
	}

	public bool IsPartySearch
	{
		get
		{
			return this.isPartySearch;
		}
		set
		{
			this.isPartySearch = value;
			MythRaid_Lobby_DLG mythRaid_Lobby_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_LOBBY_DLG) as MythRaid_Lobby_DLG;
			if (mythRaid_Lobby_DLG != null)
			{
				mythRaid_Lobby_DLG.SetSearchVisible(this.isPartySearch);
			}
		}
	}

	private MythRaidManager()
	{
		this.myInfo = new GS_MYTHRAID_CHARINFO_ACK();
	}

	public void Init()
	{
		this.dic_SolInfo.Clear();
		for (int i = 0; i < 4; i++)
		{
			this.partyPersonID[i] = 0L;
			this.partyPersonName[i] = string.Empty;
		}
		this.IsRewardMail = false;
	}

	public void MatchStartCancel(bool isStart)
	{
		GS_MYTHRAID_PARTYSEARCH_REQ gS_MYTHRAID_PARTYSEARCH_REQ = new GS_MYTHRAID_PARTYSEARCH_REQ();
		if (isStart)
		{
			gS_MYTHRAID_PARTYSEARCH_REQ.i8MatchType = 0;
		}
		else
		{
			gS_MYTHRAID_PARTYSEARCH_REQ.i8MatchType = 1;
		}
		gS_MYTHRAID_PARTYSEARCH_REQ.i8RaidType = (byte)this.GetRaidType();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_PARTYSEARCH_REQ, gS_MYTHRAID_PARTYSEARCH_REQ);
	}

	public int MyNowBestRank()
	{
		if (this.myInfo.soloRank == 0)
		{
			return this.myInfo.partyRank;
		}
		if (this.myInfo.partyRank == 0)
		{
			return this.myInfo.soloRank;
		}
		if (this.myInfo.soloRank >= this.myInfo.partyRank)
		{
			return this.myInfo.partyRank;
		}
		return this.myInfo.soloRank;
	}

	public void SetMyInfo(GS_MYTHRAID_CHARINFO_ACK _info)
	{
		this.myInfo.nRaidType = _info.nRaidType;
		this.myInfo.clearRound = _info.clearRound;
		this.myInfo.upRankDamage = _info.upRankDamage;
		this.myInfo.nRaidSeason = _info.nRaidSeason;
		if (_info.soloRank == -1)
		{
			this.myInfo.partyRank = _info.partyRank;
			this.myInfo.partyDamage = _info.partyDamage;
		}
		else
		{
			this.myInfo.soloDamage = _info.soloDamage;
			this.myInfo.soloRank = _info.soloRank;
		}
		this.SetDlg();
	}

	public void SetRaidType(eMYTHRAID_DIFFICULTY _type)
	{
		this.myInfo.nRaidType = (byte)_type;
	}

	public void SetBattleContinueCount(int _continueCount)
	{
		if (this.myInfo.clearRound < _continueCount)
		{
			this.myInfo.clearRound = _continueCount;
		}
	}

	private void SetDlg()
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MYTHRAID_RESULT_DLG))
		{
			MythRaid_Result_DLG mythRaid_Result_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_RESULT_DLG) as MythRaid_Result_DLG;
			if (mythRaid_Result_DLG.IsParty())
			{
				mythRaid_Result_DLG.SetRank(this.myInfo.partyRank, this.myInfo.upRankDamage);
			}
			else
			{
				mythRaid_Result_DLG.SetRank(this.myInfo.soloRank, this.myInfo.upRankDamage);
			}
		}
		else if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MYTHRAID_LOBBY_DLG))
		{
			MythRaid_Lobby_DLG mythRaid_Lobby_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_LOBBY_DLG) as MythRaid_Lobby_DLG;
			mythRaid_Lobby_DLG.SetMyInfo();
		}
		else if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MYTHRAID_MODESELECT_DLG))
		{
			this.ShowLobbyDlg();
		}
	}

	public GS_MYTHRAID_CHARINFO_ACK GetMyInfo()
	{
		return this.myInfo;
	}

	public void ShowLobbyDlg()
	{
		MythRaid_Lobby_DLG mythRaid_Lobby_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTHRAID_LOBBY_DLG) as MythRaid_Lobby_DLG;
		if (this.myInfo.nRaidType == 2)
		{
			this.AskRewardInfo(true);
		}
		mythRaid_Lobby_DLG.Show();
	}

	public void MythRaidBGMOn()
	{
		TsAudio.StoreMuteAllAudio();
		TsAudio.SetExceptMuteAllAudio(EAudioType.UI, true);
		TsAudio.RefreshAllMuteAudio();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("BGM", "MYTH", "START", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), string.Empty, true);
	}

	public void AskRewardInfo(bool isAsk)
	{
		GS_MYTHRAID_GETREWARD_REQ gS_MYTHRAID_GETREWARD_REQ = new GS_MYTHRAID_GETREWARD_REQ();
		gS_MYTHRAID_GETREWARD_REQ.i8RaidType = (byte)this.GetRaidType();
		if (isAsk)
		{
			gS_MYTHRAID_GETREWARD_REQ.i8AskType = 0;
		}
		else
		{
			gS_MYTHRAID_GETREWARD_REQ.i8AskType = 1;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_GETREWARD_REQ, gS_MYTHRAID_GETREWARD_REQ);
	}

	public bool IsBestRecord(long nowBattledamage, bool isParty)
	{
		long num;
		if (isParty)
		{
			num = this.myInfo.partyDamage;
		}
		else
		{
			num = this.myInfo.soloDamage;
		}
		return nowBattledamage > num;
	}

	public long MyBestDamage(bool isParty)
	{
		if (isParty)
		{
			return this.myInfo.partyDamage;
		}
		return this.myInfo.soloDamage;
	}

	public int MyBestRank(bool isParty)
	{
		if (isParty)
		{
			return this.myInfo.partyRank;
		}
		return this.myInfo.soloRank;
	}

	public void RequestMyInfo(eMYTHRAID_DIFFICULTY raidType, bool isResult)
	{
		GS_MYTHRAID_CHARINFO_REQ gS_MYTHRAID_CHARINFO_REQ = new GS_MYTHRAID_CHARINFO_REQ();
		gS_MYTHRAID_CHARINFO_REQ.type = (byte)raidType;
		gS_MYTHRAID_CHARINFO_REQ.nPersonID = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().m_PersonID;
		gS_MYTHRAID_CHARINFO_REQ.isResult = isResult;
		gS_MYTHRAID_CHARINFO_REQ.isParty = NrTSingleton<MythRaidManager>.Instance.IsParty;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_CHARINFO_REQ, gS_MYTHRAID_CHARINFO_REQ);
	}

	public eMYTHRAID_DIFFICULTY GetRaidType()
	{
		return (eMYTHRAID_DIFFICULTY)this.myInfo.nRaidType;
	}

	public void RewardGet()
	{
		if (this.GetRaidType() == eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD)
		{
			MythRaid_RewardInfo_DLG mythRaid_RewardInfo_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTHRAID_REWARDINFO_DLG) as MythRaid_RewardInfo_DLG;
			mythRaid_RewardInfo_DLG.Show(this.MyRewardRank);
		}
	}

	public void OpenBatchMode()
	{
		GS_MYTHRAID_GOLOBBY_REQ gS_MYTHRAID_GOLOBBY_REQ = new GS_MYTHRAID_GOLOBBY_REQ();
		gS_MYTHRAID_GOLOBBY_REQ.mode = 0;
		gS_MYTHRAID_GOLOBBY_REQ.difficulty = (byte)this.GetRaidType();
		gS_MYTHRAID_GOLOBBY_REQ.nPersonID = 0L;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_GOLOBBY_REQ, gS_MYTHRAID_GOLOBBY_REQ);
	}

	public void GetReward(int[] rewardUnique, int[] rewardNumber, eMYTHRAID_DIFFICULTY difficulty)
	{
		string[] array = new string[7];
		int[] array2 = new int[7];
		for (int i = 0; i < 7; i++)
		{
			array[i] = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(rewardUnique[i]);
			array2[i] = rewardNumber[i];
		}
		switch (difficulty)
		{
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_EASY:
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_NORMAL:
		{
			MythRaid_Result_DLG mythRaid_Result_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_RESULT_DLG) as MythRaid_Result_DLG;
			if (mythRaid_Result_DLG != null)
			{
				mythRaid_Result_DLG.SetRewardInfo(array, array2);
			}
			break;
		}
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD:
		{
			MythRaid_RewardInfo_DLG mythRaid_RewardInfo_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_REWARDINFO_DLG) as MythRaid_RewardInfo_DLG;
			if (mythRaid_RewardInfo_DLG != null)
			{
				mythRaid_RewardInfo_DLG.SetRewardInfo(array, array2);
			}
			this.CanGetReward = false;
			break;
		}
		}
	}

	public void ActiveRewardEffect(AutoSpriteControlBase _obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "EXPLOERE", "BOX_OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("ui/mythicraid/fx_myth_raid_treasure_chest_mobile", _obj, _obj.GetSize());
	}

	public void ActiveRewardMsgBox(string[] itemName, int[] itemNum)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3169");
		for (int i = 0; i < 7; i++)
		{
			if (!string.IsNullOrEmpty(itemName[i]))
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromInterface,
					"rewardname",
					itemName[i],
					"rewardnum",
					itemNum[i].ToString()
				});
				stringBuilder.AppendLine(empty);
			}
		}
		stringBuilder.Remove(stringBuilder.Length - 1, 1);
		msgBoxUI.SetMsg(null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3170"), stringBuilder.ToString(), eMsgType.MB_OK, 2);
		msgBoxUI.ChangeSceneDestory = false;
		if (this.IsRewardMail)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("705");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		this.IsRewardMail = false;
	}

	public string AddComma(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}
		int num = 3;
		string text2 = string.Empty;
		for (int i = 0; i < text.Length; i++)
		{
			int index = text.Length - i - 1;
			if (i != 0 && i % num == 0)
			{
				text2 = "," + text2;
			}
			text2 = text[index] + text2;
		}
		return text2;
	}

	public int GetRank(bool isSoloMode)
	{
		if (isSoloMode)
		{
			if (this.myInfo.soloRank <= 0)
			{
				return 0;
			}
			return this.myInfo.soloRank;
		}
		else
		{
			if (this.myInfo.partyRank <= 0)
			{
				return 0;
			}
			return this.myInfo.partyRank;
		}
	}

	public long GetDamage(bool isSoloMode)
	{
		if (isSoloMode)
		{
			if (this.myInfo.soloDamage <= 0L)
			{
				return 0L;
			}
			return this.myInfo.soloDamage;
		}
		else
		{
			if (this.myInfo.partyDamage <= 0L)
			{
				return 0L;
			}
			return this.myInfo.partyDamage;
		}
	}

	public string GetMythRaidTypeText(eMYTHRAID_DIFFICULTY difficult)
	{
		switch (difficult)
		{
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_EASY:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3243");
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_NORMAL:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3244");
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3245");
		default:
			return string.Empty;
		}
	}

	private bool IsBossCharKind(int _charKind)
	{
		MYTHRAIDINFO_DATA mythRaidInfoData = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidInfoData(this.myInfo.nRaidSeason.ToString() + this.myInfo.nRaidType.ToString());
		return mythRaidInfoData != null && mythRaidInfoData.nMainBossCharKind == _charKind;
	}

	public bool IsMythRaidBossCharKind(int _charKind)
	{
		return Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID && NrTSingleton<MythRaidManager>.Instance.GetRaidType() == eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD && NrTSingleton<MythRaidManager>.Instance.IsBossCharKind(_charKind);
	}

	public bool IsMythStart()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return false;
		}
		int level = kMyCharInfo.GetLevel();
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_MYTHRAID_LIMITLEVEL);
		if (level < value)
		{
			string empty = string.Empty;
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromNotify,
				"level",
				value.ToString()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		return true;
	}

	public void SetSeason(byte i8Season)
	{
		this.myInfo.nRaidSeason = i8Season;
	}
}
