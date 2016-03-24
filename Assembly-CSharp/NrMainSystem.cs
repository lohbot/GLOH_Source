using GameMessage.Private;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using SERVICE;
using StageHelper;
using System;
using Ts;
using TsBundle;
using UnityEngine;
using UnityForms;

public class NrMainSystem : NrTSingleton<NrMainSystem>
{
	public NkInputManager m_kInputManager;

	public Main m_kMainCore;

	private float fTempTime;

	private float fLastTime;

	private float[] fAvgProcessTime = new float[20];

	private GUIStyle m_LabelStyle;

	private bool bShowLog;

	private long m_lAppMemory;

	private float m_fWorldEnterTime;

	private int m_nPlaytimeMinute;

	public string m_strWorldServerIP = "20.0.1.27";

	public int m_nWorldServerPort = 4000;

	public string m_strLatestPersonID = string.Empty;

	public long m_nLatestPersonID;

	public bool m_ReLogin;

	public bool m_bIsAutoLogin;

	public float fPingSendTime;

	public bool m_bPingTest;

	public bool m_bSendPing;

	public int m_nWorldServerPingCount;

	public bool m_Login_BG;

	public bool m_bIsBilling;

	public bool m_bIsShowBI;

	private bool m_bShowTerm;

	private long nCacheSize = 10737418240L;

	private int QuitTime;

	private CharMoveCommandLayer m_charMoveCommandLayer = new CharMoveCommandLayer();

	public long CurAppMemory
	{
		get
		{
			return this.m_lAppMemory;
		}
	}

	public long AppMemory
	{
		get
		{
			if (!TsPlatform.IsEditor && TsPlatform.IsMobile)
			{
				this.m_lAppMemory = TsPlatform.Operator.GetAppMemory();
				return this.m_lAppMemory;
			}
			return 0L;
		}
		set
		{
			this.m_lAppMemory = value;
		}
	}

	public float WorldEnterTime
	{
		get
		{
			return this.m_fWorldEnterTime;
		}
		set
		{
			this.m_fWorldEnterTime = value;
		}
	}

	public bool ShowTerm
	{
		get
		{
			return this.m_bShowTerm;
		}
		set
		{
			Debug.Log("m_bShowTerm ==" + value);
			this.m_bShowTerm = value;
		}
	}

	private NrMainSystem()
	{
		NrTSingleton<NrConfigFile>.Instance.LoadData();
		NrLoadPageScreen.Init();
		NrReceiveGame.RegisterReceivePacketFunction();
	}

	public CharMoveCommandLayer GetCharMoveCommandLayer()
	{
		return this.m_charMoveCommandLayer;
	}

	private void SetAuthorizeInfo()
	{
		switch (NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea())
		{
		case eSERVICE_AREA.SERVICE_WEBPLAYER_KORQA:
			Debug.Log("Cache Authorize Inner Domain - " + NrTSingleton<NrGlobalReference>.Instance.basePath);
			if (!TsCaching.Authorize("ndoorsat2_test_devkorea", "http://atlantica2.dn.nexoncdn.co.kr/", this.nCacheSize, -1, "9ed99fec890dc6ff08a55a1bb3a2ee35f230beea1978e8e35deaa9f43081ec73bcba689108e4299aff7bbb924471afed5603c143933d21162b581206058fb66850f9e421912f7c302aab9bdd99b5a71c7b238adffe9f7b027844c652ddd30ca4b822109cc41980c007de1c02559ae3c08837bf7507ad521ba3cc2ae7d1e2a58a"))
			{
				Debug.LogWarning("Cache Authorize [name = ndoorsat2_test_devkorea] Error!!");
			}
			break;
		case eSERVICE_AREA.SERVICE_WEBPLAYER_KORLIVE:
			Debug.Log("Cache Authorize Outer Domain - " + NrTSingleton<NrGlobalReference>.Instance.basePath);
			if (!TsCaching.Authorize("ndoorsat2_service_livekorea", "http://atlantica2.dn.nexoncdn.co.kr/", this.nCacheSize, -1, "5b180632c670ec79f15aa72bcca1218cae3b598d381a28dec47935a19144f299082dfcd9c33c2c6306ba26c8a7cf42c681a21bd48d8bc605424fcd022f6df5f0b36aa8d21231403dc1c65aa6efa49d4c2b351e9f67e3cebb589cc4ef39f18655170eebf16e9a38e315aa185f54db6062ba1d5a84e8a50b96a17227cc28c88f23"))
			{
				Debug.LogWarning("Cache Authorize [name = ndoorsat2_out_livetest] Error!!");
			}
			break;
		}
	}

	public void GenerationCacheInfo()
	{
		if (!NrTSingleton<NrGlobalReference>.Instance.IsSetServiceArea())
		{
			return;
		}
		if (TsPlatform.IsWeb && NrTSingleton<NrGlobalReference>.Instance.useCache)
		{
			this.SetAuthorizeInfo();
		}
	}

	public bool Initialize()
	{
		this.GenerationCacheInfo();
		this.m_kMainCore = new Main();
		this.m_kMainCore.Initialize();
		NrTSingleton<UIManager>.Instance.Initialize();
		NrTSingleton<FormsManager>.Instance.Initialize();
		this.m_kInputManager = new NkInputManager();
		this.m_kInputManager.Initialize();
		this.m_kInputManager.AddInputCommandLayer(new UICommandLayer());
		this.m_kInputManager.AddInputCommandLayer(new KeyboardShotCutCommandLayer());
		this.m_kInputManager.AddInputCommandLayer(this.m_charMoveCommandLayer);
		Holder.PushBundleGroup(NkBundleCallBack.PlayerBundleStackName);
		Holder.PushBundleGroup(NkBundleCallBack.NPCBundleStackName);
		Holder.PushBundleGroup(NkBundleCallBack.EffectBundleStackName);
		Holder.PushBundleGroup(NkBundleCallBack.BuildingBundleStackName);
		Holder.PushBundleGroup(NkBundleCallBack.UIBundleStackName);
		Holder.PushBundleGroup(Option.IndependentFromStageStackName);
		Holder.PushBundleGroup(NkBundleCallBack.BattlePreLoadingChar);
		Holder.PushBundleGroup(TsAudio.AssetBundleStackName);
		Holder.PushBundleGroup(NkBundleCallBack.AudioBundleStackName);
		for (int i = 0; i < 20; i++)
		{
			this.fAvgProcessTime[i] = 0f;
		}
		this.m_LabelStyle = new GUIStyle();
		this.m_LabelStyle.normal.textColor = new Color(255f, 255f, 255f);
		this.bShowLog = false;
		return true;
	}

	public void Awake()
	{
	}

	public void Alert(string msg)
	{
		Debug.LogError("[경보] " + msg);
	}

	public void ShutdownUnity()
	{
	}

	public void Start()
	{
	}

	public void CheckSpentTime(eMSTime eIndex)
	{
		if (!this.bShowLog)
		{
			this.fLastTime = Time.realtimeSinceStartup;
			if (this.fAvgProcessTime[(int)eIndex] > 0f)
			{
				float num = (this.fLastTime - this.fTempTime + this.fAvgProcessTime[(int)eIndex]) / 2f;
				int num2 = (int)(num * 10000f);
				this.fAvgProcessTime[(int)eIndex] = (float)num2 / 10000f;
			}
			else
			{
				float num3 = this.fLastTime - this.fTempTime;
				int num4 = (int)(num3 * 10000f);
				this.fAvgProcessTime[(int)eIndex] = (float)num4 / 10000f;
			}
			this.fTempTime = Time.realtimeSinceStartup;
		}
	}

	public void Update()
	{
		this.CheckSpentTime(eMSTime.MS_None);
		NrTSingleton<NrLogSystem>.Instance.Update();
		this.CheckSpentTime(eMSTime.MS_LogSystem);
		this.m_kInputManager.Update();
		this.CheckSpentTime(eMSTime.MS_InputManager);
		NrTSingleton<NkQuestManager>.Instance.UpdateEventCheck();
		this.CheckSpentTime(eMSTime.MS_QuestManager);
		StageSystem.Update();
		this.CheckSpentTime(eMSTime.MS_StageStackSystem);
		NrTSingleton<Nr3DCharSystem>.Instance.Update();
		this.CheckSpentTime(eMSTime.MS_3DCharSystem);
		NrTSingleton<NkCharManager>.Instance.Update();
		this.CheckSpentTime(eMSTime.MS_CharacterManager);
		using (new ScopeProfile("NrDebugConsole"))
		{
			NrTSingleton<NrDebugConsole>.Instance.Update();
		}
		this.CheckSpentTime(eMSTime.MS_DebugConsole);
		NrTSingleton<CMovingServer>.Instance.Update();
		this.CheckSpentTime(eMSTime.MS_MovingServer);
		NrTSingleton<FormsManager>.Instance.Update();
		this.CheckSpentTime(eMSTime.MS_FormsManager);
		NrTSingleton<NkEffectManager>.Instance.Update();
		this.CheckSpentTime(eMSTime.MS_EffectManager);
		using (new ScopeProfile("NkClientLogic"))
		{
			NrTSingleton<NkClientLogic>.Instance.Update();
		}
		this.CheckSpentTime(eMSTime.MS_ClientLogic);
		this.m_kMainCore.Update();
		this.CheckSpentTime(eMSTime.MS_MainCore);
		NrTSingleton<GameGuideManager>.Instance.Update();
		this.CheckSpentTime(eMSTime.MS_GameGuideManager);
		NrTSingleton<NkAutoRelogin>.Instance.Update();
		this.CheckSpentTime(eMSTime.MS_CAutoRelogin);
		this.CheckSpentTime(eMSTime.MS_ChallengeManager);
		NrTSingleton<NkBabelMacroManager>.Instance.Update();
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			Screen.sleepTimeout = -1;
		}
		if (this.m_bPingTest)
		{
			if (Time.realtimeSinceStartup - this.fPingSendTime > 4f)
			{
				GS_PING_REQ gS_PING_REQ = new GS_PING_REQ();
				gS_PING_REQ.fPingSendTime = Time.realtimeSinceStartup;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PING_REQ, gS_PING_REQ);
				this.fPingSendTime = Time.realtimeSinceStartup;
			}
		}
		else if (this.m_bSendPing && Time.realtimeSinceStartup - this.fPingSendTime > 45f)
		{
			GS_PING_REQ gS_PING_REQ2 = new GS_PING_REQ();
			gS_PING_REQ2.fPingSendTime = Time.realtimeSinceStartup;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PING_REQ, gS_PING_REQ2);
			this.fPingSendTime = Time.realtimeSinceStartup;
		}
		this.PlayTimeCheck();
		if (this.QuitTime != 0 && this.QuitTime < Environment.TickCount)
		{
			Application.Quit();
		}
	}

	public void LateUpdate()
	{
		this.CheckSpentTime(eMSTime.MS_None);
		NrTSingleton<FormsManager>.Instance.LateUpdate();
		NrTSingleton<NkCharManager>.Instance.LateUpdate();
		this.CheckSpentTime(eMSTime.MS_LateUpdate);
	}

	public void FixedUpdate()
	{
	}

	public void EscQuitGame()
	{
		if (TsPlatform.IsAndroid)
		{
			BaseNet_Login.GetInstance().Quit();
			this.QuitTime = Environment.TickCount + 100;
		}
	}

	public void QuitGame(bool bQuit = false)
	{
		if (PlayerPrefs.GetInt(NrPrefsKey.GUESTID) == 0)
		{
			PlayerPrefs.SetInt(NrPrefsKey.GUESTID, 0);
		}
		PlayerPrefs.Save();
		if (TsPlatform.IsEditor)
		{
			this.ReLogin(true);
		}
		else if (!TsPlatform.IsIPhone && !TsPlatform.IsWeb)
		{
			BaseNet_Login.GetInstance().Quit();
			this.QuitTime = Environment.TickCount + 300;
		}
		else
		{
			this.ReLogin(true);
		}
	}

	public void ReLogin(bool bShow = true)
	{
		if (this.m_ReLogin)
		{
			return;
		}
		this.m_bSendPing = false;
		this.m_nLatestPersonID = 0L;
		BaseNet_Game.GetInstance().Quit();
		BaseNet_Login.GetInstance().Quit();
		NrTSingleton<NkAutoRelogin>.Instance.SetActivity(false);
		Option.SetPause(false);
		TsLog.LogError("NEED REFACTORING - Old Stage Stack System Access!!!!!!!!!!!!", new object[0]);
		FacadeHandler.ClearStageStack();
		FacadeHandler.MoveStage(Scene.Type.LOGIN);
		this.ClearGameInfo(bShow);
		this.m_ReLogin = true;
	}

	public static void CheckAndSetReLoginMainCamera()
	{
		if (!Camera.main || !Camera.main.gameObject)
		{
			GameObject gameObject = new GameObject("Main Camera");
			gameObject.AddComponent<Camera>();
			gameObject.tag = "MainCamera";
			gameObject.transform.position = new Vector3(0f, 1f, -10f);
			Camera.main.gameObject.AddComponent<DefaultCameraController>();
			TsAudioListenerSwitcher tsAudioListenerSwitcher = new TsAudioListenerSwitcher(Camera.main.gameObject);
			tsAudioListenerSwitcher.Switch();
		}
	}

	public Main GetMainCore()
	{
		return this.m_kMainCore;
	}

	public NkInputManager GetInputManager()
	{
		return this.m_kInputManager;
	}

	public float GetProcessTime(int index)
	{
		return this.fAvgProcessTime[index];
	}

	public void LogProcessTime()
	{
		if (!this.bShowLog)
		{
			this.bShowLog = true;
		}
		else
		{
			this.bShowLog = false;
		}
	}

	public void SetLatestPersonID(string szPersonID)
	{
		this.m_strLatestPersonID = szPersonID.Trim();
		long.TryParse(this.m_strLatestPersonID, out this.m_nLatestPersonID);
	}

	public long GetLatestPersonID()
	{
		return this.m_nLatestPersonID;
	}

	public void OnApplicationPause()
	{
		EventTriggerMapManager.Instance.ActionTrigger_Pause();
	}

	public void MemoryCleanUP()
	{
		AutoMemoryCleanUp.CleanUp();
	}

	public void CleanUpReserved()
	{
		AutoMemoryCleanUp.CleanUpReserved();
	}

	public void CleanUpImmediate()
	{
		AutoMemoryCleanUp.CleanUpImmediate();
	}

	private void ClearGameInfo(bool bClear)
	{
		EventTriggerMapManager.Instance.Claer();
		this.m_nLatestPersonID = 0L;
		this.m_kInputManager.Initialize();
		TsAudioManager.Instance.InitAudioListenerSwitcher();
		if (bClear && NkUserInventory.instance != null)
		{
			NkUserInventory.instance.Clear();
		}
		NrTSingleton<NkQuestManager>.Instance.ClearCurrentQuest();
		NrTSingleton<NkQuestManager>.Instance.ClearCompleteQuest();
		NrTSingleton<Nr3DCharSystem>.Instance.Initialize();
		NrTSingleton<NkCharManager>.Instance.DeleteAllChar();
		NrTSingleton<NkCharManager>.Instance.Init(false);
		NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.Init();
		NrTSingleton<NkCharManager>.Instance.CharacterListSetComplete = false;
		if (bClear)
		{
			NrTSingleton<NewGuildManager>.Instance.Clear();
		}
		NrTSingleton<GameGuideManager>.Instance.InitGameGuide();
		NrTSingleton<GameGuideManager>.Instance.InitReserveGuide();
		NrTSingleton<FormsManager>.Instance.DeleteAll();
		NrTSingleton<NkClientLogic>.Instance.Init();
		NrTSingleton<NrGlobalReference>.Instance.ReloginInit();
		TsImmortal.bundleService.Init();
		NrTSingleton<NkEffectManager>.Instance.ClearEffectCache();
		NpcCache.Clear();
		NrTSingleton<ItemMallItemManager>.Instance.ClearItemMallBuyCount();
		NrTSingleton<ChatManager>.Instance.InitChatMsg();
		NrTSingleton<UIManager>.Instance.ClearInputQueue();
	}

	private void PlayTimeCheck()
	{
		if (this.m_fWorldEnterTime != 0f)
		{
			int num = (int)((Time.realtimeSinceStartup - this.m_fWorldEnterTime) / 60f);
			if (num != this.m_nPlaytimeMinute)
			{
				this.m_nPlaytimeMinute = num;
				if (this.m_nPlaytimeMinute % 5 == 0)
				{
					NrTSingleton<FiveRocksEventManager>.Instance.Placement("playtime_" + this.m_nPlaytimeMinute);
				}
			}
		}
	}
}
