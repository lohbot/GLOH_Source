using System;
using UnityEngine;

namespace NPatch
{
	internal class DownloderAsyncToWWW : DownloderAsync
	{
		private NPatchDownloderAsyncBehaviour behaviour;

		public DownloderAsyncToWWW()
		{
			base.ErrorString = string.Empty;
			base.ErrorLevel = ERRORLEVEL.SUCCESS;
		}

		public DownloderAsyncToWWW(int limitSec, int retryTerm)
		{
			this.m_timeoutLimitSec = limitSec;
			this.m_retryTerm = retryTerm;
			this.m_reconnectCountMax = this.m_timeoutLimitSec / this.m_retryTerm - 1;
			base.isTimeout = false;
			base.isNeedRetry = false;
		}

		public override bool DownloadFile(string url, string filename, Action<ERRORLEVEL> _OnCompleted)
		{
			DownloderAsync.__DebugOutput("AsyncDownloder.DownloadFile : " + url);
			base.ErrorString = string.Empty;
			base.ErrorLevel = ERRORLEVEL.SUCCESS;
			this.OnCompletedFile = _OnCompleted;
			url = url.Replace("//", "/");
			url = url.Replace("http:/", "http://");
			this.behaviour = NPatchLauncherBehaviour.Owner.GetComponent<NPatchDownloderAsyncBehaviour>();
			if (this.behaviour == null)
			{
				this.behaviour = NPatchLauncherBehaviour.Owner.AddComponent<NPatchDownloderAsyncBehaviour>();
			}
			this.behaviour.StartCoroutine(this.behaviour.DownloadFileRoutine(url, filename, this.OnCompletedFile, new Action(this.OnPrograss_File)));
			return true;
		}

		public override bool DownloadString(string url, Action<ERRORLEVEL, string> _OnCompleted)
		{
			base.ErrorString = string.Empty;
			base.ErrorLevel = ERRORLEVEL.SUCCESS;
			this.OnCompletedString = _OnCompleted;
			url = url.Replace("//", "/");
			url = url.Replace("http:/", "http://");
			this.behaviour = NPatchLauncherBehaviour.Owner.GetComponent<NPatchDownloderAsyncBehaviour>();
			if (this.behaviour == null)
			{
				this.behaviour = NPatchLauncherBehaviour.Owner.AddComponent<NPatchDownloderAsyncBehaviour>();
			}
			this.behaviour.StartCoroutine(this.behaviour.DownloadStringRoutine(url, this.OnCompletedString));
			return true;
		}

		public override long GetDownloadedBytes(string zipFile, int fileSize)
		{
			return (long)((int)(this.behaviour.wwwProgress * (float)fileSize));
		}

		private void OnPrograss_File()
		{
		}

		public override void DownloadCancel()
		{
			Debug.Log("NPatch.Unity.cs : DownloadCancel");
			if (this.behaviour != null)
			{
				this.behaviour.StopAllCoroutines();
			}
		}
	}
}
