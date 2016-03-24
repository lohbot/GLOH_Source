using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace NPatch
{
	internal class DownloderAsync
	{
		private Stopwatch _swRetry = new Stopwatch();

		private Stopwatch _swTimeout = new Stopwatch();

		public int m_timeoutLimitSec = 60;

		public int m_retryTerm = 10;

		public int m_reconnectCountMax = 5;

		protected Action<ERRORLEVEL> OnCompletedFile;

		protected Action<ERRORLEVEL, string> OnCompletedString;

		private WebClient client;

		private long downloadSize;

		public bool isNeedRetry
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

		public bool isTimeout
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

		public string ErrorString
		{
			get;
			set;
		}

		public ERRORLEVEL ErrorLevel
		{
			get;
			set;
		}

		public DownloderAsync()
		{
			this.ErrorString = string.Empty;
			this.ErrorLevel = ERRORLEVEL.SUCCESS;
		}

		public DownloderAsync(int limitSec, int retryTerm)
		{
			this.m_timeoutLimitSec = limitSec;
			this.m_retryTerm = retryTerm;
			this.m_reconnectCountMax = this.m_timeoutLimitSec / this.m_retryTerm - 1;
		}

		public static void __DebugOutput(string msg)
		{
			DownloderAsync.__DebugOutput(msg, null);
		}

		public static void __DebugOutput(string msg, LauncherHandler _handler)
		{
			if (msg == null)
			{
				return;
			}
			UnityEngine.Debug.LogWarning(msg);
		}

		public static DownloderAsync Create()
		{
			return new DownloderAsync();
		}

		public static DownloderAsync Create(int limitSec, int retryTerm)
		{
			return new DownloderAsync(limitSec, retryTerm);
		}

		public virtual bool DownloadFile(string url, string filename, Action<ERRORLEVEL> _OnCompleted)
		{
			DownloderAsync.__DebugOutput("AsyncDownloder.DownloadFile : " + url);
			this.ErrorString = string.Empty;
			this.ErrorLevel = ERRORLEVEL.SUCCESS;
			this.OnCompletedFile = _OnCompleted;
			this.client = new WebClient();
			this.client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.OnDownloadFileProgressChanged);
			this.client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.OnCompleted_File);
			this.client.DownloadFileAsync(new Uri(url), filename);
			return true;
		}

		public virtual bool DownloadString(string url, Action<ERRORLEVEL, string> _OnCompleted)
		{
			this.isNeedRetry = true;
			this.OnCompletedString = _OnCompleted;
			this.ErrorString = string.Empty;
			this.ErrorLevel = ERRORLEVEL.SUCCESS;
			this.client = new WebClient();
			this.client.Encoding = Encoding.UTF8;
			this.client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.OnCompleted_String);
			this.client.DownloadStringAsync(new Uri(url));
			DownloderAsync.__DebugOutput("AsyncDownloder.DownloadString : " + url);
			return true;
		}

		private void OnDownloadFileProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this.downloadSize = e.BytesReceived;
		}

		public virtual long GetDownloadedBytes(string zipFile, int fileSize)
		{
			return this.downloadSize;
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
				this.isNeedRetry = false;
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

		public virtual void DownloadCancel()
		{
			if (this.client != null)
			{
				this.client.CancelAsync();
			}
		}
	}
}
