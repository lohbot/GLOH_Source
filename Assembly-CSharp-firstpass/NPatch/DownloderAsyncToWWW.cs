using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace NPatch
{
	internal class DownloderAsyncToWWW : DownloderAsync
	{
		private Stopwatch _swRetry = new Stopwatch();

		private Stopwatch _swTimeout = new Stopwatch();

		public new int m_timeoutLimitSec = 60;

		public new int m_retryTerm = 10;

		public new int m_reconnectCountMax = 5;

		protected new Action<ERRORLEVEL> OnCompletedFile;

		protected new Action<ERRORLEVEL, string> OnCompletedString;

		private NPatchDownloderAsyncBehaviour behaviour;

		public new bool isNeedRetry
		{
			get
			{
				return this._swRetry.IsRunning && this._swRetry.Elapsed.Seconds > this.m_retryTerm;
			}
			set
			{
				if (value)
				{
					if (!this._swRetry.IsRunning)
					{
						this._swRetry.Reset();
						this._swRetry.Start();
					}
				}
				else if (this._swRetry.IsRunning)
				{
					this._swRetry.Stop();
				}
			}
		}

		public new bool isTimeout
		{
			get
			{
				return this._swTimeout.IsRunning && this._swTimeout.Elapsed.Seconds > this.m_timeoutLimitSec;
			}
			set
			{
				if (value)
				{
					if (!this._swTimeout.IsRunning)
					{
						this._swTimeout.Reset();
						this._swTimeout.Start();
					}
				}
				else if (this._swTimeout.IsRunning)
				{
					this._swTimeout.Stop();
				}
			}
		}

		public new string ErrorString
		{
			get;
			set;
		}

		public new ERRORLEVEL ErrorLevel
		{
			get;
			set;
		}

		public DownloderAsyncToWWW()
		{
			this.ErrorString = string.Empty;
			this.ErrorLevel = ERRORLEVEL.SUCCESS;
		}

		public DownloderAsyncToWWW(int limitSec, int retryTerm)
		{
			this.m_timeoutLimitSec = limitSec;
			this.m_retryTerm = retryTerm;
			this.m_reconnectCountMax = this.m_timeoutLimitSec / this.m_retryTerm - 1;
			this.isTimeout = false;
			this.isNeedRetry = false;
		}

		public new static void __DebugOutput(string msg)
		{
			DownloderAsyncToWWW.__DebugOutput(msg, null);
		}

		public new static void __DebugOutput(string msg, LauncherHandler _handler)
		{
			if (msg == null)
			{
				return;
			}
			UnityEngine.Debug.LogWarning(msg);
		}

		public new static DownloderAsyncToWWW Create()
		{
			return new DownloderAsyncToWWW();
		}

		public new static DownloderAsyncToWWW Create(int limitSec, int retryTerm)
		{
			return new DownloderAsyncToWWW(limitSec, retryTerm);
		}

		public override bool DownloadFile(string url, string filename, Action<ERRORLEVEL> _OnCompleted)
		{
			DownloderAsyncToWWW.__DebugOutput("AsyncDownloder.DownloadFile : " + url);
			this.ErrorString = string.Empty;
			this.ErrorLevel = ERRORLEVEL.SUCCESS;
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
			this.ErrorString = string.Empty;
			this.ErrorLevel = ERRORLEVEL.SUCCESS;
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

		private void OnCompleted_File(object sender, AsyncCompletedEventArgs e)
		{
			try
			{
				if (this.OnCompletedFile != null)
				{
					if (e.Cancelled)
					{
						this.OnCompletedFile(ERRORLEVEL.ERR_NEEDRETRY);
					}
					else if (e.Error != null)
					{
						this.ErrorString = e.Error.Message;
						if (e.Error.InnerException != null)
						{
							this.ErrorString = string.Format("{0} {1}", this.ErrorString, e.Error.InnerException.Message);
							if (e.Error.InnerException is SocketException)
							{
								this.OnCompletedFile(ERRORLEVEL.ERR_NEEDRETRY);
							}
							else if (e.Error.InnerException is IOException)
							{
								if (e.Error.InnerException.InnerException != null)
								{
									if (e.Error.InnerException.InnerException is SocketException)
									{
										this.OnCompletedFile(ERRORLEVEL.ERR_NEEDRETRY);
									}
									else
									{
										this.OnCompletedFile(ERRORLEVEL.ERR_IOEXCEPTION);
									}
								}
								else
								{
									Logger.WriteLog(string.Format("Exception : {0}", e.Error.InnerException));
									Logger.WriteLog(string.Format("Exception message : {0}", e.Error.InnerException.Message));
									this.OnCompletedFile(ERRORLEVEL.ERR_IOEXCEPTION);
								}
							}
							else
							{
								Logger.WriteLog(string.Format("Exception : {0}", e.Error.InnerException));
								Logger.WriteLog(string.Format("Exception message : {0}", e.Error.InnerException.Message));
								this.OnCompletedFile(ERRORLEVEL.ERR_NETWORKEXCEPTION);
							}
						}
						else
						{
							Logger.WriteLog(string.Format("Exception : {0}", e.Error));
							Logger.WriteLog(string.Format("Exception message : {0}", e.Error.InnerException.Message));
							this.OnCompletedFile(ERRORLEVEL.ERR_NETWORKEXCEPTION);
						}
					}
					else
					{
						this.OnCompletedFile(ERRORLEVEL.SUCCESS);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog(string.Format("Exception : {0}", ex.Message));
				this.OnCompletedFile(ERRORLEVEL.ERR_NETWORKEXCEPTION);
			}
		}

		private void OnCompleted_String(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				if (this.OnCompletedString != null)
				{
					if (e.Cancelled)
					{
						this.OnCompletedString(ERRORLEVEL.ERR_NEEDRETRY, "Download Cancelled");
					}
					else if (e.Error != null)
					{
						this.ErrorString = e.Error.Message;
						if (e.Error.InnerException != null)
						{
							this.ErrorString = string.Format("{0} {1}", this.ErrorString, e.Error.InnerException.Message);
							if (e.Error.InnerException is SocketException)
							{
								this.OnCompletedString(ERRORLEVEL.ERR_NEEDRETRY, this.ErrorString);
							}
							else if (e.Error.InnerException is IOException)
							{
								if (e.Error.InnerException.InnerException != null)
								{
									if (e.Error.InnerException.InnerException is SocketException)
									{
										this.OnCompletedString(ERRORLEVEL.ERR_NEEDRETRY, this.ErrorString);
									}
									else
									{
										this.OnCompletedString(ERRORLEVEL.ERR_IOEXCEPTION, this.ErrorString);
									}
								}
								else
								{
									Logger.WriteLog(string.Format("Exception : {0}", e.Error.InnerException));
									Logger.WriteLog(string.Format("Exception message : {0}", e.Error.InnerException.Message));
									this.OnCompletedString(ERRORLEVEL.ERR_IOEXCEPTION, this.ErrorString);
								}
							}
							else
							{
								Logger.WriteLog(string.Format("Exception : {0}", e.Error.InnerException));
								Logger.WriteLog(string.Format("Exception message : {0}", e.Error.InnerException.Message));
								this.OnCompletedString(ERRORLEVEL.ERR_NETWORKEXCEPTION, this.ErrorString);
							}
						}
						else
						{
							Logger.WriteLog(string.Format("Exception : {0}", e.Error));
							Logger.WriteLog(string.Format("Exception message : {0}", e.Error.InnerException.Message));
							this.OnCompletedString(ERRORLEVEL.ERR_NETWORKEXCEPTION, this.ErrorString);
						}
					}
					else
					{
						string result = e.Result;
						this.OnCompletedString(ERRORLEVEL.SUCCESS, result);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog(string.Format("Exception : {0}", ex.Message));
				this.OnCompletedString(ERRORLEVEL.ERR_NETWORKEXCEPTION, ex.Message);
			}
		}

		public override void DownloadCancel()
		{
			UnityEngine.Debug.Log("NPatch.Unity.cs : DownloadCancel");
			if (this.behaviour != null)
			{
				this.behaviour.StopAllCoroutines();
			}
		}
	}
}
