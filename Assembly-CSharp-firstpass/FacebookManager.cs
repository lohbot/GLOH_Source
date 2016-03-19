using GameMessage;
using Prime31;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

	public static event Action<string> dialogCompletedWithUrlEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			FacebookManager.dialogCompletedWithUrlEvent = (Action<string>)Delegate.Combine(FacebookManager.dialogCompletedWithUrlEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			FacebookManager.dialogCompletedWithUrlEvent = (Action<string>)Delegate.Remove(FacebookManager.dialogCompletedWithUrlEvent, value);
		}
	}

	public static event Action<P31Error> dialogFailedEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			FacebookManager.dialogFailedEvent = (Action<P31Error>)Delegate.Combine(FacebookManager.dialogFailedEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			FacebookManager.dialogFailedEvent = (Action<P31Error>)Delegate.Remove(FacebookManager.dialogFailedEvent, value);
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

	public static event Action reauthorizationSucceededEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			FacebookManager.reauthorizationSucceededEvent = (Action)Delegate.Combine(FacebookManager.reauthorizationSucceededEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			FacebookManager.reauthorizationSucceededEvent = (Action)Delegate.Remove(FacebookManager.reauthorizationSucceededEvent, value);
		}
	}

	public static event Action<P31Error> reauthorizationFailedEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			FacebookManager.reauthorizationFailedEvent = (Action<P31Error>)Delegate.Combine(FacebookManager.reauthorizationFailedEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			FacebookManager.reauthorizationFailedEvent = (Action<P31Error>)Delegate.Remove(FacebookManager.reauthorizationFailedEvent, value);
		}
	}

	public static event Action<Dictionary<string, object>> shareDialogSucceededEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			FacebookManager.shareDialogSucceededEvent = (Action<Dictionary<string, object>>)Delegate.Combine(FacebookManager.shareDialogSucceededEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			FacebookManager.shareDialogSucceededEvent = (Action<Dictionary<string, object>>)Delegate.Remove(FacebookManager.shareDialogSucceededEvent, value);
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

	public void dialogCompletedWithUrl(string url)
	{
		FacebookManager.dialogCompletedWithUrlEvent.fire(url);
	}

	public void dialogFailedWithError(string json)
	{
		FacebookManager.dialogFailedEvent.fire(P31Error.errorFromJson(json));
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

	public void reauthorizationSucceeded(string empty)
	{
		FacebookManager.reauthorizationSucceededEvent.fire();
	}

	public void reauthorizationFailed(string json)
	{
		FacebookManager.reauthorizationFailedEvent.fire(P31Error.errorFromJson(json));
	}

	public void shareDialogFailed(string json)
	{
		FacebookManager.shareDialogFailedEvent.fire(P31Error.errorFromJson(json));
	}

	public void shareDialogSucceeded(string json)
	{
		FacebookManager.shareDialogSucceededEvent.fire(json.dictionaryFromJson());
	}
}
