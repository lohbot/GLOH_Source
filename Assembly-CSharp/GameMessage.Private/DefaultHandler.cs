using Ndoors.Framework.Message;
using System;

namespace GameMessage.Private
{
	internal class DefaultHandler : IMsgHandler
	{
		public bool Handle(string functionName, params object[] msg)
		{
			return Dispatcher.DispatchStatic(functionName, typeof(FacadeHandler), msg);
		}

		public void HandleGet<T>(string functionName, ref T obj, params object[] msg) where T : class
		{
			obj = Dispatcher.DispatchRef<T>(null, typeof(FacadeHandler), functionName, msg);
		}

		public T HandleReturn<T>(string functionName, params object[] msg)
		{
			return Dispatcher.DispatchReturn<T>(null, typeof(FacadeHandler), functionName, msg);
		}
	}
}
