using BattleTriggerClient;
using GAME;
using GameMessage;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle : IDisposable
{
	private static Battle BattleClient = null;

	private static bool m_bReplay = false;

	private static int m_nBabelPartyCount = 0;

	private static bool m_bLeader = false;

	private Queue<PacketClientOrder> m_PacketQueue;

	private stBattleInfo m_stBattleInfo;

	public short m_nBattleMapIDX = -1;

	private bool m_bSetbattleInfo;

	private bool m_bLoadCompleteTerrain;

	private bool m_bObserver;

	private bool m_bColosseumObserver;

	private eBATTLE_ROOM_STATE m_eBattleState;

	private bool m_bShowFirstTurnAllyEffect;

	private GameObject m_goFirstTurnEffectObject;

	private bool m_bUpdatePacket;

	private NrBattleMap m_BattleMap;

	private NrGridManager m_GridMgr = new NrGridManager();

	private eBATTLE_ROOMTYPE m_eBattleRoomtype;

	private List<BATTLE_SOLDIER_INFO> m_MakeCharList;

	private int m_nScenarioUnique = -1;

	private List<int> m_PreLoadingCharKindList;

	private List<short> m_MyCharBUID;

	private int m_nSelectCharIndex;

	private eBATTLE_ALLY m_eMyAlly = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;

	private short m_nMyStartPosIndex = -1;

	private List<short> m_SelectCharList;

	public float m_fContinueBattleWaitTime;

	public float m_fServerBattleTime;

	public float m_fServerTimeSetTime;

	private int m_nChangeSolCount;

	private bool m_bUseEmergencyHelpByThisRound;

	private bool m_bUseEmergencyHelpByThisTurn;

	public bool m_bShowColosseumSummonEffect;

	private int m_nDieSolCount;

	private long bossMaxHP;

	private long bossCurrentHP;

	public GS_BF_TURN_ACTIVE_POWER[] m_TurnActivePower;

	private static string ms_currentBattleMapPath = string.Empty;

	private static string ms_downloadingBatteMapPath = string.Empty;

	private NrBattleCamera m_Camera;

	private bool m_bRestartAction;

	private bool m_bRestartMakeChar;

	public Dictionary<int, Queue<GS_BF_CHARINFO_NFY>> mOrderNfyTable = new Dictionary<int, Queue<GS_BF_CHARINFO_NFY>>();

	private Dictionary<eBATTLE_ALLY, Dictionary<int, BATTLE_POS_GRID>> m_BattlePosGrid;

	public bool m_bOnlyServerMove;

	private bool m_bStop;

	private eBATTLE_ALLY m_eCurrentTurnAlly = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;

	private float m_fTurnTime;

	public int m_nTurnCount;

	private bool m_bInputControlTrigger;

	private TsAudio m_BGMAudio;

	private int m_iStartBGMIndex = -1;

	private int[] m_iBGMIndex;

	private List<GetItemDlg> m_ListGetItemDlg;

	private bool m_bExitReservation;

	private GameObject m_goDamageEffect;

	private GameObject m_goCameraTarget;

	private GameObject m_goResultEffect;

	private GameObject m_goMoveArrow;

	private GameObject m_goSkillDirecting;

	private GameObject m_goSkillRivalDirecting;

	private GameObject m_goControlAngergauge;

	private GameObject m_goColosseumCardSummons;

	private GameObject m_goColosseumKill;

	private GameObject m_goColosseumCount;

	private GameObject m_goColosseumCritical;

	private GameObject m_goColosseumRecall;

	private bool m_bSlowMotion;

	private float m_fSlowStartTime;

	private Battle_Input m_pkBattleInput;

	public float m_fOrderPing;

	public float m_fInfoPing;

	private bool m_bCameraRotate;

	private float m_fCameraRotateEnd;

	private bool m_bCameraMove;

	private float m_fCameraMoveEnd;

	private float m_fEnableOrderTime;

	private short m_nBabelAdvantageCharUnique;

	private bool m_bEmotionSet;

	private eBATTLE_EMOTICON m_eSetEmoticon = eBATTLE_EMOTICON.eBATTLE_EMOTICON_MAX;

	private List<int> m_lsBabelCharKind;

	private List<long> m_babelBattleSolList;

	private bool m_bRelogin;

	private ArrayList m_arDamageExpension;

	private NkBattleDamage[] m_arDamage;

	private static float m_fPlayAddRate = 0f;

	private bool m_bSolCombinationDirectionIsEnd;

	private eBATTLE_ORDER m_eRequestOrder;

	public int m_iBattleSkillIndex = -1;

	private bool m_bCheckTargetBt;

	private int m_nTargetBtCount;

	private bool m_bTurnOverRequest;

	private static float m_fTurnOverRequestTime;

	public static Battle BATTLE
	{
		get
		{
			return Battle.BattleClient;
		}
	}

	public static bool Replay
	{
		get
		{
			return Battle.m_bReplay;
		}
		set
		{
			Battle.m_bReplay = value;
		}
	}

	public static int BabelPartyCount
	{
		get
		{
			return Battle.m_nBabelPartyCount;
		}
		set
		{
			Battle.m_nBabelPartyCount = value;
		}
	}

	public static bool isLeader
	{
		get
		{
			return Battle.m_bLeader;
		}
		set
		{
			Battle.m_bLeader = value;
		}
	}

	public stBattleInfo BattleInfo
	{
		get
		{
			return this.m_stBattleInfo;
		}
		set
		{
			this.m_stBattleInfo = value;
		}
	}

	public bool Observer
	{
		get
		{
			return this.m_bObserver;
		}
		set
		{
			this.m_bObserver = value;
		}
	}

	public bool ColosseumObserver
	{
		get
		{
			return this.m_bColosseumObserver;
		}
		set
		{
			this.m_bColosseumObserver = value;
		}
	}

	public bool ShowTurnAllyEffect
	{
		get
		{
			return this.m_bShowFirstTurnAllyEffect;
		}
		set
		{
			this.m_bShowFirstTurnAllyEffect = value;
		}
	}

	public bool UpdatePacketEnable
	{
		get
		{
			return this.m_bUpdatePacket;
		}
		set
		{
			this.m_bUpdatePacket = value;
		}
	}

	public NrGridManager GRID_MANAGER
	{
		get
		{
			return this.m_GridMgr;
		}
	}

	public eBATTLE_ROOMTYPE BattleRoomtype
	{
		get
		{
			return this.m_eBattleRoomtype;
		}
		set
		{
			this.m_eBattleRoomtype = value;
		}
	}

	public eBATTLE_ALLY MyAlly
	{
		get
		{
			return this.m_eMyAlly;
		}
		set
		{
			this.m_eMyAlly = value;
		}
	}

	public short MyStartPosIndex
	{
		get
		{
			return this.m_nMyStartPosIndex;
		}
		set
		{
			this.m_nMyStartPosIndex = value;
		}
	}

	public int ChangeSolCount
	{
		get
		{
			return this.m_nChangeSolCount;
		}
		set
		{
			this.m_nChangeSolCount = value;
		}
	}

	public bool UseEmergencyHelpByThisRound
	{
		get
		{
			return this.m_bUseEmergencyHelpByThisRound;
		}
		set
		{
			this.m_bUseEmergencyHelpByThisRound = value;
		}
	}

	public bool UseEmergencyHelpByThisTurn
	{
		get
		{
			return this.m_bUseEmergencyHelpByThisTurn;
		}
		set
		{
			this.m_bUseEmergencyHelpByThisTurn = value;
		}
	}

	public bool ShowColosseumSummonEffect
	{
		get
		{
			return this.m_bShowColosseumSummonEffect;
		}
		set
		{
			this.m_bShowColosseumSummonEffect = value;
		}
	}

	public int DieSolCount
	{
		get
		{
			return this.m_nDieSolCount;
		}
		set
		{
			this.m_nDieSolCount = value;
		}
	}

	public long BossMaxHP
	{
		get
		{
			return this.bossMaxHP;
		}
		set
		{
			this.bossMaxHP = value;
			this.BossCurrentHP = value;
		}
	}

	public long BossCurrentHP
	{
		get
		{
			return this.bossCurrentHP;
		}
		set
		{
			this.bossCurrentHP = value;
			if (this.bossCurrentHP <= 0L)
			{
				this.bossCurrentHP = 0L;
			}
		}
	}

	public NrBattleCamera BattleCamera
	{
		get
		{
			return this.m_Camera;
		}
		set
		{
			this.m_Camera = value;
		}
	}

	public bool Stop
	{
		get
		{
			return this.m_bStop;
		}
		set
		{
			this.m_bStop = value;
		}
	}

	public eBATTLE_ALLY CurrentTurnAlly
	{
		get
		{
			return this.m_eCurrentTurnAlly;
		}
		set
		{
			this.m_eCurrentTurnAlly = value;
		}
	}

	public bool InputControlTrigger
	{
		get
		{
			return this.m_bInputControlTrigger;
		}
		set
		{
			this.m_bInputControlTrigger = value;
		}
	}

	public List<GetItemDlg> ListGetItemDlg
	{
		get
		{
			return this.m_ListGetItemDlg;
		}
		set
		{
			this.m_ListGetItemDlg = value;
		}
	}

	public bool ExitReservation
	{
		get
		{
			return this.m_bExitReservation;
		}
		set
		{
			this.m_bExitReservation = value;
		}
	}

	public GameObject DamageEffect
	{
		get
		{
			return this.m_goDamageEffect;
		}
		set
		{
			this.m_goDamageEffect = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goDamageEffect);
			}
		}
	}

	public GameObject CameraTarget
	{
		get
		{
			return this.m_goCameraTarget;
		}
		set
		{
			this.m_goCameraTarget = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goCameraTarget);
			}
		}
	}

	public GameObject ResultEffect
	{
		get
		{
			return this.m_goResultEffect;
		}
		set
		{
			this.m_goResultEffect = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goResultEffect);
			}
		}
	}

	public GameObject MoveArrow
	{
		get
		{
			return this.m_goMoveArrow;
		}
		set
		{
			this.m_goMoveArrow = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goMoveArrow);
			}
		}
	}

	public GameObject SkillDirecting
	{
		get
		{
			return this.m_goSkillDirecting;
		}
		set
		{
			this.m_goSkillDirecting = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goSkillDirecting);
			}
		}
	}

	public GameObject SkillRivalDirecting
	{
		get
		{
			return this.m_goSkillRivalDirecting;
		}
		set
		{
			this.m_goSkillRivalDirecting = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goSkillRivalDirecting);
			}
		}
	}

	public GameObject ControlAngergauge
	{
		get
		{
			return this.m_goControlAngergauge;
		}
		set
		{
			this.m_goControlAngergauge = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goControlAngergauge);
			}
		}
	}

	public GameObject ColosseumCardSummons
	{
		get
		{
			return this.m_goColosseumCardSummons;
		}
		set
		{
			this.m_goColosseumCardSummons = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goColosseumCardSummons);
			}
		}
	}

	public GameObject ColosseumKill
	{
		get
		{
			return this.m_goColosseumKill;
		}
		set
		{
			this.m_goColosseumKill = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goColosseumKill);
			}
		}
	}

	public GameObject ColosseumCount
	{
		get
		{
			return this.m_goColosseumCount;
		}
		set
		{
			this.m_goColosseumCount = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goColosseumCount);
			}
		}
	}

	public GameObject ColosseumCritical
	{
		get
		{
			return this.m_goColosseumCritical;
		}
		set
		{
			this.m_goColosseumCritical = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goColosseumCritical);
			}
		}
	}

	public GameObject ColosseumRecall
	{
		get
		{
			return this.m_goColosseumRecall;
		}
		set
		{
			this.m_goColosseumRecall = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goColosseumRecall);
			}
		}
	}

	public bool SlowMotion
	{
		get
		{
			return this.m_bSlowMotion;
		}
	}

	public float EnableOrderTime
	{
		get
		{
			return this.m_fEnableOrderTime;
		}
		set
		{
			this.m_fEnableOrderTime = value;
		}
	}

	public bool IsEnableOrderTime
	{
		get
		{
			return this.m_fEnableOrderTime == 0f || this.m_fEnableOrderTime < Time.realtimeSinceStartup;
		}
	}

	public short BabelAdvantageCharUnique
	{
		get
		{
			return this.m_nBabelAdvantageCharUnique;
		}
		set
		{
			this.m_nBabelAdvantageCharUnique = value;
		}
	}

	public bool IsEmotionSet
	{
		get
		{
			return this.m_bEmotionSet;
		}
		set
		{
			this.m_bEmotionSet = value;
			Battle_Babel_CharinfoDlg battle_Babel_CharinfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_BABEL_CHARINFO_DLG) as Battle_Babel_CharinfoDlg;
			if (battle_Babel_CharinfoDlg != null)
			{
				battle_Babel_CharinfoDlg.SetEmoticonText(this.m_bEmotionSet);
			}
			this.GRID_MANAGER.BabelTower_Battle_Grid_Update();
		}
	}

	public eBATTLE_EMOTICON SetEmoticon
	{
		get
		{
			return this.m_eSetEmoticon;
		}
		set
		{
			this.m_eSetEmoticon = value;
		}
	}

	public List<int> BabelCharKind
	{
		get
		{
			return this.m_lsBabelCharKind;
		}
	}

	public List<long> BabelBattleSolList
	{
		get
		{
			return this.m_babelBattleSolList;
		}
	}

	public static float PlayAddRate
	{
		get
		{
			return Battle.m_fPlayAddRate;
		}
		set
		{
			Battle.m_fPlayAddRate = value;
		}
	}

	public eBATTLE_ORDER REQUEST_ORDER
	{
		get
		{
			return this.m_eRequestOrder;
		}
		set
		{
			this.m_eRequestOrder = value;
			this.m_GridMgr.InitPreTargetIndex();
			this.m_GridMgr.SetGridChangePosMode(value == eBATTLE_ORDER.eBATTLE_ORDER_CHANGEPOS, this.m_eMyAlly, this.MyStartPosIndex);
			if (value == eBATTLE_ORDER.eBATTLE_ORDER_SKILL)
			{
				Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
				if (battle_Control_Dlg != null)
				{
					battle_Control_Dlg.UpdateSelectSkillObject();
				}
			}
		}
	}

	public bool TurnOverRequest
	{
		get
		{
			return this.m_bTurnOverRequest;
		}
		set
		{
			this.m_bTurnOverRequest = value;
		}
	}

	public void Dispose()
	{
		if (this.m_pkBattleInput != null)
		{
			NrTSingleton<NrMainSystem>.Instance.GetInputManager().RemoveInputCommandLayer(this.m_pkBattleInput);
		}
		Battle.BattleClient = null;
	}

	public NkBattleChar GetCurrentSelectChar()
	{
		for (int i = 0; i < this.m_SelectCharList.Count; i++)
		{
			short buid = this.m_SelectCharList[i];
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(buid);
			if (charByBUID != null && charByBUID.GetTurnState() == eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
			{
				return charByBUID;
			}
		}
		return null;
	}

	public int GetScenarioUnique()
	{
		return this.m_nScenarioUnique;
	}

	public void SetBattleRoomState(eBATTLE_ROOM_STATE eState)
	{
		this.m_eBattleState = eState;
	}

	public eBATTLE_ROOM_STATE GetBattleRoomState()
	{
		return this.m_eBattleState;
	}

	public void Init()
	{
		Battle.BattleClient = this;
		this.SetBattleRoomState(eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_NONE);
		this.m_PacketQueue = new Queue<PacketClientOrder>();
		this.m_PacketQueue.Clear();
		this.m_eBattleRoomtype = eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NONE;
		this.m_nScenarioUnique = -1;
		this.m_stBattleInfo.m_BattleMapId = -1;
		this.m_stBattleInfo.m_BFID = -1;
		this.m_MakeCharList = new List<BATTLE_SOLDIER_INFO>();
		this.m_Camera = new NrBattleCamera();
		this.m_Camera.Init();
		this.m_MyCharBUID = new List<short>();
		this.m_SelectCharList = new List<short>();
		this.m_ListGetItemDlg = new List<GetItemDlg>();
		this.m_bObserver = false;
		this.m_bStop = false;
		this.m_bInputControlTrigger = false;
		this.m_goFirstTurnEffectObject = null;
		this.m_BattlePosGrid = new Dictionary<eBATTLE_ALLY, Dictionary<int, BATTLE_POS_GRID>>();
		this.m_fServerBattleTime = 0f;
		this.m_fServerTimeSetTime = 0f;
		this.m_nChangeSolCount = 0;
		this.m_nDieSolCount = 0;
		for (int i = 0; i < 2; i++)
		{
			this.m_BattlePosGrid.Add((eBATTLE_ALLY)i, new Dictionary<int, BATTLE_POS_GRID>());
		}
		this.m_bExitReservation = false;
		this.m_pkBattleInput = new Battle_Input(this);
		NrTSingleton<NkBattleCharManager>.Instance.Init();
		NrTSingleton<NkCharManager>.Instance.SetChildActive(false);
		NrTSingleton<NrMainSystem>.Instance.GetInputManager().AddInputCommandLayer(this.m_pkBattleInput);
		this.m_bCameraRotate = false;
		this.m_fCameraRotateEnd = 0f;
		this.m_bCameraMove = false;
		this.m_fCameraMoveEnd = 0f;
		Time.timeScale = 1f + Battle.PlayAddRate;
		this.m_fEnableOrderTime = 0f;
		this.m_bEmotionSet = false;
		this.m_eSetEmoticon = eBATTLE_EMOTICON.eBATTLE_EMOTICON_MAX;
		this.m_nBabelAdvantageCharUnique = 0;
		this.m_bRelogin = false;
		this.m_arDamageExpension = new ArrayList();
		this.m_arDamage = new NkBattleDamage[15];
		for (int i = 0; i < 15; i++)
		{
			this.m_arDamage[i] = new NkBattleDamage();
		}
		this.m_bSolCombinationDirectionIsEnd = false;
		this.m_bUseEmergencyHelpByThisRound = false;
		this.m_bUseEmergencyHelpByThisTurn = false;
	}

	public void SetCameraRotate(bool bRotate)
	{
		if (!bRotate && this.m_bCameraRotate)
		{
			this.m_fCameraRotateEnd = Time.realtimeSinceStartup + 0.3f;
		}
		this.m_bCameraRotate = bRotate;
	}

	public void SetCameraMove(bool bMove)
	{
		if (!bMove && this.m_bCameraMove)
		{
			this.m_fCameraMoveEnd = Time.realtimeSinceStartup + 0.1f;
		}
		this.m_bCameraMove = bMove;
	}

	public bool IsEnableMouseInput()
	{
		return !this.m_bCameraRotate && this.m_fCameraRotateEnd <= Time.realtimeSinceStartup && !this.m_bCameraMove && this.m_fCameraMoveEnd <= Time.realtimeSinceStartup;
	}

	public Dictionary<int, BATTLE_POS_GRID> GetBattleGrid(eBATTLE_ALLY eAlly)
	{
		if (this.m_BattlePosGrid.ContainsKey(eAlly))
		{
			return this.m_BattlePosGrid[eAlly];
		}
		return null;
	}

	public BATTLE_POS_GRID GetBattleGrid(eBATTLE_ALLY eAlly, short nStartPosindex)
	{
		Dictionary<int, BATTLE_POS_GRID> battleGrid = this.GetBattleGrid(eAlly);
		if (battleGrid == null)
		{
			return null;
		}
		if (battleGrid.ContainsKey((int)nStartPosindex))
		{
			return battleGrid[(int)nStartPosindex];
		}
		return null;
	}

	public void InitBattleGrid()
	{
		for (int i = 0; i < 2; i++)
		{
			this.m_BattlePosGrid[(eBATTLE_ALLY)i].Clear();
		}
	}

	public bool IsSetBattleInfo()
	{
		return this.m_bSetbattleInfo;
	}

	public bool IsCompleteLoadBattleMap()
	{
		if (this.m_BattleMap == null)
		{
			return false;
		}
		if (!this.m_BattleMap.IsLoadCompleted())
		{
			return false;
		}
		if (this.GetBattleRoomState() == eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_WAIT_CONTINUE && this.m_fContinueBattleWaitTime != 0f && this.m_fContinueBattleWaitTime > Time.realtimeSinceStartup)
		{
			return false;
		}
		GameObject gameObject = GameObject.Find("battle_terrain");
		if (gameObject == null)
		{
			return false;
		}
		if (Battle.ms_currentBattleMapPath == string.Empty)
		{
			return false;
		}
		this.m_BattleMap.SetTerrainGameObject(gameObject);
		this.m_bLoadCompleteTerrain = true;
		if (NrTSingleton<NkAutoRelogin>.Instance.AutoReloginState == NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_DETECT_DISCONNECT && BaseNet_Game.GetInstance().IsSocketConnected())
		{
			for (int i = 0; i < this.m_MakeCharList.Count; i++)
			{
				BATTLE_SOLDIER_INFO info = this.m_MakeCharList[i];
				this.MakeBattleChar(info);
			}
		}
		this.m_MakeCharList.Clear();
		TsAudioAdapterBGM[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapterBGM)) as TsAudioAdapterBGM[];
		TsAudioAdapterBGM[] array2 = array;
		for (int j = 0; j < array2.Length; j++)
		{
			TsAudioAdapterBGM tsAudioAdapterBGM = array2[j];
			if (tsAudioAdapterBGM != null)
			{
				string text = tsAudioAdapterBGM.gameObject.name.ToLower();
				if (text.Contains("bgm") && text.Contains("battle"))
				{
					this.m_BGMAudio = tsAudioAdapterBGM.GetAudioEx();
					if (this.m_iBGMIndex != null && this.m_BGMAudio != null && this.m_BGMAudio.baseData != null)
					{
						this.m_BGMAudio.PlayOnDownload = false;
						this.m_BGMAudio.baseData.IsManualDecideMode = true;
						if (this.m_BGMAudio.RequestDownload_SelectiveAudioBundles(this.m_iBGMIndex))
						{
							this.m_iStartBGMIndex = this.m_iBGMIndex[0];
						}
						TsLog.Log("Battle BGMGO[{0}] StartBGMIndex[{1}/{2}]", new object[]
						{
							tsAudioAdapterBGM.name,
							this.m_iStartBGMIndex,
							tsAudioAdapterBGM.GetAudioEx().baseData.BundleInfoCount
						});
					}
				}
			}
		}
		return true;
	}

	public bool IsAllCharLoadComplete()
	{
		if (!NrTSingleton<NkBattleCharManager>.Instance.IsAllCharLoadComplete())
		{
			return false;
		}
		if (this.GetBattleRoomState() == eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_WAIT_CONTINUE || this.m_bRelogin)
		{
			return true;
		}
		this.SetBattleRoomState(eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_READY);
		this.m_GridMgr.Init();
		this.ClearSelectCharList();
		this.SelectMyFirstChar();
		if (!this.m_Camera.SetBattleCamera(ref this.m_BattleMap))
		{
			return false;
		}
		if (!this.m_bObserver && !Battle.Replay && this.m_eBattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER && this.m_eBattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY && this.m_eBattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PREVIEW)
		{
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			if (this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_0) != null && this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_0).Count != 0)
			{
				vector = this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_0)[0].GetCenter();
			}
			if (this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_1) != null && this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_1).Count != 0)
			{
				vector2 = this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_1)[0].GetCenter();
			}
			Vector3 pos = Vector3.zero;
			if (vector2 != Vector3.zero && vector != Vector3.zero)
			{
				pos = (vector + vector2) / 2f;
			}
			else if (vector != Vector3.zero)
			{
				pos = vector;
			}
			else if (vector2 != Vector3.zero)
			{
				pos = vector2;
			}
			else
			{
				pos = this.m_BattleMap.GetBattleMapCenterPos();
			}
			pos.y = this.m_BattleMap.GetBattleMapHeight(pos);
			this.m_Camera.SetcameraPos(pos);
		}
		else
		{
			if (!NrTSingleton<NkBattleReplayManager>.Instance.m_bHiddenEnemyName)
			{
				this.m_eMyAlly = eBATTLE_ALLY.eBATTLE_ALLY_0;
			}
			Vector3 a = Vector3.zero;
			Vector3 b = Vector3.zero;
			if (this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_0) != null)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_0, (short)i) != null)
					{
						a = this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_0, (short)i).GetCenter();
						break;
					}
				}
			}
			if (this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_1) != null)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_1, (short)i) != null)
					{
						b = this.GetBattleGrid(eBATTLE_ALLY.eBATTLE_ALLY_1, (short)i).GetCenter();
						break;
					}
				}
			}
			Vector3 pos2 = (a + b) / 2f;
			pos2.y = this.m_BattleMap.GetBattleMapHeight(pos2);
			this.m_Camera.SetcameraPos(pos2);
		}
		this.m_Camera.CameraDataRestore();
		if (this.m_PreLoadingCharKindList != null && this.m_PreLoadingCharKindList.Count > 0)
		{
			for (int j = 0; j < this.m_PreLoadingCharKindList.Count; j++)
			{
				int num = this.m_PreLoadingCharKindList[j];
				if (num > 0)
				{
					NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(num);
					if (charKindInfo != null)
					{
						string path = "Char/" + charKindInfo.GetBundlePath();
						NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(path, NkBundleCallBack.BattlePreLoadingChar, new NkBundleParam.funcParamBundleCallBack(this.LoadPreLoadingChar), charKindInfo, false);
					}
				}
			}
		}
		NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
		NkBattleChar[] array = charArray;
		for (int k = 0; k < array.Length; k++)
		{
			NkBattleChar nkBattleChar = array[k];
			if (nkBattleChar != null)
			{
				string hitEffectCode = nkBattleChar.GetCharKindInfo().GetHitEffectCode();
				NrTSingleton<NkEffectManager>.Instance.RequestEffectBundle(hitEffectCode, true);
				if (nkBattleChar.GetJobType() != 1 && nkBattleChar.GetJobType() != 2)
				{
					BULLET_INFO bulletInfo = NrTSingleton<NkBulletManager>.Instance.GetBulletInfo(nkBattleChar.GetSoldierInfo().GetAttackInfo().BulletCode);
					if (bulletInfo != null)
					{
						NrTSingleton<NkEffectManager>.Instance.RequestEffectBundle(bulletInfo.EFFECT_KIND, true);
					}
				}
			}
		}
		return true;
	}

	public void AllCharSetPosition()
	{
		if (this.GetBattleRoomState() == eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_WAIT_CONTINUE)
		{
			if (!this.m_bRestartMakeChar)
			{
				this.m_GridMgr.Init();
				NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
				NkBattleChar[] array = charArray;
				for (int i = 0; i < array.Length; i++)
				{
					NkBattleChar nkBattleChar = array[i];
					if (nkBattleChar != null && nkBattleChar.IsMonster)
					{
						Vector3 zero = Vector3.zero;
						nkBattleChar.GetGridPosCenter(ref zero);
						nkBattleChar.AddAstarPath(zero.x, zero.y, zero.z, eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_NORMAL);
						nkBattleChar.MoveServerAStar();
						nkBattleChar.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_MOVE);
						nkBattleChar.SetComeBackRotate(true);
					}
				}
				this.m_bRestartMakeChar = true;
			}
			return;
		}
		NkBattleChar[] charArray2 = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
		NkBattleChar[] array2 = charArray2;
		for (int j = 0; j < array2.Length; j++)
		{
			NkBattleChar nkBattleChar2 = array2[j];
			if (nkBattleChar2 != null && !nkBattleChar2.IsMonster)
			{
				Vector3 zero2 = Vector3.zero;
				nkBattleChar2.GetGridPosCenter(ref zero2);
				nkBattleChar2.AddAstarPath(zero2.x, zero2.y, zero2.z, eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_NORMAL);
				nkBattleChar2.MoveServerAStar();
				nkBattleChar2.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_MOVE);
				nkBattleChar2.SetComeBackRotate(true);
			}
		}
	}

	public bool IsAllOrderComplete()
	{
		if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
		{
			return true;
		}
		NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
		NkBattleChar[] array = charArray;
		for (int i = 0; i < array.Length; i++)
		{
			NkBattleChar nkBattleChar = array[i];
			if (nkBattleChar != null && nkBattleChar.GetCurrentOrder() != eBATTLE_ORDER.eBATTLE_ORDER_NONE)
			{
				return false;
			}
		}
		return true;
	}

	public List<int> GetBattleCharOwnKindList()
	{
		NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
		if (charArray == null)
		{
			return null;
		}
		List<int> list = new List<int>();
		NkBattleChar[] array = charArray;
		for (int i = 0; i < array.Length; i++)
		{
			NkBattleChar nkBattleChar = array[i];
			if (nkBattleChar != null)
			{
				if (nkBattleChar.GetCharKindType() == eCharKindType.CKT_USER || nkBattleChar.GetCharKindType() == eCharKindType.CKT_SOLDIER)
				{
					if (nkBattleChar.Ally == eBATTLE_ALLY.eBATTLE_ALLY_0)
					{
						list.Add(nkBattleChar.GetCharKindInfo().GetCharKind());
					}
				}
			}
		}
		return list;
	}

	public void IsShowFirstTurnEffectFinish()
	{
		if (this.m_bShowFirstTurnAllyEffect)
		{
			if (this.m_goFirstTurnEffectObject == null)
			{
				this.m_goFirstTurnEffectObject = EffectDefine.Attach(string.Format("UI_{0}", "VSMODE"));
				Vector2 v = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 position = GUICamera.ScreenToGUIPoint(v);
				position.y = -position.y;
				position.z = 75f;
				NrTSingleton<NkEffectManager>.Instance.AddEffect("FX_COIN", this.m_goFirstTurnEffectObject, new NkEffectUnit.DeleteCallBack(this.FirstTurnEffectDeleteCallBack));
				this.m_goFirstTurnEffectObject.layer = TsLayer.GUI;
				this.m_goFirstTurnEffectObject.transform.position = position;
				this.m_goFirstTurnEffectObject.SetActive(true);
				Transform child = NkUtil.GetChild(this.m_goFirstTurnEffectObject.transform, "fx_coin");
				if (child != null)
				{
					GameObject gameObject = child.gameObject;
					Animation component = gameObject.GetComponent<Animation>();
					if (component != null)
					{
						if (component.isPlaying)
						{
							component.Stop();
						}
						if (this.m_eCurrentTurnAlly == this.m_eMyAlly)
						{
							component.Play("coin_user");
						}
						else
						{
							component.Play("coin_enemy");
						}
					}
				}
			}
			return;
		}
		this.Send_GS_LOAD_COMPLETE_REQ(1);
	}

	public void FirstTurnEffectDeleteCallBack()
	{
		if (this.m_goFirstTurnEffectObject != null)
		{
			UnityEngine.Object.Destroy(this.m_goFirstTurnEffectObject);
		}
		this.Send_GS_LOAD_COMPLETE_REQ(1);
	}

	public void MakeBattleChar(BATTLE_SOLDIER_INFO _Info)
	{
		bool flag = false;
		NkBattleChar nkBattleChar = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(_Info.BUID);
		if (nkBattleChar != null)
		{
			if (nkBattleChar.GetCharKindInfo().GetCharKind() != _Info.CharKind)
			{
				NrTSingleton<NkBattleCharManager>.Instance.DeleteChar(nkBattleChar.GetID());
			}
			else if (nkBattleChar.GetIDInfo().m_nCharUnique != _Info.CharUnique)
			{
				NrTSingleton<NkBattleCharManager>.Instance.DeleteChar(nkBattleChar.GetID());
			}
			else if (nkBattleChar.Get3DCharStep() == NkBattleChar.e3DCharStep.DIED)
			{
				NrTSingleton<NkBattleCharManager>.Instance.DeleteChar(nkBattleChar.GetID());
			}
			flag = true;
		}
		int id = NrTSingleton<NkBattleCharManager>.Instance.SetChar(_Info);
		nkBattleChar = NrTSingleton<NkBattleCharManager>.Instance.GetChar(id);
		if (nkBattleChar != null)
		{
			if (NkCutScene_Camera_Manager.Instance.ReservationCharKind != 0 && _Info.CharKind == NkCutScene_Camera_Manager.Instance.ReservationCharKind)
			{
				nkBattleChar.ReservationCurSceneCamera = true;
				NkCutScene_Camera_Manager.Instance.ReservationCharKind = 0;
			}
			float battleMapHeight = this.m_BattleMap.GetBattleMapHeight(new Vector3(_Info.CharPos.x, 0f, _Info.CharPos.z));
			nkBattleChar.GetPersonInfo().SetCharPos(_Info.CharPos.x, battleMapHeight + 0.3f, _Info.CharPos.z);
			nkBattleChar.Ally = (eBATTLE_ALLY)_Info.Ally;
			if ((_Info.BattleCharATB & 64) == 0)
			{
				Dictionary<int, BATTLE_POS_GRID> battleGrid = this.GetBattleGrid(nkBattleChar.Ally);
				BATTLE_POS_GRID bATTLE_POS_GRID;
				if (!battleGrid.ContainsKey((int)_Info.nStartPosIndex))
				{
					bATTLE_POS_GRID = new BATTLE_POS_GRID();
					BATTLE_POS_GRID info = BASE_BATTLE_POS_Manager.GetInstance().GetInfo((int)_Info.nGridID);
					bATTLE_POS_GRID.Set(info, _Info.nGridRotate, this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY);
					bATTLE_POS_GRID.SetCenter(new Vector3(_Info.GridStartPos.x, battleMapHeight + 0.3f, _Info.GridStartPos.z));
					bATTLE_POS_GRID.nCharUnique = _Info.CharUnique;
					battleGrid.Add((int)_Info.nStartPosIndex, bATTLE_POS_GRID);
				}
				else if (battleGrid[(int)_Info.nStartPosIndex].GRID_ID != (int)_Info.nGridID)
				{
					battleGrid.Remove((int)_Info.nStartPosIndex);
					bATTLE_POS_GRID = new BATTLE_POS_GRID();
					BATTLE_POS_GRID info2 = BASE_BATTLE_POS_Manager.GetInstance().GetInfo((int)_Info.nGridID);
					bATTLE_POS_GRID.Set(info2, _Info.nGridRotate, this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY);
					bATTLE_POS_GRID.SetCenter(new Vector3(_Info.GridStartPos.x, battleMapHeight + 0.3f, _Info.GridStartPos.z));
					bATTLE_POS_GRID.nCharUnique = _Info.CharUnique;
					battleGrid.Add((int)_Info.nStartPosIndex, bATTLE_POS_GRID);
				}
				else
				{
					bATTLE_POS_GRID = battleGrid[(int)_Info.nStartPosIndex];
				}
				bATTLE_POS_GRID.SetBUID(nkBattleChar.GetBUID(), _Info.nGridPos, nkBattleChar.GetCharKindInfo().GetBattleSizeX(), nkBattleChar.GetCharKindInfo().GetBattleSizeY());
			}
			nkBattleChar.SetSolID(_Info.SolID);
			nkBattleChar.SetStartPosIndex((short)_Info.nStartPosIndex);
			nkBattleChar.SetGridPos((short)_Info.nGridPos);
			nkBattleChar.SetGridRotate((float)_Info.nGridRotate);
			nkBattleChar.AddHP = _Info.HP_Max - nkBattleChar.GetMaxHP(true);
			nkBattleChar.GetSoldierInfo().SetHP(_Info.HP, nkBattleChar.AddHP);
			nkBattleChar.InitBattleCharATB();
			nkBattleChar.SetBattleCharATB(_Info.BattleCharATB);
			nkBattleChar.BattleCharType = _Info.CharType;
			if (!this.m_bRelogin)
			{
				if (this.m_nTurnCount == 0)
				{
					if (!nkBattleChar.IsMonster && !nkBattleChar.IsChangeSoldier && !nkBattleChar.IsFriend && !nkBattleChar.IsDefenceObject && !nkBattleChar.IsReviveChar)
					{
						nkBattleChar.GetPersonInfo().SetCharPos(nkBattleChar.GetCenterBack());
					}
				}
				else if (this.GetBattleRoomState() == eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_WAIT_CONTINUE && nkBattleChar.IsMonster)
				{
					nkBattleChar.GetPersonInfo().SetCharPos(nkBattleChar.GetCenterBack());
				}
				if (nkBattleChar.IsFriend && !flag)
				{
					if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COMMUNITY_DLG))
					{
						NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COMMUNITY_DLG);
					}
					NrTSingleton<NkEffectManager>.Instance.AddEffect("HIT_REBIRTH", nkBattleChar.GetCharPos());
				}
				if (nkBattleChar.IsChangeSoldier && !flag)
				{
					if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCT_SELECT_DLG))
					{
						NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_EMERGENCT_SELECT_DLG);
					}
					if (this.Observer)
					{
						Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_SUMMON_DLG);
						if (form != null)
						{
							form.Close();
						}
						ColosseumObserverControlDlg colosseumObserverControlDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
						if (colosseumObserverControlDlg != null)
						{
							colosseumObserverControlDlg.SetSummonEffect(_Info.CharKind);
						}
					}
					NrTSingleton<NkEffectManager>.Instance.AddEffect("HIT_REBIRTH", nkBattleChar.GetCharPos());
				}
				if (this.ColosseumObserver)
				{
					ColosseumObserverControlDlg colosseumObserverControlDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
					if (colosseumObserverControlDlg2 != null)
					{
						colosseumObserverControlDlg2.MakeBattleCharInfo(nkBattleChar.GetBUID());
					}
				}
				if (nkBattleChar.IsReviveChar && !flag)
				{
					NrTSingleton<NkEffectManager>.Instance.AddEffect("HIT_REBIRTH", nkBattleChar.GetCharPos());
				}
			}
			if (nkBattleChar.GetCharKindInfo().GetCharKind() == 916 && this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER && !Battle.m_bReplay)
			{
				PlunderGoldDlg plunderGoldDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDER_GOLD_DLG) as PlunderGoldDlg;
				if (plunderGoldDlg != null)
				{
					plunderGoldDlg.SetTreasureChar(_Info.nStartPosIndex, nkBattleChar);
					plunderGoldDlg.Show();
				}
			}
			if (this.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || this.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS || this.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT || this.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID || this.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_DAILYDUNGEON)
			{
				if (this.m_lsBabelCharKind == null)
				{
					this.m_lsBabelCharKind = new List<int>();
				}
				if (this.m_babelBattleSolList == null)
				{
					this.m_babelBattleSolList = new List<long>();
				}
				if (nkBattleChar.Ally == eBATTLE_ALLY.eBATTLE_ALLY_0)
				{
					if (!this.m_lsBabelCharKind.Contains(nkBattleChar.GetCharKindInfo().GetCharKind()))
					{
						this.m_lsBabelCharKind.Add(nkBattleChar.GetCharKindInfo().GetCharKind());
					}
					if (!this.m_babelBattleSolList.Contains(nkBattleChar.GetSolID()))
					{
						this.m_babelBattleSolList.Add(nkBattleChar.GetSolID());
					}
				}
			}
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser != null && nrCharUser.GetCharUnique() == _Info.CharUnique && !NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
			{
				if (!this.m_MyCharBUID.Contains(nkBattleChar.GetBUID()))
				{
					this.m_MyCharBUID.Add(nkBattleChar.GetBUID());
					this.m_eMyAlly = (eBATTLE_ALLY)_Info.Ally;
					this.m_nMyStartPosIndex = (short)_Info.nStartPosIndex;
					if (this.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_WAIT_CONTINUE)
					{
						nkBattleChar.GetSoldierInfo().SetHP(_Info.HP, nkBattleChar.AddHP);
						nkBattleChar.UpdateHPDlg();
					}
					else
					{
						nkBattleChar.UpdateHpDlgForRestart(_Info.HP);
					}
				}
				else if (nkBattleChar.GetSoldierInfo().GetHP() == 0 && _Info.HP > 0)
				{
					nkBattleChar.GetSoldierInfo().SetHP(_Info.HP, nkBattleChar.AddHP);
				}
				if (nkBattleChar.IsChangeSoldier && !flag)
				{
					if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCY_CALL_DLG))
					{
						NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_EMERGENCY_CALL_DLG);
					}
					this.m_nChangeSolCount++;
					this.m_bUseEmergencyHelpByThisRound = true;
					this.m_bUseEmergencyHelpByThisTurn = true;
				}
				if (nkBattleChar.IsReviveChar)
				{
					Battle_SkilldescDlg battle_SkilldescDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_SKILLDESC_DLG) as Battle_SkilldescDlg;
					if (battle_SkilldescDlg != null && nkBattleChar != null)
					{
						battle_SkilldescDlg.AddReviveCount(nkBattleChar.GetSolID());
						if (nkBattleChar.GetCharObject() != null)
						{
							Transform child = NkUtil.GetChild(nkBattleChar.GetCharObject().transform, "actioncam");
							if (null != child)
							{
								child.gameObject.SetActive(false);
							}
						}
					}
				}
			}
		}
	}

	private void InitBattleInfo()
	{
		this.LoadBattleMapCellInfo();
		NrTSingleton<NkBattleCharManager>.Instance.Init();
		if (Battle.m_bReplay)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_REPLAY_DLG);
		}
		if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER && !Battle.m_bReplay)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_GOLD_DLG);
		}
		if (this.m_PreLoadingCharKindList != null)
		{
			this.m_PreLoadingCharKindList.Clear();
		}
		this.m_PreLoadingCharKindList = null;
	}

	public bool LoadBattleMap()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (this.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
		{
			kMyCharInfo.SetAutoBattle(E_BF_AUTO_TYPE.MANUAL);
		}
		Battle.DeleteBattleMap();
		this.m_nBattleMapIDX = this.m_stBattleInfo.m_BattleMapId;
		return true;
	}

	public static void DeleteBattleMap()
	{
		Battle.ms_currentBattleMapPath = string.Empty;
		Battle.ms_downloadingBatteMapPath = string.Empty;
		TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.BattleScene);
	}

	public static bool IsDownLoadingMap()
	{
		return (Battle.ms_downloadingBatteMapPath != string.Empty && Battle.ms_currentBattleMapPath == string.Empty) || (Battle.ms_downloadingBatteMapPath == string.Empty && Battle.ms_currentBattleMapPath == string.Empty && false);
	}

	[DebuggerHidden]
	public static IEnumerator DownloadBattleMap(AStage stage, BATTLE_MAP BASEMAP, TsSceneSwitcher.ESceneType eSceneType)
	{
		Battle.<DownloadBattleMap>c__Iterator3 <DownloadBattleMap>c__Iterator = new Battle.<DownloadBattleMap>c__Iterator3();
		<DownloadBattleMap>c__Iterator.BASEMAP = BASEMAP;
		<DownloadBattleMap>c__Iterator.<$>BASEMAP = BASEMAP;
		return <DownloadBattleMap>c__Iterator;
	}

	public static void OnDownloadBattleMap(object obj)
	{
		GameObject gameObject = TsSceneSwitcher.Instance._GetBundle_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
		if (gameObject == null)
		{
			return;
		}
		Transform child = NkUtil.GetChild(gameObject.transform, "battle_terrain");
		if (child != null)
		{
			child.gameObject.layer = TsLayer.TERRAIN;
		}
		else
		{
			TsPlatform.FileLog("Battle Battle_Terrain Problem");
		}
		bool flag = true;
		if (TsSceneSwitcher.Instance.CurrentSceneType != TsSceneSwitcher.ESceneType.BattleScene)
		{
			flag = false;
		}
		TsSceneSwitcher.Instance.CollectAllMapGameObjects(TsSceneSwitcher.ESceneType.BattleScene, flag);
		NkUtil.SetAllChildActive(gameObject, flag);
		Battle.ms_currentBattleMapPath = Battle.ms_downloadingBatteMapPath;
		Battle.ms_downloadingBatteMapPath = string.Empty;
	}

	public void LoadBattleMapCellInfo()
	{
		BASE_BATTLE_MAP_Manager instance = BASE_BATTLE_MAP_Manager.GetInstance();
		BATTLE_MAP info = instance.GetInfo(this.m_stBattleInfo.m_BattleMapId);
		if (this.m_BattleMap == null && info != null)
		{
			this.m_BattleMap = new NrBattleMap();
			this.m_BattleMap.Init(info);
		}
	}

	public NrBattleMap GetBattleMap()
	{
		return this.m_BattleMap;
	}

	public void CloseBattle()
	{
		NrTSingleton<NrMainSystem>.Instance.GetInputManager().RemoveInputCommandLayer(this.m_pkBattleInput);
		this.m_pkBattleInput = null;
		this.m_GridMgr.Delete();
		NrTSingleton<NkBulletManager>.Instance.RemoveAllBullet();
		if (Battle.Replay && (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE || this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_EXPEDITION))
		{
			this.m_bObserver = true;
		}
		if (this.m_bObserver || this.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PREVIEW)
		{
			NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
		}
		NrTSingleton<NkCharManager>.Instance.SetChildActive(true);
		NrTSingleton<FormsManager>.Instance.Hide(G_ID.BATTLE_CONTROL_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_BOSSAGGRO_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_SWAPMODE_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_COUNT_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_REPLAY_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDER_GOLD_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_COLOSSEUM_CHARINFO_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_EMERGENCY_CALL_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_SKILLDESC_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWER_REPEAT_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWEXPLORATION_REPEAT_DLG);
		if (!this.m_bObserver)
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
				if (soldierInfo != null)
				{
					if (soldierInfo.GetSolID() > 0L)
					{
						soldierInfo.SetHP(soldierInfo.GetMaxHP(), 0);
					}
				}
			}
		}
		Time.timeScale = 1f;
		Battle.m_bReplay = false;
	}

	public void ClearStaticValue()
	{
		this.BattleCamera.CloseBattle();
		GameObject gameObject = TsSceneSwitcher.Instance._GetBundle_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
		if (gameObject != null)
		{
			Transform child = NkUtil.GetChild(gameObject.transform, "@BattlePreCharLoading");
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		if (this.m_goCameraTarget != null)
		{
			UnityEngine.Object.Destroy(this.m_goCameraTarget);
		}
		this.m_goCameraTarget = null;
		if (this.m_goDamageEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goDamageEffect);
		}
		this.m_goDamageEffect = null;
		if (this.m_goResultEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goResultEffect);
		}
		this.m_goResultEffect = null;
		if (this.m_goMoveArrow != null)
		{
			UnityEngine.Object.Destroy(this.m_goMoveArrow);
		}
		this.m_goMoveArrow = null;
		if (this.m_goSkillDirecting != null)
		{
			UnityEngine.Object.Destroy(this.m_goSkillDirecting);
		}
		this.m_goSkillDirecting = null;
		if (this.m_goSkillRivalDirecting != null)
		{
			UnityEngine.Object.Destroy(this.m_goSkillRivalDirecting);
		}
		this.m_goSkillRivalDirecting = null;
		if (this.m_goControlAngergauge != null)
		{
			UnityEngine.Object.Destroy(this.m_goControlAngergauge);
		}
		this.m_goControlAngergauge = null;
		if (this.m_goColosseumCardSummons != null)
		{
			UnityEngine.Object.Destroy(this.m_goColosseumCardSummons);
		}
		this.m_goColosseumCardSummons = null;
		if (this.m_goColosseumKill != null)
		{
			UnityEngine.Object.Destroy(this.m_goColosseumKill);
		}
		this.m_goColosseumKill = null;
		if (this.m_goColosseumCount != null)
		{
			UnityEngine.Object.Destroy(this.m_goColosseumCount);
		}
		this.m_goColosseumCount = null;
		if (this.m_goColosseumCritical != null)
		{
			UnityEngine.Object.Destroy(this.m_goColosseumCritical);
		}
		this.m_goColosseumCritical = null;
		if (this.m_goColosseumRecall != null)
		{
			UnityEngine.Object.Destroy(this.m_goColosseumRecall);
		}
		this.m_goColosseumRecall = null;
		NrTSingleton<NkBundleCallBack>.Instance.ClearBattlePreLoadingCharStack();
		Battle.BattleClient = null;
		for (int i = 0; i < 15; i++)
		{
			this.m_arDamage[i].Close();
			this.m_arDamage[i] = null;
		}
		int count = this.m_arDamageExpension.Count;
		for (int i = 0; i < count; i++)
		{
			NkBattleDamage nkBattleDamage = this.m_arDamageExpension[i] as NkBattleDamage;
			nkBattleDamage.Close();
		}
		this.m_arDamageExpension.Clear();
		if (NkBattleDamage.goDamageParent != null)
		{
			UnityEngine.Object.Destroy(NkBattleDamage.goDamageParent);
			NkBattleDamage.goDamageParent = null;
		}
	}

	public void SelectCharacter(short nBUID)
	{
		if (Battle.Replay)
		{
			return;
		}
		if (this.m_SelectCharList.Contains(nBUID))
		{
			Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
			if (battle_Control_Dlg != null)
			{
				battle_Control_Dlg.UpdateBattleSkillData();
			}
			this.GRID_MANAGER.ActiveAttackGridCanTarget();
			return;
		}
		this.ClearSelectCharList();
		if (!this.m_SelectCharList.Contains(nBUID))
		{
			this.m_SelectCharList.Add(nBUID);
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(nBUID);
			if (charByBUID != null)
			{
				if (charByBUID.IsNpcMode())
				{
					return;
				}
				charByBUID.SetSelect(true);
			}
			this.m_nSelectCharIndex = this.GetMyCharListIndex(nBUID);
			this.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
			Battle_Control_Dlg battle_Control_Dlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
			if (battle_Control_Dlg2 != null)
			{
				battle_Control_Dlg2.UpdateBattleSkillData();
			}
			this.Init_BattleSkill_Input(false);
			this.GRID_MANAGER.ActiveAttackGridCanTarget();
		}
	}

	public void SelectCharacterFromSolIndex(long SolID)
	{
		if (SolID < 0L)
		{
			return;
		}
		for (int i = 0; i < this.m_MyCharBUID.Count; i++)
		{
			if (this.m_MyCharBUID[i] >= 0)
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_MyCharBUID[i]);
				if (charByBUID != null && charByBUID.GetSolID() == SolID)
				{
					this.ClearSelectCharList();
					this.SelectCharacter(charByBUID.GetBUID());
				}
			}
		}
	}

	public void ClearSelectCharList()
	{
		for (int i = 0; i < this.m_SelectCharList.Count; i++)
		{
			short buid = this.m_SelectCharList[i];
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(buid);
			if (charByBUID != null)
			{
				charByBUID.SetSelect(false);
			}
		}
		this.m_SelectCharList.Clear();
	}

	public void AllSelectMyChar()
	{
	}

	public bool IsMyChar(short nBUID)
	{
		if (nBUID < 0)
		{
			return false;
		}
		for (int i = 0; i < this.m_MyCharBUID.Count; i++)
		{
			if (this.m_MyCharBUID[i] >= 0)
			{
				if (this.m_MyCharBUID[i] == nBUID)
				{
					return true;
				}
			}
		}
		return false;
	}

	public NkBattleChar GetMyFirstBattleChar()
	{
		return NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_MyCharBUID[0]);
	}

	public void SelectMyFirstChar()
	{
		for (int i = 0; i < this.m_MyCharBUID.Count; i++)
		{
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_MyCharBUID[i]);
			if (charByBUID != null && charByBUID.GetSoldierInfo().GetHP() > 0 && charByBUID.GetTurnState() == eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
			{
				this.SelectCharacter(this.m_MyCharBUID[i]);
				break;
			}
		}
	}

	public void SelectNextChar()
	{
		if (this.Observer)
		{
			return;
		}
		for (int i = 1; i < this.m_MyCharBUID.Count; i++)
		{
			int index = (this.m_nSelectCharIndex + i) % this.m_MyCharBUID.Count;
			short num = this.m_MyCharBUID[index];
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(num);
			if (charByBUID != null)
			{
				if (charByBUID.GetTurnState() == eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
				{
					if (!charByBUID.IsNpcMode())
					{
						this.SelectCharacter(num);
						break;
					}
				}
			}
		}
	}

	public int GetMyCharListIndex(short nBUid)
	{
		int result = -1;
		for (int i = 0; i < this.m_MyCharBUID.Count; i++)
		{
			if (this.m_MyCharBUID[i] == nBUid)
			{
				result = i;
			}
			else
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_MyCharBUID[i]);
				if (charByBUID != null)
				{
					charByBUID.SetSelect(false);
				}
			}
		}
		return result;
	}

	public void SetCharRotate()
	{
		NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
		if (charArray == null)
		{
			return;
		}
		for (int i = 0; i < charArray.Length; i++)
		{
			NkBattleChar nkBattleChar = charArray[i];
			if (nkBattleChar != null)
			{
				nkBattleChar.SetRotate(nkBattleChar.GetGridRotate());
			}
		}
	}

	private void LoadPreLoadingChar(ref IDownloadedItem wItem, object obj)
	{
		if (wItem.isCanceled)
		{
			return;
		}
		if (!wItem.canAccessAssetBundle)
		{
			return;
		}
	}

	public void InitBattleGrid(short nCharUnique)
	{
		for (int i = 0; i < 2; i++)
		{
			if (this.m_BattlePosGrid.ContainsKey((eBATTLE_ALLY)i))
			{
				Dictionary<int, BATTLE_POS_GRID> dictionary = this.m_BattlePosGrid[(eBATTLE_ALLY)i];
				if (dictionary != null)
				{
					foreach (KeyValuePair<int, BATTLE_POS_GRID> current in dictionary)
					{
						if (current.Value != null && current.Value.nCharUnique == nCharUnique)
						{
							dictionary.Remove(current.Key);
							return;
						}
					}
				}
			}
		}
	}

	public void ExitUser(short nCharUnique)
	{
		if (this.m_eBattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER && this.m_eBattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT && this.m_eBattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			this.InitBattleGrid(nCharUnique);
		}
		List<int> list = new List<int>();
		if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
		{
			eBATTLE_ALLY eBATTLE_ALLY = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;
			NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
			for (int i = 0; i < charArray.Length; i++)
			{
				NkBattleChar nkBattleChar = charArray[i];
				if (nkBattleChar != null && nkBattleChar.GetIDInfo().m_nCharUnique == nCharUnique)
				{
					eBATTLE_ALLY = nkBattleChar.Ally;
					break;
				}
			}
			if (eBATTLE_ALLY != eBATTLE_ALLY.eBATTLE_ALLY_INVALID)
			{
				NkBattleChar[] charArray2 = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
				for (int j = 0; j < charArray2.Length; j++)
				{
					NkBattleChar nkBattleChar2 = charArray2[j];
					if (nkBattleChar2 != null && nkBattleChar2.Ally == eBATTLE_ALLY)
					{
						if (nkBattleChar2.IsLastAttacker)
						{
							this.BattleCamera.SetLastAttackCamera(null, false);
						}
						list.Add(nkBattleChar2.GetID());
					}
				}
			}
		}
		else
		{
			NkBattleChar[] charArray3 = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
			for (int k = 0; k < charArray3.Length; k++)
			{
				NkBattleChar nkBattleChar3 = charArray3[k];
				if (nkBattleChar3 != null && nkBattleChar3.GetIDInfo().m_nCharUnique == nCharUnique)
				{
					list.Add(nkBattleChar3.GetID());
					if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT || this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
					{
						BATTLE_POS_GRID battleGrid = this.GetBattleGrid(nkBattleChar3.Ally, nkBattleChar3.GetStartPosIndex());
						battleGrid.RemoveBUID(nkBattleChar3.GetBUID());
						this.GRID_MANAGER.RemoveBUID(nkBattleChar3.Ally, nkBattleChar3.GetStartPosIndex(), nkBattleChar3.GetBUID());
					}
				}
			}
		}
		foreach (int current in list)
		{
			NrTSingleton<NkBattleCharManager>.Instance.DeleteChar(current);
		}
	}

	public void RemoveGetItemDlg(GetItemDlg dlg)
	{
		this.m_ListGetItemDlg.Remove(dlg);
		for (int i = 0; i < this.m_ListGetItemDlg.Count; i++)
		{
			this.m_ListGetItemDlg[i].SetIndex(i);
		}
	}

	public void SetSlowMotion(bool bSet, float fTime, float fRate)
	{
		if (bSet == this.m_bSlowMotion)
		{
			return;
		}
		if (bSet)
		{
			this.m_fSlowStartTime = Time.time + fTime;
			Time.timeScale = fRate;
		}
		else
		{
			Time.timeScale = 1f + Battle.PlayAddRate;
			this.m_fSlowStartTime = 0f;
			if (Battle.Replay)
			{
				Battle_ReplayDlg battle_ReplayDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_REPLAY_DLG) as Battle_ReplayDlg;
				if (battle_ReplayDlg != null)
				{
					Time.timeScale = battle_ReplayDlg.Speed;
				}
			}
		}
		this.m_bSlowMotion = bSet;
	}

	public float BattleTimeLagFromServer(float fServerTime)
	{
		if (Battle.m_bReplay)
		{
			return 0f;
		}
		if (this.m_fServerBattleTime == 0f)
		{
			return 0f;
		}
		return this.m_fServerBattleTime + (Time.realtimeSinceStartup - this.m_fServerTimeSetTime) - fServerTime;
	}

	public void PlayBossBGM()
	{
		if (this.m_BGMAudio != null)
		{
			int bundleInfoCount = this.m_BGMAudio.baseData.BundleInfoCount;
			if (bundleInfoCount >= 10)
			{
				int bundleIndex = UnityEngine.Random.Range(7, 10);
				this.m_BGMAudio.Stop();
				this.m_BGMAudio.PlayByManual(bundleIndex);
			}
			else
			{
				this.m_BGMAudio.Stop();
				this.m_BGMAudio.PlayByManual(bundleInfoCount - 1);
			}
		}
	}

	public void PlayTutorialEndBGM()
	{
		if (this.m_BGMAudio != null)
		{
			this.m_BGMAudio.Stop();
			this.m_BGMAudio.PlayByManual(1);
		}
	}

	public void BATTLE_RECONNECT_PROCESS(GS_BATTLE_RECONNECT_SOLDIERINFO[] PosList)
	{
		for (int i = 0; i < PosList.Length; i++)
		{
			short nBUID = PosList[i].m_nBUID;
			if (nBUID > -1)
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(nBUID);
				if (charByBUID != null)
				{
					charByBUID.SetTurnState((eBATTLE_TURN_STATE)PosList[i].m_nTurnState);
				}
			}
		}
	}

	public void BATTLE_RECONNECT_PROCESS(BATTLE_SOLDIER_INFO[] PosList)
	{
		this.InitBattleGrid();
		this.m_MyCharBUID.Clear();
		this.m_bRelogin = true;
		for (int i = 0; i < PosList.Length; i++)
		{
			short bUID = PosList[i].BUID;
			if (bUID > -1)
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(bUID);
				if (charByBUID == null && PosList[i].CharKind > 0 && PosList[i].HP > 0)
				{
					this.MakeBattleChar(PosList[i]);
				}
				if (this.ColosseumObserver)
				{
					ColosseumObserverControlDlg colosseumObserverControlDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
					if (colosseumObserverControlDlg != null)
					{
						colosseumObserverControlDlg.MakeBattleCharInfo(charByBUID.GetBUID());
					}
				}
			}
		}
		this.m_GridMgr.Init();
		NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
		for (int i = 0; i < charArray.Length; i++)
		{
			if (charArray[i] != null)
			{
				if (charArray[i].GetBUID() > -1)
				{
					if (charArray[i].GetSoldierInfo() != null)
					{
						bool flag = true;
						for (int j = 0; j < PosList.Length; j++)
						{
							if (PosList[j].BUID == charArray[i].GetBUID())
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							NrTSingleton<NkBattleCharManager>.Instance.DeleteChar(charArray[i].GetID());
						}
					}
				}
			}
		}
	}

	public void CreateDamageEffectFromReservationArray()
	{
		for (int i = 0; i < 15; i++)
		{
			if (this.m_arDamage[i] != null)
			{
				this.m_arDamage[i].CreateDamageEffectFromReservationArray(i);
			}
		}
	}

	public void PushBattleDamage(NkBattleChar pkTarget, int nDamage, bool bCritical, int nAngerlyPoint, int nInfoNum)
	{
		bool flag = false;
		for (int i = 0; i < 15; i++)
		{
			if (!this.m_arDamage[i].SetData)
			{
				this.m_arDamage[i].Set(pkTarget, nDamage, bCritical, nAngerlyPoint, nInfoNum);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			NkBattleDamage nkBattleDamage = new NkBattleDamage();
			nkBattleDamage.Set(pkTarget, nDamage, bCritical, nAngerlyPoint, nInfoNum);
			this.m_arDamageExpension.Add(nkBattleDamage);
		}
	}

	public void UpdateDamage()
	{
		for (int i = 0; i < 15; i++)
		{
			this.m_arDamage[i].Update();
		}
		int count = this.m_arDamageExpension.Count;
		for (int i = 0; i < count; i++)
		{
			((NkBattleDamage)this.m_arDamageExpension[i]).Update();
		}
		for (int i = 0; i < count; i++)
		{
			if (!((NkBattleDamage)this.m_arDamageExpension[i]).SetData)
			{
				NkBattleDamage nkBattleDamage = this.m_arDamageExpension[i] as NkBattleDamage;
				nkBattleDamage.Close();
				this.m_arDamageExpension.RemoveAt(i);
				break;
			}
		}
	}

	public void ShowSolCombinationDirection()
	{
		if (this.IsSkip_SolCombinationDirection())
		{
			this.SolCombinationIsEnd();
			return;
		}
		SolCombinationDirection_Dlg solCombinationDirection_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMBINATION_DIRECTION_DLG) as SolCombinationDirection_Dlg;
		if (solCombinationDirection_Dlg == null)
		{
			UnityEngine.Debug.LogError("ERROR, Battle.cs, ShowSolCombinationDirection(), SolCombinationDirection_Dlg is Null");
			return;
		}
		solCombinationDirection_Dlg.SetDirectionEndCallback(new SolCombinationDirection_Dlg.DirectionEndCallback(this.SolCombinationIsEnd));
	}

	public bool IsSolCombinationDirectionEnd()
	{
		return this.m_bSolCombinationDirectionIsEnd;
	}

	public void SolCombinationIsEnd()
	{
		this.m_bSolCombinationDirectionIsEnd = true;
	}

	private bool IsSkip_SolCombinationDirection()
	{
		return Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_EXPEDITION;
	}

	private void MythRaidEmergencyHelpUICheck()
	{
		if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCY_CALL_DLG))
		{
			return;
		}
		Form form = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_EMERGENCY_CALL_DLG);
		if (form == null)
		{
			return;
		}
		form.Show();
	}

	public NkBattleChar SelectBattleSkillChar()
	{
		NkBattleChar nkBattleChar = null;
		if (this.m_SelectCharList.Count > 0)
		{
			for (int i = 0; i < this.m_SelectCharList.Count; i++)
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_SelectCharList[i]);
				if (charByBUID != null)
				{
					if (charByBUID != null && (nkBattleChar == null || charByBUID.GetBUID() > nkBattleChar.GetBUID()))
					{
						nkBattleChar = charByBUID;
					}
				}
			}
		}
		return nkBattleChar;
	}

	public bool CanSelecActionBattleSkill(int BattleSkillUnique)
	{
		NkBattleChar nkBattleChar = this.SelectBattleSkillChar();
		if (nkBattleChar == null)
		{
			return false;
		}
		if (BattleSkillUnique > 0)
		{
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(BattleSkillUnique);
			int battleSkillLevelByUnique = nkBattleChar.GetBattleSkillLevelByUnique(BattleSkillUnique);
			BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(BattleSkillUnique, battleSkillLevelByUnique);
			if (battleSkillBase != null && battleSkillDetail != null)
			{
				if (battleSkillBase.m_nSkillType == 2)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("470"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					return false;
				}
				Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
				if (battle_Control_Dlg != null && !battle_Control_Dlg.CheckBattleSkillUseAble(BattleSkillUnique, battleSkillDetail.m_nSkillNeedAngerlyPoint))
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("572"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					return false;
				}
				if (nkBattleChar.IsBattleCharATB(32))
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("393"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					return false;
				}
			}
			return true;
		}
		Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("469"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		return false;
	}

	public void ShowBattleSkillRange(bool bshow, int BattleSkillUnique)
	{
		NkBattleChar currentSelectChar = this.GetCurrentSelectChar();
		if (currentSelectChar != null && bshow)
		{
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(BattleSkillUnique);
			int battleSkillLevelByUnique = currentSelectChar.GetBattleSkillLevelByUnique(BattleSkillUnique);
			BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(BattleSkillUnique, battleSkillLevelByUnique);
			if (battleSkillBase != null && battleSkillBase != null)
			{
				if (battleSkillBase.m_nSkillType == 2)
				{
					this.Init_BattleSkill_Input(true);
					this.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
				}
				else
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_SKILL_NAME_DLG);
					Battle_Control_Dlg battle_Control_Dlg = (Battle_Control_Dlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG);
					if (battle_Control_Dlg == null)
					{
						return;
					}
					if (battle_Control_Dlg.CheckBattleSkillUseAble(BattleSkillUnique, battleSkillDetail.m_nSkillNeedAngerlyPoint))
					{
						int num = 0;
						bool closeUpdate = false;
						if (battleSkillBase.m_nSkillTargetType == 1)
						{
							num = 1;
							closeUpdate = true;
						}
						else if (battleSkillBase.m_nSkillGridType == 6)
						{
							num = 2;
							closeUpdate = true;
						}
						Battle_Skill_Name_Dlg battle_Skill_Name_Dlg = (Battle_Skill_Name_Dlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_SKILL_NAME_DLG);
						battle_Skill_Name_Dlg.SetMagic(currentSelectChar, BattleSkillUnique);
						battle_Skill_Name_Dlg.SetCloseUpdate(closeUpdate);
						if (num == 1)
						{
							this.GRID_MANAGER.ActiveBattleSkillGridCanTarget(currentSelectChar, battleSkillBase, battleSkillDetail);
							this.Send_BattleSkill_Order(this.m_iBattleSkillIndex, this.SelectBattleSkillChar(), currentSelectChar, currentSelectChar.GetCharPos(), currentSelectChar.GetGridPos());
							this.Init_BattleSkill_Input(false);
							this.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
							return;
						}
						if (num != 2)
						{
							this.GRID_MANAGER.ActiveBattleSkillGridCanTarget(currentSelectChar, battleSkillBase, battleSkillDetail);
							return;
						}
						eBATTLE_ALLY eAlly = eBATTLE_ALLY.eBATTLE_ALLY_1;
						bool flag = false;
						switch (battleSkillBase.m_nSkillTargetType)
						{
						case 1:
						case 2:
							eAlly = ((this.MyAlly != eBATTLE_ALLY.eBATTLE_ALLY_0) ? eBATTLE_ALLY.eBATTLE_ALLY_1 : eBATTLE_ALLY.eBATTLE_ALLY_0);
							break;
						case 3:
							eAlly = ((this.MyAlly != eBATTLE_ALLY.eBATTLE_ALLY_0) ? eBATTLE_ALLY.eBATTLE_ALLY_0 : eBATTLE_ALLY.eBATTLE_ALLY_1);
							break;
						case 4:
							flag = true;
							break;
						}
						if (!flag)
						{
							NkBattleChar nkBattleChar = NrTSingleton<NkBattleCharManager>.Instance.SelectBattleSkillChar_GRID_ALL(currentSelectChar, BattleSkillUnique, eAlly);
							if (nkBattleChar != null)
							{
								this.GRID_MANAGER.ActiveBattleSkillGridCanTarget(currentSelectChar, battleSkillBase, battleSkillDetail);
								this.Send_BattleSkill_Order(this.m_iBattleSkillIndex, currentSelectChar, nkBattleChar, nkBattleChar.GetCharPos(), nkBattleChar.GetGridPos());
								this.Init_BattleSkill_Input(false);
								this.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
								return;
							}
							if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
							{
								Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("576"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
							}
						}
						else
						{
							for (int i = 0; i < 2; i++)
							{
								NkBattleChar nkBattleChar2 = NrTSingleton<NkBattleCharManager>.Instance.SelectBattleSkillChar_GRID_ALL(currentSelectChar, BattleSkillUnique, (eBATTLE_ALLY)i);
								if (nkBattleChar2 != null)
								{
									this.GRID_MANAGER.ActiveBattleSkillGridCanTarget(currentSelectChar, battleSkillBase, battleSkillDetail);
									this.Send_BattleSkill_Order(this.m_iBattleSkillIndex, currentSelectChar, nkBattleChar2, nkBattleChar2.GetCharPos(), nkBattleChar2.GetGridPos());
									this.Init_BattleSkill_Input(false);
									this.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
									return;
								}
							}
							if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
							{
								Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("576"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
							}
						}
					}
				}
			}
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_SKILL_NAME_DLG);
		this.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
		this.Init_BattleSkill_Input(true);
	}

	public void Init_BattleSkill_Input(bool bCancle)
	{
		if (this.m_iBattleSkillIndex > -1)
		{
			Battle_Skill_Name_Dlg battle_Skill_Name_Dlg = (Battle_Skill_Name_Dlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_SKILL_NAME_DLG);
			if (battle_Skill_Name_Dlg != null)
			{
				battle_Skill_Name_Dlg.SetCloseUpdate(true);
			}
			if (bCancle && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("403"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			Battle_Control_Dlg battle_Control_Dlg = (Battle_Control_Dlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG);
			if (battle_Control_Dlg != null)
			{
				battle_Control_Dlg.SetAngergaugeFX_Click(false);
			}
		}
		this.m_iBattleSkillIndex = -1;
		this.m_GridMgr.InitAll();
	}

	public void ClickCheckTargetBt()
	{
		if (!this.m_bCheckTargetBt)
		{
			this.m_bCheckTargetBt = true;
		}
		else
		{
			this.m_bCheckTargetBt = false;
		}
	}

	public bool GetCheckTargetBt()
	{
		return this.m_bCheckTargetBt;
	}

	public void SetTargetBtCount(int TargetBtCount)
	{
		this.m_nTargetBtCount = TargetBtCount;
	}

	public void SetTargetBtDisCount()
	{
		if (this.m_nTargetBtCount > 0)
		{
			this.m_nTargetBtCount--;
		}
	}

	public int GetTargetBtCount()
	{
		return this.m_nTargetBtCount;
	}

	public void InPacket(PacketClientOrder _Order)
	{
		this.m_PacketQueue.Enqueue(_Order);
		this.UpdatePacketProcess();
	}

	public void UpdatePacket()
	{
		if (0 < this.m_PacketQueue.Count)
		{
			PacketClientOrder packetClientOrder = this.m_PacketQueue.Dequeue();
			MethodInfo method = packetClientOrder.Method;
			if (method != null)
			{
				try
				{
					method.Invoke(this, packetClientOrder.Parameters);
				}
				catch (Exception ex)
				{
					if (ex.InnerException != null)
					{
						Exception innerException = ex.InnerException;
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendFormat("{0} : {1}", method.Name, innerException.Message);
						stringBuilder.AppendFormat("{0} : {1}", method.Name, innerException.Source);
						stringBuilder.AppendFormat("{0} : {1}", method.Name, innerException.StackTrace);
					}
					packetClientOrder.DEBUG_LOG();
				}
			}
			else
			{
				packetClientOrder.DEBUG_LOG();
			}
		}
	}

	public void GS_BATTLE_INFO_NFY(GS_BATTLE_INFO_NFY _InfoACK)
	{
		this.m_stBattleInfo.m_BattleMapId = _InfoACK.MAP_ID;
		this.m_eBattleRoomtype = (eBATTLE_ROOMTYPE)_InfoACK.i8BattleRoomType;
		this.m_stBattleInfo.m_BFID = _InfoACK.BFRoomID;
		this.m_nScenarioUnique = _InfoACK.i32ScenarioUnique;
		this.m_bObserver = ((int)_InfoACK.Observer != 0);
		this.m_bSetbattleInfo = true;
		if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER)
		{
		}
		if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
		{
			this.m_stBattleInfo.m_MAXCHANGESOLDIERNUM = 3;
			if (this.m_bObserver)
			{
				this.ColosseumObserver = true;
			}
			else
			{
				this.ColosseumObserver = false;
			}
		}
		else if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			this.m_stBattleInfo.m_MAXCHANGESOLDIERNUM = (int)BATTLE_CONSTANT_Manager.GetInstance().GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_EMERGENCY_COUNT);
			MYTHRAIDINFO_DATA mythRaidInfoData = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidInfoData(NrTSingleton<MythRaidManager>.Instance.GetMyInfo().nRaidSeason.ToString() + NrTSingleton<MythRaidManager>.Instance.GetMyInfo().nRaidType.ToString());
			if (mythRaidInfoData == null)
			{
				UnityEngine.Debug.LogError("raid_info.ndt error");
				return;
			}
			this.BossMaxHP = mythRaidInfoData.i64BossHP;
		}
		else
		{
			this.m_stBattleInfo.m_MAXCHANGESOLDIERNUM = 1;
		}
		this.SetBattleRoomState(eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_OPEN);
		this.InitBattleInfo();
		if (this.m_PreLoadingCharKindList == null)
		{
			this.m_PreLoadingCharKindList = new List<int>();
		}
		for (int i = 0; i < _InfoACK.nPreLoadingKind.Length; i++)
		{
			if (_InfoACK.nPreLoadingKind[i] > 0)
			{
				this.m_PreLoadingCharKindList.Add(_InfoACK.nPreLoadingKind[i]);
			}
		}
		this.m_eCurrentTurnAlly = (eBATTLE_ALLY)_InfoACK.i8CurrentTurnAlly;
		this.m_bShowFirstTurnAllyEffect = _InfoACK.bShowStartTurnAllyEffect;
		float num = (float)_InfoACK.i32BattlePlayRate / 100f;
		Battle.PlayAddRate = num - 1f;
		if (Battle.PlayAddRate <= 0f)
		{
			Battle.PlayAddRate = 0f;
		}
	}

	public void GS_BATTLE_SOLDIER_LIST_NFY(BATTLE_SOLDIER_INFO[] _Infos)
	{
		for (int i = 0; i < _Infos.Length; i++)
		{
			BATTLE_SOLDIER_INFO bATTLE_SOLDIER_INFO = _Infos[i];
			if (!this.m_bLoadCompleteTerrain || this.GetBattleRoomState() == eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_WAIT_CONTINUE)
			{
				this.m_MakeCharList.Add(bATTLE_SOLDIER_INFO);
			}
			else
			{
				this.MakeBattleChar(bATTLE_SOLDIER_INFO);
			}
		}
		if (this.GetBattleRoomState() == eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			this.m_GridMgr.Init();
		}
		if (1 < this.m_nTurnCount && this.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_WAIT_CONTINUE)
		{
			NrSound.ImmedatePlay("UI_SFX", "BATTLE", "ALLYJOIN");
		}
		else
		{
			this.m_bRestartAction = true;
			this.m_bRestartMakeChar = false;
		}
	}

	public void GS_BATTLE_CLOSE_NFY(GS_BATTLE_CLOSE_NFY _ACK)
	{
		if (!NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
		{
			if (_ACK.iCloseReason == 0)
			{
				MsgHandler.Handle("Rcv_BATTLE_RESULT", new object[0]);
			}
			else if (_ACK.iCloseReason == 1)
			{
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser != null)
				{
					if (nrCharUser.GetCharUnique() != _ACK.iCharUnique)
					{
						Battle.BATTLE.ExitUser(_ACK.iCharUnique);
						return;
					}
					MsgHandler.Handle("Rcv_BATTLE_RESULT", new object[0]);
				}
			}
			else
			{
				MsgHandler.Handle("Rcv_BATTLE_RESULT", new object[0]);
			}
		}
		else
		{
			MsgHandler.Handle("Rcv_BATTLE_RESULT", new object[0]);
		}
		if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
		{
			Battle_ResultPlunderDlg battle_ResultPlunderDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_PLUNDER_DLG) as Battle_ResultPlunderDlg;
			if (battle_ResultPlunderDlg == null)
			{
				return;
			}
			battle_ResultPlunderDlg.UpdateCheck = true;
		}
		else if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
		{
			Battle_ResultTutorialDlg battle_ResultTutorialDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_TUTORIAL_DLG) as Battle_ResultTutorialDlg;
			if (battle_ResultTutorialDlg == null)
			{
				return;
			}
			battle_ResultTutorialDlg.UpdateCheck = true;
		}
		else
		{
			Battle_ResultDlg battle_ResultDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_DLG) as Battle_ResultDlg;
			if (battle_ResultDlg == null)
			{
				return;
			}
			battle_ResultDlg.UpdateCheck = true;
		}
	}

	public void GS_BF_ORDER_ACK(GS_BF_ORDER_ACK _ACK)
	{
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID((short)_ACK.iFromBFCharUnique);
		if (charByBUID == null)
		{
			return;
		}
		charByBUID.EnOrderACK(_ACK);
		charByBUID.PopCharOrder();
	}

	public void GS_BATTLE_SERVER_ERROR_NFY(GS_BATTLE_SERVER_ERROR_NFY _ACK)
	{
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(_ACK.i16BUID);
		if (charByBUID != null)
		{
			if (charByBUID.MyChar)
			{
				string text = string.Empty;
				Battle.BATTLE.Init_BattleSkill_Input(true);
				switch (_ACK.i32ServerErrorType)
				{
				case 2:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("346");
					break;
				case 3:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("1146");
					break;
				case 4:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("1149");
					break;
				case 5:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("352");
					break;
				case 6:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("351");
					break;
				case 7:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("1146");
					break;
				case 8:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("353");
					break;
				case 9:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("348");
					break;
				case 10:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("1147");
					break;
				case 11:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("393");
					break;
				case 13:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("353");
					break;
				case 15:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("571");
					break;
				case 16:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("575");
					break;
				case 17:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("353");
					break;
				case 18:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("404");
					break;
				}
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}
		else
		{
			string text2 = string.Empty;
			eBATTLE_SERVER_ERROR i32ServerErrorType = (eBATTLE_SERVER_ERROR)_ACK.i32ServerErrorType;
			if (i32ServerErrorType == eBATTLE_SERVER_ERROR.eBATTLE_AUTOBATTLE_FAIL_RESAON_ONEUSER)
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("430");
			}
			if (text2 != string.Empty)
			{
				Main_UI_SystemMessage.ADDMessage(text2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
	}

	public void GS_BF_CHARINFO_NFY(GS_BF_CHARINFO_NFY[] _CharInfoACKArray)
	{
		if (0 < _CharInfoACKArray.Length)
		{
			int iOrderUnique = _CharInfoACKArray[0].cBFOrderUnique.iOrderUnique;
			this.ADDCharInfoNFY(iOrderUnique, _CharInfoACKArray);
		}
	}

	public void GS_BF_MOVE_POS_LIST_NFY(GS_BF_MOVE_POS_LIST_NFY _OrderACK)
	{
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID((short)_OrderACK.iFromBFCharUnique);
		if (charByBUID == null)
		{
			return;
		}
		if ((int)_OrderACK.iMovePosCount <= 0)
		{
			return;
		}
		if (!this.m_bOnlyServerMove)
		{
			charByBUID.ForceStopChar(true, -1f, -1f);
			for (int i = 0; i < (int)_OrderACK.iMovePosCount; i++)
			{
				charByBUID.AddAstarPath(charByBUID.MovePath[i].Pos.x, 0f, charByBUID.MovePath[i].Pos.z, eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_NORMAL);
			}
			if ((int)_OrderACK.iBFOrderType == 3)
			{
				int num = _OrderACK.iPara[2];
				if (num >= 0 && charByBUID.isUpdateAngerlyPoint())
				{
					if (!this.ColosseumObserver)
					{
						Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
						if (battle_Control_Dlg != null)
						{
							if (!Battle.BATTLE.Observer)
							{
								if (Battle.BATTLE.MyAlly == charByBUID.Ally && Battle.BATTLE.MyStartPosIndex == charByBUID.GetStartPosIndex())
								{
									battle_Control_Dlg.SetAngerlyPoint(num);
									battle_Control_Dlg.UpdateBattleSkillData();
								}
							}
							else if (charByBUID.GetStartPosIndex() == 0 && charByBUID.Ally == Battle.BATTLE.CurrentTurnAlly)
							{
								battle_Control_Dlg.SetAngerlyPoint(num);
								battle_Control_Dlg.UpdateBattleSkillData();
							}
						}
					}
					else
					{
						ColosseumObserverControlDlg colosseumObserverControlDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
						if (colosseumObserverControlDlg != null)
						{
							colosseumObserverControlDlg.SetAngerPoint(charByBUID.Ally, num);
						}
					}
				}
			}
			if ((int)_OrderACK.iBFOrderType == 6)
			{
				charByBUID.MakeArrow();
			}
			if ((int)_OrderACK.iBFOrderType == 7 && charByBUID.GetCharKindInfo().GetCharKind() == 703)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("191"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			charByBUID.MoveServerAStar();
			charByBUID.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_MOVE);
			charByBUID.RunEffect = NrTSingleton<NkEffectManager>.Instance.AddEffect("FX_BRUN", charByBUID);
			charByBUID.SetTurnState(eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_DISABLE);
			if (_OrderACK.iPara[4] == 1)
			{
				charByBUID.SetComeBackRotate(true);
			}
			charByBUID.SetTurnState((eBATTLE_TURN_STATE)_OrderACK.nTurnState);
		}
	}

	public void GS_BATTLE_CHAR_POS_NFY(GS_BATTLE_CHAR_POS_NFY _ACK)
	{
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(_ACK.i16BUID);
		if (charByBUID == null)
		{
			return;
		}
		Vector3 path = new Vector3(_ACK.Pos.x, 0f, _ACK.Pos.z);
		if ((int)_ACK.i8Status == 0 && charByBUID.m_kCharMove.m_eMoveStatus == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_WAIT)
		{
			charByBUID.m_kCharMove.m_eMoveStatus = eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_NORMAL;
		}
		if ((int)_ACK.i8Status == 1)
		{
			charByBUID.AddAstarPath(_ACK.Pos.x, 0f, _ACK.Pos.z, (eBATTLE_MOVE_STATUS)_ACK.i8Status);
			charByBUID.MoveServerAStar();
		}
		if ((int)_ACK.i8Status == 2)
		{
			if (charByBUID.m_kCharMove.m_eMoveStatus == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_WAIT)
			{
				charByBUID.m_kCharMove.m_eMoveStatus = eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_RENEWPOS;
			}
			charByBUID.m_kCharMove.RenewMovePath(path, eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_RENEWPOS);
		}
		if ((int)_ACK.i8Status == 3)
		{
			charByBUID.m_kCharMove.RenewMovePath(path, eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_WAIT);
		}
		if ((int)_ACK.i8Status == 4)
		{
			charByBUID.m_kCharMove.m_eMoveStatus = eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_CHANGEPOS;
			charByBUID.m_kCharMove.RenewMovePath(path, eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_CHANGEPOS);
		}
	}

	public void GS_BATTLE_STOP_NFY(GS_BATTLE_STOP_NFY _NFY)
	{
		this.m_bStop = _NFY.bStop;
		Battle_CountDlg battle_CountDlg = (Battle_CountDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_COUNT_DLG);
		if (battle_CountDlg == null)
		{
			battle_CountDlg = (Battle_CountDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_COUNT_DLG);
		}
		battle_CountDlg.StopTurn(this.m_bStop);
		if (!this.m_bStop && NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_COLOSSEUM_WAIT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_COLOSSEUM_WAIT_DLG);
		}
	}

	public void GS_BF_TURN_STATE_NFY(GS_BF_TURN_STATE_NFY _NFY)
	{
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(_NFY.iBFCharUnique);
		if (charByBUID == null)
		{
			return;
		}
		bool flag = false;
		if (charByBUID.GetTurnState() == eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_DISABLE && _NFY.iBFTurnState == 0)
		{
			flag = true;
		}
		charByBUID.SetTurnState((eBATTLE_TURN_STATE)_NFY.iBFTurnState);
		if (flag)
		{
			this.m_GridMgr.InitbyAlly(charByBUID.Ally);
		}
	}

	public void GS_BF_TURNINFO_NFY(GS_BF_TURNINFO_NFY _NFY)
	{
		this.m_eCurrentTurnAlly = (eBATTLE_ALLY)_NFY.m_iCurTurnAlly;
		bool flag = this.MyAlly == this.m_eCurrentTurnAlly;
		if (flag)
		{
			Battle.BATTLE.UseEmergencyHelpByThisTurn = false;
			this.MythRaidEmergencyHelpUICheck();
		}
		NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
		for (int i = 0; i < charArray.Length; i++)
		{
			NkBattleChar nkBattleChar = charArray[i];
			if (nkBattleChar != null && nkBattleChar.m_DCharEffect != null)
			{
				nkBattleChar.m_DCharEffect.Clear();
			}
		}
		this.m_fTurnTime = _NFY.m_fTurnTime;
		this.m_nTurnCount = (int)_NFY.m_iTurnNum;
		if (!Battle.m_bReplay)
		{
			if (this.m_bColosseumObserver)
			{
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG))
				{
					NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG);
				}
			}
			else
			{
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_CONTROL_DLG))
				{
					NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_CONTROL_DLG);
				}
				if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
				{
					NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWER_REPEAT_DLG);
				}
				if (NrTSingleton<NewExplorationManager>.Instance.AutoBattle)
				{
					NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWER_REPEAT_DLG);
				}
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MYTHRAID_BATTLEINFO_DLG) && Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
				{
					Battle_MythRaidBattleInfo_DLG battle_MythRaidBattleInfo_DLG = (Battle_MythRaidBattleInfo_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTHRAID_BATTLEINFO_DLG);
					if (battle_MythRaidBattleInfo_DLG != null)
					{
						battle_MythRaidBattleInfo_DLG.UpdateRoundInfo(-1);
						battle_MythRaidBattleInfo_DLG.UpdateDamageInfo(0L);
					}
				}
				if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
				{
					Battle_GuildBossBattleInfo_DLG battle_GuildBossBattleInfo_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDBOSS_BATTLEINFO_DLG) as Battle_GuildBossBattleInfo_DLG;
					if (battle_GuildBossBattleInfo_DLG == null)
					{
						battle_GuildBossBattleInfo_DLG = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GUILDBOSS_BATTLEINFO_DLG) as Battle_GuildBossBattleInfo_DLG);
					}
					else
					{
						battle_GuildBossBattleInfo_DLG.UpdateRoundInfo((int)_NFY.m_iTurnNum);
						battle_GuildBossBattleInfo_DLG.UpdateDamageInfo(0L);
					}
				}
			}
		}
		bool flag2 = false;
		bool[] array = new bool[]
		{
			false,
			false
		};
		for (int j = 0; j < this.m_TurnActivePower.Length; j++)
		{
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_TurnActivePower[j].m_nBUID);
			if (charByBUID != null)
			{
				charByBUID.SetBattleSkillCharATB(this.m_TurnActivePower[j].m_nBattleSkillCharATB);
			}
			if (this.ColosseumObserver)
			{
				ColosseumObserverControlDlg colosseumObserverControlDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
				if (colosseumObserverControlDlg != null && !array[(int)charByBUID.Ally])
				{
					colosseumObserverControlDlg.SetAngerPoint(charByBUID.Ally, (int)this.m_TurnActivePower[j].m_nTemp);
					array[(int)charByBUID.Ally] = true;
				}
			}
			else if (this.Observer && !flag2)
			{
				Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
				if (battle_Control_Dlg != null && charByBUID.GetStartPosIndex() == 0 && charByBUID.Ally == this.CurrentTurnAlly)
				{
					battle_Control_Dlg.SetAngerlyPoint((int)this.m_TurnActivePower[j].m_nTemp);
					battle_Control_Dlg.UpdateBattleSkillData();
					flag2 = true;
				}
			}
		}
		NrTSingleton<NkBattleCharManager>.Instance.InitTurn();
		if (flag)
		{
			this.m_bTurnOverRequest = false;
		}
		Battle_CountDlg battle_CountDlg = (Battle_CountDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_COUNT_DLG);
		if (battle_CountDlg == null)
		{
			battle_CountDlg = (Battle_CountDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_COUNT_DLG);
		}
		if (battle_CountDlg != null)
		{
			battle_CountDlg.SwapTurn(flag, this.m_fTurnTime);
		}
		this.m_GridMgr.InitAll();
		if (flag)
		{
			this.SelectMyFirstChar();
		}
		else
		{
			Battle_Control_Dlg battle_Control_Dlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
			if (battle_Control_Dlg2 != null)
			{
				battle_Control_Dlg2.UpdateBattleSkillData();
			}
		}
		Battle_Skill_Name_Dlg battle_Skill_Name_Dlg = (Battle_Skill_Name_Dlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_SKILL_NAME_DLG);
		if (battle_Skill_Name_Dlg != null)
		{
			battle_Skill_Name_Dlg.SetCloseUpdate(true);
		}
		this.SetBattleRoomState(eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION);
		this.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
		if (this.m_fServerBattleTime == 0f)
		{
			this.m_fServerBattleTime = _NFY.m_fBattleRoomTime;
			this.m_fServerTimeSetTime = Time.realtimeSinceStartup;
		}
		if (Time.timeScale != 1f + Battle.PlayAddRate && !Battle.m_bReplay)
		{
			Time.timeScale = 1f + Battle.PlayAddRate;
		}
		if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			Battle_Babel_CharinfoDlg battle_Babel_CharinfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_BABEL_CHARINFO_DLG) as Battle_Babel_CharinfoDlg;
			if (battle_Babel_CharinfoDlg != null)
			{
				battle_Babel_CharinfoDlg.ChangeTurn(this.m_eCurrentTurnAlly);
			}
		}
		else if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE)
		{
			Battle_Mine_CharinfoDlg battle_Mine_CharinfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_MINE_CHARINFO_DLG) as Battle_Mine_CharinfoDlg;
			if (battle_Mine_CharinfoDlg != null)
			{
				battle_Mine_CharinfoDlg.Show();
				battle_Mine_CharinfoDlg.UPdateTurnInfo();
			}
		}
		else if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			Battle_Control_Dlg battle_Control_Dlg3 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
			if (battle_Control_Dlg3 != null)
			{
				battle_Control_Dlg3.UpdateAngelSkillData(flag);
			}
		}
		Battle_Plunder_TurnCountDlg battle_Plunder_TurnCountDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_PLUNDER_TURNCOUNT_DLG) as Battle_Plunder_TurnCountDlg;
		if (battle_Plunder_TurnCountDlg != null)
		{
			battle_Plunder_TurnCountDlg.UpdateData();
		}
		if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM && !Battle.m_bReplay)
		{
			if (this.ChangeSolCount >= this.m_stBattleInfo.m_MAXCHANGESOLDIERNUM || !flag)
			{
				Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_EMERGENCY_CALL_DLG);
				if (form != null)
				{
					form.Close();
				}
			}
		}
	}

	public void GS_BATTLE_CHANGE_POS_LIST_NFY(GS_BATTLE_CHANGE_POS[] PosList)
	{
		for (int i = 0; i < PosList.Length; i++)
		{
			short nBUID = PosList[i].nBUID;
			if (nBUID > -1)
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(nBUID);
				if (charByBUID != null)
				{
					if (!charByBUID.m_bDeadReaservation)
					{
						BATTLE_POS_GRID battleGrid = this.GetBattleGrid(charByBUID.Ally, charByBUID.GetStartPosIndex());
						battleGrid.RemoveBUID(charByBUID.GetBUID());
						this.GRID_MANAGER.RemoveBUID(charByBUID.Ally, charByBUID.GetStartPosIndex(), charByBUID.GetBUID());
					}
				}
			}
		}
		for (int i = 0; i < PosList.Length; i++)
		{
			short nBUID2 = PosList[i].nBUID;
			short nGridPos = PosList[i].nGridPos;
			if (nBUID2 > -1)
			{
				NkBattleChar charByBUID2 = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(nBUID2);
				if (charByBUID2 != null)
				{
					if (!charByBUID2.m_bDeadReaservation)
					{
						BATTLE_POS_GRID battleGrid2 = Battle.BATTLE.GetBattleGrid(charByBUID2.Ally, charByBUID2.GetStartPosIndex());
						charByBUID2.SetGridPos(nGridPos);
						battleGrid2.SetBUID(charByBUID2.GetBUID(), (byte)nGridPos, charByBUID2.GetCharKindInfo().GetBattleSizeX(), charByBUID2.GetCharKindInfo().GetBattleSizeY());
						Vector3 zero = Vector3.zero;
						battleGrid2.GetCenter(charByBUID2.GetBUID(), ref zero);
						float battleMapHeight = this.m_BattleMap.GetBattleMapHeight(new Vector3(zero.x, 0f, zero.z));
						charByBUID2.GetPersonInfo().SetCharPos(zero.x, battleMapHeight + 0.3f, zero.z);
					}
				}
			}
		}
		this.GRID_MANAGER.Init();
	}

	public void GS_BATTLE_RESTART_NFY(GS_BATTLE_RESTART_NFY _NFY)
	{
		Battle.BATTLE.UseEmergencyHelpByThisRound = false;
		this.MythRaidEmergencyHelpUICheck();
		this.InitBattleGrid();
		this.m_GridMgr.Init();
		this.m_MyCharBUID.Clear();
		NrTSingleton<NkBattleCharManager>.Instance.DeleteAllCharByAlly((eBATTLE_ALLY)_NFY.nEnemyAlly);
		NrTSingleton<NkBattleCharManager>.Instance.DeleteDeadChar();
		this.SetBattleRoomState(eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_WAIT_CONTINUE);
		this.m_bRestartAction = false;
		this.m_bRestartMakeChar = false;
		int nCount = (int)_NFY.nContinueCount + 1;
		Battle_CountDlg battle_CountDlg = (Battle_CountDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_COUNT_DLG);
		if (battle_CountDlg == null)
		{
			battle_CountDlg = (Battle_CountDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_COUNT_DLG);
		}
		if (battle_CountDlg != null)
		{
			battle_CountDlg.SetChallengeCount(nCount, _NFY.bBoss, _NFY.nMonsterKind);
		}
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			Battle_MythRaidBattleInfo_DLG battle_MythRaidBattleInfo_DLG = (Battle_MythRaidBattleInfo_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_BATTLEINFO_DLG);
			if (battle_MythRaidBattleInfo_DLG != null)
			{
				battle_MythRaidBattleInfo_DLG.UpdateRoundInfo((int)_NFY.nContinueCount);
			}
		}
		NrTSingleton<NrMainSystem>.Instance.MemoryCleanUP();
	}

	public void GS_BATTLE_SLOW_ACK(GS_BATTLE_SLOW_ACK _ACK)
	{
		bool bSet = (int)_ACK.nSlowMotion != 0;
		this.SetSlowMotion(bSet, float.PositiveInfinity, _ACK.fRate);
	}

	public void GS_BATTLE_EMOTICON_ACK(GS_BATTLE_EMOTICON_ACK _ACK)
	{
		if (_ACK.nEmoticon < 0 || _ACK.nEmoticon >= 9)
		{
			return;
		}
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(_ACK.nBUID);
		if (charByBUID == null)
		{
			return;
		}
		BATTLE_EMOTICON data = BATTLE_EMOTICON_Manager.GetInstance().GetData((eBATTLE_EMOTICON)_ACK.nEmoticon);
		if (data != null)
		{
			NrTSingleton<NkEffectManager>.Instance.AddEffect(data.m_szEffect, charByBUID);
		}
	}

	public void GS_BATTLE_EVENT_TRIGGER_NFY(GS_BATTLE_EVENT_TRIGGER_NFY _Nfy)
	{
		BATTLETRIGGERTYPE i32Type = (BATTLETRIGGERTYPE)_Nfy.i32Type;
		if (i32Type != BATTLETRIGGERTYPE.TYPE_TRIGGER)
		{
			if (i32Type == BATTLETRIGGERTYPE.TYPE_ACTION)
			{
				this.ProcessAction((ACTIONKIND)_Nfy.i32Code, _Nfy.i32Params);
			}
		}
		else
		{
			this.ProcessTrigger((TRIGGERKIND)_Nfy.i32Code, _Nfy.i32Params);
		}
	}

	private void ProcessTrigger(TRIGGERKIND eKind, int[] i32Params)
	{
		switch (eKind)
		{
		}
	}

	private void ProcessAction(ACTIONKIND eKind, int[] i32Params)
	{
		switch (eKind)
		{
		case ACTIONKIND.ACTIONKIND_CHARERASE:
		{
			short buid = (short)i32Params[3];
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(buid);
			if (charByBUID != null)
			{
				charByBUID.SetDeleteChar(1);
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_SHOWTEXT:
		{
			int nTextIndex = i32Params[0];
			int num = i32Params[1];
			int num2 = i32Params[2];
			int num3 = i32Params[3];
			string strName = string.Empty;
			NkBattleChar nkBattleChar;
			if (num2 == 9999)
			{
				nkBattleChar = this.GetMyFirstBattleChar();
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser != null)
				{
					NrPersonInfoUser nrPersonInfoUser = nrCharUser.GetPersonInfo() as NrPersonInfoUser;
					if (nrPersonInfoUser != null)
					{
						strName = nrPersonInfoUser.GetCharName();
					}
				}
			}
			else
			{
				nkBattleChar = NrTSingleton<NkBattleCharManager>.Instance.GetCharByCharKind(num2);
				if (nkBattleChar != null)
				{
					strName = nkBattleChar.GetCharName();
				}
			}
			if (num3 == 1)
			{
				Battle_HeadUpTalk battle_HeadUpTalk = NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.BATTLE_HEADUP_TALK) as Battle_HeadUpTalk;
				if (battle_HeadUpTalk != null && nkBattleChar != null)
				{
					battle_HeadUpTalk.Set(nkBattleChar, strName, (float)num, nTextIndex);
				}
			}
			else if (num3 == 2 || num3 == 4)
			{
				Battle_TalkDlg battle_TalkDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_TALK_DLG) as Battle_TalkDlg;
				if (battle_TalkDlg != null && nkBattleChar != null)
				{
					bool bSkip = num3 == 2;
					battle_TalkDlg.Set(nkBattleChar, strName, (float)num, nTextIndex, bSkip);
				}
			}
			else if (num3 == 3)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromTBS(nTextIndex.ToString()), SYSTEM_MESSAGE_TYPE.BATTLE_SHOWTEXT_MODE4);
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_TUTORIAL:
		{
			int num4 = i32Params[0];
			int num5 = i32Params[1];
			int num6 = i32Params[2];
			int charkind = i32Params[3];
			if (num4 < 0 || num4 >= BATTLE_DEFINE.TRIGGERDLG_ID.Length)
			{
				return;
			}
			G_ID g_ID = BATTLE_DEFINE.TRIGGERDLG_ID[num4];
			if (g_ID == G_ID.UIGUIDE_DLG)
			{
				UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.LoadForm(g_ID) as UI_UIGuide;
				if (uI_UIGuide == null)
				{
					return;
				}
				NkBattleChar charByCharKind = NrTSingleton<NkBattleCharManager>.Instance.GetCharByCharKind(charkind);
				uI_UIGuide.SetBattleTutorial(num5, num6, charByCharKind);
			}
			else if (g_ID == G_ID.NONE)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.UIGUIDE_DLG);
			}
			else
			{
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_TUTORIAL_DLG))
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_TUTORIAL_DLG);
				}
				BattleTutorialDlg battleTutorialDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_TUTORIAL_DLG) as BattleTutorialDlg;
				if (battleTutorialDlg != null)
				{
					battleTutorialDlg.SetDlgData(num5, g_ID, (float)num6);
				}
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_SHOWUI:
		{
			int num7 = i32Params[0];
			int data = i32Params[1];
			if (num7 < 0 || num7 >= BATTLE_DEFINE.TRIGGERDLG_ID.Length)
			{
				return;
			}
			G_ID g_ID2 = BATTLE_DEFINE.TRIGGERDLG_ID[num7];
			if (g_ID2 == G_ID.BATTLE_PLUNDER_TURNCOUNT_DLG)
			{
				Battle_Plunder_TurnCountDlg battle_Plunder_TurnCountDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_PLUNDER_TURNCOUNT_DLG) as Battle_Plunder_TurnCountDlg;
				if (battle_Plunder_TurnCountDlg != null)
				{
					battle_Plunder_TurnCountDlg.SetData(data);
				}
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_ANGERPOINT:
		{
			int num8 = i32Params[0];
			int num9 = i32Params[1];
			int num10 = i32Params[2];
			if (!this.ColosseumObserver)
			{
				Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
				if (battle_Control_Dlg != null)
				{
					if (!Battle.BATTLE.Observer)
					{
						if (Battle.BATTLE.MyAlly == (eBATTLE_ALLY)num9 && (int)Battle.BATTLE.MyStartPosIndex == num8)
						{
							battle_Control_Dlg.AddAngerlyPoint(num10);
							battle_Control_Dlg.UpdateBattleSkillData();
						}
					}
					else if (num9 == (int)Battle.BATTLE.CurrentTurnAlly)
					{
						battle_Control_Dlg.AddAngerlyPoint(num10);
						battle_Control_Dlg.UpdateBattleSkillData();
					}
				}
			}
			else
			{
				ColosseumObserverControlDlg colosseumObserverControlDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
				if (colosseumObserverControlDlg != null)
				{
					colosseumObserverControlDlg.AddAngerPoint((eBATTLE_ALLY)num9, num10);
				}
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_INPUTCONTROL:
			if (i32Params[0] == 0)
			{
				this.m_bInputControlTrigger = true;
			}
			else
			{
				this.m_bInputControlTrigger = false;
			}
			break;
		case ACTIONKIND.ACTIONKIND_NPCMODE:
		{
			short buid2 = (short)i32Params[0];
			int num11 = i32Params[1];
			bool npcMode = num11 > 0;
			NkBattleChar charByBUID2 = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(buid2);
			if (charByBUID2 != null)
			{
				charByBUID2.SetNpcMode(npcMode);
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_CAMERAMOVETARGET:
		{
			int num12 = i32Params[0];
			int num13 = i32Params[1];
			int num14 = i32Params[2];
			NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
			if (charArray == null)
			{
				return;
			}
			for (int i = 0; i < charArray.Length; i++)
			{
				NkBattleChar nkBattleChar2 = charArray[i];
				if (nkBattleChar2 != null)
				{
					if (num12 == 9999)
					{
						if (!NrTSingleton<NrCharKindInfoManager>.Instance.IsUserCharKind(nkBattleChar2.GetCharKindInfo().GetCharKind()))
						{
							goto IL_3AF;
						}
					}
					else if (nkBattleChar2.GetCharKindInfo().GetCharKind() != num12)
					{
						goto IL_3AF;
					}
					this.BattleCamera.SetTriggerAction(nkBattleChar2.GetCharPos(), (float)num14, (float)num13);
				}
				IL_3AF:;
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_BGM:
		{
			int num15 = i32Params[0];
			int num16 = i32Params[1];
			if (0 <= num15)
			{
				float pitch = (float)num16 / 1000f;
				if (this.m_BGMAudio != null)
				{
					this.m_BGMAudio.PlayOnDownload = true;
					this.m_BGMAudio.PlayByManual(num15, pitch);
				}
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_CHARATB:
		{
			eBATTLE_ALLY eBATTLE_ALLY = (eBATTLE_ALLY)i32Params[0];
			int num17 = i32Params[1];
			int battleCharATB = i32Params[2];
			NkBattleChar[] charArray2 = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
			if (charArray2 == null)
			{
				return;
			}
			for (int j = 0; j < charArray2.Length; j++)
			{
				NkBattleChar nkBattleChar3 = charArray2[j];
				if (nkBattleChar3 != null)
				{
					if (nkBattleChar3.Ally == eBATTLE_ALLY)
					{
						if (num17 == 9999)
						{
							if (!NrTSingleton<NrCharKindInfoManager>.Instance.IsUserCharKind(nkBattleChar3.GetCharKindInfo().GetCharKind()))
							{
								goto IL_4B7;
							}
						}
						else if (nkBattleChar3.GetCharKindInfo().GetCharKind() != num17)
						{
							goto IL_4B7;
						}
						nkBattleChar3.InitBattleCharATB();
						nkBattleChar3.SetBattleCharATB(battleCharATB);
					}
				}
				IL_4B7:;
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_ACTIVEGRID:
		{
			eBATTLE_ALLY eAlly = (eBATTLE_ALLY)i32Params[0];
			short nStartPosindex = (short)i32Params[1];
			short num18 = (short)i32Params[2];
			byte nGridPos = (byte)i32Params[3];
			byte nSizeX = (byte)i32Params[4];
			byte nSizeY = (byte)i32Params[5];
			bool bActive = i32Params[6] > 0;
			BATTLE_POS_GRID battleGrid = this.GetBattleGrid(eAlly, nStartPosindex);
			if (battleGrid != null)
			{
				if (battleGrid.GRID_ID != (int)num18)
				{
					return;
				}
				battleGrid.SetActive(bActive, nGridPos, nSizeX, nSizeY);
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_CAMERA:
		{
			int num19 = i32Params[0];
			int num20 = i32Params[1];
			float x = (float)i32Params[2] / 100f;
			float y = (float)i32Params[3] / 100f;
			float z = (float)i32Params[4] / 100f;
			float fAngle = (float)i32Params[5];
			Vector3 cameraPosition = new Vector3(x, y, z);
			if (num19 == 1)
			{
				this.BattleCamera.SetCameraMode(1, cameraPosition, fAngle);
			}
			else if (num19 == 2)
			{
				this.BattleCamera.SetCameraMode(2, cameraPosition, fAngle);
			}
			else if (num19 == 3)
			{
				NkBattleChar nkBattleChar4;
				if (num20 == 9999)
				{
					nkBattleChar4 = this.GetMyFirstBattleChar();
					cameraPosition = nkBattleChar4.GetCameraPosition();
				}
				else
				{
					nkBattleChar4 = NrTSingleton<NkBattleCharManager>.Instance.GetCharByCharKind(num20);
					if (nkBattleChar4 != null)
					{
						cameraPosition = nkBattleChar4.GetCameraPosition();
					}
				}
				if (nkBattleChar4 != null)
				{
					this.BattleCamera.SetCameraMode(3, cameraPosition, fAngle);
				}
			}
			else if (num19 == 4)
			{
				this.BattleCamera.SetCameraMode(4, cameraPosition, fAngle);
			}
			break;
		}
		case ACTIONKIND.ACTIONKIND_MOVIESTART:
			UnityEngine.Debug.Log("BattleTrigger ACtion : ACTIONKIND_MOVIESTART");
			if (i32Params == null)
			{
				UnityEngine.Debug.LogError("ACTIONKIND_MOVIESTART i32Params is Null");
			}
			else
			{
				int movieUnique = i32Params[0];
				string text = NrTSingleton<NrTableMovieUrlManager>.Instance.GetMovieUrl(movieUnique);
				text = text.ToLower();
				UnityEngine.Debug.Log("BattleTrigger ACtion : ACTIONKIND_MOVIESTART szFilePath : " + text);
				string text2 = string.Format("{0}/{1}", NrTSingleton<NrGlobalReference>.Instance.basePath, text);
				UnityEngine.Debug.Log("BattleTrigger ACtion : ACTIONKIND_MOVIESTART path : " + text2);
				NrTSingleton<NrUserDeviceInfo>.Instance.PlayMovieURL(text2, Color.black, false, 1f, true);
			}
			break;
		}
	}

	public void GS_BATTLE_EVENT_ACTION_EFFECT_NFY(GS_BATTLE_EVENT_ACTION_EFFECT_NFY _Nfy)
	{
		if (_Nfy.nType == 0)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffectFromName(TKString.NEWString(_Nfy.szEffectName));
			return;
		}
		if (_Nfy.nStartPos >= 0)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.Clear();
			BATTLE_POS_GRID battleGrid = this.GetBattleGrid((eBATTLE_ALLY)_Nfy.nAlly, (short)_Nfy.nStartPos);
			if (battleGrid == null)
			{
				return;
			}
			for (int i = 0; i < battleGrid.GetGridBuidArrayLength(); i++)
			{
				if (battleGrid.GetGridBUID((sbyte)i) > -1)
				{
					NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(battleGrid.GetGridBUID((sbyte)i));
					if (charByBUID != null)
					{
						if (_Nfy.nCharKind != 0)
						{
							if (_Nfy.nCharKind == 9999)
							{
								if (!NrTSingleton<NrCharKindInfoManager>.Instance.IsUserCharKind(charByBUID.GetCharKindInfo().GetCharKind()))
								{
									goto IL_10E;
								}
							}
							else if (charByBUID.GetCharKindInfo().GetCharKind() != _Nfy.nCharKind)
							{
								goto IL_10E;
							}
						}
						if (!arrayList.Contains(charByBUID.GetBUID()))
						{
							arrayList.Add(charByBUID.GetBUID());
						}
					}
				}
				IL_10E:;
			}
			if (arrayList.Count > 0)
			{
				for (int i = 0; i < arrayList.Count; i++)
				{
					NkBattleChar charByBUID2 = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID((short)arrayList[i]);
					if (charByBUID2 != null)
					{
						Vector3 zero = Vector3.zero;
						if (charByBUID2.GetGridPosCenter(ref zero))
						{
							float battleMapHeight = this.m_BattleMap.GetBattleMapHeight(new Vector3(zero.x, 0f, zero.z));
							zero.y = battleMapHeight;
							uint num = NrTSingleton<NkEffectManager>.Instance.AddEffect(TKString.NEWString(_Nfy.szEffectCode), zero);
							if (num > 0u)
							{
								NkEffectUnit effectUnit = NrTSingleton<NkEffectManager>.Instance.GetEffectUnit(num);
								float num2 = 3f;
								if (_Nfy.nType == 2)
								{
									num2 = float.PositiveInfinity;
								}
								else if (effectUnit != null && NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectUnit.EffectName) != null)
								{
									num2 = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectUnit.EffectName).LIFE_TIME;
								}
								if (effectUnit == null)
								{
									NrTSingleton<NkEffectManager>.Instance.SetReservationEffectData(num, TKString.NEWString(_Nfy.szEffectName), num2, 0f);
								}
								else
								{
									effectUnit.EffectName = TKString.NEWString(_Nfy.szEffectName);
									if (_Nfy.nType != 2)
									{
										effectUnit.LifeTime = num2;
									}
								}
							}
						}
					}
				}
			}
		}
		else
		{
			Vector3 v3Target = new Vector3(_Nfy.fPosX, 0f, _Nfy.fPosZ);
			float battleMapHeight2 = this.m_BattleMap.GetBattleMapHeight(new Vector3(v3Target.x, 0f, v3Target.z));
			v3Target.y = battleMapHeight2;
			uint num3 = NrTSingleton<NkEffectManager>.Instance.AddEffect(TKString.NEWString(_Nfy.szEffectCode), v3Target);
			if (num3 > 0u)
			{
				NkEffectUnit effectUnit2 = NrTSingleton<NkEffectManager>.Instance.GetEffectUnit(num3);
				float num4 = 0f;
				if (_Nfy.nType == 1)
				{
					num4 = float.PositiveInfinity;
				}
				if (effectUnit2 == null)
				{
					NrTSingleton<NkEffectManager>.Instance.SetReservationEffectData(num3, TKString.NEWString(_Nfy.szEffectName), num4, 0f);
					return;
				}
				effectUnit2.EffectName = TKString.NEWString(_Nfy.szEffectName);
				if (_Nfy.nType == 1)
				{
					effectUnit2.LifeTime = num4;
				}
			}
		}
	}

	public void GS_BATTLE_EVENT_STATUS_NFY(GS_BATTLE_EVENT_STATUS_NFY _Nfy)
	{
		if (_Nfy == null)
		{
			return;
		}
	}

	public void GS_BATTLE_EVENT_ACTION_ANIMATION_NFY(GS_BATTLE_EVENT_ACTION_ANIMATION_NFY _Nfy)
	{
		if (_Nfy == null)
		{
			return;
		}
		NkBattleChar nkBattleChar;
		if (_Nfy.nCharKind == 9999)
		{
			nkBattleChar = this.GetMyFirstBattleChar();
		}
		else
		{
			nkBattleChar = NrTSingleton<NkBattleCharManager>.Instance.GetCharByCharKind(_Nfy.nCharKind);
		}
		if (nkBattleChar != null)
		{
			string aniType = TKString.NEWString(_Nfy.szAnimationType);
			eCharAnimationType charAniTypeForString = (eCharAnimationType)NrTSingleton<NrCharKindInfoManager>.Instance.GetCharDataCodeInfo().GetCharAniTypeForString(aniType);
			nkBattleChar.SetAnimation(charAniTypeForString);
		}
	}

	public void GS_BATTLE_EVENT_ACTION_CUTSCENE_CAMERA_NFY(GS_BATTLE_EVENT_ACTION_CUTSCENE_CAMERA_NFY _Nfy)
	{
		if (_Nfy == null)
		{
			return;
		}
		eCharAnimationType eAni = eCharAnimationType.None;
		int nCharKind = _Nfy.nCharKind;
		if (_Nfy.nCharKind != 0)
		{
			NkBattleChar nkBattleChar;
			if (_Nfy.nCharKind == 9999)
			{
				nkBattleChar = this.GetMyFirstBattleChar();
			}
			else
			{
				nkBattleChar = NrTSingleton<NkBattleCharManager>.Instance.GetCharByCharKind(_Nfy.nCharKind);
			}
			if (nkBattleChar != null)
			{
				nkBattleChar.ReservationCurSceneCamera = true;
				nCharKind = 0;
			}
			string aniType = TKString.NEWString(_Nfy.szAnimationType);
			eAni = (eCharAnimationType)NrTSingleton<NrCharKindInfoManager>.Instance.GetCharDataCodeInfo().GetCharAniTypeForString(aniType);
		}
		NkCutScene_Camera_Manager.Instance.ReadCutScneData(TKString.NEWString(_Nfy.szCurSceneCamera), nCharKind, eAni, _Nfy.nHide, _Nfy.nAlly);
	}

	public void Send_GS_BATTLE_READY_NFY(byte Start)
	{
		if (!Battle.m_bReplay)
		{
			GS_BATTLE_READY_NFY gS_BATTLE_READY_NFY = new GS_BATTLE_READY_NFY();
			gS_BATTLE_READY_NFY.i8Start = Start;
			SendPacket.GetInstance().SendObject(202, gS_BATTLE_READY_NFY);
		}
		else
		{
			NrTSingleton<NkBattleReplayManager>.Instance.bGS_BATTLE_INFO_NFY = true;
		}
	}

	public void Send_GS_LOAD_COMPLETE_REQ(int iParam)
	{
		if (!Battle.m_bReplay)
		{
			GS_LOAD_COMPLETED_REQ gS_LOAD_COMPLETED_REQ = new GS_LOAD_COMPLETED_REQ();
			gS_LOAD_COMPLETED_REQ.Status = 16384L;
			if (!this.m_bRelogin)
			{
				gS_LOAD_COMPLETED_REQ.i8Param = (byte)iParam;
			}
			else
			{
				gS_LOAD_COMPLETED_REQ.i8Param = 2;
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_LOAD_COMPLETED_REQ, gS_LOAD_COMPLETED_REQ);
			TsPlatform.FileLog("Send_GS_LOAD_COMPLETE_REQ Pass");
			if (this.GetBattleRoomState() == eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_WAIT_CONTINUE)
			{
				TsPlatform.FileLog("========= SEND READY FROM RESTART ================ ");
			}
			else if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NORMAL)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo == null)
				{
					return;
				}
				for (int i = 0; i < 6; i++)
				{
					NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
					if (soldierInfo != null && soldierInfo.IsValid())
					{
						if (soldierInfo.IsInjuryStatus())
						{
							string empty = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("115"),
								"sol",
								soldierInfo.GetName()
							});
							if (empty != string.Empty)
							{
								Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
							}
						}
					}
				}
			}
		}
		else
		{
			NrTSingleton<NkBattleReplayManager>.Instance.bGS_BF_TURNINFO_NFY = true;
		}
	}

	public void ChangeBattleAuto()
	{
		if (this.Observer)
		{
			return;
		}
		if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM || this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("156"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		Battle.Send_GS_BATTLE_AUTO_REQ();
	}

	public static void Send_GS_BATTLE_AUTO_REQ()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_AUTO_BATTLE_LV);
		if (kMyCharInfo.GetLevel() < value)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("151"),
				"level",
				value.ToString()
			});
			if (empty != string.Empty)
			{
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			return;
		}
		E_BF_AUTO_TYPE e_BF_AUTO_TYPE = kMyCharInfo.GetAutoBattle();
		if ((e_BF_AUTO_TYPE += 1) == E_BF_AUTO_TYPE.END)
		{
			e_BF_AUTO_TYPE = E_BF_AUTO_TYPE.MANUAL;
		}
		GS_BATTLE_AUTO_REQ gS_BATTLE_AUTO_REQ = new GS_BATTLE_AUTO_REQ();
		gS_BATTLE_AUTO_REQ.i8Mode = (byte)e_BF_AUTO_TYPE;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_AUTO_REQ, gS_BATTLE_AUTO_REQ);
	}

	public void Send_GS_BATTLE_CONTINUE_REQ()
	{
		if (this.Observer)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		E_BATTLE_CONTINUE_TYPE e_BATTLE_CONTINUE_TYPE = kMyCharInfo.GetBattleContinueType();
		if ((e_BATTLE_CONTINUE_TYPE += 1) == E_BATTLE_CONTINUE_TYPE.END)
		{
			e_BATTLE_CONTINUE_TYPE = E_BATTLE_CONTINUE_TYPE.NONE;
		}
		if (this.m_bSlowMotion && e_BATTLE_CONTINUE_TYPE == E_BATTLE_CONTINUE_TYPE.CONTINUE)
		{
			return;
		}
		GS_BATTLE_CONTINUE_REQ gS_BATTLE_CONTINUE_REQ = new GS_BATTLE_CONTINUE_REQ();
		gS_BATTLE_CONTINUE_REQ.i8Mode = (byte)e_BATTLE_CONTINUE_TYPE;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_CONTINUE_REQ, gS_BATTLE_CONTINUE_REQ);
	}

	public void Send_Move_Order(Vector3 pos)
	{
		if (this.m_bStop)
		{
			return;
		}
		if (this.CurrentTurnAlly != this.MyAlly)
		{
			return;
		}
		NkBattleChar nkBattleChar = null;
		NkBattleChar nkBattleChar2 = null;
		Vector3 vector = pos;
		if (this.m_SelectCharList.Count > 0)
		{
			for (int i = 0; i < this.m_SelectCharList.Count; i++)
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_SelectCharList[i]);
				if (charByBUID != null)
				{
					if (!charByBUID.m_bDeadReaservation)
					{
						if (charByBUID.GetSoldierInfo().GetHP() > 0)
						{
							if (nkBattleChar != null)
							{
								Vector3 a = charByBUID.GetCharPos() - nkBattleChar.GetCharPos();
								a.Normalize();
								vector += a * (nkBattleChar2.GetCharHalfBound() + charByBUID.GetCharHalfBound() + 0.5f);
								nkBattleChar2 = charByBUID;
							}
							charByBUID.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
							charByBUID.OrderMoveReq(vector, true, -1);
							if (nkBattleChar == null)
							{
								nkBattleChar = charByBUID;
								nkBattleChar2 = charByBUID;
							}
						}
					}
				}
			}
		}
	}

	public void Send_AttackLand_Order(Vector3 pos)
	{
		if (this.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL && this.m_bStop)
		{
			return;
		}
		if (this.CurrentTurnAlly != this.MyAlly)
		{
			return;
		}
		NkBattleChar nkBattleChar = null;
		NkBattleChar nkBattleChar2 = null;
		Vector3 vector = pos;
		if (this.m_SelectCharList.Count > 0)
		{
			for (int i = 0; i < this.m_SelectCharList.Count; i++)
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_SelectCharList[i]);
				if (charByBUID != null)
				{
					if (!charByBUID.m_bDeadReaservation)
					{
						if (charByBUID.GetSoldierInfo().GetHP() > 0)
						{
							if (nkBattleChar != null)
							{
								Vector3 a = charByBUID.GetCharPos() - nkBattleChar.GetCharPos();
								a.Normalize();
								vector += a * (nkBattleChar2.GetCharHalfBound() + charByBUID.GetCharHalfBound() + 0.5f);
								nkBattleChar2 = charByBUID;
							}
							charByBUID.OrderAttackLandReq(vector);
							if (nkBattleChar == null)
							{
								nkBattleChar = charByBUID;
								nkBattleChar2 = charByBUID;
							}
						}
					}
				}
			}
		}
	}

	public void Send_BattleSkill_Order(int iBattleSkillIndex, NkBattleChar pkSendChar, NkBattleChar pkTargetChar, Vector3 pos, short nGridPos)
	{
		if (this.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL && this.m_bStop)
		{
			return;
		}
		if (this.CurrentTurnAlly != this.MyAlly)
		{
			return;
		}
		if (pkSendChar != null)
		{
			pkSendChar.OrderBattleSkillReq(iBattleSkillIndex, pkTargetChar, pos, nGridPos, 0, 0);
		}
	}

	public void Send_GS_BATTLE_CLOSE_REQ()
	{
		SendPacket.GetInstance().SendIDType(217);
	}

	public void Send_GS_BATTLE_TRIGGER_SKIP_REQ()
	{
		SendPacket.GetInstance().SendIDType(281);
	}

	public void Send_GS_BATTLE_PLUNDER_AGGROADD_REQ(short nBUID)
	{
		if (nBUID < 0)
		{
			return;
		}
		GS_BATTLE_PLUNDER_AGGROADD_REQ gS_BATTLE_PLUNDER_AGGROADD_REQ = new GS_BATTLE_PLUNDER_AGGROADD_REQ();
		gS_BATTLE_PLUNDER_AGGROADD_REQ.m_nBUID = nBUID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_PLUNDER_AGGROADD_REQ, gS_BATTLE_PLUNDER_AGGROADD_REQ);
	}

	public void Send_GS_BF_HOPE_TO_ENDTURN_REQ()
	{
		if (this.Observer)
		{
			return;
		}
		if (this.CurrentTurnAlly != this.MyAlly)
		{
			return;
		}
		GS_BF_HOPE_TO_ENDTURN_REQ gS_BF_HOPE_TO_ENDTURN_REQ = new GS_BF_HOPE_TO_ENDTURN_REQ();
		int num = 0;
		for (int i = 0; i < this.m_MyCharBUID.Count; i++)
		{
			gS_BF_HOPE_TO_ENDTURN_REQ.iBFCharUnique[i] = this.m_MyCharBUID[i];
			num++;
			if (num >= 6)
			{
				break;
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BF_HOPE_TO_ENDTURN_REQ, gS_BF_HOPE_TO_ENDTURN_REQ);
		Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("363"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
		this.m_bTurnOverRequest = true;
	}

	public void Send_GS_BF_TURNOVER_REQ()
	{
		if (Time.time - Battle.m_fTurnOverRequestTime > 0.3f)
		{
			GS_BF_TURNOVER_REQ gS_BF_TURNOVER_REQ = new GS_BF_TURNOVER_REQ();
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_SelectCharList[0]);
			if (charByBUID == null)
			{
				return;
			}
			if (charByBUID.GetTurnState() != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
			{
				return;
			}
			gS_BF_TURNOVER_REQ.iBFBUID = this.m_SelectCharList[0];
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BF_TURNOVER_REQ, gS_BF_TURNOVER_REQ);
			this.SelectNextChar();
			Battle.m_fTurnOverRequestTime = Time.time;
		}
	}

	public void Send_GS_BF_ALL_SEARCH_REQ()
	{
	}

	public void Send_GS_BATTLE_EMOTICON_REQ(short nBuid)
	{
		GS_BATTLE_EMOTICON_REQ gS_BATTLE_EMOTICON_REQ = new GS_BATTLE_EMOTICON_REQ();
		gS_BATTLE_EMOTICON_REQ.nBUID = nBuid;
		gS_BATTLE_EMOTICON_REQ.nEmoticon = (byte)this.m_eSetEmoticon;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_EMOTICON_REQ, gS_BATTLE_EMOTICON_REQ);
		this.IsEmotionSet = false;
		this.m_eSetEmoticon = eBATTLE_EMOTICON.eBATTLE_EMOTICON_MAX;
	}

	public void UpdatePacketProcess()
	{
		if (this.UpdatePacketEnable)
		{
			this.UpdatePacket();
		}
	}

	public void Update()
	{
		if (Battle.m_bReplay)
		{
			NrTSingleton<NkBattleReplayManager>.Instance.ReplayUpdate();
		}
		this.UpdatePacketProcess();
		NrTSingleton<NkBulletManager>.Instance.Update();
		this.m_Camera.CameraUpdate(ref this.m_BattleMap);
		NrTSingleton<NkBattleCharManager>.Instance.Update();
		this.UpdateDamage();
		this.CheckSelectChar();
		if (this.m_bSlowMotion && Time.time > this.m_fSlowStartTime)
		{
			this.SetSlowMotion(false, 0f, 1f);
		}
		if (this.GetBattleRoomState() == eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_WAIT_CONTINUE && this.m_bRestartAction && this.IsCompleteLoadBattleMap() && this.IsAllCharLoadComplete())
		{
			this.AllCharSetPosition();
			if (this.IsAllOrderComplete())
			{
				Resources.UnloadUnusedAssets();
				this.m_bShowFirstTurnAllyEffect = false;
				this.IsShowFirstTurnEffectFinish();
				this.m_bRestartAction = false;
				this.m_fContinueBattleWaitTime = 0f;
			}
		}
		if (this.m_bRelogin && this.IsAllCharLoadComplete())
		{
			Resources.UnloadUnusedAssets();
			this.m_bShowFirstTurnAllyEffect = false;
			this.IsShowFirstTurnEffectFinish();
			this.m_bRestartAction = false;
			this.m_fContinueBattleWaitTime = 0f;
			this.m_bRelogin = false;
		}
		if (!NrTSingleton<NkBattleCharManager>.Instance.MyCharExist() && NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCY_CALL_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_EMERGENCY_CALL_DLG);
		}
	}

	public void FixedUpdate()
	{
	}

	private void FindDragAreaInMyChar()
	{
	}

	public void ADDCharInfoNFY(int _OrderUnique, GS_BF_CHARINFO_NFY[] _InfoNFY)
	{
		Queue<GS_BF_CHARINFO_NFY> queue = new Queue<GS_BF_CHARINFO_NFY>();
		for (int i = 0; i < _InfoNFY.Length; i++)
		{
			GS_BF_CHARINFO_NFY item = _InfoNFY[i];
			queue.Enqueue(item);
		}
		this.mOrderNfyTable.Add(_OrderUnique, queue);
		this.UpdateCharInfoNFY(_OrderUnique);
	}

	public bool UpdateCharInfoNFY(int _OrderUnique)
	{
		GS_BF_CHARINFO_NFY gS_BF_CHARINFO_NFY = null;
		GS_BF_CHARINFO_NFY gS_BF_CHARINFO_NFY2;
		do
		{
			gS_BF_CHARINFO_NFY2 = this.PopCharInfoNFY(_OrderUnique, out gS_BF_CHARINFO_NFY);
			if (gS_BF_CHARINFO_NFY2 != null)
			{
				this.DoGS_BF_CHARINFO_NFY(gS_BF_CHARINFO_NFY2);
			}
		}
		while (gS_BF_CHARINFO_NFY2 != null);
		return true;
	}

	private GS_BF_CHARINFO_NFY PopCharInfoNFY(int _OrderUnique, out GS_BF_CHARINFO_NFY _OutNext)
	{
		GS_BF_CHARINFO_NFY result = null;
		_OutNext = null;
		if (this.mOrderNfyTable.ContainsKey(_OrderUnique))
		{
			Queue<GS_BF_CHARINFO_NFY> queue = this.mOrderNfyTable[_OrderUnique];
			result = queue.Dequeue();
			this.mOrderNfyTable.Remove(_OrderUnique);
			if (0 < queue.Count)
			{
				this.mOrderNfyTable.Add(_OrderUnique, queue);
				_OutNext = queue.Peek();
			}
		}
		return result;
	}

	public void DoGS_BF_CHARINFO_NFY(GS_BF_CHARINFO_NFY _Nfy)
	{
		eBATTLE_CHARINFO_REASON eBATTLE_CHARINFO_REASON = (eBATTLE_CHARINFO_REASON)_Nfy.iCharInfoType;
		int iBFCharUnique = _Nfy.iBFCharUnique;
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID((short)iBFCharUnique);
		if (charByBUID == null)
		{
			return;
		}
		this.m_fInfoPing = this.BattleTimeLagFromServer(_Nfy.fServerTime);
		if (this.m_fInfoPing >= 1f)
		{
		}
		switch (eBATTLE_CHARINFO_REASON)
		{
		case eBATTLE_CHARINFO_REASON.eBATTLE_CHARINFO_REASON_INCLIFE:
		{
			int num = _Nfy.iPara[0];
			bool bCritical = _Nfy.iPara[1] > 0;
			int num2 = _Nfy.iPara[2];
			int num3 = _Nfy.iPara[3];
			int num4 = _Nfy.iPara[4];
			int num5 = _Nfy.iPara[5];
			int skillLevel = _Nfy.iPara[6];
			int num6 = _Nfy.iPara[7];
			int num7 = _Nfy.iPara[8];
			int num8 = _Nfy.iPara[9];
			NkBattleChar pkFromChar = null;
			if (num2 >= 0)
			{
				pkFromChar = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID((short)num2);
			}
			if (num8 != 0 && charByBUID.isUpdateAngerlyPoint())
			{
				if (!this.ColosseumObserver)
				{
					Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
					if (battle_Control_Dlg != null)
					{
						if (!Battle.BattleClient.Observer)
						{
							if (charByBUID.Ally == this.MyAlly && this.MyStartPosIndex == charByBUID.GetStartPosIndex())
							{
								battle_Control_Dlg.AddAngerlyPoint(num8);
								battle_Control_Dlg.UpdateBattleSkillData();
							}
						}
						else if (charByBUID.GetStartPosIndex() == 0 && charByBUID.Ally == Battle.BATTLE.CurrentTurnAlly)
						{
							battle_Control_Dlg.AddAngerlyPoint(num8);
							battle_Control_Dlg.UpdateBattleSkillData();
						}
					}
				}
				else
				{
					ColosseumObserverControlDlg colosseumObserverControlDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
					if (colosseumObserverControlDlg != null)
					{
						colosseumObserverControlDlg.AddAngerPoint(charByBUID.Ally, num8);
					}
				}
			}
			if (num3 > 0)
			{
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(num3);
				BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num3, skillLevel);
				if (battleSkillBase == null || battleSkillDetail == null)
				{
					return;
				}
				bool flag = false;
				bool bEndureDamage = false;
				if ((battleSkillDetail.m_nSkillDurationTurn > 0 && num5 > -1) || num5 == -100)
				{
					flag = true;
					if (num5 >= 0 && battleSkillDetail.m_nSkillDurationTurn != num5)
					{
						bEndureDamage = true;
					}
				}
				int num9 = 0;
				if (flag && num6 == 1)
				{
					if (battleSkillDetail.GetSkillDetalParamValue(54) != 0)
					{
						num9 = 1;
					}
					else if (battleSkillDetail.GetSkillDetalParamValue(40) != 0)
					{
						num9 = 2;
					}
					else if (battleSkillDetail.GetSkillDetalParamValue(39) != 0)
					{
						num9 = 3;
					}
					else if (battleSkillDetail.GetSkillDetalParamValue(45) != 0)
					{
						num9 = 4;
					}
				}
				if (charByBUID.IsBattleCharATB(1024))
				{
					num9 = 0;
				}
				else if (charByBUID.IsBattleCharATB(32768) && num9 != 3)
				{
					num9 = 0;
				}
				if (battleSkillDetail.CheckSkillDetailATB(4) && num > 0)
				{
					num9 = 0;
				}
				if (flag)
				{
					if (num != 0)
					{
						charByBUID.SetDamage(num, bCritical, pkFromChar, battleSkillBase.m_nSkillTargetType, num8, num9);
					}
				}
				else
				{
					charByBUID.SetDamage(num, bCritical, pkFromChar, battleSkillBase.m_nSkillTargetType, num8, num9);
				}
				if (flag && num6 == 1)
				{
					Battle_SkilldescDlg battle_SkilldescDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_SKILLDESC_DLG) as Battle_SkilldescDlg;
					if (battle_SkilldescDlg != null && charByBUID.GetPersonInfo().GetSoldierInfo(0).GetHP() > 0)
					{
						battle_SkilldescDlg.SetBuffTextShowUp(num3, null);
					}
				}
				if (num4 == iBFCharUnique && num6 == 1 && (!battleSkillDetail.CheckSkillDetailATB(4) || num <= 0))
				{
					charByBUID.SetGridEffectBattleSkill(battleSkillBase);
					charByBUID.SetHitCenterGridEffectBattleSkill(battleSkillBase);
				}
				if (battleSkillDetail.CheckSkillDetailATB(4))
				{
					if (num < 0)
					{
						charByBUID.SetHitBattleSkill(pkFromChar, battleSkillBase, battleSkillDetail, num7, bEndureDamage);
					}
				}
				else if (flag)
				{
					if (num != 0)
					{
						charByBUID.SetHitBattleSkill(pkFromChar, battleSkillBase, battleSkillDetail, num7, bEndureDamage);
					}
				}
				else
				{
					charByBUID.SetHitBattleSkill(pkFromChar, battleSkillBase, battleSkillDetail, num7, bEndureDamage);
				}
				if (num7 > -1 && !charByBUID.m_bDeadReaservation && battleSkillDetail.GetSkillDetalParamValue(87) > 0)
				{
					BATTLE_POS_GRID battleGrid = this.GetBattleGrid(charByBUID.Ally, charByBUID.GetStartPosIndex());
					battleGrid.RemoveBUID(charByBUID.GetBUID());
					this.GRID_MANAGER.RemoveBUID(charByBUID.Ally, charByBUID.GetStartPosIndex(), charByBUID.GetBUID());
					charByBUID.SetGridPos((short)num7);
					battleGrid.SetBUID(charByBUID.GetBUID(), (byte)num7, charByBUID.GetCharKindInfo().GetBattleSizeX(), charByBUID.GetCharKindInfo().GetBattleSizeY());
					Vector3 zero = Vector3.zero;
					battleGrid.GetCenter(charByBUID.GetBUID(), ref zero);
					float battleMapHeight = this.m_BattleMap.GetBattleMapHeight(new Vector3(zero.x, 0f, zero.z));
					charByBUID.GetPersonInfo().SetCharPos(zero.x, battleMapHeight + 0.3f, zero.z);
					charByBUID.m_k3DChar.MoveTo(zero.x, battleMapHeight + 0.3f, zero.z, eCharAnimationType.Stay1, true);
					charByBUID.SetRotate(charByBUID.GetGridRotate());
					this.GRID_MANAGER.Init();
				}
				if (battleSkillBase.GetBSkillCameraShake() != string.Empty)
				{
					Battle.BATTLE.BattleCamera.CameraAnimationPlay(battleSkillBase.GetBSkillCameraShake());
				}
			}
			else
			{
				charByBUID.SetDamage(num, bCritical, pkFromChar, 0, num8, 0);
			}
			if (charByBUID.GetPersonInfo().GetSoldierInfo(0).GetHP() <= 0 && charByBUID.m_bDeadReaservation)
			{
				charByBUID.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				charByBUID.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				charByBUID.SetNextOrderTarget(-1);
			}
			break;
		}
		case eBATTLE_CHARINFO_REASON.eBATTLE_CHARINFO_REASON_DEAD:
			if (charByBUID.GetSoldierInfo().GetHP() <= 0)
			{
				if (this.ColosseumObserver)
				{
					ColosseumObserverControlDlg colosseumObserverControlDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
					if (colosseumObserverControlDlg2 != null)
					{
						colosseumObserverControlDlg2.SetDeadFlag(charByBUID.Ally, charByBUID.GetBUID());
					}
				}
				charByBUID.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				charByBUID.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				charByBUID.SetNextOrderTarget(-1);
				charByBUID.SetDead();
				this.GRID_MANAGER.ActiveAttackGridCanTarget();
				if (charByBUID.MyChar && this.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("573"),
						"targetname",
						charByBUID.GetCharName()
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
			if (charByBUID.GetCharKindInfo().GetCharKind() == 703)
			{
				if ((short)_Nfy.iPara[0] != charByBUID.GetBUID())
				{
					Battle_CountDlg battle_CountDlg = (Battle_CountDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_COUNT_DLG);
					if (battle_CountDlg != null)
					{
						battle_CountDlg.ShowTreasureMonsterDie();
					}
				}
			}
			else if (charByBUID.GetCharKindInfo().GetCharKind() == 995 && (short)_Nfy.iPara[0] != charByBUID.GetBUID())
			{
				Battle_CountDlg battle_CountDlg2 = (Battle_CountDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_COUNT_DLG);
				if (battle_CountDlg2 != null)
				{
					battle_CountDlg2.ShowHiddenTreasureMonsterDie();
				}
			}
			break;
		case eBATTLE_CHARINFO_REASON.eBATTLE_CHARINFO_REASON_REBIRTH:
			if (charByBUID.GetSoldierInfo().GetHP() > 0)
			{
				charByBUID.Set3DCharStep(NkBattleChar.e3DCharStep.READY);
				charByBUID.m_bDeadReaservation = false;
				charByBUID.SetTurnState(eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_DISABLE);
				charByBUID.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				charByBUID.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				charByBUID.SetBattleSkillCoolTurn(false);
				charByBUID.SetNextOrderTarget(-1);
			}
			break;
		case eBATTLE_CHARINFO_REASON.eBATTLE_CHARINFO_REASON_DELETECHAR:
			charByBUID.SetAcquiredItem(_Nfy);
			charByBUID.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			charByBUID.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			charByBUID.SetNextOrderTarget(-1);
			charByBUID.SetDeleteChar(_Nfy.iPara[1]);
			if (this.m_eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
			{
				Battle_Colossum_CharinfoDlg battle_Colossum_CharinfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_COLOSSEUM_CHARINFO_DLG) as Battle_Colossum_CharinfoDlg;
				if (battle_Colossum_CharinfoDlg != null)
				{
					battle_Colossum_CharinfoDlg.SetDeadCount(charByBUID.Ally);
				}
			}
			if (charByBUID.MyChar)
			{
				this.m_nDieSolCount++;
				Battle_SkilldescDlg battle_SkilldescDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_SKILLDESC_DLG) as Battle_SkilldescDlg;
				if (battle_SkilldescDlg2 != null)
				{
					battle_SkilldescDlg2.SetDeadSol(charByBUID.GetSolID());
				}
				NrTSingleton<FiveRocksEventManager>.Instance.Placement("battle_diesol_" + this.m_nDieSolCount.ToString());
			}
			if (this.m_eBattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM && this.m_eBattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL && this.m_eBattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION && charByBUID.MyChar && this.m_nChangeSolCount < this.m_stBattleInfo.m_MAXCHANGESOLDIERNUM && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCY_CALL_DLG))
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BATTLE_EMERGENCY_CALL_DLG);
			}
			break;
		case eBATTLE_CHARINFO_REASON.eBATTLE_CHARINFO_REASON_SETBUFFSKILL:
		{
			int num10 = _Nfy.iPara[0];
			int num11 = _Nfy.iPara[1];
			int skillLevel2 = _Nfy.iPara[2];
			int addUseAngerlypoint = _Nfy.iPara[3];
			if (num10 > 0)
			{
				BATTLESKILL_BASE battleSkillBase2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(num10);
				BATTLESKILL_DETAIL battleSkillDetail2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num10, skillLevel2);
				if (battleSkillBase2 == null || battleSkillDetail2 == null)
				{
					return;
				}
				if ((battleSkillDetail2.m_nSkillDurationTurn > 0 && num11 > -1) || num11 == -100)
				{
					if (battleSkillDetail2.GetSkillDetalParamValue(98) != 0 || battleSkillDetail2.GetSkillDetalParamValue(99) != 0)
					{
						charByBUID.InitBattleSkillCharATB();
					}
					charByBUID.SetBattleSkillBuf(battleSkillBase2.m_nSkillUnique, num11, skillLevel2, addUseAngerlypoint);
				}
				else
				{
					charByBUID.CheckBattleSkillDetail(battleSkillBase2, battleSkillDetail2);
				}
			}
			break;
		}
		}
	}

	private void CheckSelectChar()
	{
		if (this.MyAlly != this.CurrentTurnAlly)
		{
			return;
		}
		if (this.m_SelectCharList.Count <= 0)
		{
			this.SelectNextChar();
			return;
		}
		short buid = this.m_SelectCharList[0];
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(buid);
		if (charByBUID == null)
		{
			return;
		}
		if (charByBUID.GetTurnState() == eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
		{
			return;
		}
		this.SelectNextChar();
	}
}
