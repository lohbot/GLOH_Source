using Prime31;
using System;

public class FacebookAuthHelper
{
	public Action afterAuthAction;

	public bool requiresPublishPermissions;

	private static FacebookAuthHelper _instance;

	public FacebookAuthHelper(bool requiresPublishPermissions, Action afterAuthAction)
	{
		FacebookAuthHelper._instance = this;
		this.requiresPublishPermissions = requiresPublishPermissions;
		this.afterAuthAction = afterAuthAction;
		FacebookManager.sessionOpenedEvent += new Action(this.sessionOpenedEvent);
		FacebookManager.loginFailedEvent += new Action<P31Error>(this.loginFailedEvent);
		if (requiresPublishPermissions)
		{
			FacebookManager.reauthorizationSucceededEvent += new Action(this.reauthorizationSucceededEvent);
			FacebookManager.reauthorizationFailedEvent += new Action<P31Error>(this.reauthorizationFailedEvent);
		}
	}

	~FacebookAuthHelper()
	{
		this.cleanup();
	}

	public void cleanup()
	{
		if (this.afterAuthAction != null)
		{
			FacebookManager.sessionOpenedEvent -= new Action(this.sessionOpenedEvent);
			FacebookManager.loginFailedEvent -= new Action<P31Error>(this.loginFailedEvent);
			if (this.requiresPublishPermissions)
			{
				FacebookManager.reauthorizationSucceededEvent -= new Action(this.reauthorizationSucceededEvent);
				FacebookManager.reauthorizationFailedEvent -= new Action<P31Error>(this.reauthorizationFailedEvent);
			}
		}
		FacebookAuthHelper._instance = null;
	}

	public void start()
	{
		FacebookAndroid.login();
	}

	private void sessionOpenedEvent()
	{
		if (this.requiresPublishPermissions && !FacebookAndroid.getSessionPermissions().Contains("publish_stream"))
		{
			FacebookAndroid.reauthorizeWithPublishPermissions(new string[]
			{
				"publish_actions",
				"publish_stream"
			}, FacebookSessionDefaultAudience.Everyone);
		}
		else
		{
			this.afterAuthAction();
			this.cleanup();
		}
	}

	private void loginFailedEvent(P31Error error)
	{
		this.cleanup();
	}

	private void reauthorizationSucceededEvent()
	{
		this.afterAuthAction();
		this.cleanup();
	}

	private void reauthorizationFailedEvent(P31Error error)
	{
		this.cleanup();
	}
}
