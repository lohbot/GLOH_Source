using ICSharpCode.SharpZipLib.Zip;
using NLibCs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace NPatch
{
	public class Launcher
	{
		internal class DownloderSync
		{
			private WebClient client;

			private Action<bool> OnCompletedFile;

			private Action<string> OnCompletedString;

			public static byte[] DownloadBytes(string url)
			{
				try
				{
					byte[] array = null;
					byte[] result;
					if (!Util.IsVaildURL(url))
					{
						result = null;
						return result;
					}
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
					HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					Stream responseStream = httpWebResponse.GetResponseStream();
					using (BinaryReader binaryReader = new BinaryReader(responseStream))
					{
						array = binaryReader.ReadBytes((int)responseStream.Length);
					}
					result = array;
					return result;
				}
				catch (Exception ex)
				{
					Launcher.__Output(ex.Message);
				}
				return null;
			}

			public static string DownloadString(string url, int timeout_ms = 1000000)
			{
				try
				{
					if (!Util.IsVaildURL(url))
					{
						string result = string.Empty;
						return result;
					}
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
					httpWebRequest.Timeout = timeout_ms;
					HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					Stream responseStream = httpWebResponse.GetResponseStream();
					using (StreamReader streamReader = new StreamReader(responseStream))
					{
						string result = streamReader.ReadToEnd();
						return result;
					}
				}
				catch (Exception ex)
				{
					Launcher.__Output(ex.Message);
				}
				return string.Empty;
			}

			public bool DownloadFile(string url, string filename, Action<bool> _OnCompleted)
			{
				Launcher.__Output("AsyncDownloder.DownloadFile : " + url);
				this.OnCompletedFile = _OnCompleted;
				this.client = new WebClient();
				this.client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.OnCompleted_File);
				this.client.DownloadFile(new Uri(url), filename);
				return true;
			}

			public bool DownloadString(string url, Action<string> _OnCompleted)
			{
				Launcher.__Output("AsyncDownloder.DownloadString : " + url);
				this.OnCompletedString = _OnCompleted;
				this.client = new WebClient();
				this.client.Encoding = Encoding.UTF8;
				this.client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.OnCompleted_String);
				this.client.DownloadString(new Uri(url));
				Launcher.__Output("AsyncDownloder.DownloadString return : " + url);
				return true;
			}

			private void OnCompleted_File(object sender, AsyncCompletedEventArgs e)
			{
				if (this.OnCompletedFile != null)
				{
					this.OnCompletedFile(e.Error != null);
				}
			}

			private void OnCompleted_String(object sender, DownloadStringCompletedEventArgs e)
			{
				if (this.OnCompletedString != null)
				{
					this.OnCompletedString(e.Result);
				}
			}
		}

		public class Task
		{
			public enum TaskResult
			{
				NONE,
				RUNNING,
				ABORTED,
				SUCCESS,
				FAILED,
				DONE
			}

			protected string _error_string;

			public Launcher Owner
			{
				get;
				set;
			}

			public Launcher.Task.TaskResult Result
			{
				get;
				set;
			}

			public string ErrorString
			{
				get
				{
					return this._error_string;
				}
			}

			public ERRORLEVEL PatchErrorLevel
			{
				get;
				set;
			}

			public virtual string StatusString
			{
				set
				{
					Launcher.__Output(value);
				}
			}

			public Task()
			{
				this.Owner = null;
				this.Result = Launcher.Task.TaskResult.NONE;
			}

			public void OccurError(ERRORLEVEL errorlevel, string fmt, params object[] objs)
			{
				this.Result = Launcher.Task.TaskResult.FAILED;
				this.PatchErrorLevel = errorlevel;
				this._error_string = string.Format(fmt, objs);
				Launcher.__Output(this._error_string);
				Logger.WriteLog(this._error_string, this.PatchErrorLevel);
			}

			public void OccurError(ERRORLEVEL errorlevel, string fmt, string objs)
			{
				this.OccurError(errorlevel, fmt, new object[]
				{
					objs
				});
			}

			public virtual Launcher.Task.TaskResult StartTask()
			{
				return Launcher.Task.TaskResult.NONE;
			}

			public virtual Launcher.Task.TaskResult UpdateTask()
			{
				return (this.ErrorString == null) ? this.Result : Launcher.Task.TaskResult.FAILED;
			}

			public virtual void EndTask()
			{
			}
		}

		internal class Task_CheckFinalClientVersion : Launcher.Task
		{
			private string url = string.Empty;

			private DownloderAsync downloder;

			private Launcher.ClientInfo ci = default(Launcher.ClientInfo);

			private bool loadFinish;

			public override Launcher.Task.TaskResult StartTask()
			{
				Launcher.Task.TaskResult result;
				try
				{
					this.StatusString = string.Format("Check Final Client Version...", new object[0]);
					this.url = string.Format("{0}/apk/final.client.version.txt", base.Owner.m_src_url_origin_root);
					this.DownloadFinalVersion();
					result = Launcher.Task.TaskResult.RUNNING;
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_URLFILE, ex.ToString(), new object[0]);
					result = base.Result;
				}
				return result;
			}

			private void DownloadFinalVersion()
			{
				this.downloder = DownloderAsync.Create(base.Owner.m_Systeminfo.timeoutLimitSec, base.Owner.m_Systeminfo.retryTerm);
				this.downloder.DownloadString(this.url, new Action<ERRORLEVEL, string>(this.OnCompleted_FinalVersion));
				base.Owner._status.taskReconnectCountMax = this.downloder.m_reconnectCountMax;
			}

			private void Retry_DownloadFinalVersion()
			{
				this.downloder.DownloadCancel();
				this.downloder = null;
				this.DownloadFinalVersion();
			}

			public override Launcher.Task.TaskResult UpdateTask()
			{
				if (this.downloder.isNeedRetry)
				{
					this.downloder.isNeedRetry = false;
					string msg = string.Format("Disconnected to Server. Retry... [ {0} / {1} ]", base.Owner._status.taskReconnectCount, this.downloder.m_reconnectCountMax);
					Launcher.__Output(msg);
					base.Owner.PrintTaskProgress("final.client.version.txt", TASKTYPE.RETRY, 0);
					this.Retry_DownloadFinalVersion();
				}
				if (!this.loadFinish)
				{
					return base.Result;
				}
				bool flag = base.Owner.LaunchHandler.OnCheckFinalClientVersion(this.ci);
				if (flag)
				{
					return Launcher.Task.TaskResult.SUCCESS;
				}
				return Launcher.Task.TaskResult.DONE;
			}

			public override void EndTask()
			{
			}

			protected void OnCompleted_FinalVersion(ERRORLEVEL eLevel, string context)
			{
				if (eLevel == ERRORLEVEL.SUCCESS)
				{
					this.__read_final_version(context);
				}
				else if (eLevel == ERRORLEVEL.ERR_NEEDRETRY)
				{
					if (base.Owner._status.taskReconnectCount < this.downloder.m_reconnectCountMax)
					{
						string arg_6F_0 = "Disconnected to Server. Retry... [ {0} / {1} ]";
						Launcher expr_4A_cp_0 = base.Owner;
						Logger.WriteLog(string.Format(arg_6F_0, expr_4A_cp_0._status.taskReconnectCount = expr_4A_cp_0._status.taskReconnectCount + 1, this.downloder.m_reconnectCountMax), eLevel);
						this.downloder.isNeedRetry = true;
					}
					else
					{
						this.downloder.DownloadCancel();
						base.OccurError(ERRORLEVEL.ERR_TIMEOUT, "Download File Timeout!!", new object[0]);
					}
				}
				else if (eLevel == ERRORLEVEL.ERR_IOEXCEPTION)
				{
					this.downloder.DownloadCancel();
					base.OccurError(ERRORLEVEL.ERR_IOEXCEPTION, string.Format("final.client.version.txt Download Failed! - {0}", context), new object[0]);
				}
				else
				{
					this.downloder.DownloadCancel();
					base.OccurError(ERRORLEVEL.ERR_NETWORKEXCEPTION, string.Format("final.client.version.txt Download Failed! - {0}", context), new object[0]);
				}
			}

			protected void __read_final_version(string context)
			{
				try
				{
					using (NDataReader nDataReader = new NDataReader())
					{
						if (context == null)
						{
							base.OccurError(ERRORLEVEL.ERR_URLFILE, "final.client.version.txt Not Exist !!!" + context, new object[0]);
							return;
						}
						if (nDataReader.LoadFrom(context, Encoding.UTF8))
						{
							this.ci.bundleVersion = nDataReader["Header"]["bundleversion"];
							this.loadFinish = true;
							return;
						}
						Launcher.__Output("NDataReader.Load Failed!" + context);
					}
					base.OccurError(ERRORLEVEL.ERR_URLFILE, "final.client.version.txt Parse Error!!!" + context, new object[0]);
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_URLFILE, ex.ToString(), new object[0]);
				}
			}
		}

		internal class Task_CheckFinalVersion : Launcher.Task
		{
			private float version;

			private string url = string.Empty;

			private string packName = string.Empty;

			private PACKTYPE checktype;

			private DownloderAsync downloder;

			public Task_CheckFinalVersion(PACKTYPE type)
			{
				switch (type)
				{
				case PACKTYPE.RESOURCE:
					this.packName = string.Format("pack", new object[0]);
					break;
				case PACKTYPE.LANG:
					this.packName = string.Format("langpack", new object[0]);
					break;
				case PACKTYPE.PREPATCH:
					this.packName = string.Format("prepack", new object[0]);
					break;
				}
				this.checktype = type;
			}

			public override Launcher.Task.TaskResult StartTask()
			{
				Launcher.Task.TaskResult result;
				try
				{
					this.StatusString = string.Format("Check Final Version...", new object[0]);
					this.version = 0f;
					if (!string.IsNullOrEmpty(base.Owner.APKVersion))
					{
						this.url = string.Format("{0}/{1}/{2}/{3}/final.version.txt", new object[]
						{
							base.Owner.m_src_url_origin_root,
							this.packName,
							base.Owner.infoFolderName,
							base.Owner.APKVersion
						});
					}
					else
					{
						this.url = string.Format("{0}/{1}/{2}/final.version.txt", base.Owner.m_src_url_origin_root, this.packName, base.Owner.infoFolderName);
					}
					this.DownloadFinalVersion();
					result = Launcher.Task.TaskResult.RUNNING;
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_URLFILE, ex.ToString(), new object[0]);
					result = base.Result;
				}
				return result;
			}

			private void DownloadFinalVersion()
			{
				this.downloder = DownloderAsync.Create(base.Owner.m_Systeminfo.timeoutLimitSec, base.Owner.m_Systeminfo.retryTerm);
				this.downloder.DownloadString(this.url, new Action<ERRORLEVEL, string>(this.OnCompleted_FinalVersion));
				base.Owner._status.taskReconnectCountMax = this.downloder.m_reconnectCountMax;
			}

			private void Retry_DownloadFinalVersion()
			{
				this.downloder.DownloadCancel();
				this.downloder = null;
				this.DownloadFinalVersion();
			}

			public override Launcher.Task.TaskResult UpdateTask()
			{
				if (this.downloder.isNeedRetry)
				{
					this.downloder.isNeedRetry = false;
					string msg = string.Format("Disconnected to Server. Retry... [ {0} / {1} ]", base.Owner._status.taskReconnectCount, this.downloder.m_reconnectCountMax);
					Launcher.__Output(msg);
					base.Owner.PrintTaskProgress("final.version.txt", TASKTYPE.RETRY, 0);
					this.Retry_DownloadFinalVersion();
				}
				if (this.version != 0f)
				{
					if (this.checktype == PACKTYPE.RESOURCE)
					{
						base.Owner.ResourceFinalVersion = this.version;
						this.CheckVersion(base.Owner.LocalResourceVersion, base.Owner.ResourceFinalVersion);
					}
					else if (this.checktype == PACKTYPE.LANG)
					{
						base.Owner.LangFinalVersion = this.version;
						this.CheckVersion(base.Owner.DefaultLangVersion, base.Owner.LangFinalVersion);
					}
					else if (this.checktype == PACKTYPE.PREPATCH)
					{
						base.Owner.PrepatchFinalVersion = this.version;
						this.CheckVersion(base.Owner.LocalPrepatchVersion, base.Owner.PrepatchFinalVersion);
					}
				}
				return base.Result;
			}

			public override void EndTask()
			{
			}

			protected void CheckVersion(float localVersion, float finalVersion)
			{
				this.StatusString = string.Format("{0} Final Version is [{1}]", this.checktype, finalVersion);
				if (localVersion > finalVersion)
				{
					base.OccurError(ERRORLEVEL.ERR_LOCALFILEINFO, string.Format("{0} Local Version Error!!", this.checktype), new object[0]);
				}
				else
				{
					base.PatchErrorLevel = ERRORLEVEL.SUCCESS;
					base.Result = Launcher.Task.TaskResult.SUCCESS;
				}
			}

			protected void OnCompleted_FinalVersion(ERRORLEVEL eLevel, string context)
			{
				if (eLevel == ERRORLEVEL.SUCCESS)
				{
					this.__read_final_version(context);
				}
				else if (eLevel == ERRORLEVEL.ERR_NEEDRETRY)
				{
					if (base.Owner._status.taskReconnectCount < this.downloder.m_reconnectCountMax)
					{
						string arg_6F_0 = "Disconnected to Server. Retry... [ {0} / {1} ]";
						Launcher expr_4A_cp_0 = base.Owner;
						Logger.WriteLog(string.Format(arg_6F_0, expr_4A_cp_0._status.taskReconnectCount = expr_4A_cp_0._status.taskReconnectCount + 1, this.downloder.m_reconnectCountMax), eLevel);
						this.downloder.isNeedRetry = true;
					}
					else
					{
						this.downloder.DownloadCancel();
						base.OccurError(ERRORLEVEL.ERR_TIMEOUT, "Download File Timeout!!", new object[0]);
					}
				}
				else if (eLevel == ERRORLEVEL.ERR_IOEXCEPTION)
				{
					this.downloder.DownloadCancel();
					base.OccurError(ERRORLEVEL.ERR_IOEXCEPTION, string.Format("final.version.txt Download Failed! - {0}", context), new object[0]);
				}
				else
				{
					this.downloder.DownloadCancel();
					base.OccurError(ERRORLEVEL.ERR_NETWORKEXCEPTION, string.Format("final.version.txt Download Failed! - {0}", context), new object[0]);
				}
			}

			protected void __read_final_version(string context)
			{
				try
				{
					using (NDataReader nDataReader = new NDataReader())
					{
						if (context == null)
						{
							base.OccurError(ERRORLEVEL.ERR_URLFILE, "final.version.txt Not Exist !!!" + context, new object[0]);
							return;
						}
						if (nDataReader.LoadFrom(context, Encoding.UTF8))
						{
							this.version = nDataReader["Header"]["PatchVersion"];
							return;
						}
						Launcher.__Output("NDataReader.Load Failed!" + context);
					}
					base.OccurError(ERRORLEVEL.ERR_URLFILE, "final.version.txt Parse Error!!!" + context, new object[0]);
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_URLFILE, ex.ToString(), new object[0]);
				}
			}
		}

		internal class Task_CheckLangPackList : Launcher.Task
		{
			private List<string> packedList = new List<string>();

			private Queue<int> DivisonQueue = new Queue<int>();

			private float cur_begin;

			private float cur_end;

			private float dest_ver;

			private int langcode;

			private bool _isNeededPatch;

			private bool _isSuccessTask;

			private Queue<int> langcodeQueue = new Queue<int>();

			private Queue<string> szMD5Queue = new Queue<string>();

			private Queue<int> zipfileSizeQueue = new Queue<int>();

			private Queue<int> unzipfileSizeQueue = new Queue<int>();

			private Queue<int> packSizeQueue = new Queue<int>();

			private Queue<int> unpackSizeQueue = new Queue<int>();

			private DownloderAsync downloder;

			public override Launcher.Task.TaskResult StartTask()
			{
				this.packedList.Clear();
				foreach (int current in base.Owner.langCodeList)
				{
					this.langcodeQueue.Enqueue(current);
				}
				if (this.langcodeQueue.Count == 0)
				{
					base.OccurError(ERRORLEVEL.ERR_PACKLIST, "langcodeQueue가 비었습니다.", new object[0]);
				}
				Launcher.__Output(string.Format("Check LangPack List...", new object[0]));
				while (this.langcodeQueue.Count > 0)
				{
					this.langcode = this.langcodeQueue.Dequeue();
					this.cur_begin = base.Owner.ReadLocalLangVersion(this.langcode);
					this.dest_ver = base.Owner.LangFinalVersion;
					if (this.cur_begin != this.dest_ver)
					{
						break;
					}
				}
				if (this.cur_begin == this.dest_ver)
				{
					Logger.WriteLog("All LangPack is Already Final Version...!", ERRORLEVEL.SUCCESS);
					return Launcher.Task.TaskResult.SUCCESS;
				}
				this.RequestPackText();
				return Launcher.Task.TaskResult.RUNNING;
			}

			private void StartThread()
			{
				base.Result = Launcher.Task.TaskResult.RUNNING;
				Thread thread = new Thread(new ThreadStart(this.CheckListMain));
				thread.Start();
			}

			public void CheckListMain()
			{
				if (this.DivisonQueue.Count != 0)
				{
					Launcher expr_1B_cp_0 = base.Owner;
					expr_1B_cp_0._status.totalPackCount = expr_1B_cp_0._status.totalPackCount + this.DivisonQueue.Count;
					List<string> list = new List<string>();
					int num = this.DivisonQueue.Dequeue();
					foreach (string current in this.packedList)
					{
						list.Add(current);
						if (num > 1)
						{
							num--;
						}
						else
						{
							Launcher.__Output(string.Format("Ready Download LangPack...[Packs:{0}]", list.Count));
							int num2 = 0;
							int num3 = this.packSizeQueue.Dequeue();
							foreach (string current2 in list)
							{
								string text = base.Owner.m_local_root + "/langpack/" + current2;
								FileInfo fileInfo = new FileInfo(text);
								if (fileInfo.Exists)
								{
									if (!fileInfo.Name.Contains(","))
									{
										base.Owner._status.packDownloadSize = (long)num3;
									}
									string mD = Util.GetMD5(text);
									string text2 = this.szMD5Queue.Dequeue();
									int num4 = this.zipfileSizeQueue.Dequeue();
									if (mD.Equals(text2))
									{
										Launcher.__Output(string.Format("{0} is already exist!", current2));
										base.Owner.AddExistZipSize(num4);
										base.Owner.PrintTotalProgress(TASKTYPE.DOWNLOAD);
										num2++;
									}
									else
									{
										Launcher.__Output(string.Format("{0} is broken. Delete and redownload!", current2));
										File.Delete(text);
										base.Owner.list_Tasks.AddLast(new Launcher.Task_DownloadPack(base.Owner, current2, list.Count, num2++, text2, num4, num3, PACKTYPE.LANG));
										this._isNeededPatch = true;
									}
								}
								else
								{
									base.Owner.list_Tasks.AddLast(new Launcher.Task_DownloadPack(base.Owner, current2, list.Count, num2++, this.szMD5Queue.Dequeue(), this.zipfileSizeQueue.Dequeue(), num3, PACKTYPE.LANG));
									this._isNeededPatch = true;
								}
							}
							num2 = 0;
							int count = list.Count;
							int unzipSize = this.unpackSizeQueue.Dequeue();
							foreach (string current3 in list)
							{
								base.Owner.list_Tasks.AddLast(new Launcher.Task_InstallPack(base.Owner, current3, count--, list.Count, num2++, this.unzipfileSizeQueue.Dequeue(), unzipSize, PACKTYPE.LANG));
								this._isNeededPatch = true;
							}
							if (this.DivisonQueue.Count != 0)
							{
								num = this.DivisonQueue.Dequeue();
								list.Clear();
							}
						}
					}
					this._isSuccessTask = true;
				}
				else
				{
					base.OccurError(ERRORLEVEL.ERR_PACKLIST, "Division정보가 존재하지 않습니다", new object[0]);
				}
			}

			public override Launcher.Task.TaskResult UpdateTask()
			{
				if (this._isSuccessTask && this._isNeededPatch && base.Owner.LaunchHandler != null)
				{
					return base.Owner.LaunchHandler.OnCheckStart();
				}
				if (this.cur_end != this.dest_ver)
				{
					if (this.downloder.isTimeout)
					{
						this.downloder.isTimeout = false;
						base.OccurError(ERRORLEVEL.ERR_TIMEOUT, "Check LangPack List Timeout!!", new object[0]);
						return base.Result;
					}
					if (this.downloder.isNeedRetry)
					{
						this.downloder.isNeedRetry = false;
						string msg = string.Format("Disconnected to Server. Retry... [ {0} / {1} ]", base.Owner._status.taskReconnectCount, this.downloder.m_reconnectCountMax);
						Launcher.__Output(msg);
						base.Owner.PrintTaskProgress(this.cur_begin.ToString(), TASKTYPE.RETRY, 0);
						this.Retry_RequestPackText();
					}
				}
				return base.Result;
			}

			public override void EndTask()
			{
				base.EndTask();
			}

			private bool RequestPackText()
			{
				Launcher.__Output(string.Format("Check Begin LangPack Info...[Vers:{0}]", this.cur_begin));
				string url = string.Empty;
				if (!string.IsNullOrEmpty(base.Owner.APKVersion))
				{
					url = string.Format("{0}/langpack/{1}/{2}/{3}({4}).txt", new object[]
					{
						base.Owner.m_src_url_origin_root,
						base.Owner.infoFolderName,
						base.Owner.APKVersion,
						this.cur_begin,
						this.langcode
					});
				}
				else
				{
					url = string.Format("{0}/langpack/{1}/{2}({3}).txt", new object[]
					{
						base.Owner.m_src_url_origin_root,
						base.Owner.infoFolderName,
						this.cur_begin,
						this.langcode
					});
				}
				this.downloder = DownloderAsync.Create(base.Owner.m_Systeminfo.timeoutLimitSec, base.Owner.m_Systeminfo.retryTerm);
				this.downloder.DownloadString(url, new Action<ERRORLEVEL, string>(this.OnCompleted_DownloadPackText));
				return true;
			}

			private void Retry_RequestPackText()
			{
				this.downloder.DownloadCancel();
				this.downloder = null;
				this.RequestPackText();
			}

			private void NextPackText()
			{
				this.cur_begin = this.cur_end;
				this.RequestPackText();
			}

			protected void OnCompleted_DownloadPackText(ERRORLEVEL eLevel, string context)
			{
				try
				{
					float num = 0f;
					if (!this.ParseLangPackInfo(context, out num))
					{
						base.OccurError(ERRORLEVEL.ERR_URLFILE, "info파일 파싱 실패. endversion : " + num.ToString(), new object[0]);
					}
					else
					{
						this.cur_end = num;
						if (this.cur_end == this.dest_ver)
						{
							this.CheckOtherLangCode();
						}
						else
						{
							this.NextPackText();
						}
					}
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_URLFILE, ex.Message, new object[0]);
				}
			}

			protected bool CheckOtherLangCode()
			{
				if (this.langcodeQueue.Count == 0)
				{
					this.StartThread();
					return false;
				}
				while (this.langcodeQueue.Count > 0)
				{
					this.langcode = this.langcodeQueue.Dequeue();
					this.cur_begin = base.Owner.ReadLocalLangVersion(this.langcode);
					this.dest_ver = base.Owner.LangFinalVersion;
					if (this.cur_begin != this.dest_ver)
					{
						break;
					}
				}
				if (this.cur_begin == this.dest_ver)
				{
					this.StartThread();
					return false;
				}
				this.RequestPackText();
				return true;
			}

			protected bool ParseLangPackInfo(string context, out float endVersion)
			{
				endVersion = 0f;
				bool result;
				try
				{
					if (context != string.Empty)
					{
						using (NDataReader nDataReader = new NDataReader())
						{
							if (nDataReader.LoadFrom(context, Encoding.UTF8))
							{
								Launcher.__Output(string.Format("Context: info File Load 성공!\r\n {0}", context));
								endVersion = nDataReader["Info"]["Until"];
								Launcher.__Output(string.Format("ver: {0} 파싱 성공!\r\n", endVersion));
								int num = 0;
								int num2 = 0;
								int num3 = 0;
								NDataSection nDataSection = nDataReader["ZipFiles"];
								foreach (NDataReader.Row row in nDataSection)
								{
									string column = row.GetColumn(0);
									string column2 = row.GetColumn(1);
									if (column2 == null || column2.Equals("(null)"))
									{
										base.OccurError(ERRORLEVEL.ERR_URLFILE, "MD5 Load Error!!!", new object[0]);
										result = false;
										return result;
									}
									this.szMD5Queue.Enqueue(column2);
									Launcher.__Output(string.Format("MD5: {0} MD5 Load 성공!\r\n", column2));
									if (row.GetColumn(2) == null || row.GetColumn(2).Equals("(null)"))
									{
										base.OccurError(ERRORLEVEL.ERR_URLFILE, "unzipSize Load Error!!!", new object[0]);
										result = false;
										return result;
									}
									int num4 = Convert.ToInt32(row.GetColumn(2));
									num3 += num4;
									Launcher.__Output(string.Format("unzipSize: {0}Byte unzipSize Load 성공!\r\n", num4));
									if (row.GetColumn(3) == null || row.GetColumn(3).Equals("(null)"))
									{
										base.OccurError(ERRORLEVEL.ERR_URLFILE, "zipSize Load Error!!!", new object[0]);
										result = false;
										return result;
									}
									int num5 = Convert.ToInt32(row.GetColumn(3));
									num2 += num5;
									Launcher.__Output(string.Format("zipSize: {0}Byte zipSize Load 성공!\r\n", num5));
									this.packedList.Add(column);
									this.AddPackSize(num4, num5);
									num++;
								}
								if (num > 0)
								{
									this.DivisonQueue.Enqueue(num);
								}
								this.packSizeQueue.Enqueue(num2);
								this.unpackSizeQueue.Enqueue(num3);
								result = true;
								return result;
							}
						}
					}
					else
					{
						Launcher.__Output(string.Format("Error!!!!! - ver: {0} 가 없습니다!\r\n{1}", endVersion, context));
					}
					base.OccurError(ERRORLEVEL.ERR_URLFILE, "PatchedVersion Parse Error!!!", new object[0]);
					result = false;
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_URLFILE, ex.ToString(), new object[0]);
					result = false;
				}
				return result;
			}

			protected void AddPackSize(int unzipSize, int zipSize)
			{
				this.unzipfileSizeQueue.Enqueue(unzipSize);
				base.Owner.AddUnzipSize(unzipSize);
				this.zipfileSizeQueue.Enqueue(zipSize);
				base.Owner.AddZipSize(zipSize);
				Launcher expr_3B_cp_0 = base.Owner;
				expr_3B_cp_0._status.totalTaskCount = expr_3B_cp_0._status.totalTaskCount + 2;
			}
		}

		internal class Task_CheckPackList : Launcher.Task
		{
			private List<string> packedList = new List<string>();

			private Queue<int> DivisonQueue = new Queue<int>();

			private float local_pv0;

			private float local_pvF;

			private float adjust_StartVersion = -1f;

			private float cur_begin;

			private float cur_end;

			private float dest_ver;

			private int patchLevel;

			private bool _isNeededPatch;

			private bool _isSuccessTask;

			private int patchlist_txt_size;

			private int patchlist_zip_size;

			private string patchlist_MD5 = string.Empty;

			private Queue<string> szMD5Queue = new Queue<string>();

			private Queue<int> zipfileSizeQueue = new Queue<int>();

			private Queue<int> unzipfileSizeQueue = new Queue<int>();

			private Queue<int> packSizeQueue = new Queue<int>();

			private Queue<int> unpackSizeQueue = new Queue<int>();

			private DownloderAsync downloder;

			private bool PrepackMode;

			private bool IsSamePatchedVersion
			{
				get
				{
					return this.local_pv0 == this.local_pvF;
				}
			}

			public override Launcher.Task.TaskResult StartTask()
			{
				if (base.Owner.LocalResourceVersion == base.Owner.ResourceFinalVersion)
				{
					Logger.WriteLog(string.Format("resource is Already Final Version...!", new object[0]), ERRORLEVEL.SUCCESS);
					return Launcher.Task.TaskResult.SUCCESS;
				}
				this.packedList.Clear();
				this.cur_begin = 0f;
				Launcher.__Output(string.Format("Check Pack List... [Local:{0},Final:{1}] ", base.Owner.LocalResourceVersion, base.Owner.ResourceFinalVersion));
				if (base.Owner.m_Systeminfo.usePrepack)
				{
					float num = base.Owner.ReadLocalPrepatchedVersion();
					if (num == base.Owner.PrepatchFinalVersion)
					{
						this.CheckPatchLevel();
					}
					else
					{
						this.CheckPrepack();
					}
				}
				else
				{
					this.CheckPatchLevel();
				}
				return Launcher.Task.TaskResult.RUNNING;
			}

			private void CheckPrepack()
			{
				this.PrepackMode = true;
				this.cur_begin = base.Owner.LocalPrepatchVersion;
				this.dest_ver = base.Owner.PrepatchFinalVersion;
				this.RequestPackText(this.PrepackMode);
			}

			private void CheckPatchLevel()
			{
				this.PrepackMode = false;
				this.local_pv0 = base.Owner.ReadLocalPatchedVersion(0);
				this.local_pvF = base.Owner.ReadLocalPatchedVersion(base.Owner.LocalPatchLevel);
				if (base.Owner.LocalPatchLevel > 1)
				{
					float num = this.local_pv0;
					for (int i = 1; i < base.Owner.LocalPatchLevel; i++)
					{
						if (num != base.Owner.ReadLocalPatchedVersion(i))
						{
						}
					}
				}
				if (this.IsSamePatchedVersion && base.Owner.IsFullPatchLevel)
				{
					this.cur_begin = base.Owner.LocalResourceVersion;
					this.dest_ver = base.Owner.ResourceFinalVersion;
					this.patchLevel = -1;
				}
				else
				{
					this.cur_begin = base.Owner.ReadLocalPatchedVersion(base.Owner.LocalPatchLevel);
					this.dest_ver = base.Owner.ResourceFinalVersion;
					this.patchLevel = base.Owner.LocalPatchLevel;
				}
				if (this.local_pv0 != base.Owner.ResourceFinalVersion && base.Owner.LocalPatchLevel > 0)
				{
					this.adjust_StartVersion = this.local_pv0;
				}
				this.RequestPackText(this.PrepackMode);
			}

			private void StartThread()
			{
				base.Result = Launcher.Task.TaskResult.RUNNING;
				Thread thread = new Thread(new ThreadStart(this.ResourceCheckListMain));
				thread.Start();
			}

			public void ResourceCheckListMain()
			{
				if (!base.Owner.isCallPrepackEndFuncOnlyFirstPatch && !base.Owner.isNeedPrepatch)
				{
					base.Owner.LaunchHandler.OnEndPrepack();
				}
				if (this.DivisonQueue.Count != 0)
				{
					base.Owner._status.totalPackCount = this.DivisonQueue.Count;
					List<string> list = new List<string>();
					int num = this.DivisonQueue.Dequeue();
					foreach (string current in this.packedList)
					{
						list.Add(current);
						if (num > 1)
						{
							num--;
						}
						else
						{
							Launcher.__Output(string.Format("Ready Download Pack...[Packs:{0}]", list.Count));
							int num2 = 0;
							int num3 = this.packSizeQueue.Dequeue();
							foreach (string current2 in list)
							{
								string text = string.Empty;
								if (current2.Contains("pre"))
								{
									text = base.Owner.m_local_root + "/prepack/" + current2;
								}
								else
								{
									text = base.Owner.m_local_root + "/pack/" + current2;
								}
								FileInfo fileInfo = new FileInfo(text);
								if (fileInfo.Exists)
								{
									if (!fileInfo.Name.Contains(","))
									{
										base.Owner._status.packDownloadSize = (long)num3;
									}
									string mD = Util.GetMD5(text);
									string text2 = this.szMD5Queue.Dequeue();
									int num4 = this.zipfileSizeQueue.Dequeue();
									if (mD.Equals(text2))
									{
										Launcher.__Output(string.Format("{0} is already exist!", current2));
										base.Owner.AddExistZipSize(num4);
										base.Owner.PrintTotalProgress(TASKTYPE.DOWNLOAD);
										num2++;
									}
									else
									{
										Launcher.__Output(string.Format("{0} is broken. Delete and redownload!", current2));
										File.Delete(text);
										base.Owner.list_Tasks.AddLast(new Launcher.Task_DownloadPack(base.Owner, current2, list.Count, num2++, text2, num4, num3, PACKTYPE.RESOURCE));
										this._isNeededPatch = true;
									}
								}
								else
								{
									base.Owner.list_Tasks.AddLast(new Launcher.Task_DownloadPack(base.Owner, current2, list.Count, num2++, this.szMD5Queue.Dequeue(), this.zipfileSizeQueue.Dequeue(), num3, PACKTYPE.RESOURCE));
									this._isNeededPatch = true;
								}
							}
							num2 = 0;
							int num5 = list.Count;
							int num6 = this.unpackSizeQueue.Dequeue();
							foreach (string current3 in list)
							{
								if (!base.Owner.IsAlreadyInstalledDivision(current3, num5))
								{
									base.Owner.list_Tasks.AddLast(new Launcher.Task_InstallPack(base.Owner, current3, num5--, list.Count, num2++, this.unzipfileSizeQueue.Dequeue(), num6, PACKTYPE.RESOURCE));
									this._isNeededPatch = true;
								}
								else
								{
									if (!current3.Contains(","))
									{
										base.Owner._status.packInstallSize = (long)num6;
									}
									int size = this.unzipfileSizeQueue.Dequeue();
									Launcher.__Output(string.Format("{0} is already Installed!", current3));
									base.Owner.AddAlreadyInstalledSize(size);
									base.Owner.PrintTotalProgress(TASKTYPE.INSTALL);
									num2++;
									num5--;
								}
							}
							if (this.DivisonQueue.Count != 0)
							{
								num = this.DivisonQueue.Dequeue();
								list.Clear();
							}
						}
					}
					if (base.Owner.m_Systeminfo.downloadPatchlist)
					{
						base.Owner.list_Tasks.AddLast(new Launcher.Task_DownloadFinalPatchList(base.Owner, this.patchlist_zip_size, this.patchlist_MD5));
						base.Owner.list_Tasks.AddLast(new Launcher.Task_InstallFinalPatchList(base.Owner, this.patchlist_txt_size));
						Launcher expr_44B_cp_0 = base.Owner;
						expr_44B_cp_0._status.totalPackCount = expr_44B_cp_0._status.totalPackCount + 1;
					}
					this._isSuccessTask = true;
				}
				else
				{
					base.OccurError(ERRORLEVEL.ERR_PACKLIST, "Division정보가 존재하지 않습니다", new object[0]);
				}
			}

			public override Launcher.Task.TaskResult UpdateTask()
			{
				if (this._isSuccessTask && this._isNeededPatch)
				{
					if (base.Owner.m_Systeminfo.useLangpack)
					{
						return Launcher.Task.TaskResult.SUCCESS;
					}
					if (base.Owner.LaunchHandler != null)
					{
						return base.Owner.LaunchHandler.OnCheckStart();
					}
				}
				if (this.cur_end != this.dest_ver)
				{
					if (this.downloder.isTimeout)
					{
						this.downloder.isTimeout = false;
						base.OccurError(ERRORLEVEL.ERR_TIMEOUT, "Check Pack List Timeout!!", new object[0]);
						return base.Result;
					}
					if (this.downloder.isNeedRetry)
					{
						this.downloder.isNeedRetry = false;
						string msg = string.Format("Disconnected to Server. Retry... [ {0} / {1} ]", base.Owner._status.taskReconnectCount, this.downloder.m_reconnectCountMax);
						Launcher.__Output(msg);
						base.Owner.PrintTaskProgress(this.cur_begin.ToString(), TASKTYPE.RETRY, 0);
						this.Retry_RequestPackText();
					}
				}
				return base.Result;
			}

			public override void EndTask()
			{
				base.EndTask();
			}

			private bool RequestPackText(bool usePrepack)
			{
				Launcher.__Output(string.Format("Check Begin Pack Info...[Vers:{0}]", this.cur_begin));
				string url = string.Empty;
				string arg = string.Empty;
				if (usePrepack)
				{
					if (!string.IsNullOrEmpty(base.Owner.APKVersion))
					{
						arg = string.Format("{0}/prepack/{1}/{2}", base.Owner.m_src_url_origin_root, base.Owner.infoFolderName, base.Owner.APKVersion);
					}
					else
					{
						arg = string.Format("{0}/prepack/{1}", base.Owner.m_src_url_origin_root, base.Owner.infoFolderName);
					}
					url = string.Format("{0}/{1}.txt", arg, this.cur_begin);
				}
				else
				{
					if (!string.IsNullOrEmpty(base.Owner.APKVersion))
					{
						arg = string.Format("{0}/pack/{1}/{2}", base.Owner.m_src_url_origin_root, base.Owner.infoFolderName, base.Owner.APKVersion);
					}
					else
					{
						arg = string.Format("{0}/pack/{1}", base.Owner.m_src_url_origin_root, base.Owner.infoFolderName);
					}
					if (this.patchLevel == -1)
					{
						url = string.Format("{0}/{1}.txt", arg, this.cur_begin);
					}
					else
					{
						url = string.Format("{0}/{1}[{2}].txt", arg, this.cur_begin, this.patchLevel);
					}
				}
				this.downloder = DownloderAsync.Create(base.Owner.m_Systeminfo.timeoutLimitSec, base.Owner.m_Systeminfo.retryTerm);
				this.downloder.DownloadString(url, new Action<ERRORLEVEL, string>(this.OnCompleted_DownloadPackText));
				return true;
			}

			private void Retry_RequestPackText()
			{
				this.downloder.DownloadCancel();
				this.downloder = null;
				this.RequestPackText(this.PrepackMode);
			}

			private void NextPackText()
			{
				this.cur_begin = this.cur_end;
				this.RequestPackText(this.PrepackMode);
			}

			protected void OnCompleted_DownloadPackText(ERRORLEVEL eLevel, string context)
			{
				try
				{
					float num = 0f;
					if (!this.ParsePackInfo(context, out num))
					{
						base.OccurError(ERRORLEVEL.ERR_URLFILE, "info파일 파싱 실패. endversion : " + num.ToString(), new object[0]);
					}
					else
					{
						this.cur_end = num;
						if (this.PrepackMode)
						{
							if (this.cur_end == this.dest_ver)
							{
								this.CheckPatchLevel();
							}
							else
							{
								this.NextPackText();
							}
						}
						else if (this.cur_end == this.dest_ver)
						{
							this.CheckAdjustVersion();
						}
						else
						{
							this.NextPackText();
						}
					}
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_URLFILE, ex.ToString(), new object[0]);
				}
			}

			protected bool CheckAdjustVersion()
			{
				if (this.adjust_StartVersion != -1f && this.patchLevel > 0)
				{
					this.patchLevel--;
					this.cur_begin = this.adjust_StartVersion;
					this.RequestPackText(false);
					return true;
				}
				base.Owner.LaunchHandler.SetEdgeURL(ref base.Owner.m_src_pack_url_root);
				this.StartThread();
				return false;
			}

			protected bool ParsePackInfo(string context, out float endVersion)
			{
				endVersion = 0f;
				float num = 0f;
				bool result;
				try
				{
					if (context != string.Empty)
					{
						using (NDataReader nDataReader = new NDataReader())
						{
							if (nDataReader.LoadFrom(context, Encoding.UTF8))
							{
								Launcher.__Output(string.Format("Context: info File Load 성공!\r\n {0}", context));
								num = nDataReader["Info"]["After"];
								endVersion = nDataReader["Info"]["Until"];
								Launcher.__Output(string.Format("ver: {0} 파싱 성공!\r\n", endVersion));
								int num2 = 0;
								int num3 = 0;
								int num4 = 0;
								int num5 = 0;
								NDataSection nDataSection = nDataReader["ZipFiles"];
								foreach (NDataReader.Row row in nDataSection)
								{
									num5++;
									string column = row.GetColumn(0);
									if (column.ToLower().Contains("pre"))
									{
										base.Owner.isNeedPrepatch = true;
									}
									string column2 = row.GetColumn(1);
									if (column2 == null || column2.Equals("(null)"))
									{
										base.OccurError(ERRORLEVEL.ERR_URLFILE, "MD5 Load Error!!!", new object[0]);
										result = false;
										return result;
									}
									this.szMD5Queue.Enqueue(column2);
									Launcher.__Output(string.Format("MD5: {0} MD5 Load 성공!\r\n", column2));
									if (row.GetColumn(2) == null || row.GetColumn(2).Equals("(null)"))
									{
										base.OccurError(ERRORLEVEL.ERR_URLFILE, "unzipSize Load Error!!!", new object[0]);
										result = false;
										return result;
									}
									int num6 = Convert.ToInt32(row.GetColumn(2));
									num4 += num6;
									Launcher.__Output(string.Format("unzipSize: {0}Byte unzipSize Load 성공!\r\n", num6));
									if (row.GetColumn(3) == null || row.GetColumn(3).Equals("(null)"))
									{
										base.OccurError(ERRORLEVEL.ERR_URLFILE, "zipSize Load Error!!!", new object[0]);
										result = false;
										return result;
									}
									int num7 = Convert.ToInt32(row.GetColumn(3));
									num3 += num7;
									Launcher.__Output(string.Format("zipSize: {0}Byte zipSize Load 성공!\r\n", num7));
									this.packedList.Add(column);
									this.AddPackSize(num6, num7);
									num2++;
								}
								if (num == base.Owner.DefaultResourceVersion)
								{
									base.Owner._status.fullPackCount = num5;
								}
								if (num2 > 0)
								{
									this.DivisonQueue.Enqueue(num2);
								}
								this.packSizeQueue.Enqueue(num3);
								this.unpackSizeQueue.Enqueue(num4);
								if (nDataReader.BeginSection("patchlist"))
								{
									NDataSection nDataSection2 = nDataReader["patchlist"];
									foreach (NDataReader.Row row2 in nDataSection2)
									{
										this.patchlist_MD5 = row2.GetColumn(1);
										this.patchlist_txt_size = Convert.ToInt32(row2.GetColumn(2));
										this.patchlist_zip_size = Convert.ToInt32(row2.GetColumn(3));
									}
									base.Owner.AddUnzipSize(this.patchlist_txt_size);
									base.Owner.AddZipSize(this.patchlist_zip_size);
									Launcher expr_388_cp_0 = base.Owner;
									expr_388_cp_0._status.totalTaskCount = expr_388_cp_0._status.totalTaskCount + 2;
								}
								base.Owner.PrintTotalProgress(TASKTYPE.CHECKFILE);
								result = true;
								return result;
							}
						}
					}
					else
					{
						Launcher.__Output(string.Format("Error!!!!! - ver: {0} 가 없습니다!\r\n{1}", endVersion, context));
					}
					base.OccurError(ERRORLEVEL.ERR_URLFILE, "PatchedVersion Parse Error!!!", new object[0]);
					result = false;
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_URLFILE, ex.ToString(), new object[0]);
					result = false;
				}
				return result;
			}

			protected void AddPackSize(int unzipSize, int zipSize)
			{
				this.unzipfileSizeQueue.Enqueue(unzipSize);
				base.Owner.AddUnzipSize(unzipSize);
				this.zipfileSizeQueue.Enqueue(zipSize);
				base.Owner.AddZipSize(zipSize);
				Launcher expr_3B_cp_0 = base.Owner;
				expr_3B_cp_0._status.totalTaskCount = expr_3B_cp_0._status.totalTaskCount + 2;
			}
		}

		internal class Task_DownloadFile : Launcher.Task
		{
			private const int reDownloadMax = 5;

			private DownloderAsync downloder;

			protected string _srcUrl = string.Empty;

			protected string _destPath = string.Empty;

			protected string _fileName = string.Empty;

			protected string _chksum = string.Empty;

			protected int _downloadOrder = 1;

			protected int _downloadCount = 1;

			protected int _fileSize;

			protected int _preSize;

			protected int _nowSize;

			protected int _retrySize;

			protected int _retryCount = 1;

			protected bool _md5CheckFinish;

			protected int packSize;

			public Task_DownloadFile()
			{
			}

			public Task_DownloadFile(string srcUrl, string destPath, string chksum)
			{
				base.Owner = Launcher.Instance;
				this._srcUrl = srcUrl;
				this._destPath = destPath;
				this._chksum = chksum;
				this._downloadOrder = 1;
				this._downloadCount = 1;
			}

			public Task_DownloadFile(string srcUrl, string destPath, string chksum, int fileSize, int total, int index)
			{
				base.Owner = Launcher.Instance;
				this._srcUrl = srcUrl;
				this._destPath = destPath;
				this._chksum = chksum;
				this._fileSize = fileSize;
				this._downloadCount = total;
				this._downloadOrder = index + 1;
			}

			public override Launcher.Task.TaskResult StartTask()
			{
				if (this._fileName.Equals(string.Empty) && this._destPath != null && !this._destPath.Equals(string.Empty))
				{
					FileInfo fileInfo = new FileInfo(this._destPath);
					this._fileName = fileInfo.Name;
				}
				if (this._fileName == null || this._fileName.Equals(string.Empty))
				{
					base.OccurError(ERRORLEVEL.ERR_URLFILE, "Error! Not Exists File!", new object[0]);
					return base.Result;
				}
				base.Owner.SetTaskSize(this._fileSize, TASKTYPE.DOWNLOAD);
				this.Request();
				base.Result = Launcher.Task.TaskResult.RUNNING;
				return base.Result;
			}

			public override Launcher.Task.TaskResult UpdateTask()
			{
				lock (this)
				{
					this._nowSize = (int)this.downloder.GetDownloadedBytes(this._destPath, this._fileSize);
					if (this._preSize < this._nowSize && this._nowSize != this._fileSize)
					{
						string fileName = Path.GetFileName(this._destPath);
						base.Owner.AddProgressSize(this._nowSize - this._preSize, TASKTYPE.DOWNLOAD);
						this._preSize = this._nowSize;
						base.Owner.PrintTaskProgress(fileName, TASKTYPE.DOWNLOAD, 0);
					}
					if (this._md5CheckFinish)
					{
						return Launcher.Task.TaskResult.SUCCESS;
					}
					if (this.downloder.isNeedRetry)
					{
						this.downloder.isNeedRetry = false;
						base.Owner.PrintTaskProgress(this._fileName, TASKTYPE.RETRY, 0);
						this.Retry_Request();
					}
				}
				return base.Result;
			}

			public override void EndTask()
			{
				this.downloder = null;
			}

			protected void Retry_Request()
			{
				this.downloder.DownloadCancel();
				this.downloder = null;
				this.Request();
			}

			protected bool Request()
			{
				this.StatusString = string.Format("[{0}/{1}] Download File...{2}", this._downloadOrder, this._downloadCount, this._fileName);
				if (!Path.IsPathRooted(this._destPath))
				{
					this._destPath = Path.GetFullPath(this._destPath);
				}
				if (!Directory.Exists(Path.GetDirectoryName(this._destPath)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(this._destPath));
				}
				this.downloder = DownloderAsync.Create(base.Owner.m_Systeminfo.timeoutLimitSec, base.Owner.m_Systeminfo.retryTerm);
				this.downloder.DownloadFile(this._srcUrl, this._destPath, new Action<ERRORLEVEL>(this.OnCompleted));
				return true;
			}

			private void OnCompleted(ERRORLEVEL eLevel)
			{
				lock (this)
				{
					if (eLevel == ERRORLEVEL.SUCCESS)
					{
						this.checkMD5Thread();
					}
					else if (eLevel == ERRORLEVEL.ERR_NEEDRETRY)
					{
						if (base.Owner._status.taskReconnectCount < this.downloder.m_reconnectCountMax)
						{
							string arg_76_0 = "Disconnected to Server. Retry... [ {0} / {1} ]";
							Launcher expr_51_cp_0 = base.Owner;
							Logger.WriteLog(string.Format(arg_76_0, expr_51_cp_0._status.taskReconnectCount = expr_51_cp_0._status.taskReconnectCount + 1, this.downloder.m_reconnectCountMax), eLevel);
							this.downloder.isNeedRetry = true;
						}
						else
						{
							this.downloder.DownloadCancel();
							base.OccurError(ERRORLEVEL.ERR_TIMEOUT, "Download File Timeout!!", new object[0]);
						}
					}
					else if (eLevel == ERRORLEVEL.ERR_IOEXCEPTION)
					{
						this.downloder.DownloadCancel();
						base.OccurError(ERRORLEVEL.ERR_IOEXCEPTION, string.Format("[{0}/{1}] Download {2}...Failed!", this._downloadOrder, this._downloadCount, this._fileName), new object[0]);
					}
					else
					{
						this.downloder.DownloadCancel();
						base.OccurError(ERRORLEVEL.ERR_NETWORKEXCEPTION, string.Format("[{0}/{1}] Download {2}...Failed!", this._downloadOrder, this._downloadCount, this._fileName), new object[0]);
					}
				}
			}

			private void checkMD5Thread()
			{
				Thread thread = new Thread(new ThreadStart(this.__check_file));
				thread.Start();
			}

			private void __check_file()
			{
				string fileName = Path.GetFileName(this._destPath);
				if (this.CheckMD5())
				{
					this.StatusString = string.Format("[{0}/{1}] Download {2}...Success!", this._downloadOrder, this._downloadCount, this._fileName);
					base.Owner.EndTaskProgress(fileName, TASKTYPE.DOWNLOAD);
					base.PatchErrorLevel = ERRORLEVEL.SUCCESS;
					base.Result = Launcher.Task.TaskResult.SUCCESS;
					this._md5CheckFinish = true;
				}
				else
				{
					this.reDownload();
				}
			}

			private void reDownload()
			{
				File.Delete(this._destPath);
				if (this._retryCount > 5)
				{
					base.OccurError(ERRORLEVEL.ERR_FILEBROKEN, string.Format("[{0}/{1}] Download {2}...File is Broken!", new object[]
					{
						this._downloadOrder,
						this._downloadCount,
						this._retryCount,
						5,
						this._fileName
					}), new object[0]);
				}
				else
				{
					this.StatusString = string.Format("[{0}/{1}] Download {2}...File is Broken! Retry... [ {3} / {4} ]", new object[]
					{
						this._downloadOrder,
						this._downloadCount,
						this._fileName,
						this._retryCount++,
						5
					});
					this.Request();
				}
			}

			private bool CheckMD5()
			{
				string mD = Util.GetMD5(this._destPath);
				bool result;
				try
				{
					if (string.IsNullOrEmpty(this._chksum))
					{
						result = true;
					}
					else if (mD.Equals(this._chksum))
					{
						result = true;
					}
					else
					{
						Logger.WriteLog(string.Format("[MD5] info_File : {0} | local_File : {1}", this._chksum, mD));
						result = false;
					}
				}
				catch (Exception ex)
				{
					Logger.WriteLog(string.Format("[MD5] info_File : {0} | local_File : {1}", this._chksum, mD));
					base.OccurError(ERRORLEVEL.ERR_CHECK_CHECKSUM, ex.ToString() + " : " + this._destPath, new object[0]);
					result = false;
				}
				return result;
			}
		}

		internal class Task_DownloadFinalPatchList : Launcher.Task_DownloadFile
		{
			public Task_DownloadFinalPatchList(Launcher Owner, int patchlist_zip_size, string _szMD5)
			{
				base.Owner = Owner;
				this._fileName = "final.patchlist.zip";
				this._downloadCount = 1;
				this._downloadOrder = 1;
				this._fileSize = patchlist_zip_size;
				this.packSize = patchlist_zip_size;
				this._srcUrl = Owner.FinalPatchListUrl;
				this._destPath = Owner.FinalPatchListPath;
				this._chksum = _szMD5;
			}

			public override Launcher.Task.TaskResult StartTask()
			{
				base.Owner._status.packDownloadSize = (long)this.packSize;
				base.Owner._status.packDownloadProcessedSize = 0L;
				Logger.WriteLog(string.Format("Download start : {0}", this._srcUrl));
				base.Owner.LaunchHandler.OnStartDownloadPack(this._fileName, this._downloadOrder);
				return base.StartTask();
			}

			public override void EndTask()
			{
				Logger.WriteLog(string.Format("Download End : {0}", this._srcUrl));
				base.Owner.LaunchHandler.OnEndDownloadPack(this._fileName, this._downloadOrder);
				base.EndTask();
			}
		}

		internal class Task_DownloadNoPack : Launcher.Task_DownloadFile
		{
			public Task_DownloadNoPack(Launcher _owner, string urlPath, string destPath, int total, int index, string _szMD5, int _fileSize)
			{
				base.Owner = _owner;
				this._fileName = destPath.Substring(destPath.LastIndexOf("/") + 1);
				this._downloadCount = total;
				this._downloadOrder = index + 1;
				this._fileSize = _fileSize;
				this._srcUrl = urlPath;
				this._destPath = destPath;
				this._chksum = _szMD5;
			}

			public override Launcher.Task.TaskResult StartTask()
			{
				if (base.Owner.LaunchHandler != null)
				{
					Logger.WriteLog(string.Format("Download start : {0}", this._srcUrl));
					base.Owner.LaunchHandler.OnStartDownloadPack(this._fileName, this._downloadOrder);
				}
				return base.StartTask();
			}

			public override void EndTask()
			{
				if (base.Owner.LaunchHandler != null)
				{
					Logger.WriteLog(string.Format("Download End : {0}", this._srcUrl));
					base.Owner.LaunchHandler.OnEndDownloadPack(this._fileName, this._downloadOrder);
				}
				base.EndTask();
			}
		}

		internal class Task_DownloadPack : Launcher.Task_DownloadFile
		{
			public Task_DownloadPack(Launcher _owner, string packName, int total, int index, string _szMD5, int _fileSize, int _packSize, PACKTYPE type)
			{
				base.Owner = _owner;
				this._fileName = packName;
				this._downloadCount = total;
				this._downloadOrder = index + 1;
				this._fileSize = _fileSize;
				this.packSize = _packSize;
				float packEndVersion = Util.GetPackEndVersion(packName);
				if (this._fileName.Contains("pre"))
				{
					this._srcUrl = string.Format("{0}/prepack/{1}/{2}", base.Owner.PackURLRoot, packEndVersion, this._fileName);
				}
				else if (type == PACKTYPE.RESOURCE)
				{
					this._srcUrl = string.Format("{0}/pack/{1}/{2}", base.Owner.PackURLRoot, packEndVersion, this._fileName);
				}
				else if (type == PACKTYPE.LANG)
				{
					this._srcUrl = string.Format("{0}/langpack/{1}/{2}", base.Owner.PackURLRoot, packEndVersion, this._fileName);
				}
				else
				{
					base.OccurError(ERRORLEVEL.ERR_URLFILE, "존재하지 않는 CHECKTYPE 입니다.", new object[0]);
				}
				this._destPath = base.Owner.GetPackFileName(this._fileName);
				this._chksum = _szMD5;
			}

			public override Launcher.Task.TaskResult StartTask()
			{
				if (base.Owner.LaunchHandler != null)
				{
					if (!this._fileName.Contains(","))
					{
						base.Owner._status.packDownloadSize = (long)this.packSize;
						base.Owner._status.packDownloadProcessedSize = 0L;
					}
					Logger.WriteLog(string.Format("Download start : {0}", this._srcUrl));
					base.Owner.LaunchHandler.OnStartDownloadPack(this._fileName, this._downloadOrder);
				}
				return base.StartTask();
			}

			public override void EndTask()
			{
				if (base.Owner.LaunchHandler != null)
				{
					Logger.WriteLog(string.Format("Download End : {0}", this._srcUrl));
					base.Owner.LaunchHandler.OnEndDownloadPack(this._fileName, this._downloadOrder);
				}
				base.EndTask();
			}
		}

		internal class Task_InstallFinalPatchList : Launcher.Task
		{
			private string pack = string.Empty;

			private string zipFile = string.Empty;

			private int fileSize;

			private int PackOrder;

			private int PackCount;

			private int preTime;

			private int nowTime;

			private double interval = 0.2;

			private int unzipSize;

			private bool isInstallEnd;

			private static int bufferSize = 1048576;

			private byte[] buffer = new byte[Launcher.Task_InstallFinalPatchList.bufferSize];

			private int preProcessedSize;

			private int nowProcessedSize;

			public Task_InstallFinalPatchList(Launcher _owner, int _fileSize)
			{
				base.Owner = _owner;
				this.pack = "final.patchlist.zip";
				this.PackCount = 1;
				this.PackOrder = 1;
				this.fileSize = _fileSize;
				this.zipFile = base.Owner.FinalPatchListPath;
				this.unzipSize = _fileSize;
			}

			private void StartThread()
			{
				this.nowTime = Environment.TickCount;
				this.preTime = this.nowTime;
				base.Result = Launcher.Task.TaskResult.RUNNING;
				Thread thread = new Thread(new ThreadStart(this.InstallMain));
				thread.Start();
			}

			public override Launcher.Task.TaskResult StartTask()
			{
				base.Owner.SetTaskSize(this.fileSize, TASKTYPE.INSTALL);
				base.Owner._status.packInstallSize = (long)this.unzipSize;
				base.Owner._status.packInstallProcessedSize = 0L;
				Logger.WriteLog(string.Format("Install Start : {0}", this.zipFile));
				base.Owner.LaunchHandler.OnStartInstallPack(this.pack, this.PackOrder);
				this.StartThread();
				return base.Result;
			}

			public override Launcher.Task.TaskResult UpdateTask()
			{
				Launcher.Task.TaskResult result;
				lock (this)
				{
					this.nowTime = Environment.TickCount;
					if ((double)(this.nowTime - this.preTime) / 1000.0 >= this.interval)
					{
						string fileName = Path.GetFileName(this.zipFile);
						if (base.Owner._status.taskProcessedSize != base.Owner._status.taskSize)
						{
							base.Owner.PrintTaskProgress(fileName, TASKTYPE.INSTALL, 0);
							this.preTime = this.nowTime;
						}
						else
						{
							if (!this.isInstallEnd)
							{
								result = base.Result;
								return result;
							}
							if (base.Owner._status.taskProcessedSize == base.Owner._status.taskSize && this.isInstallEnd)
							{
								base.Owner.EndTaskProgress(fileName, TASKTYPE.INSTALL);
								base.PatchErrorLevel = ERRORLEVEL.SUCCESS;
								base.Result = Launcher.Task.TaskResult.SUCCESS;
							}
							else
							{
								base.Result = Launcher.Task.TaskResult.FAILED;
							}
						}
					}
					if (base.Owner._status.taskProcessedSize > base.Owner._status.taskSize)
					{
						base.OccurError(ERRORLEVEL.ERR_ETCERROR, string.Format("TaskSize가 비정상적입니다. [TaskSize : {0} / TaskProcessedSize : {1}]", base.Owner._status.taskSize, base.Owner._status.taskProcessedSize), new object[0]);
					}
					result = base.Result;
				}
				return result;
			}

			public override void EndTask()
			{
				base.Owner.LaunchHandler.OnEndInstallPack(this.pack, this.PackOrder);
				base.EndTask();
			}

			private void InstallMain()
			{
				this.StatusString = string.Format("[{0}/{1}] Install Pack...{2}", this.PackOrder, this.PackCount, this.pack);
				if (this.Install(this.zipFile, base.Owner.LocalRoot))
				{
					Logger.WriteLog(string.Format("Install End : {0}", this.zipFile));
					this.StatusString = string.Format("[{0}/{1}] Install Pack...Success!", this.PackOrder, this.PackCount, this.pack);
					this.RemovePack();
				}
				else
				{
					base.OccurError(ERRORLEVEL.ERR_INSTALLFAILED, string.Format("[{0}/{1}] Install Pack... Failed!({2})", this.PackOrder, this.PackCount, this.pack), new object[0]);
				}
				this.isInstallEnd = true;
			}

			public void InstallEnd(float patchVersion)
			{
				Logger.WriteLog(string.Format("Install End : {0}", this.zipFile));
				this.StatusString = string.Format("[{0}/{1}] Install Pack...Success!", this.PackOrder, this.PackCount, this.pack);
				this.RemovePack();
				this.isInstallEnd = true;
			}

			private bool CreateDirectory(string path)
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				if (!Directory.Exists(path))
				{
					for (int i = 1; i <= 3; i++)
					{
						Thread.Sleep(200);
						if (Directory.Exists(path))
						{
							break;
						}
						if (i == 3)
						{
							base.OccurError(ERRORLEVEL.ERR_INSTALLFAILED, "{0} is not created!", path);
							return false;
						}
					}
				}
				return true;
			}

			public bool UnzipFile(ZipInputStream srcStream, string destPath)
			{
				try
				{
					using (FileStream fileStream = new FileStream(destPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
					{
						this.nowProcessedSize = 0;
						int num = srcStream.Read(this.buffer, 0, Launcher.Task_InstallFinalPatchList.bufferSize);
						while (0 < num)
						{
							this.nowProcessedSize += num;
							if (this.preProcessedSize < this.nowProcessedSize)
							{
								base.Owner.AddProgressSize(num, TASKTYPE.INSTALL);
								this.preProcessedSize = this.nowProcessedSize;
							}
							fileStream.Write(this.buffer, 0, num);
							num = srcStream.Read(this.buffer, 0, Launcher.Task_InstallFinalPatchList.bufferSize);
						}
					}
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_INSTALLFAILED, ex.ToString(), new object[0]);
					return false;
				}
				this.preProcessedSize = 0;
				return true;
			}

			private bool Install(string zipFile, string outDir)
			{
				bool result;
				try
				{
					if (!Path.IsPathRooted(zipFile))
					{
						zipFile = Path.GetFullPath(zipFile);
					}
					using (ZipInputStream zipInputStream = new ZipInputStream(new FileStream(zipFile, FileMode.Open, FileAccess.Read, FileShare.Read)))
					{
						for (ZipEntry nextEntry = zipInputStream.GetNextEntry(); nextEntry != null; nextEntry = zipInputStream.GetNextEntry())
						{
							string path = nextEntry.Name.Replace('\\', '/');
							string directoryName = Path.GetDirectoryName(path);
							string fileName = Path.GetFileName(path);
							string text = Path.Combine(outDir, directoryName);
							if (!this.CreateDirectory(text))
							{
								result = false;
								return result;
							}
							int num = 3;
							bool flag = false;
							string destPath = Path.Combine(text, fileName);
							while (!flag)
							{
								if (num-- == 0)
								{
									result = false;
									return result;
								}
								flag = this.UnzipFile(zipInputStream, destPath);
							}
						}
						this.buffer = null;
						result = true;
					}
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_INSTALLFAILED, ex.ToString(), new object[0]);
					this.buffer = null;
					result = false;
				}
				return result;
			}

			private void RemovePack()
			{
				if (!Path.IsPathRooted(this.zipFile))
				{
					this.zipFile = Path.GetFullPath(this.zipFile);
				}
				FileInfo fileInfo = new FileInfo(this.zipFile);
				if (fileInfo.Exists)
				{
					fileInfo.Delete();
				}
			}
		}

		internal class Task_InstallPack : Launcher.Task
		{
			private string pack = string.Empty;

			private string zipFile = string.Empty;

			private int division;

			private int fileSize;

			private int PackOrder;

			private int PackCount;

			private bool UseThread = true;

			private int preTime;

			private int nowTime;

			private double interval = 0.2;

			private PACKTYPE type;

			private int unzipSize;

			private bool isInstallEnd;

			private static int bufferSize = 1048576;

			private byte[] buffer = new byte[Launcher.Task_InstallPack.bufferSize];

			private int preProcessedSize;

			private int nowProcessedSize;

			public Task_InstallPack(Launcher _owner, string packName, int _division, int total, int index, int _fileSize, int _unzipSize, PACKTYPE _type)
			{
				base.Owner = _owner;
				this.pack = packName;
				this.division = _division;
				this.PackCount = total;
				this.PackOrder = index + 1;
				this.fileSize = _fileSize;
				this.zipFile = base.Owner.GetPackFileName(packName);
				this.type = _type;
				this.unzipSize = _unzipSize;
			}

			private void StartThread()
			{
				this.nowTime = Environment.TickCount;
				this.preTime = this.nowTime;
				base.Result = Launcher.Task.TaskResult.RUNNING;
				Thread thread = new Thread(new ThreadStart(this.InstallMain));
				thread.Start();
			}

			public override Launcher.Task.TaskResult StartTask()
			{
				base.Owner.SetTaskSize(this.fileSize, TASKTYPE.INSTALL);
				if (base.Owner.LaunchHandler != null)
				{
					if (!this.pack.Contains(","))
					{
						base.Owner._status.packInstallSize = (long)this.unzipSize;
						base.Owner._status.packInstallProcessedSize = 0L;
					}
					Logger.WriteLog(string.Format("Install Start : {0}", this.zipFile));
					base.Owner.LaunchHandler.OnStartInstallPack(this.pack, this.PackOrder);
				}
				if (this.UseThread)
				{
				}
				this.StartThread();
				return base.Result;
			}

			public override Launcher.Task.TaskResult UpdateTask()
			{
				Launcher.Task.TaskResult result;
				lock (this)
				{
					this.nowTime = Environment.TickCount;
					if ((double)(this.nowTime - this.preTime) / 1000.0 >= this.interval)
					{
						string fileName = Path.GetFileName(this.zipFile);
						if (base.Owner._status.taskProcessedSize != base.Owner._status.taskSize)
						{
							base.Owner.PrintTaskProgress(fileName, TASKTYPE.INSTALL, 0);
							this.preTime = this.nowTime;
						}
						else
						{
							if (!this.isInstallEnd)
							{
								result = base.Result;
								return result;
							}
							if (base.Owner._status.taskProcessedSize == base.Owner._status.taskSize && this.isInstallEnd)
							{
								base.Owner.EndTaskProgress(fileName, TASKTYPE.INSTALL);
								base.PatchErrorLevel = ERRORLEVEL.SUCCESS;
								base.Result = Launcher.Task.TaskResult.SUCCESS;
							}
							else
							{
								base.Result = Launcher.Task.TaskResult.FAILED;
							}
						}
					}
					if (base.Owner._status.taskProcessedSize > base.Owner._status.taskSize)
					{
						base.OccurError(ERRORLEVEL.ERR_ETCERROR, string.Format("TaskSize가 비정상적입니다. [TaskSize : {0} / TaskProcessedSize : {1}]", base.Owner._status.taskSize, base.Owner._status.taskProcessedSize), new object[0]);
					}
					result = base.Result;
				}
				return result;
			}

			public override void EndTask()
			{
				if (base.Owner.LaunchHandler != null)
				{
					base.Owner.LaunchHandler.OnEndInstallPack(this.pack, this.PackOrder);
				}
				base.EndTask();
			}

			private void InstallMain()
			{
				this.StatusString = string.Format("[{0}/{1}] Install Pack...{2}", this.PackOrder, this.PackCount, this.pack);
				float packEndVersion = Util.GetPackEndVersion(this.pack);
				if (this.Install(packEndVersion, this.zipFile, base.Owner.LocalRoot))
				{
					string fileName = Path.GetFileName(this.zipFile);
					Logger.WriteLog(string.Format("Install End : {0}", this.zipFile));
					this.StatusString = string.Format("[{0}/{1}] Install Pack...Success!", this.PackOrder, this.PackCount, this.pack);
					if (fileName.Contains("pre") && this.division == 1)
					{
						base.Owner.SavePrepatchedVersion(packEndVersion);
						this.RemovePack();
						if (base.Owner.isCallPrepackEndFuncOnlyFirstPatch)
						{
							if (base.Owner._status.fullPackCount > 0 && Util.GetPackEndVersion(fileName) == base.Owner.PrepatchFinalVersion)
							{
								base.Owner.LaunchHandler.OnEndPrepack();
							}
						}
						else if (Util.GetPackEndVersion(fileName) == base.Owner.PrepatchFinalVersion)
						{
							base.Owner.LaunchHandler.OnEndPrepack();
						}
					}
					else if (this.division == 1)
					{
						if (this.type == PACKTYPE.RESOURCE)
						{
							base.Owner.SavePatchedVersion(packEndVersion, Util.GetPackLevel(fileName));
						}
						else if (this.type == PACKTYPE.LANG)
						{
							base.Owner.SaveLangVersion(packEndVersion, Util.GetPackLangCode(this.zipFile));
						}
						this.RemovePack();
					}
					if (!fileName.Contains("pre"))
					{
						if (this.type == PACKTYPE.RESOURCE)
						{
							base.Owner.WriteDivision(this.zipFile, this.division);
						}
					}
					if (this.PackOrder == this.PackCount)
					{
						base.Owner.SavePatchedDate();
					}
				}
				else
				{
					base.OccurError(ERRORLEVEL.ERR_INSTALLFAILED, string.Format("[{0}/{1}] Install Pack... Failed!({2})", this.PackOrder, this.PackCount, this.pack), new object[0]);
				}
				this.isInstallEnd = true;
			}

			public void InstallEnd(float patchVersion)
			{
				string fileName = Path.GetFileName(this.zipFile);
				Logger.WriteLog(string.Format("Install End : {0}", this.zipFile));
				this.StatusString = string.Format("[{0}/{1}] Install Pack...Success!", this.PackOrder, this.PackCount, this.pack);
				if (fileName.Contains("pre") && this.division == 1)
				{
					base.Owner.SavePrepatchedVersion(patchVersion);
					this.RemovePack();
					if (base.Owner.isCallPrepackEndFuncOnlyFirstPatch)
					{
						if (base.Owner._status.fullPackCount > 0 && Util.GetPackEndVersion(fileName) == base.Owner.PrepatchFinalVersion)
						{
							base.Owner.LaunchHandler.OnEndPrepack();
						}
					}
					else if (Util.GetPackEndVersion(fileName) == base.Owner.PrepatchFinalVersion)
					{
						base.Owner.LaunchHandler.OnEndPrepack();
					}
				}
				else if (this.division == 1)
				{
					if (this.type == PACKTYPE.RESOURCE)
					{
						base.Owner.SavePatchedVersion(patchVersion, Util.GetPackLevel(fileName));
					}
					else if (this.type == PACKTYPE.LANG)
					{
						base.Owner.SaveLangVersion(patchVersion, Util.GetPackLangCode(this.zipFile));
					}
					this.RemovePack();
				}
				if (!fileName.Contains("pre"))
				{
					if (this.type == PACKTYPE.RESOURCE)
					{
						base.Owner.WriteDivision(this.zipFile, this.division);
					}
				}
				if (this.PackOrder == this.PackCount)
				{
					base.Owner.SavePatchedDate();
				}
				this.isInstallEnd = true;
			}

			private bool CreateDirectory(string path)
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				if (!Directory.Exists(path))
				{
					for (int i = 1; i <= 3; i++)
					{
						Thread.Sleep(200);
						if (Directory.Exists(path))
						{
							break;
						}
						if (i == 3)
						{
							base.OccurError(ERRORLEVEL.ERR_INSTALLFAILED, "{0} is not created!", path);
							return false;
						}
					}
				}
				return true;
			}

			public bool UnzipFile(ZipInputStream srcStream, string destPath)
			{
				try
				{
					using (FileStream fileStream = new FileStream(destPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
					{
						this.nowProcessedSize = 0;
						int num = srcStream.Read(this.buffer, 0, Launcher.Task_InstallPack.bufferSize);
						while (0 < num)
						{
							this.nowProcessedSize += num;
							if (this.preProcessedSize < this.nowProcessedSize)
							{
								base.Owner.AddProgressSize(num, TASKTYPE.INSTALL);
								this.preProcessedSize = this.nowProcessedSize;
							}
							fileStream.Write(this.buffer, 0, num);
							num = srcStream.Read(this.buffer, 0, Launcher.Task_InstallPack.bufferSize);
						}
					}
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_INSTALLFAILED, ex.ToString(), new object[0]);
					return false;
				}
				this.preProcessedSize = 0;
				return true;
			}

			private bool Install(float patchVersion, string zipFile, string outDir)
			{
				bool result;
				try
				{
					if (!Path.IsPathRooted(zipFile))
					{
						zipFile = Path.GetFullPath(zipFile);
					}
					using (ZipInputStream zipInputStream = new ZipInputStream(new FileStream(zipFile, FileMode.Open, FileAccess.Read, FileShare.Read)))
					{
						for (ZipEntry nextEntry = zipInputStream.GetNextEntry(); nextEntry != null; nextEntry = zipInputStream.GetNextEntry())
						{
							string path = nextEntry.Name.Replace('\\', '/');
							string directoryName = Path.GetDirectoryName(path);
							string fileName = Path.GetFileName(path);
							string text = Path.Combine(outDir, directoryName);
							if (!this.CreateDirectory(text))
							{
								result = false;
								return result;
							}
							int num = 3;
							bool flag = false;
							string destPath = Path.Combine(text, fileName);
							while (!flag)
							{
								if (num-- == 0)
								{
									result = false;
									return result;
								}
								flag = this.UnzipFile(zipInputStream, destPath);
							}
						}
						if (this.type == PACKTYPE.RESOURCE)
						{
							base.Owner.LocalResourceVersion = patchVersion;
						}
						this.buffer = null;
						result = true;
					}
				}
				catch (Exception ex)
				{
					base.OccurError(ERRORLEVEL.ERR_INSTALLFAILED, ex.ToString(), new object[0]);
					this.buffer = null;
					result = false;
				}
				return result;
			}

			private void RemovePack()
			{
				if (!Path.IsPathRooted(this.zipFile))
				{
					this.zipFile = Path.GetFullPath(this.zipFile);
				}
				string text = Path.GetFileName(this.zipFile);
				text = text.ToLower().Replace(".zip", string.Empty);
				if (text.Contains(","))
				{
					string fileName = this.zipFile.Substring(0, this.zipFile.LastIndexOf(',')) + ".zip";
					FileInfo fileInfo = new FileInfo(fileName);
					if (fileInfo.Exists)
					{
						fileInfo.Delete();
					}
					text = text.Substring(0, text.IndexOf(',') + 1);
				}
				string directoryName = Path.GetDirectoryName(this.zipFile);
				string[] files = Directory.GetFiles(directoryName, "*.zip", SearchOption.AllDirectories);
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = array[i];
					if (Path.GetFileName(text2).Contains(text))
					{
						FileInfo fileInfo2 = new FileInfo(text2);
						fileInfo2.Delete();
					}
				}
			}
		}

		internal class Task_NoPack : Launcher.Task
		{
			private List<string> folderList = new List<string>();

			private Queue<int> versionQueue = new Queue<int>();

			private Queue<int> donwloadFileSizeQueue = new Queue<int>();

			private Queue<string> szMD5Queue = new Queue<string>();

			public override Launcher.Task.TaskResult StartTask()
			{
				string strFileName = Path.Combine(base.Owner.m_local_root, "BrokenFileList.txt");
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.Load(strFileName))
				{
					NDataSection nDataSection = nDataReader["list"];
					foreach (NDataReader.Row row in nDataSection)
					{
						this.folderList.Add(row.GetColumn(0));
						this.versionQueue.Enqueue(Convert.ToInt32(row.GetColumn(1)));
						this.donwloadFileSizeQueue.Enqueue(Convert.ToInt32(row.GetColumn(2)));
						this.szMD5Queue.Enqueue(row.GetColumn(3));
					}
				}
				Launcher.__Output(string.Format("Download Broken File... [Count : {0}] ", this.folderList.Count));
				this.RequestList();
				return Launcher.Task.TaskResult.SUCCESS;
			}

			protected void RequestList()
			{
				string urlPath = string.Empty;
				string destPath = string.Empty;
				int num = 0;
				foreach (string current in this.folderList)
				{
					int num2 = this.donwloadFileSizeQueue.Dequeue();
					urlPath = string.Format("{0}/{1}/{2}", base.Owner.m_src_pack_url_root, this.versionQueue.Dequeue(), current);
					destPath = string.Format("{0}/{1}", base.Owner.m_local_root, current);
					base.Owner.list_Tasks.AddLast(new Launcher.Task_DownloadNoPack(base.Owner, urlPath, destPath, this.folderList.Count, num++, this.szMD5Queue.Dequeue(), num2));
					base.Owner.AddZipSize(num2);
					Launcher expr_C6_cp_0 = base.Owner;
					expr_C6_cp_0._status.totalTaskCount = expr_C6_cp_0._status.totalTaskCount + 1;
				}
			}
		}

		public struct SystemInfo
		{
			public int maxPatchLevel;

			public int defaultPatchLevel;

			public float defaultVersion;

			public int timeoutLimitSec;

			public int retryTerm;

			public bool useLangpack;

			public bool usePrepack;

			public bool usecheckclientversion;

			public bool useFileVerifier;

			public bool downloadPatchlist;
		}

		public struct LangInfo
		{
			public int maxLangCode;

			public int defaultLangCode;

			public float defaultVersion;
		}

		public struct FileVerifierInfo
		{
			public bool useFileVerifier;

			public Launcher.FILEVERIFIERMODE mode;
		}

		public struct ClientInfo
		{
			public int bundleVersion;
		}

		public enum FILEVERIFIERMODE
		{
			PACK,
			FILE
		}

		public delegate void CheckPackListDelegate(Launcher _owner, string packName, int total, int index, string _szMD5, int _fileSize, int _packSize, PACKTYPE type);

		public delegate void AddTaskInstall(Launcher _owner, string packName, int _division, int total, int index, int _fileSize, int _unzipSize, PACKTYPE _type);

		public static Launcher Instance = new Launcher();

		private int m_patchLevel;

		private float m_resourceVersion;

		private float m_prepatchVersion;

		private float m_finalVersion;

		private int m_patchVersionMax;

		private Launcher.SystemInfo m_Systeminfo = default(Launcher.SystemInfo);

		private Launcher.LangInfo m_Langinfo = default(Launcher.LangInfo);

		private Launcher.FileVerifierInfo m_FileVerifierinfo = default(Launcher.FileVerifierInfo);

		private string m_src_url_origin_root = string.Empty;

		private string m_src_pack_url_root = string.Empty;

		public string m_local_root = string.Empty;

		public LinkedList<Launcher.Task> list_Tasks = new LinkedList<Launcher.Task>();

		private LinkedListNode<Launcher.Task> TaskNode;

		private List<int> langCodeList = new List<int>();

		private List<string> verifyFolderList = new List<string>();

		private NDataReader VerifierConfigDR;

		private NPATCHSTAGE m_stage;

		public bool isCallPrepackEndFuncOnlyFirstPatch;

		public bool isNeedPrepatch;

		public Status _status = default(Status);

		private long _onProcessCount;

		private long _onProcessAtMainThread;

		private bool m_isRunning;

		private string m_apkVersion = string.Empty;

		private string m_infoFolderName = string.Empty;

		public int PatchLevelMax
		{
			get
			{
				return this.m_Systeminfo.maxPatchLevel;
			}
			set
			{
				this.m_Systeminfo.maxPatchLevel = value;
			}
		}

		public int LocalPatchLevel
		{
			get
			{
				return this.m_patchLevel;
			}
			set
			{
				this.m_patchLevel = value;
			}
		}

		public bool IsFullPatchLevel
		{
			get
			{
				return this.LocalPatchLevel == this.m_Systeminfo.maxPatchLevel;
			}
		}

		public float LocalPrepatchVersion
		{
			get
			{
				return this.m_prepatchVersion;
			}
			set
			{
				this.m_prepatchVersion = value;
			}
		}

		public float DefaultResourceVersion
		{
			get
			{
				return this.m_Systeminfo.defaultVersion;
			}
		}

		public float LocalResourceVersion
		{
			get
			{
				return this.m_resourceVersion;
			}
			set
			{
				this.m_resourceVersion = value;
			}
		}

		public int LangCodeMax
		{
			get
			{
				return this.m_Langinfo.maxLangCode;
			}
			set
			{
				this.m_Langinfo.maxLangCode = value;
			}
		}

		public float DefaultLangVersion
		{
			get
			{
				return this.m_Langinfo.defaultVersion;
			}
		}

		public string LocalRoot
		{
			get
			{
				return this.m_local_root;
			}
			set
			{
				this.m_local_root = value;
			}
		}

		public string PackURLRoot
		{
			get
			{
				return this.m_src_pack_url_root;
			}
			set
			{
				this.m_src_pack_url_root = value;
			}
		}

		public string PatchErrorString
		{
			get;
			set;
		}

		public ERRORLEVEL PatchErrorLevel
		{
			get;
			set;
		}

		public NPATCHSTAGE Stage
		{
			get
			{
				return this.m_stage;
			}
			set
			{
				this.m_stage = value;
			}
		}

		public bool IsRunning
		{
			get
			{
				return this.m_isRunning;
			}
		}

		public LauncherHandler LaunchHandler
		{
			get;
			set;
		}

		public float ResourceFinalVersion
		{
			get
			{
				return this.m_finalVersion;
			}
			set
			{
				if ((int)value > this.m_patchVersionMax)
				{
					if (this.m_patchVersionMax == 0)
					{
						this.m_finalVersion = this.m_resourceVersion;
					}
					else
					{
						this.m_finalVersion = (float)this.m_patchVersionMax;
					}
				}
				else
				{
					this.m_finalVersion = value;
				}
			}
		}

		public float LangFinalVersion
		{
			get;
			set;
		}

		public float PrepatchFinalVersion
		{
			get;
			set;
		}

		public string FILENAME_PATCHLEVEL
		{
			get
			{
				return "PatchLevel.ini";
			}
		}

		public string FILENAME_PATCHED_VERSION
		{
			get
			{
				return string.Format("PatchedVersion.{0}.txt", this.LocalPatchLevel);
			}
		}

		public string FILENAME_USELANGCODE
		{
			get
			{
				return "UseLangCode.ini";
			}
		}

		public string FILENAME_PREPATCHED_VERSION
		{
			get
			{
				return "PrepatchedVersion.txt";
			}
		}

		public string APKVersion
		{
			get
			{
				return this.m_apkVersion;
			}
			set
			{
				this.m_apkVersion = value;
			}
		}

		public string infoFolderName
		{
			get
			{
				return this.m_infoFolderName;
			}
			set
			{
				this.m_infoFolderName = value;
			}
		}

		public string FinalPatchListPath
		{
			get
			{
				string result = string.Empty;
				if (!string.IsNullOrEmpty(this.APKVersion))
				{
					result = string.Format("{0}/{1}/{2}_{3}/{4}", new object[]
					{
						this.LocalRoot,
						"pack",
						this.infoFolderName,
						this.APKVersion,
						"final.patchlist.zip"
					});
				}
				else
				{
					result = string.Format("{0}/{1}/{2}/{3}", new object[]
					{
						this.LocalRoot,
						"pack",
						this.infoFolderName,
						"final.patchlist.zip"
					});
				}
				return result;
			}
		}

		public string FinalPatchListUrl
		{
			get
			{
				string uriString = string.Empty;
				if (!string.IsNullOrEmpty(this.APKVersion))
				{
					uriString = string.Format("{0}/{1}/{2}_{3}/{4}", new object[]
					{
						this.m_src_url_origin_root,
						"pack",
						this.infoFolderName,
						this.APKVersion,
						"final.patchlist.zip"
					});
				}
				else
				{
					uriString = string.Format("{0}/{1}/{2}/{3}", new object[]
					{
						this.m_src_url_origin_root,
						"pack",
						this.infoFolderName,
						"final.patchlist.zip"
					});
				}
				Uri uri = new Uri(uriString);
				return uri.ToString();
			}
		}

		public Launcher()
		{
			this._status.Clear();
			this.PatchErrorString = string.Empty;
			this.PatchErrorLevel = ERRORLEVEL.SUCCESS;
			this.LocalPatchLevel = 0;
			this.LocalResourceVersion = 0f;
		}

		public bool FileVerifier()
		{
			bool flag = true;
			bool result;
			try
			{
				Launcher.__Output("Check File Version ...");
				string requestUriString = string.Empty;
				if (!string.IsNullOrEmpty(this.m_apkVersion))
				{
					requestUriString = string.Format("{0}/pack/{1}/{2}/final.patchlist.txt", this.m_src_url_origin_root, this.m_infoFolderName, this.m_apkVersion);
				}
				else
				{
					requestUriString = string.Format("{0}/pack/{1}/final.patchlist.txt", this.m_src_url_origin_root, this.m_infoFolderName);
				}
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.LoadFrom(responseStream, Encoding.UTF8))
				{
					if (nDataReader.BeginSection("[FinalList2]"))
					{
						bool flag2 = false;
						string text = string.Empty;
						string text2 = string.Empty;
						string text3 = string.Empty;
						string text4 = string.Empty;
						string text5 = string.Empty;
						string value = string.Empty;
						string value2 = string.Empty;
						float num = this.m_resourceVersion;
						List<string> list = new List<string>();
						foreach (NDataReader.Row row in nDataReader)
						{
							if (row.GetColumn(0).ToLower().Contains("?/"))
							{
								flag2 = false;
								value = string.Empty;
								value2 = string.Empty;
								text5 = row.GetColumn(0).ToLower() + "/";
								foreach (string current in this.verifyFolderList)
								{
									text4 = current.Replace('\\', '/');
									if (text4.Equals("*"))
									{
										flag2 = true;
										text = text5.Substring(2, text5.Length - 2);
										break;
									}
									if (text4.Substring(text4.Length - 1).Equals("*"))
									{
										if (text5.Length - 1 >= text4.Length)
										{
											if (text4.Substring(0, text4.Length - 1).Equals(text5.Substring(2, text4.Length - 1)))
											{
												flag2 = true;
												text = text5.Substring(2, text5.Length - 2);
												break;
											}
										}
									}
									else if (text4.Contains("*"))
									{
										if (text5.LastIndexOf("/") >= text4.LastIndexOf("/"))
										{
											if (text4.Substring(0, text4.LastIndexOf("/") + 1).Equals(text5.Substring(2, text5.LastIndexOf("/") + 1 - 2)))
											{
												flag2 = true;
												text = text5.Substring(2, text5.Length - 2);
												value = text4.Substring(text4.LastIndexOf("."));
												break;
											}
										}
									}
									else
									{
										if (!text4.Contains("/") && text5.Equals("?//"))
										{
											flag2 = true;
											text = "/";
											value2 = text4;
											break;
										}
										if (text5.LastIndexOf("/") >= text4.LastIndexOf("/"))
										{
											if (text4.Substring(0, text4.LastIndexOf("/") + 1).Equals(text5.Substring(2, text5.LastIndexOf("/") + 1 - 2)))
											{
												flag2 = true;
												text = text5.Substring(2, text5.Length - 2);
												value2 = text4.Substring(text4.LastIndexOf("/") + 1);
												break;
											}
										}
									}
								}
							}
							else if (flag2 && this.LocalPatchLevel >= Convert.ToInt32(row.GetColumn(5)))
							{
								if (text.Equals("/"))
								{
									text2 = string.Format("{0}{1}", this.m_local_root, row.GetColumn(0));
								}
								else
								{
									text2 = string.Format("{0}{1}{2}", this.m_local_root, text, row.GetColumn(0));
								}
								if (!string.IsNullOrEmpty(value))
								{
									if (!text2.Substring(text2.LastIndexOf(".")).Equals(value))
									{
										continue;
									}
								}
								else if (!string.IsNullOrEmpty(value2) && !text2.Substring(text2.LastIndexOf("/") + 1).Equals(value2))
								{
									continue;
								}
								if (Convert.ToInt32(row.GetColumn(6)) >= 0)
								{
									bool flag3 = true;
									foreach (int current2 in this.langCodeList)
									{
										if (current2 == Convert.ToInt32(row.GetColumn(6)))
										{
											flag3 = false;
										}
									}
									if (flag3)
									{
										continue;
									}
								}
								if (File.Exists(text2))
								{
									text3 = Util.GetMD5(text2);
									if (!row.GetColumn(4).Equals(text3))
									{
										this.PatchErrorLevel = ERRORLEVEL.ERR_FILEVERIFY;
										this.PatchErrorString = string.Format("file is not right version! [Filename : {0}] [MD5 : {1}]", text2, text3);
										list.Add(string.Format("{0}\t{1}\t{2}\t{3}", new object[]
										{
											text2.Substring(this.m_local_root.Length),
											row.GetColumn(1),
											row.GetColumn(2),
											row.GetColumn(4)
										}));
										Logger.WriteLog(this.PatchErrorString, this.PatchErrorLevel);
										flag = false;
										if (this.m_FileVerifierinfo.mode == Launcher.FILEVERIFIERMODE.PACK)
										{
											num = this.GetRollBackVersion(row, num);
										}
									}
								}
								else
								{
									this.PatchErrorLevel = ERRORLEVEL.ERR_FILEVERIFY;
									this.PatchErrorString = string.Format("file is missing! [Filename : {0}]", text2);
									list.Add(string.Format("{0}\t{1}\t{2}\t{3}", new object[]
									{
										text2.Substring(this.m_local_root.Length),
										row.GetColumn(1),
										row.GetColumn(2),
										row.GetColumn(4)
									}));
									Logger.WriteLog(this.PatchErrorString, this.PatchErrorLevel);
									flag = false;
									if (this.m_FileVerifierinfo.mode == Launcher.FILEVERIFIERMODE.PACK)
									{
										num = this.GetRollBackVersion(row, num);
									}
								}
							}
						}
						if (flag)
						{
							Logger.WriteLog("File is All Right");
							this.RemoveBrokenFileList();
						}
						else
						{
							this.LaunchHandler.OnErrorFileVerifier(list);
							if (this.m_FileVerifierinfo.mode == Launcher.FILEVERIFIERMODE.PACK)
							{
								this.RollBackPatchedVersion(num);
							}
							else if (this.m_FileVerifierinfo.mode == Launcher.FILEVERIFIERMODE.FILE)
							{
								this.SaveBrokenFileList(list);
							}
						}
					}
					else
					{
						Launcher.__Output("NDataReader.Load Failed!" + responseStream);
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
				result = flag;
			}
			catch (Exception ex)
			{
				this.PatchErrorLevel = ERRORLEVEL.ERR_URLFILE;
				this.PatchErrorString = ex.Message;
				result = false;
			}
			return result;
		}

		public float GetRollBackVersion(NDataReader.Row row, float rollbackVersion)
		{
			string arg = string.Empty;
			if (!string.IsNullOrEmpty(this.m_apkVersion))
			{
				arg = string.Format("{0}/{1}/{2}/{3}", new object[]
				{
					this.m_src_url_origin_root,
					"pack",
					this.m_infoFolderName,
					this.m_apkVersion
				});
			}
			else
			{
				arg = string.Format("{0}/{1}/{2}", this.m_src_url_origin_root, "pack", this.m_infoFolderName);
			}
			float num;
			for (num = Convert.ToSingle(row.GetColumn(1)) - 1f; num >= this.DefaultResourceVersion; num -= 1f)
			{
				if (Util.IsVaildURL(string.Format("{0}/{1}.txt", arg, num)))
				{
					break;
				}
			}
			if (num < this.DefaultResourceVersion)
			{
				return -1f;
			}
			return (rollbackVersion >= num) ? num : rollbackVersion;
		}

		public void RollBackPatchedVersion(float version)
		{
			if (version == -1f)
			{
				return;
			}
			this.SavePatchedVersion(version, -1);
			foreach (int current in this.langCodeList)
			{
				this.SaveLangVersion(version, current);
			}
			string path = string.Format("{0}/{1}", this.m_local_root, "installdivision.txt");
			if (!Path.IsPathRooted(path))
			{
				path = Path.GetFullPath(path);
			}
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		public bool SaveBrokenFileList(List<string> list)
		{
			string path = Path.Combine(this.m_local_root, "BrokenFileList.txt");
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				stringBuilder.AppendFormat("[FieldNames]", new object[0]);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("fileName\tVersion\tsize\tCRC", new object[0]);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("[List]", new object[0]);
				stringBuilder.AppendLine();
				foreach (string current in list)
				{
					stringBuilder.AppendFormat(current, new object[0]);
					stringBuilder.AppendLine();
				}
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					streamWriter.WriteLine(stringBuilder.ToString());
				}
				result = true;
			}
			catch (ArgumentException ex)
			{
				Launcher.__Output(ex.Message);
				result = false;
			}
			catch (DirectoryNotFoundException ex2)
			{
				Launcher.__Output(ex2.Message);
				result = false;
			}
			return result;
		}

		public bool RemoveBrokenFileList()
		{
			string path = Path.Combine(this.m_local_root, "BrokenFileList.txt");
			if (!Path.IsPathRooted(path))
			{
				path = Path.GetFullPath(path);
			}
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return true;
		}

		public bool PatchStart(string root_local, string root_url, LauncherHandler _handler, bool _isCallPrepackEndFuncOnlyFirstPatch, int maxver, bool isMaster = false, string _apkversion = "")
		{
			this.DataClear();
			this.Stage = NPATCHSTAGE.READYTASK;
			this.m_local_root = root_local;
			if (!this.m_local_root.Substring(this.m_local_root.Length - 1).Equals("/") && !this.m_local_root.Substring(this.m_local_root.Length - 1).Equals("\\"))
			{
				this.m_local_root += "/";
			}
			this.m_local_root = this.m_local_root.Replace('\\', '/');
			this.m_src_url_origin_root = root_url;
			this.m_src_pack_url_root = this.m_src_url_origin_root;
			this.isCallPrepackEndFuncOnlyFirstPatch = _isCallPrepackEndFuncOnlyFirstPatch;
			this.m_apkVersion = _apkversion;
			Logger.logPath = this.m_local_root;
			this.m_patchVersionMax = maxver;
			if (isMaster)
			{
				this.m_infoFolderName = "infoqa";
			}
			else
			{
				this.m_infoFolderName = "info";
			}
			string arg = string.Empty;
			if (!Path.IsPathRooted(this.m_local_root))
			{
				arg = Path.GetFullPath(this.m_local_root);
			}
			else
			{
				arg = this.m_local_root;
			}
			string text = string.Format("{0}/NPatchLog.txt", arg);
			if (File.Exists(text))
			{
				FileInfo fileInfo = new FileInfo(text);
				if (fileInfo.Length >= 5242880L)
				{
					string destFileName = string.Format("{0}/(~{1})NPatchLog.txt", arg, DateTime.Now.ToString("yyyyMMdd_hhmmss"));
					File.Move(text, destFileName);
				}
			}
			string requestUrlString = string.Empty;
			if (!string.IsNullOrEmpty(this.m_apkVersion))
			{
				requestUrlString = string.Format("{0}/{1}/{2}/{3}/{4}", new object[]
				{
					this.m_src_url_origin_root,
					"pack",
					this.m_infoFolderName,
					this.m_apkVersion,
					"final.version.txt"
				});
			}
			else
			{
				requestUrlString = string.Format("{0}/{1}/{2}/{3}", new object[]
				{
					this.m_src_url_origin_root,
					"pack",
					this.m_infoFolderName,
					"final.version.txt"
				});
			}
			if (!Util.IsVaildURL(requestUrlString))
			{
				this.PatchErrorString = "Ready Patch Error!... Resource URL is Missing!";
				this.PatchErrorLevel = ERRORLEVEL.ERR_INPUTERROR;
				Logger.WriteLog(this.PatchErrorString, this.PatchErrorLevel);
				return false;
			}
			if (!Directory.Exists(this.m_local_root))
			{
				Directory.CreateDirectory(this.m_local_root);
			}
			if (!Directory.Exists(this.m_local_root))
			{
				this.PatchErrorString = "Ready Patch Error!... Local Directory is Missing!";
				this.PatchErrorLevel = ERRORLEVEL.ERR_INPUTERROR;
				Logger.WriteLog(this.PatchErrorString, this.PatchErrorLevel);
				return false;
			}
			if (!this.LoadSystemInfo())
			{
				this.PatchErrorLevel = ERRORLEVEL.ERR_LOADSYSTEMINFO;
				if (this.PatchErrorString.Equals(string.Empty))
				{
					this.PatchErrorString = "StartInfo File Load Error!";
				}
				return false;
			}
			if (!this.CheckLocalInfoFile(this.m_Systeminfo.defaultPatchLevel, this.m_Systeminfo.defaultVersion))
			{
				this.PatchErrorLevel = ERRORLEVEL.ERR_CHECKLOCALINFO;
				if (this.PatchErrorString.Equals(string.Empty))
				{
					this.PatchErrorString = "Local Info File Check Error!";
				}
				return false;
			}
			if (this.m_Systeminfo.useLangpack)
			{
				if (!this.LoadLangInfo())
				{
					this.PatchErrorLevel = ERRORLEVEL.ERR_LOADSYSTEMINFO;
					if (this.PatchErrorString.Equals(string.Empty))
					{
						this.PatchErrorString = "LangInfo File Load Error!";
					}
					return false;
				}
				if (!this.CheckLocalLangInfoFile(this.m_Langinfo.defaultLangCode, this.m_Langinfo.defaultVersion))
				{
					this.PatchErrorLevel = ERRORLEVEL.ERR_CHECKLOCALINFO;
					if (this.PatchErrorString.Equals(string.Empty))
					{
						this.PatchErrorString = "Local Lang Info File Check Error!";
					}
					return false;
				}
			}
			if (this.m_Systeminfo.useFileVerifier && !this.CheckFileVerifierConfig())
			{
				if (this.PatchErrorString.Equals(string.Empty))
				{
					this.PatchErrorString = "FileVerifier Info File Check Error!";
				}
				return false;
			}
			this.AddTask(root_local, root_url, _handler);
			return true;
		}

		public bool PatchUpdate()
		{
			lock (this)
			{
				if (this.TaskNode == null)
				{
					this.TaskNode = this.list_Tasks.First;
				}
				if (this.TaskNode != null)
				{
					Launcher.Task value = this.TaskNode.Value;
					value.Owner = this;
					if (value.Result == Launcher.Task.TaskResult.NONE)
					{
						value.Result = value.StartTask();
					}
					else if (value.Result == Launcher.Task.TaskResult.RUNNING)
					{
						value.Result = value.UpdateTask();
					}
					if (value.Result == Launcher.Task.TaskResult.SUCCESS)
					{
						value.EndTask();
						this.TaskNode = this.TaskNode.Next;
						GC.Collect();
						GC.WaitForPendingFinalizers();
					}
					else if (value.Result == Launcher.Task.TaskResult.FAILED)
					{
						this.TaskNode = null;
						value.EndTask();
						Launcher.__Output(value.ErrorString);
						this.PatchErrorString = value.ErrorString;
						this.PatchErrorLevel = value.PatchErrorLevel;
						this.Stage = NPATCHSTAGE.ENDALLTASK;
						bool result = false;
						return result;
					}
					if (this._onProcessCount != this._onProcessAtMainThread)
					{
						this.LaunchHandler.OnProcessAtMainThread(this._status);
						this._onProcessAtMainThread = this._onProcessCount;
					}
					if (value.Result == Launcher.Task.TaskResult.DONE)
					{
						this.PatchErrorLevel = value.PatchErrorLevel;
						this.TaskNode = null;
						this.Stage = NPATCHSTAGE.ENDALLTASK;
						bool result = false;
						return result;
					}
				}
			}
			if (this.TaskNode == null)
			{
				this.m_isRunning = false;
				this.Stage = NPATCHSTAGE.ENDALLTASK;
				return false;
			}
			return true;
		}

		public bool PatchFinish()
		{
			bool flag = true;
			bool result;
			if (this.PatchErrorString == string.Empty)
			{
				if (this.m_FileVerifierinfo.useFileVerifier)
				{
					flag = (this.LoadFileVerifierConfig() && this.FileVerifier());
				}
				if (flag)
				{
					Launcher.__Output("Patch OK...!");
					result = true;
				}
				else
				{
					Launcher.__Output("File Version is different...! " + this.PatchErrorString);
					result = false;
				}
			}
			else
			{
				Launcher.__Output("Patch Failed...! " + this.PatchErrorString);
				result = false;
			}
			if (this.LaunchHandler != null)
			{
				this.LaunchHandler.OnFinish(this.PatchErrorLevel, this.PatchErrorString);
			}
			this.Stage = NPATCHSTAGE.ENDNPATCH;
			return result;
		}

		public void DataClear()
		{
			this._status.Clear();
			this.langCodeList.Clear();
			this.list_Tasks.Clear();
			this.verifyFolderList.Clear();
			this.m_isRunning = false;
			this.PatchErrorLevel = ERRORLEVEL.SUCCESS;
			this.PatchErrorString = string.Empty;
			this.Stage = NPATCHSTAGE.DEFAULT;
			this.TaskNode = null;
			this.VerifierConfigDR = null;
			this._onProcessCount = 0L;
			this._onProcessAtMainThread = 0L;
			this.m_apkVersion = string.Empty;
			this.m_infoFolderName = string.Empty;
		}

		public bool PatchOnce(string root_local, string root_url, LauncherHandler _handler = null)
		{
			if (_handler != null)
			{
				this.LaunchHandler = _handler;
			}
			if (!this.PatchStart(root_local, root_url, _handler, false, 0, false, string.Empty))
			{
				return this.PatchFinish();
			}
			while (this.PatchUpdate())
			{
			}
			return this.PatchFinish();
		}

		public static void __Output(string msg)
		{
			Launcher.__Output(msg, null);
		}

		public static void __Output(string msg, LauncherHandler _handler)
		{
			if (msg == null)
			{
				return;
			}
		}

		private void AddTask(string local_root, string url_root, LauncherHandler _handler = null)
		{
			if (_handler == null)
			{
				this.LaunchHandler = new LauncherHandler();
			}
			else
			{
				this.LaunchHandler = _handler;
			}
			this.list_Tasks.Clear();
			if (this.m_Systeminfo.usecheckclientversion)
			{
				this.list_Tasks.AddLast(new Launcher.Task_CheckFinalClientVersion());
			}
			string path = Path.Combine(this.m_local_root, "brokenFileList.txt");
			if (!Path.IsPathRooted(path))
			{
				path = Path.GetFullPath(path);
			}
			if (File.Exists(path))
			{
				this.list_Tasks.AddLast(new Launcher.Task_NoPack());
			}
			if (this.m_Systeminfo.usePrepack)
			{
				this.list_Tasks.AddLast(new Launcher.Task_CheckFinalVersion(PACKTYPE.PREPATCH));
			}
			this.list_Tasks.AddLast(new Launcher.Task_CheckFinalVersion(PACKTYPE.RESOURCE));
			this.list_Tasks.AddLast(new Launcher.Task_CheckPackList());
			if (this.m_Systeminfo.useLangpack)
			{
				this.list_Tasks.AddLast(new Launcher.Task_CheckFinalVersion(PACKTYPE.LANG));
				this.list_Tasks.AddLast(new Launcher.Task_CheckLangPackList());
			}
			this.TaskNode = this.list_Tasks.First;
			this.Stage = NPATCHSTAGE.RUNNINGTASK;
			this.m_isRunning = true;
		}

		private bool LoadSystemInfo()
		{
			string requestUriString = string.Empty;
			if (!string.IsNullOrEmpty(this.m_apkVersion))
			{
				requestUriString = string.Format("{0}/{1}/{2}/{3}/{4}", new object[]
				{
					this.m_src_url_origin_root,
					"pack",
					this.m_infoFolderName,
					this.m_apkVersion,
					"systeminfo.txt"
				});
			}
			else
			{
				requestUriString = string.Format("{0}/{1}/{2}/{3}", new object[]
				{
					this.m_src_url_origin_root,
					"pack",
					this.m_infoFolderName,
					"systeminfo.txt"
				});
			}
			bool result;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.LoadFrom(responseStream, Encoding.UTF8))
				{
					this.m_Systeminfo.maxPatchLevel = nDataReader["SystemInfo"]["maxPatchLevel"];
					this.m_Systeminfo.defaultPatchLevel = nDataReader["SystemInfo"]["defaultPatchLevel"];
					this.m_Systeminfo.timeoutLimitSec = nDataReader["SystemInfo"]["timeoutLimitSec"];
					this.m_Systeminfo.retryTerm = nDataReader["SystemInfo"]["retryTerm"];
					NDataSection nDataSection = nDataReader["systemInfo"];
					if (nDataSection.SectionString.ToLower().Contains("fullversion"))
					{
						this.m_Systeminfo.defaultVersion = nDataReader["SystemInfo"]["fullversion"];
						this.m_Systeminfo.defaultVersion = this.m_Systeminfo.defaultVersion - 1f;
					}
					else
					{
						this.m_Systeminfo.defaultVersion = nDataReader["SystemInfo"]["defaultVersion"];
					}
					this.m_Systeminfo.useLangpack = false;
					this.m_Systeminfo.usePrepack = false;
					this.m_Systeminfo.usecheckclientversion = false;
					this.m_Systeminfo.useFileVerifier = false;
					this.m_Systeminfo.downloadPatchlist = false;
					if (nDataReader.BeginSection("option"))
					{
						NDataSection nDataSection2 = nDataReader["option"];
						if (nDataSection2.SectionString.ToLower().Contains("uselangpack"))
						{
							this.m_Systeminfo.useLangpack = nDataSection2["uselangpack"];
						}
						if (nDataSection2.SectionString.ToLower().Contains("useprepack"))
						{
							this.m_Systeminfo.usePrepack = nDataSection2["usePrepack"];
						}
						if (nDataSection2.SectionString.ToLower().Contains("usecheckclientversion"))
						{
							this.m_Systeminfo.usecheckclientversion = nDataSection2["usecheckclientversion"];
						}
						if (nDataSection2.SectionString.ToLower().Contains("usefileverifier"))
						{
							this.m_Systeminfo.useFileVerifier = nDataSection2["useFileVerifier"];
						}
						if (nDataSection2.SectionString.ToLower().Contains("downloadpatchlist"))
						{
							this.m_Systeminfo.downloadPatchlist = nDataSection2["downloadpatchlist"];
						}
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog(string.Format("SystemInfo : SystemInfo 로드 실패!!", new object[0]));
				Logger.WriteLog(ex.Message);
				this.PatchErrorString = ex.Message;
				this.PatchErrorLevel = ERRORLEVEL.ERR_LOADSYSTEMINFO;
				result = false;
			}
			return result;
		}

		private bool LoadLangInfo()
		{
			string requestUriString = string.Empty;
			if (!string.IsNullOrEmpty(this.m_apkVersion))
			{
				requestUriString = string.Format("{0}/{1}/{2}/{3}/{4}", new object[]
				{
					this.m_src_url_origin_root,
					"langpack",
					this.m_infoFolderName,
					this.m_apkVersion,
					"langinfo.txt"
				});
			}
			else
			{
				requestUriString = string.Format("{0}/{1}/{2}/{3}", new object[]
				{
					this.m_src_url_origin_root,
					"langpack",
					this.m_infoFolderName,
					"langinfo.txt"
				});
			}
			bool result;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.LoadFrom(responseStream, Encoding.UTF8))
				{
					this.m_Langinfo.maxLangCode = nDataReader["LangInfo"]["maxLangCode"];
					this.m_Langinfo.defaultLangCode = nDataReader["LangInfo"]["defaultLangCode"];
					this.m_Langinfo.defaultVersion = nDataReader["LangInfo"]["defaultVersion"];
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog(string.Format("LangInfo : LangInfo 로드 실패!!", new object[0]));
				Logger.WriteLog(ex.Message);
				result = false;
			}
			return result;
		}

		private bool CheckFileVerifierConfig()
		{
			string requestUriString = string.Empty;
			if (!string.IsNullOrEmpty(this.m_apkVersion))
			{
				requestUriString = string.Format("{0}/{1}/{2}/{3}/{4}", new object[]
				{
					this.m_src_url_origin_root,
					"pack",
					this.m_infoFolderName,
					this.m_apkVersion,
					"fileverifier.txt"
				});
			}
			else
			{
				requestUriString = string.Format("{0}/{1}/{2}/{3}", new object[]
				{
					this.m_src_url_origin_root,
					"pack",
					this.m_infoFolderName,
					"fileverifier.txt"
				});
			}
			bool result;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				this.VerifierConfigDR = new NDataReader();
				if (this.VerifierConfigDR.LoadFrom(responseStream, Encoding.UTF8))
				{
					string text = this.VerifierConfigDR["FileVerifier"]["use"].ToLower();
					if (text.Equals("true"))
					{
						this.m_FileVerifierinfo.useFileVerifier = true;
					}
					else
					{
						this.m_FileVerifierinfo.useFileVerifier = false;
					}
					string text2 = this.VerifierConfigDR["FileVerifier"]["mode"].ToLower();
					if (text2.Equals("file"))
					{
						this.m_FileVerifierinfo.mode = Launcher.FILEVERIFIERMODE.FILE;
					}
					else if (text2.Equals("pack"))
					{
						this.m_FileVerifierinfo.mode = Launcher.FILEVERIFIERMODE.PACK;
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog("fileverifier.txt 파일 로드 실패!!");
				Logger.WriteLog(ex.Message);
				result = false;
			}
			return result;
		}

		private bool LoadFileVerifierConfig()
		{
			bool result;
			try
			{
				if (this.VerifierConfigDR != null)
				{
					NDataSection nDataSection = this.VerifierConfigDR["TargetFiles"];
					foreach (NDataReader.Row row in nDataSection)
					{
						this.verifyFolderList.Add(row.GetColumn(0).ToLower());
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog(string.Format("FileVerifierConfig : FileVerifierConfig 로드 실패!!", new object[0]));
				Logger.WriteLog(ex.Message);
				result = false;
			}
			return result;
		}

		private bool CheckLocalInfoFile(int defaultPatchLevel, float defaultVersion)
		{
			bool flag = false;
			string path = Path.Combine(this.m_local_root, this.FILENAME_PATCHLEVEL);
			if (!Path.IsPathRooted(path))
			{
				path = Path.GetFullPath(path);
			}
			if (!File.Exists(path))
			{
				this.LocalPatchLevel = defaultPatchLevel;
				if (!this.SavePatchLevel())
				{
					Logger.WriteLog(string.Format("초기화 : 패치레벨 기록 실패!! [defaultPatchLevel : {0}]", defaultPatchLevel));
					return false;
				}
			}
			else
			{
				this.ReadLocalPatchLevel();
			}
			if (this.m_Systeminfo.usePrepack)
			{
				string path2 = Path.Combine(this.m_local_root, this.FILENAME_PREPATCHED_VERSION);
				if (!Path.IsPathRooted(path2))
				{
					path2 = Path.GetFullPath(path2);
				}
				if (!File.Exists(path2))
				{
					if (!this.SavePrepatchedVersion(defaultVersion))
					{
						Logger.WriteLog(string.Format("초기화 : 프리 패치버전 기록 실패!! [defaultVersion : {0}]", defaultVersion));
						return false;
					}
				}
				else
				{
					float num = this.ReadLocalPrepatchedVersion();
					if (num < defaultVersion)
					{
						if (!this.SavePrepatchedVersion(defaultVersion))
						{
							Logger.WriteLog(string.Format("초기화 : 프리 패치버전 기록 실패!! [defaultVersion : {0}]", defaultVersion));
							return false;
						}
						Logger.WriteLog(string.Format("초기화 : {0} 파일을 생성했습니다.", num));
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.AppendFormat("[Local]", new object[0]);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("PatchedVersion = {0}", defaultVersion);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("Modified = {0}", DateTime.Now);
			stringBuilder.AppendLine();
			byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
			for (int i = this.m_Systeminfo.maxPatchLevel; i >= 0; i--)
			{
				string text = Path.Combine(this.m_local_root, string.Format("PatchedVersion.{0}.txt", i));
				if (!File.Exists(text))
				{
					flag = true;
					using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
					{
						fileStream.Write(bytes, 0, bytes.Length);
					}
					Logger.WriteLog(string.Format("초기화 : {0} 파일을 생성했습니다.", text));
				}
				else
				{
					float num2 = this.ReadLocalPatchedVersion();
					if (num2 < defaultVersion)
					{
						flag = true;
						using (FileStream fileStream2 = new FileStream(text, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
						{
							fileStream2.Write(bytes, 0, bytes.Length);
						}
						Logger.WriteLog(string.Format("초기화 : {0} 파일을 생성했습니다.", text));
					}
				}
			}
			if (flag)
			{
				float num3 = this.ReadLocalPatchedVersion();
				if (num3 != defaultVersion)
				{
					Logger.WriteLog(string.Format("초기화 : 버전세팅 오류. [defaultVersion : {0}, verifyVersion : {1}", defaultVersion, num3));
					return false;
				}
			}
			return true;
		}

		private bool CheckLocalLangInfoFile(int defaultLangCode, float defaultVersion)
		{
			bool flag = false;
			string path = Path.Combine(this.m_local_root, this.FILENAME_USELANGCODE);
			if (!Path.IsPathRooted(path))
			{
				path = Path.GetFullPath(path);
			}
			if (!File.Exists(path))
			{
				if (!this.SaveUseLangCode(defaultLangCode))
				{
					Logger.WriteLog(string.Format("초기화 : 사용중인 언어코드 리스트 기록 실패!! [defaultPatchLevel : {0}]", defaultLangCode));
					return false;
				}
				if (!this.SaveLastUsedLangCode(defaultLangCode))
				{
					Logger.WriteLog(string.Format("초기화 : 마지막으로 사용한 언어코드 기록 실패!! [defaultPatchLevel : {0}]", defaultLangCode));
					return false;
				}
			}
			this.langCodeList = this.GetAllLangCode();
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.AppendFormat("[Local]", new object[0]);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("LangVersion = {0}", defaultVersion);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("Modified = {0}", DateTime.Now);
			stringBuilder.AppendLine();
			for (int i = this.m_Langinfo.maxLangCode; i >= 0; i--)
			{
				string text = Path.Combine(this.m_local_root, string.Format("PatchedLangVersion.{0}.txt", i));
				if (!File.Exists(text))
				{
					flag = true;
					using (StreamWriter streamWriter = new StreamWriter(text))
					{
						streamWriter.WriteLine(stringBuilder.ToString());
					}
					Logger.WriteLog(string.Format("초기화 : {0} 파일을 생성했습니다.", text));
				}
				else
				{
					float num = this.ReadLocalLangVersion(i);
					if (num < defaultVersion)
					{
						flag = true;
						using (StreamWriter streamWriter2 = new StreamWriter(text))
						{
							streamWriter2.WriteLine(stringBuilder.ToString());
						}
						Logger.WriteLog(string.Format("초기화 : {0} 파일을 생성했습니다.", text));
					}
				}
			}
			if (flag)
			{
				float num2 = this.ReadLocalLangVersion(defaultLangCode);
				if (num2 != defaultVersion)
				{
					Logger.WriteLog(string.Format("초기화 : 언어버전세팅 오류. [defaultVersion : {0}, verifyVersion : {1}", defaultVersion, num2));
					return false;
				}
			}
			return true;
		}

		private float ReadLocalPrepatchedVersion()
		{
			string text = Path.Combine(this.m_local_root, this.FILENAME_PREPATCHED_VERSION);
			if (!Path.IsPathRooted(text))
			{
				text = Path.GetFullPath(text);
			}
			float result;
			try
			{
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.Load(text))
				{
					this.LocalPrepatchVersion = nDataReader["Local"]["PrepatchedVersion"];
					result = this.LocalPrepatchVersion;
				}
				else
				{
					this.PatchErrorString = "PrepatchedVersion.txt is Missing!";
					result = -1f;
				}
			}
			catch (Exception ex)
			{
				this.PatchErrorString = ex.Message;
				this.PatchErrorLevel = ERRORLEVEL.ERR_LOCALFILEINFO;
				result = -1f;
			}
			return result;
		}

		private int ReadLocalPatchLevel()
		{
			string text = Path.Combine(this.m_local_root, this.FILENAME_PATCHLEVEL);
			if (!Path.IsPathRooted(text))
			{
				text = Path.GetFullPath(text);
			}
			int result;
			try
			{
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.Load(text))
				{
					int localPatchLevel = nDataReader["Local"]["PatchLevel"];
					this.LocalPatchLevel = localPatchLevel;
					result = this.LocalPatchLevel;
				}
				else
				{
					this.PatchErrorString = "PatchLevel.ini is Missing!";
					result = -1;
				}
			}
			catch (Exception ex)
			{
				this.PatchErrorString = ex.Message;
				this.PatchErrorLevel = ERRORLEVEL.ERR_LOCALFILEINFO;
				result = -1;
			}
			return result;
		}

		private float ReadLocalPatchedVersion()
		{
			string text = Path.Combine(this.m_local_root, this.FILENAME_PATCHED_VERSION);
			if (!Path.IsPathRooted(text))
			{
				text = Path.GetFullPath(text);
			}
			float result;
			try
			{
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.Load(text))
				{
					this.LocalResourceVersion = nDataReader["Local"]["PatchedVersion"];
					result = this.LocalResourceVersion;
				}
				else
				{
					this.PatchErrorString = "PatchedVersion.txt is Missing!";
					result = -1f;
				}
			}
			catch (Exception ex)
			{
				this.PatchErrorString = ex.Message;
				this.PatchErrorLevel = ERRORLEVEL.ERR_LOCALFILEINFO;
				result = -1f;
			}
			return result;
		}

		private float ReadLocalPatchedVersion(int patchLevel)
		{
			float num = 0f;
			string strFileName = Path.Combine(this.m_local_root, string.Format("PatchedVersion.{0}.txt", patchLevel));
			float result;
			try
			{
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.Load(strFileName))
				{
					num = nDataReader["Local"]["PatchedVersion"];
				}
				result = num;
			}
			catch (Exception ex)
			{
				this.PatchErrorString = ex.Message;
				this.PatchErrorLevel = ERRORLEVEL.ERR_LOCALFILEINFO;
				result = -1f;
			}
			return result;
		}

		public bool SavePatchLevel(int level)
		{
			this.LocalPatchLevel = level;
			return this.SavePatchLevel();
		}

		public bool SavePatchLevel()
		{
			bool result;
			try
			{
				int lastVersionFromCacheList = Util.GetLastVersionFromCacheList(this.m_local_root);
				if (lastVersionFromCacheList != -1)
				{
					this.LocalPatchLevel = this.m_Systeminfo.maxPatchLevel;
				}
				StringBuilder stringBuilder = new StringBuilder(1024);
				stringBuilder.AppendFormat("[Local]", new object[0]);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("PatchLevel = {0}", this.LocalPatchLevel);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("Modified = {0}", DateTime.Now);
				stringBuilder.AppendLine();
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				if (!Directory.Exists(this.m_local_root))
				{
					Directory.CreateDirectory(this.m_local_root);
				}
				string text = Path.Combine(this.m_local_root, this.FILENAME_PATCHLEVEL);
				using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
				{
					fileStream.Write(bytes, 0, bytes.Length);
				}
				Logger.WriteLog(string.Format("패치레벨 : {0} 파일에 기록 완료.", text));
				result = true;
			}
			catch (ArgumentException ex)
			{
				Launcher.__Output(ex.Message);
				result = false;
			}
			catch (DirectoryNotFoundException ex2)
			{
				Launcher.__Output(ex2.Message);
				result = false;
			}
			return result;
		}

		public bool SavePatchedDate()
		{
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				stringBuilder.AppendFormat("[Local]", new object[0]);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("date = {0}", DateTime.Now);
				stringBuilder.AppendLine();
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				if (!Directory.Exists(this.m_local_root))
				{
					Directory.CreateDirectory(this.m_local_root);
				}
				string text = Path.Combine(this.m_local_root, "PatchedDate.txt");
				using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
				{
					fileStream.Write(bytes, 0, bytes.Length);
				}
				Logger.WriteLog(string.Format("패치성공 : {0} 파일에 패치시간 기록 완료.", text));
				result = true;
			}
			catch (Exception ex)
			{
				Logger.WriteLog("SavePatchedDate() failed..!! -> " + ex.Message);
				result = false;
			}
			return result;
		}

		private float ReadLocalLangVersion(int code)
		{
			float num = 0f;
			string strFileName = Path.Combine(this.m_local_root, string.Format("PatchedLangVersion.{0}.txt", code));
			float result;
			try
			{
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.Load(strFileName))
				{
					num = nDataReader["Local"]["LangVersion"];
				}
				result = num;
			}
			catch (Exception ex)
			{
				this.PatchErrorString = ex.Message;
				this.PatchErrorLevel = ERRORLEVEL.ERR_LOCALFILEINFO;
				result = -1f;
			}
			return result;
		}

		private List<int> GetAllLangCode()
		{
			int item = 0;
			List<int> list = new List<int>();
			string strFileName = Path.Combine(this.m_local_root, this.FILENAME_USELANGCODE);
			List<int> result;
			try
			{
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.Load(strFileName))
				{
					NDataSection nDataSection = nDataReader["langcode"];
					foreach (NDataReader.Row row in nDataSection)
					{
						if (int.TryParse(row.ToDataString(), out item))
						{
							list.Add(item);
						}
					}
				}
				result = list;
			}
			catch (Exception ex)
			{
				this.PatchErrorString = ex.Message;
				this.PatchErrorLevel = ERRORLEVEL.ERR_LOCALFILEINFO;
				result = null;
			}
			return result;
		}

		public bool SaveUseLangCode(int code)
		{
			bool flag = true;
			foreach (int current in this.langCodeList)
			{
				if (current == code)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.langCodeList.Add(code);
			}
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				stringBuilder.AppendFormat("[LangCode]", new object[0]);
				stringBuilder.AppendLine();
				foreach (int current2 in this.langCodeList)
				{
					stringBuilder.AppendFormat("{0}", current2);
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("Modified = {0}", DateTime.Now);
				stringBuilder.AppendLine();
				if (!Directory.Exists(this.m_local_root))
				{
					Directory.CreateDirectory(this.m_local_root);
				}
				string text = Path.Combine(this.m_local_root, this.FILENAME_USELANGCODE);
				using (StreamWriter streamWriter = new StreamWriter(text))
				{
					streamWriter.WriteLine(stringBuilder.ToString());
				}
				Logger.WriteLog(string.Format("언어코드 : {0} 파일에 기록 완료.", text));
				result = true;
			}
			catch (ArgumentException ex)
			{
				Launcher.__Output(ex.Message);
				result = false;
			}
			catch (DirectoryNotFoundException ex2)
			{
				Launcher.__Output(ex2.Message);
				result = false;
			}
			return result;
		}

		public bool SavePrepatchedVersion(float newPatchedVersion)
		{
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				stringBuilder.AppendFormat("[Local]", new object[0]);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("PrepatchedVersion = {0}", newPatchedVersion);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("Modified = {0}", DateTime.Now);
				stringBuilder.AppendLine();
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				if (!Directory.Exists(this.m_local_root))
				{
					Directory.CreateDirectory(this.m_local_root);
				}
				string path = Path.Combine(this.m_local_root, "PrepatchedVersion.txt");
				using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
				{
					fileStream.Write(bytes, 0, bytes.Length);
				}
				result = true;
			}
			catch (Exception ex)
			{
				Launcher.__Output(ex.Message);
				result = false;
			}
			return result;
		}

		public bool SavePatchedVersion(float newPatchedVersion, int level)
		{
			bool result;
			try
			{
				this.m_resourceVersion = newPatchedVersion;
				StringBuilder stringBuilder = new StringBuilder(1024);
				stringBuilder.AppendFormat("[Local]", new object[0]);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("PatchedVersion = {0}", this.LocalResourceVersion);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("Modified = {0}", DateTime.Now);
				stringBuilder.AppendLine();
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				if (!Directory.Exists(this.m_local_root))
				{
					Directory.CreateDirectory(this.m_local_root);
				}
				if (level == -1)
				{
					for (int i = this.LocalPatchLevel; i >= 0; i--)
					{
						string path = Path.Combine(this.m_local_root, string.Format("PatchedVersion.{0}.txt", i));
						using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
						{
							fileStream.Write(bytes, 0, bytes.Length);
						}
					}
				}
				else
				{
					string path2 = Path.Combine(this.m_local_root, string.Format("PatchedVersion.{0}.txt", level));
					using (FileStream fileStream2 = new FileStream(path2, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
					{
						fileStream2.Write(bytes, 0, bytes.Length);
					}
				}
				result = true;
			}
			catch (ArgumentException ex)
			{
				Launcher.__Output(ex.Message);
				result = false;
			}
			catch (DirectoryNotFoundException ex2)
			{
				Launcher.__Output(ex2.Message);
				result = false;
			}
			return result;
		}

		public bool SaveLangVersion(float newLangVersion, int langcode)
		{
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				stringBuilder.AppendFormat("[Local]", new object[0]);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("LangVersion = {0}", newLangVersion);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("Modified = {0}", DateTime.Now);
				stringBuilder.AppendLine();
				if (!Directory.Exists(this.m_local_root))
				{
					Directory.CreateDirectory(this.m_local_root);
				}
				string path = Path.Combine(this.m_local_root, string.Format("PatchedLangVersion.{0}.txt", langcode));
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					streamWriter.WriteLine(stringBuilder.ToString());
				}
				result = true;
			}
			catch (ArgumentException ex)
			{
				Launcher.__Output(ex.Message);
				result = false;
			}
			catch (DirectoryNotFoundException ex2)
			{
				Launcher.__Output(ex2.Message);
				result = false;
			}
			return result;
		}

		public bool SaveLastUsedLangCode(int langcode)
		{
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				stringBuilder.AppendFormat("[Local]", new object[0]);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("LangCode = {0}", langcode);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("Modified = {0}", DateTime.Now);
				stringBuilder.AppendLine();
				if (!Directory.Exists(this.m_local_root))
				{
					Directory.CreateDirectory(this.m_local_root);
				}
				string path = Path.Combine(this.m_local_root, "LastUsedLangCode.txt");
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					streamWriter.WriteLine(stringBuilder.ToString());
				}
				result = true;
			}
			catch (ArgumentException ex)
			{
				Launcher.__Output(ex.Message);
				result = false;
			}
			catch (DirectoryNotFoundException ex2)
			{
				Launcher.__Output(ex2.Message);
				result = false;
			}
			return result;
		}

		public int ReadLastUsedLangCode()
		{
			int num = -1;
			string strFileName = Path.Combine(this.m_local_root, "LastUsedLangCode.txt");
			int result;
			try
			{
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.Load(strFileName))
				{
					num = nDataReader["Local"]["LangCode"];
				}
				result = num;
			}
			catch (Exception ex)
			{
				this.PatchErrorString = ex.Message;
				this.PatchErrorLevel = ERRORLEVEL.ERR_LOCALFILEINFO;
				result = -1;
			}
			return result;
		}

		private bool WriteDivision(string fileLocation, int division)
		{
			string text = Path.GetFileName(fileLocation);
			text = text.ToLower().Replace(".zip", string.Empty);
			if (text.Contains(","))
			{
				text = text.Substring(0, text.LastIndexOf(','));
			}
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				stringBuilder.AppendFormat("[Local]", new object[0]);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("PackTitle = {0}", text);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("Division = {0}", division);
				stringBuilder.AppendLine();
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				if (!Directory.Exists(this.m_local_root))
				{
					Directory.CreateDirectory(this.m_local_root);
				}
				string path = Path.Combine(this.m_local_root, string.Format("InstallDivision.txt", new object[0]));
				using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
				{
					fileStream.Write(bytes, 0, bytes.Length);
				}
				result = true;
			}
			catch (ArgumentException ex)
			{
				Launcher.__Output(ex.Message);
				result = false;
			}
			catch (DirectoryNotFoundException ex2)
			{
				Launcher.__Output(ex2.Message);
				result = false;
			}
			return result;
		}

		public bool IsAlreadyInstalledDivision(string fileLocation, int fileDivision)
		{
			string value = string.Empty;
			bool result;
			try
			{
				if (!File.Exists(Path.Combine(this.m_local_root, "InstallDivision.txt")))
				{
					result = false;
				}
				else
				{
					NDataReader nDataReader = new NDataReader();
					if (nDataReader.Load(Path.Combine(this.m_local_root, "InstallDivision.txt")))
					{
						value = nDataReader["Local"]["PackTitle"];
						int num = nDataReader["Local"]["Division"];
						if (Path.GetFileName(fileLocation).Contains(value))
						{
							if (num <= fileDivision)
							{
								result = true;
							}
							else
							{
								result = false;
							}
						}
						else
						{
							result = false;
						}
					}
					else
					{
						result = false;
					}
				}
			}
			catch (Exception ex)
			{
				this.PatchErrorString = ex.Message;
				this.PatchErrorLevel = ERRORLEVEL.ERR_LOCALFILEINFO;
				result = false;
			}
			return result;
		}

		public string GetPackFileName(string packName)
		{
			if (packName.Contains("pre"))
			{
				return Path.Combine(this.LocalRoot, string.Format("prepack/{0}", packName));
			}
			return Path.Combine(this.LocalRoot, string.Format("pack/{0}", packName));
		}

		public void SetTaskSize(int size, TASKTYPE type)
		{
			lock (this)
			{
				this._status.taskSize = (long)size;
				this._status.taskProcessedSize = 0L;
				this._status.taskType = type;
			}
		}

		public void AddExistZipSize(int size)
		{
			lock (this)
			{
				this._status.totalProcessedSize = this._status.totalProcessedSize + (long)size;
				this._status.totalDownloadSize = this._status.totalDownloadSize - (long)size;
				this._status.totalTaskProcessedCount = this._status.totalTaskProcessedCount + 1;
				this._status.totalProcessedPercent = (double)this._status.totalProcessedSize / (double)this._status.totalSize * 100.0;
				this._status.totalProcessedPercent = Math.Round(this._status.totalProcessedPercent, 2);
				this._status.packDownloadSize = this._status.packDownloadSize - (long)size;
			}
		}

		public void AddZipSize(int size)
		{
			lock (this)
			{
				this._status.totalDownloadSize = this._status.totalDownloadSize + (long)size;
				this._status.totalSize = this._status.totalSize + (long)size;
			}
		}

		public void AddUnzipSize(int size)
		{
			lock (this)
			{
				this._status.totalInstallSize = this._status.totalInstallSize + (long)size;
				this._status.totalSize = this._status.totalSize + (long)size;
			}
		}

		public void AddAlreadyInstalledSize(int size)
		{
			lock (this)
			{
				this._status.totalProcessedSize = this._status.totalProcessedSize + (long)size;
				this._status.totalInstallSize = this._status.totalInstallSize - (long)size;
				this._status.totalTaskProcessedCount = this._status.totalTaskProcessedCount + 1;
				this._status.totalProcessedPercent = (double)this._status.totalProcessedSize / (double)this._status.totalSize * 100.0;
				this._status.totalProcessedPercent = Math.Round(this._status.totalProcessedPercent, 2);
				this._status.packInstallSize = this._status.packInstallSize - (long)size;
			}
		}

		public void PrintTotalProgress(TASKTYPE type)
		{
			switch (type)
			{
			case TASKTYPE.DOWNLOAD:
			case TASKTYPE.INSTALL:
				this._status.totalStatus = string.Format("Patch {0}% Complete...", this._status.totalProcessedPercent);
				break;
			case TASKTYPE.RETRY:
				this._status.totalStatus = string.Format("Disconnected to Server. Retry... [ {0} / {1} ]", this._status.taskReconnectCount, this._status.taskReconnectCountMax);
				break;
			case TASKTYPE.REDOWNLOAD:
				this._status.totalStatus = string.Format("Redownload Start...", new object[0]);
				break;
			}
			this._onProcessCount += 1L;
		}

		public void AddProgressSize(int addSize, TASKTYPE type)
		{
			lock (this)
			{
				if (addSize > 0)
				{
					this._status.taskProcessedSize = this._status.taskProcessedSize + (long)addSize;
					this._status.totalProcessedSize = this._status.totalProcessedSize + (long)addSize;
				}
				if (type == TASKTYPE.DOWNLOAD)
				{
					this._status.totalDownloadProcessedSize = this._status.totalDownloadProcessedSize + (long)addSize;
					this._status.packDownloadProcessedSize = this._status.packDownloadProcessedSize + (long)addSize;
				}
				else if (type == TASKTYPE.INSTALL)
				{
					this._status.totalInstallProcessedSize = this._status.totalInstallProcessedSize + (long)addSize;
					this._status.packInstallProcessedSize = this._status.packInstallProcessedSize + (long)addSize;
				}
				this._status.totalProcessedPercent = (double)this._status.totalProcessedSize / (double)this._status.totalSize * 100.0;
				this._status.totalProcessedPercent = Math.Round(this._status.totalProcessedPercent, 2);
			}
		}

		public void PrintTaskProgress(string fileName, TASKTYPE type, int resize = 0)
		{
			if (this._status.taskSize != this._status.taskProcessedSize)
			{
				this.PrintTotalProgress(type);
				if (fileName == null)
				{
					Launcher.__Output("DownloadHandler::PrintProgressSize(string, int, TASKTYPE) | FileName Error!!");
					return;
				}
				string arg = ((double)this._status.taskProcessedSize / (double)this._status.taskSize * 100.0).ToString("N1");
				double num = 0.0;
				if (this._status.taskProcessedSize != 0L)
				{
					num = (double)resize / (double)this._status.taskProcessedSize * 100.0;
				}
				string arg2 = num.ToString("N1");
				switch (type)
				{
				case TASKTYPE.DOWNLOAD:
					this._status.taskStatus = string.Format("{0} File Downloading...({1}%) ", fileName, arg);
					break;
				case TASKTYPE.INSTALL:
					this._status.taskStatus = string.Format("{0} File Install...({1}%) ", fileName, arg);
					break;
				case TASKTYPE.RETRY:
					this._status.taskStatus = string.Format("Disconnected to Server. Retry... [ {0} / {1} ]", this._status.taskReconnectCount, this._status.taskReconnectCountMax);
					break;
				case TASKTYPE.REDOWNLOAD:
					this._status.taskStatus = string.Format("{0} File Redownload Start...({1}%)", fileName, arg2);
					break;
				default:
					Launcher.__Output("DownloadHandler::PrintProgressSize(string, int, TASKTYPE) | TaskType Error!!");
					break;
				}
			}
			this._onProcessCount += 1L;
		}

		public void EndTaskProgress(string fileName, TASKTYPE type)
		{
			this._status.totalProcessedSize = this._status.totalProcessedSize + (this._status.taskSize - this._status.taskProcessedSize);
			if (type == TASKTYPE.DOWNLOAD)
			{
				this._status.totalDownloadProcessedSize = this._status.totalDownloadProcessedSize + (this._status.taskSize - this._status.taskProcessedSize);
				this._status.packDownloadProcessedSize = this._status.packDownloadProcessedSize + (this._status.taskSize - this._status.taskProcessedSize);
			}
			else if (type == TASKTYPE.INSTALL)
			{
				this._status.totalInstallProcessedSize = this._status.totalInstallProcessedSize + (this._status.taskSize - this._status.taskProcessedSize);
				this._status.packInstallProcessedSize = this._status.packInstallProcessedSize + (this._status.taskSize - this._status.taskProcessedSize);
				if (this._status.packInstallProcessedSize == this._status.packInstallSize)
				{
					this._status.totalProcessedPackCount = this._status.totalProcessedPackCount + 1;
				}
			}
			this._status.totalProcessedPercent = (double)this._status.totalProcessedSize / (double)this._status.totalSize * 100.0;
			this._status.totalProcessedPercent = Math.Round(this._status.totalProcessedPercent, 2);
			this._status.taskProcessedSize = this._status.taskSize;
			this._status.taskReconnectCount = 0;
			if (this.LaunchHandler != null)
			{
				this.PrintTaskProgress(fileName, type, 0);
				this.PrintTotalProgress(type);
			}
			this._status.totalTaskProcessedCount = this._status.totalTaskProcessedCount + 1;
		}

		public bool IsPatchLevelMax()
		{
			return this.LocalPatchLevel == this.PatchLevelMax;
		}
	}
}
