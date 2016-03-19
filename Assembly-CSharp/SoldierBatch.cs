using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SoldierBatch : IDisposable
{
	private static SoldierBatch m_SoldierBatch;

	private static eSOLDIER_BATCH_MODE m_eSoldierBatchMode = eSOLDIER_BATCH_MODE.MODE_MAX;

	private static clBaberTowerInfo m_cBaberTowerInfo;

	private static clMineInfo m_MineInfo;

	private static clGuildBossInfo m_GuildBossInfo;

	public bool m_bMapLoadComplete;

	public bool m_bMakeEnemyChar;

	private bool m_bSolBatchLock;

	private static clExpeditionInfo m_ExpeditionInfo;

	public clTargetCharInfo m_cTargetInfo;

	private static byte m_iGuildWarRaidUnique;

	private static byte m_iGuildWarRaidBattlePos;

	private SoldierBatchCamera m_Camera;

	public SoldierBatch_Input m_Input;

	private Dictionary<eBATTLE_ALLY, Dictionary<int, BATTLE_POS_GRID>> m_SoldierBatchPosGrid;

	private Dictionary<eBATTLE_ALLY, List<Vector3>> m_dicVecStartPos;

	private Dictionary<eBATTLE_ALLY, Dictionary<int, SoldierBatchGrid>> m_SoldierBatchGridObj;

	private Dictionary<int, SOLDIERBATCH_INFO_BABEL_TOWER> m_dicBabel_Tower_BatchInfo;

	private Dictionary<long, GameObject> m_dicSolChar;

	private SoldierBatchSetCharInfo m_MakeUpCharInfo = new SoldierBatchSetCharInfo();

	private GameObject m_goSoldierBatchCharRoot;

	private GameObject m_goSoldierBatchEnemyCharRoot;

	private SoldierBatchGrid m_SelectGrid;

	private eSOL_SUBDATA m_eSetMode = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;

	private List<PLUNDER_TARGET_INFO> m_MakeCharList;

	private GameObject m_goSoldierBatchLoading;

	private bool m_bRemoveLoadingEffect;

	private float m_fRemoveLoadingEffect;

	private clDefenceObjInfo[] m_DefenceObjectInfo;

	private clTempBattlePos[] m_TempBattlePos;

	private List<int> m_lsUiID = new List<int>();

	private MsgBoxUI msgBox;

	public static SoldierBatch SOLDIERBATCH
	{
		get
		{
			return SoldierBatch.m_SoldierBatch;
		}
	}

	public static eSOLDIER_BATCH_MODE SOLDIER_BATCH_MODE
	{
		get
		{
			return SoldierBatch.m_eSoldierBatchMode;
		}
		set
		{
			SoldierBatch.m_eSoldierBatchMode = value;
		}
	}

	public static clBaberTowerInfo BABELTOWER_INFO
	{
		get
		{
			if (SoldierBatch.m_cBaberTowerInfo == null)
			{
				SoldierBatch.m_cBaberTowerInfo = new clBaberTowerInfo();
			}
			return SoldierBatch.m_cBaberTowerInfo;
		}
		set
		{
			SoldierBatch.m_cBaberTowerInfo = value;
		}
	}

	public static clMineInfo MINE_INFO
	{
		get
		{
			if (SoldierBatch.m_MineInfo == null)
			{
				SoldierBatch.m_MineInfo = new clMineInfo();
			}
			return SoldierBatch.m_MineInfo;
		}
		set
		{
			SoldierBatch.m_MineInfo = value;
		}
	}

	public static clGuildBossInfo GUILDBOSS_INFO
	{
		get
		{
			if (SoldierBatch.m_GuildBossInfo == null)
			{
				SoldierBatch.m_GuildBossInfo = new clGuildBossInfo();
			}
			return SoldierBatch.m_GuildBossInfo;
		}
		set
		{
			SoldierBatch.m_GuildBossInfo = value;
		}
	}

	public bool SolBatchLock
	{
		get
		{
			return this.m_bSolBatchLock;
		}
		set
		{
			this.m_bSolBatchLock = value;
			this.InitSelectMoveChar(this.MakeUpCharInfo.m_SolID, this.MakeUpCharInfo.m_FriendPersonID, this.MakeUpCharInfo.m_FriendCharKind);
			this.MakeUpCharInfo.Init();
		}
	}

	public static clExpeditionInfo EXPEDITION_INFO
	{
		get
		{
			if (SoldierBatch.m_ExpeditionInfo == null)
			{
				SoldierBatch.m_ExpeditionInfo = new clExpeditionInfo();
			}
			return SoldierBatch.m_ExpeditionInfo;
		}
		set
		{
			SoldierBatch.m_ExpeditionInfo = value;
		}
	}

	public static byte GuildWarRaidUnique
	{
		get
		{
			return SoldierBatch.m_iGuildWarRaidUnique;
		}
		set
		{
			SoldierBatch.m_iGuildWarRaidUnique = value;
		}
	}

	public static byte GuildWarRaidBattlePos
	{
		get
		{
			return SoldierBatch.m_iGuildWarRaidBattlePos;
		}
		set
		{
			SoldierBatch.m_iGuildWarRaidBattlePos = value;
		}
	}

	public SoldierBatchCamera CAMERA
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

	public SoldierBatchSetCharInfo MakeUpCharInfo
	{
		get
		{
			return this.m_MakeUpCharInfo;
		}
	}

	public SoldierBatchGrid SelectGrid
	{
		get
		{
			return this.m_SelectGrid;
		}
		set
		{
			this.m_SelectGrid = value;
		}
	}

	public GameObject PlunderLoading
	{
		get
		{
			return this.m_goSoldierBatchLoading;
		}
		set
		{
			this.m_goSoldierBatchLoading = value;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goSoldierBatchLoading);
			}
		}
	}

	public bool RemoveLoadingEffect
	{
		get
		{
			return this.m_bRemoveLoadingEffect;
		}
		set
		{
			this.m_bRemoveLoadingEffect = value;
		}
	}

	public bool IsMessageBox
	{
		get
		{
			return this.m_lsUiID.Count > 0;
		}
	}

	public void Dispose()
	{
		SoldierBatch.m_SoldierBatch = null;
		this.m_cTargetInfo = null;
		SoldierBatch.m_cBaberTowerInfo = null;
		this.m_cTargetInfo = null;
		SoldierBatch.m_MineInfo = null;
		this.m_Camera = null;
		this.m_SoldierBatchPosGrid = null;
		this.m_dicVecStartPos = null;
		this.m_SoldierBatchGridObj = null;
		this.m_dicBabel_Tower_BatchInfo = null;
		this.m_dicSolChar = null;
		this.m_MakeUpCharInfo = null;
		if (this.m_goSoldierBatchCharRoot != null)
		{
			UnityEngine.Object.Destroy(this.m_goSoldierBatchCharRoot);
			this.m_goSoldierBatchCharRoot = null;
		}
		if (this.m_MakeCharList != null)
		{
			this.m_MakeCharList.Clear();
			this.m_MakeCharList = null;
		}
		if (this.m_goSoldierBatchLoading != null)
		{
			UnityEngine.Object.Destroy(this.m_goSoldierBatchLoading);
			this.m_goSoldierBatchLoading = null;
		}
		if (this.m_DefenceObjectInfo != null)
		{
			this.m_DefenceObjectInfo = null;
		}
	}

	public void Init()
	{
		SoldierBatch.m_SoldierBatch = this;
		this.m_bMakeEnemyChar = false;
		this.m_bMapLoadComplete = false;
		this.m_bSolBatchLock = false;
		this.m_Camera = new SoldierBatchCamera();
		this.m_Camera.Init();
		this.m_Input = new SoldierBatch_Input(this);
		this.m_cTargetInfo = new clTargetCharInfo();
		this.m_cTargetInfo.Init();
		this.m_SoldierBatchPosGrid = new Dictionary<eBATTLE_ALLY, Dictionary<int, BATTLE_POS_GRID>>();
		this.m_dicVecStartPos = new Dictionary<eBATTLE_ALLY, List<Vector3>>();
		this.m_SoldierBatchGridObj = new Dictionary<eBATTLE_ALLY, Dictionary<int, SoldierBatchGrid>>();
		this.m_dicSolChar = new Dictionary<long, GameObject>();
		this.m_DefenceObjectInfo = new clDefenceObjInfo[3];
		this.m_TempBattlePos = new clTempBattlePos[15];
		for (int i = 0; i < 3; i++)
		{
			this.m_DefenceObjectInfo[i] = new clDefenceObjInfo();
			this.m_DefenceObjectInfo[i].m_nSolID = (long)(-100 + i);
		}
		for (int i = 0; i < 15; i++)
		{
			this.m_TempBattlePos[i] = new clTempBattlePos();
		}
		for (int i = 0; i < 2; i++)
		{
			this.m_SoldierBatchPosGrid.Add((eBATTLE_ALLY)i, new Dictionary<int, BATTLE_POS_GRID>());
			this.m_dicVecStartPos.Add((eBATTLE_ALLY)i, new List<Vector3>());
			this.m_SoldierBatchGridObj.Add((eBATTLE_ALLY)i, new Dictionary<int, SoldierBatchGrid>());
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			this.m_dicVecStartPos[eBATTLE_ALLY.eBATTLE_ALLY_0].Add(new Vector3(103f, 41f, 98f));
			this.m_dicVecStartPos[eBATTLE_ALLY.eBATTLE_ALLY_1].Add(new Vector3(103f, 41f, 106f));
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			this.m_dicVecStartPos[eBATTLE_ALLY.eBATTLE_ALLY_0].Add(new Vector3(31f, 5f, 66f));
			this.m_dicVecStartPos[eBATTLE_ALLY.eBATTLE_ALLY_1].Add(new Vector3(31f, 5f, 76f));
		}
		else
		{
			this.m_dicVecStartPos[eBATTLE_ALLY.eBATTLE_ALLY_0].Add(new Vector3(136f, 22f, 104f));
			this.m_dicVecStartPos[eBATTLE_ALLY.eBATTLE_ALLY_0].Add(new Vector3(128f, 22f, 104f));
			this.m_dicVecStartPos[eBATTLE_ALLY.eBATTLE_ALLY_0].Add(new Vector3(144f, 20f, 104f));
			this.m_dicVecStartPos[eBATTLE_ALLY.eBATTLE_ALLY_1].Add(new Vector3(136f, 22f, 112.5f));
			this.m_dicVecStartPos[eBATTLE_ALLY.eBATTLE_ALLY_1].Add(new Vector3(128f, 22f, 112.5f));
			this.m_dicVecStartPos[eBATTLE_ALLY.eBATTLE_ALLY_1].Add(new Vector3(144f, 22f, 112.5f));
		}
		this.m_dicBabel_Tower_BatchInfo = new Dictionary<int, SOLDIERBATCH_INFO_BABEL_TOWER>();
		NrTSingleton<NkBattleCharManager>.Instance.Init();
		NrTSingleton<NkCharManager>.Instance.SetChildActive(false);
		NrTSingleton<NrMainSystem>.Instance.GetInputManager().AddInputCommandLayer(this.m_Input);
		Time.timeScale = 1f;
		this.m_bMapLoadComplete = false;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PLUNDER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP)
		{
			this.m_eSetMode = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			this.m_eSetMode = eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
		{
			this.m_eSetMode = eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2 || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			this.m_eSetMode = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			this.m_eSetMode = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP)
		{
			this.m_eSetMode = eSOL_SUBDATA.SOL_SUBDATA_DEFENSE_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			this.m_eSetMode = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		this.m_MakeCharList = new List<PLUNDER_TARGET_INFO>();
		this.m_lsUiID.Clear();
		this.msgBox = null;
	}

	public clTempBattlePos[] GetTempBattlePosInfo()
	{
		return this.m_TempBattlePos;
	}

	public long GetTempBattleSolID(int nIndex)
	{
		int num = 1;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			num = 5;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			num = 9;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			num = 3;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade((byte)SoldierBatch.EXPEDITION_INFO.m_eExpeditionGrade);
			if (expeditionDataFromGrade != null)
			{
				num = expeditionDataFromGrade.Expedition_SolBatch_Array * 3;
			}
			else
			{
				num = 15;
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			num = 5;
		}
		if (nIndex >= num)
		{
			return 0L;
		}
		return this.m_TempBattlePos[nIndex].m_nSolID;
	}

	public int GetTempBattlePos(long nSolID)
	{
		int num = 1;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			num = 5;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			num = 9;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			num = 3;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade((byte)SoldierBatch.EXPEDITION_INFO.m_eExpeditionGrade);
			if (expeditionDataFromGrade != null)
			{
				num = expeditionDataFromGrade.Expedition_SolBatch_Array * 3;
			}
			else
			{
				num = 15;
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			num = 5;
		}
		for (int i = 0; i < num; i++)
		{
			if (this.m_TempBattlePos[i].m_nSolID == nSolID)
			{
				return (int)this.m_TempBattlePos[i].m_nBattlePos;
			}
		}
		return 0;
	}

	public void SetTempBattlePos(long nSolID, byte nBattlePos, int nCharKind, bool bInitInfo)
	{
		this.InitTempSolInfo(nSolID);
		int num = 1;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			num = 5;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			num = 9;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			num = 3;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			num = 15;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			num = 5;
		}
		if (!bInitInfo)
		{
			for (int i = 0; i < num; i++)
			{
				if (this.m_TempBattlePos[i].m_nSolID <= 0L)
				{
					this.m_TempBattlePos[i].m_nSolID = nSolID;
					this.m_TempBattlePos[i].m_nBattlePos = nBattlePos;
					this.m_TempBattlePos[i].m_nCharKind = nCharKind;
					break;
				}
			}
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
		{
			PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
			if (plunderSolListDlg != null)
			{
				plunderSolListDlg.UpdateSolList(nSolID);
				plunderSolListDlg.SetSolNum(this.GetTempCount(), false);
			}
		}
	}

	public void InitTempSolInfo(long nSolID)
	{
		int num = 1;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			num = 5;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			num = 9;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			num = 3;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			num = 15;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			num = 5;
		}
		for (int i = 0; i < num; i++)
		{
			if (this.m_TempBattlePos[i].m_nSolID == nSolID)
			{
				this.m_TempBattlePos[i].m_nSolID = 0L;
				this.m_TempBattlePos[i].m_nBattlePos = 0;
				this.m_TempBattlePos[i].m_nCharKind = 0;
				break;
			}
		}
	}

	public void InitEnemyChar()
	{
		if (this.m_goSoldierBatchEnemyCharRoot != null)
		{
			UnityEngine.Object.Destroy(this.m_goSoldierBatchEnemyCharRoot);
			this.m_goSoldierBatchEnemyCharRoot = null;
		}
	}

	public Vector3 GetGridCenter()
	{
		Vector3 result = Vector3.zero;
		if (SoldierBatch.m_eSoldierBatchMode != eSOLDIER_BATCH_MODE.MODE_PLUNDER && SoldierBatch.m_eSoldierBatchMode != eSOLDIER_BATCH_MODE.MODE_INFIBATTLE && SoldierBatch.m_eSoldierBatchMode != eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE && SoldierBatch.m_eSoldierBatchMode != eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			if (SoldierBatch.m_eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
			{
				result = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][0].mListPos[5];
			}
			else
			{
				result = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][0].mListPos[1];
			}
		}
		else
		{
			result = (this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][0].GetCenter() + this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_1][0].GetCenter()) / 2f;
		}
		return result;
	}

	public int GetSolBatchNum()
	{
		int result;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			result = this.GetBabelTowerSolCount();
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			result = this.GetTempCount();
		}
		else
		{
			result = this.GetPlunderAttackPosSolNum();
		}
		return result;
	}

	public int GetPlunderAttackPosSolNum()
	{
		int num = 0;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return num;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		if (soldierList == null)
		{
			return num;
		}
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.GetSolSubData(this.m_eSetMode) > 0L)
			{
				num++;
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return num;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolSubData(this.m_eSetMode) > 0L)
			{
				num++;
			}
		}
		return num;
	}

	public Vector3 GetGridPos(eBATTLE_ALLY eAlly, byte nStartPos, byte nGridPos)
	{
		BATTLE_POS_GRID bATTLE_POS_GRID = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)nStartPos];
		Vector3 vector = bATTLE_POS_GRID.mListPos[(int)nGridPos];
		SoldierBatchGridCell.PickTerrain(vector, ref vector);
		return vector;
	}

	public void AddCharFromSolID(long SolID, GameObject goObject)
	{
		if (!this.m_dicSolChar.ContainsKey(SolID))
		{
			this.m_dicSolChar.Add(SolID, goObject);
		}
	}

	public void RemoveCharFromSolID(long SolID)
	{
		if (!this.m_dicSolChar.ContainsKey(SolID))
		{
			return;
		}
		GameObject gameObject = this.m_dicSolChar[SolID];
		if (gameObject != null)
		{
			this.m_dicSolChar.Remove(SolID);
		}
		UnityEngine.Object.Destroy(gameObject);
	}

	public GameObject GetCharFromSolID(long SolID)
	{
		if (this.m_dicSolChar.ContainsKey(SolID))
		{
			return this.m_dicSolChar[SolID];
		}
		return null;
	}

	public static void GetCalcBattlePos(long nSolSubData, ref byte nStartPos, ref byte nBattlePos)
	{
		nStartPos = (byte)(nSolSubData / 100L);
		nBattlePos = (byte)(nSolSubData % 100L - 1L);
	}

	public int SetCalcBattlePos(byte nStartPos, byte nBattlePos)
	{
		return (int)(nStartPos * 100 + (nBattlePos + 1));
	}

	public void MakePlunderPrepareUI()
	{
		PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
		if (plunderSolNumDlg != null)
		{
			plunderSolNumDlg.SetExplain();
			plunderSolNumDlg.Show();
		}
		int nSolNum = 0;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			nSolNum = this.GetSolBatchNum();
			BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
			if (babelLobbyUserListDlg != null)
			{
				for (int i = 0; i < (int)SoldierBatch.BABELTOWER_INFO.Count; i++)
				{
					babelLobbyUserListDlg.RefreshSolInfo();
					babelLobbyUserListDlg.SetWaitingLock(false);
				}
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			this.LoadGuildBossBatchSolInfo();
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BABEL_GUILDBOSS_LOBBY_DLG);
			nSolNum = this.GetTempCount();
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PLUNDER)
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.PLUNDERTARGETINFO_DLG);
			nSolNum = this.GetSolBatchNum();
			PlunderStartAndReMatchDlg plunderStartAndReMatchDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_STARTANDREMATCH_DLG) as PlunderStartAndReMatchDlg;
			if (plunderStartAndReMatchDlg != null)
			{
				plunderStartAndReMatchDlg.SetButtonMode();
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE)
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.PLUNDERTARGETINFO_DLG);
			nSolNum = this.GetSolBatchNum();
			PlunderStartAndReMatchDlg plunderStartAndReMatchDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_STARTANDREMATCH_DLG) as PlunderStartAndReMatchDlg;
			if (plunderStartAndReMatchDlg2 != null)
			{
				plunderStartAndReMatchDlg2.SetButtonMode();
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.PLUNDERTARGETINFO_DLG);
			nSolNum = this.GetSolBatchNum();
			PlunderStartAndReMatchDlg plunderStartAndReMatchDlg3 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_STARTANDREMATCH_DLG) as PlunderStartAndReMatchDlg;
			if (plunderStartAndReMatchDlg3 != null)
			{
				plunderStartAndReMatchDlg3.SetButtonMode();
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			nSolNum = this.LoadPVPMakeup2BatchSolInfo();
			PlunderStartAndReMatchDlg plunderStartAndReMatchDlg4 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_STARTANDREMATCH_DLG) as PlunderStartAndReMatchDlg;
			if (plunderStartAndReMatchDlg4 != null)
			{
				plunderStartAndReMatchDlg4.SetButtonMode();
			}
		}
		else
		{
			PlunderStartAndReMatchDlg plunderStartAndReMatchDlg5 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_STARTANDREMATCH_DLG) as PlunderStartAndReMatchDlg;
			if (plunderStartAndReMatchDlg5 != null)
			{
				plunderStartAndReMatchDlg5.SetButtonMode();
			}
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			nSolNum = this.GetSolBatchNum();
		}
		PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
		if (plunderSolListDlg != null)
		{
			plunderSolListDlg.SetSolNum(nSolNum, false);
			plunderSolListDlg.Show();
		}
	}

	public void Close()
	{
		NrTSingleton<NrMainSystem>.Instance.GetInputManager().RemoveInputCommandLayer(this.m_Input);
		this.m_Input = null;
		NrTSingleton<NkCharManager>.Instance.SetChildActive(true);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERSOLLIST_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDER_STARTANDREMATCH_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERSOLNUM_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERTARGETINFO_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWERUSERLIST_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABEL_GUILDBOSS_LOBBY_DLG);
		if (this.m_goSoldierBatchLoading != null)
		{
			UnityEngine.Object.Destroy(this.m_goSoldierBatchLoading);
		}
		this.m_goSoldierBatchLoading = null;
		if (this.m_SoldierBatchPosGrid != null)
		{
			this.m_SoldierBatchPosGrid.Clear();
			this.m_SoldierBatchPosGrid = null;
		}
		if (this.m_SoldierBatchGridObj != null)
		{
			this.m_SoldierBatchGridObj.Clear();
			this.m_SoldierBatchGridObj = null;
		}
		if (this.m_dicSolChar != null)
		{
			this.m_dicSolChar.Clear();
			this.m_dicSolChar = null;
		}
		if (this.m_goSoldierBatchCharRoot != null)
		{
			UnityEngine.Object.Destroy(this.m_goSoldierBatchCharRoot);
			this.m_goSoldierBatchCharRoot = null;
		}
		if (this.m_goSoldierBatchEnemyCharRoot != null)
		{
			UnityEngine.Object.Destroy(this.m_goSoldierBatchEnemyCharRoot);
			this.m_goSoldierBatchEnemyCharRoot = null;
		}
		Time.timeScale = 1f;
	}

	public bool IsCompleteLoadPlunderMap()
	{
		return this.m_bMapLoadComplete;
	}

	public bool IsAllCharLoadComplete()
	{
		this.MakeGridInfo();
		this.SetChangeBabelBatchGrid();
		this.MakePlunderMyCharTotal();
		for (int i = 0; i < this.m_MakeCharList.Count; i++)
		{
			PLUNDER_TARGET_INFO info = this.m_MakeCharList[i];
			this.MakePlunderCharEnemy(info);
		}
		this.m_MakeCharList.Clear();
		return true;
	}

	public void AddEnemyCharInfo(PLUNDER_TARGET_INFO _info)
	{
		if (_info.nCharKind < 0)
		{
			return;
		}
		this.m_MakeCharList.Add(_info);
	}

	[DebuggerHidden]
	public static IEnumerator DownloadPlunderMap(AStage stage, BATTLE_MAP BASEMAP, TsSceneSwitcher.ESceneType eSceneType)
	{
		SoldierBatch.<DownloadPlunderMap>c__Iterator5 <DownloadPlunderMap>c__Iterator = new SoldierBatch.<DownloadPlunderMap>c__Iterator5();
		<DownloadPlunderMap>c__Iterator.BASEMAP = BASEMAP;
		<DownloadPlunderMap>c__Iterator.<$>BASEMAP = BASEMAP;
		return <DownloadPlunderMap>c__Iterator;
	}

	public static void OnDownloadPlunderPrepareMap(object obj)
	{
		GameObject gameObject = GameObject.Find("Battle");
		if (gameObject != null)
		{
			gameObject.name = "SoldierBatch";
		}
		GameObject gameObject2 = TsSceneSwitcher.Instance._GetBundle_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		if (gameObject2 == null)
		{
			return;
		}
		Transform child = NkUtil.GetChild(gameObject2.transform, "battle_terrain");
		if (child != null)
		{
			child.gameObject.layer = TsLayer.TERRAIN;
		}
		else
		{
			TsPlatform.FileLog("Battle Battle_Terrain Problem");
		}
		TsSceneSwitcher.Instance.CollectAllMapGameObjects(TsSceneSwitcher.ESceneType.SoldierBatchScene, true);
		NkUtil.SetAllChildActive(gameObject2, true);
		SoldierBatch.SOLDIERBATCH.m_bMapLoadComplete = true;
		TsSceneSwitcher.Instance.Switch(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		SoldierBatch.SOLDIERBATCH.CAMERA.SetPlunderCamera();
	}

	private void MakeGridInfo()
	{
		int id = 0;
		if (SoldierBatch.m_eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			id = 4;
		}
		else if (SoldierBatch.m_eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			id = 3;
		}
		GameObject parent = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		GameObject parent2 = TBSUTIL.Attach("SoldierBatchGrid", parent);
		for (int i = 0; i < 2; i++)
		{
			Dictionary<int, BATTLE_POS_GRID> dictionary = this.m_SoldierBatchPosGrid[(eBATTLE_ALLY)i];
			List<Vector3> list = this.m_dicVecStartPos[(eBATTLE_ALLY)i];
			for (int j = 0; j < list.Count; j++)
			{
				Vector3 vector = list[j];
				if (!dictionary.ContainsKey(j))
				{
					BATTLE_POS_GRID bATTLE_POS_GRID = new BATTLE_POS_GRID();
					BATTLE_POS_GRID info = BASE_BATTLE_POS_Manager.GetInstance().GetInfo(id);
					bATTLE_POS_GRID.Set(info, (i != 0) ? 180 : 0);
					bATTLE_POS_GRID.SetCenter(new Vector3(vector.x, vector.y, vector.z));
					dictionary.Add(j, bATTLE_POS_GRID);
				}
			}
		}
		for (int i = 0; i < 2; i++)
		{
			Dictionary<int, BATTLE_POS_GRID> dictionary2 = this.m_SoldierBatchPosGrid[(eBATTLE_ALLY)i];
			Dictionary<int, SoldierBatchGrid> dictionary3 = this.m_SoldierBatchGridObj[(eBATTLE_ALLY)i];
			for (int j = 0; j < dictionary2.Values.Count; j++)
			{
				BATTLE_POS_GRID bATTLE_POS_GRID2 = dictionary2[j];
				Vector3[] mListPos = bATTLE_POS_GRID2.mListPos;
				GameObject gameObject = TBSUTIL.Attach(string.Format("GRID{0}_{1}", i, j), parent2);
				for (int k = 0; k < mListPos.Length; k++)
				{
					SoldierBatchGrid soldierBatchGrid = SoldierBatchGrid.Create((eBATTLE_ALLY)i, (short)j, mListPos[k], k);
					soldierBatchGrid.gameObject.transform.parent = gameObject.transform;
					dictionary3.Add(this.SetCalcBattlePos((byte)j, (byte)k), soldierBatchGrid);
				}
				if (SoldierBatch.m_eSoldierBatchMode != eSOLDIER_BATCH_MODE.MODE_PLUNDER && SoldierBatch.m_eSoldierBatchMode != eSOLDIER_BATCH_MODE.MODE_INFIBATTLE && SoldierBatch.m_eSoldierBatchMode != eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE && i == 1)
				{
					gameObject.SetActive(false);
				}
			}
		}
		Vector3 gridCenter = this.GetGridCenter();
		SoldierBatchGridCell.PickTerrain(this.GetGridCenter(), ref gridCenter);
		if (SoldierBatch.m_eSoldierBatchMode == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			BATTLE_POS_GRID info2 = BASE_BATTLE_POS_Manager.GetInstance().GetInfo(id);
			for (int i = 0; i < info2.mListPos.Length; i++)
			{
				this.m_dicBabel_Tower_BatchInfo.Add(this.SetCalcBattlePos(0, (byte)i), new SOLDIERBATCH_INFO_BABEL_TOWER());
			}
		}
		this.CAMERA.SetcameraPos(gridCenter);
	}

	private void LoadPlunderChar(ref IDownloadedItem wItem, object obj)
	{
		if (wItem.isCanceled)
		{
			return;
		}
		if (!wItem.canAccessAssetBundle)
		{
			return;
		}
		GameObject gameObject = obj as GameObject;
		TsPositionFollowerTerrain tsPositionFollowerTerrain = gameObject.GetComponent<TsPositionFollowerTerrain>();
		if (tsPositionFollowerTerrain == null)
		{
			tsPositionFollowerTerrain = gameObject.AddComponent<TsPositionFollowerTerrain>();
			tsPositionFollowerTerrain.enabled = false;
		}
		GameObject original = wItem.mainAsset as GameObject;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(original) as GameObject;
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref gameObject2);
		}
		Vector3 localScale = gameObject2.transform.localScale;
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = localScale;
		Transform child = NkUtil.GetChild(gameObject2.transform, "actioncam");
		if (null != child)
		{
			child.gameObject.SetActive(false);
		}
		Animation componentInChildren = gameObject2.GetComponentInChildren<Animation>();
		if (componentInChildren != null)
		{
			if (componentInChildren.GetClip(eCharAnimationType.BStay1.ToString().ToLower()) != null)
			{
				componentInChildren.Play(eCharAnimationType.BStay1.ToString().ToLower());
			}
			else
			{
				foreach (AnimationState animationState in componentInChildren)
				{
					if (animationState.clip.name.Contains("bstay1"))
					{
						componentInChildren.Play(animationState.clip.name);
						break;
					}
				}
			}
		}
	}

	private void LoadPlunderCharFromUI(ref IDownloadedItem wItem, object obj)
	{
		if (wItem.isCanceled)
		{
			return;
		}
		if (!wItem.canAccessAssetBundle)
		{
			return;
		}
		SoldierBatchSetCharInfo soldierBatchSetCharInfo = obj as SoldierBatchSetCharInfo;
		GameObject goChar = soldierBatchSetCharInfo.m_goChar;
		TsPositionFollowerTerrain tsPositionFollowerTerrain = goChar.GetComponent<TsPositionFollowerTerrain>();
		if (tsPositionFollowerTerrain == null)
		{
			tsPositionFollowerTerrain = goChar.AddComponent<TsPositionFollowerTerrain>();
			tsPositionFollowerTerrain.enabled = true;
		}
		GameObject original = wItem.mainAsset as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		Vector3 localScale = gameObject.transform.localScale;
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref gameObject);
		}
		gameObject.transform.parent = goChar.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = localScale;
		Transform child = NkUtil.GetChild(gameObject.transform, "actioncam");
		if (null != child)
		{
			child.gameObject.SetActive(false);
		}
		this.MakeUpCharInfo.m_SolID = long.Parse(goChar.name);
		this.MakeUpCharInfo.m_FriendPersonID = soldierBatchSetCharInfo.m_FriendPersonID;
		this.MakeUpCharInfo.m_FriendCharKind = soldierBatchSetCharInfo.m_FriendCharKind;
		this.MakeUpCharInfo.m_nObjectid = soldierBatchSetCharInfo.m_nObjectid;
		Animation componentInChildren = gameObject.GetComponentInChildren<Animation>();
		if (componentInChildren != null)
		{
			if (componentInChildren.GetClip(eCharAnimationType.BStay1.ToString().ToLower()) != null)
			{
				componentInChildren.Play(eCharAnimationType.BStay1.ToString().ToLower());
			}
			else
			{
				foreach (AnimationState animationState in componentInChildren)
				{
					if (animationState.clip.name.Contains("bstay1"))
					{
						componentInChildren.Play(animationState.clip.name);
						break;
					}
				}
			}
		}
		if (!Input.GetMouseButton(0))
		{
			this.InitSelectMoveChar(this.MakeUpCharInfo.m_SolID, this.MakeUpCharInfo.m_FriendPersonID, this.MakeUpCharInfo.m_FriendCharKind);
			this.MakeUpCharInfo.Init();
		}
		else
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "HERO-OUT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public void MakePlunderMyCharTotal()
	{
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		if (gameObject == null)
		{
			return;
		}
		if (this.m_goSoldierBatchCharRoot == null)
		{
			this.m_goSoldierBatchCharRoot = new GameObject("SoldierBatchCharRoot");
			this.m_goSoldierBatchCharRoot.transform.parent = gameObject.transform;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		if (soldierList == null)
		{
			return;
		}
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.IsInjuryStatus() && this.m_eSetMode == eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS)
			{
				nkSoldierInfo.SetSolSubData((int)this.m_eSetMode, 0L);
			}
			else if (nkSoldierInfo.GetSolSubData(this.m_eSetMode) > 0L)
			{
				long solSubData = nkSoldierInfo.GetSolSubData(this.m_eSetMode);
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkSoldierInfo.GetCharKind());
				if (charKindInfo != null)
				{
					string text = string.Empty;
					if (charKindInfo.IsATB(1L))
					{
						text = "Char/Player/" + charKindInfo.GetBundlePath();
					}
					else
					{
						text = "char/" + charKindInfo.GetBundlePath();
					}
					float num = (float)charKindInfo.GetScale() / 10f;
					num *= 1f;
					GameObject gameObject2 = new GameObject(nkSoldierInfo.GetSolID().ToString());
					gameObject2.transform.parent = this.m_goSoldierBatchCharRoot.transform;
					byte key = 0;
					byte b = 0;
					SoldierBatch.GetCalcBattlePos(solSubData, ref key, ref b);
					BATTLE_POS_GRID bATTLE_POS_GRID = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)key];
					Vector3 vector = bATTLE_POS_GRID.mListPos[(int)b];
					SoldierBatchGridCell.PickTerrain(vector, ref vector);
					gameObject2.transform.position = vector;
					gameObject2.transform.localScale = new Vector3(num, num, num);
					Transform child = NkUtil.GetChild(gameObject2.transform, "actioncam");
					if (null != child)
					{
						child.gameObject.SetActive(false);
					}
					if (this.m_eSetMode != eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
					{
						Battle_PowerDlg battle_PowerDlg = (Battle_PowerDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.BATTLE_POWER_GROUP_DLG);
						if (battle_PowerDlg != null)
						{
							battle_PowerDlg.Set(gameObject2, nkSoldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER));
						}
					}
					Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
					dictionary[(int)solSubData].SolID = nkSoldierInfo.GetSolID();
					dictionary[(int)solSubData].ObjID = 0;
					dictionary[(int)solSubData].CharKind = nkSoldierInfo.GetCharKind();
					this.AddCharFromSolID(nkSoldierInfo.GetSolID(), gameObject2);
					text = text.ToLower();
					NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(text, NkBundleCallBack.NPCBundleStackName, new NkBundleParam.funcParamBundleCallBack(this.LoadPlunderChar), gameObject2, true);
				}
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.IsInjuryStatus() && this.m_eSetMode == eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS)
			{
				current.SetSolSubData((int)this.m_eSetMode, 0L);
			}
			else if (current.GetSolSubData(this.m_eSetMode) > 0L)
			{
				long solSubData2 = current.GetSolSubData(this.m_eSetMode);
				NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.GetCharKind());
				if (charKindInfo2 != null)
				{
					string text2 = string.Empty;
					if (charKindInfo2.IsATB(1L))
					{
						text2 = "Char/Player/" + charKindInfo2.GetBundlePath();
					}
					else
					{
						text2 = "char/" + charKindInfo2.GetBundlePath();
					}
					float num2 = (float)charKindInfo2.GetScale() / 10f;
					num2 *= 1f;
					GameObject gameObject3 = new GameObject(current.GetSolID().ToString());
					gameObject3.transform.parent = this.m_goSoldierBatchCharRoot.transform;
					byte key2 = 0;
					byte b2 = 0;
					SoldierBatch.GetCalcBattlePos(solSubData2, ref key2, ref b2);
					BATTLE_POS_GRID bATTLE_POS_GRID2 = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)key2];
					Vector3 vector2 = bATTLE_POS_GRID2.mListPos[(int)b2];
					SoldierBatchGridCell.PickTerrain(vector2, ref vector2);
					gameObject3.transform.position = vector2;
					gameObject3.transform.localScale = new Vector3(num2, num2, num2);
					Transform child2 = NkUtil.GetChild(gameObject3.transform, "actioncam");
					if (null != child2)
					{
						child2.gameObject.SetActive(false);
					}
					Dictionary<int, SoldierBatchGrid> dictionary2 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
					dictionary2[(int)solSubData2].SolID = current.GetSolID();
					dictionary2[(int)solSubData2].ObjID = 0;
					dictionary2[(int)solSubData2].CharKind = current.GetCharKind();
					this.AddCharFromSolID(current.GetSolID(), gameObject3);
					if (this.m_eSetMode != eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
					{
						Battle_PowerDlg battle_PowerDlg2 = (Battle_PowerDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.BATTLE_POWER_GROUP_DLG);
						if (battle_PowerDlg2 != null)
						{
							battle_PowerDlg2.Set(gameObject3, current.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER));
						}
					}
					text2 = text2.ToLower();
					NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(text2, NkBundleCallBack.NPCBundleStackName, new NkBundleParam.funcParamBundleCallBack(this.LoadPlunderChar), gameObject3, true);
				}
			}
		}
		this.CheckSameBattlePos(SoldierBatch.SOLDIER_BATCH_MODE);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			NrTable_PlunderObjectinfo_Manager instance = NrTSingleton<NrTable_PlunderObjectinfo_Manager>.Instance;
			if (instance == null)
			{
				return;
			}
			SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			sUBDATA_UNION.nSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_PLUNDEROBJECT);
			this.m_DefenceObjectInfo[0].m_nObjID = (byte)sUBDATA_UNION.n8SubData_0;
			this.m_DefenceObjectInfo[0].m_nBattlePos = (byte)sUBDATA_UNION.n8SubData_1;
			this.m_DefenceObjectInfo[1].m_nObjID = (byte)sUBDATA_UNION.n8SubData_2;
			this.m_DefenceObjectInfo[1].m_nBattlePos = (byte)sUBDATA_UNION.n8SubData_3;
			this.m_DefenceObjectInfo[2].m_nObjID = (byte)sUBDATA_UNION.n8SubData_4;
			this.m_DefenceObjectInfo[2].m_nBattlePos = (byte)sUBDATA_UNION.n8SubData_5;
			for (int j = 0; j < 3; j++)
			{
				if (this.m_DefenceObjectInfo[j].m_nObjID <= 0)
				{
					this.m_DefenceObjectInfo[j].m_nObjID = 0;
					this.m_DefenceObjectInfo[j].m_nBattlePos = 0;
				}
				else if (this.m_DefenceObjectInfo[j].m_nBattlePos <= 0)
				{
					this.m_DefenceObjectInfo[j].m_nObjID = 0;
					this.m_DefenceObjectInfo[j].m_nBattlePos = 0;
				}
				else
				{
					PLUNDER_OBJECT_INFO pLUNDER_OBJECT_INFO = instance.Get_Value(this.m_DefenceObjectInfo[j].m_nObjID);
					if (pLUNDER_OBJECT_INFO != null)
					{
						long nSolID = this.m_DefenceObjectInfo[j].m_nSolID;
						NrCharKindInfo charKindInfo3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(pLUNDER_OBJECT_INFO.nObject_Kind);
						if (charKindInfo3 != null)
						{
							long num3 = (long)this.m_DefenceObjectInfo[j].m_nBattlePos;
							string text3 = string.Empty;
							if (charKindInfo3.IsATB(1L))
							{
								text3 = "Char/Player/" + charKindInfo3.GetBundlePath();
							}
							else
							{
								text3 = "char/" + charKindInfo3.GetBundlePath();
							}
							float num4 = (float)charKindInfo3.GetScale() / 10f;
							num4 *= 1f;
							GameObject gameObject4 = new GameObject(nSolID.ToString());
							gameObject4.transform.parent = this.m_goSoldierBatchCharRoot.transform;
							byte key3 = 0;
							byte b3 = 0;
							SoldierBatch.GetCalcBattlePos(num3, ref key3, ref b3);
							BATTLE_POS_GRID bATTLE_POS_GRID3 = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)key3];
							Vector3 vector3 = bATTLE_POS_GRID3.mListPos[(int)b3];
							SoldierBatchGridCell.PickTerrain(vector3, ref vector3);
							gameObject4.transform.position = vector3;
							gameObject4.transform.localScale = new Vector3(num4, num4, num4);
							Transform child3 = NkUtil.GetChild(gameObject4.transform, "actioncam");
							if (null != child3)
							{
								child3.gameObject.SetActive(false);
							}
							Dictionary<int, SoldierBatchGrid> dictionary3 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
							dictionary3[(int)num3].SolID = this.m_DefenceObjectInfo[j].m_nSolID;
							dictionary3[(int)num3].ObjID = this.m_DefenceObjectInfo[j].m_nObjID;
							dictionary3[(int)num3].CharKind = pLUNDER_OBJECT_INFO.nObject_Kind;
							this.AddCharFromSolID(nSolID, gameObject4);
							text3 = text3.ToLower();
							NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(text3, NkBundleCallBack.NPCBundleStackName, new NkBundleParam.funcParamBundleCallBack(this.LoadPlunderChar), gameObject4, true);
						}
					}
				}
			}
		}
	}

	public void MakePlunderCharFromUI(long nSolID, long nFriendPersonID, int nFriendCharKind, long nFriendFightPower)
	{
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		if (gameObject == null)
		{
			return;
		}
		if (this.m_goSoldierBatchCharRoot == null)
		{
			this.m_goSoldierBatchCharRoot = new GameObject("SoldierBatchCharRoot");
			this.m_goSoldierBatchCharRoot.transform.parent = gameObject.transform;
		}
		if (this.MakeUpCharInfo.m_SolID != 0L)
		{
			this.InitSelectMoveChar(this.MakeUpCharInfo.m_SolID, this.MakeUpCharInfo.m_FriendPersonID, this.MakeUpCharInfo.m_FriendCharKind);
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		bool flag = false;
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(nSolID);
		int charkind;
		if (soldierInfoFromSolID != null)
		{
			charkind = soldierInfoFromSolID.GetCharKind();
		}
		else
		{
			if (nFriendCharKind <= 0)
			{
				return;
			}
			flag = true;
			charkind = nFriendCharKind;
		}
		GameObject charFromSolID = this.GetCharFromSolID(nSolID);
		if (charFromSolID != null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(charkind);
		if (charKindInfo != null)
		{
			string text = string.Empty;
			if (charKindInfo.IsATB(1L))
			{
				text = "Char/Player/" + charKindInfo.GetBundlePath();
			}
			else
			{
				text = "char/" + charKindInfo.GetBundlePath();
			}
			float num = (float)charKindInfo.GetScale() / 10f;
			num *= 1f;
			GameObject gameObject2 = new GameObject(nSolID.ToString());
			gameObject2.transform.parent = this.m_goSoldierBatchCharRoot.transform;
			gameObject2.transform.localScale = new Vector3(num, num, num);
			Transform child = NkUtil.GetChild(gameObject2.transform, "actioncam");
			if (null != child)
			{
				child.gameObject.SetActive(false);
			}
			this.AddCharFromSolID(nSolID, gameObject2);
			SoldierBatchSetCharInfo soldierBatchSetCharInfo = new SoldierBatchSetCharInfo();
			soldierBatchSetCharInfo.m_goChar = gameObject2;
			if (flag)
			{
				soldierBatchSetCharInfo.m_FriendPersonID = nFriendPersonID;
				soldierBatchSetCharInfo.m_FriendCharKind = nFriendCharKind;
			}
			else
			{
				soldierBatchSetCharInfo.m_FriendPersonID = 0L;
				soldierBatchSetCharInfo.m_FriendCharKind = 0;
			}
			soldierBatchSetCharInfo.m_SolID = nSolID;
			if (this.m_eSetMode != eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
			{
				if (soldierInfoFromSolID != null)
				{
					Battle_PowerDlg battle_PowerDlg = (Battle_PowerDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.BATTLE_POWER_GROUP_DLG);
					if (battle_PowerDlg != null)
					{
						battle_PowerDlg.Set(gameObject2, soldierInfoFromSolID.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER));
					}
				}
				else if (nFriendCharKind > 0)
				{
					Battle_PowerDlg battle_PowerDlg2 = (Battle_PowerDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.BATTLE_POWER_GROUP_DLG);
					if (battle_PowerDlg2 != null)
					{
						battle_PowerDlg2.Set(gameObject2, nFriendFightPower);
					}
				}
			}
			text = text.ToLower();
			NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(text, NkBundleCallBack.NPCBundleStackName, new NkBundleParam.funcParamBundleCallBack(this.LoadPlunderCharFromUI), soldierBatchSetCharInfo, true);
		}
	}

	public void MakePlunderCharFromUIObject(PLUNDER_OBJECT_INFO pkObjInfo)
	{
		if (pkObjInfo == null)
		{
			return;
		}
		if (pkObjInfo.nObject_Kind <= 0)
		{
			return;
		}
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		if (gameObject == null)
		{
			return;
		}
		if (this.m_goSoldierBatchCharRoot == null)
		{
			this.m_goSoldierBatchCharRoot = new GameObject("SoldierBatchCharRoot");
			this.m_goSoldierBatchCharRoot.transform.parent = gameObject.transform;
		}
		if (this.MakeUpCharInfo.m_SolID != 0L)
		{
			this.InitSelectMoveChar(this.MakeUpCharInfo.m_SolID, this.MakeUpCharInfo.m_FriendPersonID, this.MakeUpCharInfo.m_FriendCharKind);
		}
		int num = this.FindEmptyObjInfo();
		if (num < 0)
		{
			return;
		}
		long nSolID = this.m_DefenceObjectInfo[num].m_nSolID;
		GameObject charFromSolID = this.GetCharFromSolID(nSolID);
		if (charFromSolID != null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(pkObjInfo.nObject_Kind);
		if (charKindInfo != null)
		{
			string text = string.Empty;
			if (charKindInfo.IsATB(1L))
			{
				text = "Char/Player/" + charKindInfo.GetBundlePath();
			}
			else
			{
				text = "char/" + charKindInfo.GetBundlePath();
			}
			float num2 = (float)charKindInfo.GetScale() / 10f;
			num2 *= 1f;
			GameObject gameObject2 = new GameObject(nSolID.ToString());
			gameObject2.transform.parent = this.m_goSoldierBatchCharRoot.transform;
			gameObject2.transform.localScale = new Vector3(num2, num2, num2);
			Transform child = NkUtil.GetChild(gameObject2.transform, "actioncam");
			if (null != child)
			{
				child.gameObject.SetActive(false);
			}
			this.AddCharFromSolID(nSolID, gameObject2);
			SoldierBatchSetCharInfo soldierBatchSetCharInfo = new SoldierBatchSetCharInfo();
			soldierBatchSetCharInfo.m_goChar = gameObject2;
			soldierBatchSetCharInfo.m_FriendPersonID = 0L;
			soldierBatchSetCharInfo.m_FriendCharKind = pkObjInfo.nObject_Kind;
			soldierBatchSetCharInfo.m_nObjectid = pkObjInfo.nObjectID;
			soldierBatchSetCharInfo.m_SolID = nSolID;
			text = text.ToLower();
			NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(text, NkBundleCallBack.NPCBundleStackName, new NkBundleParam.funcParamBundleCallBack(this.LoadPlunderCharFromUI), soldierBatchSetCharInfo, true);
		}
	}

	public void MakePlunderCharEnemy(PLUNDER_TARGET_INFO _info)
	{
		if (_info.nCharKind < 0)
		{
			return;
		}
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		if (gameObject == null)
		{
			return;
		}
		if (this.m_goSoldierBatchEnemyCharRoot == null)
		{
			this.m_goSoldierBatchEnemyCharRoot = new GameObject("SoldierBatchEnemyCharRoot");
			this.m_goSoldierBatchEnemyCharRoot.transform.parent = gameObject.transform;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(_info.nCharKind);
		if (charKindInfo != null)
		{
			string text = string.Empty;
			if (charKindInfo.IsATB(1L))
			{
				text = "Char/Player/" + charKindInfo.GetBundlePath();
			}
			else
			{
				text = "char/" + charKindInfo.GetBundlePath();
			}
			float num = (float)charKindInfo.GetScale() / 10f;
			num *= 1f;
			GameObject gameObject2 = new GameObject(charKindInfo.GetName());
			gameObject2.transform.parent = this.m_goSoldierBatchEnemyCharRoot.transform;
			if (_info.nStartPos < 0)
			{
				return;
			}
			if (_info.nStartPos >= 3)
			{
				return;
			}
			BATTLE_POS_GRID bATTLE_POS_GRID = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_1][(int)_info.nStartPos];
			Vector3 vector = bATTLE_POS_GRID.mListPos[(int)_info.nBattlePos];
			SoldierBatchGridCell.PickTerrain(vector, ref vector);
			gameObject2.transform.position = vector;
			gameObject2.transform.localRotation = Quaternion.AngleAxis(180f, Vector3.up);
			gameObject2.transform.localScale = new Vector3(num, num, num);
			Transform child = NkUtil.GetChild(gameObject2.transform, "actioncam");
			if (null != child)
			{
				child.gameObject.SetActive(false);
			}
			if (this.m_eSetMode != eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
			{
				Battle_PowerDlg battle_PowerDlg = (Battle_PowerDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.BATTLE_POWER_GROUP_DLG);
				if (battle_PowerDlg != null)
				{
					battle_PowerDlg.Set(gameObject2, (long)_info.nFightPower);
				}
			}
			text = text.ToLower();
			NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(text, NkBundleCallBack.NPCBundleStackName, new NkBundleParam.funcParamBundleCallBack(this.LoadPlunderChar), gameObject2, true);
		}
	}

	public void MakeBabelChar(long nSolID, int nCharKind, long PersonID, byte nBattlePos, long nFightPower)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nCharKind);
		if (charKindInfo != null)
		{
			string text = string.Empty;
			if (charKindInfo.IsATB(1L))
			{
				text = "Char/Player/" + charKindInfo.GetBundlePath();
			}
			else
			{
				text = "char/" + charKindInfo.GetBundlePath();
			}
			GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene);
			if (gameObject == null)
			{
				return;
			}
			if (this.m_goSoldierBatchCharRoot == null)
			{
				this.m_goSoldierBatchCharRoot = new GameObject("SoldierBatchCharRoot");
				this.m_goSoldierBatchCharRoot.transform.parent = gameObject.transform;
			}
			float num = (float)charKindInfo.GetScale() / 10f;
			num *= 1f;
			GameObject gameObject2 = new GameObject(nSolID.ToString());
			gameObject2.transform.parent = this.m_goSoldierBatchCharRoot.transform;
			BATTLE_POS_GRID bATTLE_POS_GRID = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][0];
			Vector3 vector = bATTLE_POS_GRID.mListPos[(int)nBattlePos];
			SoldierBatchGridCell.PickTerrain(vector, ref vector);
			gameObject2.transform.position = vector;
			gameObject2.transform.localScale = new Vector3(num, num, num);
			Transform child = NkUtil.GetChild(gameObject2.transform, "actioncam");
			if (null != child)
			{
				child.gameObject.SetActive(false);
			}
			int key = this.SetCalcBattlePos(0, nBattlePos);
			Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
			dictionary[key].SolID = nSolID;
			dictionary[key].PersonID = PersonID;
			dictionary[key].CharKind = nCharKind;
			this.m_dicBabel_Tower_BatchInfo[key].nSolID = nSolID;
			this.m_dicBabel_Tower_BatchInfo[key].nCharKind = nCharKind;
			this.m_dicBabel_Tower_BatchInfo[key].nPersonID = PersonID;
			this.AddCharFromSolID(nSolID, gameObject2);
			if (this.m_eSetMode != eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
			{
				Battle_PowerDlg battle_PowerDlg = (Battle_PowerDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.BATTLE_POWER_GROUP_DLG);
				if (battle_PowerDlg != null)
				{
					battle_PowerDlg.Set(gameObject2, nFightPower);
				}
			}
			text = text.ToLower();
			NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(text, NkBundleCallBack.NPCBundleStackName, new NkBundleParam.funcParamBundleCallBack(this.LoadPlunderChar), gameObject2, true);
		}
	}

	public void MakePVP2CharFromUI(int nCharKind)
	{
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		if (gameObject == null)
		{
			return;
		}
		if (this.m_goSoldierBatchCharRoot == null)
		{
			this.m_goSoldierBatchCharRoot = new GameObject("SoldierBatchCharRoot");
			this.m_goSoldierBatchCharRoot.transform.parent = gameObject.transform;
		}
		if (this.MakeUpCharInfo.m_SolID != 0L)
		{
			this.InitSelectMoveChar(this.MakeUpCharInfo.m_SolID, this.MakeUpCharInfo.m_FriendPersonID, this.MakeUpCharInfo.m_FriendCharKind);
		}
		GameObject charFromSolID = this.GetCharFromSolID((long)nCharKind);
		if (charFromSolID != null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nCharKind);
		if (charKindInfo != null)
		{
			string text = string.Empty;
			if (charKindInfo.IsATB(1L))
			{
				text = "Char/Player/" + charKindInfo.GetBundlePath();
			}
			else
			{
				text = "char/" + charKindInfo.GetBundlePath();
			}
			float num = (float)charKindInfo.GetScale() / 10f;
			num *= 1f;
			GameObject gameObject2 = new GameObject(nCharKind.ToString());
			gameObject2.transform.parent = this.m_goSoldierBatchCharRoot.transform;
			gameObject2.transform.localScale = new Vector3(num, num, num);
			Transform child = NkUtil.GetChild(gameObject2.transform, "actioncam");
			if (null != child)
			{
				child.gameObject.SetActive(false);
			}
			this.AddCharFromSolID((long)nCharKind, gameObject2);
			SoldierBatchSetCharInfo soldierBatchSetCharInfo = new SoldierBatchSetCharInfo();
			soldierBatchSetCharInfo.m_goChar = gameObject2;
			soldierBatchSetCharInfo.m_FriendPersonID = 0L;
			soldierBatchSetCharInfo.m_FriendCharKind = 0;
			soldierBatchSetCharInfo.m_SolID = (long)nCharKind;
			text = text.ToLower();
			NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(text, NkBundleCallBack.NPCBundleStackName, new NkBundleParam.funcParamBundleCallBack(this.LoadPlunderCharFromUI), soldierBatchSetCharInfo, true);
		}
	}

	public bool ChangePos(long nFromSolID, long nToSolID, long nFriendPersonID, int nFriendCharKInd)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		int nCharKind = 0;
		bool flag = false;
		int num = -1;
		NkSoldierInfo nkSoldierInfo = null;
		if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			nkSoldierInfo = charPersonInfo.GetSoldierInfoFromSolID(nFromSolID);
			if (nkSoldierInfo != null)
			{
				nCharKind = nkSoldierInfo.GetCharKind();
			}
			else if (nFromSolID > 0L)
			{
				if (nFriendCharKInd <= 0)
				{
					return false;
				}
				flag = true;
				nCharKind = nFriendCharKInd;
			}
			else
			{
				num = this.GetObjetIndex(nFromSolID);
				if (num < 0)
				{
					return false;
				}
			}
		}
		else
		{
			nCharKind = (int)nFromSolID;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			NkSoldierInfo nkSoldierInfo2 = null;
			if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2 && nToSolID > 0L)
			{
				nkSoldierInfo2 = charPersonInfo.GetSoldierInfoFromSolID(nToSolID);
				if (nkSoldierInfo2 == null)
				{
					return false;
				}
			}
			if ((SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PLUNDER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP) && nkSoldierInfo != null && nkSoldierInfo2 != null)
			{
				if (nkSoldierInfo2.GetCharKind() == nkSoldierInfo.GetCharKind())
				{
					if (nkSoldierInfo2.GetSolSubData(this.m_eSetMode) > 0L && nkSoldierInfo.GetSolSubData(this.m_eSetMode) > 0L)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						return false;
					}
				}
				else
				{
					if (nkSoldierInfo2.GetSolSubData(this.m_eSetMode) <= 0L && this.IsSameKindCharBatch(nkSoldierInfo2.GetCharKind(), 0L, flag))
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						return false;
					}
					if (nkSoldierInfo.GetSolSubData(this.m_eSetMode) <= 0L && this.IsSameKindCharBatch(nkSoldierInfo.GetCharKind(), 0L, flag))
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						return false;
					}
				}
			}
			long num2 = -1L;
			long num3 = -1L;
			byte objID = 0;
			byte objID2 = 0;
			if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
			{
				if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
				{
					if (nFromSolID > 0L)
					{
						num2 = (long)this.GetTempBattlePos(nFromSolID);
					}
					if (nToSolID > 0L)
					{
						num3 = (long)this.GetTempBattlePos(nToSolID);
					}
					Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
					if (num2 > 0L)
					{
						dictionary[(int)num2].SolID = nToSolID;
						this.SetTempBattlePos(nToSolID, (byte)num2, nkSoldierInfo2.GetCharKind(), false);
					}
					else
					{
						if (nkSoldierInfo2.GetCharKind() != nkSoldierInfo.GetCharKind() && this.IsSameKindCharBatch(nkSoldierInfo.GetCharKind(), 0L, flag))
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
							return false;
						}
						this.SetTempBattlePos(nToSolID, (byte)num2, nkSoldierInfo2.GetCharKind(), true);
					}
					dictionary[(int)num3].SolID = nFromSolID;
					this.SetTempBattlePos(nFromSolID, (byte)num3, nkSoldierInfo.GetCharKind(), false);
				}
				else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
				{
					if (nFromSolID > 0L)
					{
						num2 = (long)this.GetTempBattlePos(nFromSolID);
					}
					if (nToSolID > 0L)
					{
						num3 = (long)this.GetTempBattlePos(nToSolID);
					}
					Dictionary<int, SoldierBatchGrid> dictionary2 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
					if (num2 > 0L)
					{
						dictionary2[(int)num2].SolID = nToSolID;
						this.SetTempBattlePos(nToSolID, (byte)num2, (int)nToSolID, false);
					}
					else
					{
						if (nToSolID != nFromSolID && this.IsSameKindCharBatch((int)nFromSolID, 0L, flag))
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
							return false;
						}
						this.SetTempBattlePos(nToSolID, (byte)num2, (int)nToSolID, true);
					}
					dictionary2[(int)num3].SolID = nFromSolID;
					this.SetTempBattlePos(nFromSolID, (byte)num3, (int)nFromSolID, false);
				}
				else
				{
					if (nFromSolID > 0L)
					{
						num2 = nkSoldierInfo.GetSolSubData(this.m_eSetMode);
					}
					else
					{
						num2 = (long)this.m_DefenceObjectInfo[num].m_nBattlePos;
						objID = this.m_DefenceObjectInfo[num].m_nObjID;
					}
					int num4 = -1;
					if (nToSolID > 0L)
					{
						num3 = nkSoldierInfo2.GetSolSubData(this.m_eSetMode);
					}
					else
					{
						num4 = this.GetObjetIndex(nToSolID);
						if (num4 < 0)
						{
							return false;
						}
						num3 = (long)this.m_DefenceObjectInfo[num4].m_nBattlePos;
						objID2 = this.m_DefenceObjectInfo[num4].m_nObjID;
					}
					Dictionary<int, SoldierBatchGrid> dictionary3 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
					if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP && (num >= 0 || num4 >= 0) && num2 > 0L && num3 > 0L)
					{
						byte b = 0;
						byte b2 = 0;
						byte b3 = 0;
						SoldierBatch.GetCalcBattlePos(num2, ref b, ref b3);
						SoldierBatch.GetCalcBattlePos(num3, ref b2, ref b3);
						if (b != b2)
						{
							byte b4 = 0;
							if (nFromSolID > 0L)
							{
								b4 = b2;
							}
							if (nToSolID > 0L)
							{
								b4 = b;
							}
							NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
							if (kMyCharInfo == null)
							{
								return false;
							}
							int level = kMyCharInfo.GetLevel();
							int a = level / 3;
							int num5 = Mathf.Min(a, 5);
							BATTLE_POS_GRID bATTLE_POS_GRID = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)b4];
							int num6 = 0;
							for (int i = 0; i < bATTLE_POS_GRID.mListPos.Length; i++)
							{
								int key = this.SetCalcBattlePos(b4, (byte)i);
								if (dictionary3[key].SolID > 0L)
								{
									num6++;
								}
							}
							if (num6 >= num5)
							{
								string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("123");
								string empty = string.Empty;
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
								{
									textFromNotify,
									"num",
									num5.ToString()
								});
								Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
								return false;
							}
						}
					}
					if (num2 > 0L)
					{
						dictionary3[(int)num2].SolID = nToSolID;
						dictionary3[(int)num2].ObjID = objID2;
					}
					dictionary3[(int)num3].SolID = nFromSolID;
					dictionary3[(int)num3].ObjID = objID;
					if (num < 0)
					{
						nkSoldierInfo.SetSolSubData((int)this.m_eSetMode, num3);
					}
					else
					{
						if (num2 <= 0L)
						{
							this.m_DefenceObjectInfo[num].m_nObjID = this.MakeUpCharInfo.m_nObjectid;
							dictionary3[(int)num3].ObjID = this.MakeUpCharInfo.m_nObjectid;
						}
						this.m_DefenceObjectInfo[num].m_nBattlePos = (byte)num3;
					}
					if (num4 < 0)
					{
						nkSoldierInfo2.SetSolSubData((int)this.m_eSetMode, num2);
					}
					else if (num2 > 0L)
					{
						this.m_DefenceObjectInfo[num4].m_nBattlePos = (byte)num2;
					}
					else
					{
						this.m_DefenceObjectInfo[num4].m_nBattlePos = 0;
						this.m_DefenceObjectInfo[num4].m_nObjID = 0;
						if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
						{
							PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
							if (plunderSolListDlg != null)
							{
								plunderSolListDlg.SetSolNum(SoldierBatch.SOLDIERBATCH.GetObjCount(), true);
							}
						}
					}
					if (num2 <= 0L && num >= 0)
					{
						NrTable_PlunderObjectinfo_Manager instance = NrTSingleton<NrTable_PlunderObjectinfo_Manager>.Instance;
						if (instance != null)
						{
							PLUNDER_OBJECT_INFO pLUNDER_OBJECT_INFO = instance.Get_Value(this.m_DefenceObjectInfo[num].m_nObjID);
							string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(pLUNDER_OBJECT_INFO.nObject_Kind);
							string empty2 = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("131"),
								"charname",
								name,
								"gold",
								pLUNDER_OBJECT_INFO.nSpendGold.ToString()
							});
							this.msgBox = (NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI);
							this.msgBox.SetMsg(new YesDelegate(this.OnRequestObjectBatchOk), num, new NoDelegate(this.OnRequestObjectBatchCancle), num, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("130"), empty2, eMsgType.MB_OK_CANCEL);
							this.m_lsUiID.Add(this.msgBox.WindowID);
						}
					}
				}
			}
			GameObject charFromSolID = this.GetCharFromSolID(nFromSolID);
			GameObject charFromSolID2 = this.GetCharFromSolID(nToSolID);
			byte key2 = 0;
			byte b5 = 0;
			Vector3 vector = Vector3.zero;
			BATTLE_POS_GRID bATTLE_POS_GRID2;
			if (num2 > 0L)
			{
				SoldierBatch.GetCalcBattlePos(num2, ref key2, ref b5);
				bATTLE_POS_GRID2 = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)key2];
				vector = bATTLE_POS_GRID2.mListPos[(int)b5];
				SoldierBatchGridCell.PickTerrain(vector, ref vector);
				charFromSolID2.transform.position = vector;
			}
			else
			{
				this.RemoveCharFromSolID(nToSolID);
			}
			if (charFromSolID != null)
			{
				TsPositionFollowerTerrain component = charFromSolID.GetComponent<TsPositionFollowerTerrain>();
				if (component != null)
				{
					component.enabled = false;
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "HERO-LAY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
			SoldierBatch.GetCalcBattlePos(num3, ref key2, ref b5);
			bATTLE_POS_GRID2 = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)key2];
			vector = bATTLE_POS_GRID2.mListPos[(int)b5];
			SoldierBatchGridCell.PickTerrain(vector, ref vector);
			charFromSolID.transform.position = vector;
			return true;
		}
		if (SoldierBatch.BABELTOWER_INFO.IsReadyBattle(charPersonInfo.GetPersonID()))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("171"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		int positionBabel_Tower = this.GetPositionBabel_Tower(nToSolID);
		int positionBabel_Tower2 = this.GetPositionBabel_Tower(nFromSolID);
		if (positionBabel_Tower > 0)
		{
			if (positionBabel_Tower2 < 0)
			{
				if (nkSoldierInfo != null && nkSoldierInfo.GetCharKind() != this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower].nCharKind && this.IsSameKindCharBatch(nCharKind, nFromSolID, flag))
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					return false;
				}
				if (this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower].nPersonID != charPersonInfo.GetPersonID())
				{
					int num7 = 0;
					for (int j = 0; j < this.m_dicBabel_Tower_BatchInfo.Count; j++)
					{
						int key3 = this.SetCalcBattlePos(0, (byte)j);
						if (this.m_dicBabel_Tower_BatchInfo[key3].nSolID != 0L && this.m_dicBabel_Tower_BatchInfo[key3].nSolID != nFromSolID && this.m_dicBabel_Tower_BatchInfo[key3].nPersonID == charPersonInfo.GetPersonID())
						{
							num7++;
						}
					}
					if (flag)
					{
						num7 = 0;
						for (int k = 0; k < this.m_dicBabel_Tower_BatchInfo.Count; k++)
						{
							int key4 = this.SetCalcBattlePos(0, (byte)k);
							if (this.m_dicBabel_Tower_BatchInfo[key4].nSolID != 0L && this.m_dicBabel_Tower_BatchInfo[key4].nSolID != nFromSolID && this.m_dicBabel_Tower_BatchInfo[key4].nPersonID != charPersonInfo.GetPersonID())
							{
								num7++;
							}
						}
					}
					byte nCount = SoldierBatch.BABELTOWER_INFO.m_nCount;
					int num8;
					if (nCount == 1)
					{
						if (flag)
						{
							num8 = 3;
						}
						else
						{
							num8 = 9;
						}
					}
					else
					{
						num8 = (int)(12 / nCount + 1);
					}
					if (num7 >= num8)
					{
						string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("123");
						string empty3 = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
						{
							textFromNotify2,
							"num",
							num8.ToString()
						});
						Main_UI_SystemMessage.ADDMessage(empty3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						return false;
					}
				}
				else
				{
					int num9 = 0;
					if (flag)
					{
						for (int l = 0; l < this.m_dicBabel_Tower_BatchInfo.Count; l++)
						{
							int key5 = this.SetCalcBattlePos(0, (byte)l);
							if (this.m_dicBabel_Tower_BatchInfo[key5].nSolID != 0L && this.m_dicBabel_Tower_BatchInfo[key5].nSolID != nFromSolID && this.m_dicBabel_Tower_BatchInfo[key5].nPersonID != charPersonInfo.GetPersonID())
							{
								num9++;
							}
						}
					}
					byte nCount2 = SoldierBatch.BABELTOWER_INFO.m_nCount;
					int num10;
					if (nCount2 == 1)
					{
						if (flag)
						{
							num10 = 3;
						}
						else
						{
							num10 = 9;
						}
					}
					else
					{
						num10 = (int)(12 / nCount2 + 1);
					}
					if (num9 >= num10)
					{
						string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("123");
						string empty4 = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty4, new object[]
						{
							textFromNotify3,
							"num",
							num10.ToString()
						});
						Main_UI_SystemMessage.ADDMessage(empty4, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						return false;
					}
				}
			}
			byte b6 = 0;
			byte nBattlePos = 0;
			SoldierBatch.GetCalcBattlePos((long)positionBabel_Tower, ref b6, ref nBattlePos);
			GS_BABELTOWER_BATTLEPOS_SET_REQ gS_BABELTOWER_BATTLEPOS_SET_REQ = new GS_BABELTOWER_BATTLEPOS_SET_REQ();
			gS_BABELTOWER_BATTLEPOS_SET_REQ.nType = 1;
			gS_BABELTOWER_BATTLEPOS_SET_REQ.nBabelRoomIndex = SoldierBatch.m_cBaberTowerInfo.m_nBabelRoomIndex;
			gS_BABELTOWER_BATTLEPOS_SET_REQ.nLeaderPersonID = SoldierBatch.m_cBaberTowerInfo.m_nLeaderPersonID;
			gS_BABELTOWER_BATTLEPOS_SET_REQ.nSolID = nFromSolID;
			gS_BABELTOWER_BATTLEPOS_SET_REQ.nCharKind = nCharKind;
			gS_BABELTOWER_BATTLEPOS_SET_REQ.nBattlePos = nBattlePos;
			if (flag)
			{
				gS_BABELTOWER_BATTLEPOS_SET_REQ.nFriendPersonID = nFriendPersonID;
			}
			else
			{
				gS_BABELTOWER_BATTLEPOS_SET_REQ.nFriendPersonID = 0L;
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_BATTLEPOS_SET_REQ, gS_BABELTOWER_BATTLEPOS_SET_REQ);
		}
		return true;
	}

	public bool EnableChangePos(long nFromSolID, long nToSolID, long nFriendPersonID, int nFriendCharKInd)
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			return true;
		}
		if (nToSolID > 0L && nFromSolID > 0L)
		{
			return true;
		}
		int num = -1;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NkSoldierInfo nkSoldierInfo = null;
		if (nFromSolID > 0L)
		{
			nkSoldierInfo = charPersonInfo.GetSoldierInfoFromSolID(nFromSolID);
		}
		else
		{
			num = this.GetObjetIndex(nFromSolID);
		}
		long num2 = -1L;
		long num3;
		if (nFromSolID > 0L)
		{
			num3 = nkSoldierInfo.GetSolSubData(this.m_eSetMode);
		}
		else
		{
			if (num < 0)
			{
				return true;
			}
			num3 = (long)this.m_DefenceObjectInfo[num].m_nBattlePos;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2 || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			num3 = (long)this.GetTempBattlePos(nFromSolID);
		}
		if (num3 > 0L)
		{
			return true;
		}
		int num4 = -1;
		if (nToSolID < 0L)
		{
			num4 = this.GetObjetIndex(nToSolID);
			if (num4 < 0)
			{
				return true;
			}
			num2 = (long)this.m_DefenceObjectInfo[num4].m_nBattlePos;
		}
		if (num2 <= 0L || nToSolID >= 0L)
		{
			return true;
		}
		NrTable_PlunderObjectinfo_Manager instance = NrTSingleton<NrTable_PlunderObjectinfo_Manager>.Instance;
		if (instance == null)
		{
			return true;
		}
		PLUNDER_OBJECT_INFO pLUNDER_OBJECT_INFO = instance.Get_Value(this.m_DefenceObjectInfo[num4].m_nObjID);
		if (pLUNDER_OBJECT_INFO == null)
		{
			return true;
		}
		long[] array = new long[]
		{
			nFromSolID,
			nToSolID,
			nFriendPersonID,
			(long)nFriendCharKInd,
			(long)this.MakeUpCharInfo.m_nObjectid
		};
		string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(pLUNDER_OBJECT_INFO.nObject_Kind);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("149"),
			"charname",
			name
		});
		this.msgBox = (NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI);
		this.msgBox.SetMsg(new YesDelegate(this.OnRequestObjectChangeOk), array, new NoDelegate(this.OnRequestObjectChangeCancle), array, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("130"), empty, eMsgType.MB_OK_CANCEL);
		this.m_lsUiID.Add(this.msgBox.WindowID);
		return false;
	}

	public void InitCharBattlePos(long nSolID, long nFriendPersonID, int nFriendCharKind)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		int nCharKind = 0;
		bool flag = false;
		int num = -1;
		bool flag2 = false;
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(nSolID);
		if (soldierInfoFromSolID != null)
		{
			nCharKind = soldierInfoFromSolID.GetCharKind();
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			if (nFriendCharKind <= 0)
			{
				return;
			}
			flag = true;
			nCharKind = nFriendCharKind;
		}
		else
		{
			num = this.GetObjetIndex(nSolID);
			if (num >= 0)
			{
				flag2 = true;
			}
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			if (SoldierBatch.BABELTOWER_INFO.IsReadyBattle(charPersonInfo.GetPersonID()))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("171"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			int positionBabel_Tower = this.GetPositionBabel_Tower(nSolID, nCharKind);
			if (positionBabel_Tower > 0)
			{
				byte b = 0;
				byte nBattlePos = 0;
				SoldierBatch.GetCalcBattlePos((long)positionBabel_Tower, ref b, ref nBattlePos);
				GS_BABELTOWER_BATTLEPOS_SET_REQ gS_BABELTOWER_BATTLEPOS_SET_REQ = new GS_BABELTOWER_BATTLEPOS_SET_REQ();
				gS_BABELTOWER_BATTLEPOS_SET_REQ.nType = 2;
				gS_BABELTOWER_BATTLEPOS_SET_REQ.nBabelRoomIndex = SoldierBatch.m_cBaberTowerInfo.m_nBabelRoomIndex;
				gS_BABELTOWER_BATTLEPOS_SET_REQ.nLeaderPersonID = SoldierBatch.m_cBaberTowerInfo.m_nLeaderPersonID;
				gS_BABELTOWER_BATTLEPOS_SET_REQ.nSolID = nSolID;
				gS_BABELTOWER_BATTLEPOS_SET_REQ.nCharKind = nCharKind;
				gS_BABELTOWER_BATTLEPOS_SET_REQ.nBattlePos = nBattlePos;
				if (flag)
				{
					gS_BABELTOWER_BATTLEPOS_SET_REQ.nFriendPersonID = nFriendPersonID;
				}
				else
				{
					gS_BABELTOWER_BATTLEPOS_SET_REQ.nFriendPersonID = 0L;
				}
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_BATTLEPOS_SET_REQ, gS_BABELTOWER_BATTLEPOS_SET_REQ);
				return;
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2 || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			int tempBattlePos = this.GetTempBattlePos(nSolID);
			if (tempBattlePos > 0)
			{
				Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
				dictionary[tempBattlePos].SolID = 0L;
				dictionary[tempBattlePos].CharKind = 0;
				dictionary[tempBattlePos].PersonID = 0L;
				this.SetTempBattlePos(nSolID, (byte)tempBattlePos, nCharKind, true);
			}
		}
		else if (!flag2)
		{
			long solSubData = soldierInfoFromSolID.GetSolSubData(this.m_eSetMode);
			if (solSubData > 0L)
			{
				Dictionary<int, SoldierBatchGrid> dictionary2 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
				dictionary2[(int)solSubData].SolID = 0L;
				dictionary2[(int)solSubData].CharKind = 0;
				dictionary2[(int)solSubData].PersonID = 0L;
				soldierInfoFromSolID.SetSolSubData((int)this.m_eSetMode, 0L);
			}
		}
		else
		{
			long num2 = (long)this.m_DefenceObjectInfo[num].m_nBattlePos;
			if (num2 > 0L)
			{
				NrTable_PlunderObjectinfo_Manager instance = NrTSingleton<NrTable_PlunderObjectinfo_Manager>.Instance;
				if (instance == null)
				{
					return;
				}
				PLUNDER_OBJECT_INFO pLUNDER_OBJECT_INFO = instance.Get_Value(this.m_DefenceObjectInfo[num].m_nObjID);
				if (pLUNDER_OBJECT_INFO != null)
				{
					string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(pLUNDER_OBJECT_INFO.nObject_Kind);
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("149"),
						"charname",
						name
					});
					this.msgBox = (NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI);
					this.msgBox.SetMsg(new YesDelegate(this.OnRequestObjectDeleteOk), num, new NoDelegate(this.OnRequestObjectDeleteCancle), num, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("130"), empty, eMsgType.MB_OK_CANCEL);
					this.m_lsUiID.Add(this.msgBox.WindowID);
					return;
				}
			}
		}
		this.RemoveCharFromSolID(nSolID);
		if (flag2 && NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
		{
			PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
			if (plunderSolListDlg != null && flag2)
			{
				plunderSolListDlg.SetSolNum(SoldierBatch.SOLDIERBATCH.GetObjCount(), true);
			}
		}
	}

	public bool InsertEmptyGrid(byte nStartPos, byte nGridPos, long nSolID, long nFriendPersonID, int nFriendCharKind, byte nObjID)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		int num = 0;
		bool flag = false;
		bool flag2 = false;
		int num2 = -1;
		NkSoldierInfo nkSoldierInfo = null;
		if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			nkSoldierInfo = charPersonInfo.GetSoldierInfoFromSolID(nSolID);
			if (nkSoldierInfo != null)
			{
				num = nkSoldierInfo.GetCharKind();
			}
			else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
			{
				if (nFriendCharKind <= 0)
				{
					return false;
				}
				flag = true;
				num = nFriendCharKind;
			}
			else
			{
				num2 = this.GetObjetIndex(nSolID);
				if (num2 >= 0)
				{
					flag2 = true;
				}
			}
		}
		else
		{
			num = (int)nSolID;
		}
		GameObject charFromSolID = this.GetCharFromSolID(nSolID);
		if (charFromSolID == null)
		{
			return false;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PLUNDER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			if (!flag2 && nkSoldierInfo.GetSolSubData(this.m_eSetMode) <= 0L && this.IsSameKindCharBatch(num, 0L, flag))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
		}
		else if ((SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2 || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP) && this.IsSameKindCharBatch(num, nSolID, flag))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		BATTLE_POS_GRID bATTLE_POS_GRID = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)nStartPos];
		Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
		int num3 = 0;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			for (int i = 0; i < bATTLE_POS_GRID.mListPos.Length; i++)
			{
				int key = this.SetCalcBattlePos(nStartPos, (byte)i);
				if (this.m_dicBabel_Tower_BatchInfo[key].nSolID != 0L && dictionary[key].SolID != nSolID && this.m_dicBabel_Tower_BatchInfo[key].nPersonID == charPersonInfo.GetPersonID())
				{
					num3++;
				}
			}
			if (flag)
			{
				num3 = 0;
				for (int i = 0; i < bATTLE_POS_GRID.mListPos.Length; i++)
				{
					int key2 = this.SetCalcBattlePos(nStartPos, (byte)i);
					if (this.m_dicBabel_Tower_BatchInfo[key2].nSolID != 0L && dictionary[key2].SolID != nSolID && this.m_dicBabel_Tower_BatchInfo[key2].nPersonID != charPersonInfo.GetPersonID())
					{
						num3++;
					}
				}
			}
		}
		else if (!flag2)
		{
			for (int i = 0; i < bATTLE_POS_GRID.mListPos.Length; i++)
			{
				int key3 = this.SetCalcBattlePos(nStartPos, (byte)i);
				if (dictionary[key3].SolID > 0L && dictionary[key3].SolID != nSolID)
				{
					num3++;
				}
			}
		}
		else
		{
			for (int i = 0; i < bATTLE_POS_GRID.mListPos.Length; i++)
			{
				int key4 = this.SetCalcBattlePos(nStartPos, (byte)i);
				if (dictionary[key4].SolID < 0L && dictionary[key4].SolID != nSolID)
				{
					num3++;
				}
			}
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return false;
		}
		int level = kMyCharInfo.GetLevel();
		int a = level / 3;
		int num4 = Mathf.Min(a, 5);
		if (flag2)
		{
			num4 = 3;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			num4 = 3;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			num4 = 5;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			num4 = 9;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			byte nCount = SoldierBatch.BABELTOWER_INFO.m_nCount;
			if (nCount == 1)
			{
				if (flag)
				{
					num4 = 3;
				}
				else
				{
					num4 = 9;
				}
			}
			else
			{
				num4 = (int)(12 / nCount + 1);
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade((byte)SoldierBatch.EXPEDITION_INFO.m_eExpeditionGrade);
			if (expeditionDataFromGrade != null)
			{
				num4 = expeditionDataFromGrade.Expedition_SolBatch_Array;
			}
			else
			{
				num4 = 15;
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			num4 = 5;
		}
		if (num3 >= num4)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("123");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromNotify,
				"num",
				num4.ToString()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2 || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
			{
				int tempBattlePos = this.GetTempBattlePos(nSolID);
				if (tempBattlePos > 0)
				{
					dictionary[tempBattlePos].SolID = 0L;
					dictionary[tempBattlePos].CharKind = 0;
					dictionary[tempBattlePos].PersonID = 0L;
				}
				int num5 = this.SetCalcBattlePos(nStartPos, nGridPos);
				if (dictionary[num5].SolID != 0L)
				{
					return false;
				}
				dictionary[num5].SolID = nSolID;
				dictionary[num5].CharKind = num;
				dictionary[num5].PersonID = 0L;
				this.SetTempBattlePos(nSolID, (byte)num5, num, false);
			}
			else if (!flag2)
			{
				long solSubData = nkSoldierInfo.GetSolSubData(this.m_eSetMode);
				if (solSubData > 0L)
				{
					dictionary[(int)solSubData].SolID = 0L;
					dictionary[(int)solSubData].CharKind = 0;
					dictionary[(int)solSubData].PersonID = 0L;
				}
				long num6 = (long)this.SetCalcBattlePos(nStartPos, nGridPos);
				if (dictionary[(int)num6].SolID != 0L)
				{
					return false;
				}
				dictionary[(int)num6].SolID = nSolID;
				dictionary[(int)num6].CharKind = num;
				dictionary[(int)num6].PersonID = 0L;
				nkSoldierInfo.SetSolSubData((int)this.m_eSetMode, num6);
			}
			else
			{
				NrTable_PlunderObjectinfo_Manager instance = NrTSingleton<NrTable_PlunderObjectinfo_Manager>.Instance;
				if (instance == null)
				{
					return false;
				}
				long num7 = (long)this.m_DefenceObjectInfo[num2].m_nBattlePos;
				if (num7 > 0L)
				{
					dictionary[(int)num7].SolID = 0L;
					dictionary[(int)num7].CharKind = 0;
					dictionary[(int)num7].PersonID = 0L;
					dictionary[(int)num7].ObjID = 0;
					long num8 = (long)this.SetCalcBattlePos(nStartPos, nGridPos);
					if (dictionary[(int)num8].SolID != 0L)
					{
						return false;
					}
					dictionary[(int)num8].SolID = nSolID;
					dictionary[(int)num8].CharKind = num;
					dictionary[(int)num8].PersonID = 0L;
					dictionary[(int)num8].ObjID = nObjID;
					this.m_DefenceObjectInfo[num2].m_nBattlePos = (byte)num8;
					this.m_DefenceObjectInfo[num2].m_nObjID = nObjID;
				}
				else
				{
					long num9 = (long)this.SetCalcBattlePos(nStartPos, nGridPos);
					if (dictionary[(int)num9].SolID != 0L)
					{
						return false;
					}
					dictionary[(int)num9].SolID = nSolID;
					dictionary[(int)num9].CharKind = num;
					dictionary[(int)num9].PersonID = 0L;
					dictionary[(int)num9].ObjID = nObjID;
					this.m_DefenceObjectInfo[num2].m_nBattlePos = (byte)num9;
					this.m_DefenceObjectInfo[num2].m_nObjID = nObjID;
					PLUNDER_OBJECT_INFO pLUNDER_OBJECT_INFO = instance.Get_Value(this.m_DefenceObjectInfo[num2].m_nObjID);
					string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(pLUNDER_OBJECT_INFO.nObject_Kind);
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("131"),
						"charname",
						name,
						"gold",
						pLUNDER_OBJECT_INFO.nSpendGold.ToString()
					});
					this.msgBox = (NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI);
					this.msgBox.SetMsg(new YesDelegate(this.OnRequestObjectBatchOk), num2, new NoDelegate(this.OnRequestObjectBatchCancle), num2, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("130"), empty2, eMsgType.MB_OK_CANCEL);
					this.m_lsUiID.Add(this.msgBox.WindowID);
				}
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
			{
				PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
				if (plunderSolListDlg != null && flag2)
				{
					plunderSolListDlg.SetSolNum(this.GetObjCount(), true);
				}
			}
			Vector3 vector = bATTLE_POS_GRID.mListPos[(int)nGridPos];
			SoldierBatchGridCell.PickTerrain(vector, ref vector);
			if (charFromSolID != null)
			{
				TsPositionFollowerTerrain component = charFromSolID.GetComponent<TsPositionFollowerTerrain>();
				if (component != null)
				{
					component.enabled = false;
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "HERO-LAY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
			charFromSolID.transform.position = vector;
			return true;
		}
		if (SoldierBatch.BABELTOWER_INFO.IsReadyBattle(charPersonInfo.GetPersonID()))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("171"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		GS_BABELTOWER_BATTLEPOS_SET_REQ gS_BABELTOWER_BATTLEPOS_SET_REQ = new GS_BABELTOWER_BATTLEPOS_SET_REQ();
		gS_BABELTOWER_BATTLEPOS_SET_REQ.nType = 1;
		gS_BABELTOWER_BATTLEPOS_SET_REQ.nBabelRoomIndex = SoldierBatch.m_cBaberTowerInfo.m_nBabelRoomIndex;
		gS_BABELTOWER_BATTLEPOS_SET_REQ.nLeaderPersonID = SoldierBatch.m_cBaberTowerInfo.m_nLeaderPersonID;
		gS_BABELTOWER_BATTLEPOS_SET_REQ.nSolID = nSolID;
		gS_BABELTOWER_BATTLEPOS_SET_REQ.nCharKind = num;
		gS_BABELTOWER_BATTLEPOS_SET_REQ.nBattlePos = nGridPos;
		if (flag)
		{
			gS_BABELTOWER_BATTLEPOS_SET_REQ.nFriendPersonID = nFriendPersonID;
		}
		else
		{
			gS_BABELTOWER_BATTLEPOS_SET_REQ.nFriendPersonID = 0L;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_BATTLEPOS_SET_REQ, gS_BABELTOWER_BATTLEPOS_SET_REQ);
		return true;
	}

	public int GetMaxSolArray()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return -1;
		}
		int level = kMyCharInfo.GetLevel();
		int a = level / 3;
		int num = Mathf.Min(a, 5);
		return num * 3;
	}

	public void CheckSameBattlePos(eSOLDIER_BATCH_MODE eMode)
	{
		eSOL_SUBDATA eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		if (eMode == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP)
		{
			StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP;
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		}
		else if (eMode == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS;
		}
		else if (eMode == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS;
		}
		else if (eMode == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE;
		}
		else if (eMode == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENSE_INFIBATTLE;
		}
		else if (eMode == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		if (eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_NONE)
		{
			return;
		}
		Dictionary<long, long> dictionary = new Dictionary<long, long>();
		long num = 0L;
		long num2 = 0L;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		if (soldierList == null)
		{
			return;
		}
		int num3 = 0;
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.GetSolSubData(eSOL_SUBDATA) > 0L)
			{
				if (eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
				{
					if (dictionary.ContainsKey(nkSoldierInfo.GetSolSubData(eSOL_SUBDATA)))
					{
						num = nkSoldierInfo.GetSolSubData(eSOL_SUBDATA);
						long solID = nkSoldierInfo.GetSolID();
						this.InitCharBattlePos(solID, 0L, 0);
						nkSoldierInfo.SetSolSubData((int)eSOL_SUBDATA, 0L);
						break;
					}
					dictionary.Add(nkSoldierInfo.GetSolSubData(eSOL_SUBDATA), nkSoldierInfo.GetSolID());
				}
				num3++;
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolSubData(eSOL_SUBDATA) > 0L)
			{
				if (eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
				{
					if (dictionary.ContainsKey(current.GetSolSubData(eSOL_SUBDATA)))
					{
						num = current.GetSolSubData(eSOL_SUBDATA);
						long solID = current.GetSolID();
						this.InitCharBattlePos(solID, 0L, 0);
						current.SetSolSubData((int)eSOL_SUBDATA, 0L);
						break;
					}
					dictionary.Add(current.GetSolSubData(eSOL_SUBDATA), current.GetSolID());
				}
				num3++;
			}
		}
		if (num > 0L && dictionary.TryGetValue(num, out num2))
		{
			NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(num2);
			if (soldierInfoFromSolID != null)
			{
				this.InitCharBattlePos(num2, 0L, 0);
				soldierInfoFromSolID.SetSolSubData((int)eSOL_SUBDATA, 0L);
			}
		}
	}

	public void InitSelectMoveChar(long nSolID, long nFriendPersonID, int nFriendCharKind)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		int nCharKind = 0;
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(nSolID);
		if (soldierInfoFromSolID != null)
		{
			nCharKind = soldierInfoFromSolID.GetCharKind();
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			if (nFriendCharKind <= 0)
			{
				return;
			}
			nCharKind = nFriendCharKind;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			nCharKind = (int)nSolID;
		}
		GameObject charFromSolID = this.GetCharFromSolID(nSolID);
		if (charFromSolID == null)
		{
			return;
		}
		if (charFromSolID != null)
		{
			TsPositionFollowerTerrain component = charFromSolID.GetComponent<TsPositionFollowerTerrain>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			long num = (long)this.GetPositionBabel_Tower(nSolID, nCharKind);
			if (num > 0L)
			{
				byte key = 0;
				byte b = 0;
				SoldierBatch.GetCalcBattlePos(num, ref key, ref b);
				BATTLE_POS_GRID bATTLE_POS_GRID = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)key];
				Vector3 vector = bATTLE_POS_GRID.mListPos[(int)b];
				SoldierBatchGridCell.PickTerrain(vector, ref vector);
				charFromSolID.transform.position = vector;
			}
			else
			{
				this.RemoveCharFromSolID(nSolID);
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2 || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			long num2 = (long)this.GetTempBattlePos(nSolID);
			if (num2 > 0L)
			{
				byte key2 = 0;
				byte b2 = 0;
				SoldierBatch.GetCalcBattlePos(num2, ref key2, ref b2);
				BATTLE_POS_GRID bATTLE_POS_GRID2 = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)key2];
				Vector3 vector2 = bATTLE_POS_GRID2.mListPos[(int)b2];
				SoldierBatchGridCell.PickTerrain(vector2, ref vector2);
				charFromSolID.transform.position = vector2;
			}
			else
			{
				this.RemoveCharFromSolID(nSolID);
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			long num3 = (long)this.GetTempBattlePos(nSolID);
			if (num3 > 0L)
			{
				byte key3 = 0;
				byte b3 = 0;
				SoldierBatch.GetCalcBattlePos(num3, ref key3, ref b3);
				BATTLE_POS_GRID bATTLE_POS_GRID3 = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)key3];
				Vector3 vector3 = bATTLE_POS_GRID3.mListPos[(int)b3];
				SoldierBatchGridCell.PickTerrain(vector3, ref vector3);
				charFromSolID.transform.position = vector3;
			}
			else
			{
				this.RemoveCharFromSolID(nSolID);
			}
		}
		else
		{
			long num4;
			if (soldierInfoFromSolID == null && nSolID < 0L)
			{
				int objetIndex = this.GetObjetIndex(nSolID);
				if (objetIndex < 0)
				{
					return;
				}
				num4 = (long)this.m_DefenceObjectInfo[objetIndex].m_nBattlePos;
			}
			else
			{
				num4 = soldierInfoFromSolID.GetSolSubData(this.m_eSetMode);
			}
			if (num4 > 0L)
			{
				byte key4 = 0;
				byte b4 = 0;
				SoldierBatch.GetCalcBattlePos(num4, ref key4, ref b4);
				BATTLE_POS_GRID bATTLE_POS_GRID4 = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)key4];
				Vector3 vector4 = bATTLE_POS_GRID4.mListPos[(int)b4];
				SoldierBatchGridCell.PickTerrain(vector4, ref vector4);
				charFromSolID.transform.position = vector4;
			}
			else
			{
				this.RemoveCharFromSolID(nSolID);
			}
		}
	}

	public bool IsHeroBatch()
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2 || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			return true;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		if (soldierList == null)
		{
			return false;
		}
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.GetSolSubData(this.m_eSetMode) > 0L)
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkSoldierInfo.GetCharKind());
				if (charKindInfo != null && charKindInfo.IsATB(1L))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsHeroBabelBatch()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
		{
			int key = this.SetCalcBattlePos(0, (byte)i);
			if (this.m_dicBabel_Tower_BatchInfo[key].nPersonID == charPersonInfo.GetPersonID() && this.m_dicBabel_Tower_BatchInfo[key].nSolID > 0L)
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_dicBabel_Tower_BatchInfo[key].nCharKind);
				if (charKindInfo != null && charKindInfo.IsATB(1L))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsHeroGuildBossBatch()
	{
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return false;
		}
		for (int i = 0; i < 9; i++)
		{
			if (this.m_TempBattlePos[i].m_nSolID > 0L)
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_TempBattlePos[i].m_nCharKind);
				if (charKindInfo != null && charKindInfo.IsATB(1L))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsSameKindCharBatch(int nCharKind, long nSolID, bool bFriend)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nCharKind);
			bool flag = charKindInfo.IsATB(1L);
			if (flag)
			{
				return false;
			}
			if (bFriend)
			{
				return false;
			}
			int partyCount = (int)SoldierBatch.BABELTOWER_INFO.GetPartyCount();
			for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
			{
				int key = this.SetCalcBattlePos(0, (byte)i);
				if (this.m_dicBabel_Tower_BatchInfo[key].nCharKind == nCharKind && this.m_dicBabel_Tower_BatchInfo[key].nSolID != nSolID)
				{
					if (partyCount != 1 || this.m_dicBabel_Tower_BatchInfo[key].nPersonID == charPersonInfo.GetPersonID())
					{
						return true;
					}
				}
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2 || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			clTempBattlePos[] tempBattlePosInfo = this.GetTempBattlePosInfo();
			for (int j = 0; j < 15; j++)
			{
				if (tempBattlePosInfo[j].m_nSolID > 0L)
				{
					if (tempBattlePosInfo[j].m_nCharKind == nCharKind && tempBattlePosInfo[j].m_nSolID != nSolID)
					{
						return true;
					}
				}
			}
		}
		else
		{
			NrSoldierList soldierList = charPersonInfo.GetSoldierList();
			if (soldierList == null)
			{
				return false;
			}
			NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
			for (int k = 0; k < kSolInfo.Length; k++)
			{
				NkSoldierInfo nkSoldierInfo = kSolInfo[k];
				if (nkSoldierInfo.GetSolSubData(this.m_eSetMode) > 0L && nkSoldierInfo.GetCharKind() == nCharKind)
				{
					return true;
				}
			}
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null)
			{
				return false;
			}
			foreach (NkSoldierInfo current in readySolList.GetList().Values)
			{
				if (current.GetSolSubData(this.m_eSetMode) > 0L && current.GetCharKind() == nCharKind)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public int GetPositionBabel_Tower(long nSolid, int nCharKind)
	{
		for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
		{
			int num = this.SetCalcBattlePos(0, (byte)i);
			if (this.m_dicBabel_Tower_BatchInfo[num].nCharKind == nCharKind && this.m_dicBabel_Tower_BatchInfo[num].nSolID == nSolid)
			{
				return num;
			}
		}
		return -1;
	}

	public int GetPositionBabel_Tower(long nSolid)
	{
		for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
		{
			int num = this.SetCalcBattlePos(0, (byte)i);
			if (this.m_dicBabel_Tower_BatchInfo[num].nSolID == nSolid)
			{
				return num;
			}
		}
		return -1;
	}

	public void Update()
	{
		if (this.m_bRemoveLoadingEffect)
		{
			if (this.m_fRemoveLoadingEffect == 0f)
			{
				this.m_fRemoveLoadingEffect = Time.realtimeSinceStartup;
			}
			if (Time.realtimeSinceStartup - this.m_fRemoveLoadingEffect < 3f)
			{
				return;
			}
			NewLoaingDlg newLoaingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE) as NewLoaingDlg;
			if (newLoaingDlg != null)
			{
				newLoaingDlg.RemoveLoadingPageEffect();
			}
			PlunderStartAndReMatchDlg plunderStartAndReMatchDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDER_STARTANDREMATCH_DLG) as PlunderStartAndReMatchDlg;
			if (plunderStartAndReMatchDlg != null)
			{
				plunderStartAndReMatchDlg.Show();
			}
			this.m_fRemoveLoadingEffect = 0f;
			this.m_bRemoveLoadingEffect = false;
		}
	}

	public void SetBabelTowerSoldierBatch(GS_BABELTOWER_BATTLEPOS_SET_ACK _ACK)
	{
		if (_ACK.nResult != 0)
		{
			if (_ACK.nResult == 8003 || _ACK.nResult == 8006)
			{
				this.InitSelectMoveChar(_ACK.nSolID, _ACK.nPersonID, _ACK.nCharKind);
			}
			else
			{
				this.RemoveCharFromSolID(_ACK.nSolID);
			}
			if (_ACK.nResult == 8002 || _ACK.nResult == 8003)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("189"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (_ACK.nResult == 8004)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("209"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (_ACK.nResult == 8004)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("210"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		BATTLE_POS_GRID bATTLE_POS_GRID = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][0];
		if (_ACK.nType == 2)
		{
			int positionBabel_Tower = this.GetPositionBabel_Tower(_ACK.nSolID, _ACK.nCharKind);
			if (positionBabel_Tower > 0)
			{
				Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
				dictionary[positionBabel_Tower].SolID = 0L;
				dictionary[positionBabel_Tower].PersonID = 0L;
				dictionary[positionBabel_Tower].CharKind = 0;
				this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower].Init();
			}
			this.RemoveCharFromSolID(_ACK.nSolID);
		}
		else if (_ACK.nType == 1)
		{
			GameObject charFromSolID = this.GetCharFromSolID(_ACK.nSolID);
			if (charFromSolID == null)
			{
				Dictionary<int, SoldierBatchGrid> dictionary2 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
				int positionBabel_Tower2 = this.GetPositionBabel_Tower(_ACK.nSolID, _ACK.nCharKind);
				if (positionBabel_Tower2 > 0)
				{
					dictionary2[positionBabel_Tower2].SolID = 0L;
					dictionary2[positionBabel_Tower2].PersonID = 0L;
					dictionary2[positionBabel_Tower2].CharKind = 0;
					this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower2].Init();
				}
				this.MakeBabelChar(_ACK.nSolID, _ACK.nCharKind, _ACK.nPersonID, _ACK.nBattlePos, _ACK.nFightPower);
			}
			else
			{
				Dictionary<int, SoldierBatchGrid> dictionary3 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
				int positionBabel_Tower3 = this.GetPositionBabel_Tower(_ACK.nSolID, _ACK.nCharKind);
				if (positionBabel_Tower3 > 0)
				{
					dictionary3[positionBabel_Tower3].SolID = 0L;
					dictionary3[positionBabel_Tower3].CharKind = 0;
					dictionary3[positionBabel_Tower3].PersonID = 0L;
					this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower3].Init();
				}
				int key = this.SetCalcBattlePos(0, _ACK.nBattlePos);
				if (dictionary3[key].SolID != 0L)
				{
					return;
				}
				dictionary3[key].SolID = _ACK.nSolID;
				dictionary3[key].CharKind = _ACK.nCharKind;
				dictionary3[key].PersonID = _ACK.nPersonID;
				this.m_dicBabel_Tower_BatchInfo[key].nSolID = _ACK.nSolID;
				this.m_dicBabel_Tower_BatchInfo[key].nCharKind = _ACK.nCharKind;
				this.m_dicBabel_Tower_BatchInfo[key].nPersonID = _ACK.nPersonID;
				Vector3 vector = bATTLE_POS_GRID.mListPos[(int)_ACK.nBattlePos];
				SoldierBatchGridCell.PickTerrain(vector, ref vector);
				if (charFromSolID != null)
				{
					TsPositionFollowerTerrain component = charFromSolID.GetComponent<TsPositionFollowerTerrain>();
					if (component != null)
					{
						component.enabled = false;
						TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "HERO-LAY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
					}
				}
				charFromSolID.transform.position = vector;
			}
		}
		else if (_ACK.nType == 3)
		{
			GameObject charFromSolID2 = this.GetCharFromSolID(_ACK.nSolID);
			GameObject charFromSolID3 = this.GetCharFromSolID(_ACK.nMoveSolID);
			int positionBabel_Tower4 = this.GetPositionBabel_Tower(_ACK.nSolID, _ACK.nCharKind);
			int positionBabel_Tower5 = this.GetPositionBabel_Tower(_ACK.nMoveSolID, _ACK.nMoveCharKind);
			Dictionary<int, SoldierBatchGrid> dictionary4 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
			if (positionBabel_Tower4 > 0)
			{
				dictionary4[positionBabel_Tower4].SolID = _ACK.nMoveSolID;
				dictionary4[positionBabel_Tower4].CharKind = _ACK.nMoveCharKind;
				dictionary4[positionBabel_Tower4].PersonID = _ACK.nMovePersonID;
				this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower4].nSolID = _ACK.nMoveSolID;
				this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower4].nCharKind = _ACK.nMoveCharKind;
				this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower4].nPersonID = _ACK.nMovePersonID;
			}
			dictionary4[positionBabel_Tower5].SolID = _ACK.nSolID;
			dictionary4[positionBabel_Tower5].CharKind = _ACK.nCharKind;
			dictionary4[positionBabel_Tower5].PersonID = _ACK.nPersonID;
			this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower5].nSolID = _ACK.nSolID;
			this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower5].nCharKind = _ACK.nCharKind;
			this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower5].nPersonID = _ACK.nPersonID;
			if (charFromSolID2 != null)
			{
				Vector3 vector2 = bATTLE_POS_GRID.mListPos[(int)_ACK.nBattlePos];
				SoldierBatchGridCell.PickTerrain(vector2, ref vector2);
				if (charFromSolID2 != null)
				{
					TsPositionFollowerTerrain component2 = charFromSolID2.GetComponent<TsPositionFollowerTerrain>();
					if (component2 != null)
					{
						component2.enabled = false;
						TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "HERO-LAY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
					}
				}
				charFromSolID2.transform.position = vector2;
			}
			else
			{
				this.MakeBabelChar(_ACK.nSolID, _ACK.nCharKind, _ACK.nPersonID, _ACK.nBattlePos, _ACK.nFightPower);
			}
			if (charFromSolID3 != null)
			{
				Vector3 vector3 = bATTLE_POS_GRID.mListPos[(int)_ACK.nMoveBattlePos];
				SoldierBatchGridCell.PickTerrain(vector3, ref vector3);
				if (charFromSolID3 != null)
				{
					TsPositionFollowerTerrain component3 = charFromSolID3.GetComponent<TsPositionFollowerTerrain>();
					if (component3 != null)
					{
						component3.enabled = false;
						TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "HERO-LAY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
					}
				}
				charFromSolID3.transform.position = vector3;
			}
			else
			{
				this.MakeBabelChar(_ACK.nMoveSolID, _ACK.nMoveCharKind, _ACK.nMovePersonID, _ACK.nMoveBattlePos, _ACK.nMoveFightPower);
			}
		}
		else if (_ACK.nType == 4)
		{
			GameObject charFromSolID4 = this.GetCharFromSolID(_ACK.nSolID);
			int positionBabel_Tower6 = this.GetPositionBabel_Tower(_ACK.nSolID, _ACK.nCharKind);
			int positionBabel_Tower7 = this.GetPositionBabel_Tower(_ACK.nMoveSolID, _ACK.nMoveCharKind);
			Dictionary<int, SoldierBatchGrid> dictionary5 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
			if (positionBabel_Tower6 > 0)
			{
				dictionary5[positionBabel_Tower6].SolID = 0L;
				dictionary5[positionBabel_Tower6].PersonID = 0L;
				dictionary5[positionBabel_Tower6].CharKind = 0;
				this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower6].Init();
			}
			this.RemoveCharFromSolID(_ACK.nMoveSolID);
			dictionary5[positionBabel_Tower7].SolID = _ACK.nSolID;
			dictionary5[positionBabel_Tower7].CharKind = _ACK.nCharKind;
			dictionary5[positionBabel_Tower7].PersonID = _ACK.nPersonID;
			this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower7].nSolID = _ACK.nSolID;
			this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower7].nCharKind = _ACK.nCharKind;
			this.m_dicBabel_Tower_BatchInfo[positionBabel_Tower7].nPersonID = _ACK.nPersonID;
			if (charFromSolID4 != null)
			{
				Vector3 vector4 = bATTLE_POS_GRID.mListPos[(int)_ACK.nBattlePos];
				SoldierBatchGridCell.PickTerrain(vector4, ref vector4);
				if (charFromSolID4 != null)
				{
					TsPositionFollowerTerrain component4 = charFromSolID4.GetComponent<TsPositionFollowerTerrain>();
					if (component4 != null)
					{
						component4.enabled = false;
						TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "HERO-LAY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
					}
				}
				charFromSolID4.transform.position = vector4;
			}
			else
			{
				this.MakeBabelChar(_ACK.nSolID, _ACK.nCharKind, _ACK.nPersonID, _ACK.nBattlePos, _ACK.nFightPower);
			}
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
		{
			PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
			if (plunderSolListDlg != null)
			{
				if (_ACK.nSolID > 0L)
				{
					plunderSolListDlg.UpdateSolList(_ACK.nSolID);
				}
				if (_ACK.nMoveSolID > 0L)
				{
					plunderSolListDlg.UpdateSolList(_ACK.nMoveSolID);
				}
				int solBatchNum = this.GetSolBatchNum();
				plunderSolListDlg.SetSolNum(solBatchNum, false);
			}
		}
	}

	public void Babel_TowerBatchPosInit()
	{
		Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
		for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
		{
			int key = this.SetCalcBattlePos(0, (byte)i);
			this.m_dicBabel_Tower_BatchInfo[key].Init();
			dictionary[key].SolID = 0L;
			dictionary[key].PersonID = 0L;
			dictionary[key].CharKind = 0;
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
		{
			PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
			if (plunderSolListDlg != null)
			{
				plunderSolListDlg.SetSolList();
			}
		}
		if (this.m_goSoldierBatchCharRoot != null)
		{
			UnityEngine.Object.Destroy(this.m_goSoldierBatchCharRoot);
			this.m_goSoldierBatchCharRoot = null;
		}
		this.m_dicSolChar.Clear();
	}

	public void DeleteBabelBatchInfoFromSolID(long nSolID)
	{
		Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
		for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
		{
			int key = this.SetCalcBattlePos(0, (byte)i);
			if (this.m_dicBabel_Tower_BatchInfo[key].nSolID == nSolID)
			{
				this.RemoveCharFromSolID(this.m_dicBabel_Tower_BatchInfo[key].nSolID);
				this.m_dicBabel_Tower_BatchInfo[key].Init();
				dictionary[key].SolID = 0L;
				dictionary[key].PersonID = 0L;
				dictionary[key].CharKind = 0;
			}
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
		{
			PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
			if (plunderSolListDlg != null)
			{
				plunderSolListDlg.SetSolList();
			}
		}
	}

	public void DeleteBabelBatchInfoFromPersonID(long nPersonID)
	{
		Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
		for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
		{
			int key = this.SetCalcBattlePos(0, (byte)i);
			if (this.m_dicBabel_Tower_BatchInfo[key].nPersonID == nPersonID)
			{
				this.RemoveCharFromSolID(this.m_dicBabel_Tower_BatchInfo[key].nSolID);
				this.m_dicBabel_Tower_BatchInfo[key].Init();
				dictionary[key].SolID = 0L;
				dictionary[key].PersonID = 0L;
				dictionary[key].CharKind = 0;
			}
		}
	}

	public long GetBabelTowerSolIDFromIndex(int nIndex)
	{
		for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
		{
			if (i == nIndex)
			{
				int key = this.SetCalcBattlePos(0, (byte)i);
				return this.m_dicBabel_Tower_BatchInfo[key].nSolID;
			}
		}
		return 0L;
	}

	public int GetBabelTowerTotalBatchInfoCount()
	{
		return this.m_dicBabel_Tower_BatchInfo.Count;
	}

	public int GetBabelTowerSolCount()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		int num = 0;
		for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
		{
			int key = this.SetCalcBattlePos(0, (byte)i);
			if (this.m_dicBabel_Tower_BatchInfo[key].nPersonID == charPersonInfo.GetPersonID())
			{
				if (this.m_dicBabel_Tower_BatchInfo[key].nSolID > 0L)
				{
					num++;
				}
			}
			else if (SoldierBatch.BABELTOWER_INFO.Count == 1 && this.m_dicBabel_Tower_BatchInfo[key].nSolID > 0L)
			{
				num++;
			}
		}
		return num;
	}

	public int GetBabelTowerFriendSolCount()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		int num = 0;
		for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
		{
			int key = this.SetCalcBattlePos(0, (byte)i);
			if (this.m_dicBabel_Tower_BatchInfo[key].nPersonID != charPersonInfo.GetPersonID() && this.m_dicBabel_Tower_BatchInfo[key].nSolID > 0L)
			{
				num++;
			}
		}
		return num;
	}

	public int GetBabelTowerSolCount(long personID)
	{
		int num = 0;
		for (int i = 0; i < this.m_dicBabel_Tower_BatchInfo.Count; i++)
		{
			int key = this.SetCalcBattlePos(0, (byte)i);
			if (this.m_dicBabel_Tower_BatchInfo[key].nPersonID == personID)
			{
				if (this.m_dicBabel_Tower_BatchInfo[key].nSolID > 0L)
				{
					num++;
				}
			}
			else if (SoldierBatch.BABELTOWER_INFO.Count == 1 && this.m_dicBabel_Tower_BatchInfo[key].nSolID > 0L)
			{
				num++;
			}
		}
		return num;
	}

	public void LoadBatchSolInfo()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		for (byte b = 1; b <= 20; b += 1)
		{
			string str = "Babel Solpos";
			if (b >= 17)
			{
				string value = "0";
				PlayerPrefs.SetString(str + b.ToString(), value);
			}
			else
			{
				string @string = PlayerPrefs.GetString(str + b.ToString());
				if (@string != string.Empty)
				{
					long num = long.Parse(@string);
					TsLog.Log("(Get Babel Solpos {0}", new object[]
					{
						num
					});
					NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(num);
					byte b2 = 0;
					byte nBattlePos = 0;
					SoldierBatch.GetCalcBattlePos((long)b, ref b2, ref nBattlePos);
					if (soldierInfoFromSolID != null && !soldierInfoFromSolID.IsInjuryStatus())
					{
						GS_BABELTOWER_BATTLEPOS_SET_REQ gS_BABELTOWER_BATTLEPOS_SET_REQ = new GS_BABELTOWER_BATTLEPOS_SET_REQ();
						gS_BABELTOWER_BATTLEPOS_SET_REQ.nType = 1;
						gS_BABELTOWER_BATTLEPOS_SET_REQ.nBabelRoomIndex = SoldierBatch.m_cBaberTowerInfo.m_nBabelRoomIndex;
						gS_BABELTOWER_BATTLEPOS_SET_REQ.nLeaderPersonID = SoldierBatch.m_cBaberTowerInfo.m_nLeaderPersonID;
						gS_BABELTOWER_BATTLEPOS_SET_REQ.nSolID = num;
						gS_BABELTOWER_BATTLEPOS_SET_REQ.nCharKind = soldierInfoFromSolID.GetCharKind();
						gS_BABELTOWER_BATTLEPOS_SET_REQ.nBattlePos = nBattlePos;
						SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_BATTLEPOS_SET_REQ, gS_BABELTOWER_BATTLEPOS_SET_REQ);
					}
				}
			}
		}
	}

	public void SaveBatchSolInfo()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		string str = "Babel Solpos";
		for (byte b = 1; b <= 20; b += 1)
		{
			if (this.m_dicBabel_Tower_BatchInfo[(int)b] != null)
			{
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(this.m_dicBabel_Tower_BatchInfo[(int)b].nSolID);
				TsLog.Log("(Set Babel Solpos {0}", new object[]
				{
					this.m_dicBabel_Tower_BatchInfo[(int)b].nSolID
				});
				if (soldierInfoFromSolID != null)
				{
					string value = soldierInfoFromSolID.GetSolID().ToString();
					PlayerPrefs.SetString(str + b.ToString(), value);
				}
				else
				{
					string value2 = "0";
					PlayerPrefs.SetString(str + b.ToString(), value2);
				}
			}
		}
	}

	public void LoadGuildBossBatchSolInfo()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		for (byte b = 1; b <= 16; b += 1)
		{
			string str = "GuildBoss Solpos";
			string @string = PlayerPrefs.GetString(str + b.ToString());
			if (@string != string.Empty)
			{
				long num = long.Parse(@string);
				TsLog.Log("(Get Babel Solpos {0}", new object[]
				{
					num
				});
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(num);
				byte nStartPos = 0;
				byte nGridPos = 0;
				SoldierBatch.GetCalcBattlePos((long)b, ref nStartPos, ref nGridPos);
				if (soldierInfoFromSolID != null && !soldierInfoFromSolID.IsInjuryStatus())
				{
					this.MakePlunderCharFromUI(num, 0L, 0, 0L);
					this.InsertEmptyGrid(nStartPos, nGridPos, num, 0L, 0, 0);
				}
			}
		}
	}

	public void SaveGuildBossBatchSolInfo()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		string str = "GuildBoss Solpos";
		for (byte b = 1; b <= 16; b += 1)
		{
			string value = "0";
			PlayerPrefs.SetString(str + b.ToString(), value);
		}
		clTempBattlePos[] tempBattlePosInfo = SoldierBatch.SOLDIERBATCH.GetTempBattlePosInfo();
		for (int i = 0; i < 9; i++)
		{
			if (tempBattlePosInfo[i].m_nSolID > 0L)
			{
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(tempBattlePosInfo[i].m_nSolID);
				TsLog.Log("(Set Babel Solpos {0}", new object[]
				{
					tempBattlePosInfo[i].m_nSolID
				});
				if (soldierInfoFromSolID != null)
				{
					string value2 = soldierInfoFromSolID.GetSolID().ToString();
					PlayerPrefs.SetString(str + tempBattlePosInfo[i].m_nBattlePos.ToString(), value2);
				}
			}
		}
	}

	public int FindEmptyObjInfo()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_DefenceObjectInfo[i].m_nObjID <= 0)
			{
				return i;
			}
		}
		return -1;
	}

	public int GetObjetIndex(long nSolIdx)
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_DefenceObjectInfo[i].m_nSolID == nSolIdx)
			{
				return i;
			}
		}
		return -1;
	}

	public int GetObjCount()
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (this.m_DefenceObjectInfo[i].m_nObjID > 0 && this.m_DefenceObjectInfo[i].m_nBattlePos > 0)
			{
				num++;
			}
		}
		return num;
	}

	public int GetTempCount()
	{
		int num = 0;
		int num2 = 1;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			num2 = 5;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			num2 = 9;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			num2 = 3;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			num2 = 15;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			num2 = 5;
		}
		for (int i = 0; i < num2; i++)
		{
			if (this.m_TempBattlePos[i].m_nSolID > 0L && this.m_TempBattlePos[i].m_nBattlePos > 0)
			{
				num++;
			}
		}
		return num;
	}

	public SUBDATA_UNION GetObjectDataToSubData()
	{
		return new SUBDATA_UNION
		{
			n8SubData_0 = (sbyte)this.m_DefenceObjectInfo[0].m_nObjID,
			n8SubData_1 = (sbyte)this.m_DefenceObjectInfo[0].m_nBattlePos,
			n8SubData_2 = (sbyte)this.m_DefenceObjectInfo[1].m_nObjID,
			n8SubData_3 = (sbyte)this.m_DefenceObjectInfo[1].m_nBattlePos,
			n8SubData_4 = (sbyte)this.m_DefenceObjectInfo[2].m_nObjID,
			n8SubData_5 = (sbyte)this.m_DefenceObjectInfo[2].m_nBattlePos
		};
	}

	public void OnRequestObjectBatchOk(object a_oObject)
	{
		this.m_lsUiID.Remove(this.msgBox.WindowID);
		NrTable_PlunderObjectinfo_Manager instance = NrTSingleton<NrTable_PlunderObjectinfo_Manager>.Instance;
		if (instance == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int num = (int)a_oObject;
		if (num < 0)
		{
			return;
		}
		if (this.m_DefenceObjectInfo[num].m_nObjID < 0)
		{
			return;
		}
		PLUNDER_OBJECT_INFO pLUNDER_OBJECT_INFO = instance.Get_Value(this.m_DefenceObjectInfo[num].m_nObjID);
		if (pLUNDER_OBJECT_INFO == null)
		{
			this.GS_PLUNDER_OBJECT_BATCH_ACK(new GS_PLUNDER_OBJECT_BATCH_ACK
			{
				nResult = 1,
				nObjectID = this.m_DefenceObjectInfo[num].m_nObjID,
				nSolID = this.m_DefenceObjectInfo[num].m_nSolID,
				nBattlePos = this.m_DefenceObjectInfo[num].m_nBattlePos,
				nSubData = this.GetObjectDataToSubData().nSubData
			});
			return;
		}
		if (pLUNDER_OBJECT_INFO.nNeedLevel > kMyCharInfo.GetLevel())
		{
			this.GS_PLUNDER_OBJECT_BATCH_ACK(new GS_PLUNDER_OBJECT_BATCH_ACK
			{
				nResult = 2,
				nObjectID = this.m_DefenceObjectInfo[num].m_nObjID,
				nSolID = this.m_DefenceObjectInfo[num].m_nSolID,
				nBattlePos = this.m_DefenceObjectInfo[num].m_nBattlePos,
				nSubData = this.GetObjectDataToSubData().nSubData
			});
			return;
		}
		if (pLUNDER_OBJECT_INFO.nSpendGold > kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			this.GS_PLUNDER_OBJECT_BATCH_ACK(new GS_PLUNDER_OBJECT_BATCH_ACK
			{
				nResult = 3,
				nObjectID = this.m_DefenceObjectInfo[num].m_nObjID,
				nSolID = this.m_DefenceObjectInfo[num].m_nSolID,
				nBattlePos = this.m_DefenceObjectInfo[num].m_nBattlePos,
				nSubData = this.GetObjectDataToSubData().nSubData
			});
			return;
		}
		GS_PLUNDER_OBJECT_BATCH_REQ gS_PLUNDER_OBJECT_BATCH_REQ = new GS_PLUNDER_OBJECT_BATCH_REQ();
		gS_PLUNDER_OBJECT_BATCH_REQ.nObjectID = this.m_DefenceObjectInfo[num].m_nObjID;
		gS_PLUNDER_OBJECT_BATCH_REQ.nSolID = this.m_DefenceObjectInfo[num].m_nSolID;
		gS_PLUNDER_OBJECT_BATCH_REQ.nBattlePos = this.m_DefenceObjectInfo[num].m_nBattlePos;
		gS_PLUNDER_OBJECT_BATCH_REQ.nSubData = this.GetObjectDataToSubData().nSubData;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_OBJECT_BATCH_REQ, gS_PLUNDER_OBJECT_BATCH_REQ);
	}

	public void OnRequestObjectBatchCancle(object a_oObject)
	{
		this.m_lsUiID.Remove(this.msgBox.WindowID);
		int num = (int)a_oObject;
		if (num < 0)
		{
			return;
		}
		if (this.m_DefenceObjectInfo[num].m_nObjID < 0)
		{
			return;
		}
		this.RemoveCharFromSolID(this.m_DefenceObjectInfo[num].m_nSolID);
		long num2 = (long)this.m_DefenceObjectInfo[num].m_nBattlePos;
		if (num2 > 0L)
		{
			Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
			dictionary[(int)num2].SolID = 0L;
			dictionary[(int)num2].CharKind = 0;
			dictionary[(int)num2].PersonID = 0L;
			this.m_DefenceObjectInfo[num].m_nObjID = 0;
			this.m_DefenceObjectInfo[num].m_nBattlePos = 0;
		}
	}

	public void GS_PLUNDER_OBJECT_BATCH_ACK(GS_PLUNDER_OBJECT_BATCH_ACK pAck)
	{
		int objetIndex = this.GetObjetIndex(pAck.nSolID);
		if (objetIndex < 0)
		{
			return;
		}
		if (pAck.nResult != 0)
		{
			this.RemoveCharFromSolID(this.m_DefenceObjectInfo[objetIndex].m_nSolID);
			long num = (long)this.m_DefenceObjectInfo[objetIndex].m_nBattlePos;
			if (num > 0L)
			{
				Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
				dictionary[(int)num].SolID = 0L;
				dictionary[(int)num].CharKind = 0;
				dictionary[(int)num].PersonID = 0L;
				this.m_DefenceObjectInfo[objetIndex].m_nObjID = 0;
				this.m_DefenceObjectInfo[objetIndex].m_nBattlePos = 0;
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
			{
				PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
				if (plunderSolListDlg != null)
				{
					plunderSolListDlg.SetSolNum(this.GetObjCount(), true);
				}
			}
			return;
		}
		if (this.m_DefenceObjectInfo[objetIndex].m_nObjID != pAck.nObjectID)
		{
			this.RemoveCharFromSolID(this.m_DefenceObjectInfo[objetIndex].m_nSolID);
			long num2 = (long)this.m_DefenceObjectInfo[objetIndex].m_nBattlePos;
			if (num2 > 0L)
			{
				Dictionary<int, SoldierBatchGrid> dictionary2 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
				dictionary2[(int)num2].SolID = 0L;
				dictionary2[(int)num2].CharKind = 0;
				dictionary2[(int)num2].PersonID = 0L;
				this.m_DefenceObjectInfo[objetIndex].m_nObjID = 0;
				this.m_DefenceObjectInfo[objetIndex].m_nBattlePos = 0;
			}
		}
		if (this.m_DefenceObjectInfo[objetIndex].m_nBattlePos != pAck.nBattlePos)
		{
			this.RemoveCharFromSolID(this.m_DefenceObjectInfo[objetIndex].m_nSolID);
			long num3 = (long)this.m_DefenceObjectInfo[objetIndex].m_nBattlePos;
			if (num3 > 0L)
			{
				Dictionary<int, SoldierBatchGrid> dictionary3 = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
				dictionary3[(int)num3].SolID = 0L;
				dictionary3[(int)num3].CharKind = 0;
				dictionary3[(int)num3].PersonID = 0L;
				this.m_DefenceObjectInfo[objetIndex].m_nObjID = 0;
				this.m_DefenceObjectInfo[objetIndex].m_nBattlePos = 0;
			}
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
		{
			PlunderSolListDlg plunderSolListDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
			if (plunderSolListDlg2 != null)
			{
				plunderSolListDlg2.SetSolNum(this.GetObjCount(), true);
			}
		}
	}

	public void OnRequestObjectDeleteOk(object a_oObject)
	{
		this.m_lsUiID.Remove(this.msgBox.WindowID);
		int num = (int)a_oObject;
		if (num < 0)
		{
			return;
		}
		if (this.m_DefenceObjectInfo[num].m_nObjID < 0)
		{
			return;
		}
		this.RemoveCharFromSolID(this.m_DefenceObjectInfo[num].m_nSolID);
		long num2 = (long)this.m_DefenceObjectInfo[num].m_nBattlePos;
		if (num2 > 0L)
		{
			Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
			dictionary[(int)num2].SolID = 0L;
			dictionary[(int)num2].CharKind = 0;
			dictionary[(int)num2].PersonID = 0L;
			this.m_DefenceObjectInfo[num].m_nObjID = 0;
			this.m_DefenceObjectInfo[num].m_nBattlePos = 0;
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
		{
			PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
			if (plunderSolListDlg != null)
			{
				plunderSolListDlg.SetSolNum(SoldierBatch.SOLDIERBATCH.GetObjCount(), true);
			}
		}
	}

	public void OnRequestObjectDeleteCancle(object a_oObject)
	{
		this.m_lsUiID.Remove(this.msgBox.WindowID);
		int num = (int)a_oObject;
		if (num < 0)
		{
			return;
		}
		if (this.m_DefenceObjectInfo[num].m_nObjID < 0)
		{
			return;
		}
		GameObject charFromSolID = this.GetCharFromSolID(this.m_DefenceObjectInfo[num].m_nSolID);
		if (charFromSolID == null)
		{
			return;
		}
		long num2 = (long)this.m_DefenceObjectInfo[num].m_nBattlePos;
		if (num2 > 0L)
		{
			byte key = 0;
			byte b = 0;
			SoldierBatch.GetCalcBattlePos(num2, ref key, ref b);
			BATTLE_POS_GRID bATTLE_POS_GRID = this.m_SoldierBatchPosGrid[eBATTLE_ALLY.eBATTLE_ALLY_0][(int)key];
			Vector3 vector = bATTLE_POS_GRID.mListPos[(int)b];
			SoldierBatchGridCell.PickTerrain(vector, ref vector);
			charFromSolID.transform.position = vector;
			if (charFromSolID != null)
			{
				TsPositionFollowerTerrain component = charFromSolID.GetComponent<TsPositionFollowerTerrain>();
				if (component != null)
				{
					component.enabled = false;
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "HERO-LAY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
			charFromSolID.transform.position = vector;
		}
	}

	public void OnRequestObjectChangeOk(object a_oObject)
	{
		this.m_lsUiID.Remove(this.msgBox.WindowID);
		long[] array = (long[])a_oObject;
		this.MakeUpCharInfo.m_nObjectid = (byte)array[4];
		if (!this.ChangePos(array[0], array[1], array[2], (int)array[3]))
		{
			this.InitSelectMoveChar(this.MakeUpCharInfo.m_SolID, this.MakeUpCharInfo.m_FriendPersonID, this.MakeUpCharInfo.m_FriendCharKind);
		}
	}

	public void OnRequestObjectChangeCancle(object a_oObject)
	{
		this.m_lsUiID.Remove(this.msgBox.WindowID);
		long[] array = (long[])a_oObject;
		long solID = array[0];
		this.RemoveCharFromSolID(solID);
	}

	public void SetChangeBabelBatchGrid()
	{
		Dictionary<int, SoldierBatchGrid> dictionary = this.m_SoldierBatchGridObj[eBATTLE_ALLY.eBATTLE_ALLY_0];
		if (dictionary == null)
		{
			return;
		}
		bool flag = false;
		if (SoldierBatch.BABELTOWER_INFO.GetPartyCount() > 1)
		{
			flag = true;
		}
		if (flag)
		{
			foreach (SoldierBatchGrid current in dictionary.Values)
			{
				if (!(current == null))
				{
					if (current.INDEX >= 16)
					{
						current.GridShowHide(true);
					}
				}
			}
		}
		else
		{
			foreach (SoldierBatchGrid current2 in dictionary.Values)
			{
				if (!(current2 == null))
				{
					if (current2.INDEX >= 16)
					{
						this.InitCharBattlePos(current2.SolID, 0L, 0);
						current2.GridShowHide(false);
					}
				}
			}
		}
	}

	public void SavePvpMakeup2SolInfo()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			myCharInfo.SetCharSubData(31 + i, 0L);
			SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
			byte b = 0;
			byte b2 = 0;
			SoldierBatch.GetCalcBattlePos((long)this.m_TempBattlePos[i].m_nBattlePos, ref b, ref b2);
			if (b2 >= 0 && b2 < 9)
			{
				if (this.m_TempBattlePos[i].m_nSolID > 0L)
				{
					sUBDATA_UNION.n32SubData_0 = (int)this.m_TempBattlePos[i].m_nBattlePos;
					sUBDATA_UNION.n32SubData_1 = (int)this.m_TempBattlePos[i].m_nSolID;
					myCharInfo.SetCharSubData(31 + i, sUBDATA_UNION.nSubData);
				}
			}
		}
	}

	private int LoadPVPMakeup2BatchSolInfo()
	{
		int num = 0;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return 0;
		}
		for (int i = 0; i < 3; i++)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COLOSSEUMBATCH1 + i);
			if (charSubData != 0L)
			{
				SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
				sUBDATA_UNION.nSubData = charSubData;
				int n32SubData_ = sUBDATA_UNION.n32SubData_0;
				int n32SubData_2 = sUBDATA_UNION.n32SubData_1;
				byte nStartPos = 0;
				byte b = 0;
				SoldierBatch.GetCalcBattlePos((long)n32SubData_, ref nStartPos, ref b);
				if (b >= 0 && b < 9)
				{
					if (n32SubData_2 > 0)
					{
						if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsEnableBatchColosseumSoldier(n32SubData_2))
						{
							this.MakePVP2CharFromUI(n32SubData_2);
							this.InsertEmptyGrid(nStartPos, b, (long)n32SubData_2, 0L, 0, 0);
							num++;
						}
					}
				}
			}
		}
		return num;
	}
}
