using Global;
using Ndoors.Framework.Stage;
using omniata;
using PROTOCOL;
using PROTOCOL.WORLD;
using StageHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TsPatch;
using UnityEngine;
using UnityForms;

public class NmMainFrameWork : MonoBehaviour
{
	public const float BI_CHECK_TIME = 15f;

	public static int MAX_FPS = 60;

	public static int MIN_FPS = 15;

	private float m_fBITime;

	public static GameObject frameWorkGuiTexture = null;

	public static GameObject MobileStartBGM = null;

	public static Camera loginCamera = null;

	private static string m_strPushToken = string.Empty;

	private static string m_PlayLockDeviceID;

	public static bool Is1stRunPassed
	{
		get;
		set;
	}

	public string PushToken
	{
		get
		{
			return NmMainFrameWork.m_strPushToken;
		}
	}

	public static string PlayLockDeviceID
	{
		get
		{
			if (string.IsNullOrEmpty(NmMainFrameWork.m_PlayLockDeviceID))
			{
				NmMainFrameWork.m_PlayLockDeviceID = PlayerPrefs.GetString(NrPrefsKey.PLAYER_PLAYLOCKDEVICEID, string.Empty);
			}
			return NmMainFrameWork.m_PlayLockDeviceID;
		}
	}

	public static void ApplySmartFPS()
	{
		NmMainFrameWork.MAX_FPS = 60;
		int num;
		if (PlayerPrefs.HasKey("SaveFps"))
		{
			num = PlayerPrefs.GetInt("SaveFps");
		}
		else if (TsPlatform.IsAndroid)
		{
			num = TsHardwareAnalyzer.GetSmartFPS();
		}
		else
		{
			num = 30;
		}
		if (TsPlatform.IsMobile)
		{
			num = Math.Min(num, NmMainFrameWork.MAX_FPS);
		}
		Application.targetFrameRate = num;
		TsLog.LogWarning("SmartFPS={0} (H/W level={1} Mem = {2}) ", new object[]
		{
			num,
			TsHardwareAnalyzer.GetLevel(),
			SystemInfo.systemMemorySize
		});
	}

	private void Awake()
	{
		if (NmMainFrameWork.Is1stRunPassed)
		{
			return;
		}
		this.m_fBITime = Time.time + 15f;
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		QualitySettings.vSyncCount = 0;
		NmMainFrameWork.ApplySmartFPS();
		NrTSingleton<NrGlobalReference>.Instance.Init();
		TsAudioManager.Instance.FirstInit();
		if (TsPlatform.IsMobile)
		{
			this.ShowCompanyLogoMov();
			NmMainFrameWork.LoadImage();
		}
		NrTSingleton<NrMainSystem>.Instance.Awake();
		StageSystem.SetStartStage(new StageSystemCheck());
	}

	public static void LoadImage()
	{
		if (null == NmMainFrameWork.frameWorkGuiTexture)
		{
			if (null != NmMainFrameWork.loginCamera)
			{
				NmMainFrameWork.loginCamera.gameObject.SetActive(true);
			}
			NmMainFrameWork.frameWorkGuiTexture = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Prefabs/ObjPreLoading"));
			CResources.ADDPrefabLoad(NmMainFrameWork.frameWorkGuiTexture, "Mobile/Prefabs/ObjPreLoading");
		}
	}

	public static void DeleteImage()
	{
		if (NmMainFrameWork.frameWorkGuiTexture != null)
		{
			UnityEngine.Object.DestroyImmediate(NmMainFrameWork.frameWorkGuiTexture);
			NmMainFrameWork.frameWorkGuiTexture = null;
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		return new NmMainFrameWork.<Start>c__Iterator60();
	}

	private void Update()
	{
		DeviceOrientation deviceOrientation = Input.deviceOrientation;
		if (deviceOrientation == DeviceOrientation.LandscapeLeft)
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
		else if (deviceOrientation == DeviceOrientation.LandscapeRight)
		{
			Screen.orientation = ScreenOrientation.LandscapeRight;
		}
		if (!PatchFinalList.Instance.isLoadedOrSkipped)
		{
			return;
		}
		if (!NrTSingleton<NrGlobalReference>.Instance.IsSetServiceArea())
		{
			return;
		}
		if (NmMainFrameWork.Is1stRunPassed)
		{
			NrTSingleton<NrMainSystem>.Instance.Update();
		}
		else
		{
			StageSystem.Update();
		}
		if (Scene.CurScene <= Scene.Type.LOGIN && !NrTSingleton<NrMainSystem>.Instance.m_bIsShowBI && this.m_fBITime < Time.time)
		{
			CommonTasks.MuteAudioOnOff(false);
			NmMainFrameWork.AddBGM();
			NrTSingleton<NrMainSystem>.Instance.m_bIsShowBI = true;
		}
	}

	private void LateUpdate()
	{
		if (!PatchFinalList.Instance.isLoadedOrSkipped)
		{
			return;
		}
		NrTSingleton<NrMainSystem>.Instance.LateUpdate();
	}

	private void FixedUpdate()
	{
		if (!PatchFinalList.Instance.isLoadedOrSkipped)
		{
			return;
		}
		NrTSingleton<NrMainSystem>.Instance.FixedUpdate();
		NkCutScene_Camera_Manager.Instance.Update();
	}

	private void OnApplicationPause(bool pause)
	{
		UnityEngine.Debug.LogWarning("OnApplicationPause  == " + pause);
		if (TsPlatform.IsMobile)
		{
			if (NmFacebookManager.instance.IsFacebook)
			{
				return;
			}
			if (NrTSingleton<NrMainSystem>.Instance.m_bIsBilling)
			{
				return;
			}
			GameObject gameObject = GameObject.Find("OmniataManager");
			if (gameObject == null)
			{
				gameObject = new GameObject("OmniataManager");
			}
			if (gameObject != null)
			{
				OmniataComponent component = gameObject.GetComponent<OmniataComponent>();
				if (component && !pause)
				{
					DateTime dateTime = DateTime.Now.ToLocalTime();
					DateTime arg_AE_0 = dateTime;
					DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
					int num = (int)(arg_AE_0 - dateTime2.ToLocalTime()).TotalSeconds;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("ts", num.ToString());
					dictionary.Add("step", "pauseoff");
					dictionary.Add("device", SystemInfo.deviceUniqueIdentifier);
					if (TsPlatform.IsAndroid)
					{
						dictionary.Add("version", TsPlatform.APP_VERSION_AND);
					}
					else if (TsPlatform.IsIPhone)
					{
						dictionary.Add("version", TsPlatform.APP_VERSION_IOS);
					}
					component.TrackLoad(dictionary);
				}
			}
			if (pause && Scene.CurScene > Scene.Type.LOGIN)
			{
				NrTSingleton<FiveRocksEventManager>.Instance.Placement("Game_end");
				UnityEngine.Debug.LogWarning("OnApplicationPause");
				NrTSingleton<NrMainSystem>.Instance.OnApplicationPause();
				BaseNet_Game.GetInstance().Quit();
			}
			else if (NrWebViewObject.GetGameObject() == null)
			{
				UnityEngine.Debug.LogWarning("NrWebViewObject == NULL");
			}
		}
	}

	private void CallBackQuitGame(object kObject)
	{
		NrTSingleton<NrMainSystem>.Instance.QuitGame();
	}

	private void OnApplicationFocus(bool focussing)
	{
		if (Application.isWebPlayer)
		{
			if (focussing)
			{
				Application.targetFrameRate = NmMainFrameWork.MAX_FPS;
			}
			else
			{
				Application.targetFrameRate = NmMainFrameWork.MIN_FPS;
			}
		}
	}

	private void OnDestroy()
	{
	}

	private void OnApplicationQuit()
	{
		if (Scene.IsCurScene(Scene.Type.LOGIN))
		{
			BaseNet_Login.GetInstance().Quit();
		}
	}

	public void NotFoundCookie(string str)
	{
	}

	public void SetUserID(string strID)
	{
		NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szUserID = strID.Trim();
		UnityEngine.Debug.Log(">>Debug.Log>> SetUserID : " + strID);
	}

	public void SetAuthKey(string strAuthKey)
	{
		NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey = strAuthKey.Trim();
		UnityEngine.Debug.Log(">>Debug.Log>> SetAuthKey : " + strAuthKey);
	}

	public void SetUserSN(string sn)
	{
		NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nSerialNumber = Convert.ToInt64(sn);
		UnityEngine.Debug.Log(">>Debug.Log>> SetSerialNumber : " + sn.ToString());
	}

	public void SetLatestPersonID(string szPersonID)
	{
		NrTSingleton<NrMainSystem>.Instance.SetLatestPersonID(szPersonID);
		UnityEngine.Debug.Log(">>Debug.Log>> SetLatestPersonID : " + szPersonID);
	}

	public void SetServerIP(string strIP)
	{
		NrTSingleton<NrMainSystem>.Instance.m_strWorldServerIP = strIP.Trim();
		UnityEngine.Debug.Log(">>Debug.Log>> SetServerIP: " + strIP);
	}

	public void SetServerPort(string strPort)
	{
		if (!int.TryParse(strPort.Trim(), out NrTSingleton<NrMainSystem>.Instance.m_nWorldServerPort))
		{
			NrTSingleton<NrMainSystem>.Instance.m_nWorldServerPort = Client.GetWorldServerPort();
		}
		UnityEngine.Debug.Log(">>Debug.Log>> SetServerPort: " + strPort + ", Last: " + NrTSingleton<NrMainSystem>.Instance.m_nWorldServerPort.ToString());
	}

	public void SetClientPath(string strPath)
	{
		NrTSingleton<NrGlobalReference>.Instance.basePath = strPath;
		UnityEngine.Debug.LogWarning("NrGlobalReference basePath = " + strPath);
		NrTSingleton<NrMainSystem>.Instance.GenerationCacheInfo();
		NrGlobalReference.SetWebBasePathExternalCalled();
	}

	public void DoneRegist()
	{
	}

	public void FoundCookie(string str)
	{
		string[] array = str.Split(new char[]
		{
			'='
		});
		NrTSingleton<NrConfigFile>.Instance.SetData(array[0], array[1]);
	}

	public void LoadedAllConfigFile()
	{
		NrTSingleton<NrConfigFile>.Instance.SetLoadedAll();
	}

	public void AudioPlay(string strMute)
	{
		UnityEngine.Debug.LogWarning("AutoPlay SET: " + strMute);
		bool bMute = strMute == "1";
		CommonTasks.MuteAudioOnOff(bMute);
		if (strMute == "0")
		{
			if (Scene.CurScene <= Scene.Type.LOGIN)
			{
				NmMainFrameWork.AddBGM();
				NrTSingleton<NrMainSystem>.Instance.m_bIsShowBI = true;
			}
			else if (Scene.CurScene == Scene.Type.BATTLE)
			{
				Battle_ResultTutorialDlg battle_ResultTutorialDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_TUTORIAL_DLG) as Battle_ResultTutorialDlg;
				if (battle_ResultTutorialDlg != null)
				{
					battle_ResultTutorialDlg.Show();
					battle_ResultTutorialDlg.PlayMovie = false;
				}
			}
		}
		else
		{
			this.m_fBITime = Time.time + 15f;
		}
	}

	public void OnReceivePushToken(string token)
	{
		NmMainFrameWork.m_strPushToken = token;
		TsLog.LogWarning("OnReceivePushToken = {0}", new object[]
		{
			NmMainFrameWork.m_strPushToken
		});
		WS_PUSHTOKEN_SET_REQ wS_PUSHTOKEN_SET_REQ = new WS_PUSHTOKEN_SET_REQ();
		TKString.StringChar(NmMainFrameWork.m_strPushToken, ref wS_PUSHTOKEN_SET_REQ.m_szPushToken);
		SendPacket.GetInstance().SendObject(16777272, wS_PUSHTOKEN_SET_REQ);
	}

	public void OnReceivePlayLockDeviceID(string strDeviceID)
	{
		NmMainFrameWork.m_PlayLockDeviceID = strDeviceID;
		PlayerPrefs.SetString(NrPrefsKey.PLAYER_PLAYLOCKDEVICEID, NmMainFrameWork.m_PlayLockDeviceID);
		if (!string.IsNullOrEmpty(NmMainFrameWork.PlayLockDeviceID) && NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel() >= 10 && PlayerPrefs.GetInt(NrPrefsKey.PLAYER_PLAYLOCKDEVICEID_SEND, 0) == 0)
		{
			long deviceID = 0L;
			if (long.TryParse(NmMainFrameWork.PlayLockDeviceID, out deviceID))
			{
				WS_PLAYLOCK_REWAED_REQ wS_PLAYLOCK_REWAED_REQ = new WS_PLAYLOCK_REWAED_REQ();
				TKString.StringChar(DateTime.Now.ToString("yyyyMMddHHmmss"), ref wS_PLAYLOCK_REWAED_REQ.RequestKey);
				wS_PLAYLOCK_REWAED_REQ.DeviceID = deviceID;
				wS_PLAYLOCK_REWAED_REQ.QuestKey = 1;
				SendPacket.GetInstance().SendObject(16777278, wS_PLAYLOCK_REWAED_REQ);
			}
		}
	}

	public static void AddBGM()
	{
		if (TsAudio.IsMuteAudio(EAudioType.BGM))
		{
			return;
		}
		if (NmMainFrameWork.MobileStartBGM == null)
		{
			NmMainFrameWork.MobileStartBGM = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Prefabs/00-SOUND-BGM-CUSTOMOZING-LOGIN"));
			NmMainFrameWork.MobileStartBGM.transform.parent = TsImmortal.gameObject.transform;
		}
	}

	public static void RemoveBGM(bool Fadeout)
	{
		if (!Fadeout)
		{
			UnityEngine.Object.DestroyImmediate(NmMainFrameWork.MobileStartBGM);
			NmMainFrameWork.MobileStartBGM = null;
		}
		else
		{
			if (NmMainFrameWork.MobileStartBGM == null)
			{
				NmMainFrameWork.AddBGM();
				if (NmMainFrameWork.MobileStartBGM == null)
				{
					return;
				}
			}
			NmMainFrameWork.MobileStartBGM.GetComponent<TsAudioAdapterBGM>().FadeOut();
		}
	}

	public void ShowCompanyLogoMov()
	{
		string str = string.Empty;
		string text = "LOHBI.mp4";
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			str = Application.dataPath + "/Raw/";
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			str = Application.temporaryCachePath + "/";
			if (!File.Exists(str + text))
			{
				WWW wWW = new WWW("jar:file://" + Application.dataPath + "!/assets/" + text);
				while (!wWW.isDone)
				{
				}
				if (!string.IsNullOrEmpty(wWW.error))
				{
					UnityEngine.Debug.Log(string.Concat(new string[]
					{
						"Error unpacking 'jar:file://",
						Application.dataPath,
						"!/assets/",
						text,
						"'"
					}));
					str = string.Empty;
				}
				File.WriteAllBytes(str + text, wWW.bytes);
			}
			else
			{
				WWW wWW2 = new WWW("jar:file://" + Application.dataPath + "!/assets/" + text);
				while (!wWW2.isDone)
				{
				}
				if (!string.IsNullOrEmpty(wWW2.error))
				{
					UnityEngine.Debug.Log(string.Concat(new string[]
					{
						"Error unpacking 'jar:file://",
						Application.dataPath,
						"!/assets/",
						text,
						"'"
					}));
					str = string.Empty;
				}
				if (wWW2.bytes.Length != File.ReadAllBytes(str + text).Length)
				{
					File.WriteAllBytes(str + text, wWW2.bytes);
				}
			}
		}
		else
		{
			str = Application.dataPath + "/StreamingAssets/";
		}
		if (!TsPlatform.IsEditor)
		{
			string path = str + text;
			NmMainFrameWork.PlayMovieURL(path, false, true);
		}
	}

	public void ChatMessage(string strMessage)
	{
		if (TsPlatform.IsIPhone)
		{
			NrTSingleton<UIManager>.Instance.GetIOSIMEKeyboard(strMessage);
		}
	}

	public static void PlayMovieURL(string Path, bool bShowToast = true, bool bFile = false)
	{
		float num;
		if (TsAudio.IsMuteAudio(EAudioType.BGM))
		{
			num = 0f;
		}
		else
		{
			num = TsAudio.GetVolumeOfAudio(EAudioType.BGM);
		}
		if (bFile && TsPlatform.IsIPhone)
		{
			TsPlatform.Operator.PlayMovieAtPath(Path, num);
		}
		else
		{
			TsPlatform.Operator.PlayMovieURL(Path, Color.black, bShowToast, num);
		}
	}

	public static void ChagneResolution()
	{
		if (TsPlatform.IsMobile)
		{
			Screen.sleepTimeout = -1;
			if (Screen.width > 1280)
			{
				if (TsPlatform.IsAndroid)
				{
					if (Screen.width == 2048)
					{
						Screen.SetResolution(Screen.width / 2, Screen.height / 2, true);
					}
					else
					{
						Screen.SetResolution(1280, 720, true);
					}
				}
				else if (Screen.width == 2048)
				{
					Screen.SetResolution(1024, 768, true);
				}
				else
				{
					Screen.SetResolution(1280, 720, true);
				}
			}
			Application.runInBackground = true;
			LightmapSettings.lightmapsMode = LightmapsMode.Single;
			TsPlatform.FileLog("==== CHANGE RESOULUTION =======");
		}
	}
}
