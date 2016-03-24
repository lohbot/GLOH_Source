using GameMessage;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Prime31
{
	public class FacebookManager : AbstractManager
	{
		public static event Action sessionOpenedEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FacebookManager.sessionOpenedEvent = (Action)Delegate.Combine(FacebookManager.sessionOpenedEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FacebookManager.sessionOpenedEvent = (Action)Delegate.Remove(FacebookManager.sessionOpenedEvent, value);
			}
		}

		public static event Action preLoginSucceededEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FacebookManager.preLoginSucceededEvent = (Action)Delegate.Combine(FacebookManager.preLoginSucceededEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FacebookManager.preLoginSucceededEvent = (Action)Delegate.Remove(FacebookManager.preLoginSucceededEvent, value);
			}
		}

		public static event Action<P31Error> loginFailedEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FacebookManager.loginFailedEvent = (Action<P31Error>)Delegate.Combine(FacebookManager.loginFailedEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FacebookManager.loginFailedEvent = (Action<P31Error>)Delegate.Remove(FacebookManager.loginFailedEvent, value);
			}
		}

		public static event Action<object> graphRequestCompletedEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FacebookManager.graphRequestCompletedEvent = (Action<object>)Delegate.Combine(FacebookManager.graphRequestCompletedEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FacebookManager.graphRequestCompletedEvent = (Action<object>)Delegate.Remove(FacebookManager.graphRequestCompletedEvent, value);
			}
		}

		public static event Action<P31Error> graphRequestFailedEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FacebookManager.graphRequestFailedEvent = (Action<P31Error>)Delegate.Combine(FacebookManager.graphRequestFailedEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FacebookManager.graphRequestFailedEvent = (Action<P31Error>)Delegate.Remove(FacebookManager.graphRequestFailedEvent, value);
			}
		}

		public static event Action<bool> facebookComposerCompletedEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FacebookManager.facebookComposerCompletedEvent = (Action<bool>)Delegate.Combine(FacebookManager.facebookComposerCompletedEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FacebookManager.facebookComposerCompletedEvent = (Action<bool>)Delegate.Remove(FacebookManager.facebookComposerCompletedEvent, value);
			}
		}

		public static event Action<string> shareDialogSucceededEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FacebookManager.shareDialogSucceededEvent = (Action<string>)Delegate.Combine(FacebookManager.shareDialogSucceededEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FacebookManager.shareDialogSucceededEvent = (Action<string>)Delegate.Remove(FacebookManager.shareDialogSucceededEvent, value);
			}
		}

		public static event Action<P31Error> shareDialogFailedEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FacebookManager.shareDialogFailedEvent = (Action<P31Error>)Delegate.Combine(FacebookManager.shareDialogFailedEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FacebookManager.shareDialogFailedEvent = (Action<P31Error>)Delegate.Remove(FacebookManager.shareDialogFailedEvent, value);
			}
		}

		public static event Action<Dictionary<string, object>> gameDialogSucceededEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FacebookManager.gameDialogSucceededEvent = (Action<Dictionary<string, object>>)Delegate.Combine(FacebookManager.gameDialogSucceededEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FacebookManager.gameDialogSucceededEvent = (Action<Dictionary<string, object>>)Delegate.Remove(FacebookManager.gameDialogSucceededEvent, value);
			}
		}

		public static event Action<P31Error> gameDialogFailedEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FacebookManager.gameDialogFailedEvent = (Action<P31Error>)Delegate.Combine(FacebookManager.gameDialogFailedEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FacebookManager.gameDialogFailedEvent = (Action<P31Error>)Delegate.Remove(FacebookManager.gameDialogFailedEvent, value);
			}
		}

		static FacebookManager()
		{
			AbstractManager.initialize(typeof(FacebookManager));
		}

		public void sessionOpened(string accessToken)
		{
			FacebookManager.preLoginSucceededEvent.fire();
			Facebook.instance.accessToken = accessToken;
			FacebookManager.sessionOpenedEvent.fire();
		}

		public void loginFailed(string json)
		{
			FacebookManager.loginFailedEvent.fire(P31Error.errorFromJson(json));
			MsgHandler.Handle("ShowPlatformLogin", new object[0]);
		}

		public void graphRequestCompleted(string json)
		{
			if (FacebookManager.graphRequestCompletedEvent != null)
			{
				object param = Json.decode(json);
				FacebookManager.graphRequestCompletedEvent.fire(param);
			}
		}

		public void graphRequestFailed(string json)
		{
			FacebookManager.graphRequestFailedEvent.fire(P31Error.errorFromJson(json));
		}

		public void facebookComposerCompleted(string result)
		{
			FacebookManager.facebookComposerCompletedEvent.fire(result == "1");
		}

		public void shareDialogFailed(string json)
		{
			FacebookManager.shareDialogFailedEvent.fire(P31Error.errorFromJson(json));
		}

		public void shareDialogSucceeded(string postId)
		{
			FacebookManager.shareDialogSucceededEvent.fire(postId);
		}

		public void gameDialogFailed(string json)
		{
			FacebookManager.gameDialogFailedEvent.fire(P31Error.errorFromJson(json));
		}

		public void gameDialogSucceeded(string json)
		{
			FacebookManager.gameDialogSucceededEvent.fire(json.dictionaryFromJson());
		}
	}
}
