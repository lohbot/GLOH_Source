using Prime31;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

public class Facebook : P31RestKit
{
	public string accessToken;

	public string appAccessToken;

	public bool useSessionBabysitter = true;

	private static Facebook _instance;

	public static Facebook instance
	{
		get
		{
			if (Facebook._instance == null)
			{
				Facebook._instance = new Facebook();
			}
			return Facebook._instance;
		}
	}

	public Facebook()
	{
		this._baseUrl = "https://graph.facebook.com/";
		this.forceJsonResponse = true;
	}

	protected override IEnumerator send(string path, HTTPVerb httpVerb, Dictionary<string, object> parameters, Action<string, object> onComplete)
	{
		if (parameters == null)
		{
			parameters = new Dictionary<string, object>();
		}
		if (!parameters.ContainsKey("access_token"))
		{
			parameters.Add("access_token", this.accessToken);
		}
		if (httpVerb == HTTPVerb.PUT || httpVerb == HTTPVerb.DELETE)
		{
			parameters.Add("method", httpVerb.ToString());
		}
		return base.send(path, httpVerb, parameters, onComplete);
	}

	protected bool shouldSendRequest()
	{
		if (!this.useSessionBabysitter)
		{
			return true;
		}
		try
		{
			Type type = typeof(Facebook).Assembly.GetType("FacebookAndroid");
			if (type != null)
			{
				MethodInfo method = type.GetMethod("isSessionValid", BindingFlags.Static | BindingFlags.Public);
				object obj = method.Invoke(null, null);
				return (bool)obj;
			}
		}
		catch (Exception)
		{
		}
		return true;
	}

	public void prepareForMetroUse(GameObject go, MonoBehaviour mb)
	{
		UnityEngine.Object.DontDestroyOnLoad(go);
		this.surrogateGameObject = go;
		base.surrogateMonobehaviour = mb;
	}

	public void graphRequest(string path, Action<string, object> completionHandler)
	{
		this.graphRequest(path, HTTPVerb.GET, completionHandler);
	}

	public void graphRequest(string path, HTTPVerb verb, Action<string, object> completionHandler)
	{
		this.graphRequest(path, verb, null, completionHandler);
	}

	public void graphRequest(string path, HTTPVerb verb, Dictionary<string, object> parameters, Action<string, object> completionHandler)
	{
		if (this.shouldSendRequest())
		{
			base.surrogateMonobehaviour.StartCoroutine(this.send(path, verb, parameters, completionHandler));
		}
		else
		{
			try
			{
				Type type = typeof(Facebook).Assembly.GetType("FacebookAndroid");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("babysitRequest", BindingFlags.Static | BindingFlags.NonPublic);
					if (method != null)
					{
						Action action = delegate
						{
							this.surrogateMonobehaviour.StartCoroutine(this.send(path, verb, parameters, completionHandler));
						};
						method.Invoke(null, new object[]
						{
							verb == HTTPVerb.POST,
							action
						});
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	public void graphRequestBatch(IEnumerable<FacebookBatchRequest> requests, Action<string, object> completionHandler)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		foreach (FacebookBatchRequest current in requests)
		{
			list.Add(current.requestDictionary());
		}
		dictionary.Add("batch", Json.encode(list));
		base.surrogateMonobehaviour.StartCoroutine(this.send(string.Empty, HTTPVerb.POST, dictionary, completionHandler));
	}

	public void fetchProfileImageForUserId(string userId, Action<Texture2D> completionHandler)
	{
		string url = "http://graph.facebook.com/" + userId + "/picture?type=large";
		base.surrogateMonobehaviour.StartCoroutine(this.fetchImageAtUrl(url, completionHandler));
	}

	[DebuggerHidden]
	public IEnumerator fetchImageAtUrl(string url, Action<Texture2D> completionHandler)
	{
		Facebook.<fetchImageAtUrl>c__Iterator1C <fetchImageAtUrl>c__Iterator1C = new Facebook.<fetchImageAtUrl>c__Iterator1C();
		<fetchImageAtUrl>c__Iterator1C.url = url;
		<fetchImageAtUrl>c__Iterator1C.completionHandler = completionHandler;
		<fetchImageAtUrl>c__Iterator1C.<$>url = url;
		<fetchImageAtUrl>c__Iterator1C.<$>completionHandler = completionHandler;
		return <fetchImageAtUrl>c__Iterator1C;
	}

	public void postMessage(string message, Action<string, object> completionHandler)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>
		{
			{
				"message",
				message
			}
		};
		this.graphRequest("me/feed", HTTPVerb.POST, parameters, completionHandler);
	}

	public void postMessageWithLink(string message, string link, string linkName, Action<string, object> completionHandler)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>
		{
			{
				"message",
				message
			},
			{
				"link",
				link
			},
			{
				"name",
				linkName
			}
		};
		this.graphRequest("me/feed", HTTPVerb.POST, parameters, completionHandler);
	}

	public void postMessageWithLinkAndLinkToImage(string message, string link, string linkName, string linkToImage, string caption, string description, Action<string, object> completionHandler)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>
		{
			{
				"message",
				message
			},
			{
				"link",
				link
			},
			{
				"name",
				linkName
			},
			{
				"picture",
				linkToImage
			},
			{
				"caption",
				caption
			},
			{
				"description",
				description
			}
		};
		this.graphRequest("me/feed", HTTPVerb.POST, parameters, completionHandler);
	}

	public void postImage(byte[] image, string message, Action<string, object> completionHandler)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>
		{
			{
				"picture",
				image
			},
			{
				"message",
				message
			}
		};
		this.graphRequest("me/photos", HTTPVerb.POST, parameters, completionHandler);
	}

	public void postImageToAlbum(byte[] image, string caption, string albumId, Action<string, object> completionHandler)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>
		{
			{
				"picture",
				image
			},
			{
				"message",
				caption
			}
		};
		this.graphRequest(albumId, HTTPVerb.POST, parameters, completionHandler);
	}

	public void getMe(Action<string, FacebookMeResult> completionHandler)
	{
		this.graphRequest("me", delegate(string error, object obj)
		{
			if (completionHandler == null)
			{
				return;
			}
			if (error != null)
			{
				completionHandler(error, null);
			}
			else
			{
				completionHandler(null, Json.decodeObject<FacebookMeResult>(obj, null));
			}
		});
	}

	public void getFriends(Action<string, FacebookFriendsResult> completionHandler)
	{
		this.graphRequest("me/friends?fields=installed,name", delegate(string error, object obj)
		{
			if (completionHandler == null)
			{
				return;
			}
			if (error != null)
			{
				completionHandler(error, null);
			}
			else
			{
				completionHandler(null, Json.decodeObject<FacebookFriendsResult>(obj, null));
			}
		});
	}

	public void extendAccessToken(string appId, string appSecret, Action<DateTime?> completionHandler)
	{
		if (Facebook.instance.accessToken == null)
		{
			UnityEngine.Debug.LogError("There is no access token to extend. The user must be autenticated before attempting to extend their access token");
			return;
		}
		Dictionary<string, object> parameters = new Dictionary<string, object>
		{
			{
				"client_id",
				appId
			},
			{
				"client_secret",
				appSecret
			},
			{
				"grant_type",
				"fb_exchange_token"
			},
			{
				"fb_exchange_token",
				Facebook.instance.accessToken
			}
		};
		base.get("oauth/access_token", parameters, delegate(string error, object obj)
		{
			if (obj is string)
			{
				string text = obj as string;
				if (text.StartsWith("access_token="))
				{
					Dictionary<string, string> dictionary = text.parseQueryString();
					Facebook.instance.accessToken = dictionary["access_token"];
					double value = double.Parse(dictionary["expires"]);
					completionHandler(new DateTime?(DateTime.Now.AddSeconds(value)));
				}
				else
				{
					UnityEngine.Debug.LogError("error extending access token: " + text);
					completionHandler(null);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("error extending access token: " + error);
				completionHandler(null);
			}
		});
	}

	public void checkSessionValidityOnServer(Action<bool> completionHandler)
	{
		base.get("me", delegate(string error, object obj)
		{
			if (error == null && obj != null && obj is IDictionary)
			{
				completionHandler(true);
			}
			else
			{
				completionHandler(false);
			}
		});
	}

	public void getSessionPermissionsOnServer(Action<string, List<string>> completionHandler)
	{
		base.get("me/permissions", delegate(string error, object obj)
		{
			if (error == null && obj != null && obj is IDictionary)
			{
				IDictionary dictionary = obj as IDictionary;
				IList list = dictionary["data"] as IList;
				IDictionary dictionary2 = list[0] as IDictionary;
				List<string> list2 = new List<string>();
				foreach (DictionaryEntry dictionaryEntry in dictionary2)
				{
					if (dictionaryEntry.Value.ToString() == "1")
					{
						list2.Add(dictionaryEntry.Key.ToString());
					}
				}
				completionHandler(null, list2);
			}
			else
			{
				completionHandler(error, null);
			}
		});
	}

	public void getAppAccessToken(string appId, string appSecret, Action<string> completionHandler)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>
		{
			{
				"client_id",
				appId
			},
			{
				"client_secret",
				appSecret
			},
			{
				"grant_type",
				"client_credentials"
			}
		};
		base.get("oauth/access_token", parameters, delegate(string error, object obj)
		{
			if (obj is string)
			{
				string text = obj as string;
				if (text.StartsWith("access_token="))
				{
					this.appAccessToken = text.Replace("access_token=", string.Empty);
					completionHandler(this.appAccessToken);
				}
				else
				{
					completionHandler(null);
				}
			}
			else
			{
				completionHandler(null);
			}
		});
	}

	public void postScore(int score, Action<bool> completionHandler)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>
		{
			{
				"score",
				score.ToString()
			}
		};
		base.post("me/scores", parameters, delegate(string error, object obj)
		{
			if (error == null && obj is string)
			{
				completionHandler(((string)obj).ToLower() == "true");
			}
			else
			{
				completionHandler(false);
			}
		});
	}

	public void getScores(string userId, Action<string, object> onComplete)
	{
		string path = userId + "/scores";
		this.graphRequest(path, onComplete);
	}
}
