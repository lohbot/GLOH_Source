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

		private Action OnProgressFile;

		private float oldProgress;

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
			NPatchDownloderAsyncBehaviour.<DownloadFileRoutine>c__IteratorE <DownloadFileRoutine>c__IteratorE = new NPatchDownloderAsyncBehaviour.<DownloadFileRoutine>c__IteratorE();
			<DownloadFileRoutine>c__IteratorE.url = url;
			<DownloadFileRoutine>c__IteratorE._OnProgressFile = _OnProgressFile;
			<DownloadFileRoutine>c__IteratorE.OnCompletedFile = OnCompletedFile;
			<DownloadFileRoutine>c__IteratorE.fileName = fileName;
			<DownloadFileRoutine>c__IteratorE.<$>url = url;
			<DownloadFileRoutine>c__IteratorE.<$>_OnProgressFile = _OnProgressFile;
			<DownloadFileRoutine>c__IteratorE.<$>OnCompletedFile = OnCompletedFile;
			<DownloadFileRoutine>c__IteratorE.<$>fileName = fileName;
			<DownloadFileRoutine>c__IteratorE.<>f__this = this;
			return <DownloadFileRoutine>c__IteratorE;
		}

		[DebuggerHidden]
		public IEnumerator DownloadStringRoutine(string url, Action<ERRORLEVEL, string> OnCompletedString)
		{
			NPatchDownloderAsyncBehaviour.<DownloadStringRoutine>c__IteratorF <DownloadStringRoutine>c__IteratorF = new NPatchDownloderAsyncBehaviour.<DownloadStringRoutine>c__IteratorF();
			<DownloadStringRoutine>c__IteratorF.url = url;
			<DownloadStringRoutine>c__IteratorF.OnCompletedString = OnCompletedString;
			<DownloadStringRoutine>c__IteratorF.<$>url = url;
			<DownloadStringRoutine>c__IteratorF.<$>OnCompletedString = OnCompletedString;
			<DownloadStringRoutine>c__IteratorF.<>f__this = this;
			return <DownloadStringRoutine>c__IteratorF;
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
