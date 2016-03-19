using GameMessage;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using PROTOCOL.WORLD;
using System;
using UnityEngine;
using UnityForms;

public class NkAutoRelogin : NrTSingleton<NkAutoRelogin>
{
	public enum eAUTORELOGIN_STATE
	{
		E_AUTORELOGIN_STATE_DETECT_DISCONNECT,
		E_AUTORELOGIN_STATE_RECONNECTING,
		E_AUTORELOGIN_STATE_WAITING_ENCRYPTKEY,
		E_AUTORELOGIN_STATE_TRYING_RELOGIN,
		E_AUTORELOGIN_STATE_WAITING_RELOGIN,
		E_AUTORELOGIN_STATE_RELOGINED
	}

	private bool m_bIsActivity;

	public long m_tickCount;

	private NkAutoRelogin.eAUTORELOGIN_STATE m_eAutoReloginState;

	private bool bRequestRelogin;

	public bool bRequestConvertID;

	public NkAutoRelogin.eAUTORELOGIN_STATE AutoReloginState
	{
		get
		{
			return this.m_eAutoReloginState;
		}
	}

	private NkAutoRelogin()
	{
	}

	public bool Initialize()
	{
		this.SetActivity(false);
		this.bRequestRelogin = false;
		return true;
	}

	public void SetActivity(bool bIsActivity)
	{
		if (bIsActivity)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.LogWarning("[NkAutoRelogin] Set Activity True", new object[0]);
			}
		}
		else if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.LogWarning("[NkAutoRelogin] Set Activity false", new object[0]);
		}
		this.m_bIsActivity = bIsActivity;
		this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_DETECT_DISCONNECT;
		this.m_tickCount = PublicMethod.GetTick();
	}

	public void SetReconnecting(string addmsg)
	{
		this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_RECONNECTING;
	}

	public bool IsWaitingEncryptKey()
	{
		return this.m_eAutoReloginState == NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_WAITING_ENCRYPTKEY;
	}

	public void Yes_Delegate(object a_oObject)
	{
		this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_RECONNECTING;
		this.bRequestRelogin = false;
	}

	public void No_Delegate(object a_oObject)
	{
		Debug.LogWarning("Quit application");
		NrTSingleton<NrMainSystem>.Instance.QuitGame();
		this.bRequestRelogin = false;
	}

	private void askReconnectionFailed()
	{
		this.bRequestRelogin = true;
		if (!TsPlatform.IsWeb && !TsPlatform.IsEditor && !TsPlatform.IsAndroid && !TsPlatform.IsIPhone)
		{
			this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_RECONNECTING;
			return;
		}
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("162");
		string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("79");
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (TsPlatform.IsWeb || TsPlatform.IsEditor)
		{
			msgBoxUI.SetMsg(new YesDelegate(this.Yes_Delegate), null, new NoDelegate(this.No_Delegate), null, textFromMessageBox, textFromMessageBox2, eMsgType.MB_OK);
			msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("216"));
		}
		else if (TsPlatform.IsIPhone)
		{
			msgBoxUI.SetMsg(new YesDelegate(this.Yes_Delegate), null, textFromMessageBox, textFromMessageBox2, eMsgType.MB_OK);
			msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2083"));
		}
		else
		{
			msgBoxUI.SetMsg(new YesDelegate(this.Yes_Delegate), null, new NoDelegate(this.No_Delegate), null, textFromMessageBox, textFromMessageBox2, eMsgType.MB_OK_CANCEL);
			msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2083"));
			msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("216"));
		}
	}

	public void Update()
	{
		if (NrTSingleton<CMovingServer>.Instance.IsMovingWorld())
		{
			return;
		}
		if (this.m_tickCount + 1000L > PublicMethod.GetTick())
		{
			return;
		}
		this.m_tickCount = PublicMethod.GetTick();
		if (!this.m_bIsActivity)
		{
			return;
		}
		if (TsPlatform.IsMobile)
		{
			if (Scene.CurScene != Scene.Type.SELECTCHAR && Scene.CurScene != Scene.Type.LOGIN)
			{
				if (NrTSingleton<NkClientLogic>.Instance.IsLoginGameServer())
				{
					return;
				}
			}
		}
		if (this.m_eAutoReloginState >= NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_WAITING_ENCRYPTKEY && !BaseNet_Game.GetInstance().IsSocketConnected())
		{
			this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_RECONNECTING;
		}
		switch (this.m_eAutoReloginState)
		{
		case NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_DETECT_DISCONNECT:
			if (BaseNet_Game.GetInstance().IsSocketConnected())
			{
				return;
			}
			if (this.bRequestRelogin)
			{
				return;
			}
			this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_RECONNECTING;
			this.bRequestRelogin = false;
			if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsBackupPersonInfo())
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BackupPersonInfo(@char.GetPersonInfo() as NrPersonInfoUser);
				}
			}
			if (TsPlatform.IsMobile && Scene.IsCurScene(Scene.Type.BATTLE))
			{
				NrTSingleton<NkBattleCharManager>.Instance.DeleteAllChar();
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_RANDOM_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_RARERANDOM_DLG);
			TsLog.LogWarning("[NkAutoRelogin] Disconnected", new object[0]);
			SendPacket.GetInstance().SetBlockSendPacket(true);
			break;
		case NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_RECONNECTING:
			if (this.bRequestRelogin)
			{
				return;
			}
			TsLog.LogWarning("[NkAutoRelogin] trying to connect to server", new object[0]);
			if (TsPlatform.IsMobile && Scene.IsCurScene(Scene.Type.SELECTCHAR))
			{
				MsgHandler.Handle("OnFirstConnectChar", new object[0]);
			}
			SendPacket.GetInstance().SetBlockSendPacket(false);
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WAIT_DLG))
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WAIT_DLG);
			}
			if (BaseNet_Game.GetInstance().ReConnectServer())
			{
				TsLog.LogWarning("[NkAutoRelogin] server connected", new object[0]);
				this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_WAITING_ENCRYPTKEY;
				SendPacket.GetInstance().SetBlockSendPacket(true);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WAIT_DLG);
				TsLog.LogWarning("[NkAutoRelogin] server not connected", new object[0]);
				if (!Scene.IsCurScene(Scene.Type.BATTLE))
				{
					this.askReconnectionFailed();
					SendPacket.GetInstance().SetBlockSendPacket(true);
				}
				else if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_RESULT_DLG) && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_RESULT_PLUNDER_DLG))
				{
					this.askReconnectionFailed();
					SendPacket.GetInstance().SetBlockSendPacket(true);
				}
				else
				{
					this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_RECONNECTING;
					this.bRequestRelogin = false;
					SendPacket.GetInstance().SetBlockSendPacket(true);
				}
			}
			break;
		case NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_TRYING_RELOGIN:
		{
			SendPacket.GetInstance().SetBlockSendPacket(false);
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WAIT_DLG))
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WAIT_DLG);
			}
			WS_USER_RELOGIN_REQ wS_USER_RELOGIN_REQ = new WS_USER_RELOGIN_REQ();
			wS_USER_RELOGIN_REQ.UID = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID;
			wS_USER_RELOGIN_REQ.i64AccountWorldInfoKey = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_i64AccountWorldInfoKey;
			if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
			{
				TKString.StringChar(TsPlatform.Operator.GetMobileDeviceId(), ref wS_USER_RELOGIN_REQ.szDeviceToken);
			}
			else if (TsPlatform.IsWeb)
			{
				TKString.StringChar(string.Empty, ref wS_USER_RELOGIN_REQ.szDeviceToken);
			}
			wS_USER_RELOGIN_REQ.i8HP_AuthRequest = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.HP_AuthRequest;
			SendPacket.GetInstance().SendObject(16777237, wS_USER_RELOGIN_REQ);
			SendPacket.GetInstance().SetBlockSendPacket(true);
			this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_WAITING_RELOGIN;
			TsLog.LogWarning("[NkAutoRelogin] send relogin packet to server", new object[0]);
			break;
		}
		case NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_RELOGINED:
			this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_DETECT_DISCONNECT;
			if (!Scene.IsCurScene(Scene.Type.SELECTCHAR))
			{
				if (!Scene.IsCurScene(Scene.Type.LOGIN))
				{
					GS_CLIENT_RELOGIN_REQ gS_CLIENT_RELOGIN_REQ = new GS_CLIENT_RELOGIN_REQ();
					gS_CLIENT_RELOGIN_REQ.CharState = 0L;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CLIENT_RELOGIN_REQ, gS_CLIENT_RELOGIN_REQ);
				}
			}
			break;
		}
	}

	public void OnReceivedEncryptKey()
	{
		this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_TRYING_RELOGIN;
		TsLog.LogWarning("[NkAutoRelogin] encrypt key received", new object[0]);
	}

	public void OnReloginSuccessed()
	{
		this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_RELOGINED;
		SendPacket.GetInstance().SetBlockSendPacket(false);
		if (this.bRequestConvertID)
		{
		}
		TsLog.LogWarning("[NkAutoRelogin] relogin successed : CurScene = " + Scene.CurScene.ToString(), new object[0]);
	}

	public void OnReloginFailed()
	{
		this.m_eAutoReloginState = NkAutoRelogin.eAUTORELOGIN_STATE.E_AUTORELOGIN_STATE_TRYING_RELOGIN;
		TsLog.LogWarning("[NkAutoRelogin] relogin Failed", new object[0]);
	}
}
