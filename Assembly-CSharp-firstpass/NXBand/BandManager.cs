using System;
using System.Collections.Generic;
using UnityEngine;

namespace NXBand
{
	public class BandManager
	{
		private static string GAMEOBJECT_NAME = "BandManager";

		private static string SET_HANDLER = "setHandler";

		private GameObject _gameObject;

		private static BandManager _instance;

		private static string NX_BAND_PLUGIN = "com.nexon.bandsdk.NXBand";

		private static AndroidJavaObject _activity;

		public bool isInitilazed
		{
			get;
			set;
		}

		public static BandManager instance
		{
			get
			{
				if (BandManager._instance == null)
				{
					BandManager._instance = new BandManager();
				}
				return BandManager._instance;
			}
		}

		private BandManager()
		{
			this._gameObject = new GameObject(BandManager.GAMEOBJECT_NAME, new Type[]
			{
				typeof(BandGameObject)
			});
			UnityEngine.Object.DontDestroyOnLoad(this._gameObject);
			this.isInitilazed = false;
		}

		public void init(string clientId, string clientSecret)
		{
			if (!this.isInitilazed)
			{
				this.isInitilazed = true;
				this.callJavaMethod(RequestCode.REQUEST_INIT, new object[]
				{
					clientId,
					clientSecret
				});
			}
		}

		public void login(BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call login()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_LOGIN, new object[0]);
		}

		public void getUserKey(BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call getUserKey()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_USER_KEY, new object[0]);
		}

		public string getUserKey()
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call getUserKey()");
			}
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(BandManager.NX_BAND_PLUGIN))
			{
				result = androidJavaClass.CallStatic<string>(this.methodNameFromRequestCode(RequestCode.REQUEST_USER_KEY), new object[0]);
			}
			return result;
		}

		public void getAccessToken(BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call getUserKey()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_ACCESS_TOKEN, new object[0]);
		}

		public string getAccessToken()
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call getAccessToken()");
			}
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(BandManager.NX_BAND_PLUGIN))
			{
				result = androidJavaClass.CallStatic<string>(this.methodNameFromRequestCode(RequestCode.REQUEST_ACCESS_TOKEN), new object[0]);
			}
			return result;
		}

		public void logout(BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call logout()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_LOGOUT, new object[0]);
		}

		public void disconnect(BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call disconnect()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_DISCONNECT, new object[0]);
		}

		public void getProfile(BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call getProfile()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_MY_PROFILE, new object[0]);
		}

		public void getProfile(string userKey, BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call getProfile()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_PROFILE, new object[]
			{
				userKey
			});
		}

		public void listMembers(BandListOption option, BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call listMembers()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			string text = null;
			if (option != null)
			{
				text = option.getJsonString();
			}
			this.callJavaMethod(RequestCode.REQUEST_LIST_MEMBER, new object[]
			{
				text
			});
		}

		public void listBandMembers(BandListOption option, BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call listBandMembers()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			string text = null;
			if (option != null)
			{
				text = option.getJsonString();
			}
			this.callJavaMethod(RequestCode.REQUEST_LIST_BAND_MEMBER, new object[]
			{
				text
			});
		}

		public void listBandMembers(string bandKey, BandListOption option, BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call listBandMembers()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			string text = null;
			if (option != null)
			{
				text = option.getJsonString();
			}
			this.callJavaMethod(RequestCode.REQUEST_LIST_BAND_MEMBER, new object[]
			{
				bandKey,
				text
			});
		}

		public void listBands(BandListOption option, BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call listBands()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			string text = null;
			if (option != null)
			{
				text = option.getJsonString();
			}
			this.callJavaMethod(RequestCode.REQUEST_LIST_BAND, new object[]
			{
				text
			});
		}

		public void sendInvitation(string receiverKey, string message, string title, string imageUrl, Dictionary<string, string> customUrl, BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call sendInvitation()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_SEND_INVITATION, new object[]
			{
				this.messageJson(receiverKey, message, title, imageUrl, customUrl)
			});
		}

		public void sendInvitation(string receiverKey, string message, string title, string imageUrl, BandMessageCustomUrl customUrl, BandResultHandler resultHandler)
		{
			this.sendInvitation(receiverKey, message, title, imageUrl, customUrl.getCustomUrlDictionary(), resultHandler);
		}

		public void sendMessage(string receiverKey, string message, string title, string imageUrl, Dictionary<string, string> customUrl, BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call sendMessage()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_SEND_MESSAGE, new object[]
			{
				this.messageJson(receiverKey, message, title, imageUrl, customUrl)
			});
		}

		public void sendMessage(string receiverKey, string message, string title, string imageUrl, BandMessageCustomUrl customUrl, BandResultHandler resultHandler)
		{
			this.sendMessage(receiverKey, message, title, imageUrl, customUrl.getCustomUrlDictionary(), resultHandler);
		}

		public void writePost(string bandKey, string body, string imageUrl, string subTitle, string subText, Dictionary<string, string> customUrl, BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call writePost()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_WRITE_POST, new object[]
			{
				this.postJson(bandKey, body, imageUrl, subTitle, subText, customUrl)
			});
		}

		public void writePost(string bandKey, string body, string imageUrl, string subTitle, string subText, BandMessageCustomUrl customUrl, BandResultHandler resultHandler)
		{
			this.writePost(bandKey, body, imageUrl, subTitle, subText, customUrl.getCustomUrlDictionary(), resultHandler);
		}

		public void getQuota(BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call getQuota()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_QUOTA, new object[0]);
		}

		public void getInviter(BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call getInviter()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_INVITER, new object[0]);
		}

		public void getCacheInfo(BandResultHandler resultHandler)
		{
			if (!this.isInitilazed)
			{
				throw new InvalidOperationException("You must call init() before call getCacheInfo()");
			}
			this._gameObject.SendMessage(BandManager.SET_HANDLER, resultHandler);
			this.callJavaMethod(RequestCode.REQUEST_CACHE_INFO, new object[0]);
		}

		private void callJavaMethod(RequestCode reqCode, params object[] arg)
		{
			if (BandManager._activity == null && Application.platform == RuntimePlatform.Android)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					BandManager._activity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				}
			}
			using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass(BandManager.NX_BAND_PLUGIN))
			{
				object[] array = new object[arg.Length + 1];
				array[0] = BandManager._activity;
				if (arg.Length > 0)
				{
					arg.CopyTo(array, 1);
				}
				androidJavaClass2.CallStatic(this.methodNameFromRequestCode(reqCode), array);
			}
		}

		private string methodNameFromRequestCode(RequestCode reqCode)
		{
			switch (reqCode)
			{
			case RequestCode.REQUEST_INIT:
				return "init";
			case RequestCode.REQUEST_LOGIN:
				return "login";
			case RequestCode.REQUEST_LOGOUT:
				return "logout";
			case RequestCode.REQUEST_DISCONNECT:
				return "disconnect";
			case RequestCode.REQUEST_PROFILE:
			case RequestCode.REQUEST_MY_PROFILE:
				return "getProfile";
			case RequestCode.REQUEST_USER_KEY:
				return "getUserKey";
			case RequestCode.REQUEST_LIST_MEMBER:
				return "listMembers";
			case RequestCode.REQUEST_LIST_BAND_MEMBER:
				return "listBandMembers";
			case RequestCode.REQUEST_SEND_INVITATION:
				return "sendInvitation";
			case RequestCode.REQUEST_SEND_MESSAGE:
				return "sendMessage";
			case RequestCode.REQUEST_LIST_BAND:
				return "listBands";
			case RequestCode.REQUEST_WRITE_POST:
				return "writePost";
			case RequestCode.REQUEST_ACCESS_TOKEN:
				return "getAccessToken";
			case RequestCode.REQUEST_QUOTA:
				return "getQuota";
			case RequestCode.REQUEST_INVITER:
				return "getInviter";
			case RequestCode.REQUEST_CACHE_INFO:
				return "getCacheInfo";
			default:
				return string.Empty;
			}
		}

		private string messageJson(string receiverKey, string message, string title, string imageUrl, Dictionary<string, string> customUrl)
		{
			string text = "{";
			bool flag = false;
			if (receiverKey != null)
			{
				text = text + "\"receiverKey\":\"" + receiverKey + "\"";
				flag = true;
			}
			if (message != null)
			{
				if (flag)
				{
					text += ",";
				}
				text = text + "\"message\":\"" + message + "\"";
				flag = true;
			}
			if (title != null)
			{
				if (flag)
				{
					text += ",";
				}
				text = text + "\"title\":\"" + title + "\"";
				flag = true;
			}
			if (imageUrl != null)
			{
				if (flag)
				{
					text += ",";
				}
				text = text + "\"imageUrl\":\"" + imageUrl + "\"";
				flag = true;
			}
			if (customUrl != null && customUrl.Count > 0)
			{
				if (flag)
				{
					text += ",";
				}
				text += "\"customUrl\":{";
				bool flag2 = true;
				foreach (KeyValuePair<string, string> current in customUrl)
				{
					if (!flag2)
					{
						text += ",";
					}
					else
					{
						flag2 = false;
					}
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\"",
						current.Key,
						"\":\"",
						current.Value,
						"\""
					});
				}
				text += "}";
			}
			text += "}";
			return text;
		}

		private string postJson(string bandKey, string body, string imageUrl, string subTitle, string subText, Dictionary<string, string> customUrl)
		{
			string text = "{";
			bool flag = false;
			if (bandKey != null)
			{
				text = text + "\"bandKey\":\"" + bandKey + "\"";
				flag = true;
			}
			if (body != null)
			{
				if (flag)
				{
					text += ",";
				}
				text = text + "\"body\":\"" + body + "\"";
				flag = true;
			}
			if (imageUrl != null)
			{
				if (flag)
				{
					text += ",";
				}
				text = text + "\"imageUrl\":\"" + imageUrl + "\"";
				flag = true;
			}
			if (subTitle != null)
			{
				if (flag)
				{
					text += ",";
				}
				text = text + "\"subTitle\":\"" + subTitle + "\"";
				flag = true;
			}
			if (subText != null)
			{
				if (flag)
				{
					text += ",";
				}
				text = text + "\"subText\":\"" + subText + "\"";
				flag = true;
			}
			if (customUrl != null && customUrl.Count > 0)
			{
				if (flag)
				{
					text += ",";
				}
				text += "\"customUrl\":{";
				bool flag2 = true;
				foreach (KeyValuePair<string, string> current in customUrl)
				{
					if (!flag2)
					{
						text += ",";
					}
					else
					{
						flag2 = false;
					}
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\"",
						current.Key,
						"\":\"",
						current.Value,
						"\""
					});
				}
				text += "}";
			}
			text += "}";
			return text;
		}
	}
}
