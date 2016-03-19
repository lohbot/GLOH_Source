using System;

namespace Ndoors.Framework.Message
{
	public interface IMsgHandler
	{
		bool Handle(string functionName, params object[] msg);

		void HandleGet<T>(string functionName, ref T obj, params object[] msg) where T : class;

		T HandleReturn<T>(string functionName, params object[] msg);
	}
}
