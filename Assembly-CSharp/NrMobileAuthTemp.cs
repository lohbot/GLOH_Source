using GameMessage;
using GameMessage.Private;
using Global;
using GooglePlayGames;
using Ndoors.Framework.Stage;
using System;
using UnityEngine;
using UnityForms;

public class NrMobileAuthTemp : IWebViewListener, NrMobileAuth
{
	public void Init()
	{
	}

	public bool Login(string id, string pw)
	{
		return false;
	}

	public bool IsLogin(Action ShowLoginFunc)
	{
		if (ShowLoginFunc != null)
		{
			ShowLoginFunc();
		}
		return false;
	}

	public void RequestOauthToken(string id, string pw, Action<string> ResultFunc)
	{
	}

	public bool IsGuestLogin(Action ShowGuestIDLGFunc)
	{
		return false;
	}

	public bool IsGuest()
	{
		return false;
	}

	public void DeleteAuthInfo()
	{
		PlayerPrefs.SetString(NrPrefsKey.PLAYER_PREFS_MOBILEAUTHKEY, string.Empty);
		PlayerPrefs.SetInt(NrPrefsKey.LAST_AUTH_PLATFORM, 0);
		PlayerPrefs.Save();
		if (NrMobileAuthSystem.Instance.RequestLogout)
		{
			NmFacebookManager.instance.Logout();
			NmFacebookManager.instance.Logout();
			if (TsPlatform.IsAndroid)
			{
				((PlayGamesPlatform)Social.Active).SignOut();
			}
		}
		NrMobileAuthSystem.Instance.RequestLogout = false;
	}

	public void RequestLogin()
	{
		NrTSingleton<NkClientLogic>.Instance.SetLoginGameServer(true);
		string @string = PlayerPrefs.GetString(NrPrefsKey.PLAYER_PREFS_MOBILEAUTHKEY);
		string uRL = string.Empty;
		byte b = 3;
		string text = string.Empty;
		if (TsPlatform.IsAndroid)
		{
			b = 2;
			text = TsPlatform.APP_VERSION_AND;
		}
		else if (TsPlatform.IsIPhone)
		{
			b = 1;
			text = TsPlatform.APP_VERSION_IOS;
		}
		if (!string.IsNullOrEmpty(@string) && @string.Length > 0 && !NrTSingleton<NrGlobalReference>.Instance.IsLocalServiceArea())
		{
			uRL = string.Format("{0}://{1}/mobile/android_server_ck.aspx?authkey={2}&platform={3}&GameVer={4}&HpType={5}&HpOS={6}&HpMem={7}&SolID={8}", new object[]
			{
				(!NrTSingleton<NrGlobalReference>.Instance.IsLocalServiceArea()) ? "https" : "http",
				NrGlobalReference.strWebPageDomain,
				@string,
				b.ToString(),
				text,
				SystemInfo.deviceModel,
				SystemInfo.operatingSystem,
				SystemInfo.systemMemorySize,
				PlayerPrefs.GetString(NrPrefsKey.LATEST_SOLID, string.Empty)
			});
			TsPlatform.Operator.OpenURL(uRL, this, true, true, false, true);
		}
		else
		{
			if (TsPlatform.IsAndroid)
			{
				uRL = string.Format("{0}://{1}/mobile/login.aspx?clientpath={2}&device={3}&platform={4}&GameVer={5}&HpType={6}&HpOS={7}&HpMem={8}&SolID={9}", new object[]
				{
					(!NrTSingleton<NrGlobalReference>.Instance.IsLocalServiceArea()) ? "https" : "http",
					NrGlobalReference.strWebPageDomain,
					NrGlobalReference.SERVICECODE,
					TsPlatform.Operator.GetMobileDeviceId(),
					b.ToString(),
					text,
					SystemInfo.deviceModel,
					SystemInfo.operatingSystem,
					SystemInfo.systemMemorySize,
					PlayerPrefs.GetString(NrPrefsKey.LATEST_SOLID, string.Empty)
				});
			}
			else if (TsPlatform.IsIPhone || TsPlatform.IsIPad())
			{
				uRL = string.Format("{0}://{1}/mobile/login.aspx?clientpath={2}&device={3}&platform={4}&GameVer={5}&HpType={6}&HpMem={7}&SolID={8}", new object[]
				{
					(!NrTSingleton<NrGlobalReference>.Instance.IsLocalServiceArea()) ? "https" : "http",
					NrGlobalReference.strWebPageDomain,
					NrGlobalReference.SERVICECODE,
					TsPlatform.Operator.GetMobileDeviceId(),
					b.ToString(),
					text,
					SystemInfo.deviceModel,
					SystemInfo.systemMemorySize,
					PlayerPrefs.GetString(NrPrefsKey.LATEST_SOLID, string.Empty)
				});
			}
			TsPlatform.Operator.OpenURL(uRL, this, true, true, false, true);
		}
	}

	public void RequestPlatformLogin()
	{
		NrTSingleton<NkClientLogic>.Instance.SetLoginGameServer(true);
		Debug.LogWarning("========== NrMobileAuthSystem.RequestPlatformLogin : SetLoginGameServer true ----- ");
		string @string = PlayerPrefs.GetString(NrPrefsKey.PLAYER_PREFS_MOBILEAUTHKEY);
		string uRL = string.Empty;
		byte b = 3;
		string text = string.Empty;
		if (TsPlatform.IsAndroid)
		{
			b = 2;
			text = TsPlatform.APP_VERSION_AND;
		}
		else if (TsPlatform.IsIPhone)
		{
			b = 1;
			text = TsPlatform.APP_VERSION_IOS;
		}
		uRL = string.Format("{0}://{1}/mobile/android_server_list.aspx?authkey={2}&platform={3}&GameVer={4}&HpType={5}&HpOS={6}&HpMem={7}&SolID={8}", new object[]
		{
			(!NrTSingleton<NrGlobalReference>.Instance.IsLocalServiceArea()) ? "https" : "http",
			NrGlobalReference.strWebPageDomain,
			@string,
			b.ToString(),
			text,
			SystemInfo.deviceModel,
			SystemInfo.operatingSystem,
			SystemInfo.systemMemorySize,
			PlayerPrefs.GetString(NrPrefsKey.LATEST_SOLID, string.Empty)
		});
		TsPlatform.Operator.OpenURL(uRL, this, true, true, false, true);
	}

	public bool OnWebCall(string WebCall)
	{
		bool flag = false;
		Debug.Log("OnWebCall = " + WebCall);
		if (WebCall.Contains("close"))
		{
			TsPlatform.Operator.CloseWebView();
			if (Scene.CurScene == Scene.Type.LOGIN)
			{
				NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
			}
			MsgHandler.Handle("ShowPlatformLogin", new object[0]);
			return true;
		}
		if (WebCall.Contains("InternetConnnetError"))
		{
			Debug.Log("!!!!!!!InternetConnnetError!!");
			MsgHandler.Handle("InternetConnnetErrorMessage", new object[0]);
			TsPlatform.Operator.CloseWebView();
			MsgHandler.Handle("ShowPlatformLogin", new object[0]);
			return true;
		}
		string value = "mobile/android_server_list_proc.aspx";
		if (WebCall.Length == 0 || !WebCall.Contains(value))
		{
			return true;
		}
		if (WebCall.Contains(value))
		{
			string[] array = WebCall.Split(new char[]
			{
				'?'
			});
			if (array.Length < 1)
			{
				return flag;
			}
			string[] array2 = array[1].Split(new char[]
			{
				'&'
			});
			string text = string.Empty;
			if (array2.Length >= 0)
			{
				string[] array3 = array2[0].Split(new char[]
				{
					'='
				});
				if (array3.Length > 0)
				{
					text = array3[1];
				}
			}
			Debug.LogError("########## OnWebCall Result : " + text);
			string text2 = text;
			switch (text2)
			{
			case "0":
			{
				TsPlatform.Operator.CloseWebView();
				string text3 = array2[1].Split(new char[]
				{
					'='
				})[1];
				string text4 = array2[2].Split(new char[]
				{
					'='
				})[1];
				string text5 = array2[3].Split(new char[]
				{
					'='
				})[1];
				string text6 = array2[4].Split(new char[]
				{
					'='
				})[1];
				string text7 = array2[5].Split(new char[]
				{
					'='
				})[1];
				string text8 = array2[7].Split(new char[]
				{
					'='
				})[1];
				if (!string.IsNullOrEmpty(text5))
				{
					long num2 = 0L;
					long.TryParse(text5, out num2);
					NrTSingleton<NrMainSystem>.Instance.SetLatestPersonID(num2.ToString());
				}
				long cID = TsPlatform.Operator.GetCID();
				if (cID != 0L)
				{
					NrTSingleton<NrMainSystem>.Instance.SetLatestPersonID(cID.ToString());
				}
				NrTSingleton<NrMainSystem>.Instance.m_strWorldServerIP = text3.Trim();
				if (!int.TryParse(text4.Trim(), out NrTSingleton<NrMainSystem>.Instance.m_nWorldServerPort))
				{
					NrTSingleton<NrMainSystem>.Instance.m_nWorldServerPort = Client.GetWorldServerPort();
				}
				NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey = text6.Trim();
				NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nSerialNumber = long.Parse(text7.Trim());
				PlayerPrefs.SetString(NrPrefsKey.PLAYER_PREFS_MOBILEAUTHKEY, NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey);
				if (text8.Equals(NrGlobalReference.CDNTYPE))
				{
					NrTSingleton<NrGlobalReference>.Instance.ChangeEdgeDataCDNPath();
				}
				if (text8.Equals(NrGlobalReference.CDNTYPEUS))
				{
					NrTSingleton<NrGlobalReference>.Instance.ChangeEdgeDataCDNPath();
				}
				Debug.LogError(string.Concat(new string[]
				{
					"======================== CDNTYPE : ",
					NrGlobalReference.CDNTYPE,
					" === CDNTYPEUS ",
					NrGlobalReference.CDNTYPEUS,
					" === ",
					text8
				}));
				if (NrTSingleton<NrGlobalReference>.Instance.localWWW)
				{
					FacadeHandler.MoveStage(Scene.Type.INITIALIZE);
				}
				else
				{
					FacadeHandler.MoveStage(Scene.Type.NPATCH_DOWNLOAD);
				}
				TsLog.LogWarning("OnWecall Complete!!!", new object[0]);
				flag = true;
				break;
			}
			case "errMsg":
			{
				TsPlatform.Operator.CloseWebView();
				this.DeleteAuthInfo();
				string text9 = array2[1].Split(new char[]
				{
					'='
				})[1];
				text9 = WWW.UnEscapeURL(text9);
				MsgHandler.Handle("LoginFailed", new object[]
				{
					text9
				});
				flag = true;
				break;
			}
			case "ReLogin":
				TsPlatform.Operator.CloseWebView();
				this.DeleteAuthInfo();
				MsgHandler.Handle("LoginFailed", new object[]
				{
					text
				});
				flag = true;
				break;
			}
			if (!flag)
			{
				this.DeleteAuthInfo();
				MsgHandler.Handle("LoginFailed", new object[]
				{
					"WebCallError"
				});
				flag = true;
			}
		}
		return flag;
	}

	public void OnAlertViewConfirmed(string message)
	{
		if (message.Contains("â�� �ݾ��ֽñ�"))
		{
			TsPlatform.Operator.CloseWebView();
		}
	}

	public void OnFinishCall(string message)
	{
		if (TsPlatform.IsAndroid)
		{
			NrTSingleton<NrMainSystem>.Instance.QuitGame();
		}
		if (TsPlatform.IsIPhone)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGIN_SELECT_PLATFORM_DLG);
		}
	}
}
