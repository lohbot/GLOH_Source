using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace NPatch
{
	internal class NPatchDownloderAsyncBehaviour : MonoBehaviour
	{
		public DownloderAsyncToWWW owner;

		public bool isDownloading;

		private WWW www;

		private bool isErrorDone;

		public float wwwProgress
		{
			get
			{
				if (this.isErrorDone)
				{
					return 0f;
				}
				return this.www.progress;
			}
		}

		private void Start()
		{
		}

		[DebuggerHidden]
		public IEnumerator DownloadFileRoutine(string url, string fileName, Action<ERRORLEVEL> OnCompletedFile, Action _OnProgressFile)
		{
			NPatchDownloderAsyncBehaviour.<DownloadFileRoutine>c__IteratorF <DownloadFileRoutine>c__IteratorF = new NPatchDownloderAsyncBehaviour.<DownloadFileRoutine>c__IteratorF();
			<DownloadFileRoutine>c__IteratorF.url = url;
			<DownloadFileRoutine>c__IteratorF.OnCompletedFile = OnCompletedFile;
			<DownloadFileRoutine>c__IteratorF.fileName = fileName;
			<DownloadFileRoutine>c__IteratorF.<$>url = url;
			<DownloadFileRoutine>c__IteratorF.<$>OnCompletedFile = OnCompletedFile;
			<DownloadFileRoutine>c__IteratorF.<$>fileName = fileName;
			<DownloadFileRoutine>c__IteratorF.<>f__this = this;
			return <DownloadFileRoutine>c__IteratorF;
		}

		[DebuggerHidden]
		public IEnumerator DownloadStringRoutine(string url, Action<ERRORLEVEL, string> OnCompletedString)
		{
			NPatchDownloderAsyncBehaviour.<DownloadStringRoutine>c__Iterator10 <DownloadStringRoutine>c__Iterator = new NPatchDownloderAsyncBehaviour.<DownloadStringRoutine>c__Iterator10();
			<DownloadStringRoutine>c__Iterator.url = url;
			<DownloadStringRoutine>c__Iterator.OnCompletedString = OnCompletedString;
			<DownloadStringRoutine>c__Iterator.<$>url = url;
			<DownloadStringRoutine>c__Iterator.<$>OnCompletedString = OnCompletedString;
			<DownloadStringRoutine>c__Iterator.<>f__this = this;
			return <DownloadStringRoutine>c__Iterator;
		}

		private bool SaveFile(string fileName, byte[] bytes)
		{
			try
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite, 1048576))
				{
					fileStream.Write(bytes, 0, bytes.Length);
					fileStream.Close();
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
				return false;
			}
			return true;
		}
	}
}
