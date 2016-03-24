using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using PROTOCOL.WORLD;
using SERVICE;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NkCharManager : NrTSingleton<NkCharManager>
{
	private NrCharBase[] m_arChar;

	private int m_nCharCount;

	private int m_nUserCharCount;

	private int[] m_nMaxUserNum = new int[]
	{
		6,
		7,
		8,
		9,
		10
	};

	private int[] m_nMaxUserNumByMap = new int[]
	{
		10,
		15,
		20,
		25,
		30
	};

	private bool m_bShowCharUnique;

	private NrCharDefine.CharLODStep m_eCharLODStep = NrCharDefine.CharLODStep.CHARLOD_STEP_1;

	public NrCharAccountInfo m_kCharAccountInfo;

	public NrMyCharInfo m_kMyCharInfo;

	private bool m_bCharListSetComplete;

	private int m_nSelectedCharID;

	private List<NrReservedCharMakeInfo> m_lsFirstGeneration;

	private List<NrReservedCharMakeInfo> m_lsSecondGeneration;

	private Vector3 m_vLastMyCharPos = Vector3.zero;

	private bool m_bNeedSort;

	public int m_nGenerationCharCount;

	private bool m_bCharActiveFrom = true;

	private bool m_bCharActiveTo = true;

	private bool m_bFakeShadowEnable;

	private bool m_bInputControl = true;

	private float m_fWorldLoadSafeTime;

	private short i16MostNearNpcUnique;

	private Dictionary<short, int> m_dicIndunCharATB;

	private bool m_bPlunderProtectEnd_30;

	private bool m_bPlunderProtectEnd_10;

	private bool m_bPlunderProtectEnd_5;

	private bool m_bPlunderProtectEnd;

	private float m_lCongraturationTimeFriend;

	private float m_lCongraturationTimeALL;

	private float m_fCongraturationTimeSpecial;

	private long m_InjuryCureSolID;

	private bool m_isInjuryCureAllChar;

	private bool m_bCharacterChangeName;

	public bool gProp_isShowIntro
	{
		get;
		set;
	}

	public bool InputControl
	{
		get
		{
			return this.m_bInputControl;
		}
		set
		{
			this.m_bInputControl = value;
		}
	}

	public short I16MostNearNpcUnique
	{
		get
		{
			return this.i16MostNearNpcUnique;
		}
		set
		{
			this.i16MostNearNpcUnique = value;
		}
	}

	public bool IsInjuryCureAllChar
	{
		get
		{
			return this.m_isInjuryCureAllChar;
		}
		set
		{
			this.m_isInjuryCureAllChar = value;
			this.m_InjuryCureSolID = 0L;
		}
	}

	public bool CharacterChangeName
	{
		get
		{
			return this.m_bCharacterChangeName;
		}
		set
		{
			this.m_bCharacterChangeName = value;
		}
	}

	public bool CharacterListSetComplete
	{
		get
		{
			return this.m_bCharListSetComplete;
		}
		set
		{
			this.m_bCharListSetComplete = value;
		}
	}

	public int SelectedCharID
	{
		get
		{
			return this.m_nSelectedCharID;
		}
		set
		{
			this.m_nSelectedCharID = value;
		}
	}

	private NkCharManager()
	{
		this.m_arChar = new NrCharBase[300];
		this.m_nCharCount = 0;
		this.m_nUserCharCount = 0;
		this.m_bShowCharUnique = false;
		this.m_eCharLODStep = NrCharDefine.CharLODStep.CHARLOD_STEP_1;
		this.m_kCharAccountInfo = new NrCharAccountInfo();
		this.m_kMyCharInfo = new NrMyCharInfo();
		this.m_bCharListSetComplete = false;
		this.m_nSelectedCharID = 0;
		this.m_lsFirstGeneration = new List<NrReservedCharMakeInfo>();
		this.m_lsSecondGeneration = new List<NrReservedCharMakeInfo>();
		this.gProp_isShowIntro = false;
		this.m_bCharActiveFrom = true;
		this.m_bCharActiveTo = true;
		this.m_bFakeShadowEnable = false;
		this.m_bInputControl = true;
		this.m_dicIndunCharATB = new Dictionary<short, int>();
		this.m_bPlunderProtectEnd_30 = false;
		this.m_bPlunderProtectEnd_10 = false;
		this.m_bPlunderProtectEnd_5 = false;
		this.m_bPlunderProtectEnd = false;
		this.m_lCongraturationTimeFriend = 0f;
		this.m_lCongraturationTimeALL = 0f;
	}

	public NrCharBase[] Get_Char()
	{
		return this.m_arChar;
	}

	public void Init(bool gameworld)
	{
		if (gameworld && NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
		{
			this.m_kMyCharInfo.BackupPersonInfo(this.GetCharPersonInfo(1));
		}
		else
		{
			this.m_kMyCharInfo.Init();
		}
		this.DeleteAllChar();
		this.m_nGenerationCharCount = 0;
		this.m_nCharCount = 0;
		this.m_nUserCharCount = 0;
		this.m_lsFirstGeneration.Clear();
		this.m_lsSecondGeneration.Clear();
		this.m_dicIndunCharATB.Clear();
		this.m_fWorldLoadSafeTime = 0f;
		NrTSingleton<NkBundleCallBack>.Instance.ClearNPCBundleStack();
	}

	public bool CheckCongraturationTime(byte byReceibeUerType)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return false;
		}
		switch (byReceibeUerType)
		{
		case 1:
		{
			if (this.m_lCongraturationTimeFriend == 0f)
			{
				this.m_lCongraturationTimeFriend = Time.time;
				return true;
			}
			float num = Time.time - this.m_lCongraturationTimeFriend;
			float num2 = 180f;
			if (num > num2)
			{
				this.m_lCongraturationTimeFriend = Time.time;
				return true;
			}
			return false;
		}
		case 3:
		{
			if (this.m_fCongraturationTimeSpecial == 0f)
			{
				this.m_fCongraturationTimeSpecial = Time.time;
				return true;
			}
			float num3 = Time.time - this.m_fCongraturationTimeSpecial;
			float num4 = 180f;
			if (num3 > num4)
			{
				this.m_fCongraturationTimeSpecial = Time.time;
				return true;
			}
			return false;
		}
		}
		if (this.m_lCongraturationTimeALL == 0f)
		{
			this.m_lCongraturationTimeALL = Time.time;
			return true;
		}
		float num5 = Time.time - this.m_lCongraturationTimeALL;
		float num6 = 300f;
		if (num5 > num6)
		{
			this.m_lCongraturationTimeALL = Time.time;
			return true;
		}
		return false;
	}

	public void InitExceptMyChar(bool bRemove3DModel)
	{
		for (int i = 2; i <= this.m_nCharCount; i++)
		{
			if (this.m_arChar[i] != null)
			{
				this.m_arChar[i].DeleteChar();
				this.m_arChar[i] = null;
			}
		}
		if (this.m_arChar[1] == null)
		{
			this.m_nCharCount = 0;
			this.m_nUserCharCount = 0;
		}
		else
		{
			this.m_nCharCount = 1;
			this.m_nUserCharCount = 1;
			if (bRemove3DModel)
			{
				this.m_arChar[1].Release();
				this.m_arChar[1].Set3DCharStep(NrCharBase.e3DCharStep.IDLE);
			}
		}
	}

	public void InitChar3DModelAll()
	{
		this.ChangeChar3DStepToDeleted();
	}

	public bool IsCharMoveEnable()
	{
		Scene.Type curScene = Scene.CurScene;
		switch (curScene)
		{
		case Scene.Type.SELECTCHAR:
		case Scene.Type.BATTLE:
		case Scene.Type.CUTSCENE:
		case Scene.Type.SOLDIER_BATCH:
			return false;
		case Scene.Type.PREPAREGAME:
		case Scene.Type.JUSTWAIT:
		case Scene.Type.WORLD:
		case Scene.Type.DUNGEON:
			IL_2E:
			if (curScene != Scene.Type.EMPTY)
			{
				return this.m_bCharActiveFrom == this.m_bCharActiveTo && this.m_bCharActiveFrom && this.m_bCharActiveTo && this.m_bInputControl;
			}
			return false;
		}
		goto IL_2E;
	}

	public int FindEmptyChar()
	{
		int num = 1;
		for (int i = num; i < 300; i++)
		{
			if (this.m_arChar[i] == null)
			{
				return i;
			}
		}
		return -1;
	}

	public int FindCharIDByCharUnique(short charunique)
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				if (nrCharBase.GetCharUnique() == charunique)
				{
					return nrCharBase.GetID();
				}
			}
		}
		return -1;
	}

	public int SetChar(WS_CHARLIST_ACK.NEW_CHARLIST_INFO charinfo, bool bSelectChar)
	{
		eCharKindType kindtype = eCharKindType.CKT_USER;
		NrPersonInfoUser nrPersonInfoUser = new NrPersonInfoUser();
		nrPersonInfoUser.SetUserData(charinfo);
		int num = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.GetCharUniqueNum() + 1;
		int num2 = NrTSingleton<NkCharManager>.Instance.SetChar((short)num, charinfo.CharKind, kindtype, nrPersonInfoUser, 1L, 0f);
		if (bSelectChar)
		{
			NrTSingleton<NkCharManager>.Instance.SelectedCharID = num2;
		}
		NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.AddSlotChar(num2);
		return num2;
	}

	public int SetChar(NEW_MAKECHAR_INFO MakeCharInfo, bool bReserve, bool bSubNpc)
	{
		if (TsPlatform.IsMobile && MakeCharInfo.CharKindType == 0)
		{
			int currLevel = (int)TsQualityManager.Instance.CurrLevel;
			if (currLevel <= 4 && this.m_nUserCharCount >= this.GetMaxUserNum(currLevel))
			{
				return -1;
			}
		}
		if (MakeCharInfo == null)
		{
			return -1;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(MakeCharInfo.CharKind);
		if (charKindInfo == null)
		{
			Debug.Log("CharKindInfo not found (CharKind = " + MakeCharInfo.CharKind.ToString());
			return -1;
		}
		if (bReserve)
		{
			this.AddReservedCharMakeInfo(MakeCharInfo);
			return -1;
		}
		if (charKindInfo != null)
		{
			eCharKindType charKindType = (eCharKindType)MakeCharInfo.CharKindType;
			NrPersonInfoBase nrPersonInfoBase;
			if (charKindInfo.IsATB(1L))
			{
				nrPersonInfoBase = new NrPersonInfoUser();
			}
			else
			{
				nrPersonInfoBase = new NrPersonInfoNPC();
			}
			nrPersonInfoBase.SetUserData(MakeCharInfo);
			return this.SetChar(MakeCharInfo.CharUnique, MakeCharInfo.CharKind, charKindType, nrPersonInfoBase, MakeCharInfo.Status, MakeCharInfo.Speed);
		}
		Debug.LogWarning("CharKind not found !!!!! Kind = " + MakeCharInfo.CharKind.ToString());
		return -1;
	}

	public int SetChar(short charunique, int charkind, eCharKindType kindtype, NrPersonInfoBase kPersonInfo, long eCharStatus, float speed)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(charkind);
		if (charKindInfo == null)
		{
			Debug.Log("CharKindInfo not found (CharKind = " + charkind.ToString());
			return -1;
		}
		eCharKindType eCharKindType = kindtype;
		if (charKindInfo.IsATB(1L))
		{
			eCharKindType = eCharKindType.CKT_USER;
		}
		if (charKindInfo.IsATB(2L))
		{
			eCharKindType = eCharKindType.CKT_SOLDIER;
		}
		if (charKindInfo.IsATB(8L))
		{
			eCharKindType = eCharKindType.CKT_NPC;
		}
		if (charKindInfo.IsATB(4L))
		{
			eCharKindType = eCharKindType.CKT_MONSTER;
		}
		if (charKindInfo.IsATB(16L))
		{
			eCharKindType = eCharKindType.CKT_OBJECT;
		}
		int num = this.AddNewChar(charunique, eCharKindType, eCharStatus);
		if (num < 0)
		{
			if (eCharKindType == eCharKindType.CKT_NPC)
			{
				GS_QUEST_INFO gS_QUEST_INFO = new GS_QUEST_INFO();
				gS_QUEST_INFO.nType = 0;
				TKString.StringChar("2_" + charKindInfo.GetName() + "_" + charunique.ToString(), ref gS_QUEST_INFO.strInfo);
				SendPacket.GetInstance().SendObject(1003, gS_QUEST_INFO);
			}
			return num;
		}
		NrCharBase @char = this.GetChar(num);
		if (@char != null)
		{
			if (num == 1 && NrTSingleton<NkClientLogic>.Instance.IsGameWorld() && this.m_kMyCharInfo.IsBackupPersonInfo())
			{
				this.m_kMyCharInfo.m_kBackupPersonInfo.SetCharPos(kPersonInfo.GetCharPos());
				this.m_kMyCharInfo.m_kBackupPersonInfo.SetDirection(kPersonInfo.GetDirection());
				if (this.m_kMyCharInfo.m_kBackupPersonInfo.GetPersonID() <= 0L)
				{
					this.m_kMyCharInfo.m_kBackupPersonInfo.SetPersonID(kPersonInfo.GetPersonID());
					this.m_kMyCharInfo.m_kBackupPersonInfo.SetCharName(kPersonInfo.GetCharName());
					this.m_kMyCharInfo.m_kBackupPersonInfo.SetSolID(kPersonInfo.GetSolID());
				}
				@char.SetPersonInfo(this.m_kMyCharInfo.m_kBackupPersonInfo);
				@char.SetReadyPartInfo();
				NrCharUser nrCharUser = @char as NrCharUser;
				if (nrCharUser != null)
				{
					int faceCharKind = this.m_kMyCharInfo.GetFaceCharKind();
					byte faceSolGrade = this.m_kMyCharInfo.GetFaceSolGrade();
					long faceSolID = this.m_kMyCharInfo.GetFaceSolID();
					int faceCostumeUnique = this.m_kMyCharInfo.GetFaceCostumeUnique();
					nrCharUser.ChangeCharModel(faceCharKind, faceSolGrade, faceSolID, faceCostumeUnique);
				}
				GS_CLIENT_STANDBY_REQ gS_CLIENT_STANDBY_REQ = new GS_CLIENT_STANDBY_REQ();
				gS_CLIENT_STANDBY_REQ.Mode = 1;
				if (TsPlatform.IsLowSystemMemory)
				{
					gS_CLIENT_STANDBY_REQ.LowMemory = 1;
				}
				else
				{
					gS_CLIENT_STANDBY_REQ.LowMemory = 0;
				}
				if (TsPlatform.IsMobile && !TsPlatform.IsEditor && (NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea() == eSERVICE_AREA.SERVICE_ANDROID_CNTEST || NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea() == eSERVICE_AREA.SERVICE_IOS_CNTEST))
				{
					NrTSingleton<NkClientLogic>.Instance.GetClientInfo(ref gS_CLIENT_STANDBY_REQ.kClientInfo);
				}
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CLIENT_STANDBY_REQ, gS_CLIENT_STANDBY_REQ);
			}
			else
			{
				@char.SetPersonInfo(kPersonInfo);
				if (kindtype == eCharKindType.CKT_USER)
				{
					if (!NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
					{
						@char.SetReadyPartInfo();
					}
					else if (num == 1)
					{
						GS_CLIENT_STANDBY_REQ gS_CLIENT_STANDBY_REQ2 = new GS_CLIENT_STANDBY_REQ();
						gS_CLIENT_STANDBY_REQ2.Mode = 0;
						if (TsPlatform.IsLowSystemMemory)
						{
							gS_CLIENT_STANDBY_REQ2.LowMemory = 1;
						}
						else
						{
							gS_CLIENT_STANDBY_REQ2.LowMemory = 0;
						}
						if (TsPlatform.IsMobile && !TsPlatform.IsEditor && (NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea() == eSERVICE_AREA.SERVICE_ANDROID_CNTEST || NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea() == eSERVICE_AREA.SERVICE_IOS_CNTEST))
						{
							NrTSingleton<NkClientLogic>.Instance.GetClientInfo(ref gS_CLIENT_STANDBY_REQ2.kClientInfo);
						}
						SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CLIENT_STANDBY_REQ, gS_CLIENT_STANDBY_REQ2);
						NrTSingleton<NkQuestManager>.Instance.m_bIsProlog = false;
					}
					else
					{
						GS_USERCHAR_INFO_REQ gS_USERCHAR_INFO_REQ = new GS_USERCHAR_INFO_REQ();
						gS_USERCHAR_INFO_REQ.CharUnique = @char.GetCharUnique();
						SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_USERCHAR_INFO_REQ, gS_USERCHAR_INFO_REQ);
					}
				}
				else if (eCharKindType == eCharKindType.CKT_USER)
				{
					@char.SetReadyPartInfo();
				}
			}
			if (speed > 0f)
			{
				NrPersonInfoBase personInfo = @char.GetPersonInfo();
				if (personInfo != null)
				{
					personInfo.SetMoveSpeed(speed);
				}
			}
			if (kindtype == eCharKindType.CKT_NPC && @char.IsCharStateAtb(24L))
			{
				@char.MoveTo(kPersonInfo.GetDirection().x, kPersonInfo.GetDirection().y, kPersonInfo.GetDirection().z, false);
			}
		}
		if (this.m_kMyCharInfo.m_nIndunUnique != -1)
		{
			int reservationIndunCharATB = this.GetReservationIndunCharATB(@char.GetCharUnique());
			if (reservationIndunCharATB != 0)
			{
				@char.SetIndunCharATB(reservationIndunCharATB);
			}
		}
		return num;
	}

	public void SetMyChar(GS_LOAD_CHAR_NFY pkMyCharInfo)
	{
		this.m_kMyCharInfo.m_PersonID = pkMyCharInfo.PersonID;
		this.m_kMyCharInfo.m_Money = pkMyCharInfo.Money;
		this.m_kMyCharInfo.m_kCharMapInfo.MapUnique = pkMyCharInfo.MapUnique;
		this.m_kMyCharInfo.m_i64LastLoginTime = pkMyCharInfo.m_lLoginTime;
		this.m_kMyCharInfo.m_i64CreateDate = pkMyCharInfo.m_CreateDate;
		this.m_kMyCharInfo.m_i64TotalPlayTime = pkMyCharInfo.m_TotalPlayTime;
		this.m_kMyCharInfo.m_kCharMapInfo.m_nBattleMapIdx = pkMyCharInfo.m_iBattleMapID;
		this.m_kMyCharInfo.SetActivityPoint(pkMyCharInfo.ActivityPoint);
		this.m_kMyCharInfo.SetActivityTime(pkMyCharInfo.nServerTime);
		this.m_kMyCharInfo.SetVipActivityAddTime(pkMyCharInfo.VipActivityAddTime);
		this.m_kMyCharInfo.SetEquipSellMoney(pkMyCharInfo.m_nEquipSellMoney, false);
		this.m_kMyCharInfo.SetHeroPoint(pkMyCharInfo.m_nHeroPoint);
		this.m_kMyCharInfo.SetEquipPoint(pkMyCharInfo.m_nEquipPoint);
	}

	public void SetMyChar(GS_WARP_ACK pkMyCharInfo)
	{
		this.m_kMyCharInfo.m_kCharMapInfo.MapUnique = pkMyCharInfo.MapUnique;
		this.m_kMyCharInfo.m_kCharMapInfo.m_nBattleMapIdx = (short)pkMyCharInfo.BattleMapIdx;
		this.m_kMyCharInfo.m_bNoMove = false;
	}

	public void SetDummyChar(short charunique)
	{
		int charkind = 1;
		NrPersonInfoUser nrPersonInfoUser = new NrPersonInfoUser();
		nrPersonInfoUser.SetCharKind(0, charkind);
		this.SetChar(charunique, charkind, eCharKindType.CKT_USER, nrPersonInfoUser, 0L, 0f);
	}

	public int AddNewChar(short charunique, eCharKindType kindtype, long eCharStatus)
	{
		int num = this.FindCharIDByCharUnique(charunique);
		if (num < 0)
		{
			num = this.FindEmptyChar();
			if (num >= 0 && !this.AddChar(num, charunique, kindtype, eCharStatus))
			{
				return -1;
			}
		}
		return num;
	}

	public bool AddChar(int id, short charunique, eCharKindType kindtype, long eCharStatus)
	{
		if (this.m_arChar[id] != null)
		{
			return false;
		}
		switch (kindtype)
		{
		case eCharKindType.CKT_USER:
			this.m_arChar[id] = new NrCharUser();
			this.m_nUserCharCount++;
			break;
		case eCharKindType.CKT_SOLDIER:
			this.m_arChar[id] = new NrCharSoldier();
			break;
		case eCharKindType.CKT_MONSTER:
			this.m_arChar[id] = new NrCharMonster();
			break;
		case eCharKindType.CKT_NPC:
			this.m_arChar[id] = new NrCharNPC();
			break;
		case eCharKindType.CKT_OBJECT:
			this.m_arChar[id] = new NrCharObject();
			break;
		default:
			return false;
		}
		this.m_arChar[id].Init();
		NkCharIDInfo iDInfo = new NkCharIDInfo(id, charunique);
		this.m_arChar[id].SetIDInfo(iDInfo);
		this.m_arChar[id].SetCharState_ALL(eCharStatus);
		this.m_nCharCount++;
		return true;
	}

	private int GetMaxUserNum(int qualitylevel)
	{
		if (!TsPlatform.IsMobile)
		{
			return this.m_nMaxUserNum[qualitylevel] * 2;
		}
		if (this.m_kMyCharInfo.m_kCharMapInfo.MapIndex == 12)
		{
			return this.m_nMaxUserNumByMap[qualitylevel];
		}
		return this.m_nMaxUserNum[qualitylevel];
	}

	public NrMyCharInfo GetMyCharInfo()
	{
		return this.m_kMyCharInfo;
	}

	public NrCharBase GetCharClone(int id)
	{
		if (this.m_arChar[id] != null)
		{
			return (NrCharBase)this.m_arChar[id].Clone();
		}
		return null;
	}

	public NrCharBase GetCharByCharUnique(short charunique)
	{
		int num = this.FindCharIDByCharUnique(charunique);
		if (num < 0)
		{
			return null;
		}
		return this.m_arChar[num];
	}

	public NrCharBase GetCharByPersonID(long personid)
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				if (nrCharBase.GetPersonInfo().GetPersonID() == personid)
				{
					return nrCharBase;
				}
			}
		}
		return null;
	}

	public NrCharBase GetCharByCharKind(int charkind)
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				if (nrCharBase.GetPersonInfo().GetKind(0) == charkind)
				{
					return nrCharBase;
				}
			}
		}
		return null;
	}

	public NrCharBase GetChar(int id)
	{
		if (id < 0 || id >= 300)
		{
			return null;
		}
		return this.m_arChar[id];
	}

	public NrCharBase GetChar(GameObject kGameObj)
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				if (nrCharBase.Get3DChar() != null)
				{
					if (nrCharBase.Get3DChar().GetRootGameObject() == kGameObj)
					{
						return nrCharBase;
					}
				}
			}
		}
		return null;
	}

	public GameObject GetMyCharObject()
	{
		return this.m_kMyCharInfo.GetMyCharObject();
	}

	public NrPersonInfoUser GetCharPersonInfo(int id)
	{
		NrCharBase @char = this.GetChar(id);
		if (@char == null)
		{
			return null;
		}
		return @char.GetPersonInfo() as NrPersonInfoUser;
	}

	public NrSoldierList GetCharSoldierList(int id)
	{
		NrCharBase @char = this.GetChar(id);
		if (@char == null)
		{
			return null;
		}
		return @char.GetPersonInfo().m_kSoldierList;
	}

	public NrCharKindInfo GetCharKindInfo(int id)
	{
		NrCharBase @char = this.GetChar(id);
		if (@char == null)
		{
			return null;
		}
		return @char.GetCharKindInfo();
	}

	public bool DeleteChar(int id)
	{
		if (id < 0 || id >= 300)
		{
			return false;
		}
		if (this.m_arChar[id] != null)
		{
			bool flag = this.m_arChar[id].IsCharKindATB(1L);
			this.m_arChar[id].DeleteChar();
			this.m_arChar[id] = null;
			this.m_nCharCount--;
			if (flag)
			{
				this.m_nUserCharCount--;
			}
			return true;
		}
		return false;
	}

	public void DeleteAllChar()
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				int iD = nrCharBase.GetID();
				if (iD >= 0 && this.m_arChar[iD] != null)
				{
					this.m_arChar[iD].DeleteChar();
					this.m_arChar[iD] = null;
				}
			}
		}
		this.m_nCharCount = 0;
		this.m_nUserCharCount = 0;
		this.m_lsFirstGeneration.Clear();
		this.m_lsSecondGeneration.Clear();
		NrTSingleton<NrNpcPosManager>.Instance.ClearWideCollArea();
	}

	public void DeleteBattleChar()
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				int iD = nrCharBase.GetID();
				NrPersonInfoBase personInfo = nrCharBase.GetPersonInfo();
				if (personInfo != null && personInfo.IsBattleChar() && iD >= 0 && this.m_arChar[iD] != null)
				{
					this.m_arChar[iD].DeleteChar();
					this.m_arChar[iD] = null;
				}
			}
		}
	}

	public void ReleaseChar(NrCharBase kCharInst)
	{
		this.m_arChar[kCharInst.GetID()] = null;
		kCharInst.Release();
	}

	public void DeleteTerritoryChar()
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				if (NrCharDefine.IsTerritoryChar(nrCharBase.GetCharUnique()))
				{
					int iD = nrCharBase.GetID();
					if (iD >= 0 && this.m_arChar[iD] != null)
					{
						this.m_arChar[iD].DeleteChar();
						this.m_arChar[iD] = null;
					}
				}
			}
		}
	}

	public void Update()
	{
		if (!NkClientLogic.bWorldCharUpdate)
		{
			return;
		}
		this.ProcessGenerateChars();
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null && nrCharBase.GetID() >= 0)
			{
				nrCharBase.Update();
			}
		}
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			Vector3 charPos = @char.m_kCharMove.GetCharPos();
			if (Vector3.Distance(charPos, this.m_vLastMyCharPos) > 15f)
			{
				this.m_vLastMyCharPos = charPos;
				this.m_bNeedSort = true;
			}
			@char.GetPersonInfo().UpdateSoldierInfo();
			this.m_kMyCharInfo.UpdateReadySoldierInfo();
		}
		if (this.m_bCharActiveFrom != this.m_bCharActiveTo)
		{
			this.SetChildActiveAll();
		}
		if (NrTSingleton<ContentsLimitManager>.Instance.IsHeroBattle() && this.m_kMyCharInfo.GetLevel() >= 10)
		{
			this.UpdatePlunderShieldTime();
		}
	}

	public void LateUpdate()
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
		{
			return;
		}
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null && nrCharBase.GetID() >= 1)
			{
				nrCharBase.LateUpdate(this.m_vLastMyCharPos);
			}
		}
		if (NrTSingleton<NkClientLogic>.Instance.IsRemainFrame(100))
		{
			NrTSingleton<NrNpcPosManager>.Instance.CleanWideCollArea();
		}
	}

	public void UpdatePlunderShieldTime()
	{
		long charDetail = this.m_kMyCharInfo.GetCharDetail(13);
		long num = charDetail / 60L;
		if (num <= 0L)
		{
			return;
		}
		long num2 = 180L;
		long num3 = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_SHIELD_TIME);
		if (num3 > 0L)
		{
			num2 = num3;
		}
		long num4 = num2 - num;
		if (num4 <= 30L && !this.m_bPlunderProtectEnd_30)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("275");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
			this.m_bPlunderProtectEnd_30 = true;
		}
		if (num4 <= 10L && !this.m_bPlunderProtectEnd_10)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("277");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
			this.m_bPlunderProtectEnd_10 = true;
		}
		if (num4 <= 5L && !this.m_bPlunderProtectEnd_5)
		{
			string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("278");
			Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
			this.m_bPlunderProtectEnd_5 = true;
		}
		if (num4 <= 0L && !this.m_bPlunderProtectEnd)
		{
			string textFromNotify4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("281");
			Main_UI_SystemMessage.ADDMessage(textFromNotify4, SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
			this.m_bPlunderProtectEnd = true;
		}
	}

	public void ShowHideAll(bool bShow, bool bNameCheck)
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null && nrCharBase.m_k3DChar != null)
			{
				nrCharBase.SetShowHide3DModel(bShow, bShow, bNameCheck);
			}
		}
	}

	public void ShowHideAll(bool bShow, bool bNameCheck, bool bParticleSystem)
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				if (nrCharBase.m_k3DChar != null)
				{
					if (nrCharBase.IsReady3DModel())
					{
						nrCharBase.SetShowHide3DModel(bShow, bShow, bNameCheck, bParticleSystem);
					}
				}
			}
		}
	}

	public bool IsCharActive()
	{
		return this.m_bCharActiveTo;
	}

	public void SetChildActive(bool bActive)
	{
		this.m_bCharActiveTo = bActive;
	}

	public void SetChildActiveAll()
	{
		if (this.m_bCharActiveTo)
		{
			if (!NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
			{
				return;
			}
			if (!StageSystem.IsStable)
			{
				return;
			}
		}
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null && nrCharBase.m_k3DChar != null)
			{
				nrCharBase.SetChildActive(this.m_bCharActiveTo);
				if (this.m_bCharActiveTo && nrCharBase.IsChangedItem())
				{
					NrCharUser nrCharUser = nrCharBase as NrCharUser;
					if (nrCharUser != null)
					{
						nrCharUser.ChangeEquipItem();
					}
					nrCharBase.SetChangedItem(false);
				}
			}
		}
		this.m_bCharActiveFrom = this.m_bCharActiveTo;
	}

	public void ChangeChar3DStepToDeleted()
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null && nrCharBase.GetID() > 0)
			{
				nrCharBase.Refresh3DChar();
			}
		}
	}

	public bool Is_Money(long a_lMoney)
	{
		return this.m_kMyCharInfo.m_Money >= a_lMoney;
	}

	public void SetBillboardScale()
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				nrCharBase.SetBillboardScale();
			}
		}
	}

	public void SyncBillboardRotate()
	{
		NrCharBase @char = this.GetChar(1);
		if (@char != null && @char.GetID() == 1)
		{
			@char.SyncBillboardRotate(true);
		}
		bool bScaleUpdate = false;
		long frameRate = NrTSingleton<NkClientLogic>.Instance.GetFrameRate();
		if (frameRate % 5L == 0L)
		{
			bScaleUpdate = true;
		}
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null && nrCharBase.GetID() > 1)
			{
				nrCharBase.SyncBillboardRotate(bScaleUpdate);
			}
		}
	}

	public void ToggleShowCharUnique()
	{
		this.m_bShowCharUnique = !this.m_bShowCharUnique;
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null && nrCharBase.GetID() > 0)
			{
				if (nrCharBase.GetID() != 1)
				{
					nrCharBase.RefreshCharName(this.m_bShowCharUnique);
				}
			}
		}
	}

	public void RefreshCharName()
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null && nrCharBase.GetID() > 0)
			{
				if (nrCharBase.GetID() != 1)
				{
					nrCharBase.RefreshCharName(this.m_bShowCharUnique);
				}
			}
		}
	}

	public void SetCharLODByQualityLevel(TsQualityManager.Level level)
	{
		NrCharDefine.CharLODStep charLODStep = NrCharDefine.CharLODStep.CHARLOD_STEP_1;
		switch (level)
		{
		case TsQualityManager.Level.LOWEST:
		case TsQualityManager.Level.LOW:
			charLODStep = NrCharDefine.CharLODStep.CHARLOD_STEP_3;
			break;
		case TsQualityManager.Level.MEDIUM:
			charLODStep = NrCharDefine.CharLODStep.CHARLOD_STEP_2;
			break;
		case TsQualityManager.Level.HIGH:
		case TsQualityManager.Level.HIGHEST:
			charLODStep = NrCharDefine.CharLODStep.CHARLOD_STEP_1;
			break;
		}
		this.SetCharLODStep(charLODStep);
	}

	public void SetCharLODStep(NrCharDefine.CharLODStep lodstep)
	{
		if (this.m_eCharLODStep == lodstep)
		{
			return;
		}
		this.m_eCharLODStep = lodstep;
	}

	public NrCharDefine.CharLODStep GetCharLODStep()
	{
		return this.m_eCharLODStep;
	}

	public string GetCharLODStepString()
	{
		if (Scene.IsCurScene(Scene.Type.CUTSCENE))
		{
			return string.Empty;
		}
		switch (this.m_eCharLODStep)
		{
		case NrCharDefine.CharLODStep.CHARLOD_STEP_1:
			return "_LOD1";
		case NrCharDefine.CharLODStep.CHARLOD_STEP_2:
			return "_LOD1";
		case NrCharDefine.CharLODStep.CHARLOD_STEP_3:
			return "_LOD1";
		default:
			return "_LOD1";
		}
	}

	public void AddReservedCharMakeInfo(NEW_MAKECHAR_INFO makecharinfo)
	{
		this.DelReservedCharMakeInfo(makecharinfo.CharUnique);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(makecharinfo.CharKind);
		float distance = 0f;
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser != null)
		{
			Vector3 b = new Vector3(makecharinfo.CharPos.x, makecharinfo.CharPos.y, makecharinfo.CharPos.z);
			distance = Vector3.Distance(nrCharUser.GetPersonInfo().GetCharPos(), b);
		}
		NrReservedCharMakeInfo nrReservedCharMakeInfo = new NrReservedCharMakeInfo(distance, makecharinfo);
		if ((makecharinfo.Status & 24L) != 0L)
		{
			nrReservedCharMakeInfo.ReservedMoveTo.x = makecharinfo.Direction.x;
			nrReservedCharMakeInfo.ReservedMoveTo.y = makecharinfo.Direction.y;
			nrReservedCharMakeInfo.ReservedMoveTo.z = makecharinfo.Direction.z;
		}
		if (charKindInfo != null)
		{
			if (charKindInfo.IsATB(8L) || charKindInfo.IsATB(16L))
			{
				this.m_lsFirstGeneration.Add(nrReservedCharMakeInfo);
			}
			else
			{
				this.m_lsSecondGeneration.Add(nrReservedCharMakeInfo);
			}
		}
		this.m_bNeedSort = true;
	}

	public void SortReservedCharMakeInfo()
	{
		this.m_lsFirstGeneration.Sort((NrReservedCharMakeInfo kLeft, NrReservedCharMakeInfo kRight) => kLeft.Distance.CompareTo(kRight.Distance));
		this.m_lsSecondGeneration.Sort((NrReservedCharMakeInfo kLeft, NrReservedCharMakeInfo kRight) => kLeft.Distance.CompareTo(kRight.Distance));
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			this.m_vLastMyCharPos = @char.m_kCharMove.GetCharPos();
		}
	}

	public void DelReservedCharMakeInfo(short charunique)
	{
		this.m_lsFirstGeneration.RemoveAll((NrReservedCharMakeInfo makeinfo) => makeinfo.MakeCharInfo.CharUnique == charunique);
		this.m_lsSecondGeneration.RemoveAll((NrReservedCharMakeInfo makeinfo) => makeinfo.MakeCharInfo.CharUnique == charunique);
	}

	private int GenerateChar(NrReservedCharMakeInfo makeinfo)
	{
		if (makeinfo.MakeCharInfo.CharUnique <= 0)
		{
			Debug.LogError("couldn't generate character.");
			return -1;
		}
		int num = this.SetChar(makeinfo.MakeCharInfo, false, false);
		if (num > 0)
		{
			NrCharBase @char = this.GetChar(num);
			if (@char.IsCharKindATB(2L))
			{
				NrCharUser nrCharUser = @char as NrCharUser;
				if (nrCharUser != null && makeinfo.kShapeInfo.kPartInfo != null)
				{
					nrCharUser.ChangeCharModel(makeinfo.kShapeInfo.nFaceCharKind, makeinfo.kShapeInfo.nFaceGrade, makeinfo.kShapeInfo.nFaceCharSolID, makeinfo.kShapeInfo.nFaceCostumeUnique);
					nrCharUser.ChangeCharPartInfo(makeinfo.kShapeInfo.kPartInfo, true, true);
				}
			}
			if (num > 1)
			{
				this.m_nGenerationCharCount++;
				@char.m_bIsManagedGeneration = true;
			}
			if (makeinfo.ReservedMoveTo.x != 0f && makeinfo.ReservedMoveTo.z != 0f && @char != null)
			{
				@char.MoveTo(makeinfo.ReservedMoveTo.x, makeinfo.ReservedMoveTo.y, makeinfo.ReservedMoveTo.z, false);
			}
		}
		return num;
	}

	public void DecreaseGenerationCount()
	{
		this.m_nGenerationCharCount = Math.Max(0, this.m_nGenerationCharCount - 1);
	}

	public void ProcessGenerateChars()
	{
		if (this.m_bNeedSort)
		{
			this.SortReservedCharMakeInfo();
			this.m_bNeedSort = false;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsRemainFrame(2))
		{
			return;
		}
		if (this.m_nGenerationCharCount < 1)
		{
			int num = 0;
			for (int i = 0; i < this.m_lsFirstGeneration.Count; i++)
			{
				if (this.m_nGenerationCharCount >= 1)
				{
					break;
				}
				NrReservedCharMakeInfo makeinfo = this.m_lsFirstGeneration[i];
				this.GenerateChar(makeinfo);
				num++;
			}
			this.m_lsFirstGeneration.RemoveRange(0, num);
			num = 0;
			for (int j = 0; j < this.m_lsSecondGeneration.Count; j++)
			{
				if (this.m_nGenerationCharCount >= 1)
				{
					break;
				}
				NrReservedCharMakeInfo makeinfo = this.m_lsSecondGeneration[j];
				this.GenerateChar(makeinfo);
				num++;
			}
			this.m_lsSecondGeneration.RemoveRange(0, num);
		}
	}

	public NrReservedCharMakeInfo FindReservedChar(short charUnique)
	{
		for (int i = 0; i < this.m_lsFirstGeneration.Count; i++)
		{
			NrReservedCharMakeInfo nrReservedCharMakeInfo = this.m_lsFirstGeneration[i];
			if (nrReservedCharMakeInfo != null && nrReservedCharMakeInfo.MakeCharInfo.CharUnique == charUnique)
			{
				return nrReservedCharMakeInfo;
			}
		}
		for (int j = 0; j < this.m_lsSecondGeneration.Count; j++)
		{
			NrReservedCharMakeInfo nrReservedCharMakeInfo2 = this.m_lsSecondGeneration[j];
			if (nrReservedCharMakeInfo2 != null && nrReservedCharMakeInfo2.MakeCharInfo.CharUnique == charUnique)
			{
				return nrReservedCharMakeInfo2;
			}
		}
		return null;
	}

	public void SetReservedCharPartInfo(short charUnique, int facecharkind, byte facegrade, NrCharPartInfo pkPartInfo)
	{
		NrReservedCharMakeInfo nrReservedCharMakeInfo = this.FindReservedChar(charUnique);
		if (nrReservedCharMakeInfo == null)
		{
			return;
		}
		nrReservedCharMakeInfo.kShapeInfo.nFaceCharKind = facecharkind;
		nrReservedCharMakeInfo.kShapeInfo.nFaceGrade = facegrade;
		nrReservedCharMakeInfo.kShapeInfo.kPartInfo = pkPartInfo;
	}

	public void DrawMyCharInfoGUI()
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
		{
			return;
		}
		NrCharUser nrCharUser = this.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return;
		}
		Color color = GUI.color;
		GUI.color = new Color(255f, 255f, 0f);
		float width = 200f;
		Vector2 vector = default(Vector2);
		vector.x = 20f;
		GUI.Label(new Rect(vector.x, vector.y += 20f, width, 20f), "Char Name:[" + nrCharUser.GetCharName() + "]");
		GUI.Label(new Rect(vector.x, vector.y += 20f, width, 20f), "Update Step:[" + nrCharUser.Get3DCharStep().ToString() + "]");
		GUI.Label(new Rect(vector.x, vector.y += 20f, width, 20f), "Animation:[" + nrCharUser.GetAnimation().ToString() + "]");
		if (nrCharUser.IsReady3DModel())
		{
			GUI.Label(new Rect(vector.x, vector.y += 20f, width, 20f), "Char Speed:[" + nrCharUser.GetSpeed().ToString() + "]");
		}
		GUI.color = color;
	}

	public bool IsValidCharByCharUnique(short nCharUnique)
	{
		int num = this.FindCharIDByCharUnique(nCharUnique);
		return 0 <= num;
	}

	public bool IsValidChar(int nCharID)
	{
		return nCharID >= 0 && nCharID < 300 && this.m_arChar[nCharID] != null;
	}

	public float GetDistanceCharPos(int fromid, int toid)
	{
		float result = 0f;
		NrCharBase @char = this.GetChar(fromid);
		if (@char == null)
		{
			return result;
		}
		NrCharBase char2 = this.GetChar(toid);
		if (char2 == null)
		{
			return result;
		}
		return Vector3.Distance(@char.GetPersonInfo().GetCharPos(), char2.GetPersonInfo().GetCharPos());
	}

	public void SetWorldLoadSafeTime()
	{
		this.m_fWorldLoadSafeTime = Time.time;
	}

	public bool IsSafeToWorld(bool bCheckSecondGeneralCount)
	{
		if (this.m_fWorldLoadSafeTime <= 0f)
		{
			this.SetWorldLoadSafeTime();
		}
		else
		{
			if (this.m_lsFirstGeneration.Count > 0)
			{
				return false;
			}
			if (bCheckSecondGeneralCount && this.m_lsSecondGeneration.Count > 0)
			{
				return false;
			}
		}
		NrCharBase @char = this.GetChar(1);
		return @char != null && @char.IsShaderRecovery() && Time.time - this.m_fWorldLoadSafeTime > 1f;
	}

	public string GetCharName()
	{
		NrCharBase @char = this.GetChar(1);
		return @char.GetCharName();
	}

	public void SetIndunATBReservation(short nCharUnique, int nATB)
	{
		if (this.m_dicIndunCharATB.ContainsKey(nCharUnique))
		{
			this.m_dicIndunCharATB[nCharUnique] = nATB;
		}
		else
		{
			this.m_dicIndunCharATB.Add(nCharUnique, nATB);
		}
	}

	public int GetReservationIndunCharATB(short nCharUnique)
	{
		if (this.m_dicIndunCharATB.ContainsKey(nCharUnique))
		{
			return this.m_dicIndunCharATB[nCharUnique];
		}
		return 0;
	}

	public void RemoveReservationIndunCharATB(short nCharUnique)
	{
		if (this.m_dicIndunCharATB.ContainsKey(nCharUnique))
		{
			this.m_dicIndunCharATB.Remove(nCharUnique);
		}
	}

	public void DeleteQuestMonsterEffect()
	{
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				if ((nrCharBase.GetCharKindType() == eCharKindType.CKT_MONSTER || nrCharBase.GetCharKindType() == eCharKindType.CKT_OBJECT) && nrCharBase.IsSetNoticeQuestEffect() && !NrTSingleton<NkQuestManager>.Instance.IsQuestMonster(nrCharBase.GetCharKindInfo().GetCharKind()))
				{
					NrTSingleton<NkEffectManager>.Instance.DeleteEffect(nrCharBase.GetNoticeQuestEffectNum());
					nrCharBase.InitNoticeQuestEffectNum();
				}
			}
		}
	}

	public void SetFakeShadowEnable(bool bEnable)
	{
		if (this.m_bFakeShadowEnable == bEnable)
		{
			return;
		}
		NrCharBase[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NrCharBase nrCharBase = arChar[i];
			if (nrCharBase != null)
			{
				nrCharBase.SetFakeShadowEnable(bEnable);
			}
		}
		this.m_bFakeShadowEnable = bEnable;
	}

	public NkSoldierInfo GetMyHelpSol(long FriendPersonID)
	{
		NrPersonInfoUser charPersonInfo = this.GetCharPersonInfo(1);
		NkReadySolList readySolList = this.m_kMyCharInfo.GetReadySolList();
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkSoldierInfo nkSoldierInfo = readySolList.IsHelpSol(FriendPersonID);
		if (nkSoldierInfo != null)
		{
			return nkSoldierInfo;
		}
		nkSoldierInfo = soldierList.IsHelpSol(FriendPersonID);
		if (nkSoldierInfo != null)
		{
			return nkSoldierInfo;
		}
		return nkSoldierInfo;
	}

	public int AddExpHelpsolCount()
	{
		int num = 0;
		NrPersonInfoUser charPersonInfo = this.GetCharPersonInfo(1);
		NkReadySolList readySolList = this.m_kMyCharInfo.GetReadySolList();
		num += readySolList.AddExpHelpsolCount();
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		return num + soldierList.AddExpHelpsolCount();
	}

	public string GetGuildPortraitURL(long i64Guild)
	{
		if (this.m_kMyCharInfo == null)
		{
			return string.Format(string.Empty, new object[0]);
		}
		return string.Format("http://{0}/Guild/{1}/{2}/{3}.jpg", new object[]
		{
			NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceAreaInfo().szImageURL,
			this.m_kMyCharInfo.m_szWorldType.ToUpper(),
			(i64Guild / 10000L).ToString(),
			(i64Guild % 10000L).ToString()
		});
	}

	public string GetUserPortraitURL(long i64PersonID)
	{
		if (this.m_kMyCharInfo == null)
		{
			return string.Format(string.Empty, new object[0]);
		}
		return string.Format("http://{0}/user/{1}/{2}/{3}.jpg", new object[]
		{
			NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceAreaInfo().szImageURL,
			this.m_kMyCharInfo.m_szWorldType.ToUpper(),
			(i64PersonID / 10000L).ToString(),
			(i64PersonID % 10000L).ToString()
		});
	}

	public bool IsMySameCharUnique(short iCharUnique)
	{
		NrCharBase @char = this.GetChar(1);
		return @char != null && @char.GetCharUnique() == iCharUnique;
	}

	private bool InjuryCureFirstSol()
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return false;
		}
		NkSoldierInfo nkSoldierInfo = null;
		for (int i = 0; i < 6; i++)
		{
			nkSoldierInfo = nrCharUser.GetPersonInfo().GetSoldierInfo(i);
			if (nkSoldierInfo == null || !nkSoldierInfo.IsValid())
			{
				nkSoldierInfo = null;
			}
			else if (!nkSoldierInfo.IsInjuryStatus())
			{
				if (this.m_InjuryCureSolID == nkSoldierInfo.GetSolID())
				{
					this.m_InjuryCureSolID = 0L;
				}
				nkSoldierInfo = null;
			}
			else
			{
				if (this.m_InjuryCureSolID == nkSoldierInfo.GetSolID())
				{
					return true;
				}
				break;
			}
		}
		if (nkSoldierInfo == null)
		{
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null)
			{
				return false;
			}
			Dictionary<long, NkSoldierInfo> list = readySolList.GetList();
			foreach (KeyValuePair<long, NkSoldierInfo> current in list)
			{
				nkSoldierInfo = current.Value;
				if (nkSoldierInfo == null || !nkSoldierInfo.IsValid())
				{
					nkSoldierInfo = null;
				}
				else if (!nkSoldierInfo.IsInjuryStatus())
				{
					if (this.m_InjuryCureSolID == nkSoldierInfo.GetSolID())
					{
						this.m_InjuryCureSolID = 0L;
					}
					nkSoldierInfo = null;
				}
				else
				{
					if (this.m_InjuryCureSolID == nkSoldierInfo.GetSolID())
					{
						return true;
					}
					break;
				}
			}
		}
		if (nkSoldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("739"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			this.m_InjuryCureSolID = 0L;
			return false;
		}
		if (nkSoldierInfo.InjuryCureByItem())
		{
			this.m_InjuryCureSolID = nkSoldierInfo.GetSolID();
			return true;
		}
		if (nkSoldierInfo.InjuryCureByMoney())
		{
			this.m_InjuryCureSolID = nkSoldierInfo.GetSolID();
			return true;
		}
		Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		return false;
	}

	public void ResetInjuryCureSolID()
	{
		this.m_InjuryCureSolID = 0L;
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
}
