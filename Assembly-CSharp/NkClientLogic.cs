using GameMessage.Private;
using Ndoors.Framework.Stage;
using NPatch;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using PROTOCOL.WORLD;
using SERVICE;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NkClientLogic : NrTSingleton<NkClientLogic>
{
	public static bool bWorldCharUpdate;

	private long nFrameRate;

	private float ServerTickCount;

	private float SendHeartBeatTime;

	private bool bGameWorld;

	private bool bLoginGameServer;

	private int nFocusCharID;

	private NrCharBase PickChar;

	private NkBattleChar PickBattleChar;

	private int LastPickCharID;

	private float PickCharTime;

	private Vector3 PickCharMousePos = Vector3.zero;

	private bool bPickingEnable;

	private bool bNPCTalkState;

	private int nNPCTalkCharUnique;

	private int nWarpGateIndex;

	private bool bReadyResponse = true;

	private bool bReadyLoadingCapture;

	private int nWorldCharSkipFrame;

	private Dictionary<string, string> kEditorShaderConvert = new Dictionary<string, string>();

	private eAuthPlatformType m_nAuthPlatformType;

	private bool m_bWarp;

	private int m_nGateIndex = -1;

	private int m_nMapIndex = -1;

	private uint m_nEffectNum;

	private string[] szOTPAuthKey = new string[8];

	public bool showDown = true;

	private bool m_bGuestLogin;

	public eAuthPlatformType AuthPlatformType
	{
		get
		{
			return this.m_nAuthPlatformType;
		}
		set
		{
			this.m_nAuthPlatformType = value;
		}
	}

	private NkClientLogic()
	{
		this.Init();
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			this.kEditorShaderConvert.Add("AT2/Effect/Standard_mobile", "AT2/Effect/Standard_mobile");
			this.kEditorShaderConvert.Add("AT2/Effect/Add_mobile", "AT2/Effect/Add_mobile");
			this.kEditorShaderConvert.Add("AT2/Effect/FlowAdd_mobile", "AT2/Effect/FlowAdd_mobile");
			this.kEditorShaderConvert.Add("AT2/Effect/FlowStandard_mobile", "AT2/Effect/FlowStandard_mobile");
			this.kEditorShaderConvert.Add("AT2/Effect/Headup_mobile", "AT2/Effect/Headup_mobile");
			this.kEditorShaderConvert.Add("T4MShaders/ShaderModel2/Unlit/T4M 4 Textures Unlit LM", "T4MShaders/ShaderModel2/Unlit/T4M 4 Textures Unlit LM");
			this.kEditorShaderConvert.Add("AT2/Effect/FlowMulti_mobile", "AT2/Effect/FlowMulti_mobile");
			this.kEditorShaderConvert.Add("AT2/Effect/Multi_mobile", "AT2/Effect/Multi_mobile");
			this.kEditorShaderConvert.Add("AT2/AT2_Selected_Object_mobile", "AT2/AT2_Selected_Object_mobile");
		}
	}

	public void Init()
	{
		NkClientLogic.bWorldCharUpdate = false;
		this.nFrameRate = 1L;
		this.ServerTickCount = 0f;
		this.SendHeartBeatTime = 0f;
		this.bGameWorld = false;
		this.nFocusCharID = 0;
		this.PickChar = null;
		this.PickBattleChar = null;
		this.LastPickCharID = 0;
		this.PickCharTime = 0f;
		this.PickCharMousePos = Vector3.zero;
		this.bPickingEnable = true;
		this.bNPCTalkState = false;
		this.nNPCTalkCharUnique = 0;
		this.nWarpGateIndex = 0;
		this.bReadyResponse = true;
		this.bReadyLoadingCapture = false;
		this.nWorldCharSkipFrame = 0;
		this.m_bWarp = false;
		this.m_nGateIndex = -1;
		this.m_nMapIndex = -1;
		this.m_nEffectNum = 0u;
		for (int i = 0; i < 8; i++)
		{
			this.szOTPAuthKey[i] = string.Empty;
		}
	}

	public bool IsWarp()
	{
		return this.m_bWarp;
	}

	public void SetWarp(bool warp)
	{
		if (this.showDown && !NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(this.m_nGateIndex, this.m_nMapIndex))
		{
			return;
		}
		this.m_bWarp = warp;
	}

	public void SetWarp(bool warp, int gateIndex, int mapIndex)
	{
		if (!this.ShowDownLoadUI(gateIndex, mapIndex))
		{
			return;
		}
		this.m_bWarp = warp;
		this.m_nGateIndex = gateIndex;
		this.m_nMapIndex = mapIndex;
		this.ShowWarpEffect();
	}

	public bool ShowDownLoadUI(int gateIndex = 0, int mapIndex = 0)
	{
		if (gateIndex == 2 || gateIndex == 3 || gateIndex == 123 || gateIndex == 5 || gateIndex == 6)
		{
			return true;
		}
		if (mapIndex == 2 || mapIndex == 60 || mapIndex == 4 || mapIndex == 61)
		{
			return true;
		}
		if (Launcher.Instance.LocalPatchLevel != Launcher.Instance.PatchLevelMax)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("761"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		return true;
	}

	public void OnOKDownStart(object a_oObject)
	{
		Launcher.Instance.SavePatchLevel(Launcher.Instance.PatchLevelMax);
		NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
	}

	public void Warp(uint effectNum)
	{
		if (this.m_nEffectNum != effectNum)
		{
			return;
		}
		if (-1 < this.m_nGateIndex && -1 < this.m_nMapIndex)
		{
			NrTSingleton<NkClientLogic>.Instance.CharWarpRequest(0);
			GS_WARP_REQ gS_WARP_REQ = new GS_WARP_REQ();
			gS_WARP_REQ.nGateIndex = this.m_nGateIndex;
			gS_WARP_REQ.nWorldMapWarp_MapIDX = this.m_nMapIndex;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WARP_REQ, gS_WARP_REQ);
			this.m_bWarp = false;
			this.m_nGateIndex = -1;
			this.m_nMapIndex = -1;
			this.m_nEffectNum = 0u;
		}
	}

	public void ShowWarpEffect()
	{
		if (this.m_bWarp)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser != null)
			{
				this.m_nEffectNum = NrTSingleton<NkEffectManager>.Instance.AddEffect("WARP", nrCharUser);
			}
			this.m_bWarp = false;
		}
	}

	public void Update()
	{
		this.IncreaseFrameRate();
		this.SendHeartBeat();
		this.CheckWorldCharUpdateTime();
	}

	public void IncreaseFrameRate()
	{
		this.nFrameRate += 1L;
	}

	public long GetFrameRate()
	{
		return this.nFrameRate;
	}

	public bool IsRemainFrame(int remainder)
	{
		return this.nFrameRate % (long)remainder == 0L;
	}

	public void SetServerTickCount(ulong servertick)
	{
		this.ServerTickCount = servertick / 1000f;
	}

	public float GetServerTickCount()
	{
		return this.ServerTickCount;
	}

	private void SendHeartBeat()
	{
		if (NrTSingleton<CMovingServer>.Instance.IsMovingWorld())
		{
			return;
		}
		if (!NrTSingleton<NkCharManager>.Instance.CharacterListSetComplete)
		{
			return;
		}
		this.ServerTickCount += Time.deltaTime;
		float num = Time.time - this.SendHeartBeatTime;
		float num2 = 60f;
		if (num < num2)
		{
			return;
		}
		WS_USER_HEARTBEAT_REQ wS_USER_HEARTBEAT_REQ = new WS_USER_HEARTBEAT_REQ();
		wS_USER_HEARTBEAT_REQ.TickCount = (uint)(this.ServerTickCount * 1000f);
		SendPacket.GetInstance().SendObject(16777226, wS_USER_HEARTBEAT_REQ);
		this.SendHeartBeatTime = Time.time;
	}

	public void SetFocusCharID(int charid, bool bFocus)
	{
		if (bFocus)
		{
			this.nFocusCharID = charid;
		}
		else if (this.nFocusCharID == charid)
		{
			this.nFocusCharID = 0;
		}
	}

	public NrCharBase GetFocusChar()
	{
		if (this.nFocusCharID == 0)
		{
			return null;
		}
		return NrTSingleton<NkCharManager>.Instance.GetChar(this.nFocusCharID);
	}

	public void InitPickChar()
	{
		if (this.PickChar != null)
		{
			this.PickChar.CancelClickMe();
			this.PickChar = null;
		}
		if (this.PickBattleChar != null)
		{
			this.PickBattleChar.CancelClickMe();
			this.PickBattleChar = null;
		}
		this.PickCharTime = 0f;
		this.PickCharMousePos = Vector3.zero;
	}

	public void SetPickChar(NrCharBase pkChar)
	{
		if (pkChar != null)
		{
			this.PickChar = pkChar;
			this.LastPickCharID = pkChar.GetID();
			this.PickCharTime = Time.time;
			this.PickCharMousePos = NkInputManager.mousePosition;
			if (this.PickChar.IsCharKindATB(8L))
			{
				this.PickChar.SetClickMe();
			}
			else if (this.PickChar.IsCharKindATB(4L))
			{
				this.PickChar.SetClickMe();
			}
			else if (this.PickChar.IsCharKindATB(16L))
			{
				this.PickChar.SetClickMe();
			}
		}
		else
		{
			this.InitPickChar();
		}
	}

	public void SetPickChar(NkBattleChar pkChar)
	{
		if (pkChar != null)
		{
			this.PickBattleChar = pkChar;
			this.LastPickCharID = pkChar.GetID();
			this.PickCharTime = Time.time;
			this.PickCharMousePos = NkInputManager.mousePosition;
			if (this.PickBattleChar.IsCharKindATB(8L))
			{
				this.PickBattleChar.SetClickMe();
			}
			else if (this.PickBattleChar.IsCharKindATB(4L))
			{
				this.PickBattleChar.SetClickMe();
			}
		}
		else
		{
			this.InitPickChar();
		}
	}

	public NrCharBase GetPickChar()
	{
		if (this.PickCharTime == 0f || Time.time - this.PickCharTime > 1f || Vector3.Distance(NkInputManager.mousePosition, this.PickCharMousePos) > 10f)
		{
			this.InitPickChar();
		}
		return this.PickChar;
	}

	public int GetLastPickCharID()
	{
		return this.LastPickCharID;
	}

	public void SetPickingEnable(bool bPickingUse)
	{
		this.bPickingEnable = bPickingUse;
	}

	public bool IsPickingEnable()
	{
		return this.bPickingEnable;
	}

	public void SetNPCTalkState(bool talkstate)
	{
		this.bNPCTalkState = talkstate;
		if (talkstate)
		{
			if (this.IsWorldScene())
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null)
				{
					@char.m_kCharMove.MoveStop(true, false);
				}
			}
		}
	}

	public bool IsNPCTalkState()
	{
		return this.bNPCTalkState;
	}

	public void SetGameWorld(bool gameworld)
	{
		if (NrTSingleton<CMovingServer>.Instance.ReqMovingCharInit)
		{
			NrTSingleton<NkCharManager>.Instance.Init(false);
			NrTSingleton<CMovingServer>.Instance.ReqMovingCharInit = false;
		}
		else
		{
			NrTSingleton<NkCharManager>.Instance.Init(true);
		}
		this.bGameWorld = gameworld;
	}

	public bool IsGameWorld()
	{
		return this.bGameWorld;
	}

	public void SetLoginGameServer(bool loginserver)
	{
		this.bLoginGameServer = loginserver;
	}

	public bool IsLoginGameServer()
	{
		return this.bLoginGameServer;
	}

	public bool IsWorldScene()
	{
		return Scene.CurScene == Scene.Type.WORLD;
	}

	public bool IsBattleScene()
	{
		return Scene.CurScene == Scene.Type.BATTLE;
	}

	public void SetClearMiddleStage()
	{
		switch (Scene.CurScene)
		{
		case Scene.Type.BATTLE:
		case Scene.Type.CUTSCENE:
		case Scene.Type.SOLDIER_BATCH:
			FacadeHandler.PopStage();
			break;
		}
	}

	public bool IsEffectEnable()
	{
		if (!this.IsWorldScene())
		{
			return false;
		}
		if (!StageSystem.IsStable)
		{
			return false;
		}
		if (this.IsNPCTalkState())
		{
			return false;
		}
		Battle_ResultDlg battle_ResultDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_DLG) as Battle_ResultDlg;
		if (battle_ResultDlg != null && battle_ResultDlg.Visible)
		{
			return false;
		}
		Battle_ResultPlunderDlg battle_ResultPlunderDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_PLUNDER_DLG) as Battle_ResultPlunderDlg;
		if (battle_ResultPlunderDlg != null && battle_ResultPlunderDlg.Visible)
		{
			return false;
		}
		BabelTowerMainDlg babelTowerMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERMAIN_DLG) as BabelTowerMainDlg;
		if (babelTowerMainDlg != null && babelTowerMainDlg.Visible)
		{
			return false;
		}
		BabelTowerSubDlg babelTowerSubDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERSUB_DLG) as BabelTowerSubDlg;
		return babelTowerSubDlg == null || !babelTowerSubDlg.Visible;
	}

	public int CharColliderLayerMask()
	{
		TsLayerMask layerMask = TsLayer.EVERYTHING - TsLayer.DEFAULT - TsLayer.PC - TsLayer.PC_DECORATION - TsLayer.PC_OTHER - TsLayer.NPC - TsLayer.FADE_OBJECT - TsLayer.IGNORE_RAYCAST - TsLayer.IGNORE_PICK;
		return layerMask;
	}

	public int CharClickLayerMask()
	{
		TsLayerMask layerMask = TsLayer.EVERYTHING - TsLayer.DEFAULT - TsLayer.PC - TsLayer.PC_DECORATION - TsLayer.PC_OTHER - TsLayer.NPC - TsLayer.TERRAIN - TsLayer.IGNORE_RAYCAST;
		return layerMask;
	}

	public void SetReadyResponse(bool bReady)
	{
		this.bReadyResponse = bReady;
	}

	public bool GetReadyResponse()
	{
		return this.bReadyResponse;
	}

	public void SetReadyLoadingCapture(bool bReady)
	{
		this.bReadyLoadingCapture = bReady;
	}

	public bool GetReadyLoadingCapture()
	{
		return this.bReadyLoadingCapture;
	}

	public void BackMainCameraInfo()
	{
		if (Camera.main == null)
		{
			return;
		}
		maxCamera component = Camera.main.GetComponent<maxCamera>();
		if (component != null)
		{
			component.BackUpCameraInfo();
		}
	}

	public void SetNpcTalkCharUnique(int unique)
	{
		this.nNPCTalkCharUnique = unique;
	}

	public int GetNpcTalkCharUnique()
	{
		return this.nNPCTalkCharUnique;
	}

	public void CharWarpRequest(int gateindex)
	{
		NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bNoMove = true;
		if (gateindex > 0)
		{
			this.SetWarpGateIndex(gateindex);
		}
		this.BackMainCameraInfo();
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			@char.m_kCharMove.MoveStop(false, false);
		}
		NrTSingleton<UIManager>.Instance.ClearInputQueue();
	}

	public void SetWarpGateIndex(int gateindex)
	{
		this.nWarpGateIndex = gateindex;
	}

	public int GetWarpGateIndex()
	{
		return this.nWarpGateIndex;
	}

	public bool IsMovable()
	{
		if (!this.IsWorldScene() || !StageSystem.IsStable)
		{
			return false;
		}
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bNoMove)
		{
			return false;
		}
		if (this.IsNPCTalkState())
		{
			return false;
		}
		if (!NrTSingleton<NkCharManager>.Instance.InputControl)
		{
			return false;
		}
		if (this.GetWarpGateIndex() > 0)
		{
			return false;
		}
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		return @char == null || !@char.IsCharStateAtb(7392L);
	}

	public void SetWorldCharSkipFrame(int skipframe)
	{
		this.nWorldCharSkipFrame = skipframe;
	}

	public void CheckWorldCharUpdateTime()
	{
		if (!this.IsWorldScene() && this.nWorldCharSkipFrame > 0 && !this.IsRemainFrame(this.nWorldCharSkipFrame))
		{
			NkClientLogic.bWorldCharUpdate = false;
		}
		NkClientLogic.bWorldCharUpdate = true;
	}

	public bool IsCharEventEnable()
	{
		return !this.IsNPCTalkState() && (!this.IsWorldScene() || !NkInputManager.IsJoystick());
	}

	public void SetGuestLogin(bool flag)
	{
		this.m_bGuestLogin = flag;
		if (flag)
		{
			PlayerPrefs.SetInt(NrPrefsKey.GUESTID, 1);
		}
		else
		{
			PlayerPrefs.SetInt(NrPrefsKey.GUESTID, 0);
		}
	}

	public bool IsGuestLogin()
	{
		if (TsPlatform.IsAndroid)
		{
			return false;
		}
		if (PlayerPrefs.HasKey(NrPrefsKey.GUESTID))
		{
			if (PlayerPrefs.GetInt(NrPrefsKey.GUESTID) == 1)
			{
				this.m_bGuestLogin = true;
			}
			else
			{
				this.m_bGuestLogin = false;
			}
		}
		return this.m_bGuestLogin;
	}

	public void ShowChangeGuestIDUI()
	{
		if (TsPlatform.IsAndroid)
		{
			return;
		}
		if (!PlayerPrefs.HasKey(NrPrefsKey.GUESTID))
		{
			return;
		}
		if (PlayerPrefs.GetInt(NrPrefsKey.GUESTID) == 0)
		{
			return;
		}
		if (this.m_bGuestLogin)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			if (kMyCharInfo.GetLevel() >= 10)
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GUESTID_COMBINE_DLG);
			}
		}
	}

	public int GetAuthPlatformType()
	{
		if (this.m_nAuthPlatformType != eAuthPlatformType.AUTH_PLATFORMTYPE_NONE)
		{
			return (int)this.m_nAuthPlatformType;
		}
		int @int = PlayerPrefs.GetInt(NrPrefsKey.LAST_AUTH_PLATFORM);
		if (@int > 0)
		{
			return @int;
		}
		eAuthPlatformType result = eAuthPlatformType.AUTH_PLATFORMTYPE_NONE;
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		if (currentServiceArea > eSERVICE_AREA.SERVICEAREA_NDOORS_START && currentServiceArea < eSERVICE_AREA.SERVICEAREA_NDOORS_END)
		{
			eSERVICE_AREA eSERVICE_AREA = currentServiceArea;
			switch (eSERVICE_AREA)
			{
			case eSERVICE_AREA.SERVICE_ANDROID_CNTEST:
				result = eAuthPlatformType.AUTH_PLATFORMTYPE_CHUKONG;
				return (int)result;
			case eSERVICE_AREA.SERVICE_ANDROID_CNREVIEW:
			case eSERVICE_AREA.SERVICE_IOS_KORLOCAL:
			case eSERVICE_AREA.SERVICE_IOS_KORQA:
			case eSERVICE_AREA.SERVICE_IOS_KORAPPSTORE:
			case eSERVICE_AREA.SERVICE_IOS_USQA:
			case eSERVICE_AREA.SERVICE_IOS_USIOS:
			case eSERVICE_AREA.SERVICE_IOS_CNQA:
			case eSERVICE_AREA.SERVICE_IOS_CNTEST:
				IL_91:
				switch (eSERVICE_AREA)
				{
				case eSERVICE_AREA.SERVICE_ANDROID_BANDNAVER:
				case eSERVICE_AREA.SERVICE_ANDROID_BANDGOOGLE:
					result = eAuthPlatformType.AUTH_PLATFORMTYPE_BAND;
					return (int)result;
				case eSERVICE_AREA.SERVICE_ANDROID_KORKAKAO:
				case eSERVICE_AREA.SERVICE_ANDROID_KAKAOTSTORE:
					goto IL_B5;
				default:
					result = eAuthPlatformType.AUTH_PLATFORMTYPE_NDOORS;
					return (int)result;
				}
				break;
			case eSERVICE_AREA.SERVICE_ANDROID_JPLOCAL:
			case eSERVICE_AREA.SERVICE_ANDROID_JPQA:
			case eSERVICE_AREA.SERVICE_ANDROID_JPQALINE:
			case eSERVICE_AREA.SERVICE_ANDROID_JPLINE:
			case eSERVICE_AREA.SERVICE_IOS_JPQA:
			case eSERVICE_AREA.SERVICE_IOS_JPQALINE:
			case eSERVICE_AREA.SERVICE_IOS_JPLINE:
				result = eAuthPlatformType.AUTH_PLATFORMTYPE_LINE;
				return (int)result;
			case eSERVICE_AREA.SERVICE_IOS_KORKAKAO:
				goto IL_B5;
			}
			goto IL_91;
			IL_B5:
			result = eAuthPlatformType.AUTH_PLATFORMTYPE_KAKAO;
		}
		return (int)result;
	}

	public int GetPlayerPlatformType()
	{
		if (TsPlatform.IsAndroid)
		{
			return 2;
		}
		if (TsPlatform.IsIPhone)
		{
			return 3;
		}
		return 1;
	}

	public int GetStoreType()
	{
		if (TsPlatform.IsAndroid)
		{
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			eSERVICE_AREA eSERVICE_AREA = currentServiceArea;
			switch (eSERVICE_AREA)
			{
			case eSERVICE_AREA.SERVICE_ANDROID_KORQA:
			case eSERVICE_AREA.SERVICE_ANDROID_KORGOOGLE:
			case eSERVICE_AREA.SERVICE_ANDROID_BANDGOOGLE:
			case eSERVICE_AREA.SERVICE_ANDROID_KORKAKAO:
				return 1;
			case eSERVICE_AREA.SERVICE_ANDROID_KORTSTORE:
			case eSERVICE_AREA.SERVICE_ANDROID_KAKAOTSTORE:
				return 2;
			case eSERVICE_AREA.SERVICE_ANDROID_KORNAVER:
			case eSERVICE_AREA.SERVICE_ANDROID_BANDNAVER:
				return 3;
			default:
				if (eSERVICE_AREA != eSERVICE_AREA.SERVICE_ANDROID_CNTEST)
				{
					return 1;
				}
				return 10;
			}
		}
		else
		{
			if (TsPlatform.IsIPhone)
			{
				return 4;
			}
			return 0;
		}
	}

	public string GetEditorShaderConvert(string source)
	{
		string result = string.Empty;
		if (this.kEditorShaderConvert.ContainsKey(source))
		{
			result = this.kEditorShaderConvert[source];
		}
		return result;
	}

	public void SetEditorShaderConvert(GameObject pkTarget)
	{
		if (pkTarget == null)
		{
			return;
		}
		Renderer[] components = pkTarget.GetComponents<Renderer>();
		Renderer[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			if (renderer.material != null)
			{
				string editorShaderConvert = this.GetEditorShaderConvert(renderer.material.shader.name);
				if (editorShaderConvert.Length > 0)
				{
					renderer.material.shader = Shader.Find(editorShaderConvert);
				}
			}
		}
		Renderer[] componentsInChildren = pkTarget.GetComponentsInChildren<Renderer>(true);
		Renderer[] array2 = componentsInChildren;
		for (int j = 0; j < array2.Length; j++)
		{
			Renderer renderer2 = array2[j];
			if (renderer2.material != null)
			{
				string editorShaderConvert2 = this.GetEditorShaderConvert(renderer2.material.shader.name);
				if (editorShaderConvert2.Length > 0)
				{
					renderer2.material.shader = Shader.Find(editorShaderConvert2);
				}
			}
		}
	}

	public void SetEditorShaderConvert(ref GameObject pkTarget)
	{
		if (pkTarget == null)
		{
			return;
		}
		Renderer[] components = pkTarget.GetComponents<Renderer>();
		Renderer[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			if (renderer.material != null)
			{
				string editorShaderConvert = this.GetEditorShaderConvert(renderer.material.shader.name);
				if (editorShaderConvert.Length > 0)
				{
					renderer.material.shader = Shader.Find(editorShaderConvert);
				}
			}
		}
		Renderer[] componentsInChildren = pkTarget.GetComponentsInChildren<Renderer>(true);
		Renderer[] array2 = componentsInChildren;
		for (int j = 0; j < array2.Length; j++)
		{
			Renderer renderer2 = array2[j];
			if (renderer2.material != null)
			{
				string editorShaderConvert2 = this.GetEditorShaderConvert(renderer2.material.shader.name);
				if (editorShaderConvert2.Length > 0)
				{
					renderer2.material.shader = Shader.Find(editorShaderConvert2);
				}
			}
		}
	}

	public void SetOTPAuthKey(byte otptype, string authkey)
	{
		this.szOTPAuthKey[(int)otptype] = authkey;
		if (this.szOTPAuthKey[(int)otptype].Length > 0)
		{
			this.ProcessOTPReady((eOTPRequestType)otptype);
		}
	}

	public string GetOTPAuthKey(eOTPRequestType eReqType)
	{
		return this.szOTPAuthKey[(int)eReqType];
	}

	public void RequestOTPAuthKey(eOTPRequestType eReqType)
	{
		WS_OTPAUTH_REQ wS_OTPAUTH_REQ = new WS_OTPAUTH_REQ();
		wS_OTPAUTH_REQ.nOTPRequestType = (byte)eReqType;
		SendPacket.GetInstance().SendObject(16777276, wS_OTPAUTH_REQ);
	}

	public void SetOTPRequestInfo(eOTPRequestType eReqType)
	{
		if (this.szOTPAuthKey[(int)eReqType].Length <= 0)
		{
			this.RequestOTPAuthKey(eReqType);
		}
		else
		{
			this.ProcessOTPReady(eReqType);
		}
	}

	private string GetProcessLeaderHero(int i32Kind)
	{
		string result = string.Empty;
		switch (i32Kind)
		{
		case 1:
			result = "humanmale_64";
			break;
		case 2:
			result = "humanfemale_64";
			break;
		case 3:
			result = "furrymale_64";
			break;
		case 6:
			result = "fairyfemale_64";
			break;
		}
		return result;
	}

	private void ProcessOTPReady(eOTPRequestType eReqType)
	{
		if (this.szOTPAuthKey[(int)eReqType].Length > 0)
		{
			switch (eReqType)
			{
			case eOTPRequestType.OTPREQ_USERAUTH:
			{
				bool flag = false;
				NrWebViewObject gameObject = NrWebViewObject.GetGameObject();
				if (!gameObject.FirstNoticeOpen || gameObject.MainmenuNoticeOpen)
				{
					flag = true;
				}
				if (flag)
				{
					string strCharName = string.Empty;
					NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
					if (@char != null)
					{
						strCharName = @char.GetCharName();
					}
					NrMobileNoticeWeb nrMobileNoticeWeb = new NrMobileNoticeWeb();
					nrMobileNoticeWeb.OnGameNotice(this.szOTPAuthKey[0], strCharName);
				}
				gameObject.FirstNoticeOpen = true;
				gameObject.MainmenuNoticeOpen = false;
				break;
			}
			case eOTPRequestType.OTPREQ_CHARPORTRAIT:
			{
				NrCharBase char2 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (char2 != null)
				{
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					NkSoldierInfo leaderSoldierInfo = charPersonInfo.GetLeaderSoldierInfo();
					string arg = string.Empty;
					if (charPersonInfo != null && leaderSoldierInfo != null)
					{
						arg = this.GetProcessLeaderHero(leaderSoldierInfo.GetCharKind());
					}
					string url = string.Format("http://{0}/mobilephoto/user.aspx?otp={1}&type={2}", NrGlobalReference.strWebPageDomain, this.szOTPAuthKey[1], arg);
					Application.OpenURL(url);
				}
				break;
			}
			case eOTPRequestType.OTPREQ_GUILDMARK:
				if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID())
				{
					string url2 = string.Format("http://{0}/mobilephoto/guild.aspx?otp={1}", NrGlobalReference.strWebPageDomain, this.szOTPAuthKey[2]);
					Application.OpenURL(url2);
				}
				break;
			case eOTPRequestType.OTPREQ_EMAIL:
			{
				string text = string.Format("http://{0}/member/member_confirm1.aspx?OTP={1}", NrGlobalReference.strWebPageDomain, this.szOTPAuthKey[3]);
				NrMobileNoticeWeb nrMobileNoticeWeb2 = new NrMobileNoticeWeb();
				nrMobileNoticeWeb2.OpenWebURL(text);
				TsLog.LogOnlyEditor("!!!!!!!!!!!!!!!! OTPREQ_EMAIL :{0}" + text);
				break;
			}
			case eOTPRequestType.OTPREQ_HP_AUTH:
			{
				string url3 = string.Format("http://{0}/mobileAuth/auth.aspx?otp={1}", NrGlobalReference.strWebPageDomain, this.szOTPAuthKey[4]);
				Application.OpenURL(url3);
				NrTSingleton<NrMainSystem>.Instance.QuitGame();
				break;
			}
			case eOTPRequestType.OTPREQ_GUESTID:
			{
				string url4 = string.Empty;
				if (TsPlatform.IsAndroid)
				{
					url4 = string.Format("http://{0}/member/AuthPlatformSync_auth.aspx?OTP={1}&platform=android", NrGlobalReference.strWebPageDomain, this.szOTPAuthKey[5]);
				}
				else
				{
					url4 = string.Format("http://{0}/member/AuthPlatformSync_auth.aspx?OTP={1}&platform=ios", NrGlobalReference.strWebPageDomain, this.szOTPAuthKey[5]);
				}
				NrMobileNoticeWeb nrMobileNoticeWeb3 = new NrMobileNoticeWeb();
				nrMobileNoticeWeb3.OpenWebURL(url4);
				break;
			}
			case eOTPRequestType.OTPREQ_HELPQUESTION:
			{
				NrMobileNoticeWeb nrMobileNoticeWeb4 = new NrMobileNoticeWeb();
				nrMobileNoticeWeb4.OnGameQuestion(this.szOTPAuthKey[6]);
				break;
			}
			case eOTPRequestType.OTPREQ_UNREGISTER:
			{
				NrMobileNoticeWeb nrMobileNoticeWeb5 = new NrMobileNoticeWeb();
				nrMobileNoticeWeb5.OnGameUnregister(this.szOTPAuthKey[7]);
				break;
			}
			}
			this.szOTPAuthKey[(int)eReqType] = string.Empty;
		}
	}

	public void GetClientInfo(ref USER_CLIENT_INFO pkClientInfo)
	{
	}
}
