using System;

namespace Prime31
{
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
			}
			FacebookAuthHelper._instance = null;
		}

		public void start()
		{
			FacebookAndroid.login();
		}

		private void sessionOpenedEvent()
		{
			if (this.requiresPublishPermissions && !FacebookAndroid.getSessionPermissions().Contains("publish_actions"))
			{
				FacebookAndroid.loginWithPublishPermissions(new string[]
				{
					"publish_actions"
				});
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
	}
}
