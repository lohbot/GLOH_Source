using GAME;
using Global;
using GooglePlayGames;
using Ndoors.Framework.Stage;
using NPatch;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using PROTOCOL.LOGIN;
using PROTOCOL.WORLD;
using SERVICE;
using System;
using System.Collections.Generic;
using TsBundle;
using TsPatch;
using UnityEngine;
using UnityForms;

namespace GameMessage.Private
{
	public static class FacadeHandler
	{
		private static Dictionary<Scene.Type, AStage> _stageCache;

		static FacadeHandler()
		{
			FacadeHandler._stageCache = new Dictionary<Scene.Type, AStage>();
			MsgHandler.SetMsgHandler(new DefaultHandler());
		}

		public static void Init()
		{
		}

		public static bool EndPreDownload()
		{
			FacadeHandler.MoveStage(Scene.Type.NPATCH_DOWNLOAD);
			return true;
		}

		public static bool EndNPATCHDownLoad(bool bStartButton = false)
		{
			if (bStartButton)
			{
				Mobile_PreDownloadDlg mobile_PreDownloadDlg = (Mobile_PreDownloadDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PREDOWNLOAD_DLG);
				if (mobile_PreDownloadDlg != null)
				{
					mobile_PreDownloadDlg.ShowStartButton(true);
				}
			}
			else
			{
				FacadeHandler.MoveStage(Scene.Type.INITIALIZE);
			}
			return true;
		}

		public static bool Rcv_ServerMoveResetStage()
		{
			NrTSingleton<NkCharManager>.Instance.Init(true);
			StageSystem.ResetStack();
			FacadeHandler.MoveStage(Scene.Type.JUSTWAIT);
			return true;
		}

		public static bool EnableMobileLogin()
		{
			return StageSystem.CurrentStageHandleMessage("EnableMobileLogin", new object[0]);
		}

		public static bool LoginMobileGUI()
		{
			return StageSystem.CurrentStageHandleMessage("LoginMobileGUI", new object[0]);
		}

		public static bool SelectAvatarFirstCharMobile()
		{
			return StageSystem.CurrentStageHandleMessage("SelectChar_FirstCharacter", new object[0]);
		}

		public static bool OnFirstConnectChar()
		{
			bool flag = StageSystem.CurrentStageHandleMessage("OnFirstConnectChar", new object[0]);
			if (flag)
			{
				WS_CONNECT_GAMESERVER_REQ obj = new WS_CONNECT_GAMESERVER_REQ();
				SendPacket.GetInstance().SendObject(16777261, obj);
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("1167"));
			}
			return true;
		}

		public static bool Save_LOGIN_USER_REQ(string strID, string strPassword)
		{
			if (strID == string.Empty)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("82");
				Main_UI_SystemMessage.ADDMessage(textFromNotify);
			}
			if (TsPlatform.IsMobile)
			{
				PlayerPrefs.SetString("UserID", strID);
				PlayerPrefs.SetString("UserPW", strPassword);
			}
			else
			{
				NrTSingleton<NrConfigFile>.Instance.SetData(NrConfigFile.eKey.LoginID.ToString(), strID);
				NrTSingleton<NrConfigFile>.Instance.SetData(NrConfigFile.eKey.LoginPW.ToString(), strPassword);
				NrTSingleton<NrConfigFile>.Instance.SaveData();
			}
			bool result = false;
			try
			{
				result = BaseNet_Login.GetInstance().ConnectLoginServer();
			}
			catch (Exception obj)
			{
				TsLog.LogError(obj);
			}
			return result;
		}

		public static bool LOGIN_USER_REQ()
		{
			LS_LOGIN_USER_REQ lS_LOGIN_USER_REQ = new LS_LOGIN_USER_REQ();
			string input = string.Empty;
			string sourceString = string.Empty;
			if (TsPlatform.IsMobile && Scene.IsCurScene(Scene.Type.LOGIN))
			{
				input = PlayerPrefs.GetString("UserID");
				sourceString = PlayerPrefs.GetString("UserPW");
			}
			else
			{
				input = NrTSingleton<NrConfigFile>.Instance.GetData(NrConfigFile.eKey.LoginID.ToString());
				sourceString = NrTSingleton<NrConfigFile>.Instance.GetData(NrConfigFile.eKey.LoginPW.ToString());
			}
			string src = NkUtil.ConvertStrToByteBase64(input);
			TKString.StringChar(src, ref lS_LOGIN_USER_REQ.szEncAccountID);
			NkUtil.SetSHA1Hash(sourceString, ref lS_LOGIN_USER_REQ.szPassword);
			lS_LOGIN_USER_REQ.nAuthPlatformType = NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType();
			lS_LOGIN_USER_REQ.nPlayerPlatformType = NrTSingleton<NkClientLogic>.Instance.GetPlayerPlatformType();
			lS_LOGIN_USER_REQ.nStoreType = NrTSingleton<NkClientLogic>.Instance.GetStoreType();
			if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
			{
				TKString.StringChar(TsPlatform.Operator.GetMobileDeviceId(), ref lS_LOGIN_USER_REQ.szDeviceID);
			}
			else if (TsPlatform.IsWeb)
			{
				TKString.StringChar(string.Empty, ref lS_LOGIN_USER_REQ.szDeviceID);
			}
			SendPacket.GetInstance().SendObject(2097154, lS_LOGIN_USER_REQ);
			return true;
		}

		public static bool PLATFORM_LOGIN_REQ()
		{
			TsLog.LogWarning("Send~~~PLATFORM_LOGIN_REQ", new object[0]);
			LS_PLATFORM_LOGIN_REQ lS_PLATFORM_LOGIN_REQ = new LS_PLATFORM_LOGIN_REQ();
			string input = string.Empty;
			string text = string.Empty;
			if (NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType() == 2)
			{
				input = NmFacebookManager.instance.UserData.m_ID;
				if (!string.IsNullOrEmpty(NmFacebookManager.instance.UserData.m_Email))
				{
					text = NmFacebookManager.instance.UserData.m_Email;
				}
			}
			else if (NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType() == 6)
			{
				input = PlayGamesPlatform.Instance.localUser.id;
				Debug.Log("@@@@@@@@@GOOGLEPLAY USERID : " + PlayGamesPlatform.Instance.localUser.id);
				text = PlayGamesPlatform.Instance.GetUserEmail();
				Debug.Log("@@@@@@@@@GOOGLEPLAY Email : " + text);
			}
			else if (NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType() != 4)
			{
				if (NrTSingleton<NkClientLogic>.Instance.IsGuestLogin() || NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType() != 5)
				{
					if (NrTSingleton<NkClientLogic>.Instance.IsGuestLogin() && PlayerPrefs.GetInt(NrPrefsKey.GUESTID) == 1 && PlayerPrefs.GetInt(NrPrefsKey.CONVERT_GUESTID) == 0)
					{
						input = "@GT" + TsPlatform.Operator.GetMobileDeviceId();
						NrTSingleton<NkClientLogic>.Instance.SetGuestLogin(true);
					}
				}
			}
			string src = NkUtil.ConvertStrToByteBase64(input);
			TKString.StringChar(src, ref lS_PLATFORM_LOGIN_REQ.szEncPlatformID);
			if (!string.IsNullOrEmpty(text))
			{
				TKString.StringChar(text, ref lS_PLATFORM_LOGIN_REQ.szAccountID);
			}
			TKString.StringChar(TsPlatform.Operator.GetMobileDeviceId(), ref lS_PLATFORM_LOGIN_REQ.szDeviceID);
			lS_PLATFORM_LOGIN_REQ.nAuthPlatformType = NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType();
			lS_PLATFORM_LOGIN_REQ.nPlayerPlatformType = NrTSingleton<NkClientLogic>.Instance.GetPlayerPlatformType();
			lS_PLATFORM_LOGIN_REQ.nStoreType = NrTSingleton<NkClientLogic>.Instance.GetStoreType();
			SendPacket.GetInstance().SendObject(2097162, lS_PLATFORM_LOGIN_REQ);
			return true;
		}

		public static bool Recv_LS_PLATFORM_LOGIN_ACK()
		{
			NrMobileAuthSystem.Instance.Auth.RequestPlatformLogin();
			return true;
		}

		public static bool Rcv_LOGIN_USER_ACK()
		{
			if (TsPlatform.IsWeb)
			{
				FacadeHandler.MoveStage(Scene.Type.INITIALIZE);
			}
			else if (NrTSingleton<NrGlobalReference>.Instance.localWWW)
			{
				FacadeHandler.MoveStage(Scene.Type.INITIALIZE);
			}
			else
			{
				FacadeHandler.MoveStage(Scene.Type.NPATCH_DOWNLOAD);
			}
			return true;
		}

		public static bool DeleteAvatar(long PersonID)
		{
			return StageSystem.CurrentStageHandleMessage("DeleteAvatar", new object[]
			{
				PersonID
			});
		}

		public static bool Req_CLIENT_VERIFY_REQ()
		{
			WS_CLIENT_VERIFY_REQ wS_CLIENT_VERIFY_REQ = new WS_CLIENT_VERIFY_REQ();
			wS_CLIENT_VERIFY_REQ.nPlayerPlatformType = NrTSingleton<NkClientLogic>.Instance.GetPlayerPlatformType();
			int val = int.Parse(NrTSingleton<NrGlobalReference>.Instance.ResourcesVer);
			wS_CLIENT_VERIFY_REQ.nPatchVersion = Math.Max(1001, val);
			if (TsPlatform.IsAndroid)
			{
				string aPP_VERSION_AND = TsPlatform.APP_VERSION_AND;
				int nAPPVersion = int.Parse(aPP_VERSION_AND.Replace(".", string.Empty));
				wS_CLIENT_VERIFY_REQ.nAPPVersion = nAPPVersion;
			}
			else if (TsPlatform.IsIPhone)
			{
				string aPP_VERSION_IOS = TsPlatform.APP_VERSION_IOS;
				int nAPPVersion2 = int.Parse(aPP_VERSION_IOS.Replace(".", string.Empty));
				wS_CLIENT_VERIFY_REQ.nAPPVersion = nAPPVersion2;
			}
			Debug.Log("test 5 m_LoadPatchListVersion = " + PatchFinalList.Instance.Version.ToString());
			TsPlatform.FileLog("test 5 m_LoadPatchListVersion = " + PatchFinalList.Instance.Version.ToString());
			SendPacket.GetInstance().SendObject(16777231, wS_CLIENT_VERIFY_REQ);
			TsLog.LogWarning(string.Concat(new object[]
			{
				"SEND CLIENT VERSION [",
				wS_CLIENT_VERIFY_REQ.nPatchVersion,
				", apk = ",
				wS_CLIENT_VERIFY_REQ.nAPPVersion,
				" ]"
			}), new object[0]);
			return true;
		}

		public static bool Req_CONNECT_GAMESERVER_REQ(long PersonID)
		{
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log("NETLOG==========CONNECT_GAMESERVER_REQ PersonID(" + PersonID.ToString() + ") " + currentServiceArea.ToString(), new object[0]);
			}
			WS_CONNECT_GAMESERVER_REQ wS_CONNECT_GAMESERVER_REQ = new WS_CONNECT_GAMESERVER_REQ();
			wS_CONNECT_GAMESERVER_REQ.PersonID = PersonID;
			wS_CONNECT_GAMESERVER_REQ.ServiceArea = (int)currentServiceArea;
			SendPacket.GetInstance().SendObject(16777261, wS_CONNECT_GAMESERVER_REQ);
			return true;
		}

		public static bool Rcv_WS_CONNECT_GAMESERVER_ACK()
		{
			bool flag = StageSystem.CurrentStageHandleMessage("OnGameServerConnected", new object[0]);
			if (!flag)
			{
				MsgBoxUI msgBoxUI = (MsgBoxUI)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG);
				if (msgBoxUI != null)
				{
					msgBoxUI.SetMsg(new YesDelegate(FacadeHandler._OnMessageBoxOK_QuitGame), null, "경고", "scene 이 올바르지않아......\r\n어플을 재실행해주세요.\n" + StageSystem.GetCurrentStageName(), eMsgType.MB_OK);
				}
			}
			return flag;
		}

		public static void _OnMessageBoxOK_QuitGame(object a_oObject)
		{
			NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
			NrTSingleton<NrMainSystem>.Instance.QuitGame();
		}

		public static bool Req_GS_AUTH_SESSION_REQ(long uid, int skey, long personid, byte mvServer, int nMode)
		{
			TsLog.LogError("!!!!!!!!!!!!!!!!!!!NETLOG==========GS_AUTH_SESSION_REQ", new object[0]);
			GS_AUTH_SESSION_REQ gS_AUTH_SESSION_REQ = new GS_AUTH_SESSION_REQ();
			gS_AUTH_SESSION_REQ.UID = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID;
			gS_AUTH_SESSION_REQ.SessionKey = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_siAuthSessionKey;
			gS_AUTH_SESSION_REQ.PersonID = personid;
			gS_AUTH_SESSION_REQ.nMode = nMode;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUTH_SESSION_REQ, gS_AUTH_SESSION_REQ);
			return true;
		}

		public static bool Rcv_GoToPrepareGameStage()
		{
			if (!Scene.IsCurScene(Scene.Type.PREPAREGAME))
			{
				FacadeHandler.MoveStage(Scene.Type.PREPAREGAME);
			}
			return true;
		}

		public static bool CharacterSelect(NrCharUser pkChar, bool bConnectServer)
		{
			return StageSystem.CurrentStageHandleMessage("CharacterSelect", new object[]
			{
				pkChar,
				bConnectServer
			});
		}

		public static bool SetCreateCharPartInfo(bool bCustom2Create, bool bCreate2Custom)
		{
			return StageSystem.CurrentStageHandleMessage("SetCreateCharPartInfo", new object[]
			{
				bCustom2Create,
				bCreate2Custom
			});
		}

		public static bool SetCreateChar(E_CHAR_TRIBE _tribe)
		{
			return StageSystem.CurrentStageHandleMessage("SetCreateChar", new object[]
			{
				_tribe
			});
		}

		public static bool SetSelectStep(E_CHAR_SELECT_STEP _step)
		{
			return StageSystem.CurrentStageHandleMessage("SetSelectStep", new object[]
			{
				_step
			});
		}

		public static void NotifyUnityVersion()
		{
		}

		public static bool Rcv_BATTLE_RESULT()
		{
			bool flag = StageSystem.CurrentStageHandleMessage("GameResult", new object[0]);
			if (!flag && NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.LogWarning(StageSystem.GetCurrentStageName() + ".BATTLE_RESULT Handler not found ###", new object[0]);
			}
			return flag;
		}

		public static bool SendFacebookID(string strFacebookID)
		{
			string src = NkUtil.ConvertStrToByteBase64(strFacebookID);
			WS_PLATFORMID_SET_REQ wS_PLATFORMID_SET_REQ = new WS_PLATFORMID_SET_REQ();
			TKString.StringChar(src, ref wS_PLATFORMID_SET_REQ.m_szPlatformID);
			wS_PLATFORMID_SET_REQ.m_nPlatformType = 2;
			SendPacket.GetInstance().SendObject(16777273, wS_PLATFORMID_SET_REQ);
			TsLog.LogWarning("!!!!!!!!!!!SendFacebookID = {0}", new object[]
			{
				strFacebookID
			});
			return true;
		}

		public static bool RequestLogin()
		{
			if (Scene.CurScene > Scene.Type.LOGIN)
			{
				return true;
			}
			int loginServerPort = Client.GetLoginServerPort();
			BaseNet_Login.GetInstance().SetServerIPandPort(NrGlobalReference.strLoginServerIP, loginServerPort);
			bool flag = false;
			try
			{
				flag = BaseNet_Login.GetInstance().ConnectLoginServer();
			}
			catch (Exception obj)
			{
				TsLog.LogError(obj);
			}
			if (!flag)
			{
				MsgBoxUI msgBoxUI = (MsgBoxUI)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG);
				if (msgBoxUI != null)
				{
					msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"));
					msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("9"));
					msgBoxUI.SetMsg(new YesDelegate(FacadeHandler._OnMessageBoxOK_Relogin), null, new NoDelegate(FacadeHandler._OnMessageBoxCancle_Relogin), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("2"), NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("3"), eMsgType.MB_OK_CANCEL);
				}
			}
			NrTSingleton<NkClientLogic>.Instance.SetLoginGameServer(true);
			Debug.LogWarning("========== FacadeHandler.RequestLogin : SetLoginGameServer true ----- ");
			return false;
		}

		public static bool RequestAutoLogin()
		{
			if (Scene.CurScene > Scene.Type.LOGIN)
			{
				return true;
			}
			NrMobileAuthSystem.Instance.Auth.RequestLogin();
			return true;
		}

		public static bool RequestFacebookFriendInvite(string srtFacebookID, string strFacebookName)
		{
			GS_FRIEND_APPLY_REQ gS_FRIEND_APPLY_REQ = new GS_FRIEND_APPLY_REQ();
			gS_FRIEND_APPLY_REQ.ui8ApplyType = 1;
			gS_FRIEND_APPLY_REQ.i32WorldID = 0;
			TKString.StringChar(srtFacebookID, ref gS_FRIEND_APPLY_REQ.FaceBookID);
			TKString.StringChar(strFacebookName, ref gS_FRIEND_APPLY_REQ.PlatformFriendName);
			TKString.StringChar(NmFacebookManager.instance.UserData.m_Name, ref gS_FRIEND_APPLY_REQ.PlatformMyName);
			SendPacket.GetInstance().SendObject(904, gS_FRIEND_APPLY_REQ);
			TsLog.LogWarning("RequestFacebookFriendInvite ID = {0} packet id = {1}, Time = {2}", new object[]
			{
				srtFacebookID,
				gS_FRIEND_APPLY_REQ.FaceBookID[0],
				Time.realtimeSinceStartup
			});
			return true;
		}

		public static bool FacebookFriendInviteDlgShow()
		{
			if (TsPlatform.IsBand)
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BAND_FRIENDS_DLG);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.FACEBOOK_FRIEND_INVITE);
			}
			return true;
		}

		public static bool UpdateFriendInviteDlgShow(Texture2D pTexture, object obj)
		{
			return true;
		}

		public static bool UpdatePlatformEvent()
		{
			return true;
		}

		public static bool FriendInviteDlgFriendShow(string Key)
		{
			return true;
		}

		public static bool FacebookFriendDataArrage()
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo != null)
			{
				List<string> list = new List<string>();
				foreach (FacebookUserData facebookUserData in NmFacebookManager.instance.FriendDataGetValue())
				{
					if (kMyCharInfo.m_kFriendInfo.GetFriendPersonIDByFacebookID(facebookUserData.m_ID) != 0L)
					{
						TsLog.LogWarning("FacebookFriendDataArrage ADD Remove ID ={0} Name = {1}", new object[]
						{
							facebookUserData.m_ID,
							facebookUserData.m_Name
						});
						list.Add(facebookUserData.m_ID);
					}
				}
				if (list.Count != 0)
				{
					int num = 0;
					GS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ gS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ = new GS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ();
					for (int i = 0; i < 30; i++)
					{
						gS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ.FriendNames[i] = new PLATFORM_FRIEND_NAME();
					}
					for (int j = 0; j < list.Count; j++)
					{
						string friendPlatformNameByFacebookID = kMyCharInfo.m_kFriendInfo.GetFriendPlatformNameByFacebookID(list[j]);
						if (string.IsNullOrEmpty(friendPlatformNameByFacebookID) && NmFacebookManager.instance.FriendsData.ContainsKey(list[j]))
						{
							gS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ.FriendNames[num].nPersonID = kMyCharInfo.m_kFriendInfo.GetFriendPersonIDByFacebookID(list[j]);
							TKString.StringChar(NmFacebookManager.instance.FriendsData[list[j]].m_Name, ref gS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ.FriendNames[num].szPlatforName);
							USER_FRIEND_INFO friend = kMyCharInfo.m_kFriendInfo.GetFriend(gS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ.FriendNames[num].nPersonID);
							TKString.CharChar(gS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ.FriendNames[num].szPlatforName, ref friend.szPlatformName);
							kMyCharInfo.m_kFriendInfo.UpdateFriend(friend);
							num++;
						}
						NmFacebookManager.instance.FriendsData.Remove(list[j]);
					}
					if (num > 0)
					{
						gS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ.nMyPersonID = kMyCharInfo.m_PersonID;
						TKString.StringChar(NmFacebookManager.instance.UserData.m_Name, ref gS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ.MyCharName);
						gS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ.byCount = (byte)num;
						SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ, gS_PLATFORM_FRIENDINFO_NAME_UPDATE_REQ);
					}
				}
			}
			return true;
		}

		public static bool BandMamberDataArrage()
		{
			return true;
		}

		public static bool ShowSystemMessage(string Message)
		{
			Main_UI_SystemMessage.ADDMessage(Message);
			return true;
		}

		public static bool PlatFormInviteFriend()
		{
			string empty = string.Empty;
			WS_PLATFORM_INVITE_FRIENDS_REQ wS_PLATFORM_INVITE_FRIENDS_REQ = new WS_PLATFORM_INVITE_FRIENDS_REQ();
			if (!string.IsNullOrEmpty(empty))
			{
				TKString.StringChar(empty, ref wS_PLATFORM_INVITE_FRIENDS_REQ.Friend_Key);
				SendPacket.GetInstance().SendObject(16777291, wS_PLATFORM_INVITE_FRIENDS_REQ);
				TsLog.LogError("Send PlatFormInviteFriend={0}", new object[]
				{
					empty
				});
			}
			return true;
		}

		public static bool SendInviteMessage()
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("302"),
				"charname",
				empty2
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			return true;
		}

		public static bool FriendInviteFailed(int nCode)
		{
			switch (nCode + 17)
			{
			case 0:
			case 1:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("668"));
				return true;
			case 2:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("670"));
				return true;
			case 3:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("669"));
				return true;
			case 4:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("671"));
				return true;
			case 5:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("672"));
				return true;
			case 6:
				goto IL_128;
			case 7:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("673"));
				return true;
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
				IL_4B:
				switch (nCode)
				{
				case 8:
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("674"));
					return true;
				case 9:
					IL_5F:
					switch (nCode)
					{
					case 1001:
					case 1002:
						break;
					case 1003:
						goto IL_1A5;
					default:
						switch (nCode)
						{
						case 60100:
							goto IL_128;
						case 60101:
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("636"));
							return true;
						case 60102:
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("638"));
							return true;
						default:
							if (nCode != -32)
							{
								if (nCode == -31)
								{
									goto IL_1A5;
								}
								if (nCode == -9798)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("676"));
									return true;
								}
								if (nCode == -1000)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("677"));
									return true;
								}
								if (nCode == -500)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("678"));
									return true;
								}
								if (nCode == -451)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("679"));
									return true;
								}
								if (nCode == -400)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("680"));
									return true;
								}
								if (nCode == -200)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("681"));
									return true;
								}
								if (nCode == -100)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("682"));
									return true;
								}
								if (nCode == 3000)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("629"));
									return true;
								}
								if (nCode == 60200)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("637"));
									return true;
								}
								if (nCode != 60300)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("629"));
									return true;
								}
								Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("640"));
								return true;
							}
							break;
						}
						break;
					}
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("639"));
					return true;
					IL_1A5:
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("624"));
					return true;
				case 10:
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("675"));
					return true;
				}
				goto IL_5F;
			case 15:
				return true;
			}
			goto IL_4B;
			IL_128:
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("635"));
			return true;
		}

		public static bool FacebookLoginFailed(string error)
		{
			if (NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
			{
				return true;
			}
			Main_UI_SystemMessage.ADDMessage(string.Format("Login Failed  {0}", error));
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("146"));
			Login_Select_PlatformDLG login_Select_PlatformDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGIN_SELECT_PLATFORM_DLG) as Login_Select_PlatformDLG;
			if (login_Select_PlatformDLG != null)
			{
				login_Select_PlatformDLG.Show();
			}
			return true;
		}

		public static bool InviteRewardFailed()
		{
			long personID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID;
			PlayerPrefs.SetInt(string.Format("{0}{1}", NrPrefsKey.KAKAO_REWARD_SET, personID), 1);
			return true;
		}

		public static bool InviteRewardRequest()
		{
			GS_INVITE_REWARD_REQ gS_INVITE_REWARD_REQ = default(GS_INVITE_REWARD_REQ);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INVITE_REWARD_REQ, gS_INVITE_REWARD_REQ);
			return true;
		}

		public static bool LoginFailed(string error)
		{
			if (NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
			{
				return true;
			}
			MsgBoxUI msgBoxUI = (MsgBoxUI)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG);
			if (msgBoxUI != null && !NrTSingleton<NrMainSystem>.Instance.m_bIsAutoLogin)
			{
				if (error == "ReLogin")
				{
					error = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("Message3");
				}
				msgBoxUI.SetMsg(null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("LOGIN"), error, eMsgType.MB_OK);
			}
			NrTSingleton<NrMainSystem>.Instance.m_bIsAutoLogin = false;
			Login_Select_PlatformDLG login_Select_PlatformDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGIN_SELECT_PLATFORM_DLG) as Login_Select_PlatformDLG;
			if (login_Select_PlatformDLG != null)
			{
				login_Select_PlatformDLG.Show();
			}
			return true;
		}

		public static bool RequestWorldLogin(string ip, int port)
		{
			if (!BaseNet_Game.GetInstance().IsSocketConnected() && !BaseNet_Game.GetInstance().ConnectGameServer(ip, port))
			{
				Debug.LogWarning("connect to world server error.");
				return false;
			}
			WS_USER_LOGIN_REQ wS_USER_LOGIN_REQ = new WS_USER_LOGIN_REQ();
			wS_USER_LOGIN_REQ.nAuthPlatformType = NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType();
			wS_USER_LOGIN_REQ.nPlayerPlatformType = NrTSingleton<NkClientLogic>.Instance.GetPlayerPlatformType();
			wS_USER_LOGIN_REQ.nStoreType = NrTSingleton<NkClientLogic>.Instance.GetStoreType();
			wS_USER_LOGIN_REQ.nClientPatchLevel = (byte)Launcher.Instance.LocalPatchLevel;
			TKString.StringChar(NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey, ref wS_USER_LOGIN_REQ.szAuthKey);
			Debug.LogWarning(string.Concat(new object[]
			{
				"AUTH KEY : ",
				NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey,
				", time : ",
				Time.time
			}));
			wS_USER_LOGIN_REQ.nSerialNumber = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nSerialNumber;
			if (wS_USER_LOGIN_REQ.nAuthPlatformType == 2)
			{
				TKString.StringChar(NmFacebookManager.instance.UserData.m_ID, ref wS_USER_LOGIN_REQ.szAccountID);
				Debug.LogWarning(string.Concat(new object[]
				{
					"AccountID : ",
					NmFacebookManager.instance.UserData.m_ID,
					", time : ",
					Time.time
				}));
			}
			else if (wS_USER_LOGIN_REQ.nAuthPlatformType != 4)
			{
				if (wS_USER_LOGIN_REQ.nAuthPlatformType != 8)
				{
					TKString.StringChar(wS_USER_LOGIN_REQ.nSerialNumber.ToString(), ref wS_USER_LOGIN_REQ.szAccountID);
					Debug.LogWarning(string.Concat(new object[]
					{
						"AccountID : ",
						wS_USER_LOGIN_REQ.nSerialNumber.ToString(),
						", time : ",
						Time.time
					}));
				}
			}
			if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
			{
				TKString.StringChar(TsPlatform.Operator.GetMobileDeviceId(), ref wS_USER_LOGIN_REQ.szDeviceToken);
			}
			else if (TsPlatform.IsWeb)
			{
				TKString.StringChar(string.Empty, ref wS_USER_LOGIN_REQ.szDeviceToken);
			}
			SendPacket.GetInstance().SendObject(16777233, wS_USER_LOGIN_REQ);
			Debug.LogWarning("try login to world server.");
			return true;
		}

		public static void RequestMemberSecession()
		{
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(FacadeHandler.MsgBoxOKEvent), null, null, null, textFromMessageBox, NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("301"), eMsgType.MB_OK);
		}

		public static void MsgBoxOKEvent(object EventObject)
		{
			FacadeHandler.QuitGame();
		}

		public static bool UnregisterKakao()
		{
			return true;
		}

		public static bool DeleteAuthInfo()
		{
			NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
			return true;
		}

		public static bool QuitGame()
		{
			NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
			NrTSingleton<NrMainSystem>.Instance.QuitGame();
			return true;
		}

		public static bool QuitAPP()
		{
			if (Scene.CurScene == Scene.Type.LOGIN)
			{
				Login_Select_PlatformDLG login_Select_PlatformDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGIN_SELECT_PLATFORM_DLG) as Login_Select_PlatformDLG;
				if (login_Select_PlatformDLG != null)
				{
					login_Select_PlatformDLG.Show();
				}
			}
			else
			{
				NrTSingleton<NrMainSystem>.Instance.QuitGame();
			}
			return true;
		}

		private static void _OnMessageBoxOK_Relogin(object a_oObject)
		{
			FacadeHandler.RequestLogin();
		}

		private static void _OnMessageBoxCancle_Relogin(object a_oObject)
		{
			FacadeHandler.ShowPlatformLogin();
		}

		public static bool ShowTerm()
		{
			NrTSingleton<NrMainSystem>.Instance.ShowTerm = true;
			return true;
		}

		public static bool ShowPlatformLogin()
		{
			if (Scene.CurScene == Scene.Type.LOGIN)
			{
				Login_Select_PlatformDLG login_Select_PlatformDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGIN_SELECT_PLATFORM_DLG) as Login_Select_PlatformDLG;
				if (login_Select_PlatformDLG != null)
				{
					login_Select_PlatformDLG.Show();
				}
			}
			return true;
		}

		public static bool RequestPlatformLogin()
		{
			Debug.Log("RequestPlatformLogin");
			Login_Select_PlatformDLG login_Select_PlatformDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGIN_SELECT_PLATFORM_DLG) as Login_Select_PlatformDLG;
			if (login_Select_PlatformDLG != null)
			{
				login_Select_PlatformDLG.PlatformLogin();
			}
			return true;
		}

		public static bool SetPlacement(string strPlacement)
		{
			NrTSingleton<FiveRocksEventManager>.Instance.Placement(strPlacement);
			return true;
		}

		public static bool InternetConnnetErrorMessage()
		{
			FacadeHandler.ShowSystemMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("44"));
			return true;
		}

		public static bool PlatformLoginComplete()
		{
			TsPlatform.Operator.PlatformLoginComplete();
			return true;
		}

		public static bool ClearStageStack()
		{
			StageSystem.ResetStack();
			return true;
		}

		public static bool InsertPush(Scene.Type stype)
		{
			AStage astg = FacadeHandler._CreateStage(stype);
			StageSystem.InsertPush(astg);
			return true;
		}

		public static bool MoveStage(Scene.Type stype)
		{
			AStage aStage = FacadeHandler._CreateStage(stype);
			if (aStage == null)
			{
				TsLog.LogError("StageSystem.MoveStage(null) null parameter!", new object[0]);
			}
			else
			{
				StageSystem.ReserveMoveStage(aStage);
			}
			return true;
		}

		public static bool PushStage(Scene.Type stype)
		{
			AStage iStg = FacadeHandler._CreateStage(stype);
			if (StageSystem.IsReservedMoveStage)
			{
				TsLog.LogWarning("StageSystem PushStage Multiple Request! Please Debug!!!!!!", new object[0]);
				return false;
			}
			StageSystem.ReservePushStage(iStg);
			return true;
		}

		public static bool PopStage()
		{
			StageSystem.ReservePopStage();
			return true;
		}

		public static bool ReloadCurrentStage()
		{
			if (StageSystem.IsReservedMoveStage)
			{
				TsLog.LogWarning("StageSystem ReloadCurrentStage Multiple Request! Please Debug!!!!!!", new object[0]);
			}
			StageSystem.ReloadStage();
			TsLog.Log("StageSystem.ReloadStage - " + StageSystem.GetCurrentStageName() + " : " + StageSystem.GetCurrentStageCoTaskCount().ToString(), new object[0]);
			return true;
		}

		public static bool GoToErrorStage(Scene.Type stype, string logMessage)
		{
			StageSystem.ReserveMoveStage(new StageError(logMessage));
			return true;
		}

		private static bool GotoBattleReserve()
		{
			return StageSystem.CurrentStageHandleMessage("GotoBattleReserve", new object[0]);
		}

		private static AStage _CreateStage(Scene.Type stype)
		{
			AStage aStage = null;
			if (!FacadeHandler._stageCache.TryGetValue(stype, out aStage))
			{
				switch (stype)
				{
				case Scene.Type.SYSCHECK:
					aStage = new StageSystemCheck();
					break;
				case Scene.Type.PREDOWNLOAD:
					aStage = new StagePreDownloadMobile();
					break;
				case Scene.Type.NPATCH_DOWNLOAD:
					aStage = new StageNPatchLauncher();
					break;
				case Scene.Type.LOGIN:
					aStage = new StageLoginMobile();
					break;
				case Scene.Type.INITIALIZE:
					aStage = new StageInitialize();
					break;
				case Scene.Type.SELECTCHAR:
					aStage = new StageSelectCharMobile();
					break;
				case Scene.Type.PREPAREGAME:
					aStage = new StagePrepareGame();
					break;
				case Scene.Type.JUSTWAIT:
					aStage = new StageJustWait();
					break;
				case Scene.Type.WORLD:
					aStage = new StageWorld();
					break;
				case Scene.Type.BATTLE:
					aStage = new StageBattle();
					break;
				case Scene.Type.CUTSCENE:
					aStage = new StageCutScene();
					break;
				case Scene.Type.SOLDIER_BATCH:
					aStage = new Stage_SoldierBatch();
					break;
				}
				if (aStage != null)
				{
					FacadeHandler._stageCache.Add(stype, aStage);
				}
			}
			return aStage;
		}

		public static bool ShowDropDownDLG(DropDownList list)
		{
			MobileDropDownDlg mobileDropDownDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MOBILE_DROPDOWN_DLG) as MobileDropDownDlg;
			if (mobileDropDownDlg != null)
			{
				mobileDropDownDlg.SetData(list);
			}
			return true;
		}

		public static bool ConvertGuestID(bool facebook)
		{
			if (facebook)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2284"));
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2283"));
				NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
			}
			PlayerPrefs.SetInt(NrPrefsKey.CONVERT_GUESTID, 1);
			NrTSingleton<NkClientLogic>.Instance.SetGuestLogin(false);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.GUESTID_COMBINE_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CONVER_PLATFORMID_DLG);
			return true;
		}

		public static bool MuteSound(bool bMuteBGM, bool bMuteEffect, bool flag)
		{
			if (flag)
			{
				if (!bMuteBGM)
				{
					TsAudio.SetMuteAudioType(EAudioType.BGM, true);
					TsAudio.RefreshMuteAudio(EAudioType.BGM);
				}
				if (!bMuteEffect)
				{
					TsAudio.SetMuteAudioType(EAudioType.SFX, true);
					TsAudio.RefreshMuteAudio(EAudioType.SFX);
					TsAudio.SetMuteAudioType(EAudioType.AMBIENT, true);
					TsAudio.RefreshMuteAudio(EAudioType.AMBIENT);
					TsAudio.SetMuteAudioType(EAudioType.VOICE, true);
					TsAudio.RefreshMuteAudio(EAudioType.VOICE);
				}
			}
			else
			{
				if (!bMuteBGM)
				{
					TsAudio.SetMuteAudioType(EAudioType.BGM, false);
					TsAudio.RefreshMuteAudio(EAudioType.BGM);
				}
				if (!bMuteEffect)
				{
					TsAudio.SetMuteAudioType(EAudioType.SFX, false);
					TsAudio.RefreshMuteAudio(EAudioType.SFX);
					TsAudio.SetMuteAudioType(EAudioType.AMBIENT, false);
					TsAudio.RefreshMuteAudio(EAudioType.AMBIENT);
					TsAudio.SetMuteAudioType(EAudioType.VOICE, false);
					TsAudio.RefreshMuteAudio(EAudioType.VOICE);
				}
			}
			return true;
		}

		public static bool MuteMiniDramaSound(bool bMuteEffect, bool flag)
		{
			return true;
		}

		public static bool ButtonSound()
		{
			TsAudioManager.Container.RequestAudioClip("UI_COMMON", "BUTTON", "OK", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			return true;
		}

		public static bool ListSound()
		{
			TsAudioManager.Container.RequestAudioClip("UI_COMMON", "LIST", "SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedForSystem));
			return true;
		}

		public static bool CheckBoxSound()
		{
			TsAudioManager.Container.RequestAudioClip("UI_COMMON", "CHECKBOX", "SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			return true;
		}

		public static bool ToolbarSound()
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "TAP", "CLICK", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			return true;
		}

		public static bool CloseToolTip()
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_SECOND_DLG);
			return true;
		}

		public static bool DLG_ItemTooltipDlg_Second(G_ID c_eWindowID, object item)
		{
			ItemTooltipDlg_Second itemTooltipDlg_Second = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_SECOND_DLG) as ItemTooltipDlg_Second;
			if (itemTooltipDlg_Second != null)
			{
				itemTooltipDlg_Second.Set_Tooltip(c_eWindowID, (ITEM)item, true, Vector3.zero, 0L);
			}
			return true;
		}

		public static string UIBundleStackName()
		{
			return NkBundleCallBack.UIBundleStackName;
		}

		public static string GetTextColor(string color)
		{
			return NrTSingleton<CTextParser>.Instance.GetTextColor(color);
		}

		public static string GetItemName(ITEM item)
		{
			return NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(item);
		}

		public static string ReservedWordManagerIsUse()
		{
			if (NrTSingleton<ReservedWordManager>.Instance.IsUse())
			{
				return "true";
			}
			return "false";
		}

		public static string ReservedWordManagerReplaceWord(string temp)
		{
			return NrTSingleton<ReservedWordManager>.Instance.ReplaceWord(temp);
		}

		public static UIListItemContainer EmoticonInfoParseEmoticon(string str, float lineWidth, float fontSize, SpriteText.Font_Effect fontEffect, ITEM linkItem, bool bNo)
		{
			return EmoticonInfo.ParseEmoticon(str, lineWidth, fontSize, fontEffect, linkItem, true);
		}

		public static short GetLegendType(int kind, int grade)
		{
			return NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(kind, grade);
		}

		public static string GetLegendTypeToString(int kind, int grade)
		{
			return FacadeHandler.GetLegendType(kind, grade).ToString();
		}

		public static UIBaseInfoLoader GetSolGradeImg(int kind, int grade)
		{
			return NrTSingleton<NrCharKindInfoManager>.Instance.GetSolGradeImg(kind, grade);
		}

		public static string PortraitFileName(int kind, int solgrade)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(kind);
			if (charKindInfo == null)
			{
				return string.Empty;
			}
			return charKindInfo.GetPortraitFile1(solgrade);
		}

		public static string GetTextFrom(string group, string index)
		{
			return NrTSingleton<NrTextMgr>.Instance.GetTextFrom(group, index);
		}

		public static string IsReincarnation()
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsReincarnation())
			{
				return "true";
			}
			return "false";
		}

		public static string CharKindIsATB(int kind, long atb)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(kind);
			if (charKindInfo == null)
			{
				return "false";
			}
			if (charKindInfo.IsATB(atb))
			{
				return "true";
			}
			return "false";
		}

		public static UIBaseInfoLoader GetBattleSkillIconTexture(int unique)
		{
			return NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(unique);
		}

		public static string GetUseMinLevel(ITEM item)
		{
			return NrTSingleton<ItemManager>.Instance.GetUseMinLevel(item).ToString();
		}

		public static string IsRank(int uniuqe)
		{
			if (Protocol_Item.Is_Rank(uniuqe))
			{
				return "true";
			}
			return "false";
		}

		public static string RankStateString(int rank)
		{
			return ItemManager.RankStateString(rank);
		}

		public static string ChangeRankToString(int rank)
		{
			return ItemManager.ChangeRankToString(rank);
		}

		public static string GetTextFromInterface(string str)
		{
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(str);
		}

		public static bool DoLinkText(LinkText.TYPE linkTextType, string strText, string strTextKey, object objData)
		{
			NrLinkTextProcessor.LinkTextProcessor(linkTextType, strText, strTextKey, objData);
			return true;
		}

		public static bool DoLinkTextRightClick(LinkText.TYPE linkTextType, string strText, string strTextKey, object objData)
		{
			NrLinkTextProcessor.LinkTextProcessorRightClick(linkTextType, strText, strTextKey, objData);
			return true;
		}

		public static UIBaseInfoLoader GetItemTexture(int unique)
		{
			return NrTSingleton<ItemManager>.Instance.GetItemTexture(unique);
		}

		public static bool SetMsgBox(YesDelegate a_deYes, object a_oObject, string title, string message, eMsgType type)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI == null)
			{
				return false;
			}
			msgBoxUI.SetMsg(a_deYes, a_oObject, title, message, type);
			msgBoxUI.Show();
			return true;
		}

		public static string EffectFileName(string key)
		{
			return NkEffectManager.FileName(key);
		}

		public static bool SetEditorShaderConvert(GameObject prefab)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(prefab);
			return true;
		}

		public static bool SetAllChildLayer(GameObject obj, int layer)
		{
			NkUtil.SetAllChildLayer(obj, layer);
			return true;
		}

		public static bool IsNPCTalkState()
		{
			return NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState();
		}

		public static bool Main_UI_Show()
		{
			if (Scene.CurScene == Scene.Type.WORLD)
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.MYCHARINFO_DLG);
				if (NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType() == 6)
				{
					NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.GOOGLEPLAY_DLG);
				}
				else if (StageLoginMobile.m_bConnectGameCenter)
				{
					NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.GOOGLEPLAY_DLG);
				}
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo.ColosseumMatching)
				{
					ColosseumNoticeDlg colosseumNoticeDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMNOTICE_DLG) as ColosseumNoticeDlg;
					if (colosseumNoticeDlg != null)
					{
						colosseumNoticeDlg.Show();
					}
				}
			}
			Protocol_Supply.Supply_Buff_Show();
			NrTSingleton<WhisperManager>.Instance.ShowWhisperDlg();
			NrTSingleton<NkQuestManager>.Instance.ShowMsg();
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser != null && nrCharUser.GetFollowCharPersonID() > 0L)
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_FOLLOWCHAR);
			}
			NrTSingleton<NkIndunManager>.Instance.Main_UI_Show();
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.ReMove();
			}
			return true;
		}

		public static bool VisibleDropDownList(IUIObject obj)
		{
			DropDownList dropDownList = obj as DropDownList;
			if (null != dropDownList)
			{
				dropDownList.SetVisible(true);
				dropDownList.SetHideList();
			}
			return true;
		}

		public static bool ChangeZ(IUIObject obj, float value)
		{
			DropDownList dropDownList = obj as DropDownList;
			if (null != dropDownList)
			{
				dropDownList.SetLocationZ(dropDownList.GetLocation().z + value);
			}
			ListBox listBox = obj as ListBox;
			if (null != listBox)
			{
				listBox.SetLocationZ(listBox.GetLocation().z + value);
			}
			NewListBox newListBox = obj as NewListBox;
			if (null != newListBox)
			{
				newListBox.SetLocationZ(newListBox.GetLocation().z + value);
			}
			return true;
		}

		public static bool ParseEmoticonFlashLabel(UIListItemContainer container, string str, float lineWidth, float height, int fontSize, float lineSpacing, SpriteText.Anchor_Pos anchor, string mainColor)
		{
			return EmoticonInfo.ParseEmoticonFlashLabel(ref container, str, lineWidth, height, fontSize, lineSpacing, anchor, mainColor);
		}

		public static Form CreateForm(G_ID windowID)
		{
			if (NrTSingleton<EventTriggerMiniDrama>.Instance.ShowTime)
			{
				return null;
			}
			Form result = null;
			switch (windowID)
			{
			case G_ID.BOOKMARK_DLG:
				result = new BookmarkDlg();
				break;
			case G_ID.MENUICON_DLG:
				result = new MenuIconDlg();
				break;
			case G_ID.MYCHARINFO_DLG:
				result = new MyCharInfoDlg();
				break;
			case G_ID.GOOGLEPLAY_DLG:
				result = new GooglePlayDlg();
				break;
			case G_ID.MAINMENU_DLG:
				result = new MainMenuDlg();
				break;
			case G_ID.MAIN_UI_LEVELUP_ALARM_MONARCH:
				result = new Main_UI_LevelUpAlarmMonarch();
				break;
			case G_ID.MAIN_UI_LEVELUP_ALARM_SOLDIER:
				result = new Main_UI_LevelUpAlarmSoldier();
				break;
			case G_ID.MAIN_UI_AUTO_MOVE:
				result = new AutoMove_DLG();
				break;
			case G_ID.MAIN_UI_ICON:
				result = new NoticeIconDlg();
				break;
			case G_ID.TOASTMSG_DLG:
				result = new ToastMsgDlg();
				break;
			case G_ID.MAIN_EXPBOOSTER_DLG:
				result = new ExpBoosterDlg();
				break;
			case G_ID.GAMEGUIDE_DLG:
				result = new GameGuideDlg();
				break;
			case G_ID.BUBBLEGAMEGUIDE_DLG:
				result = new BubbleGameGuideDlg();
				break;
			case G_ID.BATTLE_TUTORIAL_DLG:
				result = new BattleTutorialDlg();
				break;
			case G_ID.GET_ITEM_DLG:
				result = new GetItemDlg();
				break;
			case G_ID.ITEMCOMPOSE_DLG:
				result = new ItemComposeDlg();
				break;
			case G_ID.ITEMSELL_DLG:
				result = new ItemSellDlg();
				break;
			case G_ID.WORLD_MAP:
				result = new WorldMapDlg();
				break;
			case G_ID.DAILYDUNGEON_MAIN:
				result = new DailyDungeon_Main_Dlg();
				break;
			case G_ID.DAILYDUNGEON_DIFFICULTY:
				result = new DailyDungeon_Difficulty_Dlg();
				break;
			case G_ID.BATTLE_HP_GROUP_DLG:
				result = new Battle_HpDlg();
				break;
			case G_ID.BATTLE_RESULT_DLG:
				result = new Battle_ResultDlg();
				break;
			case G_ID.BATTLE_RESULT_CONTENT_DLG:
				result = new Battle_ResultDlg_Content();
				break;
			case G_ID.BATTLE_RESULT_PLUNDER_DLG:
				result = new Battle_ResultPlunderDlg();
				break;
			case G_ID.BATTLE_RESULT_PLUNDER_CONTENT_DLG:
				result = new Battle_ResultPlunderDlg_Content();
				break;
			case G_ID.BATTLE_RESULT_MINE_DLG:
				result = new Battle_ResultMineDlg();
				break;
			case G_ID.BATTLE_RESULT_TUTORIAL_DLG:
				result = new Battle_ResultTutorialDlg();
				break;
			case G_ID.BATTLE_CONTROL_DLG:
				result = new Battle_Control_Dlg();
				break;
			case G_ID.BATTLE_SKILL_DIRECTION_DLG:
				result = new Battle_Skill_Direction_Dlg();
				break;
			case G_ID.BATTLE_SKILL_NAME_DLG:
				result = new Battle_Skill_Name_Dlg();
				break;
			case G_ID.BATTLE_CHARINFO_DLG:
				result = new Battle_CharinfoDlg();
				break;
			case G_ID.BATTLE_COUNT_DLG:
				result = new Battle_CountDlg();
				break;
			case G_ID.BATTLE_SWAPMODE_DLG:
				result = new Battle_SwapModeDlg();
				break;
			case G_ID.BATTLE_HEADUP_TALK:
				result = new Battle_HeadUpTalk();
				break;
			case G_ID.BATTLE_TALK_DLG:
				result = new Battle_TalkDlg();
				break;
			case G_ID.BATTLE_FIGHT_LIST_DLG:
				result = new Battle_Fight_ListDlg();
				break;
			case G_ID.BATTLE_SHAREREPLAY_DLG:
				result = new Battle_ShareReplayDlg();
				break;
			case G_ID.BATTLE_REPLAY_DLG:
				result = new Battle_ReplayDlg();
				break;
			case G_ID.BATTLE_REPLAY_LIST_DLG:
				result = new Battle_ReplayListDlg();
				break;
			case G_ID.BATTLE_SKILLINFO_DLG:
				result = new Battle_SkillInfoDlg();
				break;
			case G_ID.BATTLE_BOSSAGGRO_DLG:
				result = new Battle_BossAggro_DLG();
				break;
			case G_ID.BATTLE_COLOSSEUM_CHARINFO_DLG:
				result = new Battle_Colossum_CharinfoDlg();
				break;
			case G_ID.BATTLE_EMERGENCY_CALL_DLG:
				result = new Battle_Emergency_CallDlg();
				break;
			case G_ID.BATTLE_EMERGENCT_SELECT_DLG:
				result = new Battle_Emergency_SelectDlg();
				break;
			case G_ID.BATTLE_BABEL_CHARINFO_DLG:
				result = new Battle_Babel_CharinfoDlg();
				break;
			case G_ID.BATTLE_EMOTICON_DLG:
				result = new Battle_EmoticonDlg();
				break;
			case G_ID.BATTLE_SKILLDESC_DLG:
				result = new Battle_SkilldescDlg();
				break;
			case G_ID.BATTLE_PLUNDER_TURNCOUNT_DLG:
				result = new Battle_Plunder_TurnCountDlg();
				break;
			case G_ID.BATTLE_POWER_GROUP_DLG:
				result = new Battle_PowerDlg();
				break;
			case G_ID.BATTLE_MINE_CHARINFO_DLG:
				result = new Battle_Mine_CharinfoDlg();
				break;
			case G_ID.BATTLE_BOSS_RANKUP_DLG:
				result = new Battle_BossRankUp_DLG();
				break;
			case G_ID.BATTLE_COLOSSEUMEMERGENCY_SELECT_DLG:
				result = new Battle_ColosseumEmergency_SelectDlg();
				break;
			case G_ID.BATTLE_COLOSSEUM_WAIT_DLG:
				result = new Battle_Colosseum_WaitDlg();
				break;
			case G_ID.LOGIN_DLG:
				result = new LoginDlg();
				break;
			case G_ID.LOGINBG_DLG:
				result = new LoginBGDlg();
				break;
			case G_ID.MOBILELOGIN_DLG:
				result = new MobileLoginDlg();
				break;
			case G_ID.PREDOWNLOAD_DLG:
				result = new Mobile_PreDownloadDlg();
				break;
			case G_ID.SELECTSERVER_DLG:
				result = new SelectServerDlg();
				break;
			case G_ID.LOGIN_SELECT_PLATFORM_DLG:
				result = new Login_Select_PlatformDLG();
				break;
			case G_ID.LOGINRATING_DLG:
				result = new LoginRating();
				break;
			case G_ID.RECONNECT_DLG:
				result = new ReconnectDlg();
				break;
			case G_ID.CHAR_SELECT_DLG:
				result = new CharSelectDlg();
				break;
			case G_ID.RACE_SELECT_DLG:
				result = new RaceSelectDlg();
				break;
			case G_ID.NEW_CREATECHAR_DLG:
				result = new DLG_CreateChar();
				break;
			case G_ID.CHANGENAME_DLG:
				result = new ChangeNameDlg();
				break;
			case G_ID.SOLSELECT_DLG:
				result = new SoldierSelectDlg();
				break;
			case G_ID.SOLMILITARYGROUP_DLG:
				result = new SolMilitaryGroupDlg();
				break;
			case G_ID.SOLMILITARYSELECT_DLG:
				result = new SolMilitarySelectDlg();
				break;
			case G_ID.SOLMILITARYPOSITION_DLG:
				result = new SolMilitaryPositionDlg();
				break;
			case G_ID.SOLEQUIPITEMSELECT_DLG:
				result = new SolEquipItemSelectDlg();
				break;
			case G_ID.SOLSKILLUPDATE_DLG:
				result = new SolSkillUpdateDlg();
				break;
			case G_ID.SOLDETAILINFO_DLG:
				result = new SolDetailinfoDlg();
				break;
			case G_ID.EXPLAIN_TOOLTIP_DLG:
				result = new ExplainTooltipDlg();
				break;
			case G_ID.SOLRECRUIT_DLG:
				result = new SolRecruitDlg();
				break;
			case G_ID.SOLRECRUITSUCCESS_DLG:
				result = new SolRecruitSuccessDlg();
				break;
			case G_ID.SOLCOMPOSE_MAIN_DLG:
				result = new SolComposeMainDlg();
				break;
			case G_ID.SOLCOMPOSE_LIST_DLG:
				result = new SolComposeListDlg();
				break;
			case G_ID.SOLCOMPOSE_CHECK_DLG:
				result = new SolComposeCheckDlg();
				break;
			case G_ID.SOLCOMPOSE_SUCCESS_DLG:
				result = new SolComposeSuccessDlg();
				break;
			case G_ID.SOLCOMPOSE_GRADE_UP_SUCCESS_DLG:
				result = new SolComposeGradeUpSuccessDlg();
				break;
			case G_ID.SOLCOMPOSE_DIRECTION_DLG:
				result = new SolComposeDirection();
				break;
			case G_ID.SOLCOMPOSE_SELL_SUCCESS:
				result = new SolSellSuccess();
				break;
			case G_ID.SOLCOMPOSE_TRANSCENDENCE_DLG:
				result = new SolTranscendenceSuccess();
				break;
			case G_ID.BACK_DLG:
				result = new BackDlg();
				break;
			case G_ID.TAKETALK_DLG:
				result = new TakeTalk_DLG();
				break;
			case G_ID.TAKE_DLG:
				result = new Take_DLG();
				break;
			case G_ID.QUESTGIVEITEM_DLG:
				result = new QuestGiveItemDlg();
				break;
			case G_ID.NPCTALK_DLG:
				result = new NpcTalkUI_DLG();
				break;
			case G_ID.QUESTITEM_SEL_DLG:
				result = new QuestItemSel_Dlg();
				break;
			case G_ID.ADVENTURE_DLG:
				result = new AdventureDlg();
				break;
			case G_ID.EPISODECHECK_DLG:
				result = new EpisodeCheckDlg();
				break;
			case G_ID.CHALLENGE_DLG:
				result = new ChallengeDlg();
				break;
			case G_ID.CHALLENGEPOPUP_DLG:
				result = new ChallengePopupDlg();
				break;
			case G_ID.CHALLENGE_BUNDLE_DLG:
				result = new ChallengeBundleDlg();
				break;
			case G_ID.EXPLORATION_DLG:
				result = new ExplorationDlg();
				break;
			case G_ID.EXPLORATION_PLAY_DLG:
				result = new ExplorationPlayDlg();
				break;
			case G_ID.EXPLORATION_REWARD_DLG:
				result = new ExplorationRewardDlg();
				break;
			case G_ID.CAPTION_DLG:
				result = new CaptionDlg();
				break;
			case G_ID.PLUNDERMAIN_DLG:
				result = new PlunderMainDlg();
				break;
			case G_ID.PLUNDERSOLLIST_DLG:
				result = new PlunderSolListDlg();
				break;
			case G_ID.PLUNDER_STARTANDREMATCH_DLG:
				result = new PlunderStartAndReMatchDlg();
				break;
			case G_ID.PLUNDERSOLNUM_DLG:
				result = new PlunderSolNumDlg();
				break;
			case G_ID.PLUNDERTARGETINFO_DLG:
				result = new PlunderTargetInfoDlg();
				break;
			case G_ID.PLUNDERRECORD_DLG:
				result = new PlunderRecordDlg();
				break;
			case G_ID.PLUNDER_AGREE_DLG:
				result = new PlunderAgreeDlg();
				break;
			case G_ID.PLUNDER_PROTECT_DLG:
				result = new PlunderProtectDlg();
				break;
			case G_ID.PLUNDER_GOLD_DLG:
				result = new PlunderGoldDlg();
				break;
			case G_ID.PLUNDER_RANKINFO_DLG:
				result = new PlunderRankInfoDlg();
				break;
			case G_ID.COLOSSEUMMAIN_DLG:
				result = new ColosseumDlg();
				break;
			case G_ID.COLOSSEUMNOTICE_DLG:
				result = new ColosseumNoticeDlg();
				break;
			case G_ID.COLOSSEUMRANKINFO_DLG:
				result = new ColosseumRankInfoDlg();
				break;
			case G_ID.COLOSSEUMREWARD_DLG:
				result = new ColosseumRewardDlg();
				break;
			case G_ID.COLOSSEUMREWARD_EXPLAIN_DLG:
				result = new ColosseumRewardExplainDlg();
				break;
			case G_ID.COLOSSEUM_CHALLENGE_DLG:
				result = new ColosseumChallenge();
				break;
			case G_ID.COLOSSEUM_CHALLENGE_CHECK_DLG:
				result = new ColosseumChallengeCheck();
				break;
			case G_ID.COLOSSEUM_CHANGERANK_DLG:
				result = new ColosseumChangeRankDlg();
				break;
			case G_ID.COLOSSEUM_BATTLE_RECORD_DLG:
				result = new ColosseumBattleRecordDlg();
				break;
			case G_ID.COLOSSEUM_OBSERVER_SUMMON_DLG:
				result = new ColosseumObserverSummonDlg();
				break;
			case G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG:
				result = new ColosseumObserverControlDlg();
				break;
			case G_ID.COLOSSEUM_SETTING_CARD_DLG:
				result = new ColosseumCardSettingDlg();
				break;
			case G_ID.TOURNAMENT_MASTER_DLG:
				result = new TournamentMasterDlg();
				break;
			case G_ID.TOURNAMENT_LOBBY_DLG:
				result = new TournamentLobbyDlg();
				break;
			case G_ID.BABELTOWERMAIN_DLG:
				result = new BabelTowerMainDlg();
				break;
			case G_ID.BABELTOWERSUB_DLG:
				result = new BabelTowerSubDlg();
				break;
			case G_ID.BABELTOWERUSERLIST_DLG:
				result = new BabelLobbyUserListDlg();
				break;
			case G_ID.BABELTOWER_OPENROOMLIST_DLG:
				result = new BabelTowerOpenRoomListDlg();
				break;
			case G_ID.BABELTOWER_INVITEFRIENDLIST_DLG:
				result = new BabelTowerInviteFriendListDlg();
				break;
			case G_ID.BABELTOWER_FRIENDLIST:
				result = new BabelTower_FriendList();
				break;
			case G_ID.BABELTOWER_CHAT:
				result = new BabelTower_ChatDlg();
				break;
			case G_ID.BABELTOWER_REPEAT_DLG:
				result = new BabelTower_RepeatDlg();
				break;
			case G_ID.BABELTOWER_REPEAT_MAIN_DLG:
				result = new BabelTower_RepeatMainDlg();
				break;
			case G_ID.BABELTOWER_MODESELECT_DLG:
				result = new BabelTower_ModeSelect();
				break;
			case G_ID.INITIATIVE_SET_DLG:
				result = new InitiativeSetDlg();
				break;
			case G_ID.BABEL_GUILDBOSS_MAIN_DLG:
				result = new BabelGuildBossDlg();
				break;
			case G_ID.BABEL_GUILDBOSS_INFO_DLG:
				result = new BabelGuildBossInfoDlg();
				break;
			case G_ID.BABEL_GUILDBOSS_LOBBY_DLG:
				result = new BabelTowerGuildBossLobbyDlg();
				break;
			case G_ID.SOLGUIDE_DLG:
				result = new SolGuide_Dlg();
				break;
			case G_ID.SOLDETAIL_DLG:
				result = new SolDetail_Info_Dlg();
				break;
			case G_ID.SOLELEMENTSUCCESS_DLG:
				result = new SolElementSuccessDlg();
				break;
			case G_ID.SOLDETAIL_SKILLICON_DLG:
				result = new SolDetail_Skill_Dlg();
				break;
			case G_ID.GMCOMMAND_DLG:
				result = new GMCommand_Dlg();
				break;
			case G_ID.ITEM_BOX_RANDOM_DLG:
				result = new Item_Box_Random_Dlg();
				break;
			case G_ID.ITEM_BOX_SELECT_DLG:
				result = new Item_Box_Select_Dlg();
				break;
			case G_ID.ITEM_BOX_ALL_DLG:
				result = new Item_Box_All_Dlg();
				break;
			case G_ID.ITEM_BOX_RARERANDOM_DLG:
				result = new Item_Box_RareRandom_Dlg();
				break;
			case G_ID.CONGRATURATIONDLG:
				result = new Congraturation_DLG();
				break;
			case G_ID.EXCHANGE_POINT_DLG:
				result = new ExchangePointDlg();
				break;
			case G_ID.EXCHANGE_JEWELRY_DLG:
				result = new ExchangeJewelryDlg();
				break;
			case G_ID.EXCHANGE_ITEM_DLG:
				result = new ExchangeItemDlg();
				break;
			case G_ID.EXCHANGE_MYTHICSOL_DLG:
				result = new ExchangeMythicSolDlg();
				break;
			case G_ID.GUESTID_COMBINE_DLG:
				result = new GuestIDCombineDlg();
				break;
			case G_ID.CONVER_PLATFORMID_DLG:
				result = new ConvertPlatformIDDlg();
				break;
			case G_ID.NEARNPCSELECTUI_DLG:
				result = new NearNpcSelectUI_DLG();
				break;
			case G_ID.INDUNTIME_DLG:
				result = new IndunTime_DLG();
				break;
			case G_ID.INDUN_HEADUP_TALK:
				result = new Indun_HeadUpTalk();
				break;
			case G_ID.INDUN_INFO_DLG:
				result = new IndunInfo_DLG();
				break;
			case G_ID.INDUN_RESULT_DLG:
				result = new IndunResult_DLG();
				break;
			case G_ID.INDUN_RECOMMENDLIST_DLG:
				result = new IndunRecommendList_DLG();
				break;
			case G_ID.INDUN_ENTER_RAID_DLG:
				result = new IndunEnterRaid_DLG();
				break;
			case G_ID.INDUN_ENTER_SCENARIO_DLG:
				result = new IndunEnterScenario_DLG();
				break;
			case G_ID.MAIN_QUEST:
				result = new RightMenuQuestUI();
				break;
			case G_ID.QUEST_REWARD:
				result = new QuestReward_DLG();
				break;
			case G_ID.QUEST_GROUP_REWARD:
				result = new CQuestGroupReward();
				break;
			case G_ID.QUESTLIST_DLG:
				result = new QuestList_DLG();
				break;
			case G_ID.QUESTLISTINFO_DLG:
				result = new QuestListInfo_DLG();
				break;
			case G_ID.QUESTLIST_CHAPTERINFO_GAMEDRAMAVIEW_DLG:
				result = new QuestList_ChapterInfo_GameDramaView_DLG();
				break;
			case G_ID.QUEST_RESET_DLG:
				result = new QuestResetUI_DLG();
				break;
			case G_ID.QUEST_GRADE_INFO_DLG:
				result = new QuestGradeInfoUI_DLG();
				break;
			case G_ID.QUEST_CHAPTERSTART:
				result = new ChapterStart_DLG();
				break;
			case G_ID.QUEST_GRADESELECT_DLG:
				result = new QuestGradeSelect_DLG();
				break;
			case G_ID.QUEST_GM_DLG:
				result = new QuestGM_DLG();
				break;
			case G_ID.QUEST_SUBSTORY_DLG:
				result = new QuestSubStoryDlg();
				break;
			case G_ID.DLG_RIGHTCLICK_MENU:
				result = new UI_RightClickMenu();
				break;
			case G_ID.DLG_FPS:
				result = new DLG_FPS();
				break;
			case G_ID.DLG_MEMORYCHECK:
				result = new DLG_AppMemory();
				break;
			case G_ID.SELECTITEM_DLG:
				result = new SelectItemDlg();
				break;
			case G_ID.ITEMLIST_DLG:
				result = new ItemListDlg();
				break;
			case G_ID.COMMUNITY_DLG:
				result = new CommunityUI_DLG();
				break;
			case G_ID.COMMUNITYMSG_DLG:
				result = new CommunityMsgUI_DLG();
				break;
			case G_ID.COMMUNITY_FRIENDMENU_DLG:
				result = new Community_FriendMenu_DLG();
				break;
			case G_ID.COMMUNITYPOPUPMENU_DLG:
				result = new CommunityPopupMenuUI_DLG();
				break;
			case G_ID.FACEBOOK_FRIEND_INVITE:
				result = new FacebookFriendInviteDlg();
				break;
			case G_ID.FRIEND_PUSH_DLG:
				result = new FriendPush_DLG();
				break;
			case G_ID.FRIEND_INVITE_INFO_DLG:
				result = new Friend_Invite_Info_DLG();
				break;
			case G_ID.DLG_OTHER_CHAR_DETAIL:
				result = new DLG_OtherCharDetailInfo();
				break;
			case G_ID.DLG_OTHER_CHAR_EQUIPMENT:
				result = new DLG_OtherCharEquipment();
				break;
			case G_ID.CHANNEL_MOVE_DLG:
				result = new ChannelMove_DLG();
				break;
			case G_ID.CREDIT_DLG:
				result = new CreditDlg();
				break;
			case G_ID.MINE_GUILDPUSH_CONFIRM_DLG:
				result = new MineGuildPushConfirmDlg();
				break;
			case G_ID.TREASUREBOX_DLG:
				result = new TreasureBox_DLG();
				break;
			case G_ID.MSGBOX_DLG:
				result = new MsgBoxUI();
				break;
			case G_ID.INTROMSGBOX_DLG:
				result = new IntroMsgBoxDlg();
				break;
			case G_ID.MESSAGE_DLG:
				result = new MessageDlg();
				break;
			case G_ID.MESSAGE_NOTIFY_DLG:
				result = new MessageNotifyDlg();
				break;
			case G_ID.MSGBOX_TWOCHECK_DLG:
				result = new MsgBoxTwoCheckUI();
				break;
			case G_ID.INVENTORY_DLG:
				result = new Inventory_Dlg();
				break;
			case G_ID.TOOLTIP_DLG:
				result = new Tooltip_Dlg();
				break;
			case G_ID.TOOLTIP_SECOND_DLG:
				result = new Tooltip_Second_Dlg();
				break;
			case G_ID.ITEMTOOLTIP_DLG:
				result = new ItemTooltipDlg();
				break;
			case G_ID.ITEMTOOLTIP_SECOND_DLG:
				result = new ItemTooltipDlg_Second();
				break;
			case G_ID.ITEMTOOLTIP_BUTTON:
				result = new ItemTooltip_Btn_Dlg();
				break;
			case G_ID.CHAT_MAIN_DLG:
				if (TsPlatform.IsWeb)
				{
					result = new MainChatDlg();
				}
				else
				{
					result = new ChatMobileDlg();
				}
				break;
			case G_ID.CHAT_OPTION_DLG:
				result = new ChatOptionDlg();
				break;
			case G_ID.CHAT_AD_DLG:
				result = new AdChatDlg();
				break;
			case G_ID.CHAT_MOBILE_SUB_DLG:
				result = new ChatMobile_Sub_Dlg();
				break;
			case G_ID.EMOTICON_DLG:
				result = new EmoticonDlg();
				break;
			case G_ID.WHISPER_DLG:
				result = new New_Whisper_Dlg();
				break;
			case G_ID.WHISPER_USERLIST_DLG:
				result = new WhisperFriendsDlg();
				break;
			case G_ID.WHISPER_COLOR_DLG:
				result = new WhisperColorDlg();
				break;
			case G_ID.WHISPER_MINIMIZE_DLG:
				result = new WhisperMinimizeDlg();
				break;
			case G_ID.WHISPER_WHISPERPOPUPMENU_DLG:
				result = new WhisperPopupMenu();
				break;
			case G_ID.DLG_SYSTEMMESSAGE:
				result = new Main_UI_SystemMessage();
				break;
			case G_ID.DLG_LOADINGPAGE:
				result = new NewLoaingDlg();
				break;
			case G_ID.MINIDIRECTONTALK_DLG:
				result = new UI_MiniDramaTalk();
				break;
			case G_ID.MiniDRAMAEMOTICON_DLG:
				result = new UI_MiniDramaEmoticon();
				break;
			case G_ID.MINIDRAMACAPTION_DLG:
				result = new UI_MiniDramaCaption();
				break;
			case G_ID.UIGUIDE_DLG:
				result = new UI_UIGuide();
				break;
			case G_ID.POST_DLG:
				result = new PostDlg();
				break;
			case G_ID.POST_RECV_DLG:
				result = new PostRecvDlg();
				break;
			case G_ID.POST_RESULT_DLG:
				result = new PostResultDlg();
				break;
			case G_ID.POSTFRIEND_DLG:
				result = new PostFriendDlg();
				break;
			case G_ID.DLG_STOPAUTOMOVE:
				result = new StopAutoMove();
				break;
			case G_ID.DLG_FOLLOWCHAR:
				result = new Main_UI_FollowChar();
				break;
			case G_ID.DLG_COLLECT:
				result = new Dlg_Collect();
				break;
			case G_ID.CHAT_NOTICE_DLG:
				result = new ChatNoticeDlg();
				break;
			case G_ID.REGION_NAME_DLG:
				result = new Region_Name_Dlg();
				break;
			case G_ID.SYSTEM_OPTION:
				result = new System_Option_Dlg();
				break;
			case G_ID.WAIT_DLG:
				result = new Wait_Dlg();
				break;
			case G_ID.DLG_INPUTNUMBER:
				result = new InputNumberDlg();
				break;
			case G_ID.REFORGEMAIN_DLG:
				result = new ReforgeMainDlg();
				break;
			case G_ID.REFORGESELECT_DLG:
				result = new ReforgeSelectDlg();
				break;
			case G_ID.REFORGECONFIRM_DLG:
				result = new ReforgeConfirmDlg();
				break;
			case G_ID.REFORGERESULT_DLG:
				result = new ReforgeResultDlg();
				break;
			case G_ID.REFORGESELECTITEM_DLG:
				result = new ItemSelectDlg();
				break;
			case G_ID.FACEBOOK_FEED_DLG:
				result = new Facebook_Feed_Dlg();
				break;
			case G_ID.JOYSTICK_DLG:
				result = new JoyStickDlg();
				break;
			case G_ID.MOBILE_DROPDOWN_DLG:
				result = new MobileDropDownDlg();
				break;
			case G_ID.TEST_DLG:
				result = new CTestDlg();
				break;
			case G_ID.DLG_AUDIO:
				result = new DLG_Audio();
				break;
			case G_ID.STORYCHAT_SET_DLG:
				result = new StoryChatSetDlg();
				break;
			case G_ID.STORYCHAT_DLG:
				result = new StoryChatDlg();
				break;
			case G_ID.STORYCHATDETAIL_DLG:
				result = new StoryChatDetailDlg();
				break;
			case G_ID.STORYCHAT_LIKELIST_DLG:
				result = new StoryChatLikeListDlg();
				break;
			case G_ID.ITEMMALL_DLG:
				result = new ItemMallDlg();
				break;
			case G_ID.ITEMMALL_PRODUCTDETAIL_DLG:
				result = new ItemMallProductDetailDlg();
				break;
			case G_ID.ITEMMALL_SOL_DETAIL:
				result = new ItemMallSolDetailDlg();
				break;
			case G_ID.COUPON_DLG:
				result = new CouponDlg();
				break;
			case G_ID.CHARCHANGEMAIN_DLG:
				result = new CharChangeMainDlg();
				break;
			case G_ID.CHARCHANGE_DLG:
				result = new CharChangeDlg();
				break;
			case G_ID.REDUCEMAIN_DLG:
				result = new ReduceMainDlg();
				break;
			case G_ID.REDUCERESULT_DLG:
				result = new ReduceResultDlg();
				break;
			case G_ID.WILLCHARGE_DLG:
				result = new WillChargeDlg();
				break;
			case G_ID.ITEMSKILL_DLG:
				result = new ItemSkill_Dlg();
				break;
			case G_ID.ITEMSKILL_RESULT_DLG:
				result = new ItemSkillResult_Dlg();
				break;
			case G_ID.VIPINFO_DLG:
				result = new VipInfoDlg();
				break;
			case G_ID.RECOMMEND_DLG:
				result = new ReCommendDlg();
				break;
			case G_ID.SUPPORTER_DLG:
				result = new SupporterDlg();
				break;
			case G_ID.SUPPORTERSUB_DLG:
				result = new SupporterSubDlg();
				break;
			case G_ID.AUCTION_ITEMSELECT_DLG:
				result = new AuctionItemSelectDlg();
				break;
			case G_ID.AUCTION_MAIN_DLG:
				result = new AuctionMainDlg();
				break;
			case G_ID.AUCTION_PURCHASECHECK_DLG:
				result = new AuctionPurchaseCheckDlg();
				break;
			case G_ID.AUCTION_SEARCH_DLG:
				result = new AuctionSearchDlg();
				break;
			case G_ID.AUCTION_SELLCHECK_DLG:
				result = new AuctionSellCheckDlg();
				break;
			case G_ID.AUCTION_TENDERCHECK_DLG:
				result = new AuctionTenderCheckDlg();
				break;
			case G_ID.MINE_SEARCH_DLG:
				result = new MineSearchDlg();
				break;
			case G_ID.MINE_SEARCH_DETAILINFO_DLG:
				result = new MineSearchDetailInfoDlg();
				break;
			case G_ID.MINE_MILITARY_SET_DLG:
				result = new MineMilitarySetDlg();
				break;
			case G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG:
				result = new MineGuildCurrentStatusInfoDlg();
				break;
			case G_ID.MINE_TUTORIAL_STEP_DLG:
				result = new MineTutorialStepDlg();
				break;
			case G_ID.MINE_WAITMILTARYINFO_DLG:
				result = new MineWaitMiltaryInfoDlg();
				break;
			case G_ID.DLG_DIRECTION:
				result = new DirectionDLG();
				break;
			case G_ID.NPC_AUTOMOVE_DLG:
				result = new NpcAutoMove_DLG();
				break;
			case G_ID.SOL_WAREHOUSE_EXPANSION_DLG:
				result = new SolWarehouseExpansionDlg();
				break;
			case G_ID.NEWGUILD_ADMINMENU_DLG:
				result = new NewGuildAdminMenuDlg();
				break;
			case G_ID.NEWGUILD_APPLICATION_DLG:
				result = new NewGuildApplicationDlg();
				break;
			case G_ID.NEWGUILD_APPROVAL_DLG:
				result = new NewGuildApprovalDlg();
				break;
			case G_ID.NEWGUILD_CREATE_DLG:
				result = new NewGuildCreateDlg();
				break;
			case G_ID.NEWGUILD_INVITE_DLG:
				result = new NewGuildInviteDlg();
				break;
			case G_ID.NEWGUILD_LIST_DLG:
				result = new NewGuildListDlg();
				break;
			case G_ID.NEWGUILD_MAIN_DLG:
				result = new NewGuildMainDlg();
				break;
			case G_ID.NEWGUILD_MEMBER_DLG:
				result = new NewGuildMemberDlg();
				break;
			case G_ID.NEWGUILD_RANKCHANGE_DLG:
				result = new NewGuildRankChangeDlg();
				break;
			case G_ID.NEWGUILD_CHANGENAME_DLG:
				result = new NewGuildChangeNameDlg();
				break;
			case G_ID.NEWGUILD_MAINSELECT_DLG:
				result = new NewGuildMainSelectDlg();
				break;
			case G_ID.REINCARNATION_DLG:
				result = new ReincarnationDlg();
				break;
			case G_ID.REINCARNATION_SUCCESS_DLG:
				result = new ReincarnationSuccessDlg();
				break;
			case G_ID.INFIBATTLE_RANK_DLG:
				result = new InfiBattleRankDlg();
				break;
			case G_ID.INFIBATTLE_REWARD_DLG:
				result = new InfiBattleReward();
				break;
			case G_ID.EVENT_MAIN:
				result = new Event_Dlg();
				break;
			case G_ID.EVENT_MAIN_EXPLAIN:
				result = new Event_Detail_Dlg();
				break;
			case G_ID.EVENT_DAILY_GIFT_DLG:
				result = new DailyGift_Dlg();
				break;
			case G_ID.GOLDLACK_DLG:
				result = new LackGold_dlg();
				break;
			case G_ID.SOLAWAKENING_DLG:
				result = new SolAwakeningDlg();
				break;
			case G_ID.BOUNTYHUNTING_DLG:
				result = new BountyHuntingDlg();
				break;
			case G_ID.BOUNTYCHECK_DLG:
				result = new BountyCheckDlg();
				break;
			case G_ID.ITEMGIFTTARGET_DLG:
				result = new ItemGiftTargetDlg();
				break;
			case G_ID.ITEMGIFTINPUTNAME_DLG:
				result = new ItemGiftInputNameDlg();
				break;
			case G_ID.EXPEDITION_SEARCH_DLG:
				result = new ExpeditionSearchDlg();
				break;
			case G_ID.EXPEDITION_SEARCHDETAILINFO_DLG:
				result = new ExpeditionSearchDetallInfoDlg();
				break;
			case G_ID.MINE_MAINSELECT_DLG:
				result = new MineMainSelectDlg();
				break;
			case G_ID.EXPEDITION_CURRENTSTATUSINFO_DLG:
				result = new ExpeditionCurrentStatusInfoDlg();
				break;
			case G_ID.EXPEDITION_BATTLE_RESULT_DLG:
				result = new Battle_ResultExpeditionDlg();
				break;
			case G_ID.GUILDWAR_CONDITION_DLG:
				result = new NewGuildWarConditionDlg();
				break;
			case G_ID.GUILDWAR_DETAILINFO_DLG:
				result = new NewGuildWarDetailInfoDlg();
				break;
			case G_ID.GUILDWAR_REWARDINFO_DLG:
				result = new NewGuildWarRewardInfoDlg();
				break;
			case G_ID.AGIT_MAIN_DLG:
				result = new Agit_MainDlg();
				break;
			case G_ID.AGIT_GOLDRECORD_DLG:
				result = new Agit_GoldRecordDlg();
				break;
			case G_ID.AGIT_LEVELUP_DLG:
				result = new Agit_LevelUpDlg();
				break;
			case G_ID.AGIT_MERCHANT_DLG:
				result = new Agit_MerchantDlg();
				break;
			case G_ID.AGIT_NPC_INVITE_DLG:
				result = new Agit_NPCInviteDlg();
				break;
			case G_ID.AGIT_INFO_DLG:
				result = new Agit_InfoDlg();
				break;
			case G_ID.AGIT_GOLDENEGG_DLG:
				result = new Agit_GoldenEggDlg();
				break;
			case G_ID.AGIT_GOLDENEGGDRAMA_DLG:
				result = new Agit_GoldenEggDramaDlg();
				break;
			case G_ID.DECLAREWAR_GUILDLIST_DLG:
				result = new DeclareWar_GuildListDlg();
				break;
			}
			return result;
		}
	}
}
