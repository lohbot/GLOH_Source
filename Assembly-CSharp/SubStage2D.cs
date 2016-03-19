using GameMessage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SubStage2D : ASubStage
{
	private const float CREATEANI_FIX_TIME = 2f;

	private const float CREATECOMPLETE_FIX_TIME = 2f;

	private CharSelectDlg m_SelectForm;

	private NrCharUser m_kSelectedChar;

	private MsgBoxUI m_Boxdeletemsg;

	private bool m_isConnectGameServer;

	private long m_SelectPersonID;

	private CCameraAniPlay m_CameraAniPlay = NrTSingleton<CCameraAniPlay>.Instance;

	private E_CHAR_SELECT_STEP m_SelectStep;

	private InputCommandLayer m_CharselInput;

	public override void Start()
	{
		this.m_CameraAniPlay.Init();
		this.m_SelectForm = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHAR_SELECT_DLG) as CharSelectDlg);
		for (int i = 0; i < 3; i++)
		{
			Button expr_36 = this.m_SelectForm.CharSlot[i];
			expr_36.Click = (EZValueChangedDelegate)Delegate.Combine(expr_36.Click, new EZValueChangedDelegate(this.OnEventSelect));
			Button expr_64 = this.m_SelectForm.Delete[i];
			expr_64.Click = (EZValueChangedDelegate)Delegate.Combine(expr_64.Click, new EZValueChangedDelegate(this.OnEventDeleteUser));
		}
		this.m_SelectForm.SetScreenCenter();
		this.m_SelectForm.Hide();
		this.DisplayNames(true);
		this.m_isConnectGameServer = false;
		Camera.main.backgroundColor = Color.black;
	}

	public override void OnExit()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SELECTCHAR_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SYSTEM_OPTION);
		NrTSingleton<NrMainSystem>.Instance.m_kInputManager.RemoveInputCommandLayer(this.m_CharselInput);
		this.m_CameraAniPlay = null;
	}

	public bool _ConnectGameServer()
	{
		this._OnBottomButtonSound();
		if (!this.m_isConnectGameServer)
		{
			long personID;
			if (this.m_kSelectedChar != null)
			{
				personID = this.m_kSelectedChar.GetPersonID();
			}
			else
			{
				personID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID;
			}
			MsgHandler.Handle("Req_CONNECT_GAMESERVER_REQ", new object[]
			{
				personID
			});
			this.m_isConnectGameServer = true;
			return true;
		}
		return false;
	}

	public bool _StartGame()
	{
		long num;
		if (this.m_SelectPersonID != 0L)
		{
			num = this.m_SelectPersonID;
		}
		else if (NrTSingleton<NrMainSystem>.Instance.GetLatestPersonID() != 0L)
		{
			num = NrTSingleton<NrMainSystem>.Instance.GetLatestPersonID();
		}
		else
		{
			num = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID;
		}
		if (num == 0L)
		{
			MsgBoxUI msgBoxUI = (MsgBoxUI)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG);
			if (msgBoxUI != null)
			{
				msgBoxUI.SetMsg(new YesDelegate(this._OnMessageBoxOK_QuitGame), null, "경고", "personid가 0이다,,...\r\n어플을 재실행해주세요.", eMsgType.MB_OK);
			}
			TsLog.LogWarning("_StartGame personid == 0", new object[0]);
		}
		GS_AUTH_SESSION_REQ gS_AUTH_SESSION_REQ = new GS_AUTH_SESSION_REQ();
		gS_AUTH_SESSION_REQ.UID = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID;
		gS_AUTH_SESSION_REQ.SessionKey = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_siAuthSessionKey;
		gS_AUTH_SESSION_REQ.PersonID = num;
		gS_AUTH_SESSION_REQ.nMode = 500;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUTH_SESSION_REQ, gS_AUTH_SESSION_REQ);
		return true;
	}

	private void _OnMessageBoxOK_QuitGame(object a_oObject)
	{
		NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
		NrTSingleton<NrMainSystem>.Instance.QuitGame();
	}

	public void CharacterSelect(NrCharUser pkChar, bool bConnectServer)
	{
		this.m_kSelectedChar = pkChar;
		if (bConnectServer)
		{
			this._ConnectGameServer();
		}
	}

	public override void Update()
	{
		if (NrTSingleton<NkCharManager>.Instance.CharacterListSetComplete)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(NrTSingleton<NkCharManager>.Instance.SelectedCharID);
			if (@char != null)
			{
				NrCharUser nrCharUser = @char as NrCharUser;
				if (nrCharUser != null && nrCharUser.IsShaderRecovery() && NrTSingleton<NkCharManager>.Instance.SelectedCharID != 0)
				{
					this.CharacterSelect(nrCharUser, false);
				}
			}
		}
		if (this.m_CameraAniPlay == null)
		{
			return;
		}
		E_CHAR_SELECT_STEP e_CHAR_SELECT_STEP = this.m_CameraAniPlay.Update();
		if (e_CHAR_SELECT_STEP != E_CHAR_SELECT_STEP.NONE)
		{
			this.m_SelectStep = e_CHAR_SELECT_STEP;
		}
		E_CHAR_SELECT_STEP selectStep = this.m_SelectStep;
		if (selectStep != E_CHAR_SELECT_STEP.INTRO)
		{
			if (selectStep != E_CHAR_SELECT_STEP.CREATE_SELECT)
			{
				if (this.m_CharselInput != null)
				{
					NrTSingleton<NrMainSystem>.Instance.m_kInputManager.RemoveInputCommandLayer(this.m_CharselInput);
					this.m_CharselInput = null;
				}
			}
			else if (this.m_CharselInput == null)
			{
				this.m_CharselInput = new CharSelectCommandLayer();
				NrTSingleton<NrMainSystem>.Instance.m_kInputManager.AddInputCommandLayer(this.m_CharselInput);
			}
		}
		else if (NkInputManager.GetMouseButtonUp(0))
		{
			if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.TOASTMSG_DLG))
			{
				NrTSingleton<CCameraAniPlay>.Instance.SkipEvent();
			}
			else
			{
				ToastMsgDlg toastMsgDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TOASTMSG_DLG) as ToastMsgDlg;
				toastMsgDlg.SetMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("24"));
			}
		}
	}

	public void DisplayNames(bool bMakeName)
	{
		for (short num = 1; num <= 3; num += 1)
		{
			int charID = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.GetCharID((int)num);
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(charID) as NrCharUser;
			if (nrCharUser == null)
			{
				this.ShowHideCharName((int)num, false);
			}
			else
			{
				this.ShowHideCharName((int)num, true);
				if (bMakeName)
				{
					this.SetCharName((int)num, nrCharUser.GetPersonInfo().GetLevel(0L), nrCharUser.GetPersonInfo().GetCharName(), nrCharUser.GetCharKindInfo().GetCHARKIND_INFO().CharTribe, nrCharUser.GetPersonInfo().GetPersonID(), nrCharUser.GetCharKindInfo().GetCHARKIND_INFO().CHARKIND);
				}
				else
				{
					TsLog.Log("Test bMakeName == false", new object[0]);
					this.ShowHideCharName((int)num, bMakeName);
				}
			}
		}
	}

	private void SetCharName(int nSlot)
	{
		this.SetCharName(nSlot, 0, string.Empty, string.Empty, 0L, 0);
	}

	private void SetCharName(int nSlot, int nLevel, string strName, string CharTribe, long _PersonID, int CHARKIND)
	{
		if (this.m_SelectForm != null)
		{
			this.m_SelectForm.SetCharSet(nSlot, nLevel, strName, CharTribe, _PersonID, CHARKIND);
		}
	}

	public void ShowHideCharName(int nSlot, bool bShow)
	{
		TsLog.Log("ShowHideCharName Slot : {0} , SHOW : {1}", new object[]
		{
			nSlot,
			bShow
		});
		if (this.m_SelectForm != null)
		{
			this.m_SelectForm.ShowHideCharName(nSlot, bShow);
		}
	}

	public void _CreateCharComplete()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CUSTOMOZING", "CREATE-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		TsAudioManager.Instance.AudioContainer.RemoveBGM("intro");
		NmMainFrameWork.AddBGM();
	}

	public void OnEventSystemOption(IUIObject obj)
	{
	}

	private void _OnBottomButtonSound()
	{
	}

	private void _OnSelectButtonClickSound()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CUSTOMOZING", "SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void OnEventSelect(IUIObject obj)
	{
		Button button = obj as Button;
		if (this.m_SelectForm.SelectButton == null || this.m_SelectForm.SelectButton != button)
		{
			this.m_SelectForm.SelectButton = button;
			this._OnSelectButtonClickSound();
		}
		else if (this.m_SelectForm.SelectButton == button)
		{
			this.OnEventStartGame(obj);
			this.OnEventCreateUser(obj);
		}
	}

	public void OnEventMouseOver(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CUSTOMOZING", "CHAR_MOUSEOVER", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void OnEventStartGame(IUIObject obj)
	{
		Button button = obj as Button;
		if (this.m_SelectForm.GetShowChar(button.TabIndex))
		{
			this._ConnectGameServer();
			this.m_SelectPersonID = this.m_SelectForm.GetPersonID(button.TabIndex);
		}
	}

	public void OnEventCreateUser(IUIObject obj)
	{
		Button button = obj as Button;
		if (!this.m_SelectForm.GetShowChar(button.TabIndex))
		{
			this.m_CameraAniPlay.Add(E_CHAR_SELECT_STEP.CREATE_SELECT, new Action<object>(this._OnRaceSelect), new object[]
			{
				E_CAMARA_STATE_ANI.INTRO1,
				E_CAMARA_STATE_ANI.INTROTOCREATE
			});
			NrTSingleton<FormsManager>.Instance.Hide(G_ID.CHAR_SELECT_DLG);
			this.m_isConnectGameServer = false;
			this.m_SelectStep = E_CHAR_SELECT_STEP.INTRO;
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("BGM", "CUSTOMIZING", "CHAR-CREATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			NmMainFrameWork.RemoveBGM(false);
		}
	}

	public void OnEventDeleteUser(IUIObject obj)
	{
		Button button = obj as Button;
		if (this.m_SelectForm.GetShowChar(button.TabIndex))
		{
			this._OnBottomButtonSound();
			this.m_Boxdeletemsg = (MsgBoxUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG);
			this.m_Boxdeletemsg.SetLocation((GUICamera.width - this.m_Boxdeletemsg.GetSize().x) / 2f, ((float)Screen.height - this.m_Boxdeletemsg.GetSize().y) / 2f);
			string text = string.Empty;
			string title = string.Empty;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("51");
			title = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("53");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"Char_Name",
				this.m_SelectForm.GetName(button.TabIndex)
			});
			this.m_Boxdeletemsg.SetMsg(new YesDelegate(this._OKCharDelete), obj, title, empty, eMsgType.MB_OK_CANCEL);
			this.m_isConnectGameServer = false;
		}
	}

	private void _OnRaceSelect(object obj)
	{
	}

	private void _OnReceSelectToSelectBack(IUIObject obj)
	{
		NmMainFrameWork.AddBGM();
		TsAudioManager.Instance.AudioContainer.RemoveBGM("intro");
		this._OnBottomButtonSound();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.RACE_SELECT_DLG);
		NrTSingleton<FormsManager>.Instance.Show(G_ID.CHAR_SELECT_DLG);
		this.m_SelectStep = E_CHAR_SELECT_STEP.CHAR_SELECT;
	}

	private void _OnCharSelect(object obj)
	{
		NrTSingleton<FormsManager>.Instance.Show(G_ID.CHAR_SELECT_DLG);
		this.DisplayNames(true);
	}

	private void _OKCharDelete(object a_oObject)
	{
		Button button = a_oObject as Button;
		NrNetworkCustomizing nrNetworkCustomizing = (NrNetworkCustomizing)NrTSingleton<NrNetworkSystem>.Instance.GetNetworkComponent();
		long personID = this.m_SelectForm.GetPersonID(button.TabIndex);
		if (personID != 0L)
		{
			nrNetworkCustomizing.SendPacket_DeleteAvatar(personID);
		}
	}

	public void SetSelectStep(E_CHAR_SELECT_STEP _step)
	{
		this.m_SelectStep = _step;
	}

	public void SetCreateChar(E_CHAR_TRIBE _type)
	{
	}

	public void GoIntro()
	{
		this.m_CameraAniPlay.Add(E_CHAR_SELECT_STEP.CREATE_SELECT, new Action<object>(this._OnRaceSelect), new object[]
		{
			E_CAMARA_STATE_ANI.INTRO1,
			E_CAMARA_STATE_ANI.INTROTOCREATE
		});
		NrTSingleton<FormsManager>.Instance.Hide(G_ID.CHAR_SELECT_DLG);
		this.m_isConnectGameServer = false;
		this.m_SelectStep = E_CHAR_SELECT_STEP.INTRO;
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("BGM", "CUSTOMIZING", "CHAR-CREATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NmMainFrameWork.RemoveBGM(false);
	}
}
