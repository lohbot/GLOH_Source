using System;

namespace TsLibs
{
	public class NoSectionException : Exception
	{
		private string strNoSectionException = string.Empty;

		public NoSectionException(TsDataReader owner, string sectionName) : base(string.Format("NoSectionException: FileName : {0}, SectionName : {1}", (!(owner.FileName != string.Empty)) ? "[DownloadString]" : owner.FileName, sectionName))
		{
			this.strNoSectionException = this.Message;
		}
	}
}
