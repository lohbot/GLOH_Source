using GameMessage;
using Prime31;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

public class NmFacebookManager
{
	private const float INVATE_TIME = 0.1f;

	private static NmFacebookManager _instance;

	private SortedDictionary<string, FacebookUserData> m_FriendsData = new SortedDictionary<string, FacebookUserData>();

	private Stack<FacebookUserData> m_InviteUser = new Stack<FacebookUserData>();

	private List<string> m_RequestRecevieUsers = new List<string>();

	private FacebookUserData m_UserData = new FacebookUserData();

	private bool _isFacebook;

	private string m_SaveMethodName = string.Empty;

	private object[] m_SaveDatas;

	public FACEBOOK_STEP m_Step;

	private bool m_bWorldConnect;

	private float m_InvateFriendsTime;

	private bool m_FacebookLogin;

	private bool m_bPublishPermission = true;

	private bool m_bSendSync;

	private bool m_bFacebookActive = true;

	public static NmFacebookManager instance
	{
		get
		{
			if (NmFacebookManager._instance == null)
			{
				NmFacebookManager._instance = new NmFacebookManager();
			}
			return NmFacebookManager._instance;
		}
	}

	public SortedDictionary<string, FacebookUserData> FriendsData
	{
		get
		{
			return this.m_FriendsData;
		}
		set
		{
			this.m_FriendsData = value;
		}
	}

	public List<string> RequestRecevieUsers
	{
		get
		{
			return this.m_RequestRecevieUsers;
		}
		set
		{
			this.m_RequestRecevieUsers = value;
		}
	}

	public FacebookUserData UserData
	{
		get
		{
			return this.m_UserData;
		}
	}

	public bool IsFacebook
	{
		get
		{
			return this._isFacebook;
		}
		set
		{
			this._isFacebook = value;
		}
	}

	public bool WorldConnect
	{
		get
		{
			return this.m_bWorldConnect;
		}
		set
		{
			this.m_bWorldConnect = value;
		}
	}

	public bool FacebookLogin
	{
		get
		{
			return this.m_FacebookLogin;
		}
		set
		{
			this.m_FacebookLogin = value;
		}
	}

	public bool PublishPermission
	{
		get
		{
			return this.m_bPublishPermission;
		}
		set
		{
			this.m_bPublishPermission = value;
		}
	}

	public bool SendSync
	{
		get
		{
			return this.m_bSendSync;
		}
		set
		{
			this.m_bSendSync = value;
		}
	}

	public bool FacebookActive
	{
		get
		{
			return this.m_bFacebookActive;
		}
		set
		{
			this.m_bFacebookActive = value;
		}
	}

	public NmFacebookManager()
	{
		if (!GameObject.Find("FacebookEventListener"))
		{
			GameObject gameObject = new GameObject("FacebookEventListener");
			gameObject.AddComponent<FacebookEventListener>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	[DebuggerHidden]
	public IEnumerable FriendDataGetValue()
	{
		NmFacebookManager.<FriendDataGetValue>c__Iterator1E <FriendDataGetValue>c__Iterator1E = new NmFacebookManager.<FriendDataGetValue>c__Iterator1E();
		<FriendDataGetValue>c__Iterator1E.<>f__this = this;
		NmFacebookManager.<FriendDataGetValue>c__Iterator1E expr_0E = <FriendDataGetValue>c__Iterator1E;
		expr_0E.$PC = -2;
		return expr_0E;
	}

	public string GetFriendName(string ID)
	{
		if (this.m_FriendsData.ContainsKey(ID))
		{
			return this.m_FriendsData[ID].m_Name;
		}
		return string.Empty;
	}

	public bool initFBUserData()
	{
		this.m_UserData.init();
		return true;
	}

	private void completionHandler(string error, object result)
	{
		if (error != null)
		{
			UnityEngine.Debug.LogError(error);
		}
		else
		{
			Utils.logObject(result);
		}
	}

	public bool isSessionValid()
	{
		return FacebookAndroid.isSessionValid();
	}

	public void init()
	{
		FacebookAndroid.init(true);
	}

	public void Login()
	{
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			this._isFacebook = true;
			if (!this.isSessionValid() || this.m_UserData.m_ID == string.Empty)
			{
				NmFacebookManager.instance.FacebookLogin = true;
				UnityEngine.Debug.LogWarning("FacebookAndroid Login");
				FacebookAndroid.init(true);
				FacebookAndroid.loginWithPublishPermissions(new string[]
				{
					"publish_actions",
					"manage_friendlists"
				});
			}
			else if (this.m_UserData.m_ID == string.Empty)
			{
				this.GetMeFacebookData();
			}
			else
			{
				this.GetFriends();
			}
			this.m_Step = FACEBOOK_STEP.LOGIN;
		}
	}

	public void Logout()
	{
		this.m_bWorldConnect = false;
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			FacebookAndroid.logout();
		}
	}

	public void GetMeFacebookData()
	{
		Facebook.instance.getMe(new Action<string, FacebookMeResult>(this.GetMeData));
	}

	public void GetMeData(string error, object result)
	{
		if (error != null)
		{
			UnityEngine.Debug.LogError(error);
			this.Login();
			return;
		}
		FacebookMeResult facebookMeResult = result as FacebookMeResult;
		if (facebookMeResult != null)
		{
			this.m_UserData.m_ID = facebookMeResult.id;
			this.m_UserData.m_Name = facebookMeResult.name;
			this.m_UserData.m_Email = facebookMeResult.email;
			if (this.m_bWorldConnect)
			{
				MsgHandler.Handle("SendFacebookID", new object[]
				{
					this.m_UserData.m_ID
				});
				this.m_bFacebookActive = false;
				this.m_bSendSync = true;
				this.m_Step = FACEBOOK_STEP.GETME;
			}
			else
			{
				this.GetFriends();
				string @string = PlayerPrefs.GetString(NrPrefsKey.PLAYER_PREFS_MOBILEAUTHKEY);
				if (!string.IsNullOrEmpty(@string))
				{
					MsgHandler.Handle("RequestAutoLogin", new object[0]);
				}
				else
				{
					MsgHandler.Handle("RequestLogin", new object[0]);
				}
			}
			return;
		}
		this.GetMeFacebookData();
	}

	public void GetFriends()
	{
		Facebook.instance.getFriends(new Action<string, FacebookFriendsResult>(this.GetFriendList));
	}

	private void GetFriendList(string error, object result)
	{
		if (error != null)
		{
			UnityEngine.Debug.LogError(error);
		}
		else if (result != null)
		{
			FacebookFriendsResult facebookFriendsResult = result as FacebookFriendsResult;
			if (facebookFriendsResult != null)
			{
				List<FacebookFriend> data = facebookFriendsResult.data;
				if (data != null)
				{
					for (int i = 0; i < data.Count; i++)
					{
						string id = data[i].id;
						string name = data[i].name;
						bool installed = data[i].installed;
						if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !this.m_FriendsData.ContainsKey(id))
						{
							FacebookUserData facebookUserData = new FacebookUserData();
							facebookUserData.m_ID = id;
							facebookUserData.m_Name = name;
							facebookUserData.m_Installed = installed;
							this.m_FriendsData.Add(id, facebookUserData);
						}
					}
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("result");
		}
		UnityEngine.Debug.Log("Get FriendsList Complete!!  FriendCount = " + this.m_FriendsData.Count);
		MsgHandler.Handle("FacebookFriendDataArrage", new object[0]);
		this.m_Step = FACEBOOK_STEP.CALL_SAVE_METHOD;
	}

	public void PostMessage(string message, string link, string linkName, string linkToImage, string caption, string description = null)
	{
		if (!TsPlatform.IsMobile || TsPlatform.IsEditor)
		{
			return;
		}
		if (this.IsLoginAndPermissionCheck("PostMessage", new object[]
		{
			message,
			link,
			linkName,
			linkToImage,
			caption,
			description
		}))
		{
			if (!this.m_bFacebookActive)
			{
				this.IsFacebook = false;
				return;
			}
			Facebook.instance.postMessageWithLinkAndLinkToImage(message, link, linkName, linkToImage, caption, description, new Action<string, object>(this.PostMessageComplete));
		}
		this.IsFacebook = true;
	}

	public void PostMessageComplete(string error, object result)
	{
		if (string.IsNullOrEmpty(error))
		{
			if (result != null)
			{
				Dictionary<string, object> dictionary = result as Dictionary<string, object>;
				if (dictionary.ContainsKey("id"))
				{
					TsLog.LogWarning("PosetMessageComplete \n{0}", new object[]
					{
						dictionary["id"] as string
					});
				}
			}
		}
		else
		{
			TsLog.LogWarning("PostMessage Error = {0}", new object[]
			{
				error
			});
		}
		this.IsFacebook = false;
	}

	public void FriendRequestMessage(string message, string FriendId)
	{
		if (!TsPlatform.IsMobile || TsPlatform.IsEditor)
		{
			return;
		}
		if (this.IsLoginAndPermissionCheck("FriendRequestMessage", new object[]
		{
			message,
			FriendId
		}))
		{
			if (!this.m_bFacebookActive)
			{
				this.IsFacebook = false;
				return;
			}
			FacebookAndroid.showAppInviteDialog("https://fb.me/896641427051249", "http://us-loh.s3.amazonaws.com/android_usgoogle/facebookimg/300.png");
		}
		this.IsFacebook = true;
	}

	public void DialogCompleteEvent(string Message)
	{
		this.m_RequestRecevieUsers.Clear();
		string[] array = Message.Split(new char[]
		{
			'&'
		});
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Contains("to"))
			{
				string[] array2 = array[i].Split(new char[]
				{
					'='
				});
				if (array2.Length > 1)
				{
					this.m_RequestRecevieUsers.Add(array2[1]);
					UnityEngine.Debug.LogWarning(string.Format("RequestRecevieUser ID = {0}", array2[1]));
				}
			}
		}
		UnityEngine.Debug.LogWarning("DialogCompleteEvent Save RequestRecevieUser");
		this.IsFacebook = false;
	}

	public void FriendFeedPost(string FriendID, string message, string link, string linkName, string linkToImage, string caption)
	{
		if (!TsPlatform.IsMobile || TsPlatform.IsEditor)
		{
			return;
		}
		if (this.IsLoginAndPermissionCheck("FriendFeedPost", new object[]
		{
			FriendID,
			message,
			link,
			linkName,
			linkToImage,
			caption
		}))
		{
			if (!this.m_bFacebookActive)
			{
				this.IsFacebook = false;
				return;
			}
			Dictionary<string, object> parameters = new Dictionary<string, object>
			{
				{
					"to",
					FriendID
				},
				{
					"message",
					message
				},
				{
					"link",
					link
				},
				{
					"picture",
					linkToImage
				},
				{
					"caption",
					caption
				}
			};
			FacebookAndroid.showFacebookShareDialog(parameters);
		}
		this.IsFacebook = true;
	}

	public void AutoInviteUser()
	{
		if (this.IsLoginAndPermissionCheck("AutoInviteUser", new object[0]))
		{
			if (!this.m_bFacebookActive)
			{
				this.IsFacebook = false;
				return;
			}
			foreach (FacebookUserData current in this.m_FriendsData.Values)
			{
				if (current.m_Installed)
				{
					this.m_InviteUser.Push(current);
				}
			}
			this.m_Step = FACEBOOK_STEP.INVITE_FRIENDS;
			this.m_InvateFriendsTime = Time.realtimeSinceStartup + 0.1f;
			TsLog.LogWarning("AutoInviteUser Count : {0}", new object[]
			{
				this.m_InviteUser.Count
			});
		}
		this.IsFacebook = true;
	}

	public void InviteUser()
	{
		if (this.IsLoginAndPermissionCheck("InviteUser", new object[0]))
		{
			if (!this.m_bFacebookActive)
			{
				this.IsFacebook = false;
				return;
			}
			MsgHandler.Handle("FacebookFriendInviteDlgShow", new object[0]);
		}
		this.IsFacebook = true;
	}

	public void SyncGestID()
	{
		if (this.IsLoginAndPermissionCheck("SyncGestID", new object[0]))
		{
			if (!this.m_bFacebookActive)
			{
				this.IsFacebook = false;
				return;
			}
			MsgHandler.Handle("ConvertGuestID", new object[]
			{
				true
			});
		}
		this.IsFacebook = true;
	}

	public void SaveMethod(string MethodName, params object[] _datas)
	{
		TsLog.LogWarning("SaveMethodName = {0}", new object[]
		{
			MethodName
		});
		this.m_SaveMethodName = MethodName;
		this.m_SaveDatas = _datas;
	}

	public void LateUpdate()
	{
		switch (this.m_Step)
		{
		case FACEBOOK_STEP.LOGIN:
			if (this.isSessionValid())
			{
				this.GetMeFacebookData();
				this.m_Step = FACEBOOK_STEP.GETME;
			}
			break;
		case FACEBOOK_STEP.GETME:
			if (this.m_bFacebookActive && this.m_bSendSync)
			{
				this.GetFriends();
				this.m_Step = FACEBOOK_STEP.GETFRIEND;
			}
			break;
		case FACEBOOK_STEP.INVITE_FRIENDS:
		{
			if (!this.m_bFacebookActive)
			{
				this.m_Step = FACEBOOK_STEP.END;
				this.IsFacebook = false;
				return;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (this.m_InvateFriendsTime < realtimeSinceStartup)
			{
				this.m_InvateFriendsTime = realtimeSinceStartup + 0.1f;
				if (this.m_InviteUser.Count != 0)
				{
					FacebookUserData facebookUserData = this.m_InviteUser.Pop();
					MsgHandler.Handle("RequestFacebookFriendInvite", new object[]
					{
						facebookUserData.m_ID,
						facebookUserData.m_Name
					});
				}
				else
				{
					this.m_Step = FACEBOOK_STEP.END;
					TsLog.LogWarning("INVITE_FRIENDS == 0", new object[0]);
					this.IsFacebook = false;
				}
			}
			break;
		}
		case FACEBOOK_STEP.CALL_SAVE_METHOD:
			TsLog.LogWarning("CALL_SAVE_METHOD SaveMethodName = {0} m_bFacebookActive = {1}", new object[]
			{
				this.m_SaveMethodName,
				this.m_bFacebookActive
			});
			if (string.IsNullOrEmpty(this.m_SaveMethodName))
			{
				this._isFacebook = false;
				this.m_Step = FACEBOOK_STEP.END;
			}
			else
			{
				try
				{
					string saveMethodName = this.m_SaveMethodName;
					object[] saveDatas = this.m_SaveDatas;
					this.m_SaveMethodName = string.Empty;
					this.m_SaveDatas = null;
					if (this.m_bFacebookActive)
					{
						MethodInfo method = typeof(NmFacebookManager).GetMethod(saveMethodName);
						if (method != null)
						{
							method.Invoke(this, saveDatas);
						}
					}
				}
				catch (Exception ex)
				{
					TsLog.LogWarning("SaveMethod Failed!!\nError ={0}", new object[]
					{
						ex.Message
					});
				}
				finally
				{
					if (this.m_Step == FACEBOOK_STEP.CALL_SAVE_METHOD)
					{
						this.m_Step = FACEBOOK_STEP.END;
					}
				}
			}
			break;
		}
	}

	public void FacebookRequestLogin()
	{
		UnityEngine.Debug.LogWarning("FacebookRequestLogin!!!!");
		MsgHandler.Handle("RequestLogin", new object[0]);
	}

	private void reauthorizeWithPublishPermissions()
	{
		FacebookAndroid.loginWithPublishPermissions(new string[]
		{
			"publish_actions",
			"manage_friendlists"
		});
		TsLog.LogError("reauthorizeWithPublishPermissions!!!", new object[0]);
		this.m_Step = FACEBOOK_STEP.RERMISSION;
	}

	private bool IsLoginAndPermissionCheck(string MethodName, params object[] _datas)
	{
		if (!this.isSessionValid() || this.m_UserData.m_ID == string.Empty)
		{
			this.init();
			this.GetMeFacebookData();
			this.SaveMethod(MethodName, _datas);
			return false;
		}
		if (TsPlatform.IsAndroid && !this.m_bPublishPermission)
		{
			this.reauthorizeWithPublishPermissions();
			this.SaveMethod(MethodName, _datas);
			return false;
		}
		return true;
	}

	public void TestFacebookFriend()
	{
		this.m_FriendsData.Clear();
		for (int i = 0; i < 19; i++)
		{
			FacebookUserData facebookUserData = new FacebookUserData();
			bool installed = i % 2 == 1;
			facebookUserData.m_ID = i.ToString();
			if (i >= 1 && i <= 3)
			{
				facebookUserData.m_Name = string.Format("테스트유저{0}", i);
				facebookUserData.m_Installed = true;
			}
			else
			{
				facebookUserData.m_Name = string.Format("테스트유저{0}", i);
				facebookUserData.m_Installed = installed;
			}
			this.m_FriendsData.Add(facebookUserData.m_ID, facebookUserData);
		}
	}
}
