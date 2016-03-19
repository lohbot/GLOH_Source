using System;

namespace TsBundle
{
	public interface ISelfRemover
	{
		void UnloadBundle(bool clearMemory);
	}
}
