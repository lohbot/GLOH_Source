using GameMessage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prime31
{
	public class FacebookEventListener : MonoBehaviour
	{
		private void OnEnable()
		{
			FacebookManager.sessionOpenedEvent += new Action(this.sessionOpenedEvent);
			FacebookManager.loginFailedEvent += new Action<P31Error>(this.loginFailedEvent);
			FacebookManager.graphRequestCompletedEvent += new Action<object>(this.graphRequestCompletedEvent);
			FacebookManager.graphRequestFailedEvent += new Action<P31Error>(this.facebookCustomRequestFailed);
			FacebookManager.facebookComposerCompletedEvent += new Action<bool>(this.facebookComposerCompletedEvent);
			FacebookManager.shareDialogFailedEvent += new Action<P31Error>(this.shareDialogFailedEvent);
			FacebookManager.shareDialogSucceededEvent += new Action<string>(this.shareDialogSucceededEvent);
			FacebookManager.gameDialogFailedEvent += new Action<P31Error>(this.gameDialogFailedEvent);
			FacebookManager.gameDialogSucceededEvent += new Action<Dictionary<string, object>>(this.gameDialogSucceededEvent);
		}

		private void OnDisable()
		{
			FacebookManager.sessionOpenedEvent -= new Action(this.sessionOpenedEvent);
			FacebookManager.loginFailedEvent -= new Action<P31Error>(this.loginFailedEvent);
			FacebookManager.graphRequestCompletedEvent -= new Action<object>(this.graphRequestCompletedEvent);
			FacebookManager.graphRequestFailedEvent -= new Action<P31Error>(this.facebookCustomRequestFailed);
			FacebookManager.facebookComposerCompletedEvent -= new Action<bool>(this.facebookComposerCompletedEvent);
			FacebookManager.shareDialogFailedEvent -= new Action<P31Error>(this.shareDialogFailedEvent);
			FacebookManager.shareDialogSucceededEvent -= new Action<string>(this.shareDialogSucceededEvent);
			FacebookManager.gameDialogFailedEvent -= new Action<P31Error>(this.gameDialogFailedEvent);
			FacebookManager.gameDialogSucceededEvent -= new Action<Dictionary<string, object>>(this.gameDialogSucceededEvent);
		}

		private void sessionOpenedEvent()
		{
			Debug.Log("Successfully logged in to Facebook");
		}

		private void loginFailedEvent(P31Error error)
		{
			Debug.Log("Facebook login failed: " + error);
			MsgHandler.Handle("FacebookLoginFailed", new object[]
			{
				error.message
			});
		}

		private void dialogCompletedWithUrlEvent(string url)
		{
			Debug.Log("dialogCompletedWithUrlEvent: " + url);
			NmFacebookManager.instance.DialogCompleteEvent(url);
		}

		private void dialogFailedEvent(P31Error error)
		{
			Debug.Log("dialogFailedEvent: " + error);
		}

		private void facebokDialogCompleted()
		{
			Debug.Log("facebokDialogCompleted");
		}

		private void graphRequestCompletedEvent(object obj)
		{
			Debug.Log("graphRequestCompletedEvent");
			Utils.logObject(obj);
		}

		private void facebookCustomRequestFailed(P31Error error)
		{
			Debug.Log("facebookCustomRequestFailed failed: " + error);
		}

		private void facebookComposerCompletedEvent(bool didSucceed)
		{
			Debug.Log("facebookComposerCompletedEvent did succeed: " + didSucceed);
		}

		private void reauthorizationSucceededEvent()
		{
			Debug.LogWarning("reauthorizationSucceededEvent");
			NmFacebookManager.instance.PublishPermission = true;
			NmFacebookManager.instance.m_Step = FACEBOOK_STEP.CALL_SAVE_METHOD;
		}

		private void reauthorizationFailedEvent(P31Error error)
		{
			Debug.Log("reauthorizationFailedEvent: " + error);
		}

		private void shareDialogFailedEvent(P31Error error)
		{
			Debug.Log("shareDialogFailedEvent: " + error);
		}

		private void shareDialogSucceededEvent(Dictionary<string, object> dict)
		{
			Debug.Log("shareDialogSucceededEvent");
			Utils.logObject(dict);
		}

		private void Update()
		{
			NmFacebookManager.instance.LateUpdate();
		}

		private void shareDialogSucceededEvent(string postId)
		{
			Debug.Log("shareDialogSucceededEvent: " + postId);
		}

		private void gameDialogFailedEvent(P31Error error)
		{
			Debug.Log("gameDialogFailedEvent: " + error);
		}

		private void gameDialogSucceededEvent(Dictionary<string, object> dict)
		{
			Debug.Log("gameDialogSucceededEvent");
			Utils.logObject(dict);
		}
	}
}
