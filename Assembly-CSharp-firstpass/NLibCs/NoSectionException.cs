using System;
using UnityEngine;

namespace NLibCs
{
	public class NoSectionException : Exception
	{
		private string strNoSectionException = string.Empty;

		public NoSectionException(NDataReader owner, string sectionName) : base(string.Format("NoSectionException: FileName : {0}, SectionName : {1}", (!(owner.FileName != string.Empty)) ? "[DownloadString]" : owner.FileName, sectionName))
		{
			this.strNoSectionException = this.Message;
			Debug.LogError(this.strNoSectionException);
		}
	}
}
