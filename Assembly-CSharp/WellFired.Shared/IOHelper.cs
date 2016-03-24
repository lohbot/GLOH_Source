using System;
using System.IO;

namespace WellFired.Shared
{
	public class IOHelper : IIOHelper
	{
		public bool FileExists(string file)
		{
			return File.Exists(file);
		}
	}
}
