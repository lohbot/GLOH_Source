using GameMessage;
using SERVICE;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class TsPlatform
{
	public interface IOperator
	{
		void SetMobileIdentifier();

		string GetMobileDeviceId();

		void Login(string strOauthToken);

		void OpenURL(string URL, IWebViewListener WebCall, bool bVisility, bool bAccessBack, bool bAppQuit = false, bool bShowCloseButton = true);

		void OpenURL(string URL, IWebViewListener WebCall, int nX, int nY, bool bVisibility);

		void CloseBanner();

		void CloseWebView();

		void SaveOauthToken(string strToken);

		string GetOauthToken();

		bool IsWifiConnect();

		bool CheckMobilePerfomance();

		void ConnectGooglePlay(string url);

		string GetFileDir();

		string GetRootDir();

		void PlayMovie(string path, Color bgColor);

		void PlayMovieAtPath(string path, float fVolume = 1f);

		void PlayMovieURL(string url, Color bgColor, bool bIsBGMMute, bool bShowToast = true, float fVolum = 1f);

		void RePlayMovieAlertMsg();

		void ShowProgreesDlg(string strMsg);

		void DestroyProgreesDlg();

		void ShowToast(string sms);

		string GetPhonyNumber();

		string GetPhonyAgencyName();

		string[] GetContact();

		void SendSMS(string strPhoneNum, string strMsg);

		string GetPackageName();

		void Vibrate();

		void Vibrator(long milliSeconds);

		string GetOSVersion();

		string GetLocale();

		void ShowIMEKeyboard(string message, string placeHolder, bool secureField);

		long GetFileLength(string path);

		void DeleteDirectory(string path, bool recursive);

		void MoveDirectory(string oldpath, string newpath);

		string GetDeviceToken();

		long GetCID();

		long GetSDCardCapacity();

		long GetAppMemory();

		void RegisterPushToken();

		bool IsAutoBrightnessMode();

		int GetBrightness();

		void SetAutoBrightnessMode(bool bAuto);

		void Brightness(int nBrightness);

		void SendLocalPush(int id, long AddTime, string Message);

		void CancelLocalPush(int id);

		void GetPlayLockID();

		bool isEmulator();

		bool isEmulatorTest();

		string isMacro();

		void PlatformLoginComplete();

		void CopyClipboard(string text);

		string Getimei();

		string Getimsi();

		string GetIPAddress();
	}

	public class BuildPackage
	{
		public static readonly TsPlatform.BuildPackage Unknown = new TsPlatform.BuildPackage("Unknown");

		public static readonly TsPlatform.BuildPackage GooglePlay = new TsPlatform.BuildPackage("GooglePlay");

		public static readonly TsPlatform.BuildPackage GooglePlayWifi = new TsPlatform.BuildPackage("GooglePlayWifi");

		public static readonly TsPlatform.BuildPackage TStore = new TsPlatform.BuildPackage("TStore");

		public static readonly TsPlatform.BuildPackage AppStore = new TsPlatform.BuildPackage("AppStore");

		private string _pakage;

		protected BuildPackage(string pakage)
		{
			this._pakage = pakage;
		}

		public override string ToString()
		{
			return this._pakage;
		}

		public override int GetHashCode()
		{
			return this._pakage.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public static bool operator ==(TsPlatform.BuildPackage x, TsPlatform.BuildPackage y)
		{
			return x._pakage == y._pakage || (x == TsPlatform.BuildPackage.GooglePlay && y == TsPlatform.BuildPackage.GooglePlayWifi) || (y == TsPlatform.BuildPackage.GooglePlay && x == TsPlatform.BuildPackage.GooglePlayWifi);
		}

		public static bool operator !=(TsPlatform.BuildPackage x, TsPlatform.BuildPackage y)
		{
			return !(x == y);
		}
	}

	public class WebOperator : TsPlatform.IOperator
	{
		public void SetMobileIdentifier()
		{
		}

		public string GetMobileDeviceId()
		{
			return string.Empty;
		}

		public void Login(string strOauthToken)
		{
		}

		public void OpenURL(string URL, IWebViewListener WebCall, bool bVisility, bool bAccessBack, bool bAppQuit = false, bool bShowCloseButton = true)
		{
		}

		public void OpenURL(string URL, IWebViewListener WebCall, int nX, int nY, bool bVisibility)
		{
		}

		public void CloseBanner()
		{
		}

		public void CloseWebView()
		{
		}

		public void SaveOauthToken(string strToken)
		{
		}

		public string GetOauthToken()
		{
			return string.Empty;
		}

		public void RePlayMovieAlertMsg()
		{
		}

		public bool IsWifiConnect()
		{
			return true;
		}

		public bool CheckMobilePerfomance()
		{
			return true;
		}

		public void ConnectGooglePlay(string url)
		{
		}

		public string GetFileDir()
		{
			return string.Empty;
		}

		public string GetRootDir()
		{
			return string.Empty;
		}

		public void PlayMovie(string path, Color bgColor)
		{
		}

		public void PlayMovieAtPath(string path, float fVolume = 1f)
		{
		}

		public void PlayMovieURL(string url, Color bgColor, bool bIsBGMMute, bool bShowToast = true, float fVolum = 1f)
		{
		}

		public void ShowProgreesDlg(string strMsg)
		{
		}

		public void DestroyProgreesDlg()
		{
		}

		public void ShowToast(string sms)
		{
		}

		public string GetPhonyNumber()
		{
			return string.Empty;
		}

		public string GetPhonyAgencyName()
		{
			return string.Empty;
		}

		public string[] GetContact()
		{
			return null;
		}

		public void SendSMS(string strPhoneNum, string strMsg)
		{
		}

		public string GetPackageName()
		{
			return string.Empty;
		}

		public void Vibrate()
		{
		}

		public void Vibrator(long milliSeconds)
		{
		}

		public string GetOSVersion()
		{
			return string.Empty;
		}

		public string GetLocale()
		{
			return string.Empty;
		}

		public void ShowIMEKeyboard(string message, string placeHolder, bool secureField)
		{
		}

		public long GetFileLength(string path)
		{
			return 0L;
		}

		public void DeleteDirectory(string path, bool recursive)
		{
		}

		public void MoveDirectory(string oldpath, string newpath)
		{
		}

		public string GetDeviceToken()
		{
			return string.Empty;
		}

		public long GetCID()
		{
			return 0L;
		}

		public long GetSDCardCapacity()
		{
			return 0L;
		}

		public long GetAppMemory()
		{
			return 0L;
		}

		public void RegisterPushToken()
		{
		}

		public bool IsAutoBrightnessMode()
		{
			return true;
		}

		public int GetBrightness()
		{
			return 0;
		}

		public void Brightness(int nBrightness)
		{
		}

		public void SetAutoBrightnessMode(bool bAuto)
		{
		}

		public void SendLocalPush(int id, long AddTime, string Message)
		{
		}

		public void CancelLocalPush(int id)
		{
		}

		public void GetPlayLockID()
		{
		}

		public bool isEmulator()
		{
			return false;
		}

		public bool isEmulatorTest()
		{
			return false;
		}

		public string isMacro()
		{
			return string.Empty;
		}

		public void PlatformLoginComplete()
		{
		}

		public void CopyClipboard(string text)
		{
		}

		public string Getimei()
		{
			return string.Empty;
		}

		public string Getimsi()
		{
			return string.Empty;
		}

		public string GetIPAddress()
		{
			return string.Empty;
		}
	}

	public abstract class MobileOperator : TsPlatform.IOperator
	{
		public virtual void SetMobileIdentifier()
		{
		}

		public virtual string GetMobileDeviceId()
		{
			return string.Empty;
		}

		public virtual void Login(string strOauthToken)
		{
		}

		public virtual void SaveOauthToken(string strToken)
		{
		}

		public virtual string GetOauthToken()
		{
			return string.Empty;
		}

		public virtual bool IsWifiConnect()
		{
			return true;
		}

		public virtual bool CheckMobilePerfomance()
		{
			return true;
		}

		public virtual void ConnectGooglePlay(string url)
		{
		}

		public virtual void PlayMovieAtPath(string path, float fVolume = 1f)
		{
		}

		public virtual void PlayMovieURL(string url, Color bgColor, bool bIsBGMMute, bool bShowToast = true, float fVolum = 1f)
		{
		}

		public virtual void Vibrator(long milliSeconds)
		{
		}

		public virtual string GetDeviceToken()
		{
			return string.Empty;
		}

		public virtual void ShowIMEKeyboard(string message, string placeHolder, bool secureField)
		{
		}

		public virtual long GetCID()
		{
			return 0L;
		}

		public virtual long GetSDCardCapacity()
		{
			return 0L;
		}

		public virtual void RePlayMovieAlertMsg()
		{
		}

		public virtual string GetFileDir()
		{
			return string.Empty;
		}

		public virtual string GetRootDir()
		{
			return string.Empty;
		}

		public virtual string GetOSVersion()
		{
			return string.Empty;
		}

		public virtual string GetLocale()
		{
			return string.Empty;
		}

		public virtual void ShowProgreesDlg(string strMsg)
		{
		}

		public virtual void DestroyProgreesDlg()
		{
		}

		public virtual long GetAppMemory()
		{
			return 0L;
		}

		public virtual void RegisterPushToken()
		{
		}

		public virtual bool IsAutoBrightnessMode()
		{
			return true;
		}

		public virtual int GetBrightness()
		{
			return 0;
		}

		public virtual void SetAutoBrightnessMode(bool bAuto)
		{
		}

		public virtual void Brightness(int nBrightness)
		{
		}

		public virtual void SendLocalPush(int id, long AddTime, string Message)
		{
		}

		public virtual void CancelLocalPush(int id)
		{
		}

		public virtual void GetPlayLockID()
		{
		}

		public virtual bool isEmulator()
		{
			return false;
		}

		public virtual bool isEmulatorTest()
		{
			return false;
		}

		public virtual string isMacro()
		{
			return string.Empty;
		}

		public virtual void PlatformLoginComplete()
		{
		}

		public virtual void CopyClipboard(string text)
		{
		}

		public virtual string Getimei()
		{
			return string.Empty;
		}

		public virtual string Getimsi()
		{
			return string.Empty;
		}

		public virtual string GetIPAddress()
		{
			return string.Empty;
		}

		public virtual void ShowToast(string sms)
		{
		}

		public virtual string GetPhonyNumber()
		{
			return string.Empty;
		}

		public virtual string GetPhonyAgencyName()
		{
			return string.Empty;
		}

		public virtual string[] GetContact()
		{
			return null;
		}

		public virtual void SendSMS(string strPhoneNum, string strMsg)
		{
		}

		public virtual string GetPackageName()
		{
			return string.Empty;
		}

		public virtual void OpenURL(string url, IWebViewListener WebCall, bool bVisility, bool bAccessBack, bool bAppQuit = false, bool bShowCloseButton = true)
		{
			NrWebViewObject gameObject = NrWebViewObject.GetGameObject();
			gameObject.Init(WebCall, bShowCloseButton);
			gameObject.LoadURL(url, bVisility, bAccessBack, bAppQuit, bShowCloseButton);
			gameObject.SetVisibility(bVisility);
			gameObject.SetMargins(0, 0, Screen.width, Screen.height);
		}

		public virtual void OpenURL(string url, IWebViewListener WebCall, int nX, int nY, bool bVisibility)
		{
			NrWebViewObject gameObject = NrWebViewObject.GetGameObject();
			gameObject.Init(WebCall, true);
			gameObject.OpenURL(url, nX, nY, bVisibility);
		}

		public void CloseBanner()
		{
			NrWebViewObject gameObject = NrWebViewObject.GetGameObject();
			if (gameObject != null)
			{
				gameObject.CloseBanner();
			}
		}

		public void CloseWebView()
		{
			NrWebViewObject gameObject = NrWebViewObject.GetGameObject();
			if (gameObject != null)
			{
				gameObject.Destory();
			}
			else
			{
				TsLog.LogError("CloseWebView WebView == null", new object[0]);
			}
		}

		public virtual void PlayMovie(string path, Color bgColor)
		{
			Handheld.PlayFullScreenMovie(path, bgColor);
		}

		public virtual void Vibrate()
		{
			Handheld.Vibrate();
		}

		public virtual long GetFileLength(string path)
		{
			FileInfo fileInfo = new FileInfo(path);
			return fileInfo.Length;
		}

		public virtual void DeleteDirectory(string path, bool recursive)
		{
			Directory.Delete(path, true);
		}

		public virtual void MoveDirectory(string oldpath, string newpath)
		{
			Directory.Move(oldpath, newpath);
		}
	}

	public class AndroidOperator : TsPlatform.MobileOperator
	{
		public static string _strIdentifierName = "com.nexon.loh.korlocal.AndroidAPI";

		private byte m_ucKey1 = 102;

		private byte m_ucKey2 = 136;

		public override void SetMobileIdentifier()
		{
			TsPlatform.AndroidOperator._strIdentifierName = NrGlobalReference.MOBILEID + ".AndroidAPI";
			Debug.LogWarning("AndroidOperator._strIdentifierName ====>>> " + TsPlatform.AndroidOperator._strIdentifierName);
		}

		public override void PlayMovieURL(string url, Color bgColor, bool bIsBGMMute, bool bShowToast = true, float fVolum = 1f)
		{
			if (TsPlatform.IsEditor)
			{
				return;
			}
			if (bIsBGMMute && url.Contains("mp4"))
			{
				fVolum = 0f;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					@static.CallStatic("PlayMovieURL", new object[]
					{
						@static,
						url,
						bShowToast,
						fVolum
					});
					MsgHandler.Handle("PlayMovieTime", new object[0]);
				}
			}
		}

		public override void RePlayMovieAlertMsg()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("RePlayMovieAlertMsg", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public override string GetMobileDeviceId()
		{
			string result = string.Empty;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<string>("getAndID", new object[0]);
					}
				}
			}
			return result;
		}

		public override void Login(string strOauthToken)
		{
		}

		public override void Vibrator(long milliSeconds)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("Vibrator", new object[]
						{
							milliSeconds
						});
					}
				}
			}
		}

		public override void SaveOauthToken(string strToken)
		{
		}

		public override string GetOauthToken()
		{
			return string.Empty;
		}

		public override bool CheckMobilePerfomance()
		{
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						float num = androidJavaClass2.CallStatic<float>("GetFreeMemInfo", new object[0]);
						if (num < 300f)
						{
							result = false;
						}
						else
						{
							result = true;
						}
					}
				}
			}
			return result;
		}

		public override bool IsWifiConnect()
		{
			bool result = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<bool>("IsWifiConnect", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public override void ConnectGooglePlay(string url)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("ConnectGooglePlay", new object[]
						{
							@static,
							url
						});
					}
				}
			}
		}

		public override string GetDeviceToken()
		{
			string result = string.Empty;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<string>("GetDeviceToken", new object[0]);
					}
				}
			}
			return result;
		}

		public override long GetCID()
		{
			Debug.LogWarning("_strIdentifierName :" + TsPlatform.AndroidOperator._strIdentifierName);
			long result = 0L;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<long>("GetCid", new object[0]);
					}
				}
			}
			return result;
		}

		public override string GetFileDir()
		{
			return Application.persistentDataPath;
		}

		public override string GetRootDir()
		{
			string result = string.Empty;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<string>("GetRootDir", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public override string GetOSVersion()
		{
			string result = string.Empty;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<string>("GetOSVersion", new object[0]);
					}
				}
			}
			return result;
		}

		public override void ShowProgreesDlg(string strMsg)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("ShowProgreesDlg", new object[]
						{
							@static,
							strMsg
						});
					}
				}
			}
		}

		public override void DestroyProgreesDlg()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("DismissProgreesDlg", new object[0]);
					}
				}
			}
		}

		public override void ShowToast(string sms)
		{
			if (TsPlatform.IsEditor)
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("Toast", new object[]
						{
							@static,
							sms
						});
					}
				}
			}
		}

		public override string GetPhonyNumber()
		{
			return string.Empty;
		}

		public override string GetPhonyAgencyName()
		{
			return string.Empty;
		}

		public override string[] GetContact()
		{
			string[] result = null;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<string[]>("GetContact", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public override string GetPackageName()
		{
			string result = string.Empty;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					result = @static.CallStatic<string>("GetPackageName", new object[]
					{
						@static
					});
				}
			}
			return result;
		}

		public override void SendSMS(string strPhoneNum, string strMsg)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("SendSMS", new object[]
						{
							strPhoneNum,
							strMsg
						});
					}
				}
			}
		}

		public override long GetSDCardCapacity()
		{
			long result = 0L;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<long>("GetSDCardCapacity", new object[0]);
					}
				}
			}
			return result;
		}

		public override long GetAppMemory()
		{
			if (TsPlatform.IsEditor)
			{
				return 0L;
			}
			long result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<long>("GetAppMemory", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public override void RegisterPushToken()
		{
			if (TsPlatform.IsEditor || NrTSingleton<NrGlobalReference>.Instance.IsLocalServiceArea())
			{
				return;
			}
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNTEST || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNREVIEW || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_CNQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_CNTEST)
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("RegisterGCM", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public override bool IsAutoBrightnessMode()
		{
			if (TsPlatform.IsEditor)
			{
				return true;
			}
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<bool>("IsAutoBrightnessMode", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public override int GetBrightness()
		{
			if (TsPlatform.IsEditor)
			{
				return 0;
			}
			int result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<int>("GetBrightness", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public override void Brightness(int nBrightness)
		{
			if (TsPlatform.IsEditor)
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("Brightness", new object[]
						{
							@static,
							nBrightness
						});
					}
				}
			}
		}

		public override void SetAutoBrightnessMode(bool bAuto)
		{
			if (TsPlatform.IsEditor)
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("SetAutoBrightness", new object[]
						{
							@static,
							bAuto
						});
					}
				}
			}
		}

		public override void SendLocalPush(int id, long AddTime, string Message)
		{
			if (TsPlatform.IsEditor)
			{
				return;
			}
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNTEST || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNREVIEW || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_CNQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_CNTEST)
			{
				return;
			}
			TsLog.Log("SendLocalPush ID:{0} AddTime:{1}, Message:{2} CurTime:{3}", new object[]
			{
				id,
				AddTime,
				Message,
				DateTime.Now
			});
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("SendLocalPush", new object[]
						{
							@static,
							id,
							AddTime * 1000L,
							Message
						});
					}
				}
			}
		}

		public override void CancelLocalPush(int id)
		{
			if (TsPlatform.IsEditor)
			{
				return;
			}
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNTEST || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNREVIEW || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_CNQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_CNTEST)
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("CancelLocalPush", new object[]
						{
							@static,
							id
						});
					}
				}
			}
		}

		public override void GetPlayLockID()
		{
			if (TsPlatform.IsEditor)
			{
				return;
			}
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORGOOGLE)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
						{
							androidJavaClass2.CallStatic("GetPlayLockID", new object[]
							{
								@static
							});
						}
					}
				}
			}
		}

		public override bool isEmulator()
		{
			if (TsPlatform.IsEditor)
			{
				return false;
			}
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<bool>("isEmulator", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public override string isMacro()
		{
			if (TsPlatform.IsEditor)
			{
				return string.Empty;
			}
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<string>("isMacro", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public override bool isEmulatorTest()
		{
			if (TsPlatform.IsEditor)
			{
				return false;
			}
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.CallStatic<bool>("isEmulatorTest", new object[0]);
					}
				}
			}
			return result;
		}

		public override void CopyClipboard(string text)
		{
			if (TsPlatform.IsEditor)
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						androidJavaClass2.CallStatic("CopyClipboard", new object[]
						{
							@static,
							text
						});
					}
				}
			}
		}

		public override string Getimei()
		{
			if (TsPlatform.IsEditor || !TsPlatform.isChina())
			{
				return string.Empty;
			}
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.Call<string>("Getimei", new object[0]);
					}
				}
			}
			return result;
		}

		public override string Getimsi()
		{
			if (TsPlatform.IsEditor || !TsPlatform.isChina())
			{
				return string.Empty;
			}
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(TsPlatform.AndroidOperator._strIdentifierName))
					{
						result = androidJavaClass2.Call<string>("Getimsi", new object[0]);
					}
				}
			}
			return result;
		}

		private void _Encode(byte[] pData)
		{
			ushort num = (ushort)pData.Length;
			if (3 < num)
			{
				byte b = pData[2] + this.m_ucKey1;
				byte b2 = this.m_ucKey1;
				byte b3 = this.m_ucKey2;
				int num2 = (int)num;
				for (int i = 3; i < num2; i++)
				{
					byte b4 = pData[i];
					b2 ^= b4;
					pData[i] = b2 + b3;
					b3 += b4;
				}
				pData[2] = b;
			}
		}

		private bool _Decode(byte[] pData, int index)
		{
			ushort num = (ushort)pData.Length;
			pData[index + 2] = pData[index + 2] - this.m_ucKey1;
			byte b = this.m_ucKey1;
			byte b2 = this.m_ucKey2;
			ushort num2 = num;
			for (int i = 3; i < (int)num2; i++)
			{
				byte b3 = pData[index + i];
				pData[index + i] = b3 - b2;
				pData[index + i] = (pData[index + i] ^ b);
				b ^= pData[index + i];
				b2 += pData[index + i];
			}
			return true;
		}
	}

	public class iOSOperator : TsPlatform.MobileOperator
	{
	}

	public enum TouchScreenKeyboardType
	{
		Default,
		ASCIICapable,
		NumbersAndPunctuation,
		URL,
		NumberPad,
		PhonePad,
		NamePhonePad,
		EmailAddress
	}

	public class TouchScreenKeyboard
	{
		public UnityEngine.TouchScreenKeyboard keyboard;

		public static bool autorotateToPortrait
		{
			set
			{
				Screen.autorotateToPortrait = value;
			}
		}

		public static bool autorotateToPortraitUpsideDown
		{
			set
			{
				Screen.autorotateToPortraitUpsideDown = value;
			}
		}

		public static bool autorotateToLandscapeLeft
		{
			set
			{
				Screen.autorotateToLandscapeLeft = value;
			}
		}

		public static bool autorotateToLandscapeRight
		{
			set
			{
				Screen.autorotateToLandscapeRight = value;
			}
		}

		public bool done
		{
			get
			{
				return this.keyboard.done;
			}
		}

		public string text
		{
			get
			{
				return this.keyboard.text;
			}
			set
			{
				this.keyboard.text = value;
			}
		}

		public bool active
		{
			get
			{
				return this.keyboard.active;
			}
			set
			{
				this.keyboard.active = value;
			}
		}

		public static TsPlatform.TouchScreenKeyboard Open(string text, TsPlatform.TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert, string textPlaceholder)
		{
			return new TsPlatform.TouchScreenKeyboard
			{
				keyboard = new UnityEngine.TouchScreenKeyboard(text, (UnityEngine.TouchScreenKeyboardType)keyboardType, autocorrection, multiline, secure, alert, textPlaceholder)
			};
		}
	}

	public enum iPhoneGeneration
	{
		Unknown,
		iPhone,
		iPhone3G,
		iPhone3GS,
		iPodTouch1Gen,
		iPodTouch2Gen,
		iPodTouch3Gen,
		iPad1Gen,
		iPhone4,
		iPodTouch4Gen,
		iPad2Gen,
		iPhone4S,
		iPad3Gen,
		iPhone5,
		iPodTouch5Gen,
		iPhoneUnknown = 10001,
		iPadUnknown,
		iPodTouchUnknown
	}

	public static class iPhone
	{
		public static TsPlatform.iPhoneGeneration generation
		{
			get
			{
				return TsPlatform.iPhoneGeneration.Unknown;
			}
		}
	}

	public enum eHDMode
	{
		NORMAL,
		HD7,
		HD10
	}

	public static string APP_VERSION_AND = "1.0.01";

	public static string APP_VERSION_IOS = "1.0.01";

	public static string strLogFilePath = string.Empty;

	private static TsPlatform.IOperator _operator = null;

	private static StringBuilder stringBuilder = new StringBuilder(1024);

	private static TsPlatform.eHDMode m_bHDMode = TsPlatform.eHDMode.NORMAL;

	public static string GooglePlayURL
	{
		get
		{
			return string.Format("http://{0}/mobile/updateurl.aspx?code={1}&platform=android", NrGlobalReference.strWebPageDomain, NrGlobalReference.MOBILEID);
		}
	}

	public static string IOSMaketURL
	{
		get
		{
			return string.Format("http://{0}/mobile/updateurl.aspx?code={1}&platform=ios", NrGlobalReference.strWebPageDomain, NrGlobalReference.MOBILEID);
		}
	}

	public static TsPlatform.IOperator Operator
	{
		get
		{
			if (TsPlatform._operator == null)
			{
				if (TsPlatform.IsWeb)
				{
					TsPlatform._operator = new TsPlatform.WebOperator();
				}
				else if (TsPlatform.IsMobile)
				{
					if (TsPlatform.IsAndroid)
					{
						TsPlatform._operator = new TsPlatform.AndroidOperator();
					}
					else
					{
						TsPlatform._operator = new TsPlatform.iOSOperator();
					}
				}
			}
			return TsPlatform._operator;
		}
	}

	public static bool IsMobile
	{
		get
		{
			return true;
		}
	}

	public static bool IsAndroid
	{
		get
		{
			return true;
		}
	}

	public static bool IsIPhone
	{
		get
		{
			return false;
		}
	}

	public static bool IsWeb
	{
		get
		{
			return false;
		}
	}

	public static bool IsEditor
	{
		get
		{
			return false;
		}
	}

	public static bool IsDebugBuild
	{
		get
		{
			return Debug.isDebugBuild;
		}
	}

	public static bool IsLowSystemMemory
	{
		get
		{
			return TsPlatform.IsEditor || (SystemInfo.systemMemorySize < 1024 && SystemInfo.systemMemorySize > 0);
		}
	}

	public static bool IsBand
	{
		get
		{
			if (TsPlatform.IsEditor)
			{
				return false;
			}
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			return currentServiceArea >= eSERVICE_AREA.SERVICE_ANDROID_BANDNAVER && currentServiceArea <= eSERVICE_AREA.SERVICE_ANDROID_BANDGOOGLE;
		}
	}

	public static bool IsKakao
	{
		get
		{
			if (TsPlatform.IsEditor)
			{
				return false;
			}
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			return currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORKAKAO || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KAKAOTSTORE || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_KORKAKAO;
		}
	}

	public static bool IsLINE
	{
		get
		{
			if (TsPlatform.IsEditor)
			{
				return false;
			}
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			return currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_JPQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_JPQALINE || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_JPLINE || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_JPQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_JPQALINE || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_JPLINE;
		}
	}

	public static bool IsUSA
	{
		get
		{
			if (TsPlatform.IsEditor)
			{
				return false;
			}
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			return currentServiceArea >= eSERVICE_AREA.SERVICE_ANDROID_USLOCAL && currentServiceArea <= eSERVICE_AREA.SERVICE_ANDROID_USAMAZON;
		}
	}

	public static TsPlatform.eHDMode HDMode
	{
		get
		{
			return TsPlatform.m_bHDMode;
		}
		set
		{
			TsPlatform.m_bHDMode = value;
		}
	}

	public static string GetAppURL()
	{
		return string.Format("http://{0}/mobile/updateurl.aspx?code={1}&platform=android", NrGlobalReference.strWebPageDomain, NrGlobalReference.MOBILEID);
	}

	public static TsPlatform.BuildPackage GetBuildPackage()
	{
		if (TsPlatform.IsAndroid)
		{
			string packageName = TsPlatform.Operator.GetPackageName();
			if (packageName != null)
			{
				if (packageName.ToLower().Contains("google"))
				{
					if (packageName.ToLower().Contains("wifi"))
					{
						return TsPlatform.BuildPackage.GooglePlayWifi;
					}
					return TsPlatform.BuildPackage.GooglePlay;
				}
				else if (packageName.ToLower().Contains("tstore"))
				{
					return TsPlatform.BuildPackage.TStore;
				}
			}
		}
		else if (TsPlatform.IsIPhone)
		{
			return TsPlatform.BuildPackage.AppStore;
		}
		return TsPlatform.BuildPackage.Unknown;
	}

	public static string ConvertToMobileFileName(string fileName)
	{
		if (string.IsNullOrEmpty(fileName) || fileName == "0")
		{
			return fileName;
		}
		TsPlatform.stringBuilder.Length = 0;
		if (TsPlatform.IsAndroid)
		{
			TsPlatform.stringBuilder.AppendFormat("{0}_and", fileName);
		}
		else if (TsPlatform.IsIPhone)
		{
			TsPlatform.stringBuilder.AppendFormat("{0}_ios", fileName);
		}
		return TsPlatform.stringBuilder.ToString();
	}

	public static string GetPlatformPostfix()
	{
		if (TsPlatform.IsAndroid)
		{
			return "_mobile_and";
		}
		if (TsPlatform.IsIPhone)
		{
			return "_mobile_ios";
		}
		return string.Empty;
	}

	public static void FileLog(string strLog, bool bStart)
	{
		if (!TsPlatform.IsEditor)
		{
			try
			{
				if (TsPlatform.strLogFilePath == string.Empty)
				{
					TsPlatform.strLogFilePath = string.Format("{0}/TestLog_{1}_{2}_{3}{4}{5}.txt", new object[]
					{
						TsPlatform.Operator.GetFileDir(),
						DateTime.Now.Month.ToString(),
						DateTime.Now.Day.ToString(),
						DateTime.Now.Hour.ToString("00"),
						DateTime.Now.Minute.ToString("00"),
						DateTime.Now.Second.ToString("00")
					});
				}
				StreamWriter streamWriter = new StreamWriter(TsPlatform.strLogFilePath, bStart);
				streamWriter.WriteLine(strLog);
				streamWriter.Close();
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
			}
		}
		else
		{
			Debug.LogWarning(strLog);
		}
	}

	public static void FileLog(string strLog)
	{
		TsPlatform.FileLog(strLog, true);
	}

	public static bool IsIPad()
	{
		return false;
	}

	public static bool isChina()
	{
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		return currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNLOCAL || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNTEST || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_CNREVIEW || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_CNQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_CNTEST;
	}
}
