using System;

namespace TsLibs
{
	public class NoKeyDataException : Exception
	{
		private string strNoSectionException = string.Empty;

		public NoKeyDataException(TsDataReader owner, TsDataSection section, string keyName) : base(string.Format("NoKeyDataException: FileName : {0}, SectionName : {1}, KeyName : {2}", (!(owner.FileName != string.Empty)) ? "[DownloadString]" : owner.FileName, section._sectionName, keyName))
		{
			this.strNoSectionException = this.Message;
		}
	}
}
