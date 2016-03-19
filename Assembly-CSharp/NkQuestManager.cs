using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using PROTOCOL.WORLD;
using StageHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class NkQuestManager : NrTSingleton<NkQuestManager>
{
	public bool m_bIsProlog;

	private bool m_bToggleQuestUnique;

	private QuestAutoPathInfo m_AutoPathInfo;

	private NrQuestCamera m_kQuestCamera = new NrQuestCamera();

	private Queue<CQuestNpcTell> m_kQuestNpcTell = new Queue<CQuestNpcTell>();

	private float m_fScrollPos;

	public ITEM[] CompleteItem = new ITEM[3];

	private USER_QUEST_INFO m_UserQuestInfo = new USER_QUEST_INFO();

	private QUEST_PARAM_CHECK[] m_ParamNyf = new QUEST_PARAM_CHECK[10];

	private float m_fRequestTime;

	private bool m_bRequest;

	public List<CQuestMessage> m_MessageList = new List<CQuestMessage>();

	private List<NrQuestNpcCheckInfo> m_ClientNpcList = new List<NrQuestNpcCheckInfo>();

	private List<NrQuestParamNfyTemp> m_ParamNfyTempQueue = new List<NrQuestParamNfyTemp>();

	private NrQuestDlgCurrentInfo m_kQuestDlgInfo = new NrQuestDlgCurrentInfo();

	public bool m_bEventDramaOff;

	private Queue<string> m_DebugMsgQueue = new Queue<string>();

	private bool m_bRewardShow;

	private Queue<RewardInfo> m_QuestRewardInfoQueue = new Queue<RewardInfo>();

	private RewardInfo m_groupReward = new RewardInfo();

	private bool m_QuestFin;

	private int m_i32GroupUnique;

	private Camera m_MainCamera;

	private int m_nCharKind;

	private string m_szQuestUnique = string.Empty;

	private int m_nAutoMoveNpcKind;

	private int m_nAutoMoveMapIndex;

	private int m_nAutoMoveGateIndex;

	private string m_szAutoMoveQuestUnique = string.Empty;

	private bool m_bAutoMove;

	private short m_nAutoMoveDestX;

	private short m_nAutoMoveDestY;

	public bool m_showMsgDlg = true;

	public static bool m_downLoadBundle = false;

	public static string m_szBundlePath = string.Empty;

	private GameObject rootGameObject;

	private GameObject dramra;

	private GameObject main;

	private Dictionary<string, CQuest> m_HashQuest = new Dictionary<string, CQuest>();

	private Dictionary<int, CQuestGroup> m_HashQuestGroup = new Dictionary<int, CQuestGroup>();

	private Dictionary<int, NPC_QUEST_LIST> m_HashNpcQuestMatchTable = new Dictionary<int, NPC_QUEST_LIST>();

	private Dictionary<string, QUEST_DIALOGUE_HASH> m_HashQuestDlg = new Dictionary<string, QUEST_DIALOGUE_HASH>();

	private Dictionary<short, QUEST_CHAPTER> m_HashQuestChapter = new Dictionary<short, QUEST_CHAPTER>();

	private Dictionary<int, QUEST_AUTO_PATH_POS> m_HashQuestAutoPath = new Dictionary<int, QUEST_AUTO_PATH_POS>();

	private Dictionary<int, NrClientNpcPosList> m_HashQuestNpcPos = new Dictionary<int, NrClientNpcPosList>();

	private Dictionary<int, QUEST_GROUP_REWARD> m_HashQuestGroupReward = new Dictionary<int, QUEST_GROUP_REWARD>();

	private Dictionary<string, CAutoQuest> m_HashAutoQuest = new Dictionary<string, CAutoQuest>();

	private int m_i32TotalQuestCount;

	private Dictionary<string, QUEST_DROP_ITEM_List> m_QuestDropItem = new Dictionary<string, QUEST_DROP_ITEM_List>();

	public Action<string> p_deQuestUpdate
	{
		get;
		set;
	}

	public float ScrollPos
	{
		get
		{
			return this.m_fScrollPos;
		}
		set
		{
			this.m_fScrollPos = value;
		}
	}

	public float RequestTime
	{
		get
		{
			return this.m_fRequestTime;
		}
	}

	public bool Request
	{
		get
		{
			return this.m_bRequest;
		}
		set
		{
			this.m_bRequest = value;
			if (value)
			{
				this.m_fRequestTime = Time.realtimeSinceStartup;
			}
			else
			{
				this.m_fRequestTime = 0f;
			}
		}
	}

	public List<NrQuestParamNfyTemp> ParamNfyTempQueue
	{
		get
		{
			return this.m_ParamNfyTempQueue;
		}
		set
		{
			this.m_ParamNfyTempQueue = value;
		}
	}

	public int I32RewardCount
	{
		get
		{
			return this.m_QuestRewardInfoQueue.Count;
		}
	}

	public bool RewardShow
	{
		get
		{
			return this.m_bRewardShow;
		}
		set
		{
			this.m_bRewardShow = value;
		}
	}

	public int I32GroupUnique
	{
		get
		{
			return this.m_i32GroupUnique;
		}
		set
		{
			this.m_i32GroupUnique = value;
		}
	}

	public bool QuestFin
	{
		get
		{
			return this.m_QuestFin;
		}
		set
		{
			this.m_QuestFin = value;
		}
	}

	public Camera MainCamera
	{
		get
		{
			return this.m_MainCamera;
		}
		set
		{
			this.m_MainCamera = value;
		}
	}

	public int AutoMoveMapIndex
	{
		set
		{
			this.m_nAutoMoveMapIndex = value;
		}
	}

	public bool AutoMove
	{
		set
		{
			this.m_bAutoMove = value;
		}
	}

	public Vector2 AutoMoveDestPos
	{
		set
		{
			this.m_nAutoMoveDestX = (short)value.x;
			this.m_nAutoMoveDestY = (short)value.y;
		}
	}

	public bool DownLoadBundle
	{
		get
		{
			return NkQuestManager.m_downLoadBundle;
		}
		set
		{
			NkQuestManager.m_downLoadBundle = value;
		}
	}

	public string BundlePath
	{
		get
		{
			return NkQuestManager.m_szBundlePath;
		}
		set
		{
			NkQuestManager.m_szBundlePath = value;
		}
	}

	private NkQuestManager()
	{
		for (byte b = 0; b < 10; b += 1)
		{
			this.m_ParamNyf[(int)b] = new QUEST_PARAM_CHECK();
		}
		for (int i = 0; i < 3; i++)
		{
			this.CompleteItem[i] = new ITEM();
		}
	}

	public void InitQuest()
	{
		this.m_UserQuestInfo.Init();
		for (byte b = 0; b < 10; b += 1)
		{
			this.m_ParamNyf[(int)b].Init();
		}
	}

	public void SetCompleteItem(ITEM item1, ITEM item2, ITEM item3)
	{
		this.CompleteItem[0] = item1;
		this.CompleteItem[1] = item2;
		this.CompleteItem[2] = item3;
	}

	public void InitCompleteItem()
	{
		for (int i = 0; i < 3; i++)
		{
			this.CompleteItem[i] = new ITEM();
		}
	}

	public void ToggleQeustUnique()
	{
		this.m_bToggleQuestUnique = !this.m_bToggleQuestUnique;
		RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_QUEST);
		if (rightMenuQuestUI != null)
		{
			rightMenuQuestUI.QuestUpdate();
		}
	}

	public NrQuestCamera GetQuestCamera()
	{
		return this.m_kQuestCamera;
	}

	public void SetQuestCamera(NrCharBase kCharBase, Transform pkCamera)
	{
		this.m_kQuestCamera.SetQuestCamera(kCharBase, pkCamera);
	}

	public void ReleaseQuestCamera()
	{
		this.m_kQuestCamera.Release();
	}

	public bool GetToggleQuestUnique()
	{
		return this.m_bToggleQuestUnique;
	}

	public void SetAutoPathInfo(QuestAutoPathInfo curCondition)
	{
		this.m_AutoPathInfo = curCondition;
	}

	public QuestAutoPathInfo GetAutoPathInfo()
	{
		return this.m_AutoPathInfo;
	}

	public void ClearCompleteQuest()
	{
		this.m_UserQuestInfo.m_dicCompleteInfo.Clear();
	}

	public void ClearCurrentQuest()
	{
		for (byte b = 0; b < 10; b += 1)
		{
			this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].Init();
		}
	}

	public void AddCurrentQuest(USER_CURRENT_QUEST_INFO_PACKET currentQuest)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (string.Empty == this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique)
			{
				this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].SetUserCurrentQuestInfo(currentQuest);
				this.SetCheckParamInfo(b, this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b]);
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique);
				if (questByQuestUnique == null || questByQuestUnique.GetQuestCommon().i32QuestTime > 0)
				{
				}
				break;
			}
		}
	}

	public void SetCheckParamInfo(byte bNum, USER_CURRENT_QUEST_INFO kCurrentInfo)
	{
		string strQuestUnique = kCurrentInfo.strQuestUnique;
		CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
		if (questByQuestUnique == null)
		{
			return;
		}
		this.m_ParamNyf[(int)bNum].strQuestUnique = strQuestUnique;
		for (int i = 0; i < 3; i++)
		{
			this.m_ParamNyf[(int)bNum].kQuestUpdateInfo[i].i32QuestCode = questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode;
			this.m_ParamNyf[(int)bNum].kQuestUpdateInfo[i].i64Param = questByQuestUnique.GetQuestCommon().cQuestCondition[i].i64Param;
			this.m_ParamNyf[(int)bNum].kQuestUpdateInfo[i].i64PreParamVal = kCurrentInfo.i64ParamVal[i];
		}
	}

	public void AddCompleteQuest(USER_QUEST_COMPLETE_INFO kCompleteQuest)
	{
		if (!this.m_UserQuestInfo.m_dicCompleteInfo.ContainsKey(kCompleteQuest.i32GroupUnique))
		{
			this.m_UserQuestInfo.m_dicCompleteInfo[kCompleteQuest.i32GroupUnique] = kCompleteQuest;
		}
	}

	public bool IsDayQuest(string strQuestUnique)
	{
		CQuest questByQuestUnique = this.GetQuestByQuestUnique(strQuestUnique);
		return questByQuestUnique != null && questByQuestUnique.IsDayQuest();
	}

	public bool IsCompletedQuestGroup(int unique)
	{
		if (this.m_UserQuestInfo.m_dicCompleteInfo.ContainsKey(unique))
		{
			USER_QUEST_COMPLETE_INFO uSER_QUEST_COMPLETE_INFO = this.m_UserQuestInfo.m_dicCompleteInfo[unique];
			if (uSER_QUEST_COMPLETE_INFO == null)
			{
				return false;
			}
			if (0 < uSER_QUEST_COMPLETE_INFO.bCleared)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsWorldFirst()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		return myCharInfo != null && myCharInfo.GetLevel() <= 1 && !NrTSingleton<NkQuestManager>.Instance.IsCompletedQuest("10101_005") && !NrTSingleton<NkQuestManager>.Instance.IsCurrentQuest("10101_005") && myCharInfo.m_kCharMapInfo.MapIndex == 2;
	}

	public bool IsCompletedFirstQuest()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		return myCharInfo != null && (3 <= myCharInfo.GetLevel() || (NrTSingleton<NkQuestManager>.Instance.IsCompletedQuest("10101_005") || NrTSingleton<NkQuestManager>.Instance.IsCurrentQuest("10101_005")) || myCharInfo.m_kCharMapInfo.MapIndex != 2);
	}

	public bool IsCompletedQuest(string strQuestUnique)
	{
		NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
		if (instance != null)
		{
			int groupIDByQuestUnique = instance.GetGroupIDByQuestUnique(strQuestUnique);
			if (groupIDByQuestUnique == -1)
			{
				return false;
			}
			int bitByQuestUnique = instance.GetBitByQuestUnique(strQuestUnique);
			if (bitByQuestUnique == -1)
			{
				return false;
			}
			if (this.m_UserQuestInfo.m_dicCompleteInfo.ContainsKey(groupIDByQuestUnique))
			{
				USER_QUEST_COMPLETE_INFO uSER_QUEST_COMPLETE_INFO = this.m_UserQuestInfo.m_dicCompleteInfo[groupIDByQuestUnique];
				short num = (short)(bitByQuestUnique / 8);
				short num2 = (short)(bitByQuestUnique % 8);
				if (1 << (int)num2 == ((int)uSER_QUEST_COMPLETE_INFO.byCompleteQuest[(int)num] & 1 << (int)num2))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void SetResetQuestGroup(CQuestGroup kGroup, int i32GroupUnique, byte[] group, int i32Grade, byte bCleard)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i32QuestGroupUnique == i32GroupUnique)
			{
				this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].Init();
			}
		}
		if (this.m_UserQuestInfo.m_dicCompleteInfo.ContainsKey(i32GroupUnique))
		{
			USER_QUEST_COMPLETE_INFO uSER_QUEST_COMPLETE_INFO = this.m_UserQuestInfo.m_dicCompleteInfo[i32GroupUnique];
			uSER_QUEST_COMPLETE_INFO.byCompleteQuest = group;
			uSER_QUEST_COMPLETE_INFO.bCurrentGrade = (byte)i32Grade;
			uSER_QUEST_COMPLETE_INFO.bCleared = bCleard;
		}
		else if (kGroup != null)
		{
			USER_QUEST_COMPLETE_INFO uSER_QUEST_COMPLETE_INFO2 = new USER_QUEST_COMPLETE_INFO();
			uSER_QUEST_COMPLETE_INFO2.i32GroupUnique = kGroup.GetGroupUnique();
			uSER_QUEST_COMPLETE_INFO2.byCompleteQuest = group;
			uSER_QUEST_COMPLETE_INFO2.i32LastGrade = 1;
			uSER_QUEST_COMPLETE_INFO2.bCurrentGrade = 1;
			uSER_QUEST_COMPLETE_INFO2.bCleared = bCleard;
			this.m_UserQuestInfo.m_dicCompleteInfo.Add(i32GroupUnique, uSER_QUEST_COMPLETE_INFO2);
		}
	}

	public void SetQuestGroup(CQuestGroup kGroup, int i32GroupUnique, byte[] group, int i32Grade)
	{
		if (this.m_UserQuestInfo.m_dicCompleteInfo.ContainsKey(i32GroupUnique))
		{
			USER_QUEST_COMPLETE_INFO uSER_QUEST_COMPLETE_INFO = this.m_UserQuestInfo.m_dicCompleteInfo[i32GroupUnique];
			uSER_QUEST_COMPLETE_INFO.byCompleteQuest = group;
			uSER_QUEST_COMPLETE_INFO.bCurrentGrade = (byte)i32Grade;
		}
		else if (kGroup != null)
		{
			USER_QUEST_COMPLETE_INFO uSER_QUEST_COMPLETE_INFO2 = new USER_QUEST_COMPLETE_INFO();
			uSER_QUEST_COMPLETE_INFO2.i32GroupUnique = kGroup.GetGroupUnique();
			uSER_QUEST_COMPLETE_INFO2.byCompleteQuest = group;
			uSER_QUEST_COMPLETE_INFO2.i32LastGrade = 1;
			uSER_QUEST_COMPLETE_INFO2.bCurrentGrade = 1;
			this.m_UserQuestInfo.m_dicCompleteInfo.Add(i32GroupUnique, uSER_QUEST_COMPLETE_INFO2);
		}
	}

	public USER_QUEST_COMPLETE_INFO GetCompleteQuestInfo(int i32GroupUnique)
	{
		if (this.m_UserQuestInfo.m_dicCompleteInfo.ContainsKey(i32GroupUnique))
		{
			return this.m_UserQuestInfo.m_dicCompleteInfo[i32GroupUnique];
		}
		return null;
	}

	public bool IsCurrentQuest(string strQuestUnique)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique != null)
			{
				if (this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == strQuestUnique)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool AcceptQuest(long i64QuestID, int i32QuestGroupUnique, string strQuestUnique, int i32Grade, long i64Time, long i64LastTime)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty)
			{
				NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
				if (instance != null)
				{
					instance.SetUserCurrentQuest(i64QuestID, i32QuestGroupUnique, strQuestUnique, ref this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b]);
					this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i32Grade = i32Grade;
					this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i64QuestTime = i64Time;
					this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i64LastTime = i64LastTime;
					this.SetCheckParamInfo(b, this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b]);
					this.SortCurrentQuest();
				}
				break;
			}
		}
		return true;
	}

	public string GetLastAcceptQuestUnique()
	{
		long num = 0L;
		int num2 = -1;
		for (int i = 0; i < 10; i++)
		{
			if (num < this.m_UserQuestInfo.stUserCurrentQuestInfo[i].i64QuestID)
			{
				num = this.m_UserQuestInfo.stUserCurrentQuestInfo[i].i64QuestID;
				num2 = i;
			}
		}
		if (num2 >= 0)
		{
			return this.m_UserQuestInfo.stUserCurrentQuestInfo[num2].strQuestUnique;
		}
		return string.Empty;
	}

	public void SortCurrentQuest()
	{
		this.m_UserQuestInfo.mainList.Clear();
		this.m_UserQuestInfo.subList.Clear();
		for (int i = 0; i < 10; i++)
		{
			CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[i].strQuestUnique);
			if (questByQuestUnique != null)
			{
				CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(questByQuestUnique.GetQuestGroupUnique());
				if (questGroupByGroupUnique != null)
				{
					if (questGroupByGroupUnique.GetQuestType() == 1)
					{
						this.m_UserQuestInfo.mainList.Add(this.m_UserQuestInfo.stUserCurrentQuestInfo[i]);
					}
					else
					{
						this.m_UserQuestInfo.subList.Add(this.m_UserQuestInfo.stUserCurrentQuestInfo[i]);
					}
				}
			}
		}
		this.m_UserQuestInfo.mainList.Sort(new Comparison<USER_CURRENT_QUEST_INFO>(this.AscendingQuestID));
		this.m_UserQuestInfo.subList.Sort(new Comparison<USER_CURRENT_QUEST_INFO>(this.AscendingQuestID));
	}

	private int AscendingQuestID(USER_CURRENT_QUEST_INFO x, USER_CURRENT_QUEST_INFO y)
	{
		if (x.i64QuestID <= y.i64QuestID)
		{
			return 1;
		}
		return -1;
	}

	public List<USER_CURRENT_QUEST_INFO> GetMainlist()
	{
		return this.m_UserQuestInfo.mainList;
	}

	public List<USER_CURRENT_QUEST_INFO> GetSublist()
	{
		return this.m_UserQuestInfo.subList;
	}

	public int GetCurrentQuestCount()
	{
		int num = 0;
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				if (NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique) != null)
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetCurrentMainQuestCount()
	{
		int num = 0;
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i32QuestGroupUnique);
				if (questGroupByGroupUnique != null)
				{
					if (questGroupByGroupUnique.GetQuestType() == 1)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public int GetCurrentSubQuestCount()
	{
		int num = 0;
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i32QuestGroupUnique);
				if (questGroupByGroupUnique != null)
				{
					if (questGroupByGroupUnique.GetQuestType() == 2)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public int GetCurrentDayQuestCount()
	{
		int num = 0;
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i32QuestGroupUnique);
				if (questGroupByGroupUnique != null)
				{
					if (questGroupByGroupUnique.GetQuestType() == 100)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public void SetQuestCompleteInfo(int i32GroupUnique, byte[] byCompleteQuest)
	{
		if (this.m_UserQuestInfo.m_dicCompleteInfo.ContainsKey(i32GroupUnique))
		{
			USER_QUEST_COMPLETE_INFO uSER_QUEST_COMPLETE_INFO = this.m_UserQuestInfo.m_dicCompleteInfo[i32GroupUnique];
			uSER_QUEST_COMPLETE_INFO.byCompleteQuest = byCompleteQuest;
		}
	}

	public void CompleteQuest(string strQuestUnique, int i32GroupUnique, byte[] byCompleteQuest, byte allClear, int i32Grade, int i32CurGrade)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				if (this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == strQuestUnique)
				{
					this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].Init();
					this.m_ParamNyf[(int)b].Init();
				}
			}
		}
		if (this.m_UserQuestInfo.m_dicCompleteInfo.ContainsKey(i32GroupUnique))
		{
			USER_QUEST_COMPLETE_INFO uSER_QUEST_COMPLETE_INFO = this.m_UserQuestInfo.m_dicCompleteInfo[i32GroupUnique];
			uSER_QUEST_COMPLETE_INFO.byCompleteQuest = byCompleteQuest;
			uSER_QUEST_COMPLETE_INFO.bCleared = allClear;
			uSER_QUEST_COMPLETE_INFO.i32LastGrade = i32Grade;
			uSER_QUEST_COMPLETE_INFO.bCurrentGrade = (byte)i32CurGrade;
		}
		else
		{
			USER_QUEST_COMPLETE_INFO uSER_QUEST_COMPLETE_INFO2 = new USER_QUEST_COMPLETE_INFO();
			uSER_QUEST_COMPLETE_INFO2.i32GroupUnique = i32GroupUnique;
			uSER_QUEST_COMPLETE_INFO2.byCompleteQuest = byCompleteQuest;
			uSER_QUEST_COMPLETE_INFO2.bCleared = allClear;
			uSER_QUEST_COMPLETE_INFO2.i32LastGrade = i32Grade;
			uSER_QUEST_COMPLETE_INFO2.bCurrentGrade = (byte)i32CurGrade;
			this.m_UserQuestInfo.m_dicCompleteInfo.Add(i32GroupUnique, uSER_QUEST_COMPLETE_INFO2);
		}
	}

	public void UpdateQuestParamVal(string strQuestUnique, QUEST_CONDITION[] cQuestCondition)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				if (strQuestUnique == this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique)
				{
					this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i64ParamVal[0] = cQuestCondition[0].i64ParamVal;
					this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i64ParamVal[1] = cQuestCondition[1].i64ParamVal;
					this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i64ParamVal[2] = cQuestCondition[2].i64ParamVal;
					break;
				}
			}
		}
	}

	public void CancleQuest(string strQuestUnique)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				if (strQuestUnique == this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique)
				{
					this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].Init();
					this.m_ParamNyf[(int)b].Init();
					break;
				}
			}
		}
	}

	public void SetUserCurrentQuestInfo(USER_CURRENT_QUEST_INFO_PACKET[] UserCurrentQeustInfo)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].SetUserCurrentQuestInfo(UserCurrentQeustInfo[(int)b]);
		}
	}

	public USER_CURRENT_QUEST_INFO[] GetUserCurrentQuestInfo()
	{
		return this.m_UserQuestInfo.stUserCurrentQuestInfo;
	}

	public bool CheckQuestComplete(string strQuestUnique, USER_CURRENT_QUEST_INFO cUserCurrentQuestInfo)
	{
		NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
		return instance != null && instance.CheckQuestResult(strQuestUnique, cUserCurrentQuestInfo);
	}

	public USER_CURRENT_QUEST_INFO GetCurrentMainQuest()
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				CQuestGroup questGroupByQuestUnique = this.GetQuestGroupByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique);
				if (questGroupByQuestUnique != null)
				{
					if (questGroupByQuestUnique.GetQuestType() == 1)
					{
						return this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b];
					}
				}
			}
		}
		return null;
	}

	public USER_CURRENT_QUEST_INFO GetCurrentSubQuest()
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				CQuestGroup questGroupByQuestUnique = this.GetQuestGroupByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique);
				if (questGroupByQuestUnique != null)
				{
					if (questGroupByQuestUnique.GetQuestType() == 2)
					{
						return this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b];
					}
				}
			}
		}
		return null;
	}

	public USER_CURRENT_QUEST_INFO GetCurrentDayQuest()
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				CQuestGroup questGroupByQuestUnique = this.GetQuestGroupByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique);
				if (questGroupByQuestUnique != null)
				{
					if (questGroupByQuestUnique.GetQuestType() == 100)
					{
						return this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b];
					}
				}
			}
		}
		return null;
	}

	public USER_CURRENT_QUEST_INFO GetCurrentQuest(string strQuestUnique)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				if (this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == strQuestUnique)
				{
					return this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b];
				}
			}
		}
		return null;
	}

	public USER_CURRENT_QUEST_INFO GetCurrentQuest(QUEST_CONST.eQUESTCODE questCode)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique);
				if (questByQuestUnique != null)
				{
					if (questByQuestUnique.IsCondition(questCode))
					{
						return this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b];
					}
				}
			}
		}
		return null;
	}

	public QUEST_CONST.eQUESTSTATE GetQuestState(string strQuestUnique)
	{
		QUEST_CONST.eQUESTSTATE result = QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE;
		NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
		if (instance == null)
		{
			return result;
		}
		if (string.Empty != strQuestUnique)
		{
			CQuest questByQuestUnique = instance.GetQuestByQuestUnique(strQuestUnique);
			if (this.IsCompletedQuest(strQuestUnique))
			{
				result = QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE;
			}
			else if (this.IsCurrentQuest(strQuestUnique))
			{
				USER_CURRENT_QUEST_INFO currentQuest = this.GetCurrentQuest(strQuestUnique);
				if (currentQuest.bFailed == 0)
				{
					if (!instance.CheckQuestResult(strQuestUnique, currentQuest))
					{
						result = QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING;
					}
					else
					{
						result = QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE;
					}
				}
				else
				{
					result = QUEST_CONST.eQUESTSTATE.QUESTSTATE_FAIL;
				}
			}
			else if (!instance.PreQuestChek(strQuestUnique))
			{
				result = QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_NOT_VIEW;
			}
			else if (!instance.PreCheckQuestAccept(strQuestUnique))
			{
				result = QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_VIEW;
			}
			else if (instance.PreCheckQuestAccept(strQuestUnique))
			{
				if (questByQuestUnique != null)
				{
					NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
					if (nrCharUser == null)
					{
						return result;
					}
					if (0 < questByQuestUnique.GetQuestCommon().nLimitLevel && nrCharUser.GetPersonInfo().GetLevel(0L) > (int)questByQuestUnique.GetQuestCommon().nLimitLevel)
					{
						return QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_NOT_VIEW;
					}
					if ((short)nrCharUser.GetPersonInfo().GetLevel(0L) < questByQuestUnique.GetQuestCommon().i16RequireLevel[0])
					{
						return QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_NOT_VIEW;
					}
					result = QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE;
					if (!NrTSingleton<ContentsLimitManager>.Instance.IsQuestAccept(questByQuestUnique.GetQuestGroupUnique()))
					{
						return QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_NOT_VIEW;
					}
				}
			}
			else
			{
				result = QUEST_CONST.eQUESTSTATE.QUESTSTATE_END;
			}
		}
		return result;
	}

	public void IncreaseQuestParamVal(int i32QuestCode, long i64Param, long i64ParamVal)
	{
		if (0 >= i32QuestCode)
		{
			return;
		}
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[i].strQuestUnique == string.Empty))
				{
					CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[i].strQuestUnique);
					if (questByQuestUnique != null)
					{
						int i32QuestCode2 = questByQuestUnique.GetQuestCommon().cQuestCondition[j].i32QuestCode;
						if (i32QuestCode2 == i32QuestCode)
						{
							long i64Param2 = questByQuestUnique.GetQuestCommon().cQuestCondition[j].i64Param;
							if (i64Param2 == i64Param && !questByQuestUnique.IsServerCheck(j) && this.m_UserQuestInfo.stUserCurrentQuestInfo[i].i64ParamVal[j] < questByQuestUnique.GetQuestCommon().cQuestCondition[j].i64ParamVal)
							{
								GS_QUEST_UPDATE_PARAMVAL_REQ gS_QUEST_UPDATE_PARAMVAL_REQ = new GS_QUEST_UPDATE_PARAMVAL_REQ();
								gS_QUEST_UPDATE_PARAMVAL_REQ.i32QuestCode = i32QuestCode;
								gS_QUEST_UPDATE_PARAMVAL_REQ.i64Param = i64Param;
								SendPacket.GetInstance().SendObject(1011, gS_QUEST_UPDATE_PARAMVAL_REQ);
								if (0L < i64ParamVal)
								{
									this.m_UserQuestInfo.stUserCurrentQuestInfo[i].i64ParamVal[j] += i64ParamVal;
								}
							}
						}
					}
				}
			}
		}
	}

	public QUEST_CONST.E_QUEST_GROUP_STATE QuestGroupClearCheck(int questGroupUnique)
	{
		QUEST_CONST.E_QUEST_GROUP_STATE result = QUEST_CONST.E_QUEST_GROUP_STATE.E_QUEST_GROUP_STATE_EVEN;
		CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(questGroupUnique);
		bool flag = false;
		bool flag2 = true;
		bool flag3 = false;
		if (questGroupByGroupUnique != null)
		{
			for (int i = 0; i < 200; i++)
			{
				string questUniqueByBit = questGroupByGroupUnique.GetQuestUniqueByBit(i);
				if (!questUniqueByBit.Equals("0"))
				{
					QUEST_CONST.eQUESTSTATE questState = this.GetQuestState(questUniqueByBit);
					if (QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING <= questState)
					{
						flag3 = true;
					}
					if (questState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE)
					{
						flag2 = false;
					}
					if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE)
					{
						flag = true;
					}
				}
			}
		}
		if (flag2)
		{
			result = QUEST_CONST.E_QUEST_GROUP_STATE.E_QUEST_GROUP_STATE_NONE;
		}
		else if (!flag && !flag3)
		{
			result = QUEST_CONST.E_QUEST_GROUP_STATE.E_QUEST_GROUP_STATE_EVEN;
		}
		else if (flag3)
		{
			result = QUEST_CONST.E_QUEST_GROUP_STATE.E_QUEST_GROUP_STATE_ONGONIG;
		}
		return result;
	}

	public void DelClientNpcList(int i32NpcID)
	{
		for (int i = 0; i < this.m_ClientNpcList.Count; i++)
		{
			NrQuestNpcCheckInfo nrQuestNpcCheckInfo = this.m_ClientNpcList[i];
			for (int j = 0; j < nrQuestNpcCheckInfo.kList.Count; j++)
			{
				NrQuestNpcCheck nrQuestNpcCheck = nrQuestNpcCheckInfo.kList[j];
				if (nrQuestNpcCheck.nCharID == i32NpcID)
				{
					NrTSingleton<NkCharManager>.Instance.DeleteChar(nrQuestNpcCheck.nCharID);
					nrQuestNpcCheckInfo.kList.Remove(nrQuestNpcCheck);
					if (nrQuestNpcCheckInfo.kList.Count <= 0)
					{
						this.m_ClientNpcList.Remove(nrQuestNpcCheckInfo);
					}
					break;
				}
			}
		}
	}

	public short GetClientNpcUnique()
	{
		for (short num = 31300; num <= 31400; num += 1)
		{
			if (NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(num) == null)
			{
				return num;
			}
		}
		return 0;
	}

	public void UpdateClientNpc(int mapIndex)
	{
		if (Scene.CurScene != Scene.Type.WORLD)
		{
			return;
		}
		if (mapIndex == 0)
		{
			mapIndex = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo.m_nMapIndex;
		}
		for (int i = 0; i < this.m_ClientNpcList.Count; i++)
		{
			NrQuestNpcCheckInfo nrQuestNpcCheckInfo = this.m_ClientNpcList[i];
			for (int j = 0; j < nrQuestNpcCheckInfo.kList.Count; j++)
			{
				NrQuestNpcCheck nrQuestNpcCheck = nrQuestNpcCheckInfo.kList[j];
				if (!this.ClinetNpcCreateCheck(nrQuestNpcCheck.kStartCon, nrQuestNpcCheck.kEndCon))
				{
					NrTSingleton<NkCharManager>.Instance.DeleteChar(nrQuestNpcCheck.nCharID);
					nrQuestNpcCheckInfo.kList.Remove(nrQuestNpcCheck);
					j--;
					if (nrQuestNpcCheckInfo.kList.Count <= 0)
					{
						this.m_ClientNpcList.Remove(nrQuestNpcCheckInfo);
						i--;
					}
					break;
				}
			}
		}
		NrClientNpcPosList clientNpcPosList = NrTSingleton<NkQuestManager>.Instance.GetClientNpcPosList(mapIndex);
		if (clientNpcPosList != null)
		{
			short num = 0;
			while ((int)num < clientNpcPosList.ClientNpcPosList.Count)
			{
				NrClientNpcInfo nrClientNpcInfo = clientNpcPosList.ClientNpcPosList[(int)num];
				if (nrClientNpcInfo != null && nrClientNpcInfo.i32MapIndex == mapIndex && this.ClinetNpcCreateCheck(nrClientNpcInfo.kStartCon, nrClientNpcInfo.kEndCon))
				{
					NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(nrClientNpcInfo.strCharCode);
					if (charKindInfoFromCode != null)
					{
						bool flag = false;
						string @string = NrTSingleton<UIDataManager>.Instance.GetString(nrClientNpcInfo.strCharCode, ((int)nrClientNpcInfo.fFixPosX).ToString(), ((int)nrClientNpcInfo.fFixPosY).ToString());
						for (int k = 0; k < this.m_ClientNpcList.Count; k++)
						{
							NrQuestNpcCheckInfo nrQuestNpcCheckInfo2 = this.m_ClientNpcList[k];
							if (nrQuestNpcCheckInfo2.charCode == @string && nrQuestNpcCheckInfo2.kList.Count >= nrClientNpcInfo.i32Count)
							{
								flag = true;
								break;
							}
						}
						if (!flag && charKindInfoFromCode != null)
						{
							NEW_MAKECHAR_INFO nEW_MAKECHAR_INFO = new NEW_MAKECHAR_INFO();
							nEW_MAKECHAR_INFO.CharName = TKString.StringChar(charKindInfoFromCode.GetName());
							nEW_MAKECHAR_INFO.CharPos.x = nrClientNpcInfo.fFixPosX;
							nEW_MAKECHAR_INFO.CharPos.y = 0f;
							nEW_MAKECHAR_INFO.CharPos.z = nrClientNpcInfo.fFixPosY;
							float f = nrClientNpcInfo.i16FixPosA * 0.0174532924f;
							nEW_MAKECHAR_INFO.Direction.x = 1f * Mathf.Sin(f);
							nEW_MAKECHAR_INFO.Direction.y = 0f;
							nEW_MAKECHAR_INFO.Direction.z = 1f * Mathf.Cos(f);
							nEW_MAKECHAR_INFO.CharKind = charKindInfoFromCode.GetCharKind();
							nEW_MAKECHAR_INFO.CharKindType = 3;
							nEW_MAKECHAR_INFO.CharUnique = this.GetClientNpcUnique();
							if (nEW_MAKECHAR_INFO.CharUnique == 0)
							{
								GS_QUEST_INFO gS_QUEST_INFO = new GS_QUEST_INFO();
								gS_QUEST_INFO.nType = 0;
								TKString.StringChar(string.Concat(new object[]
								{
									"1_",
									nEW_MAKECHAR_INFO.CharName,
									"_",
									nrClientNpcInfo.kStartCon.strQuestUnique
								}), ref gS_QUEST_INFO.strInfo);
								SendPacket.GetInstance().SendObject(1003, gS_QUEST_INFO);
							}
							int nCharID = NrTSingleton<NkCharManager>.Instance.SetChar(nEW_MAKECHAR_INFO, false, false);
							bool flag2 = false;
							NrQuestNpcCheck nrQuestNpcCheck2 = new NrQuestNpcCheck();
							nrQuestNpcCheck2.nCharUnique = nEW_MAKECHAR_INFO.CharUnique;
							nrQuestNpcCheck2.nCharID = nCharID;
							nrQuestNpcCheck2.kEndCon = nrClientNpcInfo.kEndCon;
							nrQuestNpcCheck2.kStartCon = nrClientNpcInfo.kStartCon;
							for (int l = 0; l < this.m_ClientNpcList.Count; l++)
							{
								NrQuestNpcCheckInfo nrQuestNpcCheckInfo3 = this.m_ClientNpcList[l];
								if (nrQuestNpcCheckInfo3.charCode == nrClientNpcInfo.strCharCode)
								{
									nrQuestNpcCheckInfo3.kList.Add(nrQuestNpcCheck2);
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								NrQuestNpcCheckInfo nrQuestNpcCheckInfo3 = new NrQuestNpcCheckInfo();
								nrQuestNpcCheckInfo3.charCode = @string;
								nrQuestNpcCheckInfo3.kList.Add(nrQuestNpcCheck2);
								this.m_ClientNpcList.Add(nrQuestNpcCheckInfo3);
							}
							NrNpcPos nrNpcPos = new NrNpcPos();
							nrNpcPos.strKey = charKindInfoFromCode.GetCode() + nCharID.ToString();
							charKindInfoFromCode.SetPosKey(nrNpcPos.strKey);
							nrNpcPos.nMapIndex = mapIndex;
							nrNpcPos.nCharKind = nEW_MAKECHAR_INFO.CharKind;
							nrNpcPos.kPos = nEW_MAKECHAR_INFO.CharPos;
							nrNpcPos.strName = charKindInfoFromCode.GetName();
							NrTSingleton<NrNpcPosManager>.Instance.AddNpcPos(nrNpcPos);
						}
					}
				}
				num += 1;
			}
		}
	}

	public void ClearClientNpc()
	{
		for (int i = 0; i < this.m_ClientNpcList.Count; i++)
		{
			NrQuestNpcCheckInfo nrQuestNpcCheckInfo = this.m_ClientNpcList[i];
			for (int j = 0; j < nrQuestNpcCheckInfo.kList.Count; j++)
			{
				NrQuestNpcCheck nrQuestNpcCheck = nrQuestNpcCheckInfo.kList[j];
				NrTSingleton<NkCharManager>.Instance.DeleteChar(nrQuestNpcCheck.nCharID);
				nrQuestNpcCheckInfo.kList.Remove(nrQuestNpcCheck);
				j--;
				if (nrQuestNpcCheckInfo.kList.Count <= 0)
				{
					this.m_ClientNpcList.Remove(nrQuestNpcCheckInfo);
					i--;
				}
			}
		}
		this.m_ClientNpcList.Clear();
	}

	private int SetSubCharCount()
	{
		int num = 0;
		NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (nrCharUser == null)
		{
			return 0;
		}
		for (int i = 0; i < 10; i++)
		{
			if (nrCharUser.GetSubChsrKind(i) > 0)
			{
				num++;
			}
		}
		return num;
	}

	public bool ClinetNpcCreateCheck(NrNpcClientCondition startCon, NrNpcClientCondition endCon)
	{
		CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(endCon.strQuestUnique);
		if (questByQuestUnique == null)
		{
			return false;
		}
		QUEST_CONST.eQUESTSTATE questState = this.GetQuestState(startCon.strQuestUnique);
		if (questState < startCon.eQuestCase)
		{
			return false;
		}
		questState = this.GetQuestState(endCon.strQuestUnique);
		if (questState < endCon.eQuestCase)
		{
			return true;
		}
		if (startCon.strQuestUnique.Equals(endCon.strQuestUnique))
		{
			return false;
		}
		int num = 0;
		while (true)
		{
			questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(questByQuestUnique.GetQuestCommon().strPreQuestUnique);
			if (questByQuestUnique == null)
			{
				break;
			}
			questState = this.GetQuestState(questByQuestUnique.GetQuestUnique());
			if (questState < QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE)
			{
				goto Block_6;
			}
			if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE && startCon.strQuestUnique.Equals(questByQuestUnique.GetQuestUnique()))
			{
				return false;
			}
			num++;
			if (num >= 100)
			{
				return false;
			}
		}
		return false;
		Block_6:
		return !startCon.strQuestUnique.Equals(questByQuestUnique.GetQuestUnique()) || questState >= startCon.eQuestCase;
	}

	public void UpdateMonQuestMessage(long i64CharKind)
	{
		for (int i = 0; i < this.m_ParamNfyTempQueue.Count; i++)
		{
			NrQuestParamNfyTemp nrQuestParamNfyTemp = this.m_ParamNfyTempQueue[i];
			if (nrQuestParamNfyTemp != null)
			{
				if (nrQuestParamNfyTemp.i64Unique == i64CharKind && !nrQuestParamNfyTemp.bCheck && (nrQuestParamNfyTemp.i32Code == 36 || nrQuestParamNfyTemp.i32Code == 116 || nrQuestParamNfyTemp.i32Code == 101 || nrQuestParamNfyTemp.i32Code == 105 || nrQuestParamNfyTemp.i32Code == 165 || nrQuestParamNfyTemp.i32Code == 166))
				{
					this.Update(nrQuestParamNfyTemp.strQuestUnique, nrQuestParamNfyTemp.stCondition, nrQuestParamNfyTemp.i64Unique);
					nrQuestParamNfyTemp.bCheck = true;
					break;
				}
			}
		}
	}

	public void UpdateItemQuestMessage(long nItemUnique)
	{
		for (int i = 0; i < this.m_ParamNfyTempQueue.Count; i++)
		{
			NrQuestParamNfyTemp nrQuestParamNfyTemp = this.m_ParamNfyTempQueue[i];
			if (nrQuestParamNfyTemp != null)
			{
				if (!nrQuestParamNfyTemp.bCheck && nrQuestParamNfyTemp.i64Unique == nItemUnique && (nrQuestParamNfyTemp.i32Code == 7 || nrQuestParamNfyTemp.i32Code == 43))
				{
					this.Update(nrQuestParamNfyTemp.strQuestUnique, nrQuestParamNfyTemp.stCondition, nrQuestParamNfyTemp.i64Unique);
					nrQuestParamNfyTemp.bCheck = true;
					break;
				}
			}
		}
	}

	public void UpdateVictoryQuestMessage()
	{
		for (int i = 0; i < this.m_ParamNfyTempQueue.Count; i++)
		{
			NrQuestParamNfyTemp nrQuestParamNfyTemp = this.m_ParamNfyTempQueue[i];
			if (nrQuestParamNfyTemp != null)
			{
				if (nrQuestParamNfyTemp.i32Code == 14 || nrQuestParamNfyTemp.i32Code == 101 || nrQuestParamNfyTemp.i32Code == 117 || nrQuestParamNfyTemp.i32Code == 125 || nrQuestParamNfyTemp.i32Code == 155)
				{
					this.Update(nrQuestParamNfyTemp.strQuestUnique, nrQuestParamNfyTemp.stCondition, nrQuestParamNfyTemp.i64Unique);
					nrQuestParamNfyTemp.bCheck = true;
					break;
				}
			}
		}
	}

	public void ClearQuestMessage()
	{
		for (int i = 0; i < this.m_ParamNfyTempQueue.Count; i++)
		{
			NrQuestParamNfyTemp nrQuestParamNfyTemp = this.m_ParamNfyTempQueue[i];
			if (nrQuestParamNfyTemp != null)
			{
				if (!nrQuestParamNfyTemp.bCheck)
				{
					this.Update(nrQuestParamNfyTemp.strQuestUnique, nrQuestParamNfyTemp.stCondition, nrQuestParamNfyTemp.i64Unique);
					nrQuestParamNfyTemp.bCheck = true;
				}
			}
		}
		this.m_ParamNfyTempQueue.Clear();
	}

	public void Update(string strQuestUnique, QUEST_CONDITION[] kCondition, long i64Unique)
	{
		this.UpdateQuestConNfy(strQuestUnique, kCondition);
		RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_QUEST);
		if (rightMenuQuestUI != null)
		{
			rightMenuQuestUI.QuestUpdate();
		}
	}

	public void UpdateQuestConNfy(string strQuestUnique, QUEST_CONDITION[] kCondition)
	{
		CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
		if (questByQuestUnique == null)
		{
			return;
		}
		byte b;
		for (b = 0; b < 10; b += 1)
		{
			if (this.m_ParamNyf[(int)b].strQuestUnique == strQuestUnique)
			{
				break;
			}
		}
		if (b >= 10)
		{
			b = 9;
		}
		if (questByQuestUnique != null)
		{
			for (int i = 0; i < 3; i++)
			{
				if (0 < this.m_ParamNyf[(int)b].kQuestUpdateInfo[i].i32QuestCode)
				{
					bool flag = questByQuestUnique.CheckCondition(this.m_ParamNyf[(int)b].kQuestUpdateInfo[i].i64Param, ref kCondition[i].i64ParamVal, i);
					if (this.m_ParamNyf[(int)b].kQuestUpdateInfo[i].i64PreParamVal != kCondition[i].i64ParamVal)
					{
						if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_RESULT_DLG) && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NPCTALK_DLG))
						{
							string conditionText = questByQuestUnique.GetConditionText(kCondition[i].i64ParamVal, i);
							if (string.Empty != conditionText)
							{
								Main_UI_SystemMessage.ADDMessage(conditionText, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
							}
						}
						if (flag)
						{
							NrTSingleton<NkCharManager>.Instance.DeleteQuestMonsterEffect();
						}
						this.m_ParamNyf[(int)b].kQuestUpdateInfo[i].i64PreParamVal = kCondition[i].i64ParamVal;
					}
				}
			}
		}
	}

	public bool IsNPCCharLoaded()
	{
		if (this.m_kQuestDlgInfo != null)
		{
			NrCharNPC nrCharNPC = (NrCharNPC)NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(this.m_kQuestDlgInfo.i16CharUnique);
			if (nrCharNPC == null)
			{
				if (this.m_ClientNpcList.Count <= 0)
				{
					return true;
				}
				for (int i = 0; i < this.m_ClientNpcList.Count; i++)
				{
					NrQuestNpcCheckInfo nrQuestNpcCheckInfo = this.m_ClientNpcList[i];
					if (nrQuestNpcCheckInfo.charCode == this.m_kQuestDlgInfo.strCharCode)
					{
						bool result = false;
						for (int j = 0; j < nrQuestNpcCheckInfo.kList.Count; j++)
						{
							NrQuestNpcCheck nrQuestNpcCheck = nrQuestNpcCheckInfo.kList[j];
							nrCharNPC = (NrCharNPC)NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(nrQuestNpcCheck.nCharUnique);
							if (nrCharNPC == null || !nrCharNPC.IsGround())
							{
								result = false;
								break;
							}
							this.m_kQuestDlgInfo.i16CharUnique = nrQuestNpcCheck.nCharUnique;
							result = true;
						}
						return result;
					}
				}
				if (this.m_kQuestDlgInfo.i16CharUnique == 0)
				{
					return true;
				}
			}
			else if (nrCharNPC != null && nrCharNPC.IsGround())
			{
				Debug.LogError(nrCharNPC.Get3DChar().GetRootGameObject().transform.position);
				return true;
			}
		}
		return false;
	}

	public void UpdateEventCheck()
	{
		if (this.m_bEventDramaOff)
		{
			if (Scene.CurScene != Scene.Type.WORLD)
			{
				if (this.m_bIsProlog)
				{
					WS_CONNECT_GAMESERVER_REQ obj = new WS_CONNECT_GAMESERVER_REQ();
					SendPacket.GetInstance().SendObject(16777261, obj);
					this.m_bIsProlog = false;
				}
			}
		}
		if (Scene.CurScene == Scene.Type.WORLD && CommonTasks.IsEndOfPrework && 0 < this.m_nCharKind && string.Empty != this.m_szQuestUnique)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null && @char.m_e3DCharStep == NrCharBase.e3DCharStep.CHARACTION)
			{
				NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NPCTALK_DLG);
				if (npcTalkUI_DLG != null)
				{
					npcTalkUI_DLG.SetAutoQuest(true);
					npcTalkUI_DLG.SetNpcID(this.m_nCharKind, 0);
					npcTalkUI_DLG.AutoClickButton(this.m_szQuestUnique);
					npcTalkUI_DLG.Show();
					this.m_nCharKind = 0;
					this.m_szQuestUnique = string.Empty;
				}
			}
		}
	}

	public void SetQuestDlgInfo(NrQuestDlgCurrentInfo info)
	{
		this.m_kQuestDlgInfo = info;
	}

	public NrQuestDlgCurrentInfo GetTempInfo()
	{
		return this.m_kQuestDlgInfo;
	}

	public void PushMessage(string strMsg, SYSTEM_MESSAGE_TYPE type)
	{
	}

	public void ShowMsg()
	{
	}

	public void AddDebugMsg(string strMsg)
	{
		if (this.m_DebugMsgQueue.Count >= 30)
		{
			this.m_DebugMsgQueue.Dequeue();
		}
		this.m_DebugMsgQueue.Enqueue(strMsg);
	}

	public string GetDebugMsg()
	{
		string text = string.Empty;
		while (this.m_DebugMsgQueue.Count > 0)
		{
			text += this.m_DebugMsgQueue.Dequeue();
			text += "\n";
		}
		return text;
	}

	public string IsCheckCodeANDParam(QUEST_CONST.eQUESTCODE eCode, long i64Param)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique != string.Empty)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique != null)
					{
						CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique);
						if (questByQuestUnique != null)
						{
							if (questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode == (int)eCode && questByQuestUnique.GetQuestCommon().cQuestCondition[i].i64Param == i64Param)
							{
								if (questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode == (int)eCode)
								{
									USER_CURRENT_QUEST_INFO currentQuest = this.GetCurrentQuest(questByQuestUnique.GetQuestUnique());
									if (currentQuest != null && questByQuestUnique.CheckCondition(i64Param, ref currentQuest.i64ParamVal[i], i))
									{
										return string.Empty;
									}
								}
								return questByQuestUnique.GetQuestUnique();
							}
						}
					}
				}
			}
		}
		return string.Empty;
	}

	public bool CheckQuestCode(QUEST_CONST.eQUESTCODE eCode)
	{
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (!(string.Empty == this.m_UserQuestInfo.stUserCurrentQuestInfo[i].strQuestUnique))
				{
					CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[i].strQuestUnique);
					if (questByQuestUnique != null)
					{
						int i32QuestCode = questByQuestUnique.GetQuestCommon().cQuestCondition[j].i32QuestCode;
						if (i32QuestCode == (int)eCode)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public bool CheckClickObjectChar(int kind)
	{
		return NrTSingleton<NkQuestManager>.Instance.IsQuestMonster(kind);
	}

	public bool CheckQuestCode(QUEST_CONST.eQUESTCODE eCode, int checkValue)
	{
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (!(string.Empty == this.m_UserQuestInfo.stUserCurrentQuestInfo[i].strQuestUnique))
				{
					CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[i].strQuestUnique);
					if (questByQuestUnique != null)
					{
						int i32QuestCode = questByQuestUnique.GetQuestCommon().cQuestCondition[j].i32QuestCode;
						int num = (int)questByQuestUnique.GetQuestCommon().cQuestCondition[j].i64Param;
						if (i32QuestCode == (int)eCode && checkValue == num)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public bool IsQuestMonster(int i32CharKind)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
		if (charKindInfo == null)
		{
			return false;
		}
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				string strQuestUnique = this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique;
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
				if (questByQuestUnique != null)
				{
					for (int i = 0; i < 3; i++)
					{
						long i64Param = questByQuestUnique.GetQuestCommon().cQuestCondition[i].i64Param;
						int i32QuestCode = questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode;
						if (i32QuestCode == 7 || i32QuestCode == 43 || i32QuestCode == 108)
						{
							QUEST_DROP_ITEM_List questDropItemList = NrTSingleton<NkQuestManager>.Instance.GetQuestDropItemList(strQuestUnique);
							if (questDropItemList != null)
							{
								foreach (QUEST_DROP_ITEM current in questDropItemList.dropItemList)
								{
									if (current.nItemUnique == i64Param && current.strCharCode.Equals(charKindInfo.GetCode()))
									{
										if (!questByQuestUnique.CheckCondition(questByQuestUnique.GetQuestCommon().cQuestCondition[i].i64Param, ref this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i64ParamVal[i], i))
										{
											return true;
										}
									}
								}
							}
						}
						else if ((i32QuestCode == 36 || i32QuestCode == 105 || i32QuestCode == 122 || i32QuestCode == 103 || i32QuestCode == 101 || i32QuestCode == 116 || i32QuestCode == 104 || i32QuestCode == 159 || i32QuestCode == 165 || i32QuestCode == 166) && i64Param == (long)i32CharKind)
						{
							if (!questByQuestUnique.CheckCondition(questByQuestUnique.GetQuestCommon().cQuestCondition[i].i64Param, ref this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].i64ParamVal[i], i))
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	public string[] Get_Current_Quest_Unique_List()
	{
		List<string> list = new List<string>();
		USER_CURRENT_QUEST_INFO[] userCurrentQuestInfo = this.GetUserCurrentQuestInfo();
		for (int i = 0; i < userCurrentQuestInfo.Length; i++)
		{
			if (userCurrentQuestInfo[i].strQuestUnique != string.Empty)
			{
				string strQuestUnique = userCurrentQuestInfo[i].strQuestUnique;
				if (this.GetQuestState(strQuestUnique) != QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
				{
					list.Add(strQuestUnique);
				}
			}
		}
		return list.ToArray();
	}

	public CQuest GetSubCharQuest()
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				string strQuestUnique = this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique;
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
				if (questByQuestUnique != null)
				{
					for (int i = 0; i < 3; i++)
					{
						int i32QuestCode = questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode;
						if (i32QuestCode == 99 || i32QuestCode == 103 || i32QuestCode == 96 || i32QuestCode == 122)
						{
							return questByQuestUnique;
						}
					}
				}
			}
		}
		return null;
	}

	public bool ChekcSellItemQuest()
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique);
				if (questByQuestUnique != null)
				{
					for (int i = 0; i < 3; i++)
					{
						int i32QuestCode = questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode;
						if (i32QuestCode == 115)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public string IsScenarioBattleQuestNpc(int i32CharKind)
	{
		for (byte b = 0; b < 10; b += 1)
		{
			if (!(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique == string.Empty))
			{
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique);
				if (questByQuestUnique != null)
				{
					if (questByQuestUnique.GetQuestCommon().i32CharKind == i32CharKind && this.GetQuestState(questByQuestUnique.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING)
					{
						return this.m_UserQuestInfo.stUserCurrentQuestInfo[(int)b].strQuestUnique;
					}
				}
			}
		}
		return string.Empty;
	}

	public int GetRewardCount()
	{
		return this.m_QuestRewardInfoQueue.Count;
	}

	public void PushRewardInfo(RewardInfo kInfo)
	{
		this.m_QuestRewardInfoQueue.Enqueue(kInfo);
	}

	public void SetQuestReward(CQuest kQuest, int i32Grade, int type)
	{
		if (i32Grade < 1 || i32Grade >= 5)
		{
			i32Grade = 1;
		}
		QEUST_REWARD_ITEM qEUST_REWARD_ITEM = kQuest.GetQuestCommon().cQuestRewardItem[i32Grade - 1];
		if (qEUST_REWARD_ITEM != null)
		{
			byte b = 0;
			RewardInfo item = new RewardInfo();
			if ((type & 2) > 0)
			{
				this.SetExp(qEUST_REWARD_ITEM.i64RewardExp, ref item, b);
				b += 1;
			}
			if ((type & 4) > 0)
			{
				this.SetMoney(qEUST_REWARD_ITEM.i64RewardMoney, ref item, b);
				b += 1;
			}
			if ((type & 8) > 0)
			{
				this.SetRepute(qEUST_REWARD_ITEM.nReputeUnique, qEUST_REWARD_ITEM.nReputeValue, ref item, b);
				b += 1;
			}
			if ((type & 16) > 0)
			{
				if (qEUST_REWARD_ITEM.nRewardItemUnique0 > 0)
				{
					this.SetItem(qEUST_REWARD_ITEM.nRewardItemUnique0, qEUST_REWARD_ITEM.nRewardItemNum0, ref item, b);
					b += 1;
				}
				if (qEUST_REWARD_ITEM.nRewardItemUnique1 > 0)
				{
					this.SetItem(qEUST_REWARD_ITEM.nRewardItemUnique1, qEUST_REWARD_ITEM.nRewardItemNum1, ref item, b);
					b += 1;
				}
				if (qEUST_REWARD_ITEM.nRewardItemUnique2 > 0)
				{
					this.SetItem(qEUST_REWARD_ITEM.nRewardItemUnique2, qEUST_REWARD_ITEM.nRewardItemNum2, ref item, b);
					b += 1;
				}
				if (qEUST_REWARD_ITEM.nRewardItemUnique3 > 0)
				{
					this.SetItem(qEUST_REWARD_ITEM.nRewardItemUnique3, qEUST_REWARD_ITEM.nRewardItemNum3, ref item, b);
					b += 1;
				}
				if (qEUST_REWARD_ITEM.nRewardItemUnique4 > 0)
				{
					this.SetItem(qEUST_REWARD_ITEM.nRewardItemUnique4, qEUST_REWARD_ITEM.nRewardItemNum4, ref item, b);
					b += 1;
				}
			}
			if ((type & 256) > 0)
			{
				this.SetItem(qEUST_REWARD_ITEM.nUpgradeReplaceItemUnique, qEUST_REWARD_ITEM.nUpgradeReplaceItemNum, ref item, b);
				b += 1;
			}
			if (0 < b)
			{
				this.m_QuestRewardInfoQueue.Enqueue(item);
			}
			if ((type & 64) > 0)
			{
				NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
				nkSoldierInfo.SetCharKind(qEUST_REWARD_ITEM.i32UpgradeGenCharKind);
				this.SetGeneralEnhance(nkSoldierInfo);
			}
			if ((type & 512) > 0)
			{
				QUEST_GROUP_REWARD questGroupReward = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupReward(kQuest.GetQuestGroupUnique());
				CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(kQuest.GetQuestGroupUnique());
				if (questGroupReward == null || questGroupByGroupUnique == null)
				{
					string msg = kQuest.GetQuestGroupUnique().ToString() + "의 장보상이 없습니다.";
					NrTSingleton<NrMainSystem>.Instance.Alert(msg);
				}
				this.SetGroupReward(new GS_QUEST_GROUP_REWARD_ACK
				{
					i32QuestGroupUnique = kQuest.GetQuestGroupUnique(),
					i32Grade = i32Grade,
					stItemInfo = 
					{
						m_nItemUnique = questGroupReward.nItemUnique[i32Grade - 1],
						m_nItemNum = (int)((short)questGroupReward.i32ItemNum[i32Grade - 1])
					}
				});
			}
		}
	}

	public void SetItem(int nItemUnique, int i16Num, ref RewardInfo rewardinfo, byte bNum)
	{
		if (3 <= bNum)
		{
			return;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(nItemUnique);
		if (itemInfo != null)
		{
			string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1107");
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1078");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"count",
				ANNUALIZED.Convert(i16Num)
			});
			rewardinfo.imgLoder[(int)bNum] = NrTSingleton<ItemManager>.Instance.GetItemTexture(nItemUnique);
			rewardinfo.str1[(int)bNum] = textColor + NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(nItemUnique);
			rewardinfo.str2[(int)bNum] = empty;
			rewardinfo.nItemUnique[(int)bNum] = nItemUnique;
			rewardinfo.bType[(int)bNum] = 16;
			if (nItemUnique == 70000)
			{
				NrTSingleton<FiveRocksEventManager>.Instance.HeartsInflow(eHEARTS_INFLOW.QUEST_REWARD, (long)i16Num);
			}
		}
	}

	public void SetMoney(long i64Money, ref RewardInfo rewardinfo, byte bNum)
	{
		if (3 <= bNum)
		{
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1119");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"count",
			ANNUALIZED.Convert(i64Money)
		});
		rewardinfo.imgLoder[(int)bNum] = NrTSingleton<ItemManager>.Instance.GetItemTexture(777777);
		rewardinfo.str1[(int)bNum] = empty;
		rewardinfo.str2[(int)bNum] = string.Empty;
		rewardinfo.bType[(int)bNum] = 4;
	}

	public void SetRepute(short nUnique, int nValue, ref RewardInfo rewardinfo, byte bNum)
	{
		if (3 <= bNum)
		{
			return;
		}
	}

	public void SetExp(long i64Exp, ref RewardInfo rewardinfo, byte bNum)
	{
		if (3 <= bNum)
		{
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("914");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"Count",
			ANNUALIZED.Convert(i64Exp)
		});
		rewardinfo.imgLoder[(int)bNum] = NrTSingleton<ItemManager>.Instance.GetItemTexture(777777);
		rewardinfo.str1[(int)bNum] = empty;
		rewardinfo.str2[(int)bNum] = string.Empty;
		rewardinfo.bType[(int)bNum] = 2;
	}

	public void SetSolInfo(SOLDIER_INFO kInfo)
	{
		RewardInfo rewardInfo = new RewardInfo();
		rewardInfo.imgLoder[0] = null;
		rewardInfo.str1[0] = string.Empty;
		rewardInfo.str2[0] = string.Empty;
		rewardInfo.bType[0] = 3;
		rewardInfo.kInfo[0] = kInfo;
		this.m_QuestRewardInfoQueue.Enqueue(rewardInfo);
	}

	public void SetGeneralEnhance(NkSoldierInfo kInfo)
	{
		RewardInfo rewardInfo = new RewardInfo();
		rewardInfo.imgLoder[0] = null;
		rewardInfo.str1[0] = string.Empty;
		rewardInfo.str2[0] = string.Empty;
		rewardInfo.bType[0] = 4;
		rewardInfo.kInfo[0] = kInfo;
		this.m_QuestRewardInfoQueue.Enqueue(rewardInfo);
	}

	public void SetGroupReward(GS_QUEST_GROUP_REWARD_ACK kInfo)
	{
		RewardInfo rewardInfo = new RewardInfo();
		rewardInfo.imgLoder[0] = null;
		rewardInfo.str1[0] = string.Empty;
		rewardInfo.str2[0] = string.Empty;
		rewardInfo.bType[0] = 5;
		rewardInfo.kInfo[0] = kInfo;
		this.m_groupReward = rewardInfo;
	}

	public void OnClose(IUIObject obj)
	{
		Button button = obj as Button;
		if (null == button)
		{
			return;
		}
		if ((int)button.data == 181)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.QUEST_GROUP_REWARD);
			this.m_bRewardShow = false;
		}
		else
		{
			Form form = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)((int)button.data));
			if (form != null)
			{
				form.Hide();
			}
			this.m_bRewardShow = false;
			this.ShowReward();
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "ITEMGET", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public void ShowGroupReward(int i32GroupUnique)
	{
		if (this.m_groupReward.kInfo[0] == null)
		{
			GS_QUEST_GROUP_REWARD_ACK gS_QUEST_GROUP_REWARD_ACK = new GS_QUEST_GROUP_REWARD_ACK();
			QUEST_GROUP_REWARD questGroupReward = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupReward(i32GroupUnique);
			if (questGroupReward == null)
			{
				return;
			}
			USER_QUEST_COMPLETE_INFO completeQuestInfo = this.GetCompleteQuestInfo(i32GroupUnique);
			if (completeQuestInfo != null)
			{
				gS_QUEST_GROUP_REWARD_ACK.i32Grade = (int)completeQuestInfo.bCurrentGrade;
			}
			else
			{
				gS_QUEST_GROUP_REWARD_ACK.i32Grade = 1;
			}
			gS_QUEST_GROUP_REWARD_ACK.i32QuestGroupUnique = i32GroupUnique;
			gS_QUEST_GROUP_REWARD_ACK.stItemInfo.m_nItemUnique = questGroupReward.nItemUnique[gS_QUEST_GROUP_REWARD_ACK.i32Grade - 1];
			gS_QUEST_GROUP_REWARD_ACK.stItemInfo.m_nItemNum = (int)((short)questGroupReward.i32ItemNum[gS_QUEST_GROUP_REWARD_ACK.i32Grade - 1]);
			this.m_groupReward.kInfo[0] = gS_QUEST_GROUP_REWARD_ACK;
		}
		CQuestGroupReward cQuestGroupReward = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.QUEST_GROUP_REWARD) as CQuestGroupReward;
		if (cQuestGroupReward != null)
		{
			cQuestGroupReward.SetData((GS_QUEST_GROUP_REWARD_ACK)this.m_groupReward.kInfo[0]);
			cQuestGroupReward.Show();
			this.m_bRewardShow = true;
		}
	}

	public void ShowReward()
	{
		if (!this.m_bRewardShow && this.m_QuestRewardInfoQueue.Count > 0)
		{
			RewardInfo rewardInfo = this.m_QuestRewardInfoQueue.Dequeue();
			if (rewardInfo != null)
			{
				QuestReward_DLG questReward_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.QUEST_REWARD) as QuestReward_DLG;
				if (questReward_DLG != null)
				{
					questReward_DLG.SetRewardInfo(rewardInfo);
					questReward_DLG.Show();
					this.m_bRewardShow = true;
				}
			}
		}
	}

	public void CloseAllReward()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.QUEST_REWARD);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.QUEST_GROUP_REWARD);
		this.m_QuestRewardInfoQueue.Clear();
		this.m_bRewardShow = false;
	}

	public void SubCharProcess(short i16CharUnique, int i32CharKind, byte bCharKindSlot, byte bType)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(i16CharUnique) as NrCharUser;
		if (nrCharUser != null)
		{
			if (0 < i32CharKind)
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
				if (bType == 3)
				{
					if (charKindInfo != null)
					{
						ECO_TALK ecoTalk = NrTSingleton<NrBaseTableManager>.Instance.GetEcoTalk(charKindInfo.GetCode());
						if (ecoTalk != null)
						{
							string textFromEco_Talk = NrTSingleton<NrTextMgr>.Instance.GetTextFromEco_Talk(ecoTalk.GetRandTalk());
							nrCharUser.SetSubCharKind(i32CharKind, (int)bCharKindSlot, textFromEco_Talk);
						}
						else
						{
							nrCharUser.SetSubCharKind(i32CharKind, (int)bCharKindSlot);
						}
					}
				}
				else
				{
					nrCharUser.SetSubCharKind(i32CharKind, (int)bCharKindSlot);
				}
				if (nrCharUser.GetID() == 1 && charKindInfo != null)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("246");
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromNotify,
						"targetname",
						charKindInfo.GetName()
					});
					if (Scene.CurScene == Scene.Type.BATTLE)
					{
						NrTSingleton<NkQuestManager>.Instance.PushMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
				}
			}
			else
			{
				nrCharUser.DeleteSubChar((int)bCharKindSlot);
			}
		}
	}

	public void QuestAutoMove()
	{
		if (this.m_szAutoMoveQuestUnique != string.Empty)
		{
			this.QuestAutoMove(this.m_szAutoMoveQuestUnique);
		}
		else if (0 < this.m_nAutoMoveNpcKind)
		{
			this.NPCAutoMove(this.m_nAutoMoveNpcKind);
		}
		else if (this.m_bAutoMove)
		{
			NrTSingleton<NrAutoPath>.Instance.ResetData();
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char == null)
			{
				return;
			}
			if (@char.m_kCharMove == null)
			{
				return;
			}
			Vector3 lhs = Vector3.zero;
			lhs = @char.m_kCharMove.FindFirstPath(this.m_nAutoMoveMapIndex, this.m_nAutoMoveDestX, this.m_nAutoMoveDestY, false);
			if (lhs != Vector3.zero)
			{
				GS_CHAR_FINDPATH_REQ gS_CHAR_FINDPATH_REQ = new GS_CHAR_FINDPATH_REQ();
				gS_CHAR_FINDPATH_REQ.DestPos.x = lhs.x;
				gS_CHAR_FINDPATH_REQ.DestPos.y = lhs.y;
				gS_CHAR_FINDPATH_REQ.DestPos.z = lhs.z;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_FINDPATH_REQ, gS_CHAR_FINDPATH_REQ);
			}
		}
		this.m_nAutoMoveNpcKind = 0;
		this.m_nAutoMoveMapIndex = 0;
		this.m_nAutoMoveGateIndex = 0;
		this.m_szAutoMoveQuestUnique = string.Empty;
		this.m_bAutoMove = false;
		this.m_nAutoMoveDestX = 0;
		this.m_nAutoMoveDestY = 0;
	}

	public void NPCAutoMove(int charKind)
	{
		NrTSingleton<NrAutoPath>.Instance.ResetData();
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		Vector3 lhs = NrTSingleton<NkQuestManager>.Instance.FindFirstPath(charKind);
		if (lhs != Vector3.zero)
		{
			GS_CHAR_FINDPATH_REQ gS_CHAR_FINDPATH_REQ = new GS_CHAR_FINDPATH_REQ();
			gS_CHAR_FINDPATH_REQ.DestPos.x = lhs.x;
			gS_CHAR_FINDPATH_REQ.DestPos.y = lhs.y;
			gS_CHAR_FINDPATH_REQ.DestPos.z = lhs.z;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_FINDPATH_REQ, gS_CHAR_FINDPATH_REQ);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "AUTOMOVE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public void TreadureFindPath(int i32DestMapIndex, float fDestX, float fDestY, float fDestZ)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		int nMapIndex = myCharInfo.m_kCharMapInfo.m_nMapIndex;
		Vector3 lhs = Vector3.zero;
		if (0 < i32DestMapIndex && (fDestX != 0f || fDestY != 0f))
		{
			this.m_nAutoMoveMapIndex = i32DestMapIndex;
			this.m_bAutoMove = true;
			this.m_nAutoMoveDestX = (short)fDestX;
			this.m_nAutoMoveDestY = (short)fDestZ;
			if (nMapIndex != i32DestMapIndex)
			{
				string mapName = NrTSingleton<MapManager>.Instance.GetMapName(i32DestMapIndex);
				if (mapName != string.Empty)
				{
					this.IsWarp(i32DestMapIndex, fDestX, fDestY, fDestZ);
					return;
				}
			}
			else
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null && @char.m_kCharMove != null)
				{
					lhs = @char.m_kCharMove.FindFirstPath(i32DestMapIndex, (short)fDestX, (short)fDestZ, false);
					if (lhs != Vector3.zero)
					{
						NrTSingleton<NkQuestManager>.Instance.QuestAutoMove();
					}
				}
			}
		}
		else if (fDestX == 0f && fDestY == 0f && fDestZ == 0f)
		{
			TsLog.LogWarning("!!!!!!! Result X : {0} , Y : {1}, KIND : {2}", new object[]
			{
				fDestX,
				fDestY,
				fDestZ
			});
		}
	}

	public void TreasureAutoMove(float fPosx, float fPosy, float fPosz)
	{
		NrTSingleton<NrAutoPath>.Instance.ResetData();
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		Vector3 lhs = new Vector3(fPosx, fPosy, fPosz);
		if (lhs != Vector3.zero)
		{
			GS_CHAR_FINDPATH_REQ gS_CHAR_FINDPATH_REQ = new GS_CHAR_FINDPATH_REQ();
			gS_CHAR_FINDPATH_REQ.DestPos.x = fPosx;
			gS_CHAR_FINDPATH_REQ.DestPos.y = fPosy;
			gS_CHAR_FINDPATH_REQ.DestPos.z = fPosz;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_FINDPATH_REQ, gS_CHAR_FINDPATH_REQ);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "AUTOMOVE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public void QuestAutoMove(string questUnique)
	{
		NrTSingleton<NrAutoPath>.Instance.ResetData();
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		Vector3 lhs = NrTSingleton<NkQuestManager>.Instance.FindFirstPath(questUnique);
		if (lhs != Vector3.zero)
		{
			GS_CHAR_FINDPATH_REQ gS_CHAR_FINDPATH_REQ = new GS_CHAR_FINDPATH_REQ();
			gS_CHAR_FINDPATH_REQ.DestPos.x = lhs.x;
			gS_CHAR_FINDPATH_REQ.DestPos.y = lhs.y;
			gS_CHAR_FINDPATH_REQ.DestPos.z = lhs.z;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_FINDPATH_REQ, gS_CHAR_FINDPATH_REQ);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "AUTOMOVE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public Vector3 FindFirstPath(int charKind)
	{
		Vector3 result = Vector3.zero;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return result;
		}
		int mapIndex = myCharInfo.m_kCharMapInfo.MapIndex;
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(charKind);
		if (charKindInfo == null)
		{
			return result;
		}
		NrNpcPos npcPos = NrTSingleton<NrNpcPosManager>.Instance.GetNpcPos(charKindInfo.GetPosKey(), charKindInfo.GetCharKind(), mapIndex);
		if (npcPos == null)
		{
			npcPos = NrTSingleton<NrNpcPosManager>.Instance.GetNpcPos(charKindInfo.GetPosKey(), charKindInfo.GetCharKind(), 2);
		}
		if (npcPos == null)
		{
			return result;
		}
		int nMapIndex = npcPos.nMapIndex;
		short destX = (short)npcPos.kPos.x;
		short destY = (short)npcPos.kPos.z;
		if (mapIndex != nMapIndex)
		{
			string mapName = NrTSingleton<MapManager>.Instance.GetMapName(nMapIndex);
			if (mapName != string.Empty)
			{
				this.IsWarp(charKind, nMapIndex);
				return result;
			}
		}
		else
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null && @char.m_kCharMove != null)
			{
				result = @char.m_kCharMove.FindFirstPath(nMapIndex, destX, destY, false);
			}
		}
		return result;
	}

	public Vector3 FindFirstPath(string questUnique)
	{
		Vector3 vector = Vector3.zero;
		if (string.Empty == questUnique)
		{
			return vector;
		}
		int num = 0;
		USER_CURRENT_QUEST_INFO currentQuest = this.GetCurrentQuest(questUnique);
		if (currentQuest == null)
		{
			return vector;
		}
		CQuest questByQuestUnique = this.GetQuestByQuestUnique(questUnique);
		if (questByQuestUnique == null)
		{
			return vector;
		}
		bool flag = false;
		QUEST_CONDITION qUEST_CONDITION = null;
		for (int i = 0; i < 3; i++)
		{
			if (0 < questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode)
			{
				if (!questByQuestUnique.CheckCondition(questByQuestUnique.GetQuestCommon().cQuestCondition[i].i64Param, ref currentQuest.i64ParamVal[i], i) || currentQuest.bFailed != 0)
				{
					flag = false;
					qUEST_CONDITION = questByQuestUnique.GetQuestCommon().cQuestCondition[i];
					break;
				}
				flag = true;
			}
		}
		int num2;
		short num3;
		short num4;
		if (flag)
		{
			QUEST_COMMON questCommon = questByQuestUnique.GetQuestCommon();
			if (questCommon == null)
			{
				return vector;
			}
			num2 = questCommon.i32EndCasteUnique;
			num3 = (short)questCommon.fEndPosX;
			num4 = (short)questCommon.fEndPosY;
			if (questCommon.i32EndType == 0 || questCommon.i32EndType == 2)
			{
				num = (int)questByQuestUnique.GetQuestCommon().i64EndTypeVal;
				NrTSingleton<NrAutoPath>.Instance.bGoNPC = true;
			}
		}
		else
		{
			if (qUEST_CONDITION == null)
			{
				return vector;
			}
			num2 = qUEST_CONDITION.nMapUnique;
			num3 = (short)qUEST_CONDITION.fTargetPosX;
			num4 = (short)qUEST_CONDITION.fTargetPosZ;
			if (qUEST_CONDITION.i32QuestCode == 30 || qUEST_CONDITION.i32QuestCode == 8 || qUEST_CONDITION.i32QuestCode == 99)
			{
				num = (int)questByQuestUnique.GetQuestCommon().i64EndTypeVal;
				NrTSingleton<NrAutoPath>.Instance.bGoNPC = true;
			}
			else
			{
				NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(questByQuestUnique.GetQuestCommon().GiveQuestCharCode);
				if (charKindInfoFromCode != null)
				{
					if (charKindInfoFromCode.GetCharKind() == qUEST_CONDITION.i32CharKind || qUEST_CONDITION.i32CharKind == -1)
					{
						num = charKindInfoFromCode.GetCharKind();
					}
					else
					{
						num = qUEST_CONDITION.i32CharKind;
					}
				}
			}
			if (num2 == 0)
			{
				num2 = questByQuestUnique.GetQuestCommon().i32EndCasteUnique;
			}
		}
		if (num2 < 0)
		{
			return vector;
		}
		QuestAutoPathInfo questAutoPathInfo = new QuestAutoPathInfo();
		questAutoPathInfo.m_bComplete = flag;
		questAutoPathInfo.m_kQuest = questByQuestUnique;
		questAutoPathInfo.m_nCharKind = num;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return vector;
		}
		int mapIndex = myCharInfo.m_kCharMapInfo.MapIndex;
		if (0 < num2 && (num3 != 0 || num4 != 0))
		{
			if (mapIndex != num2)
			{
				string mapName = NrTSingleton<MapManager>.Instance.GetMapName(num2);
				if (mapName != string.Empty)
				{
					this.IsWarp(questUnique, num2);
					return vector;
				}
			}
			else
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null && @char.m_kCharMove != null)
				{
					vector = @char.m_kCharMove.FindFirstPath(num2, num3, num4, false);
					if (vector != Vector3.zero)
					{
						NrTSingleton<NkQuestManager>.Instance.SetAutoPathInfo(questAutoPathInfo);
					}
				}
			}
		}
		else if (num3 == 0 && num4 == 0 && 0 < num)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(num);
			if (charKindInfo != null)
			{
				NrClientNpcPosList clientNpcPosList = this.GetClientNpcPosList(num2);
				if (clientNpcPosList != null)
				{
					if (mapIndex != num2)
					{
						string mapName2 = NrTSingleton<MapManager>.Instance.GetMapName(num2);
						if (mapName2 != string.Empty)
						{
							this.IsWarp(questUnique, num2);
							return vector;
						}
					}
					for (int j = 0; j < clientNpcPosList.ClientNpcPosList.Count; j++)
					{
						NrClientNpcInfo nrClientNpcInfo = clientNpcPosList.ClientNpcPosList[j];
						if (nrClientNpcInfo != null && NrTSingleton<NkQuestManager>.Instance.ClinetNpcCreateCheck(nrClientNpcInfo.kStartCon, nrClientNpcInfo.kEndCon) && charKindInfo.GetCode() == nrClientNpcInfo.strCharCode && num2 == nrClientNpcInfo.i32MapIndex)
						{
							num3 = (short)nrClientNpcInfo.fFixPosX;
							num4 = (short)nrClientNpcInfo.fFixPosY;
							NrCharBase char2 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
							if (char2 != null && char2.m_kCharMove != null)
							{
								vector = char2.m_kCharMove.FindFirstPath(num2, num3, num4, false);
								if (vector != Vector3.zero)
								{
									NrTSingleton<NkQuestManager>.Instance.SetAutoPathInfo(questAutoPathInfo);
								}
							}
						}
					}
				}
				if (vector == Vector3.zero)
				{
					NrNpcPos npcPos = NrTSingleton<NrNpcPosManager>.Instance.GetNpcPos(charKindInfo.GetPosKey(), charKindInfo.GetCharKind(), num2);
					if (npcPos != null)
					{
						num2 = npcPos.nMapIndex;
						num3 = (short)npcPos.kPos.x;
						num4 = (short)npcPos.kPos.z;
						if (mapIndex != num2)
						{
							string mapName3 = NrTSingleton<MapManager>.Instance.GetMapName(num2);
							if (mapName3 != string.Empty)
							{
								this.IsWarp(questUnique, num2);
								return vector;
							}
						}
						else
						{
							NrCharBase char3 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
							if (char3 != null && char3.m_kCharMove != null)
							{
								vector = char3.m_kCharMove.FindFirstPath(num2, num3, num4, false);
								if (vector != Vector3.zero)
								{
									NrTSingleton<NkQuestManager>.Instance.SetAutoPathInfo(questAutoPathInfo);
								}
							}
						}
					}
				}
			}
		}
		else if (0 < questByQuestUnique.GetQuestCommon().nHelpText)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(questByQuestUnique.GetQuestCommon().nHelpText.ToString()), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("420"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
		return vector;
	}

	public void UpdateFin()
	{
	}

	public void EnqueueNpcTell(CQuestNpcTell data)
	{
		this.m_kQuestNpcTell.Enqueue(data);
	}

	public void DequeueNpcTell()
	{
		if (0 < this.m_kQuestNpcTell.Count)
		{
			CQuestNpcTell cQuestNpcTell = this.m_kQuestNpcTell.Dequeue();
			if (cQuestNpcTell.szTextKey != string.Empty)
			{
				NrCharBase charByCharKind = NrTSingleton<NkCharManager>.Instance.GetCharByCharKind(cQuestNpcTell.nCharKind);
				if (charByCharKind != null)
				{
					string textFromCharInfo = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo(cQuestNpcTell.szTextKey);
					string text = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						textFromCharInfo
					});
					if (cQuestNpcTell.bTellAll)
					{
						GS_CHAT_REQ gS_CHAT_REQ = new GS_CHAT_REQ();
						gS_CHAT_REQ.RoomUnique = 0;
						gS_CHAT_REQ.ChatType = 7;
						text = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1112"), text);
						string @string = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1112"), charByCharKind.GetCharName(), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1503"), text);
						TKMarshal.StringChar(@string, ref gS_CHAT_REQ.szChatStr);
						SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAT_REQ, gS_CHAT_REQ);
						charByCharKind.MakeChatText(NrTSingleton<CTextParser>.Instance.GetTextColor("1112"), text, true);
					}
					else
					{
						charByCharKind.MakeChatText(text, true);
					}
				}
			}
		}
	}

	public void OpenQuestBattle(object a_oObject)
	{
		CQuest cQuest = (CQuest)a_oObject;
		if (cQuest == null)
		{
			return;
		}
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (!kMyCharInfo.IsEnableBattleUseActivityPoint(1))
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("488");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		NrPersonInfoUser personInfoUser = nrCharUser.GetPersonInfoUser();
		if (personInfoUser == null)
		{
			return;
		}
		bool flag = false;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 6; i++)
		{
			if (kMyCharInfo.IsAddBattleSoldier(i))
			{
				NkSoldierInfo soldierInfo = personInfoUser.GetSoldierInfo(i);
				if (soldierInfo == null || !soldierInfo.IsValid())
				{
					if (!flag)
					{
						flag = true;
					}
				}
				else
				{
					num++;
				}
				num2++;
			}
		}
		if (flag)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("146"),
				"currentnum",
				num.ToString(),
				"maxnum",
				num2.ToString()
			});
			msgBoxUI.SetMsg(new YesDelegate(this.OnBattleOK), cQuest, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("21"), empty, eMsgType.MB_OK_CANCEL);
			return;
		}
		bool flag2 = false;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = personInfoUser.GetSoldierInfo(i);
			if (soldierInfo != null && soldierInfo.IsValid())
			{
				if (soldierInfo.IsInjuryStatus())
				{
					flag2 = true;
					break;
				}
			}
		}
		if (flag2)
		{
			MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI2.SetMsg(new YesDelegate(this.OnBattleInjuryOk), cQuest, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("21"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("20"), eMsgType.MB_OK_CANCEL);
		}
		else
		{
			this.OnBattleOK(cQuest);
		}
	}

	public void OnBattleOK(object a_oObject)
	{
		CQuest cQuest = (CQuest)a_oObject;
		if (NrTSingleton<NkCharManager>.Instance.GetChar(1) == null)
		{
			return;
		}
		GS_QUEST_SCENARIOBATTLE_REQ gS_QUEST_SCENARIOBATTLE_REQ = new GS_QUEST_SCENARIOBATTLE_REQ();
		gS_QUEST_SCENARIOBATTLE_REQ.m_nScenarioBattleUnique = (int)cQuest.GetQuestCommon().cQuestCondition[0].i64Param;
		SendPacket.GetInstance().SendObject(1025, gS_QUEST_SCENARIOBATTLE_REQ);
	}

	public void OnBattleInjuryOk(object a_oObject)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLMILITARYGROUP_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.SOLMILITARYGROUP_DLG);
		}
	}

	public bool CanDayQuest(string questUnique)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return false;
		}
		CQuestGroup questGroupByQuestUnique = this.GetQuestGroupByQuestUnique(questUnique);
		if (questGroupByQuestUnique == null)
		{
			return false;
		}
		int num = (int)myCharInfo.GetCharDetail(5);
		return 0 >= num || questGroupByQuestUnique.GetGroupUnique() == num;
	}

	public void AddDayQuest(string questUnique)
	{
	}

	public bool AutoQuestExcute()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return false;
		}
		int nMapIndex = myCharInfo.m_kCharMapInfo.m_nMapIndex;
		if (0 < nMapIndex)
		{
			foreach (CAutoQuest current in this.m_HashAutoQuest.Values)
			{
				if (current != null)
				{
					if (current.GetMapUnique() != 0)
					{
						if (Scene.CurScene == Scene.Type.WORLD)
						{
							if (nMapIndex == current.GetMapUnique())
							{
								int num = nMapIndex;
								string questUnique = current.GetQuestUnique();
								if (!this.IsCompletedQuest(questUnique))
								{
									QUEST_CONST.eQUESTSTATE questState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(questUnique);
									if (!this.IsCurrentQuest(questUnique) || questState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING)
									{
										CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(questUnique);
										if (questByQuestUnique != null)
										{
											int num2 = 0;
											if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
											{
												if (current.GetAccept())
												{
													num2 = questByQuestUnique.GetQuestCommon().i32QuestCharKind;
												}
											}
											else if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE && current.GetComplete() && num == current.GetRewardMapIndex())
											{
												num2 = (int)questByQuestUnique.GetQuestCommon().i64EndTypeVal;
											}
											if (0 < num2)
											{
												this.m_nCharKind = num2;
												this.m_szQuestUnique = questUnique;
												return true;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}
		return false;
	}

	private void MapWarp(object obj)
	{
		if (this.m_nAutoMoveMapIndex == 60 || this.m_nAutoMoveMapIndex == 2 || this.m_nAutoMoveMapIndex == 4 || this.m_nAutoMoveMapIndex == 61)
		{
			this.m_showMsgDlg = false;
			GS_WARP_REQ gS_WARP_REQ = new GS_WARP_REQ();
			gS_WARP_REQ.nGateIndex = this.m_nAutoMoveGateIndex;
			gS_WARP_REQ.nWorldMapWarp_MapIDX = this.m_nAutoMoveMapIndex;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WARP_REQ, gS_WARP_REQ);
			NrTSingleton<NkClientLogic>.Instance.CharWarpRequest(0);
		}
		else
		{
			this.m_showMsgDlg = true;
			NrTSingleton<NkClientLogic>.Instance.showDown = true;
			NrTSingleton<NkClientLogic>.Instance.SetWarp(true, this.m_nAutoMoveGateIndex, this.m_nAutoMoveMapIndex);
		}
	}

	private bool IsWarp(int charKind, int destMapIndex)
	{
		string mapName = NrTSingleton<MapManager>.Instance.GetMapName(destMapIndex);
		if (!(mapName != string.Empty))
		{
			return false;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return false;
		}
		ICollection gateInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetGateInfo_Col();
		if (gateInfo_Col == null)
		{
			return false;
		}
		int num = 0;
		foreach (GATE_INFO gATE_INFO in gateInfo_Col)
		{
			if (destMapIndex == gATE_INFO.DST_MAP_IDX)
			{
				num = gATE_INFO.GATE_IDX;
			}
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("4"),
			"mapname",
			mapName
		});
		msgBoxUI.SetMsg(new YesDelegate(this.MapWarp), num, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("3"), empty, eMsgType.MB_OK_CANCEL);
		msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("109"));
		msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
		this.m_nAutoMoveMapIndex = destMapIndex;
		this.m_nAutoMoveGateIndex = num;
		this.m_nAutoMoveNpcKind = charKind;
		return true;
	}

	private bool IsWarp(string questUnique, int destMapIndex)
	{
		string mapName = NrTSingleton<MapManager>.Instance.GetMapName(destMapIndex);
		if (!(mapName != string.Empty))
		{
			return false;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return false;
		}
		ICollection gateInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetGateInfo_Col();
		if (gateInfo_Col == null)
		{
			return false;
		}
		int num = 0;
		foreach (GATE_INFO gATE_INFO in gateInfo_Col)
		{
			if (destMapIndex == gATE_INFO.DST_MAP_IDX)
			{
				num = gATE_INFO.GATE_IDX;
			}
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("4"),
			"mapname",
			mapName
		});
		msgBoxUI.SetMsg(new YesDelegate(this.MapWarp), num, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("3"), empty, eMsgType.MB_OK_CANCEL);
		msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("109"));
		msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
		this.m_nAutoMoveMapIndex = destMapIndex;
		this.m_nAutoMoveGateIndex = num;
		this.m_szAutoMoveQuestUnique = questUnique;
		return true;
	}

	private bool IsWarp(int destMapIndex, float fDestX, float fDestY, float fDestZ)
	{
		string mapName = NrTSingleton<MapManager>.Instance.GetMapName(destMapIndex);
		if (!(mapName != string.Empty))
		{
			return false;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return false;
		}
		ICollection gateInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetGateInfo_Col();
		if (gateInfo_Col == null)
		{
			return false;
		}
		int num = 0;
		foreach (GATE_INFO gATE_INFO in gateInfo_Col)
		{
			if (destMapIndex == gATE_INFO.DST_MAP_IDX)
			{
				num = gATE_INFO.GATE_IDX;
			}
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("4"),
			"mapname",
			mapName
		});
		msgBoxUI.SetMsg(new YesDelegate(this.MapWarp), num, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("3"), empty, eMsgType.MB_OK_CANCEL);
		msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("109"));
		msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
		return true;
	}

	public void PlayBundle(string name)
	{
		string str = string.Format("{0}", "UI/Drama/" + name + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.CompleteBundle), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		UIDataManager.MuteSound(true);
	}

	public void CompleteBundle(WWWItem _item, object _param)
	{
		UIDataManager.MuteSound(false);
		if (null == _item.GetSafeBundle())
		{
			return;
		}
		if (null == _item.GetSafeBundle().mainAsset)
		{
			return;
		}
		GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
		if (null == gameObject)
		{
			return;
		}
		this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
		if (null == this.rootGameObject)
		{
			return;
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootGameObject);
		}
		this.dramra = GameObject.Find("DramaCamera");
		this.main = GameObject.Find("Main Camera");
		if (null != this.dramra && null != this.main)
		{
			this.dramra.SetActive(true);
			this.main.SetActive(false);
		}
	}

	public void DeleteBundle()
	{
		UIDataManager.MuteSound(false);
		if (null != this.dramra && null != this.main)
		{
			this.dramra.SetActive(false);
			this.main.SetActive(true);
		}
		if (null != this.rootGameObject)
		{
			UnityEngine.Object.Destroy(this.rootGameObject);
		}
		Resources.UnloadUnusedAssets();
	}

	private bool Add(CQuest Quest)
	{
		this.m_HashQuest.Add(Quest.GetQuestUnique(), Quest);
		NPC_QUEST_MATCH nPC_QUEST_MATCH = new NPC_QUEST_MATCH();
		nPC_QUEST_MATCH.i64MatchUnique = 0L;
		nPC_QUEST_MATCH.CharCode = Quest.GetQuestCommon().GiveQuestCharCode;
		nPC_QUEST_MATCH.NQUESTGROUPID = Quest.GetQuestGroupUnique();
		nPC_QUEST_MATCH.strQUESTID = Quest.GetQuestUnique();
		nPC_QUEST_MATCH.NNPCID = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(nPC_QUEST_MATCH.CharCode);
		this.AddNpcQuestMatchTable(nPC_QUEST_MATCH);
		return true;
	}

	public bool Initialize()
	{
		this.m_HashQuest.Clear();
		this.m_HashQuestGroup.Clear();
		this.m_HashNpcQuestMatchTable.Clear();
		this.m_HashQuestDlg.Clear();
		this.m_HashQuestChapter.Clear();
		this.m_HashQuestNpcPos.Clear();
		this.m_HashAutoQuest.Clear();
		this.m_i32TotalQuestCount = 0;
		return true;
	}

	public void AddAutoQuest(CAutoQuest autoQuest)
	{
		if (!this.m_HashAutoQuest.ContainsKey(autoQuest.GetQuestUnique()))
		{
			this.m_HashAutoQuest.Add(autoQuest.GetQuestUnique(), autoQuest);
		}
	}

	public bool AddQuest(QUEST_COMMON Quest)
	{
		if (!this.m_HashQuest.ContainsKey(Quest.strQuestUnique))
		{
			string strDlgID = Quest.strQuestUnique + "a";
			QUEST_DLG_INFO questDlgInfo = this.GetQuestDlgInfo(strDlgID, 1);
			if (questDlgInfo != null)
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(Quest.i32QuestCharKind);
				if (charKindInfo != null)
				{
					Quest.GiveQuestCharCode = charKindInfo.GetCode();
				}
				CQuest cQuest = new CQuest();
				cQuest.SetQuestInfo(Quest);
				return this.Add(cQuest);
			}
			string msg = Quest.strQuestUnique + "의 대사 정보가 없습니다";
			NrTSingleton<NrMainSystem>.Instance.Alert(msg);
		}
		return false;
	}

	public int GetTotalQuestCount()
	{
		return this.m_HashQuest.Count;
	}

	public bool AddQuestReward(QEUST_REWARD_ITEM QuestReward)
	{
		CQuest questByQuestUnique = this.GetQuestByQuestUnique(QuestReward.strQuestUnique);
		if (questByQuestUnique == null)
		{
			return false;
		}
		QuestReward.i32RecruitGenneralUnique = QuestReward.i32RecruitGenCharKind;
		QuestReward.i32UpgradeSoldierUnique = QuestReward.i32UpgradeGenCharKind;
		int num = QuestReward.i32Grade - 1;
		if (0 > num || num >= 5)
		{
			return false;
		}
		questByQuestUnique.GetQuestCommon().cQuestRewardItem[num] = QuestReward;
		return true;
	}

	public bool AddQuestGroupInfo(QUEST_GROUP_INFO QuestGroupInfo)
	{
		QuestGroupInfo.m_QuestList.Sort(new Comparison<QUEST_SORTID>(NkQuestManager.AscendingQuestNum));
		CQuestGroup cQuestGroup = new CQuestGroup();
		cQuestGroup.SetQuestGroupInfo(QuestGroupInfo);
		if (!this.m_HashQuestGroup.ContainsKey(cQuestGroup.GetGroupUnique()))
		{
			this.m_HashQuestGroup.Add(cQuestGroup.GetGroupUnique(), cQuestGroup);
			this.m_i32TotalQuestCount += (int)cQuestGroup.GetQuestCount();
			if (this.GetChapter(QuestGroupInfo.m_i16QuestChapterUnique) == null)
			{
				QUEST_CHAPTER qUEST_CHAPTER = new QUEST_CHAPTER();
				qUEST_CHAPTER.i16QuestChapterUnique = QuestGroupInfo.m_i16QuestChapterUnique;
				qUEST_CHAPTER.strChapterTextKey = "Title_Chapter_" + qUEST_CHAPTER.i16QuestChapterUnique.ToString();
				this.AddQuestChapter(qUEST_CHAPTER);
			}
			return true;
		}
		return false;
	}

	public static int AscendingQuestNum(QUEST_SORTID x, QUEST_SORTID y)
	{
		if (x.m_i32QuestSort >= y.m_i32QuestSort)
		{
			return 1;
		}
		return -1;
	}

	public static int DesendingQuestNum(QUEST_SORTID x, QUEST_SORTID y)
	{
		if (x.m_i32QuestSort < y.m_i32QuestSort)
		{
			return 1;
		}
		return -1;
	}

	public static int AscendingQuestPage(CQuestGroup x, CQuestGroup y)
	{
		if (x.GetPageNum() >= y.GetPageNum())
		{
			return 1;
		}
		return -1;
	}

	public static int DesendingQuestPage(CQuestGroup x, CQuestGroup y)
	{
		if (x.GetPageNum() < y.GetPageNum())
		{
			return 1;
		}
		return -1;
	}

	public bool AddNpcQuestMatchTable(NPC_QUEST_MATCH cBaseNpcQuestMatchTable)
	{
		QUEST_NPC_INFO qUEST_NPC_INFO = new QUEST_NPC_INFO();
		qUEST_NPC_INFO.i32QuestGroupID = cBaseNpcQuestMatchTable.NQUESTGROUPID;
		qUEST_NPC_INFO.strQuestUnique = cBaseNpcQuestMatchTable.strQUESTID;
		NPC_QUEST_LIST nPC_QUEST_LIST;
		if (!this.m_HashNpcQuestMatchTable.ContainsKey(cBaseNpcQuestMatchTable.NNPCID))
		{
			nPC_QUEST_LIST = new NPC_QUEST_LIST();
			nPC_QUEST_LIST.i32NpcID = cBaseNpcQuestMatchTable.NNPCID;
			nPC_QUEST_LIST.QuestListCharCode = cBaseNpcQuestMatchTable.CharCode;
			nPC_QUEST_LIST.NpcQuestList.Add(qUEST_NPC_INFO);
			this.m_HashNpcQuestMatchTable.Add(nPC_QUEST_LIST.i32NpcID, nPC_QUEST_LIST);
			return true;
		}
		nPC_QUEST_LIST = this.m_HashNpcQuestMatchTable[cBaseNpcQuestMatchTable.NNPCID];
		nPC_QUEST_LIST.NpcQuestList.Add(qUEST_NPC_INFO);
		return true;
	}

	public void AddQuestDlg(QUEST_DLG_INFO QuestDlg)
	{
		if (!this.m_HashQuestDlg.ContainsKey(QuestDlg.strDialogUnique))
		{
			QUEST_DIALOGUE_HASH qUEST_DIALOGUE_HASH = new QUEST_DIALOGUE_HASH();
			if (!qUEST_DIALOGUE_HASH.m_dicQuestDlgInfo.ContainsKey(QuestDlg.i32OrderUnique))
			{
				qUEST_DIALOGUE_HASH.m_dicQuestDlgInfo.Add(QuestDlg.i32OrderUnique, QuestDlg);
			}
			qUEST_DIALOGUE_HASH.strDlgIdx = QuestDlg.strDialogUnique;
			this.m_HashQuestDlg.Add(qUEST_DIALOGUE_HASH.strDlgIdx, qUEST_DIALOGUE_HASH);
		}
		else
		{
			QUEST_DIALOGUE_HASH qUEST_DIALOGUE_HASH = this.m_HashQuestDlg[QuestDlg.strDialogUnique];
			if (!qUEST_DIALOGUE_HASH.m_dicQuestDlgInfo.ContainsKey(QuestDlg.i32OrderUnique))
			{
				qUEST_DIALOGUE_HASH.m_dicQuestDlgInfo.Add(QuestDlg.i32OrderUnique, QuestDlg);
			}
		}
	}

	public void AddQuestChapter(QUEST_CHAPTER kQuestChapter)
	{
		if (!this.m_HashQuestChapter.ContainsKey(kQuestChapter.i16QuestChapterUnique))
		{
			this.m_HashQuestChapter.Add(kQuestChapter.i16QuestChapterUnique, kQuestChapter);
		}
	}

	public void AddQuestAutoPath(NrClientNpcInfo kNpcInfo)
	{
		if (!this.m_HashQuestAutoPath.ContainsKey((int)((short)kNpcInfo.i32MapIndex)))
		{
			QUEST_AUTO_PATH_POS_CHARCODE qUEST_AUTO_PATH_POS_CHARCODE = new QUEST_AUTO_PATH_POS_CHARCODE();
			qUEST_AUTO_PATH_POS_CHARCODE.strCharCode = kNpcInfo.strCharCode;
			qUEST_AUTO_PATH_POS_CHARCODE.fDesX = kNpcInfo.fFixPosX;
			qUEST_AUTO_PATH_POS_CHARCODE.fDesY = kNpcInfo.fFixPosY;
			QUEST_AUTO_PATH_POS_SECID qUEST_AUTO_PATH_POS_SECID = new QUEST_AUTO_PATH_POS_SECID();
			qUEST_AUTO_PATH_POS_SECID.Add(qUEST_AUTO_PATH_POS_CHARCODE);
			QUEST_AUTO_PATH_POS qUEST_AUTO_PATH_POS = new QUEST_AUTO_PATH_POS();
			qUEST_AUTO_PATH_POS.m_nMapIndex = (int)((short)kNpcInfo.i32MapIndex);
			qUEST_AUTO_PATH_POS.Add(qUEST_AUTO_PATH_POS_SECID);
			this.m_HashQuestAutoPath.Add(qUEST_AUTO_PATH_POS.m_nMapIndex, qUEST_AUTO_PATH_POS);
		}
		else
		{
			QUEST_AUTO_PATH_POS qUEST_AUTO_PATH_POS2 = this.m_HashQuestAutoPath[(int)((short)kNpcInfo.i32MapIndex)];
			QUEST_AUTO_PATH_POS_SECID qUEST_AUTO_PATH_POS_SECID2 = qUEST_AUTO_PATH_POS2.GetData((int)((short)kNpcInfo.i32MapIndex));
			if (qUEST_AUTO_PATH_POS_SECID2 == null)
			{
				QUEST_AUTO_PATH_POS_CHARCODE qUEST_AUTO_PATH_POS_CHARCODE2 = new QUEST_AUTO_PATH_POS_CHARCODE();
				qUEST_AUTO_PATH_POS_CHARCODE2.strCharCode = kNpcInfo.strCharCode;
				qUEST_AUTO_PATH_POS_CHARCODE2.fDesX = kNpcInfo.fFixPosX;
				qUEST_AUTO_PATH_POS_CHARCODE2.fDesY = kNpcInfo.fFixPosY;
				qUEST_AUTO_PATH_POS_SECID2 = new QUEST_AUTO_PATH_POS_SECID();
				qUEST_AUTO_PATH_POS_SECID2.Add(qUEST_AUTO_PATH_POS_CHARCODE2);
				qUEST_AUTO_PATH_POS2.Add(qUEST_AUTO_PATH_POS_SECID2);
			}
			else if (qUEST_AUTO_PATH_POS_SECID2.GetData(kNpcInfo.strCharCode) == null)
			{
				qUEST_AUTO_PATH_POS_SECID2.Add(new QUEST_AUTO_PATH_POS_CHARCODE
				{
					strCharCode = kNpcInfo.strCharCode,
					fDesX = kNpcInfo.fFixPosX,
					fDesY = kNpcInfo.fFixPosY
				});
			}
		}
	}

	public void AddQuestAutoPath(ECO kEco)
	{
		if (!this.m_HashQuestAutoPath.ContainsKey((int)((short)kEco.MapIndex)))
		{
			QUEST_AUTO_PATH_POS_CHARCODE qUEST_AUTO_PATH_POS_CHARCODE = new QUEST_AUTO_PATH_POS_CHARCODE();
			qUEST_AUTO_PATH_POS_CHARCODE.strCharCode = kEco.szCharCode[0];
			if (kEco.kRanPos.x > 0f)
			{
				qUEST_AUTO_PATH_POS_CHARCODE.fDesX = kEco.kRanPos.x;
				qUEST_AUTO_PATH_POS_CHARCODE.fDesY = kEco.kRanPos.z;
			}
			else if (kEco.kMovePos[0].x > 0f)
			{
				qUEST_AUTO_PATH_POS_CHARCODE.fDesX = kEco.kMovePos[0].x;
				qUEST_AUTO_PATH_POS_CHARCODE.fDesY = kEco.kMovePos[0].z;
			}
			else
			{
				qUEST_AUTO_PATH_POS_CHARCODE.fDesX = kEco.kFixPos.x;
				qUEST_AUTO_PATH_POS_CHARCODE.fDesY = kEco.kFixPos.z;
			}
			QUEST_AUTO_PATH_POS_SECID qUEST_AUTO_PATH_POS_SECID = new QUEST_AUTO_PATH_POS_SECID();
			qUEST_AUTO_PATH_POS_SECID.m_nMapIndex = kEco.MapIndex;
			qUEST_AUTO_PATH_POS_SECID.Add(qUEST_AUTO_PATH_POS_CHARCODE);
			QUEST_AUTO_PATH_POS qUEST_AUTO_PATH_POS = new QUEST_AUTO_PATH_POS();
			qUEST_AUTO_PATH_POS.m_nMapIndex = kEco.MapIndex;
			qUEST_AUTO_PATH_POS.Add(qUEST_AUTO_PATH_POS_SECID);
			this.m_HashQuestAutoPath.Add(qUEST_AUTO_PATH_POS.m_nMapIndex, qUEST_AUTO_PATH_POS);
		}
		else
		{
			QUEST_AUTO_PATH_POS qUEST_AUTO_PATH_POS2 = this.m_HashQuestAutoPath[kEco.MapIndex];
			QUEST_AUTO_PATH_POS_SECID qUEST_AUTO_PATH_POS_SECID2 = qUEST_AUTO_PATH_POS2.GetData(kEco.MapIndex);
			if (qUEST_AUTO_PATH_POS_SECID2 == null)
			{
				QUEST_AUTO_PATH_POS_CHARCODE qUEST_AUTO_PATH_POS_CHARCODE2 = new QUEST_AUTO_PATH_POS_CHARCODE();
				qUEST_AUTO_PATH_POS_CHARCODE2.strCharCode = kEco.szCharCode[0];
				if (kEco.kRanPos.x > 0f)
				{
					qUEST_AUTO_PATH_POS_CHARCODE2.fDesX = kEco.kRanPos.x;
					qUEST_AUTO_PATH_POS_CHARCODE2.fDesY = kEco.kRanPos.z;
				}
				else if (kEco.kMovePos[0].x > 0f)
				{
					qUEST_AUTO_PATH_POS_CHARCODE2.fDesX = kEco.kMovePos[0].x;
					qUEST_AUTO_PATH_POS_CHARCODE2.fDesY = kEco.kMovePos[0].z;
				}
				else
				{
					qUEST_AUTO_PATH_POS_CHARCODE2.fDesX = kEco.kFixPos.x;
					qUEST_AUTO_PATH_POS_CHARCODE2.fDesY = kEco.kFixPos.z;
				}
				qUEST_AUTO_PATH_POS_SECID2 = new QUEST_AUTO_PATH_POS_SECID();
				qUEST_AUTO_PATH_POS2.m_nMapIndex = kEco.MapIndex;
				qUEST_AUTO_PATH_POS_SECID2.Add(qUEST_AUTO_PATH_POS_CHARCODE2);
				qUEST_AUTO_PATH_POS2.Add(qUEST_AUTO_PATH_POS_SECID2);
			}
			else if (qUEST_AUTO_PATH_POS_SECID2.GetData(kEco.szCharCode[0]) == null)
			{
				QUEST_AUTO_PATH_POS_CHARCODE qUEST_AUTO_PATH_POS_CHARCODE3 = new QUEST_AUTO_PATH_POS_CHARCODE();
				qUEST_AUTO_PATH_POS_CHARCODE3.strCharCode = kEco.szCharCode[0];
				if (kEco.kRanPos.x > 0f)
				{
					qUEST_AUTO_PATH_POS_CHARCODE3.fDesX = kEco.kRanPos.x;
					qUEST_AUTO_PATH_POS_CHARCODE3.fDesY = kEco.kRanPos.z;
				}
				else if (kEco.kMovePos[0].x > 0f)
				{
					qUEST_AUTO_PATH_POS_CHARCODE3.fDesX = kEco.kMovePos[0].x;
					qUEST_AUTO_PATH_POS_CHARCODE3.fDesY = kEco.kMovePos[0].z;
				}
				else
				{
					qUEST_AUTO_PATH_POS_CHARCODE3.fDesX = kEco.kFixPos.x;
					qUEST_AUTO_PATH_POS_CHARCODE3.fDesY = kEco.kFixPos.z;
				}
				qUEST_AUTO_PATH_POS_SECID2.Add(qUEST_AUTO_PATH_POS_CHARCODE3);
			}
		}
	}

	public bool AddQuestNpcPos(NrClientNpcInfo kData)
	{
		NrClientNpcPosList nrClientNpcPosList;
		if (this.m_HashQuestNpcPos.ContainsKey(kData.i32MapIndex))
		{
			nrClientNpcPosList = this.m_HashQuestNpcPos[kData.i32MapIndex];
			nrClientNpcPosList.ClientNpcPosList.Add(kData);
			return true;
		}
		nrClientNpcPosList = new NrClientNpcPosList();
		nrClientNpcPosList.i32MapIndex = kData.i32MapIndex;
		nrClientNpcPosList.ClientNpcPosList.Add(kData);
		this.m_HashQuestNpcPos.Add(nrClientNpcPosList.i32MapIndex, nrClientNpcPosList);
		return true;
	}

	public bool AddQuestGroupReward(QUEST_GROUP_REWARD_TEMP temp)
	{
		if (temp.i32Level < 1 || temp.i32Level > 5)
		{
			return false;
		}
		if (this.GetQuestGroupByGroupUnique(temp.i32GroupUnique) == null)
		{
			return false;
		}
		if (this.m_HashQuestGroupReward.ContainsKey(temp.i32GroupUnique))
		{
			QUEST_GROUP_REWARD qUEST_GROUP_REWARD = this.m_HashQuestGroupReward[temp.i32GroupUnique];
			qUEST_GROUP_REWARD.nItemUnique[temp.i32Level - 1] = temp.nItemUnique;
			qUEST_GROUP_REWARD.i32ItemNum[temp.i32Level - 1] = temp.i32ItemNum;
		}
		else
		{
			QUEST_GROUP_REWARD qUEST_GROUP_REWARD = new QUEST_GROUP_REWARD();
			qUEST_GROUP_REWARD.i32GroupUnique = temp.i32GroupUnique;
			qUEST_GROUP_REWARD.nItemUnique[temp.i32Level - 1] = temp.nItemUnique;
			qUEST_GROUP_REWARD.i32ItemNum[temp.i32Level - 1] = temp.i32ItemNum;
			this.m_HashQuestGroupReward.Add(temp.i32GroupUnique, qUEST_GROUP_REWARD);
		}
		return true;
	}

	public QUEST_GROUP_REWARD GetQuestGroupReward(int i32QuestGroupUnique)
	{
		if (this.m_HashQuestGroupReward.ContainsKey(i32QuestGroupUnique))
		{
			return this.m_HashQuestGroupReward[i32QuestGroupUnique];
		}
		return null;
	}

	public NrClientNpcPosList GetClientNpcPosList(int mapIndex)
	{
		if (this.m_HashQuestNpcPos.ContainsKey(mapIndex))
		{
			return this.m_HashQuestNpcPos[mapIndex];
		}
		return null;
	}

	public short GetChapterUnique(string strQuestUnique)
	{
		CQuestGroup questGroupByQuestUnique = this.GetQuestGroupByQuestUnique(strQuestUnique);
		if (questGroupByQuestUnique == null)
		{
			return 0;
		}
		if (this.m_HashQuestChapter.ContainsKey(questGroupByQuestUnique.GetGroupInfo().m_i16QuestChapterUnique))
		{
			QUEST_CHAPTER qUEST_CHAPTER = this.m_HashQuestChapter[questGroupByQuestUnique.GetGroupInfo().m_i16QuestChapterUnique];
			return qUEST_CHAPTER.i16QuestChapterUnique;
		}
		return 0;
	}

	public QUEST_CHAPTER GetChapter(short i16ChapterUnique)
	{
		if (this.m_HashQuestChapter.ContainsKey(i16ChapterUnique))
		{
			return this.m_HashQuestChapter[i16ChapterUnique];
		}
		return null;
	}

	public string GetChapterTitle(string strQuestUnique)
	{
		CQuestGroup questGroupByQuestUnique = this.GetQuestGroupByQuestUnique(strQuestUnique);
		if (questGroupByQuestUnique == null)
		{
			return string.Empty;
		}
		if (this.m_HashQuestChapter.ContainsKey(questGroupByQuestUnique.GetGroupInfo().m_i16QuestChapterUnique))
		{
			QUEST_CHAPTER qUEST_CHAPTER = this.m_HashQuestChapter[questGroupByQuestUnique.GetGroupInfo().m_i16QuestChapterUnique];
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Title(qUEST_CHAPTER.strChapterTextKey);
		}
		return string.Empty;
	}

	public int GetQuestGroupCount()
	{
		return this.m_HashQuestGroup.Count;
	}

	public bool CheckQuestResult(string strQuestUnique, USER_CURRENT_QUEST_INFO cUserCurrentQuestInfo)
	{
		return this.m_HashQuest.ContainsKey(strQuestUnique) && this.m_HashQuest[strQuestUnique].CheckQuestResult(cUserCurrentQuestInfo);
	}

	public bool PreCheckQuestAccept(string strQuestUnique)
	{
		bool result = false;
		if (this.m_HashQuest.ContainsKey(strQuestUnique))
		{
			CQuest cQuest = this.m_HashQuest[strQuestUnique];
			result = cQuest.PreCheckQuestAccept();
		}
		return result;
	}

	public bool PreQuestChek(string strQuestUnique)
	{
		if (this.m_HashQuest.ContainsKey(strQuestUnique))
		{
			CQuest cQuest = this.m_HashQuest[strQuestUnique];
			if (!cQuest.GetQuestCommon().strPreQuestUnique.Equals("0"))
			{
				if (NrTSingleton<NkQuestManager>.Instance.IsCompletedQuest(cQuest.GetQuestCommon().strPreQuestUnique))
				{
					return true;
				}
				CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(cQuest.GetQuestGroupUnique());
				if (questGroupByGroupUnique == null)
				{
					return false;
				}
				if (!questGroupByGroupUnique.IsFristQuest(strQuestUnique))
				{
					return false;
				}
				CQuest questByQuestUnique = this.GetQuestByQuestUnique(cQuest.GetQuestCommon().strPreQuestUnique);
				if (questByQuestUnique != null)
				{
					USER_QUEST_COMPLETE_INFO completeQuestInfo = NrTSingleton<NkQuestManager>.Instance.GetCompleteQuestInfo(questByQuestUnique.GetQuestGroupUnique());
					if (completeQuestInfo != null && completeQuestInfo.bCleared == 1)
					{
						return true;
					}
				}
			}
			else if (cQuest.GetQuestCommon().strPreQuestUnique.Equals("0"))
			{
				return true;
			}
		}
		return false;
	}

	public bool SetUserCurrentQuest(long i64QuestID, int i32QuestGroupUnique, string strQuestUnique, ref USER_CURRENT_QUEST_INFO UserCurrentQuestInfo)
	{
		if (!this.m_HashQuest.ContainsKey(strQuestUnique))
		{
			return false;
		}
		CQuest cQuest = this.m_HashQuest[strQuestUnique];
		QUEST_COMMON questCommon = cQuest.GetQuestCommon();
		if (questCommon != null)
		{
			UserCurrentQuestInfo.strQuestUnique = questCommon.strQuestUnique;
			UserCurrentQuestInfo.i64QuestID = i64QuestID;
			UserCurrentQuestInfo.i32QuestGroupUnique = i32QuestGroupUnique;
		}
		return true;
	}

	public CQuest GetQuestByQuestUnique(string strQuestUnique)
	{
		if (!this.m_HashQuest.ContainsKey(strQuestUnique))
		{
			return null;
		}
		return this.m_HashQuest[strQuestUnique];
	}

	public string GetQuestUniqueByBit(int i32QuestGroupID, byte bBit)
	{
		if (!this.m_HashQuestGroup.ContainsKey(i32QuestGroupID))
		{
			return string.Empty;
		}
		return this.m_HashQuestGroup[i32QuestGroupID].GetQuestUniqueByBit((int)bBit);
	}

	public int GetGroupIDByQuestUnique(string strQuestUnique)
	{
		if (!this.m_HashQuest.ContainsKey(strQuestUnique))
		{
			return -1;
		}
		return this.m_HashQuest[strQuestUnique].GetQuestGroupUnique();
	}

	public CQuestGroup GetQuestGroupByQuestUnique(string strQuestUnique)
	{
		int groupIDByQuestUnique = this.GetGroupIDByQuestUnique(strQuestUnique);
		if (!this.m_HashQuestGroup.ContainsKey(groupIDByQuestUnique))
		{
			return null;
		}
		return this.m_HashQuestGroup[groupIDByQuestUnique];
	}

	public CQuestGroup GetQuestGroupByGroupUnique(int i32QuestGroupID)
	{
		if (!this.m_HashQuestGroup.ContainsKey(i32QuestGroupID))
		{
			return null;
		}
		return this.m_HashQuestGroup[i32QuestGroupID];
	}

	public CQuestGroup GetQuestGroup(short i16QuestChapterUnique, short i16QuestPageUnique)
	{
		foreach (CQuestGroup current in this.m_HashQuestGroup.Values)
		{
			if (current.GetChapterUnique() == i16QuestChapterUnique && current.GetPageUnique() == i16QuestPageUnique)
			{
				return current;
			}
		}
		return null;
	}

	public int GetBitByQuestUnique(string strQuestUnique)
	{
		int groupIDByQuestUnique = this.GetGroupIDByQuestUnique(strQuestUnique);
		if (!this.m_HashQuestGroup.ContainsKey(groupIDByQuestUnique))
		{
			return -1;
		}
		return this.m_HashQuestGroup[groupIDByQuestUnique].GetBitByQuestUnique(strQuestUnique);
	}

	public NPC_QUEST_LIST GetNpcQuestMatchInfo(int i32NpcID)
	{
		if (!this.m_HashNpcQuestMatchTable.ContainsKey(i32NpcID))
		{
			return null;
		}
		return this.m_HashNpcQuestMatchTable[i32NpcID];
	}

	public Dictionary<int, NPC_QUEST_LIST> GetNpcQuestMatchTable()
	{
		return this.m_HashNpcQuestMatchTable;
	}

	public QUEST_DLG_INFO GetQuestDlgInfo(string strDlgID, int i32DlgOrder)
	{
		if (!this.m_HashQuestDlg.ContainsKey(strDlgID))
		{
			return null;
		}
		QUEST_DIALOGUE_HASH qUEST_DIALOGUE_HASH = this.m_HashQuestDlg[strDlgID];
		if (!qUEST_DIALOGUE_HASH.m_dicQuestDlgInfo.ContainsKey(i32DlgOrder))
		{
			return null;
		}
		return qUEST_DIALOGUE_HASH.m_dicQuestDlgInfo[i32DlgOrder];
	}

	public int GetQuestDlgMaxCount(string strDlgID)
	{
		if (!this.m_HashQuestDlg.ContainsKey(strDlgID))
		{
			return 0;
		}
		QUEST_DIALOGUE_HASH qUEST_DIALOGUE_HASH = this.m_HashQuestDlg[strDlgID];
		return qUEST_DIALOGUE_HASH.m_dicQuestDlgInfo.Count;
	}

	public string GetDlgText(string strDlgID, int i32DlgOrder)
	{
		if (!this.m_HashQuestDlg.ContainsKey(strDlgID))
		{
			return string.Empty;
		}
		QUEST_DIALOGUE_HASH qUEST_DIALOGUE_HASH = this.m_HashQuestDlg[strDlgID];
		if (!qUEST_DIALOGUE_HASH.m_dicQuestDlgInfo.ContainsKey(i32DlgOrder))
		{
			return string.Empty;
		}
		QUEST_DLG_INFO qUEST_DLG_INFO = qUEST_DIALOGUE_HASH.m_dicQuestDlgInfo[i32DlgOrder];
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Dialog(qUEST_DLG_INFO.strLang_Idx);
	}

	public void SetQuestGroupList(ref CQuestGroup[] questGroupArray)
	{
		int num = 0;
		foreach (CQuestGroup current in this.m_HashQuestGroup.Values)
		{
			questGroupArray[num] = current;
			num++;
		}
		this.SortingGroupBySortNum(ref questGroupArray, 0, 12);
	}

	public void SortingGroupBySortNum(ref CQuestGroup[] questGroupArray, int i32Low, int i32High)
	{
		if (i32Low >= i32High)
		{
			return;
		}
		int groupSortNum = questGroupArray[(i32Low + i32High) / 2].GetGroupSortNum();
		int num = i32Low;
		int num2 = i32High;
		do
		{
			while (questGroupArray[num].GetGroupSortNum() < groupSortNum)
			{
				num++;
			}
			while (questGroupArray[num2].GetGroupSortNum() > groupSortNum)
			{
				num2--;
			}
			if (num <= num2)
			{
				CQuestGroup cQuestGroup = questGroupArray[num];
				questGroupArray[num] = questGroupArray[num2];
				questGroupArray[num2] = cQuestGroup;
				num++;
				num2--;
			}
		}
		while (num <= num2);
		if (i32Low < num2)
		{
			this.SortingGroupBySortNum(ref questGroupArray, i32Low, num2);
		}
		if (num < i32High)
		{
			this.SortingGroupBySortNum(ref questGroupArray, num, i32High);
		}
	}

	public void SortingQuestBySortNum(ref QUEST_SORTID[] questArray, int i32Low, int i32High)
	{
		if (i32Low >= i32High)
		{
			return;
		}
		int i32QuestSort = questArray[(i32Low + i32High) / 2].m_i32QuestSort;
		int num = i32Low;
		int num2 = i32High;
		do
		{
			while (questArray[num].m_i32QuestSort < i32QuestSort)
			{
				num++;
			}
			while (questArray[num2].m_i32QuestSort > i32QuestSort)
			{
				num2--;
			}
			if (num <= num2)
			{
				QUEST_SORTID qUEST_SORTID = questArray[num];
				questArray[num] = questArray[num2];
				questArray[num2] = qUEST_SORTID;
				num++;
				num2--;
			}
		}
		while (num <= num2);
		if (i32Low < num2)
		{
			this.SortingQuestBySortNum(ref questArray, i32Low, num2);
		}
		if (num < i32High)
		{
			this.SortingQuestBySortNum(ref questArray, num, i32High);
		}
	}

	public CQuest GetNextQuest(string strQuestUnique)
	{
		CQuest result = null;
		if (string.Empty == strQuestUnique)
		{
			return result;
		}
		CQuest questByQuestUnique = this.GetQuestByQuestUnique(strQuestUnique);
		if (questByQuestUnique != null)
		{
			result = this.GetQuestByQuestUnique(questByQuestUnique.GetQuestCommon().strNextQuestUnique);
		}
		return result;
	}

	public Dictionary<int, CQuestGroup> GetHashQuestGroup()
	{
		return this.m_HashQuestGroup;
	}

	public Dictionary<short, QUEST_CHAPTER> GetHashQuestChapter()
	{
		return this.m_HashQuestChapter;
	}

	public void SetQuestList(ref List<CQuest> QuestList)
	{
		foreach (CQuest current in this.m_HashQuest.Values)
		{
			QuestList.Add(current);
		}
	}

	public QUEST_AUTO_PATH_POS_CHARCODE GetAutoPath(int mapIndex, short i16SecUnique, string strCharCode)
	{
		if (this.m_HashQuestAutoPath.ContainsKey(mapIndex))
		{
			QUEST_AUTO_PATH_POS qUEST_AUTO_PATH_POS = this.m_HashQuestAutoPath[mapIndex];
			QUEST_AUTO_PATH_POS_SECID data = qUEST_AUTO_PATH_POS.GetData((int)i16SecUnique);
			if (data != null)
			{
				QUEST_AUTO_PATH_POS_CHARCODE data2 = data.GetData(strCharCode);
				if (data2 != null)
				{
					return data2;
				}
			}
		}
		return null;
	}

	public void ParseQuestDlg(ref QUEST_DLG_INFO QuestDlg)
	{
		char[] array = TKString.StringChar(QuestDlg.strLang_Idx);
		byte b = 0;
		byte b2 = 0;
		string str = string.Empty;
		string str2 = string.Empty;
		string s = string.Empty;
		string text = string.Empty;
		string eventUnique = string.Empty;
		string text2 = string.Empty;
		while ((int)b < QuestDlg.strLang_Idx.Length)
		{
			if (array[(int)b] == ' ')
			{
				b += 1;
			}
			else if (array[(int)b] == '_')
			{
				if (b2 == 0)
				{
					str = text2;
					text2 = string.Empty;
					b2 += 1;
				}
				else if (b2 == 1)
				{
					if ("a" == text2)
					{
						str2 = "accept";
					}
					else if ("g" == text2)
					{
						str2 = "ongoing";
					}
					else if ("p" == text2)
					{
						str2 = "complete";
					}
					text2 = string.Empty;
					b2 += 1;
				}
				else if (b2 == 2)
				{
					s = text2;
					text2 = string.Empty;
					b2 += 1;
				}
				else if (b2 == 3)
				{
					text = text2;
					text2 = string.Empty;
					b2 += 1;
				}
				b += 1;
			}
			else if (array[(int)b] == '/')
			{
				text2 += '_';
				b += 1;
			}
			else
			{
				text2 += array[(int)b];
				b += 1;
			}
		}
		if ("event" != text2 && string.Empty == text)
		{
			text = text2;
		}
		else
		{
			eventUnique = text2;
		}
		QuestDlg.strDialogUnique = str + str2;
		int.TryParse(s, out QuestDlg.i32OrderUnique);
		QuestDlg.QuestDlgCharCode = text;
		QuestDlg.EventUnique = eventUnique;
	}

	public IDictionaryEnumerator GetEnumerator()
	{
		IDictionaryEnumerator dictionaryEnumerator = this.m_HashQuestDlg.GetEnumerator();
		dictionaryEnumerator.Reset();
		return dictionaryEnumerator;
	}

	public bool AddQuestDropItem(QUEST_DROP_ITEM item)
	{
		if (this.m_QuestDropItem.ContainsKey(item.strQuestUnique))
		{
			QUEST_DROP_ITEM_List qUEST_DROP_ITEM_List = this.m_QuestDropItem[item.strQuestUnique];
			qUEST_DROP_ITEM_List.dropItemList.Add(item);
		}
		else
		{
			QUEST_DROP_ITEM_List qUEST_DROP_ITEM_List2 = new QUEST_DROP_ITEM_List();
			qUEST_DROP_ITEM_List2.strQuestUnique = item.strQuestUnique;
			qUEST_DROP_ITEM_List2.dropItemList.Add(item);
			this.m_QuestDropItem.Add(qUEST_DROP_ITEM_List2.strQuestUnique, qUEST_DROP_ITEM_List2);
		}
		return true;
	}

	public QUEST_DROP_ITEM_List GetQuestDropItemList(string strQuestUnique)
	{
		if (this.m_QuestDropItem.ContainsKey(strQuestUnique))
		{
			return this.m_QuestDropItem[strQuestUnique];
		}
		return null;
	}

	public void SortingQuestInGroup()
	{
		foreach (CQuestGroup current in this.m_HashQuestGroup.Values)
		{
			current.SortingQuestInGroup();
		}
	}

	public QUEST_SORTID GetSortIDbyQuestUnique(string strQuestUnique)
	{
		CQuestGroup questGroupByQuestUnique = this.GetQuestGroupByQuestUnique(strQuestUnique);
		if (questGroupByQuestUnique != null)
		{
			return questGroupByQuestUnique.GetQuestSortIDByQuestUnique(strQuestUnique);
		}
		return null;
	}

	public CQuest GetFirstQuestInGroup(int i32GroupUnique)
	{
		CQuestGroup questGroupByGroupUnique = this.GetQuestGroupByGroupUnique(i32GroupUnique);
		if (questGroupByGroupUnique == null)
		{
			return null;
		}
		return questGroupByGroupUnique.GetFirstQuest();
	}

	public CQuest GetLastQuestInGroup(int i32GroupUnique)
	{
		CQuestGroup questGroupByGroupUnique = this.GetQuestGroupByGroupUnique(i32GroupUnique);
		if (questGroupByGroupUnique == null)
		{
			return null;
		}
		return questGroupByGroupUnique.GetLastQuest();
	}

	public void SetQuestTooltipMsg(string strQuestUnique, int Lang_Type_Tooltip, string Lang_Idx_Tooltip)
	{
		if (this.m_HashQuest.ContainsKey(strQuestUnique))
		{
			CQuest cQuest = this.m_HashQuest[strQuestUnique];
			cQuest.SetToolTipMsg(Lang_Type_Tooltip, Lang_Idx_Tooltip);
		}
	}

	public void SetLimit(string strQuestUnique, short i16QuestChapterUnique, short i16QuestPageUnique, bool bLimit)
	{
		if (strQuestUnique != string.Empty && this.m_HashQuest.ContainsKey(strQuestUnique))
		{
			this.SetLimit_QuestUnique(strQuestUnique, bLimit);
			return;
		}
		if (i16QuestPageUnique > 0)
		{
			this.SetLimit_QuestPageUnique(i16QuestChapterUnique, i16QuestPageUnique, bLimit);
			return;
		}
		if (i16QuestChapterUnique > 0)
		{
			this.SetLimit_QuestChapterUnique(i16QuestChapterUnique, bLimit);
			return;
		}
	}

	private void SetLimit_QuestUnique(string strQuestUnique, bool bLimit)
	{
		CQuest questByQuestUnique = this.GetQuestByQuestUnique(strQuestUnique);
		if (questByQuestUnique != null)
		{
			questByQuestUnique.GetQuestCommon().bIsLimit = bLimit;
		}
	}

	private void SetLimit_QuestPageUnique(short i16QuestChapterUnique, short i16QuestPageUnique, bool bLimit)
	{
		CQuestGroup questGroup = this.GetQuestGroup(i16QuestChapterUnique, i16QuestPageUnique);
		if (questGroup != null)
		{
			questGroup.GetGroupInfo().bIsLimit = bLimit;
			if (this.m_HashQuestChapter.ContainsKey(questGroup.GetChapterUnique()))
			{
				this.m_HashQuestChapter[questGroup.GetChapterUnique()].bIsLimit = bLimit;
			}
			foreach (QUEST_SORTID current in questGroup.GetGroupInfo().m_QuestList)
			{
				this.SetLimit_QuestUnique(current.m_strQuestUnique, bLimit);
			}
		}
	}

	private void SetLimit_QuestChapterUnique(short i16QuestChapterUnique, bool bLimit)
	{
		foreach (CQuestGroup current in this.m_HashQuestGroup.Values)
		{
			if (current.GetChapterUnique() == i16QuestChapterUnique)
			{
				current.GetGroupInfo().bIsLimit = bLimit;
				if (this.m_HashQuestChapter.ContainsKey(current.GetChapterUnique()))
				{
					this.m_HashQuestChapter[current.GetChapterUnique()].bIsLimit = bLimit;
				}
				foreach (QUEST_SORTID current2 in current.GetGroupInfo().m_QuestList)
				{
					this.SetLimit_QuestUnique(current2.m_strQuestUnique, bLimit);
				}
			}
		}
	}

	public int GetSubQuestCount()
	{
		int num = 0;
		int level = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel();
		foreach (CQuestGroup current in this.m_HashQuestGroup.Values)
		{
			if (current.GetQuestType() == 2)
			{
				for (int i = 0; i < 200; i++)
				{
					CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(current.GetQuestUniqueByBit(i));
					if (questByQuestUnique != null)
					{
						if (!NrTSingleton<NkQuestManager>.Instance.IsCompletedQuest(questByQuestUnique.GetQuestUnique()))
						{
							if ((int)questByQuestUnique.GetQuestLevel(1) <= level)
							{
								if (NrTSingleton<NkQuestManager>.Instance.GetQuestState(questByQuestUnique.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
								{
									num++;
									break;
								}
							}
						}
					}
				}
			}
		}
		return num;
	}
}
