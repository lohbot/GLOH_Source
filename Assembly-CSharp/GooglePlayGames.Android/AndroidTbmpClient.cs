using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidTbmpClient : ITurnBasedMultiplayerClient
	{
		private class ResultProxy : AndroidJavaProxy
		{
			private AndroidTbmpClient mOwner;

			private string mMethod = "?";

			private Action<bool> mSuccessCallback;

			private Action<bool, TurnBasedMatch> mMatchCallback;

			private List<int> mSuccessCodes = new List<int>();

			internal ResultProxy(AndroidTbmpClient owner, string method) : base("com.google.android.gms.common.api.ResultCallback")
			{
				this.mOwner = owner;
				this.mSuccessCodes.Add(0);
				this.mSuccessCodes.Add(5);
				this.mSuccessCodes.Add(3);
				this.mMethod = method;
			}

			public void SetSuccessCallback(Action<bool> callback)
			{
				this.mSuccessCallback = callback;
			}

			public void SetMatchCallback(Action<bool, TurnBasedMatch> callback)
			{
				this.mMatchCallback = callback;
			}

			public void AddSuccessCodes(params int[] codes)
			{
				for (int i = 0; i < codes.Length; i++)
				{
					int item = codes[i];
					this.mSuccessCodes.Add(item);
				}
			}

			public void onResult(AndroidJavaObject result)
			{
				Logger.d("ResultProxy got result for method: " + this.mMethod);
				int statusCode = JavaUtil.GetStatusCode(result);
				bool isSuccess = this.mSuccessCodes.Contains(statusCode);
				TurnBasedMatch match = null;
				if (isSuccess)
				{
					Logger.d(string.Concat(new object[]
					{
						"SUCCESS result from method ",
						this.mMethod,
						": ",
						statusCode
					}));
					if (this.mMatchCallback != null)
					{
						Logger.d("Attempting to get match from result of " + this.mMethod);
						AndroidJavaObject androidJavaObject = JavaUtil.CallNullSafeObjectMethod(result, "getMatch", new object[0]);
						if (androidJavaObject != null)
						{
							Logger.d("Successfully got match from result of " + this.mMethod);
							match = JavaUtil.ConvertMatch(this.mOwner.mClient.PlayerId, androidJavaObject);
							androidJavaObject.Dispose();
						}
						else
						{
							Logger.w("Got a NULL match from result of " + this.mMethod);
						}
					}
				}
				else
				{
					Logger.w(string.Concat(new object[]
					{
						"ERROR result from ",
						this.mMethod,
						": ",
						statusCode
					}));
				}
				if (this.mSuccessCallback != null)
				{
					Logger.d(string.Concat(new object[]
					{
						"Invoking success callback (success=",
						isSuccess,
						") for result of method ",
						this.mMethod
					}));
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						this.mSuccessCallback(isSuccess);
					});
				}
				if (this.mMatchCallback != null)
				{
					Logger.d(string.Concat(new object[]
					{
						"Invoking match callback for result of method ",
						this.mMethod,
						": (success=",
						isSuccess,
						", match=",
						(match != null) ? match.ToString() : "(null)"
					}));
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						this.mMatchCallback(isSuccess, match);
					});
				}
			}
		}

		private class SelectOpponentsProxy : AndroidJavaProxy
		{
			private AndroidTbmpClient mOwner;

			private Action<bool, TurnBasedMatch> mCallback;

			private int mVariant;

			internal SelectOpponentsProxy(AndroidTbmpClient owner, Action<bool, TurnBasedMatch> callback, int variant) : base("com.google.example.games.pluginsupport.SelectOpponentsHelperActivity$Listener")
			{
				this.mOwner = owner;
				this.mCallback = callback;
				this.mVariant = variant;
			}

			public void onSelectOpponentsResult(bool success, AndroidJavaObject opponents, bool hasAutoMatch, AndroidJavaObject autoMatchCriteria)
			{
				this.mOwner.OnSelectOpponentsResult(success, opponents, hasAutoMatch, autoMatchCriteria, this.mCallback, this.mVariant);
			}
		}

		private class InvitationInboxProxy : AndroidJavaProxy
		{
			private AndroidTbmpClient mOwner;

			private Action<bool, TurnBasedMatch> mCallback;

			internal InvitationInboxProxy(AndroidTbmpClient owner, Action<bool, TurnBasedMatch> callback) : base("com.google.example.games.pluginsupport.InvitationInboxHelperActivity$Listener")
			{
				this.mOwner = owner;
				this.mCallback = callback;
			}

			public void onInvitationInboxResult(bool success, string invitationId)
			{
				this.mOwner.OnInvitationInboxResult(success, invitationId, this.mCallback);
			}

			public void onTurnBasedMatch(AndroidJavaObject match)
			{
				this.mOwner.OnInvitationInboxTurnBasedMatch(match, this.mCallback);
			}
		}

		private AndroidClient mClient;

		private int mMaxMatchDataSize;

		private TurnBasedMatch mMatchFromNotification;

		private MatchDelegate mMatchDelegate;

		internal AndroidTbmpClient(AndroidClient client)
		{
			this.mClient = client;
		}

		public void OnSignInSucceeded()
		{
			Logger.d("AndroidTbmpClient.OnSignInSucceeded");
			Logger.d("Querying for max match data size...");
			this.mMaxMatchDataSize = this.mClient.GHManager.CallGmsApi<int>("games.Games", "TurnBasedMultiplayer", "getMaxMatchDataSize", new object[0]);
			Logger.d("Max match data size: " + this.mMaxMatchDataSize);
		}

		public void CreateQuickMatch(int minOpponents, int maxOpponents, int variant, Action<bool, TurnBasedMatch> callback)
		{
			Logger.d(string.Format("AndroidTbmpClient.CreateQuickMatch, opponents {0}-{1}, var {2}", minOpponents, maxOpponents, variant));
			this.mClient.CallClientApi("tbmp create quick game", delegate
			{
				AndroidTbmpClient.ResultProxy resultProxy = new AndroidTbmpClient.ResultProxy(this, "createMatch");
				resultProxy.SetMatchCallback(callback);
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.TbmpUtils");
				using (AndroidJavaObject androidJavaObject = @class.CallStatic<AndroidJavaObject>("createQuickMatch", new object[]
				{
					this.mClient.GHManager.GetApiClient(),
					minOpponents,
					maxOpponents,
					variant
				}))
				{
					androidJavaObject.Call("setResultCallback", new object[]
					{
						resultProxy
					});
				}
			}, delegate(bool success)
			{
				if (!success)
				{
					Logger.w("Failed to create tbmp quick match: client disconnected.");
					if (callback != null)
					{
						callback(false, null);
					}
				}
			});
		}

		public void CreateWithInvitationScreen(int minOpponents, int maxOpponents, int variant, Action<bool, TurnBasedMatch> callback)
		{
			Logger.d(string.Format("AndroidTbmpClient.CreateWithInvitationScreen, opponents {0}-{1}, variant {2}", minOpponents, maxOpponents, variant));
			this.mClient.CallClientApi("tbmp launch invitation screen", delegate
			{
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.SelectOpponentsHelperActivity");
				@class.CallStatic("launch", new object[]
				{
					false,
					this.mClient.GetActivity(),
					new AndroidTbmpClient.SelectOpponentsProxy(this, callback, variant),
					Logger.DebugLogEnabled,
					minOpponents,
					maxOpponents
				});
			}, delegate(bool success)
			{
				if (!success)
				{
					Logger.w("Failed to create tbmp w/ invite screen: client disconnected.");
					if (callback != null)
					{
						callback(false, null);
					}
				}
			});
		}

		public void AcceptFromInbox(Action<bool, TurnBasedMatch> callback)
		{
			Logger.d(string.Format("AndroidTbmpClient.AcceptFromInbox", new object[0]));
			this.mClient.CallClientApi("tbmp launch inbox", delegate
			{
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.InvitationInboxHelperActivity");
				@class.CallStatic("launch", new object[]
				{
					false,
					this.mClient.GetActivity(),
					new AndroidTbmpClient.InvitationInboxProxy(this, callback),
					Logger.DebugLogEnabled
				});
			}, delegate(bool success)
			{
				if (!success)
				{
					Logger.w("Failed to accept tbmp w/ inbox: client disconnected.");
					if (callback != null)
					{
						callback(false, null);
					}
				}
			});
		}

		public void AcceptInvitation(string invitationId, Action<bool, TurnBasedMatch> callback)
		{
			Logger.d("AndroidTbmpClient.AcceptInvitation invitationId=" + invitationId);
			this.TbmpApiCall("accept invitation", "acceptInvitation", null, callback, new object[]
			{
				invitationId
			});
		}

		public void DeclineInvitation(string invitationId)
		{
			Logger.d("AndroidTbmpClient.DeclineInvitation, invitationId=" + invitationId);
			this.TbmpApiCall("decline invitation", "declineInvitation", null, null, new object[]
			{
				invitationId
			});
		}

		private void TbmpApiCall(string simpleDesc, string methodName, Action<bool> callback, Action<bool, TurnBasedMatch> tbmpCallback, params object[] args)
		{
			this.mClient.CallClientApi(simpleDesc, delegate
			{
				AndroidTbmpClient.ResultProxy resultProxy = new AndroidTbmpClient.ResultProxy(this, methodName);
				if (callback != null)
				{
					resultProxy.SetSuccessCallback(callback);
				}
				if (tbmpCallback != null)
				{
					resultProxy.SetMatchCallback(tbmpCallback);
				}
				this.mClient.GHManager.CallGmsApiWithResult("games.Games", "TurnBasedMultiplayer", methodName, resultProxy, args);
			}, delegate(bool success)
			{
				if (!success)
				{
					Logger.w("Failed to " + simpleDesc + ": client disconnected.");
					if (callback != null)
					{
						callback(false);
					}
				}
			});
		}

		public void TakeTurn(string matchId, byte[] data, string pendingParticipantId, Action<bool> callback)
		{
			Logger.d(string.Format("AndroidTbmpClient.TakeTurn matchId={0}, data={1}, pending={2}", matchId, (data != null) ? ("[" + data.Length + "bytes]") : "(null)", pendingParticipantId));
			this.TbmpApiCall("tbmp take turn", "takeTurn", callback, null, new object[]
			{
				matchId,
				data,
				pendingParticipantId
			});
		}

		public int GetMaxMatchDataSize()
		{
			return this.mMaxMatchDataSize;
		}

		public void Finish(string matchId, byte[] data, MatchOutcome outcome, Action<bool> callback)
		{
			Logger.d(string.Format("AndroidTbmpClient.Finish matchId={0}, data={1} outcome={2}", matchId, (data != null) ? (data.Length + " bytes") : "(null)", outcome));
			Logger.d("Preparing list of participant results as Android ArrayList.");
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.ArrayList", new object[0]);
			if (outcome != null)
			{
				foreach (string current in outcome.ParticipantIds)
				{
					Logger.d("Converting participant result to Android object: " + current);
					AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.google.android.gms.games.multiplayer.ParticipantResult", new object[]
					{
						current,
						JavaUtil.GetAndroidParticipantResult(outcome.GetResultFor(current)),
						outcome.GetPlacementFor(current)
					});
					Logger.d("Adding participant result to Android ArrayList.");
					androidJavaObject.Call<bool>("add", new object[]
					{
						androidJavaObject2
					});
					androidJavaObject2.Dispose();
				}
			}
			this.TbmpApiCall("tbmp finish w/ outcome", "finishMatch", callback, null, new object[]
			{
				matchId,
				data,
				androidJavaObject
			});
		}

		public void AcknowledgeFinished(string matchId, Action<bool> callback)
		{
			Logger.d("AndroidTbmpClient.AcknowledgeFinished, matchId=" + matchId);
			this.TbmpApiCall("tbmp ack finish", "finishMatch", callback, null, new object[]
			{
				matchId
			});
		}

		public void Leave(string matchId, Action<bool> callback)
		{
			Logger.d("AndroidTbmpClient.Leave, matchId=" + matchId);
			this.TbmpApiCall("tbmp leave", "leaveMatch", callback, null, new object[]
			{
				matchId
			});
		}

		public void LeaveDuringTurn(string matchId, string pendingParticipantId, Action<bool> callback)
		{
			Logger.d("AndroidTbmpClient.LeaveDuringTurn, matchId=" + matchId + ", pending=" + pendingParticipantId);
			this.TbmpApiCall("tbmp leave during turn", "leaveMatchDuringTurn", callback, null, new object[]
			{
				matchId,
				pendingParticipantId
			});
		}

		public void Cancel(string matchId, Action<bool> callback)
		{
			Logger.d("AndroidTbmpClient.Cancel, matchId=" + matchId);
			this.TbmpApiCall("tbmp cancel", "cancelMatch", callback, null, new object[]
			{
				matchId
			});
		}

		public void Rematch(string matchId, Action<bool, TurnBasedMatch> callback)
		{
			Logger.d("AndroidTbmpClient.Rematch, matchId=" + matchId);
			this.TbmpApiCall("tbmp rematch", "rematch", null, callback, new object[]
			{
				matchId
			});
		}

		public void RegisterMatchDelegate(MatchDelegate deleg)
		{
			Logger.d("AndroidTbmpClient.RegisterMatchDelegate");
			if (deleg == null)
			{
				Logger.w("Can't register a null match delegate.");
				return;
			}
			this.mMatchDelegate = deleg;
			if (this.mMatchFromNotification != null)
			{
				Logger.d("Delivering pending match to the newly registered delegate.");
				TurnBasedMatch match = this.mMatchFromNotification;
				this.mMatchFromNotification = null;
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					deleg(match, true);
				});
			}
		}

		private void OnSelectOpponentsResult(bool success, AndroidJavaObject opponents, bool hasAutoMatch, AndroidJavaObject autoMatchCriteria, Action<bool, TurnBasedMatch> callback, int variant)
		{
			Logger.d(string.Concat(new object[]
			{
				"AndroidTbmpClient.OnSelectOpponentsResult, success=",
				success,
				", hasAutoMatch=",
				hasAutoMatch
			}));
			if (!success)
			{
				Logger.w("Tbmp select opponents dialog terminated with failure.");
				if (callback != null)
				{
					Logger.d("Reporting select-opponents dialog failure to callback.");
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						callback(false, null);
					});
				}
				return;
			}
			Logger.d("Creating TBMP match from opponents received from dialog.");
			this.mClient.CallClientApi("create match w/ opponents from dialog", delegate
			{
				AndroidTbmpClient.ResultProxy resultProxy = new AndroidTbmpClient.ResultProxy(this, "createMatch");
				resultProxy.SetMatchCallback(callback);
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.TbmpUtils");
				using (AndroidJavaObject androidJavaObject = @class.CallStatic<AndroidJavaObject>("create", new object[]
				{
					this.mClient.GHManager.GetApiClient(),
					opponents,
					variant,
					(!hasAutoMatch) ? null : autoMatchCriteria
				}))
				{
					androidJavaObject.Call("setResultCallback", new object[]
					{
						resultProxy
					});
				}
			}, delegate(bool ok)
			{
				if (!ok)
				{
					Logger.w("Failed to create match w/ opponents from dialog: client disconnected.");
					if (callback != null)
					{
						callback(false, null);
					}
				}
			});
		}

		private void OnInvitationInboxResult(bool success, string invitationId, Action<bool, TurnBasedMatch> callback)
		{
			Logger.d(string.Concat(new object[]
			{
				"AndroidTbmpClient.OnInvitationInboxResult, success=",
				success,
				", invitationId=",
				invitationId
			}));
			if (!success)
			{
				Logger.w("Tbmp invitation inbox returned failure result.");
				if (callback != null)
				{
					Logger.d("Reporting tbmp invitation inbox failure to callback.");
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						callback(false, null);
					});
				}
				return;
			}
			Logger.d("Accepting invite received from inbox: " + invitationId);
			this.TbmpApiCall("accept invite returned from inbox", "acceptInvitation", null, callback, new object[]
			{
				invitationId
			});
		}

		private void OnInvitationInboxTurnBasedMatch(AndroidJavaObject matchObj, Action<bool, TurnBasedMatch> callback)
		{
			Logger.d("AndroidTbmpClient.OnInvitationTurnBasedMatch");
			Logger.d("Converting received match to our format...");
			TurnBasedMatch match = JavaUtil.ConvertMatch(this.mClient.PlayerId, matchObj);
			Logger.d("Resulting match: " + match);
			if (callback != null)
			{
				Logger.d("Invoking match callback w/ success.");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(true, match);
				});
			}
		}

		internal void HandleMatchFromNotification(TurnBasedMatch match)
		{
			Logger.d("AndroidTbmpClient.HandleMatchFromNotification");
			Logger.d("Got match from notification: " + match);
			if (this.mMatchDelegate != null)
			{
				Logger.d("Delivering match directly to match delegate.");
				MatchDelegate del = this.mMatchDelegate;
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					del(match, true);
				});
			}
			else
			{
				Logger.d("Since we have no match delegate, holding on to the match until we have one.");
				this.mMatchFromNotification = match;
			}
		}
	}
}
