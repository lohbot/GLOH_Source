using GameMessage;
using Global;
using GooglePlayGames;
using Ndoors.Framework.Stage;
using PROTOCOL;
using StageHelper;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityForms;

public class StageLoginMobile : AStage
{
	private MobileLoginDlg m_LoginDlg;

	private LoginBGDlg m_LoginDlgBG;

	private SelectServerDlg m_SelectServer;

	private WWW m_NoticeCheck;

	public bool ExitFadeoutBGM;

	public static bool m_bConnectGameCenter;

	public override Scene.Type SceneType()
	{
		return Scene.Type.LOGIN;
	}

	public override void OnPrepareSceneChange()
	{
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		NrLoadPageScreen.StepUpMain(1);
		if (NrTSingleton<NrMainSystem>.Instance.m_ReLogin)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_LOADINGPAGE);
		}
		NrLoadPageScreen.ShowHideLoadingImg(false);
		UIDataManager.MuteBGM = TsAudio.IsMuteAudio(EAudioType.BGM);
		UIDataManager.MuteEffect = TsAudio.IsMuteAudio(EAudioType.SFX);
		if (null == NmMainFrameWork.loginCamera)
		{
			NmMainFrameWork.loginCamera = (Camera)UnityEngine.Object.Instantiate(Camera.main);
			NmMainFrameWork.loginCamera.name = "loginCamera";
			NmMainFrameWork.loginCamera.gameObject.SetActive(false);
			Transform child = NmMainFrameWork.loginCamera.transform.GetChild(0);
			if (null != child)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
			UnityEngine.Object.DontDestroyOnLoad(NmMainFrameWork.loginCamera);
		}
		NrTSingleton<NkBattleReplayManager>.Instance.Init();
	}

	public override void OnEnter()
	{
		string text = string.Format("{0} OnEnter OnEnter Memory = {1}MB", base.GetType().FullName, TsPlatform.Operator.GetAppMemory());
		TsPlatform.FileLog(text);
		TsLog.LogWarning(text, new object[0]);
		if (!TsPlatform.IsEditor && TsPlatform.IsAndroid && SystemInfo.processorCount < 2)
		{
			IntroMsgBoxDlg introMsgBoxDlg = (IntroMsgBoxDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INTROMSGBOX_DLG);
			if (introMsgBoxDlg != null)
			{
				string textFromPreloadText = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("10");
				introMsgBoxDlg.SetBtnChangeName(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"));
				introMsgBoxDlg.SetMsg(null, null, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"), textFromPreloadText, eMsgType.MB_OK);
			}
		}
		if (TsPlatform.IsEditor)
		{
			NmMainFrameWork.AddBGM();
		}
		if (TsPlatform.IsAndroid)
		{
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
		}
		TsLog.Log("====== {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		Scene.ChangeSceneType(this.SceneType());
		if (Camera.main != null && Camera.main.gameObject != null)
		{
			DefaultCameraController component = Camera.main.gameObject.GetComponent<DefaultCameraController>();
			if (component == null)
			{
				Camera.main.gameObject.AddComponent<DefaultCameraController>();
			}
			GameObject target = GameObject.Find("MainFramework");
			UnityEngine.Object.DontDestroyOnLoad(target);
		}
		else
		{
			NrMainSystem.CheckAndSetReLoginMainCamera();
		}
		if (NrTSingleton<NrMainSystem>.Instance.m_ReLogin)
		{
			TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.WorldScene);
			TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.BattleScene);
			base.StartTaskSerial(this.LoadLoginSleep());
			base.StartTaskSerial(CommonTasks.LoadEmptyMainScene());
			base.StartTaskSerial(CommonTasks.ClearAudioStack());
			base.StartTaskSerial(CommonTasks.MemoryCleaning(true, 8));
			NmMainFrameWork.LoadImage();
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGINBG_DLG);
		}
		base.StartTaskSerial(CommonTasks.SetGUIBehaviourScene());
		if (!NrTSingleton<NrMainSystem>.Instance.m_ReLogin && TsPlatform.IsEditor)
		{
			string strFileName = string.Format("{0}/../SystemData/ServerList.txt", Application.dataPath);
			NrConnectTable nrConnectTable = new NrConnectTable();
			nrConnectTable.AddServerList(strFileName);
		}
		if (!TsPlatform.IsEditor)
		{
			base.StartTaskSerial(this._RequestNoticeCheck());
			base.StartTaskSerial(this._RequestNoticePage());
			NrTSingleton<NrMainSystem>.Instance.m_bIsAutoLogin = true;
		}
		base.StartTaskSerial(this._RequestLoginNexonAuthorize());
		base.StartTaskSerial(this._StageProcess());
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(true));
	}

	public override void OnExit()
	{
		TsLog.Log("====== {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MOBILELOGIN_DLG);
	}

	protected override void OnUpdateAfterStagePrework()
	{
	}

	[DebuggerHidden]
	private IEnumerator _StageProcess()
	{
		return new StageLoginMobile.<_StageProcess>c__Iterator36();
	}

	public void BtnClickNexon(IUIObject obj)
	{
		int num = 0;
		bool flag = true;
		if (TsPlatform.IsEditor)
		{
			flag = true;
		}
		if (flag || num == 1)
		{
			NrLoadPageScreen.LoginLatestChar = false;
		}
	}

	public void BtnClickGuest(IUIObject obj)
	{
	}

	public void BtnGuestIDLogin(object obj)
	{
	}

	[DebuggerHidden]
	private IEnumerator LoadLoginMainScene()
	{
		return new StageLoginMobile.<LoadLoginMainScene>c__Iterator37();
	}

	[DebuggerHidden]
	private IEnumerator LoadLoginSleep()
	{
		return new StageLoginMobile.<LoadLoginSleep>c__Iterator38();
	}

	[DebuggerHidden]
	private IEnumerator _RequestLoginNexonAuthorize()
	{
		StageLoginMobile.<_RequestLoginNexonAuthorize>c__Iterator39 <_RequestLoginNexonAuthorize>c__Iterator = new StageLoginMobile.<_RequestLoginNexonAuthorize>c__Iterator39();
		<_RequestLoginNexonAuthorize>c__Iterator.<>f__this = this;
		return <_RequestLoginNexonAuthorize>c__Iterator;
	}

	public void ShowPlatform()
	{
		string @string = PlayerPrefs.GetString(NrPrefsKey.PLAYER_PREFS_MOBILEAUTHKEY);
		if (TsPlatform.IsIPhone)
		{
			Social.localUser.Authenticate(delegate(bool success)
			{
				if (success)
				{
					StageLoginMobile.m_bConnectGameCenter = true;
					UnityEngine.Debug.LogWarning("Start Game Center Authenticate. !!!!!!!!!!!");
				}
				else
				{
					StageLoginMobile.m_bConnectGameCenter = false;
					UnityEngine.Debug.LogWarning("Fail Start Game Center Authenticate. !!!!!!!!!!!");
				}
			});
		}
		if (!string.IsNullOrEmpty(@string))
		{
			MsgHandler.Handle("RequestAutoLogin", new object[0]);
			UnityEngine.Debug.LogError("@@@@@@@@@AutoLogin");
		}
		else
		{
			UnityEngine.Debug.LogError("@@@@@@@@@RequestLogin");
			Login_Select_PlatformDLG login_Select_PlatformDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGIN_SELECT_PLATFORM_DLG) as Login_Select_PlatformDLG;
			if (login_Select_PlatformDLG != null)
			{
				login_Select_PlatformDLG.Show();
			}
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGINRATING_DLG);
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_LOADINGPAGE);
	}

	public void ShowGuestLoginDLG()
	{
	}

	public void ShowLoginDLG()
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_LOADINGPAGE);
		this.m_LoginDlg = (MobileLoginDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MOBILELOGIN_DLG);
		this.m_LoginDlgBG = (LoginBGDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGINBG_DLG);
		this.m_LoginDlgBG.Show();
		this.m_SelectServer = (SelectServerDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SELECTSERVER_DLG);
		this.m_SelectServer.requestLogin.AddValueChangedDelegate(new EZValueChangedDelegate(this.LoginGameServer));
		this.m_SelectServer.pkListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnChannelList));
		this.m_SelectServer.pkListBox.AddDoubleClickDelegate(new EZValueChangedDelegate(this.LoginGameServerList));
		this.LoadPWFromFile();
		this.m_SelectServer.pkListBox.Visible = true;
		this.m_SelectServer.pkListBox.Clear();
		this.m_SelectServer.pkListBox.ColumnNum = 1;
		this.m_SelectServer.pkListBox.SetColumnWidth((int)this.m_LoginDlgBG.pkListBox.GetSize().x, 6, 0, 0, 0);
		this.m_SelectServer.pkListBox.SetColumnAlignment(0, SpriteText.Anchor_Pos.Middle_Left);
		foreach (NrServerInfo current in NrConnectTable.Instance.m_arServerInfo)
		{
			ListItem listItem = new ListItem();
			listItem.SetColumnStr(0, string.Concat(new string[]
			{
				current.m_strServerName,
				" ",
				current.m_strLoginServerIP,
				"/",
				current.m_strWorldServerIP
			}));
			listItem.Key = current;
			this.m_SelectServer.pkListBox.Add(listItem);
		}
		ListItem listItem2 = new ListItem();
		listItem2.SetColumnStr(0, "server list end.");
		listItem2.Key = new NrServerInfo();
		this.m_SelectServer.pkListBox.Add(listItem2);
		this.m_SelectServer.pkListBox.RepositionItems();
		this.m_SelectServer.Hide();
	}

	public bool LoginMobileGUI()
	{
		return true;
	}

	public bool EnableMobileLogin()
	{
		TsLog.Log("StageLoginMobile.EnableMobileLogin!", new object[0]);
		return true;
	}

	private void LoadPWFromFile()
	{
		string @string = PlayerPrefs.GetString("UserID", string.Empty);
		string string2 = PlayerPrefs.GetString("UserPW", string.Empty);
		this.m_LoginDlg.SetUserID(@string);
		this.m_LoginDlg.SetPassWD(string2);
	}

	private void RequestGameTerms()
	{
	}

	private void LoginGameServer(IUIObject obk)
	{
		MsgHandler.Handle("Save_LOGIN_USER_REQ", new object[]
		{
			this.m_LoginDlg.GetUserID(),
			this.m_LoginDlg.GetPassWD()
		});
	}

	private void BtnChannelList(IUIObject obj)
	{
		ListBox listBox = (ListBox)obj;
		NrServerInfo nrServerInfo = (NrServerInfo)listBox.GetDataObject();
		if (nrServerInfo.m_strLoginServerIP.Length == 0)
		{
			Main_UI_SystemMessage.ADDMessage("server IP Error !!!", SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		string ip = nrServerInfo.m_strLoginServerIP.Trim();
		string s = nrServerInfo.m_strLoginServerPort.Trim();
		BaseNet_Login.GetInstance().SetServerIPandPort(ip, int.Parse(s));
		NrTSingleton<NrMainSystem>.Instance.m_strWorldServerIP = nrServerInfo.m_strWorldServerIP.Trim();
		if (!int.TryParse(nrServerInfo.m_strWorldServerPort.Trim(), out NrTSingleton<NrMainSystem>.Instance.m_nWorldServerPort))
		{
			NrTSingleton<NrMainSystem>.Instance.m_nWorldServerPort = Client.GetWorldServerPort();
		}
	}

	private void LoginGameServerList(IUIObject obj)
	{
		this.BtnChannelList(obj);
		MsgHandler.Handle("Save_LOGIN_USER_REQ", new object[]
		{
			this.m_LoginDlg.GetUserID(),
			this.m_LoginDlg.GetPassWD()
		});
	}

	[DebuggerHidden]
	private IEnumerator _RequestNoticePage()
	{
		StageLoginMobile.<_RequestNoticePage>c__Iterator3A <_RequestNoticePage>c__Iterator3A = new StageLoginMobile.<_RequestNoticePage>c__Iterator3A();
		<_RequestNoticePage>c__Iterator3A.<>f__this = this;
		return <_RequestNoticePage>c__Iterator3A;
	}

	[DebuggerHidden]
	private IEnumerator _RequestNoticeCheck()
	{
		StageLoginMobile.<_RequestNoticeCheck>c__Iterator3B <_RequestNoticeCheck>c__Iterator3B = new StageLoginMobile.<_RequestNoticeCheck>c__Iterator3B();
		<_RequestNoticeCheck>c__Iterator3B.<>f__this = this;
		return <_RequestNoticeCheck>c__Iterator3B;
	}
}
