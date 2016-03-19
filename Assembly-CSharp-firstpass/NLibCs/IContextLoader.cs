using System;

namespace NLibCs
{
	public interface IContextLoader
	{
		bool Load(string path, out string context);
	}
}
