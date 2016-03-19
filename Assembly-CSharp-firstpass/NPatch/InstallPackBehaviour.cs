using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace NPatch
{
	public class InstallPackBehaviour : MonoBehaviour
	{
		private const long YIELDCOUNT = 20L;

		private float patchVersion;

		private string zipFile;

		private string outDir;

		private Action<float> InstallEnd;

		private Action<ERRORLEVEL, string, string> OccurError;

		private Func<ZipInputStream, string, bool> UnzipFile;

		public void Install(float _patchVersion, string _zipFile, string _outDir, Action<float> _InstallEnd, Action<ERRORLEVEL, string, string> _OccurError, Func<ZipInputStream, string, bool> _UnzipFile)
		{
			this.patchVersion = _patchVersion;
			this.zipFile = _zipFile;
			this.outDir = _outDir;
			this.InstallEnd = _InstallEnd;
			this.OccurError = _OccurError;
			this.UnzipFile = _UnzipFile;
			base.StartCoroutine("InstallCoroutine");
		}

		[DebuggerHidden]
		private IEnumerator InstallCoroutine()
		{
			InstallPackBehaviour.<InstallCoroutine>c__IteratorD <InstallCoroutine>c__IteratorD = new InstallPackBehaviour.<InstallCoroutine>c__IteratorD();
			<InstallCoroutine>c__IteratorD.<>f__this = this;
			return <InstallCoroutine>c__IteratorD;
		}
	}
}
