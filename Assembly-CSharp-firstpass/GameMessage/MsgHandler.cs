using Ndoors.Framework.Message;
using System;

namespace GameMessage
{
	public static class MsgHandler
	{
		private static IMsgHandler _msgHandler;

		private static bool bErrAlarm;

		public static void SetMsgHandler(IMsgHandler msghandler)
		{
			MsgHandler._msgHandler = msghandler;
		}

		public static bool Handle(string functionName, params object[] msg)
		{
			if (MsgHandler._msgHandler != null)
			{
				return MsgHandler._msgHandler.Handle(functionName, msg);
			}
			if (!MsgHandler.bErrAlarm)
			{
				TsLog.LogError("MSGH --- Handler is null!", new object[0]);
				MsgHandler.bErrAlarm = false;
			}
			return false;
		}

		public static T HandleGet<T>(string functionName, ref T obj, params object[] msg) where T : class
		{
			if (MsgHandler._msgHandler != null)
			{
				MsgHandler._msgHandler.HandleGet<T>(functionName, ref obj, msg);
			}
			else if (!MsgHandler.bErrAlarm)
			{
				TsLog.LogError("MSGH --- Handler is null!", new object[0]);
				MsgHandler.bErrAlarm = false;
			}
			return obj;
		}

		public static T HandleReturn<T>(string functionName, params object[] msg)
		{
			if (MsgHandler._msgHandler != null)
			{
				return MsgHandler._msgHandler.HandleReturn<T>(functionName, msg);
			}
			if (!MsgHandler.bErrAlarm)
			{
				TsLog.LogError("MSGH --- Handler is null!", new object[0]);
				MsgHandler.bErrAlarm = false;
			}
			return default(T);
		}
	}
}
